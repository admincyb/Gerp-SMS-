using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using GTIService;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        private static void MaterialAcceptManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveMaterialAccept":
                        Handlers.SaveMaterialAccept(context);
                        break;
                    case "GetPendingSearchAuto":
                        Handlers.GetPendingSISearchAuto(context);
                        break;
                    case "GetStoreIssuePending":
                        Handlers.GetStoreIssuePending(context);
                        break;
                    case "GetMaterialAcceptList":
                        Handlers.GetMaterialAcceptList(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetMASearchValue(context);
                        break;
                    case "DeleteMaterialAccept":
                        Handlers.DeleteMaterialAccept(context);
                        break;
                    case "GetPreviousMADetailsView":
                        Handlers.GetPreviousMADetailsView(context);
                        break;
                    case "GetSTAforConvert":
                        Handlers.GetSTAforConvert(context);
                        break;
                    case "SaveSTAConversion":
                        Handlers.SaveSTAConversion(context);
                        break;
                    case "DeletSTAConversion":
                        Handlers.DeletSTAConversion(context);
                        break;


                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveMaterialAccept(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string materialAcceptDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.StoreManagement.MaterialAccept.SaveMaterialAccept(materialAcceptDetails, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = GTIService.Constants.Common.ResponseTypes.JSON;
                List<object> retvals = new List<object>();
                retvals.Add("-1");
                retvals.Add("");
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(retvals));
            }
        }

        private static void SaveSTAConversion(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string STAConversionDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.StoreManagement.MaterialAccept.SaveSTAConversion(STAConversionDetails, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Stock Transfer Accept");
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
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPendingSISearchAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                int deptPK = Request.Params["SearchCorr"] != null ? int.Parse(Request.Params["SearchCorr"]) : 0;
                int mahPK = Request.Params["SearchCorr1"] != null ? int.Parse(Request.Params["SearchCorr1"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.MaterialAccept.GetPendingSearchAuto(searchBy, searchValue, deptPK, mahPK, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetStoreIssuePending(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int mahPK = Request.Params["MAHPK"] != null ? int.Parse(Request.Params["MAHPK"]) : 0;
                int store = Request.Params["Store"] != null ? int.Parse(Request.Params["Store"]) : 0;
                int miPK = Request.Params["MIPk"] != null ? int.Parse(Request.Params["MIPk"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.MaterialAccept.GetStoreIssuePending(CommonFunctions.GetGridParams(Request), objUser.SBUID, mahPK, store, miPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetMaterialAcceptList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string pageURL = Request.Params["PageUrl"] != null ? Request.Params["PageUrl"].ToString() : string.Empty;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.MaterialAccept.GetMaterialAcceptList(CommonFunctions.GetGridParams(Request), objUser, pageURL));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        private static void GetSTAforConvert(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // string pageURL = Request.Params["PageUrl"] != null ? Request.Params["PageUrl"].ToString() : string.Empty;
                int mahPk = Request.Params["MAHPK"] != null ? Convert.ToInt32((Request.Params["MAHPK"].Trim())) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.MaterialAccept.GetSTAforConvert(mahPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetMASearchValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString() + "%" : string.Empty;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.MaterialAccept.GetMASearchValue(searchBy, searchValue, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteMaterialAccept(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int mahPk = Request.Params["MAHPK"] != null ? Convert.ToInt32((Request.Params["MAHPK"].Trim())) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.StoreManagement.MaterialAccept.DeleteMaterialAccept(mahPk));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Goods Receipt Note");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = GTIService.Constants.Common.ResponseTypes.JSON;
                List<object> retvals = new List<object>();
                retvals.Add("-1");
                retvals.Add("");
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(retvals));
            }           
        }

        private static void DeletSTAConversion(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int mahPk = Request.Params["MAHPK"] != null ? Convert.ToInt32((Request.Params["MAHPK"].Trim())) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.StoreManagement.MaterialAccept.DeletSTAConversion(mahPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Stock Transfer Accept");
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
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetPreviousMADetailsView(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int mihID = Request.Params["MIHPK"] != null ? int.Parse(Request.Params["MIHPK"]) : 0;
                Response.Write(BusinessLogic.StoreManagement.MaterialAccept.GetPreviousMADetailsView(mihID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Issue");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
    }
}
