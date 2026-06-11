using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants.DA.Administration
{
    public class MailSend
    {
        #region SPS
        public const string SP_GET_MAILQUE = "SPADM_MAIL_QUEUE_TRX_GET_LIST";
        public const string SP_GET_MAIL_QUEUE_AUTO = "SPADM_MAIL_QUEUE_TRX_AUTO";
        #endregion

        #region Parameters
        public const string P_PROCESS = "P_PROCESS";
        public const string P_FROM_DATE = "P_FROM_DATE";
        public const string P_TO_DATE = "P_TO_DATE";
        public const string P_MLQ_PK = "P_MLQ_PK";
        public const string P_MLQ_STATUS = "P_MLQ_STATUS";
        public const string P_BIZUNIT = "P_BIZUNIT";

        #endregion

        #region Fields
        public const string F_ATTEMPT = "MLQ_Attempt";
        public const string F_ATTEMPTON = "MLQ_Attempt_On";
        public const string F_CONTENT = "MLQ_Content";
        public const string F_FROM = "MLQ_From";
        public const string F_NAME = "MLQ_User_Name";
        public const string F_PK = "MLQ_PK";
        public const string F_PRIORITY = "MLQ_Priority";
        public const string F_STATUS = "MLQ_Status_Text";
        public const string F_SUBJECT = "MLQ_Subject";
        public const string F_TEMPLATE = "MLQ_Template_Name";
        public const string F_TO = "MLQ_To";
        public const string F_CRTD_DT = "MLQ_CRTD_DT";
        public const string F_WKF_PROCESS_TEXT = "MLQ_WKF_PROCESS_TEXT";
        public const string F_WKF_PROCESS = "MLQ_WKF_PROCESS";
        #endregion
    }
}
