--region Drop Existing Procedures

IF OBJECT_ID(N'[dbo].[ResultDetail_Insert]') IS NOT NULL
	DROP PROCEDURE [dbo].[ResultDetail_Insert]

IF OBJECT_ID(N'[dbo].[ResultDetail_Update]') IS NOT NULL
	DROP PROCEDURE [dbo].[ResultDetail_Update]

IF OBJECT_ID(N'[dbo].[ResultDetail_Delete]') IS NOT NULL
	DROP PROCEDURE [dbo].[ResultDetail_Delete]

IF OBJECT_ID(N'[dbo].[ResultDetail_Select]') IS NOT NULL
	DROP PROCEDURE [dbo].[ResultDetail_Select]

--endregion

GO


--region [dbo].[ResultDetail_Select]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[ResultDetail_Select]
	@Page int = null,
	@PageSize int = null,
    @SortBy nvarchar(255) = null,
	@SortType bit = null,
	@ResultID nvarchar(50) = null,
	@AID int = null,
	@Content ntext = null
AS

SET TRANSACTION ISOLATION LEVEL READ COMMITTED

Declare @RowCount int  
Set @RowCount = (
					Select Count (*) 
					From [dbo].[ResultDetail]
					WHERE
					(
						(@ResultID is null OR [ResultDetail].[ResultID] like @ResultID)
						AND (@AID is null OR [ResultDetail].[AID] = @AID)
						AND (@Content is null OR [ResultDetail].[Content] like @Content)
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
			SELECT [dbo].[ResultDetail].*,
                   ROW_NUMBER() OVER 
                   (ORDER BY
                        CASE WHEN @SortBy = 'DBNull' THEN NewID() END ASC,
						CASE WHEN @SortBy = 'ResultID' and @SortType = 1 THEN [ResultDetail].[ResultID] END ASC,
						CASE WHEN @SortBy = 'ResultID' and @SortType = 0 THEN [ResultDetail].[ResultID] END DESC,
						CASE WHEN @SortBy = 'AID' and @SortType = 1 THEN [ResultDetail].[AID] END ASC,
						CASE WHEN @SortBy = 'AID' and @SortType = 0 THEN [ResultDetail].[AID] END DESC                   
                    ) AS RowNumber 
			FROM [dbo].[ResultDetail]
			Where 
			(
				(@ResultID is null OR [ResultDetail].[ResultID] like @ResultID)
				AND (@AID is null OR [ResultDetail].[AID] = @AID)
				AND (@Content is null OR [ResultDetail].[Content] like @Content)
			)
		) AS Temp
WHERE  RowNumber Between (@Page * @PageSize - @PageSize + 1) And @Page * @PageSize
Return @RowCount


--end [dbo].[ResultDetail_Select]
--endregion

GO
--=========================================================================================--

	

--region [dbo].[ResultDetail_Insert]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[ResultDetail_Insert]
	@ResultID nvarchar(50),
	@AID int,
	@Content ntext
AS


INSERT INTO [dbo].[ResultDetail] (
	[ResultID],
	[AID],
	[Content]
) VALUES (
	@ResultID,
	@AID,
	@Content
)

--end [dbo].[ResultDetail_Insert]
--endregion

GO
--=========================================================================================--

--region [dbo].[ResultDetail_Update]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[ResultDetail_Update]
	@ResultID nvarchar(50),
	@AID int,
	@Content ntext
AS


UPDATE [dbo].[ResultDetail] SET
	[Content] = @Content
WHERE
	[ResultID] = @ResultID
	AND [AID] = @AID

--end [dbo].[ResultDetail_Update]
--endregion

GO
--=========================================================================================--

--region [dbo].[ResultDetail_Delete]

-- Create By: vohungcung
-- Date Generated: Monday, July 16, 2018

CREATE PROCEDURE [dbo].[ResultDetail_Delete]
	@ResultID nvarchar(50),
	@AID int
AS


DELETE FROM [dbo].[ResultDetail]
WHERE
(
	[ResultID] = @ResultID
	AND [AID] = @AID
)

--end [dbo].[ResultDetail_Delete]
--endregion

GO
--=========================================================================================--

