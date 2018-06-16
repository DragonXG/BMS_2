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
    public partial class QueryAfterLogin : Form
    {
        
        string id = "";
        string strnum = "";
        public QueryAfterLogin(string str)
        {
            InitializeComponent();
            strnum = str;
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
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
            //MessageBox.Show(s);
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

            //MessageBox.Show(strnum);

            string s = "";
            if (listView1.FocusedItem == null)
            {
                MessageBox.Show("请选择一本书！");
                return;
            }
            s = listView1.FocusedItem.SubItems[0].Text;
            String strConn = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";
            MySqlConnection conn = new MySqlConnection(strConn);
            conn.Open();

            String strSql2 = "select * from booking where BookID = '" + id + "'";

            MySqlDataAdapter da1 = new MySqlDataAdapter(strSql2, strConn);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1, "booking");
            if (ds1.Tables["booking"].Rows.Count != 0)
            {
                MessageBox.Show("此书已被预定，您不能再预定！");
            }
            else
            {
                String strSql3 = "Select * from booking where CardNum = '" + strnum + "'";
                MySqlDataAdapter da3 = new MySqlDataAdapter(strSql3, strConn);
                DataSet ds3 = new DataSet();
                int t;
                da3.Fill(ds3, "booking");
                //if(ds3 != null && ds3.Tables.Count > 0)
                
                t = ds3.Tables[0].Rows.Count;
                //MessageBox.Show(t);
                
                
                

                if (t < 3)
                {
                    //MessageBox.Show("未预定");
                    string time = DateTime.Now.ToString();
                    //MessageBox.Show(time);
                    //MessageBox.Show(s);
                    String strSql = "select * from tbookclass where BookClassID = '" + s + "'";

                    MySqlDataAdapter da = new MySqlDataAdapter(strSql, strConn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "tbookclass");
                    string name = "";
                    //MessageBox.Show(name+"---\n");
                    foreach (DataRow row in ds.Tables["tbookclass"].Rows)
                    {
                        name = row["BookName"].ToString();
                    }
                    //MessageBox.Show(name);
                    string strSqladd = "insert into booking values('" + id + "','" + name + "','" + strnum + "','" + time + "')";
                    MySqlCommand cmd = new MySqlCommand(strSqladd, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("《" + name + "》" + "预订成功！");
                }
                else
                {
                    MessageBox.Show("每人最多同时预定3本书，您已达上限！");
                }
            }

            conn.Close();
        }

        private void QueryAfterLogin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = "";
            if (listView2.FocusedItem == null)
            {
                MessageBox.Show("请选择一本书！");
                return;
            }
            DialogResult dr = MessageBox.Show("确定取消预定吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                s = listView2.FocusedItem.SubItems[0].Text;
                //MessageBox.Show(s);
                if (s != "")
                {
                    String strConn = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";
                    MySqlConnection conn = new MySqlConnection(strConn);
                    conn.Open();

                    String strSql1 = "select * from booking where BookID = '" + s + "'";

                    MySqlDataAdapter da = new MySqlDataAdapter(strSql1, strConn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "tbookclass");
                    string name = "";
                    //MessageBox.Show(name+"---\n");
                    foreach (DataRow row in ds.Tables["tbookclass"].Rows)
                    {
                        name = row["BookName"].ToString();
                    }

                    String strSql2 = "delete from booking where BookID = '" + s + "'";
                    MySqlCommand cmd = new MySqlCommand(strSql2, conn);
                    cmd.ExecuteNonQuery();
                    int d = 0;
                    d = this.listView2.SelectedItems[0].Index;
                    listView2.Items[d].Remove();
                    MessageBox.Show("《" + name + "》" + "取消预定成功！");

                    conn.Close();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            String strConn = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";
            MySqlConnection conn = new MySqlConnection(strConn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Select * from booking where CardNum = '" + strnum + "'";
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ListViewItem it = new ListViewItem();
                it.Text = dr["BookID"].ToString();
                it.SubItems.Add(dr["BookName"].ToString());
                it.SubItems.Add(dr["BookDate"].ToString());
                listView2.Items.Add(it);
            }

            conn.Close();
        }
    }
}
