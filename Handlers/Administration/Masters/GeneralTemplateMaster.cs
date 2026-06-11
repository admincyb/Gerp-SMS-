using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using BusinessLogic;
using BusinessLogic.Administration.Masters;
using BusinessObject;
using GTIService;
using Newtonsoft;

using System.Web.SessionState;
namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {

        /// <summary>
        /// Main handler for General Template management
        /// </summary>
        /// <param name="context"></param>
        private static void GeneralTemplateMaster(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                   
                  
                    // Get General Template Details By Search Option
                    case "SaveTemplateGroup":
                        Handlers.SaveTemplateGroup(context);
                        break;

                    // Save General Template Details
                    case "SavePage":
                        Handlers.SaveTemplateDetails(context);
                        break;


                    // Get all General Template List
                    case "GetTemplateList":
                        Handlers.GetGeneralTemplateList(context);
                        break;
                    // Get all General Template List
                    case "GetTemplateDetail":
                        Handlers.GetGeneralTemplateDetails(context);
                        break;
                    // Get all General Template List
                    case "GetPOTemplate":
                        Handlers.GetPOTemplate(context);
                        break;
                    // Delete General Template Details
                    case "DeleteTemplate":

                        Handlers.DeleteGeneralTemplateDtls(context);
                        break;

                    // Delete Template Group Details
                    case "DeleteTemplateGroup":

                        Handlers.DeleteTemplateGroupDtls(context);
                        break;
                    //Get Template Group list for filling dropdown
                    case "GetTemplateGroupCombo":
                        Handlers.GetTemplateGroupCombo(context);
                        break;
                        //Get Template Group list for filling grid
                    case "GetTemplateGroupGrid":
                        Handlers.GetTemplateGroupDetailsGrid(context);
                        break;
                        //Get auto complete search values
                    case "GetSearchValue":
                        Handlers.GetSearchValueGeneralTemplate(context);
                        break;
                    case "GetGeneralTerms":
                        Handlers.GetGeneralTemplate(context);
                        break;
                        
                }
            }
        }

        /// <summary>
        ///Save General Template details to database
        /// </summary>
        /// <param name="context"></param>
        private static void SaveTemplateDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string TemplateDetails = GetRequestString(context);
                string Pk = GeneralTemplateMasterBL.SaveTemplateDetail(TemplateDetails);
                Response.Write(Pk);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("General Template Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");

            }

        }
        /// <summary>
        ///  Save Machine Type Details 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveTemplateGroup(HttpContext context)
        {
            string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Masters.GeneralTemplateMasterBL.SaveTemplateGroup(requestData,Convert.ToInt32(user)));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("General Template Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");

            }
        }
        /// <summary>
        /// Method to fill Template Group list to dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetTemplateGroupCombo(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnitPk = 0;
            try
            {
                if (Request.Params["BizUnitPk"] != null)
                {
                    bizUnitPk = Convert.ToInt32((Request.Params["BizUnitPk"].Trim()));
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(GeneralTemplateMasterBL.GetTemplateGroupListCombo(bizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Template  Group");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method to fill Template Group list to Grid
        /// </summary>
        /// <param name="context"></param>
        private static void GetTemplateGroupDetailsGrid(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnitPk=0;
            try
            {
                if (Request.Params["BizUnitPk"] != null)
                {
                    bizUnitPk = Convert.ToInt32((Request.Params["BizUnitPk"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(GeneralTemplateMasterBL.GetTemplateGroupDtls(CommonFunctions.GetGridParams(Request),bizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Tempalte Group Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Delete Template Group Details By ID
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteTemplateGroupDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int templateGrpPK = 0;
            try
            {
                if (Request.Params["TemplateGrpPK"] != null)
                {
                    templateGrpPK = Convert.ToInt32((Request.Params["TemplateGrpPK"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(GeneralTemplateMasterBL.DeleteTemplateGroupDtls(templateGrpPK));
            }
            catch (Exception ex)
            {

              
                NLog.Logger logger = NLog.LogManager.GetLogger("Template Group");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    Response.Write("0"); //reference exists
                }
                else
                {
                    Response.Write("-3");
                }
            }
        }

        /// <summary>
        /// Method to get the General Template list 
        /// </summary>
        /// <param name="context"></param>
        private static void GetGeneralTemplateList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnitPk = 0;
            try
            {
                if (Request.Params["BizUnitPk"] != null)
                {
                    bizUnitPk = Convert.ToInt32((Request.Params["BizUnitPk"].Trim()));
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(GeneralTemplateMasterBL.GetGeneralTemplateList(CommonFunctions.GetGridParams(Request),bizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("General Template Master Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteGeneralTemplateDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int templateID = 0;
            try
            {

                if (Request.Params["TemplateID"] != null)
                {
                    templateID = Convert.ToInt32((Request.Params["TemplateID"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(GeneralTemplateMasterBL.DeleteGeneralTemplateDtls(templateID));
            }
            catch (Exception ex)
            {
                //Response.Write("-1");
                NLog.Logger logger = NLog.LogManager.GetLogger("General Template Deletion");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    Response.Write("0"); //reference exists
                }
                else
                {
                    Response.Write("-1");
                }
            }
        }

        /// <summary>
        /// Get General Template Datails
        /// </summary>
        /// <param name="context"></param>
        private static void GetGeneralTemplateDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int templateID = 0;
            try
            {
                if (Request.Params["TemplateID"] != null)
                {
                    templateID = Convert.ToInt32(Request.Params["TemplateID"].Trim());
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(GeneralTemplateMasterBL.GetGeneralTemplateDetails(templateID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("General Template Detail");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Auto complete fuctionality
        /// </summary>
        /// <param name="context"></param>
        private static void GetSearchValueGeneralTemplate(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int bizUnitPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                if (Request.Params["SearchType"] != null)
                {
                    searchBy = Request.Params["SearchType"].ToString();
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                if (Request.Params["BizUnitPk"] != null)
                {
                    bizUnitPk = Convert.ToInt32((Request.Params["BizUnitPk"].Trim()));
                }
                Response.Write(GeneralTemplateMasterBL.GetSearchValues(searchBy, searchValue,bizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("General Template Master Search");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Method to get the General Template list FOr Filling Drop Down
        /// <createdBy>Vineeth Babu</createdBy>
        /// <for>Po Creation</for>
        /// <usedin>Po listing General templates</usedin>
        /// </summary>
        /// <param name="context"></param>
        private static void GetGeneralTemplate(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int termsID = 0;
            try
            {
                int bizUnitPk =((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["TermsID"] != null)
                {
                    termsID = Convert.ToInt32(Request.Params["TermsID"].Trim());
                }
                // Get the list in json string format from BL
                Response.Write(GeneralTemplateMasterBL.GetGeneralTemplate(bizUnitPk, termsID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Template Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Method to get the General Template list FOr Filling Drop Down
        /// <createdBy>Vineeth Babu</createdBy>
        /// <for>Po Creation</for>
        /// <usedin>Po listing General templates</usedin>
        /// </summary>
        /// <param name="context"></param>
        private static void GetPOTemplate(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int termsID = 0;
            try
            {
                int bizUnitPk = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["TermsID"] != null)
                {
                    termsID = Convert.ToInt32(Request.Params["TermsID"].Trim());
                }
                // Get the list in json string format from BL
                Response.Write(GeneralTemplateMasterBL.GetPOTemplate(bizUnitPk, termsID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Template Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}



