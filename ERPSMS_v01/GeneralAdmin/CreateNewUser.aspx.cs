using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Administration.Configurations;
using BusinessObject.AccountManagement;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using GTIService;
using GTIService.Constants.Common;
using System.IO;
using System.Configuration;
using BusinessObject.Common;
using BusinessLogic.CommonManagement;
using System.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class CreateNewUser : ERP.Store.UI.MyBasePage
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
                return this.ViewState[ViewstateStrings.CurrPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }

        /// <summary>
        /// Current Status
        /// </summary>
        private int CurrStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrStatus] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrStatus]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrStatus] = value;
            }
        }

        /// <summary>
        /// Keep User Limit Setting
        /// </summary>
        private int UserLimitSetting
        {
            get
            {
                return this.ViewState[ViewstateStrings.UserLimitSetting] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.UserLimitSetting]);
            }
            set
            {
                this.ViewState[ViewstateStrings.UserLimitSetting] = value;
            }
        }
        /// <summary>
        /// Total Records
        /// </summary>
        private int TotalPages
        {
            get
            {
                return this.ViewState[ViewstateStrings.TotalPages] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.TotalPages]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }
        /// <summary>
        /// User Type 1->Supper Admin, 0->OtherUser
        /// </summary>
        private int UserType
        {
            get
            {
                return this.ViewState[ViewstateStrings.UserType] == null ? (int)UserRight.OtherUser : Convert.ToInt32(this.ViewState[ViewstateStrings.UserType]);
            }
            set
            {
                this.ViewState[ViewstateStrings.UserType] = value;
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
        /// Deleted File Path
        /// </summary>
        private string FilePath
        {
            get
            {
                return this.ViewState[ViewstateStrings.FilePath] == null ? string.Empty : this.ViewState[ViewstateStrings.FilePath].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.FilePath] = value;
            }
        }
        /// <summary>
        /// CurrentT ab
        /// </summary>
        private int CurrentTab
        {
            get
            {
                return this.ViewState["CurrentTab"] == null ? 1 : Convert.ToInt32(this.ViewState["CurrentTab"].ToString());
            }
            set
            {
                this.ViewState["CurrentTab"] = value;
            }
        }
        #endregion

        BusinessObject.User currentUser;
        //user details
        private DataTable dtUserDtl;
        //Employes list
        private DataTable dtEmployees;
        //User Department
        private DataTable dtDepartment;
        //Get Or Save User detais
        private UserManagementBO objUserManagement;
        //User Type
        private DataTable dtUserType;
        private DataTable dtLocatios;
        private DataTable dtUserModule;
        BusinessObject.User objUser;
        // List variables for binding details to controls
        private ActionsEnum commonActions;
        //New Code
        private int departmentPK;
        private string TestString = string.Empty;
        private string newUserPassword = string.Empty;
        private List<ERP.Utilities.ClientCulture> lstCulture;
        private UserMISReportBO ObjMISRptData;
        private List<UserMISReportGroups> lstReportGroups;
        private List<UserGroupReports> lstReports;
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
                Int16 module;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                switch (controlType)
                {
                    case ControlsEnum.CULTURE:
                        lstCulture = ERP.Utilities.CommonFunctions.GetClientCulture(GetGlobalResourceObject("ConfigurationsRes", "Culture").ToString());
                        break;
                    case ControlsEnum.USERMODULE:
                        dtUserModule = UserManagementBL.GetUserModuleDetails(CurrPK);
                        break;
                    case ControlsEnum.LOCATIONSMAPPED:
                        dtLocatios = UserManagementBL.GetUserLocations(CurrPK);
                        break;
                    case ControlsEnum.USERTYPE:
                        dtUserType = CommonBL.GetAppConfig(currentUser.SBUID, "USER LEVEL TYPE");
                        break;
                    case ControlsEnum.EMPLOYEE:
                        dtEmployees = EmployeesBL.GetEmployees(0, null, 0, 1, CurrPK > 0 ? Convert.ToInt32(DbActiveStatus.HASPK) : Convert.ToInt32(DbActiveStatus.ACTIVE));
                        break;
                    case ControlsEnum.USER:
                        dtUserDtl = UserManagementBL.GetUserDetails(CurrPK);
                        break;
                    case ControlsEnum.DEPARTMENT:
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        Int16.TryParse(ConfigurationManager.AppSettings[ERP.Utilities.ConfigStrings.GERPModule], out module);
                        dtDepartment = BusinessLogic.CommonManagement.CommonBL.GetDepartment(CurrPK, currentUser.SBUID, module);
                        break;
                    case ControlsEnum.MANAGEMISRPT:
                        ObjMISRptData = new UserMISReportBO();
                        string xmlDoc = UserManagementBL.GetMISReportList(CurrPK, 0, currentUser.CurrentSBUPK);
                        if (!string.IsNullOrEmpty(xmlDoc))
                            ObjMISRptData = (UserMISReportBO)CommonFunctions.DeserializeObject(xmlDoc, new UserMISReportBO());
                        break;
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
                    case ControlsEnum.CULTURE:
                        BindDropDown(ControlsEnum.CULTURE);
                        break;
                    case ControlsEnum.SECONDARYCULTURE:
                        BindDropDown(ControlsEnum.SECONDARYCULTURE);
                        break;
                    case ControlsEnum.USERMODULE:
                        BindGrid();
                        break;
                    case ControlsEnum.LOCATIONSMAPPED:
                        BindLocationTree();
                        break;
                    case ControlsEnum.EMPLOYEE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.USER:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.DEPARTMENT:
                        BindDropDown(ControlsEnum.DEPARTMENT);
                        break;
                    case ControlsEnum.USERTYPE:
                        BindDropDown(ControlsEnum.USERTYPE);
                        objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        // if (objUser.PKUser == (int)UserRight.SuperAdmin)
                        if (IsSuperAdminUser(objUser.PKUser))
                        {
                            divUserType.Visible = true;
                            UserType = (int)UserRight.SuperAdmin;
                        }
                        break;
                    case ControlsEnum.MANAGEMISRPT:
                        BindMISTree();
                        break;
                    default:
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
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
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
            string file = "";

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
                    case ActionsEnum.USERMAPPING:
                        Response.Redirect(Resources.PageURL.UserMappingUrl + CurrPK.ToString(), true);
                        break;
                    case ActionsEnum.USERMODULE:
                        if (CurrPK > 0)
                        {
                            CurrentTab = Convert.ToInt32(UserTabs.USERMODULE);
                            mltUserTabs.SetActiveView(viewUserModule);
                            GetFieldValues(ControlsEnum.USERMODULE);
                            SetFieldValues(ControlsEnum.USERMODULE);
                            SetActiveTab(commonActions);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_User_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    case ActionsEnum.USERINFO:
                        CurrentTab = Convert.ToInt32(UserTabs.USERINFO);
                        mltUserTabs.SetActiveView(viewUserInfo);
                        SetActiveTab(commonActions);
                        break;
                    case ActionsEnum.LOCATIONMAPPING:
                        if (CurrPK > 0)
                        {
                            CurrentTab = Convert.ToInt32(UserTabs.LOCATIONMAPPING);
                            mltUserTabs.SetActiveView(viewLocationMapping);
                            SetActiveTab(commonActions);
                            GetFieldValues(ControlsEnum.LOCATIONSMAPPED);
                            SetFieldValues(ControlsEnum.LOCATIONSMAPPED);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_User_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    case ActionsEnum.MANAGEMISREPORTS:
                        if (CurrPK > 0)
                        {
                            CurrentTab = Convert.ToInt32(UserTabs.MANAGEMISREPORT);
                            mltUserTabs.SetActiveView(viewManageMISrpts);
                            SetActiveTab(commonActions);
                            GetFieldValues(ControlsEnum.MANAGEMISRPT);
                            SetFieldValues(ControlsEnum.MANAGEMISRPT);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_User_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        #region User Information
                        if (CurrentTab == Convert.ToInt32(UserTabs.USERINFO))
                        {
                            if (IsValid)
                            {
                                if (UserType != (int)UserRight.SuperAdmin || GetGlobalResourceObject("ConfigurationsRes", "SuperAdmLimit").ToString() == "1")
                                {
                                    if ((CurrPK > 0
                                        && CurrStatus != Convert.ToInt32(ddlStatus.SelectedValue)
                                        && Convert.ToInt32(RecordStatus.ACTIVE) == Convert.ToInt32(ddlStatus.SelectedValue))
                                       || CurrPK == 0)
                                        if (IsLimitExeed())
                                        {
                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Limi_Exeed;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + litErrorMsg.Text + "');", true);
                                            return;
                                        }
                                }
                                if (IsPasswordStrength())
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Err_Password_Format;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + litErrorMsg.Text + "');", true);
                                    return;
                                }
                                else
                                {
                                    objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                                    string routeURL;
                                    int EditUserNewMode = 0;
                                    if (Page.Request.QueryString["UserProphile"] != null && Page.Request.QueryString["UserProphile"] == "1")
                                    {
                                        EditUserNewMode = 1;
                                        routeURL = GetGlobalResourceObject("PageURL", "EditUser").ToString() + objUser.PKUser.ToString() + "&UserProphile=1";
                                    }
                                    else
                                    {
                                        routeURL = Resources.PageURL.User.ToString();
                                    }
                                    objUserManagement = (UserManagementBO)SetUIValuesToObject(UserTabs.USERINFO);
                                    result = UserManagementBL.SaveUser(objUserManagement, objUser);
                                    if (result > 0) // Success !  redirect to listing page
                                    {
                                        if (!string.IsNullOrEmpty(FilePath))
                                        {
                                            lblSignatureName.Text = FilePath;
                                            //string file = Server.MapPath(GetLocalResourceObject("AttachPath").ToString()) + FilePath;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                            {
                                                file = Server.MapPath(GetLocalResourceObject("AttachPath").ToString()) + FilePath;
                                            }
                                            else
                                            {
                                                //SavePath = Server.MapPath("../Upload");
                                                file = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + FilePath;
                                            }

                                            if (File.Exists(file))
                                                File.Delete(file);
                                        }
                                        // Save Signature to folder
                                        if (!SaveSignature())
                                            return;
                                        if (GetGlobalResourceObject("ConfigurationsRes", "ShowLocationMappingTab").ToString() == "1")
                                        {
                                            SetNewUserInfo(result);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + String.Format(Resources.ErrorMessages.Msg_SavSuccess, Resources.Captions.NewUser) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else
                                        {
                                            // Show Save Message and redired to listing page
                                            if (EditUserNewMode == 0)
                                            {
                                                ResetForm();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + String.Format(Resources.ErrorMessages.Msg_SavSuccess, Resources.Captions.NewUser) + "','" + Resources.Captions.Information + "','" + routeURL + "');", true);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + String.Format(Resources.ErrorMessages.Msg_SavSuccess, Resources.Captions.NewUser) + "','" + Resources.Captions.Information + "');", true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // if any error occur, show error details
                                        DbSaveStatus saveStatus = (DbSaveStatus)result;
                                        switch (saveStatus)
                                        {
                                            case DbSaveStatus.SQLERROR://SQl Error
                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + litErrorMsg.Text + "');", true);
                                                break;
                                            case DbSaveStatus.CONCURRENCY://Cuncurrency Check
                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Concurrent;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.Captions.NewUser.ToString());
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "'," + routeURL + "');", true);
                                                break;
                                            case DbSaveStatus.ALREADYDELETED://Cuncurrency Check
                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.Captions.NewUser.ToString());
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "'," + routeURL + "');", true);
                                                break;
                                            case DbSaveStatus.CODEEXIST://UserID already exist
                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_UidExist;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + litErrorMsg.Text + "');", true);
                                                break;

                                            default:
                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError; //Other Errors
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + litErrorMsg.Text + "');", true);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        #region LOCATION MAPPING
                        else if (CurrentTab == Convert.ToInt32(UserTabs.LOCATIONMAPPING))
                        {
                            if (IsValid)
                            {
                                string routeURL = Resources.PageURL.User.ToString();
                                UserLocationMappingBO objUserLocationMapping = new UserLocationMappingBO();
                                objUserLocationMapping = (UserLocationMappingBO)SetUIValuesToObject(UserTabs.LOCATIONMAPPING);
                                XmlDocument xmlDoc = CommonFunctions.ObjectTOXml(objUserLocationMapping);
                                result = UserManagementBL.SaveUserLocations(xmlDoc.InnerXml);
                                if (result > 0) // Success !  redirect to listing page
                                {
                                    // Show Save Message and redired to listing page
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + String.Format(Resources.ErrorMessages.Msg_SavSuccess, GetLocalResourceObject("ocationMapping")) + "','" + Resources.Captions.Information + "','" + routeURL + "');", true);
                                    // if (Page.Request.QueryString["UserProphile"] != null && Page.Request.QueryString["UserProphile"] == "1")

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
                                        default:
                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError; //Other Errors
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                            break;
                                    }
                                }
                            }
                        }

                        #endregion
                        else if (CurrentTab == Convert.ToInt32(UserTabs.MANAGEMISREPORT))
                        {
                            if (IsValid)
                            {
                                string routeURL = Resources.PageURL.User.ToString();
                                UserMISReportMappingBO objUserReportMapping = new UserMISReportMappingBO();
                                objUserReportMapping = (UserMISReportMappingBO)SetUIValuesToObject(UserTabs.MANAGEMISREPORT);
                                XmlDocument xmlDoc = CommonFunctions.ObjectTOXml(objUserReportMapping);
                                result = UserManagementBL.SaveUserReports(xmlDoc.InnerXml);
                                if (result > 0) // Success !  redirect to listing page
                                {
                                    // Show Save Message and redired to listing page
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + String.Format(Resources.ErrorMessages.Msg_SavSuccess, GetLocalResourceObject("ocationMapping")) + "','" + Resources.Captions.Information + "','" + routeURL + "');", true);
                                    // if (Page.Request.QueryString["UserProphile"] != null && Page.Request.QueryString["UserProphile"] == "1")

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
                                        default:
                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError; //Other Errors
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                            break;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        Response.Redirect(Resources.PageURL.User, true);
                        break;
                    #endregion
                    ///User Management 
                    #region User
                    case ActionsEnum.USER:
                        Response.Redirect(Resources.PageURL.User, true);
                        break;
                    #endregion

                    #region Delete File
                    case ActionsEnum.DELETEITEM:
                        FilePath = lblSignatureName.Text;
                        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                        {
                            file = Server.MapPath(GetLocalResourceObject("AttachPath").ToString()) + FilePath;
                        }
                        else
                        {
                            //SavePath = Server.MapPath("../Upload");
                            file = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + FilePath;
                        }

                        if (File.Exists(file))
                            File.Delete(file);
                        lblSignatureName.Text = string.Empty;
                        anchorFile.InnerHtml = string.Empty;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDeleteButton", "ShowHideDelete();", true);
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
        #endregion

        #region Pager Methods + Init
        private void HideTabByResource()
        {
            spnLocationMapping.Visible = GetGlobalResourceObject("ConfigurationsRes", "ShowLocationMappingTab").ToString() == "1" ? true : false;
            spnMISrpts.Visible = GetGlobalResourceObject("ConfigurationsRes", "ShowReprotsMappingTab").ToString() == "1" ? true : false;

        }
        /// <summary>
        /// Set Active Tab
        /// </summary>
        /// <param name="currentAction"></param>
        private void SetActiveTab(ActionsEnum currentAction)
        {
            spnUserModule.Attributes.Remove("class");
            spnUserModule.Attributes.Add("class", "tab-inactive");
            lnkUserModule.Attributes.Remove("class");
            lnkUserModule.Attributes.Add("class", "tab-inactive");
            spnUserInfo.Attributes.Remove("class");
            lnkUserInfo.Attributes.Remove("class");
            spnUserInfo.Attributes.Add("class", "tab-inactive");
            lnkUserInfo.Attributes.Add("class", "tab-inactive");
            lnkLocationMapping.Attributes.Remove("class");
            spnLocationMapping.Attributes.Remove("class");
            lnkLocationMapping.Attributes.Add("class", "tab-inactive");
            spnLocationMapping.Attributes.Add("class", "tab-inactive");
            lnkMISrpts.Attributes.Add("class", "tab-inactive");
            spnMISrpts.Attributes.Add("class", "tab-inactive");
            btnUserMapping.Visible = false;
            switch (currentAction)
            {
                case ActionsEnum.USERINFO:
                    spnUserInfo.Attributes.Remove("class");
                    lnkUserInfo.Attributes.Remove("class");
                    spnUserInfo.Attributes.Add("class", "tab-active");
                    lnkUserInfo.Attributes.Add("class", "tab-active");
                    btnSave.Visible = true;
                    break;
                case ActionsEnum.LOCATIONMAPPING:
                    lnkLocationMapping.Attributes.Remove("class");
                    spnLocationMapping.Attributes.Remove("class");
                    lnkLocationMapping.Attributes.Add("class", "tab-active");
                    spnLocationMapping.Attributes.Add("class", "tab-active");
                    btnSave.Visible = true;
                    break;
                case ActionsEnum.USERMODULE:
                    lnkUserModule.Attributes.Remove("class");
                    spnUserModule.Attributes.Remove("class");
                    lnkUserModule.Attributes.Add("class", "tab-active");
                    spnUserModule.Attributes.Add("class", "tab-active");
                    btnSave.Visible = false;
                    btnUserMapping.Visible = true;
                    break;
                case ActionsEnum.MANAGEMISREPORTS:
                    lnkMISrpts.Attributes.Remove("class");
                    spnMISrpts.Attributes.Remove("class");
                    lnkMISrpts.Attributes.Add("class", "tab-active");
                    spnMISrpts.Attributes.Add("class", "tab-active");
                    btnSave.Visible = true;
                    break;
            }
        }
        /// <summary>
        /// Method for Page PreInit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //if (Page.Request.QueryString["UserProphile"] != null && Page.Request.QueryString["UserProphile"] == "1")
            //    this.MasterPageFile = "~/IFrameMaster.Master";
        }


        /// <summary>
        /// Methord  For PageInit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
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
        private void SetConfigValue()
        {

            DataTable dtConfig = CommonBL.GetApplicaitonConfiguaration("USER LIMIT SETTINGS", string.Empty);
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                GTIService.Utilities.CryptoServices crypto = new GTIService.Utilities.CryptoServices();
                string retVal = crypto.DecryptString(dtConfig.Rows[0]["ACF_DATA"].ToString(),
                                        ConfigurationManager.AppSettings["SYSKEY"]);
                UserLimitSetting = Convert.ToInt32((retVal.Split('-')[0]));
            }

            #region Configuration SBUSpecificUser

            hdfSBUSpecificUser.Value = GetGlobalResourceObject("ConfigurationsRes", "SBUSpecificUser").ToString();

            #endregion
        }
        private int ActiveCount(int userType)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            int count = 0;
            DataTable dtActiveCount = UserManagementBL.GetUserCount(userType, Convert.ToInt32(RecordStatus.ACTIVE), currentUser.SBUID);
            if (dtActiveCount != null && dtActiveCount.Rows.Count > 0)
            {
                count = Convert.ToInt32(dtActiveCount.Rows[0]["UserCount"].ToString());
            }
            return count;
        }
        private bool IsLimitExeed()
        {
            bool retVal = false;
            if (ActiveCount(Convert.ToInt32(ddlUserType.SelectedValue)) >= UserLimitSetting)
            {
                retVal = true;
            }
            return retVal;
        }
        private bool IsPasswordStrength()
        {
            bool retVal = false;
            TestString = txtPassword.Text;
            if (Page.Request.QueryString["UserProphile"] != null && Page.Request.QueryString["UserProphile"] == "1")
            {
                GetFieldValues(ControlsEnum.USER);
                SetFieldValues(ControlsEnum.USER);
            }
            if (!(System.Text.RegularExpressions.Regex.IsMatch(TestString, "[a-zA-Z]") && System.Text.RegularExpressions.Regex.IsMatch(TestString, "[0-9]")))
            {
                retVal = true;
            }
            return retVal;
        }
        /// <summary>
        /// Bind Tree View
        /// </summary>
        private void BindLocationTree()
        {
            try
            {
                TreeNode node;
                TreeNode child;
                TreeNode root;
                bool IsChildMapped = false;
                trvUserLocations.Nodes.Clear();
                //root = new TreeNode(GetLocalResourceObject("Department").ToString(), "0");
                root = new TreeNode("Locations", "0");
                root.ShowCheckBox = true;
                trvUserLocations.Nodes.Add(root);
                root.Expand();

                if (dtLocatios != null)
                {
                    foreach (DataRow dr in dtLocatios.Rows)
                    {
                        IsChildMapped = false;
                        node = new TreeNode();
                        node.ShowCheckBox = true;
                        // node.Checked = false;
                        node.Text = dr["WUL_LOCATION_TEXT"].ToString();
                        node.Value = dr["WUL_PK"].ToString() + "," + dr["WUL_LOCATION"].ToString();
                        node.Checked = dr["IS_MAP"].ToString() == "1" ? true : false;
                        root.ChildNodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }

        private void BindMISTree()
        {
            try
            {
                TreeNode node;
                TreeNode child;
                TreeNode root;
                bool IsChildMapped = false;
                trvMISReports.Nodes.Clear();
                root = new TreeNode("Reports", "0");
                root.ShowCheckBox = true;
                trvMISReports.Nodes.Add(root);
                root.Expand();
                lstReportGroups = ObjMISRptData.ReportGroupList;
                if (lstReportGroups != null && lstReportGroups.Count > 0)
                {
                    foreach (UserMISReportGroups items in lstReportGroups)
                    {
                        int nodeUnchecked = 0;
                        IsChildMapped = false;
                        node = new TreeNode();
                        node.ShowCheckBox = true;
                        // node.Checked = false;
                        node.Text = items.RPT_GROUP_TEXT;
                        node.Value = items.RPT_GROUP.ToString();
                        lstReports = new List<UserGroupReports>();
                        lstReports = items.ReportList;
                        if (lstReports != null && lstReports.Count > 0)
                        {
                            foreach (UserGroupReports reports in lstReports)
                            {
                                child = new TreeNode();
                                child.ShowCheckBox = true;
                                child.Checked = reports.RPT_USER_FLAG == 1 ? true : false;
                                IsChildMapped = IsChildMapped == false ? reports.RPT_USER_FLAG == 1 ? true : false : true;
                                child.Text = reports.RPT_TEXT;
                                child.Value = reports.RPT_PK.ToString();
                                node.ChildNodes.Add(child);
                                if (reports.RPT_USER_FLAG == 0)
                                    nodeUnchecked += 1;
                            }
                            node.Checked = nodeUnchecked == 0 ? true : false;
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

        private void BindGrid()
        {
            grdUserModule.DataSource = dtUserModule;
            grdUserModule.DataBind();
        }
        /// <summary>
        /// Bind DropDown  as per type 
        /// </summary>
        /// <param name="drpName"></param>
        private void BindDropDown(ControlsEnum drpName)
        {
            switch (drpName)
            {
                case ControlsEnum.USERTYPE:
                    ddlUserType.Items.Clear();
                    if (dtUserType != null && dtUserType.Rows.Count > 0)
                    {
                        ddlUserType.DataSource = dtUserType;
                        ddlUserType.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
                        ddlUserType.DataValueField = GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
                        ddlUserType.DataBind();
                    }
                    break;
                // Fill Shift Details to DropDown
                case ControlsEnum.EMPLOYEE:
                    ddlEmployee.Items.Clear();
                    if (dtEmployees != null && dtEmployees.Rows.Count > 0)
                    {

                        ddlEmployee.DataSource = ERP.Utilities.CommonFunctions.HtmlDecodeDataTable(dtEmployees, GTIService.Constants.Configurations.Employees.Fields.EMPLOYEETEXT);

                        ddlEmployee.DataTextField = GTIService.Constants.Configurations.Employees.Fields.EMPLOYEETEXT;
                        ddlEmployee.DataValueField = GTIService.Constants.Configurations.Employees.Fields.PK;
                        ddlEmployee.DataBind();
                    }
                    ddlEmployee.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    ddlEmployee.SelectedIndex = -1;
                    break;
                case ControlsEnum.DEPARTMENT:
                    ddlDepartment.Items.Clear();
                    if (dtDepartment != null && dtDepartment.Rows.Count > 0)
                    {
                        ddlDepartment.DataSource = dtDepartment;
                        ddlDepartment.DataTextField = "DPT_NAME";
                        ddlDepartment.DataValueField = "DPT_PK";
                        ddlDepartment.DataBind();
                    }
                    ddlDepartment.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    ddlDepartment.SelectedIndex = -1;
                    break;
                case ControlsEnum.CULTURE:
                    ddlCulture.Items.Clear();
                    if (lstCulture.Count > 0)
                    {
                        ddlCulture.DataSource = lstCulture;
                        ddlCulture.DataValueField = "Culture";
                        ddlCulture.DataTextField = "Name";
                        ddlCulture.DataBind();
                    }
                    break;
                case ControlsEnum.SECONDARYCULTURE:
                    ddlSecCulture.Items.Clear();
                    if (lstCulture.Count > 0)
                    {
                        ddlSecCulture.DataSource = lstCulture;
                        ddlSecCulture.DataValueField = "Culture";
                        ddlSecCulture.DataTextField = "Name";
                        ddlSecCulture.DataBind();
                    }
                    ddlSecCulture.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    ddlSecCulture.SelectedIndex = -1;
                    break;
            }
        }

        /// <summary>
        /// Get Value from Control to object
        /// </summary>
        /// <returns></returns>
        private object SetUIValuesToObject(UserTabs curTab)
        {
            object retObj = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            switch (curTab)
            {
                case UserTabs.LOCATIONMAPPING:
                    UserLocationMappingBO objLoc = new UserLocationMappingBO();
                    objLoc.UserPK = CurrPK;
                    objLoc.LocationDtailList = new System.Collections.Generic.List<UserLocationBO>();
                    foreach (TreeNode root in trvUserLocations.Nodes)
                    {
                        foreach (TreeNode node in root.ChildNodes)
                        {
                            if (node.Checked)
                            {
                                string[] rootVal = node.Value.Split(',');
                                objLoc.LocationDtailList.Add(new UserLocationBO
                                {
                                    TransactionPK = string.IsNullOrEmpty(rootVal[0]) ? 0 : Convert.ToInt32(rootVal[0]),
                                    LocationPK = Convert.ToInt32(rootVal[1]),

                                });
                            }
                        }
                    }
                    retObj = objLoc;
                    break;
                case UserTabs.MANAGEMISREPORT:
                    UserMISReportMappingBO objRpt = new UserMISReportMappingBO();
                    objRpt.USER_PK = currentUser.PKUser;
                    objRpt.RPT_USER = CurrPK;
                    objRpt.BIZUNIT_PK = currentUser.CurrentSBUPK;
                    objRpt.RptDetail = new System.Collections.Generic.List<UserReportsList>();
                    foreach (TreeNode root in trvMISReports.Nodes)
                    {
                        foreach (TreeNode node in root.ChildNodes)
                        {
                            foreach (TreeNode ChildNode in node.ChildNodes)
                            {
                                if (ChildNode.Checked)
                                {
                                    objRpt.RptDetail.Add(new UserReportsList
                                    {
                                        RPT_PK = Convert.ToInt32(ChildNode.Value)
                                    });
                                }
                            }
                        }
                    }
                    retObj = objRpt;
                    break;
                case UserTabs.USERINFO:
                    string password;
                    ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
                    UserManagementBO objUser = new UserManagementBO();
                    objUser.PK = CurrPK;
                    newUserPassword = txtPassword.Text.Trim();
                    objUser.Name = txtUserName.Text;
                    if (Page.Request.QueryString["UserProphile"] != null && Page.Request.QueryString["UserProphile"] == "1")
                    {
                        password = HttpUtility.HtmlEncode(crypto.EncryptString(TestString, System.Configuration.ConfigurationManager.AppSettings["salt"]));
                    }
                    else
                    {
                        password = HttpUtility.HtmlEncode(crypto.EncryptString(this.txtPassword.Text.Trim(), System.Configuration.ConfigurationManager.AppSettings["salt"]));
                    }
                    objUser.Password = password;
                    objUser.Employee = Convert.ToInt32(ddlEmployee.SelectedValue);
                    objUser.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                    objUser.Email = txtEmail.Text;
                    objUser.Theme = ddlTheme.SelectedValue.Trim();//ddlTheme.SelectedItem.Text;
                    objUser.IsAlertSound = chkIsAlertSound.Checked ? 1 : 0;
                    objUser.UserType = 1; // 1- Application User
                    objUser.Sid = "0";
                    objUser.IsPublicUser = chkIsPublicUser.Checked ? 1 : 0;
                    objUser.IsSysUser = UserType == (int)UserRight.SuperAdmin ? Convert.ToInt32(ddlUserType.SelectedValue) : 0;
                    if (ddlDepartment.Items.Count > 0 && Convert.ToInt32(ddlDepartment.SelectedValue) > 0)
                    {
                        objUser.DefaultDepartment = ddlDepartment.SelectedValue;
                    }

                    if (GetGlobalResourceObject("ConfigurationsRes", "CultureChangeVisibility").ToString().ToLower() == "true")
                        objUser.usrCulture = ddlCulture.SelectedValue;
                    else
                        objUser.usrCulture = GetGlobalResourceObject("ConfigurationsRes", "DefaultCulture").ToString();

                    if (GetGlobalResourceObject("ConfigurationsRes", "CultureChangeVisibility").ToString().ToLower() == "true" && ddlSecCulture.SelectedIndex != 0)
                        objUser.usrCultureSec = ddlSecCulture.SelectedValue;
                    //else
                    //    objUser.usrSecCulture = DBNull.Value.ToString();

                    string fileName = string.Empty;
                    if (fudSignature.HasFile)
                        fileName = txtUserName.Text + "_Signature" + Path.GetExtension(fudSignature.PostedFile.FileName);
                    else if (lblSignatureName.Text != string.Empty && (lblSignatureName.Text.Substring(0, lblSignatureName.Text.IndexOf('_')) != txtUserName.Text))
                        fileName = lblSignatureName.Text.Replace(lblSignatureName.Text.Substring(0, lblSignatureName.Text.IndexOf('_')), txtUserName.Text);
                    else
                        fileName = lblSignatureName.Text;
                    objUser.Signature = fileName;
                    objUser.usrDefInbox = chkInboxDeflt.Checked == true ? 1 : 0;
                    objUser.usrPhone = txtContactNo.Text;
                    retObj = objUser;
                    break;

            }
            return retObj;
        }

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            string SignaturePath = string.Empty;
            try
            {
                if (dtUserDtl != null && dtUserDtl.Rows.Count > 0)
                {
                    ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
                    ddlEmployee.SelectedIndex = Convert.ToInt32(ddlEmployee.Items.IndexOf(ddlEmployee.Items.FindByValue(dtUserDtl.Rows[0]["usrEmployee"].ToString())));
                    ddlCulture.SelectedIndex = Convert.ToInt32(ddlCulture.Items.IndexOf(ddlCulture.Items.FindByValue(dtUserDtl.Rows[0]["usrCulture"].ToString())));
                    ddlSecCulture.SelectedIndex = Convert.ToInt32(ddlSecCulture.Items.IndexOf(ddlSecCulture.Items.FindByValue(dtUserDtl.Rows[0]["usrCultureSec"].ToString())));
                    //ddlTheme.SelectedIndex = Convert.ToInt32(ddlTheme.Items.IndexOf(ddlTheme.Items.FindByText(dtUserDtl.Rows[0]["usrTheme"].ToString())));
                    ddlTheme.SelectedValue = dtUserDtl.Rows[0]["usrTheme"].ToString();
                    ddlStatus.SelectedIndex = Convert.ToInt32(ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(dtUserDtl.Rows[0]["usrStatus"].ToString())));
                    CurrStatus = Convert.ToInt32(dtUserDtl.Rows[0]["usrStatus"].ToString());
                    txtUserName.Text = dtUserDtl.Rows[0]["usrName"].ToString();
                    string password = dtUserDtl.Rows[0]["usrPassword"].ToString();
                    password = crypto.DecryptString(HttpUtility.HtmlDecode(password), System.Configuration.ConfigurationManager.AppSettings["salt"]);
                    TestString = password;
                    txtPassword.Attributes.Add("value", password);
                    txtConfirmPwd.Attributes.Add("value", password);
                    txtEmail.Text = dtUserDtl.Rows[0]["usrEmail"].ToString();
                    lblSignatureName.Text = dtUserDtl.Rows[0]["usrSignature"].ToString();
                    chkIsAlertSound.Checked = string.IsNullOrEmpty(dtUserDtl.Rows[0]["usrHasAlertSound"].ToString()) ? false : Convert.ToInt32(dtUserDtl.Rows[0]["usrHasAlertSound"].ToString()) == 1 ? true : false;
                    chkIsPublicUser.Checked = string.IsNullOrEmpty(dtUserDtl.Rows[0]["usrIsPublic"].ToString()) ? false : Convert.ToInt32(dtUserDtl.Rows[0]["usrIsPublic"].ToString()) == 1 ? true : false;
                    //ddlDepartment.SelectedIndex = Convert.ToInt32(ddlDepartment.Items.IndexOf(ddlDepartment.Items.FindByText(dtUserDtl.Rows[0]["usrDefaultDept"].ToString())));
                    if (ddlDepartment.Items.Count > 0)
                        ddlDepartment.SelectedValue = dtUserDtl.Rows[0]["usrDefaultDept"].ToString();
                    ddlUserType.SelectedValue = dtUserDtl.Rows[0]["usrIsSysUser"].ToString();
                    txtContactNo.Text = dtUserDtl.Rows[0]["usrPhone"].ToString();

                    anchorFile.Visible = true;
                    //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    //{
                    //    SignaturePath = Server.MapPath(GetLocalResourceObject("AttachPath").ToString());
                    //}
                    //else
                    //{
                    //    SignaturePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                    //}

                    anchorFile.HRef = "~/Upload/" + dtUserDtl.Rows[0]["usrSignature"].ToString();
                    anchorFile.InnerHtml = dtUserDtl.Rows[0]["usrSignature"].ToString();

                    chkInboxDeflt.Checked = string.IsNullOrEmpty(dtUserDtl.Rows[0]["usrDefInbox"].ToString()) ? false : Convert.ToInt32(dtUserDtl.Rows[0]["usrDefInbox"].ToString()) == 1 ? true : false;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDeleteButton", "ShowHideDelete();", true);
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
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
        /// <summary>
        /// Resets the form for a fresh entry
        /// </summary>
        private void ResetForm()
        {
            //Codes for Clearing the controls in the page
            CurrPK = 0;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlEmployee.SelectedIndex = -1;
            ddlStatus.SelectedIndex = -1;
            chkIsPublicUser.Checked = true;
            chkInboxDeflt.Checked = false;
        }
        /// <summary>
        /// Set New User Info
        /// </summary>
        /// <param name="result"></param>
        private void SetNewUserInfo(int result)
        {
            txtPassword.Attributes.Add("value", newUserPassword);
            txtConfirmPwd.Attributes.Add("value", newUserPassword);
            CurrPK = result;
        }

        /// <summary>
        /// <summary>
        /// Save signature
        /// </summary>
        private bool SaveSignature()
        {
            try
            {
                string fileName = string.Empty;
                string targetPath = string.Empty;
                //string targetPath = Server.MapPath(GetLocalResourceObject("AttachPath").ToString());
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                {
                    targetPath = Server.MapPath(GetLocalResourceObject("AttachPath").ToString());
                    if (!Directory.Exists(targetPath))
                        Directory.CreateDirectory(targetPath);
                }
                else
                {
                    //SavePath = Server.MapPath("../Upload");
                    targetPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                }

                if (fudSignature.HasFile)
                {
                    fileName = txtUserName.Text + "_Signature" + Path.GetExtension(fudSignature.PostedFile.FileName);
                    targetPath = targetPath + fileName;
                    fudSignature.SaveAs(targetPath);

                    if (lblSignatureName.Text != string.Empty && (lblSignatureName.Text.Substring(0, lblSignatureName.Text.IndexOf('_')) != txtUserName.Text))
                    {
                        targetPath = targetPath.Replace(fileName, lblSignatureName.Text);
                        if (File.Exists(targetPath))
                            File.Delete(targetPath);
                    }
                }
                else
                {
                    if (lblSignatureName.Text != string.Empty && (lblSignatureName.Text.Substring(0, lblSignatureName.Text.IndexOf('_')) != txtUserName.Text))
                    {
                        targetPath = targetPath + lblSignatureName.Text;
                        if (File.Exists(targetPath))
                            File.Move(targetPath, targetPath.Replace(lblSignatureName.Text.Substring(0, lblSignatureName.Text.IndexOf('_')), txtUserName.Text));
                    }
                }
                return true;
            }
            catch
            {
                litErrorMsg.Text = this.GetLocalResourceObject("Err_SaveSignature").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                return false;
            }
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
                trvUserLocations.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                if (!IsPostBack)
                {
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    SetConfigValue();
                    HideTabByResource();
                    GetFieldValues(ControlsEnum.USERTYPE);
                    SetFieldValues(ControlsEnum.USERTYPE);
                    GetFieldValues(ControlsEnum.EMPLOYEE);
                    SetFieldValues(ControlsEnum.EMPLOYEE);
                    GetFieldValues(ControlsEnum.CULTURE);
                    SetFieldValues(ControlsEnum.CULTURE);
                    //GetFieldValues(ControlsEnum.SECONDARYCULTURE);
                    SetFieldValues(ControlsEnum.SECONDARYCULTURE);
                    if (Request.QueryString["UserID"] != null)
                    {
                        CurrPK = Convert.ToInt32(Request.QueryString["UserID"].ToString());

                        GetFieldValues(ControlsEnum.DEPARTMENT);
                        SetFieldValues(ControlsEnum.DEPARTMENT);
                        GetFieldValues(ControlsEnum.USER);
                        SetFieldValues(ControlsEnum.USER);
                    }
                    if (Page.Request.QueryString["UserProphile"] != null && Page.Request.QueryString["UserProphile"] == "1")
                    {
                        btnCancel.Visible = false;
                        lblBreadCrum.Visible = false;
                        divTabContainer.Visible = false;
                        ddlEmployee.Enabled = false;
                        ddlStatus.Enabled = false;
                        txtPassword.Enabled = false;
                        txtConfirmPwd.Enabled = false;
                        divBtnContainer.Attributes.Add("class", "Button-iframe");
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
            EMPLOYEE,
            USER,
            SAVE,
            CANCEL,
            DEPARTMENT,
            USERTYPE,
            LOCATIONSMAPPED,
            USERMODULE,
            CULTURE,
            SECONDARYCULTURE,
            MANAGEMISRPT
        }
        private enum UserTabs
        {
            LIST = 0,
            USERINFO = 1,
            LOCATIONMAPPING = 2,
            USERMODULE = 3,
            MANAGEMISREPORT = 4
        }

        #endregion
    }
}