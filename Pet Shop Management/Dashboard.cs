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
    public partial class Dashboard: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        SqlDataReader SqlDataReader;
        dbConnect dbConnect = new dbConnect();

        public Dashboard()
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
        }

        #region METHOD
        public int extractData(string sql)
        {
            int data = 0;
            try
            {
                SqlConnection.Open();
                SqlCommand = new SqlCommand("SELECT ISNULL(SUM(pqty),0) AS qty FROM tbProduct WHERE pcategory='"+ sql +"'", SqlConnection);
                data = (int)SqlCommand.ExecuteScalar();
                SqlConnection.Close();
            }
            catch (Exception ex)
            {
                SqlConnection.Close();
                MessageBox.Show(ex.Message);
            }

            return data;
        }

        #endregion METHOD

        private void Dashboard_Load(object sender, EventArgs e)
        {
            labelDog.Text = extractData("Dog").ToString();
            labelDomba.Text = extractData("Domba").ToString();
            labelKucing.Text = extractData("Kucing").ToString();
            labelUsagi.Text = extractData("Usagi").ToString();
        }
    }
}
