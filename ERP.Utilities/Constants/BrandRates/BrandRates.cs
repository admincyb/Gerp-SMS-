using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants
{
  public class BrandRatesDA
    {
        #region SPS
        public const string SP_GetCustomers = "SPCRM_CUSTOMER_MST_GET_KV";
        public const string SP_Products = "SPINV_ITEM_MST_GET_KV_LST";
        public const string SP_ProductDetails = "SPINV_ITEM_MST_LIST_GET";
        public const string SP_CustomerDetails = "SPCRM_CUSTOMERS_MST_LIST_GET";
        public const string SP_GET_BTANDRATE = "SPCRM_CUS_ITEM_RATE_GET";
        public const string SP_GetCustomerRate = "SPCRM_CUST_ITEM_RATE_LIST_GET";
        public const string SP_GetBrandDetails = "SPCRM_CUST_ITEM_RATE_HDR_GET_KV";
        public const string SP_SaveBrandDetails = "SPCRM_CUST_ITEM_RATE_HDR_SAVE";
        public const string SP_ImportCustomerBrands = "SPCRM_CUST_ITEM_IMPORT";
        public const string SP_CopyBrandDetails = "SPCRM_CUST_ITEM_RATE_COPY_SAVE";
        public const string SP_DeleteBrandDetails = "SPCRM_CUST_ITEM_RATE_DELETE";
        public const string SP_GetBrandRateHistory = "SPCRM_BRAND_RATE_HISTORY_GET";

        public const string SP_GetBrandpRroductRateHistory = "SPSAL_BRAND_PRODUCT_RATE_HISTORY_GET";
        public const string SP_CusRateLatest = "SPCRM_CUST_ITEM_RATE_LATEST";
        public const string SPCRM_CUST_ITEM_RATE_HDR_LIST="SPCRM_CUST_ITEM_RATE_HDR_LIST";
        public const string SPADM_CONST_MST_GET_KV = "SPADM_CONST_MST_GET_KV";
        public const string SP_FilterProductDetails = "SPSAL_BRAND_PRODUCT_RATE_GET_KV";
        public const string SP_GetProductRate = "SPSAL_BRAND_PRODUCT_RATE_GET_LIST";
        public const string SP_SaveBrandsRates = "SPSAL_BRAND_PRODUCT_RATE_DTL_SAVE";
        public const string SP_BrandItemInsert = "SPINV_ITEM_CUST_ITEM_SAVE";

        #endregion
        #region Parameters

        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_ACTIVE = "P_ACTIVE";
        public const string ITM_CATEGORY = "ITM_CATEGORY";
        public const string ITEM_ACTIVE = "ITM_ACTIVE";
        public const string P_XML = "P_XML";
        public const string P_CIM_PK = "P_CIM_PK";
        public const string P_BRH_PK_SRC = "P_BRH_PK_SRC";
        public const string P_BRH_DATE_FROM = "P_BRH_DATE_FROM";
        public const string P_BRH_DATE_TO = "P_BRH_DATE_TO";
        public const string P_BRH_DESC = "P_BRH_DESC";
        public const string P_USER = "P_USER";
        public const string P_PROD_DATE = "P_PROD_DATE";
        public const string P_BRAND_PK = "P_BRAND_PK";
        public const string P_CUS_PK = "P_CUS_PK";
        public const string P_ITEM_PK = "P_ITEM_PK";
        public const string P_FROM_DATE = "P_BRH_DATE_FROM";
        public const string P_TO_DATE = "P_BRH_DATE_TO";
        public const string P_BRH_PK = "P_BRH_PK";
        public const string P_CIM_BRAND_CODE = "P_CIM_BRAND_CODE";
        public const string P_CIM_BRAND_NAME = "P_CIM_BRAND_NAME";
        public const string P_CIM_CUSTOMER = "P_CIM_CUSTOMER";
        public const string P_USER_PK = "P_USER_PK";

        //For Brand Pricce list
        public const string P_FROM_DATE_BR = "P_FROM_DATE";
        public const string P_TO_DATE_BR = "P_TO_DATE";
        public const string P_CUR_PK = "P_CUR_PK";
        public const string P_CON_ACTIVE = "P_CON_ACTIVE ";
        public const string P_CGT_VALUE = "P_CGT_VALUE";
        public const string P_CNG_VALUE = "P_CNG_VALUE";
        public const string P_CON_BIZUNIT = "P_CON_BIZUNIT";
        public const string P_CON_PK = "P_CON_PK";
        public const string P_ISD_NATURE = "P_ISD_NATURE";
        public const string P_ISD_THICKNESS = "P_ISD_THICKNESS";
        public const string P_ISD_PROCESS = "P_ISD_PROCESS";
        public const string P_ISD_SURFACE = "P_ISD_SURFACE";
        public const string P_ISD_COLOUR = "P_ISD_COLOUR";
        public const string P_ISD_GRADE = "P_ISD_GRADE";
        public const string P_ISD_SIZE = "P_ISD_SIZE";
        public const string P_ISD_LENGTH = "P_ISD_LENGTH";
        public const string P_ISD_CHLORINATION = "P_ISD_CHLORINATION";
        public const string P_ISD_ADNL_SPEC01 = "P_ISD_ADNL_SPEC01";
        public const string P_CUS_SPECIAL_CAT="P_CUS_SPECIAL_CAT";
        public const string P_DISABLE_DTLS = "P_DISABLE_DTLS";
        public const string P_ISD_ADNL_SPEC05 = "P_ISD_ADNL_SPEC05";
        public const string P_ISD_ADNL_SPEC06 = "P_ISD_ADNL_SPEC06";
        public const string P_ISD_ADNL_SPEC07 = "P_ISD_ADNL_SPEC07";

        #endregion



        
    }
}
