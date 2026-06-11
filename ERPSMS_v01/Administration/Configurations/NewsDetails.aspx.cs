using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.AccountManagement;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using BusinessLogic.Administration.Configurations;
using ERP.Utilities;
using GTIService.Constants.Administration.Configurations;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class NewsDetails : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        #region Properties

        /// <summary>
        /// To maintain the sort order in viewstate
        /// </summary>
        private string SortOrder
        {
            get
            {
                return (string)this.ViewState["SortOrder"];
            }
            set
            {
                this.ViewState["SortOrder"] = value;
            }
        }

        /// <summary>
        /// To maintain the sort expression in viewstate
        /// </summary>
        private string SortExpression
        {
            get
            {
                return (string)this.ViewState["SortExpression"];
            }
            set
            {
                this.ViewState["SortExpression"] = value;
            }
        }

        /// <summary>
        /// To maintain the sort field in viewstate
        /// </summary>
        private string SortFilter
        {
            get
            {
                return (string)this.ViewState["SortFilter"];
            }
            set
            {
                this.ViewState["SortFilter"] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndex
        {
            get
            {
                return (string)this.ViewState["PageIndex"];
            }
            set
            {
                this.ViewState["PageIndex"] = value;
            }
        }

        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["CurrPK"]);
            }
            set
            {
                this.ViewState["CurrPK"] = value;
            }
        }

        #endregion
        // Holds the current logged in user
        private BusinessObject.User currentUser;

        ////Rename the Template with corresponding Page Name eg if CostCenter  then dtCostcenter
        //Data table for binding the Template Grid
        private DataTable dtTemplate;

        // Indicates the state as well as action
        private ActionsEnum commonActions;

        private int section;

        #endregion

        #region Set Page Variables
        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
            //Initialze the current logged in user to the currentUser variable
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //For Assigning the Bread crum Value
            ((Label)this.Master.FindControl("lblBreadCrum")).Text = this.GetLocalResourceObject("Breadcrumb").ToString();
            if (!IsPostBack || (base.GetPostBackControl() != null && base.GetPostBackControl().GetType().IsEquivalentTo(typeof(Button))))
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails1", "ShowHide('ADD_DETAILS');", true);
            }
            section = 0;

        }
        #endregion

        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(string type)
        {

            DbActiveStatus opMode; //Indicates which mode of operation need to be performed in DB
            switch (type)
            {
                case "GRID":
                    opMode = commonActions == ActionsEnum.EDIT_ACTION ? DbActiveStatus.HASPK : DbActiveStatus.INACTIVE; //2 - Edit mode ; 0 - Normal Lists all
                    //Change the Currency BL with the corresponding BLs Get Function
                    //dtTemplate = CurrenciesBL.GetCurrencys(CurrPK, opMode, currentUser.SBU);
                    break;
                default: // if passed nothing or string.empty(), then Meand Default Bing Will Bind all the Data in initial stage
                    //Change the Currency BL with the corresponding BLs Get Function
                    dtTemplate = new DataTable();//CurrenciesBL.GetCurrencys(CurrPK, 0, currentUser.SBU);
                    break;
            }
        }

        #endregion

        #region Set Field Values

        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(string type)
        {
            switch (type)
            {
                case "GRID":
                    if (commonActions == ActionsEnum.EDIT_ACTION) // Edit mode. Fill the details of the edit record to the corresponding fields
                    {
                        if (dtTemplate.Rows.Count > 0)
                        {
                            GetUIValuesFromObject();
                        }
                    }
                    else
                    {
                        BindGrid();
                    }
                    break;
                default: // if passed nothing or string.empty(), then Bind for Initail
                    BindGrid();
                    break;
            }
        }

        #endregion

        #region Action Handlers

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click (Save/Cancel)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));

            pnlEdit.Visible = false;
            pnlSave.Visible = false;
            switch (commonActions)
            {
                case ActionsEnum.SAVE:

                    break;
                case ActionsEnum.CANCEL:

                    break;
                case ActionsEnum.EDIT:

                    break;
                case ActionsEnum.VIEW:


                    break;
                case ActionsEnum.NEW:
                    pnlSave.Visible = true;
                    ResetForm();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDetails", "ShowHide();", true);
                    break;
                case ActionsEnum.DELETE:

                    break;
                case ActionsEnum.ACTIVATE:

                    break;
                case ActionsEnum.DEACTIVATE:

                    break;
            }
        }
        #endregion

        #region --- For Grid Actions----
        /// <summary>
        /// Page Index Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            BindGrid();
        }
        /// <summary>
        /// Sorting Event Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            SortExpression = e.SortExpression;
            BindGrid();
        }
        #endregion

        #endregion

        #region Page Level Events
        /// <summary>
        /// To handle pageload event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageVariables();
            if (!IsPostBack)
            {
                section = 1;
                GetFieldValues(string.Empty);
                SetFieldValues(string.Empty);
            }
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
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnActivate.PreRender += new EventHandler(btnAction_PreRender);
            this.btnInActivate.PreRender += new EventHandler(btnAction_PreRender);

            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);

            this.btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCanel.PreRender += new EventHandler(btnAction_PreRender);
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



        #endregion

        #region Helper Methods

        /// <summary>
        /// Binds the Currency grid with data
        /// </summary>
        private void BindGrid()
        {
            GetFieldValues("GRID");
            //SortExpression = SortExpression == null ? "SortByFieldName" : SortExpression;//TODO: get the value from constants
            //SortOrder = SortOrder == null ? "asc" : SortOrder;
            //PageIndex = PageIndex == null ? "0" : PageIndex;
            //SortFilter = SortExpression + " " + SortOrder;
            //dtTemplate .DefaultView.Sort = SortFilter;
            //dtTemplate.PageIndex = Convert.ToInt32(PageIndex);
            //dtTemplate.DataSource = dtTemplate.DefaultView;
            //dtTemplate.DataBind();
        }

        /// <summary>
        /// Resets the form for a fresh entry
        /// </summary>
        private void ResetForm()
        {
            //Codes for Clearing the controls in the page
            commonActions = ActionsEnum.ADD_ACTION;
            CurrPK = 0;
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        /// Rename the Currency BO with the corresponding BO
        private CurrenciesBO SetUIValuesToObject()
        {
            CurrenciesBO objCurrency;
            objCurrency = new CurrenciesBO();
            //objCurrency.BizUnit = currentUser.SBU;
            //objCurrency.ActiveStatus = 1;

            return objCurrency;
        }

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            //assigning the UI controls with the corresponding Datatable value dtTemplate
        }


        #endregion
    }
}