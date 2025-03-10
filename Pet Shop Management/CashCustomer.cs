using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pet_Shop_Management
{
    public partial class CashCustomer: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        SqlDataReader SqlDataReader;
        dbConnect dbConnect = new dbConnect();

        CashForm cashForm;
        public CashCustomer(CashForm Form)
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
            cashForm = Form;
            LoadCustomer();            
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void dataGridViewCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dataGridViewCustomer.Columns[e.ColumnIndex].Name;
            if(colname == "Choice")
            {
                dbConnect.executeQuery("UPDATE tbCash SET cid=" + dataGridViewCustomer.Rows[e.RowIndex].Cells[1].Value.ToString() + " WHERE transno=" + cashForm.labelTransno.Text + "");

                cashForm.LoadCash();
                this.Dispose();
            }
        }

        #region METHOD
        public void LoadCustomer()
        {            
            try
            {
                int i = 0;
                dataGridViewCustomer.Rows.Clear();
                SqlCommand = new SqlCommand("SELECT id,name,phone FROM tbCustomer WHERE name LIKE '%" + textSearch.Text + "%'", SqlConnection);
                SqlConnection.Open();
                SqlDataReader = SqlCommand.ExecuteReader();
                while (SqlDataReader.Read())
                {
                    i++;
                    dataGridViewCustomer.Rows.Add(i, SqlDataReader[0].ToString(), SqlDataReader[1].ToString(),
                       SqlDataReader[2].ToString());
                }
                SqlDataReader.Close();
                SqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                       
        }
        #endregion METHOD       
    }
}
