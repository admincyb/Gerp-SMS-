using System;
using System.Web;

using BusinessObject;
using GTIService;
using Newtonsoft;

namespace Handlers
{
    public partial class Handlers : IHttpHandler
    {
        private static void ProjectSiteManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveProjectSiteDetails":
                        Handlers.SaveProjectSite(context);
                        break;
                    case "DeleteProjectSite":
                        Handlers.DeleteProjectSite(context);
                        break;
                    case "GetSearchValue":
                        Handlers.ProjectSiteGetSearchValue(context);
                        break;
                    case "GetProjectSiteList":
                        Handlers.GetProjectSiteList(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Function Used To save sbu details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveProjectSite(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Configurations.ProjectSiteBL.SaveProjectSite(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Project Site");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Method Used to Delete Requisition Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteProjectSite(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int ProjectID = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                if (Request.Params["ProjectID"] != null)
                {
                    // Assign requisitionID From Request to requisitionID variable
                    ProjectID = Convert.ToInt32((Request.Params["ProjectID"].Trim()));
                }
                Response.Write(BusinessLogic.Administration.Configurations.ProjectSiteBL.DeleteProjectSite(ProjectID));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Project Site");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Function Used To get all active sbu details for fill combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetProjectSiteList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int procID = 0;
            int transactionType = 1;
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
                if (Request.Params["Type"] != null)
                {
                    transactionType = Convert.ToInt32(Request.Params["Type"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Configurations.ProjectSiteBL.GetProjectSiteList(CommonFunctions.GetGridParams(Request), bizUnit, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Project Site list");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Search Requisition Details , and Get Data Related to Search Criteria
        /// </summary>
        /// <param name="context"></param>
        private static void ProjectSiteGetSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int sbuPk = 0;
            int procID = 0;
            int transactionType = 1;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"].ToString());
                }
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
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.Administration.Configurations.ProjectSiteBL.ProjectSiteGetSearchValue(searchBy, searchValue, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip List");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

    }
}
