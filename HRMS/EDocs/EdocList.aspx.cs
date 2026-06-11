#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using System.Data;
using BusinessObject.Common;
using ERP.Utilities;
using System.Web.UI.HtmlControls;
using BusinessLogic.HRMS.eDocs;
using BusinessObject.CommonManagement;
using BusinessObject.HRMS.eDocs;
using BusinessLogic.HRMS.Common;
using System.IO;
using HRMS.UserControls;
using System.Reflection;
#endregion

namespace HRMS.EDocs
{
    public partial class EdocList : System.Web.UI.Page
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
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.PageIndex] == null ? 1 : Convert.ToInt32(this.ViewState[ViewstateStrings.PageIndex]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }

        /// <summary>
        /// To maintain the PageSize in viewstate
        /// </summary>
        private int PageSize
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.PageSize] ?? Convert.ToInt32(GetLocalResourceObject("PageSize").ToString()));
            }
            set
            {
                this.ViewState[ViewstateStrings.PageSize] = value;
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

        public PageTabs CurrentTab
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrTab] == null
                    ? PageTabs.Summary
                    : (PageTabs)this.ViewState[ViewstateStrings.CurrTab];
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrTab] = value;
            }
        }

        private string ProjectPk
        {
            get
            {
                int dummy = -1;
                return Request.QueryString[ERP.Utilities.QueryStrings.ProjID] == null || !Int32.TryParse(Request.QueryString[ERP.Utilities.QueryStrings.ProjID].Trim(), out dummy)
                    ? "-1"
                    : Request.QueryString[ERP.Utilities.QueryStrings.ProjID].Trim();
            }
        }
        #endregion

        #region Variables
        BusinessObject.User currentUser;
        private ActionsEnum commonActions;
        RadioButton rbtn;
        DataTable dtPageData;
        DataTable dtEmployees;
        string @xml = string.Empty;
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
                if (!IsPostBack)
                {
                    FileExplorer1.DisplayMode = FolderBrowserDisplayMode.Collapsed;
                    FileExplorer1.ShowComandBar = false;
                    FileExplorer1.ShowMaximumCharacters = Convert.ToInt32(GetLocalResourceObject("MaximumCharacrers").ToString());
                    // -1 -> Summary, 0 -> Pending, 1 -> InProgress, 2 -> Finalized, 3 -> All
                    PageTabs parmPagTab;
                    if (Request.QueryString[ERP.Utilities.QueryStrings.TabID] != null
                        && Enum.TryParse<PageTabs>(Request.QueryString[ERP.Utilities.QueryStrings.TabID].Trim(), out parmPagTab))
                        this.CurrentTab = parmPagTab;

                    GetFieldValues(ControlsEnum.PROJECT);
                    SetFieldValues(ControlsEnum.PROJECT);
                    GetFieldValues(ControlsEnum.STATUS);
                    SetFieldValues(ControlsEnum.STATUS);
                    GetFieldValues(ControlsEnum.FROM);
                    SetFieldValues(ControlsEnum.FROM);
                    GetFieldValues(ControlsEnum.TO);
                    SetFieldValues(ControlsEnum.TO);
                    GetFieldValues(ControlsEnum.DEPARTMENT);
                    SetFieldValues(ControlsEnum.DEPARTMENT);
                    GetFieldValues(ControlsEnum.EMPLOYEES);
                    SetFieldValues(ControlsEnum.EMPLOYEES);
                    ControlsEnum ctrlEnum = GetCurrentControlsEnum();
                    GetFieldValues(ctrlEnum);
                    SetFieldValues(ctrlEnum);
                    txtSearchText.Focus();
                    //uclPagingPending.CurrentPage = 1;
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {
                switch (type)
                {
                    case ControlsEnum.SUMMARYLIST:
                        dtPageData = EDocManagementBL.GetEdocsSummary(currentUser.PKUser, currentUser.SBUID);
                        break;
                    case ControlsEnum.PENDINGLIST:
                        dtPageData = EDocManagementBL.GetEdocs(CreateEDocSearchParameter());
                        break;
                    case ControlsEnum.INPROGRESSLIST:
                        dtPageData = EDocManagementBL.GetEdocs(CreateEDocSearchParameter());
                        break;
                    case ControlsEnum.FINALIZEDLIST:
                        dtPageData = EDocManagementBL.GetEdocs(CreateEDocSearchParameter());
                        break;
                    case ControlsEnum.SHOWALLLIST:
                        dtPageData = EDocManagementBL.GetEdocs(CreateEDocSearchParameter());
                        break;
                    case ControlsEnum.SHOWFILESLIST:
                        dtPageData = EDocManagementBL.GetEdocs(CreateEDocSearchParameter());
                        break;
                    case ControlsEnum.PROJECT:
                        dtPageData = EDocManagementBL.GetUsedProjectsOrSite();
                        break;
                    case ControlsEnum.STATUS:
                        dtPageData = HRMSCommonBL.GetHrmsCommonConfigMst("DMS DOC TRX STATUS", null, currentUser.SBUID, 0, 1);
                        break;
                    case ControlsEnum.FROM:
                        dtPageData = EDocManagementBL.GetFromTo(0, 0, ConstGroupType.EDocSetUp, 1, (int)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    case ControlsEnum.TO:
                        dtPageData = EDocManagementBL.GetFromTo(0, 0, ConstGroupType.EDocSetUp, 1, (int)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    case ControlsEnum.DEPARTMENT:
                        dtPageData = EDocManagementBL.GetFromTo(0, 0, ConstGroupType.EDocSetUp, 2, (int)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    case ControlsEnum.FOLDER:
                        @xml = EDocManagementBL.GetFolderTreeXml(0, currentUser.SBUID);
                        break;
                    case ControlsEnum.EMPLOYEES:
                        dtEmployees = EDocManagementBL.GetSendToUsers(currentUser.PKUser, (int)DbActiveStatus.ACTIVE);
                        //dtEmployees = EDocManagementBL.GetAutoEdocEmployee("");

                        break;
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

        private EDocSearchParameter CreateEDocSearchParameter()
        {
            return new BusinessObject.HRMS.eDocs.EDocSearchParameter
            {
                GridParams = new BusinessObject.GridPrams()
                                    {
                                        SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.EDocDate : SortBy,
                                        SortDirection = string.IsNullOrEmpty(SortDirection)
                                                    ? Resources.Report.SortDescending
                                                    : SortDirection,
                                        ThenBy = SortBy == ThenBy || SortBy == Resources.DataFieldRes.EDocNo
                                                    ? string.Empty
                                                    : string.IsNullOrEmpty(ThenBy)
                                                        ? Resources.DataFieldRes.EDocNo
                                                        : ThenBy,
                                        ThenDirection = SortBy == ThenBy || SortBy == Resources.DataFieldRes.EDocNo
                                                    ? string.Empty
                                                    : string.IsNullOrEmpty(ThenDirection)
                                                        ? Resources.Report.SortDescending
                                                        : ThenDirection,
                                        FromDate = string.IsNullOrEmpty(txtSearchDateFrom.Text.Trim())
                                                    ? string.Empty
                                                    : txtSearchDateFrom.Text.Trim(),
                                        ToDate = string.IsNullOrEmpty(txtSearchDateTo.Text.Trim())
                                                    ? string.Empty
                                                    : txtSearchDateTo.Text.Trim(),
                                        SearchBy = Resources.DataFieldRes.EDocNo,
                                        SearchValue = string.IsNullOrEmpty(txtSearchEDocNo.Text.Trim())
                                                    ? string.Empty
                                                    : txtSearchEDocNo.Text.Trim(),
                                        PageNumber = Convert.ToInt32(this.PageIndex),
                                        PageSize = this.PageSize,
                                        UserPK = this.currentUser.PKUser
                                    },
                BizUnit = currentUser.SBUID,
                Comments = txtSearchComment.Text.Trim(),
                DocNo = txtSearchEDocNo.Text.Trim(),
                LetterNo = txtSearchLetterNo.Text.Trim(),
                ProjectSite = this.ProjectPk != "-1"
                                ? Convert.ToInt32(this.ProjectPk)
                                : ddlProjectSite.SelectedValue != null || ddlProjectSite.SelectedValue != "-1"
                                    ? Convert.ToInt32(ddlProjectSite.SelectedValue)
                                    : -1,
                Department = ddlDepartment.SelectedValue != null || ddlDepartment.SelectedValue != "-1"
                                   ? Convert.ToInt32(ddlDepartment.SelectedValue)
                                   : -1,
                Status = (int)this.CurrentTab,
                //Status = this.CurrentTab == PageTabs.All
                //                                ? ddlSearchStatus.SelectedValue != null && ddlSearchStatus.SelectedValue != CommonConstants.SELECTVAL
                //                                    ? Convert.ToInt32(ddlSearchStatus.SelectedValue)
                //                                    : (int)this.CurrentTab
                //                                : (int)this.CurrentTab,
                DocStatus = this.CurrentTab == PageTabs.All
                                                ? ddlSearchStatus.SelectedValue != null && ddlSearchStatus.SelectedValue != CommonConstants.SELECTVAL
                                                    ? Convert.ToInt32(ddlSearchStatus.SelectedValue)
                                                    : -1
                                                : -1,
                FileMode = this.CurrentTab == PageTabs.Files ? true : false,
                FileText = this.CurrentTab == PageTabs.Files ? txtSearchFileTitle.Text.TrimStart() : null,
                Subject = txtSearchSubject.Text.Trim(),
                Tags = txtSearchTags.Text.Trim(),
                SearchText = string.IsNullOrWhiteSpace(txtSearchText.Text) ? string.Empty : txtSearchText.Text.Trim(),
                Folder = FileExplorer1.SelectedPk,
                From = ddlFrom.SelectedValue != null || ddlFrom.SelectedValue != "-1"
                                   ? Convert.ToInt32(ddlFrom.SelectedValue)
                                   : -1,
                To = ddlTo.SelectedValue != null || ddlTo.SelectedValue != "-1"
                                   ? Convert.ToInt32(ddlTo.SelectedValue)
                                   : -1,
                Send = Convert.ToInt32(hdfSender.Value) > 0 ? Convert.ToInt32(hdfSender.Value)
                                   : -1,
                SendTo = Convert.ToInt32(hdfSendTo.Value)>0
                                   ? Convert.ToInt32(hdfSendTo.Value)
                                   : -1,
                //Send = hdfSender.Value != null || hdfSender.Value != "-1"
                //                    ? Convert.ToInt32(hdfSender.Value)
                //                    : -1,
                //SendTo = hdfSendTo.Value != null || hdfSendTo.Value  != "-1"
                //? Convert.ToInt32(hdfSendTo.Value)
                //: -1,
            };

        }

        public string GetPhysicalPath(object myEvalObj, object projectPK)
        {
            try
            {
                string link = myEvalObj.ToString();
                //string FilePath = "../Upload/"; // Commented by Biju
                string FilePath = "~/Upload/EDocs/" + projectPK.ToString() + "/";
                FileInfo file = new FileInfo(link);
                link = FilePath + file.Name;
                return link;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private TrxStatus GetStatusFromCurrentTab()
        {
            switch (this.CurrentTab)
            {
                case PageTabs.Pending:
                    return TrxStatus.Pending;
                case PageTabs.InProgress:
                    return TrxStatus.Drafted;
                case PageTabs.Finalize:
                    return TrxStatus.Completed;
                case PageTabs.All:
                case PageTabs.Files:
                    return TrxStatus.None;
            }

            return TrxStatus.None;
        }

        private GridView GetActiveTabGrid()
        {
            switch (this.CurrentTab)
            {
                case PageTabs.Pending:
                    //hdfFieldName = "hdfPendingDocPk";
                    return grdPendingDocList;
                case PageTabs.InProgress:
                    // hdfFieldName = "";
                    return grdInProgressDocList;
                case PageTabs.Finalize:
                    //hdfFieldName = "";
                    return grdFinalizedDocList;
                case PageTabs.All:
                    //hdfFieldName = "";
                    return grdAllDocList;
                case PageTabs.Files:
                    return grdFilesList;
            }
            //hdfFieldName = string.Empty;
            return null;
        }

        private ControlsEnum GetCurrentControlsEnum()
        {
            switch (this.CurrentTab)
            {
                case PageTabs.Summary:
                    return ControlsEnum.SUMMARYLIST;
                case PageTabs.Pending:
                    return ControlsEnum.PENDINGLIST;
                case PageTabs.InProgress:
                    return ControlsEnum.INPROGRESSLIST;
                case PageTabs.Finalize:
                    return ControlsEnum.FINALIZEDLIST;
                case PageTabs.All:
                    return ControlsEnum.SHOWALLLIST;
                case PageTabs.Files:
                    return ControlsEnum.SHOWFILESLIST;
            }

            return ControlsEnum.SUMMARYLIST;
        }

        private PgerControlNew GetCurrentPager()
        {
            switch (this.CurrentTab)
            {
                case PageTabs.Pending:
                    return uclPagingPending;
                case PageTabs.InProgress:
                    return uclPagingInProgress;
                case PageTabs.Finalize:
                    return uclPagingFinalized;
                case PageTabs.All:
                    return uclPagingAllDocs;
                case PageTabs.Files:
                    return uclPagingFiles;
            }
            return null;
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
                    case ControlsEnum.SUMMARYLIST:
                        BindRepeater(ControlsEnum.SUMMARYLIST);
                        break;
                    case ControlsEnum.PENDINGLIST:
                        BindGrid(ControlsEnum.PENDINGLIST);
                        break;
                    case ControlsEnum.INPROGRESSLIST:
                        BindGrid(ControlsEnum.INPROGRESSLIST);
                        break;
                    case ControlsEnum.FINALIZEDLIST:
                        BindGrid(ControlsEnum.FINALIZEDLIST);
                        break;
                    case ControlsEnum.SHOWALLLIST:
                        BindGrid(ControlsEnum.SHOWALLLIST);
                        break;
                    case ControlsEnum.SHOWFILESLIST:
                        BindGrid(ControlsEnum.SHOWFILESLIST);
                        break;
                    case ControlsEnum.PROJECT:
                        BindDropDown(ControlsEnum.PROJECT);
                        break;
                    case ControlsEnum.STATUS:
                        BindDropDown(ControlsEnum.STATUS);
                        break;
                    case ControlsEnum.FROM:
                        BindDropDown(ControlsEnum.FROM);
                        break;
                    case ControlsEnum.TO:
                        BindDropDown(ControlsEnum.TO);
                        break;
                    case ControlsEnum.DEPARTMENT:
                        BindDropDown(ControlsEnum.DEPARTMENT);
                        break;
                    case ControlsEnum.FOLDER:
                        FileExplorer1.ShowComandBar = false;
                        FileExplorer1.SetDataSource(@xml);
                        FileExplorer1.DataBind();
                        break;
                    case ControlsEnum.EMPLOYEES:
                        BindDropDown(ControlsEnum.EMPLOYEES);
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
                case ControlsEnum.PROJECT:
                    ddlProjectSite.Items.Clear();
                    ddlProjectSite.DataSource = dtPageData;
                    ddlProjectSite.DataTextField = GTIService.Constants.HRMS.eDocs.Fields.prjName;
                    ddlProjectSite.DataValueField = GTIService.Constants.HRMS.eDocs.Fields.PprjPK;
                    ddlProjectSite.DataBind();
                    ddlProjectSite.Items.HtmlDecode();
                    ddlProjectSite.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                    break;
                case ControlsEnum.STATUS:
                    ddlSearchStatus.Items.Clear();
                    ddlSearchStatus.DataSource = dtPageData;
                    ddlSearchStatus.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.ADM_CFG_TEXT;
                    ddlSearchStatus.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.ADM_CFG_VALUE;
                    ddlSearchStatus.DataBind();
                    ddlSearchStatus.Items.HtmlDecode();
                    ddlSearchStatus.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                    break;
                case ControlsEnum.FROM:
                    ddlFrom.Items.Clear();
                    ddlFrom.DataSource = dtPageData;
                    ddlFrom.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                    ddlFrom.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                    ddlFrom.DataBind();
                    ddlFrom.Items.HtmlDecode();
                    ddlFrom.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                    break;
                case ControlsEnum.TO:
                    ddlTo.Items.Clear();
                    ddlTo.DataSource = dtPageData;
                    ddlTo.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                    ddlTo.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                    ddlTo.DataBind();
                    ddlTo.Items.HtmlDecode();
                    ddlTo.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                    break;
                case ControlsEnum.DEPARTMENT:
                    ddlDepartment.Items.Clear();
                    ddlDepartment.DataSource = dtPageData;
                    ddlDepartment.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                    ddlDepartment.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                    ddlDepartment.DataBind();
                    ddlDepartment.Items.HtmlDecode();
                    ddlDepartment.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                    break;
                case ControlsEnum.EMPLOYEES:
                //    ddlSend.Items.Clear();
                //    ddlSend.DataSource = dtEmployees;
                //    ddlSend.DataTextField = GTIService.Constants.HRMS.eDocs.Fields.usrEmployeeText;
                //    ddlSend.DataValueField = GTIService.Constants.HRMS.eDocs.Fields.usrPk;
                //    ddlSend.DataBind();
                //    ddlSend.Items.HtmlDecode();
                //    ddlSend.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                ////    break;
                ////case ControlsEnum.EMPLOYEES:
                //    ddlSendTo.Items.Clear();
                //    ddlSendTo.DataSource = dtEmployees;
                //    ddlSendTo.DataTextField = GTIService.Constants.HRMS.eDocs.Fields.usrEmployeeText;
                //    ddlSendTo.DataValueField = GTIService.Constants.HRMS.eDocs.Fields.usrPk;
                //    ddlSendTo.DataBind();
                //    ddlSendTo.Items.HtmlDecode();
                //    ddlSendTo.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                    break;
                    
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private object SetUIValuesToObject(ControlsEnum controlType)
        {

            try
            {
                Object retObject;
                retObject = null;
                //BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                switch (controlType)
                {
                }

                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = new DataTable();
            //   dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                // BizUnitConfigValue = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "SHIPPING")["ACF_VALUE"].ToString());
            }
        }
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
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
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            int rowCount = 0;
            TotalPages = 0;
            
            try
            {
                if (dtPageData.Rows.Count > 0)
                {
                    rowCount = Convert.ToInt32(dtPageData.Rows[0]["ROW_COUNT"].ToString());
                }
                TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                              (rowCount / this.PageSize) + 1;

                switch (controlType)
                {
                    #region  Pending List
                    case ControlsEnum.PENDINGLIST:
                        uclPagingPending.TotalPages = TotalPages;
                        uclPagingPending.CurrentPage = PageIndex;
                        grdPendingDocList.DataSource = dtPageData;
                        grdPendingDocList.DataBind();
                        uclPagingPending.Visible = true;
                        uclPagingPending.BindPager();
                        break;
                    #endregion
                    case ControlsEnum.INPROGRESSLIST:
                        uclPagingInProgress.TotalPages = TotalPages;
                        uclPagingInProgress.CurrentPage = PageIndex;
                        grdInProgressDocList.DataSource = dtPageData;
                        grdInProgressDocList.DataBind();
                        uclPagingInProgress.Visible = true;
                        uclPagingInProgress.BindPager();
                        break;
                    case ControlsEnum.FINALIZEDLIST:
                        uclPagingFinalized.TotalPages = TotalPages;
                        uclPagingFinalized.CurrentPage = PageIndex;
                        grdFinalizedDocList.DataSource = dtPageData;
                        grdFinalizedDocList.DataBind();
                        uclPagingFinalized.Visible = true;
                        uclPagingFinalized.BindPager();
                        break;
                    case ControlsEnum.SHOWALLLIST:
                       
                        uclPagingAllDocs.TotalPages = TotalPages;
                        uclPagingAllDocs.CurrentPage = PageIndex;
                        grdAllDocList.DataSource = dtPageData;
                        grdAllDocList.DataBind();
                        uclPagingAllDocs.Visible = true;
                        uclPagingAllDocs.BindPager();
                        break;
                    case ControlsEnum.SHOWFILESLIST:
                        uclPagingFiles.TotalPages = TotalPages;
                        uclPagingFiles.CurrentPage = PageIndex;
                        grdFilesList.DataSource = dtPageData;
                        grdFilesList.DataBind();
                        uclPagingFiles.Visible = true;
                        uclPagingFiles.BindPager();
                        break;
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
        public void BindRepeater(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region  LIST
                    case ControlsEnum.SUMMARYLIST:
                        uclPagingPending.TotalPages = TotalPages;
                        uclPagingPending.CurrentPage = PageIndex;
                        rptDocSummary.DataSource = dtPageData;
                        rptDocSummary.DataBind();
                        uclPagingPending.Visible = true;
                        uclPagingPending.BindPager();
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
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ActionsEnum action)
        {
            CurrPK = 0;

            switch (action)
            {
                case ActionsEnum.SEARCH:
                    txtSearchText.Text = string.Empty;
                    txtSearchDateFrom.Text = string.Empty;
                    txtSearchDateTo.Text = string.Empty;
                    txtSearchEDocNo.Text = string.Empty;
                    txtSearchSubject.Text = string.Empty;
                    txtSearchTags.Text = string.Empty;
                    txtSearchComment.Text = string.Empty;
                    txtSearchLetterNo.Text = string.Empty;
                    if (divFolderBrowse.Visible == true)
                        FileExplorer1.SelectedPk = 0;
                    ddlProjectSite.SelectedValue = CommonConstants.SELECTVAL;
                    ddlSearchStatus.SelectedValue = CommonConstants.SELECTVAL;
                    txtSender.Text = string.Empty;
                    hdfSender.Value = "0";
                    txtSendTo.Text = string.Empty;
                    hdfSendTo.Value = "0";
                    break;
                case ActionsEnum.SUMMARYLIST:
                    RemoveKeyQueryString(ERP.Utilities.QueryStrings.ProjID);
                    ResetForm(ActionsEnum.SEARCH);
                    rptDocSummary.DataSource = null;
                    rptDocSummary.DataBind();
                    //divFolderBrowse.Visible = false;
                    break;
                case ActionsEnum.PENDINGLIST:
                    ResetForm(ActionsEnum.SEARCH);
                    grdPendingDocList.DataSource = null;
                    grdInProgressDocList.DataBind();
                    //divFolderBrowse.Visible = false;
                    break;
                case ActionsEnum.INPROGRESSLIST:
                    ResetForm(ActionsEnum.SEARCH);
                    grdInProgressDocList.DataSource = null;
                    grdInProgressDocList.DataBind();
                    //divFolderBrowse.Visible = false;
                    break;
                case ActionsEnum.FINALIZELIST:
                    ResetForm(ActionsEnum.SEARCH);
                    grdFinalizedDocList.DataSource = null;
                    grdFinalizedDocList.DataBind();
                    //divFolderBrowse.Visible = false;
                    break;
                case ActionsEnum.SHOWALL:
                    ResetForm(ActionsEnum.SEARCH);
                    grdAllDocList.DataSource = null;
                    grdAllDocList.DataBind();
                    //divFolderBrowse.Visible = false;
                    break;
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
                ControlsEnum currentctrl;
                GridViewRow gvr;
                GridView grd;
                string arg;
                bool bIsChecked = false;
                int result;
                string action;
                int selectedItemPK;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }

                //switch (commonActions)
                switch (commonActions)
                {
                    #region Item Selected
                    case ActionsEnum.ITEMSELECTED:
                        string hdfFieldName = "hdfEDocPk";
                        grd = GetActiveTabGrid();
                        foreach (GridViewRow grdrow in grd.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                this.CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl(hdfFieldName)).Value);
                            }
                        }
                        if (!bIsChecked)
                        {
                            throw new ApplicationException("Items not selected");
                        }
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ActionsEnum.SEARCH);
                         
                        if (this.CurrentTab == PageTabs.Summary)
                        {
                            currentctrl = ControlsEnum.SHOWALLLIST;
                            this.CurrentTab = PageTabs.All;
                        }
                        else
                            currentctrl = GetCurrentControlsEnum();
                        PageIndex = 1;
                        uclPagingAllDocs.CurrentPage = 1;

                        GetFieldValues(currentctrl);
                        SetFieldValues(currentctrl);
                        break;
                    #endregion
                    #region Search
                    case ActionsEnum.SEARCH:
                       
                        if (this.CurrentTab == PageTabs.Summary)
                        {
                            currentctrl = ControlsEnum.SHOWALLLIST;
                            this.CurrentTab = PageTabs.All;
                        }
                        else
                            currentctrl = GetCurrentControlsEnum();
                        uclPagingAllDocs.CurrentPage = 1;

                        GetFieldValues(currentctrl);
                        SetFieldValues(currentctrl);
                        break;
                    #endregion
                    #region Summary List
                    case ActionsEnum.SUMMARYLIST:
                        ResetForm(ActionsEnum.SUMMARYLIST);
                        this.CurrentTab = PageTabs.Summary;
                        //uclPagingPending.CurrentPage = 1;

                        GetFieldValues(ControlsEnum.SUMMARYLIST);
                        SetFieldValues(ControlsEnum.SUMMARYLIST);
                        break;
                    #endregion
                    #region Pendinf List
                    case ActionsEnum.PENDINGLIST:
                        ResetForm(ActionsEnum.PENDINGLIST);
                        this.CurrentTab = PageTabs.Pending;
                        PageIndex = 1;
                        uclPagingPending.CurrentPage = 1;
                        GetFieldValues(ControlsEnum.PENDINGLIST);
                        SetFieldValues(ControlsEnum.PENDINGLIST);
                        break;
                    #endregion
                    #region In Progress List
                    case ActionsEnum.INPROGRESSLIST:
                        ResetForm(ActionsEnum.INPROGRESSLIST);
                        this.CurrentTab = PageTabs.InProgress;
                        PageIndex = 1;
                        uclPagingInProgress.CurrentPage = 1;
                        GetFieldValues(ControlsEnum.INPROGRESSLIST);
                        SetFieldValues(ControlsEnum.INPROGRESSLIST);
                        break;
                    #endregion
                    #region Finalize
                    case ActionsEnum.FINALIZELIST:
                        ResetForm(ActionsEnum.FINALIZELIST);
                        this.CurrentTab = PageTabs.Finalize;
                        PageIndex = 1;
                        uclPagingFinalized.CurrentPage = 1;
                        GetFieldValues(ControlsEnum.FINALIZEDLIST);
                        SetFieldValues(ControlsEnum.FINALIZEDLIST);
                        break;
                    #endregion
                    #region Show All
                    case ActionsEnum.SHOWALL:
                        ResetForm(ActionsEnum.SHOWALL);
                        this.CurrentTab = PageTabs.All;
                        PageIndex = 1;
                        uclPagingAllDocs.CurrentPage = 1;
                        GetFieldValues(ControlsEnum.SHOWALLLIST);
                        SetFieldValues(ControlsEnum.SHOWALLLIST);
                        break;
                    #endregion
                    #region Show Files
                    case ActionsEnum.SHOWFILES:
                        ResetForm(ActionsEnum.SHOWFILES);
                        this.CurrentTab = PageTabs.Files;
                        uclPagingFiles.CurrentPage = 1;
                        //divFolderBrowse.Visible = true;
                        GetFieldValues(ControlsEnum.SHOWFILESLIST);
                        SetFieldValues(ControlsEnum.SHOWFILESLIST);
                        GetFieldValues(ControlsEnum.FOLDER);
                        SetFieldValues(ControlsEnum.FOLDER);
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        Response.Redirect(Resources.PageURL.EdocCreateUrl);
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        if (this.CurrPK > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.EDocPk] = this.CurrPK;
                            Response.Redirect(Resources.PageURL.EdocCreateUrl);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        this.CurrPK = Convert.ToInt32(((HiddenField)((LinkButton)sender).Parent.Parent.FindControl("hdfEDocPk")).Value);
                        Session[ERP.Utilities.SessionStrings.EDocPk] = this.CurrPK;
                        Response.Redirect(Resources.PageURL.EdocCreateUrl, true);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
                    if (SortDirection == Resources.Report.SortAscending)
                        SortDirection = Resources.Report.SortDescending;
                    else
                        SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    //  SortDirection = Resources.Report.SortAscending;
                }
                this.PageIndex = 1;
                ControlsEnum currntCtrl = GetCurrentControlsEnum();
                GetFieldValues(currntCtrl);
                SetFieldValues(currntCtrl);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            string[] gridID = new string[] {
                "grdPendingDocList",
                "grdInProgressDocList",
                "grdFinalizedDocList",
                "grdAllDocList",
                "grdFilesList"
            };
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (gridID.Contains(((GridView)sender).ID))
                    {
                        HtmlGenericControl imbStatusIndicator = (HtmlGenericControl)e.Row.FindControl("imbStatusIndicator");
                        HiddenField hdf2 = (HiddenField)e.Row.FindControl("hdfStatus");
                        if (hdf2 != null)
                        {
                            string _statusText = hdf2.Value;
                            _statusText = _statusText.Trim();
                            string _toolTipText = string.Empty;
                            imbStatusIndicator.Attributes.Add("class", GetListImage(_statusText, out _toolTipText));
                            if (!_toolTipText.IsNullOrEmptyOrWhitespace()) imbStatusIndicator.Attributes.Add("tooltip", _toolTipText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        protected string GetListImage(string str, out string toolTip)
        {
            var result = "";
            toolTip = string.Empty;

            if (str == "0")
            {
                result = "yellow-icon";
                toolTip = "Saved";
            }
            if (str == "1")
            {
                result = "orange-icon";
                toolTip = "Sent/Forwarded";
            }
            if (str == "2")
            {
                result = "green-icon";
                toolTip = "Finalized";
            }
            if (str == "3")
            {
                result = "grey-icon";
                toolTip = "Cancelled";
            }
            return result;
        }

        protected void ActionHandler(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = sender as Repeater;
            if (rpt != null && rpt.Items.Count > 0)
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    HtmlGenericControl emptyDiv = (HtmlGenericControl)e.Item.FindControl("divEmptyTemplate");
                    emptyDiv.Style.Add("display", "none");
                }
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
            uclPagingPending.CurrentPage = 1;
            uclPagingInProgress.CurrentPage = 1;
            uclPagingFinalized.CurrentPage = 1;
            uclPagingAllDocs.CurrentPage = 1;
            uclPagingFiles.CurrentPage = 1;
        }


        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.uclPagingPending.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPending.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPending.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPending.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPending.PageChanged += new ActionHandler(this.ActionHandler);


            this.uclPagingInProgress.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingInProgress.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingInProgress.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingInProgress.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingInProgress.PageChanged += new ActionHandler(this.ActionHandler);


            this.uclPagingFinalized.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFinalized.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFinalized.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFinalized.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFinalized.PageChanged += new ActionHandler(this.ActionHandler);

            this.uclPagingAllDocs.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingAllDocs.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingAllDocs.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingAllDocs.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingAllDocs.PageChanged += new ActionHandler(this.ActionHandler);

            this.uclPagingFiles.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFiles.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFiles.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFiles.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingFiles.PageChanged += new ActionHandler(this.ActionHandler);

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
                        GetCurrentPager().CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        // Assignment the first page index.
                        if (e.CurrentPage > 1)
                            GetCurrentPager().CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assignment the last page index.
                        if (e.CurrentPage <= e.TotalPages)
                            GetCurrentPager().CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the next page index.
                        if (e.CurrentPage <= e.TotalPages)
                            GetCurrentPager().CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the previous page index.
                        if (e.CurrentPage > 1)
                            GetCurrentPager().CurrentPage--;
                        break;
                }
                PageIndex = GetCurrentPager().CurrentPage;// uclPagingPending.CurrentPage;

                ControlsEnum currntCtrl = GetCurrentControlsEnum();
                GetFieldValues(currntCtrl);
                SetFieldValues(currntCtrl);

                EnableDisableButtons(e.TotalPages);

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
            GetCurrentPager().FirstButtonEnabled = (uclPagingPending.CurrentPage == 1) ? false : true;
            GetCurrentPager().PreviousButtonEnabled = (uclPagingPending.CurrentPage == 1) ? false : true;
            GetCurrentPager().NextButtonEnabled = (uclPagingPending.CurrentPage < iTotalPages) ? true : false;
            GetCurrentPager().LastButtonEnabled = (uclPagingPending.CurrentPage < iTotalPages) ? true : false;
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
                HideTabs();
                lblSearchStatus.Visible = false;
                ddlSearchStatus.Visible = false;
                lblSearchFileTitle.Visible = false;
                txtSearchFileTitle.Visible = false;
                switch (this.CurrentTab)
                {
                    case PageTabs.Summary:
                        trSummary.Visible = true;
                        btnView.Visible = false;
                        spnSummary.Attributes["class"] = "tab-active";
                        break;
                    case PageTabs.Pending:
                        trPending.Visible = true;
                        btnView.Visible = true;
                        spnPending.Attributes["class"] = "tab-active";
                        break;
                    case PageTabs.InProgress:
                        trInProgress.Visible = true;
                        btnView.Visible = true;
                        spnInProgress.Attributes["class"] = "tab-active";
                        break;
                    case PageTabs.Finalize:
                        trFinalized.Visible = true;
                        btnView.Visible = true;
                        spnFinalized.Attributes["class"] = "tab-active";
                        break;
                    case PageTabs.All:
                        trAll.Visible = true;
                        btnView.Visible = true;
                        lblSearchStatus.Visible = true;
                        ddlSearchStatus.Visible = true;
                        spnAll.Attributes["class"] = "tab-active";
                        break;
                    case PageTabs.Files:
                        trFiles.Visible = true;
                        btnView.Visible = false;
                        divFolderBrowse.Visible = true;
                        //divFolderBrowse.Attributes["style"] = "display:inline";
                        lblSearchFileTitle.Visible = true;
                        txtSearchFileTitle.Visible = true;
                        spnFiles.Attributes["class"] = "tab-active";
                        break;
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideAdvancedSearch", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        public void RemoveKeyQueryString(string key)
        {
            PropertyInfo isreadonly =  typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove(key);
        }

        private void HideTabs()
        {
            trSummary.Visible = false;
            trPending.Visible = false;
            trInProgress.Visible = false;
            trFinalized.Visible = false;
            trAll.Visible = false;
            trFiles.Visible = false;

            //controls
            //divFolderBrowse.Attributes["style"] = "display:none";
            divFolderBrowse.Visible = false;

            spnSummary.Attributes["class"] = "tab-inactive";
            spnPending.Attributes["class"] = "tab-inactive";
            spnInProgress.Attributes["class"] = "tab-inactive";
            spnFinalized.Attributes["class"] = "tab-inactive";
            spnAll.Attributes["class"] = "tab-inactive";
            spnFiles.Attributes["class"] = "tab-inactive";
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            SUMMARYLIST,
            PENDINGLIST,
            INPROGRESSLIST,
            FINALIZEDLIST,
            SHOWALLLIST,
            SHOWFILESLIST,
            SEARCH,
            PROJECT,
            STATUS,
            DEPARTMENT,
            FOLDER,
            FROM,
            TO,
            EMPLOYEES
        }

        //public enum PageTabs
        //{
        //    Summary = 0,
        //    Pending = 1,
        //    InProgress = 2,
        //    Finalize = 3,
        //    All = 4
        //}

        public enum PageTabs
        {
            Summary = -1,
            Pending = 0,
            InProgress = 1,
            Finalize = 2,
            All = 3,
            Files = 4
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