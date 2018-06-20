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
                string success = "缴费成功!";
                MessageBox.Show(success);
                PayForOverdue PayOver = new PayForOverdue();
                PayOver.Success(success);
                get_number_llm.become_ture = 1;
                /************日志*********************/
                string logstring = "缴费成功." + "\n";
                Log.WriteLog(logstring);
            }
            else
            {
                errorProvider1.SetError(textBox1, "请输入正确的缴费金额");
                /************日志*********************/
                string logstring = "缴费失败." + "\n";
                Log.WriteLog(logstring);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {          
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text == label1.Text)
            {
                /************日志*********************/
                string logstring = "输入正确的缴费金额!" + "\n";
                Log.WriteLog(logstring);
            }
            else
            {
                /************日志*********************/
                string logstring = "输入错误的缴费金额!" + "\n";
                Log.WriteLog(logstring);
            }
        }
    }
}
