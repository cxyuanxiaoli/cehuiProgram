using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialExplore
{
    class Point
    {
        //数据点类
        public string id;
        public double x;
        public double y;
        public int area_code;

        public Point(string id,double x,double y,int code)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.area_code = code;
        }
    }
}
