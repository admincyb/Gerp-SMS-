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
        private static void TaxSettings(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {

                    // Get General Template Details By Search Option
                    case "GetTaxParameters":
                        Handlers.GetTaxParameters(context);
                        break;
                    case "SaveTaxDetails":
                        Handlers.SaveTaxDetails(context);
                        break;
                    case "DeleteTaxDetails":
                        Handlers.DeleteTaxDetails(context);
                        break;
                    case "GetTaxDetails":
                        Handlers.GetTaxDetails(context);
                        break;
                    case "GetTaxTypeDetails":
                        Handlers.GetTaxTypeDetails(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetTaxSearchValue(context);
                        break;
                    case "GetActiveTax":
                        Handlers.GetActiveTax(context);
                        break;
                    case "GetTaxCategory":
                        Handlers.GetTaxCategory(context);
                        break;
                    case "GetCfgValue":
                        Handlers.GetCfgValue(context);
                        break;
                    case "GetTaxTypeCategory":
                        Handlers.GetTaxTypeCategory(context);
                        break;

                    case "GetActiveCategoryValue":
                        Handlers.GetActiveCategoryValue(context);
                        break;
                    case "GetTaxCategoryValue":
                        Handlers.GetTaxCategoryValue(context);
                        break;
                    case "GetActiveCategoryDateValue":
                        Handlers.GetActiveCategoryDateValue(context);
                        break;
                        //Vendor Tax
                    case "GetVendorActiveCategoryDateValue":
                        Handlers.GetVendorActiveCategoryDateValue(context);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteTaxDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int taxPK = 0;
            try
            {
                if (Request.Params["TaxPK"] != null)
                {
                    taxPK = Convert.ToInt32(Request.Params["TaxPK"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.DeleteTaxDetails(taxPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Tax Settings  Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method to fill Category list to dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetTaxCategory(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnitPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnitPK = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetTaxCategory(bizUnitPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Template  Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetCfgValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnitPK = 0;
            string cfgValue = "";
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnitPK = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["CfgValue"] != null)
                {
                    cfgValue = Request.Params["CfgValue"];
                }
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetCfgValue(cfgValue,bizUnitPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Template  Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetTaxTypeCategory(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnitPK = 0;
            int isTax = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnitPK = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["IsTax"] != null)
                {
                    isTax = Convert.ToInt32(Request.Params["IsTax"]);
                }
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetTaxTypeCategory(isTax, bizUnitPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Template  Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method to fill Template Group list to dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetTaxParameters(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetTaxParameters());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Template  Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Method to Save Tax Details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveTaxDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int taxType = 0;
            try
            {
                string requestData = GetRequestString(context);

                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                if (Request.Params["TaxType"] != null)
                {
                    taxType = Convert.ToInt32(Request.Params["TaxType"]);
                }
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.SaveTaxSettings(requestData, taxType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Tax Settings  Listing");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Material List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetTaxDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetTaxDetails(CommonFunctions.GetGridParams(Request), bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetTaxTypeDetails(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int isTax = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["IsTax"] != null)
                {
                    isTax = Convert.ToInt32(Request.Params["IsTax"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetTaxTypeDetails(CommonFunctions.GetGridParams(Request),isTax, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        
        /// <summary>
        /// Search Material Details , and Get Data Related to Search Criteria
        /// </summary>
        /// <param name="context"></param>
        private static void GetTaxSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int BizUnit = 0;
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
                    BizUnit = Convert.ToInt32(Request.QueryString["SBU"].ToString());
                }
                //BizUnit = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;

                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetSearchValues(searchBy, searchValue, BizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }



        /// <summary>
        /// Get Material List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetActiveTax(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                bizUnit = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTax(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Material List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetActiveCategoryValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int categoryPK = 0;
            int taxPK = 0;
            int active = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                bizUnit = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                categoryPK = Request.Params["CategoryPK"] != null ? int.Parse(Request.Params["CategoryPK"]) : 0;
                taxPK = Request.Params["TaxPK"] != null ? int.Parse(Request.Params["TaxPK"]) : 0;
                active = Request.Params["Active"] != null ? int.Parse(Request.Params["Active"]) : 0;
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveCategoryValue(taxPK, categoryPK, bizUnit, active));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Tax category List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetTaxCategoryValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int categoryPK = 0;
            int subCategoryPK = 0;
            int active = 1;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                bizUnit = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                categoryPK = Request.Params["CategoryPK"] != null ? int.Parse(Request.Params["CategoryPK"]) : 0;
                subCategoryPK = Request.Params["SubCategoryPK"] != null ? int.Parse(Request.Params["SubCategoryPK"]) : 0;
                active = Request.Params["Active"] != null ? int.Parse(Request.Params["Active"]) : 1;
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetTaxCategoryValue(categoryPK, subCategoryPK, bizUnit, active));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetActiveCategoryDateValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int categoryPK = 0;
            int taxPK = 0;
            string specialCond = null;
            int taxDue = 0;
            DateTime taxDate;
            int active = 0;
            int IsPurchaseTax = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                bizUnit = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                categoryPK = Request.Params["CategoryPK"] != null ? int.Parse(Request.Params["CategoryPK"]) : 0;
                taxPK = Request.Params["TaxPK"] != null ? int.Parse(Request.Params["TaxPK"]) : 0;
                active = Request.Params["Active"] != null ? int.Parse(Request.Params["Active"]) : 0;
                taxDate = Request.Params["TaxDate"] != null ? DateTime.Parse(Request.Params["TaxDate"]) : DateTime.Now;
                taxDue = Request.Params["TaxDue"] != null ? int.Parse(Request.Params["TaxDue"]) : 0;

                IsPurchaseTax = Request.Params["ISPURCHASE"] != null ? int.Parse(Request.Params["ISPURCHASE"]) : 0;               
                //Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveCategoryDateValue(categoryPK, bizUnit, active, taxDate, 0, specialCond,0, IsPurchaseTax));
                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveCategoryDateValue(categoryPK, bizUnit, active, taxDate, 0, specialCond,taxDue, IsPurchaseTax));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorActiveCategoryDateValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int ITM_PK = 0;
            int taxPK = 0;
            string specialCond = null;
            int taxDue = 0;
            DateTime taxDate;
            int active = 0;
            int vendorPK = 0;
            
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                //bizUnit = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                ITM_PK = Request.Params["ITMPK"] != null ? int.Parse(Request.Params["ITMPK"]) : 0;
               // taxPK = Request.Params["TaxPK"] != null ? int.Parse(Request.Params["TaxPK"]) : 0;
                active = Request.Params["Active"] != null ? int.Parse(Request.Params["Active"]) : 0;
                taxDate = Request.Params["TaxDate"] != null ? DateTime.Parse(Request.Params["TaxDate"]) : DateTime.Now;
               // taxDue = Request.Params["TaxDue"] != null ? int.Parse(Request.Params["TaxDue"]) : 0;
                vendorPK = Request.Params["VenPk"] != null ? int.Parse(Request.Params["VenPk"]) : 0;

                Response.Write(BusinessLogic.Administration.Masters.TaxSettingsMaster.GetVendorActiveCategoryDateValue(ITM_PK, vendorPK, active, taxDate));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


    }
}
