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
    public partial class EventAssistantChecklistcs : Form
    {
       
        public EventAssistantChecklistcs(string assistantName)
        {
            InitializeComponent();
            eventAssistantName = assistantName;
            LoadAssignedEventIDs();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";
        private string eventAssistantName;

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        private void LoadAssignedEventIDs()
        {
            string query = "SELECT EventID FROM Events WHERE EventAssistantName = @EventAssistantName";

            cmbBoxEventID.Items.Clear();

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventAssistantName", eventAssistantName);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            cmbBoxEventID.Items.Add(reader["EventID"].ToString());
                        }

                        if (cmbBoxEventID.Items.Count > 0)
                        {
                            cmbBoxEventID.SelectedIndex = 0;  
                        }
                        else
                        {
                            MessageBox.Show("No events assigned to you.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while loading Event IDs: {ex.Message}");
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int selectedEventID = Convert.ToInt32(cmbBoxEventID.SelectedItem);

            foreach (object item in checkedListBox.CheckedItems)
            {
                string checklistItem = item.ToString();
                bool isDone = checkedListBox.GetItemChecked(checkedListBox.Items.IndexOf(item));

                // Update checklist status in the database
                string query = "UPDATE ChecklistTable SET IsDone = @IsDone WHERE EventID = @EventID AND Checklist = @Checklist";

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventID", selectedEventID);
                        cmd.Parameters.AddWithValue("@Checklist", checklistItem);
                        cmd.Parameters.AddWithValue("@IsDone", isDone ? 1 : 0);

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery(); // Execute the update query
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                }
            }

            MessageBox.Show("Checklist items updated successfully!");
        }

        private void cmbBoxEventID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedEventID = Convert.ToInt32(cmbBoxEventID.SelectedItem);
            LoadChecklistItems(selectedEventID);
        }
        private void LoadChecklistItems(int eventID)
        {
            string query = "SELECT ChecklistID, Checklist, IsDone FROM ChecklistTable WHERE EventID = @EventID";

            checkedListBox.Items.Clear(); 

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventID", eventID);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string checklistItem = reader["Checklist"].ToString();
                            bool isDone = Convert.ToBoolean(reader["IsDone"]);

                          
                            int index = checkedListBox.Items.Add(checklistItem);
                            checkedListBox.SetItemChecked(index, isDone);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while loading checklist items: {ex.Message}");
                    }
                }
            }
        }
    }
}
