using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using System.IO;
using BusinessObject.Shipping;
using BusinessLogic.Shipping;
using ERPData;
using ERPService;
using ERPManager;

namespace ERPSMS_v01.Shipping
{
    public partial class ShippingUploads : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        private int CurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrPK] = value;
            }
        }
        private int WkfProcessID
        {
            get
            {
                return this.ViewState["WkfProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["WkfProcessID"].ToString());
            }
            set
            {
                this.ViewState["WkfProcessID"] = value;
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
        /// <summary>
        /// Shipping Plan PK
        /// </summary>
        private int ShippingPlanPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int ShippingUploadType
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.ShippingUploadType] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.ShippingUploadType];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.ShippingUploadType] = value;
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
        private List<BusinessObject.Shipping.ShippingUploadsBO> ShippingUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.ShippingUploadList] == null ? null : (List<BusinessObject.Shipping.ShippingUploadsBO>)ViewState[ViewstateStrings.ShippingUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.ShippingUploadList] = value;
            }
        }
        private List<BusinessObject.Shipping.FileDetails> FileDetailsList
        {
            get
            {
                return Session["FileDetailsList"] == null ? null : (List<BusinessObject.Shipping.FileDetails>)Session["FileDetailsList"];
            }
            set
            {
                Session["FileDetailsList"] = value;
            }
        }
        /// <summary>
        /// Aplication referance ID
        /// </summary>
        private int ReferanceID
        {
            get
            {
                return this.ViewState["ReferanceID"] == null ? 0 : Convert.ToInt32(this.ViewState["ReferanceID"].ToString());
            }
            set
            {
                this.ViewState["ReferanceID"] = value;
            }
        }
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        DataSet dsShippingUploads;
        DataTable dtPageData;
        private BusinessObject.User currentUser;
        BusinessObject.Shipping.ShippingUploadsBO shippingUploadObj;

        private string refID;
        private string inboxFlag;
        private int processPK;

        private DataSet dsShippingPlanHDR;
        private DataSet dsLoadingPlan;
        private int tabLevel;

        private ServiceUtility serviceUtilityObj;

        //Company Details
        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private int CompanyPkByUserSBU = 0;
        private int prevCompany = 0;
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            ShippingUploadsEnum uploadType;
            int referenceID;
            int processID;
            int appId;
            referenceID = 0;
            processID = 0;
            appId = 0;

            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.COMPANYLIST);
                    SetFieldValues(ControlsEnum.COMPANYLIST);
                    //Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = 1;
                    //Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.QA;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "SCD_SL_NO";
                    grdUploads.DataKeyNames = itemkeyarray;
                    FileDetailsList = null;

                    ShippingPlanPK = Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] != null ? (int)Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] : 0;
                    ShippingUploadType = Request.QueryString[QueryStrings.PageType] != null && Enum.TryParse<ShippingUploadsEnum>(Request.QueryString[QueryStrings.PageType].ToString(), out uploadType) ? (int)uploadType :
                        Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] != null ? (int)Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] : 0;

                    processID = FillProcessID();
                    ucrWrkf.ProcessID = processID;
                    if (ucrWrkf.ProcessID > 0)
                        hdfProcessID.Value = ucrWrkf.ProcessID.ToString();

                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    EntryStatus = EntryStatus.ENTRYMODE;
                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        ReferanceID = int.Parse(refID);
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                            //btnSave.Visible = false;
                            //btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        referenceID = int.Parse(refID);
                        appId = GetApplicationID(referenceID);
                        if (processPK == ucrWrkf.ProcessID)
                        {
                            ShippingPlanPK = appId;
                            base.WkfRefID = ucrWrkf.RefID = referenceID;
                        }
                    }

                    if (ShippingPlanPK > 0 && ShippingUploadType > 0)
                    {
                        Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = ShippingPlanPK;
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                        }
                        GetFieldValues(ControlsEnum.UPLOADTYPE);
                        SetFieldValues(ControlsEnum.UPLOADTYPE);
                        GetFieldValues(ControlsEnum.UPLOADEDFILES);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);

                    }
                    else
                    {
                        Response.Redirect(Resources.PageURL.ShippingPlan);
                    }
                    SetTabVisibility();
                    Session[ERP.Utilities.SessionStrings.RefID] = null;
                    Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    //Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = null;
                    ddlType.Focus();

                }
            }
            catch (Exception ex)
            {
                ucrWrkf.ViewType = 0;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
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
                    #region
                    case ControlsEnum.UPLOADTYPE:
                        if (ShippingUploadType == (int)ShippingUploadsEnum.QA)
                        {
                            dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO INSP TYPE");
                        }
                        else if (ShippingUploadType == (int)ShippingUploadsEnum.Export)
                        {
                            dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO EXP DOC");
                        }
                        else
                        {
                            dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, ShippingUploadType, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID);
                        }
                        break;
                    #endregion
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (CurrPK > 0)
                        {
                            dsShippingUploads = BusinessLogic.Shipping.ShippingUploadsBL.GetShippingUploads(CurrPK, 1, Convert.ToByte(DbActiveStatus.HASPK), ShippingUploadType);
                        }
                        else
                        {
                            dsShippingUploads = BusinessLogic.Shipping.ShippingUploadsBL.GetShippingUploads(CurrPK, ShippingPlanPK, Convert.ToByte(DbActiveStatus.ACTIVE), ShippingUploadType);
                        }
                        if (dsShippingUploads != null && dsShippingUploads.Tables[0].Rows.Count > 0)
                        {
                            var ShippingUpload = from myRow in dsShippingUploads.Tables[0].AsEnumerable()
                                                 select new ShippingUploadsBO()
                                                 {
                                                     SCD_PK = myRow.Field<int>("SCD_PK"),
                                                     SCD_SL_NO = myRow.Field<int>("SCD_PK"),
                                                     SCD_PLAN_HDR = myRow.Field<int>("SCD_PLAN_HDR"),
                                                     SCD_PLAN_HDR_NO = myRow.Field<string>("SCD_PLAN_HDR_NO"),
                                                     SCD_TYPE = myRow.Field<byte>("SCD_TYPE"),
                                                     SCD_ITEM = myRow.Field<int>("SCD_ITEM"),
                                                     SCD_ITEM_Text = myRow.Field<string>("SCD_ITEM_Text"),
                                                     SCD_DATE = myRow.Field<string>("SCD_DATE"),
                                                     SCD_TITLE = myRow.Field<string>("SCD_TITLE"),
                                                     SCD_DESC = myRow.Field<string>("SCD_DESC"),
                                                     SCD_FILE = myRow.Field<string>("SCD_FILE"),
                                                     SCD_FILE_PATH = myRow.Field<string>("SCD_FILE_PATH"),
                                                     SCD_ACTIVE = myRow.Field<byte>("SCD_ACTIVE"),
                                                     SCD_MOD_BY = myRow.Field<int>("SCD_MOD_BY"),
                                                     SCD_MOD_DT = myRow.Field<DateTime>("SCD_MOD_DT"),
                                                     SCD_COMPANY = myRow.Field<int>("SCD_COMPANY")

                                                 };
                            if (ShippingUpload != null)
                                ShippingUploadList = ShippingUpload.ToList();

                        }
                        else
                        {
                            GetFieldValues(ControlsEnum.PREVCOMPANY);
                        }
                        break;
                    #endregion
                    #region SHIPPINGPLANLEVEL
                    case ControlsEnum.SHIPPINGPLANLEVEL:
                        dsShippingPlanHDR = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanHDR(currentUser, ShippingPlanPK, Convert.ToInt32(CommonConstants.ACTIVE));
                        break;
                    #endregion
                    #region GETSALEORDERHDRBYSHIPPINGPLANPK
                    case ControlsEnum.GETSALEORDERHDRBYSHIPPINGPLANPK:
                        dtPageData = BusinessLogic.Shipping.ShippingPlanBL.GetSaleOrderHdrByShippingPlanPK(ShippingPlanPK);
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANYLIST:
                        //gets Company List
                        AdmCompanyMstService admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);

                        //if (admCompanyMstList != null && admCompanyMstList.Any())
                        //{
                        //    byte active = Convert.ToByte(DbActiveStatus.ACTIVE);
                        //    this.CompanyPkByUserSBU = admCompanyMstList.Single(x => x.CMP_BIZUNIT == currentUser.SBUID).CMP_PK;
                        //}
                        break;
                    #endregion
                    #region Default
                    case ControlsEnum.DEFAULT:
                        //dsLoadingPlan = BusinessLogic.Shipping.ShippingPlanBL.GetPlanInfo(ShippingPlanPK);
                        dsLoadingPlan = new DataSet();
                        dsLoadingPlan = LoadingPlanBL.GetLoadingPlan(ShippingPlanPK, currentUser.SBUID, 1);

                        break;
                    #endregion
                    #region Previous company
                    case ControlsEnum.PREVCOMPANY:
                        if (CurrPK == 0)
                        {
                            prevCompany = BusinessLogic.Shipping.ShippingPlanBL.GetPrevCompany(ShippingPlanPK);
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
                    #region Bind DropDown
                    case ControlsEnum.COMPANYLIST:
                        BindDropDownList(ControlsEnum.COMPANYLIST);
                        break;
                    #endregion
                    case ControlsEnum.UPLOADEDFILES:
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        if (prevCompany != null && prevCompany != 0)
                        {
                            ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(prevCompany.ToString())));
                        }
                        else
                        {
                            if (dsShippingUploads != null && dsShippingUploads.Tables[0].Rows.Count > 0)
                            {
                                ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dsShippingUploads.Tables[0].Rows[0]["SCD_COMPANY"].ToString())));
                            }
                        }
                        break;
                    case ControlsEnum.UPLOADTYPE:
                        BindDropDownList(ControlsEnum.UPLOADTYPE);
                        break;
                    case ControlsEnum.DEFAULT:
                        GetUIValuesFromObject(controlType);
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region Action Handlers

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            int result;
            string saveXml;
            int selectedItemPK;
            string action;
            int wkStatus = 0;
            DropDownList ddlWkfAction;

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

                switch (commonActions)
                {
                    #region ADDITEM
                    case ActionsEnum.ADDITEM:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {

                            if (CurrSlNo != 0)
                            {
                                if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                {
                                    shippingUploadObj = ShippingUploadList.SingleOrDefault(itm => itm.SCD_SL_NO == CurrSlNo);
                                    if (shippingUploadObj != null)
                                    {
                                        if (FileDetailsList == null)
                                        {
                                            FileDetailsList = new List<FileDetails>();
                                        }

                                        shippingUploadObj.SCD_ITEM = Convert.ToInt32(ddlType.SelectedValue);
                                        shippingUploadObj.SCD_ITEM_Text = ddlType.SelectedItem.Text;
                                        shippingUploadObj.SCD_DATE = txtDate.Text.Trim();
                                        shippingUploadObj.SCD_TITLE = txtTitle.Text.Trim();
                                        shippingUploadObj.SCD_DESC = txtDescription.Text.Trim();
                                        shippingUploadObj.SCD_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                                        shippingUploadObj.WKF_PROCESS = WkfProcessID;
                                        if (fupUpload.HasFile)
                                        {
                                            FileInfo tempFileInfoObj;
                                            //string SavePath = string.Empty;
                                            //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                            //{
                                            //    SavePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                            //}
                                            //else
                                            //{
                                            //    //SavePath = Server.MapPath("../Upload");
                                            //    SavePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                                            //}
                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                            shippingUploadObj.AttachmentFileName = attachmentFileName;
                                            shippingUploadObj.FileExtension = tempFileInfoObj.Extension;
                                            shippingUploadObj.SCD_FILE = fupUpload.FileName;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                shippingUploadObj.SCD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }
                                            //shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                            //shippingUploadObj.SCD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()+ "/"+ attachmentFileName;

                                            FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                            if (fileDetailsObj == null)
                                            {
                                                FileDetailsList.Add(new FileDetails() { SlNo = CurrSlNo, ShippingFile = HttpContext.Current.Request.Files[0] });
                                            }
                                            else
                                            {
                                                fileDetailsObj.ShippingFile = HttpContext.Current.Request.Files[0];
                                            }
                                        }
                                        shippingUploadObj.SCD_MOD_BY = currentUser.PKUser;
                                    }
                                }

                            }
                            else
                            {
                                if (fupUpload.HasFile)
                                {

                                    int slno = 1;
                                    if (ShippingUploadList == null || ShippingUploadList.Count == 0)
                                    {
                                        ShippingUploadList = new List<BusinessObject.Shipping.ShippingUploadsBO>();
                                        slno = 1;
                                    }
                                    else
                                    {
                                        slno = ShippingUploadList.Max(itm => itm.SCD_SL_NO);
                                        slno++;
                                    }
                                    if (FileDetailsList == null)
                                    {
                                        FileDetailsList = new List<FileDetails>();
                                    }

                                    shippingUploadObj = new ShippingUploadsBO();
                                    shippingUploadObj.SCD_PK = 0;
                                    shippingUploadObj.SCD_PLAN_HDR = ShippingPlanPK;
                                    shippingUploadObj.SCD_SL_NO = slno;
                                    shippingUploadObj.SCD_TYPE = Convert.ToByte(ShippingUploadType);
                                    shippingUploadObj.SCD_ITEM = Convert.ToInt32(ddlType.SelectedValue);
                                    shippingUploadObj.SCD_ITEM_Text = ddlType.SelectedItem.Text;
                                    shippingUploadObj.SCD_DATE = txtDate.Text.Trim();
                                    shippingUploadObj.SCD_TITLE = txtTitle.Text.Trim();
                                    shippingUploadObj.SCD_DESC = txtDescription.Text.Trim();
                                    shippingUploadObj.WKF_PROCESS = WkfProcessID;
                                    shippingUploadObj.SCD_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                                    FileInfo tempFileInfoObj;
                                    //string SavePath = string.Empty;
                                    //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                    //{
                                    //    SavePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                    //}
                                    //else
                                    //{
                                    //    //SavePath = Server.MapPath("../Upload");
                                    //    SavePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                                    //}
                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                    string attachmentFileFormat = tempFileInfoObj.Extension;
                                    string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                    shippingUploadObj.AttachmentFileName = attachmentFileName;
                                    shippingUploadObj.FileExtension = tempFileInfoObj.Extension;
                                    shippingUploadObj.SCD_FILE = fupUpload.FileName;
                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                    {
                                        shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    }
                                    else
                                    {
                                        //SavePath = Server.MapPath("../Upload");
                                        //shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                        shippingUploadObj.SCD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                    }

                                    // shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    shippingUploadObj.SCD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    shippingUploadObj.SCD_MOD_BY = currentUser.PKUser;
                                    FileDetailsList.Add(new FileDetails() { SlNo = slno, ShippingFile = HttpContext.Current.Request.Files[0] });
                                    ShippingUploadList.Add(shippingUploadObj);

                                }
                            }
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ResetForm(ActionsEnum.ADDITEM);
                        }
                        break;
                    #endregion
                    #region Save
                    case ActionsEnum.SAVE:
                        if (ShippingUploadList != null && ShippingUploadList.Count > 0)
                        {
                            ShippingUploadList.ForEach(dtl =>
                            {
                                if (dtl.WKF_PROCESS <= 0)
                                    dtl.WKF_PROCESS = WkfProcessID;
                            });                           
                            ShippingUploadsBOHeader ShippingUploadHdrObj = new ShippingUploadsBOHeader();
                            ShippingUploadHdrObj.ShippingUploadsBOList = ShippingUploadList;                          
                            saveXml = CommonFunctions.XmlSerialize<ShippingUploadsBOHeader>(ShippingUploadHdrObj);
                            result = BusinessLogic.Shipping.ShippingUploadsBL.SaveShippingUploads(saveXml);
                            if (result >= 0) // Success ! re-initialize the page
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

                                foreach (ShippingUploadsBO obj in ShippingUploadList)
                                {
                                    string filePath = savePath + obj.AttachmentFileName;
                                    FileInfo attachedFileInfo = new FileInfo(filePath);
                                    if (FileDetailsList != null)
                                    {
                                        FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.SCD_SL_NO);
                                        if (fileDetailsObj != null)
                                        {
                                            fileDetailsObj.ShippingFile.SaveAs(attachedFileInfo.FullName);

                                        }
                                    }
                                }
                                ResetForm(ActionsEnum.SAVE);
                                GetFieldValues(ControlsEnum.UPLOADEDFILES);
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                                        : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
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
                                    litErrorMsg.Text = (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                                        : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs) + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                                        : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs) + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                                        : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs));
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        else
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ItemCount").ToString())
                                        + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm(ActionsEnum.ADDITEM);
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        //if (ShippingUploadList != null && ShippingUploadList.Count > 0)
                        //{
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        //}
                        //else
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ItemCount").ToString())
                        //                + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        //Show WorkFlow Popup
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    case ActionsEnum.WRKFSUBMIT:

                        ucrWrkf.ApplicationID = 0;
                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                        {
                            if (ShippingUploadList != null && ShippingUploadList.Count > 0)
                            {
                                ShippingUploadList.ForEach(dtl =>
                                {
                                    if (dtl.WKF_PROCESS <= 0)
                                        dtl.WKF_PROCESS = WkfProcessID;
                                });
                                ShippingUploadsBOHeader ShippingUploadHdrObj = new ShippingUploadsBOHeader();
                                ShippingUploadHdrObj.ShippingUploadsBOList = ShippingUploadList;                              
                                saveXml = CommonFunctions.XmlSerialize<ShippingUploadsBOHeader>(ShippingUploadHdrObj);
                                result = BusinessLogic.Shipping.ShippingUploadsBL.SaveShippingUploads(saveXml);
                                if (result >= 0) // Success ! re-initialize the page
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
                                        //savePath = Server.MapPath("../Upload");
                                        savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                                    }

                                    foreach (ShippingUploadsBO obj in ShippingUploadList)
                                    {
                                        string filePath = savePath + obj.AttachmentFileName;
                                        FileInfo attachedFileInfo = new FileInfo(filePath);
                                        if (FileDetailsList != null)
                                        {
                                            FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.SCD_SL_NO);
                                            if (fileDetailsObj != null)
                                            {
                                                fileDetailsObj.ShippingFile.SaveAs(attachedFileInfo.FullName);

                                            }
                                        }
                                    }
                                    ucrWrkf.ApplicationID = ShippingPlanPK;
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
                                        litErrorMsg.Text = (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                                            : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs) + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                                            : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs) + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                                            : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs));
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                            else
                                ucrWrkf.ApplicationID = ShippingPlanPK;
                        }
                        else
                            ucrWrkf.ApplicationID = ShippingPlanPK;

                        if (ucrWrkf.ApplicationID > 0)
                        {
                            ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                            //Do WorkFlow if WorkFlow has Actions
                            if (ddlWkfAction.Items.Count > 0)
                            {
                                action = ddlWkfAction.SelectedItem.ToString();
                                result = ucrWrkf.DoWorkFlow();
                                if (result > 0)
                                {
                                    ResetForm(ActionsEnum.SAVE);
                                    GetFieldValues(ControlsEnum.UPLOADEDFILES);
                                    SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                    SetTabVisibility();
                                    ucrWrkf.FillWorkFlowDetails();
                                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                        ucrWrkf.ViewType = 1;
                                    else
                                    {
                                        ucrWrkf.ViewType = 0;
                                        //EntryStatus = EntryStatus.VIEWMODE;
                                    }
                                    ucrWrkf.ViewAction();
                                    ResetForm(ActionsEnum.SAVE);
                                    //Show Save success message and reset Contract Entry
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                                        : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs));

                                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Trx not saved
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }

                        //if (ShippingUploadList != null && ShippingUploadList.Count > 0)
                        //{
                        //ShippingUploadsBOHeader ShippingUploadHdrObj = new ShippingUploadsBOHeader();
                        //ShippingUploadHdrObj.ShippingUploadsBOList = ShippingUploadList;
                        //saveXml = CommonFunctions.XmlSerialize<ShippingUploadsBOHeader>(ShippingUploadHdrObj);
                        //result = BusinessLogic.Shipping.ShippingUploadsBL.SaveShippingUploads(saveXml);
                        //if (result >= 0) // Success ! re-initialize the page
                        //{
                        //string savePath = string.Empty;
                        //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower()))
                        //{
                        //    savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                        //}
                        //else
                        //{
                        //    savePath = Server.MapPath("../Upload");
                        //}
                        //if (!Directory.Exists(savePath))
                        //    Directory.CreateDirectory(savePath);
                        //foreach (ShippingUploadsBO obj in ShippingUploadList)
                        //{
                        //    string filePath = savePath + "\\" + obj.AttachmentFileName;
                        //    FileInfo attachedFileInfo = new FileInfo(filePath);
                        //    if (FileDetailsList != null)
                        //    {
                        //        FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.SCD_SL_NO);
                        //        if (fileDetailsObj != null)
                        //        {
                        //            fileDetailsObj.ShippingFile.SaveAs(attachedFileInfo.FullName);

                        //        }
                        //    }
                        //}
                        //Workflow submission
                        //ucrWrkf.ApplicationID = ShippingPlanPK;
                        //ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                        ////Do WorkFlow if WorkFlow has Actions
                        //if (ddlWkfAction.Items.Count > 0)
                        //{
                        //    action = ddlWkfAction.SelectedItem.ToString();
                        //    result = ucrWrkf.DoWorkFlow();
                        //    if (result > 0)
                        //    {
                        //        ResetForm(ActionsEnum.SAVE);
                        //        GetFieldValues(ControlsEnum.UPLOADEDFILES);
                        //        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        //        SetTabVisibility();
                        //        ucrWrkf.FillWorkFlowDetails();
                        //        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                        //            ucrWrkf.ViewType = 1;
                        //        else
                        //        {
                        //            ucrWrkf.ViewType = 0;
                        //            //EntryStatus = EntryStatus.VIEWMODE;
                        //        }
                        //        ucrWrkf.ViewAction();
                        //        ResetForm(ActionsEnum.SAVE);
                        //        //Show Save success message and reset Contract Entry
                        //        litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                        //        litErrorMsg.Text = string.Format(litErrorMsg.Text, (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                        //            : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs));
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //            + "','" + Resources.ErpRes.Information + "');", true);
                        //    }
                        //}
                        //}
                        //else
                        //{
                        //    if (result == (int)DbSaveStatus.SQLERROR)
                        //    {
                        //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //            + "','" + Resources.ErpRes.Information + "');", true);
                        //    }
                        //    else if (result == (int)DbSaveStatus.CONCURRENCY)
                        //    {
                        //        litErrorMsg.Text = (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                        //            : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs) + " " + Resources.Messages.EditUsedByAnotherUser;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                        //    }
                        //    else if (result == (int)DbSaveStatus.CODEEXIST)
                        //    {
                        //        litErrorMsg.Text = (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                        //            : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs) + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                        //    }
                        //    else
                        //    {
                        //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        //        litErrorMsg.Text = string.Format(litErrorMsg.Text, (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                        //            : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs));
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //            + "','" + Resources.ErpRes.Information + "');", true);
                        //    }
                        //}
                        //}
                        //else
                        //{
                        //    //Workflow submission
                        //    //ucrWrkf.ApplicationID = ShippingPlanPK;
                        //    ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                        //    //Do WorkFlow if WorkFlow has Actions
                        //    if (ddlWkfAction.Items.Count > 0)
                        //    {
                        //        action = ddlWkfAction.SelectedItem.ToString();
                        //        result = ucrWrkf.DoWorkFlow();
                        //        if (result > 0)
                        //        {
                        //            SetTabVisibility();
                        //            ucrWrkf.FillWorkFlowDetails();
                        //            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                        //                ucrWrkf.ViewType = 1;
                        //            else
                        //            {
                        //                ucrWrkf.ViewType = 0;
                        //                //EntryStatus = EntryStatus.VIEWMODE;
                        //            }
                        //            ucrWrkf.ViewAction();
                        //            //Show Save success message and reset Contract Entry
                        //            litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                        //            litErrorMsg.Text = string.Format(litErrorMsg.Text, (ShippingUploadType == (int)ShippingUploadsEnum.Export ? Resources.PageNameRes.UploadExportDocs
                        //                : ShippingUploadType == (int)ShippingUploadsEnum.QA ? Resources.PageNameRes.UploadQADocs : Resources.PageNameRes.UploadPhotographs));
                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //                + "','" + Resources.ErpRes.Information + "');", true);
                        //        }
                        //    }
                        //}
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (ShippingUploadList != null && ShippingUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                ShippingUploadList = ShippingUploadList.Where(row => selectedItemPK != row.SCD_SL_NO).ToList();
                                if (FileDetailsList != null)
                                {
                                    FileDetailsList = FileDetailsList.Where(fl => selectedItemPK != fl.SlNo).ToList();
                                }
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ActionsEnum.ADDITEM);
                            }
                        }
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        if (ShippingUploadList != null && ShippingUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                shippingUploadObj = ShippingUploadList.SingleOrDefault(row => selectedItemPK == row.SCD_SL_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        break;
                    #endregion
                    #region Tab navigation
                    case ActionsEnum.DEFAULT:
                        Response.Redirect(Resources.PageURL.SalesOrderListing);
                        break;
                    case ActionsEnum.SHIPPINGPLAN:
                        Response.Redirect(Resources.PageURL.ShippingPlan);
                        break;
                    case ActionsEnum.CONTAINEREVALUATION:
                        Response.Redirect(Resources.PageURL.ContainerEvaulation);
                        break;
                    case ActionsEnum.CONTAINERINSPECTION:
                        Response.Redirect(Resources.PageURL.ContainerInspection);
                        break;
                    case ActionsEnum.UPLOADQA:
                        Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.QA;
                        Response.Redirect(Resources.PageURL.UploadQa);
                        break;
                    case ActionsEnum.UPLOADEXPORT:
                        Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.Export;
                        Response.Redirect(Resources.PageURL.UploadExport);
                        break;
                    case ActionsEnum.LOADINGPLAN:
                        Response.Redirect(Resources.PageURL.LoadingPlan);
                        break;
                    case ActionsEnum.UPLOADPHOTOGRAPHS:
                        Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.Photographs;
                        Response.Redirect(Resources.PageURL.UploadPhotographs);
                        break;
                    case ActionsEnum.GOODOUTWARD:
                        Response.Redirect(Resources.PageURL.GoodOutward);
                        break;
                    case ActionsEnum.BL:
                        Response.Redirect(Resources.PageURL.BillofLoading);
                        break;
                    case ActionsEnum.CONTAINERRELEASE:
                        Response.Redirect(Resources.PageURL.ContainerRelease);
                        break;
                    #endregion

                    #region PRINT
                    case ActionsEnum.PRINT:
                        PrinterControl1.ShippingPlanID = ShippingPlanPK;
                        PrinterControl1.SetCommericalInvoice(PrinterControl1.ShippingPlanID);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);
                        break;
                    #endregion
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
                if (((GridView)sender).ID == "grdUploads")
                {
                    if (EntryStatus == EntryStatus.VIEWMODE)
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                        {
                            //e.Row.Cells[4].Visible = false;
                            e.Row.Cells[5].Visible = false;
                            e.Row.Cells[6].Visible = false;
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

        #endregion
        #region Helper Methods
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        /// <param name="controlType">Controls to Bind</param>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.DEFAULT:
                        if (dsLoadingPlan != null && dsLoadingPlan.Tables[0] != null && dsLoadingPlan.Tables[0].Rows.Count > 0)
                        {
                            lblCustomerHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUS_TEXT].ToString(), 25);
                            lblCustomerHdr.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUS_TEXT].ToString(), 300);

                            lblDestinationPortHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_SHIP_TO_PORT].ToString(), 20);
                            lblDestinationPortHdr.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_SHIP_TO_PORT].ToString(), 300);

                            lblInTimeHdr.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CVH_BOOKING_DATE]).ToString(Resources.Constants.DateFormatShort), 20) + " " + ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME]).ToString(Resources.Constants.TimeFormatShort), 20) : string.Empty;
                            lblInTimeHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CVH_BOOKING_DATE]).ToString(Resources.Constants.DateFormatShort), 20) + " " + ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME]).ToString(Resources.Constants.TimeFormatShort), 20) : string.Empty;

                            lblContainerTypeValueHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_CONTAINER_TYPE_TEXT].ToString(), 20);
                            lblContainerTypeValueHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_CONTAINER_TYPE_TEXT].ToString();

                            lblShippingPlanNoHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_NO].ToString(), 20);
                            lblShippingPlanNoHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_NO].ToString();

                            lblShippingPlanDateHdr.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString()).ToString(Resources.Constants.DateFormatShort), 20) : string.Empty;
                            lblShippingPlanDateHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString() != string.Empty ? Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString()).ToString(Resources.Constants.DateFormatShort) : string.Empty;
                            hdfDelstatus.Value = dsLoadingPlan.Tables[0].Rows[0][Resources.DataFieldRes.SPDeleteStatus].ToString();
                        }
                        break;

                    case ControlsEnum.SELECTEDDOC:
                        if (shippingUploadObj != null)
                        {
                            CurrSlNo = shippingUploadObj.SCD_SL_NO;
                            ddlType.SelectedValue = shippingUploadObj.SCD_ITEM.ToString();
                            txtDate.Text = shippingUploadObj.SCD_DATE;
                            txtTitle.Text = shippingUploadObj.SCD_TITLE;
                            txtDescription.Text = shippingUploadObj.SCD_DESC;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = shippingUploadObj.SCD_FILE;
                            anchorFile.HRef = shippingUploadObj.SCD_FILE_PATH;


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
                        else
                        {
                            vrfFileUpload.Enabled = true;
                        }
                        break;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <param name="mode">Save Action</param>
        /// <returns>Object to Save</returns>
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;

            try
            {
                switch (mode)
                {

                }
                return returnObj;
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
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.UPLOADEDFILES:
                        if (ShippingUploadList != null)
                        {
                            grdUploads.DataSource = ShippingUploadList;
                            grdUploads.DataBind();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Upload Type
                    case ControlsEnum.UPLOADTYPE:
                        ddlType.Items.Clear();

                        if (dtPageData != null)
                        {
                            if (ShippingUploadType != (int)ShippingUploadsEnum.Photographs)
                            {
                                ddlType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CFG_DATA");
                                ddlType.DataTextField = "CFG_DATA";
                                ddlType.DataValueField = "CFG_VALUE";
                                ddlType.DataBind();
                            }
                            else
                            {
                                ddlType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                                ddlType.DataTextField = "CON_NAME";
                                ddlType.DataValueField = "CON_PK";
                                ddlType.DataBind();
                            }
                        }
                        ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        dtPageData = null;
                        if (ShippingUploadType == (int)ShippingUploadsEnum.QA)
                        {
                            GetFieldValues(ControlsEnum.GETSALEORDERHDRBYSHIPPINGPLANPK);
                            if (dtPageData != null)
                            {
                                if (dtPageData.Rows[0]["SOH_INSP_TYPE"] != null)
                                {
                                    ddlType.SelectedValue = dtPageData.Rows[0]["SOH_INSP_TYPE"].ToString();
                                }
                            }
                        }
                        else if (ShippingUploadType == (int)ShippingUploadsEnum.Export)
                        {
                            GetFieldValues(ControlsEnum.GETSALEORDERHDRBYSHIPPINGPLANPK);
                            if (dtPageData != null)
                            {
                                if (dtPageData.Rows[0]["SOH_EXP_DOC"] != null)
                                {
                                    ddlType.SelectedValue = dtPageData.Rows[0]["SOH_EXP_DOC"].ToString();
                                }
                            }
                        }
                        else if (ShippingUploadType == (int)ShippingUploadsEnum.Photographs)
                        {
                            if (ddlType.Items.Count > 1)
                            {
                                ddlType.SelectedValue = ddlType.Items[1].Value;
                            }
                        }
                        break;
                    #endregion
                    case ControlsEnum.COMPANYLIST:
                        ddlCompany.Items.Clear();
                        if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                        {
                            ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                            ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                            ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                            ddlCompany.DataBind();

                            //if (this.CompanyPkByUserSBU != 0)
                            //{
                            //    ddlCompany.SelectedValue = Convert.ToString(this.CompanyPkByUserSBU);
                            //}
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ResetForm(ActionsEnum action)
        {
            switch (action)
            {
                case ActionsEnum.ADDITEM:
                    //ddlType.ClearSelection();
                    txtTitle.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;
                case ActionsEnum.SAVE:
                    FileDetailsList = null;
                    ShippingUploadList = null;
                    ResetForm(ActionsEnum.ADDITEM);
                    break;
            }
        }
        /// <summary>
        /// Set Tab Visibility
        /// </summary>
        private void SetTabVisibility()
        {
            GetFieldValues(ControlsEnum.SHIPPINGPLANLEVEL);
            if (dsShippingPlanHDR != null && dsShippingPlanHDR.Tables.Count > 0 && dsShippingPlanHDR.Tables[0].Rows.Count > 0)
            {
                tabLevel = Convert.ToInt32(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_TRX_STATUS"]);
            }
            //spnShippingPlan.Visible = lnkShippingPlan.Visible = tabLevel >= (int)ShippingTabsEnum.ShippingPlan;
            spnContainerEval.Visible = lnkContainerEval.Visible = tabLevel >= (int)ShippingTabsEnum.PaymentCleared;
            spnContainerInspection.Visible = lnkContainerInspection.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerEvaluated;
            spnUploadQADocs.Visible = lnkUploadQADocs.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerInspected;
            spnUploadExportDocs.Visible = lnkUploadExportDocs.Visible = tabLevel >= (int)ShippingTabsEnum.QADocsUploaded;
            spnLoadingPlan.Visible = lnkLoadingPlan.Visible = tabLevel >= (int)ShippingTabsEnum.ExportDocsUploaded;
            spnUploadPhotographs.Visible = lnkUploadPhotographs.Visible = tabLevel >= (int)ShippingTabsEnum.LoadingPlanCompleted;
            spnDeliveryOrder.Visible = lnkDeliveryOrder.Visible = tabLevel >= (int)ShippingTabsEnum.PhotographsUploaded;
            spnContainerRelease.Visible = lnkContainerRelease.Visible = tabLevel >= (int)ShippingTabsEnum.DeliveryOrderCompleted;
            spnBillofLoading.Visible = lnkBillofLoading.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerReleased;
        }
        #endregion
        #region WorkFlow Methods

        #region WorkFlow Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private int GetApplicationID(int refId)
        {
            int appId = 0;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtApplication = wrkfService.GetApplicationID(refId);
            if (dtApplication != null)
            {
                if (dtApplication.Rows.Count > 0)
                {
                    processPK = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PROCESS] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PROCESS]);
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessID()
        {
            int processID = 0;
            string path = string.Empty;
            switch (ShippingUploadType)
            {
                case (int)ShippingUploadsEnum.QA:
                    path = Resources.PageURL.UploadQa.Replace("~", "");
                    break;
                case (int)ShippingUploadsEnum.Export:
                    path = Resources.PageURL.UploadExport.Replace("~", "");
                    break;
                default:
                    path = Resources.PageURL.UploadPhotographs.Replace("~", "");
                    break;
            }
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                ucrWrkf.PageUrl = path;
                processID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                WkfProcessID = processID;
            }
            return processID;
        }

        #endregion

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
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnAddItem.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkShippingPlan.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerEval.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerInspection.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadQADocs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadExportDocs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkLoadingPlan.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadPhotographs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerRelease.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkBillofLoading.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkPrintShippingDocs.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnAddItem.Load += new EventHandler(btnAction_Load);
            this.lnkShippingPlan.Load += new EventHandler(btnAction_Load);
            this.lnkContainerEval.Load += new EventHandler(btnAction_Load);
            this.lnkContainerInspection.Load += new EventHandler(btnAction_Load);
            this.lnkUploadQADocs.Load += new EventHandler(btnAction_Load);
            this.lnkUploadExportDocs.Load += new EventHandler(btnAction_Load);
            this.lnkLoadingPlan.Load += new EventHandler(btnAction_Load);
            this.lnkUploadPhotographs.Load += new EventHandler(btnAction_Load);
            this.lnkDeliveryOrder.Load += new EventHandler(btnAction_Load);
            this.lnkContainerRelease.Load += new EventHandler(btnAction_Load);
            this.lnkBillofLoading.Load += new EventHandler(btnAction_Load);
            this.lnkPrintShippingDocs.Load += new EventHandler(btnAction_Load);
        }

        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
        }
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {

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
            try
            {

                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                }
                //else if (EntryStatus == EntryStatus.NEWMODE)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                //}
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }










        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (ShippingUploadType == (int)ShippingUploadsEnum.Export)
            {
                lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb_ExportDocs").ToString();

                spnUploadExportDocs.Attributes.Remove("class");
                spnUploadExportDocs.Attributes.Add("class", "tab-active");
                lnkUploadExportDocs.CssClass = "tab-active";
                spnUploadPhotographs.Attributes.Remove("class");
                spnUploadPhotographs.Attributes.Add("class", "tab-inactive");
                lnkUploadPhotographs.CssClass = "tab-inactive";
                spnUploadQADocs.Attributes.Remove("class");
                spnUploadQADocs.Attributes.Add("class", "tab-inactive");
                lnkUploadQADocs.CssClass = "tab-inactive";
                Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.Export;
            }
            else if (ShippingUploadType == (int)ShippingUploadsEnum.QA)
            {
                lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb_UploadQADocs").ToString();

                spnUploadExportDocs.Attributes.Remove("class");
                spnUploadExportDocs.Attributes.Add("class", "tab-inactive");
                lnkUploadExportDocs.CssClass = "tab-inactive";
                spnUploadPhotographs.Attributes.Remove("class");
                spnUploadPhotographs.Attributes.Add("class", "tab-inactive");
                lnkUploadPhotographs.CssClass = "tab-inactive";
                spnUploadQADocs.Attributes.Remove("class");
                spnUploadQADocs.Attributes.Add("class", "tab-active");
                lnkUploadQADocs.CssClass = "tab-active";
                Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.QA;
            }
            else
            {
                lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb_Photographs").ToString();

                spnUploadExportDocs.Attributes.Remove("class");
                spnUploadExportDocs.Attributes.Add("class", "tab-inactive");
                lnkUploadExportDocs.CssClass = "tab-inactive";
                spnUploadPhotographs.Attributes.Remove("class");
                spnUploadPhotographs.Attributes.Add("class", "tab-active");
                lnkUploadPhotographs.CssClass = "tab-active";
                spnUploadQADocs.Attributes.Remove("class");
                spnUploadQADocs.Attributes.Add("class", "tab-inactive");
                lnkUploadQADocs.CssClass = "tab-inactive";
                Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.Photographs;
            }
            lblBreadCrum.Text = lblBreadCrum.Text.Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }
        #endregion
        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            UPLOADEDFILES,
            UPLOADTYPE,
            SELECTEDDOC,
            SHIPPINGPLANLEVEL,
            GETSALEORDERHDRBYSHIPPINGPLANPK,
            DEFAULT,
            COMPANYLIST,
            PREVCOMPANY

        }

        #endregion
    }
}

