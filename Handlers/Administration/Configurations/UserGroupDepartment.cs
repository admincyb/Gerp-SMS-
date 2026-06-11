using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Handlers
{
    public partial class Handlers : IHttpHandler
    {
        private static void UserGroupDepartment(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveUserGroupDepartment":
                        Handlers.SaveUserGroupDepartment(context);
                        break;
                    case "DeleteUserGroupDepartment":
                        Handlers.DeleteUserGroupDepartment(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Function Used To Save Default Values
        /// </summary>
        /// <param name="context"></param>
        private static void SaveUserGroupDepartment(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Configurations.UserGroupDepartment.SaveUserGroupDepartment(GetRequestString(context)));
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
        private static void DeleteUserGroupDepartment(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sbuPK = Request.QueryString["SBUPk"] != null ? int.Parse(Request.QueryString["SBUPk"]) : 0;
                int group = Request.QueryString["Group"] != null ? int.Parse(Request.QueryString["Group"]) : 0;
                Response.Write(BusinessLogic.Administration.Configurations.UserGroupDepartment.DeleteUserGroupDepartment(sbuPK, group));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("DefaultValue Config");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
