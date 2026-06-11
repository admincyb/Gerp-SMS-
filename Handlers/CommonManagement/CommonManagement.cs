using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using BusinessLogic;
using BusinessObject;
using GTIService;
using Newtonsoft;

using System.Web.SessionState;
using System.Text;
using BusinessObject.CommonManagement;
using System.Xml;
using System.Web.Script.Serialization;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        #region methods

        /// <summary>
        /// handiling Common management handlers.
        /// </summary>
        /// <param name="context"></param>
        private static void CommonMaster(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    // Get Process ID
                    case "GetProcessID":
                        Handlers.GetProcessID(context);
                        break;
                    case "GetWorkflowStatus":
                        Handlers.GetWorkflowStatus(context);
                        break;
                    case "GetAccountType":
                        Handlers.GetAccountType(context);
                        break;
                    case "GetAccountTypeXML":
                        Handlers.GetAccountTypeXML(context);
                        break;
                    case "GetAccountTypeAuto":
                        Handlers.GetAccountTypeAuto(context);
                        break;
                    // Get all Country List to fill DropDown
                    case "GetCountryList":
                        Handlers.GetCountryList(context);
                        break;
                    // Get all Project List to fill DropDown
                    case "GetProjectListAuto":
                        Handlers.GetProjectListAuto(context);
                        break;
                    // Get all Company List to fill DropDown
                    case "GetCompany":
                        Handlers.GetCompany(context);
                        break;
                    case "GetSBU":
                        Handlers.GetSBU(context);
                        break;
                        //Get Company Autocomplete
                    case "GetCompanyAutoComplete":
                        Handlers.GetCompanyAutoComplete(context);
                        break;
                    // To get All Currency For Fill in DropDown
                    case "GetStateList":
                        Handlers.GetStateList(context);
                        break;
                    // To get All Currency For Fill in DropDown
                    case "GetCurrencyList":
                        Handlers.GetCurrencyList(context);
                        break;
                    // Get All Department
                    case "GetDepartment":
                        Handlers.GetDepartmentList(context);
                        break;

                    // To Fill Department name in DropDown
                    case "GetDepartmentName":
                        Handlers.GetDepartmentName(context);
                        break;
                    // To Fill Department type and anme in DropDown
                    case "GetParentDepartments":
                        Handlers.GetParentDepartments(context);
                        break;
                    case "GetParentDepartmentCategories":
                        Handlers.GetParentDepartmentCategories(context);
                        break;
                    case "GetParentDepartmentCategoriesByID":
                        Handlers.GetParentDepartmentCategoriesByID(context);
                        break;

                    // For Department tree View
                    case "GetDepartmentDetails":
                        Handlers.GetDeptDtls(context);
                        break;
                    case "GetAccount":
                        Handlers.GetAccount(context);
                        break;

                    case "GetAccountWithCode":
                        Handlers.GetAccountWithCode(context);
                        break;

                    // Auto Complete For Country
                    case "GetCountryDetailsAuto":
                        Handlers.GetCountryAutoComplete(context);
                        break;
                    // Auto Complete For State
                    case "GetStateDetailsAuto":
                        Handlers.GetStateAutoComplete(context);
                        break;
                    // Auto Complete For Currency
                    case "GetCurrencyDetailsAuto":
                        Handlers.GetCurrencyAutoComplete(context);
                        break;
                    case "GetUserGroup":
                        Handlers.GetUserGroup(context);
                        break;
                    //27042011
                    case "MenuListSearch":
                        Handlers.MenuListSearch(context);
                        break;
                    case "GetWrkfCommentList":
                        Handlers.GetWrkfCommentList(context);
                        break;
                    case "GetShift":
                        Handlers.GetShift(context);
                        break;
                    case "GetLines":
                        Handlers.GetLines(context);
                        break;
                    case "GetProducts":
                        Handlers.GetProducts(context);
                        break;
                    case "GetPlans":
                        Handlers.GetPlans(context);
                        break;
                    case "GetEmployees":
                        Handlers.GetEmployees(context);
                        break;
                    case "GetCompoundBatch":
                        Handlers.GetCompoundBatchno(context);
                        break;
                    case "GetProductDetails":
                        Handlers.GetProductDetails(context);
                        break;
                    case "GetMenuAutoComplete":
                        Handlers.GetMenuAutoComplete(context);
                        break;
                    case "SaveWorkFlow":
                        Handlers.SaveWorkFlow(context);
                        break;
                    case "UpdateReferenceID":
                        Handlers.UpdateReferenceID(context);
                        break;
                    case "GetPackingType":
                        Handlers.GetPackingType(context);
                        break;
                    case "GetCategoryValue":
                        Handlers.GetCategoryValue(context);
                        break;
                    case "GetPackingTypeDtl":
                        Handlers.GetPackingTypeDtl(context);
                        break;
                    case "GetCustomers":
                        Handlers.GetCustomers(context);
                        break;
                    case "GetCheckList":
                        Handlers.GetCheckList(context);
                        break;
                    case "GetTemplateCheckList":
                        Handlers.GetTemplateCheckList(context);
                        break;
                    case "GetAppConfigTree":
                        Handlers.GetAppConfigTree(context);
                        break;
                    case "GetAppConfig":
                        Handlers.GetAppConfig(context);
                        break;
                    case "GetAppStatus":
                        Handlers.GetAppStatus(context);
                        break;
                    case "CommonDDL":
                        Handlers.CommonDDL(context);
                        break;
                    case "GetClassification":
                        Handlers.GetClassification(context);
                        break;
                    case "CheckInventoryLocking":
                        Handlers.CheckInventoryLocking(context);
                        break;
                    case "GetItemUOMConversionFactor":
                        Handlers.GetItemUOMConversionFactor(context);
                        break;
                    case "POCategoryDDLGet":
                        Handlers.POCategoryDDLGet(context);
                        break;
                    case "DepartmentsDDL":
                        Handlers.DepartmentsDDL(context);
                        break;
                    case "GetConstantValue":
                        Handlers.GetConstantValue(context);
                        break;
                    case "GetInvestmentValue":
                        Handlers.GetInvestmentValue(context); 
                        break;
                    case "GetInvestmentList":
                        Handlers.GetInvestmentList(context);
                        break;
                    case "GetConstantValueAuto":
                        Handlers.GetConstantValueAuto(context);
                        break;
                    case "GetPortDetails":
                        Handlers.GetPortDetails(context);
                        break;
                    case "SubDepartmentsDDL":
                        Handlers.SubDepartmentsDDL(context);
                        break;
                    case "GetPlant":
                        Handlers.GetPlant(context);
                        break;
                    case "GetCompanyMappingDetails":
                        Handlers.GetCompanyMappingDetails(context);
                        break;
                    case "GetCompanyDisplayNames":
                        Handlers.GetCompanyDisplayNames(context);
                        break;
                    case "GetCurrentDepartment":
                        Handlers.GetCurrentDepartment(context);
                        break;
                    case "GetAccountTypesforMapping":
                        Handlers.GetAccountTypesforMapping(context);
                        break;
                    case "GetEmployeeCreatorList":
                        Handlers.GetEmployeeCreatorList(context);
                        break;


                }
            }
        }

        private static void GetCurrentDepartment(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                //if (Request.Params["SBU"] != null)
                //{
                //    bizUnit = Convert.ToInt32(Request.Params["SBU"]);
                //}
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL

                Response.Write(objUser.CurrentDeptPK.ToString());

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary> 
        /// Get user departments
        /// </summary>
        /// <param name="context"></param>
        private static void POCategoryDDLGet(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int Pk = 0;
            int active = 1;
            int ProcessID = 0;
            int RefID = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["DeptPk"] != null && Request.Params["DeptPk"] != "null")
                {
                    Pk = Convert.ToInt32(Request.Params["DeptPk"]);
                }
                if (Request.Params["ProcessID"] != null && Request.Params["ProcessID"] != "null")
                {
                    ProcessID = Convert.ToInt32(Request.Params["ProcessID"]);
                }
                if (Request.Params["RefID"] != null && Request.Params["RefID"] != "null")
                {
                    RefID = Convert.ToInt32(Request.Params["RefID"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.POCategoryDDLGet(Pk, active, objUser.PKUser, objUser.SBUID, ProcessID, RefID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary> 
        /// Get user departments
        /// </summary>
        /// <param name="context"></param>
        private static void DepartmentsDDL(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int Pk = 0;
            int active = 1;  
            int ProcessID=0;
            int RefID = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["DeptPk"] != null)
                {
                    Pk = Convert.ToInt32(Request.Params["DeptPk"]);
                }
                if (Request.Params["ProcessID"] != null)
                {
                    ProcessID = Convert.ToInt32(Request.Params["ProcessID"]);
                }
                if (Request.Params["RefID"] != null)
                {
                    RefID = Convert.ToInt32(Request.Params["RefID"]);
                }



                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetDepartmentsDDL(Pk, active, objUser.PKUser, objUser.SBUID, ProcessID, RefID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetConstantValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int Pk = 0;
            int Active = 1;
            int Group = 0;
            int GroupTypeConst = 0;
            int GroupConstant = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["Pk"] != null)
                {
                    Pk = Convert.ToInt32(Request.Params["Pk"]);
                }
                if (Request.Params["Group"] != null)
                {
                    Group = Convert.ToInt32(Request.Params["Group"]);
                }
                if (Request.Params["GroupTypeConst"] != null)
                {
                    GroupTypeConst = Convert.ToInt32(Request.Params["GroupTypeConst"]);
                }
                if (Request.Params["GroupConstant"] != null)
                {
                    GroupConstant = Convert.ToInt32(Request.Params["GroupConstant"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetConstantValue(Pk, Active, Group, GroupTypeConst, GroupConstant, objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetInvestmentValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int Pk = 0;
            int Active = 1;  
            int Group=0;
            int GroupTypeConst = 0;
            int GroupConstant = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["Pk"] != null)
                {
                    Pk = Convert.ToInt32(Request.Params["Pk"]);
                }
                if (Request.Params["Group"] != null)
                {
                    Group = Convert.ToInt32(Request.Params["Group"]);
                }
                if (Request.Params["GroupTypeConst"] != null)
                {
                    GroupTypeConst = Convert.ToInt32(Request.Params["GroupTypeConst"]);
                }
                if (Request.Params["GroupConstant"] != null)
                {
                    GroupConstant = Convert.ToInt32(Request.Params["GroupConstant"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetInvestmentValue(Pk, Active, Group, GroupTypeConst, GroupConstant, objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetInvestmentList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int Pk = 0;
            int Active = 1;
            string SeartchValue = "%";
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SeartchValue"] != null)
                {
                    SeartchValue = Request.Params["SeartchValue"];
                }
                if (Request.Params["Pk"] != null)
                {
                    Pk = Convert.ToInt32(Request.Params["Pk"]);
                }
                if (Request.Params["Active"] != null)
                {
                    Active = Convert.ToInt32(Request.Params["Active"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetInvestmentList(Pk,SeartchValue, Active, objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetConstantValueAuto(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int Pk = 0;
            int Active = 1;
            int Group = 0;
            int GroupTypeConst = 0;
            int GroupConstant = 0;
            string searchValue = string.Empty;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["Pk"] != null)
                {
                    Pk = Convert.ToInt32(Request.Params["Pk"]);
                }
                if (Request.Params["Group"] != null)
                {
                    Group = Convert.ToInt32(Request.Params["Group"]);
                }
                if (Request.Params["GroupTypeConst"] != null)
                {
                    GroupTypeConst = Convert.ToInt32(Request.Params["GroupTypeConst"]);
                }
                if (Request.Params["GroupConstant"] != null)
                {
                    GroupConstant = Convert.ToInt32(Request.Params["GroupConstant"]);
                }
                if (Request.Params["SearchValue"] != null)
                {
                    searchValue = "%" + Request.Params["SearchValue"].ToString() + "%";
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetConstantValueAuto(Pk, Active, Group, GroupTypeConst, GroupConstant, objUser.SBUID, searchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        

        /// <summary>
        /// Get user From Port and To Port Autocomplete
        /// </summary>
        /// <param name="context"></param>
        private static void GetPortDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int Pk = 0;
            int active = 1;
            int ProcessID = 0;
            int RefID = 0;

            List<AutoCompleteBO> result;
            DataTable dtData;
            int SaleFromPort = 0;
            int SaleToPort = 0;
            int PurFromPort = 0;
            int PurToPort = 0;
            int Type = 0;
            int PrmPK = 0;
            string searchValue = string.Empty;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SaleFromPort"] != null)
                {
                    SaleFromPort = Convert.ToInt32(Request.Params["SaleFromPort"]);
                }
                if (Request.Params["SaleToPort"] != null)
                {
                    SaleToPort = Convert.ToInt32(Request.Params["SaleToPort"]);
                }
                if (Request.Params["PurFromPort"] != null)
                {
                    PurFromPort = Convert.ToInt32(Request.Params["PurFromPort"]);
                }
                if (Request.Params["PurToPort"] != null)
                {
                    PurToPort = Convert.ToInt32(Request.Params["PurToPort"]);
                }
                if (Request.Params["Type"] != null)
                {
                    Type = Convert.ToInt32(Request.Params["Type"]);
                }
                if (Request.Params["PrmPK"] != null)
                {
                    PrmPK = Convert.ToInt32(Request.Params["PrmPK"]);
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                // Get the list in json string format from BL
                //Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetDepartmentsDDL(Pk, active, objUser.PKUser, objUser.SBUID, ProcessID, RefID));
                //Response.Write(BusinessLogic.CommonManagement.CommonBL.GetPortDetails(searchValue, PrmPK, (byte)DbActiveStatus.ACTIVE, objUser.SBUID,
                //                  SIType, SaleFromPort, SaleToPort, PurFromPort, PurToPort));

                DataTable dtTable = BusinessLogic.CommonManagement.CommonBL.GetPortDetails(searchValue, PrmPK, (byte)DbActiveStatus.ACTIVE, objUser.SBUID,
                                  Type, SaleFromPort, SaleToPort, PurFromPort, PurToPort);
                string jString = string.Empty;
                if (dtTable.Rows.Count > 0)
                {
                    jString = GTIService.CommonFunctions.GetTextValueList(dtTable
                                ,"PRM_NAME"
                                ,"PRM_PK");
                }
                Response.Write(jString);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }



        /// <summary>
        /// Get sub departments
        /// </summary>
        /// <param name="context"></param>
        private static void SubDepartmentsDDL(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int Pk = 0;
            int ParentPk = 0;
            int active = 1;           
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["DeptPk"] != null && Request.Params["DeptPk"] != "null")
                {
                    int.TryParse(Request.Params["DeptPk"], out Pk);
                   // Pk = Convert.ToInt32(Request.Params["DeptPk"]);
                }
                if (Request.Params["ParentDeptPk"] != null && Request.Params["ParentDeptPk"] != "null")
                {
                    int.TryParse(Request.Params["ParentDeptPk"], out ParentPk);
                    //ParentPk = Convert.ToInt32(Request.Params["ParentDeptPk"]);
                }

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetSubDepartmentsDDL(Pk, active, ParentPk, objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveWorkFlow(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string workFlowDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.SaveWorkFlow(workFlowDetails, objUser).ToString());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.Write("0");
            }
        }

        /// <summary>
        /// function used to get the shif details corresponding to value entered in text box if it is an auto complete 
        /// </summary>
        /// <param name="context"></param>
        private static void GetProductDetails(HttpContext context)
        {
            int productID = 0;
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.QueryString["ProductID"] != null)
                {
                    productID = Convert.ToInt32(Request.QueryString["ProductID"].ToString());
                }
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetProductDetails(productID));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// function used to get the shif details corresponding to value entered in text box if it is an auto complete 
        /// </summary>
        /// <param name="context"></param>
        private static void GetShift(HttpContext context)
        {
            string searchValue = string.Empty;
            int bizUnit = 0;
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                //int bizUnit = Request.Params["SBU"] != null ? Convert.ToInt32(Request.Params["SBU"]) : 0;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetShift(bizUnit, searchValue));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// function used to get the shif details corresponding to value entered in text box if it is an auto complete 
        /// </summary>
        /// <param name="context"></param>
        private static void GetEmployees(HttpContext context)
        {
            string searchValue = string.Empty;
            int bizUnit = 0;
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                //int bizUnit = Request.Params["SBU"] != null ? Convert.ToInt32(Request.Params["SBU"]) : 0;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetEmployees(bizUnit, searchValue));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetLines(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchValue = string.Empty;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetLines(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetCustomers(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchValue = string.Empty;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCustomer(0, string.Empty, bizUnit, 1));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// function used to get the CheckList details corresponding to Pk 
        /// </summary>
        /// <param name="context"></param>
        private static void GetCheckList(HttpContext context)
        {
            int checkListID = 0;
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.QueryString["checkListID"] != null)
                {
                    checkListID = Convert.ToInt32(Request.QueryString["checkListID"].ToString());
                }
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCheckListDetails(checkListID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        private static void GetTemplateCheckList(HttpContext context)
        {
            int checkListPK = 0;
            int bizUnit = 0;
            int active = 0;
            int processID = 0;
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.QueryString["checkListPK"] != null)
                {
                    checkListPK = Convert.ToInt32(Request.QueryString["checkListPK"].ToString());
                }
                if (Request.Params["Active"] != null)
                {
                    active = Convert.ToInt16(Request.Params["Active"].ToString());
                }
                if (Request.Params["processID"] != null)
                {
                    processID = Convert.ToInt16(Request.Params["processID"].ToString());
                }
                if (Request.Params["SBU"] != null)
                {
                    bizUnit = Convert.ToInt16(Request.Params["SBU"].ToString());
                }
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetTemplateCheckList(checkListPK, bizUnit, active, processID));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetCategoryValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int categoryPK = 0;
            int active = 2;
            try
            {
                if (Request.QueryString["CategoryPK"] != null)
                {
                    categoryPK = Convert.ToInt32(Request.QueryString["CategoryPK"].ToString());
                }
                if (Request.QueryString["Active"] != null)
                {
                    active = Convert.ToInt32(Request.QueryString["Active"].ToString());
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCategoryValue(categoryPK, active));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPackingType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int typePK = 0;
            int bizUnit = 0;
            try
            {
                if (Request.QueryString["typePK"] != null)
                {
                    typePK = Convert.ToInt32(Request.QueryString["typePK"].ToString());
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetConstMstValues(typePK, 0, ConstGroupType.Packing, (int)PackingType.PakingMaterialType, 1, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetClassification(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int typePK = 0;
            int bizUnit = 0;
            try
            {
                //if (Request.QueryString["typePK"] != null)
                //{
                //    typePK = Convert.ToInt32(Request.QueryString["typePK"].ToString());
                //}
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetConstMstValues(typePK, 0, ConstGroupType.Packing, (int)ConstGroupType.Classification, 1, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPackingTypeDtl(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int typePK = 0;
            int bizUnit = 0;
            try
            {
                if (Request.QueryString["typePK"] != null && Request.QueryString["typePK"] != "null")
                {
                    typePK = Convert.ToInt32(Request.QueryString["typePK"].ToString());
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetConstMstValuesDtl(typePK, 0, ConstGroupType.Packing, (int)PackingType.PakingMaterialType, Convert.ToInt32(DbActiveStatus.HASPK), bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetAppConfig(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int bizUnit = 0;
            string cfgValue = "";
            string splCond = string.Empty;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL              
                if (Request.Params["CfgValue"] != null)
                {
                    cfgValue = Request.Params["CfgValue"];
                }
                if (Request.Params["splCond"] != null)
                {
                    splCond = Request.Params["splCond"];
                }

                DataTable dtTable = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(objUser.SBUID, cfgValue, splCond);
                string jString = string.Empty;
                if (dtTable.Rows.Count > 0)
                {
                    jString = GTIService.CommonFunctions.GetTextValueList(dtTable
                                , GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD
                                , GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD);
                }
                Response.Write(jString);

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Get App Configuration");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetAppConfigTree(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            int? machinePk = null;
            int? tankPk = null;

            try
            {
                if (Request.QueryString["machinePK"] != null)
                {
                    machinePk = Convert.ToInt32(Request.QueryString["machinePK"].ToString());
                }
                if (Request.QueryString["tankPk"] != null)
                {
                    tankPk = Convert.ToInt32(Request.QueryString["tankPk"].ToString());
                }

                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                string jString = string.Empty;
                DataTable dataTable = BusinessLogic.CommonManagement.CommonBL.GetAppConfigTree(machinePk, tankPk, (short)DbActiveStatus.ACTIVE);
                if (dataTable.Rows.Count > 0)
                {
                    jString = GTIService.CommonFunctions.GetTreeList(dataTable,
                                    "PK",
                                    "NAME",
                                    "PARENT",
                                    "HAS_CHILD",
                                    "IS_CHECKED",
                                    string.Empty,
                                    "IS_ITEM"
                                    );
                }
                Response.Write(jString);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetProducts(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchValue = string.Empty;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetProducts(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPlans(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            string searchValue = string.Empty;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetPlans(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompoundBatchno(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            string searchValue = string.Empty;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCompoundBatchno(bizUnit, 0));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetWrkfCommentList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int refPk = Request.Params["RefPk"] != null ? Convert.ToInt32(Request.Params["RefPk"]) : 0;
                int appID = Request.Params["AppID"] != null ? Convert.ToInt32(Request.Params["AppID"]) : 0;
                int procID = Request.Params["ProcID"] != null ? Convert.ToInt32(Request.Params["ProcID"]) : 0;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetWrkfCommentList(refPk, appID, procID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Menu Details For Auto Compelete
        /// </summary>
        /// <param name="context"></param>
        private static void MenuListSearch(HttpContext context)
        {
            string prefixText = context.Request.QueryString["q"];
            DataTable dtMenu = new DataTable();
            dtMenu = BusinessLogic.MenuManager.GetMenuDetailsSearch(prefixText);

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dtMenu.Rows)
            {
                sb.Append(string.Format("{0}-{1}-{2}{3}",
                          dr["UmsNem"], dr["UmsGim"], dr["UmsKln"],
                            Environment.NewLine));
            }
            context.Response.Write(sb.ToString());
        }

        /// <summary>
        /// Get User Group List
        /// </summary>
        /// <param name="context"></param>
        private static void GetUserGroup(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetUserGroup());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Department Details For TreeView
        /// </summary>
        /// <param name="context"></param>
        private static void GetDeptDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int deptID = Request.Params["deptID"] != null ? Convert.ToInt32(Request.Params["deptID"]) : 0;
                int deptParentId = Request.Params["deptParentID"] != null ? Convert.ToInt32(Request.Params["deptParentID"]) : 0;
                int bizUnit = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                int userGroup = Request.Params["UserGroup"] != null ? Convert.ToInt32(Request.Params["UserGroup"]) : 0;
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetDepartmentDtls(deptID, deptParentId, bizUnit, userGroup));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }


        /// <summary>
        /// Get Workflow Status
        /// </summary>
        /// <param name="context"></param>
        private static void GetWorkflowStatus(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int refID = 0;
            int processID = 0;
            string path = "";
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["RefID"] != null)
                {
                    refID = Convert.ToInt32(Request.Params["RefID"]);
                }
                if (Request.Params["ProcessID"] != null)
                {
                    processID = Convert.ToInt32(Request.Params["ProcessID"]);
                }
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetWorkflowStatus(refID, processID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }


        /// <summary>
        /// Get Plant
        /// </summary>
        /// <param name="context"></param>
        private static void GetPlant(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int plantID = 0;
            int sbuID=0;
            string path = "";
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["PlantID"] != null)
                {
                    plantID = Convert.ToInt32(Request.Params["PlantID"]);
                }
                if (Request.Params["SBU"] != null)
                {
                    sbuID = Convert.ToInt32(Request.Params["SBU"]);
                }
                XmlDocument plantDoc = CommonFunctions.ObjectTOXml(new PlantBO { ACTIVE = 1, BIZUNIT = sbuID, PLT_PK = plantID });
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetPlant(plantDoc.InnerXml));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }


        /// <summary>
        /// Get Process ID
        /// </summary>
        /// <param name="context"></param>
        private static void GetProcessID(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int deptID = 0;
            string path = "";
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["DepID"] != null)
                {
                    deptID = Convert.ToInt32(Request.Params["DepID"]);
                }
                if (Request.Params["Path"] != null)
                {
                    path = Request.Params["Path"];
                }

                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetProcesID(path, deptID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }
        /// <summary>
        /// Get Department Name and Pk To Fill Deparment Drop Down
        /// </summary>
        /// <param name="context"></param>
        private static void GetDepartmentName(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int deptID = 0;
            int deptParentId = 0;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["deptID"] != null)
                {
                    deptID = Convert.ToInt32(Request.Params["deptID"]);
                }
                if (Request.Params["deptParentID"] != null)
                {
                    deptParentId = Convert.ToInt32(Request.Params["deptParentID"]);
                }

                bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetDepartmentName(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }

        /// <summary>
        /// Get Account Type
        /// </summary>
        /// <param name="context"></param>
        private static void GetAccountType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int subType = 0;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SubType"] != null)
                {
                    subType = Convert.ToInt32(Request.Params["SubType"]);
                }

                bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetAccountType(subType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }
        /// <summary>
        /// Get Application Status
        /// </summary>
        /// <param name="context"></param>
        private static void GetAppStatus(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string subType = string.Empty;
            string type = string.Empty;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SubType"] != null)
                {
                    subType = Request.Params["SubType"];
                }

                if (Request.Params["Type"] != null)
                {
                    type = Request.Params["Type"];
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetAppStatus(type, subType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }

        /// <summary>
        ///  Get Account Type with XML Param
        /// </summary>
        /// <param name="context"></param>
        private static void GetAccountTypeXML(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string RequestString = GetRequestString(context);
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetAccountType(RequestString));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetAccountTypeAuto(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string SubType = Request.Params["SubType"] != null ? Request.Params["SubType"].ToString() : "";
                string xmlstr = GTIService.CommonFunctions.JsonToXml(SubType);
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                int AccountPK = Request.Params["P_COA_PK"] != null ? int.Parse(Request.Params["P_COA_PK"].ToString()) : 0;
                int Active = Request.Params["ACTIVE"] != null ? int.Parse(Request.Params["ACTIVE"].ToString()) : 0;
                int IsGroup = Request.Params["COA_IS_GROUP"] != null ? int.Parse(Request.Params["COA_IS_GROUP"].ToString()) : 0;
                int BizUnitPK = Request.Params["BIZUNIT_PK"] != null ? int.Parse(Request.Params["BIZUNIT_PK"].ToString()) : 0;
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetAccountTypeAuto(xmlstr, AccountPK, Active, IsGroup, BizUnitPK, srchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Department Name and type To Fill Deparment Drop Down
        /// </summary>
        /// <param name="context"></param>
        private static void GetParentDepartments(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetParentDepartments(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }
        /// <summary>
        /// Get Parent Department Categories
        /// </summary>
        /// <param name="context"></param>
        private static void GetParentDepartmentCategories(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string parentDepartement = string.Empty;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                if (Request.Params["ParentDepartement"] != null)
                {
                    parentDepartement = Request.Params["ParentDepartement"].ToString();
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetParentDepartmentCategories(bizUnit, parentDepartement));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }

        /// <summary>
        /// Get Parent Department Categories
        /// </summary>
        /// <param name="context"></param>
        private static void GetParentDepartmentCategoriesByID(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string parentDepartement = string.Empty;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                if (Request.Params["ParentDepartement"] != null)
                {
                    parentDepartement = Request.Params["ParentDepartement"].ToString();
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetParentDepartmentCategoriesByID(bizUnit, parentDepartement));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }

        /// <summary>
        /// Get Department List Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetDepartmentList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetDepartment());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Country Name To Fill DropDown
        /// </summary>
        /// <param name="context"></param>
        private static void GetCountryList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCountry());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Project Autocomplete
        /// </summary>
        /// <param name="context"></param>
        private static void GetProjectListAuto(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string srhcType = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                string srchBy = "prjName";
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetProjectListAuto(srchBy, srhcType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        //
        private static void GetSBU(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 1;
            short active = 1;
            int apsPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetSBU(0));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Company List
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompany(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 1;
            short active = 1;
            int apsPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["Active"] != null)
                {
                    active = Convert.ToInt16(Request.Params["Active"].ToString());
                }
                if (Request.Params["ApsPK"] != null)
                {
                    apsPK = Convert.ToInt32(Request.Params["ApsPK"].ToString());
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCompany(apsPK, active, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Company List
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompanyAutoComplete(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 1;
            short active = 1;
            int apsPK = 0;
            string SplCond = string.Empty;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["Active"] != null)
                {
                    active = Convert.ToInt16(Request.Params["Active"].ToString());
                }
                if (Request.Params["ApsPK"] != null)
                {
                    apsPK = Convert.ToInt32(Request.Params["ApsPK"].ToString());
                }
                if (Request.Params["SplCond"] != null)
                {
                    SplCond = Request.Params["SplCond"].ToString();
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCompanyAutoComplete(apsPK, active, bizUnit, SplCond));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void CommonDDL(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int group = 9;
            short active = 1;
            int apsPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["Group"] != null)
                {
                    group = Convert.ToInt32(Request.Params["Group"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCommonDDL(group));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetAccount(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int accountPK = 0;
            short active = 1;
            int subType = 1;
            int isGroup = 0;
            int isIncludeAccCode = 0;
         
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["AccountPK"] != null)
                {
                    accountPK = Convert.ToInt32(Request.Params["AccountPK"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
               // bizUnit = objUser.SBUID

                if (Request.Params["Active"] != null)
                {
                    active = Convert.ToInt16(Request.Params["Active"].ToString());
                }
                if (Request.Params["SubType"] != null)
                {
                    subType = Convert.ToInt32(Request.Params["SubType"].ToString());
                }
                if (Request.Params["IsGroup"] != null)
                {
                    isGroup = Convert.ToInt32(Request.Params["IsGroup"].ToString());
                }
                if (Request.Params["IsIncludeAccCode"] != null)
                {
                    isIncludeAccCode = Convert.ToInt32(Request.Params["IsIncludeAccCode"].ToString());
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetAccount(accountPK, active, subType, isGroup, isIncludeAccCode, objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetAccountWithCode(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int accountPK = 0;
            short active = 1;
            int subType = 1;
            int isGroup = 0;
            int isIncludeAccCode = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["AccountPK"] != null)
                {
                    accountPK = Convert.ToInt32(Request.Params["AccountPK"]);
                }
                if (Request.Params["Active"] != null)
                {
                    active = Convert.ToInt16(Request.Params["Active"].ToString());
                }
                if (Request.Params["SubType"] != null)
                {
                    subType = Convert.ToInt32(Request.Params["SubType"].ToString());
                }
                if (Request.Params["IsGroup"] != null)
                {
                    isGroup = Convert.ToInt32(Request.Params["IsGroup"].ToString());
                }
                if (Request.Params["IsIncludeAccCode"] != null)
                {
                    isIncludeAccCode = Convert.ToInt32(Request.Params["IsIncludeAccCode"].ToString());
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetAccountWithCode(accountPK, active, subType, isGroup, isIncludeAccCode, objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get State name to Fill DropDown
        /// </summary>
        /// <param name="context"></param>
        private static void GetStateList(HttpContext context)
        {
            int countryID = 0;
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["CountryID"] != null)
                {
                    if (Request.Params["CountryID"].ToString() != "null")
                    {
                        countryID = Convert.ToInt32(Request.Params["CountryID"].ToString());
                    }
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetState(countryID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Currency name to Fill DropDown
        /// </summary>
        /// <param name="context"></param>
        private static void GetCurrencyList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                if (Request.Params["SBU"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["SBU"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL

                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCurrency(objUser, bizUnit));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        // 05042011

        /// <summary>
        /// Get CountryName For AutoComplete
        /// </summary>
        /// <param name="context"></param>
        private static void GetCountryAutoComplete(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string countryName = string.Empty;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);


                if (Request.QueryString["SearchValue"] != null)
                {
                    countryName = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }

                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCountryAutoComplete(countryName));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// To Search StateName as AutoComplete
        /// </summary>
        /// <param name="context"></param>
        private static void GetStateAutoComplete(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string stateName = string.Empty;
            int country = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);


                if (Request.QueryString["SearchValue"] != null)
                {
                    stateName = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                if (Request.QueryString["Country"] != null)
                {
                    country = Convert.ToInt32(Request.QueryString["Country"].ToString());
                }
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetStateAutoComplete(stateName, country));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Currency Name For AutoComplete Search
        /// </summary>
        /// <param name="context"></param>
        private static void GetCurrencyAutoComplete(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string currencyName = string.Empty;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);


                if (Request.QueryString["SearchValue"] != null)
                {
                    currencyName = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }

                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCurrencyAutoComplete(currencyName));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Methode used to get menu auto complete 
        /// </summary>
        /// <param name="context"></param>
        private static void GetMenuAutoComplete(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string menuName = string.Empty;
            int userPK = 0;
            int bizUnit = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                menuName = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : "%%";
                bizUnit = Request.QueryString["BizUnit"] != null ? Convert.ToInt32(Request.QueryString["BizUnit"].ToString()) : 0;
                userPK = Request.QueryString["UserPK"] != null ? Convert.ToInt32(Request.QueryString["UserPK"].ToString()) : 0;
                List<MenuType> lstMenu = new List<MenuType>() { new MenuType() { PK = 2 } };
                XmlDocument xDoc = CommonFunctions.ObjectTOXml(lstMenu);
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetMenuAutoComplete(menuName, userPK, bizUnit, xDoc.InnerXml));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// function used to Update Reference id to Store adjustment and vendor Evaluation
        /// </summary>
        /// <param name="context"></param>
        private static void UpdateReferenceID(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int type = 0;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string workFlowDetails = GetRequestString(context);
                if (Request.QueryString["Type"] != null)
                {
                    type = Convert.ToInt32(Request.QueryString["Type"].ToString());
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // for Store Adjustment
                if (type == 1)
                {
                    Response.Write(BusinessLogic.StoreManagement.StockAdjustment.UpdateEvaluationRefID(workFlowDetails, objUser).ToString());
                }
                // for vendor evaluation
                else
                {
                    Response.Write(BusinessLogic.VendorManagement.VendorEvaluation.UpdateEvaluationRefID(workFlowDetails, objUser).ToString());
                }
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.Write("0");
            }
        }

        /// <summary>
        /// Method to Check Inventory locking date
        /// </summary>
        /// <param name="context"></param>
        private static void CheckInventoryLocking(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            DateTime transactionDate;
            int bizUnit = 0;
            string LockUptoDate = string.Empty;
            string result;
            int module = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                transactionDate = Request.Params["Date"] != null ? DateTime.Parse(Request.Params["Date"]) : DateTime.Now;
                module = Request.Params["Module"] != null ? Convert.ToInt16(Request.Params["Module"]) : 0;
                bizUnit = objUser.SBUID;
                System.Collections.Generic.List<object> retvals = new System.Collections.Generic.List<object>();
                result = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(transactionDate, bizUnit, module, ref LockUptoDate);
                retvals.Add(result);
                if (LockUptoDate != "")
                    retvals.Add(Convert.ToDateTime(LockUptoDate).ToString("dd/MMM/yyyy"));
                string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
                Response.Write(jString);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Inspection Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method to Get Item UOMConversionFactor
        /// </summary>
        /// <param name="context"></param>
        private static void GetItemUOMConversionFactor(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string result;
            int fromUomPK = 0;
            int toUomPK = 0;
            int itemPk = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                fromUomPK = Request.Params["FromUOM"] != null ? Convert.ToInt16(Request.Params["FromUOM"]) : 0;
                toUomPK = Request.Params["ToUOM"] != null ? Convert.ToInt16(Request.Params["ToUOM"]) : 0;
                itemPk = Request.Params["ItemID"] != null ? Convert.ToInt16(Request.Params["ItemID"]) : 0;

                result = BusinessLogic.CommonManagement.CommonBL.GetUOMConversionFactor(itemPk, fromUomPK, toUomPK).ToString();
                string jString = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                Response.Write(jString);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("GetItemUOMConversionFactor");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Company/Plant List w. r. to current dept
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompanyMappingDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 1;
            short active = 1;
            int apsPK = 0;
            int deptPk = 0;            
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["Active"] != null)
                {
                    active = Convert.ToInt16(Request.Params["Active"].ToString());
                }
                if (Request.Params["ApsPK"] != null &&  Request.Params["ApsPK"] !="null")
                {
                    apsPK = Convert.ToInt32(Request.Params["ApsPK"].ToString());
                }
                if (Request.Params["DeptPk"] != null)
                {
                    deptPk = Convert.ToInt32(Request.Params["DeptPk"].ToString());
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCompanyMappingDetails(apsPK, active, bizUnit, deptPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// To Bind Company/Plant Display Name List w. r. to current dept
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompanyDisplayNames(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 1;
            short active = 1;
            int apsPK = 0;
            int deptPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["Active"] != null)
                {
                    active = Convert.ToInt16(Request.Params["Active"].ToString());
                }
                if (Request.Params["ApsPK"] != null)
                {
                    apsPK = Convert.ToInt32(Request.Params["ApsPK"].ToString());
                }
                if (Request.Params["DeptPk"] != null)
                {
                    deptPk = Convert.ToInt32(Request.Params["DeptPk"].ToString());
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetCompanyDisplayNames(apsPK, active, bizUnit, deptPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Account types for Item category
        /// </summary>
        /// <param name="context"></param>
        private static void GetAccountTypesforMapping(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 1;
            string CfgType = string.Empty;
            int CfgPK = 0;
            string SplCond = string.Empty;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["CfgType"] != null)
                {
                    CfgType = Request.Params["CfgType"].ToString();
                }
                if (Request.Params["CfgPK"] != null)
                {
                    CfgPK = Convert.ToInt32(Request.Params["CfgPK"].ToString());
                }
                if (Request.Params["SplCond"] != null)
                {
                    SplCond = Request.Params["SplCond"].ToString();
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetAccountTypesforMapping(bizUnit, CfgType,SplCond));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Country Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        #endregion
        private static void PoCreatorDDL1(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int Pk = 0;
            int bizUnit = 0;
            int active = 1;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }

                //if (Request.Params["DeptPk"] != null && Request.Params["DeptPk"] != "null")
                //{
                //    int.TryParse(Request.Params["DeptPk"], out Pk);
                //    // Pk = Convert.ToInt32(Request.Params["DeptPk"]);
                //}
                //if (Request.Params["ParentDeptPk"] != null && Request.Params["ParentDeptPk"] != "null")
                //{
                //    int.TryParse(Request.Params["ParentDeptPk"], out ParentPk);
                //    //ParentPk = Convert.ToInt32(Request.Params["ParentDeptPk"]);
                //}

                // Get the list in json string format from BL
                //Response.Write(BusinessLogic.CommonManagement.CommonManagement.PoCreatorDDL(objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetEmployeeCreatorList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);


                if (Request.Params["SBU"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["SBU"]);
                }

                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetPoCreators(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
