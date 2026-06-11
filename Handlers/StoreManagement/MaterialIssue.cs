using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using GTIService;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        private static void MaterialIssue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "GetMRPending":
                        Handlers.GetMRPending(context);
                        break;
                    case "SaveMaterialIssue":
                        Handlers.SaveMaterialIssue(context);
                        break;
                    case "SaveMaterialIssueWkf":
                        Handlers.SaveMaterialIssueWkf(context);
                        break;
                    case "SaveMRIssue":
                        Handlers.SaveMRIssue(context);
                        break;
                    case "SaveMRIssuePlantToPlant":
                        Handlers.SaveMRIssuePlantToPlant(context);
                        break;
                    case "GetPendingSearchAuto":
                        Handlers.GetPendingSRSSearchAuto(context);
                        break;
                    case "GetPendingWIHSearchAuto":
                        Handlers.GetPendingWIHSearchAuto(context);
                        break;
                    case "GetMIPendingSearchAuto":
                        Handlers.GetMIPendingSearchAuto(context);
                        break;
                    case "GetSRSPending":
                        Handlers.GetSRSPending(context);
                        break;
                    case "GetWOPending":
                        Handlers.GetWOPending(context);
                        break;
                    case "GetPreviousSRSDetailsView":
                        Handlers.GetPreviousSRSDetailsView(context);
                        break;
                    case "GetMIList":
                        Handlers.GetMIList(context);
                        break;
                    case "GetMRIssueList":
                        Handlers.GetMRIssueList(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetMISearchValue(context);
                        break;
                    case "DeleteMaterialIssue":
                        Handlers.DeleteMaterialIssue(context);
                        break;
                    case "DeleteMRIssue":
                        Handlers.DeleteMRIssue(context);
                        break;
                    case "DeleteMRIssuePlantToPlant":
                        Handlers.DeleteMRIssuePlantToPlant(context);
                        break;
                    case "GetAllStores":
                        Handlers.GetAllStores(context);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveMaterialIssue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string materialIssueDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string materialIssue = BusinessLogic.StoreManagement.MaterialIssue.SaveMaterialIssue(materialIssueDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(materialIssue);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
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
        private static void SaveMaterialIssueWkf(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string materialIssueDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string materialIssue = BusinessLogic.StoreManagement.MaterialIssue.SaveMaterialIssueWkf(materialIssueDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(materialIssue);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");
            }
        }
        private static void SaveMRIssue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string materialIssueDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string materialIssue = BusinessLogic.StoreManagement.MaterialIssue.SaveMRIssue(materialIssueDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(materialIssue);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");
            }
        }

        private static void SaveMRIssuePlantToPlant(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string materialIssueDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string materialIssue = BusinessLogic.StoreManagement.MaterialIssue.SaveMRIssuePlantToPlant(materialIssueDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(materialIssue);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");
            }
        }
        private static void GetPendingWIHSearchAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                int searchCorr = Request.Params["SearchCorr"] != null ? int.Parse(Request.Params["SearchCorr"]) : 0;
                int searchCorr1 = Request.Params["SearchCorr1"] != null ? int.Parse(Request.Params["SearchCorr1"]) : 0;
                int PendingWO = Request.Params["IsPendingWO"] != null ? int.Parse(Request.Params["IsPendingWO"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.MaterialIssue.GetPendingWIHSearchAuto(searchBy, searchValue, searchCorr, objUser, searchCorr1,PendingWO));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPendingSRSSearchAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                int searchCorr = Request.Params["SearchCorr"] != null ? int.Parse(Request.Params["SearchCorr"]) : 0;
                int searchCorr1 = Request.Params["SearchCorr1"] != null ? int.Parse(Request.Params["SearchCorr1"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.MaterialIssue.GetPendingSearchAuto(searchBy, searchValue, searchCorr, objUser, searchCorr1));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetMIPendingSearchAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                int searchCorr = Request.Params["SearchCorr"] != null ? int.Parse(Request.Params["SearchCorr"]) : 0;
                int searchCorr1 = Request.Params["SearchCorr1"] != null ? int.Parse(Request.Params["SearchCorr1"]) : 0;
                int dept = Request.Params["Dept"] != null ? int.Parse(Request.Params["Dept"]) : 0;
                int MenuType = Request.Params["MenuType"] != null ? int.Parse(Request.Params["MenuType"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.MaterialIssue.GetMIPendingSearchAuto(searchBy, searchValue, objUser, dept, MenuType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetWOPending(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int store = Request.Params["Store"] != null ? int.Parse(Request.Params["Store"]) : 0;
                int dept = Request.Params["Dept"] != null ? int.Parse(Request.Params["Dept"]) : 0;
                int miPK = Request.Params["miPK"] != null ? int.Parse(Request.Params["miPK"]) : 0;
                int mrhPK = Request.Params["mrhPK"] != null ? int.Parse(Request.Params["mrhPK"]) : 0;
                int PendingWO = Request.Params["IsPendingWO"] != null ? int.Parse(Request.Params["IsPendingWO"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.MaterialIssue.GetWOPending(CommonFunctions.GetGridParams(Request), objUser.SBUID, store, miPK, dept, mrhPK,PendingWO));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Po Details corresponding to sbu
        /// </summary>
        /// <param name="context"></param>
        private static void GetSRSPending(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int store = Request.Params["Store"] != null ? int.Parse(Request.Params["Store"]) : 0;
                int dept = Request.Params["Dept"] != null ? int.Parse(Request.Params["Dept"]) : 0;
                int miPK = Request.Params["miPK"] != null ? int.Parse(Request.Params["miPK"]) : 0;
                int mrhPK = Request.Params["mrhPK"] != null ? int.Parse(Request.Params["mrhPK"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.MaterialIssue.GetSRSPending(CommonFunctions.GetGridParams(Request), objUser.SBUID, store, miPK, dept, mrhPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Po Details corresponding to sbu
        /// </summary>
        /// <param name="context"></param>
        private static void GetMRPending(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int dept = Request.Params["Dept"] != null ? int.Parse(Request.Params["Dept"]) : 0;
                int miPK = Request.Params["miPK"] != null ? int.Parse(Request.Params["miPK"]) : 0;
                int mrhPK = Request.Params["mrhPK"] != null ? int.Parse(Request.Params["mrhPK"]) : 0;
                int MenuType = Request.Params["MenuType"] != null ? int.Parse(Request.Params["MenuType"]) : 0;
                int CurrDept = Request.Params["CurrDept"] != null ? int.Parse(Request.Params["CurrDept"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.MaterialIssue.GetMRPending(CommonFunctions.GetGridParams(Request), objUser.SBUID, miPK, dept, mrhPK, MenuType, CurrDept));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPreviousSRSDetailsView(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int srsID = Request.Params["SRSPK"] != null ? int.Parse(Request.Params["SRSPK"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.MaterialIssue.GetPreviousSRSDetailsView(srsID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetMIList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string pageURL = Request.Params["PageUrl"] != null ? Request.Params["PageUrl"].ToString() : string.Empty;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.MaterialIssue.GetMIList(CommonFunctions.GetGridParams(Request), objUser, pageURL));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetMRIssueList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string pageURL = Request.Params["PageUrl"] != null ? Request.Params["PageUrl"].ToString() : string.Empty;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.MaterialIssue.GetMRIssueList(CommonFunctions.GetGridParams(Request), objUser, pageURL));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Detials For Auto Complete 
        /// </summary>
        /// <param name="context"></param>
        private static void GetMISearchValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.MaterialIssue.GetMISearchValue(searchBy, searchValue, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteMaterialIssue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int miPk = Request.Params["MIPK"] != null ? Convert.ToInt32((Request.Params["MIPK"].Trim())) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);               
                string materialIssue = BusinessLogic.StoreManagement.MaterialIssue.DeleteMaterialIssue(miPk);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(materialIssue);
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        private static void DeleteMRIssue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int miPk = Request.Params["MIPK"] != null ? Convert.ToInt32((Request.Params["MIPK"].Trim())) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);               
                string materialIssue = BusinessLogic.StoreManagement.MaterialIssue.DeleteMRIssue(miPk);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(materialIssue);
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        private static void DeleteMRIssuePlantToPlant(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int miPk = Request.Params["MIPK"] != null ? Convert.ToInt32((Request.Params["MIPK"].Trim())) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);               
                string materialIssue = BusinessLogic.StoreManagement.MaterialIssue.DeleteMRIssuePlantToPlant(miPk);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(materialIssue);
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }


        private static void GetAllStores(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            // string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            int sbuPk = 0;
            int Catg = 0;
            try
            {
                // clear all the response
                if (Request.Params["SBUPK"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPK"]);
                }
                if (Request.Params["Catg"] != null)
                {
                    Catg = Convert.ToInt32(Request.Params["Catg"]);
                }
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.MaterialIssue.GetAllStores(objUser, sbuPk, Catg));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
