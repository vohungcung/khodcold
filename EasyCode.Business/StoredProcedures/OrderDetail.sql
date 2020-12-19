--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[OrderDetail_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[OrderDetail_Insert]

IF OBJECT_ID(N'[dbo].[OrderDetail_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[OrderDetail_Update]

IF OBJECT_ID(N'[dbo].[OrderDetail_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[OrderDetail_Delete]

IF OBJECT_ID(N'[dbo].[OrderDetail_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[OrderDetail_Select]

--endregion

GO


--region [dbo].[OrderDetail_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[OrderDetail_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@OrderNo int = null,
	@OrderID nvarchar(50) = null,
	@ItemID nvarchar(50) = null,
	@UoM nvarchar(50) = null,
	@UnitPrice money = null,
	@Quantity decimal(18, 0) = null,
	@Discount decimal(8, 4) = null,
	@TotalAmount money = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[OrderDetail]
					WHERE
					(
						(@OrderNo is null OR [OrderDetail].[OrderNo] = @OrderNo)
						AND (@OrderID is null OR [OrderDetail].[OrderID] like @OrderID)
						AND (@ItemID is null OR [OrderDetail].[ItemID] like @ItemID)
						AND (@UoM is null OR [OrderDetail].[UoM] like @UoM)
						AND (@UnitPrice is null OR [OrderDetail].[UnitPrice] = @UnitPrice)
						AND (@Quantity is null OR [OrderDetail].[Quantity] = @Quantity)
						AND (@Discount is null OR [OrderDetail].[Discount] = @Discount)
						AND (@TotalAmount is null OR [OrderDetail].[TotalAmount] = @TotalAmount)
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
			SELECT [dbo].[OrderDetail].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'OrderNo' and @SortType = 1 THEN [OrderDetail].[OrderNo] END ASC,
						CASE WHEN @SortBy = 'OrderNo' and @SortType = 0 THEN [OrderDetail].[OrderNo] END DESC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 1 THEN [OrderDetail].[OrderID] END ASC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 0 THEN [OrderDetail].[OrderID] END DESC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 1 THEN [OrderDetail].[ItemID] END ASC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 0 THEN [OrderDetail].[ItemID] END DESC,
						CASE WHEN @SortBy = 'UoM' and @SortType = 1 THEN [OrderDetail].[UoM] END ASC,
						CASE WHEN @SortBy = 'UoM' and @SortType = 0 THEN [OrderDetail].[UoM] END DESC,
						CASE WHEN @SortBy = 'UnitPrice' and @SortType = 1 THEN [OrderDetail].[UnitPrice] END ASC,
						CASE WHEN @SortBy = 'UnitPrice' and @SortType = 0 THEN [OrderDetail].[UnitPrice] END DESC,
						CASE WHEN @SortBy = 'Quantity' and @SortType = 1 THEN [OrderDetail].[Quantity] END ASC,
						CASE WHEN @SortBy = 'Quantity' and @SortType = 0 THEN [OrderDetail].[Quantity] END DESC,
						CASE WHEN @SortBy = 'Discount' and @SortType = 1 THEN [OrderDetail].[Discount] END ASC,
						CASE WHEN @SortBy = 'Discount' and @SortType = 0 THEN [OrderDetail].[Discount] END DESC,
						CASE WHEN @SortBy = 'TotalAmount' and @SortType = 1 THEN [OrderDetail].[TotalAmount] END ASC,
						CASE WHEN @SortBy = 'TotalAmount' and @SortType = 0 THEN [OrderDetail].[TotalAmount] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[OrderDetail]
			Where 
			(
				(@OrderNo is null OR [OrderDetail].[OrderNo] = @OrderNo)
				AND (@OrderID is null OR [OrderDetail].[OrderID] like @OrderID)
				AND (@ItemID is null OR [OrderDetail].[ItemID] like @ItemID)
				AND (@UoM is null OR [OrderDetail].[UoM] like @UoM)
				AND (@UnitPrice is null OR [OrderDetail].[UnitPrice] = @UnitPrice)
				AND (@Quantity is null OR [OrderDetail].[Quantity] = @Quantity)
				AND (@Discount is null OR [OrderDetail].[Discount] = @Discount)
				AND (@TotalAmount is null OR [OrderDetail].[TotalAmount] = @TotalAmount)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[OrderDetail_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[OrderDetail_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[OrderDetail_Insert]
	@OrderNo int,
	@OrderID nvarchar(50),
	@ItemID nvarchar(50),
	@UoM nvarchar(50),
	@UnitPrice money,
	@Quantity decimal(18, 0),
	@Discount decimal(8, 4),
	@TotalAmount money
AS


INSERT INTO [dbo].[OrderDetail] (
	[OrderNo],
	[OrderID],
	[ItemID],
	[UoM],
	[UnitPrice],
	[Quantity],
	[Discount],
	[TotalAmount]
) VALUES (
	@OrderNo,
	@OrderID,
	@ItemID,
	@UoM,
	@UnitPrice,
	@Quantity,
	@Discount,
	@TotalAmount
)

--end [dbo].[OrderDetail_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[OrderDetail_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[OrderDetail_Update]
	@OrderNo int,
	@OrderID nvarchar(50),
	@ItemID nvarchar(50),
	@UoM nvarchar(50),
	@UnitPrice money,
	@Quantity decimal(18, 0),
	@Discount decimal(8, 4),
	@TotalAmount money
AS


UPDATE [dbo].[OrderDetail] SET
	[OrderNo] = @OrderNo,
	[UoM] = @UoM,
	[UnitPrice] = @UnitPrice,
	[Quantity] = @Quantity,
	[Discount] = @Discount,
	[TotalAmount] = @TotalAmount
WHERE
	[OrderID] = @OrderID
	AND [ItemID] = @ItemID

--end [dbo].[OrderDetail_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[OrderDetail_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[OrderDetail_Delete]
	@OrderID nvarchar(50),
	@ItemID nvarchar(50)
AS


DELETE FROM [dbo].[OrderDetail]
WHERE
(
	[OrderID] = @OrderID
	AND [ItemID] = @ItemID
)

--end [dbo].[OrderDetail_Delete]
--endregion

GO
--=========================================================================================--

