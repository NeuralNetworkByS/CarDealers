using Komisy.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Komisy.Services
{
    class VisualizationService
    {
        private PositionPointService ppService = new PositionPointService();

        List<Point> sortedPoints;
        List<List<Point>> boundaryPoints;

        public List<List<Point>> getBoundaryPoints(ref List<CarDealer> carDealers)
        {
            boundaryPoints = new List<List<Point>>();

            foreach (CarDealer carDealer in carDealers)
            {
              
                if (carDealer.carList.Count == 0)
                {
                    Debug.WriteLine("Pomijam dealera");
                    continue;
                }

                sortedPoints = new List<Point>();

                Debug.WriteLine("");
                Debug.WriteLine("Szukam punktów dla: " + carDealer.toString());

                foreach (CarDealer carDealerSmaller in carDealers)
                {
                    if (carDealerSmaller.carList.Count == 0)
                    {
                        Debug.WriteLine("Pomijam dealera");
                        continue;
                    }

                    if (carDealer == carDealerSmaller)
                    {
                        Debug.WriteLine("Pomijam");
                        continue;
                    }

                    float a1 = carDealer.age - carDealerSmaller.age;
                    float a2 = carDealerSmaller.beuty - carDealer.beuty;

                    Point point = new Point(a2, a1);
                    ppService.setPosition(ref point);
                    Debug.WriteLine("");
                    Debug.WriteLine("Sprawdzam punkt: " + point.toString());


                    if (checkPointIsBoundary(ref point, a1, a2))
                    {
                        Debug.WriteLine("Dodaję punkt: " + point.toString());
                        
                        sortedPoints.Add(point);
                    }

                    point = new Point(-a2, -a1);
                    ppService.setPosition(ref point);
                    Debug.WriteLine("");
                    Debug.WriteLine("Sprawdzam punkt: " + point.toString());

                    if (checkPointIsBoundary(ref point, a1, a2))
                    {
                        Debug.WriteLine("Dodaję punkt: " + point.toString());
                        
                        sortedPoints.Add(point);
                    }
                }

                Debug.WriteLine("");
                Debug.WriteLine("Punkty dla " + carDealer.toString());

              

                findTwoBoundaryPoints(carDealer, sortedPoints);
                Debug.WriteLine("");
            }

            return boundaryPoints;

        }

        private bool checkPointIsBoundary(ref Point point, float a1, float a2)
        {
            bool isBoundary = false;

            Point nextPointUp = new Point();
            Point nextPointDown = new Point();

            nextPointUp = new Point(a2, a1 + 1);
            nextPointDown = new Point(a2, a1 - 1);
            
            

            bool isUpGood = a1 * nextPointUp.x1 > a2 * nextPointUp.x2;
            bool isLowGood = a1 * nextPointDown.x1 > a2 * nextPointDown.x2;

            if (isUpGood != isLowGood)
            {
                point.isUp = isUpGood;
                isBoundary = true;
            }

            return isBoundary;
        } 

        public void findTwoBoundaryPoints(CarDealer carDealer, List<Point> points)
        {
            sortedPoints = ppService.sortPoints(points);
            List<Point> boundaryPointsForOne = new List<Point>(); 

            Debug.WriteLine("Posortowane punkty.");
            foreach (Point point in sortedPoints)
            {
                Debug.WriteLine(point.toString() + ", isUP: " + point.isUp + ", position: " + point.position);
            }

            DirectionCorrection directionCorrection = new DirectionCorrection();

            Point firstPoint = null;
            Point secondPoint = null;

            List<List<Point>> candiatesPoints = new List<List<Point>>();

            Debug.WriteLine("Ilość posortowanych punktów przed forami: " + sortedPoints.Count); 
            for (int i = 0; i < sortedPoints.Count - 1; i++)
            {
                if (sortedPoints[i].isUp != directionCorrection.firstPointFlow[sortedPoints[i].position.ToString()])
                {
                    continue;
                }

                firstPoint = sortedPoints[i];
                Debug.WriteLine("Pierwszy punkt przynany jako: " + sortedPoints[i].toString());

                for (int j = i + 1; j < sortedPoints.Count; j++)
                {
                    String key = sortedPoints[i].position.ToString() + "" + sortedPoints[j].position.ToString();

                    Debug.WriteLine("Ilość posortowanych punktów: " + sortedPoints.Count);
                    Debug.WriteLine("Indexy: {0}, {1}", i, j);
                    Debug.WriteLine("Key: " + key);

                    if (sortedPoints[j].isUp != directionCorrection.secondPointFlow[key])
                    {
                        continue;
                    }

                    Debug.WriteLine("Drugi punkt przynany jako: " + sortedPoints[j].toString());
                    secondPoint = sortedPoints[j];
                    break;
                }

                if (firstPoint != null && secondPoint != null)
                {
                    List<Point> twoPoints = new List<Point>();
                    twoPoints.Add(firstPoint);
                    twoPoints.Add(secondPoint);
                    candiatesPoints.Add(twoPoints);
                }

                secondPoint = null;

            }

            if (candiatesPoints.Count == 0)
            {
                Debug.WriteLine("Nie odnaleziono żadnych punktów.");

                bool isUpLastPoint = sortedPoints[sortedPoints.Count - 1].isUp;
                int lastPointPosition = sortedPoints[sortedPoints.Count - 1].position;

                if (directionCorrection.firstPointFlow[lastPointPosition.ToString()] == isUpLastPoint)
                {
                    bool isUpFirstPoint = sortedPoints[0].isUp;
                    int firstPointPosition = sortedPoints[0].position;

                    String position = lastPointPosition.ToString() + "" + firstPointPosition.ToString();
                    if (directionCorrection.secondPointInvertedFlow[position] == isUpFirstPoint)
                    {
                        Debug.WriteLine("Pierwszy punkt: " + sortedPoints[0].toString());
                        Debug.WriteLine("Drugi punkt: " + sortedPoints[sortedPoints.Count - 1].toString());
                        boundaryPointsForOne.Add(sortedPoints[0]);
                        boundaryPointsForOne.Add(sortedPoints[sortedPoints.Count - 1]);
                        boundaryPoints.Add(boundaryPointsForOne);
                    }
                }

            }
            else
            {
                Debug.WriteLine("Ilość kandydatów: " + candiatesPoints.Count);

                
                foreach (List<Point> listOfPoints in candiatesPoints)
                {
                    Debug.WriteLine("Kandydat");
                    foreach (Point point in listOfPoints)
                    {
                        Debug.WriteLine("Punkt: " + point.toString());
                    }
                }
                

                List<Point> pointsResult = candiatesPoints[candiatesPoints.Count - 1];
                Debug.WriteLine("Pierwszy punkt: " + pointsResult[0].toString());
                Debug.WriteLine("Drugi punkt: " + pointsResult[1].toString());
                boundaryPointsForOne.Add(pointsResult[0]);
                boundaryPointsForOne.Add(pointsResult[1]);
                boundaryPoints.Add(boundaryPointsForOne);
            }
        }
    }
}
