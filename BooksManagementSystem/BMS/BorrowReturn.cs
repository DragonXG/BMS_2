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

//*************************** 注：需要一个cardnum，请添加到，open_mysql_llm那里面的get_number_llm的borrow_cardnum********

namespace BMS
{
    public partial class BorrowReturn : Form
    {
        public BorrowReturn()
        {
            InitializeComponent();
        }

        bool if_can_borrow = true;     //是否能够借书
        bool max_book_num = true;      //是否达到最大借书量
        int if_get_bookid = 0;         //如果为零，则不能输入图书id，为1才能够
        int maxmouth = 0;              //如果当前日期比预定日期早一个月，为1，否则为0，不能够借书。
        int mouth_max = 30;            //最大借书日期,修改，这里为天数。
        int num_book_max = 5;          //最多可借书数
        int xujie = 0;                 //是否可以续借，0，不可续借，1，可借
        int huanshu = 0;               //是否可以进行还书操作，0，不可，1，可。
        int butten1_times = 0;         //是否可以执行butten1，0可以执行。
        int butten3_times = 0;         //是否可以执行butten3，butten4,0为可以执行
        int mouse_leave = 0;           //窗体建好后，鼠标移动则执行一次butten6_Click_run（）注：这个窗体只执行一次。
        int delet_booking = 0;         //执行借书时，是否删除booking里面的数据。
        int if_message = 0;            //是否弹出提示还款信息，0，为不提醒，1为提醒。
        String bookid_1 = "";          //图书id
        bool do_huanshu = false;          //是否执行还书按钮
       

        private void button2_Click(object sender, EventArgs e)   //退出
        {
            this.Close();
        }

        private void if_go_lendbook()
        {

        }

