using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using Komisy.Models;

namespace Komisy
{
    public partial class ChartResult : Form
    {
        public ILPlotCube plotCube;
        public List<List<Models.Point>> boundaryPoints;
        public List<CarDealer> carDealers;
        public float maxPointValue = 0.0f;

        public ChartResult()
        {
            InitializeComponent();
        }

        // Initial plot setup, modify this as needed
        private void ilPanel1_Load(object sender, EventArgs e)
        {

            // create some test data, using our private computation module as inner class
            int[,] array2D = new int[,] { {1, 2}, {3, 4} };
            ILArray<float> Pos = array2D;

            preparePlotCube();

            // setup the plot (modify as needed)
            ilPanel1.Scene.Add(plotCube);
            // register event handler for allowing updates on right mouse click:
            /*ilPanel1.Scene.First<ILLinePlot>().MouseClick += (_s, _a) =>
            {
                if (_a.Button == MouseButtons.Right)
                    Update(ILMath.rand(3, 30));
            };*/
        }

        /// <summary>
        /// Example update function used for dynamic updates to the plot
        /// </summary>
        /// <param name="A">New data, matching the requirements of your plot</param>
        public void Update(ILInArray<double> A)
        {
            using (ILScope.Enter(A))
            {

                // fetch a reference to the plot
                var plot = ilPanel1.Scene.First<ILLinePlot>(tag: "mylineplot");
                if (plot != null)
                {
                    // make sure, to convert elements to float
                    plot.Update(ILMath.tosingle(A));
                    //
                    // ... do more manipulations here ...

                    // finished with updates? -> Call Configure() on the changes 
                    plot.Configure();

                    // cause immediate redraw of the scene
                    ilPanel1.Refresh();
                }

            }
        }

        /// <summary>
        /// Example computing module as private class 
        /// </summary>
        private class Computation : ILMath
        {
            /// <summary>
            /// Create some test data for plotting
            /// </summary>
            /// <param name="ang">end angle for a spiral</param>
            /// <param name="resolution">number of points to plot</param>
            /// <returns>3d data matrix for plotting, points in columns</returns>
            public static ILRetArray<float> CreateData(int ang, int resolution)
            {
                using (ILScope.Enter())
                {
                    ILArray<float> A = linspace<float>(0, ang * pi, resolution);
                    ILArray<float> Pos = zeros<float>(3, A.S[1]);
                    Pos["0;:"] = sin(A);
                    Pos["1;:"] = cos(A);
                    Pos["2;:"] = A;
                    return Pos;
                }
            }

        }
        
        private void ChartResult_Load(object sender, EventArgs e)
        {

        }

        public void preparePlotCube()
        {
            plotCube = new ILPlotCube(null, twoDMode: true);

            Random random = new Random();

            drawOnlyPoints(random);
            //drawPointsAndLines(random);




            Debug.WriteLine("PlotCube przygotowany");
        }

        private void drawOnlyPoints(Random random)
        {
            for (int i = 0; i < carDealers.Count; i++)
            {

                Color color = roundColor(random);

                try
                {

                    float[,] pointsForCar = new float[carDealers[i].carList.Count, 3];
                    for (int j = 0; j < carDealers[i].carList.Count; j++)
                    {
                        pointsForCar[j, 0] = carDealers[i].carList[j].age;
                        pointsForCar[j, 1] = carDealers[i].carList[j].beuty;
                        pointsForCar[j, 2] = 0;
                    }

                    ILPoints points = new ILPoints();
                    points.Positions = pointsForCar;
                    points.Color = color;

                    plotCube.Add(points);
                }
                catch (NullReferenceException e)
                {
                    Debug.WriteLine("Poleciał wyjątek brak listy samchoddów.");
                }
            }
        }

        private void drawPointsAndLines(Random random)
        {
            for (int i = 0; i < boundaryPoints.Count; i++)
            {

                Color color = roundColor(random);
                for (int j = 0; j < boundaryPoints[i].Count; j++)
                {
                    Debug.WriteLine("Rysuję punkt: " + boundaryPoints[i][j].toString());

                    Models.Point thirdPoint = findThirdPoint(new Models.Point(0, 0), new Models.Point(boundaryPoints[i][j].x1, boundaryPoints[i][j].x2), maxPointValue);
                    float[,] array2D = new float[,] { { 0, 0 }, { boundaryPoints[i][j].x1, boundaryPoints[i][j].x2 }, { thirdPoint.x1, thirdPoint.x2 } };
                    ILArray<float> Pos = array2D;
                    plotCube.Add(new ILLinePlot(Pos, tag: "mylineplot")
                    {
                        Line = {
                                    Width = 2,
                                    Color = color,
                                    Antialiasing = true,
                                    DashStyle = DashStyle.Solid
                                }
                    });
                }

                try
                {

                    float[,] pointsForCar = new float[carDealers[i].carList.Count, 3];
                    for (int j = 0; j < carDealers[i].carList.Count; j++)
                    {
                        pointsForCar[j, 0] = carDealers[i].carList[j].age;
                        pointsForCar[j, 1] = carDealers[i].carList[j].beuty;
                        pointsForCar[j, 2] = 0;
                    }

                    ILPoints points = new ILPoints();
                    points.Positions = pointsForCar;
                    points.Color = color;

                    plotCube.Add(points);
                }
                catch (NullReferenceException e)
                {
                    Debug.WriteLine("Poleciał wyjątek brak listy samchoddów.");
                }
            }

        }

        private Color roundColor(Random random)
        { 

            byte R = (byte) random.Next(0, 256);
            byte G = (byte) random.Next(0, 256);
            byte B = (byte) random.Next(0, 256);

            return Color.FromArgb(R, G, B);
        }

        public Models.Point findThirdPoint(Models.Point point1, Models.Point point2, float maxValue)
        {
            Debug.WriteLine("Wyszukiwanie punktu maksymalnego: ");
            Debug.WriteLine("Punkt1: " + point1.toString());
            Debug.WriteLine("Punkt2: " + point2.toString());

            if (point2.x1 < 0)
            {
                maxValue = -maxValue;
            }

            float y = point1.x2 - point2.x2;
            float aH = point1.x1 - point2.x1;
            float a = y / aH;
            float b = point1.x2 - a * point1.x1;

            Debug.WriteLine("a: {0}, b: {1} ", a, b);

            float x2 = a * maxValue + b;
            Debug.WriteLine("Finalny punkt: ({0}, {1}) ", maxValue, x2);

            return new Models.Point(maxValue, x2);
        }
    }
}
