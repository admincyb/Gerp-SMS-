using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject.Common;
using ERPSMS_v01.UserControls;
using CustomControls;
using System.Data;
using BusinessObject.PurchaseOrderManagement;
using System.Linq;
using BusinessLogic;
using System.Xml;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.PurchaseOrderManagement
{
    public partial class RequestForQuote : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        private RFQSelectedDetails RFQSelectedDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.RFQSelectedDetails] == null ? null : (RFQSelectedDetails)Session[ERP.Utilities.SessionStrings.RFQSelectedDetails];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.RFQSelectedDetails] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int RFQPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.RFQPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.RFQPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.RFQPK] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private RFQParameters RFQParameters
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.RFQParameters] == null ? null : (RFQParameters)Session[ERP.Utilities.SessionStrings.RFQParameters];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.RFQParameters] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private RFQDetailParameters RFQItemDetailParameters
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.RFQItemDetailParameters] == null ? null : (RFQDetailParameters)Session[ERP.Utilities.SessionStrings.RFQItemDetailParameters];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.RFQItemDetailParameters] = value;
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
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        //page related Entity Object
        private ADM_COMPANY_MST admCompanyMstObj;
        private ServiceUtility serviceUtilityObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        DataTable dtCurrentCompany;
        private DataSet dsPageData;
        private string xmlParameter;
        private BusinessObject.User currentUser;
        private RFQHeader rfqHeadObj;
        private string trxNo;

        private string refID;
        private string inboxFlag;

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;

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
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            string resultXml;
            string xmlHeader;
            resultXml = string.Empty;
            xmlHeader = string.Empty;
            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    // Qty Decimal setting for Purchase Start
                    hdfDecimalFormat.Value = "#0.";
                    int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                    for (int i = 0; i < NoDecimalDigitsP2P; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                    }
                    // End  Qty Decimal setting for Purchase 

                    AST_DOC_MODE.Value = "0";

                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "RFD_SL_NO";
                    grdItemSelected.DataKeyNames = itemkeyarray;

                    string[] vendorkeyarray;
                    vendorkeyarray = new string[1];
                    vendorkeyarray[0] = "RVM_VENDOR";
                    grdVendorSelected.DataKeyNames = vendorkeyarray;

                    //Used for Integration purpose
                    FillProcessID();
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    
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
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(refID);
                        RFQPK = GetApplicationID(ucrWrkf.RefID);
                    }
                    else if (Session[ERP.Utilities.SessionStrings.RFQPK] != null)
                    {
                        RFQPK = (int)Session[ERP.Utilities.SessionStrings.RFQPK];
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(RFQPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (Session[ERP.Utilities.SessionStrings.RFQMODE] != null)
                        {
                            EntryStatus = (EntryStatus)Session[ERP.Utilities.SessionStrings.RFQMODE];
                        }
                        else
                            EntryStatus = EntryStatus.VIEWMODE;
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                           // EntryStatus = EntryStatus.VIEWMODE;
                        }
                    }
                    if (RFQPK > 0)
                    {
                        GetFieldValues(ControlsEnum.RFQ);
                        if (dsPageData != null && dsPageData.Tables.Count > 0)
                        {
                            foreach (DataRow dr in dsPageData.Tables[0].Rows)
                            {
                                xmlHeader += dr[0].ToString();
                            }
                            rfqHeadObj = CommonFunctions.XmlDeserialize<RFQHeader>(xmlHeader);
                        }
                        else
                            rfqHeadObj = null;
                        SetFieldValues(ControlsEnum.RFQ);
                    }
                    else if (RFQParameters != null)
                    {
                        EntryStatus = EntryStatus.NEWMODE;
                        RFQItemDetailParameters = new RFQDetailParameters()
                        {
                            VendorParameters = RFQParameters.VendorParameters,
                            ItemParameters = RFQParameters.ItemParameters.Where(itm => !itm.PRD_PK.HasValue || itm.PRD_PK.Value <= 0).
                            Select(itm => new ItemParameter()
                            {
                                RFD_ITEM = itm.ITM_PK,
                                RFD_ITEM_SPEC = itm.PRD_Spec,
                                RFD_QTY_REQUESTED = itm.PRD_Qty,
                                RFD_REQD_DATE = itm.PRD_ReqDate,
                                RFD_UOM = itm.PRD_UOM
                            }).ToList(),
                            PRParameters = RFQParameters.ItemParameters.Where(itm => itm.PRD_PK.HasValue && itm.PRD_PK.Value > 0).
                            Select(itm => new PRParameter()
                            {
                                PRD_PK = itm.PRD_PK.Value
                            }).ToList()
                        };
                        txtRFQDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);
                        xmlParameter = CommonFunctions.XmlSerialize<RFQDetailParameters>(RFQItemDetailParameters);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        if (dsPageData != null && dsPageData.Tables.Count > 0)
                        {
                            foreach (DataRow dr in dsPageData.Tables[0].Rows)
                            {
                                resultXml += dr[0].ToString();
                            }
                            RFQSelectedDetails = CommonFunctions.XmlDeserialize<RFQSelectedDetails>(resultXml);
                            if (RFQParameters.ItemParameters != null)
                            {
                                RFQSelectedDetails.ItemDetails.ForEach(itm => itm.PRDetails = RFQParameters.ItemParameters
                                    .Where(prm => prm.ITM_PK == itm.RFD_ITEM)
                                    // && prm.PRD_Spec == itm.RFD_ITEM_SPEC && prm.PRD_ReqDate == itm.RFD_REQD_DATE
                                    .Select(prm => new RFQSelectedItemPR()
                                    {
                                        RRM_ITEM = prm.ITM_PK,
                                        RRM_PR_DTL = prm.PRD_PK.HasValue ? prm.PRD_PK.Value : 0,
                                        RRM_QTY_REQUESTED = prm.PRD_Qty,
                                        RRM_UOM = prm.PRD_UOM,
                                        RRM_SL_NO = itm.RFD_SL_NO,
                                        RRM_ITEM_SPEC = HttpUtility.HtmlEncode(prm.PRD_Spec),
                                        RRM_REQD_DATE = prm.PRD_ReqDate
                                    }).ToList());
                            }
                        }
                        else
                            RFQSelectedDetails = new RFQSelectedDetails();
                        AST_DOC_MODE.Value = GetDOCMODE();

                        SetFieldValues(ControlsEnum.DEFAULT);
                        //GetFieldValues(ControlsEnum.TRXNO);
                        //lblRFQTrxNoTxt.Text = trxNo;
                        lblRFQTrxNoTxt.Text = Resources.Messages.DocGenerationNew;

                        Session[ERP.Utilities.SessionStrings.RefID] = null;
                        Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("ItemNotselected").ToString()
                                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                    }
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

                    hdfAppType.Value = ApplicationType.RFQ;
                    hdfAppSubType.Value = string.Empty;
                }
            }
            catch (Exception ex)
            {
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
                AdmCompanyMstService admCompanyMstServiceClient;
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        dsPageData = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQHeader(xmlParameter, 0);
                        break;
                    case ControlsEnum.TRXNO:
                        trxNo = BusinessLogic.CommonManagement.CommonBL.GetTrxNo(ApplicationType.RFQ, 0, 0, currentUser.PKUser);
                        break;
                    case ControlsEnum.RFQ:
                        dsPageData = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQHeader(string.Empty, RFQPK);
                        break;
                    #region Company
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        dtCurrentCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
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
                        BindGrid(ControlsEnum.SELECTEDITEM);
                        BindGrid(ControlsEnum.SELECTEDVENDORS);
                        if (RFQSelectedDetails != null && RFQSelectedDetails.ItemDetails[0].RFH_COMPANY !=null)
                            hdfCompanyPk.Value = RFQSelectedDetails.ItemDetails[0].RFH_COMPANY.ToString();
                        else
                            hdfCompanyPk.Value = currentUser.SBUID.ToString();
                        break;
                    case ControlsEnum.RFQ:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.SELECTEDVENDORS:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.SELECTEDITEM:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.COMPANY:
                        BindDropDownList(ControlsEnum.COMPANY);
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
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            int result;
            string saveXml;
            int selectedVendorPK;
            int selectedItemPK;
            IEnumerable<RFQSelectedVendor> selectedVendors;//for WorkFlow
            DropDownList ddlWkfAction;
            string action;
            int status;
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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                switch (commonActions)
                {
                    case ActionsEnum.SAVE:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            rfqHeadObj = (RFQHeader)SetUIValuesToObject(commonActions);
                            if (rfqHeadObj != null)
                            {
                                saveXml = CommonFunctions.XmlSerialize<RFQHeader>(rfqHeadObj);
                                result = BusinessLogic.PurchaseOrderManagement.RequestForQuote.SaveRFQDetails(saveXml);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    //Show Save success message and reset Contract Entry
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
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
                                        litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                        }
                        break;
                    case ActionsEnum.DELETE:
                        if (RFQPK > 0)
                        {
                            result = BusinessLogic.PurchaseOrderManagement.RequestForQuote.DeleteRFQDetails(RFQPK, LastModifiedTime);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
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
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDiv('#divWkfSubmit','" + Resources.ErpRes.Submit + "','950','400');", true);
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                rfqHeadObj = (RFQHeader)SetUIValuesToObject(commonActions);
                                if (rfqHeadObj != null)
                                {
                                    if (rfqHeadObj.VendorDetails != null && rfqHeadObj.VendorDetails.Count > 0)
                                    {
                                        saveXml = CommonFunctions.XmlSerialize<RFQHeader>(rfqHeadObj);
                                        result = BusinessLogic.PurchaseOrderManagement.RequestForQuote.SaveRFQDetails(saveXml);
                                        if (result >= 0) // Save Success ! do WorkFlow
                                        {
                                            //Workflow submission
                                            ucrWrkf.ApplicationID = result;
                                            //ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                            ////Do WorkFlow if WorkFlow has Actions
                                            //if (ddlWkfAction.Items.Count > 0)
                                            //{
                                            //    action = ddlWkfAction.SelectedItem.ToString();
                                            //    result = ucrWrkf.DoWorkFlow();
                                            //    //Show Save success message and reset Contract Entry
                                            //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                            //    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            //        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                            //}
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
                                                litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("VendorNotselected").ToString()
                                               + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }

                            }
                            else
                                ucrWrkf.ApplicationID = RFQPK;
                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();

                                    //Show Save success message and reset Contract Entry
                                    #region Inbox or Listing Page Redirection
                                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                    {
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                //Trx not saved
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }


                        }
                        break;
                    #endregion
                    case ActionsEnum.REMOVEITEM:
                        if (RFQSelectedDetails != null && RFQSelectedDetails.ItemDetails != null && RFQSelectedDetails.ItemDetails.Count > 1)
                        {
                            selectedItemPK = Convert.ToInt32(grdItemSelected.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                RFQSelectedDetails.ItemDetails = RFQSelectedDetails.ItemDetails.Where(row => selectedItemPK != row.RFD_SL_NO).ToList();
                                foreach (GridViewRow gvr in grdItemSelected.Rows)
                                {
                                    if (gvr.RowType == DataControlRowType.DataRow)
                                    {
                                        RFQSelectedItem rfqSelectedItem;
                                        rfqSelectedItem = RFQSelectedDetails.ItemDetails.SingleOrDefault(itm => itm.RFD_SL_NO == Convert.ToInt32(grdItemSelected.DataKeys[gvr.RowIndex][0].ToString()));
                                        if (rfqSelectedItem != null)
                                        {
                                            rfqSelectedItem.RFD_ITEM_SPEC = HttpUtility.HtmlEncode((gvr.FindControl("txtItemSpecLst") as TextBox).Text);
                                            rfqSelectedItem.RFD_QTY_REQUESTED = Convert.ToDecimal((gvr.FindControl("txtItemQuantityLst") as TextBox).Text);
                                            rfqSelectedItem.RFD_REQD_DATE = Convert.ToDateTime((gvr.FindControl("txtItemDateLst") as TextBox).Text);
                                        }
                                    }
                                }
                                SetFieldValues(ControlsEnum.SELECTEDITEM);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Min_Items").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    case ActionsEnum.REMOVEVENDOR:
                        if (RFQSelectedDetails != null && RFQSelectedDetails.VendorDetails != null && RFQSelectedDetails.VendorDetails.Count > 1)
                        {
                            selectedVendorPK = Convert.ToInt32(grdVendorSelected.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedVendorPK > 0)
                            {
                                RFQSelectedDetails.VendorDetails = RFQSelectedDetails.VendorDetails.Where(row => selectedVendorPK != row.RVM_VENDOR).ToList();
                                SetFieldValues(ControlsEnum.SELECTEDVENDORS);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Min_Vendors").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    case ActionsEnum.RFQSEARCH:
                        RFQSelectedDetails = null;
                        RFQParameters = null;
                        RFQItemDetailParameters = null;
                        Session[ERP.Utilities.SessionStrings.RFQPK] = null;
                        Session[ERP.Utilities.SessionStrings.PRSearchResult] = null;
                        Session[ERP.Utilities.SessionStrings.PRSelected] = null;
                        Session[ERP.Utilities.SessionStrings.RFQVendor] = null;
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RFQSearch), false);
                        break;
                    case ActionsEnum.RFQRESPONSE:
                        if (RFQPK > 0)
                        {
                            status = string.IsNullOrEmpty(hdfStatus.Value) ? 0 : Convert.ToInt32(hdfStatus.Value);
                            if (status == (int)WorkFlowStatus.APPROVED)
                            {
                                Session[ERP.Utilities.SessionStrings.RFQPK] = RFQPK;
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RFQResponse), false);
                            }
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Response").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                       GetLocalResourceObject("SelecctRequest").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    case ActionsEnum.RFQVENDORRESPONSE:
                        status = string.IsNullOrEmpty(hdfStatus.Value) ? 0 : Convert.ToInt32(hdfStatus.Value);
                        if (status == (int)WorkFlowStatus.APPROVED)
                        {
                            selectedVendorPK = Convert.ToInt32(grdVendorSelected.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (RFQPK > 0)
                            {
                                if (selectedVendorPK > 0)
                                    Session[ERP.Utilities.SessionStrings.RFQVendor] = selectedVendorPK;
                                Session[ERP.Utilities.SessionStrings.RFQPK] = RFQPK;
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RFQResponse), false);
                                return;
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Response").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    case ActionsEnum.CANCEL:
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RFQListing), false);
                        break;
                    case ActionsEnum.PRINT:
                        selectedVendorPK = Convert.ToInt32(grdVendorSelected.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                        if (RFQPK > 0)
                        {
                            // Response.Redirect("../Reports/GenerateReport.aspx?ID=" + RFQPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value + "&VNDPK=" + selectedVendorPK.ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + RFQPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value + "&VNDPK=" + selectedVendorPK.ToString() + "');", true);

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Record").ToString() + "','" + Resources.ErpRes.Information + "');", true); 
                        }
                        break;
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
            try
            {
                if ((sender as GridView).ID == "grdVendorSelected")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //(e.Row.FindControl("imbPrint") as ImageButton).Visible = RFQPK > 0 && !string.IsNullOrEmpty(hdfStatus.Value)
                        //   && Convert.ToInt32(hdfStatus.Value) == (int)WorkFlowStatus.APPROVED;
                        //(e.Row.FindControl("btnRespond") as ImageButton).Visible = RFQPK > 0 && !string.IsNullOrEmpty(hdfStatus.Value)
                        //    && Convert.ToInt32(hdfStatus.Value) == (int)WorkFlowStatus.APPROVED;
                        //(e.Row.FindControl("btnRemove") as ImageButton).Visible = (string.IsNullOrEmpty(hdfStatus.Value)
                        //    || Convert.ToInt32(hdfStatus.Value) == (int)WorkFlowStatus.DRAFT) && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE);
                    }
                }
                else if ((sender as GridView).ID == "grdItemSelected")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //(e.Row.FindControl("btnRemove") as ImageButton).Visible = (string.IsNullOrEmpty(hdfStatus.Value)
                        //    || Convert.ToInt32(hdfStatus.Value) == (int)WorkFlowStatus.DRAFT) && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE);
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

        #region --- For Grid Actions----

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ActionHandler(object sender, GridViewRowEventArgs e)
        //{
        //    int sthPK = 0;
        //    try
        //    {
        //        #region Grid Fixed Columns
        //        if ((sender as GridView).ID == "grdPoList")
        //        {
        //            if (e.Row.RowType == DataControlRowType.DataRow)
        //            {
        //                Label lblVendor = e.Row.FindControl("lblVendor") as Label;
        //                Label lblShipping = e.Row.FindControl("lblShipping") as Label;
        //                if (PurOrderHdrList != null && PurOrderHdrList.Count > 0)
        //                {

        //                    //   lblVendor.Text = PurOrderHdrList[e.Row.RowIndex].PUR_VENDOR_MST.VEN_CODE;
        //                    ADM_DEPT_MST AdmDeptMstObj = new ADM_DEPT_MST();
        //                    //  lblShipping.Text = PurOrderHdrList[e.Row.RowIndex].ADM_DEPT_MST.DPT_NAME;
        //                }
        //            }
        //        }

        //        #endregion

        //        if ((sender as GridView).ID == "grdStockTransfer")
        //        {
        //            if (e.Row.RowType == DataControlRowType.DataRow)
        //            {
        //                sthPK = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfStockTransferPK")).Value);
        //                Label lblTranTo = e.Row.FindControl("lblStTransferTo") as Label;

        //                List<INV_STK_TRAN_DTL> objList = InvStkTranHdrList[0].INV_STK_TRAN_DTL.Where(aa => aa.SFD_PK == sthPK).ToList();
        //                foreach (INV_STK_TRAN_DTL objItem in objList)
        //                    lblTranTo.Text = objItem.ADM_DEPT_MST1.DPT_NAME;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ActionHandler(object sender, GridViewSortEventArgs e)
        //{
        //    try
        //    {
        //        if (SortBy == e.SortExpression)
        //        {
        //            ////Toggle the sort expression
        //            //if (SortDirection == Resources.gComsRes.SortAscending)
        //            //    SortDirection = Resources.gComsRes.SortDescending;
        //            //else
        //            //    SortDirection = Resources.gComsRes.SortAscending;
        //        }
        //        else
        //        {
        //            //SortBy = e.SortExpression;
        //            //SortDirection = Resources.gComsRes.SortAscending;
        //        }

        //        this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
        //        GetFieldValues(ControlsEnum.DEFAULT);
        //        SetFieldValues(ControlsEnum.DEFAULT);
        //        EntryStatus = EntryStatus.LISTMODE;
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}

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
                    #region RFQ
                    case ControlsEnum.RFQ:
                        //assigning the UI controls with the corresponding Contract ListObject value
                        if (rfqHeadObj != null)
                        {
                            RFQPK = rfqHeadObj.RFH_PK;
                            hdfStatus.Value = rfqHeadObj.RFH_STATUS.ToString();
                            lblRFQTrxNoTxt.Text = ((rfqHeadObj.RFH_NO == null || rfqHeadObj.RFH_NO == "") ? Resources.Messages.DocGenerationNew : rfqHeadObj.RFH_NO);
                            txtRFQDate.Text = rfqHeadObj.RFH_DATE.ToString(Resources.ErpRes.DateFormatShort);
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(rfqHeadObj.RFH_PAYMENT_TERMS);
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(rfqHeadObj.RFH_DELIVERY_TERMS);
                            txtOtherdetails.Text = HttpUtility.HtmlDecode(rfqHeadObj.RFH_OTHER_DETAILS);

                            RFQSelectedDetails = new RFQSelectedDetails()
                            {
                                ItemDetails = rfqHeadObj.ItemDetails,
                                VendorDetails = rfqHeadObj.VendorDetails
                            };
                            SetFieldValues(ControlsEnum.SELECTEDITEM);
                            SetFieldValues(ControlsEnum.SELECTEDVENDORS);

                            ModifiedDatePnl.Visible = true;
                            LastModifiedTime = rfqHeadObj.LAST_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            hdfCompanyPk.Value = rfqHeadObj.RFH_COMPANY != null ? rfqHeadObj.RFH_COMPANY.ToString() : currentUser.SBUID.ToString();
                        }
                        else
                        {
                            //Show Concurrency and bind Listing if Query yield no results
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                             + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                        }
                        break;
                    #endregion
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
            RFQSelectedItem selectedItem;
            try
            {
                switch (mode)
                {
                    #region Save
                    case ActionsEnum.SAVE:
                    case ActionsEnum.WRKFSUBMIT:
                        rfqHeadObj = new RFQHeader();
                        rfqHeadObj.RFH_PK = RFQPK;
                        rfqHeadObj.RFH_NO = lblRFQTrxNoTxt.Text;
                        rfqHeadObj.RFH_DATE = Convert.ToDateTime(txtRFQDate.Text);
                        rfqHeadObj.RFH_DELIVERY_TERMS = HttpUtility.HtmlEncode(txtDeliveryTerms.Text);
                        rfqHeadObj.RFH_PAYMENT_TERMS = HttpUtility.HtmlEncode(txtPaymentTerms.Text);
                        rfqHeadObj.RFH_OTHER_DETAILS = HttpUtility.HtmlEncode(txtOtherdetails.Text);
                        //rfqHeadObj.AST_PK = (int)AppType.RequestForQuote;
                        if (RFQSelectedDetails != null)
                        rfqHeadObj.VendorDetails = RFQSelectedDetails.VendorDetails;

                        foreach (GridViewRow gvr in grdItemSelected.Rows)
                        {
                            if (gvr.RowType == DataControlRowType.DataRow)
                            {
                                if (RFQSelectedDetails != null)
                                {
                                    selectedItem = RFQSelectedDetails.ItemDetails.SingleOrDefault(itm => itm.RFD_SL_NO == Convert.ToInt32(grdItemSelected.DataKeys[gvr.RowIndex][0].ToString()));
                                    if (selectedItem != null)
                                    {
                                        selectedItem.RFD_ITEM_SPEC = HttpUtility.HtmlEncode((gvr.FindControl("txtItemSpecLst") as TextBox).Text);
                                        selectedItem.RFD_QTY_REQUESTED = Convert.ToDecimal((gvr.FindControl("txtItemQuantityLst") as TextBox).Text);
                                        selectedItem.RFD_REQD_DATE = Convert.ToDateTime((gvr.FindControl("txtItemDateLst") as TextBox).Text);
                                    }
                                }
                            }
                        }
                        if (RFQSelectedDetails != null)
                        rfqHeadObj.ItemDetails = RFQSelectedDetails.ItemDetails;

                        rfqHeadObj.BIZUNIT_PK = currentUser.SBUID;
                        rfqHeadObj.RFH_STATUS = string.IsNullOrEmpty(hdfStatus.Value) ? 0 : Convert.ToInt32(hdfStatus.Value);
                        rfqHeadObj.RFH_VERSION = 1;
                        rfqHeadObj.RFH_DEPT = currentUser.CurrentDeptPK;
                        rfqHeadObj.USER_PK = currentUser.PKUser;
                        rfqHeadObj.LAST_MOD_DT = LastModifiedTime;
                        rfqHeadObj.ACTIVE = 1;

                        rfqHeadObj.APT_CODE = ApplicationType.RFQ;
                        rfqHeadObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                        rfqHeadObj.WKF_FLAG = 0;

                        rfqHeadObj.RFH_COMPANY = ddlCompany.SelectedItem.Value;

                        if (mode == ActionsEnum.SAVE)
                        {
                            rfqHeadObj.WKF_FLAG = 0;
                        }
                        else if (mode == ActionsEnum.WRKFSUBMIT)
                        {
                            rfqHeadObj.WKF_FLAG = 1;
                        }

                        returnObj = rfqHeadObj;
                        break;
                    #endregion
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
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        //private void SetUIEditView(ActionsEnum mode)
        //{
        //    try
        //    {
        //        //Session[SessionStrings.ActivityPK] = null;
        //        bool bIsChecked = false;

        //        //foreach (GridViewRow grdrow in grdActivities.Rows)
        //        //{
        //        //    RadioButton rbtn;
        //        //    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");

        //        //    if (rbtn.Checked)
        //        //    {
        //        //        // get pk from the grid and assign to CurrPk
        //        //        CurrPK = Convert.ToInt32(grdActivities.DataKeys[grdrow.RowIndex].Values[0]);
        //        //        Session[SessionStrings.ActivityPK] = CurrPK;
        //        //        bIsChecked = true;
        //        //    }
        //        //}
        //        if (bIsChecked)
        //            switch (mode)
        //            {
        //                //case ActionsEnum.BASICINFO:
        //                //    Response.Redirect(Resources.PageURL.ManageActivity, true);
        //                //    break;
        //                //case ActionsEnum.DOCUMENTS:
        //                //    Response.Redirect(Resources.PageURL.ActivityAttachment, true);
        //                //    break;
        //                //case ActionsEnum.CHARGES:
        //                //    Response.Redirect(Resources.PageURL.ActivityCharges, true);
        //                //    break;
        //                //case ActionsEnum.FLIGHTINFO:
        //                //    Response.Redirect(Resources.PageURL.ActivityFlightInfo, true);
        //                //    break;
        //                //case ActionsEnum.ACTIVITYINPUTS:
        //                //    Response.Redirect(Resources.PageURL.ActivityInputs, true);
        //                //    break;
        //                //case ActionsEnum.FREEUSAGE:
        //                //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ActivityFreeUsage), false);
        //                //    break;
        //                //case ActionsEnum.REPORT:
        //                //    Response.Redirect(Resources.PageURL.ActivityCostReport, true);
        //                //    break;

        //            }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + "Please selct a row from the list" + "');", true);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
                        BindGrid(ControlsEnum.SELECTEDITEM);
                        BindGrid(ControlsEnum.SELECTEDVENDORS);
                        break;
                    case ControlsEnum.SELECTEDITEM:
                        if (RFQSelectedDetails != null)
                        {
                            foreach (var item in RFQSelectedDetails.ItemDetails)
                            {
                                if (item.RFD_ITEM_SPEC == "undefined")
                                {
                                    item.RFD_ITEM_SPEC = string.Empty;
                                }
                            }
                            grdItemSelected.DataSource = RFQSelectedDetails.ItemDetails;
                            grdItemSelected.DataBind();
                        }
                        break;
                    case ControlsEnum.SELECTEDVENDORS:
                        if (RFQSelectedDetails != null)
                        {
                            grdVendorSelected.DataSource = RFQSelectedDetails.VendorDetails;
                            grdVendorSelected.DataBind();
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
        /// Method for Dropdownlist binding
        /// </summary>
        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        ddlCompany.Items.Clear();
                        if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                        {
                            ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);                            
                            ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                            ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                            ddlCompany.DataBind();

                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(hdfCompanyPk.Value));
                        }
                        break;
                    #endregion
                    //case ControlsEnum.AIRCRAFTTYPE:
                    //    ddlAcftType.Items.Clear();
                    //    if (adAircraftTypeMstList != null && adAircraftTypeMstList.Count > 0)
                    //    {
                    //        ddlAcftType.DataSource = adAircraftTypeMstList;
                    //        ddlAcftType.DataTextField = Resources.DataFieldRes.AircraftTypeCode;
                    //        ddlAcftType.DataValueField = Resources.DataFieldRes.AircraftTypePK;
                    //        ddlAcftType.DataBind();
                    //    }
                    //    ddlAcftType.Items.Insert(0, new ListItem(Resources.gComsRes.Select, CommonConstants.SELECTVAL));
                    //    break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.RFQ, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }

        #endregion
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
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID()
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                //((HiddenField)this.Master.FindControl("hdfPageID")).Value = dtProcess.Rows[0][CommonConstants.F_PAGE].ToString();
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
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnRFQSearch.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnRFQRequest.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnRFQResponse.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnDelete.Load += new EventHandler(btnAction_Load);
            this.lbnRFQSearch.Load += new EventHandler(btnAction_Load);
            this.lbnRFQRequest.Load += new EventHandler(btnAction_Load);
            this.lbnRFQResponse.Load += new EventHandler(btnAction_Load);
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
            //this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            //this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        //{
        //    try
        //    {
        //        //switch (e.Action)
        //        //{
        //        //    case NavigationEnum.PAGECHANGE:
        //        //        uclPaging.CurrentPage = e.CurrentPage;
        //        //        break;
        //        //    case NavigationEnum.FIRST:
        //        //        // Assignment the first page index.
        //        //        if (e.CurrentPage > 1)
        //        //            uclPaging.CurrentPage = 1;
        //        //        break;
        //        //    case NavigationEnum.LAST:
        //        //        // Assignment the last page index.
        //        //        if (e.CurrentPage <= e.TotalPages)
        //        //            uclPaging.CurrentPage = e.TotalPages;
        //        //        break;
        //        //    case NavigationEnum.NEXT:
        //        //        // Increment the next page index.
        //        //        if (e.CurrentPage <= e.TotalPages)
        //        //            uclPaging.CurrentPage++;
        //        //        break;
        //        //    case NavigationEnum.PREVIOUS:
        //        //        // Decrement the previous page index.
        //        //        if (e.CurrentPage > 1)
        //        //            uclPaging.CurrentPage--;
        //        //        break;
        //        //}
        //        //PageIndex = uclPaging.CurrentPage.ToString();
        //        GetFieldValues(ControlsEnum.DEFAULT);
        //        SetFieldValues(ControlsEnum.DEFAULT);
        //        EnableDisableButtons(e.TotalPages);
        //        EntryStatus = EntryStatus.LISTMODE;
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link
            //uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we disable the previous link
            //uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we enable the next link
            //uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            //// Should we enable the last link
            //uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideForView", "$(document).ready(function(){HideForView();});", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                if (RFQPK > 0)
                    lbnRFQSearch.OnClientClick = "javascript:return false;";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }

        #endregion

        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            RFQ,
            TRXNO,
            SELECTEDITEM,
            SELECTEDVENDORS,
            COMPANY
        }

        #endregion
    }
}