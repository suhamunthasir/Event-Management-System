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
    public partial class VehicleDetails : Form
    {
        public VehicleDetails()
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

        private void btnDriverDetails_Click(object sender, EventArgs e)
        {
           LogisticManages newForm = new LogisticManages();
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
    if (string.IsNullOrEmpty(txtPlateNumber.Text) ||
        string.IsNullOrEmpty(txtModel.Text) ||
        string.IsNullOrEmpty(txtVehicleType.Text) || // Vehicle Type is now a TextBox
        cmbDriverName.SelectedIndex == -1)
    {
        MessageBox.Show("Please fill all fields before saving.");
        return;
    }

    // Prepare the SQL query to insert vehicle details
    string query = "INSERT INTO Vehicle (VehicleType, PlateNumber, Model, DriverID, DriverName) VALUES (@VehicleType, @PlateNumber, @Model, @DriverID, @DriverName)";

    using (SqlConnection conn = new SqlConnection(conString))
    {
        try
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // Add parameters to avoid SQL injection
                cmd.Parameters.AddWithValue("@VehicleType", txtVehicleType.Text.Trim());
                cmd.Parameters.AddWithValue("@PlateNumber", txtPlateNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@Model", txtModel.Text.Trim());

                // Get DriverID and DriverName from ComboBox
                int driverID = ((dynamic)cmbDriverName.SelectedItem).DriverID;
                string driverName = ((dynamic)cmbDriverName.SelectedItem).DriverName;
                
                cmd.Parameters.AddWithValue("@DriverID", driverID);  // Insert DriverID
                cmd.Parameters.AddWithValue("@DriverName", driverName);  // Insert DriverName

                // Execute the query
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Vehicle details added successfully!");
                    LoadVehicles();
                    ClearFormFields();
                }
                else
                {
                    MessageBox.Show("Failed to add vehicle details.");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error adding vehicle: " + ex.Message);
        }
    }
        }
        private void ClearFormFields()
        {
            txtPlateNumber.Clear();
            txtModel.Clear();
            txtVehicleType.Clear(); 
            cmbDriverName.SelectedIndex = -1;
        }
        private void PopulateDriverComboBox()
        {
            string query = "SELECT DriverID, Name FROM Driver";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                   
                    while (reader.Read())
                    {
                        cmbDriverName.Items.Add(new { DriverID = reader["DriverID"], DriverName = reader["Name"] });
                    }

                    // Set DisplayMember and ValueMember
                    cmbDriverName.DisplayMember = "DriverName";
                    cmbDriverName.ValueMember = "DriverID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading drivers: " + ex.Message);
                }
            }
        }

        private void VehicleDetails_Load(object sender, EventArgs e)
        {
            PopulateDriverComboBox();
            LoadVehicles();
        }

        private void LoadVehicles()
        {
            string query = @"
        SELECT v.VehicleID, v.VehicleType, v.PlateNumber, v.Model, d.Name AS DriverName
        FROM Vehicle v
        INNER JOIN Driver d ON v.DriverID = d.DriverID";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = dt;

                    // Auto-resize columns
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoResizeColumns();
                    dataGridView1.AllowUserToAddRows = false; // Disable adding rows manually
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading vehicles: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate the form fields with the selected row's data
                txtPlateNumber.Text = row.Cells["PlateNumber"].Value.ToString();
                txtModel.Text = row.Cells["Model"].Value.ToString();
                txtVehicleType.Text = row.Cells["VehicleType"].Value.ToString();

                // Get the Driver Name from the selected row
                string driverName = row.Cells["DriverName"].Value.ToString();

                // Set the corresponding driver in the ComboBox
                foreach (var item in cmbDriverName.Items)
                {
                    if (((dynamic)item).DriverName == driverName)
                    {
                        cmbDriverName.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow row = dataGridView1.SelectedRows[0];

                // Get the VehicleID from the selected row
                int vehicleID = Convert.ToInt32(row.Cells["VehicleID"].Value);

                // Ask for confirmation before deletion
                var confirmation = MessageBox.Show("Are you sure you want to delete this vehicle?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmation == DialogResult.Yes)
                {
                    // Prepare the SQL query to delete the selected vehicle
                    string query = "DELETE FROM Vehicle WHERE VehicleID = @VehicleID";

                    using (SqlConnection conn = new SqlConnection(conString))
                    {
                        try
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                // Add the VehicleID parameter
                                cmd.Parameters.AddWithValue("@VehicleID", vehicleID);

                                // Execute the query
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Vehicle deleted successfully!");
                                    LoadVehicles(); // Reload the DataGridView
                                    ClearFormFields(); // Clear the form fields
                                }
                                else
                                {
                                    MessageBox.Show("Failed to delete vehicle.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error deleting vehicle: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a vehicle to delete.");
            }
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            AssignDriverLoader newForm = new AssignDriverLoader();
            newForm.Show();
            this.Hide();
        }
    }
}
