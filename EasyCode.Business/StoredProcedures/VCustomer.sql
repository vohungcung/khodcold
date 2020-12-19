--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[VCustomers_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[VCustomers_Select]

--endregion

GO


--region [dbo].[VCustomers_Select]

-- Create By: Hung Cung
-- Date Generated: Thursday, April 26, 2018

CREATE PROCEDURE [dbo].[VCustomers_Select]
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
	@CustomerTypeName nvarchar(250) = null,
	@Area nvarchar(50) = null,
	@ZipCode nvarchar(50) = null,
	@ZO1 bit = null,
	@ZOD bit = null,
	@ZO2 bit = null,
	@ZOC bit = null,
	@Email nvarchar(50) = null,
	@IsMarketing bit = null,
	@District nvarchar(250) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[VCustomers]
					WHERE
					(
						(@CustomerID is null OR [VCustomers].[CustomerID] like @CustomerID)
						AND (@CustomerName is null OR [VCustomers].[CustomerName] like @CustomerName)
						AND (@Address is null OR [VCustomers].[Address] like @Address)
						AND (@CustomerType is null OR [VCustomers].[CustomerType] like @CustomerType)
						AND (@Password is null OR [VCustomers].[Password] like @Password)
						AND (@Phone is null OR [VCustomers].[Phone] like @Phone)
						AND (@CustomerTypeName is null OR [VCustomers].[CustomerTypeName] like @CustomerTypeName)
						AND (@Area is null OR [VCustomers].[Area] like @Area)
						AND (@ZipCode is null OR [VCustomers].[ZipCode] like @ZipCode)
						AND (@ZO1 is null OR [VCustomers].[ZO1] = @ZO1)
						AND (@ZOD is null OR [VCustomers].[ZOD] = @ZOD)
						AND (@ZO2 is null OR [VCustomers].[ZO2] = @ZO2)
						AND (@ZOC is null OR [VCustomers].[ZOC] = @ZOC)
						AND (@Email is null OR [VCustomers].[Email] like @Email)
						AND (@IsMarketing is null OR [VCustomers].[IsMarketing] = @IsMarketing)
						AND (@District is null OR [VCustomers].[District] like @District)
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
			SELECT [dbo].[VCustomers].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY 
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 1 THEN [VCustomers].[CustomerID] END ASC,
						CASE WHEN @SortBy = 'CustomerID' and @SortType = 0 THEN [VCustomers].[CustomerID] END DESC,
						CASE WHEN @SortBy = 'CustomerName' and @SortType = 1 THEN [VCustomers].[CustomerName] END ASC,
						CASE WHEN @SortBy = 'CustomerName' and @SortType = 0 THEN [VCustomers].[CustomerName] END DESC,
						CASE WHEN @SortBy = 'Address' and @SortType = 1 THEN [VCustomers].[Address] END ASC,
						CASE WHEN @SortBy = 'Address' and @SortType = 0 THEN [VCustomers].[Address] END DESC,
						CASE WHEN @SortBy = 'CustomerType' and @SortType = 1 THEN [VCustomers].[CustomerType] END ASC,
						CASE WHEN @SortBy = 'CustomerType' and @SortType = 0 THEN [VCustomers].[CustomerType] END DESC,
						CASE WHEN @SortBy = 'Password' and @SortType = 1 THEN [VCustomers].[Password] END ASC,
						CASE WHEN @SortBy = 'Password' and @SortType = 0 THEN [VCustomers].[Password] END DESC,
						CASE WHEN @SortBy = 'Phone' and @SortType = 1 THEN [VCustomers].[Phone] END ASC,
						CASE WHEN @SortBy = 'Phone' and @SortType = 0 THEN [VCustomers].[Phone] END DESC,
						CASE WHEN @SortBy = 'CustomerTypeName' and @SortType = 1 THEN [VCustomers].[CustomerTypeName] END ASC,
						CASE WHEN @SortBy = 'CustomerTypeName' and @SortType = 0 THEN [VCustomers].[CustomerTypeName] END DESC,
						CASE WHEN @SortBy = 'Area' and @SortType = 1 THEN [VCustomers].[Area] END ASC,
						CASE WHEN @SortBy = 'Area' and @SortType = 0 THEN [VCustomers].[Area] END DESC,
						CASE WHEN @SortBy = 'ZipCode' and @SortType = 1 THEN [VCustomers].[ZipCode] END ASC,
						CASE WHEN @SortBy = 'ZipCode' and @SortType = 0 THEN [VCustomers].[ZipCode] END DESC,
						CASE WHEN @SortBy = 'ZO1' and @SortType = 1 THEN [VCustomers].[ZO1] END ASC,
						CASE WHEN @SortBy = 'ZO1' and @SortType = 0 THEN [VCustomers].[ZO1] END DESC,
						CASE WHEN @SortBy = 'ZOD' and @SortType = 1 THEN [VCustomers].[ZOD] END ASC,
						CASE WHEN @SortBy = 'ZOD' and @SortType = 0 THEN [VCustomers].[ZOD] END DESC,
						CASE WHEN @SortBy = 'ZO2' and @SortType = 1 THEN [VCustomers].[ZO2] END ASC,
						CASE WHEN @SortBy = 'ZO2' and @SortType = 0 THEN [VCustomers].[ZO2] END DESC,
						CASE WHEN @SortBy = 'ZOC' and @SortType = 1 THEN [VCustomers].[ZOC] END ASC,
						CASE WHEN @SortBy = 'ZOC' and @SortType = 0 THEN [VCustomers].[ZOC] END DESC,
						CASE WHEN @SortBy = 'Email' and @SortType = 1 THEN [VCustomers].[Email] END ASC,
						CASE WHEN @SortBy = 'Email' and @SortType = 0 THEN [VCustomers].[Email] END DESC,
						CASE WHEN @SortBy = 'IsMarketing' and @SortType = 1 THEN [VCustomers].[IsMarketing] END ASC,
						CASE WHEN @SortBy = 'IsMarketing' and @SortType = 0 THEN [VCustomers].[IsMarketing] END DESC,
						CASE WHEN @SortBy = 'District' and @SortType = 1 THEN [VCustomers].[District] END ASC,
						CASE WHEN @SortBy = 'District' and @SortType = 0 THEN [VCustomers].[District] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[VCustomers]
			Where 
			(
				(@CustomerID is null OR [VCustomers].[CustomerID] like @CustomerID)
				AND (@CustomerName is null OR [VCustomers].[CustomerName] like @CustomerName)
				AND (@Address is null OR [VCustomers].[Address] like @Address)
				AND (@CustomerType is null OR [VCustomers].[CustomerType] like @CustomerType)
				AND (@Password is null OR [VCustomers].[Password] like @Password)
				AND (@Phone is null OR [VCustomers].[Phone] like @Phone)
				AND (@CustomerTypeName is null OR [VCustomers].[CustomerTypeName] like @CustomerTypeName)
				AND (@Area is null OR [VCustomers].[Area] like @Area)
				AND (@ZipCode is null OR [VCustomers].[ZipCode] like @ZipCode)
				AND (@ZO1 is null OR [VCustomers].[ZO1] = @ZO1)
				AND (@ZOD is null OR [VCustomers].[ZOD] = @ZOD)
				AND (@ZO2 is null OR [VCustomers].[ZO2] = @ZO2)
				AND (@ZOC is null OR [VCustomers].[ZOC] = @ZOC)
				AND (@Email is null OR [VCustomers].[Email] like @Email)
				AND (@IsMarketing is null OR [VCustomers].[IsMarketing] = @IsMarketing)
				AND (@District is null OR [VCustomers].[District] like @District)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[VCustomers_Select]
--endregion

GO
--=========================================================================================--


