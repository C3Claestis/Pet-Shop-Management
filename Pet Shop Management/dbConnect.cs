﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pet_Shop_Management
{
    class dbConnect
    {
        SqlConnection connectionSql = new SqlConnection();
        SqlCommand command = new SqlCommand();
        private string con;

        public string connection()
        {
            con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\AplikasiWindows\Pet Shop Management\Pet Shop Management\dbUsagiShop.mdf;Integrated Security=True";
            return con;
        }

        public void executeQuery(String query)
        {
            try
            {
                connectionSql.ConnectionString = connection();
                connectionSql.Open();
                command = new SqlCommand(query, connectionSql);
                command.ExecuteNonQuery();
                connectionSql.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
