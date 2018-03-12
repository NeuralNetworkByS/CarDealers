using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komisy.Models
{
    public class BasicInfo
    {
        public float beuty;
        public float age;
        public String name;

        public BasicInfo( float age, float beuty, String name)
        {
            this.beuty = beuty;
            this.age = age;
            this.name = name;
        }

        public String toString()
        {
            return name + " <" + age.ToString() + ", " + beuty.ToString() + ">";
        }
    }
}
