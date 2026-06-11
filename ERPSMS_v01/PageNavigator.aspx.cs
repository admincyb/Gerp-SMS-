using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject.Common;
using ERPSMS_v01.UserControls;
using System.Linq;
using BusinessObject.CommonManagement;
using System.Data;
using BusinessObject;
using System.Web.UI.HtmlControls;
using BusinessObject.Journalize;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01
{
    public partial class PageNavigator : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Process ID of the Page
        /// </summary>
        private int PageProcessID
        {
            get
            {
                return this.ViewState["PageProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["PageProcessID"]);
            }
            set
            {
                this.ViewState["PageProcessID"] = value;
            }
        }
        private long refPK
        {
            get
            {
                return this.ViewState["refPK"] == null ? 0 : Convert.ToInt64(this.ViewState["refPK"]);
            }
            set
            {
                this.ViewState["refPK"] = value;
            }
        }
        private bool isReturn
        {
            get { return this.ViewState["isReturn"] == null ? false : Convert.ToBoolean(this.ViewState["isReturn"].ToString()); }
            set { this.ViewState["isReturn"] = value; }
        }
        private bool isPdcReverse
        {
            get { return this.ViewState["isPdcReverse"] == null ? false : Convert.ToBoolean(this.ViewState["isPdcReverse"].ToString()); }
            set { this.ViewState["isPdcReverse"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private string GRefType
        {
            get
            {
                return this.ViewState["GRefType"] == null ? string.Empty : this.ViewState["GRefType"].ToString();
            }
            set
            {
                this.ViewState["GRefType"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int GRefPK
        {
            get
            {
                return this.ViewState["GRefPK"] == null ? 0 : (int)this.ViewState["GRefPK"];
            }
            set
            {
                this.ViewState["GRefPK"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int TrxPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.TrxPK] == null ? 0 : (int)this.ViewState[ViewstateStrings.TrxPK];
            }
            set
            {
                this.ViewState[ViewstateStrings.TrxPK] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int JrnlPK
        {
            get
            {
                return this.ViewState["JrnlPK"] == null ? 0 : (int)this.ViewState["JrnlPK"];
            }
            set
            {
                this.ViewState["JrnlPK"] = value;
            }
        }
        private string JrnlType
        {
            get
            {
                return this.ViewState["JrnlType"] == null ? string.Empty : this.ViewState["JrnlType"].ToString();
            }
            set
            {
                this.ViewState["JrnlType"] = value;
            }
        }
        #endregion
        User currentUser;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private List<FIN_CRDR_NOTE_HDR> finCrDrNoteHdrList;
        private DataTable dtPageDetails;
        private DataTable dtResult;
        private string PageURL;
        long drcrPk;
        #endregion
        #region PageLevel Events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //  base.WkfPageUrl = "/journalize/journalizelisting.aspx?type=jv";
            PageActionHandler();
        }
        /// <summary>
        /// 
        /// </summary>
        private void PageActionHandler()
        {
            ucrWrkf.ViewType = 1;
            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {

                if (Request.QueryString[QueryStrings.GRefType] != null)
                {
                    GRefPK = Convert.ToInt32(Request.QueryString[QueryStrings.GRefPK].ToString());
                    NavigateRefPages(Request.QueryString[QueryStrings.GRefType].ToString());

                }
                else if (Request.QueryString[QueryStrings.JrnlType] != null)
                {
                    JrnlType = Request.QueryString[QueryStrings.JrnlType];
                    JrnlPK = Convert.ToInt32(Request.QueryString[QueryStrings.JrnlPK].ToString());
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        private void GetFieldValues(ControlsEnum type)
        {
            FinCrDrHdrNoteService finCrDrHdrNoteServiceClient = null;
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;
            ServiceUtility serviceUtilityObj = null;
            int? Status = null;
            FIN_TRX_HDR finTrxHdrObj;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region FIN HEADER
                    case ControlsEnum.DEFAULT:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = JrnlType;
                        finTrxHdrObj.FTH_REF_PK = 0;
                        finTrxHdrObj.FTH_PK = JrnlPK == 0 ? -1 : JrnlPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrObj.FTH_CRTD_BY = currentUser.PKUser;
                        finTrxHdrObj.FTH_BIZUNIT = currentUser.SBUID;
                        serviceUtilityObj.NeedAdvanceFilter = true;
                        if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CNJ || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.DNJ)
                        {
                            Session[ERP.Utilities.SessionStrings.SubType] = finTrxHdrObj.FTH_REF_TYPE;
                            finTrxHdrList = finTrxServiceClient.GetfinTxtHdrListCRDR(finTrxHdrObj, serviceUtilityObj, finTrxHdrObj.FTH_REF_TYPE);
                        }
                        else
                        {
                            finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        }
                        break;
                    #endregion
                    #region DRCR
                    case ControlsEnum.DRCR:
                        finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                        finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                        if (drcrPk > 0)
                        {
                            finCrDrNoteHdrList = finCrDrHdrNoteServiceClient.GetCrDrNoteHdr(drcrPk);
                        }
                        else
                        {
                            finCrDrNoteHdrList = finCrDrHdrNoteServiceClient.GetCrDrNoteHdr(GRefPK);
                        }
                        break;
                    #endregion
                    #region PAGE DETAILS
                    case ControlsEnum.PAGEDETAILS:
                        dtPageDetails = BusinessLogic.Administration.Configurations.PageActionBL.GetPages(null, (int)DbActiveStatus.ACTIVE, PageURL);
                        break;
                    #endregion
                    #region PAYROLL DETAILS
                    case ControlsEnum.PAYROLLDETAILS:
                        dtResult = BusinessLogic.HRMS.Payroll.PayrollProcessBL.GetPayroll(GRefPK, currentUser.SBUID);
                        break;
                    #endregion
                    #region SALARY PAYMENT DETAILS
                    case ControlsEnum.SALPAYMENTDETAILS:
                        ERP.Utilities.HRMS.FilterParameters objFilterParams = new ERP.Utilities.HRMS.FilterParameters();
                        objFilterParams.PageNumber = 0;
                        objFilterParams.PageSize = 20;
                        objFilterParams.PaymentMode = null;
                        objFilterParams.FromDate = (DateTime?)null;
                        objFilterParams.ToDate = (DateTime?)null;
                        objFilterParams.Name = string.Empty;
                        objFilterParams.UserPK = currentUser.PKUser;
                        objFilterParams.Status = null;
                        objFilterParams.BizUnit = currentUser.SBUID;
                        objFilterParams.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        objFilterParams.PK = GRefPK;
                        dtResult = BusinessLogic.HRMS.Payroll.SalaryPaymentBL.GetSalaryPaymentList(objFilterParams, 0);
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                finTrxServiceClient = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        private void SetFieldValues(ControlsEnum type)
        {
            string pageURL = string.Empty;
            string yearEndType = string.Empty;
            string trnType = string.Empty;
            switch (type)
            {
                case ControlsEnum.DEFAULT:
                    if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                    {
                        refPK = finTrxHdrList[0].FTH_REF_PK;
                        isReturn = finTrxHdrList[0].FTH_BOUNCED == 1 ? true : false;
                        isPdcReverse = finTrxHdrList[0].FTH_PDC > 1 ? true : false;
                        pageURL = GetJournalURL(JrnlType, Convert.ToInt32(refPK)).Replace("~", "");
                        //For resolving : no need to show purchase voucher along with sales when search by sales voucher  
                        if (JrnlType == ApplicationType.DNJ || JrnlType == ApplicationType.CNJ)
                            trnType = GetTransactionType(refPK);
                        if (HasPrivilege(pageURL))
                        {
                            switch (JrnlType)
                            {
                                case ApplicationType.PIJYE:
                                case ApplicationType.VPJYE:
                                case ApplicationType.SIJYE:
                                case ApplicationType.CRJYE:
                                case ApplicationType.EIJYE:
                                case ApplicationType.PSIJYE:
                                case ApplicationType.EIPJYE:
                                case ApplicationType.SIPJYE:
                                case ApplicationType.MSIJYE:
                                case ApplicationType.MSIRJYE:
                                case ApplicationType.FCHRJYE:
                                case ApplicationType.CNSJYE:
                                case ApplicationType.DNSJYE:
                                case ApplicationType.CNPJYE:
                                case ApplicationType.DNPJYE:
                                    yearEndType = JrnlType;
                                    JrnlType = ApplicationType.YE;
                                    break;
                            }
                            //Response.Redirect("journalize/JournalizeListing.aspx?" + "FromExt=T" + "&PK=" + JrnlPK.ToString() + "&Dep=" + currentUser.CurrentDeptPK.ToString() + "&Type=" + JrnlType + "&YearEndType=" + yearEndType);
                            if (JrnlType == "VPJ" || JrnlType == "EIPJ" || JrnlType == "SIPJ")
                            {
                                Response.Redirect("journalize/JournalizeListing.aspx?" + "FromExt=T" + "&PK=" + JrnlPK.ToString() + "&Dep=" + currentUser.CurrentDeptPK.ToString() + "&Type=" + "VPJ" + "&YearEndType=" + yearEndType + "&SType=" + trnType);
                            }
                            else if (JrnlType == "MSIRJ")
                            {
                                Response.Redirect("journalize/JournalizeListing.aspx?" + "FromExt=T" + "&PK=" + JrnlPK.ToString() + "&Dep=" + currentUser.CurrentDeptPK.ToString() + "&Type=" + "CRJ" + "&YearEndType=" + yearEndType + "&SType=" + trnType);
                            }
                            else if (JrnlType == "PSIJ")
                            {
                                Response.Redirect("journalize/JournalizeListing.aspx?" + "FromExt=T" + "&PK=" + JrnlPK.ToString() + "&Dep=" + currentUser.CurrentDeptPK.ToString() + "&Type=" + "PIJ" + "&YearEndType=" + yearEndType + "&SType=" + trnType);
                            }
                            else
                            {
                                Response.Redirect("journalize/JournalizeListing.aspx?" + "FromExt=T" + "&PK=" + JrnlPK.ToString() + "&Dep=" + currentUser.CurrentDeptPK.ToString() + "&Type=" + JrnlType + "&YearEndType=" + yearEndType + "&SType=" + trnType);
                            }

                        }
                        else
                        {
                            litErrorMsg.Text = GetGlobalResourceObject("ErrorMessages", "NoRights").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Helper Methods

        private string GetJournalURL(string type, int refPK)
        {
            string path = string.Empty;
            int urlType = 1;
            if (type == ApplicationType.CNJ || type == ApplicationType.DNJ || type == ApplicationType.PIJ || type == ApplicationType.SIJ)
            {
                if (refPK > 0)
                {
                    FinTrxService finTrxServiceClient;
                    finTrxServiceClient = null;
                    finTrxServiceClient = new FinTrxService();
                    finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                    int retVal = finTrxServiceClient.GetFinType(type, refPK);
                    finTrxServiceClient = null;
                    if (retVal > 0)
                    {
                        urlType = retVal;
                    }
                }
            }
            switch (type)
            {
                //case ApplicationType.PIJYE:
                //    path = GetGlobalResourceObject("PageURL", "YearEndVoucher").ToString().ToLower();
                //    break;
                case ApplicationType.CWIPJ:
                    path = GetGlobalResourceObject("PageURL", "CWIPJPath").ToString().ToLower();
                    break;
                case ApplicationType.DPRJ:
                    path = GetGlobalResourceObject("PageURL", "DepreciationPath").ToString().ToLower();
                    break;
                case ApplicationType.ACIJ:
                    path = GetGlobalResourceObject("PageURL", "AGTCommPath").ToString().ToLower();
                    break;
                case ApplicationType.FCHRJ:
                    path = GetGlobalResourceObject("PageURL", "FCReversePath").ToString().ToLower();
                    break;
                case ApplicationType.JV:
                    path = GetGlobalResourceObject("PageURL", "JVPath").ToString().ToLower();
                    break;
                case ApplicationType.PSIJ:
                    path = GetGlobalResourceObject("PageURL", "JVPathPIJ").ToString().ToLower();
                    break;
                case ApplicationType.SIPJ:
                    path = GetGlobalResourceObject("PageURL", "JVPathVPJ").ToString().ToLower();
                    break;
                case ApplicationType.PCS:
                    path = GetGlobalResourceObject("PageURL", "PCSPath").ToString().ToLower();
                    break;
                case ApplicationType.AIPJ:
                case ApplicationType.VPJ:
                    path = GetGlobalResourceObject("PageURL", "VPJPath").ToString().ToLower();
                    break;
                case ApplicationType.VPTJ:
                    path = GetGlobalResourceObject("PageURL", "VPTJPath").ToString().ToLower();
                    break;
                case ApplicationType.EIPJ:
                    path = GetGlobalResourceObject("PageURL", "VPJPath").ToString().ToLower();
                    break;
                case ApplicationType.CRJ:
                    path = GetGlobalResourceObject("PageURL", "CRJPath").ToString().ToLower();
                    break;
                case ApplicationType.CRTJ:
                    path = GetGlobalResourceObject("PageURL", "CRTJPath").ToString().ToLower();
                    break;
                case ApplicationType.MSIRJ:
                    path = GetGlobalResourceObject("PageURL", "CRJPath").ToString().ToLower();
                    break;
                case ApplicationType.DSIJ:
                    path = GetGlobalResourceObject("PageURL", "DSIJPath").ToString().ToLower();
                    break;
                case ApplicationType.CNJ:
                    if (urlType == 1)
                        path = GetGlobalResourceObject("PageURL", "CNJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetGlobalResourceObject("PageURL", "CNJPath2").ToString().ToLower();

                    if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                    {
                        path = GetGlobalResourceObject("PageURL", "CNJPath2").ToString().ToLower();
                    }
                    break;
                case ApplicationType.CNTJ:
                    if (urlType == 1)
                        path = GetGlobalResourceObject("PageURL", "CNTJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetGlobalResourceObject("PageURL", "CNTJPath2").ToString().ToLower();

                    //if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                    //{
                    //    path = GetGlobalResourceObject("PageURL", "CNTJPath2").ToString().ToLower();
                    //}
                    break;
                case ApplicationType.DNJ:
                    if (urlType == 1)
                        path = GetGlobalResourceObject("PageURL", "DNJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetGlobalResourceObject("PageURL", "DNJPath2").ToString().ToLower();
                    if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                    {
                        path = GetGlobalResourceObject("PageURL", "DNJPath2").ToString().ToLower();
                    }
                    break;
                case ApplicationType.DNTJ:
                    if (urlType == 1)
                        path = GetGlobalResourceObject("PageURL", "DNTJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetGlobalResourceObject("PageURL", "DNTJPath2").ToString().ToLower();
                    //if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                    //{
                    //    path = GetGlobalResourceObject("PageURL", "DNTJPath2").ToString().ToLower();
                    //}
                    break;
                case ApplicationType.PIJ:
                    if (urlType == 1)
                        path = GetGlobalResourceObject("PageURL", "PIJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetGlobalResourceObject("PageURL", "PIJPath2").ToString().ToLower();
                    break;
                case ApplicationType.TPIJ:
                    if (urlType == 1)
                        path = GetGlobalResourceObject("PageURL", "TPIJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetGlobalResourceObject("PageURL", "TPIJPath2").ToString().ToLower();
                    break;
                case ApplicationType.SIJ:
                    if (urlType == 1)
                        path = GetGlobalResourceObject("PageURL", "SIJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetGlobalResourceObject("PageURL", "SIJPath2").ToString().ToLower();
                    break;
                case ApplicationType.EIJ:
                    path = GetGlobalResourceObject("PageURL", "EIJPath").ToString().ToLower();
                    break;
                case ApplicationType.PDCCJ:
                    path = GetGlobalResourceObject("PageURL", "PDCCJPath").ToString().ToLower();
                    break;
                case ApplicationType.PDCCTJ:
                    path = GetGlobalResourceObject("PageURL", "PDCCTJPath").ToString().ToLower();
                    break;
                case ApplicationType.PPCCJ:
                    path = GetGlobalResourceObject("PageURL", "PPCCJPath").ToString().ToLower();
                    break;
                case ApplicationType.PPCCTJ:
                    path = GetGlobalResourceObject("PageURL", "PPCCTJPath").ToString().ToLower();
                    break;
                case ApplicationType.RCBJ:
                    path = GetGlobalResourceObject("PageURL", "RCBJPath").ToString().ToLower();
                    break;
                case ApplicationType.PCBJ:
                    path = GetGlobalResourceObject("PageURL", "PCBJPath").ToString().ToLower();
                    break;
                case ApplicationType.MSIJ:
                    path = GetGlobalResourceObject("PageURL", "MSIJPath").ToString().ToLower();
                    break;
                case ApplicationType.MSITJ:
                    path = GetGlobalResourceObject("PageURL", "MSITJPath").ToString().ToLower();
                    break;
                case ApplicationType.OBV:
                    path = GetGlobalResourceObject("PageURL", "OBVPath").ToString().ToLower();
                    break;
                case ApplicationType.DPVJ:
                    path = GetGlobalResourceObject("PageURL", "DPVJPath").ToString().ToLower();
                    break;
                case ApplicationType.PCVJ:
                    path = GetGlobalResourceObject("PageURL", "PCVJPath").ToString().ToLower();
                    break;
                case ApplicationType.DRVJ:
                    path = GetGlobalResourceObject("PageURL", "DRVJPath").ToString().ToLower();
                    break;
                case ApplicationType.PIJYE:
                case ApplicationType.VPJYE:
                case ApplicationType.SIJYE:
                case ApplicationType.CRJYE:
                case ApplicationType.EIJYE:
                case ApplicationType.PSIJYE:
                case ApplicationType.EIPJYE:
                case ApplicationType.SIPJYE:
                case ApplicationType.MSIJYE:
                case ApplicationType.MSIRJYE:
                case ApplicationType.FCHRJYE:
                case ApplicationType.CNSJYE:
                case ApplicationType.DNSJYE:
                case ApplicationType.CNPJYE:
                case ApplicationType.DNPJYE:
                case ApplicationType.YE:
                    path = GetGlobalResourceObject("PageURL", "YEPath").ToString().ToLower();
                    break;
                case ApplicationType.DPVCJ:
                    if (isPdcReverse)
                    {
                        path = GetGlobalResourceObject("PageURL", "DPVCJReversePath").ToString().ToLower();
                    }
                    else if (isReturn)
                    {
                        path = GetGlobalResourceObject("PageURL", "DPBJPath").ToString().ToLower();
                    }
                    else
                    {
                        path = GetGlobalResourceObject("PageURL", "DPVCJPath").ToString().ToLower();
                    }
                    break;
                case ApplicationType.DPBJ:
                    path = GetGlobalResourceObject("PageURL", "DPBJPath").ToString().ToLower();
                    break;
                case ApplicationType.YCV:
                    path = GetGlobalResourceObject("PageURL", "YCVPath").ToString().ToLower();
                    break;
                case ApplicationType.CLSTJ:
                    path = GetGlobalResourceObject("PageURL", "CLSTJPath").ToString().ToLower();
                    break;
                case ApplicationType.BDJ:
                    path = GetGlobalResourceObject("PageURL", "BDJPath").ToString().ToLower();
                    break;
                case ApplicationType.PAYRLJ:
                    path = GetGlobalResourceObject("PageURL", "PAYRLJPath").ToString().ToLower();
                    break;
                case ApplicationType.SALPYMTJ:
                    path = GetGlobalResourceObject("PageURL", "SALPYMTJPath").ToString().ToLower();
                    break;
            }
            return path;
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID(string type, int refPK, string path)
        {

            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    PageProcessID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                    base.WkfPageUrl = path;
                    ucrWrkf.PageUrl = path;
                    //base.WkfPageType = (int)PageTypeEnum.Listing;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        private void NavigateRefPages(string type)
        {
            switch (type)
            {
                case ApplicationType.PI:
                    Navigate(GetGlobalResourceObject("PageURL", "PurchaseOrderInvoicing").ToString());
                    break;
                case ApplicationType.SI:
                    Navigate(GetGlobalResourceObject("PageURL", "Invoicing").ToString());
                    break;
                case ApplicationType.EI:
                    Navigate(GetGlobalResourceObject("PageURL", "ExpenseInvoice").ToString());
                    break;
                case ApplicationType.MSI:
                    Navigate(GetGlobalResourceObject("PageURL", "MiscellaneousInv").ToString());
                    break;
                case ApplicationType.PSI:
                    Navigate(GetGlobalResourceObject("PageURL", "PurchaseOrderInvoicing").ToString());
                    break;
                case ApplicationType.CN:
                case ApplicationType.DN:
                    GetFieldValues(ControlsEnum.DRCR);
                    if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
                    {
                        if (finCrDrNoteHdrList[0].CDH_VENDOR.HasValue)//Purchase
                            Navigate(GetGlobalResourceObject("PageURL", "DrCrNote").ToString());
                        else if (finCrDrNoteHdrList[0].CDH_CUSTOMER.HasValue)//Sales
                            Navigate(GetGlobalResourceObject("PageURL", "DrCrNoteSales").ToString());
                    }
                    break;
                case ApplicationType.ACI:
                    Navigate(GetGlobalResourceObject("PageURL", "AgentCommPath").ToString());
                    break;
                case ApplicationType.FCHR:
                    Navigate(GetGlobalResourceObject("PageURL", "FCReversePath1").ToString());
                    break;
                case ApplicationType.PCBJ:
                    Navigate(GetGlobalResourceObject("PageURL", "PoPayment").ToString());
                    break;
                case ApplicationType.RCBJ:
                case ApplicationType.CR:
                case ApplicationType.MSIR:
                    Navigate(GetGlobalResourceObject("PageURL", "SalesReceipt").ToString());
                    break;
                case ApplicationType.DRVJ:
                    Navigate(GetGlobalResourceObject("PageURL", "DirectReceiptDetails").ToString());
                    break;
                case ApplicationType.VP:
                case ApplicationType.EIP:
                case ApplicationType.SIP:
                    Navigate(GetGlobalResourceObject("PageURL", "PoPayment").ToString());
                    break;
                case ApplicationType.BDJ:
                    Navigate(GetGlobalResourceObject("PageURL", "BadDebtURL").ToString());
                    break;
                case ApplicationType.CLST:
                    Navigate(GetGlobalResourceObject("PageURL", "ClosingStock").ToString());
                    break;
                case ApplicationType.DPR:
                    Navigate(GetGlobalResourceObject("PageURL", "DeprcnPath").ToString());
                    break;
                case ApplicationType.PAYRL:
                    NavigateToOtherServer(GetGlobalResourceObject("PageURL", "HrmsPayroll").ToString(), ApplicationType.PAYRL);
                    break;
                case ApplicationType.SALPYMT:
                    NavigateToOtherServer(GetGlobalResourceObject("PageURL", "HrmsSalaryPayment").ToString(), ApplicationType.SALPYMT);
                    break;
                case ApplicationType.JV:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPath").ToString());
                    break;
                case ApplicationType.SIJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathSIJ").ToString());
                    break;
                case ApplicationType.PIJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathPIJ").ToString());
                    break;
                case ApplicationType.MSIRJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathCRJ").ToString());
                    break;
                case ApplicationType.CRJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathCRJ").ToString());
                    break;
                case ApplicationType.VPJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathVPJ").ToString());
                    break;
                case ApplicationType.EIPJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathEIPJ").ToString());
                    break;
                case ApplicationType.MSIJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathMSIJ").ToString());
                    break;
                case ApplicationType.EIJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathEIJ").ToString());
                    break;
                case ApplicationType.DNJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathDNJ").ToString());
                    break;
                case ApplicationType.DPVJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathDPVJ").ToString());
                    break;
                case ApplicationType.MIJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathMIJ").ToString());
                    break;
                case ApplicationType.PCVJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathPCVJ").ToString());
                    break;
                case ApplicationType.CNJ:
                    Navigate(GetGlobalResourceObject("PageURL", "JVPathCNJ").ToString());
                    break;
                default:
                    litErrorMsg.Text = GetGlobalResourceObject("ErrorMessages", "NotYetHandled").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    break;
            }
        }
        /// <summary>
        /// Navigate Pages
        /// </summary>
        /// <param name="pageURL"></param>
        private void Navigate(string pageURL)
        {
            if (HasPrivilege(pageURL.Replace("~", "")))
            {
                Response.Redirect(pageURL + "&FromExt=T" + "&PK=" + GRefPK.ToString() + "&Dep=" + currentUser.CurrentDeptPK.ToString());
            }
            else
            {
                litErrorMsg.Text = GetGlobalResourceObject("ErrorMessages", "NoRights").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Navigate Pages to other server/domain
        /// </summary>
        /// <param name="pageURL"></param>
        private void NavigateToOtherServer(string pagURL, string appType)
        {
            bool HasPagePermission = false;
            string Redirecturl = string.Empty;
            PageURL = pagURL.Replace("~", "");
            GetFieldValues(ControlsEnum.PAGEDETAILS);
            if (dtPageDetails != null && dtPageDetails.Rows.Count > 0)
            {
                Redirecturl = Convert.ToString(dtPageDetails.Rows[0]["PAG_SERVER"]) + PageURL + "&MnChange=1";

                switch (appType)
                {
                    #region PAYRL
                    case ApplicationType.PAYRL:
                        GetFieldValues(ControlsEnum.PAYROLLDETAILS);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            if (HasPrivilege(PageURL, Convert.ToInt32(dtResult.Rows[0]["EPH_DEPT"])))
                            {
                                Redirecturl = Redirecturl + "&Dep=" + Convert.ToString(dtResult.Rows[0]["EPH_DEPT"]);
                                HasPagePermission = true;
                            }
                        }
                        break;
                    #endregion
                    #region SALPYMT
                    case ApplicationType.SALPYMT:
                        GetFieldValues(ControlsEnum.SALPAYMENTDETAILS);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            if (HasPrivilege(PageURL, Convert.ToInt32(dtResult.Rows[0]["PSH_DEPT"])))
                            {
                                Redirecturl = Redirecturl + "&Dep=" + Convert.ToString(dtResult.Rows[0]["PSH_DEPT"]);
                                HasPagePermission = true;
                            }
                        }
                        break;
                        #endregion
                }

                if (!string.IsNullOrEmpty(Redirecturl) && HasPagePermission)
                    Response.Redirect(Redirecturl + "&FromExt=T" + "&PK=" + GRefPK.ToString());
                else
                {
                    litErrorMsg.Text = GetGlobalResourceObject("ErrorMessages", "NoRights").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    return;
                }
            }
            else
            {
                litErrorMsg.Text = GetGlobalResourceObject("ErrorMessages", "NoRights").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                return;
            }

        }
        /// <summary>
        /// For Check the user has the privilege to access the page
        /// </summary>
        /// <param name="pageURL"></param>
        /// <returns></returns>
        private bool HasPrivilege(string pageURL, int? DeptPk = null)
        {
            bool result = false;
            CommonBL userAuth = new CommonBL();
            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            UserRightsBO UsrRights = userAuth.GetUserPageRights(currentUser.PKUser, pageURL, currentUser.SBUID, DeptPk.HasValue ? DeptPk.Value : currentUser.CurrentDeptPK);
            if (UsrRights != null && UsrRights.Rights.Count() > 0 && UsrRights.Rights[0].UserDeptRight)
                result = true;
            return result;

        }

        /// <summary>
        /// For Checking transaction is DebitNote (Purchase) or Debit Note (Sales)
        /// </summary>       
        private string GetTransactionType(long PK)
        {
            string transactionType = string.Empty;
            drcrPk = PK;
            GetFieldValues(ControlsEnum.DRCR);
            if (finCrDrNoteHdrList != null && finCrDrNoteHdrList.Count > 0)
            {
                if (finCrDrNoteHdrList[0].CDH_VENDOR.HasValue)//Purchase
                    transactionType = "PI";
                else if (finCrDrNoteHdrList[0].CDH_CUSTOMER.HasValue)//Sales
                    transactionType = "SI";
            }
            return transactionType;
        }


        #endregion
    }
    public enum ControlsEnum
    {
        DEFAULT,
        DRCR,
        PAGEDETAILS,
        PAYROLLDETAILS,
        SALPAYMENTDETAILS,
        LINE,
        PRODUCT,
        DATECHANGED,
        LINEPKCHANGED,
        PLANT,
        STATUS,
        TYPE,
        FINHEADER
    }
}