﻿using System;
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

        public static int reader_admin = 1;  //看到底是什么要修改密码，0为读者，1为管理员
        public static String cardnum = "WXH";  //获得登账号
        public static String denlumima = "123123";  //获得登录密码
        public static String borrow_cardnum = "2016081002";  //获得借书的CardNum，也为登录账号。
        public static String str_message = "";   //逾期提示信息。不用管，我自己使用


        //注意添加该函数！！！！！！！！！！！！！！！！！！！！！！！！
        public static int become_ture = 0; //在交了费以后，该值赋值为1;并使if_can_borrow变为真值；

        //public static String bookid = "";  //测试；

    }
    
}
