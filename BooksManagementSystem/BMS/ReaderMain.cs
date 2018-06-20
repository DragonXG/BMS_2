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
    public partial class ReaderMain : Form
    {
        static public string str_cardname = "", str_cardnum = "";
        static private bool ts;
        static public string bookname;
        static private bool DeleteFlag;
        static private string DeleteMessage;
        public ReaderMain()
        {
            InitializeComponent();
        }
        public ReaderMain(string text_name, string text_num)
            :this()
        {
            str_cardname = text_name;
            str_cardnum = text_num;
            DeleteFlag = false;
            ts = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /************日志*********************/
            string strlog = "进入修改密码界面." + "\n";
            Log.WriteLog(strlog);
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
            /************日志*********************/

            string logstring = "借阅证号：" + str_cardnum +"\t"+ "读者：" + label2.Text + "查询了自己的信息." + "\n";

            Log.WriteLog(logstring);
            readinfo.Show();
        }

        public void ReaderMain_Load2()
        {
            
        }
        
        private void ReaderMain_Load(object sender, EventArgs e)
        {
            label2.Text = str_cardname;
            str_cardname = "";
            //ReaderMain_Load2();
            Timer time1 = new Timer();
            time1.Interval = 1000;
            timer1.Start();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            BorrowHistory borrowhistory = new BorrowHistory(str_cardnum);
            /************日志*********************/

            string logstring = "借阅证号：" + str_cardnum + "\t"+"读者：" + label2.Text + "进入了个人图书信息页面." + "\n";

            Log.WriteLog(logstring);
            borrowhistory.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PayForOverdue form = new PayForOverdue(str_cardnum);
            /************日志*********************/

            string logstring = "借阅证号：" + str_cardnum + "\t" + "读者：" + label2.Text + "进入了缴费页面." + "\n";

            Log.WriteLog(logstring);
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BorrowReturn borrowreturn = new BorrowReturn();
            borrowreturn.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string strlog = "进入预定图书界面." + "\n";
            Log.WriteLog(strlog);
            QueryAfterLogin form = new QueryAfterLogin(str_cardnum);
            form.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            /*string ts1 = ts.ToString();
            MessageBox.Show(ts1);
            string deleteflag1 = DeleteFlag.ToString();
            MessageBox.Show(deleteflag1);*/

            if (ts == false && DeleteFlag == false)
            {
                MessageBox.Show("暂无消息。");
            }
            else
            {
                if (ts == true)
                {
                    yudingtishi form = new yudingtishi(bookname);
                    form.ShowDialog();
                }
                if (DeleteFlag == true)
                {
                    MessageBox.Show(DeleteMessage);
                    DeleteFlag = false;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //删书提示
                open_mysql_llm.conn.Open();
                DataSet dsmydata = new DataSet();
                MySqlCommand cmd = new MySqlCommand("select * from systemprompt where CardNum ='" + str_cardnum + "'", open_mysql_llm.conn);
                MySqlDataAdapter da1 = new MySqlDataAdapter();
                da1.SelectCommand = cmd;
                da1.Fill(dsmydata, "systemprompt");
                foreach (DataRow row in dsmydata.Tables["systemprompt"].Rows)
                {
                    if (row[0].ToString() == str_cardnum)
                    {
                        DeleteMessage = row[1].ToString();
                        DeleteFlag = true;
                    }
                }
                MySqlCommand cmd3 = new MySqlCommand();
                cmd3.Connection = open_mysql_llm.conn;
                cmd3.CommandText = "delete from systemprompt where CardNum = '" + str_cardnum + "'";
                cmd3.ExecuteNonQuery();
                open_mysql_llm.conn.Close();

                //预定提示
                string strnum = str_cardnum;
                String strConn = "Server = localhost;Database = bms;Uid = root;password = 123456;sslmode = none;";
                MySqlConnection conn = new MySqlConnection(strConn);
                conn.Open();
                String strSql3 = "Select * from booking where CardNum = '" + strnum + "'";
                MySqlDataAdapter da3 = new MySqlDataAdapter(strSql3, strConn);
                DataSet ds3 = new DataSet();
                int t;
                da3.Fill(ds3, "booking");
                //if(ds3 != null && ds3.Tables.Count > 0)
                string[] bookids = { "", "", "" };
                string[] booknames = { "", "", "" };
                bookname = "";
                ts = false;

                t = ds3.Tables[0].Rows.Count;
                if (t > 0)
                {
                    for (int i = 0; i < t; i++)
                    {
                        //foreach (DataRow row in ds3.Tables["booking"].Rows)

                        bookids[i] = ds3.Tables[0].Rows[i]["BookID"].ToString();
                        booknames[i] = ds3.Tables[0].Rows[i]["BookName"].ToString();


                        String strSql = "Select * from bookinformation where BookID = '" + bookids[i] + "'";
                        MySqlDataAdapter da = new MySqlDataAdapter(strSql, strConn);
                        DataSet ds = new DataSet();
                        da.Fill(ds, "bookinformation");
                        string sf = "";
                        foreach (DataRow row in ds.Tables["bookinformation"].Rows)
                        {
                            sf = row["SendFlag"].ToString();
                        }

                        if (sf == "0")
                        {
                            bookname += bookids[i] + "《" + booknames[i] + "》\r\n";
                            ts = true;
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                open_mysql_llm.conn.Close();
                MessageBox.Show(ex.Message.ToString());
                Log.WriteLog(ex.Message.ToString());
            }
            if (ts == false && DeleteFlag == false)
            {
                button7.BackColor = Color.White;
            }
            else
            {
                string change1 = DateTime.Now.Millisecond.ToString();
                int change2 = Convert.ToInt32(change1);
                if (change2 < 500)
                {
                    button7.BackColor = Color.Red;
                }
                else
                {
                    button7.BackColor = Color.White;
                }
            }
        }

    }
}
