--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Orders_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Orders_Insert]

IF OBJECT_ID(N'[dbo].[Orders_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Orders_Update]

IF OBJECT_ID(N'[dbo].[Orders_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Orders_Delete]

IF OBJECT_ID(N'[dbo].[Orders_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Orders_Select]

--endregion

GO


--region [dbo].[Orders_Select]

-- Create By: Hung Cung
-- Date Generated: Tuesday, May 08, 2018

CREATE PROCEDURE [dbo].[Orders_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@OrderID nvarchar(50) = null,
	@OrderType nvarchar(50) = null,
	@CustomerID nvarchar(50) = null,
	@DeliveryAddress nvarchar(250) = null,
	@SAPOrderNumber nvarchar(50) = null,
	@Ref2 nvarchar(50) = null,
	@Status int = null,
	@TotalAmount money = null,
	@CreateDate datetime = null,
	@CreateBy int = null,
	@MarketingID nvarchar(50) = null,
	@Note nvarchar(250) = null,
	@Description nvarchar(250) = null,
	@District nvarchar(250) = null,
	@IsTran bit = null,
	@HNID nvarchar(50) = null,
	@Confirmed bit = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Orders]
					WHERE
					(
						(@OrderID is null OR [Orders].[OrderID] like @OrderID)
						AND (@OrderType is null OR [Orders].[OrderType] like @OrderType)
						AND (@CustomerID is null OR [Orders].[CustomerID] like @CustomerID)
						AND (@DeliveryAddress is null OR [Orders].[DeliveryAddress] like @DeliveryAddress)
						AND (@SAPOrderNumber is null OR [Orders].[SAPOrderNumber] like @SAPOrderNumber)
						AND (@Ref2 is null OR [Orders].[Ref2] like @Ref2)
						AND (@Status is null OR [Orders].[Status] = @Status)
						AND (@TotalAmount is null OR [Orders].[TotalAmount] = @TotalAmount)
						AND (@CreateDate is null OR [Orders].[CreateDate] = @CreateDate)
						AND (@CreateBy is null OR [Orders].[CreateBy] = @CreateBy)
						AND (@MarketingID is null OR [Orders].[MarketingID] like @MarketingID)
						AND (@Note is null OR [Orders].[Note] like @Note)
						AND (@Description is null OR [Orders].[Description] like @Description)
						AND (@District is null OR [Orders].[District] like @District)
						AND (@IsTran is null OR [Orders].[IsTran] = @IsTran)
						AND (@HNID is null OR [Orders].[HNID] like @HNID)
						AND (@Confirmed is null OR [Orders].[Confirmed] = @Confirmed)
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
			SELECT [dbo].[Orders].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 1 THEN [Orders].[OrderID] END ASC,
						CASE WHEN @SortBy = 'OrderID' and @SortType = 0 THEN [Orders].[OrderID] END DESC,
						CASE WHEN @SortBy = 'OrderType' and @SortType = 1 THEN [Orders].[OrderType] END ASC,
						CASE WHEN @SortBy = 'OrderType' and @SortType = 0 THEN [Orders].[OrderType] END DESC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 1 THEN [Orders].[CustomerID] END ASC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 0 THEN [Orders].[CustomerID] END DESC,
						CASE WHEN @SortBy = 'DeliveryAddress' and @SortType = 1 THEN [Orders].[DeliveryAddress] END ASC,
						CASE WHEN @SortBy = 'DeliveryAddress' and @SortType = 0 THEN [Orders].[DeliveryAddress] END DESC,
						CASE WHEN @SortBy = 'SAPOrderNumber' and @SortType = 1 THEN [Orders].[SAPOrderNumber] END ASC,
						CASE WHEN @SortBy = 'SAPOrderNumber' and @SortType = 0 THEN [Orders].[SAPOrderNumber] END DESC,
						CASE WHEN @SortBy = 'Ref2' and @SortType = 1 THEN [Orders].[Ref2] END ASC,
						CASE WHEN @SortBy = 'Ref2' and @SortType = 0 THEN [Orders].[Ref2] END DESC,
						CASE WHEN @SortBy = 'Status' and @SortType = 1 THEN [Orders].[Status] END ASC,
						CASE WHEN @SortBy = 'Status' and @SortType = 0 THEN [Orders].[Status] END DESC,
						CASE WHEN @SortBy = 'TotalAmount' and @SortType = 1 THEN [Orders].[TotalAmount] END ASC,
						CASE WHEN @SortBy = 'TotalAmount' and @SortType = 0 THEN [Orders].[TotalAmount] END DESC,
						CASE WHEN @SortBy = 'CreateDate' and @SortType = 1 THEN [Orders].[CreateDate] END ASC,
						CASE WHEN @SortBy = 'CreateDate' and @SortType = 0 THEN [Orders].[CreateDate] END DESC,
						CASE WHEN @SortBy = 'CreateBy' and @SortType = 1 THEN [Orders].[CreateBy] END ASC,
						CASE WHEN @SortBy = 'CreateBy' and @SortType = 0 THEN [Orders].[CreateBy] END DESC,
						CASE WHEN @SortBy = 'MarketingID' and @SortType = 1 THEN [Orders].[MarketingID] END ASC,
						CASE WHEN @SortBy = 'MarketingID' and @SortType = 0 THEN [Orders].[MarketingID] END DESC,
						CASE WHEN @SortBy = 'Note' and @SortType = 1 THEN [Orders].[Note] END ASC,
						CASE WHEN @SortBy = 'Note' and @SortType = 0 THEN [Orders].[Note] END DESC,
						CASE WHEN @SortBy = 'Description' and @SortType = 1 THEN [Orders].[Description] END ASC,
						CASE WHEN @SortBy = 'Description' and @SortType = 0 THEN [Orders].[Description] END DESC,
						CASE WHEN @SortBy = 'District' and @SortType = 1 THEN [Orders].[District] END ASC,
						CASE WHEN @SortBy = 'District' and @SortType = 0 THEN [Orders].[District] END DESC,
						CASE WHEN @SortBy = 'IsTran' and @SortType = 1 THEN [Orders].[IsTran] END ASC,
						CASE WHEN @SortBy = 'IsTran' and @SortType = 0 THEN [Orders].[IsTran] END DESC,
						CASE WHEN @SortBy = 'HNID' and @SortType = 1 THEN [Orders].[HNID] END ASC,
						CASE WHEN @SortBy = 'HNID' and @SortType = 0 THEN [Orders].[HNID] END DESC,
						CASE WHEN @SortBy = 'Confirmed' and @SortType = 1 THEN [Orders].[Confirmed] END ASC,
						CASE WHEN @SortBy = 'Confirmed' and @SortType = 0 THEN [Orders].[Confirmed] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Orders]
			Where 
			(
				(@OrderID is null OR [Orders].[OrderID] like @OrderID)
				AND (@OrderType is null OR [Orders].[OrderType] like @OrderType)
				AND (@CustomerID is null OR [Orders].[CustomerID] like @CustomerID)
				AND (@DeliveryAddress is null OR [Orders].[DeliveryAddress] like @DeliveryAddress)
				AND (@SAPOrderNumber is null OR [Orders].[SAPOrderNumber] like @SAPOrderNumber)
				AND (@Ref2 is null OR [Orders].[Ref2] like @Ref2)
				AND (@Status is null OR [Orders].[Status] = @Status)
				AND (@TotalAmount is null OR [Orders].[TotalAmount] = @TotalAmount)
				AND (@CreateDate is null OR [Orders].[CreateDate] = @CreateDate)
				AND (@CreateBy is null OR [Orders].[CreateBy] = @CreateBy)
				AND (@MarketingID is null OR [Orders].[MarketingID] like @MarketingID)
				AND (@Note is null OR [Orders].[Note] like @Note)
				AND (@Description is null OR [Orders].[Description] like @Description)
				AND (@District is null OR [Orders].[District] like @District)
				AND (@IsTran is null OR [Orders].[IsTran] = @IsTran)
				AND (@HNID is null OR [Orders].[HNID] like @HNID)
				AND (@Confirmed is null OR [Orders].[Confirmed] = @Confirmed)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Orders_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Orders_Insert]

-- Create By: Hung Cung
-- Date Generated: Tuesday, May 08, 2018

CREATE PROCEDURE [dbo].[Orders_Insert]
	@OrderID nvarchar(50),
	@OrderType nvarchar(50),
	@CustomerID nvarchar(50),
	@DeliveryAddress nvarchar(250),
	@SAPOrderNumber nvarchar(50),
	@Ref2 nvarchar(50),
	@Status int,
	@TotalAmount money,
	@CreateDate datetime,
	@CreateBy int,
	@MarketingID nvarchar(50),
	@Note nvarchar(250),
	@Description nvarchar(250),
	@District nvarchar(250),
	@IsTran bit,
	@HNID nvarchar(50),
	@Confirmed bit
AS


INSERT INTO [dbo].[Orders] (
	[OrderID],
	[OrderType],
	[CustomerID],
	[DeliveryAddress],
	[SAPOrderNumber],
	[Ref2],
	[Status],
	[TotalAmount],
	[CreateDate],
	[CreateBy],
	[MarketingID],
	[Note],
	[Description],
	[District],
	[IsTran],
	[HNID],
	[Confirmed]
) VALUES (
	@OrderID,
	@OrderType,
	@CustomerID,
	@DeliveryAddress,
	@SAPOrderNumber,
	@Ref2,
	@Status,
	@TotalAmount,
	@CreateDate,
	@CreateBy,
	@MarketingID,
	@Note,
	@Description,
	@District,
	@IsTran,
	@HNID,
	@Confirmed
)

--end [dbo].[Orders_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Orders_Update]

-- Create By: Hung Cung
-- Date Generated: Tuesday, May 08, 2018

CREATE PROCEDURE [dbo].[Orders_Update]
	@OrderID nvarchar(50),
	@OrderType nvarchar(50),
	@CustomerID nvarchar(50),
	@DeliveryAddress nvarchar(250),
	@SAPOrderNumber nvarchar(50),
	@Ref2 nvarchar(50),
	@Status int,
	@TotalAmount money,
	@CreateDate datetime,
	@CreateBy int,
	@MarketingID nvarchar(50),
	@Note nvarchar(250),
	@Description nvarchar(250),
	@District nvarchar(250),
	@IsTran bit,
	@HNID nvarchar(50),
	@Confirmed bit
AS


UPDATE [dbo].[Orders] SET
	[OrderType] = @OrderType,
	[CustomerID] = @CustomerID,
	[DeliveryAddress] = @DeliveryAddress,
	[SAPOrderNumber] = @SAPOrderNumber,
	[Ref2] = @Ref2,
	[Status] = @Status,
	[TotalAmount] = @TotalAmount,
	[CreateDate] = @CreateDate,
	[CreateBy] = @CreateBy,
	[MarketingID] = @MarketingID,
	[Note] = @Note,
	[Description] = @Description,
	[District] = @District,
	[IsTran] = @IsTran,
	[HNID] = @HNID,
	[Confirmed] = @Confirmed
WHERE
	[OrderID] = @OrderID

--end [dbo].[Orders_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Orders_Delete]

-- Create By: Hung Cung
-- Date Generated: Tuesday, May 08, 2018

CREATE PROCEDURE [dbo].[Orders_Delete]
	@OrderID nvarchar(50)
AS


DELETE FROM [dbo].[Orders]
WHERE
(
	[OrderID] = @OrderID
)

--end [dbo].[Orders_Delete]
--endregion

GO
--=========================================================================================--

