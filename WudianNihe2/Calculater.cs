using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WudianNihe2
{
    //核心计算类
    class Calculater
    {
        Point[] points;

        public Calculater(Point[] data)
        {
            points = data;
        }

        #region 核心算法
        /// <summary>
        /// 五点拟合核心算法
        /// </summary>
        /// <param name="isClose">是否闭合</param>
        public List<Point> GetResult(bool isClose)
        {
            double[] border = FindMaxMin();
            MyLog.Add("--------------基本信息------------------");
            MyLog.Add("总点数: " + points.Length);
            MyLog.Add("x边界:" + border[0] + "至" + border[1]);
            MyLog.Add("y边界:" + border[2] + "至" + border[3]);
            MyLog.Add("是否闭合:" + (isClose ? "是" : "否"));

            //补充点
            AddPoints(isClose);

            //计算各点梯度
            int n = points.Length;
            for (int i = 2; i < n - 2; i++)
            {
                CalTidu(points[i - 2], points[i - 1], points[i], points[i + 1], points[i + 2]);
            }

            MyLog.Add("----------计算结果------------\n" +
                "说明: 两点之间的曲线方程为:\n" +
                "x = p0 + p1 * z + p2 * z * z + p3 * z * z * z\n" +
                "y = q0 + q1 * z + q2 * z * z + q3 * z * z * z\n" +
                "z为两点之间的弦长变量[0, 1]");
            MyLog.Add("起点id    起点x     起点y     终点id    终点x     终点y     " +
                "p0      p1      p2      p3      " +
                "q0      q1      q2      q3      ");

            //计算各点间曲线参数，生成曲线段
            List<Point> list = new List<Point>();
            if (isClose)
            {
                for (int i = 2; i < n - 2; i++)
                {
                    MyCurve curve = CalCurveParam(points[i], points[i + 1]);
                    list.AddRange(curve.GetInsertPoints());
                    //输出曲线参数
                    Point start = points[i], end = points[i + 1];
                    string[] p = { Math.Round(curve.E[0], 3).ToString().PadRight(8),
                        Math.Round(curve.E[1], 3).ToString().PadRight(8),
                        Math.Round(curve.E[2], 3).ToString().PadRight(8),
                        Math.Round(curve.E[3], 3).ToString().PadRight(8)
                    };
                    string[] q = { Math.Round(curve.F[0], 3).ToString().PadRight(8),
                        Math.Round(curve.F[1], 3).ToString().PadRight(8),
                        Math.Round(curve.F[2], 3).ToString().PadRight(8),
                        Math.Round(curve.F[3], 3).ToString().PadRight(8)
                    };
                    MyLog.Add(start.ToString()+end.ToString()+p[0]+p[1]+p[2]+p[3]+
                        q[0] + q[1] + q[2] + q[3]);
                }
            }
            else
            {
                for (int i = 2; i < n - 3; i++)
                {
                    MyCurve curve = CalCurveParam(points[i], points[i + 1]);
                    list.AddRange(curve.GetInsertPoints());
                    //输出曲线参数
                    Point start = points[i], end = points[i + 1];
                    string[] p = { Math.Round(curve.E[0], 3).ToString().PadRight(8),
                        Math.Round(curve.E[1], 3).ToString().PadRight(8),
                        Math.Round(curve.E[2], 3).ToString().PadRight(8),
                        Math.Round(curve.E[3], 3).ToString().PadRight(8)
                    };
                    string[] q = { Math.Round(curve.F[0], 3).ToString().PadRight(8),
                        Math.Round(curve.F[1], 3).ToString().PadRight(8),
                        Math.Round(curve.F[2], 3).ToString().PadRight(8),
                        Math.Round(curve.F[3], 3).ToString().PadRight(8)
                    };
                    MyLog.Add(start.ToString() + end.ToString() + p[0] + p[1] + p[2] + p[3] +
                        q[0] + q[1] + q[2] + q[3]);
                }
            }
            return list;
        }
        #endregion
        

        #region 分算法
        /// <summary>
        /// 计算离散点范围
        /// </summary>
        /// <returns>xmin,xmax,ymin,ymax</returns>
        public double[] FindMaxMin()
        {
            double xmin, xmax, ymin, ymax;
            xmin = xmax = points[0].x;
            ymin = ymax = points[0].y;
            for (int i = 1; i < points.Length; i++)
            {
                double x = points[i].x, y = points[i].y;
                xmin = x < xmin ? x : xmin;
                xmax = x > xmax ? x : xmax;
                ymin = y < ymin ? y : ymin;
                ymax = y > ymax ? y : ymax;
            }
            return new double[] { xmin, xmax, ymin, ymax };
        }

        /// <summary>
        /// 计算两点间曲线参数
        /// </summary>
        /// <param name="p">一点</param>
        /// <param name="p1">另一点</param>
        /// <returns>曲线参数对象</returns>
        public MyCurve CalCurveParam(Point p, Point p1)
        {
            double r = Math.Sqrt(Math.Pow(p1.x - p.x, 2) + Math.Pow(p1.y - p.y, 2));
            double e0 = p.x;
            double e1 = r * p.tidu[0];
            double e2 = 3 * (p1.x - p.x) - r * (p1.tidu[0] + 2 * p.tidu[0]);
            double e3 = -2 * (p1.x - p.x) + r * (p1.tidu[0] + p.tidu[0]);
            double f0 = p.y;
            double f1 = r * p.tidu[1];
            double f2 = 3 * (p1.y - p.y) - r * (p1.tidu[1] + 2 * p.tidu[1]);
            double f3 = -2 * (p1.y - p.y) + r * (p1.tidu[1] + p.tidu[1]);
            double[] E = new double[] { e0, e1, e2, e3 };
            double[] F = new double[] { f0, f1, f2, f3 };
            return new MyCurve(E, F);
        }


        /// <summary>
        /// 计算单点p的梯度
        /// </summary>
        /// <param name="p_2">p前面第二点</param>
        /// <param name="p_1">p前面一点</param>
        /// <param name="p">p点</param>
        /// <param name="p1">p后一点</param>
        /// <param name="p2">p后第二点</param>
        public void CalTidu(Point p_2, Point p_1, Point p, Point p1, Point p2)
        {
            double a1 = p_1.x - p_2.x;
            double a2 = p.x - p_1.x;
            double a3 = p1.x - p.x;
            double a4 = p2.x - p1.x;
            double b1 = p_1.y - p_2.y;
            double b2 = p.y - p_1.y;
            double b3 = p1.y - p.y;
            double b4 = p2.y - p1.y;

            double w2 = Math.Abs(a3 * b4 - a4 * b3);
            double w3 = Math.Abs(a1 * b2 - a2 * b1);

            double a0 = w2 * a2 + w3 * a3;
            double b0 = w2 * b2 + w3 * b3;

            double cos = a0 / Math.Sqrt(a0 * a0 + b0 * b0);
            double sin = b0 / Math.Sqrt(a0 * a0 + b0 * b0);
            p.tidu = new double[] { cos, sin };
        }

        /// <summary>
        /// 补充点算法
        /// </summary>
        /// <param name="isClose">是否闭合</param>
        public void AddPoints(bool isClose)
        {
            int pn = points.Length;
            Point[] data = new Point[pn + 4];
            int dn = data.Length;
            for (int i = 2; i < dn - 2; i++)
            {
                data[i] = points[i - 2];
            }
            if (isClose)
            {
                data[0] = points[pn - 2];
                data[1] = points[pn - 1];
                data[dn - 1] = points[1];
                data[dn - 2] = points[0];
            }
            else
            {
                data[1] = new Point("A", data[2], data[3], data[4]);
                data[0] = new Point("B", data[1], data[2], data[3]);
                data[dn - 2] = new Point("C", data[dn - 3], data[dn - 4], data[dn - 5]);
                data[dn - 1] = new Point("D", data[dn - 2], data[dn - 3], data[dn - 4]);
            }
            points = data;
        }
        #endregion
    }
}
