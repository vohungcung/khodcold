--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Customers_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Customers_Insert]

IF OBJECT_ID(N'[dbo].[Customers_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Customers_Update]

IF OBJECT_ID(N'[dbo].[Customers_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Customers_Delete]

IF OBJECT_ID(N'[dbo].[Customers_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Customers_Select]

--endregion

GO


--region [dbo].[Customers_Select]

-- Create By: Hung Cung
-- Date Generated: Thursday, April 26, 2018

CREATE PROCEDURE [dbo].[Customers_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@CustomerID nvarchar(250) = null,
	@CustomerName nvarchar(250) = null,
	@Address nvarchar(250) = null,
	@CustomerType nvarchar(50) = null,
	@Password nvarchar(50) = null,
	@Phone nvarchar(50) = null,
	@Email nvarchar(50) = null,
	@Area nvarchar(50) = null,
	@ZipCode nvarchar(50) = null,
	@ZO1 bit = null,
	@ZOD bit = null,
	@ZO2 bit = null,
	@ZOC bit = null,
	@IsMarketing bit = null,
	@District nvarchar(250) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Customers]
					WHERE
					(
						(@CustomerID is null OR [Customers].[CustomerID] like @CustomerID)
						AND (@CustomerName is null OR [Customers].[CustomerName] like @CustomerName)
						AND (@Address is null OR [Customers].[Address] like @Address)
						AND (@CustomerType is null OR [Customers].[CustomerType] like @CustomerType)
						AND (@Password is null OR [Customers].[Password] like @Password)
						AND (@Phone is null OR [Customers].[Phone] like @Phone)
						AND (@Email is null OR [Customers].[Email] like @Email)
						AND (@Area is null OR [Customers].[Area] like @Area)
						AND (@ZipCode is null OR [Customers].[ZipCode] like @ZipCode)
						AND (@ZO1 is null OR [Customers].[ZO1] = @ZO1)
						AND (@ZOD is null OR [Customers].[ZOD] = @ZOD)
						AND (@ZO2 is null OR [Customers].[ZO2] = @ZO2)
						AND (@ZOC is null OR [Customers].[ZOC] = @ZOC)
						AND (@IsMarketing is null OR [Customers].[IsMarketing] = @IsMarketing)
						AND (@District is null OR [Customers].[District] like @District)
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
			SELECT [dbo].[Customers].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 1 THEN [Customers].[CustomerID] END ASC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 0 THEN [Customers].[CustomerID] END DESC,
						CASE WHEN @SortBy = 'CustomerName' and @SortType = 1 THEN [Customers].[CustomerName] END ASC,
						CASE WHEN @SortBy = 'CustomerName' and @SortType = 0 THEN [Customers].[CustomerName] END DESC,
						CASE WHEN @SortBy = 'Address' and @SortType = 1 THEN [Customers].[Address] END ASC,
						CASE WHEN @SortBy = 'Address' and @SortType = 0 THEN [Customers].[Address] END DESC,
						CASE WHEN @SortBy = 'CustomerType' and @SortType = 1 THEN [Customers].[CustomerType] END ASC,
						CASE WHEN @SortBy = 'CustomerType' and @SortType = 0 THEN [Customers].[CustomerType] END DESC,
						CASE WHEN @SortBy = 'Password' and @SortType = 1 THEN [Customers].[Password] END ASC,
						CASE WHEN @SortBy = 'Password' and @SortType = 0 THEN [Customers].[Password] END DESC,
						CASE WHEN @SortBy = 'Phone' and @SortType = 1 THEN [Customers].[Phone] END ASC,
						CASE WHEN @SortBy = 'Phone' and @SortType = 0 THEN [Customers].[Phone] END DESC,
						CASE WHEN @SortBy = 'Email' and @SortType = 1 THEN [Customers].[Email] END ASC,
						CASE WHEN @SortBy = 'Email' and @SortType = 0 THEN [Customers].[Email] END DESC,
						CASE WHEN @SortBy = 'Area' and @SortType = 1 THEN [Customers].[Area] END ASC,
						CASE WHEN @SortBy = 'Area' and @SortType = 0 THEN [Customers].[Area] END DESC,
						CASE WHEN @SortBy = 'ZipCode' and @SortType = 1 THEN [Customers].[ZipCode] END ASC,
						CASE WHEN @SortBy = 'ZipCode' and @SortType = 0 THEN [Customers].[ZipCode] END DESC,
						CASE WHEN @SortBy = 'ZO1' and @SortType = 1 THEN [Customers].[ZO1] END ASC,
						CASE WHEN @SortBy = 'ZO1' and @SortType = 0 THEN [Customers].[ZO1] END DESC,
						CASE WHEN @SortBy = 'ZOD' and @SortType = 1 THEN [Customers].[ZOD] END ASC,
						CASE WHEN @SortBy = 'ZOD' and @SortType = 0 THEN [Customers].[ZOD] END DESC,
						CASE WHEN @SortBy = 'ZO2' and @SortType = 1 THEN [Customers].[ZO2] END ASC,
						CASE WHEN @SortBy = 'ZO2' and @SortType = 0 THEN [Customers].[ZO2] END DESC,
						CASE WHEN @SortBy = 'ZOC' and @SortType = 1 THEN [Customers].[ZOC] END ASC,
						CASE WHEN @SortBy = 'ZOC' and @SortType = 0 THEN [Customers].[ZOC] END DESC,
						CASE WHEN @SortBy = 'IsMarketing' and @SortType = 1 THEN [Customers].[IsMarketing] END ASC,
						CASE WHEN @SortBy = 'IsMarketing' and @SortType = 0 THEN [Customers].[IsMarketing] END DESC,
						CASE WHEN @SortBy = 'District' and @SortType = 1 THEN [Customers].[District] END ASC,
						CASE WHEN @SortBy = 'District' and @SortType = 0 THEN [Customers].[District] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Customers]
			Where 
			(
				(@CustomerID is null OR [Customers].[CustomerID] like @CustomerID)
				AND (@CustomerName is null OR [Customers].[CustomerName] like @CustomerName)
				AND (@Address is null OR [Customers].[Address] like @Address)
				AND (@CustomerType is null OR [Customers].[CustomerType] like @CustomerType)
				AND (@Password is null OR [Customers].[Password] like @Password)
				AND (@Phone is null OR [Customers].[Phone] like @Phone)
				AND (@Email is null OR [Customers].[Email] like @Email)
				AND (@Area is null OR [Customers].[Area] like @Area)
				AND (@ZipCode is null OR [Customers].[ZipCode] like @ZipCode)
				AND (@ZO1 is null OR [Customers].[ZO1] = @ZO1)
				AND (@ZOD is null OR [Customers].[ZOD] = @ZOD)
				AND (@ZO2 is null OR [Customers].[ZO2] = @ZO2)
				AND (@ZOC is null OR [Customers].[ZOC] = @ZOC)
				AND (@IsMarketing is null OR [Customers].[IsMarketing] = @IsMarketing)
				AND (@District is null OR [Customers].[District] like @District)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Customers_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Customers_Insert]

-- Create By: Hung Cung
-- Date Generated: Thursday, April 26, 2018

CREATE PROCEDURE [dbo].[Customers_Insert]
	@CustomerID nvarchar(250),
	@CustomerName nvarchar(250),
	@Address nvarchar(250),
	@CustomerType nvarchar(50),
	@Password nvarchar(50),
	@Phone nvarchar(50),
	@Email nvarchar(50),
	@Area nvarchar(50),
	@ZipCode nvarchar(50),
	@ZO1 bit,
	@ZOD bit,
	@ZO2 bit,
	@ZOC bit,
	@IsMarketing bit,
	@District nvarchar(250)
AS


INSERT INTO [dbo].[Customers] (
	[CustomerID],
	[CustomerName],
	[Address],
	[CustomerType],
	[Password],
	[Phone],
	[Email],
	[Area],
	[ZipCode],
	[ZO1],
	[ZOD],
	[ZO2],
	[ZOC],
	[IsMarketing],
	[District]
) VALUES (
	@CustomerID,
	@CustomerName,
	@Address,
	@CustomerType,
	@Password,
	@Phone,
	@Email,
	@Area,
	@ZipCode,
	@ZO1,
	@ZOD,
	@ZO2,
	@ZOC,
	@IsMarketing,
	@District
)

--end [dbo].[Customers_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Customers_Update]

-- Create By: Hung Cung
-- Date Generated: Thursday, April 26, 2018

CREATE PROCEDURE [dbo].[Customers_Update]
	@CustomerID nvarchar(250),
	@CustomerName nvarchar(250),
	@Address nvarchar(250),
	@CustomerType nvarchar(50),
	@Password nvarchar(50),
	@Phone nvarchar(50),
	@Email nvarchar(50),
	@Area nvarchar(50),
	@ZipCode nvarchar(50),
	@ZO1 bit,
	@ZOD bit,
	@ZO2 bit,
	@ZOC bit,
	@IsMarketing bit,
	@District nvarchar(250)
AS


UPDATE [dbo].[Customers] SET
	[CustomerName] = @CustomerName,
	[Address] = @Address,
	[CustomerType] = @CustomerType,
	[Password] = @Password,
	[Phone] = @Phone,
	[Email] = @Email,
	[Area] = @Area,
	[ZipCode] = @ZipCode,
	[ZO1] = @ZO1,
	[ZOD] = @ZOD,
	[ZO2] = @ZO2,
	[ZOC] = @ZOC,
	[IsMarketing] = @IsMarketing,
	[District] = @District
WHERE
	[CustomerID] = @CustomerID

--end [dbo].[Customers_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Customers_Delete]

-- Create By: Hung Cung
-- Date Generated: Thursday, April 26, 2018

CREATE PROCEDURE [dbo].[Customers_Delete]
	@CustomerID nvarchar(250)
AS


DELETE FROM [dbo].[Customers]
WHERE
(
	[CustomerID] = @CustomerID
)

--end [dbo].[Customers_Delete]
--endregion

GO
--=========================================================================================--

