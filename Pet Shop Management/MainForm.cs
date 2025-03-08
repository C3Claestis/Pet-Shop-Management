using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pet_Shop_Management
{
    public partial class MainForm: Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {

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

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

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
        #endregion METHOD
    }
}
