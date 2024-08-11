using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dadixian
{
    class EarthLineInfo
    {
        #region 大地线参数
        public string startName;
        public double B1;
        public double L1;
        public string endName;
        public double B2;
        public double L2;
        public double S;
        #endregion
        static int i = 1;

        public EarthLineInfo(string s,double B1,double L1,string e,double B2,double L2)
        {
            startName = s;
            this.B1 = B1;
            this.L1 = L1;
            endName = e;
            this.B2 = B2;
            this.L2 = L2;
            S = 0;
        }

        #region 核心代码
        /// <summary>
        /// 计算大地线长度
        /// </summary>
        /// <param name="param">椭球体参数</param>
        public void CalculateLength(EarthParam param)
        {
            //辅助计算
            double e_2 = param.e_2;
            double e_12 = param.e_12;
            B1 = AngleToRad(B1);
            B2 = AngleToRad(B2);
            double l = AngleToRad(L2) - AngleToRad(L1);

            double u1 = Math.Atan(Math.Sqrt(1 - e_2) * Math.Tan(B1));
            double u2 = Math.Atan(Math.Sqrt(1 - e_2) * Math.Tan(B2));
            double a1 = Math.Sin(u1) * Math.Sin(u2);
            double a2 = Math.Cos(u1) * Math.Cos(u2);
            double b1 = Math.Cos(u1) * Math.Sin(u2);
            double b2 = Math.Sin(u1) * Math.Cos(u2);

            if (startName=="P1")
            {
                MyLog.Add("第1条大地线辅助参数u1," + Math.Round(u1, 8));
                MyLog.Add("第1条大地线辅助参数u2," + Math.Round(u2, 8));
                MyLog.Add("第1条大地线辅助参数经差l(弧度)," + Math.Round(l, 8));
                MyLog.Add("第1条大地线辅助参数a1," + Math.Round(a1, 8));
                MyLog.Add("第1条大地线辅助参数a2," + Math.Round(a2, 8));
                MyLog.Add("第1条大地线辅助参数b1," + Math.Round(b1, 8));
                MyLog.Add("第1条大地线辅助参数b2," + Math.Round(b2, 8));
            }
            
            //计算起点大地方位角
            double A1 = 0, lamda = 0, _8 = 0, p = 0, q = 0, sin_6 = 0, 
                cos_6 = 0, _6 = 0, _61 = 0, sinA0 = 0, _a = 0, _b = 0, _r = 0;
            double dif = 0;
            do
            {
                lamda = l + _8;
                dif = _8;
                p = Math.Cos(u2) * Math.Sin(lamda);
                q = b1 - b2 * Math.Cos(lamda);
                A1 = Math.Atan(p/q);
                
                A1 = (p>0) ? ((q>0)?(Math.Abs(A1)) :(Math.PI - Math.Abs(A1))) 
                    : ((q > 0) ? (2*Math.PI - Math.Abs(A1)) : (Math.PI + Math.Abs(A1)));
                if (A1 < 0)
                {
                    A1 = A1 + Math.PI * 2;
                }
                if (A1> Math.PI * 2)
                {
                    A1 = A1 - Math.PI * 2;
                }

                sin_6 = p * Math.Sin(A1) + q * Math.Cos(A1);
                cos_6 = a1 + a2 * Math.Cos(lamda);
                _6 = Math.Atan(sin_6/cos_6);
                _6 = (cos_6 > 0 ? Math.Abs(_6) : (Math.PI - Math.Abs(_6)));

                sinA0 = Math.Cos(u1) * Math.Sin(A1);
                double cosA0_2 = 1 - Math.Pow(sinA0, 2);
                _61 = Math.Atan(Math.Tan(u1)/Math.Cos(A1));

                double e2 = e_2, e4 = Math.Pow(e2, 2), e6 = Math.Pow(e2, 3);
                _a = (e2 / 2 + e4 / 8 + e6 / 16) - (e4 / 16 + e6 / 16) * cosA0_2 + (3 * e6 / 128) * Math.Pow(cosA0_2, 2);
                _b = (e4 / 16 + e6 / 16) * cosA0_2 - (e6 / 32) * Math.Pow(cosA0_2, 2);
                _r = (e6 / 256) * Math.Pow(cosA0_2, 2);


                _8 = (_a*_6+_b*Math.Cos(2*_61+_6)*Math.Sin(_6)+
                    _r*Math.Sin(2*_6)*Math.Cos(4*_61+2*_6)) * sinA0;
                dif = Math.Abs(_8 - dif);
            } while (dif>Math.Pow(10,-10));
            //
            lamda = l + _8;

            //计算长度
            double cosA02 = 1 - Math.Pow(sinA0, 2);
            double k2 = e_12 * cosA02, k4 = Math.Pow(k2, 2), k6 = Math.Pow(k2, 3);
            double A = (1 - k2 / 4 + 7 * k4 / 64 - 15 * k6 / 256) / param.b;
            double B = (k2 / 4 - k4 / 8 + 37 * k6 / 512);
            double C = k4 / 128 - k6 / 128;

            _61 = Math.Atan(Math.Tan(u1)/Math.Cos(A1));
            double x_s = C * Math.Sin(2 * _6) * Math.Cos(4 * _61 + 2 * _6);
            S = (_6-B*Math.Sin(_6)*Math.Cos(2*_61+_6)-x_s) / A;

            if (startName=="P1")
            {
                MyLog.Add("第1条大地线a," + Math.Round(_a, 8));
                MyLog.Add("第1条大地线b," + Math.Round(_b, 8));
                MyLog.Add("第1条大地线r," + Math.Round(_r, 8));
                MyLog.Add("第1条大地线A1," + Math.Round(A1, 8));
                MyLog.Add("第1条大地线lamda," + Math.Round(lamda, 8));
                MyLog.Add("第1条大地线6," + Math.Round(_6, 8));
                MyLog.Add("第1条大地线sinA0," + Math.Round(sinA0, 8));
                MyLog.Add("第1条大地线A," + Math.Round(A, 8));
                MyLog.Add("第1条大地线B," + Math.Round(B, 8));
                MyLog.Add("第1条大地线C," + Math.Round(C, 8));
                MyLog.Add("第1条大地线61," + Math.Round(_61, 8));
            }

            MyLog.Add("第" + (i++) + "条大地线长度," + Math.Round(S, 3));
            
        }
        #endregion
        //角度转弧度
        public double AngleToRad(double angle)
        {
            //dd.mmsss->dd.ddddd
            double d = Math.Floor(angle);
            double m = Math.Floor((angle - d) * 100);
            double s = ((angle - d) * 100 - m) * 100;
            angle = d + m / 60 + s / 3600;
            //转弧度
            
            return angle * Math.PI / 180;
        }

    }
}
