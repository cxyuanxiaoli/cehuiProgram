using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpatialExplore
{
    class DataCenter
    {
        //文件读取，输出类
        public Point[] points;
        public DataTable dt;

        #region 导入
        /// <summary>
        /// 导入数据
        /// </summary>
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
                List<Point> list = new List<Point>();
                dt = new DataTable();
                using (StreamReader sr=new StreamReader(ofd.FileName))
                {
                    string line = sr.ReadLine().Trim();
                    string[] items = line.Split(',');
                    dt.Columns.Add(items[0]);
                    dt.Columns.Add(items[1]);
                    dt.Columns.Add(items[2]);
                    dt.Columns.Add(items[3]);

                    while ((line = sr.ReadLine()) != null)
                    {
                        items = line.Trim().Split(',');
                        dt.Rows.Add(items);
                        double x = Convert.ToDouble(items[1]);
                        double y = Convert.ToDouble(items[2]);
                        int code = Convert.ToInt32(items[3]);
                        list.Add(new Point(items[0], x, y, code));
                        if (items[0].Equals("P6"))
                        {
                            MyLog.Add("P6的坐标x," + Math.Round(x, 3));
                            MyLog.Add("P6的坐标y," + Math.Round(y, 3));
                            MyLog.Add("P6的区号," + code);
                        }
                    }
                    points = list.ToArray();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 保存
        public void SaveReport(string log)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "保存报告";
            sfd.FileName = "result";
            sfd.DefaultExt = "txt";
            sfd.InitialDirectory = Environment.CurrentDirectory;
            sfd.Filter = "文本文件|*.txt";
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
