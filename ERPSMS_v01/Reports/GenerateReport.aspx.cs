using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using BusinessObject.CommonManagement;
using ERPData;
using ERPService;
using ERPManager;
using ERPSMS_v01.Administration.Masters;
using ERPService.Administration;
using ERP.Utilities;
using System.Xml;
using System.IO;
using BusinessLogic.VendorManagement;
using System.Threading;
using BusinessLogic.Sales;
using System.Text;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Drawing;
using BusinessObject.Reports;
using BusinessObject.Reports.Factory;
using ERPSMS_v01.Reports.ReportDataBuilders;
using System.Reflection;
using ERPSMS_v01.Reports.ReportDataBuilders.ParameterBinder;
using System.Security;
using System.Security.Permissions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using BusinessLogic.CommonManagement;
using System.Runtime;

namespace ERPSMS_v01.Reports
{
    public partial class GenerateReport : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Properties
        private string ReportFile
        {
            get
            {
                return this.ViewState["ReportFile"] == null ? string.Empty : (this.ViewState["ReportFile"]).ToString();
            }
            set
            {
                this.ViewState["ReportFile"] = value;
            }
        }
        private int ShippingPlanPK
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] != null ? (int)Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] : 0;
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = value;
            }
        }
        private int IOType
        {
            get
            {
                return (int)this.ViewState["IOType"];
            }
            set
            {
                this.ViewState["IOType"] = value;
            }
        }
        private string FTH_REF_TYPE
        {
            get
            {
                return this.ViewState["FTH_REF_TYPE"] == null ? string.Empty : (string)this.ViewState["FTH_REF_TYPE"];
            }
            set
            {
                this.ViewState["FTH_REF_TYPE"] = value;
            }
        }
        private string VOUCHER_FROM
        {
            get
            {
                return this.ViewState["VOUCHER_FROM"] == null ? string.Empty : (string)this.ViewState["VOUCHER_FROM"];
            }
            set
            {
                this.ViewState["VOUCHER_FROM"] = value;
            }
        }
        private string VOUCHER_TO
        {
            get
            {
                return this.ViewState["VOUCHER_TO"] == null ? string.Empty : (string)this.ViewState["VOUCHER_TO"];
            }
            set
            {
                this.ViewState["VOUCHER_TO"] = value;
            }
        }
        private int V_STATUS
        {
            get
            {
                return this.ViewState["V_STATUS"] == null ? 0 : (int)this.ViewState["V_STATUS"];
            }
            set
            {
                this.ViewState["V_STATUS"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsPdfGenerated
        {
            get
            {
                return this.ViewState["IsPdfGenerated"] == null ? false : Convert.ToBoolean(this.ViewState["IsPdfGenerated"]);
            }
            set
            {
                this.ViewState["IsPdfGenerated"] = value;
            }
        }
        private string ChequeID
        {
            get
            {
                return (string)this.ViewState["ChequeID"];
            }
            set
            {
                this.ViewState["ChequeID"] = value;
            }
        }
        /// <summary>
        /// To maintain the Report Type in viewstate
        /// </summary>
        public string RptType
        {
            get
            {
                return this.ViewState["ReportType"] == null ? string.Empty : (string)this.ViewState["ReportType"];
            }
            set
            {
                this.ViewState["ReportType"] = value;
            }
        }
        /// </summary>
        private int Version
        {
            get
            {
                return this.ViewState["Vrsn"] == null ? 0 : Convert.ToInt32(this.ViewState["Vrsn"]);
            }
            set
            {
                this.ViewState["Vrsn"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string EmpName
        {
            get
            {
                return this.ViewState["EmpName"] == null ? string.Empty : (string)this.ViewState["EmpName"];
            }
            set
            {
                this.ViewState["EmpName"] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        public int RptSubType
        {
            get
            {
                return this.ViewState["ReportSubType"] == null ? 0 : (int)this.ViewState["ReportSubType"];
            }
            set
            {
                this.ViewState["ReportSubType"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SbuID
        {
            get
            {
                return this.ViewState["SbuID"] == null ? -1 : Convert.ToInt32(this.ViewState["SbuID"]);
            }
            set
            {
                this.ViewState["SbuID"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool FromExternal
        {
            get
            {
                return this.ViewState["FromExternal"] == null ? false : Convert.ToBoolean(this.ViewState["FromExternal"]);
            }
            set
            {
                this.ViewState["FromExternal"] = value;
            }
        }
        /// <summary>
        /// External PDF Name
        /// </summary>
        public string ExternalPDFName
        {
            get
            {
                return this.ViewState["ExternalPDFName"] == null ? string.Empty : (string)this.ViewState["ExternalPDFName"];
            }
            set
            {
                this.ViewState["ExternalPDFName"] = value;
            }
        }
        /// <summary>
        /// To maintain Report Pk
        /// </summary>
        public int RecPK
        {
            get
            {
                return this.ViewState["RecPK"] == null ? 0 : Convert.ToInt32(this.ViewState["RecPK"]);
            }
            set
            {
                this.ViewState["RecPK"] = value;
            }
        }
        /// <summary>
        /// To maintain Revision Pk
        /// </summary>
        private int RevPK
        {
            get
            {
                return this.ViewState["RevPK"] == null ? 0 : Convert.ToInt32(this.ViewState["RevPK"]);
            }
            set
            {
                this.ViewState["RevPK"] = value;
            }
        }
        /// <summary>
        /// To maintain Report Pk
        /// </summary>
        private int VndPK
        {
            get
            {
                return (int)this.ViewState["VndPK"];
            }
            set
            {
                this.ViewState["VndPK"] = value;
            }
        }
        /// <summary>
        /// To maintain Approved Date
        /// </summary>
        private DateTime AppvdDate
        {
            get
            {
                return (DateTime)(this.ViewState["AppvdDate"] == null ? DateTime.Now.Date : this.ViewState["AppvdDate"]);
            }
            set
            {
                this.ViewState["AppvdDate"] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string TrxRefType
        {
            get
            {
                return (string)this.ViewState["TrxRefType"];
            }
            set
            {
                this.ViewState["TrxRefType"] = value;
            }
        }
        /// <summary>
        /// Department Company PK
        /// </summary>
        private int CompanyPK
        {
            get
            {
                return this.ViewState["CompanyPK"] == null ? 0 : (int)this.ViewState["CompanyPK"];
            }
            set
            {
                this.ViewState["CompanyPK"] = value;
            }
        }

        /// <summary>
        /// To maintain  Pk
        /// </summary>
        private int PK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["PK"]);
            }
            set
            {
                this.ViewState["PK"] = value;
            }
        }
        private bool IsTaxForOtherChargeSales
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsTaxForOtherChargeSales] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsTaxForOtherChargeSales].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsTaxForOtherChargeSales] = value;
            }
        }
        private bool IsTaxForOtherChargePurchase
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsTaxForOtherChargeSales] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsTaxForOtherChargeSales].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsTaxForOtherChargeSales] = value;
            }
        }
        private bool IsRepeatSIheader
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsRepeatSIheader] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsRepeatSIheader].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsRepeatSIheader] = value;
            }
        }
        private bool IsExportExcel
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsExportExcel] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsExportExcel].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsExportExcel] = value;
            }
        }
        private int IsExcelPrint
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsExcelPrint] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.IsExcelPrint].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsExcelPrint] = value;
            }
        }

        /// <summary>
        /// To maintain voucher history version in viewstate
        /// </summary>
        private short VoucherVersion
        {
            get
            {
                return this.ViewState[ViewstateStrings.VoucherVersion] == null ? (short)VoucherVersions.None : Convert.ToInt16(this.ViewState[ViewstateStrings.VoucherVersion].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.VoucherVersion] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SICountText //SICOUNTTEXT
        {
            get
            {
                return this.ViewState["SICountText"] == null ? string.Empty : (string)this.ViewState["SICountText"];
            }
            set
            {
                this.ViewState["SICountText"] = value;
            }
        }
        /// <summary>
        /// Keep Transaction Company PK
        /// </summary>
        private int? TrxCompanyPK
        {
            get
            {
                return this.ViewState["TrxCompanyPK"] == null ? null : (int?)this.ViewState["TrxCompanyPK"];
            }
            set
            {
                this.ViewState["TrxCompanyPK"] = value;
            }
        }

        private int SubReportIndex
        {
            get
            {
                return this.ViewState["SubReportIndex"] == null ? 1 : (int)this.ViewState["SubReportIndex"];
            }
            set
            {
                this.ViewState["SubReportIndex"] = value;
            }
        }

        //private string WOIssuesXml
        //{
        //    get
        //    {
        //        return Session[ERP.Utilities.SessionStrings.WOIssuesXml] == null ? null : (string)Session[ERP.Utilities.SessionStrings.WOIssuesXml];
        //    }
        //    set
        //    {
        //        Session[ERP.Utilities.SessionStrings.WOIssuesXml] = value;
        //    }
        //}

        private string BatchNo
        {
            get
            {
                return (string)this.ViewState["BatchNo"];
            }
            set
            {
                this.ViewState["BatchNo"] = value;
            }
        }

        public string LogoPath = string.Empty;
        private string OutputLogoPath = string.Empty;
        #endregion

        #region Variables
        BusinessObject.User currentUser;
        private DataSet dsPurchaseRequest;
        private DataSet dsCompoundUsage;
        private DataSet dsDelivaryOrder;
        private DataSet dsSaleOrder;
        private DataSet dsVendorEval;
        private DataSet dsMaterialIssue;
        private DataSet dsWOIssue;
        private DataSet dsMaterialAccept;
        private DataSet dsContainerInspection;
        private DataSet dsLoadingPlan;
        private DataSet dsShippingPlan;
        private DataSet dsExportForm;
        private DataSet dsWHT;
        private DataSet dsVatSaleExport;
        private DataSet dsCmpPreparation;
        private DataSet dsDispPreparation;
        private DataSet dsCNDN;
        private DataSet dsGST;
        private DataSet dsVoucher;
        private DataSet dsServiceRequest;
        private DataSet dsServiceOrder;
        private DataSet dsAssetDisposal;
        private DataSet dsCWIP;
        private DataSet dsServiceOrderReceipt;
        private DataSet dsAgentCommision;
        private DataSet dsDepreciation;
        private DataSet dsExtMaterialRecive;
        private DataSet dsOpeningStock;
        private DataTable dtStoreLocn;
        private DataTable dtStoreLocation;
        private DataSet DataSet1;
        private DataTable dtResult;
        private DataTable dtLabelRptConfig;
        private DataSet dsFundRequestDeptWise;
        private DataSet dsIssueReportDetails;
        private DataSet dsReportDetails;
        DataSet dsFinance;
        DataTable dtSOData;
        DataTable dtLocation;
        DataTable dtPRDtls;

        DataTable dtHeader;
        DataTable dtDetails1;
        DataTable dtDetails2;
        DataTable dtDetails3;
        ReportDataSource rdsHeader, rdsDetails1, rdsDetails2, rdsDetails3;

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsListNew;
        private List<SPFIN_PND54_RPT_Result> lstPND;
        private ERPEntities currentEntity;
        private CommonService cm;
        private DataSet SelectedAccountsList;
        public event TreeNodeEventHandler TreeNodeCheckChanged;
        private static string ReturnUrl;
        private string attachmentFilePath;
        private string attachmentFileFormat;
        private string attachmentFileContentType;
        private string attachmentFileName;
        private int m_currentPageIndex;
        private IList<Stream> m_streams;
        LocalReport locRpt;
        private string ChequeReport = string.Empty;
        private DateTime startOfMonth;
        private DateTime endOfMonth;
        int ProSize = 0;
        private int PNDCount = 0;
        private int DisplayTab;
        private int DPVJPNDcount = 0;
        private int SwapBuyer = 0;
        private string Buyer = string.Empty;
        private bool PrintShipTo = true;
        private double? BalanceCr = 0;
        private double? BalanceDr = 0;
        private double? NetWt = 0;
        private double? GrWt = 0;
        private string clientCode;
        private int bizUnit = 0;
        private int deptCategory = 0;
        private string searchName = string.Empty;
        private string searchValue = string.Empty;
        List<SPFIN_TRX_VOUCHER_RPT_Result> lstVoucher;
        List<SPFIN_PAYMENT_VND_VOUCHER_RPT_Result> lstPayment;
        CustomerMstService CustomerMstServiceClient;
        CRM_CUSTOMER_MST salCustomerObj;
        List<CRM_CUSTOMER_MST> salCustomerList;
        int RowIndex = 0;

        #region Crystal report variables
        private ReportDocument reportDocument;
        private ParameterField paramField;
        private ParameterFields rptParamFields;
        private ParameterDiscreteValue paramDiscreteValue;
        private List<ListItem> checkedListCheckedItems;
        #endregion
        #endregion
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

        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            #region For Crystal report
            //If any report document exist, need to dispose the object
            if (!IsPostBack)
            {
                //GERP_MIS_Report.Visible = false;
                if (reportDocument != null)
                {
                    reportDocument.Close();
                    reportDocument.Dispose();
                    reportDocument = null;
                }
                if (Request.QueryString[QueryStrings.FromExt] == null)
                    Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                else
                    Session[ERP.Utilities.SessionStrings.CRReportDataFromExt] = null;

            }
            #endregion


            ParameterFields locParamFields;
            if (Request.QueryString[QueryStrings.FromExt] == null)
            {
                reportDocument = (ReportDocument)Session[ERP.Utilities.SessionStrings.CRReportData];
                locParamFields = (ParameterFields)Session[ERP.Utilities.SessionStrings.CRReportParam];
            }
            else
            {
                reportDocument = (ReportDocument)Session[ERP.Utilities.SessionStrings.CRReportDataFromExt];
                locParamFields = (ParameterFields)Session[ERP.Utilities.SessionStrings.CRReportParamFromExt];
            }
            if (reportDocument != null)
            {
                CommonFunctions.SetCrystalReportDataBaseConnection(reportDocument);
                ApplyLedgerCrystalFormulaFixes(GetCurrentCrystalReportName());
                GERP_Report.ParameterFieldInfo = locParamFields;
                GERP_Report.ReportSource = reportDocument;
            }
            else
                ClearCrystalReport();

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
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //base.CheckBtnVisibility(sender);
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
        protected void Page_PreRenderComplete(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowFilter", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        public void GetFieldValues(string controlType)
        {
            try
            {
                AppTypeDetailsList = new List<SPADM_APP_SUB_TYPE_DATA_GET_Result>();
                cm = new CommonService();

                //Get AppType Details
                switch (controlType)
                {
                    #region location
                    case ApplicationType.LOCATION:
                        dtLocation = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetDirectGRNAutocomplete(GTIService.Constants.Common.CommonConstants.CMP_DISPLAY_CODE, string.Empty, currentUser);
                        break;
                    #endregion
                    #region PR
                    case ApplicationType.PR:
                    case ApplicationType.PRT:
                        if (GetGlobalResourceObject("ConfigurationsRes", "ISMENUWISEDOCNOREVISION").ToString() == "1")
                        {
                            AppTypeDetailsListNew = cm.GetReportParameters(RptType, RptSubType, AppvdDate, TrxCompanyPK);
                            int reportspk = AppTypeDetailsListNew[0].AST_PK;
                            dsPurchaseRequest = BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetPurchaseRequestReportDetailsDOCNOREVISION(RecPK, reportspk);
                        }
                        else
                        {
                            dsPurchaseRequest = BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetPurchaseRequestReportDetails(RecPK);
                        }

                        //  dsPurchaseRequest = BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetPurchaseRequestReportDetails(RecPK);
                        break;
                    #endregion
                    #region MTR
                    case ApplicationType.MTR:
                        dsPurchaseRequest = BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetMaterialRequestReportDetails(RecPK);
                        break;
                    #endregion
                    #region RFQ
                    case ApplicationType.RFQ:
                        dsPurchaseRequest = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQReportDetails(RecPK, VndPK);
                        AppvdDate = DateTime.Now.Date;
                        if (dsPurchaseRequest != null)
                        {
                            if (dsPurchaseRequest.Tables[0].Rows.Count > 0)
                            {
                                //AppvdDate = Convert.ToDateTime(Convert.ToString(dsPurchaseRequest.Tables[0].Rows[0]["RRH_APPROVED_DATE"]) != string.Empty ? dsPurchaseRequest.Tables[0].Rows[0]["RRH_APPROVED_DATE"].ToString() : null);
                            }
                        }
                        break;
                    #endregion
                    #region QAC
                    case ApplicationType.QAC:
                        dsPurchaseRequest = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQAmtCompReportDetails(RecPK, Convert.ToInt16(ddlCompareType.SelectedValue));
                        break;
                    #endregion
                    #region PO
                    case ApplicationType.PO:
                    case ApplicationType.POP:
                    case ApplicationType.POG:
                    case ApplicationType.POPG:
                    case ApplicationType.POTR:
                    case ApplicationType.POT:
                        if (RevPK > 0)
                        {
                            dsPurchaseRequest = BusinessLogic.PurchaseOrderManagement.PurchaseOrderCreation.PurchaseOrderDetails(RecPK, RevPK);
                        }
                        else
                        {
                            if (GetGlobalResourceObject("ConfigurationsRes", "ISMENUWISEDOCNOREVISION").ToString() == "1")
                            {
                                AppTypeDetailsListNew = cm.GetReportParameters(RptType, RptSubType, AppvdDate, TrxCompanyPK);
                                int reportspk = AppTypeDetailsListNew[0].AST_PK;
                                dsPurchaseRequest = BusinessLogic.PurchaseOrderManagement.PurchaseOrderCreation.PurchaseOrderDetailsDOCNOREVISION(RecPK, GetGlobalResourceObject("ConfigurationsRes", "PurchaseOrderOutRPTSP").ToString(), reportspk);

                            }
                            else
                            {
                                dsPurchaseRequest = BusinessLogic.PurchaseOrderManagement.PurchaseOrderCreation.PurchaseOrderDetails(RecPK, GetGlobalResourceObject("ConfigurationsRes", "PurchaseOrderOutRPTSP").ToString());
                            }
                            // dsPurchaseRequest = BusinessLogic.PurchaseOrderManagement.PurchaseOrderCreation.PurchaseOrderDetails(RecPK, GetGlobalResourceObject("ConfigurationsRes", "PurchaseOrderOutRPTSP").ToString());
                        }
                        if (dsPurchaseRequest != null)
                        {
                            if (dsPurchaseRequest.Tables[0].Rows.Count > 0)
                            {
                                AppvdDate = Convert.ToString(dsPurchaseRequest.Tables[0].Rows[0]["POH_APPROVED_DATE"]) != string.Empty ? Convert.ToDateTime(dsPurchaseRequest.Tables[0].Rows[0]["POH_APPROVED_DATE"].ToString()) : DateTime.Now.Date;
                                if (!string.IsNullOrEmpty(dsPurchaseRequest.Tables[0].Rows[0]["POH_COMPANY"].ToString()))
                                    TrxCompanyPK = Convert.ToInt32(dsPurchaseRequest.Tables[0].Rows[0]["POH_COMPANY"].ToString());
                            }
                        }
                        break;
                    #endregion
                    #region SCWO
                    case ApplicationType.SCWO:
                        if (Version > 0)
                        {
                            dsPurchaseRequest = BusinessLogic.WorkOrder.WorkOrderBL.GetWorkOrderDetailsReport(RecPK, Version);
                        }
                        else
                        {
                            dsPurchaseRequest = BusinessLogic.WorkOrder.WorkOrderBL.GetWorkOrderDetailsReport(RecPK);
                        }
                        if (dsPurchaseRequest != null)
                        {
                            if (dsPurchaseRequest.Tables[0].Rows.Count > 0)
                            {
                                AppvdDate = Convert.ToString(dsPurchaseRequest.Tables[0].Rows[0]["WIH_APPROVED_DATE"]) != string.Empty ? Convert.ToDateTime(dsPurchaseRequest.Tables[0].Rows[0]["WIH_APPROVED_DATE"].ToString()) : DateTime.Now.Date;
                                if (!string.IsNullOrEmpty(dsPurchaseRequest.Tables[0].Rows[0]["WIH_COMPANY"].ToString()))
                                    TrxCompanyPK = Convert.ToInt32(dsPurchaseRequest.Tables[0].Rows[0]["WIH_COMPANY"].ToString());
                            }
                        }
                        break;
                    #endregion
                    #region VNDEVAL
                    case ApplicationType.VNDEVAL:
                        //dsVendorEval = VendorEvaluation.GetPerformanceReport(RecPK,currentUser.SBUID);
                        string vndEvaluationDtls;
                        vndEvaluationDtls = BusinessLogic.VendorManagement.VendorEvaluation.GetEvaluationReport(RecPK);
                        dsVendorEval = new DataSet();
                        dsVendorEval.ReadXml(new XmlTextReader(new StringReader(vndEvaluationDtls)));
                        if (dsVendorEval != null)
                        {
                            AppvdDate = DateTime.Now.Date;
                            if (dsVendorEval.Tables[0].Rows.Count > 0)
                            {
                                //AppvdDate = Convert.ToString(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"]) != string.Empty ? Convert.ToDateTime(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"].ToString()) : DateTime.Now.Date;
                                AppvdDate = DateTime.Now.Date;
                            }
                        }
                        break;
                    #endregion
                    #region SO
                    case ApplicationType.SO:
                    case ApplicationType.SOD:
                        dsSaleOrder = BusinessLogic.Sales.SaleOrderBL.SaleOrderDetails(RecPK);
                        if (dsSaleOrder != null)
                        {
                            if (dsSaleOrder.Tables[0].Rows.Count > 0)
                            {
                                AppvdDate = Convert.ToString(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"]) != string.Empty ? Convert.ToDateTime(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"].ToString()) : DateTime.Now.Date;
                            }
                        }
                        break;
                    #endregion
                    #region SI, MSI
                    case ApplicationType.SI:
                    case ApplicationType.SIC:
                    case ApplicationType.SIM:
                    case ApplicationType.SIMSF:
                    case ApplicationType.SIMSS:
                    case ApplicationType.MSI://@@  

                        if (GetGlobalResourceObject("ConfigurationsRes", "ISMENUWISEDOCNOREVISION").ToString() == "1")
                        {
                            AppTypeDetailsListNew = cm.GetReportParameters(RptType, RptSubType, AppvdDate, TrxCompanyPK);
                            int reportspk = AppTypeDetailsListNew[0].AST_PK;
                            dsDelivaryOrder = BusinessLogic.Sales.SaleOrderBL.GetSalesInvoiceDtlsDOCNOREVISION(RecPK, GetGlobalResourceObject("ConfigurationsRes", "SalesInvoiceOutRPTSP").ToString(), reportspk);

                        }
                        else
                        {
                            dsDelivaryOrder = BusinessLogic.Sales.SaleOrderBL.GetSalesInvoiceDtls(RecPK, GetGlobalResourceObject("ConfigurationsRes", "SalesInvoiceOutRPTSP").ToString());
                        }
                        //      dsDelivaryOrder = BusinessLogic.Sales.SaleOrderBL.GetSalesInvoiceDtls(RecPK, GetGlobalResourceObject("ConfigurationsRes", "SalesInvoiceOutRPTSP").ToString());
                        break;
                    #endregion
                    #region CID                  
                    case ApplicationType.CID://@@      
                        if (GetGlobalResourceObject("ConfigurationsRes", "ISMENUWISEDOCNOREVISION").ToString() == "1")
                        {
                            AppTypeDetailsListNew = cm.GetReportParameters(RptType, RptSubType, AppvdDate, TrxCompanyPK);
                            int reportspk = AppTypeDetailsListNew[0].AST_PK;
                            dsDelivaryOrder = BusinessLogic.Sales.SaleOrderBL.GetSalesInvoiceCustomsDtls(RecPK, GetGlobalResourceObject("ConfigurationsRes", "SalesInvoiceOutCustomsRPTSP").ToString(), reportspk);
                        }
                        else
                            dsDelivaryOrder = BusinessLogic.Sales.SaleOrderBL.GetSalesInvoiceCustomsDtls(RecPK, GetGlobalResourceObject("ConfigurationsRes", "SalesInvoiceOutCustomsRPTSP").ToString());
                        break;

                    #endregion

                    #region DSI, MSIT,DSIJ
                    case ApplicationType.DSI:
                    case ApplicationType.MSIT:
                    case ApplicationType.DSID:
                        dsDelivaryOrder = BusinessLogic.Sales.SaleOrderBL.GetSalesInvoiceTrdDtls(RecPK, GetGlobalResourceObject("ConfigurationsRes", "SalesInvoiceTRDOutRPTSP").ToString());
                        break;
                    #endregion
                    #region DO
                    case ApplicationType.DO:
                    case ApplicationType.DOD:
                        switch (RptSubType)
                        {
                            case 11:
                                dsDelivaryOrder = BusinessLogic.ReportsManagement.DeliveryOrderBL.GetDeliveryOrderDetails(RecPK, RptSubType);
                                break;
                            case 7:
                            case 8:
                                dsDelivaryOrder = BusinessLogic.ReportsManagement.DeliveryOrderBL.GetPackingListDtls(RecPK);
                                break;
                            default:

                                if (GetGlobalResourceObject("ConfigurationsRes", "ISMENUWISEDOCNOREVISION").ToString() == "1")
                                {
                                    AppTypeDetailsListNew = cm.GetReportParameters(RptType, RptSubType, AppvdDate, TrxCompanyPK);
                                    int reportspk = AppTypeDetailsListNew[0].AST_PK;
                                    dsDelivaryOrder = BusinessLogic.ReportsManagement.DeliveryOrderBL.GetDeliveryOrderDetailsDOCNOREVISION(RecPK, RptSubType, reportspk);

                                }
                                else
                                {
                                    dsDelivaryOrder = BusinessLogic.ReportsManagement.DeliveryOrderBL.GetDeliveryOrderDetails(RecPK, RptSubType);
                                }
                                //      dsDelivaryOrder = BusinessLogic.ReportsManagement.DeliveryOrderBL.GetDeliveryOrderDetails(RecPK, RptSubType);
                                break;
                        }

                        if (dsDelivaryOrder != null)
                        {
                            if (dsDelivaryOrder.Tables[0].Rows.Count > 0)
                            {
                                AppvdDate = DateTime.Now.Date;
                            }
                        }
                        break;
                    #endregion
                    #region IO
                    case ApplicationType.IO:
                        dsSaleOrder = BusinessLogic.Sales.SaleOrderBL.SaleOrderDetails(RecPK);
                        if (dsSaleOrder != null)
                        {
                            if (dsSaleOrder.Tables[0].Rows.Count > 0)
                            {
                                AppvdDate = Convert.ToString(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"]) != string.Empty ? Convert.ToDateTime(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"].ToString()) : DateTime.Now.Date;
                            }
                        }
                        break;
                    #endregion
                    #region MI
                    case ApplicationType.MI:
                        if (GetGlobalResourceObject("ConfigurationsRes", "ISMENUWISEDOCNOREVISION").ToString() == "1")
                        {
                            AppTypeDetailsListNew = cm.GetReportParameters(RptType, RptSubType, AppvdDate, TrxCompanyPK);
                            int reportspk = AppTypeDetailsListNew[0].AST_PK;
                            dsMaterialIssue = new DataSet();
                            string issueXml = string.Empty;
                            issueXml = BusinessLogic.StoreManagement.MaterialIssue.GetMaterialIssueDetailsForReportDOCNOREVISION(RecPK, reportspk);
                            dsMaterialIssue.ReadXml(new XmlTextReader(new StringReader(issueXml)));

                        }
                        else
                        {
                            dsMaterialIssue = new DataSet();
                            string issueXml = string.Empty;
                            issueXml = BusinessLogic.StoreManagement.MaterialIssue.GetMaterialIssueDetailsForReport(RecPK);
                            dsMaterialIssue.ReadXml(new XmlTextReader(new StringReader(issueXml)));
                        }
                        //dsMaterialIssue = new DataSet();
                        //string issueXml = string.Empty;
                        //issueXml = BusinessLogic.StoreManagement.MaterialIssue.GetMaterialIssueDetailsForReport(RecPK);
                        //dsMaterialIssue.ReadXml(new XmlTextReader(new StringReader(issueXml)));
                        break;
                    #endregion
                    #region STA
                    case ApplicationType.STA:
                        dsMaterialAccept = new DataSet();
                        string AcceptXml = string.Empty;
                        AcceptXml = BusinessLogic.StoreManagement.MaterialIssue.GetMaterialAcceptDetailsForReport(RecPK);
                        dsMaterialAccept.ReadXml(new XmlTextReader(new StringReader(AcceptXml)));
                        break;
                    #endregion
                    #region MTI
                    case ApplicationType.MTI:
                        dsMaterialIssue = new DataSet();
                        string MissueXml = string.Empty;
                        MissueXml = BusinessLogic.StoreManagement.MaterialIssue.GetMRIssueDetailsForReport(RecPK);
                        dsMaterialIssue.ReadXml(new XmlTextReader(new StringReader(MissueXml)));
                        break;
                    #endregion
                    #region GRN
                    case ApplicationType.GRN:
                        dsPurchaseRequest = BusinessLogic.StoreManagement.GoodsReceiptNote.GetGRNDetailsForNewReport(RecPK);
                        break;
                    #endregion
                    #region RMI
                    case ApplicationType.RMI:
                        dsReportDetails = BusinessLogic.POInvoicing.POInvoiceBL.GetDailyInspectionDetails(RecPK);//DailyInspectionBL.GetDailyInspectionDetails(RecPK);
                        break;
                    #endregion
                    #region RMIPM
                    case ApplicationType.RMIPM:
                        dsReportDetails = BusinessLogic.POInvoicing.POInvoiceBL.GetRMIPMDetails(RecPK);
                        break;
                    #endregion
                    #region GIN
                    case ApplicationType.GIN:
                        dsPurchaseRequest = BusinessLogic.StoreManagement.GoodsInspectionNote.GetGoodsInspectionNoteTables(RecPK);
                        // dsPurchaseRequest = BusinessLogic.StoreManagement.GoodsReceiptNote.GetGRNDetailsForNewReport(RecPK);
                        break;
                    #endregion
                    #region VP
                    case ApplicationType.EMI:
                        ////if (RptSubType == 2)
                        ////{
                        ////    dsPurchaseRequest = BusinessLogic.ReportsManagement.DeliveryOrderBL.GetDeliveryOrderDetails(RecPK);
                        ////}
                        ////else
                        ////{
                        //dsPurchaseRequest = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetIssuingReportByReqId(RecPK);
                        //// }
                        if (RptSubType == 3)
                        {
                            dsPurchaseRequest = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetEMIMultipleReport(RecPK);
                        }
                        else
                        {
                            dsPurchaseRequest = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetIssuingReportByReqId(RecPK);
                        }
                        break;
                    #endregion
                    #region SPLN
                    case ApplicationType.SPLN:
                        switch (RptSubType)
                        {
                            case 4:
                                AppTypeDetailsListNew = cm.GetReportParameters(RptType, RptSubType, AppvdDate, TrxCompanyPK);
                                dsShippingPlan = BusinessLogic.Shipping.ShippingPlanBL.GetPackingMaterialInShippingPlan(RecPK, RptSubType);
                                if (dsShippingPlan != null)
                                {
                                    if (dsShippingPlan.Tables[0].Rows.Count > 0)
                                    {
                                        AppvdDate = DateTime.Now.Date;
                                    }
                                }
                                break;
                            case 3:
                                dsShippingPlan = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanReceiptReport(RecPK);
                                break;
                            default:
                                ShippingPlanPK = Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK].ToString()) : 0;
                                dsLoadingPlan = BusinessLogic.Shipping.LoadingPlanBL.GetLoadingPlanReport((int?)null, RecPK);
                                break;
                        }
                        break;
                    #endregion
                    #region VSE
                    case ApplicationType.VSE:
                        dsVatSaleExport = BusinessLogic.Finance.VatSaleExportBL.GetVatSaleReport(startOfMonth, endOfMonth, currentUser);
                        break;
                    #endregion
                    #region PI,EI
                    case ApplicationType.PI:
                    case ApplicationType.EI:
                    case ApplicationType.ES:
                    case ApplicationType.EIT:
                    case ApplicationType.TPI:
                        dsDelivaryOrder = BusinessLogic.POInvoicing.POInvoiceBL.GetPurchaseInvoiceDtls(RecPK, GetGlobalResourceObject("ConfigurationsRes", "PurchaseInvoiceOutRPTSP").ToString());
                        break;
                    #endregion
                    #region CNDN
                    case ApplicationType.CN:
                    case ApplicationType.DN:
                    case ApplicationType.CNT:
                    case ApplicationType.DNT:
                        dsCNDN = BusinessLogic.Finance.CNDNBL.GetCNDNDetails(RecPK);
                        break;
                    #endregion
                    #region GSTReturns
                    case ApplicationType.GST:
                        dsGST = BusinessLogic.Finance.GSTReturnBL.GetGSTReturnReport(RecPK);
                        break;
                    #endregion
                    #region VTYPE
                    case ApplicationType.VTYPE:
                    case ApplicationType.VTYPESIJ:
                    case ApplicationType.VTYPEPSIJ:
                    case ApplicationType.VTYPEVPJ:
                    case ApplicationType.VTYPECRJ:
                    case ApplicationType.VTYPEMSIRJ:
                    case ApplicationType.VTYPEPCVJ:
                    case ApplicationType.VTYPEDPVJ:
                    case ApplicationType.VTYPEDRVJ:
                    case ApplicationType.VTYPEDNJPI:
                    case ApplicationType.VTYPECNJPI:
                    case ApplicationType.VTYPEDNJSI:
                    case ApplicationType.VTYPECNJSI:
                    case ApplicationType.VTYPEMIJ:
                    case ApplicationType.VTYPEEIJ:
                    case ApplicationType.VTYPEMSIJ:
                    case ApplicationType.VTYPEDPRJ:
                    case ApplicationType.VTYPECWIPJ:
                        FTH_REF_TYPE = Request.QueryString["FTH_REF_TYPE"] != string.Empty ? Request.QueryString["FTH_REF_TYPE"] : string.Empty;
                        VOUCHER_FROM = Request.QueryString["VOUCHER_FROM"] != string.Empty ? Request.QueryString["VOUCHER_FROM"] : string.Empty;
                        VOUCHER_TO = Request.QueryString["VOUCHER_TO"] != string.Empty ? Request.QueryString["VOUCHER_TO"] : string.Empty;
                        V_STATUS = Request.QueryString["V_STATUS"] != string.Empty ? Convert.ToInt32(Request.QueryString["V_STATUS"]) : 0;
                        dsVoucher = BusinessLogic.Jouralize.JournalizeBL.GetVoucherDetailsRPT(FTH_REF_TYPE, VOUCHER_FROM, VOUCHER_TO, V_STATUS, VoucherVersion);
                        break;
                    #endregion

                    #region CMP
                    case ApplicationType.CMP:
                        dsCmpPreparation = BusinessLogic.Production.CompoundPreparationBL.GetCmpPreparationReport(RecPK);
                        break;
                    #endregion
                    #region DISP
                    case ApplicationType.DISP:
                    case ApplicationType.BOM:
                        dsDispPreparation = BusinessLogic.Production.DispersionPreparation.GetDispPreparationReport(RecPK);
                        break;
                    #endregion
                    #region ACI
                    case ApplicationType.ACI:
                        dsAgentCommision = BusinessLogic.Sales.SaleOrderForAgtCommBL.GetAgentCommInvoicePrint(RecPK);
                        break;
                    #endregion
                    #region DPR
                    case ApplicationType.DPR:
                        dsDepreciation = BusinessLogic.Finance.DepreciationBL.GetDepreciationRptDetails(RecPK);
                        break;
                    #endregion
                    #region EMR
                    case ApplicationType.EMR:
                        dsExtMaterialRecive = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetIssuingReportByReqId(RecPK);
                        //dsExtMaterialRecive = BusinessLogic.Production.CompoundPreparationBL.GetCmpPreparationReport(RecPK);
                        break;
                    #endregion
                    #region DSA
                    case ApplicationType.DSA:
                        if (GetGlobalResourceObject("ConfigurationsRes", "ISMENUWISEDOCNOREVISION").ToString() == "1")
                        {
                            AppTypeDetailsListNew = cm.GetReportParameters(RptType, RptSubType, AppvdDate, TrxCompanyPK);
                            int reportspk = AppTypeDetailsListNew[0].AST_PK;
                            DataSet1 = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetDirectStockAdmissionReportDOCNOREVISION(RecPK, reportspk);
                        }
                        else
                        {
                            DataSet1 = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetDirectStockAdmissionReport(RecPK);
                        }
                        //  DataSet1 = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetDirectStockAdmissionReport(RecPK);
                        break;
                    #endregion
                    #region OS
                    case ApplicationType.OS:
                        dsOpeningStock = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetIssuingReportByReqId(RecPK);
                        break;
                    #endregion
                    #region STRLOMS
                    case ApplicationType.STRLOMS:
                        //dtStoreLocn = BusinessLogic.Administration.Masters.StoreLocationMaster.GetStoreLocationPrintLabelList(searchName, searchValue, bizUnit, deptCategory);
                        dtStoreLocn = BusinessLogic.Administration.Masters.StoreLocationMaster.GetStoreLocationPrintLabelList(searchName, searchValue, bizUnit, deptCategory);
                        break;
                    #endregion
                    #region BSRC
                    case ApplicationType.BSRC:
                        dtResult = BusinessLogic.Stock.StockBL.GetStockReconciliation(RecPK, DisplayTab);
                        break;
                    #endregion
                    #region Asset Service Request
                    case ApplicationType.SR:
                        dsServiceRequest = BusinessLogic.AssetService.ServiceRequestBL.GetAssetServiceRequestReport(RecPK);
                        break;
                    #endregion
                    #region Asset Service Order
                    case ApplicationType.SOA:
                        dsServiceOrder = BusinessLogic.AssetService.ServiceOrderBL.GetAssetServiceOrderReport(RecPK);
                        break;
                    #endregion
                    #region Asset Service Order Receipt
                    case ApplicationType.SRA:
                        dsServiceOrderReceipt = BusinessLogic.AssetService.ServiceOrderReceiptBL.GetAssetServiceOrderReceiptReport(RecPK);
                        break;
                    #endregion
                    #region Department wise Fund Request
                    case ApplicationType.FRD:
                        dsFundRequestDeptWise = BusinessLogic.Administration.Masters.FundRequisitionDeptBL.GetFundRequisitionDeptReport(RecPK);
                        break;
                    #endregion
                    #region BINCARDISSUE WO
                    case ApplicationType.BINCARDISSUEWO:
                        dsIssueReportDetails = BusinessLogic.ReportsManagement.GenerateReportBL.GetBinCardIssueOutputReport(RecPK.ToString());
                        break;
                    #endregion
                    #region CARTONISSUEWO
                    case ApplicationType.CARTONISSUEWO:
                        dsIssueReportDetails = BusinessLogic.ReportsManagement.GenerateReportBL.GetCartonIssueOutputReport(RecPK.ToString());
                        break;
                    #endregion
                    #region DONOTE
                    case ApplicationType.DONOTE:
                        dsWOIssue = new DataSet();
                        string WOIssueXml = string.Empty;
                        dsWOIssue = BusinessLogic.ReportsManagement.GenerateReportBL.GetIssueOutputReport(Session["WOIssuesXml"].ToString());
                        //dsWOIssue.ReadXml(new XmlTextReader(new StringReader(WOIssueXml)));                        
                        break;
                    #endregion
                    #region MCR
                    case ApplicationType.MCR:
                        dsReportDetails = BusinessLogic.ReportsManagement.GenerateReportBL.GetBinCardTraceReportData(BatchNo, Convert.ToInt32(currentUser.CurrentSBUPK));
                        break;
                    #endregion
                    #region ASD
                    case ApplicationType.ASD:
                        dsAssetDisposal = BusinessLogic.AssetService.ServiceRequestBL.GetAssetDisposalReport(RecPK);
                        break;
                    #endregion
                    #region CWIP
                    case ApplicationType.CWIP:
                        dsCWIP = BusinessLogic.AssetService.ServiceRequestBL.GetCWIPReport(RecPK);
                        break;
                    #endregion
                    #region SHCID                  
                    case ApplicationType.SHCID://@@                  
                        dsDelivaryOrder = BusinessLogic.Sales.SaleOrderBL.GetSalesInvoiceCustomsShippingPlan(RecPK, GetGlobalResourceObject("ConfigurationsRes", "SalesInvoiceOutCustomsRPTSP1SHPlan").ToString());
                        break;

                        #endregion
                }

                AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate, TrxCompanyPK);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        public bool SetFieldValues(string appType)
        {
            bool setResult = false;
            try
            {
                ReportViewer rvCurrentRptViewer = null;
                string _printerMode = string.Empty;
                if (!FromExternal)
                {
                    rvCurrentRptViewer = rvViewReport;
                    _printerMode = Request.QueryString["PRINTERMODE"];
                    divReportViewer.Visible = true;
                    rvCurrentRptViewer.Visible = true;
                    divNodata.Visible = false;
                }
                else
                {
                    rvCurrentRptViewer = new ReportViewer();
                }

                cm = new CommonService();
                locRpt = null;
                rvCurrentRptViewer.LocalReport.DataSources.Clear();
                locRpt = rvCurrentRptViewer.LocalReport;
                rvCurrentRptViewer.LocalReport.DataSources.Clear();
                locRpt.EnableExternalImages = true;

                rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                int CompanyPK = 0;
                switch (appType)
                {

                    #region BSRC
                    case ApplicationType.BSRC:
                        if (dtResult != null)
                        {
                            SetReportParameters(locRpt);
                            ReportDataSource dsStoreReqDetail = new ReportDataSource("DataSet1", dtResult);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsStoreReqDetail);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region PR
                    case ApplicationType.PR:
                    case ApplicationType.PRT:

                        ReportDataSource dsPRHeaderDtls;
                        ReportDataSource dsWorkFlowComments;
                        ReportDataSource dsPRMaterialDtls;
                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtPRHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtPRDtls = dsPurchaseRequest.Tables[1];
                            DataTable dtWorkFlowComment = dsPurchaseRequest.Tables[2];

                            dsPRHeaderDtls = new ReportDataSource("PRHeaderDtls", dtPRHeaderDtls);
                            dsPRMaterialDtls = new ReportDataSource("PRMaterialDtls", dtPRDtls);
                            dsWorkFlowComments = new ReportDataSource("WorkFlowComment", dtWorkFlowComment);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPRHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPRMaterialDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsWorkFlowComments);
                            if (dtPRHeaderDtls != null & dtPRHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtPRHeaderDtls.Rows[0]["PRH_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region MTR
                    case ApplicationType.MTR:
                        ReportDataSource dsMRHeaderDtls;
                        ReportDataSource dsMRWorkFlowComments;
                        ReportDataSource dsMRMaterialDtls;
                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtMRHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtMRDtls = dsPurchaseRequest.Tables[1];
                            DataTable dtMRWorkFlowComment = dsPurchaseRequest.Tables[2];

                            dsMRHeaderDtls = new ReportDataSource("PRHeaderDtls", dtMRHeaderDtls);
                            dsMRMaterialDtls = new ReportDataSource("PRMaterialDtls", dtMRDtls);
                            dsMRWorkFlowComments = new ReportDataSource("WorkFlowComment", dtMRWorkFlowComment);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsMRHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsMRMaterialDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsMRWorkFlowComments);
                            if (dtMRHeaderDtls != null & dtMRHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtMRHeaderDtls.Rows[0]["PRH_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region PO
                    case ApplicationType.PO:
                    case ApplicationType.POPG:
                    case ApplicationType.POTR:
                    case ApplicationType.POP:
                    case ApplicationType.POG:
                    case ApplicationType.POT:
                        ReportDataSource dsPOHeaderDtls;
                        ReportDataSource dsPOProductDtls;
                        ReportDataSource dsPOMoreDtls;
                        ReportDataSource dsPOTaxFullDtls;
                        ReportDataSource dsPOPRDtls;
                        ReportDataSource dsPOProductDtlsNew;
                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);
                            #region Generate QRCode
                            if (GetGlobalResourceObject("ConfigurationsRes", "POCodeType").ToString() == PrintCodeType.QrCode)
                            {
                                ReportParameterInfoCollection rptPC;
                                rptPC = locRpt.GetParameters();
                                foreach (ReportParameterInfo p in rptPC)
                                {
                                    if (p.Name == "QRCode")
                                    {
                                        byte[] qrCodeInBytes = null;
                                        System.Drawing.Image qrCodeImage = null;
                                        qrCodeImage = BarcodeLib.QRCodeLib.GetQRCode(dsPurchaseRequest.Tables[0].Rows[0]["POH_NO"].ToString());
                                        qrCodeInBytes = CommonFunctions.ImageToByte(qrCodeImage);
                                        ReportParameter QrCode = new ReportParameter("QRCode", Convert.ToBase64String(qrCodeInBytes));
                                        locRpt.SetParameters(QrCode);
                                    }
                                }
                            }
                            #endregion
                            DataTable dtPOHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtPOProductDtls = dsPurchaseRequest.Tables[1];
                            DataTable dtPOMoreDtls = dsPurchaseRequest.Tables[2];
                            DataTable dtPOPRDtls = dsPurchaseRequest.Tables[3];
                            DataTable dtPOTotTaxDtls;
                            DataTable dtPOProductDtlsNew = new DataTable();
                            if (dsPurchaseRequest.Tables.Count > 5)
                                dtPOProductDtlsNew = dsPurchaseRequest.Tables[5];
                            if (dsPurchaseRequest.Tables.Count > 4)
                            {
                                dtPOTotTaxDtls = dsPurchaseRequest.Tables[4];
                                dsPOTaxFullDtls = new ReportDataSource("TaxFullDtls", dtPOTotTaxDtls);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsPOTaxFullDtls);
                            }

                            if (dtPOProductDtls.Rows.Count > 0)
                            {
                                dtPOProductDtls.Columns.Add("PRExists");
                                if (dtPOPRDtls.Rows.Count > 1)
                                {
                                    for (int i = 0; i < dtPOProductDtls.Rows.Count; i++)
                                    {
                                        int PODPK = Convert.ToInt32(dtPOProductDtls.Rows[i]["POD_PK"]);
                                        var query = dtPOPRDtls.AsEnumerable().Where(x => x["POD_PK"].Equals(PODPK) && !string.IsNullOrEmpty(Convert.ToString(x["POD_PUR_REQ_DTL"])));
                                        if (query != null && query.Count() > 0)
                                        {
                                            DataTable dtPO = query.CopyToDataTable();
                                            if (dtPO.Rows.Count > 1)
                                            {
                                                var POItems = dtPO.AsEnumerable()
                                                   .GroupBy(row => new
                                                   {
                                                       POD_PUR_REQ_DATE = row.Field<DateTime>("POD_PUR_REQ_DATE"),
                                                       POD_ITEM = row.Field<int>("POD_ITEM")
                                                   }).ToList();
                                                if (POItems.Count > 1)
                                                {
                                                    dtPOProductDtls.Rows[i]["PRExists"] = 1;
                                                }
                                                else
                                                {
                                                    dtPOProductDtls.Rows[i]["PRExists"] = 0;
                                                }
                                            }
                                            else
                                            {
                                                dtPOProductDtls.Rows[i]["PRExists"] = 0;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < dtPOProductDtls.Rows.Count; i++)
                                    {
                                        dtPOProductDtls.Rows[i]["PRExists"] = 0;
                                    }
                                }
                            }
                            dsPOHeaderDtls = new ReportDataSource("POHeaderDtls", dtPOHeaderDtls);
                            dsPOProductDtls = new ReportDataSource("POProductDtls", dtPOProductDtls);
                            dsPOMoreDtls = new ReportDataSource("POMoreDtls", dtPOMoreDtls);

                            dsPOPRDtls = new ReportDataSource("PRDetails", dtPOPRDtls);
                            dsPOProductDtlsNew = new ReportDataSource("POProductDtlsNew", dtPOProductDtlsNew);
                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            //rvCurrentRptViewer.LocalReport.ExecuteReportInCurrentAppDomain(System.Reflection.Assembly.GetExecutingAssembly().Evidence); 
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPOHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPOProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPOMoreDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPOPRDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPOProductDtlsNew);
                            if (dtPOHeaderDtls != null & dtPOHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtPOHeaderDtls.Rows[0]["POH_COMPANY"].ToString());
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            rvCurrentRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region PO
                    case ApplicationType.SCWO:

                        ReportDataSource dsWOHeaderDtls;
                        ReportDataSource dsWOProductDtls;
                        ReportDataSource dsWOMoreDtls;
                        ReportDataSource dsWOTaxFullDtls;
                        ReportDataSource dsWOPRDtls;
                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);

                            DataTable dtWOHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtWOProductDtls = dsPurchaseRequest.Tables[1];
                            DataTable dtWOMoreDtls = dsPurchaseRequest.Tables[2];
                            DataTable dtWOPRDtls = dsPurchaseRequest.Tables[3];
                            DataTable dtWOTotTaxDtls;
                            if (dsPurchaseRequest.Tables.Count > 4)
                            {
                                dtWOTotTaxDtls = dsPurchaseRequest.Tables[4];
                                dsWOTaxFullDtls = new ReportDataSource("TaxFullDtls", dtWOTotTaxDtls);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsWOTaxFullDtls);
                            }

                            //if (dtWOProductDtls.Rows.Count > 0)
                            //{
                            //    dtWOProductDtls.Columns.Add("PRExists");
                            //    if (dtWOPRDtls.Rows.Count > 1)
                            //    {
                            //        for (int i = 0; i < dtWOProductDtls.Rows.Count; i++)
                            //        {
                            //            int PODPK = Convert.ToInt32(dtWOProductDtls.Rows[i]["POD_PK"]);
                            //            var query = dtWOPRDtls.AsEnumerable().Where(x => x["POD_PK"].Equals(PODPK) && !string.IsNullOrEmpty(Convert.ToString(x["POD_PUR_REQ_DTL"])));
                            //            if (query != null && query.Count() > 0)
                            //            {
                            //                DataTable dtPO = query.CopyToDataTable();
                            //                if (dtPO.Rows.Count > 1)
                            //                {
                            //                    var POItems = dtPO.AsEnumerable()
                            //                       .GroupBy(row => new
                            //                       {
                            //                           POD_PUR_REQ_DATE = row.Field<DateTime>("POD_PUR_REQ_DATE"),
                            //                           POD_ITEM = row.Field<int>("POD_ITEM")
                            //                       }).ToList();
                            //                    if (POItems.Count > 1)
                            //                    {
                            //                        dtPOProductDtls.Rows[i]["PRExists"] = 1;
                            //                    }
                            //                    else
                            //                    {
                            //                        dtPOProductDtls.Rows[i]["PRExists"] = 0;
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    dtPOProductDtls.Rows[i]["PRExists"] = 0;
                            //                }
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        for (int i = 0; i < dtPOProductDtls.Rows.Count; i++)
                            //        {
                            //            dtPOProductDtls.Rows[i]["PRExists"] = 0;
                            //        }
                            //    }
                            //}
                            dsWOHeaderDtls = new ReportDataSource("WOHeaderDtls", dtWOHeaderDtls);
                            dsWOProductDtls = new ReportDataSource("WOProductDtls", dtWOProductDtls);
                            dsWOMoreDtls = new ReportDataSource("WOMoreDtls", dtWOMoreDtls);

                            dsWOPRDtls = new ReportDataSource("WODetails", dtWOPRDtls);

                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsWOHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsWOProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsWOMoreDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsWOPRDtls);
                            if (dtWOHeaderDtls != null & dtWOHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtWOHeaderDtls.Rows[0]["WIH_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                            //rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            //rvCurrentRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region SO
                    case ApplicationType.SO:
                    case ApplicationType.SOD:
                        ReportDataSource dsSOHeaderDtls;
                        ReportDataSource dsSOProductDtls;
                        ReportDataSource dsSOMoreDtls;
                        if (dsSaleOrder != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtSOHeaderDtls = dsSaleOrder.Tables[0];
                            DataTable dtSOProductDtls = dsSaleOrder.Tables[1];
                            DataTable dtSOMoreDtls = dsSaleOrder.Tables[2];

                            dsSOHeaderDtls = new ReportDataSource("SOHeaderDtls", dtSOHeaderDtls);
                            dsSOProductDtls = new ReportDataSource("SOProductDtls", dtSOProductDtls);
                            dsSOMoreDtls = new ReportDataSource("SOMoreDtls", dtSOMoreDtls);

                            // rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSOHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSOProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSOMoreDtls);
                            if (dtSOHeaderDtls != null & dtSOHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtSOHeaderDtls.Rows[0]["SOH_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region VNDEVAL
                    case ApplicationType.VNDEVAL:
                        if (dsVendorEval != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtReportDtls = dsVendorEval.Tables[0];
                            ReportDataSource dsStoreReqHeader = new ReportDataSource("VendorEvalHeader", dsVendorEval.Tables[0]);
                            ReportDataSource dsStoreReqDetail = new ReportDataSource("VendorEvalDetail", dsVendorEval.Tables[1]);
                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsStoreReqHeader);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsStoreReqDetail);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region IO
                    case ApplicationType.IO:
                        if (dsSaleOrder != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtSOHeaderDtls = dsSaleOrder.Tables[0];
                            DataTable dtSOProductDtls = dsSaleOrder.Tables[1];
                            DataTable dtSOMoreDtls = dsSaleOrder.Tables[2];
                            dsSOHeaderDtls = new ReportDataSource("SOHeaderDtls", dtSOHeaderDtls);
                            dsSOProductDtls = new ReportDataSource("SOProductDtls", dtSOProductDtls);
                            dsSOMoreDtls = new ReportDataSource("SOMoreDtls", dtSOMoreDtls);

                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSOHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSOProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSOMoreDtls);
                            if (dtSOHeaderDtls != null & dtSOHeaderDtls.Rows.Count > 0)
                            {
                                int CmpnyPk = Convert.ToInt32(dtSOHeaderDtls.Rows[0]["SOH_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CmpnyPk));
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;

                    #endregion
                    #region QR
                    case ApplicationType.RFQ:
                        ReportDataSource dsQRHeaderDtls;
                        ReportDataSource dsQRDtls;
                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtQRHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtQRDtls = dsPurchaseRequest.Tables[1];

                            dsQRHeaderDtls = new ReportDataSource("QRHeaderDtls", dtQRHeaderDtls);
                            dsQRDtls = new ReportDataSource("QRDetails", dtQRDtls);
                            if (dtQRHeaderDtls != null & dtQRHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtQRHeaderDtls.Rows[0]["RFH_COMPANY"].ToString());
                            }

                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsQRHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsQRDtls);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region CQTN
                    case ApplicationType.CQTN:
                        ReportDataSource dsCQTN;
                        currentEntity = new ERPEntities();
                        if (RevPK == 0)
                        {
                            dsCQTN = new ReportDataSource("QRHeaderDtls", currentEntity.SPCRM_QUOTATION_RPT(RecPK));
                        }
                        else
                        {
                            dsCQTN = new ReportDataSource("QRHeaderDtls", currentEntity.SPCRM_QUOTATION_ARCHIVE_RPT(RecPK, (byte)RevPK));
                        }
                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsCQTN);

                        dsCQTN = new ReportDataSource("QRTaxDtls", currentEntity.SPCRM_QUOTATION_TAX_DTL(RecPK));

                        //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsCQTN);

                        if (dsCQTN != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);

                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region QAC
                    case ApplicationType.QAC:
                        ReportDataSource dsQACHeader;
                        ReportDataSource dsQACDtls;
                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtGRNHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtGRNDtls = dsPurchaseRequest.Tables[1];
                            dsQACHeader = new ReportDataSource("QACHdr", dtGRNHeaderDtls);
                            dsQACDtls = new ReportDataSource("QACDtls", dtGRNDtls);
                            if (dtGRNHeaderDtls != null & dtGRNHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtGRNHeaderDtls.Rows[0]["RFH_COMPANY"].ToString());
                            }

                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsQACHeader);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsQACDtls);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region GRN
                    case ApplicationType.GRN:
                        ReportDataSource dsGRNHeaderDtls;
                        ReportDataSource dsGRNDtls;
                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtGRNHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtGRNDtls = dsPurchaseRequest.Tables[1];
                            dsGRNHeaderDtls = new ReportDataSource("GRNHdr", dtGRNHeaderDtls);
                            dsGRNDtls = new ReportDataSource("GRNDtls", dtGRNDtls);

                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGRNHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGRNDtls);
                            if (dtGRNHeaderDtls != null & dtGRNHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtGRNHeaderDtls.Rows[0]["GRH_COMPANY"].ToString());
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region RMI
                    case ApplicationType.RMI:
                        if (dsReportDetails != null)
                        {
                            SetReportParameters(locRpt);
                            dtHeader = dsReportDetails.Tables[0];
                            dtDetails1 = dsReportDetails.Tables[1];
                            if (dsReportDetails.Tables.Count > 2)
                                dtDetails2 = dsReportDetails.Tables[2];
                            if (dsReportDetails.Tables.Count > 3)
                                dtDetails3 = dsReportDetails.Tables[3];
                            rdsHeader = new ReportDataSource("DataSet1", dtHeader);
                            rdsDetails1 = new ReportDataSource("DataSet2", dtDetails1);
                            rdsDetails2 = new ReportDataSource("DataSet3", dtDetails2);
                            rdsDetails3 = new ReportDataSource("DataSet4", dtDetails3);
                            rvViewReport.LocalReport.DataSources.Add(rdsHeader);
                            rvViewReport.LocalReport.DataSources.Add(rdsDetails1);
                            rvViewReport.LocalReport.DataSources.Add(rdsDetails2);
                            rvViewReport.LocalReport.DataSources.Add(rdsDetails3);
                            rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(1));
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region RMI
                    case ApplicationType.RMIPM:
                        if (dsReportDetails != null)
                        {
                            SetReportParameters(locRpt);
                            dtHeader = dsReportDetails.Tables[0];
                            dtDetails1 = dsReportDetails.Tables[1];
                            if (dsReportDetails.Tables.Count > 2)
                                dtDetails2 = dsReportDetails.Tables[2];
                            if (dsReportDetails.Tables.Count > 3)
                                dtDetails3 = dsReportDetails.Tables[3];
                            rdsHeader = new ReportDataSource("DataSet1", dtHeader);
                            rdsDetails1 = new ReportDataSource("DataSet2", dtDetails1);
                            rdsDetails2 = new ReportDataSource("DataSet3", dtDetails2);
                            rdsDetails3 = new ReportDataSource("DataSet4", dtDetails3);
                            rvViewReport.LocalReport.DataSources.Add(rdsHeader);
                            rvViewReport.LocalReport.DataSources.Add(rdsDetails1);
                            rvViewReport.LocalReport.DataSources.Add(rdsDetails2);
                            rvViewReport.LocalReport.DataSources.Add(rdsDetails3);
                            rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(1));
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region GIN
                    case ApplicationType.GIN:
                        ReportDataSource dsGINHeaderDtls;
                        ReportDataSource dsGINDtls;

                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtGINHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtGINDtls = dsPurchaseRequest.Tables[1];

                            dsGINHeaderDtls = new ReportDataSource("GINHdr", dtGINHeaderDtls);
                            dsGINDtls = new ReportDataSource("GINDtls", dtGINDtls);

                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGINHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGINDtls);
                            if (dtGINHeaderDtls != null & dtGINHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtGINHeaderDtls.Rows[0]["GIH_COMPANY"].ToString());
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region COA
                    case ApplicationType.COA:
                        ReportDataSource dsCOA;
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=coa&appsubtype=");
                        DataTable dtCOAData = BusinessLogic.ReportsManagement.GenerateReportBL.GetAccountDtls(currentUser.SBUID);
                        dsCOA = new ReportDataSource("COADtls", dtCOAData);
                        //currentEntity = new ERPEntities();
                        //dsCOA = new ReportDataSource("COADtls", currentEntity.SPFIN_COA_LIST_RPT(currentUser.SBUID));
                        if (dsCOA != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);

                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCOA);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(currentUser.SBUID));
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region JV                              // DotMatrix Printing Enabled
                    case ApplicationType.JV:
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
                    case ApplicationType.DPRJ:
                    case ApplicationType.CWIPJ:
                    case ApplicationType.ASDJ:
                    case ApplicationType.ACIJ:
                    case ApplicationType.AIPJ:
                    case ApplicationType.DNSJYE:
                    case ApplicationType.CNSJYE:
                    case ApplicationType.CNPJYE:
                    case ApplicationType.DNPJYE:
                    case ApplicationType.DPVCJ:
                    case ApplicationType.DPBJ:
                    case ApplicationType.DRVJ://Direct Receipt Voucher
                    case ApplicationType.YCV://Year Closing Voucher
                    case ApplicationType.CLSTJ:
                    case ApplicationType.PAYRLJ:
                    case ApplicationType.SALPYMTJ:
                    case ApplicationType.MIJ:
                        if (!string.IsNullOrWhiteSpace(_printerMode) && _printerMode == PrinterMode.DOTMATRIX.ToString())
                        {
                            #region Dot Matrix Printing

                            #region Commented
                            //#region Getting Report Parameters
                            //DataSet dsParamSettings;
                            //string footer;
                            //string rptName = string.Empty;
                            //footer = string.Empty;
                            //string signaturePath = string.Empty;
                            //dsParamSettings = new DataSet();
                            //AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);

                            //dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(AppTypeDetailsList[0].AST_RPT_SETTINGS)));
                            //JournalVoucher reportData = new JournalVoucher();

                            //if (dsParamSettings.Tables.Count > 0)
                            //{
                            //    if (dsParamSettings.Tables[0].Columns.Contains("LOGO_HIDE"))
                            //    {
                            //        reportData.HideLogo = dsParamSettings.Tables[0].Rows[0]["LOGO_HIDE"].ToString();
                            //        reportData.HideLogo = reportData.HideLogo == "True" ? "none" : "inherit";
                            //    }
                            //    if (dsParamSettings.Tables[0].Columns.Contains("HEADING_HIDE"))
                            //    {
                            //        reportData.HideHeadTitle = dsParamSettings.Tables[0].Rows[0]["HEADING_HIDE"].ToString();
                            //        reportData.HideHeadTitle = reportData.HideHeadTitle == "True" ? "none" : "inherit";
                            //    }
                            //    if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING_HIDE"))
                            //    {
                            //        reportData.HideSubTitle = dsParamSettings.Tables[0].Rows[0]["SUB_HEADING_HIDE"].ToString();
                            //        reportData.HideSubTitle = reportData.HideSubTitle == "True" ? "none" : "inherit";
                            //    }
                            //    if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_LEFT_HIDE"))
                            //    {
                            //        reportData.HideFooterText = dsParamSettings.Tables[0].Rows[0]["FOOTER_LEFT_HIDE"].ToString();
                            //        reportData.HideFooterText = reportData.HideFooterText == "True" ? "none" : "inherit";
                            //    }
                            //    if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_RIGHT_HIDE"))
                            //    {
                            //        reportData.HidePageNo = dsParamSettings.Tables[0].Rows[0]["FOOTER_RIGHT_HIDE"].ToString();
                            //        reportData.HidePageNo = reportData.HidePageNo == "True" ? "none" : "inherit";
                            //    }
                            //    if (dsParamSettings.Tables[0].Columns.Contains("HEADING"))
                            //    {
                            //        reportData.HeadTitle = dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString();
                            //    }
                            //    if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING"))
                            //    {
                            //        reportData.SubTitle = dsParamSettings.Tables[0].Rows[0]["SUB_HEADING"].ToString();
                            //    }

                            //    if (dsParamSettings.Tables[0].Columns.Contains("REPORT_WIDTH"))
                            //    {
                            //        reportData.PrinterSettings.DocumentWidth = dsParamSettings.Tables[0].Rows[0]["REPORT_WIDTH"].ToString();
                            //    }
                            //    if (dsParamSettings.Tables[0].Columns.Contains("REPORT_HEIGHT"))
                            //    {
                            //        reportData.PrinterSettings.DocumentHeight = dsParamSettings.Tables[0].Rows[0]["REPORT_HEIGHT"].ToString();
                            //    }
                            //    if (dsParamSettings.Tables[0].Columns.Contains("REPORT_WIDTH_PX"))
                            //    {
                            //        reportData.PrinterSettings.WindowWidth = dsParamSettings.Tables[0].Rows[0]["REPORT_WIDTH_PX"].ToString();
                            //    }
                            //    if (dsParamSettings.Tables[0].Columns.Contains("REPORT_HEIGHT_PX"))
                            //    {
                            //        reportData.PrinterSettings.WindowHeight = dsParamSettings.Tables[0].Rows[0]["REPORT_HEIGHT_PX"].ToString();
                            //    }
                            //}
                            //#endregion

                            //#region FormatCalculation
                            //string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                            ////string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
                            //string currencyformat = "#" + currencysep + "#0.";
                            //string NoFormat = "#" + currencysep + "#0.";
                            //string ExchRateDigt = "#" + currencysep + "#0.";
                            //string RateDeciDigt = "#" + currencysep + "#0.";
                            //string RateDecDigitPP = "#" + currencysep + "#0.";
                            //string currencydecimals = "";
                            //string Nodecimal = string.Empty;
                            //string ExchRateDigit = string.Empty;
                            //string RateDecimalDigit = string.Empty;
                            //string RateDecimalDigitPP = string.Empty;
                            //int QuantityComma;
                            //int RateComma;
                            //int CurrencyComma;
                            //DataTable dt = ConfigurationSettings();
                            //if (dt != null && dt.Rows.Count > 0)
                            //{
                            //    int curdigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                            //    for (int i = 0; i < curdigit; i++)
                            //    {
                            //        currencydecimals += "0";
                            //    }

                            //    int NoDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());
                            //    for (int i = 0; i < NoDigit; i++)
                            //    {
                            //        Nodecimal += "0";
                            //    }
                            //    int ExchRate = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                            //    for (int i = 0; i < ExchRate; i++)
                            //    {
                            //        ExchRateDigit += "0";
                            //    }

                            //    int RateDecimal = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                            //    for (int i = 0; i < RateDecimal; i++)
                            //    {
                            //        RateDecimalDigit += "0";
                            //    }
                            //    int RateDecimalPP = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
                            //    for (int i = 0; i < RateDecimalPP; i++)
                            //    {
                            //        RateDecimalDigitPP += "0";
                            //    }
                            //    //QuantityComma = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "Comma in quantity values")["ACF_VALUE"].ToString());
                            //    //RateComma = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "Comma in Rate values")["ACF_VALUE"].ToString());
                            //    //CurrencyComma = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "Comma in Currency values")["ACF_VALUE"].ToString());
                            //}
                            //else
                            //{
                            //    currencydecimals = "00";
                            //    Nodecimal = "00";
                            //}
                            //currencyformat = currencyformat + currencydecimals;
                            //NoFormat = NoFormat + Nodecimal;
                            //ExchRateDigt = ExchRateDigt + ExchRateDigit;
                            //RateDeciDigt = RateDeciDigt + RateDecimalDigit;
                            //RateDecDigitPP = RateDecDigitPP + RateDecimalDigitPP;
                            //#endregion
                            // currencyformat = {0:n} + currencydecimals;
                            //#region Setting Report Data
                            //reportData.DateFormat = Resources.Constants.ReportDateFormat;
                            //reportData.CurrencyFormat = currencyformat;
                            //reportData.NumberFormat = NoFormat;

                            //if (RptType == ApplicationType.VSE || RptType == ApplicationType.PI || RptType == ApplicationType.CN ||
                            //                RptType == ApplicationType.DN || RptType == ApplicationType.SI || RptType == ApplicationType.MSI ||
                            //                RptType == ApplicationType.SIJ || RptType == ApplicationType.MSIJ || RptType == ApplicationType.CRJ ||
                            //                RptType == ApplicationType.JV || RptType == ApplicationType.SO || RptType == ApplicationType.PIJ ||
                            //                RptType == ApplicationType.PSIJ || RptType == ApplicationType.DNJ || RptType == ApplicationType.CNJ ||
                            //                RptType == ApplicationType.PDCCJ || RptType == ApplicationType.RCBJ || RptType == ApplicationType.PSAS ||
                            //                RptType == ApplicationType.PCS || RptType == ApplicationType.VPJ || RptType == ApplicationType.SIPJ ||
                            //                RptType == ApplicationType.EIPJ || RptType == ApplicationType.DPVJ || RptType == ApplicationType.PCVJ ||
                            //                RptType == ApplicationType.EIJ || RptType == ApplicationType.PPCCJ || RptType == ApplicationType.PCBJ ||
                            //                RptType == ApplicationType.PO || RptType == ApplicationType.MSIRJ || RptType == ApplicationType.MI ||
                            //                RptType == ApplicationType.FCHRJ)//@@
                            //{
                            //    reportData.ExchangeRate = ExchRateDigt;
                            //    reportData.RateFormat = RateDeciDigt;
                            //}

                            //reportData.Logo = "../Reports/Images/" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoPDF"];
                            //reportData.FooterText = "Printed by " + currentUser.EmpName + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");

                            //currentEntity = new ERPEntities();
                            //List<SPFIN_TRX_VOUCHER_RPT_Result> reportDataSourceList = currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK).ToList();
                            //SPFIN_TRX_VOUCHER_RPT_Result reportDataSource = reportDataSourceList[0];

                            //reportData.JournalNoLabel = string.Format("{0}-Journal No", TrxRefType);
                            //reportData.VoucherNo = reportDataSource.FTH_VOUCHER_NO ?? CommonConstants.HTML_SPACE;
                            //reportData.FromTo = TrxRefType == "CNJ" ? "From" : "To";
                            //reportData.FromToValue = reportDataSource.FTH_CUS_VND_TEXT ?? CommonConstants.HTML_SPACE;
                            //reportData.Date = reportDataSource.FTH_DATE.HasValue ? reportDataSource.FTH_DATE.Value.ToString(reportData.DateFormat) : CommonConstants.HTML_SPACE;
                            //reportData.TrxCurrency = reportDataSource.FTH_TRX_CURR_TEXT ?? CommonConstants.HTML_SPACE;

                            //decimal? _totalDebitAmt = reportDataSourceList.Sum(x => x.FTR_DR_AMT_BC ?? (decimal?)0) ?? 0;
                            //reportData.DebitTotal = _totalDebitAmt.HasValue ? _totalDebitAmt.Value.ToString(reportData.CurrencyFormat) : CommonConstants.HTML_SPACE;
                            //reportData.Amount = reportData.DebitTotal;
                            //reportData.RefNo = reportDataSource.FTH_REF_NO ?? CommonConstants.HTML_SPACE;
                            //reportData.RefDate = reportDataSource.FTH_REF_DATE.ToString(reportData.DateFormat);

                            ////Setting Child Data

                            //foreach (SPFIN_TRX_VOUCHER_RPT_Result subItem in reportDataSourceList)
                            //{
                            //    reportData.ItemDetails.Add(new JournalVoucherItem
                            //    {
                            //        AccCode = subItem.FTR_ACCOUNT_CODE,
                            //        AccName = subItem.FTR_ACCOUNT_NAME,
                            //        Credit = subItem.FTR_CR_AMT_BC.HasValue ? subItem.FTR_CR_AMT_BC.Value.ToString(currencyformat) : CommonConstants.HTML_SPACE,
                            //        Debit = subItem.FTR_DR_AMT_BC.HasValue ? subItem.FTR_DR_AMT_BC.Value.ToString(currencyformat) : CommonConstants.HTML_SPACE,
                            //        Description = subItem.FTR_NARRATION ?? CommonConstants.HTML_SPACE,
                            //        ExRate = subItem.FTR_EXCHG_RATE.HasValue ? subItem.FTR_EXCHG_RATE.Value.ToString(reportData.ExchangeRate) : CommonConstants.HTML_SPACE
                            //    });
                            //}

                            //reportData.BaseCurrency = reportDataSource.FTH_BASE_CURR_TEXT ?? CommonConstants.HTML_SPACE;

                            //reportData.AmountInWords = new NumberToWordsConvertorFactory(reportData.BaseCurrency)
                            //                               .GetNumberToWordsConvertor()
                            //                               .ConvertNumberToWords(_totalDebitAmt.Value.ToString());

                            //reportData.Remarks = reportDataSource.FTH_REMARKS ?? CommonConstants.HTML_SPACE;
                            //reportData.RemarksHide = string.IsNullOrWhiteSpace(reportData.Remarks) ? "none" : "inherit";
                            //reportData.Narration = reportDataSource.FTH_NARRATION ?? CommonConstants.HTML_SPACE;
                            //reportData.NarrationHide = string.IsNullOrWhiteSpace(reportData.Narration) ? "none" : "inherit";

                            //reportData.PreparedBy = reportDataSource.FTH_TASK1_BY_TEXT ?? CommonConstants.HTML_SPACE;
                            //reportData.ReviewedBy = reportDataSource.FTH_TASK2_BY_TEXT ?? CommonConstants.HTML_SPACE;
                            //reportData.ApprovedBy = reportDataSource.FTH_TASK3_BY_TEXT ?? CommonConstants.HTML_SPACE;

                            //reportData.PreparedDate = reportDataSource.FTH_TASK1_DT_TEXT.HasValue
                            //                            ? reportDataSource.FTH_TASK1_DT_TEXT.Value.ToString(reportData.DateFormat)
                            //                            : CommonConstants.HTML_SPACE;
                            //reportData.ReviewedDate = reportDataSource.FTH_TASK2_DT_TEXT.HasValue
                            //                            ? reportDataSource.FTH_TASK2_DT_TEXT.Value.ToString(reportData.DateFormat)
                            //                            : CommonConstants.HTML_SPACE;
                            //reportData.ApprovedDate = reportDataSource.FTH_TASK3_DT_TEXT.HasValue
                            //                            ? reportDataSource.FTH_TASK3_DT_TEXT.Value.ToString(reportData.DateFormat)
                            //                            : CommonConstants.HTML_SPACE; 
                            //#endregion

                            //#region Build Report Body

                            //string _rowData = string.Empty;
                            //string temp = reportData.GetRowTemplate();
                            //foreach (var subItem in reportData.ItemDetails)
                            //{
                            //    _rowData += string.Format(reportData.GetRowTemplate(),
                            //    subItem.AccCode,
                            //    subItem.AccName,
                            //    string.IsNullOrWhiteSpace(subItem.Description) ? CommonConstants.HTML_SPACE : subItem.Description,
                            //    subItem.ExRate,
                            //    subItem.Debit,
                            //    subItem.Credit
                            //    );
                            //};

                            //string reoportBody = string.Format(reportData.GetBodyTemplate(),
                            //                                               reportData.Logo,
                            //                                               reportData.HeadTitle,
                            //                                               reportData.FromTo,
                            //                                               reportData.FromToValue,
                            //                                               reportData.JournalNoLabel,
                            //                                               reportData.VoucherNo,
                            //                                               reportData.Date,
                            //                                               reportData.TrxCurrency,
                            //                                               reportData.Amount,
                            //                                               reportData.RefNo,
                            //                                               reportData.RefDate,
                            //                                               reportData.BaseCurrency,
                            //                                               reportData.DebitTotal,
                            //                                               reportData.DebitTotal,
                            //                                               reportData.AmountInWords,
                            //                                               reportData.Remarks,
                            //                                               reportData.Narration,
                            //                                               reportData.PreparedBy,
                            //                                               reportData.ReviewedBy,
                            //                                               reportData.ApprovedBy,
                            //                                               reportData.PreparedDate,
                            //                                               reportData.ReviewedDate,
                            //                                               reportData.ApprovedDate,
                            //                                               reportData.FooterText,
                            //                                               reportData.HideLogo,
                            //                                               reportData.HideHeadTitle,
                            //                                               reportData.RemarksHide,
                            //                                               reportData.NarrationHide,
                            //                                               _rowData
                            //                                           );
                            //#endregion

                            //string script = "printVoucher(\"" + reoportBody + "\",{ width:'" + reportData.PrinterSettings.DocumentWidth + "', height:'" + reportData.PrinterSettings.DocumentHeight + "', windowWidth:'" + reportData.PrinterSettings.WindowWidth + "', windowHeight:'" + reportData.PrinterSettings.WindowHeight + "' });";

                            #endregion
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            DataTable dt = ConfigurationSettings();
                            currentEntity = new ERPEntities();
                            List<SPFIN_TRX_VOUCHER_RPT_Result> reportDataSourceList = currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK, VoucherVersion).ToList();

                            if (reportDataSourceList != null && reportDataSourceList.Count > 0)
                            {
                                string htmlTemplate = new DotMatrixReportBuilderFactory(appType)
                                                                       .GetReportDataBuilder()
                                                                       .GetReportTemplate(new CommonReportParameter
                                                                       {
                                                                           AppTypeDetailsList = AppTypeDetailsList,
                                                                           PrintedUser = this.currentUser.EmpName,
                                                                           dataTable = dt,
                                                                           reportDataSourceList = reportDataSourceList,
                                                                           TrxRefType = TrxRefType
                                                                       });

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Title", htmlTemplate, true);
                            }
                            return false;
                            #endregion
                        }
                        else
                        {
                            #region Normal Printing
                            ReportDataSource dsJV;
                            currentEntity = new ERPEntities();
                            List<SPFIN_TRX_VOUCHER_RPT_Result> lstData = currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK, VoucherVersion).ToList();
                            dsJV = new ReportDataSource("JVHeader", lstData);
                            if (lstData != null & lstData.Count > 0)
                            {
                                CompanyPK = lstData[0].FTH_COMPANY;
                            }
                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            if (dsJV != null)
                            {
                                AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                SetReportParameters(locRpt);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsJV);
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region SI,MSI,DSI,MSIT
                    case ApplicationType.SI:
                    case ApplicationType.SIC:
                    case ApplicationType.MSI://@@
                    case ApplicationType.DSI:
                    case ApplicationType.SIM:
                    case ApplicationType.SIMSS:
                    case ApplicationType.SIMSF:
                    case ApplicationType.MSIT:
                    case ApplicationType.DSID:
                        ReportDataSource dsSIHeaderDtls;
                        ReportDataSource dsSIProductDtls;
                        ReportDataSource dsSIOtherDtls;
                        ReportDataSource dsTaxDtls;
                        ReportDataSource dsTaxFullDtls;
                        ReportDataSource dsTotTaxDtls;
                        if (dsDelivaryOrder != null)
                        {
                            DataTable dtSIHeaderDtls = dsDelivaryOrder.Tables[0];
                            DataTable dtSIProductDtls = dsDelivaryOrder.Tables[1];
                            DataTable dtOtherDtls = dsDelivaryOrder.Tables[2];
                            DataTable dtTaxDtls = dsDelivaryOrder.Tables[3];
                            DataTable dtTotTaxDtls = dsDelivaryOrder.Tables[4];
                            dsSIHeaderDtls = new ReportDataSource("DOHeaderDtls", dtSIHeaderDtls);
                            dsSIProductDtls = new ReportDataSource("DOProductDtls", dtSIProductDtls);
                            dsSIOtherDtls = new ReportDataSource("DOOtherDtls", dtOtherDtls);
                            dsTaxDtls = new ReportDataSource("TaxDtls", dtTaxDtls);
                            dsTaxFullDtls = new ReportDataSource("TaxFullDtls", dtTotTaxDtls);
                            if (!string.IsNullOrEmpty(Convert.ToString(dtSIHeaderDtls.Rows[0]["DPH_SWAP_BUYER"])))
                            {
                                SwapBuyer = Convert.ToInt32(dtSIHeaderDtls.Rows[0]["DPH_SWAP_BUYER"].ToString());
                            }
                            Buyer = dtSIHeaderDtls.Rows[0]["DPH_ADNL_BUYER"].ToString();
                            if (!string.IsNullOrEmpty(Convert.ToString(dtSIHeaderDtls.Rows[0]["DPH_PRINT_SHIP_TO"])))
                            {
                                PrintShipTo = Convert.ToBoolean(dtSIHeaderDtls.Rows[0]["DPH_PRINT_SHIP_TO"]);
                            }
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIOtherDtls);
                            if (dtSIHeaderDtls != null & dtSIHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtSIHeaderDtls.Rows[0]["DPH_COMPANY"].ToString());
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsTaxDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsTaxFullDtls);
                            var groupdata = (from t in dtTotTaxDtls.AsEnumerable()
                                             group t by new
                                             {
                                                 ISH_TAX = t.Field<int?>("ISH_TAX"),
                                             } into dt
                                             select new
                                             {
                                                 dt.Key.ISH_TAX,
                                                 TAX_CODE = dt.First().Field<string>("TAX_CODE"),
                                                 ISH_NAME = dt.First().Field<string>("ISH_NAME"),
                                                 TAX_DISP_NAME = dt.First().Field<string>("TAX_DISP_NAME"),
                                                 TAX_RATE = dt.First().Field<decimal?>("TAX_RATE"),
                                                 ISH_TAX_AMT = dt.Sum(x => x.Field<decimal>("ISH_TAX_AMT")),
                                                 ISH_TOTAL = dt.Sum(x => x.Field<decimal>("ISH_TOTAL"))
                                             }).ToList();
                            DataTable dtGrpTax = groupdata.ToDataTable();
                            dsTotTaxDtls = new ReportDataSource("TotTaxDtls", dtGrpTax);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsTotTaxDtls);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }

                        break;
                    #endregion

                    #region CID
                    case ApplicationType.CID:
                        ReportDataSource dsCIDHeaderDtls;
                        ReportDataSource dsCIDProductDtls;
                        ReportDataSource dsCIDOtherDtls;
                        ReportDataSource dsCIDTaxDtls;
                        ReportDataSource dsCIDTaxFullDtls;
                        ReportDataSource dsCIDTotTaxDtls;
                        ReportDataSource dsCIDCustomsDtls;
                        if (dsDelivaryOrder != null)
                        {
                            DataTable dtCIDHeaderDtls = dsDelivaryOrder.Tables[0];
                            DataTable dtCIDProductDtls = dsDelivaryOrder.Tables[1];
                            DataTable dtCIDOtherDtls = dsDelivaryOrder.Tables[2];
                            DataTable dtCIDTaxDtls = dsDelivaryOrder.Tables[3];
                            DataTable dtCIDTotTaxDtls = dsDelivaryOrder.Tables[4];
                            DataTable dtCIDCustDtls = dsDelivaryOrder.Tables[5];

                            dsCIDHeaderDtls = new ReportDataSource("DOHeaderDtls", dtCIDHeaderDtls);
                            dsCIDProductDtls = new ReportDataSource("DOProductDtls", dtCIDProductDtls);
                            dsCIDOtherDtls = new ReportDataSource("DOOtherDtls", dtCIDOtherDtls);
                            dsCIDTaxDtls = new ReportDataSource("TaxDtls", dtCIDTaxDtls);
                            dsCIDTaxFullDtls = new ReportDataSource("TaxFullDtls", dtCIDTotTaxDtls);
                            dsCIDCustomsDtls = new ReportDataSource("CustDtls", dtCIDCustDtls);
                            if (!string.IsNullOrEmpty(Convert.ToString(dtCIDHeaderDtls.Rows[0]["DPH_SWAP_BUYER"])))
                            {
                                SwapBuyer = Convert.ToInt32(dtCIDHeaderDtls.Rows[0]["DPH_SWAP_BUYER"].ToString());
                            }
                            Buyer = dtCIDHeaderDtls.Rows[0]["DPH_ADNL_BUYER"].ToString();
                            if (!string.IsNullOrEmpty(Convert.ToString(dtCIDHeaderDtls.Rows[0]["DPH_PRINT_SHIP_TO"])))
                            {
                                PrintShipTo = Convert.ToBoolean(dtCIDHeaderDtls.Rows[0]["DPH_PRINT_SHIP_TO"]);
                            }
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCIDHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCIDProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCIDOtherDtls);
                            if (dtCIDHeaderDtls != null & dtCIDHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtCIDHeaderDtls.Rows[0]["DPH_COMPANY"].ToString());
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCIDTaxDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCIDTaxFullDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCIDCustomsDtls);
                            var groupdata = (from t in dtCIDTotTaxDtls.AsEnumerable()
                                             group t by new
                                             {
                                                 ISH_TAX = t.Field<int?>("ISH_TAX"),
                                             } into dt
                                             select new
                                             {
                                                 dt.Key.ISH_TAX,
                                                 TAX_CODE = dt.First().Field<string>("TAX_CODE"),
                                                 ISH_NAME = dt.First().Field<string>("ISH_NAME"),
                                                 TAX_DISP_NAME = dt.First().Field<string>("TAX_DISP_NAME"),
                                                 TAX_RATE = dt.First().Field<decimal?>("TAX_RATE"),
                                                 ISH_TAX_AMT = dt.Sum(x => x.Field<decimal>("ISH_TAX_AMT")),
                                                 ISH_TOTAL = dt.Sum(x => x.Field<decimal>("ISH_TOTAL"))
                                             }).ToList();
                            DataTable dtGrpTax = groupdata.ToDataTable();
                            dsCIDTotTaxDtls = new ReportDataSource("TotTaxDtls", dtGrpTax);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCIDTotTaxDtls);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }

                        break;
                    #endregion
                    #region DO
                    case ApplicationType.DO:
                    case ApplicationType.DOD:
                        ReportDataSource dsDOHeaderDtls;
                        ReportDataSource dsDOProductDtls;
                        if (dsDelivaryOrder != null)
                        {
                            DataTable dtDOHeaderDtls = dsDelivaryOrder.Tables[0];
                            DataTable dtDOProductDtls = dsDelivaryOrder.Tables[1];

                            dsDOHeaderDtls = new ReportDataSource("DOHeaderDtls", dtDOHeaderDtls);
                            dsDOProductDtls = new ReportDataSource("DOProductDtls", dtDOProductDtls);

                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsDOHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsDOProductDtls);

                            if (dtDOHeaderDtls != null & dtDOHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtDOHeaderDtls.Rows[0]["ICH_COMPANY"].ToString());
                                hdfCompanyPK.Value = dtDOHeaderDtls.Rows[0]["ICH_COMPANY"].ToString();
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            if (RptSubType == 7 || RptSubType == 8)
                            {
                                DataTable dtProdDtls = dtDOProductDtls;
                                NetWt = dtProdDtls.AsEnumerable().Sum((x => x.Field<double?>("DPD_BOX_IN_CRTN") * x.Field<double?>("LPD_QTY") * x.Field<double?>("LPD_NET_WT")));
                                GrWt = dtProdDtls.AsEnumerable().Sum((x => x.Field<double?>("LPD_QTY") * x.Field<double?>("LPD_CTN_GROSS_WT")));
                            }
                            SetReportParameters(locRpt);
                            if (RptSubType == 3 || RptSubType == 4)
                            {
                                rvCurrentRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);
                            }

                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region STA
                    case ApplicationType.STA:
                        ReportDataSource dsStoreAcceptHeader;
                        ReportDataSource dsStoreAcceptDetail;
                        if (dsMaterialAccept != null)
                        {
                            DataTable dtHeader = dsMaterialAccept.Tables[0];
                            DataTable dtDetail = dsMaterialAccept.Tables[1];
                            SetReportParameters(locRpt);
                            if (dtHeader.Rows.Count > 0 || dtDetail.Rows.Count > 0)
                            {
                                dsStoreAcceptHeader = new ReportDataSource("Header", dtHeader);
                                dsStoreAcceptDetail = new ReportDataSource("Detail", dtDetail);
                                //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsStoreAcceptHeader);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsStoreAcceptDetail);
                                CompanyPK = Convert.ToInt32(dtHeader.Rows[0]["MAH_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                        }
                        break;

                    #endregion
                    #region MI
                    case ApplicationType.MI:
                        ReportDataSource dsStoreIssueHeader;
                        ReportDataSource dsStoreIssueDetail;
                        if (dsMaterialIssue != null)
                        {
                            DataTable dtHeader = dsMaterialIssue.Tables[0];
                            DataTable dtDetail = dsMaterialIssue.Tables[1];
                            SetReportParameters(locRpt);
                            if (dtHeader.Rows.Count > 0 || dtDetail.Rows.Count > 0)
                            {
                                dsStoreIssueHeader = new ReportDataSource("Header", dtHeader);
                                dsStoreIssueDetail = new ReportDataSource("Detail", dtDetail);
                                //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsStoreIssueHeader);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsStoreIssueDetail);
                                CompanyPK = Convert.ToInt32(dtHeader.Rows[0]["MIH_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                        }
                        break;

                    #endregion
                    #region MTI
                    case ApplicationType.MTI:
                        ReportDataSource dsMStoreIssueHeader;
                        ReportDataSource dsMStoreIssueDetail;
                        if (dsMaterialIssue != null)
                        {
                            DataTable dtHeader = dsMaterialIssue.Tables[0];
                            DataTable dtDetail = dsMaterialIssue.Tables[1];
                            SetReportParameters(locRpt);
                            if (dtHeader.Rows.Count > 0 || dtDetail.Rows.Count > 0)
                            {
                                dsMStoreIssueHeader = new ReportDataSource("Header", dtHeader);
                                dsMStoreIssueDetail = new ReportDataSource("Detail", dtDetail);
                                //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsMStoreIssueHeader);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsMStoreIssueDetail);
                                CompanyPK = Convert.ToInt32(dtHeader.Rows[0]["ICH_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                        }
                        break;

                    #endregion


                    #region COMR
                    case ApplicationType.COMR:
                        ReportDataSource dsCompoundUsageHeader;
                        ReportDataSource dsCompoundUsageDetail;
                        if (dsCompoundUsage != null)
                        {
                            //DataTable dtHeader = dsCompoundUsage.Tables[0];
                            DataTable dtDetail = dsCompoundUsage.Tables[0];
                            SetReportParameters(locRpt);
                            if (dtDetail.Rows.Count > 0)
                            {
                                //dsCompoundUsageHeader = new ReportDataSource("Header", dtHeader);
                                dsCompoundUsageDetail = new ReportDataSource("Detail", dtDetail);
                                //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                                //rvCurrentRptViewer.LocalReport.DataSources.Add(dsCompoundUsageHeader);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsCompoundUsageDetail);
                                CompanyPK = Convert.ToInt32(dtDetail.Rows[0]["DPT_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                        }
                        break;

                    #endregion

                    #region CDC
                    case ApplicationType.CDC:
                        //ReportDataSource dsCompoundUsageHeader;
                        //ReportDataSource dsCompoundUsageDetail;
                        if (dsCompoundUsage != null)
                        {
                            //DataTable dtHeader = dsCompoundUsage.Tables[0];
                            DataTable dtDetail = dsCompoundUsage.Tables[0];
                            SetReportParameters(locRpt);
                            if (dtDetail.Rows.Count > 0)
                            {
                                //dsCompoundUsageHeader = new ReportDataSource("Header", dtHeader);
                                dsCompoundUsageDetail = new ReportDataSource("Detail", dtDetail);
                                //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                                //rvCurrentRptViewer.LocalReport.DataSources.Add(dsCompoundUsageHeader);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsCompoundUsageDetail);
                                CompanyPK = Convert.ToInt32(dtDetail.Rows[0]["DPT_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                        }
                        break;

                    #endregion

                    #region COL
                    case ApplicationType.COL:
                        ReportDataSource dsCostCenter;
                        //  ReportDataSource dsCompoundUsageDetail;
                        if (dsCompoundUsage != null)
                        {
                            //DataTable dtHeader = dsCompoundUsage.Tables[0];
                            DataTable dtDetail = dsCompoundUsage.Tables[0];
                            SetReportParameters(locRpt);
                            if (dtDetail.Rows.Count > 0)
                            {
                                //dsCompoundUsageHeader = new ReportDataSource("Header", dtHeader);
                                dsCostCenter = new ReportDataSource("CostCenterDetail", dtDetail);
                                //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                                //rvCurrentRptViewer.LocalReport.DataSources.Add(dsCompoundUsageHeader);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsCostCenter);
                                //  CompanyPK = Convert.ToInt32(dtDetail.Rows[0]["DPT_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(1));
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                        }
                        break;

                    #endregion

                    #region SFG
                    case ApplicationType.SFG:
                    case ApplicationType.SFGCD:
                    case ApplicationType.SFGBL:
                        ReportDataSource dsSFG;
                        //  ReportDataSource dsCompoundUsageDetail;
                        if (DataSet1 != null)
                        {
                            //DataTable dtHeader = dsCompoundUsage.Tables[0];
                            DataTable dtDetail = DataSet1.Tables[0];
                            SetReportParameters(locRpt);
                            if (dtDetail.Rows.Count > 0)
                            {
                                //dsCompoundUsageHeader = new ReportDataSource("Header", dtHeader);
                                dsSFG = new ReportDataSource("SFGProduced", dtDetail);
                                //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                                //rvCurrentRptViewer.LocalReport.DataSources.Add(dsCompoundUsageHeader);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsSFG);
                                //  CompanyPK = Convert.ToInt32(dtDetail.Rows[0]["DPT_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(1));
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                        }
                        break;

                    #endregion
                    #region SFGD
                    case ApplicationType.SFGD:
                    case ApplicationType.SFGCDSP:
                    case ApplicationType.SFGBDSP:
                        ReportDataSource dsSFGD;
                        //  ReportDataSource dsCompoundUsageDetail;
                        if (DataSet1 != null)
                        {

                            DataTable dtDetail = DataSet1.Tables[0];
                            SetReportParameters(locRpt);
                            if (dtDetail.Rows.Count > 0)
                            {

                                dsSFGD = new ReportDataSource("SFGDisp", dtDetail);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsSFGD);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(1));
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                        }
                        break;

                    #endregion


                    #region TB
                    case ApplicationType.TB:
                        ReportDataSource dsTB;
                        currentEntity = new ERPEntities();
                        dsTB = new ReportDataSource("TBDtls", currentEntity.SPFIN_TRIAL_BALANCE_RPT(currentUser.SBUID, Convert.ToDateTime(txtFromDate.Text.Trim()), Convert.ToDateTime(txtToDate.Text.Trim()), null));
                        if (dsTB != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsTB);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(currentUser.SBUID));
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region AS
                    case ApplicationType.AS:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        ReportDataSource dsAS;
                        DataRow dr;
                        divAccPopUp.Visible = false;
                        dr = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        dr["FROM_DATE"] = txtFromDate.Text.Trim();
                        dr["TO_DATE"] = txtToDate.Text.Trim();
                        dr["BIZUNIT"] = currentUser.SBUID;
                        dr["CURRENCY"] = "";
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(dr);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                if (RptSubType == 0 || RptSubType == 3)
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerData(paramXml);
                                    dsAS = new ReportDataSource("ASDtls", dtData1);
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                        }
                                    }
                                    /*List<SPFIN_ACCOUNT_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_ACCOUNT_STATEMENT_RPT(paramXml).ToList();
                                    dsAS = new ReportDataSource("ASDtls", AccList);
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                    }*/
                                }
                                else
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerConsolidatedData(paramXml);
                                    dsAS = new ReportDataSource("ASDtls", dtData1);
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                        }
                                    }
                                    /*List<SPFIN_ACC_STMT_CONS_RPT_Result> AccList = currentEntity.SPFIN_ACC_STMT_CONS_RPT(paramXml).ToList();
                                    dsAS = new ReportDataSource("ASDtls", AccList);
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                    }*/
                                }
                                if (dsAS != null)
                                {
                                    AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                    SetReportParameters(locRpt);
                                    //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsAS);
                                }
                                else
                                {
                                    divReportViewer.Visible = false;
                                    rvCurrentRptViewer.Visible = false;
                                    divNodata.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region GLFIN
                    case ApplicationType.GLFIN:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        //ReportDataSource dsAS;
                        //DataRow dr;
                        divAccPopUp.Visible = false;
                        dr = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        dr["FROM_DATE"] = txtFromDate.Text.Trim();
                        dr["TO_DATE"] = txtToDate.Text.Trim();
                        dr["BIZUNIT"] = currentUser.SBUID;
                        dr["CURRENCY"] = "";
                        dr["FINYEAR"] = ddlFinYear.SelectedValue;
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(dr);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                if (RptSubType == 0 || RptSubType == 3)
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerFinYearData(paramXml);
                                    dsAS = new ReportDataSource("ASDtls", dtData1);
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                        }
                                    }
                                    /*List<SPFIN_ACCOUNT_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_ACCOUNT_STATEMENT_RPT(paramXml).ToList();
                                    dsAS = new ReportDataSource("ASDtls", AccList);
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                    }*/
                                }
                                else
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerConsolidatedData(paramXml);
                                    dsAS = new ReportDataSource("ASDtls", dtData1);
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                        }
                                    }
                                    /*List<SPFIN_ACC_STMT_CONS_RPT_Result> AccList = currentEntity.SPFIN_ACC_STMT_CONS_RPT(paramXml).ToList();
                                    dsAS = new ReportDataSource("ASDtls", AccList);
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                    }*/
                                }
                                if (dsAS != null)
                                {
                                    AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                    SetReportParameters(locRpt);
                                    //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsAS);
                                }
                                else
                                {
                                    divReportViewer.Visible = false;
                                    rvCurrentRptViewer.Visible = false;
                                    divNodata.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region PIJ
                    case ApplicationType.PIJ:
                    case ApplicationType.EIJ:
                    case ApplicationType.PSIJ:
                    case ApplicationType.EITJ:
                    case ApplicationType.TPIJ:
                    case ApplicationType.ESJ:
                        if (!string.IsNullOrWhiteSpace(_printerMode) && _printerMode == PrinterMode.DOTMATRIX.ToString())
                        {
                            #region Dot Matrix Printing
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            DataTable dt = ConfigurationSettings();
                            currentEntity = new ERPEntities();
                            List<SPFIN_INVOICE_VND_VOUCHER_RPT_Result> reportDataSourceList = currentEntity.SPFIN_INVOICE_VND_VOUCHER_RPT(RecPK).ToList();
                            List<SPFIN_TRX_VOUCHER_RPT_Result> additionalDataSourceList = currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK, VoucherVersion).ToList();

                            if (reportDataSourceList != null && reportDataSourceList.Count > 0)
                            {
                                string htmlTemplate = string.Empty;
                                if (appType == ApplicationType.PIJ || appType == ApplicationType.TPIJ)
                                {
                                    htmlTemplate = new DotMatrixReportBuilderFactory(appType)
                                                              .GetReportDataBuilder()
                                                              .GetReportTemplate(new PurchaseVoucherReportParameter
                                                              {
                                                                  dataTable = dt,
                                                                  TrxRefType = TrxRefType,
                                                                  PrintedUser = this.currentUser.EmpName,
                                                                  AppTypeDetailsList = AppTypeDetailsList,
                                                                  PVHeaderDtls = reportDataSourceList,
                                                                  PVAccountDtls = additionalDataSourceList
                                                              });
                                }
                                if (appType == ApplicationType.EIJ || appType == ApplicationType.EITJ)
                                {
                                    htmlTemplate = new DotMatrixReportBuilderFactory(appType)
                                                              .GetReportDataBuilder()
                                                              .GetReportTemplate(new ExpenseVoucherReportParameter
                                                              {
                                                                  dataTable = dt,
                                                                  TrxRefType = TrxRefType,
                                                                  PrintedUser = this.currentUser.EmpName,
                                                                  AppTypeDetailsList = AppTypeDetailsList,
                                                                  EVHeaderDtls = reportDataSourceList,
                                                                  EVAccountDtls = additionalDataSourceList
                                                              });
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Title", htmlTemplate, true);
                            }
                            return false;
                            #endregion
                        }
                        else
                        {
                            #region Normal Printing
                            ReportDataSource dsPIJ;
                            currentEntity = new ERPEntities();
                            List<SPFIN_INVOICE_VND_VOUCHER_RPT_Result> lstPIJData = currentEntity.SPFIN_INVOICE_VND_VOUCHER_RPT(RecPK).ToList();
                            dsPIJ = new ReportDataSource("PVHeaderDtls", lstPIJData);
                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPIJ);
                            dsPIJ = new ReportDataSource("PVAccountDtls", currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK, VoucherVersion));
                            if (lstPIJData != null & lstPIJData.Count > 0)
                            {
                                CompanyPK = lstPIJData[0].FTH_COMPANY;
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPIJ);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            if (dsPIJ != null)
                            {
                                AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                SetReportParameters(locRpt);
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region VPJ,PPCCJ,PCBJ,SIPJ,EIPJ,       // DotMatrix Printing Enabled
                    case ApplicationType.VPJ:
                    case ApplicationType.PPCCJ:
                    case ApplicationType.PCBJ:
                    case ApplicationType.SIPJ:
                    case ApplicationType.EIPJ:
                    case ApplicationType.EIPTJ:
                    case ApplicationType.VPTJ:
                    case ApplicationType.PPCCTJ:
                        if (!string.IsNullOrWhiteSpace(_printerMode) && _printerMode == PrinterMode.DOTMATRIX.ToString())
                        {
                            #region Dot Matrix Printing
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            DataTable dt = ConfigurationSettings();
                            currentEntity = new ERPEntities();
                            List<SPFIN_PAYMENT_VND_VOUCHER_RPT_Result> reportDataSourceList = currentEntity.SPFIN_PAYMENT_VND_VOUCHER_RPT(RecPK, appType).ToList();
                            List<SPFIN_TRX_VOUCHER_RPT_Result> additionalDataSourceList = currentEntity.SPFIN_TRX_VOUCHER_RPT(appType, RecPK, VoucherVersion).ToList();
                            if (reportDataSourceList != null && reportDataSourceList.Count > 0)
                            {
                                string htmlTemplate = new DotMatrixReportBuilderFactory(appType)
                                                                       .GetReportDataBuilder()
                                                                       .GetReportTemplate(new PaymentVoucherReportParameter
                                                                       {
                                                                           dataTable = dt,
                                                                           TrxRefType = TrxRefType,
                                                                           PrintedUser = this.currentUser.EmpName,
                                                                           AppTypeDetailsList = AppTypeDetailsList,
                                                                           reportDataSourceList = reportDataSourceList,
                                                                           additionalReportDataSourceList = additionalDataSourceList
                                                                       });

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Title", htmlTemplate, true);
                            }
                            return false;
                            #endregion
                        }
                        else
                        {
                            #region Normal Printing
                            ReportDataSource dsPV;
                            currentEntity = new ERPEntities();
                            List<SPFIN_PAYMENT_VND_VOUCHER_RPT_Result> lstVPJData = currentEntity.SPFIN_PAYMENT_VND_VOUCHER_RPT(RecPK, appType).ToList();
                            dsPV = new ReportDataSource("PVHeader", lstVPJData);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPV);
                            lstVoucher = currentEntity.SPFIN_TRX_VOUCHER_RPT(appType, RecPK, VoucherVersion).ToList();
                            dsPV = new ReportDataSource("PVRecordings", lstVoucher);

                            if (lstVPJData != null & lstVPJData.Count > 0)
                            {
                                CompanyPK = lstVPJData[0].PVH_COMPANY;
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPV);
                            List<SPFIN_PAYMENT_VND_MODE_GET_Result> lstMode = currentEntity.SPFIN_PAYMENT_VND_MODE_GET(RecPK).ToList();
                            dsPV = new ReportDataSource("PVMode", lstMode);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPV);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            if (dsPV != null)
                            {
                                AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                SetReportParameters(locRpt);
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region CRJ,PDCCJ,PDCCTJ,RCBJ,RCBTJ,MSIRJ,MSIRTJ,FCHRJ      // DotMatrix Printing Enabled
                    case ApplicationType.CRJ:
                    case ApplicationType.CRTJ:
                    case ApplicationType.PDCCJ:
                    case ApplicationType.PDCCTJ:
                    case ApplicationType.RCBJ:
                    case ApplicationType.RCBTJ:
                    case ApplicationType.MSIRJ:
                    case ApplicationType.MSIRTJ:
                    case ApplicationType.FCHRJ:
                    case ApplicationType.FCHRJYE:
                        if (!string.IsNullOrWhiteSpace(_printerMode) && _printerMode == PrinterMode.DOTMATRIX.ToString())
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            DataTable dt = ConfigurationSettings();
                            currentEntity = new ERPEntities();

                            TrxRefType = appType;
                            List<SPFIN_RECEIPT_VOUCHER_RPT_Result> reportDataSourceList = currentEntity.SPFIN_RECEIPT_VOUCHER_RPT(RecPK, appType).ToList();
                            List<SPFIN_TRX_VOUCHER_RPT_Result> additionalDataSourceList = currentEntity.SPFIN_TRX_VOUCHER_RPT(appType, RecPK, VoucherVersion).ToList();
                            string script = string.Empty;
                            IReportParameter parameter = default(IReportParameter);

                            #region Build Parameter
                            if (appType == ApplicationType.CRJ || appType == ApplicationType.CRTJ)
                            {
                                parameter = new ReceiptVoucherReportParameter
                                {
                                    dataTable = dt,
                                    TrxRefType = TrxRefType,
                                    PrintedUser = currentUser.EmpName,
                                    AppTypeDetailsList = AppTypeDetailsList,
                                    HeaderDtls = reportDataSourceList,
                                    AccountDtls = additionalDataSourceList
                                };
                            }
                            else if (appType == ApplicationType.PDCCJ || appType == ApplicationType.PDCCTJ)
                            {
                                parameter = new PDCVoucherReportParameter
                                {
                                    dataTable = dt,
                                    TrxRefType = TrxRefType,
                                    PrintedUser = currentUser.EmpName,
                                    AppTypeDetailsList = AppTypeDetailsList,
                                    HeaderDtls = reportDataSourceList,
                                    AccountDtls = additionalDataSourceList
                                };
                            }
                            else if (appType == ApplicationType.RCBJ || appType == ApplicationType.RCBTJ)
                            {
                                parameter = new ChequeReturnVoucherReceiptReportParameter
                                {
                                    dataTable = dt,
                                    TrxRefType = TrxRefType,
                                    PrintedUser = currentUser.EmpName,
                                    AppTypeDetailsList = AppTypeDetailsList,
                                    HeaderDtls = reportDataSourceList,
                                    AccountDtls = additionalDataSourceList
                                };
                            }
                            else if (appType == ApplicationType.FCHRJ)
                            {
                                parameter = new FcReverseReportParameter
                                {
                                    dataTable = dt,
                                    TrxRefType = TrxRefType,
                                    PrintedUser = currentUser.EmpName,
                                    AppTypeDetailsList = AppTypeDetailsList,
                                    HeaderDtls = reportDataSourceList,
                                    AccountDtls = additionalDataSourceList
                                };
                            }
                            #endregion

                            script = new DotMatrixReportBuilderFactory(appType)
                                               .GetReportDataBuilder()
                                               .GetReportTemplate(parameter);

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Title", script, true);
                            return false;
                        }
                        else
                        {
                            #region Normal Printing
                            ReportDataSource dsCRJ;
                            currentEntity = new ERPEntities();
                            List<SPFIN_RECEIPT_VOUCHER_RPT_Result> lstCRJData = currentEntity.SPFIN_RECEIPT_VOUCHER_RPT(RecPK, appType).ToList();
                            dsCRJ = new ReportDataSource("RVHeader", lstCRJData);
                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCRJ);
                            if (RptSubType != Convert.ToInt32(ApplicationSubType.OFFICIALRECEIPT))
                            {
                                dsCRJ = new ReportDataSource("RVAccountDtls", currentEntity.SPFIN_TRX_VOUCHER_RPT(appType, RecPK, VoucherVersion));
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsCRJ);
                            }
                            if (lstCRJData != null & lstCRJData.Count > 0)
                            {
                                if (appType == ApplicationType.CRJ || appType == ApplicationType.CRTJ)
                                {
                                    CompanyPK = Convert.ToInt32(lstCRJData[0].RCH_COMPANY);
                                }
                                else
                                {
                                    CompanyPK = Convert.ToInt32(lstCRJData[0].FTH_COMPANY);
                                }
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            if (dsCRJ != null)
                            {
                                AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                SetReportParameters(locRpt);
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region SIJ,MSIJ
                    case ApplicationType.SIJ:
                    case ApplicationType.MSIJ:
                    case ApplicationType.DSIJ:
                    case ApplicationType.MSITJ:
                        if (!string.IsNullOrWhiteSpace(_printerMode)
                                && _printerMode == PrinterMode.DOTMATRIX.ToString()
                                && RptSubType != Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE))
                        {
                            #region Dot Matrix Printing
                            currentEntity = new ERPEntities();
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            DataTable dt = ConfigurationSettings();
                            List<SPFIN_SALES_VOUCHER_RPT_Result> reportDataSourceList = currentEntity.SPFIN_SALES_VOUCHER_RPT(RecPK).ToList();
                            if (reportDataSourceList != null && reportDataSourceList.Count > 0)
                            {
                                string htmlTemplate = new DotMatrixReportBuilderFactory(appType)
                                                                       .GetReportDataBuilder()
                                                                       .GetReportTemplate(new SalesVoucherReportParameter
                                                                       {
                                                                           dataTable = dt,
                                                                           TrxRefType = TrxRefType,
                                                                           PrintedUser = this.currentUser.EmpName,
                                                                           AppTypeDetailsList = AppTypeDetailsList,
                                                                           SvHeaderDataSourceList = reportDataSourceList,
                                                                           SVAccountDtlsDataSourceList = currentEntity.SPFIN_TRX_VOUCHER_RPT(appType, RecPK, VoucherVersion).ToList()
                                                                       });

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Title", htmlTemplate, true);
                            }
                            return false;
                            #endregion
                        }
                        else
                        {
                            #region Normal Printing
                            ReportDataSource dsSIJ;
                            currentEntity = new ERPEntities();
                            if (RptSubType == Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE))
                            {
                                List<SPFIN_SALES_ADV_INV_RPT_Result> lstData = currentEntity.SPFIN_SALES_ADV_INV_RPT(RecPK).ToList();
                                dsSIJ = new ReportDataSource("SVHeader", lstData);
                                //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIJ);
                                if (lstData != null && lstData.Count > 0)
                                {
                                    CompanyPK = lstData[0].ICH_COMPANY;
                                }
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                            else
                            {
                                List<SPFIN_SALES_VOUCHER_RPT_Result> lstSIJData = currentEntity.SPFIN_SALES_VOUCHER_RPT(RecPK).ToList();
                                dsSIJ = new ReportDataSource("SVHeader", lstSIJData);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIJ);
                                dsSIJ = new ReportDataSource("SVAccountDtls", currentEntity.SPFIN_TRX_VOUCHER_RPT(appType, RecPK, VoucherVersion));
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIJ);
                                if (lstSIJData != null & lstSIJData.Count > 0)
                                {
                                    CompanyPK = lstSIJData[0].FTH_COMPANY;
                                }
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                            if (dsSIJ != null)
                            {
                                AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                SetReportParameters(locRpt);
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region SAS
                    case ApplicationType.SAS:
                        ReportDataSource dsSAS;
                        DataRow drSAS;
                        divAccPopUp.Visible = false;
                        drSAS = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        drSAS["FROM_DATE"] = txtFromDate.Text.Trim();
                        drSAS["TO_DATE"] = txtToDate.Text.Trim();
                        drSAS["BIZUNIT"] = currentUser.SBUID;
                        drSAS["CURRENCY"] = "";
                        drSAS["CST_PK"] = ddlSubLedger.SelectedValue;
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(drSAS);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetSubLedgerData(paramXml);
                                dsSAS = new ReportDataSource("ASDtls", dtData1);
                                if (dsSAS != null)
                                {
                                    AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                    SetReportParameters(locRpt);
                                    //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsSAS);
                                }
                                else
                                {
                                    divReportViewer.Visible = false;
                                    rvCurrentRptViewer.Visible = false;
                                    divNodata.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region PSAS
                    case ApplicationType.PSAS:
                        ReportDataSource dsPSAS;
                        DataRow drPSAS;
                        divAccPopUp.Visible = false;
                        drPSAS = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        drPSAS["FROM_DATE"] = txtFromDate.Text.Trim();
                        drPSAS["TO_DATE"] = txtToDate.Text.Trim();
                        drPSAS["BIZUNIT"] = currentUser.SBUID;
                        drPSAS["CURRENCY"] = "";
                        drPSAS["PARTY_TYPE"] = hdfSendTo.Value;
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(drPSAS);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetPartyLedgerData(paramXml);
                                dsPSAS = new ReportDataSource("ASDtls", dtData1);
                                if (dsPSAS != null && dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                    SetReportParameters(locRpt);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsPSAS);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails());
                                }
                                else
                                {
                                    divReportViewer.Visible = false;
                                    rvCurrentRptViewer.Visible = false;
                                    divNodata.Visible = true;
                                }
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region BRC
                    case ApplicationType.BRC:
                        ReportDataSource dsBRC;
                        DataRow drBRC;
                        divAccPopUp.Visible = false;
                        drBRC = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        if (usrDateFilter.Visible)
                        {
                            drBRC["FROM_DATE"] = usrDateFilter.FromDate;
                            drBRC["TO_DATE"] = usrDateFilter.ToDate;
                        }
                        else
                        {
                            drBRC["FROM_DATE"] = txtFromDate.Text.Trim();
                            drBRC["TO_DATE"] = txtToDate.Text.Trim();
                        }
                        //drBRC["FROM_DATE"] = txtFromDate.Text.Trim();
                        //drBRC["TO_DATE"] = txtToDate.Text.Trim();
                        drBRC["BIZUNIT"] = currentUser.SBUID;
                        drBRC["CURRENCY"] = "";
                        drBRC["CST_PK"] = ddlSubLedger.SelectedValue;
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(drBRC);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                dsBRC = new ReportDataSource("BRCDtls", currentEntity.SPFIN_BANK_RECONCILIATION_RPT(paramXml));
                                if (dsBRC != null)
                                {
                                    AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                    SetReportParameters(locRpt);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsBRC);
                                }
                                else
                                {
                                    divReportViewer.Visible = false;
                                    rvCurrentRptViewer.Visible = false;
                                    divNodata.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region CrDr
                    case ApplicationType.CN:
                    case ApplicationType.DN:
                    case ApplicationType.CNT:
                    case ApplicationType.DNT:
                        ReportDataSource dsHeaderDtls;
                        ReportDataSource dsMaterialDtls;
                        //ReportDataSource dsTotTaxDtls;
                        if (dsCNDN != null)
                        {

                            DataTable dtHeaderDtls = dsCNDN.Tables[0];
                            dtPRDtls = dsCNDN.Tables[1];
                            DataTable dtTotTaxDtls = dsCNDN.Tables[2];
                            dsHeaderDtls = new ReportDataSource("CrDrHeader", dtHeaderDtls);
                            dsMaterialDtls = new ReportDataSource("CrDrDetails", dtPRDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsMaterialDtls);
                            if (dtHeaderDtls != null & dtHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtHeaderDtls.Rows[0]["CDH_COMPANY"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                            var groupdata = (from t in dtTotTaxDtls.AsEnumerable()
                                             group t by new
                                             {
                                                 NTH_TAX = t.Field<int?>("NTH_TAX"),
                                             } into dt
                                             select new
                                             {
                                                 dt.Key.NTH_TAX,
                                                 TAX_CODE = dt.First().Field<string>("TAX_CODE"),
                                                 NTH_NAME = dt.First().Field<string>("NTH_NAME"),
                                                 TAX_DISP_NAME = dt.First().Field<string>("TAX_DISP_NAME"),
                                                 TAX_RATE = dt.First().Field<decimal?>("TAX_RATE"),
                                                 NTH_TAX_AMT = dt.Sum(x => x.Field<decimal>("NTH_TAX_AMT")),
                                                 NTH_TOTAL = dt.Sum(x => x.Field<decimal>("NTH_TOTAL"))
                                             }).ToList();
                            DataTable dtGrpTax = groupdata.ToDataTable();
                            dsTotTaxDtls = new ReportDataSource("TotTaxDtls", dtGrpTax);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsTotTaxDtls);
                            SetReportParameters(locRpt);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region LST_VCH
                    case ApplicationType.LST_VCH:
                        ReportDataSource dsLST_VCH;
                        currentEntity = new ERPEntities();
                        dsLST_VCH = new ReportDataSource("PVCLHeader", currentEntity.SPFIN_VOUCHER_LIST_SUMMARY_RPT("<Root></Root>"));
                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsLST_VCH);
                        dsLST_VCH = new ReportDataSource("PVCLAccountDtls", currentEntity.SPFIN_VOUCHER_LIST_DETAILS_RPT("<Root></Root>"));
                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsLST_VCH);

                        if (dsLST_VCH != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region CNTINSP
                    case ApplicationType.CNTINSP:
                        ReportDataSource dsCI;
                        currentEntity = new ERPEntities();
                        dsCI = new ReportDataSource("CIProductDtls", currentEntity.SPFIN_CONTAINER_INSP_RPT(RecPK));
                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsCI);
                        if (dsCI != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, DateTime.Now.Date);
                            SetReportParameters(locRpt);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region PCS
                    case ApplicationType.PCS:
                        #region Normal Printing
                        ReportDataSource dsPCS;
                        currentEntity = new ERPEntities();
                        List<SPFIN_TRX_VOUCHER_RPT_Result> lstPCSData = currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK, VoucherVersion).ToList();
                        dsPCS = new ReportDataSource("JVHeader", lstPCSData);
                        if (lstPCSData != null && lstPCSData.Count > 0)
                        {
                            CompanyPK = lstPCSData[0].FTH_COMPANY;
                        }
                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsPCS);
                        rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                        if (dsPCS != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        #endregion
                        break;
                    #endregion
                    #region EMI
                    case ApplicationType.EMI:
                        ReportDataSource dsEMIHeaderDtls;
                        ReportDataSource dsEMIProductDtls;
                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtEMIHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtEMIProductDtls = dsPurchaseRequest.Tables[1];
                            dsEMIHeaderDtls = new ReportDataSource("dtEMIHeaderDtls", dtEMIHeaderDtls);
                            dsEMIProductDtls = new ReportDataSource("dtEMIProductDtls", dtEMIProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsEMIHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsEMIProductDtls);
                            if (dtEMIHeaderDtls != null & dtEMIHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtEMIHeaderDtls.Rows[0]["ICH_COMPANY"].ToString());
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region DNJ || CNJ                      // DotMatrix Printing Enabled
                    case ApplicationType.CNJ:
                    case ApplicationType.DNJ:
                    case ApplicationType.CNTJ:
                    case ApplicationType.DNTJ:
                        if (!string.IsNullOrWhiteSpace(_printerMode) && _printerMode == PrinterMode.DOTMATRIX.ToString())
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            DataTable dt = ConfigurationSettings();
                            currentEntity = new ERPEntities();

                            TrxRefType = appType;
                            List<SPFIN_TRX_VOUCHER_RPT_Result> reportDataSourceList = currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK, VoucherVersion).ToList();

                            if (reportDataSourceList != null && reportDataSourceList.Count > 0)
                            {
                                string htmlTemplate = new DotMatrixReportBuilderFactory(appType)
                                                                       .GetReportDataBuilder()
                                                                       .GetReportTemplate(new CommonReportParameter
                                                                       {
                                                                           AppTypeDetailsList = AppTypeDetailsList,
                                                                           PrintedUser = this.currentUser.EmpName,
                                                                           dataTable = dt,
                                                                           reportDataSourceList = reportDataSourceList,
                                                                           TrxRefType = TrxRefType
                                                                       });

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Title", htmlTemplate, true);
                            }
                            return false;
                        }
                        else
                        {
                            #region Normal Printing
                            ReportDataSource dsDNJ;
                            currentEntity = new ERPEntities();
                            TrxRefType = appType;
                            List<SPFIN_TRX_VOUCHER_RPT_Result> lstDNJData = currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK, VoucherVersion).ToList();
                            dsDNJ = new ReportDataSource("JVHeader", lstDNJData);
                            if (lstDNJData != null & lstDNJData.Count > 0)
                            {
                                CompanyPK = lstDNJData[0].FTH_COMPANY;
                            }
                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsDNJ);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            if (dsDNJ != null)
                            {
                                AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                SetReportParameters(locRpt);
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region VP
                    case ApplicationType.VP:
                        ReportDataSource dsVP;
                        DataTable dtData = new DataTable();
                        //DataTable dtPNDData = new DataTable();
                        currentEntity = new ERPEntities();
                        if (RptSubType == 2)
                        {
                            List<SPFIN_WHT_CERTIFICATE_RPT_Result> reportData = currentEntity.SPFIN_WHT_CERTIFICATE_RPT(RecPK).ToList();
                            if (reportData.Any())
                            {
                                dtData = reportData.ToDataTable();
                            }

                            //dtPNDData = dtData
                            //            .AsEnumerable()
                            //            .Where(x => x.Field<int>("WTH_FORM_NO") != 390)
                            //            .ToArray<DataRow>().CopyToDataTable();

                            dsVP = new ReportDataSource("ReportDtls", dtData);

                            for (int i = 0; i < dtData.Rows.Count; i++)
                            {
                                if (Convert.ToInt32(dtData.Rows[i]["WTH_FORM_NO"]) == 390)
                                {
                                    PNDCount++;
                                }
                            }
                            rvCurrentRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);
                        }
                        //else if (RptSubType == 3)
                        //{
                        //    dsVP = new ReportDataSource("ReportDtls", currentEntity.SPFIN_PND54_RPT(RecPK));

                        //}
                        else
                        {
                            int? chqID = null;
                            if (ChequeID != null)
                                chqID = Convert.ToInt32(ChequeID);
                            List<SPFIN_PAYMENT_CHEQUE_PRINT_RPT_Result> lstCheque = currentEntity.SPFIN_PAYMENT_CHEQUE_PRINT_RPT(RecPK, chqID).ToList();
                            dsVP = new ReportDataSource("VPHeaderDtls", lstCheque);
                            if (lstCheque != null && lstCheque.Count > 0)
                            {
                                ChequeReport = lstCheque[0].CBM_CHEQUE_RDLC;
                            }
                        }
                        if (dsVP != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsVP);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region DPVJ || PCVJ                    // DotMatrix Printing Enabled
                    case ApplicationType.DPVJ:
                    case ApplicationType.PCVJ:
                    case ApplicationType.CTVJ:
                    case ApplicationType.PCRVJ:
                        #region DotMatrix
                        if (!string.IsNullOrWhiteSpace(_printerMode) && _printerMode == PrinterMode.DOTMATRIX.ToString())
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            DataTable dt = ConfigurationSettings();
                            currentEntity = new ERPEntities();

                            dynamic reportDataSourceList = null;

                            if (RptSubType == 2)
                            {
                                reportDataSourceList = currentEntity.SPFIN_DP_WHT_CERT_RPT(RecPK);
                            }
                            else if (RptSubType == 0)
                            {
                                reportDataSourceList = currentEntity.SPFIN_TRX_VOUCHER_RPT(RptType, RecPK, VoucherVersion).ToList();
                            }


                            if (reportDataSourceList != null && reportDataSourceList.Count > 0)
                            {
                                string htmlTemplate = new DotMatrixReportBuilderFactory(appType)
                                                          .GetReportDataBuilder()
                                                          .GetReportTemplate(new CommonReportParameter
                                                          {
                                                              AppTypeDetailsList = AppTypeDetailsList,
                                                              dataTable = dt,
                                                              TrxRefType = TrxRefType,
                                                              reportDataSourceList = reportDataSourceList,
                                                              PrintedUser = currentUser.EmpName
                                                          });

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Title", htmlTemplate, true);
                            }
                            else
                            {
                                throw new InvalidOperationException("No Data Found");
                            }
                            return false;
                        }
                        #endregion
                        else
                        {
                            #region Normal Printing
                            ReportDataSource dsDPVJ = null;
                            ReportDataSource dsPV = null;
                            DataTable dtPageData = new DataTable();
                            currentEntity = new ERPEntities();
                            if (RptSubType == 2)
                            {
                                List<SPFIN_DP_WHT_CERT_RPT_Result> reportData = currentEntity.SPFIN_DP_WHT_CERT_RPT(RecPK).ToList();
                                if (reportData.Any())
                                {
                                    dtPageData = reportData.ToDataTable();
                                }
                                dsDPVJ = new ReportDataSource("ReportDtls", dtPageData);

                                for (int i = 0; i < dtPageData.Rows.Count; i++)
                                {
                                    if (Convert.ToInt32(dtPageData.Rows[i]["WTH_FORM_NO"]) == 390)
                                    {
                                        DPVJPNDcount++;
                                    }
                                }
                                rvCurrentRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);
                            }
                            else if (RptSubType == 0)
                            {
                                lstVoucher = currentEntity.SPFIN_TRX_VOUCHER_RPT(RptType, RecPK, VoucherVersion).ToList();
                                dsDPVJ = new ReportDataSource("PVRecordings", lstVoucher);
                                if (lstVoucher != null & lstVoucher.Count > 0)
                                {
                                    CompanyPK = lstVoucher[0].FTH_COMPANY;
                                }
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsDPVJ);
                                List<SPFIN_PAYMENT_VND_MODE_GET_Result> lstMode = currentEntity.SPFIN_PAYMENT_VND_MODE_GET(RecPK).ToList();
                                dsPV = new ReportDataSource("PVMode", lstMode);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsPV);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                            else if (RptSubType == 4)
                            {
                                List<SPFIN_TRX_VOUCHER_RPT_Result> lstCheque = new List<SPFIN_TRX_VOUCHER_RPT_Result>();
                                lstVoucher = currentEntity.SPFIN_TRX_VOUCHER_RPT(RptType, RecPK, VoucherVersion).ToList();
                                if (ChequeID != null)
                                {
                                    lstCheque = lstVoucher.Where(x => x.FTR_PK.Value.ToString() == ChequeID).ToList();
                                    var newLst = lstCheque.Select(i => new { CUR_CODE = i.FTH_TRX_CURR_TEXT, CHEQUE_PAY = i.FTR_INSTR_FAVOUR, CHEQUE_AMOUNT = i.FTR_CR_AMT_TC + i.FTR_DR_AMT_TC, CUR_SYMBOL = i.FTH_TRX_CURR_SYMBOL, CHEQUE_DATE = i.FTR_INSTR_DATE, CUR_FRACTION = i.FTH_TRX_CURR_FRACTION, CBM_CHEQUE_RDLC = i.CBM_CHEQUE_RDLC }).ToList();
                                    dsDPVJ = new ReportDataSource("VPHeaderDtls", newLst);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsDPVJ);
                                    if (newLst != null)
                                    {
                                        ChequeReport = newLst[0].CBM_CHEQUE_RDLC;
                                    }
                                }
                            }
                            if (dsDPVJ != null)
                            {
                                AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                SetReportParameters(locRpt);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsDPVJ);
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvCurrentRptViewer.Visible = false;
                                divNodata.Visible = true;
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region SPLN
                    case ApplicationType.SPLN:
                        switch (RptSubType)
                        {
                            case 4:
                                ReportDataSource dsShippingHeaderDtls;
                                ReportDataSource dsItemDtls;
                                if (dsShippingPlan != null)
                                {
                                    SetReportParameters(locRpt);
                                    DataTable dtShippingHeaderDtls = dsShippingPlan.Tables[0];
                                    DataTable dtItemDtls = dsShippingPlan.Tables[1];
                                    dsShippingHeaderDtls = new ReportDataSource("SHGHeaderDtls", dtShippingHeaderDtls);
                                    dsItemDtls = new ReportDataSource("ItemDtls", dtItemDtls);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsShippingHeaderDtls);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsItemDtls);
                                    if (dtShippingHeaderDtls != null & dtShippingHeaderDtls.Rows.Count > 0)
                                    {
                                        CompanyPK = Convert.ToInt32(dtShippingHeaderDtls.Rows[0]["SNH_COMPANY"].ToString());
                                    }
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                                }
                                else
                                {
                                    divReportViewer.Visible = false;
                                    rvCurrentRptViewer.Visible = false;
                                    divNodata.Visible = true;
                                }
                                break;
                            case 3:
                                ReportDataSource dsShippingPlanReceipt;
                                if (dsShippingPlan != null && dsShippingPlan.Tables[0].Rows.Count > 0)
                                {
                                    SetReportParameters(locRpt);
                                    DataTable dtShippingPlanReceipt = dsShippingPlan.Tables[0];
                                    dsShippingPlanReceipt = new ReportDataSource("ShippingPlanReceiptDtls", dtShippingPlanReceipt);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsShippingPlanReceipt);
                                }
                                else
                                {
                                    divReportViewer.Visible = false;
                                    rvCurrentRptViewer.Visible = false;
                                    divNodata.Visible = true;
                                }
                                break;
                            default:
                                ReportDataSource dsSPLN;
                                ReportDataSource dsSPLN1 = null;
                                ReportDataSource dsSPLN2 = null;
                                currentEntity = new ERPEntities();
                                ShippingPlanPK = Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] != null ? (int)Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] : 0;
                                if (RptSubType == 1)
                                {
                                    dsSPLN = new ReportDataSource("LPHeader", dsLoadingPlan.Tables[0].AsDataView());
                                    DataTable dtLDGPlanDtls = dsLoadingPlan.Tables[0];
                                    if (dtLDGPlanDtls != null & dtLDGPlanDtls.Rows.Count > 0)
                                    {
                                        CompanyPK = Convert.ToInt32(dtLDGPlanDtls.Rows[0]["LPH_COMPANY"].ToString());
                                    }
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                                }
                                else
                                {
                                    dsLoadingPlan = BusinessLogic.Shipping.ShippingUploadsBL.GetExportList(RecPK);
                                    dsSPLN = new ReportDataSource("HEADER", dsLoadingPlan.Tables[0].AsDataView());
                                    dsSPLN1 = new ReportDataSource("BRANDS", dsLoadingPlan.Tables[1].AsDataView());
                                    dsSPLN2 = new ReportDataSource("EVALUATIONCHECKLIST", dsLoadingPlan.Tables[2].AsDataView());
                                }
                                if (dsSPLN != null)
                                {
                                    AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                    SetReportParameters(locRpt);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsSPLN);
                                    if (dsSPLN1 != null)
                                    {
                                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsSPLN1);
                                    }
                                    if (dsSPLN2 != null)
                                    {
                                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsSPLN2);
                                    }
                                }
                                else
                                {
                                    divReportViewer.Visible = false;
                                    rvCurrentRptViewer.Visible = false;
                                    divNodata.Visible = true;
                                }
                                break;
                        }
                        break;
                    #endregion
                    #region VSE
                    case ApplicationType.VSE://Output in Excel (search 'ShowPDF')
                        ReportDataSource dsVatSale;
                        if (dsVatSaleExport != null && dsVatSaleExport.Tables[0].Rows.Count > 0)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtVatSaleDtls = dsVatSaleExport.Tables[0];
                            dsVatSale = new ReportDataSource("VatSaleDtls", dtVatSaleDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsVatSale);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region PI,EI
                    case ApplicationType.PI:
                    case ApplicationType.EI:
                    case ApplicationType.ES:
                    case ApplicationType.EIT:
                    case ApplicationType.TPI:
                        ReportDataSource dsPIHeaderDtls;
                        ReportDataSource dsPIProductDtls;
                        ReportDataSource dsPIOtherDtls;
                        ReportDataSource dsPITaxDtls;
                        ReportDataSource dsPITotTaxDtls;
                        if (dsDelivaryOrder != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtPIHeaderDtls = dsDelivaryOrder.Tables[0];
                            DataTable dtPIProductDtls = dsDelivaryOrder.Tables[1];
                            DataTable dtPITotTaxDtls = dsDelivaryOrder.Tables[4];
                            if (dtPIHeaderDtls != null && dtPIHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtPIHeaderDtls.Rows[0][GetLocalResourceObject("CompanyPK").ToString()]);
                            }

                            dsPIHeaderDtls = new ReportDataSource("DOHeaderDtls", dtPIHeaderDtls);
                            dsPIProductDtls = new ReportDataSource("DOProductDtls", dtPIProductDtls);
                            dsPIOtherDtls = new ReportDataSource("DOOtherDtls", dsDelivaryOrder.Tables[2]);
                            dsPITaxDtls = new ReportDataSource("TaxDtls", dsDelivaryOrder.Tables[3]);

                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPIHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPIProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPIOtherDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPITaxDtls);
                            if (appType == ApplicationType.PI && GetGlobalResourceObject("ConfigurationsRes", "PI_GST_Format").ToString() == "1" || appType == ApplicationType.TPI && GetGlobalResourceObject("ConfigurationsRes", "PI_GST_Format").ToString() == "1")
                            {
                                dsPITotTaxDtls = new ReportDataSource("TotTaxDtls", dtPITotTaxDtls);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsPITotTaxDtls);
                            }
                            else
                            {
                                var grpData = (from t in dtPITotTaxDtls.AsEnumerable()
                                               group t by new
                                               {
                                                   VTH_TAX = t.Field<int?>("VTH_TAX")
                                               } into dt
                                               select new
                                               {
                                                   dt.Key.VTH_TAX,
                                                   TAX_CODE = dt.First().Field<string>("TAX_CODE"),
                                                   VTH_NAME = dt.First().Field<string>("VTH_NAME"),
                                                   TAX_DISP_NAME = dt.First().Field<string>("TAX_DISP_NAME"),
                                                   TAX_RATE = dt.First().Field<decimal?>("TAX_RATE"),
                                                   VTH_TAX_AMT = dt.Sum(x => x.Field<decimal?>("VTH_TAX_AMT")),
                                                   VTH_TOTAL = dt.Sum(x => x.Field<decimal?>("VTH_TOTAL"))
                                               }).ToList();
                                DataTable dtGrpTax = grpData.ToDataTable();
                                dsPITotTaxDtls = new ReportDataSource("TotTaxDtls", dtGrpTax);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsPITotTaxDtls);
                            }
                            //  dsPITotTaxDtls = new ReportDataSource("TotTaxDtls", dtPITotTaxDtls);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }

                        break;
                    #endregion
                    #region GST
                    case ApplicationType.GST:
                        ReportDataSource dsGSTHeaderDtls;
                        ReportDataSource dsGSTTaxDetails;
                        ReportDataSource dsBreakDownDtls;
                        if (dsGST != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtHeaderDtls = dsGST.Tables[0];
                            DataTable dtTaxDtls = dsGST.Tables[1];
                            DataTable dtBreakDownDtls = dsGST.Tables[2];
                            if (dtHeaderDtls != null && dtHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtHeaderDtls.Rows[0]["TGH_COMPANY"]);
                            }
                            dsGSTHeaderDtls = new ReportDataSource("GSTHEADER", dtHeaderDtls);
                            dsGSTTaxDetails = new ReportDataSource("GSTTaxDetails", dtTaxDtls);
                            dsBreakDownDtls = new ReportDataSource("BreakDownDetails", dtBreakDownDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGSTHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGSTTaxDetails);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsBreakDownDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            rvCurrentRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region VTYPE
                    case ApplicationType.VTYPE:
                    case ApplicationType.VTYPESIJ:
                    case ApplicationType.VTYPEPSIJ:
                    case ApplicationType.VTYPEVPJ:
                    case ApplicationType.VTYPECRJ:
                    case ApplicationType.VTYPEMSIRJ:
                    case ApplicationType.VTYPEPCVJ:
                    case ApplicationType.VTYPEDPVJ:
                    case ApplicationType.VTYPEDRVJ:
                    case ApplicationType.VTYPEDNJPI:
                    case ApplicationType.VTYPECNJPI:
                    case ApplicationType.VTYPEDNJSI:
                    case ApplicationType.VTYPECNJSI:
                    case ApplicationType.VTYPEMIJ:
                    case ApplicationType.VTYPEEIJ:
                    case ApplicationType.VTYPEMSIJ:
                    case ApplicationType.VTYPEDPRJ:
                    case ApplicationType.VTYPECWIPJ:
                        if (dsVoucher != null && dsVoucher.Tables.Count > 0)
                        {
                            SetReportParameters(locRpt);
                            //if (appType == ApplicationType.VTYPEPCVJ)
                            //{
                            //    ReportParameter parameters = new ReportParameter("VatBuy","0");
                            //    locRpt.SetParameters(parameters);
                            //    parameters = new ReportParameter("beforeVat", "0");
                            //    locRpt.SetParameters(parameters);
                            //    parameters = new ReportParameter("WithHolding", "0");
                            //    locRpt.SetParameters(parameters);
                            //    parameters = new ReportParameter("PaidBy", "0");
                            //    locRpt.SetParameters(parameters);
                            //    parameters = new ReportParameter("NetAmount", "0");
                            //    locRpt.SetParameters(parameters);
                            //}


                            ReportDataSource dsvoucher = new ReportDataSource("DataSet1", dsVoucher.Tables[0]);
                            SubReportIndex = 1;
                            RowIndex = 0;
                            hdfCompanyPK.Value = dsVoucher.Tables[0].Rows[0]["P_FTH_COMPANY"].ToString();
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsvoucher);
                            rvCurrentRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region CMP
                    case ApplicationType.CMP:
                        ReportDataSource dsCmpHr = null;
                        ReportDataSource dsCmpDt = null;
                        ReportDataSource dsCmpDt2 = null;
                        ReportDataSource dsCmpDt3 = null;
                        if (dsCmpPreparation != null)
                        {
                            if (dsCmpPreparation.Tables.Count > 0)
                            {
                                SetReportParameters(locRpt);
                                dsCmpHr = new ReportDataSource("HeaderDt", dsCmpPreparation.Tables[0]);
                                dsCmpDt = new ReportDataSource("DetailDt", dsCmpPreparation.Tables[1]);
                                dsCmpDt2 = new ReportDataSource("Detail2Dt", dsCmpPreparation.Tables[2]);
                                dsCmpDt3 = new ReportDataSource("CheckList", dsCmpPreparation.Tables[3]);
                                if (dsCmpHr != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsCmpHr);
                                if (dsCmpDt != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsCmpDt);
                                if (dsCmpDt2 != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsCmpDt2);
                                if (dsCmpDt3 != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsCmpDt3);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails());
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region DISP
                    case ApplicationType.DISP:
                    case ApplicationType.BOM:
                        ReportDataSource dsDispHr = null;
                        ReportDataSource dsDispDt = null;
                        ReportDataSource dsDispDt2 = null;
                        ReportDataSource dsDispDt3 = null;
                        if (dsDispPreparation != null)
                        {
                            if (dsDispPreparation.Tables.Count > 0)
                            {
                                SetReportParameters(locRpt);
                                dsDispHr = new ReportDataSource("HeaderDt", dsDispPreparation.Tables[0]);
                                dsDispDt = new ReportDataSource("DetailDt", dsDispPreparation.Tables[1]);
                                dsDispDt2 = new ReportDataSource("Detail2Dt", dsDispPreparation.Tables[2]);
                                dsDispDt3 = new ReportDataSource("CheckList", dsDispPreparation.Tables[3]);
                                if (dsDispHr != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsDispHr);
                                if (dsDispDt != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsDispDt);
                                if (dsDispDt2 != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsDispDt2);
                                if (dsDispDt3 != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsDispDt3);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails());
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region ACI
                    case ApplicationType.ACI:
                        ReportDataSource dsAgentCommisionRpt1;
                        ReportDataSource dsAgentCommisionRpt2;
                        //ReportDataSource dsAgentCommisionRpt3;
                        if (dsAgentCommision != null && dsAgentCommision.Tables[0].Rows.Count > 0)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtAgentCommision1 = dsAgentCommision.Tables[0];
                            DataTable dtAgentCommision2 = dsAgentCommision.Tables[1];
                            dsAgentCommisionRpt1 = new ReportDataSource("DataSet1", dtAgentCommision1);
                            dsAgentCommisionRpt2 = new ReportDataSource("DataSet2", dtAgentCommision2);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsAgentCommisionRpt1);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsAgentCommisionRpt2);
                            if (dtAgentCommision1 != null & dtAgentCommision1.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtAgentCommision1.Rows[0]["IVH_BIZUNIT"].ToString());
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region DPR
                    case ApplicationType.DPR:
                        ReportDataSource dsDepreciationRpt;
                        if (dsDepreciation != null && dsDepreciation.Tables[0].Rows.Count > 0)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtDepreciation = dsDepreciation.Tables[0];
                            dsDepreciationRpt = new ReportDataSource("DataSet1", dtDepreciation);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsDepreciationRpt);
                            if (dtDepreciation != null & dtDepreciation.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtDepreciation.Rows[0]["asr_company"].ToString());
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region EMR
                    case ApplicationType.EMR:
                        ReportDataSource dsExtHr = null;
                        ReportDataSource dsExtDt = null;
                        if (dsExtMaterialRecive != null)
                        {
                            if (dsExtMaterialRecive.Tables.Count > 0)
                            {
                                SetReportParameters(locRpt);
                                dsExtHr = new ReportDataSource("StoreRequestHeader", dsExtMaterialRecive.Tables[0]);
                                dsExtDt = new ReportDataSource("StoreRequestDetail", dsExtMaterialRecive.Tables[1]);
                                if (dsExtHr != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsExtHr);
                                if (dsExtDt != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsExtDt);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails());
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region DSA
                    case ApplicationType.DSA:
                        if (DataSet1 != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtGRNHeaderDtls = DataSet1.Tables[0];
                            DataTable dtGRNDtls = DataSet1.Tables[1];
                            dsGRNHeaderDtls = new ReportDataSource("GRNHdr", dtGRNHeaderDtls);
                            dsGRNDtls = new ReportDataSource("GRNDtls", dtGRNDtls);

                            //rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGRNHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGRNDtls);
                            if (dtGRNHeaderDtls != null & dtGRNHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtGRNHeaderDtls.Rows[0]["GRH_COMPANY"].ToString());
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region Opening Source
                    case ApplicationType.OS:
                        ReportDataSource dsExtHrOS = null;
                        ReportDataSource dsExtDtOS = null;
                        if (dsOpeningStock != null)
                        {
                            if (dsOpeningStock.Tables.Count > 0)
                            {
                                SetReportParameters(locRpt);
                                dsExtHrOS = new ReportDataSource("StoreRequestHeader", dsOpeningStock.Tables[0]);
                                dsExtDtOS = new ReportDataSource("StoreRequestDetail", dsOpeningStock.Tables[1]);
                                if (dsExtHrOS != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsExtHrOS);
                                if (dsExtDtOS != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsExtDtOS);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails());
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region SR
                    case ApplicationType.SR:
                        ReportDataSource dsServiceRequestHeaderDtls = null;
                        ReportDataSource dsServiceRequestDetails = null;
                        if (dsServiceRequest != null)
                        {
                            if (dsServiceRequest.Tables.Count > 0)
                            {
                                DataTable dtSRHeaderDtls = dsServiceRequest.Tables[0];
                                SetReportParameters(locRpt);
                                dsServiceRequestHeaderDtls = new ReportDataSource("HeaderDt", dsServiceRequest.Tables[0]);
                                dsServiceRequestDetails = new ReportDataSource("DetailDt", dsServiceRequest.Tables[1]);
                                if (dsServiceRequestHeaderDtls != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsServiceRequestHeaderDtls);
                                if (dsServiceRequestDetails != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsServiceRequestDetails);

                                if (dtSRHeaderDtls != null && dtSRHeaderDtls.Rows.Count > 0)
                                {
                                    CompanyPK = Convert.ToInt32(dtSRHeaderDtls.Rows[0]["SRH_COMPANY"]);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                                }
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region SOA
                    case ApplicationType.SOA:
                        ReportDataSource dsServiceOrderHeaderDtls = null;
                        ReportDataSource dsServiceOrderDetails = null;
                        if (dsServiceOrder != null)
                        {
                            if (dsServiceOrder.Tables.Count > 0)
                            {
                                DataTable dtSOAHeaderDtls = dsServiceOrder.Tables[0];
                                SetReportParameters(locRpt);
                                dsServiceOrderHeaderDtls = new ReportDataSource("HeaderDt", dsServiceOrder.Tables[0]);
                                dsServiceOrderDetails = new ReportDataSource("DetailDt", dsServiceOrder.Tables[1]);
                                if (dsServiceOrderHeaderDtls != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsServiceOrderHeaderDtls);
                                if (dsServiceOrderDetails != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsServiceOrderDetails);
                                if (dtSOAHeaderDtls != null && dtSOAHeaderDtls.Rows.Count > 0)
                                {
                                    CompanyPK = Convert.ToInt32(dtSOAHeaderDtls.Rows[0]["OSH_COMPANY"]);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                                }
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region SRA
                    case ApplicationType.SRA:
                        ReportDataSource dsServiceOrderReceiptHeaderDtls = null;
                        ReportDataSource dsServiceOrderReceiptDetails = null;
                        if (dsServiceOrderReceipt != null)
                        {
                            if (dsServiceOrderReceipt.Tables.Count > 0)
                            {
                                DataTable dtSRAHeaderDtls = dsServiceOrderReceipt.Tables[0];
                                SetReportParameters(locRpt);
                                dsServiceOrderReceiptHeaderDtls = new ReportDataSource("HeaderDt", dsServiceOrderReceipt.Tables[0]);
                                dsServiceOrderReceiptDetails = new ReportDataSource("DetailDt", dsServiceOrderReceipt.Tables[1]);
                                if (dsServiceOrderReceiptHeaderDtls != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsServiceOrderReceiptHeaderDtls);
                                if (dsServiceOrderReceiptDetails != null)
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsServiceOrderReceiptDetails);
                                if (dtSRAHeaderDtls != null && dtSRAHeaderDtls.Rows.Count > 0)
                                {
                                    CompanyPK = Convert.ToInt32(dtSRAHeaderDtls.Rows[0]["RSH_COMPANY"]);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                                }
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region FRD
                    case ApplicationType.FRD:
                        ReportDataSource dsFundRequestDtls = null;
                        if (dsFundRequestDeptWise != null)
                        {
                            if (dsFundRequestDeptWise.Tables.Count > 0)
                            {
                                SetReportParameters(locRpt);
                                dsFundRequestDtls = new ReportDataSource("DataSet1", dsFundRequestDeptWise.Tables[0]);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsFundRequestDtls);
                                if (dsFundRequestDtls != null)
                                {
                                    CompanyPK = Convert.ToInt32(dsFundRequestDeptWise.Tables[0].Rows[0]["DFH_COMPANY"].ToString());
                                }
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region DONOTE
                    case ApplicationType.DONOTE:
                        ReportDataSource dsIssueHeader;
                        ReportDataSource dsIssueDetail;
                        ReportDataSource dsBinIssueHeader;
                        ReportDataSource dsBinIssueDetail;
                        ReportDataSource dsCartonIssueHeader;
                        ReportDataSource dsCartonIssueDetail;

                        if (dsWOIssue != null)
                        {
                            DataTable dtIssueHeader = dsWOIssue.Tables[0];
                            DataTable dtIssueDetail = dsWOIssue.Tables[1];
                            DataTable dtBinHeader = dsWOIssue.Tables[2];
                            DataTable dtBinDetail = dsWOIssue.Tables[3];
                            DataTable dtCartonHeader = dsWOIssue.Tables[4];
                            DataTable dtCartonDetail = dsWOIssue.Tables[5];
                            SetReportParameters(locRpt);
                            dsIssueHeader = new ReportDataSource("IssueHeaderDS", dtIssueHeader);
                            dsIssueDetail = new ReportDataSource("IssueDetailDS", dtIssueDetail);
                            dsBinIssueHeader = new ReportDataSource("BinIssueHeaderDS", dtBinHeader);
                            dsBinIssueDetail = new ReportDataSource("BinIssueDetailDS", dtBinDetail);
                            dsCartonIssueHeader = new ReportDataSource("CartonIssueHeaderDS", dtCartonHeader);
                            dsCartonIssueDetail = new ReportDataSource("CartonIssueDetailDS", dtCartonDetail);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsIssueHeader);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsIssueDetail);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsBinIssueHeader);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsBinIssueDetail);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCartonIssueHeader);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCartonIssueDetail);
                            CompanyPK = Convert.ToInt32(dsWOIssue.Tables[0].Rows[0]["MIH_COMPANY"].ToString());
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;

                    #endregion

                    #region BINCARDISSUEWO
                    case ApplicationType.BINCARDISSUEWO:
                    case ApplicationType.CARTONISSUEWO:
                        if (dsIssueReportDetails != null)
                        {
                            SetReportParameters(locRpt);
                            dtHeader = dsIssueReportDetails.Tables[0];
                            dtDetails1 = dsIssueReportDetails.Tables[1];
                            rdsHeader = new ReportDataSource("DataSet1", dtHeader);
                            rdsDetails1 = new ReportDataSource("DataSet2", dtDetails1);
                            rvViewReport.LocalReport.DataSources.Add(rdsHeader);
                            rvViewReport.LocalReport.DataSources.Add(rdsDetails1);
                            CompanyPK = Convert.ToInt32(dsIssueReportDetails.Tables[0].Rows[0]["WIH_COMPANY"].ToString());
                            rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                        }
                        else
                        {
                            rvViewReport.Visible = false;
                            divReportViewer.Visible = false;
                            rvViewReport.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region MCR
                    case ApplicationType.MCR:
                        if (RptSubType == 1) //--------------ganesh
                        {
                            if (dsReportDetails != null)
                            {
                                SetReportParameters(locRpt);
                                //Modified on 9-Jan-2016 for changing report based on Bin Card type
                                if (appType == ApplicationType.MCR)
                                {
                                    #region PDF Satrt
                                    byte[] barcodeInBytes = null;
                                    byte[] qrCodeInBytes = null;
                                    System.Drawing.Image qrCodeImage = null;
                                    try
                                    {
                                        #region Barcode Library 128 B Algorithm
                                        //if BincardCodeType=1 Show Barcode
                                        if (GetGlobalResourceObject("ConfigurationsRes", "BincardCodeType").ToString() == PrintCodeType.BarCode)
                                        {
                                            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                                            b.IncludeLabel = false;
                                            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                                            string Forecolor = "000000";
                                            string Backcolor = "FFFFFF";
                                            int imageWidth = 350;
                                            int imageHeight = 175;
                                            System.Drawing.Image barcodeImage = null;
                                            BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128B;
                                            barcodeImage = b.Encode(type, dsReportDetails.Tables[13].Rows[0]["BCH_NO"].ToString().Trim(), System.Drawing.ColorTranslator.FromHtml("#" + Forecolor), System.Drawing.ColorTranslator.FromHtml("#" + Backcolor), imageWidth, imageHeight);
                                            barcodeInBytes = CommonFunctions.ImageToByte(barcodeImage);
                                        }
                                        #endregion

                                        #region Generate QRCode
                                        //if BincardCodeType=2 Show QR Code
                                        else if (GetGlobalResourceObject("ConfigurationsRes", "BincardCodeType").ToString() == PrintCodeType.QrCode)
                                        {
                                            qrCodeImage = BarcodeLib.QRCodeLib.GetQRCode(dsReportDetails.Tables[13].Rows[0]["BCH_NO"].ToString().Trim());
                                        }
                                        #endregion

                                        //System.Drawing.Image myimg = Code128Rendering.MakeBarcodeImage(dsReportDetails.Tables[13].Rows[0]["BCH_NO"].ToString(), 2, true);
                                        //barcodeInBytes = CommonFunctions.ImageToByte(myimg);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                    if (GetGlobalResourceObject("ConfigurationsRes", "BincardCodeType").ToString() == PrintCodeType.BarCode)
                                    {
                                        ReportParameter BarCode = new ReportParameter("BarCode", Convert.ToBase64String(barcodeInBytes));
                                        locRpt.SetParameters(BarCode);
                                    }
                                    else if (GetGlobalResourceObject("ConfigurationsRes", "BincardCodeType").ToString() == PrintCodeType.QrCode)
                                    {
                                        ReportParameterInfoCollection rptPC;
                                        rptPC = locRpt.GetParameters();
                                        foreach (ReportParameterInfo p in rptPC)
                                        {
                                            if (p.Name == "QRCode")
                                            {
                                                qrCodeInBytes = CommonFunctions.ImageToByte(qrCodeImage);
                                                ReportParameter QrCode = new ReportParameter("QRCode", Convert.ToBase64String(qrCodeInBytes));
                                                locRpt.SetParameters(QrCode);
                                            }
                                        }
                                    }
                                    ReportParameter CurUser = new ReportParameter("CurUser", currentUser.Name);
                                    locRpt.SetParameters(CurUser);
                                    ReportParameter IsPrinted = new ReportParameter("IsPrinted", Request.QueryString["IsPrinted"] != null ? Request.QueryString["IsPrinted"].ToString() : "0");
                                    locRpt.SetParameters(IsPrinted);
                                    ReportParameter FromPrdQA = new ReportParameter("FromPrdQA", Request.QueryString["FromPrdQA"] != null ? Request.QueryString["FromPrdQA"].ToString() : "0");
                                    locRpt.SetParameters(FromPrdQA);
                                    ReportParameter BinCardType = new ReportParameter("BinCardType", Request.QueryString["BinCardType"] != null ? Request.QueryString["BinCardType"].ToString() : "0");
                                    locRpt.SetParameters(BinCardType);
                                    #endregion

                                    ReportDataSource dsHeaderWeight = new ReportDataSource("HeaderWeightDS", dsReportDetails.Tables[0]);
                                    ReportDataSource dsTumbling = new ReportDataSource("TumblingDS", dsReportDetails.Tables[1]);
                                    ReportDataSource dsPreAudit = new ReportDataSource("PreAuditDS", dsReportDetails.Tables[2]);
                                    ReportDataSource dsInspection = new ReportDataSource("InspectionDS", dsReportDetails.Tables[3]);
                                    ReportDataSource dsChlorination = new ReportDataSource("ChlorinationDS", dsReportDetails.Tables[4]);
                                    ReportDataSource dsWaterTightTest = new ReportDataSource("WaterTightTestDS", dsReportDetails.Tables[5]);
                                    ReportDataSource dsAirtest = new ReportDataSource("AirTestDS", dsReportDetails.Tables[6]);
                                    ReportDataSource dsPacking = new ReportDataSource("PackingDS", dsReportDetails.Tables[7]);
                                    ReportDataSource dsLeaching = new ReportDataSource("LeachingDS", dsReportDetails.Tables[8]);
                                    ReportDataSource dsWashing = new ReportDataSource("WashingDS", dsReportDetails.Tables[9]);
                                    ReportDataSource dsMoulding = new ReportDataSource("MouldingDS", dsReportDetails.Tables[10]);
                                    ReportDataSource dsToyChlorination = new ReportDataSource("ToyChlorinationDS", dsReportDetails.Tables[11]);
                                    ReportDataSource dsGliding = new ReportDataSource("GlidingDS", dsReportDetails.Tables[12]);
                                    ReportDataSource dsHeading = new ReportDataSource("HeadingDS", dsReportDetails.Tables[13]);//---heading section for traceability Leaching
                                    ReportDataSource dsWaterTightNewTest = new ReportDataSource("WaterTightTestNewDS", dsReportDetails.Tables[14]);//TM no wtt
                                    ReportDataSource dsVisualInspectionNew = new ReportDataSource("VisualInspectionNewDS", dsReportDetails.Tables[15]);//TM no wtt
                                    ReportDataSource dsBasketCard = new ReportDataSource("BasketCardDS", dsReportDetails.Tables[17]);
                                    ReportDataSource dsTumblingRework = new ReportDataSource("TumblingReworkDS", dsReportDetails.Tables[18]);
                                    ReportDataSource dsPackingRework = new ReportDataSource("PackingReworkDS", dsReportDetails.Tables[19]);
                                    ReportDataSource dsTumblingQA = new ReportDataSource("TumblingQADS", dsReportDetails.Tables[20]);
                                    ReportDataSource dsPackingGrouped = new ReportDataSource("PackingGroupedDS", dsReportDetails.Tables[21]);
                                    ReportDataSource dsVisualInspectionNew_2 = new ReportDataSource("VisualInspectionNewDS_2", dsReportDetails.Tables[22]);
                                    ReportDataSource dsWaterTightTestNew_2 = new ReportDataSource("WaterTightTestNewDS_2", dsReportDetails.Tables[23]);
                                    ReportDataSource dsPackingQA = new ReportDataSource("PackingQADS", dsReportDetails.Tables[24]);
                                    ReportDataSource dsPackingReworkQA = new ReportDataSource("PackingReworkQADS", dsReportDetails.Tables[25]);
                                    ReportDataSource dsWalletBinDetails = new ReportDataSource("WalletBinDetailsDS", dsReportDetails.Tables[26]);
                                    ReportDataSource dsWalletHeader = new ReportDataSource("WalletHeaderDS", dsReportDetails.Tables[27]);

                                    rvViewReport.LocalReport.DataSources.Add(dsHeaderWeight);
                                    rvViewReport.LocalReport.DataSources.Add(dsTumbling);
                                    rvViewReport.LocalReport.DataSources.Add(dsPreAudit);
                                    rvViewReport.LocalReport.DataSources.Add(dsInspection);
                                    rvViewReport.LocalReport.DataSources.Add(dsChlorination);
                                    rvViewReport.LocalReport.DataSources.Add(dsWaterTightTest);
                                    rvViewReport.LocalReport.DataSources.Add(dsAirtest);
                                    rvViewReport.LocalReport.DataSources.Add(dsPacking);
                                    rvViewReport.LocalReport.DataSources.Add(dsLeaching);
                                    rvViewReport.LocalReport.DataSources.Add(dsWashing);
                                    rvViewReport.LocalReport.DataSources.Add(dsMoulding);
                                    rvViewReport.LocalReport.DataSources.Add(dsToyChlorination);
                                    rvViewReport.LocalReport.DataSources.Add(dsGliding);
                                    rvViewReport.LocalReport.DataSources.Add(dsHeading);

                                    rvViewReport.LocalReport.DataSources.Add(dsWaterTightNewTest);
                                    rvViewReport.LocalReport.DataSources.Add(dsVisualInspectionNew);
                                    rvViewReport.LocalReport.DataSources.Add(dsBasketCard);
                                    rvViewReport.LocalReport.DataSources.Add(dsTumblingRework);
                                    rvViewReport.LocalReport.DataSources.Add(dsPackingRework);
                                    rvViewReport.LocalReport.DataSources.Add(dsTumblingQA);
                                    rvViewReport.LocalReport.DataSources.Add(dsPackingGrouped);
                                    rvViewReport.LocalReport.DataSources.Add(dsVisualInspectionNew_2);
                                    rvViewReport.LocalReport.DataSources.Add(dsWaterTightTestNew_2);
                                    rvViewReport.LocalReport.DataSources.Add(dsPackingQA);
                                    rvViewReport.LocalReport.DataSources.Add(dsPackingReworkQA);
                                    rvViewReport.LocalReport.DataSources.Add(dsWalletBinDetails);
                                    rvViewReport.LocalReport.DataSources.Add(dsWalletHeader);

                                    rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(1));

                                    if (appType == ApplicationType.MCR)
                                    {
                                        ReportDataSource dsVisualInspectionAfterChl = new ReportDataSource("VisualInspectionAfterChlDS", dsReportDetails.Tables[16]);//For AfterProcess Chlorination
                                        rvViewReport.LocalReport.DataSources.Add(dsVisualInspectionAfterChl);
                                    }
                                    //rvViewReport.Reset();
                                    rvViewReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);

                                }
                            }
                            else
                            {
                                rvViewReport.Visible = false;
                                divReportViewer.Visible = false;
                                rvViewReport.Visible = false;
                                divNodata.Visible = true;
                            }
                        }
                        break;
                    #endregion
                    #region ASD
                    case ApplicationType.ASD:
                        ReportDataSource dsAssetDisposalDtls = null;

                        if (dsAssetDisposal != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtAssetDisposalDtls = dsAssetDisposal.Tables[0];
                            dsAssetDisposalDtls = new ReportDataSource("DataSet1", dsAssetDisposal.Tables[0]);
                            //dsServiceRequestDetails = new ReportDataSource("DetailDt", dsServiceRequest.Tables[1]);
                            if (dsAssetDisposalDtls != null)
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsAssetDisposalDtls);

                            if (dtAssetDisposalDtls != null && dtAssetDisposalDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtAssetDisposalDtls.Rows[0]["asrCompany"]);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }

                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region CWIP
                    case ApplicationType.CWIP:
                        ReportDataSource dsCWIPDtls = null;

                        if (dsCWIP != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtCWIPDtls = dsCWIP.Tables[0];
                            dsCWIPDtls = new ReportDataSource("DataSet1", dsCWIP.Tables[0]);

                            if (dsCWIPDtls != null)
                                rvCurrentRptViewer.LocalReport.DataSources.Add(dsCWIPDtls);

                            if (dtCWIPDtls != null && dtCWIPDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtCWIPDtls.Rows[0]["CWH_COMPANY"]);
                                rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            }

                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region SHCID
                    case ApplicationType.SHCID:
                        ReportDataSource dsSHCIDHeaderDtls;
                        ReportDataSource dsSHCIDProductDtls;
                        ReportDataSource dsSHCIDOtherDtls;
                        //ReportDataSource dsSHCIDTaxDtls;
                        //ReportDataSource dsSHCIDTaxFullDtls;
                        //ReportDataSource dsSHCIDTotTaxDtls;
                        //ReportDataSource dsSHCIDCustomsDtls;
                        if (dsDelivaryOrder != null)
                        {
                            DataTable dtSHCIDHeaderDtls = dsDelivaryOrder.Tables[0];
                            DataTable dtSHCIDProductDtls = dsDelivaryOrder.Tables[1];
                            DataTable dtSHCIDOtherDtls = dsDelivaryOrder.Tables[2];
                            //DataTable dtSHCIDTaxDtls = dsDelivaryOrder.Tables[3];
                            //DataTable dtSHCIDTotTaxDtls = dsDelivaryOrder.Tables[4];
                            //DataTable dtSHCIDCustDtls = dsDelivaryOrder.Tables[5];

                            dsSHCIDHeaderDtls = new ReportDataSource("DOHeaderDtls", dtSHCIDHeaderDtls);
                            dsSHCIDProductDtls = new ReportDataSource("DOProductDtls", dtSHCIDProductDtls);
                            dsSHCIDOtherDtls = new ReportDataSource("DOOtherDtls", dtSHCIDOtherDtls);
                            //dsSHCIDTaxDtls = new ReportDataSource("TaxDtls", dtSHCIDTaxDtls);
                            //dsSHCIDTaxFullDtls = new ReportDataSource("TaxFullDtls", dtSHCIDTotTaxDtls);
                            //dsSHCIDCustomsDtls = new ReportDataSource("CustDtls", dtSHCIDCustDtls);
                            /*if (!string.IsNullOrEmpty(Convert.ToString(dtSHCIDHeaderDtls.Rows[0]["DPH_SWAP_BUYER"])))
                            {
                                SwapBuyer = Convert.ToInt32(dtSHCIDHeaderDtls.Rows[0]["DPH_SWAP_BUYER"].ToString());
                            }
                            Buyer = dtSHCIDHeaderDtls.Rows[0]["DPH_ADNL_BUYER"].ToString();
                            if (!string.IsNullOrEmpty(Convert.ToString(dtSHCIDHeaderDtls.Rows[0]["DPH_PRINT_SHIP_TO"])))
                            {
                                PrintShipTo = Convert.ToBoolean(dtSHCIDHeaderDtls.Rows[0]["DPH_PRINT_SHIP_TO"]);
                            }*/
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSHCIDHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSHCIDProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSHCIDOtherDtls);
                            if (dtSHCIDHeaderDtls != null & dtSHCIDHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtSHCIDHeaderDtls.Rows[0]["DPH_COMPANY"].ToString());
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            //rvCurrentRptViewer.LocalReport.DataSources.Add(dsSHCIDTaxDtls);
                            //rvCurrentRptViewer.LocalReport.DataSources.Add(dsSHCIDTaxFullDtls);
                            //vCurrentRptViewer.LocalReport.DataSources.Add(dsSHCIDCustomsDtls);
                            /* var groupdata = (from t in dtSHCIDTotTaxDtls.AsEnumerable()
                                              group t by new
                                              {
                                                  ISH_TAX = t.Field<int?>("ISH_TAX"),
                                              } into dt
                                              select new
                                              {
                                                  dt.Key.ISH_TAX,
                                                  TAX_CODE = dt.First().Field<string>("TAX_CODE"),
                                                  ISH_NAME = dt.First().Field<string>("ISH_NAME"),
                                                  TAX_DISP_NAME = dt.First().Field<string>("TAX_DISP_NAME"),
                                                  TAX_RATE = dt.First().Field<decimal?>("TAX_RATE"),
                                                  ISH_TAX_AMT = dt.Sum(x => x.Field<decimal>("ISH_TAX_AMT")),
                                                  ISH_TOTAL = dt.Sum(x => x.Field<decimal>("ISH_TOTAL"))
                                              }).ToList();
                             DataTable dtGrpTax = groupdata.ToDataTable();
                             dsCIDTotTaxDtls = new ReportDataSource("TotTaxDtls", dtGrpTax);
                             rvCurrentRptViewer.LocalReport.DataSources.Add(dsCIDTotTaxDtls);*/
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            divNodata.Visible = true;
                        }

                        break;
                        #endregion
                }
                if (!FromExternal)
                    rvViewReport.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                rvCurrentRptViewer.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                rvCurrentRptViewer.LocalReport.Refresh();
                ConfigurationSettings();

                if (FromExternal)
                    SavePDF(locRpt);
                else
                    if (appType != ApplicationType.COA && appType != ApplicationType.AS && appType != ApplicationType.GLFIN && appType != ApplicationType.BRC && appType != ApplicationType.PSAS && appType != ApplicationType.SAS && appType != ApplicationType.TB)
                {
                    if (IsExportExcel == false)
                    {
                        if (IsExcelPrint == 1)//Show Report in Excel based on QueryString
                            SaveExcel(locRpt);
                        else
                            ShowPDF(appType);
                    }
                    else
                    {
                        if (((appType != ApplicationType.SI || appType != ApplicationType.SIC || appType != ApplicationType.DSI) || (RptSubType != (int)SalesInvoiceType.Domestic && RptSubType != (int)SalesInvoiceType.Export && RptSubType != (int)SalesInvoiceType.Proforma)) && (appType != ApplicationType.DO || (RptSubType != (int)DOSubType.DeliveryOrder && RptSubType != (int)DOSubType.PackingList && RptSubType != (int)DOSubType.PackingList_2_LP && RptSubType != (int)DOSubType.PackingList_LP)) && (appType != ApplicationType.DOD || (RptSubType != (int)DOSubType.DeliveryOrder && RptSubType != (int)DOSubType.PackingList && RptSubType != (int)DOSubType.PackingList_2_LP && RptSubType != (int)DOSubType.PackingList_LP)))
                        {
                            if (IsExcelPrint == 1)
                                SaveExcel(locRpt);
                            else
                                ShowPDF(appType);
                        }
                        else
                        {
                            btnReport.Visible = false;
                            btnSearch.Visible = false;
                        }
                    }
                }
                if (FromExternal && IsPdfGenerated)
                    setResult = true;
                else
                    setResult = false;
                return setResult;
            }
            #region HalfPagePrint
            //if (appType != ApplicationType.TB && appType != ApplicationType.PIJ && appType != ApplicationType.SIJ && appType != ApplicationType.PSIJ && appType != ApplicationType.MSIJ && appType != ApplicationType.PCS && appType != ApplicationType.JV && appType != ApplicationType.CRJ && appType != ApplicationType.EIJ && appType != ApplicationType.AS && appType != ApplicationType.SAS && appType != ApplicationType.BRC && appType != ApplicationType.PSAS && appType != ApplicationType.CNJ && appType != ApplicationType.DNJ)
            //{
            //    PrintPDF();
            //}
            //else if ((appType == ApplicationType.SIJ || appType == ApplicationType.CRJ) & RptSubType == 1)
            //{
            //    PrintPDF();
            //}
            //if (appType == ApplicationType.PIJ || appType == ApplicationType.PSIJ || appType == ApplicationType.JV || appType == ApplicationType.CRJ || appType == ApplicationType.DNJ || appType == ApplicationType.CNJ || appType == ApplicationType.MSIRJ || appType == ApplicationType.SIJ || appType == ApplicationType.EIJ || appType == ApplicationType.PCS || appType == ApplicationType.MSIJ)
            //{
            //    btnSearch.Visible = false;
            //    btnCancel.Visible = false;
            //}
            #endregion
            catch (Exception ex)
            {
                CommonBL.ExceptionWriting(ex.GetInnerExceptionMsg(), "Inner Exception");
                CommonBL.ExceptionWriting(ex.ToString(), "Generate Reports : " + ReportFile);

                if (ex.InnerException != null && ex.InnerException.StackTrace != null)
                {
                    CommonBL.ExceptionWriting(ex.InnerException.StackTrace, "Inner Stack Trace: ");
                }
                if (ex.StackTrace != null)
                {
                    CommonBL.ExceptionWriting(ex.StackTrace, "Stack Trace: ");
                }
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
                ReportFile = string.Empty;
                return false;
            }
        }


        #endregion

        #region Helper Methods

        private DataTable ConfigurationSettings()
        {
            IsTaxForOtherChargeSales = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxSales")));
            IsTaxForOtherChargePurchase = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxPurchase")));
            IsRepeatSIheader = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "RepeatSIheader")));
            IsExportExcel = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsExportExcel")));
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            if (!FromExternal)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, (FromExternal == true ? SbuID : currentUser.SBUID));
            return dt;
        }

        private void BindCrystalReportForLabel(string reportName, DataSet dsData)
        {
            reportDocument = new ReportDocument();
            reportDocument.Load(Server.MapPath("../Administration/Masters/CrystalReportFile/" + reportName));
            reportDocument.Refresh();
            CommonFunctions.SetCrystalReportDataBaseConnection(reportDocument);
            reportDocument.SetDataSource(dsData); // Added report data as dataset.
            reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Page.Response, false, reportName);
            // reportDocument.PrintToPrinter(1, false, 0, 0);
            GERP_Report.ReportSource = reportDocument;
            hdfShowCrReportDiv.Value = "1";
            Session[SessionStrings.CRReportData] = reportDocument;

        }

        private DataSet GetLabelDataSourse(DataTable dtStoreLocation)
        {
            DataSet dsResult = new DataSet();
            DataTable dtLabels = new DataTable();
            dtLabels.Columns.Add(new DataColumn("Image", System.Type.GetType("System.Byte[]")));
            dtLabels.Columns.Add(new DataColumn("DPT_NAME", System.Type.GetType("System.String")));
            dtLabels.Columns.Add(new DataColumn("DPT_CODE", System.Type.GetType("System.String")));
            dtLabels.TableName = "dtStoreLocation";

            foreach (DataRow dr in dtStoreLocation.Rows)
            {
                byte[] barcodeInBytes;
                barcodeInBytes = GetImabeByte(dr["DPT_CODE"].ToString());
                DataRow dtrow = dtLabels.NewRow();
                dtrow["Image"] = barcodeInBytes;
                dtrow["DPT_NAME"] = dr["DPT_NAME"].ToString();
                dtrow["DPT_CODE"] = dr["DPT_CODE"].ToString();
                dtLabels.Rows.Add(dtrow);
            }
            dsResult.Tables.Add(dtLabels);
            // binControl.ProdGrade
            return dsResult;
        }
        private System.Byte[] GetImabeByte(string binNo)
        {
            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            b.IncludeLabel = false;

            switch (GetGlobalResourceObject("ConfigurationsRes", "ImgAlignment").ToString().Trim().ToUpper())
            {
                case "CENTER":
                    b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                    break;
                case "LEFT":
                    b.Alignment = BarcodeLib.AlignmentPositions.LEFT;
                    break;
                case "RIGHT":
                    b.Alignment = BarcodeLib.AlignmentPositions.RIGHT;
                    break;
                default:
                    b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                    break;
            }

            string Forecolor = GetGlobalResourceObject("ConfigurationsRes", "ImgForeColor").ToString();// "000000";
            string Backcolor = GetGlobalResourceObject("ConfigurationsRes", "ImgBackcolor").ToString();// "FFFFFF";
            int imageWidth = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ImgWidth").ToString());// 350;
            int imageHeight = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ImgHeight").ToString());// 175;
            System.Drawing.Image barcodeImage = null;
            BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128B;
            barcodeImage = b.Encode(type, binNo.Trim(), System.Drawing.ColorTranslator.FromHtml("#" + Forecolor), System.Drawing.ColorTranslator.FromHtml("#" + Backcolor), imageWidth, imageHeight);
            //barcodeImage.Save(Server.MapPath("~\\Upload\\TempImage\\") + "BinNo.png", System.Drawing.Imaging.ImageFormat.Png);
            //Server.MapPath("~\\Upload\\TempImage\\")BinNo.png" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoGST"])));
            //imgBarcode.ImageUrl = ERP.Production.Utilities.CommonFunctions.ConvertToImageUrl(barcodeImage);
            return CommonFunctions.ImageToByte(barcodeImage);
        }


        private void ShowPDF(string appType)
        {
            if (appType == ApplicationType.VSE)
            {
                if (dsVatSaleExport.Tables[0].Rows.Count > 0)
                {
                    //PrintPDF();
                    SaveExcel(locRpt);
                }
                else
                {
                    btnReport.Visible = false;
                    btnSearch.Visible = false;
                    btnCancel.Visible = false;
                }
            }
            else
            {
                // SaveExcel(locRpt);
                PrintPDF();
            }
        }
        /// <summary>
        /// To set report parameters
        /// </summary>
        /// <param name="locRpt"></param>
        private void SetReportParameters(LocalReport locRpt)
        {
            try
            {
                ReportParameter parameters;
                DataSet dsParamSettings;
                string footer;
                string rptName = string.Empty;
                footer = string.Empty;
                string signaturePath = string.Empty;
                string Submittedsign = string.Empty;
                string Reviewedsign = string.Empty;
                string VerifiedSign = string.Empty;
                string signaturePathAccepted = string.Empty;
                string NumberDecimalDigitsBin = string.Empty;
                dsParamSettings = new DataSet();
                DataTable dt = ConfigurationSettings();
                locRpt.ReportPath = string.Empty;
                foreach (SPADM_APP_SUB_TYPE_DATA_GET_Result sa in AppTypeDetailsList)
                {
                    if (((sa.AST_CODE == ApplicationType.PO || sa.AST_CODE == ApplicationType.POP || sa.AST_CODE == ApplicationType.POPG || sa.AST_CODE == ApplicationType.POTR || sa.AST_CODE == ApplicationType.POG) && IsPurchaseOrderWithOutTax() && sa.AST_VALUE != 11) || (sa.AST_CODE == ApplicationType.POT && IsPurchaseOrderWithOutTax() && sa.AST_VALUE != 11) || ((sa.AST_CODE == ApplicationType.PI || sa.AST_CODE == ApplicationType.TPI || sa.AST_CODE == ApplicationType.SID || sa.AST_CODE == ApplicationType.PII) && IsInvoiceWithTax()) || (sa.AST_CODE == ApplicationType.VP && PNDCount > 0) || (sa.AST_CODE == ApplicationType.DPVJ && DPVJPNDcount > 0 || sa.AST_CODE == ApplicationType.PCVJ && DPVJPNDcount > 0))
                    {
                        rptName = sa.AST_OP_FILE2;
                    }
                    else if (sa.AST_CODE == ApplicationType.SID && SwapBuyer == 1)
                    {
                        string rdlcName = sa.AST_OP_FILE1;
                        rptName = rdlcName.Replace(".", "_1.");
                    }
                    else
                    {
                        rptName = sa.AST_OP_FILE1;//
                    }
                    if ((sa.AST_CODE == ApplicationType.VP && RptSubType == 1) || (sa.AST_CODE == ApplicationType.DPVJ && RptSubType == 4))
                    {
                        if (!string.IsNullOrEmpty(ChequeReport))
                        {
                            rptName = ChequeReport;
                        }
                    }
                    if (IsRepeatSIheader)
                    {
                        if (sa.AST_CODE == ApplicationType.SID || sa.AST_CODE == ApplicationType.SIE || sa.AST_CODE == ApplicationType.SIC)
                        {
                            if (PrintShipTo == false && SwapBuyer == 0 && Buyer == string.Empty)
                            {
                                rptName = sa.AST_OP_FILE1;
                            }
                            else if (PrintShipTo == true && SwapBuyer == 0 && Buyer == string.Empty)
                            {
                                string rdlcName = sa.AST_OP_FILE1;
                                rptName = rdlcName.Replace(".", "_1.");
                            }
                            else if (PrintShipTo == false && Buyer != string.Empty)
                            {
                                string rdlcName = sa.AST_OP_FILE1;
                                rptName = rdlcName.Replace(".", "_2.");
                            }
                            else if (PrintShipTo == true && Buyer != string.Empty)
                            {
                                string rdlcName = sa.AST_OP_FILE1;
                                rptName = rdlcName.Replace(".", "_3.");
                            }
                        }
                    }
                    if (FromExternal)
                        locRpt.ReportPath = Server.MapPath("~/Reports/" + rptName);
                    else
                        locRpt.ReportPath = Server.MapPath(rptName);
                    ReportFile = rptName;
                    if (RptType != ApplicationType.GST)
                    {
                        parameters = new ReportParameter("QMSRef", sa.AST_QMS_REF);
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("HideQMSRef", sa.AST_QMS_VISIBILITY.ToString());
                        locRpt.SetParameters(parameters);
                    }
                    if (sa.AST_RPT_SETTINGS != null)
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(sa.AST_RPT_SETTINGS)));

                    #region Signature
                    if (RptType == ApplicationType.PI || RptType == ApplicationType.TPI || RptType == ApplicationType.RFQ || RptType == ApplicationType.IO || RptType == ApplicationType.DO || RptType == ApplicationType.SI || RptType == ApplicationType.SIC || RptType == ApplicationType.DSI || RptType == ApplicationType.CID || RptType == ApplicationType.SHCID || RptType == ApplicationType.MSI || RptType == ApplicationType.MSIT || RptType == ApplicationType.CTVJ || RptType == ApplicationType.SIM || RptType == ApplicationType.SIMSS || RptType == ApplicationType.SIMSF || (RptType == ApplicationType.SPLN && RptSubType == 4))
                    {
                        parameters = new ReportParameter("ApprovedByName", sa.AST_APPROVED_USER);
                        locRpt.SetParameters(parameters);
                        if (Convert.ToString(sa.AST_APPROVED_SIGN) != string.Empty)
                        {
                            bool fileExists = false;
                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                            {
                                if (File.Exists(Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN))
                                {
                                    signaturePath = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN;
                                    fileExists = true;
                                }
                            }
                            else
                            {
                                if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + sa.AST_APPROVED_SIGN))
                                {
                                    signaturePath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + sa.AST_APPROVED_SIGN;
                                    fileExists = true;
                                }
                            }

                            //if (fileExists)
                            //{
                            //    parameters = new ReportParameter("ApprovedBySign", signaturePath);
                            //    locRpt.SetParameters(parameters);
                            //}

                            if ((sa.AST_CODE == ApplicationType.SIE && sa.AST_CODE == ApplicationType.SIC && sa.AST_VALUE == 2) || (sa.AST_CODE == ApplicationType.SIM && sa.AST_VALUE == 5) || (sa.AST_CODE == ApplicationType.SIMSF && sa.AST_VALUE == 6) || (sa.AST_CODE == ApplicationType.SIMSS && sa.AST_VALUE == 7) || sa.AST_CODE == ApplicationType.DO2 || (sa.AST_CODE == ApplicationType.CID && sa.AST_VALUE == 2) || (sa.AST_CODE == ApplicationType.SHCID && sa.AST_VALUE == 2))
                            {
                                signaturePath = string.Empty;
                                if (dsDelivaryOrder != null)
                                {
                                    if (dsDelivaryOrder.Tables.Count > 0)
                                    {
                                        if (dsDelivaryOrder.Tables[0].Rows.Count > 0)
                                        {
                                            string signature = dsDelivaryOrder.Tables[0].Rows[0][GetLocalResourceObject("SIE_ApprovedBySign").ToString()].ToString();
                                            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                            {
                                                if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature))
                                                    signaturePath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature;
                                            }
                                            else
                                            {
                                                if (File.Exists(Server.MapPath(Resources.Controls.SignaturePath) + signature))
                                                {
                                                    signaturePath = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + signature;
                                                }
                                            }
                                        }
                                    }
                                    ReportParameterInfoCollection rptPC;
                                    rptPC = locRpt.GetParameters();
                                    foreach (ReportParameterInfo p in rptPC)
                                    {
                                        if (p.Name == "ApprovedBySign")
                                        {
                                            parameters = new ReportParameter("ApprovedBySign", signaturePath);
                                            locRpt.SetParameters(parameters);
                                        }
                                        if (p.Name == "SICopyCount")
                                        {
                                            parameters = new ReportParameter("SICopyCount", SICountText.ToString());
                                            locRpt.SetParameters(parameters);
                                        }
                                    }
                                }
                            }
                            else if ((sa.AST_CODE == ApplicationType.SID && sa.AST_VALUE == 1) || (sa.AST_CODE == ApplicationType.MSI && sa.AST_VALUE == 1) || (sa.AST_CODE == ApplicationType.SAD && sa.AST_VALUE == 11) || (sa.AST_CODE == ApplicationType.MSIT && sa.AST_VALUE == 1))
                            {
                                signaturePath = string.Empty;
                                if (dsDelivaryOrder != null)
                                {
                                    if (dsDelivaryOrder.Tables.Count > 0)
                                    {
                                        if (dsDelivaryOrder.Tables[0].Rows.Count > 0)
                                        {
                                            string signature = dsDelivaryOrder.Tables[0].Rows[0][GetLocalResourceObject("SIApprovedSign").ToString()].ToString();
                                            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                            {
                                                if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature))
                                                    signaturePath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature;
                                            }
                                            else
                                            {
                                                if (File.Exists(Server.MapPath(Resources.Controls.SignaturePath) + signature))
                                                {
                                                    signaturePath = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + signature;
                                                }
                                            }
                                        }
                                    }
                                    ReportParameterInfoCollection rptPC;
                                    rptPC = locRpt.GetParameters();
                                    foreach (ReportParameterInfo p in rptPC)
                                    {
                                        if (p.Name == "ApprovedBySign")
                                        {
                                            parameters = new ReportParameter("ApprovedBySign", signaturePath);
                                            locRpt.SetParameters(parameters);
                                        }
                                        if (p.Name == "SICopyCount")
                                        {
                                            parameters = new ReportParameter("SICopyCount", SICountText.ToString());
                                            locRpt.SetParameters(parameters);
                                        }
                                    }
                                }
                            }
                            else if (sa.AST_CODE == ApplicationType.SPLNPM && sa.AST_VALUE == 4)
                            {
                                signaturePath = string.Empty;
                                if (dsShippingPlan != null)
                                {
                                    if (dsShippingPlan.Tables.Count > 0)
                                    {
                                        if (dsShippingPlan.Tables[0].Rows.Count > 0)
                                        {
                                            string signature = dsShippingPlan.Tables[0].Rows[0][GetLocalResourceObject("SNH_ApprovedBySign").ToString()].ToString();
                                            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                            {
                                                if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature))
                                                    signaturePath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature;
                                            }
                                            else
                                            {
                                                if (File.Exists(Server.MapPath(Resources.Controls.SignaturePath) + signature))
                                                {
                                                    signaturePath = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + signature;
                                                }
                                            }
                                        }
                                    }
                                    ReportParameterInfoCollection rptPC;
                                    rptPC = locRpt.GetParameters();
                                    foreach (ReportParameterInfo p in rptPC)
                                    {
                                        if (p.Name == "ApprovedBySign")
                                        {
                                            parameters = new ReportParameter("ApprovedBySign", signaturePath);
                                            locRpt.SetParameters(parameters);
                                        }
                                        if (p.Name == "SICopyCount")
                                        {
                                            parameters = new ReportParameter("SICopyCount", SICountText.ToString());
                                            locRpt.SetParameters(parameters);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    else if (RptType == ApplicationType.PO || RptType == ApplicationType.POP || RptType == ApplicationType.POG || RptType == ApplicationType.POPG || RptType == ApplicationType.POTR || RptType == ApplicationType.POT)
                    {
                        signaturePath = string.Empty;
                        Submittedsign = string.Empty;
                        Reviewedsign = string.Empty;
                        VerifiedSign = string.Empty;

                        parameters = new ReportParameter("ApprovedByName", sa.AST_APPROVED_USER);
                        locRpt.SetParameters(parameters);
                        GetClientCode();
                        if (dsPurchaseRequest != null)
                        {
                            if (dsPurchaseRequest.Tables.Count > 0)
                            {
                                if (dsPurchaseRequest.Tables[0].Rows.Count > 0)
                                {
                                    string signature = dsPurchaseRequest.Tables[0].Rows[0][Resources.DataFieldRes.POApprovedSign].ToString();
                                    if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                    {
                                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature))
                                            signaturePath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature;
                                    }
                                    else
                                    {
                                        if (File.Exists(Server.MapPath(Resources.Controls.SignaturePath) + signature))
                                        {
                                            signaturePath = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + signature;
                                        }
                                    }

                                    Submittedsign = dsPurchaseRequest.Tables[0].Rows[0][Resources.DataFieldRes.POHSubmittedSign].ToString();
                                    if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                    {
                                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + Submittedsign))
                                            Submittedsign = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + Submittedsign;
                                    }
                                    else
                                    {
                                        if (File.Exists(Server.MapPath(Resources.Controls.SignaturePath) + Submittedsign))
                                        {
                                            Submittedsign = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + Submittedsign;
                                        }
                                    }

                                    if (dsPurchaseRequest.Tables[0].Columns.Contains(Resources.DataFieldRes.POHReviewedSign))
                                    {
                                        Reviewedsign = dsPurchaseRequest.Tables[0].Rows[0][Resources.DataFieldRes.POHReviewedSign].ToString();
                                        if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                        {
                                            if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + Reviewedsign))
                                                Reviewedsign = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + Reviewedsign;
                                        }
                                        else
                                        {
                                            if (File.Exists(Server.MapPath(Resources.Controls.SignaturePath) + Reviewedsign))
                                            {
                                                Reviewedsign = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + Reviewedsign;
                                            }
                                        }
                                    }
                                    if (dsPurchaseRequest.Tables[0].Columns.Contains(Resources.DataFieldRes.POHVerifiedSign))
                                    {
                                        VerifiedSign = dsPurchaseRequest.Tables[0].Rows[0][Resources.DataFieldRes.POHVerifiedSign].ToString();
                                        if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                        {
                                            if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + VerifiedSign))
                                                VerifiedSign = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + VerifiedSign;
                                        }
                                        else
                                        {
                                            if (File.Exists(Server.MapPath(Resources.Controls.SignaturePath) + VerifiedSign))
                                            {
                                                VerifiedSign = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + VerifiedSign;
                                            }
                                        }
                                    }
                                    if (clientCode == ClientCode.WARM.ToString() && RptType == ApplicationType.PO)
                                    {
                                        if (dsPurchaseRequest.Tables[0].Columns.Contains(Resources.DataFieldRes.POVERIFIEDBYSIGN))
                                        {
                                            Submittedsign = dsPurchaseRequest.Tables[0].Rows[0][Resources.DataFieldRes.POVERIFIEDBYSIGN].ToString();
                                            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                            {
                                                if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + Submittedsign))
                                                    Submittedsign = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + Submittedsign;
                                            }
                                            else
                                            {
                                                if (File.Exists(Server.MapPath(Resources.Controls.SignaturePath) + Submittedsign))
                                                {
                                                    Submittedsign = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + Submittedsign;
                                                }
                                            }
                                        }
                                        if (dsPurchaseRequest.Tables[0].Columns.Contains(Resources.DataFieldRes.POAPPROVEDBYSIGN))
                                        {
                                            VerifiedSign = dsPurchaseRequest.Tables[0].Rows[0][Resources.DataFieldRes.POAPPROVEDBYSIGN].ToString();
                                            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                            {
                                                if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + VerifiedSign))
                                                    VerifiedSign = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + VerifiedSign;
                                            }
                                            else
                                            {
                                                if (File.Exists(Server.MapPath(Resources.Controls.SignaturePath) + VerifiedSign))
                                                {
                                                    VerifiedSign = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + VerifiedSign;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            ReportParameterInfoCollection rptPC;
                            rptPC = locRpt.GetParameters();
                            foreach (ReportParameterInfo p in rptPC)
                            {

                                if (p.Name == "ApprovedBySign")
                                {
                                    parameters = new ReportParameter("ApprovedBySign", signaturePath);
                                    locRpt.SetParameters(parameters);
                                }
                                if (p.Name == "SubmittedBySign")
                                {
                                    parameters = new ReportParameter("SubmittedBySign", Submittedsign);
                                    locRpt.SetParameters(parameters);
                                }
                                if (p.Name == "ReviewedBySign")
                                {
                                    parameters = new ReportParameter("ReviewedBySign", Reviewedsign);
                                    locRpt.SetParameters(parameters);
                                }
                                if (p.Name == "VerifiedBySign")
                                {
                                    parameters = new ReportParameter("VerifiedBySign", VerifiedSign);
                                    locRpt.SetParameters(parameters);
                                }

                            }

                        }
                    }
                    else if (RptType == ApplicationType.SO || RptType == ApplicationType.IO)
                    {
                        signaturePath = string.Empty;
                        if (dsSaleOrder != null)
                        {
                            if (dsSaleOrder.Tables.Count > 0)
                            {
                                if (dsSaleOrder.Tables[0].Rows.Count > 0)
                                {
                                    string signature = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.SOApprovedSign].ToString();
                                    string signatureAccepted = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.SOAcceptedSign].ToString();//SOH_ACCEPTED_SIGN
                                    if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                    {
                                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature))
                                            signaturePath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature;
                                        //Accepted Signature
                                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signatureAccepted))
                                            signaturePathAccepted = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signatureAccepted;
                                    }
                                }
                            }
                            ReportParameterInfoCollection rptPC;
                            rptPC = locRpt.GetParameters();
                            foreach (ReportParameterInfo p in rptPC)
                            {
                                if (p.Name == "ApprovedBySign")
                                {

                                    parameters = new ReportParameter("ApprovedBySign", signaturePath);
                                    locRpt.SetParameters(parameters);
                                }
                                if (p.Name == "AcceptedBySign")
                                {

                                    parameters = new ReportParameter("AcceptedBySign", signaturePathAccepted);
                                    locRpt.SetParameters(parameters);
                                }
                            }
                        }
                    }
                    #endregion
                    else if (RptType == ApplicationType.TB || RptType == ApplicationType.AS || RptType == ApplicationType.SAS || RptType == ApplicationType.BRC || RptType == ApplicationType.PSAS || RptType == ApplicationType.GLFIN)
                    {
                        if (usrDateFilter.Visible)
                        {
                            parameters = new ReportParameter("FromDate", usrDateFilter.FromDate);
                            locRpt.SetParameters(parameters);
                            parameters = new ReportParameter("ToDate", usrDateFilter.ToDate);
                            locRpt.SetParameters(parameters);
                        }
                        else
                        {
                            parameters = new ReportParameter("FromDate", txtFromDate.Text.Trim());
                            locRpt.SetParameters(parameters);
                            parameters = new ReportParameter("ToDate", txtToDate.Text.Trim());
                            locRpt.SetParameters(parameters);
                        }
                    }

                    if (RptType == ApplicationType.DN || RptType == ApplicationType.CN || RptType == ApplicationType.DNT || RptType == ApplicationType.CNT)
                    {
                        parameters = new ReportParameter("ParamReason", RptType == ApplicationType.DN ? this.GetLocalResourceObject("lblDNReason").ToString() : this.GetLocalResourceObject("lblCNReason").ToString());
                        locRpt.SetParameters(parameters);
                    }
                    if (RptType == ApplicationType.DNJ || RptType == ApplicationType.DNTJ || RptType == ApplicationType.JV || RptType == ApplicationType.CNJ || RptType == ApplicationType.CNTJ || RptType == ApplicationType.VPJYE || RptType == ApplicationType.SIJYE || RptType == ApplicationType.CRJYE ||
                        RptType == ApplicationType.EIJYE || RptType == ApplicationType.PSIJYE || RptType == ApplicationType.EIPJYE || RptType == ApplicationType.SIPJYE ||
                        RptType == ApplicationType.MSIJYE || RptType == ApplicationType.DPRJ || RptType == ApplicationType.CWIPJ || RptType == ApplicationType.MSIRJYE || RptType == ApplicationType.PIJYE || RptType == ApplicationType.VTYPEDPRJ || RptType == ApplicationType.VTYPECWIPJ ||
                        RptType == ApplicationType.ACIJ || RptType == ApplicationType.AIPJ || RptType == ApplicationType.DNSJYE || RptType == ApplicationType.CNSJYE ||
                        RptType == ApplicationType.CNPJYE || RptType == ApplicationType.DNPJYE || RptType == ApplicationType.DPVCJ || RptType == ApplicationType.DPBJ || RptType == ApplicationType.DRVJ || RptType == ApplicationType.MIJ ||
                        RptType == ApplicationType.VTYPE || RptType == ApplicationType.VTYPECNJPI || RptType == ApplicationType.VTYPECNJSI || RptType == ApplicationType.VTYPEDNJPI || RptType == ApplicationType.VTYPEDNJSI || RptType == ApplicationType.VTYPEMIJ || RptType == ApplicationType.ASDJ)
                    {

                        parameters = new ReportParameter("AppType", RptType);
                        locRpt.SetParameters(parameters);
                    }

                    #region FormatCalculation
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;

                    string currencyformat = "#" + currencysep + "#0.";
                    string NoFormat = "#" + currencysep + "#0.";
                    string WeightFormat = "#" + currencysep + "#0.";//Quantity Format
                    string ExchRateDigt = "#" + currencysep + "#0.";
                    string RateDeciDigt = "#" + currencysep + "#0.";
                    string RateDecDigitPP = "#" + currencysep + "#0.";
                    string QtyforPurchase = "#" + currencysep + "#0.";
                    string MisRateDecFomat = "#" + currencysep + "#0.";

                    string currencydecimals = string.Empty;
                    string Nodecimal = string.Empty;
                    string Weightdecimal = string.Empty;//Weight Decimal
                    string ExchRateDigit = string.Empty;
                    string RateDecimalDigit = string.Empty;
                    string RateDecimalDigitPP = string.Empty;
                    string QtyDecforPuchase = string.Empty;
                    string MisRateDecimal = string.Empty;
                    //for compound and dispersion
                    string CompoundingDecDigit = "#" + currencysep + "#0.";
                    string NumberDecimalCompoundingDigit = string.Empty;
                    NumberDecimalDigitsBin = "#" + currencysep + "#0.";
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int curdigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < curdigit; i++)
                        {
                            currencydecimals += "0";
                        }

                        int NoDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < NoDigit; i++)
                        {
                            Nodecimal += "0";
                        }
                        int WeightDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "WeightDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < WeightDigit; i++)
                        {
                            Weightdecimal += "0";
                        }
                        int ExchRate = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < ExchRate; i++)
                        {
                            ExchRateDigit += "0";
                        }

                        int RateDecimal = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < RateDecimal; i++)
                        {
                            RateDecimalDigit += "0";
                        }
                        int RateDecimalPP = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
                        for (int i = 0; i < RateDecimalPP; i++)
                        {
                            RateDecimalDigitPP += "0";
                        }
                        int QtyDecimalPurchase = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "NumberDecimalDigitP2P")["ACF_VALUE"].ToString());
                        for (int i = 0; i < QtyDecimalPurchase; i++)
                        {
                            QtyDecforPuchase += "0";
                        }
                        int MisRate = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "MiscRateDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < MisRate; i++)
                        {
                            MisRateDecimal += "0";
                        }

                        //for compound preperation and dispersion
                        if (sa.AST_CODE == ApplicationType.DISP || sa.AST_CODE == ApplicationType.BOM || sa.AST_CODE == ApplicationType.CMP)
                        {
                            int NumberDecimalCompounding = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "NumberDecimalDigitCompounding")["ACF_VALUE"].ToString());
                            for (int i = 0; i < NumberDecimalCompounding; i++)
                            {
                                NumberDecimalCompoundingDigit += "0";
                            }

                            //for compounding and dispersion
                            CompoundingDecDigit = CompoundingDecDigit + NumberDecimalCompoundingDigit;
                            //Adding to the report parameter
                            parameters = new ReportParameter("CompoundFormat", CompoundingDecDigit);
                            locRpt.SetParameters(parameters);

                        }
                    }
                    else
                    {
                        currencydecimals = "00";
                        Nodecimal = "00";
                        Weightdecimal = "000";
                    }
                    currencyformat = currencyformat + currencydecimals;
                    NoFormat = NoFormat + Nodecimal;
                    WeightFormat = WeightFormat + Weightdecimal;
                    ExchRateDigt = ExchRateDigt + ExchRateDigit;
                    RateDeciDigt = RateDeciDigt + RateDecimalDigit;
                    RateDecDigitPP = RateDecDigitPP + RateDecimalDigitPP;
                    QtyforPurchase = QtyforPurchase + QtyDecforPuchase;
                    MisRateDecFomat = MisRateDecFomat + MisRateDecimal;
                    #endregion
                    #region Formats
                    parameters = new ReportParameter("DateFormat", Resources.Constants.ReportDateFormat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("CurrencyFormat", currencyformat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("NumberFormat", NoFormat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("ExchangeRate", ExchRateDigt);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("RateFormat", RateDeciDigt);
                    locRpt.SetParameters(parameters);
                    if ((RptType == ApplicationType.DO && (RptSubType == 2 || RptSubType == 7 || RptSubType == 8 || RptSubType == 10)) || (RptType == ApplicationType.DOD && (RptSubType == 2 || RptSubType == 7 || RptSubType == 8)) || RptType == ApplicationType.SI || RptType == ApplicationType.SIC || RptType == ApplicationType.DSI || RptType == ApplicationType.CID || RptType == ApplicationType.SHCID || (RptType == ApplicationType.SPLN && (RptSubType == 1 || RptSubType == 4))|| RptType == ApplicationType.RMIPM)
                    {
                        parameters = new ReportParameter("WeightFormat", WeightFormat);
                        locRpt.SetParameters(parameters);
                    }
                    if (RptType == ApplicationType.PO || RptType == ApplicationType.POP || RptType == ApplicationType.POPG || RptType == ApplicationType.POTR || RptType == ApplicationType.POG || RptType == ApplicationType.PI || RptType == ApplicationType.TPI || RptType == ApplicationType.EI || RptType == ApplicationType.ES || RptType == ApplicationType.EIT || RptType == ApplicationType.POT || RptType == ApplicationType.SCWO)
                    {
                        parameters = new ReportParameter("RateFormatPP", RateDecDigitPP);
                        locRpt.SetParameters(parameters);
                    }
                    if (RptType == ApplicationType.IO)
                    {
                        parameters = new ReportParameter("DraftMode", IOType.ToString());
                        locRpt.SetParameters(parameters);
                    }
                    if (RptType == ApplicationType.PO || RptType == ApplicationType.POP || RptType == ApplicationType.POPG || RptType == ApplicationType.POTR || RptType == ApplicationType.POG || RptType == ApplicationType.POT || RptType == ApplicationType.PI || RptType == ApplicationType.TPI || RptType == ApplicationType.EI || RptType == ApplicationType.ES || RptType == ApplicationType.EIT || RptType == ApplicationType.PR || RptType == ApplicationType.PRT || RptType == ApplicationType.GIN
                        || RptType == ApplicationType.GRN || RptType == ApplicationType.DSA || RptType == ApplicationType.STA || RptType == ApplicationType.MI || RptType == ApplicationType.DONOTE || RptType == ApplicationType.CN || RptType == ApplicationType.DN || RptType == ApplicationType.CNT || RptType == ApplicationType.DNT || RptType == ApplicationType.EMR || RptType == ApplicationType.OS || RptType == ApplicationType.MTR || RptType == ApplicationType.MTI || RptType == ApplicationType.SCWO)
                    {
                        parameters = new ReportParameter("QtyFormatPurchase", QtyforPurchase);
                        locRpt.SetParameters(parameters);
                    }
                    if (RptType == ApplicationType.MSI || RptType == ApplicationType.MSIT && (RptSubType == 1 || RptSubType == 2 || RptSubType == 3 || RptSubType == 21))
                    {
                        parameters = new ReportParameter("MisRateFormat", MisRateDecFomat.ToString());
                        locRpt.SetParameters(parameters);
                    }
                    #endregion
                    #region AMOUNTTHAI
                    if (RptType == ApplicationType.VP & RptSubType == 2 || RptType == ApplicationType.VP & RptSubType == 3 || RptType == ApplicationType.DPVJ & RptSubType == 2 || RptType == ApplicationType.DPVJ & RptSubType == 3 || RptType == ApplicationType.PCVJ & RptSubType == 2 || RptType == ApplicationType.PCVJ & RptSubType == 3)
                    {
                        if (RptType == ApplicationType.VP)
                        {
                            string amountPND2 = string.Empty;
                            string amountPND3 = string.Empty;
                            string amountPND53 = string.Empty;
                            string amountPND54 = string.Empty;
                            List<SPFIN_WHT_CERTIFICATE_RPT_Result> lstPND = currentEntity.SPFIN_WHT_CERTIFICATE_RPT(RecPK).ToList();
                            amountPND2 = lstPND.Where(d => d.WTH_FORM_NO == 385).Sum(x => x.WTH_TAX_AMT).ToString();
                            amountPND3 = lstPND.Where(d => d.WTH_FORM_NO == 384).Sum(x => x.WTH_TAX_AMT).ToString();
                            amountPND53 = lstPND.Where(d => d.WTH_FORM_NO == 389).Sum(x => x.WTH_TAX_AMT).ToString();
                            amountPND54 = lstPND.Where(d => d.WTH_FORM_NO == 390).Sum(x => x.WTH_TAX_AMT).ToString();

                            string amtPND2 = new NumberToWordsConvertorFactory("THBLOCALIZE")
                                                .GetNumberToWordsConvertor()
                                                .ConvertNumberToWords(amountPND2);
                            amtPND2 = string.IsNullOrEmpty(amtPND2) ? "0" : amtPND2;
                            parameters = new ReportParameter("AmountPND2", amtPND2);
                            locRpt.SetParameters(parameters);

                            string amtPND3 = new NumberToWordsConvertorFactory("THBLOCALIZE")
                                                .GetNumberToWordsConvertor()
                                                .ConvertNumberToWords(amountPND3);
                            amtPND3 = string.IsNullOrEmpty(amtPND3) ? "0" : amtPND3;
                            parameters = new ReportParameter("AmountPND3", amtPND3);
                            locRpt.SetParameters(parameters);

                            string amtPND53 = new NumberToWordsConvertorFactory("THBLOCALIZE")
                                                .GetNumberToWordsConvertor()
                                                .ConvertNumberToWords(amountPND53);
                            amtPND53 = string.IsNullOrEmpty(amtPND53) ? "0" : amtPND53;
                            parameters = new ReportParameter("AmountPND53", amtPND53);
                            locRpt.SetParameters(parameters);

                            string amtPND54 = new NumberToWordsConvertorFactory("THBLOCALIZE")
                                                .GetNumberToWordsConvertor()
                                                .ConvertNumberToWords(amountPND54);
                            amtPND54 = string.IsNullOrEmpty(amtPND54) ? "0" : amtPND54;
                            parameters = new ReportParameter("AmountPND54", amtPND54);
                            locRpt.SetParameters(parameters);
                        }
                        else if (RptType == ApplicationType.DPVJ || RptType == ApplicationType.PCVJ)
                        {
                            string amountPND2 = string.Empty;
                            string amountPND3 = string.Empty;
                            string amountPND53 = string.Empty;
                            string amountPND54 = string.Empty;
                            List<SPFIN_DP_WHT_CERT_RPT_Result> lstPND = currentEntity.SPFIN_DP_WHT_CERT_RPT(RecPK).ToList();
                            amountPND2 = lstPND.Where(d => d.WTH_FORM_NO == 385).Sum(x => x.WTH_TAX_AMT).ToString();
                            amountPND3 = lstPND.Where(d => d.WTH_FORM_NO == 384).Sum(x => x.WTH_TAX_AMT).ToString();
                            amountPND53 = lstPND.Where(d => d.WTH_FORM_NO == 389).Sum(x => x.WTH_TAX_AMT).ToString();
                            amountPND54 = lstPND.Where(d => d.WTH_FORM_NO == 390).Sum(x => x.WTH_TAX_AMT).ToString();

                            string amtPND2 = new NumberToWordsConvertorFactory("THBLOCALIZE")
                                                .GetNumberToWordsConvertor()
                                                .ConvertNumberToWords(amountPND2);
                            amtPND2 = string.IsNullOrEmpty(amtPND2) ? "0" : amtPND2;
                            parameters = new ReportParameter("AmountPND2", amtPND2);
                            locRpt.SetParameters(parameters);

                            string amtPND3 = new NumberToWordsConvertorFactory("THBLOCALIZE")
                                                .GetNumberToWordsConvertor()
                                                .ConvertNumberToWords(amountPND3);
                            amtPND3 = string.IsNullOrEmpty(amtPND3) ? "0" : amtPND3;
                            parameters = new ReportParameter("AmountPND3", amtPND3);
                            locRpt.SetParameters(parameters);

                            string amtPND53 = new NumberToWordsConvertorFactory("THBLOCALIZE")
                                                .GetNumberToWordsConvertor()
                                                .ConvertNumberToWords(amountPND53);
                            amtPND53 = string.IsNullOrEmpty(amtPND53) ? "0" : amtPND53;
                            parameters = new ReportParameter("AmountPND53", amtPND53);
                            locRpt.SetParameters(parameters);

                            string amtPND54 = new NumberToWordsConvertorFactory("THBLOCALIZE")
                                                .GetNumberToWordsConvertor()
                                                .ConvertNumberToWords(amountPND54);
                            amtPND54 = string.IsNullOrEmpty(amtPND54) ? "0" : amtPND54;
                            parameters = new ReportParameter("AmountPND54", amtPND54);
                            locRpt.SetParameters(parameters);
                        }
                    }
                    #endregion
                    #region PND54
                    if (RptType == ApplicationType.VP && RptSubType == 2 && PNDCount > 0)
                    {
                        var q = currentEntity.SPFIN_PND54_RPT(RecPK);
                        List<SPFIN_PND54_RPT_Result> lstPND = currentEntity.SPFIN_PND54_RPT(RecPK).ToList();
                        string WHTAmount = lstPND[0].WTH_AMOUNT;
                        string TaxAmount = lstPND[0].WTH_TAX_AMT;
                        int WHTlength = WHTAmount.Length;
                        int Taxlength = TaxAmount.Length;
                        DateTime PVHdt = lstPND[0].PVH_DATE.AddYears(543);
                        string month = PVHdt.Month.ToString();
                        int monthLength = month.Length;
                        string Day = PVHdt.Day.ToString();
                        int DayLength = Day.Length;
                        for (int i = WHTlength; i < 13; i++)
                        {
                            if (WHTlength < 13)
                            {
                                WHTAmount = "0" + WHTAmount;
                            }
                        }
                        for (int i = Taxlength; i < 13; i++)
                        {
                            if (Taxlength < 13)
                            {
                                TaxAmount = "0" + TaxAmount;
                            }
                        }
                        if (monthLength == 1)
                        {
                            month = "0" + month;
                        }
                        if (DayLength == 1)
                        {
                            Day = "0" + Day;
                        }

                        string localCmpAddress = lstPND[0].CMP_ADDR3;
                        string[] Address = SplitThaiPndAddress(localCmpAddress);

                        parameters = new ReportParameter("Address1", Address[0].Length > 0 ? Address[0].ToString() : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address2", Address.Length > 1 ? (Address[1].Length > 1 ? Address[1].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address3", Address.Length > 2 ? (Address[2].Length > 1 ? Address[2].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address4", Address.Length > 3 ? (Address[3].Length > 1 ? Address[3].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address5", Address.Length > 4 ? (Address[4].Length > 1 ? Address[4].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address6", Address.Length > 5 ? (Address[5].Length > 1 ? Address[5].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address7", Address.Length > 6 ? (Address[6].Length > 1 ? Address[6].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address8", Address.Length > 7 ? (Address[7].Length > 1 ? Address[7].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address9", Address.Length > 8 ? (Address[8].Length > 1 ? Address[8].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address10", Address.Length > 9 ? (Address[9].Length > 1 ? Address[9].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address11", Address.Length > 10 ? (Address[10].Length > 1 ? Address[10].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);



                        parameters = new ReportParameter("WHTAmount", WHTAmount);
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("WHTTaxAmt", TaxAmount);
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("PVHMonth", month);
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("PVHDay", Day);
                        locRpt.SetParameters(parameters);
                    }
                    else if (RptType == ApplicationType.DPVJ && RptSubType == 2 && DPVJPNDcount > 0 || RptType == ApplicationType.PCVJ && RptSubType == 2 && DPVJPNDcount > 0)
                    {
                        var q = currentEntity.SPFIN_PND54_FIN_RPT(RecPK);
                        List<SPFIN_PND54_FIN_RPT_Result> lstPND = currentEntity.SPFIN_PND54_FIN_RPT(RecPK).ToList();

                        string WHTAmount = lstPND[0].WTH_AMOUNT;
                        string TaxAmount = lstPND[0].WTH_TAX_AMT;
                        int WHTlength = WHTAmount.Length;
                        int Taxlength = TaxAmount.Length;
                        DateTime PVHdt = Convert.ToDateTime(lstPND[0].PVH_DATE).AddYears(543);
                        string month = PVHdt.Month.ToString();
                        int monthLength = month.Length;
                        string Day = PVHdt.Day.ToString();
                        int DayLength = Day.Length;
                        for (int i = WHTlength; i < 13; i++)
                        {
                            if (WHTlength < 13)
                            {
                                WHTAmount = "0" + WHTAmount;
                            }
                        }
                        for (int i = Taxlength; i < 13; i++)
                        {
                            if (Taxlength < 13)
                            {
                                TaxAmount = "0" + TaxAmount;
                            }
                        }
                        if (monthLength == 1)
                        {
                            month = "0" + month;
                        }
                        if (DayLength == 1)
                        {
                            Day = "0" + Day;
                        }

                        string localCmpAddress = lstPND[0].CMP_ADDR3;
                        string[] Address = SplitThaiPndAddress(localCmpAddress);

                        parameters = new ReportParameter("Address1", Address[0].Length > 0 ? Address[0].ToString() : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address2", Address.Length > 1 ? (Address[1].Length > 1 ? Address[1].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address3", Address.Length > 2 ? (Address[2].Length > 1 ? Address[2].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address4", Address.Length > 3 ? (Address[3].Length > 1 ? Address[3].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address5", Address.Length > 4 ? (Address[4].Length > 1 ? Address[4].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address6", Address.Length > 5 ? (Address[5].Length > 1 ? Address[5].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address7", Address.Length > 6 ? (Address[6].Length > 1 ? Address[6].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address8", Address.Length > 7 ? (Address[7].Length > 1 ? Address[7].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address9", Address.Length > 8 ? (Address[8].Length > 1 ? Address[8].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address10", Address.Length > 9 ? (Address[9].Length > 1 ? Address[9].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("Address11", Address.Length > 10 ? (Address[10].Length > 1 ? Address[10].ToString() : "1") : "1");
                        locRpt.SetParameters(parameters);



                        parameters = new ReportParameter("WHTAmount", WHTAmount);
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("WHTTaxAmt", TaxAmount);
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("PVHMonth", month);
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("PVHDay", Day);
                        locRpt.SetParameters(parameters);
                    }
                    #endregion
                    #region Report Settings
                    if (dsParamSettings.Tables.Count > 0)
                    {
                        if (dsParamSettings.Tables[0].Columns.Contains("LOGO_HIDE"))
                        {
                            parameters = new ReportParameter("HideLogo", dsParamSettings.Tables[0].Rows[0]["LOGO_HIDE"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING_HIDE"))
                        {
                            parameters = new ReportParameter("HideHeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING_HIDE"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING_HIDE"))
                        {
                            parameters = new ReportParameter("HideSubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING_HIDE"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_LEFT_HIDE"))
                        {
                            parameters = new ReportParameter("HideFooterText", dsParamSettings.Tables[0].Rows[0]["FOOTER_LEFT_HIDE"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_RIGHT_HIDE"))
                        {
                            parameters = new ReportParameter("HidePageNo", dsParamSettings.Tables[0].Rows[0]["FOOTER_RIGHT_HIDE"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING"))
                        {

                            if (RptType == ApplicationType.BSRC)
                            {
                                DisplayTab = Convert.ToInt32(Request.QueryString["DISPLAYTYPE"]);
                                switch (DisplayTab)
                                {
                                    case 1:
                                        parameters = new ReportParameter("HeadTitle", this.GetLocalResourceObject("LocationChange").ToString());
                                        locRpt.SetParameters(parameters);
                                        break;
                                    case 2:
                                        parameters = new ReportParameter("HeadTitle", this.GetLocalResourceObject("StoreChange").ToString());
                                        locRpt.SetParameters(parameters);
                                        break;
                                    case 3:
                                        parameters = new ReportParameter("HeadTitle", this.GetLocalResourceObject("QtyChange").ToString());
                                        locRpt.SetParameters(parameters);
                                        break;
                                    case 4:
                                        parameters = new ReportParameter("HeadTitle", this.GetLocalResourceObject("Addition").ToString());
                                        locRpt.SetParameters(parameters);
                                        break;
                                    case 5:
                                        parameters = new ReportParameter("HeadTitle", this.GetLocalResourceObject("Deletion").ToString());
                                        locRpt.SetParameters(parameters);
                                        break;
                                }
                            }
                            //  lblBreadCrum.Text = this.GetLocalResourceObject("Print").ToString() + " >> " + dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString();
                            else
                            {
                                parameters = new ReportParameter("HeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString());
                                locRpt.SetParameters(parameters);
                            }

                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING"))
                        {
                            parameters = new ReportParameter("SubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                    }
                    #endregion
                }
                #region CNDN
                if (RptType == ApplicationType.CN || RptType == ApplicationType.DN || RptType == ApplicationType.CNT || RptType == ApplicationType.DNT)
                {
                    int ItemExists = 0;
                    int ItemCount = 0;
                    if (dtPRDtls != null && dtPRDtls.Rows.Count > 0)
                    {
                        ItemExists = 1;
                    }
                    if (dtPRDtls != null && dtPRDtls.Rows.Count > 10)
                    {
                        ItemCount = 1;
                    }
                    parameters = new ReportParameter("ItemExists", ItemExists.ToString());
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("ItemCount", ItemCount.ToString());
                    locRpt.SetParameters(parameters);
                }
                #endregion
                #region EIPJ
                if (RptType == ApplicationType.PCS || RptType == ApplicationType.EIPJ || RptType == ApplicationType.EIPTJ ||
                    (RptType == ApplicationType.DPVJ && DPVJPNDcount == 0 && RptSubType != 4) || RptType == ApplicationType.PCVJ && DPVJPNDcount == 0 || RptType == ApplicationType.PCRVJ && DPVJPNDcount == 0 || RptType == ApplicationType.CTVJ && DPVJPNDcount == 0 || RptType == ApplicationType.VPJ || RptType == ApplicationType.VPTJ || RptType == ApplicationType.SIPJ || RptType == ApplicationType.PPCCJ || RptType == ApplicationType.PPCCTJ || RptType == ApplicationType.PCBJ)
                {
                    lstPayment = currentEntity.SPFIN_PAYMENT_VND_VOUCHER_RPT(RecPK, TrxRefType).ToList();
                    decimal Vat = 0;
                    decimal beforeVat = 0;
                    decimal WithHolding = 0;
                    decimal VatBuyNotYetDue = 0;
                    decimal NetAmount = 0;
                    if (lstVoucher != null && lstVoucher.Count > 0)
                    {
                        for (int i = 0; i <= lstVoucher.Count - 1; i++)
                        {
                            if (lstVoucher[i].FTR_ACC_SUB_TYPE == (int)AccSubType.VatBuy)
                            {
                                decimal VatBuy = lstVoucher[i].FTR_DR_AMT_BC.Value;
                                Vat = Vat + VatBuy;
                                decimal NotYetDue = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                VatBuyNotYetDue = VatBuyNotYetDue + NotYetDue;
                            }
                            if (RptType != ApplicationType.DPVJ && RptType != ApplicationType.PCVJ)
                            {
                                if (lstVoucher[i].FTR_ACC_SUB_TYPE != (int)AccSubType.VatBuy && lstVoucher[i].FTR_ACC_SUB_TYPE != (int)AccSubType.GainLossSales && lstVoucher[i].FTR_ACC_SUB_TYPE != (int)AccSubType.BankCharge && lstVoucher[i].FTR_ACC_SUB_TYPE != (int)AccSubType.GainLossPurchase && lstVoucher[i].FTR_ACC_SUB_TYPE != (int)AccSubType.WHT)
                                {
                                    decimal BeforeVat7 = lstVoucher[i].FTR_DR_AMT_BC.Value;
                                    beforeVat = beforeVat + BeforeVat7;

                                    decimal NetAmtCR = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                    NetAmount = NetAmount + NetAmtCR;
                                }
                            }
                            else
                            {
                                if (lstVoucher[i].FTR_IS_BANK_CHARGE == 0)
                                {
                                    decimal BeforeVat7 = lstVoucher[i].FTR_DR_AMT_BC.Value;
                                    beforeVat = beforeVat + BeforeVat7;

                                    decimal NetAmtCR = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                    NetAmount = NetAmount + NetAmtCR;
                                }
                                if (lstVoucher[i].FTR_EXCLUDE_FOB == 1)
                                {
                                    decimal ExcludeFOB = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                    beforeVat = beforeVat - ExcludeFOB;
                                }
                                //Net amount shows wrong value.Commented as discussed with Manoj sir and Nikhil
                                ////if (lstVoucher[i].FTH_WHT_DTL_COUNT == 0 && lstVoucher[i].FTR_EXCLUDE_FOB == 1)
                                ////{
                                ////    decimal ExcludeFOB = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                ////    NetAmount = NetAmount - ExcludeFOB;
                                ////}
                                if (lstVoucher[i].FTH_WHT_DTL_COUNT == 0 && lstVoucher[i].FTR_ACC_SUB_TYPE == (int)AccSubType.WHT)
                                {
                                    decimal wht = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                    if (lstVoucher[i].FTR_IS_BANK_CHARGE != 0)
                                    {
                                        beforeVat = beforeVat - wht;
                                    }
                                }
                            }
                            if (RptType != ApplicationType.DPVJ && RptType != ApplicationType.PCVJ)
                            {
                                if (lstVoucher[i].FTR_ACC_SUB_TYPE == (int)AccSubType.WHT)
                                {
                                    decimal WithHoldingTax = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                    WithHolding = WithHolding + WithHoldingTax;
                                }
                            }
                            else
                            {
                                if (lstVoucher[i].FTR_ACC_SUB_TYPE == (int)AccSubType.WHT)
                                {
                                    if (lstVoucher[i].FTH_WHT_DTL_COUNT == 0)
                                    {
                                        WithHolding = 0;
                                    }
                                    else
                                    {
                                        decimal WithHoldingTax = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                        WithHolding = WithHolding + WithHoldingTax;
                                    }
                                }

                            }
                        }
                        beforeVat = beforeVat - VatBuyNotYetDue;

                        if (IsTaxAvailable())
                        {
                            if (lstPayment != null && lstPayment.Count > 0)
                            {
                                for (int i = 0; i <= lstPayment.Count - 1; i++)
                                {
                                    decimal decVat = 0;
                                    decimal GrossAmt = (Convert.ToDecimal(lstPayment[i].PVM_INV_AMOUNT_NET_TC.Value) * Convert.ToDecimal(lstPayment[i].PVM_EXCHG_RATE.Value));
                                    decimal PaymentAmt = lstPayment[i].PVM_PAID_AMOUNT_TC.Value;
                                    decimal tax = Convert.ToDecimal(lstPayment[i].PVM_IVH_TAX_TC.Value) * Convert.ToDecimal(lstPayment[i].PVM_EXCHG_RATE.Value);
                                    if (GrossAmt != 0)
                                    {
                                        decVat = (tax / GrossAmt) * PaymentAmt;
                                    }
                                    Vat = Vat + decVat;
                                }
                                beforeVat = beforeVat - Vat;
                            }
                        }
                    }
                    if (RptType == ApplicationType.VPJ || RptType == ApplicationType.VTYPEVPJ || RptType == ApplicationType.VPTJ || RptType == ApplicationType.EIPJ || RptType == ApplicationType.EIPTJ || (RptType == ApplicationType.SIPJ && RptSubType == 0))
                    {
                        decimal TotalAmt = 0;
                        decimal lineAmt = 0;
                        if (lstPayment != null && lstPayment.Count > 0)
                        {
                            for (int i = 0; i <= lstPayment.Count - 1; i++)
                            {
                                if (lstPayment[i].PVM_PAID_AMOUNT_TC.Value != 0)
                                {
                                    lineAmt = lstPayment[i].PVM_PAID_AMOUNT_TC.Value;
                                    TotalAmt = TotalAmt + lineAmt;
                                }
                            }
                        }
                        parameters = new ReportParameter("TotalAmt", TotalAmt.ToString());
                        locRpt.SetParameters(parameters);
                    }
                    parameters = new ReportParameter("VatBuy", Vat.ToString());
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("beforeVat", beforeVat.ToString());
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("WithHolding", WithHolding.ToString());
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("NetAmount", NetAmount.ToString());
                    locRpt.SetParameters(parameters);
                }
                #endregion
                #region DPVJ,PCVJ,CTVJ
                if ((RptType == ApplicationType.DPVJ || RptType == ApplicationType.CTVJ || RptType == ApplicationType.PCVJ || RptType == ApplicationType.PCRVJ || RptType == ApplicationType.SIPJ) && RptSubType == 0 || RptType == ApplicationType.VPJ || RptType == ApplicationType.VPTJ || RptType == ApplicationType.EIPJ || RptType == ApplicationType.EIPTJ)
                {
                    List<SPFIN_TRX_VOUCHER_RPT_Result> lstDPVData = currentEntity.SPFIN_TRX_VOUCHER_RPT(RptType, RecPK, VoucherVersion).ToList();
                    string PaidBy = string.Empty;
                    if (lstDPVData.Count > 0)
                    {
                        for (int i = 0; i < lstDPVData.Count; i++)
                        {
                            if (lstDPVData[i].FTR_INSTR_NO != null && lstDPVData[i].FTR_INSTR_NO != string.Empty)
                            {
                                string strPaidBy = lstDPVData[i].FTR_INSTR_NO + " : " + Convert.ToDateTime(lstDPVData[i].FTR_INSTR_DATE).ToString(Resources.Constants.ReportDateFormat) + " : " + lstDPVData[i].FTR_INSTR_FAVOUR;
                                if (PaidBy == string.Empty)
                                {
                                    PaidBy = strPaidBy;
                                }
                                else
                                {
                                    PaidBy = PaidBy + "\n" + strPaidBy;
                                }
                            }
                        }
                    }
                    if (PaidBy == string.Empty)
                    {
                        PaidBy = "0";
                    }
                    parameters = new ReportParameter("PaidBy", PaidBy.ToString());
                    locRpt.SetParameters(parameters);
                }
                #endregion
                #region AS,SAS
                if (RptType == ApplicationType.AS || RptType == ApplicationType.SAS || RptType == ApplicationType.GLFIN)
                {

                    parameters = new ReportParameter("BalanceCr", BalanceCr.ToString());
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("BalanceDr", BalanceDr.ToString());
                    locRpt.SetParameters(parameters);
                }
                #endregion
                #region GST
                if (RptType == ApplicationType.GST)
                {
                    parameters = new ReportParameter("GSTLogo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoGST"]));
                    locRpt.SetParameters(parameters);
                }
                #endregion
                locRpt.EnableHyperlinks = true;
                if (RptType == ApplicationType.IO || RptType == ApplicationType.SO)
                {
                    parameters = new ReportParameter("hand", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["hand"]));
                    //string str=Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]);
                    locRpt.SetParameters(parameters);
                }
                GetCompanyDetails(null);
                parameters = new ReportParameter("Logo", "file:///" + LogoPath);
                locRpt.SetParameters(parameters);

                if (FromExternal)
                    footer = "Printed by " + EmpName + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                else
                    footer = "Printed by " + currentUser.EmpName + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                parameters = new ReportParameter("FooterText", footer);
                locRpt.SetParameters(parameters);
                string sPath = string.Empty;
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                {
                    sPath = "file:///" + Server.MapPath(Resources.Controls.SignaturePath);
                }
                else
                {
                    sPath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                }
                switch (RptType)
                {
                    case ApplicationType.SO:
                    case ApplicationType.SI:
                    case ApplicationType.SIC:
                    case ApplicationType.CID:
                    case ApplicationType.SHCID:
                    case ApplicationType.DSI:
                    case ApplicationType.SOD:
                    case ApplicationType.DSID:
                        parameters = new ReportParameter("IsTaxForOtherChargeSales", IsTaxForOtherChargeSales.ToString());
                        locRpt.SetParameters(parameters);
                        break;
                    case ApplicationType.PI:
                    case ApplicationType.PO:
                    case ApplicationType.POPG:
                    case ApplicationType.POTR:
                    case ApplicationType.POG:
                    case ApplicationType.POP:
                    case ApplicationType.POT:
                    case ApplicationType.SCWO:
                    case ApplicationType.TPI:
                        parameters = new ReportParameter("IsTaxForOtherChargePurchase", IsTaxForOtherChargePurchase.ToString());
                        locRpt.SetParameters(parameters);
                        break;
                    case ApplicationType.DO:
                    case ApplicationType.DOD:
                        if (RptSubType == 7 || RptSubType == 8)
                        {
                            parameters = new ReportParameter("NetWt", NetWt.ToString());
                            locRpt.SetParameters(parameters);
                            parameters = new ReportParameter("GrWt", GrWt.ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (RptSubType == 6 || RptSubType == 9)
                        {
                            parameters = new ReportParameter("SignaturePath", sPath);
                            locRpt.SetParameters(parameters);
                        }
                        break;
                    case ApplicationType.DPVJ:
                    case ApplicationType.VTYPEDPVJ:
                    case ApplicationType.VTYPEVPJ:
                    case ApplicationType.VTYPEPCVJ:
                    case ApplicationType.VPJ:
                    case ApplicationType.PCVJ:
                    case ApplicationType.EIPJ:
                    case ApplicationType.SIPJ:
                    case ApplicationType.PR:
                    case ApplicationType.PRT:
                    case ApplicationType.EIPTJ:
                    case ApplicationType.VPTJ:
                    case ApplicationType.CTVJ:
                    case ApplicationType.MTR:
                    case ApplicationType.PCRVJ:
                    case ApplicationType.MTI:
                        //case ApplicationType.MIJ:
                        if (RptSubType != 4)
                        {
                            parameters = new ReportParameter("SignaturePath", sPath);
                            locRpt.SetParameters(parameters);
                        }
                        break;
                    case ApplicationType.MCR:
                        int BinDecimalDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "NumberDecimalDigitBin")["ACF_VALUE"].ToString());
                        for (int i = 0; i < BinDecimalDigit; i++)
                        {
                            NumberDecimalDigitsBin += "0";
                        }
                        parameters = new ReportParameter("BinDecimalDigit", NumberDecimalDigitsBin);
                        locRpt.SetParameters(parameters);
                        break;
                }

                locRpt.EnableExternalImages = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GetClientCode()
        {
            CommonService commonServiceObj = new CommonService();
            ADM_APP_CONFIG_MST admAppConfigMstObj;
            List<ADM_APP_CONFIG_MST> admAppConfigMstList;
            admAppConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_CONFIG_MST>();
            admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
            admAppConfigMstObj.ACF_SETTING = "CLIENT CODE";
            admAppConfigMstList = commonServiceObj.GetADM_APP_CONFIG_MST(admAppConfigMstObj);
            clientCode = admAppConfigMstList.FirstOrDefault().ACF_DATA;
        }
        /// <summary>
        /// To Get Company Details
        /// </summary>
        private ReportDataSource GetCompanyDetails()
        {
            ReportDataSource CompanyDtls = null;
            currentEntity = new ERPEntities();
            List<SPADM_COMPANY_MST_GET_KV_Result> CompanyList = currentEntity.SPADM_COMPANY_MST_GET_KV(CompanyPK, Convert.ToByte(DbActiveStatus.HASPK), null, null, null).ToList();

            if (CompanyList != null && CompanyList.Count > 0)
            {
                if (Convert.ToString(CompanyList[0].CMP_LOGO) != string.Empty)
                {
                    bool fileExists = false;
                    //string LogoPath = string.Empty;
                    // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN); 
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        if (File.Exists(Server.MapPath(Resources.Controls.LogoPath) + CompanyList[0].CMP_LOGO))
                        {
                            LogoPath = "file:///" + Server.MapPath(Resources.Controls.LogoPath) + CompanyList[0].CMP_LOGO;
                            fileExists = true;
                        }
                    }
                    else
                    {
                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_LOGO))
                        {
                            LogoPath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_LOGO;
                            fileExists = true;
                        }
                    }
                    if (!fileExists)
                    {
                        LogoPath = string.Empty;
                    }
                    CompanyList[0].CMP_LOGO = LogoPath;
                }
            }
            CompanyDtls = new ReportDataSource("CompanyDtls", CompanyList);
            return CompanyDtls;
        }
        /// <summary>
        /// To Get Company Details
        /// </summary>
        private ReportDataSource GetCompanyDetails(int? CmpnyPk = null)
        {
            ReportDataSource CompanyDtls = null;
            currentEntity = new ERPEntities();
            List<SPADM_COMPANY_MST_GET_KV_Result> CompanyList = currentEntity.SPADM_COMPANY_MST_GET_KV(CmpnyPk, Convert.ToByte(DbActiveStatus.HASPK), null, null, null).ToList();

            if (CompanyList != null && CompanyList.Count > 0)
            {
                if (Convert.ToString(CompanyList[0].CMP_LOGO) != string.Empty)
                {
                    bool fileExists = false;
                    //string LogoPath = string.Empty;
                    // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN); 
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        if (File.Exists(Server.MapPath(Resources.Controls.CompanyLogo) + CompanyList[0].CMP_LOGO))
                        {
                            LogoPath = Server.MapPath(Resources.Controls.CompanyLogo) + CompanyList[0].CMP_LOGO;
                            fileExists = true;
                        }
                    }
                    else
                    {
                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_LOGO))
                        {
                            LogoPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_LOGO;
                            fileExists = true;
                        }
                    }
                    if (!fileExists)
                    {
                        LogoPath = string.Empty;
                    }
                    CompanyList[0].CMP_LOGO = "file:///" + LogoPath;
                }
            }
            if (Convert.ToString(CompanyList[0].CMP_OP_LOGO) != string.Empty)
            {
                bool OutputFileExists = false;
                OutputLogoPath = string.Empty;
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                {
                    if (File.Exists(Server.MapPath(Resources.Controls.CompanyLogo) + CompanyList[0].CMP_OP_LOGO))
                    {
                        OutputLogoPath = Server.MapPath(Resources.Controls.CompanyLogo) + CompanyList[0].CMP_OP_LOGO;
                        OutputFileExists = true;
                    }
                }
                else
                {
                    if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_OP_LOGO))
                    {
                        OutputLogoPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_OP_LOGO;
                        OutputFileExists = true;
                    }
                }
                if (!OutputFileExists)
                {
                    OutputLogoPath = string.Empty;
                }
                CompanyList[0].CMP_OP_LOGO = "file:///" + OutputLogoPath;
            }
            CompanyDtls = new ReportDataSource("CompanyDtls", CompanyList);
            return CompanyDtls;
        }
        /// <summary>
        /// To Get Settings Details
        /// </summary>
        private ReportDataSource GetSettingDetails()
        {
            ReportDataSource Settings = null;
            DataTable dtSettings = new DataTable();
            dtSettings.Columns.Add("DateFormat", typeof(string));
            dtSettings.Columns.Add("CurrencyFormat", typeof(string));
            dtSettings.Columns.Add("NumberFormat", typeof(string));
            dtSettings.Columns.Add("ProductRateFormat", typeof(string));
            dtSettings.Columns.Add("ExchRateFormat", typeof(string));
            string currencyformat = "0.";
            string NoFormat = "0.";
            string ProductRateFormat = "0.";
            string ExchRateFormat = "0.";
            string currencydecimals = string.Empty;
            string Nodecimal = string.Empty;
            string ProductRatedecimals = string.Empty;
            string ExchRatedecimal = string.Empty;
            int curdigit = Convert.ToInt32(Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            for (int i = 0; i < curdigit; i++)
            {
                currencydecimals += "0";
            }

            int NoDigit = Convert.ToInt32(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
            for (int i = 0; i < NoDigit; i++)
            {
                Nodecimal += "0";
            }
            int RateDigit = Session[ERP.Utilities.SessionStrings.RateDecimalDigit] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]) : 1;
            for (int i = 0; i < RateDigit; i++)
            {
                ProductRatedecimals += "0";
            }
            int exchRateDigit = Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]) : 1;
            for (int i = 0; i < exchRateDigit; i++)
            {
                ExchRatedecimal += "0";
            }

            currencyformat = currencyformat + currencydecimals;
            NoFormat = NoFormat + Nodecimal;
            ProductRateFormat = ProductRateFormat + ProductRatedecimals;
            ExchRateFormat = ExchRateFormat + ExchRatedecimal;
            dtSettings.Rows.Add(Resources.Constants.ReportDateFormat, currencyformat, NoFormat, ProductRateFormat, ExchRateFormat);
            dtSettings.AcceptChanges();
            Settings = new ReportDataSource("Settings", dtSettings);
            return Settings;

        }
        /// <summary>
        /// Generates Purchase Requisition Report
        /// </summary>
        /// <returns></returns>
        private void GeneratetPurchaseRequisitionRpt()
        {
            try
            {
                currentEntity = new ERPEntities();
                rvViewReport.Visible = true;
                ReportDataSource dsReportDet;
                LocalReport locRpt;
                locRpt = null;
                rvViewReport.LocalReport.DataSources.Clear();
                rvViewReport.Visible = true;
                DataTable dtReptDtls;
                dtReptDtls = new DataTable();
                dsReportDet = new ReportDataSource("PVHeader", currentEntity.SPFIN_PAYMENT_VND_VOUCHER_RPT_TEST(1));
                locRpt = rvViewReport.LocalReport;
                locRpt.ReportPath = string.Empty;
                locRpt.ReportPath = Server.MapPath("RptTemplateWithLogo.rdlc");
                locRpt.EnableHyperlinks = true;
                locRpt.EnableExternalImages = true;
                rvViewReport.LocalReport.DataSources.Add(dsReportDet);
                rvViewReport.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                rvViewReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                rvViewReport.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
        public void PrintPDF()
        {
            SavePDF(locRpt);
            if (File.Exists(attachmentFilePath))
            {
                Response.ClearContent();
                Response.ContentType = "application/pdf";
                string redirectUrl = Resources.PageURL.PDFUrl + attachmentFileName;
                Response.Redirect(redirectUrl);
                Response.Flush();
            }
        }


        /// <summary>
        /// Method for Tree Binding
        /// </summary>
        public void BindTree()
        {
            AccountMstService AccountMstService;
            AccountMstService = null;
            CommonService commonService;
            commonService = null;
            try
            {
                TreeNode PRoot;
                TreeNode root;
                switch (RptType)
                {
                    case ApplicationType.AS:
                    case ApplicationType.GLFIN:
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        List<FIN_COA_MST> accountMstList = null;
                        FIN_COA_MST accountMstObj;
                        ServiceUtility serviceUtilityObj;
                        AccountMstService = new AccountMstService();
                        AccountMstService = ERP.Utilities.CommonFunctions.InitiateClient(AccountMstService);
                        accountMstObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_MST>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.AccountSequence;
                        serviceUtilityObj.SortDirection = Resources.ErpRes.SortAscending;
                        serviceUtilityObj.Location = ddlLocation.SelectedValue;
                        accountMstObj.COA_PK = 0;//CurrPK
                        accountMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        accountMstObj.COA_BIZUNIT = Convert.ToInt32(currentUser.SBUID);
                        //int plantPK = 0;
                        //plantPK = Convert.ToInt32(ddlLocation.SelectedValue);
                        if (Convert.ToInt32(ddlLocation.SelectedValue) > -1)
                        {
                            if (RptSubType == 3)
                            {
                                accountMstList = AccountMstService.GetFinCoaMstforGL(accountMstObj, serviceUtilityObj);
                            }
                            else
                            {
                                accountMstList = AccountMstService.GetFinCoaMst(accountMstObj, serviceUtilityObj);
                            }
                        }

                        trvAccounts.Nodes.Clear();
                        if (accountMstList != null)
                        {
                            List<FIN_COA_MST> acntList = (from accountList in accountMstList
                                                          where accountList.COA_LEVEL == 1
                                                          select accountList).ToList();
                            PRoot = new TreeNode();
                            PRoot.Text = GetLocalResourceObject("LedgerRootName").ToString();
                            PRoot.Value = "0";
                            PRoot.SelectAction = TreeNodeSelectAction.None;
                            foreach (FIN_COA_MST accounts in acntList)
                            {
                                root = new TreeNode();
                                root.Text = accounts.COA_NAME;
                                root.Value = accounts.COA_PK.ToString();
                                root.SelectAction = TreeNodeSelectAction.None;
                                CreateNode(root, accountMstList);
                                PRoot.ChildNodes.Add(root);
                                //trvAccounts.Nodes.Add(root);
                            }
                            trvAccounts.Nodes.Add(PRoot);
                        }
                        break;
                    case ApplicationType.BRC:
                    case ApplicationType.SAS:
                        FIN_COA_SUB_TYPE_CFG finCoaSubTypeCfgObj;
                        List<FIN_COA_SUB_TYPE_CFG> finCoaSubTypeCfgList;
                        string relquery;
                        currentEntity = new ERPEntities();
                        commonService = new CommonService();
                        finCoaSubTypeCfgObj = new FIN_COA_SUB_TYPE_CFG();
                        commonService = CommonFunctions.InitiateClient(commonService);
                        rvViewReport.Visible = false;
                        finCoaSubTypeCfgObj = CommonFunctions.Initilize<FIN_COA_SUB_TYPE_CFG>();
                        finCoaSubTypeCfgObj.CST_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finCoaSubTypeCfgObj.CST_PK = Convert.ToInt32(ddlSubLedger.SelectedValue);
                        finCoaSubTypeCfgList = commonService.GetSubTypeCfgValues(finCoaSubTypeCfgObj);
                        if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                        {
                            relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
                            relquery = relquery.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                            var ddlValues = ERP.Utilities.CommonFunctions.GetResults(new DDLMaster(), currentEntity, relquery);
                            TreeNode childNode;

                            trvAccounts.Nodes.Clear();
                            root = new TreeNode();
                            root.Text = Convert.ToString(ddlSubLedger.SelectedItem);
                            root.Value = Convert.ToString(ddlSubLedger.SelectedValue);
                            foreach (DDLMaster ddlMstr in ddlValues)
                            {
                                childNode = new TreeNode();
                                childNode.Text = ddlMstr.Value;
                                childNode.Value = ddlMstr.PK.ToString();
                                childNode.SelectAction = TreeNodeSelectAction.Select;
                                root.ChildNodes.Add(childNode);
                            }
                            root.SelectAction = TreeNodeSelectAction.None;
                            trvAccounts.Nodes.Add(root);
                        }
                        else
                        {
                            trvAccounts.Nodes.Clear();
                        }
                        break;
                    case ApplicationType.PSAS:
                        trvAccounts.Nodes.Clear();
                        root = new TreeNode();
                        root.Text = Convert.ToString(txtSendTo.Text);
                        root.Value = Convert.ToString(hdfSendTo.Value);
                        if (hdfSendTo.Value == "1")
                        {
                            ddlParty.Items.Clear();
                            DataSet dsCustomer = CustomerProduct.GetCustomer(0, string.Empty, currentUser.SBUID, (int?)null, Convert.ToInt32(ddlInvoType.SelectedValue));
                            TreeNode childNode;
                            ddlParty.DataSource = dsCustomer;
                            ddlParty.DataValueField = "CUS_PK";
                            ddlParty.DataTextField = "CUS_NAME";
                            ddlParty.DataBind();
                            ddlParty.Items.HtmlDecode();
                            ddlParty.Items.Insert(0, new System.Web.UI.WebControls.ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                            //foreach (DataRow dr in dsCustomer.Tables[0].Rows)
                            //{
                            //    childNode = new TreeNode();
                            //    childNode.Text = dr["CUS_NAME"].ToString();
                            //    childNode.Value = dr["CUS_PK"].ToString();
                            //    childNode.SelectAction = TreeNodeSelectAction.Select;
                            //    root.ChildNodes.Add(childNode);
                            //}
                            //root.SelectAction = TreeNodeSelectAction.None;
                            //trvAccounts.Nodes.Add(root);
                            var searchResult = (GetCheckListDataGet(dsCustomer.Tables[0], "CUS_PK", "CUS_NAME"));

                            CheckListSearchControl1.ListData = searchResult;
                            CheckListSearchControl1.BindData();


                        }
                        else if (hdfSendTo.Value == "2")
                        {
                            ddlParty.Items.Clear();
                            DataTable dtVendor = VendorMaster.GetVendorforReport(currentUser.SBUID, Convert.ToInt32(ddlInvoType.SelectedValue), (int?)null);
                            TreeNode childNode;
                            ddlParty.DataSource = dtVendor;
                            ddlParty.DataValueField = "VEN_PK";
                            ddlParty.DataTextField = "VEN_NAME";
                            ddlParty.DataBind();
                            ddlParty.Items.Insert(0, new System.Web.UI.WebControls.ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                            //foreach (DataRow dr in dtVendor.Rows)
                            //{
                            //    childNode = new TreeNode();
                            //    childNode.Text = dr["VEN_NAME"].ToString();
                            //    childNode.Value = dr["VEN_PK"].ToString();
                            //    childNode.SelectAction = TreeNodeSelectAction.Select;
                            //    root.ChildNodes.Add(childNode);

                            //}
                            //root.SelectAction = TreeNodeSelectAction.None;
                            //trvAccounts.Nodes.Add(root);
                            var searchResult = (GetCheckListDataGet(dtVendor, "VEN_PK", "VEN_NAME"));

                            CheckListSearchControl1.ListData = searchResult;
                            CheckListSearchControl1.BindData();
                        }

                        break;
                }

                lblOr.Visible = false;
                if (trvAccounts.Nodes.Count > 0)
                    lblOr.Visible = true;

                if (divchecklist.Visible)
                    lblOr.Visible = true;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
                AccountMstService = null;
                commonService = null;
            }
        }
        private void BindDropDown(string type)
        {
            switch (type)
            {
                case ApplicationType.LOCATION:
                    ddlLocation.Items.Clear();
                    if (dtLocation != null && dtLocation.Rows.Count > 0)
                    {
                        ddlLocation.DataValueField = GTIService.Constants.Designation.Fields.PK;
                        ddlLocation.DataTextField = GTIService.Constants.Designation.Fields.VALUE;
                        ddlLocation.DataSource = dtLocation;
                        ddlLocation.DataBind();
                    }
                    if (hdfIsMultiplePlant.Value == "1")
                    {
                        ddlLocation.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        ddlLocation.Items.Insert(1, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECT_VALUE_ZERO));
                    }
                    else
                        ddlLocation.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlLocation.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }

                    break;

            }

        }
        private void CustomerInvtype()
        {
            dtSOData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO TYPE");
            ddlInvoType.Items.Clear();
            if (dtSOData != null)
            {
                ddlInvoType.DataSource = dtSOData;
                ddlInvoType.DataTextField = "CFG_DATA";
                ddlInvoType.DataValueField = "CFG_PK";
                ddlInvoType.DataBind();
            }
            ddlInvoType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));


            ddlParty.Items.Clear();
        }
        private void VendorInvtype()
        {
            dtSOData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PURCHASE INVOICE TYPE");
            ddlInvoType.Items.Clear();
            if (dtSOData != null)
            {
                ddlInvoType.DataSource = dtSOData;
                ddlInvoType.DataTextField = "CFG_DATA";
                ddlInvoType.DataValueField = "CFG_VALUE";
                ddlInvoType.DataBind();
            }
            ddlInvoType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
        }
        public void CreateNode(TreeNode node, List<FIN_COA_MST> acntMstList)
        {
            TreeNode childNode;

            List<FIN_COA_MST> acntList = (from accountList in acntMstList
                                          where accountList.COA_PARENT == Convert.ToInt32(node.Value)
                                          select accountList).ToList();

            if (acntList.Count == 0) { return; }

            foreach (FIN_COA_MST accounts in acntList)
            {
                childNode = new TreeNode();
                childNode.Text = accounts.COA_NAME;
                childNode.Value = accounts.COA_PK.ToString();
                childNode.SelectAction = TreeNodeSelectAction.Select;
                node.ChildNodes.Add(childNode);
                CreateNode(childNode, acntMstList);
            }
        }

        public void GetSelectedAccounts()
        {
            DataRow dr;
            string emptyXml;
            currentEntity = new ERPEntities();
            SelectedAccountsList = new DataSet();
            switch (RptType)
            {
                case ApplicationType.AS:
                    emptyXml = "<ROOT><ACCHEAD><FROM_DATE/><TO_DATE/><BIZUNIT/><CURRENCY/></ACCHEAD><ACCOUNT><COA_PK/></ACCOUNT></ROOT>";

                    SelectedAccountsList.ReadXml(new System.IO.StringReader(emptyXml));
                    SelectedAccountsList.Tables["ACCHEAD"].Rows[0].Delete();
                    SelectedAccountsList.Tables["ACCOUNT"].Rows[0].Delete();

                    if (hdfAccount.Value != "0" && hdfAccount.Value != string.Empty && txtAccount.Text.Trim() != this.GetGlobalResourceObject("Messages", "AutoDefaultValue").ToString())
                    {
                        dr = SelectedAccountsList.Tables["ACCOUNT"].NewRow();
                        dr["COA_PK"] = hdfAccount.Value;
                        SelectedAccountsList.Tables["ACCOUNT"].Rows.Add(dr);
                    }
                    else
                    {
                        foreach (TreeNode node in trvAccounts.CheckedNodes)
                        {
                            dr = SelectedAccountsList.Tables["ACCOUNT"].NewRow();
                            dr["COA_PK"] = node.Value;
                            SelectedAccountsList.Tables["ACCOUNT"].Rows.Add(dr);
                        }
                    }
                    break;
                case ApplicationType.GLFIN:
                    emptyXml = "<ROOT><ACCHEAD><FROM_DATE/><TO_DATE/><BIZUNIT/><CURRENCY/><FINYEAR/></ACCHEAD><ACCOUNT><COA_PK/></ACCOUNT></ROOT>";

                    SelectedAccountsList.ReadXml(new System.IO.StringReader(emptyXml));
                    SelectedAccountsList.Tables["ACCHEAD"].Rows[0].Delete();
                    SelectedAccountsList.Tables["ACCOUNT"].Rows[0].Delete();

                    if (hdfAccount.Value != "0" && hdfAccount.Value != string.Empty && txtAccount.Text.Trim() != this.GetGlobalResourceObject("Messages", "AutoDefaultValue").ToString())
                    {
                        dr = SelectedAccountsList.Tables["ACCOUNT"].NewRow();
                        dr["COA_PK"] = hdfAccount.Value;
                        SelectedAccountsList.Tables["ACCOUNT"].Rows.Add(dr);
                    }
                    else
                    {
                        foreach (TreeNode node in trvAccounts.CheckedNodes)
                        {
                            dr = SelectedAccountsList.Tables["ACCOUNT"].NewRow();
                            dr["COA_PK"] = node.Value;
                            SelectedAccountsList.Tables["ACCOUNT"].Rows.Add(dr);
                        }
                    }
                    break;
                case ApplicationType.COL:
                    emptyXml = "<ROOT><ACCHEAD><FROM_DATE/><TO_DATE/><BIZUNIT/><CURRENCY/></ACCHEAD><ACCOUNT><COA_PK/></ACCOUNT></ROOT>";

                    SelectedAccountsList.ReadXml(new System.IO.StringReader(emptyXml));
                    SelectedAccountsList.Tables["ACCHEAD"].Rows[0].Delete();
                    SelectedAccountsList.Tables["ACCOUNT"].Rows[0].Delete();

                    if (hdfAccount.Value != "0" && hdfAccount.Value != string.Empty && txtAccount.Text.Trim() != this.GetGlobalResourceObject("Messages", "AutoDefaultValue").ToString())
                    {
                        dr = SelectedAccountsList.Tables["ACCOUNT"].NewRow();
                        dr["COA_PK"] = hdfAccount.Value;
                        SelectedAccountsList.Tables["ACCOUNT"].Rows.Add(dr);
                    }
                    else
                    {
                        foreach (TreeNode node in trvAccounts.CheckedNodes)
                        {
                            dr = SelectedAccountsList.Tables["ACCOUNT"].NewRow();
                            dr["COA_PK"] = node.Value;
                            SelectedAccountsList.Tables["ACCOUNT"].Rows.Add(dr);
                        }
                    }
                    break;
                case ApplicationType.BRC:
                case ApplicationType.SAS:
                    emptyXml = "<ROOT><ACCHEAD><FROM_DATE/><TO_DATE/><BIZUNIT/><CURRENCY/><CST_PK/></ACCHEAD><SUBACCOUNT><SAC_PK/></SUBACCOUNT>/</ROOT>";
                    SelectedAccountsList.ReadXml(new System.IO.StringReader(emptyXml));
                    SelectedAccountsList.Tables["ACCHEAD"].Rows[0].Delete();
                    SelectedAccountsList.Tables["SUBACCOUNT"].Rows[0].Delete();

                    if (hdfSubAccount.Value != "0" && hdfSubAccount.Value != string.Empty)
                    {
                        dr = SelectedAccountsList.Tables["SUBACCOUNT"].NewRow();
                        dr["SAC_PK"] = hdfSubAccount.Value;
                        SelectedAccountsList.Tables["SUBACCOUNT"].Rows.Add(dr);
                    }
                    else
                    {
                        foreach (TreeNode node in trvAccounts.CheckedNodes)
                        {
                            if (node.ChildNodes.Count == 0)//To avoid Parent Account PK 
                            {
                                dr = SelectedAccountsList.Tables["SUBACCOUNT"].NewRow();
                                dr["SAC_PK"] = node.Value;
                                SelectedAccountsList.Tables["SUBACCOUNT"].Rows.Add(dr);
                            }
                        }
                    }
                    break;
                case ApplicationType.PSAS:
                    emptyXml = "<ROOT><ACCHEAD><FROM_DATE/><TO_DATE/><BIZUNIT/><CURRENCY/><PARTY_TYPE/></ACCHEAD><SUBACCOUNT><SAC_PK/></SUBACCOUNT>/</ROOT>";
                    SelectedAccountsList.ReadXml(new System.IO.StringReader(emptyXml));
                    SelectedAccountsList.Tables["ACCHEAD"].Rows[0].Delete();
                    SelectedAccountsList.Tables["SUBACCOUNT"].Rows[0].Delete();
                    hdfParty.Value = ddlParty.SelectedValue;    //ddlParty.SelectedItem.Value;
                    checkedListCheckedItems = CheckListSearchControl1.GetCheckedItems();
                    if (hdfParty.Value != "0" && hdfParty.Value != string.Empty && hdfParty.Value != "-1")
                    {
                        dr = SelectedAccountsList.Tables["SUBACCOUNT"].NewRow();
                        dr["SAC_PK"] = hdfParty.Value;
                        SelectedAccountsList.Tables["SUBACCOUNT"].Rows.Add(dr);
                    }
                    else
                    {
                        if (checkedListCheckedItems != null && checkedListCheckedItems.Count > 0)
                        {
                            foreach (ListItem item in checkedListCheckedItems)
                            {
                                dr = SelectedAccountsList.Tables["SUBACCOUNT"].NewRow();
                                dr["SAC_PK"] = item.Value;
                                SelectedAccountsList.Tables["SUBACCOUNT"].Rows.Add(dr);
                            }
                        }
                    }
                    break;
            }

        }
        private bool IsTaxAvailable()
        {
            if (lstVoucher != null && lstVoucher.Count > 0 && lstPayment != null && lstPayment.Count > 0)
            {
                var query = lstVoucher.AsEnumerable().Where(x => x.FTR_ACC_SUB_TYPE == (int)AccSubType.VatBuy);
                if (query.Any())
                {
                    return false;
                }
                else
                {
                    var Payment = lstPayment.AsEnumerable().Where(x => x.PVM_IVH_TAX_TC > 0);
                    if (Payment.Any())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool IsTreeNodeChecked()
        {
            foreach (TreeNode node in trvAccounts.Nodes)
            {
                if (node.Checked == true)
                {
                    return true;
                }
                foreach (TreeNode childNode in node.ChildNodes)
                {
                    if (childNode.Checked == true)
                    {
                        return true;
                    }
                }

            }
            //foreach(ChildNode node in trvAccounts.Nodes.chil
            return false;
        }
        private bool IsPurchaseOrderWithOutTax()
        {
            if (dsPurchaseRequest != null && dsPurchaseRequest.Tables.Count > 0)
            {
                decimal Tax = Convert.ToDecimal(dsPurchaseRequest.Tables[1].Rows[0]["POD_TAX"].ToString());
                decimal Discount = Convert.ToDecimal(dsPurchaseRequest.Tables[1].Rows[0]["POD_DISC_AMT"].ToString());
                decimal Total = Tax + Discount;
                if (Total > 0)
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsInvoiceWithTax()
        {
            DataTable dtInvDtls;
            switch (RptType)
            {
                case ApplicationType.PI:
                case ApplicationType.SI:
                case ApplicationType.SIC:
                case ApplicationType.DSI:
                case ApplicationType.CID:
                case ApplicationType.SHCID:
                case ApplicationType.DSID:
                case ApplicationType.TPI:
                    if (dsDelivaryOrder != null && dsDelivaryOrder.Tables.Count > 0)
                    {
                        dtInvDtls = dsDelivaryOrder.Tables[1];
                        decimal Tax = dtInvDtls.AsEnumerable().Sum(x => x.Field<decimal>("DPD_TAX"));
                        decimal Discount = dtInvDtls.AsEnumerable().Sum(x => x.Field<decimal>("DPD_DISCOUNT"));
                        decimal Total = Tax + Discount;
                        if (Total > 0)
                        {
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }
        /// <summary>
        /// Method for Type & Category Dropdowns
        /// </summary>
        public void BindSubLedger()
        {
            CommonService CommonServiceClient;
            FIN_COA_SUB_TYPE_CFG finCoaSubTypeCfgObj;
            List<FIN_COA_SUB_TYPE_CFG> finCoaSubTypeCfgList;
            CommonServiceClient = new CommonService();

            finCoaSubTypeCfgObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_SUB_TYPE_CFG>();
            finCoaSubTypeCfgObj.CST_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
            finCoaSubTypeCfgObj.CST_PK = -1;
            finCoaSubTypeCfgList = CommonServiceClient.GetSubTypeCfgValues(finCoaSubTypeCfgObj);
            ddlSubLedger.Items.Clear();
            if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
            {
                ddlSubLedger.DataSource = finCoaSubTypeCfgList;
                ddlSubLedger.DataTextField = Resources.DataFieldRes.CoaSubTypeName;
                ddlSubLedger.DataValueField = Resources.DataFieldRes.CoaSubTypePK;
                ddlSubLedger.DataBind();
            }
            ddlSubLedger.Items.Insert(0, new System.Web.UI.WebControls.ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            ddlSubLedger.Items.Remove(ddlSubLedger.Items.FindByText(Resources.Report.SelectAll));
        }
        public void BindFinYear()
        {
            DataTable dtResult = BusinessLogic.ReportsManagement.GenerateReportBL.GetFinYear(currentUser.SBUID);
            ddlFinYear.Items.Clear();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                ddlFinYear.DataSource = dtResult;
                ddlFinYear.DataTextField = "Value";
                ddlFinYear.DataValueField = "PK";
                ddlFinYear.DataBind();
            }
            //ddlFinYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            //ddlFinYear.SelectedIndex = Convert.ToInt32(ddlFinYear.Items.IndexOf(ddlFinYear.Items.FindByValue(Convert.ToString(DateTime.Now.Year))));
        }
        public void GetInitialDate()
        {
            List<FIN_YEAR_MST> lstFinYear = new List<FIN_YEAR_MST>();
            FinTrxService finTrxService = new FinTrxService();
            lstFinYear = finTrxService.GetCurrentFinPeriod(DateTime.Now, currentUser.SBUID);
            string fromDate;
            string toDate;
            if (lstFinYear.Count > 0)
            {
                fromDate = lstFinYear[0].FYR_DATE_FROM.ToString(Resources.Constants.DateTimeFormat);
            }
            else
                fromDate = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            toDate = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            txtFromDate.Text = fromDate;
            txtToDate.Text = toDate;
        }
        /// <summary>
        /// To validate auto complete hidden fields
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            try
            {
                bool flag;
                string errMsg;
                flag = true;
                errMsg = string.Empty;

                switch (RptType)
                {
                    case ApplicationType.TB:
                        if (txtFromDate.Text.Trim() == string.Empty)
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_FromDate").ToString() : "^" + this.GetLocalResourceObject("Err_FromDate").ToString();
                            flag = false;
                        }
                        if (txtToDate.Text.Trim() == string.Empty)
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_ToDate").ToString() : "^" + this.GetLocalResourceObject("Err_ToDate").ToString();
                            flag = false;
                        }
                        break;
                    case ApplicationType.AS:
                    case ApplicationType.GLFIN:
                        if (usrDateFilter.Visible)
                        {
                            if (usrDateFilter.FromDate == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_FromDate").ToString() : "^" + this.GetLocalResourceObject("Err_FromDate").ToString();
                                flag = false;
                            }
                            if (usrDateFilter.ToDate == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_ToDate").ToString() : "^" + this.GetLocalResourceObject("Err_ToDate").ToString();
                                flag = false;
                            }
                        }
                        else
                        {
                            if (txtFromDate.Text.Trim() == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_FromDate").ToString() : "^" + this.GetLocalResourceObject("Err_FromDate").ToString();
                                flag = false;
                            }
                            if (txtToDate.Text.Trim() == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_ToDate").ToString() : "^" + this.GetLocalResourceObject("Err_ToDate").ToString();
                                flag = false;
                            }
                        }
                        if ((hdfAccount.Value.Trim() == string.Empty || txtAccount.Text == this.GetGlobalResourceObject("Messages", "AutoDefaultValue").ToString()) && trvAccounts.CheckedNodes.Count == 0)
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_Account").ToString() : "^" + this.GetLocalResourceObject("Err_Account").ToString();
                            flag = false;
                        }

                        break;
                    case ApplicationType.BRC:
                        if (txtFromDate.Text.Trim() == string.Empty)
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_FromDate").ToString() : "^" + this.GetLocalResourceObject("Err_FromDate").ToString();
                            flag = false;
                        }
                        if (txtToDate.Text.Trim() == string.Empty)
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_ToDate").ToString() : "^" + this.GetLocalResourceObject("Err_ToDate").ToString();
                            flag = false;
                        }
                        if ((txtSubAccount.Text == this.GetGlobalResourceObject("Messages", "AutoDefaultValue").ToString()) && trvAccounts.CheckedNodes.Count == 0)
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_Account").ToString() : "^" + this.GetLocalResourceObject("Err_Account").ToString();
                            flag = false;
                        }

                        break;
                    case ApplicationType.PSAS:
                        if (usrDateFilter.Visible)
                        {
                            if (usrDateFilter.FromDate == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_FromDate").ToString() : "^" + this.GetLocalResourceObject("Err_FromDate").ToString();
                                flag = false;
                            }
                            if (usrDateFilter.ToDate == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_ToDate").ToString() : "^" + this.GetLocalResourceObject("Err_ToDate").ToString();
                                flag = false;
                            }
                        }
                        else
                        {
                            if (txtFromDate.Text.Trim() == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_FromDate").ToString() : "^" + this.GetLocalResourceObject("Err_FromDate").ToString();
                                flag = false;
                            }
                            if (txtToDate.Text.Trim() == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_ToDate").ToString() : "^" + this.GetLocalResourceObject("Err_ToDate").ToString();
                                flag = false;
                            }
                        }
                        if ((ddlParty.SelectedValue == "-1" || ddlParty.SelectedValue == "Select") && checkedListCheckedItems != null && checkedListCheckedItems.Count <= 0)
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_PartyLedger").ToString() : "^" + this.GetLocalResourceObject("Err_PartyLedger").ToString();
                            flag = false;
                        }

                        break;
                    case ApplicationType.SAS:
                        if (usrDateFilter.Visible)
                        {
                            if (usrDateFilter.FromDate == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_FromDate").ToString() : "^" + this.GetLocalResourceObject("Err_FromDate").ToString();
                                flag = false;
                            }
                            if (usrDateFilter.ToDate == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_ToDate").ToString() : "^" + this.GetLocalResourceObject("Err_ToDate").ToString();
                                flag = false;
                            }
                        }
                        else
                        {
                            if (txtFromDate.Text.Trim() == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_FromDate").ToString() : "^" + this.GetLocalResourceObject("Err_FromDate").ToString();
                                flag = false;
                            }
                            if (txtToDate.Text.Trim() == string.Empty)
                            {
                                errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_ToDate").ToString() : "^" + this.GetLocalResourceObject("Err_ToDate").ToString();
                                flag = false;
                            }
                        }
                        if (ddlSubLedger.SelectedValue == "-1")
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_Subledger").ToString() : "^" + this.GetLocalResourceObject("Err_Subledger").ToString();
                            flag = false;
                        }

                        if ((hdfSubAccount.Value.Trim() == string.Empty || txtSubAccount.Text == this.GetGlobalResourceObject("Messages", "AutoDefaultValue").ToString()) && trvAccounts.CheckedNodes.Count == 0)
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_Subledger").ToString() : "^" + this.GetLocalResourceObject("Err_Subledger").ToString();
                            flag = false;
                        }

                        break;
                }
                if (!flag)
                    litErrorMsg.Text = errMsg;
                return flag;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Convert to & save pdf
        /// </summary>
        private void SavePDF(LocalReport locRpt)
        {
            try
            {
                string mimeType;
                string encoding;
                string extension;
                string[] streamids;
                Microsoft.Reporting.WebForms.Warning[] warnings;
                string format;
                string savePath;
                if (FromExternal)
                    savePath = Server.MapPath("~/") + Resources.PageURL.ExteralPdfURL;
                else
                    savePath = Server.MapPath("~/") + Resources.PageURL.OfflineTestDocs;
                attachmentFilePath = string.Empty;
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
                if (FromExternal)
                    attachmentFileName = ExternalPDFName;
                else
                    attachmentFileName = RptType + RptSubType + RecPK + ChequeID + ".pdf";
                attachmentFilePath = savePath + attachmentFileName;
                attachmentFileFormat = ".pdf";
                attachmentFileContentType = "application/pdf";
                //if file is exists delete file
                if (File.Exists(attachmentFilePath))
                    File.Delete(attachmentFilePath);
                // Array.ForEach(Directory.GetFiles(savePath), File.Delete);//Delete all files in a folder
                format = "PDF";
                string deviceInfo = "<DeviceInfo><EmbedFonts>None</EmbedFonts></DeviceInfo>";
                byte[] bytes = locRpt.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
                /* stream to use for attachment - can implement later
                Stream stream = new MemoryStream();
                stream.Write(bytes, 0, bytes.Length);
                SendMail(stream);
                 */
                //save the pdf byte to the folder

                using (FileStream fs = new FileStream(attachmentFilePath, FileMode.OpenOrCreate))
                {
                    byte[] data = new byte[fs.Length];
                    fs.Write(bytes, 0, bytes.Length);

                    fs.Close();
                }
                IsPdfGenerated = true;
            }
            catch (Exception ex)
            {
                IsPdfGenerated = false;
                throw ex;
            }
        }

        private void SaveExcel(LocalReport locRpt)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            string filename;

            byte[] bytes = locRpt.Render(
               "Excel", null, out mimeType, out encoding,
                out extension,
               out streamids, out warnings);

            filename = string.Format("{0}.{1}", "ExportToExcel", "xls");
            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
        /// <summary>
        /// Convert to & Open pdf
        /// </summary>
        private void OpenPDF(LocalReport locRpt)
        {
            try
            {
                string mimeType;
                string encoding;
                string extension;
                string[] streamids;
                Microsoft.Reporting.WebForms.Warning[] warnings;
                string format;

                attachmentFileName = RptType + RptSubType + ".pdf";

                format = "PDF";
                string deviceInfo = "<DeviceInfo><EmbedFonts>None</EmbedFonts></DeviceInfo>";
                byte[] bytes = locRpt.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                //Response.AddHeader("content-disposition", "attachment; filename= " + attachmentFileName);
                Response.AddHeader("content-disposition", "attachment; filename=(None) ");
                Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                Response.Flush(); // send it to the client to download  
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ActionHandler
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                ActionsEnum commonAction;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                    switch (commonAction)
                    {
                        case ActionsEnum.VIEW:
                            GetFieldValues(RptType);
                            if (AppTypeDetailsList == null)
                            {
                                ClearCrystalReport();
                                divReportViewer.Visible = false;
                                divCrystalReportViewer.Visible = false;
                                divNodata.Visible = false;

                            }
                            var reportName = AppTypeDetailsList.Select(l => l.AST_OP_FILE1).ToList();
                            switch (((string[])reportName[0].ToString().Split('.'))[1].Trim().ToLower())
                            {
                                #region crystal report calling
                                case ReportType.CrystalReport:
                                    if (reportDocument != null)
                                    {
                                        ClearCrystalReport();
                                        reportDocument.Close();
                                        reportDocument.Dispose();
                                        reportDocument = null;
                                        GC.Collect();
                                    }
                                    divReportViewer.Visible = true;
                                    divCrystalReportViewer.Visible = true;
                                    GERP_Report.Visible = true;
                                    divNodata.Visible = false;
                                    switch (RptType)
                                    {
                                        case ApplicationType.AS:
                                            divNodata.Visible = false;
                                            if (((Button)sender).CommandName == "SELECTEDACC")
                                            {
                                                GetSelectedAccounts();
                                            }
                                            else
                                            {
                                                if (!ValidateForm())
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                    Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                                                }
                                                else
                                                {
                                                    GetSelectedAccounts();
                                                    SetFieldValuesCrystal(RptType, reportName[0].ToString());
                                                }
                                            }
                                            break;
                                        case ApplicationType.GLFIN:
                                            divNodata.Visible = false;
                                            if (((Button)sender).CommandName == "SELECTEDACC")
                                            {
                                                GetSelectedAccounts();
                                            }
                                            else
                                            {
                                                if (!ValidateForm())
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                    Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                                                }
                                                else
                                                {
                                                    GetSelectedAccounts();
                                                    SetFieldValuesCrystal(RptType, reportName[0].ToString());
                                                }
                                            }
                                            break;
                                        case ApplicationType.SAS:
                                            divNodata.Visible = false;
                                            if (((Button)sender).CommandName == "SELECTEDACC")
                                            {
                                                GetSelectedAccounts();
                                            }
                                            else
                                            {
                                                if (!ValidateForm())
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                    Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                                                }
                                                else
                                                {
                                                    GetSelectedAccounts();
                                                    SetFieldValuesCrystal(RptType, reportName[0].ToString());
                                                }
                                            }
                                            break;
                                        case ApplicationType.PSAS:
                                            divNodata.Visible = false;
                                            checkedListCheckedItems = CheckListSearchControl1.GetCheckedItems();
                                            if (((Button)sender).CommandName == "SELECTEDACC")
                                            {
                                                GetSelectedAccounts();
                                            }
                                            else
                                            {
                                                if (!ValidateForm())
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                    Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                                                }
                                                else
                                                {
                                                    GetSelectedAccounts();
                                                    SetFieldValuesCrystal(RptType, reportName[0].ToString());
                                                }
                                            }
                                            break;
                                        case ApplicationType.TB:
                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                                            }
                                            else
                                            {
                                                SetFieldValuesCrystal(RptType, reportName[0].ToString());
                                            }
                                            break;

                                    }
                                    break;
                                #endregion
                                #region RDLC Calling
                                case ReportType.RDLCReport:
                                    switch (RptType)
                                    {
                                        case ApplicationType.QAC:
                                            GetFieldValues(RptType);
                                            SetFieldValues(RptType);
                                            break;

                                        case ApplicationType.TB:
                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                            }
                                            else
                                            {
                                                SetFieldValues(RptType);
                                            }
                                            break;

                                        case ApplicationType.AS:
                                        case ApplicationType.GLFIN:
                                            rvViewReport.Visible = false;
                                            if (((Button)sender).CommandName == "SELECTEDACC")
                                            {
                                                GetSelectedAccounts();
                                            }
                                            else
                                            {
                                                if (!ValidateForm())
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                }
                                                else
                                                {
                                                    GetSelectedAccounts();
                                                    SetFieldValues(RptType);
                                                }
                                            }
                                            break;

                                        case ApplicationType.SAS:
                                        case ApplicationType.BRC:
                                        case ApplicationType.PSAS:
                                            rvViewReport.Visible = false;

                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                            }
                                            else
                                            {
                                                GetSelectedAccounts();
                                                SetFieldValues(RptType);
                                            }
                                            break;

                                    }
                                    break;
                                    #endregion
                            }
                            break;
                        #region Cancel
                        case ActionsEnum.CANCEL:
                            //Response.Redirect(ReturnUrl);
                            if (Convert.ToString(RecPK) == string.Empty)
                                Response.Redirect(Convert.ToString(Request.Url));
                            else if (hdfRefUrl.Value != string.Empty)
                                Response.Redirect(hdfRefUrl.Value);

                            break;
                        case ActionsEnum.PARTY:
                            if (hdfSendTo.Value == "1")
                            {
                                CustomerInvtype();
                            }
                            else if (hdfSendTo.Value == "2")
                            {
                                VendorInvtype();
                            }

                            BindTree();
                            break;
                        #endregion
                        #region Print
                        case ActionsEnum.PRINT:
                            GetFieldValues(RptType);
                            if (AppTypeDetailsList == null)
                            {
                                ClearCrystalReport();
                            }
                            divReportViewer.Visible = false;
                            divCrystalReportViewer.Visible = false;
                            divNodata.Visible = false;
                            var reportName1 = AppTypeDetailsList.Select(l => l.AST_OP_FILE1).ToList();
                            switch (((string[])reportName1[0].ToString().Split('.'))[1].Trim().ToLower())
                            {
                                #region crystal report calling
                                case ReportType.CrystalReport:
                                    if (reportDocument != null)
                                    {
                                        ClearCrystalReport();
                                        reportDocument.Close();
                                        reportDocument.Dispose();
                                        reportDocument = null;
                                        GC.Collect();
                                    }
                                    switch (RptType)
                                    {
                                        case ApplicationType.AS:
                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                            }
                                            else
                                            {
                                                GetSelectedAccounts();
                                                SetFieldValuesCrystal(RptType, reportName1[0].ToString());
                                            }
                                            break;
                                        case ApplicationType.GLFIN:
                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                            }
                                            else
                                            {
                                                GetSelectedAccounts();
                                                SetFieldValuesCrystal(RptType, reportName1[0].ToString());
                                            }
                                            break;
                                        case ApplicationType.TB:
                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                            }
                                            else
                                            {
                                                GetSelectedAccounts();
                                                SetFieldValuesCrystal(RptType, reportName1[0].ToString());
                                            }
                                            break;
                                        case ApplicationType.SAS:
                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                            }
                                            else
                                            {
                                                GetSelectedAccounts();
                                                SetFieldValuesCrystal(RptType, reportName1[0].ToString());
                                            }
                                            break;
                                        case ApplicationType.PSAS:
                                            checkedListCheckedItems = CheckListSearchControl1.GetCheckedItems();
                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                            }
                                            else
                                            {
                                                GetSelectedAccounts();
                                                SetFieldValuesCrystal(RptType, reportName1[0].ToString());
                                            }
                                            break;
                                    }
                                    if (reportDocument != null)
                                    {
                                        foreach (ParameterField paramfield in GERP_Report.ParameterFieldInfo)
                                        {
                                            reportDocument.SetParameterValue(paramfield.Name, paramfield.CurrentValues);
                                        }
                                        reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Page.Response, false, (RptType + RptSubType + RecPK + ChequeID).ToString());
                                    }
                                    break;
                                #endregion
                                #region RDLC Calling
                                case ReportType.RDLCReport:
                                    switch (RptType)
                                    {
                                        case ApplicationType.TB:
                                            SetFieldValues(RptType);
                                            PrintPDF();
                                            break;
                                        case ApplicationType.AS:
                                        case ApplicationType.GLFIN:
                                        case ApplicationType.BRC:
                                        case ApplicationType.PSAS:
                                        case ApplicationType.SAS:
                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                            }
                                            else
                                            {
                                                GetSelectedAccounts();
                                                SetFieldValues(RptType);
                                                PrintPDF();
                                            }
                                            break;
                                    }
                                    break;
                                    #endregion
                            }
                            break;
                        #endregion
                        #region EXCEL
                        case ActionsEnum.EXCEL:
                            GetFieldValues(RptType);
                            if (AppTypeDetailsList == null)
                            {
                                ClearCrystalReport();
                                divReportViewer.Visible = false;
                                divCrystalReportViewer.Visible = false;
                                divNodata.Visible = false;

                            }
                            reportName = AppTypeDetailsList.Select(l => l.AST_OP_FILE1).ToList();
                            switch (((string[])reportName[0].ToString().Split('.'))[1].Trim().ToLower())
                            {
                                #region crystal report calling
                                case ReportType.CrystalReport:
                                    if (reportDocument != null)
                                    {
                                        ClearCrystalReport();
                                        reportDocument.Close();
                                        reportDocument.Dispose();
                                        reportDocument = null;
                                        GC.Collect();
                                    }
                                    divReportViewer.Visible = true;
                                    divCrystalReportViewer.Visible = true;
                                    GERP_Report.Visible = true;
                                    divNodata.Visible = false;
                                    switch (RptType)
                                    {
                                        case ApplicationType.AS:
                                            divNodata.Visible = false;
                                            if (((Button)sender).CommandName == "SELECTEDACC")
                                            {
                                                GetSelectedAccounts();
                                            }
                                            else
                                            {
                                                if (!ValidateForm())
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                    Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                                                }
                                                else
                                                {
                                                    GetSelectedAccounts();
                                                    SetFieldValuesCrystalEXCEL(RptType, reportName[0].ToString());
                                                }
                                            }
                                            break;
                                        case ApplicationType.GLFIN:
                                            divNodata.Visible = false;
                                            if (((Button)sender).CommandName == "SELECTEDACC")
                                            {
                                                GetSelectedAccounts();
                                            }
                                            else
                                            {
                                                if (!ValidateForm())
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                    Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                                                }
                                                else
                                                {
                                                    GetSelectedAccounts();
                                                    SetFieldValuesCrystalEXCEL(RptType, reportName[0].ToString());
                                                }
                                            }
                                            break;
                                        case ApplicationType.SAS:
                                            divNodata.Visible = false;
                                            if (((Button)sender).CommandName == "SELECTEDACC")
                                            {
                                                GetSelectedAccounts();
                                            }
                                            else
                                            {
                                                if (!ValidateForm())
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                    Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                                                }
                                                else
                                                {
                                                    GetSelectedAccounts();
                                                    SetFieldValuesCrystalEXCEL(RptType, reportName[0].ToString());
                                                }
                                            }
                                            break;
                                        case ApplicationType.PSAS:
                                            divNodata.Visible = false;
                                            checkedListCheckedItems = CheckListSearchControl1.GetCheckedItems();
                                            if (((Button)sender).CommandName == "SELECTEDACC")
                                            {
                                                GetSelectedAccounts();
                                            }
                                            else
                                            {
                                                if (!ValidateForm())
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                    Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                                                }
                                                else
                                                {
                                                    GetSelectedAccounts();
                                                    SetFieldValuesCrystalEXCEL(RptType, reportName[0].ToString());
                                                }
                                            }
                                            break;
                                        case ApplicationType.TB:
                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                Session[ERP.Utilities.SessionStrings.CRReportData] = null;
                                            }
                                            else
                                            {
                                                SetFieldValuesCrystalEXCEL(RptType, reportName[0].ToString());
                                            }
                                            break;

                                    }


                                    break;
                                #endregion
                                #region RDLC Calling
                                case ReportType.RDLCReport:
                                    switch (RptType)
                                    {
                                        case ApplicationType.QAC:
                                            GetFieldValues(RptType);
                                            SetFieldValues(RptType);
                                            break;

                                        case ApplicationType.TB:
                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                            }
                                            else
                                            {
                                                SetFieldValues(RptType);
                                            }
                                            break;

                                        case ApplicationType.AS:
                                        case ApplicationType.GLFIN:
                                            rvViewReport.Visible = false;
                                            if (((Button)sender).CommandName == "SELECTEDACC")
                                            {
                                                GetSelectedAccounts();
                                            }
                                            else
                                            {
                                                if (!ValidateForm())
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                                }
                                                else
                                                {
                                                    GetSelectedAccounts();
                                                    SetFieldValues(RptType);
                                                }
                                            }
                                            break;

                                        case ApplicationType.SAS:
                                        case ApplicationType.BRC:
                                        case ApplicationType.PSAS:
                                            rvViewReport.Visible = false;

                                            if (!ValidateForm())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                            }
                                            else
                                            {
                                                GetSelectedAccounts();
                                                SetFieldValues(RptType);
                                            }
                                            break;

                                    }
                                    break;
                                    #endregion
                            }
                            break;

                            #endregion
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    if (RptType == ApplicationType.AS)
                    {
                        if (((ImageButton)sender).CommandName == "SHOWACCOUNTS")
                        {
                            txtAccount.Text = string.Empty;
                            hdfAccount.Value = string.Empty;
                            BindTree();
                            divAccPopUp.Visible = true;
                        }
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlSubLedger")
                    {
                        BindTree();
                    }
                    if (((DropDownList)sender).ID == "ddlInvoType")
                    {
                        BindTree();
                    }
                    if (((DropDownList)sender).ID == "ddlLocation")
                    {
                        BindTree();
                    }
                }
            }
            catch (Exception ex)
            {
                //Process Exception and show error message
            }
            finally
            {
                //reset all objects
            }
        }
        public void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            //
            try
            {
                #region EIPJ
                /*
               if (RptType == ApplicationType.VTYPEPCVJ || RptType == ApplicationType.VTYPEVPJ )
               {
                   RecPK = Convert.ToInt32(dsVoucher.Tables[0].Rows[RowIndex++][0]);
                   lstPayment = currentEntity.SPFIN_PAYMENT_VND_VOUCHER_RPT(RecPK, TrxRefType).ToList();
                   decimal Vat = 0;
                   decimal beforeVat = 0;
                   decimal WithHolding = 0;
                   decimal VatBuyNotYetDue = 0;
                   decimal NetAmount = 0;
                   if (lstVoucher != null && lstVoucher.Count > 0)
                   {
                       for (int i = 0; i <= lstVoucher.Count - 1; i++)
                       {
                           if (lstVoucher[i].FTR_ACC_SUB_TYPE == (int)AccSubType.VatBuy)
                           {
                               decimal VatBuy = lstVoucher[i].FTR_DR_AMT_BC.Value;
                               Vat = Vat + VatBuy;
                               decimal NotYetDue = lstVoucher[i].FTR_CR_AMT_BC.Value;
                               VatBuyNotYetDue = VatBuyNotYetDue + NotYetDue;
                           }
                           if (RptType != ApplicationType.DPVJ && RptType != ApplicationType.PCVJ)
                           {
                               if (lstVoucher[i].FTR_ACC_SUB_TYPE != (int)AccSubType.VatBuy && lstVoucher[i].FTR_ACC_SUB_TYPE != (int)AccSubType.GainLossSales && lstVoucher[i].FTR_ACC_SUB_TYPE != (int)AccSubType.BankCharge && lstVoucher[i].FTR_ACC_SUB_TYPE != (int)AccSubType.GainLossPurchase && lstVoucher[i].FTR_ACC_SUB_TYPE != (int)AccSubType.WHT)
                               {
                                   decimal BeforeVat7 = lstVoucher[i].FTR_DR_AMT_BC.Value;
                                   beforeVat = beforeVat + BeforeVat7;

                                   decimal NetAmtCR = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                   NetAmount = NetAmount + NetAmtCR;
                               }
                           }
                           else
                           {
                               if (lstVoucher[i].FTR_IS_BANK_CHARGE == 0)
                               {
                                   decimal BeforeVat7 = lstVoucher[i].FTR_DR_AMT_BC.Value;
                                   beforeVat = beforeVat + BeforeVat7;

                                   decimal NetAmtCR = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                   NetAmount = NetAmount + NetAmtCR;
                               }
                               if (lstVoucher[i].FTR_EXCLUDE_FOB == 1)
                               {
                                   decimal ExcludeFOB = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                   beforeVat = beforeVat - ExcludeFOB;
                               }
                               //Net amount shows wrong value.Commented as discussed with Manoj sir and Nikhil
                               ////if (lstVoucher[i].FTH_WHT_DTL_COUNT == 0 && lstVoucher[i].FTR_EXCLUDE_FOB == 1)
                               ////{
                               ////    decimal ExcludeFOB = lstVoucher[i].FTR_CR_AMT_BC.Value;
                               ////    NetAmount = NetAmount - ExcludeFOB;
                               ////}
                               if (lstVoucher[i].FTH_WHT_DTL_COUNT == 0 && lstVoucher[i].FTR_ACC_SUB_TYPE == (int)AccSubType.WHT)
                               {
                                   decimal wht = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                   if (lstVoucher[i].FTR_IS_BANK_CHARGE != 0)
                                   {
                                       beforeVat = beforeVat - wht;
                                   }
                               }
                           }
                           if (RptType != ApplicationType.DPVJ && RptType != ApplicationType.PCVJ)
                           {
                               if (lstVoucher[i].FTR_ACC_SUB_TYPE == (int)AccSubType.WHT)
                               {
                                   decimal WithHoldingTax = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                   WithHolding = WithHolding + WithHoldingTax;
                               }
                           }
                           else
                           {
                               if (lstVoucher[i].FTR_ACC_SUB_TYPE == (int)AccSubType.WHT)
                               {
                                   if (lstVoucher[i].FTH_WHT_DTL_COUNT == 0)
                                   {
                                       WithHolding = 0;
                                   }
                                   else
                                   {
                                       decimal WithHoldingTax = lstVoucher[i].FTR_CR_AMT_BC.Value;
                                       WithHolding = WithHolding + WithHoldingTax;
                                   }
                               }

                           }
                       }
                       beforeVat = beforeVat - VatBuyNotYetDue;

                       if (IsTaxAvailable())
                       {
                           if (lstPayment != null && lstPayment.Count > 0)
                           {
                               for (int i = 0; i <= lstPayment.Count - 1; i++)
                               {
                                   decimal decVat = 0;
                                   decimal GrossAmt = (Convert.ToDecimal(lstPayment[i].PVM_INV_AMOUNT_NET_TC.Value) * Convert.ToDecimal(lstPayment[i].PVM_EXCHG_RATE.Value));
                                   decimal PaymentAmt = lstPayment[i].PVM_PAID_AMOUNT_TC.Value;
                                   decimal tax = Convert.ToDecimal(lstPayment[i].PVM_IVH_TAX_TC.Value) * Convert.ToDecimal(lstPayment[i].PVM_EXCHG_RATE.Value);
                                   if (GrossAmt != 0)
                                   {
                                       decVat = (tax / GrossAmt) * PaymentAmt;
                                   }
                                   Vat = Vat + decVat;
                               }
                               beforeVat = beforeVat - Vat;
                           }
                       }
                   }
                   if (RptType == ApplicationType.VPJ || RptType == ApplicationType.VTYPEVPJ || RptType == ApplicationType.VPTJ || RptType == ApplicationType.EIPJ || RptType == ApplicationType.EIPTJ || (RptType == ApplicationType.SIPJ && RptSubType == 0))
                   {
                       decimal TotalAmt = 0;
                       decimal lineAmt = 0;
                       if (lstPayment != null && lstPayment.Count > 0)
                       {
                           for (int i = 0; i <= lstPayment.Count - 1; i++)
                           {
                               if (lstPayment[i].PVM_PAID_AMOUNT_TC.Value != 0)
                               {
                                   lineAmt = lstPayment[i].PVM_PAID_AMOUNT_TC.Value;
                                   TotalAmt = TotalAmt + lineAmt;
                               }
                           }
                       }
                       e.Parameters
                       parameters = new ReportParameter("TotalAmt", TotalAmt.ToString());
                       locRpt.SetParameters(parameters);
                   }
                   parameters = new ReportParameter("VatBuy", Vat.ToString());
                   locRpt.SetParameters(parameters);
                   parameters = new ReportParameter("beforeVat", beforeVat.ToString());
                   locRpt.SetParameters(parameters);
                   parameters = new ReportParameter("WithHolding", WithHolding.ToString());
                   locRpt.SetParameters(parameters);
                   parameters = new ReportParameter("NetAmount", NetAmount.ToString());
                   locRpt.SetParameters(parameters);
               }
               */
                #endregion
                switch (RptType)
                {
                    case ApplicationType.VTYPE:
                    case ApplicationType.VTYPEDNJPI:
                    case ApplicationType.VTYPECNJPI:
                    case ApplicationType.VTYPEDNJSI:
                    case ApplicationType.VTYPECNJSI:
                    case ApplicationType.VTYPEDRVJ:
                    case ApplicationType.VTYPEMIJ:
                    case ApplicationType.VTYPEDPRJ:
                    case ApplicationType.VTYPECWIPJ:
                        e.DataSources.Add(new ReportDataSource("JVHeader", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(GetCompanyDetails(Convert.ToInt32(hdfCompanyPK.Value)));
                        break;
                    case ApplicationType.VTYPESIJ:
                    case ApplicationType.VTYPEMSIJ:
                        e.DataSources.Add(new ReportDataSource("SVHeader", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(new ReportDataSource("SVAccountDtls", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(GetCompanyDetails(Convert.ToInt32(hdfCompanyPK.Value)));
                        break;
                    case ApplicationType.VTYPEPSIJ:
                    case ApplicationType.VTYPEEIJ:
                        e.DataSources.Add(new ReportDataSource("SVHeader", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(new ReportDataSource("SVAccountDtls", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(GetCompanyDetails(Convert.ToInt32(hdfCompanyPK.Value)));
                        break;
                    case ApplicationType.VTYPECRJ:
                    case ApplicationType.VTYPEMSIRJ:
                        e.DataSources.Add(new ReportDataSource("RVHeader", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(new ReportDataSource("RVAccountDtls", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(GetCompanyDetails(Convert.ToInt32(hdfCompanyPK.Value)));
                        break;
                    case ApplicationType.VTYPEVPJ:
                        e.DataSources.Add(new ReportDataSource("PVHeader", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(new ReportDataSource("PVMode", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(new ReportDataSource("PVRecordings", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(GetCompanyDetails(Convert.ToInt32(hdfCompanyPK.Value)));
                        break;
                    case ApplicationType.VTYPEPCVJ:
                        e.DataSources.Add(new ReportDataSource("PVRecordings", dsVoucher.Tables[SubReportIndex++]));
                        // e.DataSources.Add(new ReportDataSource("PVMode", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(GetCompanyDetails(Convert.ToInt32(hdfCompanyPK.Value)));
                        break;
                    case ApplicationType.VTYPEDPVJ:
                        e.DataSources.Add(new ReportDataSource("PVRecordings", dsVoucher.Tables[SubReportIndex++]));
                        //  e.DataSources.Add(new ReportDataSource("PVMode", dsVoucher.Tables[SubReportIndex++]));
                        e.DataSources.Add(GetCompanyDetails(Convert.ToInt32(hdfCompanyPK.Value)));
                        break;
                    //case ApplicationType.VTYPEEIJ:
                    //    e.DataSources.Add(new ReportDataSource("PVHeaderDtls", dsVoucher.Tables[SubReportIndex++]));
                    //    e.DataSources.Add(new ReportDataSource("PVAccountDtls", dsVoucher.Tables[SubReportIndex++]));
                    //    e.DataSources.Add(GetCompanyDetails(Convert.ToInt32(hdfCompanyPK.Value)));
                    //    break;

                    #region DO
                    case ApplicationType.DO:
                    case ApplicationType.DOD:
                        ReportDataSource dsDOProductDtls;
                        DataTable dtDOProductDtls = dsDelivaryOrder.Tables[1];
                        dsDOProductDtls = new ReportDataSource("DOProductDtls", dtDOProductDtls);
                        if (RptSubType == 3 || RptSubType == 4)
                        {
                            ReportDataSource dsDOHeaderDtls;
                            DataTable dtDOHeaderDtls = dsDelivaryOrder.Tables[0];
                            dsDOHeaderDtls = new ReportDataSource("DOHeaderDtls", dtDOHeaderDtls);
                            e.DataSources.Add(dsDOHeaderDtls);
                        }
                        e.DataSources.Add(dsDOProductDtls);
                        e.DataSources.Add(GetCompanyDetails(Convert.ToInt32(hdfCompanyPK.Value)));
                        break;
                    #endregion
                    #region GST
                    case ApplicationType.GST:
                        ReportDataSource dsGSTHeaderDtls;
                        ReportDataSource dsGSTTaxDetails;
                        ReportDataSource dsBreakDownDtls;
                        if (dsGST != null)
                        {
                            DataTable dtHeaderDtls = dsGST.Tables[0];
                            DataTable dtTaxDtls = dsGST.Tables[1];
                            DataTable dtBreakDownDtls = dsGST.Tables[2];
                            if (dtHeaderDtls != null && dtHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtHeaderDtls.Rows[0]["TGH_COMPANY"]);
                            }
                            dsGSTHeaderDtls = new ReportDataSource("GSTHEADER", dtHeaderDtls);
                            dsGSTTaxDetails = new ReportDataSource("GSTTaxDetails", dtTaxDtls);
                            dsBreakDownDtls = new ReportDataSource("BreakDownDetails", dtBreakDownDtls);
                            e.DataSources.Add(dsGSTHeaderDtls);
                            e.DataSources.Add(dsGSTTaxDetails);
                            e.DataSources.Add(dsBreakDownDtls);
                            e.DataSources.Add(GetCompanyDetails(CompanyPK));
                        }
                        break;
                    #endregion
                    #region VP
                    case ApplicationType.VP:
                        ReportDataSource dsVP;
                        dsVP = new ReportDataSource("ReportDtls", currentEntity.SPFIN_PND54_RPT(RecPK));
                        e.DataSources.Add(dsVP);
                        e.DataSources.Add(GetCompanyDetails(CompanyPK));
                        break;
                    #endregion
                    #region DPVJ,PCVJ
                    case ApplicationType.DPVJ:
                    case ApplicationType.PCVJ:
                        dsVP = new ReportDataSource("ReportDtls", currentEntity.SPFIN_PND54_FIN_RPT(RecPK));
                        e.DataSources.Add(dsVP);
                        e.DataSources.Add(GetCompanyDetails(CompanyPK));
                        break;
                    #endregion
                    #region PO
                    case ApplicationType.PO:
                    case ApplicationType.POPG:
                    case ApplicationType.POTR:
                    case ApplicationType.POG:
                    case ApplicationType.POT:
                    case ApplicationType.POP:
                        ReportDataSource dsPOPRDtls;
                        DataTable dtPOPRDtls = dsPurchaseRequest.Tables[3];
                        dsPOPRDtls = new ReportDataSource("PRDetails", dtPOPRDtls);
                        int PODID = Convert.ToInt32(e.Parameters["PODID"].Values.First());
                        e.DataSources.Add(dsPOPRDtls);
                        break;
                    #endregion
                    #region MCR
                    case ApplicationType.MCR:
                        ReportDataSource dsHeaderWeight = new ReportDataSource("HeaderWeightDS", dsReportDetails.Tables[0]);
                        ReportDataSource dsTumbling = new ReportDataSource("TumblingDS", dsReportDetails.Tables[1]);
                        ReportDataSource dsPreAudit = new ReportDataSource("PreAuditDS", dsReportDetails.Tables[2]);
                        ReportDataSource dsInspection = new ReportDataSource("InspectionDS", dsReportDetails.Tables[3]);
                        ReportDataSource dsChlorination = new ReportDataSource("ChlorinationDS", dsReportDetails.Tables[4]);
                        ReportDataSource dsWaterTightTest = new ReportDataSource("WaterTightTestDS", dsReportDetails.Tables[5]);
                        ReportDataSource dsAirtest = new ReportDataSource("AirTestDS", dsReportDetails.Tables[6]);
                        ReportDataSource dsPacking = new ReportDataSource("PackingDS", dsReportDetails.Tables[7]);
                        ReportDataSource dsLeaching = new ReportDataSource("LeachingDS", dsReportDetails.Tables[8]);
                        ReportDataSource dsWashing = new ReportDataSource("WashingDS", dsReportDetails.Tables[9]);
                        ReportDataSource dsMoulding = new ReportDataSource("MouldingDS", dsReportDetails.Tables[10]);
                        ReportDataSource dsToyChlorination = new ReportDataSource("ToyChlorinationDS", dsReportDetails.Tables[11]);
                        ReportDataSource dsGliding = new ReportDataSource("GlidingDS", dsReportDetails.Tables[12]);
                        ReportDataSource dsHeading = new ReportDataSource("HeadingDS", dsReportDetails.Tables[13]);//---heading section for traceability Leaching
                        ReportDataSource dsWaterTightNewTest = new ReportDataSource("WaterTightTestNewDS", dsReportDetails.Tables[14]);//TM no wtt
                        ReportDataSource dsVisualInspectionNew = new ReportDataSource("VisualInspectionNewDS", dsReportDetails.Tables[15]);//TM no wtt
                        ReportDataSource dsBasketCard = new ReportDataSource("BasketCardDS", dsReportDetails.Tables[16]);
                        ReportDataSource dsTumblingRework = new ReportDataSource("TumblingReworkDS", dsReportDetails.Tables[18]);
                        ReportDataSource dsPackingRework = new ReportDataSource("PackingReworkDS", dsReportDetails.Tables[19]);
                        ReportDataSource dsTumblingQA = new ReportDataSource("TumblingQADS", dsReportDetails.Tables[20]);
                        ReportDataSource dsPackingGrouped = new ReportDataSource("PackingGroupedDS", dsReportDetails.Tables[21]);
                        ReportDataSource dsPackingQA = new ReportDataSource("PackingQADS", dsReportDetails.Tables[24]);
                        ReportDataSource dsPackingReworkQA = new ReportDataSource("PackingReworkQADS", dsReportDetails.Tables[25]);
                        ReportDataSource dsWalletBinDetails = new ReportDataSource("WalletBinDetailsDS", dsReportDetails.Tables[26]);
                        ReportDataSource dsWalletHeader = new ReportDataSource("WalletHeaderDS", dsReportDetails.Tables[27]);

                        e.DataSources.Add(dsHeaderWeight);
                        e.DataSources.Add(dsTumbling);
                        e.DataSources.Add(dsPreAudit);
                        e.DataSources.Add(dsInspection);
                        e.DataSources.Add(dsChlorination);
                        e.DataSources.Add(dsWaterTightTest);
                        e.DataSources.Add(dsAirtest);
                        e.DataSources.Add(dsPacking);
                        e.DataSources.Add(dsLeaching);
                        e.DataSources.Add(dsWashing);
                        e.DataSources.Add(dsMoulding);
                        e.DataSources.Add(dsToyChlorination);
                        e.DataSources.Add(dsGliding);
                        e.DataSources.Add(dsHeading);
                        e.DataSources.Add(dsWaterTightNewTest);
                        e.DataSources.Add(dsVisualInspectionNew);
                        e.DataSources.Add(dsTumblingRework);
                        e.DataSources.Add(dsPackingRework);
                        e.DataSources.Add(dsTumblingQA);
                        e.DataSources.Add(dsBasketCard);
                        e.DataSources.Add(dsPackingGrouped);
                        e.DataSources.Add(dsPackingQA);
                        e.DataSources.Add(dsPackingReworkQA);
                        e.DataSources.Add(dsWalletBinDetails);
                        e.DataSources.Add(dsWalletHeader);
                        e.DataSources.Add(GetCompanyDetails(1));
                        string QMSRef, HideQMSRef, DateFormat, CurrencyFormat, NumberFormat
                            , ExchangeRate, RateFormat, WeightFormat, BinDecimalDigit, HideLogo, HideHeadTitle, HideSubTitle
                            , HideFooterText, HidePageNo, HeadTitle, SubTitle, Logo, BarCode, FooterText;
                        DateFormat = e.Parameters["DateFormat"].Values.ToString();
                        CurrencyFormat = e.Parameters["CurrencyFormat"].Values.ToString();
                        NumberFormat = e.Parameters["NumberFormat"].Values.ToString();
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CheckUserRight(string path)
        {
            base.CheckUserRight(path.ToLower());
        }
        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            DataTable dtCmp;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            trvAccounts.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
            if (!IsPostBack)
            {
                #region If Request From Menu or Inbox
                if (Request.QueryString[QueryStrings.FromExt] == null)
                {
                    txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();

                    RptType = Request.QueryString["APPTYPE"].ToString();
                    IOType = Request.QueryString["IOType"] != string.Empty ? Convert.ToInt32(Request.QueryString["IOType"]) : 0;

                    //FTH_REF_TYPE = Request.QueryString["FTH_REF_TYPE"] != string.Empty ? (Request.QueryString["FTH_REF_TYPE"]).ToString() : "";
                    //VOUCHER_FROM = Request.QueryString["VOUCHER_FROM"] != string.Empty ? (Request.QueryString["VOUCHER_FROM"]).ToString() : "";
                    //VOUCHER_TO = Request.QueryString["VOUCHER_TO"] != string.Empty ? (Request.QueryString["VOUCHER_TO"]).ToString() : "";
                    hdfAppType.Value = RptType;
                    hdfRefUrl.Value = "";
                    RptSubType = Request.QueryString["APPSUBTYPE"] != string.Empty ? Convert.ToInt32(Request.QueryString["APPSUBTYPE"]) : 0;
                    hdfSubType.Value = RptSubType.ToString();
                    IsExcelPrint = Request.QueryString["ISEXCELPRINT"] != string.Empty ? Convert.ToInt32(Request.QueryString["ISEXCELPRINT"]) : 0;
                    if (Request.QueryString["VRSN"] != null)
                    {
                        int vsion = 0;
                        Version = Int32.TryParse(Request.QueryString["VRSN"].ToString().Trim(), out vsion) ? vsion : 0;
                    }
                    if (string.IsNullOrEmpty(Request.QueryString["SICOUNTTEXT"]))
                        SICountText = "NULL";
                    else
                        SICountText = Request.QueryString["SICOUNTTEXT"] != string.Empty ? Request.QueryString["SICOUNTTEXT"] : string.Empty;

                    if (RptType == ApplicationType.BSRC)
                    {
                        DisplayTab = Convert.ToInt32(Request.QueryString["DISPLAYTYPE"]);
                    }
                    if ((RptType == ApplicationType.VP && RptSubType == 1) || (RptType == ApplicationType.DPVJ && RptSubType == 4))
                    {
                        ChequeID = Request.QueryString["ChequeID"] != string.Empty ? Request.QueryString["ChequeID"] : "0";
                    }
                    if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString().Trim() != string.Empty)
                    {
                        RecPK = Convert.ToInt32(Request.QueryString["ID"]);
                    }
                    else
                    {
                        ReturnUrl = Convert.ToString(Request.Url);
                        hdfRefUrl.Value = ReturnUrl;
                    }
                    if (hdfRefUrl.Value == string.Empty)
                        hdfRefUrl.Value = Convert.ToString(Request.UrlReferrer);
                    if (Request.QueryString["RevID"] != null)
                    {
                        int RID = 0;
                        RevPK = Int32.TryParse(Request.QueryString["RevID"].ToString().Trim(), out RID) ? RID : 0;
                    }
                    dtCmp = BusinessLogic.CommonManagement.CommonBL.GetDeptCompany(currentUser.CurrentDeptPK);
                    if (Request.QueryString["COMPANY"] != null)
                    {
                        int company = 0;
                        CompanyPK = Int32.TryParse(Request.QueryString["COMPANY"].ToString().Trim(), out company) ? company : 0;
                        if (CompanyPK == 0)
                        {
                            if (dtCmp != null && dtCmp.Rows.Count > 0)
                                CompanyPK = Convert.ToInt32(dtCmp.Rows[0][Resources.DataFieldRes.DeptCompany].ToString());
                        }
                    }
                    else
                    {
                        if (dtCmp != null && dtCmp.Rows.Count > 0)
                            CompanyPK = Convert.ToInt32(dtCmp.Rows[0][Resources.DataFieldRes.DeptCompany].ToString());
                    }

                    if (Request.QueryString["VERSION"] != null && Request.QueryString["VERSION"].ToString().Trim() != string.Empty)
                    {
                        VoucherVersion = Convert.ToInt16(Request.QueryString["VERSION"]);
                    }

                    divQAC.Visible = false;
                    divTrialBal.Visible = false;
                    usrDateFilter.Visible = false;
                    divGL.Visible = false;
                    divFY.Visible = false;
                    divAccPopUp.Visible = false;
                    divchecklist.Visible = false;
                    if (RptType == ApplicationType.PR || RptType == ApplicationType.PRT || RptType == ApplicationType.RFQ || RptType == ApplicationType.PO || RptType == ApplicationType.POG || RptType == ApplicationType.POPG || RptType == ApplicationType.POTR || RptType == ApplicationType.POP || RptType == ApplicationType.POT || RptType == ApplicationType.GIN || RptType == ApplicationType.GRN)
                        btnSearch.Visible = false;
                    else
                        btnSearch.Visible = true;
                    if (RptType == ApplicationType.PSAS)
                    {
                        divchecklist.Visible = true;
                        CheckListSearchControl1.Visible = true;
                    }
                    lblBreadCrum.Text = this.GetLocalResourceObject("Print").ToString();
                    ConfigurationSettings();
                    GetFieldValues(ApplicationType.LOCATION);
                    BindDropDown(ApplicationType.LOCATION);
                    switch (RptType)
                    {
                        case ApplicationType.GIN:
                        case ApplicationType.GRN:
                        case ApplicationType.PR:
                        case ApplicationType.PRT:
                        case ApplicationType.PO:
                        case ApplicationType.POG:
                        case ApplicationType.POPG:
                        case ApplicationType.POTR:
                        case ApplicationType.POP:
                        case ApplicationType.SCWO:
                        case ApplicationType.POT:
                        case ApplicationType.SO:
                        case ApplicationType.SOD:
                        case ApplicationType.IO:
                        case ApplicationType.DO:
                        case ApplicationType.DOD:
                        case ApplicationType.SI:
                        case ApplicationType.SIC:
                        case ApplicationType.SIM:
                        case ApplicationType.SIMSF:
                        case ApplicationType.SIMSS:
                        case ApplicationType.DSI:
                        case ApplicationType.DSID:
                        case ApplicationType.MSI://@@                        
                        case ApplicationType.MSIT:

                        case ApplicationType.PI:
                        case ApplicationType.TPI:
                        case ApplicationType.MI:
                        case ApplicationType.STA:
                        case ApplicationType.VNDEVAL:
                        case ApplicationType.PDCCJ:
                        case ApplicationType.PDCCTJ:
                        case ApplicationType.CN:
                        case ApplicationType.CNT:
                        case ApplicationType.DN:
                        case ApplicationType.DNT:
                        case ApplicationType.GST:
                        case ApplicationType.VTYPE:
                        case ApplicationType.VTYPESIJ:
                        case ApplicationType.VTYPEPSIJ:
                        case ApplicationType.VTYPEVPJ:
                        case ApplicationType.VTYPECRJ:
                        case ApplicationType.VTYPEMSIRJ:
                        case ApplicationType.VTYPEPCVJ:
                        case ApplicationType.VTYPEDPVJ:
                        case ApplicationType.VTYPEDRVJ:
                        case ApplicationType.VTYPEDNJPI:
                        case ApplicationType.VTYPECNJPI:
                        case ApplicationType.VTYPEDNJSI:
                        case ApplicationType.VTYPECNJSI:
                        case ApplicationType.VTYPEMIJ:
                        case ApplicationType.VTYPEEIJ:
                        case ApplicationType.VTYPEMSIJ:
                        case ApplicationType.VTYPEDPRJ:
                        case ApplicationType.VTYPECWIPJ:
                        case ApplicationType.EI:
                        case ApplicationType.CMP:
                        case ApplicationType.DISP:
                        case ApplicationType.ACI:
                        case ApplicationType.EMR:
                        case ApplicationType.OS:
                        case ApplicationType.DSA:
                        case ApplicationType.BOM:
                        case ApplicationType.EIT:
                        case ApplicationType.ASD:
                        case ApplicationType.SR:
                        case ApplicationType.SOA:
                        case ApplicationType.SRA:
                        case ApplicationType.FRD:
                        case ApplicationType.ES:
                        case ApplicationType.MTR:
                        case ApplicationType.MTI:
                        case ApplicationType.CID:
                        case ApplicationType.RMI:
                        case ApplicationType.RMIPM:
                            //case ApplicationType.PSIJ:
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.SHCID:
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.EMI:
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.MRT:
                            GetFieldValues("EMI");
                            SetFieldValues("EMI");
                            break;
                        case ApplicationType.VP:
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.DPVJ:
                        case ApplicationType.PCVJ:
                        case ApplicationType.PCRVJ:
                        case ApplicationType.CTVJ:
                            TrxRefType = Request.QueryString["APPTYPE"].ToString();
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.RFQ:
                            VndPK = Convert.ToInt32(Request.QueryString["VNDPK"]);
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.QAC:
                            divQAC.Visible = true;
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.CNJ:
                        case ApplicationType.CNTJ:
                        case ApplicationType.DNJ:
                        case ApplicationType.DNTJ:
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.SIJ:
                        case ApplicationType.MSIJ://@@
                        case ApplicationType.MSITJ:
                        case ApplicationType.DSIJ:
                        case ApplicationType.CRJ:
                        case ApplicationType.CRTJ:
                        case ApplicationType.VPJ:
                        case ApplicationType.PIJ:
                        case ApplicationType.TPIJ:
                        case ApplicationType.EIJ:
                        case ApplicationType.JV:
                        case ApplicationType.CLSTJ:
                        case ApplicationType.PPCCJ:
                        case ApplicationType.PCBJ:
                        case ApplicationType.SIPJ:
                        case ApplicationType.EIPJ:
                        case ApplicationType.RCBJ:
                        case ApplicationType.RCBTJ:
                        case ApplicationType.PSIJ:
                        case ApplicationType.FCHRJ:
                        case ApplicationType.FCHRJYE:
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
                        case ApplicationType.DPRJ:
                        case ApplicationType.CWIPJ:
                        case ApplicationType.ASDJ:
                        case ApplicationType.ACIJ:
                        case ApplicationType.AIPJ:
                        case ApplicationType.DNSJYE:
                        case ApplicationType.CNSJYE:
                        case ApplicationType.CNPJYE:
                        case ApplicationType.DNPJYE:
                        case ApplicationType.DPVCJ:
                        case ApplicationType.DPBJ:
                        case ApplicationType.DRVJ://Direct Receipt Voucher
                        case ApplicationType.YCV://Year Closing Voucher
                        case ApplicationType.PAYRLJ:
                        case ApplicationType.SALPYMTJ:
                        case ApplicationType.EITJ:
                        case ApplicationType.EIPTJ:
                        case ApplicationType.VPTJ:
                        case ApplicationType.PPCCTJ:
                        case ApplicationType.ESJ:
                        case ApplicationType.MIJ:
                            TrxRefType = Request.QueryString["TRXTYPE"].ToString();
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.VSE:
                            startOfMonth = Convert.ToDateTime(Request.QueryString["FROMDATE"].ToString());
                            endOfMonth = Convert.ToDateTime(Request.QueryString["TODATE"].ToString());
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.CQTN:
                        case ApplicationType.COA:
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.TB:
                            CheckUserRight("/reports/generatereport.aspx?id=&apptype=tb&appsubtype=");
                            divTrialBal.Visible = true;
                            lblBreadCrum.Text = this.GetLocalResourceObject("TrailBalance_Report").ToString();
                            //SetFieldValues(RptType);
                            break;
                        case ApplicationType.AS:
                            CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                            if (RptSubType == 0)
                                lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedger_Report").ToString();
                            else if (RptSubType == 2)
                            {
                                lblBreadCrum.Text = this.GetLocalResourceObject("GLConsolidated_Report").ToString();
                            }
                            else if (RptSubType == 3)
                            {
                                lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedgerBreadCrum").ToString();
                            }
                            DataTable dt = ConfigurationSettings();
                            if (hdfIsMultiplePlant.Value == "1")
                                divLocation.Visible = true;
                            else
                                BindTree();
                            divTrialBal.Visible = true;
                            tdUsrDate.Visible = false;
                            td1.Visible = true;
                            usrDateFilter.Visible = false;
                            lblOr.Visible = false;

                            divGL.Visible = true;
                            divAS.Visible = true;
                            divSAS.Visible = false;
                            divPSAS.Visible = false;
                            divFY.Visible = false;
                            div_Party.Visible = false;

                            break;
                        case ApplicationType.GLFIN:
                            CheckUserRight("/reports/generatereport.aspx?id=&apptype=glfin&appsubtype=");
                            if (RptSubType == 0)
                                lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedgerFinYr_Report").ToString();
                            else if (RptSubType == 2)
                            {
                                lblBreadCrum.Text = this.GetLocalResourceObject("GLConsolidated_Report").ToString();
                            }
                            else if (RptSubType == 3)
                            {
                                lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedgeFinYrrBreadCrum").ToString();
                            }
                            divFY.Visible = false;
                            divTrialBal.Visible = false;
                            usrDateFilter.Visible = true;
                            lblOr.Visible = false;

                            divGL.Visible = true;
                            divAS.Visible = true;
                            divSAS.Visible = false;
                            divPSAS.Visible = false;
                            div_Party.Visible = false;
                            DataTable dtconfig = ConfigurationSettings();
                            if (hdfIsMultiplePlant.Value == "1")
                                divLocation.Visible = true;
                            else
                                BindTree();
                            //BindTree();
                            BindFinYear();
                            break;
                        case ApplicationType.LST_VCH:
                            //TrxRefType = Request.QueryString["TRXTYPE"].ToString();
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.BRC:
                        case ApplicationType.SAS:
                            CheckUserRight("/reports/generatereport.aspx?id=&apptype=sas&appsubtype=");
                            if (RptType == "SAS")
                                lblBreadCrum.Text = this.GetLocalResourceObject("SubLedger_Report").ToString();
                            else if (RptType == "BRC")
                            {
                                lblBreadCrum.Text = this.GetLocalResourceObject("BankReconciliation").ToString();
                            }
                            divTrialBal.Visible = false;
                            usrDateFilter.Visible = true;
                            lblOr.Visible = false;
                            divGL.Visible = true;
                            divFY.Visible = false;
                            divAS.Visible = false;
                            divSAS.Visible = true;
                            divPSAS.Visible = false;
                            div_Party.Visible = false;
                            BindSubLedger();
                            if (RptType == ApplicationType.BRC)
                            {
                                ddlSubLedger.SelectedValue = "11";   // 11 for Bank 
                                ddlSubLedger.Visible = false;
                                //txtSubAccount.Attributes.Remove("cssclass");
                                lblSubLedger.Text = "Bank";
                                txtSubAccount.CssClass = "medium-a";
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "GetSubAccounts", "getSubAccounts();", true);
                                BindTree();
                            }
                            break;
                        case ApplicationType.PSAS:
                            CheckUserRight("/reports/generatereport.aspx?id=&apptype=psas&appsubtype=");
                            if (RptType == "PSAS")
                                lblBreadCrum.Text = this.GetLocalResourceObject("PartyLedger_Report").ToString();
                            divTrialBal.Visible = false;
                            usrDateFilter.Visible = true;
                            divGL.Visible = true;
                            divFY.Visible = false;
                            divAS.Visible = false;
                            divSAS.Visible = false;
                            divPSAS.Visible = true;
                            div_Party.Visible = true;

                            BindSubLedger();
                            if (RptType == ApplicationType.BRC)
                            {
                                ddlSubLedger.SelectedValue = "11";   // 11 for Bank 
                                ddlSubLedger.Visible = false;
                                //txtSubAccount.Attributes.Remove("cssclass");
                                lblSubLedger.Text = "Bank";
                                txtSubAccount.CssClass = "medium-a";
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "GetSubAccounts", "getSubAccounts();", true);
                                BindTree();
                            }
                            break;
                        case ApplicationType.CNTINSP:
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.PCS:
                            TrxRefType = Request.QueryString["TRXTYPE"].ToString();
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.SPLN:
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.MSIRJ:
                        case ApplicationType.MSIRTJ:
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.DPR:
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        #region STRLOMS:Option to print Barcode for the Locations
                        case ApplicationType.STRLOMS:
                            bizUnit = Convert.ToInt32(Request.QueryString["bizUnit"]);
                            deptCategory = Convert.ToInt32(Request.QueryString["DPTCATEGORY"]);
                            searchName = Request.QueryString["Status"].ToString();
                            searchValue = Request.QueryString["SearchValue"].ToString();
                            GetFieldValues(RptType);
                            dtLabelRptConfig = CommonBL.GetReportDetails("STRLOMS", 0, DateTime.Now);
                            BindCrystalReportForLabel(dtLabelRptConfig.Rows[0]["AST_OP_FILE1"].ToString(), GetLabelDataSourse(dtStoreLocn));
                            //SetFieldValues(RptType);
                            break;
                        case ApplicationType.BSRC:
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        #endregion
                        case ApplicationType.BINCARDISSUEWO:
                        case ApplicationType.CARTONISSUEWO:
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.DONOTE:
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.MCR:
                            if (RptType == ApplicationType.MCR)
                            {
                                BatchNo = Request.QueryString["BatchNo"];
                            }
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                        case ApplicationType.CWIP:
                            GetFieldValues(RptType);
                            SetFieldValues(RptType);
                            break;
                    }
                }
                #endregion

                #region If Request From External(Report or Other page) otherthan Menu or Inbox
                else if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                {
                    ReturnUrl = Convert.ToString(Request.Url);
                    hdfRefUrl.Value = ReturnUrl;
                    if (Request.QueryString[QueryStrings.PK] != null)
                        PK = Request.QueryString["PK"] != string.Empty ? Convert.ToInt32(Request.QueryString["PK"]) : 0;
                    if (Request.QueryString[QueryStrings.FromDate] != null)
                        txtFromDate.Text = hdfFromDate.Value = Convert.ToDateTime(Request.QueryString["FromDate"]).ToString(Resources.Constants.DateFormatShort);
                    if (Request.QueryString[QueryStrings.ToDate] != null)
                        txtToDate.Text = hdfToDate.Value = Convert.ToDateTime(Request.QueryString["ToDate"]).ToString(Resources.Constants.DateFormatShort);
                    if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString().Trim() != string.Empty)
                    {
                        RecPK = Convert.ToInt32(Request.QueryString["ID"]);
                    }
                    else
                    {
                        ReturnUrl = Convert.ToString(Request.Url);
                        hdfRefUrl.Value = ReturnUrl;
                    }
                    RptType = Request.QueryString["APPTYPE"].ToString();
                    hdfAppType.Value = RptType;
                    RptSubType = Request.QueryString["APPSUBTYPE"] != string.Empty ? Convert.ToInt32(Request.QueryString["APPSUBTYPE"]) : 0;
                    hdfSubType.Value = RptSubType.ToString();
                    divQAC.Visible = false;
                    divTrialBal.Visible = false;
                    usrDateFilter.Visible = false;
                    divGL.Visible = false;
                    divAccPopUp.Visible = false;
                    divFY.Visible = false;
                    if (RptType == ApplicationType.PR || RptType == ApplicationType.PRT || RptType == ApplicationType.RFQ || RptType == ApplicationType.PO || RptType == ApplicationType.POP || RptType == ApplicationType.POG || RptType == ApplicationType.POPG || RptType == ApplicationType.POTR || RptType == ApplicationType.POT || RptType == ApplicationType.GIN || RptType == ApplicationType.GRN || RptType == ApplicationType.SCWO)
                        btnSearch.Visible = false;
                    else
                        btnSearch.Visible = true;
                    lblBreadCrum.Text = this.GetLocalResourceObject("Print").ToString();
                    GetFieldValues(RptType);
                    if (AppTypeDetailsList == null)
                    {
                        ClearCrystalReport();
                        divReportViewer.Visible = false;
                        divCrystalReportViewer.Visible = false;
                        divNodata.Visible = false;

                    }
                    var reportName = AppTypeDetailsList.Select(l => l.AST_OP_FILE1).ToList();
                    switch (((string[])reportName[0].ToString().Split('.'))[1].Trim().ToLower())
                    {
                        #region crystal report calling
                        case ReportType.CrystalReport:
                            if (reportDocument != null)
                            {
                                ClearCrystalReport();
                                reportDocument.Close();
                                reportDocument.Dispose();
                                reportDocument = null;
                                GC.Collect();
                            }
                            switch (RptType)
                            {
                                case ApplicationType.AS:
                                    divReportViewer.Visible = true;
                                    divCrystalReportViewer.Visible = true;
                                    divNodata.Visible = false;
                                    hdfAccount.Value = Convert.ToString(PK);
                                    if (RptSubType == 0)
                                        lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedger_Report").ToString();
                                    if (!ValidateForm())
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                    }
                                    else
                                    {

                                        GetSelectedAccounts();
                                        SetFieldValuesCrystal(RptType, reportName[0].ToString());
                                    }
                                    break;
                                case ApplicationType.GLFIN:
                                    divReportViewer.Visible = true;
                                    divCrystalReportViewer.Visible = true;
                                    divNodata.Visible = false;
                                    divFY.Visible = true;
                                    hdfAccount.Value = Convert.ToString(PK);
                                    if (RptSubType == 0)
                                        lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedgerFinYr_Report").ToString();
                                    if (!ValidateForm())
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                    }
                                    else
                                    {
                                        BindFinYear();
                                        GetSelectedAccounts();
                                        SetFieldValuesCrystal(RptType, reportName[0].ToString());
                                    }
                                    break;
                                case ApplicationType.COL:
                                    divReportViewer.Visible = true;
                                    divCrystalReportViewer.Visible = true;
                                    divNodata.Visible = false;
                                    hdfAccount.Value = Convert.ToString(PK);
                                    //if (RptSubType == 0)
                                    //    lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedger_Report").ToString();
                                    ////if (!ValidateForm())
                                    ////{
                                    ////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                    ////}
                                    //else
                                    //{

                                    GetSelectedAccounts();
                                    SetFieldValuesCrystal(RptType, reportName[0].ToString());
                                    //}
                                    break;
                                case ApplicationType.SFG:
                                case ApplicationType.SFGCD:
                                case ApplicationType.SFGBL:
                                    divReportViewer.Visible = true;
                                    divCrystalReportViewer.Visible = true;
                                    divNodata.Visible = false;
                                    //hdfAccount.Value = Convert.ToString(PK);
                                    //if (RptSubType == 0)
                                    //    lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedger_Report").ToString();
                                    ////if (!ValidateForm())
                                    ////{
                                    ////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                    ////}
                                    //else
                                    //{


                                    SetFieldValuesCrystal(RptType, reportName[0].ToString());
                                    //}
                                    break;
                                case ApplicationType.SFGD:
                                case ApplicationType.SFGCDSP:
                                case ApplicationType.SFGBDSP:
                                    divReportViewer.Visible = true;
                                    divCrystalReportViewer.Visible = true;
                                    divNodata.Visible = false;
                                    //hdfAccount.Value = Convert.ToString(PK);
                                    //if (RptSubType == 0)
                                    //    lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedger_Report").ToString();
                                    ////if (!ValidateForm())
                                    ////{
                                    ////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                    ////}
                                    //else
                                    //{


                                    SetFieldValuesCrystal(RptType, reportName[0].ToString());
                                    //}
                                    break;
                            }
                            break;
                        #endregion
                        #region RDLC Calling
                        case ReportType.RDLCReport:
                            switch (RptType)
                            {
                                case ApplicationType.MI:
                                case ApplicationType.STA:
                                case ApplicationType.EMI:
                                case ApplicationType.EMR:
                                case ApplicationType.GRN:
                                case ApplicationType.GIN:
                                case ApplicationType.MRT:
                                case ApplicationType.MTI:

                                    GetFieldValues(RptType);
                                    SetFieldValues(RptType);
                                    break;
                                case ApplicationType.COMR:
                                    if (Request.QueryString["SbuID"] != null && Request.QueryString["SbuID"].ToString().Trim() != string.Empty)
                                    {
                                        SbuID = Convert.ToInt32(Request.QueryString["SbuID"]);
                                    }
                                    startOfMonth = Convert.ToDateTime(Request.QueryString["FROMDATE"].ToString());
                                    endOfMonth = Convert.ToDateTime(Request.QueryString["TODATE"].ToString());
                                    ProSize = Convert.ToInt32(Request.QueryString["ProSize"]);
                                    dsCompoundUsage = BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetCompoundUsageSummary(RecPK, SbuID, startOfMonth, endOfMonth, ProSize);
                                    SetFieldValues(RptType);
                                    break;

                                case ApplicationType.CDC:
                                    if (Request.QueryString["SbuID"] != null && Request.QueryString["SbuID"].ToString().Trim() != string.Empty)
                                    {
                                        SbuID = Convert.ToInt32(Request.QueryString["SbuID"]);
                                    }
                                    startOfMonth = Convert.ToDateTime(Request.QueryString["FROMDATE"].ToString());
                                    endOfMonth = Convert.ToDateTime(Request.QueryString["TODATE"].ToString());
                                    //ProSize = Convert.ToInt32(Request.QueryString["ProSize"]);
                                    dsCompoundUsage = BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetCompoundYieldCost(RecPK, SbuID, startOfMonth, endOfMonth);
                                    SetFieldValues(RptType);
                                    break;

                            }
                            break;
                            #endregion
                    }
                }
                #endregion
            }
        }
        #endregion

        #region Half Page Print

        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }
        private void Export(LocalReport report)
        {
            //ScriptManager sm = (ScriptManager)Master.FindControl("scrMenu");
            //sm.RegisterScriptControl(rvViewReport);
            try
            {
                rvViewReport.Visible = false;
                ScriptManager sm = ScriptManager.GetCurrent(Page);
                sm.RegisterScriptControl(rvViewReport);
                SetFieldValues(RptType);
                StringWriter sw = new StringWriter();

                HtmlTextWriter hw = new HtmlTextWriter(sw);
                rvViewReport.RenderControl(hw);
                string gridHTML = sw.ToString().Replace("\"", "'").Replace(System.Environment.NewLine, "");

                StringBuilder sb = new StringBuilder();

                sb.Append("<script type = 'text/javascript'>");

                sb.Append("window.onload = new function(){");

                sb.Append("var printWin = window.open('', '', 'left=0");

                sb.Append(",top=0,width=1000,height=600,status=0');");

                sb.Append("printWin.document.write(\"");

                sb.Append(gridHTML);

                sb.Append("\");");

                sb.Append("printWin.document.close();");

                sb.Append("printWin.focus();");

                sb.Append("printWin.print();");

                sb.Append("printWin.close();};");

                sb.Append("</script>");

                ClientScript.RegisterStartupScript(this.GetType(), "GridPrint", sb.ToString());
                rvViewReport.Visible = true;
            }
            catch (Exception ex)
            {

            }

            //            string deviceInfo =
            //              @"<DeviceInfo>
            //                <OutputFormat>EMF</OutputFormat>
            //                <PageWidth>8.27in</PageWidth>
            //                <PageHeight>5.85in</PageHeight>
            //                <MarginTop>0.25in</MarginTop>
            //                <MarginLeft>0.25in</MarginLeft>
            //                <MarginRight>0.25in</MarginRight>
            //                <MarginBottom>0.25in</MarginBottom>
            //            </DeviceInfo>";
            //            Warning[] warnings;
            //            m_streams = new List<Stream>();
            //            report.Render("Image", deviceInfo, CreateStream,
            //               out warnings);
            //            foreach (Stream stream in m_streams)
            //                stream.Position = 0;
        }
        private void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                PageSettings sd = new PageSettings();
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            System.Drawing.Rectangle adjustedRect = new System.Drawing.Rectangle(12, ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY, ev.PageBounds.Width, ev.PageBounds.Height);
            adjustedRect.Height = 580;
            adjustedRect.Width = 800;
            // Draw a white background for the report
            //ev.Graphics.FillRectangle(Brushes.White, adjustedRect);
            ev.Graphics.FillRectangle(Brushes.White, 0, 0, 800, 580);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }
        #endregion

        #region Set Field Values Crystal Report
        private void SetFieldValuesCrystal(string appType, string reportName)
        {
            try
            {
                hdfAppTypeRpt.Value = appType;
                hdfRptNameRpt.Value = reportName;
                ClearCrystalReport();
                reportDocument = null;
                GC.Collect();
                reportDocument = new ReportDocument();
                reportDocument.Load(Server.MapPath("~/Reports/CrystalReportFiles/" + reportName));
                ApplyLedgerCrystalFormulaFixes(reportName);
                reportDocument.Refresh();
                ReportFile = reportName;
                ERP.Utilities.CommonFunctions.SetCrystalReportDataBaseConnection(reportDocument);
                dsFinance = new DataSet();
                switch (appType)
                {
                    #region AS
                    case ApplicationType.AS:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        DataRow dr;
                        divAccPopUp.Visible = false;
                        dr = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        if (usrDateFilter.Visible)
                        {
                            dr["FROM_DATE"] = usrDateFilter.FromDate;
                            dr["TO_DATE"] = usrDateFilter.ToDate;
                        }
                        else
                        {
                            dr["FROM_DATE"] = txtFromDate.Text.Trim();
                            dr["TO_DATE"] = txtToDate.Text.Trim();
                        }
                        dr["BIZUNIT"] = currentUser.SBUID;
                        dr["CURRENCY"] = "";
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(dr);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                if (RptSubType == 0 || RptSubType == 3)
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerData(paramXml);
                                    dtData1.Columns.Add("CUR_DEPT", typeof(int));
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }
                                    /*List<SPFIN_ACCOUNT_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_ACCOUNT_STATEMENT_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }*/
                                }
                                else
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerConsolidatedData(paramXml);
                                    dtData1.Columns.Add("CUR_DEPT");
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }
                                    /*List<SPFIN_ACC_STMT_CONS_RPT_Result> AccList = currentEntity.SPFIN_ACC_STMT_CONS_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }*/
                                }
                            }
                        }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region GLFIN
                    case ApplicationType.GLFIN:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        //DataRow dr;
                        divAccPopUp.Visible = false;
                        dr = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        if (usrDateFilter.Visible)
                        {
                            dr["FROM_DATE"] = usrDateFilter.FromDate;
                            dr["TO_DATE"] = usrDateFilter.ToDate;
                            dr["FINYEAR"] = usrDateFilter.FinYear;
                        }
                        else
                        {
                            dr["FROM_DATE"] = txtFromDate.Text.Trim();
                            dr["TO_DATE"] = txtToDate.Text.Trim();
                            dr["FINYEAR"] = ddlFinYear.SelectedValue;
                        }
                        dr["BIZUNIT"] = currentUser.SBUID;
                        dr["CURRENCY"] = "";
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(dr);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                if (RptSubType == 0 || RptSubType == 3)
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerFinYearData(paramXml);
                                    dtData1.Columns.Add("CUR_DEPT", typeof(int));
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }

                                }
                                else
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerConsolidatedData(paramXml);
                                    dtData1.Columns.Add("CUR_DEPT");
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }

                                }
                            }
                        }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region COL
                    case ApplicationType.COL:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        startOfMonth = Convert.ToDateTime(Request.QueryString["FROMDATE"].ToString());
                        endOfMonth = Convert.ToDateTime(Request.QueryString["TODATE"].ToString());
                        string Pxml = string.Empty;
                        if (Session[ERP.Utilities.SessionStrings.CostCenterParamsSession].ToString() != null)
                        {
                            Pxml = Session[ERP.Utilities.SessionStrings.CostCenterParamsSession].ToString();
                        }
                        if (PK > 0)
                            if (RptSubType == 0 || RptSubType == 3)
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetCostCenterLedgerData(PK, startOfMonth, endOfMonth, Pxml);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "COSTCENTER";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                            else
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetCostCenterLedgerData(RecPK, startOfMonth, endOfMonth, Pxml);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "COSTCENTER";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region SFG
                    case ApplicationType.SFG:
                    case ApplicationType.SFGCD:
                    case ApplicationType.SFGBL:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        startOfMonth = Convert.ToDateTime(Request.QueryString["FROMDATE"].ToString());
                        endOfMonth = Convert.ToDateTime(Request.QueryString["TODATE"].ToString());
                        //string Pmxml = string.Empty;
                        //if (Session[ERP.Utilities.SessionStrings.SFGParamsSession].ToString() != null)
                        //{
                        //    Pmxml = Session[ERP.Utilities.SessionStrings.SFGParamsSession].ToString();
                        //}
                        if (PK > 0)
                            if (RptSubType == 0)
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetSFGData(PK, startOfMonth, endOfMonth);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "SFGDtls";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                            else
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetSFGData(RecPK, startOfMonth, endOfMonth);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "SFGDtls";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region SFGD
                    case ApplicationType.SFGD:
                    case ApplicationType.SFGCDSP:
                    case ApplicationType.SFGBDSP:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        startOfMonth = Convert.ToDateTime(Request.QueryString["FROMDATE"].ToString());
                        endOfMonth = Convert.ToDateTime(Request.QueryString["TODATE"].ToString());
                        //string Pmxml = string.Empty;
                        //if (Session[ERP.Utilities.SessionStrings.SFGParamsSession].ToString() != null)
                        //{
                        //    Pmxml = Session[ERP.Utilities.SessionStrings.SFGParamsSession].ToString();
                        //}
                        if (PK > 0)
                            if (RptSubType == 0)
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetSFGDISPData(PK, startOfMonth, endOfMonth);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "SFGDtls";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                            else
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetSFGDISPData(RecPK, startOfMonth, endOfMonth);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "SFGDtls";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region SAS
                    case ApplicationType.SAS:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        //DataRow dr;
                        divAccPopUp.Visible = false;
                        dr = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        if (usrDateFilter.Visible)
                        {
                            dr["FROM_DATE"] = usrDateFilter.FromDate;
                            dr["TO_DATE"] = usrDateFilter.ToDate;
                        }
                        else
                        {
                            dr["FROM_DATE"] = txtFromDate.Text.Trim();
                            dr["TO_DATE"] = txtToDate.Text.Trim();
                        }
                        dr["BIZUNIT"] = currentUser.SBUID;
                        dr["CURRENCY"] = "";
                        dr["CST_PK"] = ddlSubLedger.SelectedValue;
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(dr);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                if (RptSubType == 0 || RptSubType == 3)
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetSubLedgerData(paramXml);
                                    //dtData1.Columns.Add("CUR_DEPT");
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }
                                    /*List<SPFIN_SUB_ACCOUNT_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_SUB_ACCOUNT_STATEMENT_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }*/
                                }
                                else
                                {
                                    List<SPFIN_SUB_ACCOUNT_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_SUB_ACCOUNT_STATEMENT_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                AccList[i].CUR_DEPT = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }
                                }
                            }
                        }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region PSAS
                    case ApplicationType.PSAS:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        //DataRow dr;
                        divAccPopUp.Visible = false;
                        dr = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        if (usrDateFilter.Visible)
                        {
                            dr["FROM_DATE"] = usrDateFilter.FromDate;
                            dr["TO_DATE"] = usrDateFilter.ToDate;
                        }
                        else
                        {
                            dr["FROM_DATE"] = txtFromDate.Text.Trim();
                            dr["TO_DATE"] = txtToDate.Text.Trim();
                        }
                        dr["BIZUNIT"] = currentUser.SBUID;
                        dr["CURRENCY"] = "";
                        dr["PARTY_TYPE"] = hdfSendTo.Value;
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(dr);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                if (RptSubType == 0 || RptSubType == 3)
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetPartyLedgerData(paramXml);
                                    //dtData1.Columns.Add("CUR_DEPT");
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }
                                    /*List<SPFIN_PARTY_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_PARTY_STATEMENT_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }*/
                                }
                                else
                                {
                                    List<SPFIN_PARTY_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_PARTY_STATEMENT_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                AccList[i].CUR_DEPT = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }
                                }
                            }
                        }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion


                    #region TB
                    case ApplicationType.TB:
                        currentEntity = new ERPEntities();
                        List<SPFIN_TRIAL_BALANCE_RPT_Result> TBList = currentEntity.SPFIN_TRIAL_BALANCE_RPT(currentUser.SBUID, Convert.ToDateTime(txtFromDate.Text.Trim()), Convert.ToDateTime(txtToDate.Text.Trim()), null).ToList();
                        if (TBList != null && TBList.Count > 0)
                        {
                            dsFinance.Tables.Add(TBList.ToDataTable());
                            dsFinance.Tables[0].TableName = "TBDtls";
                        }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                        #endregion

                }

                if (dsFinance.Tables.Count <= 0)
                {
                    divCrystalReportViewer.Visible = false;
                    GERP_Report.Visible = false;
                    divNodata.Visible = true;
                }

                reportDocument.SetDataSource(dsFinance);
                ParameterFieldDefinitions crParameterdef;
                crParameterdef = reportDocument.DataDefinition.ParameterFields;
                rptParamFields = SetCrystalreportParameters(crParameterdef);
                GERP_Report.ReportSource = reportDocument;
                GERP_Report.ParameterFieldInfo = rptParamFields;
                hdfShowCrReportDiv.Value = "1";
                //Keep Parameter and report data for page navigation
                if (Request.QueryString[QueryStrings.FromExt] == null)
                {
                    Session[ERP.Utilities.SessionStrings.CRReportParam] = rptParamFields;
                    Session[ERP.Utilities.SessionStrings.CRReportData] = reportDocument;
                }
                //  If Request From Same Page otherthan Menu or Inbox
                else
                {
                    Session[ERP.Utilities.SessionStrings.CRReportParamFromExt] = rptParamFields;
                    Session[ERP.Utilities.SessionStrings.CRReportDataFromExt] = reportDocument;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private string GetCurrentCrystalReportName()
        {
            if (!string.IsNullOrEmpty(ReportFile))
                return ReportFile;

            if (!string.IsNullOrEmpty(hdfRptNameRpt.Value))
                return hdfRptNameRpt.Value;

            return string.Empty;
        }

        private void ApplyLedgerCrystalFormulaFixes(string reportName)
        {
            if (!IsLedgerCrystalReportWithUflFix(reportName))
                return;

            if (reportDocument == null)
                return;

            try
            {
                int fixedFormulaCount = ApplyLedgerUflFormulaFixes(reportDocument, reportName);
                fixedFormulaCount += RemovePartyLedgerDisplayStringConditionFormulas(reportDocument, reportName);
                foreach (ReportDocument subReport in reportDocument.Subreports)
                {
                    fixedFormulaCount += ApplyLedgerUflFormulaFixes(subReport, reportName);
                    fixedFormulaCount += RemovePartyLedgerDisplayStringConditionFormulas(subReport, reportName);
                }

                if (fixedFormulaCount > 0)
                {
                    CommonBL.ExceptionWriting("Ledger Crystal UFL formulas overridden: " + fixedFormulaCount, "Crystal formula fix applied : " + reportName);
                }
            }
            catch (Exception ex)
            {
                CommonBL.ExceptionWriting(ex.ToString(), "Ledger Crystal formula fix failed : " + reportName);
            }
        }

        private bool IsLedgerCrystalReportWithUflFix(string reportName)
        {
            string crystalReportName = Path.GetFileName(reportName);
            return string.Equals(crystalReportName, "AS_IGPL.rpt", StringComparison.OrdinalIgnoreCase)
                || crystalReportName.StartsWith("PartyLedger", StringComparison.OrdinalIgnoreCase);
        }

        private int ApplyLedgerUflFormulaFixes(ReportDocument crystalReport, string reportName)
        {
            int fixedFormulaCount = 0;
            foreach (FormulaFieldDefinition formulaField in crystalReport.DataDefinition.FormulaFields)
            {
                string formulaName = (formulaField.Name ?? string.Empty).TrimStart('@').Trim();
                string formulaText = formulaField.Text ?? string.Empty;

                string fixedFormulaText = formulaText;
                if (formulaText.IndexOf("CSGtiLibraryGtiLibraryUflHtmlDecode", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    fixedFormulaText = System.Text.RegularExpressions.Regex.Replace(
                        fixedFormulaText,
                        @"CSGtiLibraryGtiLibraryUflHtmlDecode\s*\(\s*(\{[^}]+\})\s*\)",
                        "$1",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }

                if (Path.GetFileName(reportName).StartsWith("PartyLedger", StringComparison.OrdinalIgnoreCase)
                    && string.Equals(formulaName, "Display_String", StringComparison.OrdinalIgnoreCase))
                {
                    fixedFormulaText = "\"\"";
                }

                if (!string.Equals(formulaText, fixedFormulaText, StringComparison.Ordinal))
                {
                    formulaField.Text = fixedFormulaText;
                    fixedFormulaCount++;
                }
            }

            return fixedFormulaCount;
        }

        private int RemovePartyLedgerDisplayStringConditionFormulas(ReportDocument crystalReport, string reportName)
        {
            if (!Path.GetFileName(reportName).StartsWith("PartyLedger", StringComparison.OrdinalIgnoreCase))
                return 0;

            int fixedFormulaCount = 0;

            try
            {
                object reportClientDocument = crystalReport.GetType().GetProperty("ReportClientDocument").GetValue(crystalReport, null);
                object reportDefController = reportClientDocument.GetType().GetProperty("ReportDefController").GetValue(reportClientDocument, null);
                object reportObjectController = reportDefController.GetType().GetProperty("ReportObjectController").GetValue(reportDefController, null);
                object reportObjects = reportObjectController.GetType().GetMethod("GetAllReportObjects").Invoke(reportObjectController, null);
                int reportObjectCount = Convert.ToInt32(reportObjects.GetType().GetProperty("Count").GetValue(reportObjects, null));
                Type conditionFormulaType = Type.GetType("CrystalDecisions.ReportAppServer.ReportDefModel.CrObjectFormatConditionFormulaTypeEnum, CrystalDecisions.ReportAppServer.ReportDefModel");
                object displayStringFormulaType = Enum.ToObject(conditionFormulaType, 9);

                for (int i = 0; i < reportObjectCount; i++)
                {
                    object reportObject = reportObjects.GetType().GetProperty("Item").GetValue(reportObjects, new object[] { i });
                    fixedFormulaCount += RemovePartyLedgerDisplayStringConditionFormula(reportObjectController, reportObject, displayStringFormulaType);
                }
            }
            catch
            {
                return fixedFormulaCount;
            }

            return fixedFormulaCount;
        }

        private int RemovePartyLedgerDisplayStringConditionFormula(object reportObjectController, object reportObject, object displayStringFormulaType)
        {
            try
            {
                object format = reportObject.GetType().GetProperty("Format").GetValue(reportObject, null);
                object conditionFormulas = format.GetType().GetProperty("ConditionFormulas").GetValue(format, null);
                object displayStringFormula = conditionFormulas.GetType().GetProperty("Formula").GetValue(conditionFormulas, new object[] { displayStringFormulaType });

                if (displayStringFormula == null)
                    return 0;

                string formulaText = Convert.ToString(displayStringFormula.GetType().GetProperty("Text").GetValue(displayStringFormula, null));
                if (string.IsNullOrEmpty(formulaText))
                    return 0;

                object fixedReportObject = reportObject.GetType().GetMethod("Clone").Invoke(reportObject, new object[] { true });
                object fixedFormat = fixedReportObject.GetType().GetProperty("Format").GetValue(fixedReportObject, null);
                object fixedConditionFormulas = fixedFormat.GetType().GetProperty("ConditionFormulas").GetValue(fixedFormat, null);
                fixedConditionFormulas.GetType().GetMethod("RemoveFormula").Invoke(fixedConditionFormulas, new object[] { displayStringFormulaType });

                reportObjectController.GetType().GetMethod("Modify").Invoke(reportObjectController, new object[] { reportObject, fixedReportObject });
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        #region Excel Export Crystal Report CODE
        private void SetFieldValuesCrystalEXCEL(string appType, string reportName)
        {
            try
            {
                hdfAppTypeRpt.Value = appType;
                hdfRptNameRpt.Value = reportName;
                ClearCrystalReport();
                reportDocument = null;
                GC.Collect();
                reportDocument = new ReportDocument();
                reportDocument.Load(Server.MapPath("~/Reports/CrystalReportFiles/" + reportName));
                ApplyLedgerCrystalFormulaFixes(reportName);
                reportDocument.Refresh();
                ReportFile = reportName;
                ERP.Utilities.CommonFunctions.SetCrystalReportDataBaseConnection(reportDocument);
                dsFinance = new DataSet();
                switch (appType)
                {
                    #region AS
                    case ApplicationType.AS:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        DataRow dr;
                        divAccPopUp.Visible = false;
                        dr = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        if (usrDateFilter.Visible)
                        {
                            dr["FROM_DATE"] = usrDateFilter.FromDate;
                            dr["TO_DATE"] = usrDateFilter.ToDate;
                        }
                        else
                        {
                            dr["FROM_DATE"] = txtFromDate.Text.Trim();
                            dr["TO_DATE"] = txtToDate.Text.Trim();
                        }
                        dr["BIZUNIT"] = currentUser.SBUID;
                        dr["CURRENCY"] = "";
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(dr);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                if (RptSubType == 0 || RptSubType == 3)
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerData(paramXml);
                                    dtData1.Columns.Add("CUR_DEPT", typeof(int));
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }
                                    /*List<SPFIN_ACCOUNT_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_ACCOUNT_STATEMENT_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }*/
                                }
                                else
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerConsolidatedData(paramXml);
                                    dtData1.Columns.Add("CUR_DEPT");
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }
                                    /*List<SPFIN_ACC_STMT_CONS_RPT_Result> AccList = currentEntity.SPFIN_ACC_STMT_CONS_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }*/
                                }
                            }
                        }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region GLFIN
                    case ApplicationType.GLFIN:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        //DataRow dr;
                        divAccPopUp.Visible = false;
                        dr = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        if (usrDateFilter.Visible)
                        {
                            dr["FROM_DATE"] = usrDateFilter.FromDate;
                            dr["TO_DATE"] = usrDateFilter.ToDate;
                            dr["FINYEAR"] = usrDateFilter.FinYear;
                        }
                        else
                        {
                            dr["FROM_DATE"] = txtFromDate.Text.Trim();
                            dr["TO_DATE"] = txtToDate.Text.Trim();
                            dr["FINYEAR"] = ddlFinYear.SelectedValue;
                        }
                        dr["BIZUNIT"] = currentUser.SBUID;
                        dr["CURRENCY"] = "";
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(dr);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                if (RptSubType == 0 || RptSubType == 3)
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerFinYearData(paramXml);
                                    dtData1.Columns.Add("CUR_DEPT", typeof(int));
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }

                                }
                                else
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetGeneralLedgerConsolidatedData(paramXml);
                                    dtData1.Columns.Add("CUR_DEPT");
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }

                                }
                            }
                        }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region COL
                    case ApplicationType.COL:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        startOfMonth = Convert.ToDateTime(Request.QueryString["FROMDATE"].ToString());
                        endOfMonth = Convert.ToDateTime(Request.QueryString["TODATE"].ToString());
                        string Pxml = string.Empty;
                        if (Session[ERP.Utilities.SessionStrings.CostCenterParamsSession].ToString() != null)
                        {
                            Pxml = Session[ERP.Utilities.SessionStrings.CostCenterParamsSession].ToString();
                        }
                        if (PK > 0)
                            if (RptSubType == 0 || RptSubType == 3)
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetCostCenterLedgerData(PK, startOfMonth, endOfMonth, Pxml);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "COSTCENTER";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                            else
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetCostCenterLedgerData(RecPK, startOfMonth, endOfMonth, Pxml);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "COSTCENTER";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region SFG
                    case ApplicationType.SFG:
                    case ApplicationType.SFGCD:
                    case ApplicationType.SFGBL:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        startOfMonth = Convert.ToDateTime(Request.QueryString["FROMDATE"].ToString());
                        endOfMonth = Convert.ToDateTime(Request.QueryString["TODATE"].ToString());
                        //string Pmxml = string.Empty;
                        //if (Session[ERP.Utilities.SessionStrings.SFGParamsSession].ToString() != null)
                        //{
                        //    Pmxml = Session[ERP.Utilities.SessionStrings.SFGParamsSession].ToString();
                        //}
                        if (PK > 0)
                            if (RptSubType == 0)
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetSFGData(PK, startOfMonth, endOfMonth);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "SFGDtls";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                            else
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetSFGData(RecPK, startOfMonth, endOfMonth);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "SFGDtls";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region SFGD
                    case ApplicationType.SFGD:
                    case ApplicationType.SFGCDSP:
                    case ApplicationType.SFGBDSP:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        startOfMonth = Convert.ToDateTime(Request.QueryString["FROMDATE"].ToString());
                        endOfMonth = Convert.ToDateTime(Request.QueryString["TODATE"].ToString());
                        //string Pmxml = string.Empty;
                        //if (Session[ERP.Utilities.SessionStrings.SFGParamsSession].ToString() != null)
                        //{
                        //    Pmxml = Session[ERP.Utilities.SessionStrings.SFGParamsSession].ToString();
                        //}
                        if (PK > 0)
                            if (RptSubType == 0)
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetSFGDISPData(PK, startOfMonth, endOfMonth);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "SFGDtls";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                            else
                            {
                                DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetSFGDISPData(RecPK, startOfMonth, endOfMonth);
                                dsFinance = new DataSet();
                                if (dtData1 != null && dtData1.Rows.Count > 0)
                                {
                                    dtData1.TableName = "SFGDtls";
                                    dsFinance.Tables.Add(dtData1.Copy());
                                }
                            }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion
                    #region SAS
                    case ApplicationType.SAS:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        //DataRow dr;
                        divAccPopUp.Visible = false;
                        dr = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        if (usrDateFilter.Visible)
                        {
                            dr["FROM_DATE"] = usrDateFilter.FromDate;
                            dr["TO_DATE"] = usrDateFilter.ToDate;
                        }
                        else
                        {
                            dr["FROM_DATE"] = txtFromDate.Text.Trim();
                            dr["TO_DATE"] = txtToDate.Text.Trim();
                        }
                        dr["BIZUNIT"] = currentUser.SBUID;
                        dr["CURRENCY"] = "";
                        dr["CST_PK"] = ddlSubLedger.SelectedValue;
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(dr);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                if (RptSubType == 0 || RptSubType == 3)
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetSubLedgerData(paramXml);
                                    //dtData1.Columns.Add("CUR_DEPT");
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }
                                    /*List<SPFIN_SUB_ACCOUNT_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_SUB_ACCOUNT_STATEMENT_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }*/
                                }
                                else
                                {
                                    List<SPFIN_SUB_ACCOUNT_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_SUB_ACCOUNT_STATEMENT_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                AccList[i].CUR_DEPT = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }
                                }
                            }
                        }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region PSAS
                    case ApplicationType.PSAS:
                        CheckUserRight("/reports/generatereport.aspx?id=&apptype=as&appsubtype=");
                        //DataRow dr;
                        divAccPopUp.Visible = false;
                        dr = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        if (usrDateFilter.Visible)
                        {
                            dr["FROM_DATE"] = usrDateFilter.FromDate;
                            dr["TO_DATE"] = usrDateFilter.ToDate;
                        }
                        else
                        {
                            dr["FROM_DATE"] = txtFromDate.Text.Trim();
                            dr["TO_DATE"] = txtToDate.Text.Trim();
                        }
                        dr["BIZUNIT"] = currentUser.SBUID;
                        dr["CURRENCY"] = "";
                        dr["PARTY_TYPE"] = hdfSendTo.Value;
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(dr);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();
                                if (RptSubType == 0 || RptSubType == 3)
                                {
                                    DataTable dtData1 = BusinessLogic.ReportsManagement.GenerateReportBL.GetPartyLedgerData(paramXml);
                                    //dtData1.Columns.Add("CUR_DEPT");
                                    dsFinance = new DataSet();
                                    if (dtData1 != null && dtData1.Rows.Count > 0)
                                    {
                                        for (int i = 0; i <= dtData1.Rows.Count - 1; i++)
                                        {
                                            if (dtData1.Rows[i]["FTH_NARRATION"].ToString() == "Balance B/F")
                                            {
                                                BalanceCr = (dtData1.Rows[i]["COA_CR"] != null && dtData1.Rows[i]["COA_CR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_CR"]) + BalanceCr : 0 + BalanceCr;
                                                BalanceDr = (dtData1.Rows[i]["COA_DR"] != null && dtData1.Rows[i]["COA_DR"].ToString() != string.Empty) ? Convert.ToDouble(dtData1.Rows[i]["COA_DR"]) + BalanceDr : 0 + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                dtData1.Rows[i]["CUR_DEPT"] = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }

                                        dtData1.TableName = "AccList";
                                        dsFinance.Tables.Add(dtData1.Copy());
                                    }
                                    /*List<SPFIN_PARTY_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_PARTY_STATEMENT_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }*/
                                }
                                else
                                {
                                    List<SPFIN_PARTY_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_PARTY_STATEMENT_RPT(paramXml).ToList();
                                    if (AccList != null && AccList.Count > 0)
                                    {
                                        for (int i = 0; i <= AccList.Count - 1; i++)
                                        {
                                            if (AccList[i].FTH_NARRATION == "Balance B/F")
                                            {
                                                BalanceCr = AccList[i].COA_CR + BalanceCr;
                                                BalanceDr = AccList[i].COA_DR + BalanceDr;
                                            }
                                            if (Request.QueryString["Dep"] != null)
                                            {
                                                AccList[i].CUR_DEPT = Convert.ToInt32(Request.QueryString["Dep"]);
                                            }
                                        }
                                        dsFinance.Tables.Add(AccList.ToDataTable());
                                        dsFinance.Tables[0].TableName = "AccList";
                                    }
                                }
                            }
                        }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion


                    #region TB
                    case ApplicationType.TB:
                        currentEntity = new ERPEntities();
                        List<SPFIN_TRIAL_BALANCE_RPT_Result> TBList = currentEntity.SPFIN_TRIAL_BALANCE_RPT(currentUser.SBUID, Convert.ToDateTime(txtFromDate.Text.Trim()), Convert.ToDateTime(txtToDate.Text.Trim()), null).ToList();
                        if (TBList != null && TBList.Count > 0)
                        {
                            dsFinance.Tables.Add(TBList.ToDataTable());
                            dsFinance.Tables[0].TableName = "TBDtls";
                        }
                        else
                        {
                            divCrystalReportViewer.Visible = false;
                            GERP_Report.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                        #endregion

                }

                if (dsFinance.Tables.Count <= 0)
                {
                    divCrystalReportViewer.Visible = false;
                    GERP_Report.Visible = false;
                    divNodata.Visible = true;
                }

                reportDocument.SetDataSource(dsFinance);
                ParameterFieldDefinitions crParameterdef;
                crParameterdef = reportDocument.DataDefinition.ParameterFields;
                rptParamFields = SetCrystalreportParameters(crParameterdef);
                GERP_Report.ReportSource = reportDocument;
                GERP_Report.ParameterFieldInfo = rptParamFields;
                hdfShowCrReportDiv.Value = "1";
                //Keep Parameter and report data for page navigation
                if (Request.QueryString[QueryStrings.FromExt] == null)
                {
                    Session[ERP.Utilities.SessionStrings.CRReportParam] = rptParamFields;
                    Session[ERP.Utilities.SessionStrings.CRReportData] = reportDocument;
                }
                //  If Request From Same Page otherthan Menu or Inbox
                else
                {
                    Session[ERP.Utilities.SessionStrings.CRReportParamFromExt] = rptParamFields;
                    Session[ERP.Utilities.SessionStrings.CRReportDataFromExt] = reportDocument;
                }
                foreach (ParameterField rptParamField in rptParamFields)
                {
                    ParameterFieldDefinition rptParamDef = reportDocument.DataDefinition.ParameterFields[rptParamField.Name];

                    if (rptParamDef != null)
                    {
                        rptParamDef.ApplyCurrentValues(rptParamField.CurrentValues);
                    }
                }

                ExportOptions exportOptions = new ExportOptions();
                ExcelFormatOptions excelOptions = new ExcelFormatOptions();
                exportOptions.ExportFormatType = ExportFormatType.ExcelWorkbook;
                exportOptions.FormatOptions = excelOptions;

                // Specify a file path to temporarily save the Excel file
                string excelFilePath = Server.MapPath("~/Reports/report.xlsx");

                // Export the report to the Excel file
                reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, excelFilePath);


                // Provide a download link to the user
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=report.xlsx");
                Response.TransmitFile(excelFilePath);
                Response.End();




            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region SetCrystalreportCommonParameters
        private void SetCrystalreportCommonParameters(ReportDocument rptDocument, string rptPK)
        {

            #region Extra Parameter based on query string
            //if (RptType == ApplicationType.TRACE && RptSubType == 2)
            //{
            //    paramFields.Add(SetParamValue("BatchNo", BatchNo.ToString());
            //}
            #endregion
            ParameterFieldDefinitions crParameterdef;
            crParameterdef = reportDocument.DataDefinition.ParameterFields;
            DataSet dsParamSettings = new DataSet();
            string footer;
            string rptName = string.Empty;
            footer = string.Empty;
            try
            {
                if (AppTypeDetailsList != null && AppTypeDetailsList.Count > 0)
                {
                    var aST_RPT_SETTINGS = AppTypeDetailsList.Select(l => l.AST_RPT_SETTINGS).ToList();
                    if (aST_RPT_SETTINGS[0] != null)
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(aST_RPT_SETTINGS[0])));
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    int curdigit = 2;
                    int NoDigit = 2;
                    int ExchRate = 2;
                    int RateDecimal = 2;
                    int RateDecimalPP = 2;
                    int WeightDigit = 2;

                    int AvgWeightDecimalDigitPrd = 2;
                    int CurrencyNumberGroup1 = 2;
                    int CurrencyNumberGroup2 = 2;
                    int MiscRateDecimalDigit = 2;
                    int NumberDecimalDigitBin = 2;
                    int NumberDecimalDigitCompounding = 2;
                    int NumberDecimalDigitConstruction = 2;
                    int NumberDecimalDigitP2P = 2;
                    int NumberGroup1 = 2;
                    int NumberGroup2 = 2;
                    int RateDecimalDigitConstruction = 2;
                    int ShowAmountInBC = 2;
                    int WeightDecimalDigitPrd = 2;

                    DataTable dt = ConfigurationSettings();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        curdigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                        NoDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());
                        ExchRate = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                        RateDecimal = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                        RateDecimalPP = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
                        WeightDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "WeightDecimalDigit")["ACF_VALUE"].ToString());
                        AvgWeightDecimalDigitPrd = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "AvgWeightDecimalDigitPrd")["ACF_VALUE"].ToString());
                        CurrencyNumberGroup1 = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyNumberGroup1")["ACF_VALUE"].ToString());
                        CurrencyNumberGroup2 = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "CurrencyNumberGroup2")["ACF_VALUE"].ToString());
                        MiscRateDecimalDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "MiscRateDecimalDigit")["ACF_VALUE"].ToString());
                        NumberDecimalDigitBin = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "NumberDecimalDigitBin")["ACF_VALUE"].ToString());
                        NumberDecimalDigitCompounding = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitCompounding")["ACF_VALUE"].ToString());
                        NumberDecimalDigitConstruction = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitConstruction")["ACF_VALUE"].ToString());
                        NumberDecimalDigitP2P = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitP2P")["ACF_VALUE"].ToString());
                        NumberGroup1 = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "NumberGroup1")["ACF_VALUE"].ToString());
                        NumberGroup2 = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "NumberGroup2")["ACF_VALUE"].ToString());
                        RateDecimalDigitConstruction = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitConstruction")["ACF_VALUE"].ToString());
                        ShowAmountInBC = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "ShowAmountInBC")["ACF_VALUE"].ToString());
                        WeightDecimalDigitPrd = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "WeightDecimalDigitPrd")["ACF_VALUE"].ToString());
                    }
                    foreach (ParameterFieldDefinition def in crParameterdef)
                    {
                        if (def.Name.Equals("CurrencyDigits"))
                            rptDocument.SetParameterValue("CurrencyDigits", curdigit.ToString());
                        if (def.Name.Equals("NumberDigits"))
                            rptDocument.SetParameterValue("NumberDigits", NoDigit.ToString());
                        if (def.Name.Equals("ExchangeRate"))
                            rptDocument.SetParameterValue("ExchangeRate", ExchRate.ToString());
                        if (def.Name.Equals("RateDigits"))
                            rptDocument.SetParameterValue("RateDigits", RateDecimal.ToString());
                        if (def.Name.Equals("WeightDigits"))
                            rptDocument.SetParameterValue("WeightDigits", WeightDigit.ToString());
                        if (def.Name.Equals("DateFormat"))
                            rptDocument.SetParameterValue("DateFormat", Resources.Constants.ReportDateFormat.ToString());
                        if (def.Name.Equals("AvgWeightDecimalDigitPrd"))
                            rptDocument.SetParameterValue("AvgWeightDecimalDigitPrd", AvgWeightDecimalDigitPrd.ToString());
                        if (def.Name.Equals("CurrencyNumberGroup1"))
                            rptDocument.SetParameterValue("CurrencyNumberGroup1", CurrencyNumberGroup1.ToString());
                        if (def.Name.Equals("CurrencyNumberGroup2"))
                            rptDocument.SetParameterValue("CurrencyNumberGroup2", CurrencyNumberGroup2.ToString());
                        if (def.Name.Equals("MiscRateDecimalDigit"))
                            rptDocument.SetParameterValue("MiscRateDecimalDigit", MiscRateDecimalDigit.ToString());
                        if (def.Name.Equals("NumberDecimalDigitBin"))
                            rptDocument.SetParameterValue("NumberDecimalDigitBin", WeightDigit.ToString());
                        if (def.Name.Equals("NumberDecimalDigitCompounding"))
                            rptDocument.SetParameterValue("NumberDecimalDigitCompounding", NumberDecimalDigitCompounding.ToString());
                        if (def.Name.Equals("NumberDecimalDigitConstruction"))
                            rptDocument.SetParameterValue("NumberDecimalDigitConstruction", NumberDecimalDigitConstruction.ToString());
                        if (def.Name.Equals("NumberDecimalDigitP2P"))
                            rptDocument.SetParameterValue("NumberDecimalDigitP2P", NumberDecimalDigitP2P.ToString());
                        if (def.Name.Equals("NumberGroup1"))
                            rptDocument.SetParameterValue("NumberGroup1", NumberGroup1.ToString());
                        if (def.Name.Equals("NumberGroup2"))
                            rptDocument.SetParameterValue("NumberGroup2", NumberGroup2.ToString());
                        if (def.Name.Equals("RateDecimalDigitConstruction"))
                            rptDocument.SetParameterValue("RateDecimalDigitConstruction", RateDecimalDigitConstruction.ToString());
                        if (def.Name.Equals("ShowAmountInBC"))
                            rptDocument.SetParameterValue("ShowAmountInBC", ShowAmountInBC.ToString());
                        if (def.Name.Equals("WeightDecimalDigitPrd"))
                            rptDocument.SetParameterValue("WeightDecimalDigitPrd", WeightDecimalDigitPrd.ToString());

                        #region AS
                        if (RptType == ApplicationType.AS || RptType == ApplicationType.GLFIN)
                        {
                            if (def.Name.Equals("BalanceCr"))
                            {
                                rptDocument.SetParameterValue("BalanceCr", BalanceCr.ToString());
                            }
                            if (def.Name.Equals("BalanceDr"))
                            {
                                rptDocument.SetParameterValue("BalanceDr", BalanceDr.ToString());
                            }
                        }
                        #endregion
                    }

                    if (dsParamSettings.Tables.Count > 0)
                    {
                        if (dsParamSettings.Tables[0].Columns.Contains("LOGO_HIDE"))
                        {
                            rptDocument.SetParameterValue("HideLogo", dsParamSettings.Tables[0].Rows[0]["LOGO_HIDE"].ToString());
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING_HIDE"))
                        {
                            rptDocument.SetParameterValue("HideHeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING_HIDE"].ToString());
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING_HIDE"))
                        {
                            rptDocument.SetParameterValue("HideSubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING_HIDE"].ToString());
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_LEFT_HIDE"))
                        {
                            rptDocument.SetParameterValue("HideFooterText", dsParamSettings.Tables[0].Rows[0]["FOOTER_LEFT_HIDE"].ToString());
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_RIGHT_HIDE"))
                        {
                            rptDocument.SetParameterValue("HidePageNo", dsParamSettings.Tables[0].Rows[0]["FOOTER_RIGHT_HIDE"].ToString());
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING"))
                        {
                            rptDocument.SetParameterValue("HeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString());
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING"))
                        {
                            rptDocument.SetParameterValue("SubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING"].ToString());
                        }
                    }
                }
                GetCompanyDetails(null);
                rptDocument.SetParameterValue("Logo", LogoPath);
                //rptDocument.SetParameterValue("Logo", Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoPDF"]));
                footer = string.Format(GetLocalResourceObject("FooterText").ToString(), currentUser.EmpName, DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));
                rptDocument.SetParameterValue("FooterText", footer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private ParameterFields SetCrystalreportParameters(ParameterFieldDefinitions crParameterdef)
        {
            ParameterFields paramFields = new ParameterFields();
            ParameterField paramField = new ParameterField();
            ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();

            #region Extra Parameter based on query string
            //if (RptType == ApplicationType.TRACE && RptSubType == 2)
            //{
            //    rptDocument.SetParameterValue("BatchNo", BatchNo.ToString());
            //}
            #endregion

            DataSet dsParamSettings = new DataSet();
            string footer;
            string rptName = string.Empty;
            footer = string.Empty;
            try
            {
                if (AppTypeDetailsList != null && AppTypeDetailsList.Count > 0)
                {
                    var aST_RPT_SETTINGS = AppTypeDetailsList.Select(l => l.AST_RPT_SETTINGS).ToList();
                    if (aST_RPT_SETTINGS[0] != null)
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(aST_RPT_SETTINGS[0])));
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    int curdigit = 2;
                    int NoDigit = 2;
                    int ExchRate = 2;
                    int RateDecimal = 2;
                    int RateDecimalPP = 2;
                    int WeightDigit = 2;

                    int AvgWeightDecimalDigitPrd = 2;
                    int CurrencyNumberGroup1 = 2;
                    int CurrencyNumberGroup2 = 2;
                    int MiscRateDecimalDigit = 2;
                    int NumberDecimalDigitBin = 2;
                    int NumberDecimalDigitCompounding = 2;
                    int NumberDecimalDigitConstruction = 2;
                    int NumberDecimalDigitP2P = 2;
                    int NumberGroup1 = 2;
                    int NumberGroup2 = 2;
                    int RateDecimalDigitConstruction = 2;
                    int ShowAmountInBC = 2;
                    int WeightDecimalDigitPrd = 2;

                    DataTable dt = ConfigurationSettings();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        curdigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                        NoDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());
                        ExchRate = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                        RateDecimal = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                        RateDecimalPP = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
                        WeightDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "WeightDecimalDigit")["ACF_VALUE"].ToString());


                        AvgWeightDecimalDigitPrd = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "AvgWeightDecimalDigitPrd")["ACF_VALUE"].ToString());
                        CurrencyNumberGroup1 = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyNumberGroup1")["ACF_VALUE"].ToString());
                        CurrencyNumberGroup2 = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "CurrencyNumberGroup2")["ACF_VALUE"].ToString());
                        MiscRateDecimalDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "MiscRateDecimalDigit")["ACF_VALUE"].ToString());
                        NumberDecimalDigitBin = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "NumberDecimalDigitBin")["ACF_VALUE"].ToString());
                        NumberDecimalDigitCompounding = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitCompounding")["ACF_VALUE"].ToString());
                        NumberDecimalDigitConstruction = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitConstruction")["ACF_VALUE"].ToString());
                        NumberDecimalDigitP2P = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigitP2P")["ACF_VALUE"].ToString());
                        NumberGroup1 = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "NumberGroup1")["ACF_VALUE"].ToString());
                        NumberGroup2 = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "NumberGroup2")["ACF_VALUE"].ToString());
                        RateDecimalDigitConstruction = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitConstruction")["ACF_VALUE"].ToString());
                        ShowAmountInBC = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "ShowAmountInBC")["ACF_VALUE"].ToString());
                        WeightDecimalDigitPrd = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "WeightDecimalDigitPrd")["ACF_VALUE"].ToString());
                    }
                    foreach (ParameterFieldDefinition def in crParameterdef)
                    {
                        if (usrDateFilter.Visible)
                        {
                            if (def.Name.Equals("FromDate"))
                            {
                                paramFields.Add(SetParamValue("FromDate", usrDateFilter.FromDate));
                            }
                            if (def.Name.Equals("ToDate"))
                            {
                                paramFields.Add(SetParamValue("ToDate", usrDateFilter.ToDate));
                            }
                        }
                        else
                        {
                            if (def.Name.Equals("FromDate"))
                            {
                                if (txtFromDate != null && !string.IsNullOrEmpty(txtFromDate.Text.Trim()))
                                    paramFields.Add(SetParamValue("FromDate", txtFromDate.Text.Trim()));
                            }
                            if (def.Name.Equals("ToDate"))
                            {
                                if (txtToDate != null && !string.IsNullOrEmpty(txtToDate.Text.Trim()))
                                    paramFields.Add(SetParamValue("ToDate", txtToDate.Text.Trim()));
                            }
                        }
                        if (def.Name.Equals("CurrencyDigits"))
                            paramFields.Add(SetParamValue("CurrencyDigits", curdigit.ToString()));
                        if (def.Name.Equals("NumberDigits"))
                            paramFields.Add(SetParamValue("NumberDigits", NoDigit.ToString()));
                        if (def.Name.Equals("ExchangeRate"))
                            paramFields.Add(SetParamValue("ExchangeRate", ExchRate.ToString()));
                        if (def.Name.Equals("RateDigits"))
                            paramFields.Add(SetParamValue("RateDigits", RateDecimal.ToString()));
                        if (def.Name.Equals("WeightDigits"))
                            paramFields.Add(SetParamValue("WeightDigits", WeightDigit.ToString()));
                        if (def.Name.Equals("DateFormat"))
                            paramFields.Add(SetParamValue("DateFormat", Resources.Constants.ReportDateFormat.ToString()));
                        if (def.Name.Equals("AvgWeightDecimalDigitPrd"))
                            paramFields.Add(SetParamValue("AvgWeightDecimalDigitPrd", AvgWeightDecimalDigitPrd.ToString()));
                        if (def.Name.Equals("CurrencyNumberGroup1"))
                            paramFields.Add(SetParamValue("CurrencyNumberGroup1", CurrencyNumberGroup1.ToString()));
                        if (def.Name.Equals("CurrencyNumberGroup2"))
                            paramFields.Add(SetParamValue("CurrencyNumberGroup2", CurrencyNumberGroup2.ToString()));
                        if (def.Name.Equals("MiscRateDecimalDigit"))
                            paramFields.Add(SetParamValue("MiscRateDecimalDigit", MiscRateDecimalDigit.ToString()));
                        if (def.Name.Equals("NumberDecimalDigitBin"))
                            paramFields.Add(SetParamValue("NumberDecimalDigitBin", WeightDigit.ToString()));
                        if (def.Name.Equals("NumberDecimalDigitCompounding"))
                            paramFields.Add(SetParamValue("NumberDecimalDigitCompounding", NumberDecimalDigitCompounding.ToString()));
                        if (def.Name.Equals("NumberDecimalDigitConstruction"))
                            paramFields.Add(SetParamValue("NumberDecimalDigitConstruction", NumberDecimalDigitConstruction.ToString()));
                        if (def.Name.Equals("NumberDecimalDigitP2P"))
                            paramFields.Add(SetParamValue("NumberDecimalDigitP2P", NumberDecimalDigitP2P.ToString()));
                        if (def.Name.Equals("NumberGroup1"))
                            paramFields.Add(SetParamValue("NumberGroup1", NumberGroup1.ToString()));
                        if (def.Name.Equals("NumberGroup2"))
                            paramFields.Add(SetParamValue("NumberGroup2", NumberGroup2.ToString()));
                        if (def.Name.Equals("RateDecimalDigitConstruction"))
                            paramFields.Add(SetParamValue("RateDecimalDigitConstruction", RateDecimalDigitConstruction.ToString()));
                        if (def.Name.Equals("ShowAmountInBC"))
                            paramFields.Add(SetParamValue("ShowAmountInBC", ShowAmountInBC.ToString()));
                        if (def.Name.Equals("WeightDecimalDigitPrd"))
                            paramFields.Add(SetParamValue("WeightDecimalDigitPrd", WeightDecimalDigitPrd.ToString()));
                        if (def.Name.Equals("CurrentDeptPK"))
                            paramFields.Add(SetParamValue("CurrentDeptPK", currentUser.CurrentDeptPK.ToString()));

                        #region AS
                        if (RptType == ApplicationType.AS || RptType == ApplicationType.SAS || RptType == ApplicationType.PSAS || RptType == ApplicationType.GLFIN)
                        {
                            if (def.Name.Equals("BalanceCr"))
                                paramFields.Add(SetParamValue("BalanceCr", BalanceCr.ToString()));
                            if (def.Name.Equals("BalanceDr"))
                                paramFields.Add(SetParamValue("BalanceDr", BalanceDr.ToString()));
                        }
                        #endregion
                    }

                    if (dsParamSettings.Tables.Count > 0)
                    {
                        if (dsParamSettings.Tables[0].Columns.Contains("LOGO_HIDE"))
                            paramFields.Add(SetParamValue("HideLogo", dsParamSettings.Tables[0].Rows[0]["LOGO_HIDE"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING_HIDE"))
                            paramFields.Add(SetParamValue("HideHeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING_HIDE"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING_HIDE"))
                            paramFields.Add(SetParamValue("HideSubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING_HIDE"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_LEFT_HIDE"))
                            paramFields.Add(SetParamValue("HideFooterText", dsParamSettings.Tables[0].Rows[0]["FOOTER_LEFT_HIDE"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_RIGHT_HIDE"))
                            paramFields.Add(SetParamValue("HidePageNo", dsParamSettings.Tables[0].Rows[0]["FOOTER_RIGHT_HIDE"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING"))
                            paramFields.Add(SetParamValue("HeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString()));
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING"))
                            paramFields.Add(SetParamValue("SubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING"].ToString()));
                    }
                }
                GetCompanyDetails(null);
                paramFields.Add(SetParamValue("Logo", LogoPath));
                // paramFields.Add(SetParamValue("Logo", Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoPDF"])));
                footer = string.Format(GetLocalResourceObject("FooterText").ToString(), currentUser.EmpName, DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));
                paramFields.Add(SetParamValue("FooterText", footer));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return paramFields;
        }
        /// <summary>
        /// Get formated list of objects for auto complete
        /// </summary>
        /// <param name="dynamicList">List for formatting</param>
        /// <param name="textProperty">Text property name</param>
        /// <param name="valueProperty">Value property name</param>
        /// <returns></returns>
        private List<DDLMaster> GetCheckListDataGet(DataTable dtData, string valueProperty, string textProperty)
        {
            List<DDLMaster> textValueList;
            try
            {
                textValueList = new List<DDLMaster>();
                foreach (DataRow dRow in dtData.Rows)
                {
                    textValueList.Add(new DDLMaster()
                    {
                        Value = HttpUtility.HtmlDecode((dRow[textProperty]).ToString()),
                        PK = Convert.ToInt32((dRow[valueProperty])),
                    });
                }
                return textValueList.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private string[] SplitThaiPndAddress(string localAddress)
        {
            string[] address = Enumerable.Repeat("1", 11).ToArray();
            if (string.IsNullOrEmpty(localAddress) || localAddress.Trim().Length == 0)
                return address;

            string text = localAddress.Trim();
            string[] lineSplitter = { Environment.NewLine };
            string[] lines = text.Split(lineSplitter, StringSplitOptions.None);
            if (lines.Length > 1)
            {
                for (int i = 0; i < address.Length && i < lines.Length; i++)
                    address[i] = CleanPndAddressValue(lines[i]);
                return address;
            }

            int posSoi = text.IndexOf("ซอย", StringComparison.Ordinal);
            int posRoad = text.IndexOf("ถนน", posSoi >= 0 ? posSoi + 1 : 0, StringComparison.Ordinal);
            int posSubDistrict = text.IndexOf("แขวง", StringComparison.Ordinal);
            int posDistrict = text.IndexOf("เขต", StringComparison.Ordinal);
            int posPostCode = FindThaiPostCodeStart(text);

            if (posSoi > 0)
                address[0] = CleanPndAddressValue(text.Substring(0, posSoi));
            else if (posRoad > 0)
                address[0] = CleanPndAddressValue(text.Substring(0, posRoad));

            if (posSoi >= 0 && posRoad > posSoi)
                address[6] = CleanPndAddressValue(text.Substring(posSoi, posRoad - posSoi));

            if (posRoad >= 0 && posSubDistrict > posRoad)
                address[7] = CleanPndAddressValue(text.Substring(posRoad, posSubDistrict - posRoad));

            if (posSubDistrict >= 0 && posDistrict > posSubDistrict)
                address[8] = CleanPndAddressValue(text.Substring(posSubDistrict, posDistrict - posSubDistrict));

            if (posDistrict >= 0)
            {
                int distProvEnd = posPostCode > posDistrict ? posPostCode : text.Length;
                string distProv = text.Substring(posDistrict, distProvEnd - posDistrict).Trim();
                int posProvince = -1;
                string[] provinceMarkers = { "กรุงเทพฯ", "กรุงเทพมหานคร", "จังหวัด" };
                foreach (string marker in provinceMarkers)
                {
                    int markerPos = distProv.IndexOf(marker, StringComparison.Ordinal);
                    if (markerPos > 0 && (posProvince < 0 || markerPos < posProvince))
                        posProvince = markerPos;
                }

                if (posProvince > 0)
                {
                    address[9] = CleanPndAddressValue(distProv.Substring(0, posProvince));
                    address[10] = CleanPndAddressValue(distProv.Substring(posProvince));
                }
                else
                {
                    int lastSpace = distProv.LastIndexOf(' ');
                    if (lastSpace > 0)
                    {
                        address[9] = CleanPndAddressValue(distProv.Substring(0, lastSpace));
                        address[10] = CleanPndAddressValue(distProv.Substring(lastSpace + 1));
                    }
                    else
                    {
                        address[9] = CleanPndAddressValue(distProv);
                    }
                }
            }

            return address;
        }

        private string CleanPndAddressValue(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Trim().Length == 0)
                return "1";
            return value.Trim();
        }

        private int FindThaiPostCodeStart(string text)
        {
            for (int i = 0; i <= text.Length - 5; i++)
            {
                if (char.IsDigit(text[i]) && char.IsDigit(text[i + 1]) && char.IsDigit(text[i + 2]) && char.IsDigit(text[i + 3]) && char.IsDigit(text[i + 4]))
                    return i;
            }
            return -1;
        }
        private ParameterField SetParamValue(string paramName, string paramValue)
        {
            ParameterField paramField = new ParameterField();
            ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
            paramField.Name = paramName;
            paramDiscreteValue.Value = paramValue;
            paramField.CurrentValues.Add(paramDiscreteValue);
            return paramField;
        }

        #region ClearCrystalReport
        /// <summary>
        /// Cleaer Crystal Report
        /// </summary>
        private void ClearCrystalReport()
        {
            hdfShowCrReportDiv.Value = "0";
            GERP_Report.ReportSource = null;
            GERP_Report.RefreshReport();
        }
        #endregion
        public static class PrintCodeType
        {
            public const string
            None = "0",
            BarCode = "1",
            QrCode = "2";
        }
        #region ControlEnum
        public enum ControlsEnum
        {
            Location
        }
        #endregion
    }
}

