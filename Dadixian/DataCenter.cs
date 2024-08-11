using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dadixian
{
    class DataCenter
    {
        public EarthParam param;
        public EarthLineInfo[] datas;
        public DataTable dt;

        #region 导入数据
        public void ImportData()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "导入数据";
            ofd.Filter = "文本文件（*.txt）|*.txt";
            ofd.InitialDirectory = Environment.CurrentDirectory;
            if (ofd.ShowDialog()!=DialogResult.OK)
            {
                return;
            }
            dt = new DataTable();
            dt.Columns.Add("起点名称");
            dt.Columns.Add("B1");
            dt.Columns.Add("L1");
            dt.Columns.Add("终点名称");
            dt.Columns.Add("B2");
            dt.Columns.Add("L2");
            try
            {
                using(StreamReader sr=new StreamReader(ofd.FileName))
                {
                    string line = sr.ReadLine().Trim();
                    string[] items = line.Split(',');
                    param = new EarthParam(Convert.ToDouble(items[0]), Convert.ToDouble(items[1]));

                    //椭球长半轴,扁率倒数，扁率
                    MyLog.Add("椭球长半轴," + param.a);
                    MyLog.Add("扁率倒数," + Math.Round(param.f_1,3));
                    MyLog.Add("扁率," + Math.Round(param.f, 8));
                    //椭球短半轴, 第一偏心率的平方，第二偏心率的平方
                    MyLog.Add("椭球短半轴," + Math.Round(param.b, 3));
                    MyLog.Add("第一偏心率的平方," + Math.Round(param.e_2, 8));
                    MyLog.Add("第二偏心率的平方," + Math.Round(param.e_12, 8));

                    sr.ReadLine();
                    List<EarthLineInfo> list = new List<EarthLineInfo>();
                    while ((line = sr.ReadLine()) != null)
                    {
                        items = line.Trim().Split(',');
                        dt.Rows.Add(items);
                        double b1 = Convert.ToDouble(items[1]);
                        double l1 = Convert.ToDouble(items[2]);
                        double b2 = Convert.ToDouble(items[4]);
                        double l2 = Convert.ToDouble(items[5]);
                        list.Add(new EarthLineInfo(items[0], b1, l1, items[3], b2, l2));
                    }

                    datas = list.ToArray();
                }
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
            sfd.Title = "保存报告";
            sfd.Filter = "文本文件(*.txt)|*.txt";
            sfd.InitialDirectory = Environment.CurrentDirectory;
            sfd.DefaultExt = "txt";
            sfd.FileName = "result";
            if (sfd.ShowDialog()!=DialogResult.OK)
            {
                return;
            }
            using(StreamWriter sw=new StreamWriter(sfd.FileName))
            {
                sw.Write(MyLog.Log);
            }
            MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region 计算
        public void Calculate()
        {
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i].CalculateLength(param);
            }
        }
        #endregion
    }
}
