using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duanmian2
{
    class Calculate
    {
        Point jizhun;
        Point[] ceshi;
        Point[] key;
        Point[] shice;

        public Calculate(Point jizhun,Point[] ceshi,Point[] key,Point[] shice)
        {
            this.jizhun = jizhun;
            this.ceshi = ceshi;
            this.key = key;
            this.shice = shice;
        }

        #region 基本计算
        public void BaseCalculate()
        {
            Point A = ceshi[0];
            Point B = ceshi[1];
            //方位角
            double angle = GetDirectAngle(A, B);
            MyLog.Add("AB的坐标方位角," + Math.Round(angle, 5));

            //计算内差点高程
            GetInsPointH(A);
            GetInsPointH(B);
            MyLog.Add("测试点A的内插高程," + Math.Round(A.h,3));
            MyLog.Add("测试点B的内插高程," + Math.Round(B.h, 3));

            //AB断面面积的计算
            double S=GetArea(A, B);
            MyLog.Add("AB断面面积," + Math.Round(S, 3));


        }
        #endregion

        #region 纵断面计算
        /// <summary>
        /// 道路纵断面计算
        /// </summary>
        /// <returns>纵断面内插点集</returns>
        public Point[] Zongduanmian()
        {
            //计算纵断面的平面距离
            Point k0 = key[0], k1 = key[1], k2 = key[2];
            double dis01 = GetDistance(k0, k1);
            double dis12 = GetDistance(k1, k2);
            double disTotal = dis01 + dis12;
            MyLog.Add("纵断面K0-K1分段平面距离," + Math.Round(dis01, 3));
            MyLog.Add("纵断面K1-K2分段平面距离," + Math.Round(dis12, 3));
            MyLog.Add("纵断面总距离," + Math.Round(disTotal, 3));

            //计算K0到K1之间的内插点平面坐标 
            double angle01 = GetDirectAngle(k0, k1);
            double angle12 = GetDirectAngle(k1, k2);

            List<Point> insPoints01 = new List<Point>();
            insPoints01.Add(k0);
            //k0-k1段内插点数
            int count01 = Convert.ToInt32(Math.Floor(dis01 / 10));
            for (int i = 1; i <= count01; i++)
            {
                double x = k0.x + 10 * i * Math.Cos(angle01);
                double y = k0.y + 10 * i * Math.Sin(angle01);
                Point z = new Point("Z" + i, x, y, 0);
                GetInsPointH(z);
                insPoints01.Add(z);
            }
            insPoints01.Add(k1);

            MyLog.Add("方位角a01," + Math.Round(angle01, 5));
            MyLog.Add("第一条纵断面的内插点Z3坐标值X, " + Math.Round(insPoints01[3].x, 3));
            MyLog.Add("第一条纵断面的内插点Z3坐标值Y, " + Math.Round(insPoints01[3].y, 3));
            MyLog.Add("第一条纵断面的内插点Z3坐标值H, " + Math.Round(insPoints01[3].h, 3));

            //计算K1到K2之间的内插点平面坐标
            List<Point> insPoints12 = new List<Point>();
            insPoints12.Add(k1);
            //k1-k2段内插点个数
            int count12 = Convert.ToInt32(Math.Floor(disTotal / 10)) - count01;
            for (int i = 1; i <= count12; i++)
            {
                double x = k1.x + ((i + count01) * 10 - dis01) * Math.Cos(angle12);
                double y = k1.y + ((i + count01) * 10 - dis01) * Math.Sin(angle12);
                Point q = new Point("Y" + i, x, y, 0);
                GetInsPointH(q);
                insPoints12.Add(q);
            }
            insPoints12.Add(k2);
            MyLog.Add("方位角a12," + Math.Round(angle12, 5));
            MyLog.Add("第二条纵断面的内插点Y3坐标值X, " + Math.Round(insPoints12[3].x, 3));
            MyLog.Add("第二条纵断面的内插点Y3坐标值Y, " + Math.Round(insPoints12[3].y, 3));
            MyLog.Add("第二条纵断面的内插点Y3坐标值H, " + Math.Round(insPoints12[3].h, 3));

            //计算纵断面面积
            double S1 = 0, S2 = 0;
            for (int i = 0; i < insPoints01.Count-1; i++)
            {
                S1 += GetArea(insPoints01[i], insPoints01[i + 1]);
            }
            for (int i = 0; i < insPoints12.Count - 1; i++)
            {
                S2 += GetArea(insPoints12[i], insPoints12[i + 1]);
            }
            double S = S1 + S2;
            MyLog.Add("第一条纵断面面积," + Math.Round(S1, 3));
            MyLog.Add("第二条纵断面面积," + Math.Round(S2, 3));
            MyLog.Add("纵断面总面积," + Math.Round(S, 3));

            return insPoints01.Concat(insPoints12).ToArray();
        }
        #endregion

        #region 横断面计算
        /// <summary>
        /// 横断面计算
        /// </summary>
        /// <returns>多条横断面内插点集</returns>
        public List<Point[]> Hengduanmian()
        {
            //计算横断面中心点
            Point m0 = GetCenterPoint("M0", key[0], key[1]);
            Point m1 = GetCenterPoint("M1", key[1], key[2]);
            //计算横断面插值的平面坐标和高程
            double angle0 = GetDirectAngle(key[0], key[1]) + Math.PI / 2;
            double angle1 = GetDirectAngle(key[1], key[2]) + Math.PI / 2;

            List<Point> insPointsM0 = new List<Point>();
            int count = 1;
            for (int i = -5; i <= 5; i++)
            {
                if (i==0)
                {
                    insPointsM0.Add(m0);
                    continue;
                }
                double x = m0.x + i * 5 * Math.Cos(angle0);
                double y = m0.y + i * 5 * Math.Sin(angle0);
                Point p = new Point("Q" + (count++), x, y, 0);
                GetInsPointH(p);
                insPointsM0.Add(p);
            }

            List<Point> insPointsM1 = new List<Point>();
            count = 1;
            for (int i = -5; i <= 5; i++)
            {
                if (i == 0)
                {
                    insPointsM1.Add(m1);
                    continue;
                }
                double x = m1.x + i * 5 * Math.Cos(angle1);
                double y = m1.y + i * 5 * Math.Sin(angle1);
                Point p = new Point("W" + (count++), x, y, 0);
                GetInsPointH(p);
                insPointsM1.Add(p);
            }

            MyLog.Add("第一条横断面内插点Q3坐标X," + Math.Round(insPointsM0[2].x, 3));
            MyLog.Add("第一条横断面内插点Q3坐标Y," + Math.Round(insPointsM0[2].y, 3));
            MyLog.Add("第一条横断面内插点Q3坐标H," + Math.Round(insPointsM0[2].h, 3));
            MyLog.Add("第二条横断面内插点W3坐标X," + Math.Round(insPointsM1[2].x, 3));
            MyLog.Add("第二条横断面内插点W3坐标Y," + Math.Round(insPointsM1[2].y, 3));
            MyLog.Add("第二条横断面内插点W3坐标H," + Math.Round(insPointsM1[2].h, 3));

            //计算横断面面积
            double Srow1 = 0, Srow2 = 0;
            for (int i = 0; i < insPointsM0.Count-1; i++)
            {
                Srow1 += GetArea(insPointsM0[i], insPointsM0[i + 1]);
            }
            for (int i = 0; i < insPointsM1.Count - 1; i++)
            {
                Srow2 += GetArea(insPointsM1[i], insPointsM1[i + 1]);
            }
            MyLog.Add("第一条横断面的面积Srow1," + Math.Round(Srow1, 3));
            MyLog.Add("第二条横断面的面积Srow2," + Math.Round(Srow2, 3));

            List<Point[]> list = new List<Point[]>();
            list.Add(insPointsM0.ToArray());
            list.Add(insPointsM1.ToArray());
            return list;
        }
        #endregion

        #region 辅助计算
        /// <summary>
        /// 计算两点的中心点
        /// </summary>
        /// <param name="s">中心点名</param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public Point GetCenterPoint(string s,Point p1,Point p2)
        {
            Point p = new Point(s, (p1.x + p2.x) / 2, (p1.y + p2.y) / 2, 0);
            GetInsPointH(p);
            return p;
        }

        /// <summary>
        /// 断面面积的计算
        /// </summary>
        /// <param name="p1">一段</param>
        /// <param name="p2">另一端</param>
        /// <returns>面积</returns>
        public double GetArea(Point p1,Point p2)
        {
            double L = GetDistance(p1, p2);
            double S = (p1.h + p2.h - 2 * jizhun.h) * L / 2;
            return S;
        }

        /// <summary>
        /// 内插点P的高程值的计算
        /// </summary>
        /// <param name="p">内插点</param>
        public void GetInsPointH(Point p)
        {
            //实测点集
            List<Point> points = new List<Point>(this.shice);
            points = points.OrderBy(t => GetDistance(p, t)).ToList();
            //最近点集
            //Point[] near=points.Take(5).ToArray();
            Point[] near= new Point[] { points[0], points[1], points[2], points[3], points[4] };
            double sum1=near.Sum(t => t.h / GetDistance(p, t));
            double sum2=near.Sum(t => 1 / GetDistance(p, t));

            p.h = sum1 / sum2;
        }

        /// <summary>
        /// 计算两点间距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public double GetDistance(Point p1,Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
        }

        /// <summary>
        /// 计算两点的方位角（弧度）
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public double GetDirectAngle(Point p1,Point p2)
        {
            double dx = p2.x - p1.x;
            double dy = p2.y - p1.y;
            if (dx == 0)
            {
                return (dy > 0) ? Math.PI / 2 : Math.PI * 3 / 2;
            }
            double angle = Math.Atan(dy / dx);
            angle = (dy > 0) ? ((dx > 0) ? angle : (Math.PI + angle)) : ((dx > 0) ? (Math.PI * 2 + angle) : (Math.PI + angle));
            return angle;
        }
        #endregion
    }
}
