using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants.DA.Administration
{
    public class ViewMail
    {
        #region SPS
        public const string SP_GET = "SPAD_MAIL_QUEUE_TRX_GET";
        public const string SP_SAVE = "SPAD_MAIL_QUEUE_TRX_SAVE";
        public const string SP_GET_AUTO = "SPAD_MAIL_QUEUE_TRX_AUTO_GET";
        public const string SPHRM_EMP_PAYROLL_MAIL_DTL_GET = "SPHRM_EMP_PAYROLL_MAIL_DTL_GET";
        public const string SPHRM_EMP_PAYROLL_MAIL_DTL_UPDATE = "SPHRM_EMP_PAYROLL_MAIL_DTL_UPDATE";
        #endregion

        #region Fields
        public const string F_PK = "MLQ_PK";
        public const string F_NAME = "MLQ_User_Name";
        public const string F_FROM = "MLQ_From";
        public const string F_TO = "MLQ_To";
        public const string F_SUBJECT = "MLQ_Subject";
        public const string F_CONTENT = "MLQ_Content";
        public const string F_ATTEMPT = "MLQ_Attempt";
        public const string F_STATUS = "MLQ_Status_Text";
        public const string F_PRIORITY = "MLQ_Priority";
        public const string F_ATTEMPTON = "MLQ_AttemptOn";
        public const string F_TEMPLATE = "MLQ_Template_Name";
        #endregion

        #region
        public const string DROPDOWNLOCATION = "ddlLocation";
        #endregion

        #region Parameters
        public const string P_PK = "P_COC_PK";
        public const string P_MLQ_PK = "P_MLQ_PK";
        public const string P_MLQ_STATUS = "P_MLQ_STATUS";
        public const string P_LASTMODDATETIME = "P_LAST_MOD_DT";
        public const string P_COUNT = "P_COUNT";
        public const string P_User_PK = "P_User_PK";
        public const string P_EPM_PK = "P_EPM_PK";
        public const string P_EPM_IS_GENERATED = "P_EPM_IS_GENERATED";
        public const string P_ADD_PATH = "P_ADD_PATH";
        public const string P_ADD_TYPE = "P_ADD_TYPE";
        public const string P_BIZUNIT = "P_BIZUNIT";
        #endregion
    }
}
