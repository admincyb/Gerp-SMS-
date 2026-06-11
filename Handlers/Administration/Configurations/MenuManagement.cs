using System;
using System.Web;

using BusinessObject;
using GTIService;
using Newtonsoft;

namespace Handlers
{
    public partial class Handlers : IHttpHandler
    {

        private static void MenuManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveMenuDetails":
                        Handlers.SaveMenuDetails(context);
                        break;    
                    case "GetMenuListDtls":
                        Handlers.GetMenuListDtls(context);
                        break;
                    case "GetMenuDetails":
                        Handlers.GetMenuDetails(context);
                        break;
                    case "GetPageList":
                        Handlers.GetPageList(context);
                        break;
                    case "DeleteMenuDetails":
                        Handlers.DeleteMenuDetails(context);
                        break;

                    case "SaveMenuGrpDetails":
                        Handlers.SaveMenuGrpDetails(context);
                        break;

                    case "GetMenuGrpDetails":
                        Handlers.GetMenuGrpDtls(context);
                        break;

                    case "GetAutoCompleteMenu":
                        Handlers.GetAutoCompleteMenu(context);
                        break;
                }
            }
        }
        /// <summary>
        /// Get Pages List from Page Masters
        /// </summary>
        /// <param name="context"></param>
        private static void GetPageList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Configurations.MenuManagement.GetPageList());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }
        /// <summary>
        /// Get Menu Group Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetMenuGrpDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int menuPK = Request.Params["deptID"] != null ? Convert.ToInt32(Request.Params["deptID"]) : 0;
                int menuParentPK = Request.Params["MenuParentID"] != null ? Convert.ToInt32(Request.Params["MenuParentID"]) : 0;
                int bizUnit = Request.Params["SBUPk"] != "null" ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                int userGroup = Request.Params["UserGroup"] != null ? Convert.ToInt32(Request.Params["UserGroup"]) : 0;
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Configurations.MenuManagement.GetMenuGrpDtls( menuPK,  menuParentPK,bizUnit, userGroup));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }
        private static void SaveMenuGrpDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Configurations.MenuManagement.SaveMenuGroupDetails(GetRequestString(context)));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("DefaultValue Config");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Function Used To Save  Menu Details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveMenuDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Configurations.MenuManagement.SaveMenuDetails(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Menu Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Function Used To Get All Child Menu details by Menu id
        /// </summary>
        /// <param name="context"></param>
        private static void GetMenuListDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
              
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int menuParentID = Request.Params["MenuParentID"] != null ? Convert.ToInt32((Request.Params["MenuParentID"].Trim())) : 0;
                
                Response.Write(BusinessLogic.Administration.Configurations.MenuManagement.GetMenuListDtls(menuParentID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Menu Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get Details Menu details by Menu id
        /// </summary>
        /// <param name="context"></param>
        private static void GetMenuDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int menuID = Request.Params["MenuID"] != null ? Convert.ToInt32((Request.Params["MenuID"].Trim())) : 0;
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Configurations.MenuManagement.GetMenuDetails(menuID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Menu Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method Used to Delete  Menu details by Menu id
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteMenuDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int menuID = Request.Params["MenuID"] != null ? Convert.ToInt32((Request.Params["MenuID"].Trim())) : 0;
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Configurations.MenuManagement.DeleteMenuDetails(menuID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Menu Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Method Used to Delete  Menu details by Menu id
        /// </summary>
        /// <param name="context"></param>
        private static void GetAutoCompleteMenu(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {

                string searchValue = Request.Params["SearchValue"] != null ? Request.Params["SearchValue"] : string.Empty;
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.Administration.Configurations.MenuManagement.GetSearchMenuAuto(searchValue, objUser.SBUID, objUser.CurrentDeptPK, objUser.PKUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Menu Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
    }
}
