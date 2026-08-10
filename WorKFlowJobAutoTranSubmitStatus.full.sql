/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	Author	:	
	Date	:	
	Purpose :	Job after Stock Admission submission
	Execute :	BEGIN TRANSACTION
					DECLARE @P_RET_VAL INT
					EXEC [WorKFlowJobAutoTranSubmitStatus] 10,NULL,@P_RET_VAL OUTPUT
					SELECT @P_RET_VAL [RETURN_VALUE]

				ROLLBACK TRANSACTION

	SPWKF_JOB_AUTO_TRAN_SUB_STATUS >>>  WorKFlowJobAutoTranSubmitStatus
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	Modified By				On				Remarks
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	<Next Entry>
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
CREATE PROCEDURE [dbo].[WorKFlowJobAutoTranSubmitStatus]
(	
	 @pAppID	INT
	,@pRefID	INT
	,@pRetVal	INT OUTPUT
)
AS 
BEGIN
	DECLARE	@V_GIH_DEPT		INT,
			@V_GIH_PK		INT,
			@V_ROW_NO		INT,
			@V_ROW_NO_MAX	INT
	
	DECLARE	@T_INV_GIN_HDR	TABLE
		(
			[ROW_NO]	INT IDENTITY(1,1)
			,[GIH_PK]	INT
			,[GIH_DEPT]	INT
		)
	BEGIN TRY
		---------------------------------------------
		--Completing Workflow trx of previous process
		---------------------------------------------
		--IF EXISTS (	SELECT	[Id]
		--			FROM	[AppConfigurationMaster]
		--			WHERE	[Setting]	= 'LINKED SMS PROCESS'
		--				AND	[Value]		= 1)
		--BEGIN
		--	INSERT  INTO @T_INV_GIN_HDR
		--		(	
		--			[GIH_PK]
		--			,[GIH_DEPT]
		--		)
		--	SELECT	DISTINCT
		--			[GID_GI]
		--			,[GID_DEPT]
		--	FROM	[INV_GIN_DTL]
		--			INNER JOIN [INV_STK_TRAN_GIN_MAP] ON [ISG_GIN_DTL] = [GID_PK]
		--	WHERE	[ISG_ST] = @pAppID

		--	SET	@V_ROW_NO = 1
		--	SELECT	@V_ROW_NO_MAX = MAX(ROW_NO) FROM	@T_INV_GIN_HDR
		--	WHILE(@V_ROW_NO <= @V_ROW_NO_MAX)
		--	BEGIN
		--		SELECT	@V_GIH_PK		= [GIH_PK]
		--				,@V_GIH_DEPT	= [GIH_DEPT]
		--		FROM	@T_INV_GIN_HDR
		--		WHERE	[ROW_NO]		= @V_ROW_NO

		--		--IF @V_GIH_PK IS NOT NULL
		--		--BEGIN
		--		--	EXEC	[SpWkfTransactionClose]
		--		--			@pPageUrl	= '/StoreManagement/GINCreate.aspx?TYPE=1'
		--		--			,@pAppPK	= @V_GIH_PK
		--		--			,@pDept		= @V_GIH_DEPT
		--		--			,@pRetVal	= @PRetVal	OUTPUT
		--		--	IF	@PRetVal < 0
		--		--	BEGIN
		--		--		RETURN
		--		--	END

		--		--	EXEC	[SpWkfTransactionClose]
		--		--			@pPageUrl	= '/StoreManagement/GINCreate.aspx?TYPE=2'
		--		--			,@pAppPK	= @V_GIH_PK
		--		--			,@pDept		= @V_GIH_DEPT
		--		--			,@pRetVal	= @PRetVal	OUTPUT
		--		--	IF	@PRetVal < 0
		--		--	BEGIN
		--		--		RETURN
		--		--	END
					
		--		--END
		--		SET @V_ROW_NO = @V_ROW_NO + 1
		--	END	--End of Loop
		--END	--end of config checking

		-------------------
		--Update GIN Status
		-------------------
		
		UPDATE	[StockTranHeader]
		SET		 [Status]			= 1 --Submitted
				,[SubmittedBy]		= [ModifiedBy] 
				,[SubmittedDate]	= GETDATE()
		WHERE	[Id]  = @pAppID;
		
		SET @PRetVal = 1;
	END TRY
	BEGIN CATCH
		SET	@pRetVal = -1
	END CATCH
END
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
