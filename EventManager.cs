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
    public partial class EventManager : Form
    {
        public EventManager()
        {
            InitializeComponent();
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            EmCalendar newForm = new EmCalendar();
            newForm.Show();
            this.Hide();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            Supplier newForm = new Supplier();
            newForm.Show();
            this.Hide();
        }

        

        private void btnPayments_Click(object sender, EventArgs e)
        {

        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            Product newForm = new Product();
            newForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        private void btnVoucher_Click(object sender, EventArgs e)
        {
            Voucher newForm = new Voucher();
            newForm.Show();
            this.Hide();
        }
    }
}
