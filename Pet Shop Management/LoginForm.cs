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
    public partial class LoginForm: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        SqlDataReader SqlDataReader;
        dbConnect dbConnect = new dbConnect();

        public LoginForm()
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string _name = "";
                string _role = "";

                SqlConnection.Open();
                SqlCommand = new SqlCommand("SELECT name, role FROM tbUser WHERE name=@name AND password = @password", SqlConnection);
                SqlCommand.Parameters.AddWithValue("@name", txtName.Text);
                SqlCommand.Parameters.AddWithValue("@password", txtPass.Text);
                SqlDataReader = SqlCommand.ExecuteReader();
                SqlDataReader.Read();

                if(SqlDataReader.HasRows)
                {
                    _name = SqlDataReader["name"].ToString();
                    _role = SqlDataReader["role"].ToString();

                    MessageBox.Show("Welcome " + _name + "!", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm mainForm = new MainForm();
                    mainForm.labelusername.Text = _name;
                    mainForm.labelrole.Text = _role;

                    if(_role == "Administrator")
                    {
                        mainForm.btnUser.Enabled = true;
                        mainForm.btnUser.Visible = true;
                    }

                    this.Hide();
                    mainForm.ShowDialog();
                }

                else
                {
                    MessageBox.Show("Login Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonForget_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact the administrator", "Forget Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }      
    }
}
