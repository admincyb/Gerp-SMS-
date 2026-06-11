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
using CustomControls;
using System.Linq;
using BusinessObject.CommonManagement;
using System.Data;
using System.Threading;

namespace ERPSMS_v01.Sales
{
    public partial class SaleOrderForAgtComm : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties

        #region Properties
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

        public bool IsAgentInvoiceSCApproveList
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsAgentInvoiceSCApproveList] == null ? false : Convert.ToBoolean(ViewState[ViewstateStrings.IsAgentInvoiceSCApproveList].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsAgentInvoiceSCApproveList] = value;
            }
        }
        /// <summary>
        /// Customer Pk FOR DO
        /// </summary>
        private int CustomerIDForInvComm
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerIDForInvComm] == null ? 0 : (int)this.ViewState[ViewstateStrings.CustomerIDForInvComm];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerIDForInvComm] = value;
            }
        }
        /// <summary>
        /// Customer Pk FOR DO
        /// </summary>
        private int TypeIDForInvComm
        {
            get
            {
                return this.ViewState[ViewstateStrings.TypeIDForInvComm] == null ? 0 : (int)this.ViewState[ViewstateStrings.TypeIDForInvComm];
            }
            set
            {
                this.ViewState[ViewstateStrings.TypeIDForInvComm] = value;
            }
        }

        private string InvNoForAgentCommn
        {
             get
            {
                return (string)this.ViewState[ViewstateStrings.InvNoForAgentCommn];
            }
            set
            {
                this.ViewState[ViewstateStrings.InvNoForAgentCommn] = value;
            }
        }


       
        /// <summary>
        /// Sale Order ID For Adv Invoicing
        /// </summary>
        private int SoIdForInvComm
        {
            get
            {
                return this.ViewState[ViewstateStrings.SoIdForInvComm] == null ? 0 : (int)this.ViewState[ViewstateStrings.SoIdForInvComm];
            }
            set
            {
                this.ViewState[ViewstateStrings.SoIdForInvComm] = value;
            }
        }
        /// <summary>
        /// To Keep Currency For Adv Invoice
        /// </summary>
        private int CurrencyForInvComm
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrencyForInvComm] == null ? 0 : (int)this.ViewState[ViewstateStrings.CurrencyForInvComm];
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrencyForInvComm] = value;
            }
        }

        /// <summary>
        /// To keep selected Currency For Inv Comm
        /// </summary>
        private List<long> SelectedCurrencyForInvComm
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedCurrencyForInvComm];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCurrencyForInvComm] = value;
            }

        }
        /// <summary>
        /// Selected Customers for   Invoicing Comm
        /// </summary>
        private List<long> SelectedCustomersForInvComm
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedCustomersForInvComm];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCustomersForInvComm] = value;
            }

        }
        /// <summary>
        /// Selected   type  for   Invoicing Comm 
        /// </summary>
        private List<long> SelectedTypeForInvComm
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedTypeForInvComm];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedTypeForInvComm] = value;
            }

        }
       
        /// <summary>
        /// To maintain keep selected pos For Invoicing Comm
        /// </summary>
        private List<long> SelectedSosForInvComm
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSosForInvComm];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSosForInvComm] = value;
            }

        }
        /// To maintain count of selected pos for Advance Invoicing
        /// </summary>
        private int SelectedSosCountForInvComm
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedSosCountForInvComm] == null ? 0 : (int)this.ViewState[ViewstateStrings.SelectedSosCountForInvComm];

            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedSosCountForInvComm] = value;
            }
        }

        //-------------NIMISHA----------------------------



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
                return this.ViewState[ViewstateStrings.TotalPages] == null ? 0 : (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }
        /// <summary>
        /// Customer Pk
        /// </summary>
        private int CustomerID
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerID] == null ? 0 : (int)this.ViewState[ViewstateStrings.CustomerID];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerID] = value;
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
        /// To maintain the Search by in viewstate
        /// </summary>
        private int AgentCMSelectType
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.AgentCMSelectType];
            }
            set
            {
                this.ViewState[ViewstateStrings.AgentCMSelectType] = value;
            }
        }

        /// <summary>
        /// To maintain the Search by in viewstate
        /// </summary>
        private string SearchBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SearchBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.SearchBy] = value;
            }
        }

        /// <summary>
        /// To maintain the Search by in viewstate
        /// </summary>
        private string SP_NAME
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SP_NAME];
            }
            set
            {
                this.ViewState[ViewstateStrings.SP_NAME] = value;
            }
        }

        /// <summary>
        /// To maintain count of selected pos for Invoicing
        /// </summary>
        private int SelectedSosCount
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedSosCount] == null ? 0 : (int)this.ViewState[ViewstateStrings.SelectedSosCount];

            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedSosCount] = value;
            }
        }
        /// To maintain count of selected pos for DO
        /// </summary>
        private List<long> SelectedSos
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSos];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSos] = value;
            }

        }
        /// <summary>
        /// To keep selected pos For DO
        /// </summary>
        private List<long> SelectedSosForDO
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSosForDO];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = value;
            }

        }
        /// <summary>
        /// Currency
        /// </summary>
        private int Currency
        {
            get
            {
                return this.ViewState[ViewstateStrings.Currency] == null ? 0 : (int)this.ViewState[ViewstateStrings.Currency];
            }
            set
            {
                this.ViewState[ViewstateStrings.Currency] = value;
            }
        }
        /// <summary>
        /// To keep selected Currency
        /// </summary>
        private List<long> SelectedCurrency
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedCurrency];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCurrency] = value;
            }

        }
        private List<decimal> SelectedSOsTax
        {
            get
            {
                return (List<decimal>)this.ViewState["SelectedSOsTax"];
            }
            set
            {
                this.ViewState["SelectedSOsTax"] = value;
            }

        }
        private decimal SOTax
        {
            get
            {
                return (decimal)this.ViewState["SOTax"];
            }
            set
            {
                this.ViewState["SOTax"] = value;
            }

        }
        #endregion

        private DataSet dsPageData;
        private DataTable dtInvoiceList;

        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private BusinessObject.User currentUser;
        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private SAL_ORDER_HDR objSalesOrderHeader;
        private List<SAL_DESPATCH_HDR> salDespatchHdrList;
        private List<SAL_DESPATCH_DTL> salDespatchDetailList;
        private List<ADM_CONFIG_MST> workflowStatusList;
        private List<decimal> SelectedSOsTaxList;

        private SAL_ORDER_HDR salOrderHdrObj;
        private SAL_DESPATCH_DTL salDespatchObj;

        private List<SAL_ORDER_HDR> salesOrderHeaderList;
        private List<SAL_ORDER_DTL> salesOrderDetailsList;
        private List<long> SelectedSOList;
        private List<long> SelectedSOListForAdvInv;
        private List<long> SelectedSOListForDO;
        private List<string> SelectedOrderTypeList;

        private List<long> SelectedCustomerList;
        private List<long> SelectedCustomerListForAdvInv;
        private List<long> SelectedCustomerListForDO;

        private List<long> SelectedTypeListForAdvInv;
        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> admConfigMstList;

        private SPCRM_CUSTOMER_USER_GET_Result SPCRM_CUSTOMER_USER_GET_ResultObj;
        private List<SPCRM_CUSTOMER_USER_GET_Result> SPCRM_CUSTOMER_USER_GET_ResultList;
        DataTable dtSOData;
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

                    SelectedCustomersForInvComm = null;
                    SelectedSosForInvComm = null;
                    SelectedSos = null;
                    SelectedSosCount = 0;
                    SelectedTypeForInvComm = null;
                    //SelectedInvForAgentCommn = null;
                    ConfigurationSettings();
                    FillProcessID();

                    txtFromDate.Text = string.Empty;
                    hdfFromDate.Value = string.Empty;
                    txtToDate.Text = string.Empty;
                    hdfToDate.Value = string.Empty;

                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithSeperator.Value = "#" + currencysep + "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {                        
                        hdfDecimalFormatWithSeperator.Value += "0";
                    }                    
        
                    if (SelectedSosCountForInvComm != 0)
                        btnPickForInvComm.Text = GetLocalResourceObject("PickSoForAdvanceInvoicing").ToString() + "(" + SelectedSosCountForInvComm.ToString() + ")";


                    GetFieldValues(ControlsEnum.CUSTOMERPK);
                    if (SPCRM_CUSTOMER_USER_GET_ResultList != null && SPCRM_CUSTOMER_USER_GET_ResultList.Count > 0)
                    {
                        hdfCustomerID.Value = SPCRM_CUSTOMER_USER_GET_ResultList[0].CUS_PK.ToString();
                        txtCustomer.Text = SPCRM_CUSTOMER_USER_GET_ResultList[0].CUS_NAME;
                        Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                        Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
                    }
                    // for saleContracts Listing
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        public string GetFormattedNumberWithSeperator(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormatWithSeperator.Value);
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
            SaleOrderService salesOrderServiceClient;
            salesOrderServiceClient = null;
            SalDespatchHdrService salDespatchHdrServiceClient;
            salDespatchHdrServiceClient = null;
            SalDespatchDtlService salDespatchDtlServiceClient;
            salDespatchDtlServiceClient = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                    case ControlsEnum.SEARCH:
                        int cusID = String.IsNullOrEmpty(hdfCustomerID.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        if (hdfCustomerID.Value != null && hdfCustomerID.Value != "0" && hdfCustomerID.Value != "")
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
                        }
                        int InvPk = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);                        
                        string customer = string.IsNullOrEmpty(txtCustomer.Text.Trim()) ? string.Empty : (txtCustomer.Text.Trim() == "Select/Type" ? string.Empty : txtCustomer.Text.Trim());
                        string SOPK = string.IsNullOrEmpty(txtSONumber.Text.Trim()) ? string.Empty : (txtSONumber.Text.Trim() == "Select/Type" ? string.Empty : txtSONumber.Text.Trim());
                        string Agent = string.IsNullOrEmpty(txtAgent.Text.Trim()) ? string.Empty : (txtAgent.Text.Trim() == "Select/Type" ? string.Empty : txtAgent.Text.Trim());

                        dsPageData = BusinessLogic.Sales.SaleOrderForAgtCommBL.GetAgentCommInvoiceList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.SaleOrderHDate : SortBy,// SalesInvoiceDate
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                ThenBy = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenBy) ? Resources.DataFieldRes.SalesInvoiceNo : ThenBy,
                                ThenDirection = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                                SearchBy = SearchBy,
                                //SearchBy = "ICH_NO",
                                //SearchBy = "SOH_NO",
                                SearchValue = string.Empty// string.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()) ? string.Empty : (txtInvoiceNumber.Text.Trim() == "Select/Type" ? string.Empty : txtInvoiceNumber.Text.Trim())
                            }, currentUser, Resources.PageURL.SalAgentComm.Replace("~", ""), 0, SP_NAME, AgentCMSelectType);
                        if (dsPageData != null)
                        {
                            //string customer = string.IsNullOrEmpty(txtCustomer.Text.Trim()) ? string.Empty : (txtCustomer.Text.Trim() == "Select/Type" ? string.Empty : txtCustomer.Text.Trim());
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            string Fillter = string.Empty;
                            if (SOPK != string.Empty)
                            {
                                Fillter = Fillter + " ICH_SO_NO like '%" + SOPK + "%'";
                            }
                            if (customer != string.Empty)
                            {
                                if (Fillter == string.Empty)
                                {
                                    Fillter =" ICH_CUSTOMER_TEXT like '%" + HttpUtility.HtmlEncode(customer) + "%'";
                                }
                                else
                                {
                                    Fillter = Fillter + " and ICH_CUSTOMER_TEXT like '%" + HttpUtility.HtmlEncode(customer) + "%'";
                                }
                            }
                            if (Agent != string.Empty)
                            {
                                Fillter = Fillter + " ICH_AGENT_TEXT like '%" + HttpUtility.HtmlEncode(Agent) + "%'";
                            }
                            if (InvPk > 0)
                            {
                                if (Fillter == string.Empty)
                                    Fillter = Fillter + " ICH_PK = " + InvPk;
                                else
                                    Fillter = Fillter + " and ICH_PK = " + InvPk;
                            }
                           
                            dvInvoice.RowFilter = Fillter;
                            dtInvoiceList = dvInvoice.ToTable();
                        }
                        break;


                    case ControlsEnum.CUSTOMERPK:
                        CommonServiceClient = new CommonService();
                        SPCRM_CUSTOMER_USER_GET_ResultObj = ERP.Utilities.CommonFunctions.Initilize<SPCRM_CUSTOMER_USER_GET_Result>();
                        SPCRM_CUSTOMER_USER_GET_ResultList = CommonServiceClient.GetCustomerDetails(currentUser.PKUser, currentUser.SBUID);
                        break;
                    #region FILLWORKFLOWSTATUS
                    case ControlsEnum.FILLWORKFLOWSTATUS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("APPLICATION_STATUS").ToString();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        workflowStatusList = CommonServiceClient.GetConfigValues(admConfigMstObj);
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
                objSalesOrderHeader = null;
                serviceUtilityObj = null;
                salesOrderServiceClient = null;
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
                    case ControlsEnum.SEARCH:
                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
                        break;

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
            GridViewRow gvr;
            GridView grd;
            ExtGridView egrd;
            string arg;
            string soPK;
            string doPK;
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
            else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
            {
                commonActions = ActionsEnum.SHOWDETAILS;
            }
            switch (commonActions)
            {
                #region SHOWDETAILS
                case ActionsEnum.SHOWDETAILS:
                    // SetResetColour
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                    break;
                #endregion

                #region Search
                case ActionsEnum.SEARCH:
                    PageIndex = "1";
                    GetFieldValues(ControlsEnum.SEARCH);
                    SetFieldValues(ControlsEnum.SEARCH);
                    break;
                #endregion
                #region reset form
                case ActionsEnum.CLEAR:
                    ResetForm();
                    GetFieldValues(ControlsEnum.SEARCH);
                    SetFieldValues(ControlsEnum.SEARCH);
                    break;
                #endregion

                #region Show Popup
                case ActionsEnum.SHOWPOPUP:
                    soPK = ((LinkButton)sender).CommandArgument;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + soPK + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=") + "');", true);
                    break;
                #endregion
                #region Show DO Popup
                case ActionsEnum.SHOW:
                    doPK = ((LinkButton)sender).CommandArgument;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + doPK + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=6") + "');", true);
                    break;
                #endregion
                #region Pick for Receipt
                case ActionsEnum.PICKFORAGENTCOMM:
                
                    //  int result = BusinessLogic.Sales.SaleOrderForAgtCommBL.CheckInvoiceAgtComm();


                    SetUIValuesToObject(ActionsEnum.PICKFORAGENTCOMM);
                    // SetResetColour
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                    break;
                #endregion
                #region Tab navigation
                case ActionsEnum.DEFAULT:
                    Response.Redirect(Resources.PageURL.SalAgentComm);
                    break;
                case ActionsEnum.AGTINVOICE:
                    Response.Redirect(Resources.PageURL.AgtComm);
                    break;
                #endregion
                case ActionsEnum.RESETBTN:
                    btnPickForInvComm.Text = GetLocalResourceObject("PickSoForAdvanceInvoicing").ToString();
                    SelectedSosForInvComm = new List<long>();
                    SelectedSOListForAdvInv = new List<long>();
                    SelectedSosCountForInvComm = 0;
                    break;
                case ActionsEnum.RESET:
                    Session[ERP.Utilities.SessionStrings.SALEORDERPK] = null;
                    CustomerIDForInvComm = 0;
                    SoIdForInvComm = 0;
                    CurrencyForInvComm = 0;
                    TypeIDForInvComm = 0;
                    SelectedSOsTaxList = new List<decimal>();
                    SelectedSOsTax = null;
                    SelectedSosCountForInvComm = 0;
                    SelectedOrderTypeList = new List<string>();
                    SelectedCurrencyForInvComm = new List<long>();
                    SelectedCustomersForInvComm = new List<long>();
                    SelectedSosForInvComm = new List<long>();
                    SelectedCustomerListForAdvInv = new List<long>();
                    SelectedSOListForAdvInv = new List<long>();
                    SelectedTypeForInvComm = new List<long>();
                    SelectedTypeListForAdvInv = new List<long>();
                    //btnPickForInvComm.Text = GetLocalResourceObject("PickSoForAdvanceInvoicing").ToString();
                    //Resetting Color 
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

            try
            {
                #region Grid Fixed Columns

                if (((GridView)sender).ID == "grdInvoiceList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        //HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        //short appstatus = Convert.ToInt16(hdfApproved.Value);

                       // if (appstatus==1)
                       // {
                            //imgApproved.CssClass = GetLocalResourceObject("Invoiced").ToString();
                            //imgApproved.ToolTip = Resources.Captions.Completed;
                            //imgApproved.CssClass = GetLocalResourceObject("pending").ToString();
                            //imgApproved.ToolTip = Resources.Captions.Pending;
                       // }
                       // else
                        //{
                           // imgApproved.Visible = false;
                            //imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                            //imgApproved.ToolTip = Resources.Captions.Completed;
                       // }
                        //(e.Row.FindControl("hdfHasChildren") as HiddenField).Value = CommonConstants.SELECT_VALUE_ZERO;
                    }
                }
               
                

                #endregion

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
            EntryStatus = EntryStatus.LISTMODE;
        }

        #endregion


        #region Helper Methods
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID()
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                PageProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                base.WkfPageUrl = path;
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <param name="mode">Save Action</param>
        /// <returns>Object to Save</returns>
        private object SetUIValuesToObject(ActionsEnum controlType)
        {
            object returnObj = null;
            try
            {
                bool bIsChecked = false;
                foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                {
                    RadioButton rbtn;
                    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    if (rbtn.Checked)
                    {
                        bIsChecked = true;
                        // Pick for Agent Invoice Comm
                        CustomerIDForInvComm = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfAgtPK")).Value);
                        SoIdForInvComm = String.IsNullOrEmpty(((HiddenField)grdrow.FindControl("hdfInvPK")).Value.Trim()) ? 0 : Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvPK")).Value);
                        CurrencyForInvComm = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                        TypeIDForInvComm = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTypePK")).Value);
                        InvNoForAgentCommn = (((HiddenField)grdrow.FindControl("hdfInvNo")).Value);

                        break;
                    }
                }
                int result = BusinessLogic.Sales.SaleOrderForAgtCommBL.CheckInvoiceAgtComm(SoIdForInvComm);
                if (result == -1)
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_SaleInvoiceNotGenerated").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    return returnObj;
                }
                else
                {
                    if (bIsChecked)
                        switch (controlType)
                        {
                            #region Pick for Agent Commission Invoicing
                            case ActionsEnum.PICKFORAGENTCOMM:
                                if (!IsSameCurrency(SelectedCurrencyForInvComm, CurrencyForInvComm))
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (!IsSameCustomer(SelectedCustomersForInvComm, CustomerIDForInvComm))
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Customer").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (!IsSameCustomer(SelectedTypeForInvComm, TypeIDForInvComm))
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CustomerType").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (string.IsNullOrEmpty(InvNoForAgentCommn))
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_InvNo").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (!IsExixtPk(SelectedSosForInvComm, SoIdForInvComm))
                                {
                                    //Add agent
                                    if (SelectedCustomersForInvComm != null)
                                    {
                                        SelectedCustomerListForAdvInv = SelectedCustomersForInvComm;
                                    }
                                    else
                                    {
                                        SelectedCustomerListForAdvInv = new List<long>();
                                    }
                                    SelectedCustomerListForAdvInv.Add(CustomerIDForInvComm);
                                    SelectedCustomersForInvComm = SelectedCustomerListForAdvInv;

                                    //Add type
                                    if (SelectedTypeForInvComm != null)
                                    {
                                        SelectedTypeListForAdvInv = SelectedTypeForInvComm;
                                    }
                                    else
                                    {
                                        SelectedTypeListForAdvInv = new List<long>();
                                    }
                                    SelectedTypeListForAdvInv.Add(TypeIDForInvComm);
                                    SelectedTypeForInvComm = SelectedTypeListForAdvInv;

                                    //Add Currencies
                                    if (SelectedCurrencyForInvComm != null)
                                    {
                                        SelectedCurrencyForInvComm = SelectedCurrencyForInvComm;
                                    }
                                    else
                                    {
                                        SelectedCurrencyForInvComm = new List<long>();
                                    }
                                    SelectedCurrencyForInvComm.Add(CurrencyForInvComm);


                                    //Add so
                                    if (SelectedSosForInvComm != null)
                                    {
                                        SelectedSOListForAdvInv = SelectedSosForInvComm;
                                    }
                                    else
                                    {
                                        SelectedSOListForAdvInv = new List<long>();
                                    }
                                    SelectedSOListForAdvInv.Add(SoIdForInvComm);
                                    SelectedSosForInvComm = SelectedSOListForAdvInv;
                                    SelectedSosCountForInvComm = SelectedSosForInvComm.Count;

                                    btnPickForInvComm.Text = GetLocalResourceObject("PickSoForAdvanceInvoicing").ToString() + "(" + SelectedSosCountForInvComm.ToString() + ")";


                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                }
                                break;
                                #endregion
                        }
                    else
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Sc").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    }
                }
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        /// <summary>
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        private void SetUIEditView(ActionsEnum mode)
        {
            try
            {
                //foreach (GridViewRow grdrow in grdSoList.Rows)
                //{
                //    RadioButton rbtn;
                //    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                //    if (rbtn.Checked)
                //    {
                //        switch (mode)
                //        {
                //            case ActionsEnum.INVOICE:
                //                Session[ERP.Utilities.SessionStrings.SALEORDERPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOID")).Value);
                //                break;
                //        }

                //    }
                //}
                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.Invoicing), false);
                // if no items selected, Show Error Message
                //litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                //EntryStatus = EntryStatus.LISTMODE;
            }
            catch
            {
                throw;
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
                    case ControlsEnum.SEARCH:
                    case ControlsEnum.DEFAULT:
                        if (dtInvoiceList != null)
                        {

                            GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            grdInvoiceList.PageIndex = Convert.ToInt32(PageIndex);

                            grdInvoiceList.DataSource = dtInvoiceList.DefaultView;
                            grdInvoiceList.DataBind();

                            //SetResetColour
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                            //End
                        }
                        else
                        {
                            grdInvoiceList.DataSource = null;
                            grdInvoiceList.DataBind();
                        }
                        break;



                }
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
                switch (controlType)
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Is Same Currency
        /// </summary>
        /// <param name="Currencies"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameCurrency(List<long> Currencies, long pk)
        {

            bool flag = true;
            if (Currencies != null)
                foreach (long ven in Currencies)
                    if (ven != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
        }
        /// <summary>
        /// Is Matcing Tax
        /// </summary>
        /// <param name="TaxList"></param>
        /// <param name="tax"></param>
        /// <returns></returns>
        private bool IsMatcingTax(List<decimal> TaxList, decimal tax)
        {

            bool flag = true;
            bool BaseType = true;
            bool CurType = true;
            if (TaxList != null)
            {
                BaseType = tax > 0 ? true : false;
                foreach (long var in TaxList)
                {
                    CurType = var > 0 ? true : false;
                    if (BaseType != CurType)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// Is Same Customer
        /// </summary>
        /// <param name="vendors"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameCustomer(List<long> customers, long pk)
        {

            bool flag = true;
            if (customers != null)
                foreach (long cus in customers)
                    if (cus != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
        }
        /// <summary>
        /// Is Same Type
        /// </summary>
        /// <param name="SoType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsSameType(List<string> SoType, string type)
        {

            bool flag = true;
            if (SoType != null)
                foreach (string cus in SoType)
                    if (cus != type)
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
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            txtCustomer.Text = string.Empty;
            txtSONumber.Text = string.Empty;
            txtInvoiceNumber.Text = string.Empty;
            hdfSoPK.Value = "0";
            hdfCustomerID.Value = "";
            txtAgent.Text = string.Empty;
            hdfAgent.Value = "0";
            //txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            //hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            //txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            //hdfToDate.Value = DateTime.Now.ToString();
            txtFromDate.Text = string.Empty;
            hdfFromDate.Value = string.Empty;
            txtToDate.Text = string.Empty;
            hdfToDate.Value = string.Empty;
            //Resetting Color 

        }

        protected string GetConstName(object invItem)
        {
            string constName = string.Empty;
            INV_ITEM_MST invItemObj = (INV_ITEM_MST)invItem;
            if (invItemObj != null)
            {
                constName = invItemObj.INV_ITEM_SPEC_DTL.Count() > 0 ?
                    HttpUtility.HtmlDecode(invItemObj.INV_ITEM_SPEC_DTL.First().ADM_CONST_MST16.CON_NAME) : string.Empty;
            }
            return constName;
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;

            btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);
            lbnSOListing.PreRender += new EventHandler(btnAction_PreRender);
            lnbDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForInvComm.PreRender += new EventHandler(btnAction_PreRender);



            lbnSOListing.Load += new EventHandler(btnAction_Load);
            btnResetSelection.Load += new EventHandler(btnAction_Load);
            lnbDeliveryOrder.Load += new EventHandler(btnAction_Load);
            btnPickForInvComm.Load += new EventHandler(btnAction_Load);


        }
        /// <summary>
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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            //  base.CheckBtnVisibility(sender);
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
                //GetFieldValues(ControlsEnum.DEFAULT);
                //SetFieldValues(ControlsEnum.DEFAULT);
                GetFieldValues(ControlsEnum.SEARCH);
                SetFieldValues(ControlsEnum.SEARCH);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            }
        }

        #region Configuration
        /// <summary>
        /// GetConfiguration
        /// </summary>
        private void ConfigurationSettings()
        {
            SP_NAME = GetGlobalResourceObject("ConfigurationsRes", "AgentInvoiceSCApproveList").ToString();
            SearchBy=GetGlobalResourceObject("ConfigurationsRes", "AgentInvoiceSearchBy").ToString();
            AgentCMSelectType =Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "AgentCommissionSelectType").ToString());
            
        }
        #endregion

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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
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
            SODETAILS,
            SHOWSOHDR,
            SHOWSODTL,
            SHOWDETAILS,
            TYPECATEGORY,
            SEARCH,
            CUSTOMERPK,
            FILLWORKFLOWSTATUS,
            SOTYPE,
            PICKFORAGENTCOMM



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
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2,
            REQUESTFORMOREINFO = 6,
            SUBMITWITHMOREINFO = 7,
            SUBMITTED = 1,
            REJECTED = 3,
            CLOSED = 5,
            SHORTCLOSED = 4
        }

        #endregion
    }
}