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
    public partial class PayForOverdue : Form
    {

        public static string overdays = "3";
        public static string str_cardnum = "";                   //从读者主要的信息表中读取借阅证号

        public PayForOverdue()
        {
            InitializeComponent();
        }

        public PayForOverdue(string cardnum)
            :this()
        {
            str_cardnum = cardnum;
        }
        private Button btn = new Button();
        private void PayForOverdue_Load(object sender, EventArgs e)
        {
            ImageList imgList = new ImageList();                         //设置列表的高度
            imgList.ImageSize = new Size(1, 25); 
            listView1.SmallImageList = imgList;
            Dictionary<string, string> my_dic_1 = new Dictionary<string, string>();   //存借阅日期的字典
            Dictionary<string, string> my_dic_2 = new Dictionary<string, string>();   //存还书日期的字典
            Dictionary<string, string> my_dic_3 = new Dictionary<string, string>();     //存图书名
            textBox1.Text = str_cardnum;
            textBox1.ReadOnly = true;
            //添加列表头
            listView1.Columns.Add("图书ID", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("图书名", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("借阅日期", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("还书日期", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("超过天数", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("缴费操作", 120, HorizontalAlignment.Left);
            string str_bodate = "";                                          //记录借阅日期
            string str_redate = "";                                           //记录还书日期
            TimeSpan overdate = new TimeSpan();              //记录超过天数
            string str_bookid = "";                                          //记录书的ID号
            string str_bookname = "";                                   //记录图书名
            DateTime dt_borrdate = new DateTime();
            DateTime dt_redate = new DateTime();
            String str = "Server=localhost;Database=bms;Uid=root;password=123456;sslmode=none;";
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            MySqlCommand cmd_Pay = new MySqlCommand("select BookID, BookName, BorrowDate, BorrowingStatus from returnedbook where CardNum = '" + str_cardnum + "';",conn);
            MySqlDataReader read_date;
            read_date = cmd_Pay.ExecuteReader();
            while(read_date.Read())
            {
                if(Convert.ToString(read_date["BorrowingStatus"]) == "借阅")
                {
                    str_bookid = Convert.ToString(read_date["BookID"]);
                    str_bodate = Convert.ToString(read_date["BorrowDate"]);
                    my_dic_1.Add(str_bookid, str_bodate);
                }
                if(Convert.ToString(read_date["BorrowingStatus"]) == "已还")
                {
                    str_bookid = Convert.ToString(read_date["BookID"]);
                    str_redate = Convert.ToString(read_date["BorrowDate"]);
                    str_bookname = Convert.ToString(read_date["BookName"]);
                    my_dic_2.Add(str_bookid, str_redate);
                    my_dic_3.Add(str_bookid, str_bookname);
                }
            }
            if((my_dic_1 == null) || (my_dic_2 == null))
            {
                MessageBox.Show("您目前没有需要缴的费用哦!");
            }
            else
            {
                  foreach(KeyValuePair<string, string> key_1 in my_dic_1)
                  {
                      foreach(KeyValuePair<string, string> key_2 in my_dic_2)
                      {
                          if(key_1.Key == key_2.Key)
                          {
                              if(my_dic_3.ContainsKey(key_1.Key))
                              {
                                  string str_value1 = key_1.Value;
                                  string str_value2 = key_2.Value;
                                  dt_borrdate = Convert.ToDateTime(str_value1);      //字符串借阅日期转为时间类型
                                  dt_redate = Convert.ToDateTime(str_value2);         //字符串还书日期转为时间类型
                                  overdate = dt_redate.AddDays(-30) - dt_borrdate;                                  
                                  if(overdate.TotalDays >0)
                                  {
                                      overdays = Convert.ToString(((TimeSpan)(dt_redate.AddDays(-30) - dt_borrdate)).Days);
                                      ListViewItem book = new ListViewItem();                  //添加缴费信息
                                      book.Text = key_1.Key;
                                      book.SubItems.Add(my_dic_3[key_1.Key]);
                                      book.SubItems.Add(str_value1);
                                      book.SubItems.Add(str_value2);
                                      book.SubItems.Add(Convert.ToString(overdate));
                                      book.SubItems.Add("");
                                      listView1.Items.Add(book);
                                  }
                              }
                          }
                      }
                  }
            }
            btn.Visible = false;
            btn.Text = "缴费";                                                   //添加缴费操作事件
            btn.Click += button_Click;
            listView1.Controls.Add(btn);
            btn.Size = new Size(listView1.Items[0].SubItems[5].Bounds.Width, listView1.Items[0].SubItems[5].Bounds.Height);
            read_date.Close();
            conn.Close();
            
        }
        private void button_Click(object sender, EventArgs e)
        {
            double days = Convert.ToDouble(overdays);
            days = days * (0.5);
            PayMoney pay = new PayMoney(Convert.ToString(days));
            pay.ShowDialog();
        }  
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                btn.Location = new Point(listView1.SelectedItems[0].SubItems[5].Bounds.Left, listView1.SelectedItems[0].SubItems[5].Bounds.Top);
                btn.Visible = true;
            }
        }

    }
}
