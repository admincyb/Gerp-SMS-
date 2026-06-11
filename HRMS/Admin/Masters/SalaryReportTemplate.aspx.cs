using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using System.Threading;
using ERPSMS_v01.UserControls;
using BusinessObject.CommonManagement;
using ERP.Utilities.HRMS;
using CustomControls;
using BusinessObject.HRMS.Admin.Masters;
using BusinessLogic.HRMS.Admin.Masters;
using System.Xml;

namespace HRMS.Admin.Masters
{
    public partial class SalaryReportTemplate : System.Web.UI.Page
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
        private int GroupSequence
        {
            get
            {
                return ViewState[ViewstateStrings.GroupSequence] == null ? 0 : (int)ViewState[ViewstateStrings.GroupSequence];
            }
            set
            {
                ViewState[ViewstateStrings.GroupSequence] = value;
            }
        }
        private int GroupItemSequence
        {
            get
            {
                return ViewState[ViewstateStrings.GroupItemSequence] == null ? 0 : (int)ViewState[ViewstateStrings.GroupItemSequence];
            }
            set
            {
                ViewState[ViewstateStrings.GroupItemSequence] = value;
            }
        }
        private GridAction RowEditMode
        {
            get
            {
                return ViewState[ViewstateStrings.RowEditMode] == null ? GridAction.NEW : (GridAction)ViewState[ViewstateStrings.RowEditMode];
            }
            set
            {
                ViewState[ViewstateStrings.RowEditMode] = value;
            }
        }
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
        /// Group PK For Editing
        /// </summary>
        private int SelectedGroupPK
        {
            get
            {
                return this.ViewState[GetLocalResourceObject("SelectedGroupPK").ToString()] == null ? 0 : Convert.ToInt32(this.ViewState[GetLocalResourceObject("SelectedGroupPK").ToString()]);
            }
            set
            {
                this.ViewState[GetLocalResourceObject("SelectedGroupPK").ToString()] = value;
            }
        }

        private SalaryReportTemplateHeader SalaryReportTemplateHeader
        {
            get
            {
                return ViewState[GetLocalResourceObject("SalaryReportTemplateHeaderDetails").ToString()] == null ? new SalaryReportTemplateHeader() :
                    (SalaryReportTemplateHeader)ViewState[GetLocalResourceObject("SalaryReportTemplateHeaderDetails").ToString()];
            }
            set
            {
                ViewState[GetLocalResourceObject("SalaryReportTemplateHeaderDetails").ToString()] = value;
            }
        }

        private SalaryReportTemplateGroupDetails SalaryReportTemplateGroupDtl
        {
            get
            {
                return ViewState[GetLocalResourceObject("SalaryReportTemplateGroupDetails").ToString()] == null ? new SalaryReportTemplateGroupDetails() :
                    (SalaryReportTemplateGroupDetails)ViewState[GetLocalResourceObject("SalaryReportTemplateGroupDetails").ToString()];
            }
            set
            {
                ViewState[GetLocalResourceObject("SalaryReportTemplateGroupDetails").ToString()] = value;
            }
        }

        #endregion
        #region  Variables
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        private DataTable dtResult;
        DataTable dtCompany;
        DataSet dsTemplateData;
        private DataTable dtPayElement;
        private SalaryReportTemplateHeader ObjSalaryReportTemplateHeader;
        private SalaryReportTemplateGroupDetails ObjSalaryReportTemplateGrpDetails;

        #endregion
        #endregion

        #region PageLevel Events

