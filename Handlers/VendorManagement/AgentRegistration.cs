using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private static void AgentRegistration(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {                    
                    case "SaveAgentDetails":
                        Handlers.RegisterAgent(context);
                        break;                    
                    case "GetAgentDetails":
                        Handlers.GetAgentDetails(context);
                        break;                   
                    case "DeleteAgentDetails":
                        Handlers.DeleteAgentDetails(context);
                        break;  
                    case "SaveAgentBank":
                        Handlers.SaveAgentBank(context);
                        break;                  
                    case "GetAgentBanks":
                        Handlers.GetAgentBanks(context);
                        break;
                    case "GetBankDetaislByIdAgent":
                        Handlers.GetBankDetailsByIdAgent(context);
                        break;
                    case "DeleteAgentBank":
                        Handlers.DeleteAgentBank(context);
                        break;
                }
            }
        }
            

        /// <summary>
        /// Add order Details
        /// </summary>
        /// <param name="context"></param>
        private static void RegisterAgent(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {

                string vendorDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string vendorPk = BusinessLogic.VendorManagement.AgentRegistration.RegisterAgent(vendorDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(vendorPk);


            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Agent Registration");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }

        }     

        /// <summary>
        /// method used to get Agent details
        /// </summary>
        /// <param name="context"></param>
        private static void GetAgentDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procId = 0;
            string PageUrl = "";
            int VenRole = 0;
            try
            {
                if (Request.Params["ProcID"] != null)
                {
                    procId = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["PageUrl"] != null)
                {
                    PageUrl = Request.Params["PageUrl"];
                }
                if (Request.Params["Type"] != null)
                {
                    VenRole = Convert.ToInt32(Request.Params["Type"]);
                }

                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.AgentRegistration.GetAgentDetails(CommonFunctions.GetGridParams(Request), objUser.SBUID, procId, PageUrl,VenRole));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }

   
        /// <summary>
        /// Handler used to Delete Agent details by passing Agent ID
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteAgentDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int AgentID = 0;
            DateTime LASTMODDT=new DateTime();
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["AgentID"] != null)
                {
                    AgentID = Convert.ToInt32((Request.Params["AgentID"].Trim()));
                }
                if (Request.Params["LASTMODDT"] != null)
                {
                    LASTMODDT =  DateTime.Parse(Request.Params["LASTMODDT"]);
                }
                Response.Write(BusinessLogic.VendorManagement.AgentRegistration.DeleteAgentDetails(AgentID,LASTMODDT));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Agent Registration");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }       

        /// <summary>
        /// Save Agent Bank
        /// </summary>
        /// <param name="context"></param>
        private static void SaveAgentBank(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.VendorManagement.AgentRegistration.SaveAgentBank(requestData, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Agent Registration");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }      

        /// <summary>
        /// Function Used To Get Agent Mapped Banks
        /// </summary>
        /// <Created By>Shihab</Created>
        /// <param name="context"></param>
        private static void GetAgentBanks(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string requestData = GetRequestString(context);
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int agentID = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["agentID"] != null)
                {
                    agentID = Convert.ToInt32(Request.Params["agentID"].ToString());
                }              
                DataTable dtBanks = BusinessLogic.VendorManagement.AgentRegistration.GetAgentBanks(objUser, 0, agentID, 1);
                string jString = ConvertDataTabletoString(dtBanks);
               
                Response.Write(jString);

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Agent Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get Bank By P_VBD_PK
        /// </summary>
        /// <Created By></Created>
        /// <param name="context"></param>
        private static void GetBankDetailsByIdAgent(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string requestData = GetRequestString(context);
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int AgentID = 0;
            int vbdPk = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["AgentID"] != null)
                {
                    AgentID = Convert.ToInt32(Request.Params["AgentID"].ToString());
                }
                if (Request.Params["P_VBD_PK"] != null)
                {
                    vbdPk = Convert.ToInt32(Request.Params["P_VBD_PK"].ToString());
                }
                DataTable dtBank = BusinessLogic.VendorManagement.AgentRegistration.GetAgentBanks(objUser, vbdPk, AgentID, 2);
                string jString = ConvertDataTabletoString(dtBank);
                Response.Write(jString);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Agent Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Delete Agent Bank
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteAgentBank(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bankId = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["P_VBD_PK"] != null)
                {
                    bankId = Convert.ToInt32((Request.Params["P_VBD_PK"].Trim()));
                }
                Response.Write(BusinessLogic.VendorManagement.AgentRegistration.DeleteAgentBank(bankId));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Agent Registration");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
      
    }
}
