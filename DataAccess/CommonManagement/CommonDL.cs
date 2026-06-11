using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Common;
using GTIService;
using System.Configuration;

namespace DataAccess.CommonManagement
{
    /// <summary>
    /// Class used to Access/Interact with DB Layer To Get common Details
    /// </summary>
    public class CommonDL
    {
        /// <summary>
        /// Get Department Details
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDepartment()
        {
            DataTable dtDept = new DataTable();
            DBService dbService = new DBService();
            dtDept = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETDEPARTMENT).Tables[0];
            return dtDept;
        }

        /// <summary>
        /// Methord used to get the Country Details
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public static DataTable GetCountry()
        {
            DataTable dtCountry = new DataTable();
            DBService dbService = new DBService();
            dtCountry = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETCOUNTRY).Tables[0];
            return dtCountry;
        }

        /// <summary>
        /// Methord used to get the Project Details 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetProjectListAuto(string srchBy, string srhcType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHBY  , srchBy),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, srhcType),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPPRJPROJECTINFAUTO, colParameters).Tables[0];
        }

        /// <summary>
        /// Get bank account
        /// </summary>
        /// <param name="accountpk"></param>
        /// <param name="active"></param>
        /// <param name="subtype"></param>
        /// <param name="IsGroup"></param>
        /// <returns></returns>
        public static DataTable GetAccount(int accountpk, int active, int subtype, int IsGroup,int BizUnit=0)
        {
            DataTable dtState = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_COA_PK, accountpk),                         
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_COA_SUB_TYPE, subtype),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_COA_IS_GROUP, IsGroup), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, BizUnit == 0 ? (object)DBNull.Value : BizUnit) 
            };
            dtState = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPFIN_COA_MST_GET_KV, colParameters).Tables[0];
            return dtState;
        }

        /// <summary>
        /// Methord used to get the State Details Corresponding to country ID
        /// </summary>
        /// <param name="country"></param>
        /// <param name=""></param>
        public static DataTable GetState(int countryID)
        {
            DataTable dtState = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters.PSTATECTRY, countryID == 0 ? (object)DBNull.Value : countryID)                         
            };
            dtState = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETSTATE, colParameters).Tables[0];
            return dtState;
        }


        /// <summary>
        /// Methord used to get the Currency Details
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public static DataTable GetCurrency(User objUser, int bizUnit)
        {
            DataTable dtCurrency = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                 new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , bizUnit==0?(object)DBNull.Value  : bizUnit), 
            };
            dtCurrency = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETCURRENCY, colParameters).Tables[0];
            return dtCurrency;
        }




        /// <summary>
        /// Get Department Details
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="deptParentPK"></param>
        /// <returns></returns>
        public static DataTable GetDepartmentDtls(int deptID, int deptParentPK, int bizUnit, int userGroup)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.DEPTPK  , deptID == 0 ? (object)DBNull.Value : deptID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.DEPTPARENT , deptParentPK),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters.USERGROUP  , userGroup == 0 ? (object)DBNull.Value : userGroup),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETDEPARTMENTDETAILS, colParameters).Tables[0];
        }
        /// <summary>
        /// Get Department Name 
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetDepartmentName(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETDEPARTMENTNAME, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Parent Departments Name .
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetParentDepartments(int bizUnit, int parent = -1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_Bizunit, bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ParentDept,parent == -1? (object)DBNull.Value : parent),
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETDEPARTMENTNAME, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Workflow Status
        /// </summary>
        /// <param name="refID">Referance ID</param>
        /// <param name="processID">Proces ID</param>
        /// <returns></returns>
        public static DataTable GetWorkflowStatus(int refID, int processID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_REF_ID, refID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PROCESS_ID, processID),
                
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETTRANSACTIONSTATUS, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Parent Departments categories Name .
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="parentDepartment"></param>
        /// <returns></returns>
        public static DataTable GetParentDepartmentCategories(int bizUnit, string parentDepartment, int CFG_PK = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CFG_TYPE, parentDepartment),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CfgPK, CFG_PK),
                
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETDEPARTMENTCATEGORIES, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Parent Departments categories Name .
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="parentDepartment"></param>
        /// <returns></returns>
        public static DataTable GetParentDepartmentCategoriesByID(int bizUnit, string parentDepartment)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BASE_DEPT, parentDepartment)
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETDEPARTMENTCATEGORIESBYID, colParameters).Tables[0];
        }


        /// <summary>
        /// AutoComplete For Country Details
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public static DataTable GetCountrySearchValues(string country)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE  ,  country),  
             
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.COUNTRYSEARCHAUTOCOMPLETE, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Get State Name For AutoComplete 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public static DataTable GetStateSearchValues(string state, int country)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE ,  country),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.COUNTRY ,  country),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.STATESEARCHAUTOCOMPLETE, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Currency Code Auto Complete
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static DataTable GetCurrencySearchValues(string currency)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE ,  currency),  
               
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.CURRENCYSEARCHAUTOCOMPLETE, colParameters).Tables[0];
            return dtSearchValue;

        }

        /// <summary>
        /// Get User Group List
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUserGroup()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters.USERGROUPFLAG , GTIService.Constants.Common.CommonConstant.UserGroupFlag),  
               
            };
            DataTable dtSearchValue = new DataTable();
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETUSERGROUPDETAILS, colParameters).Tables[0];
        }

        /// <summary>
        /// Get User has right to perform the initial task for the corresponding process
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="processID"></param>
        /// <returns>DatatTable</returns>
        public static DataTable GetInitialTaskPermission(int userID, int processID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , userID),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , processID),  
               
            };
            DataTable dtSearchValue = new DataTable();
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETINITIALTASKPERMISSION, colParameters).Tables[0];

        }
        /// <summary>
        /// Get Process List By UserPk
        /// </summary>
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static DataTable GetProcessList(int userPK)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters("P_USER_PK", userPK == 0 ? (object)DBNull.Value : userPK)                         
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETPROCESS, colParameters).Tables[0];
            return dtProcess;
        }

        /// <summary>
        /// Get Shift Corresponding to a bizUinit
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetEmployees(int bizUnit, string searchVal)
        {
            DataTable dtShift = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit == 0 ? (object)DBNull.Value : bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchVal == "" ? "%": searchVal)           
            };
            dtShift = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETEMPLOYEES, colParameters).Tables[0];
            return dtShift;
        }

        /// <summary>
        /// Get Shift Corresponding to a bizUinit
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetShift(int bizUnit, string searchVal)
        {
            DataTable dtShift = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit == 0 ? (object)DBNull.Value : bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchVal == "" ? "%": searchVal)           
            };
            dtShift = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETSHIFT, colParameters).Tables[0];
            return dtShift;
        }

        /// <summary>
        /// Get Plans Corresponding to a bizUinit
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetPlans(int bizUnit)
        {
            DataTable dtPlan = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit == 0 ? (object)DBNull.Value : bizUnit)                         
            };
            dtPlan = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETPLANS, colParameters).Tables[0];
            return dtPlan;
        }

        /// <summary>
        /// Get Shift Corresponding to a bizUinit
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetLines(int bizUnit)
        {
            DataTable dtShift = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit == 0 ? (object)DBNull.Value : bizUnit)                         
            };
            dtShift = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETLINES, colParameters).Tables[0];
            return dtShift;
        }

        /// <summary>
        /// Get Products Corresponding to a bizUinit
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetProducts(int bizUnit)
        {
            DataTable dtProduct = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit == 0 ? (object)DBNull.Value : bizUnit)                         
            };
            dtProduct = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETPRODUCT, colParameters).Tables[0];
            return dtProduct;
        }

        /// <summary>
        /// Get Products Details Corresponding to a ProductID
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetProductDetails(int productID)
        {
            DataTable dtProduct = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters("PRO_PK", productID == 0 ? (object)DBNull.Value : productID)                         
            };
            dtProduct = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETPRODUCTDETAILS, colParameters).Tables[0];
            return dtProduct;
        }

        /// <summary>
        /// Get CheckList Details Corresponding to a CheckListID
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetCheckListDetails(int CheckListID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters("P_CHT_PK", CheckListID == 0 ? (object)DBNull.Value : CheckListID)                         
            };
            // return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETCHECKLISTTEMP, colParameters).ToString();  
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETCHECKLISTTEMP, colParameters).Tables[0];
            string strRetVal = "";
            for (int i = 0; i < dtxml.Rows.Count; i++)
                strRetVal += dtxml.Rows[i][0].ToString();
            return strRetVal;
        }

        /// <summary>
        /// Get Compound Batch # Corresponding to a bizUinit
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetCompoundBatchno(int bizUnit, int prdType)
        {
            DataTable dtProduct = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit == 0 ? (object)DBNull.Value : bizUnit),
                new DBService.Parameters("PRO_TYPE", prdType == 0 ? (object)DBNull.Value : prdType)          
            };
            dtProduct = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETCOMPOUNDBATCH, colParameters).Tables[0];
            return dtProduct;
        }


        /// <summary>
        /// Get Template Details by Template ID 
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns>string</returns>
        //public static string GetTemplateByTemplateIDORTitle(int templateID, string templateTitle)
        //{
        //    DBService dbService = new DBService();
        //    DBService.Parameters[] colParameters = null;
        //    colParameters = new DBService.Parameters[] 
        //    {   
        //        new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TEMPLATEID  ,templateID == 0 ? (object)DBNull.Value : templateID  ),  
        //        new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TEMPLATETITLE  ,templateTitle == "0" ? (object)DBNull.Value : templateTitle   ),  
        //        new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL , string.Empty, 4000,ParameterDirection.Output, DBService.ParameterType.VarChar),

        //    };
        //    dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETTEMPLATEDETAILS, colParameters);
        //    return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);



        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refPk"></param>
        /// <returns></returns>
        public static DataSet GetWrkfCommentList(int refPk, int appID, int procID)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters("CMT_REF_ID", refPk == 0 ? (object)DBNull.Value : refPk),
                new DBService.Parameters("CMT_APP_ID", appID),
                new DBService.Parameters("CMT_PROC_ID", procID)        
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETWORKFLOWCOMMENT, colParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refPk"></param>
        /// <returns></returns>
        public static string SaveWrkfCommentList(BusinessObject.CommonManagement.CommonObject.WorkFlowComment workFlowComment)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters("CMT_PROC_ID", workFlowComment.ProcessID),      
                new DBService.Parameters("CMT_APP_ID", workFlowComment.ApplicationID),  
                new DBService.Parameters("CMT_REF_ID", workFlowComment.ReferenceID),  
                new DBService.Parameters("CMT_TASK", workFlowComment.TaskID),  
                new DBService.Parameters("CMT_ACTION", workFlowComment.ActionID),  
                new DBService.Parameters("CMT_DESC", workFlowComment.WrkfComment), 
                new DBService.Parameters("CMT_MOD_BY", workFlowComment.UserPk) 
            };
            return dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SAVEWORKFLOWCOMMENT, colParameters).ToString();
        }

        /// <summary>
        /// Methode used to get the menu auto search
        /// </summary>
        /// <param name="menuName"></param>
        /// <param name="userPK"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetMenuAutoComplete(string menuName, int userPK, int bizUnit, string menuType)
        {
            DataTable dtSearch = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters("P_PAG_TITLE", menuName == string.Empty ? (object)DBNull.Value : menuName),
                new DBService.Parameters(CommonConstants.USERPK, userPK),
                new DBService.Parameters(CommonConstants.BIZUNIT, bizUnit),
                new DBService.Parameters("P_TYPE_XML", menuType),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETMENUAUTOCOMPLETE, colParameters).Tables[0];
        }
        /// <summary>
        /// Get Inbox details for send mail
        /// </summary>
        /// <param name="RefID"></param>
        /// <returns></returns>
        private static DataTable GetInboxMail(int RefID)
        {
            DataTable dtSearch = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters("pRefrerence", RefID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GetMailTransactionDtl, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Intimation details for send mail
        /// </summary>
        /// <param name="RefID"></param>
        /// <returns></returns>
        private static DataTable GetIntimationMail(int RefID)
        {
            DataTable dtSearch = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters("pRefrerence", RefID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GetIntimationMailDtl, colParameters).Tables[0];
        }
        //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrkfReq"></param>
        /// <param name="wrkfcmts"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static int SaveWorkFlow(WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq, WorkflowCore.CoreObjects.WorkFlowComment wrkfcmts, User objUser)
        {
            int result;
            int refId = 0;
            int i = 0;
            DataTable dtMail;
            wrkfReq.Comments = wrkfcmts.WrkfComment;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            refId = wrkfService.DoWorkFlow(wrkfReq);
            //if (wrkfcmts.WrkfComment != string.Empty)
            //{
            //wrkfcmts.ReferenceID = refId;
            //result = wrkfService.SaveComments(wrkfcmts);
            ////}
            //if (wrkfcmts.wrkfAdditionalComments != null)
            //{
            //    foreach (var item in wrkfcmts.wrkfAdditionalComments)
            //    {
            //        wrkfcmts.WrkfComment = item;
            //        result = wrkfService.SaveComments(wrkfcmts);
            //    }
            //}
            //Mail sending code
            try
            {
                //if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableWkfMail"]))
                //{
                //    //Inbox mail
                //    dtMail = GetInboxMail(refId);
                //    if (dtMail.Rows.Count > 0)
                //    {
                //        for(i=0;i<dtMail.Rows.Count;i++)
                //            CommonFunctions.SendMail(dtMail.Rows[i]["usrSubject"].ToString(), dtMail.Rows[i]["usrMessage"].ToString(), dtMail.Rows[i]["usrEmail"].ToString());
                //    }
                //    //Intimation Mail
                //    dtMail = GetIntimationMail(refId);
                //    if (dtMail.Rows.Count > 0)
                //    {
                //        for (i = 0; i < dtMail.Rows.Count; i++)
                //         CommonFunctions.SendMail(dtMail.Rows[i]["usrSubject"].ToString(), dtMail.Rows[i]["usrMessage"].ToString(), dtMail.Rows[i]["usrEmail"].ToString());
                //    }
                //}

            }
            catch
            { }
            return refId;
        }
        public static DataTable GetUserCustomer(int userPK, int bizUnit)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(CommonConstants.USER, userPK),
                new DBService.Parameters(CommonConstants.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GET_USERCUSTOMER, colParameters).Tables[0];
        }

        public static DataTable GetTemplateCheckList(int checkListPK, int bizUnit, int active, int processID)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_CHT_PK, checkListPK == 0 ? (object)DBNull.Value : checkListPK),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_CHT_PROCESS, processID == 0 ? (object)DBNull.Value : processID),                         
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit == 0 ? (object)DBNull.Value : bizUnit)     
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETTEMPLATECHECKLIST, colParameters).Tables[0];
        }
        public static DataSet GetProductIterationReport(int testPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
             {  
                new DBService.Parameters("P_TTH_PK", testPk)
             };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPQUC_TEST_TRX_DTL_RPT", colParameters);
        }
        public static DataSet GetBincardProductReport(int productPk, DateTime fromDate, DateTime toDate, int linePk, int shiftPk, int productType, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_PRODUCT, productPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE, fromDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, toDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_LINE, linePk== -1 ? (object)DBNull.Value : linePk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_SHIFT, shiftPk== -1 ? (object)DBNull.Value : shiftPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_PRODUCTION_TYPE, productType),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GET_BINCARD_PRODUCT_REPORT, colParameters);
        }
            public static DataTable GetPoCreators(int bizUnit)
            {
                DataTable dtPoCreators = new DataTable();
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[]
                {
                 new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_empBizUnit  , bizUnit==0?(object)DBNull.Value  : bizUnit),
                };
            DataTable dt = new DataTable();
            dtPoCreators = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETPOCREATOR, colParameters).Tables[0];
                return dtPoCreators;
            }
            #region Material Issue (External Material Issue Multiple)

            /// <summary>
            /// Get Issue Against DDL
            /// </summary>
            /// <param name="bizUnit"></param>
            /// <param name="parentDepartment"></param>
            /// <returns></returns>
            public static DataTable GetIssueAgainst(int bizUnit, string issueAgainst, string splCondition)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CFG_TYPE, issueAgainst),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CFG_SPL_COND, splCondition),
                
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETDEPARTMENTCATEGORIES, colParameters).Tables[0];
        }

        public static DataTable GetAssetDetails(int assetPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[] 
            {        
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_asrPK,  assetPK > 0 ?  assetPK: (object) DBNull.Value)                
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPASSET_DETAILS_SRM_GET, colParameters).Tables[0];
            return dtParams;
        }

        /// <summary>
        /// Get Type DDL
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="parentDepartment"></param>
        /// <returns></returns>
        public static DataTable GetType(int bizUnit, int IssueAgainst)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ICH_ISS_RCV_TYPE, IssueAgainst)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPINV_ITEM_EXT_ISS_RCV_SUB_GET_KV, colParameters).Tables[0];
            //return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETIISUINGYOLIST, colParameters).Tables[0];
        }
        #endregion

        #region Work Flow Level Change
        /// <summary>
        /// Get Department
        /// </summary>
        public static DataTable GetWorkFlowDepartment(int Active = 1, int HasWkfCfg = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.DPT_ACTIVE, Active),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_DPT_HAS_WKF_CFG, HasWkfCfg),
            };
            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPADM_DEPT_MST_GET_KV, colParameters).Tables[0];
            return dtResult;

        }
        /// <summary>
        /// Get Process
        /// </summary>
        /// <param name="DeptPK"></param>
        /// <returns></returns>
        public static DataTable GetProcess(int DeptPK, int Status = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.PDept, DeptPK),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.PprcStatus, Status)
            };
            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SpwkfProcessMstGetKV, colParameters).Tables[0];
            return dtResult;

        }
        /// <summary>
        /// Get Task
        /// </summary>
        /// <param name="ProcessPK"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static DataTable GetSequenceTask(int ProcessPK, int Status = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.pPrcPK, ProcessPK)
            };
            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SpWkfSequenceTaskGetList, colParameters).Tables[0];
            return dtResult;
        }
        /// <summary>
        /// Save Sequence Config
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveSequenceConfig(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SpWkfSequenceConfigSave, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Get Sequence Task Actions
        /// </summary>
        /// <param name="taskPK"></param>
        /// <returns></returns>
        public static DataTable GetSequenceTaskActions(int taskPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.pTskPK, taskPK)
            };
            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SpWkfSequenceTaskActionGetList, colParameters).Tables[0];
            return dtResult;
        }
        /// <summary>
        /// Get Sequence Task Action List
        /// </summary>
        /// <param name="WsqPK"></param>
        /// <param name="WsqNextTask"></param>
        /// <param name="WsqType"></param>
        /// <returns></returns>
        public static string GetSequenceTaskActionList(int WsqPK, int WsqNextTask, int WsqType)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters("pWsqPK", WsqPK>0 ? WsqPK :(object) DBNull.Value),
                new DBService.Parameters("pWsqNextTask",  WsqNextTask>0  ? WsqNextTask : (object) DBNull.Value),
                new DBService.Parameters("pWsqType",  WsqType)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SpWkfSequenceTaskActionList, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static DataTable GetWorkflowTransactions(int TaskPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.pTskPK, TaskPk)
            };
            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SpWkfSequencePendTrxList, colParameters).Tables[0];
            return dtResult;
        }

        public static int DeleteWorkflowTask(int TaskPk, int Level)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.pWsqTask, TaskPk),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.pWsqLevel, Level),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.pRetVal, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.DELETEINVOICEDETAILS, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        #endregion

    }
}