        #region Page Load
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageActionHandler();
                GetFieldValues(ControlsEnum.PAYELEMENT);
                SetFieldValues(ControlsEnum.PAYELEMENT);
            }
        }
        #endregion

        #region Page Init
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
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
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
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
                EntryStatus = EntryStatus.LISTMODE;
                ResetForm(ControlsEnum.CLEAR);
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
                //txtTemplateSrchList.Focus();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion

        #region Pager Methods + Init
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
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
        #endregion

        #endregion

        #region Action Handler
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
                GridViewRow gvr;
                GridView grd;
                ExtGridView egrd;
                string arg;
                bool bIsChecked = false;
                XmlDocument xmlDoc;

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
                    if (((DropDownList)sender).ID == "ddlPayElement")
                    {
                        commonActions = ActionsEnum.CHANGEPAYELEMENT;
                    }
                }
                switch (commonActions)
                {
                    #region New
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        CurrPK = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                        SetFieldValues(ControlsEnum.GROUPBIND);
                        ResetForm(ControlsEnum.CLEAR);
                        txtGroupSequence.Text = CommonConstants.SELECT_VALUE_ONE;
                        txtSalRptName.Focus();
                        break;
                    #endregion

                    #region Save All
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                           // litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            litErrorMsg.Text = GetLocalResourceObject("Err_Details").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (grdSalaryRptTemplate.Rows.Count < 1)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_GroupDetails").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            SalaryReportTemplateHeader = (SalaryReportTemplateHeader)SetUIValuesToObject(ControlsEnum.SALARYREPORTHDR);
                            SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl = new List<SalaryReportTemplateGroupDetails>();
                            SalaryReportTemplateGroupDetails objSalaryReportTemplateGroupDetails;
                            foreach (GridViewRow gvrow in grdSalaryRptTemplate.Rows)
                            {
                                objSalaryReportTemplateGroupDetails = new SalaryReportTemplateGroupDetails();
                                objSalaryReportTemplateGroupDetails.GroupPk = Convert.ToInt32(((HiddenField)gvrow.FindControl("hdfGroupPK")).Value);
                                objSalaryReportTemplateGroupDetails.GroupHdrPK = Convert.ToInt32(((HiddenField)gvrow.FindControl("hdfGroupHdrPK")).Value);
                                objSalaryReportTemplateGroupDetails.GroupName = ((Label)gvrow.FindControl("lblGrpName")).Text;
                                objSalaryReportTemplateGroupDetails.GroupSequence = Convert.ToInt32(((HiddenField)gvrow.FindControl("hdfSequence")).Value);
                                objSalaryReportTemplateGroupDetails.GroupActive = Convert.ToInt32(((HiddenField)gvrow.FindControl("hdfGroupActive")).Value);
                                objSalaryReportTemplateGroupDetails.SalaryReportTemplateItemDtl = new List<SalaryReportTemplateItemDetails>();

                                GridView grdDtl = (GridView)gvrow.FindControl("grdPayElt");
                                SalaryReportTemplateItemDetails objSalaryReportTemplateItemDetails;

                                foreach (GridViewRow grdrow in grdDtl.Rows)
                                {
                                    objSalaryReportTemplateItemDetails = new SalaryReportTemplateItemDetails();
                                    objSalaryReportTemplateItemDetails.TDL_SEQUENCE = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfItemSequence")).Value);
                                    objSalaryReportTemplateItemDetails.TDL_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTdlPK")).Value);
                                    objSalaryReportTemplateItemDetails.PayElementPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPayElementPK")).Value);
                                    objSalaryReportTemplateItemDetails.PayElementName = ((Label)grdrow.FindControl("lblPayElmtName")).Text;
                                    objSalaryReportTemplateItemDetails.PayElementDispName = ((Label)grdrow.FindControl("lblPayElmtDispName")).Text;
                                    objSalaryReportTemplateItemDetails.TDL_ACTIVE = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfItemActive")).Value);
                                    objSalaryReportTemplateGroupDetails.SalaryReportTemplateItemDtl.Add(objSalaryReportTemplateItemDetails);
                                }
                                SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.Add(objSalaryReportTemplateGroupDetails);
                            }
                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(SalaryReportTemplateHeader);
                            result = SalaryReportTemplateBL.SaveSalaryReportTemplateDetails(xmlDoc.InnerXml);

                            //After Save
                            if (result > 0)
                            {
                                ResetForm(ControlsEnum.CLEAR);
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                                this.EntryStatus = EntryStatus.LISTMODE;
                                CurrPK = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                                litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryReportTemplate);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalaryReportTemplate + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Name_Already_Exists, GetLocalResourceObject("SalaryReportTemplate").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalaryReportTemplate + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                }
                                else if (result == (int)DbSaveStatus.REFNOEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SalaryReportTemplate + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                    //EntryStatus = EntryStatus.LISTMODE;
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryReportTemplate);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Edit,Detail
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlsEnum.CLEAR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTemplatePkListPage")).Value);
                                txtSalRptName.Focus();
                                GetFieldValues(ControlsEnum.TEMPLATEDETAIL);
                                SetFieldValues(ControlsEnum.TEMPLATEDETAIL);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region Filter Template List
                    case ActionsEnum.TEMPLATEFILTER:
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        this.EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        result = SalaryReportTemplateBL.DeleteSalaryTemplateMaster(this.CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.CLEAR);
                            if (grdList.Rows.Count == 1 && PageIndex > 1)
                            {
                                PageIndex--;
                            }
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            btnNew.Focus();
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryReportTemplate);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryReportTemplate;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryReportTemplate + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryReportTemplate + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryReportTemplate + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BonusTypeMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region Clear Filter
                    case ActionsEnum.TEMPLATECLEAR:
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        this.EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Group

                    #region Show Group Popup
                    case ActionsEnum.SHOWPOPUP:
                        txtGroupName.Text = string.Empty;
                        txtGroupName.Focus();
                        RowEditMode = GridAction.NEW;

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divNewGroup_PopUp]','" + GetLocalResourceObject("AddGroup").ToString() + "','260','150');", true);
                        break;
                    #endregion

                    #region Save Group
                    case ActionsEnum.SAVEGROUP:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            if (!IsGroupNameExist(txtGroupName.Text))
                            {
                                if (!IsGroupSequenceExist(Convert.ToInt32(txtGroupSequence.Text), RowEditMode) && RowEditMode == GridAction.NEW)
                                {
                                    SalaryReportTemplateHeader = (SalaryReportTemplateHeader)SetUIValuesToObject(ControlsEnum.SALARYREPORTGROUPDTL);
                                    SetFieldValues(ControlsEnum.GROUPBIND);
                                    GroupSequence = 0;
                                }
                                else if (!IsGroupSequenceExist(Convert.ToInt32(txtGroupSequence.Text), RowEditMode) && RowEditMode == GridAction.EDIT)
                                {
                                    SalaryReportTemplateHeader = (SalaryReportTemplateHeader)SetUIValuesToObject(ControlsEnum.SALARYREPORTGROUPDTLEDIT);
                                    SetFieldValues(ControlsEnum.GROUPBIND);
                                    GroupSequence = 0;
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divNewGroup_PopUp]','" + GetLocalResourceObject("AddGroup").ToString() + "','260','150');", true);
                                    litErrorMsg.Text = GetLocalResourceObject("Err_GroupSequence").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divNewGroup_PopUp]','" + GetLocalResourceObject("AddGroup").ToString() + "','260','150');", true);
                                litErrorMsg.Text = GetLocalResourceObject("Err_GroupNameExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region Edit Group
                    case ActionsEnum.EDITGROUP:

                        arg = ((ImageButton)sender).CommandArgument;
                        gvr = ((ImageButton)sender).Parent.Parent as ExtGridViewRow;
                        if (gvr != null)
                        {
                            HiddenField hdfGroupSequence = gvr.FindControl("hdfSequence") as HiddenField;
                            GroupSequence = Convert.ToInt32(hdfGroupSequence.Value);
                            RowEditMode = GridAction.EDIT;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divNewGroup_PopUp]','" + GetLocalResourceObject("AddGroup").ToString() + "','260','150');", true);
                            SetFieldValues(ControlsEnum.EDITGROUP);
                        }

                        break;
                    #endregion

                    #region Delete Group
                    case ActionsEnum.DELETEGROUP:
                        arg = ((ImageButton)sender).CommandArgument;
                        gvr = ((ImageButton)sender).Parent.Parent as ExtGridViewRow;
                        if (gvr != null)
                        {
                            HiddenField hdfGroupSequence = gvr.FindControl("hdfSequence") as HiddenField;
                            GroupSequence = Convert.ToInt32(hdfGroupSequence.Value);
                            if (SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl != null)
                            {
                                SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.RemoveAll(x => x.GroupSequence == GroupSequence);
                            }
                            SetFieldValues(ControlsEnum.GROUPBIND);
                            //txtGroupSequence.Text = (SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.Count + 2).ToString();
                        }
                        break;
                    #endregion

                    #endregion

                    #region Pay Element

                    #region Show Pay Element Popup Details
                    case ActionsEnum.ADDPAYELTITEM:
                        arg = ((ImageButton)sender).CommandArgument;
                        gvr = ((ImageButton)sender).Parent.Parent as ExtGridViewRow;
                        if (gvr != null)
                        {
                            HiddenField hdfGroupSequence = gvr.FindControl("hdfSequence") as HiddenField;
                            GroupSequence = Convert.ToInt32(hdfGroupSequence.Value);
                            ResetForm(ControlsEnum.PAYELEMENT);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divPayElement_PopUp]','" + GetLocalResourceObject("PayElementDetails").ToString() + "','460','150');", true);
                            RowEditMode = GridAction.NEW;
                        }
                        break;
                    #endregion

                    #region Edit Pay Element
                    case ActionsEnum.EDITPAYELTITEM:
                        arg = ((ImageButton)sender).CommandArgument;
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        if (gvr != null)
                        {
                            HiddenField hdfGroupSequence = gvr.Parent.Parent.Parent.Parent.FindControl("hdfSequence") as HiddenField;
                            GroupSequence = Convert.ToInt32(hdfGroupSequence.Value);
                            HiddenField hdfItemSequence = gvr.FindControl("hdfItemSequence") as HiddenField;
                            GroupItemSequence = Convert.ToInt32(hdfItemSequence.Value);
                            RowEditMode = GridAction.EDIT;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divPayElement_PopUp]','" + GetLocalResourceObject("PayElementDetails").ToString() + "','460','150');", true);
                            SetFieldValues(ControlsEnum.EDITPAYELEMENT);
                        }

                        break;
                    #endregion

                    #region Save Pay Elt Item to Grid
                    case ActionsEnum.ADDITEM:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (ddlPayElement.SelectedValue == "-1")
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divPayElement_PopUp]','" + GetLocalResourceObject("PayElementDetails").ToString() + "','460','150');", true);
                            litErrorMsg.Text = GetLocalResourceObject("SelectPayElement").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);

                        }
                        else if (RowEditMode == GridAction.NEW)
                        {
                            SalaryReportTemplateHeader = (SalaryReportTemplateHeader)SetUIValuesToObject(ControlsEnum.ADDPAYELEMENT);
                            SetFieldValues(ControlsEnum.GROUPBIND);
                            GroupItemSequence = 0;
                        }
                        else
                        {
                            SalaryReportTemplateHeader = (SalaryReportTemplateHeader)SetUIValuesToObject(ControlsEnum.EDITPAYELEMENT);
                            SetFieldValues(ControlsEnum.GROUPBIND);
                            GroupItemSequence = 0;
                        }
                        break;
                    #endregion

                    #region Delete Pay Element
                    case ActionsEnum.DELETEPAYELTITEM:
                        arg = ((ImageButton)sender).CommandArgument;
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        if (gvr != null)
                        {
                            HiddenField hdfGroupSequence = gvr.Parent.Parent.Parent.Parent.FindControl("hdfSequence") as HiddenField;
                            GroupSequence = Convert.ToInt32(hdfGroupSequence.Value);
                            HiddenField hdfItemSequence = gvr.FindControl("hdfItemSequence") as HiddenField;
                            GroupItemSequence = Convert.ToInt32(hdfItemSequence.Value);
                            if (SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl != null)
                            {
                                foreach (SalaryReportTemplateGroupDetails group in SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl)
                                {
                                    foreach (SalaryReportTemplateItemDetails items in group.SalaryReportTemplateItemDtl)
                                    {
                                        if (items.TDL_SEQUENCE == GroupItemSequence)
                                        {
                                            group.SalaryReportTemplateItemDtl.RemoveAll(x => x.TDL_SEQUENCE == GroupItemSequence);
                                            break;
                                        }
                                    }
                                }
                            }
                            SetFieldValues(ControlsEnum.GROUPBIND);
                        }
                        break;
                    #endregion

                    #region Pay Element Change
                    case ActionsEnum.CHANGEPAYELEMENT:
                        if (Convert.ToInt32(ddlPayElement.SelectedValue) <= 0)
                        {
                            txtPayEltMdfyName.Text = string.Empty;
                        }
                        else
                        {
                            txtPayEltMdfyName.Text = ddlPayElement.SelectedItem.Text;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divPayElement_PopUp]','" + GetLocalResourceObject("PayElementDetails").ToString() + "','460','150');", true);
                        break;
                    #endregion

                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        break;
                    #endregion

                    #region  Close Popup
                    case ActionsEnum.CANCELPOPUP:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
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
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if ((sender as GridView).ID == "grdSalaryRptTemplate")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grdPayElt = e.Row.FindControl("grdPayElt") as GridView;
                    if (SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl != null)
                    {
                        foreach (SalaryReportTemplateGroupDetails det in SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl)
                        {
                            if (det.SalaryReportTemplateItemDtl != null)
                            {
                                if (det.GroupSequence == Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfSequence")).Value))
                                {

                                    List<SalaryReportTemplateItemDetails> Itemdetail = det.SalaryReportTemplateItemDtl.OrderBy(x => x.TDL_SEQUENCE).ToList();
                                    grdPayElt.DataSource = Itemdetail;
                                    grdPayElt.DataBind();
                                }
                                //else
                                //{
                                //    grdPayElt.DataSource = null;
                                //    grdPayElt.DataBind();
                                //}
                            }

                        }
                    }

                    //if (grdPayElt.Rows.Count > 0)
                    //{
                    //    ((ImageButton)grdPayElt.Rows[0].FindControl("imbRuleDownPop")).Visible = true;
                    //    ((ImageButton)grdPayElt.Rows[0].FindControl("imbRuleUpPop")).Visible = false;
                    //    ((ImageButton)grdPayElt.Rows[grdPayElt.Rows.Count - 1].FindControl("imbRuleDownPop")).Visible = false;
                    //}
                }
            }
        }

        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
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
                        dsTemplateData = SalaryReportTemplateBL.GetSalaryReportTemplateList(null, Convert.ToInt32(DbActiveStatus.ACTIVE), PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")), currentUser.SBUID, txtTemplateSrchList.Text.Trim());
                        break;
                    #endregion

                    #region Pay Element
                    case ControlsEnum.PAYELEMENT:
                        dtPayElement = SalaryReportTemplateBL.GetPayElementDetails(CurrPK, Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 1);
                        break;
                    #endregion

                    #region Template Details - Edit
                    case ControlsEnum.TEMPLATEDETAIL:
                        SalaryReportTemplateHeader = SalaryReportTemplateBL.GetSalaryReportTemplateDetails(CurrPK);
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
        private void SetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    #region List
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion

                    #region Bind Grid
                    case ControlsEnum.GROUPBIND:
                        BindGrid(ControlsEnum.GROUPBIND);
                        break;
                    #endregion

                    #region Pay Element Grid Bind
                    case ControlsEnum.ADDPAYELEMENT:
                        BindGrid(ControlsEnum.ADDPAYELEMENT);
                        break;
                    #endregion

                    #region Pay Element Dropdown
                    case ControlsEnum.PAYELEMENT:
                        BindDropDown(ControlsEnum.PAYELEMENT);
                        break;
                    #endregion

                    #region Edit Group
                    case ControlsEnum.EDITGROUP:
                        GetUIValuesFromObject(ControlsEnum.EDITGROUP);
                        break;
                    #endregion

                    #region Edit Pay Element Item
                    case ControlsEnum.EDITPAYELEMENT:
                        GetUIValuesFromObject(ControlsEnum.EDITPAYELEMENT);
                        break;
                    #endregion

                    #region Template Details Fill
                    case ControlsEnum.TEMPLATEDETAIL:
                        GetUIValuesFromObject(ControlsEnum.TEMPLATEDETAIL);
                        break;
                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region Set UI Values To Object

        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject = null;
            int tdlSequence = 0;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region Salary Report Template Header
                    case ControlsEnum.SALARYREPORTHDR:

                        ObjSalaryReportTemplateHeader = SalaryReportTemplateHeader;
                        ObjSalaryReportTemplateHeader.HdrPK = CurrPK;
                        ObjSalaryReportTemplateHeader.SalRptTempName = txtSalRptName.Text;
                        ObjSalaryReportTemplateHeader.SalRptTempValue = Convert.ToInt32(txtSalRptValue.Text);
                        ObjSalaryReportTemplateHeader.SalRptTempRemark = txtRemarks.Text;
                        ObjSalaryReportTemplateHeader.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        ObjSalaryReportTemplateHeader.USER_PK = currentUser.PKUser;
                        ObjSalaryReportTemplateHeader.BIZUNIT = currentUser.SBUID;
                        if (EntryStatus == EntryStatus.NEWMODE)
                            ObjSalaryReportTemplateHeader.LAST_MOD_DT = LastModifiedTime;
                        retObject = ObjSalaryReportTemplateHeader;

                        break;
                    #endregion

                    #region Salary Report Template Group Detail
                    case ControlsEnum.SALARYREPORTGROUPDTL:

                        SalaryReportTemplateHeader = (SalaryReportTemplateHeader)SetUIValuesToObject(ControlsEnum.SALARYREPORTHDR);

                        if (SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl == null)
                            SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl = new List<SalaryReportTemplateGroupDetails>();

                        SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.Add(new SalaryReportTemplateGroupDetails()
                                {
                                    GroupPk = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO),
                                    GroupHdrPK = CurrPK,
                                    GroupName = txtGroupName.Text,
                                    GroupSequence = Convert.ToInt32(txtGroupSequence.Text),
                                    GroupActive = Convert.ToInt32(DbActiveStatus.ACTIVE),
                                    //LAST_MOD_DT = LastModifiedTime
                                });

                        retObject = SalaryReportTemplateHeader;

                        break;
                    #endregion

                    #region Group Details Edit
                    case ControlsEnum.SALARYREPORTGROUPDTLEDIT:

                        SalaryReportTemplateHeader = (SalaryReportTemplateHeader)SetUIValuesToObject(ControlsEnum.SALARYREPORTHDR);

                        foreach (SalaryReportTemplateGroupDetails group in SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl)
                        {
                            if (group.GroupSequence == Convert.ToInt32(GroupSequence))
                            {
                                group.GroupName = txtGroupName.Text;
                                group.GroupSequence = Convert.ToInt32(txtGroupSequence.Text);
                            }
                        }
                        retObject = SalaryReportTemplateHeader;
                        break;
                    #endregion

                    #region Add Pay Element
                    case ControlsEnum.ADDPAYELEMENT:

                        SalaryReportTemplateHeader = (SalaryReportTemplateHeader)SetUIValuesToObject(ControlsEnum.SALARYREPORTHDR);

                        foreach (SalaryReportTemplateGroupDetails group in SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl)
                        {
                            if (group.GroupSequence == Convert.ToInt32(GroupSequence))
                            {
                                if (group.SalaryReportTemplateItemDtl == null)
                                {
                                    group.SalaryReportTemplateItemDtl = new List<SalaryReportTemplateItemDetails>();
                                    tdlSequence = 1;
                                }
                                else if (group.SalaryReportTemplateItemDtl.Count == 0)
                                {
                                    group.SalaryReportTemplateItemDtl = new List<SalaryReportTemplateItemDetails>();
                                    tdlSequence = 1;
                                }
                                else
                                    tdlSequence = group.SalaryReportTemplateItemDtl.Max(x => x.TDL_SEQUENCE) + 1;

                                group.SalaryReportTemplateItemDtl.Add(new SalaryReportTemplateItemDetails()
                                {
                                    TDL_SEQUENCE = tdlSequence,
                                    TDL_PK = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO),
                                    PayElementPK = Convert.ToInt32(ddlPayElement.SelectedValue),
                                    PayElementDispName = HttpUtility.HtmlEncode(txtPayEltMdfyName.Text),
                                    PayElementName = HttpUtility.HtmlEncode(ddlPayElement.SelectedItem.Text),
                                    TDL_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE),
                                    //LAST_MOD_DT = LastModifiedTime
                                });
                            }
                        }

                        retObject = SalaryReportTemplateHeader;
                        break;
                    #endregion

                    #region Pay Element Edit
                    case ControlsEnum.EDITPAYELEMENT:

                        SalaryReportTemplateHeader = (SalaryReportTemplateHeader)SetUIValuesToObject(ControlsEnum.SALARYREPORTHDR);

                        foreach (SalaryReportTemplateGroupDetails group in SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl)
                        {
                            if (group.SalaryReportTemplateItemDtl != null && group.SalaryReportTemplateItemDtl.Count > 0 && group.GroupSequence == GroupSequence)
                            {
                                foreach (SalaryReportTemplateItemDetails itm in group.SalaryReportTemplateItemDtl)
                                {
                                    if (itm.TDL_SEQUENCE == GroupItemSequence)
                                    {
                                        itm.PayElementPK = Convert.ToInt32(ddlPayElement.SelectedValue);
                                        itm.PayElementName = ddlPayElement.SelectedItem.Text;
                                        itm.PayElementDispName = txtPayEltMdfyName.Text;
                                    }
                                }
                            }
                        }
                        retObject = SalaryReportTemplateHeader;
                        break;
                    #endregion

                    default:
                        break;
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
        #endregion

        #region Get UI Values From Object
        private Object GetUIValuesFromObject(ControlsEnum controlType)
        {
            Object retObject = null;
            try
            {
                switch (controlType)
                {
                    #region Group Edit
                    case ControlsEnum.EDITGROUP:

                        if (SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl != null && RowEditMode == GridAction.EDIT)
                        {
                            foreach (SalaryReportTemplateGroupDetails group in SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl)
                            {
                                if (group.GroupSequence == GroupSequence)
                                {
                                    txtGroupName.Text = group.GroupName;
                                    txtGroupSequence.Text = group.GroupSequence.ToString();
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Pay Element Edit
                    case ControlsEnum.EDITPAYELEMENT:

                        if (SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl != null && RowEditMode == GridAction.EDIT)
                        {
                            foreach (SalaryReportTemplateGroupDetails group in SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl)
                            {
                                if (group.GroupSequence == GroupSequence)
                                {
                                    if (group.SalaryReportTemplateItemDtl != null && group.SalaryReportTemplateItemDtl.Count > 0)
                                    {
                                        foreach (SalaryReportTemplateItemDetails itm in group.SalaryReportTemplateItemDtl)
                                        {
                                            if (itm.TDL_SEQUENCE == GroupItemSequence)
                                            {
                                                ddlPayElement.SelectedValue = itm.PayElementPK.ToString();
                                                ddlPayElement.SelectedItem.Text = itm.PayElementName.ToString();
                                                txtPayEltMdfyName.Text = itm.PayElementDispName;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Fill Details - Edit mode
                    case ControlsEnum.TEMPLATEDETAIL:
                        txtSalRptName.Text = SalaryReportTemplateHeader.SalRptTempName.ToString();
                        txtSalRptValue.Text = SalaryReportTemplateHeader.SalRptTempValue.ToString();
                        txtRemarks.Text = SalaryReportTemplateHeader.SalRptTempRemark.ToString();
                        LastModifiedTime = SalaryReportTemplateHeader.LAST_MOD_DT;
                        SetFieldValues(ControlsEnum.GROUPBIND);
                        break;
                    #endregion

                    default:
                        break;
                }
                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Helper Methods

        private bool IsGroupSequenceExist(int value, GridAction type) //checking same sequence exist in group
        {
            bool result = false;
            if (GroupSequence != value || type == GridAction.NEW) //checking for existence of sequence in new & edit mode
                if (SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl != null && SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.Count > 0)
                {
                    result = SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.Count(x => x.GroupSequence == value) > 0;
                }
            return result;
        }

        private bool IsGroupNameExist(string name) //checking same name exist in group
        {
            bool result = false;
            if (SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl != null && SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.Count > 0)
            {
                result = SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.Count(x => x.GroupName == name) > 0;
            }
            return result;
        }

        #endregion

        #region Bind Dropdown
        private void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Pay Element
                    case ControlsEnum.PAYELEMENT:
                        ddlPayElement.Items.Clear();
                        ddlPayElement.DataTextField = GetLocalResourceObject("DdlPayEltName").ToString();
                        ddlPayElement.DataValueField = GetLocalResourceObject("DdlPayEltPk").ToString();
                        ddlPayElement.DataSource = dtPayElement;
                        ddlPayElement.DataBind();
                        ddlPayElement.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        ddlPayElement.Items.HtmlDecode();
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

        #region Bind Grid
        private void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dsTemplateData != null && dsTemplateData.Tables[0].Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dsTemplateData.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dsTemplateData;
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

                    #region List Grid
                    case ControlsEnum.GROUPBIND:
                        if (SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl != null && SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.Count > 0)
                        {
                            List<SalaryReportTemplateGroupDetails> GrpDetail = SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.OrderBy(x => x.GroupSequence).ToList();
                            grdSalaryRptTemplate.DataSource = GrpDetail; //SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl;
                            grdSalaryRptTemplate.DataBind();
                            txtGroupSequence.Text = (SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.Max(x => x.GroupSequence + 1).ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        }
                        else
                        {
                            grdSalaryRptTemplate.DataSource = null;
                            grdSalaryRptTemplate.DataBind();
                        }
                        break;
                    #endregion

                    #region Pay Element Grid
                    case ControlsEnum.ADDPAYELEMENT:
                        if (SalaryReportTemplateHeader != null)
                        {
                            grdSalaryRptTemplate.DataSource = SalaryReportTemplateHeader; //SalaryReportTemplateHeader.SalaryReportTemplateGrpDtl;
                            grdSalaryRptTemplate.DataBind();
                            txtGroupSequence.Text = (ObjSalaryReportTemplateHeader.SalaryReportTemplateGrpDtl.Count + 1).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        }
                        else
                        {
                            grdSalaryRptTemplate.DataSource = null;
                            grdSalaryRptTemplate.DataBind();
                            txtGroupSequence.Text = CommonConstants.SELECT_VALUE_ONE;
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
                case ControlsEnum.GROUPADD:
                    txtGroupName.Text = string.Empty;
                    txtGroupSequence.Text = string.Empty;
                    txtGroupName.Focus();
                    break;

                case ControlsEnum.PAYELEMENT:
                    ddlPayElement.SelectedIndex = -1;
                    txtPayEltMdfyName.Text = string.Empty;
                    break;

                case ControlsEnum.CLEAR:
                    txtTemplateSrchList.Text = string.Empty;
                    txtSalRptName.Text = string.Empty;
                    txtSalRptValue.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    ddlPayElement.SelectedIndex = -1;
                    // ddlPayElement.Items.Clear();
                    txtPayEltMdfyName.Text = string.Empty;
                    txtGroupName.Text = string.Empty;
                    txtGroupSequence.Text = string.Empty;
                    GroupSequence = 0;
                    GroupItemSequence = 0;
                    SalaryReportTemplateHeader = null;
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Enum
        public enum GridAction
        {
            NEW,
            EDIT,
            DELETE
        }
        public enum ControlsEnum
        {
            CLEAR,
            LIST,
            SALARYREPORTHDR,
            SALARYREPORTGROUPDTL,
            SALARYREPORTGROUPDTLEDIT,
            SALARYREPORTSUBGRPDTL,
            GROUPBIND,
            ADDGRID,
            PAYELEMENT,
            SALARYRPEPORTDETAILS,
            ADDPAYELTITEM,
            EDITPAYELTITEM,
            DELETEPAYELTITEM,
            GROUPADD,
            ADDPAYELEMENT,
            EDITPAYELEMENT,
            EDITGROUP,
            DELETEGROUP,
            TEMPLATEDETAIL,
            TEMPLATEFILTER,
            TEMPLATECLEAR
        }
        #endregion

        #region Enable Disable Buttons
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
    }
}