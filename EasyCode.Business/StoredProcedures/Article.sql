--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[Article_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[Article_Insert]

IF OBJECT_ID(N'[dbo].[Article_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[Article_Update]

IF OBJECT_ID(N'[dbo].[Article_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[Article_Delete]

IF OBJECT_ID(N'[dbo].[Article_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[Article_Select]

--endregion

GO


--region [dbo].[Article_Select]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Article_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@MaBC varchar(18) = null,
	@MaHH nvarchar(14) = null,
	@TenHH nvarchar(250) = null,
	@MaNhom varchar(3) = null,
	@Ma_Nhom varchar(2) = null,
	@HangCham varchar(2) = null,
	@DVT varchar(3) = null,
	@DonGia numeric(18, 0) = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[Article]
					WHERE
					(
						(@MaBC is null OR [Article].[MaBC] like @MaBC)
						AND (@MaHH is null OR [Article].[MaHH] like @MaHH)
						AND (@TenHH is null OR [Article].[TenHH] like @TenHH)
						AND (@MaNhom is null OR [Article].[MaNhom] like @MaNhom)
						AND (@Ma_Nhom is null OR [Article].[Ma_Nhom] like @Ma_Nhom)
						AND (@HangCham is null OR [Article].[HangCham] like @HangCham)
						AND (@DVT is null OR [Article].[DVT] like @DVT)
						AND (@DonGia is null OR [Article].[DonGia] = @DonGia)
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
			SELECT [dbo].[Article].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'MaBC' and @SortType = 1 THEN [Article].[MaBC] END ASC,
						CASE WHEN @SortBy = 'MaBC' and @SortType = 0 THEN [Article].[MaBC] END DESC,
						CASE WHEN @SortBy = 'MaHH' and @SortType = 1 THEN [Article].[MaHH] END ASC,
						CASE WHEN @SortBy = 'MaHH' and @SortType = 0 THEN [Article].[MaHH] END DESC,
						CASE WHEN @SortBy = 'TenHH' and @SortType = 1 THEN [Article].[TenHH] END ASC,
						CASE WHEN @SortBy = 'TenHH' and @SortType = 0 THEN [Article].[TenHH] END DESC,
						CASE WHEN @SortBy = 'MaNhom' and @SortType = 1 THEN [Article].[MaNhom] END ASC,
						CASE WHEN @SortBy = 'MaNhom' and @SortType = 0 THEN [Article].[MaNhom] END DESC,
						CASE WHEN @SortBy = 'Ma_Nhom' and @SortType = 1 THEN [Article].[Ma_Nhom] END ASC,
						CASE WHEN @SortBy = 'Ma_Nhom' and @SortType = 0 THEN [Article].[Ma_Nhom] END DESC,
						CASE WHEN @SortBy = 'HangCham' and @SortType = 1 THEN [Article].[HangCham] END ASC,
						CASE WHEN @SortBy = 'HangCham' and @SortType = 0 THEN [Article].[HangCham] END DESC,
						CASE WHEN @SortBy = 'DVT' and @SortType = 1 THEN [Article].[DVT] END ASC,
						CASE WHEN @SortBy = 'DVT' and @SortType = 0 THEN [Article].[DVT] END DESC,
						CASE WHEN @SortBy = 'DonGia' and @SortType = 1 THEN [Article].[DonGia] END ASC,
						CASE WHEN @SortBy = 'DonGia' and @SortType = 0 THEN [Article].[DonGia] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[Article]
			Where 
			(
				(@MaBC is null OR [Article].[MaBC] like @MaBC)
				AND (@MaHH is null OR [Article].[MaHH] like @MaHH)
				AND (@TenHH is null OR [Article].[TenHH] like @TenHH)
				AND (@MaNhom is null OR [Article].[MaNhom] like @MaNhom)
				AND (@Ma_Nhom is null OR [Article].[Ma_Nhom] like @Ma_Nhom)
				AND (@HangCham is null OR [Article].[HangCham] like @HangCham)
				AND (@DVT is null OR [Article].[DVT] like @DVT)
				AND (@DonGia is null OR [Article].[DonGia] = @DonGia)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[Article_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[Article_Insert]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Article_Insert]
	@MaBC varchar(18),
	@MaHH nvarchar(14),
	@TenHH nvarchar(250),
	@MaNhom varchar(3),
	@Ma_Nhom varchar(2),
	@HangCham varchar(2),
	@DVT varchar(3),
	@DonGia numeric(18, 0)
AS


INSERT INTO [dbo].[Article] (
	[MaBC],
	[MaHH],
	[TenHH],
	[MaNhom],
	[Ma_Nhom],
	[HangCham],
	[DVT],
	[DonGia]
) VALUES (
	@MaBC,
	@MaHH,
	@TenHH,
	@MaNhom,
	@Ma_Nhom,
	@HangCham,
	@DVT,
	@DonGia
)

--end [dbo].[Article_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[Article_Update]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Article_Update]
	@MaBC varchar(18),
	@MaHH nvarchar(14),
	@TenHH nvarchar(250),
	@MaNhom varchar(3),
	@Ma_Nhom varchar(2),
	@HangCham varchar(2),
	@DVT varchar(3),
	@DonGia numeric(18, 0)
AS


UPDATE [dbo].[Article] SET
	[MaBC] = @MaBC,
	[TenHH] = @TenHH,
	[MaNhom] = @MaNhom,
	[Ma_Nhom] = @Ma_Nhom,
	[HangCham] = @HangCham,
	[DVT] = @DVT,
	[DonGia] = @DonGia
WHERE
	[MaHH] = @MaHH

--end [dbo].[Article_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[Article_Delete]

-- Create By: Hung Cung
-- Date Generated: Friday, April 20, 2018

CREATE PROCEDURE [dbo].[Article_Delete]
	@MaHH nvarchar(14)
AS


DELETE FROM [dbo].[Article]
WHERE
(
	[MaHH] = @MaHH
)

--end [dbo].[Article_Delete]
--endregion

GO
--=========================================================================================--

