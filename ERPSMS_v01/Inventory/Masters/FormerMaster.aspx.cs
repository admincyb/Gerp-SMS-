#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPService;
using BusinessObject.Common;
using ERPSMS_v01.UserControls;
using BusinessObject.CommonManagement;
using System.Data;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using BusinessObject.CommonManagement;
using System.Xml.Serialization;
using BusinessObject.MaterialManagement;
#endregion

namespace ERPSMS_v01.Inventory.Masters
{
    public partial class FormerMaster : System.Web.UI.Page
    {
        #region Variables and Properties
        #region Properties

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

        private int CurrVenSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrVenSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrVenSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrVenSlNo] = value;
            }
        }
        /// <summary>
        /// To maintain the ProdDetails edit index
        /// </summary>
        private int ProductIndex
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ProductIndex"]);
            }
            set
            {
                this.ViewState["ProductIndex"] = value;
            }
        }

        //Data table for binding the Product DropDown
        private DataTable dtProduct
        {
            get
            {
                return (DataTable)this.ViewState["dtProduct"];
            }
            set
            {
                this.ViewState["dtProduct"] = value;
            }
        }

        private StoreMappingHeader StoreMappingHeaderViewState
        {
            get
            {
                if (this.ViewState["StoreMappingHeader"] == null)
                {
                    this.ViewState["StoreMappingHeader"] = new StoreMappingHeader();
                }
                return (StoreMappingHeader)this.ViewState["StoreMappingHeader"];
            }
            set
            {
                this.ViewState["StoreMappingHeader"] = value;
            }
        }

        private List<VenMappingDetails> VenMaterialList
        {
            get
            {
                return ViewState[ViewstateStrings.VenMaterialList] == null ? null : (List<VenMappingDetails>)ViewState[ViewstateStrings.VenMaterialList];
            }
            set
            {
                ViewState[ViewstateStrings.VenMaterialList] = value;
            }
        }

        #endregion

        #region Variables
        BusinessObject.User currentUser;
        private ActionsEnum commonActions;
        RadioButton rbtn;
        DataTable dtPageData;
        DataTable dtCategory;
        DataTable dtProductProperties;
        DataTable dtMaterials;
        // DataTable dtProduct;
        DataTable dtProductDetails;
        DataTable dtStoreMapping;

        private ERPData.INV_UOM_MST invUomMstObj;
        private List<ERPData.INV_UOM_MST> invUomMstList;

        private List<ERPData.INV_ITEM_GROUP_MST> invItemGroupMstList;

        /// <summary>
        /// To maintain the Product, Line Speed details (for Product Grid)
        /// </summary>
        private List<ProductDetails> ProdDetails;
        private VenMappingDetails objVenMaterial;
        private VenMappingHeader objVenHeader;
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
                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.CATEGORY);
                    SetFieldValues(ControlsEnum.CATEGORY);
                    GetFieldValues(ControlsEnum.SIZE);
                    SetFieldValues(ControlsEnum.SIZE);
                    GetFieldValues(ControlsEnum.UOM);
                    SetFieldValues(ControlsEnum.UOM);
                    //  GetFieldValues(ControlsEnum.VENDORUOM);
                    SetFieldValues(ControlsEnum.VENDORUOM);
                    GetFieldValues(ControlsEnum.PRODUCTGROUP);
                    SetFieldValues(ControlsEnum.PRODUCTGROUP);
                    GetFieldValues(ControlsEnum.PRODUCTDETAIL);
                    SetFieldValues(ControlsEnum.PRODUCTDETAIL);
                    BindProductProperties();
                    GetFieldValues(ControlsEnum.PRODUCT);
                    SetFieldValues(ControlsEnum.PRODUCT);
                    GetFieldValues(ControlsEnum.STOREMAPPING);
                    SetFieldValues(ControlsEnum.STOREMAPPING);
                    GetFieldValues(ControlsEnum.VENDORDETAILS);
                    SetFieldValues(ControlsEnum.VENDORDETAILS);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    ActivateList();
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    //  BindGrid(ControlsEnum.VENDORLISTING);
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
            ERPManager.ServiceUtility serviceUtilityObj;
            ERPService.Inventory.InvItemMstService InvItemMstServiceClient;
            ERPData.INV_ITEM_GROUP_MST invItemGroupMstObj;

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            BusinessObject.GridPrams gridParams;
            try
            {
                switch (type)
                {
                    #region Listing
                    case ControlsEnum.LIST:
                        gridParams = new BusinessObject.GridPrams();
                        gridParams.SearchBy = SearchType.SelectedValue;
                        gridParams.SearchValue = SearchValue.Text;
                        gridParams.PageNumber = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        gridParams.PageSize = grdList.PageSize;
                        gridParams.SortBy = "ITM_NAME";
                        gridParams.SortDirection = "Asc";
                        DataSet dsItems = new DataSet();
                        int TotalRecords = 0;
                        dsItems = BusinessLogic.MaterialManagement.MaterialMaster.GetMaterialTypes(gridParams, 6, currentUser.SBUID);
                        dtMaterials = dsItems.Tables[1];
                        DataTable dtTotalRecords = new DataTable();
                        dtTotalRecords = dsItems.Tables[0];
                        TotalRecords = dtTotalRecords.Rows[0].Field<Int32>(0);
                        TotalPages = (TotalRecords == 0) ? 1 : (TotalRecords <= grdList.PageSize) ? 1 : (TotalRecords % grdList.PageSize) == 0 ? (TotalRecords / grdList.PageSize) : (TotalRecords / grdList.PageSize) + 1;
                        break;
                    #endregion
                    #region CATEGORY
                    case ControlsEnum.CATEGORY:
                        //get the Categories
                        dtCategory = BusinessLogic.Inventory.ProductsBL.GetItemCategory(string.Empty, Convert.ToInt32(DbActiveStatus.ACTIVE), Convert.ToInt32(ItemCategory.Former));
                        break;
                    #endregion
                    #region SIZE
                    case ControlsEnum.SIZE:
                        //get the Product Size
                        dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, Convert.ToInt32(hdfType.Value), Convert.ToInt32(Convert.ChangeType(BusinessObject.CommonManagement.ProductProperties.Size, BusinessObject.CommonManagement.ProductProperties.Size.GetTypeCode())));
                        break;
                    #endregion
                    #region UOM
                    case ControlsEnum.UOM:
                        serviceUtilityObj = new ERPManager.ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        invUomMstObj = ERP.Utilities.CommonFunctions.Initilize<ERPData.INV_UOM_MST>();
                        invUomMstObj.UOM_PK = 0;
                        invUomMstObj.UOM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        InvItemMstServiceClient = new ERPService.Inventory.InvItemMstService();
                        invUomMstList = InvItemMstServiceClient.GetUomMst(invUomMstObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region PRODUCTGROUP
                    case ControlsEnum.PRODUCTGROUP:
                        invItemGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<ERPData.INV_ITEM_GROUP_MST>();
                        invItemGroupMstObj.IGM_PK = 0;
                        invItemGroupMstObj.IGM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        invItemGroupMstObj.IGM_GROUP_TYPE = 3;
                        InvItemMstServiceClient = new ERPService.Inventory.InvItemMstService();
                        invItemGroupMstList = InvItemMstServiceClient.GetProductGroups(invItemGroupMstObj);
                        break;
                    #endregion
                    #region EDIT
                    case ControlsEnum.EDIT:
                        dtMaterials = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialDetails(Convert.ToInt32(hdfMaterialPk.Value), currentUser.SBUID);
                        dtProductDetails = BusinessLogic.Inventory.FormerMasterBL.GetFormerMappedProducts(Convert.ToInt32(hdfMaterialPk.Value), currentUser.SBUID);
                        ProdDetails = dtProductDetails.ToList<ProductDetails>();
                        hdfFormerItemMapping.Value = JsonConvert.SerializeObject(ProdDetails);
                        dtStoreMapping = BusinessLogic.Inventory.FormerMasterBL.GetMaterialStores(Convert.ToInt32(hdfMaterialPk.Value), currentUser.SBUID);
                        StoreMappingHeaderViewState.StoreList = dtStoreMapping.ToList<StoreMapping>();
                        objVenHeader = BusinessLogic.Inventory.FormerMasterBL.GetVendorMappingDetails(Convert.ToInt32(hdfMaterialPk.Value));
                        VenMaterialList = objVenHeader.VenMapList;
                        break;
                    #endregion
                    #region PRODUCTDETAIL
                    case ControlsEnum.PRODUCTDETAIL:
                        dtCategory = BusinessLogic.Inventory.ProductsBL.GetCategory(currentUser.SBUID, 0);
                        break;
                    #endregion
                    #region PRODUCT
                    case ControlsEnum.PRODUCT:
                        int CategoryPK = Convert.ToInt32(ddlCategory.SelectedValue);
                        int typeNature = Convert.ToInt32(ddlType.SelectedValue);
                        int processCategory = Convert.ToInt32(ddlCategories.SelectedValue);
                        int surface = Convert.ToInt32(ddlSurface.SelectedValue);
                        int grade = Convert.ToInt32(ddlClassification.SelectedValue);
                        int size = Convert.ToInt32(ddlSizeTab3.SelectedValue);
                        int shade = Convert.ToInt32(Ddlshade.SelectedValue);
                        dtProduct = BusinessLogic.Inventory.FormerMasterBL.GetProduct(CategoryPK, currentUser.SBUID, typeNature, processCategory, surface, grade, size, shade);//Get Product to bind DropDown
                        break;
                    #endregion
                    #region STOREMAPPING
                    case ControlsEnum.STOREMAPPING:
                        dtStoreMapping = BusinessLogic.Inventory.FormerMasterBL.GetMaterialStores((GetNullableInt(hdfMaterialPk.Value) ?? 0), currentUser.SBUID);
                        StoreMappingHeaderViewState.StoreList = dtStoreMapping.ToList<StoreMapping>();
                        break;
                    #endregion
                    #region VENDORDETAILS
                    case ControlsEnum.VENDORDETAILS:
                        int vendorID = string.IsNullOrEmpty(hdfVendorID.Value) ? 0 : Convert.ToInt32(hdfVendorID.Value);
                        if (vendorID > 0)
                        {
                            dtPageData = BusinessLogic.VendorManagement.VendorRegistration.GetVendorData(vendorID);
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
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    case ControlsEnum.CATEGORY:
                        if (dtCategory != null && dtCategory.Rows.Count > 0)
                        {
                            hdfItemCategory.Value = (dtCategory.Rows[0][0]).ToString();
                        }
                        break;
                    case ControlsEnum.SIZE:
                        BindDropDown(ControlsEnum.SIZE);
                        break;
                    case ControlsEnum.UOM:
                        BindDropDown(ControlsEnum.UOM);
                        break;
                    case ControlsEnum.VENDORUOM:
                        BindDropDown(ControlsEnum.VENDORUOM);
                        break;
                    case ControlsEnum.PRODUCTGROUP:
                        BindDropDown(ControlsEnum.PRODUCTGROUP);
                        break;
                    case ControlsEnum.EDIT:
                        // SetUIEditView(ActionsEnum.EDIT);
                        GetUIValuesFromObject(ControlsEnum.EDIT);
                        break;
                    case ControlsEnum.PRODUCTDETAIL:
                        BindDropDown(ControlsEnum.PRODUCTDETAIL);
                        break;
                    case ControlsEnum.PRODUCT:
                        BindProduct();
                        break;
                    case ControlsEnum.PRODUCTSGRID:
                        BindProductsGrid();
                        break;
                    case ControlsEnum.STOREMAPPING:
                        BindStoreMappingTreeView();
                        break;
                    case ControlsEnum.VENDORDETAILS:
                        BindDropDown(ControlsEnum.CURRENCY);
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
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region SIZE
                case ControlsEnum.SIZE:
                    //fill the Size
                    ddlSize.Items.Clear();
                    ddlSize.DataSource = dtProductProperties;
                    ddlSize.DataTextField = "CON_NAME";
                    ddlSize.DataValueField = "CON_PK";
                    ddlSize.DataBind();
                    ddlSize.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region UOM
                case ControlsEnum.UOM:
                    //fill the UOM
                    ddlUOM.Items.Clear();
                    if (invUomMstList != null && invUomMstList.Count > 0)
                    {
                        ddlUOM.DataSource = invUomMstList
                            .OrderBy(o => o.UOM_CODE)
                            .ToList();
                        ddlUOM.DataTextField = Resources.DataFieldRes.UomCode;
                        ddlUOM.DataValueField = Resources.DataFieldRes.UomPK;
                        ddlUOM.DataBind();
                    }
                    ddlUOM.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region UNIT
                case ControlsEnum.VENDORUOM:
                    //fill the Unit
                    ddlUnit.Items.Clear();
                    if (invUomMstList != null && invUomMstList.Count > 0)
                    {
                        ddlUnit.DataSource = invUomMstList
                            .OrderBy(o => o.UOM_CODE)
                            .ToList();
                        ddlUnit.DataTextField = Resources.DataFieldRes.UomCode;
                        ddlUnit.DataValueField = Resources.DataFieldRes.UomPK;
                        ddlUnit.DataBind();
                    }
                    ddlUnit.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region PRODUCTGROUP
                case ControlsEnum.PRODUCTGROUP:
                    ddlProductGroup.Items.Clear();
                    if (invItemGroupMstList != null && invItemGroupMstList.Count > 0)
                    {
                        ddlProductGroup.DataSource = invItemGroupMstList;
                        ddlProductGroup.DataTextField = Resources.DataFieldRes.INVProductGroupName;
                        ddlProductGroup.DataValueField = Resources.DataFieldRes.INVProductGroupPK;
                        ddlProductGroup.DataBind();
                        ddlProductGroup.Items.HtmlDecode();
                    }
                    ddlProductGroup.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region PRODUCTDETAIL
                case ControlsEnum.PRODUCTDETAIL:
                    ddlCategory.Items.Clear();
                    if (dtCategory != null && dtCategory.Rows.Count > 0)
                    {
                        dtCategory = CommonFunctions.HtmlDecodeDataTable(dtCategory, "ITC_NAME");//Decode DataTable
                        ddlCategory.DataTextField = "ITC_NAME";
                        ddlCategory.DataValueField = "PK";
                        ddlCategory.DataSource = dtCategory;
                        ddlCategory.DataBind();
                        ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByText("FG"));
                    }
                    ddlCategory.Items.Insert(0, new ListItem(Resources.Report.Select, "0"));
                    break;
                #endregion
                case ControlsEnum.CURRENCY:
                    ddlCurrency.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlCurrency.DataSource = dtPageData;
                        ddlCurrency.DataTextField = "VEN_CURRENCY_CODE";
                        ddlCurrency.DataValueField = "VEN_CURRENCY";
                        ddlCurrency.DataBind();
                        //ddlCurrency.Enabled = false;
                    }
                    //ddlCurrency.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

                    break;
                default:
                    break;
            }
        }

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
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                BusinessObject.MaterialManagement.Material material;
                switch (controlType)
                {
                    case ControlsEnum.MATERIAL:
                        material = new BusinessObject.MaterialManagement.Material();
                        material.MaterialDetailId = GetNullableInt(hdfMaterialPk.Value) ?? 0;
                        material.ITM_CODE = txtItemCode.Text.Trim();
                        material.ITM_NAME = txtName.Text.Trim();
                        material.ITC_PK = Convert.ToInt32(hdfItemCategory.Value);
                        material.ITM_DESC = txtDescription.Text.Trim();
                        material.ITM_TYPE = 1;
                        material.ITM_TYPE_TEXT = "1";//Material master Eligibility field not saved (Bug ID:41103)
                        material.P_ISD_SIZE = Convert.ToInt32(ddlSize.SelectedValue);
                        material.UOM_PK = Convert.ToInt32(ddlUOM.SelectedValue);
                        material.ITM_GROUP = Convert.ToInt32(ddlProductGroup.SelectedValue);
                        material.P_OST_QTY_OPENING = Convert.ToDecimal(txtOpeningStock.Text.Trim());
                        material.ITM_NEED_QC_INSP = chkRequireInspection.Checked ? "1" : "0";
                        material.ITM_MIN_STK = 0;
                        material.ITM_MAX_STK = 0;
                        material.ITM_ROL_STK = 0;
                        material.ITM_MOQ = 0;
                        material.UserID = currentUser.PKUser;
                        material.UserPk = currentUser.PKUser;
                        material.SBU = currentUser.SBUID;
                        material.BizUnitPk = currentUser.SBUID;
                        material.ITM_SET = Convert.ToInt32(ItemCategory.Former);
                        material.STATUS = 1;
                        if (hdfLastModifiedDate.Value != string.Empty)
                        {
                            material.P_LAST_MOD_DT = Convert.ToDateTime(hdfLastModifiedDate.Value);
                        }
                        retObject = material;
                        break;

                    case ControlsEnum.VENDORLISTING:
                        objVenHeader.UserPk = Convert.ToInt16(currentUser.PKUser);
                        objVenHeader.SBU = Convert.ToInt16(currentUser.SBUID);
                        objVenHeader.ITM_PK = CurrPK;
                        objVenHeader.VenMapList = VenMaterialList;
                        retObject = objVenHeader;
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

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EDIT:
                        if (dtMaterials != null && dtMaterials.Rows.Count > 0)
                        {
                            txtItemCode.Text = (dtMaterials.Rows[0]["ITM_CODE"]).ToString();
                            txtName.Text = (dtMaterials.Rows[0]["ITM_NAME"]).ToString();
                            txtDescription.Text = (dtMaterials.Rows[0]["ITM_DESC"]).ToString();
                            lblMaterialCode.Text = txtItemCode.Text;
                            lblMaterialName.Text = txtName.Text;

                            if (dtMaterials.Rows[0]["ISD_SIZE"] != null)
                            {
                                ddlSize.SelectedIndex = Convert.ToInt32(ddlSize.Items.IndexOf(ddlSize.Items.FindByValue((dtMaterials.Rows[0]["ISD_SIZE"]).ToString())));
                            }
                            if (dtMaterials.Rows[0]["ITM_UOM"] != null)
                            {
                                ddlUOM.SelectedIndex = Convert.ToInt32(ddlUOM.Items.IndexOf(ddlUOM.Items.FindByValue((dtMaterials.Rows[0]["ITM_UOM"]).ToString())));
                            }
                            if (dtMaterials.Rows[0]["ITM_GROUP"] != null)
                            {
                                ddlProductGroup.SelectedIndex = Convert.ToInt32(ddlProductGroup.Items.IndexOf(ddlProductGroup.Items.FindByValue((dtMaterials.Rows[0]["ITM_GROUP"]).ToString())));
                            }
                            txtOpeningStock.Text = dtMaterials.Rows[0]["OST_QTY_OPENING"] != null ? (dtMaterials.Rows[0]["OST_QTY_OPENING"]).ToString() : "";
                            hdfLastModifiedDate.Value = dtMaterials.Rows[0]["LAST_MOD_DT"] != null ? (dtMaterials.Rows[0]["LAST_MOD_DT"]).ToString() : "";
                            chkRequireInspection.Checked = dtMaterials.Rows[0]["ITM_NEED_QC_INSP"] != null ? Convert.ToBoolean(dtMaterials.Rows[0]["ITM_NEED_QC_INSP"]) : false;
                            BindProductsGrid();
                            BindStoreMappingTreeView();
                            BindGrid(ControlsEnum.VENDORLISTING);
                        }
                        else
                        {
                            ActionHandler(lbnListing, EventArgs.Empty);
                            litErrorMsg.Text = GetLocalResourceObject("Msg_NoRecordsFound").ToString();
                            throw new ApplicationException(GetLocalResourceObject("Msg_NoRecordsFound").ToString());
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
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region  LIST
                    case ControlsEnum.LIST:
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdList.DataSource = dtMaterials;
                        grdList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    #endregion

                    #region VENDOR LIST
                    case ControlsEnum.VENDORLISTING:
                        if (VenMaterialList != null && VenMaterialList.Count > 0)
                        {
                            grdVendorMaterialDetails.DataSource = VenMaterialList;
                            grdVendorMaterialDetails.DataBind();
                        }
                        else
                        {
                            grdVendorMaterialDetails.DataSource = null;
                            grdVendorMaterialDetails.DataBind();
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
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                if (Mode == ActionsEnum.VIEW)
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

        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
        }

        /// <summary>
        /// Get Nullable Int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns>int?</returns>
        private int? GetNullableInt(string str)
        {
            int r;
            int? result = null;
            if (int.TryParse(str, out r))
            {
                result = r;
            }
            return result;
        }

        private int GetCurrVenSequenceNo()
        {
            if (VenMaterialList != null)
            {
                if (VenMaterialList.Count > 0)
                {
                    if (VenMaterialList.Where(itm => itm.ITV_PK == 0).Count() >= 1)
                    {
                        CurrVenSlNo = CurrVenSlNo + 1;
                    }
                    else
                    {
                        CurrVenSlNo = VenMaterialList.Max(itm => itm.ITV_PK) + 1;
                    }
                }
            }
            return CurrVenSlNo;
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
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GridViewRow gvr;
                bool bIsChecked = false;
                HiddenField hdfSlNo;

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
                    commonActions = ActionsEnum.CHANGE;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            if (hdfVendorMapping.Value == "1") // Save Vendor Mapping Details
                            {
                                objVenHeader = new VenMappingHeader();
                                objVenHeader = (VenMappingHeader)SetUIValuesToObject(ControlsEnum.VENDORLISTING);
                                if (objVenHeader != null && objVenHeader.VenMapList != null)
                                {
                                    string xmlDoc = CommonFunctions.XmlSerialize<VenMappingHeader>(objVenHeader);
                                    result = BusinessLogic.Inventory.FormerMasterBL.SaveMaterialVendorDetails(xmlDoc);
                                    if (result > 0)
                                    {
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.VendorDetails);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                        ActionHandler(lbnListing, EventArgs.Empty);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailed + " " + Resources.Messages.AddAtleastOneItem;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                            }
                            else // Save Former, Product Mapping & Store Mapping Details
                            {
                                BusinessObject.MaterialManagement.Material material;
                                material = (BusinessObject.MaterialManagement.Material)SetUIValuesToObject(ControlsEnum.MATERIAL);
                                string xmlPrd = CreateProductDetailXml();
                                string storeXml = CreateStoreMappingXml(); // string.empty
                                int materiaID = Convert.ToInt32(DataAccess.MaterialManagement.MaterialMasterDL.SaveMaterial(material, currentUser.SBUID, storeXml, xmlPrd));
                                if (materiaID > 0)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_FormerSaved").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                    ActionHandler(lbnListing, EventArgs.Empty);
                                }
                                else if (materiaID == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.FormerItem + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                                else if (materiaID == (int)DbSaveStatus.OLDCODEEXIST)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_ItemExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_ItemExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Cancel
                    // Do Action for , when click cancel button
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ClearControls();
                        ClearVendorMapping();
                        ActivateList();
                        break;
                    #endregion
                    #region Clear
                    // Do Action for , when click clear button
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Default
                    // Do Action for , when click clear button
                    case ActionsEnum.DEFAULT:
                        //if (hdfMaterialPk.Value == string.Empty)
                        //{
                        //    ActionHandler(btnEdit, EventArgs.Empty);
                        //}
                        //else
                        //{
                        //    EntryStatus = EntryStatus.ENTRYMODE;
                        //    ActivateDetail();
                        //}
                        bIsChecked = false;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ActionHandler(btnEdit, EventArgs.Empty);
                        }
                        else
                        {
                            EntryStatus = EntryStatus.ENTRYMODE;
                            ActivateDetail();
                        }
                        break;
                    #endregion
                    #region List
                    case ActionsEnum.LIST:
                        ClearControls();
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        ActivateList();
                        break;
                    #endregion
                    #region EDIT
                    // Do Action for , when click clear button
                    case ActionsEnum.EDIT:
                        bIsChecked = false;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                hdfMaterialPk.Value = ((HiddenField)grdrow.FindControl("hdfItemPk")).Value;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfItemPk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ClearVendorMapping();
                            EntryStatus = EntryStatus.ENTRYMODE;
                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            ActivateDetail();
                        }
                        else
                        {
                            ClearControls();
                            string msg = GetLocalResourceObject("Err_NoRecordsSelected").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        ActivateDetail();
                        ClearControls();
                        ClearVendorMapping();
                        break;
                    #endregion
                    #region PRODUCTDETAIL
                    case ActionsEnum.PRODUCTDETAIL:
                        GetFieldValues(ControlsEnum.PRODUCTDETAIL);
                        ActivateProductDetails();
                        break;
                    #endregion
                    #region VENDORMAPPING
                    case ActionsEnum.VENDORMAPPING:
                        ActivateVendorMapping();
                        break;
                    #endregion
                    #region CHANGE
                    case ActionsEnum.CHANGE:
                        GetFieldValues(ControlsEnum.PRODUCT);
                        SetFieldValues(ControlsEnum.PRODUCT);
                        ddlCategory.Focus();
                        break;
                    #endregion
                    #region ADD_ACTION
                    case ActionsEnum.ADD_ACTION:
                        AddLineProduct();
                        SetFieldValues(ControlsEnum.PRODUCTSGRID);
                        break;
                    #endregion
                    #region CANCEL_ACTION
                    case ActionsEnum.CANCEL_ACTION:
                        ClearProductFields();
                        break;
                    #endregion
                    #region DELETE_ACTION
                    case ActionsEnum.DELETE_ACTION:
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        ProductIndex = gvr.RowIndex + 1;
                        DeleteLineProduct();
                        SetFieldValues(ControlsEnum.PRODUCTSGRID);
                        ProductIndex = -1;
                        break;
                    #endregion
                    #region CLEARMAPPING
                    case ActionsEnum.CLEARMAPPING:
                        ProdDetails = null;
                        hdfFormerItemMapping.Value = string.Empty;
                        BindProductsGrid();
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Inventory.FormerMasterBL.DeleteFormerMasterDetails(Convert.ToInt32(hdfMaterialPk.Value));
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset 
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                ResetForm();
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                                EntryStatus = EntryStatus.LISTMODE;
                                ClearControls();
                                ActivateList();
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.FormerItem);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.FormerItem + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ClearControls();
                                    ActivateList();
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.FormerItem + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.FormerItem + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ClearControls();
                                    ActivateList();
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.FormerItem);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region STOREMAPPING
                    case ActionsEnum.STOREMAPPING:
                        ActivateStoreMapping();
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        PageIndex = 1.ToString();
                        if (SearchType.SelectedValue == 0.ToString())
                        {
                            SearchValue.Text = string.Empty;
                        }
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region VENDORMAPPING
                    case ActionsEnum.VENDORSELECTED:
                        GetFieldValues(ControlsEnum.VENDORDETAILS);
                        SetFieldValues(ControlsEnum.VENDORDETAILS);
                        break;
                    #endregion
                    #region ADD VENDOR DETAILS
                    case ActionsEnum.ADDVENDORDETAILS:
                        if (VenMaterialList == null)
                        {
                            VenMaterialList = new List<VenMappingDetails>();
                        }
                        var existVenCount=0;
                        if (VenMaterialList.Count > 0)
                        {
                            existVenCount  = VenMaterialList.Where(ven => ven.ITV_VENDOR == Convert.ToInt32(hdfVendorID.Value)).Count();
                        }

                        if (existVenCount > 0)
                        {
                            string msg = GetLocalResourceObject("VendorExist").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Messages.Information + "');", true);
                            break;
                        }
                        else
                        {
                            if (Convert.ToInt32(hdfMappingSlNo.Value) > 0)
                            {
                                objVenMaterial = VenMaterialList.SingleOrDefault(itm => itm.ITV_SL_NO == Convert.ToInt32(hdfMappingSlNo.Value));
                                objVenMaterial.ITV_VENDOR = Convert.ToInt32(hdfVendorID.Value);
                                objVenMaterial.ITV_VENDORNAME = txtVendor.Text;
                                objVenMaterial.ITV_NAME = txtMaterialName.Text;
                                objVenMaterial.ITV_PRICE = float.Parse(txtStdPrice.Text);
                                objVenMaterial.ITV_CURRENCY = Convert.ToInt32(ddlCurrency.SelectedValue);
                                objVenMaterial.MaterialCurrencyText = ddlCurrency.SelectedItem.Text;
                                objVenMaterial.ITV_MOQ = float.Parse(txtMOQ.Text);
                                objVenMaterial.ITV_MOQ_UOM = Convert.ToInt32(ddlUnit.SelectedValue);
                                objVenMaterial.UOMText = ddlUnit.SelectedItem.ToString();
                                objVenMaterial.ITV_LEAD_TIME = Convert.ToInt32(txtLeadDays.Text);
                                objVenMaterial.ITV_ACTIVE = chkActive.Checked == true ? 1 : 0;
                            }
                            else
                            {
                                objVenMaterial = new VenMappingDetails();
                                objVenMaterial.ITV_ITEM = CurrPK;
                                objVenMaterial.ITV_VENDOR = Convert.ToInt32(hdfVendorID.Value);
                                objVenMaterial.ITV_VENDORNAME = txtVendor.Text;
                                objVenMaterial.ITV_NAME = txtMaterialName.Text;
                                objVenMaterial.ITV_PRICE = float.Parse(txtStdPrice.Text);
                                objVenMaterial.ITV_CURRENCY = Convert.ToInt32(ddlCurrency.SelectedValue);
                                objVenMaterial.MaterialCurrencyText = ddlCurrency.SelectedItem.Text;
                                objVenMaterial.ITV_MOQ = float.Parse(txtMOQ.Text);
                                objVenMaterial.ITV_MOQ_UOM = Convert.ToInt32(ddlUnit.SelectedValue);
                                objVenMaterial.UOMText = ddlUnit.SelectedItem.ToString();
                                objVenMaterial.ITV_LEAD_TIME = Convert.ToInt32(txtLeadDays.Text);
                                objVenMaterial.ITV_ACTIVE = chkActive.Checked == true ? 1 : 0;
                                objVenMaterial.ITV_SL_NO = GetCurrVenSequenceNo();
                                VenMaterialList.Add(objVenMaterial);
                            }
                            BindGrid(ControlsEnum.VENDORLISTING);
                            ClearVendorMapping();
                        }
                        break;
                    #endregion

                    #region REMOVE VENDOR DETAILS
                    case ActionsEnum.REMOVEVENDORDETAILS:
                        if (VenMaterialList != null && VenMaterialList.Count > 0)
                        {
                            hdfSlNo = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfSlNo") as HiddenField);
                            var removeitm = VenMaterialList.SingleOrDefault(row => row.ITV_SL_NO == Convert.ToInt32(hdfSlNo.Value));
                            if (removeitm != null)
                            {
                                VenMaterialList.Remove(removeitm);
                                BindGrid(ControlsEnum.VENDORLISTING);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailed;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region EDIT VENDOR DETAILS
                     case ActionsEnum.EDITVENDORDETAILS:
                        if (VenMaterialList != null && VenMaterialList.Count > 0)
                        {
                            hdfSlNo = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfSlNo") as HiddenField);
                            var editItm = VenMaterialList.SingleOrDefault(row => row.ITV_SL_NO == Convert.ToInt32(hdfSlNo.Value));
                            if (editItm != null)
                            {
                                hdfMappingSlNo.Value = editItm.ITV_SL_NO.ToString();
                                txtVendor.Text = editItm.ITV_VENDORNAME;
                                hdfVendorID.Value = editItm.ITV_VENDOR.ToString();
                                GetFieldValues(ControlsEnum.VENDORDETAILS);
                                SetFieldValues(ControlsEnum.VENDORDETAILS);
                                txtMaterialName.Text = editItm.ITV_NAME;
                                txtStdPrice.Text = editItm.ITV_PRICE.ToString();
                                ddlCurrency.SelectedValue = editItm.ITV_CURRENCY.ToString();
                                txtMOQ.Text = editItm.ITV_MOQ.ToString();
                                ddlUnit.SelectedValue = editItm.ITV_MOQ_UOM.ToString();
                                txtLeadDays.Text = editItm.ITV_LEAD_TIME.ToString();
                                chkActive.Checked = editItm.ITV_ACTIVE == 1 ? true : false;
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailed;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
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
                    //if (SortDirection == Resources.Report.SortAscending)
                    //    SortDirection = Resources.Report.SortDescending;
                    //else
                    //    SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    //  SortDirection = Resources.Report.SortAscending;

                }
                this.PageIndex = "1";
                //GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                //SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                EntryStatus = EntryStatus.LISTMODE;
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
            uclPaging.CurrentPage = 1;
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
                        // Assignment the first page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assignment the last page index.
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
            // Should we disable the first link
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(1);});", true);
                    //lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(2);});", true);
                    //lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    //lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    //lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideManageResource();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }

        private void ClearControls()
        {
            hdfMaterialPk.Value = string.Empty;
            txtItemCode.Text = txtName.Text = txtDescription.Text = string.Empty;
            txtOpeningStock.Text = "0";
            hdfVendorMapping.Value = "0";
            chkRequireInspection.Checked = false;
            ddlProductGroup.SelectedIndex = ddlSize.SelectedIndex = ddlUOM.SelectedIndex = ddlUnit.SelectedIndex = 0;
            grdLineProduct.DataSource = null;
            grdLineProduct.DataBind();
            hdfFormerItemMapping.Value = string.Empty;
            GetFieldValues(ControlsEnum.STOREMAPPING);
            SetFieldValues(ControlsEnum.STOREMAPPING);
            VenMaterialList = null;
        }

        private void ActivateProductDetails()
        {
            spnProductDetail.Attributes["class"] = "tab-active";
            spnVendorMapping.Attributes["class"] = "tab-inactive";
            spnListing.Attributes["class"] = "tab-inactive";
            spnDetail.Attributes["class"] = "tab-inactive";
            lbnProductDetail.CssClass = "tab-active";
            lbnListing.CssClass = "tab-inactive";
            lbnDetail.CssClass = "tab-inactive";
            lbnVendorMapping.CssClass = "tab-inactive";
            divProductDetail.Visible = true;
            PageAction_List.Visible = false;
            PageAction_Entry.Visible = false;
            //uclPagingMyTask.CurrentPage = 1;
            //uclPagingMyTask.BindPager();
            ddlCategory.Focus();

            spnStoreMapping.Attributes["class"] = "tab-inactive";
            divStoreMapping.Visible = false;
            divVendorMapping.Visible = false;
            hdfVendorMapping.Value = "0";
        }

        private void ActivateList()
        {
            spnProductDetail.Attributes["class"] = "tab-inactive";
            spnVendorMapping.Attributes["class"] = "tab-inactive";
            spnListing.Attributes["class"] = "tab-active";
            spnDetail.Attributes["class"] = "tab-inactive";
            lbnProductDetail.CssClass = "tab-inactive";
            lbnVendorMapping.CssClass = "tab-inactive";
            lbnListing.CssClass = "tab-active";
            lbnDetail.CssClass = "tab-inactive";
            divProductDetail.Visible = false;
            PageAction_List.Visible = true;
            PageAction_Entry.Visible = false;
            //uclPagingMyTask.CurrentPage = 1;
            //uclPagingMyTask.BindPager();
            spnProductDetail.Visible = false;
            lbnProductDetail.Visible = false;
            hdfMaterialPk.Value = string.Empty;

            spnStoreMapping.Visible = false;
            lbnStoreMapping.Visible = false;
            divStoreMapping.Visible = false;

            spnVendorMapping.Visible = false;
            lbnVendorMapping.Visible = false;
            divVendorMapping.Visible = false;
            hdfVendorMapping.Value = "0";
        }

        private void ActivateDetail()
        {
            spnVendorMapping.Visible = true;
            lbnVendorMapping.Visible = true;
            spnVendorMapping.Attributes["class"] = "tab-inactive";
            lbnVendorMapping.CssClass = "tab-inactive";
            divVendorMapping.Visible = false;

            spnProductDetail.Visible = true;
            lbnProductDetail.Visible = true;
            spnProductDetail.Attributes["class"] = "tab-inactive";
            spnListing.Attributes["class"] = "tab-inactive";
            spnDetail.Attributes["class"] = "tab-active";
            lbnProductDetail.CssClass = "tab-inactive";
            lbnListing.CssClass = "tab-inactive";
            lbnDetail.CssClass = "tab-active";
            divProductDetail.Visible = false;
            PageAction_List.Visible = false;
            PageAction_Entry.Visible = true;

            spnStoreMapping.Attributes["class"] = "tab-inactive";
            lbnStoreMapping.CssClass = "tab-inactive";
            divStoreMapping.Visible = false;
            spnStoreMapping.Visible = true;
            lbnStoreMapping.Visible = true;

            //uclPagingMyTask.CurrentPage = 1;
            //uclPagingMyTask.BindPager();

            hdfVendorMapping.Value = "0";
            txtItemCode.Focus();
        }

        private void ActivateStoreMapping()
        {
            spnStoreMapping.Attributes["class"] = "tab-active";
            spnProductDetail.Attributes["class"] = "tab-inactive";
            spnVendorMapping.Attributes["class"] = "tab-inactive";
            spnListing.Attributes["class"] = "tab-inactive";
            spnDetail.Attributes["class"] = "tab-inactive";
            lbnStoreMapping.CssClass = "tab-active";
            lbnProductDetail.CssClass = "tab-inactive";
            lbnVendorMapping.CssClass = "tab-inactive";
            lbnListing.CssClass = "tab-inactive";
            lbnDetail.CssClass = "tab-inactive";
            divStoreMapping.Visible = true;
            divProductDetail.Visible = false;
            divVendorMapping.Visible = false;
            PageAction_List.Visible = false;
            PageAction_Entry.Visible = false;
            //uclPagingMyTask.CurrentPage = 1;
            //uclPagingMyTask.BindPager();
            ddlCategory.Focus();
            hdfVendorMapping.Value = "0";
        }

        private void ActivateVendorMapping()
        {
            spnStoreMapping.Attributes["class"] = "tab-inactive";
            spnProductDetail.Attributes["class"] = "tab-inactive";
            spnVendorMapping.Attributes["class"] = "tab-active";
            spnListing.Attributes["class"] = "tab-inactive";
            spnDetail.Attributes["class"] = "tab-inactive";
            lbnStoreMapping.CssClass = "tab-inactive";
            lbnProductDetail.CssClass = "tab-inactive";
            lbnVendorMapping.CssClass = "tab-active";
            lbnListing.CssClass = "tab-inactive";
            lbnDetail.CssClass = "tab-inactive";
            divStoreMapping.Visible = false;
            divProductDetail.Visible = false;
            divVendorMapping.Visible = true;
            PageAction_List.Visible = false;
            PageAction_Entry.Visible = false;
            //uclPagingMyTask.CurrentPage = 1;
            //uclPagingMyTask.BindPager();
            //ddlCategory.Focus();
            hdfVendorMapping.Value = "1";
        }

        private void BindProductProperties()
        {
            #region ProductPropertiesDropDownBind

            int groupType = Convert.ToInt32(Convert.ChangeType(ConstGroupType.Product, ConstGroupType.Product.GetTypeCode()));
            DataTable dtProductProperties = new DataTable();
            dtProductProperties = BusinessLogic.Inventory.FormerMasterBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Type, ProductProperties.Type.GetTypeCode())));
            ddlType.DataValueField = "CON_PK";
            ddlType.DataTextField = "CON_NAME";
            ddlType.DataSource = dtProductProperties;
            ddlType.DataBind();
            ddlType.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
            ddlType.SelectedIndex = 0;
            dtProductProperties = new DataTable();
            dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Category, ProductProperties.Category.GetTypeCode())));
            ddlCategories.DataValueField = "CON_PK";
            ddlCategories.DataTextField = "CON_NAME";
            ddlCategories.DataSource = dtProductProperties;
            ddlCategories.DataBind();
            ddlCategories.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
            ddlCategories.SelectedIndex = 0;
            dtProductProperties = new DataTable();
            dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Surface, ProductProperties.Surface.GetTypeCode())));
            ddlSurface.DataValueField = "CON_PK";
            ddlSurface.DataTextField = "CON_NAME";
            ddlSurface.DataSource = dtProductProperties;
            ddlSurface.DataBind();
            ddlSurface.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
            ddlSurface.SelectedIndex = 0;
            dtProductProperties = new DataTable();
            dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Classification, ProductProperties.Classification.GetTypeCode())));
            ddlClassification.DataValueField = "CON_PK";
            ddlClassification.DataTextField = "CON_NAME";
            ddlClassification.DataSource = dtProductProperties;
            ddlClassification.DataBind();
            ddlClassification.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
            ddlClassification.SelectedIndex = 0;
            dtProductProperties = new DataTable();
            dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Size, ProductProperties.Size.GetTypeCode())));
            ddlSizeTab3.DataValueField = "CON_PK";
            ddlSizeTab3.DataTextField = "CON_NAME";
            ddlSizeTab3.DataSource = dtProductProperties;
            ddlSizeTab3.DataBind();
            ddlSizeTab3.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
            ddlSizeTab3.SelectedIndex = 0;

            dtProductProperties = new DataTable();
            dtProductProperties = BusinessLogic.BrandRates.BrandRatesBL.GetProductProperties(currentUser.SBUID, groupType, Convert.ToInt32(Convert.ChangeType(ProductProperties.Shade, ProductProperties.Shade.GetTypeCode())));
            Ddlshade.DataValueField = "CON_PK";
            Ddlshade.DataTextField = "CON_NAME";
            Ddlshade.DataSource = dtProductProperties;
            Ddlshade.DataBind();
            Ddlshade.Items.Insert(0, new ListItem { Value = "0", Text = "All" });
            Ddlshade.SelectedIndex = 0;
            #endregion ProductPropertiesDropDownBind
        }

        /// <summary>
        /// Binds the Product Dropdown
        /// </summary>
        private void BindProduct()
        {
            ddlItem.Items.Clear();
            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                dtProduct = CommonFunctions.HtmlDecodeDataTable(dtProduct, "PRO_TEXT");//Decode DataTable
                ddlItem.DataTextField = "PRO_TEXT";
                ddlItem.DataValueField = "PRO_PK";
                ddlItem.DataSource = dtProduct;
                ddlItem.DataBind();
            }
            //ddlItem.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));         
            ddlItem.Items.Insert(0, new ListItem("ALL", "0"));
        }

        /// <summary>
        /// To add ProductDetails item
        /// </summary>
        private void AddLineProduct()
        {
            if (ddlItem.SelectedValue == "0")
            {
                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    ProdDetails = (dtProduct == null || dtProduct.Rows.Count == 0) ? null : new List<ProductDetails>();
                    ProductDetails objProdDetail;

                    if (hdfFormerItemMapping.Value.Trim() != string.Empty)
                    {
                        ProdDetails = JsonConvert.DeserializeObject<List<ProductDetails>>(hdfFormerItemMapping.Value.Trim());
                    }
                    if (ProdDetails == null)
                    {
                        ProdDetails = new List<ProductDetails>();
                    }
                    //Assign the Product Details List with DataTable items
                    bool _duplicate = false;
                    foreach (DataRow dr in dtProduct.Rows)
                    {
                        objProdDetail = new ProductDetails();
                        objProdDetail.ITM_PK = Convert.ToInt32(dr["PRO_PK"].ToString() == string.Empty ? "0" : dr["PRO_PK"]);
                        objProdDetail.ITM_TEXT = dr["PRO_TEXT"].ToString();
                        if (IsExist(objProdDetail.ITM_PK))  //Don't allow multiple entry of same Product
                        {
                            _duplicate = true;
                        }
                        else
                        {
                            ProdDetails.Add(objProdDetail);
                        }
                    }
                    hdfFormerItemMapping.Value = JsonConvert.SerializeObject(ProdDetails);
                    ClearProductFields();
                    if (_duplicate)
                    {
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_Duplicate").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                    }
                }
                else
                {
                    litErrorMsg.Text = this.GetLocalResourceObject("Err_NoItem").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                }
            }
            else
            {
                ProductDetails objProduct = new ProductDetails();
                objProduct.ITM_PK = Convert.ToInt32(ddlItem.SelectedValue);
                objProduct.ITM_TEXT = ddlItem.SelectedItem.Text.Trim();
                if (hdfFormerItemMapping.Value.Trim() != string.Empty)
                {
                    ProdDetails = JsonConvert.DeserializeObject<List<ProductDetails>>(hdfFormerItemMapping.Value.Trim());
                }
                if (ProdDetails == null)
                {
                    ProdDetails = new List<ProductDetails>();
                }
                int listCount = ProdDetails.Count;
                //Update the item if edit-add
                if (listCount > 0 && ProductIndex > 0 && ProductIndex <= listCount)
                {
                    ProdDetails.RemoveAt(ProductIndex - 1);
                    ProdDetails.Insert(ProductIndex - 1, objProduct);
                }
                //Add new item
                else if (ProdDetails.Find(pr => pr.ITM_PK == objProduct.ITM_PK) == null)
                {
                    ProdDetails.Add(objProduct);
                }
                //Don't allow multiple entry of same Product
                else
                {
                    litErrorMsg.Text = this.GetLocalResourceObject("Err_Product_Selection").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                }
                hdfFormerItemMapping.Value = JsonConvert.SerializeObject(ProdDetails);
                ClearProductFields();
            }
        }

        /// <summary>
        /// Is Exist an Item
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsExist(int pk)
        {
            List<ProductDetails> prdDetails;
            prdDetails = JsonConvert.DeserializeObject<List<ProductDetails>>(hdfFormerItemMapping.Value.Trim());
            return ProdDetails.Find(pr => pr.ITM_PK == pk) == null ? false : true;
        }

        /// <summary>
        /// Resets the Product Line Speed Entry Fields
        /// </summary>
        private void ClearProductFields()
        {
            //ProductIndex = 0;
            //ddlCategory.ClearSelection();
            //ddlType.ClearSelection();
            //ddlCategories.ClearSelection();
            //ddlSurface.ClearSelection();
            //ddlClassification.ClearSelection();
            //ddlSizeTab3.ClearSelection();
            //ddlItem.Items.Clear();
            //ddlItem.Items.Insert(0, new ListItem("ALL", "0"));
            //ddlItem.ClearSelection();
            //hfdPlmPk.Value = string.Empty;
        }

        private void ClearVendorMapping()
        {
            txtVendor.Text = string.Empty;
            hdfVendorID.Value = "0";
            txtStdPrice.Text = string.Empty;
            txtLeadDays.Text = string.Empty;
            ddlCurrency.ClearSelection();
            chkActive.Checked = false;
            txtMaterialName.Text = string.Empty;
            txtMOQ.Text = string.Empty;
            ddlUnit.ClearSelection();
            hdfMappingSlNo.Value = "0";
        }

        /// <summary>
        /// Binds the Products Grid
        /// </summary>
        private void BindProductsGrid()
        {
            if (ProdDetails != null && ProdDetails.Count > 0)
                grdLineProduct.DataSource = ProdDetails;
            else
                grdLineProduct.DataSource = null;

            grdLineProduct.DataBind();
        }

        /// <summary>
        /// To delete ProductDetails item
        /// </summary>
        private void DeleteLineProduct()
        {
            if (hdfFormerItemMapping.Value.Trim() != string.Empty)
            {
                ProdDetails = JsonConvert.DeserializeObject<List<ProductDetails>>(hdfFormerItemMapping.Value.Trim());
            }
            int listCount = ProdDetails.Count;
            if (ProductIndex > 0 && ProductIndex <= listCount)
            {
                ProdDetails.RemoveAt(ProductIndex - 1);
            }
            hdfFormerItemMapping.Value = JsonConvert.SerializeObject(ProdDetails);
            ClearProductFields();
        }

        private string CreateProductDetailXml()
        {
            string result = string.Empty;
            result = "<root>";
            foreach (GridViewRow grdrow in grdLineProduct.Rows)
            {
                HiddenField hdfProductPK = ((HiddenField)grdrow.FindControl("hdfProductPK"));
                result += "<ProdMap>";
                result += "<FPM_PRODUCT>" + hdfProductPK.Value + "</FPM_PRODUCT>";
                result += "<FPM_ACTIVE>1</FPM_ACTIVE>";
                result += "</ProdMap>";
            }
            result += "</root>";
            if (result == "<root></root>")
            {
                result = string.Empty;
            }

            //List<ProductDetails> ProdDetails = JsonConvert.DeserializeObject<List<ProductDetails>>(hdfFormerItemMapping.Value.Trim());
            //Prod1 objProd1 = new Prod1();
            //objProd1.Prod = ProdDetails;

            //System.Xml.XmlDocument xdoc = CommonFunctions.ObjectTOXml(objProd1);
            //string t = xdoc.InnerXml;
            ////result = CommonFunctions.XmlSerialize<List<ProductDetails>>(ProdDetails);
            //result = CommonFunctions.XmlSerialize<Prod1>(objProd1);
            //result = t;
            return result;
        }

        private void BindStoreMappingTreeView()
        {
            bool checkAll = true;
            trvStores.Nodes.Clear();
            TreeNode masterNode = new TreeNode(GetLocalResourceObject("Stores").ToString(), "0");
            foreach (var item in StoreMappingHeaderViewState.StoreList)
            {
                TreeNode node = new TreeNode(item.NAME, item.PK.ToString());
                if (item.IS_CHECKED == "true") node.Checked = true;
                else checkAll = false;
                //masterNode.ChildNodes.Add(node);
                trvStores.Nodes.Add(node);
            }
            //if (checkAll) masterNode.Checked = true;
            //else masterNode.Checked = false;
            //trvStores.Nodes.Add(masterNode);
            trvStores.ExpandAll();
        }

        private string CreateStoreMappingXml()
        {
            string resultXml = string.Empty;
            if (StoreMappingHeaderViewState.StoreList == null)
            {
                return resultXml;
            }
            foreach (var item in StoreMappingHeaderViewState.StoreList)
            {
                item.IS_CHECKED = "false";
            }
            foreach (TreeNode node in trvStores.Nodes) // trvStores.Nodes[0].ChildNodes)
            {
                if (node.Checked)
                {
                    int pk = GetNullableInt(node.Value).Value;
                    StoreMapping storeMapping = StoreMappingHeaderViewState.StoreList
                        .Where(x => x.PK == pk)
                        .Single();
                    storeMapping.IS_CHECKED = "true";
                }
            }
            StoreMappingHeader header = new StoreMappingHeader();
            header.StoreList = StoreMappingHeaderViewState.StoreList
                                    .Where(x => x.IS_CHECKED == "true")
                                    .ToList();
            resultXml = CommonFunctions.XmlSerialize<StoreMappingHeader>(header);
            return resultXml;
        }
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            LIST,
            CATEGORY,
            SIZE,
            UOM,
            PRODUCTGROUP,
            MATERIAL,
            EDIT,
            PRODUCTDETAIL,
            PRODUCT,
            PRODUCTSGRID,
            STOREMAPPING,
            VENDORDETAILS,
            CURRENCY,
            VENDORUOM,
            VENDORLISTING
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
        #endregion

        public enum ProductProperties
        {
            Type = 1,
            Thickness = 2,
            Category = 3,
            Surface = 4,
            Shade = 5,
            Classification = 6,
            Size = 7,
            Length = 8,
            Chlorination = 9,
            Side = 12
        }

        [Serializable]
        [XmlRoot("Root")]
        public class ProductDetails
        {
            [XmlElement("FPM_PRODUCT")]
            public int ITM_PK { get; set; }

            public string ITM_TEXT { get; set; }

            [XmlElement("FPM_ACTIVE")]
            public int FPM_ACTIVE
            {
                get
                {
                    return active;
                }
                set
                {
                    active = value;
                }
            }

            private int active = 1;
        }

        [Serializable]
        [XmlRoot("Root")]
        public class Prod1
        {
            [XmlElement("ProdMap")]
            public List<ProductDetails> Prod { get; set; }
        }

        [Serializable]
        [XmlRoot("root")]
        public class StoreMappingHeader
        {
            [XmlElement("StoreDtl")]
            public List<StoreMapping> StoreList { get; set; }
        }

        [Serializable]
        //[XmlRoot("root")]
        public class StoreMapping
        {
            [XmlElement("ITM_DEPT")]
            public int PK { get; set; }
            public string NAME { get; set; }
            public int PARENT { get; set; }
            public bool HAS_CHILD { get; set; }
            public string IS_CHECKED { get; set; }
        }


    }
}