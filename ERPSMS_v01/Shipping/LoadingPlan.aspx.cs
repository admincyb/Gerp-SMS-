using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ERPManager;
using BusinessObject.Common;
using ERP.Utilities;
using System.Web.UI.HtmlControls;
using System.Data;
using BusinessLogic.Shipping;
using BusinessObject.Shipping;
using BusinessObject.CommonManagement;
using System.Xml;
using BusinessObject.AccountManagement;
using ERPService;
using ERPData;
using System.Threading;
using BusinessObject.Sales;


namespace ERPSMS_v01.Shipping
{
    public partial class LoadingPlan : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
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
        /// Shipping Plan PK
        /// </summary>
        private int ShippingPlanPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK] = value;
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
        private bool LoadPlanSCDetails
        {
            get
            {
                return Convert.ToBoolean(Convert.ToInt32(this.ViewState["LoadPlanSCDetails"]));
            }
            set
            {
                this.ViewState["LoadPlanSCDetails"] = value;
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

        #endregion

        #region Variables

        private List<ADM_COMPANY_MST> admCompanyMstList;
        private ADM_COMPANY_MST admCompanyMstObj;

        //Object for service utility
        private ServiceUtility serviceUtilityObj;

        // Indicates the state as well as action
        private ActionsEnum commonActions;

        //workflow variables
        private string refID;
        private string inboxFlag;

        //DataSet for binding details to controls 
        public DataSet dsLoadingPlan
        {
            get
            {
                return (DataSet)this.ViewState[ViewstateStrings.dsLoadingPlan];
            }
            set
            {
                this.ViewState[ViewstateStrings.dsLoadingPlan] = value;
            }
        }


        private DataSet dsShippingPlanHDR;
        private DataSet dsSPDetails;
        private DataTable dtSCDetails;
        private DataTable dtBrandDetails;


        //for loading plan PK
        private static int lpdID;

        //saving and workflow loading plan details
        LoadingPlanBO loadingPlanBOObj;
        XmlDocument xmlDoc;

        //For User details
        private BusinessObject.User currentUser;
        private int tabLevel;
        private int prevCompany = 0;
        private int soPk;
        private int brandPk;
        private int sodPk;
        private SaleContractBO saleOrderHeaderObj;

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
            try
            {
                //for workflow integration
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    LoadPlanSCDetails = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "LoadPlanSCDetails").ToString()));
                    ConfigurationSettings();
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

                    //txtBrand.Enabled = false;

                    //set number format
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();

                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    //Set Weight Format
                    hdfWeightDecimalDigit.Value = "#0.";
                    int WeightDecimalDigit = (Session[ERP.Utilities.SessionStrings.WeightDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.WeightDecimalDigit]));
                    for (int i = 0; i < WeightDecimalDigit; i++)
                    {
                        hdfWeightDecimalDigit.Value += "0";
                    }

                    //for Loading plan no
                    AST_DOC_MODE.Value = ((int)DOCMODE.Submit).ToString();

                    //Set Shipping Plan Pk
                    ShippingPlanPK = Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] != null ? (int)Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] : 0;

                    txtGeneratedOn.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);

                    //Used for Integration purpose
                    FillProcessID();
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                       : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    EntryStatus = EntryStatus.ENTRYMODE;
                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        ReferanceID = int.Parse(refID);
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                            //btnSave.Visible = false;
                            //btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(refID);
                        Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = ShippingPlanPK = GetApplicationID(ucrWrkf.RefID);
                    }

                    //if ShippingPlan Pk >0 then create the Loading Plan  otherwise redirect to SP listing 
                    if (ShippingPlanPK > 0)
                    {
                        hdfShippingPlanPK.Value = ShippingPlanPK.ToString();
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                        }

                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        GetFieldValues(ControlsEnum.SHIPPINGPLANLEVEL);
                        SetFieldValues(ControlsEnum.SHIPPINGPLANLEVEL);
                        GetFieldValues(ControlsEnum.SPDEATILS);
                        SetFieldValues(ControlsEnum.SPDEATILS);

                    }
                    else
                    {
                        Response.Redirect(Resources.PageURL.ShippingPlan);
                    }
                    //set the tab for user
                    SetTabVisibility();
                    //for Loading plan no
                    AST_DOC_MODE.Value = GetDOCMODE();
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
            AdmCompanyMstService admCompanyMstServiceClient;

            //get the user details
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {
                switch (type)
                {
                    #region DEFAULT
                    case ControlsEnum.DEFAULT:
                        //get the loading plan details
                        dsLoadingPlan = new DataSet();
                        dsLoadingPlan = LoadingPlanBL.GetLoadingPlan(ShippingPlanPK, currentUser.SBUID, 1);
                        //To get prev transaction company
                        if (CurrPK == 0)
                        {
                            prevCompany = BusinessLogic.Shipping.ShippingPlanBL.GetPrevCompany(ShippingPlanPK);
                        }
                        break;
                    #endregion

                    #region SHIPPINGPLAN HEADR
                    case ControlsEnum.SHIPPINGPLANLEVEL:
                        //get the shipping plan header
                        dsShippingPlanHDR = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanHDR(currentUser, ShippingPlanPK, Convert.ToInt32(CommonConstants.ACTIVE));
                        break;
                    #endregion

                    #region SHIPPINGPLAN DETAILS
                    case ControlsEnum.SPDEATILS:
                        //get the shipping plan details
                        dsSPDetails = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanDetails(0, ShippingPlanPK, Convert.ToInt32(CommonConstants.ACTIVE));
                        break;
                    #endregion

                    #region Company
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        break;
                    #endregion

                    #region SC DETAILS BY LOAD PLAN PK
                    case ControlsEnum.SCDETAILS:
                        dtSCDetails = LoadingPlanBL.GetSCDetails(ShippingPlanPK, Convert.ToInt32(CommonConstants.HASPK), currentUser.SBUID);
                        //dtSCDetails = LoadingPlanBL.GetSCDetails(CurrPK, Convert.ToInt32(CommonConstants.HASPK), currentUser.SBUID);
                        break;
                    #endregion
                    #region SC DETAILS BY PK
                    case ControlsEnum.SODETAILS:
                        saleOrderHeaderObj = BusinessLogic.Sales.SaleOrderBL.GetSaleContractHeader(0, soPk);
                        break;
                    #endregion
                    #region BRANDSELECTED
                    case ControlsEnum.BRANDSELECTED:
                        dtBrandDetails = LoadingPlanBL.GetBrandDetails(soPk, sodPk, Convert.ToInt32(hdfShippingPlanPK.Value), Convert.ToInt32(CommonConstants.ACTIVE), currentUser.SBUID);
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
                    #region DEFAULT
                    case ControlsEnum.DEFAULT:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion

                    #region SHIPPINGPLAN HEADR
                    case ControlsEnum.SHIPPINGPLANLEVEL:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion

                    #region SHIPPINGPLAN DETAILS
                    case ControlsEnum.SPDEATILS:
                        BindDropDown(controlType);
                        break;
                    #endregion

                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANY);
                        break;

                    #region SC DETAILS
                    case ControlsEnum.SCDETAILS:
                        BindDropDown(ControlsEnum.SCDETAILS);
                        break;
                    #endregion
                    #region BRANDSELECTED
                    case ControlsEnum.BRANDSELECTED:
                        GetUIValuesFromObject(controlType);
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
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.SPDEATILS:
                        //ddlLotNo.Items.Clear();
                        //ddlLotNo.SelectedIndex = -1;
                        //ddlLotNo.SelectedValue = null;
                        //ddlLotNo.ClearSelection();
                        //if (dsSPDetails != null && dsSPDetails.Tables[0].Rows.Count > 0)
                        //{
                        //    ddlLotNo.DataSource = CommonFunctions.HtmlDecodeDataTable(dsSPDetails.Tables[0].DefaultView.ToTable(true, Resources.DataFieldRes.SODLotNo), Resources.DataFieldRes.SODLotNo);
                        //    ddlLotNo.DataTextField = Resources.DataFieldRes.SODLotNo;
                        //    ddlLotNo.DataValueField = Resources.DataFieldRes.SODLotNo;
                        //    ddlLotNo.DataBind();

                        //}
                        //ddlLotNo.Items.Insert(0, new ListItem(Resources.ErpRes.Select, ERP.Utilities.CommonConstants.SELECTVAL));
                        break;

                    #region COMPANY
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
                    #endregion
                    #region SC DETAILS
                    case ControlsEnum.SCDETAILS:
                        ddlSCNoDtls.Items.Clear();
                        ddlSCNoDtls.SelectedIndex = -1;
                        ddlSCNoDtls.SelectedValue = null;
                        ddlSCNoDtls.ClearSelection();
                        if (dtSCDetails != null && dtSCDetails.Rows.Count > 0)
                        {
                            ddlSCNoDtls.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSCDetails, Resources.DataFieldRes.SONo);
                            ddlSCNoDtls.DataTextField = Resources.DataFieldRes.SONo;
                            ddlSCNoDtls.DataValueField = Resources.DataFieldRes.SaleOrderPK;
                            ddlSCNoDtls.DataBind();

                        }
                        ddlSCNoDtls.Items.Insert(0, new ListItem(Resources.ErpRes.Select, ERP.Utilities.CommonConstants.SELECTVAL));
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
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private object SetUIValuesToObject(ControlsEnum controlType)
        {
            object returnObj;
            returnObj = null;
            Label lblGLotNo;

            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region Save Details
                    case ControlsEnum.WRKFSUBMIT:
                    case ControlsEnum.SAVE:
                        loadingPlanBOObj = new LoadingPlanBO();
                        loadingPlanBOObj.ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        loadingPlanBOObj.LAST_MOD_DT = LastModifiedTime.ToString();
                        loadingPlanBOObj.USER_PK = currentUser.PKUser.ToString();
                        loadingPlanBOObj.BIZUNIT_PK = currentUser.SBUID;
                        loadingPlanBOObj.LPH_SHIPPING_PLAN = ShippingPlanPK;
                        if (txtGeneratedOn.Text != string.Empty)
                            loadingPlanBOObj.LPH_DATE = Convert.ToDateTime(txtGeneratedOn.Text).ToString();
                        loadingPlanBOObj.LPH_DEPT = currentUser.CurrentDeptPK.ToString();
                        loadingPlanBOObj.LPH_DESC = string.Empty;
                        loadingPlanBOObj.LPH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        // if (CurrPK>0)
                        loadingPlanBOObj.LPH_PK = CurrPK.ToString();
                        loadingPlanBOObj.APT_CODE = ApplicationType.SPLN;
                        loadingPlanBOObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                        loadingPlanBOObj.WKF_FLAG = controlType == ControlsEnum.SAVE ? 0 : 1;
                        loadingPlanBOObj.WKF_PROCESS = Convert.ToInt32(hdfProcessID.Value);
                        

                        loadingPlanBOObj.DetailsList = new List<DetailsBO>();
                        List<DetailsBO> detailsList = new List<DetailsBO>();
                        DetailsBO objDetail;

                        foreach (GridViewRow inRow in grdLoadingPlanDet.Rows)
                        {
                            string[] strLoaded;
                            string[] strRow;
                            string[] strCarton;
                            strLoaded = (inRow.FindControl("lblLoaded") as Label).ToolTip.Split('x');
                            strRow = (inRow.FindControl("hdfRow") as HiddenField).Value.Split('-');
                            strCarton = (inRow.FindControl("hdfCarton") as HiddenField).Value.Split('-');
                            objDetail = new DetailsBO();
                            objDetail.LPD_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);// Convert.ToInt32((inRow.FindControl("hdfCustomerItem") as HiddenField).Value);
                            objDetail.LPD_MOD_BY = currentUser.PKUser;// Convert.ToInt32((inRow.FindControl("ddlCurrency") as DropDownList).SelectedValue);
                            objDetail.LPD_MOD_DT = DateTime.Now.ToString();
                            //if ((inRow.FindControl("hdfLPDPK") as HiddenField).Value != string.Empty)
                            objDetail.LPD_PK = (inRow.FindControl("hdfLPDPK") as HiddenField).Value == string.Empty ? "0" : (inRow.FindControl("hdfLPDPK") as HiddenField).Value;
                            objDetail.LPD_QTY = Convert.ToDouble((inRow.FindControl("lblQuantity") as Label).Text.Replace(",", ""));
                            objDetail.LPD_QTY_X = Convert.ToDouble(strLoaded[0]);
                            objDetail.LPD_QTY_Y = Convert.ToDouble(strLoaded[1]);
                            int RowFrom = 1, RowTo = 1;
                            Int32.TryParse(strRow[0], out RowFrom);
                            objDetail.LPD_ROW_FROM = RowFrom;
                            lblGLotNo = (inRow.FindControl("lblGLotNo") as Label);
                            objDetail.LPD_LOT_NO = lblGLotNo != null ? lblGLotNo.Text != "Select/Type" ? lblGLotNo.Text : string.Empty : string.Empty;
                            if (strRow.Length == 2 && strRow[1].Trim() != string.Empty)
                            {
                                Int32.TryParse(strRow[1], out RowTo);
                                objDetail.LPD_ROW_TO = RowTo;
                            }
                            objDetail.LPD_TYPE = (inRow.FindControl("lblType") as Label).ToolTip;
                            objDetail.LPD_CARTON_FROM = strCarton[0];
                            objDetail.LPD_CARTON_TO = strCarton[1];

                            //For BWH
                            if (LoadPlanSCDetails)
                            {
                                if (!string.IsNullOrEmpty((inRow.FindControl("hdfSCNo") as HiddenField).Value) && (inRow.FindControl("hdfSCNo") as HiddenField).Value != "0")
                                    objDetail.LPD_SO_HDR = (inRow.FindControl("hdfSCNo") as HiddenField).Value == string.Empty ? "0" : (inRow.FindControl("hdfSCNo") as HiddenField).Value;
                                if (!string.IsNullOrEmpty((inRow.FindControl("hdfSodPk") as HiddenField).Value) && (inRow.FindControl("hdfSodPk") as HiddenField).Value != "0")
                                    objDetail.LPH_SO_DTL = (inRow.FindControl("hdfSodPk") as HiddenField).Value == string.Empty ? "0" : (inRow.FindControl("hdfSodPk") as HiddenField).Value;
                                objDetail.LPH_SC_DATE = (inRow.FindControl("lblSCDate") as Label).Text;
                                if (!string.IsNullOrEmpty((inRow.FindControl("hdfBrand") as HiddenField).Value) && (inRow.FindControl("hdfBrand") as HiddenField).Value != "0")
                                    objDetail.LPH_BRAND = (inRow.FindControl("hdfBrand") as HiddenField).Value;
                                objDetail.LPH_BRAND_NAME = (inRow.FindControl("lblBrand") as Label).Text;
                                objDetail.LPH_IR_RADIATION_LOT_NO = (inRow.FindControl("lblIrradiationLotNo") as Label).ToolTip;
                                objDetail.LPH_EXP_DATE = (inRow.FindControl("lblExpDate") as Label).Text;
                                objDetail.LPH_NET_WT = !string.IsNullOrEmpty((inRow.FindControl("lblNetWtBox") as Label).Text) ? Convert.ToDouble((inRow.FindControl("lblNetWtBox") as Label).Text).ToString() : "0";
                                objDetail.LPH_CTN_NET_WT = !string.IsNullOrEmpty((inRow.FindControl("lblNetWtCarton") as Label).Text) ? Convert.ToDouble((inRow.FindControl("lblNetWtCarton") as Label).Text).ToString() : "0";
                                objDetail.LPH_GROSS_WT = !string.IsNullOrEmpty((inRow.FindControl("lblGrossWtBox") as Label).Text) ? Convert.ToDouble((inRow.FindControl("lblGrossWtBox") as Label).Text).ToString() : "0";
                                objDetail.LPH_CTN_GROSS_WT = !string.IsNullOrEmpty((inRow.FindControl("lblGrossWtCarton") as Label).Text) ? Convert.ToDouble((inRow.FindControl("lblGrossWtCarton") as Label).Text).ToString() : "0";
                            }
                            detailsList.Add(objDetail);
                        }

                        loadingPlanBOObj.DetailsList = detailsList;
                        returnObj = loadingPlanBOObj;

                        break;
                    #endregion
                }
                return returnObj;
            }
            catch
            {
                throw;
            }
            finally
            {

            }
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
                    #region DEFAULT
                    case ControlsEnum.DEFAULT:
                        if (dsLoadingPlan != null && dsLoadingPlan.Tables[0].Rows.Count > 0)
                        {

                            CurrPK = Convert.ToString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_PK]) == string.Empty ? 0 : Convert.ToInt32(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_PK]);
                            if (CurrPK != 0)
                            {
                                hdfLoadPlanPk.Value = CurrPK.ToString();
                            }
                            lblContainerNoValue.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_CONTAINER_NO].ToString(), 20);
                            lblContainerNoValue.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_CONTAINER_NO].ToString(), 300);
                            lblContainerTypeValue.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_CONTAINER_TYPE_TEXT].ToString(), 300);
                            lblContainerTypeValue.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_CONTAINER_TYPE_TEXT].ToString(), 300);

                            //lblDestinationPortValue.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_SHIP_TO_PORT].ToString();
                            //lblDestinationPortValue.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_SHIP_TO_PORT].ToString();
                            //lblInTimeValue.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString() != string.Empty ? Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString()).ToString(Resources.Constants.DateTimeFormat) : string.Empty;
                            //lblInTimeValue.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString() != string.Empty ? Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString()).ToString(Resources.Constants.DateTimeFormat) : string.Empty;

                            lblCustomerText.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUS_TEXT].ToString(), 25);
                            lblCustomerText.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUS_TEXT].ToString(), 300);

                            //lblSCNoText.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SC_NO].ToString(), 20);
                            //lblSCNoText.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SC_NO].ToString();
                            //lblSCDateText.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SC_DATE].ToString(), 20);
                            //lblSCDateText.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SC_DATE].ToString();
                            lblSCNoText.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_NO].ToString(), 20);
                            lblSCNoText.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_NO].ToString();

                            lblSCDateText.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString()).ToString(Resources.Constants.DateFormatShort), 20) : string.Empty;
                            lblSCDateText.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString() != string.Empty ? Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString()).ToString(Resources.Constants.DateFormatShort) : string.Empty;

                            //ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_COMPANY].ToString())));


                            txtPlanNo.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_NO].ToString();
                            txtGeneratedOn.Text = Convert.ToString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_DATE]) != string.Empty ? Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_DATE]).ToString(Resources.Constants.DateFormatShort) : DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);
                            if (Convert.ToString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_MOD_DT]) != string.Empty)
                                LastModifiedTime = Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_MOD_DT]);
                            if (txtPlanNo.Text.Trim() == string.Empty)
                            {
                                txtPlanNo.Text = Resources.Messages.DocGenerationNew;
                            }
                            //SC NO Details - Shows Only for BWH
                            if (LoadPlanSCDetails)
                            {
                                GetFieldValues(ControlsEnum.SCDETAILS);
                                SetFieldValues(ControlsEnum.SCDETAILS);
                            }

                            txtLotNO.Text = dsLoadingPlan.Tables[0].Rows[0][Resources.DataFieldRes.LoadingPlanLotNo].ToString();
                            //ddlLotNo.SelectedIndex = Convert.ToInt32(ddlLotNo.Items.IndexOf(ddlLotNo.Items.FindByValue(dsLoadingPlan.Tables[0].Rows[0][Resources.DataFieldRes.LoadingPlanLotNo].ToString())));
                            if (CurrPK != 0)
                            {
                                ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dsLoadingPlan.Tables[0].Rows[0]["LPH_COMPANY"].ToString())));
                            }
                            else
                            {
                                if (prevCompany != null && prevCompany != 0)
                                {
                                    ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(prevCompany.ToString())));
                                }
                            }

                            hdfDelstatus.Value = dsLoadingPlan.Tables[0].Rows[0][Resources.DataFieldRes.SPDeleteStatus].ToString();

                            BindGrid();

                        }
                        break;
                    #endregion
                    case ControlsEnum.SHIPPINGPLANLEVEL:
                        if (dsShippingPlanHDR != null && dsShippingPlanHDR.Tables[0].Rows.Count > 0)
                        {
                            hdfQty.Value = dsShippingPlanHDR.Tables[0].Rows[0]["SNH_CTN_QTY"].ToString();
                            //lblPlanQtytext.Text =lblPlanQtytext.ToolTip = String.Format("{0:n}", Convert.ToDecimal(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_CTN_QTY"].ToString()));
                            lblPlanQtytext.Text = lblPlanQtytext.ToolTip = dsShippingPlanHDR.Tables[0].Rows[0]["SNH_CTN_QTY"].ToString();
                        }
                        break;
                    #region BRANDSELECTED
                    case ControlsEnum.BRANDSELECTED:
                        //txtLotNO.Text = string.Empty;
                        txtLotNO.Text = dtBrandDetails.Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CIM_LOT_TEXT].ToString();
                        if (dtBrandDetails != null && dtBrandDetails.Rows.Count > 0)
                        {
                            double nb, gb, nc, gc = 0.000;
                            hdfBrand.Value = dtBrandDetails.Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CIM_PK].ToString();
                            hdfShpPlanQty.Value = dtBrandDetails.Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SND_PLAN_QTY].ToString();
                            hdfLoadPlanSodPk.Value = dtBrandDetails.Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SOD_PK].ToString();
                            hdfLoadPlanSodPk1.Value = dtBrandDetails.Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SOD_PK].ToString();
                            if (double.TryParse(dtBrandDetails.Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CIM_BOX_NET_WT].ToString(), out nb))
                                txtNetWtBox.Text = GetFormattedWeight(nb).ToString();
                            if (double.TryParse(dtBrandDetails.Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CIM_CRTN_NET_WT].ToString(), out nc))
                                txtNetWtCarton.Text = GetFormattedWeight(nc).ToString();
                            if (double.TryParse(dtBrandDetails.Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CIM_BOX_GROSS_WT].ToString(), out gb))
                                txtGrossWtBox.Text = GetFormattedWeight(gb).ToString();
                            if (double.TryParse(dtBrandDetails.Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CIM_CRTN_GROSS_WT].ToString(), out gc))
                                txtGrossWtCarton.Text = GetFormattedWeight(gc).ToString();
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

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid()
        {
            try
            {
                if (dsLoadingPlan.Tables[0].Rows.Count == 1)
                {
                    if (Convert.ToString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY]) != string.Empty && Convert.ToString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_FROM]) != string.Empty)
                    {
                        grdLoadingPlanDet.DataSource = dsLoadingPlan.Tables[0];
                        grdLoadingPlanDet.DataBind();
                    }
                    else
                    {
                        grdLoadingPlanDet.DataSource = null;
                        grdLoadingPlanDet.DataBind();
                    }
                }
                else
                {
                    grdLoadingPlanDet.DataSource = dsLoadingPlan.Tables[0];
                    grdLoadingPlanDet.DataBind();
                }
                lpdID = 0;
                txtQty.Text = string.Empty;
                txtLoaded.Text = string.Empty;
                txtLoaded2.Text = string.Empty;
                txtRowFrom.Text = string.Empty;
                txtRowTo.Text = string.Empty;
                txtType.Text = string.Empty;
                txtCartonFrom.Text = string.Empty;
                txtCartonTo.Text = string.Empty;
                txtLotNO.Text = string.Empty;
                //ddlLotNo.SelectedValue = CommonConstants.SELECTVAL;
                hdfSLNo.Value = ERP.Utilities.Constants.Shipping.LoadingPlan.VALUE_ZERO;

                //For BWH
                if (LoadPlanSCDetails)
                {
                    txtSCDtlsDate.Text = string.Empty;
                    txtIrradiationLotNo.Text = string.Empty;
                    txtExpDate.Text = string.Empty;
                    txtNetWtBox.Text = string.Empty;
                    txtGrossWtBox.Text = string.Empty;
                    txtNetWtCarton.Text = string.Empty;
                    txtGrossWtCarton.Text = string.Empty;
                    ddlSCNoDtls.SelectedIndex = 0;
                    hdfBrand.Value = ERP.Utilities.Constants.Shipping.LoadingPlan.VALUE_ZERO;
                    hdfSCNoDtls.Value = ERP.Utilities.Constants.Shipping.LoadingPlan.VALUE_ZERO;
                    txtBrand.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Method for check Shpping Plan CTN qty and Total Loading Plan CTN Qty
        /// </summary>
        private bool CheckCTNQty()
        {
            //For no need of checking
            if ((hdfAlert.Value == "1") || (hdfAlert.Value == "5"))
            {
                return true;
            }
            decimal ShippingCTNQty = Convert.ToDecimal(hdfQty.Value);
            decimal CtnQty = 0;
            //get the total LoadingPlan CTN QTy
            if (grdLoadingPlanDet.FooterRow != null)
            {
                Label lblTotQty = (Label)grdLoadingPlanDet.FooterRow.FindControl("lblTotQty");
                string x = lblTotQty.Text.Trim().Replace(",", "");
                CtnQty = Convert.ToDecimal(x);
            }
            if (ShippingCTNQty != CtnQty)
                return false;
            else
                return true;

        }
        /// <summary>
        /// Method for check Shpping Plan CTN qty and Line Item Loading Plan CTN Qty
        /// </summary>
        /// 
        private bool LineItemQtyCheck()
        {
            bool ret = true;
            decimal sphPlanQty = 0; decimal brandQty = 0;
            decimal ot;
            foreach (GridViewRow inRow in grdLoadingPlanDet.Rows)
            {
                if ((inRow.FindControl("hdfSodPk1") as HiddenField).Value == hdfLoadPlanSodPk1.Value)
                {
                    if ((inRow.FindControl("hdfRowNo") as HiddenField).Value != hdfSLNo.Value)
                    {
                        ot = 0;
                        if (decimal.TryParse((inRow.FindControl("lblQuantity") as Label).Text, out ot))
                            sphPlanQty += ot;
                    }
                }
            }
            ot = 0;
            if (decimal.TryParse(hdfShpPlanQty.Value, out ot))
                brandQty = ot;
            ot = 0;
            if (decimal.TryParse(txtQty.Text, out ot))
                sphPlanQty += ot;
            if ((sphPlanQty > brandQty) && (hdfShpPlanQtyValid.Value == "0"))
                ret = false;
            else
                ret = true;
            return ret;
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {

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
            //txtContainerNo.Text = string.Empty;
            //txtDestinationPort.Text = string.Empty;
            txtGeneratedOn.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);

            //txtInTime.Text = string.Empty;
            txtLoaded.Text = string.Empty;
            txtLoaded2.Text = string.Empty;
            txtPlanNo.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtRowFrom.Text = string.Empty;
            txtRowTo.Text = string.Empty;
            //txtSealNo.Text = string.Empty;
            txtType.Text = string.Empty;

            ModifiedDatePnl.Visible = false;

        }

        //private bool CheckQty()
        //{
        //    bool isvalid = true;
        //    double ctnqty = txtQty.Text.Trim() != string.Empty ? Convert.ToDouble(txtQty.Text.Trim()) : 0.0;
        //    double planqty;
        //    GetFieldValues(ControlsEnum.SHIPPINGPLANLEVEL);
        //    if (dsShippingPlanHDR != null && dsShippingPlanHDR.Tables[0].Rows.Count > 0)
        //    {
        //        planqty = Convert.ToDouble(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_PLAN_QTY"]);
        //        if (planqty >= ctnqty)
        //            isvalid = true;
        //        else
        //            isvalid = false;
        //    }
        //    return isvalid;
        //}

        /// <summary>
        /// To validate auto complete hidden fields
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            try
            {
                bool flag;
                int parsed;
                string errMsg;
                flag = true;
                errMsg = string.Empty;



                if (grdLoadingPlanDet.Rows.Count < 1)
                {
                    errMsg = errMsg + "^" + this.GetLocalResourceObject("Err_LoadingDet").ToString();
                    flag = false;
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
        /// Method used to Total of Grid Coloumn
        /// </summary>
        public string GetColumnTotal(object evalString)
        {
            string strOut;
            strOut = string.Empty;
            //if (!evalString.Equals(DBNull.Value))
            strOut = CommonFunctions.GetColumnTotal(ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY, dsLoadingPlan.Tables[0]).ToString();// String.Format("{0:n}", );
            //return strOut;
            return AddCommas(strOut);
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            CommonService commonServiceObj;
            List<SPADM_APP_SUB_TYPE_DATA_GET_Result> appTypeDetailsList;
            commonServiceObj = new CommonService();
            appTypeDetailsList = commonServiceObj.GetReportParameters(ApplicationType.SPLN, 0, DateTime.Now);
            if (appTypeDetailsList.Count > 0)
            {
                return appTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// Set Tab Visibility
        /// </summary>
        private void SetTabVisibility()
        {
            GetFieldValues(ControlsEnum.SHIPPINGPLANLEVEL);
            if (dsShippingPlanHDR != null && dsShippingPlanHDR.Tables.Count > 0 && dsShippingPlanHDR.Tables[0].Rows.Count > 0)
            {
                tabLevel = Convert.ToInt32(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_TRX_STATUS"]);
            }
            //spnShippingPlan.Visible = lnkShippingPlan.Visible = tabLevel >= (int)ShippingTabsEnum.ShippingPlan;
            spnContainerEval.Visible = lnkContainerEval.Visible = tabLevel >= (int)ShippingTabsEnum.PaymentCleared;
            spnContainerInspection.Visible = lnkContainerInspection.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerEvaluated;
            spnUploadQADocs.Visible = lnkUploadQADocs.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerInspected;
            spnUploadExportDocs.Visible = lnkUploadExportDocs.Visible = tabLevel >= (int)ShippingTabsEnum.QADocsUploaded;
            spnLoadingPlan.Visible = lnkLoadingPlan.Visible = tabLevel >= (int)ShippingTabsEnum.ExportDocsUploaded;
            spnUploadPhotographs.Visible = lnkUploadPhotographs.Visible = tabLevel >= (int)ShippingTabsEnum.LoadingPlanCompleted;
            spnDeliveryOrder.Visible = lnkDeliveryOrder.Visible = tabLevel >= (int)ShippingTabsEnum.PhotographsUploaded;
            spnContainerRelease.Visible = lnkContainerRelease.Visible = tabLevel >= (int)ShippingTabsEnum.DeliveryOrderCompleted;
            spnBillofLoading.Visible = lnkBillofLoading.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerReleased;
        }
        /// <summary>
        /// Set Configuration settings
        /// </summary>
        private void ConfigurationSettings()
        {
            #region SC Details
            //Set Visibility of pnlSCDetails. Show Only for BWH    
            pnlSCDetails.Visible = LoadPlanSCDetails;
            pnlSCWtDetails.Visible = LoadPlanSCDetails;
            vrfBrand.Enabled = LoadPlanSCDetails;
            hdfLotNobyBrand.Value = "0";
            if (LoadPlanSCDetails == true)
            {
                hdfLotNobyBrand.Value = "1";
            }
            if (!LoadPlanSCDetails)
                grdLoadingPlanDet.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            #endregion
        }
        public string GetFormattedWeight(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfWeightDecimalDigit.Value);
        }

        public string AddCommas(object number)
        {
            int curGroup1 = 3;//First seperation after how many digits
            int curGroup2 = 3;//Remaining seperations after how many digits           
            int.TryParse(hdfCurrencyGroup1.Value, out curGroup1);
            int.TryParse(hdfCurrencyGroup2.Value, out curGroup2);
            return CommonFunctions.AddCommaSeperations(number, curGroup1, curGroup2);
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
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                int result;
                result = 0;
                DropDownList ddlWkfAction;
                string action;
                string soPK;

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlSCNoDtls")
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }
                }
                switch (commonActions)
                {
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        //else if (!CheckQty())
                        //{
                        //    litErrorMsg.Text = GetLocalResourceObject("QtyError").ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                        //}
                        else//valid
                        {
                            //check shpping plan CTN Qty and Loading Plan CTN Qty equal
                            if (CheckCTNQty())
                            {

                                loadingPlanBOObj = (LoadingPlanBO)SetUIValuesToObject(ControlsEnum.SAVE);
                                if (loadingPlanBOObj != null)
                                {
                                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(loadingPlanBOObj);
                                    result = LoadingPlanBL.SaveLoadingPlan(xmlDoc.InnerXml);
                                    if (result >= 0) // Success ! re-initialize the page
                                    {
                                        //ResetForm();
                                        GetFieldValues(ControlsEnum.DEFAULT);
                                        SetFieldValues(ControlsEnum.DEFAULT);

                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LoadingPlan);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else//fail
                                    {
                                        if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.LoadingPlan + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.LoadingPlan + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.ALREADYCREATED)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.LoadingPlan + " " + GetLocalResourceObject("AlreadyCreated").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMessage", "$(document).ready(function(){ShowAlertConfirm(1);});", true);
                            }
                        }
                        break;
                    #endregion

                    #region Cancel
                    // Do Action for , when click Cancel button
                    case ActionsEnum.CANCEL:
                        //ResetForm();
                        //Response.Redirect(Resources.PageURL.ShippingPlan);
                        break;
                    #endregion

                    #region ADD_ACTION
                    // Do Action for , when click AddtoList button
                    case ActionsEnum.ADD_ACTION:
                        DataRow dr;
                        bool perRet = true;
                        //For BWH 
                        if (LoadPlanSCDetails)
                        {
                            if (!LineItemQtyCheck() && hdfShpPlanQtyValid.Value == "0")
                            {
                                hdfShpPlanQtyValid.Value = "1";
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMessage", "$(document).ready(function(){ShowLineItemAlertConfirm(1);});", true);
                                perRet = false;
                                break;
                            }
                            hdfShpPlanQtyValid.Value = "0";
                        }

                        //For new loading plan details
                        if (hdfSLNo.Value == ERP.Utilities.Constants.Shipping.LoadingPlan.VALUE_ZERO && dsLoadingPlan.Tables[0].Rows.Count >= 1 && Convert.ToString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY]) != string.Empty)
                        {
                            if (dsLoadingPlan == null)
                                dsLoadingPlan = new DataSet();
                            dr = dsLoadingPlan.Tables[0].NewRow();
                            dr[ERP.Utilities.Constants.Shipping.LoadingPlan.ROW_NO] = dsLoadingPlan.Tables[0] == null ? 1 : dsLoadingPlan.Tables[0].Rows.Count + 1;
                            dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY] = txtQty.Text;
                            dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_X] = txtLoaded.Text;
                            dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_Y] = txtLoaded2.Text;
                            dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_FROM] = txtRowFrom.Text;
                            if (txtRowTo.Text.Trim() != string.Empty)
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_TO] = txtRowTo.Text.Trim();
                            else
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_TO] = "0";
                            if (txtType.Text.Trim() != string.Empty)
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_TYPE] = txtType.Text.Trim();
                            dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_CARTON_FROM] = txtCartonFrom.Text;
                            if (txtCartonTo.Text.Trim() != string.Empty)
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_CARTON_TO] = txtCartonTo.Text.Trim();
                            else
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_CARTON_TO] = "0";
                            dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_NO] = txtPlanNo.Text;
                            dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_DATE] = txtGeneratedOn.Text;
                            dr[Resources.DataFieldRes.LoadingPlanLotNo] = txtLotNO.Text == "Select/Type" ? string.Empty : txtLotNO.Text;
                            //dr[Resources.DataFieldRes.LoadingPlanLotNo] = ddlLotNo.SelectedValue != ERP.Utilities.CommonConstants.SELECTVAL ? ddlLotNo.SelectedValue : string.Empty;

                            //For BWH 
                            if (LoadPlanSCDetails)
                            {
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SOH_NO] = ddlSCNoDtls.SelectedIndex != 0 ? ddlSCNoDtls.SelectedItem.Text : string.Empty;
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SO_HDR] = ddlSCNoDtls.SelectedIndex != 0 ? ddlSCNoDtls.SelectedItem.Value : string.Empty;
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SO_DTL] = !string.IsNullOrEmpty(hdfLoadPlanSodPk.Value) ? hdfLoadPlanSodPk.Value != "0" ? hdfLoadPlanSodPk.Value : "0" : "0";
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SC_DATE] = txtSCDtlsDate.Text != string.Empty ? Convert.ToDateTime(txtSCDtlsDate.Text).ToString() : string.Empty;
                                if (string.IsNullOrEmpty(hdfBrand.Value) || hdfBrand.Value == "0")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Brand").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    perRet = false;
                                    break;
                                }
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_BRAND] = !string.IsNullOrEmpty(hdfBrand.Value) ? hdfBrand.Value != "0" ? hdfBrand.Value : "0" : "0";
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_BRAND_NAME] = txtBrand.Text == "Select/Type" ? string.Empty : txtBrand.Text;
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.SND_PLAN_QTY] = !string.IsNullOrEmpty(hdfShpPlanQty.Value) ? hdfShpPlanQty.Value : "0";
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_IR_RADIATION_LOT_NO] = txtIrradiationLotNo.Text == "Select/Type" ? string.Empty : txtIrradiationLotNo.Text;
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_EXP_DATE] = txtExpDate.Text;
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_NET_WT] = !string.IsNullOrEmpty(txtNetWtBox.Text) ? txtNetWtBox.Text : "0";
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_CTN_NET_WT] = !string.IsNullOrEmpty(txtNetWtCarton.Text) ? txtNetWtCarton.Text : "0";
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_GROSS_WT] = !string.IsNullOrEmpty(txtGrossWtBox.Text) ? txtGrossWtBox.Text : "0";
                                dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_CTN_GROSS_WT] = !string.IsNullOrEmpty(txtGrossWtCarton.Text) ? txtGrossWtCarton.Text : "0";
                            }
                            dsLoadingPlan.Tables[0].Rows.Add(dr);
                            dsLoadingPlan.AcceptChanges();

                        }
                        //For edit loading plan details
                        else if (hdfSLNo.Value == ERP.Utilities.Constants.Shipping.LoadingPlan.VALUE_ZERO && dsLoadingPlan.Tables[0].Rows.Count == 1 && Convert.ToString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY]) == string.Empty)
                        {
                            int rindex = 0;
                            dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.ROW_NO] = Convert.ToInt32(hdfSLNo.Value) + 1;
                            dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY] = txtQty.Text;
                            dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_X] = txtLoaded.Text;
                            dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_Y] = txtLoaded2.Text;
                            dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_FROM] = txtRowFrom.Text;
                            if (txtRowTo.Text.Trim() != string.Empty)
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_TO] = txtRowTo.Text;
                            else
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_TO] = "0";
                            if (txtType.Text.Trim() != string.Empty)
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_TYPE] = txtType.Text;
                            dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_CARTON_FROM] = txtCartonFrom.Text;
                            if (txtCartonTo.Text.Trim() != string.Empty)
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_CARTON_TO] = txtCartonTo.Text.Trim();
                            else
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_CARTON_TO] = "0";
                            dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_NO] = txtPlanNo.Text;
                            dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_DATE] = txtGeneratedOn.Text;
                            dsLoadingPlan.Tables[0].Rows[rindex][Resources.DataFieldRes.LoadingPlanLotNo] = txtLotNO.Text == "Select/Type" ? string.Empty : txtLotNO.Text;
                            //dsLoadingPlan.Tables[0].Rows[rindex][Resources.DataFieldRes.LoadingPlanLotNo] = ddlLotNo.SelectedValue != ERP.Utilities.CommonConstants.SELECTVAL ? ddlLotNo.SelectedValue : string.Empty;

                            //For BWH 
                            if (LoadPlanSCDetails)
                            {
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SOH_NO] = ddlSCNoDtls.SelectedIndex != 0 ? ddlSCNoDtls.SelectedItem.Text : string.Empty;
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SO_HDR] = ddlSCNoDtls.SelectedIndex != 0 ? ddlSCNoDtls.SelectedItem.Value : string.Empty;
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SO_DTL] = !string.IsNullOrEmpty(hdfLoadPlanSodPk.Value) ? hdfLoadPlanSodPk.Value != "0" ? hdfLoadPlanSodPk.Value : "0" : "0";
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SC_DATE] = txtSCDtlsDate.Text != string.Empty ? Convert.ToDateTime(txtSCDtlsDate.Text).ToString() : string.Empty;
                                if (string.IsNullOrEmpty(hdfBrand.Value) || hdfBrand.Value == "0")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Brand").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    perRet = false;
                                    break;
                                }
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_BRAND] = !string.IsNullOrEmpty(hdfBrand.Value) ? hdfBrand.Value != "0" ? hdfBrand.Value : "0" : "0";
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_BRAND_NAME] = txtBrand.Text == "Select/Type" ? string.Empty : txtBrand.Text;
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.SND_PLAN_QTY] = !string.IsNullOrEmpty(hdfShpPlanQty.Value) ? hdfShpPlanQty.Value : "0";
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_IR_RADIATION_LOT_NO] = txtIrradiationLotNo.Text == "Select/Type" ? string.Empty : txtIrradiationLotNo.Text;
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_EXP_DATE] = txtExpDate.Text;
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_NET_WT] = !string.IsNullOrEmpty(txtNetWtBox.Text) ? txtNetWtBox.Text : "0";
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_CTN_NET_WT] = !string.IsNullOrEmpty(txtNetWtCarton.Text) ? txtNetWtCarton.Text : "0";
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_GROSS_WT] = !string.IsNullOrEmpty(txtGrossWtBox.Text) ? txtGrossWtBox.Text : "0";
                                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_CTN_GROSS_WT] = !string.IsNullOrEmpty(txtGrossWtCarton.Text) ? txtGrossWtCarton.Text : "0";
                            }
                            dsLoadingPlan.AcceptChanges();
                        }
                        //For edit loading plan details
                        else
                        {
                            foreach (DataRow inRow in dsLoadingPlan.Tables[0].Rows)
                            {
                                if (inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.ROW_NO].ToString() == hdfSLNo.Value)
                                {
                                    inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY] = txtQty.Text;
                                    inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_X] = txtLoaded.Text;
                                    inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_Y] = txtLoaded2.Text;
                                    inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_FROM] = txtRowFrom.Text;
                                    if (txtRowTo.Text.Trim() != string.Empty)
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_TO] = txtRowTo.Text;
                                    else
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_TO] = "0";
                                    if (txtType.Text.Trim() != string.Empty)
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_TYPE] = txtType.Text;
                                    inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_CARTON_FROM] = txtCartonFrom.Text;
                                    if (txtCartonTo.Text.Trim() != string.Empty)
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_CARTON_TO] = txtCartonTo.Text.Trim();
                                    else
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_CARTON_TO] = "0";
                                    inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_NO] = txtPlanNo.Text;
                                    inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_DATE] = txtGeneratedOn.Text;
                                    inRow[Resources.DataFieldRes.LoadingPlanLotNo] = txtLotNO.Text == "Select/Type" ? string.Empty : txtLotNO.Text;
                                    //inRow[Resources.DataFieldRes.LoadingPlanLotNo] = ddlLotNo.SelectedValue != ERP.Utilities.CommonConstants.SELECTVAL ? ddlLotNo.SelectedValue : string.Empty;

                                    //For BWH 
                                    if (LoadPlanSCDetails)
                                    {
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SOH_NO] = ddlSCNoDtls.SelectedIndex != 0 ? ddlSCNoDtls.SelectedItem.Text : string.Empty;
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SO_HDR] = ddlSCNoDtls.SelectedIndex != 0 ? ddlSCNoDtls.SelectedItem.Value : string.Empty;
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SO_DTL] = !string.IsNullOrEmpty(hdfLoadPlanSodPk.Value) ? hdfLoadPlanSodPk.Value != "0" ? hdfLoadPlanSodPk.Value : "0" : "0";
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_SC_DATE] = txtSCDtlsDate.Text != string.Empty ? Convert.ToDateTime(txtSCDtlsDate.Text).ToString() : string.Empty;
                                        if (string.IsNullOrEmpty(hdfBrand.Value) || hdfBrand.Value == "0")
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Brand").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            perRet = false;
                                            break;
                                        }
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_BRAND] = !string.IsNullOrEmpty(hdfBrand.Value) ? hdfBrand.Value != "0" ? hdfBrand.Value : "0" : "0";
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_BRAND_NAME] = txtBrand.Text == "Select/Type" ? string.Empty : txtBrand.Text;
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.SND_PLAN_QTY] = !string.IsNullOrEmpty(hdfShpPlanQty.Value) ? hdfShpPlanQty.Value : "0";
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_IR_RADIATION_LOT_NO] = txtIrradiationLotNo.Text == "Select/Type" ? string.Empty : txtIrradiationLotNo.Text;
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_EXP_DATE] = txtExpDate.Text;
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_NET_WT] = !string.IsNullOrEmpty(txtNetWtBox.Text) ? txtNetWtBox.Text : "0";
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_CTN_NET_WT] = !string.IsNullOrEmpty(txtNetWtCarton.Text) ? txtNetWtCarton.Text : "0";
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_GROSS_WT] = !string.IsNullOrEmpty(txtGrossWtBox.Text) ? txtGrossWtBox.Text : "0";
                                        inRow[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_CTN_GROSS_WT] = !string.IsNullOrEmpty(txtGrossWtCarton.Text) ? txtGrossWtCarton.Text : "0";
                                    }
                                    dsLoadingPlan.AcceptChanges();
                                }
                            }
                        }
                        if (perRet)
                            BindGrid();
                        #region OldCode
                        //if (dsLoadingPlan != null)
                        //{
                        //    if (lpdID == 0 && dsLoadingPlan.Tables[0].Rows.Count >= 1 && Convert.ToString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY]) != string.Empty)
                        //    {
                        //        dr = dsLoadingPlan.Tables[0].NewRow();
                        //        //dr[ERP.Utilities.Constants.Shipping.LoadingPlan.ROW_NO] =Convert.ToInt32(hdfSLNo.Value)+1;
                        //        //hdfSLNo.Value = dr[ERP.Utilities.Constants.Shipping.LoadingPlan.ROW_NO].ToString();
                        //        dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY] = txtQty.Text;
                        //        dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_X] = txtLoaded.Text;
                        //        dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_Y] = txtLoaded2.Text;
                        //        dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_FROM] = txtRowFrom.Text;
                        //        if (txtRowTo.Text.Trim() != string.Empty)
                        //            dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_TO] = txtRowTo.Text.Trim();
                        //        if (txtType.Text.Trim() != string.Empty)
                        //            dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_TYPE] = txtType.Text.Trim();
                        //        dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_NO] = txtPlanNo.Text;
                        //        dr[ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_DATE] = txtGeneratedOn.Text;

                        //        dsLoadingPlan.Tables[0].Rows.Add(dr);
                        //        dsLoadingPlan.AcceptChanges();

                        //    }
                        //    else if (lpdID == 0 && dsLoadingPlan.Tables[0].Rows.Count == 1 && Convert.ToString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY]) == string.Empty)
                        //    {
                        //        int rindex = 0;
                        //        //dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.ROW_NO] = Convert.ToInt32(hdfSLNo.Value) + 1;
                        //        //hdfSLNo.Value = dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.ROW_NO].ToString();
                        //        dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY] = txtQty.Text;
                        //        dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_X] = txtLoaded.Text;
                        //        dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_Y] = txtLoaded2.Text;
                        //        dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_FROM] = txtRowFrom.Text;
                        //        dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_TO] = txtRowTo.Text;
                        //        dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_TYPE] = txtType.Text;
                        //        dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_NO] = txtPlanNo.Text;
                        //        dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_DATE] = txtGeneratedOn.Text;
                        //        dsLoadingPlan.AcceptChanges();
                        //    }
                        //    else
                        //    {
                        //        foreach (GridViewRow inRow in grdLoadingPlanDet.Rows)
                        //        {

                        //            if (Convert.ToInt32((inRow.FindControl("hdfLPDPK") as HiddenField).Value) == lpdID)
                        //            {
                        //                int rindex = inRow.RowIndex;
                        //                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY] = txtQty.Text;
                        //                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_X] = txtLoaded.Text;
                        //                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_Y] = txtLoaded2.Text;
                        //                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_FROM] = txtRowFrom.Text;
                        //                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_TO] = txtRowTo.Text;
                        //                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_TYPE] = txtType.Text;
                        //                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_NO] = txtPlanNo.Text;
                        //                dsLoadingPlan.Tables[0].Rows[rindex][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_DATE] = txtGeneratedOn.Text;
                        //                dsLoadingPlan.AcceptChanges();
                        //            }
                        //        }
                        //    }

                        //    BindGrid();
                        //}
                        #endregion
                        break;
                    #endregion

                    #region DELETE
                    // Do Action for , when click Delete button from LoadingPlanDet grid
                    case ActionsEnum.DELETEGRID:
                        //Delete loading plan details 
                        GridViewRow grdrow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        if (dsLoadingPlan.Tables[0].Rows.Count > 1)
                        {
                            dsLoadingPlan.Tables[0].Rows[grdrow.RowIndex].BeginEdit();
                            dsLoadingPlan.Tables[0].Rows[grdrow.RowIndex].Delete();
                            dsLoadingPlan.AcceptChanges();
                        }
                        else
                        {
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY] = DBNull.Value;
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_X] = DBNull.Value;
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_QTY_Y] = DBNull.Value;
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_FROM] = DBNull.Value;
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_ROW_TO] = DBNull.Value;
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_TYPE] = DBNull.Value;
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_NO] = DBNull.Value;
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_DATE] = DBNull.Value;
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_PK] = DBNull.Value;
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_ACTIVE] = DBNull.Value;
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_DESC] = DBNull.Value;
                            dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_PK] = DBNull.Value;
                        }
                        dsLoadingPlan.AcceptChanges();
                        BindGrid();
                        break;
                    #endregion

                    #region EDIT
                    // Do Action for , when click Edit button from LoadingPlanDet grid
                    case ActionsEnum.EDITGRID:
                        //fill the loading plan details
                        GridViewRow grdrowEdit = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        string[] strLoaded;
                        string[] strRow;
                        string[] strCarton;
                        hdfSLNo.Value = (grdrowEdit.FindControl("hdfRowNo") as HiddenField).Value;
                        strLoaded = (grdrowEdit.FindControl("lblLoaded") as Label).ToolTip.Split('x');
                        strRow = (grdrowEdit.FindControl("hdfRow") as HiddenField).Value.Split('-');
                        strCarton = (grdrowEdit.FindControl("hdfCarton") as HiddenField).Value.Split('-');
                        lpdID = (grdrowEdit.FindControl("hdfLPDPK") as HiddenField).Value == string.Empty ? 0 : Convert.ToInt32((grdrowEdit.FindControl("hdfLPDPK") as HiddenField).Value);
                        txtQty.Text = (grdrowEdit.FindControl("lblQuantity") as Label).Text.Replace(",", "");
                        //txtType.Text = (grdrowEdit.FindControl("lblType") as Label).Text;
                        txtType.Text = (grdrowEdit.FindControl("lblType") as Label).ToolTip;
                        txtRowTo.Text = strRow[1].Trim() != "0" ? strRow[1] : string.Empty;
                        txtRowFrom.Text = strRow[0];
                        txtLoaded2.Text = strLoaded[1];
                        txtLoaded.Text = strLoaded[0];
                        txtCartonFrom.Text = strCarton[0];
                        txtCartonTo.Text = strCarton[1];
                        txtLotNO.Text = (grdrowEdit.FindControl("lblGLotNo") as Label).Text;
                        //ddlLotNo.SelectedIndex = Convert.ToInt32(ddlLotNo.Items.IndexOf(ddlLotNo.Items.FindByValue((grdrowEdit.FindControl("lblGLotNo") as Label).ToolTip)));

                        //For BWH
                        if (LoadPlanSCDetails)
                        {
                            ddlSCNoDtls.SelectedIndex = ddlSCNoDtls.Items.IndexOf(ddlSCNoDtls.Items.FindByValue((grdrowEdit.FindControl("hdfSCNo") as HiddenField).Value));
                            hdfSCNoDtls.Value = (grdrowEdit.FindControl("hdfSCNo") as HiddenField).Value;
                            hdfLoadPlanSodPk.Value = (grdrowEdit.FindControl("hdfSodPk") as HiddenField).Value;
                            hdfLoadPlanSodPk1.Value = (grdrowEdit.FindControl("hdfSodPk1") as HiddenField).Value;
                            txtSCDtlsDate.Text = (grdrowEdit.FindControl("lblSCDate") as Label).Text;
                            txtBrand.Text = (grdrowEdit.FindControl("lblBrand") as Label).ToolTip.ToString();
                            hdfBrand.Value = (grdrowEdit.FindControl("hdfBrand") as HiddenField).Value;
                            hdfShpPlanQty.Value = (grdrowEdit.FindControl("hdfShpPlanQty") as HiddenField).Value;
                            txtIrradiationLotNo.Text = (grdrowEdit.FindControl("lblIrradiationLotNo") as Label).ToolTip;
                            txtExpDate.Text = (grdrowEdit.FindControl("lblExpDate") as Label).Text;
                            txtNetWtBox.Text = (grdrowEdit.FindControl("lblNetWtBox") as Label).Text;
                            txtNetWtCarton.Text = (grdrowEdit.FindControl("lblNetWtCarton") as Label).Text;
                            txtGrossWtBox.Text = (grdrowEdit.FindControl("lblGrossWtBox") as Label).Text;
                            txtGrossWtCarton.Text = (grdrowEdit.FindControl("lblGrossWtCarton") as Label).Text;

                        }
                        break;
                    #endregion

                    #region SAVESUBMIT
                    // Do Action for , when click save&submit button
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        //check shpping plan CTN Qty and Loading Plan CTN Qty equal
                        if (CheckCTNQty())
                        {
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AlertMessage1", "$(document).ready(function(){ShowAlertConfirm(2);});", true);
                        }
                        break;
                    #endregion

                    #region SUBMIT
                    // Do Action for , when click Submit button
                    case ActionsEnum.SUBMIT:
                        //check shpping plan CTN Qty and Loading Plan CTN Qty equal
                        if (CheckCTNQty())
                        {
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                            //Show WorkFlow Popup
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AlertMessage2", "$(document).ready(function(){ShowAlertConfirm(3);});", true);
                        }
                        break;
                    #endregion

                    #region WRKSUBMIT
                    // Do Action for , when click Workflow Submit button
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!string.IsNullOrEmpty(hdfAlert.Value))
                        {
                            if (Convert.ToInt16(hdfAlert.Value) == 5)
                            {
                                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                                ucrWrkf.Visible = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                                hdfAlert.Value = "1";
                                break;
                            }
                        }
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }

                        else//valid
                        {
                            hdfAlert.Value = null;
                            ////check shpping plan CTN Qty and Loading Plan CTN Qty equal
                            //if (CheckCTNQty())
                            //{
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                loadingPlanBOObj = (LoadingPlanBO)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);
                                xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(loadingPlanBOObj);
                                result = LoadingPlanBL.SaveLoadingPlan(xmlDoc.InnerXml);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    ucrWrkf.ApplicationID = ShippingPlanPK;
                                }
                                else
                                {
                                    if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.LoadingPlan + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.LoadingPlan + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.ALREADYCREATED)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.LoadingPlan + " " + GetLocalResourceObject("AlreadyCreated").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LoadingPlan);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                            else
                                ucrWrkf.ApplicationID = ShippingPlanPK;

                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    ucrWrkf.FillWorkFlowDetails();
                                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                        ucrWrkf.ViewType = 1;
                                    else
                                    {
                                        ucrWrkf.ViewType = 0;
                                        //EntryStatus = EntryStatus.VIEWMODE;
                                    }
                                    ucrWrkf.ViewAction();
                                    SetTabVisibility();
                                    //Show Save success message and reset Contract Entry
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LoadingPlan);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    //    + "','" + Resources.ErpRes.Information + "');", true);    // + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');"


                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanPK, ucrWrkf.ProcessID);
                                    ucrWrkf.FillWorkFlowDetails();

                                    EntryStatus = EntryStatus.ENTRYMODE;
                                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                        ucrWrkf.ViewType = 1;
                                    else
                                    {
                                        ucrWrkf.ViewType = 0;
                                        //EntryStatus = EntryStatus.VIEWMODE;
                                    }
                                    ucrWrkf.ViewAction();

                                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        GetFieldValues(ControlsEnum.DEFAULT);
                                        SetFieldValues(ControlsEnum.DEFAULT);
                                    }
                                }
                            }
                            else
                            {
                                //Trx not saved
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LoadingPlan);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }

                            //loadingPlanBOObj = (LoadingPlanBO)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);
                            //xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(loadingPlanBOObj);
                            //result = LoadingPlanBL.SaveLoadingPlan(xmlDoc.InnerXml);
                            //if (result >= 0) // Success ! re-initialize the page
                            //{

                            //ucrWrkf.ApplicationID = ShippingPlanPK;
                            //ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                            ////Do WorkFlow if WorkFlow has Actions
                            //if (ddlWkfAction.Items.Count > 0)
                            //{
                            //    action = ddlWkfAction.SelectedItem.ToString();
                            //    result = ucrWrkf.DoWorkFlow();
                            //    ucrWrkf.FillWorkFlowDetails();
                            //    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            //        ucrWrkf.ViewType = 1;
                            //    else
                            //    {
                            //        ucrWrkf.ViewType = 0;
                            //        //EntryStatus = EntryStatus.VIEWMODE;
                            //    }
                            //    ucrWrkf.ViewAction();
                            //    SetTabVisibility();
                            //    //Show Save success message and reset Contract Entry
                            //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                            //    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LoadingPlan);
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //        + "','" + Resources.ErpRes.Information + "');", true);    // + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');"
                            //}
                            //}
                            //else
                            //{
                            //    if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.SQLERROR)
                            //    {
                            //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //            + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //    else if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.CONCURRENCY)
                            //    {
                            //        litErrorMsg.Text = Resources.PageNameRes.Enquiry + " " + Resources.Messages.EditUsedByAnotherUser;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //        + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //    else if (result == (int)ERPSMS_v01.Administration.Masters.DbSaveStatus.CODEEXIST)
                            //    {
                            //        litErrorMsg.Text = Resources.PageNameRes.Enquiry + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            //    }
                            //    else
                            //    {
                            //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //            + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //}
                            //}
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AlertMessage3", "$(document).ready(function(){ShowAlertConfirm(4);});", true);
                            //}
                        }
                        break;
                    #endregion

                    #region Tab navigation
                    // Do Action for , when click SaleContract tab
                    case ActionsEnum.DEFAULT:
                        Response.Redirect(Resources.PageURL.SalesOrderListing);
                        break;
                    // Do Action for , when click Shipping Plan tab
                    case ActionsEnum.SHIPPINGPLAN:
                        Response.Redirect(Resources.PageURL.ShippingPlan);
                        break;
                    // Do Action for , when click Cont. Eval. tab
                    case ActionsEnum.CONTAINEREVALUATION:
                        Response.Redirect(Resources.PageURL.ContainerEvaulation);
                        break;
                    // Do Action for , when click Cont. Insp. tab
                    case ActionsEnum.CONTAINERINSPECTION:
                        Response.Redirect(Resources.PageURL.ContainerInspection);
                        break;
                    // Do Action for , when click QA Docs tab
                    case ActionsEnum.UPLOADQA:
                        Response.Redirect(Resources.PageURL.UploadQa);
                        break;
                    // Do Action for , when click Exp.Docs tab
                    case ActionsEnum.UPLOADEXPORT:
                        Response.Redirect(Resources.PageURL.UploadExport);
                        break;
                    // Do Action for , when click Load Plan tab
                    case ActionsEnum.LOADINGPLAN:
                        Response.Redirect(Resources.PageURL.LoadingPlan);
                        break;
                    // Do Action for , when click Photos tab
                    case ActionsEnum.UPLOADPHOTOGRAPHS:
                        Response.Redirect(Resources.PageURL.UploadPhotographs);
                        break;
                    // Do Action for , when click GON tab
                    case ActionsEnum.GOODOUTWARD:
                        Response.Redirect(Resources.PageURL.GoodOutward);
                        break;
                    // Do Action for , when click B/L tab
                    case ActionsEnum.BL:
                        Response.Redirect(Resources.PageURL.BillofLoading);
                        break;
                    // Do Action for , when click Cont. Release tab
                    case ActionsEnum.CONTAINERRELEASE:
                        Response.Redirect(Resources.PageURL.ContainerRelease);
                        break;
                    #endregion

                    #region PRINT
                    // Do Action for , when click Print button
                    case ActionsEnum.PRINT:
                        PrinterControl1.ShippingPlanID = ShippingPlanPK;
                        PrinterControl1.SetCommericalInvoice(PrinterControl1.ShippingPlanID);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);
                        break;
                    #endregion

                    #region CHANGE
                    case ActionsEnum.CHANGE:
                        txtSCDtlsDate.Text = string.Empty;
                        txtBrand.Text = string.Empty;
                        hdfSCNoDtls.Value = string.Empty;
                        hdfBrand.Value = string.Empty;
                        txtNetWtBox.Text = string.Empty;
                        txtNetWtCarton.Text = string.Empty;
                        txtGrossWtBox.Text = string.Empty;
                        txtGrossWtCarton.Text = string.Empty;
                        //txtBrand.Enabled = false;
                        soPk = Convert.ToInt32(ddlSCNoDtls.SelectedValue);
                        if (soPk > 0)
                        {
                            hdfSCNoDtls.Value = soPk.ToString();
                            //txtBrand.Enabled = true;
                            GetFieldValues(ControlsEnum.SODETAILS);
                            if (saleOrderHeaderObj != null)
                            {
                                txtSCDtlsDate.Text = saleOrderHeaderObj.SOH_DATE.ToString();
                            }
                        }
                        //else
                        //ResetForm(ControlsEnum.);
                        break;
                    #endregion

                    #region BRANDSELECTED
                    case ActionsEnum.BRANDSELECTED:
                        if (LoadPlanSCDetails)
                        {
                            if (!string.IsNullOrEmpty(hdfLoadPlanSodPk.Value) && hdfLoadPlanSodPk.Value != "0")
                            {
                                
                                soPk = Convert.ToInt32(ddlSCNoDtls.SelectedItem.Value);
                                sodPk = Convert.ToInt32(hdfLoadPlanSodPk.Value);
                                GetFieldValues(ControlsEnum.BRANDSELECTED);
                                SetFieldValues(ControlsEnum.BRANDSELECTED);
                            }
                        }
                        break;
                    #endregion

                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:
                        soPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + soPK + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=") + "');", true);
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

            }
        }
        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdLoadingPlanDet")
                {
                    if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Footer)
                    {
                        //SC Details - For BWH Only
                        if (!LoadPlanSCDetails)
                        {
                            e.Row.Cells[1].Visible = false;
                            e.Row.Cells[2].Visible = false;
                            e.Row.Cells[3].Visible = false;
                            e.Row.Cells[10].Visible = false;
                            e.Row.Cells[11].Visible = false;
                            e.Row.Cells[12].Visible = false;
                            e.Row.Cells[13].Visible = false;
                            e.Row.Cells[14].Visible = false;
                            e.Row.Cells[15].Visible = false;
                        }
                        // View Mode hide the Action buttons
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            e.Row.Cells[6].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Process Exception and show error message
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
                //reset all objects
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnAddToList.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkShippingPlan.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerEval.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerInspection.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadQADocs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadExportDocs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkLoadingPlan.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadPhotographs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerRelease.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkBillofLoading.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkPrintShippingDocs.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnAddToList.Load += new EventHandler(btnAction_Load);
            this.lnkShippingPlan.Load += new EventHandler(btnAction_Load);
            this.lnkContainerEval.Load += new EventHandler(btnAction_Load);
            this.lnkContainerInspection.Load += new EventHandler(btnAction_Load);
            this.lnkUploadQADocs.Load += new EventHandler(btnAction_Load);
            this.lnkUploadExportDocs.Load += new EventHandler(btnAction_Load);
            this.lnkLoadingPlan.Load += new EventHandler(btnAction_Load);
            this.lnkUploadPhotographs.Load += new EventHandler(btnAction_Load);
            this.lnkDeliveryOrder.Load += new EventHandler(btnAction_Load);
            this.lnkContainerRelease.Load += new EventHandler(btnAction_Load);
            this.lnkBillofLoading.Load += new EventHandler(btnAction_Load);
            this.lnkPrintShippingDocs.Load += new EventHandler(btnAction_Load);


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
            this.Init += new EventHandler(this.Page_Init);
        }

        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, ERPSMS_v01.UserControls.DataNavigatorEventArgs e)
        {
            try
            {

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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }








        #endregion

        #region WorkFlow Methods

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
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                //((HiddenField)this.Master.FindControl("hdfPageID")).Value = dtProcess.Rows[0][CommonConstants.F_PAGE].ToString();
            }
        }

        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            SAVELOADINGPLN,
            WRKFSUBMIT,
            SAVE,
            SHIPPINGPLANLEVEL,
            SPDEATILS,
            COMPANY,
            SCDETAILS,
            SODETAILS,
            BRANDSELECTED
        }

        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 1,
            APPROVED = 2,
            NEW = 0
        }
        #endregion
    }
}

