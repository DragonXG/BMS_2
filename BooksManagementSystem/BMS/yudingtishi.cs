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
    public partial class yudingtishi : Form
    {
        public yudingtishi(string bookname)
        {
            InitializeComponent();

            label2.Text = bookname;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BorrowReturn t = new BorrowReturn();
            t.Show();

            this.Close();
        }
    }
}
