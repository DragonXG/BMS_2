using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMS
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// 
        static public bool checkin_reader=false;
        static public bool checkin_admin = false;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Login());

            if(checkin_reader == true)
            {
                Application.Run(new ReaderMain());
            }
            if(checkin_admin == true)
            {
                Application.Run(new AdminMain());
            }       

        }
    }
}
