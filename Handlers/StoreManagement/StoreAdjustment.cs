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
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void StoreAuditAdjustmentManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveStoreAdjustment":
                        Handlers.SaveStoreAdjustmentDtls(context);
                        break;
                    case "GetStoreAdjustment":
                        Handlers.SaveStoreAdjustmentDtls(context);
                        break;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
      
        private static void SaveStoreAdjustmentDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string requisitionDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string requisition = BusinessLogic.StoreManagement.StoreAdjustment.SaveStoreAuditAdjustmentDetails(requisitionDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(requisition);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Adjustment");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");
            }
        }
       
    }
}