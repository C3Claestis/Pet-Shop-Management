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
    public partial class CustomerForm: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        SqlDataReader SqlDataReader;
        dbConnect dbConnect = new dbConnect();

        public CustomerForm()
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
            LoadCustomer();
        }

        private void dataGridViewCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dataGridViewCustomer.Columns[e.ColumnIndex].Name;

            if (colname == "Edit")
            {
                CustomerModule userModule = new CustomerModule(this);
                userModule.labelCID.Text = dataGridViewCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                userModule.txtName.Text = dataGridViewCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                userModule.txtAddress.Text = dataGridViewCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
                userModule.txtPhone.Text = dataGridViewCustomer.Rows[e.RowIndex].Cells[4].Value.ToString();

                userModule.buttonSave.Enabled = false;
                userModule.buttonUpdate.Enabled = true;
                userModule.ShowDialog();
            }
            else if (colname == "Delete")
            {
                if (MessageBox.Show("Ingin menghapus data customer ini?", caption: "Delete Customer",
                    buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbConnect.executeQuery("DELETE FROM tbCustomer WHERE id LIKE'" + dataGridViewCustomer.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");               
                    MessageBox.Show("Data berhasil dihapus", "Delete Customer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            LoadCustomer();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModule customerModule = new CustomerModule(this);
            customerModule.ShowDialog();
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        #region METHOD
        public void LoadCustomer()
        {
            int i = 0;
            dataGridViewCustomer.Rows.Clear();
            SqlCommand = new SqlCommand("SELECT * FROM tbCustomer WHERE CONCAT(name,address,phone) LIKE '%" +
                textSearch.Text + "%'", SqlConnection);
            SqlConnection.Open();
            SqlDataReader = SqlCommand.ExecuteReader();
            while (SqlDataReader.Read())
            {
                i++;
                dataGridViewCustomer.Rows.Add(i, SqlDataReader[0].ToString(), SqlDataReader[1].ToString(),
                   SqlDataReader[2].ToString(), SqlDataReader[3].ToString());
            }
            SqlDataReader.Close();
            SqlConnection.Close();
        }
        #endregion METHOD
    }
}
