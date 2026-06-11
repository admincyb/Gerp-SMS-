using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using System.Data;
using BusinessLogic.Inventory;
using BusinessObject.CommonManagement;
using BusinessObject.Inventory;
using System.IO;
using ERPSMS_v01.UserControls;
using BusinessLogic.AccountManagement;
using BusinessObject;

namespace ERPSMS_v01.Inventory
{
    public partial class VendorItemDoc : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Properties
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
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndex
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }

        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }

        /// <summary>
        /// To maintain the SortExpression or sort By in viewstate
        /// </summary>
        private string SortBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortBy] = value;
            }
        }

        /// <summary>
        /// To maintain the Sort Direction in viewstate
        /// </summary>
        private string SortDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortDirection] = value;
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


        private VendorItemDocBO TempVendorItemDocBO
        {
            get
            {
                return (VendorItemDocBO)ViewState["TempVendorItemDocBO"] == null ? new VendorItemDocBO() : (VendorItemDocBO)ViewState["TempVendorItemDocBO"];
            }
            set
            {
                ViewState["TempVendorItemDocBO"] = value;
            }
        }


        private List<VendorFileDetails> VendorFileDetailsList
        {
            get
            {
                return Session["VendorFileDetailsList"] == null ? null : (List<VendorFileDetails>)Session["VendorFileDetailsList"];
            }
            set
            {
                Session["VendorFileDetailsList"] = value;
            }
        }


        /// <summary>
        /// To maintain the edit index in viewstate
        /// </summary>
        private int EditIndex
        {
            get
            {
                return (int)this.ViewState["EditIndex"];
            }
            set
            {
                this.ViewState["EditIndex"] = value;
            }
        }
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
            }
        }

        #endregion
        private ActionsEnum commonActions;
        private DataTable dtPage;

        private BusinessObject.User currentUser;

        #endregion

        #region PageLevel Events
        /// <summary>
        /// page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            PageActionHandler();
        }
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_InitializeComponents", "InitComponents();", true);
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
                    GetUserRights();
                    btnSave.Visible = btnAddItem.Visible = Convert.ToBoolean(Convert.ToInt16(hdfIsEditPermission.Value));
                    GetFieldValues(ControlsEnum.GETCATEGORY);
                    SetFieldValues(ControlsEnum.GETCATEGORY);
                    GetFieldValues(ControlsEnum.DOCSTYPE);
                    SetFieldValues(ControlsEnum.DOCSTYPE);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
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
                GridViewRow gvrItem;
                string saveXml;
                int result;
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
                switch (commonActions)
                {
                    #region Serach
                    case ActionsEnum.SEARCH:
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion

                    #region Popup
                    case ActionsEnum.POPUPADD:
                        ClearpopUp();
                        ddlCategory.Focus();
                        VendorFileDetailsList = null;
                        gvrItem = ((ImageButton)sender).Parent.Parent as GridViewRow;

                        lblVendorDisplay.Text = CommonFunctions.GetShortString(((Label)grdList.Rows[gvrItem.RowIndex].FindControl("lblVendor")).Text, 32);
                        lblItemCodeDisplay.Text = CommonFunctions.GetShortString(((Label)grdList.Rows[gvrItem.RowIndex].FindControl("lblItem")).Text, 32);

                        lblVendorDisplay.ToolTip = ((Label)grdList.Rows[gvrItem.RowIndex].FindControl("lblVendor")).Text;
                        lblItemCodeDisplay.ToolTip = ((Label)grdList.Rows[gvrItem.RowIndex].FindControl("lblItem")).Text;
                        hdfMapPK.Value = ((HiddenField)grdList.Rows[gvrItem.RowIndex].FindControl("hdfMapPK")).Value;

                        GetFieldValues(ControlsEnum.DOCSEDIT);
                        SetFieldValues(ControlsEnum.DOCSEDIT);
                        ShowPopUp();
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ClearAll();
                        break;
                    #endregion

                    #region AddItem
                    case ActionsEnum.ADDITEM:

                        if (fupUpload.HasFile)
                        {
                            FileInfo tempFileInfoObjValid;
                            tempFileInfoObjValid = new FileInfo(fupUpload.PostedFile.FileName);
                            if (!IsValidExtension(tempFileInfoObjValid.Extension))
                            {
                                ShowPopUp();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Valid_File) + "','" + Resources.ErpRes.Information + "');", true);
                                break;
                            }
                        }

                        if (EditIndex < 0)
                        {
                            VendorDetailsBO objDetail = new VendorDetailsBO();

                            VendorItemDocBO tempList = new VendorItemDocBO();
                            tempList = TempVendorItemDocBO;

                            if (tempList.DetailList == null)
                                tempList.DetailList = new List<VendorDetailsBO>();
                            List<VendorDetailsBO> ObjList = tempList.DetailList;

                            objDetail.IVD_ACTIVE = Convert.ToInt16(chkActive.Checked);
                            objDetail.IVD_DESC = txtRemarks.Text;
                            objDetail.IVD_DOC_CATEGORY = ddlCategory.SelectedValue;
                            objDetail.IVD_DOC_CATEGORY_TEXT = ddlCategory.SelectedItem.Text;
                            objDetail.IVD_DOC_TITLE = txtTitle.Text;
                            objDetail.IVD_VERSION = txtVersion.Text;


                            int CurrSlNo = 1;
                            if (tempList.DetailList == null || tempList.DetailList.Count == 0)
                            {
                                CurrSlNo = 1;
                            }
                            else
                            {
                                CurrSlNo = tempList.DetailList.Max(itm => itm.ListSlNo);
                                CurrSlNo++;
                            }

                            objDetail.ListSlNo = CurrSlNo;

                            if (fupUpload.HasFile)
                            {
                                FileInfo tempFileInfoObj;
                                tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                string attachmentFileFormat = tempFileInfoObj.Extension;
                                string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                objDetail.AttachmentFileName = attachmentFileName;
                                objDetail.IVD_DOC_TYPE = tempFileInfoObj.Extension;
                                objDetail.IVD_DOC_NAME = fupUpload.FileName;
                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                {
                                    objDetail.IVD_DOC_PATH = "~/Upload/" + attachmentFileName;
                                }
                                else
                                {
                                    objDetail.IVD_DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                }

                                if (VendorFileDetailsList == null)
                                {
                                    VendorFileDetailsList = new List<VendorFileDetails>();
                                }

                                VendorFileDetails fileDetailsObj = VendorFileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                if (fileDetailsObj == null)
                                {
                                    VendorFileDetailsList.Add(new VendorFileDetails() { SlNo = CurrSlNo, VendorFile = HttpContext.Current.Request.Files[0] });
                                }
                                else
                                {
                                    fileDetailsObj.VendorFile = HttpContext.Current.Request.Files[0];
                                }

                            }

                            ObjList.Add(objDetail);

                            tempList.DetailList = ObjList;

                            TempVendorItemDocBO = tempList;
                        }
                        else
                        {
                            TempVendorItemDocBO.DetailList[EditIndex].IVD_ACTIVE = Convert.ToInt16(chkActive.Checked);
                            TempVendorItemDocBO.DetailList[EditIndex].IVD_DESC = txtRemarks.Text;
                            TempVendorItemDocBO.DetailList[EditIndex].IVD_DOC_CATEGORY = ddlCategory.SelectedValue;
                            TempVendorItemDocBO.DetailList[EditIndex].IVD_DOC_CATEGORY_TEXT = ddlCategory.SelectedItem.Text;
                            TempVendorItemDocBO.DetailList[EditIndex].IVD_DOC_TITLE = txtTitle.Text;
                            TempVendorItemDocBO.DetailList[EditIndex].IVD_VERSION = txtVersion.Text;

                            if (fupUpload.HasFile)
                            {
                                FileInfo tempFileInfoObj;
                                tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                string attachmentFileFormat = tempFileInfoObj.Extension;
                                string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                TempVendorItemDocBO.DetailList[EditIndex].AttachmentFileName = attachmentFileName;
                                TempVendorItemDocBO.DetailList[EditIndex].IVD_DOC_TYPE = tempFileInfoObj.Extension;
                                TempVendorItemDocBO.DetailList[EditIndex].IVD_DOC_NAME = fupUpload.FileName;
                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                {
                                    TempVendorItemDocBO.DetailList[EditIndex].IVD_DOC_PATH = "~/Upload/" + attachmentFileName;
                                }
                                else
                                {
                                    TempVendorItemDocBO.DetailList[EditIndex].IVD_DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                }

                                if (VendorFileDetailsList == null)
                                {
                                    VendorFileDetailsList = new List<VendorFileDetails>();
                                }

                                VendorFileDetails fileDetailsObj = VendorFileDetailsList.SingleOrDefault(aa => aa.SlNo == TempVendorItemDocBO.DetailList[EditIndex].ListSlNo);
                                if (fileDetailsObj == null)
                                {
                                    VendorFileDetailsList.Add(new VendorFileDetails() { SlNo = TempVendorItemDocBO.DetailList[EditIndex].ListSlNo, VendorFile = HttpContext.Current.Request.Files[0] });
                                }
                                else
                                {
                                    fileDetailsObj.VendorFile = HttpContext.Current.Request.Files[0];
                                }
                            }

                        }
                        SetFieldValues(ControlsEnum.DOCS);
                        ddlCategory.Focus();
                        ClearpopUp();
                        ShowPopUp();
                        break;
                    #endregion

                    #region Save
                    case ActionsEnum.SAVE:
                        //if (grdDocs.Rows.Count > 0)
                        //{ 
                        TempVendorItemDocBO.ITV_PK = hdfMapPK.Value;
                        TempVendorItemDocBO.BIZUNIT_PK = currentUser.CurrentSBUPK;
                        TempVendorItemDocBO.USER_PK = currentUser.PKUser;

                        saveXml = CommonFunctions.XmlSerialize<VendorItemDocBO>(TempVendorItemDocBO);
                        result = VendorItemDocBL.SaveVendorDocs(saveXml);

                        if (result > 0) // Success ! re-initialize the page
                        {
                            string savePath = string.Empty;
                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                            {
                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                if (!Directory.Exists(savePath))
                                    Directory.CreateDirectory(savePath);
                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                            }
                            else
                            {
                                savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                            }

                            foreach (VendorDetailsBO obj in TempVendorItemDocBO.DetailList)
                            {
                                string filePath = savePath + obj.AttachmentFileName;
                                FileInfo attachedFileInfo = new FileInfo(filePath);
                                if (VendorFileDetailsList != null)
                                {
                                    VendorFileDetails fileDetailsObj = VendorFileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.ListSlNo);
                                    if (fileDetailsObj != null)
                                    {
                                        fileDetailsObj.VendorFile.SaveAs(attachedFileInfo.FullName);

                                    }
                                }
                            }
                            VendorFileDetailsList = null;
                            litErrorMsg.Text = GetLocalResourceObject("VendorDocs").ToString() + " " + Resources.Messages.SavedSuccessfully;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            switch (Convert.ToInt32(result))
                            {
                                case (int)DbSaveStatus.SQLERROR://SQL Error 
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                    break;
                                case (int)DbSaveStatus.CONCURRENCY:
                                    litErrorMsg.Text = GetLocalResourceObject("VendorDocs").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                    break;
                                case (int)DbSaveStatus.ALREADYDELETED:
                                    litErrorMsg.Text = GetLocalResourceObject("VendorDocs").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                                    break;
                            }
                        }
                        //}
                        //else
                        //{
                        //    litErrorMsg.Text = GetLocalResourceObject("Err_AddDocs").ToString();
                        //    ShowPopUp();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        //        + "','" + Resources.Captions.Information + "');", true);
                        //}
                        break;
                    #endregion

                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        if (TempVendorItemDocBO != null && TempVendorItemDocBO.DetailList != null && TempVendorItemDocBO.DetailList.Count > 0)
                        {

                            gvrItem = ((Button)sender).Parent.Parent as GridViewRow;

                            EditIndex = gvrItem.RowIndex;

                            GetUIValuesFromObject(ControlsEnum.DOCDTLSEDIT);

                            ShowPopUp();
                        }
                        break;
                    #endregion

                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (TempVendorItemDocBO != null && TempVendorItemDocBO.DetailList != null && TempVendorItemDocBO.DetailList.Count > 0)
                        {
                            gvrItem = ((Button)sender).Parent.Parent as GridViewRow;

                            EditIndex = gvrItem.RowIndex;
                            int tempSlno = TempVendorItemDocBO.DetailList[EditIndex].ListSlNo.DeepClone();
                            TempVendorItemDocBO.DetailList = TempVendorItemDocBO.DetailList.Where(row => tempSlno != row.ListSlNo).ToList();
                            if (VendorFileDetailsList != null)
                            {
                                VendorFileDetailsList = VendorFileDetailsList.Where(fl => tempSlno != fl.SlNo).ToList();
                            }
                            SetFieldValues(ControlsEnum.DOCS);
                            ClearpopUp();
                            ShowPopUp();
                        }
                        break;
                    #endregion
                    default: break;
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
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            int slno;
            try
            {
                if (((GridView)sender).ID == "grdDocs")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        HiddenField hdfSlNo = e.Row.FindControl("hdfSlNo") as HiddenField;
                        slno = hdfSlNo.Value == string.Empty ? 0 : Convert.ToInt32(hdfSlNo.Value);
                        if (slno > 0)
                        {
                            e.Row.FindControl("fileView").Visible = VendorFileDetailsList == null || VendorFileDetailsList.Where(fle => fle.SlNo == slno).Count() == 0;
                            e.Row.FindControl("lnkEdit").Visible = e.Row.FindControl("lnkRemove").Visible = Convert.ToBoolean(Convert.ToInt16(hdfIsEditPermission.Value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Process Exception and show error message
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
                //reset all objects
            }
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
            uclPaging.CurrentPage = 1;
        }

        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        uclPaging.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        // Decrement the first page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Increment the last page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the next page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the previous page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }
                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link?
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link?
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link?
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }


        #endregion

        private void GetUserRights()
        {
            hdfIsEditPermission.Value = "0";
            string path = GetLocalResourceObject("UrlPath").ToString();
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.SBUID, currentUser.CurrentDeptPK);
            if (usrRights.Rights.Count > 0)
            {
                for (int i = 0; i < usrRights.Rights.Count; i++)
                {
                    if (usrRights.Rights[i].ActionName == "EDIT" && usrRights.Rights[i].HasActionRight == true && usrRights.Rights[i].UserDeptRight == true)
                    {
                        hdfIsEditPermission.Value = "1";
                        break;
                    }
                }
            }
        }
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
                dtPage = null;
                switch (type)
                {

                    case ControlsEnum.GETCATEGORY:
                        dtPage = VendorItemDocBL.GetMaterialCategoryTypeWithoutSemiAndFinished(0, 0, currentUser.CurrentSBUPK);
                        break;

                    case ControlsEnum.LIST:
                        ERP.Utilities.Dashboard.FilterParameters objFilterParam = new ERP.Utilities.Dashboard.FilterParameters();
                        objFilterParam.PageNumber = string.IsNullOrEmpty(PageIndex) ? 1 : Convert.ToInt32(PageIndex);
                        objFilterParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        dtPage = VendorItemDocBL.GetItemList(objFilterParam, ((hdfVendor.Value == "0" || hdfVendor.Value == "-1") ? string.Empty : hdfVendor.Value), (ddlItemCategory.SelectedValue == "-1" ? string.Empty : ddlItemCategory.SelectedValue), ((hdfItem.Value == "0" || hdfItem.Value == "-1") ? string.Empty : hdfItem.Value));
                        break;
                    case ControlsEnum.DOCSTYPE:
                        dtPage = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.VendorDocType, 2, 1, currentUser.CurrentSBUPK);
                        break;
                    case ControlsEnum.DOCSEDIT:
                        TempVendorItemDocBO = VendorItemDocBL.GetDocs(hdfMapPK.Value);
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
                    case ControlsEnum.LIST:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.GETCATEGORY:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.DOCSTYPE:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.DOCS:
                    case ControlsEnum.DOCSEDIT:
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

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    default:
                        break;

                }
                return retObject;
            }
            catch
            {
                throw;
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


                    case ControlsEnum.DOCDTLSEDIT:
                        if (TempVendorItemDocBO != null && TempVendorItemDocBO.DetailList != null)
                        {
                            ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(TempVendorItemDocBO.DetailList[EditIndex].IVD_DOC_CATEGORY));
                            txtTitle.Text = TempVendorItemDocBO.DetailList[EditIndex].IVD_DOC_TITLE;

                            txtVersion.Text = TempVendorItemDocBO.DetailList[EditIndex].IVD_VERSION;

                            txtRemarks.Text = TempVendorItemDocBO.DetailList[EditIndex].IVD_DESC;
                            chkActive.Checked = Convert.ToBoolean(TempVendorItemDocBO.DetailList[EditIndex].IVD_ACTIVE);

                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;

                            anchorFile.InnerHtml = TempVendorItemDocBO.DetailList[EditIndex].IVD_DOC_NAME;
                            anchorFile.HRef = TempVendorItemDocBO.DetailList[EditIndex].IVD_DOC_PATH;


                            if (VendorFileDetailsList != null && VendorFileDetailsList.Where(fle => fle.SlNo == TempVendorItemDocBO.DetailList[EditIndex].ListSlNo).Count() > 0)
                            {
                                anchorFile.Attributes.Add("onclick", "return false;");
                                anchorFile.Attributes.Add("class", "removedownloadClass");
                            }
                            else
                            {
                                anchorFile.Attributes.Add("onclick", "return true;");
                                anchorFile.Attributes.Add("class", "downloadClass");
                            }
                        }
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
        /// <summary>
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            string BlockedExtensions = "dll";
            if (System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower() != string.Empty)
            {
                BlockedExtensions = System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower();
            }
            bool flag = true;
            string[] extensionList = BlockedExtensions.Split(',');
            for (int i = 0; i < extensionList.Length; i++)
                if (("." + extensionList[i]) == extension)
                {
                    flag = false;
                    break;
                }
            return flag;
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
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtPage != null && dtPage.Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dtPage.Rows[0]["TOTAL_ROW_COUNT"]);

                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = string.IsNullOrEmpty(PageIndex) ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dtPage;
                            grdList.DataBind();

                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdList.DataSource = dtPage;
                            grdList.DataBind();
                        }
                        break;
                    case ControlsEnum.DOCS:
                    case ControlsEnum.DOCSEDIT:
                        grdDocs.DataSource = TempVendorItemDocBO.DetailList;
                        grdDocs.DataBind();
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
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDownList(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Category
                case ControlsEnum.GETCATEGORY:
                    ddlItemCategory.Items.Clear();
                    if (dtPage != null && dtPage.Rows.Count > 0)
                    {
                        ddlItemCategory.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPage, Resources.DataFieldRes.PrdCategoryName);
                        ddlItemCategory.DataTextField = Resources.DataFieldRes.PrdCategoryName;
                        ddlItemCategory.DataValueField = Resources.DataFieldRes.PrdCategoryPK;
                        ddlItemCategory.DataBind();
                    }
                    ddlItemCategory.Items.Insert(0, new ListItem(CommonConstants.ALL, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region DocsType
                case ControlsEnum.DOCSTYPE:
                    ddlCategory.Items.Clear();
                    if (dtPage != null && dtPage.Rows.Count > 0)
                    {
                        ddlCategory.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPage, Resources.DataFieldRes.ConstName);
                        ddlCategory.DataTextField = Resources.DataFieldRes.ConstName;
                        ddlCategory.DataValueField = Resources.DataFieldRes.ConstPK;
                        ddlCategory.DataBind();
                    }
                    ddlCategory.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                    break;
                #endregion
                default:
                    break;
            }
        }

        private void ClearAll()
        {
            hdfItem.Value = "0";
            hdfVendor.Value = "0";
            txtVendor.Text = string.Empty;
            txtItem.Text = string.Empty;
            ddlItemCategory.SelectedIndex = 0;
            dtPage = null;
            EditIndex = -1;
            anchorFile.Visible = false;
            vrfFileUpload.Enabled = true;
            VendorFileDetailsList = null;
            PageIndex = CommonConstants.SELECT_VALUE_ONE;
            GetFieldValues(ControlsEnum.LIST);
            SetFieldValues(ControlsEnum.LIST);
        }
        private void ClearpopUp()
        {
            txtTitle.Text = txtVersion.Text = txtRemarks.Text = string.Empty;
            chkActive.Checked = true;
            if (ddlCategory.Items.Count > 0)
                ddlCategory.SelectedIndex = 0;
            EditIndex = -1;
            anchorFile.Visible = false;
            vrfFileUpload.Enabled = true;
        }
        private void ShowPopUp()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_PopUp", "ShowContainerDiv('#divDocPopUp','" + GetLocalResourceObject("UploadDocs").ToString() + "','" + GetLocalResourceObject("PopUpWidth").ToString() + "','" + GetLocalResourceObject("PopUpHeight").ToString() + "');", true);
        }

        #region ControlEnum
        public enum ControlsEnum
        {
            GETCATEGORY,
            LIST,
            DOCSTYPE,
            DOCS,
            DOCSEDIT,
            DOCDTLSEDIT
        }
        #endregion
    }
}