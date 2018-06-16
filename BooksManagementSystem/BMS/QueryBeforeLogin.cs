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
    public partial class QueryBeforeLogin : Form
    {
        string s = "";
        string id = "";
        //string strnum = "";
        public QueryBeforeLogin()
        {
            InitializeComponent();
            //strnum = str1;
        }

        private void QueryBeforeLogin_Load(object sender, EventArgs e)
        {
            Timer time1 = new Timer();
            time1.Interval = 1000;
            time1.Tick += new System.EventHandler(timer1_Tick);
            timer1.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            String strConn = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";

            MySqlConnection conn = new MySqlConnection(strConn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Select * from tbookclass where BookName like '%" + textBox1.Text + "%' and BookAuthor like '%" + textBox4.Text + "%'";
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem it = new ListViewItem();
                it.Text = dr["BookClassID"].ToString();
                it.SubItems.Add(dr["BookName"].ToString());
                it.SubItems.Add(dr["BookAuthor"].ToString());
                it.SubItems.Add(dr["BookPress"].ToString());
                it.SubItems.Add(dr["BookClass"].ToString());
                listView1.Items.Add(it);
            }

            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s = "";
            if (listView1.FocusedItem == null)
            {
                MessageBox.Show("请选择一本书！");
                return;
            }
            s = listView1.FocusedItem.SubItems[0].Text;
            if (s != "")
            {

                String strConn = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";

                MySqlConnection conn = new MySqlConnection(strConn);
                conn.Open();
                String strSql = "Select * from bookinformation where BookClassID = '" + s + "'";

                MySqlDataAdapter da = new MySqlDataAdapter(strSql, strConn);
                DataSet ds = new DataSet();
                da.Fill(ds, "bookinformation");
                //string id = "";
                string sf = "";
                foreach (DataRow row in ds.Tables["bookinformation"].Rows)
                {
                    id = row["BookID"].ToString();
                    sf = row["SendFlag"].ToString();
                }
                String strSql0 = "Select * from recorder where BookID = '" + id + "'";
                MySqlDataAdapter da0 = new MySqlDataAdapter(strSql0, strConn);
                DataSet ds0 = new DataSet();
                da0.Fill(ds0, "recorder");
                string time1 = "";

                foreach (DataRow row in ds0.Tables["recorder"].Rows)
                {
                    time1 = row["BorrowDate"].ToString();
                }

                if (sf == "1")
                {
                    textBox2.Clear();
                    DateTime dt = Convert.ToDateTime(time1);
                    string time2 = dt.AddDays(30).ToString();
                    textBox2.AppendText("此书已被借阅。\r\n");
                    textBox2.AppendText("借阅时间：");
                    textBox2.AppendText(time1);
                    textBox2.AppendText("\r\n最晚归还时间：");
                    textBox2.AppendText(time2);
                }
                else
                {
                    textBox2.Clear();
                    textBox2.Text = "此书未被借阅。";
                }
                //MessageBox.Show(sf);

                String strSql2 = "select * from booking where BookID = '" + id + "'";

                MySqlDataAdapter da1 = new MySqlDataAdapter(strSql2, strConn);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1, "booking");
                if (ds1.Tables["booking"].Rows.Count != 0)
                {
                    textBox3.Clear();
                    textBox3.Text = "此书已被预定。";
                }
                else
                {
                    textBox3.Clear();
                    textBox3.Text = "此书未被预定。";
                }

                conn.Close();
            }
            else
            {
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*Login lg = new Login();
            string strnum = lg.StringValue;*/

            
            DialogResult dr = MessageBox.Show("您还未登录，是否返回登录界面","提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(dr == DialogResult.OK)
            {

                Program.checkin_login = true;
                this.Close();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToLongTimeString().ToString();
        }
    }
}
