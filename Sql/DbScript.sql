use ezeepdf

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

Create TABLE Users (
	UserId INT IDENTITY(1,1) PRIMARY KEY,
	UserTypeId INT NOT NULL FOREIGN KEY REFERENCES LuUserType(UserTypeId),
	FirstName VARCHAR(100) NOT NULL,
	LastName VARCHAR(100) NOT NULL,
	EmailAddress VARCHAR(255) NOT NULL,
	[Password] VARCHAR(50) NOT NULL,
	Locked BIT DEFAULT 0,
	LockedAt DATETIME2 NULL,
	LastPasswordChangeTime DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
	FailedLoginAttemptCount INT NOT NULL DEFAULT 0,
	ForcePasswordChange BIT NOT NULL DEFAULT 0)


CREATE TABLE Transactions(
	TransactionId INT IDENTITY(1,1) PRIMARY KEY,
	TransactionStatusId INT NOT NULL FOREIGN KEY REFERENCES LuTransactionStatus(TransactionStatusId),
	TransactionModeId INT NOT NULL FOREIGN KEY REFERENCES LuTransactionMode(TransactionModeId),
	UserId INT NOT NULL FOREIGN KEY REFERENCES Users(UserId),
	ProductId INT NOT NULL FOREIGn KEY REFERENCES LuProduct (ProductId),
	SubscriptionTypeId INT NOT NULL FOREIGN KEY REFERENCES LuSubscriptionType(SubscriptionTypeId),
	AmountPaid MONEY NOT NULL,
	StartTime DATETIME2 NOT NULL,
	EndTime DATETIME2 NULL,
	IpAddress VARCHAR(25) NOT NULL,
	BankReferenceId VARCHAR(100) NULL,
	Id2 VARCHAR(100) NULL,
	Id3 VARCHAR(100) NULL,
	BankMessage VARCHAR(1000) NULL)

CREATE TABLE UserSubscription(
	UserSubscriptionId INT IDENTITY(1,1) PRIMARY KEY,
	TransactionId INT NOT NULL FOREIGN KEY REFERENCES Transactions(TransactionId),
	SourceDevice VARCHAR(100) NOT NULL,
	StartDate DATETIME2 NOT NULL,
	EndDate DATETIME2 NOT NULL,
	Active BIT NOT NULL)

CREATE TABLE UserPdfUsage(
	UserPdfUsageId INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT NULL FOREIGN KEY REFERENCES Users(UserId),
	PdfFunctionId INT NOT NULL FOREIGN KEY REFERENCES LuPdfFunction(PdfFunctionId),
	UserSubscriptionId INT NULL FOREIGN KEY REFERENCES UserSubscription(UserSubscriptionId),
	UsageDate DATETIME2 NOT NULL,
	PdfSize float NOT NULL,
	IpAddress VARCHAR(25) NOT NULL,	
	SourceDevice VARCHAR(100) NOT NULL)

----REMOVE
--DROP TABLE UserPdfUsage
--DROP TABLE UserSubscription
--DROP TABLE Transactions
--DROP TABLE Users
--DROP TABLE LuProduct
--DROP TABLE LuSettings
--DROP TABLE LuSubscriptionType
--DROP TABLE LuUserType
--DROP TABLE LuPdfFunction
--DROP TABLE LuTransactionStatus
--DROP TABLE LuTransactionMode

--------------------
-------------STATIC DATA--------------

--User Types
INSERT INTO LuUserType(UserTypeId, [Name], [Description])
VALUES (1, 'Admin', 'Administrator'), 
	   (2, 'Support', 'Supprt'), 
	   (3, 'Public', 'End users that use this application for PDF editing')

--Product
INSERT INTO LuProduct (ProductId, [Name], [Description])
VALUES (1, 'Ezee Pdf', 'Allows to to manipulate PDF files'),
	   (2, 'Ezee Pic', 'Allows image editing')

--Subscription Types
INSERT INTO LuSubscriptionType(SubscriptionTypeId, [Name], [FeeAmount], [Description], Active)
VALUES (1, 'Yearly', 2999, 'Fee paid yearly', 1),
	   (2, 'Monthly', 299, 'Fee paid monthly', 1),
	   (3, 'Daily', 99, 'Fee paid daily', 1)

--PDF Functions
INSERT INTO LuPdfFunction(PdfFunctionId, [Name], [Description], [Active])
VALUES (1, 'Watermark', 'Watermark', 1),
	   (2, 'Format', 'Change PDF format', 1),
	   (3, 'From Image', 'Create PDF from an image', 1),
	   (4, 'Editing', 'Includes annotations, form fields and comments', 1),
	   (5, 'Merge', 'Merge multiple PDFs', 0),
	   (6, 'Split', 'Split PDF into page ranges', 0)

--Transaction Status
INSERT INTO LuTransactionStatus(TransactionStatusId, [Name])
VALUES (1, 'Pending'), 
	   (2, 'Pass'), 
	   (3, 'Fail') 

--Transaction Mode
INSERT INTO LuTransactionMode(TransactionModeId, [Name])
VALUES (1, 'Payement Gateway'), 
	   (2, 'Coupon')

--Settings
INSERT INTO LuSettings(SettingsId, [Name], [Value], [Description])
VALUES (1, 'PublicPasswordExpiryInDays', '90', 'Number of days after which public users must change password'),
	   (2, 'StaffPasswordExpiryInDays', '150', 'Number of days after which staff users must change password'),
	   (3, 'LockAfterFailLoginAttempts', '3', 'Lock user after failing these many times'),
	   (4, 'FreeVersionAllowedPageCount', '2', 'Limit pdf to these many pages in free version'),
	   (5, 'FreeVersionSaveCount', '1', 'How many times user can download updated file in free version'),
	   (6, 'FreeVersionLockHours', '12', 'Numbers of hours to lock free functionality after reaching save limit'),
	   (7, 'MaxPages', '100', 'Maximum number of pages in file'),
	   (8, 'MaxUploadPerDay', '50', 'Maximum files that can be processed for each user in a day')
