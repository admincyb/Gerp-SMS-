using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Finance.Administration.Masters;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject.AccountManagement;
using ERPSMS_v01.UserControls;
using BusinessObject.Finance.Administration.Masters;
using BusinessObject.CommonManagement;
using System.Data;
using System.Threading;
namespace ERPSMS_v01.Finance.Administration.Masters
{
    public partial class BankGuarantee : ERP.Store.UI.WorkFlowBasePage//System.Web.UI.Page
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
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
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex //-- for grid
        {
            get
            {
                return (this.ViewState[ViewstateStrings.PageIndex] == null ? 0 : (int)this.ViewState[ViewstateStrings.PageIndex]);
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
        #endregion
        private ActionsEnum commonActions;
        //page related class objects  
        private BankGuaranteeHeader objBankGuaranteeHeader;
        private DataTable dtBankGuarantee;
        private BusinessObject.User currentUser;
        #endregion
        #region PageLevel Events
        /// <summary>
        /// page Load event
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
                if (!IsPostBack)
                {
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = "BGM_PK";
                    grdList.DataKeyNames = datakeyarray;
                    /*Number Format START*/
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
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
                    hdfExchangeDigits.Value = rateDecimalDigits.ToString();
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }
                    /*Number Format END*/
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            int searchByPk = 0;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region List
                    case ControlsEnum.LIST:
                        dtBankGuarantee = BankGuaranteeBL.GetKVBankGuarantee(CurrPK, (int)DbActiveStatus.ALL, currentUser.SBUID
                            , 0, 0, PageIndex, Convert.ToInt32(GetLocalResourceObject("pageSize")), SortBy, SortDirection);
                        break;
                    #endregion
                    #region BankGuarantee
                    case ControlsEnum.BANKGUARANTEE:
                        dtBankGuarantee = BankGuaranteeBL.GetKVBankGuarantee(CurrPK, (int)DbActiveStatus.HASPK, currentUser.SBUID
                            , 0, 0, 1, Convert.ToInt32(GetLocalResourceObject("pageSize")), SortBy, SortDirection);
                        break;
                    #endregion
                    #region Search
                    case ControlsEnum.SEARCH:
                        searchByPk = hdfSearchBy.Value != string.Empty ? Convert.ToInt32(hdfSearchBy.Value) : 0;
                        if (ddlFilterBy.SelectedItem.Value == "Bank")
                            dtBankGuarantee = BankGuaranteeBL.GetKVBankGuarantee(CurrPK, (int)DbActiveStatus.ALL, currentUser.SBUID
                            , 0, searchByPk, PageIndex, Convert.ToInt32(GetLocalResourceObject("pageSize")), SortBy, SortDirection);
                        else if (ddlFilterBy.SelectedItem.Value == "Party")
                            dtBankGuarantee = BankGuaranteeBL.GetKVBankGuarantee(CurrPK, (int)DbActiveStatus.ALL, currentUser.SBUID
                            , searchByPk, 0, PageIndex, Convert.ToInt32(GetLocalResourceObject("pageSize")), SortBy, SortDirection);
                        break;
                    #endregion
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
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
                    #region List
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region BankGuarantee
                    case ControlsEnum.BANKGUARANTEE:
                        GetUIValuesFromObject(ControlsEnum.BANKGUARANTEE);
                        break;
                    #endregion
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Helper Methods
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
                    #region Save
                    case ControlsEnum.SAVE:
                        objBankGuaranteeHeader = new BankGuaranteeHeader();
                        objBankGuaranteeHeader.BGM_PK = CurrPK;
                        objBankGuaranteeHeader.BGM_NO = lblNoText.Text != GetLocalResourceObject("NewNo").ToString() ? lblNoText.Text.Trim().HtmlEncode() : string.Empty;
                        objBankGuaranteeHeader.BGM_DATE = txtDate.Text != string.Empty ? Convert.ToDateTime(txtDate.Text.Trim()).ToString() : null;
                        objBankGuaranteeHeader.BGM_REF_NO = txtRefNo.Text != string.Empty ? txtRefNo.Text.Trim() : null;
                        objBankGuaranteeHeader.BGM_REF_DATE = txtRefDate.Text != string.Empty ? Convert.ToDateTime(txtRefDate.Text.Trim()).ToString() : null;
                        objBankGuaranteeHeader.BGM_PARTY = hdfParty.Value != null ? Convert.ToInt32(hdfParty.Value) : 0;
                        objBankGuaranteeHeader.BGM_BANK = hdfBank.Value != null ? Convert.ToInt32(hdfBank.Value) : 0;
                        objBankGuaranteeHeader.BGM_AMOUNT = txtAmount.Text != null ? txtAmount.Text.Trim() : null;
                        objBankGuaranteeHeader.BGM_EXP_DATE = txtExpiryDate.Text != string.Empty ? Convert.ToDateTime(txtExpiryDate.Text.Trim()).ToString() : null;
                        objBankGuaranteeHeader.BGM_FROM = txtPeriod.Text != string.Empty ? txtPeriod.Text.Trim() : null;
                        objBankGuaranteeHeader.BGM_TO = txtPeriodTo.Text != string.Empty ? txtPeriodTo.Text.Trim() : null;
                        objBankGuaranteeHeader.BGM_CLAIM_DATE = txtClaimDate.Text != string.Empty ? Convert.ToDateTime(txtClaimDate.Text.Trim()).ToString() : null;
                        objBankGuaranteeHeader.BGM_REMARKS = txtRemarks.Text != string.Empty ? txtRemarks.Text.Trim() : null;
                        objBankGuaranteeHeader.BGM_DEPT = currentUser.CurrentDeptPK;
                        objBankGuaranteeHeader.BGM_COMPANY = currentUser.SBUID;
                        objBankGuaranteeHeader.BIZUNIT_PK = currentUser.SBUID;
                        objBankGuaranteeHeader.ACTIVE = Convert.ToInt32(ddlStatus.SelectedItem.Value);
                        objBankGuaranteeHeader.USER_PK = currentUser.PKUser;
                        objBankGuaranteeHeader.LAST_MOD_DT = LastModifiedTime.ToString();
                        objBankGuaranteeHeader.WKF_FLAG = 0;
                        retObject = objBankGuaranteeHeader;
                        break;
                    #endregion
                }
                return retObject;
            }
            catch { throw; }
            finally { }
        }
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region BankGuarantee
                    case ControlsEnum.BANKGUARANTEE:
                        if (dtBankGuarantee != null && dtBankGuarantee.Rows.Count > 0)
                        {
                            lblNoText.Text = dtBankGuarantee.Rows[0]["BGM_NO"].ToString().HtmlDecode();
                            txtDate.Text = dtBankGuarantee.Rows[0]["BGM_DATE"].ToString() != string.Empty
                                ? Convert.ToDateTime(dtBankGuarantee.Rows[0]["BGM_DATE"]).ToString(Resources.Constants.DateFormatShort)
                                : string.Empty;
                            txtRefNo.Text = dtBankGuarantee.Rows[0]["BGM_REF_NO"].ToString().HtmlDecode();
                            txtRefDate.Text = dtBankGuarantee.Rows[0]["BGM_REF_DATE"].ToString() != string.Empty
                                ? Convert.ToDateTime(dtBankGuarantee.Rows[0]["BGM_REF_DATE"]).ToString(Resources.Constants.DateFormatShort)
                                : string.Empty;
                            txtParty.Text = dtBankGuarantee.Rows[0]["BGM_PARTY_TEXT"].ToString().HtmlDecode();
                            hdfParty.Value = dtBankGuarantee.Rows[0]["BGM_PARTY"].ToString();
                            txtAmount.Text = GetFormattedCurrency(dtBankGuarantee.Rows[0]["BGM_AMOUNT"]);
                            txtBank.Text = dtBankGuarantee.Rows[0]["BGM_BANK_TEXT"].ToString().HtmlDecode();
                            hdfBank.Value = dtBankGuarantee.Rows[0]["BGM_BANK"].ToString();
                            txtExpiryDate.Text = dtBankGuarantee.Rows[0]["BGM_EXP_DATE"].ToString() != string.Empty
                                ? Convert.ToDateTime(dtBankGuarantee.Rows[0]["BGM_EXP_DATE"]).ToString(Resources.Constants.DateFormatShort)
                                : string.Empty;
                            txtPeriod.Text = dtBankGuarantee.Rows[0]["BGM_FROM"].ToString() != string.Empty
                                ? Convert.ToDateTime(dtBankGuarantee.Rows[0]["BGM_FROM"]).ToString(Resources.Constants.DateFormatShort)
                                : string.Empty;
                            txtPeriodTo.Text = dtBankGuarantee.Rows[0]["BGM_TO"].ToString() != string.Empty
                                ? Convert.ToDateTime(dtBankGuarantee.Rows[0]["BGM_TO"]).ToString(Resources.Constants.DateFormatShort)
                                : string.Empty;
                            txtClaimDate.Text = dtBankGuarantee.Rows[0]["BGM_CLAIM_DATE"].ToString() != string.Empty
                                ? Convert.ToDateTime(dtBankGuarantee.Rows[0]["BGM_CLAIM_DATE"]).ToString(Resources.Constants.DateFormatShort)
                                : string.Empty;
                            txtRemarks.Text = dtBankGuarantee.Rows[0]["BGM_REMARKS"].ToString().HtmlDecode();
                            ddlStatus.SelectedValue = dtBankGuarantee.Rows[0]["BGM_ACTIVE"].ToString();
                            LastModifiedTime = Convert.ToDateTime(dtBankGuarantee.Rows[0]["LAST_MOD_DT"]);
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex) { throw ex; }
        }
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType) { }
        }
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region List
                    case ControlsEnum.LIST:
                        grdList.DataSource = null;
                        uclPaging.Visible = false;
                        if (dtBankGuarantee != null && dtBankGuarantee.Rows.Count > 0)
                        {
                            grdList.DataSource = dtBankGuarantee;
                            int rowCount = 0;
                            int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                            if (dtBankGuarantee.Rows.Count > 0)
                                rowCount = Convert.ToInt32(dtBankGuarantee.Rows[0]["TOTAL_ROW_COUNT"]);
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                  (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                  (rowCount / pageSize) + 1;
                            uclPaging.CurrentPage = PageIndex;
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        grdList.DataBind();
                        break;
                    #endregion
                }
            }
            catch (Exception ex) { throw ex; }
        }
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Clear
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    lblNoText.Text = GetLocalResourceObject("NewNo").ToString();
                    txtDate.Text = string.Empty;
                    txtRefNo.Text = string.Empty;
                    txtRefDate.Text = string.Empty;
                    txtParty.Text = "Select/Type";
                    hdfParty.Value = CommonConstants.SELECT_VALUE_ZERO;
                    txtAmount.Text = string.Empty;
                    txtBank.Text = "Select/Type";
                    hdfBank.Value = CommonConstants.SELECT_VALUE_ZERO;
                    txtExpiryDate.Text = string.Empty;
                    txtPeriod.Text = string.Empty;
                    txtPeriodTo.Text = string.Empty;
                    txtClaimDate.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    break;
                #endregion
            }
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdList.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    if (rbtn.Checked)// check row selected or not
                    {
                        CurrPK = Convert.ToInt32(grdList.DataKeys[grdrow.RowIndex].Values[0]);// get pk from the grid and assign to CurrPk
                        GetFieldValues(ControlsEnum.BANKGUARANTEE);
                        SetFieldValues(ControlsEnum.BANKGUARANTEE);
                        ModifiedDatePnl.Visible = true;
                        if (Mode == ActionsEnum.VIEW)
                            EntryStatus = EntryStatus.VIEWMODE;
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;
                        return;
                    }
                }
                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex) { throw ex; }
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
        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept("../../../login.aspx"))
                return;

            try
            {
                int? result;
                bool bIsChecked = false;
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
                    //if (((DropDownList)sender).ID == "ddlCurrency"){commonActions = ActionsEnum.BANKCURRENCY;}
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    //if (((TextBox)sender).ID == "txtReverseNow"){commonActions = ActionsEnum.CHECKAMT;}
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    //if (((RadioButton)sender).ID == "rbtSelect"){commonActions = ActionsEnum.ITEMSELECTED;}
                }
                switch (commonActions)
                {
                    #region New
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.CLEAR);
                        ModifiedDatePnl.Visible = false;
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            objBankGuaranteeHeader = (BankGuaranteeHeader)SetUIValuesToObject(ControlsEnum.SAVE);
                            if (objBankGuaranteeHeader != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<BankGuaranteeHeader>(objBankGuaranteeHeader);
                                result = BankGuaranteeBL.SaveBankGuarantee(xmlDoc);
                                if (result > 0)
                                {
                                    ResetForm(ControlsEnum.CLEAR);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BankGuarantee);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                }
                                else
                                {
                                    #region Messages
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.BankGuarantee + " " + Resources.Messages.EditUsedByAnotherUser;
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.BankGuarantee + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    }
                                    else if (result == (int)DbSaveStatus.REFNOEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.BankGuarantee + " " + GetLocalResourceObject("RefNoExist").ToString();
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BankGuarantee);
                                    }
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    #endregion
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Edit,Details
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAILS:
                        SetUIEditView(ActionsEnum.EDIT);
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(ActionsEnum.VIEW);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BankGuaranteeBL.DeleteBankGuarantee(CurrPK, LastModifiedTime);
                            if (result > 0)
                            {
                                if (grdList.Rows.Count == 1 && PageIndex > 1)
                                {
                                    PageIndex--;
                                }
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BankGuarantee);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','"
                                    + Resources.Messages.Information + "');", true);
                                ResetForm(ControlsEnum.CLEAR);
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                            }
                            else
                            {
                                #region Messages
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
                                #endregion
                            }
                        }
                        break;
                    #endregion
                    #region Search
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try { }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e) { }
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                SortBy = e.SortExpression;
                if (SortDirection == Resources.ErpRes.SortAscending)
                    SortDirection = Resources.ErpRes.SortDescending;
                else
                    SortDirection = Resources.ErpRes.SortAscending;
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #endregion
        #region Pager Methods + Init
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //////base.CheckBtnVisibility(sender);
        }
        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }
        private void EnableDisableButtons(int iTotalPages)
        {
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        uclPaging.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        // Decrement the first page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Increment the last page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the next page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the previous page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }
                PageIndex = uclPaging.CurrentPage;
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
            }
        }
        #endregion
        #region ControlEnum
        public enum ControlsEnum
        {
            CLEAR,
            LIST,
            SAVE,
            BANKGUARANTEE,
            SEARCH
        }
        #endregion
    }
}