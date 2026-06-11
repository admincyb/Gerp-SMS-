using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject.AccountManagement;
using ERPData;
using ERPManager;
using ERPService.Employee;
using BusinessObject.CommonManagement;
using ERPSMS_v01.UserControls;
using System.Data;
using BusinessLogic.CommonManagement;
using System.Configuration;
using BusinessObject.Administration.Configurations;

namespace ERPSMS_v01.Employee
{
    public partial class EmpMst : System.Web.UI.Page
    {
        private List<EmpEmployeeMst> employeeMstLst;
        private EmpEmployeeMst employeeMstObj;
        private short typePk;
        BusinessObject.User currentUser;
        private ServiceUtility serviceUtilityObj;
        private ActionsEnum commonActions;
        DataTable dtRoleMapping;
        #region Properties

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
        public RoleMappingHeader RoleMappingHeaderViewState
        {
            get
            {
                if (this.ViewState["RoleMappingHeader"] == null)
                {
                    this.ViewState["RoleMappingHeader"] = new RoleMappingHeader();
                }
                return (RoleMappingHeader)this.ViewState["RoleMappingHeader"];
            }
            set
            {
                this.ViewState["RoleMappingHeader"] = value;
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
        private int CurrStatus
        {
            get
            {
                return this.ViewState["CurrStatus"] == null ? 0 : Convert.ToInt32(this.ViewState["CurrStatus"]);
            }
            set
            {
                this.ViewState["CurrStatus"] = value;
            }
        }
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

        private int TotalUsersCount
        {
            get
            {
                return (int)this.ViewState["TotalUsersCount"];
            }
            set
            {
                this.ViewState["TotalUsersCount"] = value;
            }
        }


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

        private void BindRoleMappingTreeView()
        {
            bool checkAll = true;
            //trvEmpRole.Nodes.Clear();
            TreeNode masterNode = new TreeNode(GetLocalResourceObject("Role").ToString(), "0");
            foreach (var item in RoleMappingHeaderViewState.RoleList)
            {
                TreeNode node = new TreeNode(item.EMP_ROLE_TEXT, item.ermRole.ToString());
                if (item.IS_CHECKED == "true")
                    node.Checked = true;
                else
                    checkAll = false;
              //  trvEmpRole.Nodes.Add(node);
            }
            //trvEmpRole.ExpandAll();
        }

        #endregion
       
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    //SetConfigValue();
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.EmployeePK;
                    grdEmployee.DataKeyNames = datakeyarray;
                  //GetFieldValues(ControlsEnum.EMPLOYEETYPE);
                 // SetFieldValues(ControlsEnum.EMPLOYEETYPE);
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    btnNew.Focus();
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
         private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            EmployeesService employeeServiceClient;
            employeeServiceClient = null;
            try
            {
                employeeServiceClient = new EmployeesService();
                employeeServiceClient = CommonFunctions.InitiateClient(employeeServiceClient);
                employeeMstObj = employeeServiceClient.GetInitilizedEmpEmployeeMst();
                switch (type)
                {
                 case ControlsEnum.DEFAULT:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdEmployee.PageSize;
                        serviceUtilityObj.FilterBy = ddlFilterBy.SelectedValue == "0" ? null : ddlFilterBy.SelectedValue;
                        serviceUtilityObj.FilterValue = txtSearchBy.Text.Trim();
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.EmployeeCode : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        employeeMstObj.empPK = CurrPK;
                     
                 
                        employeeMstObj.empBizUnit = -1;
                        employeeMstObj.empDept = currentUser.CurrentDeptPK;
                        employeeMstObj.empActive = Convert.ToByte(DbActiveStatus.HASPK);
                        employeeMstLst = employeeServiceClient.GetEmpEmployeeMst(employeeMstObj, serviceUtilityObj);
                        serviceUtilityObj = employeeServiceClient.GetEmpEmployeeMstCount(employeeMstObj, serviceUtilityObj);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                     break;
                 case ControlsEnum.EMPLOYEE:
                     serviceUtilityObj = new ServiceUtility();
                     serviceUtilityObj.CurrentPage = -1;
                     serviceUtilityObj.PageSize = -1;
                     employeeMstObj.empPK = CurrPK;
                     employeeMstObj.empActive = Convert.ToByte(DbActiveStatus.HASPK);
                     employeeMstObj.empBizUnit = -1;
                     //employeeMstObj.empBizUnit = Convert.ToInt16(currentUser.SBUID);
                     employeeMstObj.empDept = currentUser.CurrentDeptPK;
                     employeeMstLst = employeeServiceClient.GetEmpEmployeeMst(employeeMstObj, serviceUtilityObj);

                     dtRoleMapping = BusinessLogic.Administration.Configurations.EmployeesBL.GetEmployeeRoles(CurrPK, currentUser.SBUID);
                     RoleMappingHeaderViewState.RoleList = dtRoleMapping.ToList<RoleMapping>();
                     break;
                 case ControlsEnum.EMPLOYEECOUNT:
                     serviceUtilityObj = new ServiceUtility();
                     employeeMstObj.empPK = 0;
                     //employeeMstObj.empBizUnit = Convert.ToInt16(currentUser.SBUID);
                     employeeMstObj.empBizUnit = -1;
                     employeeMstObj.empDept = currentUser.CurrentDeptPK;
                     employeeMstObj.empActive = Convert.ToByte(DbActiveStatus.ACTIVE);
                     serviceUtilityObj = employeeServiceClient.GetEmpEmployesCount(employeeMstObj, serviceUtilityObj);
                     TotalUsersCount = serviceUtilityObj.TotalRecords;
                     break;
                }
                //employeeServiceClient.Close();
            }
            catch (Exception ex)
            {
                //employeeServiceClient.Abort();
                throw ex;
            }
            finally
            {
                employeeMstObj = null;
                serviceUtilityObj = null;
                employeeServiceClient = null;
            }

        }

         #region Set Field Values

         private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                          
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        break;
                    case ControlsEnum.EMPLOYEE:
                        //Used Fill the UI Values From Object
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


         /// <summary>
         /// Sets the UI input controls from the object values
         /// </summary>
         private void GetUIValuesFromObject()
         {
             currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

             try
             {
                 //assigning the UI controls with the corresponding value from the employeeMstLst
                 if (employeeMstLst != null && employeeMstLst.Count() > 0)
                 {
                     CurrPK = employeeMstLst[0].empPK;
                     if (employeeMstLst[0].EmpCountry1.HasValue)
                     {
                         hdfCountry.Value = Convert.ToString(employeeMstLst[0].EmpCountry1.Value);
                         txtCountry.Text = employeeMstLst[0].ADM_COUNTRY_MST.CNT_NAME;
                     }
                     //txtDesignation.Text = HttpUtility.HtmlDecode(employeeMstLst[0].EmpDesignationMst.dsgName);
                     //hdfDesignation.Value = employeeMstLst[0].empDesignation.ToString();
                     txtEmployeeName.Text = HttpUtility.HtmlDecode(employeeMstLst[0].empName);
                     txtEmployeeCode.Text = HttpUtility.HtmlDecode(employeeMstLst[0].empCode);
                     typePk = employeeMstLst[0].empType;
                     //ddlEmpType.SelectedIndex = Convert.ToInt32(ddlEmpType.Items.IndexOf(ddlEmpType.Items.FindByValue(typePk.ToString())));
                     txtEmployeeAddress.Text = HttpUtility.HtmlDecode(employeeMstLst[0].EmpAddress1);
                     txtEmployeeCity.Text = HttpUtility.HtmlDecode(employeeMstLst[0].EmpCity1);
                     //txtDUMax.Text = employeeMstLst[0].empDayUtiliznMax.ToString();
                     //txtDUMin.Text = employeeMstLst[0].empDayUtiliznMin.ToString();
                     if (employeeMstLst[0].empDOB.HasValue)
                         txtEmployeeDOB.Text = employeeMstLst[0].empDOB.Value.ToString(Resources.Constants.DateFormatShort);
                     else txtEmployeeDOB.Text = string.Empty;
                     if (employeeMstLst[0].empDOJ.HasValue)
                         txtEmployeeDOJ.Text = employeeMstLst[0].empDOJ.Value.ToString(Resources.Constants.DateFormatShort);
                     else txtEmployeeDOJ.Text = string.Empty;
                     ddlStatus.SelectedIndex = Convert.ToInt32(ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(employeeMstLst[0].empActive.ToString())));
                     CurrStatus = Convert.ToInt32(ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(employeeMstLst[0].empActive.ToString())));
                     lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                     LastModifiedTime = employeeMstLst[0].empModOn;
                     //BindRoleMappingTreeView();
                 }
                 //Concurrency EmployeeMaster Deleted By Another User
                 else
                 {
                     litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                     litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Employee);
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
         /// Method used to Reset form Controls
         /// </summary>
         private void ResetForm()
         {
             CurrPK = 0;
             txtSearchBy.Text = string.Empty;
             txtEmployeeCode.Text = string.Empty;
             txtEmployeeCity.Text = string.Empty;
             txtEmployeeAddress.Text = string.Empty;
             txtEmployeeDOB.Text = string.Empty;
             txtEmployeeDOJ.Text = string.Empty;
             txtEmployeeName.Text = string.Empty;
             ddlFilterBy.SelectedIndex = 0;
             ddlStatus.SelectedIndex = 0;
             //ddlEmpType.SelectedIndex = 0;
             PageIndex = "1";
             lblLastModifiedHDR.Text = string.Empty;
             ModifiedDatePnl.Visible = false;
             //txtDesignation.Text = string.Empty;
             //hdfDesignation.Value = "0";
             txtCountry.Text = string.Empty;
             hdfCountry.Value = string.Empty;
             CurrStatus = 0;
             employeeMstLst = null;
             //txtDUMax.Text = string.Empty;
             //txtDUMin.Text = string.Empty;
             ActivateEmployeeList();
         }

         private void ActivateEmployeeList()
         {
            //spnEmployeeRoles.Attributes["class"] = "tab-inactive";
              spnEmployeeList.Attributes["class"] = "tab-active";
            //lbnEmployeeRoles.CssClass = "tab-inactive";
             lbnEmployee.CssClass = "tab-active";
            //divRoleMapping.Visible = false;
             PageAction_Entry.Visible = false;
         }

         private bool IsChecked()
         {
             try
             {
                 foreach (GridViewRow grdrow in grdEmployee.Rows)
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

         private void SetUIEditView(ActionsEnum Mode)
         {
             try
             {
                 foreach (GridViewRow grdrow in grdEmployee.Rows)
                 {
                     RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                     // check row selected or not                    
                     if (!IsChecked())
                     {
                         // if no items selected, Show Error Message
                         litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                         ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                         EntryStatus = EntryStatus.LISTMODE;
                         return;
                     }
                     if (rbtn.Checked)
                     {
                         // get pk from the grid and assign to CurrPk
                         CurrPK = Convert.ToInt32(grdEmployee.DataKeys[grdrow.RowIndex].Values[0]);
                         // Get And Set the Employee Values
                         GetFieldValues(ControlsEnum.EMPLOYEE);
                         //if (Mode == ActionsEnum.ADDROLE)
                         //{
                         //    if (employeeMstLst != null && employeeMstLst.Count() > 0)
                         //    {
                         //        employeeMstObj = new EmpEmployeeMst();
                         //        employeeMstObj.empPK = employeeMstLst[0].empPK;
                         //        employeeMstObj.empName = employeeMstLst[0].empName;
                         //        employeeMstObj.empCode = employeeMstLst[0].empCode;
                         //        // Passing the Employee object to Employee Roles Page
                         //        //Session[SessionStrings.Employee] = employeeMstObj;
                         //    }
                         //    //Response.Redirect(Resources.PageURL.EmployeeRoles, true);
                         //}
                         SetFieldValues(ControlsEnum.EMPLOYEE);
                         txtEmployeeCode.Focus();
                         ModifiedDatePnl.Visible = true;
                         ActivateEmployeeDetails();

                         if (Mode == ActionsEnum.VIEW)
                         {
                             EntryStatus = EntryStatus.VIEWMODE;
                         }
                         else
                             EntryStatus = EntryStatus.ENTRYMODE;
                         return;
                     }
                 }
                 // if no items selected, Show Error Message
                 litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                 ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                 EntryStatus = EntryStatus.LISTMODE;
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         public void BindGrid()
         {
             try
             {
                 if (employeeMstLst != null)
                 {
                     uclPaging.TotalPages = TotalPages;
                     PageIndex = PageIndex == null ? "1" : PageIndex;
                     uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                     grdEmployee.DataSource = employeeMstLst;
                     grdEmployee.DataBind();
                     uclPaging.Visible = true;
                     uclPaging.BindPager();
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         private void ActivateEmployeeDetails()
         {
            // spnEmployeeRoles.Attributes["class"] = "tab-inactive";
             spnEmployeeList.Attributes["class"] = "tab-active";
            // lbnEmployeeRoles.CssClass = "tab-inactive";
             lbnEmployee.CssClass = "tab-active";
             //divRoleMapping.Visible = false;
             PageAction_Entry.Visible = true;
         }

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

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);

                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitDate1", "$(document).ready(function () {InitDate();});", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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


        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCanel.PreRender += new EventHandler(btnAction_PreRender);
        }

        void btnAction_PreRender(object sender, EventArgs e)
        {
            //base.CheckBtnVisibility(sender);
        }

        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        private EmpEmployeeMst SetUIValuesToObject()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {
                employeeMstObj.empPK = CurrPK;
                employeeMstObj.empCode = HttpUtility.HtmlEncode(txtEmployeeCode.Text.Trim());
                employeeMstObj.empName = HttpUtility.HtmlEncode(txtEmployeeName.Text.Trim());
                employeeMstObj.EmpCity1 = HttpUtility.HtmlEncode(txtEmployeeCity.Text.Trim());
                employeeMstObj.empType = 1;// Convert.ToInt16(ddlEmpType.SelectedValue);
                employeeMstObj.empCategory = 0;
                if (hdfCountry.Value != string.Empty && hdfCountry.Value != "0")
                    employeeMstObj.EmpCountry1 = Convert.ToInt16(hdfCountry.Value);
                employeeMstObj.empActive = Convert.ToByte(ddlStatus.SelectedValue);
                employeeMstObj.EmpAddress1 = HttpUtility.HtmlEncode(txtEmployeeAddress.Text.Trim());
                if (txtEmployeeDOB.Text.Trim() != string.Empty)
                    employeeMstObj.empDOB = Convert.ToDateTime(txtEmployeeDOB.Text.Trim());
                if (txtEmployeeDOJ.Text.Trim() != string.Empty)
                    employeeMstObj.empDOJ = Convert.ToDateTime(txtEmployeeDOJ.Text.Trim());

                employeeMstObj.empDayUtiliznMax = 0;// Convert.ToDouble(txtDUMax.Text.Trim());
                employeeMstObj.empDayUtiliznMin = 0;// Convert.ToDouble(txtDUMin.Text.Trim());
                employeeMstObj.empCrtdBy = Convert.ToInt16(currentUser.PKUser);

                employeeMstObj.empCrtdOn = System.DateTime.Now;
                employeeMstObj.empModBy = Convert.ToInt16(currentUser.PKUser);
                employeeMstObj.empBizUnit = Convert.ToInt16(currentUser.SBUID);
                employeeMstObj.empDept = Convert.ToInt32(currentUser.CurrentDeptPK);
                //employeeMstObj.empModOn = LastModifiedTime;
                employeeMstObj.empDesignation = 1;// Convert.ToInt16(hdfDesignation.Value);
                return employeeMstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                employeeMstObj = null;
            }
        }

        protected void ActionHandler(object sender, EventArgs e)
        {
            EmployeesService EmployeeServiceClient;
            EmployeeServiceClient = null;
            try
            {
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
                switch (commonActions)
                {
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
                            if ((CurrPK > 0
                                && CurrStatus != Convert.ToInt32(ddlStatus.SelectedValue)
                                && Convert.ToInt32(RecordStatus.ACTIVE) == Convert.ToInt32(ddlStatus.SelectedValue))
                               || CurrPK == 0)
                                //if (TotalUsersCount >= EmployeeLimit)
                                //{
                                //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Emp_Limit_Exeed;
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                //    return;
                                //}
                            employeeMstLst = new List<EmpEmployeeMst>();
                            EmployeeServiceClient = new EmployeesService();
                            EmployeeServiceClient = CommonFunctions.InitiateClient(EmployeeServiceClient);
                            employeeMstObj = EmployeeServiceClient.GetInitilizedEmpEmployeeMst();
                            employeeMstObj = SetUIValuesToObject();
                            employeeMstLst.Add(employeeMstObj);
                            result = EmployeeServiceClient.SaveEmpEmployeeMst(employeeMstLst);
                            if (result >= 0) // Success ! re-initialize the page
                            {
                              //  string roleXml = CreateRoleMappingXml();
                               // if (!string.IsNullOrEmpty(roleXml))
                                  //  result = BusinessLogic.Administration.Configurations.EmployeesBL.SaveEmployeeRoleDetails(roleXml);
                                if (result > 0)
                                {
                                    SortBy = Resources.DataFieldRes.EmployeePK;
                                    SortDirection = Resources.ErpRes.SortDescending;
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Employee);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                    //EmployeeServiceClient.Close();
                                    btnNew.Focus();
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Role_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                        #region DELETE
                    case ActionsEnum.DELETE:
                        // Delete Employee Details By empPK - Return 1 is Success , 0- Fail                                               
                        employeeMstLst = new List<EmpEmployeeMst>();
                        EmployeeServiceClient = new EmployeesService();
                        EmployeeServiceClient = CommonFunctions.InitiateClient(EmployeeServiceClient);
                        employeeMstObj = EmployeeServiceClient.GetInitilizedEmpEmployeeMst();
                        employeeMstObj.empPK = CurrPK;
                        employeeMstObj.empModOn = LastModifiedTime;
                        employeeMstLst.Add(employeeMstObj);
                        result = EmployeeServiceClient.DeleteEmpEmployeeMst(employeeMstLst);
                        if (result > 0)
                        {
                             litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                             ResetForm();
                             btnNew.Focus();
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.DEFAULT);
                           
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Employee);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        break;
                        #endregion

                    #region New
                    case ActionsEnum.NEW:
                        ModifiedDatePnl.Visible = false;
                        this.txtEmployeeCode.Focus();
                        EntryStatus = EntryStatus.NEWMODE;
                        ddlStatus.SelectedIndex = Convert.ToInt32(ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue("1")));
                        //txtDUMax.Text = "8";
                        //txtDUMin.Text = "0";
                        ActivateEmployeeDetails();
                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        this.btnNew.Focus();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
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

                    #region Edit
                    case ActionsEnum.EDIT:
                       SetUIEditView(commonActions);
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.VIEW:
                       SetUIEditView(commonActions);
                        btnCanel.Focus();
                        break;
                    #endregion

                    #region Print
                    case ActionsEnum.PRINT:
                        EntryStatus = EntryStatus.LISTMODE;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.ErrorMessages.Msg_Print_Error + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    #endregion
                  
                    #region Employee Details
                    case ActionsEnum.ADDEMPLOYEE:
                        if (!IsChecked())
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                            return;
                        }
                        foreach (GridViewRow grdrow in grdEmployee.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                CurrPK = Convert.ToInt32(grdEmployee.DataKeys[grdrow.RowIndex].Values[0]);
                                GetFieldValues(ControlsEnum.EMPLOYEE);
                                SetFieldValues(ControlsEnum.EMPLOYEE);
                                ModifiedDatePnl.Visible = false;
                                if (EntryStatus == EntryStatus.VIEWMODE)
                                {
                                    EntryStatus = EntryStatus.VIEWMODE;
                                }
                                else
                                    EntryStatus = EntryStatus.ENTRYMODE;
                                ActivateEmployeeDetails();
                            }
                        }
                        break;
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
                employeeMstLst = null;
                employeeMstObj = null;
                EmployeeServiceClient = null;
            }
        }
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
 

        #region ControlEnum
        public enum ControlsEnum
        {
            EMPLOYEE,
            DEFAULT,
            EMPLOYEECOUNT
        }
        #endregion

    }
       
}