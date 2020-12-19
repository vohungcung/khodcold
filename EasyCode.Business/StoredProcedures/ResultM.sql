--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[ResultMs_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[ResultMs_Insert]

IF OBJECT_ID(N'[dbo].[ResultMs_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[ResultMs_Update]

IF OBJECT_ID(N'[dbo].[ResultMs_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[ResultMs_Delete]

IF OBJECT_ID(N'[dbo].[ResultMs_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[ResultMs_Select]

--endregion

GO


--region [dbo].[ResultMs_Select]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[ResultMs_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@ResultID nvarchar(50) = null,
	@CreateDate datetime = null,
	@EmployeeID nvarchar(50) = null,
	@TopicID int = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[ResultMs]
					WHERE
					(
						(@ResultID is null OR [ResultMs].[ResultID] like @ResultID)
						AND (@CreateDate is null OR [ResultMs].[CreateDate] = @CreateDate)
						AND (@EmployeeID is null OR [ResultMs].[EmployeeID] like @EmployeeID)
						AND (@TopicID is null OR [ResultMs].[TopicID] = @TopicID)
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
			SELECT [dbo].[ResultMs].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'ResultID' and @SortType = 1 THEN [ResultMs].[ResultID] END ASC,
						CASE WHEN @SortBy = 'ResultID' and @SortType = 0 THEN [ResultMs].[ResultID] END DESC,
						CASE WHEN @SortBy = 'CreateDate' and @SortType = 1 THEN [ResultMs].[CreateDate] END ASC,
						CASE WHEN @SortBy = 'CreateDate' and @SortType = 0 THEN [ResultMs].[CreateDate] END DESC,
						CASE WHEN @SortBy = 'EmployeeID' and @SortType = 1 THEN [ResultMs].[EmployeeID] END ASC,
						CASE WHEN @SortBy = 'EmployeeID' and @SortType = 0 THEN [ResultMs].[EmployeeID] END DESC,
						CASE WHEN @SortBy = 'TopicID' and @SortType = 1 THEN [ResultMs].[TopicID] END ASC,
						CASE WHEN @SortBy = 'TopicID' and @SortType = 0 THEN [ResultMs].[TopicID] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[ResultMs]
			Where 
			(
				(@ResultID is null OR [ResultMs].[ResultID] like @ResultID)
				AND (@CreateDate is null OR [ResultMs].[CreateDate] = @CreateDate)
				AND (@EmployeeID is null OR [ResultMs].[EmployeeID] like @EmployeeID)
				AND (@TopicID is null OR [ResultMs].[TopicID] = @TopicID)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[ResultMs_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[ResultMs_Insert]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[ResultMs_Insert]
	@ResultID nvarchar(50),
	@CreateDate datetime,
	@EmployeeID nvarchar(50),
	@TopicID int
AS


INSERT INTO [dbo].[ResultMs] (
	[ResultID],
	[CreateDate],
	[EmployeeID],
	[TopicID]
) VALUES (
	@ResultID,
	@CreateDate,
	@EmployeeID,
	@TopicID
)

--end [dbo].[ResultMs_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[ResultMs_Update]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[ResultMs_Update]
	@ResultID nvarchar(50),
	@CreateDate datetime,
	@EmployeeID nvarchar(50),
	@TopicID int
AS


UPDATE [dbo].[ResultMs] SET
	[CreateDate] = @CreateDate,
	[EmployeeID] = @EmployeeID,
	[TopicID] = @TopicID
WHERE
	[ResultID] = @ResultID

--end [dbo].[ResultMs_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[ResultMs_Delete]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[ResultMs_Delete]
	@ResultID nvarchar(50)
AS


DELETE FROM [dbo].[ResultMs]
WHERE
(
	[ResultID] = @ResultID
)

--end [dbo].[ResultMs_Delete]
--endregion

GO
--=========================================================================================--

