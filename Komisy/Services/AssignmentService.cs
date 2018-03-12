using Komisy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komisy.Services
{
    class AssignmentService
    {
        public void allocateCars(List<Car> cars, ref List<CarDealer> carDealers) 
        {
            foreach (CarDealer carDealer in carDealers)
            {
                carDealer.carList = new List<Car>();
            }

            foreach (Car car in cars)
            {
                List<float> results = new List<float>();
                foreach (CarDealer carDealer in carDealers)
                {
                    float result = car.beuty * carDealer.beuty + car.age * carDealer.age;
                    results.Add(result);
                }

                float max = results.Max();

                int index = results.IndexOf(max);

                CarDealer carDealerForCar = carDealers.ElementAt(index);

                carDealerForCar.carList.Add(car);

                carDealers[index] = carDealerForCar;
 
            }

        }
    }
}
