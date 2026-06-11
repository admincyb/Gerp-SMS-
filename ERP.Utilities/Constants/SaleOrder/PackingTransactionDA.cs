using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessObject.Constants
{
    public class PackingTransactionDA
    {
        /// <summary>
        /// Stored Procedures
        /// </summary>
        #region SPS
        public const string SP_GetPackingDtl = "SPPLN_WPR_PACKING_GET";
        public const string SP_SavePackingDtl = "SPPLN_WPR_PACKING_SAVE";
        #endregion

        /// <summary>
        /// Parameters
        /// </summary>
        #region Parameters
        public const string P_WID = "P_WID";
        public const string P_XML_PCK_LST = "P_XML_PCK_LST";
        public const string P_XML_CUS_MST = "P_XML_CUS_MST";
        public const string P_XML_ORD_HDR = "P_XML_ORD_HDR";
        public const string P_XML_DOC_NUM = "P_XML_DOC_NUM";
        public const string P_XML_SFT_MST = "P_XML_SFT_MST";
        //To Save and Get
        public const string P_XML_PCK_DTL = "P_XML_PCK_DTL";
        public const string P_XML_ORD_LST = "P_XML_ORD_LST";
        public const string P_XML_PCK_DEL = "P_XML_PCK_DEL";


        #endregion
    }
}