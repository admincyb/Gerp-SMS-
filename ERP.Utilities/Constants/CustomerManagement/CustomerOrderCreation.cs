using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessObject.Constants
{
    public class CustomerOrderCreationDA
    {
        /// <summary>
        /// Sp names
        /// </summary>

        #region SPS
        public const string SP_GetOrder = "SPPLN_WPR_ORDER_GET";
        public const string SP_SaveOrder = "SPPLN_WPR_ORDER_SAVE";


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
        public const string P_XML_SOH_LST = "P_XML_SOH_LST";
        public const string P_XML_DOC_NUM = "P_XML_DOC_NUM";
        public const string P_XML_CUS_MST = "P_XML_CUS_MST";
        public const string P_XML_PDG_MST = "P_XML_PDG_MST";
        public const string P_XML_SIZ_MST = "P_XML_SIZ_MST";
        public const string P_XML_UOM_MST = "P_XML_UOM_MST";
        public const string P_XML_SO_DTL = "P_XML_SO_DTL";
        public const string P_XML_SOD_LST = "P_XML_SOD_LST";
        public const string P_XML_PCK_DTL = "P_XML_PCK_DTL";
        public const string P_XML_SO_DEL = "P_XML_SO_DEL";

        #endregion


    }
}
