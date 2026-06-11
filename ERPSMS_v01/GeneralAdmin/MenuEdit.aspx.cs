using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.Administration.Masters;
using BusinessLogic.Administration.Configurations;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using CustomControls;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class MenuEdit : ERP.Store.UI.MyBasePage
    {
        #region Variables
        private ActionsEnum commonActions;
        private BusinessObject.User currentUser;
        private DataTable dtSections;
        private DataTable dtMenuGroups;
        private DataTable dtMenu;
        private int CurrentPK = 0;
        private string CurrentModDate = string.Empty;
        private int CurrentType = 0;
        private int SectionPK = 0;
        private int MenuGroupPK = 0;
        private int MenuPK = 0;
        #endregion

        #region Properties
        public bool IsSuperAdmin
        {
            get
            {
                return this.ViewState["IsSuperAdmin"] == null ? false : Convert.ToBoolean(this.ViewState["IsSuperAdmin"]);
            }
            set
            {
                this.ViewState["IsSuperAdmin"] = value;
            }
        }
        /// <summary>
        ///Section ID
        /// </summary>
        private int SectionId
        {
            get
            {
                return this.ViewState["SectionId"] == null ? 0 : (int)this.ViewState["SectionId"];
            }
            set
            {
                this.ViewState["SectionId"] = value;
            }
        }

        /// <summary>
        ///Section ID
        /// </summary>
        private int MenuGroupId
        {
            get
            {
                return this.ViewState["MenuGroupId"] == null ? 0 : (int)this.ViewState["MenuGroupId"];
            }
            set
            {
                this.ViewState["MenuGroupId"] = value;
            }
        }

        /// <summary>
        ///TO store type from Section ,Menu group and Menu
        /// </summary>
        private int EditType
        {
            get
            {
                return this.ViewState["EditType"] == null ? 0 : (int)this.ViewState["EditType"];
            }
            set
            {
                this.ViewState["EditType"] = value;
            }
        }


        #endregion

        #region Page Level Events
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                IsSuperAdmin = IsSuperAdminUser(currentUser.PKUser);
                GetFieldValues((int)ControlsEnum.SECTIONS);
                SetFieldValues((int)ControlsEnum.SECTIONS);
            }
        }
        #endregion

        #region Action Handlers

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            int result = 0;
            GridViewRow gvr;
            GridView grd;
            ExtGridView egrd;
            string arg;
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
            switch (commonActions)
            {
                #region SHOW MENUGROUP
                case ActionsEnum.MENUGROUPDETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdGroupDetails") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            dtMenuGroups = null;
                        }
                        else
                        {
                            SectionId = Convert.ToInt32(arg);
                            GetFieldValues((int)ControlsEnum.MENUGROUP);
                        }
                        grd.Visible = true;
                        if (dtMenuGroups != null && dtMenuGroups.Rows.Count > 0)
                        {
                            grd.DataSource = dtMenuGroups;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedSectionItem") as HiddenField).Value = "1";
                    }
                    break;
                #endregion
                #region SHOW MENUDETAILS
                case ActionsEnum.MENUDETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdMenuDetails") as GridView;
                        if (string.IsNullOrEmpty(arg))
                        {
                            dtMenu = null;
                        }
                        else
                        {
                            MenuGroupId = Convert.ToInt32(arg);
                            GetFieldValues((int)ControlsEnum.MENU);
                        }
                        grd.Visible = true;
                        if (dtMenu != null && dtMenu.Rows.Count > 0)
                        {
                            grd.DataSource = dtMenu;
                            grd.DataBind();
                        }
                        (gvr.FindControl("hdfIsExpandedMenuGroupItem") as HiddenField).Value = "1";
                    }
                    break;
                #endregion
                #region ACTIVATE
                case ActionsEnum.ACTIVATE:
                    if (sender is ImageButton)
                    {
                        string TypeName = string.Empty;
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurrentType = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfType") as HiddenField).Value);
                        if (CurrentType == (int)ControlsEnum.SECTIONS)
                        {
                            CurrentPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfSectionPk") as HiddenField).Value);
                            CurrentModDate = ((HiddenField)gvr.FindControl("hdfSectionModDate") as HiddenField).Value;
                            result = MenuManagement.UpdateSectionDetails(CurrentPK, CurrentModDate, null, null, Convert.ToInt32(DbActiveStatus.ACTIVE).ToString());
                            TypeName = GetLocalResourceObject("Section").ToString();
                        }
                        else if (CurrentType == (int)ControlsEnum.MENUGROUP)
                        {
                            CurrentPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfMenuGroupPk") as HiddenField).Value);
                            CurrentModDate = ((HiddenField)gvr.FindControl("hdfMenuGroupModDate") as HiddenField).Value;
                            result = MenuManagement.UpdateMenuGroupDetails(CurrentPK, CurrentModDate, null, null, Convert.ToInt32(DbActiveStatus.ACTIVE).ToString());
                            TypeName = GetLocalResourceObject("MenuGroup").ToString();
                        }
                        else if (CurrentType == (int)ControlsEnum.MENU)
                        {
                            CurrentPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfMenuPk") as HiddenField).Value);
                            CurrentModDate = ((HiddenField)gvr.FindControl("hdfMenuModDate") as HiddenField).Value;
                            result = MenuManagement.UpdateMenuDetails(CurrentPK, CurrentModDate, null, null, Convert.ToInt32(DbActiveStatus.ACTIVE).ToString());
                            TypeName = GetLocalResourceObject("Menu").ToString();
                        }

                        if (result > 0)
                        {
                            GetFieldValues((int)ControlsEnum.SECTIONS);
                            SetFieldValues((int)ControlsEnum.SECTIONS);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, TypeName);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            // if error or exception occur
                            DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETED:
                                    GetFieldValues((int)ControlsEnum.SECTIONS);
                                    SetFieldValues((int)ControlsEnum.SECTIONS);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Already_Deleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;

                                case DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                    }
                    break;
                #endregion
                #region DEACTIVATE
                case ActionsEnum.DEACTIVATE:
                    if (sender is ImageButton)
                    {
                        string TypeName = string.Empty;
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurrentType = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfType") as HiddenField).Value);
                        if (CurrentType == (int)ControlsEnum.SECTIONS)
                        {
                            CurrentPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfSectionPk") as HiddenField).Value);
                            CurrentModDate = ((HiddenField)gvr.FindControl("hdfSectionModDate") as HiddenField).Value;
                            result = MenuManagement.UpdateSectionDetails(CurrentPK, CurrentModDate, null, null, Convert.ToInt32(DbActiveStatus.INACTIVE).ToString());
                            TypeName = GetLocalResourceObject("Section").ToString();
                        }
                        else if (CurrentType == (int)ControlsEnum.MENUGROUP)
                        {
                            CurrentPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfMenuGroupPk") as HiddenField).Value);
                            CurrentModDate = ((HiddenField)gvr.FindControl("hdfMenuGroupModDate") as HiddenField).Value;
                            result = MenuManagement.UpdateMenuGroupDetails(CurrentPK, CurrentModDate, null, null, Convert.ToInt32(DbActiveStatus.INACTIVE).ToString());
                            TypeName = GetLocalResourceObject("MenuGroup").ToString();
                        }
                        else if (CurrentType == (int)ControlsEnum.MENU)
                        {
                            CurrentPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfMenuPk") as HiddenField).Value);
                            CurrentModDate = ((HiddenField)gvr.FindControl("hdfMenuModDate") as HiddenField).Value;
                            result = MenuManagement.UpdateMenuDetails(CurrentPK, CurrentModDate, null, null, Convert.ToInt32(DbActiveStatus.INACTIVE).ToString());
                            TypeName = GetLocalResourceObject("Menu").ToString();
                        }
                        if (result > 0)
                        {
                            GetFieldValues((int)ControlsEnum.SECTIONS);
                            SetFieldValues((int)ControlsEnum.SECTIONS);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, TypeName);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            // if error or exception occur
                            DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETED:
                                    GetFieldValues((int)ControlsEnum.SECTIONS);
                                    SetFieldValues((int)ControlsEnum.SECTIONS);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Already_Deleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;

                                case DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                    }
                    break;
                #endregion
                #region UPDATE
                case ActionsEnum.SAVE:
                    SaveDetails(EditType);
                    break;
                #endregion
                #region EDIT NAME
                case ActionsEnum.EDIT_ACTION:
                    if (((ImageButton)sender).ID == "imbEditSectionName")
                    {
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        EditType = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfType") as HiddenField).Value);
                        hdfCurrentPK.Value = ((HiddenField)gvr.FindControl("hdfSectionPk") as HiddenField).Value;
                        hdfCurrentModDate.Value = ((HiddenField)gvr.FindControl("hdfSectionModDate") as HiddenField).Value;
                        hdfCurrentActiveStatus.Value = ((HiddenField)gvr.FindControl("hdfSecActiveStatus") as HiddenField).Value;
                        txtDefaultName.Text = ((Label)gvr.FindControl("lblSectionName") as Label).Text;
                        txtForeignName.Text=((Label)gvr.FindControl("lblSectionNameForeign") as Label).Text;
                        lblDescription.Text = ((Label)gvr.FindControl("lblSecDescription") as Label).Text;
                        ShowPopup(EditType);
                    }
                    else if (((ImageButton)sender).ID == "imbEditMenuGroupName")
                    {
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        EditType = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfType") as HiddenField).Value);
                        hdfCurrentPK.Value = ((HiddenField)gvr.FindControl("hdfMenuGroupPk") as HiddenField).Value;
                        hdfCurrentModDate.Value = ((HiddenField)gvr.FindControl("hdfMenuGroupModDate") as HiddenField).Value;
                        hdfCurrentActiveStatus.Value = ((HiddenField)gvr.FindControl("hdfMenuGroupActiveStatus") as HiddenField).Value;
                        txtDefaultName.Text = ((Label)gvr.FindControl("lblMenuGroupName") as Label).Text;
                        txtForeignName.Text = ((Label)gvr.FindControl("lblMenuGroupNameForeign") as Label).Text;
                        lblDescription.Text = ((Label)gvr.FindControl("lblMenuGroupDescription") as Label).Text;
                        ShowPopup(EditType);
                    }
                    else if (((ImageButton)sender).ID == "imbEditMenuName")
                    {
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        EditType = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfType") as HiddenField).Value);
                        hdfCurrentPK.Value = ((HiddenField)gvr.FindControl("hdfMenuPk") as HiddenField).Value;
                        hdfCurrentModDate.Value = ((HiddenField)gvr.FindControl("hdfMenuModDate") as HiddenField).Value;
                        hdfCurrentActiveStatus.Value = ((HiddenField)gvr.FindControl("hdfMenuActiveStatus") as HiddenField).Value;
                        txtDefaultName.Text = ((Label)gvr.FindControl("lblMenuName") as Label).Text;
                        txtForeignName.Text = ((Label)gvr.FindControl("lblMenuNameForeign") as Label).Text;
                        lblDescription.Text = ((Label)gvr.FindControl("lblMenuDescription") as Label).Text;
                        ShowPopup(EditType);
                    }
                    break;
                #endregion
            }
        }
        #endregion
        #endregion

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //if (((GridView)sender).ID == "grdGroupDetails")
                //{
                //    if (e.Row.RowType == DataControlRowType.DataRow)
                //    {
                if ((sender as GridView).ID == "grdSectionList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ImageButton ImbSectionActive = e.Row.FindControl("imbSectionActive") as ImageButton;
                        ImageButton ImbSectionInActive = e.Row.FindControl("imbSectionInActive") as ImageButton;
                        ImbSectionActive.Enabled = IsSuperAdmin;
                        ImbSectionInActive.Enabled = IsSuperAdmin;
                    }

                }
                if ((sender as GridView).ID == "grdGroupDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ImageButton ImbGroupActive = e.Row.FindControl("imbMenuGroupActive") as ImageButton;
                        ImageButton ImbGroupInActive = e.Row.FindControl("imbMenuGroupInActive") as ImageButton;
                        ImbGroupActive.Enabled = IsSuperAdmin;
                        ImbGroupInActive.Enabled = IsSuperAdmin;
                    }
                }
                if ((sender as GridView).ID == "grdMenuDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ImageButton ImbMenuActive = e.Row.FindControl("imbMenuActive") as ImageButton;
                        ImageButton ImbMenuInActive = e.Row.FindControl("imbMenuInActive") as ImageButton;
                        ImbMenuActive.Enabled = IsSuperAdmin;
                        ImbMenuInActive.Enabled = IsSuperAdmin;
                    }
                }

            }
            catch (Exception ex)
            { }
        }

        #region Helper Methods
        private bool IsSuperAdminUser(int pkUser)
        {
            bool retVal = false;
            DataTable dtResult = UserManagementBL.SuperAdminMstGet(pkUser);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                retVal = dtResult.Rows[0]["usrIsSuperAdmin"].ToString() == "1" ? true : false;
            }
            return retVal;
        }

        #region Get Field Values
        private void GetFieldValues(int Type)
        {
            try
            {
                switch (Type)
                {
                    case (int)ControlsEnum.SECTIONS:
                        dtSections = MenuManagement.GetSectionList(SectionPK, IsSuperAdmin ? -1 : Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.CurrentSBUPK);
                        break;
                    case (int)ControlsEnum.MENUGROUP:
                        dtMenuGroups = MenuManagement.GetMenuGroupList(MenuGroupPK, IsSuperAdmin ? -1 : Convert.ToInt32(DbActiveStatus.ACTIVE), SectionId);
                        break;
                    case (int)ControlsEnum.MENU:
                        dtMenu = MenuManagement.GetMenuList(MenuPK, IsSuperAdmin ? -1 : Convert.ToInt32(DbActiveStatus.ACTIVE), MenuGroupId);
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
        #endregion

        #region Get Field Values
        private void SetFieldValues(int Type)
        {
            try
            {
                switch (Type)
                {
                    case (int)ControlsEnum.SECTIONS:
                        BindGrid(ControlsEnum.SECTIONS);
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
                    case ControlsEnum.SECTIONS:
                        grdSectionList.DataSource = dtSections;
                        grdSectionList.DataBind();
                        break;
                    case ControlsEnum.MENUGROUP:
                        if (grdSectionList.Rows.Count > 0 )
                        {
                            GridView grd = new GridView();
                            for (int i = 0; i < grdSectionList.Rows.Count; i++)
                            {
                                grd = grdSectionList.FindControl("grdGroupDetails") as ExtGridView;
                            }
                            if (dtMenuGroups != null && dtMenuGroups.Rows.Count > 0)
                            {
                                grd.DataSource = dtMenuGroups;
                                grd.DataBind();
                            }
                            else
                            {
                                grd.DataSource = null;
                                grd.DataBind();
                            }
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

        #region Show PopUp
        /// <summary>
        /// Show Section Details Popup
        /// </summary>
        private void ShowPopup(int EditType)
        {
            string HeaderName = string.Empty;
            if (EditType == (int)ControlsEnum.SECTIONS)
                HeaderName = GetLocalResourceObject("SectionPopupHeader").ToString();
            else if (EditType == (int)ControlsEnum.MENUGROUP)
                HeaderName = GetLocalResourceObject("MenuGroupPopupHeader").ToString();
            else if (EditType == (int)ControlsEnum.MENU)
                HeaderName = GetLocalResourceObject("MenuNamePopupHeader").ToString();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSectionEdit]','" + HeaderName + "','600','200');", true);
        }

        #endregion

        #region Save
        private void SaveDetails(int EditType)
        {
            int result = 0;
            string TypeName = string.Empty;
            if (EditType == (int)ControlsEnum.SECTIONS)
            {
                result = MenuManagement.UpdateSectionDetails(Convert.ToInt32(hdfCurrentPK.Value), hdfCurrentModDate.Value, txtDefaultName.Text, txtForeignName.Text, hdfCurrentActiveStatus.Value);
                TypeName = GetLocalResourceObject("SectionName").ToString();
            }
            else if (EditType == (int)ControlsEnum.MENUGROUP)
            {
                result = MenuManagement.UpdateMenuGroupDetails(Convert.ToInt32(hdfCurrentPK.Value), hdfCurrentModDate.Value, txtDefaultName.Text,txtForeignName.Text, hdfCurrentActiveStatus.Value);
                TypeName = GetLocalResourceObject("MenuGroupName").ToString();
            }
            else if (EditType == (int)ControlsEnum.MENU)
            {
                result = MenuManagement.UpdateMenuDetails(Convert.ToInt32(hdfCurrentPK.Value), hdfCurrentModDate.Value, txtDefaultName.Text, txtForeignName.Text, hdfCurrentActiveStatus.Value);
                TypeName = GetLocalResourceObject("MenuName").ToString();
            }

            if (result > 0)
            {
                GetFieldValues((int)ControlsEnum.SECTIONS);
                SetFieldValues((int)ControlsEnum.SECTIONS);
                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                litErrorMsg.Text = string.Format(litErrorMsg.Text, TypeName);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
            }
            else
            {
                // if error or exception occur
                DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
                switch (dBActiveInactiveStatus)
                {
                    // For Sql Error
                    case DBActiveInactiveStatus.SQLERROR:
                        //Scrip register for hiding the Details Part
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        break;
                    case DBActiveInactiveStatus.DELETED:
                        GetFieldValues((int)ControlsEnum.SECTIONS);
                        SetFieldValues((int)ControlsEnum.SECTIONS);
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Already_Deleted;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        break;

                    case DBActiveInactiveStatus.CONCURRENCY:
                        //Scrip register for hiding the Details Part
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        break;
                    case DBActiveInactiveStatus.DELETECONCURRENCY:
                        //Scrip register for hiding the Details Part
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        break;
                    default:
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        break;
                }
            }
        }
        #endregion
        #endregion

        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            SECTIONS = 1,
            MENUGROUP = 2,
            MENU = 3
        }
        #endregion
    }
}
