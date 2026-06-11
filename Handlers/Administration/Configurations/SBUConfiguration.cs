using System;
using System.Web;

using BusinessObject;
using GTIService;
using Newtonsoft;

namespace Handlers
{
    public partial class Handlers : IHttpHandler
    {
        private static void SBUConfiguartion(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveSBUConfig":
                        Handlers.SaveSBUConfig(context);
                        break;
                    case "ActiveSBUConfig":
                        Handlers.SBUConfigActive(context);
                        break;
                    case "GetSBUConfig":
                        Handlers.SBUConfigGet(context);
                        break;
                    case "GetAllSBUList":
                        Handlers.GetAllSBUList(context);
                        break;
                    case "GetSBUConfigDetails":
                        Handlers.GetSBUConfigDetails(context);
                        break;
                    case "GetSBUFooterDetails":
                        Handlers.GetSBUFooterDetails(context);
                        break;
                }
            }
        }

      
        /// <summary>
        /// Get Currency Datails
        /// </summary>
        /// <param name="context"></param>
        private static void GetSBUConfigDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int SBUID = 0;
            try
            {
                if (Request.Params["SBUID"] != null)
                {
                    SBUID = Convert.ToInt32(Request.Params["SBUID"].Trim());
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Configurations.SBUConfiguartion.GetSBUConfigDetails(SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SBU config Detail");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Function Used To save sbu details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveSBUConfig(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Configurations.SBUConfiguartion.SaveSBUConfig(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SBU Configuartion");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Function Used To active / inactive sbu 
        /// </summary>
        /// <param name="context"></param>
        private static void SBUConfigActive(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int sbuPK = Request.Params["SBUPk"] != null ? int.Parse(Request.Params["SBUPk"]) : 0;
                int userPK = Convert.ToInt32(((BusinessObject.ERPPrincipal)context.User).GetUserPK());
                int active = Request.Params["Active"] != null ? int.Parse(Request.Params["Active"]) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Configurations.SBUConfiguartion.ActiveSBUConfig(sbuPK, userPK, active));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SBU Configuartion");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Function Used To get sbu details
        /// </summary>
        /// <param name="context"></param>
        private static void SBUConfigGet(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Configurations.SBUConfiguartion.GetSBUConfig(CommonFunctions.GetGridParams(Request)));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SBU Configuartion");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To get all active sbu details for fill combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetAllSBUList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Configurations.SBUConfiguartion.GetAllSBU());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SBU Configuartion");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        
        /// <summary>
        /// Get Footer Datails
        /// </summary>
        /// <param name="context"></param>
        private static void GetSBUFooterDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;           
            try
            {                
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Configurations.SBUConfiguartion.GetSBUFooterDetails());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SBU footer Detail");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
