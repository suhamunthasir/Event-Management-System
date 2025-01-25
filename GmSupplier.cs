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
    public partial class GmSupplier : Form
    {
        public GmSupplier()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";

        private void button1_Click(object sender, EventArgs e)
        {
            GeneralManager newForm = new GeneralManager();
            newForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        private void LoadSupplierData()
        {
            string query = "SELECT * FROM Supplier";
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

                    // Set DataGridView properties
                    dataGridViewSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewSuppliers.AutoResizeColumns();
                    dataGridViewSuppliers.AllowUserToAddRows = false;
                    dataGridViewSuppliers.ReadOnly = true;

                    // Rename column headers
                    if (dataGridViewSuppliers.Columns.Contains("id"))
                        dataGridViewSuppliers.Columns["id"].HeaderText = "Supplier ID";

                    if (dataGridViewSuppliers.Columns.Contains("name"))
                        dataGridViewSuppliers.Columns["name"].HeaderText = "Supplier Name";

                    if (dataGridViewSuppliers.Columns.Contains("nic"))
                        dataGridViewSuppliers.Columns["nic"].HeaderText = "NIC";

                    if (dataGridViewSuppliers.Columns.Contains("address"))
                        dataGridViewSuppliers.Columns["address"].HeaderText = "Address";

                    if (dataGridViewSuppliers.Columns.Contains("contact"))
                        dataGridViewSuppliers.Columns["contact"].HeaderText = "Contact Number";

                    if (dataGridViewSuppliers.Columns.Contains("Salary"))
                        dataGridViewSuppliers.Columns["Salary"].HeaderText = "Payment";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading supplier data: " + ex.Message);
                }
            }
        }

        private void GmSupplier_Load(object sender, EventArgs e)
        {
            LoadSupplierData();
        }
    }
}
