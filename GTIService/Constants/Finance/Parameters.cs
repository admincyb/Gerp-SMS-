using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Finance
{
    public class Parameters
    {
        public const string P_AGENT_PK = "P_AGENT_PK";
        public const string HRH_PK = "P_HRH_PK";
        public const string DTL_VOUCHER_NO = "P_DTL_VOUCHER_NO";
        public const string CBM_PK = "P_CBM_PK";
        public const string CUR_PK = "P_CUR_PK";
        public const string Active = "P_ACTIVE";
        public const string LastModDate = "P_LAST_MOD_DT";
        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_VALUE= "P_VALUE";
        public const string P_BIZUNIT_ADD = "P_BIZUNIT_ADD"; 
        public const string CDH_PK = "P_CDH_PK";
        public const string P_HRH_STATUS = "P_HRH_STATUS";
        public const string SEARCHVAL = "P_HRH_NO";
        public const string SEARCHVALAUTO = "P_SEARCH_VAL";
        public const string FITERSTATUS = "P_TRX_STATUS";
        public const string P_COA_PK = "P_COA_PK";
        public const string P_STATUS = "P_STATUS";

        public const string P_FTH_PK = "P_FTH_PK";
        public const string P_IS_RECONCILED = "P_IS_RECONCILED";
        public const string P_TYPE = "P_TYPE";
        public const string P_PLANT = "P_PLANT";
        public const string P_TYPE_PK = "P_TYPE_PK";
        public const string P_INSTR_NO = "P_INSTR_NO"; 
        ////PO Invoice
        //public const string POH_PK = "P_POH_PK";
        //public const string POD_PO = "P_POD_PO";
        //public const string P_IVH_TYPE = "P_IVH_TYPE";
        public const string P_STATUS_FILTER = "P_STATUS_FILTER";
        //public const string P_CATEGORY = "P_IVH_CATEGORY";
        //public const string P_GROUP = "P_IVH_GROUP";
        //public const string P_IS_PENDING = "P_IS_PENDING";

        //Vat Sale Export
        public const string FROMDATE = "P_TSH_FROM_DATE";
        public const string TODATE = "P_TSH_TO_DATE";
        public const string TSH_PK = "P_TSH_PK";
        public const string FIELD = "P_FIELD";
        public const string VSECUSTOMERNAME = "TSD_CUSTOMER_NAME";
        public const string VSEINVOICENO = "TSD_INVOICE_NO";

        public const string TSD_INVOICE_NO = "P_TSD_INVOICE_NO";
        public const string TSD_CUSTOMER_NAME = "P_TSD_CUSTOMER_NAME";

        //GST Report
        public const string CFG_PK = "P_CFG_PK";
        public const string CFG_TYPE = "P_CFG_TYPE";
        public const string CFG_SPL_COND = "P_CFG_SPL_COND";

        public const string TAX_CATEGORY = "TAX_CATEGORY";
        public const string TAX_PK = "TAX_PK";
        public const string TAX_SUB_CATEGORY = "TAX_SUB_CATEGORY";
        public const string TAX_IS_PURCHASE = "P_TAX_IS_PURCHASE";
        public const string TAX_IS_RETURN = "P_TAX_IS_RETURN";//GST Tax Filter

        public const string V_ACTIVE = "V_ACTIVE";
        public const string V_FROM_DATE = "V_FROM_DT";
        public const string V_TO_DATE = "V_TO_DT";
        public const string TAX_TYPE = "V_TAX";
        public const string REPORT_TYPE = "V_REPORT";
        public const string UNRECRCD = "V_UNRECONCILE_RECORDS";
        public const string BIZUNIT = "V_BIZUNIT";
        public const string CMP_PK = "V_CMP_PK";

        //GST Return
        public const string FROM_DATE = "P_FROM_DATE";
        public const string TO_DATE = "P_TO_DATE";
        public const string COMPANY_PK = "P_CMP_PK";

        public const string GST_FILE = "P_GST_FILE";

        public const string TGH_PK = "P_TGH_PK";

        //Audit Trials
        public const string P_REPORT = "P_REPORT";
        public const string P_FROM_DATE = "P_FROM_DT";
        public const string P_TO_DATE = "P_TO_DT";
        public const string P_USERPK = "P_USER_PK";
        public const string P_REC_STATUS = "P_REC_STATUS";

        //Commision Setup
        public const string P_VEN_PK = "P_VEN_PK";
        public const string P_AGENT = "P_AGENT";
        public const string P_CUS_PK = "P_CUS_PK";
        public const string P_CUSTOMER = "P_CUSTOMER";
        public const string P_XML = "P_XML";

        //Depreciation
        public const string P_XCAPK = "P_xcaPK";
        public const string P_LOCPK = "P_locPK";
        public const string P_FDH_PK = "P_FDH_PK";
        public const string P_ADH_PK = "P_ADH_PK";
        public const string P_RET_NO = "P_RET_NO";
        public static string P_FDD_PK = "P_FDD_PK";
        public static string P_atpPK = "P_atpPK";
        public const string P_FDH_NO = "P_FDH_NO";
        public const string P_ADH_NO = "P_ADH_NO";
        //Depreciation-Asset Filteration
        public const string P_FDH_FROM = "P_FDH_FROM";
        public const string P_FDH_TO = "P_FDH_TO";
        public const string P_PUR_FDT = "P_PUR_FDT";
        public const string P_PUR_TDT = "P_PUR_TDT";
        public const string P_FDH_DPRMNTH = "P_FDH_DPRMNTH";
        public const string P_FDH_DPRMNTH_CONFG = "P_FDH_DPRMNTH_CONFG";
        public const string P_xcaPK = "P_xcaPK";
        public const string P_locPK = "P_locPK";
        public const string P_xbdPK = "P_xbdPK";
        public const string P_asrCode = "P_asrCode";
        public const string P_asrName = "P_asrName";
        public const string P_ADH_FROM = "P_ADH_FROM";
        public const string P_ADH_TO = "P_ADH_TO";
        public const string P_ADH_DPRMNTH = "P_ADH_DPRMNTH";
        public const string P_ADH_DPRMNTH_CONFG = "P_ADH_DPRMNTH_CONFG";

        //Year End Voucher
        public const string P_CON_PK = "P_CON_PK";
        public const string P_CON_ACTIVE = "P_CON_ACTIVE";
        public const string P_CGT_VALUE = "P_CGT_VALUE";
        public const string P_CNG_VALUE = "P_CNG_VALUE";
        public const string P_AS_ON_DATE = "P_AS_ON_DATE";
        public const string P_LST_STATUS = "P_LST_STATUS";

        //Bad Debit
        public const string P_MNTH_INTRVL = "P_MNTH_INTRVL";
        public const string P_IBD_PK = "P_IBD_PK";

        //Closing stock
        public static string P_AS_ON = "P_AS_ON";
        public static string P_LSH_PK = "P_LSH_PK";
        public static string P_LSD_ITEM_CAT = "P_LSD_ITEM_CAT";
        public static string P_LSH_AS_ON = "P_LSH_AS_ON";
        public static string P_LSH_NO = "P_LSH_NO";
        public static string P_LSH_STATUS = "P_LSH_STATUS";

        // Voucher Locking
        public static string P_FLL_PK = "P_FLL_PK";
        public static string P_FLL_DATE = "P_FLL_DATE";
        public static string P_FLL_REMARKS = "P_FLL_REMARKS";
        public static string P_DATE = "P_DATE";

        public const string DOC_PK = "DOC_PK";
        public const string P_DOC_TASK = "P_DOC_TASK";
        public const string P_DOC_TASK_ID = "P_DOC_TASK_ID";        

        //Monthly Production
        public const string P_FPH_FROM = "P_FPH_FROM";
        public const string P_FPH_TO = "P_FPH_TO";
        public const string P_PAGE_NUM = "P_PAGE_NUM";
        public const string P_PAGE_SIZE = "P_PAGE_SIZE";
        public const string P_FPH_PK = "P_FPH_PK";
        public const string P_RET_VAL = "P_RET_VAL";

        //Financial Year Master
        public const string P_FYR_PK = "P_FYR_PK";
        public const string P_FYR_NAME = "P_FYR_NAME";
        public const string P_FYR_DESC = "P_FYR_DESC";
        public const string P_FYR_DATE_FROM = "P_FYR_DATE_FROM";
        public const string P_FYR_DATE_TO = "P_FYR_DATE_TO";
        public const string P_FYR_STATUS = "P_FYR_STATUS";
        public const string P_FYR_DEPT = "P_FYR_DEPT";
        public const string P_FYR_ACTIVE = "P_FYR_ACTIVE";
        public const string P_RPT_TYPE = "P_RPT_TYPE";

        public const string P_COH_PK = "P_COH_PK";
        public const string P_COH_DATE = "P_COH_DATE";
        public const string P_RET_REF_PK = "P_RET_REF_PK";

        public const string P_DEPT_PK= "P_DEPT_PK";
        public const string P_PAGE_URL = "P_PAGE_URL";
        public const string P_SER_VAL = "P_SER_VAL";
        public const string P_SORT_DIR = "P_SORT_DIR";
        public const string P_SORT_BY = "P_SORT_BY";

        public const string P_CWH_PK = "P_CWH_PK";
        public const string P_FLD_NAME = "P_FLD_NAME";
        public const string P_DEPT = "P_DEPT";

        public const string P_PO_NUMBER = "P_PO_NUMBER"; 
    }
}
