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
        private static void DefaultValueConfig(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveDefaultValueConfig":
                        Handlers.SaveDefaultValueConfig(context);
                        break;
                    case "GetDefaultValueConfig":
                        Handlers.GetDefaultValueConfig(context);
                        break;
                    case "DeleteDefaultValueConfig":
                        Handlers.DeleteDefaultValueConfig(context);
                        break;
                    case "GetAllGroupList":
                        Handlers.GetAllGroupList(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Function Used To Save Default Values
        /// </summary>
        /// <param name="context"></param>
        private static void SaveDefaultValueConfig(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Configurations.DefaultValueConfig.SaveDefaultValueConfig(GetRequestString(context)));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("DefaultValue Config");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Function Used To Get Default Values Based on the business unit,department and group
        /// </summary>
        /// <param name="context"></param>
        private static void GetDefaultValueConfig(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sbuPK = Request.QueryString["SBUPk"] != null ? int.Parse(Request.QueryString["SBUPk"]) : 0;
                int deptPK = Request.QueryString["DeptPK"] != null ? int.Parse(Request.QueryString["DeptPK"]) : 0;
                string group = Request.QueryString["Group"] != null ? Request.QueryString["Group"].ToString() : string.Empty;
                Response.Write(BusinessLogic.Administration.Configurations.DefaultValueConfig.GetDefaultValueConfig(sbuPK, deptPK, group));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("DefaultValue Config");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get Default Values Based on the business unit,department and group
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteDefaultValueConfig(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sbuPK = Request.QueryString["SBUPk"] != null ? int.Parse(Request.QueryString["SBUPk"]) : 0;
                int deptPK = Request.QueryString["DeptPK"] != null ? int.Parse(Request.QueryString["DeptPK"]) : 0;
                string group = Request.QueryString["Group"] != null ? Request.QueryString["Group"].ToString() : string.Empty;
                Response.Write(BusinessLogic.Administration.Configurations.DefaultValueConfig.DeleteDefaultValueConfig(sbuPK, deptPK, group));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("DefaultValue Config");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To get the group in the dept. for fill auto complete
        /// </summary>
        /// <param name="context"></param>
        private static void GetAllGroupList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string group = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : "%%";
                int deptPK= Request.QueryString["SearchType"] != null ? int.Parse(Request.QueryString["SearchType"]): 0;
                Response.Write(BusinessLogic.Administration.Configurations.DefaultValueConfig.GetAllGroupList(group, deptPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("DefaultValue Config");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
