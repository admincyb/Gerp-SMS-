using System;
using System.Web;
using System.Web.SessionState;
using GTIService;

namespace Handlers
{
    public partial class Handlers : IHttpHandler, IRequiresSessionState
    {
        #region Methods
        /// <summary>
        /// handiling StoreRequisitionSlipCreation handelers.
        /// </summary>
        /// <param name="context"></param>
        private static void ExternalMaterialIssue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            if (Request.Params.Count > 0)
            {

                string Action = Request.Params["Action"] != null ? Request.Params["Action"].Trim() : string.Empty;
                switch (Action)
                {

                    case "SaveExternalMaterialIssue":
                        Handlers.SaveExternalMaterialIssue(context);
                        break;
                    case "SaveExternalMaterialIssueWkf":
                        Handlers.SaveExternalMaterialIssueWkf(context);
                        break;
                    case "SaveEMIDamageWkf":
                        Handlers.SaveEMIDamageWkf(context);
                        break;
                    case "GetExternalMaterialList":
                        Handlers.GetExternalMaterialList(context);
                        break;
                    case "GetExternalMaterialListWkf":
                        Handlers.GetExternalMaterialListWkf(context);
                        break;
                    case "GetSearchValue":
                        Handlers.GetExternalMaterialSearchValue(context);
                        break;
                    case "GetIssuingType":
                        Handlers.GetIssuingTypeList(context);
                        break;
                    case "GetIssuingToList":
                        Handlers.GetIssuingToList(context);
                        break;
                    case "FillIssuingToDamage":
                        Handlers.FillIssuingToDamage(context);
                        break;
                    case "GetAssetFormer":
                        Handlers.GetAssetFormer(context);
                        break;
                    case "DeleteExternalMaterialIssue":
                        Handlers.DeleteExternalMaterialIssue(context);
                        break;
                    case "GetGRNAutoComplteList":
                        Handlers.GetGRNAutoComplteList(context);
                        break;
                    case "GetGRNDetailsList":
                        Handlers.GetGRNDetailsList(context);
                        break;
                }
            }
        }

