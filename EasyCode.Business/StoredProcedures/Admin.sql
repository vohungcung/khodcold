--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Admins_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Admins_Insert]

IF OBJECT_ID(N'[dbo].[Admins_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Admins_Update]

IF OBJECT_ID(N'[dbo].[Admins_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Admins_Delete]

IF OBJECT_ID(N'[dbo].[Admins_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Admins_Select]

--endregion

GO


--region [dbo].[Admins_Select]

-- Create By: Hung Cung
-- Date Generated: Tuesday, April 24, 2018

CREATE PROCEDURE [dbo].[Admins_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@AdminID int = null,
	@FullName nvarchar(250) = null,
	@Phone nvarchar(50) = null,
	@UserName nvarchar(100) = null,
	@PassWord nvarchar(1000) = null,
	@Email nvarchar(255) = null,
	@IsAdmin bit = null,
	@ZIPCode nvarchar(50) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Admins]
					WHERE
					(
						(@AdminID is null OR [Admins].[AdminID] = @AdminID)
						AND (@FullName is null OR [Admins].[FullName] like @FullName)
						AND (@Phone is null OR [Admins].[Phone] like @Phone)
						AND (@UserName is null OR [Admins].[UserName] like @UserName)
						AND (@PassWord is null OR [Admins].[PassWord] like @PassWord)
						AND (@Email is null OR [Admins].[Email] like @Email)
						AND (@IsAdmin is null OR [Admins].[IsAdmin] = @IsAdmin)
						AND (@ZIPCode is null OR [Admins].[ZIPCode] like @ZIPCode)
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
			SELECT [dbo].[Admins].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'AdminID' and @SortType = 1 THEN [Admins].[AdminID] END ASC,
						CASE WHEN @SortBy = 'AdminID' and @SortType = 0 THEN [Admins].[AdminID] END DESC,
						CASE WHEN @SortBy = 'FullName' and @SortType = 1 THEN [Admins].[FullName] END ASC,
						CASE WHEN @SortBy = 'FullName' and @SortType = 0 THEN [Admins].[FullName] END DESC,
						CASE WHEN @SortBy = 'Phone' and @SortType = 1 THEN [Admins].[Phone] END ASC,
						CASE WHEN @SortBy = 'Phone' and @SortType = 0 THEN [Admins].[Phone] END DESC,
						CASE WHEN @SortBy = 'UserName' and @SortType = 1 THEN [Admins].[UserName] END ASC,
						CASE WHEN @SortBy = 'UserName' and @SortType = 0 THEN [Admins].[UserName] END DESC,
						CASE WHEN @SortBy = 'PassWord' and @SortType = 1 THEN [Admins].[PassWord] END ASC,
						CASE WHEN @SortBy = 'PassWord' and @SortType = 0 THEN [Admins].[PassWord] END DESC,
						CASE WHEN @SortBy = 'Email' and @SortType = 1 THEN [Admins].[Email] END ASC,
						CASE WHEN @SortBy = 'Email' and @SortType = 0 THEN [Admins].[Email] END DESC,
						CASE WHEN @SortBy = 'IsAdmin' and @SortType = 1 THEN [Admins].[IsAdmin] END ASC,
						CASE WHEN @SortBy = 'IsAdmin' and @SortType = 0 THEN [Admins].[IsAdmin] END DESC,
						CASE WHEN @SortBy = 'ZIPCode' and @SortType = 1 THEN [Admins].[ZIPCode] END ASC,
						CASE WHEN @SortBy = 'ZIPCode' and @SortType = 0 THEN [Admins].[ZIPCode] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Admins]
			Where 
			(
				(@AdminID is null OR [Admins].[AdminID] = @AdminID)
				AND (@FullName is null OR [Admins].[FullName] like @FullName)
				AND (@Phone is null OR [Admins].[Phone] like @Phone)
				AND (@UserName is null OR [Admins].[UserName] like @UserName)
				AND (@PassWord is null OR [Admins].[PassWord] like @PassWord)
				AND (@Email is null OR [Admins].[Email] like @Email)
				AND (@IsAdmin is null OR [Admins].[IsAdmin] = @IsAdmin)
				AND (@ZIPCode is null OR [Admins].[ZIPCode] like @ZIPCode)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Admins_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Admins_Insert]

-- Create By: Hung Cung
-- Date Generated: Tuesday, April 24, 2018

CREATE PROCEDURE [dbo].[Admins_Insert]
	@AdminID int OUTPUT,
	@FullName nvarchar(250),
	@Phone nvarchar(50),
	@UserName nvarchar(100),
	@PassWord nvarchar(1000),
	@Email nvarchar(255),
	@IsAdmin bit,
	@ZIPCode nvarchar(50)

AS


INSERT INTO [dbo].[Admins] 
(
	[FullName],
	[Phone],
	[UserName],
	[PassWord],
	[Email],
	[IsAdmin],
	[ZIPCode]
)
VALUES 
(
	@FullName,
	@Phone,
	@UserName,
	@PassWord,
	@Email,
	@IsAdmin,
	@ZIPCode
)

SET @AdminID = SCOPE_IDENTITY()

--end [dbo].[Admins_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Admins_Update]

-- Create By: Hung Cung
-- Date Generated: Tuesday, April 24, 2018

CREATE PROCEDURE [dbo].[Admins_Update]
	@AdminID int,
	@FullName nvarchar(250),
	@Phone nvarchar(50),
	@UserName nvarchar(100),
	@PassWord nvarchar(1000),
	@Email nvarchar(255),
	@IsAdmin bit,
	@ZIPCode nvarchar(50)
AS


UPDATE [dbo].[Admins] SET
	[FullName] = @FullName,
	[Phone] = @Phone,
	[UserName] = @UserName,
	[PassWord] = @PassWord,
	[Email] = @Email,
	[IsAdmin] = @IsAdmin,
	[ZIPCode] = @ZIPCode
WHERE
	[AdminID] = @AdminID

--end [dbo].[Admins_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Admins_Delete]

-- Create By: Hung Cung
-- Date Generated: Tuesday, April 24, 2018

CREATE PROCEDURE [dbo].[Admins_Delete]
	@AdminID int
AS


DELETE FROM [dbo].[Admins]
WHERE
(
	[AdminID] = @AdminID
)

--end [dbo].[Admins_Delete]
--endregion

GO
--=========================================================================================--

