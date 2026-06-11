using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using BusinessLogic;
using BusinessLogic.DispersionManagement;
using BusinessObject;
using GTIService;
using Newtonsoft;

using System.Web.SessionState;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// Main handler for dispersion manaagement
        /// </summary>
        /// <param name="context"></param>
        private static void DispersionManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    // Get all Dispersion List
                    case "GetDispersionList":
                        Handlers.GetDispersionList(context);
                        break;
                    //For getting autocompete search values
                    case "GetSearchValue":
                        Handlers.GetSearchValueDispersion(context);
                        break;

                    // Get Dispersion Details By Search Option
                    case "GetDispersionDetail":
                        Handlers.GetDispersionDetail(context);
                        break;

                    // Save Dispersion Details
                    case "SavePage":
                        Handlers.SaveDispersionDetails(context);
                        break;

                    // Delete Dispersion Details
                    case "DeleteDispersion":

                        Handlers.DeleteDispersion(context);
                        break;
                    //Get Dispersion list for filling dropdown
                    case "GetDispersionListCombo":
                        Handlers.GetDispersionListCombo(context);
                        break;
                    case "GetDispersionAuto":
                        Handlers.GetDispersionForAuto(context);
                        break;
                    case "GetDispersionTypes":
                        Handlers.GetDispersionTypes(context);
                        break;                    
                }
            }
        }

        /// <summary>
        /// Get Dispersion Datails
        /// </summary>
        /// <param name="context"></param>
        private static void GetDispersionDetail(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int DispersionID = 0;
            int Dept = 0;
            try
            {
                if (Request.Params["DispersionID"] != null)
                {
                    DispersionID = Convert.ToInt32(Request.Params["DispersionID"].Trim());
                    Dept = Request.Params["DepartmentID"] != null ? int.Parse(Request.Params["DepartmentID"]) : 0;
                }
               
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(DispersionMaster.GetDispersionDetails(DispersionID, Dept));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Detail");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method to get the dispersion list 
        /// </summary>
        /// <param name="context"></param>
        private static void GetDispersionList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                if (Request.QueryString["SBUPk"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.QueryString["SBUPk"].ToString());
                }
                Response.Write(BusinessLogic.DispersionManagement.DispersionMaster.GetDispersionList(CommonFunctions.GetGridParams(Request),bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        ///Save dispersion details to database
        /// </summary>
        /// <param name="context"></param>
        private static void SaveDispersionDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.TEXT);
                string DispersionDetails = GetRequestString(context);
                string DispersionPk = BusinessLogic.DispersionManagement.DispersionMaster.SaveDispersionDetails(DispersionDetails);
                Response.Write(DispersionPk);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteDispersion(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int DispersionID = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                if (Request.Params["DispersionID"] != null)
                {
                    DispersionID = Convert.ToInt32((Request.Params["DispersionID"].Trim()));
                }

                Response.Write(BusinessLogic.DispersionManagement.DispersionMaster.DeleteDispersion(DispersionID));
            }
            catch (Exception ex)
            {
                Response.Write("-1");
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Auto complete fuctionality
        /// </summary>
        /// <param name="context"></param>
        private static void GetSearchValueDispersion(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int sBu = 0;
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
                if (Request.QueryString["SBU"] != null)
                {
                    sBu = Convert.ToInt32(Request.QueryString["SBU"].ToString());
                }

                Response.Write(BusinessLogic.DispersionManagement.DispersionMaster.GetSearchValues(searchBy, searchValue, sBu));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Method to fill dispersion list to dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetDispersionListCombo(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.DispersionManagement.DispersionMaster.GetDispersionListCombo(1));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetDispersionForAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                //string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                int dept = Request.QueryString["Dept"] != null && Request.QueryString["Dept"] != string.Empty ? Convert.ToInt32(Request.QueryString["Dept"]) : 0;
                // int searchCorr = Request.Params["SearchCorr"] != null ? int.Parse(Request.Params["SearchCorr"]) : 0;
                Response.Write(BusinessLogic.DispersionManagement.DispersionMaster.GetDispersionForAuto(searchValue, objUser, dept));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method to get the dispersion list 
        /// </summary>
        /// <param name="context"></param>
        private static void GetDispersionTypes(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                if (Request.QueryString["SBUPk"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.QueryString["SBUPk"].ToString());
                }               
                Response.Write(BusinessLogic.DispersionManagement.DispersionMaster.GetDispersionTypes(Convert.ToInt32(Request.QueryString["cfgPK"]),Request.QueryString["cfgType"].ToString(),Convert.ToInt32(Request.QueryString["Active"]),bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Type Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

          /// <summary>
        /// Method to get the dispersion list 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPrefixConfiguration(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                if (Request.QueryString["SBUPk"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.QueryString["SBUPk"].ToString());
                }               
                Response.Write(BusinessLogic.DispersionManagement.DispersionMaster.GetDispersionTypes(Convert.ToInt32(Request.QueryString["cfgPK"]),Request.QueryString["cfgType"].ToString(),Convert.ToInt32(Request.QueryString["Active"]),bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Type Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
