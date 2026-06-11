using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.CommonManagement;
using System.Data;
using DataAccess.CommonManagement;
using GTIService.Constants.Common;
using BusinessObject;
using DataAccess.AccountManagement;
using ERP.Utilities;
using BusinessObject.Journalize;

namespace BusinessLogic.CommonManagement
{
    public class CommonBL
    {
        /// <summary>
        /// method for Get BizUnit and Department
        /// </summary>
        /// <param name="userID" Type=int></param>
        /// <returns>DataTable</returns>
        public static DataTable GetBizUnit(int userID, int sbuID)
        {
            return CommonDA.GetBizUnit(userID, sbuID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataSet ExecuteSP(string spName, string pXML, bool IsReportServer = false)
        {
            return CommonDA.ExecuteSP(spName, pXML, IsReportServer);
        }
        public static void ExceptionWriting(string exception, string remarks)
        {
            CommonDA.ExceptionWriting(exception, remarks);
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
            return CommonDA.ExecuteNonQuerySP(spName, pXML);
        }


        /// <summary>
        /// For Get Application base Parameters
        /// </summary>
        /// <param name="applicationModule"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetAppParameters(int applicationPK, int applicationModule, int active, int bizUnit, int IsSlab)
        {
            return CommonDA.GetAppParameters(applicationPK, applicationModule, active, bizUnit, IsSlab);
        }


        public static DataTable GetReportDetails(string RptType, int RptSubType, DateTime AppvdDt)
        {
            return CommonDA.GetReportDetails(RptType, RptSubType, AppvdDt);
        }

        public static DataTable GetQueryCFGValues(int queryPK)
        {
            return CommonDA.GetQueryCFGValues(queryPK);

        }

        /// <summary>
        /// Get Department Details
        /// </summary>
        /// <param name="userDept"></param>
        /// <returns></returns>
        public static DataTable GetDepartmentDetails(int userDept)
        {
            return CommonDA.GetDepartmentDetails(userDept);
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
            return CommonDA.GetDepartmentPageRight(userPK, pageURL, deptPK, active);
        }

        public static DataTable GetDepartmentList(int? deptPk = null, int? active = null, int? parentDept = null, int? deptType = null, int? deptCategory = null)
        {
            return CommonDA.GetDepartmentList(deptPk, active, parentDept, deptType, deptCategory);
        }
        /// <summary>
        /// Get Mail CFG
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMailCFGDA()
        {
            return CommonDA.GetMailCFGDA();
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
            return CommonDA.GetAccountType(accountPk, subType, active);
        }

        /// <summary>
        /// Get Account Type
        /// </summary>
        /// <param name="accountPk"></param>
        /// <param name="subType"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetAccountTypesforMapping(int bizunit, string subType, string splCond)
        {
            return CommonDA.GetAppConfig(bizunit, subType, splCond);
        }
        /// <summary>
        /// Get Application Tpe
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subType"></param>
        /// <returns></returns>
        public static DataTable GetAppStatus(string type, string subType)
        {
            return CommonDA.GetAppStatus(type, subType);
        }
        /// <summary>
        /// Get Account Type
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static DataTable GetAccountType(string xml)
        {
            return CommonDA.GetAccountType(xml);
        }
        /// <summary>
        /// Get Plant
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static DataTable GetPlant(string xml)
        {
            return CommonDA.GetPlant(xml);
        }
        /// <summary>
        /// Get WorkFlow Inbox details for send mail
        /// </summary>
        /// <param name="RefID"></param>
        /// <returns></returns>
        public static DataTable GetInboxMail(int RefID)
        {
            return CommonDA.GetInboxMail(RefID);
        }

        /// <summary>
        /// method for Get BizUnit and Department
        /// </summary>
        /// <param name="userID" Type=int></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDepartment(int userID, int sbuID, string searchkey = null)
        {
            return CommonDA.GetDepartment(userID, sbuID, searchkey);
        }

        /// <summary>
        /// method for Get BizUnit and Department
        /// </summary>
        /// <param name="userID" Type=int></param>
        /// <returns>DataTable</returns>
        public static DataTable GetMenuDepartment(int userID, int sbuID, string searchkey = null)
        {
            return CommonDA.GetMenuDepartment(userID, sbuID, searchkey);
        }


        /// <summary>
        /// method for Get BizUnit and Department
        /// </summary>
        /// <param name="userID" Type=int></param>
        /// <returns>DataTable</returns>
        public static DataTable GETAGENTLIST(int sbuID)
        {
            return CommonDA.GETAGENTLIST(sbuID);
        }

        /// <summary>
        /// method for Get BizUnit and Department
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="sbuID"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public static DataTable GetDepartment(int userID, int sbuID, int module)
        {
            return CommonDA.GetDepartment(userID, sbuID, module);
        }

        /// <summary>
        /// Get a particular department by department type 
        /// </summary>
        /// <Created By></Created>
        /// <For>Dept Filling</For>
        /// <Used In>DepFilling,FillSelectedDepartDetails </Used>
        /// <param name="userID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDepartment(int bIZUNIT, int? deptPk, int deptType, int Active)
        {
            return CommonDA.GetDepartment(bIZUNIT, deptPk, deptType, Active);
        }
        /// <summary>
        ///  Method for Get  Department URL
        /// </summary>
        /// <param name="userID" Type=int></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDepartmentUrl(int deptPk)
        {
            return CommonDA.GetDepartmentUrl(deptPk);
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
            return CommonDA.GetExchangeRate(fromCurrency, toCurrency, date);
        }
        public static string GetExchangeRate(int fromCurrency, int toCurrency, string date)
        {
            DataTable dtRate = CommonDA.GetExchangeRate(fromCurrency, toCurrency, date);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dtRate);
        }

        public static DataTable GetExchangeRateTable(int fromCurrency, int toCurrency, string date)
        {
            DataTable dtRate = CommonDA.GetExchangeRate(fromCurrency, toCurrency, date);
            return dtRate;
        }
        public static DataTable GetCurrencyHold(int BankPK)
        {
            DataTable dtRate = CommonDA.GetCurrencyHold(BankPK);
            return dtRate;
        }
        /// <summary>
        /// Methord used to get process orresponding to user pk and Department
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="depID"></param>
        /// <returns></returns>
        public static DataTable GetProcessList(int userID, int depID, string searchkey = null, int sbu = 0)
        {
            return CommonDA.GetProcessList(userID, depID, searchkey, sbu);
        }

        public static DataTable GetTrxTypeList(int aptPK, int? modPk, byte active, int bizUnit, string splCond)
        {
            return CommonDA.GetTrxTypeList(aptPK, modPk, active, bizUnit, splCond);
        }

        /// <summary>
        /// Methord used to get User groups
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static DataTable GetUsrGroups(int flag)
        {
            return CommonDA.GetUsrGroups(flag);
        }
        /// <summary>
        /// Method used to get Favourites List corresponding to user pk , pageID and sbu
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageID"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetFavouritesList(int userID, int pageID, int sbu, string menuType, string path)
        {
            return CommonDA.GetFavouritesList(userID, pageID, sbu, menuType, path);
        }
        /// <summary>
        /// Method used to add Favourites List .
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageID"></param>
        /// <param name="sbu"></param>
        /// <returns>DataTable</returns>
        public static DataTable AddToFavouritesList(int userID, int pageID, int sbu)
        {
            return CommonDA.AddToFavouritesList(userID, pageID, sbu);
        }
        /// <summary>
        /// Function for getting pagid
        /// </summary>
        /// <param name="pageURL"></param>
        /// <returns>int</returns>
        public static int GetPageId(string pageURL)
        {
            return CommonDA.GetPageId(pageURL);
        }
        /// <summary>
        /// Get page Info
        /// </summary>
        /// <param name="pageURL"></param>
        /// <returns>DataSet</returns>
        public static DataTable GetPageInfo(string pageURL)
        {
            return CommonDA.GetPageInfo(pageURL);
        }
        public static WorkFlowDetails GetWorkflowDetails(int ApplicationID, int ProcessID)
        {
            //object used to store work flow details
            WorkFlowDetails workFlowDetails = null;
            //Filling workflow details using DA function
            DataTable dtWorkflow = CommonDA.GetWorkFlowDetails(ApplicationID, ProcessID);

            if (dtWorkflow != null)
            {
                if (dtWorkflow.Rows.Count > 0)
                {
                    workFlowDetails = new WorkFlowDetails()
                    {
                        ReferenceID = (dtWorkflow.Rows[0][Common.F_WORKFLOW_REF_ID] == DBNull.Value) ? 0 : Convert.ToInt32(dtWorkflow.Rows[0][Common.F_WORKFLOW_REF_ID]),
                        ProcessID = (dtWorkflow.Rows[0][Common.F_WORKFLOW_PROCESS_ID] == DBNull.Value) ? 0 : Convert.ToInt32(dtWorkflow.Rows[0][Common.F_WORKFLOW_PROCESS_ID]),
                        TaskID = (dtWorkflow.Rows[0][Common.F_WORKFLOW_TASK_ID] == DBNull.Value) ? 0 : Convert.ToInt32(dtWorkflow.Rows[0][Common.F_WORKFLOW_TASK_ID]),
                        Action = (dtWorkflow.Rows[0][Common.F_WORKFLOW_ACTION] == DBNull.Value) ? 0 : Convert.ToInt32(dtWorkflow.Rows[0][Common.F_WORKFLOW_ACTION]),
                        IsClosed = (dtWorkflow.Rows[0][Common.F_WORKFLOW_IS_CLOSED] == DBNull.Value) ? 0 : Convert.ToInt32(dtWorkflow.Rows[0][Common.F_WORKFLOW_IS_CLOSED]),
                        Type = (dtWorkflow.Rows[0][Common.F_WORKFLOW_TYPE] == DBNull.Value) ? 0 : Convert.ToInt32(dtWorkflow.Rows[0][Common.F_WORKFLOW_TYPE])
                    };
                }
            }

            return workFlowDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static DataTable GetRelatedItemsWidget(int processID, int appId, string path)
        {
            return CommonDA.GetRelatedItemsWidget(processID, appId, path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static DataTable GetPageDept(int pageId, int userPK)
        {
            return CommonDA.GetPageDept(pageId, userPK);
        }
        /// <summary>
        /// Check Is Edoc User
        /// </summary>
        /// <param name="userPK"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static bool IsEdocUser(int userPK, int sbuID)
        {
            bool retVal = false;
            DataTable dtResult = CommonDA.IsEdocUser(userPK, sbuID);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                retVal = true;
            }
            return retVal;
        }
        /// <summary>
        /// Check Is Dashboard User
        /// </summary>
        /// <param name="userPK"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static bool IsDashBoardUser(int userPK, int sbuID, int IsHome = 0)
        {
            bool retVal = false;
            DataTable dtResult = CommonDA.IsDashBoardUser(userPK, sbuID, IsHome);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                retVal = true;
            }
            return retVal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static DataTable GetApplicaitonConfiguaration(string configText, string spclCond, int bizUnit)
        {
            return CommonDA.GetApplicaitonConfiguaration(configText, spclCond, bizUnit);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configText"></param>
        /// <param name="configData"></param>
        /// <returns></returns>
        public static DataTable GetApplicaitonConfiguaration(string configText, string configData)
        {
            return CommonDA.GetApplicaitonConfiguaration(configText, configData);
        }

        /// <summary>
        /// Currency Configuration
        /// </summary>
        /// <param name="configText"></param>     
        /// <param name="confgData"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetCurrencyConfiguration(string configText, string confgData, int bizUnit)
        {
            return CommonDA.GetCurrencyConfiguration(configText, confgData, bizUnit);
        }
        /// <summary>
        /// Get Transaction ID
        /// </summary>
        /// <param name="pRefPK"></param>
        /// <returns></returns>
        public static DataTable GetTransactionID(int pRefPK)
        {
            return CommonDA.GetTransactionID(pRefPK);
        }

        /// <summary>
        /// Get department Company 
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public static DataTable GetDeptCompany(int deptPK)
        {
            return CommonDA.GetDeptCompany(deptPK);
        }

        public static DataTable GetAppConfig(int bizUnit, string configType, string splCond = "")
        {
            return CommonDA.GetAppConfig(bizUnit, configType, splCond);
        }

        public static DataTable GetLineDetails(DateTime date)
        {
            return CommonDA.GetLineDetails(date);
        }
        public static DataTable GetLineDetailsforOnlineParameter(DateTime date)
        {
            return CommonDA.GetLineDetailsforOnlineParameter(date);
        }

        public static DataTable GetLineProductDetails(DateTime date, string product)
        {
            return CommonDA.GetLineProductDetails(date, product);
        }

        public static DataTable GetLineProductDetailsforOnlineParameter(DateTime date, string product)
        {
            return CommonDA.GetLineProductDetailsforOnlineParameter(date, product);
        }
        public static DataTable GetConstMstList(int conPK, int conActive, int conGroup, int cgtValue, int cngValue, int btzuUit)
        {
            return CommonDA.GetConstMstList(conPK, conActive, conGroup, cgtValue, cngValue, btzuUit);
        }

        public static DataTable GetUOM(int? ItemPk, int Active)
        {
            return CommonDA.GetUOM(Active, ItemPk);
        }
        public static DataTable GetAllUOMList(int Bizunit, int active, int? UomPK, int? UomType, int? UomStatus)
        {
            return CommonDA.GetAllUOMList(Bizunit, active, UomPK, UomType, UomStatus);
        }

        public static DataTable GetAppConfigTree(int? machinePK, int? tankPK, short active = 1)
        {
            return CommonDA.GetAppConfigTree(machinePK, tankPK, active);
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
            return DataAccess.CommonManagement.CommonDA.GetTrxNo(appType, subType, dept, user);
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
            return DataAccess.CommonManagement.CommonDA.GetConstMstValues(constPK, constGroup, groupType, groupValue, active, bizUnit);
        }
        /// <summary>
        /// 
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
            return DataAccess.CommonManagement.CommonDA.GetConstMstValuesAuto(constPK, constGroup, groupType, groupValue, searchValue, active, bizUnit, IsProductRequired);
        }

        /// <summary>
        /// Returns User Rights based on UserPK, and Page
        /// </summary>
        /// <param name="UserPK"></param>
        /// <param name="PageURL">Page path from the root folder</param>
        /// <returns></returns>
        public BusinessObject.UserRightsBO GetUserRights(int UserPK, string PageURL, int bizUnit, int deptPK = 0)
        {
            UserRightsBO userRights;
            AuthDA userAuthServices;
            userAuthServices = new AuthDA();
            userRights = userAuthServices.GetUserRightsInfo(UserPK, PageURL, bizUnit, deptPK);
            return userRights;
        }

        /// <summary>
        /// Get WorkFlow Intimation details for send mail
        /// </summary>
        /// <param name="RefID"></param>
        /// <returns></returns>
        public static DataTable GetIntimationMail(int RefID)
        {
            return CommonDA.GetIntimationMail(RefID);
        }
        /// <summary>
        /// Get Currency List
        /// </summary>
        /// <param name="btzuUit"></param>
        /// <returns></returns>
        public static DataTable GetCurrencyList(int btzuUit)
        {
            return CommonDA.GetCurrencyList(btzuUit);
        }
        /// <summary>
        /// Get Currency List By BtzuUnit
        /// </summary>
        /// <param name="btzuUit"></param>
        /// <returns></returns>
        public static DataTable GetCurrencyListByBtzuUnit(int btzuUit)
        {
            return CommonDA.GetCurrencyListByBtzuUnit(btzuUit);
        }


        /// <summary>
        /// Get Currency List By BtzuUnit
        /// </summary>
        /// <param name="btzuUit"></param>
        /// <returns></returns>
        public static DataTable BrandRateGetCurrencyListByBtzuUnit(int curPK, int active, int btzuUit)
        {
            return CommonDA.BrandRateGetCurrencyListByBtzuUnit(curPK, active, btzuUit);
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
            return CommonDA.GetPageTaskPermission(process, pageUrl, refID, userPK);
        }

        public static int SaveMessageTime(int UserPk, byte? inboxType)
        {
            return CommonDA.SaveMessageTime(UserPk, inboxType);
        }
        /// <summary>
        /// Save Customer As CRM Lead
        /// </summary>
        /// <param name="customerPk"></param>
        /// <param name="pRefID"></param>
        /// <returns></returns>
        public static int SaveAsCRMLead(long customerPk, int pRefID)
        {
            return CommonDA.SaveAsCRMLead(customerPk, pRefID);
        }
        /// <summary>
        /// Save Product as CRM Product
        /// </summary>
        /// <param name="productPk"></param>
        /// <returns></returns>
        public static int SaveAsCRMProduct(long productPk)
        {
            return CommonDA.SaveAsCRMProduct(productPk);
        }
        /// <summary>
        /// Get Workflow Section Action Rights
        /// </summary>
        /// <param name="UserPK"></param>
        /// <param name="PageURL"></param>
        /// <param name="department"></param>
        /// <param name="refID"></param>
        /// <param name="pageType" value="0: if Transaction and 1: if Listing"></param>
        /// <returns></returns>
        public UserRightsBO GetWorkFlowUserRightsInfo(int UserPK, string PageURL, int department, int refID = 0, int pageType = 0)
        {
            AuthDA userAuthServices;
            userAuthServices = new AuthDA();
            return userAuthServices.GetWorkFlowUserRightsInfo(UserPK, PageURL, department, refID, pageType);
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
            return CommonDA.GetItemPackDetails(ipdPK, itemPK, ipdType, ipdCustomer, active, bizUnit);
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
        public static DataTable GetItemPackDetailsAuto(int ipdPK, int itemPK, int ipdType, int ipdCustomer, int active, int bizUnit, string searchValue)
        {
            return CommonDA.GetItemPackDetailsAuto(ipdPK, itemPK, ipdType, ipdCustomer, active, bizUnit, searchValue);
        }
        public static DataTable GetTaxMstList(int taxPK, int taxCategory, int taxSubCategory, byte active, int bizUnit)
        {
            return CommonDA.GetTaxMstList(taxPK, taxCategory, taxSubCategory, active, bizUnit);
        }
        public static DataTable GetTaxMstListAuto(int taxPK, int taxCategory, int taxSubCategory, byte active, int bizUnit, string searchValue)
        {
            return CommonDA.GetTaxMstListAuto(taxPK, taxCategory, taxSubCategory, active, bizUnit, searchValue);
        }
        public static bool ValidationForCancellation(int CurrPK, string type)
        {
            return CommonDA.ValidationForCancellation(CurrPK, type);
        }

        public static int ValidationForPosting(string strXml)
        {
            return CommonDA.ValidationForPosting(strXml);
        }

        public static DataTable GetVendorContactList(int? vncPK, int active, int vendorPK, int bizUnit)
        {
            return CommonDA.GetVendorContactList(vncPK, active, vendorPK, bizUnit);
        }
        /// <summary>
        /// Get Customer Supply related Tax for Misc Invoice
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <param name="taxDate"></param>
        /// <returns></returns>
        public static DataTable GetCustomerSupplyTax(int active, int bizUnit, DateTime date, int? cusPK = null, int? itemPK = null, int? isSale = null)
        {
            return CommonDA.GetCustomerSupplyTax(active, bizUnit, date, cusPK, itemPK, isSale);
        }

        /// <summary>
        /// Get Company Details
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <param name="taxDate"></param>
        /// <returns></returns>
        /// 
        public static DataTable GetCompanyDetails(int active, int bizUnit, int? companyPK, string splCond = null, int? BudgetPk=0)
        {
            return CommonDA.GetCompanyDetails(active, bizUnit, companyPK, splCond, BudgetPk);
        }

        public static DataTable GetCompanyDetailsPlantWise(int active, int bizUnit, int? companyPK)
        {
            return CommonDA.GetCompanyDetailsPlantWise(active, bizUnit, companyPK);
        }

        public static bool CheckforPOCancellation(int CurrPK)
        {
            return CommonDA.CheckforPOCancellation(CurrPK);
        }
        /// <summary>
        /// Get Version Details
        /// </summary>
        /// <returns></returns>
        public static DataTable GetVersionDetails()
        {
            return CommonDA.GetVersionDetails();
        }

        /// <summary>
        /// Get Page Server
        /// </summary>
        /// <param name="pageUrl">Page url</param>
        /// <returns>DataTable</returns>
        public static DataTable GetPageServer(string pageUrl)
        {
            return CommonDA.GetPageServer(pageUrl);
        }
        /// <summary>
        /// Method to Check Inventory locking date
        /// </summary>
        /// <param name="stockTransferDtls"></param>
        /// <returns></returns>
        public static string IsInventoryLocked(DateTime Date, int BizUnit, int module, ref string LockUptoDate)
        {
            return CommonDA.IsInventoryLocked(Date, BizUnit, module, ref LockUptoDate);
        }

        /// <summary>
        /// Method to get Products.
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static List<BusinessObject.Common.ProductDetailsBO> GetProducts(string SearchKey, int bizUnit, byte? ItemGrade = null, string BinSubType = null, int? itemPK = null, int Active = 1)
        {
            //List For Storing Product Details
            List<BusinessObject.Common.ProductDetailsBO> lstProduct = new List<BusinessObject.Common.ProductDetailsBO>();
            DataTable dtProduct = CommonDA.GetProducts(SearchKey, bizUnit, ItemGrade, BinSubType, itemPK, Active);
            BusinessObject.Common.ProductDetailsBO product;
            foreach (DataRow drProduct in dtProduct.Rows)
            {
                product = new BusinessObject.Common.ProductDetailsBO()
                {
                    ProductPK = (drProduct["PRO_PK"] == DBNull.Value) ? -1 : Convert.ToInt32(drProduct["PRO_PK"]),
                    ProductName = (drProduct["PRO_TEXT"] == DBNull.Value) ? String.Empty : Convert.ToString(drProduct["PRO_TEXT"]),
                };
                //Adding Each Items To the List
                lstProduct.Add(product);
            }
            return lstProduct;
        }

        public static DataTable GetBrandProducts(string searchBy, string searchKey, int bizUnit, int active = 1)
        {
            return CommonDA.GetBrandProducts(searchBy, searchKey, bizUnit, active);
        }


        //not used Get DataTable(Payitem UC)
        public static DataTable GetSlab(int CurrPK, int active, int bizUnit, string Pay_Element)
        {
            return CommonDA.GetSlab(CurrPK, active, bizUnit, Pay_Element);
        }


        public UserRightsBO GetUserPageRights(int UserPK, string PageURL, int bizUnit, int deptPK = 0, string PageURL2 = null)
        {
            UserRightsBO userRights;
            AuthDA userAuthServices;
            userAuthServices = new AuthDA();
            userRights = userAuthServices.GetUserPageRights(UserPK, PageURL, bizUnit, deptPK, PageURL2);
            return userRights;
        }

        public bool IsUserHasRights(int UserPK, string PageURL, int bizUnit, int deptPK = 0)
        {
            bool blnUserRights = false;
            UserRightsBO userRights;
            AuthDA userAuthServices;
            userAuthServices = new AuthDA();
            userRights = userAuthServices.GetUserPageRights(UserPK, PageURL, bizUnit, deptPK);
            if (userRights != null && userRights.Rights.Count() > 0 && userRights.Rights[0].UserDeptRight)
                blnUserRights = true;
            return blnUserRights;
        }

        public static List<BusinessObject.MaterialManagement.Material> GetAllGloveItems(int bizUnit)
        {
            //List For Storing Product Details
            List<BusinessObject.MaterialManagement.Material> lstProduct = new List<BusinessObject.MaterialManagement.Material>();
            DataTable dtProduct = CommonDA.GetAllGloveItems(bizUnit);
            BusinessObject.MaterialManagement.Material product;
            foreach (DataRow drProduct in dtProduct.Rows)
            {
                product = new BusinessObject.MaterialManagement.Material()
                {
                    ProductPK = (drProduct["ITM_PK"] == DBNull.Value) ? -1 : Convert.ToInt32(drProduct["ITM_PK"]),
                    ProductName = (drProduct["ITM_TEXT"] == DBNull.Value) ? String.Empty : Convert.ToString(drProduct["ITM_TEXT"]),
                };
                //Adding Each Items To the List
                lstProduct.Add(product);
            }
            return lstProduct;
        }

        public static DataTable GetCurrency(int CurrencyPk, int bizUnit, int Status)
        {
            return CommonDA.GetCurrency(CurrencyPk, bizUnit, Status);
        }

        public static DataTable GetAssetTypesAuto(string searchval, int atpPK, int Active, int bizUnit)
        {
            return CommonDA.GetAssetTypesAuto(searchval, atpPK, Active, bizUnit);
        }
        public static DataTable GetAssetAuto(string searchval, int asrType, int asrPK, int Active, int bizUnit)
        {
            return CommonDA.GetAssetAuto(searchval, asrType, asrPK, Active, bizUnit);
        }
        public static DataTable GetAssetItemsAuto(string searchval, int asrType, int PK, int Active, int bizUnit)
        {
            return CommonDA.GetAssetItemsAuto(searchval, asrType, PK, Active, bizUnit);
        }

        public static int? SaveTransactionComments(TrxCommentBO objTrxComment)
        {
            return CommonDA.SaveTransactionComments(objTrxComment);
        }

        public static DataTable GetTransactionComments(TrxCommentBO objTrxCmnt)
        {
            return CommonDA.GetTransactionComments(objTrxCmnt);
        }

        public static int? DeleteTransactionComments(int CurrCmntPK, DateTime CmntLastModifiedTime)
        {
            return CommonDA.DeleteTransactionComments(CurrCmntPK, CmntLastModifiedTime);
        }

        public static DataTable GetItemCategorySale(string searchValue, int itmCategpk, int sale) //deptPk
        {
            return CommonDA.GetItemCategorySale(searchValue, itmCategpk, sale);
        }

        public static DataTable GetItemSale(string searchValue, int Itempk, int bizUnit, int sale, int category, int PackSpec = 0, int SubType = 0)
        {
            return CommonDA.GetItemSale(searchValue, Itempk, bizUnit, sale, category, PackSpec, SubType);
        }
        public static DataTable GetUsersAuto(string searchval, string fieldName, int? isSysUser = null)
        {
            return CommonDA.GetUsersAuto(searchval, fieldName, isSysUser);
        }
        public static DataTable GetItemDetailsByPK(int itemID, int sbuPk, int dept = 0)
        {
            return DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialDetails(itemID, sbuPk, dept);
        }

        public static DataTable GetItemRateDetails(int itemID, DateTime Date, int CusPk, int Currencyid)
        {
            return DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialRateDetails(itemID, Date, CusPk, Currencyid);
        }

        public static decimal GetUOMConversionFactor(int itemPk, int FromUOMPK, int ToUOMPK)
        {
            return CommonDA.GetUOMConversionFactor(itemPk, FromUOMPK, ToUOMPK);
        }

        public static DataTable GetInitialTaskAction(int ProcessID, int ReqDeptID, int userPk)
        {
            return CommonDA.GetInitialTaskAction(ProcessID, ReqDeptID, userPk);
        }

        /// <summary>
        /// Get Company Mapping Details
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <param name="taxDate"></param>
        /// <returns></returns>
        /// 
        public static DataTable GetCompanyMappingDetails(int cmpPk, short active, int bizUnit, int deptPK)
        {
            return DataAccess.Administration.Masters.CompanyMasterDA.GetCompanyMappingDetails(cmpPk, active, bizUnit, deptPK).Tables[0];
        }

        /// <summary>
        /// Method to get all cost centers mapped to this account
        /// </summary>
        /// <param name="AccountPk"></param>
        /// <returns></returns>
        public static DataTable GetCostCenter(int AccountPk, decimal Amount, int RefPk = 0, int JurCurrPK = 0)
        {
            return CommonDA.GetCostCenter(AccountPk, Amount, RefPk, JurCurrPK);
        }

        /// <summary>
        /// Method to get all cost centers against a voucher
        /// </summary>
        /// <param name="VoucherPk"></param>
        /// <returns></returns>
        public static DataTable GetAllCostCenter(long VoucherPk)
        {
            return CommonDA.GetAllCostCenter(VoucherPk);
        }

        /// <summary>
        /// Save Summary
        /// </summary>
        /// <param name="pK"></param>
        /// <param name="processPK"></param>
        /// <returns></returns>
        public static int SaveSummary(long pK, int processPK)
        {
            return CommonDA.SaveSummary(pK, processPK);
        }

        /// <summary>
        /// Get Purchase Invoice Company mapping Details
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <param name="taxDate"></param>
        /// <returns></returns>
        /// 
        public static DataTable GetDepartmentsWithUserPermission(int active, int bizUnit, int userPk)
        {
            return CommonDA.GetDepartmentsWithUserPermission(active, bizUnit, userPk);
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
            return CommonDA.GetModule(modPk, DashletActive, active, bizUnit);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static DataTable GetChangeHistory(int transPk)
        {
            return CommonDA.GetChangeHistory(transPk);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static DataTable GetAutoUserRoleInboxMapping(string searchValue, string type, int? modPK, int? userLogin, int? EmpDept, int bizUnit)
        {
            return CommonDA.GetAutoUserRoleInboxMapping(searchValue, type, modPK, userLogin, EmpDept, bizUnit);
        }
        /// <summary>
        /// Get COA Parent in AutoComplate
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public static DataTable GetCoaParent(string searchKey, string searchField, int active, int bizUnit, int isGroup, int AccountPk)
        {
            return CommonDA.GetCoaParent(searchKey, searchField, active, bizUnit, isGroup, AccountPk);
        }
        public static DataTable GetBudgetCoaParent(string searchKey, string searchField, int active, int bizUnit, int isGroup, int AccountPk,int Budget)
        {
            return CommonDA.GetBudgetCoaParent(searchKey, searchField, active, bizUnit, isGroup, AccountPk, Budget);
        }
        public static DataTable GetMappedItemvendor(string searchKey, string searchField, int bizUnit, string categoryPk, string VenPk)
        {
            return CommonDA.GetMappedItemvendor(searchKey, searchField, bizUnit, categoryPk, VenPk);
        }

        public static DataTable GetInvoiceGstType(int? FtmPk, int Active, int bizUnit, int invType, int invGstType)
        {
            return CommonDA.GetInvoiceGstType(FtmPk, Active, bizUnit, invType, invGstType);
        }

        public static DataTable GetPortDetails(string searchValue, int PrmPK, byte Active, int bizUnit, int SIType, int SaleFromPort, int SaleToPort, int PurFromPort, int PurToPort)
        {
            return CommonDA.GetPortDetails(searchValue, PrmPK, Active, bizUnit, SIType, SaleFromPort, SaleToPort, PurFromPort, PurToPort);
        }

        public static DataTable GetPortDetailsByPK(int prmPK, int status, int bizUnit)
        {
            return CommonDA.GetPortDetailsByPK(prmPK, status, bizUnit);
        }
        public static DataTable GetHSCodeList(int bizUnit)
        {
            return CommonDA.GetHSCodeList(bizUnit);
        }
        public static bool IsFinancialYearExist(DateTime Date, int bizUnit)
        {
            return CommonDA.IsFinancialYearExist(Date, bizUnit);
        }

        public static DataTable GetBrandPackSpec(string searchValue, int Active, int BizUnit)
        {
            return CommonDA.GetBrandPackSpec(searchValue, Active, BizUnit);
        }




        public static DataTable GetPackingSpecCategory(int sbuPk, string srhcType, int type = 0)
        {
            return CommonDA.GetPackingSpecCategory(sbuPk, srhcType, type);
        }


        public static DataTable GetPackingSpecType(string searchValue, int Con_PK, int Active, int Cgt_Value, int BizUnit)
        {
            return CommonDA.GetPackingSpecType(searchValue, Con_PK, Active, Cgt_Value, BizUnit);
        }

        public static DataTable GetPackingSpec(string searchValue, int Con_PK, int Itc_Pk, int Cus_Pk, int IsProductRequired)
        {
            return CommonDA.GetPackingSpec(searchValue, Con_PK, Itc_Pk, Cus_Pk, IsProductRequired);
        }




        /// <summary>
        /// Get Categro Value
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetCategoryValue(int categoryPK, int active)
        {
            return CommonDA.GetCategoryValue(categoryPK, active);
        }

        /// <summary>
        /// Get Formula Listing
        /// </summary>
        public static DataTable GetFormulaList(int bizUnit)
        {
            return CommonDA.GetFormulaList(bizUnit);
        }

        /// <summary>
        /// Delete Formula Details
        /// </summary>
        public static int DeleteFormulaDetails(int CurrPk)
        {
            return CommonDA.DeleteFormulaDetails(CurrPk);
        }

        /// <summary>
        /// Save Formula Details
        /// </summary>
        public static int SaveFormulaDetails()
        {
            return CommonDA.SaveFormulaDetails();
        }

        public static DataTable GetPackSepecDetails(int PackSpecPK, int IsProductRequired)
        {
            return DataAccess.CommonManagement.CommonDA.GetPackSepecDetails(PackSpecPK, IsProductRequired);
        }

        public static DataTable GetReport(int userID, int Group, int bizUnit, int Active, string searchValue)
        {
            return CommonDA.GetReport(userID, Group, bizUnit, Active, searchValue);
        }

        public static List<AutoCompleteBO> GetAutoBinCardsForIssue(int BinDept, int bizunit, string BinNo, int PageNumber = 0, int PageSize = 0, int IsQaPassed = 0, int department = 0, int IsAutocomplete = 0, int ItemPK = 0)
        {
            //List For Storing All values
            List<AutoCompleteBO> lstValue = new List<AutoCompleteBO>();
            BinCardIssueBO ObjBinIssue = new BinCardIssueBO();
            string strxml = CommonDA.GetAutoBinCardsForIssue(BinDept, bizunit, BinNo, PageNumber, PageSize, IsQaPassed, department, IsAutocomplete, ItemPK);
            if (!string.IsNullOrEmpty(strxml))
            {
                ObjBinIssue = CommonFunctions.XmlDeserialize<BinCardIssueBO>(strxml);

                foreach (var obj in ObjBinIssue.Detail)
                {
                    AutoCompleteBO value = new AutoCompleteBO()
                    {
                        Key = (obj.BCH_PK > 0 || obj.BCH_PK.ToString() != string.Empty) ? obj.BCH_PK : -1,
                        Name = (obj.BCH_NO == null || obj.BCH_NO.ToString() == string.Empty) ? String.Empty : obj.BCH_NO
                    };
                    //Add Each Category To category list
                    lstValue.Add(value);
                }
            }
            return lstValue;
        }

        public static string GetBinCardsForIssue(int BinDept, int bizunit, string BinPk, int? product = null, int? line = null, int? BinType = null, int? Aql = null, string date = null, int? shift = null, int itmGrade = 0, int? trxpk = 0, string BinNo = null, int PageNumber = 0, int PageSize = 0, int IsQaPassed = 0, string IssueTrxDate = null, string ExcludePks = null)
        {
            return CommonDA.GetBinCardsForIssue(BinDept, bizunit, BinPk, product, line, BinType, Aql, date, shift, itmGrade, trxpk, BinNo, PageNumber, PageSize, IsQaPassed, IssueTrxDate, ExcludePks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GroupPK"></param>
        /// <returns></returns>
        public static DataTable GetCategoryByGroup(int? GroupType, int? Group, int? pk = null, int? active = null, int? parent = null, int? conGroup = null)
        {
            return CommonDA.GetCategoryByGroup(GroupType, Group, pk, active, parent, conGroup);
        }
        /// <summary>
        /// Used Customer List  (Auto Complete)
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="name"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetUsedCustomer(int pk, string name, int bizUnit, int active)
        {
            return CommonDA.GetUsedCustomer(pk, name, bizUnit, active);
        }
        /// <summary>
        /// Customer List  (Auto Complete)
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="name"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetCustomer(int pk, string name, int bizUnit, int active, int? IsConsultant = null)
        {
            return CommonDA.GetCustomer(pk, name, bizUnit, active, IsConsultant);
        }
        public static DataSet GetHSNNO(int pk, string name, int bizUnit, int active, int? IsConsultant = null)
        {
            return CommonDA.GetHSNNO(pk, name, bizUnit, active, IsConsultant);
        }

        public static DataSet GetAssetTypeDetails(GridPrams paramObj, int atpPK, int BizUnit, int DeptPK, int Active)
        {
            return CommonDA.GetAssetTypeDetails(paramObj, atpPK, BizUnit, DeptPK, Active);
        }
        public static DataTable GetProductGroups(string searchval)
        {
            return CommonDA.GetProductGroups(searchval);
        }
        public static DataTable GetCompanyDetails(int Bizunit)
        {
            return CommonDA.GetCompanyDetails(Bizunit);
        }
        public static DataTable GetStatus()
        {
            return CommonDA.GetStatus();
        }
        public static DataTable GetCompound(string searchKey, int Status, int bizunit, int CompanyPK)
        {
            return CommonDA.GetCompound(searchKey, Status, bizunit, CompanyPK);
        }
        public static DataTable GetDispersion(string searchKey, int Status, int bizunit, int CompanyPK, string Type)
        {
            return CommonDA.GetDispersion(searchKey, Status, bizunit, CompanyPK, Type);
        }
        public static DataTable GetDispersionTypes()
        {
            return CommonDA.GetDispersionTypes();
        }
        public static DataTable GetAuditLogDisplayStatus(int CurrPk, string VoucherType)
        {
            return CommonDA.GetAuditLogDisplayStatus(CurrPk, VoucherType);
        }
        public static DataTable GetAuditLogDetails(int CurrPk)
        {
            return CommonDA.GetAuditLogDetails(CurrPk);
        }
        public static FinTrxLogBO GetAuditLogComparision(int CurrPk, int Version)
        {
            try
            {
                FinTrxLogBO rfqResponseHeaderObj = new FinTrxLogBO();
                string AuditDtls = CommonDA.GetAuditLogComparision(CurrPk, Version);
                if (AuditDtls != string.Empty)
                {
                    rfqResponseHeaderObj = (FinTrxLogBO)CommonFunctions.DeserializeObject(AuditDtls, rfqResponseHeaderObj);
                    return rfqResponseHeaderObj;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
