using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants.DA
{ 
    /// <summary>
    /// Common DA Layer Strings
    /// </summary>
   public class CommonConstants
   {
       public const string EXELSHEETNAME = "TEMPLATE$";//Sheet name for exel import
       public const string ROLLGROUP = "P_ROLE_GROUP";

       public const string SAVECOMMAND = "SAVE";
       public const string CANCELCOMMAND = "CANCEL";
       public const string USERPK = "P_USER_PK";
       public const string BIZUNIT = "P_BIZUNIT";
       public const string COSTCENTER = "P_COST_CENTER";
       public const string TIMEZONE = "P_TMZ_PK";
       public const string CREATEDBY = "P_Crtd_By";
       public const string RETURNVAL = "P_RET_VAL";
       public const string ACTIVESTATUS = "P_Active";
       public const string SELECTVAL = "-1";
       public const string LASTMODDATE = "P_LAST_MOD_DT";
       public const string LASTMODDATETIME = "LAST_MOD_DT";
       public const string SELECT_VALUE_ONE = "1";
       public const string SELECT_VALUE_ZERO = "0";
       public const string QUERY_TYPE = "QUERY TYPE";
       public const string BUDGET_TYPE = "BUDGET TYPE";
       public const string ATT_FILETYPES = "ATT_FILETYPES";
       public const string CNS_Data = "CNS_Data";
       public const string UPLOAD_FOLDER = "Upload\\Budget";
       public const string DISABLEDCHECKBOX = "~/images/Classic/Icons/img-checkbx-disable.jpg";
       public const string MONTHNAME = "MText";
       public const string MONTHVALUE = "MValue";
       public const string ROOT = "Root";
       public const string COSTCENTERS = "COSTCENTERS";
       public const string ROLEACTIONS = "ROLEACTIONS";
       public const string COCOWNER = "COC_OWNER";
       public const string DETAILS = "Details";
       public const string ASSET = "1";
       public const string GENERAL = "1";

       public const string DEPRECIATIONINDEX = "2";
       public const string ACTIONID = "P_Task_Action";
       public const string DRAFT = "DRAFT";
       public const string XML = "P_XML";
       public const string SEARCH_KEY = "P_SearchKey";
       public const string SEARCH_VALUE = "P_SearchVal";
       public const string ID = "P_ID";
       public const string LOV_Data = "LOV_Data";
       public const string LOV_Value = "LOV_Value";
       //Email Template Names
       public const string BUDGETNOTIFICATION = "BudgetNotification";
       //Email Template Names
       public const string EmployToUser = "UserCreatedFrmEmploy";

       //Email Template Names
       public const string BUDGETMASTER = "BudgetMaster";
       public const int BUDGETMASTERPWDPOS = 7;
       public const string RESETPASSWORD = "ResetPassword";
       public const string REVIEWANDAPPROOVAL = "ReviewAndApproval";
       public const string REVIEWANDAPPROOVALNOPWD = "ReviewAndApprovalCCOwner";
       public const string SENDBACK = "BudgetSendBack";
       public const string SENDBACKSUBJECT = "BudgetSentBackSubject";

       public const string BUDGETRANDASUBJECT = "BudgetRandASubject";
       public const int REVIEWANDAPPROOVALPWDPOS = 7;
       //RedirectionPageofBudgetInitiate
       public const string DEFAULTPAGE = "~/Default.aspx";
       //Rollover image folder
       public const string ROLLOVER_FOLDER = "images\\Rollover";
       //Rollover image height
       public const int ROLLOVER_HEIGHT = 333;
       //Rollover image width
       public const int ROLLOVER_WIDTH = 647;

       //assigning db lenth of username to constant
       public const int DBLENGTH_USERNAME = 30;
       public const double MAXFLOATVALUE = 922337203685477.58;

       public const string IMGHASCOMMENTS = "~/images/Classic/Icons/gbudget-has-comments.png";
       public const string IMGNOCOMMENTS = "~/images/Classic/Icons/gbudget-comments.png";
       
    }
}
