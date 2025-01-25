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
    public partial class LogisticManages : Form
    {
        public LogisticManages()
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

        private void btnVehicle_Click(object sender, EventArgs e)
        {
            VehicleDetails newForm = new VehicleDetails();
            newForm.Show();
            this.Hide();
        }

        private void btnLoaders_Click(object sender, EventArgs e)
        {
            WarehouseLoaders newForm = new WarehouseLoaders();
            newForm.Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Check if all fields are filled
            if (string.IsNullOrEmpty(txtDriverNIC.Text) ||
                string.IsNullOrEmpty(txtDriverName.Text) ||
                string.IsNullOrEmpty(txtDriverAddress.Text) ||
                string.IsNullOrEmpty(txtDriverContact.Text))
            {
                MessageBox.Show("Please fill all fields before saving.");
                return;
            }

            // Prepare the SQL query
            string query = "INSERT INTO Driver (NIC, Name, Address, ContactNumber) VALUES (@NIC, @Name, @Address, @ContactNumber)";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@NIC", txtDriverNIC.Text.Trim());
                        cmd.Parameters.AddWithValue("@Name", txtDriverName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtDriverAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@ContactNumber", txtDriverContact.Text.Trim());

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Driver added successfully!");
                            LoadDrivers();
                            ClearFormFields();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add driver.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding driver: " + ex.Message);
                }
            }
        }
        private void ClearFormFields()
        {
            txtDriverNIC.Clear();
            txtDriverName.Clear();
            txtDriverAddress.Clear();
            txtDriverContact.Clear();
        }
        private void LoadDrivers()
        {
            string query = "SELECT DriverID, NIC, Name, Address, ContactNumber FROM Driver";

            using (SqlConnection conn = new SqlConnection(conString))
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
                    dataGridView1.AllowUserToAddRows = false; // Disables adding rows manually in the grid
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading drivers: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is selected
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Fill the form controls with data from the selected row
                txtDriverNIC.Text = row.Cells["NIC"].Value.ToString();
                txtDriverName.Text = row.Cells["Name"].Value.ToString();
                txtDriverAddress.Text = row.Cells["Address"].Value.ToString();
                txtDriverContact.Text = row.Cells["ContactNumber"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {// Ensure the fields are filled
            if (string.IsNullOrEmpty(txtDriverNIC.Text) ||
                string.IsNullOrEmpty(txtDriverName.Text) ||
                string.IsNullOrEmpty(txtDriverAddress.Text) ||
                string.IsNullOrEmpty(txtDriverContact.Text))
            {
                MessageBox.Show("Please fill all fields before updating.");
                return;
            }

            // Ensure a row is selected
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index == -1)
            {
                MessageBox.Show("Please select a driver to update.");
                return;
            }

            // Get the DriverID from the selected row
            int selectedDriverID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["DriverID"].Value);

            if (selectedDriverID == 0)
            {
                MessageBox.Show("Please select a valid driver.");
                return;
            }

            // Prepare the SQL query for the update
            string query = "UPDATE Driver SET NIC = @NIC, Name = @Name, Address = @Address, ContactNumber = @ContactNumber WHERE DriverID = @DriverID";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NIC", txtDriverNIC.Text.Trim());
                        cmd.Parameters.AddWithValue("@Name", txtDriverName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address", txtDriverAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@ContactNumber", txtDriverContact.Text.Trim());
                        cmd.Parameters.AddWithValue("@DriverID", selectedDriverID);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Driver details updated successfully!");
                            LoadDrivers();  // Reload the updated list of drivers
                            ClearFormFields();  // Clear the form fields
                        }
                        else
                        {
                            MessageBox.Show("Failed to update driver details.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating driver: " + ex.Message);
                }
            }
        }
       
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a driver to delete.");
                return;
            }

       
            if (MessageBox.Show("Are you sure you want to delete this driver?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int selectedDriverID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["DriverID"].Value);

            
                string query = "DELETE FROM Driver WHERE DriverID = @DriverID";

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    try
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            
                            cmd.Parameters.AddWithValue("@DriverID", selectedDriverID);

                           
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Driver deleted successfully!");
                                LoadDrivers();  
                                ClearFormFields();  
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete driver.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting driver: " + ex.Message);
                    }
                }
            }

        }

        private void LogisticManages_Load(object sender, EventArgs e)
        {
            LoadDrivers();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            AssignDriverLoader newForm = new AssignDriverLoader();
            newForm.Show();
            this.Hide();
        }
    }
}
