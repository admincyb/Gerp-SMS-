using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Store
{
    public class Parameters
    {
         
        public const string STOREPK = "pKp";
        public const string STORETYPEPK = "pEpt";
        public const string STORENAME = "pNem";
        public const string STOREMODIFIED = "pMyB";
        public const string STORESTATUS = "pTts";
        public const string STOREID = "pLocNem";
        public const string STORESRCH = "pLocLuv";
        public const string STORERETURNVALUE = "pRteVla";
        public const string STORE = "P_DPT_PK";
        public const string ITEM = "P_ITM_PK";
        public const string PRETVAL = "P_RET_VAL";
    }
    public class Parameters_Requisition
    {
        public const string STORESRCHBY = "@pLocNem";
        public const string STORESRCHVALUE = "@ITM_CODE";
        public const string STOREMODIFIEDBY = "RMSP";
        public const string SRSRETURNVALUE = "MRH_PK";
        public const string SRSDPTTYPE = "DPT_TYPE";
        public const string STOREREQUISITIONXML = "P_ITM_REQ_XML";
        public const string STORERETURNVALUE = "P_RTN_VAL";
        public const string ITEMCODE = "ITM_CODE";
        public const string REQUISITIONSRCHBY = "P_FLD_NAME";
        public const string REQUISITIONSRCHVALUE = "P_VALUE";
        public const string REQUISITIONLSTSTATUS = "P_SER_NAME";
        public const string REQUISITIONLSTSRCH = "P_SER_VAL";
        public const string REQUISITIONRETURNVALUE = "P_RET_VAL";
        public const string REQUISITIONID = "MRH_PK";
        public const string MRDPK = "MRH_PK";
        public const string SRSNO = "P_RET_NO";
        public const string P_MRH_DEPT = "P_MRH_DEPT";

    }
    public class Parameters_MaterialComsumption
    {
        public const string CONSUMPTIONXML = "P_XML";
        public const string CONSUMPTIONRETURNVALUE = "P_RET_VAL";
        public const string SRSNO = "P_RET_NO";
        public const string ICHPK = "P_ICH_PK";

        public const string REQUISITIONSRCHBY = "P_FLD_NAME";
        public const string REQUISITIONSRCHVALUE = "P_VALUE";
        public const string REQUISITIONLSTSTATUS = "P_SER_NAME";
        public const string REQUISITIONLSTSRCH = "P_SER_VAL";
        public const string REQUISITIONRETURNVALUE = "P_RET_VAL";

    }
    public class Parameters_ExternalMaterialIssue
    {
        public const string MATERIALISSUEXML = "P_XML";
        public const string MATERIALISSUERETURNVALUE = "P_RET_VAL";
        public const string P_RET_REF_PK = "P_RET_REF_PK";
        public const string MISNO = "P_RET_NO";
        public const string ICHPK = "P_ICH_PK";
        public const string CDH_PK = "P_CDH_PK";
        public const string P_RET_VCH_NO = "P_RET_VCH_NO";

        public const string REQUISITIONSRCHBY = "P_FLD_NAME";
        public const string REQUISITIONSRCHVALUE = "P_VALUE";
        public const string REQUISITIONLSTSTATUS = "P_SER_NAME";
        public const string REQUISITIONLSTSRCH = "P_SER_VAL";
        public const string REQUISITIONRETURNVALUE = "P_RET_VAL";
        public const string ISSUINGTYPE = "P_ICH_ISS_RCV_TYPE";
        public const string DESPATCHED = "P_DESPATCHED";
        public const string TRANSACTIONTYPE = "P_TRX_TYPE";
        public const string PAGE_URL = "P_PAGE_URL";
        public const string USER_PK = "P_USER_PK";
        public const string P_HAS_WKF = "P_HAS_WKF";
        public const string STREQICHPK = "P_ICH_PK";
        public const string P_SEARCHVAL = "P_SEARCHVAL";
        public const string P_DPT_TYPE = "P_DPT_TYPE";
        public const string FROMDATE = "P_FROM_DT";
        public const string TODATE = "P_TO_DT";
        public const string P_ICH_NO = "P_ICH_NO";
        public const string P_ICH_STATUS = "P_ICH_STATUS";
        public const string P_ICH_ISS_RCV_TYPE = "P_ICH_ISS_RCV_TYPE";
        public const string P_ICH_ISS_RCV_PK = "P_ICH_ISS_RCV_PK";
        public const string P_ICH_DEPT = "P_ICH_DEPT";
        public const string P_ICH_LOT_NO = "P_ICH_LOT_NO";
        public const string P_ICH_REF_NO = "P_ICH_REF_NO";

        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_ACF_SETTING = "P_ACF_SETTING";
        public const string P_ACF_DATA = "P_ACF_DATA";
        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_GRH_PK = "P_GRH_PK";
        public const string P_ICH_ITEM = "P_ICH_ITEM";
        public const string P_ICH_COMPANY = "P_ICH_COMPANY";
        public const string P_ICH_ITEM_CATEGORY = "P_ICH_ITEM_CATEGORY";
        public const string P_ICH_ISS_RCV_SUB_TYPE = "P_ICH_ISS_RCV_SUB_TYPE";
        public const string P_ICH_MENU_TYPE = "P_ICH_MENU_TYPE";



    }


    public class Parameters_GoodsReceiptNote
    {
        public const string GRNPK = "P_GRH_PK";
        public const string P_VRM_ROLE = "P_VRM_ROLE"; 
        public const string GOODRECEIPTPK = "GRH_PK";
        public const string P_AST_PK = "P_AST_PK";
        public const string GOODSRECEIPTXML = "P_GRH_XML";
        public const string GRNNO = "P_RET_NO";
        public const string RETVAL = "P_RET_VAL";
        public const string SEARCHBY = "P_FLD_NAME";
        public const string SEARCHVALUE = "P_VALUE";
        public const string BIZUNIT = "P_BIZUNIT";
        public const string VENDOR = "P_VENDOR";
        public const string SEARCHNAME = "P_SER_NAME";
        public const string P_POH_PK = "P_POH_PK";
        public const string SEARCHVAL = "P_SER_VAL";
        public const string PAGENO = "P_PAGE_NO";
        public const string PAGESIZE = "P_PAGE_SIZE";
        public const string SORTBY = "P_SORT_BY";
        public const string SORTDIRC = "P_SORT_DIR";
        public const string FIELDS = "P_FIELDS";
        public const string USERPK = "P_USER_PK";
        public const string PROCESSID = "P_PROC_ID";
        public const string FROMDATE = "P_FROM_DT";
        public const string TODATE = "P_TO_DT";
        public const string SEARCHCORR = "P_VENDOR";
        public const string POPK = "POH_PK";
        public const string DEPARTMENT = "P_DEPT";
        public const string SHIP_DEPT = "P_SHIP_DEPT";
        public static string P_GRH_NO = "P_GRH_NO";
        public static string P_VEN_PK="P_VEN_PK";
        public static string P_POH_NO = "P_POH_NO";
        public static string P_VEN_REF_NO = "P_VEN_REF_NO";
        public static string P_GRH_COMPANY = "P_GRH_COMPANY";
    }
    public class Parameters_StoreMaterialIssue
    {
        public const string MATERIALISSUEPK = "MIH_PK";
        public const string P_AST_PK = "P_AST_PK";
        public const string MINO = "P_RET_NO"; 
        public const string MATERIALISSUEXML = "P_MIH_XML";
        public const string RETVAL = "P_RET_VAL";
        public const string RET_REF_PK = "P_RET_REF_PK";
        public const string SEARCHBY = "P_FLD_NAME";
        public const string SEARCHVALUE = "P_VALUE";
        public const string BIZUNIT = "P_BIZUNIT";
        public const string STORE = "P_DEPT_STR";
        public const string DEPARTMENT = "P_DEPT";
        public const string SEARCHNAME = "P_SER_NAME";
        public const string SEARCHVAL = "P_SER_VAL";
        public const string PAGENO = "P_PAGE_NO";
        public const string PAGESIZE = "P_PAGE_SIZE";
        public const string PAGEURL = "P_PAGE_URL";
        public const string SORTBY = "P_SORT_BY";
        public const string SORTDIRC = "P_SORT_DIR";
        public const string FIELDS = "P_FIELDS";
        public const string USERPK = "P_USER_PK";
        public const string PROCESSID = "P_PROC_ID";
        public const string FROMDATE = "P_FROM_DT";
        public const string TODATE = "P_TO_DT";
        public const string SEARCHCORR = "P_DEPT_STR";
        public const string SEARCHCORR1 = "P_DEPT";
        public const string SRSPK = "MRH_PK";
        public const string MIPK = "P_MIH_PK";
        public const string ICHPK = "P_ICH_PK";
        public const string MRH_PK = "P_MRH_PK";
        public const string P_IS_COMPLETED = "P_IS_COMPLETED"; 
        public const string PRH_PK = "P_PRH_PK";
        public const string P_RET_LOCK_DATE = "P_RET_LOCK_DATE";
        public const string M_ICH_PK = "ICH_PK";
        public const string MIH_STATUS = "P_MIH_STATUS";
        public const string P_XML = "P_XML";
        public const string P_DEPT_PK = "P_DEPT_PK";
        public const string P_DATE = "P_DATE";
        public const string P_IRH_PK = "P_IRH_PK";
        public const string P_IRH_DATE = "P_IRH_DATE";
        public const string P_PRH_TRX_TYPE = "P_PRH_TRX_TYPE";
        public const string P_ICH_TRX_TYPE = "P_ICH_TRX_TYPE";
        public const string P_ISSUE_DEPT = "P_ISSUE_DEPT";
    }

    public class Parameters_StoreMaterialAccept
    {
        public const string MAHPK = "MAH_PK";
        public const string P_MAH_PK = "P_MAH_PK";
        public const string MATERIALACCEPTPK = "P_MIH_PK";
        public const string MATERIALACCEPTXML = "P_MAH_XML";
        public const string MIHPK = "MIH_PK";
        public const string ACCPTNO = "P_RET_NO";
        public const string RETVAL = "P_RET_VAL";
        public const string MIPK = "P_MIH_PK";
        public const string SEARCHBY = "P_FLD_NAME";
        public const string SEARCHVALUE = "P_VALUE";
        public const string BIZUNIT = "P_BIZUNIT";
        public const string STORE = "P_DEPT_STR";
        public const string SEARCHNAME = "P_SER_NAME";
        public const string SEARCHVAL = "P_SER_VAL";
        public const string PAGENO = "P_PAGE_NO";
        public const string PAGESIZE = "P_PAGE_SIZE";
        public const string SORTBY = "P_SORT_BY";
        public const string SORTDIRC = "P_SORT_DIR";
        public const string FIELDS = "P_FIELDS";
        public const string USERPK = "P_USER_PK";
        public const string PROCESSID = "P_PROC_ID";
        public const string FROMDATE = "P_FROM_DT";
        public const string TODATE = "P_TO_DT";
        public const string SEARCHCORR = "P_DEPT_STORE";
        public const string SRSPK = "MRH_PK";
        public const string PAGEURL = "P_PAGE_URL";
        public const string MAH_STATUS = "P_MAH_STATUS";
    }

    public class Parameters_StoreAudit
    {
        public const string DEPID = "P_DPT_PK";
        public const string ITEMID = "P_ITM_PK";
        public const string SAXML = "P_SAH_XML";
        public const string SANO = "P_SAH_NO";
        public const string SAHPK = "P_SAH_PK";
        public const string CATEGORYTYPE = "P_ITM_TYPE";
        public const string BATCH = "P_ITM_BATCH";
        public const string STKBATCH = "P_STK_BATCH";
        public const string PRETTEXT = "P_RET_TEXT";
    }
    public class Parameters_StockAdjustment
    {
        public const string SAXML = "P_SDH_XML";
    }

    public class Parameters_GoodsInspectionNote
    {

        public const string SEARCHBY = "P_FLD_NAME";
        public const string SEARCHVALUE = "P_VALUE";
        public const string BIZUNIT = "P_BIZUNIT";
        public const string GOODINSPECTIONPK = "GIH_PK";
        public const string DEPTSTOREPK = "P_DEPT_STORE";
        public const string SEARCHNAME = "P_SER_NAME";
        public const string P_GRH_PK = "P_GRH_PK";
        public const string SEARCHVAL = "P_SER_VAL";
        public const string FIELDS = "P_FIELDS";
        public const string FROMDATE = "P_FROM_DT";
        public const string ISSTK_ITEM = "P_IS_STK_ITEM"; 
        public const string TODATE = "P_TO_DT";
        public const string GINPK = "GIH_PK";
        public const string RETVAL = "P_RET_VAL";
        public const string PAGENO = "P_PAGE_NO";
        public const string PAGESIZE = "P_PAGE_SIZE";
        public const string SORTBY = "P_SORT_BY";
        public const string SORTDIRC = "P_SORT_DIR";
        public const string USERPK = "P_USER_PK";
        public const string PROCESSID = "P_PROC_ID";
        public const string GOODSINSPXML = "P_GIH_XML";

        public const string RETGINNUMBER = "P_RET_NO";
        public const string GINID = "P_GIH_PK";
        public const string P_GIN_DTL_PK = "P_GRD_PK";
        public const string P_GIN_STATUS ="P_GIH_STATUS";
        public const string P_GIN_DEPT ="P_GIH_DEPT";
        public const string P_GIH_NO ="P_GIH_NO"; 
        public const string P_GRH_NO ="P_GRH_NO"; 
        public const string P_POH_NO ="P_POH_NO"; 
        public const string P_VEN_PK ="P_VEN_PK";
        public const string P_DEP_NAME = "P_DEP_NAME";
        public const string P_GIH_COMPANY = "P_GIH_COMPANY"; 
       
    }
    public class Parameters_NewItemRequest
    {
        public const string ITEMPK = "P_ITR_PK";
        public const string PRETVAL = "P_RET_VAL";
        public const string SEARCHBY = "P_FLD_NAME";
        public const string SEARCHVALUE = "P_VALUE";
        public const string BIZUNIT = "P_BIZUNIT";
        public const string STORE = "P_DEPT_STR";
        public const string DEPARTMENT = "P_DEPT";
        public const string SEARCHNAME = "P_SER_NAME";
        public const string SEARCHVAL = "P_SER_VAL";
        public const string PAGENO = "P_PAGE_NO";
        public const string PAGESIZE = "P_PAGE_SIZE";
        public const string SORTBY = "P_SORT_BY";
        public const string SORTDIRC = "P_SORT_DIR";
        public const string FIELDS = "P_FIELDS";
        public const string USERPK = "P_USER_PK";
        public const string PROCESSID = "P_PROC_ID";
        public const string FROMDATE = "P_FROM_DT";
        public const string TODATE = "P_TO_DT";
        public const string ID = "P_SER_NAME";
        public const string SRCH = "P_SER_VAL";

        public const string ACTIVE = "P_ACTIVE";
        public const string REMARKS = "P_ITR_REMARKS";
        public const string MODDATE = "P_LAST_MOD_DT";

        public const string NEWITEMREQUESTID = "P_ITR_PK";

        public const string NIRNO = "P_ITR_NO";

        public const string PLACEREQUESTTO = "P_ITR_DEPT_STORE";

        public const string REQUIREDFOR = "P_ITR_DEPT";

        public const string DATE = "P_ITR_DATE";

        public const string REQUIREDBYDATE = "P_ITR_REQD_DATE";

        public const string CATEGORY = "P_ITR_ITEM_CATEGORY";

        public const string NAME = "P_ITR_NAME";

        public const string REQUESTFREQUENCY = "P_ITR_FREQUENCY";

        public const string FRD = "P_ITR_FRQ_RQMT_DTL";

        public const string QTYREQUIRED = "P_ITR_QTY_REQD";
        public const string UOM = "P_ITR_QTY_UOM";
        public const string KNOWNVENDORS = "P_ITR_KWN_VENDOR";
        public const string DESCRIPTION = "P_ITR_DESC";
        public const string PURPOSE = "P_ITR_PURPOSE";
        public const string COMMERCIALDETAILS = "P_ITR_COMM_DTL";
        public const string SBU = "P_BIZUNIT";
        public const string NIRRETURNNO = "P_RET_NO";
        public const string ACTIONID = "@P_ACTION_ID";
    }

    public class Constants
    {
        public const string GINNo = "GIH_NO";
        public const string GINPK = "GIH_PK";
        public const string GINDate = "GIH_DATE";
        public const string Date = "Date";
        public const string Value_Zero = "0";
        public const string Value_Perc = "%";
        public const string Value_Empty = "";
        public const string Value_Star = "*";

        public const string GRNHEADERDATASET = "GRNHdr";
        public const string GRNDETAILDATASET = "GRNDtls";
        public const string GRNINGRDLC = "../Reports/GRNReport.rdlc";



        public const string GINHEADERDATASET = "GINHdr";
        public const string GINDETAILDATASET = "GINDtls";
        public const string GININGRDLC = "../Reports/GINReport.rdlc";

    }

    public class Parametes_Department
    {
        public const string DPT_PK = "DPT_PK";
        public const string DPT_ACTIVE = "DPT_ACTIVE";
        public const string P_DPT_HAS_WKF_CFG = "P_DPT_HAS_WKF_CFG"; 
        public const string P_DPT_PARENT = "P_DPT_PARENT";
        public const string P_DPT_TYPE = "P_DPT_TYPE";
        public const string P_DPT_CATEGORY = "P_DPT_CATEGORY";
        public const string P_DPT_NAME = "P_DPT_NAME";
    }

}
