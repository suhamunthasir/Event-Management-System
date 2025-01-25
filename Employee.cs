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
using System.Xml.Linq;

namespace FinalProject
{
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
        }

        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";
        private void btnEventPlanner_Click(object sender, EventArgs e)
        {
            EmployeeEventPlanner newForm = new EmployeeEventPlanner();
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

        private void label5_Click(object sender, EventArgs e)
        {

        }
        private bool ValidateForm()
        {
            // Validate Name
            if (string.IsNullOrEmpty(txtEmName.Text))
            {
                MessageBox.Show("Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate NIC
            if (string.IsNullOrEmpty(txtEmNIC.Text))
            {
                MessageBox.Show("NIC cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Contact
            if (string.IsNullOrEmpty(txtEmContact.Text) || !txtEmContact.Text.All(char.IsDigit))
            {
                MessageBox.Show("Contact number must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Address
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Address cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

           

            // Validate Work Hours
            if (string.IsNullOrEmpty(txtEmWorkHours.Text) || !int.TryParse(txtEmWorkHours.Text, out _))
            {
                MessageBox.Show("Work Hours must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Work Experience
            if (string.IsNullOrEmpty(txtEmWorkExperience.Text))
            {
                MessageBox.Show("Work Experience cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Salary
            if (string.IsNullOrEmpty(txtSalary.Text) )
            {
                MessageBox.Show("Salary cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Username
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Username cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Password
            if (string.IsNullOrEmpty(txtEmPassword.Text))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;  // Stop further execution if validation fails
            }
            string query = "INSERT INTO EventManager (Name, NIC, Contact, Address, EmailAddress, WorkHours, WorkExperience, Salary, Username, Password) " +
                           "VALUES (@Name, @NIC, @Contact, @Address, @EmailAddress, @WorkHours, @WorkExperience, @Salary, @Username, @Password);";

            // Retrieve data from form fields
            string name = txtEmName.Text;
            string nic = txtEmNIC.Text;
            string contact = txtEmContact.Text;
            string address = txtAddress.Text;
            string emailAddress = txtEmEmail.Text;
            int workHours = int.Parse(txtEmWorkHours.Text);
            string workExperience = txtEmWorkExperience.Text;
            decimal salary = decimal.Parse(txtSalary.Text);
            string username = txtUsername.Text;
            string password = txtEmPassword.Text;

            // Insert data into the database
            using (SqlConnection connection = new SqlConnection(conString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@NIC", nic);
                        command.Parameters.AddWithValue("@Contact", contact);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@EmailAddress", emailAddress);
                        command.Parameters.AddWithValue("@WorkHours", workHours);
                        command.Parameters.AddWithValue("@WorkExperience", workExperience);
                        command.Parameters.AddWithValue("@Salary", salary);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password); // Hash this in production!

                        int rowsAffected = command.ExecuteNonQuery();
                        MessageBox.Show($"{rowsAffected} record(s) inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtEmName.Clear();
                        txtEmNIC.Clear();
                        txtEmContact.Clear();
                        txtAddress.Clear();
                        txtEmEmail.Clear();
                        txtEmWorkHours.Clear();
                        txtEmWorkExperience.Clear();
                        txtSalary.Clear();
                        txtUsername.Clear();
                        txtEmPassword.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;  // Stop further execution if validation fails
            }
            string query = "UPDATE EventManager SET Name = @Name, Contact = @Contact, Address = @Address, EmailAddress = @EmailAddress, WorkHours = @WorkHours, WorkExperience = @WorkExperience, Salary = @Salary, Username = @Username, Password = @Password WHERE NIC = @NIC;";

            // Retrieve data from form fields
            string name = txtEmName.Text;
            string nic = txtEmNIC.Text;
            string contact = txtEmContact.Text;
            string address = txtAddress.Text;
            string emailAddress = txtEmEmail.Text;
            int workHours = int.Parse(txtEmWorkHours.Text);
            string workExperience = txtEmWorkExperience.Text;
            decimal salary = decimal.Parse(txtSalary.Text);
            string username = txtUsername.Text;
            string password = txtEmPassword.Text;
           

            using (SqlConnection connection = new SqlConnection(conString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@NIC", nic);
                        command.Parameters.AddWithValue("@Contact", contact);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@EmailAddress", emailAddress);
                        command.Parameters.AddWithValue("@WorkHours", workHours);
                        command.Parameters.AddWithValue("@WorkExperience", workExperience);
                        command.Parameters.AddWithValue("@Salary", salary);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        

                        int rowsAffected = command.ExecuteNonQuery();
                        MessageBox.Show($"{rowsAffected} record(s) updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtEmName.Clear();
                        txtEmNIC.Clear();
                        txtEmContact.Clear();
                        txtAddress.Clear();
                        txtEmEmail.Clear();
                        txtEmWorkHours.Clear();
                        txtEmWorkExperience.Clear();
                        txtSalary.Clear();
                        txtUsername.Clear();
                        txtEmPassword.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lblSearch_Click(object sender, EventArgs e)
        {
            string searchNIC = txtSearch.Text; 

            
            string query = "SELECT * FROM EventManager WHERE NIC = @NIC;";

            

            using (SqlConnection connection = new SqlConnection(conString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@NIC", searchNIC);

                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                txtEmName.Text = reader["Name"].ToString();
                                txtEmNIC.Text = reader["NIC"].ToString();
                                txtEmContact.Text = reader["Contact"].ToString();
                                txtAddress.Text = reader["Address"].ToString();
                                txtEmEmail.Text = reader["EmailAddress"].ToString();
                                txtEmWorkHours.Text = reader["WorkHours"].ToString();
                                txtEmWorkExperience.Text = reader["WorkExperience"].ToString();
                                txtSalary.Text = reader["Salary"].ToString();
                                txtUsername.Text = reader["Username"].ToString();
                                txtEmPassword.Text = reader["Password"].ToString();
                                txtSearch.Clear();
                            }
                            else
                            {
                                MessageBox.Show("No record found with the NIC: " + searchNIC, "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtSearch.Clear();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string nicToDelete = txtEmNIC.Text;

            // Check if the NIC textbox is not empty
            if (string.IsNullOrEmpty(nicToDelete))
            {
                MessageBox.Show("Please enter a NIC to delete a record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // SQL query to delete a record by NIC
            string query = "DELETE FROM EventManager WHERE NIC = @NIC;";

           

            using (SqlConnection connection = new SqlConnection(conString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                      
                        command.Parameters.AddWithValue("@NIC", nicToDelete);

                        
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            
                            txtEmName.Clear();
                            txtEmNIC.Clear();
                            txtEmContact.Clear();
                            txtAddress.Clear();
                            txtEmEmail.Clear();
                            txtEmWorkHours.Clear();
                            txtEmWorkExperience.Clear();
                            txtSalary.Clear();
                            txtUsername.Clear();
                            txtEmPassword.Clear();
                        }
                        else
                        {
                            MessageBox.Show("No record found with the provided NIC.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
