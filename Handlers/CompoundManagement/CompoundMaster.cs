using System;
using System.Web;
using GTIService;
using System.Web.SessionState;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        #region methods

        /// <summary>
        /// handiling Compound Master management handlers.
        /// </summary>
        /// <param name="context"></param>
        private static void CompoundMaster(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                  
                    case "GetPolymerList":
                        Handlers.GetPolymerListCombo(context);
                        break;

                    case "GetConversionUOMList":
                        Handlers.GetConversionUOMListCombo(context);
                        break;

                    case "GetConversionFactor":
                        Handlers.GetConversionFactor(context);
                        break;

                    case "GetMaterialTypeName":
                        Handlers.GetMaterialTypeName(context);
                        break;

                    case "GetMaterialName":
                        Handlers.GetItemMaterialName(context);
                        break;

                    case "GetMaterialUOMTypeName":
                        Handlers.GetUOMTypeByMaterialPK(context);
                        break;

                    case "SaveCompoundList":
                        Handlers.SaveCompoundDtls(context);
                        break;

                    case "GetCompoundList":
                        Handlers.GetCompoundList(context);
                        break;

                    case "GetCompoundDetails":
                        Handlers.GetCompoundDtls(context);
                        break;

                    case "GetSearchValue":
                        Handlers.GetSearchValue(context);
                        break;

                    case "DeleteCompoundDtls":
                        Handlers.DeleteCompoundDtls(context);
                        break;

                    case "GetFormulationType":
                        Handlers.GetFormulationType(context);
                        break;

                        //New  Version 
                    case "GetUOMListForItemAndCompound":
                        Handlers.GetUOMsWithConversionFactorForCompd(context);
                        break;

                    case "GetUOMListForItem":
                        Handlers.GetUOMListForItem(context);
                        break;

                        //11 July 2011 2 PM
                    case "GetMaterialUOMDtls":
                        Handlers.GetMaterialUOMDtls(context);
                        break;
                            //11 July 2011 2 PM
                    case "ActivateInactivateCompoundDtls":
                        Handlers.ActivateInactivateCompoundDtls(context);
                        break;
                    
                }
            }
        }

        /// <summary>
        /// Get Material UOM Type By material PK
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMTypeByMaterialPK(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialPK = 0;
            try
            {
                if (Request.Params["MatPK"] != null)
                {
                    materialPK = Convert.ToInt32((Request.Params["MatPK"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetUomTypeByMaterialPk(materialPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Plymer lIst Details to Fill Combo
        /// </summary>
        /// <param name="context"></param>
        private static void GetPolymerListCombo(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sBU = Request.Params["SBU"] != null ? int.Parse(Request.Params["SBU"]) : 0;
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetPolymerType(sBU));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        ///  Get Conversion UOM List , list all UOM with Have Conversion factor
        /// </summary>
        /// <param name="context"></param>
        private static void GetConversionUOMListCombo(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int uOM = Request.Params["UOM"] != null && Request.Params["UOM"] != "null" ? int.Parse(Request.Params["UOM"]) : 0;
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetConversionUOMList(uOM));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        ///  Get Conversion Factor bet Ween Two UOM
        /// </summary>
        /// <param name="context"></param>
        private static void GetConversionFactor(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int uomFrm = Request.Params["UOMFrm"] != null && Request.Params["UOMFrm"] != "null" ? int.Parse(Request.Params["UOMFrm"]) : 0;
                int uomTo = Request.Params["UOMTo"] != null && Request.Params["UOMTo"] != "null" ? int.Parse(Request.Params["UOMTo"]) : 0;
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetConversionFactor(uomFrm, uomTo));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Material Item Name By Material Category. Item Categories are Raw Material, Compound and Dispersion
        /// </summary>
        /// <param name="context"></param>
        private static void GetItemMaterialName(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sBU = Request.Params["SBU"] != null ? int.Parse(Request.Params["SBU"]) : 0;
                int catg = Request.Params["CATG"] != null ? int.Parse(Request.Params["CATG"]) : 0;
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetMaterialName(sBU, catg));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get material Type Naame - 1. Raw Material, Compound and Dispersion
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialTypeName(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sBU = Request.Params["SBU"] != null ? int.Parse(Request.Params["SBU"]) : 0;
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetMaterialTypeName(sBU));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Save Compound Details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveCompoundDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string requisitionDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string requisition = BusinessLogic.CompoundManagement.CompoundManagement.SaveCompoundDtls(requisitionDetails);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(requisition);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");
            }
        }
      
        /// <summary>
        /// Gte Compound List For List Details in Grid
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompoundList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 1;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                //if (Request.Params["SBU"] != null)
                //{
                //    bizUnit = Convert.ToInt32(Request.Params["SBU"].ToString());
                //}
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetCompoundList(CommonFunctions.GetGridParams(Request), bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Delete Compound Details ActivateInactivateCompoundDtls
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteCompoundDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int requestPk = 0;
            try
            {              
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                if (Request.Params["CompID"] != null)
                {
                    requestPk = Convert.ToInt32((Request.Params["CompID"].Trim()));
                }
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.DeleteCompoundDetails(requestPk));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Activate Compound Details 
        /// </summary>
        /// <param name="context"></param>
        private static void ActivateInactivateCompoundDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int requestPk = 0;
            int status = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                if (Request.Params["CompID"] != null)
                {
                    requestPk = Convert.ToInt32((Request.Params["CompID"].Trim()));
                }
                if (Request.Params["Status"] != null)
                {
                     status = Convert.ToInt32((Request.Params["Status"].Trim()));
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.ActivateInactivateCompoundDtls(requestPk, status, objUser.PKUser));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Get Search Details For Auto Complete
        /// </summary>
        /// <param name="context"></param>
        private static void GetSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int bizUnit = 0;
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
                if (Request.Params["SBU"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["SBU"].ToString());
                }
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetSearchValues(searchBy, searchValue, bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Compound Details 
        /// </summary>
        /// <param name="context"></param>
        private static void GetCompoundDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int compoundPK = Request.Params["PK"] != null ? int.Parse(Request.Params["PK"]) : 0;
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetCompoundDtls(compoundPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get FormulatuionType Name
        /// </summary>
        /// <param name="context"></param>
        private static void GetFormulationType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sBU = Request.Params["SBU"] != null ? int.Parse(Request.Params["SBU"]) : 0;
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetFormulationTypeName(sBU));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Material UOm Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMListForItem(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialPK = 0;
            int catg = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["MatPK"] != null)
                {
                    materialPK = Convert.ToInt32((Request.Params["MatPK"].Trim()));
                }
                if (Request.Params["Catg"] != null)
                {
                    catg = Convert.ToInt32((Request.Params["Catg"].Trim()));
                }
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetUOMListForItem(materialPK, catg));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get UOM List For Selected Material and Compound UOM
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMsWithConversionFactorForCompd(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialPK = 0;
            int catg = 0;
            int uom = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["ItmPK"] != null && Request.Params["ItmPK"] != "null")
                {
                    materialPK = Convert.ToInt32((Request.Params["ItmPK"].Trim()));
                }
                if (Request.Params["Catg"] != null && Request.Params["Catg"] != "null")
                {
                    catg = Convert.ToInt32((Request.Params["Catg"].Trim()));
                }
                if (Request.Params["UOM"] != null && Request.Params["UOM"] != "null")
                {
                    uom = Convert.ToInt32((Request.Params["UOM"].Trim()));
                }
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetUOMListWithCF(materialPK, catg, uom));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        // 11 July 2011

        /// <summary>
        /// Get Material UOM by Material Catg Type and Material PK
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialUOMDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialPK = 0;
            int catg = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["ItemID"] != null)
                {
                    materialPK = Convert.ToInt32((Request.Params["ItemID"].Trim()));
                }
                if (Request.Params["CATG"] != null)
                {
                    catg = Convert.ToInt32((Request.Params["Catg"].Trim()));
                }
                Response.Write(BusinessLogic.CompoundManagement.CompoundManagement.GetMaterialUOM(materialPK, catg));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Compound Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        #endregion

    }
}
