using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPService;
using ERPData;
using BusinessObject.CommonManagement;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace CustomerPortal.OrderToCash.UserControls
{
    public partial class SearchProducts : System.Web.UI.UserControl
    {
        #region Variables and Properties
        private ActionsEnum commonActions;
        private INV_ITEM_SPEC_DTL invItemSpecDtlObj;
        private INV_ITEM_SPEC_DTL invItemSpecDtlObjSelected;
        private List<INV_ITEM_SPEC_DTL> invItemSpecDtlList;
        private List<SPADM_CONST_GRP_GET_KV_Result> spAdmConstGrpGetKvResultList;
        private CommonService CommonServiceClient;
        private BusinessObject.User currentUser;
        private List<ADM_CONST_MST> admConstMstList;
        public event EventHandler SELECT;
        /// <summary>
        /// Dynamic Tab Desc
        /// </summary>
        public int ItemPK
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.ItemPK];
            }
            set
            {
                this.ViewState[ViewstateStrings.ItemPK] = value;
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
        //private AD_APP_CONST_CFG adAppConstCfgObj;

        #endregion
        #region Page Level Events
        /// <summary>
        /// handles page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        #endregion
        #region Page Action Handler
        /// <summary>
        /// Handles page load
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    
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
            CustomerRegistrationService costomerRegistrationServiceClient;
            costomerRegistrationServiceClient = null;
            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        #region OldCode
                        //costomerRegistrationServiceClient = new CustomerRegistrationService();
                        //costomerRegistrationServiceClient = CommonFunctions.InitiateClient(costomerRegistrationServiceClient);
                        //invItemSpecDtlObj = CommonFunctions.Initilize<INV_ITEM_SPEC_DTL>();
                        //if (!string.IsNullOrEmpty(hdfType.Value) && !hdfType.Value.Equals("0"))
                        //    invItemSpecDtlObj.ISD_NATURE = Convert.ToInt32(hdfType.Value);
                        //if (!string.IsNullOrEmpty(hdfThickness.Value) && !hdfThickness.Value.Equals("0"))
                        //    invItemSpecDtlObj.ISD_THICKNESS = Convert.ToInt32(hdfThickness.Value);
                        //if (!string.IsNullOrEmpty(hdfCategory.Value) && !hdfCategory.Value.Equals("0"))
                        //    invItemSpecDtlObj.ISD_PROCESS = Convert.ToInt32(hdfCategory.Value);
                        //if (!string.IsNullOrEmpty(hdfSurface.Value) && !hdfSurface.Value.Equals("0"))
                        //    invItemSpecDtlObj.ISD_SURFACE = Convert.ToInt32(hdfSurface.Value);
                        //if (!string.IsNullOrEmpty(hdfShade.Value) && !hdfShade.Value.Equals("0"))
                        //    invItemSpecDtlObj.ISD_COLOUR = Convert.ToInt32(hdfShade.Value);
                        //if (!string.IsNullOrEmpty(hdfClassification.Value) && !hdfClassification.Value.Equals("0"))
                        //    invItemSpecDtlObj.ISD_GRADE = Convert.ToInt32(hdfClassification.Value);
                        //if (!string.IsNullOrEmpty(hdfSize.Value) && !hdfSize.Value.Equals("0"))
                        //    invItemSpecDtlObj.ISD_SIZE = Convert.ToInt32(hdfSize.Value);
                        //if (!string.IsNullOrEmpty(hdfLength.Value) && !hdfLength.Value.Equals("0"))
                        //    invItemSpecDtlObj.ISD_LENGTH = Convert.ToInt32(hdfLength.Value);                        
                        //invItemSpecDtlObj.ISD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                        //invItemSpecDtlObj.INV_ITEM_MST= CommonFunctions.Initilize<INV_ITEM_MST>();
                        //invItemSpecDtlObj.INV_ITEM_MST.ITM_NAME = invItemSpecDtlObj.INV_ITEM_MST.ITM_CODE = string.Empty;
                        //if (txtName.Text.Trim() != string.Empty)
                        //    invItemSpecDtlObj.INV_ITEM_MST.ITM_NAME = txtName.Text.Trim();
                        //if (txtCode.Text.Trim() != string.Empty)
                        //    invItemSpecDtlObj.INV_ITEM_MST.ITM_CODE = txtCode.Text.Trim();

                        //invItemSpecDtlList=costomerRegistrationServiceClient.SearchProducts(invItemSpecDtlObj); 
                        #endregion
                        costomerRegistrationServiceClient = new CustomerRegistrationService();
                        costomerRegistrationServiceClient = CommonFunctions.InitiateClient(costomerRegistrationServiceClient);
                        invItemSpecDtlObj = CommonFunctions.Initilize<INV_ITEM_SPEC_DTL>();

                        //invItemSpecDtlObjSelected = ERP.Utilities.CommonFunctions.Initilize<INV_ITEM_SPEC_DTL>();               
                        Type objInvItemSpecDtl = invItemSpecDtlObj.GetType();
                        for (int i = 1; i <= Convert.ToInt32(hdfDdlCountPS.Value); i += 2)
                        {
                            HiddenField hdf = (HiddenField)tdCol1.FindControl("hdfddlSP" + i.ToString());
                            DropDownList ddl = (DropDownList)tdCol1.FindControl("ddlSP" + i.ToString());
                            foreach (PropertyInfo p in objInvItemSpecDtl.GetProperties())
                            {
                                if (p.Name == hdf.Value)
                                {
                                    if(ddl.SelectedValue!=CommonConstants.SELECTVAL)
                                        p.SetValue(invItemSpecDtlObj, Convert.ToInt32(ddl.SelectedValue), null);
                                }
                            }
                        }
                        for (int i = 2; i <= Convert.ToInt32(hdfDdlCountPS.Value); i += 2)
                        {
                            HiddenField hdf = (HiddenField)tdCol2.FindControl("hdfddlSP" + i.ToString());
                            DropDownList ddl = (DropDownList)tdCol2.FindControl("ddlSP" + i.ToString());
                            foreach (PropertyInfo p in objInvItemSpecDtl.GetProperties())
                            {
                                if (p.Name == hdf.Value)
                                {
                                    if (ddl.SelectedValue != CommonConstants.SELECTVAL)
                                        p.SetValue(invItemSpecDtlObj, Convert.ToInt32(ddl.SelectedValue), null);
                                }
                            }
                        }
                        invItemSpecDtlObj.ISD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);


                        invItemSpecDtlObj.INV_ITEM_MST = CommonFunctions.Initilize<INV_ITEM_MST>();
                        invItemSpecDtlObj.INV_ITEM_MST.ITM_NAME = invItemSpecDtlObj.INV_ITEM_MST.ITM_CODE = string.Empty;                      
                        invItemSpecDtlObj.INV_ITEM_MST.ITM_PK = string.IsNullOrEmpty(hdfSPProductPK.Value) ? 0 : Convert.ToInt32(hdfSPProductPK.Value); 

                        invItemSpecDtlList = costomerRegistrationServiceClient.SearchProducts(invItemSpecDtlObj, currentUser.SBUID);
                        break;
                    case ControlsEnum.CONTROLS:
                        ERPService.Inventory.InvItemMstService InvItemMstServiceClient = null;
                        InvItemMstServiceClient = new ERPService.Inventory.InvItemMstService();
                        InvItemMstServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(InvItemMstServiceClient);
                        spAdmConstGrpGetKvResultList = InvItemMstServiceClient.GetControlsList(null, null, (int)ConstGroupType.Product, null, Convert.ToByte(DbActiveStatus.ACTIVE), null, null);
                        break;
                    case ControlsEnum.BINDDROPDOWN:
                        CommonServiceClient = new CommonService();
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, Convert.ToByte(DbActiveStatus.ACTIVE), null, (int)ConstGroupType.Product, CngValue, null);
                        break;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                costomerRegistrationServiceClient = null;
            }
        }
        #endregion
        #region Set Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void SetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        BindGrid(ControlsEnum.DEFAULT);
                        break;
                    case ControlsEnum.BINDDROPDOWN:
                        BindDropDown(DropDownID);
                        break;
                    case ControlsEnum.CONTROLS:
                        LoadControls();
                        break;
                }
            }
            catch
            {
                throw;
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

            try
            {
                bool bIsChecked = false;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region Search
                    case ActionsEnum.SEARCH:
                        #region Old code
                      //if (string.IsNullOrEmpty(txtType.Text) || txtType.Text.Equals(Resources.ErpRes.AutoDefaultValue) )
                      //      hdfType.Value="0";
                      //  if (string.IsNullOrEmpty(txtThickness.Text) || txtThickness.Text.Equals(Resources.ErpRes.AutoDefaultValue))
                      //      hdfThickness.Value = "0";
                      //  if (string.IsNullOrEmpty(txtCategory.Text) || txtCategory.Text.Equals(Resources.ErpRes.AutoDefaultValue))
                      //      hdfCategory.Value = "0";
                      //  if (string.IsNullOrEmpty(txtSurface.Text) || txtSurface.Text.Equals(Resources.ErpRes.AutoDefaultValue))
                      //      hdfSurface.Value="0";
                      //  if (string.IsNullOrEmpty(txtShade.Text) || txtShade.Text.Equals(Resources.ErpRes.AutoDefaultValue))
                      //      hdfShade.Value = "0";
                      //  if (string.IsNullOrEmpty(txtClassification.Text) || txtClassification.Text.Equals(Resources.ErpRes.AutoDefaultValue))
                      //      hdfClassification.Value = "0";
                      //  if (string.IsNullOrEmpty(txtSize.Text) || txtSize.Text.Equals(Resources.ErpRes.AutoDefaultValue))
                      //      hdfSize.Value = "0";
                      //  if (string.IsNullOrEmpty(txtLength.Text) || txtLength.Text.Equals(Resources.ErpRes.AutoDefaultValue))
                      //      hdfLength.Value = "0";

                      //  GetFieldValues(ControlsEnum.DEFAULT);
                      //  SetFieldValues(ControlsEnum.DEFAULT); 
	              #endregion   
                  GetFieldValues(ControlsEnum.DEFAULT);
                  SetFieldValues(ControlsEnum.DEFAULT); 
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSearchProducts", "ShowContainerDiv('#divSearchProducts','" + "Product Search" + "','950','400');", true);
                        break;
                    #endregion
                    case ActionsEnum.SELECT:
                        foreach (GridViewRow grdrow in grdSearchProduct.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ItemPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfProductID")).Value);
                                break;
                            }
                        }
                        if (!bIsChecked)
                        {
                            ItemPK = 0;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSearchProducts", "ShowContainerDiv('#divSearchProducts','" + "Product Search" + "','950','400');", true);
                        }
                        else
                        {
                            ResetForm();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                    "ClosePopup();", true);
                            ((Button)sender).CommandName = ActionsEnum.SELECT.ToString();
                            SELECT(sender, e);
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region --- For Grid Actions----
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            
        }

         /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
        }
         
        #endregion


        #endregion
        #region Pager Methods + Init
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
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            //this.lnkFuel.PreRender += new EventHandler(btnAction_PreRender);
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //(this.Page as MyBasePage).CheckBtnVisibility(sender);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeUserControlComponents", "$(document).ready(function(){UserControlInitComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Helper Mathods
        /// <summary>
        /// Method used to Reset form Controls
        /// </summary>
        public void ResetForm()
        {
            txtType.Text = string.Empty;
            hdfType.Value = "0";
            txtThickness.Text = string.Empty;
            hdfThickness.Value = "0";
            txtCategory.Text = string.Empty;
            hdfCategory.Value = "0";
            txtSurface.Text = string.Empty;
            hdfSurface.Value = "0";
            txtShade.Text = string.Empty;
            hdfShade.Value = "0";
            txtClassification.Text = string.Empty;
            hdfClassification.Value = "0";
            txtSize.Text = string.Empty;
            hdfSize.Value = "0";
            txtLength.Text = string.Empty;
            hdfLength.Value = "0";
            grdSearchProduct.DataSource = null;
            grdSearchProduct.DataBind();           
            txtSPAdvProduct.Text = Resources.Messages.AutoDefaultValue;
            hdfSPProductPK.Value = "0";
           //For Clearing Dyanmic ddl Selection
            for (int i = 1; i <= Convert.ToInt32(hdfDdlCountPS.Value); i += 2)
            {                
                DropDownList ddl = (DropDownList)tdCol1.FindControl("ddlSP" + i.ToString());              
                ddl.ClearSelection();
            }
            for (int i = 2; i <= Convert.ToInt32(hdfDdlCountPS.Value); i += 2)
            {             
                DropDownList ddl = (DropDownList)tdCol2.FindControl("ddlSP" + i.ToString());             
                ddl.ClearSelection();
            }           
        }
        /// <summary>
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        private void SetUIEditView(ActionsEnum mode)
        {
            try
            {
                switch (mode)
                {

                    

                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        private void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.DEFAULT:
                        grdSearchProduct.DataSource = invItemSpecDtlList;
                        grdSearchProduct.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Load Controls to UI
        /// </summary>
        public void LoadControls()
        {
            if (spAdmConstGrpGetKvResultList != null && spAdmConstGrpGetKvResultList.Count > 0)
            {                
                string[] pdtAttrSearchArray;
                pdtAttrSearchArray = GetGlobalResourceObject("ConfigurationsRes", "ProductAttrCodeForSearch").ToString().Split(',');
                
                var pdtattrList = spAdmConstGrpGetKvResultList.Where(o => pdtAttrSearchArray.Any(s => o.CNG_CODE.Contains(s)))
                                  .ToList();  

                DataTable dtFieldControls = LINQToDataTable(pdtattrList);
                hdfDdlCountPS.Value = dtFieldControls.Rows.Count.ToString();

                HtmlGenericControl div1 = new HtmlGenericControl("div");
                div1.Attributes.Add("class", "div2col-S");
                tdCol1.Controls.Add(div1);

                for (int i = 0; i < dtFieldControls.Rows.Count; i += 2)
                {
                    Label lbl = new Label();
                    DropDownList ddl = new DropDownList();
                    HiddenField hdf = new HiddenField();
                    RequiredFieldValidator vrf = new RequiredFieldValidator();

                    ddl.ID = "ddlSP" + (i + 1).ToString();
                    ddl.CssClass = "select-half-a";
                    lbl.ID = "lbl" + (i + 1).ToString();
                    lbl.Text = dtFieldControls.Rows[i][Resources.DataFieldRes.GroupName].ToString();
                    lbl.AssociatedControlID = ddl.ID;

                    ddl.TabIndex = (short)(19 + i);
                    //if (dtFieldControls.Rows[i].ItemArray[11].ToString().Contains("USE_CODE"))
                    //{
                    //    ddl.Attributes.Add("onChange", "GenerateProductCode();");
                    //    ddl.Attributes.Add("canChangeProductCode", "1");
                    //}
                    //else
                    //{
                    //    ddl.Attributes.Add("canChangeProductCode", "0");
                    //}

                    //vrf.ID = "vrf" + Regex.Replace(lbl.Text, @"[^0-9a-zA-Z]+", "");
                    //vrf.CssClass = "star";
                    //vrf.SetFocusOnError = true;
                    //vrf.InitialValue = "-1";
                    //vrf.ValidationGroup = "Product";
                    //vrf.EnableClientScript = true;
                    //vrf.Display = ValidatorDisplay.Dynamic; 
                    //vrf.Text = "*";
                    //vrf.ControlToValidate = ddl.ID;
                    //vrf.ErrorMessage = string.Format(lbl.Text, GetLocalResourceObject("Err_Attributes"));
                    hdf.ID = "hdf" + ddl.ID;
                    hdf.Value = dtFieldControls.Rows[i][Resources.DataFieldRes.GroupCode].ToString();
                    DropDownID = ddl;
                    div1.Controls.Add(lbl);
                    div1.Controls.Add(ddl);
                    //div1.Controls.Add(vrf);
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

                    ddl.ID = "ddlSP" + (i + 1).ToString();
                    ddl.CssClass = "select-half-a";
                    lbl.ID = "lbl" + (i + 1).ToString();
                    lbl.Text = dtFieldControls.Rows[i][Resources.DataFieldRes.GroupName].ToString();
                    lbl.AssociatedControlID = ddl.ID;
                    ddl.TabIndex = (short)(19 + i);
                    //if (dtFieldControls.Rows[i].ItemArray[11].ToString().Contains("USE_CODE"))
                    //{
                    //    ddl.Attributes.Add("onChange", "GenerateProductCode();");
                    //    ddl.Attributes.Add("canChangeProductCode", "1");
                    //}
                    //else
                    //{
                    //    ddl.Attributes.Add("canChangeProductCode", "0");
                    //}

                    //vrf.ID = "vrf" + Regex.Replace(lbl.Text, @"[^0-9a-zA-Z]+", "");
                    //vrf.CssClass = "star";
                    //vrf.SetFocusOnError = true;
                    //vrf.InitialValue = "-1";
                    //vrf.ValidationGroup = "Product";
                    //vrf.EnableClientScript = true;
                    //vrf.Display = ValidatorDisplay.Dynamic;
                    //vrf.Text = "*";
                    //vrf.ControlToValidate = ddl.ID;
                    //vrf.ErrorMessage = string.Format(lbl.Text, GetLocalResourceObject("Err_Attributes"));
                    hdf.ID = "hdf" + ddl.ID;
                    hdf.Value = dtFieldControls.Rows[i][Resources.DataFieldRes.GroupCode].ToString();
                    DropDownID = ddl;
                    div2.Controls.Add(lbl);
                    div2.Controls.Add(ddl);
                    //div2.Controls.Add(vrf);
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
       
        #endregion
        #region ControlsEnum
        /// <summary>
        /// Controls Enum for the page
        /// </summary>
        private enum ControlsEnum
        {
            DEFAULT,
            CONTROLS,
            BINDDROPDOWN
        }
        #endregion
    }
}