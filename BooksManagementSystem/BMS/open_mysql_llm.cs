using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BMS
{
    public static class open_mysql_llm
    {
        //记着改密码，用户名
        public static String strConn = "Server = localhost;Database = BMS;Uid = root;password = 123456; sslmode = none; Charset = utf8";
        public static MySqlConnection conn = new MySqlConnection(strConn);

    }
    public static class get_number_llm
    {
        public static int reader_admin = 0;  //看到底是什么要修改密码，0为读者，1为管理员
        public static String cardnum = "0";  //获得账号
        public static String denlumima = "0";  //获得登录密码
    }
}
