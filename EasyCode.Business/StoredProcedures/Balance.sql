--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Balances_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Balances_Insert]

IF OBJECT_ID(N'[dbo].[Balances_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Balances_Update]

IF OBJECT_ID(N'[dbo].[Balances_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Balances_Delete]

IF OBJECT_ID(N'[dbo].[Balances_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Balances_Select]

--endregion

GO


--region [dbo].[Balances_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Balances_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@WareHouseID nvarchar(50) = null,
	@Site nvarchar(50) = null,
	@ItemID nvarchar(50) = null,
	@Quantity decimal(18, 4) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Balances]
					WHERE
					(
						(@WareHouseID is null OR [Balances].[WareHouseID] like @WareHouseID)
						AND (@Site is null OR [Balances].[Site] like @Site)
						AND (@ItemID is null OR [Balances].[ItemID] like @ItemID)
						AND (@Quantity is null OR [Balances].[Quantity] = @Quantity)
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
			SELECT [dbo].[Balances].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'WareHouseID' and @SortType = 1 THEN [Balances].[WareHouseID] END ASC,
						CASE WHEN @SortBy = 'WareHouseID' and @SortType = 0 THEN [Balances].[WareHouseID] END DESC,
						CASE WHEN @SortBy = 'Site' and @SortType = 1 THEN [Balances].[Site] END ASC,
						CASE WHEN @SortBy = 'Site' and @SortType = 0 THEN [Balances].[Site] END DESC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 1 THEN [Balances].[ItemID] END ASC,
						CASE WHEN @SortBy = 'ItemID' and @SortType = 0 THEN [Balances].[ItemID] END DESC,
						CASE WHEN @SortBy = 'Quantity' and @SortType = 1 THEN [Balances].[Quantity] END ASC,
						CASE WHEN @SortBy = 'Quantity' and @SortType = 0 THEN [Balances].[Quantity] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Balances]
			Where 
			(
				(@WareHouseID is null OR [Balances].[WareHouseID] like @WareHouseID)
				AND (@Site is null OR [Balances].[Site] like @Site)
				AND (@ItemID is null OR [Balances].[ItemID] like @ItemID)
				AND (@Quantity is null OR [Balances].[Quantity] = @Quantity)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Balances_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Balances_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Balances_Insert]
	@WareHouseID nvarchar(50),
	@Site nvarchar(50),
	@ItemID nvarchar(50),
	@Quantity decimal(18, 4)
AS


INSERT INTO [dbo].[Balances] (
	[WareHouseID],
	[Site],
	[ItemID],
	[Quantity]
) VALUES (
	@WareHouseID,
	@Site,
	@ItemID,
	@Quantity
)

--end [dbo].[Balances_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Balances_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Balances_Update]
	@WareHouseID nvarchar(50),
	@Site nvarchar(50),
	@ItemID nvarchar(50),
	@Quantity decimal(18, 4)
AS


UPDATE [dbo].[Balances] SET
	[Quantity] = @Quantity
WHERE
	[WareHouseID] = @WareHouseID
	AND [Site] = @Site
	AND [ItemID] = @ItemID

--end [dbo].[Balances_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Balances_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Balances_Delete]
	@WareHouseID nvarchar(50),
	@Site nvarchar(50),
	@ItemID nvarchar(50)
AS


DELETE FROM [dbo].[Balances]
WHERE
(
	[WareHouseID] = @WareHouseID
	AND [Site] = @Site
	AND [ItemID] = @ItemID
)

--end [dbo].[Balances_Delete]
--endregion

GO
--=========================================================================================--

