using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.Administration.Configurations;
using ERPSMS_v01.Administration.Masters;
using ERP.Utilities;
using BusinessLogic.Administration.Configurations;
using BusinessLogic.CommonManagement;
using System.Configuration;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class InboxUserMapping : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region private properties
        /// <summary>
        /// Keep Module Users
        /// </summary>
        private DataTable ModuleUsers
        {
            get
            {
                return this.ViewState["ModuleUsers"] == null ? null : (DataTable)this.ViewState["ModuleUsers"];
            }
            set
            {
                this.ViewState["ModuleUsers"] = value;
            }
        }
        #endregion
        #region private variables
        BusinessObject.User currentUser;
        //User Module details
        private DataTable dtUserModuleDtl;
        //User module Count
        private DataSet dsUserModuleCount;
        //Module Users details
        private DataSet dsModuleUsersDtl;
        //Module Dropdown
        private DataTable dtModuleDdl;
        //Department Dropdown
        private DataTable dtDept;
        BusinessObject.User objUser;
        // List variables for binding details to controls
        private ActionsEnum commonActions;
        InboxUserMapingBO objInboxUserMaping;
        long? result;
        int searchUser;
        int searchRole;
        int searchDepartment;
        int userLogin=0;
        //New Code

        #endregion
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.MODULEDDL:
                        dtModuleDdl = UserManagementBL.GetModuleDDL(1);
                        break;
                    case ControlsEnum.DEPARTMENTDDL:
                        objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        Int16 module;
                        Int16.TryParse(ConfigurationManager.AppSettings[ERP.Utilities.ConfigStrings.GERPModule], out module);
                        dtDept = BusinessLogic.CommonManagement.CommonBL.GetDepartment(objUser.PKUser, objUser.SBUID, module);
                        break;
                    case ControlsEnum.DEFAULT:
                        dsModuleUsersDtl = UserManagementBL.GetUsersGroupList(0, 0, 0,0,8);
                        break;
                    case ControlsEnum.SEARCH:
                        //searchUser = Convert.ToInt32(ddlUsers.SelectedValue);
                        //searchRole = Convert.ToInt32(ddlRole.SelectedValue);
                        searchUser = Convert.ToInt32(hdfUserPK.Value);
                        searchRole = Convert.ToInt32(hdfRolePK.Value);
                        searchDepartment = Convert.ToInt32(ddlDepartment.SelectedValue);
                        dsModuleUsersDtl = UserManagementBL.GetUsersGroupList(Convert.ToInt32(ddlModule.SelectedValue), searchUser, searchRole, userLogin, searchDepartment);
                        break;
                    default:
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
                    case ControlsEnum.MODULEDDL:
                        BindDropDown(ControlsEnum.MODULEDDL);
                        break;
                    case ControlsEnum.DEPARTMENTDDL:
                        BindDropDown(ControlsEnum.DEPARTMENTDDL);
                        break;
                    case ControlsEnum.DEFAULT:
                        ModuleUsers = dsModuleUsersDtl.Tables[0];
                        BindGrid(ControlsEnum.DEFAULT);
                        BindDropDown(ControlsEnum.USER);
                        BindDropDown(ControlsEnum.MODULE);
                        break;
                    case ControlsEnum.SEARCH:
                        ModuleUsers = dsModuleUsersDtl.Tables[0];
                        BindGrid(ControlsEnum.DEFAULT);
                        BindDropDown(ControlsEnum.USER);
                        BindDropDown(ControlsEnum.MODULE);
                        //ddlUsers.SelectedValue
                        break;
                    case ControlsEnum.CLEAR:

                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Page Level Events
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
           
        }
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.MODULEDDL);
                    SetFieldValues(ControlsEnum.MODULEDDL);
                    GetFieldValues(ControlsEnum.DEPARTMENTDDL);
                    SetFieldValues(ControlsEnum.DEPARTMENTDDL);
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    if (Page.Request.QueryString["UserProphile"] != null && Page.Request.QueryString["UserProphile"] == "1")
                    {
                        objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        txtUser.Text = objUser.Name.ToString();
                        hdfUserPK.Value = objUser.PKUser.ToString();
                        userLogin =Convert.ToInt32(objUser.PKUser.ToString());
                        hdfuserLogin.Value =objUser.PKUser.ToString();
                        hdfUserAutoEnable.Value = "1";
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.SEARCH);
                        txtUser.Enabled = false;
                        lblBreadCrum.Visible = false;
                        btnCancel.Visible = false;
                        ulBrudCrum.Visible = false;
                        divBtnContainer.Attributes.Add("class", "Button-iframe");
                        divTblCol1.Attributes.Add("class", "div2col-S-iframe");
                        divTblCol2.Attributes.Add("class", "div2col-S-iframe");
                    }
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
            finally
            {

            }
        }
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitComponents", "$(document).ready(function(){InitComponents();});", true);
        }
        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            int retRefID;
            retRefID = 0;
            string file = "";
            DataSet dsResult;
            GridViewRow gvr;
            try
            {
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

                    if (((DropDownList)sender).ID == "ddlModule" || ((DropDownList)sender).ID == "ddlDepartment")
                    {
                        commonActions = ActionsEnum.SEARCH;
                    }
                }

                switch (commonActions)
                {
                    #region Save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            objInboxUserMaping = (InboxUserMapingBO)SetUIValuesToObject(ActionsEnum.SAVE);
                            string xmlDoc = CommonFunctions.XmlSerialize<InboxUserMapingBO>(objInboxUserMaping);
                            result = UserManagementBL.SaveUserGroup(xmlDoc, out  retRefID);
                            if (result > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + String.Format(Resources.ErrorMessages.Msg_SavSuccess, Resources.Captions.InboxUserGroup) + "','" + Resources.Captions.Information + "');", true);

                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_UserGroup").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        Response.Redirect(Resources.PageURL.User, true);
                        break;
                    #endregion
                    #region Search
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.SEARCH);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "GetUserRoleAuto", "GetUserRoleAuto();", true);
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ddlModule.SelectedValue = CommonConstants.SELECTVAL;
                        txtUser.Text = txtRole.Text = string.Empty;
                        hdfUserPK.Value = hdfRolePK.Value = CommonConstants.SELECT_VALUE_ZERO;
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        break;
                    #endregion
                    #region Dropdown Change
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            }
            finally
            {
            }
        }

        int licenseTotal = 0;
        int usedTotal = 0;
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                licenseTotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "SYM_CFG_COUNT"));
                usedTotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "SYM_USER_COUNT"));
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbllicenseTotal = (Label)e.Row.FindControl("lblLicenseTotal");
                lbllicenseTotal.Text = licenseTotal.ToString();
                Label lblUsedTotal = (Label)e.Row.FindControl("lblUsedTotal");
                lblUsedTotal.Text = usedTotal.ToString();
            }
        }
        #endregion

        #region Pager Methods + Init
        /// <summary>
        /// Method for Page PreInit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["UserProphile"] != null && Page.Request.QueryString["UserProphile"] == "1")
                this.MasterPageFile = "~/IFrameMaster.Master";
        }
        #endregion
       
        #region Helper Methods
        private void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.MODULEDDL:
                        ddlModule.Items.Clear();
                        ddlModule.DataSource = dtModuleDdl;
                        ddlModule.DataTextField = "SYM_NAME";
                        ddlModule.DataValueField = "SYM_PK";
                        ddlModule.DataBind();
                        ddlModule.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.DEPARTMENTDDL:
                        ddlDepartment.Items.Clear();
                        ddlDepartment.DataSource = dtDept;
                        ddlDepartment.DataTextField = "DPT_NAME";
                        ddlDepartment.DataValueField = "DPT_PK";
                        ddlDepartment.DataBind();
                        //ddlDepartment.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.USER:
                        ddlUsers.Items.Clear();
                        ddlUsers.DataSource = GetModuleUsers();
                        ddlUsers.DataTextField = "gumUserText";
                        ddlUsers.DataValueField = "gumUser";
                        ddlUsers.DataBind();
                        ddlUsers.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (searchUser > 0)
                            ddlUsers.SelectedIndex = ddlUsers.Items.IndexOf(ddlUsers.Items.FindByValue(searchUser.ToString()));

                        break;
                    case ControlsEnum.MODULE:
                        ddlRole.Items.Clear();
                        ddlRole.DataSource = GetModules();
                        ddlRole.DataTextField = "gumGroupText";
                        ddlRole.DataValueField = "gumGroup";
                        ddlRole.DataBind();
                        ddlRole.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (searchRole > 0)
                            ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(searchRole.ToString()));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get Distinct Module Users from Data Table
        /// </summary>
        /// <returns></returns>
        private object GetModuleUsers()
        {
            if (ModuleUsers != null)
            {
                var distinctValues = ModuleUsers.AsEnumerable()
                              .Select(row => new
                              {
                                  gumUser = row.Field<int>("gumUser"),
                                  gumUserText = row.Field<string>("gumUserText")
                              })
                              .Distinct().OrderBy(m => m.gumUserText);
                return distinctValues;
            }
            return null;
        }
        /// <summary>
        ///  Get Distinct Modules from Data Table
        /// </summary>
        /// <returns></returns>
        private object GetModules()
        {
            if (ModuleUsers != null)
            {
                var distinctValues = ModuleUsers.AsEnumerable()
                              .Select(row => new
                              {
                                  gumGroup = row.Field<int>("gumGroup"),
                                  gumGroupText = row.Field<string>("gumGroupText")
                              })
                              .Distinct().OrderBy(m => m.gumGroupText);
                return distinctValues;
            }
            return null;
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum actionType)
        {
            object returnObj;
            InboxUserMapingBO objInboxUsrMaping = new InboxUserMapingBO();
            try
            {
                returnObj = null;
                objInboxUsrMaping.Details = new List<InboxUserMapingDetailsBO>();
                foreach (GridViewRow grdRow in grdModuleUser.Rows)
                {
                    CheckBox chkHasInInboxMsg = (CheckBox)grdRow.FindControl("chkHasInInboxMessage");
                    HiddenField hdfUsrPk = (HiddenField)grdRow.FindControl("hdfUserPk");
                    HiddenField hdfUserGrupPk = (HiddenField)grdRow.FindControl("hdfUserGroupPk");
                    InboxUserMapingDetailsBO objDtl = new InboxUserMapingDetailsBO();
                    objDtl.gumGroup = Convert.ToInt32(hdfUserGrupPk.Value);
                    objDtl.gumUser = Convert.ToInt32(hdfUsrPk.Value);
                    objDtl.gumHasInbox = chkHasInInboxMsg.Checked ? 1 : 0;
                    objInboxUsrMaping.Details.Add(objDtl);
                }
                returnObj = objInboxUsrMaping;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //returnObj = null;
            }
            return returnObj;
        }
        /// <summary>
        /// Bind Dropdown
        /// </summary>
        /// <param name="controlType"></param>
        private void BindGrid(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.DEFAULT:
                    grdModuleUser.DataSource = dsModuleUsersDtl;
                    grdModuleUser.DataBind();
                    break;
            }
        }
        /// <summary>
        /// Get User Limit Text
        /// </summary>
        /// <returns></returns>
        private string GetUserLimitText()
        {
            string retVal = string.Empty;
            DataTable dtConfig = CommonBL.GetApplicaitonConfiguaration("USER LIMIT SETTINGS", string.Empty);
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                GTIService.Utilities.CryptoServices crypto = new GTIService.Utilities.CryptoServices();
                retVal = crypto.DecryptString(dtConfig.Rows[0]["ACF_DATA"].ToString(),
                                        ConfigurationManager.AppSettings["SYSKEY"]);

            }
            return retVal;
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            VIEW,
            USER,
            MODULE,
            SEARCH,
            CLEAR,
            MODULEDDL,
            DEPARTMENTDDL
        }
        #endregion
    }
}