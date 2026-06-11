using System;
using System.Web;

using BusinessObject;
using GTIService;
using Newtonsoft;
using System.Data;

namespace Handlers
{
    public partial class Handlers : IHttpHandler
    {
        private static void UserManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                   //case "SaveProjectSiteDetails":
                    //    Handlers.SaveProjectSite(context);
                    //    break;
                    case "DeleteUser":
                        Handlers.DeleteUser(context);
                        break;
                    case "GetSearchValue":
                        Handlers.UsersListSearchValue(context);
                        break;
                    case "GetUsersList":
                        Handlers.GetUsersList(context);
                        break;
                    case "GetUserRoles":
                        Handlers.GetUserRoles(context);
                        break;
                }
            }
        }

 
        /// <summary>
        /// Function Used To save sbu details
        /// </summary>
        /// <param name="context"></param>
        //private static void SaveProjectSite(HttpContext context)
        //{
        //    HttpRequest Request = context.Request;
        //    HttpResponse Response = context.Response;
        //    try
        //    {
        //        string requestData = GetRequestString(context);
        //        GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
        //        Response.Write(BusinessLogic.Administration.Configurations.ProjectSiteBL.SaveProjectSite(requestData));
        //    }
        //    catch (Exception ex)
        //    {
        //        NLog.Logger logger = NLog.LogManager.GetLogger("Project Site");
        //        logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
        //        Response.Write("-1");
        //    }
        //}

        /// <summary>
        /// Method Used to Delete Requisition Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteUser(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int UserID = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                if (Request.Params["UserID"] != null)
                {
                    // Assign requisitionID From Request to requisitionID variable
                    UserID = Convert.ToInt32((Request.Params["UserID"].Trim()));
                }
                Response.Write(BusinessLogic.Administration.Configurations.UserManagementBL.DeleteUser(UserID));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("User Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Function Used To get all active sbu details for fill combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetUsersList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int IsActive = 0;
            int procID = 0;
            int transactionType = 1;
            int userType = 0;//1. 
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["IsActive"] != null)
                {
                    IsActive = Convert.ToInt32(Request.Params["IsActive"]);
                }
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["Type"] != null)
                {
                    transactionType = Convert.ToInt32(Request.Params["Type"]);
                }
                if (Request.Params["UserType"] != null)
                {
                    userType = Convert.ToInt32(Request.Params["UserType"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Configurations.UserManagementBL.GetUsersList(CommonFunctions.GetGridParams(Request), bizUnit,IsActive, objUser, userType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("User Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Search Requisition Details , and Get Data Related to Search Criteria
        /// </summary>
        /// <param name="context"></param>
        private static void UsersListSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int sbuPk = 0;
            int procID = 0;
            int userType = 0;


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
                if (Request.QueryString["UserType"] != null)
                {
                    userType = Convert.ToInt32(Request.QueryString["UserType"].ToString());
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.Administration.Configurations.UserManagementBL.UsersListGetSearchValue(searchBy, searchValue, objUser, userType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("User Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetUserRoles(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int userPk = 0;
            try
            {
                // clear all the response

                if (Request.Params["UserPk"] != null)
                {
                    userPk = Convert.ToInt32(Request.Params["UserPk"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Configurations.UserManagementBL.GetUserRoles(userPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

    }
}
