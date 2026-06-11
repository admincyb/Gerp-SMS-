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

namespace ERPSMS_v01.Journalize
{
    public partial class JournalizeListing : ERP.Store.UI.WorkFlowBasePage
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
        private int RefPK
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.RefPK];
            }
            set
            {
                this.ViewState[ViewstateStrings.RefPK] = value;
            }
        }
        private int TrxPK
        {
            get
            {
                if (this.ViewState[ViewstateStrings.TrxPK] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)this.ViewState[ViewstateStrings.TrxPK];
                }
            }
            set
            {
                this.ViewState[ViewstateStrings.TrxPK] = value;
            }
        }
        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrMpgPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrMpgPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrMpgPK] = value;
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
        /// Invoice
        /// </summary>
        private string RefType
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.RefType];
            }
            set
            {
                this.ViewState[ViewstateStrings.RefType] = value;
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
        /// VendorPK
        /// </summary>
        private int VendorPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.VendorPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorPK] = value;
            }
        }
        /// <summary>
        /// VendorCode
        /// </summary>
        private string VendorCode
        {
            get
            {
                return this.ViewState[ViewstateStrings.VendorCode].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorCode] = value;
            }
        }
        /// <summary>
        /// VendorName
        /// </summary>
        private string VendorName
        {
            get
            {
                return this.ViewState[ViewstateStrings.VendorName].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorName] = value;
            }
        }

        /// <summary>
        /// IsVendorSelected
        /// </summary>
        private bool IsVendorSelected
        {
            get
            {
                return (bool)this.ViewState[ViewstateStrings.IsVendorSelected];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsVendorSelected] = value;
            }
        }

        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedPos
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedPos];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedPos] = value;
            }

        }
        /// <summary>
        /// To maintain count of selected pos
        /// </summary>
        private int SelectedInvoicesCount
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.SelectedInvoicesCount];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedInvoicesCount] = value;
            }
        }
        /// <summary>
        /// To maintain count of selected invoices
        /// </summary>
        private int SelectedInvoicesCrDrCount
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.SelectedInvoicesCrDrCount];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedInvoicesCrDrCount] = value;
            }
        }
        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedInvoices
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedInvoices];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedInvoices] = value;
            }

        }

        /// <summary>
        /// To maintain keep selected invoices
        /// </summary>
        private List<long> SelectedInvoicesCrDr
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] = value;
            }

        }


        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<object> PurOrderHeaderList
        {
            get
            {
                return (List<object>)Session[ERP.Utilities.SessionStrings.PurOrderHeaderList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.PurOrderHeaderList] = value;
            }

        }

        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<object> PurOrderHeaderMappingList
        {
            get
            {
                return (List<object>)Session[ERP.Utilities.SessionStrings.PurOrderHeaderMappingList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.PurOrderHeaderMappingList] = value;
            }

        }
        /// <summary>
        /// To maintain keep PoHeader List
        /// </summary>
        private List<PUR_ORDER_HDR> PoHeaderList
        {
            get
            {
                return (List<PUR_ORDER_HDR>)Session[ERP.Utilities.SessionStrings.PoHeaderList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.PoHeaderList] = value;
            }

        }

        /// <summary>
        /// To maintain keep InvoiceMap List
        /// </summary>
        private List<FIN_INVOICE_VND_TRX_MPG> InvoiceMapList
        {
            get
            {
                return (List<FIN_INVOICE_VND_TRX_MPG>)Session[ERP.Utilities.SessionStrings.InvoiceMapList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.InvoiceMapList] = value;
            }

        }

        /// <summary>
        /// IsVendorSelected
        /// </summary>
        private bool IsVoucherDeleted
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsVoucherDeleted] == null ? false : (bool)this.ViewState[ViewstateStrings.IsVoucherDeleted];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsVoucherDeleted] = value;
            }
        }
        /// <summary>
        /// Aplication referance ID
        /// </summary>
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

        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private bool ShowVersions
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowVersions] == null ? false : (bool)(this.ViewState[ViewstateStrings.ShowVersions]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowVersions] = value;
            }
        }

        #endregion
        User currentUser;
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private FIN_TRX finTrxObj;
        private FIN_TRX_HDR finTrxHdrObj;
        private ADM_APP_TYPE_MST admAppTypeMstObj;
        private List<FIN_TRX> finTrxList;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private List<ADM_APP_TYPE_MST> admAppTypeMstList;
        private List<FIN_YEAR_MST> finYearMstList;

        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> workflowStatusList;
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusList1;
        private List<ADM_CONST_MST> admTemplateCategoryList;
        private string refID;
        private string inboxFlag;

        private DataSet dsTemplateList;
        private long VoucherPk;
        private DataTable dtVersions;       

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
                ucrWrkf.ViewType = 1;

                ucrJournalize.JournalizeSave += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeSubmit += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeDelete += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeCancel += new EventHandler(ActionHandler);

                ucrJournalize.ReverseSave += new EventHandler(ActionHandler);
                ucrJournalize.ReverseSubmit += new EventHandler(ActionHandler);
                ucrJournalize.ReverseDelete += new EventHandler(ActionHandler);
                ucrJournalize.ReverseCancel += new EventHandler(ActionHandler);

                ucrJournalize.ReturnSave += new EventHandler(ActionHandler);
                ucrJournalize.ReturnSubmit += new EventHandler(ActionHandler);
                ucrJournalize.ReturnDelete += new EventHandler(ActionHandler);
                ucrJournalize.ReturnCancel += new EventHandler(ActionHandler);
                if (!IsPostBack)
                {
                    //Sets data key for the gird

                    //txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();
                    ShowVersions = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowVoucherVersions")));
                    Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                    GetFieldValues(ControlsEnum.FINPERIOD);
                    SetFieldValues(ControlsEnum.FINPERIOD);

                    Session[ERP.Utilities.SessionStrings.Type] = Request.QueryString[QueryStrings.Type] != null ? Request.QueryString[QueryStrings.Type]
                       : Session[ERP.Utilities.SessionStrings.Type] != null ? Session[ERP.Utilities.SessionStrings.Type].ToString().Split('=')[1] : string.Empty;
                    hdfJournalizeType.Value = Session[ERP.Utilities.SessionStrings.Type].ToString();
                    hdfReturnTye.Value = Session[ERP.Utilities.SessionStrings.Type].ToString();
                    if (Request.QueryString[QueryStrings.SType] != null)
                    {
                        Session[ERP.Utilities.SessionStrings.SubType] = Request.QueryString[QueryStrings.SType] != null ? Request.QueryString[QueryStrings.SType]
                          : Session[ERP.Utilities.SessionStrings.SubType] != null ? Session[ERP.Utilities.SessionStrings.SubType].ToString().Split('=')[1] : string.Empty;
                    }
                    if (Request.QueryString[QueryStrings.Type] != null && Request.QueryString[QueryStrings.Type] == ApplicationType.YE)
                    {
                        FillProcessID(ApplicationType.YE, 0);
                    }
                    else
                    {
                        FillProcessID(Session[ERP.Utilities.SessionStrings.Type].ToString(), 0);
                    }

                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.JournalizePK;
                    grdJournalizeList.DataKeyNames = datakeyarray;
                    GetFieldValues(ControlsEnum.JOURNALIZATIONTYPE);
                    SetFieldValues(ControlsEnum.JOURNALIZATIONTYPE);
                    //GetFieldValues(ControlsEnum.FINHEADER);
                    //SetFieldValues(ControlsEnum.FINHEADER);
                    if (Session[ERP.Utilities.SessionStrings.Type].ToString() != ApplicationType.VPJ && Session[ERP.Utilities.SessionStrings.Type].ToString() != ApplicationType.DPVJ && Session[ERP.Utilities.SessionStrings.Type].ToString() != ApplicationType.DRVJ && Session[ERP.Utilities.SessionStrings.Type].ToString() != ApplicationType.VPTJ)
                    {
                        ddlStatus.Items.RemoveAt(3);
                    }

                    //if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV ||
                    //    Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCS ||
                    //     Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.OBV)
                    //{
                    //    btnNew.Visible = true;
                    //}
                    //else
                    //{
                    //    btnNew.Visible = false;
                    //}
                    if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV ||
                       Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCS)
                    {
                        btnNew.Visible = true;
                    }
                    //else
                    //{
                    //    btnNew.Visible = false;
                    //}
                    if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.CRJ
                        || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.CRTJ)
                    {
                        btnOffReceiptPrint.Visible = true;
                    }
                    else
                    {
                        btnOffReceiptPrint.Visible = false;
                    }
                    if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.SIJ || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DSIJ)
                    {
                        btnAdvInvPrint.Visible = true;
                    }
                    else
                    {
                        btnAdvInvPrint.Visible = false;
                    }
                    if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.OBV)
                    {
                        ddlStatus.Items.RemoveAt(3);
                    }
                    if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.CLSTJ || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YCV)
                    {
                        ddlStatus.Items.RemoveAt(2);
                    }
                    #region From External Link
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        TrxPK = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        int refPK = 0;
                        bool isReturn = false;
                        bool isPdcReverse = false;
                        GetFieldValues(ControlsEnum.FINHEADERONE);
                        if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                        {
                            refPK = Convert.ToInt32(finTrxHdrList[0].FTH_REF_PK);

                            Session[ERP.Utilities.SessionStrings.VoucherPk] = TrxPK;
                            Session[ERP.Utilities.SessionStrings.TrxPK] = TrxPK;
                            Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                            Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                            //Journalize New sessions start
                            Session[ERP.Utilities.SessionStrings.DrControls] = null;
                            Session[ERP.Utilities.SessionStrings.CrControls] = null;
                            Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                            Session[ERP.Utilities.SessionStrings.AccountType] = null;
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                            Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                            //Journalize New sessions End
                            ucrJournalize.TransactionType = Session[ERP.Utilities.SessionStrings.Type].ToString();
                            Session[ERP.Utilities.SessionStrings.TransactionType] = Session[ERP.Utilities.SessionStrings.Type].ToString();
                            ucrJournalize.TransactionPK = refPK;

                            if (refPK > 0)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV ? null : refPK.ToString();
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = "";
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = "";
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = "";
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                            }
                            else
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = null;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = null;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                            }
                            Session[ERP.Utilities.SessionStrings.JournalType] = Session[ERP.Utilities.SessionStrings.Type].ToString();
                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PDCCJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PDCCTJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PPCCJ)
                            {
                                ucrJournalize.JournalType = (int)JournalTypeEnum.Reverse;
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.RCBJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.RCBTJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCBJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPBJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCBTJ)
                            {
                                ucrJournalize.JournalType = (int)JournalTypeEnum.Return;
                            }
                            Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                            Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;

                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.OBV)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.OpeningBalance);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVJ)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.DirectPayment);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCVJ)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.PettyCashDetail);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DRVJ)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.DirectReceiptDetails);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YCV)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.YearClosingVoucher);
                            }
                            else
                            {
                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.Reset();
                                refPK = Convert.ToInt32(finTrxHdrList[0].FTH_REF_PK);
                                isReturn = finTrxHdrList[0].FTH_BOUNCED == 1 ? true : false;
                                isPdcReverse = finTrxHdrList[0].FTH_PDC > 1 ? true : false;
                                string type = Request.QueryString["Type"].ToString();
                                //int PK = Convert.ToInt32(Request.QueryString["PK"].ToString());
                                string path = GetJournalURL(type, Convert.ToInt32(refPK), isPdcReverse, isReturn);
                                FillProcessID(type, path);//
                                // GetFieldValues(ControlsEnum.FINHEADERONE);
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                {
                                    ucrWrkf.ViewType = 1;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                    //EntryStatus = EntryStatus.VIEWMODE;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                }
                                ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                ucrWrkf.ViewAction();
                                Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;

                                HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                                hdfExchangeRateJV.Value = "";

                                TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                                txtJournalExchangeRate.Text = "";

                                TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                                txtNarration.Text = "";

                                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";
                                ucrJournalize.VoucherDeleteStatus = finTrxHdrList[0].FTH_IS_DELETED == true ? true : false;
                                //ucrJournalize.VoucherDeleteStatus = ddlStatus.SelectedValue == "-1" ? true : false;
                                ucrJournalize.CallUserControl();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + (string.IsNullOrEmpty(hdfJournalHeader.Value) ? "test" : hdfJournalHeader.Value) + "');", true);
                            }
                        }

                    }
                    #endregion
                    else
                    {
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        ////start
                        refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                           : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                        inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                        : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                        //If Has RefID (from Inbox)
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
                            ucrWrkf.Reset();
                            base.WkfRefID = ucrWrkf.RefID = int.Parse(refID);
                            TrxPK = GetApplicationID(ucrWrkf.RefID);
                            GetFieldValues(ControlsEnum.GETREFPKBYJOURNALPK);
                            if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                            {
                                RefPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                            }
                            if (Session[ERP.Utilities.SessionStrings.Type] != null && !string.IsNullOrEmpty(Session[ERP.Utilities.SessionStrings.Type].ToString()))
                            {
                                RefType = Session[ERP.Utilities.SessionStrings.Type].ToString();
                            }
                            if (Request.QueryString[QueryStrings.Type] != null && Request.QueryString[QueryStrings.Type] == ApplicationType.YE)
                            {
                                FillProcessID(ApplicationType.YE, RefPK);
                            }
                            else
                            {
                                if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVCJ)
                                {
                                    FillProcessID(ApplicationType.DPVCJ, RefPK, false, true);
                                }
                                else
                                {
                                    FillProcessID(Session[ERP.Utilities.SessionStrings.Type].ToString(), RefPK);
                                }
                            }
                            EntryStatus = EntryStatus.ENTRYMODE;
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                                ucrWrkf.ViewType = 1;
                                //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                                //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                            }
                            ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                        }

                        if (TrxPK > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.TrxPK] = TrxPK;
                            Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                            Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                            //Journalize New sessions start
                            Session[ERP.Utilities.SessionStrings.DrControls] = null;
                            Session[ERP.Utilities.SessionStrings.CrControls] = null;
                            Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                            Session[ERP.Utilities.SessionStrings.AccountType] = null;
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                            Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                            //Journalize New sessions End
                            ucrJournalize.TransactionType = RefType;
                            Session[ERP.Utilities.SessionStrings.TransactionType] = RefType;
                            ucrJournalize.TransactionPK = RefPK;

                            if (RefPK > 0)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = RefType == ApplicationType.JV ? null : RefPK.ToString();
                                ucrJournalize.JournalizePK = TrxPK;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = TrxPK;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = "";
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = "";
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = "";
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                            }
                            else
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = null;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = null;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                            }

                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.OBV)
                            {
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                Response.Redirect(Resources.PageURL.OpeningBalance);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV && GetGlobalResourceObject("ConfigurationsRes", "IsShowVoucherDataImport").ToString() == "1")
                            {
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                                Session[ERP.Utilities.SessionStrings.VoucherPk] = TrxPK;
                                Session[ERP.Utilities.SessionStrings.TrxPK] = TrxPK;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = RefType;

                               
                              //  Response.Redirect(Resources.PageURL.JVDetails);
                                string url=Resources.PageURL.JVDetails + "&RefID="+ refID.ToString()+"&Dep="+ currentUser.CurrentDeptPK.ToString();
                                Response.Redirect(url);
                            }
                            else
                            {
                                Session[ERP.Utilities.SessionStrings.JournalType] = RefType;
                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.ViewType = 1;

                                GetFieldValues(ControlsEnum.FINHEADERONE);
                                EntryStatus = EntryStatus.ENTRYMODE;

                                ucrWrkf.ViewAction();
                                Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;

                                HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                                hdfExchangeRateJV.Value = "";

                                TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                                txtJournalExchangeRate.Text = "";

                                TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                                txtNarration.Text = "";

                                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";

                                ucrJournalize.CallUserControl();
                                EntryStatus = EntryStatus.LISTMODE;

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }


                        }
                    }
                    ////
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;

            CommonService commonServiceClient;
            commonServiceClient = null;

            int? Status = null;

            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            try
            {
                switch (type)
                {
                    #region JOURNALIZATION TYPE
                    case ControlsEnum.JOURNALIZATIONTYPE:
                        commonServiceClient = new CommonService();
                        commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        admAppTypeMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_TYPE_MST>();
                        admAppTypeMstObj.APT_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppTypeMstObj.APT_SPL_COND = "J";
                        if (Session[ERP.Utilities.SessionStrings.Type] != null)
                        {
                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YE)
                                admAppTypeMstObj.APT_SPL_COND = "YEJ";
                        }

                        admAppTypeMstList = commonServiceClient.GetAppTypeValues(admAppTypeMstObj);
                        break;
                    #endregion
                    #region FIN HEADER
                    case ControlsEnum.FINHEADER:
                        GetFieldValues(ControlsEnum.JOURNALIZATIONTYPE);

                        admAppTypeMstObj = admAppTypeMstList.SingleOrDefault(app => app.APT_NAME == ddlJournalizeType.SelectedItem.Text);
                        if (admAppTypeMstObj != null)
                        {
                            Session[ERP.Utilities.SessionStrings.Type] = admAppTypeMstObj.APT_CODE;
                        }

                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdJournalizeList.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.JournalizeDate : SortBy;
                        serviceUtilityObj.ThenBy = ThenBy = ThenBy == null ? Resources.DataFieldRes.VoucherNo : ThenBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;
                        if (txtVoucherNo.Text != string.Empty)
                        {
                            serviceUtilityObj.SearchBy = Resources.DataFieldRes.VoucherNo;
                            serviceUtilityObj.SearchValue = txtVoucherNo.Text.TrimStart().TrimEnd(); 
                        }
                        else
                        {
                            serviceUtilityObj.FilterBy = HttpUtility.HtmlEncode(string.Empty);
                            serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtFromDate.Text.Trim());
                            serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? DateTime.MaxValue : Convert.ToDateTime(txtToDate.Text.Trim());
                        }

                        //if (txtPartynameMI.Text != string.Empty)
                        //{
                        //    serviceUtilityObj.SearchBy = Resources.DataFieldRes.Partyname;
                        //    serviceUtilityObj.SearchValue = txtPartynameMI.Text.TrimStart().TrimEnd();
                        //}

                        
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.Type] == null ? "" : Session[ERP.Utilities.SessionStrings.Type].ToString();
                        finTrxHdrObj.FTH_REF_PK = 0;
                        finTrxHdrObj.FTH_PK = TrxPK == 0 ? -1 : TrxPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrObj.FTH_CRTD_BY = currentUser.PKUser;
                        finTrxHdrObj.FTH_BIZUNIT = currentUser.SBUID;
                        finTrxHdrObj.FTH_REF_NO = txtRefNo.Text.TrimStart().TrimEnd();
                        finTrxHdrObj.FTH_PARTY_NAME = txtPartynameMI.Text.TrimStart().TrimEnd();
                        if (Convert.ToInt16(ddlStatus.SelectedValue) == 0)
                        {
                            finTrxHdrObj.FTH_IS_JRNLD = false;
                            finTrxHdrObj.FTH_IS_DELETED = false;
                            serviceUtilityObj.NeedAdvanceFilter = true;
                        }
                        else if (Convert.ToInt16(ddlStatus.SelectedValue) == 1 || Convert.ToInt16(ddlStatus.SelectedValue) == 2 || Convert.ToInt16(ddlStatus.SelectedValue) == 12)
                        {
                            finTrxHdrObj.FTH_IS_JRNLD = true;
                            finTrxHdrObj.FTH_IS_DELETED = false;
                            serviceUtilityObj.NeedAdvanceFilter = true;
                            finTrxHdrObj.FTH_STATUS = Convert.ToByte(ddlStatus.SelectedValue);
                        }
                        else
                        {
                            serviceUtilityObj.NeedAdvanceFilter = false;
                        }

                        if (Convert.ToInt16(ddlStatus.SelectedValue) == -1)
                        {
                            finTrxHdrObj.FTH_IS_DELETED = true;
                            //Status of cancelled item is 4
                            finTrxHdrObj.FTH_STATUS = 4;

                        }
                        else
                            finTrxHdrObj.FTH_IS_DELETED = false;

                        if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CNJ || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.DNJ || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CNTJ || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.DNTJ)
                        {
                            finTrxHdrList = finTrxServiceClient.GetfinTxtHdrListCRDR(finTrxHdrObj, serviceUtilityObj, Session[ERP.Utilities.SessionStrings.SubType].ToString());
                        }
                        else
                        {
                            finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        }                     

                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                      (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                      (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

                        break;
                    #endregion
                    #region FIN HEADER ONE
                    case ControlsEnum.FINHEADERONE:
                        DateTime? FromDate = null;
                        DateTime? ToDate = null;
                        GetFieldValues(ControlsEnum.JOURNALIZATIONTYPE);
                        admAppTypeMstObj = admAppTypeMstList.SingleOrDefault(app => app.APT_NAME == ddlJournalizeType.SelectedItem.Text);
                        if (Request.QueryString["YearEndType"] != null && Request.QueryString["YearEndType"].Length > 1)
                            admAppTypeMstObj.APT_CODE = Request.QueryString["YearEndType"].ToString();
                        if (admAppTypeMstObj != null)
                        {
                            Session[ERP.Utilities.SessionStrings.Type] = admAppTypeMstObj.APT_CODE;
                        }

                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = 1;
                        serviceUtilityObj.PageSize = grdJournalizeList.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.JournalizeDate : SortBy;
                        serviceUtilityObj.ThenBy = ThenBy = ThenBy == null ? Resources.DataFieldRes.VoucherNo : ThenBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;
                        serviceUtilityObj.FilterBy = HttpUtility.HtmlEncode(string.Empty);
                        //serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtFromDate.Text.Trim());
                        //serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtToDate.Text.Trim());
                        serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? FromDate : Convert.ToDateTime(txtFromDate.Text.Trim());
                        serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? ToDate : Convert.ToDateTime(txtToDate.Text.Trim());


                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.Type] == null ? "" : Session[ERP.Utilities.SessionStrings.Type].ToString();
                        finTrxHdrObj.FTH_REF_PK = 0;
                        finTrxHdrObj.FTH_PK = TrxPK == 0 ? -1 : TrxPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                        //if (Convert.ToInt16(ddlStatus.SelectedValue) == 0)
                        //{
                        //    finTrxHdrObj.FTH_IS_JRNLD = false;
                        //    serviceUtilityObj.NeedAdvanceFilter = true;
                        //}
                        //else if (Convert.ToInt16(ddlStatus.SelectedValue) == 1)
                        //{
                        //    finTrxHdrObj.FTH_IS_JRNLD = true;
                        //    serviceUtilityObj.NeedAdvanceFilter = true;
                        //}
                        //else
                        //{
                        //    serviceUtilityObj.NeedAdvanceFilter = false;
                        //}

                        if (Convert.ToInt16(ddlStatus.SelectedValue) == 0)
                        {
                            finTrxHdrObj.FTH_IS_JRNLD = false;
                            finTrxHdrObj.FTH_IS_DELETED = false;
                            serviceUtilityObj.NeedAdvanceFilter = true;
                        }
                        else if (Convert.ToInt16(ddlStatus.SelectedValue) == 1)
                        {
                            finTrxHdrObj.FTH_IS_JRNLD = true;
                            finTrxHdrObj.FTH_IS_DELETED = false;
                            serviceUtilityObj.NeedAdvanceFilter = true;
                        }
                        else
                        {
                            serviceUtilityObj.NeedAdvanceFilter = false;
                        }

                        if (Convert.ToInt16(ddlStatus.SelectedValue) == -1)
                            finTrxHdrObj.FTH_IS_DELETED = true;
                        else
                            finTrxHdrObj.FTH_IS_DELETED = false;

                        if (finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CNJ || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.DNJ || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.CNTJ || finTrxHdrObj.FTH_REF_TYPE == ApplicationType.DNTJ)
                        {
                            finTrxHdrList = finTrxServiceClient.GetfinTxtHdrListCRDR(finTrxHdrObj, serviceUtilityObj, Session[ERP.Utilities.SessionStrings.SubType].ToString());
                        }
                        else
                        {
                            finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        }


                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                      (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                      (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

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
                    #region GETREFPKBYJOURNALPK
                    case ControlsEnum.GETREFPKBYJOURNALPK:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = TrxPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    #endregion
                    //#region FILLWORKFLOWSTATUS
                    //case ControlsEnum.FILLWORKFLOWSTATUS:
                    //    commonServiceClient = new CommonService();
                    //    commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                    //    admConfigMstObj = new ADM_CONFIG_MST();
                    //    admConfigMstObj.CFG_PK = 0;
                    //    admConfigMstObj.CFG_TYPE = GetLocalResourceObject("APPLICATION_STATUS").ToString();
                    //    admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                    //    workflowStatusList = commonServiceClient.GetConfigValues(admConfigMstObj);
                    //    break;
                    //#endregion
                    #region Voucher Templates
                    case ControlsEnum.VOUCHERTEMPLATES:
                        int selectedCategory = 0;
                        int.TryParse(ddlTemplateCategoryList.SelectedValue, out selectedCategory);
                        selectedCategory = selectedCategory > 0 ? selectedCategory : 0;
                        dsTemplateList = BusinessLogic.Jouralize.JournalizeBL.GetVoucherTemplateList(0, Convert.ToByte(CommonConstants.ACTIVE), selectedCategory);
                        break;
                    #endregion
                    #region Voucher Templates Categories
                    case ControlsEnum.VOUCHERTEMPLATECATEGORIES:
                        commonServiceClient = new CommonService();
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        admTemplateCategoryList = commonServiceClient.GetConstMstValues(null, Convert.ToByte(DbActiveStatus.ACTIVE), null, (int)ConstGroupType.VoucherTemplateType, ERP.Utilities.CommonConstants.VoucherTemplate, currentUser.SBUID);
                        break;
                    #endregion
                    #region FILLWORKFLOWSTATUS
                    case ControlsEnum.FILLWORKFLOWSTATUS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        workflowStatusList1 = CommonServiceClient.GetWorkFlowStatus(Session[ERP.Utilities.SessionStrings.Type].ToString(), null, Convert.ToByte(CommonConstants.ACTIVE));
                        break;
                    #endregion

                    #region FIN DIRECT PAYMENT HEADER
                    case ControlsEnum.FINDIRECTPAYMENTHEADER:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                        finTrxHdrObj.FTH_REF_PK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString());
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region VERSION HISTORY
                    case ControlsEnum.VERSIONHISTORY:
                        dtVersions = BusinessLogic.Jouralize.JournalizeBL.GetVoucherVersionHistory(VoucherPk);
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
                commonServiceClient = null;
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
                    #region Fin Header
                    case ControlsEnum.FINHEADER:
                        BindGrid(ControlsEnum.FINHEADER);
                        break;
                    #endregion
                    #region Journalization Type
                    case ControlsEnum.JOURNALIZATIONTYPE:
                        BindDropDown(ControlsEnum.JOURNALIZATIONTYPE);
                        break;
                    #endregion
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        if (finYearMstList != null && finYearMstList.Count > 0)
                        {
                            if (Request.QueryString[QueryStrings.Type] != null && Request.QueryString[QueryStrings.Type] == ApplicationType.YCV)
                            {
                                hdfToDate.Value = txtToDate.Text = hdfFromDate.Value = txtFromDate.Text = string.Empty;
                            }
                            else
                            {
                                txtFromDate.Text = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                                hdfFromDate.Value = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                                txtToDate.Text = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                                hdfToDate.Value = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                            }
                        }
                        break;
                    #endregion
                    #region VoucherTemplates
                    case ControlsEnum.VOUCHERTEMPLATES:
                        BindDropDown(ControlsEnum.VOUCHERTEMPLATES);
                        break;
                    #endregion
                    #region VoucherTemplateCategories
                    case ControlsEnum.VOUCHERTEMPLATECATEGORIES:
                        BindDropDown(ControlsEnum.VOUCHERTEMPLATECATEGORIES);
                        break;
                    #endregion
                    #region VERSION HISTORY
                    case ControlsEnum.VERSIONHISTORY:
                        BindGrid(ControlsEnum.VERSIONHISTORY);
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
        /// 
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
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="refPK"></param>
        /// <param name="isPdcReverse"></param>
        /// <returns></returns>
        private string GetJournalURL(string type, int refPK, bool isPdcReverse, bool isReturn)
        {
            string path = string.Empty;
            int urlType = 1;
            if (type == ApplicationType.CNJ || type == ApplicationType.DNJ || type == ApplicationType.PIJ || type == ApplicationType.SIJ || type == ApplicationType.CNTJ || type == ApplicationType.DNTJ)
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
                case ApplicationType.DPRJ:
                    path = GetLocalResourceObject("DepreciationPath").ToString().ToLower();
                    break;
                case ApplicationType.ACIJ:
                    path = GetLocalResourceObject("AGTCommPath").ToString().ToLower();
                    break;
                case ApplicationType.FCHRJ:
                    path = GetLocalResourceObject("FCReversePath").ToString().ToLower();
                    break;
                case ApplicationType.JV:
                    path = GetLocalResourceObject("JVPath").ToString().ToLower();
                    break;
                case ApplicationType.PCS:
                    path = GetLocalResourceObject("PCSPath").ToString().ToLower();
                    break;
                case ApplicationType.AIPJ:
                case ApplicationType.VPJ:
                    path = GetLocalResourceObject("VPJPath").ToString().ToLower();
                    break;
                case ApplicationType.CRJ:
                    path = GetLocalResourceObject("CRJPath").ToString().ToLower();
                    break;
                case ApplicationType.CNJ:
                    if (urlType == 1)
                        path = GetLocalResourceObject("CNJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetLocalResourceObject("CNJPath2").ToString().ToLower();

                    if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                    {
                        path = GetLocalResourceObject("CNJPath2").ToString().ToLower();
                    }
                    break;
                case ApplicationType.CNTJ:
                    if (urlType == 1)
                        path = GetLocalResourceObject("CNTJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetLocalResourceObject("CNTJPath2").ToString().ToLower();

                    if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                    {
                        path = GetLocalResourceObject("CNJPath2").ToString().ToLower();
                    }
                    break;
                case ApplicationType.DNJ:
                    if (urlType == 1)
                        path = GetLocalResourceObject("DNJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetLocalResourceObject("DNJPath2").ToString().ToLower();
                    if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                    {
                        path = GetLocalResourceObject("DNJPath2").ToString().ToLower();
                    }
                    break;
                case ApplicationType.DNTJ:
                    if (urlType == 1)
                        path = GetLocalResourceObject("DNTJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetLocalResourceObject("DNTJPath2").ToString().ToLower();
                    if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                    {
                        path = GetLocalResourceObject("DNTJPath2").ToString().ToLower();
                    }
                    break;
                case ApplicationType.PIJ:
                    if (urlType == 1)
                        path = GetLocalResourceObject("PIJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetLocalResourceObject("PIJPath2").ToString().ToLower();
                    break;
                case ApplicationType.TPIJ:
                    if (urlType == 1)
                        path = GetLocalResourceObject("TPIJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetLocalResourceObject("TPIJPath2").ToString().ToLower();
                    break;
                case ApplicationType.SIJ:
                    if (urlType == 1)
                        path = GetLocalResourceObject("SIJPath1").ToString().ToLower();
                    else if (urlType == 2)
                        path = GetLocalResourceObject("SIJPath2").ToString().ToLower();
                    break;
                case ApplicationType.EIJ:
                    path = GetLocalResourceObject("EIJPath").ToString().ToLower();
                    break;
                case ApplicationType.PDCCJ:
                    path = GetLocalResourceObject("PDCCJPath").ToString().ToLower();
                    break;
                case ApplicationType.PDCCTJ:
                    path = GetLocalResourceObject("PDCCTJPath").ToString().ToLower();
                    break;
                case ApplicationType.PPCCJ:
                    path = GetLocalResourceObject("PPCCJPath").ToString().ToLower();
                    break;
                case ApplicationType.PPCCTJ:
                    path = GetLocalResourceObject("PPCCTJPath").ToString().ToLower();
                    break;
                case ApplicationType.RCBJ:
                    path = GetLocalResourceObject("RCBJPath").ToString().ToLower();
                    break;
                case ApplicationType.RCBTJ:
                    path = GetLocalResourceObject("RCBTJPath").ToString().ToLower();
                    break;
                case ApplicationType.PCBJ:
                    path = GetLocalResourceObject("PCBJPath").ToString().ToLower();
                    break;
                case ApplicationType.PCBTJ:
                    path = GetLocalResourceObject("PCBTJPath").ToString().ToLower();
                    break;
                case ApplicationType.MSIJ:
                    path = GetLocalResourceObject("MSIJPath").ToString().ToLower();
                    break;
                case ApplicationType.OBV:
                    path = GetLocalResourceObject("OBVPath").ToString().ToLower();
                    break;
                case ApplicationType.DPVJ:
                    path = GetLocalResourceObject("DPVJPath").ToString().ToLower();
                    break;
                case ApplicationType.PCVJ:
                    path = GetLocalResourceObject("PCVJPath").ToString().ToLower();
                    break;
                case ApplicationType.DRVJ:
                    path = GetLocalResourceObject("DRVJPath").ToString().ToLower();
                    break;
                case ApplicationType.YE:
                    path = GetLocalResourceObject("YEPath").ToString().ToLower();
                    break;
                case ApplicationType.DPVCJ:
                    if (isPdcReverse)
                    {
                        path = GetLocalResourceObject("DPVCJReversePath").ToString().ToLower();
                    }
                    else if (isReturn)
                    {
                        path = GetLocalResourceObject("DPBJPath").ToString().ToLower();
                    }
                    else
                    {
                        path = GetLocalResourceObject("DPVCJPath").ToString().ToLower();
                    }
                    break;
                case ApplicationType.DPBJ:
                    path = GetLocalResourceObject("DPBJPath").ToString().ToLower();
                    break;
                case ApplicationType.YCV:
                    path = GetLocalResourceObject("YCVPath").ToString().ToLower();
                    break;
                case ApplicationType.CLSTJ:
                    path = GetLocalResourceObject("CLSTJPath").ToString().ToLower();
                    break;
                case ApplicationType.BDJ:
                    path = GetLocalResourceObject("BDJPath").ToString().ToLower();
                    break;
                case ApplicationType.PAYRLJ:
                    path = GetLocalResourceObject("PAYRLJPath").ToString().ToLower();
                    break;
                case ApplicationType.SALPYMTJ:
                    path = GetLocalResourceObject("SALPYMTJPath").ToString().ToLower();
                    break;
                case ApplicationType.DSIJ:
                    path = GetLocalResourceObject("DSIJPath").ToString().ToLower();
                    break;
                case ApplicationType.EITJ:
                    path = GetLocalResourceObject("EITJPath").ToString().ToLower();
                    break;
                case ApplicationType.AIPTJ:
                case ApplicationType.VPTJ:
                    path = GetLocalResourceObject("VPTJPath").ToString().ToLower();
                    break;
                case ApplicationType.CRTJ:
                    path = GetLocalResourceObject("CRTJPath").ToString().ToLower();
                    break;
                case ApplicationType.MIJ:
                    path = GetLocalResourceObject("MIJPath").ToString().ToLower();
                    break;
                case ApplicationType.CWIPJ:
                    path = GetLocalResourceObject("CWIPJPath").ToString().ToLower();
                    break;
            }
            return path;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="path"></param>
        private void FillProcessID(string Type, string path)
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
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID(string Type, int refPK, bool isCancel = false, bool isPdcReverse = false, bool isReturn = false)
        {
            string path = string.Empty;
            string Cancelpath = string.Empty;
            int urlType = 1;
            if (Type == ApplicationType.CNJ || Type == ApplicationType.DNJ || Type == ApplicationType.PIJ || Type == ApplicationType.SIJ || Type == ApplicationType.CNTJ || Type == ApplicationType.DNTJ)
            {
                if (refPK > 0)
                {
                    FinTrxService finTrxServiceClient;
                    finTrxServiceClient = null;
                    finTrxServiceClient = new FinTrxService();
                    finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                    int retVal = finTrxServiceClient.GetFinType(Type, refPK);
                    finTrxServiceClient = null;
                    if (retVal > 0)
                    {
                        urlType = retVal;
                    }
                }
            }
            switch (Type)
            {
                case ApplicationType.DPRJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("DepreciationPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("DepreciationPath_cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("DepreciationPath_cancel").ToString().ToLower();
                    break;
                case ApplicationType.ACIJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("AGTCommPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("AGTCommPath_cancel1").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("AGTCommPath_cancel1").ToString().ToLower();
                    break;
                case ApplicationType.FCHRJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("FCReversePath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("FCReversePath_cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("FCReversePath_cancel").ToString().ToLower();
                    break;
                case ApplicationType.JV:
                    if (!isCancel)
                        path = GetLocalResourceObject("JVPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("JVPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("JVPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.PCS:
                    if (!isCancel)
                        path = GetLocalResourceObject("PCSPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("PCSPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("PCSPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.AIPJ:
                case ApplicationType.VPJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("VPJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("VPJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("VPJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.CRJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("CRJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("CRJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("CRJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.CNJ:
                    if (!isCancel)
                    {
                        if (urlType == 1)
                            path = GetLocalResourceObject("CNJPath1").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("CNJPath2").ToString().ToLower();

                        if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                        {
                            path = GetLocalResourceObject("CNJPath2").ToString().ToLower();
                        }
                    }
                    else
                    {
                        if (urlType == 1)
                            path = GetLocalResourceObject("CNJPath1_Cancel").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("CNJPath2_Cancel").ToString().ToLower();
                    }
                    if (urlType == 1)
                        Cancelpath = GetLocalResourceObject("CNJPath1_Cancel").ToString().ToLower();
                    else if (urlType == 2)
                        Cancelpath = GetLocalResourceObject("CNJPath2_Cancel").ToString().ToLower();

                    break;
                case ApplicationType.DNJ:
                    if (!isCancel)
                    {

                        if (urlType == 1)
                            path = GetLocalResourceObject("DNJPath1").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("DNJPath2").ToString().ToLower();
                        if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                        {
                            path = GetLocalResourceObject("DNJPath2").ToString().ToLower();
                        }
                    }
                    else
                    {
                        if (urlType == 1)
                            path = GetLocalResourceObject("DNJPath1_Cancel").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("DNJPath2_Cancel").ToString().ToLower();
                    }
                    if (urlType == 1)
                        Cancelpath = GetLocalResourceObject("DNJPath1_Cancel").ToString().ToLower();
                    else if (urlType == 2)
                        Cancelpath = GetLocalResourceObject("DNJPath2_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.PIJ:
                    if (!isCancel)
                    {
                        if (urlType == 1)
                            path = GetLocalResourceObject("PIJPath1").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("PIJPath2").ToString().ToLower();
                    }
                    else
                    {
                        if (urlType == 1)
                            path = GetLocalResourceObject("PIJPath1_Cancel").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("PIJPath2_Cancel").ToString().ToLower();
                    }
                    if (urlType == 1)
                        Cancelpath = GetLocalResourceObject("PIJPath1_Cancel").ToString().ToLower();
                    else if (urlType == 2)
                        Cancelpath = GetLocalResourceObject("PIJPath2_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.SIJ:
                    if (!isCancel)
                    {
                        if (urlType == 1)
                            path = GetLocalResourceObject("SIJPath1").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("SIJPath2").ToString().ToLower();
                    }
                    else
                    {
                        if (urlType == 1)
                            path = GetLocalResourceObject("SIJPath1_Cancel").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("SIJPath2_Cancel").ToString().ToLower();
                    }
                    if (urlType == 1)
                        Cancelpath = GetLocalResourceObject("SIJPath1_Cancel").ToString().ToLower();
                    else if (urlType == 2)
                        Cancelpath = GetLocalResourceObject("SIJPath2_Cancel").ToString().ToLower();
                    break;

                case ApplicationType.ESJ: 
                    if (!isCancel)
                        path = GetLocalResourceObject("ESJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("ESJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("ESJPath_Cancel").ToString().ToLower();
                    break;


                case ApplicationType.EIJ: 
                    if (!isCancel)
                        path = GetLocalResourceObject("EIJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("EIJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("EIJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.PDCCJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("PDCCJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("PDCCJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("PDCCJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.PDCCTJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("PDCCTJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("PDCCTJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("PDCCTJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.PPCCJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("PPCCJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("PPCCJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("PPCCJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.PPCCTJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("PPCCTJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("PPCCTJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("PPCCTJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.RCBJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("RCBJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("RCBJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("RCBJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.RCBTJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("RCBTJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("RCBTJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("RCBTJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.PCBJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("PCBJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("PCBJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("PCBJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.PCBTJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("PCBTJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("PCBTJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("PCBTJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.MSIJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("MSIJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("MSIJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("MSIJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.OBV:
                    path = GetLocalResourceObject("OBVPath").ToString().ToLower();
                    break;
                case ApplicationType.DPVJ:
                    path = GetLocalResourceObject("DPVJPath").ToString().ToLower();
                    break;
                case ApplicationType.PCVJ:
                    path = GetLocalResourceObject("PCVJPath").ToString().ToLower();
                    break;
                case ApplicationType.CTVJ://Contra Entry
                    path = GetLocalResourceObject("CTVJPath").ToString().ToLower();
                    break;
                case ApplicationType.DRVJ:
                    path = GetLocalResourceObject("DRVJPath").ToString().ToLower();
                    break;
                case ApplicationType.YE:
                    if (!isCancel)
                        path = GetLocalResourceObject("YEPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("YEPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("YEPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.DPVCJ:
                    if (isPdcReverse)
                    {
                        path = GetLocalResourceObject("DPVCJReversePath").ToString().ToLower();
                    }
                    else if (isReturn)
                    {
                        path = GetLocalResourceObject("DPBJPath").ToString().ToLower();
                    }
                    else
                    {
                        path = GetLocalResourceObject("DPVCJPath").ToString().ToLower();
                    }
                    if (isCancel)
                    {
                        Cancelpath = path = GetLocalResourceObject("DPVCJPath_Cancel").ToString().ToLower();
                    }
                    break;
                case ApplicationType.DPBJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("DPBJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("DPBJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("DPBJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.YCV:
                    path = GetLocalResourceObject("YCVPath").ToString().ToLower();
                    break;
                case ApplicationType.CLSTJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("CLSTJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("CLSTJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("CLSTJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.PAYRLJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("PAYRLJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("PAYRLJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("PAYRLJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.SALPYMTJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("SALPYMTJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("SALPYMTJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("SALPYMTJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.BDJ:
                    path = GetLocalResourceObject("BDJPath").ToString().ToLower();
                    break;
                case ApplicationType.DSIJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("DSIJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("DSIJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("DSIJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.EITJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("EITJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("EITJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("EITJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.MSITJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("MSITJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("MSITJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("MSITJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.TPIJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("TPIJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("TPIJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("TPIJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.CNTJ:
                    #region CNTJ
		            if (!isCancel)
                    {
                        if (urlType == 1)
                            path = GetLocalResourceObject("CNTJPath1").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("CNTJPath2").ToString().ToLower();

                        if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                        {
                            path = GetLocalResourceObject("CNTJPath2").ToString().ToLower();
                        }
                    }
                    else
                    {
                        if (urlType == 1)
                            path = GetLocalResourceObject("CNTJPath1_Cancel").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("CNTJPath2_Cancel").ToString().ToLower();
                    }
                    if (urlType == 1)
                        Cancelpath = GetLocalResourceObject("CNTJPath1_Cancel").ToString().ToLower();
                    else if (urlType == 2)
                        Cancelpath = GetLocalResourceObject("CNTJPath2_Cancel").ToString().ToLower(); 
	              #endregion
                    break;
                case ApplicationType.DNTJ:
                    #region DNTJ
		           if (!isCancel)
                    {

                        if (urlType == 1)
                            path = GetLocalResourceObject("DNTJPath1").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("DNTJPath2").ToString().ToLower();
                        if ((urlType == 1) && (Session[ERP.Utilities.SessionStrings.SubType].ToString() == ApplicationType.PI))
                        {
                            path = GetLocalResourceObject("DNTJPath2").ToString().ToLower();
                        }
                    }
                    else
                    {
                        if (urlType == 1)
                            path = GetLocalResourceObject("DNTJPath1_Cancel").ToString().ToLower();
                        else if (urlType == 2)
                            path = GetLocalResourceObject("DNTJPath2_Cancel").ToString().ToLower();
                    }
                    if (urlType == 1)
                        Cancelpath = GetLocalResourceObject("DNTJPath1_Cancel").ToString().ToLower();
                    else if (urlType == 2)
                        Cancelpath = GetLocalResourceObject("DNTJPath2_Cancel").ToString().ToLower(); 
	               #endregion
                    break;
                #region VPTJ,AIPTJ
                case ApplicationType.AIPTJ:
                case ApplicationType.VPTJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("VPTJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("VPTJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("VPTJPath_Cancel").ToString().ToLower();
                    break; 
                #endregion
                case ApplicationType.CRTJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("CRTJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("CRTJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("CRTJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.MIJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("MIJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("MIJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("MIJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.CWIPJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("CWIPJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("CWIPJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("CWIPJPath_Cancel").ToString().ToLower();
                    break;
                case ApplicationType.ASDJ:
                    if (!isCancel)
                        path = GetLocalResourceObject("ASDJPath").ToString().ToLower();
                    else
                        path = GetLocalResourceObject("ASDJPath_Cancel").ToString().ToLower();
                    Cancelpath = GetLocalResourceObject("ASDJPath_Cancel").ToString().ToLower();
                    break;
            }

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
                //To Set Cancellation Referance for fill canclation comments
                DataTable dtCancelProcess = wrkfService.GetProcessID(Cancelpath, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
                {
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    ucrWrkf.CancelRefID = workflowCore.GetRefID(TrxPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
                }
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private object SetUIValuesToObject(ControlsEnum controlType)
        {

            try
            {
                Object retObject;
                retObject = null;
                BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                bool bIsChecked = false;

                switch (controlType)
                {
                    #region Finance Header
                    case ControlsEnum.FINHEADER:
                        break;
                    #endregion

                    #region Journalize
                    case ControlsEnum.REVERSE:
                    case ControlsEnum.CHEQUERETURN:
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            foreach (GridViewRow grdrow in grdJournalizeList.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(grdJournalizeList.DataKeys[grdrow.RowIndex].Values[0]);
                                    Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = null;// ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblPartyname")).Text;
                                    break;
                                }
                            }
                        }
                        else if (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.VIEWMODE)
                        {
                            bIsChecked = true;
                        }
                        if (bIsChecked)
                        {
                            //if (Approved == 2)
                            //{
                            Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                            Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                            //Journalize New sessions start
                            Session[ERP.Utilities.SessionStrings.DrControls] = null;
                            Session[ERP.Utilities.SessionStrings.CrControls] = null;
                            Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                            Session[ERP.Utilities.SessionStrings.AccountType] = null;
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                            Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                            //Journalize New sessions End

                            if (controlType == ControlsEnum.REVERSE)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.DPVCJ;
                                ucrJournalize.JournalType = (int)JournalTypeEnum.Reverse;
                            }
                            else if (controlType == ControlsEnum.CHEQUERETURN)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.DPBJ;
                                ucrJournalize.JournalType = (int)JournalTypeEnum.Return;
                            }
                            ucrJournalize.TransactionPK = (int)CurrPK;
                            Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                            ucrJournalize.JournalizePK = 0;
                            Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                            GetFieldValues(ControlsEnum.FINHEADERONE);
                            Session[ERP.Utilities.SessionStrings.TransactionNo] = finTrxHdrList[0].FTH_VOUCHER_NO;
                            Session[ERP.Utilities.SessionStrings.TransactionDate] = finTrxHdrList[0].FTH_DATE;
                            Session[ERP.Utilities.SessionStrings.TransactionCurrency] = finTrxHdrList[0].FTH_BASE_CURR;
                            Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.CR;
                            Session[ERP.Utilities.SessionStrings.AccountPayablePK] = null; // finTrxHdrList[0].RCH_CUSTOMER;
                            Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.DPVJ;


                            ucrWrkf.WrkfSubmit -= ActionHandler;
                            ucrWrkf.Reset();

                            ucrWrkf.ViewType = 1;
                            if (controlType == ControlsEnum.REVERSE)
                            {
                                // FillProcessID(4);
                                // FillProcessID(Session[ERP.Utilities.SessionStrings.Type].ToString(), RefPK);
                                FillProcessID(ApplicationType.DPVCJ, 0, false, true);
                            }
                            else if (controlType == ControlsEnum.CHEQUERETURN)
                            {
                                FillProcessID(ApplicationType.DPBJ, 0, false, false, true);
                            }
                            if (ucrJournalize.TransactionType == ApplicationType.DPVCJ || ucrJournalize.TransactionType == ApplicationType.DPBJ)
                            {
                                GetFieldValues(ControlsEnum.FINDIRECTPAYMENTHEADER);
                            }
                            else
                            {
                                GetFieldValues(ControlsEnum.FINHEADER);
                            }
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                            {
                                ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                base.WkfRefID = ucrWrkf.RefID;
                            }

                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.LISTMODE) && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                                ucrWrkf.ViewType = 1;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                            Session[ERP.Utilities.SessionStrings.JournalMode] = (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.LISTMODE) ? EntryStatus.ENTRYMODE : EntryStatus.VIEWMODE;
                            ucrWrkf.ViewAction();

                            HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                            hdfExchangeRateJV.Value = "";

                            TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                            txtJournalExchangeRate.Text = "";

                            TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                            txtNarration.Text = "";

                            TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                            WrkfComments.Text = "";
                            Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                            //hdfJournalizeWorkFlow.Value = "1";
                            ucrJournalize.CallUserControl();
                            if (controlType == ControlsEnum.REVERSE)
                            {
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Receipt_PDC_Voucher").ToString();
                            }
                            else if (controlType == ControlsEnum.CHEQUERETURN)
                            {
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Receipt_Return_Voucher").ToString();
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            //}
                            //else
                            //{
                            //    litErrorMsg.Text = GetLocalResourceObject("Msg_Journalize_Msg").ToString();
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //}
                        }
                        else
                        {
                            if (EntryStatus == EntryStatus.NEWMODE)
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Journalize_Msg").ToString();
                            else
                                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                        }
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

        /// <summary>
        /// Is Same Vendor
        /// </summary>
        /// <param name="vendors"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameInvoice(List<long> invoices, long pk)
        {

            bool flag = true;
            if (invoices != null)
                foreach (long ven in invoices)
                    if (ven != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
        }

        /// <summary>
        /// I exist Po in list
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsExixtPk(List<long> lst, long pk)
        {
            bool flag = false;
            if (lst != null)
                foreach (long item in lst)
                    if (item == pk)
                    {
                        flag = true;
                        break;
                    }
            return flag;
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
                    case ControlsEnum.FINHEADER:
                        break;
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
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.FINHEADER:
                        if (finTrxHdrList != null)
                        {
                            GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);

                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdJournalizeList.DataSource = finTrxHdrList;
                            grdJournalizeList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();

                            if (ShowVersions)
                                grdJournalizeList.Columns[10].Visible = true;
                            else
                                grdJournalizeList.Columns[10].Visible = false;
                        }
                        break;
                    case ControlsEnum.VERSIONHISTORY:
                        if (dtVersions != null && dtVersions.Rows.Count > 0)
                            grdVersionHistory.DataSource = dtVersions;
                        else
                            grdVersionHistory.DataSource = null;
                        grdVersionHistory.DataBind();
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Method for Dropdown binding
        /// </summary>
        public void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Journalization Types
                    case ControlsEnum.JOURNALIZATIONTYPE:

                        ddlJournalizeType.Items.Clear();
                        if (admAppTypeMstList != null && admAppTypeMstList.Count > 0)
                        {
                            ddlJournalizeType.DataSource = admAppTypeMstList;
                            ddlJournalizeType.DataTextField = Resources.DataFieldRes.AptName;
                            ddlJournalizeType.DataValueField = Resources.DataFieldRes.AptPK;
                            ddlJournalizeType.DataBind();
                        }

                        if (Session[ERP.Utilities.SessionStrings.Type] != null)
                        {
                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YE)
                            {
                                ddlJournalizeType.Enabled = true;
                                ddlJournalizeType.SelectedIndex = 0;
                            }
                            else
                            {
                                ddlJournalizeType.Items.Insert(0,
                               new ListItem(GTIService.Constants.Common.CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                                ddlJournalizeType.SelectedIndex = -1;
                            }
                        }
                        else
                        {
                            ddlJournalizeType.Items.Insert(0,
                               new ListItem(GTIService.Constants.Common.CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                            ddlJournalizeType.SelectedIndex = -1;
                        }
                        if (Session[ERP.Utilities.SessionStrings.Type] != null)
                        {
                            admAppTypeMstObj = admAppTypeMstList.SingleOrDefault(app => app.APT_CODE == Session[ERP.Utilities.SessionStrings.Type].ToString());
                            if (admAppTypeMstObj != null)
                            {
                                ddlJournalizeType.SelectedValue = admAppTypeMstObj.APT_PK.ToString();
                                Session[ERP.Utilities.SessionStrings.JournalHead] = admAppTypeMstObj.APT_NAME.ToString();
                                hdfJournalHeader.Value = admAppTypeMstObj.APT_NAME.ToString();
                            }
                            //for year end
                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YE)
                            {
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("YearEndVoucher").ToString();
                                hdfJournalHeader.Value = GetLocalResourceObject("YearEndVoucher").ToString();
                                if (Request.QueryString[QueryStrings.RefID] != null && !string.IsNullOrEmpty(Request.QueryString[QueryStrings.RefID].ToString()))
                                {
                                    int refid = int.Parse(Request.QueryString[QueryStrings.RefID].ToString());
                                    TrxPK = GetApplicationID(refid);
                                    GetFieldValues(ControlsEnum.GETREFPKBYJOURNALPK);
                                    if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                                    {
                                        admAppTypeMstObj = admAppTypeMstList.SingleOrDefault(app => app.APT_CODE == finTrxHdrList[0].FTH_REF_TYPE);
                                        if (admAppTypeMstObj != null)
                                        {
                                            ddlJournalizeType.SelectedValue = admAppTypeMstObj.APT_PK.ToString();
                                            //Session[ERP.Utilities.SessionStrings.JournalHead] = admAppTypeMstObj.APT_NAME.ToString();
                                            //hdfJournalHeader.Value = admAppTypeMstObj.APT_NAME.ToString();
                                        }
                                    }

                                }
                            }
                        }
                        break;
                    #endregion
                    #region voucher Templates
                    case ControlsEnum.VOUCHERTEMPLATES:
                        ddlTemplates.Items.Clear();
                        if (dsTemplateList != null && dsTemplateList.Tables[0].Rows.Count > 0)
                        {
                            ddlTemplates.DataSource = dsTemplateList.Tables[0].DefaultView;
                            ddlTemplates.DataTextField = "VLH_NAME";
                            ddlTemplates.DataValueField = "VLH_PK";
                            ddlTemplates.DataBind();
                        }
                        ddlTemplates.Items.Insert(0,
                            new ListItem(GTIService.Constants.Common.CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                        break;
                    #endregion
                    #region TEMPLATECATEGORY
                    case ControlsEnum.VOUCHERTEMPLATECATEGORIES:
                        ddlTemplateCategoryList.Items.Clear();
                        if (admTemplateCategoryList != null && admTemplateCategoryList.Count > 0)
                        {
                            ddlTemplateCategoryList.DataSource = admTemplateCategoryList;
                            ddlTemplateCategoryList.DataTextField = Resources.DataFieldRes.ConstName;
                            ddlTemplateCategoryList.DataValueField = Resources.DataFieldRes.ConstPK;
                            ddlTemplateCategoryList.DataBind();
                        }
                        ddlTemplateCategoryList.Items.Insert(0, new ListItem(Resources.Captions.All, ERP.Utilities.CommonConstants.SELECTVAL));
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
                foreach (GridViewRow grdrow in grdJournalizeList.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt32(grdJournalizeList.DataKeys[grdrow.RowIndex].Values[0]);
                        // Get ContactDetails

                        if (Mode == ActionsEnum.VIEW)
                        {
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                        else
                        {
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        return;
                    }
                }
                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            //ddlJournalizeType.SelectedIndex = -1;
            ddlStatus.SelectedValue = "3";
            GetFieldValues(ControlsEnum.FINPERIOD);
            SetFieldValues(ControlsEnum.FINPERIOD);
            base.WkfRefID = 0;
            txtVoucherNo.Text = string.Empty;
            txtRefNo.Text = string.Empty;
            txtPartynameMI.Text = string.Empty;
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

            POInvoiceService poInvoiceServiceClient;
            poInvoiceServiceClient = null;

            POPaymentService poPaymentServiceClient;
            poPaymentServiceClient = null;

            FinCrDrHdrNoteService finCrDrHdrNoteServiceClient;
            finCrDrHdrNoteServiceClient = null;

            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;

            SalesReceiptService salesReceiptServiceClient;
            salesReceiptServiceClient = null;
            try
            {
                GridViewRow selectedGrdrow;
                HiddenField hdfDept;
                int dept;
                int selectedPK;
                Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                bool bIsChecked = false;
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();

                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                int status = 0;


                long result;
                string company = string.Empty;
                string JournalType = string.Empty;


                string _printerMode = "";
                DataSet dsParamSettings;
                string RptType;
                int RptSubType;
                DateTime AppvdDate;

                Session[ERP.Utilities.SessionStrings.Type] = Request.QueryString[QueryStrings.Type] != null ? Request.QueryString[QueryStrings.Type]
                      : Session[ERP.Utilities.SessionStrings.Type] != null ? Session[ERP.Utilities.SessionStrings.Type].ToString().Split('=')[1] : string.Empty;
                hdfJournalizeType.Value = Session[ERP.Utilities.SessionStrings.Type].ToString();


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

                    if (((DropDownList)sender).ID == "ddlTemplateCategoryList")
                    {
                        commonActions = ActionsEnum.VOUCHERTEMPLATECATEGORYSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
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
                    #region ItemSelected
                    case ActionsEnum.ITEMSELECTED:
                        Session[ERP.Utilities.SessionStrings.TransactionCancel] = null;
                        //HiddenField hdfPaymentID;
                        //int pk;
                        int pdc = 0;
                        int trxStatus = 0;
                        selectedGrdrow = (sender as RadioButton).Parent.Parent as GridViewRow;
                        //hdfPaymentID = selectedGrdrow.FindControl("hdfCrDrPk") as HiddenField;
                        //if (hdfPaymentID != null && int.TryParse(hdfPaymentID.Value, out pk))
                        //{
                        //    CurrPK = pk;
                        //}
                        HiddenField hdfIsDeleted = selectedGrdrow.FindControl("hdfIsDeleted") as HiddenField;
                        HiddenField hdfStatus = selectedGrdrow.FindControl("hdfStatus") as HiddenField;
                        hdfJournalStatus.Value = hdfStatus.Value;
                        IsVoucherDeleted = (hdfIsDeleted != null && !string.IsNullOrEmpty(hdfIsDeleted.Value)) ? Convert.ToBoolean(hdfIsDeleted.Value) : false;
                        btnPrint.Visible = true;
                        if (IsVoucherDeleted)
                        {
                            btnSave.Visible = false;
                            btnPrint.Visible = false;
                            btnOffReceiptPrint.Visible = false;
                            btnEdit.Visible = false;
                            btnEditforCancel.Visible = false;
                        }
                        HiddenField hdfRefPK = selectedGrdrow.FindControl("hdfRefPK") as HiddenField;
                        int refPK = hdfRefPK != null && !string.IsNullOrEmpty(hdfRefPK.Value) ? Convert.ToInt32(hdfRefPK.Value) : 0;
                        FillProcessID(Session[ERP.Utilities.SessionStrings.Type].ToString(), refPK);
                        hdfDept = selectedGrdrow.FindControl("hdfDept") as HiddenField;
                        if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                            // base.SetUserDept();
                        }
                        selectedPK = Convert.ToInt32(grdJournalizeList.DataKeys[selectedGrdrow.RowIndex].Values[0]);

                        if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVJ)
                        {
                            trxStatus = Convert.ToInt32((selectedGrdrow.FindControl("hdfTrxFlag") as HiddenField).Value);
                            pdc = Convert.ToInt32((selectedGrdrow.FindControl("hfPdcStatus") as HiddenField).Value);
                            hdfIsReverse.Value = (pdc > 0 && trxStatus > 0) ? "1" : "0";
                            //btnReverse.Visible = (pdc > 0 && trxStatus > 0) ? true : false;
                            TrxPK = Convert.ToInt32(((HiddenField)selectedGrdrow.FindControl("hdfTrxPK")).Value);
                            GetFieldValues(ControlsEnum.FINHEADERONE);
                            if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                            {
                                if (finTrxHdrList[0].FTH_IS_JRNLD)
                                {

                                    if (finTrxHdrList[0].FIN_TRX.Where(r => Convert.ToInt32(r.ADM_CONFIG_MST.CFG_VALUE) == (int)PaymentModeEnum.Cheque).Count() > 0)
                                    {
                                        if (finTrxHdrList[0].FTH_PDC != 0)
                                        {
                                            if (finTrxHdrList[0].FTH_PDC > 1)
                                                hdfShowChequeReturn.Value = "1";
                                            else
                                                hdfShowChequeReturn.Value = "0";
                                        }
                                        else
                                            hdfShowChequeReturn.Value = "1";
                                    }
                                    else
                                    {
                                        hdfShowChequeReturn.Value = "0";
                                    }
                                }
                            }
                        }
                        else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YE)
                        {
                            if (hdfStatus.Value == "0")
                                btnPrint.Visible = false;
                        }

                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = workflowCore.GetRefID(selectedPK, PageProcessID);
                        break;
                    #endregion
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {

                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        Session[ERP.Utilities.SessionStrings.VoucherPk] = null;
                        Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                        if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.OBV)
                        {
                            //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                            Response.Redirect(Resources.PageURL.OpeningBalance);
                        }
                        else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVJ)
                        {
                            //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                            Response.Redirect(Resources.PageURL.DirectPayment);
                        }
                        else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCVJ)
                        {
                            //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                            Response.Redirect(Resources.PageURL.PettyCashDetail); 
                        }
                        else if ( Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCRVJ)
                        {
                            //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                            Response.Redirect(Resources.PageURL.PettyCashReceiptDetail); 
                        }
                        else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DRVJ)
                        {
                            //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                            Response.Redirect(Resources.PageURL.DirectReceiptDetails);
                        }
                        else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YCV)
                        {
                            //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                            Response.Redirect(Resources.PageURL.YearClosingVoucher);
                        }
                            //Contra entry voucher
                        else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.CTVJ)
                        {
                            //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                            Response.Redirect(Resources.PageURL.ContraVoucherDetails);
                        }
                        else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV && GetGlobalResourceObject("ConfigurationsRes", "IsShowVoucherDataImport").ToString() == "1")
                        {
                            Response.Redirect(Resources.PageURL.JournalVoucher);
                        }
                        else
                        {
                            RefPK = 0;
                            ucrJournalize.VoucherTemplatePK = 0;
                            RefType = Session[ERP.Utilities.SessionStrings.Type].ToString();
                            Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                            //Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                            //Journalize New sessions start
                            Session[ERP.Utilities.SessionStrings.DrControls] = null;
                            Session[ERP.Utilities.SessionStrings.CrControls] = null;
                            Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                            Session[ERP.Utilities.SessionStrings.AccountType] = null;
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                            Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                            //Journalize New sessions End
                            ucrJournalize.TransactionType = RefType;
                            Session[ERP.Utilities.SessionStrings.TransactionType] = RefType;
                            ucrJournalize.TransactionPK = RefPK;
                            Session[ERP.Utilities.SessionStrings.TransactionPK] = null;
                            ucrJournalize.JournalizePK = 0;
                            Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                            Session[ERP.Utilities.SessionStrings.TransactionNo] = null;
                            Session[ERP.Utilities.SessionStrings.TransactionDate] = null;
                            Session[ERP.Utilities.SessionStrings.TransactionCurrency] = null;
                            Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                            Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                            Session[ERP.Utilities.SessionStrings.JournalType] = RefType;
                            ucrWrkf.WrkfSubmit -= ActionHandler;
                            ucrWrkf.Reset();
                            ucrWrkf.ViewType = 1;

                            FillProcessID(Session[ERP.Utilities.SessionStrings.Type].ToString(), RefPK);

                            EntryStatus = EntryStatus.ENTRYMODE;
                            base.WkfRefID = ucrWrkf.RefID = 0;
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrWrkf.ViewType = 1;
                                //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                                //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                            }
                            ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                            //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                            ucrWrkf.ViewAction();
                            //Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                            //Session[ERP.Utilities.SessionStrings.RemoveHdfIdDr] = null;
                            //Session[ERP.Utilities.SessionStrings.RemoveHdfIdCr] = null;

                            HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                            hdfExchangeRateJV.Value = "";

                            TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                            txtJournalExchangeRate.Text = "";

                            TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                            txtNarration.Text = "";

                            TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                            WrkfComments.Text = "";

                            ucrJournalize.CallUserControl();
                            EntryStatus = EntryStatus.LISTMODE;
                            if (!ucrJournalize.CloseVoucherPopup)
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                        }
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        PageIndex = null;
                        TrxPK = 0;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;

                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdJournalizeList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                RefPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRefPK")).Value);
                                TrxPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTrxPK")).Value);
                                RefType = ((HiddenField)grdrow.FindControl("hdfRefType")).Value;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            Session[ERP.Utilities.SessionStrings.VoucherPk] = TrxPK;
                            Session[ERP.Utilities.SessionStrings.TrxPK] = TrxPK;
                            Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                            Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                            //Journalize New sessions start
                            Session[ERP.Utilities.SessionStrings.DrControls] = null;
                            Session[ERP.Utilities.SessionStrings.CrControls] = null;
                            Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                            Session[ERP.Utilities.SessionStrings.AccountType] = null;
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                            Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                            //Journalize New sessions End
                            ucrJournalize.TransactionType = RefType;
                            Session[ERP.Utilities.SessionStrings.TransactionType] = RefType;
                            ucrJournalize.TransactionPK = RefPK;

                            if (RefPK > 0)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = RefType == ApplicationType.JV ? null : RefPK.ToString();
                                ucrJournalize.JournalizePK = TrxPK;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = TrxPK;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = "";
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = "";
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = "";
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                            }
                            else
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = null;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = null;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                            }
                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PDCCJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PDCCTJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PPCCJ)
                            {
                                ucrJournalize.JournalType = (int)JournalTypeEnum.Reverse;
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.RCBJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.RCBTJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCBJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPBJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCBTJ)
                            {
                                ucrJournalize.JournalType = (int)JournalTypeEnum.Return;
                            }
                            Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.OBV)
                            {
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                Response.Redirect(Resources.PageURL.OpeningBalance);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVJ)
                            {
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                Response.Redirect(Resources.PageURL.DirectPayment);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCVJ)
                            {
                               
                                Response.Redirect(Resources.PageURL.PettyCashDetail);
                            }

                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCRVJ)
                            {

                                Response.Redirect(Resources.PageURL.PettyCashReceiptDetail);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV && GetGlobalResourceObject("ConfigurationsRes", "IsShowVoucherDataImport").ToString() == "1")
                            {
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                Response.Redirect(Resources.PageURL.JVDetails);
                            }
                                //Contra Entry
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.CTVJ)
                            {
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                Response.Redirect(Resources.PageURL.ContraVoucherDetails);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DRVJ)
                            {
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                Response.Redirect(Resources.PageURL.DirectReceiptDetails);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YCV)
                            {
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                Response.Redirect(Resources.PageURL.YearClosingVoucher);
                            }

                            else
                            {
                                Session[ERP.Utilities.SessionStrings.JournalType] = RefType;
                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.Reset();
                                ucrWrkf.ViewType = 1;
                                if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVCJ)
                                {
                                    FillProcessID(ApplicationType.DPVCJ, RefPK, false, true);
                                }
                                else
                                {
                                    FillProcessID(Session[ERP.Utilities.SessionStrings.Type].ToString(), RefPK);
                                }

                                GetFieldValues(ControlsEnum.FINHEADERONE);
                                EntryStatus = EntryStatus.ENTRYMODE;

                                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                {
                                    ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                                    ucrWrkf.ViewType = 1;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                    //EntryStatus = EntryStatus.VIEWMODE;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                }
                                ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                                //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                ucrWrkf.ViewAction();
                                Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;

                                HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                                hdfExchangeRateJV.Value = "";

                                TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                                txtJournalExchangeRate.Text = "";

                                TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                                txtNarration.Text = "";

                                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";
                                ucrJournalize.VoucherDeleteStatus = ddlStatus.SelectedValue == "-1" ? true : false;
                                ucrJournalize.CallUserControl();
                                EntryStatus = EntryStatus.LISTMODE;

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:

                        foreach (GridViewRow grdrow in grdJournalizeList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                RefPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRefPK")).Value);
                                TrxPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTrxPK")).Value);
                                RefType = ((HiddenField)grdrow.FindControl("hdfRefType")).Value;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            Session[ERP.Utilities.SessionStrings.VoucherPk] = TrxPK;
                            Session[ERP.Utilities.SessionStrings.TrxPK] = TrxPK;
                            Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                            Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                            //Journalize New sessions start
                            Session[ERP.Utilities.SessionStrings.DrControls] = null;
                            Session[ERP.Utilities.SessionStrings.CrControls] = null;
                            Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                            Session[ERP.Utilities.SessionStrings.AccountType] = null;
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                            Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                            //Journalize New sessions End
                            ucrJournalize.TransactionType = RefType;
                            Session[ERP.Utilities.SessionStrings.TransactionType] = RefType;
                            ucrJournalize.TransactionPK = RefPK;

                            if (RefPK > 0)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = RefType == ApplicationType.JV ? null : RefPK.ToString();
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = "";
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = "";
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = "";
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                            }
                            else
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = null;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = null;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                            }
                            Session[ERP.Utilities.SessionStrings.JournalType] = RefType;
                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PDCCJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PDCCTJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PPCCJ)
                            {
                                ucrJournalize.JournalType = (int)JournalTypeEnum.Reverse;
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.RCBJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.RCBTJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCBJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPBJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCBTJ)
                            {
                                ucrJournalize.JournalType = (int)JournalTypeEnum.Return;
                            }
                            Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                            Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;

                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.OBV)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.OpeningBalance);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVJ)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.DirectPayment);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV && GetGlobalResourceObject("ConfigurationsRes", "IsShowVoucherDataImport").ToString() == "1")
                            {
                               // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                Response.Redirect(Resources.PageURL.JVDetails);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCVJ)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.PettyCashDetail);
                            }

                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCRVJ)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.PettyCashReceiptDetail);
                            }
                            //Contra Entry
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.CTVJ)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.ContraVoucherDetails);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DRVJ)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.DirectReceiptDetails);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YCV)
                            {
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                Response.Redirect(Resources.PageURL.YearClosingVoucher);
                            }
                            else
                            {
                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.Reset();
                                ucrWrkf.ViewType = 1;
                                if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVCJ)
                                {
                                    FillProcessID(ApplicationType.DPVCJ, RefPK, false, true);
                                }
                                else
                                {
                                    FillProcessID(Session[ERP.Utilities.SessionStrings.Type].ToString(), RefPK);
                                }

                                GetFieldValues(ControlsEnum.FINHEADERONE);

                                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                {
                                    ucrWrkf.ViewType = 1;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                    //EntryStatus = EntryStatus.VIEWMODE;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                }
                                ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                                //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                ucrWrkf.ViewAction();
                                Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;

                                HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                                hdfExchangeRateJV.Value = "";

                                TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                                txtJournalExchangeRate.Text = "";

                                TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                                txtNarration.Text = "";

                                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";
                                ucrJournalize.VoucherDeleteStatus = finTrxHdrList[0].FTH_IS_DELETED == true ? true : false;
                                //ucrJournalize.VoucherDeleteStatus = ddlStatus.SelectedValue == "-1" ? true : false;
                                ucrJournalize.CallUserControl();
                                EntryStatus = EntryStatus.LISTMODE;

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        break;
                    #endregion
                    #region Print
                    case ActionsEnum.ADVANCEINVOICE:
                    case ActionsEnum.OFFRECPRINT:
                    case ActionsEnum.PRINT:
                        company = string.Empty;
                        JournalType = string.Empty;
                        foreach (GridViewRow grdrow in grdJournalizeList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                RefPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRefPK")).Value);
                                TrxPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTrxPK")).Value);
                                status = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                company = ((HiddenField)grdrow.FindControl("hdfCompany")).Value;
                                JournalType = ((HiddenField)grdrow.FindControl("hdfRefType")).Value;
                                GetFieldValues(ControlsEnum.JOURNALIZATIONTYPE);
                                admAppTypeMstObj = admAppTypeMstList.SingleOrDefault(app => app.APT_NAME == ddlJournalizeType.SelectedItem.Text);
                                if (admAppTypeMstObj != null)
                                {
                                    Session[ERP.Utilities.SessionStrings.Type] = admAppTypeMstObj.APT_CODE;
                                }
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (commonActions == ActionsEnum.PRINT)
                            {
                                _printerMode = "";
                                #region Getting Printer Mode
                                dsParamSettings = new DataSet();
                                RptType = Session[ERP.Utilities.SessionStrings.Type].ToString();
                                RptSubType = 0;
                                Int32.TryParse(hdfAppSubType.Value, out RptSubType);
                                AppvdDate = DateTime.Now;

                                List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList = new CommonService().GetReportParameters(RptType, RptSubType, AppvdDate);

                                if (AppTypeDetailsList != null && AppTypeDetailsList.Count > 0)
                                {
                                    dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(AppTypeDetailsList[0].AST_RPT_SETTINGS)));

                                    if (dsParamSettings.Tables.Count > 0 && dsParamSettings.Tables[0].Columns.Contains("PRINTER_MODE"))
                                    {
                                        _printerMode = dsParamSettings.Tables[0].Rows[0]["PRINTER_MODE"].ToString();
                                    }
                                }
                                #endregion

                                if (_printerMode == PrinterMode.DOTMATRIX.ToString()) // && Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV )
                                {
                                    string redirectUrl = string.Empty;
                                    if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV
                                        || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCS
                                        || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVJ
                                        || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCVJ)
                                    {
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + TrxPK.ToString() + "&APPTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() +
                                        //    "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() + "&COMPANY=" + company + "');", true);

                                        redirectUrl = string.Format("../Reports/GenerateReport.aspx?ID={0}&APPTYPE={1}&APPSUBTYPE={2}&TRXTYPE={3}&COMPANY={4}&PRINTERMODE={5}",
                                                                                                  TrxPK.ToString(),
                                                                                                  Session[ERP.Utilities.SessionStrings.Type].ToString(),
                                                                                                  hdfAppSubType.Value,
                                                                                                  Session[ERP.Utilities.SessionStrings.Type].ToString(),
                                                                                                  company,
                                                                                                  _printerMode);
                                    }
                                    else
                                    {
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + RefPK.ToString() + "&APPTYPE=" + JournalType +
                                        //    "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + JournalType + "&COMPANY=" + company + "');", true);// 

                                        redirectUrl = string.Format("../Reports/GenerateReport.aspx?ID={0}&APPTYPE={1}&APPSUBTYPE={2}&TRXTYPE={3}&COMPANY={4}&PRINTERMODE={5}",
                                                                                                 RefPK.ToString(),
                                                                                                 JournalType,
                                                                                                 hdfAppSubType.Value,
                                                                                                 JournalType,
                                                                                                 company,
                                                                                                 _printerMode);

                                    }
                                    TrxPK = 0;
                                    Response.Redirect(redirectUrl, false);
                                }
                                else
                                {
                                    if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV
                                        || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCS
                                        || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVJ
                                        || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCVJ
                                         || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.CTVJ
                                        || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DRVJ
                                        || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YCV
                                         || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCRVJ)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + TrxPK.ToString() + "&APPTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() +
                                            "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() + "&COMPANY=" + company + "');", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + RefPK.ToString() + "&APPTYPE=" + JournalType +
                                            "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + JournalType + "&COMPANY=" + company + "');", true);// 

                                    }
                                }
                                TrxPK = 0;
                                #region Commented
                                // Response.Redirect("../Reports/GenerateReport.aspx?ID=" + RefPK.ToString() + "&APPTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() +
                                //     "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString());
                                //if (status != 0)
                                //{


                                ///////Commented By Biju{
                                ////if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCS || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVJ || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCVJ)
                                ////{
                                ////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + TrxPK.ToString() + "&APPTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() +
                                ////        "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() + "&COMPANY=" + company + "');", true);
                                ////}
                                ////else
                                ////{
                                ////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + RefPK.ToString() + "&APPTYPE=" + JournalType +
                                ////        "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + JournalType + "&COMPANY=" + company + "');", true);// 

                                ////}
                                ///////}

                                //}
                                //else
                                //{
                                //    litErrorMsg.Text = GetLocalResourceObject("ReportError").ToString();
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                //} 
                                #endregion
                            }
                            else if (commonActions == ActionsEnum.OFFRECPRINT)
                            {
                                //Response.Redirect("../Reports/GenerateReport.aspx?ID=" + RefPK.ToString() + "&APPTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() +
                                //   "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.OFFICIALRECEIPT) + "&TRXTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString());
                                //if (status != 0)
                                //{
                                if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DRVJ)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + TrxPK.ToString() + "&APPTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() +
                                          "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.OFFICIALRECEIPT) + "&TRXTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() + "&COMPANY=" + company + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + RefPK.ToString() + "&APPTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() +
                                            "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.OFFICIALRECEIPT) + "&TRXTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() + "&COMPANY=" + company + "');", true);
                                }

                                //}
                                //else
                                //{
                                //    litErrorMsg.Text = GetLocalResourceObject("ReportError").ToString();
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                //}

                            }
                            else if (commonActions == ActionsEnum.ADVANCEINVOICE)
                            {
                                //Response.Redirect("../Reports/GenerateReport.aspx?ID=" + RefPK.ToString() + "&APPTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() +
                                //   "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + RefPK.ToString() + "&APPTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() +
                                   "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() + "&COMPANY=" + company + "');", true);
                            }
                            TrxPK = 0;
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                        TrxPK = 0;
                        ucrJournalize.ResetForm();
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        FillProcessAfterJournalClose();
                        CurrPK = 0;
                        base.WkfRefID = 0;
                        //ucrJournalize.ActionHandler(new Button() { CommandName = ActionsEnum.CANCEL.ToString() }, e);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.REVERSESAVE:
                    case ActionsEnum.JOURNALIZESAVE:
                    case ActionsEnum.RETURNSAVE:
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                        TrxPK = 0;
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        FillProcessAfterJournalClose();
                        CurrPK = 0;
                        base.WkfRefID = 0;
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PIJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PSIJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.EIJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.EITJ)
                                {
                                    poInvoiceServiceClient = new POInvoiceService();
                                    poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                    result = poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                                }
                                else if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.VPJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.SIPJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.EIPJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.AIPJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.VPTJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.SIPTJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.EIPTJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.AIPTJ)
                                {
                                    poPaymentServiceClient = new POPaymentService();
                                    poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                                    result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                                }
                                else if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.CNJ || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.DNJ ||
                                         Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.CNTJ || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.DNTJ )
                                {
                                    finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                    finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                    result = finCrDrHdrNoteServiceClient.UpdateCrDrHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                                }
                                else if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.SIJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.MSIJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.DSIJ)
                                {
                                    salesInvoiceServiceClient = new SalesInvoiceService();
                                    salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                    result = salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                                }
                                else if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.CRJ)
                                {
                                    salesReceiptServiceClient = new SalesReceiptService();
                                    salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                    result = salesReceiptServiceClient.UpdateReceiptHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                                }
                            }
                        }
                        ////if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        ////{

                        ////    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ////    Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                        ////    TrxPK = 0;
                        ////    Response.Redirect(Resources.PageURL.InboxURL);
                        ////}
                        ////else
                        ////{
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                        TrxPK = 0;
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        FillProcessAfterJournalClose();
                        CurrPK = 0;
                        base.WkfRefID = 0;
                        //}
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "DELETE")
                            {
                                if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PIJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.PSIJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.EIJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.EITJ)
                                {
                                    poInvoiceServiceClient = new POInvoiceService();
                                    poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                    result = poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                                }
                                else if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.VPJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.SIPJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.EIPJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.AIPJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.VPTJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.SIPTJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.EIPTJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.AIPTJ)
                                {
                                    poPaymentServiceClient = new POPaymentService();
                                    poPaymentServiceClient = CommonFunctions.InitiateClient(poPaymentServiceClient);
                                    result = poPaymentServiceClient.UpdatePaymentHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                                }
                                else if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.CNJ || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.DNJ ||
                                         Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.CNTJ||Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.DNJ)
                                {
                                    finCrDrHdrNoteServiceClient = new FinCrDrHdrNoteService();
                                    finCrDrHdrNoteServiceClient = CommonFunctions.InitiateClient(finCrDrHdrNoteServiceClient);
                                    result = finCrDrHdrNoteServiceClient.UpdateCrDrHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                                }
                                else if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.SIJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.MSIJ
                                    || Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.DSIJ)
                                {
                                    salesInvoiceServiceClient = new SalesInvoiceService();
                                    salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                    result = salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                                }
                                else if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() == ApplicationType.CRJ)
                                {
                                    salesReceiptServiceClient = new SalesReceiptService();
                                    salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                    result = salesReceiptServiceClient.UpdateReceiptHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                                }
                            }
                        }
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                        TrxPK = 0;
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        FillProcessAfterJournalClose();
                        CurrPK = 0;
                        base.WkfRefID = 0;
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.REVERSECANCEL:
                    case ActionsEnum.JOURNALIZECANCEL:
                    case ActionsEnum.RETURNCANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                        TrxPK = 0;
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        FillProcessAfterJournalClose();
                        CurrPK = 0;
                        base.WkfRefID = 0;
                        break;
                    #endregion
                    #region REVERSESUBMIT
                    case ActionsEnum.REVERSESUBMIT:
                    case ActionsEnum.RETURNSUBMIT:
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                        TrxPK = 0;
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        FillProcessAfterJournalClose();
                        CurrPK = 0;
                        base.WkfRefID = 0;
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.REVERSEDELETE:
                    case ActionsEnum.RETURNDELETE:
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                        TrxPK = 0;
                        GetFieldValues(ControlsEnum.FINHEADER);
                        SetFieldValues(ControlsEnum.FINHEADER);
                        FillProcessAfterJournalClose();
                        CurrPK = 0;
                        base.WkfRefID = 0;
                        break;
                    #endregion
                    #region New From Template
                    case ActionsEnum.NEWFROMTEMPLATE:
                        GetFieldValues(ControlsEnum.VOUCHERTEMPLATECATEGORIES);
                        SetFieldValues(ControlsEnum.VOUCHERTEMPLATECATEGORIES);
                        GetFieldValues(ControlsEnum.VOUCHERTEMPLATES);
                        SetFieldValues(ControlsEnum.VOUCHERTEMPLATES);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTemplateList", "ShowContainerDiv('[id$=divTemplateList]','" + GetLocalResourceObject("Voucher_Template").ToString() + "','600','150');", true);
                        break;
                    #endregion
                    #region LOADFROMTEMPLATE
                    case ActionsEnum.LOADFROMTEMPLATE:
                        int templatePK = 0;
                        if (int.TryParse(ddlTemplates.SelectedValue, out templatePK))
                        {
                            if (templatePK > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseTemplateList",
                                          "ClosePopup();", true);
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                                if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.OBV)
                                {
                                    Response.Redirect(Resources.PageURL.OpeningBalance);
                                }
                                else
                                {
                                    RefPK = 0;
                                    ucrJournalize.VoucherTemplatePK = templatePK;

                                    RefType = Session[ERP.Utilities.SessionStrings.Type].ToString();
                                    Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                                    Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                                    Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                                    Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                                    //Journalize New sessions start
                                    Session[ERP.Utilities.SessionStrings.DrControls] = null;
                                    Session[ERP.Utilities.SessionStrings.CrControls] = null;
                                    Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                                    Session[ERP.Utilities.SessionStrings.AccountType] = null;
                                    Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                                    Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                                    //Journalize New sessions End
                                    ucrJournalize.TransactionType = RefType;
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = RefType;
                                    ucrJournalize.TransactionPK = RefPK;
                                    Session[ERP.Utilities.SessionStrings.TransactionPK] = null;
                                    ucrJournalize.JournalizePK = 0;
                                    Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                    Session[ERP.Utilities.SessionStrings.TransactionNo] = null;
                                    Session[ERP.Utilities.SessionStrings.TransactionDate] = null;
                                    Session[ERP.Utilities.SessionStrings.TransactionCurrency] = null;
                                    Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                    Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                                    Session[ERP.Utilities.SessionStrings.JournalType] = RefType;
                                    ucrWrkf.WrkfSubmit -= ActionHandler;
                                    ucrWrkf.Reset();
                                    ucrWrkf.ViewType = 1;

                                    FillProcessID(Session[ERP.Utilities.SessionStrings.Type].ToString(), RefPK);

                                    EntryStatus = EntryStatus.ENTRYMODE;
                                    base.WkfRefID = ucrWrkf.RefID = 0;
                                    ucrWrkf.FillWorkFlowDetails();
                                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                    {
                                        ucrWrkf.ViewType = 1;
                                        //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                                    }
                                    else
                                    {
                                        ucrWrkf.ViewType = 0;
                                        //EntryStatus = EntryStatus.VIEWMODE;
                                        //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                    }
                                    ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.NEWMODE;
                                    ucrWrkf.ViewAction();
                                    Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                                    Session[ERP.Utilities.SessionStrings.RemoveHdfIdDr] = null;
                                    Session[ERP.Utilities.SessionStrings.RemoveHdfIdCr] = null;

                                    HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                                    hdfExchangeRateJV.Value = "";

                                    TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                                    txtJournalExchangeRate.Text = "";

                                    TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                                    txtNarration.Text = "";

                                    TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    WrkfComments.Text = "";
                                    ucrJournalize.CallUserControl();
                                    EntryStatus = EntryStatus.LISTMODE;

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTemplateList", "ShowContainerDiv('[id$=divTemplateList]','" + GetLocalResourceObject("Voucher_Template").ToString() + "','600','150');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTemplateList", "ShowContainerDiv('[id$=divTemplateList]','" + GetLocalResourceObject("Voucher_Template").ToString() + "','600','150');", true);
                        }
                        break;
                    #endregion
                    #region VOUCHERTEMPLATECATEGORYSELECTED
                    case ActionsEnum.VOUCHERTEMPLATECATEGORYSELECTED:
                        GetFieldValues(ControlsEnum.VOUCHERTEMPLATES);
                        SetFieldValues(ControlsEnum.VOUCHERTEMPLATES);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTemplateList", "ShowContainerDiv('[id$=divTemplateList]','" + GetLocalResourceObject("Voucher_Template").ToString() + "','600','150');", true);
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdJournalizeList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                RefPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRefPK")).Value);
                                TrxPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTrxPK")).Value);
                                RefType = ((HiddenField)grdrow.FindControl("hdfRefType")).Value;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            Session[ERP.Utilities.SessionStrings.VoucherPk] = TrxPK;
                            Session[ERP.Utilities.SessionStrings.TrxPK] = TrxPK;
                            Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                            Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                            //Journalize New sessions start
                            Session[ERP.Utilities.SessionStrings.DrControls] = null;
                            Session[ERP.Utilities.SessionStrings.CrControls] = null;
                            Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                            Session[ERP.Utilities.SessionStrings.AccountType] = null;
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                            Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                            //Journalize New sessions End
                            ucrJournalize.TransactionType = RefType;
                            Session[ERP.Utilities.SessionStrings.TransactionType] = RefType;
                            ucrJournalize.TransactionPK = RefPK;

                            if (RefPK > 0)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = RefType == ApplicationType.JV ? null : RefPK.ToString();
                                ucrJournalize.JournalizePK = TrxPK;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = TrxPK;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = "";
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = "";
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = "";
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                            }
                            else
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = null;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = null;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = "";
                            }
                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PDCCJ
                                 || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PDCCTJ
                                 || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PPCCJ
                                 || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVCJ)
                            {
                                ucrJournalize.JournalType = (int)JournalTypeEnum.Reverse;
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.RCBJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.RCBTJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCBJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCBTJ)
                            {
                                ucrJournalize.JournalType = (int)JournalTypeEnum.Return;
                            }
                            Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.OBV)
                            {

                                Session[ERP.Utilities.SessionStrings.TransactionCancel] = TransactionType.CANCELATION;
                                Response.Redirect(Resources.PageURL.OpeningBalance);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVJ)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionCancel] = TransactionType.CANCELATION;
                                Response.Redirect(Resources.PageURL.DirectPayment);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCVJ)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionCancel] = TransactionType.CANCELATION;
                                Response.Redirect(Resources.PageURL.PettyCashDetail);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCRVJ)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionCancel] = TransactionType.CANCELATION;
                                Response.Redirect(Resources.PageURL.PettyCashReceiptDetail);
                            }

                            //Contra Entry
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.CTVJ)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionCancel] = TransactionType.CANCELATION;
                                Response.Redirect(Resources.PageURL.ContraVoucherDetails);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DRVJ)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionCancel] = TransactionType.CANCELATION;
                                Response.Redirect(Resources.PageURL.DirectReceiptDetails);
                            }
                            else if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YCV)
                            {
                                Session[ERP.Utilities.SessionStrings.TransactionCancel] = TransactionType.CANCELATION;
                                Response.Redirect(Resources.PageURL.YearClosingVoucher);
                            }
                            else
                            {
                                Session[ERP.Utilities.SessionStrings.JournalType] = RefType;
                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.Reset();
                                ucrWrkf.ViewType = 1;

                                FillProcessID(Session[ERP.Utilities.SessionStrings.Type].ToString(), RefPK, true);

                                GetFieldValues(ControlsEnum.FINHEADERONE);
                                EntryStatus = EntryStatus.ENTRYMODE;

                                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                {
                                    ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                                    ucrWrkf.ViewType = 1;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                    //EntryStatus = EntryStatus.VIEWMODE;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                }
                                ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                                //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                ucrWrkf.ViewAction();
                                Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;

                                HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                                hdfExchangeRateJV.Value = "";

                                TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                                txtJournalExchangeRate.Text = "";

                                TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                                txtNarration.Text = "";

                                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";

                                ucrJournalize.CallUserControl();
                                EntryStatus = EntryStatus.LISTMODE;

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region REVERSE
                    case ActionsEnum.REVERSE:
                        foreach (GridViewRow grdrow in grdJournalizeList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                RefPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRefPK")).Value);
                                TrxPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTrxPK")).Value);
                                RefType = ((HiddenField)grdrow.FindControl("hdfRefType")).Value;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            finTrxHdrObj = CommonFunctions.Initilize<FIN_TRX_HDR>();
                            finTrxHdrObj.FTH_PK = TrxPK;
                            GetFieldValues(ControlsEnum.FINHEADERONE);
                            if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                            {
                                if (finTrxHdrList[0].FTH_PDC >= 1)
                                {
                                    if (finTrxHdrList[0].FTH_PDC == 1)
                                    {
                                        FinTrxService finTrxServiceClient;
                                        finTrxServiceClient = new FinTrxService();
                                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                        result = finTrxServiceClient.GenerateDummyEntry((int)TrxPK, ApplicationType.DPVCJ);
                                    }
                                    SetUIValuesToObject(ControlsEnum.REVERSE);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ReverseEntry_Not_PDC").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    break;
                                }
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region CHEQUERETURN
                    case ActionsEnum.CHEQUERETURN:
                        foreach (GridViewRow grdrow in grdJournalizeList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                RefPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRefPK")).Value);
                                TrxPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTrxPK")).Value);
                                RefType = ((HiddenField)grdrow.FindControl("hdfRefType")).Value;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {

                            GetFieldValues(ControlsEnum.FINHEADERONE);
                            if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                            {

                                if (finTrxHdrList[0].FTH_IS_JRNLD)
                                {
                                    #region Check whether the cheque return is possible or not
                                    bool IsReturnSuccess = true;
                                    FIN_TRX objTrxLst = finTrxHdrList[0].FIN_TRX.Where(r => Convert.ToInt32(r.ADM_CONFIG_MST.CFG_VALUE) == (int)PaymentModeEnum.Cheque).FirstOrDefault();
                                    if (objTrxLst != null)
                                    {
                                        bool IsDebit = objTrxLst.FTR_DR_AMT_BC > 0 ? true : false;
                                        if (finTrxHdrList[0].FIN_TRX.Where(r => Convert.ToInt32(r.ADM_CONFIG_MST.CFG_VALUE) != (int)PaymentModeEnum.Cheque && (IsDebit ? r.FTR_DR_AMT_BC > 0 : r.FTR_CR_AMT_BC > 0)).Count() > 0)
                                        {
                                            IsReturnSuccess = false;
                                        }
                                        else if (finTrxHdrList[0].FTH_PDC > 0)
                                        {
                                            if (finTrxHdrList[0].FIN_TRX.Where(r => r.FTR_PDC == 0 && (IsDebit ? r.FTR_DR_AMT_BC > 0 : r.FTR_CR_AMT_BC > 0)).Count() > 0)
                                                IsReturnSuccess = false;
                                        }
                                        if (!IsReturnSuccess)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("MsgErr_ReturnEntry_Multi_Mode").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                    }
                                    #endregion

                                    bool IsSuccess = false;
                                    if (finTrxHdrList[0].FIN_TRX.Where(r => Convert.ToInt32(r.ADM_CONFIG_MST.CFG_VALUE) == (int)PaymentModeEnum.Cheque).Count() > 0)
                                    {
                                        if (finTrxHdrList[0].FTH_PDC != 0)
                                        {
                                            if (finTrxHdrList[0].FTH_PDC > 1)
                                                IsSuccess = true;
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_ReturnEntry_Is_PDC").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                        }
                                        else
                                            IsSuccess = true;
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_ReturnEntry_Not_Cheque").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                    if (IsSuccess)
                                    {
                                        if (finTrxHdrList[0].FTH_BOUNCED == 0)
                                        {
                                            FinTrxService finTrxServiceClient;
                                            finTrxServiceClient = new FinTrxService();
                                            finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                            result = finTrxServiceClient.GenerateDummyEntry((int)TrxPK, ApplicationType.DPBJ);
                                            finTrxServiceClient = null;
                                        }
                                        SetUIValuesToObject(ControlsEnum.CHEQUERETURN);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_ReturnEntry_Not_Submitted").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    break;
                                }
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region VERSION HISTORY
                    case ActionsEnum.VERSIONHISTORY:
                        selectedGrdrow = (sender as ImageButton).Parent.Parent as GridViewRow;
                        HiddenField hdfTrxPK = (HiddenField)selectedGrdrow.FindControl("hdfTrxPK");
                        VoucherPk = 0;
                        long.TryParse(hdfTrxPK.Value, out VoucherPk);
                        GetFieldValues(ControlsEnum.VERSIONHISTORY);
                        SetFieldValues(ControlsEnum.VERSIONHISTORY);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divHistory]','" + GetLocalResourceObject("VoucherVersions").ToString() + "','800','400');", true);
                        break;
                    #endregion
                    #region HISTORY PRINT
                    case ActionsEnum.HISTORYPRINT:
                        company = string.Empty;
                        JournalType = string.Empty;
                        string strVersion = string.Empty;
                        bIsChecked = false;
                        selectedGrdrow = (sender as LinkButton).Parent.Parent as GridViewRow;

                        RefPK = Convert.ToInt32(((HiddenField)selectedGrdrow.FindControl("hdfHstryRefPk")).Value);
                        TrxPK = Convert.ToInt32(((HiddenField)selectedGrdrow.FindControl("hdfHstryVoucherPK")).Value);
                        //status = Convert.ToInt32(((HiddenField)selectedGrdrow.FindControl("hdfApproved")).Value);
                        company = ((HiddenField)selectedGrdrow.FindControl("hdfHstryCompany")).Value;
                        JournalType = ((HiddenField)selectedGrdrow.FindControl("hdfHstryRefType")).Value;
                        strVersion = ((HiddenField)selectedGrdrow.FindControl("hdfHstryVersion")).Value;
                        GetFieldValues(ControlsEnum.JOURNALIZATIONTYPE);
                        admAppTypeMstObj = admAppTypeMstList.SingleOrDefault(app => app.APT_NAME == ddlJournalizeType.SelectedItem.Text);
                        if (admAppTypeMstObj != null)
                        {
                            Session[ERP.Utilities.SessionStrings.Type] = admAppTypeMstObj.APT_CODE;
                        }

                        _printerMode = "";
                        #region Getting Printer Mode
                        dsParamSettings = new DataSet();
                        RptType = Session[ERP.Utilities.SessionStrings.Type].ToString();
                        RptSubType = 0;
                        Int32.TryParse(hdfAppSubType.Value, out RptSubType);
                        AppvdDate = DateTime.Now;

                        List<SPADM_APP_SUB_TYPE_DATA_GET_Result> objAppTypeDetailsList = new CommonService().GetReportParameters(RptType, RptSubType, AppvdDate);

                        if (objAppTypeDetailsList != null && objAppTypeDetailsList.Count > 0)
                        {
                            dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(objAppTypeDetailsList[0].AST_RPT_SETTINGS)));

                            if (dsParamSettings.Tables.Count > 0 && dsParamSettings.Tables[0].Columns.Contains("PRINTER_MODE"))
                            {
                                _printerMode = dsParamSettings.Tables[0].Rows[0]["PRINTER_MODE"].ToString();
                            }
                        }
                        #endregion

                        if (_printerMode == PrinterMode.DOTMATRIX.ToString())
                        {
                            string redirectUrl = string.Empty;
                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCS
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCVJ)
                            {
                                redirectUrl = string.Format("../Reports/GenerateReport.aspx?ID={0}&APPTYPE={1}&APPSUBTYPE={2}&TRXTYPE={3}&COMPANY={4}&PRINTERMODE={5}&VERSION={6}",
                                                                                          TrxPK.ToString(),
                                                                                          Session[ERP.Utilities.SessionStrings.Type].ToString(),
                                                                                          hdfAppSubType.Value,
                                                                                          Session[ERP.Utilities.SessionStrings.Type].ToString(),
                                                                                          company,
                                                                                          _printerMode,
                                                                                          strVersion);
                            }
                            else
                            {
                                redirectUrl = string.Format("../Reports/GenerateReport.aspx?ID={0}&APPTYPE={1}&APPSUBTYPE={2}&TRXTYPE={3}&COMPANY={4}&PRINTERMODE={5}&VERSION={6}",
                                                                                         RefPK.ToString(),
                                                                                         JournalType,
                                                                                         hdfAppSubType.Value,
                                                                                         JournalType,
                                                                                         company,
                                                                                         _printerMode,
                                                                                         strVersion);

                            }
                            TrxPK = 0;
                            Response.Redirect(redirectUrl, false);
                        }
                        else
                        {
                            if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.JV
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCS
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DPVJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.PCVJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.DRVJ
                                || Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YCV)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + TrxPK.ToString() + "&APPTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() +
                                    "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + Session[ERP.Utilities.SessionStrings.Type].ToString() + "&COMPANY=" + company + "&VERSION=" + strVersion + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + RefPK.ToString() + "&APPTYPE=" + JournalType +
                                    "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + JournalType + "&COMPANY=" + company + "&VERSION=" + strVersion + "');", true);// 
                            }
                        }
                        TrxPK = 0;


                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divHistory]','" + GetLocalResourceObject("VoucherVersions").ToString() + "','800','400');", true);

                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {
                poInvoiceServiceClient = null;
                poPaymentServiceClient = null;
                finCrDrHdrNoteServiceClient = null;
                salesInvoiceServiceClient = null;
                salesReceiptServiceClient = null;

            }
        }

        /// <summary>
        /// For reset button visibility
        /// Fill process after voucher popup close
        /// </summary>
        private void FillProcessAfterJournalClose()
        {
            if (Request.QueryString[QueryStrings.Type] != null && Request.QueryString[QueryStrings.Type] == ApplicationType.YE)
            {
                FillProcessID(ApplicationType.YE, 0);
            }
            else
            {
                FillProcessID(Session[ERP.Utilities.SessionStrings.Type].ToString(), 0);
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
                this.PageIndex = "1";
                GetFieldValues(ControlsEnum.FINHEADER);
                SetFieldValues(ControlsEnum.FINHEADER);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            SPADM_APP_STATUS_CFG_GET_KV_Result wkfStatus;
            try
            {
                if (((GridView)sender).ID == "grdJournalizeList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        ImageButton imgHistory = (ImageButton)e.Row.FindControl("imgHistory");
                        if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                        {
                            if ((finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "JV")
                                || (finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "OBV")
                                || (finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "PCS")
                                || (finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "DPRJ")
                                || (finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "YCV")
                                || (finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "CLSTJ"))
                            {
                                grdJournalizeList.Columns[3].Visible = false; // Party Name
                            }
                            if ((finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "PCRVJ")||(finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "PCVJ") || (finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "DPVJ") || (finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "OBV") || (finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "DRVJ") || (finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "YCV") || (finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == "CLSTJ"))
                            {
                                grdJournalizeList.Columns[7].Visible = false;
                            }
                            string AppType = finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE;
                            if (AppType == ApplicationType.PIJYE || AppType == ApplicationType.VPJYE || AppType == ApplicationType.SIJYE || AppType == ApplicationType.CRJYE || AppType == ApplicationType.EIJYE || AppType == ApplicationType.PSIJYE || AppType == ApplicationType.EIPJYE || AppType == ApplicationType.SIPJYE || AppType == ApplicationType.MSIJYE || AppType == ApplicationType.MSIRJYE || AppType == ApplicationType.FCHRJYE)
                            {
                                grdJournalizeList.Columns[5].Visible = false;
                                grdJournalizeList.Columns[6].Visible = false;
                            }                           

                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    Label lblAmount = e.Row.FindControl("lblAmount") as Label;
                    Label lblAmountBaseCur = e.Row.FindControl("lblAmountBaseCur") as Label;
                    HiddenField hdfJCurrency = e.Row.FindControl("hdfJCurrency") as HiddenField;

                    decimal Amount = 0;
                    decimal AmountBaseCur = 0;

                    if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                    {
                        Amount = decimal.Parse(finTrxHdrList[e.Row.RowIndex].FIN_TRX.ToList().Sum(fin => fin.FTR_CR_AMT_TC).ToString());
                        AmountBaseCur = decimal.Parse(finTrxHdrList[e.Row.RowIndex].FIN_TRX.ToList().Sum(fin => fin.FTR_CR_AMT_BC).ToString());
                        lblAmount.Text = String.Format("{0:c}", Amount);
                        lblAmountBaseCur.Text = String.Format("{0:c}", AmountBaseCur);
                        lblAmount.ToolTip = String.Format("{0:c}", Amount);
                        lblAmountBaseCur.ToolTip = String.Format("{0:c}", AmountBaseCur);

                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        Button imgPosted = e.Row.FindControl("imgPosted") as Button;

                        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;

                        short appstatus = Convert.ToInt16(hdfApproved.Value);
                        if (finTrxHdrList[e.Row.RowIndex].FTH_IS_DELETED)
                        {
                            imgApproved.CssClass = GetLocalResourceObject("Cancelled").ToString();
                            imgApproved.ToolTip = Resources.Captions.Cancelled;
                        }
                        else
                            if (workflowStatusList1 != null && workflowStatusList1.Count > 0)
                            {
                                wkfStatus = workflowStatusList1.SingleOrDefault(aa => aa.ASC_VALUE == appstatus);
                                if (wkfStatus != null)
                                {
                                    imgApproved.ToolTip = wkfStatus.ASC_NAME;
                                    imgApproved.CssClass = wkfStatus.ASC_CSS_CLASS;
                                }
                            }
                        //if (Convert.ToBoolean(hdfPosted.Value) == true)
                        //{
                        //    imgPosted.CssClass = GetLocalResourceObject("posted").ToString();
                        //    imgPosted.ToolTip = Resources.Captions.Posted;
                        //}
                        //else
                        //{
                        //    imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                        //    imgPosted.ToolTip = Resources.Captions.NotPosted;
                        //}

                        #region PDC Flag Settings
                        short PdcStatus = 0;

                        Button btnPDCFlag = e.Row.FindControl("btnPDCFlag") as Button;
                        if (finTrxHdrList[e.Row.RowIndex].FTH_REF_TYPE == ApplicationType.DPVJ && finTrxHdrList[e.Row.RowIndex].FTH_BOUNCED == 0)
                        {
                            PdcStatus = finTrxHdrList[e.Row.RowIndex].FTH_PDC == null ? (short)0 : (short)finTrxHdrList[e.Row.RowIndex].FTH_PDC;
                            btnPDCFlag.CssClass = PdcStatus != 0 ? (((PdcStatus == 1) || (PdcStatus == 3)) ? "flaggrey-icon" : "flaggreen-icon") : "";
                            btnPDCFlag.ToolTip = PdcStatus != 0 ? (((PdcStatus == 1) || (PdcStatus == 3)) ? GetLocalResourceObject("PDC_Cheque").ToString() : GetLocalResourceObject("Cheque_Reversed").ToString()) : "";
                            btnPDCFlag.Visible = PdcStatus != 0 ? true : false;
                        }
                        else
                        {
                            btnPDCFlag.Visible = false;
                        }

                        #endregion
                    }


                    //switch (appstatus)
                    //{
                    //    case (short)WkfStatusEnum.APPROVED:
                    //        imgApproved.CssClass = GetLocalResourceObject("mark").ToString();
                    //        if (workflowStatusList != null && workflowStatusList.Count > 0)
                    //            imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                    //                workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                    //        break;
                    //    case (short)WkfStatusEnum.DRAFTED:
                    //        imgApproved.CssClass = GetLocalResourceObject("close").ToString();
                    //        if (workflowStatusList != null && workflowStatusList.Count > 0)
                    //            imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                    //                workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                    //        break;
                    //    default:
                    //        imgApproved.CssClass = GetLocalResourceObject("prnormal-icon").ToString();
                    //        if (workflowStatusList != null && workflowStatusList.Count > 0)
                    //            imgApproved.ToolTip = workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus) != null ?
                    //                workflowStatusList.SingleOrDefault(aa => aa.CFG_VALUE == appstatus).CFG_DATA : string.Empty;
                    //        break;

                    //}



                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    GetFieldValues(ControlsEnum.BASECURRENCY);
                    Label lblHdrAmountBaseCur = e.Row.FindControl("lblHdrAmountBaseCur") as Label;
                    lblHdrAmountBaseCur.Text = GetLocalResourceObject("AmountBaseCur").ToString() + " (" + hdfBaseCurrency.Value.Split('-')[0].Trim() + ")";
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                }

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
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnAdvInvPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnOffReceiptPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNewFromTemplate.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnReverse.PreRender += new EventHandler(btnAction_PreRender);
            this.btnReturn.PreRender += new EventHandler(btnAction_PreRender);

            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            this.btnAdvInvPrint.Load += new EventHandler(btnAction_Load);
            this.btnPrint.Load += new EventHandler(btnAction_Load);
            this.btnOffReceiptPrint.Load += new EventHandler(btnAction_Load);
            this.btnNewFromTemplate.Load += new EventHandler(btnAction_Load);
            this.btnEditforCancel.Load += new EventHandler(btnAction_Load);
            this.btnReverse.Load += new EventHandler(btnAction_Load);
            this.btnReturn.Load += new EventHandler(btnAction_Load);
        }
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
                        // Assignment the first page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assignment the last page index.
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
                GetFieldValues(ControlsEnum.FINHEADER);
                SetFieldValues(ControlsEnum.FINHEADER);
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
            // Should we disable the first link
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
                //if (IsVoucherDeleted)
                //{
                //    btnPrint.Visible = false;
                //    btnOffReceiptPrint.Visible = false;
                //    btnAdvInvPrint.Visible = false;
                //}
                if (hdfReturnTye.Value != ApplicationType.DPVJ)
                    btnReturn.Visible = false;
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            INVOICEHDR,
            POINVOICELIST,
            POINVOICEDETAILS,
            FINANCEINVOICEHDR,
            FINANCEINVOICETRXMPG,
            PICKFORPAYMENT,
            PICKFORCRDRNOTE,
            INVOICENO,
            EXCHANGERATE,
            JOURNALIZE,
            INVOICE,
            FINHEADER,
            FINHEADERONE,
            FINDETAILS,
            JOURNALIZATIONTYPE,
            BASECURRENCY,
            FINPERIOD,
            GETREFPKBYJOURNALPK,
            GETFINTYPE,
            FILLWORKFLOWSTATUS,
            VOUCHERTEMPLATES,
            VOUCHERTEMPLATECATEGORIES,
            REVERSE,
            CHEQUERETURN,
            FINDIRECTPAYMENTHEADER,
            VERSIONHISTORY
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