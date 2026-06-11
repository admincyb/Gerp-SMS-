using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Common;
using BusinessObject.CommonManagement;
using BusinessObject;

namespace DataAccess.CommonManagement
{
    public class CommonDA
    {
        /// <summary>
        /// method for Get BizUnit and Department
        /// </summary>
        /// <param name="userID" Type=int></param>
        /// <returns>DataTable</returns>
        public static DataTable GetBizUnit(int userID, int sbuID)
        {
            DataTable dtsbu;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_BIZUNIT, sbuID==0?(object)DBNull.Value:sbuID),
                new DBService.Parameters( Common.P_USERPK, userID),

            };
            dtsbu = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETSBUDEPARTMENT, colParameters).Tables[0];
            return dtsbu;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataTable GetBizUnit(int sbuID)
        {
            DataTable dtsbu;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_BZU_PK, sbuID==0?(object)DBNull.Value:sbuID),
            };
            dtsbu = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETSBU, colParameters).Tables[0];
            return dtsbu;
        }

        /// <summary>
        /// Get all active bizunit
        /// </summary>
        /// <param name="sbuID"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetAllBizUnit(int sbuID, int Active)
        {
            DataTable dtsbu;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_BZU_PK, sbuID == 0 ? (object)DBNull.Value : sbuID),
                new DBService.Parameters(Common.P_ACTIVE, Active == 0 ? (object)DBNull.Value : Active)
            };
            dtsbu = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETSBU, colParameters).Tables[0];
            return dtsbu;
        }


        /// <summary>
        /// Get Department Details
        /// </summary>
        /// <param name="userDept"></param>
        /// <returns></returns>
        public static DataTable GetDepartmentDetails(int userDept)
        {
            DataTable dtsbu;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.F_DEPARTMENT, userDept)
            };
            dtsbu = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GET_DEPT_MST, colParameters).Tables[0];
            return dtsbu;
        }

        /// <summary>
        /// Get Department Page Right
        /// </summary>
        /// <param name="userPK"></param>
        /// <param name="pageURL"></param>
        /// <param name="deptPK"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetDepartmentPageRight(int userPK, string pageURL, int deptPK, int active)
        {
            DataTable dtsbu;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_ACTIVE, active),
                new DBService.Parameters(Common.P_USERPK, userPK),
                new DBService.Parameters(Common.P_PAGE_URL, pageURL),
                new DBService.Parameters(Common.P_DEPT_PK, deptPK),
            };
            dtsbu = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GET_DEPT_PAGE_GET_KV, colParameters).Tables[0];
            return dtsbu;
        }


        public static DataTable GetReportDetails(string RptType, int RptSubType, DateTime AppvdDt)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_APT_CODE, RptType),
                new DBService.Parameters(Common.P_AST_VALUE, RptSubType),
                new DBService.Parameters(Common.P_TRX_DATE,AppvdDt)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_APP_SUB_TYPE_DATA_GET, colParameters).Tables[0];
        }
        public static DataTable GetQueryCFGValues(int queryPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_QRY_PK, queryPK),
                new DBService.Parameters(Common.P_ACTIVE, 2)//Has PK
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_QUERIES_CFG_GET_KV, colParameters).Tables[0];
        }


        public static DataTable GetDepartmentList(int? deptPk = null, int? active = null, int? parentDept = null, int? deptType = null, int? deptCategory = null)
        {
            DataTable dtTable;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.F_DEPARTMENT, deptPk.HasValue && deptPk.Value > 0 ? deptPk.Value : (object)DBNull.Value ),
                new DBService.Parameters(Common.P_DPT_ACTIVE, active.HasValue ? active.Value : (object)DBNull.Value ),
                new DBService.Parameters(Common.P_DPT_PARENT, parentDept.HasValue ? parentDept.Value : (object)DBNull.Value ),
                new DBService.Parameters(Common.P_DPT_TYPE, deptType.HasValue ? deptType.Value : (object)DBNull.Value ),
                new DBService.Parameters(Common.P_DPT_CATEGORY, deptCategory.HasValue ? deptCategory.Value : (object)DBNull.Value ) ,
            };
            dtTable = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GET_DEPT_MST, colParameters).Tables[0];
            return dtTable;
        }
        /// <summary>
        /// Get Mail CFG
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMailCFGDA()
        {
            DataTable dtMailCfg;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            dtMailCfg = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GET_MAIL_CFG, colParameters).Tables[0];
            return dtMailCfg;
        }

        /// <summary>
        /// Get Account Type
        /// </summary>
        /// <param name="accountPk"></param>
        /// <param name="subType"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetAccountType(int accountPk, int subType, int active)
        {
            DataTable dtsbu;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_COA_PK, accountPk),
                new DBService.Parameters(Common.P_COA_SUB_TYPE, subType==0?(object)DBNull.Value:subType),
                new DBService.Parameters(Common.P_ACTIVE, active==0?(object)DBNull.Value:active)
            };
            dtsbu = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_COA_MST_GET_KV, colParameters).Tables[0];
            return dtsbu;
        }



        /// <summary>
        /// Get ApplicationStatus
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="subType"></param>
        /// <returns></returns>
        public static DataTable GetAppStatus(string Type, string subType)
        {
            DataTable dtsbu;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_ASC_TYPE, Type),
                new DBService.Parameters(Common.P_ASC_SUB_TYPE_VAL, subType == string.Empty ?(object)DBNull.Value:subType),
            };
            dtsbu = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_APP_STATUS_CFG_GET_KV, colParameters).Tables[0];
            return dtsbu;
        }
        /// <summary>
        /// Get  Get Account Type
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static DataTable GetAccountType(string xml)
        {
            DataTable dtsbu;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_XML, xml)
            };
            dtsbu = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_COA_MST_LIST, colParameters).Tables[0];
            return dtsbu;
        }
        /// <summary>
        /// Get Plant
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static DataTable GetPlant(string xml)
        {
            DataTable dtsbu;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_XML, xml)
            };
            dtsbu = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_PLANT_MST_GET_KV, colParameters).Tables[0];
            return dtsbu;
        }

        /// <summary>
        /// Get Exchange Rate
        /// </summary>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        /// <param name="date"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetExchangeRate(int fromCurrency, int toCurrency, DateTime date)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_FROM, fromCurrency),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_TO, toCurrency),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_DATE, date)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_CURRENCY_CONV_FACT_GET, colParameters).Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poID"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetExchangeRate(int fromCurrency, int toCurrency, string date)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_FROM, fromCurrency),
               new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_TO,  toCurrency),
               new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CUR_DATE, Convert.ToDateTime(date))
            };
            DataTable dtRate = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.SPADM_CURRENCY_CONV_FACT_GET, colParameters).Tables[0];
            return dtRate;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poID"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetCurrencyHold(int BankPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CBM_PK, BankPk),

            };
            DataTable dtRate = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.SPFIN_BANK_CUR_GET, colParameters).Tables[0];
            return dtRate;

        }
        /// <summary>
        /// method for Get BizUnit and Department
        /// </summary>
        /// <param name="userID" Type=int></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDepartment(int userID, int sbuID, string searchkey = null)
        {
            DataTable dtDept;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_BIZUNIT, sbuID==0?(object)DBNull.Value:sbuID),
                new DBService.Parameters( Common.P_USERPK, userID),
                 new DBService.Parameters( Common.P_ACTIVE,CommonConstants.SELECT_VALUE_ONE),
                 new DBService.Parameters( Common.P_DEPT, string.IsNullOrEmpty(searchkey) ? (object)DBNull.Value : searchkey),

            };
            dtDept = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETSBUDEPARTMENT, colParameters).Tables[1];
            return dtDept;
        }

        /// <summary>
        /// method for Get BizUnit and Department
        /// </summary>
        /// <param name="userID" Type=int></param>
        /// <returns>DataTable</returns>
        public static DataTable GetMenuDepartment(int userID, int sbuID, string searchkey = null)
        {
            DataTable dtDept;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_BIZUNIT, sbuID==0?(object)DBNull.Value:sbuID),
                new DBService.Parameters( Common.P_USERPK, userID),
                //new DBService.Parameters( Common.P_ACTIVE,CommonConstants.SELECT_VALUE_ONE),
                new DBService.Parameters( Common.P_DEPT, string.IsNullOrEmpty(searchkey) ? (object)DBNull.Value : searchkey),

            };
            dtDept = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETSUSERMENU, colParameters).Tables[0];
            return dtDept;
        }






        /// <summary>
        /// method for Get AgentListin Dropdown
        /// </summary>
        /// <param name="userID" Type=int></param>
        /// <returns>DataTable</returns>
        public static DataTable GETAGENTLIST(int sbuID)
        {
            DataTable dtDept;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_BIZUNIT, sbuID==0?(object)DBNull.Value:sbuID),                
                //new DBService.Parameters( Common.P_USERPK, userID),
                ////new DBService.Parameters( Common.P_ACTIVE,CommonConstants.SELECT_VALUE_ONE),
                //new DBService.Parameters( Common.P_DEPT, string.IsNullOrEmpty(searchkey) ? (object)DBNull.Value : searchkey), 

            };
            dtDept = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_AGENT_MST_GET_KV, colParameters).Tables[0];
            return dtDept;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="sbuID"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public static DataTable GetDepartment(int userID, int sbuID, int module)
        {
            DataTable dtDept;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_BIZUNIT, sbuID==0?(object)DBNull.Value:sbuID),
                new DBService.Parameters( Common.P_USERPK, userID),
                 new DBService.Parameters( Common.P_MODULE,module)

            };
            dtDept = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETSUSERMENU, colParameters).Tables[0];
            return dtDept;
        }
        /// <summary>
        /// Method for Get  Department URL
        /// </summary>
        /// <param name="deptPk" Type=int></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDepartmentUrl(int deptPk)
        {
            DataTable dtDept;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_DEPT_PK, deptPk==0?(object)DBNull.Value:deptPk),
                new DBService.Parameters( Common.P_DPT_ACTIVE,CommonConstants.SELECT_VALUE_ONE)
            };
            dtDept = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_DEPT_MST_GET, colParameters).Tables[0];
            return dtDept;
        }


        /// <summary>
        /// Get Process List By UserPk and department
        /// </summary>
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static DataTable GetProcessList(int userID, int depID, string searchkey = null, int sbu = 0)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_USERPK, userID == 0 ? (object)DBNull.Value : userID),
                new DBService.Parameters( Common.P_DEPARTMENT, depID > 0 ? depID : (object)DBNull.Value  ),
                new DBService.Parameters( Common.P_PRCNAME, string.IsNullOrEmpty(searchkey) ? (object)DBNull.Value : searchkey),
                new DBService.Parameters( Common.P_BIZUNIT, sbu> 0 ? sbu : (object)DBNull.Value),
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETPROCESSLIST, colParameters).Tables[0];
            return dtProcess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static DataTable GetUsrGroups(int flag)
        {
            DataTable dtGroups = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_USRGRPFLAG, flag == 0 ? (object)DBNull.Value : flag),

            };
            dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETUSERGROUPS, colParameters).Tables[0];
            return dtGroups;
        }
        /// <summary>
        /// Get favourite List 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageID"></param>
        /// <param name="sbu"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetFavouritesList(int userID, int pageID, int sbu, string menuType, string path)
        {
            DataTable dtList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_USERPK, userID == 0 ? (object)DBNull.Value : userID),
                new DBService.Parameters( Common.P_BIZUNIT, sbu == 0 ? (object)DBNull.Value : sbu),
                new DBService.Parameters( Common.P_PAGEPK, pageID == 0 ? (object)DBNull.Value : pageID),
                new DBService.Parameters("P_TYPE_XML", menuType),
                new DBService.Parameters("P_Path", path),
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETFAVOURITESLIST, colParameters).Tables[0];
            return dtList;
            //return null;
        }
        /// <summary>
        /// Add to favourite List 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageID"></param>
        /// <param name="sbu"></param>
        /// <returns>DataTable</returns>
        public static DataTable AddToFavouritesList(int userID, int pageID, int sbu)
        {
            DataTable dtList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_USERPK, userID == 0 ? (object)DBNull.Value : userID),
                new DBService.Parameters( Common.P_BIZUNIT, sbu == 0 ? (object)DBNull.Value : sbu),
                new DBService.Parameters( Common.P_PAGEPK, pageID == 0 ? (object)DBNull.Value : pageID),
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_ADDTOFAVOURITELIST, colParameters).Tables[0];
            return dtList;


        }
        /// <summary>
        /// Get page id 
        /// </summary>
        /// <param name="pageURL"></param>
        /// <returns>int</returns>
        public static int GetPageId(string pageURL)
        {
            DataTable dtList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            int pageId = 0;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_PAGEURL, pageURL),

            };
            // return Convert.ToInt32(dbService.ExecuteReader(CommandType.StoredProcedure,  Common.SP_GETPAGEID, colParameters));
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETPAGEID, colParameters).Tables[0];

            if (dtList != null && dtList.Rows.Count > 0)
            {
                pageId = Convert.ToInt32(dtList.Rows[0]["PAG_PK"]);

            }
            return pageId;
        }
        /// <summary>
        /// Get page Info 
        /// </summary>
        /// <param name="pageURL"></param>
        /// <returns>DataSet</returns>
        public static DataTable GetPageInfo(string pageURL)
        {
            DataTable dtList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_PAGEURL, pageURL),

            };
            return dtList = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETPAGEID, colParameters).Tables[0];

        }
        public static DataTable GetWorkFlowDetails(int ApplicationID, int ProcessID)
        {
            DataTable dtWorkFlow = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_WORKFLOW_APP_ID, ApplicationID),
                new DBService.Parameters( Common.P_WORKFLOW_PEOCESS_ID, ProcessID)
            };
            dtWorkFlow = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GET_WORKFLOW_DETAILS, colParameters).Tables[0];
            return dtWorkFlow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static DataTable GetRelatedItemsWidget(int processID, int appId, string path)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("P_PROCESS", processID),
                new DBService.Parameters("P_APP_PK", appId),
                new DBService.Parameters("P_PAG_URL", path),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GET_RELATED_WIDGET, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static DataTable GetPageDept(int pageId, int userPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("P_PAG_PK", pageId),
                new DBService.Parameters("P_USER", userPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GET_PAGE_DEPT, colParameters).Tables[0];
        }

        /// <summary>
        /// Check Is Edoc User
        /// </summary>
        /// <param name="userPK"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataTable IsEdocUser(int userPK, int sbuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("P_BIZUNIT", sbuID),
                new DBService.Parameters("P_USER_PK", userPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GET__DOC_USER_CHECK, colParameters).Tables[0];
        }
        /// <summary>
        /// Check Is Dashboard User
        /// </summary>
        /// <param name="userPK"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataTable IsDashBoardUser(int userPK, int sbuID, int IsHome = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("P_BIZUNIT", sbuID),
                new DBService.Parameters("P_USER_PK", userPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_HOME, IsHome),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_DASHBOARD_USER_CHECK, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configText"></param>
        /// <param name="spclCond"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetApplicaitonConfiguaration(string configText, string spclCond, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters("P_ACF_SETTING", configText),
               new DBService.Parameters("P_ACF_SPEC_COND", spclCond == string.Empty ? (object)DBNull.Value : spclCond),
               new DBService.Parameters("P_BIZUNIT", bizUnit==0 ? (object)DBNull.Value : bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_APP_CONFIG_GET, colParameters).Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configText"></param>
        /// <param name="configData"></param>
        /// <returns></returns>
        public static DataTable GetApplicaitonConfiguaration(string configText, string configData)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters("P_ACF_SETTING", configText),
               new DBService.Parameters("P_ACF_DATA", configData == string.Empty ? (object)DBNull.Value : configData),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_APP_CONFIG_GET, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configText"></param>
        /// <param name="spclCond"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetCurrencyConfiguration(string configText, string configData, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters("P_ACF_SETTING", configText),
               new DBService.Parameters("P_ACF_DATA", configData == string.Empty ? (object)DBNull.Value : configData),
               new DBService.Parameters("P_BIZUNIT", bizUnit==0 ? (object)DBNull.Value : bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_APP_CONFIG_GET, colParameters).Tables[0];
        }
        /// <summary>
        /// Get Transaction ID
        /// </summary>
        /// <param name="pRefPK"></param>
        /// <returns></returns>
        public static DataTable GetTransactionID(int pRefPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters("pRefPK", pRefPK),
            };
            DataSet ds = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GET_TRANSACTION_PROCESS, colParameters);
            if (ds.Tables.Count > 0) return ds.Tables[0];
            else return new DataTable();

            //return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GET_TRANSACTION_PROCESS, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Department Company
        /// </summary>
        /// <param name="deptPK"></param>
        /// <returns></returns>
        public static DataTable GetDeptCompany(int deptPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters("DPT_PK", deptPK),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_DEPT_MST_GET, colParameters).Tables[0];
        }

        public static DataTable GetAppConfig(int bizUnit, string configType, string splCond)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CFG_TYPE, configType),
                new DBService.Parameters( Common.P_CFG_SPL_COND, !string.IsNullOrEmpty(splCond)?splCond:(object)DBNull.Value)

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETAPPCINFIG, colParameters).Tables[0];
        }

        public static DataTable GetLineDetails(DateTime date, string P_VALUE = "")
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DATE, date),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_VALUE, P_VALUE),
                 

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPQUC_PROC_CTRL_TRX_LINE_FILTER, colParameters).Tables[0];
        }
        public static DataTable GetLineDetailsforOnlineParameter(DateTime date, string P_VALUE = "")
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DATE, date),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_VALUE, P_VALUE),
                 

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPQUC_PROC_CTRL_TRX_PROD_LINE_FILTER, colParameters).Tables[0];
        }

        public static DataTable GetLineProductDetails(DateTime date, string Product)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DATE, date),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LNE_PK, Product),


            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPQUC_PROC_CTRL_TRX_PRODUCT_FILTER, colParameters).Tables[0];
        }


        public static DataTable GetLineProductDetailsforOnlineParameter(DateTime date, string Product)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DATE, date),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LNE_PK, Product),


            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPQUC_PROC_CTRL_TRX_PROD_PRODUCT_FILTER, colParameters).Tables[0];
        }

        public static DataTable GetUOM(int active, int? itemPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_PK, itemPK.HasValue?(int)itemPK.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_UOM_ACTIVE, active),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPINV_ITEM_UOM_GET_KV, colParameters).Tables[0];
        }

        public static DataTable GetAllUOMList(int Bizunit, int active, int? UomPK, int? UomType, int? UomStatus)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_UOM_PK, UomPK.HasValue?(int)UomPK.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_UOM_TYPE, UomType.HasValue?(int)UomType.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_UOM_STATUS, UomStatus.HasValue?(int)UomStatus.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.UOM_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, Bizunit),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.GETUOMLIST, colParameters).Tables[0];
        }


        public static DataTable GetAppConfigTree(int? machinePK, int? tankPK, short active = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_MCH_PK, machinePK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_TNK_PK, tankPK) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPPRD_PRODUCTION_PROCESS_MAP_TREE, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Trx No by Application Type
        /// </summary>
        /// <param name="appType"></param>
        /// <param name="subType"></param>
        /// <param name="dept"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetTrxNo(string appType, int subType, int dept, int user)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_APT_CODE, appType),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_AST_VALUE, subType > 0 ? subType : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DEPT,  dept > 0 ? dept : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER, user),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DATE, DateTime.Now),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_UPDATE, 0),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_APP_PK, 0),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            DataSet dsTrxNo = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GET_TRX_DOC_NO, colParameters);
            string result = dsTrxNo != null && dsTrxNo.Tables.Count > 0 ? Convert.ToString(dsTrxNo.Tables[0].Rows[0][0]) : string.Empty;
            return result;
        }
        /// <summary>
        /// Execute SP
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataSet ExecuteSP(string spName, string pXML, bool IsReportServer = false)
        {
            DBService dbService = new DBService(IsReportServer);
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, pXML),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, spName, colParameters);
        }
        /// <summary>
        /// Exception Writing
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="remarks"></param>
        public static void ExceptionWriting(string exception, string remarks)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters("P_EXCEPTION", exception),
                 new DBService.Parameters("P_REMARKS", remarks),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Common.SP_ExceptionLog, colParameters);
            return;
        }
        /// <summary>
        /// Execute Non Query SP
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int ExecuteNonQuerySP(string spName, string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, pXML),
                new DBService.Parameters(CommonConstants.P_RetVal, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, spName, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.P_RetVal]).Value);
            return result;

        }

        /// <summary>
        /// Get Constant Table Values
        /// </summary>
        /// <param name="constPK"></param>
        /// <param name="constGroup"></param>
        /// <param name="groupType"></param>
        /// <param name="groupValue"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetConstMstValues(int constPK, int constGroup, ConstGroupType? groupType, int groupValue, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK, constPK>=0?constPK:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_GROUP, constGroup>0?constGroup:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CGT_VALUE,groupType.HasValue?(int)groupType.Value:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CNG_VALUE,groupValue>0?groupValue:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_CONST_MST_GET, colParameters).Tables[0];
        }
        /// <summary>
        /// Get ConstMst Values Auto
        /// </summary>
        /// <param name="constPK"></param>
        /// <param name="constGroup"></param>
        /// <param name="groupType"></param>
        /// <param name="groupValue"></param>
        /// <param name="searchValue"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetConstMstValuesAuto(int constPK, int constGroup, ConstGroupType? groupType, int groupValue, string searchValue, int active, int bizUnit, int IsProductRequired)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK, constPK>=0?constPK:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_GROUP, constGroup>0?constGroup:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CGT_VALUE,groupType.HasValue?(int)groupType.Value:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CNG_VALUE,groupValue>0?groupValue:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_BIZUNIT, bizUnit),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_NAME, string.IsNullOrEmpty(searchValue) ? "%%" : searchValue),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.IS_PRODUCT, IsProductRequired>0?IsProductRequired:(object)DBNull.Value),

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_CONST_MST_GET, colParameters).Tables[0];
        }
        public static DataTable GetPageDepartment(int constPK, int constGroup, ConstGroupType? groupType, int groupValue, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK, constPK>=0?constPK:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_GROUP, constGroup>0?constGroup:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CGT_VALUE,groupType.HasValue?(int)groupType.Value:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CNG_VALUE,groupValue>0?groupValue:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPINV_DEPT_PAGE_GET_KV, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Categro Value
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetCategoryValue(int categoryPK, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.ITC_PK , categoryPK>0 ? categoryPK:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.ITC_ACTIVE , active),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPINV_ITEM_CATEGORY_GET, colParameters).Tables[0];
        }


        /// <summary>
        /// Get WorkFlow Inbox details for send mail
        /// </summary>
        /// <param name="RefID"></param>
        /// <returns></returns>
        public static DataTable GetInboxMail(int RefID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_REFID, RefID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETINBOXMAIL, colParameters).Tables[0];
        }

        /// <summary>
        /// Get WorkFlow Intimation details for send mail
        /// </summary>
        /// <param name="RefID"></param>
        /// <returns></returns>
        public static DataTable GetIntimationMail(int RefID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_REFID, RefID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETINTIMATIONMAIL, colParameters).Tables[0];
        }
        /// <summary>
        /// Get currency List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetCurrencyList(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETCURRENCY, colParameters).Tables[0];
        }
        /// <summary>
        /// Get currency List By BtzuUnit
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetCurrencyListByBtzuUnit(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.CUR_PK, curPK),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.CUR_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETCURRENCYBYBTZUUNIT, colParameters).Tables[0];
        }



        /// <summary>
        /// Get currency List By BtzuUnit
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable BrandRateGetCurrencyListByBtzuUnit(int curPK, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.CUR_PK, curPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.CUR_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETCURRENCY, colParameters).Tables[0];
        }



        /// <summary>
        ///  Get Category List By BtzuUnit
        /// </summary>
        /// <param name="conPK"></param>
        /// <param name="conActive"></param>
        /// <param name="conGroup"></param>
        /// <param name="cgtValue"></param
        /// <param name="cngValue"></param>
        /// <param name="btzuUit"></param>
        /// <returns></returns>
        public static DataTable GetCategoryList(int conPK, int conActive, string conGroup, int cgtValue, int cngValue, int btzuUit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK,conPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_ACTIVE,conActive),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_GROUP,conGroup),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CGT_VALUE,cgtValue),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CNG_VALUE,cngValue),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_BIZUNIT,btzuUit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_SPADM_CONST_MST_GET_KV, colParameters).Tables[0];
        }

        public static DataTable GetConstMstList(int conPK, int conActive, int conGroup, int cgtValue, int cngValue, int btzuUit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK,conPK >0 ?conPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_ACTIVE,conActive>0 ? conActive :(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_GROUP,conGroup> 0 ? conGroup:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CGT_VALUE,cgtValue>0 ? cgtValue :(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CNG_VALUE,cngValue>0 ? cngValue :(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_BIZUNIT,btzuUit>0 ? btzuUit :(object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_SPADM_CONST_MST_GET_KV, colParameters).Tables[0];
        }


        /// <summary>
        /// WorkFlow Page Task Permission
        /// </summary>
        /// <param name="refID"></param>
        /// <param name="pageUrl"></param>
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static bool GetPageTaskPermission(int process, string pageUrl, int refID = 0, int userPK = 0)
        {
            DataTable dtSearch = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(CommonConstants.P_Refrerence, refID > 0 ? refID : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.P_PageUrl, pageUrl),
                new DBService.Parameters(CommonConstants.P_Process, process),
                new DBService.Parameters(CommonConstants.P_UserPK, userPK > 0 ? userPK : (object)DBNull.Value)
            };
            dtSearch = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GetPageTaskPermission, colParameters).Tables[0];
            if (dtSearch != null && dtSearch.Rows.Count > 0)
                return true;
            else return false;
        }

        /// Save Inbox
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static int SaveMessageTime(int UserPk, byte? inboxType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(CommonConstants.P_UserPK, UserPk > 0 ? UserPk : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.P_UserInoxType, inboxType !=null ? inboxType : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.P_RetVal, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SaveMessageInbox, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.P_RetVal]).Value);
            return result;
        }
        /// <summary>
        /// Save As CRM Lead
        /// </summary>
        /// <param name="customerPk"></param>
        /// <returns></returns>
        public static int SaveAsCRMLead(long customerPk, int pRefID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(CommonConstants.P_CustomerPK, customerPk > 0 ? customerPk : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.P_REFID, pRefID > 0 ? pRefID : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPCRM_CUSTOMER_MST_CRM_LEAD_MST_TRFER_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            return result;
        }

        /// <summary>
        /// Save As CRM Product
        /// </summary>
        /// <param name="productPk"></param>
        /// <returns></returns>
        public static int SaveAsCRMProduct(long productPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(CommonConstants.P_ItemPK, productPk > 0 ? productPk : (object)DBNull.Value),
                new DBService.Parameters(CommonConstants.RETURNVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPCRM_PRODUCT_MST_INV_ITEM_MST_TRFER_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            return result;
        }



        public static DataTable GetTrxTypeList(int aptPK, int? modPk, byte active, int bizUnit, string splCond)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_APT_PK, aptPK),
                new DBService.Parameters( Common.P_MOD_PK, modPk.HasValue?(modPk.Value!=0?(object)DBNull.Value:modPk.Value):(object)DBNull.Value),
                new DBService.Parameters( Common.P_ACTIVE, active),
                new DBService.Parameters( Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters( Common.P_APT_SPL_COND, !string.IsNullOrEmpty(splCond)?splCond:(object)DBNull.Value),
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_APP_TYPE_MST_GET_KV, colParameters).Tables[0];
            return dtProcess;
        }


        /// <summary>
        /// Get Item Pack Details
        /// </summary>
        /// <param name="ipdPK"></param>
        /// <param name="itemPK"></param>
        /// <param name="ipdType"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetItemPackDetails(int ipdPK, int itemPK, int ipdType, int ipdCustomer, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IPD_PK, ipdPK>0?ipdPK:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_PK, itemPK>0?itemPK: (itemPK==-1? itemPK: (object)DBNull.Value)),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IPD_TYPE,ipdType>0?ipdType:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IPD_CUSTOMER,ipdCustomer>0?ipdCustomer:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPINV_ITEM_PACK_DTL_GET_KV, colParameters).Tables[0];
        }

        /// <summary>
        /// Get Item Pack Details
        /// </summary>
        /// <param name="ipdPK"></param>
        /// <param name="itemPK"></param>
        /// <param name="ipdType"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetItemPackDetailsAuto(int ipdPK, int itemPK, int ipdType, int ipdCustomer, int active, int bizUnit, string stringValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IPD_PK, ipdPK>0?ipdPK:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_PK, itemPK>0?itemPK: (itemPK==-1? itemPK: (object)DBNull.Value)),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IPD_TYPE,ipdType>0?ipdType:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IPD_CUSTOMER,ipdCustomer>0?ipdCustomer:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITEM, stringValue)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPINV_ITEM_PACK_DTL_GET_KV, colParameters).Tables[0];
        }

        public static DataTable GetTaxMstList(int taxPK, int taxCategory, int taxSubCategory, byte active, int bizUnit)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.TAX_PK, taxPK ==0? (object)DBNull.Value: taxPK  ),
                new DBService.Parameters( Common.TAX_CATEGORY, taxCategory),
                new DBService.Parameters( Common.P_ACTIVE, active),
                new DBService.Parameters( Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters( Common.TAX_SUB_CATEGORY, taxSubCategory),
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_TAX_MST_GET_KV, colParameters).Tables[0];
            return dtProcess;
        }

        public static DataTable GetTaxMstListAuto(int taxPK, int taxCategory, int taxSubCategory, byte active, int bizUnit, string searchValue)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.TAX_PK, taxPK ==0? (object)DBNull.Value: taxPK  ),
                new DBService.Parameters( Common.TAX_CATEGORY, taxCategory),
                new DBService.Parameters( Common.P_ACTIVE, active),
                new DBService.Parameters( Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters( Common.TAX_SUB_CATEGORY, taxSubCategory),
                new DBService.Parameters( Common.P_TAX_HEAD, searchValue)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_TAX_MST_GET_AUTO, colParameters).Tables[0];
            return dtProcess;
        }

        public static bool ValidationForCancellation(int CurrPK, string type)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_TRX_PK, CurrPK > 0 ? CurrPK : (object)DBNull.Value),
                new DBService.Parameters(Common.P_TRX_APP_TYPE, type)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_TRX_CANCEL_CHECK, colParameters);
            return dsArchive == null || dsArchive.Tables.Count == 0 || dsArchive.Tables[0].Rows.Count == 0;
        }

        public static int ValidationForPosting(string strXml)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_XML, strXml),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20, ParameterDirection.Output, DBService.ParameterType.Number)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_INVOICE_VND_ASSET_TYPE_VALIDATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetVendorContactList(int? vncPK, int active, int vendorPK, int bizUnit)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VNC_PK, vncPK > 0 ? vncPK : (object)DBNull.Value),
                new DBService.Parameters(Common.P_ACTIVE, active > 0 ? active : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_PK, vendorPK > 0 ? vendorPK : (object)DBNull.Value),
                new DBService.Parameters(Common.P_BIZUNIT, bizUnit > 0 ? bizUnit : (object)DBNull.Value)

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPPUR_VENDOR_CONTACT_GET_KV, colParameters).Tables[0];
        }
        /// <summary>
        /// Customer supply related Tax for Misc Invoice
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetCustomerSupplyTax(int active, int bizUnit, DateTime date, int? cusPK = null, int? itemPK = null, int? isSale = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.BIZUNIT, bizUnit),
                //new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXPK, taxPK == 0 ? (object) DBNull.Value : taxPK ),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CMT_PK,DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CUS_PK, cusPK == 0 ? (object) DBNull.Value : cusPK),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CIM_PK, itemPK == 0 ? (object) DBNull.Value : itemPK),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_TAX_DATE, date )

            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETCUSTOMERSUPPLYTAX, colParameters).Tables[0];
            return dtParams;
        }



        /// <summary>
        /// Company Details
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>

        public static DataTable GetCompanyDetails(int ActStatus, int bizUnit, int? CompPK = 0, string splCond = null, int? BudgetPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COMPANY_PK,CompPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, ActStatus),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit == 0 ? (object) DBNull.Value : bizUnit),
               // new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BGH_PK, BudgetPk == 0 ? (object) DBNull.Value : BudgetPk),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_CMP_SPL_COND, !string.IsNullOrEmpty(splCond)?splCond:(object)DBNull.Value)
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_COMPANY_MST_GET_KV, colParameters).Tables[0];
            return dtParams;
        }

        /// <summary>
        /// Company Details Plant Wise
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>

        public static DataTable GetCompanyDetailsPlantWise(int ActStatus, int bizUnit, int? CompPK = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COMPANY_PK,CompPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, ActStatus),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit == 0 ? (object) DBNull.Value : bizUnit) ,
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_CMP_SPL_COND, "PRD"),  
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, bizUnit),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, ActStatus)                              
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_COMPANY_MST_GET_KV, colParameters).Tables[0];
            return dtParams;
        }
        public static DataTable GetCurrencyConfiguration(string configText, string configData)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters("P_ACF_SETTING", configText),
               new DBService.Parameters("P_ACF_DATA", configData == string.Empty ? (object)DBNull.Value : configData)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPADM_APP_CONFIG_MST_GET", colParameters).Tables[0];
        }

        public static bool CheckforPOCancellation(int CurrPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_POH_PK, CurrPK > 0 ? CurrPK : (object)DBNull.Value),
            };
            DataSet dsPO = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPPUR_ORDER_CANCEL_CHECK, colParameters);
            return dsPO == null || dsPO.Tables.Count == 0 || dsPO.Tables[0].Rows.Count == 0;
        }
        /// <summary>
        /// Get Version Details
        /// </summary>
        /// <returns></returns>
        public static DataTable GetVersionDetails()
        {
            DataTable dtVersion;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.PSYSPK,DBNull.Value)
            };
            dtVersion = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_SYSTEM_CFG_GET, colParameters).Tables[0];
            return dtVersion;
        }

        public static DataTable GetPageServer(string pageUrl)
        {
            DataTable dtVersion;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_PAGE,pageUrl)
            };
            dtVersion = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_PAGE_MENU_CFG_GET, colParameters).Tables[0];
            return dtVersion;
        }

        public static string IsInventoryLocked(DateTime Date, int BizUnit, int module, ref string LockUptoDate)
        {
            DataTable dtVoucherLockList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_DATE, Date),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT, BizUnit),
                 new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_FLL_MODULE, module),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dtVoucherLockList = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_LOCK_CHECK, colParameters).Tables[0];
            if (dtVoucherLockList != null && dtVoucherLockList.Rows.Count > 0)
                LockUptoDate = dtVoucherLockList.Rows[0]["FIN_LOCK_UPTO"].ToString();
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            //if (result > 0)
            //    return true;
            //else
            //    return false;
            return result.ToString();
        }

        /// <summary>
        /// Method to get products.
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetProducts(string SearchKey, int bizUnit, byte? ItemGrade = null, string BinSubType = null, int? itemPK = null, int Active = 1)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRO_ACTIVE , Active) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT , bizUnit) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRO_NAME , SearchKey ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_GRADE , ItemGrade),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_SUB_TYPE, (string.IsNullOrEmpty(BinSubType) || BinSubType =="-1") ? (object)DBNull.Value : BinSubType),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ATTR_CFG_ITM_PK , itemPK > 0 ? itemPK :(object)DBNull.Value)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Common.SPPRD_PRODUCT_MASTER_GET_KV, colParameters);
        }
        /// <summary>
        /// Get Brand Products
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="searchVal"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetBrandProducts(string searchBy, string searchKey, int bizUnit, int active = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE , active) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT , bizUnit) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY , searchBy ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_SET , 9),//for brand products
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE , searchKey)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Common.SPINV_ITEM_MST_GET_AUTO, colParameters);
        }

        /// <summary>
        ///  For get application base parameters
        /// </summary>
        /// <param name="applicationPK"></param>
        /// <param name="applicationModule"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetAppParameters(int applicationPK, int applicationModule, int active, int bizUnit, int IsSlab)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_APC_PK , applicationPK) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE , active) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT , bizUnit) ,
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IS_SLAB , IsSlab) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_APC_MODULE , applicationModule>0 ? applicationModule: (object)DBNull.Value),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Common.SPPADM_APP_PARAM_CFG_GET_KV, colParameters);
        }


        /// <summary>
        /// Get Slab (PayItem User Control)
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DataTable GetSlab(int CurrPK, int active, int bizUnit, string Pay_Element)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PHS_PK, CurrPK) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PHS_PAY_ELEMENT, Pay_Element==string.Empty ?(object)DBNull.Value:Pay_Element)
            };

            return dbService.DataAdapterTable(CommandType.StoredProcedure, Common.SPHRM_PAY_ELEMENT_SLAB_HDR_GET_KV, colParameters);
        }

        public static DataTable GetAllGloveItems(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_ACTIVE , 1) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT , bizUnit) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.V_IS_ITEM_LINKED , 1 )
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Common.SPINV_ITEM_MST_GET_KV, colParameters);
        }

        public static DataTable GetCurrency(int CurrencyPk, int bizUnit, int Status)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.CUR_PK, CurrencyPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.CUR_ACTIVE, Status),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_GETCURRENCY, colParameters).Tables[0];
        }

        public static DataTable GetDepartment(int bIZUNIT, int? deptPk, int deptType, int Active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT ,  bIZUNIT),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DPT_ACTIVE ,  Active),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DPT_PK  ,  (deptPk.HasValue && deptPk > 0) ? deptPk : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DPT_TYPE  , deptType > 0 ? deptType : (object)DBNull.Value)
             };
            DataTable dtDep = new DataTable();
            dtDep = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_DEPT_MST_INV_GET, colParameters).Tables[0];
            return dtDep;
        }
        public static DataTable GetAssetTypesAuto(string searchval, int atpPK, int Active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_atpName, searchval),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_atpPK, atpPK>0?atpPK:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPASR_AsrAssetTypeMst_GET_KV, colParameters).Tables[0];
        }
        public static DataTable GetAssetAuto(string searchval, int asrType, int asrPK, int Active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_asrName, searchval),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_asrType, asrType>0?asrType:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_asrPK, asrPK>0?asrPK:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPASR_AsrAssetMst_GET_KV, colParameters).Tables[0];
        }
        public static DataTable GetAssetItemsAuto(string searchval, int asrType, int PK, int Active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ATM_NAME, searchval),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ATM_PK, PK>0?PK:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_asrType, asrType>0?asrType:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPASR_ITEM_MST_GET_KV, colParameters).Tables[0];
        }

        public static int? SaveTransactionComments(TrxCommentBO objTrxComment)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACM_PK, objTrxComment.ACM_PK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACM_APP_TYPE, objTrxComment.ACM_APP_TYPE),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACM_APP_TRX_PK, objTrxComment.ACM_APP_TRX_PK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACM_APP_TRX_CODE, string.IsNullOrEmpty(objTrxComment.ACM_APP_TRX_CODE)? (object)DBNull.Value : objTrxComment.ACM_APP_TRX_CODE),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACM_DATE, objTrxComment.ACM_DATE.HasValue? objTrxComment.ACM_DATE : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACM_COMMENT, objTrxComment.ACM_COMMENT),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, objTrxComment.USER_PK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objTrxComment.BIZUNIT),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, objTrxComment.LAST_MOD_DT),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Common.SPADM_APP_TRX_COMMENT_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetTransactionComments(TrxCommentBO objTrxCmnt)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACM_PK, (objTrxCmnt.ACM_PK == 0)? (object)DBNull.Value : objTrxCmnt.ACM_PK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACM_APP_TYPE, objTrxCmnt.ACM_APP_TYPE),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACM_APP_TRX_PK, (objTrxCmnt.ACM_APP_TRX_PK == 0)? (object)DBNull.Value : objTrxCmnt.ACM_APP_TRX_PK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USR_PK, (objTrxCmnt.USER_PK == 0)? (object)DBNull.Value : objTrxCmnt.USER_PK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objTrxCmnt.BIZUNIT)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_APP_TRX_COMMENT_GET_KV, colParameters).Tables[0];
        }

        public static int? DeleteTransactionComments(int CurrCmntPK, DateTime CmntLastModifiedTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACM_PK, CurrCmntPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, CmntLastModifiedTime),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Common.SPADM_APP_TRX_COMMENT_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetItemCategorySale(string searchValue, int itmCategpk, int sale)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_ITC_NAME, searchValue),
                new DBService.Parameters( Common.ITC_PK,itmCategpk > 0 ? itmCategpk : (object)DBNull.Value),
                //new DBService.Parameters( Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters( Common.P_ITC_IS_SALE, sale),
                new DBService.Parameters( Common.ITC_ACTIVE,CommonConstants.SELECT_VALUE_ONE)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPINV_ITEM_CATEGORY_GET_KV, colParameters).Tables[0];
            return dtProcess;
        }

        public static DataTable GetItemSale(string searchValue, int Itempk, int bizUnit, int sale, int category, int PackSpec = 0, int SubType = 0)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.ITM_NAME, searchValue),  
               // new DBService.Parameters(Common.P_FLD_NAME,fieldName),
                new DBService.Parameters(Common.ITM_PK, Itempk > 0 ? Itempk : (object)DBNull.Value),
                new DBService.Parameters( Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters( Common.P_ITC_IS_SALE, sale),
                new DBService.Parameters (Common.ITM_CATEGORY, category > 0 ? category : (object)DBNull.Value),
                new DBService.Parameters( Common.ITM_ACTIVE,CommonConstants.SELECT_VALUE_ONE),
                new DBService.Parameters (Common.P_PACK_SPEC, PackSpec > 0 ? PackSpec : (object)DBNull.Value),
                new DBService.Parameters(Common.P_ITM_SUB_TYPE,SubType > 0 ? SubType : (object)DBNull.Value)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPINV_ITEM_MST_GET_KV, colParameters).Tables[0];
            return dtProcess;
        }
        public static DataTable GetUsersAuto(string searchval, string fieldName, int? isSysUser = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters( GTIService.Constants.Configurations.Users.Parameters.FIELDNAME ,  fieldName),
              new DBService.Parameters( GTIService.Constants.Configurations.Users.Parameters.VALUE ,  searchval),
              new DBService.Parameters( GTIService.Constants.Configurations.Users.Parameters.USERTYPE ,  isSysUser.HasValue ?isSysUser: (object)DBNull.Value )
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SpWkfUserMstAuto, colParameters).Tables[0];
        }

        public static decimal GetUOMConversionFactor(int itemPk, int FromUOMPK, int ToUOMPK)
        {
            decimal ConversionFactor = 0;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_PK ,  itemPk),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_UOM_FROM ,  FromUOMPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_UOM_TO ,  ToUOMPK),
              new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_RET_VAL , 0, 20,ParameterDirection.ReturnValue, DBService.ParameterType.Number)
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Common.FNINV_ITEM_UOM_CONV_FACTOR, colParameters);
            string retValue = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value.ToString();
            ConversionFactor = string.IsNullOrEmpty(retValue) ? -1 : Convert.ToDecimal(retValue);
            return ConversionFactor;
        }

        public static DataTable GetInitialTaskAction(int ProcessID, int ReqDeptID, int userPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.Pprocess ,  ProcessID),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.PConstPK,  ReqDeptID),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.PUSERPK,  userPk)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Common.SpWkfTransactionStatusInitGet, colParameters);
        }

        /// <summary>
        /// Method to get all cost centers mapped to this account
        /// </summary>
        /// <param name="AccountPk"></param>
        /// <returns></returns>
        public static DataTable GetCostCenter(int AccountPk, decimal Amount, int RefPk = 0, int JurCurrPK = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_COA_PK ,  AccountPk),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_AMOUNTS ,  Amount),
             // new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_COC_MAP ,  ccMappingRequired)
             new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_REF_PK ,  RefPk > 0 ? RefPk : (object)DBNull.Value),
             new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_FTH_PK ,  JurCurrPK > 0 ? JurCurrPK : (object)DBNull.Value)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Common.SPFIN_COA_COST_CENTER_MPG_GET, colParameters);
        }

        public static DataTable GetAllCostCenter(long VoucherPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_FTH_PK ,  VoucherPk)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Common.SPFIN_TRX_COC_DTL_GET, colParameters);
        }

        /// <summary>
        /// Save Summary 
        /// </summary>
        /// <param name="pK"></param>
        /// <param name="processPK"></param>
        /// <returns></returns>
        public static int SaveSummary(long pK, int processPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PK,  pK ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PROCESS,  processPK ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Common.SPADM_SUMMARY_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get Purchase Invoice Company mapping Details
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>

        public static DataTable GetDepartmentsWithUserPermission(int active, int bizUnit, int userPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit == 0 ? (object) DBNull.Value : bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USR_PK, userPk == 0 ? (object) DBNull.Value : userPk),
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPPUR_ORDER_DEPT_MPG_GET, colParameters).Tables[0];
            return dtParams;
        }

        /// <summary>
        /// Get Module Filter
        /// </summary>
        /// <param name="modPk"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetModule(int modPk, int DashletActive, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_MOD_PK,modPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DASHLET_EXIST, DashletActive),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),

            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_MODULE_MST_GET_KV, colParameters).Tables[0];
            return dtParams;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configText"></param>
        /// <param name="spclCond"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetChangeHistory(int transPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtChangeHistory = new DataTable();
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_TRX_PK, transPk),
            };
            dtChangeHistory = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPRATE_CHANGE_HISTORY_GET, colParameters).Tables[0];
            return dtChangeHistory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configText"></param>
        /// <param name="spclCond"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>

        public static DataTable GetAutoUserRoleInboxMapping(string searchValue, string type, int? modPK, int? userLogin, int? EmpDept, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtuser = new DataTable();
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_GUM_MODULE, modPK > 0 ? modPK : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LOGIN_USER, userLogin > 0 ? userLogin : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters.DEPTPK, EmpDept.HasValue ? EmpDept : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY,type),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchValue),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, bizUnit),
            };
            dtuser = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_USER_GROUP_GET_AUTO, colParameters).Tables[0];
            return dtuser;
        }

        public static DataTable GetCoaParent(string searchKey, string searchField, int active, int bizUnit, int isGroup, int AccountPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtData = new DataTable();
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_NAME, searchField),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchKey),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, bizUnit),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COA_PK, AccountPk>0?AccountPk:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COA_IS_GROUP, isGroup < 0 ?  (object)DBNull.Value: isGroup),
            };
            dtData = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_COA_MST_AUTO, colParameters).Tables[0];
            return dtData;
        }

        public static DataTable GetBudgetCoaParent(string searchKey, string searchField, int active, int bizUnit, int isGroup, int AccountPk, int Budget)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtData = new DataTable();
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_NAME, searchField),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchKey),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, bizUnit),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COA_PK, AccountPk>0?AccountPk:(object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COA_IS_GROUP, isGroup < 0 ?  (object)DBNull.Value: isGroup),
               //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BGH_PK,Budget>0?Budget:(object)DBNull.Value),
            };
            dtData = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_COA_MST_AUTO, colParameters).Tables[0];
            return dtData;
        }
        public static DataTable GetMappedItemvendor(string searchKey, string searchField, int bizUnit, string categoryPk, string VenPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtData = new DataTable();
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_NAME, searchField),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchKey),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, bizUnit),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_CATEGORY, string.IsNullOrEmpty(categoryPk) ? (object)DBNull.Value:categoryPk),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_VEN_PK, string.IsNullOrEmpty(VenPk) ? (object)DBNull.Value:VenPk),
            };
            dtData = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPPUR_VENDOR_ITEM_MAP_AUTO, colParameters).Tables[0];
            return dtData;
        }

        public static DataTable GetInvoiceGstType(int? FtmPk, int Active, int bizUnit, int invType, int invGstType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_FTM_PK, FtmPk),
                new DBService.Parameters(Common.P_ACTIVE, Active==0?(object)DBNull.Value:Active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(Common.P_INVOICE_TYPE,  invType < 0 ?  (object)DBNull.Value: invType),
                new DBService.Parameters(Common.P_FTM_TRX_TYPE, invGstType)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_INVOICE_GST_TYPE_MST_GET_KV, colParameters).Tables[0];
        }

        public static DataTable GetPortDetails(string searchValue, int PrmPK, byte Active, int bizUnit, int SIType, int SaleFromPort, int SaleToPort, int PurFromPort, int PurToPort)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_PRM_PK, PrmPK),
                new DBService.Parameters(Common.P_ACTIVE, Active==0?(object)DBNull.Value:Active),
                new DBService.Parameters(Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Common.PRM_TYPE,  SIType <= 0 ?  (object)DBNull.Value: SIType),
                new DBService.Parameters(Common.PRM_IS_SALES_FROM, SaleFromPort > 0 ? SaleFromPort : (object)DBNull.Value),
                new DBService.Parameters(Common.PRM_IS_SALES_TO, SaleToPort > 0 ? SaleToPort : (object)DBNull.Value),
                new DBService.Parameters(Common.PRM_IS_PUR_FROM, PurFromPort > 0 ? PurFromPort : (object)DBNull.Value),
                new DBService.Parameters(Common.PRM_IS_PUR_TO, PurToPort > 0 ? PurToPort : (object)DBNull.Value),
                new DBService.Parameters(Common.PRM_NAME, searchValue)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_PORT_MST_GET_KV, colParameters).Tables[0];
        }

        public static DataTable GetPortDetailsByPK(int prmPK, int status, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_PRM_PK, prmPK),
                new DBService.Parameters(Common.P_ACTIVE, status==0?(object)DBNull.Value:status),
                new DBService.Parameters(Common.P_BIZUNIT, bizUnit),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_PORT_MST_GET_KV, colParameters).Tables[0];
        }

        public static DataTable GetHSCodeList(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Common.P_BIZUNIT, bizUnit),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_GST_CLASS_MST_GET, colParameters).Tables[0];
        }
        public static bool IsFinancialYearExist(DateTime Date, int bizUnit)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_DATE, Date),
                new DBService.Parameters(Common.P_BIZUNIT, bizUnit)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_YEAR_GET, colParameters);
            return dsArchive == null || dsArchive.Tables.Count == 0 || dsArchive.Tables[0].Rows.Count == 0;
        }


        public static DataTable GetBrandPackSpec(string searchValue, int Active, int BizUnit)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_ACTIVE, Active),
                new DBService.Parameters( Common.P_BIZUNIT, BizUnit),
                new DBService.Parameters( Common.P_APS_NAME, searchValue)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_PACK_SPEC_MST_GET_AUTO, colParameters).Tables[0];
            return dtProcess;
        }


        public static DataTable GetPackingSpecCategory(int sbuPk, string srhcType, int type = 0)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.SERACHVALUE, srhcType),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_ITC_VALUE, type>0?type:(object)DBNull.Value)
                //new DBService.Parameters( Common.ITC_NAME, searchValue)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPINV_ITEM_CATEGORY_AUTO, colParameters).Tables[0];
            return dtProcess;
        }



        public static DataTable GetPackingSpecType(string searchValue, int Con_PK, int Active, int Cgt_Value, int BizUnit)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.P_CON_ACTIVE, Active),
                new DBService.Parameters( Common.P_CON_BIZUNIT, BizUnit),
                new DBService.Parameters( Common.P_CON_PK, Con_PK),
                new DBService.Parameters( Common.P_CGT_VALUE, Cgt_Value),
                //new DBService.Parameters( Common.P_CNG_VALUE, Cng_Value),
                new DBService.Parameters( Common.P_CON_NAME, searchValue)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_SPADM_CONST_MST_GET_KV, colParameters).Tables[0];
            return dtProcess;
        }


        public static DataTable GetPackingSpec(string searchValue, int Con_Pk, int Itc_Pk, int Cus_Pk, int IsProductRequired)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( Common.CUS_PK, Cus_Pk),
                new DBService.Parameters( Common.ITC_PK, Itc_Pk>0?Itc_Pk:(object)DBNull.Value),
                new DBService.Parameters( Common.CON_PK, Con_Pk>0?Con_Pk:(object)DBNull.Value),
                new DBService.Parameters( Common.ITM_NAME, string.IsNullOrEmpty(searchValue) ? (object)DBNull.Value:searchValue),
                new DBService.Parameters( Common.IS_PRODUCT, IsProductRequired>0?IsProductRequired:(object)DBNull.Value)

            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_PACK_MATERIAL_GET, colParameters).Tables[0];
            return dtProcess;
        }





        /// <summary>
        /// Get Fromula List
        /// </summary>
        public static DataTable GetFormulaList(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FRL_TYPE,1),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_FORMULA_MST_GET_KV, colParameters).Tables[0];
        }

        /// <summary>
        /// Delete Fromula Details
        /// </summary>
        public static int DeleteFormulaDetails(int CurrPk)
        {
            int result = 0;
            //DBService dbService = new DBService();
            //DBService.Parameters[] colParameters = null;
            //colParameters = new DBService.Parameters[] 
            //{  
            //    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_PK,CurrPK), 
            //    //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LAST_MOD_DT ? SHF_PK :(object)DBNull.Value), 
            //    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LAST_MOD_DT , (object)DBNull.Value), 
            //    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            //};
            //int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_SHIFT_MST_DELETE, colParameters);
            //int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Save Fromula Details
        /// </summary>
        public static int SaveFormulaDetails()
        {
            int result = 0;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {  
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_PK,objShiftHeader.SHF_PK>0  ? objShiftHeader.SHF_PK :(object)DBNull.Value ), 
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_CODE,objShiftHeader.SHF_CODE),  
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_NAME,objShiftHeader.SHF_NAME)         
            };
            //int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_SHIFT_MST_SAVE, colParameters);
            //int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetPackSepecDetails(int PackSepcPK, int IsProductRequired)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.ITM_PK, PackSepcPK>=0?PackSepcPK:(object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.IS_PRODUCT, IsProductRequired>=0?IsProductRequired:(object)DBNull.Value),
               //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, Active>0?Active:(object)DBNull.Value),
               //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CGT_VALUE,groupType.HasValue?(int)groupType.Value:(object)DBNull.Value),
               //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CNG_VALUE,groupValue>0?groupValue:(object)DBNull.Value),
               //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_ACTIVE, active),
               //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_PACK_MATERIAL_GET, colParameters).Tables[0];
        }

        public static DataTable GetReport(int userID, int Group, int bizUnit, int Active, string searchValue)
        {
            DataTable dtReoprts = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchValue),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, Active > 0 ? Active : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER_PK, userID > 0 ? userID : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RPT_GROUP, Group > 0 ? Group : (object)DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            dtReoprts = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_REPORT_CFG_AUTO, colParameters).Tables[0];
            return dtReoprts;
        }

        public static string GetAutoBinCardsForIssue(int BinDept, int bizunit, string BinNo, int pageNumber = 0, int pageSize = 0, int IsQaPassed = 0, int department = 0, int IsAutocomplete = 0, int ItemPK = 0)
        {
            DataTable dtBinCardList;
            DBService dbService = new DBService();
            string strRetVal = "";
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_DEPT , BinDept >0 ? BinDept : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_BIZUNIT, bizunit),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_NO,  string.IsNullOrEmpty(BinNo) || IsAutocomplete == 1 ? (object)DBNull.Value:BinNo),//Exact search value
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_NO_SRCH,  string.IsNullOrEmpty(BinNo) || IsAutocomplete == 0 ? (object)DBNull.Value:BinNo),//Autocomplete search
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM, pageNumber > 0 ? pageNumber : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, pageSize > 0 ? pageSize : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IS_QA_PASSED, IsQaPassed),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_MNU_DEPT, department >0 ? department : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_PRODUCT, ItemPK >0 ? ItemPK : (object)DBNull.Value)
            };
            dtBinCardList = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_SPPRD_BIN_CARD_ISSUE_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtBinCardList.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static string GetBinCardsForIssue(int BinDept, int bizunit, string BinPk, int? product = null, int? line = null, int? BinType = null, int? Aql = null, string date = null, int? shift = null, int Itmgrade = 0, int? trxpk = 0, string BinNo = null, int pageNumber = 0, int pageSize = 0, int IsQaPassed = 0, string IssueTrxDate = null, string ExcludePks = null)
        {
            DataTable dtBinCardList;
            DBService dbService = new DBService();
            string strRetVal = "";
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_DEPT , BinDept),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_PRODUCT , product==0?(object)DBNull.Value:product),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_LINE, line > 0? line : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_DATE, string.IsNullOrEmpty(date)?(object)DBNull.Value:date),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_SHIFT, shift > 0 ? shift : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_BIZUNIT, bizunit),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIH_PK,  trxpk > 0 ?trxpk:(object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_PK,  string.IsNullOrEmpty(BinPk) ? (object)DBNull.Value:BinPk),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_NO,  string.IsNullOrEmpty(BinNo) ? (object)DBNull.Value:BinNo),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM, pageNumber > 0 ? pageNumber : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, pageSize > 0 ? pageSize :  (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_GRADE, Itmgrade > 0 ? Itmgrade :  (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IS_QA_PASSED, IsQaPassed),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_TYPE, BinType > 0 ?BinType : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_AQL_LAST, Aql > 0? Aql : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, string.IsNullOrEmpty(ExcludePks) ? (object)DBNull.Value:ExcludePks)
            };
            dtBinCardList = dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_SPPRD_BIN_CARD_ISSUE_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtBinCardList.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        /// <summary>
        /// Get Category
        /// </summary>
        /// <param name="GroupPK"></param>
        /// <returns></returns>
        public static DataTable GetCategoryByGroup(int? GroupType, int? Group, int? pk = null, int? active = null, int? parent = null, int? conGroup = null)
        {
            DataTable dtCategory = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CGT_VALUE, GroupType.HasValue? GroupType : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CNG_VALUE, Group.HasValue? Group : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK, pk.HasValue? pk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_ACTIVE, active.HasValue? active : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PARENT,parent.HasValue ? parent :(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_GROUP,conGroup.HasValue ? conGroup :(object)DBNull.Value),
            };
            dtCategory = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_CONST_MST_GET_KV, colParameters).Tables[0];
            return dtCategory;
        }
        public static DataSet GetUsedCustomer(int pk, string name, int bizUnit, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CUS_NAME, name == string.Empty ? "%" : name + "%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CUS_PK, pk),
            };

            DataSet dsCustomers = new DataSet();
            dsCustomers = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPINV_WO_CUSTOMER_GET_KV, colParameters);
            return dsCustomers;
        }
        public static DataSet GetCustomer(int pk, string name, int bizUnit, int active, int? IsConsultant = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CUS_NAME, name == string.Empty ? "%" : name + "%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CUS_PK, pk),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IS_CONSULTANT, IsConsultant.HasValue ? IsConsultant : (object)DBNull.Value)
            };

            DataSet dsCustomers = new DataSet();
            dsCustomers = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPCRM_CUSTOMER_MST_GET_KV, colParameters);
            return dsCustomers;
        }


        public static DataSet GetHSNNO(int pk, string name, int bizUnit, int active, int? IsConsultant = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_VALUE, name == string.Empty ? "%" : name + "%")

            };

            DataSet dsCustomers = new DataSet();
            dsCustomers = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_GST_CLASS_MST_AUTO, colParameters);
            return dsCustomers;
        }

        public static DataSet GetAssetTypeDetails(GridPrams paramObj, int atpPK, int BizUnit, int DeptPK, int Active)
        {
            DataSet dsAssetBasicDetails;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME, paramObj.SearchBy == string.Empty ? (Object)DBNull.Value : paramObj.SearchBy ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL, paramObj.SearchValue == string.Empty ? (Object)DBNull.Value : paramObj.SearchValue),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO, paramObj.PageNumber),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY, paramObj.SortBy == string.Empty ? (Object)DBNull.Value : paramObj.SortBy),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, paramObj.SortDirection == string.Empty ? (Object)DBNull.Value : paramObj.SortDirection),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_atpPK, atpPK <=0?(object)DBNull.Value:atpPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_atpBizUnit, BizUnit <=0?(object)DBNull.Value:BizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_atpDept, DeptPK <=0?(object)DBNull.Value:DeptPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_atpActive, Active <=0?(object)DBNull.Value:Active)
            };
            dsAssetBasicDetails = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPASSET_AsrAssetTypeMst_GET_KV, colParameters);
            return dsAssetBasicDetails;
        }
        public static DataTable GetProductGroups(string searchval)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE,searchval)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SP_INV_ITEM_GROUP_MST_AUTO, colParameters).Tables[0];
        }
        public static DataTable GetCompanyDetails(int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT,bizunit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_COMPANY_MST_FILTER, colParameters).Tables[0];
        }
        public static DataTable GetStatus()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPADM_ACTIVE_FILTER, colParameters).Tables[0];
        }
        public static DataTable GetCompound(string searchKey, int Status, int bizunit, int CompanyPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
            new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT,bizunit),
            new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,Status),
            new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CMP_PK,CompanyPK>0?CompanyPK:(object)DBNull.Value),
            new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_VALUE,searchKey)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPPRD_COMP_MST_FILTER, colParameters).Tables[0];
        }
        public static DataTable GetDispersion(string searchKey, int Status, int bizunit, int CompanyPK, string Type)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
            new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT,bizunit),
            new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,Status),
            new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CMP_PK,CompanyPK>0?CompanyPK:(object)DBNull.Value),
            new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_VALUE,searchKey),
            new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DSP_TYPE,Type)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPPRD_DISP_MST_FILTER, colParameters).Tables[0];
        }
        public static DataTable GetDispersionTypes()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPPRD_DISP_TYPE_FILTER, colParameters).Tables[0];
        }
        public static DataTable GetAuditLogDisplayStatus(int CurrPk, string VoucherType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FTH_PK,CurrPk),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FTH_REF_TYPE,VoucherType== string.Empty ? (Object)DBNull.Value :VoucherType)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_TRX_AUDIT_BIT_GET, colParameters).Tables[0];
        }
        public static DataTable GetAuditLogDetails(int CurrPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FTH_PK,CurrPk),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_TRX_AUDIT_LIST, colParameters).Tables[0];
        }
        public static string GetAuditLogComparision(int CurrPk, int Version)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FTH_PK,CurrPk),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FTH_AUDIT_VERSION,Version)
            };
            //return dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_TRX_AUDIT_GET, colParameters);
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, Common.SPFIN_TRX_AUDIT_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
    }
}
