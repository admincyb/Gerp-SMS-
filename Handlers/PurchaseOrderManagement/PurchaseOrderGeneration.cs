using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BusinessLogic;
using BusinessObject;
using GTIService;
using Newtonsoft;

using System.Web.SessionState;
using System.Web;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        private static void CreatePurchaseOrder(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "CreatePurchaseOrder":
                        Handlers.SavePurchaseOrder(context);
                        break;
                    case "GetPurchaseAutoSearchValue":
                        Handlers.GetPurchaseAutoSearchValue(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetPurchaseSearchValue(context);
                        break;
                    case "GetPODetails":
                        Handlers.GetPurchaseOrderDetails(context);
                        break;
                    case "GetPurchaseOrderList":
                        Handlers.GetPurchaseOrderList(context);
                        break;
                    case "DeletePODetails":
                        Handlers.DeletePODetails(context);
                        break;

                    case "GetPRDetails":
                        Handlers.GetPRDetails(context);
                        break;

                    case "POShortClose":
                        Handlers.POShortClose(context);
                        break;
                    case "GetPODetailsView":
                        Handlers.GetPurchaseOrderDetailsView(context);
                        break;
                    case "GetPendingPODetails":
                        Handlers.GetPendingPODetails(context);
                        break;

                    #region Trading
                    case "GetPurchaseOrderListTrading":
                        Handlers.GetPurchaseOrderListTrading(context);
                        break;
                    #endregion
                }
            }
        }

        private static void POShortClose(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string PODetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.SavePOShortClose(PODetails, objUser));


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

        private static void SavePurchaseOrder(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {

                string pODetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string poID = BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.SavePODetails(pODetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
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
        /// Get Filter values for filling the search dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseAutoSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = string.Empty;
                if (Request.QueryString["SearchValue"] != null)
                    searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString().Trim() + "%" : "%%";
                else
                    searchValue = Request.Params["SearchValue"] != null ? "%" + Request.Params["SearchValue"].ToString().Trim() + "%" : "%%";
                string pageURL = Request.QueryString["PageURL"] != null ? Request.QueryString["PageURL"] : string.Empty;
                int type = Request.Params["TYPE"] != null && Request.Params["TYPE"] != "null" ? int.Parse(Request.Params["TYPE"]) : 0;
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPurchaseAutoSearchValue(searchBy, searchValue, pageURL, objUser, type));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("PO Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Filter values for filling the search value
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : "%%";
                int processPK = Request.QueryString["ProcessPK"] != null ? int.Parse(Request.QueryString["ProcessPK"]) : 0;
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetSearchValues(searchBy, searchValue, processPK, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("PO Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Po Details corresponding to sbu
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseOrderDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procID = 0;
            string pageUrl = "";
            byte orderGroup = 1;
            try
            {
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32((Request.Params["ProcID"].Trim()));
                }
                if (Request.Params["PageUrl"] != null)
                {
                    pageUrl = Request.Params["PageUrl"];
                }

                if (Request.Params["Service"] != null)
                {
                    orderGroup = Convert.ToByte(Request.Params["Service"]);
                }



                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPurchaseOrderDetails(CommonFunctions.GetGridParams(Request), objUser.SBUID, procID, pageUrl, orderGroup));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetPurchaseOrderList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procID = 0;
            string pageUrl = "";
            byte orderGroup = 1;
            int transactionStatus=-1;
            int vendor=0;
            string poNo=string.Empty;
            string prNo=string.Empty;
            string ioNo=string.Empty;
            string ItemName = string.Empty;
            int reqStore=0;
            int Status = 0;
            int cmpPk = 0;
            try
            {
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32((Request.Params["ProcID"].Trim()));
                }
                if (Request.Params["PageUrl"] != null)
                {
                    pageUrl = Request.Params["PageUrl"];
                }
                if (Request.Params["TrnStatus"] != null && Request.Params["TrnStatus"] != "null")
                {
                    transactionStatus = Convert.ToInt32((Request.Params["TrnStatus"].Trim()));
                }
                if (Request.Params["vendor"] != null && Request.Params["vendor"] !=string.Empty)
                {
                    vendor = Convert.ToInt32((Request.Params["vendor"].Trim()));
                }
                if (Request.Params["reqStore"] != null && Request.Params["reqStore"] != string.Empty)
                {
                    reqStore = Convert.ToInt32((Request.Params["reqStore"].Trim()));
                }
                if (Request.Params["poNo"] != null)
                {
                    poNo = Request.Params["poNo"].Trim();
                }
                if (Request.Params["prNo"] != null)
                {
                    prNo = Request.Params["prNo"].Trim();
                }
                if (Request.Params["ioNo"] != null)
                {
                    ioNo = Request.Params["ioNo"].Trim();
                }
                if (Request.Params["ItmName"] != null)
                {
                    ItemName = Request.Params["ItmName"].Trim();
                }
                if (Request.Params["Status"] != null)
                {
                    Status = Convert.ToInt32((Request.Params["Status"].Trim()));
                }
                if (Request.Params["Service"] != null)
                {
                    orderGroup = Convert.ToByte(Request.Params["Service"]);
                }
                if (Request.Params["CMP_PK"] != null && Request.Params["CMP_PK"] != "null")
                {
                    cmpPk = Convert.ToInt32(Request.Params["CMP_PK"]);
                }
               
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPurchaseOrderList(CommonFunctions.GetGridParams(Request), objUser.SBUID, pageUrl, Status, transactionStatus, vendor, poNo, prNo, ioNo, reqStore, orderGroup, ItemName,cmpPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Get Pending Po Details corresponding to materialid
        /// </summary>
        /// <param name="context"></param>
        private static void GetPendingPODetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int toUOM = 0;
            try
            {
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["ToUOM"] != null)
                {
                    toUOM = Convert.ToInt32(Request.Params["ToUOM"]);
                }  
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPendingPODetails(CommonFunctions.GetGridParams(Request), materialID, toUOM));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Handler used to Delete PO details by passing PO ID
        /// </summary>
        /// <param name="context"></param>
        private static void DeletePODetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int poID = 0;
            try
            {
                if (Request.Params["POID"] != null)
                {
                    poID = Convert.ToInt32((Request.Params["POID"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.DeletePODetails(poID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Order Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Handler used to Delete PO details by passing PO ID
        /// </summary>
        /// <param name="context"></param>
        private static void GetPRDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int poID = 0;
            int itemID = 0;
            int uom = 0;
            int bizUnit = 0;
            try
            {
                if (Request.Params["POID"] != null)
                {
                    poID = Convert.ToInt32((Request.Params["POID"].Trim()));
                }
                if (Request.Params["ITEMID"] != null)
                {
                    itemID = Convert.ToInt32((Request.Params["ITEMID"].Trim()));
                }
                if (Request.Params["UOM"] != null)
                {
                    uom = Convert.ToInt32((Request.Params["UOM"].Trim()));
                }

                bizUnit = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPRDetails(poID, itemID, uom, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Order Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Po Details corresponding to sbu
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseOrderDetailsView(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int poID = Request.Params["POPK"] != null ? int.Parse(Request.Params["POPK"]) : 0;
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPurchaseOrderDetailsView(poID, objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        #region Trading
        private static void GetPurchaseOrderListTrading(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procID = 0;
            string pageUrl = "";
            byte orderGroup = 1;
            int transactionStatus = -1;
            int vendor = 0;
            string poNo = string.Empty;
            string prNo = string.Empty;
            string ioNo = string.Empty;
            string ItemName = string.Empty;
            int reqStore = 0;
            int Status = 0;
            try
            {
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32((Request.Params["ProcID"].Trim()));
                }
                if (Request.Params["PageUrl"] != null)
                {
                    pageUrl = Request.Params["PageUrl"];
                }
                if (Request.Params["TrnStatus"] != null && Request.Params["TrnStatus"] != "null")
                {
                    transactionStatus = Convert.ToInt32((Request.Params["TrnStatus"].Trim()));
                }
                if (Request.Params["vendor"] != null && Request.Params["vendor"] != string.Empty)
                {
                    vendor = Convert.ToInt32((Request.Params["vendor"].Trim()));
                }
                if (Request.Params["reqStore"] != null && Request.Params["reqStore"] != string.Empty)
                {
                    reqStore = Convert.ToInt32((Request.Params["reqStore"].Trim()));
                }
                if (Request.Params["poNo"] != null)
                {
                    poNo = Request.Params["poNo"].Trim();
                }
                if (Request.Params["prNo"] != null)
                {
                    prNo = Request.Params["prNo"].Trim();
                }
                if (Request.Params["ioNo"] != null)
                {
                    ioNo = Request.Params["ioNo"].Trim();
                }
                if (Request.Params["ItmName"] != null)
                {
                    ItemName = Request.Params["ItmName"].Trim();
                }
                if (Request.Params["Status"] != null)
                {
                    Status = Convert.ToInt32((Request.Params["Status"].Trim()));
                }
                if (Request.Params["Service"] != null)
                {
                    orderGroup = Convert.ToByte(Request.Params["Service"]);
                }

                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPurchaseOrderListTrading(CommonFunctions.GetGridParams(Request), objUser.SBUID, pageUrl, Status, transactionStatus, vendor, poNo, prNo, ioNo, reqStore, orderGroup, ItemName));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        #endregion
    }
}
