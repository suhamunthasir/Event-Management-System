using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FinalProject
{
    public partial class Event : Form
    {
        public Event()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";
        

        private void btnProduct_Click(object sender, EventArgs e)
        {
            EpProduct newForm = new EpProduct();
            newForm.Show();
            this.Hide();
        }

        private void btnChecklist_Click_1(object sender, EventArgs e)
        {
            Checklist newForm = new Checklist();
            newForm.Show();
            this.Hide();
        }

        private void btnGoback_Click(object sender, EventArgs e)
        {

            EventPlanner newForm = new EventPlanner();
            newForm.Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (!ValidateForm())
            {
                return;
            }

            string customerNIC = txtCustomerNIC.Text;
            string eventCategory = cmbBoxCategory.SelectedItem.ToString();
            DateTime eventDate = dateTimePickerEventDate.Value; 
            string eventVenue = txtVenue.Text;
            string eventAssistant = cmbBoxEventAssistant.SelectedItem.ToString();
            string logisticManager = cmbBoxLogisticManager.SelectedItem.ToString();

            int customerId= GetCustomerIDFromNIC(customerNIC);
            if (customerId == -1)
            {
                MessageBox.Show("No customer found with the provided NIC.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearForm();
                return;
            }

            // SQL query to insert event into the Events table
            string query = "INSERT INTO Events (CustomerID, EventDate, EventVenue, EventAssistantName, LogisticManagerName, Category) " +
                           "VALUES (@CustomerID, @EventDate, @EventVenue, @EventAssistantName, @LogisticManagerName, @Category)";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                
                int customerID = GetCustomerIDFromNIC(customerNIC); 

                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@EventDate", eventDate);
                cmd.Parameters.AddWithValue("@EventVenue", eventVenue);
                cmd.Parameters.AddWithValue("@EventAssistantName", eventAssistant);
                cmd.Parameters.AddWithValue("@LogisticManagerName", logisticManager);
                cmd.Parameters.AddWithValue("@Category", eventCategory);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event saved successfully!");
                    LoadEventData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private int GetCustomerIDFromNIC(string nic)
        {
            string query = "SELECT CustomerID FROM Customer WHERE NIC = @NIC";
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NIC", nic);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;  // Return -1 if no customer found
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return -1;
                }
            }
        }

        private void Event_Load(object sender, EventArgs e)
        {
            LoadEventAssistants();
            LoadLogisticManagers();
            LoadEventData();
        }

        private void LoadEventAssistants()
        {
            string query = "SELECT Name FROM EventAssistant";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cmbBoxEventAssistant.Items.Add(reader["Name"].ToString());
                    }

                    if (cmbBoxEventAssistant.Items.Count > 0)
                    {
                        cmbBoxEventAssistant.SelectedIndex = 0; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void LoadEventData()
        {
            
            string query = "SELECT e.EventID, c.NIC, e.EventDate, e.EventVenue, e.EventAssistantName, e.LogisticManagerName, e.Category " +
                           "FROM Events e " +
                           "INNER JOIN Customer c ON e.CustomerID = c.CustomerID";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    da.Fill(dt); 
                    dataGridView1.DataSource = dt;

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoResizeColumns();
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.ReadOnly = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        // Load Logistic Managers from the LogisticManager table
        private void LoadLogisticManagers()
        {
            string query = "SELECT Name FROM LogisticManager";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cmbBoxLogisticManager.Items.Add(reader["Name"].ToString());
                    }

                    if (cmbBoxLogisticManager.Items.Count > 0)
                    {
                        cmbBoxLogisticManager.SelectedIndex = 0; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

               
                txtCustomerNIC.Text = row.Cells["NIC"].Value.ToString(); 
                dateTimePickerEventDate.Text = row.Cells["EventDate"].Value.ToString(); 
                txtVenue.Text = row.Cells["EventVenue"].Value.ToString(); 
                cmbBoxEventAssistant.Text = row.Cells["EventAssistantName"].Value.ToString(); 
                cmbBoxLogisticManager.Text = row.Cells["LogisticManagerName"].Value.ToString(); 
                cmbBoxCategory.Text = row.Cells["Category"].Value.ToString(); 

            }
        }
        private bool ValidateForm()
        {
           
            if (string.IsNullOrWhiteSpace(txtCustomerNIC.Text))
            {
                MessageBox.Show("Please enter the Customer NIC.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (cmbBoxCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Event Category.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(dateTimePickerEventDate.Text) || !DateTime.TryParse(dateTimePickerEventDate.Text, out _))
            {
                MessageBox.Show("Please enter a valid Event Date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

         
            if (string.IsNullOrWhiteSpace(txtVenue.Text))
            {
                MessageBox.Show("Please enter the Event Venue.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            
            if (cmbBoxEventAssistant.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Event Assistant.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            
            if (cmbBoxLogisticManager.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Logistic Manager.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

          
            

            
            return true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0 || dataGridView1.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows.Count > 0
                    ? dataGridView1.SelectedRows[0]
                    : dataGridView1.CurrentRow;

                int eventID = Convert.ToInt32(selectedRow.Cells["EventID"].Value);

                if (!ValidateForm())
                {
                    return;
                }

                string customerNIC = txtCustomerNIC.Text;
                string eventCategory = cmbBoxCategory.SelectedItem.ToString();
                DateTime eventDate = dateTimePickerEventDate.Value;
                string eventVenue = txtVenue.Text;
                string eventAssistant = cmbBoxEventAssistant.SelectedItem.ToString();
                string logisticManager = cmbBoxLogisticManager.SelectedItem.ToString();
                int customerID = GetCustomerIDFromNIC(customerNIC);

                string query = "UPDATE Events " +
                               "SET CustomerID = @CustomerID, EventDate = @EventDate, EventVenue = @EventVenue, " +
                               "EventAssistantName = @EventAssistantName, LogisticManagerName = @LogisticManagerName, Category = @Category " +
                               "WHERE EventID = @EventID";

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.Parameters.AddWithValue("@EventDate", eventDate);
                    cmd.Parameters.AddWithValue("@EventVenue", eventVenue);
                    cmd.Parameters.AddWithValue("@EventAssistantName", eventAssistant);
                    cmd.Parameters.AddWithValue("@LogisticManagerName", logisticManager);
                    cmd.Parameters.AddWithValue("@Category", eventCategory);
                    cmd.Parameters.AddWithValue("@EventID", eventID);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Event updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadEventData();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("Error: No event found to update.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an event to update.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ClearForm()
        {
            txtCustomerNIC.Clear();
            txtVenue.Clear();
            cmbBoxCategory.SelectedIndex = -1;
            cmbBoxEventAssistant.SelectedIndex = -1;
            cmbBoxLogisticManager.SelectedIndex = -1;
            dateTimePickerEventDate.Value = DateTime.Today;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int eventID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["EventID"].Value);

                string query = "DELETE FROM Events WHERE EventID = @EventID";

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EventID", eventID);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Event deleted successfully!");
                        LoadEventData();
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an event to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
          print newForm = new print();
            newForm.Show();
            this.Hide();
        }
    }
}
