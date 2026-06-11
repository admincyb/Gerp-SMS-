#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using System.Data;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject.HRMS.eDocs;
using BusinessLogic.HRMS.eDocs;
using BusinessObject.CommonManagement;
using HRMS.UserControls;
using ERPManager;
#endregion

namespace HRMS.EDocs
{
    public partial class EdocCreate : System.Web.UI.Page
    {
        #region Variables and Properties
        #region Properties

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
        /// Entry State for managing display status
        /// </summary>
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

        private string ProjectPk
        {
            get
            {
                int dummy = -1;
                return Request.QueryString[ERP.Utilities.QueryStrings.ProjID] == null || !Int32.TryParse(Request.QueryString[ERP.Utilities.QueryStrings.ProjID].Trim(), out dummy)
                    ? "-1"
                    : Request.QueryString[ERP.Utilities.QueryStrings.ProjID].Trim();
            }
        }

        private string UserActionLog
        {
            get
            {
                return this.ViewState[ViewstateStrings.UserActionLog] == null 
                    ? string.Empty
                    : this.ViewState[ViewstateStrings.UserActionLog].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.UserActionLog] = value;
            }
        }

        #endregion

        #region Variables
        BusinessObject.User currentUser;
        private ActionsEnum commonActions;
        private EDocBO selectedDoc = null;
        RadioButton rbtn;
        DataTable dtPageData;
        List<DDLMaster> chkUsers; 
        int? dummyPk;
        string @xml = string.Empty;
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
            ucrUploader.Click += new UserControls.FileUploaderNew.MultipleFileUploadClick(ucrUploader_Click);
            ucrUploader.AllFilesDeleted += new EventHandler(ucrUploader_AllFilesDeleted);
            ucrUploader.AfterDelete += new FileUploaderNew.AfterFileDelete(ucrUploader_AfterDelete);
            ucrUploader.ViewType = 1;            
            PageActionHandler();
        }

        void ucrUploader_AfterDelete(object sender, FileEventArgs e)
        {
            UserActionLog += string.Format(@"{0} Deleted<br/>", e.FileName);
        }

        void ucrUploader_AllFilesDeleted(object sender, EventArgs e)
        {
            ddlProjectSite.Enabled = true;
        } 

        void ucrUploader_Click(object sender, UserControls.FileCollectionEventArgs e)
        {
            if (ddlProjectSite.SelectedValue != null && ddlProjectSite.SelectedValue != CommonConstants.SELECTVAL)
            {
                ucrUploader.SubFolderPath = "EDocs/" + ddlProjectSite.SelectedValue + "/";
                ucrUploader.UploadFile(e);
                ddlProjectSite.Enabled = false;
                if (e.Count > 0)
                {
                    for (int i = 0; i < e.PostedFiles.Count; i++)
                    {
                        UserActionLog += string.Format("{0} Added<br/>", e.PostedFiles[i].FileName);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_ProjectNotSelected").ToString() + "','" + Resources.Messages.Information + "');", true);
            }
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
                if (!IsPostBack)
                {
                    //txtDate.Focus();
                    //Session[ERP.Utilities.SessionStrings.EDocPk] = 5;
                    FileExplorer1.DisplayMode = FolderBrowserDisplayMode.Collapsed;
                    FileExplorer1.ShowComandBar = true;
                    FileExplorer1.ShowMaximumCharacters = Convert.ToInt32(GetLocalResourceObject("MaximumCharacrers").ToString());
                    if (Session[ERP.Utilities.SessionStrings.EDocPk] != null)
                    {
                        this.CurrPK = Convert.ToInt64(Session[ERP.Utilities.SessionStrings.EDocPk]);
                        Session[ERP.Utilities.SessionStrings.EDocPk] = null; //Reset Session 
                        GetFieldValues(ControlsEnum.SELECTEDOCDETAILS);
                        SetFieldValues(ControlsEnum.SELECTEDOCDETAILS);
                    }
                    else
                    {
                        txtEDocNo.Text = "[NEW]";
                        UserActionLog = string.Empty;
                        GetFieldValues(ControlsEnum.PROJECT);
                        SetFieldValues(ControlsEnum.PROJECT);

                        GetFieldValues(ControlsEnum.FROM);
                        SetFieldValues(ControlsEnum.FROM);

                        GetFieldValues(ControlsEnum.TO);
                        SetFieldValues(ControlsEnum.TO);

                        GetFieldValues(ControlsEnum.DEPARTMENT);
                        SetFieldValues(ControlsEnum.DEPARTMENT);

                        GetFieldValues(ControlsEnum.FOLDER);
                        SetFieldValues(ControlsEnum.FOLDER);


                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    case ControlsEnum.PROJECT:
                        if (this.CurrPK > 0 && this.selectedDoc != null)
                        {
                            dummyPk = this.selectedDoc.DCH_SITE;
                        }
                        else
                        {
                            dummyPk = null;
                        }
                        dtPageData = EDocManagementBL.GetProjectsOrSite(dummyPk, (int)DbActiveStatus.ACTIVE);
                        break;

                    case ControlsEnum.USERS:
                        dtPageData = EDocManagementBL.GetSendToUsers(currentUser.PKUser, (int)DbActiveStatus.ACTIVE);
                        chkUsers = dtPageData.AsEnumerable().Select(row => new DDLMaster
                        {
                            PK = row.Field<int?>(GTIService.Constants.HRMS.eDocs.Fields.usrPk).GetValueOrDefault(),
                            Value = row.Field<string>(GTIService.Constants.HRMS.eDocs.Fields.usrEmployeeText),
                        }).ToList();

                        break;
                    case ControlsEnum.SELECTEDOCDETAILS:
                        this.selectedDoc = EDocManagementBL.GetEDocDetailsByID(this.CurrPK, currentUser.PKUser);
                        break;
                    case ControlsEnum.FROM:
                        if (this.CurrPK > 0 && this.selectedDoc != null && !string.IsNullOrWhiteSpace(this.selectedDoc.DCH_LETTER_FROM))
                        {
                            dummyPk = Convert.ToInt32(this.selectedDoc.DCH_LETTER_FROM.Trim());
                        }
                        else
                        {
                            dummyPk = 0;
                        }
                        dtPageData = EDocManagementBL.GetFromTo(dummyPk.Value, 0, ConstGroupType.EDocSetUp, 1, (int)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    case ControlsEnum.TO:
                        if (this.CurrPK > 0 && this.selectedDoc != null && !string.IsNullOrWhiteSpace(this.selectedDoc.DCH_LETTER_TO))
                        {
                            dummyPk = Convert.ToInt32(this.selectedDoc.DCH_LETTER_TO.Trim());
                        }
                        else
                        {
                            dummyPk = 0;
                        }
                        dtPageData = EDocManagementBL.GetFromTo(dummyPk.Value, 0, ConstGroupType.EDocSetUp, 1, (int)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    case ControlsEnum.DEPARTMENT:
                        if (this.CurrPK > 0 && this.selectedDoc != null && !string.IsNullOrWhiteSpace(this.selectedDoc.DCH_TRX_DEPT))
                        {
                            dummyPk = Convert.ToInt32(this.selectedDoc.DCH_TRX_DEPT.Trim());
                        }
                        else
                        {
                            dummyPk = 0;
                        }
                        dtPageData = EDocManagementBL.GetFromTo(dummyPk.Value, 0, ConstGroupType.EDocSetUp, 2, (int)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    case ControlsEnum.FOLDER:
                        @xml = EDocManagementBL.GetFolderTreeXml(0, currentUser.SBUID);
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
                    case ControlsEnum.PROJECT:
                        BindDropDown(ControlsEnum.PROJECT);
                        break;
                    case ControlsEnum.USERS:
                        BindCheckListSearchControl(ControlsEnum.USERS);
                        break;
                    case ControlsEnum.SELECTEDOCDETAILS:
                        GetUIValuesFromObject(ControlsEnum.SELECTEDOCDETAILS);
                        break;
                    case ControlsEnum.FROM:
                        BindDropDown(ControlsEnum.FROM);
                        break;
                    case ControlsEnum.TO:
                        BindDropDown(ControlsEnum.TO);
                        break;
                    case ControlsEnum.DEPARTMENT:
                        BindDropDown(ControlsEnum.DEPARTMENT);
                        break;
                    case ControlsEnum.FOLDER:
                        FileExplorer1.SetDataSource(@xml);
                        FileExplorer1.DataBind();
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

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.PROJECT:
                    ddlProjectSite.Items.Clear();
                    ddlProjectSite.DataSource = dtPageData;
                    ddlProjectSite.DataTextField = GTIService.Constants.HRMS.eDocs.Fields.prjName;
                    ddlProjectSite.DataValueField = GTIService.Constants.HRMS.eDocs.Fields.PprjPK;
                    ddlProjectSite.DataBind();
                    ddlProjectSite.Items.HtmlDecode();
                    ddlProjectSite.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                    ddlProjectSite.SelectedValue = this.ProjectPk;
                    break;

                //case ControlsEnum.USERS:
                    //ddlSendTo.Items.Clear();
                    //ddlSendTo.DataSource = dtPageData;
                    //ddlSendTo.DataTextField = GTIService.Constants.HRMS.eDocs.Fields.usrEmployeeText;
                    //ddlSendTo.DataValueField = GTIService.Constants.HRMS.eDocs.Fields.usrPk;
                    //ddlSendTo.DataBind();
                    //ddlSendTo.Items.HtmlDecode();
                    //ddlSendTo.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                    break;
                case ControlsEnum.FROM:
                    ddlFrom.Items.Clear();
                    ddlFrom.DataSource = dtPageData;
                    ddlFrom.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                    ddlFrom.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                    ddlFrom.DataBind();
                    ddlFrom.Items.HtmlDecode();
                    ddlFrom.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                    break;
                case ControlsEnum.TO:
                    ddlTo.Items.Clear();
                    ddlTo.DataSource = dtPageData;
                    ddlTo.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                    ddlTo.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                    ddlTo.DataBind();
                    ddlTo.Items.HtmlDecode();
                    ddlTo.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                    break;
                case ControlsEnum.DEPARTMENT:
                    ddlDepartment.Items.Clear();
                    ddlDepartment.DataSource = dtPageData;
                    ddlDepartment.DataTextField = GTIService.Constants.Common.Common.CON_DATA_TEXT_FIELD;
                    ddlDepartment.DataValueField = GTIService.Constants.Common.Common.CON_PK_FIELD;
                    ddlDepartment.DataBind();
                    ddlDepartment.Items.HtmlDecode();
                    ddlDepartment.Items.Insert(0, new ListItem { Text = CommonConstants.SELECTTEXT, Value = CommonConstants.SELECTVAL });
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private object SetUIValuesToObject(ControlsEnum controlType)
        {

            try
            {
                Object retObject;
                retObject = null;
                switch (controlType)
                {
                    case ControlsEnum.SELECTEDOCDETAILS:
                        EDocBO eDocBO = new EDocBO();

                        eDocBO.DCH_PK = this.CurrPK;
                        eDocBO.DCH_NO = txtEDocNo.Text.Trim();
                        eDocBO.DCH_DATE = txtDate.Text;
                        eDocBO.DCH_LETTER_NO = txtLetterNo.Text.HtmlEncode();

                        if (ddlFrom.SelectedValue != null && ddlFrom.SelectedValue != CommonConstants.SELECTVAL)
                            eDocBO.DCH_LETTER_FROM = ddlFrom.SelectedValue;

                        if (ddlTo.SelectedValue != null && ddlTo.SelectedValue != CommonConstants.SELECTVAL)
                            eDocBO.DCH_LETTER_TO = ddlTo.SelectedValue;

                        if (ddlDepartment.SelectedValue != null && ddlDepartment.SelectedValue != CommonConstants.SELECTVAL)
                            eDocBO.DCH_TRX_DEPT = ddlDepartment.SelectedValue;


                        eDocBO.DCH_SITE = Convert.ToInt32(ddlProjectSite.SelectedValue);
                        eDocBO.DCH_SUBJECT = txtSubject.Text.HtmlEncode();
                        eDocBO.DCH_TAGS = txtTags.Text.HtmlEncode();
                        eDocBO.DCT_COMMENT = txtComments.Text.Replace("\n", "<br />").HtmlEncode();
                        eDocBO.DCT_COMMENT_TYPE = chkInPrivate.Checked ? (int)CommentType.Private : (int)CommentType.Normal;
                        if (!string.IsNullOrWhiteSpace(UserActionLog))
                            eDocBO.DCT_DESC = UserActionLog.TrimEnd("<br/>".ToCharArray());

                        if (FileExplorer1.SelectedPk > 0)
                            eDocBO.DCH_FOLDER = FileExplorer1.SelectedPk.ToString();

                        List<EDocSendToBO> SendToDetails = new List<EDocSendToBO>();
                        foreach (ListItem item in CheckListSearchControl1.GetCheckedItems())
                        {
                            if (item.Value != hdfSendTo.Value)
                                SendToDetails.Add(new EDocSendToBO { DCT_TO_USER_CC = Convert.ToInt32(item.Value) });
                        }
                        eDocBO.EDocSendUsers = SendToDetails;

                        //---
                        //eDocBO.DCT_TO_USER = 0;
                        //eDocBO.DCH_TRX_STATUS = "";

                        eDocBO.Detail = (List<EDocDetailBO>)SetUIValuesToObject(ControlsEnum.DETAILS);

                        eDocBO.DCH_DEPT = currentUser.CurrentDeptPK;
                        eDocBO.BIZUNIT_PK = currentUser.SBUID;
                        eDocBO.USER_PK = currentUser.PKUser;
                        eDocBO.LAST_MOD_DT = this.LastModifiedTime;
                        eDocBO.ACTIVE = (int)DbActiveStatus.ACTIVE;

                        retObject = eDocBO;
                        break;
                    case ControlsEnum.DETAILS:
                        List<EDocDetailBO> fileDetails = new List<EDocDetailBO>();

                        var attachments = ucrUploader.GetAttachedFiles();
                        if (attachments != null)
                        {
                            foreach (var item in attachments)
                            {
                                fileDetails.Add(new EDocDetailBO
                                {
                                    DCD_FILE = item.FileName,
                                    DCD_FILE_PATH = item.FilePath,
                                    DCD_PK = item.PK,
                                    DCD_SEQUENCE = item.DocumentNo,
                                    DCD_TITLE = item.Title,
                                    DCD_DESC = item.FileDescription
                                });
                            }
                        }

                        retObject = fileDetails;
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

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {

                switch (controlType)
                {
                    case ControlsEnum.SELECTEDOCDETAILS:
                        if (this.selectedDoc != null)
                        {
                            if (selectedDoc.DCH_IS_EDIT == 1)
                            {
                                //Enable All Bttons
                                btnSave.Visible = true;
                                btnFinalize.Visible = true;
                                btnForward.Visible = true;
                                ucrUploader.ViewType = 1;
                            }
                            else
                            {
                                //Disable Save,Finalize,Forward buttons
                                btnSave.Visible = false;
                                btnFinalize.Visible = false;
                                btnForward.Visible = false;
                                ucrUploader.ViewType = 0;

                            }
                            //Enable Or Disable Upload functionality based on viewType
                            ucrUploader.ViewAction();

                            txtEDocNo.Text = this.selectedDoc.DCH_NO.HtmlDecode();
                            txtDate.Text = this.selectedDoc.DCH_DATE;
                            txtLetterNo.Text = this.selectedDoc.DCH_LETTER_NO.HtmlDecode();
                            txtSubject.Text = this.selectedDoc.DCH_SUBJECT.HtmlDecode();
                            txtTags.Text = this.selectedDoc.DCH_TAGS.HtmlDecode();
                            //txtComments.Text = this.selectedDoc.DCT_COMMENT.HtmlDecode();

                            GetFieldValues(ControlsEnum.PROJECT);
                            SetFieldValues(ControlsEnum.PROJECT);
                            ddlProjectSite.SelectedValue = this.selectedDoc.DCH_SITE.ToString();
                            ddlProjectSite.Enabled = false;


                            GetFieldValues(ControlsEnum.FROM);
                            SetFieldValues(ControlsEnum.FROM);
                            if (!string.IsNullOrWhiteSpace(this.selectedDoc.DCH_LETTER_FROM))
                                ddlFrom.SelectedValue = this.selectedDoc.DCH_LETTER_FROM;

                            GetFieldValues(ControlsEnum.TO);
                            SetFieldValues(ControlsEnum.TO);
                            if (!string.IsNullOrWhiteSpace(this.selectedDoc.DCH_LETTER_FROM))
                                ddlTo.SelectedValue = this.selectedDoc.DCH_LETTER_TO;

                            GetFieldValues(ControlsEnum.DEPARTMENT);
                            SetFieldValues(ControlsEnum.DEPARTMENT);
                            if (!string.IsNullOrWhiteSpace(this.selectedDoc.DCH_TRX_DEPT))
                                ddlDepartment.SelectedValue = this.selectedDoc.DCH_TRX_DEPT;


                            hdrDocHistory.Visible = true;
                            GetUIValuesFromObject(ControlsEnum.DETAILS);
                            GetUIValuesFromObject(ControlsEnum.HISTORY);

                            GetFieldValues(ControlsEnum.FOLDER);
                            SetFieldValues(ControlsEnum.FOLDER);

                            if (string.IsNullOrWhiteSpace(selectedDoc.DCH_FOLDER))
                                FileExplorer1.ClearSelection();
                            else
                                FileExplorer1.SelectedPk =Convert.ToInt64(selectedDoc.DCH_FOLDER);
                             
                            this.LastModifiedTime = this.selectedDoc.LAST_MOD_DT;
                            ModifiedDatePnl.Visible = true;
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                        }
                        else if (this.CurrPK > 0)
                        {
                            // Something wrong
                        }
                        break;
                    case ControlsEnum.DETAILS:
                        List<AttachmentBO> attachments = new List<AttachmentBO>();
                        if (this.selectedDoc != null)
                        {
                            foreach (var file in selectedDoc.Detail)
                            {
                                attachments.Add(new AttachmentBO
                                {
                                    PK = file.DCD_PK,
                                    DocumentNo = file.DCD_SEQUENCE,
                                    FileName = file.DCD_FILE,
                                    FilePath = file.DCD_FILE_PATH,
                                    Title = file.DCD_TITLE,
                                    FileDescription = file.DCD_DESC
                                });
                            }
                        }

                        ucrUploader.SubFolderPath = "EDocs/" + ddlProjectSite.SelectedValue + "/";
                        ucrUploader.SetAttachedFiles(attachments);
                        break;
                    case ControlsEnum.HISTORY:
                        if (this.selectedDoc != null)
                        {
                            rptComments.DataSource = selectedDoc.Comments;
                            rptComments.DataBind();
                        }
                        else
                        {
                            rptComments.DataSource = null;
                            rptComments.DataBind();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {

                if (Mode == ActionsEnum.VIEW)
                {
                    EntryStatus = EntryStatus.VIEWMODE;
                }
                else
                {
                    EntryStatus = EntryStatus.ENTRYMODE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ActionsEnum type)
        {
            //CurrPK = 0;
            switch (type)
            {
                case ActionsEnum.SAVE:
                    UserActionLog = null;
                    break;
                case ActionsEnum.SENDTO:
                    UserActionLog = null;
                    break;
                case ActionsEnum.FINALIZE:
                    UserActionLog = null;
                    break;

            }
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        private void BindCheckListSearchControl(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.USERS:

                        if (chkUsers != null && chkUsers.Count > 0)
                        {
                            CheckListSearchControl1.ListData = CommonFunctions.HtmlDecode(chkUsers, "Value");
                            CheckListSearchControl1.BindData();
                        }
                        else
                        {
                            CheckListSearchControl1.ClearData();
                        }
                        //chkSendToUser.Items.Clear();
                        //if (dtPageData != null && dtPageData.Rows.Count > 0)
                        //{

                        //    CheckListSearchControl1.ListData = CommonFunctions.HtmlDecode(chkUsers, "Value");
                        //    CheckListSearchControl1.BindData();

                        //    //chkSendToUser.DataSource = dtPageData;
                        //    //chkSendToUser.DataTextField = GTIService.Constants.HRMS.eDocs.Fields.usrEmployeeText;
                        //    //chkSendToUser.DataValueField = GTIService.Constants.HRMS.eDocs.Fields.usrPk;
                        //    //chkSendToUser.DataBind();
                        //}
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                string arg;
                bool bIsChecked = false;
                int result;
                string action;
                int selectedItemPK;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(FileUploaderNew)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((FileUploaderNew)sender).CommandName));
                }
                else if (sender.GetType().BaseType.Name == "GtiFolderExplorer")
                {
                    commonActions = ActionsEnum.REFRESH;
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
                            this.selectedDoc = (EDocBO)SetUIValuesToObject(ControlsEnum.SELECTEDOCDETAILS);
                            this.selectedDoc.DCH_TRX_STATUS = (int)TrxStatus.Drafted;
                            result = EDocManagementBL.Save(selectedDoc);
                            if (result > 0)
                            {
                                ResetForm(ActionsEnum.SAVE);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EDocCreate);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" + Resources.PageURL.EDocListUrl + "');", true);
                                //EntryStatus = EntryStatus.LISTMODE;
                                //ResetForm(ActionsEnum.SAVE);
                                //Response.Redirect(Resources.PageURL.EDocListUrl);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EDocCreate + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EDocCreate + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    //ResetForm(ActionsEnum.SAVE);
                                    //GetFieldValues(ControlsEnum .DEPRETRANLIST);
                                    //SetFieldValues(ControlEnums.DEPRETRANLIST);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EDocCreate + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EDocCreate);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Forward
                    case ActionsEnum.FORWARD:
                        GetFieldValues(ControlsEnum.USERS);
                        SetFieldValues(ControlsEnum.USERS);

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ClosePopup();ShowContainerDiv('[id$=divSendToUserPopupContainer]','" + Resources.Captions.SendForward + "','550','350');", true);
                        break;
                    #endregion
                    #region Send To
                    case ActionsEnum.SENDTO:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            this.selectedDoc = (EDocBO)SetUIValuesToObject(ControlsEnum.SELECTEDOCDETAILS);
                            this.selectedDoc.DCH_TRX_STATUS = (int)TrxStatus.Pending;
                            this.selectedDoc.DCT_TO_USER = Convert.ToInt32(hdfSendTo.Value);
                            string forwardedToUser = txtSendTo.Text;

                            result = EDocManagementBL.Save(selectedDoc);
                            if (result > 0)
                            {
                                ResetForm(ActionsEnum.SENDTO);
                                litErrorMsg.Text = Resources.Messages.EDoc_Forward_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, forwardedToUser);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" + Resources.PageURL.EDocListUrl + "');", true);

                                //EntryStatus = EntryStatus.LISTMODE;
                                //ResetForm(ActionsEnum.SAVE);
                                //Response.Redirect(Resources.PageURL.EDocListUrl);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EDocCreate + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EDocCreate + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    //ResetForm(ActionsEnum.SAVE);
                                    //GetFieldValues(ControlsEnum .DEPRETRANLIST);
                                    //SetFieldValues(ControlEnums.DEPRETRANLIST);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EDocCreate + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EDocCreate);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Finalize
                    case ActionsEnum.FINALIZE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            this.selectedDoc = (EDocBO)SetUIValuesToObject(ControlsEnum.SELECTEDOCDETAILS);
                            this.selectedDoc.DCH_TRX_STATUS = (int)TrxStatus.Completed;

                            result = EDocManagementBL.Save(selectedDoc);
                            if (result > 0)
                            {
                                ResetForm(ActionsEnum.FINALIZE);
                                litErrorMsg.Text = Resources.Messages.EDoc_Finalized;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" + Resources.PageURL.EDocListUrl + "');", true);
                                //Response.Redirect(Resources.PageURL.EDocListUrl);
                                //EntryStatus = EntryStatus.LISTMODE;
                                //ResetForm(ActionsEnum.SAVE);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EDocCreate + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EDocCreate + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    //ResetForm(ActionsEnum.SAVE);
                                    //GetFieldValues(ControlsEnum .DEPRETRANLIST);
                                    //SetFieldValues(ControlEnums.DEPRETRANLIST);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EDocCreate + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EDocCreate);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Cancel
                    // Do Action for , when click cancel button
                    case ActionsEnum.CANCEL:
                        //EntryStatus = EntryStatus.LISTMODE;
                        Response.Redirect(Resources.PageURL.EDocListUrl);
                        break;
                    #endregion
                    #region Refresh
                    case ActionsEnum.REFRESH:
                        GetFieldValues(ControlsEnum.FOLDER);
                        SetFieldValues(ControlsEnum.FOLDER);
                        FileExplorer1.SelectedPk = Convert.ToInt64(sender.ToString());
                        //string errorMessage = Resources.ErrorMessages.Msg_Save_Success;
                        //errorMessage = string.Format(errorMessage, Resources.PageNameRes.FolderExplorer);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + errorMessage + "','" + Resources.Messages.Information + "');", true);
                        break; 
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {

            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
        }


        #endregion
        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            FileExplorer1.FolderAdded += ActionHandler;
            FileExplorer1.FolderEdited += ActionHandler;
            FileExplorer1.FolderDeleted += ActionHandler;
        }


        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
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

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);

                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){initComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            PROJECT,
            USERS,
            SELECTEDOCDETAILS,
            DETAILS,
            HISTORY,
            FROM,
            TO,
            DEPARTMENT,
            FOLDER
        }
        #endregion
    }
}