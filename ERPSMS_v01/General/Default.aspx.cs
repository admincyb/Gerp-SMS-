using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using DataAccess.CommonManagement;
using BusinessLogic.Administration.Configurations;
using System.Xml;
using System.IO;
using System.Resources;
using System.Collections;
using System.Web.Security;
using BusinessLogic.CommonManagement;
using System.Configuration;

namespace ERPSMS_v01.General
{
    public partial class Default : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        #region Variables
        private ActionsEnum commonActions;
        private DataTable dtFolders;
        private DataTable dtResources;
        private DataTable dtResourceValues;
        private DataTable dtResult;
        private DataTable dtModule;
        private BusinessObject.User currentUser;
        #endregion

        #region Properties
        /// <summary>
        /// To Keep Root URL
        /// </summary>
        private string RootURL
        {
            get { return this.ViewState["RootURL"] == null ? string.Empty : this.ViewState["RootURL"].ToString(); }
            set { this.ViewState["RootURL"] = value; }
        }
        /// <summary>
        /// To validate User 
        /// </summary>
        private bool IsValidState
        {
            get { return this.ViewState["IsValidState"] == null ? false : Convert.ToBoolean(this.ViewState["IsValidState"].ToString()); }
            set { this.ViewState["IsValidState"] = value; }
        }
        private string RetRootURL
        {
            get { return this.ViewState["RetRootURL"] == null ? string.Empty : this.ViewState["RetRootURL"].ToString(); }
            set { this.ViewState["RetRootURL"] = value; }
        }
        #endregion

        #endregion

