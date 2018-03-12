using Komisy.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komisy.Services
{
    class PositionPointService
    {
        public const int FIRST = 1;
        public const int SECOND = 2;
        public const int THIRD = 3;
        public const int FOURTH = 4;


        public void setPosition(ref Point point)
        {
            int position = 0;
            if (point.x1 >= 0 && point.x2 >= 0)
            {
                position = FOURTH;
            }
            else if (point.x1 <= 0 && point.x2 >= 0)
            {
                position = THIRD;
            }
            else if (point.x1 <= 0 && point.x2 <= 0)
            {
                position = SECOND;
            }
            else if (point.x1 >= 0 && point.x2 <= 0)
            {
                position = FIRST;
            }


            point.position = position;
        }

        public List<Point> sortPoints(List<Point> points)
        {
            Point maxPoint;
            Point maxPoint2;

            bool isMaxFounded = false;

            int indexOfMax = 0;

            for (int i = 0; i < points.Count - 1; i++)
            {
                maxPoint = points[i];
                isMaxFounded = false;
                for (int j = i + 1; j < points.Count; j++)
                {
                    maxPoint2 = comparePoints(maxPoint, points[j]);
                    if (!isEqualTwoPoints(maxPoint, maxPoint2))
                    {
                        maxPoint = maxPoint2;
                        indexOfMax = j;
                        isMaxFounded = true;
                    }
                }

                Debug.WriteLine("Zamiana.");
                Debug.WriteLine("points[{0}/indexOfMax] = {1}/points[{2}/i]", indexOfMax, points[i].toString(), i);
                Debug.WriteLine("points[{0}]/i = {1}/maxPoint", i, maxPoint.toString());


                if (isMaxFounded)
                {
                    points[indexOfMax] = points[i];
                    points[i] = maxPoint;
                }
                
            }

            return points;
        }

        public Point comparePoints(Point firstPoint, Point secondPoint)
        {
            Point biggerPoint = new Point();

            double firstPointAngular = countAngularForXY(firstPoint.x1, firstPoint.x2);
            double secondPointAngular = countAngularForXY(secondPoint.x1, secondPoint.x2);

            if (firstPoint.position == secondPoint.position)
            {
                Debug.WriteLine("Punkty mają taką samą pozycję.");
                switch (firstPoint.position)
                {
                    case FIRST:
                        Debug.WriteLine("Są w ćwiartce z indexem: 1.");
                        if (firstPointAngular > secondPointAngular)
                        {
                            biggerPoint = firstPoint;
                        }
                        else
                        {
                            biggerPoint = secondPoint;
                        }

                        break;
                    case SECOND:
                        Debug.WriteLine("Są w ćwiartce z indexem: 2.");
                        if (firstPointAngular < secondPointAngular)
                        {
                            biggerPoint = firstPoint;
                        }
                        else
                        {
                            biggerPoint = secondPoint;
                        }

                        break;
                    case THIRD:
                        Debug.WriteLine("Są w ćwiartce z indexem: 3.");
                        Debug.WriteLine("firstPoint: {0}, angular: {1}", firstPoint.toString(), firstPointAngular);
                        Debug.WriteLine("secondPoint: {0}, angular: {1}", firstPoint.toString(), secondPointAngular);


                        if (firstPointAngular > secondPointAngular)
                        {
                            biggerPoint = firstPoint;
                        }
                        else
                        {
                            biggerPoint = secondPoint;
                        }

                        Debug.WriteLine("BiggerPoint: {0}", biggerPoint.toString());

                        break;
                    case FOURTH:
                        Debug.WriteLine("Są w ćwiartce z indexem: 4.");
                        if (firstPointAngular < secondPointAngular)
                        {
                            biggerPoint = firstPoint;
                        }
                        else
                        {
                            biggerPoint = secondPoint;
                        }

                        break;

                }
            }
            else
            {
                if (firstPoint.position > secondPoint.position)
                {
                    biggerPoint = firstPoint;
                }
                else
                {
                    biggerPoint = secondPoint;
                }
            }

            return biggerPoint;
        }

        public bool isEqualTwoPoints(Point firstPoint, Point secondPoint)
        {
            bool isEqual = true;

            if (firstPoint.x1 != secondPoint.x1)
            {
                isEqual = false;
            }
            else if (firstPoint.x2 != secondPoint.x2)
            {
                isEqual = false;
            }
            
            return isEqual;
        }

        private double  countAngularForXY(float x, float y)
        {
            double angular = 0.0;

            double xD =  x;
            double yD = y;

            if (xD < 0)
            {
                xD = -xD;
            }

            if (yD < 0)
            {
                yD = -yD;
            }

            double tgBeta = yD / xD;
            angular = Math.Atan(tgBeta);

            return angular;
        }
    }

    
}
