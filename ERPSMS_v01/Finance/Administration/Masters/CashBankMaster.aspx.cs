using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPService.Inventory;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERPSMS_v01.UserControls;
using System.Data;

namespace ERPSMS_v01.Finance.Administration.Masters
{
    public partial class CashBankMaster : ERP.Store.UI.MyBasePage
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
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
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
        private FIN_CASH_BANK_MST  FinCashBankMstObj;
        private ADM_COUNTRY_MST  AdmCountryMstObj;
        private ADM_STATE_MST AdmStateMstObj;
        private ADM_CONST_MST AdmConstMstObj;
        private ServiceUtility serviceUtilityObj;
        //List for binding details to controls
        private List<FIN_CASH_BANK_MST> FinCashBankMstList;
        private List<ADM_COUNTRY_MST> AdmCountryMstList;
        private List<ADM_STATE_MST> AdmStateMstList;
        private List<ADM_CONST_MST> AdmConstMstList;

        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;
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
                    ConfigurationSettings();
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.BankPK;
                    grdCashBankMst.DataKeyNames = datakeyarray;

                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
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
            FinCashBankService FinCashBankServiceClient = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                CommonServiceClient = new CommonService();
                FinCashBankServiceClient = new FinCashBankService();
                FinCashBankServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinCashBankServiceClient);
                FinCashBankMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_CASH_BANK_MST>();
                AdmCountryMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COUNTRY_MST>();
                AdmStateMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_STATE_MST>();
                AdmConstMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_MST>();
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdCashBankMst.PageSize;
                        serviceUtilityObj.FilterBy = ddlFilterBy.SelectedValue == CommonConstants.SELECT_VALUE_ZERO ? null : ddlFilterBy.SelectedValue;
                        serviceUtilityObj.FilterValue = HttpUtility.HtmlEncode(txtSearchBy.Text.Trim());
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.BankPK : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        serviceUtilityObj.IsSBUSpecific = Convert.ToBoolean(hdfIsSBUBank.Value);
                        FinCashBankMstObj.CBM_PK = (short)CurrPK;
                        FinCashBankMstObj.CBM_BIZUNIT = currentUser.SBUID;
                        FinCashBankMstList = FinCashBankServiceClient.GetFinCashBank(FinCashBankMstObj, serviceUtilityObj);
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    case ControlsEnum.CASHBANK:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        FinCashBankMstObj.CBM_PK = (short)CurrPK;
                        FinCashBankMstList = FinCashBankServiceClient.GetFinCashBank(FinCashBankMstObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.COUNTRY:
                        AdmCountryMstObj.CNT_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                        AdmCountryMstList = CommonServiceClient.GetCountry(AdmCountryMstObj);
                        break;
                    case ControlsEnum.STATE:
                        //AdmStateMstObj.STT_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                        //AdmStateMstObj.STT_COUNTRY = Convert.ToInt32(ddlCountry.SelectedValue);
                        //AdmStateMstList = CommonServiceClient.GetStates(AdmStateMstObj);
                        break;
                    case ControlsEnum.ACCOUNTTYPE :
                        AdmConstMstList = CommonServiceClient.GetConstMstValues(null, 1, null, 9, 4, currentUser.SBUID);
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FinCashBankMstObj = null;
                serviceUtilityObj = null;
                AdmCountryMstObj = null;
                AdmConstMstObj = null;
                AdmStateMstObj = null;
                FinCashBankServiceClient = null;
                CommonServiceClient = null;
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
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        break;
                    case ControlsEnum.CASHBANK:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.COUNTRY:
                        BindCountryDropDown();
                        break;
                    case ControlsEnum.STATE :
                        BindStateDropDown();
                        break;
                    case ControlsEnum.ACCOUNTTYPE:
                        BindAccountTypeDropDown();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods

        private void ConfigurationSettings()
        {
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "BANK");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUBank.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }
            
        }
        /// <summary>
        // Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private FIN_CASH_BANK_MST SetUIValuesToObject()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                FinCashBankMstObj.CBM_PK = (short)CurrPK;
                FinCashBankMstObj.CBM_CODE = HttpUtility.HtmlEncode(txtCode.Text.Trim());
                FinCashBankMstObj.CBM_NAME = HttpUtility.HtmlEncode(txtName.Text.Trim());
                FinCashBankMstObj.CBM_DESC = HttpUtility.HtmlEncode(txtDesc.Text.Trim());
                FinCashBankMstObj.CBM_TYPE = (byte)CashBankType.Bank; //Convert.ToByte(ddlType.SelectedValue);
                FinCashBankMstObj.CBM_ACC_NO = txtAccountNumber.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtAccountNumber.Text.Trim()) : null ;
                FinCashBankMstObj.CBM_BRANCH = txtBranch.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtBranch.Text.Trim()) : null;
                FinCashBankMstObj.CBM_ADDRESS = txtAddress.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtAddress.Text.Trim()) : null;
                FinCashBankMstObj.CBM_CITY = txtCity.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtCity.Text.Trim()) : null;
                if (ddlCountry.SelectedValue != CommonConstants.SELECTVAL)
                    FinCashBankMstObj.CBM_COUNTRY = Convert.ToInt32(ddlCountry.SelectedValue);
                else
                    FinCashBankMstObj.CBM_COUNTRY = null;
                //FinCashBankMstObj.CBM_STATE = ddlState.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlState.SelectedValue) : FinCashBankMstObj.CBM_STATE;
                FinCashBankMstObj.CBM_STATE = null;
                FinCashBankMstObj.CBM_STATE_OTHER = txtState.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtState.Text.Trim()) : null;
                FinCashBankMstObj.CBM_ZIP = txtzip.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtzip.Text.Trim()) : null;
                FinCashBankMstObj.CBM_PHONE = txtPhone.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtPhone.Text.Trim()) : null;
                FinCashBankMstObj.CBM_MOBILE = txtMobile.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtMobile.Text.Trim()) : null;
                FinCashBankMstObj.CBM_FAX = txtFax.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtFax.Text.Trim()) : null;
                FinCashBankMstObj.CBM_EMAIL = txtEmail.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtEmail.Text.Trim()) : null;
                FinCashBankMstObj.CBM_IFSC_CODE = txtIFSCcode.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtIFSCcode.Text.Trim()) : null;
                FinCashBankMstObj.CBM_SWIFT_CODE = txtSWIFTCode.Text.Trim() != string.Empty ? HttpUtility.HtmlEncode(txtSWIFTCode.Text.Trim()) : null;
                if (chkIsHoldAccount.Checked == true)
                {
                    FinCashBankMstObj.CBM_FC_HOLD = 1;
                }
                else
                {
                    FinCashBankMstObj.CBM_FC_HOLD = 0;
                }
                if (ddlAccountType.SelectedValue != "-1")
                {
                    FinCashBankMstObj.CBM_ACCOUNT_TYPE = Convert.ToInt32(ddlAccountType.SelectedValue);
                }
                else
                {
                    FinCashBankMstObj.CBM_ACCOUNT_TYPE = null;
                }
                FinCashBankMstObj.CBM_CONTACT = null;
                if (hdfAccount.Value != string.Empty && hdfAccount.Value != "0")
                {
                    FinCashBankMstObj.CBM_ACCOUNT = Convert.ToInt32(hdfAccount.Value);
                }
                else
                {
                    FinCashBankMstObj.CBM_ACCOUNT = null;
                }
                if (hdfCurrency.Value != string.Empty && hdfCurrency.Value != "0")
                {
                    FinCashBankMstObj.CBM_CURRENCY = Convert.ToInt32(hdfCurrency.Value);
                }
                else
                    FinCashBankMstObj.CBM_CURRENCY = null;
                FinCashBankMstObj.CBM_DEPT = currentUser.CurrentDeptPK;
                FinCashBankMstObj.CBM_MOD_DT = LastModifiedTime;
                FinCashBankMstObj.CBM_ACTIVE = Convert.ToByte(ddlStatus.SelectedValue);
                FinCashBankMstObj.CBM_BIZUNIT = currentUser.SBUID;
                FinCashBankMstObj.CBM_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                FinCashBankMstObj.CBM_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                return FinCashBankMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FinCashBankMstObj = null;
            }
        }



        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            try
            {
                //assigning the UI controls with the corresponding ListObject value AsrSovMstList
                if (FinCashBankMstList != null && FinCashBankMstList.Count() > 0)
                {
                    CurrPK = FinCashBankMstList[0].CBM_PK;
                    txtCode.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_CODE);
                    txtName.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_NAME);
                    ddlStatus.SelectedValue = FinCashBankMstList[0].CBM_ACTIVE.ToString();
                    txtDesc.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_DESC);
                    //ddlType.SelectedValue = FinCashBankMstList[0].CBM_TYPE.ToString();
                    txtAccountNumber.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_ACC_NO);
                    txtBranch.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_BRANCH);
                    txtAddress.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_ADDRESS);
                    txtCity.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_CITY);
                    if (FinCashBankMstList[0].CBM_COUNTRY != null)
                    {
                        ddlCountry.SelectedValue = FinCashBankMstList[0].CBM_COUNTRY > 0 ? FinCashBankMstList[0].CBM_COUNTRY.ToString() : CommonConstants.SELECTVAL;
                        //GetFieldValues(ControlsEnum.STATE);
                        //SetFieldValues(ControlsEnum.STATE);
                        //if (FinCashBankMstList[0].CBM_STATE != null)
                        //{
                        //    ddlState.SelectedValue = FinCashBankMstList[0].CBM_STATE > 0 ? FinCashBankMstList[0].CBM_STATE.ToString() : CommonConstants.SELECTVAL;
                        //}
                    }
                    if (FinCashBankMstList[0].CBM_FC_HOLD == 1)
                    {
                        chkIsHoldAccount.Checked = true;
                    }
                    else 
                    {
                        chkIsHoldAccount.Checked = false;
                    }
                    txtState.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_STATE_OTHER);
                    txtzip.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_ZIP);
                    txtPhone.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_PHONE);
                    txtMobile.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_MOBILE);
                    txtFax.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_FAX);
                    txtEmail.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_EMAIL);
                    txtIFSCcode.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_IFSC_CODE);
                    txtSWIFTCode.Text = HttpUtility.HtmlDecode(FinCashBankMstList[0].CBM_SWIFT_CODE);
                    if (FinCashBankMstList[0].CBM_ACCOUNT_TYPE != null)
                    {
                        ddlAccountType.SelectedValue = FinCashBankMstList[0].CBM_ACCOUNT_TYPE > 0 ? FinCashBankMstList[0].CBM_ACCOUNT_TYPE.ToString() : CommonConstants.SELECTVAL;
                    }
                    if (FinCashBankMstList[0].CBM_ACCOUNT != null)
                    {
                        txtAccount.Text = FinCashBankMstList[0].FIN_COA_MST.COA_CODE + " - " + FinCashBankMstList[0].FIN_COA_MST.COA_NAME;
                        hdfAccount.Value = FinCashBankMstList[0].CBM_ACCOUNT.ToString();
                    }
                    if (FinCashBankMstList[0].CBM_CURRENCY != null)
                    {
                        txtCurrency.Text = FinCashBankMstList[0].ADM_CURRENCY_MST.CUR_CODE +" - "+ FinCashBankMstList[0].ADM_CURRENCY_MST.CUR_NAME;
                        hdfCurrency.Value = FinCashBankMstList[0].CBM_CURRENCY.ToString();
                    }

                    LastModifiedTime = FinCashBankMstList[0].CBM_MOD_DT;
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                }
                //Concurrency Account details Deleted By Another User
                else
                {
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CashBank);
                    EntryStatus = EntryStatus.LISTMODE;
                    ResetForm();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    ModifiedDatePnl.Visible = false;
                    throw new Exception(litErrorMsg.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid()
        {
            try
            {
                if (FinCashBankMstList != null)
                {
                    uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdCashBankMst.DataSource = FinCashBankMstList;
                    grdCashBankMst.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                }
                else
                {
                    uclPaging.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Type Dropdown
        /// </summary>
        public void BindTypeDropDown()
        {
            //ddlType.Items.Clear();
            //ddlType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            //ddlType.Items.Insert(1, new ListItem("Cash", "1"));
            //ddlType.Items.Insert(2, new ListItem("Bank", "2"));
        }

        /// <summary>
        /// Method for Status Dropdown
        /// </summary>
        public void BindStatusDropDown()
        {
            ddlStatus.Items.Clear();
            ddlStatus.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            ddlStatus.Items.Insert(1, new ListItem(Resources.Controls.Active, ((int)RecordStatus.ACTIVE).ToString()));
            ddlStatus.Items.Insert(2, new ListItem(Resources.Controls.InActive, ((int)RecordStatus.INACTIVE).ToString()));
        }

         /// <summary>
        /// Method for Country Dropdown
        /// </summary>
        public void BindCountryDropDown()
        {
            ddlCountry.Items.Clear();
            if (AdmCountryMstList != null && AdmCountryMstList.Count > 0)
            {
                ddlCountry.DataSource = AdmCountryMstList;
                ddlCountry.DataTextField = Resources.DataFieldRes.CountryName;
                ddlCountry.DataValueField = Resources.DataFieldRes.CountryPk;
                ddlCountry.DataBind();
            }
            ddlCountry.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        /// <summary>
        /// Method for State Dropdown
        /// </summary>
        public void BindStateDropDown()
        {
            //ddlState.Items.Clear();
            //if (AdmStateMstList != null && AdmStateMstList.Count > 0)
            //{
            //    ddlState.DataSource = AdmStateMstList;
            //    ddlState.DataTextField = Resources.DataFieldRes.StateName;
            //    ddlState.DataValueField = Resources.DataFieldRes.StatePk;
            //    ddlState.DataBind();
            //}
            //ddlState.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

        }
        /// <summary>
        /// Method for Account Type Dropdown
        /// </summary>
        public void BindAccountTypeDropDown()
        {
            ddlAccountType.Items.Clear();
            if (AdmConstMstList != null && AdmConstMstList.Count > 0)
            {
                ddlAccountType.DataSource = AdmConstMstList;
                ddlAccountType.DataTextField = Resources.DataFieldRes.ConstName;
                ddlAccountType.DataValueField = Resources.DataFieldRes.ConstPK;
                ddlAccountType.DataBind();
            }
            ddlAccountType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));


        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdCashBankMst.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt16(grdCashBankMst.DataKeys[grdrow.RowIndex].Values[0]);
                        // Get And Set the Location details
                        GetFieldValues(ControlsEnum.CASHBANK);
                        SetFieldValues(ControlsEnum.CASHBANK);
                        txtCode.Focus();
                        ModifiedDatePnl.Visible = true;
                        if (Mode == ActionsEnum.VIEW )
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            txtSearchBy.Text = string.Empty;
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            ddlStatus.SelectedValue = CommonConstants.SELECTVAL;
            //ddlType.SelectedValue = CommonConstants.SELECTVAL;
            ddlAccountType.SelectedValue = CommonConstants.SELECTVAL;
            ddlCountry.SelectedValue = CommonConstants.SELECTVAL;
            //ddlState.SelectedValue = CommonConstants.SELECTVAL;
            txtState.Text = string.Empty;
            txtBranch.Text = string.Empty;
            txtAccountNumber.Text = string.Empty;
            txtIFSCcode.Text = string.Empty;
            txtSWIFTCode.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtzip.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtMobile.Text = string.Empty;
            txtFax.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtDesc.Text = string.Empty;
            txtAccount.Text = string.Empty;
            hdfAccount.Value = string.Empty;
            PageIndex = CommonConstants.SELECT_VALUE_ONE;
            lblLastModifiedHDR.Text = string.Empty;
            ModifiedDatePnl.Visible = false;
        }
        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept("../../../login.aspx"))
                return;

            FinCashBankService FinCashBankServiceClient;
            FinCashBankServiceClient = null;
            try
            {
                int result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                switch (commonActions)
                {
                    #region Save
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            FinCashBankMstList = new List<FIN_CASH_BANK_MST>();
                            FinCashBankServiceClient = new FinCashBankService();
                            FinCashBankServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinCashBankServiceClient);
                            FinCashBankMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_CASH_BANK_MST>();
                            FinCashBankMstObj = SetUIValuesToObject();
                            FinCashBankMstList.Add(FinCashBankMstObj);
                            result = FinCashBankServiceClient.SaveFinCashBank(FinCashBankMstList);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                SortBy = Resources.DataFieldRes.BankPK;
                                SortDirection = Resources.Report.SortDescending;
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CashBank);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                btnNew.Focus();
                            }
                        }
                        break;
                    #endregion

                    #region New
                    case ActionsEnum.NEW:
                        BindStatusDropDown();
                        BindTypeDropDown();
                        GetFieldValues(ControlsEnum.COUNTRY);
                        SetFieldValues(ControlsEnum.COUNTRY);
                        GetFieldValues(ControlsEnum.ACCOUNTTYPE);
                        SetFieldValues(ControlsEnum.ACCOUNTTYPE);
                        ModifiedDatePnl.Visible = false;
                        this.txtCode.Focus();
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        btnSearch.Focus();
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Edit
                    case ActionsEnum.EDIT:
                        BindStatusDropDown();
                        BindTypeDropDown();
                        GetFieldValues(ControlsEnum.COUNTRY);
                        SetFieldValues(ControlsEnum.COUNTRY);
                        GetFieldValues(ControlsEnum.ACCOUNTTYPE);
                        SetFieldValues(ControlsEnum.ACCOUNTTYPE);
                        SetUIEditView(commonActions);
                        break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        FinCashBankServiceClient = new FinCashBankService();
                        FinCashBankServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(FinCashBankServiceClient);
                        FinCashBankMstList = new List<FIN_CASH_BANK_MST>();
                        FinCashBankMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_CASH_BANK_MST>();
                        FinCashBankMstObj.CBM_PK = (short)CurrPK;
                        FinCashBankMstObj.CBM_MOD_DT = LastModifiedTime;
                        FinCashBankMstList.Add(FinCashBankMstObj);

                        result = FinCashBankServiceClient.DeleteFinCashBank(FinCashBankMstList);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            ResetForm();
                            btnNew.Focus();
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CashBank);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        //delete reference error
                        else if (result == -1)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("DeleteError").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.VIEW:
                        BindStatusDropDown();
                        BindTypeDropDown();
                        GetFieldValues(ControlsEnum.COUNTRY);
                        SetFieldValues(ControlsEnum.COUNTRY);
                        GetFieldValues(ControlsEnum.ACCOUNTTYPE);
                        SetFieldValues(ControlsEnum.ACCOUNTTYPE);
                        SetUIEditView(commonActions);
                        btnCancel.Focus();
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.ACTIVATE:
                       BindStatusDropDown();
                        BindTypeDropDown();
                        GetFieldValues(ControlsEnum.COUNTRY);
                        SetFieldValues(ControlsEnum.COUNTRY);
                        GetFieldValues(ControlsEnum.ACCOUNTTYPE);
                        SetFieldValues(ControlsEnum.ACCOUNTTYPE);
                        SetUIEditView(commonActions);
                        break;
                    #endregion

                    #region SelectIndexChanged
                    //Drop down select change
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        //if (Convert.ToInt32(ddlCountry.SelectedValue) > 0)
                        //{
                        //    GetFieldValues(ControlsEnum.STATE);
                        //    SetFieldValues(ControlsEnum.STATE);
                        //}
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("CashBankDuplicate").ToString()))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.Code)) + "','" + Resources.Messages.Information + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                FinCashBankMstObj = null;
                FinCashBankMstList = null;
                FinCashBankServiceClient = null;
            }
        }

        /// <summary>
        /// Row data bound Event Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCashBankActive = e.Row.FindControl("lblCashBankActive") as Label;
                lblCashBankActive.Text = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.BanKStatus).ToString() == ((int)RecordStatus.ACTIVE).ToString() ? Resources.Controls.Active : Resources.Controls.InActive;
                lblCashBankActive.ToolTip = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.BanKStatus).ToString() == ((int)RecordStatus.ACTIVE).ToString() ? Resources.Controls.Active : Resources.Controls.InActive;
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (SortBy == e.SortExpression)
                {
                    if (SortDirection == Resources.Report.SortAscending)
                        SortDirection = Resources.Report.SortDescending;
                    else
                        SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.Report.SortAscending;
                }
                this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //////base.CheckBtnVisibility(sender);
        }

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link?
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link?
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link?
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
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
            string breadCrumb;
            breadCrumb = this.GetLocalResourceObject("BreadcrumbCashBankCreation").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
                breadCrumb = this.GetLocalResourceObject("BreadcrumbCashBankList").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            }
            lblBreadCrum.Text = breadCrumb;
        }
        #endregion


        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            CASHBANK,
            COUNTRY,
            STATE,
            ACCOUNTTYPE
        }
        #endregion
    }
}