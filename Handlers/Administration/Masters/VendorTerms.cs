using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Web.SessionState;

using BusinessLogic;
using BusinessObject;
using GTIService;
using Newtonsoft;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// handiling Vendor Evaluation Management handeleres.
        /// </summary>
        /// <param name="context"></param>
        /// 
        private static void VendorTermsManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    //Get vendor term list to fill grid
                    case "GetVenderTermsList":
                        Handlers.GetVendorTermsList(context);
                        break;
                        //Save Vendor Term Details
                    case "SavePage":
                        Handlers.SaveVendorTerms(context);
                        break;
                        //Get vendor Term detail --not used
                    case "GetVenderTermDetails":
                        Handlers.GetVenderTermDetails(context);
                        break;
                        //Delete vendor term details
                    case "Delete":
                        Handlers.DeleteVendorTerms(context);
                        break;
                        //Get autocomplete search
                    case "GetSearchValue":
                        Handlers.GetSearchValueVenderTerms(context);
                        break;
                    case "GetVenderTerm":
                        Handlers.GetVendorTerms(context);
                        break;
                    
                }
            }


        }

        /// <summary>
        /// Method to get vendor term details
        /// </summary>
        /// <param name="context"></param>
        private static void GetVenderTermDetails(HttpContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to save vendor terms
        /// </summary>
        /// <param name="context"></param>
        private static void SaveVendorTerms(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string VendorTerm = GetRequestString(context);
                string TermPK = BusinessLogic.Administration.Masters.VendorTermsMaster.SaveVendorTermsDetails(VendorTerm);
                Response.Write(TermPK);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Term Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");

            }

        }

        /// <summary>
        /// Method to get vendor terms List
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorTermsList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnitPk=0;
            try
            {

                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnitPk = objUser.SBUID;

                if (Request.Params["BizUnitPk"] != null)
                {
                    bizUnitPk = Convert.ToInt32((Request.Params["BizUnitPk"].Trim()));

                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.VendorTermsMaster.GetVendorTermsList(CommonFunctions.GetGridParams(Request), bizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Term Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method Used to Delete material Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteVendorTerms(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int termID = 0;
            try
            {

                if (Request.Params["TermID"] != null)
                {
                    // Assign materialID From Request to materialID variable
                    termID = Convert.ToInt32((Request.Params["TermID"].Trim()));

                }

                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                Response.Write(BusinessLogic.Administration.Masters.VendorTermsMaster.DeleteVendorTerms(termID));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Term Master");
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
        /// Auto complete fuctionality
        /// </summary>
        /// <param name="context"></param>
        private static void GetSearchValueVenderTerms(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int bizUnitPk=0;
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
                if (Request.QueryString["BizUnitPk"] != null)
                {
                    bizUnitPk = Convert.ToInt32(Request.QueryString["BizUnitPk"].ToString());
                }

                Response.Write(BusinessLogic.Administration.Masters.VendorTermsMaster.GetSearchValues(searchBy, searchValue, bizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Terms Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Get Vendor Terms
        /// </summary>
        /// <param name="context"></param>
        private static void GetVendorTerms(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vendorID = 0;
            int termsID = 0;
            try
            {                  
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["TermsId"] != null)
                {
                    termsID = Convert.ToInt32(Request.Params["TermsId"].Trim());
                }
                if (Request.Params["VendorId"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorId"].Trim());
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Administration.Masters.VendorTermsMaster.GetVendorTerms(vendorID, termsID));
               
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Terms Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
