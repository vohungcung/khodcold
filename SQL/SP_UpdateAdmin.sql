alter proc SP_UpdateAdmin
(
@AdminID int,
@FullName nvarchar(250),
@Phone nvarchar(20),
@IsAdmin bit,
@DivisionID nvarchar(20)
)
as
update Admins
set 
FullName=@FullName,
IsAdmin=@IsAdmin,
DivisionID=@DivisionID,
Phone=@Phone
where AdminID=@AdminID
