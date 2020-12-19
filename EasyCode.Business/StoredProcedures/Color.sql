--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Colors_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Colors_Insert]

IF OBJECT_ID(N'[dbo].[Colors_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Colors_Update]

IF OBJECT_ID(N'[dbo].[Colors_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Colors_Delete]

IF OBJECT_ID(N'[dbo].[Colors_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Colors_Select]

--endregion

GO


--region [dbo].[Colors_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Colors_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@ColorID nvarchar(50) = null,
	@ColorName nvarchar(50) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Colors]
					WHERE
					(
						(@ColorID is null OR [Colors].[ColorID] like @ColorID)
						AND (@ColorName is null OR [Colors].[ColorName] like @ColorName)
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
			SELECT [dbo].[Colors].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'ColorID' and @SortType = 1 THEN [Colors].[ColorID] END ASC,
						CASE WHEN @SortBy = 'ColorID' and @SortType = 0 THEN [Colors].[ColorID] END DESC,
						CASE WHEN @SortBy = 'ColorName' and @SortType = 1 THEN [Colors].[ColorName] END ASC,
						CASE WHEN @SortBy = 'ColorName' and @SortType = 0 THEN [Colors].[ColorName] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Colors]
			Where 
			(
				(@ColorID is null OR [Colors].[ColorID] like @ColorID)
				AND (@ColorName is null OR [Colors].[ColorName] like @ColorName)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Colors_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Colors_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Colors_Insert]
	@ColorID nvarchar(50),
	@ColorName nvarchar(50)
AS


INSERT INTO [dbo].[Colors] (
	[ColorID],
	[ColorName]
) VALUES (
	@ColorID,
	@ColorName
)

--end [dbo].[Colors_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Colors_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Colors_Update]
	@ColorID nvarchar(50),
	@ColorName nvarchar(50)
AS


UPDATE [dbo].[Colors] SET
	[ColorName] = @ColorName
WHERE
	[ColorID] = @ColorID

--end [dbo].[Colors_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Colors_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Colors_Delete]
	@ColorID nvarchar(50)
AS


DELETE FROM [dbo].[Colors]
WHERE
(
	[ColorID] = @ColorID
)

--end [dbo].[Colors_Delete]
--endregion

GO
--=========================================================================================--

