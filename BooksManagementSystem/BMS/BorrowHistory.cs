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
    public partial class BorrowHistory : Form
    {
        public BorrowHistory()
        {
            InitializeComponent();
        }
        public BorrowHistory(string text)
            :this()
        {
            textBox1.Text = text;
        }
        private void BorrowHistory_Load(object sender, EventArgs e)
        {
            String str = "Server=localhost;Database=bms;Uid=root;password=123456;sslmode=none;";
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("select * from reader;", conn);
            MySqlDataReader borrow_info;
            string str1 = "";
            str1 = textBox1.Text;
            borrow_info = cmd.ExecuteReader();
            while(borrow_info.Read())
            {
                
                if(str1 == Convert.ToString(borrow_info["CardNum"]))
                {
                    textBox2.Text = Convert.ToString(borrow_info["ReaderName"]);
                }
            }
        }

    }
}
