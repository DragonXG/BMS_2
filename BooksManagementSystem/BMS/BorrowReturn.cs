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

        bool if_can_borrow = false;     //是否能够借书
        

        private void textBox1_MouseLeave(object sender, EventArgs e)   //借书处理1
        {
            int num_book_max = 3;     //最多可借书数
            try
            {
                open_mysql_llm.conn.Open();
                String cardnum = textBox1.Text.Trim();
                String strmy = "Select * From recorder Where CardNum = '" + cardnum + "'";  //根据借阅证号查找借书记录
                MySqlCommand cmd = new MySqlCommand(strmy, open_mysql_llm.conn);
                MySqlDataReader dr ;
                dr = cmd.ExecuteReader();
               
                String str1 = "图书ID           借阅证号           借阅日期                图书名" + "\r\n";

                int booknum = 0, if_go = 0;

                System.DateTime now = new System.DateTime();   //获取系统时间
                now = System.DateTime.Now;

                
                while(dr.Read())
                {
                    str1 = str1 + dr["BookID"].ToString() + "              " + dr["CardNum"].ToString() + "         " +
                    dr["BorrowDate"].ToString() + "      " + dr["BookName"].ToString() + "\r\n";
                    booknum += 1;  //借的书的数量
                    
                    System.DateTime borrowdate = Convert.ToDateTime(dr["BorrowDate"]); //获取借阅时间
                    borrowdate = borrowdate.AddMonths(1);       //截止日期
                    //MessageBox.Show(borrowdate.ToString()); 
                    
                    if (if_go == 0)                   //判断是否越期，逾期为false，不能借书。
                    {
                        if (System.DateTime.Compare(now, borrowdate) < 0)
                        {
                            if_can_borrow = true;
                            //MessageBox.Show("可借"); 
                        }
                        else 
                        {
                            if_can_borrow = false;
                            if_go = 1;
                            //MessageBox.Show("不可借"); 
                        }
                    }
                   
                }
                textBox2.Text = booknum.ToString();
                textBox3.Text = num_book_max.ToString();      //最多可借书数
                textBox8.Text = str1;
                // 判断是否超过最多借书量
                if (booknum >= num_book_max)
                {
                    if_can_borrow = false;
                }
                open_mysql_llm.conn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("打开数据库失败！");
            }

        }

        private void textBox4_MouseLeave(object sender, EventArgs e)   //借书处理2
        {
            
            try
            {
                open_mysql_llm.conn.Open();
                String bookid = textBox4.Text.Trim();
                String strmy = "Select * From bookinformation Where BookID = '" + bookid + "'";  //根据借阅证号查找借书记录
                MySqlCommand cmd = new MySqlCommand(strmy, open_mysql_llm.conn);
                
                /* MySqlDataAdapter da1 = new MySqlDataAdapter();
                da1.SelectCommand = cmd;
                DataSet ds1 = new DataSet();
                da1.Fill(ds1, "bookinformation");*/
                MySqlDataReader dr ;
                dr = cmd.ExecuteReader();

                String str1 = "图书ID           订阅标记           借阅日期                图书名" + "\r\n";

                System.DateTime now = new System.DateTime();   //获取系统时间
                now = System.DateTime.Now;

                //foreach (DataRow row in ds1.Tables["bookinformation"].Rows)
                while(dr.Read())
                {
                    str1 = str1 + dr["BookID"].ToString() + "              " ;
                    if ((int)dr["SendFlag"] == 1)
                    {
                        str1 = str1 + "是" + "                 ";
                    }
                    else 
                    {
                        str1 = str1 + "否" + "                 ";
                    }
                    str1 = str1 + now.ToString() + "     ";
                    //查找图书名
                   /*
                    String bookclassid = dr["BookClassID"].ToString();
                    bookclassid = bookclassid.Trim();
                    MessageBox.Show(bookclassid);
                    String selec = "Select * From tbookclass ";  //Where BookClassID = '" + bookclassid + "'
                   // MySqlCommand cmd2 = new MySqlCommand(selec, open_mysql_llm.conn);

                    DataSet ds = new DataSet();
                    MySqlDataAdapter da1 = new MySqlDataAdapter(selec, open_mysql_llm.conn);
                    MySqlCommandBuilder bda1 = new MySqlCommandBuilder(da1);
                    da1.Fill(ds, "tbook");
                    
                    /*MySqlDataReader dr2;
                    dr2 = cmd2.ExecuteReader();*/
                    /*while (dr2.Read())
                    //foreach (DataRow row in ds1.Tables["tbookclass"].Rows)
                    {
                        str1 = str1 + dr2["BookName"].ToString();
                    }*/
                  


                }
                textBox9.Text = str1;

                open_mysql_llm.conn.Close();






            }
            catch (Exception ex)
            {
                MessageBox.Show("打开数据库失败！");
            }
        }

     
    }
}
