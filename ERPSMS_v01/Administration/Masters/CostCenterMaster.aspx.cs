using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject;
using System.Data;
using ERPSMS_v01.UserControls;
using BusinessObject.Administration.Masters;
using BusinessObject.CommonManagement;
using BusinessLogic.Administration.Masters;
using ERPService;
using ERPData;
using ERPManager;
//using gAssetsManager;

namespace ERPSMS_v01.Administration.Masters
{
    public partial class CostCenterMaster : ERP.Store.UI.MyBasePage
    {
        //#region Properties & Variables

        //#region Porperties

        ///// <summary>
        ///// Current PK
        ///// </summary>
        //private int CurrPK
        //{
        //    get
        //    {
        //        return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.CurrPK] = value;
        //    }
        //}
        ///// <summary>
        ///// To maintain the PageIndex in viewstate
        ///// </summary>
        //private int PageIndex
        //{
        //    get
        //    {
        //        return (int)this.ViewState[ViewstateStrings.PageIndex];
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.PageIndex] = value;
        //    }
        //}
        //private int PageSize
        //{
        //    get
        //    {
        //        return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
        //    }
        //}
        //private int TotalPages
        //{
        //    get
        //    {
        //        return (int)(this.ViewState[ViewstateStrings.TotalPages] ?? 1);
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.TotalPages] = value;
        //    }
        //}
        //private EntryStatus EntryStatus
        //{
        //    get
        //    {
        //        return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.EntryState] = value;
        //    }
        //}
        //private DateTime LastModifiedTime
        //{
        //    get
        //    {
        //        return this.ViewState[ViewstateStrings.LastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.LastModifiedTime];
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.LastModifiedTime] = value;
        //    }
        //}
        //#endregion

        //#region Variables

        //private ActionsEnum commonActions;
        //private ControlsEnum controlEnum;
        //User currentUser;
        //private ADM_COMPANY_MST admCompanyMstObj;
        //private DataTable dtCostCenter;
        //private DataTable dtResult;
        //private DataTable dtGroup;
        ////private DataTable admCompanyMstList;
        //private List<ADM_COMPANY_MST> admCompanyMstList;
        //private CostCenterMasterBO objCostCenter;
        //private DataTable dtCompany;

        //#endregion

        //#endregion

        //#region Page Level Events

        //protected void Page_Init(object sender, System.EventArgs e)
        //{
        //    uclPaging.CurrentPage = 1;
        //}

        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    InitializeComponent();
        //}

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    PageActionHandler();
        //}

        //protected void Page_PreRender(Object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
        //        if (EntryStatus == EntryStatus.EDITMODE)
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
        //            lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
        //            lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
        //        }
        //        else if (EntryStatus == EntryStatus.NEWMODE)
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
        //            lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
        //            lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
        //        }
        //        else if (EntryStatus == EntryStatus.ENTRYMODE)
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
        //            lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
        //            lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
        //        }
        //        else if (EntryStatus == EntryStatus.LISTMODE)
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
        //            lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
        //            lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
        //        }
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}

        //private void PageActionHandler()
        //{
        //    try
        //    {
        //        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        //        InitializeComponent();
        //        if (!IsPostBack)
        //        {
        //            this.PageIndex = 1;
        //            uclPaging.CurrentPage = 1;
        //            GetFieldValues(ControlsEnum.LIST);
        //            SetFieldValues(ControlsEnum.LIST);
        //            GetFieldValues(ControlsEnum.GETGROUP);
        //            SetFieldValues(ControlsEnum.GETGROUP);
        //            GetFieldValues(ControlsEnum.COMPANY);
        //            SetFieldValues(ControlsEnum.COMPANY);
        //            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
        //            if (dtCompany != null && dtCompany.Rows.Count > 0)
        //            {
        //                hdfCompany.Value = dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString();
        //            }
        //            PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //private void InitializeComponent()
        //{
        //    this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
        //    this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
        //    this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
        //    this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
        //    this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
        //    this.Init += new EventHandler(this.Page_Init);
        //}

        //#endregion

        //#region Action Handler

        //protected void ActionHandler(object sender, EventArgs e)
        //{
        //    //Session Logout on Department change
        //    if (!(this.Master as ERPSMS_2).ValidatePageDept("../../login.aspx"))
        //        return;

        //    try
        //    {
        //        int? result;
        //        int GridRowIndex = 0;
        //        GridViewRow gvRow;

        //        bool bIsChecked = false;

        //        if (sender.GetType().IsEquivalentTo(typeof(Button)))
        //        {
        //            commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
        //        }
        //        else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
        //        {
        //            commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
        //        }
        //        else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
        //        {
        //            commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
        //        }

        //        switch (commonActions)
        //        {
        //            #region SAVE
        //            case ActionsEnum.SAVE: if (!IsValid)
        //                {
        //                    litErrorMsg.Text = Resources.Report.Msg_Save_Error;
        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                }
        //                else
        //                {
        //                    if (Convert.ToInt32(ddlGroup.SelectedValue) == -1)
        //                    {
        //                        litErrorMsg.Text = GetLocalResourceObject("Err_SelectGroup").ToString();
        //                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        objCostCenter = new CostCenterMasterBO();
        //                        objCostCenter = (CostCenterMasterBO)SetUIValuesToObject(ControlsEnum.ADDCOSTCENTER);
        //                        if (objCostCenter != null)
        //                        {
        //                            //string xmlDoc = CommonFunctions.XmlSerialize<CostCenterMasterBO>(objCostCenter);
        //                            result = CostCenterMasterBL.SaveCostCenterMaster(objCostCenter);
        //                            if (result > 0)
        //                            {
        //                                litErrorMsg.Text = GetLocalResourceObject("Msg_CostCenterSaveSuccess").ToString();
        //                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
        //                                ResetForm(ControlsEnum.CLEAR);
        //                                EntryStatus = EntryStatus.LISTMODE;
        //                                CurrPK = (int)result;
        //                                GetFieldValues(ControlsEnum.LIST);
        //                                SetFieldValues(ControlsEnum.LIST);
        //                            }
        //                            else
        //                            {
        //                                if (result == (int)DbSaveStatus.SQLERROR)
        //                                {
        //                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
        //                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
        //                                }
        //                                else if (result == (int)DbSaveStatus.CONCURRENCY)
        //                                {
        //                                    litErrorMsg.Text = Resources.PageNameRes.CostCenterMaster + " " + Resources.Messages.EditUsedByAnotherUser;
        //                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
        //                                }
        //                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
        //                                {
        //                                    litErrorMsg.Text = Resources.PageNameRes.CostCenterMaster + " " + Resources.Messages.AlreadyDeleted;
        //                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
        //                                    EntryStatus = EntryStatus.LISTMODE;
        //                                }
        //                                else if (result == (int)DbSaveStatus.ALREADYDELETED)
        //                                {
        //                                    litErrorMsg.Text = Resources.PageNameRes.CostCenterMaster + " " + string.Format(GetLocalResourceObject("Msg_AlreadyDeleted").ToString());
        //                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
        //                                }
        //                                else if (result == (int)DbSaveStatus.CODEEXIST)
        //                                {
        //                                    litErrorMsg.Text =  string.Format(GetLocalResourceObject("Msg_CodeExist").ToString());
        //                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
        //                                }
        //                                else if (result == (int)DbSaveStatus.NAMEEXIST)
        //                                {
        //                                    litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_NameExist").ToString());
        //                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
        //                                }
        //                                else
        //                                {
        //                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
        //                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster);
        //                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            litErrorMsg.Text = GetLocalResourceObject("Err_AddCostCenter").ToString();
        //                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
        //                        }
        //                    }
        //                }
        //                break;
        //            #endregion
        //            #region NEW
        //            case ActionsEnum.NEW:
        //                EntryStatus = EntryStatus.NEWMODE;
        //                txtCode.Focus();
        //                ResetForm(ControlsEnum.CLEAR);
        //                break;
        //            #endregion
        //            #region EDIT
        //            case ActionsEnum.EDIT:
        //            case ActionsEnum.DETAILS:
        //                if (IsRadioButtonSelected(ref GridRowIndex))
        //                {
        //                    txtCode.Focus();
        //                    CurrPK = Convert.ToInt32((grdCCList.Rows[GridRowIndex].FindControl("hdfCostCenterPk") as HiddenField).Value);
        //                    GetFieldValues(ControlsEnum.EDIT);
        //                    SetFieldValues(ControlsEnum.EDIT);
        //                    EntryStatus = EntryStatus.EDITMODE;
        //                }
        //                else
        //                {
        //                    litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
        //                }
        //                break;
        //            #endregion
        //            #region CANCEL
        //            case ActionsEnum.CANCEL:
        //                ResetForm(ControlsEnum.CLEAR);
        //                EntryStatus = EntryStatus.LISTMODE;
        //                break;
        //            #endregion
        //            #region DELETE
        //            case ActionsEnum.DELETE:
        //                result = CostCenterMasterBL.DeleteCostCenter(CurrPK, this.LastModifiedTime);
        //                if (result > 0)
        //                {
        //                    if (grdCCList.Rows.Count == 1 && PageIndex > 1)
        //                    {
        //                        PageIndex--;
        //                    }
        //                    ResetForm(ControlsEnum.CLEAR);
        //                    GetFieldValues(ControlsEnum.LIST);
        //                    SetFieldValues(ControlsEnum.LIST);
        //                    EntryStatus = EntryStatus.LISTMODE;
        //                    btnNew.Focus();
        //                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
        //                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("CostCenter").ToString());
        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
        //                }
        //                else
        //                {
        //                    #region Error Message
        //                    if (result == (int)DbSaveStatus.SQLERROR)
        //                    {
        //                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
        //                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
        //                            + "','" + Resources.ErpRes.Information + "');", true);
        //                    }
        //                    else if (result == (int)DbSaveStatus.CONCURRENCY)
        //                    {
        //                        litErrorMsg.Text = GetLocalResourceObject("CostCenter").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
        //                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
        //                        + "','" + Resources.ErpRes.Information + "');", true);
        //                    }
        //                    else if (result == (int)DbDeleteStatus.REFERRED)
        //                    {
        //                        litErrorMsg.Text = GetLocalResourceObject("CostCenter").ToString() + " " + Resources.Messages.UsedInAnotherPlace;
        //                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
        //                        + "','" + Resources.ErpRes.Information + "');", true);
        //                    }
        //                    else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
        //                    {
        //                        litErrorMsg.Text = GetLocalResourceObject("CostCenter").ToString() + " " + Resources.Messages.AlreadyDeleted;
        //                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
        //                        + "','" + Resources.ErpRes.Information + "');", true);
        //                    }
        //                    else
        //                    {
        //                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
        //                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
        //                            + "','" + Resources.ErpRes.Information + "');", true);
        //                    }
        //                    #endregion
        //                }
        //                break;
        //            #endregion
        //            #region ACTIVATE
        //            case ActionsEnum.ACTIVATE:
        //                gvRow = ((ImageButton)sender).Parent.Parent as GridViewRow;
        //                result = CostCenterMasterBL.UpdateCostCenterStatus(Convert.ToInt32(((HiddenField)gvRow.FindControl("hdfCostCenterPk")).Value), (int)DbActiveStatus.ACTIVE, currentUser.PKUser, null);
        //                if (result > 0)
        //                {
        //                    uclPaging.CurrentPage = 0;
        //                    this.PageIndex = 1;
        //                    this.CurrPK = 0;
        //                    GetFieldValues(ControlsEnum.LIST);
        //                    SetFieldValues(ControlsEnum.LIST);
        //                    EntryStatus = EntryStatus.LISTMODE;
        //                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
        //                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster.ToString());
        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
        //                }
        //                else
        //                {
        //                    DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
        //                    switch (dBActiveInactiveStatus)
        //                    {
        //                        // For Sql Error
        //                        case DBActiveInactiveStatus.SQLERROR:
        //                            //Scrip register for hiding the Details Part
        //                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
        //                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                            break;

        //                        case DBActiveInactiveStatus.CONCURRENCY:
        //                            //Scrip register for hiding the Details Part
        //                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
        //                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster.ToString());
        //                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                            break;
        //                        case DBActiveInactiveStatus.DELETECONCURRENCY:
        //                            //Scrip register for hiding the Details Part
        //                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
        //                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster.ToString());
        //                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                            break;
        //                        default:
        //                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
        //                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                            break;
        //                    }
        //                }
        //                break;
        //            #endregion
        //            #region INACTIVATE
        //            case ActionsEnum.INACTIVATE:
        //                gvRow = ((ImageButton)sender).Parent.Parent as GridViewRow;
        //                result = CostCenterMasterBL.UpdateCostCenterStatus(Convert.ToInt32(((HiddenField)gvRow.FindControl("hdfCostCenterPk")).Value), (int)DbActiveStatus.INACTIVE, currentUser.PKUser, null);
        //                if (result > 0)
        //                {
        //                    uclPaging.CurrentPage = 0;
        //                    this.PageIndex = 1;
        //                    this.CurrPK = 0;
        //                    GetFieldValues(ControlsEnum.LIST);
        //                    SetFieldValues(ControlsEnum.LIST);
        //                    EntryStatus = EntryStatus.LISTMODE;
        //                    litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
        //                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster.ToString());
        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
        //                }
        //                else
        //                {
        //                    // if error or exception occur
        //                    DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
        //                    switch (dBActiveInactiveStatus)
        //                    {
        //                        // For Sql Error
        //                        case DBActiveInactiveStatus.SQLERROR:
        //                            //Scrip register for hiding the Details Part
        //                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
        //                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                            break;

        //                        case DBActiveInactiveStatus.CONCURRENCY:
        //                            //Scrip register for hiding the Details Part
        //                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
        //                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster.ToString());
        //                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                            break;
        //                        case DBActiveInactiveStatus.DELETECONCURRENCY:
        //                            //Scrip register for hiding the Details Part
        //                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
        //                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CostCenterMaster.ToString());
        //                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                            break;
        //                        default:
        //                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
        //                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                            break;
        //                    }
        //                }
        //                break;
        //            #endregion
        //            #region SEARCH
        //            case ActionsEnum.SEARCH:
        //                uclPaging.CurrentPage = 0;
        //                this.PageIndex = 1;
        //                this.EntryStatus = EntryStatus.LISTMODE;
        //                this.CurrPK = 0;
        //                GetFieldValues(ControlsEnum.LIST);
        //                SetFieldValues(ControlsEnum.LIST);
        //                break;
        //            #endregion
        //            #region CLEAR
        //            case ActionsEnum.CLEAR:
        //            case ActionsEnum.LIST:
        //                ResetForm(ControlsEnum.CLEARSEARCH);
        //                uclPaging.CurrentPage = 0;
        //                this.PageIndex = 1;
        //                this.EntryStatus = EntryStatus.LISTMODE;
        //                this.CurrPK = 0;
        //                GetFieldValues(ControlsEnum.LIST);
        //                SetFieldValues(ControlsEnum.LIST);
        //                break;
        //            #endregion
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        //{
        //    try
        //    {
        //        switch (e.Action)
        //        {
        //            case NavigationEnum.PAGECHANGE:
        //                uclPaging.CurrentPage = e.CurrentPage;
        //                break;
        //            case NavigationEnum.FIRST:
        //                if (e.CurrentPage > 1)
        //                    uclPaging.CurrentPage = 1;
        //                break;
        //            case NavigationEnum.LAST:
        //                if (e.CurrentPage <= e.TotalPages)
        //                    uclPaging.CurrentPage = e.TotalPages;
        //                break;
        //            case NavigationEnum.NEXT:
        //                if (e.CurrentPage <= e.TotalPages)
        //                    uclPaging.CurrentPage++;
        //                break;
        //            case NavigationEnum.PREVIOUS:
        //                if (e.CurrentPage > 1)
        //                    uclPaging.CurrentPage--;
        //                break;
        //        }
        //        PageIndex = uclPaging.CurrentPage;
        //        GetFieldValues(ControlsEnum.LIST);
        //        SetFieldValues(ControlsEnum.LIST);
        //        EnableDisableButtons(e.TotalPages, "uclPaging");
        //        EntryStatus = EntryStatus.LISTMODE;
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}

        //protected void ActionHandler(object sender, GridViewPageEventArgs e)
        //{
        //    PageIndex = e.NewPageIndex;
        //}

        //#endregion

        //#region Get Field Values
        //private void GetFieldValues(ControlsEnum type)
        //{
        //    AdmCompanyMstService admCompanyMstServiceClient;
        //    ServiceUtility serviceUtilityObj;
        //    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
           
        //    try
        //    {
        //        switch (type)
        //        {
        //            case ControlsEnum.LIST:
        //                BusinessObject.GridPrams gridParam;
        //                gridParam = new BusinessObject.GridPrams();
        //                gridParam.PageNumber = (PageIndex == 0) ? 1 : PageIndex;
        //                gridParam.PageSize = PageSize;
        //                dtCostCenter = CostCenterMasterBL.GetCostCenterList(gridParam, currentUser.SBUID, txtCostCenterCode.Text.Trim(), txtCostCenterName.Text.Trim());
        //                break;
        //            case ControlsEnum.EDIT:
        //                dtResult = CostCenterMasterBL.GetCostCenterByPK(CurrPK, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID);
        //                break;
        //            case ControlsEnum.GETGROUP:
        //                dtGroup = CostCenterMasterBL.GetCostCenterGroup(currentUser.CurrentSBUPK, Convert.ToInt32(GroupEnum.GroupType), Convert.ToInt32(GroupEnum.GroupValue));
        //                break;
        //            case ControlsEnum.COMPANY:
        //                admCompanyMstServiceClient = new AdmCompanyMstService();
        //                admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
        //                admCompanyMstObj.CMP_ACTIVE = 1;
        //                serviceUtilityObj = new ServiceUtility();
        //                admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //#endregion

        //#region Set Field Values
        //private void SetFieldValues(ControlsEnum type)
        //{
        //    try
        //    {
        //        switch (type)
        //        {
        //            case ControlsEnum.LIST:
        //                BindGrid(ControlsEnum.LIST);
        //                break;
        //            case ControlsEnum.EDIT:
        //                GetUIValuesFromObject(ControlsEnum.EDIT);
        //                break;
        //            case ControlsEnum.GETGROUP:
        //                BindDropdown(ControlsEnum.GETGROUP);
        //                break;
        //            case ControlsEnum.COMPANY:
        //                BindDropdown(ControlsEnum.COMPANY);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //#endregion

        //#region Get UI Values From Object

        //private void GetUIValuesFromObject(ControlsEnum controlType)
        //{
        //    try
        //    {
        //        switch (controlType)
        //        {
        //            case ControlsEnum.EDIT:
        //                if (dtResult != null && dtResult.Rows.Count > 0)
        //                {
        //                    txtCode.Text = dtResult.Rows[0]["CNM_CODE"].ToString();
        //                    txtName.Text = dtResult.Rows[0]["CNM_NAME"].ToString();
        //                    txtDescription.Text = dtResult.Rows[0]["CNM_DESC"].ToString();
        //                    ddlGroup.SelectedValue = dtResult.Rows[0]["CNM_GROUP"].ToString();
        //                    if (!string.IsNullOrEmpty(Convert.ToString(dtResult.Rows[0]["CNM_COMPANY"])))
        //                    {
        //                        ddlCompany.SelectedValue = dtResult.Rows[0]["CNM_COMPANY"].ToString();
        //                    }
        //                    else
        //                    {
        //                        ddlCompany.SelectedValue = CommonConstants.SELECTVAL;
        //                    }
        //                    chkActive.Checked = Convert.ToBoolean(dtResult.Rows[0]["CNM_ACTIVE"]);
        //                    LastModifiedTime = Convert.ToDateTime(dtResult.Rows[0]["CNM_MOD_DT"]);
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        //#endregion

        //#region Set UI Values To Object

        //private Object SetUIValuesToObject(ControlsEnum controlType)
        //{
        //    Object retObject;
        //    retObject = null;
        //    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        //    try
        //    {
        //        switch (controlType)
        //        {
        //            case ControlsEnum.ADDCOSTCENTER:
        //                objCostCenter.CNM_PK = CurrPK;
        //                objCostCenter.CNM_CODE = txtCode.Text.Trim();
        //                objCostCenter.CNM_NAME = txtName.Text.Trim();
        //                objCostCenter.CNM_DESC = txtDescription.Text;
        //                objCostCenter.CNM_ACTIVE = chkActive.Checked == true ? 1 : 0;
        //                objCostCenter.CNM_DEPT = currentUser.CurrentDeptPK;
        //                objCostCenter.CNM_BIZUNIT = currentUser.SBUID;
        //                //if (Convert.ToInt32(hdfCompany.Value) > 0)
        //                //{
        //                //    objCostCenter.CNM_COMPANY = Convert.ToInt32(hdfCompany.Value);
        //                //}
                       
        //                objCostCenter.CNM_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
        //                objCostCenter.CNM_GROUP = Convert.ToInt32(ddlGroup.SelectedValue);
        //                objCostCenter.USER_PK = currentUser.PKUser;
        //                objCostCenter.LAST_MOD_DT = LastModifiedTime;
        //                retObject = objCostCenter;
        //                break;
        //            default:
        //                break;
        //        }
        //        return retObject;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        //#endregion

        //#region Bind Grid

        //public void BindGrid(ControlsEnum controlType)
        //{
        //    try
        //    {
        //        switch (controlType)
        //        {
        //            case ControlsEnum.LIST:
        //                uclPaging.Visible = false;
        //                if (dtCostCenter != null && dtCostCenter.Rows.Count > 0)
        //                {
        //                    int rowCount = 0;

        //                    rowCount = Convert.ToInt32(dtCostCenter.Rows[0]["TOTAL_ROW_COUNT"].ToString());
        //                    this.TotalPages = Convert.ToInt32(dtCostCenter.Rows[0]["ROW_NO"].ToString());

        //                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
        //                                     (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
        //                                     (rowCount / this.PageSize) + 1;

        //                    PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
        //                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
        //                    grdCCList.DataSource = dtCostCenter;
        //                    grdCCList.DataBind();

        //                    uclPaging.Visible = true;
        //                    uclPaging.BindPager();
        //                }
        //                else
        //                {
        //                    grdCCList.DataSource = null;
        //                    grdCCList.DataBind();
        //                    uclPaging.BindPager();
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //#endregion

        //#region Bind Dropdown
        //public void BindDropdown(ControlsEnum controlType)
        //{
        //    try
        //    {
        //        switch (controlType)
        //        {
        //            case ControlsEnum.GETGROUP:
        //                ddlGroup.Items.Clear();
        //                if (dtGroup != null && dtGroup.Rows.Count > 0)
        //                {
        //                    dtGroup = CommonFunctions.HtmlDecodeDataTable(dtGroup, "CON_NAME_TEXT"); //Decode DataTable
        //                    ddlGroup.DataTextField = "CON_NAME_TEXT";
        //                    ddlGroup.DataValueField = "CON_PK";
        //                    ddlGroup.DataSource = dtGroup;
        //                    ddlGroup.DataBind();
        //                }
        //                ddlGroup.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
        //                break;

        //            case ControlsEnum.COMPANY:

        //                ddlCompany.Items.Clear();
        //              if (admCompanyMstList != null && admCompanyMstList.Count > 0)
        //            {
        //                ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
        //                ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
        //                ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
        //                ddlCompany.DataBind();
        //            }
        //            ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        //            break;
        //            default: break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //#endregion

        //#region Reset
        //private void ResetForm(ControlsEnum controlType)
        //{
        //    switch (controlType)
        //    {
        //        case ControlsEnum.CLEAR:
        //            txtCode.Text = txtName.Text = txtDescription.Text = string.Empty;
        //            txtCostCenterCode.Text = txtCostCenterName.Text = string.Empty;
        //            ddlGroup.SelectedIndex = -1;
        //            chkActive.Checked = true;
        //            CurrPK = 0;
        //            break;
        //        case ControlsEnum.CLEARSEARCH:
        //            uclPaging.CurrentPage = 0;
        //            PageIndex = 1;
        //            txtCostCenterCode.Text = txtCostCenterName.Text = string.Empty;
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //#endregion

        //#region EnableDisableButtons
        ///// <summary>
        ///// Methord used to enable and Disable Page Navigation Controls
        ///// </summary>
        ///// <param name="iTotalPages"></param>
        //private void EnableDisableButtons(int iTotalPages, string pagerId)
        //{
        //    if (pagerId == "uclPaging")
        //    {
        //        uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the first link
        //        uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the previous link
        //        uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false; // Should we enable the next link
        //        uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;// Should we enable the last link
        //    }
        //}
        //#endregion

        //#region Enum
        //public enum ControlsEnum
        //{
        //    LIST,
        //    CLEAR,
        //    CLEARSEARCH,
        //    TYPE,
        //    EDIT,
        //    ADDCOSTCENTER,
        //    GETGROUP,
        //    COMPANY,
        //}
        //public enum GroupEnum
        //{
        //    GroupType = 27,  //P_CGT_VALUE
        //    GroupValue = 2  //P_CNG_VALUE
        //}
        //#endregion

        //#region Helper Methods
        ///// <summary>
        ///// for checking radio button selected in main grid
        ///// </summary>
        ///// <param name="GridRowIndex"></param>
        ///// <returns></returns>
        //private bool IsRadioButtonSelected(ref int GridRowIndex)
        //{
        //    RadioButton rbtn;
        //    foreach (GridViewRow grdrow in grdCCList.Rows)
        //    {
        //        rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
        //        if (rbtn.Checked)
        //        {
        //            GridRowIndex = grdrow.RowIndex;
        //            return true;
        //        }
        //    }
        //    return GridRowIndex == 0 ? false : true;
        //}
        //#endregion
    }
}