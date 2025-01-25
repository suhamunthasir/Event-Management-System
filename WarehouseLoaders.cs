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
    public partial class WarehouseLoaders : Form
    {
        public WarehouseLoaders()
        {
            InitializeComponent();
        }
        private string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";

        private void LoadWarehouseLoaders()
        {
            string query = "SELECT LoaderID, NIC, Name, Address, Contact FROM WarehouseLoader";

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
                    dataGridView1.AllowUserToAddRows = false; // Disable row addition by the user
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading warehouse loaders: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNIC.Text) || string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtContact.Text))
            {
                MessageBox.Show("Please fill all fields before saving.");
                return;
            }

            string query = "INSERT INTO WarehouseLoader (NIC, Name, Address, Contact) VALUES (@NIC, @Name, @Address, @Contact)";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NIC", txtNIC.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim());

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Warehouse loader added successfully!");
                        LoadWarehouseLoaders();
                        ClearFormFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add warehouse loader.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding warehouse loader: " + ex.Message);
                }
            }
        }

        private void WarehouseLoaders_Load(object sender, EventArgs e)
        {
            LoadWarehouseLoaders();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a warehouse loader to update.");
                return;
            }

            if (string.IsNullOrEmpty(txtNIC.Text) || string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtContact.Text))
            {
                MessageBox.Show("Please fill all fields before updating.");
                return;
            }

            int loaderID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["LoaderID"].Value);

            string query = "UPDATE WarehouseLoader SET NIC = @NIC, Name = @Name, Address = @Address, Contact = @Contact WHERE LoaderID = @LoaderID";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@NIC", txtNIC.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim());
                    cmd.Parameters.AddWithValue("@LoaderID", loaderID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Warehouse loader updated successfully!");
                        LoadWarehouseLoaders();
                        ClearFormFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update warehouse loader.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating warehouse loader: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a warehouse loader to delete.");
                return;
            }

            int loaderID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["LoaderID"].Value);

            string query = "DELETE FROM WarehouseLoader WHERE LoaderID = @LoaderID";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@LoaderID", loaderID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Warehouse loader deleted successfully!");
                        LoadWarehouseLoaders();
                        ClearFormFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete warehouse loader.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting warehouse loader: " + ex.Message);
                }
            }
        }
        private void ClearFormFields()
        {
            txtNIC.Clear();
            txtName.Clear();
            txtAddress.Clear();
            txtContact.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Populate the textboxes with the selected row data
                txtNIC.Text = row.Cells["NIC"].Value.ToString();
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtContact.Text = row.Cells["Contact"].Value.ToString();
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
