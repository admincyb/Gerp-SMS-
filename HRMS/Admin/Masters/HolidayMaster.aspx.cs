using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject;
using BusinessObject.Common;
using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using System.Data;
using BusinessObject.HRMS.Admin.Masters;
using BusinessObject.CommonManagement;
using BusinessLogic.HRMS.Admin.Masters;

namespace HRMS.Admin.Masters
{
    public partial class HolidayMaster :  ERP.Store.UI.MyBasePage
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
        private int CurrHolidayDtPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrHolidayDtPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrHolidayDtPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrHolidayDtPK] = value;
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
        private List<HolidayMasterDetails> HolidayDetailList
        {
            get
            {
                return (List<HolidayMasterDetails>)ViewState[ViewstateStrings.HolidayDetails];
            }
            set
            {
                ViewState[ViewstateStrings.HolidayDetails] = value;
            }
        }
        private int TypeCurrPK
        {
            get
            {
                return ViewState["TypePK"] == null ? 0 : (int)ViewState["TypePK"];
            }
            set
            {
                ViewState["TypePK"] = value;
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

        #endregion
        #region  Variables
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        private int CurrSlNo;
        private DataTable dtResult;
        private DataTable dtType;
        DataTable dtCompany;
        private HolidayMasterHeader objHolidayHeader;
        private HolidayMasterDetails objHolidayDtls;

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
            InitializeComponent();
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
                    TypeCurrPK = 0;
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
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
                        dtResult = HolidayMasterBL.GetHolidayMasterList(currentUser.SBUID, txtFilterCaption.Text.Trim(), Convert.ToInt32(DbActiveStatus.ACTIVE), PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")));
                         
                        break;
                    #endregion
                    #region Edit
                    case ControlsEnum.EDIT:
                        // dtResult = PayElementsMasterBL.GetParentElement(CurrPK, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID, 0, 0, -1);
                        break;
                    #endregion
                    #region Type
                    case ControlsEnum.TYPE:
                        dtResult = HolidayMasterBL.GetHolidayType(TypeCurrPK, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID);
                        break;
                    #endregion
                    #region GET HOLIDAY MASTER DETAILS
                    case ControlsEnum.HOLIDAYMASTERDETAILS:
                        objHolidayHeader = HolidayMasterBL.GetHolidayMasterByPK(currentUser.SBUID, Convert.ToInt32(CommonConstants.ACTIVE), CurrPK);
                        if (objHolidayHeader == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        }
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
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region HOLIDAYDETAILS
                    case ControlsEnum.HOLIDAYDETAILS:
                        BindGrid(ControlsEnum.HOLIDAYDETAILS);
                        break;
                    #endregion
                    #region HOLIDAYMASTERDETAILS
                    case ControlsEnum.HOLIDAYMASTERDETAILS:
                        GetUIValuesFromObject(ControlsEnum.HOLIDAYMASTERDETAILS);
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
        #region Set UIValues To Object
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region ADDHOLIDAYMASTERHDR
                    case ControlsEnum.ADDHOLIDAYMASTERHDR:
                        objHolidayHeader.HDR_PK = CurrPK;
                        objHolidayHeader.HDR_CAPTION = txtCaption.Text.HtmlEncode();
                        objHolidayHeader.HDR_DESC = txtDescription.Text.HtmlEncode();
                        objHolidayHeader.HDR_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        objHolidayHeader.HDR_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        objHolidayHeader.HDR_ACTIVE = Convert.ToInt16(DbActiveStatus.ACTIVE);
                        objHolidayHeader.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        objHolidayHeader.LAST_MOD_DT = LastModifiedTime;
                        objHolidayHeader.LEAVE_DEL = Convert.ToInt16(hdfIsLeaveExcYes.Value);
                        if (Convert.ToInt32(hdfCompany.Value) > 0)
                        {
                            objHolidayHeader.HDR_COMPANY = Convert.ToInt32(hdfCompany.Value);
                        }
                        objHolidayHeader.HolidayMasterDtl = HolidayDetailList;
                        retObject = objHolidayHeader;
                        break;
                    #endregion
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
        #region Get UIValues From Object
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.HOLIDAYMASTERDETAILS:
                        if (objHolidayHeader != null)
                        {
                            txtCaption.Text = objHolidayHeader.HDR_CAPTION.HtmlDecode();
                            txtDescription.Text = objHolidayHeader.HDR_DESC.HtmlDecode();
                            GetFieldValues(ControlsEnum.TYPE);
                            SetFieldValues(ControlsEnum.TYPE);
                            LastModifiedTime = objHolidayHeader.LAST_MOD_DT;
                            HolidayDetailList = objHolidayHeader.HolidayMasterDtl;
                            SetFieldValues(ControlsEnum.HOLIDAYDETAILS);
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
                    ddlType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlType.DataSource = dtResult;
                        ddlType.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_FIELD;
                        ddlType.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlType.DataBind();
                    }
                    ddlType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
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
                    #region HOLIDAY DETAILS
                    case ControlsEnum.HOLIDAYDETAILS:
                        if (HolidayDetailList != null)
                        {
                            grdHolidayDetails.DataSource = HolidayDetailList.Where(x => x.IS_DELETED == 0);
                            grdHolidayDetails.DataBind();
                        }
                        else
                        {
                            grdHolidayDetails.DataSource = null;
                            grdHolidayDetails.DataBind();
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
                    CurrHolidayDtPK = 0;
                    txtFilterCaption.Text = string.Empty;
                    txtCaption.Text = string.Empty;
                    txtDate.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    txtName.Text = string.Empty;
                    if (ddlType.Items.Count > 0)
                        ddlType.SelectedIndex = 0;
                    txtRemarks.Text = string.Empty;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    HolidayDetailList = null;
                    hdfHolidayDtSlNo.Value = CommonConstants.SELECT_ALL_VAL;
                    CurrHolidayDtPK = 0;
                    SetFieldValues(ControlsEnum.HOLIDAYDETAILS);
                    break;
                #endregion
                # region CLEARHOLIDAYDETAILS
                case ControlsEnum.CLEARHOLIDAYDETAILS:
                    hdfHolidayDtSlNo.Value = CommonConstants.SELECT_ALL_VAL;
                    CurrHolidayDtPK = 0;
                    txtDate.Text = string.Empty;
                    txtName.Text = string.Empty;
                    if (ddlType.Items.Count > 0)
                        ddlType.SelectedIndex = 0;
                    txtRemarks.Text = string.Empty;
                    break;
                #endregion
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

                switch (commonActions)
                {
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objHolidayHeader = new HolidayMasterHeader();
                            objHolidayHeader = (HolidayMasterHeader)SetUIValuesToObject(ControlsEnum.ADDHOLIDAYMASTERHDR);
                            if (objHolidayHeader != null)
                            {
                                if (objHolidayHeader.HolidayMasterDtl != null && objHolidayHeader.HolidayMasterDtl.Count > 0)
                                {
                                    string xmlDoc = CommonFunctions.XmlSerialize<HolidayMasterHeader>(objHolidayHeader);
                                    result = HolidayMasterBL.SaveHolidayMasterDetails(xmlDoc);
                                    if (result > 0)
                                    {
                                        litErrorMsg.Text = Resources.Messages.Msg_HolidayMasterSave_Successfully;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEAR);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        CurrPK = (int)result;
                                        GetFieldValues(ControlsEnum.LIST);
                                        SetFieldValues(ControlsEnum.LIST);
                                        hdfIsLeaveExcYes.Value = CommonConstants.SELECT_ALL_VAL;
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
                                            litErrorMsg.Text = Resources.PageNameRes.HolidayMaster + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.HolidayMaster + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_CaptionExist").ToString());
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_DateExist").ToString()) + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.INCORRECT)  // Leave Exist
                                        {
                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){ShowConfirmMsgLeaveExist();});", true);
                                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_LeaveExist").ToString());
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.HolidayMaster);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.Err_AddHolidayDtl;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        txtCaption.Focus();
                        GetFieldValues(ControlsEnum.TYPE);
                        SetFieldValues(ControlsEnum.TYPE);
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
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfHolidayPkListPage")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            txtCaption.Focus();
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.HOLIDAYMASTERDETAILS);
                            SetFieldValues(ControlsEnum.HOLIDAYMASTERDETAILS);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region ADDTOLIST
                    case ActionsEnum.ADDTOLIST:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideHolidayDetails", "ShowHideHolidayDetails(1);", true);
                        if (CurrHolidayDtPK > 0 || (CurrHolidayDtPK == 0 && Convert.ToInt32(hdfHolidayDtSlNo.Value) > 0)) // For Edit Item
                        {
                            objHolidayDtls = HolidayDetailList.Where(itm => itm.SLNO ==Convert.ToInt32(hdfHolidayDtSlNo.Value)).FirstOrDefault();
                            objHolidayDtls.SLNO = Convert.ToInt32(hdfHolidayDtSlNo.Value);
                            objHolidayDtls.HDL_PK = CurrHolidayDtPK;
                            objHolidayDtls.HDL_HDR_PK = CurrPK;
                            objHolidayDtls.HDL_DATE = Convert.ToDateTime(txtDate.Text);
                            objHolidayDtls.HDL_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            objHolidayDtls.HDL_NAME = txtName.Text.HtmlEncode();
                            if (Convert.ToInt32(ddlType.SelectedValue) > 0)
                            {
                                objHolidayDtls.HDL_TYPE = Convert.ToInt32(ddlType.SelectedValue);
                                objHolidayDtls.HDL_TYPE_TEXT = ddlType.SelectedItem.ToString();
                            }
                            else
                            {
                                objHolidayDtls.HDL_TYPE = 0;
                                objHolidayDtls.HDL_TYPE_TEXT = string.Empty;
                            }
                            objHolidayDtls.HDL_REMARKS = txtRemarks.Text.HtmlEncode();
                        }
                        else//For Add New Item
                        {
                            if (HolidayDetailList == null)
                            {
                                HolidayDetailList = new List<HolidayMasterDetails>();
                            }
                            else
                            {
                                var editItem = HolidayDetailList.Where(itm => itm.HDL_DATE == Convert.ToDateTime(txtDate.Text) && itm.IS_DELETED != 1).FirstOrDefault();
                                if (editItem != null)
                                {
                                    litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_Date_ExistList").ToString(), txtDate.Text);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                        CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                            }
                            objHolidayDtls = new HolidayMasterDetails();
                            objHolidayDtls.SLNO = HolidayDetailList.Count+1;
                            objHolidayDtls.HDL_PK = CurrHolidayDtPK;
                            objHolidayDtls.HDL_HDR_PK = CurrPK;
                            objHolidayDtls.HDL_DATE = Convert.ToDateTime(txtDate.Text);
                            objHolidayDtls.HDL_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            objHolidayDtls.HDL_NAME = txtName.Text.HtmlEncode();
                            if (Convert.ToInt32(ddlType.SelectedValue) > 0)
                            {
                                objHolidayDtls.HDL_TYPE = Convert.ToInt32(ddlType.SelectedValue);
                                objHolidayDtls.HDL_TYPE_TEXT = ddlType.SelectedItem.ToString();
                            }
                            else
                            {
                                objHolidayDtls.HDL_TYPE = 0;
                                objHolidayDtls.HDL_TYPE_TEXT = string.Empty;
                            }
                            objHolidayDtls.HDL_REMARKS = txtRemarks.Text.HtmlEncode();
                            HolidayDetailList.Add(objHolidayDtls);
                        }
                        SetFieldValues(ControlsEnum.HOLIDAYDETAILS);
                        ResetForm(ControlsEnum.CLEARHOLIDAYDETAILS);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = HolidayMasterBL.DeleteHolidayMaster(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(PageIndex) > 1)
                            {
                                PageIndex = Convert.ToInt32(PageIndex) - 1;
                            }
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.HolidayMaster);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.HolidayMaster;
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
                                litErrorMsg.Text = Resources.PageNameRes.HolidayMaster + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.HolidayMaster + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.HolidayMaster + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.HolidayMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
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
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "HLDM" + "&APPSUBTYPE= 0" + "&CurPK=" + CurrPK) + "');", true);
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
            if (senderGridView.ID == "grdHolidayDetails")
            {
                if (e.CommandName == "EDIT_ACTION")
                {
                    ResetForm(ControlsEnum.CLEARHOLIDAYDETAILS);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideHolidayDetails", "ShowHideHolidayDetails(1);", true);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfitemPK = row.FindControl("hdfHDL_PK") as HiddenField;
                    HiddenField hdfitemSlNo = row.FindControl("hdfSlNo") as HiddenField;
                    var editItem = HolidayDetailList.Where(itm => itm.HDL_PK == Convert.ToInt32(hdfitemPK.Value) && itm.SLNO == Convert.ToInt32(hdfitemSlNo.Value)).FirstOrDefault();
                    if (editItem != null)
                    {
                        CurrHolidayDtPK = editItem.HDL_PK;
                        hdfHolidayDtSlNo.Value = editItem.SLNO.ToString();
                        txtDate.Text = Convert.ToDateTime(editItem.HDL_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                        txtName.Text = editItem.HDL_NAME.HtmlDecode();
                        TypeCurrPK = Convert.ToString(editItem.HDL_TYPE) == string.Empty ? 0 : editItem.HDL_TYPE;
                        GetFieldValues(ControlsEnum.TYPE);
                        SetFieldValues(ControlsEnum.TYPE);
                        if (editItem.HDL_TYPE > 0)
                        {
                            ddlType.SelectedValue = editItem.HDL_TYPE.ToString();
                        }
                        else
                        {
                            ddlType.SelectedIndex = 0;
                        }

                        txtRemarks.Text = editItem.HDL_REMARKS.HtmlDecode();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DateChangeCallBack", "onAfterDateChangeCallBack();", true);
                    }

                }
                else if (e.CommandName == "DELETE_ACTION")
                {
                    ResetForm(ControlsEnum.CLEARHOLIDAYDETAILS);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfitemPK = row.FindControl("hdfHDL_PK") as HiddenField;
                    HiddenField hdfitemSlNo = row.FindControl("hdfSlNo") as HiddenField;
                    HolidayMasterDetails detail = HolidayDetailList
                        .Where(x => x.SLNO == Convert.ToInt32(hdfitemSlNo.Value) && x.HDL_PK == Convert.ToInt32(hdfitemPK.Value))
                        .SingleOrDefault();
                    if (detail != null)
                    {
                        List<HolidayMasterDetails> tempList = HolidayDetailList;
                        if (hdfitemPK.Value == "0") tempList.Remove(detail);
                        else detail.IS_DELETED = 1;
                        HolidayDetailList = tempList;

                        SetFieldValues(ControlsEnum.HOLIDAYDETAILS);
                        ResetForm(ControlsEnum.CLEARHOLIDAYDETAILS);
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailed;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideHolidayDetails", "ShowHideHolidayDetails();", true);
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
            //  EntryStatus = EntryStatus.LISTMODE;
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
            EDIT,
            ADDHOLIDAYMASTERHDR,
            HOLIDAYDETAILS,
            CLEARADDTOLIST,
            HOLIDAYMASTERDETAILS,
            CLEARHOLIDAYDETAILS
        }
        #endregion
    }
}