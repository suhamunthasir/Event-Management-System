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
    public partial class EmployeeLogisticManager : Form
    {
        public EmployeeLogisticManager()
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

        private void btnEventAssistant_Click(object sender, EventArgs e)
        {
            EmployeeEventAssistant newForm = new EmployeeEventAssistant();
            newForm.Show();
            this.Hide();
        }

        private void btnLogisticManager_Click(object sender, EventArgs e)
        {

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
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtLmName.Text))
            {
                MessageBox.Show("Name is required.");
                txtLmName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtLmNIC.Text))
            {
                MessageBox.Show("NIC is required.");
                txtLmNIC.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtLmEmail.Text) || !IsValidEmail(txtLmEmail.Text))
            {
                MessageBox.Show("Enter a valid Email Address.");
                txtLmEmail.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtLmContact.Text))
            {
                MessageBox.Show("Contact number is required.");
                txtLmContact.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtLmAddress.Text))
            {
                MessageBox.Show("Address is required.");
                txtLmAddress.Focus();
                return false;
            }
            if (numericUpDown1.Value <= 0)
            {
                MessageBox.Show("Work Hours must be greater than zero.");
                numericUpDown1.Focus();
                return false;
            }
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked)
            {
                MessageBox.Show("Select at least one language.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLmSalary.Text) || !decimal.TryParse(txtLmSalary.Text, out _))
            {
                MessageBox.Show("Enter a valid Salary.");
                txtLmSalary.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.");
                txtUsername.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtLmPassword.Text))
            {
                MessageBox.Show("Password is required.");
                txtLmPassword.Focus();
                return false;
            }
            return true;
        }

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
        private string GetSelectedLanguages()
        {
            List<string> languages = new List<string>();
            if (checkBox1.Checked) languages.Add("Sinhala");
            if (checkBox2.Checked) languages.Add("Tamil");
            if (checkBox3.Checked) languages.Add("English");
            return string.Join(",", languages);
        }

        private void SetSelectedLanguages(string languages)
        {
            checkBox1.Checked = languages.Contains("Sinhala");
            checkBox2.Checked = languages.Contains("Tamil");
            checkBox3.Checked = languages.Contains("English");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            using (SqlConnection conn = new SqlConnection(conString))
            {
                string query = "INSERT INTO LogisticManager (name, nic, email, contact, address, workHours, languagesSpoken, salary, username, password) " +
                               "VALUES (@name, @nic, @email, @contact, @address, @workHours, @languagesSpoken, @salary, @username, @password)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", txtLmName.Text);
                cmd.Parameters.AddWithValue("@nic", txtLmNIC.Text);
                cmd.Parameters.AddWithValue("@email", txtLmEmail.Text);
                cmd.Parameters.AddWithValue("@contact", txtLmContact.Text);
                cmd.Parameters.AddWithValue("@address", txtLmAddress.Text);
                cmd.Parameters.AddWithValue("@workHours", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@languagesSpoken", GetSelectedLanguages());
                cmd.Parameters.AddWithValue("@salary", decimal.Parse(txtLmSalary.Text));
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtLmPassword.Text);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Logistic Manager added successfully.");
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
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                MessageBox.Show("Enter NIC to search.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(conString))
            {
                string query = "SELECT * FROM LogisticManager WHERE nic = @nic";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nic", txtSearch.Text.Trim()); // Trim input to remove extra spaces

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        txtLmName.Text = reader["name"].ToString();
                        txtLmEmail.Text = reader["email"].ToString();
                        txtLmContact.Text = reader["contact"].ToString();
                        txtLmAddress.Text = reader["address"].ToString();
                        numericUpDown1.Value = Convert.ToInt32(reader["workHours"]);
                        txtLmSalary.Text = reader["salary"].ToString();
                        txtUsername.Text = reader["username"].ToString();
                        txtLmPassword.Text = reader["password"].ToString();
                        txtLmNIC.Text = reader["NIC"].ToString();

                        SetSelectedLanguages(reader["languagesSpoken"].ToString());
                        
                    }
                    else
                    {
                        MessageBox.Show("No record found.");
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
            txtLmName.Clear();
            txtLmNIC.Clear();
            txtLmEmail.Clear();
            txtLmContact.Clear();
            txtLmAddress.Clear();
            numericUpDown1.Value = 0;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            txtLmSalary.Clear();
            txtUsername.Clear();
            txtLmPassword.Clear();
            txtLmName.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            using (SqlConnection conn = new SqlConnection(conString))
            {
                string query = "UPDATE LogisticManager SET name = @name, email = @email, contact = @contact, address = @address, workHours = @workHours, " +
                               "languagesSpoken = @languagesSpoken, salary = @salary, username = @username, password = @password WHERE nic = @nic";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtLmName.Text);
                cmd.Parameters.AddWithValue("@nic", txtLmNIC.Text);
                cmd.Parameters.AddWithValue("@email", txtLmEmail.Text);
                cmd.Parameters.AddWithValue("@contact", txtLmContact.Text);
                cmd.Parameters.AddWithValue("@address", txtLmAddress.Text);
                cmd.Parameters.AddWithValue("@workHours", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@languagesSpoken", GetSelectedLanguages());
                cmd.Parameters.AddWithValue("@salary", decimal.Parse(txtLmSalary.Text));
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtLmPassword.Text);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully.");
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                MessageBox.Show("Enter NIC to delete a record.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(conString))
            {
                string query = "DELETE FROM LogisticManager WHERE nic = @nic";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nic", txtSearch.Text.Trim()); // Use Trim to clean up input

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted successfully.");
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

}
