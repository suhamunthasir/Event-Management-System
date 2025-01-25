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
    public partial class Calendar : Form
    {
        public Calendar()
        {
            InitializeComponent();
        }



        public string conString = "Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true";



        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login newForm = new Login();
            newForm.Show();
            this.Hide();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            EventPlanner newForm = new EventPlanner();
            newForm.Show();
            this.Hide();
        }

        private void Calendar_Load(object sender, EventArgs e)
        {
            LoadEventData();
            MarkEventDatesOnCalendar();
        }

        private void LoadEventData()
        {
                        string query = @"
                   SELECT 
                e.EventID AS [Event ID],
                e.EventVenue AS [Venue],
                e.EventDate AS [Event Date],
                c.full_name AS [Customer Name]
            FROM 
                Events e
            JOIN 
                Customer c ON e.CustomerID = c.customerId;
            ";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // Auto-resize columns for better view
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

          private List<DateTime> GetEventDates()
          {
              List<DateTime> eventDates = new List<DateTime>();
              string query = "SELECT EventDate FROM Events";

              using (SqlConnection conn = new SqlConnection(conString))
              {
                  SqlCommand cmd = new SqlCommand(query, conn);

                  try
                  {
                      conn.Open();
                      SqlDataReader reader = cmd.ExecuteReader();
                      while (reader.Read())
                      {
                          eventDates.Add(Convert.ToDateTime(reader["EventDate"]));
                      }
                  }
                  catch (Exception ex)
                  {
                      MessageBox.Show("Error fetching event dates: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  }
              }

              return eventDates;
          }
          private void MarkEventDatesOnCalendar()
          {
              List<DateTime> eventDates = GetEventDates();

              if (eventDates.Count > 0)
              {
                  monthCalendar1.BoldedDates = eventDates.ToArray();
              }
              else
              {
                  MessageBox.Show("No event dates to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
              }
          }




        private void ShowEventReminder()
        {
            var closestEvent = GetClosestEvent();

            if (closestEvent.eventDate.HasValue)
            {
                // Show a message box with details of the closest event including the venue
                MessageBox.Show($"Reminder: The closest event is on {closestEvent.eventDate.Value.ToShortDateString()} at {closestEvent.eventVenue}. Please make the necessary arrangements.",
                    "Upcoming Event Reminder", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No upcoming events found.", "No Events", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private (DateTime? eventDate, string eventVenue) GetClosestEvent()
        {
            DateTime? closestEventDate = null;
            string closestEventVenue = null;
            DateTime currentDate = DateTime.Now;
            string query = "SELECT EventDate, EventVenue FROM Events WHERE EventDate >= @currentDate ORDER BY EventDate ASC";

            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@currentDate", currentDate);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Retrieve the closest event date and venue
                        closestEventDate = Convert.ToDateTime(reader["EventDate"]);
                        closestEventVenue = reader["EventVenue"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving event data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return (closestEventDate, closestEventVenue);
        }

        private void Calendar_Shown(object sender, EventArgs e)
        {
            ShowEventReminder();
        }
    }
}
