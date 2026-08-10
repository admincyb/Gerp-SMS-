/***************************************************************************************
	Author	:	
	Date	:	
	Purpose :	Job after stock transfer approval
***************************************************************************************
	Execute :	BEGIN TRANSACTION
					DECLARE @P_RET_VAL INT
					EXEC [WorkFlowJobAutoTranApproveStatus] 10,NULL,@P_RET_VAL OUTPUT
				ROLLBACK TRANSACTION

	SPWKF_JOB_AUTO_TRAN_APR_STATUS >>> [WorkFlowJobAutoTranApproveStatus]
***************************************************************************************
	Modified By		Modified On		Remarks
***************************************************************************************
	GTI-075							(i) Submit job execution added.
	GTI-226			11-08-2021		Currency conversion changed according to GRN_date
	GTI-229			27-12-2022		Add special condition to add vencode as sufix for batch no (for mmt and only for compound and latex)
	A0031			01/04/2026		Transaction No added to insert in batch transaction detail
	A0031			24/07/2026		Copy GRN manufacture and expiry dates to the posted stock batch
	A0031			24/07/2026		Populate GRN actual batch and lot numbers from StockTranDetail.GRNBatchNo
	A0031			28/07/2026		Validate source stock against pending Company Return reservations
	A0031			28/07/2026		Block source posting when Stock UOM conversion is not configured
***************************************************************************************/
CREATE PROCEDURE [dbo].[WorkFlowJobAutoTranApproveStatus]
(	
	 @pAppID	INT
	,@pRefID	INT
	,@pRetVal	INT		OUTPUT
)
AS 
BEGIN 
	DECLARE @V_TRANSACTION_STARTED BIT = 0;

	BEGIN TRY
	IF @@TRANCOUNT = 0
	BEGIN
		BEGIN TRANSACTION;
		SET @V_TRANSACTION_STARTED = 1;
	END
	ELSE
	BEGIN
		SAVE TRANSACTION StockTransferApprove;
	END
	
	--------------------
	--Execute Submit Job
	---------------------
	
	EXEC	[WorKFlowJobAutoTranSubmitStatus]
		 @pAppID	=	@pAppID
		,@pRefID	=	@pRefID
		,@pRetVal	=	@pRetVal OUTPUT
	
	-----------------
	--Stock Updation
	-----------------
	DECLARE	@V_QTY_APPROVED		FLOAT,
			@V_ITEM				INT,
			@V_DAMGE_STORE		INT,
			@V_ITD_PK			INT,
			@V_ITM_UOM			INT,
			@V_SFD_UOM			INT,

			@V_STD_PK			INT,
			@V_ROW_NO			INT,
			@V_MAX_ROW_NO		INT,
			@V_SFH_PK			INT,	--APP ID
			@V_SFH_BIZUNIT		INT,

			@V_SFH_NO			NVARCHAR(100),
			@V_SFD_BATCH_NO		NVARCHAR(100),
			@V_SFD_GRN_BATCH_NO	NVARCHAR(200),
			@V_TXN_NO			NVARCHAR(100),
			@V_SFD_MANUFACTURE_DATE DATE,
			@V_SFD_EXPIRY_DATE	DATE,
			@V_SFH_DATE			DATE,
			@V_USER_PK		INT,
			@V_SFD_DEPT_STORE	INT,
			@V_SFD_PO_RATE		FLOAT,
			
			@V_SBD_PK			INT,
			
			@V_Module				INT,	----@V_GIH_DEPT			INT,
			@V_IVH_PK			INT,
			@V_RATE_ROUND		TINYINT,
			@V_BTD_REF_DESC1	NVARCHAR(1000),
			@V_BTD_REF_DESC2	NVARCHAR(1000),
			@V_BTD_REF_DESC3	NVARCHAR(1000),
			@V_BTD_REF_DESC5	NVARCHAR(1000),
			@V_BTD_REF_DATE1	DATE,
			@V_SOURCE_STOCK_COUNT INT,
			@V_SOURCE_STOCK_UOM INT,
			@V_SOURCE_STOCK_QTY FLOAT,
			@V_SOURCE_PHYSICAL_QTY FLOAT,
			@V_SOURCE_AVAILABLE_QTY FLOAT,
			@V_TRANSACTION_TYPE INT,
			--@V_VEN_CODE			NVARCHAR(200),
			@V_CLIENT_CODE      NVARCHAR(100),
			@GDRId				INT
	
	DECLARE @TEMP_PROD_CAT TABLE 
	(
		ITC_PK INT
	)
		
	SET		@V_SFH_PK 	= @pAppID

	--Get GIN hdr values
		SELECT	 @V_SFH_BIZUNIT	=	SD.[Bizunit]
				,@V_SFH_NO		=	SD.[No] + ' - ' + 	GDR.[BatchNo]
				,@V_SFH_DATE	=	SD.[Date]
				,@V_USER_PK		=	SD.[ModifiedBy]
				,@V_Module		=	SD.[Module]
				,@V_TRANSACTION_TYPE = SD.[TransactionType]
				,@GDRId			=	Map.GoodsReceiptHeaderId
		FROM	[StockTranHeader] SD
		INNER JOIN [STOCKTRANGRNHeaderMap]	Map	ON	MAp.[StockHeaderId]	=	SD.[Id]
		INNER JOIN [GoodsReceiptNoteDetail] GDR ON	GDR.[GoodsReceiptHeaderId] = Map.[GoodsReceiptHeaderId]
		WHERE	SD.Id	=	@V_SFH_PK

	--SELECT	@V_GIH_DEPT		= [GID_DEPT]
	--FROM	[INV_STK_TRAN_GIN_MAP]
	--INNER JOIN [INV_GIN_DTL] ON [GID_PK] = [ISG_GIN_DTL]
	--WHERE	[ISG_ST]		= @V_SFH_PK

	--SELECT	@V_VEN_CODE = [VEN_CODE]
	--FROM	[INV_STK_TRAN_GIN_MAP]
	--		INNER JOIN [INV_GIN_DTL]    ON [GID_PK] = [ISG_GIN_DTL]
	--		INNER JOIN [INV_GRN_HDR]    ON [GRH_PK] = [GID_GRN] 
	--		INNER JOIN [PUR_VENDOR_MST] ON [VEN_PK] = [GRH_VENDOR]
	--WHERE	[ISG_ST]		= @V_SFH_PK

	SELECT	@V_CLIENT_CODE = [Data]
	FROM	[AppConfigurationMaster] 
	WHERE	[Setting]	= 'CLIENT CODE'

	/*To get production category (compounding)*/
	--------------------------------------------
	;WITH RMCTE
	AS
	(
				SELECT	 [Id]
						,[Parent] 
				FROM	[ItemCategory]
				WHERE	( [Id] IN (SELECT ID FROM dbo.FNARRAYTAB((SELECT [Data] FROM [AppConfigurationMaster] WHERE  [Setting]='PRD CATEGORY CHEMICAL')))
					OR	  [Id] IN (SELECT ID FROM dbo.FNARRAYTAB((SELECT [Data] FROM [AppConfigurationMaster] WHERE  [Setting]='PRD CATEGORY LATEX'))) )
					
				UNION ALL  
				
				SELECT	 C.[Id]
						,C.[Parent]
	
				FROM	[ItemCategory] C  
				INNER JOIN RMCTE ON RMCTE.[Id] = C.[Parent] 
	)
	INSERT INTO @TEMP_PROD_CAT ( ITC_PK )
	SELECT DISTINCT  [Id]
	FROM RMCTE


		
	--get GIN dtl values
	DECLARE @T_INV_STK_TRAN_DTL	TABLE	
		(	
			[ROW_NO]			INT IDENTITY(1,1),
			[SFD_PK]			INT ,
			[SFD_ITEM]			INT ,
			[SFD_ITEM_SL_NO]	SMALLINT ,
			[SFD_QTY_APPROVED]	FLOAT ,
			[SFD_UOM]			INT,
			[SFD_PO_DTL]		INT,
			[SFD_PR_DTL]		INT,
			[SFD_DEPT_STORE]	INT,
			[SFD_PO_RATE]		FLOAT,
			[GRNBatchNo]		NVARCHAR(500),
			[GRD_PK]			INT,
			[CONV_FACT]			INT
		)
	DECLARE @T_PO_DTL TABLE
		(
			[PO_DTL_PK]				INT,
			[PO_ITEM]				INT,
			[PO_QTY_ALLOCATED]		FLOAT,
			[PO_ADDL_QTY_ALLOCATED]	FLOAT
		)
	
	DECLARE @T_INVOICE_PO_DTL TABLE
		(
			[ROW_NO]	INT IDENTITY(1,1),
			[IVH_PK]	INT
		)

	--Get PO-ST map dtls
	INSERT INTO @T_PO_DTL
		(
			[PO_DTL_PK]
			,[PO_ITEM]
			,[PO_QTY_ALLOCATED]
			,[PO_ADDL_QTY_ALLOCATED]
		)
	SELECT	 [PurchaseOrderDetailId]				
			,ISNULL((	SELECT	[Item]	
						FROM	[PurchaseOrderDetail]
						WHERE	[Id] = [PurchaseOrderDetailId]
					),0)			
			,[QuantityPurchaseOrderAllocated]		
			,[QuantityAdditionalAllocated]	
	FROM	[StockTransactionPurchaseOrderMap]
	WHERE	[StockTransactionHeaderId] = @V_SFH_PK

	--set Approved status
	UPDATE	[StockTranHeader] 
	SET		 [Status]			= 2
			,[ApprovedBy]		= dbo.[FnWkfRefUserGet] (@pRefID) 
			,[ApprovedDate]	= GETDATE() 
	WHERE	[Id] = @V_SFH_PK	

	--Set Approved Qty
	UPDATE	[StockTranDetail]  
	SET		[QuantityApproved]	= ISNULL([QuantityAllocated],0) 
	WHERE	[StockTransactionHeaderId]  = @V_SFH_PK
	
	SELECT	@V_RATE_ROUND = [Value]
	FROM	[AppConfigurationMaster]
	WHERE	[Setting]	=	'CURRENCY SETTINGS' 
		AND [Data]		=	'RateDecimalDigitP2P'
		

	INSERT INTO @T_INV_STK_TRAN_DTL
		(
			[SFD_PK]
			,[SFD_ITEM]
			,[SFD_ITEM_SL_NO]
			,[SFD_QTY_APPROVED]
			,[SFD_UOM]
			,[SFD_PO_DTL]
			,[SFD_PR_DTL]
			,[SFD_DEPT_STORE]
			,[SFD_PO_RATE]
			,[GRNBatchNo]
			,[GRD_PK]
		)
	SELECT	 [Id] 
			,[Item]
			,[SlNo]						AS	[SFD_ITEM_SL_NO]
			,[QuantityApproved]
			,[Uom]
			,[PurchaseOrderDetailId]
			,[PurchaseRequestDetailId]
			,[ModuleStore]
			,(	SELECT	CASE	WHEN	D.[Rate]	<>	0
								THEN	ROUND((D.[AmountValue]/ NULLIF((D.[QuantityRequested] * ISNULL(CF.ConvFact,1)),0)) * ISNULL(NULLIF(H.[ExchangeRate],0),(SELECT dbo.[FNADM_CURRENCY_CONV_FACT_GET](H.[Currency],H.[CurrencyBC],OD.[Date]))),@V_RATE_ROUND)
								ELSE	0
						END
						--ROUND(([POD_AMT_VALUE]/[POD_QTY_REQUESTED]) * [POH_EXCHG_RATE],@V_RATE_ROUND)	--tax and other charges included
						--Modified On 2016.01.28 as per request from EKK.
				FROM	[PurchaseOrderDetail]	D
				INNER JOIN [PurchaseOrderHeader]H	ON	H.[Id]	=	D.[Po]
				WHERE	D.[Id] = (	
										CASE	WHEN	OD.[PurchaseOrderDetailId] IS NOT NULL

												THEN	OD.[PurchaseOrderDetailId]
														--Additional Quantity
												ELSE	CASE WHEN EXISTS
														(	SELECT	TOP 1  
																	ID.[PurchaseOrderDetailId]
															FROM	[StockT
