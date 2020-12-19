--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[ApproveLogs_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[ApproveLogs_Insert]

IF OBJECT_ID(N'[dbo].[ApproveLogs_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[ApproveLogs_Update]

IF OBJECT_ID(N'[dbo].[ApproveLogs_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[ApproveLogs_Delete]

IF OBJECT_ID(N'[dbo].[ApproveLogs_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[ApproveLogs_Select]

--endregion

GO


--region [dbo].[ApproveLogs_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[ApproveLogs_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@CreateDate datetime = null,
	@CreateBy int = null,
	@OrderID nvarchar(50) = null,
	@Note nvarchar(50) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[ApproveLogs]
					WHERE
					(
						(@CreateDate is null OR [ApproveLogs].[CreateDate] = @CreateDate)
						AND (@CreateBy is null OR [ApproveLogs].[CreateBy] = @CreateBy)
						AND (@OrderID is null OR [ApproveLogs].[OrderID] like @OrderID)
						AND (@Note is null OR [ApproveLogs].[Note] like @Note)
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
			SELECT [dbo].[ApproveLogs].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'CreateDate' and @SortType = 1 THEN [ApproveLogs].[CreateDate] END ASC,
						CASE WHEN @SortBy = 'CreateDate' and @SortType = 0 THEN [ApproveLogs].[CreateDate] END DESC,
						CASE WHEN @SortBy = 'CreateBy' and @SortType = 1 THEN [ApproveLogs].[CreateBy] END ASC,
						CASE WHEN @SortBy = 'CreateBy' and @SortType = 0 THEN [ApproveLogs].[CreateBy] END DESC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 1 THEN [ApproveLogs].[OrderID] END ASC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 0 THEN [ApproveLogs].[OrderID] END DESC,
						CASE WHEN @SortBy = 'Note' and @SortType = 1 THEN [ApproveLogs].[Note] END ASC,
						CASE WHEN @SortBy = 'Note' and @SortType = 0 THEN [ApproveLogs].[Note] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[ApproveLogs]
			Where 
			(
				(@CreateDate is null OR [ApproveLogs].[CreateDate] = @CreateDate)
				AND (@CreateBy is null OR [ApproveLogs].[CreateBy] = @CreateBy)
				AND (@OrderID is null OR [ApproveLogs].[OrderID] like @OrderID)
				AND (@Note is null OR [ApproveLogs].[Note] like @Note)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[ApproveLogs_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[ApproveLogs_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[ApproveLogs_Insert]
	@CreateDate datetime,
	@CreateBy int,
	@OrderID nvarchar(50),
	@Note nvarchar(50)
AS


INSERT INTO [dbo].[ApproveLogs] (
	[CreateDate],
	[CreateBy],
	[OrderID],
	[Note]
) VALUES (
	@CreateDate,
	@CreateBy,
	@OrderID,
	@Note
)

--end [dbo].[ApproveLogs_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[ApproveLogs_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[ApproveLogs_Update]
	@CreateDate datetime,
	@CreateBy int,
	@OrderID nvarchar(50),
	@Note nvarchar(50)
AS


UPDATE [dbo].[ApproveLogs] SET
	[Note] = @Note
WHERE
	[CreateDate] = @CreateDate
	AND [CreateBy] = @CreateBy
	AND [OrderID] = @OrderID

--end [dbo].[ApproveLogs_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[ApproveLogs_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[ApproveLogs_Delete]
	@CreateDate datetime,
	@CreateBy int,
	@OrderID nvarchar(50)
AS


DELETE FROM [dbo].[ApproveLogs]
WHERE
(
	[CreateDate] = @CreateDate
	AND [CreateBy] = @CreateBy
	AND [OrderID] = @OrderID
)

--end [dbo].[ApproveLogs_Delete]
--endregion

GO
--=========================================================================================--

