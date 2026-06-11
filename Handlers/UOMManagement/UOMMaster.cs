using System;
using System.Web;
using System.Web.SessionState;
using GTIService;

namespace Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        #region Methods

        /// <summary>
        /// handiling UOM Master management handlers.
        /// </summary>
        /// <param name="context"></param>
        private static void UOMMaster(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "GetUOMType":
                        Handlers.GetUOMType(context);
                        break;

                    case "GetUOMTypeList":
                        Handlers.GetUOMTypeList(context);
                        break;

                    case "GetUnit":
                        Handlers.GetUOM(context);
                        break;

                    case "GetUOMList":
                        Handlers.GetUOMList(context);
                        break;

                    case "SaveUOM":
                        Handlers.SaveUOM(context);
                        break;

                    case "SaveUOMType":
                        Handlers.SaveUOMType(context);
                        break;

                    case "DeleteUOM":
                        Handlers.DeleteUOM(context);
                        break;

                    case "DeleteUOMType":
                        Handlers.DeleteUOMType(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetUOMSearchValue(context);
                        break;
                        
                    case "GetUOMByMaterialPK":
                        Handlers.GetUOMByMaterialPK(context);
                        break;
                    case "GetUOMDtls":
                        Handlers.GetUOMDtls(context);
                        break;
                    case "GetUOMByType":
                        Handlers.GetUOMByType(context);
                        break;
                    case "GetUOMCONVUOMPK":
                        Handlers.GetUOMConversionsByUOMPK(context);
                        break;
                        
                        
                }
            }
        }

        /// <summary>
        /// Used for searching
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
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
                if (Request.Params["SBU"] != null)
                {
                    bizUnit = Convert.ToInt32( Request.Params["SBU"].ToString());
                }
                Response.Write(BusinessLogic.UOMManagement.UOMMaster.GetUOMSearchValues(searchBy, searchValue, bizUnit));
               
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
       
        /// <summary>
        /// Used to get UOM Type
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMType(HttpContext context)
        {
            
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                // clear all the response
                int sbuPK = Request.Params["SBUPk"] != null ? Convert.ToInt32(Request.Params["SBUPk"]) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.UOMManagement.UOMMaster.GetUOMType(sbuPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Used to get UOM Corresponding to a type
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOM(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                //int uomPk =  Convert.ToInt32(Request.Params["UOMPK"]);
                int uomPk = Request.Params["UOMPK"] != null ? Convert.ToInt32(Request.Params["UOMPK"]) : 0;
                int sbu = Request.Params["SBU"] != null ? Convert.ToInt32(Request.Params["SBU"]) : ((BusinessObject.User)(HttpContext.Current.User.Identity)).SBUID;
                string uomTypeName = Request.Params["UOMTypeName"] != null ? Request.Params["UOMTypeName"] : string.Empty;
                Response.Write(BusinessLogic.UOMManagement.UOMMaster.GetUOM(Request.Params["UOMTypeID"] != null ? Request.Params["UOMTypeID"].Trim() : string.Empty, uomPk, sbu, uomTypeName));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Used to get UOM Corresponding to a type
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMByType(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int uomPk = 0;
            try
            {
                if (Request.Params["UOMPK"] != "null")
                {
                    uomPk = Convert.ToInt32(Request.Params["UOMPK"]);
                }

                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sbu = 0;
                string uomTypeName = string.Empty;
                //int uomPk =  Convert.ToInt32(Request.Params["UOMPK"]);
                // int uomPk = Request.Params["UOMPK"] != null ? Convert.ToInt32(Request.Params["UOMPK"]) : 0;
                Response.Write(BusinessLogic.UOMManagement.UOMMaster.GetUOM(Request.Params["UOMTypeID"] != null ? Request.Params["UOMTypeID"].Trim() : string.Empty, uomPk, sbu, uomTypeName));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Used to save UOM
        /// </summary>
        /// <param name="context"></param>
        private static void SaveUOM(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string uOMDetails = GetRequestString(context);
                string uomPK = BusinessLogic.UOMManagement.UOMMaster.SaveUOM(uOMDetails);
                
            
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(uomPK);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Used to save UOM Type
        /// </summary>
        /// <param name="context"></param>
        private static void SaveUOMType(HttpContext context)
        {
            //Getting UserPk from HttpContext
            string user = ((BusinessObject.User)(HttpContext.Current.User.Identity)).PKUser.ToString();
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string requestData = GetRequestString(context);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.UOMManagement.UOMMaster.SaveUOMType(requestData, Convert.ToInt32(user)));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");

            }
        }
      
        /// <summary>
        /// Used to Delete UOM
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteUOM(HttpContext context)
        {
            int UOMId = 0;
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params["UOMId"] != null)
            {
                UOMId = int.Parse(Request.Params["UOMId"].Trim());
            }
            GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
            try
            {
                Response.Write(BusinessLogic.UOMManagement.UOMMaster.DeleteUOM(UOMId));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);

                 if (ex.Message.Contains("REFERENCE constraint"))
                    Response.Write("0");
                else

                    Response.Write("-1");
              
            }
        }

        /// <summary>
        /// Used to Get UOM Details to display
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {

                bizUnit = Convert.ToInt32(Request.Params["bizUnit"]);
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.UOMManagement.UOMMaster.GetUOMList(CommonFunctions.GetGridParams(Request), bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Used to get UOM Type To display in Popup
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMTypeList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                bizUnit = Convert.ToInt32(Request.Params["bizUnit"]);
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.UOMManagement.UOMMaster.GetUOMTypeList(CommonFunctions.GetGridParams(Request), bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Used to delete UOM Type
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteUOMType(HttpContext context)
        {
            int UOMTypeId = 0;
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params["UOMTypeId"] != null)
            {
                UOMTypeId = int.Parse(Request.Params["UOMTypeId"].Trim());
            }
            GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
            try
            {
                Response.Write(BusinessLogic.UOMManagement.UOMMaster.DeleteUOMType(UOMTypeId));
            }
            catch (Exception ex)
            {


                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                if (ex.Message.Contains("REFERENCE constraint"))
                    Response.Write("0");
                else

                    Response.Write("-1");
            }
           
        }
               
        /// <summary>
        /// Get UOM Details By UOMID
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int uOMPK = 0;
            try
            {
                if (Request.Params["uOMPK"] != null)
                {
                    uOMPK = Convert.ToInt32((Request.Params["uOMPK"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.UOMManagement.UOMMaster.GetUOMDetails(uOMPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        ///  Get given UOM conversion factered UOM's.
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMConversionsByUOMPK(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int uOMPK = 0;
            try
            {
                if (Request.Params["UOMId"] != null)
                {
                    uOMPK = Convert.ToInt32((Request.Params["UOMId"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.UOMManagement.UOMMaster.GetUOMConversionsByUOMPK(uOMPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        #endregion
    }
}
