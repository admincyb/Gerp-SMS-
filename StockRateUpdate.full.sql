 /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	Author	: GTI-138
	Date	: 08-01-2015
	Purpose : *	Update stk rate w.r.t new transaction
			  *	Use this procedure after every receipt trx into table [INV_STK_TRX_DTL]
	Execute :	BEGIN TRANSATION
					DECLARE	@P_RET_VAL INT
					EXEC	[StockRateUpdate] 
							@P_ITD_PK_NEW	=	1000001
							,@P_RET_VAL		= 	@P_RET_VAL OUTPUT
					SELECT	@P_RET_VAL	AS [RET_VAL]
				ROLLBACK TRANSACTION
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	Modified By			On				Remarks
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	[SPINV_STK_RATE_UPDATE]	>>>	[StockRateUpdate]
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
CREATE PROCEDURE [dbo].[StockRateUpdate]
(
	 @P_ITD_PK_NEW	INT
	,@P_RET_VAL		INT	OUTPUT
)
AS
BEGIN

	SET NOCOUNT ON;
	BEGIN TRY
	
		DECLARE @V_ITH_DATE_SL_NO	BIGINT
				,@V_ITEM			INT
				,@V_DEPT			INT
				,@V_ITD_PK_NEXT		INT
				,@V_ITH_RATE_DTL	FLOAT
				,@V_ITD_TRX_MODE	TINYINT
				,@V_ITD_TRN			INT
				,@V_DEPT_TO			INT			

		-------------------------------------
		--Get New stock transaction details
		-------------------------------------

		SELECT	@V_ITH_DATE_SL_NO	=	[DateSlNo]
				,@V_ITEM			=	[Item]
				,@V_DEPT			=	[ModuleId]
		FROM	[StockTransactionsDetail]
		WHERE	[Id]	=	@P_ITD_PK_NEW

		-------------------------------------
		--Get Next stock transaction details
		-------------------------------------
		SELECT	TOP 1 
				@V_ITD_PK_NEXT			=	[Id]
		FROM	[StockTransactionsDetail]
		WHERE	[Item]				=	@V_ITEM
			AND	[ModuleId]			=	@V_DEPT
			AND	[DateSlNo]			>	@V_ITH_DATE_SL_NO
			AND [QuantityIssue]		>	0
			AND [TransactionMode]	NOT IN	(12,21,6)		/*6-ADJ,12-STR,21-EMR*/	-- Here no need to consider these trx, since rates are coming from UI
		ORDER BY [DateSlNo]

		WHILE	@V_ITD_PK_NEXT IS NOT NULL
		BEGIN	
				----------------------------------------
				--Update rate with previous transactions
				----------------------------------------
				SELECT		@V_ITH_RATE_DTL =		CASE WHEN ISNULL(CAST((	SELECT	SUM(T.[QuantityStockReceipt] - T.[QuantityStockIssue])	
																			FROM	[StockTransactionsDetail] T
																			WHERE	T.[DateSlNo]<	S.[DateSlNo]
																				AND	T.[ModuleId]=	S.[ModuleId]
																				AND	T.[Item]	=	S.[Item])
																		AS NUMERIC(19,6)),0) > 0
														 THEN
																ISNULL((	SELECT	SUM(ISNULL(T.[QuantityStockReceipt],0) * T.[RateDetail]) - SUM(ISNULL(T.[QuantityStockIssue],0) * T.[RateDetail])	
																			FROM	[StockTransactionsDetail] T
																			WHERE	T.[DateSlNo]<	S.[DateSlNo]
																				AND	T.[ModuleId]=	S.[ModuleId]
																				AND	T.[Item]	=	S.[Item])
																			,0)
																	/
																ISNULL((	SELECT	SUM(T.[QuantityStockReceipt] - T.[QuantityStockIssue])	
																				FROM	[StockTransactionsDetail] T
																				WHERE	T.[DateSlNo]<	S.[DateSlNo]
																					AND	T.[ModuleId]=	S.[ModuleId]
																					AND	T.[Item]	=	S.[Item])
																			,0)
														ELSE 0
													END
				FROM	[StockTransactionsDetail]	S
				WHERE	[Id] = @V_ITD_PK_NEXT				

				UPDATE	S
				SET		[RateDetail]		=	@V_ITH_RATE_DTL
						,@V_ITH_DATE_SL_NO	=	[DateSlNo]
						,@V_ITD_TRX_MODE	=	[TransactionMode]
						,@V_ITD_TRN			=	[Transaction]
				FROM	[StockTransactionsDetail]	S
				WHERE	[Id] = @V_ITD_PK_NEXT				

				---------------------
				--If Stock Transafer
				---------------------
				--//Update TO store IN stock rate
				IF @V_ITD_TRX_MODE IN (8) --(MI)(stock transafer)
				BEGIN
					UPDATE	[StockTransactionsDetail]
					SET		[RateDetail]		=	@V_ITH_RATE_DTL
							,@V_DEPT_TO		=	[ModuleId]
					WHERE	[TransactionMode]		=	@V_ITD_TRX_MODE
						AND [Transaction]			=	@V_ITD_TRN
						AND	[Item]					=	@V_ITEM
						AND	[QuantityStockReceipt]	>	0


					-----------------------------
					--Update Stock Rate (To Store)
					-----------------------------
					UPDATE	S
					SET		[Rate]	=	CAST(	CASE	WHEN	CAST([QuantityInStock] AS NUMERIC(19,6)) > 0
															THEN	[STD_VALUE_NEW] /[QuantityInStock]
															ELSE	0
													END
											AS NUMERIC(15,3))
							,[Value] =	[STD_VALUE_NEW]
					FROM	[StockDetail]	S
					CROSS JOIN (	SELECT	SUM(ISNULL(T.[QuantityStockReceipt],0) * [RateDetail]) - SUM(ISNULL(T.[QuantityStockIssue],0) * [RateDetail])	AS	[STD_VALUE_NEW]
									FROM	[StockTransactionsDetail] T
									WHERE	T.[ModuleId]= @V_DEPT_TO
										AND	T.[Item]	= @V_ITEM
								) W
					WHERE	[Module]=	@V_DEPT_TO
						AND	[Item]	=	@V_ITEM
				END

				SET @V_ITD_PK_NEXT = NULL
				-------------------------------------
				--Get Next stock transaction details
				-------------------------------------
				SELECT	TOP 1 
						@V_ITD_PK_NEXT			=	[Id]
				FROM	[StockTransactionsDetail]
				WHERE	[Item]					=	@V_ITEM
					AND	[ModuleId]				=	@V_DEPT
					AND	[DateSlNo]				>	@V_ITH_DATE_SL_NO
					AND [QuantityStockIssue]	>	0
					AND [TransactionMode]		NOT IN	(12,21,6)		/*6-ADJ,12-STR,21-EMR*/	-- Here no need to consider these trx, since rates are coming from UI
				ORDER BY [DateSlNo]
		END

		----------------
		--Update Stock 
		----------------
		UPDATE	S
		SET		[QuantityInStock] = CAST([STD_QTY_IN_STOCK_NEW] AS NUMERIC(19,4))
		FROM	[StockDetail]	S
		CROSS JOIN (	SELECT	SUM([QuantityStockReceipt]-[QuantityStockIssue])	AS	[STD_QTY_IN_STOCK_NEW]
						FROM	[StockTransactionsDetail] T
						WHERE	T.[ModuleId]		= @V_DEPT
							AND	T.[Item]		= @V_ITEM
					) W
		WHERE	[Module]=	@V_DEPT
			AND	[Item]	=	@V_ITEM

		--------------------
		--Update Stock Rate
		-------------------

		UPDATE	S
		SET	 [Rate]	=	CASE	WHEN	[QuantityInStock] > 0
									THEN
											[STD_VALUE_NEW] /	[QuantityInStock]
									ELSE	0
							END
			,[Value] =	CASE	WHEN	[QuantityInStock] > 0
									THEN
											[STD_VALUE_NEW]
									ELSE	0
								END
		FROM	[StockDetail]	S
		CROSS JOIN (	SELECT	SUM(ISNULL(T.[QuantityStockReceipt],0) * [RateDetail]) - SUM(ISNULL(T.[QuantityStockIssue],0) * [RateDetail]) AS	[STD_VALUE_NEW]
						FROM	[StockTransactionsDetail] T
						WHERE	T.[ModuleId]= @V_DEPT
							AND	T.[Item]	= @V_ITEM
					) W
		WHERE	[Module]=	@V_DEPT
			AND	[Item]	=	@V_ITEM

		SET	@P_RET_VAL = 1

	END TRY
	BEGIN CATCH
		SET @P_RET_VAL	= -1
	END CATCH
END
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
