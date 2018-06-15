using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMS
{
    public partial class PayMoney : Form
    {
        public static string str_arre = "";
        public PayMoney()
        {
            InitializeComponent();
        }
        public PayMoney(string arrearage)
            :this()
        {
            str_arre = arrearage;
        }

        private void PayMoney_Load(object sender, EventArgs e)
        {
            label1.Text = str_arre;
        }

        private void button1_Click(object sender, EventArgs e)
        {          
            if (textBox1.Text == label1.Text)
            {
                errorProvider1.Clear();
                this.Close();
                MessageBox.Show("缴费成功!");
            }
            else
            {
                errorProvider1.SetError(textBox1, "请输入正确的缴费金额");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {          
            this.Close();
        }
    }
}
