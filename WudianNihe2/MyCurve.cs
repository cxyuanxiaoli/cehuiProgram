using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WudianNihe2
{
    //曲线参数类
    class MyCurve
    {
        public double[] E;
        public double[] F;

        public MyCurve(double[] e, double[] f)
        {
            E = e;
            F = f;
        }
        public List<Point> GetInsertPoints()
        {
            int i = 1;
            List<Point> list = new List<Point>();
            for (double z = 0; z <= 1; z+=0.1)
            {
                double x = E[0] + E[1] * z + E[2] * z * z + E[3] * z * z * z;
                double y = F[0] + F[1] * z + F[2] * z * z + F[3] * z * z * z;
                list.Add(new Point((i++).ToString(), x, y));
            }
            return list;
        }
    }
}
