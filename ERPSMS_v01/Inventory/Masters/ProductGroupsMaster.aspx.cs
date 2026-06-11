using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPService.Inventory;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERPSMS_v01.UserControls;
using BusinessObject.Inventory;
using BusinessLogic.Inventory;
using System.IO;

namespace ERPSMS_v01.Inventory.Masters
{
    public partial class ProductGroupsMaster : System.Web.UI.Page
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
        /// Current PK
        /// </summary>
        private Byte CurrGrpType
        {
            get
            {
                return Convert.ToByte(this.ViewState[ViewstateStrings.CurrGrpType]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrGrpType] = value;
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
        private int CurrSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] = value;
            }
        }
        private List<ADM_DOC_ATTACH> DocAttachList
        {
            get
            {
                return ViewState[ViewstateStrings.DocAttachList] == null ? null : (List<ADM_DOC_ATTACH>)ViewState[ViewstateStrings.DocAttachList];
            }
            set
            {
                ViewState[ViewstateStrings.DocAttachList] = value;
            }
        }
        private List<FileDetails> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FilePODetailsList] == null ? null : (List<FileDetails>)Session[ERP.Utilities.SessionStrings.FilePODetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FilePODetailsList] = value;
            }
        }
        #endregion

        private ActionsEnum commonActions;
        //page related class objects      
        private INV_ITEM_GROUP_MST InvItemGroupMstObj;
        private INV_ITEM_MST InvItemMstObj;
        private ServiceUtility serviceUtilityObj;
        //List for binding details to controls
        private List<INV_ITEM_GROUP_MST> InvItemGroupMstList;
        private List<INV_ITEM_MST> InvItemMstList;
        ADM_DOC_ATTACH admDocAttachObj;

        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;
        private ProductBO productBO;
        #endregion

        #region PageLevel Events
        /// <summary>
        /// page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
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
                    this.CurrGrpType = Convert.ToByte(Request.QueryString["TYPE"].ToString());
                    if (this.CurrGrpType == 3)
                        treeLabel.InnerText = GetLocalResourceObject("Formers").ToString();
                    else
                        treeLabel.InnerText = GetLocalResourceObject("Items").ToString();

                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.INVProductGroupPK;
                    grdProductGroupMst.DataKeyNames = datakeyarray;

                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarray;

                    FileDetailsList = null;
                    DocAttachList = null;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            InvItemGroupMstService InvItemGroupMstServiceClient = null;
            try
            {
                InvItemGroupMstServiceClient = new InvItemGroupMstService();
                InvItemGroupMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemGroupMstServiceClient);
                InvItemGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_GROUP_MST>();
                InvItemMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_MST>();
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdProductGroupMst.PageSize;
                        serviceUtilityObj.FilterBy = ddlFilterBy.SelectedValue == CommonConstants.SELECT_VALUE_ZERO ? null : ddlFilterBy.SelectedValue;
                        serviceUtilityObj.FilterValue = HttpUtility.HtmlEncode(txtSearchBy.Text.Trim());
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.INVProductGroupPK : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        InvItemGroupMstObj.IGM_PK = CurrPK;
                        InvItemGroupMstObj.IGM_GROUP_TYPE = this.CurrGrpType;
                        InvItemGroupMstList = InvItemGroupMstServiceClient.GetInvItemGroupMst(InvItemGroupMstObj, serviceUtilityObj);
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    case ControlsEnum.PRODUCTGROUP:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        InvItemGroupMstObj.IGM_PK = CurrPK;
                        InvItemGroupMstObj.IGM_GROUP_TYPE = this.CurrGrpType;
                        InvItemGroupMstList = InvItemGroupMstServiceClient.GetInvItemGroupMst(InvItemGroupMstObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.ITEM:
                        InvItemMstObj.ITM_GROUP = CurrPK;
                        InvItemMstObj.ITM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        if (this.CurrGrpType == 3)
                            InvItemMstList = InvItemGroupMstServiceClient.GetInvItemMst(InvItemMstObj, ItemCategory.Former);
                        else
                            InvItemMstList = InvItemGroupMstServiceClient.GetInvItemMst(InvItemMstObj);
                        break;
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (InvItemGroupMstList != null && InvItemGroupMstList.Count > 0)
                        {
                            InvItemGroupMstServiceClient = new InvItemGroupMstService();
                            InvItemGroupMstServiceClient = CommonFunctions.InitiateClient(InvItemGroupMstServiceClient);
                            admDocAttachObj = CommonFunctions.Initilize<ADM_DOC_ATTACH>();
                            serviceUtilityObj = new ServiceUtility();
                            //serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                            //serviceUtilityObj.PageSize = grdPOInvoiceList.PageSize;
                            admDocAttachObj.DOC_TASK_ID = (int)InvItemGroupMstList[0].IGM_PK;
                            admDocAttachObj.DOC_TASK = (int)DocTaskEnum.PRODUCTGROUPTASK;
                            DocAttachList = InvItemGroupMstServiceClient.GetDocAttachments(admDocAttachObj, serviceUtilityObj);

                        }


                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                InvItemGroupMstObj = null;
                InvItemMstObj = null;
                serviceUtilityObj = null;
                InvItemGroupMstServiceClient = null;
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
                        BindGrid(ControlsEnum.DEFAULT);
                        break;
                    case ControlsEnum.PRODUCTGROUP:
                        GetUIValuesFromObject(ControlsEnum.PRODUCTGROUP);
                        break;
                    case ControlsEnum.ITEM:
                        BindTreeView();
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        BindGrid(ControlsEnum.UPLOADEDFILES);
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
        /// Method to bind the Item Tree
        /// </summary>
        private void BindTreeView()
        {
            try
            {
                TreeNode child;
                TreeNode root;
                trvItem.Nodes.Clear();
                //root = new TreeNode(GetLocalResourceObject("Department").ToString(), "0");
                root = new TreeNode(GetLocalResourceObject("AllItems").ToString(), "0");
                trvItem.Nodes.Add(root);
                root.Collapse();

                if (InvItemMstList != null && InvItemMstList.Count > 0)
                {
                    foreach (INV_ITEM_MST item in InvItemMstList)
                    {
                        child = new TreeNode();
                        child.ShowCheckBox = true;
                        child.Checked = item.ITM_GROUP == CurrPK ? true : false;
                        child.Text = item.ITM_NAME;
                        child.Value = item.ITM_PK.ToString();
                        root.ChildNodes.Add(child);
                    }
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }

        /// <summary>
        // Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private INV_ITEM_GROUP_MST SetUIValuesToObject()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                InvItemGroupMstObj.IGM_PK = CurrPK;
                InvItemGroupMstObj.IGM_CODE = HttpUtility.HtmlEncode(txtGroupCode.Text.Trim());
                InvItemGroupMstObj.IGM_NAME = HttpUtility.HtmlEncode(txtGroupName.Text.Trim());
                InvItemGroupMstObj.IGM_DESC = HttpUtility.HtmlEncode(txtGroupDesc.Text.Trim());

                InvItemGroupMstObj.IGM_MOD_DT = LastModifiedTime;
                InvItemGroupMstObj.IGM_ACTIVE = Convert.ToByte(ddlStatus.SelectedValue);
                InvItemGroupMstObj.IGM_BIZUNIT = currentUser.SBUID;
                InvItemGroupMstObj.IGM_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                InvItemGroupMstObj.IGM_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                InvItemGroupMstObj.IGM_GROUP_TYPE = this.CurrGrpType;
                return InvItemGroupMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                InvItemGroupMstObj = null;
            }
        }

        /// <summary>
        // Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private List<INV_ITEM_MST> SetUIItemValuesToObject()
        {
            productBO = new ProductBO();
            productBO.DetailList = new List<DetailsBO>();
            List<INV_ITEM_MST> INV_ITEM_MSTList;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                INV_ITEM_MSTList = new List<INV_ITEM_MST>();
                foreach (TreeNode root in trvItem.Nodes)
                {
                    foreach (TreeNode child in root.ChildNodes)
                    {
                        InvItemMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_MST>();
                        InvItemMstObj.ITM_PK = Convert.ToInt32(child.Value);
                        InvItemMstObj.ITM_MOD_DT = LastModifiedTime;
                        InvItemMstObj.ITM_BIZUNIT = currentUser.SBUID;
                        InvItemMstObj.ITM_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        if (child.Checked)
                        {
                            InvItemMstObj.ITM_GROUP = 0;
                            #region Product Group mapping for production planning
                            DetailsBO detail = new DetailsBO();
                            detail.ProductGroupPK = CurrPK;
                            detail.ProductPK = Convert.ToInt32(child.Value);
                            detail.Active = Convert.ToByte(DbActiveStatus.ACTIVE);
                            productBO.DetailList.Add(detail);
                            #endregion
                        }
                        else
                        {
                            InvItemMstObj.ITM_GROUP = null;
                        }
                        INV_ITEM_MSTList.Add(InvItemMstObj);
                    }
                }
                return INV_ITEM_MSTList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                INV_ITEM_MSTList = null;
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
                    #region PRODUCTGROUP
                    case ControlsEnum.PRODUCTGROUP:
                        //assigning the UI controls with the corresponding ListObject value AsrSovMstList
                        if (InvItemGroupMstList != null && InvItemGroupMstList.Count() > 0)
                        {
                            CurrPK = InvItemGroupMstList[0].IGM_PK;
                            txtGroupCode.Text = HttpUtility.HtmlDecode(InvItemGroupMstList[0].IGM_CODE);
                            txtGroupName.Text = HttpUtility.HtmlDecode(InvItemGroupMstList[0].IGM_NAME);
                            ddlStatus.SelectedValue = InvItemGroupMstList[0].IGM_ACTIVE.ToString();
                            txtGroupDesc.Text = HttpUtility.HtmlDecode(InvItemGroupMstList[0].IGM_DESC);
                            LastModifiedTime = InvItemGroupMstList[0].IGM_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                        }
                        //Concurrency Account details Deleted By Another User
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.CurrGrpType == 1
                                                ? Resources.PageNameRes.ProductGroups
                                                : this.CurrGrpType == 2
                                                    ? Resources.PageNameRes.ProductionGroups
                                                    : Resources.PageNameRes.FormerGroups);
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.DEFAULT);
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            GetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ModifiedDatePnl.Visible = false;
                            throw new Exception(litErrorMsg.Text);
                        }
                        break;
                    #endregion
                    #region SELECTED DOC
                    case ControlsEnum.SELECTEDDOC:
                        if (admDocAttachObj != null)
                        {
                            CurrSlNo = admDocAttachObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = admDocAttachObj.DOC_NAME;
                            anchorFile.HRef = admDocAttachObj.DOC_PATH;
                            if (FileDetailsList != null && FileDetailsList.Where(fle => fle.SlNo == CurrSlNo).Count() > 0)
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
                    #endregion
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
                    case ControlsEnum.DEFAULT:
                        if (InvItemGroupMstList != null)
                        {
                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdProductGroupMst.DataSource = InvItemGroupMstList;
                            grdProductGroupMst.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            uclPaging.Visible = false;
                        }
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        grdUploads.DataSource = DocAttachList;
                        grdUploads.DataBind();
                        break;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Status Dropdown
        /// </summary>
        public void BindStatusDropDown()
        {
            ddlStatus.Items.Clear();
            ddlStatus.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            ddlStatus.Items.Insert(1, new ListItem(Resources.Controls.Active, ((int)RecordStatus.ACTIVE).ToString()));
            ddlStatus.Items.Insert(2, new ListItem(Resources.Controls.InActive, ((int)RecordStatus.INACTIVE).ToString()));
            ddlStatus.SelectedValue = "1";
        }

        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdProductGroupMst.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt16(grdProductGroupMst.DataKeys[grdrow.RowIndex].Values[0]);
                        // Get And Set the Location details
                        GetFieldValues(ControlsEnum.PRODUCTGROUP);
                        SetFieldValues(ControlsEnum.PRODUCTGROUP);
                        GetFieldValues(ControlsEnum.UPLOADEDFILES);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        if (grdUploads.Rows.Count > 0)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
                        txtGroupCode.Focus();
                        ModifiedDatePnl.Visible = true;
                        if (Mode == ActionsEnum.VIEW)
                            EntryStatus = EntryStatus.VIEWMODE;
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;
                        return;
                    }
                }

                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.DEFAULT:
                    CurrPK = 0;
                    txtSearchBy.Text = string.Empty;
                    txtGroupCode.Text = string.Empty;
                    txtGroupName.Text = string.Empty;
                    ddlStatus.SelectedValue = CommonConstants.SELECTVAL;
                    txtGroupDesc.Text = string.Empty;
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    lblLastModifiedHDR.Text = string.Empty;
                    ModifiedDatePnl.Visible = false;
                    FileDetailsList = null;
                    DocAttachList = null;
                    SetFieldValues(ControlsEnum.UPLOADEDFILES);
                    ResetForm(ControlsEnum.ADDITEM);
                    break;
                case ControlsEnum.ADDITEM:
                    //ddlType.ClearSelection();
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
            }
        }

        /// <summary>
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            string BlockedExtensions = "exe,dll";
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
        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            InvItemGroupMstService InvItemGroupMstServiceClient;
            InvItemGroupMstServiceClient = null;
            FileInfo tempFileInfoObj;
            int selectedItemPK;
            string savePath = string.Empty;
            long? docSaveResult;
            try
            {
                int result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region Save
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            InvItemGroupMstList = new List<INV_ITEM_GROUP_MST>();
                            InvItemGroupMstServiceClient = new InvItemGroupMstService();
                            InvItemGroupMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemGroupMstServiceClient);
                            InvItemGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_GROUP_MST>();
                            InvItemGroupMstObj = SetUIValuesToObject();
                            InvItemMstList = SetUIItemValuesToObject();
                            InvItemGroupMstList.Add(InvItemGroupMstObj);
                            if (!InvItemGroupMstServiceClient.IsProductGroupExist(InvItemGroupMstObj))//Function Used For Product Group Duplication Checking
                            {
                                result = InvItemGroupMstServiceClient.SaveInvItemGroupMst(InvItemGroupMstList, InvItemMstList);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    #region ATTACHMENT SAVE
                                    if (DocAttachList != null && DocAttachList.Count > 0)
                                    {
                                        savePath = string.Empty;
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

                                        foreach (ADM_DOC_ATTACH obj in DocAttachList)
                                        {
                                            string[] docName = obj.DOC_PATH.Split('/');
                                            string filePath = savePath + obj.DOC_NAME;
                                            if (docName.Length > 0)
                                                filePath = savePath + docName[docName.Length - 1];
                                            FileInfo attachedFileInfo = new FileInfo(filePath);
                                            if (FileDetailsList != null)
                                            {
                                                FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                                if (fileDetailsObj != null)
                                                {
                                                    fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                                                }
                                            }
                                        }
                                    }
                                    docSaveResult = InvItemGroupMstServiceClient.SaveDocAttachemts(DocAttachList, (int)result, (int)DocTaskEnum.PRODUCTGROUPTASK);
                                    #endregion

                                    #region Group Mapping for Production Planning
                                    if (productBO != null && productBO.DetailList.Count > 0)
                                    {
                                        string xmlDoc = CommonFunctions.XmlSerialize<ProductBO>(productBO);
                                        int retVal = ProductsBL.SaveProductGroup(xmlDoc);
                                    }
                                    #endregion

                                    SortBy = Resources.DataFieldRes.INVProductGroupPK;
                                    SortDirection = Resources.Report.SortDescending;
                                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(),
                                        this.CurrGrpType == 1
                                            ? Resources.PageNameRes.ProductGroups
                                            : this.CurrGrpType == 2
                                                ? Resources.PageNameRes.ProductionGroups
                                                : Resources.PageNameRes.FormerGroups);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.DEFAULT);
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                    btnNew.Focus();
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ProductGroupNameExist;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Product);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);

                            }
                        }
                        break;
                    #endregion

                    #region New
                    case ActionsEnum.NEW:
                        BindStatusDropDown();
                        GetFieldValues(ControlsEnum.ITEM);
                        SetFieldValues(ControlsEnum.ITEM);
                        ModifiedDatePnl.Visible = false;
                        this.txtGroupCode.Focus();
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        btnSearch.Focus();
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.DEFAULT);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Edit
                    case ActionsEnum.EDIT:
                        BindStatusDropDown();
                        SetUIEditView(commonActions);
                        GetFieldValues(ControlsEnum.ITEM);
                        SetFieldValues(ControlsEnum.ITEM);
                        break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        InvItemGroupMstServiceClient = new InvItemGroupMstService();
                        InvItemGroupMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemGroupMstServiceClient);
                        InvItemGroupMstList = new List<INV_ITEM_GROUP_MST>();
                        InvItemGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_GROUP_MST>();
                        InvItemGroupMstObj.IGM_PK = CurrPK;
                        InvItemGroupMstObj.IGM_MOD_DT = LastModifiedTime;
                        InvItemGroupMstList.Add(InvItemGroupMstObj);

                        result = InvItemGroupMstServiceClient.DeleteInvItemGroupMst(InvItemGroupMstList);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            #region ATTACHMENT DELETE
                            docSaveResult = InvItemGroupMstServiceClient.SaveDocAttachemts(null, CurrPK, (int)DocTaskEnum.PRODUCTGROUPTASK);
                            #endregion
                            ResetForm(ControlsEnum.DEFAULT);
                            btnNew.Focus();
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), this.CurrGrpType == 1
                                        ? Resources.PageNameRes.ProductGroups
                                        : this.CurrGrpType == 2
                                            ? Resources.PageNameRes.ProductionGroups
                                            : Resources.PageNameRes.FormerGroups);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        //delete reference error
                        else if (result == -1)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("CannotdeleteAlreadyasigned").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.VIEW:
                        BindStatusDropDown();
                        SetUIEditView(commonActions);
                        GetFieldValues(ControlsEnum.ITEM);
                        SetFieldValues(ControlsEnum.ITEM);
                        btnCancel.Focus();
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.ACTIVATE:
                        BindStatusDropDown();
                        SetUIEditView(commonActions);
                        GetFieldValues(ControlsEnum.ITEM);
                        SetFieldValues(ControlsEnum.ITEM);
                        break;
                    #endregion

                    #region ADDITEM
                    case ActionsEnum.ADDITEM:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (fupUpload.HasFile && !IsValidExtension(new FileInfo(fupUpload.PostedFile.FileName).Extension))
                        {
                            litErrorMsg.Text = "Invalid File";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
                        }
                        else//valid
                        {

                            if (CurrSlNo != 0)
                            {
                                if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                {
                                    admDocAttachObj = DocAttachList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrSlNo);
                                    if (admDocAttachObj != null)
                                    {
                                        if (FileDetailsList == null)
                                        {
                                            FileDetailsList = new List<FileDetails>();
                                        }
                                        if (fupUpload.HasFile)
                                        {

                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                            admDocAttachObj.DOC_NAME = fupUpload.FileName;
                                            admDocAttachObj.DOC_TYPE = tempFileInfoObj.Extension;
                                            admDocAttachObj.DOC_CRTD_DT = DateTime.Now;
                                            admDocAttachObj.DOC_CRTD_BY = currentUser.PKUser;
                                            admDocAttachObj.DOC_MOD_DT = DateTime.Now;
                                            admDocAttachObj.DOC_MOD_BY = currentUser.PKUser;
                                            admDocAttachObj.DOC_BIZUNIT = currentUser.SBUID;
                                            admDocAttachObj.DOC_MODULE = (int)DocModuleEnum.PRODUCTGROUP;
                                            admDocAttachObj.DOC_TASK = (int)DocTaskEnum.PRODUCTGROUPTASK;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                admDocAttachObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                admDocAttachObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }
                                            FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                            if (fileDetailsObj == null)
                                            {
                                                FileDetailsList.Add(new FileDetails() { SlNo = CurrSlNo, PoFile = HttpContext.Current.Request.Files[0] });
                                            }
                                            else
                                            {
                                                fileDetailsObj.PoFile = HttpContext.Current.Request.Files[0];
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                if (fupUpload.HasFile)
                                {

                                    int slno = 1;
                                    if (DocAttachList == null || DocAttachList.Count == 0)
                                    {
                                        DocAttachList = new List<ADM_DOC_ATTACH>();
                                        slno = 1;
                                    }
                                    else
                                    {
                                        slno = DocAttachList.Max(itm => itm.DOC_SEQ_NO);
                                        slno++;
                                    }
                                    if (FileDetailsList == null)
                                    {
                                        FileDetailsList = new List<FileDetails>();
                                    }

                                    admDocAttachObj = new ADM_DOC_ATTACH();
                                    admDocAttachObj.DOC_PK = 0;
                                    admDocAttachObj.DOC_SEQ_NO = (short)slno;
                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                    string attachmentFileFormat = tempFileInfoObj.Extension;
                                    string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                    admDocAttachObj.DOC_NAME = fupUpload.FileName;
                                    admDocAttachObj.DOC_TYPE = tempFileInfoObj.Extension;
                                    admDocAttachObj.DOC_CRTD_DT = DateTime.Now;
                                    admDocAttachObj.DOC_CRTD_BY = currentUser.PKUser;
                                    admDocAttachObj.DOC_MOD_DT = DateTime.Now;
                                    admDocAttachObj.DOC_MOD_BY = currentUser.PKUser;
                                    admDocAttachObj.DOC_BIZUNIT = currentUser.SBUID;
                                    admDocAttachObj.DOC_MODULE = (int)DocModuleEnum.PRODUCTGROUP;
                                    admDocAttachObj.DOC_TASK = (int)DocTaskEnum.PRODUCTGROUPTASK;
                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                    {
                                        admDocAttachObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                    }
                                    else
                                    {
                                        admDocAttachObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                    }

                                    // admDocAttachObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    admDocAttachObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    FileDetailsList.Add(new FileDetails() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                    DocAttachList.Add(admDocAttachObj);

                                }
                            }

                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ResetForm(ControlsEnum.ADDITEM);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                            //}
                        }
                        break;
                    #endregion

                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (DocAttachList != null && DocAttachList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                DocAttachList = DocAttachList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion

                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        if (DocAttachList != null && DocAttachList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                admDocAttachObj = DocAttachList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("ProductGroupDuplicate").ToString()))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.Code)) + "','" + Resources.Messages.Information + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                InvItemGroupMstObj = null;
                InvItemGroupMstList = null;
                InvItemMstObj = null;
                InvItemMstList = null;
                InvItemGroupMstServiceClient = null;
            }
        }

        /// <summary>
        /// Row data bound Event Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if (((GridView)sender).ID == "grdProductGroupMst")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblProductGroupActive = e.Row.FindControl("lblProductGroupActive") as Label;
                    lblProductGroupActive.Text = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.INVProductGroupStatus).ToString() == ((int)RecordStatus.ACTIVE).ToString() ? Resources.Controls.Active : Resources.Controls.InActive;
                    lblProductGroupActive.ToolTip = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.INVProductGroupStatus).ToString() == ((int)RecordStatus.ACTIVE).ToString() ? Resources.Controls.Active : Resources.Controls.InActive;
                }
            }
            else if (((GridView)sender).ID == "grdUploads")
            {
                int slno;
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[4].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    slno = Convert.ToInt32(grdUploads.DataKeys[e.Row.RowIndex][0]);
                    if (slno > 0)
                    {
                        e.Row.FindControl("fileView").Visible = FileDetailsList == null || FileDetailsList.Where(fle => fle.SlNo == slno).Count() == 0;
                    }
                }
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
                if (SortBy == e.SortExpression)
                {
                    if (SortDirection == Resources.Report.SortAscending)
                        SortDirection = Resources.Report.SortDescending;
                    else
                        SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.Report.SortAscending;
                }
                this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //////base.CheckBtnVisibility(sender);
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
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
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

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            string breadCrumb;
            string docTitle = string.Empty;

            string breadCrumPage = string.Empty;
            string breadCrumMode = "Creation";

            if (this.CurrGrpType == 1)
            {
                breadCrumPage = "Product Groups";
                docTitle = Resources.Captions.Title_ProductGroups;
            }
            else if (this.CurrGrpType == 2)
            {
                breadCrumPage = "Production Groups";
                docTitle = Resources.Captions.Title_ProductionGroups;
            }
            else if (this.CurrGrpType == 3)
            {
                breadCrumPage = GetLocalResourceObject("FormerGroupsCaption").ToString();
                docTitle = Resources.Captions.Title_FormerGroups;
            }


            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails();});", true);

            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
                breadCrumMode = "Listing";
            }
            breadCrumb = string.Format(this.GetLocalResourceObject("BreadcrumbCustom").ToString(), breadCrumPage, breadCrumMode);
            breadCrumb = breadCrumb.Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            lblBreadCrum.Text = breadCrumb;
            this.Title = docTitle;
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            PRODUCTGROUP,
            ITEM,
            SELECTEDDOC,
            ADDITEM,
            UPLOADEDFILES
        }
        /// Attachment Module Enum
        /// </summary>
        public enum DocModuleEnum
        {
            PRODUCTGROUP = 2

        }
        /// <summary>
        /// Attachment Task Enum
        /// </summary>
        public enum DocTaskEnum
        {
            PRODUCTGROUPTASK = 25

        }
        #endregion

    }
}