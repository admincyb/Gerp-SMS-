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
using System.Xml;
using BusinessLogic.Shipping;
using ERPData;
using ERPManager;
using ERPService;

namespace ERPSMS_v01.Shipping
{
    public partial class BillofLoading : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Process ID of the Page
        /// </summary>
        private int PageProcessID
        {
            get
            {
                return this.ViewState["PageProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["PageProcessID"]);
            }
            set
            {
                this.ViewState["PageProcessID"] = value;
            }
        }
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
        private List<BusinessObject.Shipping.BillofLoadingUploadBO> BLUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.BLUploadList] == null ? null : (List<BusinessObject.Shipping.BillofLoadingUploadBO>)ViewState[ViewstateStrings.BLUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.BLUploadList] = value;
            }
        }
        private List<BusinessObject.Shipping.FileDetails> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FileBLDetailsList] == null ? null : (List<BusinessObject.Shipping.FileDetails>)Session[ERP.Utilities.SessionStrings.FileBLDetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FileBLDetailsList] = value;
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
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        DataSet dsShippingUploads;
        DataTable dtPageData;
        DataSet dsPlanInfo;
        private static DataSet dsLoadingPlan;
        private BusinessObject.User currentUser;
        BusinessObject.Shipping.BillofLoadingUploadBO blUploadObj;
        BillofLoadingBO billofLoadingObj;

        private string refID;
        private string inboxFlag;
        private int processPK;

        private DataSet dsShippingPlanHDR;
        private int tabLevel;

        private ServiceUtility serviceUtilityObj;
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

                    //txtDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "SCD_SEQUENCE";
                    grdUploads.DataKeyNames = itemkeyarray;
                    hdfindate.Value = DateTime.Now.ToString();

                    ShippingPlanPK = Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] != null ? (int)Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] : 0;
                    ShippingUploadType = Request.QueryString[QueryStrings.PageType] != null && Enum.TryParse<ShippingUploadsEnum>(Request.QueryString[QueryStrings.PageType].ToString(), out uploadType) ? (int)uploadType :
                        Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] != null ? (int)Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] : 0;
                    BLUploadList = null;
                    FileDetailsList = null;
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
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        GetFieldValues(ControlsEnum.UPLOADEDFILES);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        if (CurrPK == 0)
                        {
                            if (prevCompany != null && prevCompany != 0)
                            {
                                ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(prevCompany.ToString())));
                            }   
                        }
                    }
                    else
                    {
                        Response.Redirect(Resources.PageURL.ShippingPlan);
                    }
                    SetTabVisibility();
                    Session[ERP.Utilities.SessionStrings.RefID] = null;
                    Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    //Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = null;
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
            string result;
            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        dsPlanInfo = BusinessLogic.Shipping.ShippingPlanBL.GetPlanInfo(ShippingPlanPK);
                        //To get Previous transaction based company
                        if (CurrPK == 0)
                        {
                            prevCompany = BusinessLogic.Shipping.ShippingPlanBL.GetPrevCompany(ShippingPlanPK);
                        }
                        break;
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (ShippingPlanPK > 0)
                        {
                            result = BusinessLogic.Shipping.ShippingUploadsBL.GetBillofLoading(ShippingPlanPK, ShippingUploadType);
                            if (result != "")
                                billofLoadingObj = CommonFunctions.XmlDeserialize<BillofLoadingBO>(result);
                        }
                        break;
                    #endregion
                    #region UPLOADTYPE
                    case ControlsEnum.UPLOADTYPE:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.BillOfLoading, 1, currentUser.SBUID);
                        break;
                    #endregion
                    #region SHIPPINGPLANLEVEL
                    case ControlsEnum.SHIPPINGPLANLEVEL:
                        dsShippingPlanHDR = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanHDR(currentUser, ShippingPlanPK, Convert.ToInt32(CommonConstants.ACTIVE));
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
                        admCompanyMstServiceClient = null;
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
                    case ControlsEnum.DEFAULT:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        if (billofLoadingObj != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    case ControlsEnum.UPLOADTYPE:
                        BindDropDownList(ControlsEnum.UPLOADTYPE);
                        break;
                    #region Bind DropDown
                    case ControlsEnum.COMPANYLIST:
                        BindDropDownList(ControlsEnum.COMPANYLIST);
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
                                    blUploadObj = BLUploadList.SingleOrDefault(itm => itm.SCD_SEQUENCE == CurrSlNo);
                                    if (blUploadObj != null)
                                    {
                                        if (FileDetailsList == null)
                                        {
                                            FileDetailsList = new List<FileDetails>();
                                        }

                                        blUploadObj.BLD_NO = txtBLNo.Text;
                                        blUploadObj.BLD_DATE = txtDate.Text.Trim();
                                        blUploadObj.BLD_DATE = txtRemarks.Text;
                                        blUploadObj.SCD_TYPE = ShippingUploadType;
                                        blUploadObj.SCD_DOC_TYPE = Convert.ToInt32(ddlType.SelectedValue.ToString());
                                        blUploadObj.SCD_DOC_TYPE_TEXT = ddlType.SelectedItem.Text;
                                        blUploadObj.SCD_TITLE = txtTitle.Text;
                                        if (fupUpload.HasFile)
                                        {
                                            FileInfo tempFileInfoObj;
                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                            blUploadObj.AttachmentFileName = attachmentFileName;
                                            blUploadObj.FileExtension = tempFileInfoObj.Extension;
                                            blUploadObj.SCD_FILE = fupUpload.FileName;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                blUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                blUploadObj.SCD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }
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
                                    }
                                }

                            }
                            else
                            {
                                if (fupUpload.HasFile)
                                {

                                    int slno = 1;
                                    if (BLUploadList == null || BLUploadList.Count == 0)
                                    {
                                        BLUploadList = new List<BusinessObject.Shipping.BillofLoadingUploadBO>();
                                        slno = 1;
                                    }
                                    else
                                    {
                                        slno = BLUploadList.Max(itm => itm.SCD_SEQUENCE);
                                        slno++;
                                    }
                                    if (FileDetailsList == null)
                                    {
                                        FileDetailsList = new List<FileDetails>();
                                    }

                                    blUploadObj = new BillofLoadingUploadBO();
                                    blUploadObj.SCD_PK = 0;
                                    blUploadObj.SCD_SEQUENCE = slno;
                                    blUploadObj.BLD_NO = txtBLNo.Text.Trim();
                                    blUploadObj.SCD_TYPE = ShippingUploadType;
                                    blUploadObj.BLD_DATE = txtDate.Text.Trim();
                                    blUploadObj.BLD_REMARKS = txtRemarks.Text.Trim();
                                    blUploadObj.SCD_DOC_TYPE = Convert.ToInt32(ddlType.SelectedValue.ToString());
                                    blUploadObj.SCD_DOC_TYPE_TEXT = ddlType.SelectedItem.Text;
                                    blUploadObj.SCD_TITLE = txtTitle.Text;
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

                                    blUploadObj.AttachmentFileName = attachmentFileName;
                                    blUploadObj.FileExtension = tempFileInfoObj.Extension;
                                    blUploadObj.SCD_FILE = fupUpload.FileName;
                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                    {
                                        blUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    }
                                    else
                                    {
                                        //SavePath = Server.MapPath("../Upload");
                                        //blUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                        blUploadObj.SCD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                    }

                                    // blUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    blUploadObj.SCD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    FileDetailsList.Add(new FileDetails() { SlNo = slno, ShippingFile = HttpContext.Current.Request.Files[0] });
                                    BLUploadList.Add(blUploadObj);

                                }
                            }
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ResetForm(ActionsEnum.ADDITEM);
                        }
                        break;
                    #endregion
                    #region "Save"
                    case ActionsEnum.SAVE:
                        //if (BLUploadList != null && BLUploadList.Count > 0)
                        //{
                            billofLoadingObj = (BillofLoadingBO)SetUIValuesToObject(ActionsEnum.SAVE);
                            saveXml = CommonFunctions.XmlSerialize<BillofLoadingBO>(billofLoadingObj);
                            result = BusinessLogic.Shipping.ShippingUploadsBL.SaveBillofLoading(saveXml);
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
                                if (BLUploadList != null)
                                foreach (BillofLoadingUploadBO obj in BLUploadList)
                                {
                                    string filePath = savePath + obj.AttachmentFileName;
                                    FileInfo attachedFileInfo = new FileInfo(filePath);
                                    if (FileDetailsList != null)
                                    {
                                        FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.SCD_SEQUENCE);
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
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BillofLoading);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.BillofLoading + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.BillofLoading + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.ALREADYCREATED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.BillofLoading + " " + GetLocalResourceObject("AlreadyCreated").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BillofLoading);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        //}
                        //else
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ItemCount").ToString())
                        //                + "','" + Resources.ErpRes.Information + "');", true);

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
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
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
                        //if (BLUploadList != null && BLUploadList.Count > 0)
                        //{
                        ucrWrkf.ApplicationID = 0;
                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                        {

                            //if (BLUploadList != null && BLUploadList.Count > 0)
                            //{
                            billofLoadingObj = (BillofLoadingBO)SetUIValuesToObject(ActionsEnum.SAVE);
                            saveXml = CommonFunctions.XmlSerialize<BillofLoadingBO>(billofLoadingObj);
                            result = BusinessLogic.Shipping.ShippingUploadsBL.SaveBillofLoading(saveXml);
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
                                if(BLUploadList!=null)
                                foreach (BillofLoadingUploadBO obj in BLUploadList)
                                {
                                    string filePath = savePath + obj.AttachmentFileName;
                                    FileInfo attachedFileInfo = new FileInfo(filePath);
                                    if (FileDetailsList != null)
                                    {
                                        FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.SCD_SEQUENCE);
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
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.BillofLoading + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.BillofLoading + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.ALREADYCREATED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.BillofLoading + " " + GetLocalResourceObject("AlreadyCreated").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BillofLoading);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            //}
                            //else
                            //    ucrWrkf.ApplicationID = ShippingPlanPK;
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, (Resources.PageNameRes.BillofLoading));

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
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BillofLoading);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ItemCount").ToString())
                        //                           + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (BLUploadList != null && BLUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                BLUploadList = BLUploadList.Where(row => selectedItemPK != row.SCD_SEQUENCE).ToList();
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ActionsEnum.ADDITEM);
                            }
                        }
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        if (BLUploadList != null && BLUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                blUploadObj = BLUploadList.SingleOrDefault(row => selectedItemPK == row.SCD_SEQUENCE);
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
                    case ControlsEnum.SELECTEDDOC:
                        if (blUploadObj != null)
                        {
                            CurrSlNo = blUploadObj.SCD_SEQUENCE;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = blUploadObj.SCD_FILE;
                            anchorFile.HRef = blUploadObj.SCD_FILE_PATH;
                            ddlType.SelectedValue = blUploadObj.SCD_DOC_TYPE.ToString();
                            txtTitle.Text = blUploadObj.SCD_TITLE;
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
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = billofLoadingObj.BLD_PK;
                        LastModifiedTime = billofLoadingObj.LAST_MOD_DT;
                        lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                        txtBLNo.Text = billofLoadingObj.BLD_NO;
                        txtRemarks.Text = billofLoadingObj.BLD_REMARKS;
                        txtDate.Text = billofLoadingObj.BLD_DATE.ToString(Resources.Constants.DateFormatShort);
                        txtDate.ToolTip = billofLoadingObj.BLD_DATE.ToString(Resources.Constants.DateFormatShort);
                        BLUploadList = billofLoadingObj.DocDetails;
                        ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(billofLoadingObj.BLD_COMPANY.ToString())));

                        //lblContainerTypeValue.Text =billofLoadingObj.CONTAINER_NO !=null? billofLoadingObj.CONTAINER_NO.ToString() : string.Empty;// dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_CONTAINER_NO].ToString();
                        //lblDestinationPortValue.Text = billofLoadingObj.SHIP_TO_PORT !=null ? billofLoadingObj.SHIP_TO_PORT.ToString(): string.Empty;// dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_SHIP_TO_PORT].ToString();
                        //lblInTimeValue.Text = billofLoadingObj.IN_TIME != null ? Convert.ToDateTime(billofLoadingObj.IN_TIME).ToString(Resources.Constants.DateTimeFormat) : string.Empty;
                        //lblCustomer.Text = billofLoadingObj.CUSTOMER_NAME != null ? ERP.Utilities.CommonFunctions.GetShortString(billofLoadingObj.CUSTOMER_NAME.ToString(),20) : string.Empty;
                        //lblCustomer.ToolTip = billofLoadingObj.CUSTOMER_NAME != null ? billofLoadingObj.CUSTOMER_NAME.ToString() : string.Empty;
                        //lblInvoice.Text = billofLoadingObj.INV_NO != null ? billofLoadingObj.INV_NO.ToString() : string.Empty;
                        //lblInvoiceDate.Text = billofLoadingObj.INV_DATE != null ? Convert.ToDateTime(billofLoadingObj.INV_DATE.ToString()).ToString(Resources.Constants.DateFormatShort) : string.Empty;

                        break;
                    case ControlsEnum.DEFAULT:
                        if (dsPlanInfo != null && dsPlanInfo.Tables[0] != null && dsPlanInfo.Tables[0].Rows.Count > 0)
                        {
                            lblContainerTypeValue.Text = ERP.Utilities.CommonFunctions.GetShortString(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CONTAINER_NO].ToString(), 20);
                            lblContainerTypeValue.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CONTAINER_NO].ToString(),300);

                            lblDestinationPortValue.Text = ERP.Utilities.CommonFunctions.GetShortString(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SHIP_TO_PORT].ToString(), 20);
                            lblDestinationPortValue.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SHIP_TO_PORT].ToString(),300);

                            //lblInTimeValue.Text = dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.IN_TIME].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.IN_TIME]).ToString(Resources.Constants.DateTimeFormat),20) : string.Empty;
                            //lblInTimeValue.ToolTip = dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.IN_TIME].ToString() != string.Empty ? Convert.ToDateTime(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.IN_TIME]).ToString(Resources.Constants.DateTimeFormat) : string.Empty;

                            lblInTimeValue.Text = dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.IN_TIME].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CVH_BOOKING_DATE]).ToString(Resources.Constants.DateFormatShort), 20) + " " + ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.IN_TIME]).ToString(Resources.Constants.TimeFormatShort), 20) : string.Empty;
                            lblInTimeValue.ToolTip = dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.IN_TIME].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CVH_BOOKING_DATE]).ToString(Resources.Constants.DateFormatShort), 20) + " " + ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.IN_TIME]).ToString(Resources.Constants.TimeFormatShort), 20) : string.Empty;
                            hdfindate.Value = dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CVH_BOOKING_DATE].ToString() != string.Empty ? Convert.ToDateTime(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CVH_BOOKING_DATE].ToString()).ToString() : DateTime.Now.ToString();

                            lblCustomer.Text = ERP.Utilities.CommonFunctions.GetShortString(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUSTOMER_NAME].ToString(), 25);
                            lblCustomer.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUSTOMER_NAME].ToString(),300);

                            lblInvoice.Text = ERP.Utilities.CommonFunctions.GetShortString(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.INV_NO].ToString(), 30);
                            lblInvoice.ToolTip = dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.INV_NO].ToString();

                            lblInvoiceDate.Text = dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.INV_DATE].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.INV_DATE].ToString(), 25) : string.Empty;
                            lblInvoiceDate.ToolTip = dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.INV_DATE].ToString() != string.Empty ? dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.INV_DATE].ToString() : string.Empty;
                            //lblInvoiceDate.Text = dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.INV_DATE].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.INV_DATE].ToString()).ToString(Resources.Constants.DateFormatShort), 20) : string.Empty;
                            //lblInvoiceDate.ToolTip = dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.INV_DATE].ToString() != string.Empty ? Convert.ToDateTime(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.INV_DATE].ToString()).ToString(Resources.Constants.DateFormatShort) : string.Empty;
                            
                            txtDate.Text =  dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.DPH_ETD].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.DPH_ETD].ToString()).ToString(Resources.Constants.DateFormatShort), 20) : string.Empty;
                            txtDate.ToolTip = dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.DPH_ETD].ToString() != string.Empty ? Convert.ToDateTime(dsPlanInfo.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.DPH_ETD].ToString()).ToString(Resources.Constants.DateFormatShort) : string.Empty;

                            hdfDelstatus.Value = dsPlanInfo.Tables[0].Rows[0][Resources.DataFieldRes.SPDeleteStatus].ToString();
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
                    case ActionsEnum.SAVE:
                        BillofLoadingBO billofLoadingObj = new BillofLoadingBO();
                        billofLoadingObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        billofLoadingObj.BIZUNIT_PK = currentUser.SBUID;
                        billofLoadingObj.BLD_DATE = Convert.ToDateTime(txtDate.Text);
                        billofLoadingObj.BLD_DEPT = currentUser.CurrentDeptPK;
                        billofLoadingObj.BLD_NO = txtBLNo.Text;
                        billofLoadingObj.BLD_PK = CurrPK;
                        billofLoadingObj.BLD_REMARKS = txtRemarks.Text;
                        billofLoadingObj.BLD_SHIPPING_PLAN = ShippingPlanPK;
                        billofLoadingObj.DocDetails = BLUploadList;
                        billofLoadingObj.LAST_MOD_DT = LastModifiedTime;
                        billofLoadingObj.WKF_PROCESS = WkfProcessID;
                        billofLoadingObj.BLD_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        returnObj = billofLoadingObj;
                        break;

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
                        if (BLUploadList != null)
                        {
                            grdUploads.DataSource = BLUploadList;
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
                    case ControlsEnum.UPLOADTYPE:
                        ddlType.Items.Clear();
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            ddlType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                            ddlType.DataTextField = "CON_NAME";
                            ddlType.DataValueField = "CON_PK";
                            ddlType.DataBind();
                        }
                        //ddlType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.COMPANYLIST:
                        ddlCompany.Items.Clear();
                        if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                        {
                            ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                            ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                            ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                            ddlCompany.DataBind();                           
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
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    txtTitle.Text = string.Empty;
                    break;
                case ActionsEnum.SAVE:
                    //txtBLNo.Text = string.Empty;
                    //txtRemarks.Text = string.Empty;
                    //txtDate.Text = string.Empty;
                    //BLUploadList = null;
                    FileDetailsList = null;
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
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            path += "?TYPE=12";
            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    path = Resources.PageURL.BillofLoading.Replace("~", "");
                    base.WkfPageUrl = ucrWrkf.PageUrl = path;
                    PageProcessID = ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    processID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    WkfProcessID = processID;
                }
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
            //this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
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
            //  this.btnCancel.Load += new EventHandler(btnAction_Load);
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);

        }


        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
            COMPANYLIST

        }

        #endregion
    }
}