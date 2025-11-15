use ezeepdf

GO

CREATE TABLE LuSettings(
	SettingsId INT PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	[Value] VARCHAR(50) NOT NULL,
	[Description] VARCHAR(100) NULL)

CREATE TABLE LuSubscriptionType(
	SubscriptionTypeId INT PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	[FeeAmount] MONEY NOT NULL,
	[Description] VARCHAR(100) NULL,
	Active BIT NOT NULL DEFAULT 1)

CREATE TABLE LuUserType(
	UserTypeId INT PRIMARY KEY,
	[Name] VARCHAR(30) NOT NULL,
	[Description] VARCHAR(100) NULL)

CREATE TABLE LuPdfFunction(
	PdfFunctionId INT PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	[Description] VARCHAR(100) NULL,
	Active BIT NOT NULL DEFAULT 1)

CREATE TABLE LuTransactionStatus(
	TransactionStatusId INT PRIMARY KEY,
	[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE LuTransactionMode(
	TransactionModeId INT PRIMARY KEY,
	[Name] VARCHAR(30) NOT NULL)

CREATE TABLE LuProduct(
	ProductId INT PRIMARY KEY,
	[Name] VARCHAR(50),
	[Description] VARCHAR(100))

GO