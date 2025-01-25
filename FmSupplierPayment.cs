using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class FmSupplierPayment : Form
    {
        public FmSupplierPayment()
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

        private void btnEventInvoice_Click(object sender, EventArgs e)
        {
           FinanceManager newForm = new FinanceManager();
            newForm.Show();
            this.Hide();
        }

        private void btnEventPayment_Click(object sender, EventArgs e)
        {
            FmEventPayment newForm = new FmEventPayment();
            newForm.Show();
            this.Hide();
        }

        private void btnEmployeePayment_Click(object sender, EventArgs e)
        {
            EmployeePayment newForm = new EmployeePayment();
            newForm.Show();
            this.Hide();
        }

        private void FmSupplierPayment_Load(object sender, EventArgs e)
        {
            LoadSupplierData();
        }
        public void LoadSupplierData()
        {


            string query = "SELECT name FROM Supplier";


            using (SqlConnection conn = new SqlConnection(conString))
            {

                SqlCommand cmd = new SqlCommand(query, conn);


                SqlDataAdapter da = new SqlDataAdapter(cmd);


                DataTable dt = new DataTable();

                try
                {

                    conn.Open();


                    da.Fill(dt);


                    cmbBoxSup.Items.Clear();


                    foreach (DataRow row in dt.Rows)
                    {
                        cmbBoxSup.Items.Add(row["name"].ToString());
                    }


                    if (cmbBoxSup.Items.Count > 0)
                    {
                        cmbBoxSup.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void cmbBoxSup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSupplier = cmbBoxSup.SelectedItem.ToString();

            // Fetch the supplier's salary
            GetSupplierSalary(selectedSupplier);
        }

        private void GetSupplierSalary(string supplierName)
        {
            string query = "SELECT Salary FROM Supplier WHERE name = @SupplierName";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SupplierName", supplierName);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        // Set the supplier's salary in the txtPaymentAmount textbox
                        txtPaymentAmount.Text = result.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Salary not found for the selected supplier.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving supplier salary: " + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Get the selected supplier name
            string supplierName = cmbBoxSup.SelectedItem.ToString();
            decimal paymentAmount = decimal.Parse(txtPaymentAmount.Text); // Ensure it's numeric
            string paymentMethod = cmbPaymentMethod.SelectedItem.ToString();
            DateTime paymentDate = dtpPaymentDate.Value; // DateTimePicker value
            string paymentStatus = cmbPaymentStatus.SelectedItem.ToString();

            // SQL query to insert data into SupplierPayment table
            string query = "INSERT INTO SupplierPayment (SupplierName, PaymentAmount, PaymentMethod, PaymentDate, PaymentStatus) " +
                           "VALUES (@SupplierName, @PaymentAmount, @PaymentMethod, @PaymentDate, @PaymentStatus)";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SupplierName", supplierName);
                cmd.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                cmd.Parameters.AddWithValue("@PaymentDate", paymentDate);
                cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); // Execute the insert query
                    MessageBox.Show("Payment details have been successfully inserted.");
                    ClearFormFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting payment details: " + ex.Message);
                }
            }
         
        }
        private void ClearFormFields()
        {
            
           
            cmbPaymentMethod.SelectedIndex = -1;
            cmbPaymentStatus.SelectedIndex = -1;

           
            txtPaymentAmount.Clear();

           
            dtpPaymentDate.Value = DateTime.Today;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Check if all required fields are filled
            if (cmbBoxSup.SelectedItem == null || string.IsNullOrEmpty(txtPaymentAmount.Text) || cmbPaymentMethod.SelectedItem == null || dtpPaymentDate.Value == null || cmbPaymentStatus.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all the fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected supplier details
            string supplierName = cmbBoxSup.SelectedItem.ToString();
            decimal paymentAmount = decimal.Parse(txtPaymentAmount.Text); // Ensure payment amount is numeric
            string paymentMethod = cmbPaymentMethod.SelectedItem.ToString();
            DateTime paymentDate = dtpPaymentDate.Value; // Assuming you have a DateTimePicker for the date
            string paymentStatus = cmbPaymentStatus.SelectedItem.ToString();

            // Prepare SQL query to update the supplier payment data
            string query = "UPDATE SupplierPayment " +
                           "SET PaymentAmount = @PaymentAmount, " +
                           "PaymentMethod = @PaymentMethod, " +
                           "PaymentDate = @PaymentDate, " +
                           "PaymentStatus = @PaymentStatus " +
                           "WHERE SupplierName = @SupplierName";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                cmd.Parameters.AddWithValue("@PaymentDate", paymentDate);
                cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
                cmd.Parameters.AddWithValue("@SupplierName", supplierName);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery(); // Execute the update query

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Payment details have been successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFormFields(); // Optionally clear the form after updating
                    }
                    else
                    {
                        MessageBox.Show("Failed to update the payment. Please check if the payment details exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating payment details: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }

        }
    }
}
