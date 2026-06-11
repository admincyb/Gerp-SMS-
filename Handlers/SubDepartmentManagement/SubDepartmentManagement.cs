using System;
using System.Web;
using System.Web.SessionState;
using GTIService;
namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        #region Methods

        /// <summary>
        /// handiling Sub Department management handlers.
        /// </summary>
        /// <param name="context"></param>
        private static void SubDepartmentManagement(HttpContext context)
        {
          
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "GetDepartmentDtls":
                        Handlers.GetDeparmentDtls(context);
                        break;
                    case "GetDeparmentsBySBU":
                        Handlers.GetDeparmentsBySBU(context);
                        break;
                    case "GetNonStoreDept":
                        Handlers.GetNonStoreDeptDeparment(context);
                        break;
                    case "GetNonStoreDeptUserSwitch":
                        Handlers.GetNonStoreDeptUserSwitch(context);
                        break;
                    case "GetUserDepartments":
                        Handlers.GetUserDepartments(context);
                        break;
                    case "GetSubDeptDtlsByID":  // by ID
                        Handlers.GetSubDeptDtls(context);
                        break;

                    case "GetSubDeptDtlsList": // List
                        Handlers.GetSubDepartmentList(context);
                        break;

                    case "DeleteSubDeptDtls":
                        Handlers.DeleteSubDeptDtls(context);
                        break;

                    case "SavePage":
                        Handlers.SaveSubDeptDtls(context);
                        break;

                    case "GetSearchValue":
                        Handlers.GetSubDeptSearchValue(context);
                        break;
                    //Vineeth For Po Generation
                    case "GetInvDepartment":
                        Handlers.GetInvDepartment(context);
                        break;
                    case "GetInventoryStores":
                        Handlers.GetInventoryStores(context);
                        break;
                    case "GetIssuingStores":
                        Handlers.GetIssuingStores(context);
                        break;
                    // For Material Issue, Accept, SRS
                    case "GetStoresByType":
                        Handlers.GetStoresDetailsByType(context);
                        break;

                    case "GetAllStores":
                        Handlers.GetAllStoreDetails(context);
                        break;

                    case "GetGeneralStores":
                        Handlers.GetGeneralStores(context);
                        break;

                    #region GetInventoryStoresBasedOnConfig
                    case "GetInventoryStoresBasedOnConfig":
                        Handlers.GetInventoryStoresBasedOnConfig(context);
                        break; 
                    #endregion
                }
            }
        }

        /// <summary>
        /// Get  Department Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetDeparmentDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
          //  string userPK = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));           
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetDepartmentDtls(objUser, sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get  Department Details bySBU
        /// </summary>
        /// <param name="context"></param>
        private static void GetDeparmentsBySBU(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
            //  string userPK = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetDepartmentDtls(sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get non store  Department Details GetNonStoreDeptUserSwitch
        /// </summary>
        /// <param name="context"></param>
        private static void GetNonStoreDeptDeparment(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int sbuPk = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetNonStoreDeptDeparment(objUser, sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get non store  Department Details 
        /// </summary>
        /// <param name="context"></param>
        private static void GetNonStoreDeptUserSwitch(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int sbuPk = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetNonStoreDeptUserSwitch( sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get  Department Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetUserDepartments(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
            int baseDpt = 0;
            //  string userPK = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["BaseDpt"] != null)
                {
                    baseDpt = Convert.ToInt32(Request.Params["BaseDpt"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetDepartmentDtls(objUser, sbuPk, baseDpt));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
       /// Get Sub Dept Details By Sub Dept ID
       /// </summary>
       /// <param name="context"></param>
        private static void GetSubDeptDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int subDeptID = 0;
            
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SubDeptID"] != null)
                {
                    subDeptID = Convert.ToInt32(Request.Params["SubDeptID"]);
                }
               
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetSubDeptDetailsBySubDeptID(subDeptID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }

        /// <summary>
        /// Get Sub Department List 
        /// </summary>
        /// <param name="context"></param>
        private static void GetSubDepartmentList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int Status = 1;
            try
            {
                bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                Status = Convert.ToInt32(Request.Params["ActiveStatus"]);
                
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetSubDeptList(CommonFunctions.GetGridParams(Request), bizUnit, Status));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Designation Details Search value
        /// </summary>
        /// <param name="context"></param>
        private static void GetSubDeptSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                if (Request.Params["SearchType"] != null)
                {
                    searchBy = Request.Params["SearchType"].ToString();
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                if (Request.QueryString["SBU"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.QueryString["SBU"].ToString());
                }

                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetSearchValues(searchBy, searchValue, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Designation Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
       /// Save Sub Dept Details
       /// </summary>
       /// <param name="context"></param>
        private static void SaveSubDeptDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.SaveSuDepartment(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
       /// Delete Sub Department Details By Sub Dept Pk
       /// </summary>
       /// <param name="context"></param>
        private static void DeleteSubDeptDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int subDeptID = 0;
            try
            {
                if (Request.Params["SubDeptID"] != null)
                {
                    subDeptID = Convert.ToInt32((Request.Params["SubDeptID"].Trim()));

                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.DeleteSubDepartment(subDeptID));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Get  Department Details
        /// </summary>
        /// <Created By>Vineeth</Created>
        /// <For>PO Dept Filling</For>
        /// <Used In>PoDepFilling,FillSelectedDepartDetails </Used>
        /// <param name="context"></param>
        private static void GetInvDepartment(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int DeptID = 0;
            BusinessObject.User objuser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (Request.Params["DeptID"] != null)
                DeptID = Convert.ToInt32(Request.Params["DeptID"].ToString());
            int BIZUNIT = objuser.SBUID;
            try
            {

                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetInvDepartment(BIZUNIT,DeptID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Stores Name By Type
        /// </summary>
        /// <param name="context"></param>
        private static void GetInventoryStores(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int sbuPk = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                int deptType = Request.Params["DeptType"] != null ? Convert.ToInt32(Request.Params["DeptType"]) : 0;
                int userPK = Request.Params["UserPK"] != null ? Convert.ToInt32(Request.Params["UserPK"]) : 0;
                int deptCompany = Request.Params["DeptCompany"] != null ? Convert.ToInt32(Request.Params["DeptCompany"]) : 0;
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetInventoryStores(sbuPk, deptType, userPK,deptCompany));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetIssuingStores(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int sbuPk = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                int deptType = Request.Params["DeptType"] != null ? Convert.ToInt32(Request.Params["DeptType"]) : 0;
                int userPK = Request.Params["UserPK"] != null ? Convert.ToInt32(Request.Params["UserPK"]) : 0;
                int deptCompany = Request.Params["DeptCompany"] != null ? Convert.ToInt32(Request.Params["DeptCompany"]) : 0;
                int MaterialPK = Request.Params["MaterialPK"] != null ? Convert.ToInt32(Request.Params["MaterialPK"]) : 0;
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetIssuingStores(sbuPk, deptType, userPK, deptCompany, MaterialPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Stores Name By Type
        /// </summary>
        /// <param name="context"></param>
        private static void GetStoresDetailsByType(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            // string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            int deptPk = 0;
            int deptType = 0;
            int sbuPk = 0;
            int flag = 0;
            int MenuType = 0;
            try
            {
                // clear all the response
                if (Request.Params["DeptPk"] != null)
                {
                    deptPk = Convert.ToInt32(Request.Params["DeptPk"]);
                }
                if (Request.Params["DeptType"] != null)
                {
                    deptType = Convert.ToInt32(Request.Params["DeptType"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["UserFlag"] != null)
                {
                    flag = Convert.ToInt32(Request.Params["UserFlag"]);
                }
                if (Request.Params["MenuType"] != null)
                {
                    MenuType = Convert.ToInt32(Request.Params["MenuType"]);
                }

                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetStoresByType(deptPk, deptType, objUser, sbuPk,  flag, MenuType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Sub Department");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Stores Name By Type
        /// </summary>
        /// <param name="context"></param>
        private static void GetGeneralStores(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            // string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            int deptPk = 0;
            int deptType = 0;
            int sbuPk = 0;
            int flag = 0;
            int deptChild = 0;
            int deptCompany = 0;
            try
            {
                // clear all the response
                if (Request.Params["DeptPk"] != null)
                {
                    deptPk = Convert.ToInt32(Request.Params["DeptPk"]);
                }
                if (Request.Params["DeptType"] != null)
                {
                    deptType = Convert.ToInt32(Request.Params["DeptType"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["UserFlag"] != null)
                {
                    flag = Convert.ToInt32(Request.Params["UserFlag"]);
                }
                if (Request.Params["DeptChild"] != null)
                {
                    deptChild = Convert.ToInt32(Request.Params["DeptChild"]);
                }
                if (Request.Params["DeptCompany"] != null)
                {
                    deptCompany = Convert.ToInt32(Request.Params["DeptCompany"]);
                }
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetGeneralStores(deptPk, deptType, objUser, sbuPk, flag, deptChild, deptCompany));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Sub Department");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Stores Name For Store Audit
        /// </summary>
        /// <param name="context"></param>
        private static void GetAllStoreDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            // string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            int deptPk = 0;
            int deptType = 0;
            int sbuPk = 0;
            int flag = 0;
            int deptCatg = 0;
            try
            {
                // clear all the response
                if (Request.Params["DeptPk"] != null)
                {
                    deptPk = Convert.ToInt32(Request.Params["DeptPk"]);
                }
                if (Request.Params["DeptType"] != null)
                {
                    deptType = Convert.ToInt32(Request.Params["DeptType"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["UserFlag"] != null)
                {
                    flag = Convert.ToInt32(Request.Params["UserFlag"]);
                }
                if (Request.Params["DeptCatg"] != null)
                {
                    deptCatg = Convert.ToInt32(Request.Params["DeptCatg"]);
                }
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetAllStoresDetails(deptPk, deptType, objUser, sbuPk, flag, deptCatg));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Sub Department");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Stores Name Based on Configuration(	[ACF_SETTING]	= 'WKF SETTING' AND [ACF_DATA]= 'PurReqDept' )						
        /// </summary>
        /// <param name="context"></param>
        private static void GetInventoryStoresBasedOnConfig(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string fieldName = Request.Params["FLD_NAME"] != null ? Convert.ToString(Request.Params["FLD_NAME"]) : string.Empty;
                int sbuPk = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;              
                int userPK = Request.Params["UserPK"] != null ? Convert.ToInt32(Request.Params["UserPK"]) : 0;              
                int procID = Request.Params["ProcID"] != null ? Convert.ToInt32(Request.Params["ProcID"]) : 0;
             
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetInventoryStoresBasedOnConfig(fieldName, sbuPk,userPK, procID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        #endregion
    }
}
