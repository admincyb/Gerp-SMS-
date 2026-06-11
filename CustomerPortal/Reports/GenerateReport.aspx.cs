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
//using ERPSMS_v01.Administration.Masters;
using ERPService.Administration;
using ERP.Utilities;
using System.Xml;
using BusinessObject.AccountManagement;
using System.IO;
using System.Threading;
using GTIService.Dashboard;
using System.Security;
using System.Security.Permissions;
using BusinessObject.Mailer;
using System.Runtime;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.Reports
{
    public partial class GenerateReport : ERP.Store.UI.ReportBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// To maintain the PageIndex in viewstate
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
        /// <summary>
        /// To maintain Show/Hide Description in Work order report
        /// </summary>
        private int ShowHideDesc
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ShowHideDesc"]);
            }
            set
            {
                this.ViewState["ShowHideDesc"] = value;
            }
        }
        /// <summary>
        /// To maintain boq/non boq in Work order report
        /// </summary>
        private int InternalFlag
        {
            get
            {
                return Convert.ToInt32(this.ViewState["InternalFlag"]);
            }
            set
            {
                this.ViewState["InternalFlag"] = value;
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
        public int IOType
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
        private int IOReview
        {
            get
            {
                return (int)this.ViewState["IOReview"];
            }
            set
            {
                this.ViewState["IOReview"] = value;
            }
        }
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
        //private int VersonID
        //{
        //    get
        //    {
        //        return Convert.ToInt32(this.ViewState["RevID"]);
        //    }
        //    set
        //    {
        //        this.ViewState["RevID"] = value;
        //    }
        //}

        /// <summary>
        /// To maintain Revision Pk
        /// </summary>
        private int RevPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["RevPK"]);
            }
            set
            {
                this.ViewState["RevPK"] = value;
            }
        }
        private string VersionNo
        {
            get
            {
                return (string)this.ViewState["VersionNo"];
            }
            set
            {
                this.ViewState["VersionNo"] = value;
            }
        }

        private string FilterDate
        {
            get
            {
                return (string)this.ViewState["FilterDate"];
            }
            set
            {
                this.ViewState["FilterDate"] = value;
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

        private int LotNoFirstIndex
        {
            get
            {
                return Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "SCLotNoGenerateFirstIndex"));
            }
        }

        private int LotNoLastIndex
        {
            get
            {
                return Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "SCLotNoGenerateLastIndex"));
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

        #region Properties for document generation(mailing purpose)
        public string APPTYPE
        {
            get;
            set;
        }
        public int APPSUBTYPE
        {
            get;
            set;
        }
        public int TransactionPk
        {
            get;
            set;
        }
        public int Version
        {
            get;
            set;
        }
        public string DocumentName
        {
            get;
            set;
        }
        #endregion
        #endregion

        #region Variables
        BusinessObject.User currentUser;
        private DataSet dsPurchaseRequest;
        private DataSet dsDelivaryOrder;
        private DataSet dsSaleOrder;
        private DataSet dsRptDataset;
        private DataSet dsReportDetails;
        private DataSet dsContainerInspection;
        DataTable dtRptDatatable1;
        DataTable dtRptDatatable2;
        DataTable dtRptDatatable3;
        DataTable dtRptDatatable4;
        DataTable dtRptDatatable5;
        ReportDataSource dsRptDataSource1;
        ReportDataSource dsRptDataSource2;
        ReportDataSource dsRptDataSource3;
        ReportDataSource dsRptDataSource4;
        ReportDataSource dsRptDataSource5;
        DataTable dtSOProductDtls;
        DataTable dtSOHeaderDtls;
        LocalReport locRpt;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsListNew;
        private ERPEntities currentEntity;
        private CommonService cm;
        private DataSet SelectedAccountsList;
        public event TreeNodeEventHandler TreeNodeCheckChanged;
        private static string ReturnUrl;
        private static string ForMonth;
        private static string IsDue;
        private static string GroupBy;
        private static int CusID;
        private static int ItemID;
        private int SwapBuyer = 0;
        private string Buyer = string.Empty;
        private bool PrintShipTo = true;
        private double? NetWt = 0;
        private double? GrWt = 0;
        private string attachmentFilePath;
        private string attachmentFileFormat;
        private string attachmentFileContentType;
        private string attachmentFileName;
        private string clientCode;
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
                    #region PR
                    case ApplicationType.PR:
                        dsPurchaseRequest = BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetPurchaseRequestReportDetails(RecPK);
                        break;
                    #endregion
                    #region RFQ
                    case ApplicationType.RFQ:
                        dsPurchaseRequest = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQReportDetails(RecPK, VndPK);
                        if (dsPurchaseRequest != null)
                        {
                            if (dsPurchaseRequest.Tables[0].Rows.Count > 0)
                            {
                                AppvdDate = Convert.ToDateTime(Convert.ToString(dsPurchaseRequest.Tables[0].Rows[0]["RRH_APPROVED_DATE"]) != string.Empty ? dsPurchaseRequest.Tables[0].Rows[0]["RRH_APPROVED_DATE"].ToString() : null);
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
                        dsPurchaseRequest = BusinessLogic.PurchaseOrderManagement.PurchaseOrderCreation.PurchaseOrderDetails(RecPK, GetGlobalResourceObject("ConfigurationsRes", "PurchaseOrderOutRPTSP").ToString());
                        if (dsPurchaseRequest != null)
                        {
                            if (dsPurchaseRequest.Tables[0].Rows.Count > 0)
                            {
                                AppvdDate = Convert.ToString(dsPurchaseRequest.Tables[0].Rows[0]["POH_APPROVED_DATE"]) != string.Empty ? Convert.ToDateTime(dsPurchaseRequest.Tables[0].Rows[0]["POH_APPROVED_DATE"].ToString()) : DateTime.Now.Date;
                            }
                        }
                        break;
                    #endregion
                    #region SO
                    case ApplicationType.SO:
                    case ApplicationType.SOD:
                        if (RevPK > 0)
                        {
                            dsSaleOrder = BusinessLogic.Sales.SaleOrderBL.GetSaleOrderArchiveDtls(RecPK, RevPK);
                        }
                        else
                        {
                            if (GetGlobalResourceObject("ConfigurationsRes", "ISMENUWISEDOCNOREVISION").ToString() == "1")
                            {
                                AppTypeDetailsListNew =  cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                int reportspk = AppTypeDetailsListNew[0].AST_PK;
                                dsSaleOrder = BusinessLogic.Sales.SaleOrderBL.SaleOrderDetailsDOCNOREVISION(RecPK,reportspk);
                            }
                            else
                            {
                                dsSaleOrder = BusinessLogic.Sales.SaleOrderBL.SaleOrderDetails(RecPK);
                            }
                                //dsSaleOrder = BusinessLogic.Sales.SaleOrderBL.SaleOrderDetails(RecPK);
                        }
                        if (dsSaleOrder != null)
                        {
                            if (dsSaleOrder.Tables[0].Rows.Count > 0)
                            {
                                AppvdDate = Convert.ToString(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"]) != string.Empty ? Convert.ToDateTime(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"].ToString()) : DateTime.Now.Date;
                            }
                        }
                        break;
                    #endregion
                    #region DO
                    case ApplicationType.DO:
                        switch (RptSubType)
                        {
                            case 1:
                                dsDelivaryOrder = BusinessLogic.Sales.SaleOrderBL.GetSalesInvoiceDtls(RecPK, GetGlobalResourceObject("ConfigurationsRes", "SalesInvoiceOutRPTSP").ToString());
                                break;
                            default:
                                dsDelivaryOrder = BusinessLogic.ReportsManagement.DeliveryOrderBL.GetDeliveryOrderDetails(RecPK, RptSubType);
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
                        if (GetGlobalResourceObject("ConfigurationsRes", "ISMENUWISEDOCNOREVISION").ToString() == "1")
                        {
                            AppTypeDetailsListNew = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            int reportspk = AppTypeDetailsListNew[0].AST_PK;
                            dsSaleOrder = BusinessLogic.Sales.SaleOrderBL.SaleOrderDetailsDOCNOREVISION(RecPK,reportspk);
                            if (dsSaleOrder != null)
                            {
                                if (dsSaleOrder.Tables[0].Rows.Count > 0)
                                {
                                    AppvdDate = Convert.ToString(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"]) != string.Empty ? Convert.ToDateTime(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"].ToString()) : DateTime.Now.Date;
                                }
                            }
                        }
                        else
                        {
                            dsSaleOrder = BusinessLogic.Sales.SaleOrderBL.SaleOrderDetails(RecPK);
                            if (dsSaleOrder != null)
                            {
                                if (dsSaleOrder.Tables[0].Rows.Count > 0)
                                {
                                    AppvdDate = Convert.ToString(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"]) != string.Empty ? Convert.ToDateTime(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"].ToString()) : DateTime.Now.Date;
                                }
                            }

                        }
                        //    dsSaleOrder = BusinessLogic.Sales.SaleOrderBL.SaleOrderDetails(RecPK);
                        //if (dsSaleOrder != null)
                        //{
                        //    if (dsSaleOrder.Tables[0].Rows.Count > 0)
                        //    {
                        //        AppvdDate = Convert.ToString(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"]) != string.Empty ? Convert.ToDateTime(dsSaleOrder.Tables[0].Rows[0]["SOH_APPROVED_DATE"].ToString()) : DateTime.Now.Date;
                        //    }
                        //}
                        break;
                    #endregion
                    #region GRN
                    case ApplicationType.GRN:
                        dsPurchaseRequest = BusinessLogic.StoreManagement.GoodsReceiptNote.GetGRNDetailsForNewReport(RecPK);
                        break;
                    #endregion
                    #region GIN
                    case ApplicationType.GIN:
                        dsPurchaseRequest = BusinessLogic.StoreManagement.GoodsInspectionNote.GetGoodsInspectionNoteTables(RecPK);
                        break;
                    #endregion
                    #region VP
                    case ApplicationType.EMI:
                        dsPurchaseRequest = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetIssuingReportByReqId(RecPK);
                        break;
                    #endregion
                    #region SI
                    case ApplicationType.SI:
                    case ApplicationType.MSI:
                        dsDelivaryOrder = BusinessLogic.Sales.SaleOrderBL.GetSalesInvoiceDtls(RecPK, GetGlobalResourceObject("ConfigurationsRes", "SalesInvoiceOutRPTSP").ToString());
                        break;
                    #endregion
                    #region OPLN
                    case ApplicationType.OPLN:
                        if (RptSubType == 0)
                            dsReportDetails = BusinessLogic.OrderPlanning.OrderPlanningBL.GetOPReportData(RecPK);
                        else if (RptSubType == 1)
                            dsReportDetails = BusinessLogic.OrderPlanning.OrderPlanningBL.GetOPReportDataVersionwise(RecPK, VersionNo);
                        else if (RptSubType == 2)
                            dsReportDetails = BusinessLogic.OrderPlanning.OrderPlanningBL.GetOProductionProgressReport(RecPK, FilterDate);
                        else if (RptSubType == 3) //Pending Orders
                            dsReportDetails = BusinessLogic.OrderPlanning.OrderPlanningBL.GetPendingOrderDetailsReport(Convert.ToInt16(currentUser.CurrentSBUPK));
                        break;
                    #endregion
                    #region SWO, WO
                    case ApplicationType.PRJ:
                    case ApplicationType.SWO:

                        int version = -1;
                        if (!FromExternal)
                        {
                            version = Request.QueryString["VERSION"] != null
                                           ? Request.QueryString["VERSION"] != string.Empty
                                               ? Convert.ToInt32(Request.QueryString["VERSION"])
                                               : -1
                                           : -1;
                        }
                        dsRptDataset = BusinessLogic.WorkOrder.WorkOrderBL.GetWorkOrderReport(RecPK, (int)DbActiveStatus.ACTIVE, SbuID, version, ShowHideDesc, InternalFlag);
                        //CompanyPK = dsRptDataset != null
                        //            ? dsRptDataset.Tables.Count > 0
                        //                ? dsRptDataset.Tables[0].Rows.Count > 0
                        //                    ? Convert.ToInt32(dsRptDataset.Tables[0].Rows[0]["WOH_COMPANY"].ToString())
                        //                    : 0
                        //                : 0
                        //            : 0;


                        break;
                        #endregion
                }
                AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
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
                    string LogoPath = string.Empty;
                    // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN); 
                    //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    //{
                    if (File.Exists(Server.MapPath(Resources.Controls.LogoPath) + CompanyList[0].CMP_LOGO))
                    {
                        LogoPath = "file:///" + Server.MapPath(Resources.Controls.LogoPath) + CompanyList[0].CMP_LOGO;
                        fileExists = true;
                    }
                    //}
                    //else
                    //{
                    //    if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_LOGO))
                    //    {
                    //        LogoPath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_LOGO;
                    //        fileExists = true;
                    //    }
                    //}
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
        private ReportDataSource GetCompanyDetails(int? CmpnyPk)
        {
            ReportDataSource CompanyDtls = null;
            currentEntity = new ERPEntities();
            List<SPADM_COMPANY_MST_GET_KV_Result> CompanyList = currentEntity.SPADM_COMPANY_MST_GET_KV(CmpnyPk, Convert.ToByte(DbActiveStatus.HASPK), null, null, null).ToList();

            if (CompanyList != null && CompanyList.Count > 0)
            {
                if (Convert.ToString(CompanyList[0].CMP_LOGO) != string.Empty)
                {
                    bool fileExists = false;
                    bool OutputFileExists = false;
                    string LogoPath = string.Empty;
                    string OutputLogoPath = string.Empty;
                    // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN); 
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        if (File.Exists(Server.MapPath(Resources.Controls.CompanyLogo) + CompanyList[0].CMP_LOGO))
                        {
                            LogoPath = "file:///" + Server.MapPath(Resources.Controls.CompanyLogo) + CompanyList[0].CMP_LOGO;
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
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        if (File.Exists(Server.MapPath(Resources.Controls.CompanyLogo) + CompanyList[0].CMP_OP_LOGO))
                        {
                            OutputLogoPath = "file:///" + Server.MapPath(Resources.Controls.CompanyLogo) + CompanyList[0].CMP_OP_LOGO;
                            OutputFileExists = true;
                        }
                    }
                    else
                    {
                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_OP_LOGO))
                        {
                            OutputLogoPath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_OP_LOGO;
                            OutputFileExists = true;
                        }
                    }
                    if (!OutputFileExists)
                    {
                        OutputLogoPath = string.Empty;
                    }
                    CompanyList[0].CMP_LOGO = LogoPath;
                    CompanyList[0].CMP_OP_LOGO = OutputLogoPath;
                }
            }
            CompanyDtls = new ReportDataSource("CompanyDtls", CompanyList);
            return CompanyDtls;
        }
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
                if (!FromExternal)
                {
                    rvCurrentRptViewer = rvViewReport;
                    rvCurrentRptViewer.Visible = true;
                }
                else
                {
                    rvCurrentRptViewer = new ReportViewer();
                }
                LocalReport locRpt;
                ReportParameter ReportParam;
                cm = new CommonService();
                locRpt = null;
                rvCurrentRptViewer.LocalReport.DataSources.Clear();
                locRpt = rvCurrentRptViewer.LocalReport;
                rvCurrentRptViewer.LocalReport.DataSources.Clear();
                locRpt.EnableExternalImages = true;
                rvCurrentRptViewer.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                int? CompanyPK = 0;

                switch (appType)
                {
                    #region PR
                    case ApplicationType.PR:
                        ReportDataSource dsPRHeaderDtls;
                        ReportDataSource dsWorkFlowComments;
                        ReportDataSource dsPRMaterialDtls;
                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtPRHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtPRDtls = dsPurchaseRequest.Tables[1];
                            DataTable dtWorkFlowComment = dsPurchaseRequest.Tables[2];
                            //if (dtPRHeaderDtls.Rows.Count > 0 || dtPRDtls.Rows.Count > 0)
                            //{
                            dsPRHeaderDtls = new ReportDataSource("PRHeaderDtls", dtPRHeaderDtls);
                            dsPRMaterialDtls = new ReportDataSource("PRMaterialDtls", dtPRDtls);
                            dsWorkFlowComments = new ReportDataSource("WorkFlowComment", dtWorkFlowComment);

                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPRHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPRMaterialDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsWorkFlowComments);
                            //}
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region PO
                    case ApplicationType.PO:
                        ReportDataSource dsPOHeaderDtls;
                        ReportDataSource dsPOProductDtls;
                        ReportDataSource dsPOMoreDtls;
                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtPOHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtPOProductDtls = dsPurchaseRequest.Tables[1];
                            DataTable dtPOMoreDtls = dsPurchaseRequest.Tables[2];
                            //if (dtPOHeaderDtls.Rows.Count > 0 || dtPOProductDtls.Rows.Count > 0)
                            //{
                            dsPOHeaderDtls = new ReportDataSource("POHeaderDtls", dtPOHeaderDtls);
                            dsPOProductDtls = new ReportDataSource("POProductDtls", dtPOProductDtls);
                            dsPOMoreDtls = new ReportDataSource("POMoreDtls", dtPOMoreDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPOHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPOProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPOMoreDtls);
                            //}
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region SO
                    case ApplicationType.SO:
                    case ApplicationType.SOD:
                        ReportDataSource dsSOHeaderDtls;
                        ReportDataSource dsSOProductDtls;
                        ReportDataSource dsSOMoreDtls;
                        ReportDataSource dsSOTaxDtls;
                        if (dsSaleOrder != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtSOHeaderDtls = dsSaleOrder.Tables[0];
                            DataTable dtSOProductDtls = dsSaleOrder.Tables[1];
                            DataTable dtSOMoreDtls = dsSaleOrder.Tables[2];
                            DataTable dtSOTaxDtls = new DataTable();
                            if (dsSaleOrder.Tables.Count > 3)
                                dtSOTaxDtls = dsSaleOrder.Tables[3];
                            //if (dtSOHeaderDtls.Rows.Count > 0 || dtSOProductDtls.Rows.Count > 0)
                            //{
                            dsSOHeaderDtls = new ReportDataSource("SOHeaderDtls", dtSOHeaderDtls);
                            dsSOProductDtls = new ReportDataSource("SOProductDtls", dtSOProductDtls);
                            dsSOMoreDtls = new ReportDataSource("SOMoreDtls", dtSOMoreDtls);
                            dsSOTaxDtls = new ReportDataSource("SOTaxDtls", dtSOTaxDtls);


                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSOHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSOProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSOMoreDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSOTaxDtls);
                            if (dtSOHeaderDtls != null & dtSOHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtSOHeaderDtls.Rows[0]["SOH_COMPANY"].ToString());
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            //}
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region IO
                    case ApplicationType.IO:
                        if (dsSaleOrder != null)
                        {
                            SetReportParameters(locRpt);
                            dtSOHeaderDtls = dsSaleOrder.Tables[0];
                            dtSOProductDtls = dsSaleOrder.Tables[1];
                            if (IOReview == 0)
                            {
                                AutoGenerateLotNo();
                            }
                            DataTable dtSOMoreDtls = dsSaleOrder.Tables[2];
                            dsSOHeaderDtls = new ReportDataSource("SOHeaderDtls", dtSOHeaderDtls);
                            dsSOProductDtls = new ReportDataSource("SOProductDtls", dtSOProductDtls);
                            dsSOMoreDtls = new ReportDataSource("SOMoreDtls", dtSOMoreDtls);
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
                            rvCurrentRptViewer.Visible = false;
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

                            //if (dtQRHeaderDtls.Rows.Count > 0 || dtQRDtls.Rows.Count > 0)
                            //{
                            dsQRHeaderDtls = new ReportDataSource("QRHeaderDtls", dtQRHeaderDtls);
                            dsQRDtls = new ReportDataSource("QRDetails", dtQRDtls);

                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsQRHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsQRDtls);

                            //}
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region CQTN
                    case ApplicationType.CQTN:
                        ReportDataSource dsCQTN;
                        currentEntity = new ERPEntities();
                        //Check Quaotion Revision PK
                        if (RevPK == 0)
                        {
                            List<SPCRM_QUOTATION_RPT_Result> lstQuotation = currentEntity.SPCRM_QUOTATION_RPT(RecPK).ToList();
                            dsCQTN = new ReportDataSource("QRHeaderDtls", lstQuotation);
                            if (lstQuotation.Count > 0)
                            {
                                CompanyPK = lstQuotation[0].CMP_PK == null ? 0 : lstQuotation[0].CMP_PK;
                            }
                        }
                        else
                        {
                            List<SPCRM_QUOTATION_ARCHIVE_RPT_Result> lstQuotationArchive = currentEntity.SPCRM_QUOTATION_ARCHIVE_RPT(RecPK, (byte)RevPK).ToList();
                            dsCQTN = new ReportDataSource("QRHeaderDtls", lstQuotationArchive);
                            if (lstQuotationArchive.Count > 0)
                            {
                                CompanyPK = lstQuotationArchive[0].CMP_PK == null ? 0 : lstQuotationArchive[0].CMP_PK;
                            }
                        }

                        if (dsCQTN != null)
                        {
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCQTN);

                            dsCQTN = new ReportDataSource("QRTaxDtls", currentEntity.SPCRM_QUOTATION_TAX_DTL(RecPK));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCQTN);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));

                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
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
                            //if (dtGRNHeaderDtls.Rows.Count > 0 || dtGRNDtls.Rows.Count > 0)
                            //{
                            dsQACHeader = new ReportDataSource("QACHdr", dtGRNHeaderDtls);
                            dsQACDtls = new ReportDataSource("QACDtls", dtGRNDtls);


                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsQACHeader);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsQACDtls);
                            //}
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
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
                            //if (dtGRNHeaderDtls.Rows.Count > 0 || dtGRNDtls.Rows.Count > 0)
                            //{
                            dsGRNHeaderDtls = new ReportDataSource("GRNHdr", dtGRNHeaderDtls);
                            dsGRNDtls = new ReportDataSource("GRNDtls", dtGRNDtls);


                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGRNHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGRNDtls);
                            //}
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
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
                            //if (dtGINHeaderDtls.Rows.Count > 0 || dtGINDtls.Rows.Count > 0)
                            //{
                            dsGINHeaderDtls = new ReportDataSource("GINHdr", dtGINHeaderDtls);
                            dsGINDtls = new ReportDataSource("GINDtls", dtGINDtls);


                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGINHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsGINDtls);
                            //}
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region COA
                    case ApplicationType.COA:
                        ReportDataSource dsCOA;
                        currentEntity = new ERPEntities();
                        dsCOA = new ReportDataSource("COADtls", currentEntity.SPFIN_COA_LIST_RPT(currentUser.SBUID));
                        if (dsCOA != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCOA);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region JV
                    case ApplicationType.JV:
                        ReportDataSource dsJV;
                        currentEntity = new ERPEntities();
                        dsJV = new ReportDataSource("JVHeader", currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK, VoucherVersion));
                        if (dsJV != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsJV);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    //#region DO
                    //case ApplicationType.DO:
                    //    ReportDataSource dsDOHeaderDtls;
                    //    ReportDataSource dsDOProductDtls;
                    //    if (dsDelivaryOrder != null)
                    //    {
                    //        SetReportParameters(locRpt);
                    //        DataTable dtDOHeaderDtls = dsDelivaryOrder.Tables[0];
                    //        DataTable dtDOProductDtls = dsDelivaryOrder.Tables[1];
                    //        dsDOHeaderDtls = new ReportDataSource("DOHeaderDtls", dtDOHeaderDtls);
                    //        dsDOProductDtls = new ReportDataSource("DOProductDtls", dtDOProductDtls);
                    //        rvCurrentRptViewer.LocalReport.DataSources.Add(dsDOHeaderDtls);
                    //        rvCurrentRptViewer.LocalReport.DataSources.Add(dsDOProductDtls);
                    //        if (dtDOHeaderDtls != null & dtDOHeaderDtls.Rows.Count > 0)
                    //        {
                    //            CompanyPK = Convert.ToInt32(dtDOHeaderDtls.Rows[0]["ICH_COMPANY"].ToString());
                    //        }
                    //        rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                    //    }
                    //    else
                    //    {
                    //        rvCurrentRptViewer.Visible = false;
                    //    }
                    //    break;
                    //#endregion
                    #region DO
                    case ApplicationType.DO:
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
                                //hdfCompanyPK.Value = dtDOHeaderDtls.Rows[0]["ICH_COMPANY"].ToString();
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            if (RptSubType == 7 || RptSubType == 8)
                            {
                                DataTable dtProdDtls = dtDOProductDtls;
                                NetWt = dtProdDtls.AsEnumerable().Sum((x => x.Field<double?>("DPD_BOX_IN_CRTN") * x.Field<double?>("LPD_QTY") * x.Field<double?>("LPD_NET_WT")));
                                GrWt = dtProdDtls.AsEnumerable().Sum((x => x.Field<double?>("LPD_QTY") * x.Field<double?>("LPD_CTN_GROSS_WT")));
                            }
                            SetReportParameters(locRpt);
                            //if (RptSubType == 3 || RptSubType == 4)
                            //{
                            //    rvCurrentRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);
                            //}

                        }
                        else
                        {
                            //divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            //divNodata.Visible = true;
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
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsTB);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region AS
                    case ApplicationType.AS:
                        ReportDataSource dsAS;
                        DataRow dr;
                        //GetSelectedAccounts();
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
                                dsAS = new ReportDataSource("ASDtls", currentEntity.SPFIN_ACCOUNT_STATEMENT_RPT(paramXml));
                                if (dsAS != null)
                                {
                                    AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                    SetReportParameters(locRpt);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsAS);
                                }
                                else
                                {
                                    rvCurrentRptViewer.Visible = false;
                                }
                            }
                        }
                        // dsAS = new ReportDataSource("ASDtls", currentEntity.SPFIN_ACCOUNT_STATEMENT_RPT(currentUser.SBUID, Convert.ToDateTime(txtFromDate.Text.Trim()), Convert.ToDateTime(txtToDate.Text.Trim()), null));

                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region PIJ
                    case ApplicationType.PIJ:
                        ReportDataSource dsPIJ;
                        currentEntity = new ERPEntities();
                        dsPIJ = new ReportDataSource("PVHeaderDtls", currentEntity.SPFIN_INVOICE_VND_VOUCHER_RPT(RecPK));
                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsPIJ);
                        dsPIJ = new ReportDataSource("PVAccountDtls", currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK, VoucherVersion));
                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsPIJ);
                        if (dsPIJ != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region VPJ
                    case ApplicationType.VPJ:
                        ReportDataSource dsPV;
                        currentEntity = new ERPEntities();
                        dsPV = new ReportDataSource("PVHeader", currentEntity.SPFIN_PAYMENT_VND_VOUCHER_RPT(RecPK, appType));
                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsPV);
                        dsPV = new ReportDataSource("PVRecordings", currentEntity.SPFIN_TRX_VOUCHER_RPT(ApplicationType.VPJ, RecPK, VoucherVersion));
                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsPV);
                        if (dsPV != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPV);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region CRJ
                    case ApplicationType.CRJ:
                        ReportDataSource dsCRJ;
                        currentEntity = new ERPEntities();
                        //if (RptSubType == Convert.ToInt32(ApplicationSubType.OFFICIALRECEIPT))
                        //{
                        dsCRJ = new ReportDataSource("RVHeader", currentEntity.SPFIN_RECEIPT_VOUCHER_RPT(RecPK, appType));
                        rvCurrentRptViewer.LocalReport.DataSources.Add(dsCRJ);
                        //}
                        //else
                        if (RptSubType != Convert.ToInt32(ApplicationSubType.OFFICIALRECEIPT))
                        {
                            //dsCRJ = new ReportDataSource("RVHeader", currentEntity.SPFIN_RECEIPT_VOUCHER_RPT(RecPK));
                            //rvCurrentRptViewer.LocalReport.DataSources.Add(dsCRJ);
                            dsCRJ = new ReportDataSource("RVAccountDtls", currentEntity.SPFIN_TRX_VOUCHER_RPT(appType, RecPK, VoucherVersion));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCRJ);
                        }

                        if (dsCRJ != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region SIJ
                    case ApplicationType.SIJ:
                        ReportDataSource dsSIJ;
                        currentEntity = new ERPEntities();

                        if (RptSubType == Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE))
                        {
                            List<SPFIN_SALES_ADV_INV_RPT_Result> lstData = currentEntity.SPFIN_SALES_ADV_INV_RPT(RecPK).ToList();
                            dsSIJ = new ReportDataSource("SVHeader", lstData);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIJ);
                            if (lstData != null && lstData.Count > 0)
                            {
                                CompanyPK = lstData[0].ICH_COMPANY;
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                        }
                        else
                        {
                            dsSIJ = new ReportDataSource("SVHeader", currentEntity.SPFIN_SALES_VOUCHER_RPT(RecPK));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIJ);
                            dsSIJ = new ReportDataSource("SVAccountDtls", currentEntity.SPFIN_TRX_VOUCHER_RPT(appType, RecPK, VoucherVersion));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIJ);
                        }
                        if (dsSIJ != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);

                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region SAS
                    case ApplicationType.SAS:
                        ReportDataSource dsSAS;
                        DataRow drSAS;
                        //GetSelectedAccounts();
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
                                dsSAS = new ReportDataSource("ASDtls", currentEntity.SPFIN_SUB_ACCOUNT_STATEMENT_RPT(paramXml));
                                if (dsSAS != null)
                                {
                                    AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                                    SetReportParameters(locRpt);
                                    rvCurrentRptViewer.LocalReport.DataSources.Add(dsSAS);
                                }
                                else
                                {
                                    rvCurrentRptViewer.Visible = false;
                                }
                            }
                        }
                        // dsAS = new ReportDataSource("ASDtls", currentEntity.SPFIN_ACCOUNT_STATEMENT_RPT(currentUser.SBUID, Convert.ToDateTime(txtFromDate.Text.Trim()), Convert.ToDateTime(txtToDate.Text.Trim()), null));

                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region BRC
                    case ApplicationType.BRC:
                        ReportDataSource dsBRC;
                        DataRow drBRC;
                        //GetSelectedAccounts();
                        divAccPopUp.Visible = false;
                        drBRC = SelectedAccountsList.Tables["ACCHEAD"].NewRow();

                        drBRC["FROM_DATE"] = txtFromDate.Text.Trim();
                        drBRC["TO_DATE"] = txtToDate.Text.Trim();
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
                                    rvCurrentRptViewer.Visible = false;
                                }
                            }
                        }
                        // dsAS = new ReportDataSource("ASDtls", currentEntity.SPFIN_ACCOUNT_STATEMENT_RPT(currentUser.SBUID, Convert.ToDateTime(txtFromDate.Text.Trim()), Convert.ToDateTime(txtToDate.Text.Trim()), null));

                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region CrDr
                    case ApplicationType.CN:
                    case ApplicationType.DN:
                        ReportDataSource dsDCN;
                        currentEntity = new ERPEntities();
                        dsDCN = new ReportDataSource("CrDrHeader", currentEntity.SPFIN_CRDR_NOTE_RPT(RecPK));
                        if (dsDCN != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsDCN);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
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
                            rvCurrentRptViewer.Visible = false;
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
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region PCS
                    case ApplicationType.PCS:
                        ReportDataSource dsPCS;
                        currentEntity = new ERPEntities();
                        dsPCS = new ReportDataSource("JVHeader", currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK, VoucherVersion));
                        if (dsPCS != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsPCS);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region EMI
                    case ApplicationType.EMI:
                        ReportDataSource dsEMIHeaderDtls;
                        ReportDataSource dsEMIProductDtls;
                        ReportDataSource dsEMIMoreDtls;
                        if (dsPurchaseRequest != null)
                        {
                            SetReportParameters(locRpt);
                            DataTable dtEMIHeaderDtls = dsPurchaseRequest.Tables[0];
                            DataTable dtEMIProductDtls = dsPurchaseRequest.Tables[1];
                            //DataTable dtEMIMoreDtls = dsPurchaseRequest.Tables[2];
                            //if (dtPOHeaderDtls.Rows.Count > 0 || dtPOProductDtls.Rows.Count > 0)
                            //{
                            dsEMIHeaderDtls = new ReportDataSource("dtEMIHeaderDtls", dtEMIHeaderDtls);
                            dsEMIProductDtls = new ReportDataSource("dtEMIProductDtls", dtEMIProductDtls);
                            //dsEMIMoreDtls = new ReportDataSource("POMoreDtls", dtEMIMoreDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsEMIHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsEMIProductDtls);
                            //rvCurrentRptViewer.LocalReport.DataSources.Add(dsEMIMoreDtls);
                            //}
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region CBR
                    case ApplicationType.CBR:
                        ReportDataSource dsCBR;
                        currentEntity = new ERPEntities();
                        if (GroupBy == "1")
                        {
                            if (CusID > 0)
                                dsCBR = new ReportDataSource("CBRDetails", currentEntity.SPCRM_CUST_ITEM_RATE_LIST_RPT(Convert.ToDateTime(ForMonth), Convert.ToByte(GroupBy), Convert.ToInt32(currentUser.SBUID), CusID, null, null));
                            else
                                dsCBR = new ReportDataSource("CBRDetails", currentEntity.SPCRM_CUST_ITEM_RATE_LIST_RPT(Convert.ToDateTime(ForMonth), Convert.ToByte(GroupBy), Convert.ToInt32(currentUser.SBUID), null, null, null));
                        }
                        else if (GroupBy == "2")
                        {
                            if (ItemID > 0)
                                dsCBR = new ReportDataSource("CBRDetails", currentEntity.SPCRM_CUST_ITEM_RATE_LIST_RPT(Convert.ToDateTime(ForMonth), Convert.ToByte(GroupBy), Convert.ToInt32(currentUser.SBUID), null, null, ItemID));
                            else
                                dsCBR = new ReportDataSource("CBRDetails", currentEntity.SPCRM_CUST_ITEM_RATE_LIST_RPT(Convert.ToDateTime(ForMonth), Convert.ToByte(GroupBy), Convert.ToInt32(currentUser.SBUID), null, null, null));
                        }
                        else
                        {
                            dsCBR = new ReportDataSource("CBRDetails", currentEntity.SPCRM_CUST_ITEM_RATE_LIST_RPT(Convert.ToDateTime(ForMonth), Convert.ToByte(GroupBy), Convert.ToInt32(currentUser.SBUID), null, null, null));
                        }
                        if (dsCBR != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsCBR);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region VP
                    case ApplicationType.VP:
                        ReportDataSource dsVP;
                        currentEntity = new ERPEntities();
                        dsVP = new ReportDataSource("VPHeaderDtls", currentEntity.SPFIN_PAYMENT_CHEQUE_PRINT_RPT(RecPK, null));
                        if (dsVP != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsVP);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion

                    #region SALFRCST
                    case ApplicationType.SALFRCST:
                        ReportDataSource dsSALFRCST;
                        ReportParameters reportParams;
                        string CurrFilter = string.Empty;
                        reportParams = SetUIValuesToXMLObject();
                        CurrFilter = reportParams.XmlSerialize();

                        currentEntity = new ERPEntities();
                        dsSALFRCST = new ReportDataSource("ReportDtls", currentEntity.SPSAL_FORECAST_RPT(CurrFilter));
                        if (dsSALFRCST != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSALFRCST);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }
                        break;
                    #endregion
                    #region SI
                    case ApplicationType.SI:
                    case ApplicationType.MSI://@@
                        ReportDataSource dsSIHeaderDtls;
                        ReportDataSource dsSIProductDtls;
                        ReportDataSource dsSIOtherDtls;
                        ReportDataSource dsTaxDtls;
                        ReportDataSource dsTotTaxDtls;
                        if (dsDelivaryOrder != null)
                        {
                            DataTable dtSIHeaderDtls = dsDelivaryOrder.Tables[0];
                            DataTable dtSIProductDtls = dsDelivaryOrder.Tables[1];
                            DataTable dtTotTaxDtls = dsDelivaryOrder.Tables[4];

                            dsSIHeaderDtls = new ReportDataSource("DOHeaderDtls", dtSIHeaderDtls);
                            dsSIProductDtls = new ReportDataSource("DOProductDtls", dtSIProductDtls);
                            dsSIOtherDtls = new ReportDataSource("DOOtherDtls", dsDelivaryOrder.Tables[2]);
                            dsTaxDtls = new ReportDataSource("TaxDtls", dsDelivaryOrder.Tables[3]);
                            if (!string.IsNullOrEmpty(Convert.ToString(dtSIHeaderDtls.Rows[0]["DPH_SWAP_BUYER"])))
                            {
                                SwapBuyer = Convert.ToInt32(dtSIHeaderDtls.Rows[0]["DPH_SWAP_BUYER"].ToString());
                            }
                            Buyer = dtSIHeaderDtls.Rows[0]["DPH_ADNL_BUYER"].ToString();
                            if (!string.IsNullOrEmpty(Convert.ToString(dtSIHeaderDtls.Rows[0]["DPH_PRINT_SHIP_TO"])))
                            {
                                PrintShipTo = Convert.ToBoolean(dtSIHeaderDtls.Rows[0]["DPH_PRINT_SHIP_TO"]);
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIHeaderDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIProductDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsSIOtherDtls);
                            if (dtSIHeaderDtls != null & dtSIHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtSIHeaderDtls.Rows[0]["DPH_COMPANY"].ToString());
                            }
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsTaxDtls);
                            dsTotTaxDtls = new ReportDataSource("TotTaxDtls", dtTotTaxDtls);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsTotTaxDtls);
                            SetReportParameters(locRpt);
                        }
                        else
                        {
                            rvCurrentRptViewer.Visible = false;
                        }

                        break;
                    #endregion
                    #region OPLN
                    case ApplicationType.OPLN:
                        ReportDataSource dsOPHeaderDtls;
                        ReportDataSource dsOPGroupDtls;
                        ReportDataSource dsOPprogress;
                        ReportDataSource dsOPPendingOrders;
                        if (dsReportDetails != null)
                        {
                            SetReportParameters(locRpt);
                            if (RptSubType == 0 || RptSubType == 1)
                            {
                                dsOPHeaderDtls = new ReportDataSource("OPHeaderDtls", dsReportDetails.Tables[0]);
                                dsOPGroupDtls = new ReportDataSource("OPGroupDtls", dsReportDetails.Tables[1]);
                                rvViewReport.LocalReport.DataSources.Add(dsOPHeaderDtls);
                                rvViewReport.LocalReport.DataSources.Add(dsOPGroupDtls);
                                rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(1));
                            }
                            else if (RptSubType == 2)
                            {
                                dsOPprogress = new ReportDataSource("ProgressDtls", dsReportDetails.Tables[0]);
                                rvViewReport.LocalReport.DataSources.Add(dsOPprogress);
                                rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(1));
                            }
                            else if (RptSubType == 3)
                            {
                                dsOPPendingOrders = new ReportDataSource("PendingOrderDtls", dsReportDetails.Tables[0]);
                                rvViewReport.LocalReport.DataSources.Add(dsOPPendingOrders);
                                rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(1));
                            }
                        }
                        else
                        {
                            rvViewReport.Visible = false;
                        }
                        break;

                    #endregion
                    #region (5-DataSet) WO
                    case ApplicationType.PRJ:
                        if (dsRptDataset != null)
                        {
                            SetReportParameters(locRpt);
                            //switch (RptSubType)
                            //{
                            //    case 1:
                            //        dtRptDatatable1 = dsRptDataset.Tables[0];
                            //        dsRptDataSource1 = new ReportDataSource("DataSet1", dtRptDatatable1);
                            //        rvCurrentRptViewer.LocalReport.DataSources.Add(dsRptDataSource1);
                            //        rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            //        break;
                            //    default:
                            dtRptDatatable1 = dsRptDataset.Tables[0];
                            dtRptDatatable2 = dsRptDataset.Tables[1];
                            dtRptDatatable3 = dsRptDataset.Tables[2];
                            dtRptDatatable4 = dsRptDataset.Tables[3];
                            dtRptDatatable5 = dsRptDataset.Tables[4];
                            dsRptDataSource1 = new ReportDataSource("DataSet1", dtRptDatatable1);
                            dsRptDataSource2 = new ReportDataSource("DataSet2", dtRptDatatable2);
                            dsRptDataSource3 = new ReportDataSource("DataSet3", dtRptDatatable3);
                            dsRptDataSource4 = new ReportDataSource("DataSet4", dtRptDatatable4);
                            dsRptDataSource5 = new ReportDataSource("DataSet5", dtRptDatatable5);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsRptDataSource1);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsRptDataSource2);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsRptDataSource3);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsRptDataSource4);
                            rvCurrentRptViewer.LocalReport.DataSources.Add(dsRptDataSource5);
                            CompanyPK = dsRptDataset != null
                                            ? dsRptDataset.Tables.Count > 0
                                                ? dsRptDataset.Tables[0].Rows.Count > 0
                                                    ? Convert.ToInt32(dsRptDataset.Tables[0].Rows[0]["WOH_COMPANY"].ToString())
                                                    : 0
                                                : 0
                                            : 0;
                            rvCurrentRptViewer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                            //break;
                            //}
                            //break;
                        }
                        else
                        {
                            //divReportViewer.Visible = false;
                            rvCurrentRptViewer.Visible = false;
                            //divNodata.Visible = true;
                        }
                        break;
                        #endregion

                }
                rvCurrentRptViewer.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                rvCurrentRptViewer.LocalReport.Refresh();
                if (FromExternal)
                    SavePDF(locRpt);
                else
                    if (!IsExportExcel)
                {
                    if (appType == ApplicationType.PRJ && RptSubType != 1 && RptSubType != 4)
                    {
                        SaveExcel(locRpt);
                    }
                    else
                    ShowPDF(locRpt);
                }
                else
                {
                    if (appType != ApplicationType.DO && (appType != ApplicationType.SI || (RptSubType != (int)SalesInvoiceType.Domestic && RptSubType != (int)SalesInvoiceType.Export && RptSubType != (int)SalesInvoiceType.Proforma)))
                    {
                        ShowPDF(locRpt);
                    }
                    else
                    {
                        btnSearch.Visible = false;
                    }
                }
                if (FromExternal && IsPdfGenerated)
                    setResult = true;
                else
                    setResult = false;
                return setResult;
            }
            catch (Exception ex)
            {
                CommonBL.ExceptionWriting(ex.GetInnerExceptionMsg(), "Inner Exception");
                CommonBL.ExceptionWriting(ex.ToString(), "Generate Reports Portal : " + ReportFile);

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
        /// <summary>
        /// For internal order print
        /// </summary>
        private void AutoGenerateLotNo()
        {
            if (dtSOProductDtls != null && !string.IsNullOrEmpty(dtSOHeaderDtls.Rows[0]["SOH_NO"].ToString()))
            {
                GetClientCode();//For getting client id
                for (int i = 0; i < dtSOProductDtls.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(dtSOProductDtls.Rows[i]["SOD_LOT_NO"].ToString()))
                    {
                        if (clientCode == ClientCode.MMT.ToString())
                        {
                            if (GetGlobalResourceObject("ConfigurationsRes", "AutogenerateLotNo").ToString() == "1") // Generate LOT No. based on the configuration entry
                            {
                                dtSOProductDtls.Rows[i]["SOD_LOT_NO"] = dtSOProductDtls.Rows[i]["ITM_CODE"].ToString().Length < 2 ? "" : (dtSOProductDtls.Rows[i]["ITM_CODE"].ToString().Substring(0, 2) + dtSOProductDtls.Rows[i]["ISD_NAT_SUF"].ToString()
                                                + dtSOHeaderDtls.Rows[0]["SOH_NO"].ToString().Substring(LotNoFirstIndex, LotNoLastIndex)
                                                + Convert.ToDateTime(dtSOHeaderDtls.Rows[0]["SOH_BOOKING_DATE"]).ToString("MMyy"));
                            }
                        }
                        else
                        {
                            if (GetGlobalResourceObject("ConfigurationsRes", "AutogenerateLotNo").ToString() == "1") // Generate LOT No. based on the configuration entry
                            {
                                dtSOProductDtls.Rows[i]["SOD_LOT_NO"] = dtSOProductDtls.Rows[i]["ITM_CODE"].ToString().Length < 2 ? "" : (dtSOProductDtls.Rows[i]["ITM_CODE"].ToString().Substring(0, (dtSOProductDtls.Rows[i]["ITM_CODE"].ToString().Length > 7 ? 7 : dtSOProductDtls.Rows[i]["ITM_CODE"].ToString().Length)) + "-"
                                                                + Convert.ToDateTime(dtSOHeaderDtls.Rows[0]["SOH_BOOKING_DATE"].ToString()).ToString("MMyy") + "-"
                                                                + dtSOHeaderDtls.Rows[0]["SOH_NO"].ToString().Substring(LotNoFirstIndex, LotNoLastIndex));
                            }
                        }
                    }
                }
            }
        }
        private void ShowPDF(LocalReport locRpt)
        {
            SavePDF(locRpt);
            if (File.Exists(attachmentFilePath))
            {
                Response.ClearContent();
                Response.ContentType = "application/pdf";
                //Response.AddHeader("content-Disposition", "attachment;filename=" + attachmentFileName);
                //Response.TransmitFile(attachmentFilePath);
                Response.Redirect(Resources.PageURL.PDFUrl + attachmentFileName);
                Response.Flush();
            }
        }
        #endregion

        #region Helper Methods

        private ReportParameters SetUIValuesToXMLObject()
        {
            ReportParameters tempReportParams = new GTIService.Dashboard.ReportParameters();
            tempReportParams.BizUnit = currentUser.SBUID;
            tempReportParams.Dept = currentUser.CurrentDeptPK;
            tempReportParams.UserPK = currentUser.PKUser;
            tempReportParams.Currency = currentUser.BaseCurrency;
            tempReportParams.Parameters = new List<GTIService.Dashboard.ReportParameterName>();
            string dateMP = string.IsNullOrEmpty(ForMonth) ? string.Empty : ForMonth;
            tempReportParams.FromDate = "01-" + dateMP;
            tempReportParams.ToDate = "01-" + dateMP;
            int totaldays = DateTime.DaysInMonth((Convert.ToDateTime(tempReportParams.ToDate)).Year, (Convert.ToDateTime(tempReportParams.ToDate)).Month);
            tempReportParams.ToDate = totaldays.ToString() + "-" + dateMP;
            return tempReportParams;
        }

        private DataTable ConfigurationSettings()
        {
            IsTaxForOtherChargeSales = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxSales")));
            IsRepeatSIheader = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "RepeatSIheader")));
            IsExportExcel = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsExportExcel")));
            if (!FromExternal)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, (FromExternal == true ? SbuID : currentUser.SBUID));
            return dt;
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
                string signaturePathAccepted = string.Empty;
                dsParamSettings = new DataSet();
                DataTable dt = ConfigurationSettings();
                locRpt.ReportPath = string.Empty;
                foreach (SPADM_APP_SUB_TYPE_DATA_GET_Result sa in AppTypeDetailsList)
                {
                    if (sa.AST_CODE == ApplicationType.SID && SwapBuyer == 1)
                    {
                        string rdlcName = sa.AST_OP_FILE1;
                        rptName = rdlcName.Replace(".", "_1.");
                    }
                    else
                    {
                        rptName = sa.AST_OP_FILE1;//
                    }
                    if (IsRepeatSIheader)
                    {
                        if (sa.AST_CODE == ApplicationType.SID || sa.AST_CODE == ApplicationType.SIE)
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
                    //locRpt.ReportPath = Server.MapPath(rptName);
                    if (FromExternal)
                        locRpt.ReportPath = Server.MapPath("~/Reports/" + rptName);
                    else
                        locRpt.ReportPath = Server.MapPath(rptName);
                    ReportFile = rptName;
                    parameters = new ReportParameter("QMSRef", sa.AST_QMS_REF);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("HideQMSRef", sa.AST_QMS_VISIBILITY.ToString());
                    locRpt.SetParameters(parameters);
                    if (sa.AST_RPT_SETTINGS != null)
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(sa.AST_RPT_SETTINGS)));

                    if (RptType == ApplicationType.PO || RptType == ApplicationType.SI || RptType == ApplicationType.RFQ || RptType == ApplicationType.DO)
                    {
                        parameters = new ReportParameter("ApprovedByName", sa.AST_APPROVED_USER);
                        locRpt.SetParameters(parameters);
                        if (Convert.ToString(sa.AST_APPROVED_SIGN) != string.Empty)
                        {
                            bool fileExists = false;
                            // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN); 
                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                            {
                                if (File.Exists(Server.MapPath("~\\Reports\\Images\\" + sa.AST_APPROVED_SIGN)))
                                {
                                    fileExists = true;
                                    signaturePath = "file:///" + Server.MapPath("~\\Reports\\Images\\" + sa.AST_APPROVED_SIGN);
                                }

                            }
                            else
                            {
                                if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + sa.AST_APPROVED_SIGN))
                                {
                                    fileExists = true;
                                    signaturePath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + sa.AST_APPROVED_SIGN;
                                }
                            }
                            if (fileExists)
                            {
                                parameters = new ReportParameter("ApprovedBySign", signaturePath);
                                locRpt.SetParameters(parameters);
                            }

                            // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath("~\\Reports\\Images\\" + sa.AST_APPROVED_SIGN)); 
                            // locRpt.SetParameters(parameters);
                        }
                    }
                    else if (RptType == ApplicationType.SO || RptType == ApplicationType.SOD || RptType == ApplicationType.IO)
                    {
                        signaturePath = string.Empty;
                        string SignatuePathSubmitted = string.Empty;
                        string SignaturePathReviewed = string.Empty;
                        string SignaturePathPrepared = string.Empty;
                        if (dsSaleOrder != null)
                        {
                            if (dsSaleOrder.Tables.Count > 0)
                            {
                                if (dsSaleOrder.Tables[0].Rows.Count > 0)
                                {
                                    GetClientCode();//For getting client id
                                    string signature = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.SOApprovedSign].ToString();
                                    string signatureAccepted = string.Empty;
                                    //if (RptType == ApplicationType.IO)//To solve the BWH AcceptedbySign
                                    //{
                                    signatureAccepted = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.SOAcceptedSign].ToString();//SOH_ACCEPTED_SIGN
                                    // }
                                    SignatuePathSubmitted = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.SOSubmittedSign].ToString(); 
                                    SignaturePathReviewed = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.SOReviewdSign].ToString();
                                    SignaturePathPrepared = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.SOPreparedSign].ToString();
                                    int SOstatus = Convert.ToInt32(dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.SOStatus]);
                                    if (clientCode == ClientCode.MMT.ToString())
                                    {
                                        if (SOstatus == 2)//For MMT shows signature only in the IO generation(BugID:- 6544)
                                        {
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
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                        {
                                            if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature))
                                                signaturePath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature;
                                            //Accepted Signature
                                            if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signatureAccepted))
                                                signaturePathAccepted = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signatureAccepted;
                                        }
                                    }
                                    if(clientCode== ClientCode.WARM.ToString())
                                    {
                                        if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                        {
                                            //Submitted Signature
                                            if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + SignatuePathSubmitted))
                                            {
                                                SignatuePathSubmitted = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + SignatuePathSubmitted;
                                               
                                            }
                                            //reviewed Signature
                                            if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + SignaturePathReviewed))

                                            {
                                                SignaturePathReviewed = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + SignaturePathReviewed;
                                                
                                            }
                                            //prepared by
                                            if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + SignaturePathPrepared))
                                            {
                                                SignaturePathPrepared = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + SignaturePathPrepared;
                                               
                                            }
                                        }

                                        parameters = new ReportParameter("SubmittedBySign", SignatuePathSubmitted);
                                        locRpt.SetParameters(parameters);
                                        parameters = new ReportParameter("ReviewedBySign", SignaturePathReviewed);
                                        locRpt.SetParameters(parameters);
                                        parameters = new ReportParameter("PreparedBySign", SignaturePathPrepared);
                                        locRpt.SetParameters(parameters);
                                    }
                                }
                            }
                          
                            parameters = new ReportParameter("ApprovedBySign", signaturePath);
                            locRpt.SetParameters(parameters);
                            parameters = new ReportParameter("AcceptedBySign", signaturePathAccepted);
                            locRpt.SetParameters(parameters);
                        }
                    }
                    else if (RptType == ApplicationType.TB || RptType == ApplicationType.AS || RptType == ApplicationType.SAS || RptType == ApplicationType.BRC)
                    {
                        parameters = new ReportParameter("FromDate", txtFromDate.Text.Trim());
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("ToDate", txtToDate.Text.Trim());
                        locRpt.SetParameters(parameters);
                    }
                    else if (RptType == ApplicationType.SALFRCST)
                    {
                        parameters = new ReportParameter("ToDate", ForMonth);
                        locRpt.SetParameters(parameters);
                    }

                    if (RptType == ApplicationType.DN || RptType == ApplicationType.CN)
                    {
                        parameters = new ReportParameter("ParamReason", RptType == ApplicationType.DN ? this.GetLocalResourceObject("lblDNReason").ToString() : this.GetLocalResourceObject("lblCNReason").ToString());
                        locRpt.SetParameters(parameters);
                    }

                    if (RptType == ApplicationType.CBR)
                    {
                        parameters = new ReportParameter("GroupBy", GroupBy);
                        locRpt.SetParameters(parameters);
                    }

                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    //string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
                    string currencyformat = "#" + currencysep + "#0.";
                    string NoFormat = "#" + currencysep + "#0.";
                    string WeightFormat = "#" + currencysep + "#0.";//Quantity Format
                    string ExchRateDigt = "#" + currencysep + "#0.";
                    string RateDeciDigt = "#" + currencysep + "#0.";
                    string MisRateDecFomat = "#" + currencysep + "#0.";
                    string currencydecimals = "";
                    string Nodecimal = string.Empty;
                    string Weightdecimal = string.Empty;//Weight Decimal
                    string ExchRateDigit = string.Empty;
                    string RateDecimalDigit = string.Empty;
                    string MisRateDecimal = string.Empty;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int curdigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < curdigit; i++)
                        {
                            currencydecimals += "0";
                        }
                        int WeightDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "WeightDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < WeightDigit; i++)
                        {
                            Weightdecimal += "0";
                        }
                        int NoDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());

                        for (int i = 0; i < NoDigit; i++)
                        {
                            Nodecimal += "0";
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
                        int MisRate = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "MiscRateDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < MisRate; i++)
                        {
                            MisRateDecimal += "0";
                        }
                    }
                    else
                    {
                        currencydecimals = "00";
                        Nodecimal = "00";
                    }
                    currencyformat = currencyformat + currencydecimals;
                    NoFormat = NoFormat + Nodecimal;
                    ExchRateDigt = ExchRateDigt + ExchRateDigit;
                    RateDeciDigt = RateDeciDigt + RateDecimalDigit;
                    WeightFormat = WeightFormat + Weightdecimal;
                    MisRateDecFomat = MisRateDecFomat + MisRateDecimal;
                    // currencyformat = {0:n} + currencydecimals;
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
                    if ((RptType == ApplicationType.DO && (RptSubType == 2 || RptSubType == 7)) || RptType == ApplicationType.SI || RptType == ApplicationType.PRJ)
                    {
                        parameters = new ReportParameter("WeightFormat", WeightFormat);
                        locRpt.SetParameters(parameters);
                    }

                    if (RptType == ApplicationType.IO)
                    {
                        parameters = new ReportParameter("DraftMode", IOType.ToString());
                        locRpt.SetParameters(parameters);
                    }
                    if (RptType == ApplicationType.MSI && (RptSubType == 1 || RptSubType == 2))
                    {
                        parameters = new ReportParameter("MisRateFormat", MisRateDecFomat.ToString());
                        locRpt.SetParameters(parameters);
                    }
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
                            if (!FromExternal)
                                lblBreadCrum.Text = this.GetLocalResourceObject("Print").ToString() + " >> " + dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString();
                            parameters = new ReportParameter("HeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING"))
                        {
                            parameters = new ReportParameter("SubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        
                    }
                }


                locRpt.EnableHyperlinks = true;
                if (RptType == ApplicationType.IO || RptType == ApplicationType.SO || RptType == ApplicationType.SOD)
                {
                    parameters = new ReportParameter("hand", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["hand"]));
                    //string str=Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]);
                    locRpt.SetParameters(parameters);
                }
                parameters = new ReportParameter("Logo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                //string str=Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]);
                locRpt.SetParameters(parameters);

                // footer = "Printed by " + currentUser.EmpName + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
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
                    case ApplicationType.SI:
                    case ApplicationType.SO:
                    case ApplicationType.SOD:
                        parameters = new ReportParameter("IsTaxForOtherChargeSales", IsTaxForOtherChargeSales.ToString());
                        locRpt.SetParameters(parameters);
                        break;
                    case ApplicationType.DO:
                        if (RptSubType == 7 || RptSubType == 8)
                        {
                            parameters = new ReportParameter("NetWt", NetWt.ToString());
                            locRpt.SetParameters(parameters);
                            parameters = new ReportParameter("GrWt", GrWt.ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (RptSubType == 6)
                        {
                            parameters = new ReportParameter("SignaturePath", sPath);
                            locRpt.SetParameters(parameters);
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
                // dtReptDtls = GetReportData();
                //if (dtReptDtls.Rows.Count > 0)
                //{
                dsReportDet = new ReportDataSource("PVHeader", currentEntity.SPFIN_PAYMENT_VND_VOUCHER_RPT_TEST(1));
                locRpt = rvViewReport.LocalReport;
                locRpt.ReportPath = string.Empty;
                locRpt.ReportPath = Server.MapPath("RptTemplateWithLogo.rdlc");
                locRpt.EnableHyperlinks = true;
                //rvViewReport.LocalReport.DataSources.Clear();
                locRpt.EnableExternalImages = true;
                //To set Header & footer properties
                //SetReportParameters(locRpt);

                rvViewReport.LocalReport.DataSources.Add(dsReportDet);
                rvViewReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                rvViewReport.LocalReport.Refresh();
                //}
                //else
                //{
                //    rvViewReport.Visible = false;
                //}
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        /// <summary>
        /// Method for Tree Binding
        /// </summary>
        public void BindTree()
        {
            try
            {
                TreeNode root;
                switch (RptType)
                {

                    case ApplicationType.AS:
                        AccountMstService AccountMstService = null;
                        List<FIN_COA_MST> accountMstList;
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
                        accountMstObj.COA_PK = 0;//CurrPK
                        accountMstObj.COA_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        accountMstList = AccountMstService.GetFinCoaMst(accountMstObj, serviceUtilityObj);


                        trvAccounts.Nodes.Clear();
                        //root = new TreeNode("Accounts", "0");
                        //trvAccounts.Nodes.Add(root);
                        if (accountMstList != null)
                        {
                            List<FIN_COA_MST> acntList = (from accountList in accountMstList
                                                          where accountList.COA_LEVEL == 1
                                                          select accountList).ToList();
                            foreach (FIN_COA_MST accounts in acntList)
                            {
                                root = new TreeNode();
                                root.Text = accounts.COA_NAME;
                                root.Value = accounts.COA_PK.ToString();
                                root.SelectAction = TreeNodeSelectAction.None;
                                CreateNode(root, accountMstList);
                                trvAccounts.Nodes.Add(root);
                            }
                        }
                        break;
                    case ApplicationType.BRC:
                    case ApplicationType.SAS:
                        CommonService commonService;
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
                        commonService = null;
                        if (finCoaSubTypeCfgList[0].CST_REL_QUERY != null)
                        {
                            relquery = finCoaSubTypeCfgList[0].CST_REL_QUERY == null ? string.Empty : finCoaSubTypeCfgList[0].ADM_QUERIES_CFG1.QRY_QUERY;
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
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.ErpRes.Information + "');", true);
            }
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
                case ApplicationType.BRC:
                case ApplicationType.SAS:
                    emptyXml = "<ROOT><ACCHEAD><FROM_DATE/><TO_DATE/><BIZUNIT/><CURRENCY/><CST_PK/></ACCHEAD><SUBACCOUNT><SAC_PK/></SUBACCOUNT>/</ROOT>";
                    SelectedAccountsList.ReadXml(new System.IO.StringReader(emptyXml));
                    SelectedAccountsList.Tables["ACCHEAD"].Rows[0].Delete();
                    SelectedAccountsList.Tables["SUBACCOUNT"].Rows[0].Delete();

                    if (hdfSubAccount.Value != "0" && hdfSubAccount.Value != string.Empty && txtSubAccount.Text.Trim() != this.GetGlobalResourceObject("Messages", "AutoDefaultValue").ToString())
                    {
                        dr = SelectedAccountsList.Tables["SUBACCOUNT"].NewRow();
                        dr["SAC_PK"] = hdfSubAccount.Value;
                        SelectedAccountsList.Tables["SUBACCOUNT"].Rows.Add(dr);
                    }
                    else
                    {
                        foreach (TreeNode node in trvAccounts.CheckedNodes)
                        {
                            dr = SelectedAccountsList.Tables["SUBACCOUNT"].NewRow();
                            dr["SAC_PK"] = node.Value;
                            SelectedAccountsList.Tables["SUBACCOUNT"].Rows.Add(dr);
                        }
                    }
                    break;
            }

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
            ddlSubLedger.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        public void GetInitialDate()
        {
            string fromDate;
            string toDate;
            if (DateTime.Now.Month >= 4)
            {
                fromDate = "01-Apr-" + DateTime.Now.Year.ToString();
            }
            else
            {
                fromDate = "01-Apr-" + DateTime.Now.AddYears(-1).ToString();
            }
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
                        if ((hdfAccount.Value.Trim() == string.Empty || txtAccount.Text == this.GetGlobalResourceObject("Messages", "AutoDefaultValue").ToString()) && trvAccounts.CheckedNodes.Count == 0)
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_Account").ToString() : "^" + this.GetLocalResourceObject("Err_Account").ToString();
                            flag = false;
                        }

                        break;
                    case ApplicationType.SAS:
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
                        if (ddlSubLedger.SelectedValue == "-1")
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_Subledger").ToString() : "^" + this.GetLocalResourceObject("Err_Subledger").ToString();
                            flag = false;
                        }
                        if ((hdfSubAccount.Value.Trim() == string.Empty || txtSubAccount.Text == this.GetGlobalResourceObject("Messages", "AutoDefaultValue").ToString()) && trvAccounts.CheckedNodes.Count == 0)
                        {
                            errMsg = errMsg == string.Empty ? this.GetLocalResourceObject("Err_Account").ToString() : "^" + this.GetLocalResourceObject("Err_Account").ToString();
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
                    attachmentFileName = RptType + RptSubType + RecPK + ".pdf";
                attachmentFilePath = savePath + attachmentFileName;
                attachmentFileFormat = ".pdf";
                attachmentFileContentType = "application/pdf";

                //if file is exists delete file
                if (File.Exists(attachmentFilePath))
                    File.Delete(attachmentFilePath);

                format = "PDF";
                byte[] bytes = locRpt.Render(format, "", out mimeType, out encoding, out extension, out streamids, out warnings);
                /* stream to use for attachment - can implement later
                Stream stream = new MemoryStream();
                stream.Write(bytes, 0, bytes.Length);
                SendMail(stream);
                 */
                //save the pdf byte to the folder
                FileStream fs = new FileStream(attachmentFilePath, FileMode.OpenOrCreate);
                byte[] data = new byte[fs.Length];
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                IsPdfGenerated = true;

            }
            catch (Exception ex)
            {
                IsPdfGenerated = false;
                throw ex;
            }
        }

        /// <summary>
        /// Convert to & open pdf
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
                byte[] bytes = locRpt.Render(format, "", out mimeType, out encoding, out extension, out streamids, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader("content-disposition", "attachment; filename= " + attachmentFileName);
                Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                Response.Flush(); // send it to the client to download  
                Response.End();

            }
            catch (Exception ex)
            {
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
                            if (RptType == ApplicationType.QAC)
                            {
                                GetFieldValues(RptType);
                                SetFieldValues(RptType);
                            }
                            else if (RptType == ApplicationType.TB)
                            {
                                if (!ValidateForm())
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                                }
                                else
                                {
                                    SetFieldValues(RptType);
                                }
                            }
                            else if (RptType == ApplicationType.AS)
                            {
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
                            }
                            else if (RptType == ApplicationType.SAS || RptType == ApplicationType.BRC)
                            {
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
                            }
                            break;

                        case ActionsEnum.CANCEL:
                            //Response.Redirect(ReturnUrl);
                            if (Convert.ToString(RecPK) == string.Empty)
                                Response.Redirect(Convert.ToString(Request.Url));
                            else if (hdfRefUrl.Value != string.Empty)
                                Response.Redirect(hdfRefUrl.Value);

                            break;
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
                            // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AccountsTree", "ShowAccountsTree();", true);
                        }
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlSubLedger")
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
                RptType = Request.QueryString["APPTYPE"].ToString();
                hdfAppType.Value = RptType;
                hdfRefUrl.Value = "";

                RptSubType = Request.QueryString["APPSUBTYPE"] != string.Empty ? Convert.ToInt32(Request.QueryString["APPSUBTYPE"]) : 0;
                IOType = Request.QueryString["IOType"] != string.Empty ? Convert.ToInt32(Request.QueryString["IOType"]) : 0;
                IOReview = Request.QueryString["IOReview"] != string.Empty ? Convert.ToInt32(Request.QueryString["IOReview"]) : 0;
                if (Request.QueryString["ID"].ToString().Trim() != string.Empty)
                {
                    RecPK = Convert.ToInt32(Request.QueryString["ID"]);
                    // ReturnUrl = Convert.ToString(Request.UrlReferrer);
                    //ReturnUrl = hdfRefUrl.Value;

                }
                else
                {
                    ReturnUrl = Convert.ToString(Request.Url);
                    hdfRefUrl.Value = ReturnUrl;
                }

                //
                if (Request.QueryString["RevID"] != null)
                {
                    int RID = 0;
                    RevPK = Int32.TryParse(Request.QueryString["RevID"].ToString().Trim(), out RID) ? RID : 0;
                }

                if (Request.QueryString["VERSION"] != null && Request.QueryString["VERSION"].ToString().Trim() != string.Empty)
                {
                    VoucherVersion = Convert.ToInt16(Request.QueryString["VERSION"]);
                }
                ShowHideDesc = Request.QueryString["SHOWHIDEDESCRIPTION"] != null
                        ? Request.QueryString["SHOWHIDEDESCRIPTION"].ToString().Trim() != string.Empty
                            ? Convert.ToInt32(Request.QueryString["SHOWHIDEDESCRIPTION"])
                            : 0
                        : 0;
                InternalFlag = Request.QueryString["INTERNALFLAG"] != null
                        ? Request.QueryString["INTERNALFLAG"].ToString().Trim() != string.Empty
                            ? Convert.ToInt32(Request.QueryString["INTERNALFLAG"])
                            : 0
                        : 0;
                dtCmp = BusinessLogic.CommonManagement.CommonBL.GetDeptCompany(currentUser.CurrentDeptPK);
                if (dtCmp != null && dtCmp.Rows.Count > 0)
                    CompanyPK = Convert.ToInt32(dtCmp.Rows[0][Resources.DataFieldRes.DeptCompany].ToString());

                if (hdfRefUrl.Value == string.Empty)
                    hdfRefUrl.Value = Convert.ToString(Request.UrlReferrer);


                divQAC.Visible = false;
                divTrialBal.Visible = false;
                divGL.Visible = false;
                divAccPopUp.Visible = false;
                if (RptType == ApplicationType.PR || RptType == ApplicationType.RFQ)
                    btnSearch.Visible = false;
                else
                    btnSearch.Visible = true;

                GetInitialDate();
                lblBreadCrum.Text = this.GetLocalResourceObject("Print").ToString();
                switch (RptType)
                {
                    case ApplicationType.GIN:
                    case ApplicationType.GRN:
                    case ApplicationType.PR:
                    case ApplicationType.PO:
                    case ApplicationType.SO:
                    case ApplicationType.SOD:
                    case ApplicationType.IO:
                    case ApplicationType.DO:
                    case ApplicationType.SI:
                    case ApplicationType.OPLN:
                    case ApplicationType.MSI:
                    case ApplicationType.PRJ:
                        GetFieldValues(RptType);
                        SetFieldValues(RptType);
                        break;
                    case ApplicationType.EMI:
                        GetFieldValues(RptType);
                        SetFieldValues(RptType);
                        break;
                    case ApplicationType.VP:
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
                    case ApplicationType.SIJ:
                        SetFieldValues(RptType);
                        break;
                    case ApplicationType.CRJ:
                    case ApplicationType.VPJ:
                    case ApplicationType.PIJ:
                    case ApplicationType.JV:
                        TrxRefType = Request.QueryString["TRXTYPE"].ToString();
                        SetFieldValues(RptType);
                        break;
                    case ApplicationType.CN:
                    case ApplicationType.DN:
                    case ApplicationType.CQTN:
                    case ApplicationType.COA:
                        SetFieldValues(RptType);
                        break;
                    case ApplicationType.TB:
                        divTrialBal.Visible = true;
                        SetFieldValues(RptType);
                        break;
                    case ApplicationType.AS:
                        divTrialBal.Visible = true;
                        divGL.Visible = true;
                        divAS.Visible = true;
                        divSAS.Visible = false;
                        BindTree();
                        break;
                    case ApplicationType.LST_VCH:
                        //TrxRefType = Request.QueryString["TRXTYPE"].ToString();
                        SetFieldValues(RptType);
                        break;
                    case ApplicationType.BRC:
                    case ApplicationType.SAS:
                        divTrialBal.Visible = true;
                        divGL.Visible = true;
                        divAS.Visible = false;
                        divSAS.Visible = true;
                        BindSubLedger();
                        if (RptType == ApplicationType.BRC)
                        {
                            ddlSubLedger.SelectedValue = "11";   // 11 for Bank 
                            ddlSubLedger.Visible = false;
                            //txtSubAccount.Attributes.Remove("cssclass");
                            lblSubLedger.Text = "Bank";
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
                    case ApplicationType.CBR:
                        GroupBy = Request.QueryString["GroupBy"].ToString();
                        ForMonth = Request.QueryString["ForMoth"].ToString();
                        if (GroupBy == "1")
                        {
                            if (Request.QueryString["CusID"] != null)
                            {
                                CusID = Request.QueryString["CusID"].ToString() != "" ? Convert.ToInt32(Request.QueryString["CusID"].ToString()) : 0;
                            }
                            else
                            {
                                CusID = 0;
                            }
                            //GroupBy = "1";
                        }
                        if (GroupBy == "2")
                        {
                            if (Request.QueryString["ItemID"] != null)
                            {
                                ItemID = Request.QueryString["ItemID"].ToString() != "" ? Convert.ToInt32(Request.QueryString["ItemID"].ToString()) : 0;
                            }
                            else
                            {
                                ItemID = 0;
                            }
                            //GroupBy = "2";
                        }

                        //else
                        //{
                        //    //ForMonth = Request.QueryString["ForMoth"].ToString();
                        //    //GroupBy = Request.QueryString["GroupBy"].ToString();
                        //}
                        SetFieldValues(RptType);
                        break;
                    case ApplicationType.SALFRCST:
                        ForMonth = CommonFunctions.DecryptKey(Request.QueryString["Date"].ToString());
                        SetFieldValues(RptType);
                        break;
                }
            }
        }
        #endregion

        #region Generate and save reports for mailing purpose
        #region Save Report For Mail
        /// <summary>
        /// Save PDF to a particular location
        /// </summary>
        /// <returns>Saved file path</returns>
        public string SaveReportForMail()
        {
            DataTable dtCmp;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            RptType = APPTYPE;
            RptSubType = APPSUBTYPE;
            RecPK = TransactionPk;
            dtCmp = BusinessLogic.CommonManagement.CommonBL.GetDeptCompany(currentUser.CurrentDeptPK);
            if (dtCmp != null && dtCmp.Rows.Count > 0)
                CompanyPK = Convert.ToInt32(dtCmp.Rows[0][Resources.DataFieldRes.DeptCompany].ToString());
            switch (RptType)
            {
                case ApplicationType.SO:
                case ApplicationType.SOD:
                case ApplicationType.IO:
                    GetFieldValues(RptType);
                    SetFieldValuesForMail(RptType);
                    break;
            }
            return string.Empty;
        }
        #endregion
        #region Set Field Values For Mail
        /// <summary>
        /// All Field values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        /// <param name="appType"></param>
        private void SetFieldValuesForMail(string appType)
        {
            try
            {
                LocalReport locRpt;
                ReportParameter ReportParam;

                cm = new CommonService();
                locRpt = null;
                ReportViewer rvViewReport = new ReportViewer();
                rvViewReport.LocalReport.DataSources.Clear();
                rvViewReport.Visible = true;

                locRpt = rvViewReport.LocalReport;

                rvViewReport.LocalReport.DataSources.Clear();
                locRpt.EnableExternalImages = true;
                rvViewReport.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                int? CompanyPK = 0;

                switch (appType)
                {
                    #region SO
                    case ApplicationType.SO:
                    case ApplicationType.SOD:
                    case ApplicationType.IO:
                        ReportDataSource dsSOHeaderDtls;
                        ReportDataSource dsSOProductDtls;
                        ReportDataSource dsSOMoreDtls;
                        if (dsSaleOrder != null)
                        {
                            SetReportParametersForMail(locRpt);
                            DataTable dtSOHeaderDtls = dsSaleOrder.Tables[0];
                            DataTable dtSOProductDtls = dsSaleOrder.Tables[1];
                            DataTable dtSOMoreDtls = dsSaleOrder.Tables[2];
                            dsSOHeaderDtls = new ReportDataSource("SOHeaderDtls", dtSOHeaderDtls);
                            dsSOProductDtls = new ReportDataSource("SOProductDtls", dtSOProductDtls);
                            dsSOMoreDtls = new ReportDataSource("SOMoreDtls", dtSOMoreDtls);
                            rvViewReport.LocalReport.DataSources.Add(dsSOHeaderDtls);
                            rvViewReport.LocalReport.DataSources.Add(dsSOProductDtls);
                            rvViewReport.LocalReport.DataSources.Add(dsSOMoreDtls);
                            if (dtSOHeaderDtls != null & dtSOHeaderDtls.Rows.Count > 0)
                            {
                                CompanyPK = Convert.ToInt32(dtSOHeaderDtls.Rows[0]["SOH_COMPANY"].ToString());
                            }
                            rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                        }
                        else
                        {
                            rvViewReport.Visible = false;
                        }
                        break;
                    #endregion
                    #region (5-DataSet) WO
                    case ApplicationType.PRJ:
                        if (dsRptDataset != null)
                        {
                            SetReportParameters(locRpt);
                            switch (RptSubType)
                            {
                                case 1:
                                    dtRptDatatable1 = dsRptDataset.Tables[0];
                                    dsRptDataSource1 = new ReportDataSource("DataSet1", dtRptDatatable1);
                                    rvViewReport.LocalReport.DataSources.Add(dsRptDataSource1);
                                    rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                                    break;
                                default:
                                    dtRptDatatable1 = dsRptDataset.Tables[0];
                                    dtRptDatatable2 = dsRptDataset.Tables[1];
                                    dtRptDatatable3 = dsRptDataset.Tables[2];
                                    dtRptDatatable4 = dsRptDataset.Tables[3];
                                    dtRptDatatable5 = dsRptDataset.Tables[4];
                                    dsRptDataSource1 = new ReportDataSource("DataSet1", dtRptDatatable1);
                                    dsRptDataSource2 = new ReportDataSource("DataSet2", dtRptDatatable2);
                                    dsRptDataSource3 = new ReportDataSource("DataSet3", dtRptDatatable3);
                                    dsRptDataSource4 = new ReportDataSource("DataSet4", dtRptDatatable4);
                                    dsRptDataSource5 = new ReportDataSource("DataSet5", dtRptDatatable5);
                                    rvViewReport.LocalReport.DataSources.Add(dsRptDataSource1);
                                    rvViewReport.LocalReport.DataSources.Add(dsRptDataSource2);
                                    rvViewReport.LocalReport.DataSources.Add(dsRptDataSource3);
                                    rvViewReport.LocalReport.DataSources.Add(dsRptDataSource4);
                                    rvViewReport.LocalReport.DataSources.Add(dsRptDataSource5);
                                    rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                                    break;
                            }
                            break;
                        }
                        else
                        {
                            //divReportViewer.Visible = false;
                            rvViewReport.Visible = false;
                            //divNodata.Visible = true;
                        }
                        break;
                        #endregion
                }
                rvViewReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                rvViewReport.LocalReport.Refresh();
                SavePDFForMail(locRpt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Save PDF For Mail
        /// <summary>
        /// Convert the report as PDF and save
        /// </summary>
        private void SavePDFForMail(LocalReport locRpt)
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

                //savePath = Server.MapPath("~/") + Resources.PageURL.AttachmentPath;
                savePath = string.Empty;
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                {
                    savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\Attachments";
                    if (!Directory.Exists(savePath))
                        Directory.CreateDirectory(savePath);
                    savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\Attachments\\";
                }
                else
                {
                    savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "Attachments";
                    if (!Directory.Exists(savePath))
                        Directory.CreateDirectory(savePath);
                    savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "Attachments\\";
                }
                attachmentFilePath = string.Empty;
                if (!string.IsNullOrEmpty(DocumentName))
                {
                    attachmentFileName = DocumentName;//+ ".pdf";
                    attachmentFilePath = savePath + attachmentFileName;
                }
                else
                {
                    attachmentFileName = RptType + RptSubType + RecPK + ".pdf";
                    attachmentFilePath = savePath + attachmentFileName;
                }

                attachmentFileFormat = ".pdf";
                attachmentFileContentType = "application/pdf";

                //if file is exists delete file
                if (File.Exists(attachmentFilePath))
                    File.Delete(attachmentFilePath);

                format = "PDF";
                byte[] bytes = locRpt.Render(format, "", out mimeType, out encoding, out extension, out streamids, out warnings);
                /* stream to use for attachment - can implement later
                Stream stream = new MemoryStream();
                stream.Write(bytes, 0, bytes.Length);
                SendMail(stream);
                 */
                //save the pdf byte to the folder
                FileStream fs = new FileStream(attachmentFilePath, FileMode.OpenOrCreate);
                byte[] data = new byte[fs.Length];
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();

                #region Save Mail Attachments
                long? result = null;
                MailAttachmentBO objAttachmentBO = new MailAttachmentBO();
                objAttachmentBO.ACTIVE = (int)DbActiveStatus.ACTIVE;
                objAttachmentBO.ADD_APP_PK = RecPK;
                objAttachmentBO.ADD_APP_SUB_TYPE = RptSubType;
                objAttachmentBO.ADD_APP_TYPE = RptType;
                objAttachmentBO.ADD_DESC = string.Empty;
                objAttachmentBO.ADD_NAME = attachmentFileName;
                objAttachmentBO.ADD_PATH = attachmentFilePath;
                objAttachmentBO.ADD_SL_NO = 1;
                objAttachmentBO.ADD_TITLE = attachmentFileName;
                objAttachmentBO.ADD_TYPE = format;
                objAttachmentBO.BIZUNIT = currentUser.SBUID;
                objAttachmentBO.USER_PK = currentUser.PKUser;
                objAttachmentBO.ADD_VERSION = Version;
                result = BusinessLogic.Mailer.MailerBL.SaveMailAttachment(objAttachmentBO);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Set Report Parameters For Mail
        /// <summary>
        /// To set report parameters
        /// </summary>
        /// <param name="locRpt"></param>
        private void SetReportParametersForMail(LocalReport locRpt)
        {
            try
            {
                ReportParameter parameters;
                DataSet dsParamSettings;
                string footer;
                string rptName = string.Empty;
                footer = string.Empty;
                string signaturePath = string.Empty;
                dsParamSettings = new DataSet();
                DataTable dt = ConfigurationSettings();
                locRpt.ReportPath = string.Empty;
                foreach (SPADM_APP_SUB_TYPE_DATA_GET_Result sa in AppTypeDetailsList)
                {
                    if (sa.AST_CODE == ApplicationType.SID && SwapBuyer == 1)
                    {
                        string rdlcName = sa.AST_OP_FILE1;
                        rptName = rdlcName.Replace(".", "_1.");
                    }
                    else
                    {
                        rptName = sa.AST_OP_FILE1;//
                    }
                    if (IsRepeatSIheader)
                    {
                        if (sa.AST_CODE == ApplicationType.SID || sa.AST_CODE == ApplicationType.SIE)
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
                    locRpt.ReportPath = Server.MapPath("~\\Reports\\" + rptName);
                    parameters = new ReportParameter("QMSRef", sa.AST_QMS_REF);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("HideQMSRef", sa.AST_QMS_VISIBILITY.ToString());
                    locRpt.SetParameters(parameters);
                    if (sa.AST_RPT_SETTINGS != null)
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(sa.AST_RPT_SETTINGS)));

                    if (RptType == ApplicationType.PO || RptType == ApplicationType.SI || RptType == ApplicationType.RFQ || RptType == ApplicationType.DO)
                    {
                        parameters = new ReportParameter("ApprovedByName", sa.AST_APPROVED_USER);
                        locRpt.SetParameters(parameters);
                        if (Convert.ToString(sa.AST_APPROVED_SIGN) != string.Empty)
                        {
                            bool fileExists = false;
                            // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN); 
                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                            {
                                if (File.Exists(Server.MapPath("~\\Reports\\Images\\" + sa.AST_APPROVED_SIGN)))
                                {
                                    fileExists = true;
                                    signaturePath = "file:///" + Server.MapPath("~\\Reports\\Images\\" + sa.AST_APPROVED_SIGN);
                                }

                            }
                            else
                            {
                                if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + sa.AST_APPROVED_SIGN))
                                {
                                    fileExists = true;
                                    signaturePath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + sa.AST_APPROVED_SIGN;
                                }
                            }
                            if (fileExists)
                            {
                                parameters = new ReportParameter("ApprovedBySign", signaturePath);
                                locRpt.SetParameters(parameters);
                            }

                            // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath("~\\Reports\\Images\\" + sa.AST_APPROVED_SIGN)); 
                            // locRpt.SetParameters(parameters);
                        }
                    }
                    else if (RptType == ApplicationType.SO || RptType == ApplicationType.SOD || RptType == ApplicationType.IO)
                    {
                        signaturePath = string.Empty;
                        string signatureAccepted = string.Empty;
                        string signaturePathAccepted = string.Empty;
                        if (dsSaleOrder != null)
                        {
                            if (dsSaleOrder.Tables.Count > 0)
                            {
                                if (dsSaleOrder.Tables[0].Rows.Count > 0)
                                {
                                    GetClientCode();//For getting client id
                                    string signature = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.SOApprovedSign].ToString();
                                    signatureAccepted = dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.SOAcceptedSign].ToString();//SOH_ACCEPTED_SIGN
                                    int SOstatus = Convert.ToInt32(dsSaleOrder.Tables[0].Rows[0][Resources.DataFieldRes.SOStatus]);
                                    if (clientCode == ClientCode.MMT.ToString())
                                    {
                                        if (SOstatus == 2)//For MMT shows signature only in the IO generation(BugID:- 6544)
                                        {
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
                                    else
                                    {
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
                            }
                            parameters = new ReportParameter("ApprovedBySign", signaturePath);
                            locRpt.SetParameters(parameters);
                            parameters = new ReportParameter("AcceptedBySign", signaturePathAccepted);
                            locRpt.SetParameters(parameters);
                        }
                    }
                    else if (RptType == ApplicationType.TB || RptType == ApplicationType.AS || RptType == ApplicationType.SAS || RptType == ApplicationType.BRC)
                    {
                        parameters = new ReportParameter("FromDate", txtFromDate.Text.Trim());
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("ToDate", txtToDate.Text.Trim());
                        locRpt.SetParameters(parameters);
                    }
                    else if (RptType == ApplicationType.SALFRCST)
                    {
                        parameters = new ReportParameter("ToDate", ForMonth);
                        locRpt.SetParameters(parameters);
                    }

                    if (RptType == ApplicationType.DN || RptType == ApplicationType.CN)
                    {
                        parameters = new ReportParameter("ParamReason", RptType == ApplicationType.DN ? this.GetLocalResourceObject("lblDNReason").ToString() : this.GetLocalResourceObject("lblCNReason").ToString());
                        locRpt.SetParameters(parameters);
                    }

                    if (RptType == ApplicationType.CBR)
                    {
                        parameters = new ReportParameter("GroupBy", GroupBy);
                        locRpt.SetParameters(parameters);
                    }

                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    //string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
                    string currencyformat = "#" + currencysep + "#0.";
                    string NoFormat = "#" + currencysep + "#0.";
                    string WeightFormat = "#" + currencysep + "#0.";//Quantity Format
                    string ExchRateDigt = "#" + currencysep + "#0.";
                    string RateDeciDigt = "#" + currencysep + "#0.";
                    string MisRateDecFomat = "#" + currencysep + "#0.";
                    string currencydecimals = "";
                    string Nodecimal = string.Empty;
                    string Weightdecimal = string.Empty;//Weight Decimal
                    string ExchRateDigit = string.Empty;
                    string RateDecimalDigit = string.Empty;
                    string MisRateDecimal = string.Empty;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int curdigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < curdigit; i++)
                        {
                            currencydecimals += "0";
                        }
                        int WeightDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "WeightDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < WeightDigit; i++)
                        {
                            Weightdecimal += "0";
                        }
                        int NoDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());

                        for (int i = 0; i < NoDigit; i++)
                        {
                            Nodecimal += "0";
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
                        int MisRate = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "MiscRateDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < MisRate; i++)
                        {
                            MisRateDecimal += "0";
                        }
                    }
                    else
                    {
                        currencydecimals = "00";
                        Nodecimal = "00";
                    }
                    currencyformat = currencyformat + currencydecimals;
                    NoFormat = NoFormat + Nodecimal;
                    ExchRateDigt = ExchRateDigt + ExchRateDigit;
                    RateDeciDigt = RateDeciDigt + RateDecimalDigit;
                    WeightFormat = WeightFormat + Weightdecimal;
                    MisRateDecFomat = MisRateDecFomat + MisRateDecimal;
                    // currencyformat = {0:n} + currencydecimals;
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
                    if ((RptType == ApplicationType.DO && (RptSubType == 2 || RptSubType == 7)) || RptType == ApplicationType.SI)
                    {
                        parameters = new ReportParameter("WeightFormat", WeightFormat);
                        locRpt.SetParameters(parameters);
                    }

                    if (RptType == ApplicationType.IO)
                    {
                        parameters = new ReportParameter("DraftMode", IOType.ToString());
                        locRpt.SetParameters(parameters);
                    }
                    if (RptType == ApplicationType.MSI && (RptSubType == 1 || RptSubType == 2))
                    {
                        parameters = new ReportParameter("MisRateFormat", MisRateDecFomat.ToString());
                        locRpt.SetParameters(parameters);
                    }
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
                            //lblBreadCrum.Text = this.GetLocalResourceObject("Print").ToString() + " >> " + dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString();
                            parameters = new ReportParameter("HeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING"))
                        {
                            parameters = new ReportParameter("SubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                    }
                }


                locRpt.EnableHyperlinks = true;
                if (RptType == ApplicationType.IO || RptType == ApplicationType.SO || RptType == ApplicationType.SOD)
                {
                    parameters = new ReportParameter("hand", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["hand"]));
                    //string str=Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]);
                    locRpt.SetParameters(parameters);
                }
                parameters = new ReportParameter("Logo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                //string str=Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]);
                locRpt.SetParameters(parameters);

                footer = "Printed by " + currentUser.EmpName + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                parameters = new ReportParameter("FooterText", footer);
                locRpt.SetParameters(parameters);
                switch (RptType)
                {
                    case ApplicationType.SI:
                    case ApplicationType.SO:
                    case ApplicationType.SOD:
                        parameters = new ReportParameter("IsTaxForOtherChargeSales", IsTaxForOtherChargeSales.ToString());
                        locRpt.SetParameters(parameters);
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #endregion

    }
}