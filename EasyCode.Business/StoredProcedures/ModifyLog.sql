--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[ModifyLogs_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[ModifyLogs_Insert]

IF OBJECT_ID(N'[dbo].[ModifyLogs_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[ModifyLogs_Update]

IF OBJECT_ID(N'[dbo].[ModifyLogs_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[ModifyLogs_Delete]

IF OBJECT_ID(N'[dbo].[ModifyLogs_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[ModifyLogs_Select]

--endregion

GO


--region [dbo].[ModifyLogs_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[ModifyLogs_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@ModifyDate datetime = null,
	@ModifyUserID int = null,
	@CustomerID nvarchar(50) = null,
	@OrderID nvarchar(50) = null,
	@Note nvarchar(50) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[ModifyLogs]
					WHERE
					(
						(@ModifyDate is null OR [ModifyLogs].[ModifyDate] = @ModifyDate)
						AND (@ModifyUserID is null OR [ModifyLogs].[ModifyUserID] = @ModifyUserID)
						AND (@CustomerID is null OR [ModifyLogs].[CustomerID] like @CustomerID)
						AND (@OrderID is null OR [ModifyLogs].[OrderID] like @OrderID)
						AND (@Note is null OR [ModifyLogs].[Note] like @Note)
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
			SELECT [dbo].[ModifyLogs].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'ModifyDate' and @SortType = 1 THEN [ModifyLogs].[ModifyDate] END ASC,
						CASE WHEN @SortBy = 'ModifyDate' and @SortType = 0 THEN [ModifyLogs].[ModifyDate] END DESC,
						CASE WHEN @SortBy = 'ModifyUserID' and @SortType = 1 THEN [ModifyLogs].[ModifyUserID] END ASC,
						CASE WHEN @SortBy = 'ModifyUserID' and @SortType = 0 THEN [ModifyLogs].[ModifyUserID] END DESC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 1 THEN [ModifyLogs].[CustomerID] END ASC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 0 THEN [ModifyLogs].[CustomerID] END DESC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 1 THEN [ModifyLogs].[OrderID] END ASC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 0 THEN [ModifyLogs].[OrderID] END DESC,
						CASE WHEN @SortBy = 'Note' and @SortType = 1 THEN [ModifyLogs].[Note] END ASC,
						CASE WHEN @SortBy = 'Note' and @SortType = 0 THEN [ModifyLogs].[Note] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[ModifyLogs]
			Where 
			(
				(@ModifyDate is null OR [ModifyLogs].[ModifyDate] = @ModifyDate)
				AND (@ModifyUserID is null OR [ModifyLogs].[ModifyUserID] = @ModifyUserID)
				AND (@CustomerID is null OR [ModifyLogs].[CustomerID] like @CustomerID)
				AND (@OrderID is null OR [ModifyLogs].[OrderID] like @OrderID)
				AND (@Note is null OR [ModifyLogs].[Note] like @Note)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[ModifyLogs_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[ModifyLogs_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[ModifyLogs_Insert]
	@ModifyDate datetime,
	@ModifyUserID int,
	@CustomerID nvarchar(50),
	@OrderID nvarchar(50),
	@Note nvarchar(50)
AS


INSERT INTO [dbo].[ModifyLogs] (
	[ModifyDate],
	[ModifyUserID],
	[CustomerID],
	[OrderID],
	[Note]
) VALUES (
	@ModifyDate,
	@ModifyUserID,
	@CustomerID,
	@OrderID,
	@Note
)

--end [dbo].[ModifyLogs_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[ModifyLogs_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[ModifyLogs_Update]
	@ModifyDate datetime,
	@ModifyUserID int,
	@CustomerID nvarchar(50),
	@OrderID nvarchar(50),
	@Note nvarchar(50)
AS


UPDATE [dbo].[ModifyLogs] SET
	[Note] = @Note
WHERE
	[ModifyDate] = @ModifyDate
	AND [ModifyUserID] = @ModifyUserID
	AND [CustomerID] = @CustomerID
	AND [OrderID] = @OrderID

--end [dbo].[ModifyLogs_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[ModifyLogs_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[ModifyLogs_Delete]
	@ModifyDate datetime,
	@ModifyUserID int,
	@CustomerID nvarchar(50),
	@OrderID nvarchar(50)
AS


DELETE FROM [dbo].[ModifyLogs]
WHERE
(
	[ModifyDate] = @ModifyDate
	AND [ModifyUserID] = @ModifyUserID
	AND [CustomerID] = @CustomerID
	AND [OrderID] = @OrderID
)

--end [dbo].[ModifyLogs_Delete]
--endregion

GO
--=========================================================================================--

