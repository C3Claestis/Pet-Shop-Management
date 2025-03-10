using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Pet_Shop_Management
{
    public partial class MainForm: Form
    {
        SqlConnection SqlConnection = new SqlConnection();
        SqlCommand SqlCommand = new SqlCommand();
        SqlDataReader SqlDataReader;
        dbConnect dbConnect = new dbConnect();

        public MainForm()
        {
            InitializeComponent();
            SqlConnection = new SqlConnection(dbConnect.connection());
            btnDashboard.PerformClick();
            loadDailySales();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }        

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            openChildForm(new Dashboard());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm());
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            openChildForm(new UserForm());
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            openChildForm(new ProductForm());
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            openChildForm(new CashForm(this));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                this.Dispose();
                loginForm.ShowDialog();
            }               
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timeProgress.Invoke((MethodInvoker)delegate
            {
                timeProgress.Text = DateTime.Now.ToString("HH:mm:ss");
                timeProgress.Value = Convert.ToInt32(DateTime.Now.Second);
            });
        }

        #region METHOD
        private Form activeForm = null;
        public void openChildForm(Form Childform)
        {
            if(activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = Childform;
            Childform.TopLevel = false;
            Childform.FormBorderStyle = FormBorderStyle.None;
            Childform.Dock = DockStyle.Fill;
            labelTittle.Text = Childform.Text;

            panelChild.Controls.Add(Childform);
            panelChild.Tag = Childform;
            Childform.BringToFront();
            Childform.Show();
        }

        public void loadDailySales()
        {
            string sdate = DateTime.Now.ToString("yyyyMMdd");

            try
            {
                SqlConnection.Open();
                SqlCommand = new SqlCommand("SELECT ISNULL(SUM(total),0) AS total FROM tbCash WHERE transno LIKE'" + sdate + "%'", SqlConnection);
                labelSales.Text = double.Parse(SqlCommand.ExecuteScalar().ToString()).ToString("#,#00.00");
                SqlConnection.Close();
            }
            catch (Exception ex)
            {
                SqlConnection.Close();
                MessageBox.Show(ex.Message);
            }
        }
        #endregion METHOD        
    }
}
