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
    public partial class ReaderManage : Form
    {
        public string CardNum;
        public string ReaderName;
        public string College;
        public string Profession;
        public string TelNumber;
        public string LodinKey;
        public string ReaderType;

        public ReaderManage()
        {
            InitializeComponent();
            Clearstrings();
        }

        private void Clearstrings()
        {
            /*textBox6.Text = "";
            textBox5.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            textBox2.Text = "";
            textBox1.Text = "";
            textBox14.Text = "";*/

            textBox6.Text = "2016080808";
            textBox5.Text = "张三";
            textBox4.Text = "成都信息工程大学";
            textBox3.Text = "大数据";
            textBox2.Text = "13267678989";
            textBox1.Text = "111111";
            textBox14.Text = "本科生";
        }

        private void Clearstrings2()
        {
            textBox6.Text = "";
            textBox5.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            textBox2.Text = "";
            textBox1.Text = "";
            textBox14.Text = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReaderManage_Load(object sender, EventArgs e)
        {
            Clearstrings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clearstrings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CardNum = textBox6.Text;
            ReaderName = textBox5.Text;
            College = textBox4.Text;
            Profession = textBox3.Text;
            TelNumber = textBox2.Text;
            LodinKey = textBox1.Text;
            ReaderType = textBox14.Text;
            if(CardNum == "")
            {
                MessageBox.Show("借阅证号不能为空");
                return;
            }
            if(ReaderName == "")
            {
                MessageBox.Show("读者姓名不能为空");
                return;
            }
            if (LodinKey == "")
            {
                MessageBox.Show("登录密码不能为空");
                return;
            }
            if(LodinKey.Length != 6)
            {
                MessageBox.Show("登录密码必须为6位数字!");
                return;
            }
            if (ReaderType == "")
            {
                MessageBox.Show("用户类型不能为空");
                return;
            }
            try
            {
                open_mysql_llm.conn.Open();

                DataSet dsmydata = new DataSet();
                //将信息填入tbookclass表中并将其显示在DataGridView中
                string Commandstring = "Insert into reader(CardNum,ReaderName,College,Profession,TelNumber,Lodinkey,ReaderType)Values('" +
                    CardNum + "','" + ReaderName + "','" + College + "','" + Profession + "','" +
                    TelNumber + "','" + LodinKey + "','" + ReaderType + "')";
                MySqlDataAdapter darecorder = new MySqlDataAdapter(Commandstring, open_mysql_llm.conn);
                MySqlCommandBuilder bdrecorder = new MySqlCommandBuilder(darecorder);
                darecorder.Fill(dsmydata, "reader");

                MySqlCommand cmd = new MySqlCommand("select * from reader", open_mysql_llm.conn);
                MySqlDataAdapter da1 = new MySqlDataAdapter();
                dsmydata = new DataSet();
                da1.SelectCommand = cmd;
                da1.Fill(dsmydata, "reader");

                dataGridView1.DataSource = dsmydata.Tables["reader"];
                MessageBox.Show("添加用户成功");
                open_mysql_llm.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString()+"打开数据库失败!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clearstrings2();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                open_mysql_llm.conn.Open();
                DataSet dsmydata = new DataSet();

                MySqlCommand cmd = new MySqlCommand("select * from reader", open_mysql_llm.conn);
                MySqlDataAdapter da1 = new MySqlDataAdapter();
                da1.SelectCommand = cmd;
                da1.Fill(dsmydata, "reader");
                dataGridView1.DataSource = dsmydata.Tables["reader"];
                open_mysql_llm.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox12.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox11.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox10.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox9.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox8.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox7.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox15.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReaderName = textBox11.Text;
            College = textBox10.Text;
            Profession = textBox9.Text;
            TelNumber = textBox8.Text;
            LodinKey = textBox7.Text;
            ReaderType = textBox15.Text;

            if(LodinKey == "")
            {
                MessageBox.Show("登录密码不能为空");
                return;
            }
            if(LodinKey.Length != 6)
            {
                MessageBox.Show("登录密码必须为6位");
                return;
            }
            if(ReaderName  == "")
            {
                MessageBox.Show("读者姓名不能为空");
                return;
            }
            if(ReaderType == "")
            {
                MessageBox.Show("用户类型不能为空");
                return;
            }
            try
            {
                open_mysql_llm.conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = open_mysql_llm.conn;
                //修改人员
                cmd.CommandText = "UPDATE reader SET ReaderName = '" + ReaderName +
                    "', College = '" + College +
                    "', Profession = '" + Profession +
                    "', TelNumber = '" + TelNumber +
                    "', LodinKey = '" + LodinKey +
                    "', ReaderType ='" + ReaderType + "'" +
                    "where CardNum = '" + textBox12.Text + "'";
                cmd.ExecuteNonQuery();

                DataSet dsmydata = new DataSet();
                MySqlCommand cmd2 = new MySqlCommand("select * from reader", open_mysql_llm.conn);
                MySqlDataAdapter da1 = new MySqlDataAdapter();
                da1.SelectCommand = cmd2;
                da1.Fill(dsmydata, "reader");
                dataGridView1.DataSource = dsmydata.Tables["reader"];

                MessageBox.Show("修改成功");
                open_mysql_llm.conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败");
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string commandstring = textBox13.Text;
            if (commandstring == "")
            {
                MessageBox.Show("查询借阅证号不能为空");
                return;
            }
            try
            {
                open_mysql_llm.conn.Open();
                DataSet dsmydata = new DataSet();

                MySqlCommand cmd = new MySqlCommand("select * from reader where CardNum ='" + commandstring + "'", open_mysql_llm.conn);
                MySqlDataAdapter da1 = new MySqlDataAdapter();
                dsmydata = new DataSet();
                da1.SelectCommand = cmd;
                da1.Fill(dsmydata, "reader");
                dataGridView1.DataSource = dsmydata.Tables["reader"];
                open_mysql_llm.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "数据库打开失败!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                open_mysql_llm.conn.Open();
                DataSet dsmydata = new DataSet();
                string commandstring = textBox13.Text;
                MySqlCommand cmd = new MySqlCommand("select * from recorder where CardNum = '" + commandstring + "'", open_mysql_llm.conn);
                MySqlDataAdapter da1 = new MySqlDataAdapter();
                da1.SelectCommand = cmd;
                da1.Fill(dsmydata, "reader");
                int flag = 0;
                foreach (DataRow row in dsmydata.Tables["reader"].Rows)
                {
                    if (row[2].ToString() == commandstring)
                    {
                        string BookName = row[1].ToString();
                        string BookID = row[0].ToString();
                        MessageBox.Show("此用户正在借阅："+BookID+"("+BookName+")"+",不能删除该用户信息");
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1)
                {
                    open_mysql_llm.conn.Close();
                    return;
                }
                else
                {
                    MySqlCommand cmd3 = new MySqlCommand();
                    cmd3.Connection = open_mysql_llm.conn;
                    cmd3.CommandText = "delete from recorder where CardNum = '" + commandstring + "'";
                    cmd3.ExecuteNonQuery();

                    MySqlCommand cmd2 = new MySqlCommand();
                    cmd2.Connection = open_mysql_llm.conn;
                    cmd2.CommandText = "delete from booking where CardNum ='" + commandstring + "'";
                    cmd2.ExecuteNonQuery();

                    MySqlCommand cmd4 = new MySqlCommand();
                    cmd4.Connection = open_mysql_llm.conn;
                    cmd4.CommandText = "delete from reader where CardNum = '" + commandstring + "'";
                    cmd4.ExecuteNonQuery();

                    open_mysql_llm.conn.Close();
                    MessageBox.Show("删除成功");

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败");
            }
        }
    }
}
