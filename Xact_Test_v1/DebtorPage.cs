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
    public partial class DebtorPage : Form
    {
        string conString = @"Data Source=DESKTOP-PJOL8GM\SQLEXPRESS;Initial Catalog=XactTestdb;Integrated Security=True;Pooling=False";

        SqlConnection conn;     //DECLARE SQL CONNECTION OBJECT
        SqlCommand command;     //DECLARE SQL COMMAND OBJECT        
        SqlDataAdapter adapter; //DECLARE SQL ADAPTER OBJECT
        DataTable dt;           //DECLARE SQL DATATABLE OBJECT
        string query;           //DECLARE SQL QUERY OBJECT
        public DebtorPage()
        {
            InitializeComponent();
        }
        private void DebtorPage_Load(object sender, EventArgs e)
        {            
            ShowData(); //SHOW DATA ON PAGE LOAD
        }
        private void btnSave_Click(object sender, EventArgs e) //SAVE BUTTON
        {
            AddDebtor();      
        }
        private void btnRefresh_Click(object sender, EventArgs e) //GET LIST BUTTON
        {
            ShowData();
        }
        private void btnSearch_Click(object sender, EventArgs e) //SEARCH BUTTON
        {
            Search();
        }
        private void btnClear_Click(object sender, EventArgs e) //CLEAR BUTTON
        {
            ClearField();
        }
        private void btnDelete_Click(object sender, EventArgs e) //DELETE BUTTON
        {
            DeleteDebtor();
        }        

        private void btnUpdate_Click(object sender, EventArgs e) //UPDATE BUTTON
        {
            UpdateDebtor();
        }

        /*******************************************************METHODS FOR DATA MANIPULATION*******************************************************************/
        public void AddDebtor() //FIRST CHECK IF FIELDS ARE NOT EMPTY THEN ADD DEBTOR RECORD
        {
            if (cmb_Account_Code.SelectedItem == null)
            {
                MessageBox.Show("Please select code");
            }
            else if (txtAddress1.Text == "")
            {
                MessageBox.Show("Please input address");
            }
            else if (txtBalance.Text == "")
            {
                MessageBox.Show("Please input balance");
            }
            else if (txtSales.Text == "")
            {
                MessageBox.Show("Please input number of sales to date");
            }
            else if (txtCost.Text == "")
            {
                MessageBox.Show("Please input cost");
            }
            else
            {
                try
                {
                    query = "INSERT INTO tlDebtor VALUES ( '" + cmb_Account_Code.Text + "', '" + txtAddress1.Text + "', '" + float.Parse(txtBalance.Text) + "' , '" + Int16.Parse(txtSales.Text) + "','" + float.Parse(txtCost.Text) + "')";
                    conn = new SqlConnection(conString);
                    command = new SqlCommand(query, conn);
                    conn.Open();                    
                    command.ExecuteNonQuery();
                    conn.Close();                    
                    MessageBox.Show("Debtor successfully added");
                    ShowData();
                    ClearField();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Debtor not added because: " + ex.Message);
                }
            }
        }
        public void ShowData() //GET LIST OF ALL DEBTORS
        {
            try
            {
                query = "SELECT * FROM tlDebtor";
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

        public void Search() //SEARCH USING ACCOUNT CODE
        {
            if (txtSearch.Text == "")
            {
                MessageBox.Show("Please input [ ACCOUNT CODE ] to search");
            }
            else
            {
                (MydataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("Account_code like '%{0}%'", txtSearch.Text);
            }
        }
        public void ClearField() //CLEAR FIELD TO ACCEPT NEW RECORD
        {
            txtAddress1.Clear();
            txtBalance.Clear();
            txtCost.Clear();
            txtSales.Clear();
            cmb_Account_Code.Focus();
            cmb_Account_Code.SelectedIndex = -1;
        }

        private void DeleteDebtor() //DELETE DEBTOR RECORD
        {
            if (cmb_Account_Code.Text == "")
            {
                MessageBox.Show("Select Account Code to delete");
            }
            else
            {
                try
                {
                    query = "DELETE FROM tlDebtor WHERE Account_code=('" + cmb_Account_Code.Text + "')";
                    conn = new SqlConnection(conString);
                    command = new SqlCommand(query, conn);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Debtor successfully deleted");
                    ShowData();
                    ClearField();
                    cmb_Account_Code.Enabled = true; txtBalance.Enabled = true; txtCost.Enabled = true; txtSales.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Debtor not deleted because: " + ex.Message);
                }
            }
        }
        
        public void UpdateDebtor() //UPDATE DEBTOR RECORD
        {
            if(cmb_Account_Code.Text=="")
            {
                MessageBox.Show("Please Select Record to Update");
            }
            else
            {
                try
                {
                    query = "UPDATE tlDebtor SET Address1=('" + txtAddress1.Text + "') WHERE Account_code=('" + cmb_Account_Code.Text + "')";
                    command = new SqlCommand(query, conn);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Updated Successfully");
                    ShowData();
                    ClearField();
                    cmb_Account_Code.Enabled = true; txtBalance.Enabled = true; txtCost.Enabled = true; txtSales.Enabled = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Please Select Record to Update");
                }
            }
        }

        private void MydataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) //ATTACH TO TEXTBOXES FOR UPDATE AND DELETE
        {            
            cmb_Account_Code.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtAddress1.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[1].Value.ToString());
            txtBalance.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[2].Value.ToString());
            txtSales.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[3].Value.ToString());
            txtCost.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[4].Value.ToString());
            cmb_Account_Code.Enabled = false; txtBalance.Enabled = false;txtCost.Enabled = false;txtSales.Enabled = false;
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
