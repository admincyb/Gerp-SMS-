using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using GTIService;
using BusinessObject.CommonManagement;

namespace Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        #region Methods
        /// <summary>
        /// handiling Vendor management handelers.
        /// </summary>
        /// <param name="context"></param>
        private static void VendorManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    //Used for Machinery Vendor Management
                    case "GetVendor":
                        Handlers.GetVendor(context);
                        break;
                    case "SaveVendor":
                        Handlers.SaveVendor(context);
                        break;
                    case "DeleteVendor":
                        Handlers.DeleteVendor(context);
                        break;
                    case "GetVendorDetails":
                        Handlers.GetVendorDetails(context);
                        break;
                    //Used For Vendor Management
                    case "GetVendors":
                        Handlers.GetVendors(context);
                        break;

                    case "GetTypeVendors":
                        Handlers.GetTypeVendors(context);
                        break;

                    case "GetTypeVendorsActive":
                        Handlers.GetTypeVendorsActive(context);
                        break;

                    case "GetVendorAddress":
                        Handlers.GetVendorAddress(context);
                        break;

                    case "GetCurrency":
                        Handlers.GetCurrency(context);
                        break;

                    case "GetCountry":
                        Handlers.GetCountry(context);
                        break;

                    case "GetStates":
                        Handlers.GetStates(context);
                        break;

                    case "GetSearchValue":
                        Handlers.GetSearchVals(context);
                        break;

                    case "SaveVendorDetails":
                        Handlers.SaveVendorDetails(context);
                        break;

                    case "GetVendorsList":
                        Handlers.GetVendorsList(context);
                        break;

                    case "DelVendor":
                        Handlers.DeleteVendorDetails(context);
                        break;
                    case "GetVendorsByRoleAuto":
                        Handlers.GetVendorsByRoleAuto(context);
                        break;

                }
            }
        }

        #region Methods Used for Machinery Vendor Management

        /// <summary>
        /// Method Used to get the Vendor Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendor(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetVendor());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Save Vendor Details 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveVendor(HttpContext context)
        {
            string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.TEXT);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.SaveVendor(requestData, Convert.ToInt32(user)));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");

            }
        }
        /// <summary>
        /// Delete Location Details By location ID
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteVendor(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vendorID = 0;
            try
            {
                if (Request.Params["vendorID"] != null)
                {
                    vendorID = Convert.ToInt32((Request.Params["vendorID"].Trim()));
                }
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.DeleteVendorDtls(vendorID));
            }
            catch (Exception ex)
            {
               
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Get Vendor Details - For Listing
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorDetails1(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetVendorDtls(CommonFunctions.GetGridParams(Request)));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Machinery Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        #endregion

        #region Methods For Vendor Management
        ///// <summary>
        ///// To get vendors for listing
        ///// </summary>
        ///// <param name="context"></param>
        //private static void GetVendors(HttpContext context)
        //{
        //    HttpRequest Request = context.Request;
        //    HttpResponse Response = context.Response;
        //    int BizUnitPk = 0;
        //    try
        //    {
        //        if (Request.Params["BizUnitPk"] != null)
        //        {
        //            BizUnitPk = Convert.ToInt32((Request.Params["BizUnitPk"].Trim()));
        //        }
        //        // clear all the response
        //        GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
        //        Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetVendors(BizUnitPk));
        //    }
        //    catch (Exception ex)
        //    {
        //        NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
        //        logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
        //    }
        //}
   
        /// Used to get Currency
        /// </summary>
        /// <param name="context"></param>
        private static void GetCurrency(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetCurrency());
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get States for Auto complete
        /// </summary>
        /// <param name="context"></param>
        private static void GetStates(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchValue = string.Empty;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetStates(searchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Country for Auto complete
        /// </summary>
        /// <param name="context"></param>
        private static void GetCountry(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchValue = string.Empty;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetCountry(searchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Values for Auto complete
        /// </summary>
        /// <param name="context"></param>
        private static void GetSearchVals(HttpContext context)
        {
           
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SearchType"] != null)
                {
                    searchBy = Request.Params["SearchType"].ToString();
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetSearchVals(searchBy, searchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Save Vendor Details 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveVendorDetails(HttpContext context)
        {
            
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.TEXT);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.SaveVendorDetails(requestData));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");

            }
        }
        /// <summary>
        /// Get Vendor Details - For Listing
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorsList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetVendorsList(CommonFunctions.GetGridParams(Request)));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Values for Auto complete
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorsByRoleAuto(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vrmRole = 0,sbuPK=0;
            string venName = string.Empty;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VRM_ROLE"] != null)
                {
                    vrmRole = Convert.ToInt32(Request.Params["VRM_ROLE"].ToString());
                }
                if (Request.Params["SearchValue"] != null)
                {
                    venName = "%" + Request.Params["SearchValue"].ToString().Trim() + "%";
                }                
                if (Request.QueryString["SBUPk"] != null)
                {
                    sbuPK = Convert.ToInt32(Request.QueryString["SBUPk"].ToString());
                }

                Response.Write(BusinessLogic.VendorManagement.VendorMaster.GetVendorsByRoleAuto(0, vrmRole, venName, sbuPK, (byte)DbActiveStatus.ACTIVE));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        ///// <summary>
        ///// Delete Vendor Details
        ///// </summary>
        ///// <param name="context"></param>
        //private static void DeleteVendorDetails(HttpContext context)
        //{
        //    HttpRequest Request = context.Request;
        //    HttpResponse Response = context.Response;
        //    int VendorId = 0;
        //    try
        //    {
        //        if (Request.Params["VendorId"] != null)
        //        {
        //            VendorId = Convert.ToInt32((Request.Params["VendorId"].Trim()));
        //        }
        //        Response.Write(BusinessLogic.VendorManagement.VendorMaster.DeleteVendorDetails(VendorId));
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("-1");
        //        NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Management");
        //        logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
        //    }
        //}
        #endregion

        #endregion
    }
}
