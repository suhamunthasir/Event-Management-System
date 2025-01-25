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
    public partial class Product : Form
    {
        public Product()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";
       

        private void btnGoback_Click(object sender, EventArgs e)
        {
            EventManager newForm = new EventManager();
            newForm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void LoadProductDetails()
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                string query = "SELECT p.ProductID, p.ProductName, p.BuyingPrice, p.SellingPrice, p.Quantity, p.Category, " +
                               "s.Name AS Supplier, p.ProductImage " +
                               "FROM Products p " +
                               "JOIN Supplier s ON p.SupplierID = s.Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable productTable = new DataTable();

                try
                {
                    conn.Open();
                    adapter.Fill(productTable);
                    dataGridViewProducts.DataSource = productTable;
                    dataGridViewProducts.Columns["ProductImage"].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Set the image to the PictureBox
                    picProductImage.Image = Image.FromFile(openFileDialog.FileName);
                    picProductImage.Tag = openFileDialog.FileName;

                    // Set the PictureBox SizeMode to Zoom for proper scaling
                    picProductImage.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                !decimal.TryParse(txtBuyingPrice.Text, out decimal buyingPrice) || buyingPrice <= 0 ||
                !decimal.TryParse(txtSellingPrice.Text, out decimal sellingPrice) || sellingPrice <= 0 ||
                !int.TryParse(txtProductQty.Text, out int quantity) || quantity <= 0 ||
                comboBoxSupplier.SelectedIndex == -1 ||
                comboBoxCategory.SelectedIndex == -1 ||
                picProductImage.Image == null)
            {
                MessageBox.Show("Please ensure all fields are valid and filled.");
                return;
            }

            string productName = txtProductName.Text;
            string category = comboBoxCategory.SelectedItem?.ToString();
            string supplier = comboBoxSupplier.SelectedItem?.ToString();
            byte[] productImage;

            using (MemoryStream ms = new MemoryStream())
            {
                picProductImage.Image.Save(ms, picProductImage.Image.RawFormat);
                productImage = ms.ToArray();
            }

            using (SqlConnection conn = new SqlConnection(conString))
            {
                string query = "INSERT INTO Products (ProductName, BuyingPrice, SellingPrice, Quantity, SupplierID, Category, ProductImage) " +
                               "VALUES (@ProductName, @BuyingPrice, @SellingPrice, @Quantity, " +
                               "(SELECT Id FROM Supplier WHERE Name = @Supplier), @Category, @ProductImage)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.Parameters.AddWithValue("@BuyingPrice", buyingPrice);
                cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@Supplier", supplier);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@ProductImage", (object)productImage ?? DBNull.Value);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product added successfully.");
                    LoadProductDetails();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void ClearFields()
        {
            txtProductName.Clear();
            txtBuyingPrice.Clear();
            txtSellingPrice.Clear();
            txtProductQty.Clear();
            comboBoxSupplier.SelectedIndex = -1;
            comboBoxCategory.SelectedIndex = -1;
            picProductImage.Image = null;
        }

        private void Product_Load(object sender, EventArgs e)
        {
            LoadProductDetails();
            LoadSupplierData();
            comboBoxSupplier.SelectedIndex = -1;
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

                    
                    comboBoxSupplier.Items.Clear();

                    
                    foreach (DataRow row in dt.Rows)
                    {
                        comboBoxSupplier.Items.Add(row["name"].ToString());
                    }

                    
                    if (comboBoxSupplier.Items.Count > 0)
                    {
                        comboBoxSupplier.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                  
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void dataGridViewProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow row = dataGridViewProducts.Rows[e.RowIndex];

                // Populate fields with data from the selected row
                txtProductName.Text = row.Cells["ProductName"].Value.ToString();
                txtBuyingPrice.Text = row.Cells["BuyingPrice"].Value.ToString();
                txtSellingPrice.Text = row.Cells["SellingPrice"].Value.ToString();
                txtProductQty.Text = row.Cells["Quantity"].Value.ToString();
                comboBoxCategory.SelectedItem = row.Cells["Category"].Value.ToString();
                comboBoxSupplier.SelectedItem = row.Cells["Supplier"].Value.ToString();

                // Load the image into the PictureBox
                byte[] imageData = (byte[])row.Cells["ProductImage"].Value;
                if (imageData != null && imageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        picProductImage.Image = Image.FromStream(ms);
                        picProductImage.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
                else
                {
                    picProductImage.Image = null;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0 &&
                !string.IsNullOrWhiteSpace(txtProductName.Text) &&
                decimal.TryParse(txtBuyingPrice.Text, out decimal buyingPrice) && buyingPrice > 0 &&
                decimal.TryParse(txtSellingPrice.Text, out decimal sellingPrice) && sellingPrice > 0 &&
                int.TryParse(txtProductQty.Text, out int quantity) && quantity > 0 &&
                comboBoxSupplier.SelectedIndex != -1 &&
                comboBoxCategory.SelectedIndex != -1)
            {
                string productId = dataGridViewProducts.SelectedRows[0].Cells["ProductID"].Value.ToString();
                string productName = txtProductName.Text;
                string category = comboBoxCategory.SelectedItem?.ToString();
                string supplier = comboBoxSupplier.SelectedItem?.ToString();

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    string query = @"UPDATE Products 
                                     SET ProductName = @ProductName, 
                                         BuyingPrice = @BuyingPrice, 
                                         SellingPrice = @SellingPrice, 
                                         Category = @Category, 
                                         SupplierID = (SELECT Id FROM Supplier WHERE Name = @Supplier), 
                                         Quantity = @Quantity 
                                     WHERE ProductID = @ProductID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.Parameters.AddWithValue("@BuyingPrice", buyingPrice);
                    cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@Supplier", supplier);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Product updated successfully.");
                        LoadProductDetails();
                        ClearFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please ensure all fields are valid and a product is selected.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0) // Ensure a product is selected
            {
                string productId = dataGridViewProducts.SelectedRows[0].Cells["ProductID"].Value.ToString();


                using (SqlConnection conn = new SqlConnection(conString))
                {
                    string query = "DELETE FROM Products WHERE  ProductID= @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", productId);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Product deleted successfully.");
                        LoadProductDetails(); // Refresh the grid
                        ClearFields(); // Clear the form
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.");
            }
        }

        private void dataGridViewProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
