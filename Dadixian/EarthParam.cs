using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dadixian
{
    class EarthParam
    {
        //1.1椭球基本参数
        public double a;
        public double b;
        public double f;
        public double f_1;
        public double e_2;
        public double e_12;
        //构造函数
        public EarthParam(double a,double f_1)
        {
            this.f_1 = f_1;
            f = Math.Pow(f_1, -1);
            this.a = a;
            b = this.a * (1 - f);
            e_2 = (a * a - b * b) / (a * a);
            //下面公式容易出错，不要写成e12=(e2*e2)/(1-e2*e2)
            e_12 = (e_2) / (1 - e_2);

        }
        
    }
}
