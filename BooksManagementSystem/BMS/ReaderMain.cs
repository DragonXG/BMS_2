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
    public partial class ReaderMain : Form
    {
        static public string str_1 = "", str_2 = "";
        public ReaderMain()
        {
            InitializeComponent();
        }
        public ReaderMain(string text, string text_num)
            :this()
        {
            str_1 = text;
            str_2 = text_num;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            PwdChange Form = new PwdChange();
            Form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReaderInfo Form = new ReaderInfo();
            Form.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ReaderInfo readinfo = new ReaderInfo(label2.Text);
            readinfo.Show();
        }

        private void ReaderMain_Load(object sender, EventArgs e)
        {
            label2.Text = str_1;
            str_1 = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            BorrowHistory borrowhistory = new BorrowHistory(str_2);
            borrowhistory.Show();
        }
    }
}
