using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants.DA.Administration
{
    public class NewsManagements
    {
        #region SPS
        public const string SP_GET = "SPADM_NEWS_EVENTS_GET_KV";
        public const string SP_SAVE = "SPADM_NEWS_EVENTS_SAVE";
        public const string SP_DELETE = "SPADM_NEWS_EVENTS_DELETE";
        public const string SP_STATUSCHANGE = "SPADM_NEWS_EVENTS_ACTIVATE";

        #endregion

        #region Fields
        public const string F_PK = "NWE_PK";
        public const string F_TITLE = "NWE_TITLE";
        public const string F_SHORTDESC = "NWE_DESC";
        public const string F_PUBLISHEDDT = "NWE_PUBLISH_ON";
        public const string F_DETAILS = "NWE_DETAILS";
        public const string F_CREATEDBY = "NWE_MOD_BY";
        public const string F_ACTIVE = "NWE_ACTIVE";
        public const string F_LASTMODON = "NWE_MOD_DT";
        public const string F_CREATEDBYNAME = "USR_Name";

        #endregion

        #region Parameters
        public const string P_PK = "P_NWE_PK";
        public const string P_TITLE = "P_NWE_Title";
        public const string P_SHORTDESC = "P_NWE_Desc";
        public const string P_PUBLISHEDDT = "P_NWE_Publish_On";
        public const string P_DETAILS = "P_NWE_Details";
        public const string P_ACTIVE = "P_NWE_Active";
        public const string P_LASTMODON = "P_LAST_MOD_DT";

        #endregion
    }
}
