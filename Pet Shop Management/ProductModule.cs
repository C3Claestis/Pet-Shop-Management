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
    public partial class ProductModule: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        dbConnect dbConnect = new dbConnect();

        bool check = false;
        ProductForm product;
        public ProductModule(ProductForm form)
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
            product = form;
            comboBoxCategory.SelectedIndex = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();

                if (check)
                {
                    if (MessageBox.Show("Ingin register Product ini?", "Product Registration",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand = new SqlCommand("INSERT INTO tbProduct (pname,ptype,pcategory,pqty,pprice)VALUES" +
                            "(@pname,@ptype,@pcategory,@pqty,@pprice)", SqlConnection);
                        SqlCommand.Parameters.AddWithValue("@pname", txtName.Text);
                        SqlCommand.Parameters.AddWithValue("@ptype", txtType.Text);
                        SqlCommand.Parameters.AddWithValue("@pcategory", comboBoxCategory.Text);
                        SqlCommand.Parameters.AddWithValue("@pqty", int.Parse(txtQty.Text));
                        SqlCommand.Parameters.AddWithValue("@pprice", double.Parse(txtPrice.Text));                        

                        SqlConnection.Open();
                        SqlCommand.ExecuteNonQuery();
                        SqlConnection.Close();
                        MessageBox.Show("Product berhasil ditambahkan!", "Product Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        product.LoadProduct();
                    }
                }
            }
            catch (Exception)
            {
                SqlConnection.Close();
                MessageBox.Show("Registrasi gagal", "User Registration", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();

                if (check)
                {
                    if (MessageBox.Show("Ingin Edit Product ini?", "Product Update",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand = new SqlCommand("UPDATE tbProduct SET pname=@pname,ptype=@ptype,pcategory=@pcategory,pqty=@pqty,pprice=@pprice WHERE pcode=@pcode", SqlConnection);
                        SqlCommand.Parameters.AddWithValue("@pcode", labelpcode.Text);
                        SqlCommand.Parameters.AddWithValue("@pname", txtName.Text);
                        SqlCommand.Parameters.AddWithValue("@ptype", txtType.Text);
                        SqlCommand.Parameters.AddWithValue("@pcategory", comboBoxCategory.Text);
                        SqlCommand.Parameters.AddWithValue("@pqty", int.Parse(txtQty.Text));
                        SqlCommand.Parameters.AddWithValue("@pprice", double.Parse(txtPrice.Text));

                        SqlConnection.Open();
                        SqlCommand.ExecuteNonQuery();
                        SqlConnection.Close();
                        MessageBox.Show("Product berhasil diperbarui!", "Product Update", MessageBoxButtons.OK, MessageBoxIcon.Information);                        
                        product.LoadProduct();
                        this.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                SqlConnection.Close();
                MessageBox.Show("Registrasi gagal", "User Registration", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            clear();
        }
        

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(char.IsControl(e.KeyChar) || !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        #region METHOD
        public void clear()
        {
            txtName.Clear();
            txtType.Clear();
            txtQty.Clear();
            txtPrice.Clear();
            comboBoxCategory.SelectedIndex = 0;

            buttonUpdate.Enabled = false;
        }

        public void CheckField()
        {
            if (txtName.Text.Trim() == "" || txtType.Text.Trim() == "" || txtQty.Text.Trim() == "" || txtPrice.Text.Trim() == "")
            {
                MessageBox.Show("Field tidak boleh kosong", "Product Registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            check = true;
        }
        #endregion METHOD
    }
}
