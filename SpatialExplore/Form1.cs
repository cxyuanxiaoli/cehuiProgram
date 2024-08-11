using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpatialExplore
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

        #region 菜单栏
        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dc = new DataCenter();
            dc.ImportData();
            dataGridView1.DataSource = dc.dt;
            richTextBox1.Text = MyLog.Log;
            if (dc.points!=null)
            {
                cal = new Calculate(dc.points);
                for (int i = 0; i < dc.points.Length; i++)
                {
                    chart1.Series[0].Points.AddXY(dc.points[i].x, dc.points[i].y);
                }
                toolStripStatusLabel1.Text = "导入数据";

            }
        }

        private void 保存报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dc!=null)
            {
                dc.SaveReport(MyLog.Log);
                toolStripStatusLabel1.Text = "保存报告";
            }
        }

        private void 探索性空间分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(null, null);
        }

        private void 生成空间权重矩阵ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton3_Click(null, null);
        }

        private void 全局莫兰指数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton4_Click(null, null);

        }

        private void 局部莫兰指数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton5_Click(null, null);

        }

        private void 数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void 报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;

        }

        private void 绘图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;

        }

        private void 空间权重矩阵ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;

        }
#endregion

        #region 工具条
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            导入数据ToolStripMenuItem_Click(null, null);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (cal == null)
                return;
            cal.ExploreAnalyse();
            richTextBox1.Text = MyLog.Log;
            toolStripStatusLabel1.Text = "空间探索分析完成，生成报告";


        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (cal == null||cal.areaData==null)
                return;
            dataGridView2.DataSource = cal.SWMatrix();
            richTextBox1.Text = MyLog.Log;
            toolStripStatusLabel1.Text = "生成空间权重矩阵";

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (cal == null||cal.swMatrix==null)
                return;
            cal.Moran_I();
            richTextBox1.Text = MyLog.Log;
            toolStripStatusLabel1.Text = "全局莫兰指数计算完成";

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (cal == null||cal.swMatrix==null)
                return;
            cal.LocalMoran();
            richTextBox1.Text = MyLog.Log;
            toolStripStatusLabel1.Text = "局部莫兰指数计算完成";

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            保存报告ToolStripMenuItem_Click(null, null);
        }
#endregion
        

        
    }
}
