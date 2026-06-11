using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.HRMS.Employee;
using BusinessObject.AccountManagement;
using BusinessLogic.HRMS.Employee;
using DataAccess.CommonManagement;
using System.Data;
using ERPData;
using ERPService;
using ERPManager;
using BusinessObject.CommonManagement;
using BusinessObject;
using System.IO;
using CustomControls;
using BusinessLogic.CommonManagement;
using ERPSMS_v01;


namespace HRMS.Employees
{
    public partial class EmployeeSkills : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region  Properties
        public int CurrPK
        {
            get
            {
                return Convert.ToInt32(Session[SessionStrings.CurrentPK]);
            }
            set
            {
                Session[SessionStrings.CurrentPK] = value;
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
        private int categoryId
        {
            get
            {
                return this.ViewState[ViewstateStrings.categoryId] == null ? 0 : (int)this.ViewState[ViewstateStrings.categoryId];
            }
            set
            {
                this.ViewState[ViewstateStrings.categoryId] = value;
            }
        }

        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private BusinessObject.Common.EntryStatus EntryStatus
        {
            get
            {
                return this.Session[ViewstateStrings.EntryState] == null ? BusinessObject.Common.EntryStatus.ENTRYMODE : (BusinessObject.Common.EntryStatus)(this.Session[ViewstateStrings.EntryState]);
            }
            set
            {
                this.Session[ViewstateStrings.EntryState] = value;
            }
        }
        #endregion
        #region  Variables

        private EmployeeBasicInfomtn objEmployeeBasicInfo;
        DataSet dsSkillCategory;
        private EmployeeSkillsDetails EmployeeSkillsDetailsObj;
        private DataTable dtSkillLevel;
        DropDownList ddlSkillLevel;

        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private ADM_COMPANY_MST admCompanyMstObj;
        User currentUser;
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

            ButtonClicked();
            if (!IsPostBack)
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
                if (this.CurrPK == 0)
                {
                    litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    Session["SelectMessage"] = litErrorMsg.Text;
                    Response.Redirect(Resources.PageURL.EmployeeList);
                }
                if (this.CurrPK != 0)
                {
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);

                    GetFieldValues(ControlsEnum.SKILLCATEGORY);
                    SetFieldValues(ControlsEnum.SKILLCATEGORY);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }
        #endregion
        #region Get Field Values
        private void GetFieldValues(ControlsEnum type)
        {
            string showAllFlag;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region Get Employee Details
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        objEmployeeBasicInfo = EmployeeBasicInfoBL.GetEmployeebyID(CurrPK);
                       
                        break;
                    #endregion
                    #region Get Skill Category
                    case ControlsEnum.SKILLCATEGORY:
                        showAllFlag = (chkShowAllSkill.Checked ? "1" : "0");
                        dsSkillCategory = EmployeeSkillsBL.GetSkillCategoryList(CurrPK, showAllFlag, currentUser.SBUID, 1);
                        if (dsSkillCategory.Tables[0].Rows.Count>0)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString( dsSkillCategory.Tables[0].Rows[0]["ESD_MOD_DT"])) )
                            {
                                lblLastModifiedDate.InnerHtml = Resources.ErpRes.LastModifiedOn + Convert.ToDateTime(dsSkillCategory.Tables[0].Rows[0]["ESD_MOD_DT"].ToString()).ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                                ModifiedDatePnl.Visible = true;

                            }
                         
                        }
                        else
                        {
                            ModifiedDatePnl.Visible = false;
                        }
                       
                         //lblLastModifiedDate.InnerHtml = Resources.ErpRes.LastModifiedOn + Convert.ToDateTime((objEmployeeBasicInfo.LAST_MOD_DT.ToString())).ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                     
                        break;
                    #endregion
                    #region Get Skill Details
                    case ControlsEnum.SKILLDETAILS:
                        showAllFlag = (chkShowAllSkill.Checked ? "1" : "0");
                        EmployeeSkillsDetailsObj = EmployeeSkillsBL.GetSkillDetails(CurrPK, categoryId, showAllFlag, currentUser.SBUID);
                        break;
                    #endregion
                    #region Get Skill Level
                    case ControlsEnum.SKILLLEVEL:
                        dtSkillLevel = EmployeeSkillsBL.GetSkillLevelsList(0, 1, "EMP SKILL EXPERT LEVEL", currentUser.SBUID);
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
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Set Employee Details
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region Set Skill Category
                    case ControlsEnum.SKILLCATEGORY:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region Set Skill Level
                    case ControlsEnum.SKILLLEVEL:
                        BindDropDown(controlType);
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
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region Set - Employee Skill Details (Save)
                    case ControlsEnum.EMPLOYEESKILLDETAILS:
                        if (CurrPK != 0)
                        {
                            EmployeeSkillsDetailsObj.EMPLOYEE_PK = CurrPK;
                            EmployeeSkillsDetailsObj.BIZUNIT_PK = Convert.ToInt16(currentUser.SBUID);
                            EmployeeSkillsDetailsObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            EmployeeSkillsDetailsObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            EmployeeSkillsDetailsObj.LAST_MOD_DT = LastModifiedTime.ToString();
                            EmployeeSkillsDetailsObj.listEmployeeSkillsList = new List<SkillDetails>();
                            List<SkillDetails> detailsList = new List<SkillDetails>();
                            SkillDetails SkillDetailsObj;
                            foreach (GridViewRow inRowCat in grdSkillCategoryList.Rows)
                            {
                                HiddenField hdfSkillCategoryId = (HiddenField)inRowCat.FindControl("hdfSkillCategoryId");
                                GridView grdSkillDetails = inRowCat.FindControl("grdSkillDetails") as GridView;
                                foreach (GridViewRow inRowSkill in grdSkillDetails.Rows)
                                {
                                    CheckBox chkSkill = (CheckBox)inRowSkill.FindControl("chkSkill");
                                    if (chkSkill.Checked)
                                    {

                                        SkillDetailsObj = new SkillDetails();
                                        HiddenField hdfPk = (HiddenField)inRowSkill.FindControl("hdfPk");
                                        HiddenField hdfSkillCategoryPk = (HiddenField)inRowSkill.FindControl("hdfSkillCategoryPk");
                                        HiddenField hdfSkillPk = (HiddenField)inRowSkill.FindControl("hdfSkillPk");
                                        Label lblSkillArea = (Label)inRowSkill.FindControl("lblSkillArea");
                                        TextBox txtSkillYear = (TextBox)inRowSkill.FindControl("txtSkillYear");
                                        DropDownList ddlSkillLevel = (DropDownList)inRowSkill.FindControl("ddlSkillLevel");
                                        TextBox txtSkilRating = (TextBox)inRowSkill.FindControl("txtSkilRating");
                                        TextBox txtSkilRemarks = (TextBox)inRowSkill.FindControl("txtSkilRemarks");
                                        if (chkSkill.Checked == true && txtSkillYear.Text != string.Empty)
                                        {
                                            SkillDetailsObj.ESD_PK = Convert.ToInt32(hdfPk.Value);
                                            SkillDetailsObj.ESD_SKILL_CATEGORY = Convert.ToInt32(hdfSkillCategoryPk.Value);
                                            SkillDetailsObj.ESD_SKILL_FLAG = 1;
                                            SkillDetailsObj.ESD_SKILL = hdfSkillPk.Value;
                                            SkillDetailsObj.ESD_SKILL_TEXT = HttpUtility.HtmlEncode(lblSkillArea.Text);
                                            SkillDetailsObj.ESD_EXP_YEAR = txtSkillYear.Text == string.Empty ? "NULL" : txtSkillYear.Text;
                                            SkillDetailsObj.ESD_EXPERT_LEVEL = Convert.ToInt32(ddlSkillLevel.SelectedItem.Value);
                                            SkillDetailsObj.ESD_RATING = HttpUtility.HtmlEncode(txtSkilRating.Text);
                                            SkillDetailsObj.ESD_REMARKS = HttpUtility.HtmlEncode(txtSkilRemarks.Text);
                                            detailsList.Add(SkillDetailsObj);
                                        }
                                    }
                                }
                            }
                            EmployeeSkillsDetailsObj.listEmployeeSkillsList = detailsList;
                            retObject = EmployeeSkillsDetailsObj;
                        }
                        break;
                    #endregion;
                }
                return retObject;
            }
            catch
            {
                throw;
            }
            finally { }
        }
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Get - Employee Details
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        if (objEmployeeBasicInfo != null)
                        {
                            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            CurrPK = objEmployeeBasicInfo.empPK;
                            UCempBasicHdr.objEmployeeBasicInfo = objEmployeeBasicInfo;
                            UCempBasicHdr.SetFieldValues();
                            //lblhdrEmployeeNoTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode), 20);
                            //lblhdrEmployeeNameTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText), 20);
                            //if (objEmployeeBasicInfo.empDOJText == null)
                            //{

                            //}
                            //else
                            //{
                            //    string dojText = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJText).Trim(); ;
                            //    if (!dojText.IsNullOrEmptyOrWhitespace() && dojText[dojText.Length - 2] == ' ')
                            //    {
                            //        dojText = dojText.Remove(dojText.Length - 2, 1);
                            //    }
                            //    lblhdrDOJText.Text = dojText;
                            //}


                            //if (objEmployeeBasicInfo.empDOBText == null)
                            //{

                            //}
                            //else
                            //{
                            //    string dobText = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOBText).Trim();
                            //    if (!dobText.IsNullOrEmptyOrWhitespace() && dobText[dobText.Length - 2] == ' ')
                            //    {
                            //        dobText = dobText.Remove(dobText.Length - 2, 1);
                            //    }
                            //    lblhdrDOBTxt.Text = dobText;
                            //}
                            //lblhdrDesignationTxt.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText);
                            //lblhdrDepartmentTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText), 20);

                            //lblhdrEmployeeNoTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode);
                            //lblhdrEmployeeNameTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText);
                            //lblhdrDOJText.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJText);
                            //lblhdrDOBTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOBText);
                            //lblhdrDesignationTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText);
                            //lblhdrDepartmentTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText);
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
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Bind Skill Level
                case ControlsEnum.SKILLLEVEL:
                    if (dtSkillLevel != null)
                    {
                        ddlSkillLevel.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSkillLevel, "CFG_DATA");
                        ddlSkillLevel.DataTextField = "CFG_DATA";
                        ddlSkillLevel.DataValueField = "CFG_VALUE";
                        ddlSkillLevel.DataBind();
                    }
                    break;
                #endregion
            }
        }
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Bind Skill Category
                    case ControlsEnum.SKILLCATEGORY:
                        grdSkillCategoryList.DataSource = null;
                        if (dsSkillCategory.Tables.Count > 0)
                        {
                            if (dsSkillCategory.Tables[0].Rows.Count > 0)
                                grdSkillCategoryList.DataSource = dsSkillCategory.Tables[0];
                            grdSkillCategoryList.DataBind();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ExpandSelected();", true);
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
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            { }
        }

        private void ButtonClicked()
        {
            
            if (!string.IsNullOrEmpty(Request.Form[btnCancel.UniqueID]))
            {
                Session["CancelSkillClicked"] = 1;
            }
        }
        /// <summary>
        /// Validates checked skills and years of experience from inner grid
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            bool retVal = true;
            foreach (GridViewRow inRowCat in grdSkillCategoryList.Rows)
            {
                HiddenField hdfSkillCategoryId = (HiddenField)inRowCat.FindControl("hdfSkillCategoryId");
                GridView grdSkillDetails = inRowCat.FindControl("grdSkillDetails") as GridView;
                foreach (GridViewRow inRowSkill in grdSkillDetails.Rows)
                {
                    CheckBox chkSkill = (CheckBox)inRowSkill.FindControl("chkSkill");
                    TextBox txtSkillYear = (TextBox)inRowSkill.FindControl("txtSkillYear");
                    if (chkSkill.Checked == true && txtSkillYear.Text == string.Empty)
                    {
                        retVal = false;
                    }
                }
            }
            return retVal;
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
            try
            {
                int? result;
                GridViewRow gvr;
                GridView grd;
                string arg;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    Session["CheckSkill"] = 1;
                    if (((CheckBox)sender).ID == "chkShowAllSkill")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.CHECKEDCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    //if (((DropDownList)sender).ID == "ddlCompany"){commonActions = ActionsEnum.SHOW;}
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    //if (((TextBox)sender).ID == "txtReverseNow"){commonActions = ActionsEnum.CHECKAMT;}
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    //if (((RadioButton)sender).ID == "rbtSelect"){commonActions = ActionsEnum.ITEMSELECTED;}
                }
                switch (commonActions)
                {
                    #region Save
                    case BusinessObject.AccountManagement.ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (!ValidateForm())
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_YrsOfExp_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            EmployeeSkillsDetailsObj = new EmployeeSkillsDetails();
                            EmployeeSkillsDetailsObj = (EmployeeSkillsDetails)SetUIValuesToObject(ControlsEnum.EMPLOYEESKILLDETAILS);
                            if (EmployeeSkillsDetailsObj != null && EmployeeSkillsDetailsObj.listEmployeeSkillsList != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<EmployeeSkillsDetails>(EmployeeSkillsDetailsObj);
                                result = Convert.ToInt32(EmployeeSkillsBL.SaveEmployeeSkills(xmlDoc));
                                if (result > 0) // Success !  redirect to listing page
                                {                                    
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeSkills);
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ExpandSelected();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "','" + GetLocalResourceObject("RedirectUrl").ToString() + "');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ExpandSelected();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                             
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Save and continue
                    case BusinessObject.AccountManagement.ActionsEnum.SAVEANDCONTINUE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (!ValidateForm())
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_YrsOfExp_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            EmployeeSkillsDetailsObj = new EmployeeSkillsDetails();
                            EmployeeSkillsDetailsObj = (EmployeeSkillsDetails)SetUIValuesToObject(ControlsEnum.EMPLOYEESKILLDETAILS);
                            if (EmployeeSkillsDetailsObj != null && EmployeeSkillsDetailsObj.listEmployeeSkillsList != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<EmployeeSkillsDetails>(EmployeeSkillsDetailsObj);
                                result = Convert.ToInt32(EmployeeSkillsBL.SaveEmployeeSkills(xmlDoc));
                                if (result > 0) // Success !  redirect to listing page
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeSkills);
                                    CommonBL userAuth = new CommonBL();
                                    string nextPageUrl = BusinessLogic.HRMS.Common.HRMSCommonBL.GetNextTabUrl(Convert.ToInt16(EmpTabEnum.SkillsAndExpertise), GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpTabNextURL").ToString().Split(','),
                                                         GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpInactiveTabs").ToString().Split(','), Resources.PageURL.EmployeeList.ToString());
                                    if (userAuth.IsUserHasRights(currentUser.PKUser, nextPageUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK))
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(nextPageUrl) + "');", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Extra Grid Ondemand Data Population - Skill Category
                    case BusinessObject.AccountManagement.ActionsEnum.CATEGORYLIST:
                        arg = ((Button)sender).CommandArgument;
                        gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                        if (gvr != null)
                        {
                            grd = gvr.FindControl("grdSkillDetails") as GridView;
                            if (string.IsNullOrEmpty(arg))
                            {
                                EmployeeSkillsDetailsObj = null;
                            }
                            else
                            {
                                categoryId = Convert.ToInt32(arg);
                                GetFieldValues(ControlsEnum.SKILLDETAILS);
                            }
                            grd.Visible = true;
                            grd.DataSource = null;
                            if (EmployeeSkillsDetailsObj != null)
                                grd.DataSource = EmployeeSkillsDetailsObj.listEmployeeSkillsList;
                            grd.DataBind();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ExpandSelected();", true);
                            (gvr.FindControl("hdfIsSkillCategory") as HiddenField).Value = "1";
                        }
                        break;
                    #endregion
                    #region Show All Skills (Checkbox - Post)
                    case BusinessObject.AccountManagement.ActionsEnum.CHECKEDCHANGED:
                        GetFieldValues(ControlsEnum.SKILLCATEGORY);
                        SetFieldValues(ControlsEnum.SKILLCATEGORY);
                        break;
                    #endregion
                    #region Cancel
                    case BusinessObject.AccountManagement.ActionsEnum.CANCEL:
                        CurrPK = 0;
                        Response.Redirect(Resources.PageURL.EmployeeList, true);
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


        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (((GridView)sender).ID == "grdSkillCategoryList")
                    {
                        HiddenField hdfHasChild = e.Row.FindControl("hdfHasChild") as HiddenField;
                        if (Convert.ToInt32(hdfHasChild.Value) != 0)
                        {
                            HiddenField hdfSkillCategoryId = e.Row.FindControl("hdfSkillCategoryId") as HiddenField;
                            if (string.IsNullOrEmpty(hdfSkillCategoryId.Value))
                            {
                                EmployeeSkillsDetailsObj = null;
                            }
                            else
                            {
                                categoryId = Convert.ToInt32(hdfSkillCategoryId.Value);
                                GetFieldValues(ControlsEnum.SKILLDETAILS);
                            }
                            GridView grdSkillDetails = e.Row.FindControl("grdSkillDetails") as GridView;
                            grdSkillDetails.Visible = true;
                            grdSkillDetails.DataSource = null;
                            if (EmployeeSkillsDetailsObj != null)
                                grdSkillDetails.DataSource = EmployeeSkillsDetailsObj.listEmployeeSkillsList;
                            grdSkillDetails.DataBind();
                            (e.Row.FindControl("hdfIsSkillCategory") as HiddenField).Value = "1";
                        }
                    }
                    if (((GridView)sender).ID == "grdSkillDetails")
                    {
                        ddlSkillLevel = e.Row.FindControl("ddlSkillLevel") as DropDownList;
                        if (e.Row.RowIndex == 0)
                            GetFieldValues(ControlsEnum.SKILLLEVEL);
                        SetFieldValues(ControlsEnum.SKILLLEVEL);
                        HiddenField hdfSkillLevel = e.Row.FindControl("hdfSkillLevel") as HiddenField;
                        if (Convert.ToInt32(hdfSkillLevel.Value) != 0)
                            ddlSkillLevel.SelectedIndex = ddlSkillLevel.Items.IndexOf(ddlSkillLevel.Items.FindByValue(hdfSkillLevel.Value.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
        }
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
        }
        #endregion
        #endregion
        #region Pager Methods + Init
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                if (EntryStatus == BusinessObject.Common.EntryStatus.VIEWMODE)
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);                  
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            EMPLOYEEDETAILSBYID,
            SKILLCATEGORY,
            SKILLDETAILS,
            SKILLLEVEL,
            EMPLOYEESKILLDETAILS
        }
        #endregion
    }
}