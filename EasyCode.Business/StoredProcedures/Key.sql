--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Keys_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Keys_Insert]

IF OBJECT_ID(N'[dbo].[Keys_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Keys_Update]

IF OBJECT_ID(N'[dbo].[Keys_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Keys_Delete]

IF OBJECT_ID(N'[dbo].[Keys_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Keys_Select]

--endregion

GO


--region [dbo].[Keys_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Keys_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@CreateDate datetime = null,
	@MaxNumber int = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Keys]
					WHERE
					(
						(@CreateDate is null OR [Keys].[CreateDate] = @CreateDate)
						AND (@MaxNumber is null OR [Keys].[MaxNumber] = @MaxNumber)
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
			SELECT [dbo].[Keys].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'CreateDate' and @SortType = 1 THEN [Keys].[CreateDate] END ASC,
						CASE WHEN @SortBy = 'CreateDate' and @SortType = 0 THEN [Keys].[CreateDate] END DESC,
						CASE WHEN @SortBy = 'MaxNumber' and @SortType = 1 THEN [Keys].[MaxNumber] END ASC,
						CASE WHEN @SortBy = 'MaxNumber' and @SortType = 0 THEN [Keys].[MaxNumber] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Keys]
			Where 
			(
				(@CreateDate is null OR [Keys].[CreateDate] = @CreateDate)
				AND (@MaxNumber is null OR [Keys].[MaxNumber] = @MaxNumber)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Keys_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Keys_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Keys_Insert]
	@CreateDate datetime,
	@MaxNumber int
AS


INSERT INTO [dbo].[Keys] (
	[CreateDate],
	[MaxNumber]
) VALUES (
	@CreateDate,
	@MaxNumber
)

--end [dbo].[Keys_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Keys_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Keys_Update]
	@CreateDate datetime,
	@MaxNumber int
AS


UPDATE [dbo].[Keys] SET
	[MaxNumber] = @MaxNumber
WHERE
	[CreateDate] = @CreateDate

--end [dbo].[Keys_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Keys_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Keys_Delete]
	@CreateDate datetime
AS


DELETE FROM [dbo].[Keys]
WHERE
(
	[CreateDate] = @CreateDate
)

--end [dbo].[Keys_Delete]
--endregion

GO
--=========================================================================================--

