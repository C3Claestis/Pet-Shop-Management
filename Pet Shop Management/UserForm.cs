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
    public partial class UserForm: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        SqlDataReader SqlDataReader;
        dbConnect dbConnect = new dbConnect();

        public UserForm()
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
            LoadUser();
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            LoadUser();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModule module = new UserModule(this);
            module.ShowDialog();
        }

        #region Method
        private void dataGridViewUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dataGridViewUser.Columns[e.ColumnIndex].Name;
            
            if(colname == "Edit")
            {
                UserModule userModule = new UserModule(this);
                userModule.labelUID.Text = dataGridViewUser.Rows[e.RowIndex].Cells[1].Value.ToString();
                userModule.txtName.Text = dataGridViewUser.Rows[e.RowIndex].Cells[2].Value.ToString();
                userModule.txtAddress.Text = dataGridViewUser.Rows[e.RowIndex].Cells[3].Value.ToString();
                userModule.txtPhone.Text = dataGridViewUser.Rows[e.RowIndex].Cells[4].Value.ToString();
                userModule.comboBoxRole.Text = dataGridViewUser.Rows[e.RowIndex].Cells[5].Value.ToString();
                userModule.dateTimePickerDob.Value = DateTime.Parse(dataGridViewUser.Rows[e.RowIndex].Cells[6].Value.ToString());
                userModule.txtPassword.Text = dataGridViewUser.Rows[e.RowIndex].Cells[7].Value.ToString();

                userModule.buttonSave.Enabled = false;
                userModule.buttonUpdate.Enabled = true;
                userModule.ShowDialog();
            }
            else if(colname == "Delete")
            {
                if(MessageBox.Show("Ingin menghapus data ini?", caption: "Delete User",
                    buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbConnect.executeQuery("DELETE FROM tbUser WHERE id LIKE'" + dataGridViewUser.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    //SqlCommand = new SqlCommand("DELETE FROM tbUser WHERE id = @id", SqlConnection);
                    //SqlCommand.Parameters.AddWithValue("@id", dataGridViewUser.Rows[e.RowIndex].Cells[1].Value.ToString());
                    //SqlConnection.Open();
                    //SqlCommand.ExecuteNonQuery();
                    //SqlConnection.Close();
                    MessageBox.Show("Data berhasil dihapus", "Delete User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            LoadUser();
        }

        public void LoadUser()
        {
            int i = 0;
            dataGridViewUser.Rows.Clear();
            SqlCommand = new SqlCommand("SELECT * FROM tbUser WHERE CONCAT(name,address,phone,dob,role) LIKE '%" + 
                textSearch.Text + "%'", SqlConnection);
            SqlConnection.Open();
            SqlDataReader = SqlCommand.ExecuteReader();
            while (SqlDataReader.Read())
            {
                i ++;
                dataGridViewUser.Rows.Add(i, SqlDataReader[0].ToString(), SqlDataReader[1].ToString(),
                    SqlDataReader[2].ToString(), SqlDataReader[3].ToString(), SqlDataReader[4].ToString(),
                    DateTime.Parse(SqlDataReader[5].ToString()).ToShortDateString(), SqlDataReader[6].ToString());
            }
            SqlDataReader.Close();
            SqlConnection.Close();
        }

        #endregion Method        
    }
}
