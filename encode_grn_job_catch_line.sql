DECLARE @sql nvarchar(max) = OBJECT_DEFINITION(OBJECT_ID(N'dbo.WorkFlowJobGoodsReceiptNoteApproveStatusAuto'));
DECLARE @catchStart int;
DECLARE @setStart int;
DECLARE @lineEnd int;

IF @sql IS NULL
    THROW 50000, 'Procedure dbo.WorkFlowJobGoodsReceiptNoteApproveStatusAuto not found.', 1;

SET @sql = REPLACE(
    @sql,
    N'CREATE PROCEDURE [dbo].[WorkFlowJobGoodsReceiptNoteApproveStatusAuto]',
    N'ALTER PROCEDURE [dbo].[WorkFlowJobGoodsReceiptNoteApproveStatusAuto]'
);

IF CHARINDEX(N'CodexDebug: encode GRN verify catch line', @sql) = 0
BEGIN
    SET @catchStart = CHARINDEX(N'BEGIN CATCH', @sql);
    SET @setStart = CHARINDEX(N'SET', @sql, @catchStart);
    SET @lineEnd = CHARINDEX(CHAR(13) + CHAR(10), @sql, @setStart);

    IF @catchStart = 0 OR @setStart = 0 OR @lineEnd = 0
        THROW 50001, 'Could not find GRN verify catch SET line.', 1;

    SET @sql = STUFF(
        @sql,
        @setStart,
        @lineEnd - @setStart,
        N'-- CodexDebug: encode GRN verify catch line' + CHAR(13) + CHAR(10) +
        N'		SET	@pRetVal = -900000 - ERROR_LINE()'
    );
END;

IF CHARINDEX(N'encode GRN verify catch line', @sql) = 0
    THROW 50002, 'Could not update GRN verify catch line encoding.', 1;

EXEC sys.sp_executesql @sql;

SELECT 'GRN verify catch now encodes ERROR_LINE in RetVal' AS Result;
