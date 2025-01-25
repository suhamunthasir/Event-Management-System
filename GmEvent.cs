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
    public partial class GmEvent : Form
    {
        public GmEvent()
        {
            InitializeComponent();
        }

        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";

        private void dataGridViewSuppliers_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadEventData()
        {

            string query = "SELECT e.EventID, c.NIC, e.EventDate, e.EventVenue, e.EventAssistantName, e.LogisticManagerName, e.Category " +
                           "FROM Events e " +
                           "INNER JOIN Customer c ON e.CustomerID = c.CustomerID";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.AutoResizeColumns();
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.ReadOnly = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void GmEvent_Load(object sender, EventArgs e)
        {
            LoadEventData();
        }

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

        private void button3_Click(object sender, EventArgs e)
        {
            print newForm = new print();
            newForm.Show();
            
        }
    }
}
