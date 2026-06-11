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
        /// handiling StoreRequisitionSlipCreation handelers.
        /// </summary>
        /// <param name="context"></param>
        private static void NewItemRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {

                    case "SaveNewItemRequest":
                        Handlers.SaveNewItemRequest(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetNewItemSearchValue(context);
                        break;
                    case "GetNIRNo":
                        Handlers.GetNIRNo(context);
                        break;
                    case "GetNIRList":
                        Handlers.GetNIRList(context);
                        break;
                    case "GetNewItemCheckList":
                        Handlers.GetNewItemCheckList(context);
                        break;
                    case "DeleteNIR":
                        Handlers.DeleteNIR(context);
                        break;

                }
            }
        }
        /// <summary>
        /// save New Item Request Details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveNewItemRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string newItemDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string requisition = BusinessLogic.StoreManagement.NewItemRequestBL.SaveNewItemRequest(newItemDetails);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(requisition);


            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("New Item Request");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }

        }


        /// <summary>
        /// Get NIR No
        /// </summary>
        /// <param name="context"></param>
        private static void GetNIRNo(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.StoreManagement.NewItemRequestBL.GetNIRNo());
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("New Item Request ");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get New Item Request List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetNIRList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int procID = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.NewItemRequestBL.GetNewItemRequestList(CommonFunctions.GetGridParams(Request), bizUnit, objUser, procID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("New Item Request List");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get New Item Check List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetNewItemCheckList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            string itemName = string.Empty;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["ItemName"] != null)
                {
                    itemName =Request.Params["ItemName"].ToString();
                }
               
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.NewItemRequestBL.GetNewItemCheckList(bizUnit, itemName));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("New Item Request List");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Search New Item Request Details , and Get Data Related to Search Criteria
        /// </summary>
        /// <param name="context"></param>
        private static void GetNewItemSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int sbuPk = 0;
            int procId = 0;
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
                if (Request.QueryString["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.QueryString["SBUPk"].ToString());
                }
                if (Request.QueryString["ProcID"] != null)
                {
                    procId = Convert.ToInt32(Request.QueryString["ProcID"].ToString());
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.NewItemRequestBL.GetSearchValues(searchBy, searchValue, sbuPk, objUser, procId));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("New Item Request List");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Method Used to Delete New Item Request Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteNIR(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int itemID = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                if (Request.Params["ItemID"] != null)
                {
                    // Assign requisitionID From Request to requisitionID variable
                    itemID = Convert.ToInt32((Request.Params["ItemID"].Trim()));

                }

                Response.Write(BusinessLogic.StoreManagement.NewItemRequestBL.DeleteNewItemRequestDtls(itemID));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("New Item Request List");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
       
        #endregion
    }
}
