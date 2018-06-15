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
        public PayForOverdue()
        {
            InitializeComponent();
        }
   //open_mysql_llm.conn.Open();

            //DataSet dsmydata = new DataSet();

            //String strmy_returnedbook = "Select * From returnedbook Where BookID = '"+ get_number_llm.bookid + "'";
            //MySqlDataAdapter dareturnedbook = new MySqlDataAdapter(strmy_returnedbook, open_mysql_llm.conn);
            //MySqlCommandBuilder bdreturnedbook = new MySqlCommandBuilder(dareturnedbook);
            //dareturnedbook.Fill(dsmydata, "returnedbook");

            //foreach (DataRow row in dsmydata.Tables["returnedbook"].Rows)
            //{
            //    row["Arrearage"] = "否";
            //}
            //dareturnedbook.Update(dsmydata, "returnedbook");
            //dsmydata.Tables["returnedbook"].AcceptChanges();
            //get_number_llm.become_ture = 1;
        //open_mysql_llm.conn.Close();
    }
}
