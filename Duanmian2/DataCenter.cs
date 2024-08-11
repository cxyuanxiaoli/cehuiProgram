using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Duanmian2
{
    class DataCenter
    {
        #region 字段
        public Point jizhun;
        public Point[] ceshi = new Point[2];
        public Point[] key;
        public Point[] shice;
        public DataTable dt;
        #endregion

        #region 导入数据
        public void ImportData()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "导入数据";
            ofd.Filter = "文本文件|*.txt";
            ofd.InitialDirectory = Environment.CurrentDirectory;
            if (ofd.ShowDialog()!=DialogResult.OK)
            {
                return;
            }
            try
            {
                List<Point> keyList = new List<Point>();
                List<Point> shiceList = new List<Point>();
                dt = new DataTable();
                dt.Columns.Add("id");
                dt.Columns.Add("x");
                dt.Columns.Add("y");
                dt.Columns.Add("h");

                using(StreamReader sr=new StreamReader(ofd.FileName))
                {
                    //基准点
                    string line = sr.ReadLine().Trim();
                    string[] item = line.Split(',');
                    jizhun = new Point(item[0], 0, 0, Convert.ToDouble(item[1]));
                    MyLog.Add("基准点高程H0," + jizhun.h);
                    //关键点id
                    string[] keysName = sr.ReadLine().Trim().Split(',');
                    //测试点AB
                    item = sr.ReadLine().Trim().Split(',');
                    ceshi[0] = new Point(item[0], Convert.ToDouble(item[1]), Convert.ToDouble(item[2]), 0);
                    item = sr.ReadLine().Trim().Split(',');
                    ceshi[1] = new Point(item[0], Convert.ToDouble(item[1]), Convert.ToDouble(item[2]), 0);
                    //实测点
                    sr.ReadLine();
                    while ((line=sr.ReadLine()) != null)
                    {
                        item = line.Trim().Split(',');
                        dt.Rows.Add(item);
                        double x = Convert.ToDouble(item[1]);
                        double y = Convert.ToDouble(item[2]);
                        double h = Convert.ToDouble(item[3]);
                        shiceList.Add(new Point(item[0], x, y, h));

                        foreach (string s in keysName)
                        {
                            if (s.Equals(item[0]))
                                keyList.Add(new Point(item[0], x, y, h));
                        }
                    }
                    foreach (Point p in keyList)
                    {
                        MyLog.Add("关键点" + p.id + "高程," + p.h);
                    }
                    key = keyList.ToArray();
                    shice = shiceList.ToArray();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region 保存
        public void SaveImage(Chart chart1)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "保存绘图";
            sfd.InitialDirectory = Environment.CurrentDirectory;
            sfd.Filter = "PNG|*.png";
            sfd.FileName = "chart";
            sfd.DefaultExt = "png";
            if (sfd.ShowDialog()!=DialogResult.OK)
            {
                return;
            }
            chart1.SaveImage(sfd.FileName, ChartImageFormat.Png);
        }

        public void SaveReport(string log)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "保存报告";
            sfd.InitialDirectory = Environment.CurrentDirectory;
            sfd.Filter = "文本文件|*.txt";
            sfd.FileName = "report";
            sfd.DefaultExt = "txt";
            if (sfd.ShowDialog()!=DialogResult.OK)
            {
                return;
            
            }
            using(StreamWriter sw=new StreamWriter(sfd.FileName))
            {
                sw.Write(log);
            }
        }
        #endregion
    }
}
