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
    public partial class ProductForm: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        SqlDataReader SqlDataReader;
        dbConnect dbConnect = new dbConnect();

        public ProductForm()
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
            LoadProduct();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductModule productModule = new ProductModule(this);
            productModule.ShowDialog();
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void dataGridViewProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dataGridViewProduct.Columns[e.ColumnIndex].Name;

            if (colname == "Edit")
            {
                ProductModule productModule = new ProductModule(this);
                productModule.labelpcode.Text = dataGridViewProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                productModule.txtName.Text = dataGridViewProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                productModule.txtType.Text = dataGridViewProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                productModule.comboBoxCategory.Text = dataGridViewProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                productModule.txtQty.Text = dataGridViewProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                productModule.txtPrice.Text = dataGridViewProduct.Rows[e.RowIndex].Cells[6].Value.ToString();

                productModule.buttonSave.Enabled = false;
                productModule.buttonUpdate.Enabled = true;
                productModule.ShowDialog();
            }
            else if (colname == "Delete")
            {
                if (MessageBox.Show("Ingin menghapus Produk ini?", caption: "Delete Product",
                    buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand = new SqlCommand("DELETE FROM tbProduct WHERE pcode = @pcode", SqlConnection);
                    SqlCommand.Parameters.AddWithValue("@pcode", dataGridViewProduct.Rows[e.RowIndex].Cells[1].Value.ToString());
                    SqlConnection.Open();
                    SqlCommand.ExecuteNonQuery();
                    SqlConnection.Close();
                    MessageBox.Show("Data berhasil dihapus", "Delete Product", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            LoadProduct();
        }

        #region METHOD
        public void LoadProduct()
        {
            int i = 0;
            dataGridViewProduct.Rows.Clear();
            SqlCommand = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(pname,ptype,pcategory) LIKE '%" +
                textSearch.Text + "%'", SqlConnection);
            SqlConnection.Open();
            SqlDataReader = SqlCommand.ExecuteReader();
            while (SqlDataReader.Read())
            {
                i++;
                dataGridViewProduct.Rows.Add(i, SqlDataReader[0].ToString(), SqlDataReader[1].ToString(),
                    SqlDataReader[2].ToString(), SqlDataReader[3].ToString(), SqlDataReader[4].ToString(),
                    SqlDataReader[5].ToString());
            }
            SqlDataReader.Close();
            SqlConnection.Close();
        }
        #endregion METHOD
    }
}
