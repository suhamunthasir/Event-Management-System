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

namespace FinalProject
{
    public partial class EventHistory : Form
    {
        public EventHistory()
        {
            InitializeComponent();
        }

       

       

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        private void btnGoback_Click(object sender, EventArgs e)
        {
           EventPlanner newForm = new EventPlanner();
            newForm.Show();
            this.Hide();
        }

        private void lblSearch_Click(object sender, EventArgs e)
        {

            // Validate if NIC is entered
            if (string.IsNullOrEmpty(txtNIC.Text.Trim()))
            {
                MessageBox.Show("Please enter a NIC to search.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string customerNIC = txtNIC.Text.Trim();

            string query = @"
        SELECT 
            e.EventID,
            e.EventDate,
            e.Category,
            e.EventVenue,
            e.EventAssistantName,
            e.LogisticManagerName,
            ISNULL(SUM(ep.TotalPrice), 0) AS TotalAmount
        FROM 
            Events e
        INNER JOIN 
            Customer c ON e.CustomerID = c.CustomerID
        LEFT JOIN 
            EventProduct ep ON e.EventID = ep.EventID
        WHERE 
            c.NIC = @CustomerNIC
        GROUP BY 
            e.EventID, e.EventDate, e.Category, e.EventVenue, e.EventAssistantName, e.LogisticManagerName";

            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true"))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerNIC", customerNIC);

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        // Bind data to the DataGridView
                        dgvEventDetails.DataSource = dt;
                        dgvEventDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dgvEventDetails.AutoResizeColumns();
                        dgvEventDetails.AllowUserToAddRows = false;
                        dgvEventDetails.ReadOnly = true;
                    }
                    else
                    {
                        MessageBox.Show("No events found for the provided NIC.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvEventDetails.DataSource = null; // Clear grid if no data
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching events: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
