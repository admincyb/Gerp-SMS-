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
        /// handiling Currency Management handeleres.
        /// </summary>
        /// <param name="context"></param>
        private static void CurrencyManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    
                    case "GetExchangeType":
                        Handlers.GetExchangeType(context);
                        break;
                    case "GetExchangeTypeList":
                        Handlers.GetExchangeTypeList(context);
                        break;
                    case "SaveExchangeType":
                        Handlers.SaveExchangeType(context);
                        break;
                    case "DeleteExchangeType":
                        Handlers.DeleteExchangeType(context);
                        break;
                    case "GetCurrencyMasterList":
                        Handlers.GetCurrencyMasterList(context);
                        break;
                    case "SavePage":
                        Handlers.SaveCurrencyMaster(context);
                        break;
                    case "SaveCurrencyConversion":
                        Handlers.SaveCurrencyConversion(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetCurrencyMasterSearchValues(context);
                        break;
                    case "DeleteCurrencyMasterDtls":
                        Handlers.DeleteCurrencyMasterDtls(context);
                        break;
                    case "DeleteCurrencyConversionDtls":
                        Handlers.DeleteCurrencyConversionDtls(context);
                        break;
                    case "GetCurrencyForCombo":
                        Handlers.GetCurrencyForCombo(context);
                        break;
                    case "GetCurrencyDetails":
                        Handlers.GetCurrencyDetail(context);
                        break;
                    case "GetCurrencyDetailsList":
                        Handlers.GetCurrencyDetailList(context);
                        break;
                }
            }
        }
        /// <summary>
        /// Get Exchange Type based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetExchangeType(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
           
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
               
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.GetExchangeType());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Exchange Type list based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetExchangeTypeList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
          
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.GetExchangeTypeList());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// saving exchange Type details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveExchangeType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.SaveExchangeType(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Method Used to Delete exchange Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteExchangeType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int exchangeTypeID = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);

                if (Request.Params["ExchangeMasterID"] != null)
                {
                    // Assign materialID From Request to materialID variable
                    exchangeTypeID = Convert.ToInt32((Request.Params["ExchangeMasterID"].Trim()));

                }

                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.DeleteExchangeType(exchangeTypeID));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Get CurrencyMaster List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetCurrencyMasterList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            
            try
            {
                bizUnit = Convert.ToInt32(Request.Params["bizUnit"]);
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.GetCurrencyMasterList(CommonFunctions.GetGridParams(Request),bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// saving CurrencyMaster details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveCurrencyMaster(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.SaveCurrencyMaster(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        private static void SaveCurrencyConversion(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.SaveCurrencyConversion(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Search CurrencyMaster , and Get Data Related to Search Criteria
        /// </summary>
        /// <param name="context"></param>
        private static void GetCurrencyMasterSearchValues(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int bizUnit = 0;
           
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
                if (Request.QueryString["bizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.QueryString["bizUnit"].ToString());
                }
                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.GetCurrencyMasterSearchValues(searchBy, searchValue,bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Method Used to Delete CurrencyMaster Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteCurrencyMasterDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int currencyMasterID = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);

                if (Request.Params["CurrencyMasterID"] != null)
                {
                    // Assign materialID From Request to materialID variable
                    currencyMasterID = Convert.ToInt32((Request.Params["CurrencyMasterID"].Trim()));

                }

                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.DeleteCurrencyMaster(currencyMasterID));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteCurrencyConversionDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int curConversionID = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);

                if (Request.Params["CurConversionID"] != null)
                {
                    curConversionID = Convert.ToInt32((Request.Params["CurConversionID"].Trim()));
                }
                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.DeleteCurrencyConversionDtls(curConversionID));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Method to fill Currency list to dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetCurrencyForCombo(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                if (Request.QueryString["bizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.QueryString["bizUnit"].ToString());
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.GetCurrencyForCombo(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compounding Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Currency Datails
        /// </summary>
        /// <param name="context"></param>
        private static void GetCurrencyDetail(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int currencyID = 0;
            try
            {
                if (Request.Params["CurrencyID"] != null)
                {
                    currencyID = Convert.ToInt32(Request.Params["CurrencyID"].Trim());
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.GetCurrencyDetails(currencyID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Detail");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetCurrencyDetailList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int currencyID = 0;
            try
            {
                if (Request.Params["CurrencyID"] != null)
                {
                    currencyID = Convert.ToInt32(Request.Params["CurrencyID"].Trim());
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Administration.Masters.CurrencyMaster.GetCurrencyDetailsList(CommonFunctions.GetGridParams(Request), currencyID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Currency Detail");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


    }
}
