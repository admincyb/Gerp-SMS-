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
ranDetail] ID
															WHERE	ID.[StockTransactionHeaderId]	=	OD.[StockTransactionHeaderId]
																AND	ID.[Item]						=	OD.[Item]
																AND	ID.[QuantityType]				=	1 -- Normal qty
														)
														THEN 
														-- Additional qty in POD detail
														(	SELECT	TOP 1  
																	ID.[PurchaseOrderDetailId]
															FROM	[StockTranDetail] ID
															WHERE	ID.[StockTransactionHeaderId]	=	OD.[StockTransactionHeaderId]
																AND	ID.[Item]						=	OD.[Item]
																AND	ID.[QuantityType]				=	1 -- Normal qty
															
														)
														ELSE
														-- Additional row item qty in POD detail
															(	SELECT	TOP 1  
																		ID.[PurchaseOrderDetailId]
																FROM	[StockTransactionPurchaseOrderMap]	ID
																INNER JOIN [PurchaseOrderDetail]			P	ON P.[Id] = ID.[PurchaseOrderDetailId]
																WHERE	ID.[StockTransactionHeaderId]	=	OD.[StockTransactionHeaderId]
																	AND	P.[Item]						=	OD.[Item]
															)
														END
										END
										)
									)
			,ISNULL(OD.[GRNBatchNo],'')
			,[GoodsReceiptNoteDetailId]
			
	FROM	[StockTranDetail] OD
	OUTER APPLY
	(
		SELECT TOP 1 ISNULL(ICM.[ConvFact],1) AS ConvFact
		FROM [GoodsReceiptNoteDetail] GRD
		LEFT JOIN [ItemUomConversionMap] ICM
			ON ICM.[PurchaseItemId] = GRD.[Item]
		   AND ICM.[Active] = 1
		WHERE GRD.[Id] = OD.[GoodsReceiptNoteDetailId]
	) CF
	WHERE	[StockTransactionHeaderId] = @V_SFH_PK
	--PO Details Quantity allocation & Addl. Quantity allocation Updation.			
	UPDATE	T
	SET		[QuantityAllocated]			= ISNULL(T.[QuantityAllocated],0)			+ ISNULL([PO_QTY_ALLOCATED],0),
			[QuantityAdditionalAllocated]	= ISNULL(T.[QuantityAdditionalAllocated],0)	+ ISNULL([PO_ADDL_QTY_ALLOCATED],0)
	FROM	[PurchaseOrderDetail]	T 
	INNER JOIN @T_PO_DTL			D ON	[PO_DTL_PK]	= T.[Id] AND [PO_ITEM] = T.[Item]

	--PO Details Quantity allocation & Addl. Quantity allocation Updation.			
	UPDATE	T
	SET		T.[QuantityAllocated]			= ISNULL(T.[QuantityAllocated],0)	+ ISNULL(S.[QuantityApproved],0)
	FROM	[PurchaseOrderRequestMap]	T 
	INNER JOIN [StockTranDetail]		S	ON	(		T.[PurchaseOrderDetailId]	=	S.[PurchaseOrderDetailId] 
													AND T.[Item]					=	S.[Item] 
													AND T.[PurchaseRequestDetailId] =	S.[PurchaseRequestDetailId] 
													AND S.[StockTransactionHeaderId]=	@V_SFH_PK
												)

	SET		@V_ROW_NO	= 1;
			
	SELECT @V_MAX_ROW_NO = MAX([ROW_NO]) FROM @T_INV_STK_TRAN_DTL
	
	WHILE (@V_ROW_NO <= @V_MAX_ROW_NO) 
	BEGIN
	
		SELECT	@V_QTY_APPROVED		= [SFD_QTY_APPROVED]  
				,@V_ITEM			= [SFD_ITEM]
				,@V_SFD_UOM			= [SFD_UOM]
				,@V_SFD_DEPT_STORE	= [SFD_DEPT_STORE]
				,@V_SFD_PO_RATE		= ISNULL([SFD_PO_RATE],0)
				,@V_SFD_BATCH_NO	= ISNULL(SD.[No] + ' - ' + 	ISNULL([GRNBatchNo],''),ISNULL(@V_SFH_NO,'')) --+ '-' + CAST(SFD_ITEM_SL_NO AS NVARCHAR)
				,@V_SFD_GRN_BATCH_NO = NULLIF(LTRIM(RTRIM(S.[GRNBatchNo])), N'')
				,@GDRId				= [GRD_PK]
				,@V_TXN_NO			= [GoodsReceiptNo]
				,@V_SFD_MANUFACTURE_DATE = G.[DateOfManufacture]
				,@V_SFD_EXPIRY_DATE	= G.[DateOfExpiry]
		FROM	@T_INV_STK_TRAN_DTL S
		LEFT JOIN GoodsReceiptNoteDetail G	ON	G.[Id]					=	S.[GRD_PK]
		INNER JOIN [StockTranHeader] SD ON SD.No = G.GoodsReceiptNo
									--	AND SlNo = ROW_NO
		WHERE	[ROW_NO]			= @V_ROW_NO;
		
		IF EXISTS(	SELECT [Id]
					FROM ItemMaster				C
					INNER JOIN @TEMP_PROD_CAT	I	ON	I.[ITC_PK] = C.[Category] 
					WHERE [Id] = @V_ITEM
				)
			  AND @V_CLIENT_CODE = 'MMT'
		BEGIN
			SET @V_SFD_BATCH_NO = @V_SFD_BATCH_NO --+'-'+ @V_VEN_CODE
		END
		
		
		-------------------------------
		--Fields for Stock reports
		-------------------------------
		SELECT	@V_BTD_REF_DESC1	=	( ISNULL((	STUFF((
													SELECT	DISTINCT ', '+ P.[No]
													FROM	[StockTranDetail]			S
													INNER JOIN [PurchaseOrderDetail]	P	ON	P.[Id]	=	S.[Pur
chaseOrderDetailId]
													WHERE	S.[Item]					=	@V_ITEM 
														AND S.[StockTransactionHeaderId]=	@V_SFH_PK
													FOR XML PATH('')
											),1,2,'')
										),''))
									+
								( ISNULL('(' +(	STUFF((
													SELECT	DISTINCT ', '+ I.[No] + '- ' + REPLACE(CONVERT(VARCHAR(11), I.[Date], 106), ' ', '-')
													FROM	[StockTranDetail]			S
													INNER JOIN [GoodsReceiptNoteDetail]	G						ON	G.[PurchaseOrderDetailId]	=	S.[PurchaseOrderDetailId]
													INNER JOIN [GoodsReceiptNoteHeader]	N						ON	N.[Id]						=	G.[GoodsReceiptHeaderId]		
													INNER JOIN [FinanceInvoiceVendorGoodsReceiptNoteDetail]	D	ON	D.[GoodsReceiptNoteDetailId]=	N.[Id] 
																												AND D.[QuantityInvoiced]		>	0
													INNER JOIN [FinanceInvoiceVendorDetail]				I		ON	I.[Id]						=	D.[InvoiceDetail]
													INNER JOIN [FinanceInvoiceVendorHeader]				F		ON	F.[Id]						=	I.[InvoiceVendorHeaderId] 
																												AND F.[DeletedStatus]			=	0
													WHERE	S.[Item]					=	@V_ITEM 
														AND S.[StockTransactionHeaderId]=	@V_SFH_PK
													FOR XML PATH('')
												)+ ')',1,2,'')		
												),''))
				,@V_BTD_REF_DESC2	=	( ISNULL((
													SELECT	TOP 1 ISNULL(F.[VendorInvoiceNo] ,N.[VendorRefernceNo])
													FROM	[StockTranDetail]			S
													INNER JOIN [GoodsReceiptNoteDetail]	G						ON	G.[PurchaseOrderDetailId]	=	S.[PurchaseOrderDetailId]
													INNER JOIN [GoodsReceiptNoteHeader]	N						ON	N.[Id]						=	G.[GoodsReceiptHeaderId]		
													LEFT JOIN [FinanceInvoiceVendorGoodsReceiptNoteDetail]	D	ON	D.[GoodsReceiptNoteDetailId]=	N.[Id] 																												
													LEFT JOIN [FinanceInvoiceVendorDetail]				I		ON	I.[Id]						=	D.[InvoiceDetail]
																												AND D.[QuantityInvoiced]		>	0
													LEFT JOIN [FinanceInvoiceVendorHeader]				F		ON	F.[Id]						=	I.[InvoiceVendorHeaderId] 
																												AND F.[DeletedStatus]			=	0
													WHERE	S.[Item]					=	@V_ITEM 
														AND S.[StockTransactionHeaderId]=	@V_SFH_PK
										),''))
				,@V_BTD_REF_DESC3	=	( ISNULL((
											SELECT	TOP 1 CAST(V.[Id]	AS NVARCHAR(100))
											FROM	[StockTranDetail]			S
											INNER JOIN [GoodsReceiptNoteDetail]	G						ON	G.[PurchaseOrderDetailId]	=	S.[PurchaseOrderDetailId]
											INNER JOIN [GoodsReceiptNoteHeader]	N						ON	N.[Id]						=	G.[GoodsReceiptHeaderId]
											INNER JOIN [PurchaseVendorMaster]	V						ON	N.[Vendor]					=	V.[Id] 
											LEFT JOIN [FinanceInvoiceVendorGoodsReceiptNoteDetail]	D	ON	D.[GoodsReceiptNoteDetailId]=	N.[Id] 																												
											LEFT JOIN [FinanceInvoiceVendorDetail]				I		ON	I.[Id]						=	D.[InvoiceDetail]
																										AND D.[QuantityInvoiced]		>	0
											LEFT JOIN [FinanceInvoiceVendorHeader]				F		ON	F.[Id]						=	I.[InvoiceVendorHeaderId] 
																												AND F.[DeletedStatus]			=	0
											WHERE	S.[Item]					=	@V_ITEM 
												AND S.[StockTransactionHeaderId]=	@V_SFH_PK
										),''))
				,@V_BTD_REF_DESC5	=	( ISNULL((
											SELECT	TOP 1 G.[VendorReferenceNo]
											FROM	[StockTranDetail]			S
											INNER JOIN [GoodsReceiptNoteDetail]	G	ON	G.[PurchaseOrderDetailId]	=	S.[PurchaseOrderDetailId]
											WHERE	S.[Item]					=	@V_ITEM 
												AND S.[StockTransactionHeaderId]=	@V_SFH_PK
										),''))
				,@V_BTD_REF_DATE1	=	(	ISNULL((		
											SELECT	TOP 1 ISNULL(F.[DateReceived] ,N.[VendorRefernceDate])	--NULLIF(ISNULL([IVH_DATE_RECEIVED] ,[GRH_VND_REF_DATE]),'1900-01-01 00:00:00.000')
											FROM	[StockTranDetail]			S
											INNER JOIN [GoodsReceiptNoteDetail]	G						ON	G.[PurchaseOrderDetailId]	=	S.[PurchaseOrderDetailId]
											INNER JOIN [GoodsReceiptNoteHeader]	N						ON	N.[Id]						=	G.[GoodsReceiptHeaderId]		
											LEFT JOIN [FinanceInvoiceVendorGoodsReceiptNoteDetail]	D	
ON	D.[GoodsReceiptNoteDetailId]=	N.[Id] 																												
											LEFT JOIN [FinanceInvoiceVendorDetail]				I		ON	I.[Id]						=	D.[InvoiceDetail]
																										AND D.[QuantityInvoiced]		>	0
											LEFT JOIN [FinanceInvoiceVendorHeader]				F		ON	F.[Id]						=	I.[InvoiceVendorHeaderId] 
																												AND F.[DeletedStatus]			=	0
											WHERE	S.[Item]					=	@V_ITEM 
												AND S.[StockTransactionHeaderId]=	@V_SFH_PK
										),NULL))

		--Item UOM
		SELECT @V_ITM_UOM = [Uom] 
		FROM [ItemMaster] 
		WHERE [Id] = @V_ITEM

		IF ISNULL(@V_TRANSACTION_TYPE, 0) <> 1
		BEGIN
		SET @V_SOURCE_STOCK_COUNT = 0;
				SET @V_SOURCE_STOCK_UOM = NULL;
				SET @V_SOURCE_PHYSICAL_QTY = NULL;
				SET @V_SOURCE_AVAILABLE_QTY = NULL;
		
				SELECT	 @V_SOURCE_STOCK_COUNT = COUNT(*)
						,@V_SOURCE_STOCK_UOM = MAX(SD.[Uom])
						,@V_SOURCE_PHYSICAL_QTY =
							SUM(ISNULL(SD.[QuantityInStock],0) - ISNULL(SD.[QuantityPnTransit],0))
						,@V_SOURCE_AVAILABLE_QTY =
							SUM(ISNULL(SD.[QuantityInStock],0)
								- ISNULL(SD.[QuantityPnTransit],0)
								- ISNULL(SD.[QuantityReserved],0))
				FROM [dbo].[StockDetail] SD WITH (UPDLOCK, HOLDLOCK)
				WHERE SD.[Item] = @V_ITEM
				  AND SD.[Module] = @V_Module
				  AND SD.[Bizunit] = @V_SFH_BIZUNIT;
		
				IF @V_SOURCE_STOCK_COUNT <> 1
				BEGIN
					SET @pRetVal = -797;	--Stock Transfer Source Store Stock Missing or Duplicated
					GOTO StockTransferApproveFailure;
				END
		
				SET @V_SOURCE_STOCK_QTY = @V_QTY_APPROVED
					* dbo.[FNINV_ITEM_UOM_CONV_FACTOR](@V_ITEM,@V_SFD_UOM,@V_SOURCE_STOCK_UOM);
		
				IF @V_SOURCE_STOCK_QTY IS NULL OR @V_SOURCE_STOCK_QTY <= 0
				BEGIN
					SET @pRetVal = -797;	--Stock Transfer Source Store UOM Conversion Missing
					GOTO StockTransferApproveFailure;
				END
		
				IF @V_SOURCE_STOCK_QTY
						> @V_SOURCE_AVAILABLE_QTY + 0.000001
				BEGIN
					IF @V_SOURCE_STOCK_QTY
							<= @V_SOURCE_PHYSICAL_QTY + 0.000001
					   AND EXISTS
					   (
							SELECT 1
							FROM [dbo].[ItemIssueDetail] CRD
							INNER JOIN [dbo].[ItemIssueHeader] CRH ON CRH.[Id] = CRD.[IssueId]
							WHERE CRD.[Item] = @V_ITEM
							  AND CRH.[Department] = @V_Module
							  AND CRH.[Bizunit] = @V_SFH_BIZUNIT
							  AND CRH.[Status] IN (0,1)
							  AND ISNULL(CRH.[DeletedStatus],0) = 0
							  AND CRH.[TransactionType] = 15
							  AND CRD.[ReservedQuantity] > 0
					   )
					BEGIN
						SET @pRetVal = -794;	--Insufficient stock. Quantity is reserved against pending company return.
					END
					ELSE
					BEGIN
						SET @pRetVal = -796;	--Insufficient Stock Transfer Source Store Stock
					END
					GOTO StockTransferApproveFailure;
				END
		
				
		END

		-- Update Qty to Header
		UPDATE 	[StockHeader]
		SET		 [QuantityInStock]	=	[QuantityInStock] + @V_QTY_APPROVED  * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])--REJECTED QTY is not updated to Hdr stk
				,[QuantityPnIssue]	=	[QuantityPnIssue] - @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])	
				,[Value]			=	CASE	WHEN [Rate] = 0
												THEN @V_SFD_PO_RATE * (	[QuantityInStock] + @V_QTY_APPROVED  * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom]))
												ELSE
													 [Value] + ( @V_SFD_PO_RATE * @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])) --total value
										END
				,[Rate]				=	CASE	WHEN [Rate] = 0
												THEN @V_SFD_PO_RATE
												ELSE
													 ISNULL(( [Value] + ( @V_SFD_PO_RATE * @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom]))) --total value
													 /
													 CASE	WHEN	([QuantityInStock] + @V_QTY_APPROVED  * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])) = 0
															THEN	NULL
															ELSE	([QuantityInStock] + @V_QTY_APPROVED  * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])) --total stock
													 END,0)
										END
		WHERE	[Item]				=	@V_ITEM


		--Update From Store (GIN store)
		--Update Qty to dtl
		IF ISNULL(@V_TRANSACTION_TYPE, 0) <> 1
		BEGIN
			UPDATE 	[StockDetail]
					SET		 [QuantityInStock]	=	ISNULL([QuantityInStock],0) - @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])
							,[QuantityPnIssue]	=	ISNULL([QuantityPnIssue],0) - @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])
							,[ModifiedBy]		=	@V_USER_PK
							,[ModifiedDate]		=	GETDATE()
					WHERE	[Item]		=	@V_ITEM
						AND	[Module]	=	@V_Module
						AND [Bizunit]	=	@V_SFH_BIZUNIT;
		END
		--Item-Dept Mapping
		IF NOT EXISTS ( SELECT	[Item]  FROM [ItemModuleMap] WHERE [Item]	= @V_ITEM  AND 	[Module]		= @V_Module)
		INSERT	INTO [ItemModuleMap]
			(	
				 [Item]
				,[Module]
				,[SbuId]
				,[ModifiedBy]
				,[ModifiedDate]
			)
		VALUES
			( 
				@V_ITEM
				,@V_Module 
				,@V_SFH_BIZUNIT
				,@V_USER_PK
				,GETDATE()
			);
			
		--------------------------
		--Update Store-wise Stock
		--------------------------
		IF EXISTS ( SELECT	[Id]
					FROM	[StockDetail]
					WHERE	[Item]	= @V_ITEM  
						AND [Module]= @V_SFD_DEPT_STORE)
		BEGIN
			--Update Qty to dtl
			UPDATE 	[StockDetail]
			SET	 [QuantityInStock]	=	[QuantityInStock] + @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])
				,[Value]			=	CASE	WHEN	[Rate] = 0
												THEN	@V_SFD_PO_RATE * (	[QuantityInStock] + @V_QTY_APPROVED  * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom]))
												ELSE	[Value] + ( @V_SFD_PO_RATE * @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])) --total value
										END
				,[Rate]				=	CASE	WHEN	[Rate] = 0
												THEN	@V_SFD_PO_RATE
												ELSE	ISNULL(( [Value] + ( @V_SFD_PO_RATE * @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom]))) --total value
																 /
																 CASE	WHEN	([QuantityInStock] + @V_QTY_APPROVED  * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])) = 0
																		THEN	NULL
																		ELSE	([QuantityInStock] + @V_QTY_APPROVED  * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])) --total stock
																END,0)
										END
				,[ModifiedBy]		=	@V_USER_PK
				,[ModifiedDate]		=	GETDATE()
			WHERE	[Item]	= @V_ITEM
				AND	[Module]= @V_SFD_DEPT_STORE
		END
		ELSE
		BEGIN
			INSERT INTO	 [StockDetail]
				(	
					 [Item]
					,[QuantityInStock]
					,[QuantityPnOrder]
					,[QuantityPnReceipt]
					,[QuantityPnInspection]
					,[QuantityPnIssue]
					,[QuantityReserved]
					,[QuantityDamaged]
					,[Uom]
					,[Module]
					,[Bizunit]
					,[ModifiedBy]
					,[ModifiedDate]
				)
				
			VALUES		
				(	
					@V_ITEM
					,@V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,@V_ITM_UOM)
					,0
					,0
					,0
					,0
					,0
					,0
					,@V_ITM_UOM
					,@V_SFD_DEPT_STORE
					,@V_SFH_BIZUNIT
					,@V_USER_PK
					,GETDATE()
				)
		END
		
		--------------------------------------------
		--Update Store-wise Stock transaction (ISSD)
		--------------------------------------------
		--SELECT  @V_ITD_PK = ISNULL(MAX([Id]),0)+1 FROM [StockTransactionsDetail];
		--INSERT INTO [StockTransactionsDetail]
		--	(	
		--		 [Id] 
		--		,[Module]
		--		,[TransactionMode]
		--		,[Transaction]
		--		,[TransactionNo]
		--		,[TransactionDate]
		--		,[SlNo]
		--		,[Item]
		--		,[QuantityReceipt]
		--		,[UomReceipt]
		--		,[QuantityIssue]
		--		,[UomIssue]
		--		,[QuantityStockReceipt]
		--		,[QuantityStockIssue]
		--		,[UomStock]
		--		,[ModuleId]
		--		,[Bizunit]
		--	)
		--VALUES
		--	(
		--		@V_ITD_PK
		--		,2	
		--		,12 --Stk transfer
		--		,@V_SFH_PK
		--		,@V_SFH_NO
		--		,@V_SFH_DATE
		--		,(	SELECT	ISNULL(MAX([SlNo]),0)+1 
		--			FROM	[StockTransactionsDetail] 
		--			WHERE	[TransactionDate]	=	@V_SFH_DATE
		--				AND [Item]				=	@V_ITEM 
		--				AND [ModuleId]			=	@V_Module)
		--		,@V_ITEM
		--		,0
		--		,NULL
		--		,@V_QTY_APPROVED

		--		,@V_SFD_UOM
		--		,0
		--		,@V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,@V_ITM_UOM)
		--		,@V_ITM_UOM
		--		,@V_Module
		--		,@V_SFH_BIZUNIT
		--	)
			
		----------------------------------------------
		--Update Store-wise Stock transaction (RCVD)
		----------------------------------------------
		SELECT  @V_ITD_PK = ISNULL(MAX([Id]),0)+1 FROM [StockTransactionsDetail];
		INSERT INTO [StockTransactionsDetail]
			(	
				 [Id] 
				,[Module]
				,[TransactionMode]
				,[Transaction]
				,[TransactionNo]
				,[TransactionDate]
				,[SlNo]
				,[Item]
				,[QuantityReceipt]
				,[UomReceipt]
				,[QuantityIssue]
				,[UomIssue]
				,[QuantityStockReceipt]
				,[QuantityStockIssue]
				,[UomStock]
				,[ModuleId]
				,[Bizunit]
				,[Rate]
				,[RateDetail]
			)
		VALUES
			(	
				@V_ITD_PK
				,2	
				,12 --Stk transfer
				,@V_SFH_PK
				,@V_SFD_BATCH_NO  
				,@V_SFH_DATE
				,(	SELECT	ISNULL(MAX([SlNo]),0)+1 
					FROM	[StockTransactionsDetail] 
					WHERE	[TransactionDate]	=	@V_SFH_DATE
						AND [Item]				=	@V_ITEM 
						AND [ModuleId]			=	@V_SFD_DEPT_STORE)
				,@V_ITEM
				,@V_QTY_APPROVED
				,@V_SFD_UOM
				,0
				,NULL
				,@V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,@V_ITM_UOM)
				,0
				,@V_ITM_UOM
				,@V_SFD_DEPT_STORE 
				,@V_SFH_BIZUNIT
				,@V_SFD_PO_RATE
				,@V_SFD_PO_RATE
			)
		
		-----------------------------------------------------------------
		--Recalculate TRX RATE if any trx OUT occured after this trx IN
		-----------------------------------------------------------------
		EXEC	[StockRateUpdate]
				@P_ITD_PK_NEW	=	@V_ITD_PK
				,@P_RET_VAL		=	@pRetVal OUTPUT
		IF	@pRetVal < 0
		BEGIN
			GOTO StockTransferApproveFailure;
		END
		------------------------------------------------------------------
		
		-------------------------------
		--Update Btach-wise Stock (RCVD)
		-------------------------------
		SET		@V_SBD_PK		= NULL
		
		SELECT	@V_SBD_PK		= [Id]				--Destination Store Batch
		FROM	[StockBatchDetail] T
		WHERE	[Item]		=	@V_ITEM  
			AND [Module]	=	@V_SFD_DEPT_STORE
			AND	[BatchType]	=	1
			AND	[BatchId]	=	@V_SFH_PK
			AND [BatchNo]   = @V_SFD_BATCH_NO
			AND [BatchDetailId]=	@GDRId
		
		IF @V_SBD_PK IS NULL
		BEGIN
			SELECT  @V_SBD_PK = ISNULL(MAX([Id]),0)+1 FROM [StockBatchDetail];

			INSERT INTO [StockBatchDetail] 
				(
					 [Id]
					,[Item]
					,[Module]
					,[SlNo]
					,[BatchType]
					,[BatchId]
					,[BatchNo]
					,[BatchDate]
					,[QuantityInStock]
					,[QuantityPnTransit]
					,[QuantityReserved]
					,[QuantityDamaged]
					,[Uom]
					,[Rate]
					,[Value]
					,[BizUnit]
					,[ModifiedBy]
					,[ModifiedDate]
					,[BatchLotNo]
					,[BatchDetailId]
					,[DateOfManufacture]
					,[ExpiryDate]
					,[ActualBatchNo]
				)
			SELECT	@V_SBD_PK
					,@V_ITEM			AS	[SBD_ITEM]
					,@V_SFD_DEPT_STORE	AS	[SBD_DEPT]
					,(	SELECT	ISNULL(MAX([SlNo]),0) + 1 
						FROM	[StockBatchDetail]
						WHERE	[BatchDate]	= @V_SFH_DATE
							AND [Item]		= @V_ITEM
							)			AS	[SBD_SL_NO]
					,1	--SA Batch		AS	[SBD_BATCH_TYPE]
					,@V_SFH_PK			AS	[SBD_BATCH_PK]
					,ISNULL(@V_SFD_BATCH_NO,0)	AS	[SBD_BATCH_NO]
					,@V_SFH_DATE		AS	[SBD_BATCH_DATE]
					,@V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,@V_ITM_UOM)	AS	[SBD_QTY_IN_STOCK]
					,0					AS	[SBD_QTY_PN_TRANSIT]		
					,0					AS	[SBD_QTY_RESERVED]
					,0					AS	[SBD_QTY_DAMAGED]
					,@V_ITM_UOM			AS	[SBD_UOM]
					,@V_SFD_PO_RATE		AS	[SBD_RATE]
					,@V_SFD_PO_RATE * @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,@V_ITM_UOM)	AS	[SBD_VALUE]
					,@V_SFH_BIZUNIT		AS	[SBD_BIZUNIT]
					,@V_USER_PK		AS	[SBD_MOD_BY]
					,GETDATE()			AS	[SBD_MOD_DT]
					,@V_SFD_GRN_BATCH_NO AS	[SBD_BATCH_LOT_NO]
					,@GDRId
					,@V_SFD_MANUFACTURE_DATE AS [SBD_MANUFACTURE_DATE]
					,@V_SFD_EXPIRY_DATE	AS	[SBD_EXPIRY_DATE]
					,@V_SFD_GRN_BATCH_NO 
