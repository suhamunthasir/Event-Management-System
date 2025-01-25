using System;
using System.Collections;
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
    public partial class Voucher : Form
    {
        public Voucher()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login newform = new Login();
            newform.Show();
            this.Hide();
        }

        private void btnGoback_Click(object sender, EventArgs e)
        {
            EventManager newForm = new EventManager();
            newForm.Show();
            this.Hide();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
          
            string voucherCode = txtVoucherCode.Text;
            decimal discountValue;
            decimal minSpend;
            decimal maxSpend;
            DateTime expiryDate;
            int usageLimit;

            
            if (string.IsNullOrEmpty(voucherCode) ||
                !decimal.TryParse(txtDiscountValue.Text, out discountValue) ||
                !decimal.TryParse(txtMinSpend.Text, out minSpend) ||
                !decimal.TryParse(txtMaxSpend.Text, out maxSpend) ||
                !DateTime.TryParse(dtpExpiryDate.Text, out expiryDate) ||
                usageLimitNumericUpDown.Value <= 0)
            {
                MessageBox.Show("Please provide valid inputs for all fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            usageLimit = (int)usageLimitNumericUpDown.Value;

           

          
            string query = @"
                INSERT INTO Voucher (VoucherCode, DiscountValue, MinSpend, MaxSpend, ExpiryDate, UsageLimit)
                VALUES (@VoucherCode, @DiscountValue, @MinSpend, @MaxSpend, @ExpiryDate, @UsageLimit);";

            try
            {
              
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();

                 
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                     
                        command.Parameters.AddWithValue("@VoucherCode", voucherCode);
                        command.Parameters.AddWithValue("@DiscountValue", discountValue);
                        command.Parameters.AddWithValue("@MinSpend", minSpend);
                        command.Parameters.AddWithValue("@MaxSpend", maxSpend);
                        command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                        command.Parameters.AddWithValue("@UsageLimit", usageLimit);

                      
                        int rowsAffected = command.ExecuteNonQuery();
                        MessageBox.Show("Voucher added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadVoucherData();
                        ClearFormFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearFormFields()
        {
          
            txtVoucherCode.Clear();
            txtDiscountValue.Clear();
            txtMinSpend.Clear();
            txtMaxSpend.Clear();

         
            usageLimitNumericUpDown.Value = usageLimitNumericUpDown.Minimum;

           
            dtpExpiryDate.Value = DateTime.Now;
        }
        private void LoadVoucherData()
        {
            string query = "SELECT VoucherID, VoucherCode, DiscountValue, MinSpend, MaxSpend, ExpiryDate, UsageLimit FROM Voucher";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // Set columns to auto-resize
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    // Rename the VoucherCode column header to "Code"
                    if (dataGridView1.Columns.Contains("VoucherCode"))
                    {
                        dataGridView1.Columns["VoucherCode"].HeaderText = "Code";
                    }

                    // Rename the DiscountValue column header to "Percentage"
                    if (dataGridView1.Columns.Contains("DiscountValue"))
                    {
                        dataGridView1.Columns["DiscountValue"].HeaderText = "Percentage";
                    }

                    // Other DataGridView settings
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.ReadOnly = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }




        private void Voucher_Load(object sender, EventArgs e)
        {
            LoadVoucherData();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Ensure VoucherCode and other fields are being populated correctly
                txtVoucherCode.Text = row.Cells["VoucherCode"].Value.ToString();
                txtDiscountValue.Text = row.Cells["DiscountValue"].Value.ToString();
                txtMinSpend.Text = row.Cells["MinSpend"].Value.ToString();
                txtMaxSpend.Text = row.Cells["MaxSpend"].Value.ToString();
                dtpExpiryDate.Text = row.Cells["ExpiryDate"].Value.ToString();
                usageLimitNumericUpDown.Value = Convert.ToDecimal(row.Cells["UsageLimit"].Value);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a voucher to delete.");
                return;
            }

            // Get the VoucherID of the selected row
            int selectedVoucherId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["VoucherID"].Value);

            if (selectedVoucherId == 0)
            {
                MessageBox.Show("Invalid VoucherID.");
                return;
            }

            string query = "DELETE FROM Voucher WHERE VoucherID = @VoucherID";

            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // Add the VoucherID parameter
                        cmd.Parameters.AddWithValue("@VoucherID", selectedVoucherId);

                        connection.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Voucher deleted successfully!");
                        LoadVoucherData();
                        ClearFormFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
