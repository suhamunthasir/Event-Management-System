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
    public partial class Supplier : Form
    {
        public Supplier()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";

        private void btnBack_Click(object sender, EventArgs e)
        {
            EventManager newForm = new EventManager();
            newForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            string name = txtName.Text;       
            string nic = txtNIC.Text;         
            string address = txtAddress.Text;  
            string contact = txtContact.Text; 

          
            

           
            string query = "INSERT INTO Supplier (name, nic, address, contact) VALUES (@Name, @NIC, @Address, @Contact)";

            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@NIC", nic);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Contact", contact);

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Supplier added successfully!");
                            LoadSupplierData();
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add supplier.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            string query = "UPDATE Supplier SET name = @Name, nic = @NIC, address = @Address, contact = @Contact WHERE nic = @NIC";
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@NIC", txtNIC.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Contact", txtContact.Text);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Supplier updated successfully.");
                        LoadSupplierData();
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("No supplier found with the given NIC.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating supplier: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNIC.Text))
            {
                MessageBox.Show("NIC is required to delete a supplier.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNIC.Focus();
                return;
            }

            string query = "DELETE FROM Supplier WHERE nic = @NIC";
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NIC", txtNIC.Text);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Supplier deleted successfully.");
                        LoadSupplierData();
                        ClearFields();
                    }
                    else
                    {
                        MessageBox.Show("No supplier found with the given NIC.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting supplier: " + ex.Message);
                }
            }
        }
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtNIC.Text))
            {
                MessageBox.Show("NIC is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNIC.Focus();
                return false;
            }
            if (txtNIC.Text.Length < 10) // Adjust based on NIC format
            {
                MessageBox.Show("NIC must be at least 10 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNIC.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAddress.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtContact.Text))
            {
                MessageBox.Show("Contact number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContact.Focus();
                return false;
            }
            if (!long.TryParse(txtContact.Text, out _))
            {
                MessageBox.Show("Contact number must be numeric.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContact.Focus();
                return false;
            }
            if (txtContact.Text.Length < 10) // Adjust for your contact number format
            {
                MessageBox.Show("Contact number must be at least 10 digits long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContact.Focus();
                return false;
            }
            return true;
        }

        private void LoadSupplierData()
        {
            string query = "SELECT id, name, nic, address, contact FROM Supplier";
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                try
                {
                    conn.Open();
                    adapter.Fill(dataTable);
                    dataGridViewSuppliers.DataSource = dataTable;

                    dataGridViewSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewSuppliers.AutoResizeColumns();
                    dataGridViewSuppliers.AllowUserToAddRows = false;
                    dataGridViewSuppliers.ReadOnly = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading supplier data: " + ex.Message);
                }
            }
        }

        private void Supplier_Load(object sender, EventArgs e)
        {
            LoadSupplierData();
        }

       

        

        private void dataGridViewSuppliers_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is selected
            {
                DataGridViewRow row = dataGridViewSuppliers.Rows[e.RowIndex];

                txtName.Text = row.Cells["name"].Value.ToString();
                txtNIC.Text = row.Cells["nic"].Value.ToString();
                txtAddress.Text = row.Cells["address"].Value.ToString();
                txtContact.Text = row.Cells["contact"].Value.ToString();
            }
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtNIC.Clear();
            txtContact.Clear();
           
        }
    }
}
