using Komisy.Models;
using Komisy.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Komisy
{
    public partial class Form1 : Form
    {
        private List<Car> cars = new List<Car>();
        private List<CarDealer> carDealers = new List<CarDealer>();
        private VisualizationService vService = new VisualizationService();

        private ValidationService validateService = new ValidationService();
        private AssignmentService aService = new AssignmentService();

        private float maxPointValue = 6.0f;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ResultTable.ColumnCount = 2;
            initializeCarDealers();
            InitializeCars();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void CarListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddCarDealerButton_Click(object sender, EventArgs e)
        {
            bool isDataCorrect = validateService.validateValuesForElement(
                CarDealerBeautyTB, CarDealerOldTB, CarDealerName);

            if (isDataCorrect)
            {
                float beuty = float.Parse(CarDealerBeautyTB.Text);
                float old = float.Parse(CarDealerOldTB.Text);
                updateMaxPointValue(beuty);
                updateMaxPointValue(old);
                String name = CarDealerName.Text;
                CarDealer carDealer = new CarDealer(old, beuty, name);
                carDealers.Add(carDealer);
                CarDealerListBox.Items.Add(carDealer.toString());
            }
            else
            {
                MessageBox.Show("Nieprawidłowe wartości dla komisu.");
            }

            
        }

        private void AddCarButton_Click(object sender, EventArgs e)
        {

            bool isDataCorrect = validateService.validateValuesForElement(
                CarBeutyTB, CarOldTB, CarName);

            if (isDataCorrect)
            {
                float beuty = float.Parse(CarBeutyTB.Text);
                float old = float.Parse(CarOldTB.Text);
                updateMaxPointValue(beuty);
                updateMaxPointValue(old);
                String name = CarName.Text;
                Car car = new Models.Car( old, beuty, name);
                cars.Add(car);
                CarListBox.Items.Add(car.toString());
            }
            else
            {
                MessageBox.Show("Nieprawidłowe wartości dla samochodu.");
            }

            
        }

        private void CarDealerDeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                int index = CarDealerListBox.SelectedIndex;
                CarDealerListBox.Items.RemoveAt(index);
                carDealers.RemoveAt(index);
            }
            catch (Exception a)
            {
                MessageBox.Show("Wybierz element z tablicy.");
            }
        }

        private void CarDeleteButton_Click(object sender, EventArgs e)
        {

            try
            {
                int index = CarListBox.SelectedIndex;
                CarListBox.Items.RemoveAt(index);
                cars.RemoveAt(index);
            }
            catch (Exception a)
            {
                MessageBox.Show("Wybierz element z tablicy.");
            }

        }

        private void RandCarsButtons_Click(object sender, EventArgs e)
        {
            randomCars();
        }

        private void AssignmentButton_Click(object sender, EventArgs e)
        {
            if (cars.Count == 0 || carDealers.Count == 0)
            {
                MessageBox.Show("Każda z list musi zawierać przynajmniej po jednej pozycji.");
            }
            else
            {
                ResultListBox.Items.Clear();
                aService.allocateCars(cars,ref carDealers);

                int index = 1;
                foreach (CarDealer carDealer in carDealers)
                {
                    if (carDealer.carList.Count == 0)
                    {  
                        continue;
                    }

                    ResultListBox.Items.Add(index + "." + carDealer.toString());
                    index++; 
                    
                    foreach (Car car in carDealer.carList)
                    {
                        ResultListBox.Items.Add(" --- " + car.toString());
                    }
                }

            }
        }

        private void ChartButton_Click(object sender, EventArgs e)
        {
            ChartResult chartResult = new ChartResult();
            chartResult.boundaryPoints = vService.getBoundaryPoints(ref carDealers);
            chartResult.carDealers = carDealers;
            chartResult.maxPointValue = maxPointValue;
            chartResult.Show();
        }

        public void initializeCarDealers()
        {
            CarDealer carDealer = new CarDealer(5, 5, "LadneNowe");
            carDealers.Add(carDealer);
           
            carDealer = new CarDealer(5, 1, "LadneSrWiek");
            carDealers.Add(carDealer);
            
            carDealer = new CarDealer(1, 5, "SrLadneNowe");
            carDealers.Add(carDealer);

            carDealer = new CarDealer(1, -5, "SrLadneStare");
            carDealers.Add(carDealer);

            carDealer = new CarDealer(-5, -5, "BrzydkieStare");
            carDealers.Add(carDealer);

            carDealer = new CarDealer(5, -5, "NoweBrzydkie");
            carDealers.Add(carDealer);

            carDealer = new CarDealer(-5, 5, "BrzydkieNowe");
            carDealers.Add(carDealer);

            carDealer = new CarDealer(1, 4, "MotoHania");
            carDealers.Add(carDealer);

            foreach (CarDealer carDealerE in carDealers)
            {
                CarDealerListBox.Items.Add(carDealerE.toString());
            }

        }

        public void InitializeCars()
        {
            Car car = new Car(5, 5, "Car1");
            cars.Add(car);

            car = new Car(5, 1, "Car2");
            cars.Add(car);

            car = new Car(1, 5, "Car3");
            cars.Add(car);

            car = new Car(1, -5, "Car4");
            cars.Add(car);

            car = new Car(-5, -5, "Car5");
            cars.Add(car);

            car = new Car(5, -5, "Car6");
            cars.Add(car);

            car = new Car(-5, 5, "Car7");
            cars.Add(car);

            car = new Car(1, 4, "Car8");
            cars.Add(car);

            foreach (Car car2 in cars)
            {
                CarListBox.Items.Add(car2.toString());
            }
        }

        public void updateMaxPointValue(float newValue)
        {
            if (newValue < 0)
            {
                newValue = -newValue;
            }

            if (newValue > maxPointValue)
            {
                maxPointValue = newValue;
            }
        }

        public void randomCars()
        {
            Random random = new Random();
            for (int i = 0; i < 30; i++)
            {
                float age = random.Next(-10, 11);
                float beuty = random.Next(-10, 11);
                Car car = new Car(age, beuty, "Car" + i.ToString());
                cars.Add(car);
                CarListBox.Items.Add(car.toString());
            }
        }

        public void randomCarDealers()
        {
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                float age = random.Next(-5, 6);
                float beuty = random.Next(-5, 6);
                CarDealer carDealer = new CarDealer(age, beuty, "CarDealer" + i.ToString());
                carDealers.Add(carDealer);
                CarDealerListBox.Items.Add(carDealer.toString());
            }
        }

        private void RandomCarDealerButton_Click(object sender, EventArgs e)
        {
            randomCarDealers();
        }
    }
}
