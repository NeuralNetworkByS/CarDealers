using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komisy.Models
{
    public class Point
    {
        public float x1;
        public float x2;
        public int position = 0;
        public bool isUp = false;

        public Point()
        {

        }

        public  Point(float x1, float x2)
        {
            this.x1 = x1;
            this.x2 = x2;
        }

        public String toString()
        {
            return "x1: " + x1 + ", x2: " + x2 + ", position: " + position + ",is up: " + isUp;
        }
    }
}
