using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.SessionState;
using GTIService;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {

        #region methods

        /// <summary>
        /// handiling Inbox related handlers.
        /// </summary>
        /// <param name="context"></param>
        private static void InboxManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    // Get all GetInboxTask
                    case "GetInboxTask":
                        Handlers.GetInboxTask(context);
                        break;
                    // Get all GetInboxIntimations
                    case "GetInboxIntimations":
                        Handlers.GetInboxIntimations(context);
                        break;

                    case "RevokeCmpltdTask":
                        Handlers.RevokeCompletedTask(context);
                        break;

                    case "GetProcessID":
                        Handlers.GetProcessList(context);
                        break;
                }
            }
        }
        /// <summary>
        /// get Process list
        /// </summary>
        /// <param name="context"></param>
        private static void GetProcessList(HttpContext context)
        {
          
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetProcess(((BusinessObject.User)(HttpContext.Current.User.Identity)).PKUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Inbox Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// hander Methord Used to Get Inbox Task
        /// </summary>
        /// <param name="context"></param>
        private static void GetInboxTask(HttpContext context)
        {
           
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.GridPrams grdparms = CommonFunctions.GetGridParams(Request);
            int user =((BusinessObject.User)(HttpContext.Current.User.Identity)).PKUser;
            int procID = 0;
            try
            {
                // clear all the response
                if (Request.Params["ProcID"] != null && Request.Params["ProcID"] != "null")
                {
                    procID = int.Parse(Request.Params["ProcID"].Trim());
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.AccountManagement.InboxManagement.GetInboxTasks(grdparms, user, procID));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Detail");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// hander Methord Used to Get Inbox Intimations
        /// </summary>
        /// <param name="context"></param>
        private static void GetInboxIntimations(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.GridPrams grdparms = CommonFunctions.GetGridParams(Request);
            int user = ((BusinessObject.User)(HttpContext.Current.User.Identity)).PKUser;
            int procID = 0;
            try
            {
                // clear all the response
                if (Request.Params["ProcID"] != null)
                {
                    procID = int.Parse(Request.Params["ProcID"].Trim());
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.AccountManagement.InboxManagement.GetInboxInitmation(CommonFunctions.GetGridParams(Request), user, procID));

            }
            catch(Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Detail");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

            
        }

        /// <summary>
        /// hander Methord Used to RevokeCompletedTask
        /// </summary>
        /// <param name="context"></param>
        private static void RevokeCompletedTask(HttpContext context)
        {
            int refID = 0;
            int actionID = 0;
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                int user = ((BusinessObject.User)(HttpContext.Current.User.Identity)).PKUser;
                if (Request.Params["RefID"] != null)
                {
                    refID = int.Parse(Request.Params["RefID"].Trim());
                }
                if (Request.Params["ActionID"] != null)
                {
                    actionID = int.Parse(Request.Params["ActionID"].Trim());
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.TEXT);
                Response.Write(BusinessLogic.AccountManagement.InboxManagement.RevokeCompletedTask(user,refID,actionID));               
               
            }
            catch { }
        }

        #endregion

        
    }
}
