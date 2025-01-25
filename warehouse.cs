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
    public partial class warehouse : Form
    {
        public warehouse()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Check if all fields are filled
            if (string.IsNullOrEmpty(txtWarehouseName.Text) ||
                string.IsNullOrEmpty(txtLocation.Text) ||
                string.IsNullOrEmpty(txtContactNumber.Text))
            {
                MessageBox.Show("Please fill all fields before saving.");
                return;
            }

            // Prepare the SQL query to insert warehouse details
            string query = "INSERT INTO Warehouse (WarehouseName, Location, ContactNumber) VALUES (@WarehouseName, @Location, @ContactNumber)";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters to avoid SQL injection
                        cmd.Parameters.AddWithValue("@WarehouseName", txtWarehouseName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Location", txtLocation.Text.Trim());
                        cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text.Trim());

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Warehouse details added successfully!");
                            ClearFormFields();  // Clear the form after successful insertion
                            LoadWarehouseData();  // Reload the data into the grid
                        }
                        else
                        {
                            MessageBox.Show("Failed to add warehouse details.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding warehouse: " + ex.Message);
                }
            }
        }
        private void ClearFormFields()
        {
            txtWarehouseName.Clear();
            txtLocation.Clear();
            txtContactNumber.Clear();
        }
        private void LoadWarehouseData()
        {
            string query = "SELECT WarehouseID, WarehouseName, Location, ContactNumber FROM Warehouse";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Set the data source for the DataGridView
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoResizeColumns();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading warehouse data: " + ex.Message);
                }
            }
        }

        private void warehouse_Load(object sender, EventArgs e)
        {
            LoadWarehouseData();
        }

        private void btnGoback_Click(object sender, EventArgs e)
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
    }
}
