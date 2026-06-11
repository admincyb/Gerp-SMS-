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

        #region methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void StoreAuditManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "GetStoreAuditList":
                        Handlers.GetStoreAuditList(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GeStoreAuditSearchValue(context);
                        break;
                    case "DeleteStoreAudit":
                        Handlers.DeleteStoreAudit(context);
                        break;
                    case "SaveStoreAuditDtls":
                        Handlers.SaveStoreAuditDtls(context);
                        break;
                    case "GetItemDetails":
                        Handlers.GetItemDetails(context);
                        break;
                    case "GetCategoryItemDetails":
                        Handlers.GetCategoryItemDetails(context);
                        break;
                    case "GetItems":
                        Handlers.GetItems(context);
                        break;
                    case "GetStore":
                        Handlers.GetItems(context);
                        break;
                    case "GetDamageTypes":
                        Handlers.GetDamageTypes(context);
                        break;

                }
            }
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="context"></param>
        private static void GetStoreAuditList(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procID = 0;
            string PageUrl = "";
            try
            {
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["PageUrl"] != null)
                {
                    PageUrl = Request.Params["PageUrl"];
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.StoreManagement.StoreAuditList.GetStoreAuditList(CommonFunctions.GetGridParams(Request), objUser, procID, PageUrl));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Audit Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
       
        private static void GeStoreAuditSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            try
            {

                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                if (Request.Params["SearchType"] != null)
                {
                    searchBy = Request.Params["SearchType"].ToString();
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }

                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.StoreAuditList.GetSearchValues(searchBy, searchValue, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Audit Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="context"></param>
        private static void DeleteStoreAudit(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int requestPk = 0;
            try
            {
                if (Request.Params["PK"] != null)
                {
                    requestPk = Convert.ToInt32((Request.Params["PK"].Trim()));

                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.StoreManagement.StoreAuditList.DeleteStoreAuditDtls(requestPk));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Store Audit Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Methord used to save store Audit details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveStoreAuditDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {

                string sADetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string sAID = BusinessLogic.StoreManagement.StoreAudit.SaveStoreAuditDetails(sADetails, objUser);
                //GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                //Response.Write(sAID);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(sAID);

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Audit");
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
        private static void GetItemDetails(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int dept = Request.Params["Dept"] != null ? int.Parse(Request.Params["Dept"]) : 0;
                int itemPK = Request.Params["Item"] != null ? int.Parse(Request.Params["Item"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.StoreAudit.GetItemDetails(dept, itemPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Audit Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetCategoryItemDetails(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int dept = Request.Params["Dept"] != null ? int.Parse(Request.Params["Dept"]) : 0;
                int categoryPK = Request.Params["Category"] != null ? int.Parse(Request.Params["Category"]) : 0;
                int itemPK = Request.Params["Item"] != null ? int.Parse(Request.Params["Item"]) : 0;
                int batchPK = Request.Params["Batch"] != null ? int.Parse(Request.Params["Batch"]) : 0;
                int StkbatchPK = Request.Params["StkBatch"] != null ? int.Parse(Request.Params["StkBatch"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.StoreAudit.GetCategoryItemDetails(dept, categoryPK, itemPK, batchPK, StkbatchPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Audit Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetItems(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit=0,  storePK=0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["PK"] != null)
                {
                    bizUnit = Convert.ToInt32((Request.Params["PK"].Trim()));

                }
                if (Request.Params["PK"] != null)
                {
                    storePK = Convert.ToInt32((Request.Params["PK"].Trim()));

                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.StoreAudit.GetItems( bizUnit,  storePK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Audit Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetStore(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["PK"] != null)
                {
                    bizUnit = Convert.ToInt32((Request.Params["PK"].Trim()));

                }
              
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.StoreAudit.GetStore(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Audit Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetDamageTypes(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBU"] != null)
                {
                    bizUnit = Convert.ToInt32((Request.Params["SBU"].Trim()));

                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.StoreAudit.GetDamageTypes(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Audit Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        #endregion
    }
}
