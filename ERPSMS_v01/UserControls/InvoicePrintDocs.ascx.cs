using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPService;
using ERP.Utilities;
using ERPData;
using System.Data;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using ERPManager;
using ERPService.Sales;

namespace ERPSMS_v01.UserControls
{
    public partial class InvoicePrintDocs : System.Web.UI.UserControl
    {

        #region Variables and Properties

        private ADM_APP_SUB_TYPE_MST admAppSubTypeMstObj;
        private List<ADM_APP_SUB_TYPE_MST> admAppSubTypeMstList;
        private List<ADM_APP_SUB_TYPE_MST> admAppSubTypeMstList_CPY;

        private FIN_INVOICE_CUS_HDR FIN_INVOICE_CUS_HDRObj;
        private List<FIN_INVOICE_CUS_HDR> FIN_INVOICE_CUS_HDRList;

        private List<SAL_DESPATCH_HDR> salDespatchHdrList;

        private SAL_DESPATCH_DTL salDespatchdtlobj;
        private List<SAL_DESPATCH_DTL> salDespatchdtlList;

        private DataSet dsShippingPlanHDR;
        private DataTable dtCommericalInvoice;

        private ServiceUtility serviceUtilityObj;
        private List<SAL_CONTAINER_INSP_HDR> SalContainerInspHdrList;
        private SAL_CONTAINER_INSP_HDR SalContainerInspHdrObj;

        private int GONPK = 0;
        private int SOPK = 0;

        private ActionsEnum commonActions;

        DataTable dtRepeat;

        public int ShippingPlanID
        {
            get { return ViewState["ShippingPlanID"] == null ? 0 : Convert.ToInt32(ViewState["ShippingPlanID"]); }
            set { ViewState["ShippingPlanID"] = value; }
        }
        public int InvoicePK
        {
            get { return ViewState["InvoicePK"] == null ? 0 : Convert.ToInt32(ViewState["InvoicePK"]); }
            set { ViewState["InvoicePK"] = value; }
        }
        public int DelStatus
        {
            get { return ViewState["DelStatus"] == null ? 0 : Convert.ToInt32(ViewState["DelStatus"]); }
            set { ViewState["DelStatus"] = value; }
        }
        public string InvoiceNo
        {
            get { return (string)this.ViewState["InvoiceNo"]; }
            set { this.ViewState["InvoiceNo"] = value; }
        }
        public int InvoiceType
        {
            get { return ViewState["InvoiceType"] == null ? 0 : Convert.ToInt32(ViewState["InvoiceType"]); }
            set { ViewState["InvoiceType"] = value; }
        }
        #endregion

