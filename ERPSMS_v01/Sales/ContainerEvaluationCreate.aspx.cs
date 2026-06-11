using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using ERPService;
using ERPData;
using BusinessObject.CommonManagement;
using System.Data;
using ERPService.Inventory;
using ERPManager;
using BusinessObject.Inventory;
using System.Web.UI.HtmlControls;
using System.Reflection;
using ERPService.Sales;
using BusinessObject.Sales;

namespace ERPSMS_v01.Sales
{
    public partial class ContainerEvaluationCreate : System.Web.UI.Page
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

        /// <summary>
        /// 
        /// </summary>
        private int TypeCurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CheckListTypePK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CheckListTypePK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CheckListTypePK] = value;
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

        private BusinessObject.User currentUser;


        private SAL_CONTAINER_EVAL_HDR SalContainerEvalHdrObj;
        private ADM_CHECK_LIST_GROUP_MST admCheckListGroupMstObj;
        private ADM_CHECK_LIST_ITEM_MST admCheckListItemMstObj;
        private ADM_CHECK_LIST_TRX_HDR admCheckListTrxHdrObj;
        private ADM_CHECK_LIST_TRX_DTL admCheckListTrxDtlObj;
        private PUR_VENDOR_MST PurVedorMstObj;

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private List<SAL_CONTAINER_EVAL_HDR> SalContainerEvalHdrList;
        private List<ADM_CHECK_LIST_TRX_HDR> admCheckListTrxHdrList;
        private List<ADM_CHECK_LIST_TRX_DTL> admCheckListTrxDtlList;
        private List<PUR_VENDOR_MST> PurVedorMstList;

        private CommonService cm;

        private string refID;
        private string inboxFlag;

        private string ContainerEvaluationNo;

        private ServiceUtility serviceUtilityObj;

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
            ContainerEvaluationNo = string.Empty;
            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    AST_DOC_MODE.Value = ((int)DOCMODE.Submit).ToString();

                    uclCheckList.CLTitle = GetLocalResourceObject("EvaluationCheckList").ToString();

                    //Used for Integration purpose
                    FillProcessID();

                    TypeCurrPK = GetCheckListType();

                    uclCheckList.CLType = TypeCurrPK;
                    uclCheckList.CLTypeCode = ApplicationType.CNTEVAL;

                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

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
                            btnSave.Visible = false;
                            btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        ucrWrkf.RefID = int.Parse(refID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                    }

                    else if (Session[ERP.Utilities.SessionStrings.ContainerEvaluationPK] != null)
                    {
                        CurrPK = (int)Session[ERP.Utilities.SessionStrings.ContainerEvaluationPK];
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (Session[ERP.Utilities.SessionStrings.ContainerEvaluationMode] != null)
                        {
                            EntryStatus = (EntryStatus)Session[ERP.Utilities.SessionStrings.ContainerEvaluationMode];
                        }
                        else
                            EntryStatus = EntryStatus.VIEWMODE;
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                    }

                    if (CurrPK > 0)
                    {
                        GetFieldValues(ControlsEnum.CONTAINEREVALUATION);
                        SetFieldValues(ControlsEnum.CONTAINEREVALUATION);
                        uclCheckList.HeaderCurrPK = CurrPK;
                        AST_DOC_MODE.Value = ((int)DOCMODE.Submit).ToString();
                        if (hdfStatus.Value == ((int)WorkFlowStatus.APPROVED).ToString())
                        {
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                    }
                    else
                    {
                        EntryStatus = EntryStatus.NEWMODE;
                        txtDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);

                        AST_DOC_MODE.Value = GetDOCMODE();

                        lblEvaluationNoTxt.Text = Resources.Messages.DocGenerationNew;

                        Session[ERP.Utilities.SessionStrings.RefID] = null;
                        Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                        txtCompany.Focus();
                    }


                    hdfAppType.Value = ApplicationType.CNTEVAL;
                    hdfAppSubType.Value = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
            AdmCheckListMstService AdmCheckListServiceClient = null;
            CommonService CommonServiceClient = null;
            ContainerEvaluationService ContainerEvaluationServiceClient = null;
            try
            {
                ContainerEvaluationServiceClient = new ContainerEvaluationService();
                ContainerEvaluationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerEvaluationServiceClient);
                AdmCheckListServiceClient = new AdmCheckListMstService();
                AdmCheckListServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCheckListServiceClient);
                admCheckListGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_GROUP_MST>();
                CommonServiceClient = new CommonService();
                admCheckListItemMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_ITEM_MST>();
                SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                admCheckListTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_HDR>();
                admCheckListTrxDtlObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_DTL>();
                PurVedorMstObj = ERP.Utilities.CommonFunctions.Initilize<PUR_VENDOR_MST>();
                switch (type)
                {

                    case ControlsEnum.CONTAINEREVALUATION:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        SalContainerEvalHdrObj.CVH_PK = CurrPK;
                        SalContainerEvalHdrObj.CVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        SalContainerEvalHdrList = ContainerEvaluationServiceClient.GetContainerEvaluationList(SalContainerEvalHdrObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.CONTAINEREVALUATIONNO:
                        ContainerEvaluationNo = ContainerEvaluationServiceClient.GetContainerEvaluationNo(ApplicationType.CNTEVAL, 0, 1,
                            DateTime.Now, currentUser.PKUser, true, 0);
                        break;
                    case ControlsEnum.COMPANY:
                        PurVedorMstObj.VEN_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        PurVedorMstList = ContainerEvaluationServiceClient.GetCompany(PurVedorMstObj).OrderBy(p => p.VEN_NAME).ToList();

                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admCheckListGroupMstObj = null;
                AdmCheckListServiceClient = null;
                CommonServiceClient = null;
                admCheckListItemMstObj = null;
                SalContainerEvalHdrObj = null;
                ContainerEvaluationServiceClient = null;
                admCheckListTrxHdrObj = null;
                admCheckListTrxDtlObj = null;
                PurVedorMstObj = null;
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

                    case ControlsEnum.CONTAINEREVALUATION:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.COMPANY:
                        BindCompanyDropDown();
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

        #region
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            int resultPK, result;
            bool completed;
            string action;
            DropDownList ddlWkfAction;
            ContainerEvaluationService ContainerEvaluationServiceClient = null;
            CommonService CommonServiceClient = null;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                switch (commonActions)
                {
                    #region Save
                    case ActionsEnum.SAVE:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            SalContainerEvalHdrList = new List<SAL_CONTAINER_EVAL_HDR>();
                            ContainerEvaluationServiceClient = new ContainerEvaluationService();
                            ContainerEvaluationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerEvaluationServiceClient);
                            CommonServiceClient = new CommonService();
                            CommonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(CommonServiceClient);
                            if (hdfEvaluationNo.Value == string.Empty && AST_DOC_MODE.Value == ((int)DOCMODE.Draft).ToString())
                            {
                                GetFieldValues(ControlsEnum.CONTAINEREVALUATIONNO);
                            }
                            else
                            {
                                ContainerEvaluationNo = hdfEvaluationNo.Value;
                            }
                            SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                            SalContainerEvalHdrObj = SetUIValuesToObject(commonActions);
                            SalContainerEvalHdrList.Add(SalContainerEvalHdrObj);
                            resultPK = ContainerEvaluationServiceClient.SaveContainerEvaluation(SalContainerEvalHdrList);
                            if (resultPK >= 0) // Success ! re-initialize the page
                            {
                                admCheckListTrxHdrList = new List<ADM_CHECK_LIST_TRX_HDR>();
                                admCheckListTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_HDR>();
                                admCheckListTrxHdrObj.CLH_TRX_PK = resultPK;
                                admCheckListTrxHdrObj.CLH_NO = SalContainerEvalHdrObj.CVH_NO;
                                admCheckListTrxHdrObj.CLH_DATE = SalContainerEvalHdrObj.CVH_DATE;
                                admCheckListTrxHdrObj.CLH_TRX_TYPE = ApplicationType.CNTEVAL;
                                admCheckListTrxHdrObj.CLH_MOD_DT = LastModifiedTime;
                                admCheckListTrxHdrObj = uclCheckList.SetCheckListHeaderValuesToObject(admCheckListTrxHdrObj);
                                admCheckListTrxHdrList.Add(admCheckListTrxHdrObj);
                                result = CommonServiceClient.SaveCheckListTrxHdr(admCheckListTrxHdrList);
                                if (result > 0)
                                {
                                    admCheckListTrxDtlObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_DTL>();
                                    admCheckListTrxDtlList = uclCheckList.SetCheckListDetailsValuesToObject(result);
                                    result = CommonServiceClient.SaveCheckListTrxDtl(admCheckListTrxDtlList, result);
                                    if (result > 0)
                                    {
                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerEvaluationList) + "');", true);
                                        completed = true;
                                    }
                                    else
                                    {
                                        completed = false;
                                    }
                                }
                                else
                                {
                                    completed = false;
                                }
                                //delete saved items
                                if (!completed)
                                {
                                    SalContainerEvalHdrObj = null;
                                    SalContainerEvalHdrList = null;
                                    SalContainerEvalHdrList = new List<SAL_CONTAINER_EVAL_HDR>();
                                    SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                                    SalContainerEvalHdrObj.CVH_PK = resultPK;
                                    SalContainerEvalHdrList.Add(SalContainerEvalHdrObj);
                                    result = ContainerEvaluationServiceClient.DeleteContainerEvaluation(SalContainerEvalHdrList);
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                            }

                            else
                            {
                                if (resultPK == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                                else if (resultPK == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerEvaluationList) + "');", true);
                                }
                                else if (resultPK == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerEvaluationList) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }

                        }
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            ContainerEvaluationServiceClient = new ContainerEvaluationService();
                            ContainerEvaluationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerEvaluationServiceClient);
                            SalContainerEvalHdrList = new List<SAL_CONTAINER_EVAL_HDR>();
                            SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                            SalContainerEvalHdrObj.CVH_PK = CurrPK;
                            SalContainerEvalHdrList.Add(SalContainerEvalHdrObj);

                            result = ContainerEvaluationServiceClient.DeleteContainerEvaluation(SalContainerEvalHdrList);

                            if (result >= 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerEvaluationList) + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);

                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerEvaluationList) + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerEvaluationList) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region WRKSUBMIT
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
                            if (hdfEvaluationNo.Value == string.Empty)
                            {
                                GetFieldValues(ControlsEnum.CONTAINEREVALUATIONNO);
                            }
                            else
                            {
                                ContainerEvaluationNo = hdfEvaluationNo.Value;
                            }
                            SalContainerEvalHdrList = new List<SAL_CONTAINER_EVAL_HDR>();
                            ContainerEvaluationServiceClient = new ContainerEvaluationService();
                            ContainerEvaluationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerEvaluationServiceClient);
                            CommonServiceClient = new CommonService();
                            CommonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(CommonServiceClient);
                            SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                            SalContainerEvalHdrObj = SetUIValuesToObject(commonActions);
                            SalContainerEvalHdrList.Add(SalContainerEvalHdrObj);
                            resultPK = ContainerEvaluationServiceClient.SaveContainerEvaluation(SalContainerEvalHdrList);
                            if (resultPK >= 0) // Success ! re-initialize the page
                            {
                                admCheckListTrxHdrList = new List<ADM_CHECK_LIST_TRX_HDR>();
                                admCheckListTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_HDR>();
                                admCheckListTrxHdrObj.CLH_TRX_PK = resultPK;
                                admCheckListTrxHdrObj.CLH_NO = SalContainerEvalHdrObj.CVH_NO;
                                admCheckListTrxHdrObj.CLH_DATE = SalContainerEvalHdrObj.CVH_DATE;
                                admCheckListTrxHdrObj.CLH_TRX_TYPE = ApplicationType.CNTEVAL;
                                admCheckListTrxHdrObj.CLH_MOD_DT = LastModifiedTime;
                                admCheckListTrxHdrObj = uclCheckList.SetCheckListHeaderValuesToObject(admCheckListTrxHdrObj);
                                admCheckListTrxHdrList.Add(admCheckListTrxHdrObj);
                                result = CommonServiceClient.SaveCheckListTrxHdr(admCheckListTrxHdrList);
                                if (result > 0)
                                {
                                    admCheckListTrxDtlObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_DTL>();
                                    admCheckListTrxDtlList = uclCheckList.SetCheckListDetailsValuesToObject(result);
                                    result = CommonServiceClient.SaveCheckListTrxDtl(admCheckListTrxDtlList, result);
                                    if (result > 0)
                                    {
                                        //Workflow submission
                                        ucrWrkf.ApplicationID = resultPK;
                                        ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                        //Do WorkFlow if WorkFlow has Actions
                                        if (ddlWkfAction.Items.Count > 0)
                                        {
                                            action = ddlWkfAction.SelectedItem.ToString();
                                            result = ucrWrkf.DoWorkFlow();
                                            //Show Save success message and reset Contract Entry
                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                            if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                                            {
                                                completed = true;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerEvaluationList) + "');", true);
                                            }
                                        }
                                        completed = true;
                                    }
                                    else
                                    {
                                        completed = false;
                                    }
                                }
                                else
                                {
                                    completed = false;
                                }
                                //delete saved items
                                if (!completed)
                                {
                                    SalContainerEvalHdrObj = null;
                                    SalContainerEvalHdrList = null;
                                    SalContainerEvalHdrList = new List<SAL_CONTAINER_EVAL_HDR>();
                                    SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                                    SalContainerEvalHdrObj.CVH_PK = resultPK;
                                    SalContainerEvalHdrList.Add(SalContainerEvalHdrObj);
                                    result = ContainerEvaluationServiceClient.DeleteContainerEvaluation(SalContainerEvalHdrList);
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                            }
                            else
                            {
                                if (resultPK == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                                else if (resultPK == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerEvaluationList) + "');", true);
                                }
                                else if (resultPK == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerEvaluation + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerEvaluationList) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ContainerEvaluationList), false);
                        break;
                    #endregion

                    #region PRINT
                    //    case ActionsEnum.PRINT:
                    //        selectedVendorPK = Convert.ToInt32(grdVendorSelected.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                    //        if (RFQPK > 0)
                    //        {
                    //            Response.Redirect("../Reports/GenerateReport.aspx?ID=" + RFQPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value + "&VNDPK=" + selectedVendorPK.ToString());
                    //        }
                    //        break;
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
                ContainerEvaluationServiceClient = null;
                CommonServiceClient = null;
                admCheckListTrxHdrObj = null;
                admCheckListTrxDtlObj = null;
                SalContainerEvalHdrObj = null;
            }
        }


        #endregion





        #endregion

        #region Helper Methods

        /// <summary>
        /// Bind Company dropdown
        /// </summary>
        /// <returns></returns>    
        private void BindCompanyDropDown()
        {
            //ddlCompany.Items.Clear();
            //if (PurVedorMstList != null && PurVedorMstList.Count > 0)
            //{
            //    ddlCompany.DataSource = PurVedorMstList;
            //    ddlCompany.DataTextField = Resources.DataFieldRes.VendorName;
            //    ddlCompany.DataValueField = Resources.DataFieldRes.VendorPK;
            //    ddlCompany.DataBind();
            //}
            //ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

        }


        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private SAL_CONTAINER_EVAL_HDR SetUIValuesToObject(ActionsEnum Actions)
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                SalContainerEvalHdrObj.CVH_PK = CurrPK;
                SalContainerEvalHdrObj.CVH_NO = ContainerEvaluationNo;
                SalContainerEvalHdrObj.CVH_DATE = txtDate.Text.Trim() != string.Empty ? Convert.ToDateTime(txtDate.Text.Trim()) : DateTime.Now;
                if (hdfCompany.Value == string.Empty || hdfCompany.Value == "0")
                {
                    SalContainerEvalHdrObj.CVH_TRANS_COMP = null;
                }
                else
                {
                    SalContainerEvalHdrObj.CVH_TRANS_COMP = Convert.ToInt32(hdfCompany.Value);
                }
                SalContainerEvalHdrObj.CVH_CONTAINER_NO = HttpUtility.HtmlEncode(txtContainerNo.Text.Trim());
                //SalContainerEvalHdrObj.CVH_SERIAL_NO = HttpUtility.HtmlEncode(txtSerialNo.Text.Trim());
                SalContainerEvalHdrObj.CVH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                SalContainerEvalHdrObj.CVH_CHECK_LIST_GROUP = uclCheckList.CLGroup;
                SalContainerEvalHdrObj.CVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                SalContainerEvalHdrObj.CVH_BIZUNIT = currentUser.SBUID;
                SalContainerEvalHdrObj.CVH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                SalContainerEvalHdrObj.CVH_CRTD_DT = DateTime.Now;
                SalContainerEvalHdrObj.CVH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                SalContainerEvalHdrObj.CVH_MOD_DT = LastModifiedTime;
                switch (commonActions)
                {

                    case ActionsEnum.SAVE:
                        //SalContainerEvalHdrObj.CVH_STATUS = (int)WorkFlowStatus.DRAFT;
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //SalContainerEvalHdrObj.CVH_STATUS = (int)WorkFlowStatus.APPROVED;
                        break;
                }
                return SalContainerEvalHdrObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SalContainerEvalHdrObj = null;
            }
        }

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        /// <param name="controlType">Controls to Bind</param>
        private void GetUIValuesFromObject()
        {
            try
            {

                if (SalContainerEvalHdrList != null && SalContainerEvalHdrList.Count > 0)
                {
                    CurrPK = SalContainerEvalHdrList[0].CVH_PK;
                    hdfStatus.Value = SalContainerEvalHdrList[0].CVH_STATUS.ToString();
                    lblEvaluationNoTxt.Text = ((SalContainerEvalHdrList[0].CVH_NO == null || SalContainerEvalHdrList[0].CVH_NO == "") ? Resources.Messages.DocGenerationNew : SalContainerEvalHdrList[0].CVH_NO);
                    hdfEvaluationNo.Value = (SalContainerEvalHdrList[0].CVH_NO == null ? string.Empty : SalContainerEvalHdrList[0].CVH_NO);
                    txtDate.Text = SalContainerEvalHdrList[0].CVH_DATE.ToString(Resources.ErpRes.DateFormatShort);
                    //ddlCompany.SelectedValue = SalContainerEvalHdrList[0].CVH_TRANS_COMP.ToString();
                    hdfCompany.Value = SalContainerEvalHdrList[0].CVH_TRANS_COMP.ToString();
                    txtCompany.Text = SalContainerEvalHdrList[0].PUR_VENDOR_MST.VEN_NAME;
                    txtContainerNo.Text = HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].CVH_CONTAINER_NO);
                    //txtSerialNo.Text = HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].CVH_SERIAL_NO);
                    txtRemarks.Text = HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].CVH_REMARKS);
                    uclCheckList.CLGroup = (int)SalContainerEvalHdrList[0].CVH_CHECK_LIST_GROUP;

                    ModifiedDatePnl.Visible = true;
                    LastModifiedTime = SalContainerEvalHdrList[0].CVH_MOD_DT;
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);


                }
                else
                {
                    //Show Concurrency and bind Listing if Query yield no results
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                     + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerEvaluationList) + "');", true);
                }

            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.CNTEVAL, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        private int GetCheckListType()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.CNTEVAL, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return (int)AppTypeDetailsList[0].AST_CHECK_LIST_TYPE;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Method for Reset Controls
        /// </summary>

        private void ResetForm()
        {
            CurrPK = 0;
            uclCheckList.CLGroup = 0;
            Session[ERP.Utilities.SessionStrings.ContainerEvaluationPK] = null;
            Session[ERP.Utilities.SessionStrings.ContainerEvaluationMode] = null;
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
            // uclPaging.CurrentPage = 1;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {

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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDateComponents();});", true);
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
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
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
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            CONTAINEREVALUATION,
            TYPE,
            CONTROL,
            GROUP,
            CHECKLISTDATA,
            CONTAINEREVALUATIONNO,
            COMPANY
        }

        /// <summary>
        /// Define Controltype Enum
        /// </summary>
        enum ControlTypes
        {
            Page,
            Label,
            Text,
            DropDown,
            DateTime,
            Numeric,
            Button,
            Spacer,
            GridView,
            CheckBox,
            TextArea,
            TimePicker,
            Header,
            Table,
            Iframe,
            HiddenField,
            HourText,
            FileUpload,
            Date,
            LinkButton,
            ImageButton,
            ValidationSummary,
            DateRange

        }

        #endregion
    }

}