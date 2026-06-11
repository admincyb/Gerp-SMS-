using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using ERP.Utilities;
using System.Data;
using BusinessObject;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using BusinessObject.CommonManagement;
using BusinessObject.Administration.Masters;
using BusinessLogic.CommonManagement;
using System.Configuration;
namespace ERPSMS_v01.GeneralAdmin
{
    public partial class DashboardSetup : ERP.Store.UI.MyBasePage//: System.Web.UI.Page
    {
        #region Variables and Properties
        #region  Properties
        private int CurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrPK] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
            }
        }
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
        private List<DashboardDetails> DashboardDetailsList
        {
            get
            {
                return (List<DashboardDetails>)ViewState["DashboardDetails"];
            }
            set
            {
                ViewState["DashboardDetails"] = value;
            }
        }

        private List<ItemTrxDetails> ItemTrxDetailsList
        {
            get
            {
                return (List<ItemTrxDetails>)ViewState["ItemTrxDetailsList"];
            }
            set
            {
                ViewState["ItemTrxDetailsList"] = value;
            }
        }

        private List<DashboardItemDetails> DashboardItemDetailsList
        {
            get
            {
                return ViewState["DashboardItemDetails"] == null ? new List<DashboardItemDetails>() : (List<DashboardItemDetails>)ViewState["DashboardItemDetails"];
            }
            set
            {
                ViewState["DashboardItemDetails"] = value;
            }
        }
        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.TotalPages] ?? 1);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }
        /// <summary>
        /// DBD_SL_NO
        /// </summary>
        private int DbsSlNo
        {
            get
            {
                return (int)(this.ViewState["DbsSlNo"] ?? 1);
            }
            set
            {
                this.ViewState["DbsSlNo"] = value;
            }
        }

        private int CurrDashGroupPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrDashGroupPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrDashGroupPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrDashGroupPK] = value;
            }
        }
        #endregion
        #region  Variables
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        private DataTable dtResult;
        private DataTable dtDept;
        private DataTable dtMISReport;
        private DataTable dtMenu;
        private DataTable dtReportList;
        private DataTable dtDashlet;
        private DataTable dtType;
        private DataTable dtMode;
        private DataTable dtTheme;
        DataTable dtQueryValue;
        DataTable dtCompany;
        private DashboardHeader objDashboardHeader;
        private DashboardItemDetails objMenuMappingDetails;
        private DashboardItemDetails objDashletMappingDetails;
        private DashboardItemDetails objReportMappingDetails;

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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            // InitializeComponent();
            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
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
                PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                uclPaging.TotalPages = TotalPages;
                uclPaging.CurrentPage = 1;
                dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                if (dtCompany != null && dtCompany.Rows.Count > 0)
                {
                    hdfCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
                }
                hdfAdvSearch.Value = "0";
                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
                GetFieldValues(ControlsEnum.TYPE);
                SetFieldValues(ControlsEnum.TYPE);
                GetFieldValues(ControlsEnum.MODE);
                SetFieldValues(ControlsEnum.MODE);
                GetFieldValues(ControlsEnum.THEME);
                SetFieldValues(ControlsEnum.THEME);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
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

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }

        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        dtResult = BusinessLogic.Administration.Masters.DashboardSetupBL.GetDashboardSetupList(Convert.ToInt32(currentUser.PKUser), txtNameFilterList.Text, string.Empty, PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")),Convert.ToInt32(ddlFilterStatus.SelectedValue));
                        break;
                    #endregion

                    #region Get Dashboard Master Details
                    case ControlsEnum.DASHBOARDMASTERDETAILS:
                        objDashboardHeader = BusinessLogic.Administration.Masters.DashboardSetupBL.DashboardSetupByPK(CurrPK);
                        if (objDashboardHeader == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region TYPE
                    case ControlsEnum.TYPE:
                        dtType = CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("TypeConfig").ToString());
                        break;
                    #endregion

                    #region MODE
                    case ControlsEnum.MODE:
                        dtMode = CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("TypeConfigMenu").ToString());
                        break;
                    #endregion

                    #region THEME
                    case ControlsEnum.THEME:
                        dtTheme = CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("ThemeConfig").ToString());
                        break;
                    #endregion

                    #region Department
                    case ControlsEnum.DEPT:
                        //dtDept = BusinessLogic.CommonManagement.CommonBL.GetDepartment(currentUser.PKUser, currentUser.SBUID);
                        dtDept = BusinessLogic.CommonManagement.CommonBL.GetDepartment(currentUser.PKUser, currentUser.SBUID, 0);
                        break;
                    #endregion

                    #region MIS Report
                    case ControlsEnum.MISREPORTS:
                        dtMISReport = BusinessLogic.Administration.Masters.DashboardSetupBL.GetMISReportDtls(currentUser.PKUser, currentUser.SBUID, Convert.ToInt32(PopupValuesEnum.MNGPK), Convert.ToInt16(PopupValuesEnum.DEPT));
                        ViewState["ReportData"] = dtMISReport;
                        break;
                    #endregion

                    #region Menu
                    case ControlsEnum.MENU:
                        dtMenu = BusinessLogic.Administration.Masters.DashboardSetupBL.GetMenuDetails(currentUser, Convert.ToInt32(ddlDeptPopUp.SelectedValue));
                        break;
                    #endregion

                    #region Report
                    case ControlsEnum.REPORT:
                        DataTable dtRptList = (DataTable)ViewState["ReportData"];
                        if (ddlMISRptPopup.SelectedIndex > 0)
                        {
                            string SelectedInformation = Convert.ToString(dtRptList.Rows[ddlMISRptPopup.SelectedIndex - 1]["MNU_ACTION_URL"]);
                            ViewState["LinkInfo"] = SelectedInformation.ToString();
                            string[] Rptportion = SelectedInformation.Split('&');
                            string GrpVal = Rptportion[0].ToString();
                            string[] GrpPk = GrpVal.Split('=');
                            dtReportList = BusinessLogic.Administration.Masters.DashboardSetupBL.GetReportListDetails(Convert.ToInt32(GrpPk[1]));
                        }
                        break;
                    #endregion

                    #region Dashlet
                    case ControlsEnum.DASHLET:
                        dtDashlet = BusinessLogic.Administration.Masters.DashboardSetupBL.GetDashletDetails(currentUser.PKUser, currentUser.SBUID, 0);
                        break;
                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region TYPE
                    case ControlsEnum.TYPE:
                        BindDropDown(ControlsEnum.TYPE);
                        break;
                    #endregion

                    #region MODE
                    case ControlsEnum.MODE:
                        BindDropDown(ControlsEnum.MODE);
                        break;
                    #endregion

                    #region THEME
                    case ControlsEnum.THEME:
                        BindDropDown(ControlsEnum.THEME);
                        break;
                    #endregion

                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion

                    #region DASHBOARD DETAILS
                    case ControlsEnum.DASHBOARDDETAILS:
                        BindGrid(ControlsEnum.DASHBOARDDETAILS);
                        break;
                    #endregion

                    #region DASHBOARD MASTER DETAILS
                    case ControlsEnum.DASHBOARDMASTERDETAILS:
                        GetUIValuesFromObject(ControlsEnum.DASHBOARDMASTERDETAILS);
                        break;
                    #endregion

                    #region Dept
                    case ControlsEnum.DEPT:
                        BindDropDown(ControlsEnum.DEPT);
                        break;
                    #endregion

                    #region Report
                    case ControlsEnum.MISREPORTS:
                        BindDropDown(ControlsEnum.MISREPORTS);
                        break;
                    #endregion

                    #region Menu
                    case ControlsEnum.MENU:
                        BindGrid(ControlsEnum.MENU);
                        break;
                    #endregion

                    #region Report
                    case ControlsEnum.REPORT:
                        BindGrid(ControlsEnum.REPORT);
                        break;
                    #endregion

                    #region Dashlet
                    case ControlsEnum.DASHLET:
                        BindGrid(ControlsEnum.DASHLET);
                        break;
                    #endregion

                    #region MENU MAPPING GRID LIST
                    case ControlsEnum.MENUMAPPINGGRIDLIST:
                        BindGrid(ControlsEnum.MENUMAPPINGGRIDLIST);
                        break;
                    #endregion

                    #region DASHLET MAPPING GRID LIST
                    case ControlsEnum.DASHLETMAPPINGGRIDLIST:
                        BindGrid(ControlsEnum.DASHLETMAPPINGGRIDLIST);
                        break;
                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region Helper Methods
        private void BindMoreinfoGrid(string detailQryID)
        {
            divMoreInfo.Visible = false;
            DataTable dtQuery = CommonBL.GetQueryCFGValues(Convert.ToInt32(detailQryID));
            if (dtQuery != null && dtQuery.Rows.Count > 0)
            {
                string strQuery = dtQuery.Rows[0]["QRY_QUERY"].ToString();
                if (strQuery.Contains(GetLocalResourceObject("ParamUserPK").ToString()))
                {
                    strQuery = strQuery.Replace(GetLocalResourceObject("ParamUserPK").ToString(), currentUser.PKUser.ToString());
                }

                dtQueryValue = BusinessLogic.Administration.Masters.DashboardSetupBL.GetDashBoardDefultValues(strQuery);
                BindGrid(ControlsEnum.MOREINFO);
                divMoreInfo.Visible = true;

            }
        }
        private void SetMappedMoreinfoGrid(List<ItemTrxDetails> lstItemTrxDetails)
        {
            foreach (GridViewRow row in grdMoreDetails.Rows)
            {
                string pk = (row.FindControl("hdfPK") as HiddenField).Value;
                if (lstItemTrxDetails.Count(x => x.DBT_TRX_PK == pk) > 0)
                {
                    (row.FindControl("chkItemChecked") as CheckBox).Checked = true;
                }
            }
        }


        #region Set UIValues To Object
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            object returnObject = new object();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region ADDDASHBOARDHDR
                    case ControlsEnum.ADDDASHBOARDHDR:
                        Int16 module;
                        Int16.TryParse(ConfigurationManager.AppSettings[ERP.Utilities.ConfigStrings.GERPModule], out module);
                        DashboardHeader tempDashboardHeader = new DashboardHeader();
                        tempDashboardHeader.IND_PK = CurrPK;
                        tempDashboardHeader.IND_NAME = txtNameHd.Text.HtmlEncode();
                        tempDashboardHeader.IND_DESC = txtDescriptionHd.Text.HtmlEncode();
                        tempDashboardHeader.IND_MODULE = module;
                        tempDashboardHeader.IND_ACTIVE = chkActiveHd.Checked == true ? 1 : 0;
                        tempDashboardHeader.IND_SEQUENCE = Convert.ToInt32(txtSeqHd.Text); ;
                        tempDashboardHeader.IND_USER_GROUP = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        tempDashboardHeader.IND_MODE = Convert.ToInt16(ddlModeHd.SelectedValue == "-1" ? null : ddlModeHd.SelectedValue);
                        tempDashboardHeader.IND_TYPE = (ddlTypeHd.SelectedValue == "-1" ? null : ddlTypeHd.SelectedValue);
                        tempDashboardHeader.IND_CLASS = (ddlThemeHd.SelectedValue == "-1" ? null : ddlThemeHd.SelectedValue);
                        tempDashboardHeader.BIZUNIT_PK = Convert.ToInt16(currentUser.SBUID);
                        tempDashboardHeader.IND_MOD_DT = LastModifiedTime;
                        tempDashboardHeader.IND_USER = currentUser.PKUser;
                        SetUIValuesToObject(ControlsEnum.DASHBOARDGROUPDETAILSTOLIST);
                        SetFieldValues(ControlsEnum.DASHBOARDDETAILS);
                        SetUIValuesToObject(ControlsEnum.DASHBOARDGROUPDETAILS);
                        tempDashboardHeader.DashboardDetails = DashboardDetailsList;
                        //Need to change code for multi level item adding
                        if (Convert.ToInt16(ddlModeHd.SelectedValue) == Convert.ToInt16(PopupValuesEnum.DASHLET) && ItemTrxDetailsList != null && ItemTrxDetailsList.Count > 0)
                        {
                            int slNo = ItemTrxDetailsList[0].DBL_SL_NO2;
                            foreach (DashboardDetails item in tempDashboardHeader.DashboardDetails)
                            {
                                if (item.DBD_SL_NO == slNo)
                                {
                                    foreach (DashboardItemDetails itemDtl in item.DashboardItemDetails)
                                    {
                                        if (itemDtl.DBL_SL_NO == slNo)
                                        {
                                            itemDtl.lstItemTrxDetails = new List<ItemTrxDetails>();
                                            itemDtl.lstItemTrxDetails = ItemTrxDetailsList;
                                        }

                                    }
                                }
                            }
                        }
                        else
                            if (tempDashboardHeader.DashboardDetails != null && tempDashboardHeader.DashboardDetails.Count > 0)
                            {
                                if (tempDashboardHeader.DashboardDetails[0].DashboardItemDetails != null &&
                                    tempDashboardHeader.DashboardDetails[0].DashboardItemDetails.Count > 0)
                                    tempDashboardHeader.DashboardDetails[0].DashboardItemDetails[0].lstItemTrxDetails = null;
                            }
                        returnObject = tempDashboardHeader;
                        break;
                    #endregion
                    #region DASHBOARD GROUP DETAILS
                    case ControlsEnum.DASHBOARDGROUPDETAILS:
                        DashboardDetailsList = new List<DashboardDetails>();
                        foreach (GridViewRow grdrow in grdGroupDetails.Rows)
                        {
                            HiddenField hdfDBD_SLNO = (HiddenField)grdrow.FindControl("hdfDBD_SLNO");
                            HiddenField hdfDBD_PK = (HiddenField)grdrow.FindControl("hdfDBD_PK");
                            HiddenField hdfGvActiveGd = (HiddenField)grdrow.FindControl("hdfGvActiveGd");
                            Label lblGvNameGd = (Label)grdrow.FindControl("lblGvNameGd");
                            Label lblGvDescGd = (Label)grdrow.FindControl("lblGvDescGd");
                            HiddenField hdfSequence = (HiddenField)grdrow.FindControl("hdfSequence");

                            DashboardDetails temp_DashboardGroupDetails = new DashboardDetails();
                            temp_DashboardGroupDetails.DBD_SL_NO = Convert.ToInt32(hdfDBD_SLNO.Value);
                            temp_DashboardGroupDetails.DBD_PK = Convert.ToInt32(hdfDBD_PK.Value);
                            temp_DashboardGroupDetails.DBD_NAME = lblGvNameGd.ToolTip.HtmlEncode();
                            temp_DashboardGroupDetails.DBD_DESC = lblGvDescGd.ToolTip.HtmlEncode();
                            temp_DashboardGroupDetails.DBD_SEQUENCE = Convert.ToInt32(grdrow.RowIndex + 1); //Convert.ToInt32(lblGvSequenceGd.Text);
                            temp_DashboardGroupDetails.DBD_ACTIVE = Convert.ToInt32(hdfGvActiveGd.Value);
                            if (DashboardItemDetailsList != null)
                            {
                                List<DashboardItemDetails> tempDetails = DashboardItemDetailsList.Where(x => x.DBL_SL_NO == Convert.ToInt32(hdfDBD_SLNO.Value))
                                .ToList();
                                temp_DashboardGroupDetails.DashboardItemDetails = tempDetails.ToList();
                            }
                            DashboardDetailsList.Add(temp_DashboardGroupDetails);
                        }
                        break;
                    #endregion

                    #region SAVE MENU MAPPING
                    case ControlsEnum.SAVEMENUMAPPINGGRID:
                        int itemCount = 0;
                        if (DashboardItemDetailsList != null && DashboardItemDetailsList.Count != 0)
                        {
                            // DashboardItemDetailsList.RemoveAll(x => x.DBL_SL_NO == Convert.ToInt32(hdfCurGroupSlNo_PopUp.Value) && x.DBL_DPT == Convert.ToInt32(ddlDeptPopUp.SelectedValue));
                            var itemCount1 = DashboardItemDetailsList.Where(x => x.DBL_SL_NO == Convert.ToInt32(hdfCurGroupSlNo_PopUp.Value));
                            if (itemCount1.Count() > 0) itemCount = itemCount1.Max(x => x.DBL_SEQUENCE);
                        }
                        else
                        {
                            DashboardItemDetailsList = new List<DashboardItemDetails>();
                        }
                        foreach (GridViewRow grdrow in grdMenuPopUp.Rows)
                        {
                            CheckBox chkMenu_popUp = (CheckBox)grdrow.FindControl("chkMenu_popUp");
                            if (chkMenu_popUp.Checked == true)
                            {
                                DashboardItemDetailsList.RemoveAll(x => x.DBL_SL_NO == Convert.ToInt32(hdfCurGroupSlNo_PopUp.Value) && x.DBL_DPT == Convert.ToInt32(ddlDeptPopUp.SelectedValue)
                                    && x.DBL_MENU_CFG == Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfMNUPK_PopUp")).Value));
                                itemCount++;
                                objMenuMappingDetails = new DashboardItemDetails();
                                objMenuMappingDetails.DBL_SL_NO = Convert.ToInt32(hdfCurGroupSlNo_PopUp.Value);
                                objMenuMappingDetails.DBL_MENU_CFG = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfMNUPK_PopUp")).Value);
                                objMenuMappingDetails.DBL_NAME = (((TextBox)grdrow.FindControl("txtUserMenuNamePopUp")).Text).HtmlEncode();
                                objMenuMappingDetails.DBL_LINK = (((HiddenField)grdrow.FindControl("hdfMNUACTION_URL_PopUp")).Value).HtmlEncode();
                                objMenuMappingDetails.DBL_DPT = Convert.ToInt32(ddlDeptPopUp.SelectedValue);
                                objMenuMappingDetails.DBL_DPT_TEXT = (ddlDeptPopUp.SelectedItem.Text);
                                objMenuMappingDetails.DBL_DASH_DTL = Convert.ToInt32(hdfCurDBDPK_PopUp.Value);
                                objMenuMappingDetails.DBL_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                                objMenuMappingDetails.DBL_SEQUENCE = itemCount;//(((TextBox)grdrow.FindControl("txtSequencePopUp")).Text) == string.Empty ? itemCount : Convert.ToInt32((((TextBox)grdrow.FindControl("txtSequencePopUp")).Text));
                                objMenuMappingDetails.DBL_TYPE = Convert.ToInt32(PopupValuesEnum.MENUVALUE);
                                DashboardItemDetailsList.Add(objMenuMappingDetails);
                            }
                        }
                        foreach (GridViewRow grdrow in grdReportPopup.Rows)
                        {
                            CheckBox chkRpt_popUp = (CheckBox)grdrow.FindControl("chkRpt_popUp");
                            if (chkRpt_popUp.Checked == true)
                            {
                                string dbLink = (ViewState["LinkInfo"].ToString() + "&Report=" + Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRptPK_PopUp")).Value)).HtmlEncode();
                                DashboardItemDetailsList.RemoveAll(x => x.DBL_SL_NO == Convert.ToInt32(hdfCurRGroupSlNo_PopUp.Value) && x.DBL_LINK == dbLink);
                                itemCount++;
                                objReportMappingDetails = new DashboardItemDetails();
                                objReportMappingDetails.DBL_SL_NO = Convert.ToInt32(hdfCurRGroupSlNo_PopUp.Value);
                                objReportMappingDetails.DBL_MENU_CFG = Convert.ToInt32(ddlMISRptPopup.SelectedValue);//Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRptPK_PopUp")).Value); // ID:21704  Forgn Key Error While saving
                                objReportMappingDetails.DBL_NAME = (((TextBox)grdrow.FindControl("txtUserRptNamePopUp")).Text).HtmlEncode();
                                objReportMappingDetails.DBL_LINK = dbLink; //(ViewState["LinkInfo"].ToString() + "&Report=" + Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRptPK_PopUp")).Value)).HtmlEncode();
                                objReportMappingDetails.DBL_DPT = Convert.ToInt32(PopupValuesEnum.DEPT);
                                objReportMappingDetails.DBL_DASH_DTL = Convert.ToInt32(hdfCurRDBDPK_PopUp.Value);
                                objReportMappingDetails.DBL_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                                objReportMappingDetails.DBL_SEQUENCE = itemCount;
                                objReportMappingDetails.DBL_TYPE = Convert.ToInt32(PopupValuesEnum.RPTVALUE);
                                DashboardItemDetailsList.Add(objReportMappingDetails);
                            }
                        }
                        break;
                    #endregion

                    #region SAVE DASHLET MAPPING
                    case ControlsEnum.SAVEDASHLETMAPPINGGRID:
                        int itmCount = 0;
                        if (DashboardItemDetailsList != null && DashboardItemDetailsList.Count != 0)
                        {
                            // DashboardItemDetailsList.RemoveAll(x => x.DBL_SL_NO == Convert.ToInt32(hdfCurGroupSlNo_PopUp.Value) && x.DBL_DPT == Convert.ToInt32(ddlDeptPopUp.SelectedValue));
                            var itmCount1 = DashboardItemDetailsList.Where(x => x.DBL_SL_NO == Convert.ToInt32(hdfDashCurGroupSlNo_PopUp.Value));
                            if (itmCount1.Count() > 0) itmCount = itmCount1.Max(x => x.DBL_SEQUENCE);
                        }
                        else
                        {
                            DashboardItemDetailsList = new List<DashboardItemDetails>();
                        }
                        foreach (GridViewRow grdrow in grdDashletPopUp.Rows)
                        {
                            CheckBox chkDash_popUp = (CheckBox)grdrow.FindControl("chkDashlet_popUp");
                            if (chkDash_popUp.Checked == true)
                            {
                                DashboardItemDetailsList.RemoveAll(x => x.DBL_SL_NO == Convert.ToInt32(hdfDashCurGroupSlNo_PopUp.Value)
                                    && x.DBL_DASHLET == Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDasHPK_PopUp")).Value));
                                itmCount++;
                                objDashletMappingDetails = new DashboardItemDetails();
                                objDashletMappingDetails.DBL_SL_NO = Convert.ToInt32(hdfDashCurGroupSlNo_PopUp.Value);
                                objDashletMappingDetails.DBL_SL_NO2 = objDashletMappingDetails.DBL_SL_NO;//Need to change if multiple Item adding
                                //objDashletMappingDetails.DBL_MENU_CFG = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDasHPK_PopUp")).Value);
                                objDashletMappingDetails.DBL_NAME = (((TextBox)grdrow.FindControl("txtDashUserMenuNamePopUp")).Text).HtmlEncode();
                                objDashletMappingDetails.DBL_LINK = (((HiddenField)grdrow.FindControl("hdfDashACTION_URL_PopUp")).Value).HtmlEncode();
                                //objMenuMappingDetails.DBL_DPT = Convert.ToInt32(ddlDeptPopUp.SelectedValue);
                                //objMenuMappingDetails.DBL_DPT_TEXT = (ddlDeptPopUp.SelectedItem.Text);
                                objDashletMappingDetails.DBL_DASH_DTL = Convert.ToInt32(hdfCurDBDPK_PopUp.Value);
                                objDashletMappingDetails.DBL_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                                objDashletMappingDetails.DBL_SEQUENCE = itmCount;
                                objDashletMappingDetails.DBL_DASHLET = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDasHPK_PopUp")).Value);
                                objDashletMappingDetails.DBL_TYPE = Convert.ToInt32(PopupValuesEnum.DASHLETVALUE);
                                objDashletMappingDetails.DLC_TYPE = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDashDLC_TYPE_PopUp")).Value);
                                objDashletMappingDetails.DLC_TYPE_TEXT = (((Label)grdrow.FindControl("lblDashTypePopUp")).Text).HtmlEncode();
                                objDashletMappingDetails.DLC_DESC = (((Label)grdrow.FindControl("lblDashDescPopUp")).Text).HtmlEncode();
                                if (((DropDownList)grdrow.FindControl("ddlDefult")).Visible == true && (((DropDownList)grdrow.FindControl("ddlDefult")).SelectedValue) != CommonConstants.SELECTVAL)
                                {
                                    objDashletMappingDetails.DBL_DEF_PK = (((DropDownList)grdrow.FindControl("ddlDefult")).SelectedValue);
                                    objDashletMappingDetails.DBL_DEF_NAME = (((DropDownList)grdrow.FindControl("ddlDefult")).SelectedItem.Text).HtmlEncode();
                                }
                                DashboardItemDetailsList.Add(objDashletMappingDetails);
                            }
                        }
                        break;
                    #endregion

                    #region DASHBOARD GROUP GRID DETAILS TO LIST
                    case ControlsEnum.DASHBOARDGROUPDETAILSTOLIST:
                        DashboardDetailsList = new List<DashboardDetails>();
                        foreach (GridViewRow grdrow in grdGroupDetails.Rows)
                        {
                            HiddenField hdfDBD_SLNO = (HiddenField)grdrow.FindControl("hdfDBD_SLNO");
                            HiddenField hdfDBD_PK = (HiddenField)grdrow.FindControl("hdfDBD_PK");
                            HiddenField hdfGvActiveGd = (HiddenField)grdrow.FindControl("hdfGvActiveGd");
                            Label lblGvNameGd = (Label)grdrow.FindControl("lblGvNameGd");
                            Label lblGvDescGd = (Label)grdrow.FindControl("lblGvDescGd");
                            HiddenField hdfSequence = (HiddenField)grdrow.FindControl("hdfSequence");

                            DashboardDetails temp_DashboardGroupDetails = new DashboardDetails();
                            temp_DashboardGroupDetails.DBD_SL_NO = Convert.ToInt32(hdfDBD_SLNO.Value);
                            temp_DashboardGroupDetails.DBD_PK = Convert.ToInt32(hdfDBD_PK.Value);
                            temp_DashboardGroupDetails.DBD_NAME = lblGvNameGd.ToolTip.HtmlEncode();
                            temp_DashboardGroupDetails.DBD_DESC = lblGvDescGd.ToolTip.HtmlEncode();
                            temp_DashboardGroupDetails.DBD_SEQUENCE = Convert.ToInt32(hdfSequence.Value);
                            temp_DashboardGroupDetails.DBD_ACTIVE = Convert.ToInt32(hdfGvActiveGd.Value);
                            DashboardDetailsList.Add(temp_DashboardGroupDetails);
                        }
                        break;
                    #endregion
                }
                return returnObject;
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
        #region Get UIValues From Object
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.DASHBOARDMASTERDETAILS:
                        if (objDashboardHeader != null)
                        {
                            txtNameHd.Text = objDashboardHeader.IND_NAME.HtmlDecode();
                            txtDescriptionHd.Text = objDashboardHeader.IND_DESC.HtmlDecode();
                            ddlTypeHd.SelectedValue = Convert.ToString(objDashboardHeader.IND_TYPE);
                            ddlThemeHd.SelectedValue = Convert.ToString(objDashboardHeader.IND_CLASS);
                            ddlModeHd.SelectedValue = Convert.ToString(objDashboardHeader.IND_MODE);
                            txtSeqHd.Text = Convert.ToString(objDashboardHeader.IND_SEQUENCE);
                            chkActiveHd.Checked = objDashboardHeader.IND_ACTIVE == 1 ? true : false;
                            LastModifiedTime = objDashboardHeader.IND_MOD_DT;
                            DashboardDetailsList = objDashboardHeader.DashboardDetails;
                            SetFieldValues(ControlsEnum.DASHBOARDDETAILS);
                            DashboardItemDetailsList = new List<DashboardItemDetails>();
                            foreach (var Grouprow in DashboardDetailsList)
                                foreach (var itemRows in Grouprow.DashboardItemDetails)
                                {
                                    objMenuMappingDetails = new DashboardItemDetails();
                                    objMenuMappingDetails = itemRows;
                                    DashboardItemDetailsList.Add(itemRows);
                                }
                            ddlModeHd.Enabled = false;
                            if (objDashboardHeader.DashboardDetails[0].DashboardItemDetails.Count > 0)
                                ItemTrxDetailsList = objDashboardHeader.DashboardDetails[0].DashboardItemDetails[0].lstItemTrxDetails;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Bind DropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region TYPE
                case ControlsEnum.TYPE:
                    ddlTypeHd.Items.Clear();
                    if (dtType != null && dtType.Rows.Count > 0)
                    {
                        ddlTypeHd.DataSource = dtType;
                        ddlTypeHd.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_FIELD;
                        ddlTypeHd.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlTypeHd.DataBind();
                    }
                    ddlTypeHd.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region MODE
                case ControlsEnum.MODE:
                    ddlModeHd.Items.Clear();
                    if (dtMode != null && dtMode.Rows.Count > 0)
                    {
                        ddlModeHd.DataSource = dtMode;
                        ddlModeHd.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_FIELD;
                        ddlModeHd.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlModeHd.DataBind();
                    }
                    ddlModeHd.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region THEME
                case ControlsEnum.THEME:
                    ddlThemeHd.Items.Clear();
                    if (dtTheme != null && dtTheme.Rows.Count > 0)
                    {
                        ddlThemeHd.DataSource = dtTheme;
                        ddlThemeHd.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                        ddlThemeHd.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlThemeHd.DataBind();
                    }
                    ddlThemeHd.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Dept
                case ControlsEnum.DEPT:
                    ddlDeptPopUp.Items.Clear();
                    if (dtDept != null && dtDept.Rows.Count > 0)
                    {
                        ddlDeptPopUp.DataSource = dtDept;
                        ddlDeptPopUp.DataTextField = GTIService.Constants.Common.Common.F_DEPARTMENTNAME;
                        ddlDeptPopUp.DataValueField = GTIService.Constants.Common.Common.F_DEPARTMENT;
                        ddlDeptPopUp.DataBind();
                    }
                    ddlDeptPopUp.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Report
                case ControlsEnum.MISREPORTS:
                    ddlMISRptPopup.Items.Clear();
                    if (dtMISReport != null && dtMISReport.Rows.Count > 0)
                    {
                        ddlMISRptPopup.DataSource = dtMISReport;
                        ddlMISRptPopup.DataTextField = GTIService.Constants.Common.Common.F_MNU_NAME;
                        ddlMISRptPopup.DataValueField = GTIService.Constants.Common.Common.F_MNU_PK;
                        ddlMISRptPopup.DataBind();
                    }
                    ddlMISRptPopup.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                default:
                    break;
            }
        }
        #endregion
        #region BindGrid
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.MOREINFO:
                        if (dtQueryValue != null)
                        {
                            grdMoreDetails.DataSource = dtQueryValue;
                            grdMoreDetails.DataBind();
                        }
                        break;
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dtResult.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dtResult;
                            grdList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdList.DataSource = null;
                            grdList.DataBind();
                        }
                        break;
                    #endregion

                    #region DASHBOARD DETAILS
                    case ControlsEnum.DASHBOARDDETAILS:
                        if (DashboardDetailsList != null)
                        {
                            grdGroupDetails.DataSource = DashboardDetailsList.Where(x => x.IS_DELETED == 0).OrderBy(x => x.DBD_SEQUENCE);
                            grdGroupDetails.DataBind();
                        }
                        else
                        {
                            grdGroupDetails.DataSource = null;
                            grdGroupDetails.DataBind();
                        }
                        break;
                    #endregion

                    #region Menu
                    case ControlsEnum.MENU:
                        if (dtMenu != null)
                        {
                            grdMenuPopUp.DataSource = dtMenu;
                            grdMenuPopUp.DataBind();
                        }
                        else
                        {
                            grdMenuPopUp.DataSource = null;
                            grdMenuPopUp.DataBind();
                        }
                        break;
                    #endregion

                    #region Report
                    case ControlsEnum.REPORT:
                        if (dtReportList != null)
                        {
                            grdReportPopup.DataSource = dtReportList;
                            grdReportPopup.DataBind();
                        }
                        else
                        {
                            grdReportPopup.DataSource = null;
                            grdReportPopup.DataBind();
                        }
                        break;
                    #endregion

                    #region Dashlet
                    case ControlsEnum.DASHLET:
                        if (dtDashlet != null)
                        {
                            grdDashletPopUp.DataSource = dtDashlet;
                            grdDashletPopUp.DataBind();
                        }
                        else
                        {
                            grdDashletPopUp.DataSource = null;
                            grdDashletPopUp.DataBind();
                        }
                        break;
                    #endregion

                    #region MENU MAPPING GRID LIST
                    case ControlsEnum.MENUMAPPINGGRIDLIST:
                        if (DashboardItemDetailsList != null)
                        {
                            grdMenuPopUpList.DataSource = DashboardItemDetailsList.Where(x => x.DBL_SL_NO == Convert.ToInt32(hdfCurGroupSlNo_PopUp.Value) &&
                                x.DBL_TYPE != Convert.ToInt16(PopupValuesEnum.DASHLETVALUE)).OrderBy(x => x.DBL_SEQUENCE);
                            grdMenuPopUpList.DataBind();
                            if (grdMenuPopUpList.Rows.Count > 0)
                            {
                                ((ImageButton)grdMenuPopUpList.Rows[0].FindControl("imbRuleUpPop")).Visible = false;
                                ((ImageButton)grdMenuPopUpList.Rows[grdMenuPopUpList.Rows.Count - 1].FindControl("imbRuleDownPop")).Visible = false;
                            }
                        }
                        else
                        {
                            grdMenuPopUpList.DataSource = null;
                            grdMenuPopUpList.DataBind();
                        }
                        break;
                    #endregion

                    #region DASHLET MAPPING GRID LIST
                    case ControlsEnum.DASHLETMAPPINGGRIDLIST:
                        if (DashboardItemDetailsList != null)
                        {
                            grdDashletPopupList.DataSource = DashboardItemDetailsList.Where(x => x.DBL_SL_NO == Convert.ToInt32(hdfDashCurGroupSlNo_PopUp.Value) &&
                                x.DBL_TYPE == Convert.ToInt16(PopupValuesEnum.DASHLETVALUE)).OrderBy(x => x.DBL_SEQUENCE);
                            grdDashletPopupList.DataBind();

                            if (grdDashletPopupList.Rows.Count > 0)
                            {
                                ((ImageButton)grdDashletPopupList.Rows[0].FindControl("imbRuleUpPop")).Visible = false;
                                ((ImageButton)grdDashletPopupList.Rows[grdDashletPopupList.Rows.Count - 1].FindControl("imbRuleDownPop")).Visible = false;
                            }
                        }
                        else
                        {
                            grdDashletPopupList.DataSource = null;
                            grdDashletPopupList.DataBind();
                        }
                        break;
                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Reset Form
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                # region CLEAR
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    DbsSlNo = -1;
                    divMoreInfo.Visible = false;
                    ItemTrxDetailsList = null;
                    txtNameFilterList.Text = string.Empty;
                    txtNameHd.Text = string.Empty;
                    txtDescriptionHd.Text = string.Empty;
                    txtSeqHd.Text = string.Empty;
                    if (ddlTypeHd.Items.Count > 0)
                        ddlTypeHd.SelectedIndex = 0;
                    if (ddlThemeHd.Items.Count > 0)
                        ddlThemeHd.SelectedIndex = 0;
                    ddlModeHd.Enabled = true;
                    if (ddlModeHd.Items.Count > 0)
                        ddlModeHd.SelectedIndex = 0;
                    txtNameGd.Text = string.Empty;
                    txtDescriptionGd.Text = string.Empty;
                    //txtSeqGd.Text = string.Empty;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    DashboardDetailsList = null;
                    CurrDashGroupPK = 0;
                    DashboardItemDetailsList = null;
                    SetFieldValues(ControlsEnum.DASHBOARDDETAILS);
                    SetControlsWithMode();
                    break;
                #endregion
                # region CLEAR DASHBOARD DETAILS
                case ControlsEnum.CLEARDASHBOARDDETAILS:
                    hdfgvdDtailsSiNo.Value = "0";
                    CurrDashGroupPK = 0;
                    txtNameGd.Text = string.Empty;
                    txtDescriptionGd.Text = string.Empty;
                    //txtSeqGd.Text = string.Empty;
                    if (DashboardDetailsList.Count == 0)
                        ddlModeHd.Enabled = true;
                    //ddlModeHd.SelectedIndex = -1;
                    break;
                #endregion
                # region CLEAR FILTER
                case ControlsEnum.CLEARFILTER:
                    txtNameFilterList.Text = string.Empty;
                    ddlFilterStatus.SelectedIndex = 0;
                    CurrPK = 0;
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = 1;
                    this.EntryStatus = EntryStatus.LISTMODE;
                    break;
                #endregion
            }
        }
        #endregion
        #region SetControlsWithMode
        private void SetControlsWithMode()
        {
            if (Convert.ToInt16(ddlModeHd.SelectedValue) == Convert.ToInt16(PopupValuesEnum.DASHLET))
            {
                if (CurrPK == 0)
                    txtNameHd.Text = string.Empty;
                txtNameHd.Enabled = false;
                txtNameHd.CssClass = "input-half input-disabled";
                txtNameGd.Enabled = false;
                txtNameGd.CssClass = "input-half input-disabled";
                rfvNameHd.Enabled = false;
                imgAdd.Visible = false;
                imgbtnMappingPopup.Visible = true;
            }
            else
            {
                txtNameHd.Enabled = true;
                txtNameHd.CssClass = "input-half";
                txtNameGd.Enabled = true;
                txtNameGd.CssClass = "input-half";
                rfvNameHd.Enabled = true;
                imgAdd.Visible = true;
                imgbtnMappingPopup.Visible = false;
            }
        }
        #endregion
        #endregion

        #region InitializeComponent
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
        #endregion

        #region EnableDisableButtons
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the first link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the previous link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false; // Should we enable the next link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;// Should we enable the last link
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
                int? result;
                bool bIsChecked = false;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlDeptPopUp")
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlMISRptPopup")
                    {
                        commonActions = ActionsEnum.MISCHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlModeHd")
                    {
                        commonActions = ActionsEnum.MODECHANGE;
                    }
                }
                switch (commonActions)
                {
                    case ActionsEnum.APPLY:
                        List<ItemTrxDetails> itemTrxDtlsList = new List<ItemTrxDetails>();
                        foreach (GridViewRow grdrow in grdMoreDetails.Rows)
                        {
                            ItemTrxDetails objItem = new ItemTrxDetails();
                            CheckBox chkItemChkd = (CheckBox)grdrow.FindControl("chkItemChecked");
                            if (chkItemChkd.Checked)
                            {
                                objItem.DBL_SL_NO2 = DbsSlNo;
                                objItem.DBT_TRX_PK = (((HiddenField)grdrow.FindControl("hdfPK")).Value).ToString();
                                objItem.DBT_TRX_NAME = (((Label)grdrow.FindControl("lblValue")).Text);
                                itemTrxDtlsList.Add(objItem);
                            }

                        }
                        ItemTrxDetailsList = itemTrxDtlsList;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        break;
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        txtNameHd.Focus();
                        ResetForm(ControlsEnum.CLEAR);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objDashboardHeader = new DashboardHeader();
                            objDashboardHeader = (DashboardHeader)SetUIValuesToObject(ControlsEnum.ADDDASHBOARDHDR);
                            if (objDashboardHeader != null)
                            {
                                if (objDashboardHeader.DashboardDetails != null && objDashboardHeader.DashboardDetails.Count > 0)
                                {
                                    string xmlDoc = CommonFunctions.XmlSerialize<DashboardHeader>(objDashboardHeader);
                                    result = BusinessLogic.Administration.Masters.DashboardSetupBL.SaveDashboardSetupDetails(xmlDoc);
                                    if (result > 0)
                                    {
                                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DashboardSetup);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEAR);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        CurrPK = (int)result;
                                        GetFieldValues(ControlsEnum.LIST);
                                        SetFieldValues(ControlsEnum.LIST);
                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.DashboardSetup + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.DashboardSetup + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_NameExist").ToString());
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        //else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        //{
                                        //    litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_DateExist").ToString()) + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        //        + "','" + Resources.ErpRes.Information + "');", true);
                                        //}
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DashboardSetup);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.Err_AddDashboardDtl1;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        CurrPK = 0;
                        string LastModDate = string.Empty;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfINDPK_List")).Value);
                                LastModDate = (((HiddenField)grdrow.FindControl("hdfModDate_List")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            result = 0;
                            result = BusinessLogic.Administration.Masters.DashboardSetupBL.DeleteRecord(CurrPK, LastModDate);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry 
                                if (grdList.Rows.Count == 1 && PageIndex > 1)
                                {
                                    PageIndex--;
                                }
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DashboardSetup);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.DashboardSetup + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.DashboardSetup + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.DashboardSetup + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DashboardSetup);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DETAIL
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfINDPK_List")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            txtNameHd.Focus();
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.DASHBOARDMASTERDETAILS);
                            SetFieldValues(ControlsEnum.DASHBOARDMASTERDETAILS);
                            SetControlsWithMode();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideGroupDetails", "ShowHideGroupDetails(1);", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SHOW MENU POPUP DETAILS
                    case ActionsEnum.SHOWMENUDETAILS:
                        divMenuPopupDetails.Visible = true;
                        divReportPopupDetails.Visible = false;
                        lnkMISReport.CssClass = GetLocalResourceObject("TabInActive").ToString();
                        lnkMenu.CssClass = GetLocalResourceObject("TabActive").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpGroupItemDetails]','" + GetLocalResourceObject("GroupMenuMapping").ToString() + "','660','540');", true);
                        break;
                    #endregion
                    #region SHOW REPORT POPUP DETAILS
                    case ActionsEnum.SHOWREPORTDETAILS:
                        divReportPopupDetails.Visible = true;
                        divMenuPopupDetails.Visible = false;
                        lnkMISReport.CssClass = GetLocalResourceObject("TabActive").ToString();
                        lnkMenu.CssClass = GetLocalResourceObject("TabInActive").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpGroupItemDetails]','" + GetLocalResourceObject("GroupMenuMapping").ToString() + "','660','540');", true);
                        break;
                    #endregion
                    #region ADDTOLIST
                    case ActionsEnum.ADDTOLIST:
                        if (Convert.ToInt16(ddlModeHd.SelectedValue) == Convert.ToInt16(PopupValuesEnum.DASHLET) && grdGroupDetails.Rows.Count > 0)
                        {
                            bool isEdit = false;
                            if (CurrDashGroupPK > 0 || (CurrDashGroupPK == 0 && Convert.ToInt32(hdfgvdDtailsSiNo.Value) > 0)) // For Edit Item 
                                isEdit = true;
                            else
                                isEdit = false;

                            if (!isEdit)
                            {
                                ResetForm(ControlsEnum.CLEARDASHBOARDDETAILS);
                                litErrorMsg.Text = GetLocalResourceObject("Msg_AddOnlyOneGroup").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                break;
                            }
                        }

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideGroupDetails", "ShowHideGroupDetails(1);", true);
                        DashboardDetails objDashboardDetails = new DashboardDetails();
                        List<DashboardDetails> tempList = DashboardDetailsList;
                        if (CurrDashGroupPK > 0 || (CurrDashGroupPK == 0 && Convert.ToInt32(hdfgvdDtailsSiNo.Value) > 0)) // For Edit Item
                        {
                            objDashboardDetails = tempList.Where(itm => itm.DBD_SL_NO == Convert.ToInt32(hdfgvdDtailsSiNo.Value) && itm.IS_DELETED != 1).FirstOrDefault();
                            objDashboardDetails.DBD_SL_NO = Convert.ToInt32(hdfgvdDtailsSiNo.Value);
                            objDashboardDetails.DBD_PK = CurrDashGroupPK;
                            objDashboardDetails.DBD_NAME = txtNameGd.Text.HtmlEncode();
                            objDashboardDetails.DBD_DESC = txtDescriptionGd.Text.HtmlEncode();
                            objDashboardDetails.DBD_SEQUENCE = Convert.ToInt32(hdfgrdDtailsCurSeqNo.Value);
                            objDashboardDetails.DBD_ACTIVE = chkActiveGd.Checked == true ? 1 : 0;
                        }
                        else//For Add New Item
                        {
                            if (DashboardDetailsList == null)
                            {
                                tempList = DashboardDetailsList = new List<DashboardDetails>();
                            }
                            else
                            {
                                SetUIValuesToObject(ControlsEnum.DASHBOARDGROUPDETAILSTOLIST);
                            }
                            objDashboardDetails = new DashboardDetails();
                            objDashboardDetails.DBD_SL_NO = tempList.Count + 1;
                            objDashboardDetails.DBD_PK = CurrDashGroupPK;
                            objDashboardDetails.DBD_NAME = txtNameGd.Text.HtmlEncode();
                            objDashboardDetails.DBD_DESC = txtDescriptionGd.Text.HtmlEncode();
                            objDashboardDetails.DBD_SEQUENCE = tempList.Count + 1;
                            objDashboardDetails.DBD_ACTIVE = chkActiveGd.Checked == true ? 1 : 0;
                            tempList.Add(objDashboardDetails);
                        }
                        DashboardDetailsList = tempList;
                        if (DashboardDetailsList.Count > 0)
                            ddlModeHd.Enabled = false;

                        SetFieldValues(ControlsEnum.DASHBOARDDETAILS);
                        ResetForm(ControlsEnum.CLEARDASHBOARDDETAILS);
                        break;
                    #endregion
                    #region  Dept. CHANGE
                    case ActionsEnum.CHANGE:
                        GetFieldValues(ControlsEnum.MENU);
                        SetFieldValues(ControlsEnum.MENU);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpGroupItemDetails]','" + GetLocalResourceObject("GroupMenuMapping").ToString() + "','660','540');", true);
                        break;
                    #endregion
                    #region  Report CHANGE
                    case ActionsEnum.MISCHANGE:
                        GetFieldValues(ControlsEnum.REPORT);
                        SetFieldValues(ControlsEnum.REPORT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpGroupItemDetails]','" + GetLocalResourceObject("GroupMenuMapping").ToString() + "','660','540');", true);
                        break;
                    #endregion
                    #region  ADD MENU MAPPING
                    case ActionsEnum.SAVE_ACTIONPOPUP:
                        SetUIValuesToObject(ControlsEnum.SAVEMENUMAPPINGGRID);
                        SetFieldValues(ControlsEnum.MENUMAPPINGGRIDLIST);
                        GetFieldValues(ControlsEnum.MENU);
                        SetFieldValues(ControlsEnum.MENU);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpGroupItemDetails]','" + GetLocalResourceObject("GroupMenuMapping").ToString() + "','660','540');", true);
                        break;
                    #endregion
                    #region  ADD DASHLET MAPPING
                    case ActionsEnum.SAVE_ACTIONDASHLET:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupItemDashletDetails]','" + GetLocalResourceObject("DashletMenuMapping").ToString() + "','820','540');", true);
                        if (grdDashletPopupList.Rows.Count == 0)
                        {
                            #region Set Group Details
                            DbsSlNo = -1;
                            string detailQryID = string.Empty;
                            string dblName = string.Empty, dlcDesc = string.Empty;
                            List<DashboardDetails> tempDashDetailsList = new List<DashboardDetails>();
                            foreach (GridViewRow grdrow in grdDashletPopUp.Rows)
                            {
                                CheckBox chkDash_popUp = (CheckBox)grdrow.FindControl("chkDashlet_popUp");
                                if (chkDash_popUp.Checked == true)
                                {
                                    dblName = (((TextBox)grdrow.FindControl("txtDashUserMenuNamePopUp")).Text).HtmlEncode();
                                    dlcDesc = (((Label)grdrow.FindControl("lblDashDescPopUp")).Text).HtmlEncode();
                                    detailQryID = (((HiddenField)grdrow.FindControl("hdfDetailQry")).Value);
                                }
                            }

                            if (DashboardDetailsList == null)
                            {
                                tempDashDetailsList = DashboardDetailsList = new List<DashboardDetails>();
                            }
                            divMoreInfo.Visible = false;
                            if (!string.IsNullOrEmpty(detailQryID))
                            {
                                BindMoreinfoGrid(detailQryID);
                            }
                            objDashboardDetails = new DashboardDetails();
                            objDashboardDetails.DBD_SL_NO = tempDashDetailsList.Count + 1;
                            hdfDashCurGroupSlNo_PopUp.Value = objDashboardDetails.DBD_SL_NO.ToString();
                            objDashboardDetails.DBD_PK = CurrDashGroupPK;
                            objDashboardDetails.DBD_NAME = dblName;
                            txtNameHd.Text = dblName;
                            objDashboardDetails.DBD_DESC = dlcDesc;
                            objDashboardDetails.DBD_SEQUENCE = tempDashDetailsList.Count + 1;
                            objDashboardDetails.DBD_ACTIVE = chkActiveGd.Checked == true ? 1 : 0;
                            tempDashDetailsList.Add(objDashboardDetails);

                            DbsSlNo = objDashboardDetails.DBD_SL_NO;
                            DashboardDetailsList = tempDashDetailsList;
                            if (DashboardDetailsList.Count > 0)
                                ddlModeHd.Enabled = false;
                            SetFieldValues(ControlsEnum.DASHBOARDDETAILS);
                            ResetForm(ControlsEnum.CLEARDASHBOARDDETAILS);

                            #endregion

                            SetUIValuesToObject(ControlsEnum.SAVEDASHLETMAPPINGGRID);
                            SetFieldValues(ControlsEnum.DASHLETMAPPINGGRIDLIST);

                            GetFieldValues(ControlsEnum.DASHLET);
                            SetFieldValues(ControlsEnum.DASHLET);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_AddOnlyOneDashlet").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region  CLOSE MENU POPUP
                    case ActionsEnum.CANCELPOPUP:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        break;
                    #endregion
                    #region  CLOSE DASHLET POPUP
                    case ActionsEnum.CANCELDPOPUP:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEARFILTER);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CHANGE SEQUENCE PopUp Menu
                    case ActionsEnum.CHANGESEQUENCE:

                        List<DashboardItemDetails> DashItemTemp = DashboardItemDetailsList.OrderBy(x => x.DBL_SEQUENCE).ToList();
                        List<DashboardItemDetails> listDashItemGroup = DashItemTemp.Where(x => (x.DBL_SL_NO == Convert.ToInt32(hdfCurGroupSlNo_PopUp.Value)
                           ) && x.DBL_TYPE != Convert.ToInt16(PopupValuesEnum.DASHLETVALUE)).ToList();

                        int curSEQUENCE = Convert.ToInt32((sender as ImageButton).CommandArgument);
                        DashboardItemDetails currLine = listDashItemGroup.Where(rl => rl.DBL_SEQUENCE == curSEQUENCE).First();
                        int lineIndex = listDashItemGroup.FindIndex(rl => rl.DBL_SEQUENCE == curSEQUENCE);
                        DashboardItemDetails nextLine = new DashboardItemDetails();
                        if (((ImageButton)sender).ID == "imbRuleUpPop")  // 
                            nextLine = lineIndex == 0 ? null : listDashItemGroup.ElementAt(lineIndex - 1);
                        else if (((ImageButton)sender).ID == "imbRuleDownPop")  // 
                            nextLine = lineIndex == listDashItemGroup.Count - 1 ? null : listDashItemGroup.ElementAt(lineIndex + 1);
                        if (nextLine != null)
                        {
                            int currentlineNumber = currLine.DBL_SEQUENCE;
                            currLine.DBL_SEQUENCE = nextLine.DBL_SEQUENCE;
                            nextLine.DBL_SEQUENCE = currentlineNumber;
                        }
                        DashboardItemDetailsList = DashItemTemp
                             .OrderBy(x => x.DBL_SEQUENCE)
                             .ToList();
                        SetFieldValues(ControlsEnum.MENUMAPPINGGRIDLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpGroupItemDetails]','" + GetLocalResourceObject("GroupMenuMapping").ToString() + "','660','540');", true);
                        break;
                    #endregion
                    #region CHANGE SEQUENCE PopUp Dashlet
                    case ActionsEnum.CHANGEDASHLETSEQUENCE:

                        List<DashboardItemDetails> DashletItemTemp = DashboardItemDetailsList.OrderBy(x => x.DBL_SEQUENCE).ToList();
                        List<DashboardItemDetails> listDashletItemGroup = DashletItemTemp.Where(x => x.DBL_SL_NO == Convert.ToInt32(hdfDashCurGroupSlNo_PopUp.Value) &&
                            x.DBL_TYPE == Convert.ToInt16(PopupValuesEnum.DASHLETVALUE)).ToList();

                        int curDashSEQUENCE = Convert.ToInt32((sender as ImageButton).CommandArgument);
                        DashboardItemDetails currDashLine = listDashletItemGroup.Where(rl => rl.DBL_SEQUENCE == curDashSEQUENCE).First();
                        int lineDashIndex = listDashletItemGroup.FindIndex(rl => rl.DBL_SEQUENCE == curDashSEQUENCE);
                        DashboardItemDetails nextDashLine = new DashboardItemDetails();
                        if (((ImageButton)sender).ID == "imbRuleUpPop")  // 
                            nextDashLine = lineDashIndex == 0 ? null : listDashletItemGroup.ElementAt(lineDashIndex - 1);
                        else if (((ImageButton)sender).ID == "imbRuleDownPop")  // 
                            nextDashLine = lineDashIndex == listDashletItemGroup.Count - 1 ? null : listDashletItemGroup.ElementAt(lineDashIndex + 1);
                        if (nextDashLine != null)
                        {
                            int currentlineNumber = currDashLine.DBL_SEQUENCE;
                            currDashLine.DBL_SEQUENCE = nextDashLine.DBL_SEQUENCE;
                            nextDashLine.DBL_SEQUENCE = currentlineNumber;
                        }
                        DashboardItemDetailsList = DashletItemTemp
                             .OrderBy(x => x.DBL_SEQUENCE)
                             .ToList();
                        SetFieldValues(ControlsEnum.DASHLETMAPPINGGRIDLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupItemDashletDetails]','" + GetLocalResourceObject("DashletMenuMapping").ToString() + "','820','540');", true);
                        break;
                    #endregion
                    #region  MODE CHANGE
                    case ActionsEnum.MODECHANGE:
                        if (Convert.ToInt16(ddlModeHd.SelectedValue) == Convert.ToInt16(PopupValuesEnum.DASHLET))
                        {
                            txtNameHd.Text = string.Empty;
                            txtNameHd.Enabled = false;
                            txtNameHd.CssClass = "input-half input-disabled";
                            txtNameGd.Enabled = false;
                            txtNameGd.CssClass = "input-half input-disabled";
                            rfvNameHd.Enabled = false;
                            txtDescriptionGd.Enabled = false;
                            txtDescriptionGd.CssClass = "input-w48-7per input-disabled";

                            imgAdd.Visible = false;
                            imgbtnMappingPopup.Visible = true;
                        }
                        else
                        {
                            txtNameHd.Enabled = true;
                            txtNameHd.CssClass = "input-half";
                            txtNameGd.Enabled = true;
                            txtNameGd.CssClass = "input-half";
                            rfvNameHd.Enabled = true;
                            txtDescriptionGd.Enabled = true;
                            txtDescriptionGd.CssClass = "input-w48-7per";
                            imgAdd.Visible = true;
                            imgbtnMappingPopup.Visible = false;
                        }
                        break;
                    #endregion
                    #region SHOW DASHLET MAPPING Popup
                    case ActionsEnum.SHOWDASHLETMAPPING:
                        string groupName = string.Empty;
                        if (DashboardDetailsList != null && DashboardDetailsList.Count > 0)
                        {
                            #region Edit Mode
                            ResetForm(ControlsEnum.CLEARDASHBOARDDETAILS);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideGroupDetails", "ShowHideGroupDetails(1);", true);
                            var editItem = DashboardDetailsList.FirstOrDefault();    //Dashlet contains only one group at a time                         
                            if (editItem != null)
                            {
                                CurrDashGroupPK = editItem.DBD_PK;
                                hdfgrdDtailsCurSeqNo.Value = editItem.DBD_SEQUENCE.ToString();
                                hdfgvdDtailsSiNo.Value = editItem.DBD_SL_NO.ToString(); ;
                                groupName = editItem.DBD_NAME.ToString().HtmlDecode();
                            }
                            #endregion
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideGroupDetails", "ShowHideGroupDetails(1);", true);
                        hdfDashCurGroupSlNo_PopUp.Value = hdfgvdDtailsSiNo.Value;
                        hdfDashCurDBDPK_PopUp.Value = CurrDashGroupPK.ToString();
                        lblDashletPopup.Text = ERP.Utilities.CommonFunctions.GetShortString((string.Format(GetLocalResourceObject("PopUpMenuHead").ToString(), txtNameHd.Text)), 40);
                        lblGrpDashltPopup.Text = ERP.Utilities.CommonFunctions.GetShortString((string.Format(GetLocalResourceObject("PopUpGroupHead").ToString(), groupName)), 30);
                        GetFieldValues(ControlsEnum.DASHLET);
                        SetFieldValues(ControlsEnum.DASHLET);
                        SetFieldValues(ControlsEnum.DASHLETMAPPINGGRIDLIST);
                        SetFieldValues(ControlsEnum.DASHBOARDDETAILS);
                        if (EntryStatus == EntryStatus.EDITMODE && DashboardItemDetailsList.Count > 0
                            && !string.IsNullOrEmpty(DashboardItemDetailsList[0].DLC_DTL_QUERY))//ned to changes if multiple item adding
                        {
                            DbsSlNo = DashboardItemDetailsList[0].DBL_SL_NO;
                            BindMoreinfoGrid(DashboardItemDetailsList[0].DLC_DTL_QUERY);
                            SetMappedMoreinfoGrid(DashboardItemDetailsList[0].lstItemTrxDetails);
                        }

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupItemDashletDetails]','" + GetLocalResourceObject("DashletMenuMapping").ToString() + "','820','540');", true);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            if (senderGridView.ID == "grdGroupDetails")
            {
                SetUIValuesToObject(ControlsEnum.DASHBOARDGROUPDETAILSTOLIST);
                if (e.CommandName == "EDIT_ACTION")
                {
                    ResetForm(ControlsEnum.CLEARDASHBOARDDETAILS);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideGroupDetails", "ShowHideGroupDetails(1);", true);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfDBD_PK = row.FindControl("hdfDBD_PK") as HiddenField;
                    HiddenField hdfDBD_SLNO = row.FindControl("hdfDBD_SLNO") as HiddenField;
                    var editItem = DashboardDetailsList.Where(itm => itm.DBD_SL_NO == Convert.ToInt32(hdfDBD_SLNO.Value)).FirstOrDefault();
                    if (editItem != null)
                    {
                        CurrDashGroupPK = editItem.DBD_PK;
                        hdfgrdDtailsCurSeqNo.Value = editItem.DBD_SEQUENCE.ToString();
                        hdfgvdDtailsSiNo.Value = editItem.DBD_SL_NO.ToString(); ;
                        txtNameGd.Text = editItem.DBD_NAME.ToString().HtmlDecode();
                        txtDescriptionGd.Text = editItem.DBD_DESC.ToString().HtmlDecode();
                        //txtSeqGd.Text = editItem.DBD_SEQUENCE.ToString();
                        chkActiveGd.Checked = editItem.DBD_ACTIVE == 1 ? true : false;
                        SetFieldValues(ControlsEnum.DASHBOARDDETAILS);
                    }

                }
                else if (e.CommandName == "DELETE_ACTION")
                {
                    ResetForm(ControlsEnum.CLEARDASHBOARDDETAILS);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfDBD_PK = row.FindControl("hdfDBD_PK") as HiddenField;
                    HiddenField hdfDBD_SLNO = row.FindControl("hdfDBD_SLNO") as HiddenField;
                    DashboardDetails detail = DashboardDetailsList
                        .Where(x => x.DBD_SL_NO == Convert.ToInt32(hdfDBD_SLNO.Value) && x.DBD_PK == Convert.ToInt32(hdfDBD_PK.Value))
                        .SingleOrDefault();
                    if (detail != null)
                    {
                        DashboardDetailsList.Remove(detail);
                        ResetForm(ControlsEnum.CLEARDASHBOARDDETAILS);
                        SetFieldValues(ControlsEnum.DASHBOARDDETAILS);
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailed;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }

                }
                else if (e.CommandName == "MENUMAPPING")
                {
                    ResetForm(ControlsEnum.CLEARPOPUP);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideGroupDetails", "ShowHideGroupDetails(1);", true);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfDBD_PK = row.FindControl("hdfDBD_PK") as HiddenField;
                    HiddenField hdfDBD_SLNO = row.FindControl("hdfDBD_SLNO") as HiddenField;
                    Label lblGvNameGd = row.FindControl("lblGvNameGd") as Label;
                    hdfCurGroupSlNo_PopUp.Value = hdfDBD_SLNO.Value;
                    hdfCurRGroupSlNo_PopUp.Value = hdfDBD_SLNO.Value;
                    hdfCurDBDPK_PopUp.Value = hdfDBD_PK.Value;
                    hdfCurRDBDPK_PopUp.Value = hdfDBD_PK.Value;
                    lblDashHdPopup.Text = ERP.Utilities.CommonFunctions.GetShortString((string.Format(GetLocalResourceObject("PopUpMenuHead").ToString(), txtNameHd.Text)), 40);
                    lblGroupHdPopup.Text = ERP.Utilities.CommonFunctions.GetShortString((string.Format(GetLocalResourceObject("PopUpGroupHead").ToString(), lblGvNameGd.Text)), 30);
                    GetFieldValues(ControlsEnum.DEPT);
                    SetFieldValues(ControlsEnum.DEPT);
                    GetFieldValues(ControlsEnum.MISREPORTS);
                    SetFieldValues(ControlsEnum.MISREPORTS);
                    GetFieldValues(ControlsEnum.MENU);
                    SetFieldValues(ControlsEnum.MENU);
                    SetFieldValues(ControlsEnum.MENUMAPPINGGRIDLIST);
                    SetFieldValues(ControlsEnum.DASHBOARDDETAILS);
                    divReportPopupDetails.Visible = false;
                    divMenuPopupDetails.Visible = true;
                    lnkMISReport.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkMenu.CssClass = GetLocalResourceObject("TabActive").ToString();
                    grdReportPopup.DataSource = null;
                    grdReportPopup.DataBind();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpGroupItemDetails]','" + GetLocalResourceObject("GroupMenuMapping").ToString() + "','660','540');", true);
                }
                else if (e.CommandName == "DASHLETMAPPING")
                {
                    ResetForm(ControlsEnum.CLEARPOPUP);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideGroupDetails", "ShowHideGroupDetails(1);", true);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfDBD_PK = row.FindControl("hdfDBD_PK") as HiddenField;
                    HiddenField hdfDBD_SLNO = row.FindControl("hdfDBD_SLNO") as HiddenField;
                    Label lblGvNameGd = row.FindControl("lblGvNameGd") as Label;
                    hdfDashCurGroupSlNo_PopUp.Value = hdfDBD_SLNO.Value;
                    hdfDashCurDBDPK_PopUp.Value = hdfDBD_PK.Value;
                    lblDashletPopup.Text = ERP.Utilities.CommonFunctions.GetShortString((string.Format(GetLocalResourceObject("PopUpMenuHead").ToString(), txtNameHd.Text)), 40);
                    lblGrpDashltPopup.Text = ERP.Utilities.CommonFunctions.GetShortString((string.Format(GetLocalResourceObject("PopUpGroupHead").ToString(), lblGvNameGd.Text)), 30);
                    GetFieldValues(ControlsEnum.DASHLET);
                    SetFieldValues(ControlsEnum.DASHLET);
                    SetFieldValues(ControlsEnum.DASHLETMAPPINGGRIDLIST);
                    SetFieldValues(ControlsEnum.DASHBOARDDETAILS);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupItemDashletDetails]','" + GetLocalResourceObject("DashletMenuMapping").ToString() + "','820','540');", true);
                }
            }
            else if (senderGridView.ID == "grdMenuPopUpList")
            {
                if (e.CommandName == "DELETE_ACTION")
                {
                    ResetForm(ControlsEnum.CLEARDASHBOARDDETAILS);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfMNUPK_PopUpList = row.FindControl("hdfMNUPK_PopUpList") as HiddenField;
                    HiddenField hdfDBL_SLNO_PopUpList = row.FindControl("hdfDBL_SLNO_PopUpList") as HiddenField;
                    HiddenField hdfMNUACTION_URL_PopUpList = row.FindControl("hdfMNUACTION_URL_PopUpList") as HiddenField;
                    List<DashboardItemDetails> detail = DashboardItemDetailsList
                          .Where(x => x.DBL_SL_NO == Convert.ToInt32(hdfDBL_SLNO_PopUpList.Value) && x.DBL_MENU_CFG == Convert.ToInt32(hdfMNUPK_PopUpList.Value))
                          .ToList();
                    if (detail != null && detail.Count > 0)
                    {
                        if (detail.Count == 1)
                            DashboardItemDetailsList.Remove(detail[0]);
                        else    //MIS Reports
                        {
                            DashboardItemDetails Misdetail = detail.Where(x => x.DBL_LINK == hdfMNUACTION_URL_PopUpList.Value).SingleOrDefault();
                            DashboardItemDetailsList.Remove(Misdetail);
                        }
                        SetFieldValues(ControlsEnum.MENUMAPPINGGRIDLIST);
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailed;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpGroupItemDetails]','" + GetLocalResourceObject("GroupMenuMapping").ToString() + "','660','540');", true);
                }

            }
            else if (senderGridView.ID == "grdDashletPopupList")
            {
                if (e.CommandName == "DELETE_ACTION")
                {
                    ResetForm(ControlsEnum.CLEARDASHBOARDDETAILS);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfDashPK_PopUpList = row.FindControl("hdfDashPK_PopUpList") as HiddenField;
                    HiddenField hdfDBL_SLNO_PopUpList = row.FindControl("hdfDashDBL_SLNO_PopUpList") as HiddenField;
                    DashboardItemDetails detail = DashboardItemDetailsList
                        .Where(x => x.DBL_SL_NO == Convert.ToInt32(hdfDBL_SLNO_PopUpList.Value) && x.DBL_DASHLET == Convert.ToInt32(hdfDashPK_PopUpList.Value))
                        .SingleOrDefault();
                    if (detail != null)
                    {
                        DashboardItemDetailsList.Remove(detail);
                        SetFieldValues(ControlsEnum.DASHLETMAPPINGGRIDLIST);
                        #region Deleting its group also
                        int dbdPK = detail.DBL_DASH_DTL;
                        DashboardDetails dbdDetail = DashboardDetailsList.Where(x => x.DBD_PK == Convert.ToInt32(dbdPK)).SingleOrDefault();
                        if (dbdDetail != null)
                        {
                            DashboardDetailsList.Remove(dbdDetail);
                            ResetForm(ControlsEnum.CLEARDASHBOARDDETAILS);
                            SetFieldValues(ControlsEnum.DASHBOARDDETAILS);
                        }
                        #endregion
                        #region Clear More Info
                        ItemTrxDetailsList = null;
                        divMoreInfo.Visible = false;
                        grdMoreDetails.DataSource = null;
                        grdMoreDetails.DataBind();

                        #endregion
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailed;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupItemDashletDetails]','" + GetLocalResourceObject("DashletMenuMapping").ToString() + "','820','540');", true);
                }

            }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((GridView)sender).ID == "grdGroupDetails")
                {
                    ImageButton imgMenu = e.Row.FindControl("imbEditItemDetails") as ImageButton;
                    ImageButton imgDashlet = e.Row.FindControl("imbEditDetailsDashlet") as ImageButton;
                    ImageButton imbEditDetails = e.Row.FindControl("imbEditDetails") as ImageButton;

                    if (Convert.ToInt16(ddlModeHd.SelectedValue) == Convert.ToInt16(PopupValuesEnum.MENU))
                    {
                        imgMenu.Visible = true;
                        imgDashlet.Visible = false;
                    }
                    else
                    {
                        imgDashlet.Visible = false;
                        imgMenu.Visible = false;
                        imbEditDetails.Visible = false;
                    }
                }
                else if (((GridView)sender).ID == "grdDashletPopUp")
                {
                    DropDownList ddlDefult = e.Row.FindControl("ddlDefult") as DropDownList;
                    HiddenField hdfQueryText = e.Row.FindControl("hdfQueryText") as HiddenField;
                    if (!string.IsNullOrEmpty(hdfQueryText.Value))
                    {
                        string strQuery = hdfQueryText.Value;
                        if (strQuery.Contains(GetLocalResourceObject("ParamUserPK").ToString()))
                        {
                            strQuery = strQuery.Replace(GetLocalResourceObject("ParamUserPK").ToString(), currentUser.PKUser.ToString());
                        }
                        DataTable dtDefult = BusinessLogic.Administration.Masters.DashboardSetupBL.GetDashBoardDefultValues(strQuery);
                        ddlDefult.DataSource = dtDefult;
                        ddlDefult.DataTextField = GTIService.Constants.Common.Common.F_VALUE;
                        ddlDefult.DataValueField = GTIService.Constants.Common.Common.F_PK;
                        ddlDefult.DataBind();
                        ddlDefult.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    }
                    else
                    {
                        ddlDefult.Visible = false;
                    }
                }
            }
        }
        #endregion

        #region Pager Methods + Init
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                #region In the case of dashlet,hide edit,delete & mapping buttons of GroupDetails Grid
                if (grdGroupDetails.Rows != null)
                {
                    foreach (GridViewRow gvr in grdGroupDetails.Rows)
                    {
                        ImageButton imbEditDetails = gvr.FindControl("imbEditDetails") as ImageButton;
                        ImageButton imgMenu = gvr.FindControl("imbEditItemDetails") as ImageButton;
                        ImageButton imgDashlet = gvr.FindControl("imbEditDetailsDashlet") as ImageButton;
                        ImageButton imbDeleteRejectedDetails = gvr.FindControl("imbDeleteRejectedDetails") as ImageButton;

                        if (Convert.ToInt16(ddlModeHd.SelectedValue) == Convert.ToInt16(PopupValuesEnum.MENU))
                        {
                            imgMenu.Visible = true;
                            imgDashlet.Visible = false;
                        }
                        else
                        {
                            imgDashlet.Visible = false;
                            imgMenu.Visible = false;
                            imbEditDetails.Visible = false;
                            imbDeleteRejectedDetails.Visible = false;
                        }
                    }
                }
                #endregion

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideGroupDetails", "ShowHideGroupDetails();", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                PgerControlNew pagerControl = (PgerControlNew)sender;
                string senderId = pagerControl.ID;
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        pagerControl.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage--;
                        break;
                }
                if (senderId == "uclPaging")
                {
                    PageIndex = uclPaging.CurrentPage;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
        }
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {


            LIST,
            CLEAR,
            TYPE,
            THEME,
            EDIT,
            ADDDASHBOARDHDR,
            DASHBOARDDETAILS,
            CLEARADDTOLIST,
            DASHBOARDMASTERDETAILS,
            DASHBOARDGROUPDETAILS,
            DASHBOARDGROUPITEMDETAILS,
            CLEARDASHBOARDDETAILS,
            MENU,
            REPORT,
            DEPT,
            DASHLET,
            CLEARPOPUP,
            SAVEMENUMAPPINGGRID,
            SAVEDASHLETMAPPINGGRID,
            MENUMAPPINGGRIDLIST,
            DASHLETMAPPINGGRIDLIST,
            CLEARFILTER,
            DASHBOARDGROUPDETAILSTOLIST,
            MISREPORTS,
            MODE,
            MOREINFO

        }

        public enum PopupValuesEnum
        {
            MENUVALUE = 1,
            DASHLETVALUE = 2,
            RPTVALUE = 3,
            DEPT = 8,
            MNGPK = 92,
            MENU = 2,
            DASHLET = 3
        }
        #endregion
    }
}