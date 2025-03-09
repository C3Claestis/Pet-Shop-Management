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


        private void dataGridViewCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridViewCash.Columns[e.ColumnIndex].Name;

        removeItem:
            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this item?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbConnect.executeQuery("DELETE FROM tbCash WHERE cashId = " + dataGridViewCash.Rows[e.RowIndex].Cells[1].Value.ToString());
                    MessageBox.Show("Item has been deleted successfully!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            else if (colName == "Increase")
            {
                int i = checkPqty(dataGridViewCash.Rows[e.RowIndex].Cells[2].Value.ToString());

                if(int.Parse(dataGridViewCash.Rows[e.RowIndex].Cells[4].Value.ToString()) < i)
                {
                    dbConnect.executeQuery("UPDATE tbCash SET qty = qty + " + 1 + " WHERE cashId LIKE '" + dataGridViewCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");                    
                }
                else
                {
                    MessageBox.Show("The quantity of this item has reached the maximum limit!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }                
            }

            else if (colName == "Decrease")
            {
                if (int.Parse(dataGridViewCash.Rows[e.RowIndex].Cells[4].Value.ToString()) == 1)
                {
                    colName = "Delete";
                    goto removeItem;
                }

                dbConnect.executeQuery("UPDATE tbCash SET qty = qty - " + 1 + " WHERE cashId = " + dataGridViewCash.Rows[e.RowIndex].Cells[1].Value.ToString());
            }

            LoadCash();
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
                double total = 0;
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
                    total += double.Parse(SqlDataReader[5].ToString());
                }
                SqlDataReader.Close();
                SqlConnection.Close();
                labelTotal.Text = total.ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
        public int checkPqty(string pcode)
        {
            int i = 0;

            try
            {
                SqlConnection.Open();
                SqlCommand = new SqlCommand("SELECT pqty FROM tbProduct WHERE pcode LIKE '" + pcode + "'", SqlConnection);
                i = int.Parse(SqlCommand.ExecuteScalar().ToString());
                SqlConnection.Close();
            }
            catch (Exception ex)
            {
                SqlConnection.Close();
                MessageBox.Show(ex.Message);
            }

            return i;
        }
        #endregion METHOD        
    }
}
