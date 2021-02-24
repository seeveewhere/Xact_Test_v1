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
    public partial class StockTransaction : Form
    {
        string conString = @"Data Source=DESKTOP-PJOL8GM\SQLEXPRESS;Initial Catalog=XactTestdb;Integrated Security=True;Pooling=False";

        SqlConnection conn;     //DECLARE SQL CONNECTION OBJECT
        SqlCommand command;     //DECLARE SQL COMMAND OBJECT        
        SqlDataAdapter adapter; //DECLARE SQL ADAPTER OBJECT
        DataTable dt;           //DECLARE SQL DATATABLE OBJECT
        string query;           //DECLARE SQL QUERY OBJECT
        public StockTransaction()
        {
            InitializeComponent();
        }

        private void StockTransaction_Load(object sender, EventArgs e)
        {
            ShowData();
        }

        public void ShowData() //GET LIST OF ALL DEBTORS
        {
            try
            {
                query = "SELECT * FROM tlStockTransactionFile";
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
                MessageBox.Show("Cannot show list of debtors: " + ex.Message);
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
