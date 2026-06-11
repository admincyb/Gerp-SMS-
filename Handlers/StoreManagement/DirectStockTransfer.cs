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
        private static void DirectStockTransferManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "GetPendingDirectGRNSearchAuto":
                        Handlers.GetPendingDirectGRNSearchAuto(context);
                        break;
                    case "GetPendingWOGRNSearchAuto":
                        Handlers.GetPendingWOGRNSearchAuto(context);
                        break;
                    case "GetPlantCodes":
                        Handlers.GetPlantCodes(context);
                        break;   
                }
            }
        }

        private static void GetPendingWOGRNSearchAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                int vendorPK = Request.Params["SearchCorr"] != null ? int.Parse(Request.Params["SearchCorr"]) : 0;
                int grnPK = Request.Params["SearchCorr1"] != null ? int.Parse(Request.Params["SearchCorr1"]) : 0;
                int deptPk = Request.Params["DeptPk"] != null ? int.Parse(Request.Params["DeptPk"]) : 0;
                int PendingWo = Request.Params["IsPendingWO"] != null ? int.Parse(Request.Params["IsPendingWO"]) : 1;
                Response.Write(BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetPendingWOGRNSearchAuto(searchBy, searchValue, vendorPK, grnPK, objUser, deptPk, PendingWo));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPendingDirectGRNSearchAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                int vendorPK = Request.Params["SearchCorr"] != null ? int.Parse(Request.Params["SearchCorr"]) : 0;
                int grnPK = Request.Params["SearchCorr1"] != null ? int.Parse(Request.Params["SearchCorr1"]) : 0;
                int deptPk = Request.Params["DeptPk"] != null ? int.Parse(Request.Params["DeptPk"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetPendingDirectGRNSearchAuto(searchBy, searchValue, vendorPK, grnPK, objUser, deptPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// For getting Company Display Names
        /// </summary>
        /// <param name="searchKey">
        /// </param>
        private static void GetPlantCodes(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string fieldName = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                Response.Write(BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetDirectGRNAutocomplete(fieldName, searchValue, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
