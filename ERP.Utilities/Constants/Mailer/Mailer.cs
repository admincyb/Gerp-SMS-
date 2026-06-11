using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants
{
    public class MailerDA
    {
        #region SPS
        public const string SP_SaveMail = "SPCRM_MAIL_HDR_SAVE";
        public const string SP_DeleteMail = "SPADM_MAIL_QUEUE_TRX_DELETE";
        public const string SP_GetMail = "SPCRM_MAIL_HDR_GET";
        public const string SP_GetCustomerMail = "SPCRM_MAIL_LIST_GET";
        public const string SP_GetMailList = "SPCRM_MAIL_HDR_GET_LIST";
        public const string SP_GetMaiQlList = "SPADM_MAIL_QUEUE_TRX_GET_KV";
        public const string SPADM_MAIL_APP_DOC_DTL_SAVE = "SPADM_MAIL_APP_DOC_DTL_SAVE";
        public const string SPADM_MAIL_TEMPLATE_GET_KV = "SPADM_MAIL_TEMPLATE_GET_KV";
        public const string SPADM_MAIL_TEMPLATE_SAVE = "SPADM_MAIL_TEMPLATE_SAVE";
        #endregion
        #region Parameters
        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_MLQ_PARTY_TYPE = "P_MLQ_PARTY_TYPE";
        public const string P_FROM_DATE = "P_FROM_DATE";
        public const string P_MLQ_STATUS = "P_MLQ_STATUS";
        public const string P_TO_DATE = "P_TO_DATE";
        public const string P_MLQ_PARTY_PK = "P_MLQ_PARTY_PK";
        public const string P_MLQ_APP_TYPE = "P_MLQ_APP_TYPE";         
        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_XML = "P_XML";
        public const string P_CMH_PK = "P_CMH_PK";
        public const string P_STATUS = "P_STATUS";
        public const string P_CUS_PK = "P_CUS_PK";
        public const string P_SUBJECT = "P_SUBJECT";
        public const string P_FROM_DT = "P_FROM_DT";
        public const string P_TO_DT = "P_TO_DT";
        public const string P_APT_PK = "P_APT_PK";
        public const string P_MLQ_PK = "P_MLQ_PK";
        public const string P_LAST_MOD_DT = "P_LAST_MOD_DT";
        public const string P_MLQ_SUBJECT = "P_MLQ_SUBJECT";

        #region Mail Attachments
        public const string P_ADD_APP_TYPE = "P_ADD_APP_TYPE";
        public const string P_ADD_APP_SUB_TYPE = "P_ADD_APP_SUB_TYPE";
        public const string P_ADD_APP_PK = "P_ADD_APP_PK";
        public const string P_ADD_SL_NO = "P_ADD_SL_NO";
        public const string P_ADD_TITLE = "P_ADD_TITLE";
        public const string P_ADD_NAME = "P_ADD_NAME";
        public const string P_ADD_PATH = "P_ADD_PATH";
        public const string P_ADD_TYPE = "P_ADD_TYPE";
        public const string P_ADD_DESC = "P_ADD_DESC";
        public const string P_USER_PK = "P_USER_PK";
        public const string P_ADD_VERSION = "P_ADD_VERSION";
        #endregion
        #region Mail Template
        public const string P_TML_PK = "P_TML_PK";
        public const string P_IS_EDIT = "P_IS_EDIT";
        public const string P_TML_NAME = "P_TML_NAME";
        public const string P_TML_TEMPLATE = "P_TML_TEMPLATE";
        public const string P_CRTD_BY = "P_CRTD_BY";
        public const string P_TML_TO_CC = "P_TML_TO_CC";
        public const string P_TML_TO_BCC = "P_TML_TO_BCC";
        public const string P_TML_FROM = "P_TML_FROM";
        public const string P_TML_IS_EDIT = "P_TML_IS_EDIT";
        public const string P_TML_NAME2 = "P_TML_NAME2";
        public const string P_TML_MOD_BY = "P_TML_MOD_BY";
        #endregion
        #endregion      
        
    }
}
