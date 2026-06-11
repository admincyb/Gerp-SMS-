using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using System.Data;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using System.Threading;
using System.Text;
using System.IO;
using BusinessLogic.CommonManagement;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject;
using BusinessObject.HRMS.Payroll;
using BusinessObject.Common;
using BusinessObject.AssetService;
using BusinessLogic.AssetService;

namespace ERPSMS_v01.AssetService
{
    public partial class AssetServiceRequest : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region  Properties
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
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
            }
        }
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
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.TotalPages] ?? 1);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }

        /// <summary>
        /// To keep RowIndex in view state
        /// </summary>
        private int RowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndex] == null ? -1 : (int)this.ViewState[ViewstateStrings.RowIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndex] = value;
            }
        }
        private int ItemRowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.ItemRowIndex] == null ? -1 : (int)this.ViewState[ViewstateStrings.ItemRowIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.ItemRowIndex] = value;
            }
        }

        /// <summary>
        /// To keep popup RowIndex in view state
        /// </summary>
        private int RowIndexPopup
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndexPopup] == null ? -1 : (int)this.ViewState[ViewstateStrings.RowIndexPopup];
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndexPopup] = value;
            }
        }

        /// <summary>
        /// To keep popup display mode
        /// </summary>
        private bool PopupViewMode
        {
            get
            {
                return this.ViewState[ViewstateStrings.PopupViewMode] == null ? false : (bool)this.ViewState[ViewstateStrings.PopupViewMode];
            }
            set
            {
                this.ViewState[ViewstateStrings.PopupViewMode] = value;
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
        /// Approved
        /// </summary>
        private int Approved
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.Approved]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Approved] = value;
            }
        }

        private int Status
        {
            get
            {
                return this.ViewState[ViewstateStrings.Status] == null ? 0 : Convert.ToInt32((this.ViewState[ViewstateStrings.Status]));
            }
            set
            {
                this.ViewState[ViewstateStrings.Status] = value;
            }
        }
        /// <summary>
        /// Duty slip Detail PK
        /// </summary>
        private int SRDPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.PoDtlPK]);//[SRD_PK]
            }
            set
            {
                this.ViewState[ViewstateStrings.PoDtlPK] = value;
            }
        }
        /// <summary>
        /// To keep details in view state
        /// </summary>
        private List<AssetServiceRequestDetails> AssetServiceRequestDetailList
        {
            get
            {
                return (List<AssetServiceRequestDetails>)ViewState[ViewstateStrings.AssetServiceRequestDetailList];
            }
            set
            {
                ViewState[ViewstateStrings.AssetServiceRequestDetailList] = value;
            }
        }
        private int CurrSlNo
        {
            get
            {
                return ViewState[ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ViewstateStrings.CurrSlNo] = value;
            }
        }
        private int SID_SL_NO
        {
            get
            {
                return ViewState[ViewstateStrings.CurrDocSlNo] == null ? 0 : (int)ViewState[ViewstateStrings.CurrDocSlNo];
            }
            set
            {
                ViewState[ViewstateStrings.CurrDocSlNo] = value;
            }
        }

        #endregion

        #region  Variables
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataTable dtDept;

        private string strResult = string.Empty;
        private string refID;
        private string inboxFlag;
        private string prefID;
        private int CompanyPk = 0;
        private int dptType;
        DataSet dsPageData;
        DataTable dtPageData;
        private AssetServiceRequestHeader objAssetServiceRequestHeader;
        private AssetServiceRequestDetails assetServiceRequestDetailsObj;
        private AssetServiceRequestItemDetails assetServiceRequestItemDetObj;
        List<AssetServiceRequestItemDetails> assetServiceRequestItemDetList;
        GridViewRow grvRow;
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
            try
            {
                if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                {
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                    hdfJournalizeWorkFlow.Value = "0";
                }

                ucrWrkf.ViewType = 1;
                // InitializeComponent();
                if (!IsPostBack)
                {

                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;

                    int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < NoDecimalDigitsP2P; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithSeperation.Value += "0";
                    }
                    hdfNumberDigits.Value = NoDecimalDigitsP2P.ToString();

                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }
                    hdfExchangeRateFormat.Value = "#0.";
                    int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchrateDecimalDigits; i++)
                    {
                        hdfExchangeRateFormat.Value += "0";
                    }

                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                       : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    ReferanceID = string.IsNullOrEmpty(refID)
                           ? string.IsNullOrEmpty(prefID)
                                 ? 0
                                 : int.Parse(prefID)
                           : int.Parse(refID);
                    FillProcessID(1);

                    hdfAdvSearch.Value = "0";

                    string[] datakeyarray;
                    datakeyarray = new string[2];
                    datakeyarray[0] = Resources.DataFieldRes.SRD_PK;
                    datakeyarray[1] = "SRD_SL_NO";
                    grdAssetDetails.DataKeyNames = datakeyarray;

                    AssetServiceRequestDetailList = new List<AssetServiceRequestDetails>();

                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.CURRENCY);
                    SetFieldValues(ControlsEnum.CURRENCY);
                    GetFieldValues(ControlsEnum.REQUESTINGSTORE);
                    SetFieldValues(ControlsEnum.REQUESTINGSTORE);
                    GetFieldValues(ControlsEnum.ASSETSERVICETYPE);
                    SetFieldValues(ControlsEnum.ASSETSERVICETYPE);
                    ddlRequestingStore.SelectedValue = currentUser.CurrentDeptPK.ToString();
                    #region Work Flow
                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        ////start
                        if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                        {
                            ucrWrkf.RefID = int.Parse(refID);
                            base.WkfRefID = ucrWrkf.RefID;
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            if (pid.Equals("11"))
                                hdfIsCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                        }
                    }
                    else if (!string.IsNullOrEmpty(prefID))
                    {
                        ResetForm(ControlsEnum.CLEAR);
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                        GetFieldValues(ControlsEnum.DEPARTMENTBYTYPE);
                        if (dtDept != null && dtDept.Rows.Count > 0)
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = Convert.ToInt32(dtDept.Rows[0]["DPT_PK"]);
                            base.SetUserDept();
                        }
                    }
                    if (CurrPK > 0)
                    {
                        EntryStatus = EntryStatus.ENTRYMODE;
                        FillProcessID(1);
                        WorkflowCore.CoreService objWorkflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = objWorkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        SetUIEditView(commonActions);
                        GetFieldValues(ControlsEnum.EDIT);
                        SetFieldValues(ControlsEnum.EDIT);
                        ResetForm(ControlsEnum.CLEARADD);
                    }
                    else
                    {
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion

        #region Helper Methods
        #region Get Field Values
        private void GetFieldValues(ControlsEnum type)
        {
            BusinessObject.GridPrams gridParam;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            string pageUrl = string.Empty;
            try
            {
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.SearchBy = string.Empty;
                        gridParam.SearchValue = string.Empty;
                        gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        gridParam.FromDate = string.IsNullOrEmpty(txtListFromDate.Text) ? string.Empty : txtListFromDate.Text.Trim();
                        gridParam.ToDate = string.IsNullOrEmpty(txtListToDate.Text) ? string.Empty : txtListToDate.Text.Trim();
                        gridParam.FilterStatus = ddlStatus.SelectedValue == "-1" ? string.Empty : ddlStatus.SelectedValue;

                        string trxNo = txtListTrxNo.Text.Trim() == "Select/Type" ? string.Empty : txtListTrxNo.Text.Trim();
                        int VendorPK = txtListVendorName.Text.Trim() == "Select/Type" || txtListVendorName.Text.Trim() == string.Empty ? 0 : Convert.ToInt32(hdfListVendor.Value);
                        int serviceType = string.IsNullOrEmpty(ddlListServiceType.SelectedValue) ? 0 : Convert.ToInt32(ddlListServiceType.SelectedValue);
                        dtResult = BusinessLogic.AssetService.ServiceRequestBL.GetServiceRequestList(gridParam, currentUser, trxNo, VendorPK, serviceType);
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        dtResult = CommonBL.GetCurrency(0, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region REQUESTINGSTORE
                    case ControlsEnum.REQUESTINGSTORE:
                         dtPageData = BusinessLogic.AssetService.ServiceRequestBL.GetRequestingStores(currentUser, currentUser.SBUID, 0, 0);
                        break;
                    #endregion
                    #region ASSETSERVICETYPE
                    case ControlsEnum.ASSETSERVICETYPE:
                        dtPageData = BusinessLogic.AssetService.ServiceRequestBL.GetAssetServiceType(currentUser, 0, 1);
                        break;
                    #endregion
                    #region Edit
                    case ControlsEnum.EDIT:
                        objAssetServiceRequestHeader = ServiceRequestBL.GetServiceRequestByPK(CurrPK);
                        break;
                    #endregion
                    #region ITEMDETAILS
                    case ControlsEnum.ITEMDETAILS:
                        int itemPk = string.IsNullOrEmpty(hdfPopupItemPk.Value) ? 0 : Convert.ToInt32(hdfPopupItemPk.Value);
                        dtPageData = CommonBL.GetAssetItemsAuto(string.Empty, 0, itemPk, 2, currentUser.SBUID);
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion
        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        BindDropDown(ControlsEnum.CURRENCY);
                        break;
                    #endregion
                    case ControlsEnum.REQUESTINGSTORE:
                        BindDropDown(ControlsEnum.REQUESTINGSTORE);
                        break;
                    case ControlsEnum.ASSETSERVICETYPE:
                        BindDropDown(ControlsEnum.ASSETSERVICETYPE);
                        break;
                    case ControlsEnum.ASSETSERVICEREQUESTDTL:
                        BindGrid(ControlsEnum.ASSETSERVICEREQUESTDTL);
                        break;
                    case ControlsEnum.ASSETSERVICEREQUEST_ITEMDTL:
                        BindGrid(ControlsEnum.ASSETSERVICEREQUEST_ITEMDTL);
                        break;
                    case ControlsEnum.ITEMDETAILS:
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtPopupUOM.Text = dtPageData.Rows[0]["ATM_UOM_TEXT"].ToString();
                            hdfPopupUomPk.Value = dtPageData.Rows[0]["ATM_UOM"].ToString();
                        }
                        break;
                    #region EDIT
                    case ControlsEnum.EDIT:
                        GetUIValuesFromObject(ControlsEnum.EDIT);
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Set UIValues To Object
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region ASSETSERVICEREQUESTHDR
                    case ControlsEnum.ASSETSERVICEREQUESTHDR:
                        objAssetServiceRequestHeader.SRH_PK = CurrPK;
                        objAssetServiceRequestHeader.SRH_NO = lblTrxNo.Text;
                        objAssetServiceRequestHeader.SRH_DATE = Convert.ToDateTime(txtDate.Text);
                        objAssetServiceRequestHeader.SRH_REF_NO = HttpUtility.HtmlEncode(txtRefNO.Text);
                        if (String.IsNullOrEmpty(txtRefDate.Text.Trim()))
                            objAssetServiceRequestHeader.SRH_REF_DATE = null;
                        else
                            objAssetServiceRequestHeader.SRH_REF_DATE = txtRefDate.Text.Trim();
                        objAssetServiceRequestHeader.SRH_VENDOR = String.IsNullOrEmpty(hdfVendor.Value) ? 0 : Convert.ToInt32(hdfVendor.Value);
                        objAssetServiceRequestHeader.SRH_SERVICE_TYPE = String.IsNullOrEmpty(ddlServiceType.SelectedValue) ? 0 : Convert.ToInt32(ddlServiceType.SelectedValue);
                        objAssetServiceRequestHeader.SRH_REMARKS = HttpUtility.HtmlEncode(txtWorkDescription.Text);
                        objAssetServiceRequestHeader.SRH_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        objAssetServiceRequestHeader.SRH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        objAssetServiceRequestHeader.SRH_DEPT_STORE = Convert.ToInt16(ddlRequestingStore.SelectedValue);
                        objAssetServiceRequestHeader.SRH_CURRENCY = Convert.ToInt32(ddlCurrency.SelectedValue);
                        objAssetServiceRequestHeader.SRH_BIZUNIT = Convert.ToInt32(currentUser.SBUID);
                        objAssetServiceRequestHeader.USER_PK = Convert.ToInt32(currentUser.PKUser);
                        objAssetServiceRequestHeader.SRH_CRTD_BY = Convert.ToInt32(currentUser.PKUser);
                        objAssetServiceRequestHeader.SRH_CRTD_DT = DateTime.Now;
                        objAssetServiceRequestHeader.LAST_MOD_DT = LastModifiedTime;

                        AssetServiceRequestDetailList.ForEach(dtl =>
                        {
                            if (dtl.Item_details != null && dtl.Item_details.Count > 0)
                            {
                                dtl.SRD_AMOUNT = dtl.Item_details.Sum(r => r.SID_AMOUNT);
                            }
                        });
                        objAssetServiceRequestHeader.Details = AssetServiceRequestDetailList;
                        retObject = objAssetServiceRequestHeader;
                        break;
                    #endregion
                    #region ASSETSERVICEREQUESTDTL
                    case ControlsEnum.ASSETSERVICEREQUESTDTL:
                        if (CurrSlNo != 0 && AssetServiceRequestDetailList != null)
                        {
                            assetServiceRequestDetailsObj = AssetServiceRequestDetailList.SingleOrDefault(itm => itm.SRD_SL_NO == CurrSlNo);
                            if (assetServiceRequestDetailsObj != null)
                            {
                                assetServiceRequestDetailsObj.SRD_PK = SRDPK;
                                assetServiceRequestDetailsObj.SRD_SRH_HDR = CurrPK;
                                assetServiceRequestDetailsObj.SRD_ASSET_TYPE = Convert.ToInt32(hdfAssetType.Value);
                                assetServiceRequestDetailsObj.SRD_ASSET_TYPE_TEXT = HttpUtility.HtmlEncode(txtAssetType.Text);
                                assetServiceRequestDetailsObj.SRD_ASSET = Convert.ToInt32(hdfAsset.Value);
                                assetServiceRequestDetailsObj.SRD_ASSET_TEXT = HttpUtility.HtmlEncode(txtAsset.Text);
                                assetServiceRequestDetailsObj.SRD_DESC = HttpUtility.HtmlEncode(txtDescription.Text);
                                assetServiceRequestDetailsObj.SRD_SRH_NO = lblTrxNo.Text;
                                assetServiceRequestDetailsObj.SRD_SRH_DATE = Convert.ToDateTime(txtDate.Text);
                                assetServiceRequestDetailsObj.SRD_ACTIVE = 1;

                                retObject = assetServiceRequestDetailsObj;
                            }
                        }
                        else
                        {
                            int slno = 1;
                            if (AssetServiceRequestDetailList == null || AssetServiceRequestDetailList.Count == 0)
                            {
                                AssetServiceRequestDetailList = new List<AssetServiceRequestDetails>();
                                slno = 1;
                            }
                            else
                            {
                                slno = AssetServiceRequestDetailList.Max(itm => itm.SRD_SL_NO);
                                slno++;
                            }
                            assetServiceRequestDetailsObj = new AssetServiceRequestDetails();
                            CurrSlNo = assetServiceRequestDetailsObj.SRD_SL_NO = slno;
                            assetServiceRequestDetailsObj.SRD_SRH_HDR = CurrPK;
                            assetServiceRequestDetailsObj.SRD_PK = 0;
                            assetServiceRequestDetailsObj.SRD_ASSET_TYPE = Convert.ToInt32(hdfAssetType.Value);
                            assetServiceRequestDetailsObj.SRD_ASSET_TYPE_TEXT = HttpUtility.HtmlEncode(txtAssetType.Text);
                            assetServiceRequestDetailsObj.SRD_ASSET = Convert.ToInt32(hdfAsset.Value);
                            assetServiceRequestDetailsObj.SRD_ASSET_TEXT = HttpUtility.HtmlEncode(txtAsset.Text);
                            assetServiceRequestDetailsObj.SRD_DESC = HttpUtility.HtmlEncode(txtDescription.Text);
                            assetServiceRequestDetailsObj.SRD_SRH_NO = lblTrxNo.Text;
                            assetServiceRequestDetailsObj.SRD_SRH_DATE = Convert.ToDateTime(txtDate.Text);
                            assetServiceRequestDetailsObj.SRD_ACTIVE = 1;

                            AssetServiceRequestDetailList.Add(assetServiceRequestDetailsObj);
                        }
                        retObject = AssetServiceRequestDetailList;
                        break;
                    #endregion
                    #region ASSETSERVICEREQUEST_ITEMDTL
                    case ControlsEnum.ASSETSERVICEREQUEST_ITEMDTL:
                        if (AssetServiceRequestDetailList != null)
                        {
                            if (CurrSlNo != 0)
                            {
                                AssetServiceRequestItemDetails ItemDetailsObj;
                                AssetServiceRequestDetails assetServiceRequestDetailsObj = AssetServiceRequestDetailList.SingleOrDefault(itm => itm.SRD_SL_NO == CurrSlNo);
                                if (assetServiceRequestDetailsObj.Item_details == null)
                                {
                                    assetServiceRequestDetailsObj.Item_details = new List<AssetServiceRequestItemDetails>();
                                }
                                if (assetServiceRequestDetailsObj.Item_details.Count > 0 && ItemRowIndex >= 0)
                                {
                                    ItemDetailsObj = assetServiceRequestDetailsObj.Item_details[ItemRowIndex];
                                    ItemDetailsObj.SID_ASR_ITEM = Convert.ToInt32(hdfPopupItemPk.Value) > 0 ? Convert.ToInt32(hdfPopupItemPk.Value) : 0;
                                    ItemDetailsObj.SID_ASR_ITEM_TEXT = txtPopupItems.Text.Trim();
                                    ItemDetailsObj.SID_QTY = string.IsNullOrEmpty(txtPopupQty.Text) ? 0 : Convert.ToDouble(txtPopupQty.Text);
                                    ItemDetailsObj.SID_RATE = string.IsNullOrEmpty(txtPopupApproxRate.Text) ? 0 : Convert.ToDouble(txtPopupApproxRate.Text);
                                    ItemDetailsObj.SID_UOM = string.IsNullOrEmpty(hdfPopupUomPk.Value) ? 0 : Convert.ToInt32(hdfPopupUomPk.Value);
                                    ItemDetailsObj.SID_UOM_TEXT = txtPopupUOM.Text;
                                    ItemDetailsObj.SID_AMOUNT = string.IsNullOrEmpty(txtPopupAmount.Text) ? 0 : Convert.ToDouble(txtPopupAmount.Text);
                                    ItemDetailsObj.SID_REMARKS = HttpUtility.HtmlEncode(txtPopupRemarks.Text);
                                }
                                else
                                {
                                    ItemDetailsObj = new AssetServiceRequestItemDetails();
                                    ItemDetailsObj.SID_PK = 0;
                                    ItemDetailsObj.SID_SL_NO = CurrSlNo;
                                    ItemDetailsObj.SID_SRD_PK = SRDPK;
                                    ItemDetailsObj.SID_ASR_ITEM = Convert.ToInt32(hdfPopupItemPk.Value) > 0 ? Convert.ToInt32(hdfPopupItemPk.Value) : 0;
                                    ItemDetailsObj.SID_ASR_ITEM_TEXT = txtPopupItems.Text;
                                    ItemDetailsObj.SID_QTY = string.IsNullOrEmpty(txtPopupQty.Text) ? 0 : Convert.ToDouble(txtPopupQty.Text);
                                    ItemDetailsObj.SID_RATE = string.IsNullOrEmpty(txtPopupApproxRate.Text) ? 0 : Convert.ToDouble(txtPopupApproxRate.Text);
                                    ItemDetailsObj.SID_UOM = string.IsNullOrEmpty(hdfPopupUomPk.Value) ? 0 : Convert.ToInt32(hdfPopupUomPk.Value);
                                    ItemDetailsObj.SID_UOM_TEXT = txtPopupUOM.Text;
                                    ItemDetailsObj.SID_AMOUNT = string.IsNullOrEmpty(txtPopupAmount.Text) ? 0 : Convert.ToDouble(txtPopupAmount.Text);
                                    ItemDetailsObj.SID_REMARKS = HttpUtility.HtmlEncode(txtPopupRemarks.Text);
                                    ItemDetailsObj.SID_ACTIVE = 1;

                                    assetServiceRequestDetailsObj.Item_details.Add(ItemDetailsObj);
                                }
                            }
                        }
                        retObject = AssetServiceRequestDetailList;
                        break;
                    #endregion
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
        #endregion
        #region Get UIValues From Object
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EDIT
                    case ControlsEnum.EDIT:
                        if (objAssetServiceRequestHeader != null)
                        {
                            txtDate.Text = Convert.ToDateTime(objAssetServiceRequestHeader.SRH_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            lblTrxNo.Text = string.IsNullOrEmpty(objAssetServiceRequestHeader.SRH_NO) ? Resources.ErpRes.Draft : objAssetServiceRequestHeader.SRH_NO;
                            txtRefNO.Text = string.IsNullOrEmpty(objAssetServiceRequestHeader.SRH_REF_NO) ? string.Empty : objAssetServiceRequestHeader.SRH_REF_NO;
                            txtRefDate.Text = string.IsNullOrEmpty(objAssetServiceRequestHeader.SRH_REF_DATE) ? string.Empty : Convert.ToDateTime(objAssetServiceRequestHeader.SRH_REF_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            hdfVendor.Value = objAssetServiceRequestHeader.SRH_VENDOR.ToString();
                            txtVendorName.Text = HttpUtility.HtmlDecode(objAssetServiceRequestHeader.SRH_VENDOR_TEXT.ToString());
                            ddlServiceType.SelectedValue = objAssetServiceRequestHeader.SRH_SERVICE_TYPE.ToString();
                            ddlRequestingStore.SelectedValue = objAssetServiceRequestHeader.SRH_DEPT_STORE.ToString();
                            ddlCurrency.SelectedValue = objAssetServiceRequestHeader.SRH_CURRENCY.ToString();
                            txtWorkDescription.Text = HttpUtility.HtmlDecode(objAssetServiceRequestHeader.SRH_REMARKS);
                            LastModifiedTime = objAssetServiceRequestHeader.SRH_MOD_DT;
                            Status = objAssetServiceRequestHeader.SRH_STATUS;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            hdfIsCancelled.Value = Convert.ToString(objAssetServiceRequestHeader.SRH_DEL_STATUS);
                            AssetServiceRequestDetailList = objAssetServiceRequestHeader.Details;
                            SetFieldValues(ControlsEnum.ASSETSERVICEREQUESTDTL);
                        }
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            //txtCurrency.Text = string.Format(GetLocalResourceObject("CurrencyDisplayFormat").ToString(), Convert.ToString(dtResult.Rows[0]["CUR_CODE"]), Convert.ToString(dtResult.Rows[0]["CUR_NAME"]));
                            //hdfCurrency.Value = Convert.ToString(dtResult.Rows[0]["CUR_PK"]);
                        }
                        break;
                    #endregion
                    case ControlsEnum.FILLASSETDETAILS:
                        if (assetServiceRequestDetailsObj != null)
                        {

                            CurrSlNo = assetServiceRequestDetailsObj.SRD_SL_NO;
                            txtAsset.Text = HttpUtility.HtmlDecode(assetServiceRequestDetailsObj.SRD_ASSET_TEXT);
                            hdfAsset.Value = assetServiceRequestDetailsObj.SRD_ASSET.ToString();
                            txtAssetType.Text = HttpUtility.HtmlDecode(assetServiceRequestDetailsObj.SRD_ASSET_TYPE_TEXT);
                            hdfAssetType.Value = assetServiceRequestDetailsObj.SRD_ASSET_TYPE.ToString();
                            txtDescription.Text = HttpUtility.HtmlDecode(assetServiceRequestDetailsObj.SRD_DESC);
                        }
                        break;
                    case ControlsEnum.FILLITEMDETAILS:
                        if (assetServiceRequestItemDetObj != null)
                        {
                            SID_SL_NO = assetServiceRequestItemDetObj.SID_SL_NO;
                            txtPopupItems.Text = HttpUtility.HtmlDecode(assetServiceRequestItemDetObj.SID_ASR_ITEM_TEXT);
                            hdfPopupItemPk.Value = assetServiceRequestItemDetObj.SID_ASR_ITEM.ToString();
                            txtPopupQty.Text = assetServiceRequestItemDetObj.SID_QTY.ToString();
                            txtPopupApproxRate.Text = assetServiceRequestItemDetObj.SID_RATE.ToString();
                            txtPopupAmount.Text = assetServiceRequestItemDetObj.SID_AMOUNT.ToString();
                            txtPopupAmount.Text = assetServiceRequestItemDetObj.SID_AMOUNT.ToString();
                            txtPopupUOM.Text = assetServiceRequestItemDetObj.SID_UOM_TEXT.ToString();
                            hdfPopupUomPk.Value = assetServiceRequestItemDetObj.SID_UOM.ToString();
                            txtPopupRemarks.Text = assetServiceRequestItemDetObj.SID_REMARKS;
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Bind DropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {

                #region CURRENCY
                case ControlsEnum.CURRENCY:
                    ddlCurrency.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlCurrency.DataValueField = GTIService.Constants.Common.Fields.CURRENCYID;
                        ddlCurrency.DataTextField = GTIService.Constants.Common.Fields.CURRENCYCODE;
                        ddlCurrency.DataSource = dtResult;
                        ddlCurrency.DataBind();
                    }
                    ddlCurrency.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlCurrency.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    break;
                #endregion
                #region RECIEVINGSTORE
                case ControlsEnum.REQUESTINGSTORE:
                    ddlRequestingStore.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlRequestingStore.DataValueField = GTIService.Constants.DirectStockTransfer.Fields.DPT_PK;
                        ddlRequestingStore.DataTextField = GTIService.Constants.DirectStockTransfer.Fields.DPT_NAME;
                        ddlRequestingStore.DataSource = dtPageData;
                        ddlRequestingStore.DataBind();
                    }
                    ddlRequestingStore.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlRequestingStore.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    break;
                #endregion
                #region ASSETSERVICETYPE
                case ControlsEnum.ASSETSERVICETYPE:
                    ddlServiceType.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlServiceType.DataValueField = GTIService.Constants.AssetService.Fields.VTP_PK;
                        ddlServiceType.DataTextField = GTIService.Constants.AssetService.Fields.VTP_Name;
                        ddlServiceType.DataSource = dtPageData;
                        ddlServiceType.DataBind();
                    }
                    ddlServiceType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlServiceType.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    #region Listing ServiceType
                    ddlListServiceType.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlListServiceType.DataValueField = GTIService.Constants.AssetService.Fields.VTP_PK;
                        ddlListServiceType.DataTextField = GTIService.Constants.AssetService.Fields.VTP_Name;
                        ddlListServiceType.DataSource = dtPageData;
                        ddlListServiceType.DataBind();
                    }
                    ddlListServiceType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlListServiceType.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    #endregion
                    break;
                #endregion
                default:
                    break;
            }
        }
        #endregion
        #region BindGrid
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dtResult.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dtResult;
                            grdList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdList.DataSource = null;
                            grdList.DataBind();
                        }
                        break;
                    #endregion
                    #region ASSETSERVICEREQUESTDTL
                    case ControlsEnum.ASSETSERVICEREQUESTDTL:
                        grdAssetDetails.DataSource = AssetServiceRequestDetailList;
                        grdAssetDetails.DataBind();
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Reset Form
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                # region CLEARSERACH
                case ControlsEnum.CLEARSEARCH:
                    CurrPK = 0;
                    txtListFromDate.Text = string.Empty;
                    txtListToDate.Text = string.Empty;
                    txtListTrxNo.Text = string.Empty;
                    txtListVendorName.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfListVendor.Value = string.Empty;
                    ddlListServiceType.SelectedValue = "0";
                    ddlStatus.SelectedValue = "-1";
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;

                    Status = 0;
                    break;
                #endregion
                # region CLEAR
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    txtListTrxNo.Text = string.Empty;
                    hdfTrxPk.Value = string.Empty;
                    txtVendorName.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfVendor.Value = string.Empty;
                    ddlRequestingStore.SelectedValue = "0";
                    ddlServiceType.SelectedValue = "0";
                    ddlCurrency.SelectedValue = "0";
                    txtWorkDescription.Text = string.Empty;
                    Status = 0;
                    base.WkfRefID = 0;
                    hdfSelRecordStatus.Value = "0";
                    AssetServiceRequestDetailList = null;
                    txtRefNO.Text = txtRefDate.Text = string.Empty;
                    ResetForm(ControlsEnum.CLEARADD);
                    ResetForm(ControlsEnum.CLEARPOPUPDETAILS);

                    break;
                #endregion
                # region CLEAR ADD
                case ControlsEnum.CLEARADD:
                    txtAssetType.Text = string.Empty;
                    hdfAssetType.Value = string.Empty;
                    txtDescription.Text = string.Empty;
                    txtAsset.Text = string.Empty;
                    hdfAsset.Value = string.Empty;
                    CurrSlNo = 0;
                    SID_SL_NO = 0;
                    RowIndex = -1;
                    break;
                #endregion
                # region CLEAR POPUPDETAILS
                case ControlsEnum.CLEARPOPUPDETAILS:
                    hdfPopupItemPk.Value = string.Empty;
                    txtPopupItems.Text = string.Empty;
                    txtPopupQty.Text = string.Empty;
                    txtPopupApproxRate.Text = string.Empty;
                    txtPopupUOM.Text = string.Empty;
                    hdfPopupUomPk.Value = string.Empty;
                    txtPopupAmount.Text = string.Empty;
                    txtPopupRemarks.Text = string.Empty;
                    CurrSlNo = 0;
                    SID_SL_NO = 0;
                    ItemRowIndex = -1;
                    break;
                #endregion
            }
        }
        #endregion
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
        /// <summary>
        /// 
        #endregion

        #region EnableDisableButtons
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the first link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the previous link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false; // Should we enable the next link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;// Should we enable the last link
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

                int? result;
                bool bIsChecked = false;
                DropDownList ddlWkfAction;
                TextBox WrkfComments;
                string TrxNo = string.Empty;
                string action;
                int grdListRowDeptId = 0;

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.LISTITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    #region Grid Item Selected
                    case ActionsEnum.LISTITEMSELECTED:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            HiddenField hdfDept;
                            HiddenField hdfDelStatus;
                            HiddenField hdfStatus;
                            int selectedPK;
                            int dept;

                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSRH_PK")).Value);
                                hdfDelStatus = grdrow.FindControl("hdfDelStatus") as HiddenField;
                                hdfStatus = grdrow.FindControl("hdfStatus") as HiddenField;
                                hdfSelRecordStatus.Value = hdfStatus.Value.ToString();//For btnEditforCancel show/Hide
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = workflowCore.GetRefID(selectedPK, PageProcessID);
                                if (Convert.ToInt32(hdfDelStatus.Value) == 1)
                                {
                                    btnSave.Visible = false;
                                    btnEditforCancel.Visible = false;
                                    btnEdit.Visible = false;
                                }
                                else
                                {
                                    btnEditforCancel.Visible = true;
                                }
                                if (Convert.ToInt32(hdfStatus.Value) == 0)
                                {
                                    btnEditforCancel.Visible = false;
                                }
                                break;
                            }
                        }
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        ResetForm(ControlsEnum.CLEAR);
                        ResetForm(ControlsEnum.CLEARADD);
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        SetFieldValues(ControlsEnum.ASSETSERVICEREQUESTDTL);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        //SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        hdfIsCancelled.Value = "0";
                        txtDate.Focus();
                        btnPrint.Visible = false;
                        break;
                    #endregion
                    #region Asset(Add/Edit/Remove)
                    #region Add
                    case ActionsEnum.ADD:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            // insert duty slip details 
                            if (AssetServiceRequestDetailList == null)
                                AssetServiceRequestDetailList = new List<AssetServiceRequestDetails>();
                            if (!IsAssetExists(Convert.ToInt32(hdfAsset.Value)))
                            {
                                AssetServiceRequestDetailList = (List<AssetServiceRequestDetails>)SetUIValuesToObject(ControlsEnum.ASSETSERVICEREQUESTDTL);
                                if (AssetServiceRequestDetailList != null && AssetServiceRequestDetailList.Count > 0)
                                {
                                    SetFieldValues(ControlsEnum.ASSETSERVICEREQUESTDTL);
                                    ResetForm(ControlsEnum.CLEARADD);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_AlreadyAdded").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region EDITASSET
                    //To Edit Asset Details
                    case ActionsEnum.EDITASSET:
                        ResetForm(ControlsEnum.CLEARADD);
                        if (AssetServiceRequestDetailList != null && AssetServiceRequestDetailList.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdAssetDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][1]);
                            if (CurrSlNo > 0)
                            {
                                assetServiceRequestDetailsObj = AssetServiceRequestDetailList.SingleOrDefault(row => CurrSlNo == row.SRD_SL_NO);
                                GetUIValuesFromObject(ControlsEnum.FILLASSETDETAILS);
                            }
                        }
                        break;
                    #endregion
                    #region REMOVEASSET
                    case ActionsEnum.REMOVEASSET:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        if (AssetServiceRequestDetailList != null && AssetServiceRequestDetailList.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
                            if (CurrSlNo > 0)
                            {
                                AssetServiceRequestDetailList = AssetServiceRequestDetailList.Where(row => CurrSlNo != row.SRD_SL_NO).ToList();
                                SetFieldValues(ControlsEnum.ASSETSERVICEREQUESTDTL);
                            }
                        }
                        ResetForm(ControlsEnum.CLEARADD);
                        break;
                    #endregion
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (AssetServiceRequestDetailList == null || AssetServiceRequestDetailList.Count == 0)
                        {
                            litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                        }
                        else
                        {
                            objAssetServiceRequestHeader = new AssetServiceRequestHeader();
                            objAssetServiceRequestHeader = (AssetServiceRequestHeader)SetUIValuesToObject(ControlsEnum.ASSETSERVICEREQUESTHDR);
                            if (objAssetServiceRequestHeader != null)
                            {
                                if (objAssetServiceRequestHeader.Details != null && objAssetServiceRequestHeader.Details.Count > 0)
                                {
                                    objAssetServiceRequestHeader.WKF_FLAG = 0;
                                    objAssetServiceRequestHeader.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                    string trxNo = string.Empty;
                                    string xmlDoc = CommonFunctions.XmlSerialize<AssetServiceRequestHeader>(objAssetServiceRequestHeader);
                                    result = ServiceRequestBL.SaveServiceRequestDetails(xmlDoc, out trxNo);
                                    if (result > 0)
                                    {

                                        lblTrxNo.Text = trxNo;
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEAR);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        CurrPK = (int)result;
                                        GetFieldValues(ControlsEnum.LIST);
                                        SetFieldValues(ControlsEnum.LIST);
                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceRequest);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
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
                        //Show WorkFlow Popup   
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region WRKF SUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else if (AssetServiceRequestDetailList == null || AssetServiceRequestDetailList.Count == 0)
                        {
                            litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                objAssetServiceRequestHeader = new AssetServiceRequestHeader();
                                objAssetServiceRequestHeader = (AssetServiceRequestHeader)SetUIValuesToObject(ControlsEnum.ASSETSERVICEREQUESTHDR);
                                if (objAssetServiceRequestHeader != null)
                                {
                                    if (objAssetServiceRequestHeader.Details != null && objAssetServiceRequestHeader.Details.Count > 0)
                                    {
                                        TrxNo = string.Empty;
                                        objAssetServiceRequestHeader.WKF_FLAG = 1;
                                        SaveTransaction(objAssetServiceRequestHeader, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    }
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)//Cancel 
                            {
                                if (ServiceRequestBL.ValidationForCancellationASR(CurrPK))
                                {
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;

                                    this.CurrPK = base.WkfRefID = 0;
                                    this.ModifiedDatePnl.Visible = false;
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                }
                            }
                            else//Submit
                            {
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }

                        }
                        break;
                    #endregion
                    #region SHOWPOPUP (Item Popup)
                    case ActionsEnum.SHOWPOPUP:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                            CurrSlNo = Convert.ToInt32(grdAssetDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][1]);
                            grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                            RowIndex = grvRow.RowIndex;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemPopup]','" + GetLocalResourceObject("ItemDetails").ToString() + "','800','280');", true);
                        }
                        break;
                    #endregion
                    #region Item(Add/Edit/Remove)
                    #region ADDITEM
                    case ActionsEnum.ADDITEM:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            // insert Item details                              
                            if (!IsItemExists(txtPopupItems.Text.Trim()))
                            {
                                AssetServiceRequestDetailList = (List<AssetServiceRequestDetails>)SetUIValuesToObject(ControlsEnum.ASSETSERVICEREQUEST_ITEMDTL);
                                if (AssetServiceRequestDetailList != null && AssetServiceRequestDetailList.Count > 0)
                                {
                                    SetFieldValues(ControlsEnum.ASSETSERVICEREQUESTDTL);
                                    ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_AlreadyAdded").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemPopup]','" + GetLocalResourceObject("ItemDetails").ToString() + "','800','280');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        //EntryStatus = EntryStatus.ENTRYMODE;
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        ItemRowIndex = grvRow.RowIndex;
                        if (AssetServiceRequestDetailList != null && AssetServiceRequestDetailList.Count > 0)
                        {
                            CurrSlNo = SID_SL_NO = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
                            if (CurrSlNo > 0)
                            {
                                assetServiceRequestDetailsObj = AssetServiceRequestDetailList.SingleOrDefault(row => CurrSlNo == row.SRD_SL_NO);
                                assetServiceRequestItemDetObj = assetServiceRequestDetailsObj.Item_details[ItemRowIndex];
                                GetUIValuesFromObject(ControlsEnum.FILLITEMDETAILS);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemPopup]','" + GetLocalResourceObject("ItemDetails").ToString() + "','800','280');", true);
                            }
                        }
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        ItemRowIndex = grvRow.RowIndex;
                        if (AssetServiceRequestDetailList != null && AssetServiceRequestDetailList.Count > 0)
                        {
                            CurrSlNo = SID_SL_NO = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
                            if (CurrSlNo > 0)
                            {
                                assetServiceRequestDetailsObj = AssetServiceRequestDetailList.SingleOrDefault(row => CurrSlNo == row.SRD_SL_NO);
                                assetServiceRequestDetailsObj.Item_details.RemoveAt(ItemRowIndex);
                                SetFieldValues(ControlsEnum.ASSETSERVICEREQUESTDTL);
                            }
                        }
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        break;
                    #endregion
                    #endregion
                    #region DETAIL/EDIT/VIEW
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                        btnPrint.Visible = true;
                        Status = 0;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlsEnum.CLEAR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSRH_PK")).Value);
                                HiddenField hdfDelStatus = (HiddenField)grdrow.FindControl("hdfDelStatus");
                                HiddenField hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                                Status = Convert.ToInt32(hdfStatus.Value);
                                hdfIsCancelled.Value = hdfDelStatus.Value;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            SetUIEditView(commonActions);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                ucrWrkf.ViewAction();
                            }
                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            ResetForm(ControlsEnum.CLEARADD);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSRH_PK")).Value);
                                grdListRowDeptId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDept")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ucrWrkf.Reset();
                            FillProcessID(12);//12-->For Cancel
                            SetUIEditView(commonActions);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                                ucrWrkf.ViewType = 0;
                            ucrWrkf.ViewAction();

                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            ResetForm(ControlsEnum.CLEARADD);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETESUBMIT (CANCEL SUBMIT Button Click)
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEARSEARCH
                    case ActionsEnum.CLEARSEARCH:
                        FillProcessID(1);
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CANCEL /LIST
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        base.WkfRefID = 0;
                        FillProcessID(1);
                        CurrPK = 0;
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        GetFieldValues(ControlsEnum.ITEMDETAILS);
                        SetFieldValues(ControlsEnum.ITEMDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemPopup]','" + GetLocalResourceObject("ItemDetails").ToString() + "','800','280');", true);
                        break;
                    #endregion
                    #region CLEARADD
                    case ActionsEnum.CLEARADD:
                        ResetForm(ControlsEnum.CLEARADD);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = ServiceRequestBL.DeleteServiceRequest(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(PageIndex) > 1)
                            {
                                PageIndex = Convert.ToInt32(PageIndex) - 1;
                            }
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceRequest);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceRequest);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSRH_PK")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK + "&APPTYPE=" + "SR" + "&APPSUBTYPE=0") + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        private void ResetPageAndWorkflow()
        {
            ResetForm(ControlsEnum.CLEAR);
            EntryStatus = EntryStatus.LISTMODE;
            GetFieldValues(ControlsEnum.LIST);
            SetFieldValues(ControlsEnum.LIST);
            #region Reset Workflow
            ucrWrkf.Reset();
            FillProcessID(1);
            WorkflowCore.CoreService objworkflowCore = new WorkflowCore.CoreService();
            base.WkfRefID = ucrWrkf.RefID = objworkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
            SetCancelRef(CurrPK);
            ucrWrkf.FillWorkFlowDetails();
            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.LISTMODE && ucrWrkf.HasPageTaskPermission)
                ucrWrkf.ViewType = 1;
            else
            {
                ucrWrkf.ViewType = 0;
                ucrWrkf.ViewAction();
            }
            #endregion
        }

        /// <summary>
        /// Save With workflow submition
        /// </summary>
        /// <param name="objServiceRequestHeader"></param>
        private void SaveTransaction(AssetServiceRequestHeader objServiceRequestHeader, int workflowFlag)
        {
            int? result = 0;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objServiceRequestHeader == null)
                objServiceRequestHeader = new AssetServiceRequestHeader();
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objServiceRequestHeader.USER_PK = wkfDetails.UserPK;
            objServiceRequestHeader.WKF_APPLICATION = CurrPK;
            objServiceRequestHeader.WKF_COMMENTS = wkfDetails.Comments;
            objServiceRequestHeader.WKF_TRX_FLAG = workflowFlag;
            objServiceRequestHeader.WKF_PROCESS = wkfDetails.ProcessID;
            objServiceRequestHeader.WKF_REFERENCE = wkfDetails.ReferenceID;
            objServiceRequestHeader.WKF_TASK = wkfDetails.TaskID;
            objServiceRequestHeader.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<AssetServiceRequestHeader>(objServiceRequestHeader);
            result = ServiceRequestBL.SaveServiceRequestDetails(xmlDoc, out TrxNo);
            if (result > 0)
            {
                if (!string.IsNullOrEmpty(TrxNo))
                    lblTrxNo.Text = TrxNo;
                if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                {
                    FillProcessID(1);
                    litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                }
                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                ((TextBox)ucrWrkf.FindControl("WrkfComments")).Text = string.Empty;//Clear Workflow comments

                object[] args = new object[2];
                args[0] = Resources.PageNameRes.AssetServiceRequest;
                args[1] = lblTrxNo.Text.Trim();
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                // Show Save Message and redired to listing page                                      
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ResetForm(ControlsEnum.CLEAR);
                    EntryStatus = EntryStatus.LISTMODE;
                    CurrPK = (int)result;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                    ResetForm(ControlsEnum.CLEAR);
                    EntryStatus = EntryStatus.LISTMODE;
                    CurrPK = (int)result;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                }

            }
            else
            {
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.REFNOEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceRequest + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceRequest);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
            }
        }

        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                if (Mode == ActionsEnum.NEW)
                {
                    EntryStatus = EntryStatus.NEWMODE;
                }
                else if (Mode == ActionsEnum.VIEW)
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
        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridView senderGridView = (GridView)sender;
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (senderGridView.ID == "grdAssetDetails")
                    {
                        if (AssetServiceRequestDetailList != null)
                        {
                            HiddenField hdfSRDSlNo = e.Row.FindControl("hdfSRDSlNo") as HiddenField;
                            Label lblTotal = e.Row.FindControl("lblTotal") as Label;
                            int SRD_SL_NO = Convert.ToInt32(hdfSRDSlNo.Value);
                            assetServiceRequestItemDetList = new List<AssetServiceRequestItemDetails>();
                            assetServiceRequestItemDetList = AssetServiceRequestDetailList.SingleOrDefault(c => c.SRD_SL_NO == SRD_SL_NO).Item_details;
                            if (assetServiceRequestItemDetList != null)
                            {
                                GridView grdItems = e.Row.FindControl("grdItems") as GridView;
                                grdItems.DataSource = assetServiceRequestItemDetList;
                                grdItems.DataBind();
                                if (lblTotal != null)
                                {
                                    lblTotal.Text = lblTotal.ToolTip = GetFormattedCurrencyWithComma(assetServiceRequestItemDetList.Sum(itm => itm.SID_AMOUNT));
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Pager Methods + Init
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

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "ShowHideAdvancedSearch();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                else if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);

                if (CurrPK == 0)
                    btnSubmit.Visible = false;
                if (CurrPK == 0 || Status == 0)
                    btnCancelSubmit.Visible = false;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                PgerControlNew pagerControl = (PgerControlNew)sender;
                string senderId = pagerControl.ID;
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        pagerControl.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage--;
                        break;
                }
                if (senderId == "uclPaging")
                {
                    PageIndex = uclPaging.CurrentPage;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        #region InitializeComponent
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
        private void FillProcessID(int pid)
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();

            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    ucrWrkf.PageUrl = path;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    if (pid == 1)
                    {
                        PageProcessID = ucrWrkf.ProcessID;
                    }
                    base.WkfPageUrl = path;
                }
            }
        }
        /// <summary>
        /// For Bind Cancelation comment on workflow user control
        /// </summary>
        /// <param name="curPK"></param>
        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            }
            #endregion
        }
        private string GetUrl()
        {
            string path = string.Empty;
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            if (Request.QueryString[QueryStrings.PID] == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=1";
                else
                    path = Request.Url.AbsolutePath.ToLower() + "?PID=1";
            }
            else
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                else
                    path = Request.Url.AbsolutePath.ToLower();
            }
            return path;
        }
        private bool IsAssetExists(int assetPk)
        {
            bool IsExists = false;
            if (AssetServiceRequestDetailList != null)
            {
                foreach (AssetServiceRequestDetails item in AssetServiceRequestDetailList)
                {
                    if (item.SRD_ASSET == assetPk && CurrSlNo == 0)
                    {
                        IsExists = true;
                        break;
                    }
                }
            }
            return IsExists;
        }
        private bool IsItemExists(string itemName)
        {
            bool IsExists = false;
            if (AssetServiceRequestDetailList != null)
            {
                List<AssetServiceRequestItemDetails> lstASRItemDetails = new List<AssetServiceRequestItemDetails>();
                AssetServiceRequestDetails assetServiceRequestDetailsObj = AssetServiceRequestDetailList.SingleOrDefault(itm => itm.SRD_SL_NO == CurrSlNo);
                if (assetServiceRequestDetailsObj.Item_details != null)
                {
                    lstASRItemDetails = assetServiceRequestDetailsObj.Item_details.DeepClone();
                    if (lstASRItemDetails.Count > 0 && ItemRowIndex >= 0)
                    {
                        lstASRItemDetails.RemoveAt(ItemRowIndex);
                    }
                    foreach (AssetServiceRequestItemDetails item in lstASRItemDetails)
                    {
                        if (item.SID_ASR_ITEM_TEXT == itemName)
                        {
                            IsExists = true;
                            break;
                        }
                    }
                }
            }
            return IsExists;
        }
        #endregion
        #region Page Events
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);


            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnDeleteNew.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);

        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
        #endregion
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            CURRENCY,
            LIST,
            CLEAR,
            PAYMENTMODE,
            EDIT,
            CLEARPOPUPDETAILS,
            COMPANY,
            BRANCH,
            PROCESSMODE,
            CLEARADD,
            GRIDEDIT,
            GRIDDELETE,
            DATDETAILS,
            DEPARTMENTBYTYPE,
            REQUESTINGSTORE,
            ASSETSERVICEREQUESTHDR,
            ASSETSERVICEREQUESTDTL,
            ASSETSERVICEREQUEST_ITEMDTL,
            ASSETSERVICETYPE,
            FILLASSETDETAILS,
            ITEMDETAILS,
            FILLITEMDETAILS,
            CLEARSEARCH,
            PRINT
        }
        #endregion
    }
}