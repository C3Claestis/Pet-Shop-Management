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
    public partial class CustomerModule: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        SqlDataReader SqlDataReader;
        dbConnect dbConnect = new dbConnect();

        bool check = false;
        CustomerForm customerForm;
        public CustomerModule(CustomerForm form)
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
            customerForm = form;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();

                if (check)
                {
                    if (MessageBox.Show("Ingin register dengan customer ini?", "Customer Registration",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand = new SqlCommand("INSERT INTO tbCustomer (name,address,phone)VALUES" +
                            "(@name,@address,@phone)", SqlConnection);
                        SqlCommand.Parameters.AddWithValue("@name", txtName.Text);
                        SqlCommand.Parameters.AddWithValue("@address", txtAddress.Text);
                        SqlCommand.Parameters.AddWithValue("@phone", txtPhone.Text);                      

                        SqlConnection.Open();
                        SqlCommand.ExecuteNonQuery();
                        SqlConnection.Close();
                        MessageBox.Show("Registrasi berhasil", "Customer Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        customerForm.LoadCustomer();
                    }
                }
            }
            catch (Exception)
            {
                SqlConnection.Close();
                MessageBox.Show("Registrasi gagal", "Customer Registration", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();

                if (check)
                {
                    if (MessageBox.Show("Ingin update akun ini?", "Customer Update",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand = new SqlCommand("UPDATE tbCustomer SET name=@name, address=@address, phone=@phone WHERE id=@id", SqlConnection);
                        SqlCommand.Parameters.AddWithValue("@id", labelCID.Text);
                        SqlCommand.Parameters.AddWithValue("@name", txtName.Text);
                        SqlCommand.Parameters.AddWithValue("@address", txtAddress.Text);
                        SqlCommand.Parameters.AddWithValue("@phone", txtPhone.Text);                      

                        SqlConnection.Open();
                        SqlCommand.ExecuteNonQuery();
                        SqlConnection.Close();
                        MessageBox.Show("Update data berhasil", "Customer Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        customerForm.LoadCustomer();
                        this.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                SqlConnection.Close();
                MessageBox.Show("Update gagal", "Customer Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #region METHOD
        public void CheckField()
        {
            if (txtName.Text.Trim() == "" || txtAddress.Text.Trim() == "" || txtPhone.Text.Trim() == "")
            {
                MessageBox.Show("Field tidak boleh kosong", "Customer Registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            check = true;
        }

        public void clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();

            buttonSave.Enabled = true;
            buttonUpdate.Enabled = false;
        }
        #endregion METHOD

       
    }
}
