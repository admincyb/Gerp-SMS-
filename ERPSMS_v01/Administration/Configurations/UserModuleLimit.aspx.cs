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

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class UserModuleLimit : ERP.Store.UI.MyBasePage
    {
        #region private properties
        /// <summary>
        /// Keep Module PK
        /// </summary>
        private int ModulePK
        {
            get
            {
                return this.ViewState[ViewstateStrings.ModulePK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.ModulePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ModulePK] = value;
            }
        }
        /// <summary>
        /// Keep Module Name
        /// </summary>
        private string ModuleName
        {
            get
            {
                return this.ViewState["ModuleName"] == null ? string.Empty : this.ViewState["ModuleName"].ToString();
            }
            set
            {
                this.ViewState["ModuleName"] = value;
            }
        }

        private int sbuID
        {
            get
            {
                return this.ViewState["sbuID"] == null ? 0 :Convert.ToInt32(this.ViewState["sbuID"].ToString());
            }
            set
            {
                this.ViewState["sbuID"] = value;
            }
        }
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
        private int TotalEmployees
        {
            get
            {
                return this.ViewState[ViewstateStrings.TotalEmployees] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.TotalEmployees]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalEmployees] = value;
            }
        }
        private int EmployeesLimit
        {
            get
            {
                return this.ViewState[ViewstateStrings.EmployeeLimits] == null ? TotalEmployees + 1 : Convert.ToInt32(this.ViewState[ViewstateStrings.EmployeeLimits]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EmployeeLimits] = value;
            }
        }
        #region private variables
        BusinessObject.User currentUser;
        //User Module details
        private DataTable dtUserModuleDtl;
        private DataTable dtTranscationModuleDtl;
        //User module Count
        private DataSet dsUserModuleCount;
        //Module Users details
        private DataSet dsModuleUsersDtl;
        BusinessObject.User objUser;
        // List variables for binding details to controls
        private ActionsEnum commonActions;
        //New Code
        private string licenseValues;
        #endregion

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
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    GetFieldValues(ControlsEnum.LICENCEVIEW);
                    SetFieldValues(ControlsEnum.LICENCEVIEW);

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
        protected void ActionHandler(object sender, EventArgs e)
        {
            int result;
            result = 0;
            string file = "";
            GridViewRow gvr;
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        Response.Redirect(Resources.PageURL.UserModuleUrl, true);
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        ModulePK = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                        hdfModulePK.Value = ModulePK.ToString();
                        gvr = ((LinkButton)sender).Parent.Parent as GridViewRow;
                        if (gvr != null)
                        {
                            Label lblModule = gvr.FindControl("lblModuleName") as Label;
                            ModuleName = lblModule.Text;
                        }
                        GetFieldValues(ControlsEnum.VIEW);
                        SetFieldValues(ControlsEnum.VIEW);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divRoleDetails]','" + Resources.Captions.ModuleUsers + "-" + ModuleName + "','600','500');", true);
                        
                        //register Autocomplete User and Role
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitComponents", "$(document).ready(function(){InitComponents();});", true);

                        break;
                    #endregion
                    #region Search
                    case ActionsEnum.SEARCH:
                        SetFieldValues(ControlsEnum.SEARCH);

                        //register Autocomplete User and Role
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitComponents", "$(document).ready(function(){InitComponents();});", true);

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divRoleDetails]','" + Resources.Captions.ModuleUsers + "-" + ModuleName + "','600','500');", true);
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        SetFieldValues(ControlsEnum.CLEAR);

                        //register Autocomplete User and Role
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitComponents", "$(document).ready(function(){InitComponents();});", true);

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowContainerDiv('[id$=divRoleDetails]','" + Resources.Captions.ModuleUsers + "-" + ModuleName + "','600','500');", true);
                        break;
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

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum controlType)
        {
            
            objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.DEFAULT:
                        dtUserModuleDtl = UserManagementBL.GetUserModuleList(0, Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE), GetUserLimitText(), sbuID);
                        dsUserModuleCount = UserManagementBL.GetUserCount(sbuID);
                        break;
                    case ControlsEnum.VIEW:
                        dsModuleUsersDtl = UserManagementBL.GetModuleUsers(ModulePK, sbuID);
                        break;
                    case ControlsEnum.LICENCEVIEW:
                        GetLicenceValues();
                        string EncrytDtls = licenseValues;
                        dtTranscationModuleDtl = UserManagementBL.GetTranscation(sbuID, EncrytDtls);
                        break;
                    // if passed nothing or string.empty(), then Means Default Bing Will Bind all the Data in initial stage       
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
                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
                        if(dsUserModuleCount!=null && dsUserModuleCount.Tables.Count>1){
                            try
                            {
                                if (dsUserModuleCount.Tables[0].Rows.Count > 0)
                                {
                                    lblApplicationUserCount.Text = dsUserModuleCount.Tables[0].Rows[0]["COUNT"].ToString();
                                    lblSystemnUserCount.Text = dsUserModuleCount.Tables[0].Rows[1]["COUNT"].ToString();
                                    lblServiceUserCount.Text = dsUserModuleCount.Tables[0].Rows[2]["COUNT"].ToString();
                                }
                                if (dsUserModuleCount.Tables[1].Rows.Count > 0)
                                {
                                    lblPortalUserCount.Text = dsUserModuleCount.Tables[1].Rows[0]["CUS_PORTAL_COUNT"].ToString();
                                }
                            }
                            catch (Exception ex)
                            { 
                            }
                        }
                        break;
                    case ControlsEnum.VIEW:
                        ModuleUsers = dsModuleUsersDtl.Tables[0];
                        BindGrid(controlType);
                        BindDropDown(ControlsEnum.USER);
                        BindDropDown(ControlsEnum.MODULE);
                        break;
                    case ControlsEnum.SEARCH:
                        int usrPK = Convert.ToInt16(hdfUserPK.Value);
                        int modPK = Convert.ToInt16(hdfRolePK.Value);
                        //grdModuleUser.DataSource = SearchModuleUsers(Convert.ToInt32(ddlUsers.SelectedValue), Convert.ToInt32(ddlRole.SelectedValue));
                        grdModuleUser.DataSource = SearchModuleUsers(Convert.ToInt16(hdfUserPK.Value),Convert.ToInt16(hdfRolePK.Value));
                        grdModuleUser.DataBind();
                        break;
                    case ControlsEnum.CLEAR:
                        ddlUsers.SelectedValue = CommonConstants.SELECTVAL;
                        ddlRole.SelectedValue = CommonConstants.SELECTVAL;
                        txtUser.Text = txtRole.Text = string.Empty;
                        hdfUserPK.Value = hdfRolePK.Value = CommonConstants.SELECT_VALUE_ZERO;
                        //grdModuleUser.DataSource = SearchModuleUsers(Convert.ToInt32(ddlUsers.SelectedValue), Convert.ToInt32(ddlRole.SelectedValue));
                        grdModuleUser.DataSource = SearchModuleUsers(Convert.ToInt16(hdfUserPK.Value), Convert.ToInt16(hdfRolePK.Value));
                        grdModuleUser.DataBind();
                        break;
                    case ControlsEnum.LICENCEVIEW:
                        BindGrid(controlType);
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
        private void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.USER:
                        ddlUsers.Items.Clear();
                        ddlUsers.DataSource = GetModuleUsers();
                        ddlUsers.DataTextField = "SYM_USER_TEXT";
                        ddlUsers.DataValueField = "SYM_USER";
                        ddlUsers.DataBind();
                        ddlUsers.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.MODULE:
                        ddlRole.Items.Clear();
                        ddlRole.DataSource = GetModules();
                        ddlRole.DataTextField = "SYM_USER_GROUP_TEXT";
                        ddlRole.DataValueField = "SYM_USER_GROUP";
                        ddlRole.DataBind();
                        ddlRole.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                                  SYM_USER = row.Field<int>("SYM_USER"),
                                  SYM_USER_TEXT = row.Field<string>("SYM_USER_TEXT")
                              })
                              .Distinct().OrderBy(m => m.SYM_USER_TEXT);
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
                                  SYM_USER_GROUP = row.Field<int>("SYM_USER_GROUP"),
                                  SYM_USER_GROUP_TEXT = row.Field<string>("SYM_USER_GROUP_TEXT")
                              })
                              .Distinct().OrderBy(u=> u.SYM_USER_GROUP_TEXT);
                return distinctValues;
            }
            return null;
        }
        /// <summary>
        /// Search Module Users
        /// </summary>
        /// <param name="userPK"></param>
        /// <param name="modulePK"></param>
        /// <returns></returns>
        private object SearchModuleUsers(int userPK, int modulePK)
        {
            if (ModuleUsers != null)
            {
                var results = from row in ModuleUsers.AsEnumerable()
                              where row.Field<int>("SYM_USER") == (userPK > 0 ? userPK : row.Field<int>("SYM_USER")) 
                              &&
                              row.Field<int>("SYM_USER_GROUP") == (modulePK > 0 ? modulePK : row.Field<int>("SYM_USER_GROUP"))
                              select new
                              {
                                  SYM_USER = row.Field<int>("SYM_USER"),
                                  SYM_USER_TEXT = row.Field<string>("SYM_USER_TEXT"),
                                  SYM_USER_GROUP = row.Field<int>("SYM_USER_GROUP"),
                                  SYM_USER_GROUP_TEXT = row.Field<string>("SYM_USER_GROUP_TEXT")
                              };
                return results;
            }
            return null;
        
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
                    grdUserModule.DataSource = dtUserModuleDtl;
                    grdUserModule.DataBind();
                    break;
                case ControlsEnum.VIEW:
                    grdModuleUser.DataSource = dsModuleUsersDtl;
                    grdModuleUser.DataBind();
                    break;
                case ControlsEnum.LICENCEVIEW:
                    grdLicenceview.DataSource = dtTranscationModuleDtl;
                    grdLicenceview.DataBind();
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
            DataTable dtConfig = new DataTable();
            objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            dtConfig = CommonBL.GetApplicaitonConfiguaration("BIZUNIT WISE LICENCE", string.Empty);
          
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                if (dtConfig.Rows[0]["ACF_DATA"].ToString() == "1")
                {
                    sbuID = objUser.SBUID;
                }

            }
            dtConfig = CommonBL.GetApplicaitonConfiguaration("USER LIMIT SETTINGS", string.Empty, sbuID);
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                GTIService.Utilities.CryptoServices crypto = new GTIService.Utilities.CryptoServices();
                retVal = crypto.DecryptString(dtConfig.Rows[0]["ACF_DATA"].ToString(),
                                        ConfigurationManager.AppSettings["SYSKEY"]);

            }
            return retVal;
        }
        #endregion
        private void GetLicenceValues()
        {
            string retVal = string.Empty;
            DataTable dtConfig = CommonBL.GetApplicaitonConfiguaration("USER MODULE SETTING", string.Empty, sbuID);
            if (dtConfig!=null&&dtConfig.Rows.Count>0)
            {
                GTIService.Utilities.CryptoServices crypto = new GTIService.Utilities.CryptoServices();
                 retVal = crypto.DecryptString(dtConfig.Rows[0]["ACF_DATA"].ToString(),
                                        ConfigurationManager.AppSettings["SYSKEY"]);
                try
                {
                    if (retVal.Length > 0)
                        EmployeesLimit = Convert.ToInt32((retVal.Split('-')[(int)BusinessObject.Common.UserModuleLimitIndex.Employee]));
                    licenseValues = retVal;
                }
                catch
                {
                   
                }
                
            }
        }
    }
    public enum ControlsEnum
    {
        DEFAULT,
        VIEW,
        USER,
        MODULE,
        SEARCH,
        CLEAR,
        LICENCEVIEW
    }
}