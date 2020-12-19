

ALTER proc [dbo].[SP_insertAdmin]
(
 @FullName nvarchar(250), 
 @Phone nvarchar(20), 
 @UserName nvarchar(50), 
 @PassWord nvarchar(20), 
 @Email nvarchar(50), 
 @IsAdmin bit, 
 @DivisionID nvarchar(20)
)

as
declare @AdminID int

insert into admins( FullName, Phone, UserName, PassWord, Email, IsAdmin, DivisionID)
values(@FullName, @Phone, @UserName, @PassWord, @Email, @IsAdmin, @DivisionID)
SELECT @AdminID=@@IDENTITY


return @AdminID
GO


