--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Items_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Items_Insert]

IF OBJECT_ID(N'[dbo].[Items_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Items_Update]

IF OBJECT_ID(N'[dbo].[Items_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Items_Delete]

IF OBJECT_ID(N'[dbo].[Items_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Items_Select]

--endregion

GO


--region [dbo].[Items_Select]

-- Create By: Hung Cung
-- Date Generated: Monday, April 23, 2018

CREATE PROCEDURE [dbo].[Items_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@ItemID nvarchar(50) = null,
	@ItemName nvarchar(250) = null,
	@StockSAP decimal(18, 0) = null,
	@OrderQuantity decimal(18, 0) = null,
	@UnitPrice money = null,
	@UoM nvarchar(50) = null,
	@ItemType nvarchar(50) = null,
	@ThumbImage nvarchar(250) = null,
	@Sex bit = null,
	@IsNew bit = null,
	@MC nvarchar(50) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Items]
					WHERE
					(
						(@ItemID is null OR [Items].[ItemID] like @ItemID)
						AND (@ItemName is null OR [Items].[ItemName] like @ItemName)
						AND (@StockSAP is null OR [Items].[StockSAP] = @StockSAP)
						AND (@OrderQuantity is null OR [Items].[OrderQuantity] = @OrderQuantity)
						AND (@UnitPrice is null OR [Items].[UnitPrice] = @UnitPrice)
						AND (@UoM is null OR [Items].[UoM] like @UoM)
						AND (@ItemType is null OR [Items].[ItemType] like @ItemType)
						AND (@ThumbImage is null OR [Items].[ThumbImage] like @ThumbImage)
						AND (@Sex is null OR [Items].[Sex] = @Sex)
						AND (@IsNew is null OR [Items].[IsNew] = @IsNew)
						AND (@MC is null OR [Items].[MC] like @MC)
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
			SELECT [dbo].[Items].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 1 THEN [Items].[ItemID] END ASC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 0 THEN [Items].[ItemID] END DESC,
						CASE WHEN @SortBy = 'ItemName' and @SortType = 1 THEN [Items].[ItemName] END ASC,
						CASE WHEN @SortBy = 'ItemName' and @SortType = 0 THEN [Items].[ItemName] END DESC,
						CASE WHEN @SortBy = 'StockSAP' and @SortType = 1 THEN [Items].[StockSAP] END ASC,
						CASE WHEN @SortBy = 'StockSAP' and @SortType = 0 THEN [Items].[StockSAP] END DESC,
						CASE WHEN @SortBy = 'OrderQuantity' and @SortType = 1 THEN [Items].[OrderQuantity] END ASC,
						CASE WHEN @SortBy = 'OrderQuantity' and @SortType = 0 THEN [Items].[OrderQuantity] END DESC,
						CASE WHEN @SortBy = 'UnitPrice' and @SortType = 1 THEN [Items].[UnitPrice] END ASC,
						CASE WHEN @SortBy = 'UnitPrice' and @SortType = 0 THEN [Items].[UnitPrice] END DESC,
						CASE WHEN @SortBy = 'UoM' and @SortType = 1 THEN [Items].[UoM] END ASC,
						CASE WHEN @SortBy = 'UoM' and @SortType = 0 THEN [Items].[UoM] END DESC,
						CASE WHEN @SortBy = 'ItemType' and @SortType = 1 THEN [Items].[ItemType] END ASC,
						CASE WHEN @SortBy = 'ItemType' and @SortType = 0 THEN [Items].[ItemType] END DESC,
						CASE WHEN @SortBy = 'ThumbImage' and @SortType = 1 THEN [Items].[ThumbImage] END ASC,
						CASE WHEN @SortBy = 'ThumbImage' and @SortType = 0 THEN [Items].[ThumbImage] END DESC,
						CASE WHEN @SortBy = 'Sex' and @SortType = 1 THEN [Items].[Sex] END ASC,
						CASE WHEN @SortBy = 'Sex' and @SortType = 0 THEN [Items].[Sex] END DESC,
						CASE WHEN @SortBy = 'IsNew' and @SortType = 1 THEN [Items].[IsNew] END ASC,
						CASE WHEN @SortBy = 'IsNew' and @SortType = 0 THEN [Items].[IsNew] END DESC,
						CASE WHEN @SortBy = 'MC' and @SortType = 1 THEN [Items].[MC] END ASC,
						CASE WHEN @SortBy = 'MC' and @SortType = 0 THEN [Items].[MC] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Items]
			Where 
			(
				(@ItemID is null OR [Items].[ItemID] like @ItemID)
				AND (@ItemName is null OR [Items].[ItemName] like @ItemName)
				AND (@StockSAP is null OR [Items].[StockSAP] = @StockSAP)
				AND (@OrderQuantity is null OR [Items].[OrderQuantity] = @OrderQuantity)
				AND (@UnitPrice is null OR [Items].[UnitPrice] = @UnitPrice)
				AND (@UoM is null OR [Items].[UoM] like @UoM)
				AND (@ItemType is null OR [Items].[ItemType] like @ItemType)
				AND (@ThumbImage is null OR [Items].[ThumbImage] like @ThumbImage)
				AND (@Sex is null OR [Items].[Sex] = @Sex)
				AND (@IsNew is null OR [Items].[IsNew] = @IsNew)
				AND (@MC is null OR [Items].[MC] like @MC)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Items_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Items_Insert]

-- Create By: Hung Cung
-- Date Generated: Monday, April 23, 2018

CREATE PROCEDURE [dbo].[Items_Insert]
	@ItemID nvarchar(50),
	@ItemName nvarchar(250),
	@StockSAP decimal(18, 0),
	@OrderQuantity decimal(18, 0),
	@UnitPrice money,
	@UoM nvarchar(50),
	@ItemType nvarchar(50),
	@ThumbImage nvarchar(250),
	@Sex bit,
	@IsNew bit,
	@MC nvarchar(50)
AS


INSERT INTO [dbo].[Items] (
	[ItemID],
	[ItemName],
	[StockSAP],
	[OrderQuantity],
	[UnitPrice],
	[UoM],
	[ItemType],
	[ThumbImage],
	[Sex],
	[IsNew],
	[MC]
) VALUES (
	@ItemID,
	@ItemName,
	@StockSAP,
	@OrderQuantity,
	@UnitPrice,
	@UoM,
	@ItemType,
	@ThumbImage,
	@Sex,
	@IsNew,
	@MC
)

--end [dbo].[Items_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Items_Update]

-- Create By: Hung Cung
-- Date Generated: Monday, April 23, 2018

CREATE PROCEDURE [dbo].[Items_Update]
	@ItemID nvarchar(50),
	@ItemName nvarchar(250),
	@StockSAP decimal(18, 0),
	@OrderQuantity decimal(18, 0),
	@UnitPrice money,
	@UoM nvarchar(50),
	@ItemType nvarchar(50),
	@ThumbImage nvarchar(250),
	@Sex bit,
	@IsNew bit,
	@MC nvarchar(50)
AS


UPDATE [dbo].[Items] SET
	[ItemName] = @ItemName,
	[StockSAP] = @StockSAP,
	[OrderQuantity] = @OrderQuantity,
	[UnitPrice] = @UnitPrice,
	[UoM] = @UoM,
	[ItemType] = @ItemType,
	[ThumbImage] = @ThumbImage,
	[Sex] = @Sex,
	[IsNew] = @IsNew,
	[MC] = @MC
WHERE
	[ItemID] = @ItemID

--end [dbo].[Items_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Items_Delete]

-- Create By: Hung Cung
-- Date Generated: Monday, April 23, 2018

CREATE PROCEDURE [dbo].[Items_Delete]
	@ItemID nvarchar(50)
AS


DELETE FROM [dbo].[Items]
WHERE
(
	[ItemID] = @ItemID
)

--end [dbo].[Items_Delete]
--endregion

GO
--=========================================================================================--

