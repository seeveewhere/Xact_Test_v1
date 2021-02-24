Create database XACTestDB
Go
USE XACTestDB
GO

CREATE TABLE tlDebtor
(
	Account_code varchar(150) primary key not null,
	Address1 varchar(150),	
	Balance float,
	Sales_year_to_date float,
	Cost_year_to_date float
)

create table tlDebtorTransactionFile
(
	DocumentNo int primary key identity(100,1) not null,
	Account_code varchar(150) foreign key references tlDebtor(Account_code),
	[Date] datetime,
	Transaction_type varchar(50),
	Gross_transaction_value float,
	Vat_value float
)

create table tlStock
(
	Stock_code varchar(150) primary key,
	Stock_description varchar(150),
	Cost float,
	Selling_price float,
	Total_purchases_excl_vat float,
	Total_sales_excl_vat int,
	Qty_purchased int,
	Qty_sold int,
	Stock_on_hand int
)

create table tlStockTransactionFile
(
	DocumentNo int primary key identity(100,1) not null,
	Stock_code varchar(150) foreign key references tlStock(Stock_code),
	[Date] datetime,
	Transaction_type varchar(100),
	Qty int,
	Unit_cost float,
	Unit_sell float
)

create table tlInvoiceHeader
(
	Invoice_no int primary key identity(100,1) not null,
	Account_code varchar(150) foreign key references tlDebtor(Account_code),
	[Date] datetime,
	Total_sell_amount_excl_vat float,
	Vat float,
	Total_cost float
)

create table tlInvoiceDetail
(
	Item_no int primary key identity(100,1) not null,
	Invoice_no int foreign key references tlInvoiceHeader(Invoice_no),
	Stock_code varchar(150) foreign key references tlStock(Stock_code),
	Qty_Sold int,
	Unit_Cost float,
	Unit_Sell float,
	Discount float,
	Total float
)