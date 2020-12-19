--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[VItemHasThumb_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[VItemHasThumb_Select]

--endregion

GO


--region [dbo].[VItemHasThumb_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[VItemHasThumb_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@ItemID nvarchar(9) = null,
	@ItemName nvarchar(250) = null,
	@ThumbImage nvarchar(250) = null,
	@Size nvarchar(2) = null,
	@unitprice money = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[VItemHasThumb]
					WHERE
					(
						(@ItemID is null OR [VItemHasThumb].[ItemID] like @ItemID)
						AND (@ItemName is null OR [VItemHasThumb].[ItemName] like @ItemName)
						AND (@ThumbImage is null OR [VItemHasThumb].[ThumbImage] like @ThumbImage)
						AND (@Size is null OR [VItemHasThumb].[Size] like @Size)
						AND (@unitprice is null OR [VItemHasThumb].[unitprice] = @unitprice)
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
			SELECT [dbo].[VItemHasThumb].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY 
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 1 THEN [VItemHasThumb].[ItemID] END ASC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 0 THEN [VItemHasThumb].[ItemID] END DESC,
						CASE WHEN @SortBy = 'ItemName' and @SortType = 1 THEN [VItemHasThumb].[ItemName] END ASC,
						CASE WHEN @SortBy = 'ItemName' and @SortType = 0 THEN [VItemHasThumb].[ItemName] END DESC,
						CASE WHEN @SortBy = 'ThumbImage' and @SortType = 1 THEN [VItemHasThumb].[ThumbImage] END ASC,
						CASE WHEN @SortBy = 'ThumbImage' and @SortType = 0 THEN [VItemHasThumb].[ThumbImage] END DESC,
						CASE WHEN @SortBy = 'Size' and @SortType = 1 THEN [VItemHasThumb].[Size] END ASC,
						CASE WHEN @SortBy = 'Size' and @SortType = 0 THEN [VItemHasThumb].[Size] END DESC,
						CASE WHEN @SortBy = 'unitprice' and @SortType = 1 THEN [VItemHasThumb].[unitprice] END ASC,
						CASE WHEN @SortBy = 'unitprice' and @SortType = 0 THEN [VItemHasThumb].[unitprice] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[VItemHasThumb]
			Where 
			(
				(@ItemID is null OR [VItemHasThumb].[ItemID] like @ItemID)
				AND (@ItemName is null OR [VItemHasThumb].[ItemName] like @ItemName)
				AND (@ThumbImage is null OR [VItemHasThumb].[ThumbImage] like @ThumbImage)
				AND (@Size is null OR [VItemHasThumb].[Size] like @Size)
				AND (@unitprice is null OR [VItemHasThumb].[unitprice] = @unitprice)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[VItemHasThumb_Select]
--endregion

GO
--=========================================================================================--


