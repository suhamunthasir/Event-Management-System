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
    public partial class FmEventPayment : Form
    {
        public FmEventPayment()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true");
        private void btnEventInvoice_Click(object sender, EventArgs e)
        { FinanceManager newForm = new FinanceManager();
            newForm.Show();
            this.Hide();
        }

        private void btnEventPayment_Click(object sender, EventArgs e)
        {

        }

        private void btnSupPayment_Click(object sender, EventArgs e)
        {
            FmSupplierPayment newForm = new FmSupplierPayment();
            newForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        private void btnEmployeePayment_Click(object sender, EventArgs e)
        {
            EmployeePayment newForm = new EmployeePayment();
            newForm.Show();
            this.Hide();
        }

     

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedItem == null || cmbEvent.SelectedItem == null || string.IsNullOrEmpty(txtFinalAmount.Text) || PaymentDate.Value == null || cmbBoxPaymentType.SelectedItem == null || cmbBoxStatus.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all the fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int customerId = ((Customer)cmbCustomer.SelectedItem).CustomerID;
            int eventId = Convert.ToInt32(cmbEvent.SelectedItem);
            decimal totalAmount = Convert.ToDecimal(txtFinalAmount.Text);
            DateTime paymentDate = PaymentDate.Value;
            string paymentType = cmbBoxPaymentType.SelectedItem.ToString();
            string paymentStatus = cmbBoxStatus.SelectedItem.ToString();

            string query = @"
                INSERT INTO EventPayment (CustomerID, EventID, TotalAmount, PaymentDate, PaymentType, PaymentStatus)
                VALUES (@CustomerID, @EventID, @TotalAmount, @PaymentDate, @PaymentType, @PaymentStatus)";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.AddWithValue("@EventID", eventId);
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                cmd.Parameters.AddWithValue("@PaymentDate", paymentDate);
                cmd.Parameters.AddWithValue("@PaymentType", paymentType);
                cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Payment saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearPaymentForm();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save the payment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void LoadCustomers()
        {
            cmbCustomer.Items.Clear();

            string query = "SELECT CustomerID, Full_Name FROM Customer";


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
                    cmbCustomer.Items.Add(new Customer { CustomerID = customerID, CustomerName = customerName });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
            finally
            {
                conn.Close();
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

        private void LoadEventsForCustomer(int customerID)
        {
            cmbEvent.Items.Clear();

            string query = "SELECT EventID FROM Events WHERE CustomerID = @CustomerID";


            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            try
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        cmbEvent.Items.Add(reader["EventID"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading events: " + ex.Message);
            }

        }

        private void FmEventPayment_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedIndex == -1)
            {
               
                return;
            }

            Customer selectedCustomer = (Customer)cmbCustomer.SelectedItem;



            LoadEventsForCustomer(selectedCustomer.CustomerID);
        }

        private void FillFinalAmount()
        {
            if (cmbCustomer.SelectedItem == null || cmbEvent.SelectedItem == null)
            {
                txtFinalAmount.Text = string.Empty;
                return;
            }

            // Use SelectedItem for unbound ComboBoxes
            Customer selectedCustomer = (Customer)cmbCustomer.SelectedItem;
            int customerId = selectedCustomer.CustomerID;
            int eventId = Convert.ToInt32(cmbEvent.SelectedItem);

            string query = "SELECT FinalAmount FROM EventInvoice WHERE EventID = @EventID AND CustomerID = @CustomerID";

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@EventID", eventId);
                command.Parameters.AddWithValue("@CustomerID", customerId);

                try
                {
                    conn.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        txtFinalAmount.Text = Convert.ToDecimal(result).ToString("0.00");
                    }
                    else
                    {
                        MessageBox.Show("No final amount found for the selected customer and event.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtFinalAmount.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void ClearPaymentForm()
        {
            cmbCustomer.SelectedIndex = -1;
            cmbEvent.SelectedIndex = -1;
            cmbEvent.Items.Clear();
            txtFinalAmount.Clear();
            PaymentDate.Value = DateTime.Now;
            cmbBoxPaymentType.SelectedIndex = -1;
            cmbBoxStatus.SelectedIndex = -1;
        }


        private void cmbEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            FillFinalAmount();
            
        }

        

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedItem == null || cmbEvent.SelectedItem == null || string.IsNullOrEmpty(txtFinalAmount.Text) || PaymentDate.Value == null || cmbBoxPaymentType.SelectedItem == null || cmbBoxStatus.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all the fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get customer and event details
            Customer selectedCustomer = (Customer)cmbCustomer.SelectedItem;
            int customerId = selectedCustomer.CustomerID;
            int eventId = Convert.ToInt32(cmbEvent.SelectedItem);
            decimal totalAmount = Convert.ToDecimal(txtFinalAmount.Text);
            DateTime paymentDate = PaymentDate.Value;
            string paymentType = cmbBoxPaymentType.SelectedItem.ToString();
            string paymentStatus = cmbBoxStatus.SelectedItem.ToString();

            // Prepare SQL query to update the payment
            string query = @"
        UPDATE EventPayment 
        SET TotalAmount = @TotalAmount, 
            PaymentDate = @PaymentDate, 
            PaymentType = @PaymentType, 
            PaymentStatus = @PaymentStatus
        WHERE CustomerID = @CustomerID AND EventID = @EventID";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CustomerID", customerId);
                cmd.Parameters.AddWithValue("@EventID", eventId);
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                cmd.Parameters.AddWithValue("@PaymentDate", paymentDate);
                cmd.Parameters.AddWithValue("@PaymentType", paymentType);
                cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Payment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearPaymentForm(); // Optionally, clear the form after updating
                    }
                    else
                    {
                        MessageBox.Show("Failed to update the payment. Please check if the payment exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }


            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {

        }
    }
}
