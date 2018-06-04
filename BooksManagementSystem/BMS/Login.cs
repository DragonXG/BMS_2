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
                bool flag_1 = false, flag_2 = false;
                string str1 = "";         //用来记录读取数据库的用户名
                string str2 = "";        //用来记录读取数据库的密码
                string str3 = "";       //用来记录读取数据库的姓名
                MySqlCommand cmd = new MySqlCommand("select * from reader;", conn);
                MySqlDataReader read_info;
                read_info = cmd.ExecuteReader();
                while (read_info.Read())
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
                }
                if (flag_1 == false)
                {
                    errorProvider1.SetError(textBox1, "用户名不正确，请重新输入!");
                    textBox1.Clear();
                }
                if (flag_2 == false)
                {
                    errorProvider2.SetError(textBox2, "密码不正确，请重新输入！");
                    textBox2.Clear();
                }
                if ((flag_1 == true) && (flag_2 == true))
                {
                    ReaderMain readmain = new ReaderMain(str3);
                    Program.checkIn = true;
                    this.Close();
                }
                read_info.Close();
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
    }
}
