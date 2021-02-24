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

namespace Xact_Test_v1
{
    public partial class InvoiceDetailsPage : Form
    {
        private string conString = @"Data Source=DESKTOP-PJOL8GM\SQLEXPRESS;Initial Catalog=XactTestdb;Integrated Security=True;Pooling=False";

        SqlConnection conn;     //DECLARE SQL CONNECTION OBJECT
        SqlCommand command;     //DECLARE SQL COMMAND OBJECT        
        SqlDataAdapter adapter; //DECLARE SQL ADAPTER OBJECT
        DataTable dt;           //DECLARE SQL DATATABLE OBJECT
 
        string query;
        public InvoiceDetailsPage()
        {
            InitializeComponent();
        }

        private void InvoiceDetailsPage_Load(object sender, EventArgs e)
        {
            ShowData();
        }

        private void ShowData() //GET LIST OF INVOICEHEADER
        {
            try
            {
                query = "SELECT * FROM tlInvoiceDetail";
                conn = new SqlConnection(conString);
                command = new SqlCommand(query, conn);
                conn.Open();
                adapter = new SqlDataAdapter(query, conn);
                dt = new DataTable();
                adapter.Fill(dt);
                MydataGridView.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot show list: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                MessageBox.Show("Please input [ STOCK CODE ] to search");
            }
            else
            {
                (MydataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("Stock_code like '%{0}%'", txtSearch.Text);
            }
        }

        private void linkhome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HomePage s = new HomePage();
            if (s == null)
            {
                s.Parent = this;
            }
            s.Show();
            this.Hide();
        }

        private void Debtors_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DebtorPage s = new DebtorPage();
            if (s == null)
            {
                s.Parent = this;
            }
            s.Show();
            this.Hide();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StockPage s = new StockPage();
            if (s == null)
            {
                s.Parent = this;
            }
            s.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DebtorTransaction s = new DebtorTransaction();
            if (s == null)
            {
                s.Parent = this;
            }
            s.Show();
            this.Hide();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StockTransaction s = new StockTransaction();
            if (s == null)
            {
                s.Parent = this;
            }
            s.Show();
            this.Hide();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            InvoiceHeaderPage s = new InvoiceHeaderPage();
            if (s == null)
            {
                s.Parent = this;
            }
            s.Show();
            this.Hide();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            InvoiceDetailsPage s = new InvoiceDetailsPage();
            if (s == null)
            {
                s.Parent = this;
            }
            s.Show();
            this.Hide();
        }
    }
}
