using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komisy.Models
{
    public class CarDealer : BasicInfo
    {
        public List<Car> carList;

        public List<Point> points = new List<Point>();

        public CarDealer(float old, float beauty, string name) : base(old, beauty, name)
        {
            
        }
    }
}
