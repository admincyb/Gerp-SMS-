using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using BusinessLogic.PurchaseOrderManagement;
using BusinessLogic.CommonManagement;
using GTIService;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        private static void PurchaseOrderManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SavePurchaseOrder":
                        Handlers.SavePurchaseOrderGenerate(context);
                        break;
                    case "SavePurchaseOrderWkf":
                        Handlers.SavePurchaseOrderGenerateWkf(context);
                        break;
                    case "GetPendingPurchaseRequest":
                        Handlers.GetPendingPurchaseRequest(context);
                        break;
                    case "GetPurchaseRequestItemVendor":
                        Handlers.GetPurchaseRequestItemVendor(context);
                        break;
                    case "GetPOVendor":
                        Handlers.GetPOVendor(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GePurchaseOrderSearchValue(context);
                        break;
                    case "GetPOType":
                        Handlers.GetConfigMaster(context);
                        break;
                    case "GetPOCategory":
                        Handlers.GetConfigMaster(context);
                        break;
                    case "GetVendorRates":
                        Handlers.GetVendorRates(context);
                        break;
                    case "GetGrnQty":
                        Handlers.GetGrnQty(context);
                        break;
                    case "GetExchangeRate":
                        Handlers.GetExchangeRate(context);
                        break;
                    case "GetRevisionHistory":
                        Handlers.GetRevisionHistory(context);
                        break;
                    case "GetPurchaseOrderMapPR":
                        Handlers.GetPurchaseOrderMapPR(context);//To get PUR_ORDER_REQ_MAP key values.  
                        break;
                    case "GetPurchaseOrderTypes":
                        Handlers.GetPurchaseOrderTypes(context);
                        break;
                 
                    case "SavePurchaseOrderTrading":
                        Handlers.SavePurchaseOrderGenerateTrading(context);
                        break;
                    case "GetPurchaseOrderProjectBudget":
                        Handlers.GetPurchaseOrderProjectBudget(context);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void SavePurchaseOrderGenerate(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string pODetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string poID = PurchaseOrderGenerateBL.SavePODetails(pODetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(poID);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Order Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void SavePurchaseOrderGenerateWkf(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string pODetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string poID = PurchaseOrderGenerateBL.SavePODetailsWkf(pODetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(poID);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Order Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetConfigMaster(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string CfgType = Request.Params["CfgType"] != null ? Request.Params["CfgType"].ToString() : string.Empty;
                int active = Request.Params["Active"] != null ? int.Parse(Request.Params["Active"]) : 1;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(PurchaseOrderGenerateBL.GetConfigMaster(objUser, CfgType, active));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseOrderTypes(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int active = Request.Params["Active"] != null ? int.Parse(Request.Params["Active"]) : 1;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(PurchaseOrderGenerateBL.GetPurchaseOrderTypes(objUser, active));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GePurchaseOrderSearchValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                int vendor = Request.Params["Vendor"] != null ? int.Parse(Request.Params["Vendor"]) : 0;
                int poPK = Request.Params["POPK"] != null ? int.Parse(Request.Params["POPK"]) : 0;
                int DeptPK = Request.Params["DeptPK"] != null ? int.Parse(Request.Params["DeptPK"]) : 0;
                int processPK = Request.Params["ProcessID"] != null ? int.Parse(Request.Params["ProcessID"]) : 0;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(PurchaseOrderGenerateBL.GetSearchValues(searchBy, searchValue, objUser, vendor, poPK, DeptPK,processPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseRequestItemVendor(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int poID = Request.Params["POID"] != null ? int.Parse(Request.Params["POID"]) : 0;
                string selectedPRItems = GetRequestString(context);
                    //Request.Params["PRItems"] != null ? Convert.ToString(Request.Params["PRItems"]) : "";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(PurchaseOrderGenerateBL.GetPurchaseRequestItemVendor(poID, objUser.SBUID, selectedPRItems));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


       /// <summary>
       /// 
       /// </summary>
       /// <param name="context"></param>
        private static void GetPOVendor(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int poID = Request.Params["POID"] != null ? int.Parse(Request.Params["POID"]) : 0;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(PurchaseOrderGenerateBL.GetPOVendor(poID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetExchangeRate(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int fromCurr = Request.Params["fromCurrency"] != null ? int.Parse(Request.Params["fromCurrency"]) : 0;
                int toCurr = Request.Params["toCurrency"] != null ? int.Parse(Request.Params["toCurrency"]) : 0;
                string strDate = Request.Params["date"];
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(CommonBL.GetExchangeRate(fromCurr,toCurr,strDate));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
  
        private static void GetVendorRates(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int vendorId = Request.Params["VendorId"] != null ? int.Parse(Request.Params["VendorId"]) : 0;
                string selectedItems = GetRequestString(context);
                    //Request.Params["Items"] != null ? Convert.ToString(Request.Params["Items"]) : "";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(PurchaseOrderGenerateBL.GetVendorRates(vendorId, selectedItems));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        //Grn qty against an item
        private static void GetGrnQty(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int podPK = Request.Params["podPK"] != null ? int.Parse(Request.Params["podPK"]) : 0;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(PurchaseOrderGenerateBL.GetGrnQty(podPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

       /// <summary>
       /// Get Revision History
       /// </summary>
       /// <param name="context"></param>
        private static void GetRevisionHistory(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int pohPK = Request.Params["PohPK"] != null ? int.Parse(Request.Params["PohPK"]) : 0;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(PurchaseOrderGenerateBL.GetRevisionHistory(pohPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPendingPurchaseRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int poID = Request.Params["POID"] != null ? int.Parse(Request.Params["POID"]) : 0;
                int vendor = Request.Params["Vendor"] != null ? int.Parse(Request.Params["Vendor"]) : 0;
                string sortBy = Request.Params["SortBy"] != null ? Request.Params["SortBy"] : "";
                int applicationPK = Request.Params["ApplicationPK"] != null ? int.Parse(Request.Params["ApplicationPK"]) : 0;
                int processId = Request.Params["ProcessID"] != null ? int.Parse(Request.Params["ProcessID"]) : 0;
                int type = Request.Params["TYPE"] != null && Request.Params["TYPE"] != "null" ? int.Parse(Request.Params["TYPE"]) : 0;
                
                Response.Write(PurchaseOrderGenerateBL.GetPendingPurchaseRequest(CommonFunctions.GetGridParams(Request), objUser.SBUID, vendor, poID, objUser.PKUser, applicationPK, sortBy, processId, type));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Order Generate");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseOrderMapPR(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int POD_PK = Request.Params["POD_PK"] != null ? int.Parse(Request.Params["POD_PK"]) : 0;
                Response.Write(PurchaseOrderGenerateBL.GetPurchaseOrderMapPR(objUser.SBUID, POD_PK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Order Generate");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


       #region Trading
		 /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void SavePurchaseOrderGenerateTrading(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string pODetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string poID = PurchaseOrderGenerateBL.SavePODetailsTrading(pODetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(poID);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Order Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }
        }
        #endregion

        private static void GetPurchaseOrderProjectBudget(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string Investor = Request.Params["POH_INVESTOR_CODE"] != null ? Request.Params["POH_INVESTOR_CODE"].ToString() : string.Empty;
                int CurrencyPK = Request.Params["POH_CURRENCY"] != null ? int.Parse(Request.Params["POH_CURRENCY"]) : 0;
                string PODate = Request.Params["POH_DATE"] != null ? Request.Params["POH_DATE"].ToString() : string.Empty;
                Response.Write(PurchaseOrderGenerateBL.GetPurchaseOrderProjectBudget(Investor, CurrencyPK, PODate));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Order Generate");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
