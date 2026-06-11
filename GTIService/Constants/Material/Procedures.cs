using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Material
{
    public class Procedures
    {
        
        
       
        public const string SAVEMATERIALCATEGORY = "SPINV_ITEM_CATEGORY_SAVE";
        public const string GETMATERIALCATEGORY = "SPINV_ITEM_CATEGORY_GET_TREE";
        public const string GETMATERIALCATEGORYEXCEPTFG = "SPINV_ITEM_CATEGORY_FG_TREE";
        public const string GETMATERIALCATEGORYEXCEPTFGAUTO = "SPINV_ITEM_CATEGORY_FG_AUTO";
        public const string GETMATERIALCATEGORYSTKEXCEPTFGAUTO = "SPINV_ITEM_CATEGORY_STK_AUTO";
        public const string GETMATERIALCATEGORYAUTO = "SPINV_ITEM_CATEGORY_AUTO";
        public const string DELETEMATERIALCATEGORY = "SPINV_ITEM_CATEGORY_DELETE";
        public const string SPINV_ITEM_CATEGORY_DEPT_AUTO = "SPINV_ITEM_CATEGORY_DEPT_AUTO";
        public const string SPINV_ITEM_CATEGORY_DEPT_MR_AUTO = "SPINV_ITEM_CATEGORY_DEPT_MR_AUTO";
        public const string GETMATERIALCATEGORYRAWMATERIAL = "SPINV_ITEM_CATEGORY_RM_GET_TREE";
        public const string GETMATERIALLIST = "SpGRmTldTime";
        public const string GETMATERIALCATEGORYWITHOUTSEMIANDFINISHED = "SPINV_ITEM_CATEGORY_WITHOUT_FG_TREE";
        public const string GETUOMTYPECATEGORY = "SPINV_UOM_TYPE_CATEGORY";
        public const string GETMATERIALSNAME = "SpGMtiUOM";
        public const string SPINV_ITEM_CATEGORY_COA_MAP_GET = "SPINV_ITEM_CATEGORY_COA_MAP_GET";

        public const string GETMATERIALUMO = "SpGUOMYbTime";
        //280311

        public const string GETITEMNAME = "SPINV_ITEM_NAME_GET";
        public const string GETMATERIALBYCATEGORY = "SPINV_ITEM_MST_GET_KV";
        public const string GETRELATEDMATERIAL = "SPINV_ALT_ITEM_MST_GET_KV";
        public const string SPINV_BOM_ITEM_MST_GET_KV = "SPINV_BOM_ITEM_MST_GET_KV"; 

        public const string SPINV_ITEM_PACK_DOC_DTL_GET = "SPINV_ITEM_PACK_DOC_DTL_GET";
        public const string GETMATERIALDESCRIPTION = "SPINV_STK_HDR_GET";
        public const string GETMATERIALBYCODE = "SPINV_ITEM_MST_KV";
        public const string GETMATERIALBYCATEGORYANDSTORE = "SPINV_ITEM_DEPT_MST_GET_KV";
        public const string GETMATERIALBYCATEGORYANDSTOREAUTO = "SPINV_ITEM_DEPT_MST_GET_AUTO";
        public const string GETITEMUOM = "SPINV_ITEM_UOM_GET";
        public const string GETCURRENTSTOCK = "SPINV_STK_DTL_GET_KV";
        public const string GETCURRENTSTOCKDISP = "SPPRD_DISP_TRX_HDR_GET_KV";
        public const string GETGSTCLASSIFICATION = "SPFIN_GST_CLASS_MST_GET_KV";
       //310311 

        public const string DELETEMATERIALDETAILS = "SPINV_ITEM_MST_DELETE";
        public const string SAVEMATERAILDETAILS = "SPINV_ITEM_MST_SAVE";
        public const string SPINV_ITEM_VENDOR_SAVE = "SPINV_ITEM_VENDOR_SAVE";
        public const string GETMATERIALLISTBYSRH = "SPINV_ITEM_MST_GET_LIST";
        public const string GETWOMATERIALLISTBYSRH = "SPINV_ITEM_MST_WO_TYPE_WISE_GET_LIST";
        public const string SPINV_ITEM_MST_GET_CAT_LIST = "SPINV_ITEM_MST_GET_CAT_LIST";
        public const string SPINV_ITEM_DEPT_GET_TREE = "SPINV_ITEM_DEPT_GET_TREE";
        public const string GETSEARCHVALUE = "SPINV_ITEM_MST_AUTO";
        public const string GETMATERIALNAMESEARCHVALUE = "SPINV_ITEM_AUTO";
        public const string GETSTOREMATERIALNAMESEARCHVALUE = "SPINV_ITEM_DEPT_CAT_GET";
        public const string SPINV_ITEM_DEPT_STOCK_WISE_GET = "SPINV_ITEM_DEPT_STOCK_WISE_GET";
        public const string GETSPFINCOAMSTAUTOGET = "SPFIN_COA_MST_AUTO_GET";
        public const string GETSTOREMATERIALNAMESEARCHVALUESTK = "SPINV_ITEM_DEPT_CAT_STK_GET";
        public const string SPINV_ITEM_MST_GET_AUTO = "SPINV_ITEM_MST_GET_AUTO";


        //05/4/11
           
        public const string SAVEVENDORDETAILSXML = "SPINV_ITEM_VENDOR_MAP_SAVE";
        public const string GETVENDORMAPPINGDETAILSBYITEMPKXML = "SPINV_ITEM_VENDOR_MAP_GET";
        public const string SPINV_WO_ITEM_MATERIAL_MAP_SAVE = "SPINV_WO_ITEM_MATERIAL_MAP_SAVE";
        public const string SPFIN_BUDGET_GET = "SPFIN_BUDGET_GET";

        //07/04/2011

        public const string GETMATERIALUOMCONVERSION = "SPINV_UOM_CONV_FACT_GET";

        public const string GETMATERIALUMOCONV = "SPINV_UOM_CONV_KV";
        public const string GETMATERIALUOMTRD = "SPINV_ITEM_UOM_GET_KV";

        //10/05/2010

        public const string GETSTOREMATERIALS = "SPINV_ITEM_DEPT_MAP_GET";
        public const string GETMATERIALUOMCONVERSIONFACT = "SPINV_UOM_CONV_FACT_GET";
        public const string GETSTORECATEGORYMATERIALS = "SPINV_STK_AUD_ITEM_GET_KV";
        public const string GETRATEHISTORY = "SPINV_ITEM_VENDOR_HISTORY_GET";
        public const string GET_ORDER_ITEM = "SPINV_WO_ITEM_MATERIAL_MAP_GET_KV"; 
        public const string GET_ITEM_RATES = "SPPUR_VENDOR_ITEM_RATES_GET_LIST";
        public const string GET_BATCH_NO = "SPINV_STK_BATCH_GET_KV";
        public const string GET_BATCHDETAILS = "SPINV_STK_BATCH_DTL_GET";
        public const string GET_NEXT_ITEM_CODE = "INV_ITEM_CODE_GEN";
        public const string IS_ITEM_CODE_EXIST = "";
        public const string SPINV_ITEM_MST_GET_KV = "SPINV_ITEM_MST_GET_KV";
        public const string SPINV_STK_BATCH_CONS_GET_KV = "SPINV_STK_BATCH_CONS_GET_KV";

        //21/03/2018
        public const string SPINV_ITEM_EXT_ISS_GET_LIST = "SPINV_ITEM_EXT_ISS_GET_LIST";

        //03/10/2019

        public const string SPSAL_BRAND_PRODUCT_RATE_GET = "SPSAL_BRAND_PRODUCT_RATE_GET";
        public const string SPINV_WO_ITEM_MATERIAL_MAP_GET_LIST = "SPINV_WO_ITEM_MATERIAL_MAP_GET_LIST";

        public const string SPFIN_BUDGET_WKF_SAVE = "SPFIN_BUDGET_WKF_SAVE";
        public const string SPFIN_BUDGET_DELETE = "SPFIN_BUDGET_DELETE";

    }
}