        //为returnedbook添加代码，
        private void increase_returnedbook(String bookid, String cardnum, String name, String status,String Arre, ref DataSet dsmydata)      
        {
            try
            {
                String strmy_returnedbook = "Select * From returnedbook";
                MySqlDataAdapter dareturnedbook = new MySqlDataAdapter(strmy_returnedbook, open_mysql_llm.conn);
                MySqlCommandBuilder bdreturnedbook = new MySqlCommandBuilder(dareturnedbook);
                dareturnedbook.Fill(dsmydata, "returnedbook");

                System.DateTime now = new System.DateTime();   //获取系统时间
                now = System.DateTime.Now;

                //为eturnedbook添加借阅信息。
                DataRow newrow_2 = dsmydata.Tables["returnedbook"].NewRow();
                newrow_2["BookID"] = bookid;
                newrow_2["BorrowDate"] = now;
                newrow_2["CardNum"] = cardnum;
                newrow_2["BookName"] = name;
                newrow_2["BorrowingStatus"] = status;
                newrow_2["Arrearage"] = Arre;

                dsmydata.Tables["returnedbook"].Rows.Add(newrow_2);
                dareturnedbook.Update(dsmydata, "returnedbook");
                dsmydata.Tables["returnedbook"].AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }

        private void xiugai(String borrow_time)    //returnedbook 是与否的修改。
        {
            try
            {
                open_mysql_llm.conn.Open();

                DataSet dsmydata = new DataSet();

                String strmy_returnedbook = "Select * From returnedbook Where BorrowDate = '" + borrow_time + "'";
                MySqlDataAdapter dareturnedbook = new MySqlDataAdapter(strmy_returnedbook, open_mysql_llm.conn);
                MySqlCommandBuilder bdreturnedbook = new MySqlCommandBuilder(dareturnedbook);
                dareturnedbook.Fill(dsmydata, "returnedbook");

                //MessageBox.Show("执行2");
                foreach (DataRow row in dsmydata.Tables["returnedbook"].Rows)
                {
                    //MessageBox.Show(row["BorrowDate"].ToString());
                    row["Arrearage"] = "是";
                }
                dareturnedbook.Update(dsmydata, "returnedbook");
                dsmydata.Tables["returnedbook"].AcceptChanges();

                open_mysql_llm.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }

        private void updata_returnbook()      //更新returnbook数据
        {
            try
            {
                open_mysql_llm.conn.Open();


                String borrow = "借阅";
                String cardnum = get_number_llm.borrow_cardnum;

                DataSet dsmydata = new DataSet();
                String strmy_returnedbook = "Select * From returnedbook Where BookID = '" + bookid_1 + "' and BorrowingStatus = '" + borrow + "' and CardNum = '" + cardnum + "'";
                MySqlDataAdapter dareturnedbook = new MySqlDataAdapter(strmy_returnedbook, open_mysql_llm.conn);
                MySqlCommandBuilder bdreturnedbook = new MySqlCommandBuilder(dareturnedbook);
                dareturnedbook.Fill(dsmydata, "returnedbook");

               
                //MessageBox.Show("执行");
                String borrow_time = "";    //获得借书时间。

                foreach (DataRow row in dsmydata.Tables["returnedbook"].Rows)
                {
                    //MessageBox.Show(row["BorrowDate"].ToString());
                    borrow_time = row["BorrowDate"].ToString();
                }
                open_mysql_llm.conn.Close();
                xiugai(borrow_time);     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }

        private void button1_Click(object sender, EventArgs e)    //借书按钮
        {
            try
            {
                if (do_huanshu == false)
                {
                    if (max_book_num == true)
                    {
                        if (butten1_times == 0)
                        {
                            butten1_times++;
                            if (textBox1.Text.Count() != 0 && textBox4.Text.Count() != 0)
                            {
                                if (if_can_borrow == true)
                                {
                                    if (maxmouth == 0)
                                    {
                                        MessageBox.Show("不能借，因为该书预定在" + mouth_max.ToString() + "天以内！");
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

                                        //删除预定书的记录
                                        if (delet_booking == 1)
                                        {
                                            String strmy_booking = "Select * From booking Where BookID = '" + bookid + "'";
                                            MySqlDataAdapter dabooking = new MySqlDataAdapter(strmy_booking, open_mysql_llm.conn);
                                            MySqlCommandBuilder bdbooking = new MySqlCommandBuilder(dabooking);
                                            dabooking.Fill(dsmydata, "booking");

                                            dsmydata.Tables["booking"].Rows[0].Delete();
                                            dabooking.Update(dsmydata, "booking");
                                            dsmydata.Tables["booking"].AcceptChanges();

                                        }

                                        //到此结束

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
                                        newrow["BorrowingStatus"] = "在借";


                                        dsmydata.Tables["recorder"].Rows.Add(newrow);
                                        darecorder.Update(dsmydata, "recorder");
                                        dsmydata.Tables["recorder"].AcceptChanges();

                                        //####################################   returnedbook,添加
                                        increase_returnedbook(bookid, cardnum, name, "借阅", "否", ref  dsmydata);

                                        MessageBox.Show("借书成功！");
                                        textBox4.Text = "";
                                        textBox9.Text = "";
                                        open_mysql_llm.conn.Close();

                                        //******************日志代码******************
                                        //********************************************************************************************

                                        FileStream file = new FileStream(@"F:\log.txt", FileMode.Append);
                                        StreamWriter sw = new StreamWriter(file, System.Text.Encoding.GetEncoding("GB2312"));
                                        String str_log = "";
                                        str_log = "借阅证号：" + cardnum.ToString() + " 于 " + now.ToString() + "  借阅ID为：" + bookid.ToString() + "  的图书,";
                                        sw.WriteLine();
                                        sw.Write(str_log);
                                        sw.Close();
                                        file.Close();

                                        //********************************************
                                        button6_Click_run();      //再次执行该函数，进行刷新。

                                    }
                                }
                                else
                                {
                                    //MessageBox.Show("不能够借书！");
                                    textBox4.Text = "";
                                    textBox9.Text = "";

                                    get_number_llm.str_message = "要交完费用，才能借书哦！";
                                    if_message_1 f1 = new if_message_1();
                                    f1.Show();

                                }
                            }
                            else
                            {
                                MessageBox.Show("不能执行该操作！");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("已达到最大借数量，不能借书！");
                    }
                }
                else
                {
                    MessageBox.Show("同学，请先还书啊！不然我不借给你！啊!");
                    textBox4.Text = "";
                    textBox9.Text = "";
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }

        private void button5_Click(object sender, EventArgs e)  //还书退出
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)   //还书按钮
        {
            try
            {
                if (butten3_times == 0)
                {
                    butten3_times++;
                    if (huanshu == 0)
                    {
                        MessageBox.Show("不能够还书！请重输入图书ID！");
                    }
                    else
                    {
                        String cardnum = "";
                        String name = "";

                        open_mysql_llm.conn.Open();
                        DataSet dsmydata = new DataSet();
                        String bookid = textBox5.Text.Trim();
                        String strmy_reader = "Select * From recorder Where BookID = '" + bookid + "'";
                        MySqlDataAdapter darecorder = new MySqlDataAdapter(strmy_reader, open_mysql_llm.conn);
                        MySqlCommandBuilder bdrecorder = new MySqlCommandBuilder(darecorder);
                        darecorder.Fill(dsmydata, "recorder");

                        foreach (DataRow row in dsmydata.Tables["recorder"].Rows)
                        {
                            name = row["BookName"].ToString();
                            cardnum = row["CardNum"].ToString();
                        }
                        //删除recorder记录
                        dsmydata.Tables["recorder"].Rows[0].Delete();   

                        darecorder.Update(dsmydata, "recorder");
                        dsmydata.Tables["recorder"].AcceptChanges();

                        String strmy_bookinformation = "Select * From bookinformation Where BookID = '" + bookid + "'";
                        MySqlDataAdapter dabookinformation = new MySqlDataAdapter(strmy_bookinformation, open_mysql_llm.conn);
                        MySqlCommandBuilder bdbookinformation = new MySqlCommandBuilder(dabookinformation);
                        dabookinformation.Fill(dsmydata, "bookinformation");
                        foreach (DataRow row1 in dsmydata.Tables["bookinformation"].Rows)
                        {
                            row1["SendFlag"] = "0";
                        }
                        dabookinformation.Update(dsmydata, "bookinformation");
                        dsmydata.Tables["bookinformation"].AcceptChanges();


                        //**************************注***********************
                        //为returnedbook添加数据

                        if (if_message == 1)
                        {
                            increase_returnedbook(bookid, cardnum, name, "已还", "是", ref  dsmydata);
                        }
                        else
                        {
                            increase_returnedbook(bookid, cardnum, name, "已还", "否", ref  dsmydata);
                        }

                        open_mysql_llm.conn.Close();

                        textBox5.Text = "";
                        textBox10.Text = "";
                        textBox6.Text = "";
                        textBox7.Text = "";

                        //更新returnbook数据
                        if (if_message == 1)
                        {
                            updata_returnbook();
                        }

                        //******************日志代码******************
                        //**************************************************************************************************

                        System.DateTime now = new System.DateTime();   //获取系统时间
                        now = System.DateTime.Now;
                        FileStream file = new FileStream(@"F:\log.txt", FileMode.Append);
                        StreamWriter sw = new StreamWriter(file, System.Text.Encoding.GetEncoding("GB2312"));
                        String str_log = "";
                        str_log = "借阅证号：" + cardnum.ToString() + " 于 " + now.ToString() + "  归还ID为：" + bookid.ToString() + "  的图书,";
                        sw.WriteLine();
                        sw.Write(str_log);
                        sw.Close();
                        file.Close();
                        //*************************************************
                        MessageBox.Show("还书成功！");
                        if_can_borrow = true;
                        
                        button6_Click_run();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }

        private void button4_Click(object sender, EventArgs e)   //续借
        {
            try
            {
                if (butten3_times == 0)
                {
                    butten3_times++;
                    if (huanshu == 0)
                    {
                        MessageBox.Show("请重新输入图书ID！");
                    }
                    else if (xujie == 0)
                    {
                        MessageBox.Show("书逾期归还，不能够续借！");
                        butten3_times = 0;
                    }
                    else
                    {
                        open_mysql_llm.conn.Open();
                        int havebook = 0;   //该书被预定为1
                        System.DateTime bookingtime = new System.DateTime();     //预定时间
                        System.DateTime now = new System.DateTime();   //获取系统时间
                        now = System.DateTime.Now;

                        DataSet dsmydata = new DataSet();
                        String bookid = textBox5.Text.Trim();
                        String str_booking = "Select * From booking Where BookID = '" + bookid + "'";
                        MySqlDataAdapter dabooking = new MySqlDataAdapter(str_booking, open_mysql_llm.conn);
                        MySqlCommandBuilder bdbooking = new MySqlCommandBuilder(dabooking);
                        dabooking.Fill(dsmydata, "booking");
                        foreach (DataRow row in dsmydata.Tables["booking"].Rows)
                        {
                            havebook = 1;
                            bookingtime = Convert.ToDateTime(row["BookDate"]);
                        }

                        int days = (now - bookingtime).Days;

                        if (havebook == 1 && days < 30 && days > -(mouth_max))  //书被预定，且预定时间不早于现在一个月,迟于现在一个月，不能借
                        {
                            MessageBox.Show("不能够续借，该书已被预定！");
                        }
                        else
                        {
                            String strmy_reader = "Select * From recorder Where BookID = '" + bookid + "'";
                            MySqlDataAdapter darecorder = new MySqlDataAdapter(strmy_reader, open_mysql_llm.conn);
                            MySqlCommandBuilder bdrecorder = new MySqlCommandBuilder(darecorder);
                            darecorder.Fill(dsmydata, "recorder");

                            String cardnum = "";    //为新增加的借阅号。
                            String name = "";       //为新增加的图书名。
                            foreach (DataRow row2 in dsmydata.Tables["recorder"].Rows)
                            {
                                row2["BorrowDate"] = now;
                                cardnum = row2["CardNum"].ToString();
                                name = row2["BookName"].ToString();
                                
                            }
                            darecorder.Update(dsmydata, "recorder");
                            dsmydata.Tables["recorder"].AcceptChanges();

                            //添加returnedbook表内容，
                            increase_returnedbook(bookid, cardnum, name, "借阅", "否", ref  dsmydata);

                            //******************日志代码******************
                            //******************************************************************************************
                            
                            FileStream file = new FileStream(@"F:\log.txt", FileMode.Append);
                            StreamWriter sw = new StreamWriter(file, System.Text.Encoding.GetEncoding("GB2312"));
                            String str_log = "";
                            str_log = "借阅证号：" + cardnum.ToString() + " 于 " + now.ToString() + "  续借ID为：" + bookid.ToString() + "  的图书,";
                            sw.WriteLine();
                            sw.Write(str_log);
                            sw.Close();
                            file.Close();
                            //***********************************************************************
                            MessageBox.Show("续借成功！");
                            textBox5.Text = "";
                            textBox10.Text = "";
                            textBox6.Text = "";
                            textBox7.Text = "";
                            
                        }

                        open_mysql_llm.conn.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }
        private void button6_Click_run()
        {
            if_get_bookid = 0;
            if_can_borrow = true;
            do_huanshu = false;  //是否执行还书按钮
            //int people = 1;     //有该借阅证号的人数
            int text8moder = 0;   //textBox8显示的内容

            textBox1.Text = get_number_llm.borrow_cardnum;  //获得节约证号

            textBox2.Text = "";
            textBox3.Text = "";
            try
            {


                int booknum = 0; //借书的数量
                

                open_mysql_llm.conn.Open();
                String cardnum = textBox1.Text.Trim();
                String ID = "";
                String last_data = "";

                DataSet dslibrary = new DataSet();

                String str1 = "图书ID           借阅证号           借阅日期                图书名" + "\r\n\r\n";
                String str2 = "您有书逾期没还，不能够借书。信息如下：\r\n\r\n";                      //不能借书，以及未归还书籍信息
                str2 = str2 + "书名               " + "还书日期                  " + "当前日期" + "\r\n\r\n";
                System.DateTime now = new System.DateTime();   //获取系统时间
                now = System.DateTime.Now;


                String strmy_recorder = "Select * From recorder Where CardNum = '" + cardnum + "'";
                MySqlDataAdapter darecorder = new MySqlDataAdapter(strmy_recorder, open_mysql_llm.conn);
                MySqlCommandBuilder bdrecorder = new MySqlCommandBuilder(darecorder);
                darecorder.Fill(dslibrary, "recorder");


                foreach (DataRow row1 in dslibrary.Tables["recorder"].Rows)
                {

                    text8moder = 1;
                    //textBox8.Text = "无借阅信息！";

                    str1 = str1 + row1["BookID"].ToString() + "              " + row1["CardNum"].ToString() + "         " +
                    row1["BorrowDate"].ToString() + "      " + row1["BookName"].ToString() + "\r\n\r\n";
                    booknum += 1;  //借的书的数量

                    System.DateTime borrowdate = Convert.ToDateTime(row1["BorrowDate"]); //获取借阅时间
                    borrowdate = borrowdate.AddDays(mouth_max);       //截止日期
                    if (System.DateTime.Compare(now, borrowdate) > 0)
                    {
                        str2 = str2 + row1["BookName"].ToString() + "    " + borrowdate.ToString() + "   " + now.ToString() + "\r\n";
                        if_can_borrow = false;
                        do_huanshu = true;
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


                //判断是否可以借书，，查看是否有欠费记录。
                if (if_can_borrow == true)
                {
                    int t = 0;
                    String arr = "是";
                    String strmy_returnedbook = "Select * From returnedbook Where CardNum = '" + cardnum + "' and Arrearage = '" + arr + "'";
                    MySqlDataAdapter dareturnedbook = new MySqlDataAdapter(strmy_returnedbook, open_mysql_llm.conn);
                    MySqlCommandBuilder bdreturnedbook = new MySqlCommandBuilder(dareturnedbook);
                    dareturnedbook.Fill(dslibrary, "returnedbook");

                    foreach (DataRow row in dslibrary.Tables["returnedbook"].Rows)
                    {
                        t++;
                    }
                    if (t != 0)
                    {
                        if_can_borrow = false;
                        str2 = "您有费用没交，不能够借书哦！";
                    }
                }

                if (do_huanshu == true)
                {
                    MessageBox.Show(str2 + "\r\n" + "                              请先还书后再缴费！");
                }
                else
                {
                    if (if_can_borrow == false)    //显示不能借书原因
                    {
                        //MessageBox.Show(str2);
                        get_number_llm.str_message = str2;
                        if_message_1 f1 = new if_message_1();
                        f1.Show();
                        //if_message_pay f2 = new if_message_pay();
                        //f2.Show();

                    }
                }
                if (booknum >= num_book_max)   // 判断是否超过最多借书量
                {
                    MessageBox.Show("不能够借书，原因：已达到最大借书量！");
                    max_book_num = false;
                }

                open_mysql_llm.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }

        }
        private void button6_Click(object sender, EventArgs e)       
        {

        }

        private void button7_Click(object sender, EventArgs e)    //图书id确认按钮
        {
            maxmouth = 0;
            delet_booking = 0;
            if (get_number_llm.become_ture == 1)
            {
                get_number_llm.become_ture = 0;
                button6_Click_run();
                
            }
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
                            int delete = 0; //是否删除booking记录，1，删除，0不删除。
                            foreach (DataRow row1 in dsmydata.Tables["booking"].Rows)
                            {
                                bookoder = 1;
                                System.DateTime borrowdate = Convert.ToDateTime(row1["BookDate"]);  //获取预定时间
                                System.DateTime latemouse = new System.DateTime();
                                latemouse = now.AddDays(mouth_max);    //一个月以后的现在
                                int Days = (now - borrowdate).Days;   //计算现在与预定日之间的天数。
                                if (System.DateTime.Compare(latemouse, borrowdate) < 0 || Days > 30) // 一个月后的现在小于预定日期或者预定日期在两个月以前，借，否则不借。
                                {
                                    maxmouth = 1;
                                    if (Days > 30)   //如果长时间不取消预定，早于当前日期一个月，则删除该预定记录。
                                    {
                                        delete = 1;
                                    }
                                }
                                else if (row1["CardNum"].ToString() == get_number_llm.borrow_cardnum)
                                {
                                    maxmouth = 1;  //可以借书
                                    delet_booking = 1;   //该学生预定了此书，要接该书，在借书时应删除预定记录。
                                }
                                else
                                {
                                    maxmouth = 0;
                                    delet_booking = 0;
                                }

                            }
                            if (delete == 1)  //删除预定记录
                            {
                                bookoder = 0;
                                dsmydata.Tables["booking"].Rows[0].Delete();
                                dabooking.Update(dsmydata, "booking");
                                dsmydata.Tables["booking"].AcceptChanges();
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
                            butten1_times = 0;    //重置butten1执行次数，以便可以运行butten1_click代码

                        }

                    }

                    open_mysql_llm.conn.Close();

                }
                else
                {
                    textBox9.Text = "该借阅号，不允许借书！";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }

        }

        private void button8_Click(object sender, EventArgs e)    //还书按钮
        {

            try
            {
                //不要
                //get_number_llm.bookid = textBox5.Text;
                xujie = 0;
                huanshu = 0;
                if_message = 0;    //重置该函数。
                String jie_cardnum = "";
                if (textBox5.Text.Count() == 0)
                {
                    textBox10.Text = "请读入图书ID！";
                }
                else
                {
                    open_mysql_llm.conn.Open();
                    int havebook = 0;        //看借书表里是否有该书的借书信息。

                    String str1_out = "借阅证号               " + "图书ID               " +
                        "借阅日期            " + "图书名 " + "\r\n\r\n";

                    DataSet dsmydata = new DataSet();
                    String bookid = textBox5.Text.Trim();
                    bookid_1 = bookid;
                    String strmy_reader = "Select * From recorder Where BookID = '" + bookid + "'";
                    MySqlDataAdapter darecorder = new MySqlDataAdapter(strmy_reader, open_mysql_llm.conn);
                    MySqlCommandBuilder bdrecorder = new MySqlCommandBuilder(darecorder);
                    darecorder.Fill(dsmydata, "recorder");

                    System.DateTime borrowdata = new System.DateTime();

                    foreach (DataRow row6 in dsmydata.Tables["recorder"].Rows)
                    {
                        havebook = 1;
                        jie_cardnum = row6["CardNum"].ToString();  //获得借阅号。

                        str1_out = str1_out + row6["CardNum"].ToString() + "             " + row6["BookID"].ToString() +
                        "               " + row6["BorrowDate"].ToString() + "    " + row6["BookName"].ToString() + "\r\n";
                        borrowdata = Convert.ToDateTime(row6["BorrowDate"]);
                        borrowdata = borrowdata.AddDays(mouth_max);     //截止日期

                    }
                    if (jie_cardnum == get_number_llm.borrow_cardnum)
                    {
                        if (havebook == 0)
                        {
                            textBox10.Text = "该书没有被借出，重新输入！";
                        }
                        else
                        {
                            textBox10.Text = str1_out;   //输出相关信息

                            //*********************需要改***************************

                            System.DateTime now = new System.DateTime();   //获取系统时间
                            now = System.DateTime.Now;
                            double days = ((TimeSpan)(now - borrowdata)).Days;
                            if (days <= 0)
                            {
                                double daoday = days * (-1);
                                textBox6.Text = "0";
                                textBox7.Text = daoday.ToString();
                                xujie = 1;                   //是否可以续借
                            }
                            else
                            {
                                double money = days * 0.5;
                                textBox6.Text = money.ToString() + "元";
                                textBox7.Text = "超过还书日期：" + days.ToString() + "天";
                                if_message = 1;   //在还书时提示还款。
                            }
                            huanshu = 1;
                            butten3_times = 0;   //可以执行还书按钮。
                        }
                    }
                    else
                    {
                        MessageBox.Show("该书不为当前借阅证号所借，不能执行还书及续借操作！");
                    }

                    open_mysql_llm.conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
            }
        }


        //以下为了让butten6_click_run函数执行一次
        private void BorrowReturn_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_leave == 0)
            {
                button6_Click_run();
                mouse_leave = 1;
            }
        }

        private void tabPage1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_leave == 0)
            {
                button6_Click_run();
                mouse_leave = 1;
            }
        }

        private void textBox8_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_leave == 0)
            {
                button6_Click_run();
                mouse_leave = 1;
            }
        }

     
    }
}
