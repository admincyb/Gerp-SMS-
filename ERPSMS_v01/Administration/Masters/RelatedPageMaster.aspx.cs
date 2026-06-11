using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.Administration.Masters;
using GTIService.Constants.Common;
using GTIService;
using BusinessLogic.Administration.Masters;
using System.Xml;

namespace ERPSMS_v01.Administration.Masters
{
    public partial class RelatedPageMaster : System.Web.UI.Page
    {

        #region Variables and Properties

        BusinessObject.User currentUser = new BusinessObject.User();
        #region Properties

        /// <summary>
        /// Current PK (Primary Key of the current)
        /// </summary>
        private int CurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["CurrPK"]);
            }
            set
            {
                this.ViewState["CurrPK"] = value;
            }
        }
        #endregion

        private DataTable dtPages;
        private DataTable dtDataList;
        // Indicates the state as well as action
        private ActionsEnum commonActions;

        #endregion

        #region Set Page Variables
        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
            //Initialze the current logged in user to the currentUser variable
            currentUser = (BusinessObject.User)HttpContext.Current.User.Identity;
        }
        #endregion

        #region Page Level Events
        /// <summary>
        /// To handle pageload event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageVariables();
            if (!IsPostBack)
            {

                GetFieldValues(RelatedLinksBO.ControlsEnum.PageDropdown);
                SetFieldValues(RelatedLinksBO.ControlsEnum.PageDropdown);
                ddlPage.Focus();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            (Master.FindControl("ContentPlaceHolder1") as ContentPlaceHolder).EnableViewState = true;
        }

        #endregion

        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(RelatedLinksBO.ControlsEnum type)
        {

            switch (type)
            {
                case RelatedLinksBO.ControlsEnum.BindDataList:
                    dtDataList = RelatedLinkBL.GetRelatedPageList(CurrPK);//Gets the Related Pages to bind datalist
                    break;
                case RelatedLinksBO.ControlsEnum.PageDropdown:
                    dtPages = RelatedLinkBL.GetPages(CurrPK);//Gets the pages to bind dropdown
                    break;
                case RelatedLinksBO.ControlsEnum.Default:
                    dtDataList = RelatedLinkBL.GetRelatedPageList(CurrPK);//Gets the Related Pages to bind datalist
                    dtPages = RelatedLinkBL.GetPages(CurrPK);//Gets the pages to bind dropdown
                    break;
            }
        }

        #endregion

        #region Set Field Values

        /// <summary>
        /// All Field(Input controls) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(RelatedLinksBO.ControlsEnum type)
        {
            switch (type)
            {
                case RelatedLinksBO.ControlsEnum.PageDropdown:
                    BindDropDown(RelatedLinksBO.ControlsEnum.PageDropdown);
                    break;
                case RelatedLinksBO.ControlsEnum.BindDataList:
                    BindDatalist();
                    break;
                default: // if passed nothing or string.empty(), then Bind for Initail
                    BindDropDown(RelatedLinksBO.ControlsEnum.PageDropdown);
                    BindDatalist();
                    break;
            }
        }
        
        #endregion

        #region Action Handlers

        #region -- For Buttons ---

        /// <summary>
        /// For Button Click (Save/Delete)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (sender is ImageButton)
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
            }
            // Check Action From DropDown
            else if (sender is DropDownList)
            {
                commonActions = ActionsEnum.CHANGE;
            }
            switch (commonActions)
            {

                //Save related page details
                case ActionsEnum.SAVE:
                    if (!IsValid)//if page is not valid
                    {
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    }
                    else
                    {
                        //save related page details
                        SaveRelatedLinks();
                    }
                    break;
                case ActionsEnum.CANCEL:
                    ResetForm();
                    break;
                case ActionsEnum.CHANGE:
                    if (((DropDownList)sender).ID.ToLower() == "ddlpage")
                    {
                        if (ddlPage.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            CurrPK = Convert.ToInt32(ddlPage.SelectedValue);
                            GetFieldValues(RelatedLinksBO.ControlsEnum.BindDataList);
                            SetFieldValues(RelatedLinksBO.ControlsEnum.BindDataList);
                        }
                        else
                        {
                            dtDataList = null;
                            dlPages.DataSource = dtDataList;
                            dlPages.DataBind();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// item databound Event Handler for datalist 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Label lblGradeChecked = (Label)e.Item.FindControl("lblPageChecked");
                CheckBox chkPage = (CheckBox)e.Item.FindControl("chkPage");
                if (lblGradeChecked.Text == "1")
                {
                    chkPage.Checked = true;
                }
                else
                {
                    chkPage.Checked = false;
                }
            }
        }

        #endregion

        #endregion

        #region Helper Methods

        /// <summary>
        /// Method to fill Data to DropDown  
        /// </summary>
        private void BindDropDown(RelatedLinksBO.ControlsEnum drpName)
        {
            switch (drpName)
            {
                case RelatedLinksBO.ControlsEnum.PageDropdown:
                    ddlPage.ClearSelection();
                    ddlPage.Items.Clear();
                    if (dtPages != null && dtPages.Rows.Count > 0)
                    {

                        ddlPage.DataSource = dtPages;
                        ddlPage.DataTextField = GTIService.Constants.Administration.Masters.RelatedLink.Fields.PAGENAME;
                        ddlPage.DataValueField = GTIService.Constants.Administration.Masters.RelatedLink.Fields.PAGEID;
                        ddlPage.DataBind();
                        ddlPage.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    }
                    else
                    {
                        ddlPage.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///  Binds the  datalist( dsProductProperties) with data
        /// </summary>
        private void BindDatalist()
        {
            if (dtDataList != null && dtDataList.Rows.Count > 0)
            {
                dlPages.DataSource = dtDataList;
                dlPages.DataBind();
            }
        }

        /// <summary>
        /// Resets the form for a fresh entry
        /// </summary>
        private void ResetForm()
        {
            //Codes for Clearing the controls in the page
            commonActions = ActionsEnum.ADD_ACTION;
            CurrPK = 0;
            ddlPage.ClearSelection();
            dtDataList = null;
            dlPages.DataSource = dtDataList;
            dlPages.DataBind();
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns>objRelatedPages</returns>
        private RelatedLinksBO SetUIValuesToObject()
        {
            RelatedLinksBO objRelatedPages = new RelatedLinksBO();
            objRelatedPages.PageId = Convert.ToInt32(ddlPage.SelectedValue);
            objRelatedPages.Created_By = currentUser.PKUser;
            objRelatedPages.RelatedPages = new List<RelatedPage>();
            CheckBox chkPage;
            foreach (DataListItem item in dlPages.Items)
            {
                chkPage = (CheckBox)item.FindControl("chkPage");
                if (chkPage.Checked)
                {
                    RelatedPage objRelatedPage = new RelatedPage();
                    objRelatedPage.RelatedPagePK = Convert.ToInt32(dlPages.DataKeys[item.ItemIndex]);
                    objRelatedPages.RelatedPages.Add(objRelatedPage);
                }
            }
            return objRelatedPages;
        }

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>InnerXml</returns>
        private string SerializeAnObject(object obj)
        {

            System.Xml.XmlDocument doc;
            System.Xml.Serialization.XmlSerializer serializer;
            System.IO.MemoryStream stream;
            doc = new XmlDocument();
            serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            stream = new System.IO.MemoryStream();
            try
            {
                serializer.Serialize(stream, obj);
                stream.Position = 0;
                doc.Load(stream);
                return doc.InnerXml;
            }
            catch
            {
                return doc.InnerXml;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
        }

        /// <summary>
        /// To Save Product details
        /// </summary>
        private void SaveRelatedLinks()
        {
            int result = 0;
            RelatedLinksBO objRelatedPages = null;
            DbSaveStatus saveStatus;
            objRelatedPages = SetUIValuesToObject();
            string xmlProducts = SerializeAnObject(objRelatedPages);
            result = RelatedLinkBL.SaveRelatedLinks(xmlProducts);
            if (result > 0) // Success ! re-initialize the page
            {
                ResetForm();
                litErrorMsg.Text = Resources.ErrorMessages.Msg_SavSuccess;
                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetGlobalResourceObject("Controls", "RelatedLink").ToString());
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
            }
            else
            {
                saveStatus = (DbSaveStatus)result;
                switch (saveStatus)
                {

                    case DbSaveStatus.SQLERROR://SQl Error
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        break;
                    case DbSaveStatus.CODEEXIST://CODEEXIST
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_Code_Dupl").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        break;
                    case DbSaveStatus.CONCURRENCY://Cuncurrency Check
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Concurrent;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Related_Link").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        break;
                    case DbSaveStatus.ALREADYDELETED://Cuncurrency Check
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.GetLocalResourceObject("Related_Link").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        break;
                    default:
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        break;
                }
            }
        }

        #endregion

    }

    public enum ActionsEnum
    {
        CANCEL,
        SAVE,
        CHANGE,
        ADD_ACTION,
        VIEW,
        DETAILS,
        DELETE,
        EDIT,
        SUBMIT,
        WRKFSUBMIT,
        SHOWDETAILS,
        NEW,
        ACTIVATE,
        INACTIVATE,
        PETTYCASHACCOUNTSELECTED,
        REPORT,
        PARTY,
        PRINT,
        SEARCH,
        CLEAR,
        UNKNOWN,
        ITEMSELECTED,
        LIST,
        SEND,
        RESEND,
        ITEMSTOCK,
        TYPE,
        ADD,
        GETCURRENTSTOCK,
        EDIT_ACTION,
        DELETE_ACTION,
        SAVESUBMIT,
        TYPEFILTER, 
        ITEMNAME,
        DETAIL,
        ASSETDETAILS,
        DEPTCHANGED,
        LOCATION,
        MAPPING,
        EDITCOSTCENTER,
        REMOVECOSTCENTER,
        EXCEL,
        SHOWREPORT,
        SHOWREPORTINFILTER,
        REVISIONDELETE
    }

    public enum DbSaveStatus
    {
        REFERRED = 0,
        OLDCODEEXIST = 0,
        SAVED = 1,
        SQLERROR = -1,
        CODEEXIST = -2,
        CONCURRENCY = -3,
        DATEOVERLAP = -4,
        ALREADYDELETED = -5,
        NAMEEXIST = -30,
        ALREADYMAPPED=-40
    }

    public enum DbDeleteStatus
    {
        REFERRED = 0,
        DELETED = 1,
        SQLERROR = -1,
        CONCURRENCY = -3,
        DELETECONCURRENCY = -5,
        ALREADYMAPPED = -40
    }
}