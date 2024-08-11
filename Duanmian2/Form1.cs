using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Duanmian2
{
    public partial class Form1 : Form
    {
        DataCenter dc;
        Calculate cal;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "初始化完成";
        }

        #region 工具条
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dc = new DataCenter();
            dc.ImportData();
            dataGridView1.DataSource = dc.dt;
            richTextBox1.Text = MyLog.Log;
            //绘图
            if (dc.shice != null)
            {
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
                foreach (Point item in dc.shice)
                {
                    chart1.Series[0].Points.AddXY(item.x, item.y);
                }
                foreach (Point item in dc.key)
                {
                    chart1.Series[1].Points.AddXY(item.x, item.y);
                }
                chart1.Legends[0].Enabled = true;
                cal = new Calculate(dc.jizhun, dc.ceshi, dc.key, dc.shice);
                toolStripStatusLabel1.Text = "导入数据并完成实测点绘图";

            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (dc!=null&&cal!=null)
            {
                cal.BaseCalculate();
                richTextBox1.Text = MyLog.Log;
                toolStripStatusLabel1.Text = "完成基本计算并生成报告";

            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (dc != null && cal != null)
            {
                Point[] res=cal.Zongduanmian();
                richTextBox1.Text = MyLog.Log;
                chart1.Series[2].Points.Clear();
                foreach (Point item in res)
                {
                    chart1.Series[2].Points.AddXY(item.x, item.y);
                }
                toolStripStatusLabel1.Text = "完成纵断面计算并绘制纵断面内插点";

            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (dc != null && cal != null)
            {
                List<Point[]> res = cal.Hengduanmian();
                Point[] m0 = res[0];
                Point[] m1 = res[1];
                richTextBox1.Text = MyLog.Log;
                chart1.Series[3].Points.Clear();
                chart1.Series[4].Points.Clear();
                for (int i = 0; i < m0.Length; i++)
                {
                    chart1.Series[3].Points.AddXY(m0[i].x, m0[i].y);
                    chart1.Series[4].Points.AddXY(m1[i].x, m1[i].y);
                }
                toolStripStatusLabel1.Text = "完成横断面计算并绘制横断面内插线";
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (dc!=null)
            {
                dc.SaveReport(MyLog.Log);
                toolStripStatusLabel1.Text = "保存报告";

            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (dc != null)
            {
                dc.SaveImage(chart1);
                toolStripStatusLabel1.Text = "保存绘图";
            }
        }
#endregion
        
        #region 菜单栏
        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(null, null);
        }

        private void 保存报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton5_Click(null, null);
        }

        private void 保存绘图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton6_Click(null, null);
        }

        private void 基本计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(null, null);
        }

        private void 纵断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton3_Click(null, null);
        }

        private void 横断面计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton4_Click(null, null);
        }
        #endregion
    }
}
