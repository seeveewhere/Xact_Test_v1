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
    public partial class StockPage : Form
    {
        private string conString = @"Data Source=DESKTOP-PJOL8GM\SQLEXPRESS;Initial Catalog=XactTestdb;Integrated Security=True;Pooling=False";

        SqlConnection conn;     //DECLARE SQL CONNECTION OBJECT
        SqlCommand command;     //DECLARE SQL COMMAND OBJECT        
        SqlDataAdapter adapter; //DECLARE SQL ADAPTER OBJECT
        DataTable dt;           //DECLARE SQL DATATABLE OBJECT
        string query;           //DECLARE SQL QUERY OBJECT
        public StockPage()
        {
            InitializeComponent();
        }

        private void StockPage_Load(object sender, EventArgs e)
        {
            ShowData(); //SHOW LIST OF STOCK ON PAGE LOAD
            cmb_stock_code.Focus();            
            lblDate.Text = DateTime.Now.Date.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e) //SAVE BUTTON
        {
           AddStock();            
        }
        private void btnRefresh_Click(object sender, EventArgs e) //GET LIST BUTTON
        {
            ShowData();
        }       
        private void btnClear_Click(object sender, EventArgs e) //CLEAR BUTTON
        {
            ClearField();
        }
        private void btnDelete_Click(object sender, EventArgs e) //DELETE BUTTON
        {
            DeleteStock();
        }

        private void btnUpdate_Click(object sender, EventArgs e) //UPDATE BUTTON
        {
            UpdateStock();
        }
        private void btnSearch_Click_1(object sender, EventArgs e) //SEARCH BUTTON
        {
            Search();
        }

        /*******************************************************METHODS FOR DATA MANIPULATION*******************************************************************/
        public void AddStock() //FIRST CHECK IF FIELDS ARE NOT EMPTY THEN ADD STOCK RECORD
        {
            if (cmb_stock_code.SelectedItem == null)
            {
                MessageBox.Show("Please select code");
            }
            else if (txtDescription.Text == "")
            {
                MessageBox.Show("Please input address");
            }
            else if (txtSellingPrice.Text == "")
            {
                MessageBox.Show("Please input balance");
            }
            else if (txtCost.Text == "")
            {
                MessageBox.Show("Please input number of sales to date");
            }            
            else if (txtTotPurchase_excl_vat.Text == "")
            {
                MessageBox.Show("Please input cost");
            }
            else if (txtTotalSales_exl_vat.Text == "")
            {
                MessageBox.Show("Please input number of sales to date");
            }
            else if (txtQuantityPurchased.Text == "")
            {
                MessageBox.Show("Please input number of sales to date");
            }
            else if (txtQuantitySold.Text == "")
            {
                MessageBox.Show("Please input number of sales to date");
            }
            else if (txtStockOnHand.Text == "")
            {
                MessageBox.Show("Please input number of sales to date");
            }            
            else
            {
                try
                {
                    //ADD TO tlStock
                    query = "INSERT INTO tlStock VALUES ( '" + cmb_stock_code.Text + "', '" + txtDescription.Text + "', '" + float.Parse(txtCost.Text) + "' , '" + float.Parse(txtSellingPrice.Text) + "','" + float.Parse(txtTotPurchase_excl_vat.Text)+ "','" + float.Parse(txtTotalSales_exl_vat.Text) + "','" + Int32.Parse(txtQuantityPurchased.Text) + "','" + Int16.Parse(txtQuantitySold.Text) + "','" + Int16.Parse(txtStockOnHand.Text) + "')";
                    conn = new SqlConnection(conString);
                    command = new SqlCommand(query, conn);

                    //ADD TO tlStockTransactionFile
                    string transactionType = "Adding Stock";
                    string query2 = "INSERT INTO tlStockTransactionFile VALUES ( '" + cmb_stock_code.Text + "', '" + lblDate.Text + "', '" + transactionType.ToString() + "' , '" + Int16.Parse(txtQuantityPurchased.Text) + "','" + float.Parse(txtCost.Text) + "','" + float.Parse(txtSellingPrice.Text) + "')";
                    SqlCommand command2 = new SqlCommand(query2, conn);              
                    
                    conn.Open();
                    command.ExecuteNonQuery();
                    command2.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Successfully added");
                    ShowData();
                    ClearField();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Stock not added because: " + ex.Message);
                }
            }            
        }
       
        public void ShowData() //GET LIST OF ALL STOCK
        {
            try
            {
                query = "SELECT * FROM tlStock";
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
                MessageBox.Show("Cannot show list of Stock: " + ex.Message);
            }
        }

        public void Search() //SEARCH USING STOCK CODE
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
        public void ClearField() //CLEAR FIELD TO ACCEPT NEW RECORD
        {
            txtDescription.Clear();
            txtSellingPrice.Clear();
            txtCost.Clear();
            txtTotPurchase_excl_vat.Clear();
            txtTotalSales_exl_vat.Clear();            
            cmb_stock_code.SelectedIndex = -1;
            txtQuantityPurchased.Clear();
            txtQuantitySold.Clear();
            txtStockOnHand.Clear();
            cmb_stock_code.Focus();
        }

        private void DeleteStock() //DELETE STOCK RECORD
        {
            if (cmb_stock_code.Text == "")
            {
                MessageBox.Show("Select Stock Code to delete");
            }
            else
            {
                try
                {
                    query = "DELETE FROM tlStock WHERE Stock_code=('" + cmb_stock_code.Text + "')";
                    conn = new SqlConnection(conString);
                    command = new SqlCommand(query, conn);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Stock successfully deleted");
                    ShowData();
                    //ClearField();
                    cmb_stock_code.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Debtor not deleted because: " + ex.Message);
                }
            }
        }

        public void UpdateStock() //UPDATE STOCK RECORD
        {
            if (cmb_stock_code.Text == "")
            {
                MessageBox.Show("Please Select Record to Update");
            }
            else
            {
                try
                {
                    query = "UPDATE tlStock SET Stock_on_hand=('" + Int16.Parse(txtStockOnHand.Text) + "') WHERE Stock_code=('" + cmb_stock_code.Text + "')";
                    command = new SqlCommand(query, conn);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Updated Successfully");
                    ShowData();
                    ClearField();
                    cmb_stock_code.Enabled = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Please Select Record to Update");
                }
            }
        }

        private void MydataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) //ATTACH TO TEXTBOXES FOR UPDATE AND DELETE
        {
            cmb_stock_code.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtDescription.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[1].Value.ToString());
            txtCost.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[2].Value.ToString());
            txtSellingPrice.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[3].Value.ToString());
            txtTotPurchase_excl_vat.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[4].Value.ToString());
            txtTotalSales_exl_vat.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[5].Value.ToString());
            txtQuantityPurchased.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[6].Value.ToString());
            txtQuantitySold.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[7].Value.ToString());
            txtStockOnHand.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[8].Value.ToString());
            cmb_stock_code.Enabled = false;
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
