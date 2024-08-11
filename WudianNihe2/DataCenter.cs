using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WudianNihe2
{
    class DataCenter
    {
        public Point[] points;
        public DataTable dt;

        #region 导入数据
        public void ImportData()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "导入离散点数据";
            ofd.Filter = "文本数据（*.txt）|*.txt";
            ofd.InitialDirectory = Environment.CurrentDirectory;
            if (ofd.ShowDialog()!=DialogResult.OK)
            {
                return;
            }
            dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("x");
            dt.Columns.Add("y");
            try
            {
                List<Point> list = new List<Point>();
                using(StreamReader sr=new StreamReader(ofd.FileName))
                {
                    string line;
                    string[] items;
                    while ((line = sr.ReadLine()) != null)
                    {
                        items = line.Trim().Split(',');
                        dt.Rows.Add(items);
                        double x = Convert.ToDouble(items[1]);
                        double y = Convert.ToDouble(items[2]);
                        list.Add(new Point(items[0], x, y));
                    }
                }
                points = list.ToArray();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
        #endregion

        #region 保存报告
        public void SaveReport()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "保存计算报告";
            sfd.Filter = "文本文件（*.txt）|*.txt";
            sfd.FileName = "result";
            sfd.DefaultExt = "txt";
            sfd.InitialDirectory = Environment.CurrentDirectory;
            if (sfd.ShowDialog()!=DialogResult.OK)
            {
                return;
            }
            using (StreamWriter sw = new StreamWriter(sfd.FileName))
            {
                sw.Write(MyLog.Log);
            }
            MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
        #region 保存图片
        public void SaveChart(Chart chart)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "保存图片";
            sfd.Filter = "PNG|*.png|JEPG|*.jpg";
            sfd.FileName = "chart1";
            sfd.DefaultExt = "png";
            sfd.InitialDirectory = Environment.CurrentDirectory;
            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string ex=Path.GetExtension(sfd.FileName);
            if (ex==".png")
            {
                chart.SaveImage(sfd.FileName, ChartImageFormat.Png);
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (ex == ".jpg")
            {
                chart.SaveImage(sfd.FileName, ChartImageFormat.Jpeg);
                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        #endregion
    }
}
