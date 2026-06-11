using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using BusinessLogic;
using BusinessLogic.Production;
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
        /// </summary>/// <param name="context"></param>
        private static void CompoundPreparation(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    //// Get all Dispersion List
                    //case "GetCompoundBatchNo":
                    //    Handlers.GetCompoundBatchNo(context);
                    //    break;
                    //For getting autocompete search values
                    case "GetCompoundBatchNo":
                        Handlers.GetCompoundBatchNo(context);
                        break;

                    case "GetPlanListCombo":
                        Handlers.GetPlanListCombo(context);
                        break;
                    case "GetCompoundDetail":
                        Handlers.GetCompoundDetail(context);
                        break;
                    case "SaveCompoundTrxDetails":
                        Handlers.SaveCompoundTrxDetails(context);
                        break;
                    case "GetCompoundPreparationList":
                        Handlers.GetCompoundTrxList(context);
                        break;
                    case "GetBatchesForItem":
                        Handlers.GetBatchesForItem(context);
                        break;
                    case "GetCategoryItemBatch":
                        Handlers.GetCategoryItemBatch(context);
                        break;
                    case "GetStockForMaterial":
                        Handlers.GetStockForMaterial(context);
                        break;
                    case "DeleteCompoundPreparation":
                        Handlers.DeleteCompoundPreparation(context);
                        break;
                         case "GetSearchValue":
                        Handlers.GetCompoundPreparationSearchValue(context);
                        break;
                    case "GetCompoundPreparationDetails":
                        Handlers.GetCompoundPreparationDetails(context);
                        break;
                    case "GetTankListCombo":
                        Handlers.GetTankListCombo(context);
                        break;
                    case "GetTankListComboOnly":
                        Handlers.GetTankListComboOnly(context);
                        break;
                    case "GetSelectedTankDetails":
                        Handlers.GetSelectedTankDetails(context);
                        break;
        
                }

            }
        }

        /// <summary>
        /// Method to fill Compound list to dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompoundBatchNo(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetCompoundsForCombo(objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compounding Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Method to fill Plan list to dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetPlanListCombo(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetPlanListCombo(objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compounding Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method to fill Plan list to dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetTankListCombo(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetTankForCombo(objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compounding Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

         /// <summary>
        /// Method to fill Plan list to dropdown
        /// </summary>
        /// <param name="context"></param>
        private static void GetTankListComboOnly(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetTankForComboOnly(objUser.SBUID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compounding Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }



        /// <summary>
        /// Get Compound Datails
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompoundDetail(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int compoundID = 0;
            int deptId=0;
            int bizunit = 0;
            try
            {
                
                if (Request.Params["CompoundID"] != null)
                {
                    compoundID = Convert.ToInt32(Request.Params["CompoundID"].Trim()); 
                }
                if (Request.Params["DepartmentID"] != null)
                { 
                    deptId = Convert.ToInt32(Request.Params["DepartmentID"].Trim());
                }
                if (Request.Params["BizUnitPk"] != null)
                {
                    bizunit = Convert.ToInt32(Request.Params["BizUnitPk"].Trim());
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetCompoundDetails(compoundID, deptId, bizunit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compounding Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        ///Save Compound details to database
        /// </summary>
        /// <param name="context"></param>
        private static void SaveCompoundTrxDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string compoundTrx = GetRequestString(context);
                string retvals = BusinessLogic.Production.CompoundPreparationBL.SaveCompoundTrxDetails(compoundTrx, objUser);
                Response.Write(retvals);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound transaction");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");

            }

        }

        /// <summary>
        /// Get Compound List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompoundTrxList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            //int CompoundID = 0;
            //int bizUnit = 0;
            int procId = 0;
            try
            {
                var objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                procId = Request.Params["ProcId"] != null ? int.Parse(Request.Params["ProcId"]) : 0;
                
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetCompoundTrxList(CommonFunctions.GetGridParams(Request), objUser, procId));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Transaction");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }



        /// <summary>
        /// Method to Batch details for compound
        /// </summary>
        /// <param name="context"></param>
        private static void GetBatchesForItem(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int itemCat = 0;
            int itemPk = 0;
            int batchPk = 0;
            int compPK = 0;
            int CompDtlPK = 0;
            try
            {
                if (Request.Params["ItemType"] != null)
                {
                    itemCat = Convert.ToInt32(Request.Params["ItemType"].Trim());
                }
                if (Request.Params["ItemID"] != null)
                {
                    itemPk = Convert.ToInt32(Request.Params["ItemID"].Trim());
                }
                if (Request.Params["BatchPK"] != null)
                {
                    batchPk = Convert.ToInt32(Request.Params["BatchPK"].Trim());
                }
                if (Request.Params["CompPK"] != null)
                {
                    compPK = Convert.ToInt32(Request.Params["CompPK"].Trim());
                }
                if (Request.Params["CompDtlPK"] != null)
                {
                    CompDtlPK = Convert.ToInt32(Request.Params["CompDtlPK"].Trim());
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetBatchesForItem(itemCat, itemPk, batchPk, compPK, CompDtlPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compounding Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method to Batch details for compound
        /// </summary>
        /// <param name="context"></param>
        private static void GetCategoryItemBatch(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int itemCat = 0;
            int itemPk = 0;
            try
            {
                if (Request.Params["ItemType"] != null)
                {
                    itemCat = Convert.ToInt32(Request.Params["ItemType"].Trim());
                }
                if (Request.Params["ItemID"] != null)
                {
                    itemPk = Convert.ToInt32(Request.Params["ItemID"].Trim());
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetCategoryItemBatch(itemCat, itemPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compounding Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method to stock details for a material
        /// </summary>
        /// <param name="context"></param>
        private static void GetStockForMaterial(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int itemCat = 0;
            int itemPk = 0;
            int batchId = 0;
            try
            {
                if (Request.Params["ItemType"] != null)
                {
                    itemCat = Convert.ToInt32(Request.Params["ItemType"].Trim());
                }
                if (Request.Params["ItemID"] != null)
                {
                    itemPk = Convert.ToInt32(Request.Params["ItemID"].Trim());
                }
                if (Request.Params["BatchID"] != null)
                {
                    batchId = Convert.ToInt32(Request.Params["BatchID"].Trim());
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetStockValue(itemCat, itemPk, batchId));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compounding Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        private static void DeleteCompoundPreparation(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int compoundID = Request.Params["CompoundID"] != null ? Convert.ToInt32((Request.Params["CompoundID"].Trim())) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.DeleteCompopundPreparation(compoundID));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Get Detials For Auto Complete 
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompoundPreparationSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procId = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                procId = Request.Params["ProcID"] != null ? int.Parse(Request.Params["ProcID"]) : 0;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetSearchValues(searchBy, searchValue, objUser, procId));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Preparations Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Compound Datails
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompoundPreparationDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int compoundID = 0;
            try
            {
                if (Request.Params["CompoundID"] != null)
                {
                    compoundID = Convert.ToInt32(Request.Params["CompoundID"].Trim());
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetCompoundPreparationDetails(compoundID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compounding Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Method to get tank details w. r. to tank pk
        /// </summary>
        /// <param name="context"></param>
        private static void GetSelectedTankDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int tnkPK = 0, tnkType = 0, bizUnit = 0, active=0;
            try
            {
                if (Request.Params["TnkPK"] != null)
                {
                    tnkPK = Convert.ToInt32(Request.Params["TnkPK"].Trim());
                }
                if (Request.Params["TnkType"] != null)
                {
                    tnkType = Convert.ToInt32(Request.Params["TnkType"].Trim());
                }
                if (Request.Params["SBU"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["SBU"].Trim());
                }
                if (Request.Params["Active"] != null)
                {
                    active = Convert.ToInt32(Request.Params["Active"].Trim());
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.Production.CompoundPreparationBL.GetSelectedTankDetails(tnkPK,tnkType,bizUnit,active));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compounding Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}