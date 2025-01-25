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
    public partial class AssignDriverLoader : Form
    {
        public AssignDriverLoader()
        {
            InitializeComponent();
        }

        private string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }
        private void LoadComboBoxesAndListBox()
        {
            // Load events (use EventID or any other relevant column)
            string query = "SELECT EventID FROM Events"; // Assuming EventID is the only column
            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbEvent.Items.Add(new { EventID = reader["EventID"] });
                    }
                    cmbEvent.DisplayMember = "EventID";  // Display EventID
                    cmbEvent.ValueMember = "EventID";  // Use EventID as the value
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading events: " + ex.Message);
                }
            }

            // Load drivers
            query = "SELECT DriverID, Name FROM Driver";
            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbDriver.Items.Add(new { DriverID = reader["DriverID"], DriverName = reader["Name"] });
                    }
                    cmbDriver.DisplayMember = "DriverName";
                    cmbDriver.ValueMember = "DriverID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading drivers: " + ex.Message);
                }
            }

            query = "SELECT LoaderID, Name FROM WarehouseLoader";
            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lstLoaders.Items.Add(new { LoaderID = reader["LoaderID"], LoaderName = reader["Name"] });
                    }
                    lstLoaders.DisplayMember = "LoaderName";
                    lstLoaders.ValueMember = "LoaderID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading warehouse loaders: " + ex.Message);
                }
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Check if all fields are filled
            if (cmbEvent.SelectedIndex == -1 || cmbDriver.SelectedIndex == -1 || lstLoaders.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an event, driver, and at least one warehouse loader.");
                return;
            }

            int eventID = ((dynamic)cmbEvent.SelectedItem).EventID;
            int driverID = ((dynamic)cmbDriver.SelectedItem).DriverID;

            // Store the selected loader IDs
            var selectedLoaderIDs = new List<int>();

            foreach (var selectedItem in lstLoaders.SelectedItems)
            {
                selectedLoaderIDs.Add(((dynamic)selectedItem).LoaderID);
            }

            // Prepare the SQL query to insert the assignments
            string query = "INSERT INTO EventDriverLoaderAssignment (EventID, DriverID, LoaderID) VALUES (@EventID, @DriverID, @LoaderID)";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    foreach (int loaderID in selectedLoaderIDs)
                    {
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@EventID", eventID);
                        cmd.Parameters.AddWithValue("@DriverID", driverID);
                        cmd.Parameters.AddWithValue("@LoaderID", loaderID);

                        // Execute the query for each loader
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected <= 0)
                        {
                            MessageBox.Show("Failed to assign driver and warehouse loader.");
                            return;
                        }
                    }

                    MessageBox.Show("Driver and warehouse loaders assigned successfully!");
                    LoadAssignments();  // Reload the assignments grid
                    ClearFormFields();  // Clear form fields after assignment
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error assigning: " + ex.Message);
                }
            }
        }
        private void LoadAssignments()
        {
            // Adjusting query to display EventID instead of EventName
            string query = "SELECT e.EventID, d.Name AS DriverName, l.Name AS LoaderName " +
                           "FROM EventDriverLoaderAssignment a " +
                           "JOIN Events e ON a.EventID = e.EventID " +
                           "JOIN Driver d ON a.DriverID = d.DriverID " +
                           "JOIN WarehouseLoader l ON a.LoaderID = l.LoaderID";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading assignments: " + ex.Message);
                }
            }
        }

        private void ClearFormFields()
        {
            // Reset ComboBoxes
            cmbEvent.SelectedIndex = -1;
            cmbDriver.SelectedIndex = -1;

            // Uncheck all items in the CheckedListBox
            for (int i = 0; i < lstLoaders.Items.Count; i++)
            {
                lstLoaders.SetItemChecked(i, false);
            }
        }
        private void AssignDriverLoader_Load(object sender, EventArgs e)
        {
            LoadComboBoxesAndListBox();
            LoadAssignments();
        }
    }
}
