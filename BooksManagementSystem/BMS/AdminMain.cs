using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BMS
{
    public partial class AdminMain : Form
    {
        static public string AdminName="";
        static public int Authority=0;

        public AdminMain()
        {
            InitializeComponent();

        }

        public AdminMain(string text_name)
            :this()
        {
            AdminName = text_name;
            open_mysql_llm.conn.Open();
            DataSet dsmydata = new DataSet();
            MySqlCommand cmd = new MySqlCommand("select * from administrator where LoginName ='" + AdminName + "'", open_mysql_llm.conn);
            MySqlDataAdapter da1 = new MySqlDataAdapter();
            da1.SelectCommand = cmd;
            da1.Fill(dsmydata, "administrator");
            open_mysql_llm.conn.Close();
            if (dsmydata.Tables[0].Rows[0]["Power"].ToString() == "系统管理员")
            {
                Authority = 1;
            }
            else if (dsmydata.Tables[0].Rows[0]["Power"].ToString() == "图书管理员")
            {
                Authority = 2;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

            if (Authority == 1)
            {
                MessageBox.Show("您没有权限查看此部分");
                return;
            }

            BookManage form = new BookManage();
            form.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            if (Authority == 2)
            {
                MessageBox.Show("您没有权限查看此部分");
                return;
            }
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

        private void AdminMain_Load(object sender, EventArgs e)
        {

        }
        private void toolStrip1_Paint(object sender, PaintEventArgs e)
        {
            if ((sender as ToolStrip).RenderMode == ToolStripRenderMode.System)
            {
                Rectangle rect = new Rectangle(0, 0, this.toolStrip1.Width, this.toolStrip1.Height - 2);
                e.Graphics.SetClip(rect);
            }
        }

        private void 关于此软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.Show();
        }  
    }
}
