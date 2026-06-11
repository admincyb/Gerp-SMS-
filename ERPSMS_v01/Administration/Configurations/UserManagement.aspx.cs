using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GTIService.Constants.Common;
using BusinessObject.AccountManagement;
using ERPSMS_v01.UserControls;
using GTIService;
using BusinessObject.Utilities;
using System.Web.Script.Serialization;
using BusinessObject.Common;
using BusinessLogic.Administration.Configurations;
using BusinessObject.Administration.Configurations;
using System.Xml;
using BusinessObject.CommonManagement;
using BusinessLogic.CommonManagement;
using System.Configuration;


namespace ERPSMS_v01.Administration.Configurations
{
    public partial class UserManagement : ERP.Store.UI.MyBasePage
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
        /// Save Return Value
        /// </summary>
        private int RetVal
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.RetVal]);
            }
            set
            {
                this.ViewState[ViewstateStrings.RetVal] = value;
            }
        }

        /// <summary>
        /// View State of CopyFromUserPK 
        /// </summary>
        private int CopyFromUserPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["CopyFromUserPK"]);
            }
            set
            {
                this.ViewState["CopyFromUserPK"] = value;
            }
        }
        /// <summary>
        /// ReplaceMergeFlag ViewState
        /// </summary>
        private int ReplaceMergeFlag
        {
            get
            {
                return Convert.ToInt16(this.ViewState["ReplaceMergeFlag"]);
            }
            set
            {
                this.ViewState["ReplaceMergeFlag"] = value;
            }
        }


        #endregion

        private DataSet dsDepartment;
        private DataSet dsUserDepartment;
        private DataTable dtUserDtl;
        BusinessObject.User objUser;
        private UserRoleMappingBO objUserRoleMapping;

        // List variables for binding details to controls
        private ActionsEnum commonActions;
        //New Code
        private int departmentPK;
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
                    case ControlsEnum.TREE:

                        break;
                    case ControlsEnum.DEPARTMENT:
                        objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        dsDepartment = UserManagementBL.GetDepartment(objUser.SBUID);
                        break;
                    case ControlsEnum.USERGROUPDEPARTMENT:
                        objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        dsUserDepartment = UserManagementBL.GetUserGroupDepartment(departmentPK, CurrPK);
                        break;
                    case ControlsEnum.USERGROUP:

                        break;
                    case ControlsEnum.DEFAULT:
                        dtUserDtl = UserManagementBL.GetUserDetails(CurrPK);
                        break;
                    // if passed nothing or string.empty(), then Means Default Bing Will Bind all the Data in initial stage       
                    default:
                        break;
                }
                //UserManagementService.Close();
            }
            catch (Exception ex)
            {
                // UserManagementService.Abort();
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
                    case ControlsEnum.DEPARTMENT:
                        BindTreeView();
                        break;
                    case ControlsEnum.TREE:
                        BindTreeView();
                        break;
                    case ControlsEnum.USERGROUP:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.DEFAULT:
                        //dtUserDtl
                        if (dtUserDtl.Rows.Count > 0)
                        {
                            lblUserNameTxt.Text = dtUserDtl.Rows[0]["usrName"].ToString();
                            lblEmployeeTxt.Text = dtUserDtl.Rows[0]["usrEmployeeText"].ToString().Length > 35 ? dtUserDtl.Rows[0]["usrEmployeeText"].ToString().Substring(0, 35) + "..." : dtUserDtl.Rows[0]["usrEmployeeText"].ToString();
                            lblEmployeeTxt.ToolTip = dtUserDtl.Rows[0]["usrEmployeeText"].ToString();
                            hdfUserType.Value = dtUserDtl.Rows[0]["usrIsSysUser"].ToString();
                        }
                        break;
                    default:
                        BindTreeView();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region PageLevel Events
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        #endregion
        #region Action Handlers
        /// <summary>
        /// For Button Click  (Save/Cancel/New/Edit/View/Delete, Print) and Dropdown SelectedIndexChange Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            int result;
            result = 0;
            DataSet dsResult = null;

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
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (IsValid)
                        {
                            string routeURL = Resources.PageURL.UserManagementUrl.ToString();
                            UserRoleMappingBO objUserRoleMapping = new UserRoleMappingBO();
                            objUserRoleMapping = SetUIValuesToObject();
                            XmlDocument xmlDoc = CommonFunctions.ObjectTOXml(objUserRoleMapping);
                            dsResult = UserManagementBL.SaveUserRole(xmlDoc.InnerXml, out result);
                            if (result > 0) // Success !  redirect to listing page
                            {
                                // Show Save Message and redired to listing page
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + String.Format(Resources.ErrorMessages.Msg_SavSuccess, Resources.Captions.UserGroup) + "','" + Resources.Captions.Information + "','" + routeURL + "');", true);
                            }
                            else
                            {
                                // if any error occur, show error details
                                DbSaveStatus saveStatus = (DbSaveStatus)result;
                                switch (saveStatus)
                                {
                                    case DbSaveStatus.SQLERROR://SQl Error
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                        break;
                                    case DbSaveStatus.CONCURRENCY://Cuncurrency Check
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Concurrent;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.Captions.UserGroup.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "'," + routeURL + "');", true);
                                        break;
                                    case DbSaveStatus.ALREADYDELETED://Cuncurrency Check
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.Captions.UserGroup.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "'," + routeURL + "');", true);
                                        break;
                                    case DbSaveStatus.EXEEDLIMIT:
                                        if (dsResult.Tables.Count > 0)
                                        {
                                            litErrorMsg.Text = FormatError(dsResult.Tables[0]);
                                        }
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        break;
                                    default:
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError; //Other Errors
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                        break;
                                }
                            }
                        }
                        break;

                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        Response.Redirect(Resources.PageURL.UserModuleUrl, true);
                        break;
                    #endregion
                    ///User Management 
                    #region User
                    case ActionsEnum.USER:
                        Response.Redirect(Resources.PageURL.User, true);
                        break;
                    #endregion

                    #region POPUP ACTIONS
                    #region SHOWPOPUP (For Showing UserRole Copying Popup
                    case ActionsEnum.SHOWPOPUP:
                        rbtnMerge.Checked = rbtnReplace.Checked = false;
                        txtCopyUserName.Text = string.Empty;
                        hdfCopyUserPk.Value = "0";
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpCopyUserRoles]','" + GetLocalResourceObject("SelectUserForCopying").ToString() + "','400','200');", true);
                        break;
                    #endregion
                    #region APPLY (POPUP)
                    case ActionsEnum.APPLY:
                        CopyFromUserPK = String.IsNullOrEmpty(hdfCopyUserPk.Value) ? 0 : Convert.ToInt32(hdfCopyUserPk.Value);
                        if (rbtnReplace.Checked)
                        {
                            ReplaceMergeFlag = 1; //Replace
                        }
                        else if (rbtnMerge.Checked)
                        {
                            ReplaceMergeFlag = 2;//Merge
                        }
                        else
                        {
                            ReplaceMergeFlag = 0;//Normal
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        ActionHandler(btnSave, EventArgs.Empty);
                        break;
                    #endregion
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
        #endregion
        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {

        }
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){AutoInit();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            // base.CheckBtnVisibility(sender);
        }

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
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

        #endregion
        #region Helper Methods
        private string FormatError(DataTable dtList)
        {
            //string errorMsg ="<ul><li>"+ Resources.ErrorMessages.Msg_User_Module_Limt_Exeed+"</li>";
            string errorMsg = Resources.ErrorMessages.Msg_User_Module_Limt_Exeed + "<ul>";
            for (int i = 0; i < dtList.Rows.Count; i++)
            {
                errorMsg += "<li>" + Enum.GetName(typeof(UserModuleIndex), (object)dtList.Rows[i][0]) + "</li>";
            }
            errorMsg += "</ul>";
            return errorMsg;
        }
        /// <summary>
        /// Get User Limit Text
        /// </summary>
        /// <returns></returns>
        private string GetUserLimitText()
        {
            string retVal = string.Empty;
            int sbuID = 0;
            DataTable dtConfig = new DataTable();
            objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            dtConfig = CommonBL.GetApplicaitonConfiguaration("BIZUNIT WISE LICENCE", string.Empty);
            
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                if (dtConfig.Rows[0]["ACF_DATA"].ToString() == "1")
                    sbuID = objUser.SBUID;
            }
            dtConfig = CommonBL.GetApplicaitonConfiguaration("USER LIMIT SETTINGS", string.Empty, sbuID);
           
            if (dtConfig != null && dtConfig.Rows.Count > 0 && !IsSuperAdminUser(objUser.PKUser))
            {
                GTIService.Utilities.CryptoServices crypto = new GTIService.Utilities.CryptoServices();
                retVal = crypto.DecryptString(dtConfig.Rows[0]["ACF_DATA"].ToString(),
                                        ConfigurationManager.AppSettings["SYSKEY"]);

            }
            return retVal;
        }
        /// <summary>
        /// Is the User Is Super Admin
        /// </summary>
        /// <param name="pkUser"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Value from Control to object
        /// </summary>
        /// <returns></returns>
        private UserRoleMappingBO SetUIValuesToObject()
        {
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            objUserRoleMapping = new UserRoleMappingBO();
            objUserRoleMapping.usrPK = CurrPK;
            objUserRoleMapping.userCountText = GetUserLimitText();
            objUserRoleMapping.bizUnitPK = currentUser.SBUID;
            objUserRoleMapping.MODIFIED_BY = currentUser.PKUser;

            objUserRoleMapping.COPY_TYPE = ReplaceMergeFlag;
            objUserRoleMapping.USER_PK_SRC = CopyFromUserPK;

            List<UserGroupDtailList> UserGroupMpgList = new List<UserGroupDtailList>();
            foreach (TreeNode root in trvGroup.Nodes)
            {
                foreach (TreeNode node in root.ChildNodes)
                    foreach (TreeNode child in node.ChildNodes)
                    {
                        if (child.Checked)
                        {
                            UserGroupDtailList UserGroupMpgObj = new UserGroupDtailList();
                            UserGroupMpgObj.gumGroup = Convert.ToInt32(child.Value);
                            UserGroupMpgList.Add(UserGroupMpgObj);
                        }
                    }
            }
            objUserRoleMapping.GroupDtailList = UserGroupMpgList;

            return objUserRoleMapping;

        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        //private List<WkfUserGroupMpg> SetUIValuesToObject()
        //{
        //    try
        //    {
        //        wkfUserGroupMpgList = new List<WkfUserGroupMpg>();
        //        foreach (TreeNode root in trvGroup.Nodes)
        //        {
        //            foreach (TreeNode node in root.ChildNodes)
        //                foreach (TreeNode child in node.ChildNodes)
        //                {
        //                    if (child.Checked)
        //                    {
        //                        wkfUserGroupMpgObj = new WkfUserGroupMpg();
        //                        wkfUserGroupMpgObj.gumGroup = Convert.ToInt16(child.Value);
        //                        wkfUserGroupMpgObj.gumUser = CurrPK;
        //                        wkfUserGroupMpgList.Add(wkfUserGroupMpgObj);
        //                    }
        //                }
        //        }

        //        //foreach (TreeNode node in trvGroup.CheckedNodes)
        //        //{
        //        //    wkfUserGroupMpgObj = new WkfUserGroupMpg();
        //        //    wkfUserGroupMpgObj.gumGroup = Convert.ToInt16(node.Value);
        //        //    wkfUserGroupMpgObj.gumUser = CurrPK;
        //        //    wkfUserGroupMpgList.Add(wkfUserGroupMpgObj);
        //        //}
        //        if (wkfUserGroupMpgList.Count == 0)
        //        {
        //            // if to save null group
        //            //pdrgrsuObj = new WkfUserGroupMpg();
        //            //pdrgrsuObj.gumGroup = -1;
        //            //pdrgrsuObj.gumUser = CurrPK;
        //            //pdrgrsuList.Add(pdrgrsuObj);

        //            //don't allow null group
        //            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Group").ToString();
        //            throw new Exception(litErrorMsg.Text);
        //        }
        //        return wkfUserGroupMpgList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        wkfUserGroupMpgList = null;
        //        wkfUserGroupMpgObj = null;
        //    }


        //}
        /// <summary>
        /// Method to bind the Group Tree
        /// </summary>
        private void BindTreeView()
        {
            try
            {
                TreeNode node;
                TreeNode child;
                TreeNode root;
                bool IsChildMapped = false;
                trvGroup.Nodes.Clear();
                //root = new TreeNode(GetLocalResourceObject("Department").ToString(), "0");
                root = new TreeNode("Department", "0");
                trvGroup.Nodes.Add(root);
                root.Expand();

                if (dsDepartment != null)
                {
                    foreach (DataRow dr in dsDepartment.Tables[0].Rows)
                    {
                        IsChildMapped = false;
                        node = new TreeNode();
                        node.ShowCheckBox = false;
                        // node.Checked = false;
                        node.Text = dr[1].ToString();
                        node.Value = dr[0].ToString();
                        departmentPK = Convert.ToInt32(dr[0].ToString());
                        GetFieldValues(ControlsEnum.USERGROUPDEPARTMENT);
                        if (dsUserDepartment != null)
                        {
                            foreach (DataRow drDept in dsUserDepartment.Tables[0].Rows)
                            {
                                child = new TreeNode();
                                child.ShowCheckBox = true;
                                child.Checked = drDept[2].ToString() == "1" ? true : false;
                                IsChildMapped = IsChildMapped == false ? drDept[2].ToString() == "1" ? true : false : true;
                                child.Text = drDept[1].ToString();
                                child.Value = drDept[0].ToString();
                                node.ChildNodes.Add(child);

                            }
                        }
                        if (IsChildMapped)
                            node.ExpandAll();
                        root.ChildNodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            try
            {
                //foreach (WkfUserGroupMpg usergroup in wkfUserGroupMpgList)
                //{
                //    foreach (TreeNode node in trvGroup.Nodes[0].ChildNodes)
                //    {
                //        if (node.Value.Equals(usergroup.gumGroup.ToString()))
                //        {
                //            node.Checked = true;
                //            break;
                //        }
                //    }
                //}
                trvGroup.Nodes[0].Expand();
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        /// <summary>
        /// Resets the form for a fresh entry
        /// </summary>
        private void ResetForm()
        {
            //Codes for Clearing the controls in the page
            CurrPK = 0;
            // wkfUserGroupList = null;
        }
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Method to handle Page Load Action
        /// </summary>
        public void PageActionHandler()
        {

            try
            {

                if (!IsPostBack)
                {
                    if (Request.QueryString["UserID"] != null)
                    {

                        CurrPK = Convert.ToInt32(Request.QueryString["UserID"].ToString());
                        GetFieldValues(ControlsEnum.DEPARTMENT);
                        SetFieldValues(ControlsEnum.DEPARTMENT);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
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
        #endregion
        #region ControlEnum
        public enum ControlsEnum
        {
            USERGROUP,
            DEFAULT,
            TREE,
            DEPARTMENT,
            USERGROUPDEPARTMENT
        }
        #endregion

    }
}
