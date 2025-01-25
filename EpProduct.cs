using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class EpProduct : Form
    {
        public EpProduct()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";

        // A dictionary to store selected product IDs and their corresponding quantities
        private Dictionary<int, int> selectedProducts = new Dictionary<int, int>();
        Dictionary<int, Product> productsList = new Dictionary<int, Product>();

        private void btnGoback_Click(object sender, EventArgs e)
        {
            Event newForm = new Event();
            newForm.Show();
            this.Hide();
        }

       
        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            flowLayoutPanelProducts.Controls.Clear();

            // Updated SQL query to include SellingPrice
            string query = "SELECT ProductID, ProductName, SellingPrice, ProductImage FROM Products WHERE Category = @Category";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Category", comboBoxCategory.SelectedItem?.ToString());

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int productId = (int)reader["ProductID"];
                            string productName = reader["ProductName"].ToString();
                            decimal sellingPrice = Convert.ToDecimal(reader["SellingPrice"]);

                            // Store image if available
                            Image productImage = null;
                            if (reader["ProductImage"] != DBNull.Value)
                            {
                                byte[] imageData = (byte[])reader["ProductImage"];
                                using (MemoryStream ms = new MemoryStream(imageData))
                                {
                                    productImage = Image.FromStream(ms);
                                }
                            }

                            // Create a panel for the product
                            Panel productPanel = new Panel
                            {
                                Size = new Size(150, 250),
                                BorderStyle = BorderStyle.FixedSingle,
                                Margin = new Padding(10)
                            };

                            // PictureBox for the product image
                            PictureBox pictureBox = new PictureBox
                            {
                                Size = new Size(100, 100),
                                Image = productImage,
                                SizeMode = PictureBoxSizeMode.Zoom,
                                Dock = DockStyle.Top
                            };

                            // Label for the product name
                            Label lblName = new Label
                            {
                                Text = productName,
                                Dock = DockStyle.Top,
                                TextAlign = ContentAlignment.MiddleCenter
                            };

                            // Label for the product price
                            Label lblPrice = new Label
                            {
                                Text = $"Rs {sellingPrice:0.00}",
                                Dock = DockStyle.Top,
                                TextAlign = ContentAlignment.MiddleCenter
                            };

                            // NumericUpDown for quantity selection
                            NumericUpDown qtySelector = new NumericUpDown
                            {
                                Minimum = 1,
                                Maximum = 100,
                                Value = 1,
                                Dock = DockStyle.Bottom,
                                Tag = productId // Store ProductID in Tag
                            };

                            // CheckBox for product selection
                            CheckBox chkSelectProduct = new CheckBox
                            {
                                Text = "Select",
                                Dock = DockStyle.Bottom,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Tag = new { ProductID = productId, SellingPrice = sellingPrice } // Store data in Tag
                            };

                            // Event handler for CheckBox
                            chkSelectProduct.CheckedChanged += (sender, e) =>
                            {
                                var productDetails = (dynamic)chkSelectProduct.Tag;

                                if (chkSelectProduct.Checked)
                                {
                                    selectedProducts[productDetails.ProductID] = (int)qtySelector.Value;
                                }
                                else
                                {
                                    selectedProducts.Remove(productDetails.ProductID);
                                }

                                UpdateTotalPrice(selectedProducts);
                            };

                            // Event handler for NumericUpDown
                            qtySelector.ValueChanged += (sender, e) =>
                            {
                                if (chkSelectProduct.Checked)
                                {
                                    var productDetails = (dynamic)chkSelectProduct.Tag;
                                    selectedProducts[productDetails.ProductID] = (int)qtySelector.Value;
                                    UpdateTotalPrice(selectedProducts);
                                }
                            };

                            // Add controls to the product panel
                            productPanel.Controls.Add(lblPrice);
                            productPanel.Controls.Add(lblName);
                            productPanel.Controls.Add(pictureBox);
                            productPanel.Controls.Add(chkSelectProduct);
                            productPanel.Controls.Add(qtySelector);

                            // Add panel to the FlowLayoutPanel
                            flowLayoutPanelProducts.Controls.Add(productPanel);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        public class Product
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public decimal BuyingPrice { get; set; }
            public decimal SellingPrice { get; set; }
        }
        private void UpdateTotalPrice(Dictionary<int, int> selectedProducts)
        {
            decimal totalPrice = 0;

            foreach (var product in selectedProducts)
            {
                int productID = product.Key;
                int quantity = product.Value;
                decimal price = GetProductPrice(productID);

                totalPrice += price * quantity;
            }

            lblTotalPrice.Text = $"Total Price: Rs {totalPrice:0.00}";
        }

        private decimal GetProductPrice(int productID)
        {
            decimal price = 0;
            string query = "SELECT SellingPrice FROM Products WHERE ProductID = @ProductID"; // Use SellingPrice

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductID", productID);

                try
                {
                    conn.Open();
                    price = Convert.ToDecimal(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching price: " + ex.Message);
                }
            }

            return price;
        }





        private void Product_Panel_Click(object sender, EventArgs e)
        {
            Panel clickedPanel = sender as Panel;
            if (clickedPanel != null)
            {
                int selectedProductID = (int)clickedPanel.Tag;

              
                MessageBox.Show($"Product {selectedProductID} selected");

                // Optionally: Highlight the selected product, or add it to a cart list
                // Example: Change the background color of the selected panel
                clickedPanel.BackColor = Color.LightBlue;

                // You can also calculate the total price here, if you're storing selected products in a list
            }
        }
        private void LoadCategories()
        {
            // Query to fetch unique categories from the database
            string query = "SELECT DISTINCT Category FROM Products";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBoxCategory.Items.Add(reader["Category"].ToString());
                    }

                    if (comboBoxCategory.Items.Count > 0)
                    {
                        comboBoxCategory.SelectedIndex = 0;  // Select the first category by default
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void EpProduct_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadProducts();
            LoadCustomers();
        }

        private void flowLayoutPanelProducts_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Debugging: Check if the ComboBox items are selected correctly
            var eventIdSelectedItem = comboBoxEventID.SelectedItem;
            var customerIdSelectedItem = comboBoxCustomerID.SelectedItem;

            // Debugging output to verify selected items
            Console.WriteLine("Event ID Selected: " + eventIdSelectedItem?.ToString());
            Console.WriteLine("Customer ID Selected: " + customerIdSelectedItem?.ToString());

            // Check if both Event ID and Customer ID are selected
            if (eventIdSelectedItem == null || customerIdSelectedItem == null)
            {
                MessageBox.Show("Please select both Event ID and Customer ID.");
                return;
            }

            int customerId = 0;
           

            // If the selected item is a string, parse it as integer (for comboBoxEventID)
            if (eventIdSelectedItem is string eventIdStr)
            {
                MessageBox.Show("Please select a valid Event ID.");
                return;
            }

            // If the selected item is a Customer object, get CustomerID (for comboBoxCustomerID)
            if (customerIdSelectedItem is Customer selectedCustomer)
            {
                customerId = selectedCustomer.CustomerID;
            }
            else
            {
                MessageBox.Show("Please select a valid Customer ID.");
                return;
            }

            // Now, eventId and customerId are properly parsed and validated
            // Proceed with the rest of the logic

            decimal totalPrice = 0; // Total price to save in EventProduct
            Dictionary<int, decimal> supplierCosts = new Dictionary<int, decimal>(); // To track total cost per supplier

            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    foreach (var selectedProduct in selectedProducts)
                    {
                        int productId = selectedProduct.Key;
                        int quantity = selectedProduct.Value;

                        // Fetch product details (including SupplierID, SellingPrice, and BuyingPrice)
                        string query = "SELECT SellingPrice, BuyingPrice, SupplierID FROM Products WHERE ProductID = @ProductID";
                        SqlCommand cmd = new SqlCommand(query, conn, transaction);
                        cmd.Parameters.AddWithValue("@ProductID", productId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            decimal productTotalCost = 0;
                            while (reader.Read())
                            {
                                decimal sellingPrice = Convert.ToDecimal(reader["SellingPrice"]);
                                decimal buyingPrice = Convert.ToDecimal(reader["BuyingPrice"]);
                                int supplierId = Convert.ToInt32(reader["SupplierID"]);

                                // Calculate price and cost
                                decimal productTotalPrice = sellingPrice * quantity;
                                productTotalCost = buyingPrice * quantity;

                                // Add to the total price
                                totalPrice += productTotalPrice;

                                // Add to supplier cost
                                if (!supplierCosts.ContainsKey(supplierId))
                                {
                                    supplierCosts[supplierId] = 0;
                                }
                                supplierCosts[supplierId] += productTotalCost;
                               
                            }
                            reader.Close();
                            // Save product details to EventProduct table
                            string insertQuery = "INSERT INTO EventProduct (EventID, ProductID, Quantity, TotalPrice) VALUES (@EventID, @ProductID, @Quantity, @TotalPrice)";
                            SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction);
                            insertCmd.Parameters.AddWithValue("@EventID", eventIdSelectedItem);  // Use the valid eventId
                            insertCmd.Parameters.AddWithValue("@ProductID", productId);
                            insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                            insertCmd.Parameters.AddWithValue("@TotalPrice", productTotalCost);
                            insertCmd.ExecuteNonQuery();
                            insertCmd.Dispose();


                            


                        }
                    }

                    // Update supplier salaries
                    foreach (var supplierCost in supplierCosts)
                    {
                        int supplierId = supplierCost.Key;
                        decimal totalCost = supplierCost.Value;

                        string updateQuery = "UPDATE Supplier SET Salary = Salary + @TotalCost WHERE id = @SupplierID";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction);
                        updateCmd.Parameters.AddWithValue("@TotalCost", totalCost);
                        updateCmd.Parameters.AddWithValue("@SupplierID", supplierId);
                        updateCmd.ExecuteNonQuery();
                    }

                    // Commit transaction
                    transaction.Commit();

                    MessageBox.Show("Products and total price saved successfully!");
                    lblTotalPrice.Text = "Total Price: Rs 0.00"; // Reset total price label
                    selectedProducts.Clear(); // Clear the selected products list
                   // flowLayoutPanelProducts.Controls.Clear(); // Optionally refresh the product list
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void LoadEventsForCustomer(int customerID)
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








        private void comboBoxCustomerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomerID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a customer first.");
                return;
            }

            Customer selectedCustomer = (Customer)comboBoxCustomerID.SelectedItem;


         
            LoadEventsForCustomer(selectedCustomer.CustomerID);
        }

        private void LoadCustomers()
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

    }
}