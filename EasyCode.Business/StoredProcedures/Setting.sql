--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Settings_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Settings_Insert]

IF OBJECT_ID(N'[dbo].[Settings_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Settings_Update]

IF OBJECT_ID(N'[dbo].[Settings_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Settings_Delete]

IF OBJECT_ID(N'[dbo].[Settings_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Settings_Select]

--endregion

GO


--region [dbo].[Settings_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Settings_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@ID int = null,
	@SMTP nvarchar(250) = null,
	@EmailSender nvarchar(50) = null,
	@Password nvarchar(50) = null,
	@Port int = null,
	@EmailReceiver nvarchar(50) = null,
	@Domain nvarchar(250) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Settings]
					WHERE
					(
						(@ID is null OR [Settings].[ID] = @ID)
						AND (@SMTP is null OR [Settings].[SMTP] like @SMTP)
						AND (@EmailSender is null OR [Settings].[EmailSender] like @EmailSender)
						AND (@Password is null OR [Settings].[Password] like @Password)
						AND (@Port is null OR [Settings].[Port] = @Port)
						AND (@EmailReceiver is null OR [Settings].[EmailReceiver] like @EmailReceiver)
						AND (@Domain is null OR [Settings].[Domain] like @Domain)
					)
				)
if(@Page is null)
begin
	Set @Page = 1
end

if(@PageSize is null)
begin
	Set @PageSize = @RowCount
end

if(@SortBy is null)
begin
	Set @SortBy = 'DBNull'
end

if(@SortType is null)
begin
	Set @SortType = 1
end

SELECT *
FROM   (
			SELECT [dbo].[Settings].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'ID' and @SortType = 1 THEN [Settings].[ID] END ASC,
						CASE WHEN @SortBy = 'ID' and @SortType = 0 THEN [Settings].[ID] END DESC,
						CASE WHEN @SortBy = 'SMTP' and @SortType = 1 THEN [Settings].[SMTP] END ASC,
						CASE WHEN @SortBy = 'SMTP' and @SortType = 0 THEN [Settings].[SMTP] END DESC,
						CASE WHEN @SortBy = 'EmailSender' and @SortType = 1 THEN [Settings].[EmailSender] END ASC,
						CASE WHEN @SortBy = 'EmailSender' and @SortType = 0 THEN [Settings].[EmailSender] END DESC,
						CASE WHEN @SortBy = 'Password' and @SortType = 1 THEN [Settings].[Password] END ASC,
						CASE WHEN @SortBy = 'Password' and @SortType = 0 THEN [Settings].[Password] END DESC,
						CASE WHEN @SortBy = 'Port' and @SortType = 1 THEN [Settings].[Port] END ASC,
						CASE WHEN @SortBy = 'Port' and @SortType = 0 THEN [Settings].[Port] END DESC,
						CASE WHEN @SortBy = 'EmailReceiver' and @SortType = 1 THEN [Settings].[EmailReceiver] END ASC,
						CASE WHEN @SortBy = 'EmailReceiver' and @SortType = 0 THEN [Settings].[EmailReceiver] END DESC,
						CASE WHEN @SortBy = 'Domain' and @SortType = 1 THEN [Settings].[Domain] END ASC,
						CASE WHEN @SortBy = 'Domain' and @SortType = 0 THEN [Settings].[Domain] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Settings]
			Where 
			(
				(@ID is null OR [Settings].[ID] = @ID)
				AND (@SMTP is null OR [Settings].[SMTP] like @SMTP)
				AND (@EmailSender is null OR [Settings].[EmailSender] like @EmailSender)
				AND (@Password is null OR [Settings].[Password] like @Password)
				AND (@Port is null OR [Settings].[Port] = @Port)
				AND (@EmailReceiver is null OR [Settings].[EmailReceiver] like @EmailReceiver)
				AND (@Domain is null OR [Settings].[Domain] like @Domain)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Settings_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Settings_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Settings_Insert]
	@ID int OUTPUT,
	@SMTP nvarchar(250),
	@EmailSender nvarchar(50),
	@Password nvarchar(50),
	@Port int,
	@EmailReceiver nvarchar(50),
	@Domain nvarchar(250)

AS


INSERT INTO [dbo].[Settings] 
(
	[SMTP],
	[EmailSender],
	[Password],
	[Port],
	[EmailReceiver],
	[Domain]
)
VALUES 
(
	@SMTP,
	@EmailSender,
	@Password,
	@Port,
	@EmailReceiver,
	@Domain
)

SET @ID = SCOPE_IDENTITY()

--end [dbo].[Settings_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Settings_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Settings_Update]
	@ID int,
	@SMTP nvarchar(250),
	@EmailSender nvarchar(50),
	@Password nvarchar(50),
	@Port int,
	@EmailReceiver nvarchar(50),
	@Domain nvarchar(250)
AS


UPDATE [dbo].[Settings] SET
	[SMTP] = @SMTP,
	[EmailSender] = @EmailSender,
	[Password] = @Password,
	[Port] = @Port,
	[EmailReceiver] = @EmailReceiver,
	[Domain] = @Domain
WHERE
	[ID] = @ID

--end [dbo].[Settings_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Settings_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Settings_Delete]
	@ID int
AS


DELETE FROM [dbo].[Settings]
WHERE
(
	[ID] = @ID
)

--end [dbo].[Settings_Delete]
--endregion

GO
--=========================================================================================--

