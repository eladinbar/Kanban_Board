using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Apple
    {
        public string Color { get; set; }
        public string Taste { get; set; }

        public Apple() { }

        public Apple (string color, string taste) {
            Color = color;
            Taste = taste;
        }
    }
}