AS	[SBD_ACTUAL_BATCH_NO]
			----------------------------------------------
			--Update Batch-wise Stock transaction (RCVD)
			----------------------------------------------
			INSERT INTO [StockBatchTransactionDetails]
				(
					 [BatchDetail]
					,[BatchType]
					,[BatchPk]
					,[BatchNo]
					,[BatchDate]
					,[Module]
					,[TransactionMode]
					,[Transaction]
					,[TransactionNo]
					,[TransactionDate]
					,[SlNo]
					,[Item]
					,[QuantityReciept]
					,[UomReciept]
					,[QuantityIssue]
					,[UomIssue]
					,[QuantityStockReciept]
					,[QuantityStockIssue]
					,[UomStock]
					,[Rate]
					,[RateDetail]
					,[Value]
					,[ModuleId]
					,[Bizunit]
					,[ModifiedDate]
					,[ModifiedBy]
					
					,[ReferenceDescription1]
					,[ReferenceDescription2]
					,[ReferenceDescription3]
					,[ReferenceDescription5]
					,[referenceDate1]
				)
			SELECT	@V_SBD_PK			AS	[BTD_BATCH_DTL]
					,1					AS	[BTD_BATCH_TYPE]	--SA Batch Type
					,@V_SFH_PK			AS	[BTD_BATCH_PK]
					,@V_SFD_BATCH_NO	AS	[BTD_BATCH_NO]
					,@V_SFH_DATE		AS	[BTD_BATCH_DATE]
					,2					AS	[BTD_MODULE]		--Inventory
					,12					AS	[BTD_TRX_MODE]		--Stk transfer
					,@V_SFH_PK			AS	[BTD_TRN]
					,@V_TXN_NO			AS	[BTD_TRN_NO]
					,@V_SFH_DATE		AS	[BTD_TRN_DATE]
					,1					AS	[BTD_SL_NO]
					,@V_ITEM			AS	[BTD_ITEM]
					,@V_QTY_APPROVED	AS	[BTD_QTY_RCP]
					,@V_SFD_UOM			AS	[BTD_UOM_RCP]
					,0					AS	[BTD_QTY_ISS]
					,NULL				AS	[BTD_UOM_ISS]
					,@V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,@V_ITM_UOM)	AS	[BTD_QTY_STK_RCP]
					,0					AS	[BTD_QTY_STK_ISS]
					,@V_ITM_UOM			AS	[BTD_UOM_STK]
					,@V_SFD_PO_RATE		AS	[BTD_RATE]
					,@V_SFD_PO_RATE		AS	[BTD_RATE_DTL]
					,@V_SFD_PO_RATE	 * @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,@V_ITM_UOM)	AS	[BTD_VALUE]
					,@V_SFD_DEPT_STORE	AS	[BTD_DEPT]
					,@V_SFH_BIZUNIT		AS	[BTD_BIZUNIT]
					,GETDATE()			AS	[BTD_MOD_DT]
					,@V_USER_PK			AS	[BTD_MOD_BY]
					
					,@V_BTD_REF_DESC1	AS	[BTD_REF_DESC1]
					,@V_BTD_REF_DESC2	AS	[BTD_REF_DESC2]
					,@V_BTD_REF_DESC3	AS	[BTD_REF_DESC3]
					,@V_BTD_REF_DESC5	AS	[BTD_REF_DESC5]
					,@V_BTD_REF_DATE1	AS	[BTD_REF_DATE1]
		END
		ELSE
		BEGIN

			UPDATE 	[StockBatchDetail]
			SET	 [QuantityInStock]	= [QuantityInStock] + @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])
				,[Value]		=	CASE	WHEN	[Rate] = 0
											THEN	@V_SFD_PO_RATE * (	[QuantityInStock] + @V_QTY_APPROVED  * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom]))
											ELSE	[Value] + ( @V_SFD_PO_RATE * @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])) --total value
									END
				,[Rate]			=	CASE	WHEN	[Rate] = 0
											THEN	@V_SFD_PO_RATE
											ELSE	ISNULL(
															( [Value] + ( @V_SFD_PO_RATE * @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom]))) --total value
															 /
															 CASE	WHEN	([QuantityInStock] + @V_QTY_APPROVED  * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])) = 0
																	THEN	NULL
																	ELSE	([QuantityInStock] + @V_QTY_APPROVED  * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,[Uom])) --total stock
															 END,0)
													 
									END
				,[BatchLotNo]	=	@V_SFD_GRN_BATCH_NO
				,[ActualBatchNo] =	@V_SFD_GRN_BATCH_NO
				,[ModifiedBy]	=	@V_USER_PK
				,[ModifiedDate]	=	GETDATE()
			WHERE	[Id]	=	@V_SBD_PK

			--------------------------------
			--Update batch trx entry
			--------------------------------
			IF EXISTS	(	SELECT	1
							FROM	[StockBatchTransactionDetails]	T
							INNER JOIN [StockBatchDetail]			B	ON	B.[Id]	=	T.[BatchDetail]
							WHERE	B.[Id]				=	@V_SBD_PK
								AND	T.[TransactionMode]	=	12
								AND	T.[Transaction]		=	@V_SFH_PK
							)
			BEGIN
				UPDATE	T 
				SET		 [QuantityReciept]		=	B.[QuantityInStock]
						,[QuantityStockReciept]	=	B.[QuantityInStock]
						,[Rate]					=	B.[Rate]

						,[RateDetail]			=	B.[Rate]
						,[Value]				=	B.[Rate] * B.[QuantityInStock]
				FROM	[StockBatchTransactionDetails]	T
				INNER JOIN [StockBatchDetail]			B	ON	B.[Id]	=	T.[BatchDetail]
				WHERE	B.[Id]				=	@V_SBD_PK
					AND	T.[TransactionMode]	=	12
					AND	T.[Transaction]		=	@V_SFH_PK
			END
			ELSE
			BEGIN
				INSERT INTO [StockBatchTransactionDetails]
					(
						 [BatchDetail]
						,[BatchType]
						,[BatchPk]
						,[BatchNo]
						,[BatchDate]
						,[Module]
						,[TransactionMode]
						,[Transaction]
						,[TransactionNo]
						,[TransactionDate]
						,[SlNo]
						,[Item]
						,[QuantityReciept]
						,[UomReciept]
						,[QuantityIssue]
						,[UomIssue]
						,[QuantityStockReciept]
						,[QuantityStockIssue]
						,[UomStock]
						,[Rate]
						,[RateDetail]
						,[Value]
						,[ModuleId]
						,[Bizunit]
						,[ModifiedDate]
						,[ModifiedBy]

						,[ReferenceDescription1]
						,[ReferenceDescription2]
						,[ReferenceDescription3]
						,[ReferenceDescription5]
						,[referenceDate1]
					)
				SELECT	@V_SBD_PK			AS	[BTD_BATCH_DTL]
						,1					AS	[BTD_BATCH_TYPE]	--SA Batch Type
						,@V_SFH_PK			AS	[BTD_BATCH_PK]
						,@V_SFH_NO			AS	[BTD_BATCH_NO]
						,@V_SFH_DATE		AS	[BTD_BATCH_DATE]
						,2					AS	[BTD_MODULE]		--Inventory
						,12					AS	[BTD_TRX_MODE]		--Stk transfer
						,@V_SFH_PK			AS	[BTD_TRN]
						,@V_SFH_NO			AS	[BTD_TRN_NO]
						,@V_SFH_DATE		AS	[BTD_TRN_DATE]
						,1					AS	[BTD_SL_NO]
						,@V_ITEM			AS	[BTD_ITEM]
						,@V_QTY_APPROVED	AS	[BTD_QTY_RCP]
						,@V_SFD_UOM			AS	[BTD_UOM_RCP]
						,0					AS	[BTD_QTY_ISS]
						,NULL				AS	[BTD_UOM_ISS]
						,@V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,@V_ITM_UOM)	AS	[BTD_QTY_STK_RCP]
						,0					AS	[BTD_QTY_STK_ISS]
						,@V_ITM_UOM			AS	[BTD_UOM_STK]
						,@V_SFD_PO_RATE		AS	[BTD_RATE]
						,@V_SFD_PO_RATE		AS	[BTD_RATE_DTL]
						,@V_SFD_PO_RATE * @V_QTY_APPROVED * dbo.[FNINV_UOM_CONV_FACT_GET](@V_SFD_UOM,@V_ITM_UOM)	AS	[BTD_VALUE]
						,@V_SFD_DEPT_STORE	AS	[BTD_DEPT]
						,@V_SFH_BIZUNIT		AS	[BTD_BIZUNIT]
						,GETDATE()			AS	[BTD_MOD_DT]
						,@V_USER_PK			AS	[BTD_MOD_BY]

						,@V_BTD_REF_DESC1	AS	[BTD_REF_DESC1]
						,@V_BTD_REF_DESC2	AS	[BTD_REF_DESC2]
						,@V_BTD_REF_DESC3	AS	[BTD_REF_DESC3]
						,@V_BTD_REF_DESC5	AS	[BTD_REF_DESC5]
						,@V_BTD_REF_DATE1	AS	[BTD_REF_DATE1]
			END
		END

		SET @V_ROW_NO = @V_ROW_NO + 1;
	END --End of while loop
	
	UPDATE	T
	SET	 [QuantityInStock]	= ISNULL((	SELECT	SUM(B.[QuantityStockReciept] - B.[QuantityStockIssue] )
											FROM	[StockBatchTransactionDetails] B
											WHERE	B.[BatchDetail]	=	T.[Id]
									),0)
		,[Rate]				= CASE WHEN EXISTS (	SELECT	B.[RateDetail]
												FROM	[StockBatchTransactionDetails] B
												WHERE	B.[BatchDetail]			=	T.[Id]
													AND	B.[QuantityStockReciept]>	0)
								THEN (	SELECT	TOP 1 B.[RateDetail]
										FROM	[StockBatchTransactionDetails] B
										WHERE	B.[BatchDetail] = T.[Id]
											AND	B.[QuantityStockReciept] > 0)
								ELSE T.[Rate]
						 END
	FROM	[StockBatchDetail] T
	WHERE	[BatchType]	=	1	--SA
		AND	[BatchId]	=	@V_SFH_PK

	UPDATE	T
	SET		T.[Value] = T.[QuantityInStock] * T.[Rate] 
	FROM	[StockBatchDetail] T
	WHERE	[BatchType]	=	1	--SA
		AND	[BatchId]	=	@V_SFH_PK

	----Update GRN qty w.r.t GIN qty __GIN IS NOT IN ERP2
	--MERGE	[INV_GIN_DTL]		AS O
	--USING	(	SELECT	*
	--			FROM	[INV_STK_TRAN_GIN_MAP]
	--			WHERE	[ISG_ST] = 	@V_SFH_PK)	AS T
	--ON		(O.[GID_PK] = T.[ISG_GIN_DTL])
	--WHEN	MATCHED 
	--THEN 
	--UPDATE 
	--SET	O.[GID_QTY_TRANSFERED] = (ISNULL(O.[GID_QTY_TRANSFERED],0) + ISNULL(O.GID_QTY_APPROVED,0) - ISNULL(O.[GID_QTY_REJECTED],0));

	------------------------------------------
	---Update Stock value with Invoiced Rate
	------------------------------------------
	
	INSERT INTO @T_INVOICE_PO_DTL
		(
			[IVH_PK]
		)
	SELECT	[In
voiceVendorHeaderId]
	FROM
	(
		SELECT	MIN(V.[InvoiceVendorHeaderId]) AS [InvoiceVendorHeaderId] ,V.[PurchaseOrderDetailId]--, [GID_GRN_DTL]
		FROM	[StockTranDetail]						S
		INNER JOIN [VwFinanceInvoiceVendorDetail]		V	ON  V.[PurchaseOrderDetailId]	=	S.[PurchaseOrderDetailId] 
															AND V.[DeletedStatus]			=	0
		WHERE	S.[Id] = @V_SFH_PK
		GROUP BY V.[PurchaseOrderDetailId]
	)T

	SET		@V_ROW_NO	= 1;
	
	SELECT @V_MAX_ROW_NO = MAX([ROW_NO]) FROM @T_INVOICE_PO_DTL
	WHILE (@V_ROW_NO <= @V_MAX_ROW_NO) 
	BEGIN
		SELECT	@V_IVH_PK	= [IVH_PK] 
		FROM	@T_INVOICE_PO_DTL
		WHERE	[ROW_NO]	= @V_ROW_NO

		IF EXISTS (	SELECT	[Id]
					FROM	[FinanceInvoiceVendorHeader]
					WHERE	[Id]				= @V_IVH_PK
						AND [DeletedStatus]		= 0
						AND [HasJournalEntry]	= 1
					)
		BEGIN
			EXEC	[FinanceInvoiceStockUpdate]
					@P_IVH_PK	= @V_IVH_PK
					,@P_USER_PK = @V_USER_PK
					,@P_RET_VAL	= @pRetVal	OUTPUT
		END

		SET @V_ROW_NO = @V_ROW_NO + 1;
	END
	
	-------------------
	----STOCK DWH ETL
	-------------------
	--EXEC	[SPDWH_INV_STK_TRX_DTL_SAVE]
	--		@P_DIS_TRX_MODE	= 12			--Stock Admission
	--		,@P_IS_DELETE	= 0				--Save
	--		,@P_DIS_TRN		= @V_SFH_PK
	--		,@P_RET_VAL		= @pRetVal	OUTPUT

	--IF @pRetVal < 0
	--BEGIN
	--	RETURN
	--END

	SET @pRetVal	= @V_SFH_PK

	IF @V_TRANSACTION_STARTED = 1
		COMMIT TRANSACTION;

	RETURN;

	StockTransferApproveFailure:
	IF @V_TRANSACTION_STARTED = 1 AND XACT_STATE() <> 0
		ROLLBACK TRANSACTION;
	ELSE IF @V_TRANSACTION_STARTED = 0 AND XACT_STATE() = 1
		ROLLBACK TRANSACTION StockTransferApprove;
	RETURN;

	END TRY

	BEGIN CATCH
		IF @V_TRANSACTION_STARTED = 1 AND XACT_STATE() <> 0
			ROLLBACK TRANSACTION;
		ELSE IF @V_TRANSACTION_STARTED = 0 AND XACT_STATE() = 1
			ROLLBACK TRANSACTION StockTransferApprove;

		SET @pRetVal	= -1
		SELECT	ERROR_LINE()	ErrorLine,	ERROR_MESSAGE()		ErrorMessage,
				ERROR_NUMBER()	ErrorNo,	ERROR_PROCEDURE()	ErrorProcedure
	END CATCH
END

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
