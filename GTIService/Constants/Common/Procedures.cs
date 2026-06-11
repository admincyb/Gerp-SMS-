using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Common
{
    public class Procedures
    {
        public const string SPADM_CONFIG_MST_GET_KV = "SPADM_CONFIG_MST_GET_KV";
        public const string SP_GETCOUNTRY = "SPADM_COUNTRY_MST_GET_KV";
        public const string SP_GETSTATE = "SPADM_STATE_MST_GET_KV";
        public const string SPFIN_COA_MST_GET_KV = "SPFIN_COA_MST_GET_KV";
        public const string SP_GETCURRENCY = "SPADM_CURRENCY_MST_GET_KV";
        public const string SP_GETDEPARTMENT = "SPADM_BASE_DEPT_GET_KV";
        public const string GETDEPARTMENTDETAILS = "SPADM_DEPT_MST_GET_TREE";
        public const string GETUSERGROUPDETAILS = "ADM_USER_GROUP_KV";
        public const string GETUOMLIST = "SPINV_UOM_MST_GET_KV";
        public const string SPPRJPROJECTINFAUTO = "SpPrjProjectInfAuto";

        public const string SP_GETDEPARTMENTNAME = "SpwkfDepartmentGetKV";
        public const string GETTEMPLATEDETAILS = "";
        public const string GETTRANSACTIONSTATUS = "SpWkfTransactionProcStatusGet";
        public const string SP_GETDEPARTMENTCATEGORIES = "SPADM_CONFIG_MST_GET_KV";
        public const string SP_GETDEPARTMENTCATEGORIESBYID = "SPADM_DEPT_CATEGORY_GET_KV";

        public const string COUNTRYSEARCHAUTOCOMPLETE = "SPADM_COUNTRY_MST_AUTO";
        public const string STATESEARCHAUTOCOMPLETE = "SPADM_STATE_MST_AUTO";
        public const string CURRENCYSEARCHAUTOCOMPLETE = "SPADM_CURRENCY_MST_AUTO";


        public const string GETINITIALTASKPERMISSION = "SPWKF_INIT_TASK_PERM_GET";

        public const string SP_GETPROCESS = "SPWKF_USER_PROC_AUTO";
        public const string GETWORKFLOWCOMMENT = "SPADM_COMMENT_HDR_GET";
        public const string SAVEWORKFLOWCOMMENT = "SPADM_COMMENT_HDR_SAVE";


        public const string GETSHIFT = "SPADM_SHIFT_MST_GET_KV";
        public const string GETLINES = "SPPRD_LINE_MST_GET_KV";
        public const string GETPRODUCT = "SPPRD_PRODUCT_MST_GET_KV";
        public const string GETPRODUCTDETAILS = "SPPRD_PRODUCT_MST_GET_DTL";
        public const string GETPLANS = "SPPRD_PLAN_MST_GET_KV";
        public const string GETEMPLOYEES = "SPADM_USER_GET_KV";
        public const string GETCOMPOUNDBATCH = "SPPRD_COMP_BATCH_GET_KV";
        public const string GETMENUAUTOCOMPLETE = "SPADM_USER_PAGE_GET";
        public const string GETIISUINGYOLIST = "SPINV_ITEM_EXT_ISS_RCV_GET_KV";
        public const string GetMailTransactionDtl = "SpWkfTransactionMailGet";
        public const string GetIntimationMailDtl = "SpWkfTransactionIntMailGet";
        public const string GET_TRX_DOC_NO = "SPADM_TRX_DOC_NO_GENERATE";
        public const string GET_USERCUSTOMER = "SPCRM_CUSTOMER_USER_GET";

        public const string GETCHECKLISTTEMP = "SPPRD_CHECK_LIST_TEMP_GET";
        public const string GETTEMPLATECHECKLIST = "SPPRD_CHECK_LIST_TEMP_GET_KV";

        public const string SP_GETPOCREATOR = "SPHRM_EMP_ROLE_GET";

        //WorkFlow Page Task Permission
        public const string GetPageTaskPermission = "SpWkfTransactionPermissionGet";

        //Save Inbox Message Time
        public const string SaveMessageInbox = "SpWkfUserMstInboxSave";

        public const string GET_BINCARD_PRODUCT_REPORT = "SPPRD_BIN_CARD_PRODUCT_RPT";
        public const string SPADM_FOLDER_MST_SAVE = "SPADM_FOLDER_MST_SAVE";
        public const string SPADM_FOLDER_MST_DELETE = "SPADM_FOLDER_MST_DELETE";
        public const string SPADM_CONST_LOC_GET_KV = "SPADM_CONST_LOC_GET_KV";
        public const string SPCRM_CUSTOMER_MST_CRM_LEAD_MST_TRFER_SAVE = "SPCRM_CUSTOMER_MST_CRM_LEAD_MST_TRFER_SAVE";
        public const string SPCRM_PRODUCT_MST_INV_ITEM_MST_TRFER_SAVE = "SPCRM_PRODUCT_MST_INV_ITEM_MST_TRFER_SAVE"; 
        public const string SPADM_CONST_MST_GET_KV = "SPADM_CONST_MST_GET_KV";
        public const string SPINV_WORK_ORDER_HDR_AUTO = "SPINV_WORK_ORDER_HDR_AUTO"; 

        //Material Issue (External Material Issue Multiple)
        public const string SPINV_ITEM_EXT_ISS_RCV_SUB_GET_KV = "SPINV_ITEM_EXT_ISS_RCV_SUB_GET_KV";
        public const string SPASSET_DETAILS_SRM_GET = "SPASSET_DETAILS_SRM_GET";

        //Work Flow Level Change Page
        public const string SPADM_DEPT_MST_GET_KV = "SPADM_DEPT_MST_GET_KV";
        public const string SpwkfProcessMstGetKV = "SpwkfProcessMstGetKV";
        public const string SpWkfSequenceTaskGetList = "SpWkfSequenceTaskGetList";
        public const string SpWkfSequenceTaskActionGetList = "SpWkfSequenceTaskActionGetList";
        public const string SpWkfSequenceConfigSave = "SpWkfSequenceConfigSave";
        public const string SpWkfSequenceTaskActionList = "SpWkfSequenceTaskActionList";
        public const string SpWkfSequencePendTrxList = "SpWkfSequencePendTrxList";

        public const string SPADM_CONST_MST_GET_AUTO="SPADM_CONST_MST_GET_AUTO";
        public const string SPADM_DEPT_DMG_GET = "SPADM_DEPT_DMG_GET";
        public const string SPINV_ITEM_CATEGORY_ASSET_GET = "SPINV_ITEM_CATEGORY_ASSET_GET";
    }
}