        #region PageMethods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetFieldValues(ControlsEnum.PRINTLIST);
                SetFieldValues(ControlsEnum.PRINTLIST);
                if (GetGlobalResourceObject("ConfigurationsRes", "ShowSICopyPrintSection").ToString() == CommonConstants.SELECT_VALUE_ONE)
                {
                    lblSICopyPrint.Visible = ddlSICopy.Visible = btnPrintCopySI.Visible = true;
                }
                else
                {
                    lblSICopyPrint.Visible = ddlSICopy.Visible = btnPrintCopySI.Visible = false;
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
        private void GetFieldValues(ControlsEnum type)
        {

            ContainerInspectionService ContainerInspectionServiceClient = null;

            SalesInvoiceService SalesInvoiceServiceClient = null;
            SalDespatchDtlService SalDespatchDtlServiceClient;

            CommonService CommonServiceClient;
            CommonServiceClient = null;

            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            string getxml;

            try
            {

                CommonServiceClient = new CommonService();
                switch (type)
                {
                    case ControlsEnum.PRINTLIST:
                        CommonServiceClient = new CommonService();
                        admAppSubTypeMstObj = CommonFunctions.Initilize<ADM_APP_SUB_TYPE_MST>();
                        admAppSubTypeMstObj.ADM_APP_TYPE_MST = CommonFunctions.Initilize<ADM_APP_TYPE_MST>();
                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.DO;
                        admAppSubTypeMstObj.AST_SPL_COND = "RPT";
                        admAppSubTypeMstList = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList = admAppSubTypeMstList.OrderBy(c => c.AST_NAME).ToList();
                        //admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.SPLN;
                        //admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        //admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                        //admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();
                        //admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.CNTINSP;
                        //admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        //admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                        //admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();
                        break;
                    case ControlsEnum.GONDETAILS:
                        SalesInvoiceServiceClient = new SalesInvoiceService();
                        FIN_INVOICE_CUS_HDRObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                        FIN_INVOICE_CUS_HDRObj.ICH_PK = InvoicePK;
                        FIN_INVOICE_CUS_HDRObj.ICH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        FIN_INVOICE_CUS_HDRObj.ICH_DEL_STATUS = Convert.ToByte(DelStatus);
                        FIN_INVOICE_CUS_HDRList = SalesInvoiceServiceClient.GetInvoiceCusHdrByPK(FIN_INVOICE_CUS_HDRObj);
                        break;
                    case ControlsEnum.ADMAPPSUBTYPES:
                        CommonServiceClient = new CommonService();
                        admAppSubTypeMstObj = CommonFunctions.Initilize<ADM_APP_SUB_TYPE_MST>();
                        admAppSubTypeMstObj.ADM_APP_TYPE_MST = CommonFunctions.Initilize<ADM_APP_TYPE_MST>();
                        admAppSubTypeMstObj.AST_PK = Convert.ToInt16(ddlSDPrint.SelectedValue);
                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = string.Empty;
                        admAppSubTypeMstList = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);

                        break;
                    case ControlsEnum.SPHDR:
                        dsShippingPlanHDR = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanHDR(currentUser, ShippingPlanID, Convert.ToInt32(CommonConstants.ACTIVE));
                        break;
                    case ControlsEnum.COMMERICALINVOICE:
                        dtCommericalInvoice = BusinessLogic.Shipping.ShippingPlanBL.GetCommericalInvoice(ShippingPlanID);
                        break;
                    #region SalContainerInspHdr List
                    case ControlsEnum.CONTAINERINSPECTION:
                        SalContainerInspHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_INSP_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        ContainerInspectionServiceClient = new ContainerInspectionService();
                        ContainerInspectionServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerInspectionServiceClient);
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        SalContainerInspHdrObj.CSH_SHIPPING_PLAN = ShippingPlanID;
                        SalContainerInspHdrObj.CSH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        SalContainerInspHdrList = ContainerInspectionServiceClient.GetContainerInspectionList(SalContainerInspHdrObj, serviceUtilityObj);
                        break;
                    #endregion

                    case ControlsEnum.GODETAILS:
                        SalDespatchDtlServiceClient = new SalDespatchDtlService();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admAppSubTypeMstObj = null;
                SalContainerInspHdrObj = null;
                CommonServiceClient = null;
                ContainerInspectionServiceClient = null;
                SalesInvoiceServiceClient = null;
                SalDespatchDtlServiceClient = null;
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
                    case ControlsEnum.PRINTLIST:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.GONDETAILS:
                        if (FIN_INVOICE_CUS_HDRList != null && FIN_INVOICE_CUS_HDRList.Count > 0)
                        {
                            GONPK = FIN_INVOICE_CUS_HDRList[0].ICH_DESPATCH_HDR.ToString() != string.Empty ? Convert.ToInt32(FIN_INVOICE_CUS_HDRList[0].ICH_DESPATCH_HDR.ToString()) : 0;
                            //GONPK = 0;
                            //List<int> SOHdrPks = FIN_INVOICE_CUS_HDRList[0].FIN_INVOICE_CUS_TRX_MPG.Select(c => c.ICM_SO_HDR).ToList();
                            //if (SOHdrPks != null && SOHdrPks.Count > 0)
                            //{
                            //    SOPK = SOHdrPks[0];

                            //}
                        }
                        else
                        {
                            GONPK = 0;
                        }
                        break;
                    case ControlsEnum.COMMERICALINVOICE:
                        BindDropDown(controlType);
                        break;

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
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.PRINTLIST:
                    ddlSDPrint.Items.Clear();
                    if (admAppSubTypeMstList != null && admAppSubTypeMstList.Count > 0)
                    {
                        ddlSDPrint.DataSource = admAppSubTypeMstList;
                        ddlSDPrint.DataTextField = "AST_NAME";
                        ddlSDPrint.DataValueField = "AST_PK";
                        ddlSDPrint.DataBind();
                    }
                    break;
                case ControlsEnum.COMMERICALINVOICE:
                    //ddlCIPrint.Items.Clear();
                    //if (dtCommericalInvoice != null && dtCommericalInvoice.Rows.Count > 0)
                    //{
                    //    ddlCIPrint.DataSource = dtCommericalInvoice;
                    //    ddlCIPrint.DataTextField = "ICH_TEXT";
                    //    ddlCIPrint.DataValueField = "ICH_PK";
                    //    ddlCIPrint.DataBind();
                    //}
                    //ddlCIPrint.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.SICOPYTEXT:
                    ddlSICopy.Items.Clear();
                    if (dtRepeat != null && dtRepeat.Rows.Count > 0)
                    {
                        ddlSICopy.DataSource = dtRepeat;
                        ddlSICopy.DataTextField = "RPT_TEXT";
                        ddlSICopy.DataValueField = "RPT_SLNO";
                        ddlSICopy.DataBind();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// set Commerical invoice
        /// </summary>
        /// <returns></returns>      
        public void SetCommericalInvoice(int SPlanID)
        {
            ShippingPlanID = SPlanID;
            GetFieldValues(ControlsEnum.COMMERICALINVOICE);
            SetFieldValues(ControlsEnum.COMMERICALINVOICE);

        }

        private void GenerateRepeatTable(int Count)
        {
            DataRow row;
            dtRepeat = new DataTable();
            string SlNo = GetGlobalResourceObject("DataFieldRes", "RPT_SLNO").ToString();
            string Txt = GetGlobalResourceObject("DataFieldRes", "RPT_TEXT").ToString();
            string copy = GetGlobalResourceObject("Controls", "Copy").ToString();

            dtRepeat.Columns.Add(SlNo, typeof(string));
            dtRepeat.Columns.Add(Txt, typeof(string));

            for (int i = 2; i <= Count; i++)
            {
                row = dtRepeat.NewRow();
                row[SlNo] = copy + " " + (i - 1);
                row[Txt] = InvoiceNo + " (" + copy + "-" + (i - 1) + ")";
                dtRepeat.Rows.Add(row);
            }
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

            try
            {
                bool reportgenrated = false;
                string reoprturl = "";

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
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                switch (commonActions)
                {

                    #region PRINTGON
                    case ActionsEnum.PRINTGON:
                        if (InvoicePK != 0)
                        {
                            GetFieldValues(ControlsEnum.ADMAPPSUBTYPES);
                            if (admAppSubTypeMstList != null && admAppSubTypeMstList.Count > 0)
                            {

                                switch (admAppSubTypeMstList[0].ADM_APP_TYPE_MST.APT_CODE)
                                {
                                    case ApplicationType.DO:
                                        GetFieldValues(ControlsEnum.GONDETAILS);
                                        SetFieldValues(ControlsEnum.GONDETAILS);
                                        if (GONPK != 0)
                                        {
                                            reportgenrated = true;
                                            reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + GONPK + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=" + admAppSubTypeMstList[0].AST_VALUE);
                                        }
                                        else
                                        {
                                            reportgenrated = false;
                                        }
                                        break;
                                    case ApplicationType.SPLN:
                                        GetFieldValues(ControlsEnum.SPHDR);
                                        if (dsShippingPlanHDR != null && dsShippingPlanHDR.Tables[0].Rows.Count > 0)
                                        {
                                            int trxStatus = Convert.ToInt32(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_TRX_STATUS"].ToString());
                                            if (admAppSubTypeMstList[0].AST_VALUE.ToString() == AppSubType.SPLNLP)
                                            {
                                                if (trxStatus >= (int)ShippingTabsEnum.LoadingPlanCompleted)
                                                {
                                                    reportgenrated = true;
                                                    reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + dsShippingPlanHDR.Tables[0].Rows[0]["SNH_PK"] + "&APPTYPE=" + ApplicationType.SPLN + "&APPSUBTYPE=" + AppSubType.SPLNLP);

                                                }
                                                else
                                                {
                                                    reportgenrated = false;
                                                }
                                            }
                                            else if (admAppSubTypeMstList[0].AST_VALUE.ToString() == AppSubType.SPLNEF)
                                            {
                                                if (trxStatus >= (int)ShippingTabsEnum.ExportDocsUploaded)
                                                {
                                                    reportgenrated = true;
                                                    reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + dsShippingPlanHDR.Tables[0].Rows[0]["SNH_PK"] + "&APPTYPE=" + ApplicationType.SPLN + "&APPSUBTYPE=" + AppSubType.SPLNEF);

                                                }
                                                else
                                                {
                                                    reportgenrated = false;

                                                }
                                            }
                                        }
                                        else
                                        {
                                            reportgenrated = false;
                                        }
                                        break;
                                    case ApplicationType.CNTINSP:
                                        GetFieldValues(ControlsEnum.SPHDR);
                                        GetFieldValues(ControlsEnum.CONTAINERINSPECTION);
                                        if (dsShippingPlanHDR != null && dsShippingPlanHDR.Tables[0].Rows.Count > 0)
                                        {
                                            int trxStatus = Convert.ToInt32(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_TRX_STATUS"].ToString());
                                            if (trxStatus >= (int)ShippingTabsEnum.ContainerInspected)
                                            {
                                                if (SalContainerInspHdrList != null && SalContainerInspHdrList.Count > 0)
                                                {
                                                    reportgenrated = true;
                                                    reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + SalContainerInspHdrList[0].CSH_PK + "&APPTYPE=" + ApplicationType.CNTINSP + "&APPSUBTYPE=");

                                                }
                                                else
                                                    reportgenrated = false;

                                            }
                                            else
                                            {
                                                reportgenrated = false;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        if (reportgenrated)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.SalesInvoicePrint + "','430','140');", true);

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + reoprturl + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.SalesInvoicePrint + "','430','140');", true);

                            string msg = Resources.Messages.ReportError.ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region PRINTCI
                    case ActionsEnum.PRINTCI:
                        //if (ddlCIPrint.SelectedValue != CommonConstants.SELECTVAL)
                        //{
                        //    reportgenrated = true;
                        //    string Type = ddlCIPrint.SelectedItem.Text;
                        //    if (Type.Split('/')[0] == ApplicationType.CI)
                        //    {
                        //        reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=2");
                        //    }
                        //    else
                        //    {
                        //        reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=1");
                        //    }
                        //}
                        //if (reportgenrated)
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + reoprturl + "');", true);
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);
                        //    string msg = Resources.Messages.Err_CommericalInvoice.ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
                        //}

                        break;
                    #endregion

                    #region PRINTSI
                    case ActionsEnum.PRINTSI:
                        if (InvoicePK != 0)
                        {
                            if (InvoiceType == (int)SalesInvoiceType.Domestic)
                            {
                                reportgenrated = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                   InvoicePK + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Domestic) + "&SICOUNTTEXT=" + ddlSICopy.SelectedValue + "');", true);
                            }
                            else if (InvoiceType == (int)SalesInvoiceType.Export)
                            {
                                reportgenrated = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                InvoicePK + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export) + "&SICOUNTTEXT=" + ddlSICopy.SelectedValue + "');", true);
                            }
                            //else if (InvoiceType == (int)SalesInvoiceType.Proforma)
                            //{
                            //    reportgenrated = true;
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                            //                    InvoicePK + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma) + "&SICOUNTTEXT=" + ddlSICopy.SelectedValue + "');", true);
                            //}
                        }
                        else
                        {
                            reportgenrated = false;
                        }

                        if (reportgenrated)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.SalesInvoicePrint + "','430','140');", true);

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + reoprturl + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.SalesInvoicePrint + "','420','180');", true);

                            string msg = Resources.Messages.ReportError.ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    default:
                        break;
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

        public void PrintDoc_Click(object sender, EventArgs e)
        {
            GenerateRepeatTable(Convert.ToInt16(GetGlobalResourceObject("Constants", "InvoiceCopyCount")));
            BindDropDown(ControlsEnum.SICOPYTEXT);

            //Copy Reports only for Domestic Type
            if (InvoiceType == (int)SalesInvoiceType.Domestic)
                lblSICopyPrint.Visible = ddlSICopy.Visible = btnPrintCopySI.Visible = true;
            else
                lblSICopyPrint.Visible = ddlSICopy.Visible = btnPrintCopySI.Visible = false;
        }

        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            SHIPPINGPLANLIST,
            SHIPPINGPLANHDR,
            SHIPPINGPLANDETAILS,
            PLANNO,
            SHIPPINGPLANDTL,
            WRKFSUBMIT,
            STATUS,
            SPDEATILS,
            SPCONTAINERTYPE,
            PRINTLIST,
            GONDETAILS,
            ADMAPPSUBTYPES,
            SPHDR,
            COMMERICALINVOICE,
            CONTAINERINSPECTION,
            GODETAILS,
            SICOPYTEXT
        }
        #endregion
    }
}