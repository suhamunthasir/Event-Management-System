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
    public partial class Meetings : Form
    {
        public Meetings()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";
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
        private void LoadCustomers()
        {
            comboBoxCustomerID.Items.Clear(); // Clear previous items

            string query = "SELECT CustomerID, Full_Name FROM Customer";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int customerID = Convert.ToInt32(reader["CustomerID"]);
                        string customerName = reader["Full_Name"].ToString();

                        // Create a new Customer object and add it to the ComboBox
                        comboBoxCustomerID.Items.Add(new Customer { CustomerID = customerID, CustomerName = customerName });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customers: " + ex.Message);
                }
            }
        }
        public class Customer
        {
            public int CustomerID { get; set; }
            public string CustomerName { get; set; }

            public override string ToString()
            {
                return CustomerName;
            }
        }




        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (comboBoxCustomerID.SelectedItem == null || dateTimePicker1.Value == null)
            {
                MessageBox.Show("Please select a Customer and choose a Meeting Date.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string customerName = comboBoxCustomerID.SelectedItem.ToString();
            DateTime meetingDate = dateTimePicker1.Value;

            string query = "INSERT INTO Meeting (CustomerName, MeetingDate) VALUES (@CustomerName, @MeetingDate)";

            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true"))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerName", customerName);
                cmd.Parameters.AddWithValue("@MeetingDate", meetingDate);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Meeting added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadMeetings();
                    ClearMeetingFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding meeting: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadMeetings()
        {
            string query = "SELECT MeetingID, CustomerName, MeetingDate FROM Meeting";

            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true"))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoResizeColumns();
                    dataGridView1.AllowUserToAddRows = false; 
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading meetings: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void Meetings_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            LoadMeetings();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is selected
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                string customerName = row.Cells["CustomerName"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["MeetingDate"].Value);

                // Find and set the matching item in the combo box
                for (int i = 0; i < comboBoxCustomerID.Items.Count; i++)
                {
                    if (comboBoxCustomerID.GetItemText(comboBoxCustomerID.Items[i]) == customerName)
                    {
                        comboBoxCustomerID.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a meeting to update.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxCustomerID.SelectedItem == null || dateTimePicker1.Value == null)
            {
                MessageBox.Show("Please select a Customer and choose a Meeting Date.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected meeting ID
            int meetingId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MeetingID"].Value);
            string customerName = comboBoxCustomerID.SelectedItem.ToString();
            DateTime meetingDate = dateTimePicker1.Value;

            string query = "UPDATE Meeting SET CustomerName = @CustomerName, MeetingDate = @MeetingDate WHERE MeetingID = @MeetingID";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerName", customerName);
                cmd.Parameters.AddWithValue("@MeetingDate", meetingDate);
                cmd.Parameters.AddWithValue("@MeetingID", meetingId);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Meeting updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMeetings(); // Refresh the data grid
                        ClearMeetingFields(); // Clear the input fields
                    }
                    else
                    {
                        MessageBox.Show("Meeting update failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating meeting: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ClearMeetingFields()
        {
            comboBoxCustomerID.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;  
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a meeting to delete.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected meeting ID
            int meetingId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MeetingID"].Value);

            string query = "DELETE FROM Meeting WHERE MeetingID = @MeetingID";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MeetingID", meetingId);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Meeting deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMeetings(); // Refresh the data grid
                        ClearMeetingFields(); // Clear the input fields
                    }
                    else
                    {
                        MessageBox.Show("Meeting deletion failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting meeting: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
