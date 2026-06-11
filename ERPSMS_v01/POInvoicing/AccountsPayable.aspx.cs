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
using BusinessObject.CommonManagement;
using System.Linq;
using System.Data;
using BusinessObject;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.POInvoicing
{
    public partial class AccountsPayable : ERP.Store.UI.MyBasePageUserRight
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
                return this.ViewState[ViewstateStrings.PageIndex]==null? "1" : (string)this.ViewState[ViewstateStrings.PageIndex];
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
                return this.ViewState[ViewstateStrings.TotalPages] == null ? 0 : (int)this.ViewState[ViewstateStrings.TotalPages];
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
        #endregion

        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        private ADM_APP_TYPE_MST admAppTypeMstObj;
        private List<ADM_APP_TYPE_MST> admAppTypeMstList;
        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private List<FIN_TRX> finTrxList;
        private FIN_TRX finTrxObj;
        private decimal drTotal;
        private decimal crTotal;
        private decimal balanceTotal;
        private List<FIN_YEAR_MST> finYearMstList;

        //List for binding details to controls      

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

                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();

                    Session[ERP.Utilities.SessionStrings.SelectedPos] = null;
                    //GetFieldValues(ControlsEnum.FINPERIOD);
                    //SetFieldValues(ControlsEnum.FINPERIOD);

                    if (Session[ERP.Utilities.SessionStrings.VendorPK] != null && Session[ERP.Utilities.SessionStrings.Vendor] != null)
                    {
                        hdfVendor.Value = Session[ERP.Utilities.SessionStrings.VendorPK].ToString();
                        txtVendor.Text = Session[ERP.Utilities.SessionStrings.Vendor].ToString();

                        if (hdfVendor.Value != "" && hdfVendor.Value != "0")
                        {
                            GetFieldValues(ControlsEnum.DEFAULT);
                            GetFieldValues(ControlsEnum.JOURNALIZATIONTYPE);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            Session[ERP.Utilities.SessionStrings.VendorPK] = null;
                            Session[ERP.Utilities.SessionStrings.Vendor] = null;
                        }
                        else
                        {
                            grdAccountPayables.DataSource = null;
                            grdAccountPayables.DataBind();
                        }
                    }
                    ConfigurationSettings();
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    //uclPaging.TotalPages = TotalPages;
                    //uclPaging.CurrentPage = 1;
                    EntryStatus = EntryStatus.LISTMODE;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            }
        }
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
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            FinTrxService finTrxServiceClient;
            CommonService commonServiceClient;
            commonServiceClient = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {
                switch (type)
                {
                    #region Fin Trx List
                    case ControlsEnum.DEFAULT:                    
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        finTrxObj = CommonFunctions.Initilize<ERPData.FIN_TRX>();
                        serviceUtilityObj = new ServiceUtility();
                        //serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        //serviceUtilityObj.PageSize =grdAccountPayables.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.FinTrxDate : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        if (string.IsNullOrEmpty(txtFromDate.Text.Trim()))
                            serviceUtilityObj.FilterDate = DateTime.Now;
                        else
                            serviceUtilityObj.FilterDate = Convert.ToDateTime(txtFromDate.Text.Trim());
                        if (string.IsNullOrEmpty(txtToDate.Text.Trim()))
                            serviceUtilityObj.FilterToDate = DateTime.Now;
                        else
                            serviceUtilityObj.FilterToDate = Convert.ToDateTime(txtToDate.Text.Trim());
                        int VendorId=hdfVendor.Value ==""?0:Convert.ToInt32(hdfVendor.Value);
                        finTrxList = finTrxServiceClient.GetAccoutPayablesList(VendorId, ApplicationType.AP,serviceUtilityObj);
                        
                        TotalPages = serviceUtilityObj.TotalRecords > 0 ? serviceUtilityObj.TotalRecords : 0;
                        break;
                    #endregion
                    #region JOURNALIZATION TYPE
                    case ControlsEnum.JOURNALIZATIONTYPE:
                        commonServiceClient = new CommonService();
                        commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        admAppTypeMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_TYPE_MST>();
                        admAppTypeMstObj.APT_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppTypeMstObj.APT_SPL_COND = "J";
                        admAppTypeMstList = commonServiceClient.GetAppTypeValues(admAppTypeMstObj);
                        break;
                    #endregion  
                    #region Base Currency
                    case ControlsEnum.BASECURRENCY:
                        CurrencyMstService CurrencyMstServiceClient = new CurrencyMstService();
                        string BaseCurrency = CurrencyMstServiceClient.GetCurrencyCodeName(currentUser.BaseCurrency);
                        hdfBaseCurrency.Value = BaseCurrency;
                        break;
                    #endregion
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                        finYearMstList = finTrxServiceClient.GetCurrentFinPeriod(DateTime.Now, currentUser.SBUID);
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
                finTrxServiceClient=null;
                commonServiceClient=null;
                commonServiceClient = null;

               
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
                    #region Bind Grid
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        break;
                    #endregion
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        if (finYearMstList != null && finYearMstList.Count > 0)
                        {
                            txtFromDate.Text = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                            hdfFromDate.Value = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                            txtToDate.Text = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                            hdfToDate.Value = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                        }
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
        #region Action Handlers

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
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
            switch (commonActions)
            {
                #region Search
                case ActionsEnum.SEARCH:
                    if (hdfVendor.Value != "" && hdfVendor.Value != "0")
                    {
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.DEFAULT);
                        GetFieldValues(ControlsEnum.JOURNALIZATIONTYPE);
                        SetFieldValues(ControlsEnum.DEFAULT);
                    }
                    else
                    {
                        grdAccountPayables.DataSource = null;
                        grdAccountPayables.DataBind();
                    }
                    break;
                #endregion
                #region Clear
                case ActionsEnum.CLEAR:
                    ResetForm();
                    if (hdfVendor.Value != "" && hdfVendor.Value != "0")
                    {
                        GetFieldValues(ControlsEnum.DEFAULT);
                        GetFieldValues(ControlsEnum.JOURNALIZATIONTYPE);
                        SetFieldValues(ControlsEnum.DEFAULT);
                    }
                    else
                    {
                        grdAccountPayables.DataSource = null;
                        grdAccountPayables.DataBind();
                    }
                    break;
                #endregion
                #region Tabs
                case ActionsEnum.POINVOICE:
                    CheckUserRightsAndRedirect(Resources.PageURL.PurchaseOrderInvoicing);
                    //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PurchaseOrderInvoicing), false);
                    break;
                case ActionsEnum.INVOICE:
                    CheckUserRightsAndRedirect(Resources.PageURL.PoInvoicing);
                    //Response.Redirect(Resources.PageURL.PoInvoicing);
                    break;
                case ActionsEnum.EXPENSES:
                    CheckUserRightsAndRedirect(Resources.PageURL.ExpenseInvoice);
                    //Response.Redirect(Resources.PageURL.ExpenseInvoice);
                    break;
                case ActionsEnum.PAYMENT:
                    CheckUserRightsAndRedirect(Resources.PageURL.PoPayment);
                    //Response.Redirect(Resources.PageURL.PoPayment);
                    break;
                case ActionsEnum.DEFAULT:
                    CheckUserRightsAndRedirect(Resources.PageURL.PoListing);
                    //Response.Redirect(Resources.PageURL.PoListing);
                    break;
                case ActionsEnum.CRDRNOTE:
                    Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.PI;
                    CheckUserRightsAndRedirect(Resources.PageURL.DrCrNote);
                    //Response.Redirect(Resources.PageURL.DrCrNote);
                    break;
                case ActionsEnum.ACPAYABLES:
                    CheckUserRightsAndRedirect(Resources.PageURL.AccountsPayable);
                    //Response.Redirect(Resources.PageURL.AccountsPayable);
                    break;
                #endregion  


            }
        }


        #endregion

        #region --- For Grid Actions----

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            decimal drAmount=0;
            decimal crAmount=0;
            decimal balanceAmount = 0;

            try
            { 
                #region Grid Fixed Columns
                if ((sender as GridView).ID == "grdAccountPayables")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        if (finTrxList != null && finTrxList.Count > 0)
                        {
                            Label lbltrx = (Label)e.Row.FindControl("lbltrx");
                            admAppTypeMstObj = admAppTypeMstList.SingleOrDefault(app => app.APT_CODE == lbltrx.Text);
                            if (admAppTypeMstObj != null)
                            {
                                string VoucherDetails = string.Empty;
                                if (!String.IsNullOrEmpty(finTrxList[e.Row.RowIndex].FIN_TRX_HDR.FTH_VOUCHER_NO))
                                {
                                    VoucherDetails = " ( " + finTrxList[e.Row.RowIndex].FIN_TRX_HDR.FTH_VOUCHER_NO + "  " + (finTrxList[e.Row.RowIndex].FIN_TRX_HDR.FTH_DATE != null ? (finTrxList[e.Row.RowIndex].FIN_TRX_HDR.FTH_DATE.ToString() != string.Empty ? Convert.ToDateTime(finTrxList[e.Row.RowIndex].FIN_TRX_HDR.FTH_DATE).ToString(Resources.Constants.DateFormatShort) : string.Empty) : string.Empty) + " )";
                                }
                                lbltrx.Text = ERP.Utilities.CommonFunctions.GetShortString(admAppTypeMstObj.APT_NAME.ToString() + VoucherDetails, 55);
                                lbltrx.ToolTip = admAppTypeMstObj.APT_NAME.ToString() + VoucherDetails;

                            }
                        }

                        decimal.TryParse(((Label)e.Row.FindControl("lblDr")).Text.Replace(",",""),out drAmount);
                        decimal.TryParse(((Label)e.Row.FindControl("lblCr")).Text.Replace(",", ""), out crAmount);
                        if (e.Row.RowIndex > 0)
                        {
                            double balance = Convert.ToDouble(hdfRunningBal.Value) + Convert.ToDouble(drAmount) - Convert.ToDouble(crAmount);
                            hdfRunningBal.Value = balance.ToString();

                            //decimal.TryParse(((Label)e.Row.FindControl("lblBalance")).Text.Replace(",", ""), out balanceAmount);
                            decimal.TryParse(hdfRunningBal.Value, out balanceAmount);
                            drTotal += drAmount;
                            crTotal += crAmount;
                        }
                        else
                        {
                            balanceAmount = ((balanceAmount + drAmount) - crAmount);
                        }
                        //balanceAmount=((balanceAmount + drAmount) - crAmount);
                        //balanceTotal += balanceAmount;
                        if (balanceAmount <= 0)
                        {
                            ((Label)e.Row.FindControl("lblBalance")).Text =String.Format("{0:c}", (0-balanceAmount)) +" Cr";
                            ((Label)e.Row.FindControl("lblBalance")).ToolTip = String.Format("{0:c}", (0 - balanceAmount)) + " Cr";
                        }
                        else if (balanceAmount >= 0)
                        {
                            ((Label)e.Row.FindControl("lblBalance")).Text =String.Format("{0:c}", balanceAmount) + " Dr";
                            ((Label)e.Row.FindControl("lblBalance")).ToolTip = String.Format("{0:c}", balanceAmount) + " Dr";
                        }
                        //For TRXAmt
                        HiddenField hdfDebitTC = (HiddenField)e.Row.FindControl("hdfDebitTC");
                        HiddenField hdfCreditTC = (HiddenField)e.Row.FindControl("hdfCreditTC");
                        HiddenField hdfCurrText = (HiddenField)e.Row.FindControl("hdfCurrText");
                        Label lblTRXAmt = (Label)e.Row.FindControl("lblTRXAmt");

                        double DEBIT_TC = 0;
                        double CREDIT_TC = 0;

                        double.TryParse(Convert.ToString(hdfDebitTC.Value), out DEBIT_TC);
                        double.TryParse(Convert.ToString(hdfCreditTC.Value), out CREDIT_TC);

                        lblTRXAmt.Text = lblTRXAmt.ToolTip = hdfCurrText.Value + " " + (String.Format("{0:c}", DEBIT_TC + CREDIT_TC)).ToString();
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        decimal.TryParse(hdfRunningBal.Value, out balanceTotal);

                        ((Label)e.Row.FindControl("lblCrTotal")).Text =String.Format("{0:c}", crTotal);
                        ((Label)e.Row.FindControl("lblCrTotal")).ToolTip = String.Format("{0:c}", crTotal);
                        ((Label)e.Row.FindControl("lblDrTotal")).Text =String.Format("{0:c}", drTotal);
                        ((Label)e.Row.FindControl("lblDrTotal")).ToolTip = String.Format("{0:c}", drTotal);
                        if (balanceTotal <= 0)
                        {
                            ((Label)e.Row.FindControl("lblBalanceTotal")).Text = String.Format("{0:c}",(0 - balanceTotal)) + " Cr";
                            ((Label)e.Row.FindControl("lblBalanceTotal")).ToolTip = String.Format("{0:c}", (0 - balanceTotal)) + " Cr";
                        }
                        else if (balanceTotal >= 0)
                        {
                            ((Label)e.Row.FindControl("lblBalanceTotal")).Text = String.Format("{0:c}",balanceTotal) + " Dr";
                            ((Label)e.Row.FindControl("lblBalanceTotal")).ToolTip = String.Format("{0:c}", balanceTotal) + " Dr";
                        }

                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        Label lblHdrAmountBaseCur = e.Row.FindControl("LblCaption") as Label;
                        lblHdrAmountBaseCur.Text = GetLocalResourceObject("Total").ToString() + " (" + hdfBaseCurrency.Value.Split('-')[0].Trim() + ")";

                    }
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        e.Row.Cells[4].Text = GetLocalResourceObject("Dr").ToString() + " (" + hdfBaseCurrency.Value.Split('-')[0].Trim() + ")";
                        e.Row.Cells[5].Text = GetLocalResourceObject("Cr").ToString() + " (" + hdfBaseCurrency.Value.Split('-')[0].Trim() + ")";
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
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
                    //Toggle the sort expression
                    if (SortDirection == Resources.ErpRes.SortAscending)
                        SortDirection = Resources.ErpRes.SortDescending;
                    else
                        SortDirection = Resources.ErpRes.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.ErpRes.SortAscending;
                }

                this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            }
        }

        #endregion

        #endregion
        #region Helper Methods

        private void ConfigurationSettings() {
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "VENDOR");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUVendor.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }

        }
        /// <summary>
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        private void SetUIEditView(ActionsEnum mode)
        {
            try
            {
               
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
                if (finTrxList != null)
                {
                    //uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    //uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    if (finTrxList != null && finTrxList.Count > 0)
                    {
                        hdfOpeningBal.Value = (finTrxList[0].FTR_CR_AMT_BC == 0 ? finTrxList[0].FTR_DR_AMT_BC : -finTrxList[0].FTR_CR_AMT_BC).ToString();
                        hdfRunningBal.Value = hdfOpeningBal.Value;

                    }
                    grdAccountPayables.DataSource = finTrxList;
                    grdAccountPayables.DataBind();                   
                }
                //uclPaging.Visible = TotalPages > 0 ? true : false;
                //uclPaging.BindPager();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method for Dropdownlist binding
        /// </summary>
        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
               

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
            txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString();
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();
            //GetFieldValues(ControlsEnum.FINPERIOD);
            //SetFieldValues(ControlsEnum.FINPERIOD);
            hdfVendor.Value = "0";
            txtVendor.Text = string.Empty;
            grdAccountPayables.DataSource = null;
            grdAccountPayables.DataBind();


        }

        /// <summary>
        /// Redirect to a page if user has permission on that page.
        /// </summary>
        /// <param name="RedirectUrl">Page Url</param>
        private void CheckUserRightsAndRedirect(string RedirectUrl)
        {
            CommonBL userAuth = new CommonBL();
            if(currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            UserRightsBO UsrRights = userAuth.GetUserPageRights(currentUser.PKUser, RedirectUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK);
            if (UsrRights != null && UsrRights.Rights.Count() > 0 && UsrRights.Rights[0].UserDeptRight)
                Response.Redirect(RedirectUrl, false);
            else
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErpRes.NoPressionMsg) + "','" + Resources.ErpRes.Information + "');", true);
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
            //uclPaging.CurrentPage = 1;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            //this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            //try
            //{
            //    switch (e.Action)
            //    {
            //        case NavigationEnum.PAGECHANGE:
            //            uclPaging.CurrentPage = e.CurrentPage;
            //            break;
            //        case NavigationEnum.FIRST:
            //            // Assignment the first page index.
            //            if (e.CurrentPage > 1)
            //                uclPaging.CurrentPage = 1;
            //            break;
            //        case NavigationEnum.LAST:
            //            // Assignment the last page index.
            //            if (e.CurrentPage <= e.TotalPages)
            //                uclPaging.CurrentPage = e.TotalPages;
            //            break;
            //        case NavigationEnum.NEXT:
            //            // Increment the next page index.
            //            if (e.CurrentPage <= e.TotalPages)
            //                uclPaging.CurrentPage++;
            //            break;
            //        case NavigationEnum.PREVIOUS:
            //            // Decrement the previous page index.
            //            if (e.CurrentPage > 1)
            //                uclPaging.CurrentPage--;
            //            break;
            //    }
            //    PageIndex = uclPaging.CurrentPage.ToString();
            //    GetFieldValues(ControlsEnum.DEFAULT);
            //    SetFieldValues(ControlsEnum.DEFAULT);
            //    EnableDisableButtons(e.TotalPages);
            //    EntryStatus = EntryStatus.LISTMODE;
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            //}
        }
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link
            //uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we disable the previous link
            //uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we enable the next link
            //uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            //// Should we enable the last link
            //uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
            try
            {                

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            }
        }

        #endregion

        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            AIRCRAFTTYPE,
            ACTIVITY,
            ACTIVITYBYPK,
            ACTIVITYTASKSTATUS,
            JOURNALIZATIONTYPE,
            BASECURRENCY,
            FINPERIOD

        }

        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum StatusEnum
        {
            DRAFTED = 0,
            SUBMITTED = 1,
            UNATTENDED = 6
        }

        /// <summary>
        /// Define Flight Status Enum
        /// </summary>
        public enum FlightStatusEnum
        {
            Scheduled = 0,
            Arrived = 1,
            Delayed = 2,
            Diverted = 3,
            Cancelled = 4
        }
        #endregion
    }
}