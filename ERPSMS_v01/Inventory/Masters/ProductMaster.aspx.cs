using System;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject.AccountManagement;
using ERPData;
using ERPManager;
using System.Collections.Generic;
using ERPService;
using ERPService.Inventory;
using System.Web;
using BusinessObject.CommonManagement;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using System.Web.UI;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Linq;
using BusinessObject.Inventory;
using BusinessLogic.Inventory;
using System.Text.RegularExpressions;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.Inventory.Masters
{
    public partial class ProductMaster : ERP.Store.UI.MyBasePage
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

        private bool IsBrandProduct
        {
            get { return this.ViewState["IsBrandProduct"] == null ? false : Convert.ToBoolean(this.ViewState["IsBrandProduct"].ToString()); }
            set { this.ViewState["IsBrandProduct"] = value; }
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
        /// Item PK
        /// </summary>
        private int ItemPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.ItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ItemPK] = value;
            }
        }

        /// <summary>
        /// CNG Value
        /// </summary>
        private int CngValue
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CngVal]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CngVal] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private DropDownList DropDownID
        {
            get
            {
                return (DropDownList)this.ViewState[ViewstateStrings.DropDownID];
            }
            set
            {
                this.ViewState[ViewstateStrings.DropDownID] = value;
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
        /// Managing tab visibility
        /// 1: Product Detail Tab
        /// 2: Related Product Tab
        /// </summary>
        private int DisplayTab
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisplayTab] == null ? 1 : (int)(this.ViewState[ViewstateStrings.DisplayTab]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisplayTab] = value;
            }
        }

        /// <summary>
        /// To maintain Product Grades
        /// </summary>
        private DataTable dtProductGrade
        {
            get
            {
                return this.ViewState[ViewstateStrings.ProductGrade] != null ? (DataTable)this.ViewState[ViewstateStrings.ProductGrade] : new DataTable();
            }
            set
            {
                this.ViewState[ViewstateStrings.ProductGrade] = value;
            }
        }

        private DataTable dtSubType
        {
            get
            {
                return this.ViewState[ViewstateStrings.SubType] != null ? (DataTable)this.ViewState[ViewstateStrings.SubType] : new DataTable();
            }
            set
            {
                this.ViewState[ViewstateStrings.SubType] = value;
            }
        }

        /// <summary>
        /// To maintain Related Product List
        /// </summary>
        private List<INV_ITEM_REL_MAP> objInvItemRelMapList
        {
            get
            {
                return this.ViewState[ViewstateStrings.RelatedItems] != null ? (List<INV_ITEM_REL_MAP>)this.ViewState[ViewstateStrings.RelatedItems] : new List<INV_ITEM_REL_MAP>();
            }
            set
            {
                this.ViewState[ViewstateStrings.RelatedItems] = value;
            }
        }

        /// <summary>
        /// To maintain Sub Type Map List
        /// </summary>
        private List<INV_ITEM_SUB_TYPE_MAP> objInvItemSubTypeMapList
        {
            get
            {
                return this.ViewState[ViewstateStrings.SubTypeMapList] != null ? (List<INV_ITEM_SUB_TYPE_MAP>)this.ViewState[ViewstateStrings.SubTypeMapList] : new List<INV_ITEM_SUB_TYPE_MAP>();
            }
            set
            {
                this.ViewState[ViewstateStrings.SubTypeMapList] = value;
            }
        }
        /// <summary>
        /// To maintain Packing Material Map List
        /// </summary>
        private List<INV_ITEM_PACK_ITEM_MAP> objInvItemPackMatMapList
        {
            get
            {
                return this.ViewState[ViewstateStrings.PackMatMapList] != null ? (List<INV_ITEM_PACK_ITEM_MAP>)this.ViewState[ViewstateStrings.PackMatMapList] : new List<INV_ITEM_PACK_ITEM_MAP>();
            }
            set
            {
                this.ViewState[ViewstateStrings.PackMatMapList] = value;
            }
        }
        /// <summary>
        /// Related product Pk      
        /// </summary>
        private int RelProductPk
        {
            get
            {
                return this.ViewState[ViewstateStrings.RelProductPk] == null ? 0 : (int)(this.ViewState[ViewstateStrings.RelProductPk]);
            }
            set
            {
                this.ViewState[ViewstateStrings.RelProductPk] = value;
            }
        }

        private int SubTypeProductPk
        {
            get
            {
                return this.ViewState[ViewstateStrings.SubTypeProductPk] == null ? 0 : (int)(this.ViewState[ViewstateStrings.SubTypeProductPk]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SubTypeProductPk] = value;
            }
        }
        private int PackMatProductPk
        {
            get
            {
                return this.ViewState[ViewstateStrings.PackMatProductPk] == null ? 0 : (int)(this.ViewState[ViewstateStrings.PackMatProductPk]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PackMatProductPk] = value;
            }
        }

        #endregion
        private ActionsEnum commonActions;
        //page related class objects      
        private INV_ITEM_MST invItemMstObj;
        private INV_ITEM_SPEC_DTL invItemSpecDtlObj;
        private PRD_PRODUCT_GRADE_MAP invItemGradeObj;
        private INV_ITEM_PACK_COMB_DTL invItemPackCombObj;
        private INV_UOM_MST invUomMstObj;
        private ADM_CONST_MST admConstMstObj;
        private INV_ITEM_GROUP_MST invItemGroupMstObj;
        private ServiceUtility serviceUtilityObj;
        //List for binding details to controls
        private List<INV_ITEM_MST> invItemMstList;
        private List<INV_ITEM_SPEC_DTL> invItemSpecDtlList;
        private List<PRD_PRODUCT_GRADE_MAP> invItemGradeDtlList;
        private List<INV_ITEM_PACK_COMB_DTL> invItemPackCombList;
        private List<INV_UOM_MST> invUomMstList;
        private List<ADM_CONST_MST> admConstMstList;
        private List<SPADM_CONST_GRP_GET_KV_Result> spAdmConstGrpGetKvResultList;
        private List<SPADM_CONST_GRP_GET_KV_Result> spCombinationsList;
        private List<SPPRD_INV_ITEM_PACK_COMB_GET_Result> spPrdItemPackCombList;
        private List<INV_ITEM_GROUP_MST> invItemGroupMstList;
        private List<INV_ITEM_REL_MAP> objTempInvItemRelMapList;
        private List<INV_ITEM_SUB_TYPE_MAP> objTempInvItemTypeMapList;
        private INV_ITEM_REL_MAP objInvItemRelMap;
        private INV_ITEM_SUB_TYPE_MAP objInvItemSubTypeMap;
        private INV_ITEM_MST objInvItemMst;
        private List<INV_ITEM_REL_MAP> invRelItemMapList;
        private List<INV_ITEM_SUB_TYPE_MAP> invSubTypeItemMapList;
        private List<INV_ITEM_PACK_ITEM_MAP> invPackMatItemMapList;
        private List<INV_ITEM_PACK_ITEM_MAP> objTempInvItemPackItemMapList;
        private INV_ITEM_PACK_ITEM_MAP objInvItemPackItemMap;

        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;

        //FOR Grade Properties
        private DataSet dsGradeProperties;
        private DataTable dtProductCategory;
        private ProductBO productBO;
        private ProductLineBO productLineBO;
        private DataTable dtNoCompounds;
        private DataTable dtBasedOn;
        private DataTable dtPackCombinations;
        private DataTable dtPlanGroup;
        private DataTable dtFiles;
        private DataTable dtGSTclasslist;
        private DataSet dsPackingSpec;
        HiddenField hdfRelPrdtPk;
        HiddenField hdfPackMatPrdtPk;
        DataTable dtStoreMapping;
        // HiddenField hdfProductPK;
        int packingMatPK = 0;
        int PlanGroupPK = 0;
        int PrdCategoryPK = 0;
        public string FileUrl = string.Empty;
        public int HSNpk = 0;

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
            int groupTypeGradePK = Convert.ToInt32(GTIService.Constants.Inventory.Fields.C_GroupGradeTypePK);
            InvItemMstService InvItemMstServiceClient = null;
            try
            {
                InvItemMstServiceClient = new InvItemMstService();
                InvItemMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemMstServiceClient);
                invItemMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_MST>();
                invItemGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_GROUP_MST>();
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        int BizUnit = 0;
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdProductList.PageSize;
                        serviceUtilityObj.FilterBy = serviceUtilityObj.FilterValue = string.Empty;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.ItemCode : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        serviceUtilityObj.NeedAdvanceFilter = chkMapped.Checked;
                        invItemMstObj.ITM_PK = string.IsNullOrEmpty(hdfProductPK.Value) ? 0 : Convert.ToInt32(hdfProductPK.Value);
                        //invItemMstObj.ITM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);  
                        invItemMstObj.ITM_ACTIVE = Convert.ToByte(chkActiveFilter.Checked == true ? Convert.ToByte(DbActiveStatus.ACTIVE) : Convert.ToByte(DbActiveStatus.INACTIVE));
                        if (IsBrandProduct)
                            invItemMstObj.ITM_CATEGORY = 9;
                        else
                            invItemMstObj.ITM_CATEGORY = 2;
                        invItemMstObj.ITM_GROUP = ddlAdvProductGroup.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlAdvProductGroup.SelectedValue) : 0;
                        byte? ProductGrade = null;
                        if (Convert.ToInt32(ddlAdvPrdtGrade.SelectedValue) > 0)
                            ProductGrade = Convert.ToByte(ddlAdvPrdtGrade.SelectedValue);
                        invItemMstObj.ITM_GRADE = ProductGrade;
                        invItemMstObj.ITM_SUB_TYPE = ddlAdvProdSubCategory.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlAdvProdSubCategory.SelectedValue) : 0;
                        invItemMstObj.ITM_BIZUNIT = currentUser.SBUID;

                        int ItemCategoryPrdn = 2;
                        if (IsBrandProduct)
                            ItemCategoryPrdn = Convert.ToInt32(GetLocalResourceObject("BrandProducts"));
                        else
                            ItemCategoryPrdn = Convert.ToInt32(GetLocalResourceObject("ItemCategoryProduction"));
                        if (hdfIsProductInSBU.Value == "1")
                        {
                            BizUnit = currentUser.SBUID;
                        }

                        invItemMstList = InvItemMstServiceClient.GetInvItemMstAdvSearch(invItemMstObj, serviceUtilityObj, ItemCategoryPrdn,Convert.ToInt32(hdfIsProductInSBU.Value), BizUnit);
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    case ControlsEnum.PRODUCT:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        invItemMstObj.ITM_PK = CurrPK;
                        //invItemMstObj.ITM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        invItemMstObj.ITM_ACTIVE = Convert.ToByte(chkActiveFilter.Checked == true ? Convert.ToByte(DbActiveStatus.ACTIVE) : Convert.ToByte(DbActiveStatus.INACTIVE));
                        invItemMstList = InvItemMstServiceClient.GetInvItemMst(invItemMstObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.CONTROLS:
                        spAdmConstGrpGetKvResultList = InvItemMstServiceClient.GetControlsList(null, null, (int)ConstGroupType.Product, null, Convert.ToByte(DbActiveStatus.ACTIVE), null, null);
                        break;
                    case ControlsEnum.UOM:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        invUomMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_UOM_MST>();
                        invUomMstObj.UOM_PK = 0;
                        invUomMstObj.UOM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        invUomMstList = InvItemMstServiceClient.GetUomMst(invUomMstObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.BINDDROPDOWN:
                        CommonServiceClient = new CommonService();
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, Convert.ToByte(DbActiveStatus.ACTIVE), null, (int)ConstGroupType.Product, CngValue, null);
                        break;
                    case ControlsEnum.PRODUCTGROUP:
                        invItemGroupMstObj.IGM_PK = 0;
                        invItemGroupMstObj.IGM_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        invItemGroupMstList = InvItemMstServiceClient.GetProductGroups(invItemGroupMstObj);
                        break;
                    case ControlsEnum.GRADEPROPERTY:
                        dsGradeProperties = BusinessLogic.Inventory.ProductsBL.GetPropertyGradeXML(groupTypeGradePK, CurrPK);//Gets the Grades to bind datalist
                        break;
                    case ControlsEnum.PRODUCTGRADES:
                        dtProductGrade = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PRODUCT GRADE");
                        break;
                    case ControlsEnum.SUBGRADEPRODUCTS:
                        objInvItemRelMapList = InvItemMstServiceClient.GetRelatedItems(CurrPK);
                        break;
                    case ControlsEnum.RELATEDPRODUCTS:
                        objInvItemSubTypeMapList = InvItemMstServiceClient.GetSubTypeItems(CurrPK);
                        break;
                    case ControlsEnum.PRODUCTBYPK:
                        int productPk = 0;
                        int.TryParse(hdfRelItemPk.Value, out productPk);
                        objInvItemMst = InvItemMstServiceClient.GetInvItemMst(productPk);
                        break;
                    case ControlsEnum.SUBTYPE:
                        dtSubType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PRODUCT SUB TYPE");
                        break;
                    case ControlsEnum.BASEDONCOMBINATIONS:
                        spCombinationsList = InvItemMstServiceClient.GetControlsList(null, null, (int)ConstGroupType.Product, null, Convert.ToByte(DbActiveStatus.ACTIVE), null, "PCKCOMB");
                        dtBasedOn = LINQToDataTable(spCombinationsList);
                        break;
                    case ControlsEnum.PACKINGCOMBINATIONS:
                        spPrdItemPackCombList = InvItemMstServiceClient.GetPrdPackingCombinations(currentUser.SBUID, string.IsNullOrEmpty(ddlCombinationBasedOn.SelectedValue) ? 0 : Convert.ToInt32(ddlCombinationBasedOn.SelectedValue), CurrPK);
                        dtPackCombinations = LINQToDataTable(spPrdItemPackCombList);
                        break;
                    case ControlsEnum.PACKINGMATERIALS:
                        objInvItemPackMatMapList = InvItemMstServiceClient.GetPackMatItems(CurrPK);
                        break;
                    case ControlsEnum.PACKINGMATERIALBYPK:
                        objInvItemMst = InvItemMstServiceClient.GetInvItemMst(packingMatPK);
                        break;
                    case ControlsEnum.PLANGROUP:
                        dtPlanGroup = BusinessLogic.Inventory.ProductsBL.GetProductPlanGroups(PlanGroupPK, Convert.ToByte(DbActiveStatus.ACTIVE), currentUser.SBUID);
                        break;
                    case ControlsEnum.PRODUCTCATEGORY:
                        if (IsBrandProduct)
                            dtProductCategory = BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetProductCategory(PrdCategoryPK, Convert.ToByte(DbActiveStatus.ACTIVE),currentUser.SBUID, 9);
                        else
                            dtProductCategory = BusinessLogic.MaterialManagement.MaterialCategoryMaster.GetProductCategory(PrdCategoryPK, Convert.ToByte(DbActiveStatus.ACTIVE), currentUser.SBUID);
                        break;
                    case ControlsEnum.LINES:
                        productLineBO = BusinessLogic.Inventory.ProductsBL.GetProductLines(CurrPK);
                        break;
                    case ControlsEnum.STOREMAPPING:
                        dtStoreMapping = BusinessLogic.Inventory.FormerMasterBL.GetMaterialStores(Convert.ToInt32(CurrPK), currentUser.SBUID);
                        break;
                    case ControlsEnum.PRODUCTDOC:
                        dtFiles = null;
                        dtFiles = BusinessLogic.Inventory.ProductsBL.GetProductGroupDoc(CurrPK, 25);
                        break;
                    case ControlsEnum.GSTCLASS:
                        dtGSTclasslist = BusinessLogic.Inventory.ProductsBL.GetGSTclassificationList(HSNpk, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.CurrentSBUPK);
                        break;
                    case ControlsEnum.PACKINGSPEC:
                        dsPackingSpec = BusinessLogic.Inventory.PackingMasterBL.GetPackingMaster(0, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.CurrentSBUPK, string.Empty, string.Empty, string.Empty);
                        break;

                }
                if (admConstMstList != null && admConstMstList.Count > 0)
                {
                    foreach (ADM_CONST_MST admConstMstObj in admConstMstList)
                    {
                        admConstMstObj.CON_NAME = admConstMstObj.CON_CODE + "-" + admConstMstObj.CON_NAME;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                invItemMstObj = null;
                serviceUtilityObj = null;
                InvItemMstServiceClient = null;
                invItemGroupMstObj = null;
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
                    case ControlsEnum.PACKINGSPEC:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.PRODUCT:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        break;
                    case ControlsEnum.CONTROLS:
                        LoadControls();
                        break;
                    case ControlsEnum.UOM:
                        BindUOMDropDown();
                        break;
                    case ControlsEnum.BINDDROPDOWN:
                        BindDropDown(DropDownID);
                        break;
                    case ControlsEnum.PRODUCTGROUP:
                        BindProductGroupDropDown();
                        break;
                    case ControlsEnum.GRADEPROPERTY:
                        BindGradeDatalist();
                        break;
                    case ControlsEnum.PRODUCTGRADES:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SUBGRADEPRODUCTS:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.RELATEDPRODUCTS:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.MASTERPRODUCTGRADES:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SUBTYPE:
                        BindSubTypeDropDown();
                        break;
                    case ControlsEnum.PRODUCTSUBCATEGORY:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.BASEDONCOMBINATIONS:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.PACKINGCOMBINATIONS:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.PACKINGMATERIALS:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.PLANGROUP:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.PRODUCTCATEGORY:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.LINES:
                    case ControlsEnum.STOREMAPPING:
                        BindTreeView(controlType);
                        break;
                    case ControlsEnum.PRODUCTDOC:
                        if (dtFiles != null && dtFiles.Rows.Count > 0)
                        {
                            //Response.Write("<script>window.open('" + dtFiles.Rows[0]["DOC_PATH"].ToString() + "','_blank')</script>");
                            Response.Redirect(dtFiles.Rows[0]["DOC_PATH"].ToString());
                        }
                        break;
                    case ControlsEnum.GSTCLASS:
                        BindDropDownList(ControlsEnum.GSTCLASS);
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
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private INV_ITEM_MST SetUIValuesToObject()
        {
            CheckBox chkGrade, chkProduct;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                invItemMstObj.ITM_PK = CurrPK;
                invItemMstObj.ITM_CODE = HttpUtility.HtmlEncode(txtProductCode.Text.Trim());
                invItemMstObj.ITM_NAME = HttpUtility.HtmlEncode(txtProductName.Text.Trim());
                invItemMstObj.ITM_DESC = HttpUtility.HtmlEncode(txtProductDesc.Text.Trim());
                invItemMstObj.ITM_REF1 = HttpUtility.HtmlEncode(txtFirstRef.Text.Trim());
                invItemMstObj.ITM_REF2 = HttpUtility.HtmlEncode(txtSecRef.Text.Trim());
                invItemMstObj.ITM_WEIGHT = txtWeight.Text == string.Empty ? 0 : Convert.ToDouble(txtWeight.Text);
                invItemMstObj.ITM_MAX_WEIGHT = txtMaxWeight.Text == string.Empty ? 0 : Convert.ToDouble(txtMaxWeight.Text);
                invItemMstObj.ITM_MIN_WEIGHT = txtMinWeight.Text == string.Empty ? 0 : Convert.ToDouble(txtMinWeight.Text);
                invItemMstObj.ITM_UOM = Convert.ToInt32(ddlUOM.SelectedValue);
                invItemMstObj.ITM_INTER_STATE = string.IsNullOrEmpty(txtInterState.Text.Trim()) ? 0 : Convert.ToDouble(txtInterState.Text);
                invItemMstObj.ITM_INTRA_STATE = string.IsNullOrEmpty(txtIntraState.Text.Trim()) ? 0 : Convert.ToDouble(txtIntraState.Text);
                invItemMstObj.ITM_OTHERS = string.IsNullOrEmpty(txtOthers.Text.Trim()) ? 0 : Convert.ToDouble(txtOthers.Text);
                invItemMstObj.ITM_EXPORT = string.IsNullOrEmpty(txtExport.Text.Trim()) ? 0 : Convert.ToDouble(txtExport.Text);
                invItemMstObj.ITM_CATEGORY = Convert.ToInt32(ddlPrdCategory.SelectedValue);
                //if (ddlProductGroup.SelectedValue == CommonConstants.SELECTVAL)
                //{
                //    invItemMstObj.ITM_GROUP = null;
                //}
                //else
                //{
                //    invItemMstObj.ITM_GROUP = Convert.ToInt32(ddlProductGroup.SelectedValue);
                //}
                //product group
                if (Convert.ToInt32(hdfProductGroupPk.Value) > 0)
                {
                    invItemMstObj.ITM_GROUP = Convert.ToInt32(hdfProductGroupPk.Value);
                }
                else
                {
                    invItemMstObj.ITM_GROUP = null;
                }
                //Packing Spec
                if (ddlPackingSpec.Items.Count == 0 || ddlPackingSpec.SelectedValue == CommonConstants.SELECTVAL)
                {
                    invItemMstObj.ITM_PACK_SPEC = null;
                }
                else
                {
                    invItemMstObj.ITM_PACK_SPEC = Convert.ToInt32(ddlPackingSpec.SelectedValue);
                }

                if (Convert.ToInt32(hdfInventoryProdPK.Value) > 0)
                {
                    invItemMstObj.ITM_LINKED_ITEM = Convert.ToInt32(hdfInventoryProdPK.Value);
                }
                else
                {
                    invItemMstObj.ITM_LINKED_ITEM = null;
                }
                invItemMstObj.ITM_ACTIVE = chkActive.Checked == true ? Convert.ToByte(DbActiveStatus.ACTIVE) : Convert.ToByte(DbActiveStatus.INACTIVE);
                invItemMstObj.ITM_BIZUNIT = currentUser.SBUID;
                invItemMstObj.ITM_CRTD_BY = Convert.ToInt32(currentUser.PKUser);
                invItemMstObj.ITM_CRTD_DT = DateTime.Now;
                invItemMstObj.ITM_MOD_BY = Convert.ToInt32(currentUser.PKUser);
                invItemMstObj.ITM_MOD_DT = LastModifiedTime;
                invItemMstObj.ITM_REF3 = HttpUtility.HtmlEncode(txtThirdRef.Text.Trim());
                invItemMstObj.ITM_REF4 = HttpUtility.HtmlEncode(txtFourthRef.Text.Trim());
                invItemMstObj.ITM_NO_OF_COMP = byte.Parse(ddlNoCompounds.SelectedItem.Text);
                if (ddlGSTClass.SelectedValue != CommonConstants.SELECTVAL)
                {
                    invItemMstObj.ITM_GST_CLASS = Convert.ToInt32(ddlGSTClass.SelectedValue);
                }
                if (ddlCombinationBasedOn.SelectedValue == CommonConstants.SELECTVAL)
                {
                    invItemMstObj.ITM_PACK_COMB_BSD_ON = null;
                }
                else
                {
                    invItemMstObj.ITM_PACK_COMB_BSD_ON = Convert.ToInt32(ddlCombinationBasedOn.SelectedValue);
                }
                byte? ProductGrade = null;
                if (Convert.ToInt32(ddlPrdtGrade.SelectedValue) > 0)
                    ProductGrade = Convert.ToByte(ddlPrdtGrade.SelectedValue);
                invItemMstObj.ITM_GRADE = ProductGrade;

                int subType = 0;
                if (Convert.ToInt32(ddlProdSubCategory.SelectedValue) > 0)
                    subType = Convert.ToByte(ddlProdSubCategory.SelectedValue);
                invItemMstObj.ITM_SUB_TYPE = subType;

                int PlanGroup = 0;
                if (ddlPlanGroup.Items.Count > 0 && Convert.ToInt32(ddlPlanGroup.SelectedValue) > 0)
                {
                    PlanGroup = Convert.ToInt32(ddlPlanGroup.SelectedValue);
                    invItemMstObj.ITM_PLAN_GROUP = PlanGroup;
                }
                else
                    invItemMstObj.ITM_PLAN_GROUP = null;


                int PrdCategoryPK = 0;
                if (ddlPrdCategory.Items.Count > 0 && Convert.ToInt32(ddlPrdCategory.SelectedValue) > 0)
                {
                    PrdCategoryPK = Convert.ToInt32(ddlPrdCategory.SelectedValue);
                    invItemMstObj.ITM_CATEGORY = PrdCategoryPK;
                }


                invItemSpecDtlObj = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_SPEC_DTL>();
                invItemSpecDtlObj.ISD_PK = ItemPK;
                invItemSpecDtlObj.ISD_ITEM = CurrPK;
                invItemSpecDtlObj.ISD_AGRADE_PER = txtGradeRealisation.Text == string.Empty ? 100 : Convert.ToDouble(txtGradeRealisation.Text.Trim());
                Type objInvItemSpecDtl = invItemSpecDtlObj.GetType();
                for (int i = 1; i <= Convert.ToInt32(hdfDdlCount.Value); i += 2)
                {
                    HiddenField hdf = (HiddenField)tdCol1.FindControl("hdfddl" + i.ToString());
                    DropDownList ddl = (DropDownList)tdCol1.FindControl("ddl" + i.ToString());
                    foreach (PropertyInfo p in objInvItemSpecDtl.GetProperties())
                    {
                        if (p.Name == hdf.Value)
                        {
                            p.SetValue(invItemSpecDtlObj, Convert.ToInt32(ddl.SelectedValue), null);
                        }
                    }
                }
                for (int i = 2; i <= Convert.ToInt32(hdfDdlCount.Value); i += 2)
                {
                    HiddenField hdf = (HiddenField)tdCol2.FindControl("hdfddl" + i.ToString());
                    DropDownList ddl = (DropDownList)tdCol2.FindControl("ddl" + i.ToString());
                    foreach (PropertyInfo p in objInvItemSpecDtl.GetProperties())
                    {
                        if (p.Name == hdf.Value)
                        {
                            p.SetValue(invItemSpecDtlObj, Convert.ToInt32(ddl.SelectedValue), null);
                        }
                    }
                }
                invItemSpecDtlObj.ISD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                invItemSpecDtlObj.ISD_MOD_BY = Convert.ToInt32(currentUser.PKUser);
                invItemSpecDtlObj.ISD_MOD_DT = LastModifiedTime;

                invItemMstObj.INV_ITEM_SPEC_DTL.Add(invItemSpecDtlObj);
                //Grades
                invItemGradeDtlList = new List<PRD_PRODUCT_GRADE_MAP>();
                //Type objinvItemGrade = invItemGradeObj.GetType();
                foreach (DataListItem item in dlGradeProperties.Items)
                {
                    chkGrade = (CheckBox)item.FindControl("chkGrade");
                    if (chkGrade.Checked)
                    {
                        invItemGradeObj = ERP.Utilities.CommonFunctions.Initilize<PRD_PRODUCT_GRADE_MAP>();
                        invItemGradeObj.PGM_PRODUCT = ItemPK;
                        invItemGradeObj.PGM_CONST = Convert.ToInt32(dlGradeProperties.DataKeys[item.ItemIndex]);
                        invItemGradeDtlList.Add(invItemGradeObj);
                    }
                }
                invItemGradeDtlList.ForEach(dtl => invItemMstObj.PRD_PRODUCT_GRADE_MAP.Add(dtl));

                #region Group Mapping for production Planning
                productBO = new ProductBO();
                productBO.DetailList = new List<DetailsBO>();
                DetailsBO detail = new DetailsBO();
                if (invItemMstObj.ITM_GROUP != null)
                    detail.ProductGroupPK = invItemMstObj.ITM_GROUP;
                detail.ProductPK = CurrPK;
                detail.Active = Convert.ToByte(DbActiveStatus.ACTIVE);
                productBO.DetailList.Add(detail);

                #endregion

                //Packing Combinations
                invItemPackCombList = new List<INV_ITEM_PACK_COMB_DTL>();
                HiddenField hdfPackCombPK;
                HiddenField hdfAttributePK;
                TextBox txtCount;
                foreach (GridViewRow grvRow in gdPackingCombination.Rows)
                {
                    hdfPackCombPK = (HiddenField)grvRow.FindControl("hdfPackCombPK");
                    hdfAttributePK = (HiddenField)grvRow.FindControl("hdfAttributePK");
                    txtCount = (TextBox)grvRow.FindControl("txtCount");
                    invItemPackCombObj = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_PACK_COMB_DTL>();
                    invItemPackCombObj.IPC_PK = Convert.ToInt32(hdfPackCombPK.Value);
                    invItemPackCombObj.IPC_ITEM = CurrPK;
                    invItemPackCombObj.IPC_PROPERTY = Convert.ToInt32(hdfAttributePK.Value);
                    invItemPackCombObj.IPC_PROPERTY_COUNT = string.IsNullOrEmpty(txtCount.Text) ? 0 : Convert.ToInt32(txtCount.Text);
                    invItemPackCombList.Add(invItemPackCombObj);
                }
                invItemPackCombList.ForEach(dtl => invItemMstObj.INV_ITEM_PACK_COMB_DTL.Add(dtl));
                return invItemMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                invItemMstObj = null;
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {

                    #region GRADE PRODUCTS ADD
                    case ControlsEnum.GRADEPRODUCTSADD:
                        retObject = objTempInvItemRelMapList;
                        break;
                    #endregion

                    #region SAVE GRADE PRODUCTS
                    case ControlsEnum.SAVEGRADEPRODUCTS:
                        List<INV_ITEM_REL_MAP> objRelItems = new List<INV_ITEM_REL_MAP>();
                        INV_ITEM_REL_MAP objItemDet;
                        if (objInvItemRelMapList != null && objInvItemRelMapList.Count > 0)
                        {
                            foreach (INV_ITEM_REL_MAP objItem in objInvItemRelMapList)
                            {
                                objItemDet = new INV_ITEM_REL_MAP();
                                objItemDet.IMR_CRTD_BY = objItem.IMR_CRTD_BY;
                                objItemDet.IMR_CRTD_DT = objItem.IMR_CRTD_DT;
                                objItemDet.IMR_ITEM = objItem.IMR_ITEM;
                                objItemDet.IMR_ITEM_GRADE = objItem.IMR_ITEM_GRADE;
                                objItemDet.IMR_MOD_BY = objItem.IMR_MOD_BY;
                                objItemDet.IMR_MOD_DT = objItem.IMR_MOD_DT;
                                objItemDet.IMR_PK = objItem.IMR_PK;
                                objItemDet.IMR_REL_ITEM = objItem.IMR_REL_ITEM;
                                objRelItems.Add(objItemDet);
                            }
                        }
                        retObject = objRelItems;
                        break;
                    #endregion

                    #region RELATED PRODUCTS ADD
                    case ControlsEnum.RELATEDPRODUCTSADD:
                        retObject = objTempInvItemTypeMapList;
                        break;
                    #endregion

                    #region SAVE RELATED PRODUCTS
                    case ControlsEnum.SAVERELPRODUCTS:
                        List<INV_ITEM_SUB_TYPE_MAP> objTypeItems = new List<INV_ITEM_SUB_TYPE_MAP>();
                        INV_ITEM_SUB_TYPE_MAP objTypeItemDet;
                        if (objInvItemSubTypeMapList != null && objInvItemSubTypeMapList.Count > 0)
                        {
                            foreach (INV_ITEM_SUB_TYPE_MAP objItem in objInvItemSubTypeMapList)
                            {
                                objTypeItemDet = new INV_ITEM_SUB_TYPE_MAP();
                                objTypeItemDet.ISM_CRTD_BY = objItem.ISM_CRTD_BY;
                                objTypeItemDet.ISM_CRTD_DT = objItem.ISM_CRTD_DT;
                                objTypeItemDet.ISM_ITEM = objItem.ISM_ITEM;
                                objTypeItemDet.ISM_ITEM_SUB_TYPE = objItem.ISM_ITEM_SUB_TYPE;
                                objTypeItemDet.ISM_MOD_BY = objItem.ISM_MOD_BY;
                                objTypeItemDet.ISM_MOD_DT = objItem.ISM_MOD_DT;
                                objTypeItemDet.ISM_PK = objItem.ISM_PK;
                                objTypeItemDet.ISM_REL_ITEM = objItem.ISM_REL_ITEM;
                                objTypeItems.Add(objTypeItemDet);
                            }
                        }
                        retObject = objTypeItems;
                        break;
                    #endregion
                    #region PACKING MATERIALS
                    case ControlsEnum.SAVEPACKMATERIALMAP:
                        List<INV_ITEM_PACK_ITEM_MAP> objPackMatList = new List<INV_ITEM_PACK_ITEM_MAP>();
                        INV_ITEM_PACK_ITEM_MAP objPackItemDet;
                        if (objInvItemPackMatMapList != null && objInvItemPackMatMapList.Count > 0)
                        {
                            foreach (INV_ITEM_PACK_ITEM_MAP objItem in objInvItemPackMatMapList)
                            {
                                objPackItemDet = new INV_ITEM_PACK_ITEM_MAP();
                                objPackItemDet.IMP_CRTD_BY = objItem.IMP_CRTD_BY;
                                objPackItemDet.IMP_CRTD_DT = objItem.IMP_CRTD_DT;
                                objPackItemDet.IMP_ITEM = objItem.IMP_ITEM;
                                objPackItemDet.IMP_QUANITY = objItem.IMP_QUANITY;
                                objPackItemDet.IMP_MOD_BY = objItem.IMP_MOD_BY;
                                objPackItemDet.IMP_MOD_DT = objItem.IMP_MOD_DT;
                                objPackItemDet.IMP_PK = objItem.IMP_PK;
                                objPackItemDet.IMP_PACK_ITEM = objItem.IMP_PACK_ITEM;
                                objPackMatList.Add(objPackItemDet);
                            }
                        }
                        retObject = objPackMatList;
                        break;
                    #endregion

                    #region LINES
                    case ControlsEnum.LINES:
                        ProductLineSaveBO oblProdLineSave = new ProductLineSaveBO();
                        List<ProductLineSaveDtlsBO> lstProdLine = new List<ProductLineSaveDtlsBO>();
                        foreach (TreeNode node in trvLine.Nodes) //Root
                            foreach (TreeNode child in node.ChildNodes) //Child 
                            {
                                if (child.Checked)
                                {
                                    ProductLineSaveDtlsBO objData = new ProductLineSaveDtlsBO();
                                    objData.PLM_LINE = Convert.ToInt32(child.Value);
                                    lstProdLine.Add(objData);
                                }
                            }
                        oblProdLineSave.PLM_PRODUCT = CurrPK;
                        oblProdLineSave.Lines = lstProdLine;
                        retObject = oblProdLineSave;
                        break;
                    #endregion
                    #region STOREMAPPING
                    case ControlsEnum.STOREMAPPING:
                        ProductStoreSaveBO oblProdStoreSave = new ProductStoreSaveBO();
                        List<ProductStoreSaveDtlsBO> lstProdStore = new List<ProductStoreSaveDtlsBO>();
                        foreach (TreeNode node in trvStore.Nodes)
                        {
                            if(node.Checked)
                            {
                                ProductStoreSaveDtlsBO objData = new ProductStoreSaveDtlsBO();
                                objData.STORE_PK = Convert.ToInt32(node.Value);
                                lstProdStore.Add(objData);
                            }
                        }
                        oblProdStoreSave.PSM_PRODUCT = CurrPK;
                        oblProdStoreSave.UserPk = Convert.ToInt32(currentUser.PKUser);
                        oblProdStoreSave.Bizunit = Convert.ToInt32(currentUser.SBUID);
                        oblProdStoreSave.Stores = lstProdStore;
                        retObject = oblProdStoreSave;
                        break;
                        #endregion
                }
                return retObject;
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            try
            {
                //assigning the UI controls with the corresponding ListObject value AsrSovMstList
                if (invItemMstList != null && invItemMstList.Count > 0)
                {
                    CurrPK = invItemMstList[0].ITM_PK;
                    txtProductCode.Text = HttpUtility.HtmlDecode(invItemMstList[0].ITM_CODE);
                    txtProductName.Text = HttpUtility.HtmlDecode(invItemMstList[0].ITM_NAME);
                    ddlUOM.SelectedIndex = Convert.ToInt32(ddlUOM.Items.IndexOf(ddlUOM.Items.FindByValue(invItemMstList[0].ITM_UOM.ToString())));
                    txtWeight.Text = invItemMstList[0].ITM_WEIGHT.ToString();
                    txtMinWeight.Text = invItemMstList[0].ITM_MIN_WEIGHT.ToString();

                    txtInterState.Text = invItemMstList[0].ITM_INTER_STATE.HasValue ? invItemMstList[0].ITM_INTER_STATE.ToString() : string.Empty;
                    txtIntraState.Text = invItemMstList[0].ITM_INTRA_STATE.HasValue ? invItemMstList[0].ITM_INTRA_STATE.ToString() : string.Empty;
                    txtOthers.Text = invItemMstList[0].ITM_OTHERS.HasValue ? invItemMstList[0].ITM_OTHERS.ToString() : string.Empty;
                    txtExport.Text = invItemMstList[0].ITM_EXPORT.HasValue ? invItemMstList[0].ITM_EXPORT.ToString() : string.Empty;

                    txtMaxWeight.Text = invItemMstList[0].ITM_MAX_WEIGHT.ToString();
                    chkActive.Checked = invItemMstList[0].ITM_ACTIVE == 1 ? true : false;
                    txtFirstRef.Text = string.IsNullOrEmpty(invItemMstList[0].ITM_REF1) ? string.Empty : invItemMstList[0].ITM_REF1;
                    txtSecRef.Text = string.IsNullOrEmpty(invItemMstList[0].ITM_REF2) ? string.Empty : invItemMstList[0].ITM_REF2;
                    txtThirdRef.Text = string.IsNullOrEmpty(invItemMstList[0].ITM_REF3) ? string.Empty : invItemMstList[0].ITM_REF3;
                    txtFourthRef.Text = string.IsNullOrEmpty(invItemMstList[0].ITM_REF4) ? string.Empty : invItemMstList[0].ITM_REF4;
                    ddlNoCompounds.SelectedIndex = Convert.ToInt32(ddlNoCompounds.Items.IndexOf(ddlNoCompounds.Items.FindByValue(invItemMstList[0].ITM_NO_OF_COMP.ToString())));
                    ddlPrdtGrade.SelectedValue = invItemMstList[0].ITM_GRADE.HasValue ? invItemMstList[0].ITM_GRADE.ToString() : CommonConstants.SELECTVAL;
                    PlanGroupPK = invItemMstList[0].ITM_PLAN_GROUP.HasValue ? Convert.ToInt32(invItemMstList[0].ITM_PLAN_GROUP) : 0;
                    GetFieldValues(ControlsEnum.PLANGROUP);
                    SetFieldValues(ControlsEnum.PLANGROUP);
                    ddlPlanGroup.SelectedValue = invItemMstList[0].ITM_PLAN_GROUP.HasValue ? invItemMstList[0].ITM_PLAN_GROUP.ToString() : CommonConstants.SELECTVAL;
                    HSNpk = invItemMstList[0].ITM_GST_CLASS != null && invItemMstList[0].ITM_GST_CLASS.ToString() != string.Empty ? Convert.ToInt32(invItemMstList[0].ITM_GST_CLASS) : 0;
                    GetFieldValues(ControlsEnum.GSTCLASS);
                    SetFieldValues(ControlsEnum.GSTCLASS);
                    ddlGSTClass.SelectedIndex = ddlGSTClass.Items.IndexOf(ddlGSTClass.Items.FindByValue(HSNpk.ToString()));
                    PrdCategoryPK = Convert.ToInt32(invItemMstList[0].ITM_CATEGORY);
                    GetFieldValues(ControlsEnum.PRODUCTCATEGORY);
                    SetFieldValues(ControlsEnum.PRODUCTCATEGORY);
                    ddlPrdCategory.SelectedValue = invItemMstList[0].ITM_CATEGORY.ToString();
                    //Packing Spec
                    ddlPackingSpec.SelectedIndex = Convert.ToInt32(ddlPackingSpec.Items.IndexOf(ddlPackingSpec.Items.FindByValue(invItemMstList[0].ITM_PACK_SPEC.ToString())));
                    ddlCombinationBasedOn.SelectedValue = invItemMstList[0].ITM_PACK_COMB_BSD_ON.HasValue ? invItemMstList[0].ITM_PACK_COMB_BSD_ON.ToString() : CommonConstants.SELECTVAL;
                    ddlProdSubCategory.SelectedValue = Convert.ToInt32(invItemMstList[0].ITM_SUB_TYPE) > 0 ? invItemMstList[0].ITM_SUB_TYPE.ToString() : CommonConstants.SELECTVAL;
                    if (invItemMstList[0].CRM_CUST_ITEM_MAP.Count > 0)
                    {
                        ddlUOM.Enabled = false;
                        //Enable/Disable Weight  based on Config.
                        if (GetGlobalResourceObject("ConfigurationsRes", "IsModifyWeight").ToString() == "0")//No need to modify weight.Hence it must be disabled.
                        {
                            txtWeight.Enabled = false;
                            txtWeight.CssClass = "input-small numeric input-disabled";
                            txtMinWeight.Enabled = false;
                            txtMinWeight.CssClass = "input-small numeric input-disabled";
                            txtMaxWeight.Enabled = false;
                            txtMaxWeight.CssClass = "input-small numeric input-disabled";
                        }
                    }
                    else
                    {
                        ddlUOM.Enabled = true;
                        txtWeight.Enabled = true;
                        txtWeight.CssClass = "input-small numeric";
                        txtMinWeight.Enabled = true;
                        txtMinWeight.CssClass = "input-small numeric";
                        txtMaxWeight.Enabled = true;
                        txtMaxWeight.CssClass = "input-small numeric";
                    }

                    //ddlProductGroup.SelectedValue = invItemMstList[0].ITM_GROUP != null ? invItemMstList[0].ITM_GROUP.ToString() : CommonConstants.SELECTVAL;
                    hdfProductGroupPk.Value = invItemMstList[0].ITM_GROUP != null ? invItemMstList[0].ITM_GROUP.ToString() : CommonConstants.SELECT_VALUE_ZERO;
                    txtProductGroup.Text = invItemMstList[0].INV_ITEM_GROUP_MST != null ? invItemMstList[0].INV_ITEM_GROUP_MST.IGM_NAME : GetLocalResourceObject("TypeMin3").ToString();
                    hdfInventoryProdPK.Value = invItemMstList[0].ITM_LINKED_ITEM != null ? invItemMstList[0].ITM_LINKED_ITEM.ToString() : CommonConstants.SELECT_VALUE_ZERO;
                    txtInventoryProd.Text = invItemMstList[0].ITM_LINKED_ITEM != null ? "(" + invItemMstList[0].INV_ITEM_MST2.ITM_CODE.ToString() + ") " + invItemMstList[0].INV_ITEM_MST2.ITM_NAME.ToString() : Resources.Messages.AutoDefaultValue;
                    txtProductDesc.Text = HttpUtility.HtmlDecode(invItemMstList[0].ITM_DESC);

                    if (invItemMstList[0].INV_ITEM_SPEC_DTL.Count > 0)
                    {
                        txtGradeRealisation.Text = Convert.ToString(invItemMstList[0].INV_ITEM_SPEC_DTL.First().ISD_AGRADE_PER ?? 0);
                        foreach (INV_ITEM_SPEC_DTL invItemSpecDtl in invItemMstList[0].INV_ITEM_SPEC_DTL)
                        {
                            ItemPK = invItemSpecDtl.ISD_PK;
                            Type objInvItemSpecDtl = invItemSpecDtl.GetType();
                            for (int i = 1; i <= Convert.ToInt32(hdfDdlCount.Value); i += 2)
                            {
                                HiddenField hdf = (HiddenField)tdCol1.FindControl("hdfddl" + i.ToString());
                                DropDownList ddl = (DropDownList)tdCol1.FindControl("ddl" + i.ToString());
                                foreach (PropertyInfo p in objInvItemSpecDtl.GetProperties())
                                {
                                    if (p.Name == hdf.Value)
                                    {
                                        //ddl.SelectedValue = p.GetValue(invItemSpecDtl, null) == null ? CommonConstants.SELECTVAL : p.GetValue(invItemSpecDtl, null).ToString();
                                        string itemval = p.GetValue(invItemSpecDtl, null) == null ? CommonConstants.SELECTVAL : p.GetValue(invItemSpecDtl, null).ToString();
                                        ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(itemval.ToString()));
                                    }
                                }
                            }
                            for (int i = 2; i <= Convert.ToInt32(hdfDdlCount.Value); i += 2)
                            {
                                HiddenField hdf = (HiddenField)tdCol2.FindControl("hdfddl" + i.ToString());
                                DropDownList ddl = (DropDownList)tdCol2.FindControl("ddl" + i.ToString());
                                foreach (PropertyInfo p in objInvItemSpecDtl.GetProperties())
                                {
                                    if (p.Name == hdf.Value)
                                    {
                                        ddl.SelectedValue = p.GetValue(invItemSpecDtl, null) == null ? CommonConstants.SELECTVAL : p.GetValue(invItemSpecDtl, null).ToString();
                                    }
                                }
                            }
                        }
                    }

                    LastModifiedTime = invItemMstList[0].ITM_MOD_DT;
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                }
                //Concurrency Account details Deleted By Another User
                else
                {
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Product);
                    EntryStatus = EntryStatus.LISTMODE;
                    ResetForm();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    ModifiedDatePnl.Visible = false;
                    throw new Exception(litErrorMsg.Text);
                }
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
                    case ControlsEnum.SELECTEDRELITEM:
                        if (RelProductPk > 0 && objInvItemRelMapList != null)
                        {
                            objInvItemRelMap = objInvItemRelMapList.SingleOrDefault(r => r.IMR_REL_ITEM == RelProductPk);
                            if (objInvItemRelMap != null)
                            {
                                txtItem.Text = HttpUtility.HtmlDecode(objInvItemRelMap.INV_ITEM_MST1.ITM_NAME);
                                hdfItemID.Value = objInvItemRelMap.IMR_REL_ITEM.ToString();
                                ddlProductGrade.SelectedValue = objInvItemRelMap.IMR_ITEM_GRADE.ToString();
                            }
                        }
                        break;
                    case ControlsEnum.REMOVEITEM:
                        if (RelProductPk > 0 && objInvItemRelMapList != null)
                        {
                            objInvItemRelMap = objInvItemRelMapList.SingleOrDefault(r => r.IMR_REL_ITEM == RelProductPk);
                            if (objInvItemRelMap != null)
                            {
                                objInvItemRelMapList.Remove(objInvItemRelMap);
                            }
                        }
                        break;
                    case ControlsEnum.SELECTEDSUBTYPEITEM:
                        if (SubTypeProductPk > 0 && objInvItemSubTypeMapList != null)
                        {
                            objInvItemSubTypeMap = objInvItemSubTypeMapList.SingleOrDefault(r => r.ISM_REL_ITEM == SubTypeProductPk);
                            if (objInvItemSubTypeMap != null)
                            {
                                txtRelatedItem.Text = HttpUtility.HtmlDecode(objInvItemSubTypeMap.INV_ITEM_MST1.ITM_NAME);
                                hdfRelatedItem.Value = objInvItemSubTypeMap.ISM_REL_ITEM.ToString();
                                ddlSubType.SelectedValue = objInvItemSubTypeMap.ISM_ITEM_SUB_TYPE.ToString();
                            }
                        }
                        break;
                    case ControlsEnum.REMOVESUBTYPEITEM:
                        if (SubTypeProductPk > 0 && objInvItemSubTypeMapList != null)
                        {
                            objInvItemSubTypeMap = objInvItemSubTypeMapList.SingleOrDefault(r => r.ISM_REL_ITEM == SubTypeProductPk);
                            if (objInvItemSubTypeMap != null)
                            {
                                objInvItemSubTypeMapList.Remove(objInvItemSubTypeMap);
                            }
                        }
                        break;
                    case ControlsEnum.SELECTEDPACKMATITEM:
                        if (PackMatProductPk > 0 && objInvItemPackMatMapList != null)
                        {
                            objInvItemPackItemMap = objInvItemPackMatMapList.SingleOrDefault(r => r.IMP_PACK_ITEM == PackMatProductPk);
                            if (objInvItemPackItemMap != null)
                            {
                                packingMatPK = PackMatProductPk;
                                GetFieldValues(ControlsEnum.PACKINGMATERIALBYPK);
                                if (objInvItemMst != null)
                                {
                                    txtMaterialCategory.Text = HttpUtility.HtmlDecode(objInvItemMst.INV_ITEM_CATEGORY.ITC_NAME);
                                    hdnMaterialCategoryPK.Value = objInvItemPackItemMap.INV_ITEM_MST1.ITM_CATEGORY.ToString();
                                }

                                txtPackingMaterial.Text = HttpUtility.HtmlDecode(objInvItemPackItemMap.INV_ITEM_MST1.ITM_NAME) + " (" + HttpUtility.HtmlDecode(objInvItemPackItemMap.INV_ITEM_MST1.ITM_CODE) + ") ";
                                hdnPackingMatID.Value = objInvItemPackItemMap.IMP_PACK_ITEM.ToString();
                                txtPackMatCount.Text = objInvItemPackItemMap.IMP_QUANITY.ToString();
                            }
                        }
                        break;
                    case ControlsEnum.REMOVEPACKMATITEM:
                        if (PackMatProductPk > 0 && objInvItemPackMatMapList != null)
                        {
                            objInvItemPackItemMap = objInvItemPackMatMapList.SingleOrDefault(r => r.IMP_PACK_ITEM == PackMatProductPk);
                            if (objInvItemPackItemMap != null)
                            {
                                objInvItemPackMatMapList.Remove(objInvItemPackItemMap);
                            }
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
                if (invItemMstList != null)
                {
                    uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdProductList.DataSource = invItemMstList;
                    grdProductList.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
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
                    case ControlsEnum.SUBGRADEPRODUCTS:
                        grdRelatedPrdts.DataSource = objInvItemRelMapList;
                        grdRelatedPrdts.DataBind();
                        break;
                    case ControlsEnum.RELATEDPRODUCTS:
                        gvSubTypeProducts.DataSource = objInvItemSubTypeMapList;
                        gvSubTypeProducts.DataBind();
                        break;
                    case ControlsEnum.PACKINGCOMBINATIONS:
                        if (dtPackCombinations != null && dtPackCombinations.Rows.Count > 0)
                        {
                            gdPackingCombination.DataSource = dtPackCombinations;
                            gdPackingCombination.DataBind();
                            if (gdPackingCombination.FooterRow != null)
                            {
                                Label lblTotalCount = gdPackingCombination.FooterRow.FindControl("lblTotalCount") as Label;
                                var totCount = dtPackCombinations.AsEnumerable().Sum(c => c.Field<int>("IPC_PROPERTY_COUNT"));
                                lblTotalCount.Text = totCount.ToString();
                            }
                        }
                        else
                        {
                            gdPackingCombination.DataSource = null;
                            gdPackingCombination.DataBind();
                        }
                        break;
                    case ControlsEnum.PACKINGMATERIALS:
                        gvPackingMaterials.DataSource = objInvItemPackMatMapList;
                        gvPackingMaterials.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Binds the  datalist( dlGradeProperties) with data
        /// </summary>
        private void BindGradeDatalist()
        {

            tableNoGradeProperties.Visible = false;
            if (dsGradeProperties.Tables.Count > 0)
            {
                if ((dsGradeProperties != null) && (dsGradeProperties.Tables[1] != null) && (dsGradeProperties.Tables[1].Rows.Count > 0))
                {
                    dlGradeProperties.DataSource = dsGradeProperties.Tables[1];
                    dlGradeProperties.DataBind();
                }
            }
            if (dlGradeProperties.Items.Count == 0)
            {
                tableNoGradeProperties.Visible = true;
            }
        }

        /// <summary>
        /// Method to Load Controls to UI
        /// If control sequence changed by Script, UI element not reflect directly,
        /// Need to logout and reload page. or Ctrl+F5, otherwise LoadControls() call on OnInit() function
        /// and curresponding logic need to change
        /// </summary>
        public void LoadControls()
        {
            if (spAdmConstGrpGetKvResultList != null && spAdmConstGrpGetKvResultList.Count > 0)
            {
                DataTable dtFieldControls = LINQToDataTable(spAdmConstGrpGetKvResultList);
                hdfDdlCount.Value = dtFieldControls.Rows.Count.ToString();

                HtmlGenericControl div1 = new HtmlGenericControl("div");
                div1.Attributes.Add("class", "div2col-S");
                tdCol1.Controls.Add(div1);

                for (int i = 0; i < dtFieldControls.Rows.Count; i += 2)
                {
                    Label lbl = new Label();
                    DropDownList ddl = new DropDownList();
                    HiddenField hdf = new HiddenField();
                    RequiredFieldValidator vrf = new RequiredFieldValidator();

                    ddl.ID = "ddl" + (i + 1).ToString();
                    ddl.CssClass = "select-half-a";
                    lbl.ID = "lbl" + (i + 1).ToString();
                    lbl.Text = dtFieldControls.Rows[i][Resources.DataFieldRes.GroupName].ToString();
                    lbl.AssociatedControlID = ddl.ID;

                    ddl.TabIndex = (short)(19 + i);
                    if (dtFieldControls.Rows[i].ItemArray[11].ToString().Contains("USE_CODE"))
                    {
                        ddl.Attributes.Add("onChange", "GenerateProductCode();");
                        ddl.Attributes.Add("canChangeProductCode", "1");
                    }
                    else
                    {
                        ddl.Attributes.Add("canChangeProductCode", "0");
                    }

                    vrf.ID = "vrfDyn" + Regex.Replace(lbl.Text, @"[^0-9a-zA-Z]+", "");
                    vrf.CssClass = "star";
                    vrf.SetFocusOnError = true;
                    vrf.InitialValue = "-1";
                    vrf.ValidationGroup = "Product";
                    vrf.EnableClientScript = true;
                    vrf.Display = ValidatorDisplay.Dynamic;
                    vrf.Text = "*";
                    vrf.ControlToValidate = ddl.ID;
                    vrf.ErrorMessage = string.Format(lbl.Text, GetLocalResourceObject("Err_Attributes"));
                    hdf.ID = "hdf" + ddl.ID;
                    hdf.Value = dtFieldControls.Rows[i][Resources.DataFieldRes.GroupCode].ToString();
                    DropDownID = ddl;
                    div1.Controls.Add(lbl);
                    div1.Controls.Add(ddl);
                    div1.Controls.Add(vrf);
                    div1.Controls.Add(hdf);

                    CngValue = Convert.ToInt32(dtFieldControls.Rows[i][Resources.DataFieldRes.GroupValue].ToString());
                    GetFieldValues(ControlsEnum.BINDDROPDOWN);
                    SetFieldValues(ControlsEnum.BINDDROPDOWN);
                }

                HtmlGenericControl div2 = new HtmlGenericControl("div");
                div2.Attributes.Add("class", "div2col-S");
                tdCol2.Controls.Add(div2);
                for (int i = 1; i < dtFieldControls.Rows.Count; i += 2)
                {
                    Label lbl = new Label();
                    DropDownList ddl = new DropDownList();
                    HiddenField hdf = new HiddenField();
                    RequiredFieldValidator vrf = new RequiredFieldValidator();

                    ddl.ID = "ddl" + (i + 1).ToString();
                    ddl.CssClass = "select-half-a";
                    lbl.ID = "lbl" + (i + 1).ToString();
                    lbl.Text = dtFieldControls.Rows[i][Resources.DataFieldRes.GroupName].ToString();
                    lbl.AssociatedControlID = ddl.ID;
                    ddl.TabIndex = (short)(19 + i);
                    if (dtFieldControls.Rows[i].ItemArray[11].ToString().Contains("USE_CODE"))
                    {
                        ddl.Attributes.Add("onChange", "GenerateProductCode();");
                        ddl.Attributes.Add("canChangeProductCode", "1");
                    }
                    else
                    {
                        ddl.Attributes.Add("canChangeProductCode", "0");
                    }

                    vrf.ID = "vrfDyn" + Regex.Replace(lbl.Text, @"[^0-9a-zA-Z]+", "");
                    vrf.CssClass = "star";
                    vrf.SetFocusOnError = true;
                    vrf.InitialValue = "-1";
                    vrf.ValidationGroup = "Product";
                    vrf.EnableClientScript = true;
                    vrf.Display = ValidatorDisplay.Dynamic;
                    vrf.Text = "*";
                    vrf.ControlToValidate = ddl.ID;
                    vrf.ErrorMessage = string.Format(lbl.Text, GetLocalResourceObject("Err_Attributes"));
                    hdf.ID = "hdf" + ddl.ID;
                    hdf.Value = dtFieldControls.Rows[i][Resources.DataFieldRes.GroupCode].ToString();
                    DropDownID = ddl;
                    div2.Controls.Add(lbl);
                    div2.Controls.Add(ddl);
                    div2.Controls.Add(vrf);
                    div2.Controls.Add(hdf);

                    CngValue = Convert.ToInt32(dtFieldControls.Rows[i][Resources.DataFieldRes.GroupValue].ToString());
                    GetFieldValues(ControlsEnum.BINDDROPDOWN);
                    SetFieldValues(ControlsEnum.BINDDROPDOWN);
                }
            }
        }

        /// <summary>
        /// Method for creating datatable from list
        /// </summary>
        /// <returns></returns>      
        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others          will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        /// <summary>
        /// Method for Bind UOM DropDown
        /// </summary>
        public void BindUOMDropDown()
        {
            ddlUOM.Items.Clear();
            if (invUomMstList != null && invUomMstList.Count > 0)
            {
                ddlUOM.DataSource = invUomMstList;
                ddlUOM.DataTextField = Resources.DataFieldRes.UomCode;
                ddlUOM.DataValueField = Resources.DataFieldRes.UomPK;
                ddlUOM.DataBind();
            }
            ddlUOM.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }

        /// <summary>
        /// Method for Bind Product Group DropDown
        /// </summary>
        public void BindProductGroupDropDown()
        {
            //ddlProductGroup.Items.Clear();
            //if (invItemGroupMstList != null && invItemGroupMstList.Count > 0)
            //{
            //    ddlProductGroup.DataSource = invItemGroupMstList;
            //    ddlProductGroup.DataTextField = Resources.DataFieldRes.INVProductGroupName;
            //    ddlProductGroup.DataValueField = Resources.DataFieldRes.INVProductGroupPK;
            //    ddlProductGroup.DataBind();
            //}
            //ddlProductGroup.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

            #region Advance Search
            ddlAdvProductGroup.Items.Clear();
            if (invItemGroupMstList != null && invItemGroupMstList.Count > 0)
            {
                ddlAdvProductGroup.DataSource = invItemGroupMstList;
                ddlAdvProductGroup.DataTextField = Resources.DataFieldRes.INVProductGroupName;
                ddlAdvProductGroup.DataValueField = Resources.DataFieldRes.INVProductGroupPK;
                ddlAdvProductGroup.DataBind();
            }
            ddlAdvProductGroup.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            #endregion
        }

        public void BindSubTypeDropDown()
        {
            ddlSubType.Items.Clear();
            if (dtSubType != null && dtSubType.Rows.Count > 0)
            {
                ddlSubType.DataSource = dtSubType;
                ddlSubType.DataTextField = Resources.DataFieldRes.cfgData;
                ddlSubType.DataValueField = Resources.DataFieldRes.cfgValue;
                ddlSubType.DataBind();
            }
            ddlSubType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
        }
        /// <summary>
        /// Method for Bind Dynamic DropDowns
        /// </summary>
        public void BindDropDown(DropDownList ddl)
        {
            ddl.Items.Clear();
            if (admConstMstList != null && admConstMstList.Count > 0)
            {
                ddl.DataSource = admConstMstList;
                ddl.DataTextField = Resources.DataFieldRes.ConstName;
                ddl.DataValueField = Resources.DataFieldRes.ConstPK;
                ddl.DataBind();
            }
            ddl.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            //ddl.Items.Insert(0, CommonConstants.SELECTVAL);
        }

        //Function to bind No of compounds dropdown
        public void BindDDLNoCompounds()
        {
            ddlNoCompounds.Items.Clear();

            dtNoCompounds = new DataTable();
            //DataRow dr;
            dtNoCompounds.Columns.Add("Key", typeof(String));
            dtNoCompounds.Columns.Add("Value", typeof(int));
            for (int i = 0; i < Convert.ToInt32(GetLocalResourceObject("NoCmpUpperLimit")); i++)
            {
                DataRow dr = dtNoCompounds.NewRow();
                dr["Key"] = (i + 1).ToString();
                dr["Value"] = (i + 1).ToString();
                dtNoCompounds.Rows.Add(dr);
            }
            ddlNoCompounds.DataSource = dtNoCompounds;
            ddlNoCompounds.DataTextField = "Key";
            ddlNoCompounds.DataValueField = "Value";
            ddlNoCompounds.DataBind();
        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDownList(ControlsEnum controlType)
        {
            switch (controlType)
            {

                case ControlsEnum.PACKINGSPEC:
                    //ddlPackingSpec
                    ddlPackingSpec.Items.Clear();
                    if (dsPackingSpec != null && dsPackingSpec.Tables.Count > 0 && dsPackingSpec.Tables[0].Rows.Count > 0)
                    {
                        ddlPackingSpec.DataSource = dsPackingSpec.Tables[0];
                        ddlPackingSpec.DataTextField = Resources.DataFieldRes.PackingSpecs;
                        ddlPackingSpec.DataValueField = Resources.DataFieldRes.PackingMstPK;
                        ddlPackingSpec.DataBind();
                    }
                    ddlPackingSpec.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #region PRODUCT GRADES
                case ControlsEnum.PRODUCTGRADES:
                    ddlProductGrade.Items.Clear();
                    if (dtProductGrade != null && dtProductGrade.Rows.Count > 0)
                    {
                        ddlProductGrade.DataSource = dtProductGrade;
                        ddlProductGrade.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlProductGrade.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlProductGrade.DataBind();
                    }
                    ddlProductGrade.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region MASTER PRODUCT GRADES
                case ControlsEnum.MASTERPRODUCTGRADES:
                    ddlPrdtGrade.Items.Clear();
                    if (dtProductGrade != null && dtProductGrade.Rows.Count > 0)
                    {
                        ddlPrdtGrade.DataSource = dtProductGrade;
                        ddlPrdtGrade.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlPrdtGrade.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlPrdtGrade.DataBind();
                    }
                    ddlPrdtGrade.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    #region Advance Search
                    ddlAdvPrdtGrade.Items.Clear();
                    if (dtProductGrade != null && dtProductGrade.Rows.Count > 0)
                    {
                        ddlAdvPrdtGrade.DataSource = dtProductGrade;
                        ddlAdvPrdtGrade.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlAdvPrdtGrade.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlAdvPrdtGrade.DataBind();
                    }
                    ddlAdvPrdtGrade.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    #endregion
                    break;
                #endregion

                #region PLANNING GROUP
                case ControlsEnum.PLANGROUP:
                    ddlPlanGroup.Items.Clear();
                    if (dtPlanGroup != null && dtPlanGroup.Rows.Count > 0)
                    {
                        ddlPlanGroup.DataSource = dtPlanGroup;
                        ddlPlanGroup.DataTextField = Resources.DataFieldRes.PlanGroupName;
                        ddlPlanGroup.DataValueField = Resources.DataFieldRes.PlanGroupPK;
                        ddlPlanGroup.DataBind();
                    }
                    ddlPlanGroup.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region MASTER PRODUCT SUB CATEGORY
                case ControlsEnum.PRODUCTSUBCATEGORY:
                    ddlProdSubCategory.Items.Clear();
                    if (dtSubType != null && dtSubType.Rows.Count > 0)
                    {
                        ddlProdSubCategory.DataSource = dtSubType;
                        ddlProdSubCategory.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlProdSubCategory.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlProdSubCategory.DataBind();
                    }
                    ddlProdSubCategory.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    #region Advance Region
                    ddlAdvProdSubCategory.Items.Clear();
                    if (dtSubType != null && dtSubType.Rows.Count > 0)
                    {
                        ddlAdvProdSubCategory.DataSource = dtSubType;
                        ddlAdvProdSubCategory.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlAdvProdSubCategory.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlAdvProdSubCategory.DataBind();
                    }
                    ddlAdvProdSubCategory.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    #endregion
                    break;
                #endregion

                #region BASED ON COMBINATIONS
                case ControlsEnum.BASEDONCOMBINATIONS:
                    ddlCombinationBasedOn.Items.Clear();
                    if (dtBasedOn != null && dtBasedOn.Rows.Count > 0)
                    {
                        ddlCombinationBasedOn.DataSource = dtBasedOn;
                        ddlCombinationBasedOn.DataTextField = Resources.DataFieldRes.GroupName;
                        ddlCombinationBasedOn.DataValueField = Resources.DataFieldRes.GroupPK;
                        ddlCombinationBasedOn.DataBind();
                    }
                    ddlCombinationBasedOn.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region Product Category

                case ControlsEnum.PRODUCTCATEGORY:
                    ddlPrdCategory.Items.Clear();
                    if (dtProductCategory != null && dtProductCategory.Rows.Count > 0)
                    {
                        ddlPrdCategory.DataSource = dtProductCategory;
                        ddlPrdCategory.DataTextField = Resources.DataFieldRes.PrdCategoryName;
                        ddlPrdCategory.DataValueField = Resources.DataFieldRes.PrdCategoryPK;
                        ddlPrdCategory.DataBind();
                    }
                    ddlPrdCategory.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;

                #endregion

                #region GSTCLASS
                case ControlsEnum.GSTCLASS:
                    ddlGSTClass.Items.Clear();
                    if (dtGSTclasslist != null && dtGSTclasslist.Rows.Count > 0)
                    {
                        ddlGSTClass.DataSource = dtGSTclasslist;
                        ddlGSTClass.DataTextField = Resources.DataFieldRes.HsnCode;
                        ddlGSTClass.DataValueField = Resources.DataFieldRes.HSNpk;
                        ddlGSTClass.DataBind();
                    }
                    ddlGSTClass.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
            }
        }

        /// <summary>
        /// function used to bind treeview 
        /// </summary>
        private void BindTreeView(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Bind Lines
                case ControlsEnum.LINES:
                    try
                    {
                        TreeNode treeNode;
                        trvLine.Nodes.Clear();
                        int elePK;
                        if (productLineBO != null)
                        {
                            foreach (PlantListBO rowElement in productLineBO.Plants)
                            {
                                treeNode = new TreeNode(rowElement.PLT_NAME, rowElement.PLT_PK.ToString());
                                treeNode.SelectAction = TreeNodeSelectAction.Select;
                                treeNode.ShowCheckBox = true;
                                treeNode.ToolTip = rowElement.PLT_CODE;
                                if (rowElement.Line != null && rowElement.Line.Count > 0)
                                    treeNode.Checked = rowElement.Line.Count == (rowElement.Line.Where(x => x.LNE_MAPPED == 1)).Count() ? true : false;
                                elePK = rowElement.PLT_PK;
                                PopulateElement(treeNode, elePK, rowElement); // here call the methode to populate the child element corr. parent element 
                                trvLine.Nodes.Add(treeNode);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    break;
                #endregion
                #region Bind Store Mapping
                case ControlsEnum.STOREMAPPING:
                    try
                    {
                        TreeNode treeNode;
                        trvStore.Nodes.Clear();
                        int elePK;
                        if (dtStoreMapping != null)
                        {
                            foreach (DataRow row in dtStoreMapping.Rows)
                            {
                                treeNode = new TreeNode(row["NAME"].ToString(), row["PK"].ToString());
                                treeNode.SelectAction = TreeNodeSelectAction.Select;
                                treeNode.ShowCheckBox = true;
                                treeNode.ToolTip = row["NAME"].ToString();
                                    treeNode.Checked = Convert.ToBoolean(row["IS_CHECKED"]);
                                //elePK = rowElement.PLT_PK;
                                //PopulateElement(treeNode, elePK, rowElement); // here call the methode to populate the child element corr. parent element 
                                trvStore.Nodes.Add(treeNode);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    break;
                    #endregion
            }
        }

        private void PopulateElement(TreeNode treeNode, int elementPK, PlantListBO rowElement)
        {
            TreeNode childNode;
            if (productLineBO != null && productLineBO.Plants.Count > 0)
            {
                foreach (PlantLineBO Elements in rowElement.Line)
                {
                    childNode = new TreeNode(Elements.LNE_CODE, Elements.LNE_PK.ToString());
                    childNode.SelectAction = TreeNodeSelectAction.Select;
                    childNode.ShowCheckBox = true;
                    childNode.ToolTip = Elements.LNE_NAME;
                    childNode.Checked = Convert.ToBoolean(Elements.LNE_MAPPED);
                    treeNode.ChildNodes.Add(childNode);
                    if (childNode.Checked)
                        treeNode.ExpandAll();
                }
            }
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                switch (commonActions)
                {
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                    case ActionsEnum.ACTIVATE:

                        if (CurrPK > 0)
                        {
                            if (IsBrandProduct)
                            {
                                trPackigSpec.Visible = true;
                                GetFieldValues(ControlsEnum.PACKINGSPEC);
                                SetFieldValues(ControlsEnum.PACKINGSPEC);
                            }
                            // Get And Set the Location details
                            GetFieldValues(ControlsEnum.UOM);
                            SetFieldValues(ControlsEnum.UOM);
                            GetFieldValues(ControlsEnum.PRODUCTCATEGORY);
                            SetFieldValues(ControlsEnum.PRODUCTCATEGORY);
                            GetFieldValues(ControlsEnum.PRODUCTGROUP);
                            SetFieldValues(ControlsEnum.PRODUCTGROUP);
                            GetFieldValues(ControlsEnum.PRODUCT);
                            SetFieldValues(ControlsEnum.PRODUCT);
                            GetFieldValues(ControlsEnum.GRADEPROPERTY);
                            SetFieldValues(ControlsEnum.GRADEPROPERTY);
                            GetFieldValues(ControlsEnum.PACKINGCOMBINATIONS);
                            SetFieldValues(ControlsEnum.PACKINGCOMBINATIONS);
                            txtProductName.Focus();
                            ModifiedDatePnl.Visible = true;
                            if (Mode == ActionsEnum.VIEW)
                                EntryStatus = EntryStatus.VIEWMODE;
                            else
                                EntryStatus = EntryStatus.ENTRYMODE;

                            return;
                        }

                        // if no items selected, Show Error Message
                        litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    case ActionsEnum.SUBGRADEPRODUCTS:
                        if (CurrPK > 0)
                        {

                            // Get And Set the Location details  
                            hdfRelItemPk.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.PRODUCTBYPK);
                            if (objInvItemMst != null)
                            {
                                lblRelProductCode.ToolTip = HttpUtility.HtmlDecode(objInvItemMst.ITM_CODE);
                                lblRelProductCode.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objInvItemMst.ITM_CODE), 90);
                                lblRelProductName.ToolTip = HttpUtility.HtmlDecode(objInvItemMst.ITM_NAME);
                                lblRelProductName.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objInvItemMst.ITM_NAME), 115);
                                #region Show the grade of main product in main header
                                if (dtProductGrade == null || dtProductGrade.Rows.Count == 0)
                                    GetFieldValues(ControlsEnum.PRODUCTGRADES);
                                if (dtProductGrade != null || dtProductGrade.Rows.Count > 0)
                                {
                                    var prdtGrade = (from r in dtProductGrade.AsEnumerable()
                                                     where r.Field<byte>("CFG_VALUE") == Convert.ToByte(objInvItemMst.ITM_GRADE)
                                                     select new
                                                     {
                                                         GradeName = r.Field<string>("CFG_DATA")
                                                     }).ToList();
                                    if (prdtGrade != null && prdtGrade.Count > 0)
                                        lblSubGradePrdtGrade.Text = lblSubGradePrdtGrade.ToolTip = HttpUtility.HtmlDecode(prdtGrade[0].GradeName);
                                }
                                #endregion
                            }
                            ModifiedDatePnl.Visible = false;
                            EntryStatus = EntryStatus.ENTRYMODE;
                            return;
                        }
                        // if no items selected, Show Error Message
                        litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;

                    case ActionsEnum.RELATEDPRODUCTS:
                        if (CurrPK > 0)
                        {
                            hdfRelItemPk.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.PRODUCTBYPK);
                            if (objInvItemMst != null)
                            {
                                lblRelatedProdCode.ToolTip = HttpUtility.HtmlDecode(objInvItemMst.ITM_CODE);
                                lblRelatedProdCode.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objInvItemMst.ITM_CODE), 90);
                                lblRelatedProdName.ToolTip = HttpUtility.HtmlDecode(objInvItemMst.ITM_NAME);
                                lblRelatedProdName.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objInvItemMst.ITM_NAME), 115);
                                #region Show the sub category of main product in main header
                                if (dtSubType == null || dtSubType.Rows.Count == 0)
                                    GetFieldValues(ControlsEnum.SUBTYPE);
                                if (dtSubType != null || dtSubType.Rows.Count > 0)
                                {
                                    var subCategory = (from r in dtSubType.AsEnumerable()
                                                       where r.Field<byte>("CFG_VALUE") == Convert.ToByte(objInvItemMst.ITM_SUB_TYPE)
                                                       select new
                                                       {
                                                           subType = r.Field<string>("CFG_DATA")
                                                       }).ToList();
                                    if (subCategory != null && subCategory.Count > 0)
                                        lblRelatedProdSubCategory.Text = lblRelatedProdSubCategory.ToolTip = HttpUtility.HtmlDecode(subCategory[0].subType);
                                }
                                #endregion
                                #region Show the grade of main product in main header
                                if (dtProductGrade == null || dtProductGrade.Rows.Count == 0)
                                    GetFieldValues(ControlsEnum.PRODUCTGRADES);
                                if (dtProductGrade != null || dtProductGrade.Rows.Count > 0)
                                {
                                    var prdtGrade = (from r in dtProductGrade.AsEnumerable()
                                                     where r.Field<byte>("CFG_VALUE") == Convert.ToByte(objInvItemMst.ITM_GRADE)
                                                     select new
                                                     {
                                                         GradeName = r.Field<string>("CFG_DATA")
                                                     }).ToList();
                                    if (prdtGrade != null && prdtGrade.Count > 0)
                                        lblRelatedProdGrade.Text = lblRelatedProdGrade.ToolTip = HttpUtility.HtmlDecode(prdtGrade[0].GradeName);
                                }
                                #endregion
                            }
                            ModifiedDatePnl.Visible = false;
                            EntryStatus = EntryStatus.ENTRYMODE;
                            return;
                        }
                        // if no items selected, Show Error Message
                        litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;

                    #region Packing Materials
                    case ActionsEnum.PACKINGMATERIALS:
                        if (CurrPK > 0)
                        {
                            hdfRelItemPk.Value = CurrPK.ToString();


                            GetFieldValues(ControlsEnum.PRODUCTBYPK);
                            if (objInvItemMst != null)
                            {
                                lblPackMatProdCode.ToolTip = HttpUtility.HtmlDecode(objInvItemMst.ITM_CODE);
                                lblPackMatProdCode.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objInvItemMst.ITM_CODE), 90);
                                lblPackMatProdName.ToolTip = HttpUtility.HtmlDecode(objInvItemMst.ITM_NAME);
                                lblPackMatProdName.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objInvItemMst.ITM_NAME), 115);
                            }
                            ModifiedDatePnl.Visible = false;
                            EntryStatus = EntryStatus.ENTRYMODE;
                            return;
                        }
                        // if no items selected, Show Error Message
                        litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    case ActionsEnum.STOREMAPPING:
                        if (CurrPK > 0)
                        {
                            EntryStatus = EntryStatus.ENTRYMODE;
                            DisplayTab = (int)TabEnum.STOREMAPPING;
                            return;
                        }
                        // if no items selected, Show Error Message
                        litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    case ActionsEnum.LINES:
                        if (CurrPK > 0)
                        {

                            EntryStatus = EntryStatus.ENTRYMODE;
                            DisplayTab = (int)TabEnum.LINES;
                            return;
                        }
                        // if no items selected, Show Error Message
                        litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            ItemPK = 0;
            ddlFilterBy.SelectedIndex = 0;
            txtSearchBy.Text = string.Empty;
            txtProductCode.Text = string.Empty;
            chbAutoGenerate.Checked = true;
            chkActive.Checked = true;
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AutoGenerateChecked", "AutoGenerateChecked();", true);
            txtProductName.Text = string.Empty;
            ddlUOM.SelectedIndex = 0;
            ddlPrdCategory.SelectedIndex = 0;
            txtWeight.Text = string.Empty;
            txtMinWeight.Text = string.Empty;
            txtMaxWeight.Text = string.Empty;
            txtProductDesc.Text = string.Empty;
            txtInterState.Text = string.Empty;
            txtIntraState.Text = string.Empty;
            txtOthers.Text=string.Empty;
            txtExport.Text = string.Empty; 
            txtFirstRef.Text = txtSecRef.Text = txtThirdRef.Text = txtFourthRef.Text = string.Empty;
            ddlNoCompounds.SelectedIndex = 0;
            ddlCombinationBasedOn.ClearSelection();
            hdfInventoryProdPK.Value=hdfProductGroupPk.Value = "0";
            txtInventoryProd.Text = Resources.Messages.AutoDefaultValue;
            txtProductGroup.Text = GetLocalResourceObject("TypeMin3").ToString();
            HSNpk = 0;
            ddlGSTClass.ClearSelection();
            pnlPrint.Visible = false;
            //Clear grade checked box values other that hiddenvalue 1
            foreach (DataListItem item in dlGradeProperties.Items)
            {
                CheckBox chkGrade = (CheckBox)item.FindControl("chkGrade");
                HiddenField hdnGradeValue = (HiddenField)item.FindControl("hdnGradeValue");
                if (hdnGradeValue.Value == "1")
                {
                    chkGrade.Checked = true;
                    chkGrade.Enabled = false;
                }
                else
                {
                    //chkGrade.Checked = false;
                    chkGrade.Checked = true;
                }
            }

            for (int i = 1; i <= Convert.ToInt32(hdfDdlCount.Value); i += 2)
            {
                ((DropDownList)tdCol1.FindControl("ddl" + i.ToString())).SelectedIndex = 0;
            }
            for (int i = 2; i <= Convert.ToInt32(hdfDdlCount.Value); i += 2)
            {
                ((DropDownList)tdCol1.FindControl("ddl" + i.ToString())).SelectedIndex = 0;
            }

            PageIndex = CommonConstants.SELECT_VALUE_ONE;
            lblLastModifiedHDR.Text = string.Empty;
            ModifiedDatePnl.Visible = false;
            ResetAdvanceSearch();
        }

        /// <summary>
        /// Method used to Reset Advanced Searchform Controls
        /// </summary>
        private void ResetAdvanceSearch()
        {
            ddlAdvPrdtGrade.SelectedIndex = 0;
            ddlAdvProdSubCategory.SelectedIndex = 0;
            ddlAdvProductGroup.SelectedIndex = 0;
            txtAdvProduct.Text = string.Empty;
            hdfProductPK.Value = string.Empty;
            chkActiveFilter.Checked = true;
            chkProdAuto.Checked = true;
        }

        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        private void ResetRelTab()
        {
            txtItem.Text = GetLocalResourceObject("TypeMin3").ToString();
            ddlProductGrade.SelectedValue = CommonConstants.SELECTVAL;
        }

        private void ResetSubTypeTab()
        {
            txtRelatedItem.Text = GetLocalResourceObject("TypeMin3").ToString();
            ddlSubType.SelectedValue = CommonConstants.SELECTVAL;
        }
        private void ResetPackMatTab()
        {
            txtMaterialCategory.Text = GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString();
            hdnMaterialCategoryPK.Value = "0";
            txtPackingMaterial.Text = GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString();
            hdnPackingMatID.Value = "0";
            txtPackMatCount.Text = string.Empty;
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
            GridViewRow gvr;
            InvItemMstService InvItemMstServiceClient;
            InvItemMstServiceClient = null;
            try
            {
                DisplayTab = (int)TabEnum.PRODUCTDETAILS;
                int result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlCombinationBasedOn")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.ITEMSELECTED;
                }
              
                switch (commonActions)
                {
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        foreach (GridViewRow grdrow in grdProductList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            // check row selected or not
                            if (rbtn.Checked)
                            {
                                CurrPK = Convert.ToInt32(grdProductList.DataKeys[grdrow.RowIndex].Values[0]);
                                GetFieldValues(ControlsEnum.PRODUCTDOC);
                                if (dtFiles != null && dtFiles.Rows.Count > 0)
                                {
                                    pnlPrint.Visible = true;
                                    FileUrl = dtFiles.Rows[0]["DOC_PATH"].ToString().Replace("~", "../..");
                                }
                                else
                                {
                                    pnlPrint.Visible = false;
                                    FileUrl = string.Empty;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Save
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            int BizUnit = 0;
                            invItemMstList = new List<INV_ITEM_MST>();
                            InvItemMstServiceClient = new InvItemMstService();
                            InvItemMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemMstServiceClient);
                            invItemMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_MST>();
                            invItemMstObj = SetUIValuesToObject();
                            invItemMstList.Add(invItemMstObj);
                            if (hdfIsProductInSBU.Value == "1")
                            {
                                BizUnit = currentUser.SBUID;
                            }

                            if (!InvItemMstServiceClient.IsItemCodeExist(invItemMstObj,Convert.ToInt32(hdfIsProductInSBU.Value)))//Function Used For Item Code Duplication Checking
                            {
                                result = InvItemMstServiceClient.SaveInvItemMst(invItemMstList, BizUnit);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    #region Group Mapping for Production Planning
                                    if (productBO != null && productBO.DetailList.Count > 0)
                                    {
                                        string xmlDoc = CommonFunctions.XmlSerialize<ProductBO>(productBO);
                                        int retVal = ProductsBL.SaveProductGroup(xmlDoc);
                                    }
                                    #endregion
                                    #region CRM Product Insert
                                    if (GetGlobalResourceObject("ConfigurationsRes", "IsCrmEnabled").ToString() == "1")
                                    {
                                        int crmResult = CommonBL.SaveAsCRMProduct(result);
                                        if (crmResult <= 0)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("msg_ErrorCRM").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                    }
                                    #endregion  

                                    SortBy = Resources.DataFieldRes.ItemCode;
                                    SortDirection = Resources.Report.SortAscending;
                                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Product);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                    btnNew.Focus();

                                }
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ProductCodeExist;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Product);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            
                        }
                        break;
                    #endregion
                      
                    #region GENERATEBRAND
                    case ActionsEnum.GENERATEBRAND:
                        gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurrPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfItemPK")).Value);
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        result = ProductsBL.BrandProductSave(CurrPK, currentUser.PKUser);
                        if (result > 0)
                        {
                            //ResetForm();
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            litErrorMsg.Text = Resources.Messages.Msg_Generate_Brand_Success;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else if (result <=0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_BrandProductSaveFailed").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region New
                    case ActionsEnum.NEW:
                        if (IsBrandProduct)
                        {
                            trPackigSpec.Visible = true;
                            GetFieldValues(ControlsEnum.PACKINGSPEC);
                            SetFieldValues(ControlsEnum.PACKINGSPEC);
                        }
                        GetFieldValues(ControlsEnum.UOM);
                        SetFieldValues(ControlsEnum.UOM);
                        GetFieldValues(ControlsEnum.PRODUCTCATEGORY);
                        SetFieldValues(ControlsEnum.PRODUCTCATEGORY);
                        GetFieldValues(ControlsEnum.PRODUCTGROUP);
                        SetFieldValues(ControlsEnum.PRODUCTGROUP);
                        PlanGroupPK = 0;
                        PrdCategoryPK = 0;
                        GetFieldValues(ControlsEnum.PLANGROUP);
                        SetFieldValues(ControlsEnum.PLANGROUP);
                        GetFieldValues(ControlsEnum.GSTCLASS);
                        SetFieldValues(ControlsEnum.GSTCLASS);
                        //ResetForm();
                        ModifiedDatePnl.Visible = false;
                        this.txtProductName.Focus();
                        ddlUOM.Enabled = true;
                        txtWeight.Enabled = true;
                        txtWeight.CssClass = "input-small numeric";
                        txtMinWeight.Enabled = true;
                        txtMinWeight.CssClass = "input-small numeric";
                        txtMaxWeight.Enabled = true;
                        txtMaxWeight.CssClass = "input-small numeric";
                        EntryStatus = EntryStatus.NEWMODE;
                        GetFieldValues(ControlsEnum.PACKINGCOMBINATIONS);
                        SetFieldValues(ControlsEnum.PACKINGCOMBINATIONS);
                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        PageIndex = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnNew.Focus();
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
                        InvItemMstServiceClient = new InvItemMstService();
                        InvItemMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemMstServiceClient);
                        invItemMstList = new List<INV_ITEM_MST>();
                        invItemMstObj = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_MST>();
                        invItemMstObj.ITM_PK = CurrPK;
                        invItemMstObj.ITM_MOD_DT = LastModifiedTime;
                        invItemMstList.Add(invItemMstObj);

                        result = InvItemMstServiceClient.DeleteInvItemMst(invItemMstList);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            ResetForm();
                            btnNew.Focus();
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Product);
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
                        SetUIEditView(commonActions);
                        btnCancel.Focus();
                        break;
                    #endregion

                    #region ACTIVATE
                    case ActionsEnum.ACTIVATE:
                        SetUIEditView(commonActions);
                        break;
                    #endregion

                    #region RELATEDPRODUCTS
                    case ActionsEnum.RELATEDPRODUCTS:
                        SetUIEditView(commonActions);
                        if (CurrPK > 0)
                        {
                            objInvItemSubTypeMapList = null;
                            EntryStatus = EntryStatus.ENTRYMODE;
                            DisplayTab = (int)TabEnum.SUBTYPEPRODUCTS;
                            GetFieldValues(ControlsEnum.SUBTYPE);
                            SetFieldValues(ControlsEnum.SUBTYPE);
                            GetFieldValues(ControlsEnum.RELATEDPRODUCTS);
                            SetFieldValues(ControlsEnum.RELATEDPRODUCTS);
                            ResetSubTypeTab();
                            txtRelatedItem.Focus();
                        }
                        break;
                    #endregion

                    #region SUBGRADEPRODUCTS
                    case ActionsEnum.SUBGRADEPRODUCTS:
                        SetUIEditView(commonActions);
                        if (CurrPK > 0)
                        {
                            objInvItemRelMapList = null;
                            EntryStatus = EntryStatus.ENTRYMODE;
                            DisplayTab = (int)TabEnum.SUBGRADEPRODUCTS;
                            GetFieldValues(ControlsEnum.PRODUCTGRADES);
                            SetFieldValues(ControlsEnum.PRODUCTGRADES);
                            GetFieldValues(ControlsEnum.SUBGRADEPRODUCTS);
                            SetFieldValues(ControlsEnum.SUBGRADEPRODUCTS);
                            ResetRelTab();
                            txtItem.Focus();
                        }
                        break;
                    #endregion

                    #region ADD ITEM
                    case ActionsEnum.ADDITEM:
                        DisplayTab = (int)TabEnum.SUBGRADEPRODUCTS;
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            objTempInvItemRelMapList = objInvItemRelMapList;
                            int ItemId = 0;
                            int.TryParse(hdfItemID.Value, out ItemId);
                            if (ItemId > 0)
                            {
                                if (CurrPK == ItemId)//Relative product should not be same as product in master.
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_SameRelativeProduct;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    return;
                                }
                                else
                                {
                                    if (objTempInvItemRelMapList != null && objTempInvItemRelMapList.Where(r => r.IMR_REL_ITEM == ItemId && r.IMR_REL_ITEM != RelProductPk).Count() > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ItemAlreadyAdded").ToString()) + "');", true);
                                    }
                                    else
                                    {
                                        if (RelProductPk != 0 && objTempInvItemRelMapList != null)
                                        {
                                            objInvItemRelMap = objTempInvItemRelMapList.SingleOrDefault(itm => itm.IMR_REL_ITEM == RelProductPk);
                                            if (objInvItemRelMap != null)
                                            {
                                                ItemId = 0;
                                                int.TryParse(hdfItemID.Value, out ItemId);
                                                hdfRelItemPk.Value = ItemId.ToString();
                                                GetFieldValues(ControlsEnum.PRODUCTBYPK);
                                                if (objInvItemMst != null && Convert.ToInt32(ddlProductGrade.SelectedValue) == (int)ProductGradeEnum.Scrap && objInvItemMst.ITM_GRADE != (int)ProductGradeEnum.Scrap)
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SelectScrapItem").ToString()) + "');", true);
                                                    return;
                                                }
                                                objInvItemRelMap.INV_ITEM_MST1 = null;
                                                objInvItemRelMap.INV_ITEM_MST1 = objInvItemMst;
                                                objInvItemRelMap.IMR_REL_ITEM = ItemId;
                                                objInvItemRelMap.IMR_ITEM = CurrPK;
                                                objInvItemRelMap.IMR_CRTD_BY = currentUser.PKUser;
                                                objInvItemRelMap.IMR_CRTD_DT = DateTime.Now;
                                                objInvItemRelMap.IMR_ITEM_GRADE = Convert.ToByte(ddlProductGrade.SelectedValue);
                                                objInvItemRelMap.IMR_MOD_BY = currentUser.PKUser;
                                                objInvItemRelMap.IMR_MOD_DT = DateTime.Now;

                                            }
                                        }
                                        else
                                        {
                                            if (objTempInvItemRelMapList == null || objTempInvItemRelMapList.Count == 0)
                                            {
                                                objTempInvItemRelMapList = new List<INV_ITEM_REL_MAP>();
                                            }
                                            objInvItemRelMap = new INV_ITEM_REL_MAP();
                                            ItemId = 0;
                                            int.TryParse(hdfItemID.Value, out ItemId);
                                            hdfRelItemPk.Value = ItemId.ToString();
                                            GetFieldValues(ControlsEnum.PRODUCTBYPK);
                                            if (objInvItemMst != null && Convert.ToInt32(ddlProductGrade.SelectedValue) == (int)ProductGradeEnum.Scrap && objInvItemMst.ITM_GRADE != (byte)ProductGradeEnum.Scrap)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SelectScrapItem").ToString()) + "');", true);
                                                return;
                                            }
                                            objInvItemRelMap.INV_ITEM_MST1 = objInvItemMst;
                                            objInvItemRelMap.IMR_PK = 0;
                                            objInvItemRelMap.IMR_REL_ITEM = ItemId;
                                            objInvItemRelMap.IMR_ITEM = CurrPK;
                                            objInvItemRelMap.IMR_CRTD_BY = currentUser.PKUser;
                                            objInvItemRelMap.IMR_CRTD_DT = DateTime.Now;
                                            objInvItemRelMap.IMR_ITEM_GRADE = Convert.ToByte(ddlProductGrade.SelectedValue);
                                            objInvItemRelMap.IMR_MOD_BY = currentUser.PKUser;
                                            objInvItemRelMap.IMR_MOD_DT = DateTime.Now;
                                            objTempInvItemRelMapList.Add(objInvItemRelMap);
                                        }

                                        objInvItemRelMapList = objTempInvItemRelMapList;
                                        SetFieldValues(ControlsEnum.SUBGRADEPRODUCTS);
                                        ResetRelTab();
                                        RelProductPk = 0;
                                    }
                                }
                            }
                        }

                        break;
                    #endregion

                    #region EDIT ITEM
                    case ActionsEnum.EDITITEM:
                        DisplayTab = (int)TabEnum.SUBGRADEPRODUCTS;
                        hdfRelPrdtPk = (HiddenField)(((ImageButton)sender).Parent.Parent as GridViewRow).FindControl("hdfRelPrdtPk");
                        RelProductPk = Convert.ToInt32(hdfRelPrdtPk.Value);
                        if (RelProductPk > 0)
                        {
                            GetUIValuesFromObject(ControlsEnum.SELECTEDRELITEM);
                        }
                        break;
                    #endregion

                    #region REMOVE ITEM
                    case ActionsEnum.REMOVEITEM:
                        DisplayTab = (int)TabEnum.SUBGRADEPRODUCTS;
                        hdfRelPrdtPk = (HiddenField)(((ImageButton)sender).Parent.Parent as GridViewRow).FindControl("hdfRelPrdtPk");
                        RelProductPk = Convert.ToInt32(hdfRelPrdtPk.Value);
                        if (RelProductPk > 0)
                        {
                            GetUIValuesFromObject(ControlsEnum.REMOVEITEM);
                            SetFieldValues(ControlsEnum.SUBGRADEPRODUCTS);
                        }
                        RelProductPk = 0;
                        break;
                    #endregion

                    #region SAVE GRADE PRODUCTS
                    // Do Action for , when click save button
                    case ActionsEnum.SAVEGRADEPRODUCTS:
                        DisplayTab = (int)TabEnum.SUBGRADEPRODUCTS;
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            if (grdRelatedPrdts.Rows.Count > 0)
                            {
                                invRelItemMapList = new List<INV_ITEM_REL_MAP>();
                                InvItemMstServiceClient = new InvItemMstService();
                                InvItemMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemMstServiceClient);
                                invRelItemMapList = (List<INV_ITEM_REL_MAP>)SetUIValuesToObject(ControlsEnum.SAVEGRADEPRODUCTS);
                                result = InvItemMstServiceClient.SaveInvRelatedItemMap(invRelItemMapList, CurrPK);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_Sub_Prdt_SaveSuccess") + "','" + Resources.Messages.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                    btnNew.Focus();
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Msg_Sub_Prdt_Save").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRelPrdts").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region DELETE GRADE PRODUCTS
                    case ActionsEnum.DELETEGRADEPRODUCTS:
                        DisplayTab = (int)TabEnum.SUBGRADEPRODUCTS;
                        if (grdRelatedPrdts.Rows.Count > 0)
                        {
                            InvItemMstServiceClient = new InvItemMstService();
                            InvItemMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemMstServiceClient);
                            objInvItemRelMap = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_REL_MAP>();
                            objInvItemRelMap.IMR_ITEM = CurrPK;
                            result = InvItemMstServiceClient.DeleteInvRelatedItemMap(objInvItemRelMap);
                            if (result > 0)
                            {
                                ResetForm();
                                btnNew.Focus();
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_SubPrdt_Delete_Success").ToString() + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (result == -1) //delete reference error
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("CannotdeleteAlreadyasigned").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRelPrdts").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region CLEAR ITEM
                    case ActionsEnum.CLEARITEM:
                        ResetRelTab();
                        DisplayTab = (int)TabEnum.SUBGRADEPRODUCTS;
                        break;
                    #endregion

                    #region ADD RELEATED SUB TYPE ITEM
                    case ActionsEnum.ADDSUBTYPEITEM:
                        DisplayTab = (int)TabEnum.SUBTYPEPRODUCTS;
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            objTempInvItemTypeMapList = objInvItemSubTypeMapList;
                            int ItemId = 0;
                            int.TryParse(hdfRelatedItem.Value, out ItemId);
                            if (CurrPK == ItemId)//Relative product should not be same as product in master.
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_SameRelativeProduct;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                return;
                            }
                            else
                            {
                                if (objTempInvItemTypeMapList != null && objTempInvItemTypeMapList.Where(r => r.ISM_REL_ITEM == ItemId && r.ISM_REL_ITEM != SubTypeProductPk).Count() > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ItemAlreadyAdded").ToString()) + "');", true);
                                }
                                else
                                {
                                    if (SubTypeProductPk != 0 && objTempInvItemTypeMapList != null)
                                    {
                                        objInvItemSubTypeMap = objTempInvItemTypeMapList.SingleOrDefault(itm => itm.ISM_REL_ITEM == SubTypeProductPk);
                                        if (objInvItemSubTypeMap != null)
                                        {
                                            ItemId = 0;
                                            int.TryParse(hdfRelatedItem.Value, out ItemId);
                                            hdfRelItemPk.Value = ItemId.ToString();
                                            GetFieldValues(ControlsEnum.PRODUCTBYPK);

                                            objInvItemSubTypeMap.INV_ITEM_MST1 = null;
                                            objInvItemSubTypeMap.INV_ITEM_MST1 = objInvItemMst.DeepClone();
                                            objInvItemSubTypeMap.ISM_REL_ITEM = ItemId;
                                            objInvItemSubTypeMap.ISM_ITEM = CurrPK;
                                            objInvItemSubTypeMap.ISM_CRTD_BY = currentUser.PKUser;
                                            objInvItemSubTypeMap.ISM_CRTD_DT = DateTime.Now;
                                            objInvItemSubTypeMap.ISM_ITEM_SUB_TYPE = Convert.ToByte(objInvItemMst.ITM_SUB_TYPE);//Convert.ToByte(ddlSubType.SelectedValue);
                                            objInvItemSubTypeMap.ISM_MOD_BY = currentUser.PKUser;
                                            objInvItemSubTypeMap.ISM_MOD_DT = DateTime.Now;

                                        }
                                    }
                                    else
                                    {
                                        if (objTempInvItemTypeMapList == null || objTempInvItemTypeMapList.Count == 0)
                                        {
                                            objTempInvItemTypeMapList = new List<INV_ITEM_SUB_TYPE_MAP>();
                                        }
                                        objInvItemSubTypeMap = new INV_ITEM_SUB_TYPE_MAP();
                                        ItemId = 0;
                                        int.TryParse(hdfRelatedItem.Value, out ItemId);
                                        hdfRelItemPk.Value = ItemId.ToString();
                                        GetFieldValues(ControlsEnum.PRODUCTBYPK);

                                        objInvItemSubTypeMap.INV_ITEM_MST1 = objInvItemMst;
                                        objInvItemSubTypeMap.ISM_PK = 0;
                                        objInvItemSubTypeMap.ISM_REL_ITEM = ItemId;
                                        objInvItemSubTypeMap.ISM_ITEM = CurrPK;
                                        objInvItemSubTypeMap.ISM_CRTD_BY = currentUser.PKUser;
                                        objInvItemSubTypeMap.ISM_CRTD_DT = DateTime.Now;
                                        objInvItemSubTypeMap.ISM_ITEM_SUB_TYPE = Convert.ToByte(objInvItemMst.ITM_SUB_TYPE);//Convert.ToByte(ddlSubType.SelectedValue);
                                        objInvItemSubTypeMap.ISM_MOD_BY = currentUser.PKUser;
                                        objInvItemSubTypeMap.ISM_MOD_DT = DateTime.Now;
                                        objTempInvItemTypeMapList.Add(objInvItemSubTypeMap);
                                    }

                                    objInvItemSubTypeMapList = objTempInvItemTypeMapList;
                                    SetFieldValues(ControlsEnum.RELATEDPRODUCTS);
                                    ResetSubTypeTab();
                                    SubTypeProductPk = 0;
                                }
                            }
                        }

                        break;
                    #endregion

                    #region EDIT RELEATED SUB TYPE ITEM
                    case ActionsEnum.EDITSUBTYPEITEM:
                        DisplayTab = (int)TabEnum.SUBTYPEPRODUCTS;
                        hdfRelPrdtPk = (HiddenField)(((ImageButton)sender).Parent.Parent as GridViewRow).FindControl("hdfRelPrdtPk");
                        SubTypeProductPk = Convert.ToInt32(hdfRelPrdtPk.Value);
                        if (SubTypeProductPk > 0)
                        {
                            GetUIValuesFromObject(ControlsEnum.SELECTEDSUBTYPEITEM);
                        }
                        break;
                    #endregion

                    #region REMOVE RELEATED SUB TYPE ITEM
                    case ActionsEnum.REMOVESUBTYPEITEM:
                        DisplayTab = (int)TabEnum.SUBTYPEPRODUCTS;
                        hdfRelPrdtPk = (HiddenField)(((ImageButton)sender).Parent.Parent as GridViewRow).FindControl("hdfRelPrdtPk");
                        SubTypeProductPk = Convert.ToInt32(hdfRelPrdtPk.Value);
                        if (SubTypeProductPk > 0)
                        {
                            GetUIValuesFromObject(ControlsEnum.REMOVESUBTYPEITEM);
                            SetFieldValues(ControlsEnum.RELATEDPRODUCTS);
                        }
                        SubTypeProductPk = 0;
                        break;
                    #endregion

                    #region SAVE SUB TYPE PRODUCTS
                    // Do Action for , when click save button
                    case ActionsEnum.SAVESUBTYPEPRODUCTS:
                        DisplayTab = (int)TabEnum.SUBTYPEPRODUCTS;
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            if (gvSubTypeProducts.Rows.Count > 0)
                            {
                                invSubTypeItemMapList = new List<INV_ITEM_SUB_TYPE_MAP>();
                                InvItemMstServiceClient = new InvItemMstService();
                                InvItemMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemMstServiceClient);
                                invSubTypeItemMapList = (List<INV_ITEM_SUB_TYPE_MAP>)SetUIValuesToObject(ControlsEnum.SAVERELPRODUCTS);
                                result = InvItemMstServiceClient.SaveInvSubTypeItemMap(invSubTypeItemMapList, CurrPK);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_Rel_Prdt_SaveSuccess") + "','" + Resources.Messages.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                    btnNew.Focus();
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Msg_Rel_Prdt_Save").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRelPrdts").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region DELETE SUB TYPE PRODUCTS
                    case ActionsEnum.DELETESUBTYPEPRODUCTS:
                        DisplayTab = (int)TabEnum.SUBTYPEPRODUCTS;
                        if (gvSubTypeProducts.Rows.Count > 0)
                        {
                            InvItemMstServiceClient = new InvItemMstService();
                            InvItemMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemMstServiceClient);
                            objInvItemSubTypeMap = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_SUB_TYPE_MAP>();
                            objInvItemSubTypeMap.ISM_ITEM = CurrPK;
                            result = InvItemMstServiceClient.DeleteInvSubTypeItemMap(objInvItemSubTypeMap);
                            if (result > 0)
                            {
                                ResetForm();
                                btnNew.Focus();
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_RelPrdt_Delete_Success").ToString() + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (result == -1) //delete reference error
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("CannotdeleteAlreadyasigned").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRelPrdts").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region CLEAR SUB TYPE ITEM
                    case ActionsEnum.CLEARSUBTYPEITEM:
                        ResetSubTypeTab();
                        DisplayTab = (int)TabEnum.SUBTYPEPRODUCTS;
                        break;
                    #endregion

                    #region SELECTED INDEX CHANGED
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        GetFieldValues(ControlsEnum.PACKINGCOMBINATIONS);
                        SetFieldValues(ControlsEnum.PACKINGCOMBINATIONS);
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CLEARSEARCH:
                        ResetAdvanceSearch();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region PACKINGMATERIAL TAB ACTIONS

                    #region PACKINGMATERIALS
                    case ActionsEnum.PACKINGMATERIALS:
                        SetUIEditView(commonActions);
                        if (CurrPK > 0)
                        {
                            objInvItemPackMatMapList = null;
                            EntryStatus = EntryStatus.ENTRYMODE;
                            DisplayTab = (int)TabEnum.PACKINGMATERIALS;
                            GetFieldValues(ControlsEnum.PACKINGMATERIALS);
                            SetFieldValues(ControlsEnum.PACKINGMATERIALS);
                            ResetPackMatTab();
                            txtMaterialCategory.Focus();
                        }
                        break;
                    #endregion
                    #region ADD PACKING MATERIAL ITEM
                    case ActionsEnum.ADDPACKMATITEM:
                        DisplayTab = (int)TabEnum.PACKINGMATERIALS;
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            objTempInvItemPackItemMapList = objInvItemPackMatMapList;
                            int ItemId = 0;
                            int.TryParse(hdnPackingMatID.Value, out ItemId);
                            if (objTempInvItemPackItemMapList != null && objTempInvItemPackItemMapList.Where(r => r.IMP_PACK_ITEM == ItemId && r.IMP_PACK_ITEM != PackMatProductPk).Count() > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ItemAlreadyAdded").ToString()) + "');", true);
                            }
                            else
                            {
                                if (PackMatProductPk != 0 && objTempInvItemPackItemMapList != null)
                                {
                                    objInvItemPackItemMap = objTempInvItemPackItemMapList.SingleOrDefault(itm => itm.IMP_PACK_ITEM == PackMatProductPk);
                                    if (objInvItemPackItemMap != null)
                                    {
                                        ItemId = 0;
                                        int.TryParse(hdnPackingMatID.Value, out ItemId);
                                        hdfPackMatItemPk.Value = ItemId.ToString();
                                        packingMatPK = ItemId;
                                        GetFieldValues(ControlsEnum.PACKINGMATERIALBYPK);

                                        objInvItemPackItemMap.INV_ITEM_MST1 = null;
                                        objInvItemPackItemMap.INV_ITEM_MST1 = objInvItemMst.DeepClone();
                                        objInvItemPackItemMap.IMP_PACK_ITEM = ItemId;
                                        objInvItemPackItemMap.IMP_ITEM = CurrPK;
                                        objInvItemPackItemMap.IMP_QUANITY = Convert.ToDouble(txtPackMatCount.Text);//Count
                                        objInvItemPackItemMap.IMP_CRTD_BY = currentUser.PKUser;
                                        objInvItemPackItemMap.IMP_CRTD_DT = DateTime.Now;
                                        objInvItemPackItemMap.IMP_MOD_BY = currentUser.PKUser;
                                        objInvItemPackItemMap.IMP_MOD_DT = DateTime.Now;
                                    }
                                }
                                else
                                {
                                    if (objTempInvItemPackItemMapList == null || objTempInvItemPackItemMapList.Count == 0)
                                    {
                                        objTempInvItemPackItemMapList = new List<INV_ITEM_PACK_ITEM_MAP>();
                                    }
                                    objInvItemPackItemMap = new INV_ITEM_PACK_ITEM_MAP();
                                    ItemId = 0;
                                    int.TryParse(hdnPackingMatID.Value, out ItemId);
                                    hdfPackMatItemPk.Value = ItemId.ToString();
                                    packingMatPK = ItemId;
                                    GetFieldValues(ControlsEnum.PACKINGMATERIALBYPK);

                                    objInvItemPackItemMap.INV_ITEM_MST1 = objInvItemMst;
                                    objInvItemPackItemMap.IMP_PK = 0;
                                    objInvItemPackItemMap.IMP_PACK_ITEM = ItemId;
                                    objInvItemPackItemMap.IMP_ITEM = CurrPK;
                                    objInvItemPackItemMap.IMP_QUANITY = Convert.ToDouble(txtPackMatCount.Text);//Count
                                    objInvItemPackItemMap.IMP_CRTD_BY = currentUser.PKUser;
                                    objInvItemPackItemMap.IMP_CRTD_DT = DateTime.Now;
                                    objInvItemPackItemMap.IMP_MOD_BY = currentUser.PKUser;
                                    objInvItemPackItemMap.IMP_MOD_DT = DateTime.Now;
                                    objTempInvItemPackItemMapList.Add(objInvItemPackItemMap);
                                }

                                objInvItemPackMatMapList = objTempInvItemPackItemMapList;
                                SetFieldValues(ControlsEnum.PACKINGMATERIALS);
                                ResetPackMatTab();
                                PackMatProductPk = 0;
                            }
                        }

                        break;
                    #endregion
                    #region EDIT PACKING MATERIAL ITEM MAP
                    case ActionsEnum.EDITPACKMATITEM:
                        DisplayTab = (int)TabEnum.PACKINGMATERIALS;
                        hdfPackMatPrdtPk = (HiddenField)(((ImageButton)sender).Parent.Parent as GridViewRow).FindControl("hdfPackMatPrdtPk");
                        PackMatProductPk = Convert.ToInt32(hdfPackMatPrdtPk.Value);
                        if (PackMatProductPk > 0)
                        {
                            GetUIValuesFromObject(ControlsEnum.SELECTEDPACKMATITEM);
                        }
                        break;
                    #endregion
                    #region REMOVE PACK MATERIAL ITEM MAP
                    case ActionsEnum.REMOVEPACKMATITEM:
                        DisplayTab = (int)TabEnum.PACKINGMATERIALS;
                        hdfPackMatPrdtPk = (HiddenField)(((ImageButton)sender).Parent.Parent as GridViewRow).FindControl("hdfPackMatPrdtPk");
                        PackMatProductPk = Convert.ToInt32(hdfPackMatPrdtPk.Value);
                        if (PackMatProductPk > 0)
                        {
                            GetUIValuesFromObject(ControlsEnum.REMOVEPACKMATITEM);
                            SetFieldValues(ControlsEnum.PACKINGMATERIALS);
                        }
                        PackMatProductPk = 0;
                        ResetPackMatTab();
                        break;
                    #endregion
                    #region SAVE PACKING MATERIALS
                    // Do Action for , when click save button
                    case ActionsEnum.SAVEPACKINGMATERIALS:
                        DisplayTab = (int)TabEnum.PACKINGMATERIALS;
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            if (gvPackingMaterials.Rows.Count > 0)
                            {
                                invPackMatItemMapList = new List<INV_ITEM_PACK_ITEM_MAP>();
                                InvItemMstServiceClient = new InvItemMstService();
                                InvItemMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemMstServiceClient);
                                invPackMatItemMapList = (List<INV_ITEM_PACK_ITEM_MAP>)SetUIValuesToObject(ControlsEnum.SAVEPACKMATERIALMAP);
                                result = InvItemMstServiceClient.SaveInvPackMatItemMap(invPackMatItemMapList, CurrPK);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_Pack_Mat_SaveSuccess") + "','" + Resources.Messages.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                    btnNew.Focus();
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Msg_Pack_Mat_Save").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoPackMat").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region DELETE PACKING MATERIALS
                    case ActionsEnum.DELETEPACKINGMATERIALS:
                        DisplayTab = (int)TabEnum.PACKINGMATERIALS;
                        if (gvPackingMaterials.Rows.Count > 0)
                        {
                            InvItemMstServiceClient = new InvItemMstService();
                            InvItemMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemMstServiceClient);
                            objInvItemPackItemMap = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_PACK_ITEM_MAP>();
                            objInvItemPackItemMap.IMP_ITEM = CurrPK;
                            result = InvItemMstServiceClient.DeleteInvPackMatItemMap(objInvItemPackItemMap);
                            if (result > 0)
                            {
                                ResetForm();
                                btnNew.Focus();
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_PackMat_Delete_Success").ToString() + "','" + Resources.Messages.Information + "');", true);
                            }
                            else if (result == -1) //delete reference error
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("CannotdeleteAlreadyasigned").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoPackMat").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region CLEARPACKMATITEM
                    case ActionsEnum.CLEARPACKMATITEM:
                        ResetPackMatTab();
                        DisplayTab = (int)TabEnum.PACKINGMATERIALS;
                        break;
                    #endregion

                    #endregion

                    #region SEARCHWITHPROPERTIES
                    case ActionsEnum.SEARCHWITHPROPERTIES:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSearchProducts", "ShowContainerDiv('#divSearchProducts','" + "Product Search" + "','950','400');", true);
                        break;
                    #endregion

                    #region Select
                    case ActionsEnum.SELECT:
                        // get pk from the usercontrol grid and assign to CurrPk
                        CurrPK = ucrSearchProducts.ItemPK;
                        GetFieldValues(ControlsEnum.UOM);
                        SetFieldValues(ControlsEnum.UOM);
                        GetFieldValues(ControlsEnum.PRODUCTCATEGORY);
                        SetFieldValues(ControlsEnum.PRODUCTCATEGORY);
                        GetFieldValues(ControlsEnum.PRODUCTGROUP);
                        SetFieldValues(ControlsEnum.PRODUCTGROUP);
                        GetFieldValues(ControlsEnum.PRODUCT);
                        SetFieldValues(ControlsEnum.PRODUCT);
                        GetFieldValues(ControlsEnum.GRADEPROPERTY);
                        SetFieldValues(ControlsEnum.GRADEPROPERTY);
                        GetFieldValues(ControlsEnum.PACKINGCOMBINATIONS);
                        SetFieldValues(ControlsEnum.PACKINGCOMBINATIONS);
                        txtProductName.Focus();
                        ModifiedDatePnl.Visible = true;
                        EntryStatus = EntryStatus.ENTRYMODE;
                        break;
                    #endregion
                    #region CLEARPROPSEARCH
                    case ActionsEnum.CLEARSEARCHPROPERTIES:
                        ucrSearchProducts.ResetForm();
                        break;
                    #endregion
                    #region Store Mapping Tab
                    case ActionsEnum.STOREMAPPING:
                        SetUIEditView(ActionsEnum.STOREMAPPING);
                        if (CurrPK > 0)
                        {
                            GetFieldValues(ControlsEnum.STOREMAPPING);
                            SetFieldValues(ControlsEnum.STOREMAPPING);
                        }
                        break;
                    #endregion 
                    #region Lines Tab
                    case ActionsEnum.LINES:
                        SetUIEditView(ActionsEnum.LINES);
                        if (CurrPK > 0)
                        {
                            GetFieldValues(ControlsEnum.LINES);
                            SetFieldValues(ControlsEnum.LINES);

                            // Get And Set the Location details  
                            hdfRelItemPk.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.PRODUCTBYPK);
                            if (objInvItemMst != null)
                            {
                                lbllneProductCodeDisplay.ToolTip = HttpUtility.HtmlDecode(objInvItemMst.ITM_CODE);
                                lbllneProductCodeDisplay.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objInvItemMst.ITM_CODE), 90);
                                lbllnePrdNameDisplay.ToolTip = HttpUtility.HtmlDecode(objInvItemMst.ITM_NAME);
                                lbllnePrdNameDisplay.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objInvItemMst.ITM_NAME), 115);
                                #region Show the grade of main product in main header
                                if (dtProductGrade == null || dtProductGrade.Rows.Count == 0)
                                    GetFieldValues(ControlsEnum.PRODUCTGRADES);
                                if (dtProductGrade != null || dtProductGrade.Rows.Count > 0)
                                {
                                    var prdtGrade = (from r in dtProductGrade.AsEnumerable()
                                                     where r.Field<byte>("CFG_VALUE") == Convert.ToByte(objInvItemMst.ITM_GRADE)
                                                     select new
                                                     {
                                                         GradeName = r.Field<string>("CFG_DATA")
                                                     }).ToList();
                                    if (prdtGrade != null && prdtGrade.Count > 0)
                                        lbllnePrdtGradeDisplay.Text = lbllnePrdtGradeDisplay.ToolTip = HttpUtility.HtmlDecode(prdtGrade[0].GradeName);
                                }
                                #endregion
                            }
                        }
                        break;
                    #endregion
                    #region SAVE Store Mapping
                    case ActionsEnum.SAVEPRODSTOREMAP:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            ProductStoreSaveBO objSave = new ProductStoreSaveBO();
                            objSave = (ProductStoreSaveBO)SetUIValuesToObject(ControlsEnum.STOREMAPPING);
                            string xml = CommonFunctions.XmlSerialize<ProductStoreSaveBO>(objSave);
                            result = BusinessLogic.Inventory.ProductsBL.SaveStoreMap(CommonFunctions.XmlSerialize<ProductStoreSaveBO>(objSave));
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Msg_PrdtStoreMapping_SaveSuccess") + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                btnNew.Focus();
                            }
                            else
                            {
                                DisplayTab = (int)TabEnum.LINES;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Msg_PrdtLine_SaveFail").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region SAVE GRADE PRODUCTS
                    // Do Action for , when click save button
                    case ActionsEnum.SAVEPRODLINEMAP:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            ProductLineSaveBO objSave = new ProductLineSaveBO();
                            objSave = (ProductLineSaveBO)SetUIValuesToObject(ControlsEnum.LINES);
                            result = BusinessLogic.Inventory.ProductsBL.SaveLineItemMap(CommonFunctions.XmlSerialize<ProductLineSaveBO>(objSave));
                            if (result >= 0) // Success ! re-initialize the page
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Msg_PrdtLine_SaveSuccess") + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                btnNew.Focus();
                            }
                            else
                            {
                                DisplayTab = (int)TabEnum.LINES;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Msg_PrdtLine_SaveFail").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    //#region PRINT
                    //case ActionsEnum.PRINT:
                    //    GetFieldValues(ControlsEnum.PRODUCTDOC);
                    //    SetFieldValues(ControlsEnum.PRODUCTDOC);
                    //    break;
                    //#endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                invItemMstObj = null;
                invItemMstList = null;
                InvItemMstServiceClient = null;
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
                if (((GridView)sender).ID == "grdProductList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Label lblItemSize = e.Row.FindControl("lblItemSize") as Label;
                        Label lblItemLength = e.Row.FindControl("lblItemLength") as Label;
                        ImageButton btnBrand = e.Row.FindControl("imbGenerateBrand") as ImageButton;
                        HiddenField hdfprohasbrandprd=e.Row.FindControl("hdfprohasbrandprd") as HiddenField;
                        hdfProductPK = e.Row.FindControl("hdfProductPK") as HiddenField;
                        if (GetGlobalResourceObject("ConfigurationsRes", "ShowPRDGenerateButton").ToString() == "1" && hdfprohasbrandprd.Value=="0")
                        {
                            btnBrand.Visible = true;
                        }
                        else
                        {
                            btnBrand.Visible = false;
                        }

                        if (invItemMstList != null && invItemMstList.Count > 0)
                        {
                            int itemPK = invItemMstList[e.Row.RowIndex].ITM_PK;
                            invItemSpecDtlList = invItemMstList[e.Row.RowIndex].INV_ITEM_SPEC_DTL.Where(c => c.ISD_ITEM == itemPK).ToList();

                            if (invItemSpecDtlList != null && invItemSpecDtlList.Count > 0)
                            {
                                lblItemSize.Text = invItemSpecDtlList[0].ADM_CONST_MST16 != null ? invItemSpecDtlList[0].ADM_CONST_MST16.CON_CODE + "-" + invItemSpecDtlList[0].ADM_CONST_MST16.CON_NAME : string.Empty;
                                lblItemSize.ToolTip = invItemSpecDtlList[0].ADM_CONST_MST16 != null ? invItemSpecDtlList[0].ADM_CONST_MST16.CON_CODE + "-" + invItemSpecDtlList[0].ADM_CONST_MST16.CON_NAME : string.Empty;

                                lblItemLength.Text = invItemSpecDtlList[0].ADM_CONST_MST17 != null ? invItemSpecDtlList[0].ADM_CONST_MST17.CON_CODE + "-" + invItemSpecDtlList[0].ADM_CONST_MST17.CON_NAME : string.Empty;
                                lblItemLength.ToolTip = invItemSpecDtlList[0].ADM_CONST_MST17 != null ? invItemSpecDtlList[0].ADM_CONST_MST17.CON_CODE + "-" + invItemSpecDtlList[0].ADM_CONST_MST17.CON_NAME : string.Empty;
                            }

                        }

                        //Product Grade Name
                        Label lblProductGrade = e.Row.FindControl("lblProductGrade") as Label;
                        HiddenField hdfProductGrade = e.Row.FindControl("hdfProductGrade") as HiddenField;
                        if (dtProductGrade == null || dtProductGrade.Rows.Count == 0)
                            GetFieldValues(ControlsEnum.PRODUCTGRADES);
                        if (dtProductGrade != null || dtProductGrade.Rows.Count > 0)
                        {
                            if (hdfProductGrade.Value != "")
                            {
                                var prdtGrade = (from r in dtProductGrade.AsEnumerable()
                                                 where r.Field<byte>("CFG_VALUE") == Convert.ToByte(hdfProductGrade.Value)
                                                 select new
                                                 {
                                                     GradeName = r.Field<string>("CFG_DATA")
                                                 }).ToList();
                                if (prdtGrade != null && prdtGrade.Count > 0)
                                    lblProductGrade.Text = lblProductGrade.ToolTip = HttpUtility.HtmlDecode(prdtGrade[0].GradeName);
                            }
                            else
                            {
                                lblProductGrade.Text = lblProductGrade.ToolTip = "";
                            }
                        }

                        if (GetGlobalResourceObject("ConfigurationsRes", "ShowProductSubCategory").ToString().ToLower() == "true")
                        {
                            //Product Subcategory
                            Label lblProdSubCategory = e.Row.FindControl("lblProdSubCategory") as Label;
                            HiddenField hdfProdSubCategory = e.Row.FindControl("hdfProdSubCategory") as HiddenField;
                            if (dtSubType == null || dtSubType.Rows.Count == 0)
                                GetFieldValues(ControlsEnum.SUBTYPE);
                            if (dtSubType != null || dtSubType.Rows.Count > 0)
                            {
                                var subCategory = (from r in dtSubType.AsEnumerable()
                                                   where r.Field<byte>("CFG_VALUE") == Convert.ToByte(hdfProdSubCategory.Value)
                                                   select new
                                                   {
                                                       subType = r.Field<string>("CFG_DATA")
                                                   }).ToList();
                                if (subCategory != null && subCategory.Count > 0)
                                    lblProdSubCategory.Text = lblProdSubCategory.ToolTip = HttpUtility.HtmlDecode(subCategory[0].subType);
                            }
                        }

                        HiddenField hdfMappedCount = e.Row.FindControl("hdfMappedCount") as HiddenField;
                        Button imgMapped = e.Row.FindControl("imgMapped") as Button;
                        if (hdfMappedCount.Value == "0")
                        {
                            imgMapped.CssClass = GetLocalResourceObject("NotMappedIcon").ToString();
                            imgMapped.ToolTip = GetLocalResourceObject("RelatedProdNotMapped").ToString();
                        }
                        else
                        {
                            imgMapped.CssClass = GetLocalResourceObject("MappedIcon").ToString();
                            imgMapped.ToolTip = GetLocalResourceObject("RelatedProdMapped").ToString();
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                    }
                }
                else if (((GridView)sender).ID == "grdRelatedPrdts")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Label lblPrdtGrade = e.Row.FindControl("lblPrdtGrade") as Label;
                        HiddenField hdfPrdtGrade = e.Row.FindControl("hdfPrdtGrade") as HiddenField;
                        if (dtProductGrade == null || dtProductGrade.Rows.Count == 0)
                            GetFieldValues(ControlsEnum.PRODUCTGRADES);
                        if (dtProductGrade != null || dtProductGrade.Rows.Count > 0)
                        {
                            var prdtGrade = (from r in dtProductGrade.AsEnumerable()
                                             where r.Field<byte>("CFG_VALUE") == Convert.ToByte(hdfPrdtGrade.Value)
                                             select new
                                             {
                                                 GradeName = r.Field<string>("CFG_DATA")
                                             }).ToList();
                            if (prdtGrade != null && prdtGrade.Count > 0)
                                lblPrdtGrade.Text = lblPrdtGrade.ToolTip = HttpUtility.HtmlDecode(prdtGrade[0].GradeName);
                        }
                    }
                }
                else if (((GridView)sender).ID == "gvSubTypeProducts")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Label lblSubCategory = e.Row.FindControl("lblSubCategory") as Label;
                        HiddenField hdfSubCategory = e.Row.FindControl("hdfSubCategory") as HiddenField;
                        if (dtSubType == null || dtSubType.Rows.Count == 0)
                            GetFieldValues(ControlsEnum.SUBTYPE);
                        if (dtSubType != null || dtSubType.Rows.Count > 0)
                        {
                            var subCategory = (from r in dtSubType.AsEnumerable()
                                               where r.Field<byte>("CFG_VALUE") == Convert.ToByte(hdfSubCategory.Value)
                                               select new
                                               {
                                                   subType = r.Field<string>("CFG_DATA")
                                               }).ToList();
                            if (subCategory != null && subCategory.Count > 0)
                                lblSubCategory.Text = lblSubCategory.ToolTip = HttpUtility.HtmlDecode(subCategory[0].subType);
                        }
                    }
                }
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

        /// <summary>
        /// item databound Event Handler for datalist 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandlerGrade(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {

                Label lblGradeChecked = (Label)e.Item.FindControl("lblGradeChecked");
                CheckBox chkGrade = (CheckBox)e.Item.FindControl("chkGrade");
                HiddenField hdnGradeValue = (HiddenField)e.Item.FindControl("hdnGradeValue");
                if (hdnGradeValue.Value == "1")
                {
                    chkGrade.Checked = true;
                    chkGrade.Enabled = false;
                }
                else
                {
                    if (lblGradeChecked.Text == "true")
                    {
                        chkGrade.Checked = true;
                    }
                    else
                    {
                        //chkGrade.Checked = false;
                        chkGrade.Checked = true;
                    }
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

            GetFieldValues(ControlsEnum.CONTROLS);
            SetFieldValues(ControlsEnum.CONTROLS);
        }

        private void ConfigSettings()
        {
            if (GetGlobalResourceObject("ConfigurationsRes", "ShowRateInBrandProducts").ToString().ToLower() == "1")
            {
                tblRate.Visible = true;
            }
            if (GetGlobalResourceObject("ConfigurationsRes", "ShowProductSubCategory").ToString().ToLower() == "true")
            {
                spnRelatedProducts.Visible = true;
                divProdSubCategory.Visible = true;
                vrfProductSubCategory.Enabled = true;
                grdProductList.Columns[3].Visible = true;
                divAdvProdSubCategory.Visible = true;
            }
            else
            {
                spnRelatedProducts.Visible = false;
                divProdSubCategory.Visible = false;
                vrfProductSubCategory.Enabled = false;
                grdProductList.Columns[3].Visible = false;
                divAdvProdSubCategory.Visible = false;
            }
            if (GetGlobalResourceObject("ConfigurationsRes", "ShowPackCombination").ToString() == "1")
            {
                divCombination.Visible = true;
            }
            else
            {
                divCombination.Visible = false;
            }
            if (GetGlobalResourceObject("ConfigurationsRes", "ShowInventoryProduct").ToString() == "1")
            {
                divInvProduct.Visible = true;
            }
            else
            {
                divInvProduct.Visible = false;
            }
            if (GetGlobalResourceObject("ConfigurationsRes", "ShowPackingMaterialMapping").ToString() == "1")
            {
                spnPackingMaterials.Visible = true;
            }
            else
            {
                spnPackingMaterials.Visible = false;
            }
            if (GetGlobalResourceObject("ConfigurationsRes", "ShowPlanningGroup").ToString() == "1")
            {
                lblPlanGroup.Visible = true;
                ddlPlanGroup.Visible = true;
            }
            else
            {
                lblPlanGroup.Visible = false;
                ddlPlanGroup.Visible = false;
            }
            if (GetGlobalResourceObject("ConfigurationsRes", "ShowPlanGroupValidator").ToString() == "1")
            {
                vrfddlPlanGroup.Enabled = true;
            }
            else
            {
                vrfddlPlanGroup.Enabled = false;
            }
            hdfProdSpecCompare.Value = GetGlobalResourceObject("ConfigurationsRes", "CompareProductSpecs").ToString();
            hdfIsProductInSBU.Value = GetGlobalResourceObject("ConfigurationsRes", "IsProductInSBU").ToString(); 
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);

            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AutoGenerateChecked", "AutoGenerateChecked();", true);
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                if (DisplayTab == (int)TabEnum.SUBTYPEPRODUCTS)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(2);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(3);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
                }
                else if (DisplayTab == (int)TabEnum.SUBGRADEPRODUCTS)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(3);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(4);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(3);", true);
                }
                else if (DisplayTab == (int)TabEnum.PACKINGMATERIALS)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(4);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(5);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(4);", true);
                }
                else if (DisplayTab == (int)TabEnum.LINES)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(5);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(6);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(5);", true);
                }
                else if (DisplayTab == (int)TabEnum.STOREMAPPING)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(6);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(7);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(6);", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(1);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
                }
             
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(0);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(0);", true);
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            if (IsBrandProduct)
                BindPageTitle(9);
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
                ucrSearchProducts.SELECT += new EventHandler(ActionHandler);
                if (!IsPostBack)
                {
                    pnlPrint.Visible = false;
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.ItemPK;
                    grdProductList.DataKeyNames = datakeyarray;
                    SortBy = Resources.DataFieldRes.ItemCode;
                    SortDirection = Resources.Report.SortAscending;
                    EntryStatus = EntryStatus.LISTMODE;
                    if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "9")//Brand Product
                    {
                        IsBrandProduct = true;
                        hdfIsBrandProduct.Value = "1";
                        liGrade.Visible = liPacking.Visible = liLines.Visible = false;
                    }
                    ConfigSettings();
                    GetFieldValues(ControlsEnum.PRODUCTGROUP);
                    SetFieldValues(ControlsEnum.PRODUCTGROUP);
                    GetFieldValues(ControlsEnum.GRADEPROPERTY);
                    SetFieldValues(ControlsEnum.GRADEPROPERTY);
                    BindDDLNoCompounds();
                    GetFieldValues(ControlsEnum.PRODUCTGRADES);
                    SetFieldValues(ControlsEnum.MASTERPRODUCTGRADES);
                    GetFieldValues(ControlsEnum.SUBTYPE);
                    SetFieldValues(ControlsEnum.PRODUCTSUBCATEGORY);
                    GetFieldValues(ControlsEnum.BASEDONCOMBINATIONS);
                    SetFieldValues(ControlsEnum.BASEDONCOMBINATIONS);
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);


                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

       
        private void BindPageTitle(int category)
        {
            string breadCrumb;
            if (category == 9)
            {
                Page.Title = Resources.Captions.Title_BrandProduct;
                breadCrumb = this.GetLocalResourceObject("BreadcrumbBrand").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                lblBreadCrum.Text = breadCrumb;
            }
        }

        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            PRODUCT,
            UOM,
            PRODUCTCATEGORY,
            CONTROLS,
            BINDDROPDOWN,
            PRODUCTGROUP,
            GRADEPROPERTY,
            PRODUCTGRADES,
            SUBGRADEPRODUCTS,
            GRADEPRODUCTSADD,
            SELECTEDRELITEM,
            PRODUCTBYPK,
            REMOVEITEM,
            SAVEGRADEPRODUCTS,
            MASTERPRODUCTGRADES,
            SUBTYPE,
            RELATEDPRODUCTS,
            SAVERELPRODUCTS,
            SAVEPACKMATERIALMAP,
            RELATEDPRODUCTSADD,
            SELECTEDSUBTYPEITEM,
            REMOVESUBTYPEITEM,
            PRODUCTSUBCATEGORY,
            BASEDONCOMBINATIONS,
            PACKINGCOMBINATIONS,
            PACKINGMATERIALS,
            SELECTEDPACKMATITEM,
            REMOVEPACKMATITEM,
            PACKINGMATERIALBYPK,
            PLANGROUP,
            LINES,
            PRODUCTDOC,
            GSTCLASS,
            PACKINGSPEC,
            STOREMAPPING
        }
        #endregion

        #region TabEnum
        public enum TabEnum
        {
            PRODUCTDETAILS = 1,
            SUBTYPEPRODUCTS = 2,
            SUBGRADEPRODUCTS = 3,
            PACKINGMATERIALS = 4,
            LINES = 5,
            STOREMAPPING=6
        }
        #endregion

        #region ProductGradeEnum
        public enum ProductGradeEnum
        {
            BGrade = 1,
            Scrap = 2,
            AlernateProduct = 3,
            AGrade = 4
        }
        #endregion
    }
}