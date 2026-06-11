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
    public partial class ContainerInspectionCreate : System.Web.UI.Page
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
        /// 
        /// </summary>
        private int GroupPK
        {
            get
            {
                return ViewState["GroupPK"] == null ? 0 : (int)ViewState["GroupPK"];
            }
            set
            {
                ViewState["GroupPK"] = value;
            }
        }        
        /// <summary>
        /// 
        /// </summary>
        private int DESPATCHPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.DespatchID] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.DespatchID];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.DespatchID] = value;
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
        #region Variables
        private ActionsEnum commonActions;
        private BusinessObject.User currentUser;
        private SAL_CONTAINER_INSP_HDR SalContainerInspHdrObj;
        private ADM_CHECK_LIST_GROUP_MST admCheckListGroupMstObj;
        private ADM_CHECK_LIST_ITEM_MST admCheckListItemMstObj;
        private ADM_CHECK_LIST_TRX_HDR admCheckListTrxHdrObj;
        private ADM_CHECK_LIST_TRX_DTL admCheckListTrxDtlObj;
        private SAL_DESPATCH_HDR SaldespatchhdrObj;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private List<SAL_CONTAINER_INSP_HDR> SalContainerInspHdrList;
        private List<ADM_CHECK_LIST_TRX_HDR> admCheckListTrxHdrList;
        private List<ADM_CHECK_LIST_TRX_DTL> admCheckListTrxDtlList;
        private List<SAL_DESPATCH_HDR> saldespatchhdrList;
        private CommonService cm;
        private string refID;
        private string inboxFlag;
        private string ContainerInspectionNo;
        private ServiceUtility serviceUtilityObj;
        private string DONo; 
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
            ContainerInspectionNo = string.Empty;
            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    hdfApproved.Value = ((int)WorkFlowStatus.APPROVED).ToString();
                    txtDespatchNumber.Enabled = true;

                    uclCheckList.CLTitle = GetLocalResourceObject("InspectionCheckList").ToString();
                    AST_DOC_MODE.Value = ((int)DOCMODE.Submit).ToString();

                    //Used for Integration purpose
                    FillProcessID();

                    TypeCurrPK = GetCheckListType();

                    uclCheckList.CLType = TypeCurrPK;
                    uclCheckList.CLTypeCode = ApplicationType.CNTINSP;

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

                    else if (Session[ERP.Utilities.SessionStrings.ContainerInspectionPK] != null)
                    {
                        CurrPK = (int)Session[ERP.Utilities.SessionStrings.ContainerInspectionPK];
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (Session[ERP.Utilities.SessionStrings.ContainerInspectionMode] != null)
                        {
                            EntryStatus = (EntryStatus)Session[ERP.Utilities.SessionStrings.ContainerInspectionMode];
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
                        GetFieldValues(ControlsEnum.CONTAINERINSPECTION);
                        SetFieldValues(ControlsEnum.CONTAINERINSPECTION);
                        uclCheckList.HeaderCurrPK = CurrPK;
                        AST_DOC_MODE.Value = ((int)DOCMODE.Submit).ToString();
                        if (hdfStatus.Value == ((int)WorkFlowStatus.APPROVED).ToString())
                        {
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                    }
                    else
                    {
                        DONo = Request.QueryString[QueryStrings.DONO] != null ? Request.QueryString[QueryStrings.DONO]
                   : Session[ERP.Utilities.SessionStrings.DONO] != null ? Session[ERP.Utilities.SessionStrings.DONO].ToString() : string.Empty;
                        int DONumber;
                        DESPATCHPK = Int32.TryParse(DONo, out DONumber) ? DONumber : 0;
                        EntryStatus = EntryStatus.NEWMODE;
                        //check Delivery Number
                        if (DESPATCHPK > 0)
                        {
                            GetFieldValues(ControlsEnum.DELIVERYORDER);
                            CheckDeliveryOrder();
                            //ddlDeliveryNumber.SelectedValue = DESPATCHPK.ToString();
                            //hdfDPHPK.Value = DESPATCHPK.ToString();
                            //txtDespatchNumber.Enabled = false;
                        }



                        txtDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);

                        AST_DOC_MODE.Value = GetDOCMODE();

                        lblInspectionNoTxt.Text = Resources.Messages.DocGenerationNew;

                        Session[ERP.Utilities.SessionStrings.RefID] = null;
                        Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                        txtContainerNo.Focus();
                        //}
                        //else
                        //{
                        //    uclCheckList.CLType = 0;
                        //    uclCheckList.CLTypeCode = string.Empty;
                        //    //Show Save success message and reset Contract Entry
                        //    litErrorMsg.Text = Resources.ErrorMessages.Msg_DeliveryOrder;
                        //    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
                        //}
                    }


                    hdfAppType.Value = ApplicationType.CNTINSP;
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
            ContainerInspectionService ContainerInspectionServiceClient = null;
            try
            {
                ContainerInspectionServiceClient = new ContainerInspectionService();
                ContainerInspectionServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerInspectionServiceClient);
                AdmCheckListServiceClient = new AdmCheckListMstService();
                AdmCheckListServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCheckListServiceClient);
                admCheckListGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_GROUP_MST>();
                CommonServiceClient = new CommonService();
                admCheckListItemMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_ITEM_MST>();
                SalContainerInspHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_INSP_HDR>();
                admCheckListTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_HDR>();
                admCheckListTrxDtlObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_DTL>();
                SaldespatchhdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                switch (type)
                {

                    case ControlsEnum.CONTAINERINSPECTION:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        SalContainerInspHdrObj.CSH_PK = CurrPK;
                        SalContainerInspHdrObj.CSH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        SalContainerInspHdrList = ContainerInspectionServiceClient.GetContainerInspectionList(SalContainerInspHdrObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.CONTAINERINSPECTIONNO:
                        ContainerInspectionNo = ContainerInspectionServiceClient.GetContainerInspectionNo(ApplicationType.CNTINSP, 0, 1,
                            DateTime.Now, currentUser.PKUser, true, 0);
                        break;
                    case ControlsEnum.DELIVERYORDER:
                        SaldespatchhdrObj.DPH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        SaldespatchhdrObj.DPH_PK = DESPATCHPK;
                        SaldespatchhdrObj.DPH_STATUS = (byte)WorkFlowStatus.APPROVED;
                        saldespatchhdrList = ContainerInspectionServiceClient.GetDeliveryOrders(SaldespatchhdrObj);
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
                SalContainerInspHdrObj = null;
                ContainerInspectionServiceClient = null;
                admCheckListTrxHdrObj = null;
                admCheckListTrxDtlObj = null;
                SaldespatchhdrObj = null;
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

                    case ControlsEnum.CONTAINERINSPECTION:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.DELIVERYORDER:
                        BindDropDown(controlType);
                        //CheckDeliveryOrder();
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
            int Dpk = 0;
            bool completed;
            string action;
            DropDownList ddlWkfAction;
            ContainerInspectionService ContainerInspectionServiceClient = null;
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
                            //check delivery number
                            Dpk = hdfDPHPK.Value == "" ? 0 : Convert.ToInt32(hdfDPHPK.Value);
                            if (Dpk != 0)
                            {
                                SalContainerInspHdrList = new List<SAL_CONTAINER_INSP_HDR>();
                                ContainerInspectionServiceClient = new ContainerInspectionService();
                                ContainerInspectionServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerInspectionServiceClient);
                                CommonServiceClient = new CommonService();
                                CommonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(CommonServiceClient);
                                if (hdfInspectionNo.Value == string.Empty && AST_DOC_MODE.Value == ((int)DOCMODE.Draft).ToString())
                                {
                                    GetFieldValues(ControlsEnum.CONTAINERINSPECTIONNO);
                                }
                                else
                                {
                                    ContainerInspectionNo = hdfInspectionNo.Value;
                                }
                                SalContainerInspHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_INSP_HDR>();
                                SalContainerInspHdrObj = SetUIValuesToObject(commonActions);
                                SalContainerInspHdrList.Add(SalContainerInspHdrObj);
                                resultPK = ContainerInspectionServiceClient.SaveContainerInspection(SalContainerInspHdrList);
                                if (resultPK >= 0) // Success ! re-initialize the page
                                {
                                    admCheckListTrxHdrList = new List<ADM_CHECK_LIST_TRX_HDR>();
                                    admCheckListTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_HDR>();
                                    admCheckListTrxHdrObj.CLH_TRX_PK = resultPK;
                                    admCheckListTrxHdrObj.CLH_NO = SalContainerInspHdrObj.CSH_NO;
                                    admCheckListTrxHdrObj.CLH_DATE = SalContainerInspHdrObj.CSH_DATE;
                                    admCheckListTrxHdrObj.CLH_TRX_TYPE = ApplicationType.CNTINSP;
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
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
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
                                        SalContainerInspHdrObj = null;
                                        SalContainerInspHdrList = null;
                                        SalContainerInspHdrList = new List<SAL_CONTAINER_INSP_HDR>();
                                        SalContainerInspHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_INSP_HDR>();
                                        SalContainerInspHdrObj.CSH_PK = resultPK;
                                        SalContainerInspHdrList.Add(SalContainerInspHdrObj);
                                        result = ContainerInspectionServiceClient.DeleteContainerInspection(SalContainerInspHdrList);
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
                                        litErrorMsg.Text = Resources.PageNameRes.ContainerInspection + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
                                    }
                                    else if (resultPK == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.ContainerInspection + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                uclCheckList.CLType = 0;
                                uclCheckList.CLTypeCode = string.Empty;
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_DeliveryOrder;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
                            }

                        }
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            ContainerInspectionServiceClient = new ContainerInspectionService();
                            ContainerInspectionServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerInspectionServiceClient);
                            SalContainerInspHdrList = new List<SAL_CONTAINER_INSP_HDR>();
                            SalContainerInspHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_INSP_HDR>();
                            SalContainerInspHdrObj.CSH_PK = CurrPK;
                            SalContainerInspHdrList.Add(SalContainerInspHdrObj);

                            result = ContainerInspectionServiceClient.DeleteContainerInspection(SalContainerInspHdrList);

                            if (result >= 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
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
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerInspection + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerInspection + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ContainerInspection + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
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
                            //Check Delivery number 
                            Dpk = hdfDPHPK.Value == "" ? 0 : Convert.ToInt32(hdfDPHPK.Value);
                            if (Dpk != 0)
                            {
                                //generate inspection number
                                if (hdfInspectionNo.Value == string.Empty)
                                {
                                    GetFieldValues(ControlsEnum.CONTAINERINSPECTIONNO);
                                }
                                else
                                {
                                    ContainerInspectionNo = hdfInspectionNo.Value;
                                }
                                SalContainerInspHdrList = new List<SAL_CONTAINER_INSP_HDR>();
                                ContainerInspectionServiceClient = new ContainerInspectionService();
                                ContainerInspectionServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerInspectionServiceClient);
                                CommonServiceClient = new CommonService();
                                CommonServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(CommonServiceClient);
                                SalContainerInspHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_INSP_HDR>();
                                SalContainerInspHdrObj = SetUIValuesToObject(commonActions);
                                SalContainerInspHdrList.Add(SalContainerInspHdrObj);
                                resultPK = ContainerInspectionServiceClient.SaveContainerInspection(SalContainerInspHdrList);
                                if (resultPK >= 0) // Success ! re-initialize the page
                                {
                                    admCheckListTrxHdrList = new List<ADM_CHECK_LIST_TRX_HDR>();
                                    admCheckListTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_HDR>();
                                    admCheckListTrxHdrObj.CLH_TRX_PK = resultPK;
                                    admCheckListTrxHdrObj.CLH_NO = SalContainerInspHdrObj.CSH_NO;
                                    admCheckListTrxHdrObj.CLH_DATE = SalContainerInspHdrObj.CSH_DATE;
                                    admCheckListTrxHdrObj.CLH_TRX_TYPE = ApplicationType.CNTINSP;
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
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);

                                                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                                                {
                                                    completed = true;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
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
                                        SalContainerInspHdrObj = null;
                                        SalContainerInspHdrList = null;
                                        SalContainerInspHdrList = new List<SAL_CONTAINER_INSP_HDR>();
                                        SalContainerInspHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_INSP_HDR>();
                                        SalContainerInspHdrObj.CSH_PK = resultPK;
                                        SalContainerInspHdrList.Add(SalContainerInspHdrObj);
                                        result = ContainerInspectionServiceClient.DeleteContainerInspection(SalContainerInspHdrList);
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
                                        litErrorMsg.Text = Resources.PageNameRes.ContainerInspection + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
                                    }
                                    else if (resultPK == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.ContainerInspection + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                uclCheckList.CLType = 0;
                                uclCheckList.CLTypeCode = string.Empty;
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_DeliveryOrder;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
                            }

                        }
                        break;
                    #endregion

                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList), false);
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

                    #region TEXTCHANGED
                    case ActionsEnum.SHOW:
                        DESPATCHPK = hdfDPHPK.Value != string.Empty ? Convert.ToInt32(hdfDPHPK.Value) : 0;
                        GetFieldValues(ControlsEnum.DELIVERYORDER);
                        if(saldespatchhdrList!=null && saldespatchhdrList.Count>0)
                        {
                            SetGODetails(saldespatchhdrList[0]);
                        }
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
                ContainerInspectionServiceClient = null;
                CommonServiceClient = null;
            }
        }

       
        #endregion

       

       

        #endregion

        #region Helper Methods


        /// <summary>
        /// Check Delivery Order
        /// </summary>
        /// <returns></returns>    
        private void CheckDeliveryOrder()
        {
            if (saldespatchhdrList == null || saldespatchhdrList.Count == 0)
            {
                uclCheckList.CLType = 0;
                uclCheckList.CLTypeCode = string.Empty;
                //Show Save success message and reset Contract Entry
                litErrorMsg.Text = Resources.ErrorMessages.Msg_DeliveryOrder;
                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
            }
            else if (saldespatchhdrList != null && saldespatchhdrList.Count > 0)
            {
                bool flag = false;
                foreach (SAL_DESPATCH_HDR item in saldespatchhdrList)
                {
                    if (item.DPH_PK == DESPATCHPK)
                    {
                        flag = true;
                        SetGODetails(item);
                        txtDespatchNumber.Enabled = false;
                        EntryStatus = EntryStatus.ENTRYMODE;
                        break;
                    }
                }
                if (!flag)
                {
                    uclCheckList.CLType = 0;
                    uclCheckList.CLTypeCode = string.Empty;
                    //Show Save success message and reset Contract Entry
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_InvalidDO;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);

                }
            }
        }

        private void SetGODetails(SAL_DESPATCH_HDR item)
        {
            txtDespatchNumber.Text = item.DPH_NO;
            hdfDPHPK.Value = item.DPH_PK.ToString();
            txtContainerNo.Text = item.DPH_CONTAINER_NO;
            txtSerialNo.Text = item.DPH_SEAL_NO;
            txtContainerNo.Focus();
        }

        /// <summary>
        /// Bind Dropdown
        /// </summary>
        /// <returns></returns>    
        private void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.DELIVERYORDER:
                       
                            //ddlDeliveryNumber.Items.Clear();
                            //if (saldespatchhdrList != null && saldespatchhdrList.Count > 0)
                            //{
                            //    ddlDeliveryNumber.DataSource = saldespatchhdrList;
                            //    ddlDeliveryNumber.DataTextField = Resources.DataFieldRes.DeliveryOrderNo;
                            //    ddlDeliveryNumber.DataValueField = Resources.DataFieldRes.DeliveryOrderPK;
                            //    ddlDeliveryNumber.DataBind();
                            //}
                            //ddlDeliveryNumber.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        
                       
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private SAL_CONTAINER_INSP_HDR SetUIValuesToObject(ActionsEnum Actions)
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                SalContainerInspHdrObj.CSH_PK = CurrPK;
                SalContainerInspHdrObj.CSH_NO = ContainerInspectionNo;
                SalContainerInspHdrObj.CSH_DATE = txtDate.Text.Trim() != string.Empty ? Convert.ToDateTime(txtDate.Text.Trim()) : DateTime.Now;
                SalContainerInspHdrObj.CSH_CONTAINER_NO = HttpUtility.HtmlEncode(txtContainerNo.Text.Trim());
                SalContainerInspHdrObj.CSH_SEAL_NO = HttpUtility.HtmlEncode(txtSerialNo.Text.Trim());
                SalContainerInspHdrObj.CSH_ISO_PAS = HttpUtility.HtmlEncode(txtISOPAS.Text.Trim());
                int DPk = hdfDPHPK.Value == "" ? 0 : Convert.ToInt32(hdfDPHPK.Value);
                if (DPk==0)
                {
                    SalContainerInspHdrObj.CSH_DESPATCH=null;
                }
                else
                {
                    SalContainerInspHdrObj.CSH_DESPATCH = DPk;
                }
                SalContainerInspHdrObj.CSH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                SalContainerInspHdrObj.CSH_CHECK_LIST_GROUP = uclCheckList.CLGroup;
                SalContainerInspHdrObj.CSH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                SalContainerInspHdrObj.CSH_BIZUNIT = currentUser.SBUID;
                SalContainerInspHdrObj.CSH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                SalContainerInspHdrObj.CSH_CRTD_DT = DateTime.Now;
                SalContainerInspHdrObj.CSH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                SalContainerInspHdrObj.CSH_MOD_DT = LastModifiedTime;
                switch (commonActions)
                {

                    case ActionsEnum.SAVE:
                        //SalContainerInspHdrObj.CSH_STATUS = (int)WorkFlowStatus.DRAFT;
                        //SalContainerInspHdrObj.CSH_SUPERVISOR_NAME = string.Empty;
                        //SalContainerInspHdrObj.CSH_INSPECTOR_NAME = string.Empty;                        
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //SalContainerInspHdrObj.CSH_STATUS = (int)WorkFlowStatus.APPROVED;
                        //SalContainerInspHdrObj.CSH_SUPERVISOR = Convert.ToInt16(currentUser.PKUser);
                        //SalContainerInspHdrObj.CSH_SUPERVISOR_NAME=currentUser.UserName;
                        //SalContainerInspHdrObj.CSH_SUPERVISED_ON = DateTime.Now;
                        //SalContainerInspHdrObj.CSH_INSPECTOR = Convert.ToInt16(currentUser.PKUser);
                        //SalContainerInspHdrObj.CSH_INSPECTOR_NAME=currentUser.UserName;
                        //SalContainerInspHdrObj.CSH_INSPECTED_ON = DateTime.Now;
                        break;
                }
                return SalContainerInspHdrObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SalContainerInspHdrObj = null;
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

                if (SalContainerInspHdrList != null && SalContainerInspHdrList.Count > 0)
                {
                    CurrPK = SalContainerInspHdrList[0].CSH_PK;
                    hdfStatus.Value = SalContainerInspHdrList[0].CSH_STATUS.ToString();
                    lblInspectionNoTxt.Text = ((SalContainerInspHdrList[0].CSH_NO == null || SalContainerInspHdrList[0].CSH_NO == "") ? Resources.Messages.DocGenerationNew : SalContainerInspHdrList[0].CSH_NO);
                    hdfInspectionNo.Value = (SalContainerInspHdrList[0].CSH_NO == null ? string.Empty : SalContainerInspHdrList[0].CSH_NO);
                    //DESPATCHPK = (SalContainerInspHdrList[0].CSH_DESPATCH != null ? (int)SalContainerInspHdrList[0].CSH_DESPATCH : 0);
                    txtDate.Text = SalContainerInspHdrList[0].CSH_DATE.ToString(Resources.ErpRes.DateFormatShort);
                    txtContainerNo.Text = HttpUtility.HtmlDecode(SalContainerInspHdrList[0].CSH_CONTAINER_NO);
                    txtSerialNo.Text = HttpUtility.HtmlDecode(SalContainerInspHdrList[0].CSH_SEAL_NO);
                    txtISOPAS.Text = HttpUtility.HtmlDecode(SalContainerInspHdrList[0].CSH_ISO_PAS);
                    txtRemarks.Text = HttpUtility.HtmlDecode(SalContainerInspHdrList[0].CSH_REMARKS);
                    GroupPK = (int)SalContainerInspHdrList[0].CSH_CHECK_LIST_GROUP;
                    uclCheckList.CLGroup = (int)SalContainerInspHdrList[0].CSH_CHECK_LIST_GROUP;
                    //ddlDeliveryNumber.SelectedValue = SalContainerInspHdrList[0].CSH_DESPATCH != null ? SalContainerInspHdrList[0].CSH_DESPATCH.ToString() : CommonConstants.SELECTVAL;
                    //ddlDeliveryNumber.Enabled = false;
                    txtDespatchNumber.Text = SalContainerInspHdrList[0].SAL_DESPATCH_HDR.DPH_NO;
                    hdfDPHPK.Value = SalContainerInspHdrList[0].CSH_DESPATCH.ToString();
                    txtDespatchNumber.Enabled = false;
                    ModifiedDatePnl.Visible = true;
                    LastModifiedTime = SalContainerInspHdrList[0].CSH_MOD_DT;
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);


                }
                else
                {
                    //Show Concurrency and bind Listing if Query yield no results
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                     + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionList) + "');", true);
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
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.CNTINSP, 0, DateTime.Now);
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
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.CNTINSP, 0, DateTime.Now);
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
            Session[ERP.Utilities.SessionStrings.ContainerInspectionPK] = null;
            Session[ERP.Utilities.SessionStrings.ContainerInspectionMode] = null;
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
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
            CONTAINERINSPECTION,
            TYPE,
            CONTROL,
            GROUP,
            CHECKLISTDATA,
            CONTAINERINSPECTIONNO,
            DELIVERYORDER
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