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
        private static void GoodsReceiptNoteManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveGoodsReceiptNote":
                        Handlers.SaveGoodsReceiptNote(context);
                        break;
                    case "GetGoodsReceiptNote":
                        Handlers.GetGoodsReceiptNote(context);
                        break;
                    case "GetPendingSearchAuto":
                        Handlers.GetPendingSearchAuto(context);
                        break;
                    case "GetPurchaseOrderPending":
                        Handlers.GetPurchaseOrderPending(context);
                        break;
                    case "GetPrevGRNDetailsView":
                        Handlers.GetPreviousGRNDetailsView(context);
                        break;
                    case "GetGRNList":
                        Handlers.GetGoodsReceiptNoteList(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetGRNSearchValue(context);
                        break;
                    case "DeleteGRN":
                        Handlers.DeleteGoodsReceiptNote(context);
                        break;


                    case "GetGINDetailsView":
                        Handlers.GetGRNDetailsView(context);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveGoodsReceiptNote(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string goodsReceiptNoteDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.StoreManagement.GoodsReceiptNote.SaveGoodsReceiptNote(goodsReceiptNoteDetails, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = GTIService.Constants.Common.ResponseTypes.JSON;
                List<object> retvals = new List<object>();
                retvals.Add("-1");
                retvals.Add("");
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(retvals));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetGoodsReceiptNote(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int grnPK = Request.Params["GRNPK"] != null ? Convert.ToInt32(Request.Params["GRNPK"].Trim()) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.StoreManagement.GoodsReceiptNote.GetGoodsReceiptNoteDetails(grnPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Detail");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPendingSearchAuto(HttpContext context)
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
                Response.Write(BusinessLogic.StoreManagement.GoodsReceiptNote.GetPendingSearchAuto(searchBy, searchValue, vendorPK, grnPK, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Po Details corresponding to sbu
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseOrderPending(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int grnID = Request.Params["GRNID"] != null ? int.Parse(Request.Params["GRNID"]) : 0;
                int vendor = Request.Params["Vendor"] != null ? int.Parse(Request.Params["Vendor"]) : 0;
                int storeID = Request.Params["StoreID"] != null ? int.Parse(Request.Params["StoreID"]) : 0;
                int pohPK = Request.Params["PohPK"] != null ? int.Parse(Request.Params["PohPK"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.GoodsReceiptNote.GetPurchaseOrderPending(CommonFunctions.GetGridParams(Request), objUser.SBUID, vendor, grnID, storeID, pohPK));
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
        private static void GetPreviousGRNDetailsView(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int poID = Request.Params["POPK"] != null ? int.Parse(Request.Params["POPK"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.GoodsReceiptNote.GetPreviousGRNDetailsView(poID, objUser.SBUID));
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
        private static void GetGoodsReceiptNoteList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procID = 0, vendorPk = 0, trnStatus = 0, cmpPk=0;
            string pageUrl = "",refNo=string.Empty,grnNo=string.Empty,poNo=string.Empty;
            try
            {
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["PageUrl"] != null)
                {
                    pageUrl = Request.Params["PageUrl"];
                }
                if (Request.Params["Status"] != null && Request.Params["Status"] != "null" && Request.Params["Status"] != "")
                {
                    trnStatus = Convert.ToInt32(Request.Params["Status"]);
                }
                if (Request.Params["GRNNo"] != null && Request.Params["GRNNo"] != "null" && Request.Params["GRNNo"] != "")
                {
                    grnNo = Convert.ToString(Request.Params["GRNNo"]);
                }
                if (Request.Params["PONo"] != null && Request.Params["PONo"] != "null" && Request.Params["PONo"] != "")
                {
                    poNo = Convert.ToString(Request.Params["PONo"]);
                }
                if (Request.Params["Vendor"] != null && Request.Params["Vendor"] != "null" && Request.Params["Vendor"] != "")
                {
                    vendorPk = Convert.ToInt32(Request.Params["Vendor"]);
                }
                if (Request.Params["RefNo"] != null && Request.Params["RefNo"] != "null" && Request.Params["RefNo"] != "")
                {
                    refNo = Convert.ToString(Request.Params["RefNo"]);
                }
                if (Request.Params["CMP_PK"] != null && Request.Params["CMP_PK"] != "null" && Request.Params["CMP_PK"] != "")
                {
                    cmpPk = Convert.ToInt32(Request.Params["CMP_PK"]);
                }
                   
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.GoodsReceiptNote.GetGoodsReceiptNoteList(CommonFunctions.GetGridParams(Request), objUser, procID, pageUrl, grnNo, poNo, vendorPk, refNo, trnStatus,cmpPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Detials For Auto Complete 
        /// </summary>
        /// <param name="context"></param>
        private static void GetGRNSearchValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                string pageUrl = Request.QueryString["PageUrl"] != null ? Request.QueryString["PageUrl"] : string.Empty;                
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.GoodsReceiptNote.GetGRNSearchValue(searchBy, searchValue, objUser, pageUrl));
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
        private static void DeleteGoodsReceiptNote(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int grnPk = Request.Params["PK"] != null ? Convert.ToInt32((Request.Params["PK"].Trim())) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.StoreManagement.GoodsReceiptNote.DeleteGoodsReceiptNote(grnPk));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// get GIN Details View
        /// </summary>
        /// <param name="context"></param>
        private static void GetGRNDetailsView(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int ginID = Request.Params["GINPK"] != null ? int.Parse(Request.Params["GINPK"]) : 0;
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.GoodsReceiptNote.GetGRNDetailsView(ginID, objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("GRN Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
