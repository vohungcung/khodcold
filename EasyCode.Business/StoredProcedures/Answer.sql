--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Answers_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Answers_Insert]

IF OBJECT_ID(N'[dbo].[Answers_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Answers_Update]

IF OBJECT_ID(N'[dbo].[Answers_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Answers_Delete]

IF OBJECT_ID(N'[dbo].[Answers_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Answers_Select]

--endregion

GO


--region [dbo].[Answers_Select]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[Answers_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@QID int = null,
	@AID int = null,
	@Title nvarchar(250) = null,
	@Pos int = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Answers]
					WHERE
					(
						(@QID is null OR [Answers].[QID] = @QID)
						AND (@AID is null OR [Answers].[AID] = @AID)
						AND (@Title is null OR [Answers].[Title] like @Title)
						AND (@Pos is null OR [Answers].[Pos] = @Pos)
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
			SELECT [dbo].[Answers].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'QID' and @SortType = 1 THEN [Answers].[QID] END ASC,
						CASE WHEN @SortBy = 'QID' and @SortType = 0 THEN [Answers].[QID] END DESC,
						CASE WHEN @SortBy = 'AID' and @SortType = 1 THEN [Answers].[AID] END ASC,
						CASE WHEN @SortBy = 'AID' and @SortType = 0 THEN [Answers].[AID] END DESC,
						CASE WHEN @SortBy = 'Title' and @SortType = 1 THEN [Answers].[Title] END ASC,
						CASE WHEN @SortBy = 'Title' and @SortType = 0 THEN [Answers].[Title] END DESC,
						CASE WHEN @SortBy = 'Pos' and @SortType = 1 THEN [Answers].[Pos] END ASC,
						CASE WHEN @SortBy = 'Pos' and @SortType = 0 THEN [Answers].[Pos] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Answers]
			Where 
			(
				(@QID is null OR [Answers].[QID] = @QID)
				AND (@AID is null OR [Answers].[AID] = @AID)
				AND (@Title is null OR [Answers].[Title] like @Title)
				AND (@Pos is null OR [Answers].[Pos] = @Pos)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Answers_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Answers_Insert]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[Answers_Insert]
	@AID int OUTPUT,
	@QID int,
	@Title nvarchar(250),
	@Pos int

AS


INSERT INTO [dbo].[Answers] 
(
	[QID],
	[Title],
	[Pos]
)
VALUES 
(
	@QID,
	@Title,
	@Pos
)

SET @AID = SCOPE_IDENTITY()

--end [dbo].[Answers_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Answers_Update]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[Answers_Update]
	@QID int,
	@AID int,
	@Title nvarchar(250),
	@Pos int
AS


UPDATE [dbo].[Answers] SET
	[QID] = @QID,
	[Title] = @Title,
	[Pos] = @Pos
WHERE
	[AID] = @AID

--end [dbo].[Answers_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Answers_Delete]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[Answers_Delete]
	@AID int
AS


DELETE FROM [dbo].[Answers]
WHERE
(
	[AID] = @AID
)

--end [dbo].[Answers_Delete]
--endregion

GO
--=========================================================================================--

