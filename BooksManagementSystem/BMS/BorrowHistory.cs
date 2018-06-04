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
    public partial class BorrowHistory : Form
    {
        public BorrowHistory()
        {
            InitializeComponent();
        }
        public BorrowHistory(string text)
            :this()
        {
            textBox1.Text = text;
        }
        private void BorrowHistory_Load(object sender, EventArgs e)
        {
            String str = "Server=localhost;Database=bms;Uid=root;password=123456;sslmode=none;";
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from reader;", conn);
            MySqlDataReader borrow_info;
            string str1 = "";
            str1 = textBox1.Text;
            textBox1.ReadOnly = true;
            borrow_info = cmd.ExecuteReader();
            while(borrow_info.Read())
            {
                
                if(str1 == Convert.ToString(borrow_info["CardNum"]))
                {
                    textBox2.Text = Convert.ToString(borrow_info["ReaderName"]);
                    textBox2.ReadOnly = true;
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            String str = "Server=localhost;Database=bms;Uid=root;password=123456;sslmode=none;";
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            string cardnum = textBox1.Text;
            MySqlCommand cmd_1 = new MySqlCommand("select BookID, BookName, BorrowDate, BorrowingStatus from recorder where CardNum = '" + cardnum + "' union select BookID, BookName, BorrowDate, BorrowingStatus from returnedbook where CardNum = '" + cardnum + "' order by BorrowDate ASC;", conn);
            DataTable dataread = new DataTable();
            dataread.Load(cmd_1.ExecuteReader());
            dataGridView1.DataSource = dataread;
            //设置dataGridView1控件第一列的列头文字
            dataGridView1.Columns[0].HeaderText = "图书ID";
            //设置dataGridView1控件第一列的列宽
            dataGridView1.Columns[0].Width = 100;
            //设置dataGridView1控件第二列的列头文字
            dataGridView1.Columns[1].HeaderText = "图书名";
            //设置dataGridView1控件第二列的列宽
            dataGridView1.Columns[1].Width = 200;

            //设置dataGridView1控件第三列的列头文字
            dataGridView1.Columns[2].HeaderText = "借阅日期";
            //设置dataGridView1控件第三列的列宽
            dataGridView1.Columns[2].Width = 200;

            //设置dataGridView1控件第四列的列头文字
            dataGridView1.Columns[3].HeaderText = "借阅状态";
            //设置dataGridView1控件第四列的列宽
            dataGridView1.Columns[3].Width = 200;
            conn.Close();
            dataGridView1.ReadOnly = true;
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            String str = "Server=localhost;Database=bms;Uid=root;password=123456;sslmode=none;";
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            string  cardnum = textBox1.Text;
            MySqlCommand cmd = new MySqlCommand("select BookID, BookName, BorrowDate, BorrowingStatus from recorder where CardNum = '" + cardnum + "';", conn);
            DataTable dataread = new DataTable();
            dataread.Load(cmd.ExecuteReader());
            dataGridView1.DataSource = dataread;

            //设置dataGridView1控件第一列的列头文字
            dataGridView1.Columns[0].HeaderText = "图书ID";
            //设置dataGridView1控件第一列的列宽
            dataGridView1.Columns[0].Width = 100;

            //设置dataGridView1控件第二列的列头文字
            dataGridView1.Columns[1].HeaderText = "图书名";
            //设置dataGridView1控件第二列的列宽
            dataGridView1.Columns[1].Width = 200;

            //设置dataGridView1控件第三列的列头文字
            dataGridView1.Columns[2].HeaderText = "借阅日期";
            //设置dataGridView1控件第三列的列宽
            dataGridView1.Columns[2].Width = 200;

            //设置dataGridView1控件第四列的列头文字
            dataGridView1.Columns[3].HeaderText = "借阅状态";
            //设置dataGridView1控件第四列的列宽
            dataGridView1.Columns[3].Width = 200;
            conn.Close();
            dataGridView1.ReadOnly = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            String str = "Server=localhost;Database=bms;Uid=root;password=123456;sslmode=none;";
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            string cardnum = textBox1.Text;
            MySqlCommand cmd = new MySqlCommand("select BookID,BookName,BorrowDate,Borrowingstatus from returnedbook where CardNum = '" + cardnum + "';", conn);
            DataTable dataread = new DataTable();
            dataread.Load(cmd.ExecuteReader());
            dataGridView1.DataSource = dataread;
            //设置dataGridView1控件第一列的列头文字
            dataGridView1.Columns[0].HeaderText = "图书ID";
            //设置dataGridView1控件第一列的列宽
            dataGridView1.Columns[0].Width = 100;

            //设置dataGridView1控件第二列的列头文字
            dataGridView1.Columns[1].HeaderText = "图书名";
            //设置dataGridView1控件第二列的列宽
            dataGridView1.Columns[1].Width = 200;

            //设置dataGridView1控件第三列的列头文字
            dataGridView1.Columns[2].HeaderText = "还书日期";
            //设置dataGridView1控件第三列的列宽
            dataGridView1.Columns[2].Width = 200;

            //设置dataGridView1控件第四列的列头文字
            dataGridView1.Columns[3].HeaderText = "借阅状态";
            //设置dataGridView1控件第四列的列宽
            dataGridView1.Columns[3].Width = 200;
            conn.Close();
            dataGridView1.ReadOnly = true;

        }

        private void 图书ID(object sender, DataGridViewColumnEventArgs e)
        {

        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow r = this.dataGridView1.Rows[i];
                r.HeaderCell.Value = string.Format("{0}", i + 1);
            }
            this.dataGridView1.Refresh();
        }

    }
}
