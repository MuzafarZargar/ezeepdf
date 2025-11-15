USE ezeepdf

--User Types
INSERT INTO LuUserType(UserTypeId, [Name], [Description])
VALUES (1, 'Admin', 'Administrator'), 
	   (2, 'Support', 'Supprt'), 
	   (3, 'Public', 'End users that use this application for PDF editing')

--Product
INSERT INTO LuProduct (ProductId, [Name], [Description])
VALUES (1, 'Ezee Pdf', 'Allows to to manipulate PDF files'),
	   (2, 'Ezee Pic', 'Allows image editing'),
	   (3, 'Ezee Pdf + Ezee Pic', 'Includes both products for purchase')

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
	   (6, 'Split', 'Split PDF into page ranges', 0),
	   (7, 'HtmltoPdf', 'Converts HTML into Pdf format', 0)

--Transaction Status
INSERT INTO LuTransactionStatus(TransactionStatusId, [Name])
VALUES (1, 'Pending'), 
	   (2, 'Pass'), 
	   (3, 'Fail') 

--Transaction Mode
INSERT INTO LuTransactionMode(TransactionModeId, [Name])
VALUES (1, 'Payment Gateway'), 
	   (2, 'Coupon')

--Settings
INSERT INTO LuSettings(SettingsId, [Name], [Value], [Description])
VALUES (1, 'PublicPasswordExpiryInDays', '90', 'Number of days after which public users must change password'),
	   (2, 'StaffPasswordExpiryInDays', '150', 'Number of days after which staff users must change password'),
	   (3, 'LockAfterFailLoginAttempts', '3', 'Lock user after failing these many times'),
	   (4, 'FreeVersionAllowedPageCount', '3', 'Limit pdf to these many pages in free version'),
	   (5, 'FreeVersionSaveCount', '1', 'How many times user can download updated file in free version'),
	   (6, 'FreeVersionLockHours', '12', 'Numbers of hours to lock free functionality after reaching save limit'),
	   (7, 'MaxPages', '100', 'Maximum number of pages in file'),
	   (8, 'MaxUploadPerDay', '50', 'Maximum files that can be processed for each user in a day'),
	   (9, 'WatermarkMaxImageSizeKB', '50', 'Maximum size (KB) of watermark image that can be uploaded '),
	   (10, 'MaxPdfSizeMB', '2', 'Maximum size (MB) of pdf that can be uploaded '),
	   (11, 'MaxPdfSizeMBPerDay', '100', 'Maximum size (MB) of all pdfs that can be uploaded/downloaded in a day'),
	   (12, 'ConsecutiveDownloadWait', '5', 'Number of minutes to wait before using a feature again')

	   
