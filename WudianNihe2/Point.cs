using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WudianNihe2
{
    class Point
    {
        public string id;
        public double x;
        public double y;
        //梯度  cos sin
        public double[] tidu;

        public Point(string id,double x,double y)
        {
            this.id = id;
            this.x = x;
            this.y = y;

        }

        /// <summary>
        /// 补充点
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p1">最边缘点</param>
        /// <param name="p2">次边缘点</param>
        /// <param name="p3">内部点</param>
        public Point(string id, Point p1,Point p2,Point p3)
        {
            this.id = id;
            //this.x = p3.x - p2.x - (p2.x - p1.x);
            //this.y = p3.y - p2.y - (p2.y - p1.y);
            x = p3.x + 3 * p1.x - 3 * p2.x;
            y = p3.y + 3 * p1.y - 3 * p2.y;
        }

        public override string ToString()
        {
            return this.id.PadRight(8) + Math.Round(x,3).ToString().PadRight(8) + Math.Round(y,3).ToString().PadRight(8);
        }
    }
}
