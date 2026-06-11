using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ERP.Utilities;
using System.Web.Script.Serialization;
using System.Data;
using BusinessObject.AccountManagement;

namespace ERPSMS_v01.Handlers
{
    /// <summary>
    /// Summary description for DataHandler
    /// </summary>
    public class DataHandler : IHttpHandler, IRequiresSessionState
    {

        string SearchType;
        string SearchBy;
        BusinessObject.User currentUser;
        HttpRequest request;
        HttpResponse response;
        public void ProcessRequest(HttpContext context)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            GetFieldValues(context);
        }
        #region GetFieldValues
        /// <summary>
        /// get field values
        /// </summary>
        /// <param name="context"></param>
        private void GetFieldValues(HttpContext context)
        {
            AutoEnum currAutoEnum;
            request = context.Request;
            response = context.Response;
            if (request.Params[RequestParameters.SearchType] != null)
            {
                SearchType = request.Params[RequestParameters.SearchType].Trim().ToString();
            }
            if (request.Params[RequestParameters.SearchBy] != null)
            {
                SearchBy = HttpUtility.HtmlEncode(request.Params[RequestParameters.SearchBy].Trim().ToString());
            }

            currAutoEnum = (AutoEnum)Enum.Parse(typeof(AutoEnum), SearchType.ToUpper());
            response.Clear();
            response.Cache.SetNoServerCaching();
            response.Cache.SetNoStore();
            //response.ContentType = "text/plain";
            response.ContentType = "application/json; charset=utf-8";
            switch (currAutoEnum)
            {
                case AutoEnum.SUMMARY:
                    GetSummaryDetails(SearchBy);
                    break;
                case AutoEnum.SHPWEIGHTDTL:
                    int soPK=0;
                    string itemCode=string.Empty;
                    double qty=0;
                    int sodPk = 0;
                    if (request.Params["qty"] != null)
                    {
                        qty =double.Parse(request.Params["qty"].Trim().ToString());
                    }
                    if (request.Params["soPK"] != null)
                    {
                        soPK = int.Parse(request.Params["soPK"].Trim().ToString());
                    }
                    if (request.Params["sodPK"] != null)
                    {
                        sodPk = int.Parse(request.Params["sodPK"].Trim().ToString());
                    }
                    GetShippingPlanWeightDetails(soPK, itemCode, qty, sodPk);

                    break;
                case AutoEnum.ASSETTYPE:
                    GetAssetType(SearchBy);
                    break;
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Methord used to get Summary Details
        /// </summary>
        public void GetSummaryDetails(string SearchBy)
        {
            DataTable dtPage = BusinessLogic.AccountManagement.WorkflowInboxBL.GetInboxSummary(Convert.ToInt32(SearchBy));
            SummaryInfo objSummery = new SummaryInfo();
            if (dtPage != null && dtPage.Rows.Count > 0 && !string.IsNullOrEmpty(dtPage.Rows[0]["refSummary"].ToString()))
            {
                objSummery = CommonFunctions.XmlDeserialize<SummaryInfo>(dtPage.Rows[0]["refSummary"].ToString());
            }
            response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(objSummery));
        }
        /// <summary>
        /// Get Shipping Plan Weight Details
        /// </summary>
        /// <param name="soPk"></param>
        /// <param name="itemCode"></param>
        /// <param name="qty"></param>
        /// <param name="sodPk"></param>
        public void GetShippingPlanWeightDetails(int soPk, string itemCode, double qty, int sodPk = 0)
        {
            try
            {
                response.ClearContent();
                DataTable dtResult = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanWeightDetails(soPk, itemCode, qty, sodPk);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dtResult));
                }
                else
                    response.Write(null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {

            }
        }


        public void GetAssetType(string assetPK)
        {
            try
            {
                response.ClearContent();
                DataTable dtResult = BusinessLogic.AssetService.ServiceRequestBL.GetAssetType(currentUser.SBUID,assetPK,Convert.ToInt32(CommonConstants.HASPK));
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(dtResult));
                }
                else
                    response.Write(null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {

            }
        }


        #endregion

    }
    #region AutoEnum
    public enum AutoEnum
    {
        SUMMARY,
        SHPWEIGHTDTL,
        ASSETTYPE
    }
    #endregion
}