--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[CustomerTypes_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[CustomerTypes_Insert]

IF OBJECT_ID(N'[dbo].[CustomerTypes_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[CustomerTypes_Update]

IF OBJECT_ID(N'[dbo].[CustomerTypes_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[CustomerTypes_Delete]

IF OBJECT_ID(N'[dbo].[CustomerTypes_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[CustomerTypes_Select]

--endregion

GO


--region [dbo].[CustomerTypes_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[CustomerTypes_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@CustomerTypeID nvarchar(50) = null,
	@CustomerTypeName nvarchar(250) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[CustomerTypes]
					WHERE
					(
						(@CustomerTypeID is null OR [CustomerTypes].[CustomerTypeID] like @CustomerTypeID)
						AND (@CustomerTypeName is null OR [CustomerTypes].[CustomerTypeName] like @CustomerTypeName)
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
			SELECT [dbo].[CustomerTypes].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'CustomerTypeID' and @SortType = 1 THEN [CustomerTypes].[CustomerTypeID] END ASC,
						CASE WHEN @SortBy = 'CustomerTypeID' and @SortType = 0 THEN [CustomerTypes].[CustomerTypeID] END DESC,
						CASE WHEN @SortBy = 'CustomerTypeName' and @SortType = 1 THEN [CustomerTypes].[CustomerTypeName] END ASC,
						CASE WHEN @SortBy = 'CustomerTypeName' and @SortType = 0 THEN [CustomerTypes].[CustomerTypeName] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[CustomerTypes]
			Where 
			(
				(@CustomerTypeID is null OR [CustomerTypes].[CustomerTypeID] like @CustomerTypeID)
				AND (@CustomerTypeName is null OR [CustomerTypes].[CustomerTypeName] like @CustomerTypeName)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[CustomerTypes_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[CustomerTypes_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[CustomerTypes_Insert]
	@CustomerTypeID nvarchar(50),
	@CustomerTypeName nvarchar(250)
AS


INSERT INTO [dbo].[CustomerTypes] (
	[CustomerTypeID],
	[CustomerTypeName]
) VALUES (
	@CustomerTypeID,
	@CustomerTypeName
)

--end [dbo].[CustomerTypes_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[CustomerTypes_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[CustomerTypes_Update]
	@CustomerTypeID nvarchar(50),
	@CustomerTypeName nvarchar(250)
AS


UPDATE [dbo].[CustomerTypes] SET
	[CustomerTypeName] = @CustomerTypeName
WHERE
	[CustomerTypeID] = @CustomerTypeID

--end [dbo].[CustomerTypes_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[CustomerTypes_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[CustomerTypes_Delete]
	@CustomerTypeID nvarchar(50)
AS


DELETE FROM [dbo].[CustomerTypes]
WHERE
(
	[CustomerTypeID] = @CustomerTypeID
)

--end [dbo].[CustomerTypes_Delete]
--endregion

GO
--=========================================================================================--

