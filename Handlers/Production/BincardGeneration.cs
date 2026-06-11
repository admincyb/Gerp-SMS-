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
        /// <summary>
        /// Hanlder function used for Bin Card Generation and Listing
        /// </summary>
        /// <param name="context"></param>
        private static void BinCardGeneration(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
            if (Request.Params.Count > 0)
            {
                switch (Action)
                {
                    case "CreateBinCard":
                        Handlers.SaveBinCard(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetBinSearchValue(context);
                        break;
                    case "GetBinDetails":
                        Handlers.GetBinDetails(context);
                        break;
                    case "DeleteBinDetails":
                        Handlers.DeleteBinDetails(context);
                        break;
                }
            }
        }

        /// <summary>
        /// function used to save/update bin card details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveBinCard(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {

                string binDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string binID = BusinessLogic.Production.BinCardGeneration.SaveBinDetails(binDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(binID);


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
        private static void GetBinSearchValue(HttpContext context)
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
                Response.Write(BusinessLogic.Production.BinCardGeneration.GetSearchValues(searchBy, searchValue, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("PO Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Bin Details corresponding to sbu
        /// </summary>
        /// <param name="context"></param>
        private static void GetBinDetails(HttpContext context)
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
                Response.Write(BusinessLogic.Production.BinCardGeneration.GetBinCardDetails(CommonFunctions.GetGridParams(Request), objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Handler used to Delete Bin details by passing Bin ID
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteBinDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int poID = 0;
            try
            {
                if (Request.Params["BinID"] != null)
                {
                    poID = Convert.ToInt32((Request.Params["BinID"].Trim()));
                }
                Response.Write(BusinessLogic.Production.BinCardGeneration.DeleteBinCardDetails(poID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Bin Card Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
