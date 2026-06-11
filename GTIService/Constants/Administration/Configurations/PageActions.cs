using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Administration.Configurations
{
    public class PageActions
    {
        #region SP'S
        public const string SP_GETPAGES = "SPADM_PAGE_MST_GET_KV";
        public const string SP_GETPAGEACTIONS = "SPADM_PAGE_ACTION_CFG_GET_LIST";
        public const string SP_SAVEPAGEACTION = "SPADM_PAGE_ACTION_CFG_SAVE";
        public const string SP_GETPAGEACTIONSKV = "SPADM_PAGE_ACTION_CFG_GET_KV";
        public const string SP_DELETEPAGEACTION = "SPADM_PAGE_ACTION_CFG_DEL";

        #endregion

        #region Parameters
        public const string P_PAGEPK = "P_PAG_PK";
        public const string P_PK = "P_ACT_PK";
        public const string P_PAGE = "P_ACT_PAGE";
        public const string P_SECTION = "P_ACT_SECTION";
        public const string P_ACTION = "P_ACT_ACTION";
        public const string P_DESC = "P_ACT_DESC";
        public const string P_PAG_URL = "P_PAG_URL";

        #endregion

        #region Fields
        public const string F_PAGEPK = "PAG_PK";
        public const string F_PAGENAME = "PAG_TITLE";
        public const string F_ACTIONPAGEPK = "ACT_PAGE";
        public const string F_SECTION = "ACT_SECTION";
        public const string F_ACTION = "ACT_ACTION";
        public const string F_DESCRIPTION = "ACT_DESC";
        public const string F_STATUS = "ACT_ACTIVE";


        #endregion

        #region Constants
        public const string C_ACTIVE = "1";
        public const string C_INACTIVE = "0";
        public const string C_PKVALUE = "2";
        public const string C_ZERO_VAL = "0";
        public const string C_DATE_FORMAT = "dd/MM/yyyy";
        public const string C_SELECT = "select";
        public const string C_EMPTY_VAL = " ";
        public const string C_ASC = "asc";
        public const string C_DESC = "desc";        
        #endregion
    }
}
