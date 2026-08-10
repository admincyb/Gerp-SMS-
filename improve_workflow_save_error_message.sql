DECLARE @sql nvarchar(max) = OBJECT_DEFINITION(OBJECT_ID(N'dbo.WorkFlowSave'));
DECLARE @p int;
DECLARE @insertAt int;

IF @sql IS NULL
    THROW 50000, 'Procedure dbo.WorkFlowSave not found.', 1;

SET @sql = REPLACE(@sql, N'CREATE PROCEDURE [dbo].[WorkFlowSave]', N'ALTER PROCEDURE [dbo].[WorkFlowSave]');

IF CHARINDEX(N'CodexDebug: preserve workflow job failure message', @sql) = 0
BEGIN
    SET @p = CHARINDEX(N'IF	( @ErrorNumber < 0)', @sql);
    SET @insertAt = CHARINDEX(N'--ROLLBACK TRAN', @sql, @p);

    IF @p = 0 OR @insertAt = 0
        THROW 50001, 'Could not find workflow job failure block.', 1;

    SET @sql = STUFF(
        @sql,
        @insertAt,
        0,
        N'-- CodexDebug: preserve workflow job failure message' + CHAR(13) + CHAR(10) +
        N'			SET @ErrorMessage = CONCAT(N''Workflow job failed: '', ISNULL(@JobProcedure, N''''), N'', RetVal='', ISNULL(CONVERT(NVARCHAR(20), @ErrorNumber), N''NULL''));' + CHAR(13) + CHAR(10) +
        N'			'
    );
END;

IF CHARINDEX(N'CodexDebug: preserve workflow transaction failure message', @sql) = 0
BEGIN
    SET @p = CHARINDEX(N'IF @ErrorNumber < 0', @sql);
    SET @insertAt = CHARINDEX(N'--ROLLBACK TRAN', @sql, @p);

    IF @p = 0 OR @insertAt = 0
        THROW 50002, 'Could not find workflow transaction failure block.', 1;

    SET @sql = STUFF(
        @sql,
        @insertAt,
        0,
        N'-- CodexDebug: preserve workflow transaction failure message' + CHAR(13) + CHAR(10) +
        N'		SET @ErrorMessage = CONCAT(N''WkfTransactionSave failed, RetVal='', ISNULL(CONVERT(NVARCHAR(20), @ErrorNumber), N''NULL''));' + CHAR(13) + CHAR(10) +
        N'		'
    );
END;

IF CHARINDEX(N'Workflow job failed:', @sql) = 0
    THROW 50003, 'Could not add workflow job failure message.', 1;

EXEC sys.sp_executesql @sql;

SELECT 'WorkFlowSave error message improved' AS Result;
