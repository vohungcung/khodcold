--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[CustomerSigns_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[CustomerSigns_Insert]

IF OBJECT_ID(N'[dbo].[CustomerSigns_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[CustomerSigns_Update]

IF OBJECT_ID(N'[dbo].[CustomerSigns_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[CustomerSigns_Delete]

IF OBJECT_ID(N'[dbo].[CustomerSigns_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[CustomerSigns_Select]

--endregion

GO


--region [dbo].[CustomerSigns_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[CustomerSigns_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@CustomerID nvarchar(50) = null,
	@SignImage image = null,
	@Ext nvarchar(5) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[CustomerSigns]
					WHERE
					(
						(@CustomerID is null OR [CustomerSigns].[CustomerID] like @CustomerID)
						AND (@Ext is null OR [CustomerSigns].[Ext] like @Ext)
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
			SELECT [dbo].[CustomerSigns].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 1 THEN [CustomerSigns].[CustomerID] END ASC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 0 THEN [CustomerSigns].[CustomerID] END DESC,
						CASE WHEN @SortBy = 'SignImage' and @SortType = 1 THEN [CustomerSigns].[SignImage] END ASC,
						CASE WHEN @SortBy = 'SignImage' and @SortType = 0 THEN [CustomerSigns].[SignImage] END DESC,
						CASE WHEN @SortBy = 'Ext' and @SortType = 1 THEN [CustomerSigns].[Ext] END ASC,
						CASE WHEN @SortBy = 'Ext' and @SortType = 0 THEN [CustomerSigns].[Ext] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[CustomerSigns]
			Where 
			(
				(@CustomerID is null OR [CustomerSigns].[CustomerID] like @CustomerID)
				AND (@Ext is null OR [CustomerSigns].[Ext] like @Ext)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[CustomerSigns_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[CustomerSigns_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[CustomerSigns_Insert]
	@CustomerID nvarchar(50),
	@SignImage image,
	@Ext nvarchar(5)
AS


INSERT INTO [dbo].[CustomerSigns] (
	[CustomerID],
	[SignImage],
	[Ext]
) VALUES (
	@CustomerID,
	@SignImage,
	@Ext
)

--end [dbo].[CustomerSigns_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[CustomerSigns_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[CustomerSigns_Update]
	@CustomerID nvarchar(50),
	@SignImage image,
	@Ext nvarchar(5)
AS


UPDATE [dbo].[CustomerSigns] SET
	[SignImage] = @SignImage,
	[Ext] = @Ext
WHERE
	[CustomerID] = @CustomerID

--end [dbo].[CustomerSigns_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[CustomerSigns_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[CustomerSigns_Delete]
	@CustomerID nvarchar(50)
AS


DELETE FROM [dbo].[CustomerSigns]
WHERE
(
	[CustomerID] = @CustomerID
)

--end [dbo].[CustomerSigns_Delete]
--endregion

GO
--=========================================================================================--

