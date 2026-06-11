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
using System.Data;
using BusinessObject;
using System.IO;
using BusinessObject.Inventory;
using System.Reflection;

namespace ERPSMS_v01.Inventory.Masters
{
    public partial class PackingSpecMapping : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        /// 





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
        /// Shipping Plan PK
        /// </summary>
        private int PackingPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.PackingPk]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PackingPk] = value;
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

        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private DataTable DynamicControls
        {
            get
            {
                return this.ViewState["DynamicControls"] == null ? null : (DataTable)(this.ViewState["DynamicControls"]);
            }
            set
            {
                this.ViewState["DynamicControls"] = value;
            }
        }

        /// <summary>
        /// PackingPk
        /// </summary>
        private int PackingSpecPk
        {
            get
            {
                return this.ViewState[ViewstateStrings.PackingSpecPk] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.PackingSpecPk]) : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.PackingSpecPk] = value;
            }
        }

        #endregion

        private ActionsEnum commonActions;
        //page related class objects      
        private Packingmapping packingSpecMstObj;
        private ADM_CONST_MST admConstMstObj;
        private ServiceUtility serviceUtilityObj;
        //List for binding details to controls
        //private List<ADM_PACKING_SPEC_MST> packingSpecMstList;
        private List<ADM_CONST_MST> admConstMstList;

        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;

        private DataTable dtPageData;
        private DataTable dtControlsData;
        private DataTable dtItemDetails;
        private DataTable dtBrandDetails;
        private DataSet dsPackingMaster;
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

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            int itemPk = 0;
            int customerPK = 0;

            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        SortBy = SortBy == null ? Resources.DataFieldRes.ItemCode : SortBy;
                        SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        int CID = hdfCustomerID.Value != string.Empty ? Convert.ToInt32(hdfCustomerID.Value) : 0;
                        int BID = hdfBrandID.Value != string.Empty ? Convert.ToInt32(hdfBrandID.Value) : 0;
                        int active = Convert.ToInt32(ddlActive.SelectedValue);
                        if (CID > 0)
                        {
                            dtPageData = BusinessLogic.Inventory.PackingMasterBL.GetPackingMappingList(CurrPK, active, currentUser.SBUID, 0, CID, BID, txtAWVersion.Text.Trim(), false, txtPackSpecCode.Text.Trim());
                        }
                        else
                        {
                            dtPageData = BusinessLogic.Inventory.PackingMasterBL.GetPackingMappingList(CurrPK, active, currentUser.SBUID, PackingPK, CID, BID, txtAWVersion.Text.Trim());
                        }
                        break;
                    case ControlsEnum.PACKING:
                        dtPageData = BusinessLogic.Inventory.PackingMasterBL.GetPackingMappingList(CurrPK, 2, currentUser.SBUID, 0, 0, 0, string.Empty);
                        break;
                    case ControlsEnum.CONTROLS:
                        dtControlsData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Packing, (int)PackingType.PakingMaterialType, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.POUCHPACK:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfPouchPack.Value), customerPK, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.SACKBAG:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfSackBag.Value), customerPK, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.INNERBOX:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfInnerBox.Value), customerPK, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.MINIINNERCARTON:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfMiniInnerCarton.Value), customerPK, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.ZIPPERBAG:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfZipperBag.Value), customerPK, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.MASTERCARTON:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfMasterCarton.Value), customerPK, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.POUCHPACKDETAILS:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        itemPk = Convert.ToInt32(hdfAutoPouchPack.Value);//Convert.ToInt32(ddlPouchPack.SelectedValue);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, itemPk, 0, 0, 1, currentUser.SBUID);
                        hdfPouchPackCode.Value = (dtItemDetails != null && dtItemDetails.Rows.Count > 0) ? dtItemDetails.Rows[0]["ITM_CODE"].ToString() : "";
                        break;
                    case ControlsEnum.SACKBAGDETAILS:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        itemPk = Convert.ToInt32(hdfAutoSackBag.Value);//Convert.ToInt32(ddlSackBag.SelectedValue);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, itemPk, 0, 0, 1, currentUser.SBUID);
                        hdfSackBagCode.Value = (dtItemDetails != null && dtItemDetails.Rows.Count > 0) ? dtItemDetails.Rows[0]["ITM_CODE"].ToString() : "";
                        break;
                    case ControlsEnum.INNERBOXDETAILS:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        itemPk = Convert.ToInt32(hdfAutoInnerBox.Value);//Convert.ToInt32(ddlInnerBox.SelectedValue);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, itemPk, 0, 0, 1, currentUser.SBUID);
                        hdfInnerBoxCode.Value = (dtItemDetails != null && dtItemDetails.Rows.Count > 0) ? dtItemDetails.Rows[0]["ITM_CODE"].ToString() : "";
                        break;
                    case ControlsEnum.MINIINNERCARTONDETAILS:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        itemPk = Convert.ToInt32(hdfAutoMiniInnerCarton.Value);//Convert.ToInt32(ddlMiniInnerCarton.SelectedValue);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, itemPk, 0, 0, 1, currentUser.SBUID);
                        hdfMiniInnerCartonCode.Value = (dtItemDetails != null && dtItemDetails.Rows.Count > 0) ? dtItemDetails.Rows[0]["ITM_CODE"].ToString() : "";
                        break;
                    case ControlsEnum.ZIPPERBAGDETAILS:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        itemPk = Convert.ToInt32(hdfAutoZipperBag.Value);//Convert.ToInt32(ddlZipperBag.SelectedValue);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, itemPk, 0, 0, 1, currentUser.SBUID);
                        hdfZipperBagCode.Value = (dtItemDetails != null && dtItemDetails.Rows.Count > 0) ? dtItemDetails.Rows[0]["ITM_CODE"].ToString() : "";
                        break;
                    case ControlsEnum.MASTERCARTONDETAILS:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        itemPk = Convert.ToInt32(hdfAutoMasterCarton.Value);//Convert.ToInt32(ddlMasterCarton.SelectedValue);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, itemPk, 0, 0, 1, currentUser.SBUID);
                        hdfMasterCartonCode.Value = (dtItemDetails != null && dtItemDetails.Rows.Count > 0) ? dtItemDetails.Rows[0]["ITM_CODE"].ToString() : "";
                        break;
                    case ControlsEnum.BRANDDETAILS:
                        int BrandID = hdfBrand.Value != string.Empty ? Convert.ToInt32(hdfBrand.Value) : 0;
                        dtBrandDetails = BusinessLogic.Sales.CustomerProduct.GetCustomerProduct(BrandID, 0, 0, string.Empty, "%%", currentUser.SBUID, 2, false).Tables[0];
                        break;
                    case ControlsEnum.PACKINGSPECMASTERBYPK:
                        dsPackingMaster = BusinessLogic.Inventory.PackingMasterBL.GetPackingMaster(PackingPK, Convert.ToInt16(DbActiveStatus.HASPK), currentUser.SBUID, string.Empty, string.Empty, string.Empty);
                        break;
                    case ControlsEnum.POB:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfPOB.Value), customerPK, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.POBDETAILS:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        itemPk = Convert.ToInt32(hdfAutoPOB.Value);//Convert.ToInt32(ddlPOB.SelectedValue); // itemPk = Convert.ToInt32(ddlPOB.SelectedValue == string.Empty ? "-1" : ddlPOB.SelectedValue);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, itemPk, 0, 0, 1, currentUser.SBUID);
                        hdfPOBCode.Value = (dtItemDetails != null && dtItemDetails.Rows.Count > 0) ? dtItemDetails.Rows[0]["ITM_CODE"].ToString() : "";
                        break;
                    case ControlsEnum.PRB:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfPolybag.Value), customerPK, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.PRBDETAILS:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        itemPk = Convert.ToInt32(hdfAutoPolybag.Value);//Convert.ToInt32(ddlPolybag.SelectedValue); // itemPk = Convert.ToInt32(ddlPolybag.SelectedValue == string.Empty ? "-1" : ddlPolybag.SelectedValue);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, itemPk, 0, 0, 1, currentUser.SBUID);
                        hdfPolybagCode.Value = (dtItemDetails != null && dtItemDetails.Rows.Count > 0) ? dtItemDetails.Rows[0]["ITM_CODE"].ToString() : "";
                        break;
                    case ControlsEnum.WLT:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        //dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfCustomer.Value), customerPK, 1, currentUser.SBUID);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfWallet.Value), customerPK, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.WLTDETAILS:
                        dtItemDetails = null;
                        customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                        itemPk = Convert.ToInt32(hdfAutoWallet.Value);//Convert.ToInt32(ddlWallet.SelectedValue);// itemPk = Convert.ToInt32(ddlWallet.SelectedValue == string.Empty ? "-1" : ddlWallet.SelectedValue);
                        dtItemDetails = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, itemPk, 0, 0, 1, currentUser.SBUID);
                        hdfWalletCode.Value = (dtItemDetails != null && dtItemDetails.Rows.Count > 0) ? dtItemDetails.Rows[0]["ITM_CODE"].ToString() : "";
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
                    case ControlsEnum.PACKING:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        break;
                    case ControlsEnum.CONTROLS:
                        SetControls();
                        break;
                    case ControlsEnum.POUCHPACK:
                    case ControlsEnum.INNERBOX:
                    case ControlsEnum.MINIINNERCARTON:
                    case ControlsEnum.ZIPPERBAG:
                    case ControlsEnum.MASTERCARTON:
                    case ControlsEnum.SACKBAG:
                    case ControlsEnum.POB:
                    case ControlsEnum.PRB:
                    case ControlsEnum.WLT:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.POUCHPACKDETAILS:
                    case ControlsEnum.INNERBOXDETAILS:
                    case ControlsEnum.MINIINNERCARTONDETAILS:
                    case ControlsEnum.ZIPPERBAGDETAILS:
                    case ControlsEnum.MASTERCARTONDETAILS:
                    case ControlsEnum.SACKBAGDETAILS:
                    case ControlsEnum.POBDETAILS:
                    case ControlsEnum.PRBDETAILS:
                    case ControlsEnum.WLTDETAILS:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.BRANDDETAILS:
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

        #region Helper Methods

        private void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.POUCHPACK:
                        //ddlPouchPack.Items.Clear();
                        //if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        //{
                        //    ddlPouchPack.DataSource = dtItemDetails;
                        //    ddlPouchPack.DataTextField = Resources.DataFieldRes.ItemName; // IPDITEMText;
                        //    ddlPouchPack.DataValueField = Resources.DataFieldRes.IPDITEM;
                        //    ddlPouchPack.DataBind();
                        //}
                        //ddlPouchPack.Items.Insert(0, new ListItem(Resources.Captions.SelectText, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.INNERBOX:
                        //ddlInnerBox.Items.Clear();
                        //if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        //{
                        //    ddlInnerBox.DataSource = dtItemDetails;
                        //    ddlInnerBox.DataTextField = Resources.DataFieldRes.ItemName; // IPDITEMText;
                        //    ddlInnerBox.DataValueField = Resources.DataFieldRes.IPDITEM;
                        //    ddlInnerBox.DataBind();
                        //}
                        //ddlInnerBox.Items.Insert(0, new ListItem(Resources.Captions.SelectText, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.MINIINNERCARTON:
                        //ddlMiniInnerCarton.Items.Clear();
                        //if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        //{
                        //    ddlMiniInnerCarton.DataSource = dtItemDetails;
                        //    ddlMiniInnerCarton.DataTextField = Resources.DataFieldRes.ItemName; // IPDITEMText;
                        //    ddlMiniInnerCarton.DataValueField = Resources.DataFieldRes.IPDITEM;
                        //    ddlMiniInnerCarton.DataBind();
                        //}
                        //ddlMiniInnerCarton.Items.Insert(0, new ListItem(Resources.Captions.SelectText, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.ZIPPERBAG:
                        //ddlZipperBag.Items.Clear();
                        //if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        //{
                        //    ddlZipperBag.DataSource = dtItemDetails;
                        //    ddlZipperBag.DataTextField = Resources.DataFieldRes.ItemName; // IPDITEMText;
                        //    ddlZipperBag.DataValueField = Resources.DataFieldRes.IPDITEM;
                        //    ddlZipperBag.DataBind();
                        //}
                        //ddlZipperBag.Items.Insert(0, new ListItem(Resources.Captions.SelectText, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.MASTERCARTON:
                        //ddlMasterCarton.Items.Clear();
                        //if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        //{
                        //    ddlMasterCarton.DataSource = dtItemDetails;
                        //    ddlMasterCarton.DataTextField = Resources.DataFieldRes.ItemName; // IPDITEMText;
                        //    ddlMasterCarton.DataValueField = Resources.DataFieldRes.IPDITEM;
                        //    ddlMasterCarton.DataBind();
                        //}
                        //ddlMasterCarton.Items.Insert(0, new ListItem(Resources.Captions.SelectText, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.SACKBAG:
                        //ddlSackBag.Items.Clear();
                        //if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        //{
                        //    ddlSackBag.DataSource = dtItemDetails;
                        //    ddlSackBag.DataTextField = Resources.DataFieldRes.ItemName; // IPDITEMText;
                        //    ddlSackBag.DataValueField = Resources.DataFieldRes.IPDITEM;
                        //    ddlSackBag.DataBind();
                        //}
                        //ddlSackBag.Items.Insert(0, new ListItem(Resources.Captions.SelectText, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.POB:
                        //ddlPOB.Items.Clear();
                        //if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        //{
                        //    ddlPOB.DataSource = dtItemDetails;
                        //    ddlPOB.DataTextField = Resources.DataFieldRes.ItemName; // IPDITEMText;
                        //    ddlPOB.DataValueField = Resources.DataFieldRes.IPDITEM;
                        //    ddlPOB.DataBind();
                        //}
                        //ddlPOB.Items.Insert(0, new ListItem(Resources.Captions.SelectText, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.PRB:
                        //ddlPolybag.Items.Clear();
                        //if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        //{
                        //    ddlPolybag.DataSource = dtItemDetails;
                        //    ddlPolybag.DataTextField = Resources.DataFieldRes.ItemName; // IPDITEMText;
                        //    ddlPolybag.DataValueField = Resources.DataFieldRes.IPDITEM;
                        //    ddlPolybag.DataBind();
                        //}
                        //ddlPolybag.Items.Insert(0, new ListItem(Resources.Captions.SelectText, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.WLT:
                        //ddlWallet.Items.Clear();
                        //if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        //{
                        //    ddlWallet.DataSource = dtItemDetails;
                        //    ddlWallet.DataTextField = Resources.DataFieldRes.ItemName; // IPDITEMText;
                        //    ddlWallet.DataValueField = Resources.DataFieldRes.IPDITEM;
                        //    ddlWallet.DataBind();
                        //}
                        //ddlWallet.Items.Insert(0, new ListItem(Resources.Captions.SelectText, CommonConstants.SELECTVAL));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void SetControls()
        {
            if (dtControlsData != null)
            {
                if (dtControlsData.Rows.Count > 0)
                {

                    DataView dv = dtControlsData.DefaultView;
                    dv.Sort = Resources.DataFieldRes.ConstValue + " " + Resources.Constants.DefaultSortDirection;
                    DataTable dtControlsDataSort = dv.ToTable();


                    hdfPouchPack.Value = dtControlsDataSort.Rows[0][Resources.DataFieldRes.ConstPK].ToString();
                    lblPouchPack.Text = dtControlsDataSort.Rows[0][Resources.DataFieldRes.ConstName].ToString();
                    //GetFieldValues(ControlsEnum.POUCHPACK);//These Functionalities are done in  setPackingMatAuto();
                    //SetFieldValues(ControlsEnum.POUCHPACK);


                    if (dtControlsDataSort.Rows.Count > 1)
                    {
                        hdfInnerBox.Value = dtControlsDataSort.Rows[1][Resources.DataFieldRes.ConstPK].ToString();
                        lblInnerBox.Text = dtControlsDataSort.Rows[1][Resources.DataFieldRes.ConstName].ToString();
                        //GetFieldValues(ControlsEnum.INNERBOX);
                        //SetFieldValues(ControlsEnum.INNERBOX);
                    }
                    if (dtControlsDataSort.Rows.Count > 2)
                    {

                        hdfMiniInnerCarton.Value = dtControlsDataSort.Rows[2][Resources.DataFieldRes.ConstPK].ToString();
                        lblMiniInnerCarton.Text = dtControlsDataSort.Rows[2][Resources.DataFieldRes.ConstName].ToString();
                        //GetFieldValues(ControlsEnum.MINIINNERCARTON);
                        //SetFieldValues(ControlsEnum.MINIINNERCARTON);
                    }

                    if (dtControlsDataSort.Rows.Count > 3)
                    {
                        hdfZipperBag.Value = dtControlsDataSort.Rows[3][Resources.DataFieldRes.ConstPK].ToString();
                        lblZipperBag.Text = dtControlsDataSort.Rows[3][Resources.DataFieldRes.ConstName].ToString();
                        //GetFieldValues(ControlsEnum.ZIPPERBAG);
                        //SetFieldValues(ControlsEnum.ZIPPERBAG);
                    }

                    if (dtControlsDataSort.Rows.Count > 4)
                    {
                        hdfMasterCarton.Value = dtControlsDataSort.Rows[4][Resources.DataFieldRes.ConstPK].ToString();
                        lblMasterCarton.Text = dtControlsDataSort.Rows[4][Resources.DataFieldRes.ConstName].ToString();
                        //GetFieldValues(ControlsEnum.MASTERCARTON);
                        //SetFieldValues(ControlsEnum.MASTERCARTON);
                    }

                    if (dtControlsDataSort.Rows.Count > 5)
                    {
                        hdfSackBag.Value = dtControlsDataSort.Rows[5][Resources.DataFieldRes.ConstPK].ToString();
                        lblSackBag.Text = dtControlsDataSort.Rows[5][Resources.DataFieldRes.ConstName].ToString();
                        //GetFieldValues(ControlsEnum.SACKBAG);
                        //SetFieldValues(ControlsEnum.SACKBAG);
                    }

                    //var pobList = dtControlsDataSort.AsEnumerable()
                    //     .Where(l => l.Field<string>("CON_DATA") == "POB")
                    //     .SingleOrDefault();
                    //if (pobList != null)
                    //{
                    if (dtControlsDataSort.Rows.Count > 6)
                    {
                        //hdfPOB.Value = (pobList.Field<int>(Resources.DataFieldRes.ConstPK)).ToString();
                        //lblPOB.Text = pobList.Field<string>(Resources.DataFieldRes.ConstName);

                        //hdfPOB.Value = dtControlsDataSort.Rows[6][Resources.DataFieldRes.ConstPK].ToString();
                        //lblPOB.Text = dtControlsDataSort.Rows[6][Resources.DataFieldRes.ConstName].ToString();
                        //GetFieldValues(ControlsEnum.POB);
                        //SetFieldValues(ControlsEnum.POB);
                        hdfWallet.Value = dtControlsDataSort.Rows[6][Resources.DataFieldRes.ConstPK].ToString();
                        lblWallet.Text = dtControlsDataSort.Rows[6][Resources.DataFieldRes.ConstName].ToString();
                        //GetFieldValues(ControlsEnum.WLT);
                        //SetFieldValues(ControlsEnum.WLT);
                    }
                    //var prbList = dtControlsDataSort.AsEnumerable()
                    //    .Where(l => l.Field<string>("CON_DATA") == "PRB")
                    //    .SingleOrDefault();
                    //if (prbList != null)
                    //{
                    if (dtControlsDataSort.Rows.Count > 7)
                    {
                        //hdfPolybag.Value = (prbList.Field<int>(Resources.DataFieldRes.ConstPK)).ToString();
                        //lblPolybag.Text = prbList.Field<string>(Resources.DataFieldRes.ConstName);

                        ////hdfPolybag.Value = dtControlsDataSort.Rows[7][Resources.DataFieldRes.ConstPK].ToString();
                        ////lblPolybag.Text = dtControlsDataSort.Rows[7][Resources.DataFieldRes.ConstName].ToString();
                        ////GetFieldValues(ControlsEnum.PRB);
                        ////SetFieldValues(ControlsEnum.PRB);
                        hdfPOB.Value = dtControlsDataSort.Rows[7][Resources.DataFieldRes.ConstPK].ToString();
                        lblPOB.Text = dtControlsDataSort.Rows[7][Resources.DataFieldRes.ConstName].ToString();
                        //GetFieldValues(ControlsEnum.POB);
                        //SetFieldValues(ControlsEnum.POB);
                    }
                    //var wltList = dtControlsDataSort.AsEnumerable()
                    //   .Where(l => l.Field<string>("CON_DATA") == "WLT")
                    //   .SingleOrDefault();
                    //if (wltList != null)
                    //{
                    if (dtControlsDataSort.Rows.Count > 8)
                    {
                        //hdfWallet.Value = (wltList.Field<int>(Resources.DataFieldRes.ConstPK)).ToString();
                        //lblWallet.Text = wltList.Field<string>(Resources.DataFieldRes.ConstName);

                        ////hdfWallet.Value = dtControlsDataSort.Rows[8][Resources.DataFieldRes.ConstPK].ToString();
                        ////lblWallet.Text = dtControlsDataSort.Rows[8][Resources.DataFieldRes.ConstName].ToString();
                        ////GetFieldValues(ControlsEnum.WLT);
                        ////SetFieldValues(ControlsEnum.WLT);
                        hdfPolybag.Value = dtControlsDataSort.Rows[8][Resources.DataFieldRes.ConstPK].ToString();
                        lblPolybag.Text = dtControlsDataSort.Rows[8][Resources.DataFieldRes.ConstName].ToString();
                        //GetFieldValues(ControlsEnum.PRB);
                        //SetFieldValues(ControlsEnum.PRB);
                    }

                    if (dtControlsDataSort.Rows.Count > 9)
                    {
                        BindDynamicTable(dtControlsDataSort);
                    }
                }
                GetFieldValues(ControlsEnum.PACKINGSPECMASTERBYPK);
                if (dsPackingMaster != null && dsPackingMaster.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_PC_PCS"].ToString()) <= 0)
                    {
                        txtPouchPack.Enabled = false;//ddlPouchPack.Enabled = false;
                    }
                    if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_IB_PCS"].ToString()) <= 0)
                    {
                        txtInnerBox.Enabled = false;//ddlInnerBox.Enabled = false;
                    }
                    if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_IC_PCS"].ToString()) <= 0)
                    {
                        txtMiniInnerCarton.Enabled = false;//ddlMiniInnerCarton.Enabled = false;
                    }
                    if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_ZB_PCS"].ToString()) <= 0)
                    {
                        txtZipperBag.Enabled = false;//ddlZipperBag.Enabled = false;
                    }
                    if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_MC_PCS"].ToString()) <= 0)
                    {
                        txtMasterCarton.Enabled = false; //ddlMasterCarton.Enabled = false;
                    }
                    if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_SC_PCS"].ToString()) <= 0)
                    {
                        txtSackBag.Enabled = false; //ddlSackBag.Enabled = false;
                    }
                    // if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_WLT_PCS"].ToString()) <= 0)
                    if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_POB_PCS"].ToString()) <= 0)//Issue is if wallet value is <0,then disabling POB ddl.Same  as Other two ddl .In the case of wallet,currently given name as ddlPOB. I am only correcting the condition for disabling  Wallet ,Polybag and Printed Outed bag.
                    {
                        txtPOB.Enabled = false; //ddlPOB.Enabled = false;
                    }
                    //if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_POB_PCS"].ToString()) <= 0)
                    if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_PRB_PCS"].ToString()) <= 0)
                    {
                        txtPolybag.Enabled = false; //ddlPolybag.Enabled = false;
                    }
                    // if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_PRB_PCS"].ToString()) <= 0)
                    if (Convert.ToDouble(dsPackingMaster.Tables[0].Rows[0]["APS_WLT_PCS"].ToString()) <= 0)
                    {
                        txtWallet.Enabled = false; //ddlWallet.Enabled = false;
                    }
                }
            }
        }


        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private Packingmapping SetUIValuesToObject()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                packingSpecMstObj.P_PIM_PK = CurrPK;
                packingSpecMstObj.PIM_PACK_SPEC = PackingPK;
                packingSpecMstObj.PIM_CUSTOMER = Convert.ToInt32(hdfCustomer.Value);
                packingSpecMstObj.PIM_CUST_ITEM = Convert.ToInt32(hdfBrand.Value);
                packingSpecMstObj.PIM_ART_WORK = HttpUtility.HtmlEncode(txtArtworkVersion.Text.Trim());
                packingSpecMstObj.PIM_DESC = "";
                packingSpecMstObj.PIM_PC_ITEM = Convert.ToInt32(hdfAutoPouchPack.Value); //ddlPouchPack.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlPouchPack.SelectedValue) : 0;
                packingSpecMstObj.PIM_IB_ITEM = Convert.ToInt32(hdfAutoInnerBox.Value); //ddlInnerBox.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlInnerBox.SelectedValue) : 0;
                packingSpecMstObj.PIM_IC_ITEM = Convert.ToInt32(hdfAutoMiniInnerCarton.Value);//ddlMiniInnerCarton.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlMiniInnerCarton.SelectedValue) : 0;
                packingSpecMstObj.PIM_ZB_ITEM = Convert.ToInt32(hdfAutoZipperBag.Value);//ddlZipperBag.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlZipperBag.SelectedValue) : 0;
                packingSpecMstObj.PIM_MC_ITEM = Convert.ToInt32(hdfAutoMasterCarton.Value);//ddlMasterCarton.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlMasterCarton.SelectedValue) : 0;
                packingSpecMstObj.PIM_SC_ITEM = Convert.ToInt32(hdfAutoSackBag.Value);//ddlSackBag.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlSackBag.SelectedValue) : 0;
                packingSpecMstObj.P_LAST_MOD_DT = LastModifiedTime;
                packingSpecMstObj.P_ACTIVE = Convert.ToInt32(chkStatus.Checked);
                packingSpecMstObj.P_BIZUNIT = currentUser.SBUID;
                packingSpecMstObj.P_USER_PK = currentUser.PKUser;
                packingSpecMstObj.PIM_POB_ITEM = Convert.ToInt32(hdfAutoPOB.Value); //ddlPOB.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlPOB.SelectedValue) : 0;
                packingSpecMstObj.PIM_PRB_ITEM = Convert.ToInt32(hdfAutoPolybag.Value);// ddlPolybag.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlPolybag.SelectedValue) : 0;
                packingSpecMstObj.PIM_WLT_ITEM = Convert.ToInt32(hdfAutoWallet.Value);//ddlWallet.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlWallet.SelectedValue) : 0;

                packingSpecMstObj.P_DETAILS = GetDinamicItemDetails();

                return packingSpecMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                packingSpecMstObj = null;
            }
        }

        //private int? GetItemPK(ControlsEnum controlType)
        //{
        //    GetFieldValues(controlType);
        //    if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
        //    {
        //        return Convert.ToInt32(dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString());
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.PACKING:
                        //assigning the UI controls with the corresponding ListObject value AsrSovMstList
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(dtPageData.Rows[0][Resources.DataFieldRes.PIMPK].ToString());
                            PackingPK = Convert.ToInt32(dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterPK].ToString());
                            setPackingSpecHdr();//filling packing spec in edit mode
                            hdfCustomer.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterCustomer].ToString();
                            lblCustomer.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterCustomerText].ToString();
                            lblCustomer.ToolTip = HttpUtility.HtmlDecode(dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterCustomerText].ToString());
                            hdfBrand.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterItem].ToString();
                            if (hdfBrand.Value != string.Empty && hdfBrand.Value != "0")
                            {
                                GetFieldValues(ControlsEnum.BRANDDETAILS);
                                SetFieldValues(ControlsEnum.BRANDDETAILS);
                            }
                            lblBrand.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterItemText].ToString();
                            lblBrand.ToolTip = dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterItemText].ToString();
                            txtArtworkVersion.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterArtWork] != null ? dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterArtWork].ToString() : string.Empty;
                            if (dtPageData.Rows[0][Resources.DataFieldRes.PackingPP].ToString() != string.Empty)
                            {
                                //ddlPouchPack.SelectedIndex = Convert.ToInt32(ddlPouchPack.Items.IndexOf(ddlPouchPack.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.PackingPP].ToString())));
                                txtPouchPack.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingPPText].ToString();
                                hdfAutoPouchPack.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingPP].ToString();

                                GetFieldValues(ControlsEnum.POUCHPACKDETAILS);
                                SetFieldValues(ControlsEnum.POUCHPACKDETAILS);
                            }
                            if (dtPageData.Rows[0][Resources.DataFieldRes.PackingIB].ToString() != string.Empty)
                            {
                                //ddlInnerBox.SelectedIndex = Convert.ToInt32(ddlInnerBox.Items.IndexOf(ddlInnerBox.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.PackingIB].ToString())));
                                txtInnerBox.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingIBText].ToString();
                                hdfAutoInnerBox.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingIB].ToString();

                                //ddlInnerBox.SelectedValue = dtPageData.Rows[0][Resources.DataFieldRes.PackingIB].ToString();
                                GetFieldValues(ControlsEnum.INNERBOXDETAILS);
                                SetFieldValues(ControlsEnum.INNERBOXDETAILS);
                            }
                            if (dtPageData.Rows[0][Resources.DataFieldRes.PackingMIC].ToString() != string.Empty)
                            {
                                //ddlMiniInnerCarton.SelectedIndex = Convert.ToInt32(ddlMiniInnerCarton.Items.IndexOf(ddlMiniInnerCarton.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.PackingMIC].ToString())));
                                txtMiniInnerCarton.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingMICText].ToString();
                                hdfAutoMiniInnerCarton.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingMIC].ToString();

                                //ddlMiniInnerCarton.SelectedValue = dtPageData.Rows[0][Resources.DataFieldRes.PackingMIC].ToString();
                                GetFieldValues(ControlsEnum.MINIINNERCARTONDETAILS);
                                SetFieldValues(ControlsEnum.MINIINNERCARTONDETAILS);
                            }
                            if (dtPageData.Rows[0][Resources.DataFieldRes.PackingZB].ToString() != string.Empty)
                            {
                                //ddlZipperBag.SelectedIndex = Convert.ToInt32(ddlZipperBag.Items.IndexOf(ddlZipperBag.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.PackingZB].ToString())));
                                txtZipperBag.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingZBText].ToString();
                                hdfAutoZipperBag.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingZB].ToString();

                                //ddlZipperBag.SelectedValue = dtPageData.Rows[0][Resources.DataFieldRes.PackingZB].ToString();
                                GetFieldValues(ControlsEnum.ZIPPERBAGDETAILS);
                                SetFieldValues(ControlsEnum.ZIPPERBAGDETAILS);
                            }
                            if (dtPageData.Rows[0][Resources.DataFieldRes.PackingMC].ToString() != string.Empty)
                            {
                                //ddlMasterCarton.SelectedIndex = Convert.ToInt32(ddlMasterCarton.Items.IndexOf(ddlMasterCarton.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.PackingMC].ToString())));
                                txtMasterCarton.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingMCText].ToString();
                                hdfAutoMasterCarton.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingMC].ToString();

                                //ddlMasterCarton.SelectedValue = dtPageData.Rows[0][Resources.DataFieldRes.PackingMC].ToString();
                                GetFieldValues(ControlsEnum.MASTERCARTONDETAILS);
                                SetFieldValues(ControlsEnum.MASTERCARTONDETAILS);
                            }
                            if (dtPageData.Rows[0][Resources.DataFieldRes.PackingSB].ToString() != string.Empty)
                            {
                                //ddlSackBag.SelectedIndex = Convert.ToInt32(ddlSackBag.Items.IndexOf(ddlSackBag.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.PackingSB].ToString())));
                                txtSackBag.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingSBText].ToString();
                                hdfAutoSackBag.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingSB].ToString();

                                //ddlSackBag.SelectedValue = dtPageData.Rows[0][Resources.DataFieldRes.PackingSB].ToString();
                                GetFieldValues(ControlsEnum.SACKBAGDETAILS);
                                SetFieldValues(ControlsEnum.SACKBAGDETAILS);
                            }
                            if (dtPageData.Rows[0][Resources.DataFieldRes.PackingPOB].ToString() != string.Empty)
                            {
                                //ddlPOB.SelectedIndex = Convert.ToInt32(ddlPOB.Items.IndexOf(ddlPOB.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.PackingPOB].ToString())));
                                txtPOB.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingPOBText].ToString();
                                hdfAutoPOB.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingPOB].ToString();

                                //ddlZipperBag.SelectedValue = dtPageData.Rows[0][Resources.DataFieldRes.PackingZB].ToString();
                                GetFieldValues(ControlsEnum.POBDETAILS);
                                SetFieldValues(ControlsEnum.POBDETAILS);
                            }
                            if (dtPageData.Rows[0][Resources.DataFieldRes.PackingPRB].ToString() != string.Empty)
                            {
                                //ddlPolybag.SelectedIndex = Convert.ToInt32(ddlPolybag.Items.IndexOf(ddlPolybag.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.PackingPRB].ToString())));
                                txtPolybag.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingPRBText].ToString();
                                hdfAutoPolybag.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingPRB].ToString();

                                //ddlZipperBag.SelectedValue = dtPageData.Rows[0][Resources.DataFieldRes.PackingZB].ToString();
                                GetFieldValues(ControlsEnum.PRBDETAILS);
                                SetFieldValues(ControlsEnum.PRBDETAILS);
                            }
                            if (dtPageData.Rows[0][Resources.DataFieldRes.PackingWLT].ToString() != string.Empty)
                            {
                                //ddlWallet.SelectedIndex = Convert.ToInt32(ddlWallet.Items.IndexOf(ddlWallet.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.PackingWLT].ToString())));
                                txtWallet.Text = dtPageData.Rows[0][Resources.DataFieldRes.PackingWLTText].ToString();
                                hdfAutoWallet.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingWLT].ToString();
                                //ddlZipperBag.SelectedValue = dtPageData.Rows[0][Resources.DataFieldRes.PackingZB].ToString();
                                GetFieldValues(ControlsEnum.WLTDETAILS);
                                SetFieldValues(ControlsEnum.WLTDETAILS);
                            }

                            if (Convert.ToInt32(dtPageData.Rows[0][Resources.DataFieldRes.PackingActive].ToString()) == 1)
                            {
                                chkStatus.Checked = true;
                                //chkStatus.Enabled = false;
                            }
                            else
                            {
                                chkStatus.Checked = false;
                                chkStatus.Enabled = true;
                            }
                            LastModifiedTime = Convert.ToDateTime(dtPageData.Rows[0][Resources.DataFieldRes.PackingModDt].ToString());
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);

                            if (Convert.ToInt32(dtPageData.Rows[0]["USED_COUNT"].ToString()) > 0)
                            {
                                btnSave.Visible = false;
                            }
                        }
                        //Concurrency Account details Deleted By Another User
                        else
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Packing);
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm();
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            ModifiedDatePnl.Visible = false;
                            throw new Exception(litErrorMsg.Text);
                        }
                        break;
                    case ControlsEnum.POUCHPACKDETAILS:
                        ClearControl(ControlsEnum.POUCHPACKDETAILS);
                        if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        {
                            //lblPPArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            //lnkPPArtWork.InnerText = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblPPArtWork.Text = "<span>" + dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty + "</span>";
                            lblPPArtWork.Visible = true;
                            lnkPPArtWork.Visible = false;
                            if (File.Exists(Server.MapPath(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString())))
                            {
                                lnkPPArtWork.InnerText = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                                lnkPPArtWork.HRef = Page.ResolveClientUrl(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString());
                                lnkPPArtWork.Attributes.Add("target", "_blank");
                                lblPPArtWork.Visible = false;
                                lnkPPArtWork.Visible = true;
                            }
                            else
                            {
                                //lnkPPArtWork.HRef =  "javascript:FileNotFound();";
                                //lnkPPArtWork.Attributes.Add("target", "");
                            }
                            lblPPColour.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour].ToString() : string.Empty;
                            lblPPColour.ToolTip = lblPPColour.Text;
                            lblPPDimension.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension].ToString() : string.Empty;
                            lblPPDimension.ToolTip = lblPPDimension.Text;
                        }
                        break;
                    case ControlsEnum.INNERBOXDETAILS:
                        ClearControl(ControlsEnum.INNERBOXDETAILS);
                        if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        {
                            //lblIBArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblIBArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblIBArtWork.Visible = true;
                            lnkIBArtWork.Visible = false;
                            if (File.Exists(Server.MapPath(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString())))
                            {
                                lnkIBArtWork.InnerText = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                                lnkIBArtWork.HRef = Page.ResolveClientUrl(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString());
                                lnkIBArtWork.Attributes.Add("target", "_blank");
                                lblIBArtWork.Visible = false;
                                lnkIBArtWork.Visible = true;
                            }
                            else
                            {
                                //lnkIBArtWork.HRef = "javascript:FileNotFound();";
                                //lnkPPArtWork.Attributes.Add("target", "");
                            }
                            lblIBColour.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour].ToString() : string.Empty;
                            lblIBColour.ToolTip = lblIBColour.Text;
                            lblIBDimension.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension].ToString() : string.Empty;
                            lblIBDimension.ToolTip = lblIBDimension.Text;
                        }
                        break;
                    case ControlsEnum.MINIINNERCARTONDETAILS:
                        ClearControl(ControlsEnum.MINIINNERCARTONDETAILS);
                        if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        {
                            //lblMICArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblMICArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblMICArtWork.Visible = true;
                            lnkMICArtWork.Visible = false;
                            if (File.Exists(Server.MapPath(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString())))
                            {
                                lnkMICArtWork.InnerText = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                                lnkMICArtWork.HRef = Page.ResolveClientUrl(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString());
                                lnkMICArtWork.Attributes.Add("target", "_blank");
                                lblMICArtWork.Visible = false;
                                lnkMICArtWork.Visible = true;
                            }
                            else
                            {
                                //lnkMICArtWork.HRef = "javascript:FileNotFound();";
                                //lnkPPArtWork.Attributes.Add("target", "");
                            }
                            lblMICColour.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour].ToString() : string.Empty;
                            lblMICColour.ToolTip = lblMICColour.Text;
                            lblMICDimension.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension].ToString() : string.Empty;
                            lblMICDimension.ToolTip = lblMICDimension.Text;
                        }
                        break;
                    case ControlsEnum.ZIPPERBAGDETAILS:
                        ClearControl(ControlsEnum.ZIPPERBAGDETAILS);
                        if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        {
                            //lblZBArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblZBArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblZBArtWork.Visible = true;
                            lnkZBArtWork.Visible = false;
                            if (File.Exists(Server.MapPath(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString())))
                            {
                                lnkZBArtWork.InnerText = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                                lnkZBArtWork.HRef = Page.ResolveClientUrl(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString());
                                lnkZBArtWork.Attributes.Add("target", "_blank");
                                lblZBArtWork.Visible = false;
                                lnkZBArtWork.Visible = true;
                            }
                            else
                            {
                                //lnkZBArtWork.HRef = "javascript:FileNotFound();";
                                //lnkZBArtWork.Attributes.Add("target", "");
                            }
                            lblZBColour.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour].ToString() : string.Empty;
                            lblZBColour.ToolTip = lblZBColour.Text;
                            lblZBDimension.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension].ToString() : string.Empty;
                            lblZBDimension.ToolTip = lblZBDimension.Text;
                        }
                        break;
                    case ControlsEnum.MASTERCARTONDETAILS:
                        ClearControl(ControlsEnum.MASTERCARTONDETAILS);
                        if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        {
                            //lblMCArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblMCArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblMCArtWork.Visible = true;
                            lnkMCArtWork.Visible = false;
                            if (File.Exists(Server.MapPath(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString())))
                            {
                                lnkMCArtWork.InnerText = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                                lnkMCArtWork.HRef = Page.ResolveClientUrl(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString());
                                lnkMCArtWork.Attributes.Add("target", "_blank");
                                lblMCArtWork.Visible = false;
                                lnkMCArtWork.Visible = true;
                            }
                            else
                            {
                                //lnkMCArtWork.HRef = "javascript:FileNotFound();";
                                //lnkMCArtWork.Attributes.Add("target", "");
                            }

                            lblMCColour.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour].ToString() : string.Empty;
                            lblMCColour.ToolTip = lblMCColour.Text;
                            lblMCDimension.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension].ToString() : string.Empty;
                            lblMCDimension.ToolTip = lblMCDimension.Text;

                        }
                        break;
                    case ControlsEnum.SACKBAGDETAILS:
                        ClearControl(ControlsEnum.SACKBAGDETAILS);
                        if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        {
                            //lblSBArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblSBArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblSBArtWork.Visible = true;
                            lnkSBArtWork.Visible = false;
                            if (File.Exists(Server.MapPath(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString())))
                            {
                                lnkSBArtWork.InnerText = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                                lnkSBArtWork.HRef = Page.ResolveClientUrl(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString());
                                lnkSBArtWork.Attributes.Add("target", "_blank");
                                lblSBArtWork.Visible = false;
                                lnkSBArtWork.Visible = true;
                            }
                            else
                            {
                                //lnkSBArtWork.HRef = "javascript:FileNotFound();";
                                //lnkSBArtWork.Attributes.Add("target", "");
                            }


                            lblSBColour.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour].ToString() : string.Empty;
                            lblSBColour.ToolTip = lblSBColour.Text;
                            lblSBDimension.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension].ToString() : string.Empty;
                            lblSBDimension.ToolTip = lblSBDimension.Text;

                        }
                        break;
                    case ControlsEnum.BRANDDETAILS:
                        lblBrandCodeText.Text = string.Empty;
                        lblBrandCodeText.ToolTip = string.Empty;
                        lblBrandDetailsText.ToolTip = string.Empty;
                        lblBrandDetailsText.Text = string.Empty;
                        if (dtBrandDetails != null && dtBrandDetails.Rows.Count > 0)
                        {
                            lblBrandCodeText.Text = dtBrandDetails.Rows[0][Resources.DataFieldRes.ItemCode].ToString();
                            lblBrandCodeText.ToolTip = dtBrandDetails.Rows[0][Resources.DataFieldRes.ItemCode].ToString();
                            lblBrandDetailsText.Text = dtBrandDetails.Rows[0][Resources.DataFieldRes.BrandCode].ToString() + " - " + dtBrandDetails.Rows[0][Resources.DataFieldRes.BrandName].ToString();
                            lblBrandDetailsText.ToolTip = dtBrandDetails.Rows[0][Resources.DataFieldRes.BrandCode].ToString() + " - " + dtBrandDetails.Rows[0][Resources.DataFieldRes.BrandName].ToString();
                        }
                        break;
                    case ControlsEnum.POBDETAILS:
                        ClearControl(ControlsEnum.POBDETAILS);
                        if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        {
                            //lblIBArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblPOBArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblPOBArtWork.Visible = true;
                            lnkPOBArtWork.Visible = false;
                            if (File.Exists(Server.MapPath(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString())))
                            {
                                lnkPOBArtWork.InnerText = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                                lnkPOBArtWork.HRef = Page.ResolveClientUrl(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString());
                                lnkPOBArtWork.Attributes.Add("target", "_blank");
                                lblPOBArtWork.Visible = false;
                                lnkPOBArtWork.Visible = true;
                            }
                            else
                            {
                                //lnkIBArtWork.HRef = "javascript:FileNotFound();";
                                //lnkPPArtWork.Attributes.Add("target", "");
                            }
                            lblPOBColour.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour].ToString() : string.Empty;
                            lblPOBColour.ToolTip = lblPOBColour.Text;
                            lblPOBDimension.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension].ToString() : string.Empty;
                            lblPOBDimension.ToolTip = lblPOBDimension.Text;
                        }
                        break;
                    case ControlsEnum.PRBDETAILS:
                        ClearControl(ControlsEnum.PRBDETAILS);
                        if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        {
                            //lblIBArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblPRBArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblPRBArtWork.Visible = true;
                            lnkPRBArtWork.Visible = false;
                            if (File.Exists(Server.MapPath(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString())))
                            {
                                lnkPRBArtWork.InnerText = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                                lnkPRBArtWork.HRef = Page.ResolveClientUrl(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString());
                                lnkPRBArtWork.Attributes.Add("target", "_blank");
                                lblPRBArtWork.Visible = false;
                                lnkPRBArtWork.Visible = true;
                            }
                            else
                            {
                                //lnkIBArtWork.HRef = "javascript:FileNotFound();";
                                //lnkPPArtWork.Attributes.Add("target", "");
                            }
                            lblPRBColour.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour].ToString() : string.Empty;
                            lblPRBColour.ToolTip = lblPRBColour.Text;
                            lblPRBDimension.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension].ToString() : string.Empty;
                            lblPRBDimension.ToolTip = lblPRBDimension.Text;
                        }
                        break;
                    case ControlsEnum.WLTDETAILS:
                        ClearControl(ControlsEnum.WLTDETAILS);
                        if (dtItemDetails != null && dtItemDetails.Rows.Count > 0)
                        {
                            //lblIBArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblWLTArtWork.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            lblWLTArtWork.Visible = true;
                            lnkWLTArtWork.Visible = false;
                            if (File.Exists(Server.MapPath(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString())))
                            {
                                lnkWLTArtWork.InnerText = dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                                lnkWLTArtWork.HRef = Page.ResolveClientUrl(Resources.PageURL.ArtworkUploads + dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDocument].ToString());
                                lnkWLTArtWork.Attributes.Add("target", "_blank");
                                lblWLTArtWork.Visible = false;
                                lnkWLTArtWork.Visible = true;
                            }
                            else
                            {
                                //lnkIBArtWork.HRef = "javascript:FileNotFound();";
                                //lnkPPArtWork.Attributes.Add("target", "");
                            }
                            lblWLTColour.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PaperColour].ToString() : string.Empty;
                            lblWLTColour.ToolTip = lblWLTColour.Text;
                            lblWLTDimension.Text = dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension] != null ? dtItemDetails.Rows[0][Resources.DataFieldRes.PackingDimension].ToString() : string.Empty;
                            lblWLTDimension.ToolTip = lblWLTDimension.Text;
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
        /// Method for Grid binding
        /// </summary>
        public void BindGrid()
        {
            try
            {
                if (dtPageData != null && dtPageData.Rows.Count > 0)
                {
                    PackingPK = Convert.ToInt32(dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterPK].ToString());
                    //setPackingSpecHdr();
                    dtPageData.DefaultView.Sort = SortBy + " " + SortDirection;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ZERO : PageIndex;
                    grdPackingMst.PageIndex = Convert.ToInt32(PageIndex);
                    grdPackingMst.DataSource = dtPageData;
                    grdPackingMst.DataBind();
                }
                else
                {
                    grdPackingMst.DataSource = null;
                    grdPackingMst.DataBind();
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
                foreach (GridViewRow grdrow in grdPackingMst.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt16(grdPackingMst.DataKeys[grdrow.RowIndex].Values[0]);
                        // Get And Set the Location details
                        GetFieldValues(ControlsEnum.CONTROLS);
                        SetFieldValues(ControlsEnum.CONTROLS);
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

        private void ResetSearch()
        {
            txtCustomerID.Text = string.Empty;
            hdfCustomerID.Value = string.Empty;
            txtBrandID.Text = string.Empty;
            hdfBrandID.Value = string.Empty;
            ddlActive.SelectedValue = CommonConstants.SELECTVAL;
            txtAWVersion.Text = string.Empty;
            txtPackSpecCode.Text = string.Empty;
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            lblCustomer.Text = string.Empty;
            lblCustomer.ToolTip = string.Empty;
            hdfCustomer.Value = string.Empty;
            lblBrand.Text = string.Empty;
            hdfBrand.Value = string.Empty;
            txtArtworkVersion.Text = string.Empty;
            txtPouchPack.Text = string.Empty; hdfAutoPouchPack.Value = "0";// ddlPouchPack.SelectedValue = CommonConstants.SELECTVAL;
            txtInnerBox.Text = string.Empty; hdfAutoInnerBox.Value = "0"; //ddlInnerBox.SelectedValue = CommonConstants.SELECTVAL;
            txtMiniInnerCarton.Text = string.Empty; hdfAutoMiniInnerCarton.Value = "0"; //ddlMiniInnerCarton.SelectedValue = CommonConstants.SELECTVAL;
            txtZipperBag.Text = string.Empty; hdfAutoZipperBag.Value = "0"; //ddlZipperBag.SelectedValue = CommonConstants.SELECTVAL;
            txtMasterCarton.Text = string.Empty; hdfAutoMasterCarton.Value = "0"; //ddlMasterCarton.SelectedValue = CommonConstants.SELECTVAL;

            txtSackBag.Text = string.Empty; hdfAutoSackBag.Value = "0";//ddlSackBag.SelectedValue = CommonConstants.SELECTVAL;
            ClearControl(ControlsEnum.POUCHPACKDETAILS);
            ClearControl(ControlsEnum.INNERBOXDETAILS);
            ClearControl(ControlsEnum.MINIINNERCARTONDETAILS);
            ClearControl(ControlsEnum.ZIPPERBAGDETAILS);
            ClearControl(ControlsEnum.MASTERCARTONDETAILS);
            ClearControl(ControlsEnum.SACKBAGDETAILS);
            PageIndex = CommonConstants.SELECT_VALUE_ZERO;
            lblLastModifiedHDR.Text = string.Empty;
            ModifiedDatePnl.Visible = false;
            chkStatus.Enabled = true;
            lblBrandDetailsText.Text = string.Empty;
            lblBrandCodeText.Text = string.Empty;
            lblBrandCodeText.ToolTip = string.Empty;
            txtPouchPack.Enabled = true;//ddlPouchPack.Enabled = true;
            txtInnerBox.Enabled = true;//ddlInnerBox.Enabled = true;
            txtMiniInnerCarton.Enabled = true;//ddlMiniInnerCarton.Enabled = true;
            txtZipperBag.Enabled = true;//ddlZipperBag.Enabled = true;
            txtMasterCarton.Enabled = true;//ddlMasterCarton.Enabled = true;
            txtSackBag.Enabled = true;//ddlSackBag.Enabled = true;
            hdfMode.Value = string.Empty;
            hdfOldVersion.Value = string.Empty;
            btnSave.Visible = true;
            txtPOB.Text = string.Empty; hdfAutoPOB.Value = "0"; //ddlPOB.SelectedValue = CommonConstants.SELECTVAL;
            ClearControl(ControlsEnum.POBDETAILS);
            txtPOB.Enabled = true;// ddlPOB.Enabled = true;
            txtPolybag.Text = string.Empty; hdfAutoPolybag.Value = "0"; //ddlPolybag.SelectedValue = CommonConstants.SELECTVAL;
            ClearControl(ControlsEnum.PRBDETAILS);
            txtPolybag.Enabled = true;//ddlPolybag.Enabled = true;
            txtWallet.Text = string.Empty; hdfAutoWallet.Value = "0";//ddlWallet.SelectedValue = CommonConstants.SELECTVAL;
            ClearControl(ControlsEnum.WLTDETAILS);
            txtWallet.Enabled = true;//ddlWallet.Enabled = true;
        }
        private void ClearControl(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.POUCHPACKDETAILS:
                    lblPPArtWork.Text = string.Empty;
                    lnkPPArtWork.InnerText = string.Empty;
                    lblPPArtWork.Visible = true;
                    lnkPPArtWork.Visible = false;
                    lblPPDimension.Text = string.Empty;
                    lblPPColour.Text = string.Empty;
                    break;
                case ControlsEnum.INNERBOXDETAILS:
                    lblIBArtWork.Text = string.Empty;
                    lnkIBArtWork.InnerText = string.Empty;
                    lblIBArtWork.Visible = true;
                    lnkIBArtWork.Visible = false;
                    lblIBDimension.Text = string.Empty;
                    lblIBColour.Text = string.Empty;
                    break;
                case ControlsEnum.MINIINNERCARTONDETAILS:
                    lblMICArtWork.Text = string.Empty;
                    lnkMICArtWork.InnerText = string.Empty;
                    lblMICArtWork.Visible = true;
                    lnkMICArtWork.Visible = false;
                    //lnkMICArtWork.Attributes.Add("target", "");
                    lblMICDimension.Text = string.Empty;
                    lblMICColour.Text = string.Empty;
                    break;
                case ControlsEnum.ZIPPERBAGDETAILS:
                    lblZBArtWork.Text = string.Empty;
                    lnkZBArtWork.InnerText = string.Empty;
                    //lnkZBArtWork.Attributes.Add("target", "");
                    lblZBArtWork.Visible = true;
                    lnkZBArtWork.Visible = false;
                    lblZBDimension.Text = string.Empty;
                    lblZBColour.Text = string.Empty;
                    break;
                case ControlsEnum.MASTERCARTONDETAILS:
                    lblMCArtWork.Text = string.Empty;
                    lnkMCArtWork.InnerText = string.Empty;
                    //lnkMCArtWork.Attributes.Add("target", "");

                    lblMCArtWork.Visible = true;
                    lnkMCArtWork.Visible = false;
                    lblMCDimension.Text = string.Empty;
                    lblMCColour.Text = string.Empty;
                    break;
                case ControlsEnum.SACKBAGDETAILS:
                    lblSBArtWork.Text = string.Empty;
                    lnkSBArtWork.InnerText = string.Empty;
                    //lnkSBArtWork.Attributes.Add("target", "");
                    lblSBArtWork.Visible = true;
                    lnkSBArtWork.Visible = false;
                    lblSBDimension.Text = string.Empty;
                    lblSBColour.Text = string.Empty;
                    break;
                case ControlsEnum.POBDETAILS:
                    lblPOBArtWork.Text = string.Empty;
                    lnkPOBArtWork.InnerText = string.Empty;
                    lblPOBArtWork.Visible = true;
                    lnkPOBArtWork.Visible = false;
                    lblPOBDimension.Text = string.Empty;
                    lblPOBColour.Text = string.Empty;
                    break;
                case ControlsEnum.PRBDETAILS:
                    lblPRBArtWork.Text = string.Empty;
                    lnkPRBArtWork.InnerText = string.Empty;
                    lblPRBArtWork.Visible = true;
                    lnkPRBArtWork.Visible = false;
                    lblPRBDimension.Text = string.Empty;
                    lblPRBColour.Text = string.Empty;
                    break;
                case ControlsEnum.WLTDETAILS:
                    lblWLTArtWork.Text = string.Empty;
                    lnkWLTArtWork.InnerText = string.Empty;
                    lblWLTArtWork.Visible = true;
                    lnkWLTArtWork.Visible = false;
                    lblWLTDimension.Text = string.Empty;
                    lblWLTColour.Text = string.Empty;
                    break;
            }
        }

        private void setPackingSpecHdr()
        {
            if (PackingPK > 0)
            {
                PackingHeaderDiv.Visible = true;
                GetFieldValues(ControlsEnum.PACKINGSPECMASTERBYPK);
                if (dsPackingMaster != null && dsPackingMaster.Tables[0].Rows.Count > 0)
                {
                    lblPackingSpecCodeHdr.ToolTip = dsPackingMaster.Tables[0].Rows[0]["APS_CODE"].ToString();
                    lblPackingSpecCodeHdr.Text = CommonFunctions.GetShortString(lblPackingSpecCodeHdr.ToolTip, 50);
                    lblPackingSpecTotalHdr.Text = dsPackingMaster.Tables[0].Rows[0]["APS_TOTAL_PCS"].ToString();
                }
            }
            else
            {
                PackingHeaderDiv.Visible = false;
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
            //AdmPackingSpecMstService AdmPackingMstServiceClient;
            //AdmPackingMstServiceClient = null;
            string DropDownID = string.Empty;
            try
            {
                int? result;
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
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
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

                            packingSpecMstObj = ERP.Utilities.CommonFunctions.Initilize<Packingmapping>();
                            packingSpecMstObj = SetUIValuesToObject();
                            result = BusinessLogic.Inventory.PackingMasterBL.SavePackingMapping(packingSpecMstObj);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                SortBy = Resources.DataFieldRes.ItemCode;
                                SortDirection = Resources.Report.SortDescending;
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.PackingMaterialMapping);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                       + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PackingSpecMapping) + "');", true);

                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                //btnNew.Focus();
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
                                    litErrorMsg.Text = Resources.PageNameRes.PackingMaterialMapping + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PackingSpecMapping) + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PackingMaterialMapping + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PackingSpecMapping) + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.DATEOVERLAP)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.PackingMaterialMapping + " " + GetLocalResourceObject("Err_ArtworkUsed").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PackingSpecMapping) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PackingMaterialMapping);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }

                        }
                        break;
                    #endregion

                    #region New
                    case ActionsEnum.NEW:
                        chkStatus.Enabled = true;
                        ModifiedDatePnl.Visible = false;
                        GetFieldValues(ControlsEnum.CONTROLS);
                        SetFieldValues(ControlsEnum.CONTROLS);
                        EntryStatus = EntryStatus.NEWMODE;
                        lblCustomer.Focus();
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        btnSearch.Focus();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        int tempPacSpc = Convert.ToInt32(ViewState["TmpPackSpec"]);
                        if (tempPacSpc > 0)
                        {
                            PackingPK = tempPacSpc;
                            setPackingSpecHdr();
                        }
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetSearch();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:

                        if (Request.QueryString[QueryStrings.CustPk] != null && Request.QueryString[QueryStrings.BrankPk] != null)
                        {
                            txtCustomerID.Text = lblCustomer.Text.Trim();
                            hdfCustomerID.Value = Request.QueryString[QueryStrings.CustPk];
                            txtBrandID.Text = lblBrand.Text.Trim();
                            hdfBrandID.Value = Request.QueryString[QueryStrings.BrankPk];
                            RemoveKeyQueryString(QueryStrings.BrankPk);
                        }
                        ResetForm();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);

                        int tempPacSp = Convert.ToInt32(ViewState["TmpPackSpec"]);
                        if (tempPacSp > 0)
                        {
                            PackingPK = tempPacSp;
                            setPackingSpecHdr();
                        }
                        else
                        {
                            PackingHeaderDiv.Visible = false;
                        }
                        //this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Edit
                    case ActionsEnum.EDIT:
                        SetUIEditView(commonActions);
                        break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        //AdmPackingMstServiceClient = new AdmPackingSpecMstService();
                        //AdmPackingMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmPackingMstServiceClient);
                        //packingSpecMstList = new List<ADM_PACKING_SPEC_MST>();
                        //packingSpecMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_PACKING_SPEC_MST>();
                        //packingSpecMstObj.APS_PK = CurrPK;
                        //packingSpecMstObj.APS_MOD_DT = LastModifiedTime;
                        //packingSpecMstList.Add(packingSpecMstObj);

                        //result = AdmPackingMstServiceClient.DeletePackingSpecMst(packingSpecMstList);
                        //if (result > 0)
                        //{
                        //    litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                        //    ResetForm();
                        //    btnNew.Focus();
                        //    EntryStatus = EntryStatus.LISTMODE;
                        //    GetFieldValues(ControlsEnum.DEFAULT);
                        //    SetFieldValues(ControlsEnum.DEFAULT);
                        //    litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                        //    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Packing);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        //}
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(commonActions);
                        btnCancel.Focus();
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.ACTIVATE:
                        SetUIEditView(commonActions);
                        break;
                    #endregion

                    #region dropdownchange
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        #region OldCode Region
                        //DropDownID = ((DropDownList)sender).ID;
                        //if (DropDownID == "ddlPouchPack")
                        //{
                        //    GetFieldValues(ControlsEnum.POUCHPACKDETAILS);
                        //    SetFieldValues(ControlsEnum.POUCHPACKDETAILS);
                        //}
                        //else if (DropDownID == "ddlInnerBox")
                        //{
                        //    GetFieldValues(ControlsEnum.INNERBOXDETAILS);
                        //    SetFieldValues(ControlsEnum.INNERBOXDETAILS);
                        //}
                        //else if (DropDownID == "ddlMiniInnerCarton")
                        //{
                        //    GetFieldValues(ControlsEnum.MINIINNERCARTONDETAILS);
                        //    SetFieldValues(ControlsEnum.MINIINNERCARTONDETAILS);
                        //}
                        //else if (DropDownID == "ddlZipperBag")
                        //{
                        //    GetFieldValues(ControlsEnum.ZIPPERBAGDETAILS);
                        //    SetFieldValues(ControlsEnum.ZIPPERBAGDETAILS);
                        //}
                        //else if (DropDownID == "ddlMasterCarton")
                        //{
                        //    GetFieldValues(ControlsEnum.MASTERCARTONDETAILS);
                        //    SetFieldValues(ControlsEnum.MASTERCARTONDETAILS);
                        //}
                        //else if (DropDownID == "ddlSackBag")
                        //{
                        //    GetFieldValues(ControlsEnum.SACKBAGDETAILS);
                        //    SetFieldValues(ControlsEnum.SACKBAGDETAILS);
                        //}
                        //else if (DropDownID == "ddlPOB")
                        //{
                        //    //GetFieldValues(ControlsEnum.POB);
                        //    //SetFieldValues(ControlsEnum.POB);
                        //    GetFieldValues(ControlsEnum.POBDETAILS);
                        //    SetFieldValues(ControlsEnum.POBDETAILS);
                        //}
                        //else if (DropDownID == "ddlPolybag")
                        //{
                        //    GetFieldValues(ControlsEnum.PRBDETAILS);
                        //    SetFieldValues(ControlsEnum.PRBDETAILS);
                        //}
                        //else if (DropDownID == "ddlWallet")
                        //{
                        //    GetFieldValues(ControlsEnum.WLTDETAILS);
                        //    SetFieldValues(ControlsEnum.WLTDETAILS);
                        //} 
                        #endregion
                        if (hdfSelectedTextboxId.Value == "txtPouchPack")
                        {
                            GetFieldValues(ControlsEnum.POUCHPACKDETAILS);
                            SetFieldValues(ControlsEnum.POUCHPACKDETAILS);
                        }
                        else if (hdfSelectedTextboxId.Value == "txtInnerBox")
                        {
                            GetFieldValues(ControlsEnum.INNERBOXDETAILS);
                            SetFieldValues(ControlsEnum.INNERBOXDETAILS);
                        }
                        else if (hdfSelectedTextboxId.Value == "txtMiniInnerCarton")
                        {
                            GetFieldValues(ControlsEnum.MINIINNERCARTONDETAILS);
                            SetFieldValues(ControlsEnum.MINIINNERCARTONDETAILS);
                        }
                        else if (hdfSelectedTextboxId.Value == "txtZipperBag")
                        {
                            GetFieldValues(ControlsEnum.ZIPPERBAGDETAILS);
                            SetFieldValues(ControlsEnum.ZIPPERBAGDETAILS);
                        }
                        else if (hdfSelectedTextboxId.Value == "txtMasterCarton")
                        {
                            GetFieldValues(ControlsEnum.MASTERCARTONDETAILS);
                            SetFieldValues(ControlsEnum.MASTERCARTONDETAILS);
                        }
                        else if (hdfSelectedTextboxId.Value == "txtSackBag")
                        {
                            GetFieldValues(ControlsEnum.SACKBAGDETAILS);
                            SetFieldValues(ControlsEnum.SACKBAGDETAILS);
                        }
                        else if (hdfSelectedTextboxId.Value == "txtPOB")
                        {
                            //GetFieldValues(ControlsEnum.POB);
                            //SetFieldValues(ControlsEnum.POB);
                            GetFieldValues(ControlsEnum.POBDETAILS);
                            SetFieldValues(ControlsEnum.POBDETAILS);
                        }
                        else if (hdfSelectedTextboxId.Value == "txtPolybag")
                        {
                            GetFieldValues(ControlsEnum.PRBDETAILS);
                            SetFieldValues(ControlsEnum.PRBDETAILS);
                        }
                        else if (hdfSelectedTextboxId.Value == "txtWallet")
                        {
                            GetFieldValues(ControlsEnum.WLTDETAILS);
                            SetFieldValues(ControlsEnum.WLTDETAILS);
                        }
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            EntryStatus = EntryStatus.NEWMODE;
                        }
                        break;
                    #endregion

                    #region GRIDEDIT
                    case ActionsEnum.GRIDEDIT:
                        CurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        GetFieldValues(ControlsEnum.PACKING);
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(dtPageData.Rows[0][Resources.DataFieldRes.PIMPK].ToString());
                            PackingPK = Convert.ToInt32(dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterPK].ToString());
                            hdfCustomer.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterCustomer].ToString();
                        }
                        GetFieldValues(ControlsEnum.CONTROLS);
                        SetFieldValues(ControlsEnum.CONTROLS);
                        SetFieldValues(ControlsEnum.PACKING);
                        EntryStatus = EntryStatus.ENTRYMODE;
                        ModifiedDatePnl.Visible = true;
                        break;
                    #endregion

                    #region GRIDDELETE
                    case ActionsEnum.GRIDDELETE:
                        CurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        result = BusinessLogic.Inventory.PackingMasterBL.DeletePackingMapping(CurrPK, DateTime.Now);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            ResetForm();
                            btnSave.Focus();
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.PackingMaterialMapping);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
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
                                litErrorMsg.Text = Resources.PageNameRes.PackingMaterialMapping + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PackingSpecMapping) + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.PackingMaterialMapping + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PackingSpecMapping) + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PackingMaterialMapping);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region PACKINGLIST
                    case ActionsEnum.PACKINGLIST:
                        Session[ERP.Utilities.SessionStrings.PackingMode] = 1;
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PackingMaster));
                        break;
                    #endregion
                    #region PACKINGDETAILS
                    case ActionsEnum.PACKINGDETAILS:
                        Session[ERP.Utilities.SessionStrings.PackingMode] = 2;
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PackingMaster));
                        break;
                    #endregion
                    #region BRANDDETAILS
                    case ActionsEnum.BRANDDETAILS:
                        GetFieldValues(ControlsEnum.BRANDDETAILS);
                        SetFieldValues(ControlsEnum.BRANDDETAILS);
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            EntryStatus = EntryStatus.NEWMODE;
                        }
                        break;
                    #endregion

                    #region CustomerIndexChange
                    case ActionsEnum.CUSTOMERCHANGE:
                        GetFieldValues(ControlsEnum.POUCHPACK);
                        SetFieldValues(ControlsEnum.POUCHPACK);
                        GetFieldValues(ControlsEnum.INNERBOX);
                        SetFieldValues(ControlsEnum.INNERBOX);
                        GetFieldValues(ControlsEnum.MINIINNERCARTON);
                        SetFieldValues(ControlsEnum.MINIINNERCARTON);
                        GetFieldValues(ControlsEnum.ZIPPERBAG);
                        SetFieldValues(ControlsEnum.ZIPPERBAG);
                        GetFieldValues(ControlsEnum.MASTERCARTON);
                        SetFieldValues(ControlsEnum.MASTERCARTON);
                        GetFieldValues(ControlsEnum.SACKBAG);
                        SetFieldValues(ControlsEnum.SACKBAG);
                        break;
                    #endregion

                    #region NEWVERSION
                    case ActionsEnum.NEWVERSION:
                        CurrPK = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        GetFieldValues(ControlsEnum.PACKING);
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(dtPageData.Rows[0][Resources.DataFieldRes.PIMPK].ToString());
                            PackingPK = Convert.ToInt32(dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterPK].ToString());
                            hdfCustomer.Value = dtPageData.Rows[0][Resources.DataFieldRes.PackingMasterCustomer].ToString();
                        }
                        GetFieldValues(ControlsEnum.CONTROLS);
                        SetFieldValues(ControlsEnum.CONTROLS);
                        SetFieldValues(ControlsEnum.PACKING);
                        EntryStatus = EntryStatus.ENTRYMODE;
                        CurrPK = 0;
                        ViewState[ViewstateStrings.LastModifiedTime] = null;
                        hdfMode.Value = "NEW";
                        hdfOldVersion.Value = txtArtworkVersion.Text.Trim();
                        ModifiedDatePnl.Visible = true;
                        //ddlPouchPack.Enabled = true;
                        //ddlInnerBox.Enabled = true;
                        //ddlMiniInnerCarton.Enabled = true;
                        //ddlZipperBag.Enabled = true;
                        //ddlMasterCarton.Enabled = true;
                        //ddlSackBag.Enabled = true;
                        //ddlPOB.Enabled = true;
                        //ddlPolybag.Enabled = true;
                        //ddlWallet.Enabled = true;
                        txtPouchPack.Text = string.Empty; hdfAutoPouchPack.Value = "0";//ddlPouchPack.SelectedValue = CommonConstants.SELECTVAL;
                        txtInnerBox.Text = string.Empty; hdfAutoInnerBox.Value = "0";//ddlInnerBox.SelectedValue = CommonConstants.SELECTVAL;
                        txtMiniInnerCarton.Text = string.Empty; hdfAutoMiniInnerCarton.Value = "0";//ddlMiniInnerCarton.SelectedValue = CommonConstants.SELECTVAL;
                        txtZipperBag.Text = string.Empty; hdfAutoZipperBag.Value = "0";//ddlZipperBag.SelectedValue = CommonConstants.SELECTVAL;
                        txtMasterCarton.Text = string.Empty; hdfAutoMasterCarton.Value = "0";//ddlMasterCarton.SelectedValue = CommonConstants.SELECTVAL;
                        txtSackBag.Text = string.Empty; hdfAutoSackBag.Value = "0";//ddlSackBag.SelectedValue = CommonConstants.SELECTVAL;
                        txtPOB.Text = string.Empty; hdfAutoPOB.Value = "0";// ddlPOB.SelectedValue = CommonConstants.SELECTVAL;
                        txtPolybag.Text = string.Empty; hdfAutoPolybag.Value = "0";//ddlPolybag.SelectedValue = CommonConstants.SELECTVAL;
                        txtWallet.Text = string.Empty; hdfAutoWallet.Value = "0";//ddlWallet.SelectedValue = CommonConstants.SELECTVAL;

                        ClearControl(ControlsEnum.POUCHPACKDETAILS);
                        ClearControl(ControlsEnum.INNERBOXDETAILS);
                        ClearControl(ControlsEnum.MINIINNERCARTONDETAILS);
                        ClearControl(ControlsEnum.ZIPPERBAGDETAILS);
                        ClearControl(ControlsEnum.MASTERCARTONDETAILS);
                        ClearControl(ControlsEnum.SACKBAGDETAILS);
                        ClearControl(ControlsEnum.POBDETAILS);
                        ClearControl(ControlsEnum.PRBDETAILS);
                        ClearControl(ControlsEnum.WLTDETAILS);
                        btnSave.Visible = true;
                        break;
                    #endregion

                    #region GRIDACTIVATE
                    case ActionsEnum.GRIDACTIVATE:
                        PackingSpecPk = 0;
                        PackingSpecPk = Convert.ToInt32(((ImageButton)sender).CommandArgument);
                        if (hdfActivate.Value == "0")
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowArtworkActivateConfirm();", true);
                            return;
                        }
                        break;
                    #endregion
                    #region ACTIVATE ARTWORK
                    case ActionsEnum.ACTIVATEARTWORK:
                        if (PackingSpecPk > 0)
                        {
                            result = BusinessLogic.Inventory.PackingMasterBL.ActivateArtwork(PackingSpecPk, currentUser.PKUser);
                            hdfActivate.Value = "0";
                            PackingSpecPk = 0;
                            btnSearch.Focus();
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            int tmpPackingSpc = Convert.ToInt32(ViewState["TmpPackSpec"]);
                            if (tmpPackingSpc > 0)
                            {
                                PackingPK = tmpPackingSpc;
                                setPackingSpecHdr();
                            }
                            EntryStatus = EntryStatus.LISTMODE;
                        }

                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("PackingSpecDuplicate").ToString()))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.PackingSpecs)) + "','" + Resources.Messages.Information + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                packingSpecMstObj = null;
                // packingSpecMstList = null;
                //AdmPackingMstServiceClient = null;
            }
        }

        private void RemoveKeyQueryString(string key)
        {
            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove(key);
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            try
            {
                PageIndex = e.NewPageIndex.ToString();
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
                PageIndex = CommonConstants.SELECT_VALUE_ZERO;
                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdPackingMst")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        //e.Row.DataItem
                        HiddenField hdfStatus = e.Row.FindControl("hdfStatus") as HiddenField;
                        if (hdfStatus != null && !string.IsNullOrEmpty(hdfStatus.Value))
                        {
                            ImageButton imgNew = e.Row.FindControl("imgNew") as ImageButton;
                            if (!hdfStatus.Value.ToString().Equals("1"))
                            {
                                imgNew.Visible = false;
                            }
                        }
                    }
                }
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
            //uclPaging.CurrentPage = 1;
            //this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            //this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            //this.btnView.PreRender += new EventHandler(btnAction_PreRender);
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
            //this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            //try
            //{
            //    switch (e.Action)
            //    {
            //        case NavigationEnum.PAGECHANGE:
            //            //uclPaging.CurrentPage = e.CurrentPage;
            //            break;
            //        case NavigationEnum.FIRST:
            //            // Decrement the first page index.
            //            if (e.CurrentPage > 1)
            //            //    uclPaging.CurrentPage = 1;
            //            break;
            //        case NavigationEnum.LAST:
            //            // Increment the last page index.
            //            if (e.CurrentPage <= e.TotalPages)
            //            //    uclPaging.CurrentPage = e.TotalPages;
            //            break;
            //        case NavigationEnum.NEXT:
            //            // Increment the next page index.
            //            if (e.CurrentPage <= e.TotalPages)
            //            //    uclPaging.CurrentPage++;
            //            break;
            //        case NavigationEnum.PREVIOUS:
            //            // Decrement the previous page index.
            //            if (e.CurrentPage > 1)
            //            //    uclPaging.CurrentPage--;
            //            break;
            //    }
            //    //PageIndex = uclPaging.CurrentPage.ToString();
            //    GetFieldValues(ControlsEnum.DEFAULT);
            //    SetFieldValues(ControlsEnum.DEFAULT);
            //    EnableDisableButtons(e.TotalPages);
            //    EntryStatus = EntryStatus.LISTMODE;
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            //}
        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            //// Should we disable the first link?
            //uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we disable the previous link?
            //uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we enable the next link?
            //uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            //// Should we enable the last link?
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
            string breadCrumb;
            breadCrumb = this.GetLocalResourceObject("BreadcrumbPackingCreation").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);


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
                breadCrumb = this.GetLocalResourceObject("BreadcrumbPackingList").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            }
            lblBreadCrum.Text = breadCrumb;
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
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

                //if (PackingPK == 0)
                //{
                //   // Response.Redirect(Resources.PageURL.PackingMaster);
                //}
                if (!IsPostBack)
                {
                    //Session[ERP.Utilities.SessionStrings.PackingPk] = 16;//Test
                    PackingPK = Session[ERP.Utilities.SessionStrings.PackingPk] != null ? (int)Session[ERP.Utilities.SessionStrings.PackingPk] : 0;
                    ViewState["TmpPackSpec"] = Session[ERP.Utilities.SessionStrings.PackingPk] != null ? (int)Session[ERP.Utilities.SessionStrings.PackingPk] : 0;
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.PIMPK;
                    grdPackingMst.DataKeyNames = datakeyarray;
                    //GetFieldValues(ControlsEnum.DEFAULT);
                    //SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;

                    setPackingSpecHdr();
                    //Request From Customer Registration Page (Brand => Packing Spec Edit)
                    if (Request.QueryString[QueryStrings.CustPk] != null && Request.QueryString[QueryStrings.BrankPk] != null)
                    {
                        hdfCustomerID.Value = Request.QueryString[QueryStrings.CustPk].Trim();
                        hdfBrandID.Value = Request.QueryString[QueryStrings.BrankPk].Trim();
                        GetFieldValues(ControlsEnum.DEFAULT);

                        if (dtPageData != null)
                        {
                            List<int> packingSpecMappingID = dtPageData.AsEnumerable().Where(s => s.Field<byte>("PIM_ACTIVE") == 1).Select(p => p.Field<int>(Resources.DataFieldRes.PIMPK)).ToList();
                            if (packingSpecMappingID != null && packingSpecMappingID.Count > 0)
                            {
                                ImageButton imbDummy = new ImageButton();
                                imbDummy.CommandName = ActionsEnum.GRIDEDIT.ToString();
                                imbDummy.CommandArgument = packingSpecMappingID[0].ToString();
                                ActionHandler(imbDummy, EventArgs.Empty);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("PackingSpecNotFound").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }

                    }


                    //////this.btnNew.Focus();
                }
                BindDynamicTable(DynamicControls);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            PACKING,
            IPPACKING,
            OPPACKING,
            POUCHPACKING,
            PACKINGTYPE,
            CONTROLS,
            POUCHPACK,
            INNERBOX,
            MINIINNERCARTON,
            ZIPPERBAG,
            MASTERCARTON,
            SACKBAG,
            POUCHPACKDETAILS,
            INNERBOXDETAILS,
            MINIINNERCARTONDETAILS,
            ZIPPERBAGDETAILS,
            MASTERCARTONDETAILS,
            SACKBAGDETAILS,
            BRANDDETAILS,
            PACKINGSPECMASTERBYPK,
            POB,
            POBDETAILS,
            PRB,
            PRBDETAILS,
            WLT,
            WLTDETAILS
        }
        #endregion

        private int ToInteger(string strValue)
        {
            int intValue = 0;
            int.TryParse(strValue, out intValue);
            return intValue;
        }

        private void BindDynamicTable(DataTable dtControls)
        {
            short tabIndex = 33;
            tblDynamicMaterials.Rows.Clear();
            if (dtControls != null)
            {
                DataTable dtSelectedMaterials = BusinessLogic.Inventory.PackingMasterBL.GetPackingMappingListDynamic(0, 1, CurrPK, currentUser.SBUID);
                DynamicControls = dtControls;
                TableHeaderRow headerRow = new TableHeaderRow();
                headerRow.CssClass = "grdhead";
                TableHeaderCell headerCell1 = new TableHeaderCell();
                headerCell1.Width = new Unit("17%");
                headerCell1.Text = Resources.Controls.PackingMaterial;
                headerRow.Cells.Add(headerCell1);
                TableHeaderCell headerCell2 = new TableHeaderCell();
                headerCell2.Width = new Unit("41%");
                headerCell2.Text = Resources.Controls.MaterialCodePacking;
                headerRow.Cells.Add(headerCell2);
                TableHeaderCell headerCell3 = new TableHeaderCell();
                headerCell3.Width = new Unit("10%");
                headerCell3.Text = Resources.Controls.Quantity;
                headerRow.Cells.Add(headerCell3);
                TableHeaderCell headerCell4 = new TableHeaderCell();
                headerCell4.Width = new Unit("10%");
                headerCell4.Text = Resources.Controls.Artwork;
                headerRow.Cells.Add(headerCell4);
                TableHeaderCell headerCell5 = new TableHeaderCell();
                headerCell5.Width = new Unit("14%");
                headerCell5.Text = Resources.Controls.Dimension;
                headerRow.Cells.Add(headerCell5);
                TableHeaderCell headerCell6 = new TableHeaderCell();
                headerCell6.Width = new Unit("18%");
                headerCell6.Text = Resources.Controls.Color;
                headerRow.Cells.Add(headerCell6);
                tblDynamicMaterials.Rows.Add(headerRow);
                for (int i = 9; i < dtControls.Rows.Count; i++)
                {
                    TableRow tableRow = new TableRow();
                    //string conData = dtControls.Rows[i].Field<string>("CON_DATA");
                    string conData = i.ToString();
                    tableRow.Attributes.Add("conData", conData);
                    //td1
                    TableCell cell1 = new TableCell();
                    cell1.Width = new Unit("17%");
                    HiddenField hdfCol1 = new HiddenField();
                    hdfCol1.ID = "hdf" + conData;
                    int hdfCol1Value = dtControls.Rows[i].Field<int>(Resources.DataFieldRes.ConstPK);
                    hdfCol1.Value = (hdfCol1Value).ToString();
                    Label lblCol1 = new Label();
                    lblCol1.ID = "lbl" + conData;
                    lblCol1.Text = dtControls.Rows[i].Field<string>(Resources.DataFieldRes.ConstName);
                    cell1.Controls.Add(hdfCol1);
                    cell1.Controls.Add(lblCol1);
                    tableRow.Cells.Add(cell1);
                    //td2
                    TableCell cell2 = new TableCell();
                    cell2.Width = new Unit("41%");
                    HiddenField hdfBmdPk = new HiddenField();
                    hdfBmdPk.ID = "hdf" + conData + "BmdPk";
                    hdfBmdPk.Value = "0";
                    DropDownList ddlCol2 = new DropDownList();
                    ddlCol2.ID = "ddl" + conData;
                    ddlCol2.TabIndex = tabIndex;
                    ++tabIndex;
                    ddlCol2.Width = new Unit("90%");
                    ddlCol2.Attributes.Add("onmouseover", "javascript:ShowTooltip('" + ddlCol2.ID + "');");
                    DataTable dtItems;
                    int customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                    dtItems = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfCol1.Value), customerPK, 1, currentUser.SBUID);
                    ddlCol2.Items.Clear();
                    if (dtItems != null && dtItems.Rows.Count > 0)
                    {
                        ddlCol2.DataSource = dtItems;
                        ddlCol2.DataTextField = Resources.DataFieldRes.ItemName; // IPDITEMText;
                        ddlCol2.DataValueField = Resources.DataFieldRes.IPDITEM;
                        ddlCol2.DataBind();
                    }
                    ddlCol2.Items.Insert(0, new ListItem(Resources.Captions.SelectText, CommonConstants.SELECTVAL));

                    // Set Selected Index from  Db  
                    HiddenField hdfCodeCol2 = new HiddenField();
                    hdfCodeCol2.ID = "hdf" + conData + "Code";
                    var material = dtSelectedMaterials.AsEnumerable()
                        .Where(l => l.Field<int>("BMD_TYPE") == hdfCol1Value)
                        .SingleOrDefault();
                    ddlCol2.AutoPostBack = true;
                    ddlCol2.SelectedIndexChanged += new EventHandler(ddlCol2_SelectedIndexChanged);
                    DataTable dtItemDetailsDynamic = new DataTable();
                    if (material != null)
                    {
                        hdfBmdPk.Value = Convert.ToString(material.Field<int>("BMD_PK"));
                        ddlCol2.SelectedValue = Convert.ToString(material.Field<int>("BMD_ITEM"));
                        DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, material.Field<int>("BMD_ITEM"), 0, 0, 1, currentUser.SBUID);
                        hdfCodeCol2.Value = (dt != null && dt.Rows.Count > 0) ? dt.Rows[0]["ITM_CODE"].ToString() : "";
                        //Bind Next Columns
                        dtItemDetailsDynamic = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdfCol1.Value), customerPK, 1, currentUser.SBUID);
                    }

                    cell2.Controls.Add(hdfBmdPk);
                    cell2.Controls.Add(ddlCol2);
                    cell2.Controls.Add(hdfCodeCol2);
                    ImageButton imbCol2 = new ImageButton();
                    imbCol2.ID = "imb" + conData + "Code";
                    imbCol2.SkinID = "btnview";
                    imbCol2.Attributes.Add("onclick", "return ShowMaterialWithCode('" + hdfCodeCol2.ID + "')");
                    imbCol2.ToolTip = (GetLocalResourceObject("ViewMaterial")).ToString();
                    cell2.Controls.Add(imbCol2);
                    tableRow.Cells.Add(cell2);
                    //td3
                    TableCell cell3 = new TableCell();
                    cell3.Width = new Unit("10%");
                    TextBox txtCol3 = new TextBox();
                    txtCol3.ID = "txt" + conData + "Quantity";
                    txtCol3.CssClass = "small numeric";
                    if (material != null)
                    {
                        txtCol3.Text = material["BMD_QTY"].ToString();
                    }
                    cell3.Controls.Add(txtCol3);
                    tableRow.Cells.Add(cell3);
                    //td4     
                    TableCell cell4 = new TableCell();
                    cell4.Width = new Unit("10%");
                    Label lblArtWorkCol4 = new Label();
                    lblArtWorkCol4.ID = "lbl" + conData + "ArtWork";
                    cell4.Controls.Add(lblArtWorkCol4);
                    HyperLink hplArtWorkCol4 = new HyperLink();
                    hplArtWorkCol4.ID = "lnk" + conData + "ArtWork";
                    //hplArtWorkCol4.title
                    cell4.Controls.Add(hplArtWorkCol4);
                    cell4.CssClass = "bggrey";
                    tableRow.Cells.Add(cell4);
                    //td5
                    TableCell cell5 = new TableCell();
                    cell5.Width = new Unit("14%");
                    Label lblDimensionCol5 = new Label();
                    lblDimensionCol5.ID = "lbl" + conData + "Dimension";
                    cell5.Controls.Add(lblDimensionCol5);
                    cell5.CssClass = "bggrey";
                    tableRow.Cells.Add(cell5);
                    //td6
                    TableCell cell6 = new TableCell();
                    cell6.Width = new Unit("18%");
                    Label lblColourCol6 = new Label();
                    lblColourCol6.ID = "lbl" + conData + "Colour";
                    cell6.Controls.Add(lblColourCol6);
                    cell6.CssClass = "bggrey";
                    tableRow.Cells.Add(cell6);

                    // Bind Next columns according to Dropdown Selection
                    if (dtItemDetailsDynamic.Rows.Count > 0)
                    {
                        lblArtWorkCol4.Text = dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                        lblArtWorkCol4.Visible = true;
                        hplArtWorkCol4.Visible = false;
                        if (File.Exists(Server.MapPath(Resources.PageURL.ArtworkUploads + dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PackingDocument].ToString())))
                        {
                            hplArtWorkCol4.Text = dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                            hplArtWorkCol4.NavigateUrl = Page.ResolveClientUrl(Resources.PageURL.ArtworkUploads + dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PackingDocument].ToString());
                            hplArtWorkCol4.Attributes.Add("target", "_blank");
                            hplArtWorkCol4.Visible = false;
                            hplArtWorkCol4.Visible = true;
                        }
                        else
                        {
                            //lnkIBArtWork.HRef = "javascript:FileNotFound();";
                            //lnkPPArtWork.Attributes.Add("target", "");
                        }
                        lblColourCol6.Text = dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PaperColour] != null ? dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PaperColour].ToString() : string.Empty;
                        lblColourCol6.ToolTip = lblColourCol6.Text;
                        lblDimensionCol5.Text = dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PackingDimension] != null ? dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PackingDimension].ToString() : string.Empty;
                        lblDimensionCol5.ToolTip = lblDimensionCol5.Text;
                    }

                    tblDynamicMaterials.Rows.Add(tableRow);

                }
            }


        }

        void ddlCol2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlSender = (DropDownList)sender;
            string ddlId = ddlSender.ID;
            string con_data = ddlId.Replace("ddl", "");
            string hdfId = ddlId.Replace("ddl", "hdf");
            HiddenField hdf = (HiddenField)FindControlRecursive(tblDynamicMaterials, hdfId);
            string hdfCodeId = hdfId + "Code";
            HiddenField hdfCode = (HiddenField)FindControlRecursive(tblDynamicMaterials, hdfCodeId);
            Label lblArtWork = (Label)FindControlRecursive(tblDynamicMaterials, "lbl" + con_data + "ArtWork");
            HyperLink lnkArtWork = (HyperLink)FindControlRecursive(tblDynamicMaterials, "lnk" + con_data + "ArtWork");
            Label lblColour = (Label)FindControlRecursive(tblDynamicMaterials, "lbl" + con_data + "Colour");
            Label lblDimension = (Label)FindControlRecursive(tblDynamicMaterials, "lbl" + con_data + "Dimension");



            if (ddlSender.SelectedIndex > 0)
            {
                int itemPk = Convert.ToInt32(ddlSender.SelectedValue);
                DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, itemPk, 0, 0, 1, currentUser.SBUID);
                hdfCode.Value = (dt != null && dt.Rows.Count > 0) ? dt.Rows[0]["ITM_CODE"].ToString() : "";
                int customerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value);
                DataTable dtItemDetailsDynamic = BusinessLogic.CommonManagement.CommonBL.GetItemPackDetails(0, 0, Convert.ToInt32(hdf.Value), customerPK, 1, currentUser.SBUID);
                lblArtWork.Text = dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                lblArtWork.Visible = true;
                lnkArtWork.Visible = false;
                if (File.Exists(Server.MapPath(Resources.PageURL.ArtworkUploads + dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PackingDocument].ToString())))
                {
                    lnkArtWork.Text = dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.Artwork] != null ? dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.Artwork].ToString() : string.Empty;
                    lnkArtWork.NavigateUrl = Page.ResolveClientUrl(Resources.PageURL.ArtworkUploads + dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PackingDocument].ToString());
                    lnkArtWork.Attributes.Add("target", "_blank");
                    lblArtWork.Visible = false;
                    lnkArtWork.Visible = true;
                }
                else
                {
                    //lnkIBArtWork.HRef = "javascript:FileNotFound();";
                    //lnkPPArtWork.Attributes.Add("target", "");
                }
                lblColour.Text = dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PaperColour] != null ? dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PaperColour].ToString() : string.Empty;
                lblColour.ToolTip = lblColour.Text;
                lblDimension.Text = dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PackingDimension] != null ? dtItemDetailsDynamic.Rows[0][Resources.DataFieldRes.PackingDimension].ToString() : string.Empty;
                lblDimension.ToolTip = lblDimension.Text;
            }
            else
            {
                hdfCode.Value = lblArtWork.Text = string.Empty;
                lblArtWork.Visible = lnkArtWork.Visible = false;
                lblColour.Text = lblColour.ToolTip = lblDimension.Text = lblDimension.ToolTip = string.Empty;
            }
        }

        private string GetDinamicItemDetails()
        {
            string xml = string.Empty;
            if (tblDynamicMaterials.Rows.Count > 1)
            {
                List<Detail> Details = new List<Detail>();
                for (int i = 1; i < tblDynamicMaterials.Rows.Count; i++)
                {
                    Detail dtl = new Detail();
                    TableRow tr = tblDynamicMaterials.Rows[i];
                    string conData = tr.Attributes["conData"];

                    HiddenField hdfType = (HiddenField)FindControlRecursive(tblDynamicMaterials, "hdf" + conData);
                    int bmdType = Convert.ToInt32(hdfType.Value);
                    DropDownList ddlItem = (DropDownList)FindControlRecursive(tblDynamicMaterials, "ddl" + conData);
                    int bmdItem = Convert.ToInt32(ddlItem.SelectedValue);
                    if (bmdItem < 1)
                    {
                        continue;
                    }
                    HiddenField hdfBmdPk = (HiddenField)FindControlRecursive(tblDynamicMaterials, "hdf" + conData + "BmdPk");
                    int bmdPk = 0;
                    int.TryParse(hdfBmdPk.Value, out bmdPk);
                    TextBox txtQuantity = (TextBox)FindControlRecursive(tblDynamicMaterials, "txt" + conData + "Quantity");
                    decimal bmdQuantity = 0;
                    decimal.TryParse(txtQuantity.Text.Trim(), out bmdQuantity);
                    int bmdSequence = i - 1;
                    int bmdActive = 1;
                    Details.Add(new Detail
                    {
                        BMD_PK = bmdPk,
                        BMD_TYPE = bmdType,
                        BMD_ITEM = bmdItem,
                        BMD_QTY = bmdQuantity,
                        BMD_SEQUENCE = bmdSequence,
                        BMD_ACTIVE = bmdActive
                    });

                }
                var t = GTIService.CommonFunctions.SerializeObject(Details);

                t = t.Replace("ArrayOfDetail", "Root");
                int indx1 = t.IndexOf("<Root");
                t = t.Remove(0, indx1);
                t = t.Replace("<Root >", "<Root>");
                xml = t;
            }

            return xml;
        }

        /// <summary>
        /// recursively finds a child control of the specified parent.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private Control FindControlRecursive(Control control, string id)
        {
            if (control == null) return null;
            //try to find the control at the current level
            Control ctrl = control.FindControl(id);

            if (ctrl == null)
            {
                //search the children
                foreach (Control child in control.Controls)
                {
                    ctrl = FindControlRecursive(child, id);

                    if (ctrl != null) break;
                }
            }
            return ctrl;
        }

    }
}
