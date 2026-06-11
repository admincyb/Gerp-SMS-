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
        /// handiling Vendor Evaluation Management handleres.
        /// </summary>
        /// <param name="context"></param>
        private static void VendorEvaluationManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    // Get all Evaluation List
                    case "GetEvaluationList":
                        Handlers.GetEvaluationList(context);
                        break;
                        //save Evaluation details -xml
                    case "SavePage":
                        Handlers.SaveEvaluation(context);
                        break;
                        //Get autocomplete search
                    case "GetEvaluationSearchValue":
                        Handlers.GetEvaluationSearchValue(context);
                        break;
                        //Delete Evaluation 
                    case "DeleteEvaluation":
                        Handlers.DeleteEvaluation(context);
                        break;

                    case "GetEvaluations":
                        Handlers.GetEvaluations(context);
                        break;
                        //NewEval Start
                    case "GetEvalGroups":
                        Handlers.GetEvalGroups(context);
                        break;
                        //New End
                    case "GetParameterValue":
                        Handlers.GetParameterValue(context);
                        break;
                    case "GetParameters":
                        Handlers.GetParameters(context);
                        break;
                    case "GetSuppliedMaterial":
                        Handlers.GetSuppliedMaterial(context);
                        break;
                    case "GetEvalDetail":
                        Handlers.GetEvalDetails(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetEvaluationSearchValue(context);
                        break;
                }
            }


        }
        /// <summary>
        /// Get Evaluation List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetEvaluationList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vendorID = 0;
            int bizUnit = 0;
            int procID = 0;
            string PageUrl = "";
            try
            {
                bizUnit = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;

                if (Request.Params["VendorID"] != null)
                {
                    // Assign VendorID From Request to VendorID variable
                    vendorID = Convert.ToInt32((Request.Params["VendorID"].Trim()));

                }
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["PageUrl"] != null)
                {
                    PageUrl = Request.Params["PageUrl"];
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorEvaluation.GetEvaluationList(CommonFunctions.GetGridParams(Request), vendorID, bizUnit, procID, PageUrl));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Method Used to Delete Evaluation Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteEvaluation(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int evaluationID = 0;
            try
            {

                if (Request.Params["EvaluationID"] != null)
                {
                    // Assign materialID From Request to materialID variable
                    evaluationID = Convert.ToInt32((Request.Params["EvaluationID"].Trim()));

                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorEvaluation.DeleteEvaluations(evaluationID));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// save evaluation Details - XML
        /// </summary>
        /// <param name="context"></param>
        private static void SaveEvaluation(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int vendorID=0;
            bool isDraft = false;
            try
            {
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Convert.ToInt32(Request.Params["VendorID"].Trim());
                }
                //if (Request.Params["IsDraft"] != null) //flag indicating whether it is saved as Draft
                //{
                //    isDraft = Convert.ToInt16(Request.Params["IsDraft"].Trim())==0?false:true;
                //}
                string evaluationDetails = GetRequestString(context);
                string evaluationPk = BusinessLogic.VendorManagement.VendorEvaluation.SaveEvaluationDetails(evaluationDetails,vendorID);
               

                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(evaluationPk);


            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }

        }
        /// <summary>
        /// Get General Template Datails
        /// </summary>
        /// <param name="context"></param>
        private static void GetEvalDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int templateID = 0;
            try
            {
                if (Request.Params["EvalID"] != null)
                {
                    templateID = Convert.ToInt32(Request.Params["EvalID"].Trim());
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.VendorManagement.VendorEvaluation.GetEvaluationDetail(templateID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Template Detail");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Search evaluation Details , and Get Data Related to Search Criteria
        /// </summary>
        /// <param name="context"></param>
        private static void GetEvaluationSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int bizunitPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                bizunitPk = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                if (Request.Params["SearchType"] != null)
                {
                    searchBy = Request.Params["SearchType"].ToString();
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                

                Response.Write(BusinessLogic.VendorManagement.VendorEvaluation.GetSearchValues(searchBy, searchValue, bizunitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Function Used To Get all evaluation
        /// </summary>
        /// <param name="context"></param>
        private static void GetEvaluations(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string parameterID="";
           
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["ParameterID"] != null)
                {
                    parameterID = Request.Params["ParameterID"].ToString();
                }
              
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorEvaluation.GetEvaluations(parameterID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Function Used To Get list of materials supplied by a vendor
        /// </summary>
        /// <param name="context"></param>
        private static void GetSuppliedMaterial(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string vendorID = "";

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["VendorID"] != null)
                {
                    vendorID = Request.Params["VendorID"].ToString();
                }

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorEvaluation.GetSuppliedMaterial(vendorID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Function Used To Get all parameters To Bind Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetParameters(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnitPk = 0;
            int templatePK = 0;
            try
            {
                bizUnitPk = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                
                if (Request.Params["TemplatePK"] != null)
                {
                    templatePK = Convert.ToInt32((Request.Params["TemplatePK"].Trim()));
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorEvaluation.GetParametersList(bizUnitPk,templatePK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        //NewEval Start
        /// <summary>
        /// Function Used To Get all parameters To Bind Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetEvalGroups(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnitPk = 0;
            try
            {
                bizUnitPk = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorEvaluation.GetEvalGroupList(bizUnitPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        //New End

        /// <summary>
        /// Function Used To Get all parameters To Bind Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetParameterValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnitPk = 0;
            int tmdPK = 0;
            try
            {
                bizUnitPk = ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["TmdPK"] != null)
                {
                    tmdPK = Convert.ToInt32((Request.Params["TmdPK"].Trim()));
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.VendorManagement.VendorEvaluation.GetParameterValue(tmdPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Vendor Evaluation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

    }
}
