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

        #region methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void StockAdjustmentManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SaveStockAdjustment":
                        Handlers.SaveStockAdjustment(context);
                        break;
                    case "GetStockAdjustmentList":
                        Handlers.GetStockAdjustmentList(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Methord used to save stock Adjustment Details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveStockAdjustment(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {

                string sADetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string sAID = BusinessLogic.StoreManagement.StockAdjustment.SaveStockAdjustmentDetails(sADetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(sAID);


            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Stock Adjustment");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private static void GetStockAdjustmentList(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procID = 0;
            string PageUrl = "";
            try
            {
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["PageUrl"] != null)
                {
                    PageUrl = Request.Params["PageUrl"];
                }

                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.StoreManagement.StockAdjustment.GetStockAdjustmentList(CommonFunctions.GetGridParams(Request), objUser, procID, PageUrl));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Stock Adjustment Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        #endregion
    }

}