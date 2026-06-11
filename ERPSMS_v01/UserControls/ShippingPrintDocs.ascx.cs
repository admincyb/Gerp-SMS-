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
    public partial class ShippingPrintDocs : System.Web.UI.UserControl
    {

        #region Variables and Properties

        private ADM_APP_SUB_TYPE_MST admAppSubTypeMstObj;
        private List<ADM_APP_SUB_TYPE_MST> admAppSubTypeMstList;
        private List<ADM_APP_SUB_TYPE_MST> admAppSubTypeMstList_CPY;

        private List<ADM_APP_SUB_TYPE_MST> TempadmAppSubTypeMstList;

        private List<SAL_DESPATCH_HDR> salDespatchHdrList;
        private List<SAL_SHIPPING_PLAN_DTL> salDespatchDtlList;
        private List<SAL_ORDER_HDR> salOrderHdrList;
        private SAL_ORDER_HDR salOrderHdrObj;
        private List<SAL_ORDER_DTL> salOrdrDtlList;


        private DataSet dsShippingPlanHDR;
        private DataSet dsSaleOrder;
        private DataTable dtCommericalInvoice;

        private ServiceUtility serviceUtilityObj;
        private List<SAL_CONTAINER_INSP_HDR> SalContainerInspHdrList;
        private SAL_CONTAINER_INSP_HDR SalContainerInspHdrObj;

        private int GONPK = 0;
        private int SONPK = 0;

        private ActionsEnum commonActions;

        public int ShippingPlanID
        {
            get { return ViewState["ShippingPlanID"] == null ? 0 : Convert.ToInt32(ViewState["ShippingPlanID"]); }
            set { ViewState["ShippingPlanID"] = value; }
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

        private bool IsInvoiceTerms
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsInvoiceTerms] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsInvoiceTerms].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsInvoiceTerms] = value;
            }
        }

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

        public int SPPrintDocsLimit
        {
            get
            {
                return this.ViewState["SPPrintDocsLimit"] == null ? 0 : Convert.ToInt32(this.ViewState["SPPrintDocsLimit"]);
            }
            set
            {
                this.ViewState["SPPrintDocsLimit"] = value;
            }
        }
        #endregion

        #region PageMethods
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SPPrintDocsLimit = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "SPPrintDocsLimit"));
                GetFieldValues(ControlsEnum.PRINTLIST);
                SetFieldValues(ControlsEnum.PRINTLIST);
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
            string ICHTYPE = "";
            ContainerInspectionService ContainerInspectionServiceClient = null;
            SaleOrderService SaleOrderServiceClient = null;

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
                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.SPLN;
                        admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                        admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();

                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.CNTINSP;
                        admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                        admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();

                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.SO;
                        admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                        admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();

                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.SIC;
                        admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                        admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();

                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.CID;
                        admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                        admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();

                        break;
                    case ControlsEnum.GONDETAILS:
                        salDespatchHdrList = CommonServiceClient.GetGONHdr(ShippingPlanID);
                        break;
                    case ControlsEnum.SODETAILS:

                        salOrdrDtlList = CommonServiceClient.GetGONDtl(ShippingPlanID);

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
                        ViewState["dtInvoice"] = dtCommericalInvoice;
                        if (dtCommericalInvoice.Rows.Count > 0)
                        {
                              ICHTYPE = dtCommericalInvoice.Rows[0]["ICH_TYPE"].ToString();
                        }


                        CommonServiceClient = new CommonService();
                        admAppSubTypeMstObj = CommonFunctions.Initilize<ADM_APP_SUB_TYPE_MST>();
                        admAppSubTypeMstObj.ADM_APP_TYPE_MST = CommonFunctions.Initilize<ADM_APP_TYPE_MST>();
                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.DO;
                        admAppSubTypeMstObj.AST_SPL_COND = "RPT";
                        admAppSubTypeMstList = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList = admAppSubTypeMstList.OrderBy(c => c.AST_NAME).ToList();
                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.SPLN;
                        admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                        admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();

                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.CNTINSP;
                        admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                        admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();

                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.SO;
                        admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                        admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();

                        admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.SIC;
                        admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                        admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                        admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();
                        if (ICHTYPE.Trim() == "2")
                        {
                            admAppSubTypeMstObj.ADM_APP_TYPE_MST.APT_CODE = ApplicationType.CID;
                            admAppSubTypeMstList_CPY = CommonServiceClient.GetADMAPPSUBTYPEMST_Dtls(admAppSubTypeMstObj);
                            admAppSubTypeMstList_CPY = admAppSubTypeMstList_CPY.OrderBy(c => c.AST_NAME).ToList();
                            admAppSubTypeMstList = admAppSubTypeMstList.Union(admAppSubTypeMstList_CPY).ToList();
                        }


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
                        if (salDespatchHdrList != null && salDespatchHdrList.Count > 0)
                        {
                            GONPK = salDespatchHdrList[0].DPH_PK.ToString() != string.Empty ? Convert.ToInt32(salDespatchHdrList[0].DPH_PK.ToString()) : 0;
                        }
                        else
                        {
                            GONPK = 0;
                        }
                        break;
                    case ControlsEnum.SODETAILS:
                        if (salOrdrDtlList != null && salOrdrDtlList.Count > 0)
                        {
                            SONPK = salOrdrDtlList[0].SOD_SO.ToString() != string.Empty ? Convert.ToInt32(salOrdrDtlList[0].SOD_SO.ToString()) : 0;
                        }
                        else
                        {
                            SONPK = 0;
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
                        if (SPPrintDocsLimit == 1)//ONLY SHOW TWO TYPE PRINT
                        {
                            TempadmAppSubTypeMstList = new List<ADM_APP_SUB_TYPE_MST>();
                            for (int i = 0; i < admAppSubTypeMstList.Count; i++)
                            {
                                if (admAppSubTypeMstList[i].AST_CODE == "DO6" || admAppSubTypeMstList[i].AST_CODE == "DO2")
                                {
                                    TempadmAppSubTypeMstList.Add(admAppSubTypeMstList[i]);
                                }
                            }
                            ddlSDPrint.DataSource = TempadmAppSubTypeMstList;
                            TempadmAppSubTypeMstList = null;
                        }
                        else
                        {
                            ddlSDPrint.DataSource = admAppSubTypeMstList;
                        }
                        ddlSDPrint.DataTextField = "AST_NAME";
                        ddlSDPrint.DataValueField = "AST_PK";
                        ddlSDPrint.DataBind();
                    }
                    break;
                case ControlsEnum.COMMERICALINVOICE:
                    GetFieldValues(ControlsEnum.COMMERICALINVOICE);
                    ddlSDPrint.Items.Clear();
                    if (admAppSubTypeMstList != null && admAppSubTypeMstList.Count > 0)
                    {
                        if (SPPrintDocsLimit == 1)//ONLY SHOW TWO TYPE PRINT
                        {
                            TempadmAppSubTypeMstList = new List<ADM_APP_SUB_TYPE_MST>();
                            for (int i = 0; i < admAppSubTypeMstList.Count; i++)
                            {
                                if (admAppSubTypeMstList[i].AST_CODE == "DO6" || admAppSubTypeMstList[i].AST_CODE == "DO2")
                                {
                                    TempadmAppSubTypeMstList.Add(admAppSubTypeMstList[i]);
                                }
                            }
                            ddlSDPrint.DataSource = TempadmAppSubTypeMstList;
                            TempadmAppSubTypeMstList = null;
                        }
                        else
                        {
                            ddlSDPrint.DataSource = admAppSubTypeMstList;
                        }
                        ddlSDPrint.DataTextField = "AST_NAME";
                        ddlSDPrint.DataValueField = "AST_PK";
                        ddlSDPrint.DataBind();
                    }

                    ddlCIPrint.Items.Clear();
                    if (dtCommericalInvoice != null && dtCommericalInvoice.Rows.Count > 0)
                    {
                        ddlCIPrint.DataSource = dtCommericalInvoice;
                        ddlCIPrint.DataTextField = "ICH_TEXT";
                        ddlCIPrint.DataValueField = "ICH_PK";
                        ddlCIPrint.DataBind();
                    }
                    ddlCIPrint.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
        private void ConfigurationSettings()
        {
            IsExportExcel = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsExportExcel")));
            IsInvoiceTerms = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsInvoiceTerms")));

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
                        if (ShippingPlanID != 0)
                        {
                            GetFieldValues(ControlsEnum.ADMAPPSUBTYPES);
                            if (admAppSubTypeMstList != null && admAppSubTypeMstList.Count > 0)
                            {
                                ConfigurationSettings();
                                switch (admAppSubTypeMstList[0].ADM_APP_TYPE_MST.APT_CODE)
                                {
                                    case ApplicationType.DO:
                                        GetFieldValues(ControlsEnum.GONDETAILS);
                                        SetFieldValues(ControlsEnum.GONDETAILS);
                                        if (GONPK != 0)
                                        {
                                            reportgenrated = true;
                                            if (IsExportExcel && (admAppSubTypeMstList[0].AST_VALUE == 6 || admAppSubTypeMstList[0].AST_VALUE == 2 || admAppSubTypeMstList[0].AST_VALUE == 7 || admAppSubTypeMstList[0].AST_VALUE == 8 || admAppSubTypeMstList[0].AST_VALUE == 9))
                                            {
                                                reportgenrated = false;
                                                Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + GONPK + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=" + admAppSubTypeMstList[0].AST_VALUE);
                                            }
                                            else
                                            {
                                                reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + GONPK + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=" + admAppSubTypeMstList[0].AST_VALUE);
                                            }
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
                                    case ApplicationType.SO:
                                        GetFieldValues(ControlsEnum.SODETAILS);
                                        SetFieldValues(ControlsEnum.SODETAILS);

                                        if (SONPK != 0)
                                        {
                                            reportgenrated = true;
                                            if (IsExportExcel && (admAppSubTypeMstList[0].AST_VALUE == 6 || admAppSubTypeMstList[0].AST_VALUE == 2 || admAppSubTypeMstList[0].AST_VALUE == 7 || admAppSubTypeMstList[0].AST_VALUE == 8 || admAppSubTypeMstList[0].AST_VALUE == 9))
                                            {
                                                reportgenrated = false;
                                                Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + SONPK + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=" + admAppSubTypeMstList[0].AST_VALUE);
                                            }
                                            else
                                            {
                                                reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + SONPK + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=" + admAppSubTypeMstList[0].AST_VALUE);
                                            }
                                        }
                                        else
                                        {
                                            reportgenrated = false;
                                        }
                                        break;
                                    case ApplicationType.SIC:
                                        int ICH_PK;// dtCommericalInvoice.Rows.Count > 0 ? Convert.ToInt16(dtCommericalInvoice.Rows[0]["ICH_PK"]) : 0;
                                        if (ddlSDPrint.SelectedValue != CommonConstants.SELECTVAL)
                                        {
                                            DataTable dt = (DataTable)ViewState["dtInvoice"];
                                            if (dt != null && dt.Rows.Count > 0)
                                            {
                                                ICH_PK = dt.Rows.Count > 0 ? Convert.ToInt16(dt.Rows[0]["ICH_PK"]) : 0;
                                                ConfigurationSettings();
                                                reportgenrated = true;
                                                if (IsExportExcel)
                                                {
                                                    reportgenrated = false;
                                                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + ddlSDPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SIC + "&APPSUBTYPE=" + admAppSubTypeMstList[0].AST_VALUE);
                                                }
                                                else
                                                {
                                                    if (ICH_PK > 0)
                                                        reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ICH_PK + "&APPTYPE=" + ApplicationType.SIC + "&APPSUBTYPE=" + admAppSubTypeMstList[0].AST_VALUE);
                                                    else
                                                        reportgenrated = false;
                                                }
                                            }
                                        }
                                        break;
                                    case ApplicationType.CID:
                                        ConfigurationSettings();
                                        DataTable dt1 = (DataTable)ViewState["dtInvoice"];
                                        if (dt1 != null && dt1.Rows.Count > 0)
                                        {
                                            int SPid;
                                            //DataTable dtrow = dt1.Select("ICH_PK = " + ddlCIPrint.SelectedValue).CopyToDataTable();
                                            DataTable dtrow = dt1;
                                            SPid= Convert.ToInt32(dtrow.Rows[0]["ICH_PK"].ToString());
                                            int Type = Convert.ToInt32(dtrow.Rows[0]["ICH_TYPE"].ToString());
                                            int TermType = Convert.ToInt32(dtrow.Rows[0]["ICH_DEL_TERM_VALUE"].ToString());

                                            reportgenrated = true;

                                            if (IsInvoiceTerms == true)
                                            {
                                                if (Type.ToString() == "2")
                                                {

                                                    reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + SPid + "&APPTYPE=" + ApplicationType.CID + "&APPSUBTYPE=" + Type);

                                                }

                                                else
                                                {
                                                    //reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + SPid + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + admAppSubTypeMstList[0].AST_VALUE);
                                                    reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + SPid + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + Type);

                                                }

                                            }
                                            else
                                            {
                                                if (IsExportExcel)
                                                {
                                                    reportgenrated = false;
                                                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + SPid + "&APPTYPE=" + ApplicationType.CID + "&APPSUBTYPE=" + Type);
                                                }
                                                else
                                                {
                                                    reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + SPid + "&APPTYPE=" + ApplicationType.CID + "&APPSUBTYPE=" + Type);
                                                }
                                            }
                                        }
                                        break;

                                }
                            }
                        }
                        if (reportgenrated)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + reoprturl + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);

                            string msg = Resources.Messages.ReportError.ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region PRINTCI
                    case ActionsEnum.PRINTCI:
                        if (ddlCIPrint.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            ConfigurationSettings();
                            DataTable dt = (DataTable)ViewState["dtInvoice"];
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataTable dtrow = dt.Select("ICH_PK = " + ddlCIPrint.SelectedValue).CopyToDataTable();
                                int Type = Convert.ToInt32(dtrow.Rows[0]["ICH_TYPE"].ToString());
                                int TermType = Convert.ToInt32(dtrow.Rows[0]["ICH_DEL_TERM_VALUE"].ToString());

                                reportgenrated = true;

                                if (IsInvoiceTerms == true)
                                {
                                    if (Type.ToString() == "2")
                                    {

                                        if (TermType.ToString() == "0")
                                        {
                                            if (IsExportExcel)
                                            {
                                                reportgenrated = false;
                                                Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + Type);
                                            }
                                            else
                                            {
                                                reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + Type);
                                            }

                                        }
                                        else
                                        {
                                            if (IsExportExcel)
                                            {
                                                reportgenrated = false;
                                                Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + TermType);
                                            }
                                            else
                                            {
                                                reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + TermType);
                                            }
                                        }

                                    }

                                    else
                                    {
                                        if (IsExportExcel)
                                        {
                                            reportgenrated = false;
                                            Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + Type);
                                        }
                                        else
                                        {
                                            reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + Type);
                                        }

                                    }

                                }
                                else
                                {
                                    if (IsExportExcel)
                                    {
                                        reportgenrated = false;
                                        Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + Type);
                                    }
                                    else
                                    {
                                        reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + Type);
                                    }
                                }
                            }
                            //string Type = ddlCIPrint.SelectedItem.Text;
                            //if (Type.Split('/')[0] == ApplicationType.CI)
                            //{
                            //    reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=2");
                            //}
                            //else
                            //{
                            //    reoprturl = Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ddlCIPrint.SelectedValue + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=1");
                            //}
                        }
                        if (reportgenrated)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + reoprturl + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);
                            string msg = Resources.Messages.Err_CommericalInvoice.ToString();
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
            SODETAILS,
            ADMAPPSUBTYPES,
            SPHDR,
            SAORDER,
            COMMERICALINVOICE,
            CONTAINERINSPECTION

        }
        #endregion
    }
}