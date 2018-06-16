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
        
        public Login()
        {
            InitializeComponent();
            textBox1.Text = "2016080808";
            textBox2.Text = "111111";
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            String str = "Server=localhost;Database=bms;Uid=root;password=123456;sslmode=none;";
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            if (comboBox1.Text == "读者")
            {
                bool flag_1 = false, flag_2 = false, flag_3 = false;
                string str1 = "";         //用来记录读取数据库的用户名
                string str2 = "";        //用来记录读取数据库的密码
                string str3 = "";       //用来记录读取数据库的姓名
                
                //QueryBeforeLogin form1 = new QueryBeforeLogin(str1);
                

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
                    if((textBox1.Text == str1) && (textBox2.Text == str2))
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
                    ReaderMain readmain = new ReaderMain(str3, textBox1.Text);
                    Program.checkin_reader = true;
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
                while(admin_info.Read())                        //从数据库中扫描匹配的用户名和密码
                {
                    admin_name = Convert.ToString(admin_info["LoginName"]);
                    admin_pwd  = Convert.ToString(admin_info["LodinKey"]);
                    if(textBox1.Text == admin_name)
                    {
                        admin_flag1 = true;
                    }
                    if (textBox2.Text == admin_pwd)
                    {
                        admin_flag2 = true;
                    }
                    if((textBox1.Text == admin_name) && (textBox2.Text == admin_pwd))
                    {
                        admin_flag3 = true;
                    }
                }
                if(admin_flag1 == false)            //用户名不正确，给出提示
                {
                    errorProvider1.SetError(textBox1, "用户名不存在，请重新输入!");
                    textBox1.Clear();
                    textBox2.Clear();
                } 
                if(admin_flag2 == false)            //密码不正确，给出提示
                {
                    errorProvider2.SetError(textBox2, "密码不正确，请重新输入！");
                    textBox2.Clear();
                }
                if (admin_flag3 == true)
                {
                    AdminMain readmain = new AdminMain();
                    Program.checkin_admin = true;
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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
