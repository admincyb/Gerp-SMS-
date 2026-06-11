using System;
using System.Text;
using System.Web;
using BusinessObject;
using GTIService;
using Newtonsoft;

namespace Handlers
{
    public partial class Handlers : IHttpHandler
    {
        private static void DepartmentConfig(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveSBUDepartmentConfig":
                        Handlers.SaveSBUDepartmentConfig(context);
                        break;
                    case "GetSBUDepartmentConfig":
                        Handlers.GetSBUDepartmentConfig(context);
                        break;
                    case "GetSBUDeptList":
                        Handlers.GetSBUDeptList(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Function Used To save Department agianst sbu
        /// </summary>
        /// <param name="context"></param>
        private static void SaveSBUDepartmentConfig(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Configurations.DepartmentConfig.SaveSBUDepartmentConfig(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Department Config");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Function Used To Get Department Details by sbu
        /// </summary>
        /// <param name="context"></param>
        private static void GetSBUDepartmentConfig(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int sbuPK = Request.Params["SBUPk"] != null ? int.Parse(Request.Params["SBUPk"]) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Configurations.DepartmentConfig.GetSBUDepartmentConfig(sbuPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Department Config");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get Department Details For Fill Combo by sbu
        /// </summary>
        /// <param name="context"></param>
        private static void GetSBUDeptList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sbuPK = Request.Params["SBUPk"] != null ? int.Parse(Request.Params["SBUPk"]) : 0;
                Response.Write(BusinessLogic.Administration.Configurations.DepartmentConfig.GetSBUDeptList(sbuPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("SBU Configuartion");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
