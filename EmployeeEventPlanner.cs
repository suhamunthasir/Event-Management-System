using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FinalProject
{
    public partial class EmployeeEventPlanner : Form
    {
        public EmployeeEventPlanner()
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

        private void btnEventAssistant_Click(object sender, EventArgs e)
        {
            EmployeeEventAssistant newForm = new EmployeeEventAssistant();
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

        private void btnEventPlanner_Click(object sender, EventArgs e)
        {

        }
        private bool ValidateForm()
        {
            // Validate Name
            if (string.IsNullOrWhiteSpace(txtEpName.Text))
            {
                MessageBox.Show("Please enter the name.");
                txtEpName.Focus();
                return false;
            }

            // Validate NIC
            if (string.IsNullOrWhiteSpace(txtEpNIC.Text))
            {
                MessageBox.Show("Please enter the NIC.");
                txtEpNIC.Focus();
                return false;
            }

            // Validate Contact
            if (string.IsNullOrWhiteSpace(txtEpContact.Text) || !txtEpContact.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid contact number.");
                txtEpContact.Focus();
                return false;
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(txtEpEmail.Text) || !IsValidEmail(txtEpEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.");
                txtEpEmail.Focus();
                return false;
            }

            // Validate Address
            if (string.IsNullOrWhiteSpace(txtEpAddress.Text))
            {
                MessageBox.Show("Please enter the address.");
                txtEpAddress.Focus();
                return false;
            }

            // Validate Salary
            if (string.IsNullOrWhiteSpace(txtEpSalary.Text) || !decimal.TryParse(txtEpSalary.Text, out decimal salary) || salary <= 0)
            {
                MessageBox.Show("Please enter a valid salary.");
                txtEpSalary.Focus();
                return false;
            }

            // Validate Username
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter a username.");
                txtUsername.Focus();
                return false;
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(txtEpPassword.Text))
            {
                MessageBox.Show("Please enter a password.");
                txtEpPassword.Focus();
                return false;
            }

            return true;
        }

        // Helper method for email validation
        private bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return mail.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                // Proceed with adding data to the database
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    string query = @"
            INSERT INTO EventPlanner (name, nic, contact, email, address, specializedFields, salary, username, password)
            VALUES (@name, @nic, @contact, @email, @address, @specializedFields, @salary, @username, @password)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtEpName.Text);
                    cmd.Parameters.AddWithValue("@nic", txtEpNIC.Text);
                    cmd.Parameters.AddWithValue("@contact", txtEpContact.Text);
                    cmd.Parameters.AddWithValue("@email", txtEpEmail.Text);
                    cmd.Parameters.AddWithValue("@address", txtEpAddress.Text);
                    cmd.Parameters.AddWithValue("@specializedFields", txtSpecializedFields.Text);
                    cmd.Parameters.AddWithValue("@salary", decimal.Parse(txtEpSalary.Text));
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@password", txtEpPassword.Text);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Event Planner added successfully.");
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    string query = @"
            UPDATE EventPlanner
            SET name = @name, nic = @nic, contact = @contact, email = @email, address = @address,
                specializedFields = @specializedFields, salary = @salary, username = @username, password = @password
            WHERE nic = @nic";  // Update query using NIC as the unique identifier

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtEpName.Text);
                    cmd.Parameters.AddWithValue("@nic", txtEpNIC.Text);
                    cmd.Parameters.AddWithValue("@contact", txtEpContact.Text);
                    cmd.Parameters.AddWithValue("@email", txtEpEmail.Text);
                    cmd.Parameters.AddWithValue("@address", txtEpAddress.Text);
                    cmd.Parameters.AddWithValue("@specializedFields", txtSpecializedFields.Text);
                    cmd.Parameters.AddWithValue("@salary", decimal.Parse(txtEpSalary.Text));
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@password", txtEpPassword.Text);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Event Planner details updated successfully.");
                            ClearFields();
                            txtSearch.Clear();
                        }
                        else
                        {
                            MessageBox.Show("No record found with the specified NIC.");
                            txtSearch.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                MessageBox.Show("Please enter NIC to delete a record.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(conString))
            {
                string query = "DELETE FROM EventPlanner WHERE nic = @nic";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nic", txtSearch.Text);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Event Planner record deleted successfully.");
                        ClearFields();
                        txtSearch.Clear();
                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified NIC.");
                        txtSearch.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void lblSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                MessageBox.Show("Please enter NIC to search.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(conString))
            {
                string query = "SELECT * FROM EventPlanner WHERE nic = @nic";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nic", txtSearch.Text);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        txtEpName.Text = reader["name"].ToString();
                        txtEpContact.Text = reader["contact"].ToString();
                        txtEpEmail.Text = reader["email"].ToString();
                        txtEpAddress.Text = reader["address"].ToString();
                        txtSpecializedFields.Text = reader["specializedFields"].ToString();
                        txtEpSalary.Text = reader["salary"].ToString();
                        txtUsername.Text = reader["username"].ToString();
                        txtEpPassword.Text = reader["password"].ToString();
                        txtEpNIC.Text = reader["nic"].ToString();

                    }
                    else
                    {
                        MessageBox.Show("No record found with the specified NIC.");
                        ClearFields();
                        txtSearch.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void ClearFields()
        {
            txtEpName.Clear();
            txtEpNIC.Clear();
            txtEpContact.Clear();
            txtEpEmail.Clear();
            txtEpAddress.Clear();
            txtSpecializedFields.Clear();
            txtEpSalary.Clear();
            txtUsername.Clear();
            txtEpPassword.Clear();
        }

    }
}
