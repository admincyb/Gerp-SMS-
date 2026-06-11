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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void StockTranferManagement(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    #region Entry Section

                    case "GetAllocatedAndTransferDetails":
                        Handlers.GetAllocatedAndTransferDetails(context);
                        break;
                    case "GetGINDtls":
                        Handlers.GetGINItemDetails(context);
                        break;
                    case "GetStoreByItem":
                        Handlers.GetStoreByItem(context);
                        break;
                    case "GetItemUOM":
                        Handlers.GetUOMDetailsBYItem(context);
                        break;
                    case "SavePage":
                        Handlers.SaveStockTransfer(context);
                        break;
                  
                    #endregion


                    #region Listing Section

                    case "GetStockTransferList":
                        Handlers.GetStockTransferList(context);
                        break;

                    case "GetSearchValue":
                        Handlers.GetAutoSearchValue(context);
                        break;

                    case "DeleteStockTransfer":
                        Handlers.DeleteStockTransferDetails(context);
                        break;

                    #endregion

                }
            }
        }
        #region Entry Section
        /// <summary>
        /// Get Allocatiuon And Transfer Details - Get PO detils by selected GIN  And get PR Details by Selected po Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetAllocatedAndTransferDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            int type = 0;
            int itemList = 0;
            int sbu = 0;
            int Pk = 0;
            try
            {
                string xmlDtls = GetRequestString(context);
               
                if (Request.Params["Type"] != null)
                {
                    type = Convert.ToInt32(Request.Params["Type"]);
                }
                if (Request.Params["ItemList"] != null)
                {
                    itemList = Convert.ToInt32(Request.Params["ItemList"]);
                }
                if (Request.Params["Sbu"] != null)
                {
                    sbu = Convert.ToInt32(Request.Params["Sbu"]);
                }
                if (Request.Params["Pk"] != null)
                {
                    Pk = Convert.ToInt32(Request.Params["Pk"]);
                }

                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string allocatedAndTransferDetails = BusinessLogic.StoreManagement.StockTransferBL.GetPRPODetails(sbu, type, itemList, xmlDtls, Pk);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(allocatedAndTransferDetails);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Stock Transfer");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Method to Pending GIN Details fro Stock Transfer
        /// </summary>
        /// <param name="context"></param>
        private static void GetGINItemDetails(HttpContext context)
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
                Response.Write(BusinessLogic.StoreManagement.StockTransferBL.GetGINDetails(grnDtlPk, status, sbu, storePk, ginPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Inspection Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// This Function Used To Get all store name by selected item
        /// </summary>
        /// <param name="context"></param>
        private static void GetStoreByItem(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int categoryID = 0;
            int materialID = 0;
            int sbuPk = 0;
            int type = 0;
            int store = 0;
            int userPK = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                userPK = objUser.PKUser;
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                if (Request.Params["DeptPk"] != null)
                {
                    store = Convert.ToInt32(Request.Params["DeptPk"]);
                }
                
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.StockTransferBL.GetItemByStore(categoryID, materialID, sbuPk, type, userPK, store));  
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Method to get UOM Code By Item
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMDetailsBYItem(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int status = 0;
            int itemPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["Status"] != null)
                {
                    status = Convert.ToInt32(Request.Params["Status"]);
                }
                if (Request.Params["ItemPk"] != null)
                {
                    itemPk = Convert.ToInt32(Request.Params["ItemPk"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.StockTransferBL.GetItemUOMDetails(itemPk, status));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Transfer ");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Method to Save Stock Transfer 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveStockTransfer(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            string lastModDate = string.Empty;
            try
            {
                string goodsReceiptNoteDetails = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["LastModDate"] != null)
                {
                    lastModDate = Request.Params["LastModDate"].ToString();
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string goodsReceiptNote = BusinessLogic.StoreManagement.StockTransferBL.SaveStockTransferDtls(goodsReceiptNoteDetails, lastModDate);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(goodsReceiptNote);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Stock Transfer");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");
            }
        }
        #endregion

      

        #region Listing Section
        /// <summary>
        /// Get GIn Details List To Fill All Detail to List Grid
        /// </summary>
        /// <param name="context"></param>
        private static void GetStockTransferList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procID = 0, vendorPk = 0, trnStatus = 0,cmpPk = 0;
            string PageUrl = "", deptName = string.Empty,saNo = string.Empty, ginNo = string.Empty, grnNo = string.Empty, poNo = string.Empty;
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
                if (Request.Params["SANo"] != null && Request.Params["SANo"] != "null" && Request.Params["SANo"] != "")
                {
                    saNo = Convert.ToString(Request.Params["SANo"]);
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
                Response.Write(BusinessLogic.StoreManagement.StockTransferBL.GetStockTransferList(CommonFunctions.GetGridParams(Request), objUser, procID, PageUrl,saNo,ginNo, grnNo, poNo, vendorPk, deptName, trnStatus,cmpPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Stock Transfer");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Delete Stock Transfer Details by PK
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteStockTransferDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string lastModDt = string.Empty;
            try
            {              
                int grnPk = Request.Params["PK"] != null ? Convert.ToInt32((Request.Params["PK"].Trim())) : 0;             
                if (Request.Params["LastModDt"] != null)
                {
                    lastModDt = Request.Params["LastModDt"].Trim();
                }
                string stocktransferDet = BusinessLogic.StoreManagement.StockTransferBL.DeleteStockTransferDtls(grnPk, lastModDt);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(stocktransferDet);
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Stock Transfer");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Method to Auto Search values
        /// </summary>
        /// <param name="context"></param>
        private static void GetAutoSearchValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procID = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.StockTransferBL.GetAutoCompleteSearch(searchBy, searchValue, objUser, procID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Stock Transfer");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        #endregion

    }
}
