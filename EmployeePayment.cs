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
    public partial class EmployeePayment : Form
    {
        public EmployeePayment()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true");
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

        private void btnSupPayment_Click(object sender, EventArgs e)
        {
            FmSupplierPayment newForm = new FmSupplierPayment();
            newForm.Show();
            this.Hide();
        }
        private void LoadCategories()
        {
            // Add categories manually
            cmbEmployeeCategory.Items.Add("EventManager");
            cmbEmployeeCategory.Items.Add("EventAssistant");
            cmbEmployeeCategory.Items.Add("LogisticManager");
            cmbEmployeeCategory.Items.Add("EventPlanner");
            cmbEmployeeCategory.Items.Add("GeneralManager");
            cmbEmployeeCategory.Items.Add("FinanceManager");
        }

        private void cmbEmployeeCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string category = cmbEmployeeCategory.SelectedItem.ToString();

            cmbEmployee.Items.Clear();

            if (category == "EventPlanner")
            {
                LoadEmployeeIds("EventPlanner");
            }
            else if (category == "EventManager")
            {
                LoadEmployeeIds("EventManager");
            }
            else if (category == "EventAssistant")
            {
                LoadEmployeeIds("EventAssistant");
            }
            else if (category == "LogisticManager")
            {
                LoadEmployeeIds("LogisticManager");
            }
            else if (category == "FinanceManager")
            {
                LoadEmployeeIds("FinanceManager");
            }
            else if (category == "GeneralManager")
            {
                LoadEmployeeIds("GeneralManager");
            }
        }

        private void LoadEmployeeIds(string category)
        {
            string query = $"SELECT ID, Name FROM {category}";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int employeeId = reader.GetInt32(0);
                    string employeeName = reader.GetString(1);

                    cmbEmployee.Items.Add(new KeyValuePair<int, string>(employeeId, employeeName));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employee IDs: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void EmployeePayment_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadMonths();
        }

        private void cmbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected employee (ID, Name)
            KeyValuePair<int, string> selectedEmployee = (KeyValuePair<int, string>)cmbEmployee.SelectedItem;
            int employeeId = selectedEmployee.Key;

            // Fetch the salary for the selected employee
            string category = cmbEmployeeCategory.SelectedItem.ToString();
            GetEmployeeSalary(category, employeeId);
        }
        private void GetEmployeeSalary(string category, int employeeId)
        {
            string query = $"SELECT Salary FROM {category} WHERE ID = @EmployeeID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EmployeeID", employeeId);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar(); // Retrieve the salary value

                if (result != null)
                {
                    // Set the salary in the txtSalary textbox
                    txtSalary.Text = result.ToString();
                }
                else
                {
                    MessageBox.Show("Salary not found for the selected employee.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving salary: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            // Get the selected employee details
            KeyValuePair<int, string> selectedEmployee = (KeyValuePair<int, string>)cmbEmployee.SelectedItem;
            int employeeId = selectedEmployee.Key;
            string employeeName = selectedEmployee.Value;
            string category = cmbEmployeeCategory.SelectedItem.ToString();
            decimal salary = decimal.Parse(txtSalary.Text); // Ensure salary is numeric
            string paymentMethod = cmbPaymentMethod.SelectedItem.ToString();
            string paymentMonth = cmbPaymentMonth.SelectedItem.ToString();
            DateTime paymentDate = dtpPaymentDate.Value; // Assuming you have a DateTimePicker for the date
            string paymentStatus = cmbPaymentStatus.SelectedItem.ToString();

            // Prepare SQL query to insert the payment data into the table
            string query = "INSERT INTO EmployeePayments (EmployeeCategory, EmployeeId, EmployeeName, Salary, PaymentMethod, PaymentMonth, PaymentDate, PaymentStatus) " +
                           "VALUES (@EmployeeCategory, @EmployeeId, @EmployeeName, @Salary, @PaymentMethod, @PaymentMonth, @PaymentDate, @PaymentStatus)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EmployeeCategory", category);
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            cmd.Parameters.AddWithValue("@EmployeeName", employeeName);
            cmd.Parameters.AddWithValue("@Salary", salary);
            cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
            cmd.Parameters.AddWithValue("@PaymentMonth", paymentMonth);
            cmd.Parameters.AddWithValue("@PaymentDate", paymentDate);
            cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Payment details have been successfully inserted.");
                ClearFormFields();

                cmbEmployeeCategory.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting payment details: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void LoadMonths()
        {
            // Add months to the combo box
            cmbPaymentMonth.Items.Clear();
            cmbPaymentMonth.Items.Add("January");
            cmbPaymentMonth.Items.Add("February");
            cmbPaymentMonth.Items.Add("March");
            cmbPaymentMonth.Items.Add("April");
            cmbPaymentMonth.Items.Add("May");
            cmbPaymentMonth.Items.Add("June");
            cmbPaymentMonth.Items.Add("July");
            cmbPaymentMonth.Items.Add("August");
            cmbPaymentMonth.Items.Add("September");
            cmbPaymentMonth.Items.Add("October");
            cmbPaymentMonth.Items.Add("November");
            cmbPaymentMonth.Items.Add("December");
        }

        private void ClearFormFields()
        {
            // Clear the combo boxes
           
           
            cmbPaymentMethod.SelectedIndex = -1;
            cmbPaymentMonth.SelectedIndex = -1;
            cmbPaymentStatus.SelectedIndex = -1;

            // Clear the text boxes
            txtSalary.Clear();

            // Reset the DateTimePicker to the default date (or today's date)
            dtpPaymentDate.Value = DateTime.Today;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Check if all required fields are filled
            if (cmbEmployee.SelectedItem == null || string.IsNullOrEmpty(txtSalary.Text) || cmbPaymentMethod.SelectedItem == null || cmbPaymentMonth.SelectedItem == null || cmbPaymentStatus.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all the fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected employee details
            KeyValuePair<int, string> selectedEmployee = (KeyValuePair<int, string>)cmbEmployee.SelectedItem;
            int employeeId = selectedEmployee.Key;
            string employeeName = selectedEmployee.Value;
            string category = cmbEmployeeCategory.SelectedItem.ToString();
            decimal salary = decimal.Parse(txtSalary.Text); // Ensure salary is numeric
            string paymentMethod = cmbPaymentMethod.SelectedItem.ToString();
            string paymentMonth = cmbPaymentMonth.SelectedItem.ToString();
            DateTime paymentDate = dtpPaymentDate.Value; // Assuming you have a DateTimePicker for the date
            string paymentStatus = cmbPaymentStatus.SelectedItem.ToString();

            // Prepare SQL query to update the payment data
            string query = "UPDATE EmployeePayments " +
                           "SET EmployeeCategory = @EmployeeCategory, " +
                           "Salary = @Salary, " +
                           "PaymentMethod = @PaymentMethod, " +
                           "PaymentMonth = @PaymentMonth, " +
                           "PaymentDate = @PaymentDate, " +
                           "PaymentStatus = @PaymentStatus " +
                           "WHERE EmployeeId = @EmployeeId AND EmployeeName = @EmployeeName";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EmployeeCategory", category);
            cmd.Parameters.AddWithValue("@Salary", salary);
            cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
            cmd.Parameters.AddWithValue("@PaymentMonth", paymentMonth);
            cmd.Parameters.AddWithValue("@PaymentDate", paymentDate);
            cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
            cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
            cmd.Parameters.AddWithValue("@EmployeeName", employeeName);

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
