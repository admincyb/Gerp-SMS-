using System;
using System.Web;
using System.Web.SessionState;
using GTIService;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        # region Methods

        private static void GoodsInspectionNoteManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                   
                    case "GetPendingSearchAuto":
                        Handlers.GetGRNPendingSearchAuto(context);
                        break;

                    case "GetGRNPending":
                        Handlers.GetGRNPending(context);
                        break;

                    case "SaveGoodsInspectionNote":
                        Handlers.SaveGoodsInspectionNote(context);
                        break;
                 
                    case "GetGINList":
                        Handlers.GetGoodsInspectionNoteList(context);
                        break;

                    case "GetSearchValue":
                        Handlers.GetGINSearchValue(context);
                        break;

                    case "DeleteGIN":
                        Handlers.DeleteGoodsInspectionNote(context);
                        break;

                    case "GetGINDetails":
                        Handlers.GetGINDetails(context);
                        break;

                    case "GetGINAlreadyInspectionDtls":
                        Handlers.GetGINInspectionDetails(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Get GIN Details To Update or another Actions
        /// </summary>
        /// <param name="context"></param>
        private static void GetGINDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int ginID = Request.Params["GIHPK"] == null || Request.Params["GIHPK"] == "null" ? 0 : int.Parse(Request.Params["GIHPK"]);
                Response.Write(BusinessLogic.StoreManagement.GoodsInspectionNote.GetGoodsInspectionNoteDetails(ginID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Inspection Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        
        /// <summary>
        /// Get Pending GRN Iem Details To AutoComplete
        /// </summary>
        /// <param name="context"></param>
        private static void GetGRNPendingSearchAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                int store = Request.Params["SearchCorr"] != null ? int.Parse(Request.Params["SearchCorr"]) : 0;
                int IsStockItem = Request.Params["IsStockItem"] == null || Request.Params["IsStockItem"] == "null" ? 0 : int.Parse(Request.Params["IsStockItem"]);
                Response.Write(BusinessLogic.StoreManagement.GoodsInspectionNote.GetPendingSearchAuto(searchBy, searchValue, store, objUser, IsStockItem));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Inspection Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        
        /// <summary>
        /// Get GRN Peding Item Details To Fill to Grid
        /// </summary>
        /// <param name="context"></param>
        private static void GetGRNPending(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int store = Request.Params["Store"] == null || Request.Params["Store"] == "null" ? 0: int.Parse(Request.Params["Store"]) ;
                int ginPk = Request.Params["GINPk"] == null || Request.Params["GINPk"] == "null" ? 0 : int.Parse(Request.Params["GINPk"]);
                int grhPk = Request.Params["GrhPK"] == null || Request.Params["GrhPK"] == "null" ? 0 : int.Parse(Request.Params["GrhPK"]);
                int isStockItem = Request.Params["IsStockItem"] == null || Request.Params["IsStockItem"] == "null" ? 0 : int.Parse(Request.Params["IsStockItem"]);
                Response.Write(BusinessLogic.StoreManagement.GoodsInspectionNote.GetGRNItemPending(CommonFunctions.GetGridParams(Request), objUser.SBUID, store, ginPk, grhPk, isStockItem));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Inspection Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Save GIN Details 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveGoodsInspectionNote(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string goodsReceiptNoteDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string goodsReceiptNote = BusinessLogic.StoreManagement.GoodsInspectionNote.SaveGoodsInspectionNote(goodsReceiptNoteDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(goodsReceiptNote);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Inspection Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Get GIn Details List To Fill All Detail to List Grid
        /// </summary>
        /// <param name="context"></param>
        private static void GetGoodsInspectionNoteList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procID = 0, vendorPk = 0, trnStatus = 0, cmpPk = 0; 
            string PageUrl = "", deptName = string.Empty, ginNo = string.Empty, grnNo = string.Empty, poNo = string.Empty;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["PageUrl"] != null)
                {
                    PageUrl = Request.Params["PageUrl"];
                }
                if (Request.Params["Status"] != null && Request.Params["Status"] != "null" && Request.Params["Status"] != "")
                {
                    trnStatus = Convert.ToInt32(Request.Params["Status"]);
                }
                if (Request.Params["GINNo"] != null && Request.Params["GINNo"] != "null" && Request.Params["GINNo"] != "")
                {
                    ginNo = Convert.ToString(Request.Params["GINNo"]);
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
                if (Request.Params["Dept"] != null && Request.Params["Dept"] != "null" && Request.Params["Dept"] != "")
                {
                    deptName = Convert.ToString(Request.Params["Dept"]);
                }
                if (Request.Params["CMP_PK"] != null && Request.Params["CMP_PK"] != "null" && Request.Params["CMP_PK"] != "")
                {
                    cmpPk = Convert.ToInt32(Request.Params["CMP_PK"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.GoodsInspectionNote.GetGoodsInspectionNoteList(CommonFunctions.GetGridParams(Request), objUser, procID, PageUrl, ginNo, grnNo, poNo, vendorPk, deptName, trnStatus, cmpPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Inspection Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get GIN Search Value For Search Details in Listing Section
        /// </summary>
        /// <param name="context"></param>
        private static void GetGINSearchValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                string pageUrl=Request.QueryString["PageUrl"] != null? Request.QueryString["PageUrl"]:string.Empty;                
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.GoodsInspectionNote.GetGINSearchValue(searchBy, searchValue, objUser,pageUrl));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Inspection Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Delete GIN Details By GIN PK
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteGoodsInspectionNote(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int grnPk = Request.Params["PK"] != null ? Convert.ToInt32((Request.Params["PK"].Trim())) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.StoreManagement.GoodsInspectionNote.DeleteGoodsInspectionNote(grnPk));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Inspection Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Method to get Previous Inspection details
        /// </summary>
        /// <param name="context"></param>
        private static void GetGINInspectionDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int status = 0;
            int sbu = 0;
            int grnDtlPk = 0;
            int storePk = 0;
            int ginPk = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                grnDtlPk = Request.Params["GRDPK"] == null || Request.Params["GRDPK"] == "null" ? 0 : int.Parse(Request.Params["GRDPK"]);
                status = Request.Params["Status"] == null || Request.Params["Status"] == "null" ? 0 : int.Parse(Request.Params["Status"]);
                sbu = Request.Params["SBU"] == null || Request.Params["SBU"] == "null" ? 0 : int.Parse(Request.Params["Sbu"]);
                storePk = Request.Params["Store"] == null || Request.Params["Store"] == "null" ? 0 : int.Parse(Request.Params["Store"]);
                ginPk = Request.Params["GinPk"] == null || Request.Params["GinPk"] == "null" ? 0 : int.Parse(Request.Params["GinPk"]);
                Response.Write(BusinessLogic.StoreManagement.GoodsInspectionNote.GetGINInspectedDetails(grnDtlPk,status, sbu, storePk, ginPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Inspection Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        # endregion
    }
}
