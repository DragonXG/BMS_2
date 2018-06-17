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


//####################################   注意在open_mysql_llm类里添加reader_admin，cardnum，denlumima相关信息
namespace BMS
{
    public partial class PwdChange : Form
    {
        public PwdChange()
        {
            InitializeComponent();
        }
        int next = 0;   //是否进行下一步,原密码
        int next2 = 0;  //是否进行下一步，新密码对比。

        private void button1_Click(object sender, EventArgs e)
        {
            String yumima = textBox1.Text.Trim();
            next = 0;
            next2 = 0;


            if (get_number_llm.denlumima != yumima)
            {
                next = 0;
                label4.Text = "密码不正确！请重新输入！";
                //MessageBox.Show("密码不正确！请重新输入！");
            }
            else
            {
                next = 1;
            }
            

            if (next != 1)
            {
                label4.Text = "请重新输入原密码！";
                //MessageBox.Show("请重新输入原密码！");
            }
            else if (textBox2.Text.Count() != 6)
            {
                label4.Text = "密码为六位！请重新输入！";
                //MessageBox.Show("密码为六位！请重新输入！");
                textBox2.Text = "";
                textBox3.Text = "";
            }
            else
            {
                if (textBox2.Text != textBox3.Text)
                {
                    label4.Text = "两次新密码输入不一样！请重新输入！";
                    //MessageBox.Show("两次新密码输入不一样！请重新输入！");
                    textBox2.Text = "";
                    textBox3.Text = "";
                }
                else
                {
                    next2 = 1;
                }

            }
            
           
            if(next == 1 && next2 == 1)
            {
                try
                {
                    open_mysql_llm.conn.Open();
                    if (get_number_llm.reader_admin == 0) //为读者修改密码。
                    {
                        DataSet dsmydata = new DataSet();
                        //String cardnum = "0";//类里获取账号哪一类
                        String strmy_reader = "Select * From reader Where CardNum = '" + get_number_llm.cardnum + "'";
                        MySqlDataAdapter dareader = new MySqlDataAdapter(strmy_reader, open_mysql_llm.conn);
                        MySqlCommandBuilder bdreader = new MySqlCommandBuilder(dareader);
                        dareader.Fill(dsmydata, "reader");

                        foreach (DataRow row in dsmydata.Tables["reader"].Rows)
                        {
                            row["LodinKey"] = textBox3.Text.Trim();
                        }
                        dareader.Update(dsmydata, "reader");
                        dsmydata.Tables["reader"].AcceptChanges();

                       get_number_llm.denlumima = textBox3.Text.Trim();
                        label4.Text = "修改成功！";
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        
                        //MessageBox.Show("修改成功！");
                        //this.Close();


                    }
                    else     //为管理员修改密码
                    {
                        DataSet dsmydata = new DataSet();
                        //String cardnum = "0";//类里获取账号哪一类
                        String strmy_administrator = "Select * From administrator Where LoginName = '" + get_number_llm.cardnum + "'";
                        MySqlDataAdapter daadministrator = new MySqlDataAdapter(strmy_administrator, open_mysql_llm.conn);
                        MySqlCommandBuilder bdadministrator = new MySqlCommandBuilder(daadministrator);
                        daadministrator.Fill(dsmydata, "administrator");

                        foreach (DataRow row in dsmydata.Tables["administrator"].Rows)
                        {
                            row["LodinKey"] = textBox3.Text.Trim();
                        }
                        daadministrator.Update(dsmydata, "administrator");
                        dsmydata.Tables["administrator"].AcceptChanges();
                        label4.Text = "修改成功！";
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        get_number_llm.denlumima = textBox3.Text.Trim();
                        //MessageBox.Show("修改成功！");
                        //this.Close();

                    }
                    open_mysql_llm.conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
                }
            }

            //this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)

        {

            String yumima = textBox1.Text.Trim();
            next = 0;


            if (get_number_llm.denlumima != yumima)
            {

                next = 0;
                MessageBox.Show("密码不正确！请重新输入！");
            }
            else
            {
                next = 1;
            }


          
        }

        private void textBox3_MouseLeave(object sender, EventArgs e)
        {


            next2 = 0;


            if (next != 1)
            {
                MessageBox.Show("请重新输入原密码！");
            }
            else if (textBox2.Text.Count() != 6)
            {
                MessageBox.Show("密码为六位！请重新输入！");
                textBox2.Text = "";
                textBox3.Text = "";
            }
            else
            {
                if (textBox2.Text != textBox3.Text)
                {
                    MessageBox.Show("两次新密码输入不一样！请重新输入！");
                    textBox2.Text = "";
                    textBox3.Text = "";
                }
                else
                {

                    try
                    {
                        open_mysql_llm.conn.Open();
                        if (get_number_llm.reader_admin == 0) //为读者修改密码。
                        {
                            DataSet dsmydata = new DataSet();
                            //String cardnum = "0";//类里获取账号哪一类
                            String strmy_reader = "Select * From reader Where CardNum = '" + get_number_llm.cardnum + "'";
                            MySqlDataAdapter dareader = new MySqlDataAdapter(strmy_reader, open_mysql_llm.conn);
                            MySqlCommandBuilder bdreader = new MySqlCommandBuilder(dareader);
                            dareader.Fill(dsmydata, "reader");

                            foreach (DataRow row in dsmydata.Tables["reader"].Rows)
                            {
                                row["LodinKey"] = textBox3.Text.Trim();
                            }
                            dareader.Update(dsmydata, "reader");
                            dsmydata.Tables["reader"].AcceptChanges();
                            MessageBox.Show("修改成功！");
                            this.Close();


                        }
                        else     //为管理员修改密码
                        {
                            DataSet dsmydata = new DataSet();
                            //String cardnum = "0";//类里获取账号哪一类

                            String strmy_administrator = "Select * From administrator Where LoginName = '" + get_number_llm.cardnum + "'";

                            MySqlDataAdapter daadministrator = new MySqlDataAdapter(strmy_administrator, open_mysql_llm.conn);
                            MySqlCommandBuilder bdadministrator = new MySqlCommandBuilder(daadministrator);
                            daadministrator.Fill(dsmydata, "administrator");

                            foreach (DataRow row in dsmydata.Tables["administrator"].Rows)
                            {
                                row["LodinKey"] = textBox3.Text.Trim();
                            }
                            daadministrator.Update(dsmydata, "administrator");
                            dsmydata.Tables["administrator"].AcceptChanges();
                            MessageBox.Show("修改成功！");
                            this.Close();

                        }
                        open_mysql_llm.conn.Close();
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message.ToString() + "打开数据库失败！");
                    }

                    next2 = 1;

                }
            }
        }
            


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
