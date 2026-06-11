using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.MaterialManagement;
using BusinessLogic.CommonManagement;
using BusinessObject.MaterialManagement;
using BusinessObject.CommonManagement;


namespace ERPSMS_v01.GeneralAdmin
{
    public partial class OrderItem : ERP.Store.UI.MyBasePage
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
                return this.ViewState[ViewstateStrings.CurrPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }

        private int SelectSlno
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedSlno] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedSlno]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedSlno] = value;
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

        /// <summary>
        /// To Keep Work Order Item
        /// </summary>
        private WorkOrderBomBO WorkOrderItem
        {
            get
            {
                return this.ViewState["WorkOrderItem"] == null ? null : (WorkOrderBomBO)this.ViewState["WorkOrderItem"];
            }
            set
            {
                this.ViewState["WorkOrderItem"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private List<BOMDetails> lstBomDetails
        {
            get
            {
                return this.ViewState["BomDetailsList"] == null ? null : (List<BOMDetails>)this.ViewState["BomDetailsList"];
            }
            set
            {
                this.ViewState["BomDetailsList"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int HasPackingMaterial
        {
            get
            {
                return this.ViewState["HasPackingMaterial"] == null ? 0 : Convert.ToInt32(this.ViewState["HasPackingMaterial"]);
            }
            set
            {
                this.ViewState["HasPackingMaterial"] = value;
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
        #endregion
        private ActionsEnum commonActions;
        BusinessObject.User currentUser;
        DataSet dsResult;
        DataTable dtResult;


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
        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            // this.btnCanel.PreRender += new EventHandler(btnAction_PreRender);
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //base.CheckBtnVisibility(sender);
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
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
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
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "InitComponents();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);

                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                }
                else if (EntryStatus == EntryStatus.ALLOCATEMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(2);", true);
                }


                //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitDate1", "$(document).ready(function () {InitDate();});", true);
            }
            catch (Exception ex)
            {
                throw ex;
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
            //InvItemMstServiceClient = new InvItemMstService();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {

                switch (type)
                {
                    //Used Case to swich using Controls Enum in this Name Space
                    case ControlsEnum.DEFAULT:
                        GridPrams objSearch = new GridPrams();
                        objSearch.PageNumber = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        objSearch.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        int WoItemType = Convert.ToInt32(hdfItemType.Value) == 0 ? 1 : Convert.ToInt32(hdfItemType.Value);//ddl_ItemType.SelectedValue
                        if ((Convert.ToInt32(MaterialPK.Value) > 0) || (Convert.ToInt32(hdfProductPK.Value) > 0))
                        {
                            objSearch.SearchBy = "ITM_PK";
                            if (WoItemType == 1)
                                objSearch.SearchValue = MaterialPK.Value;
                            else if (WoItemType == 2)
                                objSearch.SearchValue = hdfProductPK.Value;
                        }
                        else
                        {
                            objSearch.SearchBy = "0";
                            objSearch.SearchValue = "%%";
                        }
                        objSearch.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.ItemCode : SortBy;
                        objSearch.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        hdfSelectCatpak.Value = MaterialCategoryPK.Value;
                        hdfSelectCat.Value = MaterialCategory.Text;
                        hdfSelectMaterial.Value = ItemCodeMaterial.Text;
                        hdfSelectMaterialPK.Value = MaterialPK.Value;
                        int ItemIsMapped = Convert.ToByte(ChkMappedItem.Checked == true ? Convert.ToByte(DbActiveStatus.ACTIVE) : Convert.ToByte(DbActiveStatus.INACTIVE));
                        int subCategory = 0;
                        int customer = 0;
                        int PackingCustomer = 0;
                        if (Convert.ToInt32(ddl_ItemType.SelectedValue) == 2)
                            PackingCustomer = hdfCustomerSearchPK.Value == "" ? 0 : Convert.ToInt32(hdfCustomerSearchPK.Value);
                        int brand = 0;
                        int isworkorder = 1;

                        if (WoItemType == 2)
                        {
                            subCategory = Convert.ToInt32(ddl_matcat.SelectedValue) > 0 ? Convert.ToInt32(ddl_matcat.SelectedValue) : subCategory;
                            isworkorder = 0;
                        }
                        else if (WoItemType == 3)
                        {
                            customer = Convert.ToString(hdfCustomerID.Value) == "" ? customer : Convert.ToInt32(hdfCustomerID.Value);
                            brand = Convert.ToInt32(hdfBrand.Value) > 0 ? Convert.ToInt32(hdfBrand.Value) : brand;
                            isworkorder = 0;
                        }
                        dsResult = BusinessLogic.MaterialManagement.MaterialMaster.GetWOMaterialList(objSearch, currentUser.SBUID, Convert.ToInt32(ddlStatus.SelectedValue), Convert.ToInt32(MaterialCategoryPK.Value), isworkorder, WoItemType, ItemIsMapped, subCategory, customer, brand, PackingCustomer);
                        break;
                    case ControlsEnum.OPERATON:
                        dtResult = CommonBL.GetAppConfig(currentUser.SBUID, "WO OPERATION");
                        break;
                    case ControlsEnum.BOMTYPE:
                        dtResult = CommonBL.GetAppConfig(currentUser.SBUID, "BOM TYPE");
                        break;
                    case ControlsEnum.BOMDETAILS:
                        string XmlResult = BusinessLogic.MaterialManagement.MaterialMaster.GetOrderItem(CurrPK, Convert.ToInt32(hdfItemType.Value));

                        if (XmlResult != "")
                        {
                            WorkOrderItem = CommonFunctions.XmlDeserialize<WorkOrderBomBO>(XmlResult);
                        }
                        else
                        {
                            WorkOrderItem = null;
                            lstBomDetails = null;
                            txtQuantity.Text = "1";
                        }

                        break;
                    case ControlsEnum.UOM:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAllUOMList(currentUser.SBUID, Convert.ToInt32(CommonConstants.ACTIVE), null, null, null);
                        break;
                    case ControlsEnum.ITEMTYPE:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "WO ITEM TYPE");
                        break;
                    case ControlsEnum.ITEMCATEGORY:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PRODUCT SUB TYPE");
                        break;


                }
            }
            catch (Exception ex)
            {
                //employeeServiceClient.Abort();
                throw ex;
            }
            finally
            {
                //employeeMstObj = null;
                //serviceUtilityObj = null;
                //employeeServiceClient = null;
            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// </summary>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.BOMTYPE:
                    case ControlsEnum.OPERATON:
                    case ControlsEnum.UOM:
                    case ControlsEnum.ITEMTYPE:
                    case ControlsEnum.ITEMCATEGORY:
                    case ControlsEnum.ITEMCATEGORYBOM:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.BOMDETAILS:
                        GetUIValuesFromObject();
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

        private void ConfigurationSettings()
        {
            //divCompany.Visible = GetConfigData().IsMultiplePlant;
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {

                hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }

            //hdfIsShowInvNo.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowInvNo").ToString();
            //if (hdfIsShowInvNo.Value == "0")
            //{
            //    lblInvNo.Visible = false;
            //    txtInvNo.Visible = false;
            //    grdSoList.Columns[11].Visible = false;
            //}
        }
        private void ActivateWOItemList()
        {
            spnBOM.Attributes["class"] = "tab-inactive";
            spnWOItemList.Attributes["class"] = "tab-active";
            lbnBOM.CssClass = "tab-inactive";
            lbnWorkOrderItems.CssClass = "tab-active";
            spnPackingSpecsMapping.Attributes["class"] = "tab-inactive";
            lblPackingSpecsMapping.CssClass = "tab-inactive";
        }

        private void ActivateBOMDetails()
        {
            spnBOM.Attributes["class"] = "tab-active";
            spnWOItemList.Attributes["class"] = "tab-inactive";
            lbnBOM.CssClass = "tab-active";
            lbnWorkOrderItems.CssClass = "tab-inactive";
            spnPackingSpecsMapping.Attributes["class"] = "tab-inactive";
            lblPackingSpecsMapping.CssClass = "tab-inactive";
        }
        private void ActivatePackMatMapping()
        {
            spnPackingSpecsMapping.Attributes["class"] = "tab-active";
            spnWOItemList.Attributes["class"] = "tab-inactive";
            spnBOM.Attributes["class"] = "tab-inactive";
            lblPackingSpecsMapping.CssClass = "tab-active";
            lbnBOM.CssClass = "tab-inactive";
            lbnWorkOrderItems.CssClass = "tab-inactive";
        }
        private void ClearSelection()
        {
            
            ItemCodeMaterial.Text = "Select/Type";
            //MaterialPK.Value = "0";
            hdfSelectCat.Value = string.Empty;
            hdfSelectCatpak.Value = "0";         
            MaterialPK.Value = "0";
            hdfSelectItem.Value = string.Empty;
            hdfSelectItemPK.Value = "0";
            MaterialCategory.Text= "Select/Type";
            hdfBOMCategoryPK.Value = "0";
            ItemCodeMaterial.Text= "Select/Type";
            MaterialCategoryPK.Value = "0";
            ddlStatus.SelectedIndex = 1;
            ddl_ItemType.SelectedIndex = 0;
            //ddl_matcat.SelectedIndex = 0;
            txtCustomer.Text= "Select/Type";
            hdfCustomerID.Value = "0";
            txtItemProduct.Text= "Select/Type";
            hdfProductPK.Value = "0";
            txtBrandProduct.Text = "Select/Type";
            hdfBrand.Value = "0";
            hdfItemType.Value = "1";
            ShowHideSearchComtrol(Convert.ToInt32(ddl_ItemType.SelectedValue));
            ChkMappedItem.Checked = true;
            txtCustomerSearch.Text = "Select/Type";
            hdfCustomerSearchPK.Value = "0";
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
        }
        private void ClearBOMControls()
        {
            ///ddlOperation.SelectedIndex = 0;
            ddlWOItemType.SelectedIndex = 0;
            txtBOMCategory.Text = "Select/Type";
            txtBOMMaterial.Text = "Select/Type";
            txtCustomerBOM.Text = "Select/Type";
            txtBrandProductBOM.Text = "Select/Type";
            if (ddlProductcatBOM.SelectedIndex > 0)
                ddlProductcatBOM.SelectedIndex = 0;
            txtItemProductBOM.Text = "Select/Type";
            txtPMCategory.Text = "Select/Type";
            txtPMItem.Text = "Select/Type";
            txtUOM.Text = string.Empty;
            txtIssue_Qty.Text = string.Empty;
            dddlIssueUom.SelectedIndex = 0;
            txtTolerance.Text = string.Empty;
            ddlBOMType.SelectedIndex = 0;

            txtCustomerID.Text = "Select/Type";
            hdfCustomerPK.Value = "0";
            txtBrandID.Text = "Select/Type";
            hdfBrandID.Value = "0";
            hdfbrandcode.Value = "0";
        }
        private void ClearPackMapControls()
        {
            hdfSelectPMMCatPk.Value = "0";
            hdfSelectPMMCat.Value = string.Empty;
            hdfSelectPMMMatPk.Value = "0";
            hdfSelectPMMMat.Value = string.Empty; ;
            //ddlPMMOperationWork.SelectedIndex = 0;
            txtPMMCategory.Text = "Select/Type";
            txtPMMItem.Text = "Select/Type";
            hdfPMMCatPK.Value = "0";
            hdfPMMItemPK.Value = "0";
            hdfPMMUOMPK.Value = "0";
            txtPMMUOM.Text = string.Empty;
            txtPMMIssueQty.Text = string.Empty;
            ddlPMMIssueUom.SelectedIndex = 0;
            ddlPMMType.SelectedIndex = 0;
            txtPMMTolerance.Text = string.Empty;
            txtPMMQty.Text = string.Empty;
            hdfSelectedPMMCATTEXT.Value = string.Empty;
            hdfSelectedPMMCATPK.Value = "0";
            hdfSelectedPMMTEXT.Value = string.Empty;
            hdfSelectedPMMPK.Value = "0";
            hdfSelectedCustomerPK.Value = "0";
            hdfSelectedCustomer.Value = string.Empty; ;
            hdfSelectedBrand.Value = string.Empty; ;
            hdfSelectedBrandPK.Value = "0";

        }
        private void ClearFields()
        {
            hdfSelectBOMCatPk.Value = "0";
            hdfSelectBOMCat.Value = string.Empty;
            hdfSelectBOMMatPk.Value = "0";
            hdfSelectBOMMat.Value = string.Empty;
            txtQty.Text = string.Empty;
           // ddlOperation.SelectedIndex = 0;
            txtUOM.Text = string.Empty;
            txtIssue_Qty.Text = string.Empty;
            dddlIssueUom.SelectedIndex = 0;
            //ddlWOItemType.SelectedIndex = 0;
            txtTolerance.Text = string.Empty;
            SelectSlno = 0;
            hdfSelectedCatpak.Value = "0";
            hdfSelectedCat.Value = string.Empty;
            hdfSelectedMaterialPK.Value = "0";
            hdfSelectedMaterial.Value = string.Empty;
            //hdfItemType.Value = "0";
            ddlBOMType.SelectedIndex = 0;
            txtBOMCategory.Text = "Select/Type";
            txtBOMMaterial.Text = "Select/Type";
            txtCustomerBOM.Text = "Select/Type";
            txtBrandProductBOM.Text = "Select/Type";
            //ddlProductcatBOM.SelectedIndex = 0;
            ddlProductcatBOM.SelectedIndex = Convert.ToInt32(ddlProductcatBOM.Items.IndexOf(ddlProductcatBOM.Items.FindByValue("0")));
            txtItemProductBOM.Text = "Select/Type";
            txtPMCategory.Text = "Select/Type";
            txtPMItem.Text = "Select/Type";
          

        }
        /// <summary>
        /// Is Same Item Exist in List
        /// </summary>
        /// <param name="itemPK"></param>
        /// <returns></returns>
        private bool IsSameItemExist(string type)
        {
            bool result = false;
            int itempk = 0, itemcat = 0;
            if (type == "1")
            {
                itemcat = Convert.ToInt32(hdfBOMCategoryPK.Value);
                itempk = Convert.ToInt32(hdfBOMMaterialPK.Value);

            }
            if (type == "2")
            {
                itemcat = Convert.ToInt32(ddlProductcatBOM.SelectedValue);
                itempk = Convert.ToInt32(hdfProductPKBOM.Value);
            }
            if (type == "3")
            {
                itemcat = Convert.ToInt32(hdfCustomerIDBOM.Value);
                itempk = Convert.ToInt32(hdfProductPKBOM.Value);
            }
            if (type == "4")
            {
                itemcat = Convert.ToInt32(hdfPMCatgoryPK.Value);
                itempk = Convert.ToInt32(hdfPMItem.Value);
            }
            if (lstBomDetails == null)
                result = false;
            else
                result = lstBomDetails.Count(m => m.BOMaterialPK == itempk && m.BomMaterialCat == itemcat&& m.IsPrdMapp == 0) > 0;
            return result;

        }
        /// <summary>
        /// Is Same Item Exist in List
        /// </summary>
        /// <param name="itemPK"></param>
        /// <returns></returns>
        private bool IsSameItemExistInPM(int type)
        {
            bool result = false;
            int itempk = 0, itemcat = 0;
            int customer = 0;
            if (type == 4)
            {
                itemcat = Convert.ToInt32(hdfPMMCatPK.Value);
                itempk = Convert.ToInt32(hdfPMMItemPK.Value);
                customer = Convert.ToInt32(hdfCustomerPK.Value);
            }
            if (lstBomDetails == null)
                result = false;
            else
                result = lstBomDetails.Count(m => m.BOMaterialPK == itempk 
                && m.BomMaterialCat == itemcat 
                && m.IsPrdMapp==1
                && m.CustomerPK==customer) > 0;
            return result;

        }

        /// <summary>
        /// Configuration Settings
        /// </summary>
        private void SetConfigValue()
        {

        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>   
        /// 
        private Object SetUIValuesToObject(ControlsEnum setType)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            object retObject;
            retObject = null;
            try
            {
                switch (setType)
                {
                    case ControlsEnum.SAVE:
                        WorkOrderBomBO tempWorkOrderItem = new WorkOrderBomBO();
                        tempWorkOrderItem.BizUnit = currentUser.SBUID;
                        tempWorkOrderItem.UserPK = currentUser.PKUser;
                        tempWorkOrderItem.WomItem = CurrPK;
                        tempWorkOrderItem.Details = lstBomDetails;
                        tempWorkOrderItem.Bom_Uom = Convert.ToInt32(hdfWOUomPk.Value);
                        tempWorkOrderItem.Bom_UomText = lblUOM.Text;
                        tempWorkOrderItem.Bom_Qty = decimal.Parse(txtQuantity.Text);
                        tempWorkOrderItem.WOMModOn = LastModifiedTime;
                        tempWorkOrderItem.WOMModBy = Convert.ToInt16(currentUser.PKUser);
                        tempWorkOrderItem.WOMItemType = Convert.ToInt32(hdfItemType.Value);//ddl_ItemType.SelectedValue
                        retObject = tempWorkOrderItem;
                        break;

                    case ControlsEnum.PACKMAPLIST:
                        BOMDetails objBOMDetail = new BOMDetails();
                        int slno = 1;
                        if (lstBomDetails == null || lstBomDetails.Count == 0)
                            lstBomDetails = new List<BOMDetails>();
                        else
                            slno = lstBomDetails.Max(m => m.SlNo) + 1;
                        objBOMDetail.Operation = Convert.ToInt32(ddlPMMOperationWork.SelectedValue);
                        objBOMDetail.OperationText = ddlPMMOperationWork.SelectedItem.Text;
                        objBOMDetail.BomType = Convert.ToInt32(ddlPMMType.SelectedValue);
                        objBOMDetail.BomTypeText = ddlPMMType.SelectedItem.Text;
                        objBOMDetail.UOM = Convert.ToInt32(hdfPMMUOMPK.Value);
                        objBOMDetail.UomText = txtPMMUOM.Text;
                        objBOMDetail.QTY = Decimal.Parse(txtPMMQty.Text);
                        objBOMDetail.BOMaterialPK = Convert.ToInt32(hdfPMMItemPK.Value);
                        objBOMDetail.BOMaterial = txtPMMItem.Text;
                        objBOMDetail.BomMaterialCatText = txtPMMCategory.Text;
                        objBOMDetail.BomMaterialCat = Convert.ToInt32(hdfPMMCatPK.Value);
                        objBOMDetail.BomItemType = 4;
                        objBOMDetail.BomItemTypeText = ddlWOItemType.Items[3].Text;//packing material
                        objBOMDetail.IssueQty = decimal.Parse(txtPMMIssueQty.Text);
                        objBOMDetail.IssueUOM = Convert.ToInt32(ddlPMMIssueUom.SelectedValue);
                        objBOMDetail.IssueUomText = ddlPMMIssueUom.SelectedItem.Text;
                        objBOMDetail.Tolerance = decimal.Parse(txtPMMTolerance.Text);
                        objBOMDetail.SlNo = slno;
                        objBOMDetail.IsPrdMapp = 1;
                        objBOMDetail.CustomerName = txtCustomerID.Text;
                        objBOMDetail.CustomerPK = Convert.ToInt16(hdfCustomerPK.Value);
                        objBOMDetail.BrandName = txtBrandID.Text;
                        objBOMDetail.BrandPK = Convert.ToInt16(hdfBrandID.Value);
                        objBOMDetail.BrandCode = hdfbrandcode.Value;
                        lstBomDetails.Add(objBOMDetail);
                        break;
                    case ControlsEnum.BOMADD:

                        BOMDetails objBOMDetails = new BOMDetails();
                        objBOMDetails.Operation = Convert.ToInt32(ddlOperation.SelectedValue);
                        objBOMDetails.OperationText = ddlOperation.SelectedItem.Text;
                        objBOMDetails.BomType = Convert.ToInt32(ddlBOMType.SelectedValue);
                        objBOMDetails.BomTypeText = ddlBOMType.SelectedItem.Text;
                        objBOMDetails.UOM = Convert.ToInt32(hdfItemUOM.Value);
                        objBOMDetails.UomText = txtUOM.Text;
                        objBOMDetails.QTY = Decimal.Parse(txtQty.Text);
                        int slNo = 1;
                        if (ddlWOItemType.SelectedValue == "1")//Material
                        {

                            objBOMDetails.BOMaterialPK = Convert.ToInt32(hdfBOMMaterialPK.Value);//prodct
                            objBOMDetails.BOMaterial = txtBOMMaterial.Text;
                            objBOMDetails.BomMaterialCatText = txtBOMCategory.Text;//cat
                            objBOMDetails.BomMaterialCat = Convert.ToInt32(hdfBOMCategoryPK.Value);
                        }
                        if (ddlWOItemType.SelectedValue == "2")//Product
                        {
                            objBOMDetails.BOMaterialPK = Convert.ToInt32(hdfProductPKBOM.Value);
                            objBOMDetails.BOMaterial = txtItemProductBOM.Text;
                            objBOMDetails.BomMaterialCatText = ddlProductcatBOM.SelectedItem.Text;
                            objBOMDetails.BomMaterialCat = Convert.ToInt32(ddlProductcatBOM.SelectedValue);
                        }
                        if (ddlWOItemType.SelectedValue == "3")//Brand
                        {
                            objBOMDetails.BOMaterialPK = Convert.ToInt32(hdfBrandBOM.Value);
                            objBOMDetails.BOMaterial = txtBrandProductBOM.Text;
                            objBOMDetails.BomMaterialCatText = txtCustomerBOM.Text;
                            objBOMDetails.BomMaterialCat = Convert.ToInt32(hdfCustomerIDBOM.Value);
                           

                        }
                        if (ddlWOItemType.SelectedValue == "4")//Packing material
                        {
                            objBOMDetails.BOMaterialPK = Convert.ToInt32(hdfPMItem.Value);
                            objBOMDetails.BOMaterial = txtPMItem.Text;
                            objBOMDetails.BomMaterialCatText = txtPMCategory.Text;
                            objBOMDetails.BomMaterialCat = Convert.ToInt32(hdfPMCatgoryPK.Value);
                        }

                        objBOMDetails.BomItemType = Convert.ToInt32(ddlWOItemType.SelectedValue);
                        objBOMDetails.BomItemTypeText = ddlWOItemType.SelectedItem.Text;
                        objBOMDetails.IssueQty = Decimal.Parse(txtIssue_Qty.Text);
                        objBOMDetails.IssueUOM = Convert.ToInt32(dddlIssueUom.SelectedValue);
                        objBOMDetails.IssueUomText = dddlIssueUom.SelectedItem.Text;
                        objBOMDetails.Tolerance = Decimal.Parse(txtTolerance.Text);
                        if (Convert.ToInt32(hdfCustomerID.Value) > 0)
                            objBOMDetails.CustomerPK = Convert.ToInt16(hdfCustomerID.Value);
                        if (Convert.ToInt32(hdfBrand.Value) > 0)
                            objBOMDetails.BrandPK = Convert.ToInt16(hdfBrand.Value);
                        if (lstBomDetails == null || lstBomDetails.Count == 0)
                            lstBomDetails = new List<BOMDetails>();
                        else
                            slNo = lstBomDetails.Max(m => m.SlNo) + 1;
                        objBOMDetails.SlNo = slNo;
                        lstBomDetails.Add(objBOMDetails);
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
        private void GetUIValuesFromObject()
        {

            try
            {
                //WorkOrderItem
                if (WorkOrderItem != null)
                {
                    lstBomDetails = WorkOrderItem.Details;
                    if (WorkOrderItem.Bom_Qty.ToString() != "0")
                    {
                        txtQuantity.Text = WorkOrderItem.Bom_Qty.ToString();
                    }
                    if (WorkOrderItem.WOMItemType == 3 && WorkOrderItem.Details!=null && WorkOrderItem.Details.Count>0)
                    {
                        hdfCustomerID.Value = WorkOrderItem.Details[0].CustomerPK.ToString();
                        hdfBrand.Value = WorkOrderItem.Details[0].BrandPK.ToString();
                        txtBrandProduct.Text = WorkOrderItem.Details[0].BrandName;
                        txtCustomer.Text = WorkOrderItem.Details[0].CustomerName;
                    }
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                    LastModifiedTime = WorkOrderItem.WOMModOn;
                }

                BindGrid(ControlsEnum.BOMLIST);
                BindGrid(ControlsEnum.PACKMAPLIST);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void FillItemDetails(int SelectSlno, ActionsEnum type)
        {

            BOMDetails bomdetails = lstBomDetails.FirstOrDefault(x => x.SlNo == SelectSlno);
            switch (type)
            {
                case ActionsEnum.EDITPACKMATITEM:
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitPMMCategory();", true);
                    ddlPMMOperationWork.SelectedIndex = Convert.ToInt32(ddlOperation.Items.IndexOf(ddlOperation.Items.FindByValue(bomdetails.Operation.ToString())));
                    hdfPMMUOMPK.Value= bomdetails.UOM.ToString();
                    txtPMMUOM.Text = bomdetails.UomText;
                    txtPMMQty.Text = bomdetails.QTY.ToString();
                    ddlPMMIssueUom.SelectedIndex = Convert.ToInt32(dddlIssueUom.Items.IndexOf(dddlIssueUom.Items.FindByValue(bomdetails.IssueUOM.ToString())));
                    txtPMMIssueQty.Text = bomdetails.IssueQty.ToString();
                    txtPMMTolerance.Text = bomdetails.Tolerance.ToString();
                    //hdfItemTypeBOM.Value = bomdetails.BomItemType.ToString();
                    hdfSelectedPMMCATTEXT.Value = bomdetails.BomMaterialCatText.ToString();
                    hdfSelectedPMMCATPK.Value = bomdetails.BomMaterialCat.ToString();
                    hdfSelectedPMMTEXT.Value = bomdetails.BOMaterial.ToString();
                    hdfSelectedPMMPK.Value = bomdetails.BOMaterialPK.ToString();
                    hdfSelectedCustomerPK.Value = bomdetails.CustomerPK.ToString();
                    hdfSelectedCustomer.Value = bomdetails.CustomerName.ToString();
                    hdfSelectedBrand.Value = bomdetails.BrandName.ToString();
                    hdfSelectedBrandPK.Value = bomdetails.BrandPK.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitPMMCategory(1);", true);
                    break;
                case ActionsEnum.EDITITEM:
                    ddlOperation.SelectedIndex = Convert.ToInt32(ddlOperation.Items.IndexOf(ddlOperation.Items.FindByValue(bomdetails.Operation.ToString())));
                    ddlWOItemType.SelectedIndex = Convert.ToInt32(ddlWOItemType.Items.IndexOf(ddlWOItemType.Items.FindByValue(bomdetails.BomItemType.ToString())));
                    if (bomdetails.BomType != 0)
                    {
                        ddlBOMType.SelectedIndex = Convert.ToInt32(ddlBOMType.Items.IndexOf(ddlBOMType.Items.FindByValue(bomdetails.BomType.ToString())));

                    }
                    hdfItemUOM.Value = bomdetails.UOM.ToString();
                    txtUOM.Text = bomdetails.UomText;
                    txtQty.Text = bomdetails.QTY.ToString();
                    dddlIssueUom.SelectedIndex = Convert.ToInt32(dddlIssueUom.Items.IndexOf(dddlIssueUom.Items.FindByValue(bomdetails.IssueUOM.ToString())));
                    txtIssue_Qty.Text = bomdetails.IssueQty.ToString();
                    txtTolerance.Text = bomdetails.Tolerance.ToString();
                    ShowHideSearchControlsInBOM(bomdetails.BomItemType);
                    hdfItemTypeBOM.Value = bomdetails.BomItemType.ToString();
                    hdfSelectedCat.Value = bomdetails.BomMaterialCatText.ToString();
                    hdfSelectedCatpak.Value = bomdetails.BomMaterialCat.ToString();
                    hdfSelectedMaterial.Value = bomdetails.BOMaterial.ToString();
                    hdfSelectedMaterialPK.Value = bomdetails.BOMaterialPK.ToString();
                    switch (bomdetails.BomItemType)
                    {
                        case 1://Material
                               //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitBOM", "InitBOMMAuto(1);", true);
                            break;
                        case 2://Product
                            ddlProductcatBOM.SelectedIndex = Convert.ToInt32(ddlProductcatBOM.Items.IndexOf(ddlProductcatBOM.Items.FindByValue(bomdetails.BomMaterialCat.ToString())));
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitBOM", "InitProductsBOM(1);", true);//1
                            break;
                        case 3://Brand/
                            break;
                        case 4://Packing Material
                            break;

                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitBOM", "InitBOMMAuto(1);", true);
                    break;
            }
        }

        /// <summary>
        /// For finding any radio checked
        /// </summary>
        /// <returns></returns>
        private bool IsChecked()
        {
            try
            {
                foreach (GridViewRow grdrow in grdWOItems.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum control)
        {
            try
            {
                switch (control)
                {
                    case ControlsEnum.DEFAULT:
                        if (dsResult != null)
                        {

                            int rowCount = 0;
                            int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
                            {
                                rowCount = Convert.ToInt32(dsResult.Tables[0].Rows[0][0].ToString());
                            }
                            TotalPages = uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                          (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                          (rowCount / pageSize) + 1;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);

                            PageIndex = PageIndex == null ? "1" : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdWOItems.DataSource = dsResult.Tables[1];
                            grdWOItems.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdWOItems.DataSource = null;
                            grdWOItems.DataBind();
                        }
                        break;
                    case ControlsEnum.BOMLIST:
                        if (lstBomDetails != null && lstBomDetails.Count > 0)
                        {
                            grdBOM.DataSource = lstBomDetails.Where(s => s.IsPrdMapp == 0).ToList();                          
                        }
                        else
                        {
                            grdBOM.DataSource = null;
                        }
                        grdBOM.DataBind();
                        break;
                    case ControlsEnum.PACKMAPLIST:
                        if (lstBomDetails != null && lstBomDetails.Count > 0)
                        {
                            grdPackMaterialDetail.DataSource = lstBomDetails.Where(s => s.IsPrdMapp == 1).ToList();

                        }
                        else
                        {
                            grdPackMaterialDetail.DataSource = null;
                        }
                        grdPackMaterialDetail.DataBind();
                        break;

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Bind DropDowns
        /// </summary>
        public void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.OPERATON:
                        ddlOperation.Items.Clear();
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            if (HasPackingMaterial == 1)//pouch product & wallet product
                            {
                                ddlPMMOperationWork.DataSource = dtResult;
                                ddlPMMOperationWork.DataTextField = Resources.DataFieldRes.WOData;
                                ddlPMMOperationWork.DataValueField = Resources.DataFieldRes.WOPK;
                                ddlPMMOperationWork.DataBind();
                            }
                            if (Convert.ToInt32(hdfItemType.Value) == 1)
                                dtResult = dtResult.Select("CFG_VALUE IN (1,2)").CopyToDataTable();//Material 
                            else if (Convert.ToInt32(hdfItemType.Value) == 2)
                                dtResult = dtResult.Select("CFG_VALUE NOT IN (5)").CopyToDataTable();//Product
                            else if (Convert.ToInt32(hdfItemType.Value) == 3)
                                dtResult = dtResult.Select("CFG_VALUE IN (2,5)").CopyToDataTable();//Brand
                            ddlOperation.DataSource = dtResult;
                            ddlOperation.DataTextField = Resources.DataFieldRes.WOData;
                            ddlOperation.DataValueField = Resources.DataFieldRes.WOPK;
                            ddlOperation.DataBind();
                            
                        }
                        //ddlOperation.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        //ddlOperation.SelectedIndex = 0;



                        //if (typePk > 0)
                        //{
                        //    ddlEmpType.SelectedIndex = Convert.ToInt32(ddlEmpType.Items.IndexOf(ddlEmpType.Items.FindByValue(typePk.ToString())));
                        //}
                        break;
                    case ControlsEnum.BOMTYPE:
                        ddlBOMType.Items.Clear();
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlBOMType.DataSource = dtResult;
                            ddlBOMType.DataTextField = Resources.DataFieldRes.WOData;
                            ddlBOMType.DataValueField = Resources.DataFieldRes.WOPK;
                            ddlBOMType.DataBind();
                            if (HasPackingMaterial == 1)//pouch product & wallet product
                            {
                                ddlPMMType.DataSource = dtResult;
                                ddlPMMType.DataTextField = Resources.DataFieldRes.WOData;
                                ddlPMMType.DataValueField = Resources.DataFieldRes.WOPK;
                                ddlPMMType.DataBind();
                            }
                            //ddlBOMType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                            //ddlBOMType.SelectedIndex = 0;
                            //if (typePk > 0)
                            //{
                            //    ddlEmpType.SelectedIndex = Convert.ToInt32(ddlEmpType.Items.IndexOf(ddlEmpType.Items.FindByValue(typePk.ToString())));
                            //}
                        }
                        break;
                    case ControlsEnum.UOM:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            dddlIssueUom.DataSource = dtResult;
                            dddlIssueUom.DataTextField = Resources.DataFieldRes.UomCode;
                            dddlIssueUom.DataValueField = Resources.DataFieldRes.UomPK;
                            dddlIssueUom.DataBind();
                            if (HasPackingMaterial == 1)
                            {
                                ddlPMMIssueUom.DataSource = dtResult;
                                ddlPMMIssueUom.DataTextField = Resources.DataFieldRes.UomCode;
                                ddlPMMIssueUom.DataValueField = Resources.DataFieldRes.UomPK;
                                ddlPMMIssueUom.DataBind();
                                ddlPMMIssueUom.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                            }

                        }
                        dddlIssueUom.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        lblUOM.Text = hdfWOIUOM.Value.ToString();
                        break;
                    case ControlsEnum.ITEMTYPE:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlWOItemType.DataSource = dtResult;
                            ddlWOItemType.DataTextField = Resources.DataFieldRes.cfgData;
                            ddlWOItemType.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddlWOItemType.DataBind();
                            dtResult.Rows.RemoveAt(dtResult.Rows.Count - 1);
                            ddl_ItemType.DataSource = dtResult;
                            ddl_ItemType.DataTextField = Resources.DataFieldRes.cfgData;
                            ddl_ItemType.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddl_ItemType.DataBind();
                        }
                        //ddlWOItemType.ItemType.Remove(3);

                        //ddlWOItemType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        lbl_ItemType.Text = hdfItemTypeText.Value;
                        // ddlWOItemType.SelectedIndex = 0;
                        break;
                    case ControlsEnum.ITEMCATEGORY:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddl_matcat.DataSource = dtResult;
                            ddl_matcat.DataTextField = Resources.DataFieldRes.cfgData;
                            ddl_matcat.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddl_matcat.DataBind();
                        }
                        ddl_matcat.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        //ddlWOItemType.SelectedIndex = 0;
                        break;
                    case ControlsEnum.ITEMCATEGORYBOM:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlProductcatBOM.DataSource = dtResult;
                            ddlProductcatBOM.DataTextField = Resources.DataFieldRes.cfgData;
                            ddlProductcatBOM.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddlProductcatBOM.DataBind();
                        }
                        ddlProductcatBOM.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        //ddlWOItemType.SelectedIndex = 0;
                        break;

                }
                //ddlEmpType.Items.Clear();
                //if (xacEmpTypeLst != null && xacEmpTypeLst.Count() > 0)
                //{
                //    ddlEmpType.DataSource = xacEmpTypeLst;
                //    ddlEmpType.DataTextField = Resources.DataFieldRes.EmpTypeData;
                //    ddlEmpType.DataValueField = Resources.DataFieldRes.EmpTypePK;
                //    ddlEmpType.DataBind();
                //}
                //ddlEmpType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                //ddlEmpType.SelectedIndex = 0;
                //if (typePk > 0)
                //{
                //    ddlEmpType.SelectedIndex = Convert.ToInt32(ddlEmpType.Items.IndexOf(ddlEmpType.Items.FindByValue(typePk.ToString())));
                //}
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
                //foreach (GridViewRow grdrow in grdEmployee.Rows)
                //{
                //    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                //    // check row selected or not                    
                //    if (!IsChecked())
                //    {
                //        // if no items selected, Show Error Message
                //        litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                //        EntryStatus = EntryStatus.LISTMODE;
                //        return;
                //    }
                //    if (rbtn.Checked)
                //    {
                //        // get pk from the grid and assign to CurrPk
                //        CurrPK = Convert.ToInt32(grdEmployee.DataKeys[grdrow.RowIndex].Values[0]);
                //        // Get And Set the Employee Values
                //        GetFieldValues(ControlsEnum.EMPLOYEE);
                //        //if (Mode == ActionsEnum.ADDROLE)
                //        //{
                //        //    if (employeeMstLst != null && employeeMstLst.Count() > 0)
                //        //    {
                //        //        employeeMstObj = new EmpEmployeeMst();
                //        //        employeeMstObj.empPK = employeeMstLst[0].empPK;
                //        //        employeeMstObj.empName = employeeMstLst[0].empName;
                //        //        employeeMstObj.empCode = employeeMstLst[0].empCode;
                //        //        // Passing the Employee object to Employee Roles Page
                //        //        //Session[SessionStrings.Employee] = employeeMstObj;
                //        //    }
                //        //    //Response.Redirect(Resources.PageURL.EmployeeRoles, true);
                //        //}
                //        SetFieldValues(ControlsEnum.EMPLOYEE);
                //        txtEmployeeCode.Focus();
                //        ModifiedDatePnl.Visible = true;
                //        ActivateEmployeeDetails();

                //        if (Mode == ActionsEnum.VIEW)
                //        {
                //            EntryStatus = EntryStatus.VIEWMODE;
                //        }
                //        else
                //            EntryStatus = EntryStatus.ENTRYMODE;
                //        return;
                //    }
                //}
                //// if no items selected, Show Error Message
                //litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                //EntryStatus = EntryStatus.LISTMODE;
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
            lstBomDetails = null;
            BindGrid(ControlsEnum.BOMLIST);
            txtUOM.Text = string.Empty;
            ShowHideSearchComtrol(Convert.ToInt32(ddl_ItemType.SelectedValue));

        }
        private void ShowHideSearchControlsInBOM(int type)
        {
            switch (type)
            {
                case 1://Material
                    tdItemCategoryBOM.Visible = true;
                    tdPrdCategoryBOM.Visible = false;
                    tdBrandCategoryBOM.Visible = false;
                    tdPMItem.Visible = false;
                    tdPMCatBOM.Visible = false;
                    tdMaterialBOM.Visible = true;
                    tdProductBOM.Visible = false;
                    tdBrandBOM.Visible = false;
                    thcusthead.Visible = false;
                    thcathead.Visible = true;
                    vrfBOMCategory.Visible = true;
                    vrfBomMaterial.Visible = true;
                    vrfprdcat.Visible = false;
                    vrfpmcat.Visible = false;
                    vrfprdItem.Visible = false;
                    vrfcustItem.Visible = false;
                    vrfpmItem.Visible = false;
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "FillMaterialCategoryAutoComplete(1);", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitBOM", "InitBOMMAuto();", true);//1
                    break;
                case 2://Products
                    tdPMItem.Visible = false;
                    tdPMCatBOM.Visible = false;
                    tdItemCategoryBOM.Visible = false;
                    tdPrdCategoryBOM.Visible = true;
                    tdBrandCategoryBOM.Visible = false;
                    thcathead.Visible = true;
                    tdMaterialBOM.Visible = false;
                    tdProductBOM.Visible = true;
                    tdBrandBOM.Visible = false;
                    thcusthead.Visible = false;
                    vrfBOMCategory.Visible = false;
                    vrfBomMaterial.Visible = false;
                    vrfprdcat.Visible = true;
                    vrfpmcat.Visible = false;
                    vrfprdItem.Visible = true;
                    vrfcustItem.Visible = false;
                    vrfpmItem.Visible = false;
                    GetFieldValues(ControlsEnum.ITEMCATEGORY);
                    SetFieldValues(ControlsEnum.ITEMCATEGORYBOM);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitProductsBOM();", true);
                    break;
                case 3://Brands
                    tdItemCategoryBOM.Visible = false;
                    tdPrdCategoryBOM.Visible = false;
                    tdBrandCategoryBOM.Visible = true;
                    thcusthead.Visible = true;
                    tdMaterialBOM.Visible = false;
                    tdProductBOM.Visible = false;
                    tdBrandBOM.Visible = true;
                    thcathead.Visible = false;
                    tdPMItem.Visible = false;
                    tdPMCatBOM.Visible = false;
                    vrfBOMCategory.Visible = false;
                    vrfBomMaterial.Visible = false;
                    vrfprdcat.Visible = false;
                    vrfpmcat.Visible = false;
                    vrfprdItem.Visible = false;
                    vrfcustItem.Visible = true;
                    vrfpmItem.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitCustomerBrandsBOM();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitBrandsBOM;", true);
                    break;
                case 4://packing material
                    tdItemCategoryBOM.Visible = false;
                    tdPrdCategoryBOM.Visible = false;
                    tdBrandCategoryBOM.Visible = false;
                    tdPMCatBOM.Visible = true;
                    thcusthead.Visible = false;
                    tdMaterialBOM.Visible = false;
                    tdProductBOM.Visible = false;
                    tdBrandBOM.Visible = false;
                    thcathead.Visible = true;
                    tdPMItem.Visible = true;
                    vrfBOMCategory.Visible = false;
                    vrfBomMaterial.Visible = false;
                    vrfprdcat.Visible = false;
                    vrfpmcat.Visible = true;
                    vrfprdItem.Visible = false;
                    vrfcustItem.Visible = false;
                    vrfpmItem.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitPMCategory();", true);
                    // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitPMItem();", true);
                    break;
            }

        }
        private void ShowHideSearchComtrol(int type)
        {
            switch (type)
            {
                case 1://Material
                    tdItemCategory.Visible = true;
                    tdPrdCategory.Visible = false;
                    tdBrandCategory.Visible = false;
                    tdMaterial.Visible = true;
                    tdProduct.Visible = false;
                    tdBrand.Visible = false;
                    tdPackingCat.Visible = false;
                    tdPackingItem.Visible = false;
                    trRow2.Visible = false;
                    break;
                case 2://Products
                    tdItemCategory.Visible = false;
                    tdPrdCategory.Visible = true;
                    tdBrandCategory.Visible = false;
                    tdMaterial.Visible = false;
                    tdProduct.Visible = true;
                    tdBrand.Visible = false;
                    tdPackingCat.Visible = false;
                    tdPackingItem.Visible = false;
                    trRow2.Visible = true;
                    GetFieldValues(ControlsEnum.ITEMCATEGORY);
                    SetFieldValues(ControlsEnum.ITEMCATEGORY);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitProducts();", true);
                    break;
                case 3://Brands
                    tdItemCategory.Visible = false;
                    tdPrdCategory.Visible = false;
                    tdBrandCategory.Visible = true;
                    tdMaterial.Visible = false;
                    tdProduct.Visible = false;
                    tdBrand.Visible = true;
                    tdPackingCat.Visible = false;
                    tdPackingItem.Visible = false;
                    trRow2.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitCustomerBrands();", true);
                    break;
                case 4://PM
                    tdItemCategory.Visible = false;
                    tdPrdCategory.Visible = false;
                    tdBrandCategory.Visible = false;
                    tdMaterial.Visible = false;
                    tdProduct.Visible = false;
                    tdBrand.Visible = false;
                    tdPackingCat.Visible = true;
                    tdPackingItem.Visible = true;
                    trRow2.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitWOIPMCategory();", true);
                    break;

            }
        }
        /// <summary>
        /// To validate auto complete hidden fields
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            try
            {
                bool flag;
                flag = true;
                //if (hdfDesignation.Value == string.Empty || hdfDesignation.Value == "0")
                //{
                //    litErrorMsg.Text = this.GetLocalResourceObject("Err_Designation").ToString();
                //    flag = false;
                //}
                return flag;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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



        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {

            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int result;
                result = 0;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {

                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;

                }

                switch (commonActions)
                {
                    #region SELECTED INDEX CHANGED
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        if (((DropDownList)sender).ID == "ddl_ItemType")
                        {
                            lbl_ItemType.Text = ddl_ItemType.SelectedItem.Text;
                            hdfItemTypeText.Value = ddl_ItemType.SelectedItem.Text;
                            hdfItemType.Value = ddl_ItemType.SelectedValue;
                            ShowHideSearchComtrol(Convert.ToInt32(ddl_ItemType.SelectedValue));
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        if (((DropDownList)sender).ID == "ddlOperation")
                        {
                            ddlPMMOperationWork.SelectedValue = ddlOperation.SelectedValue;
                        }
                        if (((DropDownList)sender).ID == "ddlWOItemType")
                        {
                            ShowHideSearchControlsInBOM(Convert.ToInt32(ddlWOItemType.SelectedValue));
                        }
                        break;
                    #endregion

                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErpRes.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else //valid
                        {
                            foreach (var bomdetail in lstBomDetails)
                            {
                                if (bomdetail.OperationText == "")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + this.GetLocalResourceObject("Err_Operation_Work") + "','" + Resources.ErpRes.Information + "');", true);
                                    return;

                                }
                                if (Convert.ToString(bomdetail.IssueUOM) == "")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + this.GetLocalResourceObject("Err_IssueUom") + "','" + Resources.ErpRes.Information + "');", true);
                                    return;

                                }
                                if (Convert.ToString(bomdetail.UOM) == "")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + this.GetLocalResourceObject("Err_Uom") + "','" + Resources.ErpRes.Information + "');", true);
                                    return;

                                }
                                if (Convert.ToString(bomdetail.IssueQty) == "")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + this.GetLocalResourceObject("Err_Issue_Qty") + "','" + Resources.ErpRes.Information + "');", true);
                                    return;

                                }
                            }

                            //new
                            //    for (int i = 1; i < lstBomDetails.Count; i++)
                            //{
                            //    BOMDetails bomrowdetails = lstBomDetails.FirstOrDefault(x => x.SlNo == i);

                            //    if (bomrowdetails.SlNo == i)
                            //    {
                            //        if (bomrowdetails.OperationText == "")
                            //        {
                            //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + this.GetLocalResourceObject("Err_Operation_Work") + "','" + Resources.ErpRes.Information + "');", true);
                            //            return;

                            //        }
                            //        if (Convert.ToString(bomrowdetails.IssueUOM) == "")
                            //        {
                            //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + this.GetLocalResourceObject("Err_IssueUom") + "','" + Resources.ErpRes.Information + "');", true);
                            //            return;

                            //        }
                            //        if (Convert.ToString(bomrowdetails.IssueQty) == "")
                            //        {
                            //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + this.GetLocalResourceObject("Err_Issue_Qty") + "','" + Resources.ErpRes.Information + "');", true);
                            //            return;

                            //        }
                            //    }
                            //}

                            //new end
                            if (CurrPK > 0)
                                WorkOrderItem = (WorkOrderBomBO)SetUIValuesToObject(ControlsEnum.SAVE);
                            //for checking the temp grid is empty
                            //if (lstBomDetails == null || lstBomDetails.Count == 0)
                            //{
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.ErrorMessages.Msg_NoItemInList + "','" + Resources.ErpRes.Information + "');", true);
                            //    return;
                            //}
                            string xmlDoc = CommonFunctions.XmlSerialize<WorkOrderBomBO>(WorkOrderItem);
                            result = BusinessLogic.MaterialManagement.MaterialMaster.SaveOrderItemDetails(xmlDoc);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.BillOfMaterial);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                ddl_ItemType.SelectedIndex = Convert.ToInt32(ddl_ItemType.Items.IndexOf(ddl_ItemType.Items.FindByValue(hdfItemType.Value.ToString())));
                                if (HasPackingMaterial == 1)
                                {
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ActivateWOItemList();
                                }
                                else
                                {
                                    ClearBOMControls();
                                    //ddlOperation.SelectedIndex = 0;
                                }

                                ClearPackMapControls();
                                ShowHideSearchComtrol(Convert.ToInt32(hdfItemType.Value));
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
                                    litErrorMsg.Text = Resources.PageNameRes.WorkOrderItems + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.WorkOrderItems + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.WorkOrderItems + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CustomerBrand);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    case ActionsEnum.LIST:
                        ActivateWOItemList();
                        ClearBOMControls();
                        EntryStatus = EntryStatus.LISTMODE;
                        ddl_ItemType.SelectedIndex = Convert.ToInt32(ddl_ItemType.Items.IndexOf(ddl_ItemType.Items.FindByValue(hdfItemType.Value.ToString())));
                        ShowHideSearchComtrol(Convert.ToInt32(ddl_ItemType.SelectedValue));

                        break;
                    case ActionsEnum.CLEAR:
                        ClearSelection();
                        GetFieldValues(ControlsEnum.ITEMTYPE);
                        SetFieldValues(ControlsEnum.ITEMTYPE);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);

                        break;
                    case ActionsEnum.REMOVEPACKMATITEM:
                        int bom_slno = int.Parse(((Button)sender).CommandArgument.ToString());
                        if (lstBomDetails != null)
                        {
                            lstBomDetails.RemoveAll(r => r.SlNo == bom_slno);
                            BindGrid(ControlsEnum.PACKMAPLIST);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitBOM", "InitBOMMAuto();", true);
                        }
                        break;
                    case ActionsEnum.REMOVE:

                        int pm_slno = int.Parse(((Button)sender).CommandArgument.ToString());
                        if (lstBomDetails != null)
                        {
                            lstBomDetails.RemoveAll(r => r.SlNo == pm_slno);
                            BindGrid(ControlsEnum.BOMLIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitBOM", "InitBOMMAuto();", true);
                        }
                        break;
                    #region Edit PM Details
                    case ActionsEnum.EDITPACKMATITEM:
                        SelectSlno = int.Parse(((Button)sender).CommandArgument.ToString());
                        FillItemDetails(SelectSlno, ActionsEnum.EDITPACKMATITEM);
                        break;
                    #endregion
                    #region Edit BOM details
                    case ActionsEnum.EDITITEM:
                        //SetUIEditView(commonActions);
                        SelectSlno = int.Parse(((Button)sender).CommandArgument.ToString());
                        FillItemDetails(SelectSlno, ActionsEnum.EDITITEM);
                        //old   
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitBOM", "InitBOMMAuto(1);", true);

                        break;
                    #endregion
                    case ActionsEnum.ADDNEW:
                        if (SelectSlno > 0 && lstBomDetails != null)
                        {
                            lstBomDetails.ForEach(sl =>
                            {
                                if (sl.SlNo == SelectSlno)
                                {
                                    sl.BOMaterialPK = Convert.ToInt32(hdfPMMItemPK.Value);
                                    sl.BOMaterial = txtPMMItem.Text;
                                    sl.BomMaterialCatText = txtPMMCategory.Text;
                                    sl.BomMaterialCat = Convert.ToInt32(hdfPMMCatPK.Value);
                                    sl.Operation = Convert.ToInt32(ddlPMMOperationWork.SelectedValue);
                                    sl.OperationText = ddlPMMOperationWork.SelectedItem.Text;
                                    sl.BomType = Convert.ToInt32(ddlPMMType.SelectedValue);
                                    sl.BomTypeText = ddlPMMType.SelectedItem.Text;
                                    sl.UOM = Convert.ToInt32(hdfPMMUOMPK.Value);
                                    sl.UomText = txtPMMUOM.Text;
                                    sl.QTY = Decimal.Parse(txtPMMQty.Text);
                                    //objBOMDetail.BomItemType = 4;
                                    //objBOMDetail.BomItemTypeText = ddlWOItemType.Items[3].Text;//packing material
                                    // sl.BomItemType = Convert.ToInt32(ddlWOItemType.SelectedValue);
                                    // sl.BomItemTypeText = ddlWOItemType.SelectedItem.Text;
                                    sl.IssueQty = Decimal.Parse(txtPMMIssueQty.Text);
                                    sl.IssueUOM = Convert.ToInt32(ddlPMMIssueUom.SelectedValue);
                                    sl.IssueUomText = ddlPMMIssueUom.SelectedItem.Text;
                                    sl.Tolerance = Decimal.Parse(txtPMMTolerance.Text);
                                    sl.CustomerName = txtCustomerID.Text;
                                    sl.CustomerPK = Convert.ToInt32(hdfCustomerPK.Value);
                                    sl.BrandName = txtBrandID.Text;
                                    sl.BrandPK = Convert.ToInt32(hdfBrandID.Value);
                                    sl.BrandCode = hdfbrandcode.Value;
                                }

                            });
                            BindGrid(ControlsEnum.PACKMAPLIST);
                            ClearFields();
                        }
                        else
                      if (IsSameItemExistInPM(4))
                        {
                            hdfSelectPMMCatPk.Value = hdfPMMCatPK.Value;
                            hdfSelectPMMCat.Value = txtPMMCategory.Text;
                            hdfSelectPMMMatPk.Value = hdfPMMItemPK.Value;
                            hdfSelectPMMMat.Value = txtPMMItem.Text;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SameItemExist").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            SetUIValuesToObject(ControlsEnum.PACKMAPLIST);
                            BindGrid(ControlsEnum.PACKMAPLIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitPMMCategory();", true);
                            ClearPackMapControls();
                        }
                        break;
                    case ActionsEnum.ADD:
                        if (SelectSlno > 0 && lstBomDetails != null)
                        {
                            lstBomDetails.ForEach(sl =>
                            {
                                if (sl.SlNo == SelectSlno)
                                {
                                    if (ddlWOItemType.SelectedValue == "1")
                                    {
                                        sl.BOMaterialPK = Convert.ToInt32(hdfBOMMaterialPK.Value);
                                        sl.BOMaterial = txtBOMMaterial.Text;
                                        sl.BomMaterialCatText = txtBOMCategory.Text;
                                        sl.BomMaterialCat = Convert.ToInt32(hdfBOMCategoryPK.Value);
                                    }
                                    if (ddlWOItemType.SelectedValue == "2")
                                    {
                                        sl.BOMaterialPK = Convert.ToInt32(hdfProductPKBOM.Value);
                                        sl.BOMaterial = txtItemProductBOM.Text;
                                        sl.BomMaterialCatText = ddlProductcatBOM.SelectedItem.Text;
                                        sl.BomMaterialCat = Convert.ToInt32(ddlProductcatBOM.SelectedValue);

                                    }
                                    if (ddlWOItemType.SelectedValue == "3")
                                    {
                                        sl.BOMaterialPK = Convert.ToInt32(hdfBrandBOM.Value);
                                        sl.BOMaterial = txtBrandProductBOM.Text;
                                        sl.BomMaterialCatText = txtCustomerBOM.Text;
                                        sl.BomMaterialCat = Convert.ToInt32(hdfCustomerIDBOM.Value);
                                      
                                    }
                                    if (ddlWOItemType.SelectedValue == "4")
                                    {
                                        sl.BOMaterialPK = Convert.ToInt32(hdfPMItem.Value);
                                        sl.BOMaterial = txtPMItem.Text;
                                        sl.BomMaterialCatText = txtPMCategory.Text;
                                        sl.BomMaterialCat = Convert.ToInt32(hdfPMCatgoryPK.Value);
                                    }
                                    sl.Operation = Convert.ToInt32(ddlOperation.SelectedValue);
                                    sl.OperationText = ddlOperation.SelectedItem.Text;
                                    sl.BomType = Convert.ToInt32(ddlBOMType.SelectedValue);
                                    sl.BomTypeText = ddlBOMType.SelectedItem.Text;
                                    sl.UOM = Convert.ToInt32(hdfItemUOM.Value);
                                    sl.UomText = txtUOM.Text;
                                    sl.QTY = Decimal.Parse(txtQty.Text);

                                    sl.BomItemType = Convert.ToInt32(ddlWOItemType.SelectedValue);
                                    sl.BomItemTypeText = ddlWOItemType.SelectedItem.Text;
                                    sl.IssueQty = Decimal.Parse(txtIssue_Qty.Text);
                                    sl.IssueUOM = Convert.ToInt32(dddlIssueUom.SelectedValue);
                                    sl.IssueUomText = dddlIssueUom.SelectedItem.Text;
                                    sl.Tolerance = Decimal.Parse(txtTolerance.Text);

                                    if (Convert.ToInt32(hdfCustomerID.Value) > 0)
                                        sl.CustomerPK = Convert.ToInt16(hdfCustomerID.Value);
                                    if (Convert.ToInt32(hdfBrand.Value) > 0)
                                        sl.BrandPK = Convert.ToInt16(hdfBrand.Value);
                                }

                            });
                            BindGrid(ControlsEnum.BOMLIST);
                            ClearFields();
                        }
                        else
                        if (IsSameItemExist(ddlWOItemType.SelectedValue))
                        {

                            hdfSelectBOMCatPk.Value = hdfBOMCategoryPK.Value;
                            hdfSelectBOMCat.Value = txtBOMCategory.Text;
                            hdfSelectBOMMatPk.Value = hdfBOMMaterialPK.Value;
                            hdfSelectBOMMat.Value = txtBOMMaterial.Text;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SameItemExist").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            SetUIValuesToObject(ControlsEnum.BOMADD);
                            BindGrid(ControlsEnum.BOMLIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitProductsBOM();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitCustomerBrandsBOM();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitPMCategory();", true);
                            ClearFields();

                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitBOM", "InitBOMMAuto();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "FillMaterialCategoryAutoComplete(1);", true);
                        break;
                    case ActionsEnum.PACKINGMATERIALS:
                        int bomoperation = Convert.ToInt32(ddlOperation.SelectedValue);
                        HasPackingMaterial = 1;
                        ActivatePackMatMapping();
                        lblItmname.Text = lblItemName.Text;
                        lblItmcode.Text = lblItemCode.Text;
                        //ddlPMMOperationWork.SelectedIndex = 1;//packing
                        EntryStatus = EntryStatus.ALLOCATEMODE;
                        GetFieldValues(ControlsEnum.OPERATON);
                       SetFieldValues(ControlsEnum.OPERATON);
                        GetFieldValues(ControlsEnum.UOM);
                        SetFieldValues(ControlsEnum.UOM);
                        GetFieldValues(ControlsEnum.BOMTYPE);
                        SetFieldValues(ControlsEnum.BOMTYPE);
                        ddlPMMOperationWork.SelectedValue = bomoperation.ToString();
                        ddlPMMOperationWork.Enabled = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitPMMCategory();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitCust();", true);
                        break;
                    #region Add Bill Of Materials
                    case ActionsEnum.ADDITION:
                        if (!IsChecked())
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                            return;
                        }

                        foreach (GridViewRow grdrow in grdWOItems.Rows)
                        {
                            HasPackingMaterial = 0;
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                CurrPK = Convert.ToInt32(grdWOItems.DataKeys[grdrow.RowIndex].Values[0]);
                                Label lblICode = (Label)grdrow.FindControl("lblItemCode");
                                lblItemCode.Text = lblICode.Text;
                                Label lblIName = (Label)grdrow.FindControl("lblItemName");
                                lblItemName.Text = lblIName.Text;
                                Label lblUOM = (Label)grdrow.FindControl("lblUOM");
                                hdfWOIUOM.Value = lblUOM.Text;
                                HiddenField hdf_uompk = (HiddenField)grdrow.FindControl("hdf_uompk");
                                hdfWOUomPk.Value = hdf_uompk.Value;
                                HiddenField hdf_catpk = (HiddenField)grdrow.FindControl("hdf_catpk");
                                //if (hdfItemType.Value == "2")//product
                                //{
                                //    if (hdf_catpk.Value == "2" || hdf_catpk.Value == "3")
                                //    {
                                //        spnPackingSpecsMapping.Visible = true;

                                //    }
                                //    else
                                //    {
                                //        spnPackingSpecsMapping.Visible = false;
                                //    }
                                //}
                                //GetFieldValues(ControlsEnum.EMPLOYEE);
                                //SetFieldValues(ControlsEnum.EMPLOYEE);
                                ModifiedDatePnl.Visible = false;
                                ActivateBOMDetails();
                                if (EntryStatus == EntryStatus.VIEWMODE)
                                {
                                    EntryStatus = EntryStatus.VIEWMODE;
                                }
                                else
                                    EntryStatus = EntryStatus.ENTRYMODE;
                                // hdfItemType.Value = ddl_ItemType.SelectedValue;
                                GetFieldValues(ControlsEnum.OPERATON);
                                SetFieldValues(ControlsEnum.OPERATON);
                                GetFieldValues(ControlsEnum.BOMTYPE);
                                SetFieldValues(ControlsEnum.BOMTYPE);
                                GetFieldValues(ControlsEnum.BOMDETAILS);
                                SetFieldValues(ControlsEnum.BOMDETAILS);
                                GetFieldValues(ControlsEnum.UOM);
                                SetFieldValues(ControlsEnum.UOM);
                                GetFieldValues(ControlsEnum.ITEMTYPE);
                                SetFieldValues(ControlsEnum.ITEMTYPE);
                                ShowHideSearchControlsInBOM(1);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitBOM", "InitBOMMAuto();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitProductsBOM();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitCustomerBrandsBOM();", true);
                                ClearFields();
                                ClearBOMControls();
                                if (lstBomDetails.Where(s => s.IsPrdMapp == 0).ToList().Count > 0)
                                {
                                    ddlOperation.SelectedIndex = Convert.ToInt32(ddlOperation.Items.IndexOf(ddlOperation.Items.FindByValue(lstBomDetails.Where(s => s.IsPrdMapp == 0).FirstOrDefault().Operation.ToString())));
                                }
                                if (hdfItemType.Value == "3")//brand
                                {
                                    ddlOperation.SelectedIndex = 1;//packing
                                }

                                if (hdfItemType.Value == "2")//product
                                {
                                    if (ddl_matcat.SelectedValue == "2")//wallet product
                                    {
                                        ddlOperation.SelectedIndex = 2;//walleting
                                    }
                                    if (ddl_matcat.SelectedValue == "3")//pouch product
                                    {
                                        ddlOperation.SelectedIndex = 3;//pouching
                                    }


                                }
                                break;
                            }
                        }

                        break;
                    #endregion

                    #region Search
                    case ActionsEnum.SEARCH:
                        btnSearch.Focus();
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        ddl_ItemType.SelectedIndex = Convert.ToInt32(ddl_ItemType.Items.IndexOf(ddl_ItemType.Items.FindByValue(hdfItemType.Value.ToString())));
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        ClearBOMControls();
                        ClearPackMapControls();
                        ShowHideSearchComtrol(Convert.ToInt32(hdfItemType.Value));
                        ActivateWOItemList();
                        break;
                        #endregion



                        //#region Employee Role
                        //case ActionsEnum.ADDROLE:
                        //    if (!IsChecked())
                        //    {
                        //        litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        //        EntryStatus = EntryStatus.LISTMODE;
                        //        return;
                        //    }
                        //foreach (GridViewRow grdrow in grdEmployee.Rows)
                        //{
                        //    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        //    if (rbtn.Checked)
                        //    {
                        //        CurrPK = Convert.ToInt32(grdEmployee.DataKeys[grdrow.RowIndex].Values[0]);
                        //        GetFieldValues(ControlsEnum.EMPLOYEE);
                        //        SetFieldValues(ControlsEnum.EMPLOYEE);
                        //        ModifiedDatePnl.Visible = false;
                        //        if (EntryStatus == EntryStatus.VIEWMODE)
                        //        {
                        //            EntryStatus = EntryStatus.VIEWMODE;
                        //        }
                        //        else
                        //            EntryStatus = EntryStatus.ENTRYMODE;
                        //        ActivateEmployeeRole();
                        //    }
                        //}
                        //break;
                        #endregion

                }
            }

            catch (Exception ex)
            {
                string Error = CommonFunctions.ProcessException(ex);
                if (Error == "547")
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.Messages.CannotDelete + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
                }

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
                    if (SortDirection == Resources.ErpRes.SortAscending)
                        SortDirection = Resources.ErpRes.SortDescending;
                    else
                        SortDirection = Resources.ErpRes.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.ErpRes.SortAscending;
                }
                this.PageIndex = "1";
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        //#endregion
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
                    // Session[SessionStrings.Employee] = null;
                    //SetConfigValue();
                    ConfigurationSettings();
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.ItemPK;
                    grdWOItems.DataKeyNames = datakeyarray;
                    //GetFieldValues(ControlsEnum.EMPLOYEETYPE);
                    //SetFieldValues(ControlsEnum.EMPLOYEETYPE);
                    GetFieldValues(ControlsEnum.ITEMTYPE);
                    SetFieldValues(ControlsEnum.ITEMTYPE);
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);

                    ////   GetFieldValues(ControlsEnum.EMPLOYEECOUNT);
                    //btnNew.Focus();
                    PageIndex = "1";
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    EntryStatus = EntryStatus.LISTMODE;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitComponents();", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            OPERATON,
            BOMTYPE,
            BOMLIST,
            BOMADD,
            SAVE,
            BOMDETAILS,
            UOM,
            ITEMTYPE,
            ITEMCATEGORY,
            ITEMCATEGORYBOM,
            PACKMAPLIST,

        }
        #endregion
    }
}