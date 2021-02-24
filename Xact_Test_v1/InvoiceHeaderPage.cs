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
    public partial class InvoiceHeaderPage : Form
    {
        string conString = @"Data Source=DESKTOP-PJOL8GM\SQLEXPRESS;Initial Catalog=XactTestdb;Integrated Security=True;Pooling=False";

        SqlConnection conn;     //DECLARE SQL CONNECTION OBJECT
        SqlCommand command;     //DECLARE SQL COMMAND OBJECT        
        SqlDataAdapter adapter; //DECLARE SQL ADAPTER OBJECT
        DataTable dt;           //DECLARE SQL DATATABLE OBJECT
        SqlDataReader reader;   //DECLARE SQL DATAREADER OBJECT
        string query;           //DECLARE SQL QUERY OBJECT
        public InvoiceHeaderPage()
        {
            InitializeComponent();            
        }

        private void InvoiceHeaderPage_Load(object sender, EventArgs e)
        {
            txtDate.Text = DateTime.Now.ToString();
            populateAccountCode();
            populateStockCode();
            populateInvoiceNo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            AddToCart();
            AddToDB();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        public void populateAccountCode()//LOAD ACCOUNT CODE
        {
            dt = new DataTable();
            conn = new SqlConnection(conString);
            conn.Open();
            query = "SELECT * FROM tlDebtor";
            command = new SqlCommand(query, conn);
            adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            cmb_Account_Code.ValueMember = "Account_code";
            cmb_Account_Code.DisplayMember = "Account_code";
            cmb_Account_Code.DataSource = dt;            
            conn.Close();
        }
        public void populateStockCode() //POPULATE STOCK CODE
        {
            dt = new DataTable();
            conn = new SqlConnection(conString);
            conn.Open();
            query = "SELECT * FROM tlStock";
            command = new SqlCommand(query, conn);
            adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            cmb_Stock_Code.ValueMember = "Stock_code";
            cmb_Stock_Code.DisplayMember = "Stock_code";
            cmb_Stock_Code.DataSource = dt;
            conn.Close();
        }

        public void populateInvoiceNo() //POPULATE INVOICE NUMBER
        {
            dt = new DataTable();
            conn = new SqlConnection(conString);
            conn.Open();
            query = "SELECT * FROM tlInvoiceHeader";
            command = new SqlCommand(query, conn);
            adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            cmbInvNo.ValueMember = "Invoice_no";
            cmbInvNo.DisplayMember = "Invoice_no";
            cmbInvNo.DataSource = dt;
            conn.Close();
        }

        public void checkValues() //FIRST CHECK IF FIELDS ARE NOT EMPTY
        {            
            if (lblItem_name.Text == "")
            {
                MessageBox.Show("Item name cannot be empty");
            }
            else if (lblIUnitCost.Text == "")
            {
                MessageBox.Show("unit price cannot be empty");
            }
            else if (cbmQty.Text == "")
            {
                MessageBox.Show("Please input Qty");
            }
            else if (lblVat_Value.Text == "")
            {
                MessageBox.Show("Vat value cannot be empty");
            }
            else if (lblSub.Text == "")
            {
                MessageBox.Show("Sub-total cannot be empty");
            }
            else if (lblTotal.Text == "")
            {
                MessageBox.Show("Total cannot be empty");
            }
        }      

        public void AddToCart() //ADD TO CART
        {
            if(cbmQty.SelectedIndex==-1)
            {
                MessageBox.Show("Please select Qty");
                cbmQty.Focus();
            }
            else
            {
                try
                {
                    string[] array = new string[9];
                    array[0] = lblItem_name.Text;
                    array[1] = lblIUnitCost.Text;
                    array[2] = cbmQty.Text;
                    array[3] = cmb_discount.Text;
                    array[4] = cmb_Account_Code.SelectedValue.ToString();
                    array[5] = cmb_Stock_Code.SelectedValue.ToString();
                    array[6] = lblVat_Value.Text;
                    array[7] = lblSub.Text;
                    array[8] = lblTotal.Text;
                    ListViewItem list = new ListViewItem(array);
                    listView1.Items.Add(list);
                    lbl_Cross_Total.Text = (Int32.Parse(lblSub.Text) + Int32.Parse(lbl_Cross_Total.Text)).ToString();
                    //ClearField();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot add to cart because:" + ex.Message);
                }
            }            
            
        }

        public void AddToDB() // ADD RECORDS TO DATABASE
        {
            try
            {
                conn = new SqlConnection(conString);
                //ADD TO tlInvoiceHeader
                query = "INSERT INTO tlInvoiceHeader VALUES ( '" + cmb_Account_Code.Text + "', '" + DateTime.Parse(txtDate.Text) + "', '" + float.Parse(lblSub.Text) + "' , '" + float.Parse(lblVat_Value.Text) + "','" + float.Parse(lblTotal.Text) + "')";
                command = new SqlCommand(query, conn);
                 
                //ADD TO tlInvoiceDetail
                string query2 = "INSERT INTO tlInvoiceDetail VALUES('" + cmbInvNo.Text + "','" + cmb_Stock_Code.Text + "','" + cbmQty.Text + "','" + float.Parse(lblIUnitCost.Text) + "','" + float.Parse(lblTotal.Text) + "','" + cmb_discount.Text + "','" + float.Parse(lblSub.Text) + "')";
                SqlCommand command2 = new SqlCommand(query2, conn);

                //ADD TO tlDebtorTransactionFile
                string transactionType = "Purchase";
                string query3 = "INSERT INTO tlDebtorTransactionFile VALUES ( '" + cmb_Account_Code.Text + "', '" + DateTime.Parse(txtDate.Text) + "', '" + transactionType + "' , '" + float.Parse(lblSub.Text) + "','" + float.Parse(lblVat_Value.Text) + "')";
                SqlCommand cm = new SqlCommand(query, conn);

                string query4 = "UPDATE tlDebtor SET Balance = Balance + @bal, Sales_year_to_date = Sales_year_to_date + @sale, Cost_year_to_date=Cost_year_to_date+@cost WHERE Account_code=@acc";
                SqlCommand cm2 = new SqlCommand(query4, conn);
                cm2.CommandType = CommandType.Text;
                cm2.Parameters.AddWithValue("@bal", float.Parse(lblSub.Text));
                cm2.Parameters.AddWithValue("@cost", float.Parse(lblIUnitCost.Text));
                cm2.Parameters.AddWithValue("@sale",float.Parse(lblSub.Text));
                cm2.Parameters.AddWithValue("@acc", cmb_Account_Code.Text);


                string query5 = "UPDATE tlStock SET Qty_sold = Qty_sold + '"+ Int32.Parse(cbmQty.Text)+"', Total_sales_excl_vat = Total_sales_excl_vat+ '"+float.Parse(lblTotal.Text)+"', Stock_on_hand = Stock_on_hand - '"+Int32.Parse(cbmQty.Text)+"' WHERE Stock_code='"+cmb_Stock_Code.Text+"'";
                SqlCommand cm3 = new SqlCommand(query5, conn);                

                conn.Open();
                command.ExecuteNonQuery();
                cm.ExecuteNonQuery();
                cm2.ExecuteNonQuery();
                cm3.ExecuteNonQuery();
                command2.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Successfully added to database");
                //ShowData();
                ClearField();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot add because: " + ex.Message);
            }

        }      

        public void ClearField()
        {            
            cbmQty.SelectedIndex = -1;
            cmb_discount.SelectedIndex = -1;
            lblTotal.Text = "";
            lblVat_Value.Text = "";
            lblSub.Text = "";
            //lbl_Cross_Total.Text = "";
        }

        private void cmb_Stock_Code_SelectedIndexChanged(object sender, EventArgs e) //GET VALUES TO BE POPULATED IN LABEL ON COMBOBOX (STOCK_CODE) CLICK
        {            
            query = "SELECT * FROM tlStock WHERE Stock_code='" + cmb_Stock_Code.Text + "'";
            conn = new SqlConnection(conString);            
            command = new SqlCommand(query, conn);
            adapter = new SqlDataAdapter(command);
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string Stock_description = reader["Stock_description"].ToString();
                    string price = reader["Selling_price"].ToString();
                    lblItem_name.Text = Stock_description;
                    lblIUnitCost.Text = price;
                }               
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }       
        }

        private void cbmQty_SelectedIndexChanged(object sender, EventArgs e) //COMBOBOX SELECT TRIGGER FOR QTY
        {
            if (cbmQty.Text.Length > 0)
            {
                double vat = 0.15;
                lblTotal.Text = (Int32.Parse(lblIUnitCost.Text) * Int32.Parse(cbmQty.Text)).ToString();
                lblVat_Value.Text  = ((Int32.Parse(lblTotal.Text) * vat).ToString());
            }
            cmb_discount.Focus();            
        }

        private void cmb_discount_SelectedIndexChanged(object sender, EventArgs e) //COMBOBOX SELECT TRIGGER FOR DISCOUNT
        {
            if (cmb_discount.Text.Length > 0)
            {
                lblSub.Text = (Int32.Parse(lblTotal.Text) - Int32.Parse(cmb_discount.Text)).ToString();
            }
        }

        private void txt_Paid_Amount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbl_Cross_Total.Text.Length > 0)
                {
                    txtChance.Text = (Int32.Parse(txt_Paid_Amount.Text) - Int32.Parse(lbl_Cross_Total.Text)).ToString();
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                txtChance.Text = "";

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