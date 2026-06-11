using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using BusinessObject;
using System.Data;
using BusinessObject.CommonManagement;

namespace CustomerPortal.OrderToCash
{
    public partial class BrandPriceList : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        private ActionsEnum commonActions;
        User currentUser;
        private DataTable dtPageData;
        private DataTable dtBrandPriceList;
        private int customerPK;
        DateTime fromDate, toDate;
          
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
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    //txtFromDt.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("MMM-yyyy");
                    //txtToDt.Text = DateTime.Now.ToString("MMM-yyyy");
                    GetFieldValues(ControlsEnum.USERCUSTOMER);
                    SetFieldValues(ControlsEnum.USERCUSTOMER);
                    //if (hdfIsCustomerLog.Value == "0")
                    //{
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                    //}
                }
            }
            catch (Exception ex)
            {
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
            string fromDate = string.Empty;
            string toDate = string.Empty;
            try
            {
                switch (type)
                {
                    case ControlsEnum.USERCUSTOMER:
                        dtPageData = new DataTable();
                        dtPageData = BusinessLogic.CommonManagement.CommonManagement.GetUserCustomer(currentUser.PKUser, currentUser.SBUID);
                        break;
                    case ControlsEnum.DEFAULT:
                          //customerPK = hdfCustomer.Value != "" ? Convert.ToInt32(hdfCustomer.Value) : 0;
                        customerPK = 0;
                        dtBrandPriceList = new DataTable();

                        //fromDate = txtFromDt.Text!=string.Empty ? "01-" + txtFromDt.Text : string.Empty;
                        //toDate = txtToDt.Text != string.Empty ? "01-" + txtToDt.Text : string.Empty;
                        dtBrandPriceList = BusinessLogic.BrandRates.BrandRatesBL.GetBrandPriceList(customerPK, fromDate, toDate);
                        break;
                    case ControlsEnum.SEARCH:
                        customerPK = hdfCustomer.Value != "" ? Convert.ToInt32(hdfCustomer.Value) : 0;
                        dtBrandPriceList = new DataTable();

                        fromDate = txtFromDt.Text!=string.Empty ? "01-" + txtFromDt.Text : string.Empty;
                        toDate = txtToDt.Text != string.Empty ? "01-" + txtToDt.Text : string.Empty;
                        dtBrandPriceList = BusinessLogic.BrandRates.BrandRatesBL.GetBrandPriceList(customerPK, fromDate, toDate);
                        break;
                    //case  ControlsEnum.SEARCH:
                    //    customerPK = hdfCustomer.Value != "" ? Convert.ToInt32(hdfCustomer.Value) : 0;
                    //    dtBrandPriceList = new DataTable();
                    //    dtBrandPriceList = BusinessLogic.BrandRates.BrandRatesBL.GetBrandPriceList(customerPK, txtFromDt.Text.Trim(), txtToDt.Text.Trim());
                    //    break;
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
                    case ControlsEnum.USERCUSTOMER:
                        if (dtPageData.Rows.Count > 0)
                        {
                            txtCustomer.Text = dtPageData.Rows[0]["CUS_NAME"].ToString();
                            hdfCustomer.Value = dtPageData.Rows[0]["CUS_PK"].ToString();
                            hdfIsCustomerLog.Value = "1";
                            ddlPrint.Enabled = false ;
                            txtCustomerPrint.Enabled = false;
                            txtCustomerPrint.Text = dtPageData.Rows[0]["CUS_NAME"].ToString();
                            hdfCustomerPrint.Value = dtPageData.Rows[0]["CUS_PK"].ToString();
                        }
                        else
                        {
                            hdfIsCustomerLog.Value = "0";
                            ddlPrint.Enabled = true;
                            txtCustomerPrint.Enabled = true;
                        }
                        break;
                    case ControlsEnum.DEFAULT:
                    case ControlsEnum.SEARCH:
                        BindGrid(controlType);
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
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                if (dtBrandPriceList != null && dtBrandPriceList.Rows.Count > 0)
                    grdBrandPriceList.DataSource = dtBrandPriceList;
                else
                    grdBrandPriceList.DataSource = null;
                grdBrandPriceList.DataBind();
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
            txtCustomer.Text = string.Empty;
            txtFromDt.Text = string.Empty;
            hdfFromDt.Value = string.Empty;
            txtToDt.Text = string.Empty;
            hdfToDt.Value = string.Empty;
            hdfCustomer.Value = "0";
            //txtFromDt.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            //hdfFromDt.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            //txtToDt.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            //hdfToDt.Value = DateTime.Now.ToString();
            //txtFromDt.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("MMM-yyyy");
            //txtToDt.Text = DateTime.Now.ToString("MMM-yyyy");
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
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
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_v01.ERPSMS_2).ValidatePageDept())
                return;

            string fromMonth = "";
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
               
                switch (commonActions)
                {
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.SEARCH);
                        break;
                    case ActionsEnum.CLEAR:
                        ResetForm();                       
                        break;
                    case ActionsEnum.SHOW:
                         GetFieldValues(ControlsEnum.DEFAULT);
                         SetFieldValues(ControlsEnum.DEFAULT);
                        break;
                    case ActionsEnum.PRINTGRID:
                        int BrandRatePK;
                        GridViewRow grdrowEdit = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        BrandRatePK = (grdrowEdit.FindControl("hdfBrID") as HiddenField).Value == string.Empty ? 0 : Convert.ToInt32((grdrowEdit.FindControl("hdfBrID") as HiddenField).Value);
                        fromMonth = "01-" + (grdrowEdit.FindControl("lblFromDate") as Label).Text;

                        hdfbrandRatePK.Value = BrandRatePK.ToString();
                        hdffromMonth.Value = fromMonth;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + GetLocalResourceObject("BrandRates").ToString() + "','400','200');", true);
                        ddlPrint.Focus();

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&ForMoth=" + fromMonth + "&GroupBy=1');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + BrandRatePK.ToString() + "&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=0" + "&CusID=" + hdfCustomer.Value + "');", true);
                        break;
                    case ActionsEnum.PRINT:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);

                        if (ddlPrint.SelectedValue == "1")
                        {
                            if (txtCustomerPrint.Text != string.Empty && txtCustomerPrint.Text != "All" && hdfCustomerPrint.Value != string.Empty && hdfCustomerPrint.Value != "0")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&CusID=" + hdfCustomerPrint.Value + "&ForMoth=" + hdffromMonth.Value + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&ForMoth=" + hdffromMonth.Value + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);

                            }
                        }
                        else if (ddlPrint.SelectedValue == "2")
                        {
                            if (txtProduct.Text != string.Empty && txtProduct.Text != "All" && hdfProduct.Value != string.Empty && hdfProduct.Value != "0")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&ItemID=" + hdfProduct.Value + "&ForMoth=" + hdffromMonth.Value + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&ForMoth=" +  hdffromMonth.Value + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);

                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=&APPTYPE=" + ApplicationType.CBR + "&APPSUBTYPE=" + "&ForMoth=" +  hdffromMonth.Value + "&GroupBy=" + ddlPrint.SelectedValue + "');", true);

                        }
                        break;
                }
            }
            catch (Exception ex)
            {}

            finally
            {}
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

        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
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

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {

            USERCUSTOMER,
            DEFAULT,
            SEARCH,
            CLEAR
        }

        #endregion

    }
}