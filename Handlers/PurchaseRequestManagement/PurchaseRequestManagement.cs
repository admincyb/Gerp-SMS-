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
        private static void PurcahseRequestManagement(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {
                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {
                    case "SavePurchaseRequestList":
                        Handlers.SavePurchaseRequestList(context);
                        break;
                    case "SaveMaterialRequest":
                        Handlers.SaveMaterialRequestDetails(context);
                        break;
                    case "SaveMaterialRequestPlantToPlant":
                        Handlers.SaveMaterialRequestPlantToPlant(context);
                        break;
                    case "GetPurchaseRequestList":
                        Handlers.GetPurchaseRequestList(context);
                        break;
                    case "GetPurchaseRequest":
                        Handlers.GetPurchaseRequest(context);
                        break;
                    case "PackingMaterials":
                        Handlers.PackingMaterials(context);
                        break;
                    case "CustomerItemRequestGet":
                        Handlers.CustomerItemRequestGet(context);
                        break;
                    case "GetIONumber":
                        Handlers.GetIONumber(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GePurchaseRequestSearchValue(context);
                        break;
                    case "DeletePurchaseRequest":
                        Handlers.DeletePurchaseRequest(context);
                        break;
                    //Purchase Request Trading list
                    case "GetPurchaseRequestTradingList":
                        Handlers.GetPurchaseRequestTradingList(context);
                        break;
                    case "DeletePurchaseRequestTradingList":
                        Handlers.DeletePurchaseRequestTradingList(context);
                        break;
                    case "GetSearchValueTradingList":
                        Handlers.GePurchaseRequestTrdListSearchValue(context);
                        break;
                    //Purchase Request Trading
                    case "GetPurchaseRequestTrading":
                        Handlers.GetPurchaseRequestTrading(context);
                        break;
                    case "SavePurchaseRequestTradingList":
                        Handlers.SavePurchaseRequestTradingList(context);
                        break;
                    case "GetIONumberAuto":
                        Handlers.GetIONumberAuto(context);
                        break;
                    case "GetCostCenter":
                        Handlers.GetCostCenter(context);
                        break;
                    case "GetMaterialSearchValue":
                        Handlers.GeMaterialRequestSearchValue(context);
                        break;
                    case "ValidateItemStock":
                        Handlers.ValidateItemStock(context);
                        break;

                }
            }
        }

        /// <summary>
        /// Get Detials For Auto Complete 
        /// </summary>
        /// <param name="context"></param>
        private static void GeMaterialRequestSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null && Request.Params["SearchType"] != "null" ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = string.Empty;
                if (Request.QueryString["SearchValue"] != null && Request.QueryString["SearchValue"] != "null")
                    searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString().Trim() + "%" : "%%";
                else
                    searchValue = Request.Params["SearchValue"] != null ? "%" + Request.Params["SearchValue"].ToString().Trim() + "%" : "%%";
                int processPK = Request.QueryString["ProcessPK"] != null && Request.QueryString["ProcessPK"] != "null" ? int.Parse(Request.QueryString["ProcessPK"]) : 0;
                string pageUrl = Request.Params["PageUrl"] != null && Request.Params["PageUrl"] != "null" ? Request.Params["PageUrl"] : string.Empty;
                int MenuType = Request.QueryString["MenuType"] != null && Request.QueryString["MenuType"] != "null" ? int.Parse(Request.QueryString["MenuType"]) : 0;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetMaterialSearchValues(searchBy, searchValue, processPK, objUser, pageUrl, MenuType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        private static void ValidateItemStock(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                
                int Dept = !string.IsNullOrEmpty(Request.QueryString["DeptPk"]) && Request.QueryString["DeptPk"] != "null" ? int.Parse(Request.QueryString["DeptPk"]) : 0;
                int MaterialPK = !string.IsNullOrEmpty(Request.QueryString["MaterialPK"]) && Request.QueryString["MaterialPK"] != "null" ? int.Parse(Request.QueryString["MaterialPK"]) : 0;
                decimal Qty = !string.IsNullOrEmpty(Request.QueryString["Qty"]) && Request.QueryString["Qty"] != "null" ? decimal.Parse(Request.QueryString["Qty"]) : 0;
                
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.ValidateItemStock(Dept, MaterialPK, Qty));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Validare Item Stock");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetIONumberAuto(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                int bizUnit = Request.Params["BizUnit"] != null ? int.Parse(Request.Params["BizUnit"]) : 0;
                int prhPK = Request.Params["PrhPK"] != null ? int.Parse(Request.Params["PrhPK"]) : 0;
                int sohPK = Request.Params["SohPK"] != null ? int.Parse(Request.Params["SohPK"]) : 0;
                int deptPk = Request.Params["DeptPk"] != null ? int.Parse(Request.Params["DeptPk"]) : 0;
                int isGlove = Request.Params["isGlove"] != null ? int.Parse(Request.Params["isGlove"]) : 0;
                int showAll = Request.Params["ShowAll"] != null ? int.Parse(Request.Params["ShowAll"]) : 0;
                int IsDispatched = Request.Params["IsDispatched"] != null ? int.Parse(Request.Params["IsDispatched"]) : 1; 
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.GetIONumberAuto(bizUnit, prhPK, sohPK, srchValue, deptPk, isGlove, showAll,IsDispatched));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To save purchase request
        /// </summary>
        /// <param name="context"></param>
        private static void SavePurchaseRequestList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string requisitionDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.SavePurchaseRequestDetails(requisitionDetails, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = GTIService.Constants.Common.ResponseTypes.JSON;
                List<object> retvals = new List<object>();
                retvals.Add("-1");
                retvals.Add("");
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(retvals));
            }
        }

        private static void SaveMaterialRequestDetails(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string requisitionDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.SaveMaterialRequestDetails(requisitionDetails, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Request");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = GTIService.Constants.Common.ResponseTypes.JSON;
                List<object> retvals = new List<object>();
                retvals.Add("-1");
                retvals.Add("");
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(retvals));
            }
        }

        private static void SaveMaterialRequestPlantToPlant(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string requisitionDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.SaveMaterialRequestPlantToPlant(requisitionDetails, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Material Request");
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
        /// To Get Purchase Request Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseRequestList(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procID = 0;
            int TransactionStatus = -1;
            string pageUrl = "";
            string prNo = string.Empty;
            string ioNo = string.Empty;
            string ItemName = string.Empty;
            int reqStore = 0;
            int reqDept = 0;
            string reqBy = string.Empty;
            int FilterStatus = 0;
            int cmpPk = 0;
            byte orderGroup = 1;
            try
            {
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["PageUrl"] != null)
                {
                    pageUrl = Request.Params["PageUrl"];
                }
                if (Request.Params["TranStatus"] != null && Request.Params["TranStatus"] != "null")
                {
                    TransactionStatus = Convert.ToInt32(Request.Params["TranStatus"]);
                }


                //------------------------------------------------------------
                if (Request.Params["reqStore"] != null && Request.Params["reqStore"] != string.Empty)
                {
                    reqStore = Convert.ToInt32((Request.Params["reqStore"].Trim()));
                }
                if (Request.Params["ioNo"] != null)
                {
                    ioNo = Request.Params["ioNo"].Trim();
                }
                if (Request.Params["ItmName"] != null)
                {
                    ItemName = Request.Params["ItmName"].Trim();
                }
                if (Request.Params["reqDept"] != null && Request.Params["reqDept"] != string.Empty)
                {
                    reqDept = Convert.ToInt32((Request.Params["reqDept"].Trim()));
                }
                if (Request.Params["reqBy"] != null)
                {
                    reqBy = Request.Params["reqBy"].Trim();
                }

                if (Request.Params["FilterStatus"] != null)
                {
                    FilterStatus = Convert.ToInt32((Request.Params["FilterStatus"].Trim()));
                }
                if (Request.Params["prNo"] != null)
                {
                    prNo = Request.Params["prNo"].Trim();
                }
                if (Request.Params["CMP_PK"] != null && Request.Params["CMP_PK"] != "null")
                {
                    cmpPk = Convert.ToInt32(Request.Params["CMP_PK"]);
                }
                if (Request.Params["Service"] != null)
                {
                    orderGroup = Convert.ToByte(Request.Params["Service"]);
                }

                //------------------------------------------------------------

                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                //Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetPurchaseRequestList(CommonFunctions.GetGridParams(Request), objUser, pageUrl, procID, TransactionStatus));
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetPurchaseRequestList(CommonFunctions.GetGridParams(Request), objUser, pageUrl, procID, TransactionStatus, reqStore, ioNo, ItemName, reqDept, reqBy, FilterStatus, prNo, cmpPk));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// To Get Purchase Request Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseRequest(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int bizUnit = Request.Params["BizUnit"] != null && Request.Params["BizUnit"] != "null" ? Convert.ToInt32(Request.Params["BizUnit"]) : 0;
                int dept = Request.Params["Dept"] != null && Request.Params["Dept"] != "null" ? Convert.ToInt32(Request.Params["Dept"]) : 0;
                int type = Request.Params["Type"] != null && Request.Params["Type"] != "null" ? Convert.ToInt32(Request.Params["Type"]) : 0;
                int prPK = Request.Params["PRPK"] != null && Request.Params["PRPK"] != "null" ? Convert.ToInt32(Request.Params["PRPK"]) : 0;
                int rowCount = Request.Params["RCount"] != null && Request.Params["RCount"] != "null" && Request.Params["RCount"] != string.Empty ? Convert.ToInt32(Request.Params["RCount"]) : 0;
                int itemCategory = Request.Params["ItmCat"] != null && Request.Params["ItmCat"] != "null" && Request.Params["ItmCat"] != string.Empty ? Convert.ToInt32(Request.Params["ItmCat"]) : 0;
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.GetPurchaseRequest(bizUnit, dept, type, prPK, rowCount, itemCategory));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Customer Item Request Get
        /// </summary>
        /// <param name="context"></param>
        private static void CustomerItemRequestGet(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sohPK = Request.Params["SohPK"] != null ? int.Parse(Request.Params["SohPK"]) : 0;
                int prkPK = Request.Params["PrhPK"] != null ? int.Parse(Request.Params["PrhPK"]) : 0;
                int type = Request.Params["Type"] != null && Request.Params["Type"] != "null" ? int.Parse(Request.Params["Type"]) : 0;
                int dept = Request.Params["Dept"] != null ? int.Parse(Request.Params["Dept"]) : 0;
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.CustomerItemRequestGet(sohPK, prkPK, dept, type));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Get Packing Materials
        /// </summary>
        /// <param name="context"></param>
        private static void PackingMaterials(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int sohPK = Request.Params["SohPK"] != null ? int.Parse(Request.Params["SohPK"]) : 0;
                int prkPK = Request.Params["PrhPK"] != null ? int.Parse(Request.Params["PrhPK"]) : 0;
                int type = Request.Params["Type"] != null && Request.Params["Type"] != "null" ? int.Parse(Request.Params["Type"]) : 0;
                int dept = Request.Params["Dept"] != null ? int.Parse(Request.Params["Dept"]) : 0;
                int ItmCatSC = Request.Params["ItmCatSC"] != null ? int.Parse(Request.Params["ItmCatSC"]) : 0;
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.PackingMaterials(sohPK, prkPK, dept, type, ItmCatSC));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// To Get IO Numbers
        /// </summary>
        /// <param name="context"></param>
        private static void GetIONumber(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int bizUnit = Request.Params["BizUnit"] != null ? int.Parse(Request.Params["BizUnit"]) : 0;
                int prhPK = Request.Params["PrhPK"] != null ? int.Parse(Request.Params["PrhPK"]) : 0;
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.GetIONumber(bizUnit, prhPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Detials For Auto Complete 
        /// </summary>
        /// <param name="context"></param>
        private static void GePurchaseRequestSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null && Request.Params["SearchType"] != "null" ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = string.Empty;
                if (Request.QueryString["SearchValue"] != null && Request.QueryString["SearchValue"] != "null")
                    searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString().Trim() + "%" : "%%";
                else
                    searchValue = Request.Params["SearchValue"] != null ? "%" + Request.Params["SearchValue"].ToString().Trim() + "%" : "%%";
                int processPK = Request.QueryString["ProcessPK"] != null && Request.QueryString["ProcessPK"] != "null" ? int.Parse(Request.QueryString["ProcessPK"]) : 0;
                int type = Request.Params["Type"] != null && Request.Params["Type"] != "null" ? int.Parse(Request.Params["Type"]) : 0;
                string pageUrl = Request.Params["PageUrl"] != null && Request.Params["PageUrl"] != "null" ? Request.Params["PageUrl"] : string.Empty;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetSearchValues(searchBy, searchValue, processPK, objUser, pageUrl, type));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Delete Purchase Request Details By 
        /// </summary>
        /// <param name="context"></param>
        private static void DeletePurchaseRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int requestPk = 0;
            string remarks = string.Empty;
            try
            {
                if (Request.Params["PK"] != null)
                {
                    requestPk = Convert.ToInt32((Request.Params["PK"].Trim()));

                }
                if (Request.Params["Remarks"] != null)
                {
                    remarks = Request.Params["Remarks"].ToString();

                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.DeletePurchaseRequest(requestPk, remarks));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }


        #endregion

        #region Purchase Request Trading List
        /// <summary>
        /// To Get Purchase Request Trading List Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseRequestTradingList(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int procID = 0;
            string pageUrl = "";

            try
            {
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["PageUrl"] != null)
                {
                    pageUrl = Request.Params["PageUrl"];
                }

                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestTradingList.GetPurchaseRequestTradingList(CommonFunctions.GetGridParams(Request), objUser, pageUrl, procID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }
        /// <summary>
        /// Delete Purchase Request Trading List Details By 
        /// </summary>
        /// <param name="context"></param>
        private static void DeletePurchaseRequestTradingList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int requestPk = 0;
            string remarks = string.Empty;
            try
            {
                if (Request.Params["PK"] != null)
                {
                    requestPk = Convert.ToInt32((Request.Params["PK"].Trim()));

                }
                if (Request.Params["Remarks"] != null)
                {
                    remarks = Request.Params["Remarks"].ToString();

                }
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestTradingList.DeletePurchaseRequestTradingList(requestPk, remarks));
            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Get Detials For Auto Complete 
        /// </summary>
        /// <param name="context"></param>
        private static void GePurchaseRequestTrdListSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                string searchBy = Request.Params["SearchType"] != null ? Request.Params["SearchType"].ToString() : string.Empty;
                string searchValue = string.Empty;
                if (Request.QueryString["SearchValue"] != null)
                    searchValue = Request.QueryString["SearchValue"] != null ? "%" + Request.QueryString["SearchValue"].ToString().Trim() + "%" : "%%";
                else
                    searchValue = Request.Params["SearchValue"] != null ? "%" + Request.Params["SearchValue"].ToString().Trim() + "%" : "%%";
                int processPK = Request.QueryString["ProcessPK"] != null ? int.Parse(Request.QueryString["ProcessPK"]) : 0;
                string pageUrl = Request.Params["PageUrl"] != null ? Request.Params["PageUrl"] : string.Empty;
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetSearchValues(searchBy, searchValue, processPK, objUser, pageUrl));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }


        /// <summary>
        /// To Get Purchase Request Details
        /// </summary>
        /// <param name="context"></param>
        private static void GetPurchaseRequestTrading(HttpContext context)
        {

            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int bizUnit = Request.Params["BizUnit"] != null ? int.Parse(Request.Params["BizUnit"]) : 0;
                int dept = Request.Params["Dept"] != null && Request.Params["Dept"] != "null" ? int.Parse(Request.Params["Dept"]) : 0;
                int type = Request.Params["Type"] != null ? int.Parse(Request.Params["Type"]) : 0;
                int prPK = Request.Params["PRPK"] != null ? int.Parse(Request.Params["PRPK"]) : 0;
                int rowCount = Request.Params["RCount"] != null && Request.Params["RCount"] != "null" && Request.Params["RCount"] != string.Empty ? int.Parse(Request.Params["RCount"]) : 0;
                int itemCategory = Request.Params["ItmCat"] != null && Request.Params["ItmCat"] != "null" && Request.Params["ItmCat"] != string.Empty ? int.Parse(Request.Params["ItmCat"]) : 0;
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestTrading.GetPurchaseRequestTrading(bizUnit, dept, type, prPK, rowCount, itemCategory));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Function Used To save purchase request
        /// </summary>
        /// <param name="context"></param>
        private static void SavePurchaseRequestTradingList(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string requisitionDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestTrading.SavePurchaseRequestTradingList(requisitionDetails, objUser));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = GTIService.Constants.Common.ResponseTypes.JSON;
                List<object> retvals = new List<object>();
                retvals.Add("-1");
                retvals.Add("");
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(retvals));
            }
        }

        private static void GetCostCenter(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                int bizUnit = Request.Params["BizUnit"] != null ? int.Parse(Request.Params["BizUnit"]) : 0;
                int CnmPK = Request.Params["CnmPK"] != null ? int.Parse(Request.Params["CnmPK"]) : 0;
                int deptPk = Request.Params["DeptPk"] != null ? int.Parse(Request.Params["DeptPk"]) : 0;
                int Active = Request.Params["active"] != null ? int.Parse(Request.Params["active"]) : 0;
                Response.Write(BusinessLogic.PurchaseRequestManagement.PurchaseRequestCreation.GetCostCenterByPK(CnmPK, Active, bizUnit, deptPk));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Purchase Request Management");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        #endregion
    }
}
