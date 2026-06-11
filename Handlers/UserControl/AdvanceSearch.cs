using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Data;
using GTIService;


namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        private static void AdvanceSearch(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveAdvanceSearch":
                        Handlers.SaveAdvanceSearch(context);
                        break;
                    case "GetAdvanceSearchList":
                        Handlers.GetAdvanceSearchList(context);
                        break;
                    case "GetAdvanceSearchDetails":
                        Handlers.GetAdvanceSearchDetails(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Function used to save advance search details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveAdvanceSearch(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;

            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.TEXT);
                string advSrchDetails = GetRequestString(context);
                Response.Write(BusinessLogic.UserControl.AdvanceSearch.SaveAdvSearchDetails(advSrchDetails));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Advance Search");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");

            }
        }

        /// <summary>
        /// Function Used to Get Advance Search List
        /// </summary>
        /// <param name="context"></param>
        private static void GetAdvanceSearchList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int user = int.Parse(((BusinessObject.ERPPrincipal)context.User).GetUserPK());
                string pageTitle = Request.Params["PageTitle"] != null ? (Request.Params["PageTitle"].Trim()) : string.Empty;
                Response.Write(BusinessLogic.UserControl.AdvanceSearch.GetAdvanceSearchList(CommonFunctions.GetGridParams(Request), user, pageTitle));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Advance Search");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }
        /// <summary>
        /// Function Used to get advance search details by search id
        /// </summary>
        /// <param name="context"></param>
        private static void GetAdvanceSearchDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                int srchPK = Request.Params["SearchPK"] != null ? int.Parse((Request.Params["SearchPK"].Trim())) : 0;
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.UserControl.AdvanceSearch.GetAdvanceSearchDetails(srchPK));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Advance Search");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }
    }
}
