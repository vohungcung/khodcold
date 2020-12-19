--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[OrderTypes_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[OrderTypes_Insert]

IF OBJECT_ID(N'[dbo].[OrderTypes_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[OrderTypes_Update]

IF OBJECT_ID(N'[dbo].[OrderTypes_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[OrderTypes_Delete]

IF OBJECT_ID(N'[dbo].[OrderTypes_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[OrderTypes_Select]

--endregion

GO


--region [dbo].[OrderTypes_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[OrderTypes_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@ID nvarchar(50) = null,
	@SAPType nvarchar(50) = null,
	@Description nvarchar(250) = null,
	@WareHouseID nvarchar(50) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[OrderTypes]
					WHERE
					(
						(@ID is null OR [OrderTypes].[ID] like @ID)
						AND (@SAPType is null OR [OrderTypes].[SAPType] like @SAPType)
						AND (@Description is null OR [OrderTypes].[Description] like @Description)
						AND (@WareHouseID is null OR [OrderTypes].[WareHouseID] like @WareHouseID)
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
			SELECT [dbo].[OrderTypes].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'ID' and @SortType = 1 THEN [OrderTypes].[ID] END ASC,
						CASE WHEN @SortBy = 'ID' and @SortType = 0 THEN [OrderTypes].[ID] END DESC,
						CASE WHEN @SortBy = 'SAPType' and @SortType = 1 THEN [OrderTypes].[SAPType] END ASC,
						CASE WHEN @SortBy = 'SAPType' and @SortType = 0 THEN [OrderTypes].[SAPType] END DESC,
						CASE WHEN @SortBy = 'Description' and @SortType = 1 THEN [OrderTypes].[Description] END ASC,
						CASE WHEN @SortBy = 'Description' and @SortType = 0 THEN [OrderTypes].[Description] END DESC,
						CASE WHEN @SortBy = 'WareHouseID' and @SortType = 1 THEN [OrderTypes].[WareHouseID] END ASC,
						CASE WHEN @SortBy = 'WareHouseID' and @SortType = 0 THEN [OrderTypes].[WareHouseID] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[OrderTypes]
			Where 
			(
				(@ID is null OR [OrderTypes].[ID] like @ID)
				AND (@SAPType is null OR [OrderTypes].[SAPType] like @SAPType)
				AND (@Description is null OR [OrderTypes].[Description] like @Description)
				AND (@WareHouseID is null OR [OrderTypes].[WareHouseID] like @WareHouseID)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[OrderTypes_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[OrderTypes_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[OrderTypes_Insert]
	@ID nvarchar(50),
	@SAPType nvarchar(50),
	@Description nvarchar(250),
	@WareHouseID nvarchar(50)
AS


INSERT INTO [dbo].[OrderTypes] (
	[ID],
	[SAPType],
	[Description],
	[WareHouseID]
) VALUES (
	@ID,
	@SAPType,
	@Description,
	@WareHouseID
)

--end [dbo].[OrderTypes_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[OrderTypes_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[OrderTypes_Update]
	@ID nvarchar(50),
	@SAPType nvarchar(50),
	@Description nvarchar(250),
	@WareHouseID nvarchar(50)
AS


UPDATE [dbo].[OrderTypes] SET
	[SAPType] = @SAPType,
	[Description] = @Description,
	[WareHouseID] = @WareHouseID
WHERE
	[ID] = @ID

--end [dbo].[OrderTypes_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[OrderTypes_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[OrderTypes_Delete]
	@ID nvarchar(50)
AS


DELETE FROM [dbo].[OrderTypes]
WHERE
(
	[ID] = @ID
)

--end [dbo].[OrderTypes_Delete]
--endregion

GO
--=========================================================================================--

