using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.Reporting.WebForms;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using BusinessObject.HRMS.Employee;
using BusinessLogic.ReportsManagement;
using System.IO;
using BusinessLogic.CommonManagement;
using System.Threading;
using BusinessLogic.HRMS.Payroll;
using BusinessObject.HRMS.Payroll;
using BusinessLogic.HRMS.Admin.Masters;
using System.Xml;
using ERPData;
using ERPService;
using System.Text.RegularExpressions;
using System.Security;
using System.Security.Permissions;

namespace HRMS.Reports
{
    public partial class GenerateReport : ERP.Store.UI.MyBasePage
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
                return (string)this.ViewState["ReportType"];
            }
            set
            {
                this.ViewState["ReportType"] = value;
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
        public int DeptPK
        {
            get
            {
                return this.ViewState["DeptPK"] == null ? -1 : Convert.ToInt32(this.ViewState["DeptPK"]);
            }
            set
            {
                this.ViewState["DeptPK"] = value;
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
        public string ExternalPDFURL
        {
            get
            {
                return this.ViewState["ExternalPDFURL"] == null ? string.Empty : (string)this.ViewState["ExternalPDFURL"];
            }
            set
            {
                this.ViewState["ExternalPDFURL"] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        public int RptSubType
        {
            get
            {
                return (int)this.ViewState["ReportSubType"];
            }
            set
            {
                this.ViewState["ReportSubType"] = value;
            }
        }
        /// <summary>
        /// To maintain Report Pk
        /// </summary>
        public int RecPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["RecPK"]);
            }
            set
            {
                this.ViewState["RecPK"] = value;
            }
        }
        private int CurPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["CurPK"]);
            }
            set
            {
                this.ViewState["CurPK"] = value;
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
        /// To maintain Report Iteration
        /// </summary>
        private int Iteration
        {
            get
            {
                return Convert.ToInt32(this.ViewState["Iteration"]);
            }
            set
            {
                this.ViewState["Iteration"] = value;
            }
        }
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
        /// Department Company PK
        /// </summary>
        private int? CompanyPK
        {
            get
            {
                return (int?)this.ViewState[ViewstateStrings.CompanyPK];   //this.ViewState["CompanyPK"] == null ? 0 : 
            }
            set
            {
                this.ViewState[ViewstateStrings.CompanyPK] = value;
            }
        }

        //Income tax list
        private List<Income_Tax_01BO.Section> IncomeTax_SectionsList
        {
            get
            {
                return ViewState["IncomeTax_Sections"] == null ? new List<Income_Tax_01BO.Section>() : (List<Income_Tax_01BO.Section>)ViewState["IncomeTax_Sections"];
            }
            set
            {
                ViewState["IncomeTax_Sections"] = value;
            }
        }


        private string HLURL
        {
            get
            {
                return (this.ViewState["HLURL"].ToString());
            }
            set
            {
                this.ViewState["HLURL"] = value;
            }
        }

        /// <summary>
        /// To maintain the Transaction Ref Type in viewstate
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
        #endregion
        #region Variables
        // Holds the current logged in user
        private BusinessObject.User currentUser;
        DataTable dtAppTypeDetails;
        // DataSet dsReportDetails;
        DataTable dtHeader;
        DataTable dtDetails1;
        DataTable dtLabelRptConfig;
        DataSet dsHrms;
        DataSet dsList;
        ReportDataSource rdsHeader, rdsDetails1, rdsDetails2, rdsDetails3, rdsDetails4;
        private string attachmentFilePath;
        private string attachmentFileFormat;
        private string attachmentFileContentType;
        private string attachmentFileName;
        private static string ReturnUrl;
        LocalReport locRpt;
        List<Income_Tax_01BO.SectionItems> SectionItemsList;
        DataTable dtSectionItems;
        private ERPEntities currentEntity;
        private CommonService cm;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;

        public string LogoPath = string.Empty;
        private string OutputLogoPath = string.Empty;

        #region Crystal report variables
        private ReportDocument reportDocument;
        private ParameterField paramField;
        private ParameterFields rptParamFields;
        private ParameterDiscreteValue paramDiscreteValue;
        #endregion
        #endregion
        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;
            if (!IsPostBack)
            {
                RptType = Request.QueryString["APPTYPE"].ToString();
                RptSubType = Request.QueryString["APPSUBTYPE"] != string.Empty ? Convert.ToInt32(Request.QueryString["APPSUBTYPE"]) : 0;
                Iteration = Request.QueryString["Iteration"] != string.Empty ? Convert.ToInt32(Request.QueryString["Iteration"]) : 0;
                CurPK = Request.QueryString["CurPK"] != string.Empty ? Convert.ToInt32(Request.QueryString["CurPK"]) : 0;
                IsExcelPrint = Request.QueryString["ISEXCELPRINT"] != null ? Convert.ToInt32(Request.QueryString["ISEXCELPRINT"]) : 0;

                HLURL = Request.QueryString["HLURL"] != null ? (Request.QueryString["HLURL"]) : string.Empty;

                if (Request.QueryString["ID"].ToString().Trim() != string.Empty)
                {
                    RecPK = Convert.ToInt32(Request.QueryString["ID"]);
                }
                else
                {
                    ReturnUrl = Convert.ToString(Request.Url);
                    hdfRefUrl.Value = ReturnUrl;
                }
                if (Request.QueryString["VERSION"] != null && Request.QueryString["VERSION"].ToString().Trim() != string.Empty)
                {
                    VoucherVersion = Convert.ToInt16(Request.QueryString["VERSION"]);
                }
                if (hdfRefUrl.Value == string.Empty)
                    hdfRefUrl.Value = Convert.ToString(Request.UrlReferrer);
                GenerateReports();
            }
        }

        public bool GenerateReports()
        {
            bool retFlag = false;
            #region Report
            GetFieldValues(ApplicationType.REPORTCONFIG);
            if (dtAppTypeDetails == null)
            {
                ClearCrystalReport();
                return false;
            }
            switch (((string[])dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString().Split('.'))[1].Trim().ToLower())
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
                        #region PAYRL
                        case ApplicationType.PAYRL:
                            GetFieldValues(RptType);
                            SetFieldValuesCrystal(RptType, dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString());
                            break;
                        #endregion
                        #region FSMT
                        case ApplicationType.FSMT:
                            GetFieldValues(RptType);
                            SetFieldValuesCrystal(RptType, dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString());
                            break;
                        #endregion
                        #region HLDM
                        case ApplicationType.HLDM:
                            GetFieldValues(RptType);
                            SetFieldValuesCrystal(RptType, dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString());
                            break;
                        #endregion
                        #region SALPYMT
                        case ApplicationType.SALPYMT:
                            GetFieldValues(RptType);
                            SetFieldValuesCrystal(RptType, dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString());
                            break;
                        #endregion
                        #region ITC
                        case ApplicationType.ITC:
                            GetFieldValues(RptType);
                            SetFieldValuesCrystal(RptType, dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString());
                            break;
                        #endregion
                        #region EMPATTND
                        case ApplicationType.EMPATTND:
                            GetFieldValues(RptType);
                            SetFieldValuesCrystal(RptType, dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString());
                            break;
                        #endregion
                        #region EMPATTNDDTLS
                        case ApplicationType.EMPATTNDDTLS:
                            GetFieldValues(RptType);
                            SetFieldValuesCrystal(RptType, dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString());
                            break;
                        #endregion
                        #region EMPADDDED
                        case ApplicationType.EMPADDDED:
                            GetFieldValues(RptType);
                            SetFieldValuesCrystal(RptType, dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString());
                            break;
                        #endregion
                        #region ETR
                        case ApplicationType.ETR:
                            GetFieldValues(RptType);
                            SetFieldValuesCrystal(RptType, dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString());
                            break;
                        #endregion
                        #region ETF
                        case ApplicationType.ETF:
                            GetFieldValues(RptType);
                            SetFieldValuesCrystal(RptType, dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString());
                            break;
                        #endregion
                    }
                    break;
                #endregion

                #region RDLC Calling
                case ReportType.RDLCReport:
                    switch (RptType)
                    {
                        case ApplicationType.PAYRL:                // just sample 
                            GetFieldValues(RptType);
                            SetFieldValuesRDLC(RptType);
                            break;
                        case ApplicationType.PAYRLJ:
                        case ApplicationType.SALPYMTJ:
                            TrxRefType = Request.QueryString["TRXTYPE"].ToString();
                            SetFieldValuesRDLC(RptType);
                            break;
                    }
                    break;
                #endregion

            }
            if (FromExternal && IsPdfGenerated)
                retFlag = true;
            else
                retFlag = false;
            #endregion
            return retFlag;
        }
        #endregion

        #region PageLevel Events
        #region Page_Init
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
                //crReportViewer.Visible = false;
                if (reportDocument != null)
                {
                    reportDocument.Close();
                    reportDocument.Dispose();
                    reportDocument = null;
                }
            }
            #endregion
            reportDocument = (ReportDocument)Session[SessionStrings.CRReportData];
            if (reportDocument != null)
                CommonFunctions.SetCrystalReportDataBaseConnection(reportDocument);
            ParameterFields locParamFields = (ParameterFields)Session[SessionStrings.CRReportParam];
            GERP_OutputReport.ParameterFieldInfo = locParamFields;
            GERP_OutputReport.ReportSource = reportDocument;
        }
        #endregion
        #region Page_Load
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
                        case ActionsEnum.CANCEL:
                            //Response.Redirect(ReturnUrl);
                            if (Convert.ToString(RecPK) == string.Empty)
                                Response.Redirect(Convert.ToString(Request.Url));
                            else if (hdfRefUrl.Value != string.Empty)
                                Response.Redirect(hdfRefUrl.Value);
                            break;
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

        #region Get Field Values
        public void GetFieldValues(string controlType)
        {
            try
            {
                dsHrms = new DataSet();
                switch (controlType)
                {

                    #region REPORTCONFIG
                    case ApplicationType.REPORTCONFIG:
                        dtAppTypeDetails = GenerateReportBL.GetReportParameters(RptType, RptSubType, AppvdDate);
                        break;
                    #endregion
                    #region PAYRL
                    case ApplicationType.PAYRL:
                        if (RptSubType == 1)//HRMS Pay Slip Daily
                        {
                            BusinessObject.HRMS.Payroll.EmpPaySlipHeader ObjEmpPaySlipHeader = new BusinessObject.HRMS.Payroll.EmpPaySlipHeader();
                            string xmlPaySlip = string.Empty;
                            if (RecPK > 0)
                            {
                                ObjEmpPaySlipHeader.EmpPaySlipDetails = new List<EmpPaySlipDetails>();
                                ObjEmpPaySlipHeader.EmpPaySlipDetails.Add(new EmpPaySlipDetails { EPS_PK = RecPK.ToString() });
                            }
                            else
                            {
                                ObjEmpPaySlipHeader = Session[ViewstateStrings.EmpPaySlipHeaderSession] as BusinessObject.HRMS.Payroll.EmpPaySlipHeader;
                            }
                            xmlPaySlip = CommonFunctions.XmlSerialize<BusinessObject.HRMS.Payroll.EmpPaySlipHeader>(ObjEmpPaySlipHeader);
                            dsHrms = PayrollProcessBL.GetPaySlipMultipleReport(xmlPaySlip, "SPHRM_EMP_SALARY_SLIP_ALL_OUTPUT_RPT");
                        }
                        else if (RptSubType == 9)//HRMS Pay Slip Monthly
                        {
                            BusinessObject.HRMS.Payroll.EmpPaySlipHeader ObjEmpPaySlipHeader = new BusinessObject.HRMS.Payroll.EmpPaySlipHeader();
                            string xmlPaySlip = string.Empty;
                            if (RecPK > 0)
                            {
                                ObjEmpPaySlipHeader.EmpPaySlipDetails = new List<EmpPaySlipDetails>();
                                ObjEmpPaySlipHeader.EmpPaySlipDetails.Add(new EmpPaySlipDetails { EPS_PK = RecPK.ToString() });
                            }
                            else
                            {
                                ObjEmpPaySlipHeader = Session[ViewstateStrings.EmpPaySlipHeaderSession] as BusinessObject.HRMS.Payroll.EmpPaySlipHeader;
                            }
                            xmlPaySlip = CommonFunctions.XmlSerialize<BusinessObject.HRMS.Payroll.EmpPaySlipHeader>(ObjEmpPaySlipHeader);
                            dsHrms = PayrollProcessBL.GetPaySlipMultipleReport(xmlPaySlip, GetGlobalResourceObject("ConfigurationsRes", "HrmsFinalPaySlipSP").ToString());
                        }
                        else if (RptSubType == 2 || RptSubType == 8)
                        {
                            dsHrms = PayrollProcessBL.GetSalaryStatementRPT(CurPK);
                        }
                        else if (RptSubType == 3)
                        {
                            XmlDocument doc1 = new XmlDocument();
                            string XmlIcomeTax1 = "~/XmlFiles/Income_Tax_01.xml"; //GetLocalResourceObject("XmlIncomeTaxFile").ToString();
                            doc1.Load(Server.MapPath(XmlIcomeTax1));
                            string xmlcontents1 = doc1.InnerXml;

                            string xmlData1 = doc1.InnerXml;
                            if (xmlData1 == "<Root/>" || xmlData1 == string.Empty)
                            {
                                IncomeTax_SectionsList = new List<Income_Tax_01BO.Section>();
                            }
                            else
                            {
                                Income_Tax_01BO.Income_Tax_01 root = CommonFunctions.XmlDeserialize<Income_Tax_01BO.Income_Tax_01>(xmlData1);
                                IncomeTax_SectionsList = root.Section;
                                foreach (Income_Tax_01BO.Section section in root.Section)
                                {
                                    dsList = Income_Tax_01BL.GetTaxAmountByPk(CurPK, RecPK, root.Type, section.SectionID);
                                    foreach (DataRow row in dsList.Tables[0].Rows)
                                    {
                                        section.Items.Where(r => r.DbId == Convert.ToInt32(row["IT1_ITEM"])).ToList().ForEach(c => { c.Col3 = row["IT1_AMOUNT"].ToString(); c.Col4 = row["IT1_VALUE"].ToString(); c.IT1_PK = (row["IT1_PK"] == null ? 0 : Convert.ToInt32(row["IT1_PK"])); });
                                    }
                                    foreach (DataRow row in dsList.Tables[1].Rows)
                                    {
                                        section.Items.Where(r => r.DbId == Convert.ToInt32(row["TIT_ITEM"])).ToList().ForEach(c =>
                                        {
                                            c.TIT_VALUE1 = row["TIT_VALUE1"].ToString(); c.TIT_DESC1 = row["TIT_DESC1"].ToString();
                                        });
                                    }
                                    string TaxPayerID = dsList.Tables[1].Rows[0]["TIT_DESC1"].ToString();
                                    ViewState["TaxPayerID"] = TaxPayerID.ToString();
                                }
                            }



                            //DataSet dsITReportData = new DataSet();
                            //dsITReportData.Tables.Add(IncomeTax_SectionsList[0].Items.ToDataTable());
                            //dsITReportData.Tables.Add(IncomeTax_SectionsList[1].Items.ToDataTable());
                            //dsITReportData.Tables.Add(IncomeTax_SectionsList[2].Items.ToDataTable());

                            GetReportData();
                            break;
                        }

                        else if (RptSubType == 5)
                        {
                            dsHrms = PayrollProcessBL.GetPayrollSalaryOutRPT(CurPK);
                        }
                        else if (RptSubType == 6)
                        {
                            // dsHrms = PayrollProcessBL.GetPayrollSalaryOutRPT(CurPK);
                            BusinessObject.HRMS.Payroll.PayrollPreprocessFilter ObjPayrollPreprocessFilter = new BusinessObject.HRMS.Payroll.PayrollPreprocessFilter();
                            ObjPayrollPreprocessFilter = Session[ViewstateStrings.PayrollPreprocessFilter] as BusinessObject.HRMS.Payroll.PayrollPreprocessFilter;
                            string xmlPreprocess = CommonFunctions.XmlSerialize<BusinessObject.HRMS.Payroll.PayrollPreprocessFilter>(ObjPayrollPreprocessFilter);
                            dsHrms = PayrollProcessBL.GetPayrollPreprocess(xmlPreprocess);
                        }
                        else if (RptSubType == 7)
                        {
                            dsHrms = PayrollProcessBL.GetSalaryStatementRPT(CurPK);
                        }
                        break;
                    #endregion
                    #region HLDM
                    case ApplicationType.HLDM:
                        if (RptSubType == 0)
                        {
                            dsHrms = HolidayMasterBL.GetHolidayListRPT(CurPK);
                        }
                        break;
                    #endregion
                    #region SALPYMT
                    case ApplicationType.SALPYMT:
                        if (RptSubType == 0)
                        {
                            dsHrms = SalaryPaymentBL.GetSalaryPaymentRPT(CurPK);
                        }
                        else if (RptSubType == 1)
                        {
                            dsHrms = SalaryPaymentBL.GetEmpSalaryPaymentRPT(CurPK);
                        }
                        else if (RptSubType == 2)
                        {
                            dsHrms = SalaryPaymentBL.GetEmpSalaryPaymentRPT(CurPK);
                        }
                        break;
                    #endregion
                    #region ITC
                    case ApplicationType.ITC:
                        SectionItemsList = new List<Income_Tax_01BO.SectionItems>();
                        Income_Tax_01BO.SectionItems SectionItemsObj;
                        XmlDocument doc = new XmlDocument();
                        string XmlIcomeTax = "~/XmlFiles/Income_Tax_01.xml";  //GetLocalResourceObject("XmlIncomeTaxFile").ToString();
                        doc.Load(Server.MapPath(XmlIcomeTax));
                        string xmlcontents = doc.InnerXml;

                        string xmlData = doc.InnerXml;
                        if (xmlData == "<Root/>" || xmlData == string.Empty)
                        {
                            IncomeTax_SectionsList = new List<Income_Tax_01BO.Section>();
                        }
                        else
                        {
                            Income_Tax_01BO.Income_Tax_01 root = CommonFunctions.XmlDeserialize<Income_Tax_01BO.Income_Tax_01>(xmlData);
                            IncomeTax_SectionsList = root.Section;
                            foreach (Income_Tax_01BO.Section section in root.Section)
                            {
                                dsList = Income_Tax_01BL.GetTaxAmountByPk(CurPK, RecPK, root.Type, section.SectionID, 0);
                                foreach (DataRow row in dsList.Tables[0].Rows)
                                {
                                    section.Items.Where(r => r.DbId == Convert.ToInt32(row["IT1_ITEM"])).ToList().ForEach(c => { c.Col3 = row["IT1_AMOUNT"].ToString(); c.Col4 = row["IT1_VALUE"].ToString(); c.IT1_PK = (row["IT1_PK"] == null ? 0 : Convert.ToInt32(row["IT1_PK"])); });
                                    Income_Tax_01BO.Items itemObj = section.Items.Where(r => r.DbId == Convert.ToInt32(row["IT1_ITEM"])).Single();
                                    if (itemObj != null)
                                    {
                                        SectionItemsObj = new Income_Tax_01BO.SectionItems();
                                        SectionItemsObj.IT1_EMP_PK = RecPK;
                                        SectionItemsObj.IT1_EPS_PK = CurPK;
                                        SectionItemsObj.Sec_Col1 = section.Col1;
                                        SectionItemsObj.Sec_Col2 = section.Col2;
                                        SectionItemsObj.Sec_Col3 = section.Col3;
                                        SectionItemsObj.Sec_Col4 = section.Col4;
                                        SectionItemsObj.Sec_Col5 = section.Col5;
                                        SectionItemsObj.Sec_Col5_Visible = section.Col5_Visible;
                                        SectionItemsObj.Sec_ID = section.SectionID;
                                        SectionItemsObj.Sec_Seq = section.Seq;
                                        SectionItemsObj.Sec_Title = section.Title;
                                        SectionItemsObj.Itm_Col1 = itemObj.Col1;
                                        SectionItemsObj.Itm_Col2 = itemObj.Col2;
                                        SectionItemsObj.Itm_Col3 = itemObj.Col3;
                                        SectionItemsObj.Itm_Col4 = itemObj.Col4;
                                        SectionItemsObj.Itm_Col5 = itemObj.Col5;
                                        SectionItemsObj.Itm_Col3_Edit = itemObj.Col3_Edit;
                                        SectionItemsObj.Itm_Col4_Edit = itemObj.Col4_Edit;
                                        SectionItemsObj.Itm_Col4_Edit = itemObj.Col4_Edit;
                                        SectionItemsObj.Itm_PK = itemObj.IT1_PK;
                                        SectionItemsObj.Itm_DbId = itemObj.DbId;
                                        SectionItemsObj.Itm_Seq = itemObj.Seq;
                                        SectionItemsObj.IT1_SEQ_NO = Convert.ToString(row["IT1_SEQ_NO"]);
                                        SectionItemsList.Add(SectionItemsObj);
                                    }
                                }
                            }
                            dtSectionItems = SectionItemsList.ToDataTable();
                            dsHrms.Tables.Add(dtSectionItems);
                            dsHrms.Tables[0].TableName = "IncomeTax";
                            //dsHrms.Tables.Add(SectionItemsList.ToDataTable());
                        }
                        break;
                    #endregion
                    #region EMPATTND
                    case ApplicationType.EMPATTND:
                        if (RptSubType == 0)
                        {
                            dsHrms = BusinessLogic.HRMS.Payroll.AttendanceBL.GetAttendanceReport(CurPK);
                        }
                        else
                            dsHrms = BusinessLogic.HRMS.Payroll.AttendanceBL.GetAttendanceReportDetails(CurPK);
                        break;
                    #endregion
                    //#region EMPATTNDDTLS
                    //case ApplicationType.EMPATTNDDTLS:
                    //    dsHrms = BusinessLogic.HRMS.Payroll.AttendanceBL.GetAttendanceReportDetails(CurPK);
                    //    break;
                    //#endregion
                    #region EMPADDDED
                    case ApplicationType.EMPADDDED:
                        dsHrms = BusinessLogic.HRMS.Payroll.AttendanceBL.GetAdditionDeductionReport(CurPK);
                        break;
                    #endregion
                    #region ETR
                    case ApplicationType.ETR:
                        dsHrms = BusinessLogic.HRMS.Employee.EmployeeTrainingBL.GetEmployeeTrainingReport(CurPK);
                        break;
                    #endregion
                    #region ETF
                    case ApplicationType.ETF:
                        dsHrms = BusinessLogic.HRMS.Employee.EmployeeTransferBL.GetEmployeeTransferReport(CurPK);
                        break;
                    #endregion
                    #region FSMT
                    case ApplicationType.FSMT:
                        if (RptSubType == 0)//HRMS Pay Slip Monthly
                        {
                            BusinessObject.HRMS.Payroll.EmpPaySlipHeader ObjEmpPaySlipHeader = new BusinessObject.HRMS.Payroll.EmpPaySlipHeader();
                            string xmlPaySlip = string.Empty;
                            if (RecPK > 0)
                            {
                                ObjEmpPaySlipHeader.EmpPaySlipDetails = new List<EmpPaySlipDetails>();
                                ObjEmpPaySlipHeader.EmpPaySlipDetails.Add(new EmpPaySlipDetails { EPS_PK = RecPK.ToString() });
                            }
                            else
                            {
                                ObjEmpPaySlipHeader = Session[ViewstateStrings.EmpPaySlipHeaderSession] as BusinessObject.HRMS.Payroll.EmpPaySlipHeader;
                            }
                            xmlPaySlip = CommonFunctions.XmlSerialize<BusinessObject.HRMS.Payroll.EmpPaySlipHeader>(ObjEmpPaySlipHeader);
                            dsHrms = FullandFinalSettlementBL.GetPaySlipMultipleReport(xmlPaySlip, GetGlobalResourceObject("ConfigurationsRes", "HrmsFinalPaySlipSP").ToString());
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

        #region Set Field Values RDLC
        private void SetFieldValuesRDLC(string appType)
        {
            try
            {
                //ReportViewer rvViewReport = null;
                //rvViewReport = rvViewReport;
                string _printerMode = string.Empty;
                _printerMode = Request.QueryString["PRINTERMODE"];
                divReportViewer.Visible = true;
                rvViewReport.Visible = true;
                divNodata.Visible = false;

                locRpt = null;
                rvViewReport.LocalReport.DataSources.Clear();
                locRpt = rvViewReport.LocalReport;
                rvViewReport.LocalReport.DataSources.Clear();
                locRpt.EnableExternalImages = true;
                locRpt.EnableHyperlinks = true;
                rvViewReport.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));   // Set access permission for Ref. DLL(NumberToWordConverter)

                #region Get Company Details
                if (dsHrms != null && dsHrms.Tables.Count > 0)
                {
                    if (dsHrms.Tables[0].Rows.Count > 0)
                    {
                        if (dsHrms.Tables[0].Columns.Contains(GetGlobalResourceObject("DataFieldRes", "HRMSRptCompany").ToString()))
                        {
                            CompanyPK = Convert.ToInt32(dsHrms.Tables[0].Rows[0][GetGlobalResourceObject("DataFieldRes", "HRMSRptCompany").ToString()].ToString());
                        }
                    }
                }
                #endregion

                cm = new CommonService();
                AppTypeDetailsList = new List<SPADM_APP_SUB_TYPE_DATA_GET_Result>();
                switch (appType)
                {
                    case ApplicationType.PAYRL:
                        if (dsHrms != null)
                        {
                            if (RptSubType == 6)
                            {
                                SetReportParameters(locRpt);
                                dtHeader = dsHrms.Tables[0];
                                dtDetails1 = dsHrms.Tables[1];
                                rdsHeader = new ReportDataSource("DataSet1", dtHeader);
                                rdsDetails1 = new ReportDataSource("DataSet2", dtDetails1);
                                rvViewReport.LocalReport.DataSources.Add(rdsHeader);
                                rvViewReport.LocalReport.DataSources.Add(rdsDetails1);
                            }
                            else if (RptSubType == 7)
                            {
                                SetReportParameters(locRpt);
                                dtHeader = dsHrms.Tables[0];
                                dtDetails1 = dsHrms.Tables[1];
                                rdsHeader = new ReportDataSource("DataSet1", dtHeader);
                                rdsDetails1 = new ReportDataSource("DataSet2", dtDetails1);
                                rvViewReport.LocalReport.DataSources.Add(rdsHeader);
                                rvViewReport.LocalReport.DataSources.Add(rdsDetails1);
                            }
                            // rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(1));
                        }
                        else
                        {
                            rvViewReport.Visible = false;
                            divReportViewer.Visible = false;
                            rvViewReport.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    case ApplicationType.PAYRLJ:
                    case ApplicationType.SALPYMTJ:
                        ReportDataSource dsJV;
                        currentEntity = new ERPEntities();
                        List<SPFIN_TRX_VOUCHER_RPT_Result> lstData = currentEntity.SPFIN_TRX_VOUCHER_RPT(TrxRefType, RecPK, VoucherVersion).ToList();
                        dsJV = new ReportDataSource("JVHeader", lstData);
                        if (lstData != null & lstData.Count > 0)
                        {
                            CompanyPK = lstData[0].FTH_COMPANY;
                        }
                        //rvViewReport.LocalReport.SetBasePermissionsForSandboxAppDomain(new PermissionSet(PermissionState.Unrestricted));
                        rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                        if (dsJV != null)
                        {
                            AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            rvViewReport.LocalReport.DataSources.Add(dsJV);
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvViewReport.Visible = false;
                            divNodata.Visible = true;
                        }

                        break;

                }
                rvViewReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                rvViewReport.LocalReport.Refresh();
                if (IsExcelPrint == 1)
                {
                    SaveExcel(locRpt);
                }
                else
                {
                    SavePDF(locRpt);
                    if (File.Exists(attachmentFilePath))
                    {
                        Response.ClearContent();
                        Response.ContentType = "application/pdf";

                        string redirectUrl = "~/Reports/TempPDF/" + attachmentFileName;
                        Response.Redirect(redirectUrl);
                        // Response.Redirect("~/Reports/TempPDF/" + attachmentFileName);
                        //Response.Redirect("../Reports/Rdlc/TempPDF/" + attachmentFileName);
                        Response.Flush();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Set Field Values Crystal Report
        public void SetFieldValuesCrystal(string appType, string reportName)
        {
            try
            {
                CrystalDecisions.Web.CrystalReportViewer currentCrptViewer = null;
                reportDocument = new ReportDocument();
                reportDocument.Load(Server.MapPath("~/Reports/CrystalReport/" + reportName));
                reportDocument.Refresh();
                ERP.Utilities.CommonFunctions.SetCrystalReportDataBaseConnection(reportDocument);
                if (!FromExternal)
                {
                    currentCrptViewer = GERP_OutputReport;
                }
                else
                {
                    currentCrptViewer = new CrystalDecisions.Web.CrystalReportViewer();
                }
                switch (appType)
                {

                    #region PAYRL
                    case ApplicationType.PAYRL:

                        if (dsHrms != null && (RptSubType == 1))
                        {
                            dsHrms.Tables[0].TableName = "dtPaySlip";
                            reportDocument.SetDataSource(dsHrms); // Added report data as dataset.
                        }
                        else if (dsHrms != null && (RptSubType == 2 || RptSubType == 8))
                        {
                            dsHrms.Tables[0].TableName = "dtSalaryStatement_Hdr";
                            dsHrms.Tables[1].TableName = "dtSalaryStatement_Dtls";
                            reportDocument.SetDataSource(dsHrms); // Added report data as dataset.
                        }
                        else if (dsHrms != null && RptSubType == 3)
                        {
                            dsHrms.Tables[0].TableName = "dtItDeduction";
                            reportDocument.SetDataSource(dsHrms);
                        }
                        else if (dsHrms != null && RptSubType == 5)
                        {
                            dsHrms.Tables[0].TableName = "dtSalaryStatement_Hdr";
                            dsHrms.Tables[1].TableName = "dtPayrollSalary";
                            reportDocument.SetDataSource(dsHrms); // Added report data as dataset.
                        }
                        if (dsHrms != null && (RptSubType == 9))
                        {
                            dsHrms.Tables[0].TableName = GetGlobalResourceObject("ConfigurationsRes", "HrmsMonthlyPaySlipDataTable").ToString();
                            reportDocument.SetDataSource(dsHrms); // Added report data as dataset.
                        }
                        break;
                    #endregion
                    #region HLDM
                    case ApplicationType.HLDM:
                        if (dsHrms != null && RptSubType == 0)
                        {
                            dsHrms.Tables[0].TableName = "dtHolidayMaster";
                            reportDocument.SetDataSource(dsHrms); // Added report data as dataset.
                        }
                        break;
                    #endregion
                    #region SALPYMT
                    case ApplicationType.SALPYMT:
                        if (dsHrms != null && RptSubType == 0)
                        {
                            dsHrms.Tables[0].TableName = "dtSalaryPayment";
                            reportDocument.SetDataSource(dsHrms); // Added report data as dataset.
                        }
                        else if (dsHrms != null && RptSubType == 1)
                        {
                            dsHrms.Tables[0].TableName = "dtEmpSalaryPayment";
                            reportDocument.SetDataSource(dsHrms); // Added report data as dataset.
                        }
                        else if (dsHrms != null && RptSubType == 2)
                        {
                            // JTE Only
                            string EmployerId = string.Empty;
                            if (dsHrms.Tables[0] != null && dsHrms.Tables[0].Rows.Count > 0)
                                EmployerId = (dsHrms.Tables[0].Rows[0][GetLocalResourceObject("CMP_TAX_NO").ToString()]).ToString();
                            Regex re = new Regex("[;\\/:*?\"<>|&']");
                            string CurDateTime = re.Replace(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), string.Empty);
                            reportName = EmployerId + CurDateTime.Replace(" ", string.Empty);
                            dsHrms.Tables[0].TableName = "dtEmpSalaryPayment";
                            reportDocument.SetDataSource(dsHrms); // Added report data as dataset.
                        }

                        break;
                    #endregion
                    #region ITC
                    case ApplicationType.ITC:
                        if (dsHrms != null && RptSubType == 0)
                        {
                            dsHrms.Tables[0].TableName = "dtSectionItems";
                            reportDocument.SetDataSource(dsHrms); // Added report data as dataset.
                        }
                        break;
                    #endregion
                    #region EMPATTND
                    case ApplicationType.EMPATTND:
                        if (dsHrms != null && RptSubType == 0)
                        {
                            dsHrms.Tables[0].TableName = "dtAttendanceItems";
                            reportDocument.SetDataSource(dsHrms); // Added report data as dataset.
                        }

                        else if (dsHrms != null && RptSubType == 1)
                        {
                            dsHrms.Tables[0].TableName = "dtAttendanceDetails_Hdr";
                            dsHrms.Tables[1].TableName = "dtAttendanceDetails_Dtl";
                            reportDocument.SetDataSource(dsHrms);
                        }


                        break;
                    #endregion


                    #region EMPADDDED
                    case ApplicationType.EMPADDDED:
                        if (dsHrms != null && RptSubType == 0)
                        {
                            dsHrms.Tables[0].TableName = "dtAddDed_Hdr";
                            dsHrms.Tables[1].TableName = "dtAddDed_Dtl";
                            reportDocument.SetDataSource(dsHrms);
                        }
                        break;
                    #endregion
                    #region ETR
                    case ApplicationType.ETR:
                        if (dsHrms != null && RptSubType == 0)
                        {
                            dsHrms.Tables[0].TableName = "dtEmpTraining_Dtl";
                            dsHrms.Tables[1].TableName = "dtEmpTraining_Hdr";
                            reportDocument.SetDataSource(dsHrms);
                        }
                        break;
                    #endregion
                    #region ETF
                    case ApplicationType.ETF:
                        if (dsHrms != null && RptSubType == 0)
                        {
                            dsHrms.Tables[0].TableName = "dtEmpTransfer";
                            reportDocument.SetDataSource(dsHrms);
                        }
                        break;
                    #endregion

                    #region FSMT
                    case ApplicationType.FSMT:
                        if (dsHrms != null && (RptSubType == 0))
                        {
                            dsHrms.Tables[0].TableName = GetGlobalResourceObject("ConfigurationsRes", "HrmsMonthlyPaySlipDataTable").ToString();
                            reportDocument.SetDataSource(dsHrms); // Added report data as dataset.
                        }
                        break;
                        #endregion

                }

                #region Get Company Details
                if (dsHrms != null)
                {
                    if (dsHrms.Tables[0].Rows.Count > 0)
                    {
                        if (dsHrms.Tables[0].Columns.Contains(GetGlobalResourceObject("DataFieldRes", "HRMSRptCompany").ToString()))
                        {
                            CompanyPK = Convert.ToInt32(dsHrms.Tables[0].Rows[0][GetGlobalResourceObject("DataFieldRes", "HRMSRptCompany").ToString()].ToString());
                        }
                    }
                }
                #endregion

                currentCrptViewer.ReportSource = reportDocument;
                SetCrystalreportCommonParameters(reportDocument, RecPK.ToString());
                #region Not From FromExternal
                if (!FromExternal)
                {
                    hdfShowCrReportDiv.Value = "1";
                    Session[SessionStrings.CRReportData] = reportDocument;
                    // Excel Export
                    #region Excel Export
                    if (IsExcelPrint == 1)
                    {
                        reportDocument.ExportToHttpResponse(ExportFormatType.Excel, Page.Response, false, reportName);
                    }
                    #endregion
                    // CSV/SIF
                    #region SIF Sxport
                    if (IsExcelPrint == 2)
                    {
                        string filePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + GetLocalResourceObject("UploadSIFDirectory").ToString();
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            ClearFile(filePath);
                            reportDocument.ExportToDisk(ExportFormatType.Text, filePath + reportName + ".sif");
                            string text = File.ReadAllText(filePath + reportName + ".sif");
                            text = text.Replace("", ""); // to Remove ankle(♀) character
                            File.WriteAllText(filePath + reportName + ".sif", text);
                            Download_File(filePath + reportName + ".sif");
                        }
                    }
                    #endregion
                    #region Pdf Export
                    else
                    {
                        reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Page.Response, false, reportName);
                    }
                    #region
                    #endregion
                }
                    #endregion

                #endregion
                #region From External
                else
                {
                    reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, ExternalPDFURL + ExternalPDFName);
                    IsPdfGenerated = true;
                }
                #endregion
            }

            catch (Exception ex)
            {
                IsPdfGenerated = false;
                throw ex;
            }
            finally
            {
                reportDocument.Close();
                reportDocument.Dispose();
                reportDocument = null;
                Session[SessionStrings.CRReportData] = null;
                GC.Collect();
            }
        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath.Replace(" ", string.Empty)));
            Response.WriteFile(FilePath);
            Response.End();
        }
        #endregion

        #region Helper Methods
        #region SavePDF
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
                //savePath = Server.MapPath("~/") + Resources.PageURL.OfflineTestDocs;    
                // savePath = Server.MapPath("~/");                
                //if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"] != string.Empty)
                //{
                //    savePath = System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString();
                //}
                //  savePath = savePath + Resources.PageURL.OfflineTestDocs;

                savePath = Server.MapPath("~/") + Resources.PageURL.OfflineTestDocs;
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + savePath +  Resources.Messages.Information + "');", true);
                //   ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);

                attachmentFilePath = string.Empty;
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
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

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Convert to & save pdf
        /// </summary>


        #endregion

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

        #region  ConfigurationSettings
        /// <summary>
        /// ConfigurationSettings for rate,currency,qty,weight
        /// </summary>
        /// <returns></returns>
        private DataTable ConfigurationSettings()
        {
            //Initialze the current logged in user to the currentUser variable

            int SBUPK = 1;
            if (!FromExternal)
            {
                currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;
                SBUPK = currentUser.CurrentSBUPK;
            }
            else
                SBUPK = SbuID;
            DataTable dt = CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, SBUPK);
            return dt;
        }
        #endregion

        #region ClearCrystalReport
        /// <summary>
        /// Cleaer Crystal Report
        /// </summary>
        private void ClearCrystalReport()
        {
            hdfShowCrReportDiv.Value = "0";
            GERP_OutputReport.ReportSource = null;
            GERP_OutputReport.RefreshReport();
        }
        #endregion

        #region SetParamValue
        private ParameterField SetParamValue(string paramName, string paramValue)
        {
            ParameterField paramField = new ParameterField();
            ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();
            paramField.Name = paramName;
            paramDiscreteValue.Value = paramValue;
            paramField.CurrentValues.Add(paramDiscreteValue);
            return paramField;
        }
        #endregion

        #region SetCrystalreportCommonParameters
        private void SetCrystalreportCommonParameters(ReportDocument rptDocument, string rptPK)
        {

            string signaturePath = string.Empty;

            if (!FromExternal)
                currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;

            #region Extra Parameter based on query string
            //if (RptType == ApplicationType.TRACE && RptSubType == 2)
            //{
            //    rptDocument.SetParameterValue("BatchNo", BatchNo.ToString());
            //}
            #endregion

            DataSet dsParamSettings = new DataSet();
            //------------------------29-11-2016----------------------------------
            ParameterFieldDefinitions crParameterdef;
            crParameterdef = reportDocument.DataDefinition.ParameterFields;
            //--------------------------------------------------------------------

            string footer;
            string rptName = string.Empty;
            footer = string.Empty;
            try
            {
                if (dtAppTypeDetails != null && dtAppTypeDetails.Rows.Count > 0)
                {
                    if (dtAppTypeDetails.Rows[0]["AST_RPT_SETTINGS"] != null)
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(dtAppTypeDetails.Rows[0]["AST_RPT_SETTINGS"])));
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    int curdigit = 2;
                    int NoDigit = 2;
                    int ExchRate = 2;
                    int RateDecimal = 2;
                    int RateDecimalPP = 2;
                    int WeightDigit = 2;
                    DataTable dt = ConfigurationSettings();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        curdigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                        NoDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());
                        ExchRate = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                        RateDecimal = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                        RateDecimalPP = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
                        WeightDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "WeightDecimalDigit")["ACF_VALUE"].ToString());

                    }
                    rptDocument.SetParameterValue("CurrencyDigits", curdigit.ToString());
                    rptDocument.SetParameterValue("NumberDigits", NoDigit.ToString());
                    rptDocument.SetParameterValue("ExchangeRate", ExchRate.ToString());
                    rptDocument.SetParameterValue("RateDigits", RateDecimal.ToString());
                    rptDocument.SetParameterValue("WeightDigits", WeightDigit.ToString());
                    rptDocument.SetParameterValue("DateFormat", Resources.Constants.ReportDateFormat.ToString());
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

                #region Signature

                if (RptType == ApplicationType.PAYRL && Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "HrmsPayslipMonthly")) == 2)
                {
                    signaturePath = string.Empty;
                    if (dsHrms != null)
                    {
                        if (dsHrms.Tables.Count > 0)
                        {
                            if (dsHrms.Tables[0].Columns.Contains(Resources.DataFieldRes.EPHApprovedBySign.ToString()))
                            {
                                string signature = dsHrms.Tables[0].Rows[0][Resources.DataFieldRes.EPHApprovedBySign].ToString();

                                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                {
                                    if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature))
                                        signaturePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + signature;
                                }
                            }
                        }


                    }
                }

                #endregion

                //--------------------------29-11-2016----------------------------------------------------------
                foreach (ParameterFieldDefinition def in crParameterdef)
                {
                    if (def.Name.Equals("ApprovedBySign"))
                        rptDocument.SetParameterValue("ApprovedBySign", signaturePath);

                    if (def.Name.Equals("HLURL"))
                        rptDocument.SetParameterValue("HLURL", HLURL);
                    if (def.Name.Equals("CurrentDeptPK"))
                    {
                        int CurrentDeptPK = 0;
                        if (!FromExternal)
                            CurrentDeptPK = currentUser.CurrentDeptPK;
                        else
                            CurrentDeptPK = DeptPK;
                        rptDocument.SetParameterValue("CurrentDeptPK", CurrentDeptPK);
                    }
                }
                //---------------------------------------------------------------------------------------

                //Comment Start  Modofied on 26-04-2017 Sruthy
                //To get Logo from Company Master
                //rptDocument.SetParameterValue("Logo", Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoPDF"]));
                GetCompanyDetails(CompanyPK);
                rptDocument.SetParameterValue("Logo", LogoPath);
                //Comment End
                string empName = string.Empty;
                if (!FromExternal)
                    footer = string.Format(GetLocalResourceObject("FooterText").ToString(), currentUser.EmpName, DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));
                else
                    footer = string.Format("Printed by {0}  On {1}", EmpName, DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));

                rptDocument.SetParameterValue("FooterText", footer);

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SetReportParameters
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
                dsParamSettings = new DataSet();

                locRpt.ReportPath = string.Empty;
                //foreach (DataTable  sa in dtAppTypeDetails)
                //{
                if (dtAppTypeDetails != null && dtAppTypeDetails.Rows.Count > 0)
                {
                    rptName = dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString();//
                    locRpt.ReportPath = Server.MapPath("~\\Reports\\Rdlc\\" + rptName);
                    locRpt.EnableHyperlinks = true;
                    if (RptType != ApplicationType.GST)
                    {
                        parameters = new ReportParameter("QMSRef", dtAppTypeDetails.Rows[0]["AST_QMS_REF"].ToString());
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("HideQMSRef", dtAppTypeDetails.Rows[0]["AST_QMS_VISIBILITY"].ToString());
                        locRpt.SetParameters(parameters);
                    }
                    if (dtAppTypeDetails.Rows[0]["AST_RPT_SETTINGS"] != null)
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(dtAppTypeDetails.Rows[0]["AST_RPT_SETTINGS"])));
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    //string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
                    string currencyformat = "#" + currencysep + "#0.";
                    string NoFormat = "#" + currencysep + "#0.";
                    string WeightFormat = "#" + currencysep + "#0.";
                    string ExchRateDigt = "#" + currencysep + "#0.";
                    string RateDeciDigt = "#" + currencysep + "#0.";
                    string QtyforPurchase = "#" + currencysep + "#0.";
                    string currencydecimals = "";
                    string Weightdecimal = string.Empty;//Weight Decimal
                    string Nodecimal = string.Empty;
                    string ExchRateDigit = string.Empty;
                    string RateDecimalDigit = string.Empty;
                    string QtyDecforPuchase = string.Empty;
                    string NumberDecimalDigitsBin = "#" + currencysep + "#0.";
                    //for compound and dispersion and topup
                    string CompoundingDecDigit = "#" + currencysep + "#0.";
                    string NumberDecimalCompoundingDigit = string.Empty;
                    DataTable dt = ConfigurationSettings();
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
                        int QtyDecimalPurchase = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "NumberDecimalDigitP2P")["ACF_VALUE"].ToString());
                        for (int i = 0; i < QtyDecimalPurchase; i++)
                        {
                            QtyDecforPuchase += "0";
                        }

                    }
                    else
                    {
                        currencydecimals = "00";
                        Nodecimal = "00";
                    }
                    currencyformat = currencyformat + currencydecimals;
                    NoFormat = NoFormat + Nodecimal;
                    WeightFormat = WeightFormat + Weightdecimal;
                    ExchRateDigt = ExchRateDigt + ExchRateDigit;
                    RateDeciDigt = RateDeciDigt + RateDecimalDigit;
                    QtyforPurchase = QtyforPurchase + QtyDecforPuchase;
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
                    if (dtAppTypeDetails.Rows[0]["AST_CODE"].ToString() == "TRACE" && (dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString() == "RawMaterialQC.rdlc" || dtAppTypeDetails.Rows[0]["AST_OP_FILE1"].ToString() == "ProductQCReport.rdlc"))
                    {
                        string virtulaDir = string.Empty;
                        if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"] != string.Empty)
                            virtulaDir = System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"];
                        string FilePath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/" + virtulaDir + "Upload/";
                        parameters = new ReportParameter("Path", FilePath);
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

                //Comment Start  Modofied on 26-04-2017 Sruthy
                //To get Logo from Company Master
                //parameters = new ReportParameter("Logo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoPDF"]));
                GetCompanyDetails(CompanyPK);
                parameters = new ReportParameter("Logo", LogoPath);
                //Comment End

                //string str=Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]);
                locRpt.SetParameters(parameters);

                footer = "Printed by " + currentUser.EmpName + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                parameters = new ReportParameter("FooterText", footer);
                locRpt.SetParameters(parameters);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetCompanyDetails
        /// <summary>
        /// To Get Company Details
        /// </summary>
        private ReportDataSource GetCompanyDetails(int? CmpnyPk = null)
        {
            ReportDataSource CompanyDtls = null;
            //Commented on 26-04-2017 Sruthy
            #region old
            //currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;
            //DataTable dtCompany = CommonBL.GetCompanyDetails(CmpnyPk, Convert.ToInt32(DbActiveStatus.HASPK), null);
            ////List<SPADM_COMPANY_MST_GET_KV_Result> CompanyList = currentEntity.SPADM_COMPANY_MST_GET_KV(CmpnyPk, Convert.ToByte(DbActiveStatus.HASPK), null, null).ToList();

            //if (dtCompany != null && dtCompany.Rows.Count > 0)
            //{
            //    if (Convert.ToString(dtCompany.Rows[0]["CMP_LOGO"]) != string.Empty)
            //    {
            //        bool fileExists = false;
            //        string LogoPath = string.Empty;
            //        // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN); 
            //        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
            //        {
            //            if (File.Exists(Server.MapPath(Resources.Controls.LogoPath) + dtCompany.Rows[0]["CMP_LOGO"]))
            //            {
            //                LogoPath = "file:///" + Server.MapPath(Resources.Controls.LogoPath) + dtCompany.Rows[0]["CMP_LOGO"];
            //                fileExists = true;
            //            }
            //        }
            //        else
            //        {
            //            if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + dtCompany.Rows[0]["CMP_LOGO"]))
            //            {
            //                LogoPath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + dtCompany.Rows[0]["CMP_LOGO"];
            //                fileExists = true;
            //            }
            //        }
            //        if (!fileExists)
            //        {
            //            LogoPath = string.Empty;
            //        }
            //        dtCompany.Rows[0]["CMP_LOGO"] = LogoPath;
            //    }
            //}
            //CompanyDtls = new ReportDataSource("CompanyDtls", dtCompany);
            //return CompanyDtls;
            #endregion

            //Modofied on 26-04-2017 Sruthy
            //To get Logo from Company Master
            #region new
            currentEntity = new ERPEntities();
            List<SPADM_COMPANY_MST_GET_KV_Result> CompanyList = currentEntity.SPADM_COMPANY_MST_GET_KV(CmpnyPk, Convert.ToByte(DbActiveStatus.HASPK), null, null,null).ToList();

            if (CompanyList != null && CompanyList.Count > 0)
            {
                if (Convert.ToString(CompanyList[0].CMP_LOGO) != string.Empty)
                {
                    bool fileExists = false;
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
            #endregion
        }
        #endregion

        #region IT Report Data
        //Added by Sruthy H.on 24-10-2016 for Incometax report
        public void GetReportData()
        {
            Income_Tax_01BO.ReportDetails ReportDetails = new Income_Tax_01BO.ReportDetails();

            if (IncomeTax_SectionsList[0].Items.Count > 0 && IncomeTax_SectionsList[1].Items.Count > 0 && IncomeTax_SectionsList[2].Items.Count > 0)
            {
                #region AMOUNT

                //Section Amount-A
                ReportDetails.Amt_Acol1 = IncomeTax_SectionsList[0].Items[0].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[0].Col3); //Salry wages..
                ReportDetails.Amt_Acol2 = IncomeTax_SectionsList[0].Items[1].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[1].Col3); //Less exempted..
                ReportDetails.Amt_Acol3 = IncomeTax_SectionsList[0].Items[2].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[2].Col3); //Balance(1-2)
                ReportDetails.Amt_Acol4 = IncomeTax_SectionsList[0].Items[3].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[3].Col3); //Less expense..
                ReportDetails.Amt_Acol5 = IncomeTax_SectionsList[0].Items[4].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[4].Col3); //Balance(3-4)
                ReportDetails.Amt_Acol6 = IncomeTax_SectionsList[0].Items[5].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[5].Col3); //Less allowances..
                ReportDetails.Amt_Acol7 = IncomeTax_SectionsList[0].Items[6].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[6].Col3); //Balance(5-6)
                ReportDetails.Amt_Acol8 = IncomeTax_SectionsList[0].Items[7].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[7].Col3); //Less donation...
                ReportDetails.Amt_Acol9 = IncomeTax_SectionsList[0].Items[8].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[8].Col3); //Balance(7-8)
                ReportDetails.Amt_Acol10 = IncomeTax_SectionsList[0].Items[9].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[9].Col3); //Less other..
                ReportDetails.Amt_Acol11 = IncomeTax_SectionsList[0].Items[10].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[10].Col3); //Net income(9-10)
                ReportDetails.Amt_Acol12 = IncomeTax_SectionsList[0].Items[11].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[11].Col3); //Tax computed..
                ReportDetails.Amt_Acol13 = IncomeTax_SectionsList[0].Items[12].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[12].Col3); //Less exemption..
                ReportDetails.Amt_Acol14 = IncomeTax_SectionsList[0].Items[13].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[13].Col3); //Tax payable...
                ReportDetails.Amt_Acol15 = IncomeTax_SectionsList[0].Items[14].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[14].Col3); //Less witholding..
                ReportDetails.Amt_Acol16 = IncomeTax_SectionsList[0].Items[15].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[15].Col3); //Total tax
                ReportDetails.Amt_Acol16_1 = IncomeTax_SectionsList[0].Items[16].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[16].Col3); //Total tax - payable
                ReportDetails.Amt_Acol16_2 = IncomeTax_SectionsList[0].Items[17].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[17].Col3); //Total tax - overpaid
                ReportDetails.Amt_Acol17 = IncomeTax_SectionsList[0].Items[18].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[18].Col3); //Add additional..
                ReportDetails.Amt_Acol18 = IncomeTax_SectionsList[0].Items[19].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[19].Col3); //Less tax overpaid
                ReportDetails.Amt_Acol19 = IncomeTax_SectionsList[0].Items[20].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[20].Col3); //Less tax paid..
                ReportDetails.Amt_Acol20 = IncomeTax_SectionsList[0].Items[21].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[21].Col3); //Tax
                ReportDetails.Amt_Acol20_1 = IncomeTax_SectionsList[0].Items[22].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[22].Col3); //Tax - Payable
                ReportDetails.Amt_Acol20_2 = IncomeTax_SectionsList[0].Items[23].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[23].Col3); //Tax - Overpaid
                ReportDetails.Amt_Acol21 = IncomeTax_SectionsList[0].Items[24].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[24].Col3); //Add surcharge..
                ReportDetails.Amt_Acol22 = IncomeTax_SectionsList[0].Items[25].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[25].Col3); //Total Tax
                ReportDetails.Amt_Acol22_1 = IncomeTax_SectionsList[0].Items[26].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[26].Col3); //Total Tax - Payable
                ReportDetails.Amt_Acol22_2 = IncomeTax_SectionsList[0].Items[27].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[27].Col3); //Total Tax - Overpaid

                //Section Amount-B   
                ReportDetails.Amt_Bcol1 = IncomeTax_SectionsList[1].Items[0].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[0].Col3);   //PF Contribution..
                ReportDetails.Amt_Bcol2 = IncomeTax_SectionsList[1].Items[1].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[1].Col3);   //Govt. pension..
                ReportDetails.Amt_Bcol3 = IncomeTax_SectionsList[1].Items[2].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[2].Col3);   //Private teacher..
                ReportDetails.Amt_Bcol4 = IncomeTax_SectionsList[1].Items[3].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[3].Col3);   //National saving..
                ReportDetails.Amt_Bcol5 = IncomeTax_SectionsList[1].Items[4].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[4].Col3);   //Income exemption..
                ReportDetails.Amt_Bcol5_1 = IncomeTax_SectionsList[1].Items[5].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[5].Col3);   //Disable taxpayer under 65..
                ReportDetails.Amt_Bcol5_2 = IncomeTax_SectionsList[1].Items[6].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[6].Col3);   //Taxpayer aged 65 and above..
                ReportDetails.Amt_Bcol6 = IncomeTax_SectionsList[1].Items[7].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[7].Col3);   //Serverence pay..
                ReportDetails.Amt_Bcol7 = IncomeTax_SectionsList[1].Items[8].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[8].Col3);   //Total(1-5)

                //Section Amount-C   
                ReportDetails.Amt_Ccol1 = IncomeTax_SectionsList[2].Items[0].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[0].Col3);   //Taxpayer
                ReportDetails.Amt_Ccol2 = IncomeTax_SectionsList[2].Items[1].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[1].Col3);   //Spouse
                ReportDetails.Amt_Ccol3 = IncomeTax_SectionsList[2].Items[2].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[2].Col3);   ///Child (15,000)...
                ReportDetails.Amt_Ccol3_1 = IncomeTax_SectionsList[2].Items[3].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[3].Col3);   //Fill persoNal ID
                ReportDetails.Amt_Ccol3_2 = IncomeTax_SectionsList[2].Items[4].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[4].Col3);   //Child (17,000)...
                ReportDetails.Amt_Ccol3_3 = IncomeTax_SectionsList[2].Items[5].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[5].Col3);   //Fill Personal ID
                ReportDetails.Amt_Ccol4 = IncomeTax_SectionsList[2].Items[6].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[6].Col3);   //Parental care
                ReportDetails.Amt_Ccol4_1 = IncomeTax_SectionsList[2].Items[7].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[7].Col3);   //Father of taxpayer
                ReportDetails.Amt_Ccol4_2 = IncomeTax_SectionsList[2].Items[8].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[8].Col3);   //Mother of taxpayer
                ReportDetails.Amt_Ccol4_3 = IncomeTax_SectionsList[2].Items[9].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[9].Col3);   //Father of spouse...
                ReportDetails.Amt_Ccol4_4 = IncomeTax_SectionsList[2].Items[10].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[10].Col3);   //Mother of spouse...
                ReportDetails.Amt_Ccol5 = IncomeTax_SectionsList[2].Items[11].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[11].Col3);   //Disabled/Incompetant...
                ReportDetails.Amt_Ccol6 = IncomeTax_SectionsList[2].Items[12].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[12].Col3);   //Health insurance...
                ReportDetails.Amt_Ccol6_1 = IncomeTax_SectionsList[2].Items[13].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[13].Col3);   //Father of taxpayer
                ReportDetails.Amt_Ccol6_2 = IncomeTax_SectionsList[2].Items[14].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[14].Col3);   //Mother of taxpayer
                ReportDetails.Amt_Ccol6_3 = IncomeTax_SectionsList[2].Items[15].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[15].Col3);   //Father of spouse
                ReportDetails.Amt_Ccol6_4 = IncomeTax_SectionsList[2].Items[16].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[16].Col3);   //Mother of spouse
                ReportDetails.Amt_Ccol7 = IncomeTax_SectionsList[2].Items[17].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[17].Col3);   //Life insurance...
                ReportDetails.Amt_Ccol7_1 = IncomeTax_SectionsList[2].Items[18].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[18].Col3);   //Pension insurance...
                ReportDetails.Amt_Ccol8 = IncomeTax_SectionsList[2].Items[19].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[19].Col3);   //PF Contribution...
                ReportDetails.Amt_Ccol9 = IncomeTax_SectionsList[2].Items[20].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[20].Col3);   //Retirement mutual...
                ReportDetails.Amt_Ccol10 = IncomeTax_SectionsList[2].Items[21].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[21].Col3);   //Long term equity...
                ReportDetails.Amt_Ccol11 = IncomeTax_SectionsList[2].Items[22].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[22].Col3);   //Interest paid on loan...
                ReportDetails.Amt_Ccol12 = IncomeTax_SectionsList[2].Items[23].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[23].Col3);   //First time home buyer...
                ReportDetails.Amt_Ccol13 = IncomeTax_SectionsList[2].Items[24].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[24].Col3);   //Social secutiy fund...
                ReportDetails.Amt_Ccol14 = IncomeTax_SectionsList[2].Items[25].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[25].Col3);   //Domestic tourism...
                ReportDetails.Amt_Ccol15 = IncomeTax_SectionsList[2].Items[26].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[26].Col3);   //Domestic purchase...
                ReportDetails.Amt_Ccol16 = IncomeTax_SectionsList[2].Items[27].Col3 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[27].Col3);   //Total(1-16)
                #endregion

                #region VALUE

                //Section-A
                ReportDetails.Val_Acol1 = IncomeTax_SectionsList[0].Items[0].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[0].Col4); //Salry wages..
                ReportDetails.Val_Acol2 = IncomeTax_SectionsList[0].Items[1].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[1].Col4); //Less exempted..
                ReportDetails.Val_Acol3 = IncomeTax_SectionsList[0].Items[2].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[2].Col4); //Balance(1-2)
                ReportDetails.Val_Acol4 = IncomeTax_SectionsList[0].Items[3].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[3].Col4); //Less expense..
                ReportDetails.Val_Acol5 = IncomeTax_SectionsList[0].Items[4].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[4].Col4); //Balance(3-4)
                ReportDetails.Val_Acol6 = IncomeTax_SectionsList[0].Items[5].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[5].Col4); //Less allowances..
                ReportDetails.Val_Acol7 = IncomeTax_SectionsList[0].Items[6].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[6].Col4); //Balance(5-6)
                ReportDetails.Val_Acol8 = IncomeTax_SectionsList[0].Items[7].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[7].Col4); //Less donation...
                ReportDetails.Val_Acol9 = IncomeTax_SectionsList[0].Items[8].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[8].Col4); //Balance(7-8)
                ReportDetails.Val_Acol10 = IncomeTax_SectionsList[0].Items[9].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[9].Col4); //Less other..
                ReportDetails.Val_Acol11 = IncomeTax_SectionsList[0].Items[10].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[10].Col4); //Net income(9-10)
                ReportDetails.Val_Acol12 = IncomeTax_SectionsList[0].Items[11].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[11].Col4); //Tax computed..
                ReportDetails.Val_Acol13 = IncomeTax_SectionsList[0].Items[12].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[12].Col4); //Less exemption..
                ReportDetails.Val_Acol14 = IncomeTax_SectionsList[0].Items[13].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[13].Col4); //Tax payable...
                ReportDetails.Val_Acol15 = IncomeTax_SectionsList[0].Items[14].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[14].Col4); //Less witholding..
                ReportDetails.Val_Acol16 = IncomeTax_SectionsList[0].Items[15].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[15].Col4); //Total tax
                ReportDetails.Val_Acol16_1 = IncomeTax_SectionsList[0].Items[16].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[16].Col4); //Total tax - payable
                ReportDetails.Val_Acol16_2 = IncomeTax_SectionsList[0].Items[17].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[17].Col4); //Total tax - overpaid
                ReportDetails.Val_Acol17 = IncomeTax_SectionsList[0].Items[18].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[18].Col4); //Add additional..
                ReportDetails.Val_Acol18 = IncomeTax_SectionsList[0].Items[19].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[19].Col4); //Less tax overpaid
                ReportDetails.Val_Acol19 = IncomeTax_SectionsList[0].Items[20].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[20].Col4); //Less tax paid..
                ReportDetails.Val_Acol20 = IncomeTax_SectionsList[0].Items[21].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[21].Col4); //Tax
                ReportDetails.Val_Acol20_1 = IncomeTax_SectionsList[0].Items[22].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[22].Col4); //Tax - Payable
                ReportDetails.Val_Acol20_2 = IncomeTax_SectionsList[0].Items[23].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[23].Col4); //Tax - Overpaid
                ReportDetails.Val_Acol21 = IncomeTax_SectionsList[0].Items[24].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[24].Col4); //Add surcharge..
                ReportDetails.Val_Acol22 = IncomeTax_SectionsList[0].Items[25].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[25].Col4); //Total Tax
                ReportDetails.Val_Acol22_1 = IncomeTax_SectionsList[0].Items[26].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[26].Col4); //Total Tax - Payable
                ReportDetails.Val_Acol22_2 = IncomeTax_SectionsList[0].Items[27].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[0].Items[27].Col4); //Total Tax - Overpaid

                //Section-B
                ReportDetails.Val_Bcol1 = IncomeTax_SectionsList[1].Items[0].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[0].Col4);   //PF Contribution..
                ReportDetails.Val_Bcol2 = IncomeTax_SectionsList[1].Items[1].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[1].Col4);   //Govt. pension..
                ReportDetails.Val_Bcol3 = IncomeTax_SectionsList[1].Items[2].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[2].Col4);   //Private teacher..
                ReportDetails.Val_Bcol4 = IncomeTax_SectionsList[1].Items[3].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[3].Col4);   //National saving..
                ReportDetails.Val_Bcol5 = IncomeTax_SectionsList[1].Items[4].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[4].Col4);   //Income exemption..
                ReportDetails.Val_Bcol5_1 = IncomeTax_SectionsList[1].Items[5].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[5].Col4);   //Disable taxpayer under 65..
                ReportDetails.Val_Bcol5_2 = IncomeTax_SectionsList[1].Items[6].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[6].Col4);   //Taxpayer aged 65 and above..
                ReportDetails.Val_Bcol6 = IncomeTax_SectionsList[1].Items[7].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[7].Col4);   //Serverence pay..
                ReportDetails.Val_Bcol7 = IncomeTax_SectionsList[1].Items[8].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[1].Items[8].Col4);   //Total(1-5)

                //Section-C   
                ReportDetails.Val_Ccol1 = IncomeTax_SectionsList[2].Items[0].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[0].Col4);   //Taxpayer
                ReportDetails.Val_Ccol2 = IncomeTax_SectionsList[2].Items[1].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[1].Col4);   //Spouse
                ReportDetails.Val_Ccol3 = IncomeTax_SectionsList[2].Items[2].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[2].Col4);   ///Child (15,000)...
                ReportDetails.Val_Ccol3_1 = IncomeTax_SectionsList[2].Items[3].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[3].Col4);   //Fill persoNal ID
                ReportDetails.Val_Ccol3_2 = IncomeTax_SectionsList[2].Items[4].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[4].Col4);   //Child (17,000)...
                ReportDetails.Val_Ccol3_3 = IncomeTax_SectionsList[2].Items[5].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[5].Col4);   //Fill Personal ID
                ReportDetails.Val_Ccol4 = IncomeTax_SectionsList[2].Items[6].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[6].Col4);   //Parental care
                ReportDetails.Val_Ccol4_1 = IncomeTax_SectionsList[2].Items[7].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[7].Col4);   //Father of taxpayer
                ReportDetails.Val_Ccol4_2 = IncomeTax_SectionsList[2].Items[8].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[8].Col4);   //Mother of taxpayer
                ReportDetails.Val_Ccol4_3 = IncomeTax_SectionsList[2].Items[9].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[9].Col4);   //Father of spouse...
                ReportDetails.Val_Ccol4_4 = IncomeTax_SectionsList[2].Items[10].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[10].Col4);   //Mother of spouse...
                ReportDetails.Val_Ccol5 = IncomeTax_SectionsList[2].Items[11].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[11].Col4);   //Disabled/Incompetant...
                ReportDetails.Val_Ccol6 = IncomeTax_SectionsList[2].Items[12].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[12].Col4);   //Health insurance...
                ReportDetails.Val_Ccol6_1 = IncomeTax_SectionsList[2].Items[13].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[13].Col4);   //Father of taxpayer
                ReportDetails.Val_Ccol6_2 = IncomeTax_SectionsList[2].Items[14].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[14].Col4);   //Mother of taxpayer
                ReportDetails.Val_Ccol6_3 = IncomeTax_SectionsList[2].Items[15].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[15].Col4);   //Father of spouse
                ReportDetails.Val_Ccol6_4 = IncomeTax_SectionsList[2].Items[16].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[16].Col4);   //Mother of spouse
                ReportDetails.Val_Ccol7 = IncomeTax_SectionsList[2].Items[17].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[17].Col4);   //Life insurance...
                ReportDetails.Val_Ccol7_1 = IncomeTax_SectionsList[2].Items[18].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[18].Col4);   //Pension insurance...
                ReportDetails.Val_Ccol8 = IncomeTax_SectionsList[2].Items[19].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[19].Col4);   //PF Contribution...
                ReportDetails.Val_Ccol9 = IncomeTax_SectionsList[2].Items[20].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[20].Col4);   //Retirement mutual...
                ReportDetails.Val_Ccol10 = IncomeTax_SectionsList[2].Items[21].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[21].Col4);   //Long term equity...
                ReportDetails.Val_Ccol11 = IncomeTax_SectionsList[2].Items[22].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[22].Col4);   //Interest paid on loan...
                ReportDetails.Val_Ccol12 = IncomeTax_SectionsList[2].Items[23].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[23].Col4);   //First time home buyer...
                ReportDetails.Val_Ccol13 = IncomeTax_SectionsList[2].Items[24].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[24].Col4);   //Social secutiy fund...
                ReportDetails.Val_Ccol14 = IncomeTax_SectionsList[2].Items[25].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[25].Col4);   //Domestic tourism...
                ReportDetails.Val_Ccol15 = IncomeTax_SectionsList[2].Items[26].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[26].Col4);   //Domestic purchase...
                ReportDetails.Val_Ccol16 = IncomeTax_SectionsList[2].Items[27].Col4 == string.Empty ? 0 : Double.Parse(IncomeTax_SectionsList[2].Items[27].Col4);   //Total(1-16)

                #endregion

                #region Additional Info

                ReportDetails.TaxPyr_Txt_Desc1 = ViewState["TaxPayerID"].ToString() == string.Empty ? null : ViewState["TaxPayerID"].ToString();   ///Child (15,000)...

                ReportDetails.Chld1_Txt_Value1 = IncomeTax_SectionsList[2].Items[2].TIT_VALUE1 == string.Empty ? 0 : Int32.Parse(IncomeTax_SectionsList[2].Items[2].TIT_VALUE1);   ///Child (15,000)...
                ReportDetails.Chld2_Txt_Value1 = IncomeTax_SectionsList[2].Items[4].TIT_VALUE1 == string.Empty ? 0 : Int32.Parse(IncomeTax_SectionsList[2].Items[4].TIT_VALUE1); ///Child (17,000)...                                                                                                                                    

                //Fill persoNal ID
                List<string> IdName1 = new List<string>(IncomeTax_SectionsList[2].Items[3].TIT_DESC1.Split(','));
                if (IdName1.Count == 1)
                {
                    ReportDetails.ID1_Txt_Desc1A = IdName1[0].ToString() == string.Empty ? null : IdName1[0].ToString();
                    ReportDetails.ID1_Txt_Desc1B = null;
                    ReportDetails.ID1_Txt_Desc1C = null;
                }
                else if (IdName1.Count == 2)
                {
                    ReportDetails.ID1_Txt_Desc1A = IdName1[0].ToString() == string.Empty ? null : IdName1[0].ToString();
                    ReportDetails.ID1_Txt_Desc1B = IdName1[1].ToString() == string.Empty ? null : IdName1[1].ToString();
                    ReportDetails.ID1_Txt_Desc1C = null;
                }
                else if (IdName1.Count == 3)
                {
                    ReportDetails.ID1_Txt_Desc1A = IdName1[0].ToString() == string.Empty ? null : IdName1[0].ToString();
                    ReportDetails.ID1_Txt_Desc1B = IdName1[1].ToString() == string.Empty ? null : IdName1[1].ToString();
                    ReportDetails.ID1_Txt_Desc1C = IdName1[2].ToString() == string.Empty ? null : IdName1[2].ToString();
                }

                //Fill Personal ID
                List<string> IdName2 = new List<string>(IncomeTax_SectionsList[2].Items[5].TIT_DESC1.Split(','));
                if (IdName2.Count == 1)
                {
                    ReportDetails.ID2_Txt_DescA = IdName2[0].ToString() == string.Empty ? null : IdName2[0].ToString();
                    ReportDetails.ID2_Txt_DescB = null;
                    ReportDetails.ID2_Txt_DescC = null;
                }
                else if (IdName2.Count == 2)
                {
                    ReportDetails.ID2_Txt_DescA = IdName2[0].ToString() == string.Empty ? null : IdName2[0].ToString();
                    ReportDetails.ID2_Txt_DescB = IdName2[1].ToString() == string.Empty ? null : IdName2[1].ToString();
                    ReportDetails.ID2_Txt_DescC = null;
                }
                else if (IdName2.Count == 3)
                {
                    ReportDetails.ID2_Txt_DescA = IdName2[0].ToString() == string.Empty ? null : IdName2[0].ToString();
                    ReportDetails.ID2_Txt_DescB = IdName2[1].ToString() == string.Empty ? null : IdName2[1].ToString();
                    ReportDetails.ID2_Txt_DescC = IdName2[2].ToString() == string.Empty ? null : IdName2[2].ToString();
                }


                ReportDetails.ID1_Val_Value1 = IncomeTax_SectionsList[2].Items[3].TIT_VALUE1 == string.Empty ? 0 : Int32.Parse(IncomeTax_SectionsList[2].Items[3].TIT_VALUE1);   //Fill persoNal ID
                ReportDetails.ID2_Val_Value1 = IncomeTax_SectionsList[2].Items[5].TIT_VALUE1 == string.Empty ? 0 : Int32.Parse(IncomeTax_SectionsList[2].Items[5].TIT_VALUE1);   //Fill Personal ID

                ReportDetails.Fath1_Txt_Desc1 = IncomeTax_SectionsList[2].Items[7].TIT_DESC1 == string.Empty ? null : IncomeTax_SectionsList[2].Items[7].TIT_DESC1;   //Father of taxpayer
                ReportDetails.Moth1_Txt_Desc1 = IncomeTax_SectionsList[2].Items[8].TIT_DESC1 == string.Empty ? null : IncomeTax_SectionsList[2].Items[8].TIT_DESC1;   //Mother of taxpayer
                ReportDetails.SpFth1_Txt_Desc1 = IncomeTax_SectionsList[2].Items[9].TIT_DESC1 == string.Empty ? null : IncomeTax_SectionsList[2].Items[9].TIT_DESC1;   //Father of spouse...
                ReportDetails.SpMth1_Txt_Desc1 = IncomeTax_SectionsList[2].Items[10].TIT_DESC1 == string.Empty ? null : IncomeTax_SectionsList[2].Items[10].TIT_DESC1;   //Mother of spouse...

                ReportDetails.Fath2_Txt_Desc1 = IncomeTax_SectionsList[2].Items[13].TIT_DESC1 == string.Empty ? null : IncomeTax_SectionsList[2].Items[13].TIT_DESC1;   //Father of taxpayer
                ReportDetails.Moth_Txt_Desc1 = IncomeTax_SectionsList[2].Items[14].TIT_DESC1 == string.Empty ? null : IncomeTax_SectionsList[2].Items[14].TIT_DESC1;   //Mother of taxpayer
                ReportDetails.SpFth2_Txt_Desc1 = IncomeTax_SectionsList[2].Items[15].TIT_DESC1 == string.Empty ? null : IncomeTax_SectionsList[2].Items[15].TIT_DESC1;   //Father of spouse
                ReportDetails.SpMth2_Txt_Desc1 = IncomeTax_SectionsList[2].Items[16].TIT_DESC1 == string.Empty ? null : IncomeTax_SectionsList[2].Items[16].TIT_DESC1;   //Mother of spouse

                #endregion

                List<Income_Tax_01BO.ReportDetails> lstData = new List<Income_Tax_01BO.ReportDetails>();
                lstData.Add(ReportDetails);
                dsHrms.Tables.Add(lstData.ToDataTable());
                dsHrms.Tables[0].TableName = "ITDeductionAmt";
            }
        }
        #endregion


        /// <summary>
        /// Clear File
        /// </summary>
        private void ClearFile(string filePath)
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(filePath);
                if (filePath != null && Directory.Exists(filePath))
                {
                    foreach (FileInfo file in di.GetFiles())
                        file.Delete();
                }
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.ExpectationFailed;
            }
        }
        #endregion


        #region Enum
        public enum ActionsEnum
        {
            CANCEL,
            SAVE,
            CHANGE,
            ADD_ACTION,
            VIEW,
            DELETE,
            EDIT,
            SUBMIT,
            WRKFSUBMIT,
            SHOWDETAILS,
            NEW,
            ACTIVATE,
            PETTYCASHACCOUNTSELECTED,
            REPORT,
            PARTY,
            PRINT
        }
        #endregion
    }
}