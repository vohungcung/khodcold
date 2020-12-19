--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[VOrders_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[VOrders_Select]

--endregion

GO


--region [dbo].[VOrders_Select]

-- Create By: Hung Cung
-- Date Generated: Tuesday, May 08, 2018

CREATE PROCEDURE [dbo].[VOrders_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@OrderID nvarchar(50) = null,
	@OrderType nvarchar(50) = null,
	@CustomerID nvarchar(50) = null,
	@CustomerName nvarchar(250) = null,
	@DeliveryAddress nvarchar(250) = null,
	@SAPOrderNumber nvarchar(50) = null,
	@Ref2 nvarchar(50) = null,
	@Status int = null,
	@TotalAmount money = null,
	@CreateDate datetime = null,
	@CreateBy int = null,
	@ZipCode nvarchar(50) = null,
	@Quantity decimal(38, 0) = null,
	@ZO1 bit = null,
	@ZOD bit = null,
	@ZO2 bit = null,
	@ZOC bit = null,
	@MarketingID nvarchar(50) = null,
	@Description nvarchar(250) = null,
	@Note nvarchar(250) = null,
	@District nvarchar(250) = null,
	@IsTran bit = null,
	@HNID nvarchar(50) = null,
	@Site nvarchar(50) = null,
	@Confirmed bit = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[VOrders]
					WHERE
					(
						(@OrderID is null OR [VOrders].[OrderID] like @OrderID)
						AND (@OrderType is null OR [VOrders].[OrderType] like @OrderType)
						AND (@CustomerID is null OR [VOrders].[CustomerID] like @CustomerID)
						AND (@CustomerName is null OR [VOrders].[CustomerName] like @CustomerName)
						AND (@DeliveryAddress is null OR [VOrders].[DeliveryAddress] like @DeliveryAddress)
						AND (@SAPOrderNumber is null OR [VOrders].[SAPOrderNumber] like @SAPOrderNumber)
						AND (@Ref2 is null OR [VOrders].[Ref2] like @Ref2)
						AND (@Status is null OR [VOrders].[Status] = @Status)
						AND (@TotalAmount is null OR [VOrders].[TotalAmount] = @TotalAmount)
						AND (@CreateDate is null OR [VOrders].[CreateDate] = @CreateDate)
						AND (@CreateBy is null OR [VOrders].[CreateBy] = @CreateBy)
						AND (@ZipCode is null OR [VOrders].[ZipCode] like @ZipCode)
						AND (@Quantity is null OR [VOrders].[Quantity] = @Quantity)
						AND (@ZO1 is null OR [VOrders].[ZO1] = @ZO1)
						AND (@ZOD is null OR [VOrders].[ZOD] = @ZOD)
						AND (@ZO2 is null OR [VOrders].[ZO2] = @ZO2)
						AND (@ZOC is null OR [VOrders].[ZOC] = @ZOC)
						AND (@MarketingID is null OR [VOrders].[MarketingID] like @MarketingID)
						AND (@Description is null OR [VOrders].[Description] like @Description)
						AND (@Note is null OR [VOrders].[Note] like @Note)
						AND (@District is null OR [VOrders].[District] like @District)
						AND (@IsTran is null OR [VOrders].[IsTran] = @IsTran)
						AND (@HNID is null OR [VOrders].[HNID] like @HNID)
						AND (@Site is null OR [VOrders].[Site] like @Site)
						AND (@Confirmed is null OR [VOrders].[Confirmed] = @Confirmed)
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
			SELECT [dbo].[VOrders].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY 
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 1 THEN [VOrders].[OrderID] END ASC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 0 THEN [VOrders].[OrderID] END DESC,
						CASE WHEN @SortBy = 'OrderType' and @SortType = 1 THEN [VOrders].[OrderType] END ASC,
						CASE WHEN @SortBy = 'OrderType' and @SortType = 0 THEN [VOrders].[OrderType] END DESC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 1 THEN [VOrders].[CustomerID] END ASC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 0 THEN [VOrders].[CustomerID] END DESC,
						CASE WHEN @SortBy = 'CustomerName' and @SortType = 1 THEN [VOrders].[CustomerName] END ASC,
						CASE WHEN @SortBy = 'CustomerName' and @SortType = 0 THEN [VOrders].[CustomerName] END DESC,
						CASE WHEN @SortBy = 'DeliveryAddress' and @SortType = 1 THEN [VOrders].[DeliveryAddress] END ASC,
						CASE WHEN @SortBy = 'DeliveryAddress' and @SortType = 0 THEN [VOrders].[DeliveryAddress] END DESC,
						CASE WHEN @SortBy = 'SAPOrderNumber' and @SortType = 1 THEN [VOrders].[SAPOrderNumber] END ASC,
						CASE WHEN @SortBy = 'SAPOrderNumber' and @SortType = 0 THEN [VOrders].[SAPOrderNumber] END DESC,
						CASE WHEN @SortBy = 'Ref2' and @SortType = 1 THEN [VOrders].[Ref2] END ASC,
						CASE WHEN @SortBy = 'Ref2' and @SortType = 0 THEN [VOrders].[Ref2] END DESC,
						CASE WHEN @SortBy = 'Status' and @SortType = 1 THEN [VOrders].[Status] END ASC,
						CASE WHEN @SortBy = 'Status' and @SortType = 0 THEN [VOrders].[Status] END DESC,
						CASE WHEN @SortBy = 'TotalAmount' and @SortType = 1 THEN [VOrders].[TotalAmount] END ASC,
						CASE WHEN @SortBy = 'TotalAmount' and @SortType = 0 THEN [VOrders].[TotalAmount] END DESC,
						CASE WHEN @SortBy = 'CreateDate' and @SortType = 1 THEN [VOrders].[CreateDate] END ASC,
						CASE WHEN @SortBy = 'CreateDate' and @SortType = 0 THEN [VOrders].[CreateDate] END DESC,
						CASE WHEN @SortBy = 'CreateBy' and @SortType = 1 THEN [VOrders].[CreateBy] END ASC,
						CASE WHEN @SortBy = 'CreateBy' and @SortType = 0 THEN [VOrders].[CreateBy] END DESC,
						CASE WHEN @SortBy = 'ZipCode' and @SortType = 1 THEN [VOrders].[ZipCode] END ASC,
						CASE WHEN @SortBy = 'ZipCode' and @SortType = 0 THEN [VOrders].[ZipCode] END DESC,
						CASE WHEN @SortBy = 'Quantity' and @SortType = 1 THEN [VOrders].[Quantity] END ASC,
						CASE WHEN @SortBy = 'Quantity' and @SortType = 0 THEN [VOrders].[Quantity] END DESC,
						CASE WHEN @SortBy = 'ZO1' and @SortType = 1 THEN [VOrders].[ZO1] END ASC,
						CASE WHEN @SortBy = 'ZO1' and @SortType = 0 THEN [VOrders].[ZO1] END DESC,
						CASE WHEN @SortBy = 'ZOD' and @SortType = 1 THEN [VOrders].[ZOD] END ASC,
						CASE WHEN @SortBy = 'ZOD' and @SortType = 0 THEN [VOrders].[ZOD] END DESC,
						CASE WHEN @SortBy = 'ZO2' and @SortType = 1 THEN [VOrders].[ZO2] END ASC,
						CASE WHEN @SortBy = 'ZO2' and @SortType = 0 THEN [VOrders].[ZO2] END DESC,
						CASE WHEN @SortBy = 'ZOC' and @SortType = 1 THEN [VOrders].[ZOC] END ASC,
						CASE WHEN @SortBy = 'ZOC' and @SortType = 0 THEN [VOrders].[ZOC] END DESC,
						CASE WHEN @SortBy = 'MarketingID' and @SortType = 1 THEN [VOrders].[MarketingID] END ASC,
						CASE WHEN @SortBy = 'MarketingID' and @SortType = 0 THEN [VOrders].[MarketingID] END DESC,
						CASE WHEN @SortBy = 'Description' and @SortType = 1 THEN [VOrders].[Description] END ASC,
						CASE WHEN @SortBy = 'Description' and @SortType = 0 THEN [VOrders].[Description] END DESC,
						CASE WHEN @SortBy = 'Note' and @SortType = 1 THEN [VOrders].[Note] END ASC,
						CASE WHEN @SortBy = 'Note' and @SortType = 0 THEN [VOrders].[Note] END DESC,
						CASE WHEN @SortBy = 'District' and @SortType = 1 THEN [VOrders].[District] END ASC,
						CASE WHEN @SortBy = 'District' and @SortType = 0 THEN [VOrders].[District] END DESC,
						CASE WHEN @SortBy = 'IsTran' and @SortType = 1 THEN [VOrders].[IsTran] END ASC,
						CASE WHEN @SortBy = 'IsTran' and @SortType = 0 THEN [VOrders].[IsTran] END DESC,
						CASE WHEN @SortBy = 'HNID' and @SortType = 1 THEN [VOrders].[HNID] END ASC,
						CASE WHEN @SortBy = 'HNID' and @SortType = 0 THEN [VOrders].[HNID] END DESC,
						CASE WHEN @SortBy = 'Site' and @SortType = 1 THEN [VOrders].[Site] END ASC,
						CASE WHEN @SortBy = 'Site' and @SortType = 0 THEN [VOrders].[Site] END DESC,
						CASE WHEN @SortBy = 'Confirmed' and @SortType = 1 THEN [VOrders].[Confirmed] END ASC,
						CASE WHEN @SortBy = 'Confirmed' and @SortType = 0 THEN [VOrders].[Confirmed] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[VOrders]
			Where 
			(
				(@OrderID is null OR [VOrders].[OrderID] like @OrderID)
				AND (@OrderType is null OR [VOrders].[OrderType] like @OrderType)
				AND (@CustomerID is null OR [VOrders].[CustomerID] like @CustomerID)
				AND (@CustomerName is null OR [VOrders].[CustomerName] like @CustomerName)
				AND (@DeliveryAddress is null OR [VOrders].[DeliveryAddress] like @DeliveryAddress)
				AND (@SAPOrderNumber is null OR [VOrders].[SAPOrderNumber] like @SAPOrderNumber)
				AND (@Ref2 is null OR [VOrders].[Ref2] like @Ref2)
				AND (@Status is null OR [VOrders].[Status] = @Status)
				AND (@TotalAmount is null OR [VOrders].[TotalAmount] = @TotalAmount)
				AND (@CreateDate is null OR [VOrders].[CreateDate] = @CreateDate)
				AND (@CreateBy is null OR [VOrders].[CreateBy] = @CreateBy)
				AND (@ZipCode is null OR [VOrders].[ZipCode] like @ZipCode)
				AND (@Quantity is null OR [VOrders].[Quantity] = @Quantity)
				AND (@ZO1 is null OR [VOrders].[ZO1] = @ZO1)
				AND (@ZOD is null OR [VOrders].[ZOD] = @ZOD)
				AND (@ZO2 is null OR [VOrders].[ZO2] = @ZO2)
				AND (@ZOC is null OR [VOrders].[ZOC] = @ZOC)
				AND (@MarketingID is null OR [VOrders].[MarketingID] like @MarketingID)
				AND (@Description is null OR [VOrders].[Description] like @Description)
				AND (@Note is null OR [VOrders].[Note] like @Note)
				AND (@District is null OR [VOrders].[District] like @District)
				AND (@IsTran is null OR [VOrders].[IsTran] = @IsTran)
				AND (@HNID is null OR [VOrders].[HNID] like @HNID)
				AND (@Site is null OR [VOrders].[Site] like @Site)
				AND (@Confirmed is null OR [VOrders].[Confirmed] = @Confirmed)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[VOrders_Select]
--endregion

GO
--=========================================================================================--


