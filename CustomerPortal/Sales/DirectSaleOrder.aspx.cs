using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.Sales;
using ERP.Utilities;
using ERPData;
using ERPService;
using ERPManager;
using System.IO;
using System.Configuration;
using BusinessLogic.CommonManagement;
using ERPSMS_v01.Reports;
using BusinessObject.SaleOrder;
using ERPSMS_v01.UserControls;
using CustomControls;
using BusinessLogic.AccountManagement;
using BusinessObject;


namespace CustomerPortal.Sales
{
    public partial class DirectSaleOrder : ERP.Store.UI.WorkFlowBasePage //System.Web.UI.Page
    {
        #region Variables & Properties

        #region Properties

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

        private int ReferanceID
        {
            get
            {
                return this.ViewState["ReferanceID"] == null ? 0 : Convert.ToInt32(this.ViewState["ReferanceID"].ToString());
            }
            set
            {
                this.ViewState["ReferanceID"] = value;
            }
        }

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

        private int ProcessID
        {
            get
            {
                return this.ViewState["ProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["ProcessID"].ToString());
            }
            set
            {
                this.ViewState["ProcessID"] = value;
            }
        }

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

        private int IsCopySO
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.IsCopySO]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsCopySO] = value;
            }
        }

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

        private bool IsTaxInSBU
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsTaxInSBU] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsTaxInSBU].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsTaxInSBU] = value;
            }
        }

        private bool IsTaxForOtherCharge
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsTaxForOtherCharge] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsTaxForOtherCharge].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsTaxForOtherCharge] = value;
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
        /// To maintain selected Item Detail PK
        /// </summary>
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

        /// <summary>
        /// To maintain selected SC Detail PK
        /// </summary>
        private int SelectedDtlPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedDtlPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedDtlPK] = value;
            }
        }

        /// <summary>
        /// To maintain the selected Brand PK
        /// for Detail Tax
        /// </summary>
        private int SelectedItemCategory
        {
            get
            {
                return Convert.ToInt32(this.ViewState["SelectedItemCategory"]);
            }
            set
            {
                this.ViewState["SelectedItemCategory"] = value;
            }
        }

        /// <summary>
        /// To set custom tax config value
        /// </summary>
        private bool IsCustomTaxEnabled
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsCustomTaxEnabled] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsCustomTaxEnabled]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsCustomTaxEnabled] = value;
            }
        }

        /// <summary>
        /// Whether the Tax is in edit mode
        /// </summary>
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

        /// <summary>
        /// Serail No. of the Contract Item
        /// </summary>
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
        /// Is Same brand
        /// </summary>
        private bool IsItem
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsItem] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsItem]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsItem] = value;
            }
        }

        private int CurrDocSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrDocSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrDocSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrDocSlNo] = value;
            }
        }

        private string PageIndexList
        {
            get
            {
                return (string)this.ViewState["PageIndexList"];
            }
            set
            {
                this.ViewState["PageIndexList"] = value;
            }
        }

        /// <summary>
        /// To Disable Item Tax
        /// </summary>
        private bool EnableItemTax
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisableItemTax] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.DisableItemTax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisableItemTax] = value;
            }
        }

        /// <summary>
        /// To Disable Item Discount
        /// </summary>
        private bool EnableItemDiscount
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisableItemDiscount] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.DisableItemDiscount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisableItemDiscount] = value;
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

        private int ConfigRateGet
        {
            get
            {
                return this.ViewState[ViewstateStrings.GetConfigrate] == null ? 0 : (int)this.ViewState[ViewstateStrings.GetConfigrate];
            }
            set
            {
                this.ViewState[ViewstateStrings.GetConfigrate] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        /// <summary>
        /// Sale Order ID
        /// </summary>
        private int SoId
        {
            get
            {
                return this.ViewState[ViewstateStrings.SoId] == null ? 0 : (int)this.ViewState[ViewstateStrings.SoId];
            }
            set
            {
                this.ViewState[ViewstateStrings.SoId] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int CurRecordDept
        {
            get
            {
                return this.ViewState["CurRecordDept"] == null ? 0 : (int)this.ViewState["CurRecordDept"];
            }
            set
            {
                this.ViewState["CurRecordDept"] = value;
            }
        }

        private DirectSaleOrderBO DirectSaleOrderHeaderSession
        {
            get
            {
                return (DirectSaleOrderBO)ViewState[ERP.Utilities.ViewstateStrings.DirectSaleOrderHeaderSession];
            }
            set
            {
                if (ViewState[ERP.Utilities.ViewstateStrings.DirectSaleOrderHeaderSession] != null)
                    ViewState.Remove(ERP.Utilities.ViewstateStrings.DirectSaleOrderHeaderSession);
                ViewState.Add(ERP.Utilities.ViewstateStrings.DirectSaleOrderHeaderSession, value);
            }
        }

        private DirectSaleOrderDetailsBO DirectSaleOrderDetailSession
        {
            get
            {
                return (DirectSaleOrderDetailsBO)ViewState[ERP.Utilities.ViewstateStrings.DirectSaleOrderDetailSession];
            }
            set
            {
                if (ViewState[ERP.Utilities.ViewstateStrings.DirectSaleOrderDetailSession] != null)
                    ViewState.Remove(ERP.Utilities.ViewstateStrings.DirectSaleOrderDetailSession);
                ViewState.Add(ERP.Utilities.ViewstateStrings.DirectSaleOrderDetailSession, value);
            }
        }


        /// <summary>
        /// To maintain keep Sale Order Tax Splitting
        /// </summary>
        private DirectSaleOrderBO DirectTempSaleOrderHeaderSession
        {
            get
            {
                return (DirectSaleOrderBO)ViewState[ERP.Utilities.SessionStrings.DirectTempSaleOrderHeaderSession];
            }
            set
            {
                if (ViewState[ERP.Utilities.SessionStrings.DirectTempSaleOrderHeaderSession] != null)
                    ViewState.Remove(ERP.Utilities.SessionStrings.DirectTempSaleOrderHeaderSession);
                ViewState.Add(ERP.Utilities.SessionStrings.DirectTempSaleOrderHeaderSession, value);
            }
        }

        private List<DirectSaleOrderTaxHdr> tempDirectsaleOrderTaxHdrList
        {
            get
            {
                return (List<DirectSaleOrderTaxHdr>)Session[ERP.Utilities.ViewstateStrings.tempDirectsaleOrderTaxHdrList];
            }
            set
            {
                Session[ERP.Utilities.ViewstateStrings.tempDirectsaleOrderTaxHdrList] = value;
            }

        }

        /// <summary>
        /// To maintain keep Sale Order Tax Splitting
        /// </summary>
        private DirectSaleOrderBO EditDirectTempSaleOrderHeaderSession
        {
            get
            {
                return (DirectSaleOrderBO)ViewState[ERP.Utilities.SessionStrings.EditDirectTempSaleOrderHeaderSession];
            }
            set
            {
                if (ViewState[ERP.Utilities.SessionStrings.EditDirectTempSaleOrderHeaderSession] != null)
                    ViewState.Remove(ERP.Utilities.SessionStrings.EditDirectTempSaleOrderHeaderSession);
                ViewState.Add(ERP.Utilities.SessionStrings.EditDirectTempSaleOrderHeaderSession, value);
            }
        }

        private List<DirectSaleOrderTaxHdr> tempsaleOrderTaxHdrList
        {
            get
            {
                return (List<DirectSaleOrderTaxHdr>)Session[ERP.Utilities.ViewstateStrings.tempsaleOrderTaxHdrList];
            }
            set
            {
                Session[ERP.Utilities.ViewstateStrings.tempsaleOrderTaxHdrList] = value;
            }

        }

        private List<BusinessObject.SaleOrder.DirectFileDetails> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FileSODetailsList] == null ? null : (List<BusinessObject.SaleOrder.DirectFileDetails>)Session[ERP.Utilities.SessionStrings.FileSODetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FileSODetailsList] = value;
            }
        }

        private List<DirectSaleOrderUploads> SOUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.SOUploadList] == null ? null : (List<DirectSaleOrderUploads>)ViewState[ViewstateStrings.SOUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.SOUploadList] = value;
            }
        }

        /// <summary>
        /// To keep config value for Enable/Disable Header Discount in viewstate
        /// </summary>
        private bool IsHeaderDiscountForTradingSale
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingSale] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingSale].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderDiscountForTradingSale] = value;
            }
        }

        private ERP.Utilities.Constants.DA.CustomerSpecialCategory CustomerCategory
        {
            get { return this.ViewState[ViewstateStrings.CustomerCategory] == null ? ERP.Utilities.Constants.DA.CustomerSpecialCategory.Default : (ERP.Utilities.Constants.DA.CustomerSpecialCategory)this.ViewState[ViewstateStrings.CustomerCategory]; }
            set { this.ViewState[ViewstateStrings.CustomerCategory] = value; }
        }

        /// <summary>
        /// To keep config value for Enable/Disable Header Tax in viewstate
        /// </summary>
        private bool IsHeaderTaxForTradingSale
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsHeaderTaxForTradingSale] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderTaxForTradingSale].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderTaxForTradingSale] = value;
            }
        }

        /// <summary>
        /// To keep config value for Enable/Disable Item Discount in viewstate
        /// </summary>
        private bool IsItemwiseDiscountForTradingSale
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingSale] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingSale].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsItemwiseDiscountForTradingSale] = value;
            }
        }

        /// <summary>
        /// To keep config value for Enable/Disable Item Tax in viewstate  
        /// </summary>
        private bool IsItemwiseTaxForTradingSale
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingSale] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingSale].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsItemwiseTaxForTradingSale] = value;
            }
        }
        /// <summary>
        /// Has Short Close Right
        /// </summary>
        private bool HasShortCloseRight
        {
            get
            {
                return this.ViewState["HasShortCloseRight"] == null ? false : Convert.ToBoolean(this.ViewState["HasShortCloseRight"]);
            }
            set
            {
                this.ViewState["HasShortCloseRight"] = value;
            }
        }

        #endregion

        #region Variables
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private BusinessObject.User currentUser;
        private string refID;
        private string inboxFlag;
        private int processPK;

        Label lblSubTotal;
        DataSet dsSaleOrderTaxDetails;
        DataTable dtSaleOrderTaxDetails;
        DataTable dtPageData;
        DataSet dsPageData;
        DataTable dtCompany = new DataTable();
        DataTable dtSubCategory;

        private int custPK;
        private int custItemPK;
        private bool IsItemBind;
        private double exchangeRate;
        private int grdSOHpk;

        private ServiceUtility serviceUtilityObj;
        private CommonService commonServiceObj;
        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private DirectSaleOrderBO DirectsaleOrderHeaderObj;

        private DirectSaleOrderDetailsBO DirectsaleOrderDetailsObj;
        List<DirectSaleOrderDetailsBO> DirectsaleOrderDetailsList;
        List<DirectSaleOrderTaxHdr> contractTaxHdrList;

        DirectSaleOrderDetailsBO orderDetails;
        DirectSaleOrderUploads soUploadObj;

        bool isCancelled = false;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations
            = {
                  (a1, a2) => a1 - a2,
                  (a1, a2) => a1 + a2,
                  (a1, a2) => a1 / a2,
                  (a1, a2) => a1 * a2,
                  (a1, a2) => Math.Pow(a1, a2)
              };

        #endregion

        #endregion

        #region Page Level Events

        /// <summary>
        /// Pge Prerender Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            //EntryStatus = EntryStatus.NEWMODE;
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){ViewMode(1);});", true);
            }
            else if (EntryStatus == EntryStatus.EDITMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){ViewMode(2);});", true);
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(0);});", true);
        }

        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnDelete.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
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
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        private void PageActionHandler()
        {
            string prefID;
            int referenceID;
            int processID;
            int appId;

            referenceID = 0;
            processID = 0;
            appId = 0;

            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;

                InitializeComponent();

                if (!IsPostBack)
                {
                    GetUserRights();
                    FillProcessID(0, 1, true);
                    processID = FillProcessID(0, 1);
                    ucrWrkf.ProcessID = processID;

                    if (ucrWrkf.ProcessID > 0)
                        hdfProcessID.Value = ucrWrkf.ProcessID.ToString();
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    EntryStatus = EntryStatus.ENTRYMODE;

                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "SOD_SL_NO";
                    grdOrderDetails.DataKeyNames = itemkeyarray;

                    string[] itemkeyarrayUpload;
                    itemkeyarrayUpload = new string[1];
                    itemkeyarrayUpload[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarrayUpload;

                    FormatNumber();
                    hdfDecimalVal.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    if (Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowSOCalcButton"))))
                    {
                        txtItemPchQty.Visible = true;
                        lblUom.Visible = false;
                        txtItemQuantity.CssClass = "input-xsmall-b numeric";
                        txtItemPchQty.CssClass = "input-xsmall-b numeric margnrgt77";
                    }
                    else
                    {
                        txtItemPchQty.Visible = false;
                        lblUom.Visible = true;
                        txtItemQuantity.CssClass = "input-small numeric";
                    }
                    ConfigurationSettings();

                    if (!string.IsNullOrEmpty(refID))
                    {
                        ReferanceID = int.Parse(refID);
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
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
                            CurrPK = appId;
                            base.WkfRefID = ucrWrkf.RefID = referenceID;
                        }
                    }
                    uclPaging.CurrentPage = 1;

                    FileDetailsList = null;
                    SOUploadList = null;

                    DirectSaleOrderHeaderSession = new DirectSaleOrderBO()
                    {
                        TaxHdrDtl = new List<DirectSaleOrderTaxHdr>(),
                        DirectsaleOrderDetails = new List<DirectSaleOrderDetailsBO>()
                    };
                    DirectTempSaleOrderHeaderSession = new DirectSaleOrderBO()
                    {
                        TaxHdrDtl = new List<DirectSaleOrderTaxHdr>(),
                        DirectsaleOrderDetails = new List<DirectSaleOrderDetailsBO>()
                    };

                    //fill DropDowns and assign default values
                    GetFieldValues(ControlsEnum.SOTYPE);
                    SetFieldValues(ControlsEnum.SOTYPE);

                    //fetch and fill Company
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

                    AST_DOC_MODE.Value = "0";
                    if (lblSaleOrderNo.Text.Trim().Equals(string.Empty) || lblSaleOrderNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                    {
                        AST_DOC_MODE.Value = GetDOCMODE();
                    }

                    if (CurrPK > 0)
                    {
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK); //uncomment on 13-July-2017
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        GetFieldValues(ControlsEnum.DETAILFOREDIT);
                        SetFieldValues(ControlsEnum.DETAILFOREDIT);
                        EntryStatus = EntryStatus.EDITMODE;
                    }
                    else
                    {
                        GetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                        SetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                        ResetForm(ControlsEnum.LISTING);
                        EntryStatus = EntryStatus.LISTMODE;
                    }
                    Session[ERP.Utilities.SessionStrings.RefID] = null;
                    Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    txtCustomer.Focus();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion

        #region ActionHandlers

        /// <summary>
        /// Action Handler for Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_v01.ERPSMS_2).ValidatePageDept())
                return;
            try
            {
                int? result;
                double totalAmt;
                double currentTotal;
                double taxAmt;
                bool isValidDiscAm = true;
                bool isValidDisc = true;
                bool isValidTax = true;
                bool bIsChecked = false;
                int retRefID = 0;
                string arg;
                int dept;

                GridViewRow gvr;
                GridView grd;
                GridViewRow gvrow;
                TextBox WrkfComments;
                DirectSaleOrderTaxHdr DirecttempSaleOrderTaxHdrObj = null;
                DirectSaleOrderTaxHdr DirectsaleOrderTaxHdrObj;
                List<DirectSaleOrderTaxHdr> DirectsaleOrderTaxHdrList;

                FileInfo tempFileInfoObj;
                string savePath = string.Empty;
                int selectedItemPK;
                int grdSaleOrderSearchListRowDeptId = 0;
                int selRecordStatus = 0;
                HiddenField hdfDept;
                HiddenField hdfDelStatus;
                HiddenField hdfWrkfStatus;
                bool isSelected = false;
                string sohPK;
                string address;

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlPopupTaxType")
                    {
                        commonActions = ActionsEnum.TAXTYPECHANGED;
                    }
                }
                if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    commonActions = ActionsEnum.CHECKEDCHANGED;
                }
                switch (commonActions)
                {
                    #region LIST
                    case ActionsEnum.LIST:
                        ResetForm(ControlsEnum.LISTING);
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                        SetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region SHOWDETAILS (RadioButton Selection)
                    case ActionsEnum.SHOWDETAILS:
                        gvr = ((RadioButton)sender).Parent.Parent as ExtGridViewRow;
                        SoId = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfDSOID")).Value);
                        hdfDept = gvr.FindControl("hdfDepartmentID") as HiddenField;
                        hdfDelStatus = gvr.FindControl("hdfDSODELStatus") as HiddenField;
                        hdfWrkfStatus = gvr.FindControl("hdfDSOStatus") as HiddenField;
                        hdfDelStatusCurrent.Value = (Convert.ToInt16(hdfDelStatus.Value)).ToString();
                        hdfWrkfStatusCurrent.Value = (Convert.ToInt16(hdfWrkfStatus.Value)).ToString();
                        List<string> wkfsatus = new List<string> { "1", "2", "7", "12", "20" };
                        if (Convert.ToInt16(hdfDelStatus.Value) == 1 || !(wkfsatus.Contains(hdfWrkfStatus.Value)))
                        {
                            btnEditforCancel.Visible = false;
                        }
                        else
                        {
                            btnEditforCancel.Visible = true;
                        }
                        if (Convert.ToInt16(hdfDelStatus.Value) == 1 || (Convert.ToInt16(hdfWrkfStatus.Value) != 2)) //2=>Verify&GenerateI.O
                        {
                            btnAmend.Visible = false;
                        }
                        else
                        {
                            btnAmend.Visible = true;
                        }
                        if (Convert.ToInt16(hdfDelStatus.Value) == 1 && (Convert.ToInt16(hdfWrkfStatus.Value) == 4)) //4=>Cancelled
                        {
                            btnEdit.Visible = false;
                        }
                        else
                        {
                            btnEdit.Visible = true;
                        }

                        if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                        {
                            if (CurRecordDept != dept)
                            {
                                CurRecordDept = dept;
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }
                        }

                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:

                        this.EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        ResetForm(ControlsEnum.DIRECTSALEORDER);
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        txtDtlRemark2.Text = string.Empty;

                        DirectSaleOrderHeaderSession = new DirectSaleOrderBO()
                        {
                            TaxHdrDtl = new List<DirectSaleOrderTaxHdr>(),
                            DirectsaleOrderDetails = new List<DirectSaleOrderDetailsBO>()
                        };

                        DirectTempSaleOrderHeaderSession = new DirectSaleOrderBO()
                        {
                            TaxHdrDtl = new List<DirectSaleOrderTaxHdr>(),
                            DirectsaleOrderDetails = new List<DirectSaleOrderDetailsBO>()
                        };

                        SOUploadList = new List<DirectSaleOrderUploads>();

                        IsItemBind = true;
                        BindGrid(ControlsEnum.SALEORDERDETAIL);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        SetFieldValues(ControlsEnum.SUBCATEGORY);
                        SetItemCategory();
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:

                        if (DirectSaleOrderHeaderSession.DirectsaleOrderDetails == null || DirectSaleOrderHeaderSession.DirectsaleOrderDetails.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            DirectsaleOrderHeaderObj = new DirectSaleOrderBO();
                            DirectsaleOrderHeaderObj = (DirectSaleOrderBO)SetUIValuesToObject(ControlsEnum.SALEORDERHEADER);

                            if (DirectsaleOrderHeaderObj != null && DirectsaleOrderHeaderObj.DirectsaleOrderDetails != null)
                            {
                                if (DirectsaleOrderHeaderObj.TaxHdrDtl != null && DirectsaleOrderHeaderObj.TaxHdrDtl.Count > 0)
                                {
                                    List<DirectSaleOrderTaxHdr> Templst = new List<DirectSaleOrderTaxHdr>();
                                    Templst = DirectsaleOrderHeaderObj.TaxHdrDtl.Where(x => Convert.ToDouble(x.SLT_TAX_AMT) == 0).ToList();
                                    foreach (DirectSaleOrderTaxHdr items in Templst)
                                    {
                                        DirectsaleOrderHeaderObj.TaxHdrDtl.Remove(items);
                                    }
                                }
                                DirectsaleOrderHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                string xmlDoc = CommonFunctions.XmlSerialize<DirectSaleOrderBO>(DirectsaleOrderHeaderObj);//CommonFunctions.ObjectTOXml(saleOrderHeaderObj);
                                // save Process Control inspection details
                                result = BusinessLogic.Sales.DirectSaleOrderBL.SaveDirectSaleOrderWkfDetails(xmlDoc, out retRefID);

                                if (result > 0)
                                {
                                    #region Attachment Details
                                    //Document Attach details
                                    if (SOUploadList != null && SOUploadList.Count > 0)
                                    {
                                        savePath = string.Empty;
                                        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                        {
                                            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                            if (!Directory.Exists(savePath))
                                                Directory.CreateDirectory(savePath);
                                            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                                        }
                                        else
                                        {
                                            savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                                        }

                                        foreach (DirectSaleOrderUploads obj in SOUploadList)
                                        {
                                            string filePath = savePath + obj.AttachmentFileName;
                                            FileInfo attachedFileInfo = new FileInfo(filePath);
                                            if (FileDetailsList != null)
                                            {
                                                DirectFileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                                if (fileDetailsObj != null)
                                                {
                                                    fileDetailsObj.SoFile.SaveAs(attachedFileInfo.FullName);
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    GetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                                    SetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                                    this.EntryStatus = EntryStatus.LISTMODE;
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectSaleOrder);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    ResetForm(ControlsEnum.DIRECTSALEORDER);
                                    ResetForm(ControlsEnum.SALEORDERDETAIL);
                                    txtDtlRemark2.Text = string.Empty;
                                    //ResetForm(ControlsEnum.ADDITEM);
                                }
                                else
                                {
                                    #region Error Messages
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.Captions.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.REFERRED)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.DirectSaleOrder;
                                        litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.DirectSaleOrder + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "');", true);
                                    }
                                    else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.DirectSaleOrder + " " + Resources.Messages.AlreadyDeleted;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.Captions.Information + "');", true);
                                        EntryStatus = EntryStatus.LISTMODE;
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.DirectSaleOrder + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "','" + "');", true);
                                    }
                                    else if (result == -36)
                                    {
                                        litErrorMsg.Text = Resources.Messages.CannotModifyHaveReference;//Cannot modify,some of the items were referenced in other pages
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "','" + "');", true);
                                        return;
                                    }
                                    else if (result == -37)
                                    {
                                        litErrorMsg.Text = Resources.Messages.MsgRefAdded;//Supplier Ref# already entered
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "','" + "');", true);
                                        return;
                                    }
                                    else if (result == -6)//Total outstanding amount exceeds the credit limit set for the customer.	
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_OutstandingAmt_Exceeds").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == -12)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_Qty_Despatch").ToString();//SO Qty can not be less than DO Qty
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "','" + "');", true);
                                        return;
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectSaleOrder);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.Captions.Information + "');", true);
                                    }
                                    #endregion
                                }
                            }
                        }
                        break;
                    #endregion
                    #region SEARCH

                    case ActionsEnum.SEARCH:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        this.ModifiedDatePnl.Visible = false;
                        GetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                        SetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                        break;
                    #endregion
                    #region SO DETAILS - EXT GRID
                    case ActionsEnum.SODETAILS:
                        arg = ((Button)sender).CommandArgument;
                        gvrow = ((Button)sender).Parent.Parent as GridViewRow;
                        if (gvrow != null)
                        {
                            grd = gvrow.FindControl("grdDSOList") as GridView;
                            if (string.IsNullOrEmpty(arg))
                            {
                                DirectsaleOrderDetailsList = null;
                            }
                            else
                            {
                                grdSOHpk = Convert.ToInt32(arg);
                                GetFieldValues(ControlsEnum.SODETAILS);
                            }
                            grd.Visible = true;
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                grd.DataSource = dtPageData;
                                grd.DataBind();
                            }
                            (gvrow.FindControl("hdfIsExpandedOrders") as HiddenField).Value = "1";
                        }
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.LISTING);
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                        SetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                        break;
                    #endregion
                    #region EDIT DETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        foreach (GridViewRow grdrow in grdSaleOrderSearchList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDSOID")).Value);
                                grdSaleOrderSearchListRowDeptId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDepartmentID")).Value);
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(grdSaleOrderSearchListRowDeptId, 1);
                            SetUIEditView(commonActions);
                            EntryStatus = EntryStatus.EDITMODE;
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.EDITMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                            ClearDdl(ControlsEnum.UOM);
                            GetFieldValues(ControlsEnum.DETAILFOREDIT);
                            SetFieldValues(ControlsEnum.DETAILFOREDIT);
                            SetFieldValues(ControlsEnum.SUBCATEGORY);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region COPY SO
                    case ActionsEnum.COPY:
                        #region NEW
                        ResetForm(ControlsEnum.CLEAR);
                        ResetForm(ControlsEnum.DIRECTSALEORDER);
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        txtDtlRemark2.Text = string.Empty;
                        DirectSaleOrderHeaderSession = new DirectSaleOrderBO()
                        {
                            TaxHdrDtl = new List<DirectSaleOrderTaxHdr>(),
                            DirectsaleOrderDetails = new List<DirectSaleOrderDetailsBO>()
                        };
                        DirectTempSaleOrderHeaderSession = new DirectSaleOrderBO()
                        {
                            TaxHdrDtl = new List<DirectSaleOrderTaxHdr>(),
                            DirectsaleOrderDetails = new List<DirectSaleOrderDetailsBO>()
                        };
                        SOUploadList = new List<DirectSaleOrderUploads>();
                        #endregion

                        foreach (GridViewRow grdrow in grdSaleOrderSearchList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDSOID")).Value);
                            }
                        }
                        if (bIsChecked)
                        {
                            IsCopySO = 1;
                            GetFieldValues(ControlsEnum.DETAILFOREDIT);
                            SetFieldValues(ControlsEnum.DETAILFOREDIT);

                            this.EntryStatus = EntryStatus.NEWMODE;
                        }
                        else
                        {
                            IsCopySO = 0;
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = base.WkfRefID = IsCopySO = 0;
                        FillProcessID(0, 1);
                        this.ModifiedDatePnl.Visible = false;
                        ResetForm(ControlsEnum.LISTING);
                        ResetForm(ControlsEnum.DIRECTSALEORDER);
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        GetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                        SetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Sales.DirectSaleOrderBL.DeleteSaleOrderDetails(CurrPK, LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectSaleOrder);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                GetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                                SetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.DirectSaleOrder;
                                    litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.DirectSaleOrder + " " +
                                        GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.DirectSaleOrder + " " +
                                        GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.ALREADYDELETED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.DirectSaleOrder + " " +
                                        GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectSaleOrder);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region VIEW
                    case ActionsEnum.VIEW:
                        ActionHandler(btnEdit, EventArgs.Empty);
                        if (EntryStatus == EntryStatus.EDITMODE) this.EntryStatus = EntryStatus.VIEWMODE;
                        break;
                    #endregion
                    #region CUSTOMER SELECTED
                    case ActionsEnum.CUSTOMERSELECTED:
                        if (hdfCustomer.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCustomer.Value != string.Empty)
                        {
                            custPK = Convert.ToInt32(hdfCustomer.Value);

                            GetFieldValues(ControlsEnum.CUSTOMER);

                            if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                            {
                                if (dsPageData.Tables[0].Rows[0]["CUS_SPECIAL_CAT"] != null && dsPageData.Tables[0].Rows[0]["CUS_SPECIAL_CAT"].ToString() != string.Empty)
                                    CustomerCategory = (ERP.Utilities.Constants.DA.CustomerSpecialCategory)dsPageData.Tables[0].Rows[0]["CUS_SPECIAL_CAT"];

                                if (!dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].Equals(DBNull.Value) && ddlSOType.Items.FindByValue(dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].ToString()) != null)
                                    ddlSOType.SelectedValue = dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].ToString();

                                hdfCurrency.Value = dsPageData.Tables[0].Rows[0]["CUR_PK"].ToString();
                                txtCurrency.Text = string.Format(Resources.ErpRes.NameCodeFormat, dsPageData.Tables[0].Rows[0]["CUR_CODE"].ToString()
                                    , dsPageData.Tables[0].Rows[0]["CUR_NAME"].ToString());

                                address = string.Empty;
                                if (!string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString()))
                                {
                                    address += string.IsNullOrEmpty(address) ? HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString()) :
                                        string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString().Trim()) ?
                                        string.Empty : ", " + HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString()))
                                {
                                    address += string.IsNullOrEmpty(address) ? HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString().Trim()) ?
                                        string.Empty : ", " + HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString());
                                }
                                txtCusAddress.Text = address;

                                hdfCusAddress.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString());
                                hdfCusCountry.Value = dsPageData.Tables[0].Rows[0]["CUS_COUNTRY"].ToString();
                                hdfCusCountryText.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString());
                                hdfCusZip.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ZIP"].ToString());
                                hdfCusPhone.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_PHONE"].ToString());
                                hdfCusMobile.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_MOBILE"].ToString());
                                hdfCusFax.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_FAX"].ToString());
                                hdfCusEmail.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_EMAIL"].ToString());
                                lblSpecialCat.Visible = true;
                                lblSpecialCat.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_SPECIAL_CAT_TEXT"].ToString());
                                if (dsPageData.Tables[0].Rows[0]["CUS_DISC_PERC"] != null && dsPageData.Tables[0].Rows[0]["CUS_DISC_PERC"].ToString() != string.Empty)
                                    hdfCusDiscPerc.Value = dsPageData.Tables[0].Rows[0]["CUS_DISC_PERC"].ToString();
                                if (ddlSOType.SelectedValue != null && txtCurrency.Text != null)
                                {
                                    txtCurrency.Enabled = false;
                                    ddlSOType.Enabled = false;
                                }
                                else
                                {
                                    txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                                    ddlSOType.SelectedIndex = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                                }

                                GetFieldValues(ControlsEnum.EXCHANGERATE);
                                if (exchangeRate > 0)
                                {
                                    hdfExchangeRate.Value = exchangeRate.ToString(hdfExchangeRateFormat.Value);
                                    txtExchangeRate.Text = hdfExchangeRate.Value;
                                    //txtExchangeRate.Enabled = false;
                                }
                                else
                                {
                                    txtExchangeRate.Text = string.Empty;
                                    //txtExchangeRate.Enabled = true;
                                }
                            }
                        }
                        if (IsHeaderTaxForTradingSale && hdfCustomer.Value != string.Empty && Convert.ToInt32(hdfCustomer.Value) > 0)
                        {
                            DirectSaleOrderHeaderSession.TaxHdrDtl = new List<DirectSaleOrderTaxHdr>();
                            GetFieldValues(ControlsEnum.AUTOTAXPOPUP);
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.AUTOTAXPOPUP);
                        }
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        break;
                    #endregion
                    #region ITEM SELECTED
                    case ActionsEnum.ITEMSELECTED:
                    
                        if ((DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.Where(itm => (itm.SOD_ITEM_CATEGORY == Convert.ToInt32(hdfItemCategory.Value)) && itm.SOD_ITEM == Convert.ToInt32(hdfItem.Value)).Count() == 0))
                        {
                            int HasPackSpecVal = 0;
                            if (hdfNeedPackSpecVal.Value == "0")
                                HasPackSpecVal = 1;
                            else
                            {
                                if (hdfPackingSpec.Value != string.Empty && Convert.ToInt32(hdfPackingSpec.Value) > 0)
                                    HasPackSpecVal = 1;
                                else
                                    HasPackSpecVal = 0;
                            }

                            if (HasPackSpecVal > 0)
                            {
                                int itmPK = Convert.ToInt32(hdfItem.Value);
                                GetFieldValues(ControlsEnum.UOM);
                                SetFieldValues(ControlsEnum.UOM);
                                if (dtPageData != null && dtPageData.Rows.Count > 0)
                                {
                                    DataTable dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemDetailsByPK(Convert.ToInt32(hdfItem.Value), currentUser.SBUID);
                                    if (dtItemDetails.Rows.Count > 0)
                                    {
                                        if (!dtItemDetails.Rows[0]["ITM_UOM_SALE"].Equals(DBNull.Value) && ddlUOM.Items.FindByValue(dtItemDetails.Rows[0]["ITM_UOM_SALE"].ToString()) != null)
                                            ddlUOM.SelectedValue = dtItemDetails.Rows[0]["ITM_UOM_SALE"].ToString();
                                        hdfUOM.Value = dtItemDetails.Rows[0]["ITM_UOM_SALE"].ToString();
                                        hdfItemCategory.Value = dtItemDetails.Rows[0]["ITM_CATEGORY"].ToString();
                                        txtItemCategory.Text = dtItemDetails.Rows[0]["ITC_NAME"].ToString();
                                        hdfTotalBagPcs.Value = dtItemDetails.Rows[0]["APS_TOTAL_PCS"].ToString();
                                        hdfPouchPcs.Value = dtItemDetails.Rows[0]["APS_PC_PCS"].ToString();
                                        //04-OCT-2019    
                                        txtUnitPrice.Text = "0.00";

                                        if (ConfigRateGet == 1)
                                        {
                                            DataTable dtItemDetailsNew = BusinessLogic.CommonManagement.CommonBL.GetItemRateDetails(Convert.ToInt32(hdfItem.Value), Convert.ToDateTime(txtSaleOrderDate.Text.Trim()), Convert.ToInt32(hdfCustomer.Value), Convert.ToInt32(hdfCurrency.Value));
                                            if (dtItemDetailsNew != null && dtItemDetailsNew.Rows.Count > 0)
                                            {
                                                txtUnitPrice.Text = GetFormattedRate(dtItemDetailsNew.Rows[0]["BPH_RATE"].ToString());
                                            }
                                        }
                                        else
                                        {
                                         switch (CustomerCategory)
                                        {
                                            case ERP.Utilities.Constants.DA.CustomerSpecialCategory.InterState:
                                                txtUnitPrice.Text = GetFormattedRate(dtItemDetails.Rows[0]["ITM_INTER_STATE"].ToString());
                                                break;
                                            case ERP.Utilities.Constants.DA.CustomerSpecialCategory.IntraState:
                                                txtUnitPrice.Text = GetFormattedRate(dtItemDetails.Rows[0]["ITM_INTRA_STATE"].ToString());
                                                break;
                                            case ERP.Utilities.Constants.DA.CustomerSpecialCategory.Overseas:
                                                txtUnitPrice.Text = GetFormattedRate(dtItemDetails.Rows[0]["ITM_EXPORT"].ToString());
                                                break;
                                            default:
                                                txtUnitPrice.Text = GetFormattedRate(dtItemDetails.Rows[0]["ITM_OTHERS"].ToString());
                                                break;
                                        }

                                        }

                                     //////////////////////
                                    }
                                }
                                if (hdfCustomer.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCustomer.Value != string.Empty)
                                {
                                    custPK = Convert.ToInt32(hdfCustomer.Value);
                                }
                                if (hdfItem.Value != CommonConstants.SELECT_VALUE_ZERO && hdfItem.Value != string.Empty)
                                {
                                    custItemPK = Convert.ToInt32(hdfItem.Value);

                                    txtItemQuantity.Text = txtDiscount.Text = txtTax.Text = txtAmount.Text = txtTotalAmt.Text = GetFormattedCurrency(0);
                                    //txtUnitPrice.Text = GetFormattedRate(0);
                                }

                                //Fill tax pop default 
                                if (hdfItemCatVal.Value == "9" && IsItemwiseTaxForTradingSale && Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "EnableCustomerTaxforSChdrDtl")) == 1)
                                {
                                    txtTax.Text = txtDiscount.Text = string.Empty;
                                    GetFieldValues(ControlsEnum.AUTOTAXPOPUP);
                                    SetFieldValues(ControlsEnum.AUTOTAXPOPUPITEMWISE);
                                }

                                if (IsItemwiseDiscountForTradingSale)
                                {
                                    txtDiscount.Text = string.Empty;
                                    SetFieldValues(ControlsEnum.AUTODISCOUNTPOPUP);
                                }

                                //include in list
                                DirectsaleOrderDetailsList = DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails;

                                SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                                SelectedItemCategory = string.IsNullOrEmpty(hdfItemCategory.Value) ? 0 : Convert.ToInt32(hdfItemCategory.Value);
                                SelectedItemPK = string.IsNullOrEmpty(hdfItem.Value) ? 0 : Convert.ToInt32(hdfItem.Value);
                                if (CurrSlNo > 0)
                                {
                                    DirectsaleOrderDetailsObj = DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                }
                                else
                                {
                                    if (hdfisItemHaveTax.Value.ToString() != "0")
                                    {
                                        DirectsaleOrderDetailsObj = DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails == null ? null :
                                               DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.LastOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK
                                               && ctr.SOD_ITEM_CATEGORY == SelectedItemCategory && ctr.SOD_ITEM == SelectedItemPK);
                                    }
                                }
                                if (DirectsaleOrderDetailsObj == null)
                                {
                                    if (DirectsaleOrderDetailsList != null && DirectsaleOrderDetailsList.Count > 0)
                                    {
                                        IsItem = true; hdfIsItemYes.Value = "1";
                                        DirectsaleOrderDetailsList = (List<DirectSaleOrderDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                        DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails = DirectsaleOrderDetailsList;
                                        DirectsaleOrderDetailsObj = DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                        IsItem = false; hdfIsItemYes.Value = "0";
                                    }
                                }
                                txtItem.Focus();
                            }
                            else
                            {
                                txtItem.Text = Resources.ErpRes.AutoDefaultValue;
                                hdfItem.Value = string.Empty;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_PackSpec").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        else
                        {
                            txtItem.Text = Resources.ErpRes.AutoDefaultValue;
                            hdfItem.Value = string.Empty;
                            DirectSaleOrderDetailsBO TempLst = new DirectSaleOrderDetailsBO();
                            TempLst = DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.FirstOrDefault(x => Convert.ToDouble(x.SOD_AMOUNT) == 0);
                            DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.Remove(TempLst);
                            litErrorMsg.Text = GetLocalResourceObject("ItemExist").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        break;
                    #endregion

                    #region ORDER DETAILS
                    #region ADD ITEM DETAIL
                    case ActionsEnum.ADDITEM:
                        if (txtAmount.Text.Length <= 15)
                        {
                            DirectSaleOrderHeaderSession = DirectTempSaleOrderHeaderSession;
                            DirectsaleOrderDetailsList = DirectSaleOrderHeaderSession.DirectsaleOrderDetails;
                            DirectsaleOrderDetailsList = (List<DirectSaleOrderDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);

                            if (DirectsaleOrderDetailsList != null && DirectsaleOrderDetailsList.Count > 0)
                            {
                                DirectSaleOrderHeaderSession.DirectsaleOrderDetails = DirectsaleOrderDetailsList;
                                SetDetailTax(DirectSaleOrderHeaderSession);
                                SetSubTotal();
                                SetHdrTax();
                                DirectsaleOrderHeaderObj = DirectSaleOrderHeaderSession;
                                SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                                ResetForm(ControlsEnum.SALEORDERDETAIL);
                                chkAddtlRemarkToAll.Checked = false;
                                chkRemarkToAll.Checked = false;
                                int i = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "SCAdditionalRemarksClear"));
                                if (i == 0)
                                {
                                    txtDtlRemark2.Text = string.Empty;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region EDIT ITEM DETAIL
                    case ActionsEnum.EDITITEM:
                        if (DirectSaleOrderHeaderSession.DirectsaleOrderDetails != null && DirectSaleOrderHeaderSession.DirectsaleOrderDetails.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdOrderDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSlNo > 0)
                            {
                                DirectsaleOrderDetailsObj = DirectSaleOrderHeaderSession.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                hdfItem.Value = DirectsaleOrderDetailsObj.SOD_ITEM.ToString();
                                GetFieldValues(ControlsEnum.UOM);
                                SetFieldValues(ControlsEnum.UOM);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDITEM);
                                if (dtPageData != null && dtPageData.Rows.Count > 0)
                                {
                                    DataTable dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemDetailsByPK(Convert.ToInt32(hdfItem.Value), currentUser.SBUID);
                                    if (dtItemDetails.Rows.Count > 0)
                                    {
                                        hdfTotalBagPcs.Value = dtItemDetails.Rows[0]["APS_TOTAL_PCS"].ToString();
                                        hdfPouchPcs.Value = dtItemDetails.Rows[0]["APS_PC_PCS"].ToString();
                                    }
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateBoxQty", "ClosePopup();CalculateBoxQty();", true);
                            }
                            hdfIsOrderDetailsVisible.Value = CommonConstants.SELECT_VALUE_ONE;
                        }
                        break;
                    #endregion
                    #region REMOVE ITEM
                    case ActionsEnum.REMOVEITEM:
                        if (DirectSaleOrderHeaderSession.DirectsaleOrderDetails != null && DirectSaleOrderHeaderSession.DirectsaleOrderDetails.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdOrderDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSlNo > 0)
                            {
                                DirectSaleOrderHeaderSession.DirectsaleOrderDetails = DirectSaleOrderHeaderSession.DirectsaleOrderDetails.Where(row => CurrSlNo != row.SOD_SL_NO).ToList();
                                SetDetailTax(DirectSaleOrderHeaderSession);
                                SetSubTotal();
                                SetHdrTax();
                                DirectsaleOrderHeaderObj = DirectSaleOrderHeaderSession;
                                SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                                IsItemBind = true;
                                hdfIsMsgIOReview.Value = "1";
                            }
                        }
                        ResetForm(ControlsEnum.SALEORDERDETAIL);

                        if (grdOrderDetails.Rows.Count == 0)
                        {
                            txtHdrDiscount.Text = txtHdrOtrCharge.Text = txtHdrTax.Text = txtHdrPriceAdj.Text = txtHdrTotal.Text = GetFormattedCurrency(0);
                        }

                        break;
                    #endregion
                    #region CLEAR ITEM
                    case ActionsEnum.CLEARITEM:
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        break;
                    #endregion
                    #endregion

                    #region DISCOUNT ITEM POPUP
                    case ActionsEnum.DISCDETAILS:
                        if ((IsHeaderDiscountForTradingSale && IsItemwiseDiscountForTradingSale) || txtHdrDiscount.Text == string.Empty || txtHdrDiscount.Text == "0.00")
                        {
                            dvPerc.Visible = false;
                            #region Validation
                            if (txtItemQuantity.Text == GetFormattedCurrency(0) || txtItemQuantity.Text == string.Empty || txtUnitPrice.Text == GetFormattedRate(0) || txtUnitPrice.Text == string.Empty)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_QtyPrice").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                break;
                            }
                            else if (txtItemQuantity.Text == GetFormattedCurrency(0) || txtItemQuantity.Text == string.Empty)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_Quantity").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                break;
                            }
                            else if (txtUnitPrice.Text == GetFormattedRate(0) || txtUnitPrice.Text == string.Empty)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_UnitPrice").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                break;
                            }
                            #endregion

                            EditDirectTempSaleOrderHeaderSession = DirectTempSaleOrderHeaderSession;

                            DirectsaleOrderDetailsList = EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails;

                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            if (CurrSlNo > 0)
                            {
                                DirectsaleOrderDetailsObj = EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails == null ? null :
                                        EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.SingleOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK && ctr.SOD_SL_NO == CurrSlNo);
                            }

                            if (DirectsaleOrderDetailsObj == null)
                            {
                                IsItem = true; hdfIsItemYes.Value = "1";
                                DirectsaleOrderDetailsList = (List<DirectSaleOrderDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails = DirectsaleOrderDetailsList;
                                DirectsaleOrderDetailsObj = EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                IsItem = false; hdfIsItemYes.Value = "0";
                            }
                            if (DirectsaleOrderDetailsObj.SOD_AMOUNT <= 0)
                            {
                                DirectsaleOrderDetailsObj.SOD_AMOUNT = double.Parse(txtAmount.Text);
                            }
                            if (DirectsaleOrderDetailsObj != null)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                                hdfTaxFormula.Value = string.Empty;
                                txtPopupAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value);

                                if (EditDirectTempSaleOrderHeaderSession != null)
                                {
                                    IsHeaderTax = false;

                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);

                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.TAXTYPES);
                                            TaxPK = 0;
                                            if (dsSaleOrderTaxDetails != null && dsSaleOrderTaxDetails.Tables[0].Rows.Count == 1)
                                            {
                                                string taxFormula = dsSaleOrderTaxDetails.Tables[0].Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupAmount.Text.Trim());
                                                txtPopupCharge.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                SelectedTaxText = HttpUtility.HtmlDecode(dsSaleOrderTaxDetails.Tables[0].Rows[0]["TAX_HEAD"].ToString());
                                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                            }
                                        }
                                        else
                                        {
                                            SelectedTaxText = Resources.Report.Custom;
                                            txtPopupCharge.Text = string.Empty;
                                        }
                                        txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                        {
                                            txtPopupCharge.Enabled = true;
                                            txtPopupOther.Enabled = true;
                                        }
                                        else
                                        {
                                            txtPopupCharge.Enabled = false;
                                            txtPopupOther.Enabled = false;
                                        }
                                    }
                                    divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                    IsEditMode = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                                }
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("DiscountHeaderEnabled").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        break;
                    #endregion
                    #region TAX ITEM POPUP
                    case ActionsEnum.TAXDETAILS:

                        if ((IsHeaderTaxForTradingSale && IsItemwiseTaxForTradingSale) || txtHdrTax.Text == string.Empty || txtHdrTax.Text == "0.00")
                        {
                            dvPerc.Visible = false;
                            #region Validation
                            if (txtItemQuantity.Text == GetFormattedCurrency(0) || txtItemQuantity.Text == string.Empty || txtUnitPrice.Text == GetFormattedRate(0) || txtUnitPrice.Text == string.Empty)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_QtyPrice").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                break;
                            }
                            else if (txtItemQuantity.Text == GetFormattedCurrency(0) || txtItemQuantity.Text == string.Empty)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_Quantity").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                break;
                            }
                            else if (txtUnitPrice.Text == GetFormattedRate(0) || txtUnitPrice.Text == string.Empty)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_UnitPrice").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                break;
                            }
                            #endregion

                            EditDirectTempSaleOrderHeaderSession = DirectTempSaleOrderHeaderSession;
                            DirectsaleOrderDetailsList = EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails;

                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            SelectedItemCategory = string.IsNullOrEmpty(hdfItemCategory.Value) ? 0 : Convert.ToInt32(hdfItemCategory.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfItem.Value) ? 0 : Convert.ToInt32(hdfItem.Value);
                            if (CurrSlNo > 0)
                            {
                                DirectsaleOrderDetailsObj = EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails == null ? null :
                                       EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.SingleOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK && ctr.SOD_SL_NO == CurrSlNo);
                            }
                            else
                            {
                                if (hdfisItemHaveTax.Value.ToString() != "0")
                                {
                                    DirectsaleOrderDetailsObj = EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails == null ? null :
                                           EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.LastOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK
                                           && ctr.SOD_ITEM_CATEGORY == SelectedItemCategory && ctr.SOD_ITEM == SelectedItemPK);
                                }
                            }

                            if (DirectsaleOrderDetailsObj == null)
                            {
                                DirectsaleOrderDetailsList = (List<DirectSaleOrderDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails = DirectsaleOrderDetailsList;
                            }

                            if (DirectsaleOrderDetailsList != null && DirectsaleOrderDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                                hdfTaxFormula.Value = string.Empty;

                                txtPopupAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) :
                                    string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value) :
                                    (Convert.ToDouble(txtAmount.Text.Trim()) - Convert.ToDouble(txtDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);

                                if (EditDirectTempSaleOrderHeaderSession != null)
                                {
                                    IsHeaderTax = false;

                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);

                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.TAXTYPES);
                                            TaxPK = 0;

                                            if (dsSaleOrderTaxDetails != null && dsSaleOrderTaxDetails.Tables[0].Rows.Count == 1)
                                            {
                                                string taxFormula = dsSaleOrderTaxDetails.Tables[0].Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupAmount.Text.Trim());
                                                txtPopupCharge.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                SelectedTaxText = HttpUtility.HtmlDecode(dsSaleOrderTaxDetails.Tables[0].Rows[0]["TAX_HEAD"].ToString());
                                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                            }
                                        }
                                        else
                                        {
                                            SelectedTaxText = Resources.Report.Custom;
                                            txtPopupCharge.Text = string.Empty;
                                        }
                                        txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                        {
                                            txtPopupOther.Enabled = true;
                                        }
                                        else
                                        {
                                            txtPopupOther.Enabled = false;
                                        }
                                    }
                                    divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                    IsEditMode = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                                }
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("TaxHeaderEnabled").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        break;
                    #endregion

                    #region DISCOUNT HEADER POPUP
                    case ActionsEnum.DISCOUNTHEADER:

                        if (grdOrderDetails.Rows.Count <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            dvPerc.Visible = false;
                            var footerRow = grdOrderDetails.FooterRow;
                            var lblFtrDiscount = (Label)footerRow.FindControl("lblItemTotalDiscount");

                            if (lblFtrDiscount.Text != "0.00" && (!IsHeaderDiscountForTradingSale || !IsItemwiseDiscountForTradingSale))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("LineItemDiscountEnabled").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                            }
                            else
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                                hdfTaxFormula.Value = string.Empty;
                                if (DirectSaleOrderHeaderSession != null)
                                {
                                    ResetForm(ControlsEnum.SALEORDERDETAIL);
                                    DirectTempSaleOrderHeaderSession = DirectSaleOrderHeaderSession;
                                    IsHeaderTax = true;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);
                                    lblSubTotal = grdOrderDetails.FooterRow == null ? null : (Label)grdOrderDetails.FooterRow.FindControl("lblItemTotalAmnt");
                                    if (lblSubTotal != null)
                                    {
                                        txtPopupAmount.Text = string.IsNullOrEmpty(lblSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(lblSubTotal.Text).ToString(hdfCurrencyFormat.Value);
                                        if (ddlPopupTaxType.Items.Count > 0)
                                        {
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                            {
                                                TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                                GetFieldValues(ControlsEnum.TAXTYPES);
                                                TaxPK = 0;
                                                if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                                {
                                                    string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                    hdfTaxFormula.Value = taxFormula;
                                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupAmount.Text.Trim());
                                                    txtPopupCharge.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                    SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                                }
                                            }
                                            else
                                            {
                                                SelectedTaxText = Resources.Report.Custom;
                                                txtPopupCharge.Text = string.Empty;
                                            }
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                            {
                                                txtPopupCharge.Enabled = true;
                                                txtPopupOther.Enabled = true;
                                            }
                                            else
                                            {
                                                txtPopupCharge.Enabled = false;
                                                txtPopupOther.Enabled = false;
                                            }
                                        }
                                        divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                        IsEditMode = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region TAXHEADER
                    case ActionsEnum.TAXHEADER:

                        if (grdOrderDetails.Rows.Count <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            var footerRow2 = grdOrderDetails.FooterRow;
                            var lblFtrTax = (Label)footerRow2.FindControl("lblItemTotalTax");

                            if (lblFtrTax.Text != "0.00" && (!IsHeaderTaxForTradingSale || !IsItemwiseTaxForTradingSale))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("LineItemTaxEnabled").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                            }
                            else
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                                hdfTaxFormula.Value = string.Empty;
                                if (DirectSaleOrderHeaderSession != null)
                                {
                                    ResetForm(ControlsEnum.SALEORDERDETAIL);
                                    DirectTempSaleOrderHeaderSession = DirectSaleOrderHeaderSession;
                                    IsHeaderTax = true;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);
                                    double taxable = 0;
                                    lblSubTotal = grdOrderDetails.FooterRow == null ? null : (Label)grdOrderDetails.FooterRow.FindControl("lblItemTotalAmnt");
                                    if (IsHeaderTaxForTradingSale && IsItemwiseTaxForTradingSale)
                                    {
                                        ResetForm(ControlsEnum.TAXCHECKBOX);
                                        if (chkSubTotal.Checked)
                                            taxable += lblSubTotal.Text != string.Empty ? Convert.ToDouble(lblSubTotal.Text) : 0;
                                        if (chkDiscount.Checked)
                                            taxable = txtHdrDiscount.Text != string.Empty ? taxable - Convert.ToDouble(txtHdrDiscount.Text) : taxable;
                                        if (chkOtherCharges.Checked)
                                            taxable += txtHdrOtrCharge.Text != string.Empty ? Convert.ToDouble(txtHdrOtrCharge.Text) : 0;
                                    }
                                    else
                                    {
                                        taxable = Convert.ToDouble(DirectSaleOrderHeaderSession.SOH_TOTAL_AMT) - DirectSaleOrderHeaderSession.SOH_TOTAL_DISCOUNT;

                                        //Add Other Charges Based on configuration
                                        if (IsTaxForOtherCharge)
                                        {
                                            taxable += string.IsNullOrEmpty(txtHdrOtrCharge.Text) ? 0.00 : Convert.ToDouble(txtHdrOtrCharge.Text);
                                        }
                                    }
                                    txtPopupAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);

                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.TAXTYPES);
                                            TaxPK = 0;
                                            if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupAmount.Text.Trim());
                                                txtPopupCharge.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                            }
                                        }
                                        else
                                        {
                                            SelectedTaxText = Resources.Report.Custom;
                                            txtPopupAmount.Text = string.Empty;
                                        }
                                        txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                        {
                                            txtPopupAmount.Enabled = true;
                                            txtPopupOther.Enabled = true;
                                        }
                                        else
                                        {
                                            txtPopupAmount.Enabled = false;
                                            txtPopupOther.Enabled = false;
                                        }
                                    }
                                    if (IsHeaderTaxForTradingSale && IsItemwiseTaxForTradingSale)
                                        divTaxApplicableAmount.Attributes.Add("style", "display:block;");
                                    else
                                        divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                    IsEditMode = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                                }
                            }
                        }

                        break;

                    #endregion
                    #region OTHER CHARGES HEADER POPUP
                    case ActionsEnum.OTHERCHARGEHEADER:
                        dvPerc.Visible = false;
                        hdfTaxCategory.Value = ((int)TaxType.Shipping).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (DirectSaleOrderHeaderSession != null)
                        {
                            ResetForm(ControlsEnum.SALEORDERDETAIL);
                            DirectTempSaleOrderHeaderSession = DirectSaleOrderHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            lblSubTotal = grdOrderDetails.FooterRow == null ? null : (Label)grdOrderDetails.FooterRow.FindControl("lblItemTotalAmnt");
                            if (lblSubTotal != null)
                            {
                                //double taxable = Convert.ToDouble(DirectSaleOrderHeaderSession.SOH_TOTAL_AMT) - DirectSaleOrderHeaderSession.SOH_TOTAL_DISCOUNT;
                                double taxable = DirectSaleOrderHeaderSession.DirectsaleOrderDetails.Sum(s => s.SOD_AMOUNT);
                                txtPopupAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);

                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupAmount.Text.Trim());
                                            txtPopupCharge.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupCharge.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = Resources.Report.Freight;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                        txtPopupCharge.Enabled = true;
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = false;
                                        txtPopupOther.Enabled = false;
                                        txtPopupCharge.Enabled = false;
                                    }
                                }
                                divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("OtherCharges").ToString() + "','600','300');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region TAXADD
                    case ActionsEnum.TAXADD:

                        bool errorTaxAdd = false;
                        bool errorTaxAmount = false;

                        if (IsHeaderTax ? DirectTempSaleOrderHeaderSession != null : EditDirectTempSaleOrderHeaderSession != null)
                        {
                            DirectsaleOrderHeaderObj = IsHeaderTax ? DirectTempSaleOrderHeaderSession : EditDirectTempSaleOrderHeaderSession;
                            DirecttempSaleOrderTaxHdrObj = null;
                            if (IsHeaderTax)
                            {
                                #region IsHeaderTax
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    DirecttempSaleOrderTaxHdrObj = DirectsaleOrderHeaderObj.TaxHdrDtl == null ? null :
                                        DirectsaleOrderHeaderObj.TaxHdrDtl.SingleOrDefault(ctr => ctr.SLT_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && ctr.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                else
                                {
                                    DirecttempSaleOrderTaxHdrObj = DirectsaleOrderHeaderObj.TaxHdrDtl == null ? null :
                                        DirectsaleOrderHeaderObj.TaxHdrDtl.SingleOrDefault(ctr => ctr.SLT_NAME == txtPopupOther.Text.Trim() && ctr.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                #endregion
                            }
                            else
                            {
                                if (CurrSlNo > 0)
                                {
                                    DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                }
                                else
                                {
                                    DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails == null ? null :
                                     DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK
                                     && ctr.SOD_ITEM == SelectedItemPK);
                                }

                                if (DirectsaleOrderDetailsObj != null)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        DirecttempSaleOrderTaxHdrObj = DirectsaleOrderDetailsObj.TaxDtl == null ? null :
                                            DirectsaleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        DirecttempSaleOrderTaxHdrObj = DirectsaleOrderDetailsObj.TaxDtl == null ? null :
                                            DirectsaleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_NAME == txtPopupOther.Text.Trim() && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            if (DirecttempSaleOrderTaxHdrObj == null)
                            {
                                DirectsaleOrderTaxHdrList = new List<DirectSaleOrderTaxHdr>();
                                DirectsaleOrderTaxHdrObj = new DirectSaleOrderTaxHdr();

                                try
                                {
                                    DirectsaleOrderTaxHdrObj.SLT_TAX_AMT = string.IsNullOrEmpty(txtPopupCharge.Text.Trim()) ? 0 : Convert.ToDouble(txtPopupCharge.Text.Trim());
                                }
                                catch
                                {
                                    errorTaxAmount = true;
                                }
                                if (!errorTaxAmount)
                                {
                                    DirectsaleOrderTaxHdrObj.SLT_SO_DTL = SelectedDtlPK;
                                    DirectsaleOrderTaxHdrObj.SLT_SL_NO = 1;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        DirectsaleOrderTaxHdrObj.SLT_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    }

                                    DirectsaleOrderTaxHdrObj.SLT_DISC_PERC = txtTaxPerc.Text == "" || Convert.ToDouble(txtTaxPerc.Text) < 0 ? 0 : Convert.ToDouble(txtTaxPerc.Text);
                                    DirectsaleOrderTaxHdrObj.SLT_TAX_TEXT = HttpUtility.HtmlEncode(SelectedTaxText);
                                    DirectsaleOrderTaxHdrObj.SLT_NAME = HttpUtility.HtmlEncode(txtPopupOther.Text.Trim());
                                    DirectsaleOrderTaxHdrObj.SLT_PK = 0;
                                    DirectsaleOrderTaxHdrObj.SLT_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    DirectsaleOrderTaxHdrObj.SLT_TYPE = 1;
                                    DirectsaleOrderTaxHdrObj.SLT_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    if (IsHeaderTax)
                                    {
                                        if (DirectsaleOrderTaxHdrObj.SLT_TAX_CATEGORY == (int)TaxType.Tax && IsHeaderTaxForTradingSale && IsItemwiseTaxForTradingSale)
                                        {
                                            DirectsaleOrderTaxHdrObj.SLT_HAS_SUB_TOTAL = chkSubTotal.Checked ? 1 : 0;
                                            DirectsaleOrderTaxHdrObj.SLT_HAS_DISCOUNT = chkDiscount.Checked ? 1 : 0;
                                            DirectsaleOrderTaxHdrObj.SLT_HAS_OTHER_CHARGE = chkOtherCharges.Checked ? 1 : 0;
                                        }

                                        totalAmt = 0;
                                        currentTotal = 0;
                                        taxAmt = 0;

                                        totalAmt = Convert.ToDouble(DirectsaleOrderHeaderObj.SOH_TOTAL_AMT);
                                        currentTotal = DirectsaleOrderHeaderObj.TaxHdrDtl == null ? 0 :
                                            DirectsaleOrderHeaderObj.TaxHdrDtl.Where(htx => htx.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.SLT_TAX_AMT);

                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            taxAmt = CalculateTaxFormula(DirectsaleOrderTaxHdrObj.SLT_TAX_FORMULA, totalAmt);
                                        }
                                        else
                                        {
                                            taxAmt = DirectsaleOrderTaxHdrObj.SLT_TAX_AMT;
                                        }

                                        if (totalAmt >= (currentTotal + taxAmt))
                                        {
                                            DirectsaleOrderTaxHdrList = DirectsaleOrderHeaderObj.TaxHdrDtl.ToList();
                                            DirectsaleOrderTaxHdrList.Add(DirectsaleOrderTaxHdrObj);
                                            DirectsaleOrderHeaderObj.TaxHdrDtl = DirectsaleOrderTaxHdrList;
                                        }
                                        else
                                        {
                                            if (DirectsaleOrderTaxHdrObj.SLT_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                isValidDisc = false;
                                            }
                                            else
                                            {
                                                isValidTax = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (CurrSlNo > 0)
                                        {
                                            DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                        }
                                        else
                                        {
                                            DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails == null ? null :
                                                DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(item => item.SOD_PK == SelectedDtlPK
                                                && item.SOD_ITEM == SelectedItemPK);
                                        }
                                        if (DirectsaleOrderDetailsObj != null)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = DirectsaleOrderDetailsObj.SOD_AMOUNT;
                                            currentTotal = DirectsaleOrderDetailsObj.TaxDtl == null ? 0 :
                                                DirectsaleOrderDetailsObj.TaxDtl.Where(dtx => dtx.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.SLT_TAX_AMT);
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(DirectsaleOrderTaxHdrObj.SLT_TAX_FORMULA, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = DirectsaleOrderTaxHdrObj.SLT_TAX_AMT;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                DirectsaleOrderTaxHdrList = DirectsaleOrderDetailsObj.TaxDtl == null ? new List<DirectSaleOrderTaxHdr>() : DirectsaleOrderDetailsObj.TaxDtl.ToList();
                                                DirectsaleOrderTaxHdrList.Add(DirectsaleOrderTaxHdrObj);
                                                if (CurrSlNo > 0)
                                                {
                                                    DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = DirectsaleOrderTaxHdrList;
                                                }
                                                else
                                                {
                                                    DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                                        && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = DirectsaleOrderTaxHdrList;
                                                }
                                            }
                                            else
                                            {
                                                if (DirectsaleOrderTaxHdrObj.SLT_TAX_CATEGORY == (int)TaxType.Discount)
                                                {
                                                    isValidDisc = false;
                                                }
                                                else
                                                {
                                                    isValidTax = false;
                                                }
                                            }
                                        }
                                    }
                                    EditDirectTempSaleOrderHeaderSession = DirectsaleOrderHeaderObj;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                }
                            }
                            else
                            {
                                errorTaxAdd = true;
                            }
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    txtPopupCharge.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    txtPopupCharge.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                                if (!errorTaxAdd && !errorTaxAmount)
                                {
                                    txtPopupCharge.Text = string.Empty;
                                    txtPopupOther.Text = string.Empty;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("OtherCharges").ToString() : (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString())) + "','600','300');", true);

                        if (errorTaxAdd)
                        {
                            if (hdfTaxCategory.Value == "1")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (hdfTaxCategory.Value == "2")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Shipping_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (hdfTaxCategory.Value == "3")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Disc_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        else if (errorTaxAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (!isValidDisc)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Discount_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        else if (!isValidTax)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amt").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        if (!isValidDiscAm)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Discount_Amount_Amend").ToString()) + "','" + Resources.ErpRes.Information + "');", true);

                        break;
                    #endregion
                    #region TAXAPPLY
                    case ActionsEnum.TAXAPPLY:
                        if (IsHeaderTax)
                        {
                            DirectSaleOrderHeaderSession = DirectTempSaleOrderHeaderSession;
                            SetSubTotal();
                            SetHdrTax();
                        }
                        else
                        {
                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfItem.Value) ? 0 : Convert.ToInt32(hdfItem.Value);
                            DirectTempSaleOrderHeaderSession = EditDirectTempSaleOrderHeaderSession;
                            if (CurrSlNo > 0)
                            {
                                DirectsaleOrderDetailsObj = DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                            }
                            else
                            {
                                DirectsaleOrderDetailsObj = DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.LastOrDefault(crt => crt.SOD_PK == SelectedDtlPK
                                 && crt.SOD_ITEM == SelectedItemPK);
                            }

                            if (DirectsaleOrderDetailsObj != null)
                            {
                                CurrSlNo = DirectsaleOrderDetailsObj.SOD_SL_NO;
                            }
                            SetDetailTax(DirectTempSaleOrderHeaderSession);
                        }
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region TAXDELETE
                    case ActionsEnum.TAXDELETE:
                        if (IsHeaderTax ? DirectTempSaleOrderHeaderSession != null : EditDirectTempSaleOrderHeaderSession != null)
                        {
                            DirectsaleOrderHeaderObj = IsHeaderTax ? DirectTempSaleOrderHeaderSession : EditDirectTempSaleOrderHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                DirectsaleOrderTaxHdrList = new List<DirectSaleOrderTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    #region Header Tax
                                    if (taxPK > 0)
                                    {
                                        DirecttempSaleOrderTaxHdrObj = DirectsaleOrderHeaderObj.TaxHdrDtl == null ? null :
                                            DirectsaleOrderHeaderObj.TaxHdrDtl.SingleOrDefault(rfq => rfq.SLT_TAX == taxPK && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            DirecttempSaleOrderTaxHdrObj = DirectsaleOrderHeaderObj.TaxHdrDtl == null ? null :
                                                DirectsaleOrderHeaderObj.TaxHdrDtl.LastOrDefault(rfq => rfq.SLT_NAME == hdfTaxName.Value && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                    if (DirecttempSaleOrderTaxHdrObj != null)
                                    {
                                        DirectsaleOrderTaxHdrList = DirectsaleOrderHeaderObj.TaxHdrDtl.ToList();
                                        DirectsaleOrderTaxHdrList.Remove(DirecttempSaleOrderTaxHdrObj);
                                        DirectsaleOrderHeaderObj.TaxHdrDtl = DirectsaleOrderTaxHdrList;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Detail Tax
                                    if (CurrSlNo > 0)
                                    {
                                        DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails == null ? null :
                                           DirectsaleOrderHeaderObj.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                    }
                                    else
                                    {
                                        DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails == null ? null :
                                         DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                         && rfq.SOD_ITEM == SelectedItemPK);
                                    }

                                    if (DirectsaleOrderDetailsObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            DirecttempSaleOrderTaxHdrObj = DirectsaleOrderDetailsObj.TaxDtl == null ? null :
                                                DirectsaleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_TAX == taxPK && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                DirecttempSaleOrderTaxHdrObj = DirectsaleOrderDetailsObj.TaxDtl == null ? null :
                                                    DirectsaleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_NAME == hdfTaxName.Value && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        if (CurrSlNo > 0)
                                        {
                                            DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                        }
                                        else
                                        {
                                            DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                                && rfq.SOD_ITEM == SelectedItemPK);
                                        }

                                        if (DirectsaleOrderDetailsObj != null && DirectsaleOrderDetailsObj.TaxDtl != null)
                                        {
                                            DirectsaleOrderTaxHdrList = DirectsaleOrderDetailsObj.TaxDtl.ToList();
                                            DirectsaleOrderTaxHdrList.Remove(DirecttempSaleOrderTaxHdrObj);
                                            if (CurrSlNo > 0)
                                            {
                                                DirectsaleOrderHeaderObj.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = DirectsaleOrderTaxHdrList;
                                            }
                                            else
                                            {
                                                DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                                    && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = DirectsaleOrderTaxHdrList;
                                            }
                                        }
                                    }
                                    #endregion
                                }

                                EditDirectTempSaleOrderHeaderSession = DirectsaleOrderHeaderObj;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                            }
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    SelectedTaxText = Resources.Report.Custom;
                                    txtPopupCharge.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupAmount.Text.Trim());
                                            txtPopupCharge.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    txtPopupCharge.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("OtherCharges").ToString() : (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString())) + "','600','300');", true);
                        break;
                    #endregion
                    #region TAXTYPECHANGED
                    case ActionsEnum.TAXTYPECHANGED:
                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                        {
                            dvPerc.Visible = false;
                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            TaxPK = 0;
                            if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                            {
                                string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                hdfTaxFormula.Value = taxFormula;
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupAmount.Text.Trim());
                                txtPopupCharge.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                SelectedTaxText = HttpUtility.HtmlEncode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                txtPopupCharge.Enabled = false;
                                txtPopupOther.Enabled = false;
                            }
                        }
                        else if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                        {
                            ShowOtherChargesDiv();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("OtherCharges").ToString() : (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString())) + "','600','300');", true);
                        break;
                    #endregion

                    #region ATTACHED DOCS
                    #region ADDITEM
                    case ActionsEnum.ADDITEMUPLOAD:

                        if (!string.IsNullOrEmpty(fupUpload.PostedFile.FileName))
                        {
                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                            if (!IsValidExtension(tempFileInfoObj.Extension))
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Valid_File) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                if (CurrDocSlNo != 0)
                                {
                                    if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                    {
                                        soUploadObj = SOUploadList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrDocSlNo);
                                        if (soUploadObj != null)
                                        {
                                            if (FileDetailsList == null)
                                            {
                                                FileDetailsList = new List<DirectFileDetails>();
                                            }
                                            if (fupUpload.HasFile)
                                            {

                                                tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                                string attachmentFileFormat = tempFileInfoObj.Extension;
                                                string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                                soUploadObj.AttachmentFileName = attachmentFileName;
                                                soUploadObj.FileExtension = tempFileInfoObj.Extension;
                                                soUploadObj.DOC_NAME = fupUpload.FileName;
                                                soUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                                {
                                                    soUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                                }
                                                else
                                                {
                                                    soUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;

                                                }
                                                DirectFileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrDocSlNo);
                                                if (fileDetailsObj == null)
                                                {
                                                    FileDetailsList.Add(new DirectFileDetails() { SlNo = CurrDocSlNo, SoFile = HttpContext.Current.Request.Files[0] });
                                                }
                                                else
                                                {
                                                    fileDetailsObj.SoFile = HttpContext.Current.Request.Files[0];
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (fupUpload.HasFile)
                                    {
                                        int slno = 1;
                                        if (SOUploadList == null || SOUploadList.Count == 0)
                                        {
                                            SOUploadList = new List<DirectSaleOrderUploads>();
                                            slno = 1;
                                        }
                                        else
                                        {
                                            slno = SOUploadList.Max(itm => itm.DOC_SEQ_NO);
                                            slno++;
                                        }
                                        if (FileDetailsList == null)
                                        {
                                            FileDetailsList = new List<DirectFileDetails>();
                                        }

                                        soUploadObj = new DirectSaleOrderUploads();
                                        soUploadObj.DOC_PK = 0;
                                        soUploadObj.DOC_SEQ_NO = slno;
                                        tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                        string attachmentFileFormat = tempFileInfoObj.Extension;
                                        string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                        soUploadObj.AttachmentFileName = attachmentFileName;
                                        soUploadObj.FileExtension = tempFileInfoObj.Extension;
                                        soUploadObj.DOC_NAME = fupUpload.FileName;
                                        soUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                        {
                                            soUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                        }
                                        else
                                        {
                                            soUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                        }

                                        soUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        FileDetailsList.Add(new DirectFileDetails() { SlNo = slno, SoFile = HttpContext.Current.Request.Files[0] });
                                        SOUploadList.Add(soUploadObj);
                                    }
                                }
                            }
                        }
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        ResetForm(ControlsEnum.ADDITEM);

                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEMUPLOAD:
                        if (SOUploadList != null && SOUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                SOUploadList = SOUploadList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                if (FileDetailsList != null)
                                    FileDetailsList = FileDetailsList.Where(attdoc => selectedItemPK != attdoc.SlNo).ToList();
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEMUPLOAD:
                        if (SOUploadList != null && SOUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                soUploadObj = SOUploadList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        break;
                    #endregion
                    #endregion

                    #region SAVESUBMIT POPUP
                    case ActionsEnum.SAVESUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region DELETEANDSUMBIT POPUP
                    case ActionsEnum.DELETEANDSUMBIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SUBMIT POPUP
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region WORKFLOW SUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideWkfSubmit", "ClosePopup();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                DirectsaleOrderHeaderObj = new DirectSaleOrderBO();
                                DirectsaleOrderHeaderObj = (DirectSaleOrderBO)SetUIValuesToObject(ControlsEnum.SALEORDERHEADER);

                                if (DirectsaleOrderHeaderObj != null && DirectsaleOrderHeaderObj.DirectsaleOrderDetails != null)
                                {
                                    if (DirectsaleOrderHeaderObj.TaxHdrDtl != null && DirectsaleOrderHeaderObj.TaxHdrDtl.Count > 0)
                                    {
                                        List<DirectSaleOrderTaxHdr> Templst = new List<DirectSaleOrderTaxHdr>();
                                        Templst = DirectsaleOrderHeaderObj.TaxHdrDtl.Where(x => Convert.ToDouble(x.SLT_TAX_AMT) == 0).ToList();
                                        foreach (DirectSaleOrderTaxHdr items in Templst)
                                        {
                                            DirectsaleOrderHeaderObj.TaxHdrDtl.Remove(items);
                                        }
                                    }
                                    if (DirectsaleOrderHeaderObj.DirectsaleOrderDetails == null || DirectsaleOrderHeaderObj.DirectsaleOrderDetails.Count == 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                        return;
                                    }
                                    SaveTransaction(DirectsaleOrderHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)//Cancel
                            {
                                #region Cancel Submit Codes

                                if (BusinessLogic.Sales.DirectSaleOrderBL.ValidationForCancellationSO(CurrPK))
                                {
                                    //ucrWrkf.ApplicationID = CurrPK;
                                    isCancelled = true;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_DO_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(0, 1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;

                                    this.CurrPK = base.WkfRefID = 0;
                                    this.ModifiedDatePnl.Visible = false;
                                    GetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                                    SetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                                    ResetForm(ControlsEnum.DIRECTSALEORDER);
                                    ResetForm(ControlsEnum.SALEORDERDETAIL);
                                    txtDtlRemark2.Text = string.Empty;
                                }
                                #endregion
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                        }
                        break;
                    #endregion
                    #region CANCEL S0
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdSaleOrderSearchList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDSOID")).Value);
                                grdSaleOrderSearchListRowDeptId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDepartmentID")).Value);
                                selRecordStatus = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDSOStatus")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (selRecordStatus != 0 && selRecordStatus != 4)
                            {
                                ucrWrkf.Reset();
                                FillProcessID(grdSaleOrderSearchListRowDeptId, 11);
                                SetUIEditView(commonActions);
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                                SetCancelRef(CurrPK);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                    ucrWrkf.ViewType = 1;
                                else
                                    ucrWrkf.ViewType = 0;
                                ucrWrkf.ViewAction();

                                GetFieldValues(ControlsEnum.DETAILFOREDIT);
                                SetFieldValues(ControlsEnum.DETAILFOREDIT);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_InvalidRecordForOpn").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETESUBMIT popup
                    case ActionsEnum.DELETESUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region AMEND SO
                    case ActionsEnum.AMEND:
                        foreach (GridViewRow grdrow in grdSaleOrderSearchList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            // check row selected or not
                            if (rbtn != null && rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDSOID")).Value);
                                grdSaleOrderSearchListRowDeptId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDepartmentID")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            IsCopySO = 0;
                            hdfIsForAment.Value = "1";
                            FillProcessID(grdSaleOrderSearchListRowDeptId, 2);
                            SetUIEditView(commonActions);
                            EntryStatus = EntryStatus.EDITMODE;
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.EDITMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                            GetFieldValues(ControlsEnum.DETAILFOREDIT);
                            SetFieldValues(ControlsEnum.DETAILFOREDIT);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SHORTCLOSE
                    case ActionsEnum.SHORTCLOSE:
                        CurrPK = Convert.ToInt32(((Button)sender).CommandArgument);
                        txtRemarks.Text = txtRefNo.Text = string.Empty;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowShortClose", "ShowShortClose();", true);
                        break;
                    case ActionsEnum.SHORTCLOSESAVE:
                        int? resultSC = BusinessLogic.Sales.DirectSaleOrderBL.SaveDirectSaleOrderShortClose(CurrPK, txtRemarks.Text, txtRefNo.Text, currentUser.PKUser);
                        if (resultSC > 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_ShortClose_Success").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                            GetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                            SetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                        }
                        else
                        {
                            if (resultSC == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (resultSC == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.POOpeningInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (resultSC == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.POOpeningInvoice + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (resultSC == (int)DbSaveStatus.INVOICEEXISTASDRAFT)
                            {
                                litErrorMsg.Text = Resources.Messages.Err_Msg_InvoiceExist;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalesOrder);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Print
                    case ActionsEnum.PRINTSO:
                        isSelected = false;
                        foreach (GridViewRow grdrow in grdSaleOrderSearchList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            // check row selected or not
                            if (rbtn != null && rbtn.Checked)
                            {
                                isSelected = true;
                                //When an SC(status:Draft) is selected, and on clicking 'SC No.' hyper link , 'Cancel' and 'Amend' buttons are displaying.                                   
                                gvr = (rbtn).Parent.Parent as ExtGridViewRow;
                                hdfWrkfStatus = gvr.FindControl("hdfDSOStatus") as HiddenField;
                                if (Convert.ToInt16(hdfWrkfStatus.Value) == 0)
                                {
                                    btnEditforCancel.Visible = false;
                                }
                                if (Convert.ToInt16(hdfWrkfStatus.Value) == 2 || Convert.ToInt16(hdfWrkfStatus.Value) == 12 || Convert.ToInt16(hdfWrkfStatus.Value) == 20)
                                {
                                    btnAmend.Visible = true;
                                }
                                else
                                {
                                    btnAmend.Visible = false;
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                    ((HiddenField)grdrow.FindControl("hdfDSOID")).Value + "&APPTYPE=" + ApplicationType.SOD + "&APPSUBTYPE=") + "');", true);
                                break;
                            }
                        }
                        if (!isSelected)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Revision History
                    case ActionsEnum.REVISIONHISTORY:
                        GetFieldValues(ControlsEnum.REVISIONHISTORY);
                        SetFieldValues(ControlsEnum.REVISIONHISTORY);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divRevisionHistory]','" + GetLocalResourceObject("RevisionHistory").ToString() + "','400','300');", true);
                        break;
                    #endregion

                    #region SO REPORT
                    case ActionsEnum.SHOWPOPUP:
                        sohPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + sohPK + "&APPTYPE=" + ApplicationType.SOD + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion

                    #region EDIT PRINT
                    case ActionsEnum.PRINT:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK + "&APPTYPE=" + ApplicationType.SOD + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion

                    #region TAXPOPUP CHECK CHANGED
                    case ActionsEnum.CHECKEDCHANGED:
                        if (((CheckBox)sender).ID == "chkSubTotal" || ((CheckBox)sender).ID == "chkDiscount" || ((CheckBox)sender).ID == "chkOtherCharges")
                        {
                            hdfTaxFormula.Value = string.Empty;
                            double taxable = 0;
                            if (chkSubTotal.Checked)
                            {
                                lblSubTotal = grdOrderDetails.FooterRow == null ? null : (Label)grdOrderDetails.FooterRow.FindControl("lblItemTotalAmnt");
                                taxable += lblSubTotal.Text != string.Empty ? Convert.ToDouble(lblSubTotal.Text) : 0;
                            }
                            if (chkDiscount.Checked)
                            {
                                taxable = txtHdrDiscount.Text != string.Empty ? taxable - Convert.ToDouble(txtHdrDiscount.Text) : taxable;
                            }
                            if (chkOtherCharges.Checked)
                            {
                                taxable += txtHdrOtrCharge.Text != string.Empty ? Convert.ToDouble(txtHdrOtrCharge.Text) : 0;
                            }
                            txtPopupAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    TaxPK = 0;
                                    if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                    {
                                        string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                        hdfTaxFormula.Value = taxFormula;
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupAmount.Text.Trim());
                                        txtPopupCharge.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                        SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                        txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    }
                                }
                                else
                                {
                                    SelectedTaxText = Resources.Report.Custom;
                                    txtPopupAmount.Text = string.Empty;
                                }
                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                            }

                            //SetHdrTax();
                            //SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            if (IsHeaderTaxForTradingSale && IsItemwiseTaxForTradingSale)
                                divTaxApplicableAmount.Attributes.Add("style", "display:block;");

                            else
                                divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','645','300');", true);
                        }
                        break;
                    #endregion

                    #region LineitemTax in Amount Change
                    case ActionsEnum.TOOLTIP:
                        if (hdfErrorMsgType.Value == "1")//qtyDespatched > currentQty
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Qty_Despatch").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (hdfErrorMsgType.Value == "2")//qtyInvoiced > currentQty
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Qty_Invoice").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        if ((string.IsNullOrEmpty(hdfItem.Value) ? 0 : Convert.ToInt32(hdfItem.Value)) > 0)
                        {
                            //if (hdfisItemHaveTax.Value.ToString() == "1")
                            //{
                            if (DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.Any(tm => tm.TaxDtl != null && tm.SOD_ITEM == Convert.ToInt32(hdfItem.Value)))
                            {
                                double amountItemwise;
                                amountItemwise = Convert.ToDouble(txtAmount.Text);
                                DirectsaleOrderHeaderObj = DirectTempSaleOrderHeaderSession;
                                SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                                SelectedItemCategory = string.IsNullOrEmpty(hdfItemCategory.Value) ? 0 : Convert.ToInt32(hdfItemCategory.Value);
                                SelectedItemPK = string.IsNullOrEmpty(hdfItem.Value) ? 0 : Convert.ToInt32(hdfItem.Value);
                                if (CurrSlNo > 0)
                                {
                                    DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                }
                                else
                                {
                                    DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(crt => crt.SOD_PK == SelectedDtlPK && crt.SOD_ITEM_CATEGORY == SelectedItemCategory && crt.SOD_ITEM == SelectedItemPK);
                                }
                                //If disc Available ?
                                if (DirectsaleOrderDetailsObj.TaxDtl != null)
                                {
                                    var discDetail = DirectsaleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == (int)TaxType.Discount);
                                    foreach (DirectSaleOrderTaxHdr taxHdrObj in discDetail)
                                    {
                                        string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                                        if (!string.IsNullOrEmpty(taxFormula))
                                        {
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amountItemwise.ToString());
                                            taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                        }
                                        else if (taxHdrObj.SLT_DISC_PERC > 0)
                                        {
                                            double Amount = txtAmount.Text != string.Empty ? Convert.ToDouble(txtAmount.Text) : 0;
                                            taxHdrObj.SLT_TAX_AMT = Math.Round((Amount * taxHdrObj.SLT_DISC_PERC) / 100, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                        }
                                    }
                                    //discount = saleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.SLT_TAX_AMT);
                                }
                                txtDiscount.Text = txtDiscount.ToolTip = DirectsaleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.SLT_TAX_AMT).ToString();
                                amountItemwise = Convert.ToDouble(txtAmount.Text) - Convert.ToDouble(txtDiscount.Text);
                                if (DirectsaleOrderDetailsObj.TaxDtl != null)
                                {
                                    List<DirectSaleOrderTaxHdr> taxDetail = DirectsaleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).ToList();
                                    foreach (DirectSaleOrderTaxHdr taxDtlObj in taxDetail)
                                    {
                                        string taxFormula = taxDtlObj.SLT_TAX_FORMULA;
                                        if (!string.IsNullOrEmpty(taxFormula))
                                        {
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amountItemwise.ToString());
                                            //Commented for RBPL rounding issue
                                            //taxDtlObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                            taxDtlObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                        }
                                    }
                                    if (CurrSlNo <= 0)
                                    {
                                        DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault().SOD_TAX = DirectsaleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.SLT_TAX_AMT);
                                    }

                                    //netAmount = amount - discount;

                                }
                                txtTax.Text = txtTax.ToolTip = DirectsaleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.SLT_TAX_AMT).ToString();
                                if (DirectsaleOrderDetailsObj != null)
                                    DirectsaleOrderDetailsObj.SOD_AMOUNT = amountItemwise + Convert.ToDouble(txtTax.Text);
                                ResetForm(ControlsEnum.TAXPOPUPGRID);
                                IsHeaderTax = false;
                                IsEditMode = false;
                            }
                            //}
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_Item").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateBoxQty", "ClosePopup();CalculateBoxQty();", true);
                        if (hdfFocusPdctAdd.Value == "1")
                            ddlUOM.Focus(); //txtReqByDate.Focus();
                        else if (hdfFocusPdctAdd.Value == "2")
                            imgDiscountDtl.Focus();
                        break;
                    #endregion

                    #region ITEMCATEGORYSELECTED
                    case ActionsEnum.ITEMCATEGORYSELECTED:
                        SetItemCategory();
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }


        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

    
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
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }
                PageIndexList = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                SetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            }
        }

        #region Grid Actions

        /// <summary>
        /// To get Footer details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            Label lblItemTotalQty;
            Label lblItemTotalmount;
            Label lblItemTotalDiscount;
            Label lblItemTotalTax;
            Label lblItemTotalAmnt;

            try
            {
                if ((sender as GridView).ID == "grdSaleOrderSearchList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {

                        HiddenField hdfDSOStatus = e.Row.FindControl("hdfDSOStatus") as HiddenField;
                        Button imgShortClose = e.Row.FindControl("imgShortClose") as Button;
                        imgShortClose.Style.Add("margin-bottom", "2px");

                        short appstatus = Convert.ToInt16(hdfDSOStatus.Value);
                        #region Short closing Button visibility
                        int saleContractStatus = appstatus;
                        if (HasShortCloseRight == true && saleContractStatus != 0 && saleContractStatus != 4 && saleContractStatus != 104 && saleContractStatus != 3)
                        {
                            imgShortClose.CssClass = GetLocalResourceObject("ShortClosing").ToString();
                            imgShortClose.Visible = true;
                        }
                        else
                            imgShortClose.Visible = false;
                        #endregion
                    }
                }
                else if ((sender as GridView).ID == "grdOrderDetails")
                {
                    if (e.Row.RowType == DataControlRowType.Footer && DirectsaleOrderDetailsList != null)
                    {
                        lblItemTotalQty = e.Row.FindControl("lblItemTotalQty") as Label;
                        lblItemTotalmount = e.Row.FindControl("lblItemTotalmount") as Label;
                        lblItemTotalDiscount = e.Row.FindControl("lblItemTotalDiscount") as Label;
                        lblItemTotalTax = e.Row.FindControl("lblItemTotalTax") as Label;
                        lblItemTotalAmnt = e.Row.FindControl("lblItemTotalAmnt") as Label;

                        lblItemTotalQty.Text = lblItemTotalQty.ToolTip = GetFormattedNumberWithComma(DirectsaleOrderDetailsList.Sum(itm => itm.SOD_QTY));
                        lblItemTotalmount.Text = lblItemTotalQty.ToolTip = GetFormattedNumberWithComma(DirectsaleOrderDetailsList.Sum(itm => itm.SOD_AMOUNT));
                        lblItemTotalDiscount.Text = lblItemTotalQty.ToolTip = GetFormattedNumberWithComma(DirectsaleOrderDetailsList.Sum(itm => itm.SOD_DISCOUNT));
                        lblItemTotalTax.Text = lblItemTotalQty.ToolTip = GetFormattedNumberWithComma(DirectsaleOrderDetailsList.Sum(itm => itm.SOD_TAX));
                        lblItemTotalAmnt.Text = lblItemTotalAmnt.ToolTip = GetFormattedNumberWithComma(DirectsaleOrderDetailsList.Sum(itm => itm.SOD_NET_AMOUNT));
                    }
                }
                else if (((GridView)sender).ID == "grdUploads")
                {
                    int slno;
                    if (EntryStatus == EntryStatus.VIEWMODE)
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                        {
                            e.Row.Cells[3].Visible = false;
                            e.Row.Cells[4].Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        slno = Convert.ToInt32(grdUploads.DataKeys[e.Row.RowIndex][0]);
                        if (slno > 0)
                        {
                            e.Row.FindControl("fileView").Visible = FileDetailsList == null || FileDetailsList.Where(fle => fle.SlNo == slno).Count() == 0;
                        }
                    }
                }
                else if ((sender as GridView).ID == "grdRevisionHistory")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //(e.Row.FindControl("lnkRevisionPrint") as LinkButton).PostBackUrl = "../Reports/GenerateReport.aspx?ID=" + DataBinder.Eval(e.Row.DataItem, "CEH_PK").ToString() + "&RevID=" + DataBinder.Eval(e.Row.DataItem, "CEH_VERSION").ToString() + "&APPTYPE=" + BusinessObject.CommonManagement.ApplicationType.CQTN + "&APPSUBTYPE=";
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Attributes.Add("OnClick", "javascript:return OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + DataBinder.Eval(e.Row.DataItem, "SOH_PK").ToString() + "&RevID=" + DataBinder.Eval(e.Row.DataItem, "SOH_VERSION").ToString() + "&APPTYPE=" + BusinessObject.CommonManagement.ApplicationType.SOD + "&APPSUBTYPE=" + "');");
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Attributes.Add("href", "javascript:void(0);");
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Text = DataBinder.Eval(e.Row.DataItem, "SOH_NO").ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gridview Page navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
        }

        #endregion

        #endregion

        #region Get Field Values

        private void GetFieldValues(ControlsEnum type)
        {
            AdmCompanyMstService admCompanyMstServiceClient;
            BusinessObject.GridPrams gridParam;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    case ControlsEnum.DIRECTSALEORDERLIST:
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        gridParam.FromDate = txtFromDate.Text;
                        gridParam.ToDate = txtToDate.Text;
                        gridParam.FilterStatus = ddlStatus.SelectedValue == "-1" ? string.Empty : ddlStatus.SelectedValue;
                        string dsoNumber = txtDSONumber.Text.Trim() == "Select/Type" ? string.Empty : txtDSONumber.Text.Trim();
                        string customerPK = txtAdvCustomerSrch.Text.Trim() == "Select/Type" || txtAdvCustomerSrch.Text.Trim() == string.Empty ? "0" : hdfAdvCustomerPKSrch.Value;
                        if (dsoNumber != string.Empty)
                        {
                            txtFromDate.Text = txtToDate.Text = string.Empty;
                            ddlStatus.SelectedValue = "-1";
                            txtAdvCustomerSrch.Text = Resources.ErpRes.AutoDefaultValue;
                        }
                        string SOType = ddlPortType.SelectedValue == "-1" ? string.Empty : ddlPortType.SelectedValue;
                        dsPageData = BusinessLogic.Sales.DirectSaleOrderBL.GetDirectSOGetList(gridParam, currentUser, dsoNumber, Convert.ToInt32(customerPK), SOType);
                        break;
                    case ControlsEnum.DETAILFOREDIT:
                        string xmlData = BusinessLogic.Sales.DirectSaleOrderBL.GetDirectSaleOrderByPk(CurrPK);
                        DirectSaleOrderBO tempDirectSO = CommonFunctions.XmlDeserialize<DirectSaleOrderBO>(xmlData);
                        DirectSaleOrderHeaderSession = tempDirectSO;
                        DirectTempSaleOrderHeaderSession = DirectSaleOrderHeaderSession;
                        break;
                    case ControlsEnum.SODETAILS:
                        dtPageData = new DataTable();
                        dtPageData = BusinessLogic.Sales.DirectSaleOrderBL.GetExtDirectSaleOrderDetailList(grdSOHpk);
                        break;
                    case ControlsEnum.SOTYPE:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO TYPE");
                        break;
                    case ControlsEnum.CUSTOMER:
                        dsPageData = BusinessLogic.Sales.CustomerProduct.GetCustomer(custPK, string.Empty, currentUser.SBUID, 2);
                        break;
                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtSaleOrderDate.Text.Trim()));
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            exchangeRate = string.IsNullOrEmpty(dsExchangeRate.Tables[0].Rows[0][0].ToString()) ? -1 :
                                Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0].ToString());
                        }
                        else
                        {
                            exchangeRate = -1;
                        }
                        break;
                    case ControlsEnum.UOM:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetUOM(Convert.ToInt32(hdfItem.Value), Convert.ToInt32(CommonConstants.ACTIVE));
                        break;
                    case ControlsEnum.TAXTYPES:
                        int category = 1;
                        int.TryParse(hdfTaxCategory.Value, out category);
                        if (TaxPK > 0)
                        {
                            dsSaleOrderTaxDetails = BusinessLogic.Sales.QuotationBL.GetQuotationTaxDetails(TaxPK, category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.HASPK));
                            if (dsSaleOrderTaxDetails != null && dsSaleOrderTaxDetails.Tables.Count > 0)
                            {
                                dtSaleOrderTaxDetails = dsSaleOrderTaxDetails.Tables[0];
                            }
                        }
                        else
                        {
                            if ((int)TaxType.Shipping == category)//shipping is not under sales or Purchase
                            {
                                dtSaleOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, (IsTaxInSBU == true ? 0 : currentUser.SBUID), Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtSaleOrderDate.Text), 0, TaxFilterType.SAL, 1);
                            }
                            else
                            {
                                if ((int)TaxType.Discount != category)
                                {
                                    dtSaleOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, (IsTaxInSBU == true ? 0 : currentUser.SBUID), Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtSaleOrderDate.Text), 0, TaxFilterType.SAL, 1, 0, 1);
                                }
                                else
                                {
                                    dtSaleOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, (IsTaxInSBU == true ? 0 : currentUser.SBUID), Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtSaleOrderDate.Text), 0);
                                }
                            }
                        }
                        break;
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    case ControlsEnum.REVISIONHISTORY:
                        dsPageData = BusinessLogic.Sales.SaleOrderBL.GetRevisionHistory(CurrPK);
                        break;
                    case ControlsEnum.AUTOTAXPOPUP:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetCustomerSupplyTax(1, currentUser.SBUID, Convert.ToDateTime(txtSaleOrderDate.Text), Convert.ToInt16(hdfCustomer.Value), 0);
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            IsHeaderTax = true;
                        }
                        break;
                    case ControlsEnum.SUBCATEGORY:
                        dtSubCategory = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PRODUCT SUB TYPE");
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    case ControlsEnum.SOTYPE:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.UOM:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SALEORDERDETAIL:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        BindGrid(ControlsEnum.TAXPOPUPGRID);
                        break;
                    case ControlsEnum.TAXTYPES:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        if (DirectsaleOrderHeaderObj != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    case ControlsEnum.COMPANY:
                        BindDropDownList(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.DIRECTSALEORDERLIST:
                        BindGrid(ControlsEnum.DIRECTSALEORDERLIST);
                        break;
                    case ControlsEnum.DETAILFOREDIT:
                        GetUIValuesFromObject(ControlsEnum.DETAILFOREDIT);
                        break;
                    case ControlsEnum.REVISIONHISTORY:
                        BindGrid(controlType);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    case ControlsEnum.AUTOTAXPOPUP://For HeaderTax
                        DirectSaleOrderTaxHdr saleOrderTaxHdrObj;
                        List<DirectSaleOrderTaxHdr> saleOrderTaxHdrList;
                        saleOrderTaxHdrList = new List<DirectSaleOrderTaxHdr>();
                        DirectsaleOrderHeaderObj = DirectTempSaleOrderHeaderSession;
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            hdfisItemHaveTax.Value = "1";//Item Have Tax
                            for (int i = 0; i < dtPageData.Rows.Count; i++)
                            {
                                saleOrderTaxHdrObj = new DirectSaleOrderTaxHdr();
                                saleOrderTaxHdrObj.SLT_NAME = dtPageData.Rows[i]["CMT_TAX_TEXT"].ToString();
                                saleOrderTaxHdrObj.SLT_TAX = Convert.ToInt16(dtPageData.Rows[i]["TAX_PK"]);
                                //saleOrderTaxHdrObj.SLT_DIS_PERC = 0;
                                saleOrderTaxHdrObj.SLT_TAX_AMT = 0;
                                saleOrderTaxHdrObj.SLT_TAX_CATEGORY = Convert.ToInt16(dtPageData.Rows[i]["TAX_CATEGORY"]);
                                saleOrderTaxHdrObj.SLT_TAX_FORMULA = dtPageData.Rows[i]["TAX_FORMULA"].ToString();
                                saleOrderTaxHdrObj.SLT_TAX_TEXT = dtPageData.Rows[i]["TAX_HEAD"].ToString();
                                if (IsItemwiseTaxForTradingSale && IsHeaderTaxForTradingSale)
                                    saleOrderTaxHdrObj.SLT_HAS_OTHER_CHARGE = 1;

                                saleOrderTaxHdrList.Add(saleOrderTaxHdrObj);
                            }

                            DirectsaleOrderHeaderObj.TaxHdrDtl = saleOrderTaxHdrList;
                            txtHdrTax.Text = "0.00";
                            txtTax.Text = "0.00";
                            txtDiscount.Text = "0.00";

                            DirectTempSaleOrderHeaderSession = DirectsaleOrderHeaderObj;

                        }
                        else
                        {
                            hdfisItemHaveTax.Value = "0";//ItemHave no Tax
                            DirectsaleOrderHeaderObj = DirectTempSaleOrderHeaderSession;
                            saleOrderTaxHdrList = DirectsaleOrderHeaderObj.TaxHdrDtl.ToList();
                        }
                        DirectSaleOrderHeaderSession = DirectTempSaleOrderHeaderSession;
                        break;
                    case ControlsEnum.AUTOTAXPOPUPITEMWISE:
                        DirectSaleOrderTaxHdr saleOrderTaxDtlObj;
                        List<DirectSaleOrderTaxHdr> saleOrderTaxDtlList;
                        saleOrderTaxDtlList = new List<DirectSaleOrderTaxHdr>();
                        DirectsaleOrderHeaderObj = DirectTempSaleOrderHeaderSession;
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            hdfisItemHaveTax.Value = "1";//Item Have Tax
                            if (IsItemwiseTaxForTradingSale && hdfItem.Value != string.Empty && Convert.ToInt32(hdfItem.Value) > 0)
                            {
                                DirectsaleOrderDetailsList = DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails;
                                hdfIsItemYes.Value = "1"; IsItem = true;
                                DirectsaleOrderDetailsList = (List<DirectSaleOrderDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails = DirectsaleOrderDetailsList;
                                if (Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "EnableCustomerTaxforSChdrDtl")) != 1)
                                    saleOrderTaxDtlList = DirectsaleOrderDetailsObj.TaxDtl == null ? new List<DirectSaleOrderTaxHdr>() : DirectsaleOrderDetailsObj.TaxDtl.ToList();
                                else
                                    saleOrderTaxDtlList = new List<DirectSaleOrderTaxHdr>();
                                hdfIsItemYes.Value = "0"; IsItem = false;
                                if (CurrSlNo > 0)
                                {
                                    DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = null;
                                }
                            }
                            for (int i = 0; i < dtPageData.Rows.Count; i++)
                            {
                                saleOrderTaxDtlObj = new DirectSaleOrderTaxHdr();
                                saleOrderTaxDtlObj.SLT_NAME = dtPageData.Rows[i]["CMT_TAX_TEXT"].ToString();
                                saleOrderTaxDtlObj.SLT_TAX = Convert.ToInt16(dtPageData.Rows[i]["TAX_PK"]);
                                //saleOrderTaxHdrObj.SLT_DIS_PERC = 0;
                                saleOrderTaxDtlObj.SLT_TAX_AMT = 0;
                                saleOrderTaxDtlObj.SLT_TAX_CATEGORY = Convert.ToInt16(dtPageData.Rows[i]["TAX_CATEGORY"]);
                                saleOrderTaxDtlObj.SLT_TAX_FORMULA = dtPageData.Rows[i]["TAX_FORMULA"].ToString();
                                saleOrderTaxDtlObj.SLT_TAX_TEXT = dtPageData.Rows[i]["TAX_HEAD"].ToString();

                                saleOrderTaxDtlList.Add(saleOrderTaxDtlObj);
                            }

                            if (hdfItem.Value != string.Empty && Convert.ToInt32(hdfItem.Value) > 0)
                            {
                                SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                                SelectedItemPK = string.IsNullOrEmpty(hdfItem.Value) ? 0 : Convert.ToInt32(hdfItem.Value);
                                SelectedItemCategory = string.IsNullOrEmpty(hdfItemCategory.Value) ? 0 : Convert.ToInt32(hdfItemCategory.Value);
                                if (CurrSlNo > 0)
                                {
                                    DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = saleOrderTaxDtlList;
                                }
                                else
                                {
                                    DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                    && rfq.SOD_ITEM_CATEGORY == SelectedItemCategory && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = saleOrderTaxDtlList;
                                }
                            }
                        }
                        else
                        {
                            hdfisItemHaveTax.Value = "0";//ItemHave no Tax
                            DirectsaleOrderDetailsList = DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails;
                            hdfIsItemYes.Value = "1"; IsItem = true;
                            DirectsaleOrderDetailsList = (List<DirectSaleOrderDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                            DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails = DirectsaleOrderDetailsList;
                            hdfIsItemYes.Value = "0"; IsItem = false;
                            DirectsaleOrderHeaderObj = DirectTempSaleOrderHeaderSession;
                            if (CurrSlNo > 0)
                            {
                                if (DirectsaleOrderHeaderObj.DirectsaleOrderDetails.Count(p => p.SOD_SL_NO == CurrSlNo) > 0)
                                    saleOrderTaxHdrList = DirectsaleOrderHeaderObj.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = null;
                                else
                                    saleOrderTaxHdrList = null;
                                txtTax.Text = "0.00";
                                txtDiscount.Text = "0.00";
                            }
                            else
                            {
                                saleOrderTaxHdrList = null;// saleOrderHeaderObj.SaleContractDetails.Where(rfq => rfq.SOD_PK == SelectedDtlPK 
                            }
                        }
                        DirectTempSaleOrderHeaderSession = DirectsaleOrderHeaderObj;
                        break;
                    case ControlsEnum.AUTODISCOUNTPOPUP://For Detail Discount
                        SelectedTaxText = Resources.Controls.Custom;
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                        SelectedItemPK = string.IsNullOrEmpty(hdfItem.Value) ? 0 : Convert.ToInt32(hdfItem.Value);
                        DirectSaleOrderTaxHdr saleOrderDiscDtlObj;
                        DirectsaleOrderHeaderObj = DirectTempSaleOrderHeaderSession;

                        if (IsItemwiseDiscountForTradingSale && hdfItem.Value != string.Empty && Convert.ToInt32(hdfItem.Value) > 0)
                        {
                            DirectsaleOrderDetailsList = DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails;
                            if ((DirectsaleOrderDetailsList.Where(itm => (itm.SOD_SL_NO == CurrSlNo) && itm.SOD_ITEM == Convert.ToInt32(hdfItem.Value)).Count() == 0))
                            {
                                DirectsaleOrderDetailsList = (List<DirectSaleOrderDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                DirectTempSaleOrderHeaderSession.DirectsaleOrderDetails = DirectsaleOrderDetailsList;
                            }
                        }

                        saleOrderDiscDtlObj = new DirectSaleOrderTaxHdr();
                        saleOrderDiscDtlObj.SLT_PK = 0;
                        saleOrderDiscDtlObj.SLT_SO_DTL = SelectedDtlPK;
                        saleOrderDiscDtlObj.SLT_SL_NO = 1;
                        saleOrderDiscDtlObj.SLT_TAX_TEXT = HttpUtility.HtmlEncode(SelectedTaxText);
                        saleOrderDiscDtlObj.SLT_NAME = HttpUtility.HtmlEncode(SelectedTaxText);
                        saleOrderDiscDtlObj.SLT_DISC_PERC = hdfCusDiscPerc.Value == "" || Convert.ToDouble(hdfCusDiscPerc.Value) < 0 ? 0 : Convert.ToDouble(hdfCusDiscPerc.Value);
                        hdfCustomDiscPerc.Value = hdfCusDiscPerc.Value;
                        saleOrderDiscDtlObj.SLT_TAX_AMT = 0;
                        saleOrderDiscDtlObj.SLT_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                        saleOrderDiscDtlObj.SLT_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                        saleOrderDiscDtlObj.SLT_TYPE = 1;
                        if (hdfItem.Value != string.Empty && Convert.ToInt32(hdfItem.Value) > 0)
                        {
                            SelectedItemCategory = string.IsNullOrEmpty(hdfItemCategory.Value) ? 0 : Convert.ToInt32(hdfItemCategory.Value);
                            if (CurrSlNo > 0)
                            {
                                if (DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl == null)
                                    DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = new List<DirectSaleOrderTaxHdr>();
                                DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl.Add(saleOrderDiscDtlObj);
                            }
                            else
                            {
                                if (DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK && rfq.SOD_ITEM_CATEGORY == SelectedItemCategory && rfq.SOD_ITEM == SelectedItemPK).TaxDtl == null)
                                    DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK && rfq.SOD_ITEM_CATEGORY == SelectedItemCategory && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = new List<DirectSaleOrderTaxHdr>();

                                DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                && rfq.SOD_ITEM_CATEGORY == SelectedItemCategory && rfq.SOD_ITEM == SelectedItemPK).TaxDtl.Add(saleOrderDiscDtlObj);
                            }
                        }
                        DirectTempSaleOrderHeaderSession = DirectsaleOrderHeaderObj;
                        break;
                    case ControlsEnum.SUBCATEGORY:
                        BindDropDownList(ControlsEnum.SUBCATEGORY);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region Direct Sale Order Header
                    case ControlsEnum.SALEORDERHEADER:
                        if (DirectSaleOrderHeaderSession != null)
                        {
                            DirectsaleOrderHeaderObj = DirectSaleOrderHeaderSession;
                            if (IsCopySO == 1 && CurrPK > 0)
                                DirectsaleOrderHeaderObj.SOH_PK = CurrPK = 0;
                            else
                                DirectsaleOrderHeaderObj.SOH_PK = CurrPK;

                            if (CurrPK == 0)
                                DirectsaleOrderHeaderObj.SOH_STATUS = 0;
                            DirectsaleOrderHeaderObj.SOH_NO = string.IsNullOrEmpty(lblSaleOrderNo.Text.Trim())
                                 || lblSaleOrderNo.Text.Trim() == Resources.Messages.DocGenerationNew
                                ? string.Empty : lblSaleOrderNo.Text.Trim();
                            DirectsaleOrderHeaderObj.SOH_DATE = Convert.ToDateTime(txtSaleOrderDate.Text.Trim());
                            DirectsaleOrderHeaderObj.SOH_CUSTOMER = Convert.ToInt32(hdfCustomer.Value);
                            DirectsaleOrderHeaderObj.SOH_CUSTOMER_NAME = HttpUtility.HtmlEncode(txtCustomer.Text);

                            DirectsaleOrderHeaderObj.SOH_CUSTOMER_ADDRESS = HttpUtility.HtmlEncode(txtCusAddress.Text); //HttpUtility.HtmlEncode(hdfCusAddress.Value);
                            if (hdfCusCountry.Value.Trim() != string.Empty)
                                DirectsaleOrderHeaderObj.SOH_CUSTOMER_COUNTRY = Convert.ToInt16(hdfCusCountry.Value.Trim());
                            //if (hdfCusCountryText.Value.Trim() != string.Empty)
                            //    DirectsaleOrderHeaderObj.SOH_CUSTOMER_COUNTRY_TEXT = HttpUtility.HtmlEncode(hdfCusCountryText.Value);
                            if (hdfCusEmail.Value.Trim() != string.Empty)
                                DirectsaleOrderHeaderObj.SOH_CUSTOMER_EMAIL = HttpUtility.HtmlEncode(hdfCusEmail.Value);
                            if (hdfCusFax.Value.Trim() != string.Empty)
                                DirectsaleOrderHeaderObj.SOH_CUSTOMER_FAX = HttpUtility.HtmlEncode(hdfCusFax.Value);
                            if (hdfCusMobile.Value.Trim() != string.Empty)
                                DirectsaleOrderHeaderObj.SOH_CUSTOMER_MOBILE = HttpUtility.HtmlEncode(hdfCusMobile.Value);
                            if (hdfCusPhone.Value.Trim() != string.Empty)
                                DirectsaleOrderHeaderObj.SOH_CUSTOMER_PHONE = HttpUtility.HtmlEncode(hdfCusPhone.Value);
                            if (hdfCusZip.Value.Trim() != string.Empty)
                                DirectsaleOrderHeaderObj.SOH_CUSTOMER_ZIP = HttpUtility.HtmlEncode(hdfCusZip.Value);

                            if (!string.IsNullOrEmpty(txtCustomerRef.Text))
                                DirectsaleOrderHeaderObj.SOH_REFERENCE = HttpUtility.HtmlEncode(txtCustomerRef.Text);
                            if (!string.IsNullOrEmpty(txtDeliveryDate.Text))
                                DirectsaleOrderHeaderObj.SOH_DELIVERY_DATE = Convert.ToDateTime(txtDeliveryDate.Text.Trim());
                            DirectsaleOrderHeaderObj.SOH_TYPE = Convert.ToInt32(ddlSOType.SelectedValue);
                            DirectsaleOrderHeaderObj.SOH_CURRENCY = string.IsNullOrEmpty(hdfCurrency.Value) ? 0 : Convert.ToInt32(hdfCurrency.Value);
                            DirectsaleOrderHeaderObj.SOH_CURRENCY_BC = currentUser.BaseCurrency;
                            DirectsaleOrderHeaderObj.SOH_CURRENCY_RATE = string.IsNullOrEmpty(txtExchangeRate.Text) ? 1 : Convert.ToDouble(txtExchangeRate.Text);

                            DirectsaleOrderHeaderObj.SOH_TOTAL_DISCOUNT = string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDiscount.Text.Trim());
                            DirectsaleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE = string.IsNullOrEmpty(txtHdrOtrCharge.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrOtrCharge.Text.Trim());
                            DirectsaleOrderHeaderObj.SOH_TOTAL_TAX = string.IsNullOrEmpty(txtHdrTax.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTax.Text.Trim());
                            DirectsaleOrderHeaderObj.SOH_TOTAL_ADJUST = string.IsNullOrEmpty(txtHdrPriceAdj.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrPriceAdj.Text.Trim());
                            DirectsaleOrderHeaderObj.SOH_NET_AMOUNT = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTotal.Text.Trim());

                            DirectsaleOrderHeaderObj.SOH_NET_AMOUNT_BC = DirectsaleOrderHeaderObj.SOH_NET_AMOUNT * DirectsaleOrderHeaderObj.SOH_CURRENCY_RATE;
                            DirectsaleOrderHeaderObj.SOH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            DirectsaleOrderHeaderObj.SOH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                            DirectsaleOrderHeaderObj.SOH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            DirectsaleOrderHeaderObj.ACTIVE = DirectsaleOrderHeaderObj.SOH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                            DirectsaleOrderHeaderObj.USER_PK = currentUser.PKUser;
                            DirectsaleOrderHeaderObj.LAST_MOD_DT = LastModifiedTime;
                            DirectsaleOrderHeaderObj.APT_CODE = ApplicationType.SOD;
                            DirectsaleOrderHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                            DirectsaleOrderHeaderObj.WKF_FLAG = 0;
                            DirectsaleOrderHeaderObj.WKF_PROCESS = ProcessID;

                            if (hdfIsForAment.Value != null && hdfIsForAment.Value == "1")
                            {
                                DirectsaleOrderHeaderObj.SOH_AMEND_DATE = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                                DirectsaleOrderHeaderObj.SOH_IS_AMEND = 1;
                            }
                            else
                            {
                                DirectsaleOrderHeaderObj.SOH_AMEND_DATE = string.IsNullOrEmpty(txtAmendDate.Text) ? string.Empty : txtAmendDate.Text;
                            }

                            DirectsaleOrderHeaderObj.SOH_TRX_TYPE = Convert.ToInt32(TrxType.DirectSaleOrder);

                            if (commonActions == ActionsEnum.SAVE)
                            {
                                DirectsaleOrderHeaderObj.WKF_FLAG = 0;
                            }
                            else if (commonActions == ActionsEnum.WRKFSUBMIT)
                            {
                                DirectsaleOrderHeaderObj.WKF_FLAG = 1;
                            }

                            DirectsaleOrderHeaderObj.FileList = SOUploadList;

                            retObject = DirectsaleOrderHeaderObj;

                        }
                        break;
                    #endregion
                    #region Sale Order Details
                    case ControlsEnum.SALEORDERDETAIL:
                        DirectsaleOrderDetailsObj = DirectsaleOrderDetailsList.SingleOrDefault(itm => itm.SOD_SL_NO == CurrSlNo);
                        if (CurrSlNo != 0 && DirectsaleOrderDetailsObj != null)
                        {
                            DirectsaleOrderDetailsObj = DirectsaleOrderDetailsList.SingleOrDefault(itm => itm.SOD_SL_NO == CurrSlNo);
                            if ((DirectsaleOrderDetailsList.Where(itm => (itm.SOD_SL_NO != CurrSlNo) && itm.SOD_ITEM == Convert.ToInt32(hdfItem.Value)).Count() == 0) || (IsItem == true && (hdfIsItemYes.Value == "1")))
                            {
                                IsItem = false;
                                if (DirectsaleOrderDetailsObj != null)
                                {
                                    if (IsCopySO == 1)
                                        DirectsaleOrderDetailsObj.SOD_PK = 0;
                                    else
                                        DirectsaleOrderDetailsObj.SOD_PK = Convert.ToInt32(hdfDetailPK.Value);

                                    DirectsaleOrderDetailsObj.SOD_ITEM_CATEGORY = Convert.ToInt32(hdfItemCategory.Value);
                                    DirectsaleOrderDetailsObj.SOD_ITEM_CATEGORY_TEXT = HttpUtility.HtmlEncode(txtItemCategory.Text);
                                    DirectsaleOrderDetailsObj.SOD_ITEM_SUB_TYPE = Convert.ToInt32(ddlProdSubCategory.SelectedValue);
                                    if (!string.IsNullOrEmpty(hdfPackingSpec.Value) && Convert.ToInt32(hdfPackingSpec.Value) > 0)
                                    {
                                        DirectsaleOrderDetailsObj.SOD_PACK_SPEC = Convert.ToInt32(hdfPackingSpec.Value);
                                        DirectsaleOrderDetailsObj.SOD_PACK_SPEC_NAME = HttpUtility.HtmlEncode(txtPackingSpec.Text);
                                    }
                                    DirectsaleOrderDetailsObj.SOD_ITEM = Convert.ToInt32(hdfItem.Value);
                                    DirectsaleOrderDetailsObj.SOD_ITEM_TEXT = HttpUtility.HtmlEncode(txtItem.Text);

                                    DirectsaleOrderDetailsObj.SOD_QTY = Math.Round(Convert.ToDouble(txtItemQuantity.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                    DirectsaleOrderDetailsObj.SOD_UOM = Convert.ToInt32(ddlUOM.SelectedValue);
                                    DirectsaleOrderDetailsObj.SOD_UOM_TEXT = HttpUtility.HtmlEncode(ddlUOM.SelectedItem.Text);

                                    DirectsaleOrderDetailsObj.SOD_RATE = string.IsNullOrEmpty(txtUnitPrice.Text) ? 0 : Math.Round(Convert.ToDouble(txtUnitPrice.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));

                                    DirectsaleOrderDetailsObj.SOD_DISCOUNT = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Math.Round(Convert.ToDouble(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    if (!string.IsNullOrEmpty(txtDiscount.Text) && Convert.ToDouble(txtDiscount.Text) > 0)
                                        DirectsaleOrderDetailsObj.SOD_DISC_PERC = string.IsNullOrEmpty(hdfCustomDiscPerc.Value) ? 0 : Convert.ToDouble(hdfCustomDiscPerc.Value);
                                    else
                                        DirectsaleOrderDetailsObj.SOD_DISC_PERC = 0;
                                    DirectsaleOrderDetailsObj.SOD_TAX = string.IsNullOrEmpty(txtTax.Text) ? 0 : Math.Round(Convert.ToDouble(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                    if (!string.IsNullOrEmpty(txtReqDate.Text))
                                        DirectsaleOrderDetailsObj.SOD_REQUIRED_BY = txtReqDate.Text;

                                    DirectsaleOrderDetailsObj.SOD_AMOUNT = string.IsNullOrEmpty(txtAmount.Text) ? 0 : Math.Round(Convert.ToDouble(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    DirectsaleOrderDetailsObj.SOD_NET_AMOUNT = string.IsNullOrEmpty(txtTotalAmt.Text) ? 0 : Math.Round(Convert.ToDouble(txtTotalAmt.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                    DirectsaleOrderDetailsObj.SOD_REMARKS = HttpUtility.HtmlEncode(txtDtlRemark.Text);
                                    DirectsaleOrderDetailsObj.SOD_REMARKS2 = HttpUtility.HtmlEncode(txtDtlRemark2.Text);

                                    if (DirectsaleOrderDetailsObj.TaxDtl != null)
                                    {
                                        if (DirectsaleOrderDetailsObj.TaxDtl.Count > 0)
                                        {
                                            foreach (var taxitem in DirectsaleOrderDetailsObj.TaxDtl)
                                            {
                                                taxitem.SLT_SL_NO = DirectsaleOrderDetailsObj.SOD_SL_NO;
                                                if (IsCopySO == 1)
                                                {
                                                    taxitem.SLT_PK = 0;
                                                }
                                            }
                                        }

                                    }
                                    if (chkAddtlRemarkToAll.Checked)  //update additional remark for all brands 
                                    {
                                        if (DirectsaleOrderDetailsList != null)
                                        {
                                            if (DirectsaleOrderDetailsList.Count > 0)
                                            {
                                                DirectsaleOrderDetailsList.ForEach(lst => lst.SOD_REMARKS2 = DirectsaleOrderDetailsObj.SOD_REMARKS2);
                                            }
                                        }
                                    }

                                    if (chkRemarkToAll.Checked)  //update remark for all brands 
                                    {
                                        if (DirectsaleOrderDetailsList != null)
                                        {
                                            if (DirectsaleOrderDetailsList.Count > 0)
                                            {
                                                DirectsaleOrderDetailsList.ForEach(lst => lst.SOD_REMARKS = DirectsaleOrderDetailsObj.SOD_REMARKS);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                IsItem = true;
                                litErrorMsg.Text = GetLocalResourceObject("ItemExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                ResetForm(ControlsEnum.SALEORDERDETAIL);
                                return retObject;
                            }
                        }
                        else
                        {
                            int slno = 1;
                            if (DirectsaleOrderDetailsList == null || DirectsaleOrderDetailsList.Count == 0)
                            {
                                DirectsaleOrderDetailsList = new List<DirectSaleOrderDetailsBO>();
                                slno = 1;
                            }
                            else
                            {
                                slno = DirectsaleOrderDetailsList.Max(itm => itm.SOD_SL_NO);
                                slno++;
                            }

                            if ((DirectsaleOrderDetailsList.Where(itm => (itm.SOD_SL_NO != CurrSlNo) && itm.SOD_ITEM == Convert.ToInt32(hdfItem.Value)).Count() == 0) || (IsItem == true && (hdfIsItemYes.Value == "1")))
                            {
                                IsItem = false;
                                DirectsaleOrderDetailsObj = new DirectSaleOrderDetailsBO();

                                DirectsaleOrderDetailsObj.SOD_SL_NO = slno;
                                CurrSlNo = DirectsaleOrderDetailsObj.SOD_SL_NO;

                                DirectsaleOrderDetailsObj.SOD_ITEM_CATEGORY = Convert.ToInt32(hdfItemCategory.Value);
                                DirectsaleOrderDetailsObj.SOD_ITEM_CATEGORY_TEXT = HttpUtility.HtmlDecode(txtItemCategory.Text);
                                DirectsaleOrderDetailsObj.SOD_ITEM = Convert.ToInt32(hdfItem.Value);
                                DirectsaleOrderDetailsObj.SOD_ITEM_TEXT = HttpUtility.HtmlEncode(txtItem.Text);

                                DirectsaleOrderDetailsObj.SOD_QTY = Math.Round(Convert.ToDouble(txtItemQuantity.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                DirectsaleOrderDetailsObj.SOD_UOM = Convert.ToInt32(ddlUOM.SelectedValue);
                                DirectsaleOrderDetailsObj.SOD_UOM_TEXT = HttpUtility.HtmlEncode(ddlUOM.SelectedItem.Text);

                                DirectsaleOrderDetailsObj.SOD_RATE = Math.Round(Convert.ToDouble(txtUnitPrice.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));

                                DirectsaleOrderDetailsObj.SOD_DISCOUNT = Math.Round(string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? 0 :
                                    Convert.ToDouble(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                DirectsaleOrderDetailsObj.SOD_TAX = Math.Round(string.IsNullOrEmpty(txtTax.Text.Trim()) ? 0 :
                                    Convert.ToDouble(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                if (!string.IsNullOrEmpty(txtReqDate.Text))
                                    DirectsaleOrderDetailsObj.SOD_REQUIRED_BY = txtReqDate.Text;

                                DirectsaleOrderDetailsObj.SOD_AMOUNT = Math.Round(Convert.ToDouble(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                DirectsaleOrderDetailsObj.SOD_NET_AMOUNT = Math.Round(Convert.ToDouble(txtTotalAmt.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);

                                DirectsaleOrderDetailsObj.SOD_REMARKS = HttpUtility.HtmlEncode(txtDtlRemark.Text);
                                DirectsaleOrderDetailsObj.SOD_REMARKS2 = HttpUtility.HtmlEncode(txtDtlRemark2.Text);

                                if (tempsaleOrderTaxHdrList != null)
                                {
                                    DirectsaleOrderDetailsObj.TaxDtl = tempsaleOrderTaxHdrList;
                                    tempsaleOrderTaxHdrList = null;
                                }
                                if (DirectsaleOrderDetailsObj.TaxDtl != null)
                                {
                                    if (DirectsaleOrderDetailsObj.TaxDtl.Count > 0)
                                    {
                                        foreach (var taxitem in DirectsaleOrderDetailsObj.TaxDtl)
                                        { taxitem.SLT_SL_NO = DirectsaleOrderDetailsObj.SOD_SL_NO; }
                                    }
                                }
                                if (chkAddtlRemarkToAll.Checked)  //update additional remark for all brands 
                                {
                                    if (DirectsaleOrderDetailsList != null)
                                    {
                                        if (DirectsaleOrderDetailsList.Count > 0)
                                        {
                                            DirectsaleOrderDetailsList.ForEach(lst => lst.SOD_REMARKS2 = DirectsaleOrderDetailsObj.SOD_REMARKS2);
                                        }
                                    }
                                }

                                if (chkRemarkToAll.Checked)  //update remark for all brands 
                                {
                                    if (DirectsaleOrderDetailsList != null)
                                    {
                                        if (DirectsaleOrderDetailsList.Count > 0)
                                        {
                                            DirectsaleOrderDetailsList.ForEach(lst => lst.SOD_REMARKS = DirectsaleOrderDetailsObj.SOD_REMARKS);
                                        }
                                    }
                                }

                                DirectsaleOrderDetailsList.Add(DirectsaleOrderDetailsObj);
                            }
                            else
                            {
                                IsItem = true;
                                litErrorMsg.Text = GetLocalResourceObject("ItemExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                ResetForm(ControlsEnum.SALEORDERDETAIL);
                                return retObject;
                            }
                        }
                        retObject = DirectsaleOrderDetailsList;
                        break;
                    #endregion
                }
                return retObject;
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

        #region GetUIValuesFromObject
        /// <summary>
        /// Assigns the input control values with corresponding object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Line Item Details
                    case ControlsEnum.SELECTEDITEM:
                        if (DirectsaleOrderDetailsObj != null)
                        {
                            hdfDetailPK.Value = DirectsaleOrderDetailsObj.SOD_PK.ToString();
                            CurrSlNo = DirectsaleOrderDetailsObj.SOD_SL_NO;
                            hdfItemCategory.Value = DirectsaleOrderDetailsObj.SOD_ITEM_CATEGORY.ToString();
                            txtItemCategory.Text = HttpUtility.HtmlDecode(DirectsaleOrderDetailsObj.SOD_ITEM_CATEGORY_TEXT);
                            SetItemCategory();
                            hdfPackingSpec.Value = DirectsaleOrderDetailsObj.SOD_PACK_SPEC.ToString();
                            txtPackingSpec.Text = HttpUtility.HtmlDecode(DirectsaleOrderDetailsObj.SOD_PACK_SPEC_NAME);
                            hdfItem.Value = DirectsaleOrderDetailsObj.SOD_ITEM.ToString();
                            txtItem.Text = DirectsaleOrderDetailsObj.SOD_ITEM_TEXT;
                            ddlProdSubCategory.SelectedIndex = ddlProdSubCategory.Items.IndexOf(ddlProdSubCategory.Items.FindByValue(DirectsaleOrderDetailsObj.SOD_ITEM_SUB_TYPE.ToString()));

                            txtItemQuantity.Text = hdfQtyTemp.Value = GetFormattedNumber(DirectsaleOrderDetailsObj.SOD_QTY);
                            hdfUOM.Value = DirectsaleOrderDetailsObj.SOD_UOM.ToString();
                            ddlUOM.SelectedValue = DirectsaleOrderDetailsObj.SOD_UOM.ToString();

                            txtUnitPrice.Text = GetFormattedRate(DirectsaleOrderDetailsObj.SOD_RATE);
                            txtAmount.Text = GetFormattedCurrency(DirectsaleOrderDetailsObj.SOD_AMOUNT);
                            txtDiscount.Text = GetFormattedCurrency(DirectsaleOrderDetailsObj.SOD_DISCOUNT);
                            hdfCustomDiscPerc.Value = DirectsaleOrderDetailsObj.SOD_DISC_PERC.ToString();
                            txtTax.Text = GetFormattedCurrency(DirectsaleOrderDetailsObj.SOD_TAX);
                            txtTotalAmt.Text = GetFormattedCurrency(DirectsaleOrderDetailsObj.SOD_NET_AMOUNT);
                            txtReqDate.Text = DirectsaleOrderDetailsObj.SOD_REQUIRED_BY;
                            txtDtlRemark.Text = HttpUtility.HtmlDecode(DirectsaleOrderDetailsObj.SOD_REMARKS);
                            txtDtlRemark2.Text = HttpUtility.HtmlDecode(DirectsaleOrderDetailsObj.SOD_REMARKS2);
                        }
                        break;
                    #endregion
                    # region  File Upload
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = DirectsaleOrderHeaderObj.SOH_PK;
                        SOUploadList = DirectsaleOrderHeaderObj.FileList;
                        break;
                    case ControlsEnum.SELECTEDDOC:
                        if (soUploadObj != null)
                        {
                            CurrDocSlNo = soUploadObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = soUploadObj.DOC_NAME;
                            anchorFile.HRef = soUploadObj.DOC_PATH;
                            anchorFile.Attributes.Remove("onclick");
                            anchorFile.Style.Add("cursor", "pointer");
                            if (FileDetailsList != null && FileDetailsList.Where(fle => fle.SlNo == CurrDocSlNo).Count() > 0)
                            {
                                anchorFile.Attributes.Add("onclick", "return false;");
                                anchorFile.Style.Add("cursor", "default");
                            }
                        }
                        break;
                    #endregion
                    #region Edit Details
                    case ControlsEnum.DETAILFOREDIT:
                        if (DirectSaleOrderHeaderSession != null)
                        {
                            if (IsCopySO == 1)
                            {
                                lblSaleOrderNo.Text = Resources.Messages.DocGenerationNew;
                                txtSaleOrderDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                            }
                            else
                            {
                                lblSaleOrderNo.Text = lblSaleOrderNo.ToolTip = String.IsNullOrEmpty(DirectSaleOrderHeaderSession.SOH_NO) ? Resources.Messages.DocGenerationNew : DirectSaleOrderHeaderSession.SOH_NO;
                                txtSaleOrderDate.Text = DirectSaleOrderHeaderSession.SOH_DATE == null ? string.Empty : ((DateTime)DirectSaleOrderHeaderSession.SOH_DATE).ToString(Resources.Constants.DateFormatShort);
                            }

                            custPK = DirectSaleOrderHeaderSession.SOH_CUSTOMER;
                            hdfCustomer.Value = DirectSaleOrderHeaderSession.SOH_CUSTOMER.ToString();
                            txtCustomer.Text = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_CUSTOMER_TEXT);
                            GetFieldValues(ControlsEnum.CUSTOMER);
                            custPK = Convert.ToInt32(hdfCustomer.Value);
                            if (dsPageData.Tables[0].Rows[0]["CUS_DISC_PERC"] != null && dsPageData.Tables[0].Rows[0]["CUS_DISC_PERC"].ToString() != string.Empty)
                                hdfCusDiscPerc.Value = dsPageData.Tables[0].Rows[0]["CUS_DISC_PERC"].ToString();
                            txtCustomer.Focus();
                            txtCustomerRef.Text = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_REFERENCE);

                            txtCusAddress.Text = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_CUSTOMER_ADDRESS);
                            hdfCusAddress.Value = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_CUSTOMER_ADDRESS);
                            hdfCusCountry.Value = DirectSaleOrderHeaderSession.SOH_CUSTOMER_COUNTRY.ToString();
                            //hdfCusCountryText.Value = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_CUSTOMER_COUNTRY_TEXT);
                            hdfCusZip.Value = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_CUSTOMER_ZIP);
                            hdfCusPhone.Value = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_CUSTOMER_PHONE);
                            hdfCusMobile.Value = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_CUSTOMER_MOBILE);
                            hdfCusFax.Value = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_CUSTOMER_FAX);
                            hdfCusEmail.Value = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_CUSTOMER_EMAIL);
                            if (!string.IsNullOrEmpty(DirectSaleOrderHeaderSession.CUS_SPECIAL_CAT_TEXT))
                            {
                                lblSpecialCat.Visible = true;
                                lblSpecialCat.Text = DirectSaleOrderHeaderSession.CUS_SPECIAL_CAT_TEXT.ToString();
                            }

                            ddlSOType.SelectedValue = DirectSaleOrderHeaderSession.SOH_TYPE.ToString();
                            ddlSOType.SelectedItem.Text = DirectSaleOrderHeaderSession.SOH_TYPE_TEXT.ToString();
                            ddlSOType.Enabled = false;

                            hdfCurrency.Value = DirectSaleOrderHeaderSession.SOH_CURRENCY.ToString();
                            txtCurrency.Text = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_CURRENCY_TEXT);

                            txtExchangeRate.Text = HttpUtility.HtmlDecode(DirectSaleOrderHeaderSession.SOH_CURRENCY_RATE.ToString(hdfExchangeRateFormat.Value));
                            txtDeliveryDate.Text = DirectSaleOrderHeaderSession.SOH_DELIVERY_DATE == null ? string.Empty : ((DateTime)DirectSaleOrderHeaderSession.SOH_DELIVERY_DATE).ToString(Resources.Constants.DateFormatShort);

                            txtHdrDiscount.Text = DirectSaleOrderHeaderSession.SOH_TOTAL_DISCOUNT.ToString("c");
                            txtHdrTax.Text = DirectSaleOrderHeaderSession.SOH_TOTAL_TAX.ToString("c");
                            txtHdrOtrCharge.Text = DirectSaleOrderHeaderSession.SOH_TOTAL_SHIP_CHARGE.ToString("c");
                            txtHdrPriceAdj.Text = DirectSaleOrderHeaderSession.SOH_TOTAL_ADJUST.ToString("c");
                            txtHdrTotal.Text = DirectSaleOrderHeaderSession.SOH_NET_AMOUNT.ToString("c");


                            ddlCompany.SelectedValue = DirectSaleOrderHeaderSession.SOH_COMPANY.ToString();
                            LastModifiedTime = DirectSaleOrderHeaderSession.LAST_MOD_DT;
                            if (hdfIsForAment.Value != null && hdfIsForAment.Value == "1")
                            {
                                divAmendDate.Visible = true;
                                txtAmendDate.Text = String.IsNullOrEmpty(DirectSaleOrderHeaderSession.SOH_AMEND_DATE) ? DateTime.Now.ToString(Resources.ErpRes.DateFormat) : DirectSaleOrderHeaderSession.SOH_AMEND_DATE;
                            }
                            else
                            {
                                divAmendDate.Visible = false;
                                txtAmendDate.Text = string.Empty;

                                if (DirectSaleOrderHeaderSession.SOH_VERSION > 1)
                                {
                                    if (!String.IsNullOrEmpty(DirectSaleOrderHeaderSession.SOH_AMEND_DATE))
                                    {
                                        divAmendDate.Visible = true;
                                        txtAmendDate.Text = DirectSaleOrderHeaderSession.SOH_AMEND_DATE;
                                    }
                                }
                            }
                            if (DirectSaleOrderHeaderSession.SOH_VERSION > 1)
                            {
                                btnRevision.Visible = true;
                            }
                            else
                            {
                                btnRevision.Visible = false;
                            }

                            DirectsaleOrderHeaderObj = DirectSaleOrderHeaderSession;
                            DirectsaleOrderHeaderObj.DirectsaleOrderDetails = DirectSaleOrderHeaderSession.DirectsaleOrderDetails;

                            hdfIsOrderDetailsVisible.Value = CommonConstants.SELECT_VALUE_ONE;

                            SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
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

        #region Workflow Methods

        private int GetApplicationID(int refId)
        {
            int appId = 0;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtApplication = wrkfService.GetApplicationID(refId);
            if (dtApplication != null)
            {
                if (dtApplication.Rows.Count > 0)
                {
                    processPK = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PROCESS] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PROCESS]);
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        private int FillProcessID(int deptID = 0, int pid = 0, bool SetProcessID = false)
        {
            int processID = 0;
            string path = string.Empty;

            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();

            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess;
            if (deptID > 0)
            {
                dtProcess = wrkfService.GetProcessID(path, deptID);
            }
            else
            {
                dtProcess = wrkfService.GetProcessID(path, Session[BusinessObject.Common.SessionStrings.CurDept] != null ? Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()) : 0);
            }
            if (SetProcessID && dtProcess != null && dtProcess.Rows.Count > 0)
            {
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
            }
            else
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    base.WkfPageUrl = ucrWrkf.PageUrl = path;
                    PageProcessID = ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    processID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                }
            return processID;
        }

        private void SaveTransaction(DirectSaleOrderBO objSaleOrder, int workflowFlag)
        {
            int retRfID = 0;
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;

            if (objSaleOrder == null)
                objSaleOrder = new DirectSaleOrderBO();

            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objSaleOrder.USER_PK = wkfDetails.UserPK;
            objSaleOrder.WKF_APPLICATION = CurrPK;
            objSaleOrder.WKF_COMMENTS = wkfDetails.Comments;
            objSaleOrder.WKF_TRX_FLAG = workflowFlag;
            objSaleOrder.WKF_PROCESS = wkfDetails.ProcessID;
            objSaleOrder.WKF_REFERENCE = wkfDetails.ReferenceID;
            objSaleOrder.WKF_TASK = wkfDetails.TaskID;
            objSaleOrder.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            #endregion

            string strTrxNo = string.Empty;
            string xmlDoc = CommonFunctions.XmlSerialize<DirectSaleOrderBO>(objSaleOrder);
            result = BusinessLogic.Sales.DirectSaleOrderBL.SaveDirectSaleOrderWkfDetails(xmlDoc, out retRfID);

            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                CurrPK = (int)result;

                #region Attachment Details
                //Document Attach details
                if (SOUploadList != null && SOUploadList.Count > 0)
                {
                    savePath = string.Empty;
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                        if (!Directory.Exists(savePath))
                            Directory.CreateDirectory(savePath);
                        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                    }
                    else
                    {
                        savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                    }

                    foreach (DirectSaleOrderUploads obj in SOUploadList)
                    {
                        string filePath = savePath + obj.AttachmentFileName;
                        FileInfo attachedFileInfo = new FileInfo(filePath);
                        if (FileDetailsList != null)
                        {
                            DirectFileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                            if (fileDetailsObj != null)
                            {
                                fileDetailsObj.SoFile.SaveAs(attachedFileInfo.FullName);
                            }
                        }
                    }
                }
                #endregion

                #region After Workflow

                if (!isCancelled)
                {
                    GetFieldValues(ControlsEnum.DETAILFOREDIT);
                    if (string.IsNullOrEmpty(strTrxNo))
                        strTrxNo = DirectSaleOrderHeaderSession.SOH_NO;
                    else
                        strTrxNo = lblSaleOrderNo.Text.Trim();
                }

                if (isCancelled)
                {
                    strTrxNo = lblSaleOrderNo.Text.Trim();
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Cancel_Success").ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectSaleOrder, strTrxNo);
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectSaleOrder, strTrxNo);
                }

                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ResetForm(ControlsEnum.LISTING);
                    FillProcessID(0, 1);
                    GetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                    SetFieldValues(ControlsEnum.DIRECTSALEORDERLIST);
                    ResetForm(ControlsEnum.DIRECTSALEORDER);
                    ResetForm(ControlsEnum.SALEORDERDETAIL);
                    txtDtlRemark2.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                    this.EntryStatus = EntryStatus.LISTMODE;
                }
                ucrWrkf.ApplicationID = CurrPK = Convert.ToInt32(result);

                #endregion

            }
            else
            {
                #region Validation From SQL
                if (result == 0)//Already used in other places.
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + Resources.ErrorMessages.Msg_Save_Err_Ref + "','" + Resources.ErpRes.Information + "');", true);

                }
                else if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.DirectSaleOrder + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.DirectSaleOrder + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.Captions.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.DirectSaleOrder + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == -36)
                {
                    litErrorMsg.Text = Resources.Messages.CannotModifyHaveReference;//Cannot modify,some of the items were referenced in other pages
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                    return;
                }
                else if (result == -37)
                {
                    litErrorMsg.Text = Resources.Messages.MsgRefAdded;//Supplier Ref# already entered
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                    return;
                }
                else if (result == -6)//Total outstanding amount exceeds the credit limit set for the customer.	
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Msg_OutstandingAmt_Exceeds").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == -12)
                {
                    litErrorMsg.Text = GetLocalResourceObject("Err_Qty_Despatch").ToString();//SO Qty can not be less than DO Qty
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Captions.Information + "','" + "');", true);
                    return;
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectSaleOrder);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.Captions.Information + "');", true);
                    return;
                }
                #endregion
            }
        }

        #endregion

        #region SetUIEditView
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
        #endregion

        #region Dropdown Binding

        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.SOTYPE:
                        ddlSOType.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlSOType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CFG_DATA");
                            ddlSOType.DataTextField = "CFG_DATA";
                            ddlSOType.DataValueField = "CFG_VALUE";
                            ddlSOType.DataBind();

                            ddlPortType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CFG_DATA");
                            ddlPortType.DataTextField = "CFG_DATA";
                            ddlPortType.DataValueField = "CFG_VALUE";
                            ddlPortType.DataBind();
                        }
                        ddlSOType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        ddlPortType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.UOM:
                        ddlUOM.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlUOM.DataSource = dtPageData;
                            ddlUOM.DataTextField = "UOM_CODE";
                            ddlUOM.DataValueField = "UOM_PK";
                            ddlUOM.DataBind();
                        }
                        if (dtPageData == null || (dtPageData != null && dtPageData.Rows.Count != 1))
                            ddlUOM.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.TAXTYPES:
                        ddlPopupTaxType.Items.Clear();
                        if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count > 0)
                        {
                            ddlPopupTaxType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSaleOrderTaxDetails, "TAX_DISP_NAME");
                            ddlPopupTaxType.DataTextField = "TAX_DISP_NAME";
                            ddlPopupTaxType.DataValueField = "TAX_PK";
                            ddlPopupTaxType.DataBind();
                        }

                        if (IsCustomTaxEnabled || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Discount) || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Shipping))
                        {
                            ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                            if (Convert.ToInt16(hdfTaxCategory.Value) != ((int)TaxType.Tax))
                                ddlPopupTaxType.SelectedValue = CommonConstants.SELECTVAL;
                            ShowOtherChargesDiv();
                        }

                        break;
                    case ControlsEnum.COMPANY:
                        ddlCompany.Items.Clear();
                        if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                        {
                            ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                            ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                            ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                            ddlCompany.DataBind();
                        }
                        break;
                    case ControlsEnum.SUBCATEGORY:
                        ddlProdSubCategory.Items.Clear();
                        if (dtSubCategory != null && dtSubCategory.Rows.Count > 0)
                        {
                            ddlProdSubCategory.DataSource = dtSubCategory;
                            ddlProdSubCategory.DataTextField = Resources.DataFieldRes.cfgData;
                            ddlProdSubCategory.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddlProdSubCategory.DataBind();
                        }
                        else
                            ddlProdSubCategory.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Grid Bind

        private void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region SALE ORDER LIST
                    case ControlsEnum.DIRECTSALEORDERLIST:
                        int rowCount = 0;
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        if (dsPageData.Tables[0].Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                        grdSaleOrderSearchList.DataSource = dsPageData.Tables[0];
                        grdSaleOrderSearchList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    #endregion

                    #region SALE ORDER ITEM DETAIL
                    case ControlsEnum.SALEORDERDETAIL:

                        if (DirectsaleOrderHeaderObj != null)
                        {
                            DirectsaleOrderDetailsList = new List<DirectSaleOrderDetailsBO>();
                            DirectsaleOrderDetailsList = DirectsaleOrderHeaderObj.DirectsaleOrderDetails;
                            if (DirectsaleOrderDetailsList != null)
                            {
                                grdOrderDetails.DataSource = DirectsaleOrderDetailsList;
                                grdOrderDetails.DataBind();
                            }
                        }
                        else
                        {
                            grdOrderDetails.DataSource = null;
                            grdOrderDetails.DataBind();
                        }
                        break;
                    #endregion

                    #region POP UP GRID
                    case ControlsEnum.TAXPOPUPGRID:
                        if (IsHeaderTax)
                        {
                            contractTaxHdrList = DirectTempSaleOrderHeaderSession.TaxHdrDtl == null ? new List<DirectSaleOrderTaxHdr>() :
                                DirectTempSaleOrderHeaderSession.TaxHdrDtl.Where(tax => Convert.ToInt32(tax.SLT_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                        }
                        else
                        {
                            if (CurrSlNo > 0)
                            {
                                orderDetails = EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails == null ? null :
                                    EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);

                            }
                            else
                            {
                                orderDetails = EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails == null ? null :
                                    EditDirectTempSaleOrderHeaderSession.DirectsaleOrderDetails.LastOrDefault(dtl => dtl.SOD_PK == SelectedDtlPK
                                    && dtl.SOD_ITEM == SelectedItemPK);
                            }

                            if (orderDetails != null)
                            {
                                contractTaxHdrList = orderDetails.TaxDtl == null ? new List<DirectSaleOrderTaxHdr>() :
                                    orderDetails.TaxDtl.Where(tax => Convert.ToInt32(tax.SLT_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                            else
                                contractTaxHdrList = new List<DirectSaleOrderTaxHdr>();
                        }
                        grdTaxDetails.DataSource = contractTaxHdrList;
                        grdTaxDetails.DataBind();
                        break;
                    #endregion

                    #region ATTACHMENT GRID
                    case ControlsEnum.UPLOADEDFILES:
                        if (SOUploadList != null && SOUploadList.Count > 0)
                        {
                            grdUploads.DataSource = SOUploadList;
                            grdUploads.DataBind();
                        }
                        else
                        {
                            grdUploads.DataSource = null;
                            grdUploads.DataBind();
                        }
                        break;
                    #endregion
                    case ControlsEnum.REVISIONHISTORY:
                        grdRevisionHistory.DataSource = dsPageData.Tables[0];
                        grdRevisionHistory.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region RESET

        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.CLEAR:
                    ddlSOType.Enabled = txtCurrency.Enabled = true;
                    lblSaleOrderNo.Text = Resources.Messages.DocGenerationNew;
                    btnRevision.Visible = false;
                    hdfIsOrderDetailsVisible.Value = hdfIsAttachDocsVisible.Value = CommonConstants.SELECT_VALUE_ONE;
                    base.WkfRefID = ucrWrkf.RefID = 0;
                    SetCancelRef(CurrPK);
                    ucrWrkf.FillWorkFlowDetails();
                    ucrWrkf.ViewType = 1;
                    ucrWrkf.ViewAction();
                    CurrPK = IsCopySO = 0;
                    break;
                case ControlsEnum.ADDITEM:
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
                case ControlsEnum.LISTING:
                    txtFromDate.Text = txtToDate.Text = txtDSONumber.Text = string.Empty;
                    txtAdvCustomerSrch.Text = Resources.ErpRes.AutoDefaultValue;
                    ddlStatus.SelectedValue = "-1";
                    ResetDdl(ddlPortType);
                    break;
                case ControlsEnum.DIRECTSALEORDER:
                    CurrPK = 0;
                    txtSaleOrderDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                    txtCustomerRef.Text = txtCustomer.Text = txtExchangeRate.Text = txtDeliveryDate.Text = string.Empty;
                    txtHdrDiscount.Text = txtHdrOtrCharge.Text = txtHdrTax.Text = txtHdrPriceAdj.Text = txtHdrTotal.Text = GetFormattedCurrency(0);
                    txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                    txtPopupAmount.Text = txtPopupCharge.Text = txtPopupOther.Text = string.Empty;
                    hdfCustomer.Value = "0";
                    txtCustomer.Focus();
                    ResetDdl(ddlPopupTaxType);
                    ResetDdl(ddlSOType);
                    SOUploadList = null;
                    FileDetailsList = null;
                    btnNew.Visible = true;
                    hdfIsForAment.Value = "0";
                    txtCusAddress.Text = string.Empty;
                    hdfCusAddress.Value = hdfCusCountry.Value = hdfCusCountryText.Value = hdfCusZip.Value
                    = hdfCusPhone.Value = hdfCusMobile.Value = hdfCusFax.Value = hdfCusEmail.Value = string.Empty;
                    dvPerc.Visible = false;
                    txtPackingSpec.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfPackingSpec.Value = string.Empty;
                    txtReqDate.Text = string.Empty;
                    txtItemCategory.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfItemCategory.Value = string.Empty;
                    lblSpecialCat.Text = string.Empty;
                    break;
                case ControlsEnum.SALEORDERDETAIL:
                    DirectTempSaleOrderHeaderSession = DirectSaleOrderHeaderSession;
                    CurrSlNo = 0;
                    hdfDetailPK.Value = CommonConstants.SELECT_VALUE_ZERO;
                    hdfItem.Value = string.Empty;
                    txtItem.Text = Resources.ErpRes.AutoDefaultValue;
                    //txtReqDate.Text = string.Empty;
                    ResetForm(ControlsEnum.CUSTOMERPRODUCT);
                    dvPerc.Visible = false;
                    hdfCustomDiscPerc.Value = "0";
                    break;
                case ControlsEnum.CUSTOMERPRODUCT:
                    hdfItem.Value = string.Empty;
                    txtItem.Text = Resources.ErpRes.AutoDefaultValue;
                    //txtReqDate.Text = string.Empty;
                    hdfUOM.Value = string.Empty;
                    ClearDdl(ControlsEnum.UOM);
                    txtItemQuantity.Text = txtAmount.Text = txtDiscount.Text = txtTax.Text = txtTotalAmt.Text = GetFormattedCurrency(0);
                    txtUnitPrice.Text = GetFormattedRate(0);
                    hdfItem.Value = string.Empty;
                    txtDtlRemark.Text = txtItemPchQty.Text = string.Empty;
                    hdfTotalBagPcs.Value = "0";
                    hdfPouchPcs.Value = "0";
                    break;
                case ControlsEnum.TAXPOPUPGRID:
                    TaxPK = 0;
                    SelectedDtlPK = 0;
                    SelectedItemPK = 0;
                    hdfTaxCategory.Value = string.Empty;
                    dvPerc.Visible = false;
                    break;
                case ControlsEnum.TAXCHECKBOX:
                    chkSubTotal.Checked = false;
                    chkDiscount.Checked = false;
                    chkOtherCharges.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// Reset Ddl Selected Value
        /// </summary>
        /// <param name="ddl"></param>
        private void ResetDdl(DropDownList ddl)
        {
            if (ddl != null)
            {
                ddl.ClearSelection();
                if (ddl.Items.Count > 1)
                    ddl.Items[0].Selected = true;
            }
        }

        /// <summary>
        /// Clear Ddl Values
        /// </summary>
        /// <param name="control"></param>
        private void ClearDdl(ControlsEnum control)
        {
            dtPageData = null;
            BindDropDownList(control);
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Show Other Charges Div
        /// </summary>
        private void ShowOtherChargesDiv()
        {
            hdfTaxFormula.Value = string.Empty;
            txtPopupCharge.Text = string.Empty;
            SelectedTaxText = Resources.Report.Custom;
            txtPopupOther.Text = string.Empty;
            txtPopupCharge.Enabled = true;
            txtPopupOther.Enabled = true;
            if (hdfTaxCategory.Value == ((int)TaxType.Discount).ToString() || hdfTaxCategory.Value == ((int)TaxType.Shipping).ToString())
            {
                dvPerc.Visible = true;
                txtPopupOther.Text = Resources.Controls.Discount;
            }
            else
                dvPerc.Visible = false;
        }
        private void SetDetailTax(DirectSaleOrderBO saleOrderHdr)
        {
            double quantity;
            double rate;
            quantity = 0;
            rate = 0;

            if (double.TryParse(txtUnitPrice.Text, out quantity) && double.TryParse(txtItemQuantity.Text, out rate))
            {
                if (txtAmount != null)
                {
                    txtAmount.Text = GetFormattedCurrency(rate * quantity);
                    SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                    SelectedItemPK = string.IsNullOrEmpty(hdfItem.Value) ? 0 : Convert.ToInt32(hdfItem.Value);
                    SetItemTax(saleOrderHdr);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }

        private bool SetItemTax(DirectSaleOrderBO saleOrderHdr)
        {
            double amount;
            double discount;
            double itmTax;
            double netAmount;
            amount = 0;
            discount = 0;
            netAmount = 0;
            itmTax = 0;
            Double.TryParse(txtAmount.Text.Trim(), out amount);
            if (amount >= 0)
            {
                if (saleOrderHdr != null)
                {
                    DirectsaleOrderHeaderObj = saleOrderHdr;
                    if (CurrSlNo > 0)
                    {

                        DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(crt => crt.SOD_PK == SelectedDtlPK
                             && crt.SOD_ITEM == SelectedItemPK && crt.SOD_SL_NO == CurrSlNo);
                    }
                    else
                    {
                        DirectsaleOrderDetailsObj = DirectsaleOrderHeaderObj.DirectsaleOrderDetails.LastOrDefault(crt => crt.SOD_PK == SelectedDtlPK
                            && crt.SOD_ITEM == SelectedItemPK);
                    }

                    if (DirectsaleOrderDetailsObj != null)
                    {
                        if (DirectsaleOrderDetailsObj.TaxDtl != null)
                        {
                            var discDetail = DirectsaleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (DirectSaleOrderTaxHdr taxHdrObj in discDetail)
                            {
                                string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                                if (taxHdrObj.SLT_DISC_PERC != null && taxHdrObj.SLT_DISC_PERC > 0)
                                    hdfCustomDiscPerc.Value = taxHdrObj.SLT_DISC_PERC.ToString();
                            }
                            discount = DirectsaleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.SLT_TAX_AMT);
                        }
                        netAmount = amount - discount;
                        txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                        if (DirectsaleOrderDetailsObj.TaxDtl != null)
                        {
                            List<DirectSaleOrderTaxHdr> taxDetail = DirectsaleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).ToList();
                            foreach (DirectSaleOrderTaxHdr taxDtlObj in taxDetail)
                            {
                                string taxFormula = taxDtlObj.SLT_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                    taxDtlObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                            itmTax = DirectsaleOrderDetailsObj.TaxDtl.ToList().Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(ctr => ctr.SLT_TAX_AMT);
                        }
                        txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                        DirectsaleOrderDetailsObj.SOD_AMOUNT = amount;
                        DirectsaleOrderDetailsObj.SOD_DISCOUNT = EnableItemDiscount == false ? 0 : discount;
                        DirectsaleOrderDetailsObj.SOD_TAX = EnableItemTax == false ? 0 : itmTax;
                        DirectsaleOrderDetailsObj.SOD_NET_AMOUNT = (amount - (EnableItemDiscount == false ? 0 : discount) + (EnableItemTax == false ? 0 : itmTax));
                        txtTotalAmt.Text = DirectsaleOrderDetailsObj.SOD_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                    }
                }
                return true;
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                return false;
            }
        }

        private void SetSubTotal()
        {
            Label lblSubTotalFooter;
            DirectSaleOrderHeaderSession.SOH_TOTAL_AMT = Convert.ToDouble(DirectSaleOrderHeaderSession.DirectsaleOrderDetails.Sum(dtl => dtl.SOD_NET_AMOUNT));
            if (grdOrderDetails.FooterRow != null)
            {
                lblSubTotalFooter = grdOrderDetails.FooterRow.FindControl("grid footer") as Label;
                if (lblSubTotalFooter != null)
                {
                    lblSubTotalFooter.Text = lblSubTotalFooter.ToolTip = DirectSaleOrderHeaderSession.SOH_TOTAL_AMT.ToString(hdfCurrencyFormat.Value);
                }
            }
        }

        private bool SetHdrTax()
        {
            double amount;
            double discount;
            double shipping;
            double adjust;
            amount = 0;
            shipping = 0;
            adjust = 0;

            if (DirectSaleOrderHeaderSession != null)
            {
                DirectsaleOrderHeaderObj = DirectSaleOrderHeaderSession;
                amount = Convert.ToDouble(DirectsaleOrderHeaderObj.SOH_TOTAL_AMT);

                #region Discount
                discount = 0;
                if (DirectsaleOrderHeaderObj.TaxHdrDtl != null)
                {
                    var discHeader = DirectsaleOrderHeaderObj.TaxHdrDtl.Where(hdr => hdr.SLT_TAX_CATEGORY == ((int)TaxType.Discount));
                    foreach (DirectSaleOrderTaxHdr taxHdrObj in discHeader)
                    {
                        string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    discount = DirectsaleOrderHeaderObj.TaxHdrDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(quotation => quotation.SLT_TAX_AMT);
                }
                DirectsaleOrderHeaderObj.SOH_TOTAL_DISCOUNT = discount;
                txtHdrDiscount.Text = txtHdrDiscount.ToolTip = discount.ToString(hdfCurrencyFormat.Value);
                amount = amount - discount;
                #endregion
                #region Shipping Charge
                if (DirectsaleOrderHeaderObj.TaxHdrDtl != null)
                {
                    var shippingHeader = DirectsaleOrderHeaderObj.TaxHdrDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Shipping));
                    foreach (DirectSaleOrderTaxHdr taxHdrObj in shippingHeader)
                    {
                        string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    shipping = DirectsaleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE = DirectsaleOrderHeaderObj.TaxHdrDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(quotation => quotation.SLT_TAX_AMT);
                }
                txtHdrOtrCharge.Text = txtHdrOtrCharge.ToolTip = shipping.ToString(hdfCurrencyFormat.Value);
                DirectsaleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE = shipping;
                #endregion
                #region Tax
                if (!IsHeaderTaxForTradingSale || !IsItemwiseTaxForTradingSale)
                {
                    //Add Other Charges Based On Configuration
                    if (IsTaxForOtherCharge)
                    {
                        amount += string.IsNullOrEmpty(txtHdrOtrCharge.Text) ? 0.00 : Convert.ToDouble(txtHdrOtrCharge.Text);
                    }
                }
                if (DirectsaleOrderHeaderObj.TaxHdrDtl != null)
                {
                    var taxHeader = DirectsaleOrderHeaderObj.TaxHdrDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax));
                    foreach (DirectSaleOrderTaxHdr taxHdrObj in taxHeader)
                    {
                        if (IsHeaderTaxForTradingSale && IsItemwiseTaxForTradingSale)
                        {
                            amount = 0;
                            if (taxHdrObj.SLT_HAS_SUB_TOTAL == 1)
                                amount += Convert.ToDouble(DirectsaleOrderHeaderObj.SOH_TOTAL_AMT);
                            if (taxHdrObj.SLT_HAS_DISCOUNT == 1)
                                amount = txtHdrDiscount.Text != string.Empty ? amount - Convert.ToDouble(txtHdrDiscount.Text) : amount;
                            if (taxHdrObj.SLT_HAS_OTHER_CHARGE == 1)
                                amount += txtHdrOtrCharge.Text != string.Empty ? Convert.ToDouble(txtHdrOtrCharge.Text) : 0;
                        }
                        string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    if (IsHeaderTaxForTradingSale)
                        DirectsaleOrderHeaderObj.SOH_TOTAL_TAX = DirectsaleOrderHeaderObj.TaxHdrDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.SLT_TAX_AMT);
                    else
                        DirectsaleOrderHeaderObj.SOH_TOTAL_TAX = 0;
                }
                #endregion

                txtHdrTax.Text = txtHdrTax.ToolTip = DirectsaleOrderHeaderObj.SOH_TOTAL_TAX.ToString(hdfCurrencyFormat.Value);
                //double.TryParse(txtShipping.Text, out shipping);

                double.TryParse(txtHdrPriceAdj.Text, out adjust);
                DirectsaleOrderHeaderObj.SOH_TOTAL_ADJUST = adjust;
                DirectsaleOrderHeaderObj.SOH_NET_AMOUNT = Convert.ToDouble(DirectsaleOrderHeaderObj.SOH_TOTAL_AMT) - DirectsaleOrderHeaderObj.SOH_TOTAL_DISCOUNT + DirectsaleOrderHeaderObj.SOH_TOTAL_TAX
                    + DirectsaleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE + DirectsaleOrderHeaderObj.SOH_TOTAL_ADJUST;

                txtHdrTotal.Text = txtHdrTotal.ToolTip = DirectsaleOrderHeaderObj.SOH_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                DirectSaleOrderHeaderSession = DirectsaleOrderHeaderObj;
                DirectTempSaleOrderHeaderSession = DirectsaleOrderHeaderObj;
                //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalcTotal", "CalculateTotal();", true);
            }
            return true;
        }

        private double CalculateTaxFormula(string taxFormula, double amount)
        {
            double taxAmt;
            taxAmt = 0;
            if (!string.IsNullOrEmpty(taxFormula))
            {
                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                taxAmt = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            }
            return taxAmt;
        }

        /// <summary>
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            string BlockedExtensions = "dll";
            if (System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower() != string.Empty)
            {
                BlockedExtensions = System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower();
            }
            bool flag = true;
            string[] extensionList = BlockedExtensions.Split(',');
            for (int i = 0; i < extensionList.Length; i++)
                if (("." + extensionList[i]) == extension)
                {
                    flag = false;
                    break;
                }
            return flag;
        }

        /// <summary>
        /// For Bind Cancelation comment on workflow user control
        /// </summary>
        /// <param name="curPK"></param>
        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            string TYPE = Request.QueryString[QueryStrings.PageType] != null ? Request.QueryString[QueryStrings.PageType] : string.Empty;
            if (TYPE != "3")//Type 3 for cancelation
            {
                DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
                if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
                {
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
                }
            }
            #endregion
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            commonServiceObj = new CommonService();
            AppTypeDetailsList = commonServiceObj.GetReportParameters(ApplicationType.SOD, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        private void FormatNumber()
        {
            string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            hdfDecimalFormatWithComma.Value = "#" + currencysep + "#0.";
            hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
            hdfDecimalFormat.Value = "#0.";
            for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
            {
                hdfDecimalFormat.Value += "0";
                hdfDecimalFormatWithComma.Value += "0";
            }
            hdfCurrencyFormat.Value = "#0.";
            for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
            {
                hdfCurrencyFormat.Value += "0";
                hdfCurrencyFormatWithComma.Value += "0";
            }
            hdfRateFormat.Value = "#0.";
            int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
            for (int i = 0; i < rateDecimalDigits; i++)
            {
                hdfRateFormat.Value += "0";
            }

            hdfExchangeRateFormat.Value = "#0.";
            int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
            for (int i = 0; i < exchrateDecimalDigits; i++)
            {
                hdfExchangeRateFormat.Value += "0";
            }

            hdfWeightFormat.Value = "#0.";
            int weightDecimalDigits = (Session[ERP.Utilities.SessionStrings.WeightDecimalDigit] == null
                ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.WeightDecimalDigit]));
            for (int i = 0; i < weightDecimalDigits; i++)
            {
                hdfWeightFormat.Value += "0";
            }

            hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
            hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
            hdfRateDigits.Value = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]).ToString();
            hdfExchangeRateDigits.Value = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]).ToString();
        }

        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }

        public string GetFormattedNumberWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormatWithComma.Value);
        }

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }

        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }

        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }

        public string GetCeiledInteger(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return Math.Ceiling(num).ToString();
        }

        public double StringToFormula(string expression)
        {
            List<string> tokens = getTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;
            try
            {
                while (tokenIndex < tokens.Count)
                {
                    string token = tokens[tokenIndex];
                    if (token == "(")
                    {
                        string subExpr = getSubExpression(tokens, ref tokenIndex);
                        operandStack.Push(StringToFormula(subExpr));
                        continue;
                    }
                    if (token == ")")
                    {
                        throw new ArgumentException("Mis-matched parentheses in expression");
                    }
                    //If this is an operator  
                    if (Array.IndexOf(_operators, token) >= 0)
                    {
                        while (operatorStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
                        {
                            string op = operatorStack.Pop();
                            double arg2 = operandStack.Pop();
                            double arg1 = operandStack.Pop();
                            operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                        }
                        operatorStack.Push(token);
                    }
                    else
                    {
                        operandStack.Push(double.Parse(token));
                    }
                    tokenIndex += 1;
                }

                while (operatorStack.Count > 0)
                {
                    string op = operatorStack.Pop();
                    double arg2 = operandStack.Pop();
                    double arg1 = operandStack.Pop();
                    operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                }
                return operandStack.Pop();
            }
            catch
            {
                return 0;
            }
        }

        private string getSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }

        private List<string> getTokens(string expression)
        {
            string operators = "()^*/+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }

        private void GetUserRights()
        {
            string path = GetLocalResourceObject("wkfBaseURL").ToString();
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.SBUID, currentUser.CurrentDeptPK);
            if (usrRights.Rights.Count > 0)
            {
                for (int i = 0; i < usrRights.Rights.Count; i++)
                {
                    if (usrRights.Rights[i].ActionName == "SHORTCLOSURE" && usrRights.Rights[i].HasActionRight == true && usrRights.Rights[i].UserDeptRight == true)
                    {
                        HasShortCloseRight = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Initialize Component

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

        #endregion

        #region Enable Disable Buttons
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }
        #endregion

        /// <summary>
        /// Set Configuration settings
        /// </summary>
        private void ConfigurationSettings()
        {
            IsHeaderDiscountForTradingSale = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsHeaderDiscountForTradingSale")));
            IsHeaderTaxForTradingSale = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsHeaderTaxForTradingSale")));
            IsItemwiseDiscountForTradingSale = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsItemwiseDiscountForTradingSale")));
            IsItemwiseTaxForTradingSale = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsItemwiseTaxForTradingSale")));
            if (IsHeaderDiscountForTradingSale == true && IsHeaderTaxForTradingSale == true && IsItemwiseDiscountForTradingSale == true && IsItemwiseTaxForTradingSale == true)
            {
                imgHdrDiscount.Visible = imgHdrTax.Visible = true;
                imgDiscountDtl.Visible = imgTaxDtl.Visible = true;
            }
            else if (IsHeaderDiscountForTradingSale == false && IsHeaderTaxForTradingSale == false)
            {
                imgHdrDiscount.Visible = imgHdrTax.Visible = false;
                imgDiscountDtl.Visible = imgTaxDtl.Visible = true;
            }
            else if (IsItemwiseDiscountForTradingSale == false && IsItemwiseTaxForTradingSale == false)
            {
                imgHdrDiscount.Visible = imgHdrTax.Visible = true;
                imgDiscountDtl.Visible = imgTaxDtl.Visible = false;
            }

            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BRAND PRODUCT SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                ConfigRateGet = Convert.ToInt32(dt.Rows[0]["ACF_VALUE"].ToString());
            }

         
        }

        private void SetItemCategory()
        {
            if (hdfItemCategory.Value != string.Empty && Convert.ToInt32(hdfItemCategory.Value) > 0)
            {
                DataTable dt = CommonBL.GetCategoryValue(Convert.ToInt32(hdfItemCategory.Value), Convert.ToInt32(DbActiveStatus.ACTIVE));
                hdfItemCatVal.Value = dt.Rows[0]["ITC_VALUE"].ToString();
                if (dt.Rows[0]["ITC_VALUE"].ToString() == "1")
                {
                    ddlProdSubCategory.Enabled = false;
                    txtPackingSpec.Enabled = false;
                    txtItemPchQty.Enabled = false;
                    txtItemPchQty.CssClass = "input-xsmall-b margnrgt77 input-disabled";
                    txtPackingSpec.CssClass = "input-half input-disabled";
                    ddlProdSubCategory.Attributes.Add("style", "background-color:#e7e7e7!important;");
                    hdfNeedPackSpecVal.Value = "0";
                    SetFieldValues(ControlsEnum.SUBCATEGORY);
                }
                else
                {
                    GetFieldValues(ControlsEnum.SUBCATEGORY);
                    SetFieldValues(ControlsEnum.SUBCATEGORY);
                    ddlProdSubCategory.Attributes.Add("style", "background-color:white!important;");
                    hdfNeedPackSpecVal.Value = "1";
                    ddlProdSubCategory.Enabled = true;
                    txtPackingSpec.Enabled = true;
                    txtItemPchQty.Enabled = true;
                    txtItemPchQty.CssClass = "input-xsmall-b margnrgt77 numeric";
                    txtPackingSpec.CssClass = "input-half";
                }
                txtPackingSpec.Text = Resources.ErpRes.AutoDefaultValue;
                hdfPackingSpec.Value = "0";
                txtItem.Text = Resources.ErpRes.AutoDefaultValue;
                hdfItem.Value = "0";
            }

        }

        #region Enum

        public enum ControlsEnum
        {
            SOTYPE,
            DIRECTSALEORDER,
            CUSTOMER,
            EXCHANGERATE,
            TAXSETTINGS,
            CUSTOMTAXSETTINGS,
            USERCUSTOMER,
            UOM,
            CUSTOMERPRODUCT,
            SALEORDERDETAIL,
            SALEORDERHEADER,
            AUTOTAXPOPUP,
            AUTOTAXPOPUPITEMWISE,
            DISCOUNTHEADER,
            OTHERCHARGEHEADER,
            TAXHEADER,
            TAXPOPUPGRID,
            TAXTYPES,
            SELECTEDITEM,
            CLEAR,
            UPLOADEDFILES,
            ADDITEM,
            SELECTEDDOC,
            COMPANY,
            DIRECTSALEORDERLIST,
            DETAILFOREDIT,
            LISTING,
            CLEARGRID,
            SODETAILS,
            CLEARSEARCH,
            REVISIONHISTORY,
            COPYSODETAILS,
            TAXCHECKBOX,
            SUBCATEGORY,
            AUTODISCOUNTPOPUP
        }

        public enum DateDefaultEnum
        {
            CurrentDate = 0,
            FirstDate,
            LastDate
        }

        #endregion
    }
}