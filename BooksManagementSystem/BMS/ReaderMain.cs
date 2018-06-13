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
    public partial class ReaderMain : Form
    {
        static public string str_cardname = "", str_cardnum = "";
        public ReaderMain()
        {
            InitializeComponent();
            open_mysql_llm.conn.Open();
            DataSet dsmydata = new DataSet();
            MySqlCommand cmd = new MySqlCommand("select * from systemprompt where CardNum ='" + str_cardnum + "'", open_mysql_llm.conn);
            MySqlDataAdapter da1 = new MySqlDataAdapter();
            da1.SelectCommand = cmd;
            da1.Fill(dsmydata, "systemprompt");
            foreach (DataRow row in dsmydata.Tables["systemprompt"].Rows)
            {
                if (row[0].ToString() == str_cardnum)
                {
                    MessageBox.Show(row[1].ToString());
                }
            }
            MySqlCommand cmd3 = new MySqlCommand();
            cmd3.Connection = open_mysql_llm.conn;
            cmd3.CommandText = "delete from systemprompt where CardNum = '" + str_cardnum + "'";
            cmd3.ExecuteNonQuery();
            open_mysql_llm.conn.Close();
        }
        public ReaderMain(string text_name, string text_num)
            :this()
        {
            str_cardname = text_name;
            str_cardnum = text_num;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            PwdChange Form = new PwdChange();
            Form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReaderInfo Form = new ReaderInfo();
            Form.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ReaderInfo readinfo = new ReaderInfo(label2.Text);
            readinfo.Show();
        }

        private void ReaderMain_Load(object sender, EventArgs e)
        {
            label2.Text = str_cardname;
            str_cardname = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            BorrowHistory borrowhistory = new BorrowHistory(str_cardnum);
            borrowhistory.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PayForOverdue form = new PayForOverdue(str_cardnum);
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BorrowReturn borrowreturn = new BorrowReturn();
            borrowreturn.ShowDialog();
        }
    }
}
