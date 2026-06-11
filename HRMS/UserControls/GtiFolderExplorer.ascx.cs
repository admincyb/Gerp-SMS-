#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessObject.HRMS.Employee;
using BusinessObject.UserControls;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using BusinessLogic.UserControl;
using System.Data;
using BusinessObject.HRMS.eDocs;
#endregion

namespace HRMS.UserControls
{
    #region Gti Folder Explorer
    public partial class GtiFolderExplorer : System.Web.UI.UserControl
    {
        #region Private data members
        private string _blankXml = @"<Folder FLD_PK=""-1"" FLD_NAME=""DMS"" FLD_LEVEL=""0"" FLD_SEQUENCE=""0"" FLD_CHILD_COUNT=""-1"" FLD_FILE_COUNT=""-1"" LAST_MOD_DT="""" >{0}</Folder>";
        private ActionsEnum commonActions;
        private string[] _valueAttributes = new string[] { "FLD_PK", "FLD_LEVEL", "FLD_SEQUENCE", "LAST_MOD_DT" };
        private string[] _deleteCheckAttributes = new string[] { "FLD_CHILD_COUNT", "FLD_FILE_COUNT" };
        private string _nameAttribute = "FLD_NAME";
        private BusinessObject.User currentUser;
        private DataTable dtPageData;
        #endregion

        #region Events Handlers
        public event EventHandler FolderDeleted;
        public event EventHandler FolderAdded;
        public event EventHandler FolderEdited;
        public event EventHandler Error;
        #endregion

        #region Private & Public Properties

        #region Private

        /// <summary>
        /// Current PK
        /// </summary>
        private long CurrPK
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }
        /// <summary>
        /// Preserve Selected/CurrentPk in Temp Field for cancellation
        /// </summary>
        private long TempPk
        {
            get
            {
                return this.ViewState[ViewstateStrings.TempPk] == null ? -1 : Convert.ToInt64(this.ViewState[ViewstateStrings.TempPk]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TempPk] = value;
            }
        }
        /// <summary>
        /// Current PK
        /// </summary>
        private long ParentPK
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.ParentPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ParentPK] = value;
            }
        }
        /// <summary>
        /// To maintain the LastModifiedTime in viewstate
        /// </summary>
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
        /// <summary>
        /// Caching User Mapping Permission.
        /// </summary>
        private bool HasMappingPermission
        {
            get
            {
                return this.ViewState[ViewstateStrings.HasMappingPermission] == null
                    ? SetMappingPermission()
                    : Convert.ToBoolean(this.ViewState[ViewstateStrings.HasMappingPermission]);
            }
            set
            {
                this.ViewState[ViewstateStrings.HasMappingPermission] = value;
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Determine if command bar is shown
        /// </summary>
        public bool ShowComandBar
        {
            get { return this.ViewState[ViewstateStrings.ShowCommandBar] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.ShowCommandBar]); }
            set { this.ViewState[ViewstateStrings.ShowCommandBar] = value; }
        }
        /// <summary>
        /// Current Display mode of the control
        /// </summary>
        public FolderBrowserDisplayMode DisplayMode
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisplayMode] == null
                    ? FolderBrowserDisplayMode.View
                    : (FolderBrowserDisplayMode)this.ViewState[ViewstateStrings.DisplayMode];
            }
            set { this.ViewState[ViewstateStrings.DisplayMode] = value; }
        }
        /// <summary>
        /// Maximum Folder Display characters
        /// </summary>
        public int ShowMaximumCharacters
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowMaximumCharacters] == null ? -1 : (int)this.ViewState[ViewstateStrings.ShowMaximumCharacters];
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowMaximumCharacters] = value;
            }
        }
        /// <summary>
        /// Returns the currently selected Item Pk
        /// </summary>
        public long SelectedPk
        {
            get { return this.CurrPK; }
            set
            {
                this.CurrPK = value;
                if (this.TempPk == -1)
                    this.TempPk = value;
                SetSelection(value);
            }
        }
        /// <summary>
        /// Node Text Name Field Key
        /// </summary>
        public string NameAttribute
        {
            get { return _nameAttribute; }
            set { _nameAttribute = value; }
        }
        /// <summary>
        /// Use Comma Seperated string to multiple value fileds
        /// </summary>
        /// 
        public string ValueAttribute
        {
            get { return string.Join(",", _valueAttributes); }
            set
            {
                _valueAttributes = value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
        }
        /// <summary>
        /// Use Comma Seperated string to multiple value fileds
        /// </summary>
        ///
        public string DeleteCheckAttribute
        {
            get { return string.Join(",", _deleteCheckAttributes); }
            set
            {
                _deleteCheckAttributes = value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
        }
        /// <summary>
        /// Datasource
        /// </summary>
        public XmlDocument DataSource { private get; set; }

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptInclude(this, GetType(), "controlscript", ResolveUrl("GtiFolderExplorer.js"));
            if (!IsPostBack)
            {

            }
            if (string.IsNullOrWhiteSpace(this.NameAttribute) || string.IsNullOrWhiteSpace(this.ValueAttribute))
                throw new InvalidOperationException(GetLocalResourceObject("InvalidControlStateExceptionMessage").ToString());

            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }

        /// <summary>
        /// Managing all view Rendering Logic Here (Hiding or Viewing Elements)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            HideAll();
            int popupHeight = 500;
            switch (this.DisplayMode)
            {
                #region View
                case FolderBrowserDisplayMode.View:
                    folderHeader.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                    break;
                #endregion
                #region Expanded
                case FolderBrowserDisplayMode.Expanded:
                    if (this.ShowComandBar)
                        commandBar.Visible = true;
                    popupHeight = this.ShowComandBar ? 500 : 470;
                    folderBrowserBody.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page
                                    , typeof(Page)
                                    , "ShowExpandedView"
                                    , "ClosePopup();ShowContainerDiv('[id$=gtiFolderExplorerBody]','" + Resources.Captions.FolderExplorer + "','500','" + popupHeight + "',afterFolderClose);"
                                    , true);
                    break;
                #endregion
                #region Collapsed
                case FolderBrowserDisplayMode.Collapsed:
                    imbBrowseFolder.Visible = true;
                    folderHeader.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                    break;
                #endregion
                #region Message View
                case FolderBrowserDisplayMode.MessageView:
                    this.DisplayMode = FolderBrowserDisplayMode.Expanded;
                    break;
                #endregion
                #region Mapping View
                case FolderBrowserDisplayMode.MappingView:
                    if (this.ShowComandBar)
                        commandBar.Visible = true;
                    popupHeight = this.ShowComandBar ? 500 : 470;
                    folderBrowserBody.Visible = true;
                    userMappingColumn.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page
                                    , typeof(Page)
                                    , "ShowExpandedView"
                                    , "ClosePopup();ShowContainerDiv('[id$=gtiFolderExplorerBody]','" + Resources.Captions.FolderExplorer + "','700','" + popupHeight + "',afterFolderClose);"
                                    , true);
                    break;
                #endregion
            }
        }

        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                #region Local Variables
                int result;
                string errorMessage = string.Empty;
                #endregion

                #region Get CommandName
                if (sender is Button)
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender is ImageButton)
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender is TreeView)
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                #endregion

                #region Process Command
                switch (commonActions)
                {
                    #region Save
                    case ActionsEnum.SAVE:
                        FolderBO folderBo = (FolderBO)SetUIValuesToObject(ControlEnums.FOLDER);
                        result = FolderExplorerBL.Save(folderBo);
                        if (result > 0)
                        {
                            this.CurrPK = result;
                            imbSaveFolder.Enabled = false;
                            errorMessage = Resources.ErrorMessages.Msg_Save_Success;
                            errorMessage = string.Format(errorMessage, Resources.PageNameRes.FolderExplorer);

                            this.DisplayMode = FolderBrowserDisplayMode.MessageView;
                            if (this.CurrPK == 0 && FolderAdded != null)
                                btnFolderBrowserEventInvoker.CommandName = ActionsEnum.FOLDERCREATED.ToString();
                            else if (this.CurrPK > 0 && FolderEdited != null)
                                btnFolderBrowserEventInvoker.CommandName = ActionsEnum.FOLDERMODIFIED.ToString();

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + errorMessage + "','" + Resources.Messages.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                        }
                        else
                        {
                            this.DisplayMode = FolderBrowserDisplayMode.MessageView;
                            btnFolderBrowserEventInvoker.CommandName = ActionsEnum.ERROR.ToString();

                            #region Display Error Messages
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                errorMessage = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                errorMessage = Resources.PageNameRes.FolderExplorer + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                errorMessage = Resources.PageNameRes.FolderExplorer + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                errorMessage = Resources.PageNameRes.FolderExplorer + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                            }
                            else
                            {
                                errorMessage = Resources.Messages.ActionFailedPleaseTryAgain;
                                errorMessage = string.Format(errorMessage, Resources.PageNameRes.FolderExplorer);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        if (folderExplorerTreeView.SelectedNode == null)
                            throw new InvalidOperationException(GetLocalResourceObject("NodeNotSelectedExceptionMessage").ToString());

                        this.CurrPK = Convert.ToInt64(hdfSelectedNodeValue.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].Trim());

                        txtFolderName.Enabled = true;
                        txtFolderName.Text = folderExplorerTreeView.SelectedNode.Text;
                        hdfSelectedNodeValue.Value = folderExplorerTreeView.SelectedNode.Value;
                        imbSaveFolder.Enabled = true;
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (this.CanDelete())
                        {
                            this.CurrPK = Convert.ToInt64(hdfSelectedNodeValue.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].Trim());//Bug : 6896
                            result = FolderExplorerBL.Delete(this.CurrPK, this.LastModifiedTime);
                            if (result > 0)
                            {
                                this.CurrPK = this.ParentPK;
                                txtFolderName.Text = string.Empty;
                                this.TempPk = this.CurrPK;
                                errorMessage = Resources.ErrorMessages.Msg_Delete_Success;
                                errorMessage = string.Format(errorMessage, Resources.PageNameRes.FolderExplorer);
                                this.DisplayMode = FolderBrowserDisplayMode.MessageView;
                                btnFolderBrowserEventInvoker.CommandName = ActionsEnum.FOLDERDELETED.ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + errorMessage + "','" + Resources.Messages.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                            }
                            else
                            {
                                this.DisplayMode = FolderBrowserDisplayMode.MessageView;
                                btnFolderBrowserEventInvoker.CommandName = ActionsEnum.ERROR.ToString();

                                #region Display Error Message
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    errorMessage = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    errorMessage = Resources.PageNameRes.FolderExplorer + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                                    this.DataBind();
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    errorMessage = Resources.PageNameRes.FolderExplorer + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    errorMessage = Resources.PageNameRes.FolderExplorer + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                                    this.DataBind();
                                }
                                else
                                {
                                    errorMessage = Resources.Messages.ActionFailedPleaseTryAgain;
                                    errorMessage = string.Format(errorMessage, Resources.PageNameRes.FolderExplorer);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                                }
                                #endregion
                            }
                        }
                        break;
                    #endregion
                    #region Add Item
                    case ActionsEnum.ADDITEM:
                        if (folderExplorerTreeView.SelectedNode == null)
                            throw new InvalidOperationException(GetLocalResourceObject("NodeNotSelectedExceptionMessage").ToString());

                        this.CurrPK = 0;
                        this.ParentPK = Convert.ToInt64(folderExplorerTreeView
                                                       .SelectedNode.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0]
                                                       .Trim());

                        //hdfSelectedNodeValue.Value = string.Empty; //Bug : 6890
                        txtFolderName.Text = string.Empty;
                        imbSaveFolder.Enabled = true;
                        txtFolderName.Enabled = true;
                        break;
                    #endregion
                    #region Selected Index Changed
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        AfterNodeSelect();
                        break;
                    #endregion
                    #region Browse
                    case ActionsEnum.BROWSE:
                        if (this.TempPk == -1) this.TempPk = 0;
                        SetSelection(this.SelectedPk);
                        this.DisplayMode = FolderBrowserDisplayMode.Expanded;
                        break;
                    #endregion
                    #region Select
                    case ActionsEnum.SELECT:
                        this.TempPk = this.CurrPK;
                        this.SelectedPk = this.CurrPK;
                        imbSaveFolder.Enabled = false;
                        this.DisplayMode = FolderBrowserDisplayMode.Collapsed;
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        this.SelectedPk = this.TempPk;
                        txtFolderName.Text = string.Empty;
                        imbSaveFolder.Enabled = false;
                        this.DisplayMode = FolderBrowserDisplayMode.Collapsed;
                        break;
                    #endregion
                    #region User Mapping
                    case ActionsEnum.MAPPING:
                        if (this.CurrPK > 0)
                        {
                            GetFieldValues(ControlEnums.USERS);
                            SetFieldValues(ControlEnums.USERS);
                            this.DisplayMode = FolderBrowserDisplayMode.MappingView;
                        }
                        else
                        {
                            ResetForm(ActionsEnum.MAPPING);
                            this.DisplayMode = FolderBrowserDisplayMode.MessageView;
                            btnFolderBrowserEventInvoker.CommandName = ActionsEnum.ERROR.ToString();
                            ScriptManager.RegisterStartupScript(this.Page
                                , typeof(Page)
                                , "ShowErrorMsg"
                                , "closeOverlayDiv();ShowErrorMessageCallBack('" + GetLocalResourceObject("FolderNotSelectedExceptionMessage").ToString() + "','" + Resources.Messages.Information + "',null,null,afterFolderBrowserMessageClose);"
                                , true);
                        }
                        break;
                    #endregion
                    #region Assaign To Users
                    case ActionsEnum.ASSAIGN:
                        FolderMappingBo folderMappingBo = (FolderMappingBo)SetUIValuesToObject(ControlEnums.USERS);
                        result = FolderExplorerBL.SaveUserMapping(folderMappingBo);
                        if (result > 0)
                        {
                            errorMessage = GetLocalResourceObject("SuccesfullyMappedMessage").ToString();
                            this.DisplayMode = FolderBrowserDisplayMode.MessageView;
                            btnFolderBrowserEventInvoker.CommandName = ActionsEnum.USERMAPPED.ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + errorMessage + "','" + Resources.Messages.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                        }
                        else
                        {
                            this.DisplayMode = FolderBrowserDisplayMode.MessageView;
                            btnFolderBrowserEventInvoker.CommandName = ActionsEnum.ERROR.ToString();

                            #region Dispaly rror Message
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                errorMessage = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                errorMessage = Resources.PageNameRes.FolderExplorer + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                errorMessage = Resources.PageNameRes.FolderExplorer + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                errorMessage = Resources.PageNameRes.FolderExplorer + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                            }
                            else
                            {
                                errorMessage = Resources.Messages.ActionFailedPleaseTryAgain;
                                errorMessage = string.Format(errorMessage, Resources.PageNameRes.FolderExplorer);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(errorMessage) + "','" + Resources.ErpRes.Information + "',null,null,afterFolderBrowserMessageClose);", true);
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region User Mapped
                    case ActionsEnum.USERMAPPED:
                        GetFieldValues(ControlEnums.USERS);
                        SetFieldValues(ControlEnums.USERS);
                        this.DisplayMode = FolderBrowserDisplayMode.MappingView;
                        break;
                    #endregion

                    #region Folder Created
                    case ActionsEnum.FOLDERCREATED:
                        if (this.FolderAdded != null)
                            FolderAdded(this, EventArgs.Empty);
                        break;
                    #endregion
                    #region Folder Modified
                    case ActionsEnum.FOLDERMODIFIED:
                        if (this.FolderEdited != null)
                            FolderEdited(this, EventArgs.Empty);
                        break;
                    #endregion
                    #region Folder Deleted
                    case ActionsEnum.FOLDERDELETED:
                        if (this.FolderDeleted != null)
                            FolderDeleted(this, EventArgs.Empty);
                        break;
                    #endregion
                    #region Error
                    case ActionsEnum.ERROR:
                        if (Error != null)
                            Error(this, EventArgs.Empty);
                        else
                            this.DisplayMode = FolderBrowserDisplayMode.Expanded;
                        break;
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                #region Handling Exception

                this.DisplayMode = FolderBrowserDisplayMode.MessageView;
                btnFolderBrowserEventInvoker.CommandName = ActionsEnum.ERROR.ToString();

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "closeOverlayDiv();ShowErrorMessageCallBack('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "',null,null,afterFolderBrowserMessageClose);", true);

                #endregion
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// String representation of the control
        /// </summary>
        /// <returns>The Selected Item Pk as string</returns>
        public override string ToString()
        {
            return this.SelectedPk.ToString();
        }

        /// <summary>
        ///  Bind the Folder Browser
        /// </summary>
        public override void DataBind()
        {
            base.DataBind();
            if (this.DataSource == null)
                CreateBlankXmlDocument();

            folderExplorerTreeView.Nodes.Clear();
            XmlNode xNode = this.DataSource.DocumentElement;
            string nodeText = xNode.Attributes[this.NameAttribute].Value;
            TreeNode tNode = new TreeNode(nodeText);

            #region Set Value Field
            string valueString = string.Empty;
            foreach (var item in _valueAttributes)
            {
                valueString += xNode.Attributes[item].Value + ",";
            }
            valueString = valueString.TrimEnd(',');
            #endregion

            #region Set Delete Permission
            bool canDelete = true; //_deleteCheckAttributes.Count() > 0;
            foreach (var item in _deleteCheckAttributes)
            {
                canDelete &= xNode.Attributes[item].Value == "0";
            }
            valueString += canDelete ? ",0" : ",1";
            #endregion

            tNode.Value = valueString;
            folderExplorerTreeView.Nodes.Add(tNode);

            if (this.DataSource.DocumentElement.HasChildNodes)
                AddNode(this.DataSource.DocumentElement, tNode);
            tNode.Expand();
            //folderExplorerTreeView.CollapseAll();
        }

        /// <summary>
        /// Set Datasource from hosting page as xml string 
        /// </summary>
        /// <param name="xmlData">XML string</param>
        public void SetDataSource(string xmlData)
        {
            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            XmlDocument xDoc = new XmlDocument();
            xmlData = string.Format(_blankXml, xmlData);
            xDoc.LoadXml(xmlData);
            SetDataSource(xDoc);
        }

        /// <summary>
        /// Set Datasource from hosting page as XmlDocument 
        /// </summary>
        /// <param name="xDocument">XmlDocument</param>
        public void SetDataSource(XmlDocument xDocument)
        {
            this.DataSource = xDocument;
        }

        /// <summary>
        /// Clear the Selection of the control, SelectedPk/ToString will return -1
        /// </summary>
        public void ClearSelection()
        {
            folderExplorerTreeView.Nodes[0].Select();
        }

        #endregion

        #region Private methods

        private void SetSelection(long id)
        {
            if (folderExplorerTreeView.SelectedNode != null)
                folderExplorerTreeView.SelectedNode.Selected = false;
            if (id < 1) id = -1;
            foreach (TreeNode node in folderExplorerTreeView.Nodes[0].ChildNodes)
            {
                node.Expand();
                if (id.ToString() == node.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].Trim())
                {
                    node.Selected = true;
                    AfterNodeSelect();
                    return;
                }
                if (node.ChildNodes.Count > 0)
                {
                    if (FindNodeByID(node, id))
                    {
                        AfterNodeSelect();
                        return;
                    }
                }
            }
            folderExplorerTreeView.Nodes[0].Selected = true;
            AfterNodeSelect();
        }

        private void AfterNodeSelect()
        {
            string strSelectedFolder = string.Empty;
            TreeNode selectedNode = folderExplorerTreeView.SelectedNode;
            if (selectedNode == null)
                throw new NullReferenceException();
            string folderPath = string.Empty;
            GetFullPath(selectedNode, ref folderPath);

            strSelectedFolder = ReArrangePath(folderPath).TrimEnd('\\');
            this.spnSelectedFolder.InnerText = ShowMaximumCharacters > 0 ? CommonFunctions.GetShortStringFromLast(strSelectedFolder, ShowMaximumCharacters) : strSelectedFolder;
            this.spnSelectedFolder.Attributes.Add("title", strSelectedFolder);

            bool delBtn = CanDelete();
            if (delBtn)
            {
                imbDeleteFolder.Visible = true;
                imbDeleteDisable.Visible = false;
            }
            else
            {
                imbDeleteFolder.Visible = false;
                imbDeleteDisable.Visible = true;
            }
            //imbDeleteFolder.Enabled = CanDelete();
            txtFolderName.Enabled = false;
            txtFolderName.Text = selectedNode.Text;
            hdfSelectedNodeValue.Value = selectedNode.Value;
            string[] arry = hdfSelectedNodeValue.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            this.CurrPK = Convert.ToInt64(arry[0].Trim());

            if (selectedNode.Parent != null)
            {
                this.ParentPK = Convert.ToInt64(selectedNode.Parent.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].Trim());
            }
            else
            {
                ParentPK = 0;
            }

            if (arry[0].Trim() != "-1")
                this.LastModifiedTime = Convert.ToDateTime(arry[arry.Length - 2].Trim());
            else
                this.LastModifiedTime = DateTime.Now;

            bool userMapBtn = IsUserMappingEnabled();
            if (userMapBtn)
            {
                imbUserMapping.Visible = true;
                imbUserDisable.Visible = false;
            }
            else
            {
                imbUserMapping.Visible = false;
                imbUserDisable.Visible = true;
            }
            //imbUserMapping.Enabled = IsUserMappingEnabled();
            if (IsUserMappingEnabled() && this.DisplayMode == FolderBrowserDisplayMode.MappingView)
            {
                GetFieldValues(ControlEnums.USERS);
                SetFieldValues(ControlEnums.USERS);
            }
            else if (this.DisplayMode != FolderBrowserDisplayMode.View && this.DisplayMode != FolderBrowserDisplayMode.Collapsed)
            {
                ResetForm(ActionsEnum.MAPPING);
                this.DisplayMode = FolderBrowserDisplayMode.Expanded;
            }
        }

        private void HideAll()
        {
            commandBar.Visible = false;
            folderBrowserBody.Visible = false;
            imbBrowseFolder.Visible = false;
            userMappingColumn.Visible = false;
        }

        private void ResetForm(ActionsEnum type)
        {
            switch (type)
            {
                case ActionsEnum.MAPPING:
                    grdMappingUserList.DataSource = null;
                    grdMappingUserList.DataBind();
                    break;

            }
        }

        private void GetFieldValues(ControlEnums type)
        {
            try
            {
                switch (type)
                {
                    case ControlEnums.USERS:
                        dtPageData = FolderExplorerBL.GetMappingUsersList(this.CurrPK);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetFieldValues(ControlEnums controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnums.USERS:
                        BindGrid(ControlEnums.USERS);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindGrid(ControlEnums controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnums.USERS:
                        grdMappingUserList.DataSource = dtPageData;
                        grdMappingUserList.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList nodeList;
            int indx;

            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (indx = 0; indx <= nodeList.Count - 1; indx++)
                {
                    xNode = inXmlNode.ChildNodes[indx];
                    string text = HttpUtility.HtmlDecode(xNode.Attributes[this.NameAttribute].Value);
                    tNode = new TreeNode(text);

                    #region Set Value Field
                    string valueString = string.Empty;
                    foreach (var item in _valueAttributes)
                    {
                        valueString += xNode.Attributes[item].Value + ",";
                    }
                    valueString = valueString.TrimEnd(',');
                    #endregion

                    #region Set Delete Permission
                    bool canDelete = true;
                    foreach (var item in _deleteCheckAttributes)
                    {
                        canDelete &= xNode.Attributes[item].Value == "0";
                    }
                    valueString += canDelete ? ",0" : ",1";
                    #endregion

                    tNode.Value = valueString;
                    inTreeNode.ChildNodes.Add(tNode);

                    if (xNode.HasChildNodes)
                        AddNode(xNode, tNode);
                }
            }
            else
            {
                inTreeNode.Text = (inXmlNode.OuterXml).Trim();
            }
        }

        private bool FindNodeByID(TreeNode parentNode, long id)
        {
            foreach (TreeNode node in parentNode.ChildNodes)
            {
                node.Expand();
                if (id.ToString() == node.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].Trim())
                {
                    node.Selected = true;
                    string folderPath = string.Empty;
                    string strSelectedFolder = string.Empty;
                    GetFullPath(node, ref folderPath);
                    strSelectedFolder = ReArrangePath(folderPath).TrimEnd('\\');
                    this.spnSelectedFolder.InnerText = ShowMaximumCharacters > 0 ? CommonFunctions.GetShortStringFromLast(strSelectedFolder, ShowMaximumCharacters) : strSelectedFolder;
                    this.spnSelectedFolder.Attributes.Add("title", strSelectedFolder);
                    return true;
                }
                if (node.ChildNodes.Count > 0)
                {
                    if (FindNodeByID(node, id))
                        return true;
                }
            }
            return false;
        }

        private string ReArrangePath(string path)
        {
            string[] temp = path.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Array.Reverse(temp);
            return string.Join(@"\", temp).TrimStart('\\');
        }

        private void GetFullPath(TreeNode childNode, ref string fullPath)
        {
            fullPath += string.Format(@"{0}\", childNode.Text);
            if (childNode.Parent != null)
                GetFullPath(childNode.Parent, ref fullPath);
        }

        private bool CanDelete()
        {
            return folderExplorerTreeView.SelectedNode == null
                    ? false
                    : folderExplorerTreeView.SelectedNode.Value.Substring(folderExplorerTreeView.SelectedNode.Value.Length - 1) == "0";
        }

        private bool IsUserMappingEnabled()
        {
            return folderExplorerTreeView.SelectedNode != null
                    && this.CurrPK > 0
                    && HasMappingPermission;
        }

        private bool SetMappingPermission()
        {
            string configText = "EDOC USER GROUP SETUP";
            string mappingPermissionRoleID = FolderExplorerBL.GetMappingPermissionRole(configText, currentUser.SBUID);

            if (string.IsNullOrWhiteSpace(mappingPermissionRoleID))
            {
                HasMappingPermission = false;
            }
            else
            {
                string[] assaignedRoles = currentUser.Roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //Storing in Viewstate for caching,
                HasMappingPermission = assaignedRoles.Contains(mappingPermissionRoleID);
            }

            return HasMappingPermission;
        }

        private void CreateBlankXmlDocument()
        {
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(string.Format(_blankXml, string.Empty));
            this.DataSource = dom;
        }

        private object SetUIValuesToObject(ControlEnums controlType)
        {
            try
            {
                Object retObject;
                retObject = null;
                switch (controlType)
                {
                    case ControlEnums.FOLDER:
                        FolderBO folderBo = new FolderBO();
                        folderBo.FolderPk = this.CurrPK;
                        folderBo.FolderName = HttpUtility.HtmlEncode(txtFolderName.Text.Trim());
                        folderBo.BizUnit = currentUser.SBUID;
                        folderBo.Module = (int)GTIService.Constants.Common.ApplicationModule.HRMS;
                        folderBo.ParentPk = this.ParentPK;
                        folderBo.UserPk = currentUser.PKUser;
                        folderBo.Active = (int)DbActiveStatus.ACTIVE;
                        folderBo.LastModDate = this.LastModifiedTime;
                        retObject = folderBo;
                        break;
                    case ControlEnums.USERS:
                        FolderMappingBo folderMappingBo = new FolderMappingBo();

                        folderMappingBo.Active = (int)DbActiveStatus.ACTIVE;
                        folderMappingBo.BizUnit = currentUser.SBUID;
                        folderMappingBo.FolderPk = this.CurrPK;
                        folderMappingBo.UserPk = currentUser.PKUser;
                        folderMappingBo.Detail = (List<FolderMappedUser>)SetUIValuesToObject(ControlEnums.MAPPINGS);

                        retObject = folderMappingBo;
                        break;
                    case ControlEnums.MAPPINGS:
                        List<FolderMappedUser> folderMappingList = new List<FolderMappedUser>();
                        FolderMappedUser folderMappedUser = null;

                        foreach (GridViewRow row in grdMappingUserList.Rows)
                        {
                            CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                            if (chk == null || !chk.Checked)
                                continue;

                            HiddenField hdfMappingPK = (HiddenField)row.FindControl("hdfMappingPK");
                            if (hdfMappingPK == null)
                                continue;

                            HiddenField hdfMappingUserPk = (HiddenField)row.FindControl("hdfMappingUserPk");
                            if (hdfMappingUserPk == null)
                                continue;

                            folderMappedUser = new FolderMappedUser();

                            folderMappedUser.MappingPk = string.IsNullOrWhiteSpace(hdfMappingPK.Value) ? 0 : Convert.ToInt32(hdfMappingPK.Value);
                            folderMappedUser.MappedUserPk = Convert.ToInt32(hdfMappingUserPk.Value);

                            folderMappingList.Add(folderMappedUser);
                        }

                        retObject = folderMappingList;
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

        #region Controls Enum
        enum ControlEnums
        {
            FOLDER,
            USERS,
            MAPPINGS
        }
        #endregion
    }
    #endregion

    #region Folder Browser DisplayModes
    public enum FolderBrowserDisplayMode
    {
        View,
        Expanded,
        Collapsed,
        MessageView,
        MappingView
    }
    #endregion
}