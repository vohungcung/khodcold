--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Topics_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Topics_Insert]

IF OBJECT_ID(N'[dbo].[Topics_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Topics_Update]

IF OBJECT_ID(N'[dbo].[Topics_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Topics_Delete]

IF OBJECT_ID(N'[dbo].[Topics_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Topics_Select]

--endregion

GO


--region [dbo].[Topics_Select]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[Topics_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@TopicID int = null,
	@Title nvarchar(250) = null,
	@Description ntext = null,
	@Used bit = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Topics]
					WHERE
					(
						(@TopicID is null OR [Topics].[TopicID] = @TopicID)
						AND (@Title is null OR [Topics].[Title] like @Title)
						AND (@Description is null OR [Topics].[Description] like @Description)
						AND (@Used is null OR [Topics].[Used] = @Used)
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
			SELECT [dbo].[Topics].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'TopicID' and @SortType = 1 THEN [Topics].[TopicID] END ASC,
						CASE WHEN @SortBy = 'TopicID' and @SortType = 0 THEN [Topics].[TopicID] END DESC,
						CASE WHEN @SortBy = 'Title' and @SortType = 1 THEN [Topics].[Title] END ASC,
						CASE WHEN @SortBy = 'Title' and @SortType = 0 THEN [Topics].[Title] END DESC,
						CASE WHEN @SortBy = 'Used' and @SortType = 1 THEN [Topics].[Used] END ASC,
						CASE WHEN @SortBy = 'Used' and @SortType = 0 THEN [Topics].[Used] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Topics]
			Where 
			(
				(@TopicID is null OR [Topics].[TopicID] = @TopicID)
				AND (@Title is null OR [Topics].[Title] like @Title)
				AND (@Description is null OR [Topics].[Description] like @Description)
				AND (@Used is null OR [Topics].[Used] = @Used)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Topics_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Topics_Insert]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[Topics_Insert]
	@TopicID int OUTPUT,
	@Title nvarchar(250),
	@Description ntext,
	@Used bit

AS


INSERT INTO [dbo].[Topics] 
(
	[Title],
	[Description],
	[Used]
)
VALUES 
(
	@Title,
	@Description,
	@Used
)

SET @TopicID = SCOPE_IDENTITY()

--end [dbo].[Topics_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Topics_Update]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[Topics_Update]
	@TopicID int,
	@Title nvarchar(250),
	@Description ntext,
	@Used bit
AS


UPDATE [dbo].[Topics] SET
	[Title] = @Title,
	[Description] = @Description,
	[Used] = @Used
WHERE
	[TopicID] = @TopicID

--end [dbo].[Topics_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Topics_Delete]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[Topics_Delete]
	@TopicID int
AS


DELETE FROM [dbo].[Topics]
WHERE
(
	[TopicID] = @TopicID
)

--end [dbo].[Topics_Delete]
--endregion

GO
--=========================================================================================--

