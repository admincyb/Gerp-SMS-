using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Store
{
    public class Procedures
    {
        public const string GETSTORELISTBYSRH = "SpGTrsHrc";
        public const string DELETESTOREDETAILS = "SpDRmTldTrs";
        public const string GETSTORETYPE = "SpGEptTldTrs";
        public const string GETSEARCHVALUE = "SpGTimeOta";
        public const string SAVESTOREDETAILS = "SpSRmTldTrs";
        public const string GETSTOCKDETAILS = "SPINV_STK_DTL_RPT";
        public const string GETDAMAGETYPE = "SPINV_ITEM_DMG_TYPE_MST_GET_KV";
        public const string GETCUMILATIVESTOCKDETAILS = "SPINV_STK_TRX_DTL_RPT";
        public const string SPINV_EMI_GRN_AUTO = "SPINV_EMI_GRN_AUTO";
        public const string SPADM_DEPT_MST_GET_KV = "SPADM_DEPT_MST_GET_KV";
    }
    public class Procedures_Requisition 
    {
        public const string GETSEARCHREQUISITIONVALUE = "SPINV_ITEM_MST_AUTO";
        public const string GETSTORES = "SPINV_STORE_DEPT_GET_KV";
        public const string GETSRSNO = "SPINV_ITEM_REQUEST_NO_GET";
        public const string SAVEREQUISITIONXML = "SPINV_ITEM_REQUEST_SAVE";
        public const string GETSEARCHVALUE = "SPINV_ITEM_REQUEST_AUTO";
        public const string GETREQUISITIONLISTBYSRH = "SPINV_ITEM_REQUEST_HDR_LIST";
        public const string DELETEREQUISITIONDETAILS = "SPINV_ITEM_REQUEST_HDR_DELETE";
        public const string GETREQUISITIONDETAILSBYREQUISITIONID = "SPINV_ITEM_REQUEST_GET";

        public const string GETREQUISITIONREPORTBYREQID = "SPINV_ITEM_REQUEST_GET_RPT";
        public const string GETSRSDETAILS = "SPINV_ITEM_REQUEST_VIEW";
        public const string SPADMDEPTMSTGETKV = "SPADM_DEPT_MST_GET_KV";
    }

    public class Procedure_MaterialConsumption
    {
        public const string SAVECONSUMPTIONXML = "SPINV_ITEM_CONS_SAVE";
        public const string GETITEMCONSUMPTION = "SPINV_ITEM_CONS_GET";
        public const string GETCONSUMPTIONLIST = "SPINV_ITEM_CONS_GET_LIST";
        public const string DELETECONSUMPTION = "SPINV_ITEM_CONS_DELETE";
        public const string GETICHNO = "SPINV_ITEM_CONS_NO_GET";
        public const string GETCONSREPORT = "SPINV_ITEM_CONS_RPT";
        public const string GETSEARCHVALUE = "SPINV_ITEM_CONS_AUTO";
    }
    public class Procedure_ExternalMaterialIssue
    {
        public const string GETNEXTNO = "SPINV_ITEM_CONS_NO_GET";
        public const string SAVEISSUEXML = "SPINV_ITEM_EXT_ISS_RCV_SAVE";

        public const string SAVEISSUEWKFXML = "SPINV_ITEM_EXT_ISS_RCV_WKF_SAVE";
        public const string SAVEEMIDAMAGE = "SPINV_ITEM_EXT_ISS_AFR_DMG_RCV_WKF_SAVE";

        public const string GETMATERIALISSUEDTL = "SPINV_ITEM_EXT_ISS_RCV_GET";
        public const string GETDTLFROMCRDR = "SPINV_ITEM_EXT_ISS_RCV_CRDR_GET";        
        public const string GETMATERIALISSUELIST = "SPINV_ITEM_EXT_ISS_RCV_GET_LIST";

        public const string GETMATERIALISSUELISTWKF = "SPINV_ITEM_EXT_RCV_GET_LIST";

        public const string GETMATERIALISSUEAUTO = "SPINV_ITEM_EXT_ISS_RCV_AUTO";
        public const string DELETEMATERIALISSUE = "SPINV_ITEM_CONS_DELETE";
        public const string MATERIALISSUEREPORT = "SPINV_ITEM_EXT_ISS_RCV_RPT";
        public const string STOREREQUISITIONREPORT = "SPINV_ITEM_EXT_ISS_RCV_RPT";
        public const string SPADM_APP_CONFIG_MST_GET = "SPADM_APP_CONFIG_MST_GET";
        public const string SPINV_EMI_GRN_DTL_GET = "SPINV_EMI_GRN_DTL_GET";
        public const string EMIMULTIPLEREPORT = "SPINV_ITEM_EXT_ISS_RCV_MULTIPLE_RPT";
    }


    public class Procedures_GoodsReceiptNote 
    {
        public const string GETGOODRECEIPT = "SPINV_GRN_GET";
        public const string SAVEGOODRECEIPT = "SPINV_GRN_SAVE";
        public const string GETPENDINGSEARCH = "SPPUR_ORDER_ITEMS_AUTO";
        public const string GETPENDINGPO = "SPPUR_ORDER_ITEMS_GET";
        public const string GETPREVIOUSGRN = "SPINV_GRN_DTL_LIST";
        public const string GETGRNLIST = "SPINV_GRN_LIST";
        public const string GETGRNNO = "SPINV_GRN_NO_GET";
        public const string GRNTAUTO = "SPINV_GRN_AUTO";
        public const string DELETEGRN = "SPINV_GRN_DELETE";
        public const string GETGRNDTLSVIEW = "SPINV_GRN_ITEMS_VIEW";
        public const string SPINV_GRN_RPT = "SPINV_GRN_RPT";
    }

    public class Procedures_StoreMaterialAccept
    {
        public const string GETPENDINGSEARCH = "SPINV_ITEM_ISSUE_ITEMS_AUTO";
        public const string GETPENDINGSI = "SPINV_ITEM_ISSUE_ITEMS_GET";
        public const string SAVEMATERIALACCEPT = "SPINV_ITEM_ACCEPT_SAVE";
        public const string SPINV_ITEM_ACCEPT_CONVERSION_SAVE = "SPINV_ITEM_ACCEPT_CONVERSION_SAVE"; 
        public const string SPINV_ITEM_ISSUE_DTL_GET = "SPINV_ITEM_ISSUE_DTL_GET";
        public const string GETMATERIALACCEPT = "SPINV_ITEM_ACCEPT_GET";
        public const string GETMATERIALACCEPTLIST = "SPINV_ITEM_ACCEPT_GET_LIST";
        public const string SPINV_ITEM_ACCEPT_CONVERT_ITEM_GET = "SPINV_ITEM_ACCEPT_CONVERT_ITEM_GET"; 
        public const string GETMANO = "SPINV_ITEM_ACCEPT_NO_GET";
        public const string GETMATAUTO = "SPINV_ITEM_ACCEPT_AUTO";
        public const string DELETEMATERIALACCEPT = "SPINV_ITEM_ACCEPT_DELETE";
        public const string GETPREVIOUSMA = "SPINV_ITEM_ACCEPT_DTL_LIST";
        public const string SPINV_ITEM_ACCEPT_CONVERSION_DELETE = "SPINV_ITEM_ACCEPT_CONVERSION_DELETE"; 
    }

    public class Procedures_StoreMaterialIssue
    {
        public const string GETMATERIALISSUE = "SPINV_ITEM_ISSUE_GET";
        public const string GETMATERIALISSUEREPORT = "SPINV_MATERIAL_ISSUE_RPT";
        public const string GETMRISSUE = "SPINV_MATERIAL_ISSUE_GET";
        public const string ITEM_REQUEST_DTL_GET = "SPINV_ITEM_REQUEST_DTL_GET";
        public const string SAVEMATERIALISSUE = "SPINV_ITEM_ISSUE_SAVE";
        public const string SAVEMATERIALISSUEWKF = "SPINV_ITEM_ISSUE_WKF_SAVE";
        public const string SPINV_MATERIAL_ISSUE_SAVE = "SPINV_MATERIAL_ISSUE_SAVE";
        public const string SPINV_MATERIAL_ISSUE_INTER_PLANT_SAVE = "SPINV_MATERIAL_ISSUE_INTER_PLANT_SAVE";
        public const string GETPENDINGSEARCH = "SPINV_ITEM_REQUESTED_AUTO";
        public const string SPINV_WORK_ORDER_ITEM_AUTO = "SPINV_WORK_ORDER_ITEM_AUTO"; 
        public const string GETPENDINGSRS = "SPINV_ITEM_REQUESTED_LIST_GET";
        public const string SPINV_WORK_ORDER_ITEM_LIST_GET = "SPINV_WORK_ORDER_ITEM_LIST_GET";

        public const string GETPENDINGMR = "SPINV_MATERIAL_ISSUE_PEND_GET_LIST";

        public const string GETPREVIOUSSRS = "SPINV_ITEM_ISSUE_DTL_LIST";
        public const string GETMINO = "SPINV_ITEM_ISSUE_NO_GET";
        public const string DELETEMATERIALISSUE = "SPINV_ITEM_ISSUE_DELETE";
        public const string GETMILIST = "SPINV_ITEM_ISSUE_LIST";
        public const string MILISTAUTO = "SPINV_ITEM_ISSUE_AUTO";
        public const string SPINV_ITEM_CONS_RATE_DIFF_GET = "SPINV_ITEM_CONS_RATE_DIFF_GET";
        public const string SPINV_ITEM_CONS_RATE_DIFF_WKF_SAVE = "SPINV_ITEM_CONS_RATE_DIFF_WKF_SAVE";
        public const string SPINV_ITEM_CONS_RATE_DIFF_GET_LIST = "SPINV_ITEM_CONS_RATE_DIFF_GET_LIST";
        public const string SPINV_ITEM_CONS_RATE_DIFF_GET_KV = "SPINV_ITEM_CONS_RATE_DIFF_GET_KV";
        public const string SPINV_ITEM_CONS_RATE_DIFF_AUTO = "SPINV_ITEM_CONS_RATE_DIFF_AUTO";
    }


    public class Procedures_GoodsInspectionNote
    {
        public const string GENERATEGINNO = "SPINV_GIN_NO_GET";

        public const string GETPENDINGSEARCH = "SPINV_GRN_ITEMS_AUTO";

        public const string SPINV_GRN_DTL_GET = "SPINV_GRN_DTL_GET";

        public const string GETPENDINGGRNITEMDTLS = "SPINV_GRN_ITEMS_GET";
        public const string SPINV_GIN_DTL_GET = "SPINV_GIN_DTL_GET";

        public const string GINDTLSAUTO = "SPINV_GIN_AUTO";

        public const string DELETEGINDTLS = "SPINV_GIN_DELETE";


        public const string GINDETAILSLIST = "SPINV_GIN_LIST";

        public const string GETGINDTLS = "SPINV_GIN_GET";


        public const string SAVEGINDETAILS = "SPINV_GIN_SAVE";

        public const string GIN_ALREADY_INSPECTED_DTLS = "SPINV_GIN_DTL_GET_LIST";



        public const string GETINV_GIN_RPT = "SPINV_GIN_RPT";
       

       
    }
    public class Procedure_StoreAudit
    {
        public const string GETSTOREITEMDETAILS = "SPINV_STK_ITM_DTL";
        public const string SAVESTOREAUDIT = "SPINV_STK_AUD_SAVE";
        public const string GETSTOREAUDITNO = "SPINV_STK_AUD_NO_GET";
        public const string GETSTOREAUDITDETAILS = "SPINV_STK_AUD_GET";
        public const string GETSTOREAUDITREPORT = "SPINV_STK_AUD_RPT";
        public const string GETSTORECATEGORYITEMDETAILS = "SPINV_STK_AUD_ITM_DTL";
    }

    public class Procedure_StockAdjustment
    {
        public const string GETSTOCKADJUSTMENT = "SPINV_STK_ADJ_GET";
        public const string SAVESTOCKADJUSTMENT = "SPINV_STK_ADJ_SAVE";
    
        public const string GETEVALAPPIDFORREFID = "SPINV_STK_ADJ_REF_ID_GET";
        public const string UPDATEEVALREFID = "SPINV_STK_ADJ_REF_ID_SAVE";


    }
    public class Procedure_NewItemRequest
    {
        public const string GETNEWITEMREQUESTLISTBYSRH = "SPINV_NEW_ITEM_REQ_GET_LIST";
        public const string GETNIRNO = "SPINV_NEW_ITEM_REQ_NO_GET";
        public const string GETNEWITEMREQUESTLIST = "SPINV_NEW_ITEM_REQ_GET_LIST";
        public const string SAVENEWITEMREQUEST = "SPINV_NEW_ITEM_REQ_SAVE";
        public const string GETNEWITEMCHECKLIST = "SPINV_NEW_ITEM_CHECK_LIST";
        public const string DELETENEWITEMREQUEST = "SPINV_NEW_ITEM_REQ_DELETE";
        public const string GETSEARCHVALUE = "SPINV_NEW_ITEM_REQ_AUTO";
        public const string GETNEWITEMREQUESTKV = "SPINV_NEW_ITEM_REQ_GET_KV";
    }
}

