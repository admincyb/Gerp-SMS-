/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	Author			:	GTI-262
	DATE			:	13-01-2025
	Purpose			:	To Save the Transactions and Run the Job
	DECLARE  @Json			NVARCHAR(MAX)	=	'{	"RefId"	:	8,
													"ApplicationId"	: 1,
													"Url"	: null,
													"Module": 1,
													"MenuId": 2,
													"UserId" : 41
												}'	
			,@ErrorMessage	NVARCHAR(2000)	

	EXEC [WorkFlowSave]	 @Json			=	@Json
						,@ErrorMessage	=	@ErrorMessage	OUTPUT

	SELECT @ErrorMessage	[RetValue]
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	Modified by		On		Remarks
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
CREATE PROCEDURE [dbo].[WorkFlowSave]
(
	 @RefId						INT
	,@ProcessID					INT
	,@ApplicationId				BIGINT
	,@ApplicationNo				NVARCHAR(100)
	,@CurrProcessActionId		INT
	,@UserId					INT
	,@Comments					NVARCHAR(2000)
	,@DivisionGroup				INT				=   NULL
	,@ErrorNumber				INT				=	NULL	OUTPUT
	,@ErrorMessage				NVARCHAR(2000)	=	NULL	OUTPUT
)
AS
BEGIN
--EXEC WorkFlowSave '{"ProcessId": 85, "ApplicationId": 1, "CurrProcessActionId": 138}'
	DECLARE	@JobProcedures TABLE
		(	
			 [RowNo]				INT
			,[jobProcedure]			NVARCHAR(200)
		)
	
	BEGIN TRY
	--BEGIN TRANSACTION

	
	DECLARE @vRowNo									INT,
			@vRowNoMax								INT,
			@JobProcedure							VARCHAR(100)


	EXEC	[WkfTransactionSave]
			 @RefId					=	@RefId				
			,@ProcessID				=	@ProcessID			
			,@ApplicationId			=	@ApplicationId			
			,@CurrProcessActionId	=	@CurrProcessActionId		
			,@UserId				=	@UserId
			,@Comments				=	@Comments
			,@ApplicationNo			=	@ApplicationNo
			,@DivisionGroup			=	@DivisionGroup	
			,@PRetVal				=	@ErrorNumber	OUTPUT
			
	IF @ErrorNumber < 0
	BEGIN

		--ROLLBACK TRAN
		RETURN
	END

	SET @RefId = @ErrorNumber

	----------------------
	--Normal Workflow Job
	----------------------
	INSERT INTO @JobProcedures
		(
			 [RowNo]
			,[jobProcedure]
		)
	SELECT	ROW_NUMBER() OVER (ORDER BY JSM.[Id])	AS	[RowNo]
			,[ProcedureName]	
	FROM	 [JobScheduleMappingMaster] JMM
			INNER JOIN [JobScheduleMaster]	JSM		ON JSM.Id			= JMM.JobId
	WHERE	JMM.ProcessActionId	= @CurrProcessActionId
		AND JSM.[Active]		= 1
	ORDER BY JSM.[Id]
	
	SET		@vRowNo		= 1
	SET		@vRowNoMax	= 0

	SELECT @vRowNoMax = ISNULL(MAX([RowNo]),0) FROM @JobProcedures
	WHILE (@vRowNo <= @vRowNoMax) 
	BEGIN
		SELECT	@JobProcedure	=	[jobProcedure]					
		FROM	@JobProcedures
		WHERE   [RowNo] = @vRowNo
		EXEC	[WkfJobExecute]
				@PjobProcedure		=	@JobProcedure
				,@PrefApplication	=	@ApplicationId
				,@PrefPK			=	@RefId
				,@PRetVal			=	@ErrorNumber OUTPUT
		
		IF	( @ErrorNumber < 0)
		BEGIN
		
			--ROLLBACK TRAN
			RETURN
		END

		SET @vRowNo = @vRowNo + 1
	END

	SELECT @ErrorMessage= [ApplicationNo]
	FROM [TransactionReferenceMaster]
	WHERE [RefId]	=	@RefId
	
		
	--COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
	select @ErrorNumber '@ErrorNumber'
		--ROLLBACK TRANSACTION
		IF ERROR_NUMBER()	= 547 --Foreign key violation
		BEGIN
			SET @ErrorNumber	= 0
		END
		ELSE
		BEGIN
			SET @ErrorNumber	= -1

		END
		
		SELECT	ERROR_LINE()	ErrorLine,	ERROR_MESSAGE()		ErrorMessage,
				ERROR_NUMBER()	ErrorNo,	ERROR_PROCEDURE()	ErrorProcedure
	END CATCH
END
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
