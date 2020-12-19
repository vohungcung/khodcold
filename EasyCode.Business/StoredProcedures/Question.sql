--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Questions_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Questions_Insert]

IF OBJECT_ID(N'[dbo].[Questions_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Questions_Update]

IF OBJECT_ID(N'[dbo].[Questions_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Questions_Delete]

IF OBJECT_ID(N'[dbo].[Questions_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Questions_Select]

--endregion

GO


--region [dbo].[Questions_Select]

-- Create By: vohungcung
-- Date Generated: Wednesday, August 01, 2018

CREATE PROCEDURE [dbo].[Questions_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@TopicID int = null,
	@QID int = null,
	@Title nvarchar(250) = null,
	@Content ntext = null,
	@Pos int = null,
	@QT int = null,
	@Require bit = null,
	@ParentID int = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Questions]
					WHERE
					(
						(@TopicID is null OR [Questions].[TopicID] = @TopicID)
						AND (@QID is null OR [Questions].[QID] = @QID)
						AND (@Title is null OR [Questions].[Title] like @Title)
						AND (@Content is null OR [Questions].[Content] like @Content)
						AND (@Pos is null OR [Questions].[Pos] = @Pos)
						AND (@QT is null OR [Questions].[QT] = @QT)
						AND (@Require is null OR [Questions].[Require] = @Require)
						AND (@ParentID is null OR [Questions].[ParentID] = @ParentID)
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
			SELECT [dbo].[Questions].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'TopicID' and @SortType = 1 THEN [Questions].[TopicID] END ASC,
						CASE WHEN @SortBy = 'TopicID' and @SortType = 0 THEN [Questions].[TopicID] END DESC,
						CASE WHEN @SortBy = 'QID' and @SortType = 1 THEN [Questions].[QID] END ASC,
						CASE WHEN @SortBy = 'QID' and @SortType = 0 THEN [Questions].[QID] END DESC,
						CASE WHEN @SortBy = 'Title' and @SortType = 1 THEN [Questions].[Title] END ASC,
						CASE WHEN @SortBy = 'Title' and @SortType = 0 THEN [Questions].[Title] END DESC,
						CASE WHEN @SortBy = 'Pos' and @SortType = 1 THEN [Questions].[Pos] END ASC,
						CASE WHEN @SortBy = 'Pos' and @SortType = 0 THEN [Questions].[Pos] END DESC,
						CASE WHEN @SortBy = 'QT' and @SortType = 1 THEN [Questions].[QT] END ASC,
						CASE WHEN @SortBy = 'QT' and @SortType = 0 THEN [Questions].[QT] END DESC,
						CASE WHEN @SortBy = 'Require' and @SortType = 1 THEN [Questions].[Require] END ASC,
						CASE WHEN @SortBy = 'Require' and @SortType = 0 THEN [Questions].[Require] END DESC,
						CASE WHEN @SortBy = 'ParentID' and @SortType = 1 THEN [Questions].[ParentID] END ASC,
						CASE WHEN @SortBy = 'ParentID' and @SortType = 0 THEN [Questions].[ParentID] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Questions]
			Where 
			(
				(@TopicID is null OR [Questions].[TopicID] = @TopicID)
				AND (@QID is null OR [Questions].[QID] = @QID)
				AND (@Title is null OR [Questions].[Title] like @Title)
				AND (@Content is null OR [Questions].[Content] like @Content)
				AND (@Pos is null OR [Questions].[Pos] = @Pos)
				AND (@QT is null OR [Questions].[QT] = @QT)
				AND (@Require is null OR [Questions].[Require] = @Require)
				AND (@ParentID is null OR [Questions].[ParentID] = @ParentID)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Questions_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Questions_Insert]

-- Create By: vohungcung
-- Date Generated: Wednesday, August 01, 2018

CREATE PROCEDURE [dbo].[Questions_Insert]
	@QID int OUTPUT,
	@TopicID int,
	@Title nvarchar(250),
	@Content ntext,
	@Pos int,
	@QT int,
	@Require bit,
	@ParentID int

AS


INSERT INTO [dbo].[Questions] 
(
	[TopicID],
	[Title],
	[Content],
	[Pos],
	[QT],
	[Require],
	[ParentID]
)
VALUES 
(
	@TopicID,
	@Title,
	@Content,
	@Pos,
	@QT,
	@Require,
	@ParentID
)

SET @QID = SCOPE_IDENTITY()

--end [dbo].[Questions_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Questions_Update]

-- Create By: vohungcung
-- Date Generated: Wednesday, August 01, 2018

CREATE PROCEDURE [dbo].[Questions_Update]
	@TopicID int,
	@QID int,
	@Title nvarchar(250),
	@Content ntext,
	@Pos int,
	@QT int,
	@Require bit,
	@ParentID int
AS


UPDATE [dbo].[Questions] SET
	[TopicID] = @TopicID,
	[Title] = @Title,
	[Content] = @Content,
	[Pos] = @Pos,
	[QT] = @QT,
	[Require] = @Require,
	[ParentID] = @ParentID
WHERE
	[QID] = @QID

--end [dbo].[Questions_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Questions_Delete]

-- Create By: vohungcung
-- Date Generated: Wednesday, August 01, 2018

CREATE PROCEDURE [dbo].[Questions_Delete]
	@QID int
AS


DELETE FROM [dbo].[Questions]
WHERE
(
	[QID] = @QID
)

--end [dbo].[Questions_Delete]
--endregion

GO
--=========================================================================================--

