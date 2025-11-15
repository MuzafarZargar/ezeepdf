USE ezeepdf

GO

CREATE OR ALTER PROCEDURE [dbo].[usp_SaveError] (  
 @URL varchar(2048),  
 @ServerName varchar(255),  
 @RemoteAddress varchar(255),  
 @ErrorMessage varchar(1000),  
 @FullMessage varchar(MAX),
 @UserId INT=NULL)  
AS   
  
INSERT INTO ErrorLog (
	DateCreated, 
	ErrorMessage, 
	[URL], 
	[Server], 
	IpAddress, 
	FullMessage,
	UserId)
	
VALUES (
	getutcdate(), 
	@ErrorMessage, 
	@URL, 
	@ServerName, 
	@RemoteAddress, 
	@FullMessage,
	@UserId)  
  
  