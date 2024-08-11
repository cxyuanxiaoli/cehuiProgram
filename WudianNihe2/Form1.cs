using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WudianNihe2
{
    public partial class Form1 : Form
    {
        DataCenter dc;
        Calculater cal;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "初始化完成";
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = "当前时间: " + DateTime.Now.ToString("yy-MM-dd  hh:mm:ss");
        }

        #region 工具条
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dc = new DataCenter();
            dc.ImportData();
            dataGridView1.DataSource = dc.dt;
            toolStripStatusLabel1.Text = "导入数据";
            if (dc.points == null)
                return;
            //绘制点
            chart1.Series[1].Points.Clear();
            chart1.Series[1].MarkerStyle = MarkerStyle.Circle;
            for (int i = 0; i < dc.points.Length; i++)
            {
                chart1.Series[1].Points.AddXY(dc.points[i].x, dc.points[i].y);
            }
            chart1.Legends[0].Enabled = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DrawCurve(true);

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DrawCurve(false);
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (dc != null)
                dc.SaveReport();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (dc != null)
                dc.SaveChart(chart1);
        }
        #endregion
        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="isClose">是否闭合</param>
        private void DrawCurve(bool isClose)
        {
            if (dc == null||dc.points==null)
                return;
            cal = new Calculater(dc.points);
            MyLog.Clear();
            List<Point> list = cal.GetResult(isClose);
            //绘制曲线
            chart1.Series[0].Points.Clear();
            foreach (Point item in list)
            {
                chart1.Series[0].Points.AddXY(item.x, item.y);
            }
            richTextBox1.Text = MyLog.Log;
            toolStripStatusLabel1.Text = "计算并绘制曲线";
        }

        #region 菜单栏
        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(null, null);
        }

        private void 保存报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton4_Click(null, null);
        }

        private void 保存图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton5_Click(null, null);
        }

        private void 闭合拟合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(null, null);
        }

        private void 不闭合拟合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton3_Click(null, null);
        }

        private void 数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void 绘图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void 报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
        
    }
}
