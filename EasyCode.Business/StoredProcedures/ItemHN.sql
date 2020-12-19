--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[ItemHN_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[ItemHN_Insert]

IF OBJECT_ID(N'[dbo].[ItemHN_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[ItemHN_Update]

IF OBJECT_ID(N'[dbo].[ItemHN_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[ItemHN_Delete]

IF OBJECT_ID(N'[dbo].[ItemHN_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[ItemHN_Select]

--endregion

GO


--region [dbo].[ItemHN_Select]

-- Create By: Hung Cung
-- Date Generated: Thursday, May 03, 2018

CREATE PROCEDURE [dbo].[ItemHN_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@HNID nvarchar(50) = null,
	@ItemID nvarchar(50) = null,
	@UnitPrice money = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[ItemHN]
					WHERE
					(
						(@HNID is null OR [ItemHN].[HNID] like @HNID)
						AND (@ItemID is null OR [ItemHN].[ItemID] like @ItemID)
						AND (@UnitPrice is null OR [ItemHN].[UnitPrice] = @UnitPrice)
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
			SELECT [dbo].[ItemHN].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'HNID' and @SortType = 1 THEN [ItemHN].[HNID] END ASC,
						CASE WHEN @SortBy = 'HNID' and @SortType = 0 THEN [ItemHN].[HNID] END DESC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 1 THEN [ItemHN].[ItemID] END ASC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 0 THEN [ItemHN].[ItemID] END DESC,
						CASE WHEN @SortBy = 'UnitPrice' and @SortType = 1 THEN [ItemHN].[UnitPrice] END ASC,
						CASE WHEN @SortBy = 'UnitPrice' and @SortType = 0 THEN [ItemHN].[UnitPrice] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[ItemHN]
			Where 
			(
				(@HNID is null OR [ItemHN].[HNID] like @HNID)
				AND (@ItemID is null OR [ItemHN].[ItemID] like @ItemID)
				AND (@UnitPrice is null OR [ItemHN].[UnitPrice] = @UnitPrice)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[ItemHN_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[ItemHN_Insert]

-- Create By: Hung Cung
-- Date Generated: Thursday, May 03, 2018

CREATE PROCEDURE [dbo].[ItemHN_Insert]
	@HNID nvarchar(50),
	@ItemID nvarchar(50),
	@UnitPrice money
AS


INSERT INTO [dbo].[ItemHN] (
	[HNID],
	[ItemID],
	[UnitPrice]
) VALUES (
	@HNID,
	@ItemID,
	@UnitPrice
)

--end [dbo].[ItemHN_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[ItemHN_Update]

-- Create By: Hung Cung
-- Date Generated: Thursday, May 03, 2018

CREATE PROCEDURE [dbo].[ItemHN_Update]
	@HNID nvarchar(50),
	@ItemID nvarchar(50),
	@UnitPrice money
AS


UPDATE [dbo].[ItemHN] SET
	[UnitPrice] = @UnitPrice
WHERE
	[HNID] = @HNID
	AND [ItemID] = @ItemID

--end [dbo].[ItemHN_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[ItemHN_Delete]

-- Create By: Hung Cung
-- Date Generated: Thursday, May 03, 2018

CREATE PROCEDURE [dbo].[ItemHN_Delete]
	@HNID nvarchar(50),
	@ItemID nvarchar(50)
AS


DELETE FROM [dbo].[ItemHN]
WHERE
(
	[HNID] = @HNID
	AND [ItemID] = @ItemID
)

--end [dbo].[ItemHN_Delete]
--endregion

GO
--=========================================================================================--

