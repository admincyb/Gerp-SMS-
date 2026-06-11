using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Finance
{
    public class Procedures
    {
       //FC Reverse
        //public const string GETINVOICEDETAILS = "SPFIN_INVOICE_VND_GET";
        //public const string SAVEINVOICEDETAILS = "SPFIN_INVOICE_VND_SAVE";
        //public const string DELETEINVOICEDETAILS = "SPFIN_INVOICE_VND_DELETE";
        public const string GETFCRLIST = "SPFIN_FC_HOLD_REVERT_GET_KV";
        public const string GETFCHOLDDTL = "SPFIN_FC_HOLD_REVERT_DTL_GET";
        public const string SAVEFCHOLD = "SPFIN_FC_HOLD_REVERT_SAVE";
        public const string DELETEFCHOLD = "SPFIN_FC_HOLD_REVERT_DELETE";
        public const string CNDNGetReport = "SPFIN_CRDR_NOTE_RPT";
        public const string FCVOUCHERNUMBERAUTO = "SPFIN_FC_HOLD_REVERT_AUTO";
        public const string SPFIN_TRX_GET_LIST = "SPFIN_TRX_GET_LIST"; 
        
        
        //VAT SALE EXPORT
        public const string GETVATSALELIST = "SPFIN_INVOICE_CUS_TAX_TRX_GET";
        public const string SAVEVATSALELIST = "SPFIN_INVOICE_CUS_TAX_TRX_SAVE";
        public const string GETVATSALEREPORT = "SPFIN_INVOICE_CUS_TAX_TRX_RPT";

        public const string VSEFIELDAUTO = "SPFIN_INVOICE_CUS_TAX_TRX_AUTO";
        public const string GETVATSALESEARCHLIST = "SPFIN_INVOICE_CUS_TAX_TRX_GET_LIST";

        //GST Report
        public const string GETTAXCATEGORY = "SPFIN_TAX_MST_GET_KV";
        public const string GETGSTREPORTLIST = "SPADM_CONFIG_MST_GET_KV";
        public const string GETGSTREPORTDATA = "SPFIN_VAT_BUY_DTL_RPT";
        public const string GETGSTMONTHLYREPORTDATA = "SPFIN_VAT_BUY_MONTHLY_SUMMARY_RPT";

        //GST Return
        public const string GETDETAILS = "SPFIN_TAX_GST_GET";
        public const string SAVEGSTRETURNLIST = "SPFIN_TAX_GST_TRX_SAVE";
        public const string GETGSTRETURNREPORT = "SPFIN_TAX_GST_TRX_RPT";
        public const string GETGSTRETURNLIST = "SPFIN_TAX_GST_TRX_GET_LIST";
        public const string GETGSTRETURNDETAILS = "SPFIN_TAX_GST_TRX_GET";
        public const string DELETEGSTRETURN = "SPFIN_TAX_GST_TRX_DELETE";

        public const string GETCOMPANYINFOGSTFILE = "SPFIN_GST_XML_FILE_GET";
        public const string GETCOMPANYINFOGSTFILEFORPIPE = "SPFIN_GST_PIPE_FILE_GET";

        //Audit Trials
        public const string GETREPORTDATA_AUDIT_TRIALS = "SPFIN_VAT_AUD_TRIAL_RPT";
        public const string GETAUDITTRIALINVOICERPT = "SPFIN_AUD_TRIAL_RPT";

        //CommSetup
        public const string GETAGENTLIST = "SPFIN_AGENT_MST_GET_KV";
        public const string SPFIN_AGENT_COMMISION_DTL_DELETE = "SPFIN_AGENT_COMMISION_DTL_DELETE"; 
        public const string GETMAPPEDCUSTOMERLIST = "SPFIN_AGENT_COMMISION_GET_LIST";
        public const string GETALLCUSTOMERLIST = "SPCRM_CUSTOMER_MST_GET_KV";
        public const string SPFIN_AGENT_COMMISION_GET = "SPFIN_AGENT_COMMISION_GET";
        public const string SPFIN_AGENT_COMMISION_SAVE = "SPFIN_AGENT_COMMISION_SAVE";

        //Depreciation
        public const string SPFIN_DEPRECIATION_HDR_GET_LIST = "SPFIN_DEPRECIATION_HDR_GET_LIST";
        public const string SPXACCTGASSET_GET_KV = "SPXacCtgAsset_GET_KV";
        public const string SPASRASSETTYPEMST_GET_KV = "SPAsrAssetTypeMst_GET_KV";
        public const string SPADMLOCATIONMST_GET_KV = "SPAdmLocationMst_GET_KV";
        public const string SPADM_COMPANY_MST_GET_KV = "SPADM_COMPANY_MST_GET_KV";
        public const string SPFIN_DEPRECIATION_HDR_SAVE = "SPFIN_DEPRECIATION_HDR_SAVE";
        public const string SPFIN_DEPRECIATION_GET_XML = "SPFIN_DEPRECIATION_GET_XML";
        public const string SPFIN_DEPRECIATION_ASSET_GET = "SPFIN_DEPRECIATION_ASSET_GET";
        public const string SPFIN_DEPRECIATION_HDR_DELETE = "SPFIN_DEPRECIATION_HDR_DELETE";
        public const string SPFIN_DEPRECIATION_DTL_DELETE = "SPFIN_DEPRECIATION_DTL_DELETE";
        public const string SPASR_DEPRECIATION_RPT = "SPASR_DEPRECIATION_RPT";
        public const string SPFIN_DEPRECIATION_HDR_GET = "SPFIN_DEPRECIATION_HDR_GET";

        public const string SPFIN_ASSET_DISPOSAL_HDR_GET_LIST = "SPFIN_ASSET_DISPOSAL_HDR_GET_LIST";
        public const string SPFIN_ASSET_DISPOSAL_GET_XML = "SPFIN_ASSET_DISPOSAL_GET_XML";
        public const string SPFIN_ASSET_DISPOSAL_HDR_GET = "SPFIN_ASSET_DISPOSAL_HDR_GET";
        public const string SPFIN_ASSET_DISPOSAL_GET = "SPFIN_ASSET_DISPOSAL_GET";

        //Year End Voucher
        public const string SPADM_APP_TYPE_MST_GET_KV = "SPADM_APP_TYPE_MST_GET_KV";
        public const string SPADM_CONST_MST_GET_KV = "SPADM_CONST_MST_GET_KV";
        public const string SPFIN_YEAR_END_TRX_GET_LIST = "SPFIN_YEAR_END_TRX_GET_LIST";
        public const string SPFIN_YEAR_END_TRX_DTL_SAVE = "SPFIN_YEAR_END_TRX_DTL_SAVE";
        public const string SPFIN_YEAR_END_TRX_GET_AUTO = "SPFIN_YEAR_END_TRX_GET_AUTO";

        //BadDebits

        public const string GETBADDEBITDETAILS = "SPFIN_INVOICE_BAD_DEBIT_GET";
        public const string SAVEBADDEBITDETAILS = "SPFIN_INVOICE_CUS_BAD_DEBIT_DTL_SAVE";
        public const string DELETEBADDEBITDETAILS = "";
        public const string GETBDRELIEFLIST = "SPFIN_INVOICE_CUS_BAD_DEBIT_GET_LIST";
        public const string GETBDRELIEF_GET_KV = "SPFIN_INVOICE_CUS_BAD_DEBIT_DTL_GET_KV";
        public const string DELETEBDRELIEFDETAILS = "SPFIN_INVOICE_CUS_BAD_DEBIT_DTL_DELETE";

        //Bank Guarantee
        public const string SPFIN_BANK_GUARANTEE_MST_SAVE = "SPFIN_BANK_GUARANTEE_MST_SAVE";
        public const string SPFIN_BANK_GUARANTEE_MST_GET_KV = "SPFIN_BANK_GUARANTEE_MST_GET_KV";
        public const string SPFIN_BANK_GUARANTEE_MST_DELETE = "SPFIN_BANK_GUARANTEE_MST_DELETE";

        //Closing Stock
        public static string SPINV_INV_ITEM_DEPT_OPENING_STOCK_GET="SPINV_INV_ITEM_DEPT_OPENING_STOCK_GET";
        public static string SPINV_CLOSING_STOCK_GET_LIST = "SPINV_CLOSING_STOCK_GET_LIST";
        public static string SPINV_CLOSING_STOCK_ITEM_GET = "SPINV_CLOSING_STOCK_ITEM_GET";
        public static string SPINV_CLOSING_STOCK_GET_XML = "SPINV_CLOSING_STOCK_GET_XML";
        public static string SPINV_CLOSING_STOCK_SAVE = "SPINV_CLOSING_STOCK_SAVE";
        public static string SPINV_CLOSING_STOCK_DELETE = "SPINV_CLOSING_STOCK_DELETE";
        public static string SPINV_CLOSING_STOCK_DTL_HISTORY_GET = "SPINV_CLOSING_STOCK_DTL_HISTORY_GET";
        public static string SPFIN_YEAR_MST_AUTO = "SPFIN_YEAR_MST_AUTO";
        public static string SPFIN_YEAR_INV_DEPT_MAP_GET = "SPFIN_YEAR_INV_DEPT_MAP_GET";
        //Finance Report cfg

        public static string SPFIN_REPORT_TEMPLATE_CFG_GET_KV = "SPFIN_REPORT_TEMPLATE_CFG_GET_KV";
        public static string SPFIN_REPORT_TEMPLATE_CFG_SAVE = "SPFIN_REPORT_TEMPLATE_CFG_SAVE";
        public static string SPFIN_COA_MST_GET_LIST = "SPFIN_COA_MST_GET_LIST";
        public static string FIN_REPORT_COA_MAP_SAVE = "FIN_REPORT_COA_MAP_SAVE";

        //Voucher Locking
        public static string SPFIN_LOCK_LOG_SAVE = "SPFIN_LOCK_LOG_SAVE";
        public static string SPFIN_LOCK_LOG_GET_KV = "SPFIN_LOCK_LOG_GET_KV";
        public static string SPFIN_LOCK_CHECK = "SPFIN_LOCK_CHECK";
        public static string SPFIN_YEAR_LOCK_CHECK = "SPFIN_YEAR_LOCK_CHECK"; 

        public static string SPWKF_JOB_AUTO_YEND_JRNL_CAN_STATUS = "SPWKF_JOB_AUTO_YEND_JRNL_CAN_STATUS";

        public static string SPFIN_YEAR_CLOSE_SAVE = "SPFIN_YEAR_CLOSE_SAVE";

        public static string SPADM_DOC_ATTACH_DELETE = "SPADM_DOC_ATTACH_DELETE";

        //Monthly Production
        public static string SPFIN_MTHLY_PRODUCTION_GET_LIST = "SPFIN_MTHLY_PRODUCTION_GET_LIST";
        public static string SPFIN_MTHLY_PRODUCTION_SAVE = "SPFIN_MTHLY_PRODUCTION_SAVE";
        public static string SPFIN_MTHLY_PRODUCTION_GET_XML = "SPFIN_MTHLY_PRODUCTION_GET_XML";
        public static string SPFIN_MTHLY_PRODUCTION_DELETE = "SPFIN_MTHLY_PRODUCTION_DELETE";
        
        //Finanacial Year Master
        public static string SPFIN_YEAR_MST_SAVE = "SPFIN_YEAR_MST_SAVE";
        public static string SPFIN_YEAR_MST_GET_KV = "SPFIN_YEAR_MST_GET_KV";
        public static string SPFIN_YEAR_MST_DELETE = "SPFIN_YEAR_MST_DELETE";

        public static string SPFIN_GSTR_GET = "SPFIN_GSTR_GET";

        //stock closing
        public static string SPINV_INV_ITEM_DEPT_OPENING_STOCK_SAVE = "SPINV_INV_ITEM_DEPT_OPENING_STOCK_SAVE";
        public static string SPINV_INV_ITEM_DEPT_OPENING_STOCK_GET_LIST = "SPINV_INV_ITEM_DEPT_OPENING_STOCK_GET_LIST";
        public static string SPINV_INV_ITEM_DEPT_OPENING_STOCK_GET_KV = "SPINV_INV_ITEM_DEPT_OPENING_STOCK_GET_KV";
        public static string SPINV_INV_ITEM_DEPT_OPENING_STOCK_DELETE="SPINV_INV_ITEM_DEPT_OPENING_STOCK_DELETE";

        public static string SPFIN_COA_OPENING_BAL_GET_LIST = "SPFIN_COA_OPENING_BAL_GET_LIST";
        public static string SPFIN_COA_OPENING_BAL_GET = "SPFIN_COA_OPENING_BAL_GET";
        public static string SPFIN_COA_OPENING_BAL_WKF_SAVE = "SPFIN_COA_OPENING_BAL_WKF_SAVE";
        public static string SPFIN_COA_OPENING_BAL_GET_KV = "SPFIN_COA_OPENING_BAL_GET_KV";
        public static string SPFIN_COA_OPENING_BAL_AUTO = "SPFIN_COA_OPENING_BAL_AUTO";
        public static string SPFIN_COA_OPENING_BAL_DELETE = "SPFIN_COA_OPENING_BAL_DELETE";

        public static string SPFIN_COA_MST_TREE_GET = "SPFIN_COA_MST_TREE_GET";

        public static string SPFIN_COA_MST_CWIP_ACCOUNT_GET = "SPFIN_COA_MST_CWIP_ACCOUNT_GET";
        public static string SPFIN_COA_MST_CHILD_ACCOUNT_GET = "SPFIN_COA_MST_CHILD_ACCOUNT_GET";
        public static string SPFIN_TRX_COA_WISE_GET_LIST = "SPFIN_TRX_COA_WISE_GET_LIST";
        public static string SPFIN_TRX_CWIP_WKF_SAVE = "SPFIN_TRX_CWIP_WKF_SAVE";
        public static string SPFIN_TRX_CWIP_GET_LIST = "SPFIN_TRX_CWIP_GET_LIST";
        public static string SPFIN_TRX_CWIP_GET_KV = "SPFIN_TRX_CWIP_GET_KV";
        public static string SPFIN_TRX_CWIP_DELETE = "SPFIN_TRX_CWIP_DELETE";
        public static string SPFIN_TRX_CWIP_HDR_AUTO = "SPFIN_TRX_CWIP_HDR_AUTO";
        public static string SPFIN_DEPRECIATION_HDR_WKF_SAVE = "SPFIN_DEPRECIATION_HDR_WKF_SAVE";
        public static string SPFIN_ASSET_DISPOSAL_HDR_WKF_SAVE = "SPFIN_ASSET_DISPOSAL_HDR_WKF_SAVE";
        public static string SPASSET_XacConReuset_GET_KV = "SPASSET_XacConReuset_GET_KV";


        //Budget
        public const string SPFIN_YEAR_GET_KV = "SPFIN_YEAR_GET_KV";
        public const string SPFIN_BUDGET_LIST = "SPFIN_BUDGET_LIST";
        public const string SPFIN_BUDGET_VERSION_GET = "SPFIN_BUDGET_VERSION_GET";
        public const string SPFIN_BUDGET_DTL_VERSION_GET = "SPFIN_BUDGET_DTL_VERSION_GET";
        public const string SPFIN_BUDGET_NO_AUTO = "SPFIN_BUDGET_NO_AUTO";
        public const string SPFIN_BUDGET_DTL_GET = "SPFIN_BUDGET_DTL_GET";
        

    }
}
