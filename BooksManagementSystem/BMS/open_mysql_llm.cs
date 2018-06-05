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
        public static String strConn = "Server = localhost;Database = library;Uid = root;password = 19971230llm; sslmode = none; Charset = utf8";
        public static MySqlConnection conn = new MySqlConnection(strConn);

    }
}
