using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duanmian2
{
    class Point
    {
        public string id;
        public double x;
        public double y;
        public double h;

        public Point(string id,double x,double y,double h)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.h = h;
        }
    }
}
