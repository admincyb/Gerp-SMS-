using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.Common;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using BusinessObject.HRMS.Payroll;
using System.Xml;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERP.Utilities.HRMS;
using System.Web.UI.HtmlControls;
using BusinessLogic.HRMS.Payroll;
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class SalaryYearEnd : ERP.Store.UI.MyBasePage//: System.Web.UI.Page
    {
        #region Variables and Properties
        #region Properties
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

        private string PageIndexList
        {
            get
            {
                return (string)this.ViewState["PageIndexList"];
            }
            set
            {
                this.ViewState["PageIndexList"] = value;
            }
        }
        /// <summary>
        /// To keep Leave Details List in view state
        /// </summary>
        private List<HRYearCloseDetails> HRYearClauseDetailsList
        {
            get
            {
                return ViewState["HRYearClauseDetailsList"] == null ? new List<HRYearCloseDetails>() : (List<HRYearCloseDetails>)ViewState["HRYearClauseDetailsList"];
            }
            set
            {
                ViewState["HRYearClauseDetailsList"] = value; //ViewstateStrings.HRYearClauseDetailsList
            }
        }
        /// <summary>
        /// To keep Current PK in view state
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
        /// To keep Leave Details List in view state
        /// </summary>
        private DataTable vwCarryFWDList
        {
            get
            {
                return ViewState["vwCarryFWDList"] == null ? new DataTable() : (DataTable)ViewState["vwCarryFWDList"];
            }
            set
            {
                ViewState["vwCarryFWDList"] = value;
            }
        }
        #endregion
        #region Variables
        private BusinessObject.User currentUser;
        private DataTable dtCLeave;
        private DataTable dtSalary;
        private DataSet dsPageData;
        private HRYearCloseHeader objHRYearClauseHdr;
        #endregion
        #endregion
        #region PageEvents
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            GetFieldValues(ControlsEnum.CARRYLEAVE);
            SetFieldValues(ControlsEnum.CARRYLEAVE);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }

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

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
               // lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
               // lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                //lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
               // lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
               // lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
              //  lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
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
                InitializeComponent();
                if (!IsPostBack)
                {
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Configuration Settings
        /// </summary>
        private void ConfigurationSettings()
        {

        }
        #endregion
        #region Action Handler
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActionHandler(object sender, EventArgs e)
        {
            //To check if department is different by opening in new tab
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            int result;
            XmlDocument xmlDoc;
            try
            {
                #region Getting Command Action
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
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
                #endregion
                switch (commonActions)
                {
                    #region New
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.SALARYDETAILS);
                        SetFieldValues(ControlsEnum.SALARYDETAILS);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if ((hdfIscontYes.Value != "1"))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){SaveConfirmationMsg();});", true);
                            return;
                        }
                        objHRYearClauseHdr = (HRYearCloseHeader)SetUIValuesToObject(ControlsEnum.HRYearClauseHeader);
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objHRYearClauseHdr);
                        result = HRYearCloseBL.SaveHRYearClause(xmlDoc.InnerXml);
                        if (result > 0)
                        {
                            hdfIscontYes.Value = "0";
                            ResetForm(ControlsEnum.CLEAR);
                            ResetForm(ControlsEnum.CLEARSEARCH);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString();
                            object[] args = new object[2];
                            args[0] = Resources.PageNameRes.HRYearClose;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.HRYearClose + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.HRYearClose + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.HRYearClose + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.HRYearClose);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region LIST
                    case ActionsEnum.LIST:
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            try
            {
                FilterParameters objFilterParam;
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        objFilterParam = new FilterParameters();
                        objFilterParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        objFilterParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        objFilterParam.FromDate = string.IsNullOrEmpty(txtFilterFromDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterFromDate.Text);
                        objFilterParam.UserPK = currentUser.PKUser;
                        objFilterParam.BizUnit = currentUser.SBUID;
                        dsPageData = HRYearCloseBL.GetLeaveList(objFilterParam);
                        break;
                    #endregion
                    #region CARRY LEAVE
                    case ControlsEnum.CARRYLEAVE:
                        if (dtCLeave == null)
                        {
                            dtCLeave = HRYearCloseBL.GetLeaveType(0, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID);
                            vwCarryFWDList = dtCLeave;
                        }
                        break;
                    #endregion
                    #region SALARY DETAILS
                    case ControlsEnum.SALARYDETAILS:
                        dtSalary = HRYearCloseBL.GetSalaryDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID);
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
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion

                    #region CARRYLEAVE
                    case ControlsEnum.CARRYLEAVE:
                        BindControl(ControlsEnum.CARRYLEAVE);
                        break;
                    #endregion

                    #region SALARYDETAILS
                    case ControlsEnum.SALARYDETAILS:
                        GetUIValuesFromObject(ControlsEnum.SALARYDETAILS);
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
        #region Sets the UI input controls from the object values
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region SALARYDETAILS
                    case ControlsEnum.SALARYDETAILS:
                        if (dtSalary != null)
                        {
                            var last = dtSalary.AsEnumerable().OrderByDescending(p => p["HYR_DATE_TO"]).FirstOrDefault();
                            lblCurSalYear.Text = string.Format("{0} To {1}", Convert.ToDateTime(last["HYR_DATE_FROM"]).ToString(Resources.Constants.HRMSDateFormatShort),
                                Convert.ToDateTime(last["HYR_DATE_TO"]).ToString(Resources.Constants.HRMSDateFormatShort));
                            CurrPK = Convert.ToInt32(last["HYR_PK"]);
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
        #region BindGrid
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        int rowCount = 0;
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        if (dsPageData.Tables[0].Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                        grdList.DataSource = dsPageData.Tables[0];
                        grdList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
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
        #region BindControl
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindControl(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region CARRY LEAVE
                    case ControlsEnum.CARRYLEAVE:
                        HtmlGenericControl divMainGroup = new HtmlGenericControl("div");
                        Table Tbl = new Table();
                        Tbl.CssClass = "table-devide";
                        TableRow Tr;
                        DataRow drFieldControls;
                        TableCell Tc = new TableCell();
                        HtmlGenericControl div = new HtmlGenericControl();
                        int TabIndex = Convert.ToInt32(GetLocalResourceObject("CFLTabIndex"));

                        if (dtCLeave != null && dtCLeave.Rows.Count > 0)
                        {
                            string div2colStyle = "div2col-S lbl padglft25-5per";
                            for (int i = 0; i < dtCLeave.Rows.Count; i += 2)
                            {
                                Tr = new TableRow();
                                //No.Of Td
                                for (int j = 0; j < 2; j++)
                                {
                                    div = new HtmlGenericControl("div");
                                    div.Attributes.Add("class", div2colStyle);
                                    Tc = new TableCell();
                                    //No.Of Checkbox per Td
                                    for (int k = 0; k < 2; k++)
                                    {
                                        if (dtCLeave.Rows.Count > i + j + k)
                                        {
                                            drFieldControls = dtCLeave.Rows[i + j + k];
                                            CheckBox chk = new CheckBox()
                                            {
                                                ID = "chk" + drFieldControls["LTM_PK"].ToString(),
                                                ClientIDMode = ClientIDMode.Static,
                                                Text = drFieldControls["LTM_NAME"].ToString(),
                                                TabIndex = (short)(TabIndex),
                                                Enabled = false,
                                                CssClass = "span-normal"

                                            };
                                            if (Convert.ToInt32(drFieldControls["LTM_CARRY_FWD"]) == 1)
                                                chk.Checked = true;
                                            div.Controls.Add(chk);
                                        }
                                    }
                                    i++;
                                    Tc.Controls.Add(div);
                                    Tr.Controls.Add(Tc);
                                }
                                Tbl.Controls.Add(Tr);
                            }
                            divMainGroup.Controls.Add(Tbl);
                            pnlControls.Controls.Add(divMainGroup);
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
        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            object returnObject = new object();
            switch (controlType)
            {
                #region HRYearClauseHeader
                case ControlsEnum.HRYearClauseHeader:
                    HRYearCloseHeader objHRYearClauseHdr = new HRYearCloseHeader();
                    objHRYearClauseHdr.HYR_PK = CurrPK;
                    objHRYearClauseHdr.HYE_DATE = Convert.ToDateTime(txtDateHd.Text);
                    objHRYearClauseHdr.HYE_DESC = txtDescription.Text.HtmlDecode();
                    objHRYearClauseHdr.USER_PK = currentUser.PKUser;
                    objHRYearClauseHdr.BIZUNIT_PK = currentUser.SBUID;
                    SetUIValuesToObject(ControlsEnum.HRYearClauseDetails);
                    objHRYearClauseHdr.HRYearClauseDetails = HRYearClauseDetailsList;
                    returnObject = objHRYearClauseHdr;
                    break;
                #endregion

                #region HRYear Clause Details
                case ControlsEnum.HRYearClauseDetails:

                    HRYearClauseDetailsList = new List<HRYearCloseDetails>();
                    HRYearCloseDetails objTempVer = new HRYearCloseDetails();
                    foreach (DataRow row in vwCarryFWDList.Rows)
                    {
                        CheckBox chkCtrlId = (CheckBox)pnlControls.FindControl("chk" + row["LTM_PK"].ToString());
                        if (chkCtrlId != null)
                        {
                            if (chkCtrlId.Checked)
                            {
                                HRYearClauseDetailsList.Add(new HRYearCloseDetails { LTM_PK = Convert.ToInt32(row["LTM_PK"]) });
                            }
                        }
                    }
                    break;
                #endregion
            }
            return returnObject;
        }
        #endregion
        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region CLEAR
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    txtDateHd.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    HRYearClauseDetailsList = null;
                    txtFilterFromDate.Text = string.Empty;
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    txtFilterFromDate.Text = string.Empty;
                    uclPaging.CurrentPage = 0;
                    this.PageIndexList = "1";
                    break;
                #endregion

            }
        }
        #endregion
        #region Custom Pager Control Navigated Event
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    PageIndexList = uclPaging.CurrentPage.ToString();
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
                // Should we disable the first link
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                // Should we disable the previous link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                // Should we enable the next link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
                // Should we enable the last link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            }
        }

        #endregion
        #region UtitlityMethods
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
        private decimal? GetNullableDecimal(string str)
        {
            decimal result;
            return (decimal.TryParse(str, out result) ? (decimal?)result : null);
        }
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            string format = "#0.00";
            string s = num.ToString(format);
            return s;
        }

        public string GetSubstring(object str)
        {
            return ((string)str).Substring(0, 3);
        }
        #endregion
        #region ControlEnum
        public enum ControlsEnum
        {
            SALARYDETAILS,
            CARRYLEAVE,
            HRYearClauseHeader,
            HRYearClauseDetails,
            CLEAR,
            CLEARSEARCH,
            LIST
        }
        #endregion
    }
}