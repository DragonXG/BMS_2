﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMS
{
    public partial class if_message_pay : Form
    {
        public if_message_pay()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PayForOverdue f1 = new PayForOverdue(get_number_llm.cardnum);
            string strlog = "还书成功并进入缴费页面.\n";
            Log.WriteLog(strlog);
            f1.Show();
            this.Close();
        }
    }
}
