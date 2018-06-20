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
using System.IO;


namespace BMS
{
    public partial class QueryAfterLogin : Form
    {
        
        string id = "";
        string strnum = "";
        string time0 = DateTime.Now.ToString();
        string name = "";
        static private bool ts;
        static public string bookname;
        Timer mytime = new Timer();
        

        public QueryAfterLogin(string str)
        {
            InitializeComponent();
            strnum = str;
            mytime.Tick += new EventHandler(timer2_Tick);
            mytime.Enabled = true;
            mytime.Interval = 1000;
            mytime.Start();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            try
            {
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

                    String strSql = "Select * from bookinformation where BookClassID = '" + it.Text + "'";
                    MySqlDataAdapter da = new MySqlDataAdapter(strSql, strConn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "bookinformation");
                    //string id = "";
                    foreach (DataRow row in ds.Tables["bookinformation"].Rows)
                    {
                        id = row["BookID"].ToString();
                    }
                    it.SubItems.Add(id);

                    it.SubItems.Add(dr["BookName"].ToString());
                    it.SubItems.Add(dr["BookAuthor"].ToString());
                    it.SubItems.Add(dr["BookPress"].ToString());
                    it.SubItems.Add(dr["BookClass"].ToString());
                    listView1.Items.Add(it);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
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

            if (this.listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请选择一本书！");
                return;
            }

            s = listView1.FocusedItem.SubItems[0].Text;
            //MessageBox.Show(s);
            if (s != "")
            {
                try
                {
                    String strConn = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";
                    MySqlConnection conn = new MySqlConnection(strConn);
                    conn.Open();

                    String strSql11 = "select * from tbookclass where BookClassID = '" + s + "'";

                    MySqlDataAdapter da11 = new MySqlDataAdapter(strSql11, strConn);
                    DataSet ds11 = new DataSet();
                    da11.Fill(ds11, "tbookclass");
                    //string name = "";
                    //MessageBox.Show(name+"---\n");
                    foreach (DataRow row in ds11.Tables["tbookclass"].Rows)
                    {
                        name = row["BookName"].ToString();
                    }

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

                    textBox5.Text = id + "《" + name + "》";

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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
                }
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

            if (this.listView1.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请选择一本书！");
                return;
            }


            s = listView1.FocusedItem.SubItems[0].Text;
            //MessageBox.Show(s);
            if (s != "")
            {
                try
                {
                    String strConn1 = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";
                    MySqlConnection conn1 = new MySqlConnection(strConn1);
                    conn1.Open();
                    String strSql = "Select * from bookinformation where BookClassID = '" + s + "'";

                    MySqlDataAdapter da = new MySqlDataAdapter(strSql, strConn1);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "bookinformation");
                    //string id = "";
                    foreach (DataRow row in ds.Tables["bookinformation"].Rows)
                    {
                        id = row["BookID"].ToString();
                    }
                    conn1.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
                }
            }


            s = listView1.FocusedItem.SubItems[0].Text;
            try
            {
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

                        time0 = time;
                        //MessageBox.Show(time0);

                        //MessageBox.Show(s);

                        String strSql = "select * from tbookclass where BookClassID = '" + s + "'";

                        MySqlDataAdapter da = new MySqlDataAdapter(strSql, strConn);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "tbookclass");
                        //string name = "";
                        //MessageBox.Show(name+"---\n");
                        foreach (DataRow row in ds.Tables["tbookclass"].Rows)
                        {
                            name = row["BookName"].ToString();
                        }
                        //MessageBox.Show(name);
                        string strSqladd = "insert into booking values('" + id + "','" + name + "','" + strnum + "','" + time + "')";
                        MySqlCommand cmd = new MySqlCommand(strSqladd, conn);
                        cmd.ExecuteNonQuery();


                        /*DateTime dt = Convert.ToDateTime(time);
                        //string time2 = dt.AddDays(30).ToString();
                        string time2 = dt.AddSeconds(10).ToString();
                        if(time0 == time2)
                        {
                            String strSqlt = "delete from booking where BookDate = '" + time + "'";
                            MySqlCommand cmdt = new MySqlCommand(strSqlt, conn);
                            cmdt.ExecuteNonQuery();
                        }*/

                        //QueryAfterLogin_MouseMove(sender, e);

                        //MessageBox.Show("《" + name + "》" + "预订成功！");
                        MessageBox.Show("《" + name + "》" + "预订成功！\r\n请在30天内借阅。");

                        string strlog = strnum + "预定" + "《" + name + "》" + "成功！" + "\n";
                        Log.WriteLog(strlog);
                        /*Logmaintain logmain = new Logmaintain();
                        logmain.ShowDialog();*/
                    }
                    else
                    {
                        MessageBox.Show("每人最多同时预定3本书，您已达上限！");
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = "";
            if (listView2.FocusedItem == null)
            {
                MessageBox.Show("请选择一本书！");
                return;
            }

            int d = 0;
            if(this.listView2.SelectedItems.Count<=0)
            {
                MessageBox.Show("请选择一本书！");
                return;
            }

            DialogResult dr = MessageBox.Show("确定取消预定吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                s = listView2.FocusedItem.SubItems[0].Text;
                //MessageBox.Show(s);

                d = this.listView2.SelectedItems[0].Index;
                listView2.Items[d].Remove();

                if (s != "")
                {
                    try
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

                        /*int d = 0;
                        d = this.listView2.SelectedItems[0].Index;
                        listView2.Items[d].Remove();*/

                        //listView2.Clear();
                        MessageBox.Show("《" + name + "》" + "取消预定成功！");

                        string strlog = strnum + "取消预定" + "《" + name + "》" + "成功！" + "\n";
                        Log.WriteLog(strlog);

                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(time0);

            listView2.Items.Clear();
            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }

        private void QueryAfterLogin_Load(object sender, EventArgs e)
        {
            Timer time1 = new Timer();
            time1.Interval = 1000;
            time1.Tick += new System.EventHandler(timer1_Tick);
            timer1.Start();

            Timer time2 = new Timer();
            time2.Interval = 1000;
            time2.Start();
            

            /*DateTime dt = Convert.ToDateTime(time);
            //string time2 = dt.AddDays(30).ToString();
            string time2 = dt.AddSeconds(10).ToString();
            if (time0 == time2)
            {
                String strSqlt = "delete from booking where BookDate = '" + time + "'";
                MySqlCommand cmdt = new MySqlCommand(strSqlt, conn);
                cmdt.ExecuteNonQuery();
            }*/

            /*label4.Text = DateTime.Now.ToLongTimeString().ToString();
            Application.DoEvents();
            System.Threading.Thread.Sleep(100);*/

            /*String strConn = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";
            MySqlConnection conn = new MySqlConnection(strConn);
            conn.Open();

            DateTime dt = Convert.ToDateTime(time0);
            //string time2 = dt.AddDays(30).ToString();
            string time2 = dt.AddSeconds(10).ToString();
            if (time0 == time2)
            {
                String strSql2 = "delete from booking where BookDate = '" + time0 + "'";
                MySqlCommand cmd = new MySqlCommand(strSql2, conn);
                cmd.ExecuteNonQuery();
            }
            conn.Close();*/

            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToLongTimeString().ToString();

            /*if (listView1.FocusedItem == null)
            {
                return;
            }
            if (this.listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            //button3_Click(sender, e);

            String strConn = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";
            MySqlConnection conn = new MySqlConnection(strConn);
            conn.Open();

            DateTime dt = Convert.ToDateTime(time0);
            //string time2 = dt.AddDays(30).ToString();
            string time2 = dt.AddSeconds(10).ToString();
            if(time0 == time2)
            {
                String strSql2 = "delete from booking where BookDate = '" + time0 + "'";
                MySqlCommand cmd = new MySqlCommand(strSql2, conn);
                cmd.ExecuteNonQuery();
            }
            conn.Close();*/
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            //button3_Click(sender, e);

            if (name == "")
            {
                return;
            }

            try
            {
                String strConn = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";
                MySqlConnection conn = new MySqlConnection(strConn);
                conn.Open();

                DateTime dt = Convert.ToDateTime(time0);

                /*string time2 = dt.AddDays(30).ToString();
                string time2 = dt.AddSeconds(10).ToString();
                if (time0 == time2)
                {
                    String strSql2 = "delete from booking where BookDate = '" + time0 + "'";
                    MySqlCommand cmd = new MySqlCommand(strSql2, conn);
                    cmd.ExecuteNonQuery();
                }*/

                DateTime time3 = DateTime.Now;
                TimeSpan t = time3 - dt;
                int day = t.Days;
                int sec = t.Seconds;
                if (day == 30)
                //if (sec == 10)
                {
                    String strSql3 = "select * from booking where BookID = '" + id + "'";

                    MySqlDataAdapter da1 = new MySqlDataAdapter(strSql3, strConn);
                    DataSet ds1 = new DataSet();
                    da1.Fill(ds1, "booking");
                    if (ds1.Tables["booking"].Rows.Count != 0)
                    {
                        String strSql2 = "delete from booking where BookDate = '" + time0 + "'";
                        MySqlCommand cmd = new MySqlCommand(strSql2, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("《" + name + "》" + "预定失效！");
                        button6_Click(sender, e);

                        string strlog = strnum + "预定" + "《" + name + "》" + "失效！" + "\n";
                        Log.WriteLog(strlog);

                        return;
                    }

                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
            
        }

        private void QueryAfterLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                button5.PerformClick();
            }
        }

        /*private void QueryAfterLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if(name == "")
            {
                return;
            }
            
            String strConn = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";
            MySqlConnection conn = new MySqlConnection(strConn);
            conn.Open();

            DateTime dt = Convert.ToDateTime(time0);

            string time2 = dt.AddDays(30).ToString();
            string time2 = dt.AddSeconds(10).ToString();
            if (time0 == time2)
            {
                String strSql2 = "delete from booking where BookDate = '" + time0 + "'";
                MySqlCommand cmd = new MySqlCommand(strSql2, conn);
                cmd.ExecuteNonQuery();
            }

            DateTime time3 = DateTime.Now;
            TimeSpan t = time3 - dt;
            int day = t.Days;
            int sec = t.Seconds;
            //if (day > 30)
            if (sec > 10)
            {
                String strSql2 = "delete from booking where BookDate = '" + time0 + "'";
                MySqlCommand cmd = new MySqlCommand(strSql2, conn);
                cmd.ExecuteNonQuery();
                //MessageBox.Show("《" + name + "》" + "预定失效！");
                button6_Click(sender, e);
                return;
            }

            conn.Close();
        
        }*/
    }
}
