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
    public partial class FinanceManager : Form
    {
        public FinanceManager()
        {
            InitializeComponent();
        }
       
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true");

        private void FinanceManager_Load(object sender, EventArgs e)
        {
            LoadCustomers();
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        

        private void btnSupPayment_Click(object sender, EventArgs e)
        {
            FmSupplierPayment newForm = new FmSupplierPayment();
            newForm.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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

        private void btnEventBudget_Click(object sender, EventArgs e)
        {

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

           
            if (cmbCustomer.SelectedIndex == -1 || cmbEvent.SelectedIndex == -1)
            {
                MessageBox.Show("Please select both customer and event.");
                return;
            }

            
            Customer selectedCustomer = (Customer)cmbCustomer.SelectedItem;
            int selectedEventID = (int)cmbEvent.SelectedItem;

      
            decimal totalAmount = Convert.ToDecimal(txtTotalAmount.Text);
            decimal discountAmount = Convert.ToDecimal(txtDiscountAmount.Text);
            decimal finalAmount = Convert.ToDecimal(txtFinalAmount.Text);
            string voucherCode = txtVoucherCode.Text.Trim();
            DateTime invoiceDate = DateTime.Now;

          
            int? voucherID = null;

      
            if (!string.IsNullOrEmpty(voucherCode))
            {
                string voucherQuery = "SELECT VoucherID FROM Voucher WHERE VoucherCode = @VoucherCode";
                SqlCommand voucherCmd = new SqlCommand(voucherQuery, conn);
                voucherCmd.Parameters.AddWithValue("@VoucherCode", voucherCode);

                try
                {
                    conn.Open();
                    object result = voucherCmd.ExecuteScalar();
                    if (result != null)
                    {
                        voucherID = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Voucher code not found.");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error finding voucher: " + ex.Message);
                    return;
                }
                finally
                {
                    conn.Close();
                }
            }

            // Insert the data into the EventInvoice table
            string insertQuery = "INSERT INTO EventInvoice (CustomerID, CustomerName, EventID, TotalAmount, DiscountAmount, FinalAmount, VoucherID, VoucherCode, InvoiceDate) " +
                                 "VALUES (@CustomerID, @CustomerName, @EventID, @TotalAmount, @DiscountAmount, @FinalAmount, @VoucherID, @VoucherCode, @InvoiceDate)";

            SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
            insertCmd.Parameters.AddWithValue("@CustomerID", selectedCustomer.CustomerID);
            insertCmd.Parameters.AddWithValue("@CustomerName", selectedCustomer.CustomerName);
            insertCmd.Parameters.AddWithValue("@EventID", selectedEventID);
            insertCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
            insertCmd.Parameters.AddWithValue("@DiscountAmount", discountAmount);
            insertCmd.Parameters.AddWithValue("@FinalAmount", finalAmount);

           
            if (voucherID.HasValue)
            {
                insertCmd.Parameters.AddWithValue("@VoucherID", voucherID.Value);
            }
            else
            {
                insertCmd.Parameters.AddWithValue("@VoucherID", DBNull.Value);
            }

           
            insertCmd.Parameters.AddWithValue("@VoucherCode", voucherCode);
            insertCmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);

            try
            {
                conn.Open();
                insertCmd.ExecuteNonQuery();
                MessageBox.Show("Invoice saved successfully.");
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving invoice: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a customer first.");
                return;
            }

            Customer selectedCustomer = (Customer)cmbCustomer.SelectedItem;



            LoadEventsForCustomer(selectedCustomer.CustomerID);
        }

        private void cmbEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEvent.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an event first.");
                return;
            }

            int selectedEventID = (int)cmbEvent.SelectedItem;

            CalculateTotalAmount(selectedEventID);
        }
        private void CalculateTotalAmount(int eventID)
        {
            txtTotalAmount.Clear(); 

            string query = "SELECT SUM(TotalPrice) AS TotalAmount " +
                           "FROM EventProduct " +
                           "WHERE EventID = @EventID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EventID", eventID);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar(); 

                if (result != DBNull.Value)
                {
                    decimal totalAmount = Convert.ToDecimal(result);
                    txtTotalAmount.Text = totalAmount.ToString("F2"); 
                }
                else
                {
                    MessageBox.Show("No products found for this event.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total amount: " + ex.Message);
            }
            finally
            {
                conn.Close(); 
            }
        }




        private void txtVoucherCode_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void CalculateDiscount(string voucherCode)
        {
            string query = "SELECT DiscountValue, MinSpend, MaxSpend FROM Voucher WHERE VoucherCode = @VoucherCode";

            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@VoucherCode", voucherCode);

                conn.Open(); 
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    decimal discountPercentage = Convert.ToDecimal(reader["DiscountValue"]);
                    decimal minimumSpend = Convert.ToDecimal(reader["MinSpend"]);
                    decimal maximumSpend = Convert.ToDecimal(reader["MaxSpend"]);

                    decimal totalAmount = Convert.ToDecimal(txtTotalAmount.Text);

                    if (totalAmount >= minimumSpend)
                    {
                        
                        decimal discountAmount = (discountPercentage / 100) * totalAmount;

                        // Ensure the discount does not exceed the maximum spend limit
                        if (discountAmount > maximumSpend)
                        {
                            discountAmount = maximumSpend;
                        }

                        txtDiscountAmount.Text = discountAmount.ToString("F2");

                        
                        decimal finalAmount = totalAmount - discountAmount;
                        txtFinalAmount.Text = finalAmount.ToString("F2");
                    }
                    else
                    {
                        MessageBox.Show("Voucher cannot be applied. Minimum spend not met.");
                        txtDiscountAmount.Text = "0.00";
                        txtFinalAmount.Text = txtTotalAmount.Text;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Voucher Code.");
                    txtDiscountAmount.Text = "0.00";
                    txtFinalAmount.Text = txtTotalAmount.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying voucher: " + ex.Message);
            }
            finally
            {
                conn.Close(); 
            }
        }

        private void ClearFields()
        {
            
            cmbCustomer.SelectedIndex = -1;
            cmbEvent.Items.Clear();
            cmbEvent.SelectedIndex = -1;

            
            txtTotalAmount.Clear();
            txtDiscountAmount.Clear();
            txtFinalAmount.Clear();
            txtVoucherCode.Clear();

        
        }

        private void txtVoucherCode_Leave(object sender, EventArgs e)
        {
            string voucherCode = txtVoucherCode.Text.Trim();

            if (string.IsNullOrEmpty(voucherCode))
            {
                txtDiscountAmount.Text = "0.00";
                txtFinalAmount.Text = txtTotalAmount.Text;
                return;
            }

            CalculateDiscount(voucherCode);
        }
    }
}
