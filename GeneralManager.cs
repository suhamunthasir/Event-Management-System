using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class GeneralManager : Form
    {
        public GeneralManager()
        {
            InitializeComponent();
        }

        private void btnEmployeeDetails_Click(object sender, EventArgs e)
        {
            Employee newForm = new Employee();
            newForm.Show();
            this.Hide();
        }

        private void btnFinancialBudget_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
           Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        private void btnEventDetails_Click(object sender, EventArgs e)
        {

          GmEvent newForm = new GmEvent();
            newForm.Show();
            this.Hide();
        }

        private void btnSupplierDetails_Click(object sender, EventArgs e)
        {
            GmSupplier newForm = new GmSupplier();
            newForm.Show();
            this.Hide();
        }

        private void btnCustomerDetails_Click(object sender, EventArgs e)
        {
            GmCustomer newForm = new GmCustomer();
            newForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            warehouse newForm = new warehouse();
            newForm.Show();
            this.Hide();
        }
    }
}
