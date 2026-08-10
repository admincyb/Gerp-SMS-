/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	Author	:	
	Date	:	
	Purpose :	Job after Direct GRN re-submission
	Execute :	BEGIN TRANSACTION
					DECLARE @P_RET_VAL INT
					EXEC [SPWKF_JOB_AUTO_GRN_DIRECT_APR_STATUS] 10,NULL,@P_RET_VAL OUTPUT
					SELECT @P_RET_VAL [RETURN_VALUE]
					SELECT	* FROM	[WkfTransactionDtl] WHERE	[wtdReference] = 308
				ROLLBACK TRANSACTION
	[SPWKF_JOB_AUTO_GRN_DIRECT_APR_STATUS] >>>			[WorkFlowJobGoodsReceiptNoteApproveStatusAuto]
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	Modified By				On				Remarks
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	GTI-229             07-03-2022      Add separate condition to consume packing material stock (work order printing)
	GTI-229             23-03-2022      Change packing material consumption stock calculation (receipt - issued)
	GTI-229             30-06-2022      Chnage rapper sp of EMI consumption (rollback removed sp)
						11-06-2025		Added close query section for purchase order case(workflow inbox)
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
CREATE PROCEDURE [dbo].[WorkFlowJobGoodsReceiptNoteApproveStatusAuto]
(	
	 @pAppID		INT
	,@pRefID		INT
	,@pRetVal		INT OUTPUT
)
AS  
BEGIN
	BEGIN TRY
		DECLARE @V_GRH_PK		INT
				,@V_GIH_PK		INT
				,@V_SFH_PK		INT
				,@V_GRH_DATE	DATE
				,@V_POH_PK		INT
				,@V_POH_DEPT	INT
				,@V_POH_TYPE	INT = NULL				
				,@V_ROW_NO_MAX	INT				
				,@vProcess		INT

		DECLARE @T_BIN_PRODUCT TABLE	
			(	
				[ROW_NO]			INT,
				[RW]				INT,
				[BCH_BAL_PCS]		INT ,
				[BVD_TOTAL_PCS]		INT,
				[TOT_PCS]			INT,
				[WEI_PK]			INT,
				[GMD_PK]			INT
			)
		DECLARE @T_BIN_PACK TABLE	
			(	
				[ROW_NO]			INT,
				[RW]				INT,
				[SBD_PK]			INT ,
				[SBD_BATCH_NO]		NVARCHAR(500) ,
				[SBD_BAL_PCS]		FLOAT ,
				[BVD_TOTAL_PCS]		FLOAT,
				[TOT_PCS]			FLOAT,
				[BOM_PK]			INT,
				[SBD_UOM]			INT
			)

		DECLARE	@T_INV_GRN_MAT_RET_DTL TABLE 
			(	
				[T_ROW_NO]				INT IDENTITY(1,1)	
				,[T_GMD_PK]				INT
				,[T_GMD_QTY]			FLOAT
				,[T_GMD_MATERIAL]		INT
				,[T_GMD_BIN_CARD]		INT
				,[T_GMD_DEPT_TO]		INT
				,[T_GMD_AVG_GLOVE_WT]	FLOAT
				,[T_GMR_PK]				INT
			)
		DECLARE	@T_PUR_ORDER_HDR	TABLE
			(
				 [ROW_NO]		INT IDENTITY(1,1)
				,[POH_PK]		INT
				,[POH_DEPT]		INT
				,[POH_TYPE]		INT
			)
		DECLARE	 @i			INT
				,@j			INT
				,@DR_VAL	INT
				,@QTY		INT
				,@BAL_QTY	INT
				,@x			INT
				,@y			INT
			
				,@DR_VAL_PACK	FLOAT
				,@QTY_PACK		FLOAT
				,@BAL_QTY_PACK	FLOAT
				,@V_ROW_NO		INT
				,@V_MAX_ROW_NO	INT
				,@V_GMD_PK		INT
				,@V_USER_PK		INT
				,@V_GMD_BIN_CARD	INT
				,@V_PROD_XML	XML
				,@V_WIH_DEPT_TO	INT

		DECLARE	 @V_X1			NVARCHAR(MAX)
				,@V_XML			XML
				,@V_RET_NO		NVARCHAR(200)	
				,@V_RET_REF_PK	INT	
				,@V_BID_PK		INT
				,@V_MAX_ROW		INT
				,@V_ROW			INT
				
		SET	@V_GRH_PK		=	@pAppID
		
		SELECT @V_SFH_PK =[StockHeaderId]
		FROM [STOCKTRANGRNHeaderMap]
		WHERE [GoodsReceiptHeaderId]	=@V_GRH_PK

		-------------------
		--GRN approval Job
		-------------------
		EXEC	[WorkFlowJobAutoGoodsReceiptApproveStatus] --SPWKF_JOB_AUTO_GRN_APR_STATUS
				@pAppID		= @V_GRH_PK
				,@pRefID	= @pRefID
				,@pRetVal	= @pRetVal
				
		IF	@PRetVal < 0
		BEGIN
			RETURN
		END
		
		---------------------	--GIN Is Not in ERPV2
		----GIN approval Job
		---------------------
		--EXEC	[SPWKF_JOB_AUTO_GIN_APR_STATUS]
		--		@pAppID		= @V_GIH_PK
		--		,@pRefID	= @pRefID
		--		,@pRetVal	= @pRetVal	OUTPUT
		--IF	@PRetVal < 0
		--BEGIN
		--	RETURN
		--END

		-----------------------------
		--Stk Admission approval Job
		-----------------------------
		EXEC	[WorkFlowJobAutoTranApproveStatus]--[SPWKF_JOB_AUTO_TRAN_APR_STATUS]
				@pAppID		= @V_SFH_PK
				,@pRefID	= @pRefID
				,@pRetVal	= @pRetVal OUTPUT
				
		IF	@PRetVal < 0
		BEGIN
			RETURN
		END

 		-----------------
