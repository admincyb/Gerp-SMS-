using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Reports
{
    public class Parameters
    {
        public const string DEPT_PK = "P_DPT_PK";
        public const string DATE = "P_DATE";
        public const string ITEM_CATG = "P_ITM_CATEGORY";
        public const string SBU = "P_BIZUNIT";
        public const string WRKF_SBU = "PBizunit";

        public const string ITEM_CATG_PK = "P_ITC_PK";
        public const string ITEM_PK = "P_ITM_PK";
        public const string DPT_CATEGORY="DPT_CATEGORY";

        public const string BDATE = "P_AS_ON_DATE";

        public const string P_XML = "P_XML";
         

        #region Purchase Request Report Parameters
        
        public const string PRR_FROMDATE = "P_FROM_DT";
        public const string PRR_TODATE = "P_TO_DT";
        public const string PRR_STATUS = "P_PRH_STATUS";
        public const string PRR_NO = "P_PRH_NO";
        public const string PRR_SBU = "P_BIZUNIT";
        public const string PRR_PK = "PRH_PK";
        public const string P_PRR_PK = "P_PRH_PK";
        public const string P_DPT_CATEGORY = "P_DPT_CATEGORY";
        public const string P_DPT_TYPE = "P_DPT_TYPE";
        public const string P_ITM_CATEGORY = "P_ITM_CATEGORY";

        #endregion
        #region Delivary order
        public const string P_DPH_PK = "P_DPH_PK";
        #endregion

        public const string P_AppType = "P_FTH_REF_TYPE";
        public const string P_RccPK = "P_FTH_REF_PK";
        public const string P_AST_VALUE = "P_AST_VALUE";
        public const string P_AST_PK = "P_AST_PK";
    }
}
