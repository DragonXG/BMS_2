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
    public partial class AdminMain : Form
    {
        public AdminMain()
        {
            InitializeComponent();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            BookManage form = new BookManage();
            form.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReaderManage form = new ReaderManage();
            form.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            BorrowHistory borrowForm = new BorrowHistory();
            borrowForm.ShowDialog();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            BorrowReturn form = new BorrowReturn();
            form.ShowDialog();
        }
    }
}
