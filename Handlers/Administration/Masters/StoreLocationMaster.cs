using System;
using System.Web;
using System.Web.SessionState;
using GTIService;
namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        #region Methods

        /// <summary>
        /// handiling Sub Department management handlers.
        /// </summary>
        /// <param name="context"></param>
        private static void StoreLocationManagement(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "GetProductionStoreDepartments":
                        Handlers.GetProductionStoreDepartments(context);
                        break;                    
                    case "GetStoreLocDtlsByID":  // by ID
                        Handlers.GetStoreLocDtlsByID(context);
                        break;
                    case "GetStoreLocationList": // List
                        Handlers.GetStoreLocationList(context);
                        break;
                    case "DeleteStoreLocDtls":
                        Handlers.DeleteStoreLocationDtls(context);
                        break;
                    case "SavePage":
                        Handlers.SaveStoreLocationDtls(context);
                        break;

                    case "GetSearchValue":
                        Handlers.GetStoreLocSearchValue(context);
                        break;                    
                }
            }
        }

        /// <summary>
        /// Get  Department Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetProductionStoreDepartments(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;           
            try
            {
                if (Request.Params["BizUnit"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Masters.StoreLocationMaster.GetProductionDepartmentDtls(sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Save StoreLocation Dtls
        /// </summary>
        /// <param name="context"></param>
        private static void SaveStoreLocationDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Masters.StoreLocationMaster.SaveStoreLocationDtls(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Location Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Get StoreLocation List
        /// </summary>
        /// <param name="context"></param>
        private static void GetStoreLocationList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int deptCategory = 0;
            try
            {
                bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                deptCategory = Convert.ToInt32(Request.Params["DPTCATEGORY"]);
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.StoreLocationMaster.GetStoreLocationList(CommonFunctions.GetGridParams(Request), bizUnit, deptCategory));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Delete StoreLocation  Details By Sub Dept Pk
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteStoreLocationDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int storeLocID = 0;
            try
            {
                if (Request.Params["StoreLocID"] != null)
                {
                    storeLocID = Convert.ToInt32((Request.Params["StoreLocID"].Trim()));

                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Masters.StoreLocationMaster.DeleteStoreLOcation(storeLocID));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("SubDepartment Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Get StoreLocDtls  By store location ID
        /// </summary>
        /// <param name="context"></param>
        private static void GetStoreLocDtlsByID(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int storeLocID = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["StoreLocID"] != null)
                {
                    storeLocID = Convert.ToInt32(Request.Params["StoreLocID"]);
                }

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.StoreLocationMaster.GetStoreLocDetailsByID(storeLocID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Location Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }

        /// <summary>
        /// Get StoreLocation Details Search value
        /// </summary>
        /// <param name="context"></param>
        private static void GetStoreLocSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int bizUnit = 0;
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
                if (Request.QueryString["SBU"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.QueryString["SBU"].ToString());
                }

                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Masters.StoreLocationMaster.GetSearchValues(searchBy, searchValue, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Designation Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        #endregion
    }
}
