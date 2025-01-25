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
using System.Xml.Linq;


namespace FinalProject
{
    public partial class EmployeeEventAssistant : Form
    {
        public EmployeeEventAssistant()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";
        private void btnEventManager_Click(object sender, EventArgs e)
        {
           Employee newForm = new Employee();
            newForm.Show();
            this.Hide();
        }

        private void btnEventPlanner_Click(object sender, EventArgs e)
        {
            EmployeeEventPlanner newForm = new EmployeeEventPlanner();
            newForm.Show();
            this.Hide();
        }

        private void btnLogisticManager_Click(object sender, EventArgs e)
        {
            EmployeeLogisticManager newForm = new EmployeeLogisticManager();
            newForm.Show();
            this.Hide();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            GeneralManager newForm = new GeneralManager();
            newForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
           Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        private bool ValidateForm()
        {
            // Validate Name
            if (string.IsNullOrEmpty(txtEaName.Text))
            {
                MessageBox.Show("Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate NIC
            if (string.IsNullOrEmpty(txtEaNIC.Text))
            {
                MessageBox.Show("NIC cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Contact
            if (string.IsNullOrEmpty(txtEaContact.Text) || !txtEaContact.Text.All(char.IsDigit))
            {
                MessageBox.Show("Contact number must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Address
            if (string.IsNullOrEmpty(txtEaAddress.Text))
            {
                MessageBox.Show("Address cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Email Address
            if (string.IsNullOrEmpty(txtEaEmail.Text) || !IsValidEmail(txtEaEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Availability
            if (string.IsNullOrEmpty(txtEaAvailability.Text))
            {
                MessageBox.Show("Availability cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Salary
            if (string.IsNullOrEmpty(txtEaSalary.Text) || !decimal.TryParse(txtEaSalary.Text, out _))
            {
                MessageBox.Show("Salary must be a valid decimal number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Username
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Password
            if (string.IsNullOrEmpty(txtEaPassword.Text))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        // Helper function to validate email format using regular expression
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private void ClearFields()
        {
            // Clear all textboxes
            txtEaName.Clear();
            txtEaNIC.Clear();
            txtEaEmail.Clear();
            txtEaContact.Clear();
            txtEaAddress.Clear();
            txtEaAvailability.Clear();
            txtEaSalary.Clear();
            txtUsername.Clear();
            txtEaPassword.Clear();

            
            numericUpDown1.Value = numericUpDown1.Minimum; 

            
            txtEaName.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate the form first
            if (!ValidateForm())
                return; // Stop if validation fails

            // Create SQL connection
            using (SqlConnection conn = new SqlConnection(conString))
            {
                // SQL Insert Query
                string query = "INSERT INTO EventAssistant (name, nic, emailAddress, contact, address, availability, workExperience, salary, username, password) " +
                               "VALUES (@name, @nic, @emailAddress, @contact, @address, @availability, @workExperience, @salary, @username, @password)";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Add parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@name", txtEaName.Text);
                cmd.Parameters.AddWithValue("@nic", txtEaNIC.Text);
                cmd.Parameters.AddWithValue("@emailAddress", txtEaEmail.Text);
                cmd.Parameters.AddWithValue("@contact", txtEaContact.Text);
                cmd.Parameters.AddWithValue("@address", txtEaAddress.Text);
                cmd.Parameters.AddWithValue("@availability", txtEaAvailability.Text);
                cmd.Parameters.AddWithValue("@workExperience", numericUpDown1.Value); // NumericUpDown control
                cmd.Parameters.AddWithValue("@salary", decimal.Parse(txtEaSalary.Text)); // Ensure salary is numeric
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtEaPassword.Text);

                try
                {
                    // Open connection and execute the query
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event Assistant added successfully.");
                    ClearFields();


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            using (SqlConnection conn = new SqlConnection(conString))
            {
                // SQL Update Query
                string query = "UPDATE EventAssistant SET name = @name, nic = @nic, emailAddress = @emailAddress, contact = @contact, " +
                               "address = @address, availability = @availability, workExperience = @workExperience, salary = @salary, " +
                               "username = @username, password = @password WHERE nic = @nic";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Add parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@name", txtEaName.Text);
                cmd.Parameters.AddWithValue("@nic", txtEaNIC.Text);
                cmd.Parameters.AddWithValue("@emailAddress", txtEaEmail.Text);
                cmd.Parameters.AddWithValue("@contact", txtEaContact.Text);
                cmd.Parameters.AddWithValue("@address", txtEaAddress.Text);
                cmd.Parameters.AddWithValue("@availability", txtEaAvailability.Text);
                cmd.Parameters.AddWithValue("@workExperience", numericUpDown1.Value); 
                cmd.Parameters.AddWithValue("@salary", decimal.Parse(txtEaSalary.Text));
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtEaPassword.Text);

                try
                {
                    // Open connection and execute the query
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event Assistant updated successfully.");
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            

            using (SqlConnection conn = new SqlConnection(conString))
            {
                // SQL Delete Query
                string query = "DELETE FROM EventAssistant WHERE nic = @nic";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Add NIC parameter
                cmd.Parameters.AddWithValue("@nic", txtEaNIC.Text);

                try
                {
                    // Open connection and execute the query
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event Assistant deleted successfully.");
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void lblSearch_Click(object sender, EventArgs e)
        {
            // Validate NIC input
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                MessageBox.Show("Please enter NIC to search.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(conString))
            {
                // SQL Search Query
                string query = "SELECT * FROM EventAssistant WHERE nic = @nic";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Add NIC parameter
                cmd.Parameters.AddWithValue("@nic", txtSearch.Text);

                try
                {
                    // Open connection
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read(); // Read the first result

                        // Fill form fields with data from the database
                        txtEaName.Text = reader["name"].ToString();
                        txtEaEmail.Text = reader["emailAddress"].ToString();
                        txtEaContact.Text = reader["contact"].ToString();
                        txtEaAddress.Text = reader["address"].ToString();
                        txtEaAvailability.Text = reader["availability"].ToString();
                        numericUpDown1.Value = Convert.ToDecimal(reader["workExperience"]);
                        txtEaSalary.Text = reader["salary"].ToString();
                        txtUsername.Text = reader["username"].ToString();
                        txtEaPassword.Text = reader["password"].ToString();
                        txtEaNIC.Text = reader["nic"].ToString();
                        txtSearch.Clear();
                    }
                    else
                    {
                        MessageBox.Show("No records found.");
                        txtSearch.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void EmployeeEventAssistant_Load(object sender, EventArgs e)
        {
            txtEaName.Focus();
        }
    }
}
