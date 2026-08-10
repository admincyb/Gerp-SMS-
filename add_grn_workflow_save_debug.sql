IF OBJECT_ID(N'dbo.CodexGrnVerifyDebugLog', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.CodexGrnVerifyDebugLog
    (
        Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        CreatedDate datetime NOT NULL CONSTRAINT DF_CodexGrnVerifyDebugLog_CreatedDate DEFAULT GETDATE(),
        ProcName sysname NOT NULL,
        AppId int NULL,
        RefId int NULL,
        RetVal int NULL,
        ErrorLine int NULL,
        ErrorNumber int NULL,
        ErrorProcedure sysname NULL,
        ErrorMessage nvarchar(4000) NULL
    );
END;
GO

DECLARE @sql nvarchar(max) = OBJECT_DEFINITION(OBJECT_ID(N'dbo.GoodsReceiptNoteWorkFlowSave'));
DECLARE @p int;
DECLARE @rollback int;
DECLARE @lineEnd int;

IF @sql IS NULL
    THROW 50000, 'Procedure dbo.GoodsReceiptNoteWorkFlowSave not found.', 1;

SET @sql = REPLACE(
    @sql,
    N'CREATE PROCEDURE [dbo].[GoodsReceiptNoteWorkFlowSave]',
    N'ALTER PROCEDURE [dbo].[GoodsReceiptNoteWorkFlowSave]'
);

IF CHARINDEX(N'CodexDebug: GoodsReceiptNoteWorkFlowSave app save failure', @sql) = 0
BEGIN
    SET @p = CHARINDEX(N'IF (@ErrorNumber<= 0)', @sql);
    SET @rollback = CHARINDEX(N'ROLLBACK TRANSACTION', @sql, @p);
    SET @lineEnd = CHARINDEX(CHAR(13) + CHAR(10), @sql, @rollback);

    IF @p = 0 OR @rollback = 0 OR @lineEnd = 0
        THROW 50001, 'Could not find app save rollback block.', 1;

    SET @sql = STUFF(
        @sql,
        @lineEnd,
        0,
        CHAR(13) + CHAR(10) +
        N'				-- CodexDebug: GoodsReceiptNoteWorkFlowSave app save failure' + CHAR(13) + CHAR(10) +
        N'				INSERT INTO dbo.CodexGrnVerifyDebugLog' + CHAR(13) + CHAR(10) +
        N'					(ProcName, AppId, RefId, RetVal, ErrorNumber, ErrorMessage)' + CHAR(13) + CHAR(10) +
        N'				VALUES' + CHAR(13) + CHAR(10) +
        N'					(N''GoodsReceiptNoteWorkFlowSave.AppSave'', @V_WKF_APPLICATION, @V_WKF_REFERENCE, @ErrorNumber, @ErrorNumber, @ErrorMessage);'
    );
END;

IF CHARINDEX(N'CodexDebug: GoodsReceiptNoteWorkFlowSave workflow failure', @sql) = 0
BEGIN
    SET @p = CHARINDEX(N'IF (@ErrorNumber < 0)', @sql);
    SET @rollback = CHARINDEX(N'ROLLBACK TRANSACTION', @sql, @p);
    SET @lineEnd = CHARINDEX(CHAR(13) + CHAR(10), @sql, @rollback);

    IF @p = 0 OR @rollback = 0 OR @lineEnd = 0
        THROW 50002, 'Could not find workflow rollback block.', 1;

    SET @sql = STUFF(
        @sql,
        @lineEnd,
        0,
        CHAR(13) + CHAR(10) +
        N'				-- CodexDebug: GoodsReceiptNoteWorkFlowSave workflow failure' + CHAR(13) + CHAR(10) +
        N'				INSERT INTO dbo.CodexGrnVerifyDebugLog' + CHAR(13) + CHAR(10) +
        N'					(ProcName, AppId, RefId, RetVal, ErrorNumber, ErrorMessage)' + CHAR(13) + CHAR(10) +
        N'				VALUES' + CHAR(13) + CHAR(10) +
        N'					(N''GoodsReceiptNoteWorkFlowSave.WorkflowSave'', @V_WKF_APPLICATION, @V_WKF_REFERENCE, @ErrorNumber, @ErrorNumber, @ErrorMessage);'
    );
END;

IF CHARINDEX(N'CodexDebug: GoodsReceiptNoteWorkFlowSave catch', @sql) = 0
BEGIN
    SET @p = CHARINDEX(N'BEGIN CATCH', @sql);
    SET @rollback = CHARINDEX(N'ROLLBACK TRANSACTION', @sql, @p);
    SET @lineEnd = CHARINDEX(CHAR(13) + CHAR(10), @sql, @rollback);

    IF @p = 0 OR @rollback = 0 OR @lineEnd = 0
        THROW 50003, 'Could not find catch rollback block.', 1;

    SET @sql = STUFF(
        @sql,
        @lineEnd,
        0,
        CHAR(13) + CHAR(10) +
        N'			-- CodexDebug: GoodsReceiptNoteWorkFlowSave catch' + CHAR(13) + CHAR(10) +
        N'			INSERT INTO dbo.CodexGrnVerifyDebugLog' + CHAR(13) + CHAR(10) +
        N'				(ProcName, AppId, RefId, RetVal, ErrorLine, ErrorNumber, ErrorProcedure, ErrorMessage)' + CHAR(13) + CHAR(10) +
        N'			VALUES' + CHAR(13) + CHAR(10) +
        N'				(N''GoodsReceiptNoteWorkFlowSave.Catch'', @V_WKF_APPLICATION, @V_WKF_REFERENCE, -1, ERROR_LINE(), ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_MESSAGE());'
    );
END;

IF CHARINDEX(N'CodexDebug: GoodsReceiptNoteWorkFlowSave', @sql) = 0
    THROW 50004, 'Debug logging was not added to GoodsReceiptNoteWorkFlowSave.', 1;

EXEC sys.sp_executesql @sql;

SELECT 'GoodsReceiptNoteWorkFlowSave debug logging installed' AS Result;
