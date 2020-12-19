--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[VOrderDetail_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[VOrderDetail_Select]

--endregion

GO


--region [dbo].[VOrderDetail_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[VOrderDetail_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@OrderNo int = null,
	@OrderID nvarchar(50) = null,
	@ItemID nvarchar(50) = null,
	@ItemName nvarchar(250) = null,
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
					From [dbo].[VOrderDetail]
					WHERE
					(
						(@OrderNo is null OR [VOrderDetail].[OrderNo] = @OrderNo)
						AND (@OrderID is null OR [VOrderDetail].[OrderID] like @OrderID)
						AND (@ItemID is null OR [VOrderDetail].[ItemID] like @ItemID)
						AND (@ItemName is null OR [VOrderDetail].[ItemName] like @ItemName)
						AND (@UoM is null OR [VOrderDetail].[UoM] like @UoM)
						AND (@UnitPrice is null OR [VOrderDetail].[UnitPrice] = @UnitPrice)
						AND (@Quantity is null OR [VOrderDetail].[Quantity] = @Quantity)
						AND (@Discount is null OR [VOrderDetail].[Discount] = @Discount)
						AND (@TotalAmount is null OR [VOrderDetail].[TotalAmount] = @TotalAmount)
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
			SELECT [dbo].[VOrderDetail].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY 
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'OrderNo' and @SortType = 1 THEN [VOrderDetail].[OrderNo] END ASC,
						CASE WHEN @SortBy = 'OrderNo' and @SortType = 0 THEN [VOrderDetail].[OrderNo] END DESC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 1 THEN [VOrderDetail].[OrderID] END ASC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 0 THEN [VOrderDetail].[OrderID] END DESC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 1 THEN [VOrderDetail].[ItemID] END ASC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 0 THEN [VOrderDetail].[ItemID] END DESC,
						CASE WHEN @SortBy = 'ItemName' and @SortType = 1 THEN [VOrderDetail].[ItemName] END ASC,
						CASE WHEN @SortBy = 'ItemName' and @SortType = 0 THEN [VOrderDetail].[ItemName] END DESC,
						CASE WHEN @SortBy = 'UoM' and @SortType = 1 THEN [VOrderDetail].[UoM] END ASC,
						CASE WHEN @SortBy = 'UoM' and @SortType = 0 THEN [VOrderDetail].[UoM] END DESC,
						CASE WHEN @SortBy = 'UnitPrice' and @SortType = 1 THEN [VOrderDetail].[UnitPrice] END ASC,
						CASE WHEN @SortBy = 'UnitPrice' and @SortType = 0 THEN [VOrderDetail].[UnitPrice] END DESC,
						CASE WHEN @SortBy = 'Quantity' and @SortType = 1 THEN [VOrderDetail].[Quantity] END ASC,
						CASE WHEN @SortBy = 'Quantity' and @SortType = 0 THEN [VOrderDetail].[Quantity] END DESC,
						CASE WHEN @SortBy = 'Discount' and @SortType = 1 THEN [VOrderDetail].[Discount] END ASC,
						CASE WHEN @SortBy = 'Discount' and @SortType = 0 THEN [VOrderDetail].[Discount] END DESC,
						CASE WHEN @SortBy = 'TotalAmount' and @SortType = 1 THEN [VOrderDetail].[TotalAmount] END ASC,
						CASE WHEN @SortBy = 'TotalAmount' and @SortType = 0 THEN [VOrderDetail].[TotalAmount] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[VOrderDetail]
			Where 
			(
				(@OrderNo is null OR [VOrderDetail].[OrderNo] = @OrderNo)
				AND (@OrderID is null OR [VOrderDetail].[OrderID] like @OrderID)
				AND (@ItemID is null OR [VOrderDetail].[ItemID] like @ItemID)
				AND (@ItemName is null OR [VOrderDetail].[ItemName] like @ItemName)
				AND (@UoM is null OR [VOrderDetail].[UoM] like @UoM)
				AND (@UnitPrice is null OR [VOrderDetail].[UnitPrice] = @UnitPrice)
				AND (@Quantity is null OR [VOrderDetail].[Quantity] = @Quantity)
				AND (@Discount is null OR [VOrderDetail].[Discount] = @Discount)
				AND (@TotalAmount is null OR [VOrderDetail].[TotalAmount] = @TotalAmount)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[VOrderDetail_Select]
--endregion

GO
--=========================================================================================--


