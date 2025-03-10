using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pet_Shop_Management
{
    public partial class CashProduct: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        SqlDataReader SqlDataReader;
        dbConnect dbConnect = new dbConnect();

        CashForm cashForm;
        public string uName;
        public CashProduct(CashForm form)
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
            LoadProduct();
            cashForm = form;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridViewProduct.Rows)
            {
                bool checkBox = Convert.ToBoolean(row.Cells["Select"].Value);
                if (checkBox)
                {
                    try
                    {
                        SqlCommand = new SqlCommand("INSERT INTO tbCash(transno, pcode, pname, qty, price, cashier) VALUES (@transno, @pcode, @pname, @qty, @price, @cashier)", SqlConnection);
                        SqlCommand.Parameters.AddWithValue("@transno", cashForm.labelTransno.Text);
                        SqlCommand.Parameters.AddWithValue("@pcode", row.Cells[1].Value.ToString());
                        SqlCommand.Parameters.AddWithValue("@pname", row.Cells[2].Value.ToString());
                        SqlCommand.Parameters.AddWithValue("@qty", 1);
                        SqlCommand.Parameters.AddWithValue("@price", Convert.ToDouble(row.Cells[5].Value.ToString()));
                        SqlCommand.Parameters.AddWithValue("@cashier", uName);

                        SqlConnection.Open();
                        SqlCommand.ExecuteNonQuery();
                        SqlConnection.Close();
                    }
                    catch (Exception ex)
                    {
                        SqlConnection.Close();
                        MessageBox.Show(ex.Message);
                    }                    
                    
                }
            }

            cashForm.LoadCash();
            this.Dispose();
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        #region METHOD
        public void LoadProduct()
        {
            int i = 0;
            dataGridViewProduct.Rows.Clear();
            SqlCommand = new SqlCommand("SELECT pcode, pname, ptype, pcategory, pprice FROM tbProduct WHERE CONCAT(pname,ptype,pcategory) LIKE '%" +
                textSearch.Text + "%' AND pqty > "+0+"", SqlConnection);
            SqlConnection.Open();
            SqlDataReader = SqlCommand.ExecuteReader();
            while (SqlDataReader.Read())
            {
                i++;
                dataGridViewProduct.Rows.Add(i, SqlDataReader[0].ToString(), SqlDataReader[1].ToString(),
                    SqlDataReader[2].ToString(), SqlDataReader[3].ToString(), SqlDataReader[4].ToString());                    
            }
            SqlDataReader.Close();
            SqlConnection.Close();
        }
        #endregion METHOD      
    }
}
