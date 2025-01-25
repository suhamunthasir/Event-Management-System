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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";
        private void btnLogin_Click(object sender, EventArgs e)
        {

           
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            
            string selectedCategory = comboCategory.SelectedItem.ToString();
            string tableName = string.Empty;

            switch (selectedCategory)
            {
                case "Event Planner":
                    tableName = "EventPlanner";
                    break;
                case "Event Manager":
                    tableName = "EventManager";
                    break;
                case "Event Assistant":
                    tableName = "EventAssistant";
                    break;
                case "Logistic Manager":
                    tableName = "LogisticManager";
                    break;
                case "General Manager":
                    tableName = "GeneralManager";  
                    break;
                case "Finance Manager":
                    tableName = "FinanceManager";  
                    break;
                default:
                    MessageBox.Show("Please select a valid category.");
                    return;
            }

           
            using (SqlConnection conn = new SqlConnection(conString))
            {
                string query = $"SELECT * FROM {tableName} WHERE username = @username AND password = @password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        // Assuming Event Assistant is logging in
                        if (selectedCategory == "Event Assistant")
                        {
                            // Fetch the Event Assistant's name
                            reader.Read();
                            string eventAssistantName = reader["Name"].ToString(); // Assuming the name field is 'Name'

                            // Hide the login form and open the checklist form, passing the event assistant's name
                            this.Hide();
                            EventAssistantChecklistcs eaForm = new EventAssistantChecklistcs(eventAssistantName);
                            eaForm.Show();
                        }
                        else
                        {
                            // For other categories, open respective forms as usual
                            this.Hide();
                            OpenMainForm(selectedCategory);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                        txtPassword.Clear();
                        txtUsername.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }


        }
        private void OpenMainForm(string category)
        {
          
            if (category == "Event Planner")
            {
                EventPlanner epForm = new EventPlanner();
                epForm.Show();
            }
            else if (category == "Event Manager")
            {
                EventManager emForm = new EventManager();
                emForm.Show();
            }
          
            else if (category == "Logistic Manager")
            {
                LogisticManages lmForm = new LogisticManages();
                lmForm.Show();
            }
            else if (category == "General Manager")
            {
                GeneralManager gmForm = new GeneralManager();
                gmForm.Show();  
            }
            else if (category == "Finance Manager")
            {
                FinanceManager fmForm = new FinanceManager();
                fmForm.Show();  
            }
        }
       /* private void lnkLblForgotPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPassword newForm = new ForgotPassword();
            newForm.Show();
            this.Hide();
        } */

        private void Login_Load(object sender, EventArgs e)
        {
            comboCategory.Items.Add("Event Planner");
            comboCategory.Items.Add("Event Manager");
            comboCategory.Items.Add("Event Assistant");
            comboCategory.Items.Add("Logistic Manager");
            comboCategory.Items.Add("General Manager");
            comboCategory.Items.Add("Finance Manager");
        }
    }
}
