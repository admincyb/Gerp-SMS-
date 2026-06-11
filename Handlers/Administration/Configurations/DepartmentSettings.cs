using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Handlers
{
    public partial class Handlers : IHttpHandler
    {
        private static void DepartmentSettings(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveConfigValue":
                        Handlers.SaveConfigValue(context);
                        break;
                    case "GetConfigValue":
                        Handlers.GetConfigValue(context);
                        break;
                    case "DeleteConfigValue":
                        Handlers.DeleteConfigValue(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Function Used To save Department config values
        /// </summary>
        /// <param name="context"></param>
        private static void SaveConfigValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Configurations.DepartmentSettings.SaveConfigValue(GetRequestString(context)));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("DefaultValue Config");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Function Used To get Department config values based on the dept and sbu
        /// </summary>
        /// <param name="context"></param>
        private static void GetConfigValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sbuPK = Request.QueryString["SBUPk"] != null ? int.Parse(Request.QueryString["SBUPk"]) : 0;
                int deptPK = Request.QueryString["DeptPK"] != null ? int.Parse(Request.QueryString["DeptPK"]) : 0;
                Response.Write(BusinessLogic.Administration.Configurations.DepartmentSettings.GetConfigValue(sbuPK, deptPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("DefaultValue Config");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To delete Department config values based on the dept and sbu
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteConfigValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sbuPK = Request.QueryString["SBUPk"] != null ? int.Parse(Request.QueryString["SBUPk"]) : 0;
                int deptPK = Request.QueryString["DeptPK"] != null ? int.Parse(Request.QueryString["DeptPK"]) : 0;
                Response.Write(BusinessLogic.Administration.Configurations.DepartmentSettings.DeleteConfigValue(sbuPK, deptPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("DefaultValue Config");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
