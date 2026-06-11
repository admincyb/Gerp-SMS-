using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPData;
using ERPService;
using ERPManager;
using BusinessObject.CommonManagement;
using BusinessObject;
using System.Data;

namespace ERPSMS_v01.Journalize
{
    public partial class VoucherSearch : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables
        private ActionsEnum commonActions;
        User currentUser;
        private ADM_APP_TYPE_MST admAppTypeMstObj;
        private List<ADM_APP_TYPE_MST> admAppTypeMstList;
        private ServiceUtility serviceUtilityObj;
        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private List<FIN_YEAR_MST> finYearMstList;
        private DataTable dtvoucherdetails;     
        #endregion
        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();

        }
        #endregion
        #region InitializeComponent
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
        #endregion
        #region Page_Init
        protected void Page_Init(object sender, System.EventArgs e)
        { }
        #endregion
        #region Page_PreRender
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);

        }
        #endregion
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion


        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.FINPERIOD);
                    SetFieldValues(ControlsEnum.FINPERIOD);
                    //GetFieldValues(ControlsEnum.VOUCHERTYPE);
                    //SetFieldValues(ControlsEnum.VOUCHERTYPE);
                    ddlStatus.SelectedValue = "2";
                    SetFieldValues(ControlsEnum.VOUCHERDETAILS);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        public void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Journalization Types
                    case ControlsEnum.VOUCHERTYPE:

                        ddlVoucherType.Items.Clear();
                        if (admAppTypeMstList != null && admAppTypeMstList.Count > 0)
                        {
                            ddlVoucherType.DataSource = admAppTypeMstList;
                            ddlVoucherType.DataTextField = Resources.DataFieldRes.AptName;
                            ddlVoucherType.DataValueField = Resources.DataFieldRes.AptCode;
                            ddlVoucherType.DataBind();
                        }
                        ddlVoucherType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    #endregion
                    #region Voucher no
                    case ControlsEnum.VOUCHERDETAILS:

                        ddlFrom.Items.Clear();
                        ddlTo.Items.Clear();
                        if (dtvoucherdetails != null && dtvoucherdetails.Rows.Count > 0)
                        {
                            ddlFrom.DataSource = dtvoucherdetails;
                            ddlFrom.DataTextField = Resources.DataFieldRes.VoucherNo;
                            ddlFrom.DataValueField = Resources.DataFieldRes.JournalizePK;
                            ddlFrom.DataBind();
                        }
                        else
                        {
                            ddlFrom.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        }
                        if (dtvoucherdetails != null && dtvoucherdetails.Rows.Count > 0)
                        {
                            ddlTo.DataSource = dtvoucherdetails;
                            ddlTo.DataTextField = Resources.DataFieldRes.VoucherNo;
                            ddlTo.DataValueField = Resources.DataFieldRes.JournalizePK;
                            ddlTo.DataBind();
                        }
                        else
                        {
                            ddlTo.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
    
        #region Get Field Values
        private void GetFieldValues(ControlsEnum type)
        {
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;
            CommonService commonServiceClient;
            commonServiceClient = null;
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region VOUCHER DETAILS
                    case ControlsEnum.VOUCHERDETAILS:                        
                      
                        dtvoucherdetails = BusinessLogic.Jouralize.JournalizeBL.GetVoucherDetails(ddlVoucherType.SelectedValue.ToString(), Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), Convert.ToInt32(ddlStatus.SelectedValue), currentUser.SBUID);
                        break;
                    #endregion
                    #region FIN HEADER
                    case ControlsEnum.FINHEADER:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.SearchBy = Resources.DataFieldRes.VoucherNo;
                        serviceUtilityObj.SearchValue = txtVoucherNo.Text.Trim();
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_PK = 0;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrObj.FTH_CRTD_BY = currentUser.PKUser;
                        finTrxHdrObj.FTH_BIZUNIT = currentUser.SBUID;
                        finTrxHdrObj.FTH_IS_JRNLD = false;
                        finTrxHdrObj.FTH_IS_DELETED = false;
                        serviceUtilityObj.NeedAdvanceFilter = true;
                        finTrxHdrObj.FTH_IS_DELETED = false;
                        finTrxHdrList = finTrxServiceClient.GetVoucherSearchList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    #endregion
                    case ControlsEnum.VOUCHERTYPE:
                        commonServiceClient = new CommonService();
                        commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        admAppTypeMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_TYPE_MST>();
                        admAppTypeMstObj.APT_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppTypeMstObj.APT_SPL_COND = "J,LST_VCH";
                        admAppTypeMstList = commonServiceClient.GetAppTypeValues(admAppTypeMstObj);
                        break;

                    case ControlsEnum.FINPERIOD:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                        finYearMstList = finTrxServiceClient.GetCurrentFinPeriod(DateTime.Now, currentUser.SBUID);
                        break;
                 
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
        #endregion

        #region Set Field Values

        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Fin Header
                    case ControlsEnum.FINHEADER:
                        if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "VoucherSearch", "window.open('" + "../PageNavigator.aspx?" + "JrnlType=" + finTrxHdrList[0].FTH_REF_TYPE + "&JrnlPK=" + finTrxHdrList[0].FTH_PK + "','_blank')", true);
                            // Response.Redirect("~/PageNavigator.aspx?" + "JrnlType=" + finTrxHdrList[0].FTH_REF_TYPE + "&JrnlPK=" + finTrxHdrList[0].FTH_PK, true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Messages.Msg_EmptyGrid) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    case ControlsEnum.VOUCHERTYPE:
                        BindDropDown(ControlsEnum.VOUCHERTYPE);
                        break;
                    case ControlsEnum.FINPERIOD:
                        if (finYearMstList != null && finYearMstList.Count > 0)
                        {                         
                                txtFromDate.Text = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                                hdfFromDate.Value = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                                txtToDate.Text = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                                hdfToDate.Value = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                        }
                        break;
                    case ControlsEnum.VOUCHERDETAILS:
                        BindDropDown(ControlsEnum.VOUCHERDETAILS);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {

                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;

                }
                switch (commonActions)
                {
                    #region SEARCH
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                    case ActionsEnum.SEARCHVOUCHERDETAILS:
                        GetFieldValues(ControlsEnum.VOUCHERDETAILS);
                        SetFieldValues(ControlsEnum.VOUCHERDETAILS);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        break;
                    #endregion

                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        txtVoucherNo.Text = string.Empty;
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        if (ddlVoucherType.SelectedValue == ApplicationType.JV)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?FTH_REF_TYPE=" + ddlVoucherType.SelectedValue + "&VOUCHER_FROM=" + ddlFrom.SelectedItem.Text + "&VOUCHER_TO=" + ddlTo.SelectedItem.Text + "&V_STATUS=" + ddlStatus.SelectedValue + "&APPTYPE=" + ApplicationType.VTYPE + "&APPSUBTYPE=0") + "');", true);

                        }

                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?FTH_REF_TYPE=" + ddlVoucherType.SelectedValue + "&VOUCHER_FROM=" + ddlFrom.SelectedItem.Text + "&VOUCHER_TO=" + ddlTo.SelectedItem.Text + "&V_STATUS=" + ddlStatus.SelectedValue + "&APPTYPE=" + ApplicationType.VTYPE+ddlVoucherType.SelectedValue + "&APPSUBTYPE=0") + "');", true);
                        }
                      

                        //Response.Redirect(Resources.PageURL.ReportUrl + "?FTH_REF_TYPE=" + ddlVoucherType.SelectedValue + "&VOUCHER_FROM=" + ddlFrom.SelectedItem.Text + "&VOUCHER_TO=" + ddlTo.SelectedItem.Text + "&APPTYPE=" + ApplicationType.VTYPE + "&APPSUBTYPE=0");


                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Controls Enum
        public enum ControlsEnum
        {
            FINHEADER,
            VOUCHERTYPE,
            VOUCHERNO,
            FINPERIOD,
            VOUCHERDETAILS
        }
        #endregion        
    }
}