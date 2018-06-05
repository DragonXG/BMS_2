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
    public partial class BorrowReturn : Form
    {
        public BorrowReturn()
        {
            InitializeComponent();
        }

        bool if_can_borrow = true;     //是否能够借书
        int if_get_bookid = 0; //如果为零，则不能输入图书id
        int maxmouth = 0; //如果当前日期比预定日期早一个月，为1，否则为0，不能够借书。
        

        private void textBox1_MouseLeave(object sender, EventArgs e)   //借书处理1
        {
            if_can_borrow = true;
            int num_book_max = 3;     //最多可借书数
            int people = 0;     //有该借阅证号的人数
            int text8moder = 0;   //textBox8显示的内容
            try
            {
                if (textBox1.Text.Count() == 0)
                {
                    textBox8.Text = "请读入借阅证号！";
                    if_get_bookid = 2;
                }
                else
                {

                    int booknum = 0; //借书的数量
                    open_mysql_llm.conn.Open();
                    String cardnum = textBox1.Text.Trim();
                    String strmy_reader = "Select * From reader Where CardNum = '" + cardnum + "'";  //根据借阅证号查找借书记录

                    DataSet dslibrary = new DataSet();

                    MySqlDataAdapter dareader = new MySqlDataAdapter(strmy_reader, open_mysql_llm.conn);
                    MySqlCommandBuilder bdreader = new MySqlCommandBuilder(dareader);
                    dareader.Fill(dslibrary, "reader");
                    foreach (DataRow row in dslibrary.Tables["reader"].Rows)
                    {
                        if (row["CardNum"].ToString() != null)
                        {
                            //MessageBox.Show("有该学生！");
                            people += 1;
                        }
                    }
                    if (people == 0)
                    {
                        textBox8.Text = "没有该借阅证号！请重新读取！";
                        if_get_bookid = 2;
                    }
                    else
                    {
                        String str1 = "图书ID           借阅证号           借阅日期                图书名" + "\r\n\r\n";
                        String str2 = "该学生有书逾期没还，不能够借书。信息如下：\r\n";                      //不能借书，以及未归还书籍信息
                        str2 = str2 + "书名               " + "还书日期                  " + "当前日期" + "\r\n";
                        System.DateTime now = new System.DateTime();   //获取系统时间
                        now = System.DateTime.Now;
  

                        String strmy_recorder = "Select * From recorder Where CardNum = '" + cardnum + "'";
                        MySqlDataAdapter darecorder = new MySqlDataAdapter(strmy_recorder, open_mysql_llm.conn);
                        MySqlCommandBuilder bdrecorder = new MySqlCommandBuilder(darecorder);
                        darecorder.Fill(dslibrary, "recorder");

                        //if(dslibrary.Tables)


                        foreach (DataRow row1 in dslibrary.Tables["recorder"].Rows)
                        {

                            text8moder = 1;
                            //textBox8.Text = "无借阅信息！";


                            str1 = str1 + row1["BookID"].ToString() + "              " + row1["CardNum"].ToString() + "         " +
                            row1["BorrowDate"].ToString() + "      " + row1["BookName"].ToString() + "\r\n\r\n";
                            booknum += 1;  //借的书的数量

                            System.DateTime borrowdate = Convert.ToDateTime(row1["BorrowDate"]); //获取借阅时间
                            borrowdate = borrowdate.AddMonths(1);       //截止日期
                            if (System.DateTime.Compare(now, borrowdate) > 0)
                            {
                                str2 = str2 + row1["BookName"].ToString() + "    " + borrowdate.ToString() + "   " + now.ToString() + "\r\n";
                                if_can_borrow = false;
                            }

                        }
                        if (text8moder == 0)
                        {
                            textBox8.Text = "无借阅信息！";
                            if_get_bookid = 1;
                        }
                        else
                        {
                            textBox8.Text = str1;
                            if_get_bookid = 1;
                        }
                        textBox2.Text = booknum.ToString();
                        textBox3.Text = num_book_max.ToString();      //最多可借书数
                        
                        if (if_can_borrow == false)    //显示不能借书原因
                        {
                            MessageBox.Show(str2);
                            this.Close();
                        }
                        if (booknum >= 3)   // 判断是否超过最多借书量
                        {
                            MessageBox.Show("不能够借书，原因：已达到最大借书量！");
                            this.Close();
                        }

                    }

                open_mysql_llm.conn.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("打开数据库失败！");
            }

        }

        private void textBox4_MouseLeave(object sender, EventArgs e)   //借书处理2
        {

            maxmouth = 0;
            try
            {
                if (if_get_bookid == 1)    //判断是否可以执行这个函数,为1才能后执行
                {
                    if (textBox4.Text.Count() == 0)
                    {
                        textBox9.Text = "请读入图书ID！";
                    }
                    else
                    {
                        int book = 0;
                        open_mysql_llm.conn.Open();
                      
                        DataSet dsmydata = new DataSet();
                        String bookid = textBox4.Text.Trim();
                     




                        String strmy_bookinformation = "Select * From bookinformation Where BookID = '" + bookid + "'";  //根据借阅证号查找借书记录
                        MySqlDataAdapter dabookinformation = new MySqlDataAdapter(strmy_bookinformation, open_mysql_llm.conn);
                        MySqlCommandBuilder bdbookinformation = new MySqlCommandBuilder(dabookinformation);
                        dabookinformation.Fill(dsmydata, "bookinformation");
                        String yi = "1";
                        int jiechu = 0;

                        foreach (DataRow row in dsmydata.Tables["bookinformation"].Rows)
                        {
                            if (row["SendFlag"].ToString() == yi)
                            {
                                //MessageBox.Show("该书已借出！");
                                jiechu = 1;

                            }
                            if (row["BookID"].ToString() != null)
                            {
                                //MessageBox.Show("有该图书！");
                                book += 1;
                            }
                        }
                        if (book == 0)
                        {
                            textBox9.Text = "没有该图书！请重新读取ID！";
                        }
                        else if (jiechu == 1)     //判断该书是否借出，借出则不进行下步动作。
                        {
                            textBox9.Text = "该书已借出，请重新读入ID！";
                        }
                        else
                        {
                            String str1 = "图书ID           订阅标记           借阅日期                图书名" + "\r\n\r\n";

                            System.DateTime now = new System.DateTime();   //获取系统时间
                            now = System.DateTime.Now;

                            String bookclassid = "";
                            foreach (DataRow row3 in dsmydata.Tables["bookinformation"].Rows)
                            {
                                bookclassid = row3["BookClassID"].ToString().Trim();
                            }

                            String strmy_tbookclass = "Select * From tbookclass Where BookClassID = '" + bookclassid + "'";
                            MySqlDataAdapter datbookclass = new MySqlDataAdapter(strmy_tbookclass, open_mysql_llm.conn);
                            MySqlCommandBuilder bdtbookclass = new MySqlCommandBuilder(datbookclass);
                            datbookclass.Fill(dsmydata, "tbookclass");

                            String str_booking = "Select * From booking Where BookID = '" + bookid + "'";
                            MySqlDataAdapter dabooking = new MySqlDataAdapter(str_booking, open_mysql_llm.conn);
                            MySqlCommandBuilder bdbooking = new MySqlCommandBuilder(dabooking);
                            dabooking.Fill(dsmydata, "booking");

                            foreach (DataRow row in dsmydata.Tables["bookinformation"].Rows)
                            {
                                str1 = str1 + row["BookID"].ToString() + "              ";
                            }

                            int bookoder = 0; //看书是否被预定。1为预定    
                            //String data = "";
                            foreach (DataRow row1 in dsmydata.Tables["booking"].Rows)
                            {
                                bookoder = 1;
                                //data = row1["BookDate"].ToString();
                                System.DateTime borrowdate = Convert.ToDateTime(row1["BookDate"]);  //获取预定时间
                                System.DateTime latemouse = new System.DateTime();
                                latemouse = now.AddMonths(1);    //一个月以后的现在
                                if (System.DateTime.Compare(latemouse, borrowdate) < 0)  // 一个月后的现在小于预定日期，接，否则不接。
                                {
                                    maxmouth = 1;
                                }

                            }
                            if (bookoder == 1)
                            {
                                str1 = str1 + "是" + "                 ";
                            }
                            else
                            {
                                maxmouth = 1;  //没有预定记录，可以借
                                str1 = str1 + "否" + "                 ";
                            }
                            str1 = str1 + now.ToString() + "      ";
                            foreach (DataRow row2 in dsmydata.Tables["tbookclass"].Rows)
                            {
                                str1 = str1 + row2["BookName"].ToString() + "\r\n\r\n";
                            }
                            textBox9.Text = str1;

                        }

                        
                        

                    }
                    
                    open_mysql_llm.conn.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开数据库失败！");
            }
        }

        private void button2_Click(object sender, EventArgs e)   //退出
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)    //借书按钮
        {
            if (if_can_borrow == true)
            {
                if (maxmouth == 0)
                {
                    MessageBox.Show("不能借，因为该书预定在一个月以内！");
                    //this.Close();
                }
                else
                {
                    open_mysql_llm.conn.Open();

                    String cardnum = textBox1.Text.Trim();
                    String bookid = textBox4.Text.Trim();
                    DataSet dsmydata = new DataSet();
                    String strmy_bookinformation = "Select * From bookinformation Where BookID = '" + bookid + "'";
                    MySqlDataAdapter dabookinformation = new MySqlDataAdapter(strmy_bookinformation, open_mysql_llm.conn);
                    MySqlCommandBuilder bdbookinformation = new MySqlCommandBuilder(dabookinformation);
                    dabookinformation.Fill(dsmydata, "bookinformation");

                    String bookclassid = "";  //获得编码
                    foreach (DataRow row5 in dsmydata.Tables["bookinformation"].Rows)
                    {
                        row5["SendFlag"] = "1";
                        bookclassid = row5["BookClassID"].ToString().Trim();
                    }
                    dabookinformation.Update(dsmydata, "bookinformation");
                    dsmydata.Tables["bookinformation"].AcceptChanges();

                    //打开tbookclass获得图书名
                    String strmy_tbookclass = "Select * From tbookclass Where BookClassID = '" + bookclassid + "'";
                    MySqlDataAdapter datbookclass = new MySqlDataAdapter(strmy_tbookclass, open_mysql_llm.conn);
                    MySqlCommandBuilder bdtbookclass = new MySqlCommandBuilder(datbookclass);
                    datbookclass.Fill(dsmydata, "tbookclass");

                    String name = "";
                    foreach (DataRow row6 in dsmydata.Tables["tbookclass"].Rows)
                    {
                        name = row6["BookName"].ToString().Trim();
                    }

                    //
                    //打开recoreder表
                    String strmy_recorder = "Select * From recorder";
                    MySqlDataAdapter darecorder = new MySqlDataAdapter(strmy_recorder, open_mysql_llm.conn);
                    MySqlCommandBuilder bdrecorder = new MySqlCommandBuilder(darecorder);
                    darecorder.Fill(dsmydata, "recorder");

                    System.DateTime now = new System.DateTime();   //获取系统时间
                    now = System.DateTime.Now;


                    //为recorder添加借阅信息。
                    DataRow newrow = dsmydata.Tables["recorder"].NewRow();
                    newrow["BookID"] = bookid;
                    newrow["BorrowDate"] = now;
                    newrow["CardNum"] = cardnum;
                    //name = "名";
                    newrow["BookName"] = name;
 

                    dsmydata.Tables["recorder"].Rows.Add(newrow);
                    darecorder.Update(dsmydata, "recorder");
                    dsmydata.Tables["recorder"].AcceptChanges();

                    MessageBox.Show("借书成功！");



                    open_mysql_llm.conn.Close();
                }
            }
            else
            {
                MessageBox.Show("不能够借书！");
                this.Close();
            }

        }

     
    }
}
