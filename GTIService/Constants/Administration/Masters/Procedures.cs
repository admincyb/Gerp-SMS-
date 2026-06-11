using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Administration.Masters
{
    public class Procedures
    {
        public const string SP_SAVE = "SPADM_COMPANY_MST_SAVE";
        public const string SP_DELETE = "SPADM_COMPANY_MST_DELETE";
        public const string SP_GET_KV = "SPADM_COMPANY_MST_GET_KV";
        public const string SP_GET_LIST = "SPADM_COMPANY_MST_GET_LIST";
        public const string SPADM_COMPANY_MPG_GET_LIST = "SPADM_COMPANY_MPG_GET_LIST";
        public const string GET_COMMONDROPDOWNLIST = "SPHRM_CONST_MST_GET_KV";

        public const string SPCRM_CUST_ITEM_GROUP_SAVE = "SPCRM_CUST_ITEM_GROUP_SAVE";
        public const string SPCRM_CUST_ITEM_GROUP_DELETE = "SPCRM_CUST_ITEM_GROUP_DELETE";
        public const string SPCRM_CUST_ITEM_GROUP_GET_KV = "SPCRM_CUST_ITEM_GROUP_GET_KV";

        public const string SPPRD_DEPT_LOC_AUTO = "SPPRD_DEPT_LOC_AUTO";
        public const string SAVESTORELOC = "SPADM_DEPT_MST_SAVE";
        public const string GETSTORELOCLIST = "SPADM_DEPT_MST_GET_LIST";
        public const string SPADM_DEPT_MST_DTL_GET = "SPADM_DEPT_MST_DTL_GET";

        // Dashboard Setup
        public const string SPADM_DASHBOARD_CFG_SAVE = "SPADM_DASHBOARD_CFG_SAVE";
        public const string SPADM_DASHBOARD_ITEM_DTL_GET = "SPADM_DASHBOARD_ITEM_DTL_GET";
        public const string SPADM_DASHBOARD_CFG_GET_LIST = "SPADM_DASHBOARD_CFG_GET_LIST";
        public const string SPADM_DASHBOARD_GET_XML = "SPADM_DASHBOARD_GET_XML";
        public const string SPADM_DASHLET_CFG_GET_KV = "SPADM_DASHLET_CFG_GET_KV";
        public const string SPADM_REPORT_CFG_GET_KV = "SPADM_REPORT_CFG_GET_KV";
        public const string SPADM_DASHBOARD_CFG_DELETE = "SPADM_DASHBOARD_CFG_DELETE";


        //Dashboard User Mapping

        public const string SPADM_DASHLET_USER_GRP_MAP_SAVE = "SPADM_DASHLET_USER_GRP_MAP_SAVE";
        public const string SPADM_DASHLET_USER_GRP_MAP_GET_XML = "SPADM_DASHLET_USER_GRP_MAP_GET_XML";

        // SEND SMS
        public const string SPADM_SMS_QUEUE_TRX_SAVE = "SPADM_SMS_QUEUE_TRX_SAVE";
        public const string SPADM_SMS_QUEUE_TRX_EMP_GET = "SPADM_SMS_QUEUE_TRX_EMP_GET";
        public const string SPADM_SMS_QUEUE_TRX_GET_LIST = "SPADM_SMS_QUEUE_TRX_GET_LIST";
        public const string SPADM_SMS_QUEUE_TRX_DELETE = "SPADM_SMS_QUEUE_TRX_DELETE";

        //Shift Master
        public const string SPADM_SHIFT_GET_KV = "SPADM_SHIFT_GET_KV";
        public const string SPADM_SHIFT_MST_SAVE = "SPADM_SHIFT_MST_SAVE";
        public const string SPADM_SHIFT_MST_DELETE = "SPADM_SHIFT_MST_DELETE";
        public const string SPADM_SHIFT_MST_GET_LIST = "SPADM_SHIFT_MST_GET_LIST";

        //Cost Center Master
        public const string SPADM_COST_CENTER_MST_SAVE = "SPADM_COST_CENTER_MST_SAVE";
        public const string SPADM_COST_CENTER_MST_GET_KV = "SPADM_COST_CENTER_MST_GET_KV";
        public const string SPADM_COST_CENTER_MST_GET_LIST = "SPADM_COST_CENTER_MST_GET_LIST";
        public const string SPFIN_COA_COST_CENTER_MPG_GET_LIST = "SPFIN_COA_COST_CENTER_MPG_GET_LIST";
        public const string SPADM_COST_CENTER_MST_ACTIVATE = "SPADM_COST_CENTER_MST_ACTIVATE";
        public const string SPADM_CONST_MST_GET_KV = "SPADM_CONST_MST_GET_KV";
        public const string SPADM_COST_CENTER_MST_DELETE = "SPADM_COST_CENTER_MST_DELETE";
        public const string SPADM_COST_CENTER_MST_AUTO = "SPADM_COST_CENTER_MST_AUTO";

        //Activity
        public const string SPPRD_EMP_ACTIVITY_MST_SAVE = "SPPRD_EMP_ACTIVITY_MST_SAVE";
        public const string SPPRD_EMP_ACTIVITY_MST_GET_LIST = "SPPRD_EMP_ACTIVITY_MST_GET_LIST"; // all activity list sp
        public const string SPPRD_EMP_ACTIVITY_MST_GET_KV = "SPPRD_EMP_ACTIVITY_MST_GET_KV";//single activity get sp using activity pk
        public const string SPPRD_EMP_ACTIVITY_MST_GET_DELETE = "SPPRD_EMP_ACTIVITY_MST_GET_DELETE";//-- activity delete sp using activity pk
        public const string SPPRD_EMP_ACTIVITY_MST_ACTIVATE = "SPPRD_EMP_ACTIVITY_MST_ACTIVATE";
        public const string SPPRD_EMP_ACTIVITY_MST_AUTO= "SPPRD_EMP_ACTIVITY_MST_AUTO";

        //GST Classification
        public const string SPFIN_GST_CLASS_MST_SAVE = "SPFIN_GST_CLASS_MST_SAVE";
        public const string SPFIN_GST_CLASS_MST_GET_LIST = "SPFIN_GST_CLASS_MST_GET_LIST";
        public const string SPFIN_GST_CLASS_MST_GET_KV = "SPFIN_GST_CLASS_MST_GET_KV";
        public const string SPFIN_GST_CLASS_MST_DELETE = "SPFIN_GST_CLASS_MST_DELETE";

        //Invoice Type Master
        public const string SPFIN_INVOICE_GST_TYPE_MST_SAVE = "SPFIN_INVOICE_GST_TYPE_MST_SAVE";
        public const string SPFIN_INVOICE_GST_TYPE_MST_GET_KV = "SPFIN_INVOICE_GST_TYPE_MST_GET_KV";
        public const string SPFIN_INVOICE_GST_TYPE_MST_GET_LIST = "SPFIN_INVOICE_GST_TYPE_MST_GET_LIST";
        public const string SPFIN_INVOICE_GST_TYPE_MST_DELETE = "SPFIN_INVOICE_GST_TYPE_MST_DELETE";

        //Port Master
        public const string SPADM_PORT_MST_SAVE = "SPADM_PORT_MST_SAVE";
        public const string SPADM_PORT_MST_GET_LIST = "SPADM_PORT_MST_GET_LIST";
        public const string SPADM_PORT_MST_DELETE = "SPADM_PORT_MST_DELETE";
        public const string SPADM_PORT_MST_GET_KV = "SPADM_PORT_MST_GET_KV";

        //Inventory Location Master
        public const string SPADM_DEPT_INV_LOC_SAVE = "SPADM_DEPT_INV_LOC_SAVE";
        public const string SPADM_DEPT_INV_LOC_GET_LIST = "SPADM_DEPT_INV_LOC_GET_LIST";
        public const string SPADM_DEPT_MST_GET_KV = "SPADM_DEPT_MST_GET_KV";
        public const string SPADM_DEPT_MST_GET = "SPADM_DEPT_MST_GET";
        public const string SPADM_DEPT_MST_DELETE = "SPADM_DEPT_MST_DELETE";

        //Fund Requisition Dept
        public const string SPINV_DEPT_FUND_REQ_WKF_SAVE="SPINV_DEPT_FUND_REQ_WKF_SAVE" ;
        public const string SPINV_DEPT_FUND_REQ_GET_LIST = "SPINV_DEPT_FUND_REQ_GET_LIST";
        public const string SPINV_DEPT_FUND_REQ_GET = "SPINV_DEPT_FUND_REQ_GET";
        public const string SPINV_DEPT_FUND_REQ_DELETE = "SPINV_DEPT_FUND_REQ_DELETE";
        public const string SPINV_DEPT_FUND_REQ_AUTO="SPINV_DEPT_FUND_REQ_AUTO";
        public const string SPINV_DEPT_FUND_REQ_RPT="SPINV_DEPT_FUND_REQ_RPT";

        //Document Revision
        public const string SPADM_DOC_REVISION_RPT_MST_AUTO = "SPADM_DOC_REVISION_RPT_MST_AUTO";
        public const string SPADM_DOC_REVISION_RPT_MAP_GET_KV = "SPADM_DOC_REVISION_RPT_MAP_GET_KV";
        public const string SPADM_DOC_REVISION_RPT_MAP_SAVE = "SPADM_DOC_REVISION_RPT_MAP_SAVE";
        public const string SPADM_DOC_REVISION_RPT_MAP_LIST = "SPADM_DOC_REVISION_RPT_MAP_LIST";
        public const string SPADM_DOC_REVISION_RPT_MAP_DELETE = "SPADM_DOC_REVISION_RPT_MAP_DELETE";

        //Production Batch 
        public const string SPADM_PRD_BATCH_NO_SAVE = "SPADM_PRD_BATCH_NO_SAVE";
        public const string SPADM_PRD_BATCH_NO_LIST = "SPADM_PRD_BATCH_NO_LIST";
        public const string SPADM_PRD_BATCH_NO_DELETE = "SPADM_PRD_BATCH_NO_DELETE";
        public const string SPADM_PRD_BATCH_NO_GET_KV = "SPADM_PRD_BATCH_NO_GET_KV";
        public const string SPADM_PRD_BATCH_NO_INACTIVE = "SPADM_PRD_BATCH_NO_INACTIVE";
    }

}
