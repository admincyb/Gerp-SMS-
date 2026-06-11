/*Changes
			DATE								Changes
		01-June-2015						IPD_THICKNESS changed to NULL from 0.000
*/
IF EXISTS (SELECT 1 FROM sys.objects where name like 'SP_BRAND_IMPORT')
BEGIN 
	DROP PROCEDURE dbo.SP_BRAND_IMPORT
END 
GO

CREATE PROCEDURE dbo.SP_BRAND_IMPORT
AS
BEGIN

BEGIN TRY	
	BEGIN TRAN
		
		DECLARE @V_DEPT		INT	=	86	--PACKING - Inv Store

		DECLARE	@V_APS_PK	INT			--ADM_PACK_SPEC_MST
				,@V_CON_PK	INT			--ADM_CONST_MST
				,@V_ITM_PK	INT			--INV_ITEM_MST
				,@V_IPD_PK	INT			--INV_ITEM_PACK_DTL
				
				,@V_ITV_PK	INT			--INV_ITEM_VENDOR_MAP
				,@V_VIH_PK	INT			--INV_ITEM_VENDOR_HISTORY
				,@V_CIM_PK	INT			--CRM_CUST_ITEM_MAP
				,@V_PIM_PK	INT			--ADM_PACK_CUST_ITEM_MAP 
				,@V_BMD_PK	INT			--ADM_PACK_CUST_ITEM_MAP_DTL
				
		DECLARE @V_BASE_CURR	INT	=	(	SELECT	ACF_DATA
											FROM	ADM_APP_CONFIG_MST	
											WHERE	ACF_SETTING	='BASE CURRENCY')
		
		/***START	:	1.TEMPORARY UPDATIONS*/
		--1.Update Pack Qty
		--UPDATE	Z_BWH_BRANDS
		--SET		PACK	=	1
		--WHERE	ISNULL(PACK,0)	=	0
				
		--2.Update Brand Name (Size Missing)
		--UPDATE	Z_BWH_BRANDS
		--SET		BRANDNAME	=	LTRIM(BRANDNAME + ISNULL(' ('+ CAST(SUBSTRING(PRDCODE,12,3) AS NVARCHAR(10)) +')',''))
		
		
		--3.Update Product Code
		--UPDATE	Z_BWH_BRANDS
		--SET		PRDCODE = ITM_CODE
		--FROM	Z_BWH_BRANDS
		--INNER JOIN	INV_ITEM_MST			ON	ITM_PK	=	286 + ROWNO 
		--WHERE	ITM_CATEGORY	=	2

		--DECLARE	@V_CUS_PK		INT
		--SELECT	@V_CUS_PK = ISNULL(MAX(CUS_PK),0) FROM CRM_CUSTOMER_MST 
		
		----*CUSTOMER
		--INSERT INTO CRM_CUSTOMER_MST
		--(
		--		 CUS_PK
		--		,CUS_CODE
		--		,CUS_NAME
				
		--		,CUS_COUNTRY
		--		,CUS_CURRENCY
		--		,CUS_CURRENCY_RATE
				
		--		,CUS_BUILDING
		--		,CUS_STREET
				
		--		,CUS_STATE_OTHER
		--		,CUS_ZIP
		--		,CUS_WEBSITE
		--		,CUS_PHONE
		--		,CUS_MOBILE
		--		,CUS_FAX
		--		,CUS_EMAIL
				
		--		,CUS_CREDIT_LIMIT
		--		,CUS_CREDIT_DAYS
		--		,CUS_SHIP_TO_PORT
				
		--		,CUS_STATUS
		--		,CUS_ACTIVE
		--		,CUS_BIZUNIT
		--		,CUS_CRTD_BY
		--		,CUS_CRTD_DT
		--		,CUS_MOD_BY
		--		,CUS_MOD_DT
		--)
		--SELECT	@V_CUS_PK + ROW_NUMBER() OVER(ORDER BY ROWNO)
		--		,CODE
		--		,NAME
				
		--		,ISNULL(CNT_PK,239)					--	THAILAND
		--		,ISNULL(CUR_PK,@V_BASE_CURR)		--	THB
		--		,dbo.FNADM_CURRENCY_CONV_FACT_GET(ISNULL(CUR_PK,@V_BASE_CURR),@V_BASE_CURR,GETDATE())
				
		--		,ADDRESS1
		--		,ADDRESS2
				
		--		,STATE_TEXT
		--		,ZIP
		--		,WEBSITE
		--		,PHONE
		--		,MOBILE
		--		,FAX
		--		,EMAIL
				
		--		,CREDIT_LIMIT
		--		,CREDIT_DAYS
		--		,SHIP_TO_PORT
				
		--		,0						--STATUS
		--		,1
		--		,1
		--		,1
		--		,GETDATE()
		--		,1
		--		,GETDATE()
		--FROM	Z_BWH_CUSTOMER
		--	LEFT OUTER JOIN ADM_COUNTRY_MST		ON	CNT_NAME	=	COUNTRY
		--	LEFT OUTER JOIN ADM_CURRENCY_MST	ON	CUR_CODE	=	CURRENCY
		--	--LEFT OUTER JOIN ADM_STATE_MST		ON	STT_NAME	=	STATE_TEXT	
		--	--									AND	STT_COUNTRY	=	CNT_PK
			
		--	LEFT OUTER JOIN CRM_CUSTOMER_MST	ON	CUS_CODE	=	CODE
		--										OR	CUS_NAME	=	NAME
		--WHERE	CUS_PK	IS NULL		
		
		--UPDATE	ZB
		--SET		ZB.PRDCODE	=	ZP.UPDATEDCODE
		--FROM	Z_BWH_BRANDS	AS	ZB
		--	LEFT JOIN Z_BWH_PRODUCTS	ZP	ON	/*ZP.OLDCODE	=	ZB.OLDPRDCODE*/
		--										ZP.NEWCODE	=	ZB.PRDCODE
		--									--AND	ZP.CUSCODE	=	ZB.CUSCODE
		
		--DELETE FROM  Z_BWH_BRANDS	WHERE	PRDCODE IS NULL		
		--								OR	ROWNO IN (496)	
					
		--UPDATE	ZB
		--SET		ZB.PRDCODE	=	ZP.UPDATEDCODE
		--FROM	Z_BWH_BRANDS	AS	ZB
		--	LEFT JOIN Z_BWH_PRODUCTS	ZP	ON	ZP.NEWCODE	=	ZB.PRDCODE
		--									AND	ZP.CUSCODE	=	ZB.CUSCODE
		--WHERE	ZB.PRDCODE	IS NULL
		
		/***END	:	TEMPORARY UPDATIONS*/
		/* validation in SSIS
		--Check Product Exists or Not
		IF EXISTS (	SELECT	1
					FROM	Z_BWH_BRANDS
					WHERE	ISNULL(PRDCODE,'')	=	'')
		BEGIN
			SELECT 'Product code Missing'	AS VALIDATION_TEXT
		
			SELECT	ROWNO
					,CUSCODE
					,BRANDCODE
					,BRANDNAME
					,PRDCODE
			FROM	Z_BWH_BRANDS
			WHERE	ISNULL(PRDCODE,'')	=	''
			ORDER BY ROWNO
			
			ROLLBACK TRAN
			RETURN
		END

		--Check Prdoduct master
		IF EXISTS ( SELECT	1
					FROM	Z_BWH_BRANDS
						LEFT OUTER JOIN INV_ITEM_MST	ON	ITM_CODE = PRDCODE
														AND	ITM_CATEGORY	=	2
					WHERE	ITM_PK IS NULL)
		BEGIN
			SELECT 'Product Not Found'	AS VALIDATION_TEXT
		
			SELECT	ROWNO
					,CUSCODE
					,OLDPRDCODE
					,PRDCODE
					,ITM_CODE
			FROM	Z_BWH_BRANDS
				LEFT OUTER JOIN INV_ITEM_MST	ON	ITM_CODE = PRDCODE
												AND	ITM_CATEGORY	=	2
			WHERE	ITM_PK IS NULL
			ORDER BY ROWNO
			
			ROLLBACK TRAN
			RETURN
		END

		DECLARE	@PM_INV_ITEM_MST TABLE
			(
				 ROW_NO					INT	IDENTITY (1,1)
				,BRAND_NO				INT	
				,ITM_CODE				NVARCHAR(100)
				,ITM_NAME				NVARCHAR(200)
				,ITM_CATEGORY			INT
				,ITM_TYPE				INT
				,ITM_WEIGHT				FLOAT
				,ITM_MIN_WEIGHT			FLOAT
				,ITM_MAX_WEIGHT			FLOAT
				,ITM_UOM				INT
				
				,[IPD_TYPE]				INT
				,[IPD_INNER_LENGTH]		INT
				,[IPD_INNER_BREADTH]	INT
				,[IPD_INNER_HEIGHT]		INT
				,[IPD_OUTER_LENGTH]		INT
				,[IPD_OUTER_BREADTH]	INT
				,[IPD_OUTER_HEIGHT]		INT
				,[IPD_PLY]				NVARCHAR(200)
				,[IPD_PAPER_COLOR]		NVARCHAR(200)
				,[IPD_ART_WORK]			NVARCHAR(200)
				,[IPD_CUSTOMER]			INT
				,[IPD_THICKNESS]		FLOAT
				,[IPD_VEN_CODE]			NVARCHAR(200)
				,[IPD_VEN_MAT_CODE]		NVARCHAR(200)
				,[IPD_RATE]				FLOAT
				,[IPD_PTYPE]			NVARCHAR(200)
			)

		--Check Brand Duplication
		IF EXISTS	(
			SELECT	BRANDNAME
			FROM	Z_BWH_BRANDS_VW
			GROUP BY	CUSCODE
						,BRANDCODE
						,BRANDNAME
			HAVING	COUNT(*) > 1
		)
		BEGIN
		
			SELECT 'Brand Duplicates in XL File'	AS VALIDATION_TEXT
		
			SELECT	CUSCODE
					,BRANDCODE
					,BRANDNAME
					,COUNT(*)	AS	REC_COUNT
			FROM	Z_BWH_BRANDS_VW
			GROUP BY	CUSCODE
						,BRANDCODE
						,BRANDNAME
			HAVING	COUNT(*) > 1
						
			ROLLBACK TRAN
			RETURN
		END
		
		--Check Brand already exists
		IF EXISTS	(
			SELECT	CIM_PK
			FROM	Z_BWH_BRANDS
				INNER JOIN	CRM_CUST_ITEM_MAP	ON	BRANDNAME	=	CIM_BRAND_NAME
				INNER JOIN	CRM_CUSTOMER_MST	ON	CUS_PK		=	CIM_CUSTOMER
												AND	CUS_CODE	=	CUSCODE
		)
		BEGIN
			SELECT	'Brand(s) Already Exists'
			
			SELECT	CUSCODE
					,BRANDNAME
					,BRANDCODE
			FROM	Z_BWH_BRANDS
				INNER JOIN	CRM_CUST_ITEM_MAP	ON	BRANDNAME	=	CIM_BRAND_NAME
				INNER JOIN	CRM_CUSTOMER_MST	ON	CUS_PK		=	CIM_CUSTOMER
												AND	CUS_CODE	=	CUSCODE
			
			ROLLBACK TRAN
			RETURN
		END
		
		--Check Packing Qty
		IF EXISTS (	SELECT	1
					FROM	Z_BWH_BRANDS
					WHERE	ISNULL(PACK,0)  <= 0
						OR	ISNULL(CARTON,0)  <= 0)
		BEGIN
			SELECT 'Packing Qty Missing'	AS VALIDATION_TEXT
		
			SELECT	CUSCODE
					,BRANDCODE
					,BRANDNAME
					,PACK
					,CARTON
			FROM	Z_BWH_BRANDS
					WHERE	ISNULL(PACK,0)  <= 0
						OR	ISNULL(CARTON,0)  <= 0
						
			ROLLBACK TRAN
			RETURN
		END
		
		--Check Product Exists or Not
		IF EXISTS (	SELECT	1
					FROM	Z_BWH_BRANDS
					WHERE	ISNULL(PRDCODE,'')	=	'')
		BEGIN
			SELECT 'Product code Missing'	AS VALIDATION_TEXT
		
			SELECT	CUSCODE
					,BRANDCODE
					,BRANDNAME
			FROM	Z_BWH_BRANDS
			WHERE	ISNULL(PRDCODE,'')	=	''
						
			ROLLBACK TRAN
			RETURN
		END

		--Temporay Packing Materials
		PRINT 'Temporay Packing Materials'
		INSERT INTO @PM_INV_ITEM_MST
		SELECT	BRAND_NO
				,PM_CODE
				,PM_NAME
				,12
				,1
				,PM_WEIGHT
				,0
				,0
				,8
				,PMT.CON_PK		--PACKING MATERIAL PK
				,PM_IL
				,PM_IB
				,PM_IH
				,PM_OL
				,PM_OB
				,PM_OH
				,PM_PLY
				,PM_PAPER_COLOUR
				--,PM_ARTWORK
				,CAST((RIGHT(YEAR(GETDATE()),2)*100)+RIGHT(MONTH(GETDATE()),2) AS NVARCHAR(4))+'-1'
				,CUS_PK
				,PM_THICKNESS
				,PM_VEN_CODE
				,PM_VEN_MAT_CODE
				,PM_RATE
				,PM_PTYPE
		FROM	(
			SELECT	ROWNO		AS BRAND_NO
					,IB_PMC		AS	PM_CODE
					,IB_PMN + ' mm'		AS	PM_NAME
					,IB_WT		AS	PM_WEIGHT
					,'IB'		AS	PM_DATA
					,IB_IL		AS	PM_IL
					,IB_IW		AS	PM_IB
					,IB_IH		AS	PM_IH
					,IB_OL		AS	PM_OL
					,IB_OW		AS	PM_OB
					,IB_OH		AS	PM_OH
					,IB_PLY		AS	PM_PLY
					,IB_PC		AS	PM_PAPER_COLOUR
					,IB_AWF		AS	PM_ARTWORK
					,IB_THK		AS	PM_THICKNESS
					,IB_VND		AS	PM_VEN_CODE
					,IB_VMC		AS	PM_VEN_MAT_CODE
					,CUSCODE	AS	PM_CUS_CODE
					,IB_RATE	AS	PM_RATE
					,IB_TYPE	AS	PM_PTYPE
			FROM	Z_BWH_BRANDS
			WHERE	IB_QTY	>	0
			UNION
			SELECT	ROWNO
					,WL_PMC
					,WL_PMN
					,WL_WT
					,'WLT'
					,WL_IL
					,WL_IW
					,WL_IH
					,WL_OL
					,WL_OW
					,WL_OH
					,WL_PLY
					,WL_PC
					,WL_AWF
					,WL_THK
					,WL_VND	
					,WL_VMC
					,CUSCODE
					,WL_RATE
					,WL_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	WL_QTY	>	0
			UNION
			SELECT	ROWNO
					,MC_PMC
					,MC_PMN + ' mm'
					,MC_WT
					,'MC'
					,MC_IL
					,MC_IW
					,MC_IH
					,MC_OL
					,MC_OW
					,MC_OH
					,MC_PLY
					,MC_PC
					,MC_AWF
					,MC_THK
					,MC_VND	
					,MC_VMC
					,CUSCODE
					,MC_RATE
					,MC_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	MC_QTY	>	0
			UNION
			SELECT	ROWNO
					,PC_PMC
					,PC_PMN
					,PC_WT
					,'PC'
					,PC_IL
					,PC_IW
					,PC_IH
					,PC_OL
					,PC_OW
					,PC_OH
					,PC_PLY
					,PC_PC
					,PC_AWF
					,PC_THK
					,PC_VND	
					,PC_VMC
					,CUSCODE
					,PC_RATE
					,PC_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	PC_QTY	>	0
			UNION
			SELECT	ROWNO
					,ZB_PMC
					,ZB_PMN + ' in'
					,ZB_WT
					,'ZB'
					,ZB_IL
					,ZB_IW
					,ZB_IH
					,ZB_OL
					,ZB_OW
					,ZB_OH
					,ZB_PLY
					,ZB_PC
					,ZB_AWF
					,ZB_THK
					,ZB_VND	
					,ZB_VMC
					,CUSCODE
					,ZB_RATE
					,ZB_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	ZB_QTY	>	0
			UNION
			SELECT	ROWNO
					,OP_PMC
					,OP_PMN + ' in'
					,OP_WT
					,'POB'
					,OP_IL
					,OP_IW
					,OP_IH
					,OP_OL
					,OP_OW
					,OP_OH
					,OP_PLY
					,OP_PC
					,OP_AWF
					,OP_THK
					,OP_VND	
					,OP_VMC
					,CUSCODE
					,OP_RATE
					,OP_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	OP_QTY	>	0
			UNION
			SELECT	ROWNO
					,OR_PMC
					,OR_PMN + ' in'
					,OR_WT
					,'PRB'
					,OR_IL
					,OR_IW
					,OR_IH
					,OR_OL
					,OR_OW
					,OR_OH
					,OR_PLY
					,OR_PC
					,OR_AWF
					,OR_THK
					,OR_VND	
					,OR_VMC
					,CUSCODE
					,OR_RATE
					,OR_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	OR_QTY	>	0
			--Non Stock Material
			UNION
			SELECT	ROWNO
					,IC_PMC
					,IC_PMN + ' in'
					,IC_WT
					,'CIB'	--Cleanroom Inner Bag
					,IC_IL
					,IC_IW
					,IC_IH
					,IC_OL
					,IC_OW
					,IC_OH
					,IC_PLY
					,IC_PC
					,IC_AWF
					,IC_THK
					,IC_VND	
					,IC_VMC
					,CUSCODE
					,IC_RATE
					,IC_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	IC_QTY	>	0
			UNION
			SELECT	ROWNO
					,IN_PMC
					,IN_PMN + ' in'
					,IN_WT
					,'NIB'	--Normal Inner Bag (Printed/ Plain)
					,IN_IL
					,IN_IW
					,IN_IH
					,IN_OL
					,IN_OW
					,IN_OH
					,IN_PLY
					,IN_PC
					,IN_AWF
					,IN_THK
					,IN_VND	
					,IN_VMC
					,CUSCODE
					,IN_RATE
					,IN_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	IN_QTY	>	0
			UNION
			SELECT	ROWNO
					,LC_PMC
					,LC_PMN
					,LC_WT
					,'CLR'	--Carton Liner
					,LC_IL
					,LC_IW
					,LC_IH
					,LC_OL
					,LC_OW
					,LC_OH
					,LC_PLY
					,LC_PC
					,LC_AWF
					,LC_THK
					,LC_VND	
					,LC_VMC
					,CUSCODE
					,LC_RATE
					,LC_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	LC_QTY	>	0
			UNION
			SELECT	ROWNO
					,IP_PMC
					,IP_PMN
					,IP_WT
					,'ITP'	--Insert Paper / Insert Card
					,IP_IL
					,IP_IW
					,IP_IH
					,IP_OL
					,IP_OW
					,IP_OH
					,IP_PLY
					,IP_PC
					,IP_AWF
					,IP_THK
					,IP_VND	
					,IP_VMC
					,CUSCODE
					,IP_RATE
					,IP_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	IP_QTY	>	0
			UNION
			SELECT	ROWNO
					,RS_PMC
					,RS_PMN
					,RS_WT
					,'RST'	--Radiation Sticker
					,RS_IL
					,RS_IW
					,RS_IH
					,RS_OL
					,RS_OW
					,RS_OH
					,RS_PLY
					,RS_PC
					,RS_AWF
					,RS_THK
					,RS_VND	
					,RS_VMC
					,CUSCODE
					,RS_RATE
					,RS_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	RS_QTY	>	0
			UNION
			SELECT	ROWNO
					,SS_PMC
					,SS_PMN
					,SS_WT
					,'SST'	--Size Sticker
					,SS_IL
					,SS_IW
					,SS_IH
					,SS_OL
					,SS_OW
					,SS_OH
					,SS_PLY
					,SS_PC
					,SS_AWF
					,SS_THK
					,SS_VND	
					,SS_VMC
					,CUSCODE
					,SS_RATE
					,SS_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	SS_QTY	>	0
			UNION
			SELECT	ROWNO
					,BI_PMC
					,BI_PMN
					,BI_WT
					,'IBL'	--Inner Barcode Label
					,BI_IL
					,BI_IW
					,BI_IH
					,BI_OL
					,BI_OW
					,BI_OH
					,BI_PLY
					,BI_PC
					,BI_AWF
					,BI_THK
					,BI_VND	
					,BI_VMC
					,CUSCODE
					,BI_RATE
					,BI_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	BI_QTY	>	0
			UNION
			SELECT	ROWNO
					,BO_PMC
					,BO_PMN
					,BO_WT
					,'OBL'	--Outer Barcode Label
					,BO_IL
					,BO_IW
					,BO_IH
					,BO_OL
					,BO_OW
					,BO_OH
					,BO_PLY
					,BO_PC
					,BO_AWF
					,BO_THK
					,BO_VND	
					,BO_VMC
					,CUSCODE
					,BO_RATE
					,BO_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	BO_QTY	>	0
			UNION
			SELECT	ROWNO
					,IL_PMC
					,IL_PMN
					,IL_WT
					,'INL'	--Inner Label
					,IL_IL
					,IL_IW
					,IL_IH
					,IL_OL
					,IL_OW
					,IL_OH
					,IL_PLY
					,IL_PC
					,IL_AWF
					,IL_THK
					,IL_VND	
					,IL_VMC
					,CUSCODE
					,IL_RATE
					,IL_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	IL_QTY	>	0
			UNION
			SELECT	ROWNO
					,LL_PMC
					,LL_PMN
					,LL_WT
					,'LNL'	--Lot Number Label
					,LL_IL
					,LL_IW
					,LL_IH
					,LL_OL
					,LL_OW
					,LL_OH
					,LL_PLY
					,LL_PC
					,LL_AWF
					,LL_THK
					,LL_VND	
					,LL_VMC
					,CUSCODE
					,LL_RATE
					,LL_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	LL_QTY	>	0
			UNION
			SELECT	ROWNO
					,SB_PMC
					,SB_PMN
					,SB_WT
					,'OCL'	--Outer Carton / Shipper Bag Label
					,SB_IL
					,SB_IW
					,SB_IH
					,SB_OL
					,SB_OW
					,SB_OH
					,SB_PLY
					,SB_PC
					,SB_AWF
					,SB_THK
					,SB_VND	
					,SB_VMC
					,CUSCODE
					,SB_RATE
					,SB_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	SB_QTY	>	0
			UNION
			SELECT	ROWNO
					,PL_PMC
					,PL_PMN
					,PL_WT
					,'PCL'	--Pouch label
					,PL_IL
					,PL_IW
					,PL_IH
					,PL_OL
					,PL_OW
					,PL_OH
					,PL_PLY
					,PL_PC
					,PL_AWF
					,PL_THK
					,PL_VND	
					,PL_VMC
					,CUSCODE
					,PL_RATE
					,PL_TYPE
			FROM	Z_BWH_BRANDS
			WHERE	PL_QTY	>	0
			
		)	PM
		INNER JOIN	ADM_CONST_MST	PMT	ON	PMT.CON_GROUP	=	83	--PACKING MATERIAL
										AND	PMT.CON_DATA	=	PM_DATA
		LEFT JOIN	CRM_CUSTOMER_MST	ON	CUS_CODE		=	PM.PM_CUS_CODE
		WHERE	LEN(PM_CODE) > 0
			
		/***START	:	2.TEMPORARY UPDATIONS*/
		--1.Update Paper colour
		
		--select * from @PM_INV_ITEM_MST where itm_code ='PC-530MM BWAT LATEX HT'
		
		
		--UPDATE	@PM_INV_ITEM_MST
		--SET		IPD_PAPER_COLOR	=	'NATURAL'
		--WHERE	ITM_CODE		=	'CL-29x29LDPE'
		
		--UPDATE	@PM_INV_ITEM_MST
		--SET		IPD_PTYPE=	'LDPE'
		--		,IPD_OUTER_LENGTH = 16
		--WHERE	ITM_CODE		=	'NIB-12x16'
		
		--UPDATE	@PM_INV_ITEM_MST
		--SET		IPD_PTYPE=	'LDPE'
		--		,IPD_OUTER_LENGTH = 14
		--WHERE	ITM_CODE		=	'NIB-12x14'
		--/* 2015-03-24*/
		--UPDATE	@PM_INV_ITEM_MST
		--SET		IPD_OUTER_BREADTH	=	160
		--		,ITM_NAME			=	'Pouch - 420x160'
		--WHERE	ITM_CODE		=	'PC-530MM BWAT LATEX HT'

		
		
		/*
		UPDATE	@PM_INV_ITEM_MST
		SET		ITM_NAME	=	'Outer Carton - 360x254x251'
				,IPD_OUTER_BREADTH = 254
		WHERE	ITM_CODE		=	'MC-245MM BLACK NBXBLK'
		
		UPDATE	@PM_INV_ITEM_MST
		SET		ITM_NAME	=	'Outer Carton - 610x310x205'
				,IPD_OUTER_BREADTH = 310
				,IPD_OUTER_HEIGHT = 205
		WHERE	ITM_CODE		=	'MC-400MM LATEX BWAY '
		
		UPDATE	@PM_INV_ITEM_MST
		SET		ITM_NAME	=	'Outer Carton - 413x260x230'
				,IPD_OUTER_LENGTH	=	413
				,IPD_OUTER_BREADTH = 260
				,IPD_OUTER_HEIGHT = 230
		WHERE	ITM_CODE		=	'MC-LATEX 400MM GAYDE'
		
		UPDATE	@PM_INV_ITEM_MST
		SET		ITM_NAME	=	'Outer Carton - 413x260x230'
				,IPD_OUTER_LENGTH	=	413
				,IPD_OUTER_BREADTH = 260
				,IPD_OUTER_HEIGHT = 230
		WHERE	ITM_CODE		=	'MC-NITRILE 400MM PURPLE GAYDE'
		*/
		
		----select * from @PM_INV_ITEM_MST where ITM_CODE = 'PC-530MM BWAT LATEX HT'
		
		--4.Update Vendor Code
		--UPDATE	Z
		--SET		Z.IPD_VEN_CODE	=	VEN_CODE
		--FROM	@PM_INV_ITEM_MST	Z
		--	INNER JOIN	(
		--		SELECT	DISTINCT
		--				IPD_VEN_CODE
		--				,DENSE_RANK() OVER (ORDER BY IPD_VEN_CODE) VENPK
		--		FROM	@PM_INV_ITEM_MST
		--	)	D	ON	Z.IPD_VEN_CODE = D.IPD_VEN_CODE
		--	INNER JOIN PUR_VENDOR_MST	ON	 VEN_PK	=	VENPK
		
		/*
		UPDATE	@PM_INV_ITEM_MST
		SET		IPD_VEN_CODE	=	'FED EXP'
		WHERE	IPD_VEN_CODE	=	'FECC'
		
		UPDATE	@PM_INV_ITEM_MST
		SET		IPD_VEN_CODE	=	'PMA'
		WHERE	IPD_VEN_CODE	=	'PCS'
		
		UPDATE	@PM_INV_ITEM_MST
		SET		IPD_VEN_CODE	=	'EURO PP'
		WHERE	IPD_VEN_CODE	=	'EUROPLAS'
		*/
		
		UPDATE	P
		SET		P.IPD_INNER_LENGTH = IPD_OUTER_LENGTH
				,P.IPD_INNER_BREADTH = IPD_OUTER_LENGTH
				,P.IPD_INNER_HEIGHT = IPD_OUTER_HEIGHT
		FROM @PM_INV_ITEM_MST P
			INNER JOIN ADM_CONST_MST ON CON_PK = IPD_TYPE 
		WHERE	(IPD_OUTER_LENGTH > 0 OR IPD_OUTER_LENGTH > 0 OR IPD_OUTER_HEIGHT > 0)
			AND	CON_VALUE > 100
		
		/***END	:	TEMPORARY UPDATIONS*/

		--Check vendor is exists or not
		IF EXISTS	(
			SELECT	VEN_CODE
			FROM	@PM_INV_ITEM_MST
				LEFT OUTER JOIN PUR_VENDOR_MST	ON	VEN_CODE	=	IPD_VEN_CODE
			WHERE	VEN_PK	IS NULL
				AND	ISNULL(IPD_VEN_CODE,'') <> ''
		)
		BEGIN
		
			SELECT	'Vendor Not Found'		AS VALIDATION_TEXT
			
			SELECT	DISTINCT [IPD_VEN_CODE]			AS	[VENDOR_CODE]
					,PMT.CON_NAME					AS	[PACK_MAT_TYPE]
			FROM	@PM_INV_ITEM_MST
				INNER JOIN	ADM_CONST_MST	PMT	ON	PMT.CON_PK	=	IPD_TYPE
				LEFT OUTER JOIN PUR_VENDOR_MST	ON	VEN_CODE	=	IPD_VEN_CODE
			WHERE	VEN_PK	IS NULL
				AND	ISNULL(IPD_VEN_CODE,'') <> ''
			ORDER BY [VENDOR_CODE]
			
			ROLLBACK TRAN
			RETURN
		END
		
		
		--Check Packing Material Duplication
		IF EXISTS	(
			SELECT	ITM_CODE
			FROM	(SELECT	DISTINCT
							ITM_CODE			
							,ITM_NAME			
							,ITM_CATEGORY		
							,ITM_TYPE			
							,ITM_WEIGHT			
							,ITM_MIN_WEIGHT		
							,ITM_MAX_WEIGHT		
							,ITM_UOM			

							,[IPD_TYPE]			
							,[IPD_INNER_LENGTH]	
							,[IPD_INNER_BREADTH]
							,[IPD_INNER_HEIGHT]	
							,[IPD_OUTER_LENGTH]	
							,[IPD_OUTER_BREADTH]
							,[IPD_OUTER_HEIGHT]	
							,[IPD_PLY]			
							,[IPD_PAPER_COLOR]	
							,[IPD_ART_WORK]		
							--,[IPD_CUSTOMER]		
							,[IPD_THICKNESS]	
							,[IPD_PTYPE]
					FROM	@PM_INV_ITEM_MST)	AS	D
			WHERE	ITM_CODE IS NOT NULL
			GROUP BY ITM_CODE
			HAVING COUNT(*) > 1
		)
		BEGIN
			SELECT 'Packing Material Duplication'	AS VALIDATION_TEXT
			
			SELECT	ITM_CODE,COUNT(*)
			FROM	(SELECT	DISTINCT
							ITM_CODE			
							,ITM_NAME			
							,ITM_CATEGORY		
							,ITM_TYPE			
							,ITM_WEIGHT			
							,ITM_MIN_WEIGHT		
							,ITM_MAX_WEIGHT		
							,ITM_UOM			

							,[IPD_TYPE]			
							,[IPD_INNER_LENGTH]	
							,[IPD_INNER_BREADTH]
							,[IPD_INNER_HEIGHT]	
							,[IPD_OUTER_LENGTH]	
							,[IPD_OUTER_BREADTH]
							,[IPD_OUTER_HEIGHT]	
							,[IPD_PLY]			
							,[IPD_PAPER_COLOR]	
							,[IPD_ART_WORK]		
							--,[IPD_CUSTOMER]		
							,[IPD_THICKNESS]	
							,[IPD_PTYPE]
					FROM	@PM_INV_ITEM_MST)	AS	D
			WHERE	ITM_CODE IS NOT NULL
			GROUP BY ITM_CODE
			HAVING COUNT(*) > 1
			
			ROLLBACK TRAN
			RETURN
		END
				VAlidation in ssis*/
		--Update blank details to NULL
		UPDATE	Z_PM_INV_ITEM_MST
		SET		IPD_PLY				=	(CASE WHEN ISNULL(LTRIM(IPD_PLY),'') = '' THEN NULL ELSE IPD_PLY END)
				,IPD_PAPER_COLOR	=	(CASE WHEN ISNULL(LTRIM(IPD_PAPER_COLOR),'') = '' THEN NULL ELSE IPD_PAPER_COLOR END)
				,IPD_TYPE			=	(CASE WHEN ISNULL(LTRIM(IPD_TYPE),'') = '' THEN NULL ELSE IPD_TYPE END)
			
		SELECT	@V_ITM_PK	=	ISNULL(MAX(ITM_PK),0) FROM INV_ITEM_MST
		
		PRINT 'Packing Material Item'
		
		INSERT INTO INV_ITEM_MST
		(	 ITM_PK
			,ITM_CODE
			,ITM_NAME
			,ITM_DESC
			,ITM_CATEGORY
			,ITM_TYPE
			,ITM_WEIGHT
			,ITM_MIN_WEIGHT
			,ITM_MAX_WEIGHT
			,ITM_UOM
			,ITM_ACTIVE
			,ITM_BIZUNIT
			,ITM_CRTD_BY
			,ITM_CRTD_DT
			,ITM_MOD_BY
			,ITM_MOD_DT
		)
		SELECT	@V_ITM_PK	+	ROW_NUMBER() OVER(ORDER BY PM.ITM_CODE)
				,PM.ITM_CODE
				,PM.ITM_NAME
				--,STUFF(
				--	ISNULL(' - ' +LTRIM(CAST([PM].[IPD_THICKNESS]		AS NVARCHAR(100))),'')	+
				--	ISNULL(' - ' +LTRIM(CAST([PM].[IPD_PLY]			AS NVARCHAR(100))),'')	+
				--	ISNULL(' - ' +LTRIM(CAST([PM].[IPD_PAPER_COLOR]	AS NVARCHAR(100))),'')	/*+
				--	ISNULL(' - ' +LTRIM(CAST([PM].[IPD_TYPE]			AS NVARCHAR(100))),'')*/
				--	,1,3,'')
				,NULL
				,PM.ITM_CATEGORY	--12			--	PM-PACKING MATERIAL
				,PM.ITM_TYPE		--1
				,ISNULL(PM.ITM_WEIGHT,0)		--
				,PM.ITM_MIN_WEIGHT	--0
				,PM.ITM_MAX_WEIGHT	--0
				,PM.ITM_UOM		--8		--	Nos
				,1
				,1
				,1
				,GETDATE()
				,1
				,GETDATE()
		FROM	(
					SELECT	DISTINCT
							ITM_CODE			
							,ITM_NAME			
							,ITM_CATEGORY		
							,ITM_TYPE			
							,ITM_WEIGHT			
							,ITM_MIN_WEIGHT		
							,ITM_MAX_WEIGHT		
							,ITM_UOM			

							,[IPD_TYPE]			
							,[IPD_INNER_LENGTH]	
							,[IPD_INNER_BREADTH]
							,[IPD_INNER_HEIGHT]	
							,[IPD_OUTER_LENGTH]	
							,[IPD_OUTER_BREADTH]
							,[IPD_OUTER_HEIGHT]	
							,[IPD_PLY]			
							,[IPD_PAPER_COLOR]	
							,[IPD_ART_WORK]		
							--,[IPD_CUSTOMER]		
							,[IPD_THICKNESS]	
							,[IPD_PTYPE]
					FROM	Z_PM_INV_ITEM_MST
			)	AS	PM
		LEFT JOIN INV_ITEM_MST T ON T.ITM_CODE	= PM.ITM_CODE
		WHERE T.ITM_PK IS NULL
		ORDER BY PM.ITM_CODE
		
		--SELECT * FROM INV_ITEM_MST WHERE ITM_PK > @V_ITM_PK 
		
		PRINT 'Packing Material Packing Details'
		
		SELECT	@V_IPD_PK	=	ISNULL(MAX([IPD_PK]),0) FROM INV_ITEM_PACK_DTL

		INSERT INTO INV_ITEM_PACK_DTL
		(	 [IPD_PK]
			,[IPD_ITEM]
			,[IPD_TYPE]
			,[IPD_INNER_LENGTH]
			,[IPD_INNER_BREADTH]
			,[IPD_INNER_HEIGHT]
			,[IPD_OUTER_LENGTH]
			,[IPD_OUTER_BREADTH]
			,[IPD_OUTER_HEIGHT]
			,[IPD_PLY]
			,[IPD_PAPER_COLOR]
			,[IPD_PAPER_TYPE]
			,[IPD_ART_WORK]
			,[IPD_CUSTOMER]
			,[IPD_THICKNESS]
			,[IPD_ACTIVE]
			,[IPD_MOD_BY]
			,[IPD_MOD_DT]
		)
		SELECT	@V_IPD_PK	+	ROW_NUMBER() OVER(ORDER BY ITM_PK)
				,ITM_PK
				,T.IPD_TYPE
				,[T].[IPD_INNER_LENGTH]
				,[T].[IPD_INNER_BREADTH]
				,[T].[IPD_INNER_HEIGHT]
				,[T].[IPD_OUTER_LENGTH]
				,[T].[IPD_OUTER_BREADTH]
				,[T].[IPD_OUTER_HEIGHT]
				,[T].[IPD_PLY]
				,[T].[IPD_PAPER_COLOR]
				,[T].[IPD_PTYPE]
				,CAST((RIGHT(YEAR(GETDATE()),2)*100)+RIGHT(MONTH(GETDATE()),2) AS NVARCHAR(4))+'-1'	--[T].[IPD_ART_WORK]
				,NULL	--[T].[IPD_CUSTOMER]
				,ISNULL([T].[IPD_THICKNESS],'')
				--,[T].[IPD_THICKNESS]
				,1
				,1
				,GETDATE()
		FROM	INV_ITEM_MST	I
			INNER JOIN (
					SELECT	DISTINCT
							ITM_CODE			
							,ITM_NAME			
							,ITM_CATEGORY		
							,ITM_TYPE			
							,ITM_WEIGHT			
							,ITM_MIN_WEIGHT		
							,ITM_MAX_WEIGHT		
							,ITM_UOM			

							,[IPD_TYPE]			
							,[IPD_INNER_LENGTH]	
							,[IPD_INNER_BREADTH]
							,[IPD_INNER_HEIGHT]	
							,[IPD_OUTER_LENGTH]	
							,[IPD_OUTER_BREADTH]
							,[IPD_OUTER_HEIGHT]	
							,[IPD_PLY]			
							,[IPD_PAPER_COLOR]
							,[IPD_PTYPE]
							,[IPD_ART_WORK]		
							--,[IPD_CUSTOMER]		
							,[IPD_THICKNESS]	
					FROM	Z_PM_INV_ITEM_MST
			)	AS	T ON T.ITM_CODE = I.ITM_CODE
		WHERE	ITM_PK	>	@V_ITM_PK
		
		--	Update Customer in Packing Material
		PRINT 'Update Customer in Packing Material Details'
		
		UPDATE	IPD
		SET		IPD.IPD_CUSTOMER	=	(	SELECT	(CASE WHEN MAX(T.IPD_CUSTOMER) = MIN(T.IPD_CUSTOMER) 
													THEN MAX(T.IPD_CUSTOMER) ELSE NULL END )
											FROM	Z_PM_INV_ITEM_MST	T
											WHERE	T.ITM_CODE	=	I.ITM_CODE
										)
		FROM	INV_ITEM_PACK_DTL		AS	IPD
			INNER JOIN INV_ITEM_MST	I	ON	I.ITM_PK	=	IPD.IPD_ITEM
										AND	I.ITM_PK	>	@V_ITM_PK
		
		-- Insert Packing Material vendor Mapping
		PRINT 'Packing Material Vendor Mapping'
		SELECT	@V_ITV_PK	=	ISNULL(MAX(ITV_PK),0)	FROM	INV_ITEM_VENDOR_MAP
		INSERT INTO	INV_ITEM_VENDOR_MAP
		(
			 [ITV_PK]
			,[ITV_ITEM]
			,[ITV_VENDOR]
			,[ITV_NAME]
			,[ITV_PRICE]
			,[ITV_CURRENCY]
			,[ITV_MOQ]
			,[ITV_MOQ_UOM]
			,[ITV_TAX_PERC]
			,[ITV_DISC_PERC]
			,[ITV_LEAD_TIME]
			,[ITV_ACTIVE]
			,[ITV_BIZUNIT]
			,[ITV_MOD_BY]
			,[ITV_MOD_DT]
			,[ITV_SL_NO]
		)
		SELECT	 @V_ITV_PK	+	ROW_NUMBER() OVER(ORDER BY VEN_CODE,I.ITM_CODE)
				,I.ITM_PK
				,VEN_PK
				, ISNULL([IPD_VEN_MAT_CODE] + ' - ','') + [ITM_NAME]
				--,[IPD_VEN_MAT_CODE] + ISNULL(' - '+[ITM_NAME]+'','')
				,ISNULL(IPD_RATE,0.00)
				,VEN_CURRENCY		
				,0.00
				,I.ITM_UOM
				,0,0,0,1,1,1,GETDATE(),DENSE_RANK() OVER ( PARTITION BY VEN_CODE ORDER BY I.ITM_CODE)
		FROM	INV_ITEM_MST	AS	I
			INNER JOIN	(
					SELECT	DISTINCT
							[ITM_CODE]
							,IPD_VEN_CODE
							,[IPD_VEN_MAT_CODE]
							,[IPD_RATE]
					FROM	Z_PM_INV_ITEM_MST
				)	AS	TPM	ON	TPM.ITM_CODE	=	I.ITM_CODE
			INNER JOIN	PUR_VENDOR_MST				ON	VEN_CODE		=	TPM.IPD_VEN_CODE
			LEFT OUTER JOIN INV_ITEM_VENDOR_MAP		ON	ITV_ITEM		=	I.ITM_PK
													AND	ITV_VENDOR		=	VEN_PK
		WHERE	I.ITM_PK	>	@V_ITM_PK
			AND	ITV_PK	IS NULL	--	Mapping not exists
		
		PRINT 'Packing Material Vendor Mapping History'
		SELECT	@V_VIH_PK	=	ISNULL(MAX(VIH_PK),0)	FROM	INV_ITEM_VENDOR_HISTORY
		INSERT INTO INV_ITEM_VENDOR_HISTORY
		(
			[VIH_PK]
			,[VIH_VENDOR]
			,[VIH_ITEM]
			,[VIH_SL_NO]
			,[VIH_UOM]
			,[VIH_RATE]
			,[VIH_CURRENCY]
			,[VIH_EFCT_FROM]
			,[VIH_EFCT_TO]
			,[VIH_TAX_PERC]
			,[VIH_DISC_PERC]
			,[VIH_LEAD_TIME]
			,[VIH_LAST_ORDR_DATE]
			,[VIH_LAST_ORDR_QTY]
			,[VIH_LAST_ORDR_RATE]
			,[VIH_RATING]
			,[VIH_MOD_BY]
			,[VIH_MOD_DT]
		)
		SELECT	 @V_VIH_PK + ROW_NUMBER() OVER(ORDER BY ITV_PK)	AS	[VIH_PK]
				,ITV_VENDOR			AS	[VIH_VENDOR]
				,ITV_ITEM			AS	[VIH_ITEM]
				,1					AS	[VIH_SL_NO]
				,ITV_MOQ_UOM		AS	[VIH_UOM]
				,ITV_PRICE			AS	[VIH_RATE]
				,ITV_CURRENCY		AS	[VIH_CURRENCY]
				,ITV_MOD_DT			AS	[VIH_EFCT_FROM]
				,NULL				AS	[VIH_EFCT_TO]
				,ITV_TAX_PERC		AS	[VIH_TAX_PERC]
				,ITV_DISC_PERC		AS	[VIH_DISC_PERC]
				,ITV_LEAD_TIME		AS	[VIH_LEAD_TIME]
				,NULL				AS	[VIH_LAST_ORDR_DATE]
				,0					AS	[VIH_LAST_ORDR_QTY]
				,0					AS	[VIH_LAST_ORDR_RATE]
				,NULL				AS	[VIH_RATING]
				,ITV_MOD_BY			AS	[VIH_MOD_BY]
				,ITV_MOD_DT			AS	[VIH_MOD_DT]
		FROM	[INV_ITEM_VENDOR_MAP]
		WHERE	ITV_PK	>	@V_ITV_PK
		
		--Vendor Role
		PRINT 'Vendor Role Mapping'
		
		INSERT INTO PUR_VENDOR_ROLE_MAP
		SELECT	DISTINCT ITV_VENDOR,9,1
		FROM	INV_ITEM_VENDOR_MAP
		LEFT OUTER JOIN PUR_VENDOR_ROLE_MAP	ON	VRM_VENDOR	=	ITV_VENDOR
											AND	VRM_ROLE	=	9
		WHERE	ITV_PK	>	@V_ITV_PK
			AND	VRM_PK	IS NULL
			
		--Stock Header
		PRINT 'Stock Header'
		INSERT INTO INV_STK_HDR
		(
			[STH_ITEM]
			,[STH_QTY_IN_STOCK]
			,[STH_QTY_PN_ORDER]
			,[STH_QTY_PN_RECEIPT]
			,[STH_QTY_PN_INSPECTION]
			,[STH_QTY_PN_ISSUE]
			,[STH_QTY_RESERVED]
			,[STH_QTY_DAMAGED]
			,[STH_UOM]
			,[STH_RATE]
			,[STH_VALUE]
			,[STH_BIZUNIT]
			,[STH_MOD_BY]
			,[STH_MOD_DT]
		)
		SELECT	ITM_PK		AS	[STH_ITEM]
				,0			AS	[STH_QTY_IN_STOCK]
				,0			AS	[STH_QTY_PN_ORDER]
				,0			AS	[STH_QTY_PN_RECEIPT]
				,0			AS	[STH_QTY_PN_INSPECTION]
				,0			AS	[STH_QTY_PN_ISSUE]
				,0			AS	[STH_QTY_RESERVED]
				,0			AS	[STH_QTY_DAMAGED]
				,ITM_UOM	AS	[STH_UOM]
				,0			AS	[STH_RATE]
				,0			AS	[STH_VALUE]
				,1			AS	[STH_BIZUNIT]
				,2			AS	[STH_MOD_BY]
				,GETDATE()	AS	[STH_MOD_DT]
		FROM	INV_ITEM_MST
		WHERE	ITM_CATEGORY in (61,62,12)
			AND	ITM_PK	>	@V_ITM_PK
			AND	ITM_PK	NOT IN (SELECT [STH_ITEM] FROM INV_STK_HDR)

		--Stock Detail	
		PRINT 'Stock Detail'
			
		INSERT INTO INV_STK_DTL 
		(	
			[STD_ITEM]
			,[STD_DEPT]
			,[STD_QTY_IN_STOCK]
			,[STD_QTY_PN_ORDER]
			,[STD_QTY_PN_RECEIPT]
			,[STD_QTY_PN_INSPECTION]
			,[STD_QTY_PN_ISSUE]
			,[STD_QTY_PN_TRANSIT]
			,[STD_QTY_RESERVED]
			,[STD_QTY_DAMAGED]
			,[STD_UOM]
			,[STD_BIZUNIT]
			,[STD_MOD_BY]
			,[STD_MOD_DT]
		)
		SELECT	ITM_PK		AS	[STD_ITEM]
				,@V_DEPT 	AS	[STD_DEPT]
				,0 			AS	[STD_QTY_IN_STOCK]
				,0 			AS	[STD_QTY_PN_ORDER]
				,0 			AS	[STD_QTY_PN_RECEIPT]
				,0 			AS	[STD_QTY_PN_INSPECTION]
				,0 			AS	[STD_QTY_PN_ISSUE]
				,0 			AS	[STD_QTY_PN_TRANSIT]
				,0 			AS	[STD_QTY_RESERVED]
				,0 			AS	[STD_QTY_DAMAGED]
				,ITM_UOM	AS	[STD_UOM]
				,1 			AS	[STD_BIZUNIT]
				,1			AS	[STD_MOD_BY]
				,GETDATE() 	AS	[STD_MOD_DT]
		FROM	INV_ITEM_MST
		WHERE	ITM_CATEGORY IN (61,62,12) 
			AND	ITM_PK	>	@V_ITM_PK
			AND	ITM_PK	NOT IN (SELECT [STD_ITEM] FROM INV_STK_DTL WHERE [STD_DEPT] = @V_DEPT)
			
		--Department Mapping	
		PRINT 'Stock - Department Mapping'
		INSERT INTO INV_ITEM_DEPT_MAP
		(
			[IDM_ITEM]
			,[IDM_DEPT]
			,[IDM_ACTIVE]
			,[IDM_BIZUNIT]
			,[IDM_MOD_BY]
			,[IDM_MOD_DT]
		)
		SELECT	ITM_PK		AS	[IDM_ITEM]
				,@V_DEPT 	AS	[IDM_DEPT]
				,1			AS	[IDM_ACTIVE]
				,1 			AS	[IDM_BIZUNIT]
				,1			AS	[IDM_MOD_BY]
				,GETDATE()	AS	[IDM_MOD_DT]
		FROM	INV_ITEM_MST
		WHERE	ITM_CATEGORY IN (61,62,12)
			AND	ITM_PK	>	@V_ITM_PK
			AND	ITM_PK	NOT IN (SELECT [IDM_ITEM] FROM INV_ITEM_DEPT_MAP WHERE [IDM_DEPT] = @V_DEPT)
		
		--Change Packing Spec Qtys Properly
		/*
		PRINT 'Change Packing Spec Qtys Properly'
		UPDATE	Z_BWH_BRANDS
		SET		MC_QTY		=	CASE WHEN MC_QTY > 0 THEN
									COALESCE(	ZB_QTY		--Zipper Bag 
												,OR_QTY		--Printed Outer Bag 
												,OP_QTY		--Plain Outer Bag 
												,IB_QTY		--Inner Box /Bag
												,PC_QTY		--Pouch (Printed / Plain)
												,WL_QTY		--Wallet	
												,PACK
											)
								ELSE MC_QTY END
				,ZB_QTY	=		CASE WHEN ZB_QTY > 0 THEN
									COALESCE(	OR_QTY		--Printed Outer Bag 
												,OP_QTY		--Plain Outer Bag 
												,IB_QTY		--Inner Box /Bag
												,PC_QTY		--Pouch (Printed / Plain)
												,WL_QTY		--Wallet
												,PACK	
											)
								ELSE ZB_QTY END
				,OR_QTY	=		CASE WHEN OR_QTY > 0 THEN
									COALESCE(	OP_QTY		--Plain Outer Bag 
												,IB_QTY		--Inner Box /Bag
												,PC_QTY		--Pouch (Printed / Plain)
												,WL_QTY		--Wallet
												,PACK	
											)
								ELSE OR_QTY END
				,OP_QTY	=		CASE WHEN OP_QTY > 0 THEN
									COALESCE(	IB_QTY		--Inner Box /Bag
												,PC_QTY		--Pouch (Printed / Plain)
												,WL_QTY		--Wallet	
												,PACK
											)
								ELSE OP_QTY END
				,IB_QTY	=		CASE WHEN IB_QTY > 0 THEN
									COALESCE(	PC_QTY		--Pouch (Printed / Plain)
												,WL_QTY		--Wallet
												,PACK	
											)
								ELSE IB_QTY END
				,PC_QTY	=		CASE WHEN PC_QTY > 0 THEN
									COALESCE(	WL_QTY		--Wallet	
												,PACK
											)
								ELSE PC_QTY END
				,WL_QTY	=		CASE WHEN WL_QTY > 0 THEN
									COALESCE(	PACK
												,NULL
											)
								ELSE WL_QTY END
		*/
							
		SELECT	@V_CON_PK	=	ISNULL(MAX(CON_PK),0) FROM ADM_CONST_MST 

		--Insert New Packing Spec Types
		PRINT 'Insert New Packing Spec Types'
		INSERT INTO ADM_CONST_MST
		(
			 [CON_PK]
			,[CON_GROUP]
			,[CON_NAME]
			,[CON_VALUE]
			,[CON_CODE]
			,[CON_DESC]
			,[CON_DEFAULT]
			,[CON_ACTIVE]
			,[CON_BIZUNIT]
			,[CON_MOD_BY]
			,[CON_MOD_DT]
		)
		SELECT	 @V_CON_PK + ROW_NUMBER() OVER(ORDER BY MIN(CARTON))
				,79
				,STUFF(  ISNULL(' - '+WLT_CON_NAME,'') + ISNULL(' - '+PC_CON_NAME,'') + ISNULL(' - '+IB_CON_NAME,'') +
					ISNULL(' - '+POB_CON_NAME,'') + ISNULL(' - '+PRB_CON_NAME,'') + ISNULL(' - '+ZB_CON_NAME,'') +
					ISNULL(' - '+MC_CON_NAME,'') ,1,3,'')
				,(SELECT ISNULL(MAX(CON_VALUE),0) FROM ADM_CONST_MST WHERE CON_GROUP = 79) + ROW_NUMBER() OVER(ORDER BY MIN(CARTON)) --Value
				,STUFF(  ISNULL(' - '+WLT_CON_CODE,'') + ISNULL(' - '+PC_CON_CODE,'') + ISNULL(' - '+IB_CON_CODE,'') +
					ISNULL(' - '+POB_CON_CODE,'') + ISNULL(' - '+PRB_CON_CODE,'') + ISNULL(' - '+ZB_CON_CODE,'') +
					ISNULL(' - '+MC_CON_CODE,'') ,1,3,'')
				,STUFF(  ISNULL(' - '+WLT_CON_NAME,'') + ISNULL(' - '+PC_CON_NAME,'') + ISNULL(' - '+IB_CON_NAME,'') +
					ISNULL(' - '+POB_CON_NAME,'') + ISNULL(' - '+PRB_CON_NAME,'') + ISNULL(' - '+ZB_CON_NAME,'') +
					ISNULL(' - '+MC_CON_NAME,'') ,1,3,'')
				,1
				,1
				,1
				,1
				,GETDATE()
		FROM	Z_BWH_BRANDS_VW
		WHERE	STUFF(  ISNULL(' - '+WLT_CON_CODE,'') + ISNULL(' - '+PC_CON_CODE,'') + ISNULL(' - '+IB_CON_CODE,'') +
					ISNULL(' - '+POB_CON_CODE,'') + ISNULL(' - '+PRB_CON_CODE,'') + ISNULL(' - '+ZB_CON_CODE,'') +
					ISNULL(' - '+MC_CON_CODE,'') ,1,3,'')
				NOT IN (SELECT CON_CODE FROM ADM_CONST_MST WHERE CON_GROUP = 79)
		GROUP BY STUFF(  ISNULL(' - '+WLT_CON_NAME,'') + ISNULL(' - '+PC_CON_NAME,'') + ISNULL(' - '+IB_CON_NAME,'') +
					ISNULL(' - '+POB_CON_NAME,'') + ISNULL(' - '+PRB_CON_NAME,'') + ISNULL(' - '+ZB_CON_NAME,'') +
					ISNULL(' - '+MC_CON_NAME,'') ,1,3,'')
				,STUFF(  ISNULL(' - '+WLT_CON_CODE,'') + ISNULL(' - '+PC_CON_CODE,'') + ISNULL(' - '+IB_CON_CODE,'') +
					ISNULL(' - '+POB_CON_CODE,'') + ISNULL(' - '+PRB_CON_CODE,'') + ISNULL(' - '+ZB_CON_CODE,'') +
					ISNULL(' - '+MC_CON_CODE,'') ,1,3,'')					

		--Packing Spec Master
		PRINT 'Packing Spec Master'
		SELECT	@V_APS_PK	=	ISNULL(MAX(APS_PK),0) FROM ADM_PACK_SPEC_MST
		
		INSERT INTO ADM_PACK_SPEC_MST
		(
			 [APS_PK]
			,[APS_CODE]
			,[APS_NAME]
			,[APS_TYPE]
			,[APS_WLT_PCS]
			,[APS_PC_PCS]
			,[APS_IB_PCS]
			--,[APS_IC_PCS]
			,[APS_POB_PCS]
			,[APS_PRB_PCS]
			,[APS_ZB_PCS]
			,[APS_MC_PCS]
			--,[APS_SC_PCS]
			,[APS_TOTAL_PCS]
			,[APS_DESC]
			,[APS_ACTIVE]
			,[APS_BIZUNIT]
			,[APS_CRTD_BY]
			,[APS_CRTD_DT]
			,[APS_MOD_BY]
			,[APS_MOD_DT]
		)
		SELECT	@V_APS_PK	+	ROW_NUMBER() OVER(ORDER BY PST.CON_PK)
				,'P'+CAST(CARTON AS NVARCHAR(10))+ ' : ' +
				STUFF(  ISNULL(' - '+WLT_CON_CODE,'') + ISNULL(' - '+PC_CON_CODE,'') + ISNULL(' - '+IB_CON_CODE,'') +
						ISNULL(' - '+POB_CON_CODE,'') + ISNULL(' - '+PRB_CON_CODE,'') + ISNULL(' - '+ZB_CON_CODE,'') +
						ISNULL(' - '+MC_CON_CODE,'') ,1,3,'') + ' ('+
				STUFF(  ISNULL(' x '+CAST(WL_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(PC_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(IB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(OP_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(OR_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(ZB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(MC_QTY AS NVARCHAR(10)),'') ,1,3,'') + ')'	AS	APS_CODE
				,STUFF( ISNULL(' - '+WLT_CON_NAME,'') + ISNULL(' - '+PC_CON_NAME,'') + ISNULL(' - '+IB_CON_NAME,'') +
						ISNULL(' - '+POB_CON_NAME,'') + ISNULL(' - '+PRB_CON_NAME,'') + ISNULL(' - '+ZB_CON_NAME,'') +
						ISNULL(' - '+MC_CON_NAME,'') ,1,3,'') + ' ('+
				STUFF(  ISNULL(' x '+CAST(WL_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(PC_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(IB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(OP_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(OR_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(ZB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(MC_QTY AS NVARCHAR(10)),'') ,1,3,'') + ') -' +CAST(CARTON AS NVARCHAR(10))	AS	APS_NAME
				,PST.CON_PK
		
				,ISNULL(WL_QTY,0)
				,ISNULL(PC_QTY,0)
				,ISNULL(IB_QTY,0)
				,ISNULL(OP_QTY,0)
				,ISNULL(OR_QTY,0)
				,ISNULL(ZB_QTY,0)
				,ISNULL(MC_QTY,0)
				,MAX(CARTON)
				
				,PST.[CON_NAME]	+ '('+CAST(MAX(CARTON) AS NVARCHAR(20))+')'
				,1
				,1
				,1
				,GETDATE()
				,1
				,GETDATE()
		FROM	Z_BWH_BRANDS_VW

			INNER JOIN ADM_CONST_MST	PST ON	PST.CON_GROUP	=	79
											AND	PST.CON_CODE	=	STUFF(  ISNULL(' - '+WLT_CON_CODE,'') + ISNULL(' - '+PC_CON_CODE,'') + ISNULL(' - '+IB_CON_CODE,'') +
																	ISNULL(' - '+POB_CON_CODE,'') + ISNULL(' - '+PRB_CON_CODE,'') + ISNULL(' - '+ZB_CON_CODE,'') +
																	ISNULL(' - '+MC_CON_CODE,'') ,1,3,'')
			LEFT OUTER JOIN	ADM_PACK_SPEC_MST	ON	APS_NAME	=	STUFF(  ISNULL(' - '+WLT_CON_NAME,'') + ISNULL(' - '+PC_CON_NAME,'') + ISNULL(' - '+IB_CON_NAME,'') +
						ISNULL(' - '+POB_CON_NAME,'') + ISNULL(' - '+PRB_CON_NAME,'') + ISNULL(' - '+ZB_CON_NAME,'') +
						ISNULL(' - '+MC_CON_NAME,'') ,1,3,'') + ' ('+
				STUFF(  ISNULL(' x '+CAST(WL_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(PC_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(IB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(OP_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(OR_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(ZB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(MC_QTY AS NVARCHAR(10)),'') ,1,3,'') + ') -' +CAST(CARTON AS NVARCHAR(10))
				OR	APS_CODE	=	'P'+CAST(CARTON AS NVARCHAR(10))+ ' : ' +
				STUFF(  ISNULL(' - '+WLT_CON_CODE,'') + ISNULL(' - '+PC_CON_CODE,'') + ISNULL(' - '+IB_CON_CODE,'') +
						ISNULL(' - '+POB_CON_CODE,'') + ISNULL(' - '+PRB_CON_CODE,'') + ISNULL(' - '+ZB_CON_CODE,'') +
						ISNULL(' - '+MC_CON_CODE,'') ,1,3,'') + ' ('+
				STUFF(  ISNULL(' x '+CAST(WL_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(PC_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(IB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(OP_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(OR_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(ZB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(MC_QTY AS NVARCHAR(10)),'') ,1,3,'') + ')'
		WHERE	APS_PK	IS NULL
		GROUP BY PST.CON_PK
				,PST.[CON_NAME]
				,WL_QTY
				,PC_QTY
				,IB_QTY
				,OP_QTY
				,OR_QTY
				,ZB_QTY
				,MC_QTY
				,'P'+CAST(CARTON AS NVARCHAR(10))+ ' : ' +
				STUFF(  ISNULL(' - '+WLT_CON_CODE,'') + ISNULL(' - '+PC_CON_CODE,'') + ISNULL(' - '+IB_CON_CODE,'') +
						ISNULL(' - '+POB_CON_CODE,'') + ISNULL(' - '+PRB_CON_CODE,'') + ISNULL(' - '+ZB_CON_CODE,'') +
						ISNULL(' - '+MC_CON_CODE,'') ,1,3,'') + ' ('+
				STUFF(  ISNULL(' x '+CAST(WL_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(PC_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(IB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(OP_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(OR_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(ZB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(MC_QTY AS NVARCHAR(10)),'') ,1,3,'') + ')'
				,STUFF(  ISNULL(' - '+WLT_CON_NAME,'') + ISNULL(' - '+PC_CON_NAME,'') + ISNULL(' - '+IB_CON_NAME,'') +
						ISNULL(' - '+POB_CON_NAME,'') + ISNULL(' - '+PRB_CON_NAME,'') + ISNULL(' - '+ZB_CON_NAME,'') +
						ISNULL(' - '+MC_CON_NAME,'') ,1,3,'') + ' ('+
				STUFF(  ISNULL(' x '+CAST(WL_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(PC_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(IB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(OP_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(OR_QTY AS NVARCHAR(10)),'') + ISNULL(' x '+CAST(ZB_QTY AS NVARCHAR(10)),'') +
						ISNULL(' x '+CAST(MC_QTY AS NVARCHAR(10)),'') ,1,3,'') + ') -' +CAST(CARTON AS NVARCHAR(10))
			
		--Insert Brands
		PRINT 'Customer Brands'
		
		SELECT	@V_CIM_PK	=	ISNULL(MAX(CIM_PK),0)	FROM CRM_CUST_ITEM_MAP 
		
		INSERT INTO CRM_CUST_ITEM_MAP 
		(	 [CIM_CUSTOMER]
			,[CIM_BRAND_CODE]
			,[CIM_BRAND_NAME]
			,[CIM_DESC]
			,[CIM_ITEM]
			,[CIM_NATURE]
			,[CIM_THICKNESS]
			,[CIM_PROCESS]
			,[CIM_SURFACE]
			,[CIM_COLOUR]
			,[CIM_GRADE]
			,[CIM_SIZE]
			,[CIM_LENGTH]
			,[CIM_CHLORINATION]
			,[CIM_STERILE]
			,[CIM_CLEAN_ROOM]
			,[CIM_ADNL_SPEC01]
			,[CIM_ADNL_SPEC02]
			,[CIM_ADNL_SPEC03]
			,[CIM_ADNL_SPEC04]
			,[CIM_ADNL_SPEC05]
			,[CIM_ADNL_SPEC06]
			,[CIM_ADNL_SPEC07]
			,[CIM_ADNL_SPEC08]
			,[CIM_ADNL_SPEC09]
			,[CIM_PACKING_SPEC]
			,[CIM_PACKING_TYPE]
			--,[CIM_PC_PCS]
			--,[CIM_IB_PCS]
			--,[CIM_IC_PCS]
			--,[CIM_ZB_PCS]
			--,[CIM_MC_PCS]
			--,[CIM_SC_PCS]
			,[CIM_TOTAL_PCS]
			,[CIM_RATE]
			,[CIM_CURRENCY]
			--,[CIM_CURRENCY_RATE]
			--,[CIM_QTY]
			,[CIM_UOM]
			,[CIM_STATUS]
			,[CIM_ACTIVE]
			,[CIM_MOD_BY]
			,[CIM_MOD_DT]
			,[CIM_SALE_UOM]
			,[CIM_SALE_UOM_CONV]
			,[CIM_BRAND_SIZE]
			,[CIM_PACKING_SPEC_NAME]
			,[CIM_QC_SPEC_NAME]
			
		)
		SELECT	 CUS_PK
				,BRANDCODE
				,BRANDNAME
				,BRANDDESC --PRDDESC
				,ITM_PK
				,[ISD_NATURE]
				,[ISD_THICKNESS]
				,[ISD_PROCESS]
				,[ISD_SURFACE]
				,[ISD_COLOUR]
				,[ISD_GRADE]
				,[ISD_SIZE]
				,[ISD_LENGTH]
				,[ISD_CHLORINATION]
				,[ISD_STERILE]
				,[ISD_CLEAN_ROOM]
				,[ISD_ADNL_SPEC01]
				,[ISD_ADNL_SPEC02]
				,[ISD_ADNL_SPEC03]
				,[ISD_ADNL_SPEC04]
				,[ISD_ADNL_SPEC05]
				,[ISD_ADNL_SPEC06]
				,[ISD_ADNL_SPEC07]
				,[ISD_ADNL_SPEC08]
				,[ISD_ADNL_SPEC09]
				,[PM_APS_PK]
				,[PM_APS_TYPE]
				,[PM_APS_TOTAL_PCS]
				,[RATE]
				,ISNULL((	SELECT	CUR_PK
							FROM	ADM_CURRENCY_MST
							WHERE	CUR_CODE	=	CURR),@V_BASE_CURR)
				,[ITM_UOM]
				,0
				,1
				,1
				,GETDATE()
				,(SELECT [CFG_PK] 
					FROM [ADM_CONFIG_MST] 
					WHERE [CFG_DATA] = SALE_UOM)
				,ISNULL(NULLIF(SALE_UOM_CONV,0),1)
				,[ISD_SIZE]
				,[CIM_PACKING_SPEC_NAME]
				,[CIM_QC_SPEC_NAME]
		FROM	Z_BWH_BRANDS_VW
			INNER JOIN	CRM_CUSTOMER_MST		ON	CUS_CODE	=	CUSCODE
			INNER JOIN	INV_ITEM_MST			ON	ITM_CODE	=	PRDCODE
			LEFT OUTER JOIN	INV_ITEM_SPEC_DTL	ON	ISD_ITEM	=	ITM_PK
		--SELECT * FROM CRM_CUST_ITEM_MAP
		PRINT 'PACKING SPEC AND BRAND MAPPING'
		SELECT	@V_PIM_PK	=	ISNULL(MAX(PIM_PK),0) FROM ADM_PACK_CUST_ITEM_MAP 
		
		INSERT INTO ADM_PACK_CUST_ITEM_MAP
			(
				 [PIM_PK]
				,[PIM_PACK_SPEC]
				,[PIM_CUSTOMER]
				,[PIM_CUST_ITEM]
				,[PIM_ART_WORK]
				,[PIM_DESC]
				
				,[PIM_WLT_ITEM]
				,[PIM_PC_ITEM]
				,[PIM_IB_ITEM]
				,[PIM_IC_ITEM]
				,[PIM_POB_ITEM]
				,[PIM_PRB_ITEM]
				,[PIM_ZB_ITEM]
				,[PIM_MC_ITEM]
				,[PIM_SC_ITEM]

				,[PIM_ACTIVE]
				,[PIM_BIZUNIT]
				,[PIM_CRTD_BY]
				,[PIM_CRTD_DT]
				,[PIM_MOD_BY]
				,[PIM_MOD_DT]
			)
		SELECT	@V_PIM_PK + ROW_NUMBER() OVER(ORDER BY ROWNO)
				,PM_APS_PK
				,CUS_PK
				,CIM_PK
				,CAST((RIGHT(YEAR(GETDATE()),2)*100)+RIGHT(MONTH(GETDATE()),2) AS NVARCHAR(4))+'-1'
				,BRANDNAME
				
				,(	SELECT	ITM_PK
					FROM	INV_ITEM_MST
					WHERE	ITM_CODE	=	WL_PMC)	--PIM_WLT_ITEM
				,(	SELECT	ITM_PK
					FROM	INV_ITEM_MST
					WHERE	ITM_CODE	=	PC_PMC)	--PIM_PC_ITEM
				,(	SELECT	ITM_PK
					FROM	INV_ITEM_MST
					WHERE	ITM_CODE	=	IB_PMC)	--PIM_IB_ITEM
				,NULL								--PIM_IC_ITEM
				,(	SELECT	ITM_PK
					FROM	INV_ITEM_MST
					WHERE	ITM_CODE	=	OP_PMC)	--PIM_POB_ITEM
				,(	SELECT	ITM_PK
					FROM	INV_ITEM_MST
					WHERE	ITM_CODE	=	OR_PMC)	--PIM_PRB_ITEM	
				,(	SELECT	ITM_PK
					FROM	INV_ITEM_MST
					WHERE	ITM_CODE	=	ZB_PMC)	--PIM_ZB_ITEM	
				,(	SELECT	ITM_PK
					FROM	INV_ITEM_MST
					WHERE	ITM_CODE	=	MC_PMC)	--PIM_MC_ITEM		
				,NULL								--PIM_SC_ITEM
				
				,1
				,1
				,1
				,GETDATE()
				,1
				,GETDATE()
		FROM	Z_BWH_BRANDS_VW
			INNER JOIN	CRM_CUSTOMER_MST	ON	CUS_CODE		=	CUSCODE
			INNER JOIN	CRM_CUST_ITEM_MAP	ON	CIM_BRAND_NAME	=	BRANDNAME
											AND	ISNULL(CIM_BRAND_CODE,'')	=	ISNULL(BRANDCODE,'')
											AND	CIM_CUSTOMER	=	CUS_PK  
		WHERE	CIM_PK	>	@V_CIM_PK
					
		SELECT	@V_BMD_PK =	ISNULL(MAX(BMD_PK),0) FROM ADM_PACK_CUST_ITEM_MAP_DTL

		PRINT 'PACKING SPEC AND BRAND MAPPING DETAILS -- (NON STOCK)'
		
		INSERT INTO	[ADM_PACK_CUST_ITEM_MAP_DTL]
		(	 [BMD_PK]
			,[BMD_ART_WORK]
			,[BMD_TYPE]
			,[BMD_ITEM]
			,[BMD_QTY]
			,[BMD_SEQUENCE]
			,[BMD_ACTIVE]
		)
		SELECT	@V_BMD_PK + ROW_NUMBER() OVER (ORDER BY  TART_WORK)
				,TART_WORK
				,(	SELECT	CON_PK 
					FROM	ADM_CONST_MST
					WHERE	CON_GROUP = 83 
						AND CON_VALUE = TTYPE_VAL)
				,ITM_PK
				,TQTY
				,TSEQUENCE
				,1
		FROM	(
			--IC_PMC
			SELECT	DISTINCT 1						AS	TSEQUENCE
					,PM_PIM_PK						AS	TART_WORK
					,IC_PMC							AS	TITEM
					,IC_QTY							AS	TQTY
					,101							AS	TTYPE_VAL
			FROM	Z_BWH_BRANDS_VW
			--IN_PMC
			UNION ALL
			SELECT	DISTINCT 2
					,PM_PIM_PK
					,IN_PMC
					,IN_QTY
					,102
			FROM	Z_BWH_BRANDS_VW
			--LC_PMC
			UNION ALL
			SELECT	DISTINCT 3
					,PM_PIM_PK
					,LC_PMC
					,LC_QTY
					,103
			FROM	Z_BWH_BRANDS_VW
			--IP_PMC
			UNION ALL
			SELECT	DISTINCT 4
					,PM_PIM_PK
					,IP_PMC
					,IP_QTY
					,104
			FROM	Z_BWH_BRANDS_VW
			--RS_PMC
			UNION ALL
			SELECT	DISTINCT 5
					,PM_PIM_PK
					,RS_PMC
					,RS_QTY
					,105
			FROM	Z_BWH_BRANDS_VW
			--SS_PMC
			UNION ALL
			SELECT	DISTINCT 6
					,PM_PIM_PK
					,SS_PMC
					,SS_QTY
					,106
			FROM	Z_BWH_BRANDS_VW
			--BI_PMC
			UNION ALL
			SELECT	DISTINCT 7
					,PM_PIM_PK
					,BI_PMC
					,BI_QTY
					,107
			FROM	Z_BWH_BRANDS_VW
			--BO_PMC
			UNION ALL
			SELECT	DISTINCT 8
					,PM_PIM_PK
					,BO_PMC
					,BO_QTY
					,108
			FROM	Z_BWH_BRANDS_VW
			--IL_PMC
			UNION ALL
			SELECT	DISTINCT 9
					,PM_PIM_PK
					,IL_PMC
					,IL_QTY
					,109
			FROM	Z_BWH_BRANDS_VW
			--LL_PMC
			UNION ALL
			SELECT	DISTINCT 10
					,PM_PIM_PK
					,LL_PMC
					,LL_QTY
					,110
			FROM	Z_BWH_BRANDS_VW
			--SB_PMC
			UNION ALL
			SELECT	DISTINCT 11
					,PM_PIM_PK
					,SB_PMC
					,SB_QTY
					,111
			FROM	Z_BWH_BRANDS_VW
			--PL_PMC
			UNION ALL
			SELECT	DISTINCT 12
					,PM_PIM_PK
					,PL_PMC
					,PL_QTY
					,112
			FROM	Z_BWH_BRANDS_VW
		)	D
		INNER JOIN	INV_ITEM_MST	ON	ITM_CODE	=	TITEM
		--LEFT OUTER JOIN [ADM_PACK_CUST_ITEM_MAP_DTL]	ON	BMD_ITEM	= ITM_PK
		--												AND	BMD_ART_WORK= TART_WORK
		WHERE	/*TITEM	IS NOT NULL
			AND*/	ISNULL(TQTY,0)	>	0
			--AND		BMD_PK IS NULL
		ORDER BY TART_WORK
				,D.TSEQUENCE
			
		PRINT 'BRAND RATE'
		DECLARE	@V_BRH_PK		INT
				,@V_BRD_PK		INT
				,@V_BRH_MIN_PK	INT
				,@MY_DATE		DATE	=	GETDATE()
					
		SELECT	@V_BRH_PK		=	ISNULL(MAX(BRH_PK),0)  
				,@V_BRH_MIN_PK	=	ISNULL(MIN(BRH_PK),0)  
		FROM	CRM_CUST_ITEM_RATE_HDR
		
		SELECT	@V_BRD_PK		=	ISNULL(MAX(BRD_PK),0)	FROM CRM_CUST_ITEM_RATE_DTL
		
		IF ISNULL(@V_BRH_MIN_PK,0) = 0	--New entry
		BEGIN
			INSERT INTO CRM_CUST_ITEM_RATE_HDR
			(	 [BRH_PK]
				,[BRH_DATE_FROM]
				,[BRH_DATE_TO]
				,[BRH_DESC]
				,[BRH_STATUS]
				,[BRH_ACTIVE]
				,[BRH_BIZUNIT]
				,[BRH_CRTD_BY]
				,[BRH_CRTD_DT]
				,[BRH_MOD_BY]
				,[BRH_MOD_DT]
				
			)
			SELECT	1	--New entry -First Header PK
				,DATEADD(dd,-(DAY(@MY_DATE)-1),@MY_DATE)
				,DATEADD(dd,-(DAY(DATEADD(mm,1,@MY_DATE))),DATEADD(mm,1,@MY_DATE))
				,NULL
				,0
				,1
				,1
				,1
				,GETDATE()
				,1
				,GETDATE()
				
			INSERT INTO CRM_CUST_ITEM_RATE_DTL
			(	 [BRD_PK]
				,[BRD_RATE_HDR]
				,[BRD_CUST_ITEM]
				,[BRD_CURRENCY]
				,[BRD_RATE]
				,[BRD_ACTIVE]
				,[BRD_MOD_BY]
				,[BRD_MOD_DT]
			)
			SELECT	ROW_NUMBER() OVER(ORDER BY CIM_PK)
					,1
					,CIM_PK
					,CIM_CURRENCY
					,CIM_RATE
					,1
					,1
					,GETDATE()
			FROM	CRM_CUST_ITEM_MAP
			WHERE	CIM_PK	>	@V_CIM_PK
				AND	ISNULL(CIM_RATE,0)	>	0.00
			
		END
		ELSE
		BEGIN
		
			INSERT INTO CRM_CUST_ITEM_RATE_DTL
			(	 [BRD_PK]
				,[BRD_RATE_HDR]
				,[BRD_CUST_ITEM]
				,[BRD_CURRENCY]
				,[BRD_RATE]
				,[BRD_ACTIVE]
				,[BRD_MOD_BY]
				,[BRD_MOD_DT]
			)
			SELECT	@V_BRD_PK + ROW_NUMBER() OVER(ORDER BY CIM_PK)
					,BRH_PK
					,CIM_PK
					,CIM_CURRENCY
					,CIM_RATE
					,1
					,1
					,GETDATE()
			FROM	CRM_CUST_ITEM_MAP
				INNER JOIN CRM_CUST_ITEM_RATE_HDR ON BRH_PK BETWEEN @V_BRH_MIN_PK AND @V_BRH_PK
			WHERE	CIM_PK	>	@V_CIM_PK
				AND	ISNULL(CIM_RATE,0)	>	0.00
			ORDER BY BRH_PK
					,CIM_PK
		
		END
		
		--set uom to PCS 
		UPDATE INV_ITEM_MST 
		SET ITM_UOM = 80 
		WHERE ITM_CATEGORY = 12 AND ITM_UOM <> 80
					
		SELECT	'Completed !!! (?)'
		--select * from INV_ITEM_VENDOR_MAP
		--select * from inv_item_mst
	COMMIT
END TRY
BEGIN CATCH
	SELECT	ERROR_MESSAGE()	AS ERROR_TEXT
	RAISERROR ('Error', 16, 1)
	ROLLBACK TRAN
END CATCH

END






