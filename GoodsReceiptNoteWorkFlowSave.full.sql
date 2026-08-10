
/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	Author	:	GTI-261
	Date	:	03-02-2025
	Purpose :	Save application trx and workflow trx.
	Execute :	[GoodsReceiptNoteWorkFlowSave]
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	Modified By				On				Remarks
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	<Next Entry>
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
CREATE PROCEDURE [dbo].[GoodsReceiptNoteWorkFlowSave]
(
		@Json				NVARCHAR(MAX)		
	,	@GUID				INT					=	NULL	OUTPUT
	,	@ErrorNumber		INT					=	NULL	OUTPUT	
	,	@ErrorMessage		NVARCHAR(2000)		=	NULL	OUTPUT 	
	
)
AS 
BEGIN

	DECLARE	 @V_DOC_HANDLE			INT			
			,@V_WKF_REFERENCE		INT	
			,@V_WKF_APPLICATION		INT
			,@V_WKF_PROCESS			INT
			,@V_WKF_TASK			INT
			,@V_WKF_TASK_ACTION		INT
			,@V_WKF_USER_CSV		NVARCHAR(MAX)	=	NULL
			,@V_WKF_COMMENTS		NVARCHAR(MAX)	=	NULL
			,@V_WKF_TRX_FLAG		INT			--(0) SAVE transaction (1) SAVE transaction & SUBMIT workflow (2) SUBMIT workflow
			,@V_USER_PK				INT
			,@ApplicationNo			NVARCHAR(100)
			,@DivisionGroup			INT

	BEGIN TRY
		BEGIN TRANSACTION


		SELECT   @V_WKF_REFERENCE		=	[WKF_REFERENCE]
				,@V_WKF_APPLICATION		=	[WKF_APPLICATION]
				,@V_WKF_PROCESS			=	[WKF_PROCESS]
				--,@V_WKF_TASK			=	[WKF_TASK]
				,@V_WKF_TASK_ACTION		=	[WKF_TASK_ACTION]	
				,@V_WKF_COMMENTS		=	[WKF_COMMENTS]
				,@V_WKF_TRX_FLAG		=	[WKF_TRX_FLAG]
				,@V_USER_PK				=	[USER_PK]
				,@DivisionGroup			=	[DivisionGroup]

		FROM	OPENJSON(@JSON)
		WITH
			(	
				 [WKF_REFERENCE]		INT				N'$.Refid'
				,[WKF_APPLICATION]		INT				N'$.Id'
				,[WKF_PROCESS]			INT				N'$.ProcessId'
				--,[WKF_TASK]				INT				N'$.Task'
				,[WKF_TASK_ACTION]		INT				N'$.ProcessActionId'
				,[WKF_COMMENTS]			NVARCHAR(MAX)	N'$.WorkflowComment'
				,[WKF_TRX_FLAG]			INT				N'$.WorkflowFlag'
				,[USER_PK]				INT				N'$.UserId'
				,[DivisionGroup]		INT				N'$.DivisionGroup'

			)
		SET	@ErrorNumber	=	0
		----------------------------
		--(1.) Application Trx Save
		----------------------------
		IF @V_WKF_TRX_FLAG IN (0,1)
		BEGIN
			EXEC	[GoodsReceiptNoteSave]
					 @Json			=	@Json	
					,@GUID			=	@GUID			OUTPUT
					,@ErrorNumber	=	@ErrorNumber	OUTPUT
					,@ErrorMessage	=	@ErrorMessage	OUTPUT
			IF (@ErrorNumber<= 0)
			BEGIN
				ROLLBACK TRANSACTION
				EXEC [TransactionErrorLogs] @UserId = @V_USER_PK,@ProcessId = @V_WKF_PROCESS,@ProcessActionId=@V_WKF_TASK_ACTION,@ParamJson = @Json,@ErrorNumber = @ErrorNumber
				RETURN
			END
			SELECT	@V_WKF_APPLICATION	=	@GUID
			SET @ApplicationNo = @ErrorMessage
		END
		----------------------------
		--(2.) Workflow Trx Save
		----------------------------
		IF @V_WKF_TRX_FLAG IN (1,2)
		BEGIN

			EXEC	[WorkFlowSave]
					 @RefId					=	@V_WKF_REFERENCE
					,@ProcessID				=	@V_WKF_PROCESS
					,@ApplicationId			=	@V_WKF_APPLICATION
					,@ApplicationNo			=	@ApplicationNo
					,@CurrProcessActionId	=	@V_WKF_TASK_ACTION
					,@UserId				=	@V_USER_PK
					,@Comments				=	@V_WKF_COMMENTS
					,@DivisionGroup			=	@DivisionGroup	
					,@ErrorNumber			=	@ErrorNumber	OUTPUT
					,@ErrorMessage			=	@ErrorMessage	OUTPUT


			IF (@ErrorNumber < 0)
			BEGIN
				ROLLBACK TRANSACTION
				EXEC [TransactionErrorLogs] @UserId = @V_USER_PK,@ProcessId = @V_WKF_PROCESS,@ProcessActionId=@V_WKF_TASK_ACTION,@ParamJson = @Json,@ErrorNumber = @ErrorNumber
				RETURN
			END
			
			SELECT	 @ErrorNumber	=	@V_WKF_APPLICATION
					,@ErrorMessage	=	ISNULL(@ErrorMessage,'')
		END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 
		BEGIN
			ROLLBACK TRANSACTION
		END
		EXEC [TransactionErrorLogs] @UserId = @V_USER_PK,@ProcessId = @V_WKF_PROCESS,@ProcessActionId=@V_WKF_TASK_ACTION,@ParamJson = @Json,@ErrorNumber = @ErrorNumber
		SET @ErrorNumber	= -1
		SELECT	ERROR_LINE()	    ErrorLine,	ERROR_MESSAGE()		ErrorMessage,
				ERROR_NUMBER()		ErrorNo,	ERROR_P
ROCEDURE()	ErrorProcedure		
	END CATCH
END
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