        #region PageLevel Events

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            SetPageVariables();
            if (!IsSuperAdminUser(currentUser.PKUser))
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                ResetForm();
                rbnGlobalResource.Checked = true;
                GetFieldValues(ControlEnum.MODULE);
                if (ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    RootURL = GetRootPath();
                GetFieldValues(ControlEnum.FILLGLOBALRESOURCEFOLDER);
                SetFieldValues(ControlEnum.FILLGLOBALRESOURCEFOLDER);
            }
        }
        #endregion

        #region Page_PreRender
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            if (IsValidState)
            {
                divManageResource.Visible = true;
                divUserLogin.Visible = false;
            }
            else
            {
                divManageResource.Visible = false;
                divUserLogin.Visible = true;
            }
        }
        #endregion

        #endregion

        #region Get Field Values
        private string GetFormattedURL(string url)
        {
            string result = string.Empty;
            string[] split;
            if (!string.IsNullOrEmpty(url))
            {
                split = url.Split('/');
                result = split.Length > 1 ? split[1] : split[0];
            }
            return result;
        }
        private void GetFieldValues(ControlEnum type)
        {
            int index = 0;
            string fileName = string.Empty;
            string folderName = string.Empty;
            string folderPath = string.Empty;
            try
            {
                switch (type)
                {
                    case ControlEnum.MODULE:
                        dtModule = BusinessLogic.CommonManagement.CommonBL.GetModule(0, 0, 1, currentUser.SBUID);

                        var distinctValues = dtModule.AsEnumerable()
                        .Select(row => new
                        {
                            MOD_SERVER = GetFormattedURL(row.Field<string>("MOD_SERVER"))
                        }).Where(v => v.MOD_SERVER != string.Empty)
                        .Distinct();
                        //  distinctValues = distinctValues.Where(a => a.MOD_SERVER != null);
                        ddlModuleServer.DataSource = distinctValues;
                        ddlModuleServer.DataTextField = GTIService.Constants.Common.Common.MOD_SERVER;
                        ddlModuleServer.DataValueField = GTIService.Constants.Common.Common.MOD_SERVER;
                        ddlModuleServer.DataBind();
                        ddlModuleServer.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        break;

                    case ControlEnum.FILLLOCALRESOURCEFOLDER:
                        folderPath = Server.MapPath("~/");
                        string[] DirLocal = Directory.GetDirectories(folderPath);
                        dtFolders = new DataTable();
                        dtFolders.Columns.AddRange(new DataColumn[] { new DataColumn("SlNo"), new DataColumn("FolderName") });
                        index = 0;
                        foreach (string item in DirLocal)
                        {
                            DataRow row = dtFolders.NewRow();
                            if (Directory.Exists(item + "\\App_LocalResources"))
                            {
                                string[] folder = item.Split(new[] { Server.MapPath("~/") }, StringSplitOptions.None);
                                row["SlNo"] = ++index;
                                row["FolderName"] = folder[1].ToString();
                                dtFolders.Rows.Add(row);
                            }
                        }
                        RootURL = folderPath;
                        break;
                    case ControlEnum.FILLGLOBALRESOURCEFOLDER:
                        folderPath = Server.MapPath("~/");
                        if (Directory.Exists(folderPath + "App_GlobalResources\\languages"))
                            folderPath = folderPath + "App_GlobalResources\\languages\\";
                        else
                            folderPath = folderPath + "App_GlobalResources\\";

                        string[] Dirfiles = Directory.GetDirectories(folderPath);
                        dtFolders = new DataTable();
                        dtFolders.Columns.AddRange(new DataColumn[] { new DataColumn("SlNo"), new DataColumn("FolderName") });
                        index = 0;
                        foreach (string item in Dirfiles)
                        {
                            DataRow row = dtFolders.NewRow();
                            string[] folder = item.Split(new[] { Server.MapPath("~/App_GlobalResources/languages//") }, StringSplitOptions.None);
                            row["SlNo"] = ++index;
                            row["FolderName"] = folder[1].ToString();
                            dtFolders.Rows.Add(row);
                        }
                        RootURL = folderPath;
                        break;
                    case ControlEnum.FILLRESOURCESUBFOLDER:
                        folderName = ddlFolder.SelectedItem.Text;
                        if (folderName != "Select")
                        {
                            string[] filePaths = Directory.GetFiles(RootURL + folderName, "*.resx", SearchOption.AllDirectories);
                            index = 0;
                            dtResources = new DataTable();
                            dtResources.Columns.AddRange(new DataColumn[] { new DataColumn("URL"), new DataColumn("ResourceName") });
                            foreach (string res in filePaths)
                            {
                                DataRow row = dtResources.NewRow();
                                row["URL"] = res;
                                row["ResourceName"] = res.Substring(res.LastIndexOf("\\") + 1, res.Length - res.LastIndexOf("\\") - 1);// filePathSplit[1].ToString();
                                dtResources.Rows.Add(row);
                            }
                        }
                        
                        break;
                    case ControlEnum.FILLGRID:
                        folderName = ddlFolder.SelectedItem.Text;
                        fileName = ddlResources.SelectedItem.Text;
                        dtResourceValues = FillResources(ddlResources.SelectedValue);
                        break;
                }

            }
            catch (Exception ex)
            {
                //Label1.Text = ex.InnerException.ToString();
                throw ex;
            }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnum.MODULE:
                        BindDropDown(ControlEnum.MODULE);
                        break;
                    case ControlEnum.FILLGLOBALRESOURCEFOLDER:
                        BindDropDown(ControlEnum.FILLGLOBALRESOURCEFOLDER);
                        break;
                    case ControlEnum.FILLRESOURCESUBFOLDER:
                        BindDropDown(ControlEnum.FILLRESOURCESUBFOLDER);
                        break;
                    case ControlEnum.FILLGRID:
                        BindGrid(controlType);
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

        #region Helper Method

        #region BindGrid
        /// <summary>
        /// BindGrid
        /// </summary>
        /// <param name="controlType"></param>
        protected void BindGrid(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region BIND  GRID
                    case ControlEnum.FILLGRID:
                        if (dtResourceValues != null && dtResourceValues.Rows.Count > 0)
                        {
                            DataView dv = dtResourceValues.DefaultView;
                            dv.Sort = "Name asc";
                            DataTable sortedDT = dv.ToTable();
                            grdResource.DataSource = sortedDT;
                        }
                        else
                            grdResource.DataSource = new DataTable();
                        grdResource.DataBind();
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

        #region BindDropdown
        /// <summary>
        /// Bind DropDown  as per type 
        /// </summary>
        /// <param name="drpName"></param>
        private void BindDropDown(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    // Fill Shift Details to DropDown
                    case ControlEnum.FILLGLOBALRESOURCEFOLDER:
                        if (dtFolders != null)
                        {
                            ddlFolder.DataSource = CommonFunctions.HtmlDecodeDataTable(dtFolders);
                            ddlFolder.DataTextField = "FolderName";
                            ddlFolder.DataValueField = "SlNo";
                            ddlFolder.DataBind();
                        }
                        ddlFolder.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;

                    case ControlEnum.FILLRESOURCESUBFOLDER:
                        if (dtResources.Rows.Count > 0)
                        {
                            ddlResources.DataSource = dtResources;
                            ddlResources.DataTextField = "ResourceName";
                            ddlResources.DataValueField = "URL";
                            ddlResources.DataBind();
                        }
                        else
                        {
                            ddlResources.Items.Clear();
                            ddlResources.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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

        #region GetUIValues
        private void SaveChanges()
        {
            string folderName = ddlFolder.SelectedItem.Text;
            string fileName = ddlResources.SelectedItem.Text;
            string sResxPath;
            sResxPath = ddlResources.SelectedValue;
            Hashtable data = new Hashtable();
            foreach (GridViewRow row in grdResource.Rows)
            {
                Label Key = (Label)row.FindControl("lblResName");
                TextBox Value = (TextBox)row.FindControl("txtResValue");
                data.Add(Key.Text, Value.Text);
            }
            UpdateResourceFile(data, sResxPath);
            litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Resource);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
        }
        #endregion

        #region Save and Update
        public static void UpdateResourceFile(Hashtable data, String path)
        {
            Hashtable resourceEntries = new Hashtable();
            //Get existing resources
            ResXResourceReader reader = new ResXResourceReader(path);
            if (reader != null)
            {
                IDictionaryEnumerator id = reader.GetEnumerator();
                foreach (DictionaryEntry d in reader)
                {
                    if (d.Value == null)
                        resourceEntries.Add(d.Key.ToString(), "");
                    else
                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                } reader.Close();
            }
            //Modify resources here...
            foreach (String key in data.Keys)
            {
                if (!resourceEntries.ContainsKey(key))
                {
                    String value = data[key].ToString();
                    if (value == null)
                        value = "";
                    resourceEntries.Add(key, value);
                }
                else
                {
                    String value = data[key].ToString();
                    if (value == null)
                        value = "";
                    resourceEntries.Remove(key);
                    resourceEntries.Add(key, data[key].ToString());
                }
            }
            //Write the combined resource file
            ResXResourceWriter resourceWriter = new ResXResourceWriter(path);
            foreach (String key in resourceEntries.Keys)
            {
                resourceWriter.AddResource(key, resourceEntries[key]);
            }
            resourceWriter.Generate();
            resourceWriter.Close();
        }
        #endregion

        #region Fill Resources
        /// <summary>
        /// Fill Resources
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public DataTable FillResources(string URL)
        {
            ResXResourceReader resRreader = new ResXResourceReader(URL);
            IDictionaryEnumerator id = resRreader.GetEnumerator();
            DataTable dtResourceValues = new DataTable();
            dtResourceValues.Columns.Add(new DataColumn("Name", System.Type.GetType("System.String")));
            dtResourceValues.Columns.Add(new DataColumn("Value", System.Type.GetType("System.String")));
            foreach (DictionaryEntry dicRow in resRreader)
            {
                DataRow dr = dtResourceValues.NewRow();
                dr["Name"] = dicRow.Key.ToString();
                dr["Value"] = dicRow.Value.ToString();
                dtResourceValues.Rows.Add(dr);
            }
            return dtResourceValues;
        }
        #endregion

        #region Is Super Admin User
        /// <summary>
        /// Is Super Admin User
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
        #endregion

        #region IsValidUser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool IsValidUser(string password)
        {

            bool result = false;
            dtResult = CommonBL.GetApplicaitonConfiguaration("MAIL STATUS", "MTTB", currentUser.SBUID);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                result = dtResult.Rows[0]["ACF_DATA"].ToString().ToLower() == password.ToLower();
            }
            return result;
        }
        #endregion

        #region Return URL
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string GetRootPath()
        {
            string RootPath = string.Empty;
            RootPath = Server.MapPath("~/");
            string[] Split = RootPath.Split('\\');
            ddlModuleServer.SelectedValue = Split[Split.Length - 1];
            return RootPath;
        }
        #endregion

        #region SetPageVariables
        private void SetPageVariables()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            grdResource.DataSource = new DataTable();
            grdResource.DataBind();
            ddlFolder.Items.Clear();
            ddlResources.Items.Clear();
            ddlFolder.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
            ddlResources.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
        }
        #endregion

        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlFolder")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                    else if (((DropDownList)sender).ID == "ddlResources")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGEDRES;
                    }
                    else if (((DropDownList)sender).ID == "ddlModuleServer")
                    {
                        commonActions = ActionsEnum.SELECT;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbnGlobalResource")
                    {
                        commonActions = ActionsEnum.SELECTEGLOBALFOLDER;
                    }
                    else if (((RadioButton)sender).ID == "rbnLocalResource")
                    {
                        commonActions = ActionsEnum.SELECTELOCALFOLDER;
                    }
                }
                switch (commonActions)
                {
                    #region Select Module Server
                    case ActionsEnum.SELECT:
                        //ResetForm();
                        if (rbnGlobalResource.Checked)
                        {
                            //Label1.Text = Label1.Text + "  In Global -> ";
                            //Label1.Text = Label1.Text +  RootURL + ddlModuleServer.SelectedValue + "\\";
                            //GetFieldValues(ControlEnum.FILLGLOBALRESOURCEFOLDER);
                            //SetFieldValues(ControlEnum.FILLGLOBALRESOURCEFOLDER);
                        }
                        else
                        {
                            //GetFieldValues(ControlEnum.FILLLOCALRESOURCEFOLDER);
                            //SetFieldValues(ControlEnum.FILLLOCALRESOURCEFOLDER);
                        }

                        break;
                    #endregion
                    #region SelectIndexChanged
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        #region closed
                        //if (ddlMappingType.SelectedValue == "0")
                        //{
                        //    lbllocalType.Visible = false;
                        //    ddlFolder.Visible = true;
                        //    ddlMappingType.Visible = true;
                        //}
                        //else
                        //{
                        //    lbllocalType.Visible = true;
                        //    ddlFolder.Visible = true;
                        //    ddlMappingType.Visible = true;
                        //}
                        #endregion
                        //ddlResources.Items.Clear();
                        grdResource.DataSource = new DataTable();
                        grdResource.DataBind();
                        GetFieldValues(ControlEnum.FILLRESOURCESUBFOLDER);
                        SetFieldValues(ControlEnum.FILLRESOURCESUBFOLDER);
                        break;
                    case ActionsEnum.SELECTEDINDEXCHANGEDRES:
                        grdResource.DataSource = new DataTable();
                        grdResource.DataBind();
                        GetFieldValues(ControlEnum.FILLRESOURCESUBFOLDER);
                        break;
                    case ActionsEnum.SELECTEGLOBALFOLDER:
                        ResetForm();
                        GetFieldValues(ControlEnum.FILLGLOBALRESOURCEFOLDER);
                        SetFieldValues(ControlEnum.FILLGLOBALRESOURCEFOLDER);
                        break;
                    case ActionsEnum.SELECTELOCALFOLDER:
                        ResetForm();
                        GetFieldValues(ControlEnum.FILLLOCALRESOURCEFOLDER);
                        SetFieldValues(ControlEnum.FILLGLOBALRESOURCEFOLDER);
                        break;
                    #endregion
                    #region Save
                    case ActionsEnum.SAVE:
                        SaveChanges();
                        break;
                    #endregion
                    #region Search
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlEnum.FILLGRID);
                        BindGrid(ControlEnum.FILLGRID);
                        break;
                    #endregion
                    #region Submit
                    case ActionsEnum.SUBMIT:
                        IsValidState = IsValidUser(txtPassword.Text);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        rbnGlobalResource.Checked = true;
                        rbnLocalResource.Checked = false;
                        GetFieldValues(ControlEnum.FILLGLOBALRESOURCEFOLDER);
                        SetFieldValues(ControlEnum.FILLGLOBALRESOURCEFOLDER);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("CashBankDuplicate").ToString()))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.Code)) + "','" + Resources.Messages.Information + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }
        #endregion

        #region ControlEnum
        public enum ControlEnum
        {
            MODULE,
            FILLGLOBALRESOURCEFOLDER,
            FILLLOCALRESOURCEFOLDER,
            FILLRESOURCESUBFOLDER,
            FILLGRID,
            BINDGRID,
            SAVE
        }
        #endregion
    }
}
