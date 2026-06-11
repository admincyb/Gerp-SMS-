using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessObject.Constants
{
 
    public class ShiftReport
    {
        /// <summary>
        /// Sp names
        /// </summary>

        #region SPS
        public const string SP_GetShiftReport = "SPPLN_WPR_SHIFT_REPORT_GET";
        public const string SP_SaveShiftReport = "SPPLN_WPR_SHIFT_REPORT_SAVE";


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
        public const string P_XML_SFT_RPT = "P_XML_SFT_RPT";

        public const string P_XML_SFT_LST = "P_XML_SFT_LST";
        public const string P_XML_LNE_MST = "P_XML_LNE_MST";
        public const string P_XML_PRO_MST = "P_XML_PRO_MST";

        public const string P_XML_SFT_DTL = "P_XML_SFT_DTL";
        public const string P_XML_DOC_NUM = "P_XML_DOC_NUM";

        public const string P_XML_SFT_MST = "P_XML_SFT_MST";

 



        #endregion

        #region Fields



        #endregion
    }
}