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
    public partial class print : Form
    {
        public print()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-SM1EC12;Initial Catalog=EventManagementSystemDb;Integrated Security=True;TrustServerCertificate=true");
        SqlCommand cmd1;
        SqlCommand cmd2;

        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        private void print_Load(object sender, EventArgs e)
        {

            string query = "SELECT EventID, CustomerID, EventDate, EventVenue, EventAssistantName, LogisticManagerName, Category FROM Events";
                           
                         

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet dt = new DataSet();

                try
                {
                    conn.Open();
                    da.Fill(dt,"Events");
                    CrystalReport3 report = new CrystalReport3();
                    report.SetDataSource(dt);
                    crystalReportViewer1.ReportSource = report;
                    crystalReportViewer1.RefreshReport();


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            /*
            CrystalReport3 report = new CrystalReport3(); // instance of my rpt file
            var ds = new DataSet3();  // DsBilling is mine XSD
            var table2 = ds.;
            var adapter2 = new VendorTableAdapter();
            adapter2.Fill(table2);

            var table = ds.Bill;
            var adapter = new BillTableAdapter();
            string name = cboCustReport.Text;
            int month = int.Parse(cboRptFromMonth.SelectedItem.ToString());
            int year = int.Parse(cboReportFromYear.SelectedItem.ToString());
            adapter.Fill(table, name, month, year);

            ds.AcceptChanges();
            */


        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