        /// <summary>
        /// Get GRN Details List
        /// </summary>
        /// <param name="context"></param>
        private static void GetGRNDetailsList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int GRNPK = 0;
            int IssuingStore = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["GRNPK"] != null)
                {
                    GRNPK = Convert.ToInt32(Request.Params["GRNPK"]);
                }
                if (Request.Params["IssuingStore"] != null)
                {
                    IssuingStore = Convert.ToInt32(Request.Params["IssuingStore"]);
                }
                // Get the list in json string format from BL
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetGRNDetailsList(GRNPK, IssuingStore));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Requisition Slip list");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get GRN AuroComplte List
        /// </summary>
        /// <param name="context"></param>
        private static void GetGRNAutoComplteList(HttpContext context)
        {

            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["SBUPk"]);
                }

                //string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : "%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetGRNAutoComplteList(bizUnit, srchValue));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Requisition Slip list");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }

        }
        /// <summary>
        /// Save External Material Issue/Receipt with Workflow
        /// </summary>
        /// <param name="context"></param>
        private static void SaveExternalMaterialIssueWkf(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string requisitionDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string requisition = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.SaveExternalMaterialIssueWkf(requisitionDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(requisition);


            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }
        }

        private static void SaveEMIDamageWkf(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string emiDamageDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string result = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.SaveEMIDamageWkf(emiDamageDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(result);
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");
            }
        }

        /// <summary>
        /// save Requisition Details
        /// </summary>
        /// <param name="context"></param>
        private static void SaveExternalMaterialIssue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string UserPk = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
            try
            {
                string requisitionDetails = GetRequestString(context);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                string requisition = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.SaveExternalMaterialIssue(requisitionDetails, objUser);
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                Response.Write(requisition);


            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Clear();
                Response.ContentType = "text";
                Response.Write("-1");

            }

        }

        /// <summary>
        /// Get GetIssuing Type List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetIssuingToList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int despatch = 0;
            int procID = 0;
            int issuingType = 0;
            int deptType = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["IssuingType"] != null)
                {
                    issuingType = Convert.ToInt32(Request.Params["IssuingType"]);
                }
                if (Request.Params["Despatch"] != null)
                {
                    despatch = Convert.ToInt32(Request.Params["Despatch"]);
                }
                if (Request.Params["DPT_TYPE"] != null)
                {
                    deptType = Convert.ToInt32(Request.Params["DPT_TYPE"]);
                }
                //string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : string.Empty;
                string srchValue = Request.Params["SearchValue"] != null ? "%" + Convert.ToString(Request.Params["SearchValue"]) + "%" : "%";
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetIssuingTypeList(bizUnit, issuingType, despatch, srchValue, deptType));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Requisition Slip list");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        public static void FillIssuingToDamage(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetDamageStore(objUser.CurrentDeptPK));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Damage Store list");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        public static void GetAssetFormer(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            try
            {
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetAssetFormer(bizUnit));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Damage Store list");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get GetIssuing Type List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetIssuingTypeList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int procID = 0, CFG_PK = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["SBUPk"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["SBUPk"]);
                }
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["CFG_PK"] != null)
                {
                    CFG_PK = Convert.ToInt32(Request.Params["CFG_PK"]);
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.CommonManagement.CommonManagement.GetParentDepartmentCategories(bizUnit, "EXTERNAL ISS RCV TYPE", CFG_PK));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Requisition Slip list");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Get Requisition List based search results
        /// </summary>
        /// <param name="context"></param>
        private static void GetExternalMaterialList(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int procID = 0;
            int transactionType = 1;
            int trnStatus = 0, issueTo = 0, issueType = 0, issueStore = 0, itmCatPk = 0, itmPk = 0, cmpPk=0;
            string lotNo = string.Empty, refNo = string.Empty, issueNo = string.Empty;
            string pageURL = "/StoreManagement/ExternalMaterialIssue.aspx";
            DateTime? fromDate = null;
            DateTime? toDate = null;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["Type"] != null)
                {
                    transactionType = Convert.ToInt32(Request.Params["Type"]);
                }
                if (Request.Params["PageURL"] != null)
                {
                    pageURL = Request.Params["PageURL"].ToString();
                }
                if (Request.Params["FromDate"] != null && Request.Params["FromDate"] != "")
                {
                    fromDate = Convert.ToDateTime(Request.Params["FromDate"]);
                }
                if (Request.Params["ToDate"] != null && Request.Params["ToDate"] != "")
                {
                    toDate = Convert.ToDateTime(Request.Params["ToDate"]);
                }
                if (Request.Params["TrnStatus"] != null && Request.Params["TrnStatus"] != "null" && Request.Params["TrnStatus"] != "")
                {
                    trnStatus = Convert.ToInt16(Request.Params["TrnStatus"]);
                }
                if (Request.Params["IssueNo"] != null && Request.Params["IssueNo"] != "null" && Request.Params["IssueNo"] != "")
                {
                    issueNo = Convert.ToString(Request.Params["IssueNo"]);
                }
                if (Request.Params["ISS_TYPE"] != null && Request.Params["ISS_TYPE"] != "null" && Request.Params["ISS_TYPE"] != "")
                {
                    issueType = Convert.ToInt16(Request.Params["ISS_TYPE"]);
                }
                if (Request.Params["ISS_TO"] != null && Request.Params["ISS_TO"] != "null" && Request.Params["ISS_TO"] != "")
                {
                    issueTo = Convert.ToInt16(Request.Params["ISS_TO"]);
                }
                if (Request.Params["ISS_STORE"] != null && Request.Params["ISS_STORE"] != "null" && Request.Params["ISS_STORE"] != "")
                {
                    issueStore = Convert.ToInt16(Request.Params["ISS_STORE"]);
                }
                if (Request.Params["LotNo"] != null && Request.Params["LotNo"] != "")
                {
                    lotNo = Convert.ToString(Request.Params["LotNo"]);
                }
                if (Request.Params["RefNo"] != null && Request.Params["RefNo"] != "")
                {
                    refNo = Convert.ToString(Request.Params["RefNo"]);
                }
                if (Request.Params["CatPk"] != null && Request.Params["CatPk"] != "")
                {
                    itmCatPk = Convert.ToInt32(Request.Params["CatPk"]);
                }
                if (Request.Params["ItmPk"] != null && Request.Params["ItmPk"] != "")
                {
                    itmPk = Convert.ToInt32(Request.Params["ItmPk"]);
                }
                //if (Request.Params["CMP_PK"] != null && Request.Params["CMP_PK"] != "null" && Request.Params["CMP_PK"] != "")
                //{
                //    cmpPk = Convert.ToInt32(Request.Params["CMP_PK"]);
                //}

                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetIssuingList(CommonFunctions.GetGridParams(Request), bizUnit, objUser, procID, transactionType, pageURL, fromDate, toDate, issueNo, trnStatus, issueType, issueTo, issueStore, lotNo, refNo, itmCatPk, itmPk)); //cmpPk
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Requisition Slip list");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        private static void GetExternalMaterialListWkf(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int bizUnit = 0;
            int procID = 0;
            int transactionType = 1;
            int trnStatus = 0, issueTo = 0, issueType = 0, issueStore = 0, itmCatPk = 0, itmPk = 0, cmpPk=0;
            string lotNo = string.Empty, refNo = string.Empty, issueNo = string.Empty;
            string pageURL = "/StoreManagement/EMRCreate.aspx";
            DateTime? fromDate = null;
            DateTime? toDate = null;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["BizUnit"] != null)
                {
                    bizUnit = Convert.ToInt32(Request.Params["BizUnit"]);
                }
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"]);
                }
                if (Request.Params["Type"] != null)
                {
                    transactionType = Convert.ToInt32(Request.Params["Type"]);
                }
                if (Request.Params["PageURL"] != null)
                {
                    pageURL = Request.Params["PageURL"].ToString();
                }
                if (Request.Params["FromDate"] != null && Request.Params["FromDate"] != "")
                {
                    fromDate = Convert.ToDateTime(Request.Params["FromDate"]);
                }
                if (Request.Params["ToDate"] != null && Request.Params["ToDate"] != "")
                {
                    toDate = Convert.ToDateTime(Request.Params["ToDate"]);
                }
                if (Request.Params["TrnStatus"] != null && Request.Params["TrnStatus"] != "null" && Request.Params["TrnStatus"] != "")
                {
                    trnStatus = Convert.ToInt16(Request.Params["TrnStatus"]);
                }
                if (Request.Params["IssueNo"] != null && Request.Params["IssueNo"] != "null" && Request.Params["IssueNo"] != "")
                {
                    issueNo = Convert.ToString(Request.Params["IssueNo"]);
                }
                if (Request.Params["ISS_TYPE"] != null && Request.Params["ISS_TYPE"] != "null" && Request.Params["ISS_TYPE"] != "")
                {
                    issueType = Convert.ToInt16(Request.Params["ISS_TYPE"]);
                }
                if (Request.Params["ISS_TO"] != null && Request.Params["ISS_TO"] != "null" && Request.Params["ISS_TO"] != "")
                {
                    issueTo = Convert.ToInt16(Request.Params["ISS_TO"]);
                }
                if (Request.Params["ISS_STORE"] != null && Request.Params["ISS_STORE"] != "null" && Request.Params["ISS_STORE"] != "")
                {
                    issueStore = Convert.ToInt16(Request.Params["ISS_STORE"]);
                }
                if (Request.Params["LotNo"] != null && Request.Params["LotNo"] != "")
                {
                    lotNo = Convert.ToString(Request.Params["LotNo"]);
                }
                if (Request.Params["RefNo"] != null && Request.Params["RefNo"] != "")
                {
                    refNo = Convert.ToString(Request.Params["RefNo"]);
                }
                if (Request.Params["CatPk"] != null && Request.Params["CatPk"] != "")
                {
                    itmCatPk = Convert.ToInt32(Request.Params["CatPk"]);
                }
                if (Request.Params["ItmPk"] != null && Request.Params["ItmPk"] != "")
                {
                    itmPk = Convert.ToInt32(Request.Params["ItmPk"]);
                }
                //if (Request.Params["CMP_PK"] != null && Request.Params["CMP_PK"] != "null" && Request.Params["CMP_PK"] != "")
                //{
                //    cmpPk = Convert.ToInt32(Request.Params["CMP_PK"]);
                //}

                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // Get the list in json string format from BL
                Response.Write(BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetIssuingListWkf(CommonFunctions.GetGridParams(Request), bizUnit, objUser, procID, transactionType, pageURL, fromDate, toDate, issueNo, trnStatus, issueType, issueTo, issueStore, lotNo, refNo, itmCatPk, itmPk)); //cmpPk
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Requisition Slip list");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        /// <summary>
        /// Method Used to Delete Requisition Detials
        /// </summary>
        /// <param name="context"></param>
        private static void DeleteExternalMaterialIssue(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            int issueID = 0;
            int USERPK = 0;
            int HasWorkflow = 0;
            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.HTML);
                if (Request.Params["IssueID"] != null)
                {
                    // Assign requisitionID From Request to requisitionID variable
                    issueID = Convert.ToInt32((Request.Params["IssueID"].Trim()));

                }
                if (Request.Params["USERPK"] != null)
                {
                    USERPK = Convert.ToInt32((Request.Params["USERPK"].Trim()));
                }
                if (Request.Params["HasWorkflow"] != null)
                {
                    HasWorkflow = Convert.ToInt32((Request.Params["HasWorkflow"].Trim()));
                }
                Response.Write(BusinessLogic.StoreManagement.ExternalMaterialIssueBL.DeleteExternalMaterialIssue(issueID, USERPK, HasWorkflow));

            }
            catch (Exception ex)
            {

                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip List");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                Response.Write("-1");
            }
        }

        /// <summary>
        /// Search Requisition Details , and Get Data Related to Search Criteria
        /// </summary>
        /// <param name="context"></param>
        private static void GetExternalMaterialSearchValue(HttpContext context)
        {
            // Create the request and response objects from context
            HttpRequest Request = context.Request;
            HttpResponse Response = context.Response;
            string searchBy = string.Empty;
            string searchValue = string.Empty;
            int sbuPk = 0;
            int procID = 0;
            int transactionType = 1;

            try
            {
                // clear all the response
                GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
                if (Request.Params["ProcID"] != null)
                {
                    procID = Convert.ToInt32(Request.Params["ProcID"].ToString());
                }
                if (Request.Params["SearchType"] != null)
                {
                    searchBy = Request.Params["SearchType"].ToString();
                }
                if (Request.QueryString["SearchValue"] != null)
                {
                    searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
                }
                if (Request.QueryString["SBUPk"] != null)
                {
                    sbuPk = Convert.ToInt32(Request.QueryString["SBUPk"].ToString());
                }
                if (Request.QueryString["Type"] != null)
                {
                    transactionType = Convert.ToInt32(Request.QueryString["Type"].ToString());
                }
                BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Response.Write(BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetSearchValues(searchBy, searchValue, sbuPk, objUser, procID, transactionType));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip List");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
            }
        }

        ///// <summary>
        ///// Search item name Details , and Get Data Related to code
        ///// </summary>
        ///// <param name="context"></param>
        //private static void GetStoreReqSearchValue(HttpContext context)
        //{
        //    // Create the request and response objects from context
        //    HttpRequest Request = context.Request;
        //    HttpResponse Response = context.Response;
        //    string searchBy = string.Empty;
        //    string searchValue = string.Empty;
        //    try
        //    {
        //        // clear all the response
        //        GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);


        //        if (Request.QueryString["SearchValue"] != null)
        //        {
        //            searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
        //        }

        //        Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetSearchValues(searchValue));
        //    }
        //    catch (Exception ex)
        //    {
        //        NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
        //        logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
        //    }
        //}
        ///// <summary>
        ///// Get Stores Name
        ///// </summary>
        ///// <param name="context"></param>
        //private static void GetStores(HttpContext context)
        //{
        //    // Create the request and response objects from context
        //    HttpRequest Request = context.Request;
        //    HttpResponse Response = context.Response;
        //    // string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
        //    int sbuPk = 0;
        //    int flag = 0;
        //    try
        //    {
        //        // clear all the response
        //        if (Request.Params["SBUPk"] != null)
        //        {
        //            sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
        //        }
        //        if (Request.Params["Flag"] != null)
        //        {
        //            flag = Convert.ToInt32(Request.Params["Flag"]);
        //        }
        //        string requestData = GetRequestString(context);
        //        GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
        //        BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        //        Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetStores(objUser, sbuPk, flag));
        //    }
        //    catch (Exception ex)
        //    {
        //        NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
        //        logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
        //    }
        //}
        ///// <summary>
        ///// Get Stores Name
        ///// </summary>
        ///// <param name="context"></param>
        //private static void GetStoresFilterByCategory(HttpContext context)
        //{
        //    // Create the request and response objects from context
        //    HttpRequest Request = context.Request;
        //    HttpResponse Response = context.Response;
        //    // string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
        //    int sbuPk = 0;
        //    int flag = 0;
        //    int category = 0;
        //    try
        //    {
        //        // clear all the response
        //        if (Request.Params["SBUPk"] != null)
        //        {
        //            sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
        //        }
        //        if (Request.Params["Flag"] != null)
        //        {
        //            flag = Convert.ToInt32(Request.Params["Flag"]);
        //        }
        //        if (Request.Params["Category"] != null)
        //        {
        //            category = Convert.ToInt32(Request.Params["Category"]);
        //        }
        //        string requestData = GetRequestString(context);
        //        GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
        //        BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        //        Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetStores(objUser, sbuPk, flag, category));
        //    }
        //    catch (Exception ex)
        //    {
        //        NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
        //        logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
        //    }
        //}
        ///// <summary>
        ///// Get SRS No
        ///// </summary>
        ///// <param name="context"></param>
        //private static void GetSRSNo(HttpContext context)
        //{
        //    HttpRequest Request = context.Request;
        //    HttpResponse Response = context.Response;
        //    try
        //    {
        //        GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
        //        Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetSRSNo());
        //    }
        //    catch (Exception ex)
        //    {

        //        NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
        //        logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Search Requisition Details , and Get Data Related to Search Criteria
        ///// </summary>
        ///// <param name="context"></param>
        //private static void GetRequisitionSearchValue(HttpContext context)
        //{
        //    // Create the request and response objects from context
        //    HttpRequest Request = context.Request;
        //    HttpResponse Response = context.Response;
        //    string searchBy = string.Empty;
        //    string searchValue = string.Empty;
        //    int sbuPk = 0;
        //    int procID = 0;

        //    try
        //    {
        //        // clear all the response
        //        GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
        //        if (Request.Params["ProcID"] != null)
        //        {
        //            procID = Convert.ToInt32(Request.Params["ProcID"].ToString());
        //        }
        //        if (Request.Params["SearchType"] != null)
        //        {
        //            searchBy = Request.Params["SearchType"].ToString();
        //        }
        //        if (Request.QueryString["SearchValue"] != null)
        //        {
        //            searchValue = "%" + Request.QueryString["SearchValue"].ToString() + "%";
        //        }
        //        if (Request.QueryString["SBUPk"] != null)
        //        {
        //            sbuPk = Convert.ToInt32(Request.QueryString["SBUPk"].ToString());
        //        }
        //        BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        //        Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipList.GetSearchValues(searchBy, searchValue, sbuPk, objUser, procID));
        //    }
        //    catch (Exception ex)
        //    {
        //        NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip List");
        //        logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Method Used to get Requisition Detials refered by rijoy in MA.
        ///// </summary>
        ///// <param name="context"></param>
        //private static void GetSRSDetails(HttpContext context)
        //{
        //    HttpRequest Request = context.Request;
        //    HttpResponse Response = context.Response;
        //    try
        //    {
        //        GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
        //        BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        //        int srsID = Request.Params["SRSPK"] != null ? int.Parse(Request.Params["SRSPK"]) : 0;
        //        Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetSRSDetails(srsID));
        //    }
        //    catch (Exception ex)
        //    {
        //        NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
        //        logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Get Stores Name By Type
        ///// </summary>
        ///// <param name="context"></param>
        //private static void GetStoresByType(HttpContext context)
        //{
        //    // Create the request and response objects from context
        //    HttpRequest Request = context.Request;
        //    HttpResponse Response = context.Response;
        //    // string user = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
        //    int sbuPk = 0;
        //    int deptType = 0;
        //    int deptPk = 0;
        //    try
        //    {
        //        // clear all the response
        //        if (Request.Params["SBUPk"] != null)
        //        {
        //            sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
        //        }

        //        if (Request.Params["DeptType"] != null)
        //        {
        //            deptType = Convert.ToInt32(Request.Params["DeptType"]);
        //        }
        //        string requestData = GetRequestString(context);
        //        GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
        //        BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        //        Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetStoresByType(objUser, sbuPk, deptType, deptPk));
        //    }
        //    catch (Exception ex)
        //    {
        //        NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
        //        logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Get  Department Details
        ///// </summary>
        ///// <param name="context"></param>
        //private static void GetUserDeparmentDtls(HttpContext context)
        //{
        //    HttpRequest Request = context.Request;
        //    HttpResponse Response = context.Response;
        //    int sbuPk = 0;
        //    //  string userPK = ((BusinessObject.ERPPrincipal)context.User).GetUserPK();
        //    try
        //    {
        //        if (Request.Params["SBUPk"] != null)
        //        {
        //            sbuPk = Convert.ToInt32(Request.Params["SBUPk"]);
        //        }
        //        BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        //        GTIService.CommonFunctions.PrepareResponse(Response, GTIService.Constants.Common.ResponseTypes.JSON);
        //        Response.Write(BusinessLogic.StoreManagement.StoreRequisitionSlipCreation.GetDepartmentDtls(objUser, sbuPk));
        //    }
        //    catch (Exception ex)
        //    {
        //        NLog.Logger logger = NLog.LogManager.GetLogger("Store Requisition Slip");
        //        logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
        //    }
        //}
        #endregion
    }
}
