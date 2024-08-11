using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dadixian
{
    public partial class Form1 : Form
    {
        DataCenter dc;
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dc = new DataCenter();
            dc.ImportData();
            dataGridView1.DataSource = dc.dt;
            Console.WriteLine(MyLog.Log);
            if (dc.param!=null)
            {
                toolStripStatusLabel1.Text = "导入数据";
                toolStripStatusLabel3.Text = "椭球长半轴：" + dc.param.a + "扁率倒数：" + dc.param.f_1;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (dc==null)
            {
                return;
            }
            dc.Calculate();
            Console.WriteLine(MyLog.Log);
            richTextBox1.Text = MyLog.Log;
            tabControl1.SelectedIndex = 1;
            toolStripStatusLabel1.Text = "计算成功";
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (dc == null)
            {
                return;
            }
            dc.SaveReport();
            toolStripStatusLabel1.Text = "保存报告";
        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton1_Click(null, null);
        }

        private void 保存报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton3_Click(null, null);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
