using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.POInvoicing;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPSMS_v01.UserControls;
using BusinessObject.AlertManagement;
using BusinessObject.PurchaseOrderManagement;
using BusinessObject.Finance;
using System.IO;

namespace ERPSMS_v01.Finance
{
    public partial class GSTReturn : ERP.Store.UI.WorkFlowBasePage//ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Properties
        private int CurrSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] = value;
            }
        }
        /// <summary>
        /// WorkFlow RefID
        /// </summary>
        public int WkfRefID
        {
            get
            {
                return (this.ViewState["BaseWkfRefID"] == null ? 0 : (int)this.ViewState["BaseWkfRefID"]);
            }
            set
            {
                this.ViewState["BaseWkfRefID"] = value;
            }
        }
        /// <summary>
        /// To maintain the LastModifiedTime in viewstate
        /// </summary>
        private DateTime LastModifiedTime
        {
            get
            {
                return this.ViewState[ViewstateStrings.LastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.LastModifiedTime];
            }
            set
            {
                this.ViewState[ViewstateStrings.LastModifiedTime] = value;
            }
        }
        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrPK] = value;
            }
        }
        /// <summary>
        /// Is continue
        /// </summary>
        private bool Iscont
        {
            get
            {
                return this.ViewState[ViewstateStrings.Iscont] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.Iscont]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Iscont] = value;
            }
        }
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
        /// <summary>
        /// Current Quotation PK
        /// </summary>
        private int CurrPOPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrSOPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrSOPK] = value;
            }
        }
        /// <summary>
        /// Tax PK
        /// </summary>
        private int TaxPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.TaxPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TaxPK] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndex
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }
        /// <summary>
        /// To maintain the SortExpression or sort By in viewstate
        /// </summary>
        private string SortBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortBy] = value;
            }
        }
        /// <summary>
        /// To maintain the SortExpression or then By in viewstate
        /// </summary>
        private string ThenBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenBy] = value;
            }
        }
        /// <summary>
        /// To maintain the Sort Direction in viewstate
        /// </summary>
        private string SortDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortDirection] = value;
            }
        }
        /// <summary>
        /// To maintain the Then Direction in viewstate
        /// </summary>
        private string ThenDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenDirection] = value;
            }
        }
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.ENTRYMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }
        private int SelectedItemPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedItemPK] = value;
            }
        }
        private bool IsHeaderTax
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderTax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderTax] = value;
            }
        }
        private bool IsEditMode
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsEditMode]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsEditMode] = value;
            }
        }
        private string SelectedTaxText
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SelectedTaxText];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedTaxText] = value;
            }
        }
        /// <summary>
        /// Approved
        /// </summary>
        private int Approved
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.Approved]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Approved] = value;
            }
        }
        /// <summary>
        /// Approved
        /// </summary>
        private bool Posted
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.Posted]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Posted] = value;
            }
        }
        /// <summary>
        /// To maintain keep selected Currency
        /// </summary>
        private long SelectedCurrency
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.SelectedCurrency]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCurrency] = value;
            }
        }
        /// <summary>
        /// GST Return 
        /// </summary>
        private GSTReturnHeader GSTReturnHeaderSession
        {
            get
            {
                return (GSTReturnHeader)Session[ERP.Utilities.SessionStrings.GSTReturnHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.GSTReturnHeaderSession] = value;
            }
        }
        private DateTime RecentEndDate
        {
            get
            {
                return this.ViewState[ViewstateStrings.RecentEndDate] == null ? System.DateTime.Now.AddMonths(-1) : (DateTime)this.ViewState[ViewstateStrings.RecentEndDate];
            }
            set
            {
                this.ViewState[ViewstateStrings.RecentEndDate] = value;
            }
        }
        private bool IsAnyDraft
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsAnyDraft]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsAnyDraft] = value;
            }
        }
        #endregion
        // Indicates the state as well as action
        private string refID;
        private string inboxFlag;
        private int processPK;
        private ActionsEnum commonActions;
        User currentUser;
        private CommonService cm;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        //page related Entity Object
        private ADM_COMPANY_MST admCompanyMstObj;
        private ServiceUtility serviceUtilityObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        private GSTReturnHeader GSTReturnHeaderObj;
        private DateTime startDate;
        private DateTime endDate;
        private DateTime startOfMonth;
        private DateTime dueDate;
        DataTable dtCurrentCompany;
        DataSet dsGSTReturn;
        DataTable dtGSTReturn;

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2)
    };

        #endregion
        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                int referenceID;
                int processID;
                int appId;
                referenceID = 0;
                processID = 0;
                appId = 0;
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    processID = FillProcessID();
                    ucrWrkf.ProcessID = processID;
                    if (ucrWrkf.ProcessID > 0)
                        hdfProcessID.Value = ucrWrkf.ProcessID.ToString();

                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.EDITMODE;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        referenceID = int.Parse(refID);
                        appId = GetApplicationID(referenceID);
                        if (processPK == ucrWrkf.ProcessID)
                        {
                            base.WkfRefID = ucrWrkf.RefID = referenceID;
                        }
                    }
                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.GSTRETURNLIST);
                    SetFieldValues(ControlsEnum.GSTRETURNLIST);
                    AST_DOC_MODE.Value = "0";//Genarate TrNo
                    if (CurrPK > 0)
                    {
                        AST_DOC_MODE.Value = GetDOCMODE();
                        AST_CODE.Value = ApplicationType.GSTR;
                        txtGSTValueNo.Text = hdfGSTValueNo.Value == string.Empty ? "[NEW]" : hdfGSTValueNo.Value;
                        hdfAppType.Value = ApplicationType.GSTR;
                        hdfAppSubType.Value = string.Empty;
                        hdfCurrentPk.Value = CurrPK.ToString();
                    }
                    Session[ERP.Utilities.SessionStrings.RefID] = null;
                    Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            AdmCompanyMstService admCompanyMstServiceClient;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region Company Details
                    case ControlsEnum.SHOWCOMPANYDETAILS:
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_PK = Convert.ToInt32(ddlCompany.SelectedItem.Value);
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        dtCurrentCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion
                    #region  SET
                    case ControlsEnum.SET:
                        startDate = Convert.ToDateTime(txtStartDate.Text);
                        endDate = Convert.ToDateTime(txtEndDate.Text);
                        startOfMonth = new DateTime(endDate.Year, endDate.Month, 1);
                        dueDate = startOfMonth.AddMonths(1).AddDays(-1);
                        GSTReturnHeaderObj = BusinessLogic.Finance.GSTReturnBL.GetDetails(startDate, endDate, currentUser.SBUID, 1, 0);
                        if (GSTReturnHeaderObj != null)
                        {
                            GSTReturnHeaderSession = GSTReturnHeaderObj;
                            CurrPK = GSTReturnHeaderObj.TGH_PK;
                            LastModifiedTime = string.IsNullOrEmpty(GSTReturnHeaderObj.LAST_MOD_DT) ? DateTime.Now : Convert.ToDateTime(GSTReturnHeaderObj.LAST_MOD_DT);
                        }
                        else
                        {
                            GSTReturnHeaderObj = null;
                            GSTReturnHeaderSession = GSTReturnHeaderObj;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Edit_Delete").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.GSTReturn);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region GSTRETURNLIST - For Gridview
                    case ControlsEnum.GSTRETURNLIST:
                        dsGSTReturn = BusinessLogic.Finance.GSTReturnBL.GetReportList(1, currentUser.SBUID, Convert.ToInt32(ddlCompany.SelectedItem.Value));
                        if (dsGSTReturn.Tables.Count > 0)
                        {
                            dtGSTReturn = dsGSTReturn.Tables[0];
                            if (dtGSTReturn.Rows.Count > 0)
                            {
                                IsAnyDraft = dtGSTReturn.AsEnumerable().Where(c => c.Field<byte>("TGH_STATUS").Equals(0)).Count() > 0;
                                if (!IsAnyDraft)
                                {
                                    RecentEndDate = Convert.ToDateTime(dtGSTReturn.AsEnumerable().Max(row => row["TGH_TO_DATE"]));
                                    hdfSavedStartDate.Value = RecentEndDate.ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DateConfig", "StartDateConfig(1);", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DateConfig", "StartDateConfig(2);", true);
                            }
                        }
                        break;
                    #endregion
                    #region DETAILSWITHPK - For Editing,Veiw - Bind
                    case ControlsEnum.DETAILSWITHPK:
                        GSTReturnHeaderObj = BusinessLogic.Finance.GSTReturnBL.GetDetailsWithPk(CurrPK, currentUser.SBUID, 1, 0);
                        if (GSTReturnHeaderObj != null)
                        {
                            GSTReturnHeaderSession = GSTReturnHeaderObj;
                            GSTReturnHeaderObj.TGH_PK = CurrPK;
                            EntryStatus = EntryStatus.EDITMODE;
                            LastModifiedTime = string.IsNullOrEmpty(GSTReturnHeaderObj.LAST_MOD_DT) ? DateTime.Now : Convert.ToDateTime(GSTReturnHeaderObj.LAST_MOD_DT);
                        }
                        else
                        {
                            GSTReturnHeaderObj = null;
                            GSTReturnHeaderSession = GSTReturnHeaderObj;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Edit_Delete").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.GSTReturn);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
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

            }
        }
        #endregion
        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Company Details
                    case ControlsEnum.SHOWCOMPANYDETAILS:
                        switch (EntryStatus)
                        {
                            case EntryStatus.NEWMODE:
                                txtPartA_Quest1.Text = HttpUtility.HtmlDecode(admCompanyMstList[0].CMP_GST_NO);
                                txtPartA_Quest2.Text = HttpUtility.HtmlDecode(admCompanyMstList[0].CMP_NAME);
                                txtGSTValueNo.Text = CurrPK == 0 ? "[NEW]" : "";
                                break;
                            case EntryStatus.LISTMODE:
                                break;
                            case EntryStatus.EDITMODE:
                                txtPartA_Quest1.Text = HttpUtility.HtmlDecode(admCompanyMstList[0].CMP_GST_NO);
                                txtPartA_Quest2.Text = HttpUtility.HtmlDecode(admCompanyMstList[0].CMP_NAME);
                                break;
                        }
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region GSTRETURNLIST - For Gridview
                    case ControlsEnum.GSTRETURNLIST:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region DETAILSWITHPK - For Editing,Veiw - Bind
                    case ControlsEnum.DETAILSWITHPK:
                        GetUIValuesFromObject(ControlsEnum.DETAILSWITHPK);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            //string BlockedExtensions = "dll";
            //if (System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower() != string.Empty)
            //{
            //    BlockedExtensions = System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower();
            //}
            bool flag = true;
            //string[] extensionList = BlockedExtensions.Split(',');
            //for (int i = 0; i < extensionList.Length; i++)
            //    if (("." + extensionList[i]) == extension)
            //    {
            //        flag = false;
            //        break;
            //    }
            return flag;
        }
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.GSTR, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            bool bIsChecked = false;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region SAVE
                    case ControlsEnum.SAVE:
                        if (GSTReturnHeaderSession != null)
                        {
                            GSTReturnHeaderObj = GSTReturnHeaderSession;
                            if (GSTReturnHeaderObj != null && GSTReturnHeaderObj.listGSTReturnDetails.Count > 0)
                            {
                                GSTReturnHeaderObj.TGH_PK = CurrPK;
                                GSTReturnHeaderObj.TGH_NO = string.Empty;
                                GSTReturnHeaderObj.TGH_FROM_DATE = Convert.ToDateTime(txtStartDate.Text).ToString();
                                GSTReturnHeaderObj.TGH_TO_DATE = Convert.ToDateTime(txtEndDate.Text).ToString();
                                GSTReturnHeaderObj.TGH_DUE_DATE = Convert.ToDateTime(txtReturnableDate.Text).ToString();
                                GSTReturnHeaderObj.TGH_STATUS = 0;
                                GSTReturnHeaderObj.TGH_DEPT = currentUser.CurrentDeptPK;
                                GSTReturnHeaderObj.TGH_COMPANY = Convert.ToInt32(ddlCompany.SelectedItem.Value);
                                GSTReturnHeaderObj.BIZUNIT_PK = currentUser.SBUID;
                                GSTReturnHeaderObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                GSTReturnHeaderObj.USER_PK = currentUser.PKUser;
                                GSTReturnHeaderObj.LAST_MOD_DT = LastModifiedTime.ToString();
                                GSTReturnHeaderObj.AST_CODE = ApplicationType.GSTR;//Genarate TrNo
                                GSTReturnHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;//Genarate TrNo
                                //Part D : declaration
                                GSTReturnHeaderObj.TGH_DEC_NAME = txtDeclName.Text;
                                GSTReturnHeaderObj.TGH_DEC_ID_NEW = txtDeclIdentityCardNewPart1.Text + "-" + txtDeclIdentityCardNewPart2.Text + "-" + txtDeclIdentityCardNewPart3.Text;
                                GSTReturnHeaderObj.TGH_DEC_ID_OLD = txtDeclIdentityCardOld.Text;
                                GSTReturnHeaderObj.TGH_DEC_PASSPORT_NO = txtDeclPassportNo.Text;
                                GSTReturnHeaderObj.TGH_DEC_NATIONALITY = txtDeclNationality.Text;
                                GSTReturnHeaderObj.TGH_DEC_DATE = txtDeclDate.Text != string.Empty ? Convert.ToDateTime(txtDeclDate.Text).ToString() : DateTime.Now.ToShortDateString();
                                //Details
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.RATEDSUPPLY].TGD_AMOUNT = txtPartB_Quest5Sub1.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.OUTPUTTAX].TGD_AMOUNT = txtPartB_Quest5Sub2.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.RATEDACQUISION].TGD_AMOUNT = txtPartB_Quest6Sub1.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.INPUTTAX].TGD_AMOUNT = txtPartB_Quest6Sub2.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.AMTPAYABLE].TGD_AMOUNT = txtPartB_Quest7.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.AMTCLAIMABLE].TGD_AMOUNT = txtPartB_Quest8.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.CARRYFWDREFUND].TGD_AMOUNT = chkPartB_Quest9.Checked == true ? "1" : "0";
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.RATEDSUPPLIES].TGD_AMOUNT = txtPartC_Quest10.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.EXPORTSUPPLIES].TGD_AMOUNT = txtPartC_Quest11.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.EXEMPTSUPPLIES].TGD_AMOUNT = txtPartC_Quest12.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GSTRELIEF].TGD_AMOUNT = txtPartC_Quest13.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GOODSIMPORTED].TGD_AMOUNT = txtPartC_Quest14.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GSTSUSPENDED].TGD_AMOUNT = txtPartC_Quest15.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GOODSACQUIRED].TGD_AMOUNT = txtPartC_Quest16.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.DEBTRELIEF].TGD_AMOUNT = txtPartC_Quest17.Text;
                                GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.DEBTRECOVERD].TGD_AMOUNT = txtPartC_Quest18.Text;
                                GSTReturnBreakDetail GSTReturnBreakDetail1Obj = new GSTReturnBreakDetail();
                                GSTReturnBreakDetail GSTReturnBreakDetail2Obj = new GSTReturnBreakDetail();
                                GSTReturnBreakDetail GSTReturnBreakDetail3Obj = new GSTReturnBreakDetail();
                                GSTReturnBreakDetail GSTReturnBreakDetail4Obj = new GSTReturnBreakDetail();
                                GSTReturnBreakDetail GSTReturnBreakDetail5Obj = new GSTReturnBreakDetail();
                                GSTReturnBreakDetail GSTReturnBreakDetailOtherObj = new GSTReturnBreakDetail();
                                //Codes 1
                                GSTReturnBreakDetail1Obj.TGS_PK = GSTReturnHeaderObj.listGSTReturnBreakDetail.Count > 0 ? GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEA].TGS_PK : 0;
                                GSTReturnBreakDetail1Obj.TGS_CODE = txtTaxCode1.Text;
                                GSTReturnBreakDetail1Obj.TGS_AMOUNT = txtTaxCodeValue1.Text;
                                GSTReturnBreakDetail1Obj.TGS_RATE = txtTaxCodePer1.Text;
                                //Codes 2
                                GSTReturnBreakDetail2Obj.TGS_PK = GSTReturnHeaderObj.listGSTReturnBreakDetail.Count > 0 ? GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEB].TGS_PK : 0;
                                GSTReturnBreakDetail2Obj.TGS_CODE = txtTaxCode2.Text;
                                GSTReturnBreakDetail2Obj.TGS_AMOUNT = txtTaxCodeValue2.Text;
                                GSTReturnBreakDetail2Obj.TGS_RATE = txtTaxCodePer2.Text;
                                //Codes 3
                                GSTReturnBreakDetail3Obj.TGS_PK = GSTReturnHeaderObj.listGSTReturnBreakDetail.Count > 0 ? GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEC].TGS_PK : 0;
                                GSTReturnBreakDetail3Obj.TGS_CODE = txtTaxCode3.Text;
                                GSTReturnBreakDetail3Obj.TGS_AMOUNT = txtTaxCodeValue3.Text;
                                GSTReturnBreakDetail3Obj.TGS_RATE = txtTaxCodePer3.Text;
                                //Codes 4
                                GSTReturnBreakDetail4Obj.TGS_PK = GSTReturnHeaderObj.listGSTReturnBreakDetail.Count > 0 ? GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODED].TGS_PK : 0;
                                GSTReturnBreakDetail4Obj.TGS_CODE = txtTaxCode4.Text;
                                GSTReturnBreakDetail4Obj.TGS_AMOUNT = txtTaxCodeValue4.Text;
                                GSTReturnBreakDetail4Obj.TGS_RATE = txtTaxCodePer4.Text;
                                //Codes 5
                                GSTReturnBreakDetail5Obj.TGS_PK = GSTReturnHeaderObj.listGSTReturnBreakDetail.Count > 0 ? GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEE].TGS_PK : 0;
                                GSTReturnBreakDetail5Obj.TGS_CODE = txtTaxCode5.Text;
                                GSTReturnBreakDetail5Obj.TGS_AMOUNT = txtTaxCodeValue5.Text;
                                GSTReturnBreakDetail5Obj.TGS_RATE = txtTaxCodePer5.Text;
                                //Others
                                GSTReturnBreakDetailOtherObj.TGS_PK = GSTReturnHeaderObj.listGSTReturnBreakDetail.Count > 0 ? GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEO].TGS_PK : 0;
                                GSTReturnBreakDetailOtherObj.TGS_CODE = GetLocalResourceObject("TaxCodeOther").ToString();
                                GSTReturnBreakDetailOtherObj.TGS_AMOUNT = txtTaxCodeOtherValue.Text;
                                GSTReturnBreakDetailOtherObj.TGS_RATE = txtTaxCodeOtherPer.Text;
                                if (GSTReturnHeaderObj.listGSTReturnBreakDetail.Count > 0)
                                {
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEA].TGS_PK = GSTReturnBreakDetail1Obj.TGS_PK;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEA].TGS_CODE = GSTReturnBreakDetail1Obj.TGS_CODE;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEA].TGS_AMOUNT = GSTReturnBreakDetail1Obj.TGS_AMOUNT;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEA].TGS_RATE = GSTReturnBreakDetail1Obj.TGS_RATE;

                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEB].TGS_PK = GSTReturnBreakDetail2Obj.TGS_PK;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEB].TGS_CODE = GSTReturnBreakDetail2Obj.TGS_CODE;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEB].TGS_AMOUNT = GSTReturnBreakDetail2Obj.TGS_AMOUNT;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEB].TGS_RATE = GSTReturnBreakDetail2Obj.TGS_RATE;

                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEC].TGS_PK = GSTReturnBreakDetail3Obj.TGS_PK;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEC].TGS_CODE = GSTReturnBreakDetail3Obj.TGS_CODE;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEC].TGS_AMOUNT = GSTReturnBreakDetail3Obj.TGS_AMOUNT;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEC].TGS_RATE = GSTReturnBreakDetail3Obj.TGS_RATE;

                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODED].TGS_PK = GSTReturnBreakDetail4Obj.TGS_PK;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODED].TGS_CODE = GSTReturnBreakDetail4Obj.TGS_CODE;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODED].TGS_AMOUNT = GSTReturnBreakDetail4Obj.TGS_AMOUNT;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODED].TGS_RATE = GSTReturnBreakDetail4Obj.TGS_RATE;

                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEE].TGS_PK = GSTReturnBreakDetail5Obj.TGS_PK;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEE].TGS_CODE = GSTReturnBreakDetail5Obj.TGS_CODE;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEE].TGS_AMOUNT = GSTReturnBreakDetail5Obj.TGS_AMOUNT;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEE].TGS_RATE = GSTReturnBreakDetail5Obj.TGS_RATE;

                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEO].TGS_PK = GSTReturnBreakDetailOtherObj.TGS_PK;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEO].TGS_CODE = GSTReturnBreakDetailOtherObj.TGS_CODE;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEO].TGS_AMOUNT = GSTReturnBreakDetailOtherObj.TGS_AMOUNT;
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEO].TGS_RATE = GSTReturnBreakDetailOtherObj.TGS_RATE;
                                }
                                else
                                {
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail.Add(GSTReturnBreakDetail1Obj);
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail.Add(GSTReturnBreakDetail2Obj);
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail.Add(GSTReturnBreakDetail3Obj);
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail.Add(GSTReturnBreakDetail4Obj);
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail.Add(GSTReturnBreakDetail5Obj);
                                    GSTReturnHeaderObj.listGSTReturnBreakDetail.Add(GSTReturnBreakDetailOtherObj);
                                }
                                retObject = GSTReturnHeaderObj;
                            }
                        }
                        break;
                    #endregion
                }
                return retObject;
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region SET
                    case ControlsEnum.SET:
                        if (GSTReturnHeaderObj != null && GSTReturnHeaderObj.listGSTReturnDetails.Count > 0)
                        {
                            txtReturnableDate.Text = Convert.ToDateTime(dueDate).ToString(Resources.Constants.DateFormatShort);
                            txtPartB_Quest5Sub1.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.RATEDSUPPLY].TGD_AMOUNT.ToString());
                            txtPartB_Quest5Sub2.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.OUTPUTTAX].TGD_AMOUNT.ToString());
                                                        
                            txtPartB_Quest6Sub1.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.RATEDACQUISION].TGD_AMOUNT.ToString());
                            txtPartB_Quest6Sub2.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.INPUTTAX].TGD_AMOUNT.ToString());
                            //Calculation : ( 5b - 6b)
                            // 5b = GSTReturnHeaderObj.listGSTReturnDetails[Convert.ToInt32(ControlsEnum.OUTPUTTAX)].TGD_AMOUNT.ToString()
                            // 6b = GSTReturnHeaderObj.listGSTReturnDetails[Convert.ToInt32(ControlsEnum.INPUTTAX)].TGD_AMOUNT.ToString()
                            decimal GSTAmountPayable = Convert.ToDecimal(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.OUTPUTTAX].TGD_AMOUNT)
                                                    - Convert.ToDecimal(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.INPUTTAX].TGD_AMOUNT);

                            txtPartB_Quest7.Text = GSTAmountPayable > 0 ? GetFormattedCurrency(GSTAmountPayable) : "0";
                            //txtPartB_Quest7.Text = GetFormattedCurrency(Convert.ToDecimal(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.OUTPUTTAX].TGD_AMOUNT)
                            //- Convert.ToDecimal(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.INPUTTAX].TGD_AMOUNT));
                            //old : - txtPartB_Quest7.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[Convert.ToInt32(ControlsEnum.AMTPAYABLE)].TGD_AMOUNT.ToString());
                            //Calculation : ( 6b - 5b)
                            decimal GSTAmountClaimable = Convert.ToDecimal(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.INPUTTAX].TGD_AMOUNT)
                                                    - Convert.ToDecimal(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.OUTPUTTAX].TGD_AMOUNT);
                            txtPartB_Quest8.Text = GSTAmountClaimable > 0 ? GetFormattedCurrency(GSTAmountClaimable) : "0";
                            //txtPartB_Quest8.Text = GetFormattedCurrency(Convert.ToDecimal(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.INPUTTAX].TGD_AMOUNT)
                            //- Convert.ToDecimal(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.OUTPUTTAX].TGD_AMOUNT));
                            //old : - txtPartB_Quest8.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[Convert.ToInt32(ControlsEnum.AMTCLAIMABLE)].TGD_AMOUNT.ToString());
                            txtPartC_Quest10.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.RATEDSUPPLIES].TGD_AMOUNT.ToString());
                            txtPartC_Quest11.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.EXPORTSUPPLIES].TGD_AMOUNT.ToString());
                            txtPartC_Quest12.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.EXEMPTSUPPLIES].TGD_AMOUNT.ToString());
                            txtPartC_Quest13.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GSTRELIEF].TGD_AMOUNT.ToString());
                            txtPartC_Quest14.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GOODSIMPORTED].TGD_AMOUNT.ToString());
                            txtPartC_Quest15.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GSTSUSPENDED].TGD_AMOUNT.ToString());
                            txtPartC_Quest16.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GOODSACQUIRED].TGD_AMOUNT.ToString());
                            txtPartC_Quest17.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.DEBTRELIEF].TGD_AMOUNT.ToString());
                            txtPartC_Quest18.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.DEBTRECOVERD].TGD_AMOUNT.ToString());
                        }
                        break;
                    #endregion
                    #region DETAILSWITHPK - For Editing,Veiw - Bind
                    case ControlsEnum.DETAILSWITHPK:
                        //Header
                        hdfLastModDate.Value = GSTReturnHeaderObj.TGH_MOD_DT;
                        LastModifiedTime = Convert.ToDateTime(hdfLastModDate.Value);
                        txtGSTValueNo.Text = GSTReturnHeaderObj.TGH_NO == string.Empty ? "[NEW]" : GSTReturnHeaderObj.TGH_NO;
                        txtStartDate.Text = Convert.ToDateTime(GSTReturnHeaderObj.TGH_FROM_DATE).ToString(Resources.Constants.DateFormatShort);
                        txtEndDate.Text = Convert.ToDateTime(GSTReturnHeaderObj.TGH_TO_DATE).ToString(Resources.Constants.DateFormatShort);
                        txtReturnableDate.Text = Convert.ToDateTime(GSTReturnHeaderObj.TGH_DUE_DATE).ToString(Resources.Constants.DateFormatShort);
                        //Questions 5-18
                        txtPartB_Quest5Sub1.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.RATEDSUPPLY].TGD_AMOUNT.ToString());
                        txtPartB_Quest5Sub2.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.OUTPUTTAX].TGD_AMOUNT.ToString());
                        txtPartB_Quest6Sub1.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.RATEDACQUISION].TGD_AMOUNT.ToString());
                        txtPartB_Quest6Sub2.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.INPUTTAX].TGD_AMOUNT.ToString());
                        txtPartB_Quest7.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.AMTPAYABLE].TGD_AMOUNT.ToString());
                        txtPartB_Quest8.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.AMTCLAIMABLE].TGD_AMOUNT.ToString());
                        chkPartB_Quest9.Checked = Convert.ToInt32(Convert.ToDecimal(GetFormattedNumber(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.CARRYFWDREFUND].TGD_AMOUNT))) == 1 ? true : false;
                        txtPartC_Quest10.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.RATEDSUPPLIES].TGD_AMOUNT.ToString());
                        txtPartC_Quest11.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.EXPORTSUPPLIES].TGD_AMOUNT.ToString());
                        txtPartC_Quest12.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.EXEMPTSUPPLIES].TGD_AMOUNT.ToString());
                        txtPartC_Quest13.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GSTRELIEF].TGD_AMOUNT.ToString());
                        txtPartC_Quest14.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GOODSIMPORTED].TGD_AMOUNT.ToString());
                        txtPartC_Quest15.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GSTSUSPENDED].TGD_AMOUNT.ToString());
                        txtPartC_Quest16.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.GOODSACQUIRED].TGD_AMOUNT.ToString());
                        txtPartC_Quest17.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.DEBTRELIEF].TGD_AMOUNT.ToString());
                        txtPartC_Quest18.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnDetails[(int)ControlsEnum.DEBTRECOVERD].TGD_AMOUNT.ToString());
                        //Codes
                        txtTaxCode1.Text = GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEA].TGS_CODE;
                        txtTaxCode2.Text = GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEB].TGS_CODE;
                        txtTaxCode3.Text = GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEC].TGS_CODE;
                        txtTaxCode4.Text = GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODED].TGS_CODE;
                        txtTaxCode5.Text = GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEE].TGS_CODE;
                        txtTaxCodeValue1.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEA].TGS_AMOUNT.ToString());
                        txtTaxCodeValue2.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEB].TGS_AMOUNT.ToString());
                        txtTaxCodeValue3.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEC].TGS_AMOUNT.ToString());
                        txtTaxCodeValue4.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODED].TGS_AMOUNT.ToString());
                        txtTaxCodeValue5.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEE].TGS_AMOUNT.ToString());
                        txtTaxCodeOtherValue.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEO].TGS_AMOUNT.ToString());
                        txtTaxCodePer1.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEA].TGS_RATE.ToString());
                        txtTaxCodePer2.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEB].TGS_RATE.ToString());
                        txtTaxCodePer3.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEC].TGS_RATE.ToString());
                        txtTaxCodePer4.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODED].TGS_RATE.ToString());
                        txtTaxCodePer5.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEE].TGS_RATE.ToString());
                        txtTaxCodeOtherPer.Text = GetFormattedCurrency(GSTReturnHeaderObj.listGSTReturnBreakDetail[(int)ControlsEnum.CODEO].TGS_RATE.ToString());
                        //For Calculating percentage and total values
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PerCalculations", "$(document).ready(function(){CalculateOutputTax(this);});", true);
                        //Declartion
                        txtDeclName.Text = GSTReturnHeaderObj.TGH_DEC_NAME;
                        string declNewId = GSTReturnHeaderObj.TGH_DEC_ID_NEW;
                        string[] declNewIdArr = declNewId.Split('-');
                        txtDeclIdentityCardNewPart1.Text = declNewIdArr[0].ToString();
                        txtDeclIdentityCardNewPart2.Text = declNewIdArr[1].ToString();
                        txtDeclIdentityCardNewPart3.Text = declNewIdArr[2].ToString();
                        txtDeclIdentityCardOld.Text = GSTReturnHeaderObj.TGH_DEC_ID_OLD;
                        txtDeclPassportNo.Text = GSTReturnHeaderObj.TGH_DEC_PASSPORT_NO;
                        txtDeclNationality.Text = GSTReturnHeaderObj.TGH_DEC_NATIONALITY;
                        txtDeclDate.Text = GSTReturnHeaderObj.TGH_DEC_DATE != string.Empty ? Convert.ToDateTime(GSTReturnHeaderObj.TGH_DEC_DATE).ToString(GetLocalResourceObject("DeclDateFormat").ToString()) : string.Empty;
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region COMPANY
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                        if (dtCurrentCompany != null && dtCurrentCompany.Rows.Count > 0)
                        {
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCurrentCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                            ddlCompany.Enabled = false;
                        }
                    }
                    break;
                #endregion
            }
        }
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region GSTRETURNLIST - For Gridview
                    case ControlsEnum.GSTRETURNLIST:
                        grdGSTReturnList.DataSource = null;
                        if (dsGSTReturn != null && dsGSTReturn.Tables[0].Rows.Count > 0)
                        {
                            grdGSTReturnList.DataSource = dsGSTReturn;
                        }
                        grdGSTReturnList.DataBind();
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                if (Mode == ActionsEnum.VIEW)
                {
                    EntryStatus = EntryStatus.VIEWMODE;
                }
                else
                {
                    EntryStatus = EntryStatus.ENTRYMODE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Clear -- After Save
                case ControlsEnum.CLEARFORM:

                    GSTReturnHeaderSession = null;
                    GSTReturnHeaderObj = null;
                    txtStartDate.Text = string.Empty;
                    txtEndDate.Text = string.Empty;
                    txtReturnableDate.Text = string.Empty;
                    //Question 5-18
                    txtPartB_Quest5Sub1.Text = string.Empty;
                    txtPartB_Quest5Sub2.Text = string.Empty;
                    txtPartB_Quest6Sub1.Text = string.Empty;
                    txtPartB_Quest6Sub2.Text = string.Empty;
                    txtPartB_Quest7.Text = string.Empty;
                    txtPartB_Quest8.Text = string.Empty;
                    chkPartB_Quest9.Checked = false;
                    txtPartC_Quest10.Text = string.Empty;
                    txtPartC_Quest11.Text = string.Empty;
                    txtPartC_Quest12.Text = string.Empty;
                    txtPartC_Quest13.Text = string.Empty;
                    txtPartC_Quest14.Text = string.Empty;
                    txtPartC_Quest15.Text = string.Empty;
                    txtPartC_Quest16.Text = string.Empty;
                    txtPartC_Quest17.Text = string.Empty;
                    txtPartC_Quest18.Text = string.Empty;
                    //Codes
                    txtTaxCode1.Text = GetLocalResourceObject("TaxCode1").ToString();
                    txtTaxCode2.Text = GetLocalResourceObject("TaxCode2").ToString();
                    txtTaxCode3.Text = GetLocalResourceObject("TaxCode3").ToString();
                    txtTaxCode4.Text = GetLocalResourceObject("TaxCode4").ToString();
                    txtTaxCode5.Text = GetLocalResourceObject("TaxCode5").ToString();
                    txtTaxCodeValue1.Text = string.Empty;
                    txtTaxCodeValue2.Text = string.Empty;
                    txtTaxCodeValue3.Text = string.Empty;
                    txtTaxCodeValue4.Text = string.Empty;
                    txtTaxCodeValue5.Text = string.Empty;
                    txtTaxCodeOtherValue.Text = string.Empty;
                    txtTaxCodeTotalValue.Text = string.Empty;
                    txtTaxCodePer1.Text = string.Empty;
                    txtTaxCodePer2.Text = string.Empty;
                    txtTaxCodePer3.Text = string.Empty;
                    txtTaxCodePer4.Text = string.Empty;
                    txtTaxCodePer5.Text = string.Empty;
                    txtTaxCodeOtherPer.Text = string.Empty;
                    txtTaxCodeTotalPer.Text = string.Empty;
                    //Declaration
                    txtDeclName.Text = string.Empty;
                    txtDeclIdentityCardNewPart1.Text = string.Empty;
                    txtDeclIdentityCardNewPart2.Text = string.Empty;
                    txtDeclIdentityCardNewPart3.Text = string.Empty;
                    txtDeclIdentityCardOld.Text = string.Empty;
                    txtDeclPassportNo.Text = string.Empty;
                    txtDeclNationality.Text = string.Empty;
                    txtDeclDate.Text = string.Empty;
                    break;
                #endregion
            }
        }
        private void setvisibility(ActionsEnum ActionsEnum)
        {
            switch (ActionsEnum)
            {
            }
        }
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        #endregion
        #region WorkFlow Methods
        /// <summary>
        /// GetApplicationID
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private int GetApplicationID(int refId)
        {
            int appId = 0;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtApplication = wrkfService.GetApplicationID(refId);
            if (dtApplication != null)
            {
                if (dtApplication.Rows.Count > 0)
                {
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessID()
        {
            int processID = 0;
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    path = Resources.PageURL.GSTReturn.Replace("~", "");
                    base.WkfPageUrl = ucrWrkf.PageUrl = path;
                    PageProcessID = ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    processID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                }
            }

            return processID;
        }
        private string GetUrl()
        {
            string path = string.Empty;

            return path;
        }
        #endregion
        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            CommonService CommonServiceClient;
            CommonServiceClient = null;
            try
            {
                int? result;
                bool bIsChecked = false;
                DropDownList ddlWkfAction;
                TextBox WrkfComments;
                string action;
                string savePath = string.Empty;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlCompany")
                    {
                        commonActions = ActionsEnum.SHOW;
                    }
                    //if (((DropDownList)sender).ID == "ddlHoldAccount")
                    //{
                    //    commonActions = ActionsEnum.FCHOLDREVERTDTL;
                    //}
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    //if (((TextBox)sender).ID == "txtReverseNow")
                    //{
                    //    commonActions = ActionsEnum.CHECKAMT;
                    //}
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                switch (commonActions)
                {
                    #region SHOW -- Company dropdown change
                    case ActionsEnum.SHOW:
                        ResetForm(ControlsEnum.CLEARFORM);
                        GetFieldValues(ControlsEnum.SHOWCOMPANYDETAILS);
                        SetFieldValues(ControlsEnum.SHOWCOMPANYDETAILS);
                        break;
                    #endregion
                    #region SET -- Go btn Function
                    case ActionsEnum.SET:
                        GetFieldValues(ControlsEnum.SET);
                        GetUIValuesFromObject(ControlsEnum.SET);
                        break;
                    #endregion
                    #region LIST
                    case ActionsEnum.LIST:
                        CurrPK = 0;
                        hdfCurrentPk.Value = CurrPK.ToString();
                        GetFieldValues(ControlsEnum.GSTRETURNLIST);
                        SetFieldValues(ControlsEnum.GSTRETURNLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Details
                    case ActionsEnum.DETAILS:
                        foreach (GridViewRow grdrow in grdGSTReturnList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfGSTReturnID")).Value);
                                hdfCurrentPk.Value = CurrPK.ToString();
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.DETAILSWITHPK);
                            SetFieldValues(ControlsEnum.DETAILSWITHPK);
                            GetFieldValues(ControlsEnum.SHOWCOMPANYDETAILS);
                            SetFieldValues(ControlsEnum.SHOWCOMPANYDETAILS);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_GSTReturn").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        if (!IsAnyDraft)
                        {
                            EntryStatus = EntryStatus.NEWMODE;
                            CurrPK = 0;
                            hdfCurrentPk.Value = CurrPK.ToString();
                            ResetForm(ControlsEnum.CLEARFORM);
                            GetFieldValues(ControlsEnum.SHOWCOMPANYDETAILS);
                            SetFieldValues(ControlsEnum.SHOWCOMPANYDETAILS);

                            GetFieldValues(ControlsEnum.GSTRETURNLIST);
                            if (dtGSTReturn != null)
                            {
                                if (dtGSTReturn.Rows.Count > 0)
                                {
                                    RecentEndDate = Convert.ToDateTime(dtGSTReturn.AsEnumerable().Max(row => row["TGH_TO_DATE"]));
                                    DateTime nextDate = Convert.ToDateTime(RecentEndDate.ToString());
                                    nextDate = nextDate.AddDays(1);
                                    hdfSavedStartDate.Value = nextDate.ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DateConfig", "StartDateConfig(1);", true);
                                }
                            }
                            txtStartDate.Text = (RecentEndDate != null ? Convert.ToDateTime(RecentEndDate).AddDays(1).ToString(Resources.Constants.DateFormatShort) : "");
                            txtEndDate.Text = (RecentEndDate != null ? Convert.ToDateTime(RecentEndDate).AddDays(1).AddMonths(1).AddDays(-1).ToString(Resources.Constants.DateFormatShort) : "");
                            endDate = Convert.ToDateTime(RecentEndDate).AddDays(1).AddMonths(1).AddDays(-1);
                            startOfMonth = new DateTime(endDate.Year, endDate.Month, 1);
                            dueDate = startOfMonth.AddMonths(1).AddDays(-1);
                            txtReturnableDate.Text = (RecentEndDate != null ? Convert.ToDateTime(dueDate).ToString(Resources.Constants.DateFormatShort) : "");
                            FillProcessID();
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                                ucrWrkf.ViewType = 0;
                            ucrWrkf.ViewAction();
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("ErrMsg_IsDraft").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region EDIT
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdGSTReturnList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfGSTReturnID")).Value);
                                hdfCurrentPk.Value = CurrPK.ToString();
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            GetFieldValues(ControlsEnum.DETAILSWITHPK);
                            SetFieldValues(ControlsEnum.DETAILSWITHPK);
                            GetFieldValues(ControlsEnum.SHOWCOMPANYDETAILS);
                            SetFieldValues(ControlsEnum.SHOWCOMPANYDETAILS);

                            FillProcessID();
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_GSTReturn").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        CurrPK = 0;
                        hdfCurrentPk.Value = CurrPK.ToString();
                        ResetForm(ControlsEnum.CLEARFORM);
                        GetFieldValues(ControlsEnum.GSTRETURNLIST);
                        SetFieldValues(ControlsEnum.GSTRETURNLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            if (GSTReturnHeaderSession != null)
                            {
                                GSTReturnHeaderObj = (GSTReturnHeader)SetUIValuesToObject(ControlsEnum.SAVE);
                                GSTReturnHeaderObj.WKF_FLAG = 0;
                                if (GSTReturnHeaderObj != null)
                                {
                                    string xmlDoc = CommonFunctions.XmlSerialize<GSTReturnHeader>(GSTReturnHeaderObj);
                                    result = BusinessLogic.Finance.GSTReturnBL.SaveGSTReturn(xmlDoc);
                                    if (result > 0)
                                    {
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.GSTReturn);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEARFORM);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        GetFieldValues(ControlsEnum.GSTRETURNLIST);
                                        SetFieldValues(ControlsEnum.GSTRETURNLIST);
                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + GetLocalResourceObject("RefNoExist").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.miscellaneous);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_SaveNoDataFound").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.VatSaleExport);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("ErrMsg_SaveNoDataFound").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.VatSaleExport);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup   
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup   
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region case ActionsEnum.WRKFSUBMIT:
                    case ActionsEnum.WRKFSUBMIT:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            #region SAVE&SUBMIT
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                if (GSTReturnHeaderSession != null)
                                {
                                    GSTReturnHeaderObj = (GSTReturnHeader)SetUIValuesToObject(ControlsEnum.SAVE);
                                    GSTReturnHeaderObj.WKF_FLAG = 1;
                                    if (GSTReturnHeaderObj != null)
                                    {
                                        string xmlDoc = CommonFunctions.XmlSerialize<GSTReturnHeader>(GSTReturnHeaderObj);
                                        result = BusinessLogic.Finance.GSTReturnBL.SaveGSTReturn(xmlDoc);
                                        if (result > 0)
                                        {
                                            ucrWrkf.ApplicationID = result.Value;
                                        }
                                        else
                                        {
                                            if (result == (int)DbSaveStatus.SQLERROR)
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + Resources.Messages.EditUsedByAnotherUser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.miscellaneous + " " + GetLocalResourceObject("RefNoExist").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.miscellaneous);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_SaveNoDataFound").ToString();
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.VatSaleExport);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("ErrMsg_SaveNoDataFound").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.VatSaleExport);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            #endregion
                            else
                                ucrWrkf.ApplicationID = CurrPK;
                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result.HasValue && result.Value > 0)
                                    {
                                        ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                                        //Show Save success message and reset Contract Entry
                                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        {
                                            FillProcessID();
                                            litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                                            AdmTrxLogDet.ATL_ACTION = (byte)LogAction.CANCEL;
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                            AdmTrxLogDet.ATL_ACTION = (byte)LogAction.SUBMIT;
                                        }
                                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                        WrkfComments.Text = "";
                                        CurrPK = ucrWrkf.ApplicationID;
                                        GetFieldValues(ControlsEnum.DETAILSWITHPK);
                                        if (GSTReturnHeaderObj != null)
                                            txtGSTValueNo.Text = GSTReturnHeaderObj.TGH_NO;
                                        object[] args = new object[2];
                                        args[0] = GetLocalResourceObject("GSTTrNo").ToString(); //Resources.PageNameRes.Expenses;
                                        args[1] = txtGSTValueNo.Text.Trim() == "[NEW]" ? "" : txtGSTValueNo.Text;
                                        #region LOG SAVE
                                        CommonServiceClient = new CommonService();
                                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                                        List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                                        AdmTrxLogDet.ATL_APP_TRX_CODE = GSTReturnHeaderObj.TGH_NO;
                                        AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.GSTR;
                                        AdmTrxLogDet.ATL_MOD_BY = GSTReturnHeaderObj.USER_PK;
                                        AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                                        AdmTrxLogDet.ATL_BIZUNIT = GSTReturnHeaderObj.BIZUNIT_PK;
                                        AdmTrxLogDet.ATL_APP_TRX_PK = GSTReturnHeaderObj.TGH_PK;
                                        AdmTrxLogDet.ATL_PK = 0;
                                        AdmTrxLogList.Add(AdmTrxLogDet);
                                        CommonServiceClient.SaveLog(AdmTrxLogList);
                                        #endregion
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEARFORM);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        GetFieldValues(ControlsEnum.GSTRETURNLIST);
                                        SetFieldValues(ControlsEnum.GSTRETURNLIST);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region PRINTLIST
                    case ActionsEnum.PRINTLIST:
                        foreach (GridViewRow grdrow in grdGSTReturnList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfGSTReturnID")).Value);
                                hdfCurrentPk.Value = CurrPK.ToString();
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK + "&APPTYPE=" + "GST" + "&APPSUBTYPE=7") + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_GSTReturn").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        CurrPK = Convert.ToInt32(hdfCurrentPk.Value);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK + "&APPTYPE=" + "GST" + "&APPSUBTYPE=7") + "');", true);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        CurrPK = Convert.ToInt32(hdfCurrentPk.Value);
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Finance.GSTReturnBL.DeleteGSTReturn(CurrPK, LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Delete_Success").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalesInvoice);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.CLEARFORM);
                                IsAnyDraft = false;
                                GetFieldValues(ControlsEnum.GSTRETURNLIST);
                                SetFieldValues(ControlsEnum.GSTRETURNLIST);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalesInvoice + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalesInvoice);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {
                CommonServiceClient = null;
            }
        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// Page Index Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {

        }
        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
        }
        #endregion
        #endregion
        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnNew.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnListPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);

            btnSave.Load += new EventHandler(btnAction_Load);
            btnDelete.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnNew.Load += new EventHandler(btnAction_Load);
            btnEdit.Load += new EventHandler(btnAction_Load);
            btnListPrint.Load += new EventHandler(btnAction_Load);
            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
        }

        /// <summary>
        /// Button Load event
        /// Resets visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {

        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {

        }

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    //lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(1);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CodeCalculations", "$(document).ready(function(){DefaultOutputTax();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PercCalculations", "$(document).ready(function(){CalculateOutputTax(this);});", true);

                    //lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing(1);});", true);
                    //lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing(1);});", true);
                    //lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            #region -- Used for textbox binding
            RATEDSUPPLY = 0,
            OUTPUTTAX = 1,
            RATEDACQUISION = 2,
            INPUTTAX = 3,
            AMTPAYABLE = 4,
            AMTCLAIMABLE = 5,
            CARRYFWDREFUND = 6,
            RATEDSUPPLIES = 7,
            EXPORTSUPPLIES = 8,
            EXEMPTSUPPLIES = 9,
            GSTRELIEF = 10,
            GOODSIMPORTED = 11,
            GSTSUSPENDED = 12,
            GOODSACQUIRED = 13,
            DEBTRELIEF = 14,
            DEBTRECOVERD = 15,
            #endregion
            #region -- BREAK DOWN VALUE
            CODEA = 0,
            CODEB = 1,
            CODEC = 2,
            CODED = 3,
            CODEE = 4,
            CODEO = 5,
            #endregion
            SET,
            COMPANY,
            SHOWCOMPANYDETAILS,
            SAVE,
            CLEARFORM,
            GSTRETURNLIST,
            DETAILSWITHPK
        }
        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2
        }
        #endregion
    }
}