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
    public partial class BookManage : Form
    {
        public string BookClassID;
        public string Bookid;
        public string Bookname;
        public string Writername;
        public string Publishname;
        public double Price;
        public string Remarks;
        public string Classification;
        public string ListBoxText;


        public BookManage()
        {
            InitializeComponent();
            ClearStrings();
            ClearStrings2();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void ClearStrings()
        {
            /*textBox6.Text = "";
            textBox5.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            textBox2.Text = "";
            textBox1.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            listBox1.Text = "";
            ListBoxText = "";*/

            textBox6.Text = "01010110203";
            textBox5.Text = "ACM国际大学生程序设计竞赛-题目与解读";
            textBox4.Text = "俞勇";
            textBox3.Text = "清华大学出版社";
            textBox2.Text = "69.00";
            textBox1.Text = "019";
            textBox14.Text = "谨以此书献给上海交通大学获得ACM-ICPC世界冠军十周年";
            textBox15.Text = "计算机类";
            listBox1.Text = "";
            ListBoxText = "";
        }

        private void ClearStrings2()
        {
            textBox16.Text = "";
            textBox17.Text = "";
            textBox12.Text = "";
            textBox11.Text = "";
            textBox10.Text = "";
            textBox9.Text = "";
            textBox8.Text = "";
            textBox7.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearStrings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BookClassID = textBox6.Text;
            Bookname = textBox5.Text;
            Writername = textBox4.Text;
            Publishname = textBox3.Text;
            if (textBox2.Text == "") Price = 0;
            else Price = Convert.ToDouble(textBox2.Text);
            Bookid = textBox1.Text;
            Remarks = textBox14.Text;
            Classification = textBox15.Text;
            if (Bookid == "" || Bookname == "")
            {
                MessageBox.Show("编号或书名不能为空");
                ClearStrings();
                return;
            }
            try
            {
                open_mysql_llm.conn.Open();

                DataSet dsmydata = new DataSet();
                string Selectstring = "Select * from tbookclass where BookClassID = '" + BookClassID + "'";
                MySqlDataAdapter da = new MySqlDataAdapter(Selectstring, open_mysql_llm.conn);
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                da.Fill(dsmydata, "tbookclass");
                if (dsmydata.Tables.Count == 1 && dsmydata.Tables[0].Rows.Count != 0)
                {
                    MessageBox.Show("该编号已经存在，请重新输入编号！");
                    open_mysql_llm.conn.Close();
                    return;
                }

                dsmydata = new DataSet();
                Selectstring = "Select * from bookinformation where BookID = '" + Bookid + "'";
                da = new MySqlDataAdapter(Selectstring, open_mysql_llm.conn);
                bd = new MySqlCommandBuilder(da);
                da.Fill(dsmydata, "bookinformation");
                if (dsmydata.Tables.Count == 1 && dsmydata.Tables[0].Rows.Count != 0)
                {
                    MessageBox.Show("该ID号已经存在，请重新输入ID号！");
                    open_mysql_llm.conn.Close();
                    return;
                }

                //将信息填入tbookclass表中并将其显示在DataGridView中
                string Commandstring = "Insert into tbookclass(BookClassID,BookName,BookAuthor,BookPress,BookPrice,BookSummary,BookClass)Values('" +
                    BookClassID + "','" + Bookname + "','" + Writername + "','" + Publishname + "','" +
                    Price + "','" + Remarks + "','" + Classification + "')";
                MySqlDataAdapter darecorder = new MySqlDataAdapter(Commandstring, open_mysql_llm.conn);
                MySqlCommandBuilder bdrecorder = new MySqlCommandBuilder(darecorder);
                darecorder.Fill(dsmydata, "tbookclass");

                MySqlCommand cmd = new MySqlCommand("select * from tbookclass", open_mysql_llm.conn);
                MySqlDataAdapter da1 = new MySqlDataAdapter();
                dsmydata = new DataSet();
                da1.SelectCommand = cmd;
                da1.Fill(dsmydata, "tbookclass");

                dataGridView1.DataSource = dsmydata.Tables["tbookclass"];
                ListBoxText = "添加 " + BookClassID + " " + Bookname + " " + Writername + " " + Publishname +
                    " " + Price.ToString() + " " + Bookid + " " + Classification;
                listBox1.Items.Add(ListBoxText);

                MessageBox.Show("添加成功");
                ClearStrings();
                //将信息填入表中
                DataSet ds1 = new DataSet();
                string commandstring2 = "Insert into bookinformation(BookID,BookClassID,SendFlag)values('" + Bookid + "','" + BookClassID + "','" + 0 + "')";
                MySqlCommand cmd2 = new MySqlCommand(commandstring2, open_mysql_llm.conn);
                MySqlDataAdapter da2 = new MySqlDataAdapter(cmd2);
                da2.InsertCommand = cmd2;
                da2.Fill(ds1, "bookinformation");

                open_mysql_llm.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox12.Text =dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox11.Text =dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox10.Text=dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox9.Text=dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox8.Text=dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox16.Text=dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox17.Text=dataGridView1.CurrentRow.Cells[6].Value.ToString();
            try
            {
                open_mysql_llm.conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("select * from bookinformation where BookClassID = '" + textBox12.Text + "'", open_mysql_llm.conn);
                MySqlCommandBuilder bd = new MySqlCommandBuilder(da);
                DataSet ds = new DataSet();
                da.Fill(ds, "bookinformation");
                foreach (DataRow rows in ds.Tables["bookinformation"].Rows)
                {
                    if (rows["BookClassID"].ToString() == textBox12.Text)
                    {
                        textBox7.Text = rows["BookID"].ToString();
                        break;
                    }
                }
                open_mysql_llm.conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败!");
            }
        }
        private void label16_Click(object sender, EventArgs e) { }
        private void tabPage2_Click(object sender, EventArgs e) { }
        private void textBox12_TextChanged(object sender, EventArgs e) { }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearStrings2();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                open_mysql_llm.conn.Open();
                DataSet dsmydata = new DataSet();
                MySqlCommand cmd = new MySqlCommand("select * from tbookclass", open_mysql_llm.conn);
                MySqlDataAdapter da1 = new MySqlDataAdapter();
                dsmydata = new DataSet();
                da1.SelectCommand = cmd;
                da1.Fill(dsmydata, "tbookclass");
                dataGridView1.DataSource = dsmydata.Tables["tbookclass"];
                open_mysql_llm.conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }            
        }

        private void textBox9_TextChanged(object sender, EventArgs e) { }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string BookClassID = textBox12.Text;
                string BookName = textBox11.Text;
                string BookAuthor = "";
                int len = textBox10.Text.Length;
                for(int i = 0; i < len; i++)
                {
                    if (textBox10.Text[i] == '\\')
                    {
                        BookAuthor = textBox10.Text.Insert(i, "\\");
                        break;
                    }
                }
                string BookPress = textBox9.Text;
                string BookPrice = textBox8.Text;
                string BookSummary = textBox16.Text;
                string BookClass = textBox17.Text;

                if(BookName == "")
                {
                    MessageBox.Show("书名不能为空");
                    return;
                }

                open_mysql_llm.conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = open_mysql_llm.conn;
                //修改书本
                cmd.CommandText = "UPDATE tbookclass SET BookName = '" + BookName +
                    "', BookAuthor ='" + BookAuthor +
                    "', BookPress = '" + BookPress +
                    "', BookPrice = " + Convert.ToDouble(BookPrice) +
                    ", BookSummary = '" + BookSummary +
                    "', BookClass ='" + BookClass+"'"+
                    "where BookClassID = '" + BookClassID +"'";

                string commandText = "修改 "+ BookClassID + " " + BookName + " " + textBox10.Text + " " + BookPress +
                    " " + BookPrice + " " + Bookid + " " + BookClass;
                listBox1.Items.Add(commandText);
                cmd.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand("select * from tbookclass", open_mysql_llm.conn);
                MySqlDataAdapter da1 = new MySqlDataAdapter();
                DataSet dsmydata2 = new DataSet();
                da1.SelectCommand = cmd2;
                da1.Fill(dsmydata2, "tbookclass");
                dataGridView1.DataSource = dsmydata2.Tables["tbookclass"];
                MessageBox.Show("修改成功");
                open_mysql_llm.conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string commandstring = textBox13.Text;
            if (commandstring == "")
            {
                MessageBox.Show("查询编号不能为空");
                return;
            }
            try {
                open_mysql_llm.conn.Open();
                DataSet dsmydata = new DataSet();

                MySqlCommand cmd = new MySqlCommand("select * from tbookclass where BookClassID ='"+commandstring+"'", open_mysql_llm.conn);
                MySqlDataAdapter da1 = new MySqlDataAdapter();
                dsmydata = new DataSet();
                da1.SelectCommand = cmd;
                da1.Fill(dsmydata, "tbookclass");
                dataGridView1.DataSource = dsmydata.Tables["tbookclass"];
                open_mysql_llm.conn.Close();
            }
            catch(Exception ex)
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
                string BookID = "";
                MySqlCommand cmd = new MySqlCommand("select * from bookinformation where BookClassID = '"+commandstring+"'", open_mysql_llm.conn);
                MySqlDataAdapter da1 = new MySqlDataAdapter();
                da1.SelectCommand = cmd;
                da1.Fill(dsmydata, "bookinformation");
                int flag = 0;
                foreach (DataRow row in dsmydata.Tables["bookinformation"].Rows)
                {
                    if(row[1].ToString() == commandstring)
                    {
                        BookID = row[0].ToString();
                        if (Convert.ToInt32(row[2].ToString()) == 1)
                        {
                            MessageBox.Show("此书正在被借阅，信息不能删除，请联系用户及时归还");
                            flag = 1;
                            break;
                        }
                    }
                }
                if (flag == 1)
                {
                    open_mysql_llm.conn.Close();
                    return;
                }
                else
                {

                    DataSet dsmydata2 = new DataSet();
                    MySqlCommand cmd31 = new MySqlCommand("select * from booking where BookID = '" + BookID + "'", open_mysql_llm.conn);
                    MySqlDataAdapter da2 = new MySqlDataAdapter();
                    da2.SelectCommand = cmd31;
                    da2.Fill(dsmydata2, "booking");
                    foreach (DataRow row in dsmydata2.Tables["booking"].Rows)
                    {
                        if (row[0].ToString() == BookID)
                        {
                            string commandtem = "Insert into systemprompt(CardNum,PromptMessage)values('" + row[2].ToString() + "','您于" +
                                row[3].ToString() + "对" + row[1].ToString() + "(" + row[0].ToString() + ")的预定由于书籍信息删除而被取消，我们为给您造成的不便而抱歉。" + " ')";
                            MySqlCommand cmd6 = new MySqlCommand();
                            cmd6.Connection = open_mysql_llm.conn;
                            cmd6.CommandText = commandtem;
                            cmd6.ExecuteNonQuery();
                            break;
                        }
                    }

                    MySqlCommand cmd3 = new MySqlCommand();
                    cmd3.Connection = open_mysql_llm.conn;
                    cmd3.CommandText = "delete from booking where BookID = '" + BookID + "'";
                    cmd3.ExecuteNonQuery();

                    MySqlCommand cmd2 = new MySqlCommand();
                    cmd2.Connection = open_mysql_llm.conn;
                    cmd2.CommandText = "delete from bookinformation where BookClassID ='" + commandstring + "'";
                    cmd2.ExecuteNonQuery();

                    MySqlCommand cmd4 = new MySqlCommand();
                    cmd4.Connection = open_mysql_llm.conn;
                    cmd4.CommandText = "delete from tbookclass where BookClassID = '" + commandstring + "'";
                    cmd4.ExecuteNonQuery();

                    string tem = "";
                    tem = "删除 编号为: " + commandstring+" ID号为: "+BookID +" 的书籍\n";
                    listBox1.Items.Add(tem);

                    open_mysql_llm.conn.Close();
                    MessageBox.Show("删除成功");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败");
            }
        }

        private void BookManage_Load(object sender, EventArgs e)
        {

        }

       
    }
}
