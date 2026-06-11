using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessObject.Constants
{
    public class CustomerOrderTrackerDA
    {
        /// <summary>
        /// Sp names
        /// </summary>

        #region SPS
        public const string SP_GetCustomers_Orders = "SPPLN_WPR_ORDER_STATUS_GET";
        public const string SP_GetCustomers_Auto = "SPSAL_CUSTOMER_MASTER_AUTO";
       

        #endregion
        /// <summary>
        /// Bind Fields  //Can avoid
        /// </summary>
        #region Fields
        public const string F_PK = "PK";
        public const string F_VALUE = "VALUE";
        public const string F_TEXT = "TEXT";

        public const string CustomerPK = "CUS_PK";
        public const string CustomerCode = "CUS_CODE";


        #endregion
        /// <summary>
        /// Procedure Parameters
        /// </summary>
        #region Parameters

        public const string P_WID = "P_WID";
        public const string P_XML_CUS_MST = "P_XML_CUS_MST";
        public const string P_XML_ORD_HDR = "P_XML_ORD_HDR";
        public const string P_XML_ORD_LST = "P_XML_ORD_LST";
        public const string P_XML_ORD_STT = "P_XML_ORD_STT";
        public const string CustomerName = "P_CUS_NAME";
        public const string Active = "P_ACTIVE";
        public const string BizUnit = "P_BIZUNIT";


       

        #endregion

        #region Fields



        #endregion
    }
}