----------------------------
		--Completing Workflow trx of previous process
		---------------------------------------------
		IF EXISTS (	SELECT	[Id]
					FROM	[AppConfigurationMaster]
					WHERE	[Setting]	= 'LINKED SMS PROCESS'
						AND	[Value]		= 1)
		BEGIN
			INSERT  INTO @T_PUR_ORDER_HDR
				(	
					 [POH_PK]
					,[POH_DEPT]
					,[POH_TYPE]
				)
			SELECT	DISTINCT
					 D.[Po]
					,D.[Module]
					,H.[Type]
			FROM	[GoodsReceiptNoteDetail]	M
			INNER JOIN [PurchaseOrderDetail]	D	ON	D.[Id]	=	M.[PurchaseOrderDetailId]
			INNER JOIN [PurchaseOrderHeader]	H	ON	H.[Id]	=	D.[Po]
			WHERE	M.[GoodsReceiptHeaderId] = @pAppID
		
			--Current Process 
			SELECT	@vProcess	=	[ProcessId]
			FROM [TransactionReferenceMaster]
			WHERE [RefId]	=	@pRefID
		
			SET	@V_ROW_NO = 1
			SELECT	@V_ROW_NO_MAX = MAX(ROW_NO) FROM	@T_PUR_ORDER_HDR
			WHILE(@V_ROW_NO <= @V_ROW_NO_MAX)
			BEGIN
			    
				SELECT	 @V_POH_PK		= [POH_PK]
						,@V_POH_DEPT	= [POH_DEPT]
						,@V_POH_TYPE	= [POH_TYPE]
				FROM	@T_PUR_ORDER_HDR
				WHERE	[ROW_NO]		= @V_ROW_NO
				IF @V_POH_PK IS NOT NULL 
				BEGIN
					EXEC	[TransactionClose]
							 @pProcess	= @vProcess
							,@pAppPK	= @V_POH_PK	
							,@pDept		= @V_POH_DEPT
							,@pRetVal	= @PRetVal	OUTPUT
					IF	@PRetVal < 0
					BEGIN
						RETURN
					END
				END
				SET @V_ROW_NO = @V_ROW_NO + 1
			END	--End of Loop
		END	--end config checking

		IF EXISTS(	SELECT 1
					FROM [GoodsReceiptNoteDetail]	G
					INNER JOIN [PurchaseOrderDetail]O	ON	O.[Id]=G.[PurchaseOrderDetailId]
					INNER JOIN ItemMaster I             ON  I.Id  = G.Item
					LEFT JOIN [StockBatchDetail]	S	ON	S.[BatchDetailId]=G.[Id]	
														AND	S.[BatchType]	 =1
					WHERE	G.[QuantityApproved]	> 0
					    AND I.IsReceiptBatchRequired = 1
						AND	S.[Id] IS NULL
						AND	G.[GoodsReceiptHeaderId]=@V_GRH_PK
				)
		BEGIN
			SET @pRetVal	=	-700--Stock not entred
			RETURN
		END

		IF EXISTS(	SELECT 1
					FROM [GoodsReceiptNoteDetail]	G
					INNER JOIN [PurchaseOrderDetail]O	ON	O.[Id]=G.[PurchaseOrderDetailId]
					INNER JOIN ItemMaster I             ON  I.Id  = G.Item
					LEFT JOIN [StockBatchDetail]	S	ON	S.[BatchDetailId]=G.[Id]	
														AND	S.[BatchType]	=1
					WHERE	O.[Rate]	>	0
					    AND I.IsReceiptBatchRequired = 1
						AND	S.[Rate]	=	0
						AND	G.[GoodsReceiptHeaderId]=@V_GRH_PK
				)
		BEGIN
			SET @pRetVal	=	-701--Stock Rate not entred
			RETURN
		END
		
		-------------------
		--Update GRN Status
		-------------------
		UPDATE	[GoodsReceiptNoteHeader]
		SET	 [Status]		=	2 --Approved
			,[VerifiedBy]	=	dbo.[FnWkfRefUserGet] (@pRefID) 
			,[VerifiedDate]	=	GETDATE() 
			,@V_GRH_DATE	= [Date]
			,@V_USER_PK		= dbo.[FnWkfRefUserGet] (@pRefID)
		WHERE	[Id] = @V_GRH_PK;
		 
			--	IF EXISTS(	SELECT	1
			--				FROM	[INV_GRN_DTL]
			--					INNER JOIN	[INV_GIN_DTL]	ON	[GID_GRN_DTL]	=	[GRD_PK]
			--				WHERE	[GRD_GR]  = @pAppID
			--					AND	[GID_QTY_REJECTED] <> 0	)
			--	BEGIN	
			--		EXEC	[SPINV_GOODS_REJECTION_MAIL_SAVE]	
			--				@P_PK		=	@pAppID
			--				,@P_REF_PK	=	@pRefID
			--				,@P_TYPE	=	2
			--				,@P_RET_VAL	=	@PRetVal	OUTPUT

			--		IF	@PRetVal < 0
			--			BEGIN
			--				RETURN
			--			END
			--	END	

			--INSERT INTO @T_BIN_PACK	
			--(
			--		 [ROW_NO]
			--		,[RW]		
			--		,[SBD_PK]		
			--		,[SBD_BATCH_NO]		
			--		,[SBD_BAL_PCS]	
			--		,[BVD_TOTAL_PCS]
			--		,[TOT_PCS]	
			--		,[BOM_PK]	
			--		,[SBD_UOM]	
			--)
			--SELECT		 ROW_NUMBER() OVER (PARTITION BY [WIB_PK] ORDER BY [SBD_PK])
			--			,DENSE_RANK() OVER(ORDER BY [WIB_PK])
			--			,[SBD_PK]									
			--			,[SBD_BATCH_NO]
			--			,[SBD_QTY_BALANCE]					AS	[BCH_BAL_PCS]
			--			,CAST((ISNULL(([WIB_ACT_QTY]/[WID_QTY])*[GRD_QTY_RECEIVED],0)-ISNULL([PACK_QTY],0))	AS NUMERIC(19,6))
			--												AS	[BVD_TOTAL_PCS]	
			--			,0									AS  [TOT_PCS]					
			--			,[WIB_PK]							AS	[BVD_PRODUCT]
			--			,[SBD_UOM
]							AS	[SBD_UOM]			
			--FROM	   [INV_WORK_ORDER_ITEM_BOM_DTL]
			----LEFT  JOIN [INV_WORK_ORDER_ITEM_BOM_STK_ALLOC]	ON [WIB_PK]	= [WSA_WIB]
			----LEFT  JOIN [INV_ITEM_ISSUE_DTL]	B				ON [WIB_PK] = [MID_WO_DTL]
			--OUTER APPLY (SELECT [MID_WO_DTL] 
			--             FROM  [INV_ITEM_ISSUE_DTL] 
			--			 WHERE  [WIB_PK] = [MID_WO_DTL] 
			--			 GROUP BY [MID_WO_DTL]) AS B
			--INNER JOIN [INV_WORK_ORDER_ITEM_DTL]			ON [WID_PK] = [WIB_WID]
			--INNER JOIN [INV_WORK_ORDER_ITEM_HDR] A			ON [WIH_PK] = [WID_WIH]	
			--INNER JOIN [INV_GRN_DTL]						ON [WID_PK] = [GRD_WO_DTL]
			--CROSS APPLY ( SELECT [SBD_PK]
			--					,[SBD_BATCH_NO]
			--					,[SBD_BATCH_LOT_TEXT]
			--					,[SBD_QTY_IN_STOCK]
			--					,[SBD_UOM]
			--					,SUM([BTD_QTY_STK_RCP])-[BTD_QTY_STK_ISS] AS [SBD_QTY_BALANCE]
			--			  FROM
			--			  (		    SELECT DISTINCT  B.[SBD_PK]
			--								,B.[SBD_BATCH_NO]
			--								,B.[SBD_BATCH_LOT_TEXT]
			--								,B.[SBD_QTY_IN_STOCK]
			--								,B.[SBD_UOM]
			--								--,[SBD_QTY_BALANCE]
			--								--,[BTD_QTY_STK_RCP]-ISNULL(CONSUMED.BTD_QTY_STK_ISS,0) AS [SBD_QTY_BALANCE]
			--							    ,[MID_QTY_ISSUED] AS [BTD_QTY_STK_RCP]
			--								,ISNULL(CONSUMED.BTD_QTY_STK_ISS,0) AS [BTD_QTY_STK_ISS]
									
			--						FROM  [INV_ITEM_ISSUE_DTL] B1
			--						INNER JOIN [INV_ITEM_ISSUE_HDR]		   ON  [MIH_PK]			  = [MID_MI]
			--						INNER JOIN [INV_STK_BATCH_DTL]	AO	   ON  AO.[SBD_PK]		  = [MID_STK_BATCH]
			--						INNER JOIN [INV_STK_BATCH_DTL]	B	   ON (B.[SBD_ITEM]		  = AO.[SBD_ITEM]  
			--															   AND B.[SBD_DEPT]		  = [MIH_DEPT_TO]
			--															   AND B.[SBD_BATCH_TYPE] = AO.[SBD_BATCH_TYPE]
			--															   AND B.[SBD_BATCH_PK]	  = AO.[SBD_BATCH_PK])
			--						INNER JOIN [INV_STK_BATCH_TRX_DTL] RCP ON  B.[SBD_PK]         = RCP.[BTD_BATCH_DTL] 
			--															   AND RCP.BTD_TRN        = B1.[MID_MI]
			--						OUTER APPLY (
			--									  SELECT ISNULL(SUM(ISS.[BTD_QTY_STK_ISS]),0)  AS BTD_QTY_STK_ISS
			--									  FROM [INV_STK_BATCH_TRX_DTL] ISS
			--									  INNER JOIN [INV_ITEM_CONS_HDR] ON [ICH_PK]            = ISS.[BTD_TRN]
			--									                                AND ISS.[BTD_TRX_MODE]  = 20 -- EMI
			--									   OUTER APPLY (SELECT ICD_WO_DTL 
			--													FROM  [INV_ITEM_CONS_DTL] 
			--													WHERE [ICD_ITEM_CONS_HDR] = [ICH_PK]
			--													  AND [ICD_STK_BATCH]     = ISS.[BTD_BATCH_DTL]
			--													GROUP BY ICD_WO_DTL)AS MID
			--									  WHERE ISS.[BTD_BATCH_DTL] = RCP.[BTD_BATCH_DTL]
			--									    AND MID.[ICD_WO_DTL]    = B1.[MID_WO_DTL]
			--									  HAVING SUM(ISS.[BTD_QTY_STK_ISS]) IS NOT NULL
										  
			--									  UNION 

			--									  SELECT ISNULL(SUM(ISS.[BTD_QTY_STK_ISS]),0)  AS BTD_QTY_STK_ISS
			--									  FROM [INV_STK_BATCH_TRX_DTL] ISS
			--									  INNER JOIN [PRD_BIN_CARD_CONVERSION_HDR]      ON [BVH_PK]             = ISS.[BTD_TRN]
			--									                                               AND ISS.[BTD_TRX_MODE]   = 38 --Wallet/Pouch
			--									  INNER JOIN [PRD_BIN_CARD_CONVERSION_PACK_DTL] ON [BVP_CONVERSION_HDR] = [BVH_PK]
			--									  								               AND [BVP_STK_BATCH]      = ISS.[BTD_BATCH_DTL]
			--									  INNER JOIN [INV_GRN_HDR]                      ON [GRH_PK]             = [BVH_GRH_PK]
			--									 											   AND [GRH_DEL_STATUS]     = 0
			--									  INNER JOIN [INV_GRN_DTL]					   ON  [GRD_GR]             = [GRH_PK] 
			--									 											   AND [GRD_ITEM]           = [WID_ITEM]
			--									  INNER JOIN [INV_WORK_ORDER_ITEM_DTL]          ON [WID_WIH]			= [GRD_WO]
			--									  WHERE ISS.[BTD_BATCH_DTL] = RCP.[BTD_BATCH_DTL]
			--									    AND [BVH_WO]        = B1.[MID_WO]
			--										AND [BVH_DEL_STATUS]= 0
			--									  HAVING SUM(ISS.[BTD_QTY_STK_ISS]) IS NOT NULL
			--									)  AS CONSUMED
			--						WHERE  ((B.[SBD_DEPT] = CASE WHEN [WIH_COMPANY] = 1 THEN 270 ELSE 202
70 END
			--						  AND  B.[SBD_ITEM] = [WIB_MATERIAL]
			--						  AND  B.[SBD_QTY_IN_STOCK]>0
			--						  AND  RCP.[BTD_TRX_MODE] =8
			--						  AND  B1.[MID_WO_DTL]    = [WIB_PK]))
			--						  --OR B.[SBD_PK] = [WSA_SBD_PK])

			--						UNION ALL

			--						SELECT	DISTINCT  B.[SBD_PK]
			--								,B.[SBD_BATCH_NO]
			--								,B.[SBD_BATCH_LOT_TEXT]
			--								,B.[SBD_QTY_IN_STOCK]
			--								,B.[SBD_UOM]
			--								--,ISNULL([WSA_ACT_QTY],[MID_QTY_ISSUED]) AS [SBD_QTY_BALANCE]
			--								--,ISNULL([WSA_ACT_QTY],[BTD_QTY_STK_RCP]-ISNULL(CONSUMED.BTD_QTY_STK_ISS,0)) AS [SBD_QTY_BALANCE]
			--								,ISNULL([WSA_ACT_QTY],0) AS [BTD_QTY_STK_RCP]
			--								,/*ISNULL(CONSUMED.BTD_QTY_STK_ISS,0) AS*/ 0 as [BTD_QTY_STK_ISS]
									
			--						FROM  INV_WORK_ORDER_ITEM_BOM_STK_ALLOC B1
			--						INNER JOIN [INV_STK_BATCH_DTL]	B	ON B.[SBD_PK] = [WSA_SBD_PK]
			--						OUTER APPLY (
			--									 SELECT ISNULL(SUM(ISS.[BTD_QTY_STK_ISS]),0)  AS BTD_QTY_STK_ISS
			--									 FROM [INV_STK_BATCH_TRX_DTL] ISS
			--									 INNER JOIN [INV_ITEM_CONS_HDR] ON [ICH_PK]            = ISS.[BTD_TRN]
			--									                               AND ISS.[BTD_TRX_MODE]  = 20 -- EMI
			--									 OUTER APPLY (SELECT ICD_WO_DTL 
			--									              FROM  [INV_ITEM_CONS_DTL] 
			--												  WHERE [ICD_ITEM_CONS_HDR] = [ICH_PK]
			--									 			    AND [ICD_STK_BATCH]     = ISS.[BTD_BATCH_DTL]
			--												  GROUP BY ICD_WO_DTL)AS MID
			--									 WHERE ISS.[BTD_BATCH_DTL] = B1.[WSA_SBD_PK]
			--									   AND MID.[ICD_WO_DTL]    = B1.[WSA_WIB]
			--									 HAVING SUM(ISS.[BTD_QTY_STK_ISS]) IS NOT NULL
										 
			--									 UNION 
										 
			--									 SELECT ISNULL(SUM(ISS.[BTD_QTY_STK_ISS]),0)  AS BTD_QTY_STK_ISS
			--									 FROM [INV_STK_BATCH_TRX_DTL] ISS
			--									 INNER JOIN [PRD_BIN_CARD_CONVERSION_HDR]      ON [BVH_PK]             = ISS.[BTD_TRN]
			--									                                              AND ISS.[BTD_TRX_MODE]   = 38 --Wallet/Pouch
			--									 INNER JOIN [PRD_BIN_CARD_CONVERSION_PACK_DTL] ON [BVP_CONVERSION_HDR] = [BVH_PK]
			--									 								              AND [BVP_STK_BATCH]      = ISS.[BTD_BATCH_DTL]
			--									 INNER JOIN [INV_GRN_HDR]                      ON [GRH_PK]             = [BVH_GRH_PK]
			--																				  AND [GRH_DEL_STATUS]     = 0
			--									 INNER JOIN [INV_GRN_DTL]					   ON [GRD_GR]             = [GRH_PK] 
			--																				  AND [GRD_ITEM]           = [WID_ITEM]
			--									 INNER JOIN [INV_WORK_ORDER_ITEM_DTL]          ON [WID_WIH]			   = [GRD_WO]
			--									 WHERE ISS.[BTD_BATCH_DTL] = B1.[WSA_SBD_PK]
			--									   AND [BVH_WO]        = B1.[WSA_WIB]
			--									   AND [BVP_PACK_ITEM] = B1.[WSA_MATERIAL]
			--									   AND [BVH_DEL_STATUS]= 0
			--									 HAVING SUM(ISS.[BTD_QTY_STK_ISS]) IS NOT NULL
			--									)  AS CONSUMED
			--						WHERE (B1.[WSA_WIB] = [WIB_PK]
			--						  AND  B1.[WSA_MATERIAL]  = [WIB_MATERIAL])

			--			  )AS [Stock]
			--			  GROUP BY [SBD_PK]
			--					  ,[SBD_BATCH_NO]
			--					  ,[SBD_BATCH_LOT_TEXT]
			--					  ,[SBD_QTY_IN_STOCK]
			--					  ,[SBD_UOM]
			--					  ,[BTD_QTY_STK_ISS]
			--			) AS [STK]

			--OUTER APPLY (	SELECT SUM([BVP_QUANTITY]) AS [PACK_QTY] 
			--				FROM [PRD_BIN_CARD_CONVERSION_PACK_DTL]  
			--				INNER JOIN [PRD_BIN_CARD_CONVERSION_HDR] ON [BVH_PK] = [BVP_CONVERSION_HDR]
			--				WHERE   [BVH_GRH_PK]    = @V_GRH_PK
			--					AND [BVH_DEL_STATUS] = 0
			--					AND [BVP_PACK_ITEM] = [WIB_MATERIAL]
			--					AND [BVP_STK_BATCH] = [SBD_PK]
			--				) AS [PACK]
			--WHERE	[GRD_GR] = @V_GRH_PK 
			--	AND ( [WIB_ITEM_TYPE] = 1 OR (WIB_OPERTAION = 1 AND [WIB_ITEM_TYPE] IN (1,4)) )
			--GROUP BY [SBD_PK],[SBD_UOM],[WIB_PK],[SBD_BATCH_NO],[SBD_QTY_BALANCE],[WIB_PK],[WIB_ACT_QTY],[WID_QTY],[PACK_QTY],[GRD_QTY_RECEIVED]
			--SET @x = 1
			--SELECT @y=MAX([RW]) FROM @T_BIN_PACK 

			--WHILE (@x<=@y )
			--BEGIN
			--	SET @i = 1
			--	SELECT @DR_VAL_PACK=MAX([BVD_TOTAL_PCS]) FROM @T_BIN_PACK WHERE [RW] = @x 
			--	SELECT @j=MAX([ROW_NO]) FROM @T_BIN_PACK WHERE [RW] = @x 
			--	WHILE (@i<=@j )
			--	BEGIN
			--		SELECT @BAL_QTY_PACK=[SBD_BAL_PCS]-@DR_VAL_PACK FROM @T_BIN_PACK WHERE [RW] = @x  AND [ROW_NO]=@i
			--		IF (@BAL_QTY_PACK>=0)
			--		BEGIN
			--			SELECT @QTY_PACK=@DR_VAL_PACK
			--		END
			--		ELSE 
			--		BEGIN
			--			SELECT @QTY_PACK=[SBD_BAL_PCS] FROM @T_BIN_PACK WHERE [RW] = @x  AND [ROW_NO]=@i
			--		END
			--		UPDATE @T_BIN_PACK SET TOT_PCS=@QTY_PACK WHERE [RW] = @x  AND [ROW_NO]=@i
			--		SET @DR_VAL_PACK = @DR_VAL_PACK-@QTY_PACK
			--		SET @i=@i+1
			--	END
			--	SET @x=@x+1
			--END

			--IF EXISTS(	SELECT	1
			--			FROM	@T_BIN_PACK
			--			WHERE	[TOT_PCS] > 0
			--				)
			--	BEGIN	
		    
			--		SET @V_X1=
			--		(SELECT	0 AS [ICH_PK]
			--				,NULL AS [ICH_NO]
			--				,NULL AS [ICH_VERSION]
			--				,REPLACE(CONVERT(VARCHAR(11), [GRH_DATE], 106), ' ','-')	AS [ICH_DATE]
			--				,NULL AS [ICH_SHIFT]
			--				,1 AS [ICH_ITEM_TYPE]
			--				,(	SELECT	[CFG_DATA]
			--					FROM	[ADM_CONFIG_MST]
			--					WHERE	[CFG_TYPE]		=	'ITEM TYPE'	
			--						AND [CFG_VALUE]		=	1 )			AS	[ICH_ITEM_TYPE_TEXT]
							
			--				,1 AS [ICH_TRX_TYPE]	
			--				,GRH_NO AS [ICH_REF_NO]		
			--				,7 AS [ICH_ISS_RCV_TYPE]
			--				,(	SELECT	[CFG_DATA]
			--					FROM	[ADM_CONFIG_MST]
			--					WHERE	[CFG_TYPE]		=	'EXTERNAL ISS RCV TYPE'	
			--						AND [CFG_VALUE]		=	7 )		AS	[ICH_ISS_RCV_TYPE_TEXT]
			--				,(	SELECT	[CFG_PK]
			--					FROM	[ADM_CONFIG_MST]
			--					WHERE	[CFG_TYPE]		=	'EXTERNAL ISS RCV TYPE'	
			--						AND [CFG_VALUE]		=	7 )		AS	[ICH_ISS_RCV_TYPE_PK]

			--				,[GRH_VENDOR] AS [ICH_ISS_RCV_PK]
			--				,(SELECT [VEN_NAME] FROM PUR_VENDOR_MST WHERE VEN_PK=[GRH_VENDOR]) AS [ICH_ISS_RCV_NAME]
			--				,'Work Order' AS [ICH_REMARKS]
			--				,1 AS [ICH_STATUS]
			--				,[GRH_COMPANY] AS [ICH_COMPANY]
			--				,CASE WHEN [GRH_COMPANY] = 1 THEN 270 ELSE 20270 END		AS  [ICH_DEPT]
			--				,(	SELECT	[DPT_NAME]
			--					FROM	[ADM_DEPT_MST]
			--					WHERE	[DPT_PK] =	(CASE WHEN [GRH_COMPANY] = 1 THEN 270 ELSE 20270 END)	)						
			--																			AS	[ICH_DEPT_TEXT]
			--				,1 AS [ICH_ACTIVE]
			--				,[GRH_BIZUNIT] AS [ICH_BIZUNIT]
			--				,[GRH_BIZUNIT] AS [BizUnitPk]
			--				,[GRH_CRTD_BY] AS [ICH_CRTD_BY]
			--				,[GRH_CRTD_BY] AS [UserPk]
			--				,[GRH_CRTD_DT] AS [ICH_CRTD_DT]
			--				,NULL 												AS	[LAST_MOD_DT]
			--				,1 AS [WKF_TRX_FLAG]
			--				,'EMI' AS [APT_CODE]
			--				,1		AS [AST_DOC_MODE]
			--				,NULL AS [ICH_CRDR_NOTE_HDR]
			--				,(	SELECT	 0											AS [ICD_PK]
			--							,[GRD_SL_NO]								AS [ICD_SL_NO]
			--							,ROW_NUMBER() OVER(ORDER BY [GRD_SL_NO])	AS [ROW_NO]
			--							,[WIB_MATERIAL]								AS [ICD_ITEM]
			--							,(	SELECT	'('+[ITM_CODE] + ') '+ [ITM_NAME]
			--								FROM	[INV_ITEM_MST]
			--								WHERE	[ITM_PK] =	[WIB_MATERIAL]	)	AS	[ICD_ITEM_TEXT]
			--							,(  SELECT  ITM_NEED_BATCH_STK  
			--		                        FROM	[INV_ITEM_MST]  
			--		                        WHERE	[ITM_PK] = [WIB_MATERIAL] )		AS [ITM_NEED_BATCH_STK]  
			--							,(	SELECT	[ITM_CATEGORY]
			--								FROM	[INV_ITEM_MST]
			--								WHERE	[ITM_PK] =	[WIB_MATERIAL]	)	AS	[ICD_ITEM_CATEGORY]
			--							,(	SELECT	[ITC_NAME]
			--								FROM	[INV_ITEM_CATEGORY]
			--								WHERE	[ITC_PK] =	(	SELECT	[ITM_CATEGORY]
			--														FROM	[INV_ITEM_MST]
			--														WHERE	[ITM_PK] =	[WIB_MATERIAL]		
			--													 )	)				AS	[ICD_ITEM_CATEGORY_TEXT]
			--							,[SBD_PK]									AS	[ICD_STK_BATCH]
			--							,[SBD_BATCH_NO]								AS	[ICD_STK_BATCH_NO]
			--							,CAST([TOT_PCS] AS NUMERIC(19,6))			AS	[ICD_QTY_CONSUMED]
			--							,[SBD_UOM]									AS  [ICD_UOM]
			--							,(	SELECT	[UOM_CODE]
			--								FROM	[INV_UOM_MST]
			--								W
HERE	[UOM_PK] =	[WIB_UOM]	)		AS	[ICD_UOM_TEXT]
			--							,0											AS	[ICD_ITEM_TYPE]
			--							,'No'										AS	[ICD_ISRETURNTEXT]
			--							,'Work Order'								AS	[ICD_REMARKS]
			--							,0											AS	[ICD_CURRENT_STK]
			--							,0											AS	[ICD_VALUE_CONSUMED]
			--							,0											AS  [ICD_RATE]
			--							,NULL										AS  [ICD_EXPIRY_DATE]
			--						 	,NULL										AS	[ICD_LOT_NO]	 
			--						 	,NULL										AS	[ICD_CRDR_NOTE_DTL]
			--							,NULL										AS	[ICD_ISS_RCV_TYPE]    
			--							,NULL										AS	[ICD_ISS_RCV_PK]
			--							,NULL										AS	[ICD_ISS_RCV_SUB_TYPE] 	
			--							,''											AS	[ICD_ISS_RCV_TYPE_TEXT]		
			--							,NULL										AS	[ICD_ISS_RCV_NAME]
			--							,''											AS	[ICD_ISS_RCV_SUB_TYPE_TEXT]
			--							,[GRD_PK]									AS	[ICD_GRN_DTL]	
			--							,[WIB_PK]									AS	[ICD_WO_DTL]	
			--					FROM	@T_BIN_PACK
			--					INNER JOIN [INV_WORK_ORDER_ITEM_BOM_DTL] ON [BOM_PK] = [WIB_PK]
			--					INNER JOIN [INV_WORK_ORDER_ITEM_DTL]	 ON [WID_PK] = [WIB_WID]
			--					INNER JOIN [INV_GRN_DTL]				 ON [WID_PK] = [GRD_WO_DTL]
			--					WHERE	[TOT_PCS] > 0
			--					AND		[GRD_GR] = @V_GRH_PK
			--					ORDER BY [GRD_SL_NO]
			--					FOR XML PATH('ConsumptionDtl'),TYPE
			--				 )
			--		FROM	[INV_GRN_HDR] AS [root]
			--		WHERE	[GRH_PK]	=  @V_GRH_PK
			--		FOR XML AUTO, ELEMENTS)

			--	--IF EXISTS(	SELECT	1
			--	--			FROM	[INV_GRN_HDR]
			--	--			INNER JOIN [INV_GRN_DTL]					ON [GRH_PK] = [GRD_GR]
			--	--			INNER JOIN [INV_WORK_ORDER_ITEM_DTL]		ON [WID_PK] = [GRD_WO_DTL]
			--	--			INNER JOIN [INV_WORK_ORDER_ITEM_HDR]		ON [WIH_PK] = [WID_WIH]
			--	--			INNER JOIN [INV_WORK_ORDER_ITEM_BOM_DTL]	ON [WID_PK] = [WIB_WID]
			--	--			CROSS APPLY (	SELECT   [SBD_PK]
			--	--									,[SBD_BATCH_NO]
			--	--									,[SBD_BATCH_LOT_TEXT]
			--	--									,CAST([SBD_QTY_IN_STOCK] AS NUMERIC(19,6)) AS [SBD_QTY_IN_STOCK]
			--	--							FROM  [INV_STK_BATCH_DTL]
			--	--							INNER JOIN [INV_STK_BATCH_TRX_DTL] ON [SBD_PK]  = [BTD_BATCH_DTL]
			--	--							INNER JOIN INV_ITEM_ISSUE_DTL      ON [BTD_TRN] = [MID_MI]
			--	--							WHERE [SBD_DEPT] = CASE WHEN [WIH_COMPANY] = 1 THEN 270 ELSE 20270 END
			--	--							 AND  [SBD_ITEM] = [WIB_MATERIAL]
			--	--							 AND  [SBD_QTY_IN_STOCK]>0
			--	--							 AND  [BTD_TRX_MODE] =8
			--	--							 AND  [MID_WO_DTL] = [WIB_PK]
			--	--						) AS [STK]
			--	--			OUTER APPLY (	SELECT SUM([WSA_ACT_QTY])  AS [WSA_QTY]
			--	--							FROM [INV_WORK_ORDER_ITEM_BOM_STK_ALLOC] 
			--	--							WHERE [WSA_WIB] = [WIB_PK]
			--	--						 )
			--	--									AS [ALLOC]
			--	--			WHERE	[GRH_PK]  = @pAppID
			--	--				AND [GRH_IS_WORK_ORDER] = 1
			--	--				--AND [WIB_ITEM_TYPE] IN(1,4)
			--	--				AND [WIB_ITEM_TYPE] = 1
			--	--				AND CAST(ISNULL(([WIB_ACT_QTY]/[WID_QTY])*[GRD_QTY_RECEIVED],0)-ISNULL([WSA_QTY],0) AS NUMERIC(19,6))>0
			--	--				)
			--	--BEGIN	
		    
			--	--	SET @V_X1=
			--	--	(SELECT	0 AS [ICH_PK]
			--	--			,NULL AS [ICH_NO]
			--	--			,NULL AS [ICH_VERSION]
			--	--			,REPLACE(CONVERT(VARCHAR(11), [GRH_DATE], 106), ' ','-')	AS [ICH_DATE]
			--	--			,NULL AS [ICH_SHIFT]
			--	--			,1 AS [ICH_ITEM_TYPE]
			--	--			,(	SELECT	[CFG_DATA]
			--	--				FROM	[ADM_CONFIG_MST]
			--	--				WHERE	[CFG_TYPE]		=	'ITEM TYPE'	
			--	--					AND [CFG_VALUE]		=	1 )			AS	[ICH_ITEM_TYPE_TEXT]
							
			--	--			,1 AS [ICH_TRX_TYPE]	
			--	--			,GRH_NO AS [ICH_REF_NO]		
			--	--			,7 AS [ICH_ISS_RCV_TYPE]
			--	--			,(	SELECT	[CFG_DATA]
			--	--				FROM	[ADM_CONFIG_MST]
			--	--				WHERE	[CFG_TYPE]		=	'EXTERNAL ISS RCV TYPE'	
			--	--					AND [CFG_VALUE]		=	7 )		AS	[ICH_ISS_RCV_TYPE_TEXT]
			--	--			,(	SELECT	[CFG_PK]
			--	--				FROM	[ADM_CONFIG_MST]
			--	--				WHERE	[CFG_TYPE]		=	'EXTERNAL ISS RCV TYPE'	
			--	--					AND [CFG_VALUE]		=	7 )		AS	[ICH_ISS_RCV_TYPE_PK]

			--	--			,[GRH_VENDOR] AS [ICH_ISS_RCV_PK]
			--	--			,(SELECT [VEN_NAM
E] FROM PUR_VENDOR_MST WHERE VEN_PK=[GRH_VENDOR]) AS [ICH_ISS_RCV_NAME]
			--	--			,'Work Order' AS [ICH_REMARKS]
			--	--			,1 AS [ICH_STATUS]
			--	--			,[GRH_COMPANY] AS [ICH_COMPANY]
			--	--			,CASE WHEN [GRH_COMPANY] = 1 THEN 270 ELSE 20270 END		AS  [ICH_DEPT]
			--	--			,(	SELECT	[DPT_NAME]
			--	--				FROM	[ADM_DEPT_MST]
			--	--				WHERE	[DPT_PK] =	(CASE WHEN [GRH_COMPANY] = 1 THEN 270 ELSE 20270 END)	)						
			--	--																		AS	[ICH_DEPT_TEXT]
			--	--			,1 AS [ICH_ACTIVE]
			--	--			,[GRH_BIZUNIT] AS [ICH_BIZUNIT]
			--	--			,[GRH_BIZUNIT] AS [BizUnitPk]
			--	--			,[GRH_CRTD_BY] AS [ICH_CRTD_BY]
			--	--			,[GRH_CRTD_BY] AS [UserPk]
			--	--			,[GRH_CRTD_DT] AS [ICH_CRTD_DT]
			--	--			,NULL 												AS	[LAST_MOD_DT]
			--	--			,1 AS [WKF_TRX_FLAG]
			--	--			,'EMI' AS [APT_CODE]
			--	--			,1		AS [AST_DOC_MODE]
			--	--			,NULL AS [ICH_CRDR_NOTE_HDR]
			--	--			,(	SELECT	 0											AS [ICD_PK]
			--	--						,[GRD_SL_NO]								AS [ICD_SL_NO]
			--	--						,ROW_NUMBER() OVER(ORDER BY [GRD_SL_NO])	AS [ROW_NO]
			--	--						,[WIB_MATERIAL]								AS [ICD_ITEM]
			--	--						,(	SELECT	'('+[ITM_CODE] + ') '+ [ITM_NAME]
			--	--							FROM	[INV_ITEM_MST]
			--	--							WHERE	[ITM_PK] =	[WIB_MATERIAL]	)	AS	[ICD_ITEM_TEXT]
			--	--						,(  SELECT  ITM_NEED_BATCH_STK  
			--	--	                        FROM	[INV_ITEM_MST]  
			--	--	                        WHERE	[ITM_PK] = [WIB_MATERIAL] )		AS [ITM_NEED_BATCH_STK]  
			--	--						,(	SELECT	[ITM_CATEGORY]
			--	--							FROM	[INV_ITEM_MST]
			--	--							WHERE	[ITM_PK] =	[WIB_MATERIAL]	)	AS	[ICD_ITEM_CATEGORY]
			--	--						,(	SELECT	[ITC_NAME]
			--	--							FROM	[INV_ITEM_CATEGORY]
			--	--							WHERE	[ITC_PK] =	(	SELECT	[ITM_CATEGORY]
			--	--													FROM	[INV_ITEM_MST]
			--	--													WHERE	[ITM_PK] =	[WIB_MATERIAL]		
			--	--												 )	)				AS	[ICD_ITEM_CATEGORY_TEXT]
			--	--						,[SBD_PK]									AS	[ICD_STK_BATCH]
			--	--						,[SBD_BATCH_LOT_TEXT]						AS	[ICD_STK_BATCH_NO]
			--	--						,CAST(ISNULL(([WIB_ACT_QTY]/[WID_QTY])*[GRD_QTY_RECEIVED],0)-ISNULL([WSA_QTY],0) AS NUMERIC(19,6))	
			--	--																	AS	[ICD_QTY_CONSUMED]
			--	--						,[WIB_UOM]									AS  [ICD_UOM]
			--	--						,(	SELECT	[UOM_CODE]
			--	--							FROM	[INV_UOM_MST]
			--	--							WHERE	[UOM_PK] =	[WIB_UOM]	)		AS	[ICD_UOM_TEXT]
			--	--						,0											AS	[ICD_ITEM_TYPE]
			--	--						,'No'										AS	[ICD_ISRETURNTEXT]
			--	--						,'Work Order'								AS	[ICD_REMARKS]
			--	--						,0											AS	[ICD_CURRENT_STK]
			--	--						,0											AS	[ICD_VALUE_CONSUMED]
			--	--						,0											AS  [ICD_RATE]
			--	--						,NULL										AS  [ICD_EXPIRY_DATE]
			--	--					 	,NULL										AS	[ICD_LOT_NO]	 
			--	--					 	,NULL										AS	[ICD_CRDR_NOTE_DTL]
			--	--						,NULL										AS	[ICD_ISS_RCV_TYPE]    
			--	--						,NULL										AS	[ICD_ISS_RCV_PK]
			--	--						,NULL										AS	[ICD_ISS_RCV_SUB_TYPE] 	
			--	--						,''											AS	[ICD_ISS_RCV_TYPE_TEXT]		
			--	--						,NULL										AS	[ICD_ISS_RCV_NAME]
			--	--						,''											AS	[ICD_ISS_RCV_SUB_TYPE_TEXT]
			--	--						,[GRD_PK]									AS	[ICD_GRN_DTL]	
			--	--						,[WIB_PK]									AS	[ICD_WO_DTL]	
			--	--				FROM	[INV_GRN_DTL] 
			--	--				INNER JOIN [INV_WORK_ORDER_ITEM_DTL]							ON [WID_PK] = [GRD_WO_DTL]
			--	--				INNER JOIN [INV_WORK_ORDER_ITEM_BOM_DTL] AS [ConsumptionDtl]	ON [WID_PK] = [WIB_WID]
			--	--				INNER JOIN [INV_WORK_ORDER_ITEM_HDR]							ON [WIH_PK] = [WID_WIH]
			--	--				CROSS APPLY (	SELECT   [SBD_PK]
			--	--										,[SBD_BATCH_NO]
			--	--										,[SBD_BATCH_LOT_TEXT]
			--	--										,CAST([SBD_QTY_IN_STOCK] AS NUMERIC(19,6)) AS [SBD_QTY_IN_STOCK]
			--	--								FROM  [INV_STK_BATCH_DTL]
			--	--								INNER JOIN [INV_STK_BATCH_TRX_DTL] ON [SBD_PK]  = [BTD_BATCH_DTL]
			--	--								INNER JOIN INV_ITEM_ISSUE_DTL      ON [BTD_TRN] = [MID_MI]
			--	--								WHERE [SB
D_DEPT] = CASE WHEN [WIH_COMPANY] = 1 THEN 270 ELSE 20270 END
			--	--								 AND  [SBD_ITEM] = [WIB_MATERIAL]
			--	--								 AND  [SBD_QTY_IN_STOCK]>0
			--	--								 AND  [BTD_TRX_MODE] =8
			--	--								 AND  [MID_WO_DTL] = [WIB_PK]
			--	--							) AS [STK]
			--	--				OUTER APPLY (	SELECT SUM([WSA_ACT_QTY])  AS [WSA_QTY]
			--	--								FROM [INV_WORK_ORDER_ITEM_BOM_STK_ALLOC] 
			--	--								WHERE [WSA_WIB] = [WIB_PK]
			--	--							 )
			--	--										AS [ALLOC]
			--	--				WHERE	[GRD_GR]		= [GRH_PK]
			--	--					--AND [WIB_ITEM_TYPE] IN(1,4)
			--	--					AND [WIB_ITEM_TYPE] = 1
			--	--					AND CAST(ISNULL(([WIB_ACT_QTY]/[WID_QTY])*[GRD_QTY_RECEIVED],0)-ISNULL([WSA_QTY],0) AS NUMERIC(19,6))>0
			--	--				ORDER BY [GRD_SL_NO]
			--	--				FOR XML AUTO,TYPE,ELEMENTS
			--	--			 )
			--	--	FROM	[INV_GRN_HDR] AS [root]
			--	--	WHERE	[GRH_PK]	=  @V_GRH_PK
			--	--	FOR XML AUTO, ELEMENTS)

			--		SELECT  @V_X1	=	REPLACE(@V_X1,'<INV_GRN_DTL>','') ;
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'</INV_GRN_DTL>','') ; 
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'<INV_WORK_ORDER_ITEM_BOM_DTL>','') ;
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'</INV_WORK_ORDER_ITEM_BOM_DTL>','') ; 
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'<STK>','') ;
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'</STK>','') ; 
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'<ALLOC>','') ;
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'</ALLOC>','') ; 
			
			--		SET  @V_XML=CONVERT(XML,@V_X1);			 
			
			--		EXEC	[SPINV_ITEM_EXT_ISS_RCV_WO_SAVE]	
			--				 @P_XML			=	@V_XML
			--				,@P_RET_VAL		=	@PRetVal		OUTPUT
			--				,@P_RET_NO		=	@V_RET_NO		OUTPUT  

			--		IF	(@PRetVal < 0)
			--			BEGIN
			--				SELECT @PRetVal = -100
			--				RETURN
			--			END
			--	END	

			--	--IF EXISTS(	SELECT	1
			--	--			FROM	[INV_GRN_HDR]
			--	--			INNER JOIN [INV_GRN_DTL]					ON [GRH_PK] = [GRD_GR]
			--	--			INNER JOIN [INV_WORK_ORDER_ITEM_DTL]		ON [WID_PK] = [GRD_WO_DTL]
			--	--			INNER JOIN [INV_WORK_ORDER_ITEM_BOM_DTL]	ON [WID_PK] = [WIB_WID]
			--	--			CROSS APPLY (	SELECT SUM([WSA_ACT_QTY])  AS [WSA_QTY]
			--	--							FROM [INV_WORK_ORDER_ITEM_BOM_STK_ALLOC] 
			--	--							WHERE [WSA_WIB] = [WIB_PK]
			--	--						 )
			--	--									AS [ALLOC]
			--	--			WHERE	[GRH_PK]  = @pAppID
			--	--				AND [GRH_IS_WORK_ORDER] = 1
			--	--				--AND [WIB_ITEM_TYPE] IN(1,4)
			--	--				AND [WIB_ITEM_TYPE] = 1
			--	--				AND [WSA_QTY] > 0
			--	--				)
			--	--BEGIN	
		    
			--	--	SET @V_X1=
			--	--	(SELECT	0 AS [ICH_PK]
			--	--			,NULL AS [ICH_NO]
			--	--			,NULL AS [ICH_VERSION]
			--	--			,REPLACE(CONVERT(VARCHAR(11), [GRH_DATE], 106), ' ','-')	AS [ICH_DATE]
			--	--			,NULL AS [ICH_SHIFT]
			--	--			,1 AS [ICH_ITEM_TYPE]
			--	--			,(	SELECT	[CFG_DATA]
			--	--				FROM	[ADM_CONFIG_MST]
			--	--				WHERE	[CFG_TYPE]		=	'ITEM TYPE'	
			--	--					AND [CFG_VALUE]		=	1 )			AS	[ICH_ITEM_TYPE_TEXT]
							
			--	--			,1 AS [ICH_TRX_TYPE]	
			--	--			,GRH_NO AS [ICH_REF_NO]		
			--	--			,7 AS [ICH_ISS_RCV_TYPE]
			--	--			,(	SELECT	[CFG_DATA]
			--	--				FROM	[ADM_CONFIG_MST]
			--	--				WHERE	[CFG_TYPE]		=	'EXTERNAL ISS RCV TYPE'	
			--	--					AND [CFG_VALUE]		=	7 )		AS	[ICH_ISS_RCV_TYPE_TEXT]
			--	--			,(	SELECT	[CFG_PK]
			--	--				FROM	[ADM_CONFIG_MST]
			--	--				WHERE	[CFG_TYPE]		=	'EXTERNAL ISS RCV TYPE'	
			--	--					AND [CFG_VALUE]		=	7 )		AS	[ICH_ISS_RCV_TYPE_PK]

			--	--			,[GRH_VENDOR] AS [ICH_ISS_RCV_PK]
			--	--			,(SELECT [VEN_NAME] FROM PUR_VENDOR_MST WHERE VEN_PK=[GRH_VENDOR]) AS [ICH_ISS_RCV_NAME]
			--	--			,'Work Order' AS [ICH_REMARKS]
			--	--			,1 AS [ICH_STATUS]
			--	--			,[GRH_COMPANY] AS [ICH_COMPANY]
			--	--			,CASE WHEN [GRH_COMPANY] = 1 THEN 270 ELSE 20270 END		AS  [ICH_DEPT]
			--	--			,(	SELECT	[DPT_NAME]
			--	--				FROM	[ADM_DEPT_MST]
			--	--				WHERE	[DPT_PK] =(CASE WHEN [GRH_COMPANY] = 1 THEN 270 ELSE 20270 END)	)						
			--	--																AS	[ICH_DEPT_TEXT]
			--	--			,1 AS [ICH_ACTIV
E]
			--	--			,[GRH_BIZUNIT] AS [ICH_BIZUNIT]
			--	--			,[GRH_BIZUNIT] AS [BizUnitPk]
			--	--			,[GRH_CRTD_BY] AS [ICH_CRTD_BY]
			--	--			,[GRH_CRTD_BY] AS [UserPk]
			--	--			,[GRH_CRTD_DT] AS [ICH_CRTD_DT]
			--	--			,NULL 												AS	[LAST_MOD_DT]
			--	--			,1 AS [WKF_TRX_FLAG]
			--	--			,'EMI' AS [APT_CODE]
			--	--			,1		AS [AST_DOC_MODE]
			--	--			,NULL AS [ICH_CRDR_NOTE_HDR]
			--	--			,(	SELECT	 0											AS [ICD_PK]
			--	--						,[GRD_SL_NO]								AS [ICD_SL_NO]
			--	--						,ROW_NUMBER() OVER(ORDER BY [GRD_SL_NO])	AS [ROW_NO]
			--	--						,[WIB_MATERIAL]								AS [ICD_ITEM]
			--	--						,(	SELECT	'('+[ITM_CODE] + ') '+ [ITM_NAME]
			--	--							FROM	[INV_ITEM_MST]
			--	--							WHERE	[ITM_PK] =	[WIB_MATERIAL]	)	AS	[ICD_ITEM_TEXT]
			--	--						,(  SELECT  ITM_NEED_BATCH_STK  
			--	--	                        FROM	[INV_ITEM_MST]  
			--	--	                        WHERE	[ITM_PK] = [WIB_MATERIAL] )		AS [ITM_NEED_BATCH_STK]  
			--	--						,(	SELECT	[ITM_CATEGORY]
			--	--							FROM	[INV_ITEM_MST]
			--	--							WHERE	[ITM_PK] =	[WIB_MATERIAL]	)	AS	[ICD_ITEM_CATEGORY]
			--	--						,(	SELECT	[ITC_NAME]
			--	--							FROM	[INV_ITEM_CATEGORY]
			--	--							WHERE	[ITC_PK] =	(	SELECT	[ITM_CATEGORY]
			--	--													FROM	[INV_ITEM_MST]
			--	--													WHERE	[ITM_PK] =	[WIB_MATERIAL]		
			--	--												 )	)				AS	[ICD_ITEM_CATEGORY_TEXT]
			--	--						,[SBD_PK]									AS	[ICD_STK_BATCH]
			--	--						,[SBD_BATCH_LOT_TEXT]						AS	[ICD_STK_BATCH_NO]
			--	--						,ISNULL([WSA_QTY],0)						AS	[ICD_QTY_CONSUMED]
			--	--						,[WIB_UOM]									AS  [ICD_UOM]
			--	--						,(	SELECT	[UOM_CODE]
			--	--							FROM	[INV_UOM_MST]
			--	--							WHERE	[UOM_PK] =	[WIB_UOM]	)		AS	[ICD_UOM_TEXT]
			--	--						,0											AS	[ICD_ITEM_TYPE]
			--	--						,'No'										AS	[ICD_ISRETURNTEXT]
			--	--						,'Work Order'								AS	[ICD_REMARKS]
			--	--						,0											AS	[ICD_CURRENT_STK]
			--	--						,0											AS	[ICD_VALUE_CONSUMED]
			--	--						,0											AS  [ICD_RATE]
			--	--						,NULL										AS  [ICD_EXPIRY_DATE]
			--	--					 	,NULL										AS	[ICD_LOT_NO]	 
			--	--					 	,NULL										AS	[ICD_CRDR_NOTE_DTL]
			--	--						,NULL										AS	[ICD_ISS_RCV_TYPE]    
			--	--						,NULL										AS	[ICD_ISS_RCV_PK]
			--	--						,NULL										AS	[ICD_ISS_RCV_SUB_TYPE] 	
			--	--						,''											AS	[ICD_ISS_RCV_TYPE_TEXT]		
			--	--						,NULL										AS	[ICD_ISS_RCV_NAME]
			--	--						,''											AS	[ICD_ISS_RCV_SUB_TYPE_TEXT]
			--	--						,[GRD_PK]									AS	[ICD_GRN_DTL]	
			--	--						,[WIB_PK]									AS	[ICD_WO_DTL]	
			--	--				FROM	[INV_GRN_DTL] 
			--	--				INNER JOIN [INV_WORK_ORDER_ITEM_DTL]							ON [WID_PK] = [GRD_WO_DTL]
			--	--				INNER JOIN [INV_WORK_ORDER_ITEM_BOM_DTL] AS [ConsumptionDtl]	ON [WID_PK] = [WIB_WID]
			--	--				INNER JOIN [INV_WORK_ORDER_ITEM_HDR]							ON [WIH_PK] = [WID_WIH]
			--	--				CROSS APPLY (	SELECT  [SBD_PK]
			--	--									   ,[SBD_BATCH_NO]
			--	--									   ,[SBD_BATCH_LOT_TEXT]
			--	--									   ,CAST([SBD_QTY_IN_STOCK] AS NUMERIC(19,6))	AS [SBD_QTY_IN_STOCK]						
			--	--									   ,CAST([WSA_ACT_QTY] AS NUMERIC(19,6))		AS [WSA_QTY]
			--	--								FROM [INV_WORK_ORDER_ITEM_BOM_STK_ALLOC] 
			--	--								INNER JOIN [INV_STK_BATCH_DTL]		ON [SBD_PK] = [WSA_SBD_PK]
			--	--								WHERE [WSA_WIB] = [WIB_PK]
			--	--								AND [SBD_QTY_IN_STOCK] > 0
			--	--							 )
			--	--										AS [ALLOC]
			--	--				WHERE	[GRD_GR]		= [GRH_PK]
			--	--					--AND [WIB_ITEM_TYPE] IN(1,4)
			--	--					AND [WIB_ITEM_TYPE] = 1
			--	--					AND ISNULL([WSA_QTY],0)>0
			--	--				ORDER BY [GRD_SL_NO]
			--	--				FOR XML AUTO,TYPE,ELEMENTS
			--	--			 )
			--	--	FROM	[INV_GRN_HDR] AS [root]
			--	--	WHERE	[GRH_PK]	=  @V_GRH_PK
			--	--	FOR XML AUTO, ELEMENTS)

			--	--	SELECT  @V_X1	=	REPLACE(@V_X1,'<INV_GRN_DTL>','') ;
			--	--	SELECT  @V_X1	=	REPLACE(@V_X1,'</INV_GR
N_DTL>','') ; 
			--	--	SELECT  @V_X1	=	REPLACE(@V_X1,'<INV_WORK_ORDER_ITEM_BOM_DTL>','') ;
			--	--	SELECT  @V_X1	=	REPLACE(@V_X1,'</INV_WORK_ORDER_ITEM_BOM_DTL>','') ; 
			--	--	SELECT  @V_X1	=	REPLACE(@V_X1,'<STK>','') ;
			--	--	SELECT  @V_X1	=	REPLACE(@V_X1,'</STK>','') ; 
			--	--	SELECT  @V_X1	=	REPLACE(@V_X1,'<ALLOC>','') ;
			--	--	SELECT  @V_X1	=	REPLACE(@V_X1,'</ALLOC>','') ; 
			
			--	--	SET  @V_XML=CONVERT(XML,@V_X1);

			--	--	EXEC	[SPINV_ITEM_EXT_ISS_RCV_SAVE]	
			--	--			 @P_XML			=	@V_XML
			--	--			,@P_RET_VAL		=	@PRetVal		OUTPUT
			--	--			,@P_RET_NO		=	@V_RET_NO		OUTPUT  

			--	--	IF	(@PRetVal < 0)
			--	--		BEGIN
			--	--			SELECT @PRetVal = -100
			--	--			RETURN
			--	--		END
			--	--END	

			--	IF EXISTS(	SELECT	1
			--				FROM	[INV_GRN_HDR]
			--				INNER JOIN [INV_GRN_MAT_RET_DTL]			ON [GRH_PK] = [GMD_GR]
			--				INNER JOIN [INV_STK_BATCH_DTL]				ON [SBD_PK] = [GMD_BATCH]
			--				WHERE	[GRH_PK]  = @pAppID
			--					AND [GRH_IS_WORK_ORDER] = 1
			--					AND [SBD_QTY_IN_STOCK] > 0
			--					)
			--	BEGIN	
		    
			--		SET @V_X1=
			--		(SELECT	0 AS [ICH_PK]
			--				,NULL AS [ICH_NO]
			--				,NULL AS [ICH_VERSION]
			--				,REPLACE(CONVERT(VARCHAR(11), [GRH_DATE], 106), ' ','-')	AS [ICH_DATE]
			--				,NULL AS [ICH_SHIFT]
			--				,1 AS [ICH_ITEM_TYPE]
			--				,(	SELECT	[CFG_DATA]
			--					FROM	[ADM_CONFIG_MST]
			--					WHERE	[CFG_TYPE]		=	'ITEM TYPE'	
			--						AND [CFG_VALUE]		=	1 )			AS	[ICH_ITEM_TYPE_TEXT]
							
			--				,3			AS [ICH_TRX_TYPE]	
			--				,GRH_NO		AS [ICH_REF_NO]		
			--				,6			AS [ICH_ISS_RCV_TYPE]
			--				,(	SELECT	[CFG_DATA]
			--					FROM	[ADM_CONFIG_MST]
			--					WHERE	[CFG_TYPE]		=	'EXTERNAL ISS RCV TYPE'	
			--						AND [CFG_VALUE]		=	6 )		AS	[ICH_ISS_RCV_TYPE_TEXT]
			--				,(	SELECT	[CFG_PK]
			--					FROM	[ADM_CONFIG_MST]
			--					WHERE	[CFG_TYPE]		=	'EXTERNAL ISS RCV TYPE'	
			--						AND [CFG_VALUE]		=	6 )		AS	[ICH_ISS_RCV_TYPE_PK]
			--				,CASE WHEN [GRH_COMPANY] = 1 THEN 2 ELSE 20002 END			AS [ICH_ISS_RCV_PK]
			--				,(SELECT [DPT_NAME] FROM [ADM_DEPT_MST] WHERE DPT_PK=(CASE WHEN [GRH_COMPANY] = 1 THEN 2 ELSE 20002 END))	
			--																			AS [ICH_ISS_RCV_NAME]
			--				,'Work Order' AS [ICH_REMARKS]
			--				,1 AS [ICH_STATUS]
			--				,[GRH_COMPANY] AS [ICH_COMPANY]
			--				,CASE WHEN [GRH_COMPANY] = 1 THEN 270 ELSE 20270 END		AS  [ICH_DEPT]
			--				,(	SELECT	[DPT_NAME]
			--					FROM	[ADM_DEPT_MST]
			--					WHERE	[DPT_PK] =	(CASE WHEN [GRH_COMPANY] = 1 THEN 270 ELSE 20270 END)	)						
			--																			AS	[ICH_DEPT_TEXT]
			--				,1 AS [ICH_ACTIVE]
			--				,[GRH_BIZUNIT] AS [ICH_BIZUNIT]
			--				,[GRH_BIZUNIT] AS [BizUnitPk]
			--				,[GRH_CRTD_BY] AS [ICH_CRTD_BY]
			--				,[GRH_CRTD_BY] AS [UserPk]
			--				,[GRH_CRTD_DT] AS [ICH_CRTD_DT]
			--				,NULL 												AS	[LAST_MOD_DT]
			--				,1		AS [WKF_TRX_FLAG]
			--				,'EMI'  AS [APT_CODE]
			--				,1		AS [AST_DOC_MODE]
			--				,NULL	AS [ICH_CRDR_NOTE_HDR]
			--				,(	SELECT	 0											AS [ICD_PK]
			--							,[GMD_SL_NO]								AS [ICD_SL_NO]
			--							,ROW_NUMBER() OVER(ORDER BY [GMD_SL_NO])	AS [ROW_NO]
			--							,[GMD_MATERIAL]								AS [ICD_ITEM]
			--							,(	SELECT	'('+[ITM_CODE] + ') '+ [ITM_NAME]
			--								FROM	[INV_ITEM_MST]
			--								WHERE	[ITM_PK] =	[GMD_MATERIAL]	)	AS	[ICD_ITEM_TEXT]
			--							,(  SELECT  ITM_NEED_BATCH_STK  
			--		                        FROM	[INV_ITEM_MST]  
			--		                        WHERE	[ITM_PK] = [GMD_MATERIAL] )		AS [ITM_NEED_BATCH_STK]  
			--							,(	SELECT	[ITM_CATEGORY]
			--								FROM	[INV_ITEM_MST]
			--								WHERE	[ITM_PK] =	[GMD_MATERIAL]	)	AS	[ICD_ITEM_CATEGORY]
			--							,(	SELECT	[ITC_NAME]
			--								FROM	[INV_ITEM_CATEGORY]
			--								WHERE	[ITC_PK] =	(	SELECT	[ITM_CATEGORY]
			--														FROM	[INV_ITEM_MST]
			--														WHERE	[ITM_PK] =	[GMD_MATERIAL]		
			--													 )	)				AS	[ICD_ITEM_
CATEGORY_TEXT]
			--							,[GMD_BATCH]								AS	[ICD_STK_BATCH]
			--							,[GMD_BATCH_NO]								AS	[ICD_STK_BATCH_NO]
			--							,[GMD_QTY]									AS	[ICD_QTY_CONSUMED]
			--							,[GMD_UOM]									AS  [ICD_UOM]
			--							,(	SELECT	[UOM_CODE]
			--								FROM	[INV_UOM_MST]
			--								WHERE	[UOM_PK] =	[GMD_UOM]	)		AS	[ICD_UOM_TEXT]
			--							,0											AS	[ICD_ITEM_TYPE]
			--							,'No'										AS	[ICD_ISRETURNTEXT]
			--							,'Work Order'								AS	[ICD_REMARKS]
			--							,0											AS	[ICD_CURRENT_STK]
			--							,0											AS	[ICD_VALUE_CONSUMED]
			--							,0											AS  [ICD_RATE]
			--							,NULL										AS  [ICD_EXPIRY_DATE]
			--						 	,NULL										AS	[ICD_LOT_NO]	 
			--						 	,NULL										AS	[ICD_CRDR_NOTE_DTL]
			--							,NULL										AS	[ICD_ISS_RCV_TYPE]    
			--							,NULL										AS	[ICD_ISS_RCV_PK]
			--							,NULL										AS	[ICD_ISS_RCV_SUB_TYPE] 	
			--							,''											AS	[ICD_ISS_RCV_TYPE_TEXT]		
			--							,NULL										AS	[ICD_ISS_RCV_NAME]
			--							,''											AS	[ICD_ISS_RCV_SUB_TYPE_TEXT]
			--							,[GRD_PK]									AS	[ICD_GRN_DTL]	
			--							,NULL										AS	[ICD_WO_DTL]
			--							,[GMD_PK]									AS	[ICD_GM_DTL]	
			--					FROM	[INV_GRN_DTL] 
			--					INNER JOIN [INV_GRN_MAT_RET_DTL]	AS [ConsumptionDtl]	ON [GRD_GR] = [GMD_GR]
			--					INNER JOIN [INV_STK_BATCH_DTL]							ON [SBD_PK] = [GMD_BATCH]
			--					WHERE	[GRD_GR]		= [GRH_PK]
			--						AND CAST([SBD_QTY_IN_STOCK] AS NUMERIC(19,6))>0
			--					ORDER BY [GMD_SL_NO]
			--					FOR XML AUTO,TYPE,ELEMENTS
			--				 )
			--		FROM	[INV_GRN_HDR] AS [root]
			--		WHERE	[GRH_PK]	=  @V_GRH_PK
			--		FOR XML AUTO, ELEMENTS)

			--		SELECT  @V_X1	=	REPLACE(@V_X1,'<INV_GRN_DTL>','') ;
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'</INV_GRN_DTL>','') ; 
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'<INV_STK_BATCH_DTL>','') ;
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'</INV_STK_BATCH_DTL>','') ; 
			
			--		SET  @V_XML=CONVERT(XML,@V_X1);

			--		EXEC	[SPINV_ITEM_EXT_ISS_RCV_WO_SAVE]	
			--				 @P_XML			=	@V_XML
			--				,@P_RET_VAL		=	@PRetVal		OUTPUT
			--				,@P_RET_NO		=	@V_RET_NO		OUTPUT  

			--		IF	(@PRetVal < 0)
			--			BEGIN
			--				SELECT @PRetVal = -100
			--				RETURN
			--			END
			--	END
		
			--	--IF EXISTS(	SELECT	1
			--	--			FROM	[INV_GRN_HDR]
			--	--			INNER JOIN [INV_GRN_MAT_RET_DTL]			ON [GRH_PK] = [GMD_GR]
			--	--			INNER JOIN [PRD_BIN_CARD_HDR]				ON [BCH_PK] = [GMD_BIN_CARD]
			--	--			WHERE	[GRH_PK]  = @pAppID
			--	--				AND [GRH_IS_WORK_ORDER] = 1
			--	--				AND [BCH_TOTAL_PCS_LAST] > 0
			--	--				)
			--	--BEGIN	

			--	--	INSERT INTO @T_INV_GRN_MAT_RET_DTL
			--	--	SELECT	[BID_PK]
			--	--	FROM	[INV_GRN_HDR]
			--	--	INNER JOIN [INV_GRN_MAT_RET_DTL]			ON [GRH_PK] = [GMD_GR]
			--	--	INNER JOIN [PRD_BIN_CARD_ISSUE_DTL]			ON  ( [BID_BIN_CARD] = [GMD_BIN_CARD]
			--	--													AND   [BID_PRODUCT] = [GMD_MATERIAL])
			--	--	INNER JOIN [PRD_BIN_CARD_ISSUE_HDR]				ON [BIH_PK]			= [BID_ISSUE_HDR]
			--	--	INNER JOIN [PRD_BIN_CARD_HDR]					ON [BCH_PK]			= [BID_BIN_CARD]
			--	--	WHERE	[GRH_PK]  = @pAppID
			--	--		AND [GRH_IS_WORK_ORDER] = 1
			--	--		AND [BCH_TOTAL_PCS_LAST] > 0
			
			--	--	SELECT @V_MAX_ROW = MAX([ROW_NO]) FROM @T_INV_GRN_MAT_RET_DTL
			--	--	SET @V_ROW = 1
			--	--	WHILE (@V_ROW<=@V_MAX_ROW)
			--	--	BEGIN
			--	--		SELECT @V_BID_PK = [BID_PK] 
			--	--		FROM @T_INV_GRN_MAT_RET_DTL
			--	--		WHERE [ROW_NO] = @V_ROW

			--	--		EXEC [SPINV_BIN_CARD_STK_M2_SAVE]   @P_MODULE	= 'BIR',
			--	--											@P_TRX_PK	= @V_BID_PK,
			--	--											@P_TRX_MODE = 44,				 
			--	--											@P_RET_VAL	= @PRetVal OUTPUT  

			--	--			IF	(@PRetVal < 0)
			--	--			BEGIN
			--	--				SELECT @PRetVal = -100
			--	--				RETURN
			--	--			END
			--	--		SET @V_ROW = @V_ROW+1
			--	--	END
			--	--END

			--	IF EXISTS(	SELECT	1
			--				FROM	[INV_GRN_HDR]
			--				INNER JOIN [INV_G
RN_DTL]					ON [GRH_PK] = [GRD_GR]
			--				INNER JOIN [INV_WORK_ORDER_ITEM_DTL]		ON [WID_PK] = [GRD_WO_DTL]
			--				INNER JOIN [INV_WORK_ORDER_ITEM_BOM_DTL]	ON [WID_PK] = [WIB_WID]
			--				WHERE	[GRH_PK]  = @pAppID
			--					AND [GRH_IS_WORK_ORDER] = 1
			--					AND [WIB_ITEM_TYPE] = 3
			--					AND [WIB_OPERTAION] = 5)

			--	BEGIN
			--		UPDATE [PRD_BIN_CARD_PACK_ISSUE_HDR]
			--		SET	   [HBI_IS_STERLIZED] = 1
			--		FROM [INV_GRN_DTL]
			--		INNER JOIN [INV_WORK_ORDER_ITEM_HDR] ON [WIH_PK] = [GRD_WO]
			--		INNER JOIN [PRD_BIN_CARD_PACK_ISSUE_HDR] ON [WIH_PK] = [HBI_WO]
			--		WHERE [GRD_GR]  = @pAppID
			--	END
			--	IF EXISTS(	SELECT	1
			--				FROM	[INV_GRN_HDR]
			--				INNER JOIN [INV_GRN_DTL]					ON [GRH_PK] = [GRD_GR]
			--				INNER JOIN [INV_WORK_ORDER_ITEM_DTL]		ON [WID_PK] = [GRD_WO_DTL]
			--				INNER JOIN [INV_WORK_ORDER_ITEM_BOM_DTL]	ON [WID_PK] = [WIB_WID]
			--				WHERE	[GRH_PK]  = @pAppID
			--					AND [GRH_IS_WORK_ORDER] = 1
			--					AND [WIB_ITEM_TYPE] = 3
			--					AND [WIB_OPERTAION] <> 5)

			--	BEGIN
			--		SET @V_X1=
			--		(SELECT	 0				AS	WKF_REFERENCE
			--				,0				AS	WKF_APPLICATION
			--				,CASE WHEN [GRH_COMPANY] = 1 THEN 908	 ELSE 20908	 END		AS	WKF_PROCESS
			--				,CASE WHEN [GRH_COMPANY] = 1 THEN 2682	 ELSE 22682	 END		AS	WKF_TASK
			--				,CASE WHEN [GRH_COMPANY] = 1 THEN 5660	 ELSE 25660	 END		AS	WKF_TASK_ACTION
			--				,''				AS	WKF_COMMENTS
			--				,1				AS	WKF_TRX_FLAG
			--				,[GRH_MOD_BY]	AS	USER_PK
			--				,[GRH_BIZUNIT]	AS	BizUnitPk
			--				,[GRH_MOD_BY]   AS	UserPk
			--				,[GRH_COMPANY]  AS MIH_COMPANY
			--				,0				AS MIH_IS_EDIT
			--				,1				AS MIH_IS_WORK_ORDER
			--				,'MI'			AS APT_CODE
			--				,1				AS	WKF_FLAG
			--				,1				AS	AST_DOC_MODE
			--				,[GRH_DATE]     AS	MIHDATE
			--				,0				AS	MIH_PK
			--				,0				AS	MIH_STATUS
			--				,CASE WHEN [GRH_COMPANY] = 1 THEN 272 ELSE 20272 END			AS	MIH_DEPT
			--				,CASE WHEN [GRH_COMPANY] = 1 THEN 30 ELSE 20030 END				AS	MIH_DEPT_TO
			--				,0				AS	MID_BATCH
			--				,1				AS	POD_CONV_FACT
			--				,0				AS	MID_BATCH
			--				,(	SELECT	 [HBI_WO]		AS	[MID_PO]
			--							,[DBI_PRODUCT]	AS	[MID_ITEM]
			--							,1				AS	[MID_SL_NO]
			--							,0				AS	[MID_MULT_BTCH_GRP]
			--							,0				AS	MID_IS_MULTIPLE_BATCH
			--							,[GRD_WO_DTL]	AS	MID_MR_DTL
			--							,0				AS	MID_PK
			--							,[WIH_NO]		AS	MRH_NO
			--							,''				AS	ITM_NAME
			--							,[SBD_UOM]		AS	MID_UOM
			--							,[GRD_QTY_RECEIVED]	AS MRD_QTY_APPROVED
			--							,[GRD_QTY_RECEIVED]	AS MRD_QTY_ISSUED
			--							,[GRD_QTY_RECEIVED] AS MID_QTY_ISSUED
			--							,[GRD_QTY_RECEIVED]	AS	ORG_BALANCE_QTY
			--							,'' AS UOM_CODE
			--							,'GRN' AS MID_REMARKS
			--							,[HBI_WO] AS MRH_PK
			--							,[SBD_PK] AS MID_STK_BATCH
			--							,[SBD_QTY_IN_STOCK] AS QTY_IN_STOCK
			--							,[SBD_QTY_IN_STOCK] AS ITM_CUR_STK
			--					FROM	[INV_GRN_DTL] 
			--					INNER JOIN [PRD_BIN_CARD_PACK_ISSUE_HDR]				ON [GRD_WO] = [HBI_WO]
			--					INNER JOIN [INV_WORK_ORDER_ITEM_HDR]					ON [WIH_PK] = [HBI_WO]
			--					INNER JOIN [PRD_BIN_CARD_PACK_ISSUE_DTL] AS [MIList]	ON [HBI_PK] = [DBI_ISSUE_HDR]
			--					CROSS APPLY (	SELECT   [SBD_PK]
			--											,[SBD_BATCH_NO]
			--											,[SBD_BATCH_LOT_TEXT]
			--											,[SBD_QTY_IN_STOCK]
			--											,[SBD_UOM]
			--									FROM  [INV_STK_BATCH_DTL]
			--									WHERE [SBD_DEPT] = [WIH_DEPT_TO]
			--									 AND  [SBD_ITEM] = [DBI_PRODUCT]
			--									 AND  [SBD_QTY_IN_STOCK]>0
			--								) AS [STK]
			--					WHERE	[GRD_GR]		= [GRH_PK]
			--					ORDER BY [GRD_SL_NO]
			--					FOR XML AUTO,TYPE,ELEMENTS
			--				 )
			--		FROM	[INV_GRN_HDR] AS [root]
			--		WHERE	[GRH_PK]	=  @V_GRH_PK
			--		FOR XML AUTO, ELEMENTS)

			--		SELECT  @V_X1	=	REPLACE(@V_X1,'<INV_WORK_ORDER_ITEM_BOM_DTL>','') ;
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'</INV_WORK_ORDER_ITEM_BOM_DTL>','') ; 
			--		SELECT
  @V_X1	=	REPLACE(@V_X1,'<INV_GRN_DTL>','') ;
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'</INV_GRN_DTL>','') ;
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'<STK>','') ;
			--		SELECT  @V_X1	=	REPLACE(@V_X1,'</STK>','') ; 
			
			--		SET  @V_XML=CONVERT(XML,@V_X1); 

			--		EXEC	[SPINV_ITEM_ISSUE_WKF_SAVE]	
			--				 @P_XML			=	@V_XML
			--				,@P_RET_VAL		=	@PRetVal		OUTPUT
			--				,@P_RET_NO		=	@V_RET_NO		OUTPUT
			--				,@P_RET_REF_PK  =	@V_RET_REF_PK	OUTPUT

			--		IF	(@PRetVal < 0)
			--			BEGIN
			--				SELECT @PRetVal = -100
			--				RETURN
			--			END
			--	END

			--	--Material return Updation
			--	INSERT INTO @T_BIN_PRODUCT	
			--	(
			--			 [ROW_NO]
			--			,[RW]				
			--			,[BCH_BAL_PCS]	
			--			,[BVD_TOTAL_PCS]
			--			,[TOT_PCS]
			--			,[WEI_PK]
			--			,[GMD_PK]
			--	)
			--	SELECT	 ROW_NUMBER() OVER (PARTITION BY [WEI_SBD_PK] ORDER BY [WEI_PK])
			--			,DENSE_RANK() OVER(ORDER BY [WEI_SBD_PK])
			--			,([WEI_QTY] - [WEI_USED_QTY])		AS	[BCH_BAL_PCS]
			--			,[GMD_QTY]							AS	[BVD_TOTAL_PCS]	
			--			,0
			--			,[WEI_PK]							AS	[WEI_PK]
			--			,[GMD_PK]							AS  [GMD_PK]
			--			FROM [INV_GRN_MAT_RET_DTL]
			--			INNER JOIN [INV_WORK_ORDER_ITEM_HDR] ON [WIH_PK] = [GMD_WO]
			--			INNER JOIN [INV_WORK_ORDER_ITEM_BOM_EXCESS_ISS_DTL] ON [WEI_SBD_PK] = [GMD_BATCH]
			--			WHERE [WEI_VENDOR] = [WIH_VENDOR]
			--				AND ([WEI_QTY] - [WEI_USED_QTY]) > 0
			--				AND [GMD_GR] = @pAppID
			--			ORDER BY [WEI_PK]

			--	SET @x = 1
			--	SELECT @y=MAX([RW]) FROM @T_BIN_PRODUCT 
			--	WHILE (@x<=@y )
			--	BEGIN
			--		SET @i = 1
			--		SELECT @DR_VAL=MAX([BVD_TOTAL_PCS]) FROM @T_BIN_PRODUCT WHERE [RW] = @x 
			--		SELECT @j=MAX([ROW_NO]) FROM @T_BIN_PRODUCT WHERE [RW] = @x 
			--		WHILE (@i<=@j )
			--		BEGIN
			--			SELECT @BAL_QTY=[BCH_BAL_PCS]-@DR_VAL FROM @T_BIN_PRODUCT WHERE [RW] = @x  AND [ROW_NO]=@i
			--			IF (@BAL_QTY>=0)
			--			BEGIN
			--				SELECT @QTY=@DR_VAL
			--			END
			--			ELSE 
			--			BEGIN
			--				SELECT @QTY=[BCH_BAL_PCS] FROM @T_BIN_PRODUCT WHERE [RW] = @x  AND [ROW_NO]=@i
			--			END
			--			UPDATE @T_BIN_PRODUCT SET [TOT_PCS]=@QTY WHERE [RW] = @x  AND [ROW_NO]=@i
			--			SET @DR_VAL = @DR_VAL-@QTY
			--			SET @i=@i+1
			--		END
			--		SET @x=@x+1
			--	END

			--	UPDATE A
			--	SET [WEI_USED_QTY] = [WEI_USED_QTY] + ISNULL([TOT_PCS],0)  
			--	FROM	[INV_WORK_ORDER_ITEM_BOM_EXCESS_ISS_DTL] A 
			--	CROSS APPLY (	SELECT SUM([TOT_PCS]) AS [TOT_PCS]
			--					FROM	@T_BIN_PRODUCT B
			--					WHERE	B.[WEI_PK] = A.[WEI_PK]
			--				) AS [ISS]

			--	INSERT INTO [INV_WORK_ORDER_ITEM_BOM_WO_RET_DTL]
			--	(
			--			 [WWR_WIB]
			--			,[WWR_MATERIAL]
			--			,[WWR_SBD_PK]
			--			,[WWR_QTY]
			--			,[WWR_BIN_CARD]
			--			,[WWR_TEXT]
			--			,[WWR_ISS_HDR]
			--			,[WWR_MATERIAL_TYPE]
			--			,[WWR_WIE_PK]
			--			,[WWR_GMD_PK]
			--	)
			--	SELECT   [WEI_WIB]			AS [WWR_WIB]
			--			,[WEI_MATERIAL]		AS [WWR_MATERIAL]
			--			,[WEI_SBD_PK]		AS [WWR_SBD_PK]
			--			,[TOT_PCS]			AS [WWR_QTY]
			--			,NULL				AS [WWR_BIN_CARD]
			--			,[WEI_TEXT]			AS [WWR_TEXT]
			--			,[WEI_ISS_HDR]		AS [WWR_ISS_HDR]
			--			,[WEI_MATERIAL_TYPE]AS [WWR_MATERIAL_TYPE]
			--			,A.[WEI_PK]			AS [WWR_WIE_PK] 
			--			,[GMD_PK]			AS [WWR_GMD_PK]
			--	FROM	@T_BIN_PRODUCT A
			--	INNER JOIN [INV_WORK_ORDER_ITEM_BOM_EXCESS_ISS_DTL] B ON A.[WEI_PK] = B.[WEI_PK]
			--	WHERE [TOT_PCS] > 0 

			--	DELETE FROM @T_BIN_PRODUCT

			--	INSERT INTO @T_BIN_PRODUCT	
			--	(
			--			 [ROW_NO]
			--			,[RW]				
			--			,[BCH_BAL_PCS]	
			--			,[BVD_TOTAL_PCS]
			--			,[TOT_PCS]
			--			,[WEI_PK]
			--			,[GMD_PK]
			--	)
			--	SELECT	 ROW_NUMBER() OVER (PARTITION BY [WEI_BIN_CARD] ORDER BY [WEI_PK])
			--			,DENSE_RANK() OVER(ORDER BY [WEI_BIN_CARD])
			--			,([WEI_QTY] - [WEI_USED_QTY])		AS	[BCH_BAL_PCS]
			--			,[GMD_QTY]							AS	[BVD_TOTAL_PCS]	
			--			,0
			--			,[WEI_PK]							AS	[WEI_PK]
			--			,[GMD_PK]							AS  [G
MD_PK]
			--			FROM [INV_GRN_MAT_RET_DTL]
			--			INNER JOIN [INV_WORK_ORDER_ITEM_HDR] ON [WIH_PK] = [GMD_WO]
			--			INNER JOIN [INV_WORK_ORDER_ITEM_BOM_EXCESS_ISS_DTL] ON [WEI_BIN_CARD]	= [GMD_BIN_CARD]
			--			WHERE [WEI_VENDOR] = [WIH_VENDOR]
			--				AND ([WEI_QTY] - [WEI_USED_QTY]) > 0
			--				AND [WIH_PK] = @pAppID
			--			ORDER BY [WEI_PK]

			--	SET @x = 1
			--	SELECT @y=MAX([RW]) FROM @T_BIN_PRODUCT 
			--	WHILE (@x<=@y )
			--	BEGIN
			--		SET @i = 1
			--		SELECT @DR_VAL=MAX([BVD_TOTAL_PCS]) FROM @T_BIN_PRODUCT WHERE [RW] = @x 
			--		SELECT @j=MAX([ROW_NO]) FROM @T_BIN_PRODUCT WHERE [RW] = @x 
			--		WHILE (@i<=@j )
			--		BEGIN
			--			SELECT @BAL_QTY=[BCH_BAL_PCS]-@DR_VAL FROM @T_BIN_PRODUCT WHERE [RW] = @x  AND [ROW_NO]=@i
			--			IF (@BAL_QTY>=0)
			--			BEGIN
			--				SELECT @QTY=@DR_VAL
			--			END
			--			ELSE 
			--			BEGIN
			--				SELECT @QTY=[BCH_BAL_PCS] FROM @T_BIN_PRODUCT WHERE [RW] = @x  AND [ROW_NO]=@i
			--			END
			--			UPDATE @T_BIN_PRODUCT SET [TOT_PCS]=@QTY WHERE [RW] = @x  AND [ROW_NO]=@i
			--			SET @DR_VAL = @DR_VAL-@QTY
			--			SET @i=@i+1
			--		END
			--		SET @x=@x+1
			--	END

			--	UPDATE A
			--	SET [WEI_USED_QTY] = [WEI_USED_QTY] + ISNULL([TOT_PCS],0) 
			--	FROM	[INV_WORK_ORDER_ITEM_BOM_EXCESS_ISS_DTL] A 
			--	CROSS APPLY (	SELECT SUM([TOT_PCS]) AS [TOT_PCS]
			--					FROM	@T_BIN_PRODUCT B
			--					WHERE	B.[WEI_PK] = A.[WEI_PK]
			--				) AS [ISS]

			--	INSERT INTO [INV_WORK_ORDER_ITEM_BOM_WO_RET_DTL]
			--	(
			--			 [WWR_WIB]
			--			,[WWR_MATERIAL]
			--			,[WWR_SBD_PK]
			--			,[WWR_QTY]
			--			,[WWR_BIN_CARD]
			--			,[WWR_TEXT]
			--			,[WWR_ISS_HDR]
			--			,[WWR_MATERIAL_TYPE]
			--			,[WWR_WIE_PK]
			--			,[WWR_GMD_PK]
			--	)
			--	SELECT   [WEI_WIB]			AS [WWR_WIB]
			--			,[WEI_MATERIAL]		AS [WWR_MATERIAL]
			--			,NULL				AS [WWR_SBD_PK]
			--			,[TOT_PCS]			AS [WWR_QTY]
			--			,[WEI_BIN_CARD]		AS [WWR_BIN_CARD]
			--			,[WEI_TEXT]			AS [WWR_TEXT]
			--			,[WEI_ISS_HDR]		AS [WWR_ISS_HDR]
			--			,[WEI_MATERIAL_TYPE]AS [WWR_MATERIAL_TYPE]
			--			,A.[WEI_PK]			AS [WWR_WIE_PK] 
			--			,[GMD_PK]			AS [WWR_GMD_PK]
			--	FROM	@T_BIN_PRODUCT A
			--	INNER JOIN [INV_WORK_ORDER_ITEM_BOM_EXCESS_ISS_DTL] B ON A.[WEI_PK] = B.[WEI_PK]
			--	WHERE [TOT_PCS] > 0 ;

			--	-- Bincard Products Splitup 		

			--	INSERT INTO	@T_INV_GRN_MAT_RET_DTL
			--		(		
			--				 [T_GMD_PK]
			--			 	,[T_GMD_QTY]
			--				,[T_GMD_MATERIAL]
			--				,[T_GMD_BIN_CARD]
			--				,[T_GMD_DEPT_TO]
			--				,[T_GMD_AVG_GLOVE_WT]
			--				,[T_GMR_PK]
			--		)
			--	SELECT   [GMD_PK]
			--			,[GMD_QTY]
			--			,[GMD_MATERIAL]
			--			,[GMD_BIN_CARD]
			--			,[WIH_DEPT_TO]
			--			,[BCH_AVG_GLOVE_WT]
			--			,[GMR_PK]
			--	FROM   [INV_GRN_MAT_RET_DTL]
			--	INNER JOIN [INV_GRN_MAT_RET]		 ON [GMR_PK] = [GMD_GM]
			--	INNER JOIN [PRD_BIN_CARD_HDR]		 ON [BCH_PK] = [GMD_BIN_CARD]
			--	INNER JOIN [INV_WORK_ORDER_ITEM_HDR] ON [WIH_PK] = [GMR_WO]
			--	WHERE [GMR_GR] = @V_GRH_PK 
			--	--AND [GMR_SCRAP_OR_BGRADE] IN(1,2) /*Removed to get all grade*/

			--	;with Cte AS
			--	(
			--		SELECT   [GMR_QTY_RTRD]-SUM([T_GMD_QTY])	AS [Added_Qty]
			--				,[T_GMR_PK]							AS [PK]
			--				,MAX([T_GMD_PK])					AS [MAX_GMD_PK]
			--		FROM  @T_INV_GRN_MAT_RET_DTL
			--		INNER JOIN [INV_GRN_MAT_RET] ON [T_GMR_PK] = [GMR_PK]
			--		GROUP BY [T_GMR_PK],[GMR_QTY_RTRD]
			--		HAVING SUM([T_GMD_QTY]) < [GMR_QTY_RTRD]
			--	)
			--	UPDATE @T_INV_GRN_MAT_RET_DTL
			--	SET [T_GMD_QTY] = [T_GMD_QTY] + [Added_Qty]
			--	FROM Cte
			--	INNER JOIN @T_INV_GRN_MAT_RET_DTL ON [T_GMD_PK] = [MAX_GMD_PK]

			--	SET	@V_ROW_NO	= 1;					
			--	SELECT @V_MAX_ROW_NO = MAX([T_ROW_NO]) FROM @T_INV_GRN_MAT_RET_DTL
			--	WHILE (@V_ROW_NO <= @V_MAX_ROW_NO) 
			--	BEGIN
			--		SELECT @V_GMD_PK		= [T_GMD_PK]
			--			  ,@V_GMD_BIN_CARD	= [T_GMD_BIN_CARD]
			--			  ,@V_WIH_DEPT_TO	= [T_GMD_DEPT_TO]
			--		FROM   @T_INV_GRN_MAT_RET_DTL 

			--		WHERE [T_ROW_NO] = @V_ROW_NO				

			--		SET @V_PROD_XML =	(SELECT  [T_GMD_BIN_CARD] AS [BSD_BIN_CARD]
			--							        ,@V_USER_PK       AS [USER_PK] 
			--							        ,(	SELECT	 [BSD_PK]				AS [BSD_PK]			
			--							        			,[T_GMD_MATERIAL]		AS [BSD_PRODUCT]		
			--							        			,([T_GMD_QTY] * ISNULL([T_GMD_AVG_GLOVE_WT],0)) / 1000						
			--																		AS [BSD_TOTAL_WT]
			--							        			,[T_GMD_AVG_GLOVE_WT]	AS [BSD_AVG_GLOVE_WT]
			--							        			,[T_GMD_QTY]			AS [BSD_TOTAL_PCS]
			--							        			,@V_ROW_NO				AS [BSD_SL_NO]	
			--							        			,[BSD_TRN_NO]			AS [BSD_TRN_NO]
			--							        			,1						AS [BSD_STATUS]	
			--							        			,[BSD_MOD_DT]			AS [LAST_MOD_DT]
			--							        			,@V_GRH_DATE			AS [BSD_TRN_DATE]	
			--							        	FROM @T_INV_GRN_MAT_RET_DTL								        	 
			--							        	LEFT JOIN [PRD_BIN_CARD_PROD_SPLIT_DTL] ON		[BSD_BIN_TAB] =  46 
			--							        												AND [BSD_TRN]	  = @V_GMD_PK 
			--							        												AND [BSD_TYPE]	  = 3 -- GRN Return
			--							        	WHERE [T_ROW_NO] = @V_ROW_NO
			--							        	AND ([T_GMD_QTY]) > 0
			--							        	FOR XML PATH('Detail'),TYPE
			--							        	)
			--					            FROM @T_INV_GRN_MAT_RET_DTL
			--					            WHERE [T_ROW_NO] = @V_ROW_NO
			--					            FOR XML PATH('Root'),ELEMENTS)
					
			--		EXEC [SPPRD_BIN_CARD_PROD_SPLIT_SAVE]
			--				 @P_XML				=	@V_PROD_XML
			--				,@P_BSD_BIN_TAB		=	46		
			--				,@P_BSD_TRN			=	@V_GMD_PK	
			--				,@P_BSD_TYPE		=	3			-- GRN Return
			--				,@P_BSD_FROM_DEPT	=	@V_WIH_DEPT_TO
			--				,@P_RET_VAL			=	@PRetVal	OUTPUT
						
			--		IF @PRetVal < 0
			--		BEGIN
			--			RETURN 
			--		END
			--		SET @V_ROW_NO = @V_ROW_NO + 1
			--	END

			--	SET @PRetVal = 1;
	END TRY
	BEGIN CATCH
		SET	@pRetVal = -1
	END CATCH
END
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
