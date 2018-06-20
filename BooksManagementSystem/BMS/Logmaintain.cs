using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace BMS
{
    public partial class Logmaintain : Form
    {
        public Logmaintain()
        {
            InitializeComponent();
        }

        private void Logmaintain_Load(object sender, EventArgs e)
        {
            string sFilePath = System.Environment.CurrentDirectory + DateTime.Now.ToString("yyyyMM");
            string sFileName = "rizhi" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径

            StreamReader sRead = new StreamReader(sFileName, Encoding.UTF8);
            string vline;
            while ((vline = sRead.ReadLine()) != null)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = vline;
                listView1.Items.Add(lvi);
            }
            sRead.Close();
             

        }
    }
}
