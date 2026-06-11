using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// handiling Material management handeleres.
        /// </summary>
        /// <param name="context"></param>
        private static void TankManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    // Get all Tank List
                    case "GetTankType":
                        Handlers.GetTankType(context);
                        break;
                    case "GetTankTypeList":
                        Handlers.GetTankTypeList(context);
                        break;
                    case "SaveTankType":
                        Handlers.SaveTankType(context);
                        break;
                    case "DeleteTankType":
                        Handlers.DeleteTankType(context);
                        break;
                    case "GetTankMasterList":
                        Handlers.GetTankMasterList(context);
                        break;
                    case "SavePage":
                        Handlers.SaveTankMaster(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetTankMasterSearchValues(context);
                        break;
                    case "DeleteTankMasterDtls":
                        Handlers.DeleteTankMasterDtls(context);
                        break;
                    case "GetAllLine":
                        Handlers.GetAllLine(context);
                        break;

                    case "GetTankName":
                        Handlers.GetTankName(context);
                        break;


                }
            }
        }
        /// <summary>
        /// Get Tank Type based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetTankType(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.TankMaster.GetTankType(sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Tank Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Tank Type list based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetTankTypeList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.TankMaster.GetTankTypeList(CommonFunctions.GetGridParams(Request), sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Tank Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// saving Tank Type details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveTankType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Masters.TankMaster.SaveTankType(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Tank Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Method Used to Delete TankType Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteTankType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int tankTypeID = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);

                if (Request.Params["TankTypeID"] != null)
                {
                    // Assign materialID From Request to materialID variable
                    tankTypeID = Convert.ToInt32((Request.Params["TankTypeID"].Trim()));

                }

                Response.Write(BusinessLogic.Administration.Masters.TankMaster.DeleteTankType(tankTypeID));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Get TankMaster List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetTankMasterList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
            int statusPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["StatusPK"] != null)
                {
                    statusPk = Convert.ToInt32(Request.Params["StatusPK"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.TankMaster.GetTankMasterList(CommonFunctions.GetGridParams(Request), sbuPk, statusPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Tank Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// saving Tank Type details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveTankMaster(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Masters.TankMaster.SaveTankMaster(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Tank Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Search Material Details , and Get Data Related to Search Criteria
        /// </summary>
        /// <param name="context"></param>
        private static void GetTankMasterSearchValues(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int sbuPk = 0;
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
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                Response.Write(BusinessLogic.Administration.Masters.TankMaster.GetTankMasterSearchValues(searchBy, searchValue, sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Tank Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Method Used to Delete TankMaster Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteTankMasterDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int tankMasterID = 0;
            string lastModdate = string.Empty;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);

                if (Request.Params["TankMasterID"] != null)
                {
                    // Assign materialID From Request to materialID variable
                    tankMasterID = Convert.ToInt32((Request.Params["TankMasterID"].Trim()));

                }

                if (Request.Params["LAST_MOD_DT"] != null)
                {
                    // Assign last Mod date 
                    lastModdate = Request.Params["LAST_MOD_DT"].ToString();

                }
                Response.Write(BusinessLogic.Administration.Masters.TankMaster.DeleteTankMaster(tankMasterID, lastModdate));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Tank Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        ///Get TankName By Tank Type 
        /// </summary>
        /// <param name="context"></param>
        private static void GetTankName(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
            int tankTypePK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["TankTypePK"] != null)
                {
                    tankTypePK = Convert.ToInt32(Request.Params["TankTypePK"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.TankMaster.GetTankName(sbuPk, tankTypePK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Tank Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// To get Lines from LineMaster
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static void GetAllLine(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
            int LinePK = 0;
            int Status = 1;
            int virtualLine = -1;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["IS_VIRTUAL"] != null)
                {
                    virtualLine = Convert.ToInt32(Request.Params["IS_VIRTUAL"]);
                }
                if (Request.Params["LINEPK"] != null)
                    LinePK = Convert.ToInt32(Request.Params["LINEPK"]);
                if (Request.Params["STATUS"] != null)
                    Status = Convert.ToInt32(Request.Params["STATUS"]);
                Response.Write(BusinessLogic.Administration.Masters.TankMaster.GetAllLine(sbuPk, LinePK, Status, virtualLine));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Tank Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

    }
}
