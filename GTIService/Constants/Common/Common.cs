using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Common
{
    public class ResponseTypes
    {
        public const string JSON = "application/json; charset=utf-8";
        public const string TEXT = "text";
        public const string XML = "text/xml; charset=utf-8";
        public const string HTML = "text/html";
    }

    public class FileUpload
    {
        public const string UPLOADURL = "Upload\\";
        public const string TEMPFOLDER = "TempFolder";
    }

    public class CommonConstant
    {
        public const string SelectOne = "--Select--";
        public const int UserGroupFlag = 1;
        public const string PONUMBERFORMAT = "PO-#ddMMyyyy#-#N#";
        public const string SRSNUMBERFORMAT = "SRS-#ddMMyyyy#-#N#";
        public const string SANUMBERFORMAT = "SA-#ddMMyyyy#-#N#";
        public const string PRNUMBERFORMAT = "PR-#ddMMyyyy#-#N#";
        public const string GRNNUMBERFORMAT = "GRN-#ddMMyyyy#-#N#";
        public const string MINNUMBERFORMAT = "MI-#ddMMyyyy#-#N#";
        public const string GINNUMBERFORMAT = "GIN-#ddMMyyyy#-#N#";
        public const string MANUMBERFORMAT = "MA-#ddMMyyyy#-#N#";
        public const string DISPNUMBERFORMAT = "DISP-#ddMMyyyy#-#N#";
        public const string NIRNUMBERFORMAT = "NIR-#ddMMyyyy#-#N#";
        public const string STOCKTRANSFERNUMBERFORMAT = "ST-#ddMMyyyy#-#N#";
        public const string VendorRegistrationProcessID = "1";
        public const string VendorEvaluationProcessID = "2";
        public const string PurchaseOrderProcessID = "3";
        public const string StoreRequisitionProcessID = "4";
        public const string PurchaseRequestProcessID = "5";
        public const string GoodsReceiptNoteProcessID = "7";
        public const string DispersionPreparationProcessID = "10";
        public const string CompoundTrxProcessID = "9";
        public const string STOREAUDIT = "8";
        public const string STOCKADJUSTMENT = "12";

        public const string GoodsInspectionNoteProcessID = "11";

        public const string MaterialIssueProcessID = "13";
    }


    public enum WorkFlowProcessId
    {
        STOREAUDIT = 8,
        STOCKADJUSTMENT = 12
    }

    public enum ApplicationModule
    {
        Production = 1,
        SMS = 2,
        QA = 3,
        Asset = 4,
        IAsset = 5,
        MFMS = 6,
        ProductionPlan = 7,
        gAero = 8,
        GCOMS = 9,
        Finance = 10,
        Sales = 11,
        CustomerPortal = 12,
        Shipping = 13,
        HRMS = 14,
        Construction = 15,
    }
    public class Common
    {

        #region SPS

        public const string SPQUC_PROC_CTRL_TRX_PRODUCT_FILTER = "SPQUC_PROC_CTRL_TRX_PRODUCT_FILTER";
        public const string SPQUC_PROC_CTRL_TRX_PROD_PRODUCT_FILTER = "SPQUC_PROC_CTRL_TRX_PROD_PRODUCT_FILTER";

        public const string SP_GETSBUDEPARTMENT = "SPADM_USER_DEPT_BIZUNIT_GET_KV";
        public const string SP_GETSBU = "SPADM_BIZUNIT_MST_GET_KV";

        public const string SP_GET_MAIL_CFG = "SPADM_MAIL_CONFIG_GET";
        public const string SP_GET_DEPT_MST = "SPADM_DEPT_MST_GET_KV";
        public const string SP_GET_DEPT_PAGE_GET_KV = "SPADM_DEPT_PAGE_GET_KV";
        public const string SP_GETSUSERMENU = "SPADM_USER_MENU_DEPT_GET_KV";
        public const string SPADM_DEPT_MST_GET = "SPADM_DEPT_MST_GET";
        public const string SP_GETPROCESSLIST = "SPWKF_USER_PROC_AUTO";
        public const string SP_GETUSERGROUPS = "ADM_USER_GROUP_KV";
        public const string SP_GETFAVOURITESLIST = "SPADM_FAVT_MST_GET_LIST";
        public const string SP_ADDTOFAVOURITELIST = "SPADM_FAVT_MST_SAVE";
        public const string SP_SPFINTAXGETLIST = "SPFIN_TAX_GET_LIST";
        public const string SP_GETPAGEID = "SPADM_PAGE_MST_GET_KV";
        public const string SP_GET_WORKFLOW_DETAILS = "SPWKF_WORK_FLOW_DTL_GET";
        public const string SP_GET_RELATED_WIDGET = "SPINV_ITEM_RELATED_APP_GET_LIST";
        public const string SP_GET_PAGE_DEPT = "SpWkfPageDeptGet";
        public const string SP_GET__DOC_USER_CHECK = "SPDMS_DOC_USER_CHECK";
        public const string SP_DASHBOARD_USER_CHECK = "SPADM_DASHBOARD_USER_CHECK";
        public const string SP_GET_TRANSACTION_PROCESS = "SpWkfTransactionProcessCheck";
        public const string SP_APP_CONFIG_GET = "SPADM_APP_CONFIG_MST_GET";
        public const string SP_CONST_MST_GET = "SPADM_CONST_MST_GET_KV";
        public const string SPINV_DEPT_PAGE_GET_KV = "SPINV_DEPT_PAGE_GET_KV";
        public const string SPINV_ITEM_CATEGORY_GET = "SPINV_ITEM_CATEGORY_GET";
        public const string SPINV_ITEM_CATEGORY_AUTO = "SPINV_ITEM_CATEGORY_AUTO";
        public const string SP_GETAPPCINFIG = "SPADM_CONFIG_MST_GET_KV";
        public const string SPPRD_PRODUCTION_PROCESS_MAP_TREE = "SPPRD_PRODUCTION_PROCESS_MAP_TREE";
        public const string SPINV_ITEM_UOM_GET_KV = "SPINV_ITEM_UOM_GET_KV";
        public const string GETUOMLIST = "SPINV_UOM_MST_GET_KV";
        public const string SPADM_PACK_SPEC_MST_GET_KV = "SPADM_PACK_SPEC_MST_GET_KV";
        public const string SPQUC_PROC_CTRL_TRX_LINE_FILTER = "SPQUC_PROC_CTRL_TRX_LINE_FILTER";
        public const string SPQUC_PROC_CTRL_TRX_PROD_LINE_FILTER = "SPQUC_PROC_CTRL_TRX_PROD_LINE_FILTER";


        public const string SPADM_APP_SUB_TYPE_DATA_GET = "SPADM_APP_SUB_TYPE_DATA_GET";
        public const string SPADM_QUERIES_CFG_GET_KV = "SPADM_QUERIES_CFG_GET_KV";

        public const string SP_GETINBOXMAIL = "SpWkfTransactionMailGet";
        public const string SP_GETINTIMATIONMAIL = "SpWkfTransactionIntMailGet";
        public const string SP_GETCURRENCY = "SPADM_CURRENCY_MST_GET_KV";
        public const string SPADM_APP_TYPE_MST_GET_KV = "SPADM_APP_TYPE_MST_GET_KV";
        public const string SPINV_ITEM_PACK_DTL_GET_KV = "SPINV_ITEM_PACK_DTL_GET_KV";
        public const string SPFIN_COA_MST_GET_KV = "SPFIN_COA_MST_GET_KV";
        public const string SPADM_APP_STATUS_CFG_GET_KV = "SPADM_APP_STATUS_CFG_GET_KV";
        public const string SPFIN_COA_MST_LIST = "SPFIN_COA_MST_LIST";
        public const string SPADM_PLANT_MST_GET_KV = "SPADM_PLANT_MST_GET_KV";
        public const string SPFIN_TAX_MST_GET_KV = "SPFIN_TAX_MST_GET_KV";
        public const string SPINV_ITEM_MST_GET_AUTO = "SPINV_ITEM_MST_GET_AUTO";
        public const string SP_ExceptionLog = "SPADM_EXCEPTION_ERROR_LOG_SAVE";

        public const string SPFIN_TRX_CANCEL_CHECK = "SPFIN_TRX_CANCEL_CHECK";
        public const string SPADM_COMPANY_MST_GET_KV = "SPADM_COMPANY_MST_GET_KV";
        public const string SP_GETCURRENCYBYBTZUUNIT = "SPCRM_CUSTOMER_CUR_GET_KV";
        public const string SP_SPADM_CONST_MST_GET_KV = "SPADM_CONST_MST_GET_KV";
        public const string SPADM_PACK_SPEC_GET = "SPADM_PACK_SPEC_GET";

        public const string SPADM_PACK_MATERIAL_GET = "SPADM_PACK_MATERIAL_GET";

        public const string SPSAL_ORDER_CANCEL_CHECK = "SPSAL_ORDER_CANCEL_CHECK";
        public const string SPPUR_ORDER_CANCEL_CHECK = "SPPUR_ORDER_CANCEL_CHECK";
        public const string SPADM_SYSTEM_CFG_GET = "SPADM_SYSTEM_CFG_GET";
        public const string SPPRD_PRODUCT_MASTER_GET_KV = "SPPRD_PRODUCT_MASTER_GET_KV";
        public const string SPPADM_APP_PARAM_CFG_GET_KV = "SPPADM_APP_PARAM_CFG_GET_KV";
        public const string CFG_DATA_TEXT_FIELD = "CFG_DATA";
        public const string CFG_VALUE_VALUE_FIELD = "CFG_VALUE";
        public const string CON_DATA_TEXT_FIELD = "CON_NAME";
        public const string CON_PK_FIELD = "CON_PK";
        public const string CON_VALUE_FIELD = "CON_VALUE";
        public const string SPHRM_PAY_ELEMENT_SLAB_HDR_GET_KV = "SPHRM_PAY_ELEMENT_SLAB_HDR_GET_KV";
        public const string SPINV_ITEM_MST_GET_KV = "SPINV_ITEM_MST_GET_KV";
        public const string SPASR_AsrAssetTypeMst_GET_KV = "SPASR_AsrAssetTypeMst_GET_KV";
        public const string SPASR_AsrAssetMst_GET_KV = "SPASR_AsrAssetMst_GET_KV";
        public const string SPASR_ITEM_MST_GET_KV = "SPASR_ITEM_MST_GET_KV";
        public const string SPADM_APP_TRX_COMMENT_SAVE = "SPADM_APP_TRX_COMMENT_SAVE";
        public const string SPADM_APP_TRX_COMMENT_GET_KV = "SPADM_APP_TRX_COMMENT_GET_KV";
        public const string SPADM_APP_TRX_COMMENT_DELETE = "SPADM_APP_TRX_COMMENT_DELETE";
        public const string SpWkfTransactionStatusInitGet = "SpWkfTransactionStatusInitGet";

        public const string SPADM_SUMMARY_SAVE = "SPADM_SUMMARY_SAVE";
        public const string SPRATE_CHANGE_HISTORY_GET = "SPRATE_CHANGE_HISTORY_GET";
        public const string SPADM_USER_GROUP_GET_AUTO = "SPADM_USER_GROUP_GET_AUTO";
        public const string SPFIN_COA_MST_AUTO = "SPFIN_COA_MST_AUTO";
        public const string SPPUR_VENDOR_ITEM_MAP_AUTO = "SPPUR_VENDOR_ITEM_MAP_AUTO";

        public const string SPFIN_INVOICE_GST_TYPE_MST_GET_KV = "SPFIN_INVOICE_GST_TYPE_MST_GET_KV";
        public const string SPADM_PORT_MST_GET_KV = "SPADM_PORT_MST_GET_KV";
        public const string SPFIN_GST_CLASS_MST_GET = "SPFIN_GST_CLASS_MST_GET";
        public const string SPFIN_AGENT_MST_GET_KV = "SPFIN_AGENT_MST_GET_KV";

        public const string SPADM_REPORT_CFG_AUTO = "SPADM_REPORT_CFG_AUTO";
        public const string SP_SPPRD_BIN_CARD_ISSUE_GET = "SPPRD_BIN_CARD_ISSUE_GET";

        public const string SPADM_CONST_MST_GET_KV = "SPADM_CONST_MST_GET_KV";
        public const string SPINV_WO_CUSTOMER_GET_KV = "SPINV_WO_CUSTOMER_GET_KV";
        public const string SPCRM_CUSTOMER_MST_GET_KV = "SPCRM_CUSTOMER_MST_GET_KV";
        public const string SPFIN_GST_CLASS_MST_AUTO = "SPFIN_GST_CLASS_MST_AUTO";
        public const string SPADM_CURRENCY_CONV_FACT_GET = "SPADM_CURRENCY_CONV_FACT_GET";
        public const string SPASSET_AsrAssetTypeMst_GET_KV = "SPASSET_AsrAssetTypeMst_GET_KV";

        public const string SPFIN_INVOICE_VND_ASSET_TYPE_VALIDATE = "SPFIN_INVOICE_VND_ASSET_TYPE_VALIDATE";
        public const string SP_INV_ITEM_GROUP_MST_AUTO= "SP_INV_ITEM_GROUP_MST_AUTO";

        public const string SPADM_COMPANY_MST_FILTER = "SPADM_COMPANY_MST_FILTER";
        public const string SPADM_ACTIVE_FILTER = "SPADM_ACTIVE_FILTER";
        public const string SPPRD_COMP_MST_FILTER = "SPPRD_COMP_MST_FILTER";
        public const string SPPRD_DISP_TYPE_FILTER = "SPPRD_DISP_TYPE_FILTER";
        public const string SPPRD_DISP_MST_FILTER = "SPPRD_DISP_MST_FILTER";
        public const string SPFIN_TRX_AUDIT_BIT_GET = "SPFIN_TRX_AUDIT_BIT_GET";
        public const string SPFIN_TRX_AUDIT_LIST = "SPFIN_TRX_AUDIT_LIST";
        public const string SPFIN_TRX_AUDIT_GET = "SPFIN_TRX_AUDIT_GET";
        #endregion


        #region Fields
        public const string F_BIZUNIT = "DPT_BIZUNIT";
        public const string F_BIZUNITNAME = "BZU_NAME";
        public const string F_DEPARTMENT = "DPT_PK";
        public const string F_DEPARTMENTNAME = "DPT_NAME";

        public const string F_MNU_PK = "MNU_PK";
        public const string F_MNU_NAME = "MNU_NAME";

        public const string F_PK = "PK";
        public const string F_VALUE = "VALUE";

        public const string APT_NAME = "APT_NAME";
        public const string APT_CODE = "APT_CODE";


        public const string F_WORKFLOW_IS_CLOSED = "WKF_CLOSED";
        public const string F_WORKFLOW_REF_ID = "WKF_REFERENCE";
        public const string F_WORKFLOW_PROCESS_ID = "WKF_PROCESS";
        public const string F_WORKFLOW_TASK_ID = "WKF_TASK";
        public const string F_WORKFLOW_ACTION = "WKF_ACTION";
        public const string F_WORKFLOW_TYPE = "WKF_TYPE";

        public const string PHS_NAME_FIELD = "PHS_NAME";
        public const string PHS_PK_FIELD = "PHS_PK";
        public const string CFG_PK_FIELD = "CFG_PK";
        public const string CFG_DATA_FIELD = "CFG_DATA";

        public const string CNG_NAME = "CNG_NAME";
        public const string CNG_PK = "CNG_PK";
        public const string CON_PK = "CON_PK";
        public const string CON_NAME = "CON_NAME";

        public const string MOD_PK = "MOD_PK";
        public const string MOD_NAME = "MOD_NAME";
        public const string MOD_SERVER = "MOD_SERVER";
        //Multilanguage ddl
        public const string F_LNG_KEY = "Code";
        public const string F_LNG_VAL = "Culture";
        #endregion

        #region Parameters

        public const string P_WORKFLOW_APP_ID = "P_APP_ID";
        public const string P_WORKFLOW_PEOCESS_ID = "P_PROC_ID";
        public const string P_DPT_ACTIVE = "DPT_ACTIVE";
        public const string P_DEPT_PK = "DPT_PK";

        public const string P_APT_CODE = "P_APT_CODE";
        public const string P_QRY_PK = "P_QRY_PK";
        public const string P_AST_VALUE = "P_AST_VALUE";
        public const string P_TRX_DATE = "P_TRX_DATE";

        public const string P_DPT_PARENT = "P_DPT_PARENT";
        public const string P_DPT_TYPE = "P_DPT_TYPE";
        public const string P_DPT_CATEGORY = "P_DPT_CATEGORY";

        public const string P_USERPK = "P_USER_PK";
        public const string P_PAGE_URL = "P_PAGE_URL";
        public const string P_MODULE = "P_MODULE";
        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_BZU_PK = "P_BZU_PK";

        public const string P_DEPARTMENT = "P_DEPT";
        public const string P_USRGRPFLAG = "P_FLAG";
        public const string P_PAGEPK = "P_PAGE";
        public const string P_PAGEURL = "P_PAG_URL";
        public const string P_SER_FLD_NAME = "P_SER_FLD_NAME";
        public const string P_SER_VALUE = "P_SER_VALUE";
        public const string P_PRCNAME = "P_prcName"; 



        public const string P_APT_PK = "P_APT_PK";
        public const string P_MOD_PK = "P_MOD_PK";
        public const string P_APT_SPL_COND = "P_APT_SPL_COND";
        public const string P_COA_PK = "P_COA_PK";
        public const string P_ASC_TYPE = "P_ASC_TYPE";
        public const string P_ASC_SUB_TYPE_VAL = "P_ASC_SUB_TYPE_VAL";
        public const string P_COA_SUB_TYPE = "P_COA_SUB_TYPE";
        public const string P_CFG_SPL_COND = "P_CFG_SPL_COND";
        public const string P_FLD_NAME = "P_FLD_NAME";
        public const string P_FTH_PK = "P_FTH_PK";
        public const string P_PARTY_PK = "P_PARTY_PK";
        public const string P_APS_NAME = "P_APS_NAME";

        public const string TAX_PK = "TAX_PK";
        public const string P_XML = "P_XML";
        public const string TAX_CATEGORY = "TAX_CATEGORY";
        public const string TAX_SUB_CATEGORY = "TAX_SUB_CATEGORY";
        public const string P_TAX_HEAD = "P_TAX_HEAD";

        public const string P_TRX_PK = "P_TRX_PK";
        public const string P_TRX_APP_TYPE = "P_TRX_APP_TYPE";
        public const string SPFIN_TAX_MST_GET_AUTO = "SPFIN_TAX_MST_GET_AUTO";
        public const string SPPUR_VENDOR_CONTACT_GET_KV = "SPPUR_VENDOR_CONTACT_GET_KV";

        public const string P_SOH_PK = "P_SOH_PK";
        public const string P_POH_PK = "P_POH_PK";
        public const string PSYSPK = "P_SYS_PK";

        public const string P_MNG_PK = "P_MNG_PK";
        public const string P_DEPT = "P_DEPT";

        public static string P_PAGE_NUM = "P_PAGE_NUM";
        public static string P_PAGE_SIZE = "P_PAGE_SIZE";
        public const string SPADM_PAGE_MENU_CFG_GET = "SPADM_PAGE_MENU_CFG_GET";
        public const string P_PAGE = "P_PAGE";
        public const string P_FROM_DT = "P_FROM_DT";
        public const string P_TO_DT = "P_TO_DT";
        public const string P_TRN_NAME = "P_TRN_NAME";
        public const string P_EIH_TYPE = "P_EIH_TYPE";
        public const string SPADM_LOCK_CHECK = "SPADM_LOCK_CHECK";
        public const string SPADM_DEPT_MST_INV_GET = "SPADM_DEPT_MST_INV_GET";

        public const string ITC_PK = "ITC_PK";
        public const string P_ITC_NAME = "P_ITC_NAME";
        public const string ITC_NAME = "ITC_NAME";
        public const string P_ITC_IS_SALE = "P_ITC_IS_SALE";
        public const string ITC_ACTIVE = "ITC_ACTIVE";
        public const string SPINV_ITEM_CATEGORY_GET_KV = "SPINV_ITEM_CATEGORY_GET_KV";
        public const string ITM_NAME = "ITM_NAME";
        public const string IS_PRODUCT = "IS_PRODUCT"; 
        public const string ITM_PK = "ITM_PK";
        public const string ITM_ACTIVE = "ITM_ACTIVE";
        public const string ITM_CATEGORY = "ITM_CATEGORY";
        public const string P_ITM_CAT = "P_ITM_CAT";
        public const string P_ITM_SUB_TYPE = "P_ITM_SUB_TYPE";
        public const string P_PACK_SPEC = "P_PACK_SPEC";
        public const string SpWkfUserMstAuto = "SpWkfUserMstAuto";
        public const string FNINV_ITEM_UOM_CONV_FACTOR = "FNINV_ITEM_UOM_CONV_FACTOR";
        public const string SPFIN_COA_COST_CENTER_MPG_GET = "SPFIN_COA_COST_CENTER_MPG_GET";
        public const string SPFIN_TRX_COC_DTL_GET = "SPFIN_TRX_COC_DTL_GET";

        public const string SPPUR_ORDER_DEPT_MPG_GET = "SPPUR_ORDER_DEPT_MPG_GET";
        public const string SPADM_MODULE_MST_GET_KV = "SPADM_MODULE_MST_GET_KV";

        public const string P_FTM_PK = "P_FTM_PK";
        public const string P_INVOICE_TYPE = "P_INVOICE_TYPE";
        public const string P_FTM_TRX_TYPE = "P_FTM_TRX_TYPE";

        public const string P_PRM_PK = "P_PRM_PK";
        //public const string P_ACTIVE = "P_ACTIVE";
        //public const string P_BIZUNIT = "P_BIZUNIT";
        public const string PRM_TYPE = "PRM_TYPE";
        public const string PRM_IS_SALES_FROM = "PRM_IS_SALES_FROM";
        public const string PRM_IS_SALES_TO = "PRM_IS_SALES_TO";
        public const string PRM_IS_PUR_FROM = "PRM_IS_PUR_FROM";
        public const string PRM_IS_PUR_TO = "PRM_IS_PUR_TO";
        public const string PRM_NAME = "PRM_NAME";
        public const string SPFIN_YEAR_GET="SPFIN_YEAR_GET";
        public const string SPADM_PACK_SPEC_MST_GET_AUTO = "SPADM_PACK_SPEC_MST_GET_AUTO";
        public const string SPADM_FORMULA_MST_GET_KV = "SPADM_FORMULA_MST_GET_KV";



        public const string P_CON_PK = "P_CON_PK";
        public const string P_CON_ACTIVE = "P_CON_ACTIVE";
        public const string P_CON_GROUP = "P_CON_GROUP";
        public const string P_CGT_VALUE = "P_CGT_VALUE";
        public const string P_CNG_VALUE = "P_CNG_VALUE";
        public const string P_CON_NAME = "P_CON_NAME";
        public const string P_CON_PARENT = "P_CON_PARENT";
        public const string P_CON_BIZUNIT = "P_CON_BIZUNIT";
        public const string P_CON_SPL_COND = "P_CON_SPL_COND";
        public const string CUS_PK = "CUS_PK";
        public const string PRETNO = "P_RET_NO";
        public const string P_RET_REF_PK = "P_RET_REF_PK"; 



        #endregion
    }


    public enum StoresGetFlag
    {
        GeneralAndCompoundStore = -1,
        GeneralStores = 0,
        DamageStore = 1,
        CompoundStore = 2,
        GRNStore = 3,
        GINStore = 4,
        WOStore=18,
        WOProductStore = 19,
        WOBrandStore=20

    }

    public enum InvoiceType
    {
        Sales = 1,
        Purchase = 2
    }
}
