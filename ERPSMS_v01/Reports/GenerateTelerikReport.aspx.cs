using AjaxControlToolkit;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using Telerik.ReportViewer.Html5.WebForms;

namespace ERPSMS_v01.Reports
{
    public partial class GenerateTelerikReport : System.Web.UI.Page
    {
        #region Variables and Properties
        #region Properties

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
        BusinessObject.User currentUser;
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
            //If any report document exist, need to dispose the object
            if (!IsPostBack)
            {

            }


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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
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
            if (!IsPostBack)
            {
                #region If Request From Menu or Inbox
                if (Request.QueryString[QueryStrings.FromExt] == null)
                {

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

                  
                    lblBreadCrum.Text = this.GetLocalResourceObject("Print").ToString();
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
                        case ApplicationType.EI:
                        case ApplicationType.CMP:
                        case ApplicationType.DISP:
                        case ApplicationType.ACI:
                        case ApplicationType.EMR:
                        case ApplicationType.OS:
                        case ApplicationType.DSA:
                        case ApplicationType.BOM:
                        case ApplicationType.EIT:
                        case ApplicationType.SR:
                        case ApplicationType.SOA:
                        case ApplicationType.SRA:
                        case ApplicationType.FRD:
                        case ApplicationType.ES:
                        case ApplicationType.MTR:
                        case ApplicationType.MTI:
                        case ApplicationType.CID:
                            //case ApplicationType.PSIJ:
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
                    }
                }
              

                //#region If Request From External(Report or Other page) otherthan Menu or Inbox
                //else if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                //{
                //    ReturnUrl = Convert.ToString(Request.Url);
                //    hdfRefUrl.Value = ReturnUrl;
                //    if (Request.QueryString[QueryStrings.PK] != null)
                //        PK = Request.QueryString["PK"] != string.Empty ? Convert.ToInt32(Request.QueryString["PK"]) : 0;
                //    if (Request.QueryString[QueryStrings.FromDate] != null)
                //        txtFromDate.Text = hdfFromDate.Value = Convert.ToDateTime(Request.QueryString["FromDate"]).ToString(Resources.Constants.DateFormatShort);
                //    if (Request.QueryString[QueryStrings.ToDate] != null)
                //        txtToDate.Text = hdfToDate.Value = Convert.ToDateTime(Request.QueryString["ToDate"]).ToString(Resources.Constants.DateFormatShort);
                //    if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString().Trim() != string.Empty)
                //    {
                //        RecPK = Convert.ToInt32(Request.QueryString["ID"]);
                //    }
                //    else
                //    {
                //        ReturnUrl = Convert.ToString(Request.Url);
                //        hdfRefUrl.Value = ReturnUrl;
                //    }
                //    RptType = Request.QueryString["APPTYPE"].ToString();
                //    hdfAppType.Value = RptType;
                //    RptSubType = Request.QueryString["APPSUBTYPE"] != string.Empty ? Convert.ToInt32(Request.QueryString["APPSUBTYPE"]) : 0;
                //    hdfSubType.Value = RptSubType.ToString();
                //    divQAC.Visible = false;
                //    divTrialBal.Visible = false;
                //    divGL.Visible = false;
                //    divAccPopUp.Visible = false;
                //    if (RptType == ApplicationType.PR || RptType == ApplicationType.PRT || RptType == ApplicationType.RFQ || RptType == ApplicationType.PO || RptType == ApplicationType.POP || RptType == ApplicationType.POG || RptType == ApplicationType.POPG || RptType == ApplicationType.POTR || RptType == ApplicationType.POT || RptType == ApplicationType.GIN || RptType == ApplicationType.GRN || RptType == ApplicationType.SCWO)
                //        btnSearch.Visible = false;
                //    else
                //        btnSearch.Visible = true;
                //    lblBreadCrum.Text = this.GetLocalResourceObject("Print").ToString();

                //    GetFieldValues(RptType);
                //    if (AppTypeDetailsList == null)
                //    {
                //        ClearCrystalReport();
                //        divReportViewer.Visible = false;
                //        divCrystalReportViewer.Visible = false;
                //        divNodata.Visible = false;

                //    }
                //    var reportName = AppTypeDetailsList.Select(l => l.AST_OP_FILE1).ToList();
                //    switch (((string[])reportName[0].ToString().Split('.'))[1].Trim().ToLower())
                //    {
                //        #region crystal report calling
                //        case ReportType.CrystalReport:
                //            if (reportDocument != null)
                //            {
                //                ClearCrystalReport();
                //                reportDocument.Close();
                //                reportDocument.Dispose();
                //                reportDocument = null;
                //                GC.Collect();
                //            }
                //            switch (RptType)
                //            {
                //                case ApplicationType.AS:
                //                    divReportViewer.Visible = true;
                //                    divCrystalReportViewer.Visible = true;
                //                    divNodata.Visible = false;
                //                    hdfAccount.Value = Convert.ToString(PK);
                //                    if (RptSubType == 0)
                //                        lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedger_Report").ToString();
                //                    if (!ValidateForm())
                //                    {
                //                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                //                    }
                //                    else
                //                    {

