--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[l_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[l_Insert]

IF OBJECT_ID(N'[dbo].[l_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[l_Update]

IF OBJECT_ID(N'[dbo].[l_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[l_Delete]

IF OBJECT_ID(N'[dbo].[l_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[l_Select]

--endregion

GO


--region [dbo].[l_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[l_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@id int = null,
	@content ntext = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[l]
					WHERE
					(
						(@id is null OR [l].[id] = @id)
						AND (@content is null OR [l].[content] like @content)
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
			SELECT [dbo].[l].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'id' and @SortType = 1 THEN [l].[id] END ASC,
						CASE WHEN @SortBy = 'id' and @SortType = 0 THEN [l].[id] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[l]
			Where 
			(
				(@id is null OR [l].[id] = @id)
				AND (@content is null OR [l].[content] like @content)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[l_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[l_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[l_Insert]
	@id int OUTPUT,
	@content ntext

AS


INSERT INTO [dbo].[l] 
(
	[content]
)
VALUES 
(
	@content
)

SET @id = SCOPE_IDENTITY()

--end [dbo].[l_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[l_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[l_Update]
	@id int,
	@content ntext
AS


UPDATE [dbo].[l] SET
	[content] = @content
WHERE
	[id] = @id

--end [dbo].[l_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[l_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[l_Delete]
	@id int
AS


DELETE FROM [dbo].[l]
WHERE
(
	[id] = @id
)

--end [dbo].[l_Delete]
--endregion

GO
--=========================================================================================--

