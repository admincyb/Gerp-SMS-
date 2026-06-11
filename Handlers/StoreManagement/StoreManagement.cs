using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using BusinessLogic;
using BusinessObject;
using GTIService;
using Newtonsoft;

using System.Web.SessionState;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        #region Methods
        /// <summary>
        /// handiling Store management handelers.
        /// </summary>
        /// <param name="context"></param>
        private static void StoreManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    
                    case "GetStoreList":
                        Handlers.GetStoreList(context);
                        break;
                    case "SaveStore":
                        Handlers.SaveStore(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetStoreSearchValue(context);
                        break;
                    case "DeleteStore":
                        Handlers.DeleteStore(context);
                        break;
                    case "GetStoreType":
                        Handlers.GetStoreType(context);
                        break;
                    case "GetInventoryLocation":
                        Handlers.GetInventoryLocation(context);
                        break;
                    
                }
            }
        }

        /// <summary>
        /// Get Store List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetStoreList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.StoreMaster.GetStoreList(CommonFunctions.GetGridParams(Request)));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Store name and pk
        /// </summary>
        /// <param name="context"></param>
        private static void GetStoreType(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.StoreMaster.GetStoreTypes());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Store name and pk
        /// </summary>
        /// <param name="context"></param>
        private static void GetInventoryLocation(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            string DeptPK = string.Empty;
            string Type = string.Empty;
            string Category = string.Empty;
            string searchValue = string.Empty;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["DeptPK"] != null)
                {
                    DeptPK = Request.Params["DeptPK"].ToString();
                }
                if (Request.Params["Type"] != null)
                {
                    Type = Request.Params["Type"].ToString();
                }
                if (Request.Params["Category"] != null)
                {
                    Category = Request.Params["Category"].ToString();
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.StoreMaster.GetInventoryLocation(DeptPK,Type,Category, searchValue));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Search Store Details , and Get Data Related to Search Criteria
        /// </summary>
        /// <param name="context"></param>
        private static void GetStoreSearchValue(HttpContext context)
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

                if (Request.Params["SearchType"] != null)
                {
                    searchBy = Request.Params["SearchType"].ToString();
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }

                Response.Write(BusinessLogic.StoreManagement.StoreMaster.GetSearchValues(searchBy, searchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// saving Store details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveStore(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.TEXT);
                Response.Write(BusinessLogic.StoreManagement.StoreMaster.SaveStore(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Method Used to Delete Store Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteStore(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int storeID = 0;
            try
            {

                if (Request.Params["StoreID"] != null)
                {
                    // Assign StoreID From Request to StoreID variable
                    storeID = Convert.ToInt32((Request.Params["StoreID"].Trim()));

                }

                Response.Write(BusinessLogic.StoreManagement.StoreMaster.DeleteStore(storeID));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Store Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        #endregion
    }
}