                //                        GetSelectedAccounts();
                //                        SetFieldValuesCrystal(RptType, reportName[0].ToString());
                //                    }
                //                    break;
                //                case ApplicationType.COL:
                //                    divReportViewer.Visible = true;
                //                    divCrystalReportViewer.Visible = true;
                //                    divNodata.Visible = false;
                //                    hdfAccount.Value = Convert.ToString(PK);
                //                    //if (RptSubType == 0)
                //                    //    lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedger_Report").ToString();
                //                    ////if (!ValidateForm())
                //                    ////{
                //                    ////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                //                    ////}
                //                    //else
                //                    //{

                //                    GetSelectedAccounts();
                //                    SetFieldValuesCrystal(RptType, reportName[0].ToString());
                //                    //}
                //                    break;
                //                case ApplicationType.SFG:
                //                case ApplicationType.SFGCD:
                //                case ApplicationType.SFGBL:
                //                    divReportViewer.Visible = true;
                //                    divCrystalReportViewer.Visible = true;
                //                    divNodata.Visible = false;
                //                    //hdfAccount.Value = Convert.ToString(PK);
                //                    //if (RptSubType == 0)
                //                    //    lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedger_Report").ToString();
                //                    ////if (!ValidateForm())
                //                    ////{
                //                    ////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                //                    ////}
                //                    //else
                //                    //{


                //                    SetFieldValuesCrystal(RptType, reportName[0].ToString());
                //                    //}
                //                    break;
                //                case ApplicationType.SFGD:
                //                case ApplicationType.SFGCDSP:
                //                case ApplicationType.SFGBDSP:
                //                    divReportViewer.Visible = true;
                //                    divCrystalReportViewer.Visible = true;
                //                    divNodata.Visible = false;
                //                    //hdfAccount.Value = Convert.ToString(PK);
                //                    //if (RptSubType == 0)
                //                    //    lblBreadCrum.Text = this.GetLocalResourceObject("GeneralLedger_Report").ToString();
                //                    ////if (!ValidateForm())
                //                    ////{
                //                    ////    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                //                    ////}
                //                    //else
                //                    //{


                //                    SetFieldValuesCrystal(RptType, reportName[0].ToString());
                //                    //}
                //                    break;
                //            }
                //            break;
                //        #endregion
                //        #region RDLC Calling
                //        case ReportType.RDLCReport:
                //            switch (RptType)
                //            {
                //                case ApplicationType.MI:
                //                case ApplicationType.STA:
                //                case ApplicationType.EMI:
                //                case ApplicationType.EMR:
                //                case ApplicationType.GRN:
                //                case ApplicationType.GIN:
                //                case ApplicationType.MRT:
                //                case ApplicationType.MTI:

                //                    GetFieldValues(RptType);
                //                    SetFieldValues(RptType);
                //                    break;
                //                case ApplicationType.COMR:
                //                    if (Request.QueryString["SbuID"] != null && Request.QueryString["SbuID"].ToString().Trim() != string.Empty)
                //                    {
                //                        SbuID = Convert.ToInt32(Request.QueryString["SbuID"]);
                //                    }
                //                    startOfMonth = Convert.ToDateTime(Request.QueryString["FROMDATE"].ToString());
                //                    endOfMonth = Convert.ToDateTime(Request.QueryString["TODATE"].ToString());
                //                    ProSize = Convert.ToInt32(Request.QueryString["ProSize"]);
                //                    dsCompoundUsage = BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetCompoundUsageSummary(RecPK, SbuID, startOfMonth, endOfMonth, ProSize);
                //                    SetFieldValues(RptType);
                //                    break;

                //                case ApplicationType.CDC:
                //                    if (Request.QueryString["SbuID"] != null && Request.QueryString["SbuID"].ToString().Trim() != string.Empty)
                //                    {
                //                        SbuID = Convert.ToInt32(Request.QueryString["SbuID"]);
                //                    }
                //                    startOfMonth = Convert.ToDateTime(Request.QueryString["FROMDATE"].ToString());
                //                    endOfMonth = Convert.ToDateTime(Request.QueryString["TODATE"].ToString());
                //                    //ProSize = Convert.ToInt32(Request.QueryString["ProSize"]);
                //                    dsCompoundUsage = BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetCompoundYieldCost(RecPK, SbuID, startOfMonth, endOfMonth);
                //                    SetFieldValues(RptType);
                //                    break;

                //            }
                //            break;
                //            #endregion
                //    }
                //}
                //#endregion
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
            { }
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
                return false;
            }
        }
    }
    #endregion
}