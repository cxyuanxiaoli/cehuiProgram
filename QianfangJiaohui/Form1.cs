using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QianfangJiaohui
{
    public partial class Form1 : Form
    {
        DataCenter dc;
        double[] result;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dc = new DataCenter();
            dc.ImportData();
            PhotoInfo p1 = dc.p[0], p2 = dc.p[1];
            if (p1==null||p2==null)
            {
                return;
            }
            Xs1.Text = p1.xs.ToString();
            Ys1.Text = p1.ys.ToString();
            Zs1.Text = p1.zs.ToString();
            o1.Text = p1._4.ToString();
            P1.Text = p1._w.ToString();
            q1.Text = p1._k.ToString();
            x1.Text = p1.x.ToString();
            y1.Text = p1.y.ToString();
            f1.Text = p1.f.ToString();

            Xs2.Text = p2.xs.ToString();
            Ys2.Text = p2.ys.ToString();
            Zs2.Text = p2.zs.ToString();
            o2.Text = p2._4.ToString();
            P2.Text = p2._w.ToString();
            q2.Text = p2._k.ToString();
            x2.Text = p2.x.ToString();
            y2.Text = p2.y.ToString();
            f2.Text = p2.f.ToString();

            u1.Text = "";
            v1.Text = "";
            w1.Text = "";
            u2.Text = "";
            v2.Text = "";
            w2.Text = "";
            N1.Text = "";
            N2.Text = "";
            X.Text =  "";
            Y.Text =  "";
            Z.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dc.p[0]==null)
            {
                return;
            }
            result=dc.Calculate();
            u1.Text = result[0].ToString();
            v1.Text = result[1].ToString();
            w1.Text = result[2].ToString();
            u2.Text = result[3].ToString();
            v2.Text = result[4].ToString();
            w2.Text = result[5].ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (result==null)
            {
                return;
            }
            N1.Text = result[6].ToString();
            N2.Text = result[7].ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (result == null)
            {
                return;
            }
            X.Text = result[8].ToString();
            Y.Text = result[9].ToString();
            Z.Text = result[10].ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
