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
    public partial class ReaderInfo : Form
    {
       /* public ReaderInfo(Login log)
            :this()
        {
            this.log = log;
            textBox5.Text = log.StringValue;
        }*/
        public ReaderInfo()
        {
            InitializeComponent();
        }
        public ReaderInfo(string text)
            :this()
        {
            textBox1.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void ReaderInfo_Load(object sender, EventArgs e)
        {
            String str = "Server=localhost;Database=bms;Uid=root;password=123456;sslmode=none;";
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from reader;", conn);
            MySqlDataReader read_info;
            read_info = cmd.ExecuteReader();
            while (read_info.Read())
            {
                if (textBox1.Text == Convert.ToString(read_info["ReaderName"]))
                {
                    textBox5.Text = Convert.ToString(read_info["CardNum"]);
                    textBox3.Text = Convert.ToString(read_info["Profession"]);
                    textBox4.Text = Convert.ToString(read_info["TelNumber"]);
                    textBox6.Text = Convert.ToString(read_info["ReaderType"]);
                }
            }
            read_info.Close();
            conn.Close();
        }
    }
}
