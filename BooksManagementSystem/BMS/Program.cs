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
        static public bool checkin_reader=false;                 //是否进入读者界面
        static public bool checkin_admin = false;               //是否进入管理员界面
        static public bool checkin_querybefore = false;     //是否进入预查询界面
        static public bool checkin_login = false;                 //是否进入登陆界面
        static public int check = 1;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          
                Application.Run(new Login());

                if (checkin_querybefore == true)
                {
                    Application.Run(new QueryBeforeLogin());
                }
                if (checkin_login == true)
                {
                    Application.Run(new Login());
                }
                if (checkin_reader == true)
                {
                    Application.Run(new ReaderMain());
                }
                if (checkin_admin == true)
                {
                    Application.Run(new AdminMain());
                }
          
        }
    }
}
