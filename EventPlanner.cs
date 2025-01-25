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
    public partial class EventPlanner : Form
    {
        public EventPlanner()
        {
            InitializeComponent();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            Customer newForm = new Customer();
            newForm.Show();
            this.Hide();
        }

        private void btnEvent_Click(object sender, EventArgs e)
        {
            Event newForm = new Event();
            newForm.Show();
            this.Hide();
        }

        private void btnFeedback_Click(object sender, EventArgs e)
        {
           Feedback newForm = new Feedback();
            newForm.Show();
            this.Hide();
        }

        private void btnChecklist_Click(object sender, EventArgs e)
        {
           Checklist newForm = new Checklist();
            newForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        private void btnCalender_Click(object sender, EventArgs e)
        {
            Calendar newForm = new Calendar();
            newForm.Show();
            this.Hide();
        }

        private void btnEventHistory_Click(object sender, EventArgs e)
        {
           EventHistory newForm = new EventHistory();
            newForm.Show();
            this.Hide();
        }

        private void btnMeetings_Click(object sender, EventArgs e)
        {
           Meetings newForm = new Meetings();
            newForm.Show();
            this.Hide();
        }
    }
}
