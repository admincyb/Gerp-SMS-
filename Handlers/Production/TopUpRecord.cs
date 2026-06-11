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
        /// handiling Machinery management handelers.
        /// </summary>
        /// <param name="context"></param>
        private static void TopUpManagement(HttpContext context)
        {
            /// <summary>
            /// Handles all the Machine Master  Requests
            /// </summary>
            /// <param name="context"></param>
            /// 
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {

                    case "SavePage":
                        Handlers.SaveTopUpDetails(context);
                        break;

                    case "GetTopUpDtls":
                        Handlers.GetTopUpDetails(context);
                        break;

                    case "GetTOPUPList":
                        Handlers.GetTopUpList(context);
                        break;
                  
                    case "GetSearchValue":
                        Handlers.GeTopUpSearchValue(context);
                        break;

                    case "DeleteTopUp":
                        Handlers.DeleteTopUpDtls(context);
                        break;

                    case "CheckStock":
                        Handlers.CheckStockAvailable(context);
                        break;

                    case "GetUOMList":
                        Handlers.GetUOMListWithConversionFactor(context);
                        break;

                    case "GetItemNameName":
                        Handlers.GetItemNameList(context);
                        break;


                }
            }
        }

        /// <summary>
        /// Get TopUop Details List 
        /// </summary>
        /// <param name="context"></param>
        private static void GetTopUpList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Production.TopUpRecord.GetTopUpList(CommonFunctions.GetGridParams(Request), objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("TopUp Record");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get TopUp Details Search AutoComplete
        /// </summary>
        /// <param name="context"></param>
        private static void GeTopUpSearchValue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
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
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.Production.TopUpRecord.GetSearchValues(searchBy, searchValue, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("TopUp Record");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Delete Purchase Request Details By 
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteTopUpDtls(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int requestPk = 0;
            try
            {
                if (Request.Params["PK"] != null)
                {
                    requestPk = Convert.ToInt32((Request.Params["PK"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.Production.TopUpRecord.DeleteTopUpDtls(requestPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("TopUp Record");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Save Machine Details 
        /// </summary>
        /// <param name="context"></param>
        private static void SaveTopUpDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                string machineDetails = GetRequestString(context);
                string machinePK = BusinessLogic.Production.TopUpRecord.SaveTopUpDetails(machineDetails);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(machinePK);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Top Up Record");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }

        }

        /// <summary>
        /// Get Machine Details By Machine ID
        /// </summary>
        /// <param name="context"></param>
        private static void GetTopUpDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int toupPk = 0;
            try
            {
                if (Request.Params["PK"] != null)
                {
                    toupPk = Convert.ToInt32((Request.Params["PK"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Production.TopUpRecord.GetTopUpDetails(toupPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Top Up Record");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Check stock Available Or Not
        /// </summary>
        /// <param name="context"></param>
        private static void CheckStockAvailable(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int materialPK = 0;
            int catg = 0;
            double qty=0;
            int toUOM=0;
            int topUpDtlsPk=0;
            try
            {
                if (Request.Params["ItemPk"] != null)
                {
                    materialPK = Convert.ToInt32((Request.Params["ItemPk"].Trim()));
                }
                if (Request.Params["CatgPk"] != null)
                {
                    catg = Convert.ToInt32((Request.Params["CatgPk"].Trim()));
                }
                if (Request.Params["Qty"] != null)
                {
                    qty = Convert.ToDouble((Request.Params["Qty"].Trim()));
                }
                if (Request.Params["ToUom"] != null)
                {
                    toUOM = Convert.ToInt32((Request.Params["ToUom"].Trim()));
                }
                if (Request.Params["DtlsPK"] != null)
                {
                    topUpDtlsPk = Convert.ToInt32((Request.Params["DtlsPK"].Trim()));
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Production.TopUpRecord.CheckStockAvailable(catg, materialPK,  qty,  toUOM,  topUpDtlsPk,objUser.SBUID ));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("UOM Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get UOM Nmae List
        /// </summary>
        /// <param name="context"></param>
        private static void GetUOMListWithConversionFactor(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int tankPK = 0;
            try
            {
                if (Request.Params["TankPk"] != null)
                {
                    tankPK = Convert.ToInt32((Request.Params["TankPk"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Production.TopUpRecord.GetUomList(tankPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("TopUp Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Item Name List
        /// </summary>
        /// <param name="context"></param>
        private static void GetItemNameList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int tankPK = 0;
            int sbu = 0;
            int catg = 0;
            try
            {
                if (Request.Params["SBU"] != null)
                {
                    sbu = Convert.ToInt32((Request.Params["SBU"].Trim()));
                }
                if (Request.Params["CATG"] != null)
                {
                    catg = Convert.ToInt32((Request.Params["CATG"].Trim()));
                }
                if (Request.Params["TankPk"] != null)
                {
                    tankPK = Convert.ToInt32((Request.Params["TankPk"].Trim()));
                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.Production.TopUpRecord.GetItemNameList(sbu, catg, tankPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("TopUp Master");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        #endregion

    }
}