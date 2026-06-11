using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLogic;
using BusinessObject;
using GTIService;
using Newtonsoft;

using System.Web.SessionState;
using System.Web;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        private static void DispersionPreparation(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "GetDispersionDetail":
                        Handlers.GetDispersionMaterialDetail(context);
                        break;
                    case "SaveDispersionPreparation":
                        Handlers.SaveDispersionPreparation(context);
                        break;
                    case "GetDispersionPreparationList":
                        Handlers.GetDispersionPreparationList(context);
                        break;
                    case "DeleteDispersionPreparation":
                        Handlers.DeleteDispersionPreparation(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetDispersionPreparationSearchValue(context);
                        break;
                    case "GetInspectionDetails":
                        Handlers.GetInspectionDetails(context);
                        break;
                    case "GetRawMaterialInspectionDetails":
                        Handlers.GetRawMaterialInspectionDetails(context);
                        break;
                    case "GetMaterialNameAuto":
                        Handlers.GetMaterialNameAuto(context);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetDispersionMaterialDetail(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int dispersionID = Request.Params["DispersionID"] != null ? int.Parse(Request.Params["DispersionID"]) : 0;
                int deptID = Request.Params["DepartmentID"] != null ? int.Parse(Request.Params["DepartmentID"]) : 0;
                Response.Write(BusinessLogic.Production.DispersionPreparation.GetDispersionMaterialDetail(dispersionID, deptID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveDispersionPreparation(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            //string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string dispersionDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Production.DispersionPreparation.SaveDispersionPreparation(dispersionDetails, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = GTIService.Constants.Common.ResponseTypes.JSON;
                List<object> retvals = new List<object>();
                retvals.Add("-1");
                retvals.Add("");
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(retvals));
            }
        }
        /// <summary>
        /// Get Dispersion Preparation List
        /// </summary>
        /// <param name="context"></param>
        private static void GetDispersionPreparationList(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procId = 0;
            string pageURL = string.Empty;
            try
            {
                procId = Request.Params["ProcId"] != null ? int.Parse(Request.Params["ProcId"]) : 0;
                pageURL = Request.Params["pageURL"];
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Production.DispersionPreparation.GetDispersionPreparationList(CommonFunctions.GetGridParams(Request), objUser, procId, pageURL,objUser.CurrentDeptPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Delete Dispersion Preparation
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteDispersionPreparation(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int dispersionID = Request.Params["DispersionID"] != null ? Convert.ToInt32((Request.Params["DispersionID"].Trim())) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Production.DispersionPreparation.DeleteDispersionPreparation(dispersionID));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Preparation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
        /// <summary>
        /// Get Detials For Auto Complete 
        /// </summary>
        /// <param name="context"></param>
        private static void GetDispersionPreparationSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procId = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                procId = Request.Params["ProcID"] != null ? int.Parse(Request.Params["ProcID"]) : 0;
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.Production.DispersionPreparation.GetSearchValues(searchBy, searchValue, objUser, procId));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Methode used to get inspection details corresponding to the dispersion and compound
        /// </summary>
        /// <param name="context"></param>
        private static void GetInspectionDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int batchPK = Request.Params["BatchPK"] != null ? int.Parse(Request.Params["BatchPK"]) : 0;
                int batchType = Request.Params["BatchType"] != null ? int.Parse(Request.Params["BatchType"]) : 0;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.Production.DispersionPreparation.GetInspectionDetails(batchPK, batchType, objUser.CurrentSBUPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Methode used to get inspection details corresponding to the dispersion and compound
        /// </summary>
        /// <param name="context"></param>
        private static void GetRawMaterialInspectionDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int trxPK = Request.Params["TrxPK"] != null ? int.Parse(Request.Params["TrxPK"]) : 0;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.Production.DispersionPreparation.GetRawMaterialInspectionDetails(trxPK, objUser.CurrentSBUPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// Methode used to get Materials based on type
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialNameAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int catPK = Request.Params["CATG"] != null ? int.Parse(Request.Params["CATG"]) : 0;
                string SearchVal = Request.Params["SearchValue"] != null ? Request.Params["SearchValue"] : string.Empty;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.Production.DispersionPreparation.GetMaterialNameAuto(objUser.CurrentSBUPK, catPK, -1, SearchVal));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
