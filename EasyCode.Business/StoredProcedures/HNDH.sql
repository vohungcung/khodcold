--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[HNDH_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[HNDH_Insert]

IF OBJECT_ID(N'[dbo].[HNDH_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[HNDH_Update]

IF OBJECT_ID(N'[dbo].[HNDH_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[HNDH_Delete]

IF OBJECT_ID(N'[dbo].[HNDH_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[HNDH_Select]

--endregion

GO


--region [dbo].[HNDH_Select]

-- Create By: Hung Cung
-- Date Generated: Thursday, May 03, 2018

CREATE PROCEDURE [dbo].[HNDH_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@HNID nvarchar(50) = null,
	@Title nvarchar(250) = null,
	@VoucherDate datetime = null,
	@Site nvarchar(50) = null,
	@Status int = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[HNDH]
					WHERE
					(
						(@HNID is null OR [HNDH].[HNID] like @HNID)
						AND (@Title is null OR [HNDH].[Title] like @Title)
						AND (@VoucherDate is null OR [HNDH].[VoucherDate] = @VoucherDate)
						AND (@Site is null OR [HNDH].[Site] like @Site)
						AND (@Status is null OR [HNDH].[Status] = @Status)
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
			SELECT [dbo].[HNDH].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'HNID' and @SortType = 1 THEN [HNDH].[HNID] END ASC,
						CASE WHEN @SortBy = 'HNID' and @SortType = 0 THEN [HNDH].[HNID] END DESC,
						CASE WHEN @SortBy = 'Title' and @SortType = 1 THEN [HNDH].[Title] END ASC,
						CASE WHEN @SortBy = 'Title' and @SortType = 0 THEN [HNDH].[Title] END DESC,
						CASE WHEN @SortBy = 'VoucherDate' and @SortType = 1 THEN [HNDH].[VoucherDate] END ASC,
						CASE WHEN @SortBy = 'VoucherDate' and @SortType = 0 THEN [HNDH].[VoucherDate] END DESC,
						CASE WHEN @SortBy = 'Site' and @SortType = 1 THEN [HNDH].[Site] END ASC,
						CASE WHEN @SortBy = 'Site' and @SortType = 0 THEN [HNDH].[Site] END DESC,
						CASE WHEN @SortBy = 'Status' and @SortType = 1 THEN [HNDH].[Status] END ASC,
						CASE WHEN @SortBy = 'Status' and @SortType = 0 THEN [HNDH].[Status] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[HNDH]
			Where 
			(
				(@HNID is null OR [HNDH].[HNID] like @HNID)
				AND (@Title is null OR [HNDH].[Title] like @Title)
				AND (@VoucherDate is null OR [HNDH].[VoucherDate] = @VoucherDate)
				AND (@Site is null OR [HNDH].[Site] like @Site)
				AND (@Status is null OR [HNDH].[Status] = @Status)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[HNDH_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[HNDH_Insert]

-- Create By: Hung Cung
-- Date Generated: Thursday, May 03, 2018

CREATE PROCEDURE [dbo].[HNDH_Insert]
	@HNID nvarchar(50),
	@Title nvarchar(250),
	@VoucherDate datetime,
	@Site nvarchar(50),
	@Status int
AS


INSERT INTO [dbo].[HNDH] (
	[HNID],
	[Title],
	[VoucherDate],
	[Site],
	[Status]
) VALUES (
	@HNID,
	@Title,
	@VoucherDate,
	@Site,
	@Status
)

--end [dbo].[HNDH_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[HNDH_Update]

-- Create By: Hung Cung
-- Date Generated: Thursday, May 03, 2018

CREATE PROCEDURE [dbo].[HNDH_Update]
	@HNID nvarchar(50),
	@Title nvarchar(250),
	@VoucherDate datetime,
	@Site nvarchar(50),
	@Status int
AS


UPDATE [dbo].[HNDH] SET
	[Title] = @Title,
	[VoucherDate] = @VoucherDate,
	[Site] = @Site,
	[Status] = @Status
WHERE
	[HNID] = @HNID

--end [dbo].[HNDH_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[HNDH_Delete]

-- Create By: Hung Cung
-- Date Generated: Thursday, May 03, 2018

CREATE PROCEDURE [dbo].[HNDH_Delete]
	@HNID nvarchar(50)
AS


DELETE FROM [dbo].[HNDH]
WHERE
(
	[HNID] = @HNID
)

--end [dbo].[HNDH_Delete]
--endregion

GO
--=========================================================================================--

