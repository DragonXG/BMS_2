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
    public partial class Login : Form
    {
        public static string LOGTYPE;
        public static string LOGNAME;
        public Login()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;

            Timer time1 = new Timer();
            time1.Interval = 1000;
            time1.Tick += new System.EventHandler(timer1_Tick);
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String str = "Server=localhost;Database=bms;Uid=root;password=123456;sslmode=none;";
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            //读者登陆
            if (comboBox1.Text == "读者")
            {
                bool flag_1 = false, flag_2 = false, flag_3 = false;
                string str1 = "";         //用来记录读取数据库的用户名
                string str2 = "";        //用来记录读取数据库的密码
                string str3 = "";       //用来记录读取数据库的姓名
                MySqlCommand cmd = new MySqlCommand("select * from reader;", conn);
                MySqlDataReader read_info;
                read_info = cmd.ExecuteReader();
                //普通用户登陆
                while (read_info.Read())               //从数据库中扫描匹配的用户名和密码
                {
                    str1 = Convert.ToString(read_info["CardNum"]);
                    str2 = Convert.ToString(read_info["LodinKey"]);

                    if ((textBox1.Text == str1))
                    {
                        flag_1 = true;
                        str3 = Convert.ToString(read_info["ReaderName"]);
                    }
                    if (textBox2.Text == str2)
                    {
                        flag_2 = true;
                    }
                    if ((textBox1.Text == str1) && (textBox2.Text == str2))
                    {
                        flag_3 = true;
                    }
                }
                if (flag_1 == false)        //用户名不正确，给出提示
                {
                    errorProvider1.SetError(textBox1, "用户名不存在，请重新输入!");
                    textBox1.Clear();
                    textBox2.Clear();
                }
                if (flag_2 == false)       //密码不正确，给出提示
                {
                    errorProvider2.SetError(textBox2, "密码不正确，请重新输入！");
                    textBox2.Clear();
                }
                if (flag_3 == true)
                {

                    Log.WriteLog( "借阅证号"+ ":" + textBox1.Text + comboBox1.Text + ":" + str3 + "登录成功!" + "\n");


                    LOGTYPE = comboBox1.Text;
                    LOGNAME = textBox1.Text;
                    ReaderMain readmain = new ReaderMain(str3, textBox1.Text);
                    Program.checkin_reader = true;
                    get_number_llm.cardnum = textBox1.Text;
                    get_number_llm.borrow_cardnum = textBox1.Text;
                    get_number_llm.denlumima = textBox2.Text;
                    get_number_llm.borrow_cardnum = textBox1.Text;
                    get_number_llm.reader_admin = 0;
                    this.Close();
                }
                else
                {
                    errorProvider2.SetError(textBox2, "密码不正确，请重新输入！");
                    textBox2.Clear();
                }
                read_info.Close();
                conn.Close();
            }
            //管理员登录
            if (comboBox1.Text == "超级管理员")
            {
                bool admin_flag1 = false, admin_flag2 = false;
                bool admin_flag3 = false;
                string admin_name = "";                 //记录管理员的用户名
                string admin_pwd = "";                   //记录管理员的密码
                MySqlCommand admin = new MySqlCommand("select * from administrator;", conn);
                MySqlDataReader admin_info;
                admin_info = admin.ExecuteReader();
                while (admin_info.Read())                        //从数据库中扫描匹配的用户名和密码
                {
                    admin_name = Convert.ToString(admin_info["LoginName"]);
                    admin_pwd = Convert.ToString(admin_info["LodinKey"]);
                    if (textBox1.Text == admin_name)
                    {
                        admin_flag1 = true;
                    }
                    if (textBox2.Text == admin_pwd)
                    {
                        admin_flag2 = true;
                    }
                    if ((textBox1.Text == admin_name) && (textBox2.Text == admin_pwd))
                    {
                        admin_flag3 = true;
                    }
                }
                if (admin_flag1 == false)            //用户名不正确，给出提示
                {
                    errorProvider1.SetError(textBox1, "用户名不存在，请重新输入!");
                    textBox1.Clear();
                    textBox2.Clear();
                }
                if (admin_flag2 == false)            //密码不正确，给出提示
                {
                    errorProvider2.SetError(textBox2, "密码不正确，请重新输入！");
                    textBox2.Clear();
                }
                if (admin_flag3 == true)
                {


                    Log.WriteLog(comboBox1.Text + ":" + textBox1.Text + "登录成功!" + "\n");

                    LOGTYPE = comboBox1.Text;
                    LOGNAME = textBox1.Text;
                    AdminMain readmain = new AdminMain(textBox1.Text);
                    Program.checkin_admin = true;
                    get_number_llm.cardnum = textBox1.Text;
                    get_number_llm.borrow_cardnum = textBox1.Text;
                    get_number_llm.denlumima = textBox2.Text;
                    get_number_llm.borrow_cardnum = textBox1.Text;
                    get_number_llm.reader_admin = 1;
                    this.Close();
                }
                else
                {
                    errorProvider2.SetError(textBox2, "密码不正确，请重新输入！");
                    textBox2.Clear();
                }
                admin_info.Close();
                conn.Close();
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
 
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public string StringValue
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QueryBeforeLogin form = new QueryBeforeLogin();
            form.ShowDialog();
            //Program.checkin_querybefore = true;
           // this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label7.Text = DateTime.Now.ToLongTimeString().ToString();

        }
    }
}
