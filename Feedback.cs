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
    public partial class Feedback : Form
    {
        public Feedback()
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
        private void btnGoback_Click(object sender, EventArgs e)
        {
           EventPlanner newForm = new EventPlanner();
            newForm.Show();
            this.Hide();
        }

        private void LoadCustomerNames()
        {
            comboBoxCustomerID.Items.Clear(); // Clear previous items

            string query = "SELECT CustomerID, Full_Name FROM Customer";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int customerID = Convert.ToInt32(reader["CustomerID"]);
                        string customerName = reader["Full_Name"].ToString();

                        // Create a new Customer object and add it to the ComboBox
                        comboBoxCustomerID.Items.Add(new Customer { CustomerID = customerID, CustomerName = customerName });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customers: " + ex.Message);
                }
            }
        }
        public class Customer
        {
            public int CustomerID { get; set; }
            public string CustomerName { get; set; }

            public override string ToString()
            {
                return CustomerName;
            }
        }

        private void LoadEventIDs(int customerID)
        {
            comboBoxEventID.Items.Clear(); // Clear previous items

            string query = "SELECT EventID FROM Events WHERE CustomerID = @CustomerID";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)) // Automatically closes the connection
                    {
                        while (reader.Read())
                        {
                            comboBoxEventID.Items.Add(reader["EventID"]);
                        }
                    } // DataReader is automatically closed here
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading events: " + ex.Message);
                }
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxCustomerID.SelectedIndex == -1 || comboBoxEventID.SelectedIndex == -1 || string.IsNullOrEmpty(txtFeedback.Text))
            {
                MessageBox.Show("Please select a customer, an event, and provide feedback.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected customer and event IDs
            Customer selectedCustomer = (Customer)comboBoxCustomerID.SelectedItem;
            int customerID = selectedCustomer.CustomerID;
            int eventID = Convert.ToInt32(comboBoxEventID.SelectedItem);

            string query = "INSERT INTO FeedBack (CustomerID, EventID, FeedbackDate, Feedback) VALUES (@CustomerID, @EventID, @FeedbackDate, @Feedback)";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@CustomerID", customerID); // CustomerID from comboBox
                        cmd.Parameters.AddWithValue("@EventID", eventID);     // EventID from comboBox
                        cmd.Parameters.AddWithValue("@FeedbackDate", feedbackDate.Value); // Date selected in DateTimePicker
                        cmd.Parameters.AddWithValue("@Feedback", txtFeedback.Text.Trim()); // Feedback text from textbox

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Feedback added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtFeedback.Clear(); // Clear the feedback textbox after successful insertion
                        }
                        else
                        {
                            MessageBox.Show("Feedback insertion failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding feedback: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmbBoxCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomerID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a customer first.");
                return;
            }

            Customer selectedCustomer = (Customer)comboBoxCustomerID.SelectedItem;



            LoadEventIDs(selectedCustomer.CustomerID);
        }

        private void Feedback_Load(object sender, EventArgs e)
        {
           
            ClearComboBoxes();
            LoadCustomerNames();

        }
        private void ClearComboBoxes()
        {
            comboBoxCustomerID.DataSource = null;
            comboBoxCustomerID.Items.Clear();

            comboBoxEventID.DataSource = null;
            comboBoxEventID.Items.Clear();
        }
    }
}
