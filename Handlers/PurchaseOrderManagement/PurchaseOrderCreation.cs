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
        private static void CreatePurchaseOrder1(HttpContext context)
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

                    case "GetSearchValue":
                        Handlers.GetPurchaseSearchValue(context);
                        break;
                    case "GetPODetails":
                        Handlers.GetPurchaseOrderDetails(context);
                        break;
                    case "DeletePODetails":
                        Handlers.DeletePODetails(context);
                        break;
                }
            }
        }

        private static void SavePurchaseOrder1(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {

                string PODetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string poID = BusinessLogic.PurchaseOrderManagement.PurchaseOrderCreation.SavePODetails(PODetails, objUser);
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
        /// Get Filter values for filling the search value
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseSearchValue1(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

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
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderCreation.GetSearchValues(searchBy, searchValue,objUser));
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
        private static void GetPurchaseOrderDetails1(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderCreation.GetPurchaseOrderDetails(CommonFunctions.GetGridParams(Request), objUser.SBUID));
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
        private static void DeletePODetails1(HttpContext context)
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
                Response.Write(BusinessLogic.PurchaseOrderManagement.PurchaseOrderCreation.DeletePODetails(poID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Order Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
