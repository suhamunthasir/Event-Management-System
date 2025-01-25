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
    public partial class GmCustomer : Form
    {
        public GmCustomer()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";

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

        private void LoadCustomerData()
        {
            string query = "SELECT * FROM customer";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);

                    dataGridView.DataSource = dataTable; // Bind data to DataGridView

                    // Adjust DataGridView settings for a clean look
                    dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView.AutoResizeColumns();
                    dataGridView.AllowUserToAddRows = false; // Disable adding rows manually in UI
                    dataGridView.ReadOnly = true; // Make it read-only
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customer data: " + ex.Message);
                }
            }
        }

        private void GmCustomer_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
