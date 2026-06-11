using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using BusinessLogic;
using BusinessObject;
using GTIService;
using Newtonsoft;



namespace Handlers
{
    public partial class Handlers : IHttpHandler
    {

        private static void MaterialCategoryManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveMaterialCategory":
                        Handlers.SaveMaterialCategory(context);
                        break;
                    case "GetMaterialCategoryDtls":
                        Handlers.GetMaterialCategoryDtls(context);
                        break;
                    case "GetMaterialCategoryDtlsWithoutFG":
                        Handlers.GetMaterialCategoryDtlsWithoutFG(context);
                        break;
                    case "GetMaterialCategoryNewWithoutFG":
                        Handlers.GetMaterialCategoryNewWithoutFG(context);
                        break;
                    case "GetMaterialCategoryWithoutSemiAndFinished":
                        Handlers.GetMaterialCategoryWithoutSemiAndFinished(context);
                        break;
                    case "GetMaterialCategoryTypeWithoutSemiAndFinished":
                        Handlers.GetMaterialCategoryTypeWithoutSemiAndFinished(context);
                        break;
                    case "GetMaterialCategoryList":
                        Handlers.GetMaterialCategoryList(context);
                        break;
                    case "GetMaterialCategoryListExceptFG":
                        Handlers.GetMaterialCategoryListExceptFG(context);
                        break;
                    case "GetMaterialCategoryAutoList":
                        Handlers.GetMaterialCategoryAutoList(context);
                        break;
                    case "GetMaterialCategoryStkAutoList":
                        Handlers.GetMaterialCategoryStkAutoList(context);
                        break;
                    case "GetMaterialCategoryListWithoutSemiAndFinished":
                        Handlers.GetMaterialCategoryListWithoutSemiAndFinished(context);
                        break;
                    case "GetMaterialCategoryTypeListWithoutSemiAndFinished":
                        Handlers.GetMaterialCategoryTypeListWithoutSemiAndFinished(context);
                        break;
                    case "GetMaterialCategoryRawMaterialList":
                        Handlers.GetMaterialCategoryRawMaterialList(context);
                        break;
                    case "GetMaterialCategoryListAuto":
                        Handlers.GetMaterialCategoryListAuto(context);
                        break;
                    case "GetMaterialCategoryListExcepetFGAuto":
                        Handlers.GetMaterialCategoryListExcepetFGAuto(context);
                        break;
                    case "GetMaterialCategoryListDeptAuto":
                        Handlers.GetMaterialCategoryListDeptAuto(context);
                        break;
                    case "GetPlantToPlantMaterialCategoryAuto":
                        Handlers.GetPlantToPlantMaterialCategoryAuto(context);
                        break;
                    case "GetMaterialCategory":
                        Handlers.GetMaterialCategory(context);
                        break;
                    case "GetUOMNameByCategory":
                        Handlers.GetUOMByCategory(context);
                        break;
                    case "DeleteMaterialCategory":
                        Handlers.DeleteMaterialCategory(context);
                        break;
                    case "GetGrnTypes":
                        Handlers.GetGrnTypes(context);
                        break;
                    case "GetMaterialCategoryByType":
                        Handlers.GetMaterialCategoryByType(context);
                        break;
                    case "GetConstantMaster":
                        Handlers.GetConstantMaster(context);
                        break;
                    case "GetMaterialCategoryAccounts":
                        Handlers.GetMaterialCategoryAccounts(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Function Used To Save Material Category 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveMaterialCategory(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.SaveMaterialCategory(requestData));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Function Used To Get all Material Category 
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int materialCategoryParentPK = Request.Params["MatCagID"] != null ? Convert.ToInt32(Request.Params["MatCagID"]) : 0;
                int sbuPK = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                int itcValue = Request.Params["ITCPK"] != null ? Convert.ToInt32(Request.Params["ITCPK"]) : 0;
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryDtls(materialCategoryParentPK, sbuPK, itcValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
            /// <summary>
        /// Function Used To Get all Material Category except fg for tree
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryDtlsWithoutFG(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int materialCategoryParentPK = Request.Params["MatCagID"] != null ? Convert.ToInt32(Request.Params["MatCagID"]) : 0;
                int sbuPK = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryDtlsWithoutFG(materialCategoryParentPK, sbuPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Function Used To Get all Material Category except fg for tree
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryNewWithoutFG(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int materialCategoryParentPK = Request.Params["MatCagID"] != null ? Convert.ToInt32(Request.Params["MatCagID"]) : 0;
                int sbuPK = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                int itemType = Request.Params["Type"] != null ? Convert.ToInt32(Request.Params["Type"]) : 0;
                int store = Request.Params["Store"] != null ? Convert.ToInt32(Request.Params["Store"]) : 0;

                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryDtlsWithoutFG(materialCategoryParentPK, sbuPK, itemType,store));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
         /// <summary>
        /// Function Used To Get all Material Category 
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryWithoutSemiAndFinished(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int materialCategoryParentPK = Request.Params["MatCagID"] != null ? Convert.ToInt32(Request.Params["MatCagID"]) : 0;
                int sbuPK = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryWithoutSemiAndFinished(materialCategoryParentPK, sbuPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryTypeWithoutSemiAndFinished(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int materialCategoryParentPK = Request.Params["MatCagID"] != null ? Convert.ToInt32(Request.Params["MatCagID"]) : 0;
                int type = Request.Params["Type"] != null ? Convert.ToInt32(Request.Params["Type"]) : 1;
                int sbuPK = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryTypeWithoutSemiAndFinished(materialCategoryParentPK,type, sbuPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        
        /// <summary>
        /// Function Used To Get Details Material Category By Catogory ID
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategory(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialCategoryPK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MatCagID"] != null)
                {
                    materialCategoryPK = Convert.ToInt32(Request.Params["MatCagID"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategory(materialCategoryPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get all Material Category To Bind Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk=0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryList(sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Function Used To Get all Material Category exept fg To Bind Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryListExceptFG(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
            int itemType = 0;
            int store = 0;
            int showSFG = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["ItemType"] != null)
                {
                    itemType = Convert.ToInt32(Request.Params["ItemType"]);
                }
                if (Request.Params["Store"] != null)
                {
                    store = Convert.ToInt32(Request.Params["Store"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                if (Request.Params["SFG"] != null)
                {
                    showSFG = Convert.ToInt32(Request.Params["SFG"]);
                }
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryListExceptFG(sbuPk, itemType, store, showSFG, srchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get all Material Category exept fg To Bind Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryAutoList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
            int itemType = 0;
            int store = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["ItemType"] != null)
                {
                    itemType = Convert.ToInt32(Request.Params["ItemType"]);
                }
                if (Request.Params["Store"] != null && Request.Params["Store"] != "null")
                {
                    store = Convert.ToInt32(Request.Params["Store"]);
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
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryAutoList(sbuPk, itemType, store, srchValue));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get all Material Category exept fg To Bind Combo
        /// Show materials based on Qty available (not based on Active)
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryStkAutoList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
            int itemType = 0;
            int store = 0;
            int stock = 1;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["ItemType"] != null)
                {
                    itemType = Convert.ToInt32(Request.Params["ItemType"]);
                }
                if (Request.Params["Store"] != null && Request.Params["Store"] != "null")
                {
                    store = Convert.ToInt32(Request.Params["Store"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                if (Request.Params["Stock"] != null)
                {
                    stock = Convert.ToInt32(Request.Params["Stock"]);
                }
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryStkAutoList(sbuPk, itemType, store, srchValue,stock));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get all Material Category To Bind Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryListWithoutSemiAndFinished(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk=0;
            int ITCVAL=0;
            int includeFG = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                if (Request.Params["ITCVAL"] != null)
                {
                    ITCVAL = Convert.ToInt32(Request.Params["ITCVAL"]);
                }
                if (Request.Params["IncludeFG"] != null)
                {
                    includeFG = Convert.ToInt32(Request.Params["IncludeFG"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryListWithoutSemiAndFinished(sbuPk, ITCVAL, includeFG));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetMaterialCategoryTypeListWithoutSemiAndFinished(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk=0;
            int type = 1;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["Type"] != null)
                {
                    type = Convert.ToInt32(Request.Params["Type"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryTypeListWithoutSemiAndFinished(sbuPk,type));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Function Used To Get raw Material Category To Bind Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryRawMaterialList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int sbuPk = 0;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                else
                {
                    sbuPk = objUser.SBUID;
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryRawMaterialList(sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Function Used To Get all Material Category To Bind Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryListAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string srhcType = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                int sbuPk = Request.Params["SearchType"] != null ? Convert.ToInt32(Request.Params["SearchType"]) : 0;
                int type = Request.Params["Type"] != null ? Convert.ToInt32(Request.Params["Type"]) : 0;
                int PM_WorkOrder = Request.Params["PM_WorkOrder"] != null ? Convert.ToInt32(Request.Params["Type"]) : 0;
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryListAuto(sbuPk, srhcType,type,PM_WorkOrder));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// Function Used To Get all Material Category Except Finished Good and Semi Finished Good To Bind Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryListDeptAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                int sbuPk = Request.Params["SearchType"] != null ? Convert.ToInt32(Request.Params["SearchType"]) : 0;
                int IsStock = Request.Params["IsStock"] != null ? Convert.ToInt32(Request.Params["IsStock"]) : -1;
                int deptPK = Request.Params["DeptPK"] != null ? Convert.ToInt32(Request.Params["DeptPK"]) : 0;
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryListDeptAuto(sbuPk,deptPK, srchValue, IsStock));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void GetPlantToPlantMaterialCategoryAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                int sbuPk = Request.Params["SearchType"] != null ? Convert.ToInt32(Request.Params["SearchType"]) : 0;
                int IsStock = Request.Params["IsStock"] != null ? Convert.ToInt32(Request.Params["IsStock"]) : -1;
                int deptPK = Request.Params["DeptPK"] != null ? Convert.ToInt32(Request.Params["DeptPK"]) : 0;
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetPlantToPlantMaterialCategoryAuto(sbuPk,deptPK, srchValue, IsStock));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get all Material Category Except Finished Good and Semi Finished Good To Bind Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialCategoryListExcepetFGAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string srhcType = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                int sbuPk = Request.Params["SearchType"] != null ? Convert.ToInt32(Request.Params["SearchType"]) : 0;
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryListExcepetFGAuto(sbuPk, srhcType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To Get UOM Name By Category to bind combo
        /// Function refered sajeer,
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMByCategory(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialCategoryPK = 0;
            int sbuPk = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MatCagID"] != null)
                {
                    materialCategoryPK = Convert.ToInt32(Request.Params["MatCagID"]);
                }
                if (Request.Params["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetUOMByCategory(materialCategoryPK, sbuPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method Used to Delete Material Category Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteMaterialCategory(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int ctgID = 0;
            try
            {
                if (Request.Params["MatCagID"] != null)
                {
                    ctgID = Convert.ToInt32((Request.Params["MatCagID"].Trim()));
                }
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.DeleteMaterialCategory(ctgID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Method Used to Get Material GRN Types
        /// </summary>
        /// <param name="context"></param>
        private static void GetGrnTypes(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int bizUnit = 0;
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32((Request.Params["BizUnit"].Trim()));
                }

                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);               
                //BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetGrnTypes(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetMaterialCategoryByType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int materialCategoryParentPK = Request.Params["MatCagID"] != null ? Convert.ToInt32(Request.Params["MatCagID"]) : 0;
                int sbuPK = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                int itemType = Request.Params["Type"] != null ? Convert.ToInt32(Request.Params["Type"]) : 0;
                int store = Request.Params["Store"] != null ? Convert.ToInt32(Request.Params["Store"]) : 0;
                int showSFG = Request.Params["SFG"] != null ? Convert.ToInt32(Request.Params["SFG"]) : 0;                

                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryByType(materialCategoryParentPK, sbuPK, itemType, store,showSFG));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Data's from ADM_CONST_MST
        /// </summary>
        /// <param name="context"></param>
        private static void GetConstantMaster(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                bizUnit = Request.QueryString["BizUnit"] != null ? Convert.ToInt32(Request.QueryString["BizUnit"].ToString()) : 0;
                int CGTVALUE = Request.Params["CGTVALUE"] != null ? int.Parse(Request.Params["CGTVALUE"].ToString()) : 1;
                int CNG_VALUE = Request.Params["CNGVALUE"] != null ? int.Parse(Request.Params["CNGVALUE"]) : 1;
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetConstMstValues(0, 0, (BusinessObject.CommonManagement.ConstGroupType)CGTVALUE, 1, 1, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetMaterialCategoryAccounts(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int materialCategoryParentPK = Request.Params["MatCagID"] != null ? Convert.ToInt32(Request.Params["MatCagID"]) : 0;
                Response.Write(BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetMaterialCategoryAccounts(materialCategoryParentPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Category Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
