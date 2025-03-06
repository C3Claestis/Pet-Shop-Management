using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Pet_Shop_Management
{
    public partial class UserModule: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        dbConnect dbConnect = new dbConnect();

        UserForm userForm;
        bool check = false;
        public UserModule(UserForm userForm)
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
            this.userForm = userForm;
            comboBoxRole.SelectedIndex = 1;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                CheckField();

                if (check)
                {
                    if (MessageBox.Show("Ingin register dengan akun ini?", "User Registration",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand = new SqlCommand("INSERT INTO tbUser (name,address,phone,role,dob,password)VALUES" +
                            "(@name,@address,@phone,@role,@dob,@password)", SqlConnection);
                        SqlCommand.Parameters.AddWithValue("@name", txtName.Text);
                        SqlCommand.Parameters.AddWithValue("@address", txtAddress.Text);
                        SqlCommand.Parameters.AddWithValue("@phone", txtPhone.Text);
                        SqlCommand.Parameters.AddWithValue("@role", comboBoxRole.Text);
                        SqlCommand.Parameters.AddWithValue("@dob", dateTimePickerDob.Value);
                        SqlCommand.Parameters.AddWithValue("@password", txtPassword.Text);

                        SqlConnection.Open();
                        SqlCommand.ExecuteNonQuery();
                        SqlConnection.Close();
                        MessageBox.Show("Registrasi berhasil", "User Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        userForm.LoadUser();
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
                    if (MessageBox.Show("Ingin update akun ini?", "User Update",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand = new SqlCommand("UPDATE tbUser SET name=@name, address=@address, phone=@phone, role=@role, dob=@dob, password=@password WHERE id=@id", SqlConnection);
                        SqlCommand.Parameters.AddWithValue("@id", labelUID.Text);
                        SqlCommand.Parameters.AddWithValue("@name", txtName.Text);
                        SqlCommand.Parameters.AddWithValue("@address", txtAddress.Text);
                        SqlCommand.Parameters.AddWithValue("@phone", txtPhone.Text);
                        SqlCommand.Parameters.AddWithValue("@role", comboBoxRole.Text);
                        SqlCommand.Parameters.AddWithValue("@dob", dateTimePickerDob.Value);
                        SqlCommand.Parameters.AddWithValue("@password", txtPassword.Text);

                        SqlConnection.Open();
                        SqlCommand.ExecuteNonQuery();
                        SqlConnection.Close();
                        MessageBox.Show("Update data berhasil", "User Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        userForm.LoadUser();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void comboBoxRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxRole.Text == "Employe")
            {
                this.Height = 450 - 28;
                labelPass.Visible = false;
                txtPassword.Visible = false;
            }
            else
            {
                this.Height = 450;
                labelPass.Visible = true;
                txtPassword.Visible = true;
            }
        }

        #region Method
        public void clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            comboBoxRole.SelectedIndex = 0;
            dateTimePickerDob.Value = DateTime.Now;
            txtPassword.Clear();

            buttonUpdate.Enabled = false;
        }

        public void CheckField()
        {
            if (txtName.Text.Trim() == "" || txtAddress.Text.Trim() == "" || txtPhone.Text.Trim() == "")
            {
                MessageBox.Show("Field tidak boleh kosong", "User Registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(checkAge(dateTimePickerDob.Value) < 18)
            {
                MessageBox.Show("Umur harus lebih dari 18 tahun", "User Registration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            check = true;
        }

        private static int checkAge(DateTime dateTime)
        {
            int age = DateTime.Now.Year - dateTime.Year;
            if(DateTime.Now.DayOfYear < dateTime.DayOfYear)
            {
                age = age - 1;                
            }
            return age;
        }
        #endregion Method
    }
}
