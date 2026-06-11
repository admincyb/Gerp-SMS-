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
using System.Linq;
using BusinessObject.CommonManagement;
using System.Data;
using System.Threading;
using BusinessObject;
using System.Web.UI.HtmlControls;
using BusinessObject.Shipping;
using BusinessLogic.Shipping;
using System.Text;

namespace ERPSMS_v01.Shipping
{
    public partial class ContainerRelease : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
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
        /// To maintain the SortExpression or then By in viewstate
        /// </summary>
        private string ThenBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenBy] = value;
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
        private ScrapAllocation scrapAllocation
        {
            get
            {
                return this.ViewState["scrapAllocation"] == null ? ScrapAllocation.BinCard : (ScrapAllocation)(this.ViewState["scrapAllocation"]);
            }
            set
            {
                this.ViewState["scrapAllocation"] = value;
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
        /// Property for to maintain ContainerReleaseDetail List
        /// </summary>
        private List<ContainerReleaseDetail> ContainerReleaseDetails
        {
            get
            {
                return (List<ContainerReleaseDetail>)ViewState["ContainerReleaseDetail"];
            }
            set
            {
                ViewState["ContainerReleaseDetail"] = value;
            }

        }

        private ContainerItems ContainerItemsObj
        {
            get
            {
                return (ContainerItems)ViewState["ContainerItemsObj"];
            }
            set
            {
                ViewState["ContainerItemsObj"] = value;
            }
        }

        /// <summary>
        /// CartonsListTemp
        /// </summary>
        private List<Cartons> CartonsListTemp
        {
            get
            {
                return ViewState["CartonsListTemp"] == null ? new List<Cartons>() : (List<Cartons>)ViewState["CartonsListTemp"];
            }
            set
            {
                ViewState["CartonsListTemp"] = value;
            }
        }

        /// <summary>
        /// CartonsGridviewViewState
        /// </summary>
        private List<CartonsGridview> CartonsGridviewListViewState
        {
            get
            {
                return ViewState["CartonsGridviewListViewState"] == null ? new List<CartonsGridview>() : (List<CartonsGridview>)ViewState["CartonsGridviewListViewState"];
            }
            set
            {
                ViewState["CartonsGridviewListViewState"] = value;
            }
        }

        private int CartonBrandPk
        {
            get
            {
                //return this.ViewState["CartonBrandPk"] == null ? 0 : Convert.ToInt32(this.ViewState["CartonBrandPk"].ToString());
                return (GetNullableInt(hdfCartonBrandPk.Value) ?? 0);
            }
            set
            {
                //this.ViewState["CartonBrandPk"] = value;
                hdfCartonBrandPk.Value = Convert.ToString(value);
            }
        }

        private int ItemType
        {
            get
            {
                return this.ViewState["ItemType"] == null ? 0 : Convert.ToInt32(this.ViewState["ItemType"].ToString());
            }
            set
            {
                this.ViewState["ItemType"] = value;
            }
        }

        private decimal CartonBrandQty
        {
            get
            {
                return this.ViewState["CartonBrandQty"] == null ? 0 : Convert.ToInt32(this.ViewState["CartonBrandQty"].ToString());
            }
            set
            {
                this.ViewState["CartonBrandQty"] = value;
            }
        }

        private string WeightFormat
        {
            get
            {
                return (string)this.ViewState["WeightFormat"];
            }
            set
            {
                this.ViewState["WeightFormat"] = value;
            }
        }
        private int IsScrapBinPacked
        {
            get
            {
                return this.ViewState["IsScrapBinPacked"] == null ? 0 : Convert.ToInt32(this.ViewState["IsScrapBinPacked"].ToString());
            }
            set
            {
                this.ViewState["IsScrapBinPacked"] = value;
            }
        }
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private string refID;
        private string inboxFlag;
        private ContainerReleaseHeader ContainerReleaseHeaderObj;
        private ContainerReleaseHdr ContainerReleaseHdrObj;
        //private ContainerItems ContainerItemsObj;
        //private ContainerItems ContainerItemsTemp;
        private ContainerItemDetails ItemDetails;
        private DataTable ContainerReleasedt;
        private DataSet dsShippingPlanHDR;
        private DataSet dsLoadingPlan;
        private int tabLevel;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private int CompanyPkByUserSBU = 0;
        private int prevCompany = 0;

        private int itemPK;
        BinCardIssueBO objNewBins = new BinCardIssueBO();
        HiddenField hdfNumberDecimalDigitBin;

        private DataTable dtResult;
        private DataSet dsResult;
        private DataTable dtIssueStore;
        //private string CartonNotFullMessage = string.Empty;
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
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    ShowHideControls();
                    GetFieldValues(ControlsEnum.COMPANYLIST);
                    SetFieldValues(ControlsEnum.COMPANYLIST);

                    ShippingPlanPK = Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] != null ? (int)Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] : 0;
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
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(refID);
                        Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = ShippingPlanPK = GetApplicationID(ucrWrkf.RefID);
                    }
                    IsScrapBinPacked = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsScrapBinPacked").ToString());
                    hdfBinPageSize.Value = GetLocalResourceObject("BinAutoPageSize").ToString();
                    hdfIsQaPassedBinsOnly.Value = GetLocalResourceObject("IsQaPassedBinsOnly").ToString();
                    #region Set Decimal Count
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    WeightFormat = "#" + currencysep + "#0.";
                    hdfNumberDecimalDigitBin = this.Page.Master.FindControl("hdfNumberDecimalDigitBin") as HiddenField;
                    for (int i = 0; i < Convert.ToInt32(hdfNumberDecimalDigitBin.Value); i++)
                    {
                        WeightFormat += "0";
                    }
                    #endregion

                    if (ShippingPlanPK > 0)
                    {
                        FillProcessID();
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();

                        EntryStatus = EntryStatus.ENTRYMODE;
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                        }
                        ucrWrkf.ViewAction();
                        GetFieldValues(ControlsEnum.CONTAINERRELEASEHDR);
                        SetFieldValues(ControlsEnum.CONTAINERRELEASEHDR);

                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        if (CurrPK == 0)
                        {
                            if (prevCompany != null && prevCompany != 0)
                            {
                                ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(prevCompany.ToString())));
                            }
                        }

                        divDeliveryList.Visible = false;
                        if (GetLocalResourceObject("ShowDeliveryList") != null && GetLocalResourceObject("ShowDeliveryList").ToString() == "1")
                        {
                            divDeliveryList.Visible = true;
                            GetFieldValues(ControlsEnum.DODETAILS);
                            SetFieldValues(ControlsEnum.DODETAILS);
                        }
                        if (GetGlobalResourceObject("ConfigurationsRes", "ShowContainerPalletNo").ToString() == "1")
                        {
                            divPalletNo.Visible = true;
                        }
                        else
                        {
                            divPalletNo.Visible = false;
                        }
                    }
                    else
                    {
                        Response.Redirect(Resources.PageURL.ShippingPlan);
                    }
                    SetTabVisibility();
                    GetFieldValues(ControlsEnum.LOCATION);
                    SetFieldValues(ControlsEnum.LOCATION);
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

            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            ContainerReleaseHeaderObj = new ContainerReleaseHeader();
            try
            {
                switch (type)
                {
                    #region Default
                    case ControlsEnum.DEFAULT:
                        //dsLoadingPlan = BusinessLogic.Shipping.ShippingPlanBL.GetPlanInfo(ShippingPlanPK);
                        dsLoadingPlan = new DataSet();
                        dsLoadingPlan = LoadingPlanBL.GetLoadingPlan(ShippingPlanPK, currentUser.SBUID, 1);
                        //To get prev transaction company
                        if (CurrPK == 0)
                        {
                            prevCompany = BusinessLogic.Shipping.ShippingPlanBL.GetPrevCompany(ShippingPlanPK);
                        }
                        break;
                    #endregion
                    #region CONTAINERRELEASEHDR
                    case ControlsEnum.CONTAINERRELEASEHDR:
                        ContainerReleasedt = ContainerReleaseBL.GetContainerReleaseHeader(ShippingPlanPK, 1, currentUser.SBUID);
                        if (ContainerReleasedt != null)
                        {
                            ContainerReleaseHeaderObj.CRH_PK = ContainerReleasedt.Rows[0]["CRH_PK"] == DBNull.Value ? 0 : int.Parse(ContainerReleasedt.Rows[0]["CRH_PK"].ToString());
                            //ContainerReleaseHeaderObj.CRH_SHIPPING_PLAN = ContainerReleasedt.Rows[0]["CRH_SHIPPING_PLAN"] == DBNull.Value ? 0 : int.Parse(ContainerReleasedt.Rows[0]["CRH_SHIPPING_PLAN"].ToString());
                            ContainerReleaseHeaderObj.CRH_DATE = ContainerReleasedt.Rows[0]["CRH_DATE"] == DBNull.Value ? DateTime.Now : DateTime.Parse(ContainerReleasedt.Rows[0]["CRH_DATE"].ToString());
                            ContainerReleaseHeaderObj.CRH_REMARKS = ContainerReleasedt.Rows[0]["CRH_REMARKS"].ToString();
                            ContainerReleaseHeaderObj.CRH_ACTIVE = ContainerReleasedt.Rows[0]["CRH_ACTIVE"] == DBNull.Value ? 0 : int.Parse(ContainerReleasedt.Rows[0]["CRH_ACTIVE"].ToString());
                            ContainerReleaseHeaderObj.CRH_DEPT = ContainerReleasedt.Rows[0]["CRH_DEPT"] == DBNull.Value ? 0 : int.Parse(ContainerReleasedt.Rows[0]["CRH_DEPT"].ToString());
                            ContainerReleaseHeaderObj.CRH_CRTD_BY = ContainerReleasedt.Rows[0]["CRH_CRTD_BY"] == DBNull.Value ? 0 : int.Parse(ContainerReleasedt.Rows[0]["CRH_CRTD_BY"].ToString());
                            ContainerReleaseHeaderObj.CRH_CRTD_DT = ContainerReleasedt.Rows[0]["CRH_CRTD_DT"] == DBNull.Value ? DateTime.Now : DateTime.Parse(ContainerReleasedt.Rows[0]["CRH_CRTD_DT"].ToString());
                            ContainerReleaseHeaderObj.CRH_MOD_BY = ContainerReleasedt.Rows[0]["CRH_MOD_BY"] == DBNull.Value ? 0 : int.Parse(ContainerReleasedt.Rows[0]["CRH_MOD_BY"].ToString());
                            ContainerReleaseHeaderObj.CRH_MOD_DT = ContainerReleasedt.Rows[0]["CRH_MOD_DT"] == DBNull.Value ? DateTime.Now : DateTime.Parse(ContainerReleasedt.Rows[0]["CRH_MOD_DT"].ToString());
                            ContainerReleaseHeaderObj.SNH_PK = int.Parse(ContainerReleasedt.Rows[0]["SNH_PK"].ToString());
                            ContainerReleaseHeaderObj.SNH_NO = ContainerReleasedt.Rows[0]["SNH_NO"].ToString();
                            ContainerReleaseHeaderObj.CSH_CONTAINER_NO = ContainerReleasedt.Rows[0]["DPH_CONTAINER_NO"].ToString();
                            ContainerReleaseHeaderObj.CSH_SEAL_NO = ContainerReleasedt.Rows[0]["DPH_SEAL_NO"].ToString();
                            //ContainerReleaseHeaderObj.CSH_IN_TIME = ContainerReleasedt.Rows[0]["CRH_DATE"] == DBNull.Value ? DateTime.Now : (ContainerReleasedt.Rows[0]["CSH_IN_TIME"] == DBNull.Value ? DateTime.Now : DateTime.Parse(ContainerReleasedt.Rows[0]["CSH_IN_TIME"].ToString()));
                            ContainerReleaseHeaderObj.CSH_IN_TIME_S = ContainerReleasedt.Rows[0]["CSH_IN_TIME"] == DBNull.Value ? string.Empty : ContainerReleasedt.Rows[0]["CSH_IN_TIME"].ToString();
                            //ContainerReleaseHeaderObj.SNH_SHIP_TO_PORT = ContainerReleasedt.Rows[0]["DPH_PORT_OF_DISCHARGE"].ToString();//Bug ID:  16091
                            ContainerReleaseHeaderObj.SNH_SHIP_TO_PORT = ContainerReleasedt.Rows[0]["DPH_TO_PORT"].ToString();
                            ContainerReleaseHeaderObj.CRH_COMPANY = ContainerReleasedt.Rows[0]["CRH_COMPANY"] == DBNull.Value ? 0 : Convert.ToInt32(ContainerReleasedt.Rows[0]["CRH_COMPANY"]);
                            ContainerReleaseHeaderObj.CRH_LOAD_DATE = ContainerReleasedt.Rows[0]["CRH_LOAD_DATE"] == DBNull.Value ? DateTime.Now : DateTime.Parse(ContainerReleasedt.Rows[0]["CRH_LOAD_DATE"].ToString());
                            ContainerReleaseHeaderObj.CRH_TRAILER_LIC_NO = ContainerReleasedt.Rows[0]["CRH_TRAILER_LIC_NO"].ToString();
                            ContainerReleaseHeaderObj.CRH_TRUCK_LIC_NO = ContainerReleasedt.Rows[0]["CRH_TRUCK_LIC_NO"].ToString();

                        }
                        if (ContainerReleaseHeaderObj == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        }
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
                        break;
                    #endregion

                    #region DODETAILS
                    case ControlsEnum.DODETAILS:
                        string xmlData = BusinessLogic.Shipping.ContainerReleaseBL.GetDODetails(ShippingPlanPK);
                        ContainerReleaseDetailGet tempContainerReleaseDetails = CommonFunctions.XmlDeserialize<ContainerReleaseDetailGet>(xmlData);
                        ContainerReleaseDetails = tempContainerReleaseDetails.ContainerReleaseDetail;

                        //#region Assign to CartonsGridviewViewState
                        //List<CartonsGridview> tempCartonsGridview = new List<CartonsGridview>();
                        //foreach (ContainerReleaseDetail item in ContainerReleaseDetails)
                        //{
                        //    var slNos=item.CartonDetails
                        //        .Select(x=>x.CRC_SL_NO_GRP)
                        //        .Distinct();
                        //    foreach (var item2 in slNos)
                        //    {
                        //        CartonsGridview cartonsGridviewTemp=new CartonsGridview();
                        //        Cartons tempCarton= item.CartonDetails
                        //            .Where(x=>x.CRC_SL_NO_GRP==item2)
                        //            .First();
                        //        cartonsGridviewTemp.CRC_SL_NO_GRP=tempCarton.CRC_SL_NO_GRP;
                        //        if (tempCarton.BCR_PALLET_NO != string.Empty) cartonsGridviewTemp.BCR_PALLET_NO = tempCarton.BCR_PALLET_NO;
                        //        else cartonsGridviewTemp.BCR_PALLET_NO = GetLocalResourceObject("Carton").ToString();
                        //        cartonsGridviewTemp.CartonsCount=item.CartonDetails.Count;
                        //        cartonsGridviewTemp.QtyPcs=Convert.ToInt32(item.CartonDetails.Sum(x=>x.CRC_QTY_DESPATCHED));
                        //        cartonsGridviewTemp.BCR_LOCATION_TEXT = item.CartonDetails[0].BCR_LOCATION_TEXT;
                        //        tempCartonsGridview.Add(cartonsGridviewTemp);
                        //    }
                        //}
                        //CartonsGridviewListViewState=tempCartonsGridview;
                        //#endregion


                        break;
                    #endregion
                    #region LOCATION
                    case ControlsEnum.LOCATION:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetDepartmentList(null, Convert.ToInt32(DbActiveStatus.ACTIVE), null, 2, 10);
                        break;
                    #endregion

                    case ControlsEnum.AUTOALLOCATE:
                        //dtResult = ContainerReleaseBL.AutoAllocateCartonDetails();
                        break;

                    case ControlsEnum.ISSUESTORE:
                        int deptType = 2;
                        int menuType = 0;
                        if (ItemType == 1)//material
                            deptType = 2;
                        else if (ItemType == 2 || ItemType == 3 || ItemType == 4 || ItemType == 5)//product, B-Grage, Scrap, Wallet
                        {
                            deptType = 2;
                            menuType = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ContainerReleaseStore")); //13 warehouse 5 production
                        }
                        dtIssueStore = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetStoresByTypeNew(0, deptType, currentUser, currentUser.SBUID, Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE), menuType);
                        break;

                    case ControlsEnum.BATCH:
                        dtResult = BusinessLogic.MaterialManagement.MaterialMaster.GetBatchNoAuto(itemPK, Convert.ToInt32(ddlIssueStore.SelectedValue), 0, DateTime.Now, 0, DateTime.Now, 0);
                        break;

                    #region ITEM STOCK
                    case ControlsEnum.ITEMSTOCK:
                        dtResult = DataAccess.MaterialManagement.MaterialMasterDL.GetBatchDetails(Convert.ToInt32(hdfBatchPK.Value));
                        break;
                    #endregion

                    case ControlsEnum.ADDBINCARD:
                        string strXml = BusinessLogic.CommonManagement.CommonBL.GetBinCardsForIssue(Convert.ToInt32(ddlIssueStore.SelectedValue), currentUser.SBUID,
                                    hdfBinCard.Value,null,null,null,null,null,null,0,0, txtIssueBinCard.Text);
                        if (!string.IsNullOrEmpty(strXml))
                        {
                            objNewBins = CommonFunctions.XmlDeserialize<BinCardIssueBO>(strXml);
                        }
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
                    #region Bind DropDown
                    case ControlsEnum.COMPANYLIST:
                        BindDropDownList(ControlsEnum.COMPANYLIST);
                        break;
                    #endregion
                    case ControlsEnum.CONTAINERRELEASEHDR:
                        GetUIValuesFromObject(controlType);
                        break;
                    #region Default
                    case ControlsEnum.DEFAULT:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region DODETAILS
                    case ControlsEnum.DODETAILS:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region LOCATION
                    case ControlsEnum.LOCATION:
                        BindDropDownList(ControlsEnum.LOCATION);
                        break;
                    #endregion
                    #region ISSUESTORE
                    case ControlsEnum.ISSUESTORE:
                        BindDropDownList(ControlsEnum.ISSUESTORE);
                        break;
                    #endregion
                    #region BATCH
                    case ControlsEnum.BATCH:
                        BindDropDownList(ControlsEnum.BATCH);
                        break;
                    #endregion
                    #region ITEMSTOCK
                    case ControlsEnum.ITEMSTOCK:
                        GetUIValuesFromObject(ControlsEnum.BATCH);
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
        #region Helper Methods
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
                BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                switch (controlType)
                {
                    #region Container Release Hdr
                    case ControlsEnum.CONTAINERRELEASEHDR:

                        ContainerReleaseHdrObj.CRH_PK = CurrPK;
                        ContainerReleaseHdrObj.CRH_SHIPPING_PLAN = ShippingPlanPK;
                        ContainerReleaseHdrObj.CRH_DATE = DateTime.Parse(txtReleaseDate.Text + " " + txtOutTime.Text);
                        ContainerReleaseHdrObj.CRH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                        ContainerReleaseHdrObj.CRH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        ContainerReleaseHdrObj.BIZUNIT_PK = Convert.ToInt16(currentUser.SBUID);
                        ContainerReleaseHdrObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        ContainerReleaseHdrObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        ContainerReleaseHdrObj.WKF_PROCESS = Convert.ToInt32(hdfProcessID.Value);
                        ContainerReleaseHdrObj.LAST_MOD_DT = LastModifiedTime;
                        ContainerReleaseHdrObj.P_RET_VAL = 0;
                        ContainerReleaseHdrObj.CRH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        ContainerReleaseHdrObj.CRH_LOAD_DATE = Convert.ToDateTime(txtStartLDate.Text);
                        ContainerReleaseHdrObj.CRH_TRAILER_LIC_NO = HttpUtility.HtmlEncode(txtLicenseNo.Text);
                        ContainerReleaseHdrObj.CRH_TRUCK_LIC_NO = HttpUtility.HtmlEncode(txtTruckLicenseNo.Text);
                        ContainerReleaseHdrObj.IS_ENABLE_PAC_COST = GetGlobalResourceObject("ConfigurationsRes", "EnablePackingCost").ToString();
                        ContainerReleaseHdrObj.IS_ENABLE_PRD_COST = GetGlobalResourceObject("ConfigurationsRes", "EnableProductCost").ToString();
                        //CartonNotFullMessage = string.Empty;
                        List<ContainerReleaseDetail> containerReleaseDetails = new List<ContainerReleaseDetail>();
                        for (int i = 0; i < grdDeliveryList.Rows.Count; i++)
                        {
                            GridViewRow row = grdDeliveryList.Rows[i];
                            HiddenField hdfCDR_PK = row.FindControl("hdfCDR_PK") as HiddenField;
                            HiddenField hdfCDR_SL_NO = row.FindControl("hdfCDR_SL_NO") as HiddenField;

                            HiddenField hdfCDR_DO_DTL = row.FindControl("hdfCDR_DO_DTL") as HiddenField;
                            HiddenField hdfSaleOrderDtlPK = row.FindControl("hdfSaleOrderDtlPK") as HiddenField;
                            HiddenField hdfBrandPk = row.FindControl("hdfBrandPk") as HiddenField;
                            HiddenField hdfCDR_ITEM = row.FindControl("hdfCDR_ITEM") as HiddenField;
                            HiddenField hdfCDR_QTY_DESPATCHED = row.FindControl("hdfCDR_QTY_DESPATCHED") as HiddenField;
                            HiddenField hdfCDR_CARTON_DESPATCHED = row.FindControl("hdfCDR_CARTON_DESPATCHED") as HiddenField;
                            HiddenField hdfCDR_UOM = row.FindControl("hdfCDR_UOM") as HiddenField;
                            HiddenField hdfAPS_TOTAL_PCS = row.FindControl("hdfAPS_TOTAL_PCS") as HiddenField;
                            HiddenField hdfIsPackedBin = row.FindControl("hdfIsPackedBin") as HiddenField;

                            Label lblDOQty = row.FindControl("lblDOQty") as Label;
                            Label lblQtyCartonsDO = row.FindControl("lblQtyCartonsDO") as Label;
                            int DOQtyPcs = lblDOQty.Text == string.Empty ? 0 : Convert.ToInt32(lblDOQty.Text);
                            int DOQtyCtn = lblQtyCartonsDO.Text == string.Empty ? 0 : Convert.ToInt32(lblQtyCartonsDO.Text);
                            int LoadedQtyPcs = hdfCDR_QTY_DESPATCHED.Value == string.Empty ? 0 : Convert.ToInt32(hdfCDR_QTY_DESPATCHED.Value);
                            int LoadedQtyCtn = hdfCDR_CARTON_DESPATCHED.Value == string.Empty ? 0 : Convert.ToInt32(hdfCDR_CARTON_DESPATCHED.Value);

                            if (LoadedQtyPcs > DOQtyPcs || LoadedQtyCtn > DOQtyCtn)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("MsgDoLoadedQty").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                                return retObject;
                            }

                            ContainerReleaseDetail containerReleaseDetail = new ContainerReleaseDetail();
                            if (hdfIsPackedBin.Value != string.Empty)
                                containerReleaseDetail.CDR_IS_PACKED_BIN = hdfIsPackedBin.Value;
                            containerReleaseDetail.CDR_PK = Convert.ToInt32(hdfCDR_PK.Value);
                            containerReleaseDetail.CDR_SL_NO = Convert.ToInt32(hdfCDR_SL_NO.Value);
                            containerReleaseDetail.CDR_DO_DTL = Convert.ToInt32(hdfCDR_DO_DTL.Value);
                            containerReleaseDetail.CDR_SO_DTL = Convert.ToInt32(hdfSaleOrderDtlPK.Value);
                            containerReleaseDetail.CDR_CUST_ITEM = Convert.ToInt32(hdfBrandPk.Value);
                            containerReleaseDetail.CDR_ITEM = Convert.ToInt32(hdfCDR_ITEM.Value);
                            containerReleaseDetail.CDR_QTY_DESPATCHED = Convert.ToInt32(hdfCDR_QTY_DESPATCHED.Value);
                            containerReleaseDetail.CDR_CARTON_DESPATCHED = Convert.ToInt32(hdfCDR_CARTON_DESPATCHED.Value);
                            //containerReleaseDetail.CDR_QTY_APPROVED = Convert.ToDecimal(hdfOldDespNow.Value);
                            containerReleaseDetail.CDR_UOM = Convert.ToInt32(hdfCDR_UOM.Value);

                            ContainerReleaseDetail temp = ContainerReleaseDetails
                                .Where(x => x.CDR_SL_NO == containerReleaseDetail.CDR_SL_NO)
                                .Single();
                            containerReleaseDetail.CartonDetails = temp.CartonDetails;
                            containerReleaseDetail.ContainerItems = temp.ContainerItems;
                            //Following lines commented since partial cartons won't appear in container release
                            //if ((containerReleaseDetail.CDR_QTY_DESPATCHED % Convert.ToDecimal(hdfAPS_TOTAL_PCS.Value)) != 0)
                            //{
                            //    CartonNotFullMessage = GetLocalResourceObject("CartonNotFullMessage").ToString();
                            //}
                            containerReleaseDetails.Add(containerReleaseDetail);
                        }
                        ContainerReleaseHdrObj.ContainerReleaseDetails = containerReleaseDetails;
                        retObject = ContainerReleaseHdrObj;

                        break;
                    #endregion

                    #region ADDITEM
                    case ControlsEnum.ADDITEM:
                        if (ContainerItemsObj == null)
                        {
                            ContainerItemsObj = new ContainerItems();
                            ContainerItemsObj.SaleContract = lblSCNo.Text;
                            ContainerItemsObj.SaleContractPK = Convert.ToInt32(hdfSCPK.Value);
                            ContainerItemsObj.ItemName = lblItemName.Text;
                            ContainerItemsObj.ItemPK = Convert.ToInt32(hdfItemPK.Value);
                            ContainerItemsObj.Store = ddlIssueStore.SelectedItem.Text;
                            ContainerItemsObj.StorePK = Convert.ToInt32(ddlIssueStore.SelectedValue);
                            ContainerItemsObj.ItemType = Convert.ToInt32(hdfItemTypePK.Value);
                        }

                        int slNo = 1;

                        if (ContainerItemsObj.ItemDetailsList != null && ContainerItemsObj.ItemDetailsList.Any())
                            slNo = ContainerItemsObj.ItemDetailsList.Max(m => m.SlNo) + 1;

                        if (ContainerItemsObj.ItemDetailsList == null)
                            ContainerItemsObj.ItemDetailsList = new List<ContainerItemDetails>();

                        if (hdfSlNo.Value != "-1" && ContainerItemsObj.ItemDetailsList.Any())//Edit case
                        {
                            ContainerItemsObj.ItemDetailsList.ForEach(it =>
                            {
                                if (it.SlNo == Convert.ToInt32(hdfSlNo.Value))
                                {
                                    it.BatchPK = Convert.ToInt32(hdfBatchPK.Value);
                                    it.Batch = txtBatch.Text;
                                    it.UOM = txtUOM.Text;
                                    it.UOMPK = Convert.ToInt16(hdfUOMPK.Value);
                                    it.Quantity = txtQty.Text == "" ? 0 : Convert.ToDecimal(txtQty.Text);
                                    it.ItemPK = Convert.ToInt32(hdfItemPK.Value);
                                }
                            });

                        }
                        else//new Item
                        {
                            ItemDetails = new ContainerItemDetails();
                            ItemDetails.ItemPK = Convert.ToInt32(hdfItemPK.Value);
                            ItemDetails.ItemType = Convert.ToInt32(hdfItemTypePK.Value);
                            if (ItemDetails.ItemType == 1)
                            {
                                ItemDetails.Batch = txtBatch.Text;
                                ItemDetails.BatchPK = Convert.ToInt32(hdfBatchPK.Value);
                                ItemDetails.UOM = txtUOM.Text;
                                ItemDetails.UOMPK = Convert.ToInt16(hdfUOMPK.Value);
                                ItemDetails.Quantity = txtQty.Text == "" ? 0 : Convert.ToDecimal(txtQty.Text);
                            }
                            else if (ItemDetails.ItemType == 2 || ItemDetails.ItemType == 3 || ItemDetails.ItemType == 4)
                            {
                                ItemDetails.Bincard = txtIssueBinCard.Text;
                                ItemDetails.BincardPK = Convert.ToInt32(hdfBinCard.Value);
                            }
                            ItemDetails.SlNo = slNo;

                            ContainerItemsObj.ItemDetailsList.Add(ItemDetails);
                        }

                        break;
                    #endregion

                    case ControlsEnum.ADDBINCARD:
                        if (ContainerItemsObj == null)
                        {
                            ContainerItemsObj = new ContainerItems();
                            ContainerItemsObj.SaleContract = lblSCNo.Text;
                            ContainerItemsObj.SaleContractPK = Convert.ToInt32(hdfSCPK.Value);
                            ContainerItemsObj.ItemName = lblItemName.Text;
                            ContainerItemsObj.ItemPK = Convert.ToInt32(hdfItemPK.Value);
                            ContainerItemsObj.Store = ddlIssueStore.SelectedItem.Text;
                            ContainerItemsObj.StorePK = Convert.ToInt32(ddlIssueStore.SelectedValue);
                            ContainerItemsObj.ItemType = Convert.ToInt32(hdfItemTypePK.Value);
                        }

                        foreach (GridViewRow gvr in grdProduct.Rows)
                        {
                            Label lblGrdBincard = (Label)gvr.FindControl("lblGrdBincard");
                            HiddenField hdfGrdBincardPK = (HiddenField)gvr.FindControl("hdfGrdBincardPK");
                            HiddenField hdfProdUOMPK = (HiddenField)gvr.FindControl("hdfProdUOMPK");
                            Label lblProdQty = (Label)gvr.FindControl("lblProdQty");

                            int No = 1;
                            if (ContainerItemsObj.ItemDetailsList != null && ContainerItemsObj.ItemDetailsList.Any())
                                No = ContainerItemsObj.ItemDetailsList.Max(m => m.SlNo) + 1;

                            ItemDetails = new ContainerItemDetails();
                            ItemDetails.ItemPK = Convert.ToInt32(hdfItemPK.Value);
                            ItemDetails.ItemType = Convert.ToInt32(hdfItemTypePK.Value);
                            ItemDetails.Bincard = txtIssueBinCard.Text;
                            ItemDetails.BincardPK = Convert.ToInt32(hdfBinCard.Value);
                            ItemDetails.UOM = txtUOM.Text;
                            ItemDetails.UOMPK = Convert.ToInt16(hdfUOMPK.Value);
                            ItemDetails.Quantity = txtQty.Text == "" ? 0 : Convert.ToDecimal(txtQty.Text);
                            ItemDetails.SlNo = No;

                            if (ContainerItemsObj.ItemDetailsList == null)
                                ContainerItemsObj.ItemDetailsList = new List<ContainerItemDetails>();

                            ContainerItemsObj.ItemDetailsList.Add(ItemDetails);
                        }
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

        private bool IsSameItemExist()
        {
            bool isExist = false;
            try
            {
                if (ContainerItemsObj == null)
                    isExist = false;
                else if (ContainerItemsObj.ItemDetailsList == null)
                    isExist = false;
                else if (ItemType == 1)
                {
                    if (ContainerItemsObj.ItemDetailsList.Any(a => a.BatchPK == Convert.ToInt32(hdfBatchPK.Value) && Convert.ToInt32(hdfSlNo.Value) == 0))
                        isExist = true;
                }
                else if (ItemType == 2)
                {
                    if (ContainerItemsObj.ItemDetailsList.Any(a => (a.BincardPK == Convert.ToInt32(hdfBinCard.Value)|| a.Bincard== txtIssueBinCard.Text) && Convert.ToInt32(hdfSlNo.Value) == 0))
                        isExist = true;
                }
                else
                    isExist = false;
                return isExist;
            }
            catch (Exception ex)
            {

                throw ex;
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
                    #region Default
                    case ControlsEnum.DEFAULT:
                        if (dsLoadingPlan != null && dsLoadingPlan.Tables[0] != null && dsLoadingPlan.Tables[0].Rows.Count > 0)
                        {
                            lblContainerTypeValueHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_CONTAINER_TYPE_TEXT].ToString(), 20);
                            lblContainerTypeValueHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_CONTAINER_TYPE_TEXT].ToString();

                            lblDestinationPortHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_SHIP_TO_PORT].ToString(), 20);
                            lblDestinationPortHdr.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_SHIP_TO_PORT].ToString(), 300);

                            //lblInTimeHdr.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME]).ToString(Resources.Constants.DateTimeFormat), 20) : string.Empty;
                            //lblInTimeHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString() != string.Empty ? Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME]).ToString(Resources.Constants.DateTimeFormat) : string.Empty;

                            lblInTimeHdr.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CVH_BOOKING_DATE]).ToString(Resources.Constants.DateFormatShort), 20) + " " + ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME]).ToString(Resources.Constants.TimeFormatShort), 20) : string.Empty;
                            lblInTimeHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CVH_BOOKING_DATE]).ToString(Resources.Constants.DateFormatShort), 20) + " " + ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME]).ToString(Resources.Constants.TimeFormatShort), 20) : string.Empty;

                            lblCustomerHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUS_TEXT].ToString(), 25);
                            lblCustomerHdr.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUS_TEXT].ToString(), 300);

                            lblGONHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.DPH_NO].ToString(), 20);
                            lblGONHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.DPH_NO].ToString();

                            lblGONDateHdr.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.DPH_DATE].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.DPH_DATE].ToString()).ToString(Resources.Constants.DateFormatShort), 20) : string.Empty;
                            lblGONDateHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.DPH_DATE].ToString() != string.Empty ? Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.DPH_DATE].ToString()).ToString(Resources.Constants.DateFormatShort) : string.Empty;

                            hdfDelstatus.Value = dsLoadingPlan.Tables[0].Rows[0][Resources.DataFieldRes.SPDeleteStatus].ToString();
                        }
                        break;
                    #endregion
                    case ControlsEnum.CONTAINERRELEASEHDR:
                        if (ContainerReleaseHeaderObj != null)
                        {
                            CurrPK = ContainerReleaseHeaderObj.CRH_PK;

                            lblDispContainerNo.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.CSH_CONTAINER_NO), 45);
                            lblDispContainerNo.ToolTip = HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.CSH_CONTAINER_NO);

                            lblDispSealNo.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.CSH_SEAL_NO), 45);
                            lblDispSealNo.ToolTip = HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.CSH_SEAL_NO);

                            lblDispDestinationPort.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.SNH_SHIP_TO_PORT), 40);
                            lblDispDestinationPort.ToolTip = HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.SNH_SHIP_TO_PORT);

                            // lblDispInTime.Text = ContainerReleaseHeaderObj.CSH_IN_TIME.ToString(Resources.Constants.TimeFormatShort);
                            //  lblDispInTime.ToolTip = ContainerReleaseHeaderObj.CSH_IN_TIME.ToString(Resources.Constants.TimeFormatShort);

                            lblDispInTime.Text = string.IsNullOrEmpty(ContainerReleaseHeaderObj.CSH_IN_TIME_S) ? string.Empty : Convert.ToDateTime(ContainerReleaseHeaderObj.CSH_IN_TIME_S).ToString(Resources.Constants.TimeFormatShort);
                            lblDispInTime.ToolTip = string.IsNullOrEmpty(ContainerReleaseHeaderObj.CSH_IN_TIME_S) ? string.Empty : Convert.ToDateTime(ContainerReleaseHeaderObj.CSH_IN_TIME_S).ToString(Resources.Constants.TimeFormatShort);

                            txtReleaseDate.Text = ContainerReleaseHeaderObj.CRH_DATE.ToString(Resources.Constants.DateFormatShort);
                            txtReleaseDate.ToolTip = ContainerReleaseHeaderObj.CRH_DATE.ToString(Resources.Constants.DateFormatShort);

                            txtOutTime.Text = ContainerReleaseHeaderObj.CRH_DATE.ToString(Resources.Constants.TimeFormatShort);
                            txtOutTime.ToolTip = ContainerReleaseHeaderObj.CRH_DATE.ToString(Resources.Constants.TimeFormatShort);

                            txtRemarks.Text = HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.CRH_REMARKS);
                            txtRemarks.ToolTip = HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.CRH_REMARKS); ;

                            txtStartLDate.Text = ContainerReleaseHeaderObj.CRH_LOAD_DATE.ToString(Resources.Constants.DateFormatShort);
                            txtStartLDate.ToolTip = ContainerReleaseHeaderObj.CRH_LOAD_DATE.ToString(Resources.Constants.DateFormatShort);

                            txtLicenseNo.Text = HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.CRH_TRAILER_LIC_NO);
                            txtLicenseNo.ToolTip = HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.CRH_TRAILER_LIC_NO);

                            txtTruckLicenseNo.Text = HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.CRH_TRUCK_LIC_NO);
                            txtTruckLicenseNo.ToolTip = HttpUtility.HtmlDecode(ContainerReleaseHeaderObj.CRH_TRUCK_LIC_NO);

                            ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(ContainerReleaseHeaderObj.CRH_COMPANY.ToString())));

                            LastModifiedTime = DateTime.Now;

                            if (CurrPK > 0)
                            {
                                ModifiedDatePnl.Visible = false;
                                LastModifiedTime = ContainerReleaseHeaderObj.CRH_MOD_DT;
                                lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            }
                        }
                        break;
                    case ControlsEnum.BATCH:
                        txtUOM.Text = dtResult.Rows[0]["SBD_UOM_TEXT"].ToString();
                        hdfUOMPK.Value = dtResult.Rows[0]["SBD_UOM"].ToString();
                        txtStock.Text = dtResult.Rows[0]["SBD_QTY_IN_STOCK"].ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDownList(ControlsEnum controlType)
        {
            switch (controlType)
            {
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
                #region LOCATION
                case ControlsEnum.LOCATION:
                    ddlLocationPopUp.Items.Clear();
                    ddlLocationPopUp.DataSource = dtResult;
                    ddlLocationPopUp.DataTextField = GTIService.Constants.DirectStockTransfer.Fields.DPT_NAME;
                    ddlLocationPopUp.DataValueField = GTIService.Constants.DirectStockTransfer.Fields.DPT_PK;
                    ddlLocationPopUp.DataBind();
                    ddlLocationPopUp.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    ddlLocationPopUp.Items.HtmlDecode();
                    break;
                #endregion
                case ControlsEnum.ISSUESTORE:
                    if (dtIssueStore != null)
                    {
                        ddlIssueStore.DataSource = dtIssueStore;
                        ddlIssueStore.DataTextField = "DPT_NAME";
                        ddlIssueStore.DataValueField = "DPT_PK";
                        ddlIssueStore.DataBind();
                    }
                    break;
                default:
                    break;
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
                    #region DODETAILS
                    case ControlsEnum.DODETAILS:
                        grdDeliveryList.DataSource = ContainerReleaseDetails;
                        grdDeliveryList.DataBind();
                        break;
                    #endregion
                    #region CONTAINERSPLITUP
                    case ControlsEnum.CONTAINERSPLITUP:
                        //grdCartons.DataSource = CartonsListTemp;
                        //grdCartons.DataBind();
                        grdCartons.DataSource = CartonsGridviewListViewState;
                        grdCartons.DataBind();

                        break;
                    #endregion
                    case ControlsEnum.ITEMDETAILS:
                        grdProduct.DataSource = null;
                        grdItems.DataSource = null;
                        // ContainerItemsObj
                        switch (ItemType)
                        {
                            case 1: //Material
                                grdItems.DataSource = ContainerItemsObj == null ? null : ContainerItemsObj.ItemDetailsList;
                                break;
                            case 2://Product
                            case 3://B-Grade
                            case 4://Scrap
                            case 5://Wallet
                                grdProduct.DataSource = ContainerItemsObj == null ? null : ContainerItemsObj.ItemDetailsList;
                                break;
                        }
                        grdItems.DataBind();
                        grdProduct.DataBind();
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
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType = ControlsEnum.DEFAULT)
        {
            switch (controlType)
            {
                #region DEFAULT
                case ControlsEnum.DEFAULT:
                    CurrPK = 0;
                    txtOutTime.Text = "";
                    txtReleaseDate.Text = "";
                    txtRemarks.Text = "";
                    txtStartLDate.Text = "";
                    txtTruckLicenseNo.Text = "";
                    txtLicenseNo.Text = "";
                    ModifiedDatePnl.Visible = false;
                    hdfConfirmDeleteAllocation.Value = "0";
                    break;
                #endregion
                #region SEARCH
                case ControlsEnum.SEARCH:
                    txtPalleteBinCard.Text = txtSONumber.Text = "Select/Type";
                    hdfPalleteBinCard.Value = hdfSoPK.Value = CommonConstants.SELECTVAL;
                    ddlLocationPopUp.SelectedIndex = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                    txtCartonPrefixPopUp.Text = txtCartonFromPopUp.Text = txtCartonToPopUp.Text = string.Empty;
                    hdfSomeCartonMissingConfirm.Value = CommonConstants.SELECT_ALL_VAL;
                    chkAutoMode.Checked = false;
                    break;
                #endregion
                #region ITEMDETAILS
                case ControlsEnum.ITEMDETAILS:
                    hdfBatchPK.Value = "0";
                    txtBatch.Text = "Select/Type";
                    txtStock.Text = string.Empty;
                    txtQty.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    hdfUOMPK.Value = "0";
                    hdfSlNo.Value = "-1";
                    hdfSelectedBatch.Value = "";
                    hdfSelectedBatchPK.Value = "0";
                    break;
                #endregion
                #region ADDBINCARD
                case ControlsEnum.ADDBINCARD:
                    txtIssueBinCard.Text = "";
                    hdfBincardPK.Value = "0";
                    break;
                    #endregion
            }

        }

        [System.Web.Services.WebMethod]
        public static object GetBinNo(string SearchKey, int dept, int PageSize, string IsQaPassed,string ItemPK)
        {
            // Get login user details


            BusinessObject.User curUser;
            curUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            // get auto complete list and return to javascript
            //List<AutoCompleteBO> lstValue = BincardCreateBL.GetAutoCompleteList(SearchKey, curUser.CurrentSBUPK); 
            List<AutoCompleteBO> lstValue = BusinessLogic.CommonManagement.CommonBL.GetAutoBinCardsForIssue(dept, Convert.ToInt32(curUser.SBUID), SearchKey, 1, PageSize, (IsQaPassed == string.Empty ? 0 : Convert.ToInt32(IsQaPassed)), -1, Convert.ToInt32(HttpContext.GetGlobalResourceObject("ConfigurationsRes", "IsLotNoAutoActive")),Convert.ToInt32(ItemPK));
            var searchResult = from rows in lstValue.AsEnumerable()
                               select new
                               {
                                   Key = rows.Key,
                                   Value = rows.Name
                               };
            return searchResult;
        }

        public string GetFormattedWeightwithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(WeightFormat);
        }

        public void ShowCartonPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ClosePopup();ShowContainerDiv('[id$=divCartonDtlsPopUp]','"
                                + GetLocalResourceObject("CartonDetails").ToString() + "','950','450');", true);
        }

        public void ShowProductCartonPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ClosePopup();ShowContainerDiv('[id$=divCartonDtlsPopUp]','"
                                + GetLocalResourceObject("CartonDetails").ToString() + "','950','450');", true);
        }

        public void ShowMaterialCartonPopup()
        {
            string title = "CartonDetails";
            if (ItemType == 1)
                title = "MaterialDetails";
            else if (ItemType == 2)
                title = "ProductDetails";
            txtIssueBinCard.Text = string.Empty;
            hdfBinCard.Value = "0";


            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ClosePopup();ShowContainerDiv('[id$=divMaterialCartonDtlsPopUp]','"
                                + GetLocalResourceObject(title).ToString() + "','950','450');", true);
        }

        public void ShowCartonPopupDetail()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divCartonDtlsPopUpDetail]','"
                             + GetLocalResourceObject("CartonDetails").ToString() + "','600','400');", true);
        }

        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
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
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            int? result;
            DropDownList ddlWkfAction;
            TextBox WrkfComments;
            string action;
            string soPK;
            string IsInventoryLocked = "";
            string LockUptoDate = string.Empty;
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {

                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    if (((CheckBox)sender).ID == "chkAutoMode")
                    {
                        if (chkAutoMode.Checked)
                            commonActions = ActionsEnum.AUTOALLOCATECARTON;
                        else
                        {
                            ShowCartonPopup();
                            chkAutoMode.Focus();
                        }
                    }
                }
                switch (commonActions)
                {
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            #region Checking :Inventory Transaction Locking
                            IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtReleaseDate.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
                            if (IsInventoryLocked == "1")
                            {
                                litErrorMsg.Text = GetLocalResourceObject("ErrInvTransactionsLocked_Msg").ToString() + " " + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            #endregion
                            ContainerReleaseHdrObj = new ContainerReleaseHdr();
                            ContainerReleaseHdrObj = (ContainerReleaseHdr)SetUIValuesToObject(ControlsEnum.CONTAINERRELEASEHDR);

                            if (ContainerReleaseHdrObj != null)
                            {
                                //
                                //string xmlDoc = CommonFunctions.XmlSerialize<ContainerReleaseHdr>(ContainerReleaseHdrObj);
                                // save Process Container Release Header
                                result = ContainerReleaseBL.SaveContainerReleaseHeader(ContainerReleaseHdrObj);
                                if (result > 0) // Success !  redirect to listing page
                                {
                                    // Show Save Message and redired to listing page                                        
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerRelease);
                                    //if (CartonNotFullMessage != string.Empty)
                                    //{
                                    //    litErrorMsg.Text += "<br/>" + CartonNotFullMessage;
                                    //}
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.ENTRYMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.CONTAINERRELEASEHDR);
                                    SetFieldValues(ControlsEnum.CONTAINERRELEASEHDR);

                                    GetFieldValues(ControlsEnum.DODETAILS);
                                    SetFieldValues(ControlsEnum.DODETAILS);
                                }
                                else
                                {
                                    if (result == (int)DbSaveStatus.ALREADYCREATED)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.ContainerRelease + " " + GetLocalResourceObject("AlreadyCreated").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_NotEnoughQtyInStock").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.REFNOEXIST)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_NoPalletExists").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.AMOUNTEXCEEDS)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_CompareCustomerReturnQty").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Save").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                    }
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
                        WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                        WrkfComments.Text = string.Empty;
                        break;
                    #endregion

                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        //Show WorkFlow Popup                       
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                        WrkfComments.Text = string.Empty;
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                #region Checking :Inventory Transaction Locking
                                IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtReleaseDate.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
                                if (IsInventoryLocked == "1")
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("ErrInvTransactionsLocked_Msg").ToString() + " " + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                #endregion
                                ContainerReleaseHdrObj = new ContainerReleaseHdr();
                                ContainerReleaseHdrObj = (ContainerReleaseHdr)SetUIValuesToObject(ControlsEnum.CONTAINERRELEASEHDR);

                                if (ContainerReleaseHdrObj != null)
                                {
                                    result = ContainerReleaseBL.SaveContainerReleaseHeader(ContainerReleaseHdrObj);
                                    if (result > 0) // Success !  redirect to listing page
                                    {
                                        ucrWrkf.ApplicationID = ShippingPlanPK;
                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            //litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            //+ "','" + Resources.ErpRes.Information + "');", true);
                                            litErrorMsg.Text = GetLocalResourceObject("Err_NotEnoughQtyInStock").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        if (result == (int)DbSaveStatus.ALREADYCREATED)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.ContainerRelease + " " + GetLocalResourceObject("AlreadyCreated").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_NoPalletExists").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.AMOUNTEXCEEDS)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_CompareCustomerReturnQty").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                #region Checking :Inventory Transaction Locking
                                IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtReleaseDate.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
                                if (IsInventoryLocked == "1")
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("ErrInvTransactionsLocked_Msg").ToString() + " " + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                #endregion
                                ucrWrkf.ApplicationID = ShippingPlanPK;
                            }

                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                //if (WrkfComments.Text.Trim().StartsWith(",,"))
                                //{
                                //    WrkfComments.Text = WrkfComments.Text.Trim().Length > 2 ? WrkfComments.Text.Trim().Substring(2, WrkfComments.Text.Trim().Length - 2) : string.Empty;
                                //}
                                //if (WrkfComments.Text.Trim().StartsWith(","))
                                //{
                                //    WrkfComments.Text = WrkfComments.Text.Trim().Length > 1 ? WrkfComments.Text.Trim().Substring(1, WrkfComments.Text.Trim().Length-1) : string.Empty;
                                //}
                                string s1 = WrkfComments.Text;
                                string s2 = string.Empty;
                                for (int i = 0; i < s1.Length; i++)
                                {
                                    if (s1[i] != ',')
                                    {
                                        s2 = s1.Remove(0, i);
                                        break;
                                    }
                                }
                                WrkfComments.Text = s2;

                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result > 0)
                                    {
                                        WrkfComments.Text = "";
                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerRelease);
                                        //if (CartonNotFullMessage != string.Empty)
                                        //{
                                        //    litErrorMsg.Text += "<br/>" + CartonNotFullMessage;
                                        //}
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        //    + "','" + Resources.ErpRes.Information + "');", true);
                                        EntryStatus = EntryStatus.ENTRYMODE;
                                        ResetForm();

                                        SetTabVisibility();
                                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanPK, ucrWrkf.ProcessID);
                                        ucrWrkf.FillWorkFlowDetails();
                                        EntryStatus = EntryStatus.ENTRYMODE;
                                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                            ucrWrkf.ViewType = 1;
                                        else
                                        {
                                            ucrWrkf.ViewType = 0;
                                            //EntryStatus = EntryStatus.VIEWMODE;
                                        }
                                        ucrWrkf.ViewAction();

                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            GetFieldValues(ControlsEnum.CONTAINERRELEASEHDR);
                                            SetFieldValues(ControlsEnum.CONTAINERRELEASEHDR);
                                            GetFieldValues(ControlsEnum.DODETAILS);
                                            SetFieldValues(ControlsEnum.DODETAILS);
                                        }
                                    }

                                }
                            }

                            //ContainerReleaseHdrObj = new ContainerReleaseHdr();
                            //ContainerReleaseHdrObj = (ContainerReleaseHdr)SetUIValuesToObject(ControlsEnum.CONTAINERRELEASEHDR);
                            //if (ContainerReleaseHdrObj != null)
                            //{
                            //
                            //string xmlDoc = CommonFunctions.XmlSerialize<ContainerReleaseHdr>(ContainerReleaseHdrObj);
                            // save Process Container Release Header
                            //result = ContainerReleaseBL.SaveContainerReleaseHeader(ContainerReleaseHdrObj);
                            //if (result > 0) // Success !  redirect to listing page
                            //{
                            //Workflow submission
                            //ucrWrkf.ApplicationID = (int)result;
                            //ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                            //WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                            ////Do WorkFlow if WorkFlow has Actions
                            //if (ddlWkfAction.Items.Count > 0)
                            //{
                            //    action = ddlWkfAction.SelectedItem.ToString();
                            //    result = ucrWrkf.DoWorkFlow();
                            //    if (result > 0)
                            //    {
                            //        WrkfComments.Text = "";
                            //        //Show Save success message and reset Contract Entry
                            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                            //        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerRelease);
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //            + "','" + Resources.ErpRes.Information + "');", true);
                            //        EntryStatus = EntryStatus.ENTRYMODE;
                            //        ResetForm();

                            //        SetTabVisibility();
                            //        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            //        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanPK, ucrWrkf.ProcessID);
                            //        ucrWrkf.FillWorkFlowDetails();
                            //        EntryStatus = EntryStatus.ENTRYMODE;
                            //        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            //            ucrWrkf.ViewType = 1;
                            //        else
                            //        {
                            //            ucrWrkf.ViewType = 0;
                            //            //EntryStatus = EntryStatus.VIEWMODE;
                            //        }
                            //        ucrWrkf.ViewAction();

                            //        GetFieldValues(ControlsEnum.CONTAINERRELEASEHDR);
                            //        SetFieldValues(ControlsEnum.CONTAINERRELEASEHDR);
                            //    }

                            //}

                            //}
                            //else
                            //{
                            //    if (result == (int)DbSaveStatus.SQLERROR)
                            //    {
                            //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //            + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //    else if (result == (int)DbSaveStatus.CONCURRENCY)
                            //    {
                            //        litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //        + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //    else if (result == (int)DbSaveStatus.CODEEXIST)
                            //    {
                            //        litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //        + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //    else
                            //    {
                            //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //            + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //}
                            //}
                        }
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        //ResetForm();                  
                        EntryStatus = EntryStatus.ENTRYMODE;
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

                    #region Show SC Popup
                    case ActionsEnum.SHOWPOPUP:
                        //for SC Print
                        soPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + soPK + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion

                    #region ADD
                    case ActionsEnum.ADD:
                        List<string> lstCartons = new List<string>();
                        int palleteId = (GetNullableInt(hdfPalleteBinCard.Value) ?? 0) > 0 ? GetNullableInt(hdfPalleteBinCard.Value).Value : 0;
                        string palleteNo = "";
                        if (palleteId == 0) { palleteNo = txtPalleteBinCard.Text.Trim(); }
                        string cartonPrefix = txtCartonPrefixPopUp.Text.Trim();
                        int cartonFrom = GetNullableInt(txtCartonFromPopUp.Text.Trim()) ?? -1;
                        int cartonTo = GetNullableInt(txtCartonToPopUp.Text.Trim()) ?? -1;
                        int locationId = GetNullableInt(ddlLocationPopUp.SelectedValue) ?? 0;
                        // int soPk = (GetNullableInt(hdfSoPK.Value) ?? 0) > 0 ? GetNullableInt(hdfSoPK.Value).Value : 0; //GetNullableInt(hdfCDR_SO_DTL.Value).Value;
                        int soPk = (GetNullableInt(hdfCurrentSCID.Value) ?? 0) > 0 ? GetNullableInt(hdfCurrentSCID.Value).Value : 0; //GetNullableInt(hdfCDR_SO_DTL.Value).Value;
                        //cartonFrom = cartonFrom > -1 ? cartonFrom : 0;
                        cartonTo = cartonTo > -1 ? cartonTo : 0;
                        if (cartonFrom > -1)
                        {
                            int strLength = txtCartonFromPopUp.Text.Length;
                            string CodeExp = cartonFrom.ToString().PadLeft(strLength, '0');

                            lstCartons.Add(cartonPrefix + CodeExp);
                            for (int i = cartonFrom + 1; i <= cartonTo; i++)
                            {
                                string CodeExpInner = i.ToString().PadLeft(strLength, '0');
                                lstCartons.Add(cartonPrefix + CodeExpInner);
                            }
                        }
                        string cartonsWithComa = string.Join(",", lstCartons.ToArray());
                        int brandPk = CartonBrandPk;
                        int currentCDRPk = GetNullableInt(hdfCurrentCDR_PK.Value).Value;
                        dtResult = BusinessLogic.Shipping.ContainerReleaseBL.AddCartonToList(soPk, cartonsWithComa, palleteId, locationId, cartonPrefix, brandPk, currentCDRPk, (palleteNo == "Select/Type" ? string.Empty : palleteNo));

                        int cdrSlNo = Convert.ToInt32(hdfCDR_SL_NO_PopUp.Value);
                        List<Cartons> tempList1 = new List<Cartons>();


                        //  List<Cartons> tempList = CartonsListTemp;
                        int slNo = CartonsListTemp.Count > 0 ? CartonsListTemp.Max(x => x.CRC_SL_NO) : 0;// 0;
                        foreach (DataRow row in dtResult.Rows)
                        {
                            Cartons temp = new Cartons();
                            temp.CRC_CARTON_MST = Convert.ToInt32(row["BCR_PK"]);
                            temp.BCR_NO = Convert.ToString(row["BCR_NO"]);
                            if (!DBNull.Value.Equals(row["BCR_PALLET"])) temp.BCR_PALLET = Convert.ToInt32(row["BCR_PALLET"]);
                            //if(row["BCR_PALLET"]!=null) temp.BCR_PALLET = Convert.ToInt32(row["BCR_PALLET"]);
                            temp.BCR_PALLET_NO = (Convert.ToString(row["BCR_PALLET_NO"])).HtmlDecode();
                            temp.CRC_QTY_DESPATCHED = Convert.ToInt32(row["BCR_TOTAL_QTY"]);
                            temp.BCR_LOCATION_TEXT = (Convert.ToString(row["BCR_LOCATION_TEXT"])).HtmlDecode();
                            temp.CRC_CDR_SL_NO = cdrSlNo;
                            temp.CRC_SL_NO = ++slNo;

                            foreach (var item in ContainerReleaseDetails)
                            {
                                if (item.CartonDetails.Exists(x => x.CRC_CARTON_MST == temp.CRC_CARTON_MST)
                                    || CartonsListTemp.Exists(x => x.CRC_CARTON_MST == temp.CRC_CARTON_MST)
                                    ) // Contains(  temp))
                                {
                                    btnPlus2.Focus();
                                    ShowCartonPopup();
                                    string message = string.Empty;
                                    if ((GetNullableInt(hdfPalleteBinCard.Value) ?? 0) > 0)
                                        message = string.Format(GetLocalResourceObject("Err_PalleteAlreadyExistsInList").ToString(), temp.BCR_NO);
                                    else
                                        message = string.Format(GetLocalResourceObject("Err_CartonAlreadyExistsInList").ToString(), temp.BCR_NO);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                            }

                            tempList1.Add(temp);
                        }
                        if (tempList1.Count < 1)
                        {
                            //btnPlus2.Focus();
                            txtPalleteBinCard.Text = "Select/Type";
                            txtPalleteBinCard.Focus();
                            ShowCartonPopup();
                            string message = GetLocalResourceObject("NoCartonsFound").ToString();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message) + "','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrMessageFocusCtrl('" + CommonFunctions.FormatErrorMessage(message) + "',null,null,null,'" + "txtPalleteBinCard" + "');", true);
                            return;
                        }

                        string message2 = string.Empty; //GetLocalResourceObject("Err_FollowingCartonsMissing").ToString();
                        bool flag = true;
                        foreach (var item in lstCartons)
                            if (!tempList1.Any(x => x.BCR_NO.ToUpper().Contains(item.ToUpper()))) // == ))
                            {
                                message2 += item + ", ";
                                flag = false;
                            }
                        if (!flag && hdfSomeCartonMissingConfirm.Value != 1.ToString())
                        {
                            hdfSomeCartonMissingMessage.Value = message2.HtmlEncode();
                            btnPlus2.Focus();
                            ShowCartonPopup();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message2) + "','" + Resources.Messages.Information + "');", true);

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnConfirmSomeCartonMissing", "fnConfirmSomeCartonMissing();", true);
                            return;
                        }

                        #region Add the new cartons to  CartonsGridviewViewState
                        List<CartonsGridview> tempCartonsGridview = CartonsGridviewListViewState;
                        int slNoGrp = 1;
                        if (tempCartonsGridview.Count > 0)
                        {
                            slNoGrp = tempCartonsGridview.Max(x => x.CRC_SL_NO_GRP) + 1;
                        }
                        CartonsGridview temp1 = new CartonsGridview();
                        temp1.CRC_SL_NO_GRP = slNoGrp;
                        if (tempList1[0].BCR_PALLET_NO != string.Empty) temp1.BCR_PALLET_NO = tempList1[0].BCR_PALLET_NO;
                        else temp1.BCR_PALLET_NO = GetLocalResourceObject("Carton").ToString();
                        temp1.CartonsCount = tempList1.Count;
                        temp1.QtyPcs = Convert.ToInt32(tempList1.Sum(x => x.CRC_QTY_DESPATCHED));
                        temp1.BCR_LOCATION_TEXT = tempList1[0].BCR_LOCATION_TEXT;
                        tempCartonsGridview.Add(temp1);
                        CartonsGridviewListViewState = tempCartonsGridview;
                        foreach (var item in tempList1)
                        {
                            item.CRC_SL_NO_GRP = slNoGrp;
                        }
                        #endregion

                        List<Cartons> tempList = CartonsListTemp;
                        tempList.AddRange(tempList1);
                        CartonsListTemp = tempList;
                        BindGrid(ControlsEnum.CONTAINERSPLITUP);
                        //btnPlus2.Focus();
                        txtPalleteBinCard.Focus();
                        ShowCartonPopup();
                        ResetForm(ControlsEnum.SEARCH);
                        chkAutoMode.Checked = false;
                        break;
                    #endregion

                    #region APPLY
                    case ActionsEnum.APPLY:
                        decimal pcsTempList = 0;
                        int cartonsTempList = 0;
                        pcsTempList = CartonsListTemp.Sum(x => x.CRC_QTY_DESPATCHED);
                        cartonsTempList = CartonsListTemp.Count;
                        int baseGridSlNo = Convert.ToInt32(hdfCDR_SL_NO_PopUp.Value);
                        List<ContainerReleaseDetail> tempContainerReleaseDetails = ContainerReleaseDetails;
                        ContainerReleaseDetail tempContainerReleaseDetail = (tempContainerReleaseDetails.Where(x => x.CDR_SL_NO == baseGridSlNo)).First();

                        if (cartonsTempList < 1)
                        {
                            tempContainerReleaseDetail.CDR_QTY_DESPATCHED = pcsTempList;
                            tempContainerReleaseDetail.CDR_CARTON_DESPATCHED = cartonsTempList;
                            tempContainerReleaseDetail.CDR_IS_PACKED_BIN = string.Empty;
                            tempContainerReleaseDetail.CartonDetails = CartonsListTemp;
                            ContainerReleaseDetails = tempContainerReleaseDetails;
                            BindGrid(ControlsEnum.DODETAILS);
                            CartonsListTemp = null;
                            hdfCDR_SL_NO_PopUp.Value = hdfCDR_SO_DTL.Value = 0.ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ClosePopup();", true);
                            hdfConfirmQuantityNotMatch.Value = 0.ToString();

                            //string message = GetLocalResourceObject("NoCartonsFound").ToString();
                            //ShowCartonPopup();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message)
                            //    + "','" + Resources.Messages.Information + "');", true);
                            return;
                        }

                        // string messageApply = GetLocalResourceObject("Err_CartonQtyOrItemQtyMissmatch").ToString();
                        if (tempContainerReleaseDetail.CDR_DO_QTY != pcsTempList && tempContainerReleaseDetail.CDR_DO_CARTON != cartonsTempList && hdfConfirmQuantityNotMatch.Value != 1.ToString())
                        {
                            ShowCartonPopup();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnConfirmQuantityNotMatch", "fnConfirmQuantityNotMatch();", true);
                            return;
                        }
                        tempContainerReleaseDetail.CDR_QTY_DESPATCHED = pcsTempList;
                        tempContainerReleaseDetail.CDR_CARTON_DESPATCHED = cartonsTempList;
                        if (CartonsListTemp != null && CartonsListTemp.Count > 0)
                            tempContainerReleaseDetail.CDR_IS_PACKED_BIN = "1";

                        tempContainerReleaseDetail.CartonDetails = CartonsListTemp;
                        ContainerReleaseDetails = tempContainerReleaseDetails;
                        BindGrid(ControlsEnum.DODETAILS);
                        CartonsListTemp = null;
                        hdfCDR_SL_NO_PopUp.Value = hdfCDR_SO_DTL.Value = 0.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ClosePopup();", true);
                        hdfConfirmQuantityNotMatch.Value = 0.ToString();
                        break;
                    #endregion

                    #region CLEARITEM
                    case ActionsEnum.CLEARITEM:
                        ResetForm(ControlsEnum.SEARCH);
                        ShowCartonPopup();
                        btnClear1.Focus();
                        break;
                    #endregion

                    #region AUTO ALLOCATE CARTON
                    case ActionsEnum.AUTOALLOCATECARTON:
                        if (grdCartons.Rows.Count > 0)
                        {
                            if (hdfCartonListReloadConfirm.Value != 1.ToString())
                            {
                                chkAutoMode.Focus();
                                ShowCartonPopup();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnCartonListReloadConfirm", "fnCartonListReloadConfirm();", true);
                                return;
                            }
                        }

                        decimal DQQtyPcs = lblDOQtyPcs.Text == "" ? 0 : Convert.ToDecimal(lblDOQtyPcs.Text);
                        dtResult = ContainerReleaseBL.AutoAllocateCartonDetails(CartonBrandPk, CartonBrandQty, (GetNullableInt(hdfCurrentSCID.Value) ?? 0), GetNullableInt(hdfCurrentCDR_PK.Value).Value, DQQtyPcs);
                        int slno = 0;
                        List<Cartons> CartonsListTemp1 = new List<Cartons>();
                        foreach (DataRow row in dtResult.Rows)
                        {
                            Cartons temp = new Cartons();
                            temp.CRC_CARTON_MST = Convert.ToInt32(row["BCR_PK"]);
                            temp.BCR_NO = Convert.ToString(row["BCR_NO"]);
                            if (!DBNull.Value.Equals(row["BCR_PALLET"])) temp.BCR_PALLET = Convert.ToInt32(row["BCR_PALLET"]);
                            temp.BCR_PALLET_NO = (Convert.ToString(row["BCR_PALLET_NO"])).HtmlDecode();
                            temp.CRC_QTY_DESPATCHED = Convert.ToInt32(row["BCR_TOTAL_QTY"]);
                            temp.BCR_LOCATION_TEXT = (Convert.ToString(row["BCR_LOCATION_TEXT"])).HtmlDecode();
                            temp.CRC_CDR_SL_NO = Convert.ToInt32(hdfCDR_SL_NO_PopUp.Value);
                            temp.CRC_SL_NO = ++slno;
                            temp.CRC_SL_NO_GRP = Convert.ToInt32(row["BCR_SL_NO_GRP"]);
                            if (dtResult.Columns.Contains("BCR_IS_PARTIAL"))
                                temp.BCR_IS_PARTIAL = Convert.ToInt32(row["BCR_IS_PARTIAL"]);
                            #region Valiation for existance in List
                            foreach (var item in ContainerReleaseDetails.Where(x => x.CDR_SL_NO != temp.CRC_CDR_SL_NO).ToList())
                            {
                                if (item.CartonDetails.Exists(x => x.CRC_CARTON_MST == temp.CRC_CARTON_MST)
                                    // || CartonsListTemp.Exists(x => x.CRC_CARTON_MST == temp.CRC_CARTON_MST)
                                    ) // Contains(  temp))
                                {
                                    chkAutoMode.Focus();
                                    ShowCartonPopup();
                                    string message = string.Empty;
                                    if ((GetNullableInt(hdfPalleteBinCard.Value) ?? 0) > 0)
                                        message = string.Format(GetLocalResourceObject("Err_PalleteAlreadyExistsInList").ToString(), temp.BCR_NO);
                                    else
                                        message = string.Format(GetLocalResourceObject("Err_CartonAlreadyExistsInList").ToString(), temp.BCR_NO);

                                    hdfCartonListReloadConfirm.Value = 0.ToString();
                                    chkAutoMode.Checked = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                            }
                            #endregion
                            CartonsListTemp1.Add(temp);
                        }
                        CartonsListTemp = CartonsListTemp1;
                        #region Add to CartonsGridviewListViewState
                        var groups = CartonsListTemp
                             .Select(x => x.CRC_SL_NO_GRP)
                            .Distinct();
                        List<CartonsGridview> tempCartonsGridview2 = new List<CartonsGridview>();
                        foreach (var item2 in groups)
                        {
                            CartonsGridview cartonsGridviewTemp = new CartonsGridview();
                            Cartons tempCarton = CartonsListTemp
                                .Where(x => x.CRC_SL_NO_GRP == item2)
                                .First();
                            cartonsGridviewTemp.CRC_SL_NO_GRP = tempCarton.CRC_SL_NO_GRP;
                            if (tempCarton.BCR_PALLET_NO != null && tempCarton.BCR_PALLET_NO != string.Empty) cartonsGridviewTemp.BCR_PALLET_NO = tempCarton.BCR_PALLET_NO;
                            else cartonsGridviewTemp.BCR_PALLET_NO = GetLocalResourceObject("Carton").ToString();
                            cartonsGridviewTemp.CartonsCount = CartonsListTemp
                                .Where(x => x.CRC_SL_NO_GRP == item2)
                                .Count();
                            cartonsGridviewTemp.QtyPcs = Convert.ToInt32(CartonsListTemp
                                .Where(x => x.CRC_SL_NO_GRP == item2)
                                .Sum(x => x.CRC_QTY_DESPATCHED));
                            cartonsGridviewTemp.BCR_LOCATION_TEXT = CartonsListTemp
                                .Where(x => x.CRC_SL_NO_GRP == item2)
                                .First()
                                .BCR_LOCATION_TEXT;
                            tempCartonsGridview2.Add(cartonsGridviewTemp);
                        }
                        CartonsGridviewListViewState = tempCartonsGridview2;
                        #endregion
                        BindGrid(ControlsEnum.CONTAINERSPLITUP);
                        chkAutoMode.Focus();
                        ShowCartonPopup();
                        hdfCartonListReloadConfirm.Value = 0.ToString();
                        break;
                    #endregion

                    #region CLEAR AUTO ALLOCATION
                    case ActionsEnum.CLEARAUTOALLOCATION:
                        grdCartons.DataSource = null;
                        grdCartons.DataBind();
                        ShowCartonPopup();
                        break;
                    #endregion

                    #region TAXPOPUPDISPLAY
                    case ActionsEnum.TAXPOPUPDISPLAY:
                        ShowCartonPopup();
                        break;
                    #endregion

                    #region SELECTEDINDEXCHANGED
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        GetFieldValues(ControlsEnum.ITEMSTOCK);
                        SetFieldValues(ControlsEnum.ITEMSTOCK);
                        ShowMaterialCartonPopup();
                        break;
                    #endregion

                    #region ADDITEM
                    case ActionsEnum.ADDITEM:
                        if (IsSameItemExist())
                        {
                            ShowMaterialCartonPopup();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SameItemExist").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        SetUIValuesToObject(ControlsEnum.ADDITEM);
                        BindGrid(ControlsEnum.ITEMDETAILS);
                        ResetForm(ControlsEnum.ITEMDETAILS);
                        ShowMaterialCartonPopup();
                        break;
                    #endregion

                    #region ADDBINCARD
                    case ActionsEnum.ADDBINCARD:
                        if (IsSameItemExist())
                        {
                            ShowMaterialCartonPopup();
                            // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SameItemExist").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrMessageFocusCtrl('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SameItemExist").ToString()) + "',null,null,null,'" + txtIssueBinCard.ClientID + "');", true);

                            return;
                        }
                        GetFieldValues(ControlsEnum.ADDBINCARD);
                        if (objNewBins == null)
                        {
                            ShowMaterialCartonPopup();
                            return;
                        }
                        if (objNewBins.Detail == null)
                        {
                            ShowMaterialCartonPopup();
                            return;
                        }
                        if (!objNewBins.Detail.Any())
                        {
                            ShowMaterialCartonPopup();
                            return;
                        }

                        #region DB object to  container object

                        int sNo = 1;

                        if (ContainerItemsObj == null)
                        {
                            ContainerItemsObj = new ContainerItems();
                            ContainerItemsObj.SaleContract = lblSCNo.Text;
                            ContainerItemsObj.SaleContractPK = Convert.ToInt32(hdfSCPK.Value);
                            ContainerItemsObj.ItemName = lblItemName.Text;
                            ContainerItemsObj.ItemPK = Convert.ToInt32(hdfItemPK.Value);
                            ContainerItemsObj.Store = ddlIssueStore.SelectedItem.Text;
                            ContainerItemsObj.StorePK = Convert.ToInt32(ddlIssueStore.SelectedValue);
                            ContainerItemsObj.ItemType = Convert.ToInt32(hdfItemTypePK.Value);
                        }

                        ItemDetails = new ContainerItemDetails();
                        ItemDetails.Bincard = objNewBins.Detail.First().BCH_NO;
                        ItemDetails.BincardPK = objNewBins.Detail.First().BCH_PK;
                        ItemDetails.Quantity = Convert.ToDecimal(objNewBins.Detail.First().BWH_TOTAL_WT);
                        ItemDetails.TotalPcs = Convert.ToDecimal(objNewBins.Detail.First().BWH_TOTAL_PCS);
                        ItemDetails.ItemType = ContainerItemsObj.ItemType;
                        ItemDetails.ItemPK = Convert.ToInt32(hdfItemPK.Value);

                        if (ContainerItemsObj.ItemDetailsList != null && ContainerItemsObj.ItemDetailsList.Any())
                            sNo = ContainerItemsObj.ItemDetailsList.Max(m => m.SlNo) + 1;

                        ItemDetails.SlNo = sNo;

                        if (ContainerItemsObj.ItemDetailsList == null)
                            ContainerItemsObj.ItemDetailsList = new List<ContainerItemDetails>();

                        ContainerItemsObj.ItemDetailsList.Add(ItemDetails);

                        #endregion
                        BindGrid(ControlsEnum.ITEMDETAILS);
                        ResetForm(ControlsEnum.ADDBINCARD);
                        ShowMaterialCartonPopup();
                        break;
                    #endregion

                    #region PRODUCTAPPLY
                    case ActionsEnum.PRODUCTAPPLY:
                        int ProdSaleUnit = Convert.ToInt32(hdfItemSaleUnit.Value);
                        if (ProdSaleUnit == (int)SaleUnit.Ctn)
                        {
                            if (ContainerItemsObj != null && ContainerItemsObj.ItemDetailsList != null &&
                                ContainerItemsObj.ItemDetailsList.Sum(s => s.Quantity) != Convert.ToDecimal(lblQty.Text) && hdfConfirmQuantityNotMatch.Value != 1.ToString())
                            {
                                ShowMaterialCartonPopup();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnConfirmQuantityNotMatch", "ConfirmQtyNotMatchInBin();", true);
                                return;
                            }
                        }
                        else
                        {
                            if (ContainerItemsObj != null && ContainerItemsObj.ItemDetailsList != null &&
                                ContainerItemsObj.ItemDetailsList.Sum(s => s.TotalPcs) != Convert.ToDecimal(lblQty.Text) && hdfConfirmQuantityNotMatch.Value != 1.ToString())
                            {
                                ShowMaterialCartonPopup();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnConfirmQuantityNotMatch", "ConfirmQtyNotMatchInBin();", true);
                                return;
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop", "ClosePopup();", true);
                        ContainerReleaseDetails.Where(f => f.CDR_ITEM == ContainerItemsObj.ItemPK).First().ContainerItems = ContainerItemsObj;
                        decimal totalPcs = 0;
                        if (ContainerItemsObj != null && ContainerItemsObj.ItemDetailsList != null)
                            totalPcs = ContainerItemsObj.ItemDetailsList.Sum(s => s.TotalPcs);
                        ContainerReleaseDetails.Where(f => f.CDR_ITEM == ContainerItemsObj.ItemPK).First().CDR_QTY_DESPATCHED = totalPcs;
                        if (ContainerItemsObj.ItemDetailsList != null && ContainerItemsObj.ItemDetailsList.Count > 0)
                            ContainerReleaseDetails.Where(f => f.CDR_ITEM == ContainerItemsObj.ItemPK).First().CDR_IS_PACKED_BIN = "0";
                        else
                            ContainerReleaseDetails.Where(f => f.CDR_ITEM == ContainerItemsObj.ItemPK).First().CDR_IS_PACKED_BIN = string.Empty;
                        hdfConfirmQuantityNotMatch.Value = "0";
                        BindGrid(ControlsEnum.DODETAILS);

                        hdfAllocationExist.Value = "0";
                        if (ContainerReleaseDetails.Where(w => w.ContainerItems != null && w.ContainerItems.ItemDetailsList.Any()).ToList().Any())
                        {
                            hdfAllocationExist.Value = "1";
                        }
                        break;
                    #endregion

                    #region CLEARADD
                    case ActionsEnum.CLEARADD:
                        ResetForm(ControlsEnum.ITEMDETAILS);
                        ShowMaterialCartonPopup();
                        btnClear.Focus();
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
                if (SortBy == e.SortExpression)
                {
                    //Toggle the sort expression
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
                this.PageIndex = "1";
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>ON SORTING
        /// Sorting Event Handler for grd AssignProducts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            //List<DirectStockAdmissionBO.StockAdmissionItem> tempStockAdmissionItemList;
            GridView senderGridView = (GridView)sender;
            if (senderGridView.ID == "grdDeliveryList")
            {
                if (e.CommandName == "EDIT_ACTION" || e.CommandName == "PACKEDBIN")
                {
                    scrapAllocation = ScrapAllocation.BinCard;
                    if (e.CommandName == "PACKEDBIN")
                        scrapAllocation = ScrapAllocation.PackedBin;
                    // ContainerReleaseDetails
                    ResetForm(ControlsEnum.SEARCH);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfCDR_PK = row.FindControl("hdfCDR_PK") as HiddenField;
                    HiddenField hdfCDR_SL_NO = row.FindControl("hdfCDR_SL_NO") as HiddenField;
                    HiddenField hdfBrandPk = row.FindControl("hdfBrandPk") as HiddenField;
                    HiddenField hdfCDR_DO_QTY = row.FindControl("hdfCDR_DO_QTY") as HiddenField;
                    HiddenField hdfItemType = row.FindControl("hdfItemType") as HiddenField;
                    HiddenField hdfCDR_ITEM = row.FindControl("hdfCDR_ITEM") as HiddenField;
                    HiddenField hdfItemName = row.FindControl("hdfItemName") as HiddenField;
                    HiddenField hdfSaleOrderHdrPK = row.FindControl("hdfSaleOrderHdrPK") as HiddenField;
                    HiddenField hdfSaleUnit = row.FindControl("hdfSaleUnit") as HiddenField;

                    LinkButton lnkSoNo = row.FindControl("lnkSoNo") as LinkButton;
                    Label lblBrandCode = row.FindControl("lblBrandCode") as Label;
                    Label lblDOQty = row.FindControl("lblDOQty") as Label;
                    Label lblITM_CODE = row.FindControl("lblITM_CODE") as Label;

                    lblSCNoPopUp.Text = lnkSoNo.Text;
                    lblSCNo.Text = lnkSoNo.Text;
                    lblBrandNamePopUp.Text = lblBrandCode.Text;
                    lblDOQtyPcs.Text = lblDOQty.Text;
                    lblQty.Text = lblDOQty.Text;
                    lblDONoPopUp.Text = lblGONHdr.Text;
                    // lblProductCodePopUp.Text = lblITM_CODE.Text;

                    hdfCurrentCDR_PK.Value = hdfCDR_PK.Value;
                    int pk = Convert.ToInt32(hdfCDR_PK.Value);
                    int slNo = Convert.ToInt32(hdfCDR_SL_NO.Value);
                    ContainerReleaseDetail releaseDetail = ContainerReleaseDetails.Where(X => X.CDR_PK == pk && X.CDR_SL_NO == slNo)
                        .Single();
                    hdfCDR_SO_DTL.Value = releaseDetail.CDR_SO_DTL.ToString();
                    hdfCDR_SL_NO_PopUp.Value = releaseDetail.CDR_SL_NO.ToString();
                    hdfCurrentSCID.Value = releaseDetail.SOD_SO.ToString();
                    CartonBrandPk = Convert.ToInt32(hdfBrandPk.Value);
                    CartonBrandQty = Convert.ToDecimal(hdfCDR_DO_QTY.Value);
                    CartonsListTemp = releaseDetail.CartonDetails;

                    #region Assign to CartonsGridviewViewState
                    List<CartonsGridview> tempCartonsGridview = new List<CartonsGridview>();
                    var slNos = CartonsListTemp
                            .Select(x => x.CRC_SL_NO_GRP)
                            .Distinct();
                    foreach (var item2 in slNos)
                    {
                        CartonsGridview cartonsGridviewTemp = new CartonsGridview();
                        Cartons tempCarton = CartonsListTemp
                            .Where(x => x.CRC_SL_NO_GRP == item2)
                            .First();
                        cartonsGridviewTemp.CRC_SL_NO_GRP = tempCarton.CRC_SL_NO_GRP;
                        if (tempCarton.BCR_PALLET_NO != null && tempCarton.BCR_PALLET_NO != string.Empty) cartonsGridviewTemp.BCR_PALLET_NO = tempCarton.BCR_PALLET_NO;
                        else cartonsGridviewTemp.BCR_PALLET_NO = GetLocalResourceObject("Carton").ToString();
                        cartonsGridviewTemp.CartonsCount = CartonsListTemp
                            .Where(x => x.CRC_SL_NO_GRP == item2)
                            .Count();
                        cartonsGridviewTemp.QtyPcs = Convert.ToInt32(CartonsListTemp
                            .Where(x => x.CRC_SL_NO_GRP == item2)
                            .Sum(x => x.CRC_QTY_DESPATCHED));
                        cartonsGridviewTemp.BCR_LOCATION_TEXT = CartonsListTemp
                            .Where(x => x.CRC_SL_NO_GRP == item2)
                            .First()
                            .BCR_LOCATION_TEXT;
                        tempCartonsGridview.Add(cartonsGridviewTemp);
                    }
                    CartonsGridviewListViewState = tempCartonsGridview;
                    #endregion
                    BindGrid(ControlsEnum.CONTAINERSPLITUP);
                    chkAutoMode.Checked = false;
                    btnApplyCartonDetails.Focus();
                    if (hdfItemType.Value == "0" || scrapAllocation == ScrapAllocation.PackedBin) //Brand Or Packed Scrap Bin
                        ShowCartonPopup();
                    else if (hdfItemType.Value == "1" || hdfItemType.Value == "2" || hdfItemType.Value == "3" || hdfItemType.Value == "4" || hdfItemType.Value == "5") //Material || Product || Bgrade || Scrap || Brand with Product Wallet
                    {
                        hdfContainerReleaseDetailPK.Value = hdfCDR_PK.Value;
                        if (ContainerReleaseDetails.Where(f => f.CDR_ITEM == Convert.ToInt32(hdfCDR_ITEM.Value)).First().ContainerItems != null)
                        {
                            ContainerItemsObj = ContainerReleaseDetails.Where(f => f.CDR_ITEM == Convert.ToInt32(hdfCDR_ITEM.Value)).First().ContainerItems;
                            lblItemName.Text = ContainerItemsObj.ItemName;
                            hdfItemPK.Value = ContainerItemsObj.ItemPK.ToString();
                            hdfItemSaleUnit.Value = ContainerReleaseDetails.Where(f => f.CDR_ITEM == Convert.ToInt32(hdfCDR_ITEM.Value)).First().SaleUnit.ToString();
                            hdfItemTypePK.Value = ContainerItemsObj.ItemType.ToString();
                            hdfSCPK.Value = ContainerItemsObj.SaleContractPK.ToString();
                            hdfContainerReleaseDetailPK.Value = ContainerItemsObj.ContainerReleaseDetailPK.ToString();
                        }
                        else
                        {
                            ContainerItemsObj = ContainerReleaseDetails.Where(f => f.CDR_ITEM == Convert.ToInt32(hdfCDR_ITEM.Value)).First().ContainerItems;
                            lblItemName.Text = hdfItemName.Value;
                            hdfItemPK.Value = hdfCDR_ITEM.Value;
                            hdfItemSaleUnit.Value = hdfSaleUnit.Value;
                            hdfItemTypePK.Value = hdfItemType.Value;
                            hdfSCPK.Value = hdfSaleOrderHdrPK.Value;
                            hdfContainerReleaseDetailPK.Value = hdfCDR_PK.Value;
                        }

                        itemPK = Convert.ToInt32(hdfItemPK.Value);
                        ItemType = Convert.ToInt32(hdfItemType.Value);

                        GetFieldValues(ControlsEnum.ISSUESTORE);
                        SetFieldValues(ControlsEnum.ISSUESTORE);
                        if (ItemType == 1)//material
                        {
                            grdProduct.Visible = false;
                            grdItems.Visible = true;
                            divMaterial.Visible = true;
                            divMaterial2.Visible = true;
                            divProduct.Visible = false;
                            GetFieldValues(ControlsEnum.BATCH);
                            SetFieldValues(ControlsEnum.BATCH);
                        }
                        else if (ItemType == 2 || ItemType == 3 || ItemType == 4 || ItemType == 5)//2.product, 3.B-Grade, 4.Scrap, 5. Wallet
                        {
                            grdProduct.Visible = true;
                            grdItems.Visible = false;
                            divMaterial.Visible = false;
                            divMaterial2.Visible = false;
                            divProduct.Visible = true;
                        }
                        BindGrid(ControlsEnum.ITEMDETAILS);
                        ShowMaterialCartonPopup();
                    }
                    //else if (hdfItemType.Value == "2") //Product
                    //    ShowProductCartonPopup();
                    txtPalleteBinCard.Focus();
                }
                else if (e.CommandName == "REMOVEITEM")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfCDR_PK = row.FindControl("hdfCDR_PK") as HiddenField;
                    HiddenField hdfCDR_SL_NO = row.FindControl("hdfCDR_SL_NO") as HiddenField;
                    int pk = Convert.ToInt32(hdfCDR_PK.Value);
                    int slNo = Convert.ToInt32(hdfCDR_SL_NO.Value);
                    bool isContinue = true;
                    hdfButtonID.Value = row.FindControl("btnRemoveItem").ClientID;
                    hdfConfirmMessage.Value = GetLocalResourceObject("Err_Delete_Warning").ToString();

                    ContainerReleaseDetail releaseDetailObj = ContainerReleaseDetails.Where(X => X.CDR_PK == pk && X.CDR_SL_NO == slNo).Single();
                    if ((hdfConfirmDeleteAllocation.Value == "0") || (hdfConfirmDeleteAllocation.Value == ""))
                    {
                        if (releaseDetailObj.CartonDetails.Count > 0)
                        {
                            hdfConfirmMessage.Value = GetLocalResourceObject("Err_CartonAllocExist").ToString();//Carton allocation exists.Do you want to delete the record?                        
                        }
                        isContinue = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ConfirmAllocationDeletion", "ShowConfirmDeleteAllocation('" + hdfButtonID.Value + "');", true);
                    }
                    if (isContinue)
                    {
                        ContainerReleaseDetails = ContainerReleaseDetails.Where(r => slNo != r.CDR_SL_NO).ToList();
                        BindGrid(ControlsEnum.DODETAILS);
                        hdfConfirmDeleteAllocation.Value = "0";
                    }
                }
            }
            else if (senderGridView.ID == "grdCartons")
            {
                if (e.CommandName == "REMOVEITEM")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);

                    //HiddenField hdfSlNo = row.FindControl("hdfSlNo") as HiddenField;
                    //int slNo = Convert.ToInt32(hdfSlNo.Value);
                    //List<Cartons> temp = CartonsListTemp;
                    //Cartons splitUp = temp.Where(x => x.CRC_SL_NO == slNo)
                    //    .Single();
                    //temp.Remove(splitUp);
                    //CartonsListTemp = temp;

                    HiddenField hdfCRC_SL_NO_GRP = row.FindControl("hdfCRC_SL_NO_GRP") as HiddenField;
                    int slNoGrp = GetNullableInt(hdfCRC_SL_NO_GRP.Value).Value;
                    List<Cartons> tempCartons = CartonsListTemp;
                    tempCartons.RemoveAll(x => x.CRC_SL_NO_GRP == slNoGrp);
                    List<CartonsGridview> tempCartonsGridview = CartonsGridviewListViewState;
                    CartonsGridview cGridview = tempCartonsGridview
                        .Where(x => x.CRC_SL_NO_GRP == slNoGrp)
                        .Single();
                    tempCartonsGridview.Remove(cGridview);
                    CartonsGridviewListViewState = tempCartonsGridview;
                    CartonsListTemp = tempCartons;
                    BindGrid(ControlsEnum.CONTAINERSPLITUP);
                    ShowCartonPopup();
                    chkAutoMode.Checked = false;
                    txtPalleteBinCard.Focus();
                }

                else if (e.CommandName == "EDIT_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfCRC_SL_NO_GRP = row.FindControl("hdfCRC_SL_NO_GRP") as HiddenField;
                    int slNoGrp = GetNullableInt(hdfCRC_SL_NO_GRP.Value).Value;
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in CartonsListTemp.Where(x => x.CRC_SL_NO_GRP == slNoGrp))
                    {
                        if (item.BCR_IS_PARTIAL == 1)
                            sb.Append("<span style=\"color: red;\">" + item.BCR_NO + "</span>" + ", ");
                        else
                            sb.Append(item.BCR_NO + ", ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                    //divMsgCartonDtlsPopUpDetail.InnerText = sb.ToString();
                    hdfCartons.Value = sb.ToString();
                    ShowCartonPopup();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnShowCartons", "fnShowCartons();", true);
                    //ShowCartonPopupDetail();
                }
                else if (e.CommandName == "REMOVEITEMALL")
                {
                    List<Cartons> tempCartons = CartonsListTemp;
                    tempCartons = new List<Cartons>();
                    List<CartonsGridview> tempCartonsGridview = CartonsGridviewListViewState;
                    tempCartonsGridview = new List<CartonsGridview>();
                    CartonsGridviewListViewState = tempCartonsGridview;
                    CartonsListTemp = tempCartons;
                    BindGrid(ControlsEnum.CONTAINERSPLITUP);
                    ShowCartonPopup();
                    chkAutoMode.Checked = false;
                    txtPalleteBinCard.Focus();
                }
            }
            else if (senderGridView.ID == "grdItems")
            {
                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                HiddenField hdfGrdBatchPK = row.FindControl("hdfGrdBatchPK") as HiddenField;
                HiddenField hdfDetailSlNo = row.FindControl("hdfDetailSlNo") as HiddenField;

                ContainerItemDetails item = new ContainerItemDetails();
                item = ContainerItemsObj.ItemDetailsList.First(f => f.BatchPK == Convert.ToInt32(hdfGrdBatchPK.Value));

                if (e.CommandName == "DELETEITEM")
                {
                    ContainerItemsObj.ItemDetailsList.Remove(item);
                    BindGrid(ControlsEnum.ITEMDETAILS);
                }
                else if (e.CommandName == "EDITITEM")
                {
                    hdfBatchPK.Value = item.BatchPK.ToString();
                    txtBatch.Text = item.Batch;
                    hdfSelectedBatch.Value = item.Batch;
                    hdfSelectedBatchPK.Value = item.BatchPK.ToString();
                    GetFieldValues(ControlsEnum.ITEMSTOCK);
                    SetFieldValues(ControlsEnum.ITEMSTOCK);
                    txtQty.Text = item.Quantity.ToString();
                    txtUOM.Text = item.UOM;
                    hdfUOMPK.Value = item.UOMPK.ToString();
                    hdfSlNo.Value = item.SlNo.ToString();
                }
                ShowMaterialCartonPopup();
            }
            else if (senderGridView.ID == "grdProduct")
            {
                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                HiddenField hdfGrdBincardPK = row.FindControl("hdfGrdBincardPK") as HiddenField;
                HiddenField hdfDetailSlNo = row.FindControl("hdfDetailSlNo") as HiddenField;

                ContainerItemDetails item = new ContainerItemDetails();
                item = ContainerItemsObj.ItemDetailsList.Where(f => f.BincardPK == Convert.ToInt32(hdfGrdBincardPK.Value)).First();

                if (e.CommandName == "DELETEITEM")
                {
                    ContainerItemsObj.ItemDetailsList.Remove(item);
                    BindGrid(ControlsEnum.ITEMDETAILS);
                }
                ShowMaterialCartonPopup();
            }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            if (senderGridView.ID == "grdCartons")
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    // Display the summary data in the appropriate cells
                    if (CartonsListTemp != null && CartonsListTemp.Count > 0)
                    {
                        e.Row.Cells[2].Text = Convert.ToString(CartonsListTemp.Count);
                        e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[3].Text = Convert.ToString(CartonsListTemp.Sum(x => x.CRC_QTY_DESPATCHED));
                        e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    }
                }
            }
            else if (senderGridView.ID == "grdDeliveryList")
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    HiddenField hdfIsPackedBin = e.Row.FindControl("hdfIsPackedBin") as HiddenField;
                    HiddenField hdfInvApproved = e.Row.FindControl("hdfInvApproved") as HiddenField;

                    ImageButton imbEditDetails = e.Row.FindControl("imbEditDetails") as ImageButton;
                    ImageButton imbPackedBin = e.Row.FindControl("imbPackedBin") as ImageButton;
                    HiddenField hdfItemType = e.Row.FindControl("hdfItemType") as HiddenField;

                    Label lblDOQty = e.Row.FindControl("lblDOQty") as Label;
                    Label lblDespNow = e.Row.FindControl("lblDespNow") as Label;

                    imbEditDetails.Visible = false;
                    if (hdfItemType.Value == "0" && hdfInvApproved.Value == "1")
                    {
                        imbEditDetails.Visible = true;
                        hdfHasAllocation.Value = "1";
                    }
                    else
                    if (hdfInvApproved.Value == "1" && hdfIsPackedBin.Value != "1")
                    {
                        imbEditDetails.Visible = true;
                        hdfHasAllocation.Value = "1";
                    }

                    #region New
                    if (imbEditDetails.CommandName == "EDIT_ACTION" || imbEditDetails.CommandName == "PACKEDBIN")
                    {
                        scrapAllocation = ScrapAllocation.BinCard;
                        if (imbEditDetails.CommandName == "PACKEDBIN")
                            scrapAllocation = ScrapAllocation.PackedBin;
                    }
                    if (hdfItemType.Value == "0" || scrapAllocation == ScrapAllocation.PackedBin)
                    {
                        hdfHasAllocation.Value = "0";
                    }
                    #endregion

                    //else
                    //    imbEditDetails.Visible = false;

                    if (hdfInvApproved.Value == "1" && (hdfIsPackedBin.Value == "1" || hdfIsPackedBin.Value == string.Empty)
                    && IsScrapBinPacked == 1 && (hdfItemType.Value == "4" || hdfItemType.Value == "3"))
                        imbPackedBin.Visible = true;
                    else
                        imbPackedBin.Visible = false;

                    if (lblDOQty.Text != lblDespNow.Text)
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("RowRedColor").ToString());
                }

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
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
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
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }
        private void ShowHideControls()
        {

            if (GetGlobalResourceObject("ConfigurationsRes", "IsVisibleControls").ToString() == "1")
            {
                txtStartLDate.Visible = true;
                txtTruckLicenseNo.Visible = true;
                txtLicenseNo.Visible = true;
                lblStartLoadingDate.Visible = true;
                lblTrailerNo.Visible = true;
                lblTruckNo.Visible = true;
               
            }
            else
            {
                txtStartLDate.Visible = false;
                txtTruckLicenseNo.Visible = false;
                txtLicenseNo.Visible = false;
                lblStartLoadingDate.Visible = false;
                lblTrailerNo.Visible = false;
                lblTruckNo.Visible = false;
            }
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {

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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
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
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
                //if (GetGlobalResourceObject("ConfigurationsRes", "IsLotNoAutoActive").ToString() == "1")
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetBinAutoComplete", "SetBinAutoComplete();", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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

            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    ucrWrkf.PageUrl = path;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                }
            }
        }

        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            CONTAINERRELEASEHDR,
            SHIPPINGPLANLEVEL,
            DEFAULT,
            COMPANYLIST,
            DODETAILS,
            LOCATION,
            CONTAINERSPLITUP,
            SEARCH,
            AUTOALLOCATE,
            ISSUESTORE,
            BATCH,
            ITEMSTOCK,
            ADDITEM,
            ITEMDETAILS,
            ADDBINCARD
        }

        private enum ScrapAllocation
        {
            BinCard = 1,
            PackedBin = 2
        }
        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 1,
            APPROVED = 2,
            NEW = 0
        }

        public enum SaleUnit
        {
            Pcs	= 1,
            Box = 2,
            Ctn	= 3,
            Bag	= 4,
            Prs	= 5,
            Pouch = 6
        }
        #endregion
    }
}

