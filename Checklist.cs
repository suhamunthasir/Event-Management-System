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
    public partial class Checklist : Form
    {
        public Checklist()
        {
            InitializeComponent();
        }
        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox.SelectedItem != null)
            {
                txtList.Text = checkedListBox.SelectedItem.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            string checklistText = txtList.Text.Trim();
            if (cmbBoxEventID.SelectedItem == null)
            {
                MessageBox.Show("Please select an Event ID.");
                return;
            }

            int eventID = Convert.ToInt32(cmbBoxEventID.SelectedItem);

            if (!string.IsNullOrEmpty(checklistText))
            {
                // Insert query, no need to insert ChecklistID explicitly
                string query = "INSERT INTO ChecklistTable (EventID, Checklist, IsDone) VALUES (@EventID, @Checklist, @IsDone)";

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventID", eventID);
                        cmd.Parameters.AddWithValue("@Checklist", checklistText);
                        cmd.Parameters.AddWithValue("@IsDone", 0); // Default to unchecked (0)

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery(); // This should work without the need for IDENTITY_INSERT

                            // Add the new item to the CheckedListBox
                            checkedListBox.Items.Add(checklistText);

                            // Optionally clear the TextBox
                            txtList.Clear();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid checklist item.");
            }
        }

        private int GetOrCreateChecklistID(int eventID)
        {
            // Query to check if a ChecklistID exists for the selected EventID
            string query = "SELECT TOP 1 ChecklistID FROM ChecklistTable WHERE EventID = @EventID";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventID", eventID);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result); // Return the existing ChecklistID
                        }
                        else
                        {
                            // If no ChecklistID exists for the Event, create a new ChecklistID
                            string insertQuery = "INSERT INTO ChecklistTable (EventID, Checklist, IsDone) VALUES (@EventID, '', 0)";
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@EventID", eventID);
                                insertCmd.ExecuteNonQuery();
                            }

                            // Retrieve the newly created ChecklistID
                            string newQuery = "SELECT TOP 1 ChecklistID FROM ChecklistTable WHERE EventID = @EventID ORDER BY ChecklistID DESC";
                            using (SqlCommand newCmd = new SqlCommand(newQuery, conn))
                            {
                                newCmd.Parameters.AddWithValue("@EventID", eventID);
                                object newResult = newCmd.ExecuteScalar();
                                return Convert.ToInt32(newResult); // Return the newly created ChecklistID
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
            }

            return 0; // Default return value (should not reach here)
        }



       

        private void btnGoback_Click(object sender, EventArgs e)
        {
            Event newForm = new Event();
            newForm.Show();
            this.Hide();
        }

        private void Checklist_Load(object sender, EventArgs e)
        {
            LoadEventIDs();
            cmbBoxEventID.SelectedIndexChanged += cmbBoxEventID_SelectedIndexChanged;
        }
        private void LoadEventIDs()
        {
            string query = "SELECT EventID FROM Events";
            cmbBoxEventID.Items.Clear();  

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (checkedListBox.SelectedItem != null)
            {
                string newChecklistText = txtList.Text.Trim();
                if (string.IsNullOrEmpty(newChecklistText))
                {
                    MessageBox.Show("Please enter a valid checklist item.");
                    return;
                }

                int eventID = Convert.ToInt32(cmbBoxEventID.SelectedItem);
                string oldChecklistText = checkedListBox.SelectedItem.ToString();
                bool isDone = checkedListBox.GetItemChecked(checkedListBox.SelectedIndex);

                // Get the ChecklistID for the selected EventID
                int checklistID = GetOrCreateChecklistID(eventID);

                // SQL query to update the checklist item
                string query = "UPDATE ChecklistTable SET Checklist = @Checklist, IsDone = @IsDone WHERE EventID = @EventID AND Checklist = @OldChecklist";

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Checklist", newChecklistText);
                        cmd.Parameters.AddWithValue("@OldChecklist", oldChecklistText);
                        cmd.Parameters.AddWithValue("@EventID", eventID);
                        cmd.Parameters.AddWithValue("@IsDone", isDone ? 1 : 0);

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery(); // Execute the update query

                            // Update the CheckedListBox item text
                            checkedListBox.Items[checkedListBox.SelectedIndex] = newChecklistText;
                            txtList.Clear(); // Optionally clear the TextBox

                            MessageBox.Show("Item updated successfully!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an item to update.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (checkedListBox.SelectedItem != null)
            {
                string checklistText = checkedListBox.SelectedItem.ToString();
                int selectedIndex = checkedListBox.SelectedIndex;
                int eventID = Convert.ToInt32(cmbBoxEventID.SelectedItem);

                // Query to get the ChecklistID for the selected item
                string query = "SELECT ChecklistID FROM ChecklistTable WHERE Checklist = @Checklist AND EventID = @EventID";

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Checklist", checklistText);
                        cmd.Parameters.AddWithValue("@EventID", eventID);

                        try
                        {
                            conn.Open();
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                int checklistID = Convert.ToInt32(result);

                                // Now, delete the item from the database
                                string deleteQuery = "DELETE FROM ChecklistTable WHERE ChecklistID = @ChecklistID AND EventID = @EventID";

                                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                                {
                                    deleteCmd.Parameters.AddWithValue("@ChecklistID", checklistID);
                                    deleteCmd.Parameters.AddWithValue("@EventID", eventID);

                                    deleteCmd.ExecuteNonQuery(); // Execute the delete query

                                    // Remove the item from the CheckedListBox
                                    checkedListBox.Items.RemoveAt(selectedIndex);

                                    // Optionally, clear the TextBox
                                    txtList.Clear();

                                    MessageBox.Show("Item deleted successfully!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Item not found in the database.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an item to delete.");
            }
        }
        private string GetEventAssistantName(int eventID)
        {
            string query = "SELECT EventAssistantName FROM Events WHERE EventID = @EventID";
            string assistantName = string.Empty;

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EventID", eventID);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        assistantName = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return assistantName;
        }
        private string GetExistingChecklistItems(int eventID)
        {
            string query = "SELECT ChecklistItem FROM Checklist WHERE EventID = @EventID";
            string existingItems = string.Empty;

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EventID", eventID);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        existingItems = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return existingItems;
        }
        private void LoadChecklistItems(int eventID)
        {
            string query = "SELECT Checklist, IsDone FROM ChecklistTable WHERE EventID = @EventID";
            checkedListBox.Items.Clear(); // Clear the CheckedListBox before loading new items

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EventID", eventID);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string checklistItem = reader["Checklist"].ToString();
                        bool isDone = Convert.ToBoolean(reader["IsDone"]);

                        checkedListBox.Items.Add(checklistItem, isDone); // Add item and set its checked state
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading checklist: " + ex.Message);
                }
            }
        }

        private void cmbBoxEventID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBoxEventID.SelectedItem != null)
            {
                int selectedEventID = Convert.ToInt32(cmbBoxEventID.SelectedItem);
                LoadChecklistItems(selectedEventID); // Load checklist for the selected Event ID
            }
        }
    }
}
