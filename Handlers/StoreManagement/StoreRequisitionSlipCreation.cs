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
        /// handiling StoreRequisitionSlipCreation handelers.
        /// </summary>
        /// <param name="context"></param>
        private static void StoreRequisitionSlip(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {

                   case "SaveRequisition":
                        Handlers.SaveRequisition(context);
                        break;
                   case "GetSearchValue":
                        Handlers.GetStoreReqSearchValue(context);
                        break;
                   case "GetStores":
                        Handlers.GetStores(context);
                        break;
                   case "GetStoresFilterByCategory":
                        Handlers.GetStoresFilterByCategory(context);
                        break;
                   case "GetSRSNo":
                        Handlers.GetSRSNo(context);
                        break;
                   case "GetSRSDetails":
                        Handlers.GetSRSDetails(context);
                        break;

                   case "GetRequisitionSearchValue":
                        Handlers.GetRequisitionSearchValue(context);
                        break;
                   case "GetRequisitionList":
                        Handlers.GetRequisitionList(context);
                        break;
                   case "DeleteRequisition":
                        Handlers.DeleteRequisition(context);
                        break;

                   case "GetStoresByType":
                        Handlers.GetStoresByType(context);
                        break;
                   case "GetDepartmentDtls":
                        Handlers.GetUserDeparmentDtls(context);
                        break;
                    case "GetOtherSBUS":
                        Handlers.FillOtherSBUS(context);
                        break;

                }
            }
        }
        /// <summary>
        /// save Requisition Details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveRequisition(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
             string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string requisitionDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string requisition = BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.SaveRequisitionDetails(requisitionDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(requisition);


            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }

        }
        /// <summary>
        /// Search item name Details , and Get Data Related to code
        /// </summary>
        /// <param name="context"></param>
        private static void GetStoreReqSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

               
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }

                Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetSearchValues(searchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Stores Name
        /// </summary>
        /// <param name="context"></param>
        private static void GetStores(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
           // string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            int sbuPk = 0;
            int flag = 0;
            try
            {
                // clear all the response
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["Flag"] != null)
                {
                    flag = Convert.ToInt32(Request.Params["Flag"]);
                }
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetStores(objUser, sbuPk, flag));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Stores Name
        /// </summary>
        /// <param name="context"></param>
        private static void GetStoresFilterByCategory(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            // string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            int sbuPk = 0;
            int flag = 0;
            int category = 0;
            try
            {
                // clear all the response
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["Flag"] != null)
                {
                    flag = Convert.ToInt32(Request.Params["Flag"]);
                }
                if (Request.Params["Category"] != null)
                {
                    category = Convert.ToInt32(Request.Params["Category"]);
                }
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetStores(objUser, sbuPk, flag, category));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get SRS No
        /// </summary>
        /// <param name="context"></param>
        private static void GetSRSNo(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetSRSNo());
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Requisition List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetRequisitionList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int procID = 0;
            string PageUrl = "";
            int DeptPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["PageUrl"] != null)
                {
                    PageUrl = Request.Params["PageUrl"];
                }
                if (Request.Params["DeptPK"] != null)
                {
                    DeptPK = Convert.ToInt32(Request.Params["DeptPK"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipList.GetRequisitionList(CommonFunctions.GetGridParams(Request), bizUnit, objUser, procID, PageUrl,DeptPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Requisition Slip list");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Search Requisition Details , and Get Data Related to Search Criteria
        /// </summary>
        /// <param name="context"></param>
        private static void GetRequisitionSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int sbuPk = 0;
            int procID = 0;
            
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["ProcID"] != null)
                {
                    procID =Convert.ToInt32( Request.Params["ProcID"].ToString());
                }
                if (Request.Params["SearchType"] != null)
                {
                    searchBy = Request.Params["SearchType"].ToString();
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                if (Request.QueryString["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.QueryString["SBUPk"].ToString());
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipList.GetSearchValues(searchBy, searchValue, sbuPk, objUser, procID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip List");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Method Used to Delete Requisition Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteRequisition(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int requisitionID = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                if (Request.Params["RequisitionID"] != null)
                {
                    // Assign requisitionID From Request to requisitionID variable
                    requisitionID = Convert.ToInt32((Request.Params["RequisitionID"].Trim()));

                }

                Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipList.DeleteRequisition(requisitionID));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip List");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Method Used to get Requisition Detials refered by rijoy in MA.
        /// </summary>
        /// <param name="context"></param>
        private static void GetSRSDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int srsID = Request.Params["SRSPK"] != null ? int.Parse(Request.Params["SRSPK"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetSRSDetails(srsID));
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
        private static void GetStoresByType(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            // string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            int sbuPk = 0;
            int deptType = 0;
            int deptPk=0;
            int deptCompany = 0;
            try
            {
                // clear all the response
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
               
                if (Request.Params["DeptType"] != null)
                {
                    deptType = Convert.ToInt32(Request.Params["DeptType"]);
                }
                if (Request.Params["DeptCompany"] != null)
                {
                    deptCompany = Convert.ToInt32(Request.Params["DeptCompany"]);
                }
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetStoresByType(objUser, sbuPk, deptType, deptPk, deptCompany));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get  Department Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetUserDeparmentDtls(HttpContext context)
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
                Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetDepartmentDtls(objUser, sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        private static void FillOtherSBUS(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int Active = 0;
            //  string userPK = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                if (Request.Params["Active"] != null)
                {
                    Active = Convert.ToInt32(Request.Params["Active"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));          
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.FillOtherSBUS(objUser, Active));
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
