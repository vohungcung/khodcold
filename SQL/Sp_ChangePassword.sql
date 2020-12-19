create proc SP_ChangePassword
(
@AdminID int,
@Password nvarchar(50)
)
as
update
admins
set 
PassWord=@Password
where AdminID=@AdminID