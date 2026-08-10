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

DECLARE @sql nvarchar(max);

SET @sql = OBJECT_DEFINITION(OBJECT_ID(N'dbo.WorkFlowJobAutoTranApproveStatus'));
SET @sql = REPLACE(@sql, N'CREATE PROCEDURE [dbo].[WorkFlowJobAutoTranApproveStatus]', N'ALTER PROCEDURE [dbo].[WorkFlowJobAutoTranApproveStatus]');

IF CHARINDEX(N'CodexDebug: WorkFlowJobAutoTranApproveStatus failure', @sql) = 0
BEGIN
    SET @sql = REPLACE(
        @sql,
        N'StockTransferApproveFailure:
	IF @V_TRANSACTION_STARTED = 1 AND XACT_STATE() <> 0
		ROLLBACK TRANSACTION;
	ELSE IF @V_TRANSACTION_STARTED = 0 AND XACT_STATE() = 1
		ROLLBACK TRANSACTION StockTransferApprove;
	RETURN;',
        N'StockTransferApproveFailure:
	IF @V_TRANSACTION_STARTED = 1 AND XACT_STATE() <> 0
		ROLLBACK TRANSACTION;
	ELSE IF @V_TRANSACTION_STARTED = 0 AND XACT_STATE() = 1
		ROLLBACK TRANSACTION StockTransferApprove;

	-- CodexDebug: WorkFlowJobAutoTranApproveStatus failure
	INSERT INTO dbo.CodexGrnVerifyDebugLog
		(ProcName, AppId, RefId, RetVal, ErrorMessage)
	VALUES
		(N''WorkFlowJobAutoTranApproveStatus'', @pAppID, @pRefID, @pRetVal, N''Returned through StockTransferApproveFailure'');
	RETURN;'
    );

    SET @sql = REPLACE(
        @sql,
        N'SET @pRetVal	= -1
		SELECT	ERROR_LINE()	ErrorLine,	ERROR_MESSAGE()		ErrorMessage,
				ERROR_NUMBER()	ErrorNo,	ERROR_PROCEDURE()	ErrorProcedure',
        N'SET @pRetVal	= -1
		INSERT INTO dbo.CodexGrnVerifyDebugLog
			(ProcName, AppId, RefId, RetVal, ErrorLine, ErrorNumber, ErrorProcedure, ErrorMessage)
		VALUES
			(N''WorkFlowJobAutoTranApproveStatus'', @pAppID, @pRefID, @pRetVal, ERROR_LINE(), ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_MESSAGE());
		SELECT	ERROR_LINE()	ErrorLine,	ERROR_MESSAGE()		ErrorMessage,
				ERROR_NUMBER()	ErrorNo,	ERROR_PROCEDURE()	ErrorProcedure'
    );
    EXEC sys.sp_executesql @sql;
END;

SET @sql = OBJECT_DEFINITION(OBJECT_ID(N'dbo.WorkFlowJobGoodsReceiptNoteApproveStatusAuto'));
SET @sql = REPLACE(@sql, N'CREATE PROCEDURE [dbo].[WorkFlowJobGoodsReceiptNoteApproveStatusAuto]', N'ALTER PROCEDURE [dbo].[WorkFlowJobGoodsReceiptNoteApproveStatusAuto]');

IF CHARINDEX(N'CodexDebug: WorkFlowJobGoodsReceiptNoteApproveStatusAuto catch', @sql) = 0
BEGIN
    SET @sql = REPLACE(
        @sql,
        N'BEGIN CATCH
		SET	@pRetVal = -1
	END CATCH',
        N'BEGIN CATCH
		SET	@pRetVal = -1
		-- CodexDebug: WorkFlowJobGoodsReceiptNoteApproveStatusAuto catch
		INSERT INTO dbo.CodexGrnVerifyDebugLog
			(ProcName, AppId, RefId, RetVal, ErrorLine, ErrorNumber, ErrorProcedure, ErrorMessage)
		VALUES
			(N''WorkFlowJobGoodsReceiptNoteApproveStatusAuto'', @pAppID, @pRefID, @pRetVal, ERROR_LINE(), ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_MESSAGE());
	END CATCH'
    );
    EXEC sys.sp_executesql @sql;
END;

SET @sql = OBJECT_DEFINITION(OBJECT_ID(N'dbo.WorkFlowJobAutoGoodsReceiptApproveStatus'));
SET @sql = REPLACE(@sql, N'CREATE PROCEDURE [dbo].[WorkFlowJobAutoGoodsReceiptApproveStatus]', N'ALTER PROCEDURE [dbo].[WorkFlowJobAutoGoodsReceiptApproveStatus]');

IF CHARINDEX(N'CodexDebug: WorkFlowJobAutoGoodsReceiptApproveStatus catch', @sql) = 0
BEGIN
    SET @sql = REPLACE(
        @sql,
        N'BEGIN CATCH
		SET	@pRetVal = -1
	END CATCH',
        N'BEGIN CATCH
		SET	@pRetVal = -1
		-- CodexDebug: WorkFlowJobAutoGoodsReceiptApproveStatus catch
		INSERT INTO dbo.CodexGrnVerifyDebugLog
			(ProcName, AppId, RefId, RetVal, ErrorLine, ErrorNumber, ErrorProcedure, ErrorMessage)
		VALUES
			(N''WorkFlowJobAutoGoodsReceiptApproveStatus'', @pAppID, @pRefID, @pRetVal, ERROR_LINE(), ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_MESSAGE());
	END CATCH'
    );
    EXEC sys.sp_executesql @sql;
END;

SELECT 'GRN debug logging installed' AS Result;
