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
    public partial class DebtorTransaction : Form
    {
        string conString = @"Data Source=DESKTOP-PJOL8GM\SQLEXPRESS;Initial Catalog=XactTestdb;Integrated Security=True;Pooling=False";

        SqlConnection conn;     //DECLARE SQL CONNECTION OBJECT
        SqlCommand command;     //DECLARE SQL COMMAND OBJECT        
        SqlDataAdapter adapter; //DECLARE SQL ADAPTER OBJECT
        DataTable dt;           //DECLARE SQL DATATABLE OBJECT
        string query;           //DECLARE SQL QUERY OBJECT
        public DebtorTransaction()
        {
            InitializeComponent();
        }       

        private void DebtorTransaction_Load(object sender, EventArgs e) //  PAGE LOAD
        {
            //populateComboBox();
            //txtDate.Text = DateTime.Now.Date.ToShortDateString();
            ShowData();
        }

        //private void btnSave_Click(object sender, EventArgs e) //SAVE BUTTON
        //{
        //    AddDebtor();
        //}
        //private void btnRefresh_Click(object sender, EventArgs e) //GET LIST BUTTON
        //{
        //    ShowData();
        //}
        private void btnSearch_Click(object sender, EventArgs e) //SEARCH BUTTON
        {
            Search();
        }
        //private void btnClear_Click(object sender, EventArgs e) //CLEAR BUTTON
        //{
        //    ClearField();
        //}
        //private void btnDelete_Click(object sender, EventArgs e) //DELETE BUTTON
        //{
        //    DeleteDebtor();
        //}

        //private void btnUpdate_Click(object sender, EventArgs e) //UPDATE BUTTON
        //{
        //    UpdateDebtor();
        //}

        /*******************************************************METHODS FOR DATA MANIPULATION*******************************************************************/
        //public void AddDebtor() //FIRST CHECK IF FIELDS ARE NOT EMPTY THEN ADD DEBTOR RECORD
        //{
        //    if (cmb_Account_Code.SelectedItem == null)
        //    {
        //        MessageBox.Show("Please select code");
        //    }
        //    else if (txtDate.Text == "")
        //    {
        //        MessageBox.Show("Please input date");
        //    }
        //    else if (cmb_Transaction_Type.Text == "")
        //    {
        //        MessageBox.Show("Please input transaction type");
        //    }
        //    else if (txtGross_Value.Text == "")
        //    {
        //        MessageBox.Show("Please input gross transaction value");
        //    }
        //    else if (txt_Vat_Value.Text == "")
        //    {
        //        MessageBox.Show("Please vat value");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            query = "INSERT INTO tlDebtorTransactionFile VALUES ( '" + cmb_Account_Code.Text + "', '" + DateTime.Parse(txtDate.Text) + "', '" + cmb_Transaction_Type.Text + "' , '" + float.Parse(txtGross_Value.Text) + "','" + float.Parse(txt_Vat_Value.Text) + "')";
        //            conn = new SqlConnection(conString);
        //            command = new SqlCommand(query, conn);
        //            conn.Open();
        //            command.ExecuteNonQuery();
        //            conn.Close();
        //            MessageBox.Show("Debtor transaction file successfully added");
        //            ShowData();
        //            ClearField();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Debtor transaction file not added because: " + ex.Message);
        //        }
        //    }
        //}
        public void ShowData() //GET LIST OF ALL DEBTORS
        {
            try
            {
                query = "SELECT * FROM tlDebtorTransactionFile";
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
        //public void ClearField() //CLEAR FIELD TO ACCEPT NEW RECORD
        //{
        //    cmb_Account_Code.SelectedIndex=-1;
        //    txtDate.Clear();
        //    cmb_Transaction_Type.SelectedIndex = -1;
        //    txtGross_Value.Clear();            
        //    txt_Vat_Value.Clear();
        //    cmb_Account_Code.Focus();
        //}

        //private void DeleteDebtor() //DELETE DEBTOR RECORD
        //{
        //    if (cmb_Account_Code.Text == "")
        //    {
        //        MessageBox.Show("Select Account Code to delete");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            query = "DELETE FROM tlDebtorTransactionFile WHERE Account_code=('" + cmb_Account_Code.Text + "')";
        //            conn = new SqlConnection(conString);
        //            command = new SqlCommand(query, conn);
        //            conn.Open();
        //            command.ExecuteNonQuery();
        //            conn.Close();
        //            MessageBox.Show("Debtor transaction file successfully deleted");
        //            ShowData();
        //            ClearField();
        //            cmb_Account_Code.Enabled = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Debtor transaction file not deleted because: " + ex.Message);
        //        }
        //    }
        //}

        //public void UpdateDebtor() //UPDATE DEBTOR RECORD
        //{
        //    if (cmb_Account_Code.Text == "")
        //    {
        //        MessageBox.Show("Please Select Record to Update");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            query = "UPDATE tlDebtorTransactionFile SET Gross_transaction_value=('" + txtGross_Value.Text + "') WHERE Account_code=('" + cmb_Account_Code.Text + "')";
        //            command = new SqlCommand(query, conn);
        //            conn.Open();
        //            command.ExecuteNonQuery();
        //            conn.Close();
        //            MessageBox.Show("Updated Successfully");
        //            ShowData();
        //            ClearField();
        //            cmb_Account_Code.Enabled = true;
        //        }
        //        catch (Exception)
        //        {
        //            MessageBox.Show("Please Select Record to Update");
        //        }
        //    }
        //}

        //private void MydataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) //ATTACH TO TEXTBOXES FOR UPDATE AND DELETE
        //{            
        //    cmb_Account_Code.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[1].Value.ToString());
        //    txtDate.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[2].Value.ToString());
        //    cmb_Transaction_Type.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[3].Value.ToString());
        //    txtGross_Value.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[4].Value.ToString());
        //    txt_Vat_Value.Text = Convert.ToString(MydataGridView.Rows[e.RowIndex].Cells[5].Value.ToString());
        //}

        //public void populateComboBox()
        //{
        //    MydataGridView.AutoGenerateColumns = false;
        //    dt = new DataTable();
        //    conn = new SqlConnection(conString);
        //    conn.Open();
        //    query = "SELECT Account_code FROM tlDebtor";
        //    command = new SqlCommand(query, conn);
        //    adapter = new SqlDataAdapter(command);
        //    adapter.Fill(dt);
        //    cmb_Account_Code.ValueMember = "Account_Code";
        //    cmb_Account_Code.DisplayMember = "Account_Code";
        //    cmb_Account_Code.DataSource = dt;
        //    cmb_Account_Code.SelectedIndex = -1;            
        //    conn.Close();
        //}
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

        //private void cmb_Account_Code_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cmb_Account_Code.Text.Length !=-1)
        //    {
        //        lblAccountCode.Text = cmb_Account_Code.Text.ToString();
        //    }
        //}       
    }
}
