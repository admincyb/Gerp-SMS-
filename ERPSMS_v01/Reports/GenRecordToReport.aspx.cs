using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.CommonManagement;
using ERPService;
using ERPData;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace ERPSMS_v01.Reports
{
    public partial class GenRecordToReport : System.Web.UI.Page
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string RptType
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
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int RptSubType
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
        /// To maintain report Special Condition
        /// </summary>
        private string RptSplCondn
        {
            get
            {
                return (string)this.ViewState["RptSplCondn"];
            }
            set
            {
                this.ViewState["RptSplCondn"] = value;
            }
        }
        /// <summary>
        /// To maintain Report Pk
        /// </summary>
        private int RecPK
        {
            get
            {
                return (int)this.ViewState["RecPK"];
            }
            set
            {
                this.ViewState["RecPK"] = value;
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
        #endregion

        #region Variables
        BusinessObject.User currentUser;
       

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private ERPEntities currentEntity;
        private CommonService cm;
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

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {

                RptSplCondn = Request.QueryString["SPLCONDN"].ToString();
                GetFieldValues(ControlsEnum.APPTYPE);

                divPeriod.Visible = false;
                divAccount.Visible = false;
                //if (RptType == ApplicationType.PR || RptType == ApplicationType.RFQ)
                //    btnSearch.Visible = false;
                //else
                //    btnSearch.Visible = true;

                GetInitialDate();
                lblBreadCrum.Text = this.GetLocalResourceObject("Print").ToString();
                switch (RptType)
                {

                    
                    case ApplicationType.QAC:
                        divPeriod.Visible = true;
                        divAccount.Visible = true;
                        //GetFieldValues(RptType);
                        SetFieldValues(RptType);
                        break;
                    

                }
            }
        }


        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.RPTPARAMS:
                        AppTypeDetailsList = new List<SPADM_APP_SUB_TYPE_DATA_GET_Result>();
                        cm = new CommonService();
                        AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                        break;
                    case ControlsEnum.APPTYPE:
                        cm = new CommonService();
                        ADM_APP_SUB_TYPE_MST ADMAPPSUBTYPEMSTOBJ;
                        ADMAPPSUBTYPEMSTOBJ = new ADM_APP_SUB_TYPE_MST();
                        List<ADM_APP_TYPE_MST> ADMAPPTYPEMSTLIST;
                        
                        ADMAPPTYPEMSTLIST = new List<ADM_APP_TYPE_MST>();
                        ADMAPPSUBTYPEMSTOBJ.AST_SPL_COND = RptSplCondn;

                        ADMAPPTYPEMSTLIST = cm.GetADM_APP_TYPE_MST_Dtls(ADMAPPSUBTYPEMSTOBJ);
                        foreach (ADM_APP_TYPE_MST APPTYPE in ADMAPPTYPEMSTLIST)
                        {
                            RptType = APPTYPE.APT_CODE;
                            foreach (ADM_APP_SUB_TYPE_MST APSUBTYPE in APPTYPE.ADM_APP_SUB_TYPE_MST)
                            {
                                RptSubType = APSUBTYPE.AST_PK;
                            }
                            
                        }
                        //RptType = Request.QueryString["APPTYPE"].ToString();
                        //hdfAppType.Value = RptType;

                        //RptSubType = Request.QueryString["APPSUBTYPE"] != string.Empty ? Convert.ToInt32(Request.QueryString["APPSUBTYPE"]) : 0;
                        //if (Request.QueryString["ID"].ToString().Trim() != string.Empty)
                        //    RecPK = Convert.ToInt32(Request.QueryString["ID"]);
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
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(string appType)
        {
            try
            {
                LocalReport locRpt;

                cm = new CommonService();
                locRpt = null;
                rvViewReport.LocalReport.DataSources.Clear();
                rvViewReport.Visible = true;
                locRpt = rvViewReport.LocalReport;

                rvViewReport.LocalReport.DataSources.Clear();
                locRpt.EnableExternalImages = true;
                GetFieldValues(ControlsEnum.RPTPARAMS);
                switch (appType)
                {
                   
                    #region QAC
                    case ApplicationType.QAC:
                        ReportDataSource dsTB;
                        currentEntity = new ERPEntities();
                        dsTB = new ReportDataSource("TBDtls", currentEntity.SPFIN_TRIAL_BALANCE_RPT(currentUser.SBUID, Convert.ToDateTime(txtFromDate.Text.Trim()), Convert.ToDateTime(txtToDate.Text.Trim()), null));
                        if (dsTB != null)
                        {
                            //AppTypeDetailsList = cm.GetReportParameters(RptType, RptSubType, AppvdDate);
                            SetReportParameters(locRpt);
                            rvViewReport.LocalReport.DataSources.Add(dsTB);
                        }
                        else
                        {
                            rvViewReport.Visible = false;
                        }
                        break;
                    #endregion
                    

                }
                rvViewReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                rvViewReport.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
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
                dsParamSettings = new DataSet();

                locRpt.ReportPath = string.Empty;
                foreach (SPADM_APP_SUB_TYPE_DATA_GET_Result sa in AppTypeDetailsList)
                {
                    rptName = sa.AST_OP_FILE1;//
                    locRpt.ReportPath = Server.MapPath(rptName);
                    parameters = new ReportParameter("QMSRef", sa.AST_QMS_REF);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("HideQMSRef", sa.AST_QMS_VISIBILITY.ToString());
                    locRpt.SetParameters(parameters);
                    if (sa.AST_RPT_SETTINGS != null)
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(sa.AST_RPT_SETTINGS)));

                    if (RptType == ApplicationType.PO || RptType == ApplicationType.RFQ || RptType == ApplicationType.SO || RptType == ApplicationType.IO || RptType == ApplicationType.DO)
                    {
                        parameters = new ReportParameter("ApprovedByName", sa.AST_APPROVED_USER);
                        locRpt.SetParameters(parameters);
                        if (Convert.ToString(sa.AST_APPROVED_SIGN) != string.Empty)
                        {
                            parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath("~\\Reports\\Images\\" + sa.AST_APPROVED_SIGN));
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

                    if (RptType == ApplicationType.DN || RptType == ApplicationType.CN)
                    {
                        parameters = new ReportParameter("ParamReason", RptType == ApplicationType.DN ? this.GetLocalResourceObject("lblDNReason").ToString() : this.GetLocalResourceObject("lblCNReason").ToString());
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

                parameters = new ReportParameter("Logo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
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
        /// <summary>
        /// Method for Type & Category Dropdowns
        /// </summary>
        public void BindSubLedger(ControlsEnum ctrlType)
        {
            switch (ctrlType)
            {
                case ControlsEnum.PARTY:

                    break;
                case ControlsEnum.ACCGROUP:
                    break;
                case ControlsEnum.TYPE:
                    break;
            }
            //CommonService CommonServiceClient;
            //FIN_COA_SUB_TYPE_CFG finCoaSubTypeCfgObj;
            //List<FIN_COA_SUB_TYPE_CFG> finCoaSubTypeCfgList;
            //CommonServiceClient = new CommonService();

            //finCoaSubTypeCfgObj = ERP.Utilities.CommonFunctions.Initilize<FIN_COA_SUB_TYPE_CFG>();
            //finCoaSubTypeCfgObj.CST_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
            //finCoaSubTypeCfgObj.CST_PK = -1;
            //finCoaSubTypeCfgList = CommonServiceClient.GetSubTypeCfgValues(finCoaSubTypeCfgObj);
            //ddlSubLedger.Items.Clear();
            //if (finCoaSubTypeCfgList != null && finCoaSubTypeCfgList.Count > 0)
            //{
            //    ddlSubLedger.DataSource = finCoaSubTypeCfgList;
            //    ddlSubLedger.DataTextField = Resources.DataFieldRes.CoaSubTypeName;
            //    ddlSubLedger.DataValueField = Resources.DataFieldRes.CoaSubTypePK;
            //    ddlSubLedger.DataBind();
            //}
            //ddlSubLedger.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }
        /// <summary>
        /// Gets Initial Date
        /// </summary>
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
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    if (RptType == ApplicationType.QAC)
                    {
                        GetFieldValues(ControlsEnum.RPTPARAMS);
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
                            //GetSelectedAccounts();
                        }
                        else
                        {
                            if (!ValidateForm())
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GTIService.CommonFunctions.FormatErrorMessage(litErrorMsg.Text.Split('^')) + "');", true);
                            }
                            else
                            {
                                //GetSelectedAccounts();
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
                            //GetSelectedAccounts();
                            SetFieldValues(RptType);
                        }
                    }


                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    if (RptType == ApplicationType.AS)
                    {
                        if (((ImageButton)sender).CommandName == "SHOWACCOUNTS")
                        {
                            //txtAccount.Text = string.Empty;
                            //hdfAccount.Value = string.Empty;
                            //BindTree();
                            //divAccPopUp.Visible = true;
                            // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AccountsTree", "ShowAccountsTree();", true);
                        }
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlSubLedger")
                    {
                        //BindTree();
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
        public enum ControlsEnum
        {
            PARTY,
            ACCGROUP,
            TYPE,
            APPTYPE,
            RPTPARAMS
        }
       
    }
}