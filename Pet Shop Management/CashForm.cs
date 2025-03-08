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
    public partial class CashForm: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        dbConnect dbConnect = new dbConnect();
        SqlDataReader SqlDataReader;

        MainForm mainForm;
        public CashForm(MainForm mainForm)
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
            this.mainForm = mainForm;
            getTransno();
            LoadCash();            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CashProduct cashProduct = new CashProduct(this);
            cashProduct.uName = mainForm.labelusername.Text;
            cashProduct.ShowDialog();
        }

        private void btnCash_Click(object sender, EventArgs e)
        {

        }

        #region METHOD
        public void getTransno()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transno;

                SqlConnection.Open();
                SqlCommand = new SqlCommand("SELECT TOP 1 transno FROM tbCash WHERE transno LIKE '" + sdate + "%' ORDER BY cashId DESC", SqlConnection);
                SqlDataReader = SqlCommand.ExecuteReader();
                SqlDataReader.Read();

                if (SqlDataReader.HasRows)
                {
                    transno = SqlDataReader[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    labelTransno.Text = sdate + (count + 1); //.ToString("0000");
                }
                else
                {
                    transno = sdate + "1001";
                    labelTransno.Text = transno;
                }

                SqlDataReader.Close();
                SqlConnection.Close();
            }
            catch (Exception ex)
            {
                SqlConnection.Close();
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadCash()
        {
            try
            {
                int i = 0;
                dataGridViewCash.Rows.Clear();
                SqlCommand = new SqlCommand("SELECT cashId,pcode,pname,qty,price,total,c.name,cashier FROM tbCash as cash LEFT JOIN tbCustomer c ON cash.cid = c.id WHERE transno LIKE "+labelTransno.Text+"", SqlConnection);
                SqlConnection.Open();
                SqlDataReader = SqlCommand.ExecuteReader();
                while (SqlDataReader.Read())
                {
                    i++;
                    dataGridViewCash.Rows.Add(i, SqlDataReader[0].ToString(), SqlDataReader[1].ToString(),
                        SqlDataReader[2].ToString(), SqlDataReader[3].ToString(), SqlDataReader[4].ToString(),
                        SqlDataReader[5].ToString(), SqlDataReader[6].ToString(), SqlDataReader[7].ToString());
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
