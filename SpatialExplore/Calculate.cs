using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpatialExplore
{
    class Calculate
    {
        //数据
        Point[] data;
        public List<List<Point>> areaData;
        //空间权重矩阵
        public double[,] swMatrix;

        public Calculate(Point[] points)
        {
            this.data = points;
        }

        #region 探索分析
        /// <summary>
        /// 探索性分析
        /// </summary>
        public void ExploreAnalyse()
        {
            SumByArea();

            //计算平均中心 
            double aveX = data.Average(t => t.x);
            double aveY = data.Average(t => t.y);

            MyLog.Add("坐标分量x的平均值X," + Math.Round(aveX, 3));
            MyLog.Add("坐标分量y的平均值Y," + Math.Round(aveY, 3));

            //标准差椭圆计算
            double varX = data.Sum(t => Math.Pow(t.x - aveX, 2));
            double varY = data.Sum(t => Math.Pow(t.y - aveY, 2));

            double A = varX - varY;
            double C = 2 * data.Sum(t => (t.x-aveX) * (t.y-aveY));
            double B = Math.Sqrt(Math.Pow(A, 2) + Math.Pow(C, 2));

            double seita = Math.Atan((A + B) / C);
            double SDEx = Math.Sqrt(2) * 
                Math.Sqrt(data.Sum(t => Math.Pow((t.x - aveX) * Math.Cos(seita) + (t.y - aveY) * Math.Sin(seita), 2)) / data.Length);
            double SDEy = Math.Sqrt(2) *
                Math.Sqrt(data.Sum(t => Math.Pow((t.x - aveX) * Math.Sin(seita) - (t.y - aveY) * Math.Cos(seita), 2)) / data.Length);

            MyLog.Add("P6坐标分量与平均中心之间的偏移量a6,"+Math.Round(data[5].x-aveX,3));
            MyLog.Add("P6坐标分量与平均中心之间的偏移量b6,"+Math.Round(data[5].y-aveY,3));
            MyLog.Add("辅助量A,"+Math.Round(A,3));
            MyLog.Add("辅助量B,"+Math.Round(B,3));
            MyLog.Add("辅助量C,"+Math.Round(C,3));
            MyLog.Add("标准差椭圆长轴与竖直方向的夹角seita,"+Math.Round(seita,3));
            MyLog.Add("标准差椭圆的长半轴SDEx,"+Math.Round(SDEx,3));
            MyLog.Add("标准差椭圆的长半轴SDEy,"+ Math.Round(SDEy, 3));
            
        }

        /// <summary>
        /// 将数据分区
        /// </summary>
        public void SumByArea()
        {
            List<Point> q1 = new List<Point>();
            List<Point> q2 = new List<Point>();
            List<Point> q3 = new List<Point>();
            List<Point> q4 = new List<Point>();
            List<Point> q5 = new List<Point>();
            List<Point> q6 = new List<Point>();
            List<Point> q7 = new List<Point>();

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].area_code == 1)
                {
                    q1.Add(data[i]);
                }
                else if (data[i].area_code == 2)
                {
                    q2.Add(data[i]);
                }
                else if (data[i].area_code == 3)
                {
                    q3.Add(data[i]);
                }
                else if (data[i].area_code == 4)
                {
                    q4.Add(data[i]);
                }
                else if (data[i].area_code == 5)
                {
                    q5.Add(data[i]);
                }
                else if (data[i].area_code == 6)
                {
                    q6.Add(data[i]);
                }
                else if (data[i].area_code == 7)
                {
                    q7.Add(data[i]);
                }
            }

            MyLog.Add("1 区（区号为1）的事件数量n1," + q1.Count);
            MyLog.Add("4 区（区号为1）的事件数量n4," + q4.Count);
            MyLog.Add("6 区（区号为1）的事件数量n6," + q6.Count);
            MyLog.Add("事件总数n," + data.Length);

            areaData = new List<List<Point>>();
            areaData.Add(q1);
            areaData.Add(q2);
            areaData.Add(q3);
            areaData.Add(q4);
            areaData.Add(q5);
            areaData.Add(q6);
            areaData.Add(q7);
        }
        #endregion


        #region 空间权重矩阵
        /// <summary>
        /// 生成空间权重矩阵
        /// </summary>
        /// <returns>空间矩阵数据表</returns>
        public DataTable SWMatrix()
        {
            //.空间权重矩阵
            //计算各区的平均中心
            DataTable st = new DataTable();
            st.Columns.Add(" ");
            st.Columns.Add("1区");
            st.Columns.Add("2区");
            st.Columns.Add("3区");
            st.Columns.Add("4区");
            st.Columns.Add("5区");
            st.Columns.Add("6区");
            st.Columns.Add("7区");
            double[] aveX_area = new double[7];
            double[] aveY_area = new double[7];
            for (int i = 0; i < areaData.Count; i++)
            {
                List<Point> l = areaData[i];
                aveX_area[i] = l.Average(p => p.x);
                aveY_area[i] = l.Average(p => p.y);
            }
            MyLog.Add("1区平均中心的坐标分量X," + Math.Round(aveX_area[0], 3));
            MyLog.Add("1区平均中心的坐标分量Y," + Math.Round(aveY_area[0], 3));
            MyLog.Add("4区平均中心的坐标分量X," + Math.Round(aveX_area[3], 3));
            MyLog.Add("4区平均中心的坐标分量Y," + Math.Round(aveY_area[3], 3));

            //计算各区之间的空间权重矩阵
            swMatrix = new double[7, 7];
            for (int i = 0; i < swMatrix.GetLength(0); i++)
            {
                for (int j = i+1; j < swMatrix.GetLength(1); j++)
                {
                    swMatrix[i, j] = swMatrix[j, i] = 1000 /
                        Math.Sqrt(Math.Pow(aveX_area[i] - aveX_area[j], 2) + Math.Pow(aveY_area[i] - aveY_area[j],2));
                }
            }
            for (int i = 0; i < swMatrix.GetLength(0); i++)
            {
                string[] items = new string[8];
                items[0] = (i + 1) + "区";
                for (int j = 0; j < swMatrix.GetLength(1); j++)
                {
                    items[j+1] = Math.Round(swMatrix[i, j],3).ToString();
                }
                st.Rows.Add(items);
            }
            MyLog.Add("1区和4区的空间权重w1,4," + Math.Round(swMatrix[0, 3], 6));
            MyLog.Add("6区和7区的空间权重w6,7," + Math.Round(swMatrix[5, 6], 6));
            return st;
        }
        #endregion

        #region 莫兰指数
        /// <summary>
        /// 全局莫兰指数计算
        /// </summary>
        public void Moran_I()
        {
            //莫兰指数计算
            int N = 7;
            //数据整理
            double aveZ=areaData.Average(list => list.Count);
            MyLog.Add("研究区域犯罪事件的平均值𝑋," + Math.Round(aveZ, 6));

            //全局莫兰指数
            double[] z = new double[7];
            for (int i = 0; i < z.Length; i++)
            {
                z[i] = areaData[i].Count;
            }

            double S0 = 0;
            for (int i = 0; i < swMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < swMatrix.GetLength(1); j++)
                {
                    S0 += swMatrix[i, j];
                }
            }
            double sum1=0, sum2=0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    sum1 += swMatrix[i, j] * (z[i] - aveZ) * (z[j] - aveZ);
                }
            }
            sum2 = z.Sum(t => Math.Pow(t - aveZ, 2));

            double I = N * sum1 / (S0 * sum2);

            MyLog.Add("全局莫兰指数辅助量S0," + Math.Round(S0, 6));
            MyLog.Add("全局莫兰指数I," + Math.Round(I, 6));

        }

        /// <summary>
        /// 局部莫兰指数计算
        /// </summary>
        public void LocalMoran()
        {
            //局部莫兰指数
            double[] I = new double[7];
            int N = 7;
            double[] z = new double[7];
            for (int i = 0; i < z.Length; i++)
            {
                z[i] = areaData[i].Count;
            }
            double aveZ = areaData.Average(t => t.Count);

            for (int i = 0; i < N; i++)
            {
                double S2 = 0;
                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                        continue;
                    S2 += Math.Pow(z[j] - aveZ, 2);
                }
                S2 /= N - 1;
                double sum = 0;
                for (int j = 0; j < N; j++)
                {
                    if (j == i)
                        continue;
                    sum += swMatrix[i, j] * (z[j] - aveZ);
                }
                I[i] = (z[i] - aveZ) * sum / S2;
            }

            MyLog.Add("1区的局部莫兰指数I1," + Math.Round(I[0], 6));
            MyLog.Add("3区的局部莫兰指数I3," + Math.Round(I[2], 6));
            MyLog.Add("5区的局部莫兰指数I5," + Math.Round(I[4], 6));
            MyLog.Add("7区的局部莫兰指数I7," + Math.Round(I[6], 6));

            //计算局部莫兰指数的Z得分
            double u = I.Average();

            double _6 = Math.Sqrt(I.Sum(t => Math.Pow(t - u, 2)) / (N - 1));

            double[] score = new double[N];
            for (int i = 0; i < N; i++)
            {
                score[i] = (I[i] - u) / _6;
            }

            MyLog.Add("局部莫兰指数的平均数u," + Math.Round(u, 6));
            MyLog.Add("局部莫兰指数的标准差6," + Math.Round(_6, 6));
            MyLog.Add("1区局部莫兰指数的Z得分Z1," + Math.Round(score[0], 6));
            MyLog.Add("3区局部莫兰指数的Z得分Z3," + Math.Round(score[2], 6));
            MyLog.Add("5区局部莫兰指数的Z得分Z5," + Math.Round(score[4], 6));
            MyLog.Add("7区局部莫兰指数的Z得分Z7," + Math.Round(score[6], 6));
        }
        #endregion

    }
}
