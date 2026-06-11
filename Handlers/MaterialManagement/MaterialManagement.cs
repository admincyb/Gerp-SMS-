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
        #region Methods
        /// <summary>
        /// handiling Material management handeleres.
        /// </summary>
        /// <param name="context"></param>
        private static void MaterialManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    // Get all Material List
                    case "GetMaterialList":
                        Handlers.GetMaterialList(context);
                        break;
                    case "GetMaterialCategoryByLevel":
                        Handlers.GetMaterialCategoryByLevel(context);
                        break;
                    case "GetMaterialTypeList":
                        Handlers.GetMaterialTypeList(context);
                        break;
                    case "SavePage":
                        Handlers.SaveMaterial(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetMaterialSearchValue(context);
                        break;
                    case "GetSearchTypeValues":
                        Handlers.GetSearchTypeValues(context);
                        break;
                    case "GetMaterialCodeNameByCategoryAuto":
                        Handlers.GetMaterialCodeNameByCategoryAuto(context);
                        break;
                    case "GetMaterialSearchValueByCategory":
                        Handlers.GetMaterialSearchValueByCategory(context);
                        break;
                    case "GetMaterialSearchValueByCategoryAndStore":
                        Handlers.GetMaterialSearchValueByCategoryAndStore(context);
                        break;
                    case "GetMaterialsPlantToPlant":
                        Handlers.GetMaterialsPlantToPlant(context);
                        break;
                    case "GetMaterialSearchValueByCategoryAndStoreStk":
                        Handlers.GetMaterialSearchValueByCategoryAndStoreStk(context);
                        break;
                    case "DeleteMaterial":
                        Handlers.DeleteMaterial(context);
                        break;
                    case "GetMaterials":
                        Handlers.GetMaterialName(context);
                        break;
                    case "GetMaterialUOM":
                        Handlers.GetMaterialUOM(context);
                        break;

                    case "GetItemName":
                        Handlers.GetItemName(context);
                        break;
                    case "GetMaterialByCategory":
                        Handlers.GetMaterialByCategory(context);
                        break;
                    case "GetMaterial":
                        Handlers.GetMaterial(context);
                        break;
                    case "GetRelatedMaterial":
                        Handlers.GetRelatedMaterial(context);
                        break;
                    case "GetBOMaterial":
                        Handlers.GetBOMaterial(context);
                        break;
                    case "GetMaterialCodeNameByCategory":
                        Handlers.GetMaterialCodeNameByCategory(context);
                        break;
                    case "GetMaterialByCategoryAndStore":
                        Handlers.GetMaterialByCategoryAndStore(context);
                        break;
                    case "GetMaterialByCategoryAndStoreAuto":
                        Handlers.GetMaterialByCategoryAndStoreAuto(context);
                        break;
                    case "GetUOMByMaterialPK":
                        Handlers.GetUOMByMaterialPK(context);
                        break;
                    case "GetMaterialDetails":
                        Handlers.GetMaterialDetails(context);
                        break;
                    case "GetBrandDetails":
                        Handlers.GetBrandDetails(context);
                        break;
                    case "GetPakingMaterialDetails":
                        Handlers.GetPakingMaterialDetails(context);
                        break;
                    case "GetMaterialDetailsForStore":
                        Handlers.GetMaterialDetailsForStore(context);
                        break;
                    case "GetCurrentStockForStore":
                        Handlers.GetCurrentStockForStore(context);
                        break;
                    case "SaveVendorPage":
                        Handlers.SaveMaterialVendorDetails(context);
                        break;
                    case "GetVendorDetails":
                        Handlers.GetVendorDetailsXML(context);
                        break;
                    case "GetMaterialUOMConversion":
                        Handlers.GetMaterialUOMConversion(context);
                        break;
                    case "GetUOMConvExistsByMaterial":
                        Handlers.GetUOMConvExistsByMaterial(context);
                        break;
                    case "GetDepartmentMaterial":
                        Handlers.GetDepartmentMaterial(context);
                        break;
                    case "GetDepartmentCategoryMaterial":
                        Handlers.GetDepartmentCategoryMaterial(context);
                        break;
                    case "GetMaterialDescription":
                        Handlers.GetMaterialDescription(context);
                        break;
                    case "GetRateHistory":
                        Handlers.GetRateHistory(context);
                        break;
                    case "GetItemRates":
                        Handlers.GetItemRates(context);
                        break;
                    case "GetMaterialStores":
                        Handlers.GetMaterialStores(context);
                        break;
                    case "GetBreakUpForStoreCurrentStock":
                        Handlers.GetBreakUpForStoreCurrentStock(context);
                        break;
                    case "GetBatchNo":
                        Handlers.GetBatchNo(context);
                        break;
                    case "GetBatchDetails":
                        Handlers.GetBatchDetails(context);
                        break;
                    case "GetBatchDetailsDispersion":
                        Handlers.GetBatchDetailsDispersion(context);
                        break;
                    case "CheckItemCodeExist":
                        Handlers.CheckItemCodeExist(context);
                        break;
                    case "GetBatchNoConsumption":
                        Handlers.GetBatchNo_Consumption(context);
                        break;
                    case "GetItemUOMTrading":
                        Handlers.GetItemUOMTrading(context);
                        break;
                    case "GetGSTClassficationList":
                        Handlers.GetGSTClassificationList(context);
                        break;
                    case "SaveMaterialFromPR":
                        Handlers.SaveMaterialFromPR(context);
                        break;

                }
            }
        }

        /// <summary>
        /// Function Used To Get Batch Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetBatchDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int batchId = 0;
            int GrnBatchConfg = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                batchId = Request.Params["BatchID"] != null ? Convert.ToInt32(Request.Params["BatchID"]) : 0;
                GrnBatchConfg = Request.Params["GrnBatchConfg"] != null ? Convert.ToInt32(Request.Params["GrnBatchConfg"]) : 0;
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetBatchDetails(batchId, GrnBatchConfg));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get Batch Details for Disperion category
        /// </summary>
        /// <param name="context"></param>
        private static void GetBatchDetailsDispersion(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int batchId = 0;
            int active = 0;
            int bizunit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                batchId = Request.Params["BatchID"] != null ? Convert.ToInt32(Request.Params["BatchID"]) : 0;
                active = Convert.ToInt32(Request.Params["active"]);
                bizunit = Convert.ToInt32(Request.Params["SBUPk"]);

                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetBatchDetailsDispersion(batchId, active, bizunit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get BatchNo
        /// </summary>
        /// <param name="context"></param>
        private static void GetBatchNo(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int departmentID = 0;
            int batchPK = 0;
            int IsShowZeroQtyBatches = 0;
            DateTime? date = null;
            DateTime? transDate = null;
            //int TestResult = 0;
            int TestResult = 1;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                materialID = (Request.Params["MaterialID"] != null && Request.Params["MaterialID"] != "null") ? Convert.ToInt32(Request.Params["MaterialID"]) : 0;
                departmentID = (Request.Params["DepartmentID"] != null && Request.Params["DepartmentID"] != "null") ? Convert.ToInt32(Request.Params["DepartmentID"]) : 0;
                batchPK = (Request.Params["BatchPK"] != null && Request.Params["BatchPK"] != "null") ? int.Parse(Request.Params["BatchPK"]) : 0;
                if (Request.Params["Date"] != null && Request.Params["Date"] != "null")
                {
                    date = Convert.ToDateTime(Request.Params["Date"]);
                }
                if (Request.Params["TestResult"] != null && Request.Params["TestResult"] != "null")
                {
                    TestResult = Convert.ToInt32(Request.Params["TestResult"]);
                }
                if (Request.Params["ZeroQty"] != null && Request.Params["ZeroQty"] != "null")//For Showing Zero Qty Batches
                {
                    IsShowZeroQtyBatches = Convert.ToInt32(Request.Params["ZeroQty"]);
                }
                if (Request.Params["transDate"] != null && Request.Params["transDate"] != "null")
                {
                    transDate = Convert.ToDateTime(Request.Params["transDate"]);
                }
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetBatchNo(materialID, departmentID, batchPK, date, IsShowZeroQtyBatches, transDate, TestResult));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Function Used To Get BatchNo (Consumption pages(EMI) specific SP)
        /// </summary>
        /// <param name="context"></param>
        private static void GetBatchNo_Consumption(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int departmentID = 0;
            int batchPK = 0;
            int IsShowZeroQtyBatches = 0;
            DateTime? date = null;
            DateTime? transDate = null;
            int creditdebitNotePk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                materialID = Request.Params["MaterialID"] != null ? Convert.ToInt32(Request.Params["MaterialID"]) : 0;
                departmentID = Request.Params["DepartmentID"] != null ? Convert.ToInt32(Request.Params["DepartmentID"]) : 0;
                batchPK = Request.Params["BatchPK"] != null ? int.Parse(Request.Params["BatchPK"]) : 0;
                if (Request.Params["Date"] != null)
                {
                    date = Convert.ToDateTime(Request.Params["Date"]);
                }
                if (Request.Params["ZeroQty"] != null)//For Showing Zero Qty Batches
                {
                    IsShowZeroQtyBatches = Convert.ToInt32(Request.Params["ZeroQty"]);
                }
                if (Request.Params["transDate"] != null)
                {
                    transDate = Convert.ToDateTime(Request.Params["transDate"]);
                }
                creditdebitNotePk = !string.IsNullOrEmpty(Request.Params["CDHPk"]) ? int.Parse(Request.Params["CDHPk"]) : 0;
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetBatchNo_Consumption(materialID, departmentID, batchPK, date, IsShowZeroQtyBatches, transDate, creditdebitNotePk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Function Used To Get Material Description
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialDescription(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int departmentID = 0;
            int toUOM = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                materialID = Request.Params["MaterialID"] != null ? Convert.ToInt32(Request.Params["MaterialID"]) : 0;
                departmentID = Request.Params["DepartmentID"] != null ? Convert.ToInt32(Request.Params["DepartmentID"]) : 0;
                toUOM = Request.Params["ToUOM"] != null ? Convert.ToInt32(Request.Params["ToUOM"]) : 0;
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialDescription(materialID, departmentID, toUOM));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get all Material UOM 
        /// </summary>
        /// <summary>
        /// This Function Used To Get all Material UOM have conversion
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMConvExistsByMaterial(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MaterialPK"] != null)
                {
                    materialPK = Convert.ToInt32(Request.Params["MaterialPK"]);
                }
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetUOMConvExistsByMaterial(materialPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Function Used To Get all Material UOM 
        /// </summary>
        /// <summary>
        /// This Function Used To Get all Material UOM For Dispersion Master GetMaterialType
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialUOM(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialPK = 0;
            int status = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MatID"] != null)
                {
                    materialPK = Convert.ToInt32(Request.Params["MatID"]);
                }
                if (Request.Params["status"] != null)
                {
                    status = Convert.ToInt32(Request.Params["status"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialUMODtls(materialPK, status));
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
        private static void GetMaterialList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            string Type = "1";
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["Type"] != null)
                {
                    Type = Request.Params["Type"];
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialList(CommonFunctions.GetGridParams(Request), bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Material Category, level base
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryByLevel(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            // string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            int level = 1;
            int sbuPk = 0;
            try
            {

                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["Level"] != null)
                {
                    level = Convert.ToInt32(Request.Params["Level"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialCategoryByLevel(sbuPk, level));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Sub Department");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetMaterialStores(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int bizUnit = Request.Params["SBU"] != "null" ? Convert.ToInt32(Request.Params["SBU"]) : 0;
                int itemPK = Request.Params["ItemPK"] != null ? Convert.ToInt32(Request.Params["ItemPK"]) : 0;
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialStores(itemPK, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Material Mapping");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialTypeList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int type = 1;
            int itmCategory = 0;
            int active = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["Type"] != null)
                {
                    type = Convert.ToInt32(Request.Params["Type"]);
                }
                if (Request.Params["ITMCAT"] != null)
                {
                    itmCategory = Convert.ToInt32(Request.Params["ITMCAT"]);
                }
                if (Request.Params["Active"] != null)
                {
                    active = Convert.ToInt32(Request.Params["Active"]);
                }

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialTypeList(CommonFunctions.GetGridParams(Request), type, bizUnit, active, itmCategory));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Materials name and pk
        /// </summary>
        /// <summary>
        /// This Function Used To Materials name and pk For Dispersion Master
        /// </summary> 
        /// <param name="context"></param>
        private static void GetMaterialName(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialName());
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
        private static void GetMaterialSearchValue(HttpContext context)
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
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetSearchValues(searchBy, searchValue, sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetSearchTypeValues(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int sbuPk = 0;
            int type = 1;
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
                if (Request.Params["Type"] != null)
                {
                    type = Convert.ToInt32(Request.Params["Type"]);
                }
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetSearchTypeValues(searchBy, searchValue, type, sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// To get auto list of Items(Code+Name) W.respect to  ITEM CATEGORY 
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCodeNameByCategoryAuto(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string fieldName = string.Empty;
            string searchValue = string.Empty;
            int sbuPk = 0;
            int type = 1;
            int catId = 0;
            int IsWorkOrderItem = 0;
            int BrandPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["FLDNAME"] != null)
                {
                    fieldName = Request.Params["FLDNAME"].ToString();
                }
                if (!string.IsNullOrEmpty(Request.Params["SearchType"]))
                {
                    catId = Convert.ToInt32(Request.Params["SearchType"].ToString());//Category ID
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["Type"] != null)
                {
                    type = Convert.ToInt32(Request.Params["Type"]);
                }
                if (Request.Params["IsWorkOrderItem"] != null)
                {
                    IsWorkOrderItem = Convert.ToInt32(Request.Params["IsWorkOrderItem"]);
                }
                if (Request.Params["BrandPK"] != null && Request.Params["BrandPK"] != "undefined")
                {
                    BrandPK = Convert.ToInt32(Request.Params["BrandPK"]);
                }
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialCodeNameByCategoryAuto(fieldName, searchValue, type, sbuPk, catId, IsWorkOrderItem, BrandPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Search Material Details , and Get Data Related to Search Criteria purchase request
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialSearchValueByCategory(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int categoryPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);

                if (Request.Params["SearchType"] != null)
                {
                    categoryPK = int.Parse(Request.Params["SearchType"].ToString());
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialNameSearchValues(searchValue, categoryPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Search Material Details , and Get Data Related to Search Criteria purchase request
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialSearchValueByCategoryAndStore(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int active = Request.Params["Active"] != null ? int.Parse(Request.Params["Active"].ToString()) : 0;
                int categoryPK = Request.Params["SearchType"] != null ? int.Parse(Request.Params["SearchType"].ToString()) : 0;
                int store = Request.Params["SearchCorr"] != null ? int.Parse(Request.Params["SearchCorr"].ToString()) : 0;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : "%%";
                int Alternate = Request.Params["Alt"] != null ? int.Parse(Request.Params["Alt"].ToString()) : 0;
                int ItemPK = Request.Params["ItemPK"] != null ? int.Parse(Request.Params["ItemPK"].ToString()) : 0;
                int StockExist = Request.Params["StockExist"] != null ? int.Parse(Request.Params["StockExist"].ToString()) : 0;
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialSearchValueByCategoryAndStore(searchValue, categoryPK, store, active, Alternate, ItemPK, StockExist));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetMaterialsPlantToPlant(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int active = Request.Params["Active"] != null ? int.Parse(Request.Params["Active"].ToString()) : 0;
                int categoryPK = Request.Params["SearchType"] != null ? int.Parse(Request.Params["SearchType"].ToString()) : 0;
                int store = Request.Params["Store"] != null ? int.Parse(Request.Params["Store"].ToString()) : 0;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : "%%";
                int Alternate = Request.Params["Alt"] != null ? int.Parse(Request.Params["Alt"].ToString()) : 0;
                int ItemPK = Request.Params["ItemPK"] != null ? int.Parse(Request.Params["ItemPK"].ToString()) : 0;
                int StockExist = Request.Params["StockExist"] != null ? int.Parse(Request.Params["StockExist"].ToString()) : 0;
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialsPlantToPlant(searchValue, categoryPK, store, active, Alternate, ItemPK, StockExist));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        
        /// <summary>
        /// Search Material Details , and Get Data Related to Search Criteria purchase request
        /// Show materials based on Qty available (not based on Active)
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialSearchValueByCategoryAndStoreStk(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int categoryPK = Request.Params["SearchType"] != null ? int.Parse(Request.Params["SearchType"].ToString()) : 0;
                int store = Request.Params["SearchCorr"] != null ? int.Parse(Request.Params["SearchCorr"].ToString()) : 0;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : "%%";
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialSearchValueByCategoryAndStoreStk(searchValue, categoryPK, store));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// saving Material details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveMaterial(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int departementPK = 0;
            try
            {
                if (Request.Params["DepartPK"] != null)
                {
                    departementPK = int.Parse(Request.Params["DepartPK"].ToString());
                }
                string requestData = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.SaveMaterial(requestData, objUser, departementPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Method Used to Delete material Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteMaterial(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);

                if (Request.Params["MaterialID"] != null)
                {
                    // Assign materialID From Request to materialID variable
                    materialID = Convert.ToInt32((Request.Params["MaterialID"].Trim()));

                }

                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.DeleteMaterial(materialID));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Get Item Name
        /// </summary>
        /// <param name="context"></param>
        private static void GetItemName(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.TEXT);

                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"].ToString());
                }


                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetItemName(materialID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get all Material UOM 
        /// </summary>
        /// <summary>
        /// This Function Used To Get all Material UOM For Dispersion Master
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialByCategory(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int categoryID = 0;
            int materialID = 0;
            int sbuPk = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["CategoryID"] != null)
                {
                    categoryID = Convert.ToInt32(Request.Params["CategoryID"]);
                }
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                // BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialByCategory(categoryID, materialID, sbuPk, srchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetBOMaterial(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int sbuPk = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetBOMaterial(materialID, sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetRelatedMaterial(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int sbuPk = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetRelatedMaterial(materialID, sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetMaterial(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int categoryID = 0;
            int materialID = 0;
            int sbuPk = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["CategoryID"] != null)
                {
                    categoryID = Convert.ToInt32(Request.Params["CategoryID"]);
                }
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                // BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterial(categoryID, materialID, sbuPk, srchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get all Material UOM 
        /// </summary>
        /// <summary>
        /// This Function Used To Get all Material Text and PK //([ITM_CODE])+ [ITM_NAME]
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCodeNameByCategory(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int categoryID = 0;
            int materialID = 0;
            int sbuPk = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["CategoryID"] != null)
                {
                    categoryID = Convert.ToInt32(Request.Params["CategoryID"]);
                }
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialCodeNameByCategory(categoryID, materialID, sbuPk, srchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Function Used To Get all Material UOM 
        /// </summary>
        /// <summary>
        /// This Function Used To Get all Material UOM For SRS
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialByCategoryAndStore(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int categoryID = 0;
            int materialID = 0;
            int sbuPk = 0;
            int type = 0;
            int store = 0;
            int stock = 0;
            int active = 0;
            int userPK = 0;
            int IsStoreRequest = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                userPK = objUser.PKUser;
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["Store"] != null)
                {
                    store = Convert.ToInt32(Request.Params["Store"]);
                }
                if (Request.Params["Stock"] != null)
                {
                    stock = Convert.ToInt32(Request.Params["Stock"]);
                }
                if (Request.Params["IsActive"] != null)
                {
                    active = Convert.ToInt32(Request.Params["IsActive"]);
                }
                if (Request.Params["CategoryID"] != null)
                {
                    categoryID = Convert.ToInt32(Request.Params["CategoryID"]);
                }
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                if (Request.Params["Type"] != null)
                {
                    type = Convert.ToInt32(Request.Params["Type"]);
                }
                if (Request.Params["StoreRequest"] != null)
                {
                    IsStoreRequest = Convert.ToInt32(Request.Params["StoreRequest"]);
                }
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialByCategoryAndStore(categoryID, materialID, sbuPk, type, userPK, store, stock, active, srchValue, IsStoreRequest));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetMaterialByCategoryAndStoreAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int categoryID = 0;
            int materialID = 0;
            int sbuPk = 0;
            int type = 0;
            int store = 0;
            int userPK = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                userPK = objUser.PKUser;
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["Store"] != null)
                {
                    store = Convert.ToInt32(Request.Params["Store"]);
                }
                if (Request.Params["CategoryID"] != null)
                {
                    categoryID = Convert.ToInt32(Request.Params["CategoryID"]);
                }
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                if (Request.Params["Type"] != null)
                {
                    type = Convert.ToInt32(Request.Params["Type"]);
                }
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;

                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialByCategoryAndStoreAuto(categoryID, materialID, sbuPk, type, userPK, store, srchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get UOM Details By Material PK
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMByMaterialPK(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialPK = 0;
            try
            {
                if (Request.Params["MaterialPK"] != null)
                {
                    materialPK = Convert.ToInt32((Request.Params["MaterialPK"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetUomDtlsByMaterialPk(materialPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get all Material details corresponding to material id
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int sbuPk = 0;
            int dept = 0, active = 0, vendorPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["Dept"] != null)
                {
                    dept = Convert.ToInt32(Request.Params["Dept"]);
                }
                if (Request.Params["Active"] != null)
                {
                    active = Convert.ToInt32(Request.Params["Active"]);
                }
                if (Request.Params["VendorPK"] != null)
                {
                    vendorPK = Convert.ToInt32(Request.Params["VendorPK"]);
                }
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialDetails(materialID, sbuPk, dept, active, vendorPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetBrandDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int brandPK = 0;
            //int sbuPk = 0;
            //int dept = 0, active = 0, vendorPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BrandPK"] != null)
                {
                    brandPK = Convert.ToInt32(Request.Params["BrandPK"]);
                }
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetBrandDetails(brandPK));
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
        private static void GetPakingMaterialDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int sbuPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetPakingMaterialDetails(materialID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Function Used To Get all Material details corresponding to material id
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialDetailsForStore(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int sbuPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialDetailsForStore(materialID, sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Function Used To Get current corresponding to material id
        /// </summary>
        /// <param name="context"></param>
        private static void GetCurrentStockForStore(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int store = 0;
            DateTime? date = null;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["Store"] != null)
                {
                    store = Convert.ToInt32(Request.Params["Store"]);
                }
                if (Request.Params["Date"] != null && Request.Params["Date"] != string.Empty)
                {
                    date = Convert.ToDateTime(Request.Params["Date"]);
                }
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialDetailsForStore(materialID, store, date));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Function Used To Get all Store and their current stock corresponding to material id
        /// </summary>
        /// <param name="context"></param>
        private static void GetBreakUpForStoreCurrentStock(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int deptType = 0, deptCat = 0;
            int toUOM = 0;
            DateTime? date = null;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MaterialID"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialID"]);
                }
                if (Request.Params["DeptType"] != null)
                {
                    deptType = Convert.ToInt32(Request.Params["DeptType"]);
                }
                if (Request.Params["DeptCat"] != null)
                {
                    deptCat = Convert.ToInt32(Request.Params["DeptCat"]);
                }
                if (Request.Params["PrDate"] != null)
                {
                    date = Convert.ToDateTime(Request.Params["PrDate"]);
                }
                if (Request.Params["ToUOM"] != null)
                {
                    toUOM = Convert.ToInt32(Request.Params["ToUOM"]);
                }
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetBreakupForStoreStock(materialID, deptType, deptCat, date, toUOM));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }




        /// <summary>
        /// save vendor Details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveMaterialVendorDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                string requisitionDetails = GetRequestString(context);
                string requisition = BusinessLogic.MaterialManagement.MaterialMaster.SaveMaterialVendorDetails(requisitionDetails);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(requisition);


            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }

        }
        /// <summary>
        /// Function Used To Get VendorDetails Based on ItemPk
        /// <param name="context"></param>
        private static void GetVendorDetailsXML(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int itemPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["ItemPk"] != null)
                {
                    itemPk = Convert.ToInt32(Request.Params["ItemPk"]);
                }
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetVendorMappingDetails(itemPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management ");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// handler used To Get all Material UOM Conversion Factor by passing the material and New UOM
        /// </summary>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>Po Creation</for>
        /// <Used In>Finding uom Conversion when adding material</Used>
        private static void GetMaterialUOMConversion(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialID = 0;
            int UomID = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MaterialId"] != null)
                {
                    materialID = Convert.ToInt32(Request.Params["MaterialId"]);
                }
                if (Request.Params["UOMId"] != null)
                {
                    UomID = Convert.ToInt32(Request.Params["UOMId"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialUOMConversion(materialID, UomID));
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
        private static void GetDepartmentMaterial(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchValue = string.Empty;
            string searchBy = string.Empty;
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
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetDepartmentMaterials(Convert.ToInt32(searchBy), searchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetDepartmentCategoryMaterial(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchValue = string.Empty;
            int deptPK = 0;
            int categoryPK = 0;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                deptPK = Request.Params["SearchType"] != null ? int.Parse(Request.Params["SearchType"]) : 0;
                categoryPK = Request.Params["SearchCorr"] != null ? int.Parse(Request.Params["SearchCorr"]) : 0;
                searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bizUnit = objUser.SBUID;
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetDepartmentCategoryMaterial(deptPK, categoryPK, searchValue, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        //NewMaterial start

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetRateHistory(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int itemPK = 0;
            int vendorPK = 0;
            //int bizUnit = 0;
            try
            {
                // clear all the response

                if (Request.Params["ItemPK"] != null)
                {
                    itemPK = Convert.ToInt32(Request.Params["ItemPK"]);
                }
                if (Request.Params["VendorPK"] != null)
                {
                    vendorPK = Convert.ToInt32(Request.Params["VendorPK"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetRateHistory(itemPK, vendorPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        //New End

        private static void GetItemRates(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int itemPK = 0;
            int toUOMPK = 0;
            try
            {
                // clear all the response

                if (Request.Params["ItemPK"] != null)
                {
                    itemPK = Convert.ToInt32(Request.Params["ItemPK"]);
                }
                if (Request.Params["ToUOM"] != null)//For trading
                {
                    toUOMPK = Convert.ToInt32(Request.Params["ToUOM"]);
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetItemRates(itemPK, 0, toUOMPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Check Is ItemCode already exist or not
        /// </summary>
        /// <param name="context"></param>
        private static void CheckItemCodeExist(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int ITMCODE = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                ITMCODE = Request.Params["ITMCODE"] != null ? Convert.ToInt32(Request.Params["ITMCODE"]) : 0;
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.CheckItemCodeExist(ITMCODE));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetGSTClassificationList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int PK = 0;
            int status = 0;
            try
            {
                // clear all the response

                if (Request.Params["ItemPK"] != null)
                {
                    PK = Convert.ToInt32(Request.Params["ItemPK"]);
                }
                if (Request.Params["Active"] != null)
                {
                    status = Convert.ToInt32(Request.Params["Active"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetGSTClassificationList(PK, objUser.CurrentSBUPK, status));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Common Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// saving Material details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveMaterialFromPR(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int departementPK = 0;
            try
            {
                if (Request.Params["DepartPK"] != null)
                {
                    departementPK = int.Parse(Request.Params["DepartPK"].ToString());
                }
                string requestData = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.SaveMaterialFromPR(requestData, objUser, departementPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        #endregion

        #region PR Trading
        /// <summary>
        /// Function Used To Get all Material UOM 
        /// </summary>
        /// <summary>
        /// This Function Used To Get all Material UOM have conversion
        /// </summary>
        /// <param name="context"></param>
        private static void GetItemUOMTrading(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MaterialPK"] != null)
                {
                    materialPK = Convert.ToInt32(Request.Params["MaterialPK"]);
                }
                Response.Write(BusinessLogic.MaterialManagement.MaterialMaster.GetItemUOMTrading(materialPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        #endregion
    }
}
