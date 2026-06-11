using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessObject.Constants
{
    public class AllocationDA
    { /// <summary>
        /// Sp names
        /// </summary>

        #region SPS
        public const string SP_GetAllocation = "SPPLN_WPR_ALLOCATION_GET";
        public const string SP_GetFormerActivity = "SPPLN_WPR_LINE_ACTIVITY_GET";


        public const string SP_SaveAllocation = "SPPLN_WPR_ALLOCATION_SAVE";
        public const string SP_SaveFormerAllocation = "SPPLN_WPR_LINE_ACTIVITY_SAVE";


        #endregion
        /// <summary>
        /// Bind Fields  //Can avoid
        /// </summary>
        #region Fields
        public const string F_PK = "PK";
        public const string F_VALUE = "VALUE";
        public const string F_TEXT = "TEXT";

        #endregion
        /// <summary>
        /// Procedure Parameters
        /// </summary>
        #region Parameters

        public const string P_WID = "P_WID";
        public const string P_XML_PRO_LST = "P_XML_PRO_LST";
        public const string P_XML_ORD_LST = "P_XML_ORD_LST";
        public const string P_XML_ALC_DTL = "P_XML_ALC_DTL";
        public const string P_XML_PLN_SOD = "P_XML_PLN_SOD";

        public const string P_XML_ALC_LST = "P_XML_ALC_LST";

        //Former Allocation

        public const string P_XML_LNE_ACT = "P_XML_LNE_ACT";
        public const string P_XML_CFG_LST = "P_XML_CFG_LST";
        public const string P_XML_FMR_ACT = "P_XML_FMR_ACT";
        public const string P_XML_PRO_MST = "P_XML_PRO_MST";
        public const string P_XML_LIN_ACH = "P_XML_LIN_ACH";
        public const string P_XML_FMR_CFG = "P_XML_FMR_CFG";
        public const string P_XML_FMR_MVT = "P_XML_FMR_MVT";
        public const string P_XML_FMR_DEP = "P_XML_FMR_DEP";
        public const string P_XML_LNE_MST = "P_XML_LNE_MST";


        public const string P_XML_LIN_ACD = "P_XML_LIN_ACD";






        #endregion

        #region Fields



        #endregion
    }
}