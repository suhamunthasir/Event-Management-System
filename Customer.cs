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
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
        }


        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";
        private void Customer_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        private void btnGoback_Click(object sender, EventArgs e)
        {
           EventPlanner newForm = new EventPlanner();
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
            
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required.");
                txtName.Focus();
                return false;
            }

        
            if (string.IsNullOrWhiteSpace(txtContact.Text) || !txtContact.Text.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid contact number.");
                txtContact.Focus();
                return false;
            }

       
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address is required.");
                txtAddress.Focus();
                return false;
            }

            
            if (string.IsNullOrWhiteSpace(txtNIC.Text) || !txtNIC.Text.All(char.IsLetterOrDigit))
            {
                MessageBox.Show("Please enter a valid NIC.");
                txtNIC.Focus();
                return false;
            }

          
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.");
                txtEmail.Focus();
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


        private void btnAdd_Click(object sender, EventArgs e)
        {
           
            if (!ValidateForm())
            {
                return; 
            }

          

            
            string query = "INSERT INTO customer (full_name, contact_number, address, nic, email) " +
                           "VALUES (@FullName, @ContactNumber, @Address, @NIC, @Email)";

           
            using (SqlConnection connection = new SqlConnection(conString))
            {
                try
                {
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                       
                        cmd.Parameters.AddWithValue("@FullName", txtName.Text);
                        cmd.Parameters.AddWithValue("@ContactNumber", txtContact.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@NIC", txtNIC.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);

                      
                        connection.Open();

                        
                        cmd.ExecuteNonQuery();

                        
                        MessageBox.Show("Customer data inserted successfully!");
                        LoadCustomerData();
                        ClearTextFields();
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

        
            SearchCustomerByNIC(txtSearch.Text);

        }

        private void SearchCustomerByNIC(string nic)
        {
          
            string query = "SELECT full_name, contact_number, address, nic, email FROM customer WHERE nic = @NIC";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                try
                {
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                       
                        cmd.Parameters.AddWithValue("@NIC", nic);

                       
                        connection.Open();

                        
                        SqlDataReader reader = cmd.ExecuteReader();

                 
                        if (reader.HasRows)
                        {
                          
                            reader.Read();

                          
                            txtName.Text = reader["full_name"].ToString();
                            txtContact.Text = reader["contact_number"].ToString();
                            txtAddress.Text = reader["address"].ToString();
                            txtNIC.Text = reader["nic"].ToString();
                            txtEmail.Text = reader["email"].ToString();
                            txtSearch.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Customer not found.");
                            txtSearch.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                   
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void ClearTextFields()
        {
            txtName.Clear();
            txtContact.Clear();
            txtAddress.Clear();
            txtNIC.Clear();
            txtEmail.Clear();
        }

        private void LoadCustomerData()
        {
           
            dataGridView.Rows.Clear();


            
            string query = "SELECT customerId, full_name, contact_number, address, nic, email FROM customer";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                try
                {
                  
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            
                            dataGridView.Rows.Add(
                                reader["customerId"].ToString(),
                                reader["full_name"].ToString(),
                                reader["contact_number"].ToString(),
                                reader["address"].ToString(),
                                reader["nic"].ToString(),
                                reader["email"].ToString()
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private int selectedCustomerId;
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];
                selectedCustomerId = Convert.ToInt32(row.Cells["ID"].Value);
                txtName.Text = row.Cells["full_name"].Value.ToString();
                txtContact.Text = row.Cells["contact_number"].Value.ToString();
                txtAddress.Text = row.Cells["address"].Value.ToString();
                txtNIC.Text = row.Cells["nic"].Value.ToString();
                txtEmail.Text = row.Cells["email"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            string query = "UPDATE customer SET full_name = @FullName, contact_number = @ContactNumber, address = @Address, nic = @NIC, email = @Email WHERE customerId= @Id";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@FullName", txtName.Text);
                        cmd.Parameters.AddWithValue("@ContactNumber", txtContact.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@NIC", txtNIC.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@Id", selectedCustomerId);  // `selectedCustomerId` is the ID of the selected customer

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Customer data updated successfully!");
                        ClearTextFields();
                        LoadCustomerData();
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
            if (selectedCustomerId == 0)
            {
                MessageBox.Show("Please select a customer to delete.");
                return;
            }

            
            string query = "DELETE FROM customer WHERE customerId = @Id";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", selectedCustomerId);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer deleted successfully!");
                    ClearTextFields();
                    LoadCustomerData();
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
