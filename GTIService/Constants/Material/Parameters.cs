using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GTIService.Constants.Material
{
    public class Parameters
    {
        public const string CATEGORYPK = "P_ITC_PK";
        public const string POITEMTYPE = "ITC_PO_ITEM_TYPE";
        public const string MATERIALCATEGORYPK = "ITC_PK";
        public const string CATEGORYCODE = "ITC_CODE";
        public const string CATEGORYNAME = "ITC_NAME";
        public const string MATERIALCATEGORYPARENTPK = "ITC_PARENT";
        public const string ITC_VALUE = "ITC_VALUE"; 
        public const string CATEGORYBIZ = "P_BIZUNIT";
        public const string CATBIZUNIT = "ITC_BIZUNIT";
        public const string CATEGORYDESC = "ITC_DESC";
        public const string INACTIVEPERIOD = "ITC_INACTIVE_PERIOD";
        public const string ITM_INACTIVE_PERIOD = "ITM_INACTIVE_PERIOD";
        public const string UOMTYPE = "ITC_UOM_TYPE";
        public const string CATEGORYTYPE = "ITC_VALUE";
        public const string ACTIVE = "ITC_ACTIVE";
        public const string CREATEDBY = "ITC_CRTD_BY";
        public const string MODBY = "ITC_MOD_BY";
        public const string PURACCOUNT = "ITC_ACCOUNT_PUR";
        public const string INVACCOUNT = "ITC_ACCOUNT_INV";
        public const string SALEACCOUNT = "ITC_ACCOUNT_SAL";
        public const string CONACCOUNT = "ITC_ACCOUNT_CON";
        public const string NEEDQCINSP = "ITC_NEED_QC_INSP";
        public const string ITC_IS_STOCK = "ITC_IS_STOCK";
        public const string ITC_IS_DIR_GRN = "ITC_IS_DIR_GRN";
        public const string ITC_RPT_CATEGORY = "ITC_RPT_CATEGORY";
        public const string ITC_IS_SALE = "ITC_IS_SALE";
        public const string ITC_IS_VCH_POST = "ITC_IS_VCH_POST";
        public const string ITM_GST_CLASS = "ITM_GST_CLASS";
        

        public const string STATUS = "pTts";
        public const string SERACHVALUE = "P_VALUE";
        public const string ITC_NAME = "P_ITC_NAME";
        public const string FLAG = "P_FLAG";
        public const string P_ITC_VALUE = "P_ITC_VALUE"; 
        public const string P_WORK_ORDER = "P_WORK_ORDER";

        public const string VENDORDETAILSXML = "P_ITV_XML";
        public const string VENDORDETAILVALUE = "P_RET_VAL";

        public const string ITEMDEPARTMENT = "IDM_DEPT";
        public const string ITEMCATEGORY = "P_ITM_TYPE";
        public const string P_SHOW_SFG = "P_SHOW_SFG";

        public const string P_INCLUDE_ALT = "P_INCLUDE_ALT";
        public const string P_ALT_ITM = "P_ALT_ITM";
        public const string P_STOCK_ITM = "P_STOCK_ITM";

        public const string BUDGETDETAILVALUE = "P_RET_VAL";
        public const string BUDGETDETAILNO = "P_RET_NO";
        public const string BUDGETDETAILREFPK = "P_RET_REF_PK";



        public const string MATERIALLSTSRCHCONTENT = "pLvLoc";

        //@ITM_PK    		
        //@ITM_CODE		
        //@ITM_NAME		
        //@ITM_DESC		
        //@ITM_CATEGORY	
        //@ITM_UOM		
        //@ITM_MIN_STK	
        //@ITM_MAX_STK	
        //@ITM_ROL_STK	
        //@ITM_ACTIVE		
        //@ITM_BIZUNIT	
        //@ITM_CRTD_BY	
        //@ITM_MOD_BY		
        //@P_RET_VAL		


        //31/03/11
        //Vendor Material
        public const string P_ITV_PK = "P_ITV_PK";
        public const string P_ITV_ITEM = "P_ITV_ITEM";
        public const string P_ITV_VENDOR = "P_ITV_VENDOR";
        public const string P_ITV_NAME = "P_ITV_NAME";
        public const string P_ITV_PRICE = "P_ITV_PRICE";
        public const string P_ITV_CURRENCY = "P_ITV_CURRENCY";
        public const string P_ITV_MOQ = "P_ITV_MOQ";
        public const string P_ITV_MOQ_UOM = "P_ITV_MOQ_UOM";
        public const string P_ITV_LEAD_TIME = "P_ITV_LEAD_TIME";
        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_USER_PK = "P_USER_PK";
        public const string P_BIZUNIT = "P_BIZUNIT";

        public const string MATERIALPK = "ITM_PK";
        public const string MATERIALCODE = "ITM_CODE";
        public const string MATERIALNAME = "ITM_NAME";
        public const string MATERIALDESC = "ITM_DESC";
        public const string MATERIALCATEGORY = "ITM_CATEGORY";
        public const string P_ITM_ACTIVE = "P_ITM_ACTIVE";
        public const string MATERIALMODIFIED = "ITM_MOD_BY";
        public const string MATERIALSTATUS = "ITM_ACTIVE";
        public const string MATERIALRETURNTYPE = "ITM_TYPE";
        public const string MATERIALMINLEVEL = "ITM_MIN_STK";
        public const string MATERIALUOM = "ITM_UOM";
        public const string ITM_UOM_PURCHASE = "ITM_UOM_PURCHASE";
        public const string ITM_UOM_SALE = "ITM_UOM_SALE";
        public const string MATERIALMAXLEVEL = "ITM_MAX_STK";
        public const string P_XML = "P_XML";
        public const string P_DRM_PK = "P_DRM_PK";
        public const string MATERIALREORDERLEVEL = "ITM_ROL_STK";
        public const string P_ITM_PHR = "ITM_PHR";
        public const string P_ITM_TSC = "ITM_TSC";
        public const string P_ITM_BATCH_CODE = "ITM_BATCH_CODE";
        public const string P_COA_SUB_TYPE = "P_COA_SUB_TYPE";
        public const string P_COA_PK = "P_COA_PK";
        public const string P_COA_IS_GROUP = "P_COA_IS_GROUP";
        public const string P_BIZUNIT_PK = "P_BIZUNIT_PK";

        //External Material Issue (Multiple)
        public const string P_ITM_CODE = "P_ITM_CODE";
        public const string P_PAGE_URL = "P_PAGE_URL";
        public const string P_ICH_NO = "P_ICH_NO";
        public const string P_ICH_ITEM = "P_ICH_ITEM";
        public const string P_PAGE_SIZE = "P_PAGE_SIZE";
        public const string PAGENO = "P_PAGE_NO";
        public const string P_FROM_DT = "P_FROM_DT";
        public const string P_TO_DT = "P_TO_DT";

        //30/01/2014
        public const string ITM_WEIGHT = "ITM_WEIGHT";
        public const string ITM_MOQ = "ITM_MOQ";
        public const string ITM_MAX_OQ = "ITM_MAX_OQ";
        public const string P_IPD_PK = "P_IPD_PK";
        public const string P_IPD_ITEM = "P_IPD_ITEM";
        public const string P_IPD_CLASSIFICATION = "P_IPD_CLASSIFICATION";
        public const string P_IPD_TYPE = "P_IPD_TYPE";
        public const string P_IPD_INNER_LENGTH = "P_IPD_INNER_LENGTH";
        public const string P_IPD_INNER_BREADTH = "P_IPD_INNER_BREADTH";
        public const string P_IPD_INNER_HEIGHT = "P_IPD_INNER_HEIGHT";
        public const string P_IPD_OUTER_LENGTH = "P_IPD_OUTER_LENGTH";
        public const string P_IPD_OUTER_BREADTH = "P_IPD_OUTER_BREADTH";
        public const string P_IPD_OUTER_HEIGHT = "P_IPD_OUTER_HEIGHT";
        public const string P_IPD_PLY = "P_IPD_PLY";
        public const string P_IPD_PAPER_COLOR = "P_IPD_PAPER_COLOR";
        public const string P_IPD_ART_WORK = "P_IPD_ART_WORK";
        public const string P_IPD_CUSTOMER = "P_IPD_CUSTOMER";
        public const string P_IPD_ACTIVE = "P_IPD_ACTIVE";
        public const string P_DOC_PK = "P_DOC_PK";
        public const string P_DOC_SEQ_NO = "P_DOC_SEQ_NO";
        public const string P_DOC_TITLE = "P_DOC_TITLE";
        public const string P_DOC_NAME = "P_DOC_NAME";
        public const string P_DOC_TYPE = "P_DOC_TYPE";
        public const string P_DOC_PATH = "P_DOC_PATH";
        public const string ITM_SET = "ITM_SET";


        public const string MATERIALCREATEDBY = "ITM_CRTD_BY";
        public const string BIZUNIT = "ITM_BIZUNIT";
        public const string MATERIALLSTSTATUS = "P_SER_NAME";
        public const string MATERIALLSTSRCH = "P_SER_VAL";

        public const string DPTPK = "P_DPT_PK";
        public const string P_ITM_SET = "P_ITM_SET";
        public const string MATERIALSEARCHVALUE = "P_VALUE";
        public const string MATERIALSEARCHBY = "P_FLD_NAME";
        public const string P_ITM_CAT = "P_ITM_CAT";

        public const string MATERIALID = "pKp";
        public const string P_IPD_THICKNESS = "P_IPD_THICKNESS";
        public const string P_IPD_PAPER_TYPE = "P_IPD_PAPER_TYPE";
        
       
        
        

       
        

        public const string ITEMCATEGORYID = "ITM_CATEGORY";
        public const string ITEMPK = "ITM_PK";
        public const string DPT_PK = "DPT_PK";
        public const string TO_UOM = "TO_UOM";
        public const string ITEMSTATUS = "ITM_ACTIVE";
        //public const string MATER@ITM_CATEGORY
        public const string TYPE = "ITM_TYPE";
        public const string STORE = "P_DEPT";
        public const string IS_STOCK = "V_IS_STOCK";
        public const string SEARCH_NAME = "ITC_NAME";
        public const string NAMESEARCH = "ITM_NAME";
        //new  2 parm
        public const string P_DPT_TYPE = "P_DPT_TYPE";
        public const string P_DPT_CATEGORY = "P_DPT_CATEGORY";


        //NewMaterial start
        public const string P_VEN_PK = "P_VEN_PK";
        public const string P_ITM_PK = "P_ITM_PK";
        public const string P_DATE = "P_DATE";
        public const string P_CUS_PK = "P_CUS_PK";
        public const string P_CURRENCY = "P_CURRENCY";
        public const string P_WIM_ITEM_TYPE = "P_WIM_ITEM_TYPE"; 
        //NewEnd

        public const string P_STD_MOD_DT="P_STD_MOD_DT";

        public const string P_ISD_SIZE = "P_ISD_SIZE";
        public const string P_OST_QTY_OPENING = "P_OST_QTY_OPENING";
        public const string ITM_GROUP = "ITM_GROUP";
        public const string ITM_TYPE = "ITM_TYPE";
        public const string P_XML_PRD = "P_XML_PRD";
        public const string P_LAST_MOD_DT = "P_LAST_MOD_DT";
        public const string P_ITM_NEED_QC_INSP = "ITM_NEED_QC_INSP";
        public const string P_ITM_NEED_BATCH_STK = "ITM_NEED_BATCH_STK";
        public const string P_ITM_IS_LINKED_ITEM = "P_ITM_IS_LINKED_ITEM";
        public const string P_ITM_IS_WORK_ORDER = "P_ITM_IS_WORK_ORDER"; 
        public const string P_GCM_PK = "P_GCM_PK";
        public const string P_ITM_IS_CONVERSION_REQD = "P_ITM_IS_CONVERSION_REQD";
        public const string P_ITM_IS_ASSET = "P_ITM_IS_ASSET";

        public const string P_HAS_STOCK = "P_HAS_STOCK";

        public const string P_ITEM_TYPE = "P_ITEM_TYPE";
        public const string P_QTY = "P_QTY";
        public const string P_OPERATION = "P_OPERATION";
        public const string P_WOM_BRAND = "P_WOM_BRAND";
        public const string P_WOM_CUSTOMER = "P_WOM_CUSTOMER";
        public const string P_VENDOR = "P_VENDOR";
        public const string P_WIH_PK = "P_WIH_PK";
        public const string P_CIM_PK = "P_CIM_PK";
        public const string P_BGH_PK = "P_BGH_PK";//@ITM_PK
    }
    public class Parameters_RequisitionSlip
    {
        public const string MATERIALREQPK = "ITM_PK";//@ITM_PK
        public const string MATERIALRETURNVALUE = "P_RET_VAL";

    }

    
}
