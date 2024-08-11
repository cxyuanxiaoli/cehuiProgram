using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QianfangJiaohui
{
    class DataCenter
    {
        public PhotoInfo[] p=new PhotoInfo[2];

        /// <summary>
        /// 读取数据
        /// </summary>
        public void ImportData()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "导入数据";
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = "文本文件（*.txt）|*.txt";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (StreamReader sr = new StreamReader(ofd.FileName))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        string line = sr.ReadLine().Trim();
                        double xs = Convert.ToDouble(line);
                        line = sr.ReadLine().Trim();
                        double ys = Convert.ToDouble(line);
                        line = sr.ReadLine().Trim();
                        double zs = Convert.ToDouble(line);
                        line = sr.ReadLine().Trim();
                        double _4 = Convert.ToDouble(line);
                        line = sr.ReadLine().Trim();
                        double _w = Convert.ToDouble(line);
                        line = sr.ReadLine().Trim();
                        double _k = Convert.ToDouble(line);
                        line = sr.ReadLine().Trim();
                        double x = Convert.ToDouble(line);
                        line = sr.ReadLine().Trim();
                        double y = Convert.ToDouble(line);
                        line = sr.ReadLine().Trim();
                        double f = Convert.ToDouble(line);
                        p[i] = new PhotoInfo(f, x, y, xs, ys, zs, _4, _w, _k);
                    }
                    
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 主要计算
        /// </summary>
        /// <returns>返回辅助参数，投影系数，和地面坐标
        /// 格式为 u1,v1,w1,u2,v2,w2,N1,N2,X,Y,Z</returns>
        public double[] Calculate()
        {
            //计算旋转矩阵元素
            double[,] a1 = GetRevolveMatrix(p[0]);
            double[,] a2 = GetRevolveMatrix(p[1]);

            // u  v  w
            double[] res1 = MulMatrix(a1, new double[] { p[0].x, p[0].y, -p[0].f });
            double[] res2 = MulMatrix(a2, new double[] { p[1].x, p[1].y, -p[1].f });

            //获取投影系数
            double Bu = p[1].xs - p[0].xs;
            double Bv = p[1].ys - p[0].ys;
            double Bw = p[1].zs - p[0].zs;

            double N1 = (Bu * res2[2] - Bw * res2[0]) / (res1[0] * res2[2] - res2[0] * res1[2]);
            double N2 = (Bu * res1[2] - Bw * res1[0]) / (res1[0] * res2[2] - res2[0] * res1[2]);

            //计算地面坐标
            double X = p[0].xs + N1 * res1[0];
            double Y = 0.5 * (p[0].ys + N1 * res1[1] + p[1].ys + N2 * res2[1]);
            double Z = p[0].zs + N1 * res1[2];

            return res1.Concat(res2).Concat(new double[] { N1, N2, X, Y, Z }).ToArray();
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="a">3*3矩阵</param>
        /// <param name="b">3*1矩阵</param>
        /// <returns>u v w</returns>
        public double[] MulMatrix(double[,] a,double[] b)
        {
            return new double[] {a[0,0]*b[0]+a[0,1]*b[1]+a[0,2]*b[2],
                a[1,0]*b[0]+a[1,1]*b[1]+a[1,2]*b[2],
                a[2,0]*b[0]+a[2,1]*b[1]+a[2,2]*b[2]
            };
        }

        /// <summary>
        /// 计算旋转矩阵
        /// </summary>
        /// <param name="photo">相片参数对象</param>
        /// <returns>3*3旋转矩阵</returns>
        public double[,] GetRevolveMatrix(PhotoInfo photo)
        {
            double _4 = AngleToRad(photo._4), _w = AngleToRad(photo._w), _k = AngleToRad(photo._k);
            double a1 = Math.Cos(_4) * Math.Cos(_k) - Math.Cos(_4) * Math.Sin(_w) * Math.Sin(_k);
            double a2 = -Math.Cos(_4) * Math.Sin(_k) - Math.Sin(_4) * Math.Sin(_w) * Math.Sin(_k);
            double a3 = -Math.Sin(_4) * Math.Cos(_w);
            double b1 = Math.Cos(_w) * Math.Sin(_k);
            double b2 = Math.Cos(_w) * Math.Cos(_k);
            double b3 = -Math.Sin(_w);
            double c1 = Math.Sin(_4) * Math.Cos(_k) + Math.Cos(_4) * Math.Sin(_w) * Math.Sin(_k);
            double c2 = -Math.Sin(_w) * Math.Cos(_k) + Math.Cos(_4) * Math.Sin(_w) * Math.Sin(_k);
            double c3 = Math.Cos(_4) * Math.Cos(_w);
            return new double[3, 3] { { a1, a2, a3 }, { b1, b2, b3 }, { c1, c2, c3 } };
        }

        public double AngleToRad(double angle)
        {
            
            return angle * Math.PI / 180;
        }
    }
}
