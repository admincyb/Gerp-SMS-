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
using BusinessObject.HRMS.Admin.Masters;
using BusinessLogic.CommonManagement;
using BusinessLogic.HRMS.Admin.Masters;
using System.Threading;
using System.Configuration;
using ERPSMS_v01;

namespace HRMS.Employees
{
    public partial class EmployeeBasicInfo : ERP.Store.UI.MyBasePage
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
        public int CurrViewModePK
        {
            get
            {
                return Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CurrentViewModePK]);
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.CurrentViewModePK] = value;
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
        private string FilePath
        {
            get
            {
                return this.ViewState[ViewstateStrings.FilePath] == null ? string.Empty : this.ViewState[ViewstateStrings.FilePath].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.FilePath] = value;
            }
        }
        private string ShowESIRequired
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowESIRequired] == null ? string.Empty : this.ViewState[ViewstateStrings.ShowESIRequired].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowESIRequired] = value;
            }
        }

        private string ShowAdditionalInfo
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowAdditionalInfo] == null ? string.Empty : this.ViewState[ViewstateStrings.ShowAdditionalInfo].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowAdditionalInfo] = value;
            }
        }

        private string ShowCostCenterTeamInfo
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowCostCenterTeamInfo] == null ? string.Empty : this.ViewState[ViewstateStrings.ShowCostCenterTeamInfo].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowCostCenterTeamInfo] = value;
            }
        }

        private string EmpPayDetailsSaveCheck
        {
            get
            {
                return this.ViewState[ViewstateStrings.EmpPayDetailsSaveCheck] == null ? string.Empty : this.ViewState[ViewstateStrings.EmpPayDetailsSaveCheck].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.EmpPayDetailsSaveCheck] = value;
            }
        }
        private string ShowPFRequired
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowPFRequired] == null ? string.Empty : this.ViewState[ViewstateStrings.ShowPFRequired].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowPFRequired] = value;
            }
        }
        /// <summary>
        /// File Name
        /// </summary>
        private string FileName
        {
            get
            {
                return this.ViewState[ViewstateStrings.FileName] == null ? string.Empty : this.ViewState[ViewstateStrings.FileName].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.FileName] = value;
            }
        }
        /// <summary>
        /// Select Pay details PK
        /// </summary>
        private int SelectedPK
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.SelectedPK] ?? 0);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPK] = value;
            }
        }
        private bool MultiCurrencyEnabled
        {
            get
            {
                return this.ViewState[ViewstateStrings.MultiCurrencyEnabled] == null ? false : Convert.ToBoolean((this.ViewState[ViewstateStrings.MultiCurrencyEnabled]));
            }
            set
            {
                this.ViewState[ViewstateStrings.MultiCurrencyEnabled] = value;
            }
        }

        /// <summary>
        /// To Maintain Total Lines
        /// </summary>
        private int TotalEmployees
        {
            get
            {
                return this.ViewState[ViewstateStrings.TotalEmployees] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.TotalEmployees]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalEmployees] = value;
            }
        }
        /// <summary>
        /// To maintain Line Limit
        /// </summary>
        private int EmployeesLimit
        {
            get
            {
                return this.ViewState[ViewstateStrings.EmployeeLimits] == null ? TotalEmployees + 1 : Convert.ToInt32(this.ViewState[ViewstateStrings.EmployeeLimits]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EmployeeLimits] = value;
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

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.EMP, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }
        #endregion
        #region  Variables
        EmployeeList objEmpList;
        private DataTable dtPageData;
        private DataTable dtMaritalStatus;
        private DataTable dtMaritalStatusConst;
        private DataTable dtDesignation;
        private DataTable dtCompany;
        private DataTable dtGender;
        private DataTable dtEmploymentType;
        private DataTable dtBloodGroup;
        private DataTable dtJobLevel;
        private DataTable dtJobCategory;
        private DataTable dtSkillLevel;
        private DataTable dtStatus;
        private DataTable dtemployeeHeader;
        private DataTable dtJobStream;
        private DataTable dtSalutation;
        private DataTable dtReligion;
        private DataTable dtSubReligion;
        private DataTable dtEmpRelStatus;
        private DataTable dtEmpRelCanStatus;
        private DataTable dtStatusHistory;
        private DataTable dtDepartmentHistory;
        private DataTable dtDesignationHistory;
        private DataTable dtBranchHistory;
        private DataTable dtEmpymntTypeHistory;
        private DataTable dtResult;
        private DataTable dtBankDetails;
        private DataTable dtPayrollType;
        private DataTable dtEmployeeList;
        private DataTable dtDesignationDetails;
        private int departmentPk;
        private int CostCenterPk;
        private int TeamPk;
        private int Country1Pk;
        private int Country2Pk;
        private int Nationality1Pk;
        private int Nationality2Pk;
        private int ReligionPk;
        private int SubReligionPk;
        private int ReportToPk;
        private int BranchPk;
        private int professionPk;
        private int empCurStatus;
        private int DesignPK;
        private int designationPK = 0;
        private int commonPK = 0;
        private int BankPk = 0;
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        private EmployeeBasicInfomtn objEmployeeBasicInfo;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private ADM_COMPANY_MST admCompanyMstObj;
        User currentUser;
        private DataTable pageData;
        private List<WorkingHours> _workingHoursList = null;
        bool empTypeChanged = false;
        private int dummyPk = 0;
        private EmployeePayDetailsBO selectedEmployeePayDetails;
        private EmployeeTypeBO selectedEmployeeType;
        private int PayrollTypePk = 0;
        #endregion

        #endregion

        #region Page level Events
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            ucEmpDocument.AfterApply += new EventHandler(ucEmpDocument_AfterApply);
            ucEmpDocument.AfterPostback += new EventHandler(ucEmpDocument_AfterPostback);
            #region Persist FileUpload Control Value after postback
            if (Session["FileUploadCntrl"] == null && fudImage.HasFile)
            {
                Session["FileUploadCntrl"] = fudImage;
            }
            // This condition will occur on next postbacks        
            else if (Session["FileUploadCntrl"] != null && (!fudImage.HasFile))
            {
                fudImage = (FileUpload)Session["FileUploadCntrl"];
            }
            //  when Session will have File but user want to change the file 
            // i.e. wants to upload a new file using same FileUpload control
            // so update the session to have the newly uploaded file
            else if (fudImage.HasFile)
            {
                Session["FileUploadCntrl"] = fudImage;
            }
            #endregion
            ConfigurationSettings();
            if (!IsPostBack)
            {
                ucEmpDocument.BLUploadList = null;
                ucEmpDocument.FileDetailsList = null;
                if (ddlReligion.SelectedIndex > -1)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdditionalInfo", "ShowHideAdditionalInfo('1');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdditionalInfo", "ShowHideAdditionalInfo('0');", true);
                }
                Session["FileUploadCntrl"] = null;
                //if (fudImage.HasFile == false)
                //{
                imgEmployee.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["EmployeeProPic"].ToLower() + Resources.ErpRes.NoImage.ToString();
                //anchorFile.InnerHtml = "No file selected";
                //}
                //AST_DOC_MODE.Value = GetDOCMODE();
                //AST_CODE.Value = ApplicationType.EMP;
                //txtEmpCode.Text = AST_DOC_MODE.Value;
                //anchorFile.Visible = false;

                //set of hidden fields used to format Quantity, Amount, Rate
                hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                hdfCurrencyFormat.Value = "#0.";
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                {
                    hdfCurrencyFormat.Value += "0";
                    hdfCurrencyFormatWithComma.Value += "0";
                }
                MultiCurrencyEnabled = CommonFunctions.IsMultyCurrencyEnabled();

                GetFieldValues(ControlsEnum.STATUSLIST);
                SetFieldValues(ControlsEnum.STATUSLIST);
                if (Convert.ToInt32(hdfEmpCodeAutoGen.Value) == 0)
                    txtEmpCode.Focus();
                else
                    txtFirstName.Focus();
                GetFieldValues(ControlsEnum.COUNTRY);
                SetFieldValues(ControlsEnum.COUNTRY);
                GetFieldValues(ControlsEnum.COUNTRY1);
                SetFieldValues(ControlsEnum.COUNTRY1);
                GetFieldValues(ControlsEnum.COUNTRYBIRTH);
                SetFieldValues(ControlsEnum.COUNTRYBIRTH);
                GetFieldValues(ControlsEnum.COMPANY);
                SetFieldValues(ControlsEnum.COMPANY);
                ddlCompany.SelectedIndex = 0;
                GetFieldValues(ControlsEnum.DESIGNATION);
                SetFieldValues(ControlsEnum.DESIGNATION);
                txtDesignation.Text = string.Empty;
                hdfDesignation.Value = "0";
                GetFieldValues(ControlsEnum.MARITALSTATUS);
                SetFieldValues(ControlsEnum.MARITALSTATUS);
                ddlMaritalStatus.SelectedIndex = 0;
                GetFieldValues(ControlsEnum.GENDER);
                SetFieldValues(ControlsEnum.GENDER);
                GetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                SetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                ddlEmploymentType.SelectedIndex = 0;
                GetFieldValues(ControlsEnum.BLOODGROUP);
                SetFieldValues(ControlsEnum.BLOODGROUP);
                ddlBloodGroup.SelectedIndex = 0;
                GetFieldValues(ControlsEnum.SALUTATION);
                SetFieldValues(ControlsEnum.SALUTATION);
                GetFieldValues(ControlsEnum.RELIGION);
                SetFieldValues(ControlsEnum.RELIGION);
                GetFieldValues(ControlsEnum.JOBLEVEL);
                SetFieldValues(ControlsEnum.JOBLEVEL);
                ddlJobLevel.SelectedIndex = 0;
                GetFieldValues(ControlsEnum.JOBCATEGORY);
                SetFieldValues(ControlsEnum.JOBCATEGORY);
                ddljobCategory.SelectedIndex = 0;
                GetFieldValues(ControlsEnum.JOBSTREAM);
                SetFieldValues(ControlsEnum.JOBSTREAM);
                ddlJobStream.SelectedIndex = 0;
                GetFieldValues(ControlsEnum.SKILLLEVEL);
                SetFieldValues(ControlsEnum.SKILLLEVEL);
                ddlSkillLevel.SelectedIndex = 0;

                GetFieldValues(ControlsEnum.WORKINGDAYSTYPE);
                SetFieldValues(ControlsEnum.WORKINGDAYSTYPE);
                ddlWorkingDayType.SelectedIndex = 0;

                GetFieldValues(ControlsEnum.CURRENCY);
                SetFieldValues(ControlsEnum.CURRENCY);

                GetFieldValues(ControlsEnum.SELECTEDPEYDETAILS);
                SetFieldValues(ControlsEnum.SELECTEDPEYDETAILS);

                GetFieldValues(ControlsEnum.PAYROLLTYPE);
                SetFieldValues(ControlsEnum.PAYROLLTYPE);

                // using Session
                if (this.CurrPK == 0)
                {
                    //divEmployeeHeader.Visible = false;
                    btnDelete.Visible = false;
                    GetFieldValues(ControlsEnum.EMPTOTALCOUNT);
                    SetFieldValues(ControlsEnum.EMPTOTALCOUNT);
                    SetConfigValue();
                }


                imbStatusDetailsPopup.Enabled = imbBranchDetailsPopup.Enabled = imbEmpymtTypeUpdation.Enabled = imgDeptDetailsPopup.Enabled = false;
                hdfStatusPk.Value = "0";
                ddlStatus.Enabled = false;
                if (this.CurrPK != 0)
                {
                    //CurrPK = Convert.ToInt32(Request.QueryString["EmpPK"]);
                    btnDelete.Visible = true;
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowHideAdditionalInfo('0')", true);
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    imbStatusDetailsPopup.Enabled = imbBranchDetailsPopup.Enabled = imbEmpymtTypeUpdation.Enabled = imgDeptDetailsPopup.Enabled = true;
                    hdfEnableBranchLocationAutoComplete.Value = "0";
                    hdfEnableDepartmentAutoComplete.Value = "0";
                    hdfEnableDesignationAutoComplete.Value = "0";
                    txtDesignation.Enabled = false;
                    txtBranchLocation.Enabled = false;
                    txtDepartment.Enabled = false;
                  //  ddlEmploymentType.Enabled = false;
                    if (!string.IsNullOrEmpty(txtExpiredOn.Text))
                        txtExpiredOn.Enabled = false;

                    if (!string.IsNullOrEmpty(hdfDesignation.Value))
                    {
                        DesignPK = Convert.ToInt16(hdfDesignation.Value);
                        GetFieldValues(ControlsEnum.GETJOBDESIGNATIONDETAILS);
                        SetFieldValues(ControlsEnum.GETJOBDESIGNATIONDETAILS);
                        ddljobCategory.Enabled = ddlJobLevel.Enabled = false;
                    }
                    else
                    {
                        ddljobCategory.Enabled = ddlJobLevel.Enabled = false;
                    }
                }
                //View Mode
                if (this.CurrViewModePK != 0)
                {
                    DisableControls();
                    lnkRemove.Visible = false;
                    fudImage.Visible = false;
                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                    btnSaveContinue.Visible = false;
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    btnPopUpAddBranch.Visible = false;
                    btnPopUpAdd.Visible = false;
                    this.CurrViewModePK = 0;
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowHideJobDetails('1')", true);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowHideContactInformation('1')", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdditionalInfo", "ShowHideAdditionalInfo('0');", true);
                }
                //Using QueryString
                //if (Request.QueryString["EmpPK"] == null)
                //{
                //    divEmployeeHeader.Visible = false;
                //}

                //if (Request.QueryString["EmpPK"] != null)
                //{
                //    CurrPK = Convert.ToInt32(Request.QueryString["EmpPK"]);
                //    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                //    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                //    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                //    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                //}
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "OTAvailableChange", "$(document).ready(function(){OTAvailableChange();});", true);
                if (EntryStatus == BusinessObject.Common.EntryStatus.VIEWMODE)
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);

                if (!MultiCurrencyEnabled)
                    txtCurrency.Enabled = false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            int? result;
            int? result2;
            DateTime EmpLastModDate;
            DataTable dtErrorList = new DataTable();
            try
            {
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {

                    if (((DropDownList)sender).ID == "ddlReligion")
                    {
                        Session["SelectReligion"] = 1;
                        Session["SelectReligionDropdownChange"] = 1;

                        commonActions = BusinessObject.AccountManagement.ActionsEnum.SELECTSUBRELIGION;
                    }
                    if (((DropDownList)sender).ID == "ddlPaymentMode")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.PAYMENTCHANGED;
                    }
                    if (((DropDownList)sender).ID == "ddlEmployementType")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.WORKINGHOURS;
                    }
                    if (((DropDownList)sender).ID == "ddlMaritalStatus")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.MARITALSTATUSCHANGED;
                    }
                    if (((DropDownList)sender).ID == "ddlWorkingDayType")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.WORKDAYTYPECHANGED;
                    }
                    if (((DropDownList)sender).ID == "ddlBankName")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.BANKCHANGED;
                    }

                }
                switch (commonActions)
                {
                    #region SELECTSTATE
                    case BusinessObject.AccountManagement.ActionsEnum.SELECTSUBRELIGION:
                        GetFieldValues(ControlsEnum.SUBRELIGION);
                        SetFieldValues(ControlsEnum.SUBRELIGION);

                        //if (Session["SelectReligionDropdownChange"] != null)
                        //{
                        //    string path;

                        //    if (fudImage.HasFile)
                        //    {
                        //        FileInfo tempFileInfoObj;
                        //        tempFileInfoObj = new FileInfo(fudImage.PostedFile.FileName);
                        //        string attachmentFileFormat = tempFileInfoObj.Extension;
                        //        fudImage.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()) + fudImage.FileName);
                        //        path = fudImage.FileName;
                        //        hdfImage.Value = path;
                        //        FileName = path;
                        //    }
                        //    if (fudImage.HasFile == true && hdfImage.Value != string.Empty)
                        //    {
                        //        hdfImage.Value = fudImage.FileName;
                        //        fudImage.Style.Add("color", "#ffffff");
                        //        anchorFile.InnerHtml = CommonFunctions.GetShortString(hdfImage.Value, 20);
                        //        anchorFile.Title = hdfImage.Value;

                        //        imgEmployee.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + hdfImage.Value;
                        //    }
                        //}
                        //else
                        //{
                        //    imgEmployee.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["EmployeeProPic"].ToLower() + "/NoImage.jpg";

                        //}
                        Session["SelectReligionDropdownChange"] = null;

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showHide9", "ShowHideAdditionalInfo('1')", true);
                        break;

                    #endregion
                    #region Save
                    case BusinessObject.AccountManagement.ActionsEnum.SAVE:
                        hdfIsSaveYesNo.Value = CommonConstants.SELECT_VALUE_ZERO;
                        if (CurrPK == 0 && TotalEmployees >= EmployeesLimit)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Employee_Limit_Exeed;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            return;
                        }
                        DateTime dteconfirmendon = Convert.ToDateTime(string.IsNullOrEmpty(txtConfirmedOn.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtConfirmedOn.Text.Trim());
                        DateTime dateJoining = Convert.ToDateTime(string.IsNullOrEmpty(txtDOJ.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtDOJ.Text.Trim());
                        int cmpDate = dteconfirmendon.CompareTo(dateJoining);
                        if (Convert.ToInt32(hdfMaritalStatus.Value) == (int)MaritalStatus.Married)
                        {
                            if (string.IsNullOrEmpty(TxtSpouseName.Text))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("EnterSpouseName").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdditionalInfo", "ShowHideAdditionalInfo(1);", true);
                                return;
                            }
                        }

                        // Against Bud ID-14835; OT-Eligible Uncheck save issue --Sruthy on 24-8-2016
                        //if (!IsValid)
                        //{
                        //    litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        //}
                        //else 

                        if (cmpDate < 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Error_Doj").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                       + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            string EmpCode = string.Empty;
                            objEmployeeBasicInfo = SetUIValuesToObject();
                            string xmlDoc = CommonFunctions.XmlSerialize<EmployeeBasicInfomtn>(objEmployeeBasicInfo);
                            result = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.SaveEmployeeBasicInfo(xmlDoc, out EmpCode, out dtErrorList);
                            if (result > 0) // Success !  redirect to listing page
                            {
                                ucEmpDocument.SaveAttachments();
                                selectedEmployeePayDetails = (EmployeePayDetailsBO)SetPaymentValuesToObject(BusinessObject.AccountManagement.ActionsEnum.SAVE);
                                //// To save 
                                ///
                                if (Convert.ToInt16(EmpPayDetailsSaveCheck) == 0)
                                {
                                    result2 = result;
                                }
                                else
                                {
                                    result2 = EmployeePayDetailsBL.Save(selectedEmployeePayDetails, out EmpLastModDate);
                                    LastModifiedTime = EmpLastModDate;
                                }
                              
                                if (result2 > 0)
                                {
                                    txtEmpCode.Text = EmpCode;
                                    object[] args = new object[2];
                                    args[0] = Resources.PageNameRes.EmployeeBasicInformation;
                                    args[1] = txtEmpCode.Text.Trim();
                                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                    if (Convert.ToInt32(hdfEmpCodeAutoGen.Value) == 1)
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);
                                }
                                else
                                {
                                    if (result2 == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result2 == (int)DbSaveStatus.INCORRECT)  // Leave Exist
                                    {
                                        // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){ShowConfirmMsgLeaveExist();});", true);
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_LeaveExist").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);

                                    }
                                }
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    //litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = "Employee Code Already Exists";
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.INVNOEXISTS)//Biometric id already exist
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_BiometricidExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.AMOUNTEXCEEDED)// Mandatory document not added
                                {
                                    string strError = string.Empty;
                                    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dtErrorList.Rows)
                                        {
                                            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["CFG_DATA"]));
                                        }
                                        ucEmpDocument.clearControls();
                                        ucEmpDocument.DefaultDocType = Convert.ToInt32(dtErrorList.Rows[0]["CFG_VALUE"]);
                                    }
                                    hdfDocConfrmMsg.Value = GetLocalResourceObject("Err_EmpDoc_Missing").ToString() + strError;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){ShowEmpDocConfirmation();});", true);
                                }
                                else if (result == (int)DbSaveStatus.REFNOEXIST)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("MsgEmpCodeExists").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeBasicInformation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region SaveAndContinue
                    case BusinessObject.AccountManagement.ActionsEnum.SAVEANDCONTINUE:
                        hdfIsSaveYesNo.Value = CommonConstants.SELECT_VALUE_ONE;
                        if (CurrPK == 0 && TotalEmployees >= EmployeesLimit)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Employee_Limit_Exeed;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            return;
                        }
                        DateTime dteconfirmendon1 = Convert.ToDateTime(string.IsNullOrEmpty(txtConfirmedOn.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtConfirmedOn.Text.Trim());
                        DateTime dateJoining1 = Convert.ToDateTime(string.IsNullOrEmpty(txtDOJ.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort) : txtDOJ.Text.Trim());
                        int cmpDate1 = dteconfirmendon1.CompareTo(dateJoining1);
                        if (Convert.ToInt32(hdfMaritalStatus.Value) == (int)MaritalStatus.Married)
                        {
                            if (string.IsNullOrEmpty(TxtSpouseName.Text))
                            {
                                // litErrorMsg.Text = GetLocalResourceObject("EnterSpouseName").ToString();
                                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);

                                litErrorMsg.Text = (GetLocalResourceObject("EnterSpouseName")).ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdditionalInfo", "ShowHideAdditionalInfo(1);", true);
                                return;
                            }
                        }
                        // Against Bug ID-14835: OT-Eligible validation --Sruthy on 24-8-2016
                        //if (IsValid)
                        //{
                        //    litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        //}
                        //else 
                        if (cmpDate1 < 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Error_Doj").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                       + "','" + Resources.ErpRes.Information + "');", true);

                        }
                        else
                        {
                            string EmpCode = string.Empty;
                            objEmployeeBasicInfo = SetUIValuesToObject();
                            string xmlDoc = CommonFunctions.XmlSerialize<EmployeeBasicInfomtn>(objEmployeeBasicInfo);
                            result = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.SaveEmployeeBasicInfo(xmlDoc, out EmpCode, out dtErrorList);
                            if (result > 0) // Success !  redirect to listing page
                            {
                                ucEmpDocument.SaveAttachments();
                                if (objEmployeeBasicInfo != null)
                                {
                                    //currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                                    CurrPK = Convert.ToInt32(result);
                                }
                                selectedEmployeePayDetails = (EmployeePayDetailsBO)SetPaymentValuesToObject(BusinessObject.AccountManagement.ActionsEnum.SAVE);
                                //// To save 
                                ///
                                if (Convert.ToInt16(EmpPayDetailsSaveCheck) == 0)
                                {
                                    result2 = result;
                                }
                                else
                                {
                                    result2 = EmployeePayDetailsBL.Save(selectedEmployeePayDetails, out EmpLastModDate);
                                    LastModifiedTime = EmpLastModDate;
                                }
                                //result2 = EmployeePayDetailsBL.Save(selectedEmployeePayDetails, out EmpLastModDate);
                                //LastModifiedTime = EmpLastModDate;
                                if (result2 > 0)
                                {
                                    hdfIsLeaveExcYes.Value = CommonConstants.SELECT_ALL_VAL;
                                    txtEmpCode.Text = EmpCode;
                                    object[] args = new object[2];
                                    args[0] = Resources.PageNameRes.EmployeeBasicInformation;
                                    args[1] = txtEmpCode.Text.Trim();
                                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                    if (Convert.ToInt32(hdfEmpCodeAutoGen.Value) == 1)
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                    CommonBL userAuth = new CommonBL();
                                    string nextPageUrl = BusinessLogic.HRMS.Common.HRMSCommonBL.GetNextTabUrl(Convert.ToInt16(EmpTabEnum.EmpDetails), GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpTabNextURL").ToString().Split(','),
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
                                    if (result2 == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result2 == (int)DbSaveStatus.INCORRECT)  // Leave Exist
                                    {
                                        // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){ShowConfirmMsgLeaveExist();});", true);
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_LeaveExist").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                              
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    //litErrorMsg.Text = "Employee Code Already Exists";
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.INVNOEXISTS)//Biometric id already exist
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_BiometricidExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.AMOUNTEXCEEDED)// Mandatory document not added
                                {
                                    string strError = string.Empty;
                                    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                    {
                                        foreach (DataRow dr in dtErrorList.Rows)
                                        {
                                            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["CFG_DATA"]));
                                        }
                                        ucEmpDocument.clearControls();
                                        ucEmpDocument.DefaultDocType = Convert.ToInt32(dtErrorList.Rows[0]["CFG_VALUE"]);
                                    }
                                    hdfDocConfrmMsg.Value = GetLocalResourceObject("Err_EmpDoc_Missing").ToString() + strError;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){ShowEmpDocConfirmation();});", true);
                                }
                                else if (result == (int)DbSaveStatus.REFNOEXIST)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("MsgEmpCodeExists").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeBasicInformation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region CANCEL
                    case BusinessObject.AccountManagement.ActionsEnum.CANCEL:
                        ResetForm();
                        if (!string.IsNullOrEmpty(Request.Form[btnCancel.UniqueID]))
                        {
                            Session["CancelClicked"] = 1;
                        }

                        Response.Redirect(Resources.PageURL.EmployeeList, true);
                        break;
                    #endregion
                    #region uploadImg
                    case BusinessObject.AccountManagement.ActionsEnum.UploadImg:
                        try
                        {
                            GetImage();
                        }
                        catch
                        {

                        }
                        break;
                    #endregion
                    #region DELETEIMAGE
                    case BusinessObject.AccountManagement.ActionsEnum.DELETEIMAGE:
                        try
                        {
                            imgEmployee.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["EmployeeProPic"].ToLower() + Resources.ErpRes.NoImage.ToString();
                            anchorFile.InnerHtml = "";
                            hdfImage.Value = "";
                            hdfFileuRL.Value = "";
                            Session["FileUploadCntrl"] = null;
                        }
                        catch
                        {

                        }
                        break;
                    #endregion
                    #region DELETE
                    case BusinessObject.AccountManagement.ActionsEnum.DELETE:
                        if (this.CurrPK != 0)
                        {
                            string routeURL = Resources.PageURL.EmployeeList.ToString();

                            result = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.DeleteEmployee(CurrPK);

                            DbDeleteStatus deleteStatus = (DbDeleteStatus)result;
                            switch (deleteStatus)
                            {
                                case DbDeleteStatus.DELETED://If deletion is success
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeDetails);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.REFERRED://If referred to another page
                                    Session["DeleteReference"] = 1;
                                    Session["DeleteReference1"] = 1;
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeDetails);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.SQLERROR://Sql error
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeDetails);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DETAILS
                    case BusinessObject.AccountManagement.ActionsEnum.DETAILS:
                        if (((ImageButton)sender).ID == "imbStatusDetailsPopup")
                        {
                            GetFieldValues(ControlsEnum.DETAILS);
                            SetFieldValues(ControlsEnum.DETAILS);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divStatusDetails]','Status Details','700','400');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowJobDetails", "ShowHideJobDetails(1);", true);
                        }
                        else if (((ImageButton)sender).ID == "imbBranchDetailsPopup")
                        {
                            GetFieldValues(ControlsEnum.BRANCHDETAILS);
                            SetFieldValues(ControlsEnum.BRANCHDETAILS);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divBranchDetails]','Branch Details','720','400');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowJobDetails", "ShowHideJobDetails(1);", true);
                        }
                        if (((ImageButton)sender).ID == "imgDeptDetailsPopup")
                        {
                            GetFieldValues(ControlsEnum.DEPARTMENTDETAILS);
                            SetFieldValues(ControlsEnum.DEPARTMENTDETAILS);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDepartmentDetails]','Department Details','700','400');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowJobDetails", "ShowHideJobDetails(1);", true);
                        }
                        if (((ImageButton)sender).ID == "imbDesigDetailsPopup")
                        {
                            GetFieldValues(ControlsEnum.DESIGNATIONDETAILS);
                            SetFieldValues(ControlsEnum.DESIGNATIONDETAILS);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDesignationDetails]','Designation Details','700','400');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowJobDetails", "ShowHideJobDetails(1);", true);
                        }
                        break;
                    #endregion
                    #region POPUPCANCEL
                    case BusinessObject.AccountManagement.ActionsEnum.POPUPCANCEL:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowJobDetails", "ShowHideJobDetails(1);", true);
                        break;
                    #endregion
                    #region POPUPADD
                    case BusinessObject.AccountManagement.ActionsEnum.POPUPADD:
                        if (((Button)sender).ID == "btnPopUpAdd")
                        {
                            int statusPk = 0;// Always Add New Record = GetNullableInt(hdfStatusPk.Value) ?? 0;
                            int empId = CurrPK;
                            DateTime statusDate = Convert.ToDateTime(txtStatusDatePopup.Text);
                            Int16 statusId = Convert.ToInt16(ddlStatusPopUp.SelectedValue);
                            Int16 cancelStatusId = 0;
                            Int16 fromStatus = 0;
                            Int16.TryParse(ddlStatus.SelectedValue, out fromStatus);
                            Int16.TryParse(ddlStatusCancelPopUp.SelectedValue, out cancelStatusId);

                            if (fromStatus == statusId)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divStatusDetails]','Status Details','700','400');", true);
                                litErrorMsg.Text = GetLocalResourceObject("MsgStatusExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                                return;
                            }


                            result = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.SaveJobDetailStatus(statusPk, empId, statusDate, statusId, fromStatus
                                       , txtReasonPopup.Text.Trim(), Convert.ToInt16(currentUser.PKUser), currentUser.SBUID, cancelStatusId);
                            if (result > 0) // Success 
                            {
                                hdfStatusPk.Value = result.Value.ToString();
                                ddlStatus.SelectedValue = ddlStatusPopUp.SelectedValue;
                                GetFieldValues(ControlsEnum.EMPRELSTATUS);
                                GetFieldValues(ControlsEnum.EMPRELCANCELSTATUS);
                                SetFieldValues(ControlsEnum.EMPRELCANCELSTATUS);
                                EmpStatusCheck(statusId);
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("StatusDetailsSave"));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "');", true);
                                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitComponents", "InitComponents();", true);
                            }
                            #region ERROR MESSAGES
                            else
                            {
                                if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divStatusDetails]','Status Details','700','400');", true);
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divStatusDetails]','Status Details','700','400');", true);
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.INCORRECT)  // payroll already processed in curent period
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divStatusDetails]','Status Details','700','400');", true);
                                    litErrorMsg.Text = string.Format(GetLocalResourceObject("MsgPayrollExist").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "','" + "');", true);

                                }
                                else if (result == (int)DbSaveStatus.REFNOEXIST)  // Date exist in history
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divStatusDetails]','Status Details','700','400');", true);
                                    litErrorMsg.Text = GetLocalResourceObject("MsgDateExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "','" + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divStatusDetails]','Status Details','700','400');", true);
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeBasicInformation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            #endregion
                        }
                        else if (((Button)sender).ID == "btnPopUpAddBranch")
                        {
                            int branchPk = 0;// Always Add New Record = GetNullableInt(hdfStatusPk.Value) ?? 0;
                            int empId = CurrPK;
                            int branchId;
                            result = null;
                            if (hdfBranchLocationPopup.Value == string.Empty)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divBranchDetails]','Branch Details','720','400');", true);
                                litErrorMsg.Text = GetLocalResourceObject("SelectBranch").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                return;
                            }
                            if (txtDatePopupBranch.Text == string.Empty)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divBranchDetails]','Branch Details','720','400');", true);
                                litErrorMsg.Text = GetLocalResourceObject("SelectDate").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                return;
                            }
                            DateTime branchDate = Convert.ToDateTime(txtDatePopupBranch.Text);
                            branchId = Convert.ToInt32(hdfBranchLocationPopup.Value);
                            int fromBranch = 0;
                            int.TryParse(hdfBranchLocation.Value, out fromBranch);
                            result = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.SaveJobDetailBranch(branchPk, empId, branchDate, branchId, fromBranch
                                        , txtReasonPopupBranch.Text, Convert.ToInt16(currentUser.PKUser), currentUser.SBUID);
                            if (result > 0) // Success 
                            {
                                hdfBranchPk.Value = result.Value.ToString();
                                txtBranchLocation.Text = txtBranchLocationPopup.Text;// ddlStatus.SelectedValue = ddlStatusPopUp.SelectedValue;
                                hdfBranchLocation.Value = hdfBranchLocationPopup.Value;
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("BranchDetailsSave"));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "');", true);
                                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitComponents", "InitComponents();", true);
                            }
                            #region Error Messages
                            else
                            {
                                if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divBranchDetails]','Branch Details','720','400');", true);
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divBranchDetails]','Branch Details','720','400');", true);
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divBranchDetails]','Branch Details','720','400');", true);
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeBasicInformation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            #endregion
                        }
                        else if (((Button)sender).ID == "btnDeptPopupAdd")
                        {
                            int deptPk = 0;// Always Add New Record = GetNullableInt(hdfStatusPk.Value) ?? 0;
                            int empId = CurrPK;
                            DateTime statusDate = Convert.ToDateTime(txtDatePopup.Text);
                            Int16 FromDeptID = Convert.ToInt16(hdfDepartment.Value);
                            Int16 ToDeptID = Convert.ToInt16(hdfDepartmentPopup.Value);

                            if (hdfDepartment.Value == hdfDepartmentPopup.Value)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDepartmentDetails]','Department Details','700','400');", true);
                                litErrorMsg.Text = GetLocalResourceObject("MsgDepartmentExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                                return;
                            }

                            result = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.SaveJobDetailDepartment(deptPk, empId, statusDate, FromDeptID, ToDeptID, txtRsnPopup.Text.Trim(), Convert.ToInt16(currentUser.PKUser), currentUser.SBUID);

                            if (result > 0) // Success 
                            {
                                hdfDepartment.Value = hdfDepartmentPopup.Value.ToString();
                                txtDepartment.Text = txtDepartmentPopup.Text;

                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("DeprtmentDetailsSave"));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDepartmentDetails]','Department Details','700','400');", true);
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDepartmentDetails]','Department Details','700','400');", true);
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                //else if (result == (int)DbSaveStatus.INCORRECT)  // Department already exist
                                //{
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDepartmentDetails]','Department Details','700','400');", true);
                                //    litErrorMsg.Text = GetLocalResourceObject("MsgDepartmentExist").ToString();
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                //    + "','" + Resources.Captions.Information + "','" + "');", true);

                                //}
                                else if (result == (int)DbSaveStatus.REFNOEXIST)  // Date exist in history
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDepartmentDetails]','Department Details','700','400');", true);
                                    litErrorMsg.Text = GetLocalResourceObject("MsgDateExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "','" + "');", true);

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDepartmentDetails]','Department Details','700','400');", true);
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeBasicInformation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        else if (((Button)sender).ID == "btnDesignPopupAdd")
                        {
                            int desigPk = 0;// Always Add New Record = GetNullableInt(hdfStatusPk.Value) ?? 0;
                            int empId = CurrPK;
                            DateTime statusDate = Convert.ToDateTime(txtDDatePopup.Text);
                            Int16 FromDesignID = Convert.ToInt16(hdfDesignation.Value);
                            Int16 ToDesigID = Convert.ToInt16(hdfDesignationPopup.Value);

                            if (hdfDesignation.Value == hdfDesignationPopup.Value)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDesignationDetails]','Designation Details','700','400');", true);
                                litErrorMsg.Text = GetLocalResourceObject("MsgDesignationExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                                return;
                            }

                            result = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.SaveJobDetailDesignation(desigPk, empId, statusDate, FromDesignID, ToDesigID, txtDRsnPopup.Text.Trim(), Convert.ToInt16(currentUser.PKUser), currentUser.SBUID);

                            if (result > 0) // Success 
                            {
                                hdfDesignation.Value = hdfDesignationPopup.Value.ToString();
                                txtDesignation.Text = txtDesignationPopup.Text;
                                DesignPK = Convert.ToInt16(hdfDesignation.Value);
                                GetFieldValues(ControlsEnum.GETJOBDESIGNATIONDETAILS);
                                SetFieldValues(ControlsEnum.GETJOBDESIGNATIONDETAILS);

                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("DesignationDetailsSave"));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDesignationDetails]','Designation Details','700','400');", true);
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDesignationDetails]','Designation Details','700','400');", true);
                                    litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.REFNOEXIST)  // Date exist in history
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDesignationDetails]','Designation Details','700','400');", true);
                                    litErrorMsg.Text = GetLocalResourceObject("MsgDateExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "','" + "');", true);

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divDesignationDetails]','Designation Details','700','400');", true);
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeBasicInformation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowJobDetails", "ShowHideJobDetails(1);", true);
                        break;
                    #endregion
                    #region PAYMENT CHANGED
                    case BusinessObject.AccountManagement.ActionsEnum.PAYMENTCHANGED:
                        // DisableBankDetails(ddlPaymentMode.SelectedValue == "4");
                        // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPayDetails", "ShowHidePayDetails(1);", true);
                        break;
                    #endregion
                    #region WORKING HOURS
                    case BusinessObject.AccountManagement.ActionsEnum.WORKINGHOURS:
                        empTypeChanged = true;
                        GetFieldValues(ControlsEnum.WORKINGDAYS);
                        SetFieldValues(ControlsEnum.WORKINGDAYS);
                        GetFieldValues(ControlsEnum.SELECTEDEMPTYPE);
                        SetFieldValues(ControlsEnum.SELECTEDEMPTYPE);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowEmpTypeDetails", "ShowHideEmpType(1);", true);
                        break;
                    #endregion
                    #region MARITAL STATUS CHANGED
                    case BusinessObject.AccountManagement.ActionsEnum.MARITALSTATUSCHANGED:
                        hdfMaritalStatus.Value = "0";
                        GetFieldValues(ControlsEnum.MARITALSTATUSCONST);
                        if (dtMaritalStatusConst != null && dtMaritalStatusConst.Rows.Count > 0)
                        {
                            hdfMaritalStatus.Value = dtMaritalStatusConst.Rows[0]["CON_VALUE"].ToString();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdditionalInfo", "ShowHideAdditionalInfo(1);", true);
                        break;
                    #endregion
                    #region WORK DAY TYPE CHANGED
                    case BusinessObject.AccountManagement.ActionsEnum.WORKDAYTYPECHANGED:
                        if (Convert.ToInt32(ddlWorkingDayType.SelectedValue) == (int)WorkingDayType.Fixed)
                        {
                            txtWorkingDays.Enabled = true;
                            txtWorkingDays.CssClass = "input-small numeric";
                            vrfWorkingDays.Enabled = true;
                        }
                        else
                        {
                            txtWorkingDays.Enabled = false;
                            txtWorkingDays.CssClass = "input-small numeric input-disabled";
                            vrfWorkingDays.Enabled = false;
                            txtWorkingDays.Text = string.Empty;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowEmpTypeDetails1", "ShowHideEmpType(1);", true);

                        //var validator = document.getElementById("<%= vrfWorkingDays.ClientID %>");
                        //var WorkingDayType = $("[id*=ddlWorkingDayType]").val();
                        //if (WorkingDayType == 1) {                  
                        //    //$("[id*=divWorkingDay]").show();
                        //    $("[id*=txtWorkingDays]").attr('disabled', false);
                        //    $("[id*=txtWorkingDays]").removeClass("input-disabled");
                        //    ValidatorEnable(validator, true);
                        //}
                        //else {              
                        //    //$("[id*=divWorkingDay]").hide();
                        //    $("[id*=txtWorkingDays]").attr('disabled', true);
                        //    $("[id*=txtWorkingDays]").addClass("input-disabled");
                        //    $("[id*=txtWorkingDays]").val('');
                        //    ValidatorEnable(validator, false);
                        //}          
                        break;
                    #endregion
                    #region BANK CHANGED
                    case BusinessObject.AccountManagement.ActionsEnum.BANKCHANGED:
                        BankPk = Convert.ToInt32(ddlBankName.SelectedValue);
                        GetFieldValues(ControlsEnum.BANKDETAILS);
                        SetFieldValues(ControlsEnum.BANKDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPayDetails", "ShowHidePayDetails(1);", true);
                        break;
                    #endregion
                    #region EMPYMNT TYPE UPDN
                    case BusinessObject.AccountManagement.ActionsEnum.EMPYMNTTYPEUPDN:
                        GetFieldValues(ControlsEnum.EMPYMNTTYPEUPDN);
                        SetFieldValues(ControlsEnum.EMPYMNTTYPEUPDN);
                        ShowEmploymentTypePopup();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowJobDetails", "ShowHideJobDetails(1);", true);
                        break;
                    #endregion
                    #region EMPYMNT TYPE SAVE
                    case BusinessObject.AccountManagement.ActionsEnum.EMPYMNTTYPESAVE:
                        ERP.Utilities.HRMS.FilterParameters objParametrs = new ERP.Utilities.HRMS.FilterParameters();
                        objParametrs.PK = 0;
                        objParametrs.Employee = CurrPK;
                        objParametrs.Date = Convert.ToDateTime(txtEmpymntTypeDatePopup.Text);
                        objParametrs.EmploymentType = Convert.ToInt16(ddlEmpymntTypePopup.SelectedValue);
                        objParametrs.UserPK = currentUser.PKUser;
                        objParametrs.BizUnit = currentUser.SBUID;
                        int IsContract = 0;
                        int MailBeforeDays = Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "MailBeforeDaysContract").ToString());
                        if (ddlEmpymntTypePopup.SelectedValue == GetGlobalResourceObject("ConfigurationsRes", "EmpymntTypeContractPk").ToString())
                        {
                            objParametrs.ToDate = Convert.ToDateTime(txtEmpymntTypeDatePopup.Text);
                            IsContract = 1;
                        }
                        Int16 fromType = 0;
                        Int16.TryParse(ddlEmploymentType.SelectedValue, out fromType);
                        result = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.SaveEmploymentTypeHistory(objParametrs, fromType, HttpUtility.HtmlEncode(txtReasonPopupEmpymntType.Text.Trim()), IsContract, MailBeforeDays);
                        if (result > 0) // Success 
                        {
                            ddlEmploymentType.SelectedValue = ddlEmpymntTypePopup.SelectedValue;
                            if (ddlEmpymntTypePopup.SelectedValue == GetGlobalResourceObject("ConfigurationsRes", "EmpymntTypeContractPk").ToString())
                                txtExpiredOn.Text = txtEmpymntTypeDatePopup.Text;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("EmpymntTypeDetails"));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                               + "','" + Resources.ErpRes.Information + "');", true);
                            // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitComponents", "InitComponents();", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                                ShowEmploymentTypePopup();
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeBasicInformation + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                ShowEmploymentTypePopup();
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeBasicInformation);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                ShowEmploymentTypePopup();
                            }
                        }


                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowJobDetails", "ShowHideJobDetails(1);", true);
                        break;
                    #endregion
                    #region EMPDOCS
                    case BusinessObject.AccountManagement.ActionsEnum.EMPDOCS:
                        ucEmpDocument.EmployeeDOB = txtDOB.Text;
                        ShowEmpDocPopup();
                        break;
                    #endregion
                    #region DESIGNATION CHANGE
                    case BusinessObject.AccountManagement.ActionsEnum.CHANGEDESIGNATION:
                        DesignPK = Convert.ToInt16(hdfDesignation.Value);
                        GetFieldValues(ControlsEnum.GETJOBDESIGNATIONDETAILS);
                        SetFieldValues(ControlsEnum.GETJOBDESIGNATIONDETAILS);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }



        private void ShowEmploymentTypePopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divEmpymTypeDetails]','" + GetLocalResourceObject("EmpymntTypeDetails").ToString() + "','750','400');", true);

        }
        #endregion
        #region Get Field Values
        private void GetFieldValues(ControlsEnum controlType)
        {
            AdmCompanyMstService admCompanyMstServiceClient;
            ServiceUtility serviceUtilityObj;

            try
            {
                switch (controlType)
                {
                    case ControlsEnum.DESIGNATION:
                        dtDesignation = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDesignation(designationPK);
                        break;
                    case ControlsEnum.MARITALSTATUS:
                        dtMaritalStatus = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetMaritalStatus(commonPK);
                        break;
                    case ControlsEnum.GENDER:
                        dtGender = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeGender(commonPK);
                        break;
                    case ControlsEnum.EMPLOYMENTTYPE:
                        dtEmploymentType = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmploymentType(commonPK);
                        break;
                    case ControlsEnum.BLOODGROUP:
                        dtBloodGroup = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetBloodgroup(commonPK);
                        break;
                    case ControlsEnum.SALUTATION:
                        dtSalutation = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetSalutation(commonPK);
                        break;
                    case ControlsEnum.RELIGION:
                        dtReligion = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetdropdownFillReligion(commonPK);
                        break;
                    case ControlsEnum.SUBRELIGION:
                        dtSubReligion = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetdropDownSubReligion(commonPK, Convert.ToInt32(ddlReligion.SelectedValue));
                        break;
                    case ControlsEnum.JOBLEVEL:
                        dtJobLevel = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.Getjoblevel(commonPK);
                        break;
                    case ControlsEnum.JOBCATEGORY:
                        dtJobCategory = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetjobCategory(commonPK);
                        break;
                    case ControlsEnum.JOBSTREAM:
                        dtJobStream = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.Getjobstream(commonPK);
                        break;
                    case ControlsEnum.SKILLLEVEL:
                        dtSkillLevel = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetSkilLevel(commonPK);
                        break;
                    case ControlsEnum.STATUSLIST:
                        dtStatus = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeStatus(GTIService.Constants.HRMS.Employee.Constatnts.EMP_EMPLOYEE_STATUS_TYPE);
                        break;
                    #region Company
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetailsPlantWise(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion
                    //Gets the Employee detail by Id
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        objEmployeeBasicInfo = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeebyID(CurrPK, currentUser.PKUser);
                        break;
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        dtemployeeHeader = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDetailListHeader(CurrPK, string.Empty);
                        break;
                    case ControlsEnum.DETAILS:
                        //dtStatusDetails = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeStatusDetails(GetNullableInt(hdfStatusPk.Value) ?? 0, currentUser.SBUID, 1);
                        dtStatusHistory = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeStatusHistory(CurrPK);
                        break;
                    case ControlsEnum.BRANCHDETAILS:
                        dtBranchHistory = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetJobDetailsBranchHistory(CurrPK);
                        break;
                    case ControlsEnum.DEPARTMENTDETAILS:
                        dtDepartmentHistory = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDepartmentHistory(CurrPK);
                        break;
                    case ControlsEnum.DESIGNATIONDETAILS:
                        dtDesignationHistory = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDesignationHistory(CurrPK);
                        break;
                    case ControlsEnum.PAYMENTMODE:
                        pageData = EmployeePayDetailsBL.GetPaymentModeSpl(currentUser.SBUID);
                        break;
                    case ControlsEnum.BANK:
                        int bankType = 2;
                        pageData = EmployeePayDetailsBL.GetBankNames(currentUser.SBUID, bankType, null, BankPk);
                        break;
                    //case ControlsEnum.SALARYTEMPLATE:
                    //    if (this.SelectedPK > 0)
                    //        dummyPk = this.selectedEmployeePayDetails.EPD_SALARY_TEMP;
                    //    pageData = EmployeePayDetailsBL.GetSalaryTemplate(currentUser.SBUID, dummyPk, (int)DbActiveStatus.ACTIVE);
                    //    break;
                    case ControlsEnum.OTTEMPLATE:
                        if (this.SelectedPK > 0)
                            dummyPk = !string.IsNullOrEmpty(selectedEmployeePayDetails.EPD_OT_TEMP) ? Convert.ToInt32(selectedEmployeePayDetails.EPD_OT_TEMP) : 0;
                        pageData = EmployeePayDetailsBL.GetOTTemplateList(currentUser.SBUID, dummyPk, (int)DbActiveStatus.ACTIVE);
                        break;
                    case ControlsEnum.LEAVETEMPLATE:
                        if (this.SelectedPK > 0)
                            dummyPk = !string.IsNullOrEmpty(selectedEmployeePayDetails.EPD_LEAVE_TEMP) ? Convert.ToInt32(selectedEmployeePayDetails.EPD_LEAVE_TEMP) : 0;
                        pageData = EmployeePayDetailsBL.GetLeaveTemplateList(currentUser.SBUID, dummyPk, (int)DbActiveStatus.ACTIVE);
                        break;
                    case ControlsEnum.EMPLOYEEMENTTYPE:
                        if (this.SelectedPK > 0)
                            dummyPk = this.selectedEmployeePayDetails.EPD_EMP_TYPE;
                        pageData = EmployeePayDetailsBL.GetEmployeeTypeGetKV(dummyPk, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                        break;
                    case ControlsEnum.SELECTEDPEYDETAILS:
                        this.selectedEmployeePayDetails = EmployeePayDetailsBL.GetEmployeePayDetailsByID(this.CurrPK, (int)DbActiveStatus.ACTIVE);
                        break;
                    case ControlsEnum.WORKINGDAYS:
                        GetFieldValues(ControlsEnum.WEEKDAYS);
                        SetFieldValues(ControlsEnum.WEEKDAYS);
                        //_workingHoursList = new List<WorkingHours>();
                        //_workingHoursList.Add(new WorkingHours { WeekDay = 1, Hours = 0 });
                        //_workingHoursList.Add(new WorkingHours { WeekDay = 2, Hours = 0 });
                        //_workingHoursList.Add(new WorkingHours { WeekDay = 3, Hours = 0 });
                        //_workingHoursList.Add(new WorkingHours { WeekDay = 4, Hours = 0 });
                        //_workingHoursList.Add(new WorkingHours { WeekDay = 5, Hours = 0 });
                        //_workingHoursList.Add(new WorkingHours { WeekDay = 6, Hours = 0 });
                        //_workingHoursList.Add(new WorkingHours { WeekDay = 7, Hours = 0 });

                        if (empTypeChanged)
                        {
                            int empTypePk = Convert.ToInt32(ddlEmployementType.SelectedValue);
                            pageData = EmployeePayDetailsBL.GetWorkingDays(empTypePk);
                            foreach (DataRow row in pageData.Rows)
                            {
                                WorkingHours wh = _workingHoursList.Find(x => x.WeekDay == Convert.ToInt32(row["ESH_WEEK_DAY"]));
                                wh.Pk = Convert.ToInt32(row["ESH_PK"]);
                                wh.Hours = Convert.ToDouble(row["ESH_HOURS"]);
                                wh.EmployeeType = Convert.ToInt32(row["ESH_EMP_TYPE"]);
                                wh.Active = Convert.ToInt32(row["ESH_ACTIVE"]);
                            }
                        }
                        else if (this.SelectedPK > 0)
                        {
                            if (selectedEmployeePayDetails != null)
                            {
                                foreach (var item in selectedEmployeePayDetails.WorkingHours)
                                {
                                    WorkingHours wh = _workingHoursList.Find(x => x.WeekDay == item.WeekDay);
                                    wh.Pk = item.Pk;
                                    wh.Hours = item.Hours;
                                    wh.EmployeeType = item.EmployeeType;
                                    wh.Active = item.Active;
                                }
                            }
                        }
                        break;

                    #region WEEKDAYS
                    case ControlsEnum.WEEKDAYS:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "Week Days", "WD");
                        break;
                    #endregion

                    case ControlsEnum.MARITALSTATUSCONST:
                        int constPK = Convert.ToInt32(ddlMaritalStatus.SelectedValue);
                        dtMaritalStatusConst = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetMaritalStatusConst(constPK);
                        break;
                    #region WORKING DAYS TYPE
                    case ControlsEnum.WORKINGDAYSTYPE:
                        pageData = CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("ConfigType_WorkingDaysType").ToString());
                        break;
                    #endregion
                    #region Selected Employee Type
                    case ControlsEnum.SELECTEDEMPTYPE:
                        selectedEmployeeType = EmployeeTypeBL.GetEmployeeTypeByID(Convert.ToInt32(ddlEmployementType.SelectedValue), (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion

                    case ControlsEnum.BANKDETAILS:
                        dtBankDetails = BusinessLogic.HRMS.Common.HRMSCommonBL.GetCashOrBank(currentUser.SBUID, 2, null, BankPk, (int)DbActiveStatus.HASPK);
                        break;

                    case ControlsEnum.PAYROLLTYPE:
                        // dtPayrollType = EmployeePayDetailsBL.GetPayrollType(PayrollTypePk, (int)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        dtPayrollType = BusinessLogic.HRMS.Payroll.PayrollProcessBL.GetPayrollType(PayrollTypePk, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE));
                        break;

                    case ControlsEnum.CURRENCY:
                        dtResult = CommonBL.GetCurrency(currentUser.BaseCurrency, currentUser.SBUID, (int)DbActiveStatus.HASPK);
                        break;
                    case ControlsEnum.EMPYMNTTYPEUPDN:
                        dtEmpymntTypeHistory = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmploymentTypeHistory(CurrPK);
                        break;

                    case ControlsEnum.EMPTOTALCOUNT:
                        this.dtEmployeeList = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmpListByFilterOptions(new ERP.Utilities.HRMS.EmpDocumentFilterParameterBinder
                        {
                            PageIndex = 1,
                            PageSize = 1,
                            EmpCategory = (int)EmployeeCategory.HRMSEmployee,
                        });
                        break;

                    #region EMP REL STATUS
                    case ControlsEnum.EMPRELSTATUS:
                        dtEmpRelStatus = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("EMPRELSTATUSTYPE").ToString(), GetLocalResourceObject("EMPRELSTATUSCOND").ToString());
                        break;
                    #endregion

                    #region EMP REL CANCEL STATUS
                    case ControlsEnum.EMPRELCANCELSTATUS:
                        dtEmpRelCanStatus = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("EMPCANCELSTATUS").ToString(), ddlStatus.SelectedValue);
                        break;
                    #endregion

                    #region Get Job Category and Grade
                    case ControlsEnum.GETJOBDESIGNATIONDETAILS:
                        dtDesignationDetails = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmpDesignationDtl(DesignPK, (int)DbActiveStatus.HASPK);
                        break;
                    #endregion
                }
            }
            catch
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

                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.DESIGNATION:
                        BindDropDown(ControlsEnum.DESIGNATION);
                        break;
                    case ControlsEnum.MARITALSTATUS:
                        BindDropDown(ControlsEnum.MARITALSTATUS);
                        break;
                    case ControlsEnum.GENDER:
                        BindDropDown(ControlsEnum.GENDER);
                        break;
                    case ControlsEnum.EMPLOYMENTTYPE:
                        BindDropDown(ControlsEnum.EMPLOYMENTTYPE);
                        break;
                    case ControlsEnum.BLOODGROUP:
                        BindDropDown(ControlsEnum.BLOODGROUP);
                        break;
                    case ControlsEnum.SALUTATION:
                        BindDropDown(ControlsEnum.SALUTATION);
                        break;
                    case ControlsEnum.RELIGION:
                        BindDropDown(ControlsEnum.RELIGION);
                        break;
                    case ControlsEnum.SUBRELIGION:
                        BindDropDown(ControlsEnum.SUBRELIGION);
                        break;
                    case ControlsEnum.JOBCATEGORY:
                        BindDropDown(ControlsEnum.JOBCATEGORY);
                        break;
                    case ControlsEnum.JOBLEVEL:
                        BindDropDown(ControlsEnum.JOBLEVEL);
                        break;
                    case ControlsEnum.SKILLLEVEL:
                        BindDropDown(ControlsEnum.SKILLLEVEL);
                        break;
                    case ControlsEnum.JOBSTREAM:
                        BindDropDown(ControlsEnum.JOBSTREAM);
                        break;
                    case ControlsEnum.STATUSLIST:
                        BindDropDown(ControlsEnum.STATUSLIST);
                        break;
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        GetUIValuesFromObjectHeader(controlType);
                        break;
                    case ControlsEnum.GETJOBDESIGNATIONDETAILS:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.DETAILS:
                        // GetUIValuesFromObject(controlType);
                        ClearPopupControls();
                        BindGrid(ControlsEnum.DETAILS);
                        break;
                    case ControlsEnum.DEPARTMENTDETAILS:
                        ClearDepartmentPopupControls();
                        BindGrid(ControlsEnum.DEPARTMENTDETAILS);
                        break;
                    case ControlsEnum.DESIGNATIONDETAILS:
                        ClearDesignationPopupControls();
                        BindGrid(ControlsEnum.DESIGNATIONDETAILS);
                        break;
                    case ControlsEnum.BRANCHDETAILS:
                        // GetUIValuesFromObject(controlType);
                        ClearBranchPopupControls();
                        BindGrid(ControlsEnum.BRANCHDETAILS);
                        break;
                    case ControlsEnum.PAYMENTMODE:
                        BindDropDown(ControlsEnum.PAYMENTMODE);
                        break;
                    case ControlsEnum.BANK:
                        BindDropDown(ControlsEnum.BANK);
                        break;
                    //case ControlsEnum.SALARYTEMPLATE:
                    //    BindDropDown(ControlsEnum.SALARYTEMPLATE);
                    //    break;
                    case ControlsEnum.LEAVETEMPLATE:
                        BindDropDown(ControlsEnum.LEAVETEMPLATE);
                        break;
                    case ControlsEnum.OTTEMPLATE:
                        BindDropDown(ControlsEnum.OTTEMPLATE);
                        break;
                    case ControlsEnum.EMPLOYEEMENTTYPE:
                        BindDropDown(ControlsEnum.EMPLOYEEMENTTYPE);
                        break;
                    #region Working Days
                    case ControlsEnum.WORKINGDAYS:
                        BindGrid(ControlsEnum.WORKINGDAYS);
                        break;
                    #endregion
                    #region WEEKDAYS
                    case ControlsEnum.WEEKDAYS:
                        if (dtResult == null || dtResult.Rows.Count < 1)
                        {
                            _workingHoursList = null;
                        }
                        else
                        {
                            List<WorkingHours> weekDaysList = new List<WorkingHours>();
                            for (int i = 0; i < dtResult.Rows.Count; i++)
                            {
                                WorkingHours weekDay = new WorkingHours();
                                weekDay.WeekDay = Convert.ToInt32(dtResult.Rows[i]["CFG_VALUE"]);
                                weekDay.WeekDayTest = Convert.ToString(dtResult.Rows[i]["CFG_DATA"]);
                                weekDaysList.Add(weekDay);
                            }
                            _workingHoursList = weekDaysList;
                        }
                        break;
                    #endregion
                    case ControlsEnum.SELECTEDPEYDETAILS:
                        if (this.selectedEmployeePayDetails != null)
                        {
                            GetUIValuesFromObject(ControlsEnum.SELECTEDPEYDETAILS);
                        }
                        else
                        {
                            ResetForm(BusinessObject.AccountManagement.ActionsEnum.NEW);
                        }
                        break;
                    #region WORKING DAYS TYPE
                    case ControlsEnum.WORKINGDAYSTYPE:
                        BindDropDown(ControlsEnum.WORKINGDAYSTYPE);
                        break;
                    #endregion
                    #region SELECTED EMP TYPE
                    case ControlsEnum.SELECTEDEMPTYPE:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    case ControlsEnum.BANKDETAILS:
                        GetUIValuesFromObject(controlType);
                        break;

                    case ControlsEnum.PAYROLLTYPE:
                        BindDropDown(ControlsEnum.PAYROLLTYPE);
                        break;
                    case ControlsEnum.CURRENCY:
                        GetUIValuesFromObject(ControlsEnum.CURRENCY);
                        break;
                    case ControlsEnum.EMPYMNTTYPEUPDN:
                        ClearEmpymntTyePopupControls();
                        BindGrid(ControlsEnum.EMPYMNTTYPEUPDN);
                        break;
                    case ControlsEnum.EMPTOTALCOUNT:
                        GetUIValuesFromObject(ControlsEnum.EMPTOTALCOUNT);
                        break;
                    case ControlsEnum.EMPRELCANCELSTATUS:
                        BindDropDown(ControlsEnum.EMPRELCANCELSTATUS);
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
        #endregion
        #region Document Usercontrol Functions
        private void ShowEmpDocPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop5", "ShowContainerDiv('[id$=divEmpDoc]','" + GetLocalResourceObject("EmpDocTitle").ToString() + "','91%','550');", true);
        }
        protected void ucEmpDocument_AfterApply(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop2", "ClosePopup();", true);
        }
        protected void ucEmpDocument_AfterPostback(object sender, EventArgs e)
        {
            ShowEmpDocPopup();
        }
        #endregion
        #region Helper Methods

        private void ConfigurationSettings()
        {
            ShowESIRequired = GetGlobalResourceObject("ConfigurationsRes", "HrmsChkESIRequired").ToString();
            ShowPFRequired = GetGlobalResourceObject("ConfigurationsRes", "HrmsChkPFRequired").ToString();
            hdfEmpCodeAutoGen.Value = GetGlobalResourceObject("ConfigurationsRes", "hrmsEmpCodeAutoGen").ToString();
            hdfSSORequired.Value = GetGlobalResourceObject("ConfigurationsRes", "hrmsEmpSSORequired").ToString();
            ShowAdditionalInfo = GetGlobalResourceObject("ConfigurationsRes", "HrmsInfoHiding").ToString();
            ShowCostCenterTeamInfo = GetGlobalResourceObject("ConfigurationsRes", "HrmsCostCenterTeamVisile").ToString();
            EmpPayDetailsSaveCheck = GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpPayDtlsCheck").ToString();
            if (Convert.ToInt16(ShowESIRequired) == 0)
            {
                lblESIReq.Visible = false;
                chkESIReqrd.Visible = false;
            }
            else
            {
                lblESIReq.Visible = true;
                chkESIReqrd.Visible = true;
            }

            if (Convert.ToInt16(ShowCostCenterTeamInfo) == 0)
            {
                divTeam.Visible = true;
                divCostCenter.Visible = true;
            }
            else
            {
                divTeam.Visible = false;
                divCostCenter.Visible = false;

            }
            if (Convert.ToInt16(ShowAdditionalInfo) == 0)
            {

                divAdditionalInfo.Visible = false;
                divEmpTypeDtls.Visible = false;
                divImageSection.Visible = false;
                divContactDetails.Visible = false;
              //  divPayDtls.Visible = false;
                divPersonalEmail.Visible = false;
                divReportTo.Visible = false;
                divReportAdm.Visible = false;
                divJobStream.Visible = false;
                divPaymentMode.Visible = false;
                divBankName.Visible = false;
            }
            else
            {
                divAdditionalInfo.Visible = true;
                divEmpTypeDtls.Visible = true;
                divImageSection.Visible = true;
                divContactDetails.Visible = true;
               // divPayDtls.Visible = true;
                divPersonalEmail.Visible = true;
                divReportTo.Visible = true;
                divReportAdm.Visible = true;
                divJobStream.Visible = true;
                divPaymentMode.Visible = true;
                divBankName.Visible = true;
            }

            if (Convert.ToInt16(ShowPFRequired) == 0)
            {
                lblPFReq.Visible = false;
                chkPFRequrd.Visible = false;
            }
            else
            {
                lblPFReq.Visible = true;
                chkPFRequrd.Visible = true;
            }
            if (Convert.ToInt16(hdfSSORequired.Value) == 0)
            {
                lblSSOReq.Visible = false;
                chkSSOReq.Visible = false;
            }
            else
            {
                lblSSOReq.Visible = true;
                chkSSOReq.Visible = true;
            }
            if (Convert.ToInt32(hdfEmpCodeAutoGen.Value) == 1)
            {
                txtEmpCode.Enabled = false;
                txtEmpCode.CssClass += " input-disabled";
                vrfEmpCode.EnableClientScript = false;
            }

           
        }

        protected void ResetForm(BusinessObject.AccountManagement.ActionsEnum action)
        {
            switch (action)
            {
                case BusinessObject.AccountManagement.ActionsEnum.SAVE:
                    GetFieldValues(ControlsEnum.SELECTEDPEYDETAILS);
                    SetFieldValues(ControlsEnum.SELECTEDPEYDETAILS);
                    break;
                case BusinessObject.AccountManagement.ActionsEnum.NEW:
                    clearControls();
                    GetFieldValues(ControlsEnum.PAYMENTMODE);
                    SetFieldValues(ControlsEnum.PAYMENTMODE);
                    //GetFieldValues(ControlsEnum.SALARYTEMPLATE);
                    //SetFieldValues(ControlsEnum.SALARYTEMPLATE);
                    GetFieldValues(ControlsEnum.BANK);
                    SetFieldValues(ControlsEnum.BANK);
                    GetFieldValues(ControlsEnum.EMPLOYEEMENTTYPE);
                    SetFieldValues(ControlsEnum.EMPLOYEEMENTTYPE);
                    GetFieldValues(ControlsEnum.WORKINGDAYS);
                    SetFieldValues(ControlsEnum.WORKINGDAYS);
                    GetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    SetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    GetFieldValues(ControlsEnum.OTTEMPLATE);
                    SetFieldValues(ControlsEnum.OTTEMPLATE);
                    break;
            }
        }

        private void clearControls()
        {
            this.SelectedPK = 0;
            //lblLastModifiedHDR.Visible = false;
        }

        //private void DisableBankDetails(bool canEnable)
        //{
        //    //Labels
        //    lblBankName.Enabled = canEnable;
        //    lblAccountCode.Enabled = canEnable;
        //    lblAccountName.Enabled = canEnable;
        //    lblIFSCCode.Enabled = canEnable;
        //    //lblPFAccount.Enabled = canEnable;
        //    //lblPFEffectiveDate.Enabled = canEnable;
        //    //lblSOCSOAccount.Enabled = canEnable;
        //    //lblSOCSOEffectiveDate.Enabled = canEnable;
        //    lblBranch.Enabled = canEnable;

        //    //Input
        //    ddlBankName.Enabled = canEnable;
        //    txtAccountCode.Enabled = canEnable;
        //    txtAccountName.Enabled = canEnable;
        //    txtIFSCCode.Enabled = canEnable;
        //    //txtPFAccount.Enabled = canEnable;
        //    //txtPFEffectiveDate.Enabled = canEnable;
        //    //txtSOCSOAccount.Enabled = canEnable;
        //    //txtSOCSOEffectiveDate.Enabled = canEnable;
        //    txtBranch.Enabled = canEnable;

        //    //Validators
        //    rfvBankName.Enabled = canEnable;
        //    //rfvAccountCode.Enabled = canEnable;
        //    //rfvAccountName.Enabled = canEnable;
        //    //rfvBranch.Enabled = canEnable;
        //    //rfvIFSCCode.Enabled = canEnable;
        //}

        private object SetPaymentValuesToObject(BusinessObject.AccountManagement.ActionsEnum mode)
        {
            object returnObj = null;
            //returnObj = null;

            //DateTime dummyDateField;
            //int dummyIntField;
            double dummyDouble;
            try
            {
                switch (mode)
                {
                    case BusinessObject.AccountManagement.ActionsEnum.SAVE:
                        EmployeePayDetailsBO obj = new EmployeePayDetailsBO();
                        obj.EPD_EMPLOYEE = this.CurrPK;
                        obj.EPD_PK = this.SelectedPK;

                        //if (Double.TryParse(txtBasic.Text.Trim(), out dummyDouble))
                        //    obj.EPD_BASIC_PAY = dummyDouble;
                        //else
                        //    throw new InvalidCastException("invalid basic pay");

                        //obj.EPD_PAN_NO = txtPANNo.Text.Trim();
                        //obj.EPD_SALARY_TEMP = Convert.ToInt32(ddlSalaryTemplate.SelectedValue);
                        obj.EPD_PAY_MODE = Convert.ToInt32(ddlPaymentMode.SelectedValue);

                        if (ddlPaymentMode.SelectedValue == "4") // Bank
                        {
                            obj.EPD_PAY_BANK = Convert.ToInt32(ddlBankName.SelectedValue) > 0 ? ddlBankName.SelectedValue : string.Empty;
                            obj.EPD_PAY_BANK_BRANCH = txtBranch.Text.Trim().HtmlEncode();
                            obj.EPD_BANK_AC_NO = txtAccountCode.Text.Trim().HtmlEncode();
                            obj.EPD_BANK_AC_NAME = txtAccountName.Text.Trim().HtmlEncode();
                            obj.EPD_BANK_IFSC = txtIFSCCode.Text.Trim().HtmlEncode();
                            //obj.EPD_PF_AC = txtPFAccount.Text.Trim().HtmlEncode();
                            //obj.EPD_SOCSO_AC = txtSOCSOAccount.Text.Trim().HtmlEncode();
                            //obj.EPD_PF_DATE = txtPFEffectiveDate.Text.Trim().IsNullOrEmptyOrWhitespace() ? string.Empty : txtPFEffectiveDate.Text.Trim();
                            //obj.EPD_SOCSO_DATE = txtSOCSOEffectiveDate.Text.Trim().IsNullOrEmptyOrWhitespace() ? string.Empty
                            //    : txtSOCSOEffectiveDate.Text.Trim();
                        }

                        obj.EPD_EMP_TYPE = Convert.ToInt32(ddlEmployementType.SelectedValue);
                        obj.EPD_LEAVE_TEMP = Convert.ToInt32(ddlLeaveTemplate.SelectedValue) > 0 ? ddlLeaveTemplate.SelectedValue : null;
                        obj.EPD_OT_TEMP = Convert.ToInt32(ddlOTTemplate.SelectedValue) > 0 ? ddlOTTemplate.SelectedValue : null;
                        obj.EPD_OT_AVAILABE = chkOTAvailable.Checked ? "1" : "0";
                        obj.EPD_CONSIDER_LATE_HRS = chkConsiderLateHrs.Checked ? "1" : "0";
                        obj.EDP_ESI_REQUIRED = chkESIReqrd.Checked ? "1" : "0";
                        obj.EDP_PF_REQUIRED = chkPFRequrd.Checked ? "1" : "0";
                        obj.EDP_SSO_REQUIRED = chkSSOReq.Checked ? "1" : "0";
                        obj.EPD_WORKING_DAY_TYPE = Convert.ToInt32(ddlWorkingDayType.SelectedValue) > 0 ? ddlWorkingDayType.SelectedValue : null;
                        if (Convert.ToInt16(ddlWorkingDayType.SelectedValue) == 1 && !string.IsNullOrEmpty(txtWorkingDays.Text.Trim()))
                            obj.EPD_WORKING_DAY = txtWorkingDays.Text.Trim();
                        else
                            obj.EPD_WORKING_DAY = null;

                        int workinghrs = 0;
                        decimal otrate = 0;

                        int.TryParse(txtNormalWorkingHrs.Text, out workinghrs);
                        decimal.TryParse(txtOTRate.Text, out otrate);
                        obj.EPD_WRK_HRS = workinghrs > 0 ? workinghrs.ToString() : null;
                        obj.EPD_OT_RATE = otrate > 0 ? otrate.ToString() : null;

                        obj.EPD_BREAK_TIME = txtBreakTime.Text.Trim().HtmlEncode();
                        obj.EPD_HAS_OT_FROM_ATT = chkOTPending.Checked ? "1" : "0";

                        //Set Working Days Here
                        obj.WorkingHours = (List<WorkingHours>)SetPaymentValuesToObject(BusinessObject.AccountManagement.ActionsEnum.DETAIL);

                        obj.EPD_CURRENCY = int.Parse(hdfCurrency.Value);
                        obj.BIZUNIT_PK = currentUser.SBUID;
                        obj.ACTIVE = (int)DbActiveStatus.ACTIVE;
                        obj.LastModifiedDate = this.LastModifiedTime;
                        obj.USER_PK = currentUser.PKUser;
                        obj.LEAVE_DEL = Convert.ToInt16(hdfIsLeaveExcYes.Value);


                        returnObj = obj;
                        break;
                    case BusinessObject.AccountManagement.ActionsEnum.DETAIL:
                        List<WorkingHours> workingHrsList = null;

                        if (grdWorkingDays.Rows.Count > 0)
                        {
                            workingHrsList = new List<WorkingHours>();
                            foreach (GridViewRow row in grdWorkingDays.Rows)
                            {
                                double hrs = 0;
                                if (Double.TryParse(((TextBox)row.FindControl("txtHrs")).Text.Trim(), out dummyDouble))
                                    hrs = dummyDouble;
                                if (hrs > 0)
                                {
                                    int pk = 0;
                                    Int32.TryParse(((HiddenField)row.FindControl("hdfWHrsPK")).Value.Trim(), out pk);
                                    int weekDay = Convert.ToInt32(((HiddenField)row.FindControl("hdfWeekDay")).Value.Trim());
                                    int empID = Convert.ToInt32(((HiddenField)row.FindControl("hdfWHrsEmpTypeID")).Value.Trim());

                                    WorkingHours workingHrs = new WorkingHours
                                    {
                                        Pk = pk,
                                        WeekDay = weekDay,
                                        EmployeeType = empID,
                                        Hours = hrs,
                                        Active = (int)DbActiveStatus.ACTIVE
                                    };
                                    workingHrsList.Add(workingHrs);
                                }
                            }
                        }
                        returnObj = workingHrsList;
                        break;

                }
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                returnObj = null;
            }
        }

        private EmployeeBasicInfomtn SetUIValuesToObject()
        {
            string Image = hdfImage.Value;
            //currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //Basic Info
            objEmployeeBasicInfo = new EmployeeBasicInfomtn();
            objEmployeeBasicInfo.empPK = CurrPK;
            objEmployeeBasicInfo.empCode = HttpUtility.HtmlEncode(txtEmpCode.Text.Trim());
            objEmployeeBasicInfo.empSalutation = Convert.ToInt16(ddlSalutaion.SelectedValue);
            objEmployeeBasicInfo.empName = HttpUtility.HtmlEncode(txtFirstName.Text.Trim());
            objEmployeeBasicInfo.empName2 = HttpUtility.HtmlEncode(txtMiddleName.Text.Trim());
            objEmployeeBasicInfo.empName3 = HttpUtility.HtmlEncode(txtLastName.Text.Trim());
            objEmployeeBasicInfo.empName4 = HttpUtility.HtmlEncode(txtSurname.Text.Trim());
            objEmployeeBasicInfo.empName_LL = HttpUtility.HtmlEncode(txtEmpNameLL.Text.Trim());
            objEmployeeBasicInfo.empMobile1 = HttpUtility.HtmlEncode(txtMobile.Text.Trim());
            //objEmployeeBasicInfo.empDOB = string.IsNullOrEmpty(txtDOB.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtDOB.Text.Trim();
            objEmployeeBasicInfo.empDOB = HttpUtility.HtmlEncode(txtDOB.Text.Trim());
            objEmployeeBasicInfo.empGender = Convert.ToInt16(ddlGender.SelectedValue);
            objEmployeeBasicInfo.empMaritalStatus = Convert.ToInt16(ddlMaritalStatus.SelectedValue == (-1).ToString() ? Convert.ToInt16(null) : Convert.ToInt16(ddlMaritalStatus.SelectedValue));
            objEmployeeBasicInfo.empEmail1 = HttpUtility.HtmlEncode(txtPersonalEmail.Text.Trim());
            //Job Details
            objEmployeeBasicInfo.empDepartment = Convert.ToInt16(hdfDepartment.Value == string.Empty ? 0 : Convert.ToInt16(hdfDepartment.Value));
            objEmployeeBasicInfo.EmpCostcenterId = Convert.ToInt16(hdfCostCenter.Value == string.Empty ? 0 : Convert.ToInt16(hdfCostCenter.Value));
            objEmployeeBasicInfo.EmpTeamId = Convert.ToInt16(hdfTeam.Value == string.Empty ? 0 : Convert.ToInt16(hdfTeam.Value));
            objEmployeeBasicInfo.empDesignation = Convert.ToInt16(hdfDesignation.Value);           
            objEmployeeBasicInfo.empType = 1;//Defauli 2
            objEmployeeBasicInfo.empEmploymentType = Convert.ToInt16(ddlEmploymentType.SelectedValue);
            objEmployeeBasicInfo.empCompany = Convert.ToInt16(ddlCompany.SelectedValue);
            objEmployeeBasicInfo.empJobLevel = Convert.ToInt16(ddlJobLevel.SelectedValue == (-1).ToString() ? Convert.ToInt16(null) : Convert.ToInt16(ddlJobLevel.SelectedValue));
            objEmployeeBasicInfo.empJobCategory = Convert.ToInt16(ddljobCategory.SelectedValue == (-1).ToString() ? Convert.ToInt16(null) : Convert.ToInt16(ddljobCategory.SelectedValue));
            objEmployeeBasicInfo.empJobStream = Convert.ToInt16(ddlJobStream.SelectedValue == (-1).ToString() ? Convert.ToInt16(null) : Convert.ToInt16(ddlJobStream.SelectedValue));
            objEmployeeBasicInfo.empSkillLevel = Convert.ToInt16(ddlSkillLevel.SelectedValue == (-1).ToString() ? Convert.ToInt16(null) : Convert.ToInt16(ddlSkillLevel.SelectedValue));
            objEmployeeBasicInfo.empBranch = Convert.ToInt16(hdfBranchLocation.Value == string.Empty ? 0 : Convert.ToInt16(hdfBranchLocation.Value));
            objEmployeeBasicInfo.empDOJ = HttpUtility.HtmlEncode(txtDOJ.Text.Trim());
            objEmployeeBasicInfo.empConfirmedOn = HttpUtility.HtmlEncode(txtConfirmedOn.Text.Trim());//string.IsNullOrEmpty(txtConfirmedOn.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtConfirmedOn.Text.Trim());
            objEmployeeBasicInfo.empReportTo = Convert.ToInt32(hdfReportTo.Value == string.Empty ? 0 : Convert.ToInt32(hdfReportTo.Value));
            objEmployeeBasicInfo.empReportTo1 = Convert.ToInt32(hdfReportToAdm.Value) <= 0 ? (int?)null : Convert.ToInt32(hdfReportToAdm.Value);
            objEmployeeBasicInfo.empPhone1 = HttpUtility.HtmlEncode(txtOfficialPhone.Text.Trim());
            objEmployeeBasicInfo.empPhoneExt1 = HttpUtility.HtmlEncode(txtOffPhoExtension.Text.Trim());
            objEmployeeBasicInfo.empMobile2 = HttpUtility.HtmlEncode(txtOfficialMobile.Text.Trim());
            objEmployeeBasicInfo.empEmail2 = HttpUtility.HtmlEncode(txtOfficialEmail.Text.Trim());
            objEmployeeBasicInfo.empCurStatus = Convert.ToInt16(ddlStatus.SelectedValue);
            objEmployeeBasicInfo.empActive = Convert.ToInt16(chkActive.Checked);
            //Contact Details
            objEmployeeBasicInfo.empAddress1 = HttpUtility.HtmlEncode(txtPermanentAddress.Text.Trim());
            objEmployeeBasicInfo.empAddress2 = HttpUtility.HtmlEncode(txtCommunicationAddress.Text.Trim());
            objEmployeeBasicInfo.empStatePK1 = Convert.ToInt32(hdfState.Value) > 0 ? hdfState.Value : null;
            objEmployeeBasicInfo.empState1 = HttpUtility.HtmlEncode(txtState.Text.Trim());
            objEmployeeBasicInfo.empStatePK2 = Convert.ToInt32(hdfState1.Value) > 0 ? hdfState1.Value : null;
            objEmployeeBasicInfo.empState2 = HttpUtility.HtmlEncode(txtState1.Text.Trim());
            objEmployeeBasicInfo.empDistrict1 = HttpUtility.HtmlEncode(txtDistrict1.Text.Trim());
            objEmployeeBasicInfo.empDistrict2 = HttpUtility.HtmlEncode(txtDistrict2.Text.Trim());
            objEmployeeBasicInfo.empCity1 = HttpUtility.HtmlEncode(txtCity.Text.Trim());
            objEmployeeBasicInfo.empCity2 = HttpUtility.HtmlEncode(txtCity1.Text.Trim());
            objEmployeeBasicInfo.empPlaceOfBirth = HttpUtility.HtmlEncode(TxtPlaceOfBirth.Text.Trim());
            objEmployeeBasicInfo.empZip1 = HttpUtility.HtmlEncode(txtZipCode.Text.Trim());
            objEmployeeBasicInfo.empZip2 = HttpUtility.HtmlEncode(txtZipCode1.Text.Trim());
            objEmployeeBasicInfo.empPhone2 = HttpUtility.HtmlEncode(txtPhone.Text.Trim());
            objEmployeeBasicInfo.empPhone3 = HttpUtility.HtmlEncode(txtPhone1.Text.Trim());
            objEmployeeBasicInfo.empCountry1 = Convert.ToInt16(hdfCountry.Value == string.Empty ? 0 : Convert.ToInt16(hdfCountry.Value));
            objEmployeeBasicInfo.empCategory = (int)EmployeeCategory.HRMSEmployee;
            objEmployeeBasicInfo.EMPCODE_AUTO = Convert.ToInt32(hdfEmpCodeAutoGen.Value);
            if (chkSameAsAbove.Checked == true && hdfCountry.Value != null)
            {
                objEmployeeBasicInfo.empCountry2 = Convert.ToInt16(hdfCountry.Value == string.Empty ? 0 : Convert.ToInt16(hdfCountry.Value));
            }
            else
            {
                if (txtCountry1.Text == "")
                {
                    hdfCountry1.Value = string.Empty;
                    objEmployeeBasicInfo.empCountry2 = Convert.ToInt16(hdfCountry1.Value == string.Empty ? 0 : Convert.ToInt16(hdfCountry1.Value));
                }
                else
                {
                    objEmployeeBasicInfo.empCountry2 = Convert.ToInt16(hdfCountry1.Value == string.Empty ? 0 : Convert.ToInt16(hdfCountry1.Value));
                }

            }
            if (txtNationality.Text == "")
            {
                hdfNationalityPK.Value = null;
                objEmployeeBasicInfo.empNationality1 = Convert.ToInt16(hdfNationalityPK.Value == string.Empty ? 0 : Convert.ToInt16(hdfNationalityPK.Value));

            }
            objEmployeeBasicInfo.empNationality1 = Convert.ToInt16(hdfNationalityPK.Value == string.Empty ? 0 : Convert.ToInt16(hdfNationalityPK.Value));
            if (txtCountryOfBirth.Text == "")
            {
                hdfCountryOfBirth.Value = null;
                objEmployeeBasicInfo.empNationality2 = Convert.ToInt16(hdfCountryOfBirth.Value == string.Empty ? 0 : Convert.ToInt16(hdfCountryOfBirth.Value));
            }
            objEmployeeBasicInfo.empNationality2 = Convert.ToInt16(hdfCountryOfBirth.Value == string.Empty ? 0 : Convert.ToInt16(hdfCountryOfBirth.Value));

            //Commented For autoSearch Religion and subreligion
            //objEmployeeBasicInfo.empReligion = Convert.ToInt16(hdfReligion.Value == string.Empty ? 0 : Convert.ToInt16(hdfReligion.Value));
            //objEmployeeBasicInfo.empSubReligion = Convert.ToInt16(hdfSubReligion.Value == string.Empty ? 0 : Convert.ToInt16(hdfSubReligion.Value));

            objEmployeeBasicInfo.empReligion = Convert.ToInt16(ddlReligion.SelectedValue == (-1).ToString() ? 0 : Convert.ToInt16(ddlReligion.SelectedValue));
            objEmployeeBasicInfo.empSubReligion = Convert.ToInt16(ddlSubReligion.SelectedValue == string.Empty ? 0 : Convert.ToInt16(ddlSubReligion.SelectedValue));
            //Additional Info
            objEmployeeBasicInfo.empFatherName = HttpUtility.HtmlEncode(TxtFatherName.Text.Trim());
            objEmployeeBasicInfo.empMotherName = HttpUtility.HtmlEncode(txtMotherName.Text.Trim());
            objEmployeeBasicInfo.empSpouseName = HttpUtility.HtmlEncode(TxtSpouseName.Text.Trim());

            objEmployeeBasicInfo.empSpouseTaxNo = HttpUtility.HtmlEncode(txtIncomeTaxNo.Text.Trim());
            //objEmployeeBasicInfo.empSpouseSurname = HttpUtility.HtmlEncode(txtSpouseSurname.Text.Trim());
            objEmployeeBasicInfo.empSpouseIsWorking = Convert.ToInt16(chkIsSpouseWorking.Checked);

            objEmployeeBasicInfo.empBloodGroup = Convert.ToInt16(ddlBloodGroup.SelectedValue == (-1).ToString() ? Convert.ToInt16(null) : Convert.ToInt16(ddlBloodGroup.SelectedValue));
            objEmployeeBasicInfo.empNoOfChildren = HttpUtility.HtmlEncode(txtNoOfchildren.Text.Trim());
            objEmployeeBasicInfo.empProfession = Convert.ToInt16(hdfProfession.Value == string.Empty ? 0 : Convert.ToInt16(hdfProfession.Value));
            objEmployeeBasicInfo.empPhone4 = HttpUtility.HtmlEncode(txtAltContactNO.Text.Trim());
            objEmployeeBasicInfo.empDept = Convert.ToInt16(currentUser.CurrentDeptPK);
            objEmployeeBasicInfo.BIZUNIT_PK = currentUser.SBUID;

            //objEmployeeBasicInfo.empActive = Convert.ToInt16(DbActiveStatus.ACTIVE);
            objEmployeeBasicInfo.USER_PK = Convert.ToInt16(currentUser.PKUser);
            objEmployeeBasicInfo.LAST_MOD_DT = LastModifiedTime;// hdflastModifiedDate.Value;

            objEmployeeBasicInfo.AST_CODE = ApplicationType.EMP;
            objEmployeeBasicInfo.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;

            //Payroll and Basic Pay
            objEmployeeBasicInfo.empPayRollType = Convert.ToInt16(ddlPayrollType.SelectedValue == (-1).ToString() ? Convert.ToInt16(null) : Convert.ToInt16(ddlPayrollType.SelectedValue));
            objEmployeeBasicInfo.empBasicPay = Convert.ToDouble(txtBasicSalary.Text == string.Empty ? null : txtBasicSalary.Text.Trim());

            //Additional info 14-11-2016
            objEmployeeBasicInfo.empSpouseBranch = HttpUtility.HtmlEncode(txtSpouseSurname.Text.Trim());
            objEmployeeBasicInfo.empFatherNameId = HttpUtility.HtmlEncode(txtFIdNumber.Text.Trim());
            objEmployeeBasicInfo.empMotherNameId = HttpUtility.HtmlEncode(txtMIdNumber.Text.Trim());
            objEmployeeBasicInfo.empSpouseNameId = HttpUtility.HtmlEncode(txtSIdNumber.Text.Trim());
            objEmployeeBasicInfo.empFatherofSpouse = HttpUtility.HtmlEncode(txtFatherSpouse.Text.Trim());
            objEmployeeBasicInfo.empFatherofSpouseId = HttpUtility.HtmlEncode(txtFatherSpouseIdNo.Text.Trim());
            objEmployeeBasicInfo.empMotherofSpouse = HttpUtility.HtmlEncode(txtMothrSpouse.Text.Trim());
            objEmployeeBasicInfo.empMotherofSpouseId = HttpUtility.HtmlEncode(txtMothrSpouseIdNo.Text.Trim());
            objEmployeeBasicInfo.empNoOfChildrenId = HttpUtility.HtmlEncode(txtNoOfChildrenId.Text.Trim());
            objEmployeeBasicInfo.empTaxPayer = Convert.ToInt16(chkTaxpayer.Checked);
            objEmployeeBasicInfo.empNoOfChildEdu = HttpUtility.HtmlEncode(txtNoOfchildrenEdu.Text.Trim());
            objEmployeeBasicInfo.empNoOfChildEduId = HttpUtility.HtmlEncode(txtNoOfChildEduId.Text.Trim());
            objEmployeeBasicInfo.empDueDate = string.IsNullOrEmpty(txtExpiredOn.Text.Trim()) ? null : txtExpiredOn.Text;
            if (ddlEmploymentType.SelectedValue == GetGlobalResourceObject("ConfigurationsRes", "EmpymntTypeContractPk").ToString())
            {
                objEmployeeBasicInfo.IS_CONTRACT = 1;
                objEmployeeBasicInfo.MAIL_BEFORE = Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "MailBeforeDaysContract").ToString());
            }
            else
            {
                objEmployeeBasicInfo.IS_CONTRACT = 0;
                objEmployeeBasicInfo.MAIL_BEFORE = 0;
            }
            if (txtConfirmedOn.Text != string.Empty)
            {
                objEmployeeBasicInfo.MAIL_BEFORE_CNFRM_ON = Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "MailBeforeDaysConfirmed").ToString());
            }
            else
            {
                objEmployeeBasicInfo.MAIL_BEFORE_CNFRM_ON = 0;
            }
            if (txtDOJ.Text != string.Empty)
            {
                objEmployeeBasicInfo.MAIL_BEFORE_DOJ = Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "MailOneYearCompletion").ToString());
            }
            else
            {
                objEmployeeBasicInfo.MAIL_BEFORE_DOJ = 1;
            }
            if (ucEmpDocument.EmpDocDetails != null)
                objEmployeeBasicInfo.EmployeeDocDetails = ucEmpDocument.EmpDocDetails;
            if (fudImage.HasFile)
            {
                if (fudImage.PostedFile.ContentType == "image/jpeg" || fudImage.PostedFile.ContentType == "image/png")
                {
                    string uploadPath = string.Empty;
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\Employee";
                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);
                        uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\Employee\\";
                    }
                    else
                    {
                        uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "Employee";
                        if (!Directory.Exists(uploadPath))
                            Directory.CreateDirectory(uploadPath);
                        uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "Employee\\";
                    }

                    //string path;
                    FileInfo tempFileInfoObj;
                    tempFileInfoObj = new FileInfo(fudImage.PostedFile.FileName);
                    string attachmentFileFormat = tempFileInfoObj.Extension;
                    FileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                    //fudImage.SaveAs(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()) + fudImage.FileName);
                    fudImage.SaveAs(uploadPath + FileName);
                    //path = fudImage.FileName;
                    //hdfImage.Value = path;
                    //FileName = path;
                    FilePath = uploadPath + FileName;
                    objEmployeeBasicInfo.empPhotoFileName = FileName;
                    objEmployeeBasicInfo.empPhotoFilePath = FilePath;
                    //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                    //{
                    //    FilePath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + FileName;
                    //}
                    //else
                    //{
                    //    FilePath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + FileName;
                    //}
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Err_EmpPhoto").ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeBasicInformation);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }

            }
            else if (!string.IsNullOrEmpty(imgEmployee.ImageUrl) && !imgEmployee.ImageUrl.Contains(Resources.ErpRes.NoImage.ToString()))
            {
                objEmployeeBasicInfo.empPhotoFileName = FileName;
                objEmployeeBasicInfo.empPhotoFilePath = FilePath;
                //Commenetd for bugid 121
                // objEmployeeBasicInfo.empPhotoFileName = FileName;
            }

            //if (fudImage.HasFile == false)
            //{
            //    if (hdfImage.Value == "")
            //    {
            //        imgEmployee.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["EmployeeProPic"].ToLower() + "NoImage.jpg";
            //    }
            //    else
            //    {

            //        FileName = hdfImage.Value;
            //        //anchorFile.Visible = true;
            //        //anchorFile.InnerHtml = FileName;
            //        objEmployeeBasicInfo.empPhotoFileName = FileName;
            //        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
            //        {
            //            FilePath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + FileName;
            //        }
            //        else
            //        {
            //            FilePath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + FileName;
            //        }
            //        imgEmployee.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + objEmployeeBasicInfo.empPhotoFileName.ToString();
            //    }
            //}
            //if (fudImage.HasFile == false && hdfImage.Value == string.Empty)
            //{

            //    imgEmployee.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["EmployeeProPic"].ToLower() + "/NoImage.jpg";
            //}


            //if (fudImage.HasFile == false && hdfImage.Value != string.Empty)
            //{
            //    hdfImage.Value = objEmployeeBasicInfo.empPhotoFileName.ToString();
            //    fudImage.Style.Add("color", "#ffffff");
            //    anchorFile.InnerHtml = CommonFunctions.GetShortString(hdfImage.Value, 20);
            //    anchorFile.Title = hdfImage.Value;
            //    imgEmployee.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + objEmployeeBasicInfo.empPhotoFileName.ToString();
            //}
            //if (fudImage.HasFile == true && hdfImage.Value != string.Empty)
            //{


            //    hdfImage.Value = objEmployeeBasicInfo.empPhotoFileName.ToString();
            //    fudImage.Style.Add("color", "#ffffff");
            //    anchorFile.InnerHtml = CommonFunctions.GetShortString(hdfImage.Value, 20);
            //    anchorFile.Title = hdfImage.Value;
            //    imgEmployee.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + objEmployeeBasicInfo.empPhotoFileName.ToString();
            //}

            //if (hdfImage.Value == string.Empty)
            //{
            //    hdfImage.Value = "";
            //}
            //else
            //{
            //    objEmployeeBasicInfo.empPhotoFilePath = FilePath;
            //}

            objEmployeeBasicInfo.empOldCode = txtOldCode.Text.Trim();
            objEmployeeBasicInfo.empBiometricId = txtBioMetricId.Text.Trim();

            return objEmployeeBasicInfo;
        }

        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EMPLOYEEDETAILSBYID
                    case ControlsEnum.EMPLOYEEDETAILSBYID:

                        if (objEmployeeBasicInfo != null)
                        {
                            //currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            hdfStatusPk.Value = objEmployeeBasicInfo.emsPk == null ? "0" : objEmployeeBasicInfo.emsPk;
                            if (txtDesignation.Text != "")
                            {
                                GetToolTips();
                            }
                            CurrPK = objEmployeeBasicInfo.empPK;
                            if (objEmployeeBasicInfo.empActive == Convert.ToInt16(1))
                            {
                                chkActive.Checked = true;
                            }
                            else
                            {
                                chkActive.Checked = false;
                            }
                            txtEmpCode.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode);
                            txtFirstName.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName);
                            txtMiddleName.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName2);
                            txtLastName.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName3);
                            txtSurname.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName4);
                            txtEmpNameLL.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName_LL);
                            txtMobile.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empMobile1);
                            ddlGender.SelectedValue = objEmployeeBasicInfo.empGender.ToString();
                            ddlSalutaion.SelectedValue = objEmployeeBasicInfo.empSalutation.ToString();
                            if (objEmployeeBasicInfo.empDOB == null)
                            {
                            }
                            else
                            {
                                txtDOB.Text = Convert.ToDateTime(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOB.ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
                            }
                            if (objEmployeeBasicInfo.empDOJ == null)
                            {
                            }
                            else
                            {
                                txtDOJ.Text = Convert.ToDateTime(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJ.ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
                            }
                            spnAge.InnerHtml = AgeInWords(objEmployeeBasicInfo.empDOBText);
                            spnDOJ.InnerHtml = DOJInWords(objEmployeeBasicInfo.empDOJText);

                            if (string.IsNullOrEmpty(objEmployeeBasicInfo.empConfirmedOn))
                            {
                                txtConfirmedOn.Text = string.Empty;
                                SpnConfirmendOn.InnerHtml = string.Empty;
                            }
                            else
                            {
                                txtConfirmedOn.Text = Convert.ToDateTime(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empConfirmedOn.ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
                                SpnConfirmendOn.InnerHtml = DOJInWords(objEmployeeBasicInfo.empConfirmedOnText);
                            }

                            if (!string.IsNullOrEmpty(objEmployeeBasicInfo.empDueDate))
                                txtExpiredOn.Text = Convert.ToDateTime(objEmployeeBasicInfo.empDueDate.ToString()).ToString(Resources.Constants.HRMSDateFormatShort);
                            else
                                txtExpiredOn.Text = string.Empty;

                            ddlGender.SelectedValue = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empGender.ToString());
                            ddlMaritalStatus.SelectedValue = objEmployeeBasicInfo.empMaritalStatus.ToString();
                            txtPersonalEmail.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empEmail1);
                            ddlStatus.SelectedValue = objEmployeeBasicInfo.empCurStatus.ToString();
                            GetFieldValues(ControlsEnum.EMPRELSTATUS);
                            GetFieldValues(ControlsEnum.EMPRELCANCELSTATUS);
                            SetFieldValues(ControlsEnum.EMPRELCANCELSTATUS);
                            EmpStatusCheck(objEmployeeBasicInfo.empCurStatus);
                            lblhdrStatusTxtPopup.Text = lblhdrStatusTxtPopupBranch.Text = lblStatusEmpymntTye.Text = lblEmpStatusTxt.Text = lblDEmpStatusText.Text = HttpUtility.HtmlDecode(ddlStatus.SelectedItem.Text);
                            ddlStatusPopUp.SelectedValue = objEmployeeBasicInfo.empCurStatus.ToString();
                            hdfReportTo.Value = objEmployeeBasicInfo.empReportTo.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empReportTo.ToString();

                            hdfReportToAdm.Value = string.IsNullOrEmpty(objEmployeeBasicInfo.empReportTo1.ToString()) ? "-1" : objEmployeeBasicInfo.empReportTo1.ToString();
                            txtReportToAdm.Text = objEmployeeBasicInfo.empReportTo1Text == null ? string.Empty : objEmployeeBasicInfo.empReportTo1Text.ToString();
                            if (Convert.ToInt16(hdfReportTo.Value) != 0)
                            {
                                ReportToPk = -1;
                                Int32.TryParse(objEmployeeBasicInfo.empReportTo.ToString(), out ReportToPk);
                                txtReportTo.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empReportToText.ToString());
                                hdfReportTo.Value = objEmployeeBasicInfo.empReportTo.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empReportTo.ToString();

                            }
                            hdfDepartment.Value = objEmployeeBasicInfo.empDepartment.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empDepartment.ToString();
                            if (Convert.ToInt16(hdfDepartment.Value) != 0)
                            {
                                departmentPk = -1;
                                Int32.TryParse(objEmployeeBasicInfo.empDepartment.ToString(), out departmentPk);
                                txtDepartment.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText.ToString());
                                hdfDepartment.Value = objEmployeeBasicInfo.empDepartment.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empDepartment.ToString();
                            }

                            hdfTeam.Value = objEmployeeBasicInfo.EmpTeamId.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.EmpTeamId.ToString();
                            if (Convert.ToInt16(hdfTeam.Value) != 0)
                            {
                                TeamPk = -1;
                                Int32.TryParse(objEmployeeBasicInfo.EmpTeamId.ToString(), out TeamPk);
                                txtTeam.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.EmpTeamText.ToString());
                                hdfTeam.Value = objEmployeeBasicInfo.EmpTeamId.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.EmpTeamId.ToString();
                            }

                            hdfCostCenter.Value = objEmployeeBasicInfo.EmpCostcenterId.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.EmpCostcenterId.ToString();
                            if (Convert.ToInt16(hdfCostCenter.Value) != 0)
                            {
                                CostCenterPk = -1;
                                Int32.TryParse(objEmployeeBasicInfo.EmpCostcenterId.ToString(), out CostCenterPk);
                                txtCostCenter.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.EmpCostcenterText.ToString());
                                hdfCostCenter.Value = objEmployeeBasicInfo.EmpCostcenterId.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.EmpCostcenterId.ToString();
                            }

                            hdfCountry.Value = objEmployeeBasicInfo.empCountry1.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empCountry1.ToString();
                            if (Convert.ToInt16(hdfCountry.Value) != 0)
                            {
                                Country1Pk = -1;
                                Int32.TryParse(objEmployeeBasicInfo.empCountry1.ToString(), out Country1Pk);
                                txtCountry.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCountry1Text.ToString());
                                hdfCountry.Value = objEmployeeBasicInfo.empCountry1.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empCountry1.ToString();
                            }

                            hdfCountry1.Value = objEmployeeBasicInfo.empCountry2.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empCountry2.ToString();
                            if (Convert.ToInt16(hdfCountry1.Value) != 0)
                            {
                                Country2Pk = -1;
                                Int32.TryParse(objEmployeeBasicInfo.empCountry2.ToString(), out Country2Pk);
                                txtCountry1.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCountry2Text.ToString());
                                hdfCountry1.Value = objEmployeeBasicInfo.empCountry2.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empCountry2.ToString();
                            }

                            hdfBranchLocation.Value = objEmployeeBasicInfo.empBranch.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empBranch.ToString();
                            if (Convert.ToInt16(hdfBranchLocation.Value) != 0)
                            {
                                BranchPk = -1;
                                Int32.TryParse(objEmployeeBasicInfo.empBranch.ToString(), out BranchPk);
                                txtBranchLocation.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empBranchText.ToString());
                                lblhdrBranchTxtPopup.Text = lblBrLocEmpymntTye.Text = lblEmpLocTxt.Text = lblDEmpBrchText.Text = txtBranchLocation.Text;
                                lblhdrBranchTxtPopupBranch.Text = ERP.Utilities.CommonFunctions.GetShortString(objEmployeeBasicInfo.empBranchText.ToString(), 26);
                                lblhdrBranchTxtPopupBranch.ToolTip = objEmployeeBasicInfo.empBranchText.HtmlDecode();
                                hdfBranchLocation.Value = objEmployeeBasicInfo.empBranch.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empBranch.ToString();
                            }

                            hdfProfession.Value = objEmployeeBasicInfo.empProfession.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empProfession.ToString();
                            if (Convert.ToInt16(hdfProfession.Value) != 0)
                            {
                                professionPk = -1;
                                Int32.TryParse(objEmployeeBasicInfo.empProfession.ToString(), out professionPk);
                                txtProfession.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empProfessionText.ToString());
                                hdfProfession.Value = objEmployeeBasicInfo.empProfession.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empProfession.ToString();
                            }

                            hdfNationalityPK.Value = objEmployeeBasicInfo.empNationality1.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empNationality1.ToString();
                            if (Convert.ToInt16(hdfNationalityPK.Value) != 0)
                            {
                                Nationality1Pk = -1;
                                Int32.TryParse(objEmployeeBasicInfo.empNationality1.ToString(), out Nationality1Pk);
                                txtNationality.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNationality1Text.ToString());
                                hdfNationalityPK.Value = objEmployeeBasicInfo.empNationality1.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empNationality1.ToString();
                            }

                            hdfCountryOfBirth.Value = objEmployeeBasicInfo.empNationality2.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empNationality2.ToString();
                            if (Convert.ToInt16(hdfCountryOfBirth.Value) != 0)
                            {
                                Nationality2Pk = -1;
                                Int32.TryParse(objEmployeeBasicInfo.empNationality2.ToString(), out Nationality2Pk);
                                txtCountryOfBirth.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNationality2Text.ToString());
                                hdfCountryOfBirth.Value = objEmployeeBasicInfo.empNationality2.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empNationality2.ToString();
                            }

                            //Commented For autoSearch Religion and subreligion

                            //hdfReligion.Value = objEmployeeBasicInfo.empReligion.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empReligion.ToString();
                            //if (Convert.ToInt16(hdfReligion.Value) != 0)
                            //{
                            //    ReligionPk = -1;
                            //    Int32.TryParse(objEmployeeBasicInfo.empReligion.ToString(), out ReligionPk);
                            //    txtreligion.Text = HttpUtility.HtmlEncode(objEmployeeBasicInfo.empReligionText.ToString());
                            //    hdfReligion.Value = objEmployeeBasicInfo.empReligion.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empReligion.ToString();
                            //}

                            //hdfSubReligion.Value = objEmployeeBasicInfo.empSubReligion.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empSubReligion.ToString();
                            //if (Convert.ToInt16(hdfSubReligion.Value) != 0)
                            //{
                            //    SubReligionPk = -1;
                            //    Int32.TryParse(objEmployeeBasicInfo.empSubReligion.ToString(), out SubReligionPk);
                            //    txtSubReligion.Text = HttpUtility.HtmlEncode(objEmployeeBasicInfo.empSubReligionText.ToString());
                            //    hdfSubReligion.Value = objEmployeeBasicInfo.empSubReligion.ToString() == string.Empty ? "-1" : objEmployeeBasicInfo.empSubReligion.ToString();
                            //}

                            ddlReligion.SelectedValue = objEmployeeBasicInfo.empReligion.ToString();
                            GetFieldValues(ControlsEnum.SUBRELIGION);
                            SetFieldValues(ControlsEnum.SUBRELIGION);
                            ddlSubReligion.SelectedValue = objEmployeeBasicInfo.empSubReligion.ToString();

                            ddlEmploymentType.SelectedValue = objEmployeeBasicInfo.empEmploymentType.ToString();
                            ddlEmpymntTypePopup.SelectedValue = objEmployeeBasicInfo.empEmploymentType.ToString();


                            ddlJobLevel.SelectedValue = objEmployeeBasicInfo.empJobLevel.ToString();
                            ddljobCategory.SelectedValue = objEmployeeBasicInfo.empJobCategory.ToString();
                            ddlJobStream.SelectedValue = objEmployeeBasicInfo.empJobStream.ToString();
                            ddlSkillLevel.SelectedValue = objEmployeeBasicInfo.empSkillLevel.ToString();

                            ddlCompany.SelectedValue = objEmployeeBasicInfo.empCompany.ToString();
                            hdfDesignation.Value = objEmployeeBasicInfo.empDesignation.ToString();
                            txtDesignation.Text = objEmployeeBasicInfo.empDesignationText;
                            txtOfficialPhone.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPhone1);
                            txtOffPhoExtension.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPhoneExt1);
                            txtOfficialMobile.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empMobile2);
                            txtOfficialEmail.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empEmail2);
                            //objEmployeeBasicInfo.empCurStatus 
                            ////Contact Details
                            txtPermanentAddress.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empAddress1);
                            txtCommunicationAddress.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empAddress2);
                            // txtState.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empState1);
                            // txtState1.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empState2);
                            txtDistrict1.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDistrict1);
                            txtDistrict2.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDistrict2);
                            txtCity.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCity1);
                            txtCity1.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCity2);
                            TxtPlaceOfBirth.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPlaceOfBirth);

                            hdfState.Value = Convert.ToInt32(objEmployeeBasicInfo.empStatePK1) > 0 ? objEmployeeBasicInfo.empStatePK1 : CommonConstants.SELECTVAL;
                            txtState.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empStatePK1_TEXT);

                            hdfState1.Value = Convert.ToInt32(objEmployeeBasicInfo.empStatePK2) > 0 ? objEmployeeBasicInfo.empStatePK2 : CommonConstants.SELECTVAL;
                            txtState1.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empStatePK2_TEXT);

                            txtZipCode.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empZip1);
                            txtZipCode1.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empZip2);
                            txtPhone.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPhone2);
                            txtPhone1.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPhone3);

                            if (txtPermanentAddress.Text == txtCommunicationAddress.Text && txtState.Text == txtState1.Text &&
                                txtDistrict1.Text == txtDistrict2.Text && txtCity.Text == txtCity1.Text && txtZipCode.Text == txtZipCode1.Text &&
                                txtPhone.Text == txtPhone1.Text)
                            {
                                chkSameAsAbove.Checked = true;
                            }

                            ////Additional Info
                            TxtFatherName.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empFatherName);
                            txtMotherName.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empMotherName);
                            TxtSpouseName.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empSpouseName);

                            txtIncomeTaxNo.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empSpouseTaxNo);
                            txtSpouseSurname.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empSpouseBranch);
                            chkIsSpouseWorking.Checked = string.IsNullOrEmpty(objEmployeeBasicInfo.empSpouseIsWorking.ToString()) ? false : objEmployeeBasicInfo.empSpouseIsWorking == 1 ? true : false;

                            ddlBloodGroup.SelectedValue = objEmployeeBasicInfo.empBloodGroup.ToString();
                            txtNoOfchildren.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNoOfChildren);
                            txtAltContactNO.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPhone4);
                            //hdflastModifiedDate.Value = Convert.ToDateTime((objEmployeeBasicInfo.LAST_MOD_DT.ToString())).ToString(CommonConstants.DATEFORMAT);
                            //hdflastModifiedDate.Value = LastModifiedTime.ToString();
                            LastModifiedTime = objEmployeeBasicInfo.LAST_MOD_DT;

                            //Additional Info 14-11-2016
                            txtFIdNumber.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empFatherNameId);
                            txtMIdNumber.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empMotherNameId);
                            txtSIdNumber.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empSpouseNameId);
                            txtFatherSpouse.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empFatherofSpouse);
                            txtFatherSpouseIdNo.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empFatherofSpouseId);
                            txtMothrSpouse.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empMotherofSpouse);
                            txtMothrSpouseIdNo.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empMotherofSpouseId);
                            txtNoOfChildrenId.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNoOfChildrenId);
                            txtNoOfChildEduId.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNoOfChildEduId);
                            chkTaxpayer.Checked = string.IsNullOrEmpty(objEmployeeBasicInfo.empTaxPayer.ToString()) ? false : objEmployeeBasicInfo.empTaxPayer == 1 ? true : false;
                            txtNoOfchildrenEdu.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNoOfChildEdu);

                            lblLastModifiedDate.InnerHtml = Resources.ErpRes.LastModifiedOn + Convert.ToDateTime((objEmployeeBasicInfo.LAST_MOD_DT.ToString())).ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            ModifiedDatePnl.Visible = true;
                            txtOldCode.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empOldCode);
                            txtBioMetricId.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empBiometricId);
                            if (!string.IsNullOrEmpty(objEmployeeBasicInfo.empPhotoFileName))
                            {
                                string uploadPath = string.Empty;
                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                {
                                    uploadPath = ResolveUrl("~/Upload/Employee/" + objEmployeeBasicInfo.empPhotoFileName.ToString());
                                }
                                else
                                {
                                    uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + "Employee/" + objEmployeeBasicInfo.empPhotoFileName.ToString();
                                }

                                imgEmployee.ImageUrl = uploadPath;
                            }
                            else
                                imgEmployee.ImageUrl = System.Configuration.ConfigurationManager.AppSettings["EmployeeProPic"].ToLower() + Resources.ErpRes.NoImage.ToString();

                            hdfFileuRL.Value = "";
                            FilePath = objEmployeeBasicInfo.empPhotoFilePath;
                            FileName = objEmployeeBasicInfo.empPhotoFileName;

                            //payroll and basicpay
                            if (objEmployeeBasicInfo.empPayRollType != 0)
                            {
                                ddlPayrollType.SelectedValue = objEmployeeBasicInfo.empPayRollType.ToString();
                                ddlPayrollType.Enabled = false;
                            }
                            else
                            {
                                ddlPayrollType.Enabled = true;
                            }

                            txtBasicSalary.Text = GetFormattedCurrency(objEmployeeBasicInfo.empBasicPay.ToString());
                            if (string.IsNullOrEmpty(objEmployeeBasicInfo.PUM_PK))
                                divBasicPay.Visible = false;

                        }

                        break;
                    
                    #endregion

                    #region EMPTOTALCOUNT
                    case ControlsEnum.EMPTOTALCOUNT:
                        if (dtEmployeeList != null && dtEmployeeList.Rows.Count > 0)
                        {
                            TotalEmployees = Convert.ToInt32(dtEmployeeList.Rows[0]["TOT_ROW_COUNT"].ToString());
                        }
                        break; 
                    #endregion

                    #region SELECTEDPEYDETAILS
                    case ControlsEnum.SELECTEDPEYDETAILS:
                        BindDetailsView();
                        break; 
                    #endregion

                    #region SELECTED EMP TYPE
                    case ControlsEnum.SELECTEDEMPTYPE:
                        if (selectedEmployeeType != null)
                        {
                            chkOTAvailable.Checked = selectedEmployeeType.OTAvailable == "1" ? true : false;
                            if (chkOTAvailable.Checked)
                            {
                                rfvOTTemplate.Enabled = true;
                                ddlOTTemplate.Enabled = true;
                            }
                            else
                            {
                                rfvOTTemplate.Enabled = false;
                                ddlOTTemplate.Enabled = false;
                            }

                            //GetFieldValues(ControlsEnum.LEAVETEMPLATE);
                            //SetFieldValues(ControlsEnum.LEAVETEMPLATE);
                            if (!string.IsNullOrEmpty(selectedEmployeeType.LeaveTemplate))
                                ddlLeaveTemplate.SelectedValue = selectedEmployeeType.LeaveTemplate;
                            else
                                ddlLeaveTemplate.SelectedValue = CommonConstants.SELECTVAL;

                            //GetFieldValues(ControlsEnum.OTTEMPLATE);
                            //SetFieldValues(ControlsEnum.OTTEMPLATE);
                            if (selectedEmployeeType.OTAvailable == "1")
                            {
                                ddlOTTemplate.SelectedValue = selectedEmployeeType.OTTemplate;
                            }
                            else
                            {
                                ddlOTTemplate.SelectedValue = CommonConstants.SELECTVAL;
                            }

                            //GetFieldValues(ControlsEnum.WORKINGDAYSTYPE);
                            //SetFieldValues(ControlsEnum.WORKINGDAYSTYPE);
                            ddlWorkingDayType.SelectedValue = selectedEmployeeType.WorkingDayType.ToString();
                            if (!string.IsNullOrEmpty(ddlWorkingDayType.SelectedValue))
                            {
                                if (Convert.ToInt32(ddlWorkingDayType.SelectedValue) == (int)WorkingDayType.Fixed)
                                {
                                    txtWorkingDays.Enabled = true;
                                    txtWorkingDays.CssClass = "input-small numeric";
                                    vrfWorkingDays.Enabled = true;
                                }
                                else
                                {
                                    txtWorkingDays.Enabled = false;
                                    txtWorkingDays.CssClass = "input-small numeric input-disabled";
                                    vrfWorkingDays.Enabled = false;
                                    txtWorkingDays.Text = string.Empty;
                                }
                            }
                            txtWorkingDays.Text = !string.IsNullOrEmpty(selectedEmployeeType.WorkingDays) ? selectedEmployeeType.WorkingDays : string.Empty;
                            if (Convert.ToInt32(ddlWorkingDayType.SelectedValue) == 1)
                                vrfWorkingDays.Enabled = true;
                            else
                                vrfWorkingDays.Enabled = false;

                            txtNormalWorkingHrs.Text = string.IsNullOrEmpty(selectedEmployeeType.NormalWorkingHrs) ? string.Empty : selectedEmployeeType.NormalWorkingHrs;
                            txtOTRate.Text = string.IsNullOrEmpty(selectedEmployeeType.OTRate) ? string.Empty : GetFormattedCurrency(selectedEmployeeType.OTRate);

                            txtBreakTime.Text = string.IsNullOrEmpty(selectedEmployeeType.BreakTime) ? string.Empty : selectedEmployeeType.BreakTime;

                        }
                        else
                        {
                            ddlLeaveTemplate.SelectedValue = CommonConstants.SELECTVAL;
                            ddlOTTemplate.SelectedValue = CommonConstants.SELECTVAL;
                            ddlWorkingDayType.SelectedValue = CommonConstants.SELECTVAL;
                            txtNormalWorkingHrs.Text = string.Empty;
                            txtOTRate.Text = string.Empty;
                            chkOTAvailable.Checked = false;
                            rfvOTTemplate.Enabled = false;
                            ddlOTTemplate.Enabled = false;
                            vrfWorkingDays.Enabled = false;

                        }

                        break;
                    #endregion

                    #region BANK DETAILS
                    case ControlsEnum.BANKDETAILS:
                        if (dtBankDetails != null && dtBankDetails.Rows.Count > 0)
                        {
                            txtBranch.Text = HttpUtility.HtmlDecode(Convert.ToString(dtBankDetails.Rows[0]["CBM_BRANCH"]));
                            txtIFSCCode.Text = Convert.ToString(dtBankDetails.Rows[0]["CBM_IFSC_CODE"]);
                        }
                        break;
                    #endregion

                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            txtCurrency.Text = string.Format(GetLocalResourceObject("CurrencyDisplayFormat").ToString(), Convert.ToString(dtResult.Rows[0]["CUR_CODE"]), Convert.ToString(dtResult.Rows[0]["CUR_NAME"]));
                            hdfCurrency.Value = Convert.ToString(dtResult.Rows[0]["CUR_PK"]);
                        }
                        break; 
                    #endregion

                    #region GET JOB DESIGNATION DETAILS
                    case ControlsEnum.GETJOBDESIGNATIONDETAILS:
                        if (dtDesignationDetails != null && dtDesignationDetails.Rows.Count > 0)
                        {
                            ddlJobLevel.ClearSelection();
                            ddljobCategory.ClearSelection();
                            if (!string.IsNullOrEmpty(dtDesignationDetails.Rows[0]["dsgJobCategory"].ToString()))
                                ddljobCategory.SelectedValue = dtDesignationDetails.Rows[0]["dsgJobCategory"].ToString();
                            else
                                ddljobCategory.SelectedValue = CommonConstants.SELECTVAL;

                            if (!string.IsNullOrEmpty(dtDesignationDetails.Rows[0]["dsgJobLevel"].ToString()))
                                ddlJobLevel.SelectedValue = dtDesignationDetails.Rows[0]["dsgJobLevel"].ToString();
                            else
                                ddlJobLevel.SelectedValue = CommonConstants.SELECTVAL;

                            ddljobCategory.Enabled = ddlJobLevel.Enabled = false;
                        }
                        else
                        {
                            ddljobCategory.Enabled = ddlJobLevel.Enabled = true;
                        }
                        break;
                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }

        private void BindDetailsView()
        {
            if (selectedEmployeePayDetails != null)
            {
                try
                {
                    this.SelectedPK = selectedEmployeePayDetails.EPD_PK;
                    //btnDelete.Visible = this.SelectedPK != 0;
                    //txtBasic.Text = selectedEmployeePayDetails.EPD_BASIC_PAY.ToString();
                    //txtPANNo.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_PAN_NO);
                    GetFieldValues(ControlsEnum.PAYMENTMODE);
                    SetFieldValues(ControlsEnum.PAYMENTMODE);
                    ddlPaymentMode.SelectedValue = selectedEmployeePayDetails.EPD_PAY_MODE.ToString();
                    // DisableBankDetails(ddlPaymentMode.SelectedValue == "4");
                    //GetFieldValues(ControlsEnum.SALARYTEMPLATE);
                    //SetFieldValues(ControlsEnum.SALARYTEMPLATE);
                    //ddlSalaryTemplate.SelectedValue = selectedEmployeePayDetails.EPD_SALARY_TEMP.ToString();
                    int.TryParse(selectedEmployeePayDetails.EPD_PAY_BANK, out BankPk);
                    GetFieldValues(ControlsEnum.BANK);
                    SetFieldValues(ControlsEnum.BANK);
                    if (!string.IsNullOrWhiteSpace(selectedEmployeePayDetails.EPD_PAY_BANK))
                    {
                        ddlBankName.SelectedValue = selectedEmployeePayDetails.EPD_PAY_BANK.ToString();
                    }

                    txtBranch.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_PAY_BANK_BRANCH);
                    txtAccountCode.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_BANK_AC_NO);
                    txtAccountName.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_BANK_AC_NAME);
                    txtIFSCCode.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_BANK_IFSC);
                    //txtPFAccount.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_PF_AC);
                    //if (selectedEmployeePayDetails.EPD_PF_DATE != null)
                    //    txtPFEffectiveDate.Text = Convert.ToDateTime(selectedEmployeePayDetails.EPD_PF_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                    //txtSOCSOAccount.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_SOCSO_AC);
                    //if (selectedEmployeePayDetails.EPD_SOCSO_DATE != null)
                    //    txtSOCSOEffectiveDate.Text = Convert.ToDateTime(selectedEmployeePayDetails.EPD_SOCSO_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                    txtCurrency.Text = HttpUtility.HtmlDecode(selectedEmployeePayDetails.EPD_CURRENCY_TEXT);
                    hdfCurrency.Value = Convert.ToString(selectedEmployeePayDetails.EPD_CURRENCY);

                    GetFieldValues(ControlsEnum.EMPLOYEEMENTTYPE);
                    SetFieldValues(ControlsEnum.EMPLOYEEMENTTYPE);
                    ddlEmployementType.SelectedValue = selectedEmployeePayDetails.EPD_EMP_TYPE.ToString();

                    GetFieldValues(ControlsEnum.WORKINGDAYS);
                    SetFieldValues(ControlsEnum.WORKINGDAYS);

                    GetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    SetFieldValues(ControlsEnum.LEAVETEMPLATE);
                    if (!string.IsNullOrEmpty(selectedEmployeePayDetails.EPD_LEAVE_TEMP))
                        ddlLeaveTemplate.SelectedValue = selectedEmployeePayDetails.EPD_LEAVE_TEMP;
                    else
                        ddlLeaveTemplate.SelectedValue = CommonConstants.SELECTVAL;

                    GetFieldValues(ControlsEnum.OTTEMPLATE);
                    SetFieldValues(ControlsEnum.OTTEMPLATE);
                    if (selectedEmployeePayDetails.EPD_OT_AVAILABE == "1")
                    {
                        ddlOTTemplate.SelectedValue = selectedEmployeePayDetails.EPD_OT_TEMP;
                        chkOTAvailable.Checked = true;
                        rfvOTTemplate.Enabled = true;
                        ddlOTTemplate.Enabled = true;
                    }
                    else
                    {
                        ddlOTTemplate.SelectedValue = CommonConstants.SELECTVAL;
                        chkOTAvailable.Checked = false;
                        rfvOTTemplate.Enabled = false;
                        ddlOTTemplate.Enabled = false;
                    }
                    if (selectedEmployeePayDetails.EPD_HAS_OT_FROM_ATT == "1")
                    {
                        chkOTPending.Checked = true;
                    }
                    else
                    {
                        chkOTPending.Checked = false;
                    }

                    if (selectedEmployeePayDetails.EPD_CONSIDER_LATE_HRS == "1")
                        chkConsiderLateHrs.Checked = true;
                    else
                        chkConsiderLateHrs.Checked = false;

                    if (selectedEmployeePayDetails.EDP_ESI_REQUIRED == "1")
                        chkESIReqrd.Checked = true;
                    else
                        chkESIReqrd.Checked = false;

                    if (selectedEmployeePayDetails.EDP_PF_REQUIRED == "1")
                        chkPFRequrd.Checked = true;
                    else
                        chkPFRequrd.Checked = false;

                    if (selectedEmployeePayDetails.EDP_SSO_REQUIRED == "1")
                        chkSSOReq.Checked = true;
                    else
                        chkSSOReq.Checked = false;

                    GetFieldValues(ControlsEnum.WORKINGDAYSTYPE);
                    SetFieldValues(ControlsEnum.WORKINGDAYSTYPE);

                    if (!string.IsNullOrEmpty(selectedEmployeePayDetails.EPD_WORKING_DAY_TYPE))
                    {
                        ddlWorkingDayType.SelectedValue = selectedEmployeePayDetails.EPD_WORKING_DAY_TYPE;
                        if (Convert.ToInt32(ddlWorkingDayType.SelectedValue) == (int)WorkingDayType.Fixed)
                        {
                            txtWorkingDays.Enabled = true;
                            txtWorkingDays.CssClass = "input-small numeric";
                            vrfWorkingDays.Enabled = true;
                        }
                        else
                        {
                            txtWorkingDays.Enabled = false;
                            txtWorkingDays.CssClass = "input-small numeric input-disabled";
                            vrfWorkingDays.Enabled = false;
                            txtWorkingDays.Text = string.Empty;
                        }
                    }
                    txtWorkingDays.Text = !string.IsNullOrEmpty(selectedEmployeePayDetails.EPD_WORKING_DAY) ? selectedEmployeePayDetails.EPD_WORKING_DAY : string.Empty;
                    if (Convert.ToInt32(ddlWorkingDayType.SelectedValue) == 1)
                        vrfWorkingDays.Enabled = true;
                    else
                        vrfWorkingDays.Enabled = false;

                    txtNormalWorkingHrs.Text = string.IsNullOrEmpty(selectedEmployeePayDetails.EPD_WRK_HRS) ? string.Empty : selectedEmployeePayDetails.EPD_WRK_HRS;
                    txtOTRate.Text = string.IsNullOrEmpty(selectedEmployeePayDetails.EPD_OT_RATE) ? string.Empty : GetFormattedCurrency(selectedEmployeePayDetails.EPD_OT_RATE);

                    txtBreakTime.Text = string.IsNullOrEmpty(selectedEmployeePayDetails.EPD_BREAK_TIME) ? string.Empty : selectedEmployeePayDetails.EPD_BREAK_TIME;
                    //ddlOTTemplate.SelectedValue = selectedEmployeePayDetails.EPD_OT_TEMP.ToString();

                    //this.LastModifiedTime = selectedEmployeePayDetails.LastModifiedDate;
                    //lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                    //lblLastModifiedHDR.Visible = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                //lblLastModifiedHDR.Visible = false;
            }
        }

        private string NoOfYears(string p)
        {
            string[] dtSplit = p.Split('(');
            if (dtSplit.Count() > 1)
                return "(" + dtSplit[1];
            else
                return string.Empty;
        }

        private string DateDiffInWords(string date)
        {
            DateTime now = DateTime.Now;
            DateTime oldDate;

            if (!DateTime.TryParse(date.Trim(), out oldDate))
                return string.Empty;

            int years;
            int months;
            int totalMonths = ((now.Year - oldDate.Year) * 12) + (now.Month - oldDate.Month);

            years = totalMonths / 12;
            months = totalMonths % 12;

            string retVal = string.Empty;
            if (years == 0 && months == 0) return string.Empty;
            else if (years > 0 && months > 0) retVal = string.Format("{0} {1}m", retVal, months);
            else if (years > 0 && months == 0) retVal = string.Format("{0}y", years);
            else if (years == 0 && months > 0) retVal = string.Format("{0}m", months);

            return retVal;
        }

        private string AgeInWords(string age)
        {
            if (age.IsNullOrEmptyOrWhitespace() || !age.Contains('(') || !age.Contains(')'))
                return string.Empty;

            bool hasYear = false;
            bool hasMonth = false;

            age = age.Substring(age.IndexOf('(') + 1);
            age = age.Remove(age.IndexOf(')'));

            if (age.Contains('y')) hasYear = true;
            if (age.Contains('m')) hasMonth = true;

            age = age.Replace("y", string.Empty).Replace("m", string.Empty).Trim();

            string[] dummy = age.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            string ageInwords = string.Empty;
            int year = 0;
            int month = 0;

            //string yearString =  "year", monthString = "month";
            string yearString = GetLocalResourceObject("YearFormatAge").ToString(), monthString = GetLocalResourceObject("MonthFormatAge").ToString();

            if (dummy != null && dummy.Length > 0 && hasYear)
            {
                Int32.TryParse(dummy[0].Trim(), out year);

                if (year > 1) yearString = GetLocalResourceObject("YearFormatAge").ToString();

                ageInwords = string.Format("{0}{1}", year, yearString).Trim();

                if (year < 1) ageInwords = string.Empty;
                else ageInwords = string.Format("{0}{1}", ageInwords, hasMonth ? " " : string.Empty);

                if (dummy.Length > 1)
                {
                    Int32.TryParse(dummy[1].Trim(), out month);
                    if (month > 1) monthString = GetLocalResourceObject("MonthFormatAge").ToString();
                    if (month < 1) monthString = string.Empty;
                    ageInwords = string.Format("{0}{1}{2}", ageInwords, (month > 0 ? month.ToString() : string.Empty), monthString).Trim();
                }
                ageInwords = string.Format("{0}", ageInwords);
            }
            else if (dummy != null && dummy.Length > 0 && hasMonth)
            {
                Int32.TryParse(dummy[0].Trim(), out month);
                if (month > 1) monthString = GetLocalResourceObject("MonthFormatAge").ToString();
                if (month < 1) monthString = string.Empty;
                ageInwords = string.Format("{0}{1}", month, monthString).Trim();
            }

            return ageInwords;
        }

        private string DOJInWords(string age)
        {
            if (age.IsNullOrEmptyOrWhitespace() || !age.Contains('(') || !age.Contains(')'))
                return string.Empty;

            bool hasYear = false;
            bool hasMonth = false;

            age = age.Substring(age.IndexOf('(') + 1);
            age = age.Remove(age.IndexOf(')'));

            if (age.Contains('y')) hasYear = true;
            if (age.Contains('m')) hasMonth = true;

            age = age.Replace("y", string.Empty).Replace("m", string.Empty).Trim();

            string[] dummy = age.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            string ageInwords = string.Empty;
            int year = 0;
            int month = 0;
            string yearString = "year", monthString = "month";

            if (dummy != null && dummy.Length > 0 && hasYear)
            {
                Int32.TryParse(dummy[0].Trim(), out year);

                if (year > 1) yearString = "years";

                ageInwords = string.Format("{0} {1}", year, yearString).Trim();

                if (year < 1) ageInwords = string.Empty;
                else ageInwords = string.Format("{0}{1}", ageInwords, hasMonth ? " and " : string.Empty);

                if (dummy.Length > 1)
                {
                    Int32.TryParse(dummy[1].Trim(), out month);
                    if (month > 1) monthString = "months";
                    if (month < 1) monthString = string.Empty;
                    ageInwords = string.Format("{0}{1} {2}", ageInwords, (month > 0 ? month.ToString() : string.Empty), monthString).Trim();
                }
                ageInwords = string.Format("{0}", ageInwords);
            }
            else if (dummy != null && dummy.Length > 0 && hasMonth)
            {
                Int32.TryParse(dummy[0].Trim(), out month);
                if (month > 1) monthString = "months";
                if (month < 1) monthString = string.Empty;
                ageInwords = string.Format("{0} {1} ", month, monthString).Trim();
            }

            return ageInwords;
        }

        private void GetUIValuesFromObjectHeader(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EMPLOYEE DETAILS HEADER
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        if (objEmployeeBasicInfo != null)
                        {
                            //currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            CurrPK = objEmployeeBasicInfo.empPK;
                            UCempBasicHdr.objEmployeeBasicInfo = objEmployeeBasicInfo;
                            UCempBasicHdr.SetFieldValues();
                            lblhdrEmployeeNoTxtPopup.Text = lblhdrEmployeeNoTxtPopupBranch.Text = lblEmployeeNoEmpymntTye.Text = lblEmpNoTxt.Text = lblDEmpNoText.Text =
                                CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode), 20);
                            lblhdrEmployeeNameTxtPopup.Text = lblhdrEmployeeNameTxtPopupBranch.Text = lblEmployeeNameEmpymntTye.Text = lblEmpNameTxt.Text = lblDEmpNameText.Text =
                                CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText), 20);



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

                            lblhdrDesignationTxtPopup.Text = lblhdrDesignationTxtPopupBranch.Text = lblEmpDesigTxt.Text = lblDEmpDesigText.Text =
                              lblDesignationEmpymntTye.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText);
                            lblhdrDepartmentTxtPopup.Text = lblhdrDepartmentTxtPopupBranch.Text = lblDepartmentEmpymntTye.Text = lblEmpDeptTxt.Text = lblDEmpDeptText.Text =
                                CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText), 20);

                            //lblhdrEmployeeNoTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode);
                            //lblhdrEmployeeNameTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText);
                            //lblhdrDOJText.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJText);
                            //lblhdrDOBTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOBText);
                            //lblhdrDesignationTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText);
                            //lblhdrDepartmentTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText);
                        }

                        break;
                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }

        private void GetToolTips()
        {
            txtEmpCode.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode);
            txtFirstName.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName);
            txtMiddleName.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName2);
            txtLastName.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName3);
            txtSurname.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName4);
            txtEmpNameLL.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empName_LL);
            txtMobile.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empMobile1);
            txtDOB.ToolTip = Convert.ToDateTime(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOB.ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
            txtDOJ.ToolTip = Convert.ToDateTime(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJ.ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
            txtPersonalEmail.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empEmail1);
            txtOfficialPhone.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPhone1);
            txtOffPhoExtension.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPhoneExt1);
            txtOfficialMobile.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empMobile2);
            txtOfficialEmail.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empEmail2);
            txtPermanentAddress.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empAddress1);
            txtCommunicationAddress.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empAddress2);
            txtState.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empState1);
            txtState1.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empState2);
            txtDistrict1.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDistrict1);
            txtDistrict2.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDistrict2);
            txtCity.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCity1);
            txtCity1.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCity2);
            TxtPlaceOfBirth.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPlaceOfBirth);
            txtZipCode.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empZip1);
            txtZipCode1.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empZip2);
            txtPhone.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPhone2);
            txtPhone1.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPhone3);
            TxtFatherName.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empFatherName);
            txtMotherName.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empMotherName);
            TxtSpouseName.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empSpouseName);

            txtIncomeTaxNo.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empSpouseTaxNo);
            txtSpouseSurname.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empSpouseBranch); //Branch to Surname
            txtNoOfchildren.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNoOfChildren);
            txtAltContactNO.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empPhone4);

        }

        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                // Fill Shift Details to DropDown


                //case ControlsEnum.DESIGNATION:
                //    if (dtDesignation != null)
                //    {
                //        ddlDesignation.DataSource = CommonFunctions.HtmlDecodeDataTable(dtDesignation, ("dsgName").ToString());
                //        ddlDesignation.DataTextField = "dsgName";
                //        ddlDesignation.DataValueField = "dsgPK";
                //        ddlDesignation.DataBind();

                //        ddlDesignation.Items.Insert(0, new ListItem("Select", "-1"));
                //    }

                //    break;

                case ControlsEnum.EMPLOYMENTTYPE:
                    if (dtEmploymentType != null)
                    {
                        ddlEmploymentType.DataSource = ddlEmpymntTypePopup.DataSource = CommonFunctions.HtmlDecodeDataTable(dtEmploymentType, ("CON_NAME").ToString());
                        ddlEmploymentType.DataTextField = ddlEmpymntTypePopup.DataTextField = "CON_NAME";
                        ddlEmploymentType.DataValueField = ddlEmpymntTypePopup.DataValueField = "CON_PK";
                        ddlEmploymentType.DataBind();
                        ddlEmploymentType.Items.Insert(0, new ListItem("Select", "-1"));

                        ddlEmpymntTypePopup.DataBind();
                        ddlEmpymntTypePopup.Items.Insert(0, new ListItem("Select", "-1"));

                    }

                    break;
                case ControlsEnum.BLOODGROUP:
                    if (dtBloodGroup != null)
                    {
                        ddlBloodGroup.DataSource = CommonFunctions.HtmlDecodeDataTable(dtBloodGroup, ("CON_NAME").ToString());
                        ddlBloodGroup.DataTextField = "CON_NAME";
                        ddlBloodGroup.DataValueField = "CON_PK";
                        ddlBloodGroup.DataBind();

                        ddlBloodGroup.Items.Insert(0, new ListItem("Select", "-1"));
                    }

                    break;
                case ControlsEnum.STATUSLIST:
                    if (dtStatus != null)
                    {
                        ddlStatus.DataSource = dtStatus;
                        ddlStatus.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                        ddlStatus.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                        ddlStatus.DataBind();
                        ddlStatusPopUp.DataSource = dtStatus;
                        ddlStatusPopUp.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                        ddlStatusPopUp.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                        ddlStatusPopUp.DataBind();
                    }

                    break;

                case ControlsEnum.EMPRELCANCELSTATUS:
                    if (dtEmpRelCanStatus != null)
                    {
                        ddlStatusCancelPopUp.DataSource = dtEmpRelCanStatus;
                        ddlStatusCancelPopUp.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                        ddlStatusCancelPopUp.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                        ddlStatusCancelPopUp.DataBind();
                    }
                    break;
                case ControlsEnum.JOBLEVEL:
                    if (dtJobLevel != null)
                    {
                        ddlJobLevel.DataSource = CommonFunctions.HtmlDecodeDataTable(dtJobLevel, ("CON_NAME").ToString());
                        ddlJobLevel.DataTextField = "CON_NAME";
                        ddlJobLevel.DataValueField = "CON_PK";
                        ddlJobLevel.DataBind();
                        ddlJobLevel.Items.Insert(0, new ListItem("Select", "-1"));
                    }

                    break;
                case ControlsEnum.JOBCATEGORY:
                    if (dtJobCategory != null)
                    {
                        ddljobCategory.DataSource = CommonFunctions.HtmlDecodeDataTable(dtJobCategory, ("CON_NAME").ToString());
                        ddljobCategory.DataTextField = "CON_NAME";
                        ddljobCategory.DataValueField = "CON_PK";
                        ddljobCategory.DataBind();
                        ddljobCategory.Items.Insert(0, new ListItem("Select", "-1"));
                    }

                    break;
                case ControlsEnum.JOBSTREAM:
                    if (dtJobStream != null)
                    {
                        ddlJobStream.DataSource = CommonFunctions.HtmlDecodeDataTable(dtJobStream, ("CON_NAME").ToString());
                        ddlJobStream.DataTextField = "CON_NAME";
                        ddlJobStream.DataValueField = "CON_PK";
                        ddlJobStream.DataBind();
                        ddlJobStream.Items.Insert(0, new ListItem("Select", "-1"));
                    }

                    break;
                case ControlsEnum.SKILLLEVEL:
                    if (dtSkillLevel != null)
                    {
                        ddlSkillLevel.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSkillLevel, ("CON_NAME").ToString());
                        ddlSkillLevel.DataTextField = "CON_NAME";
                        ddlSkillLevel.DataValueField = "CON_PK";
                        ddlSkillLevel.DataBind();

                        ddlSkillLevel.Items.Insert(0, new ListItem("Select", "-1"));
                    }

                    break;
                case ControlsEnum.SALUTATION:
                    if (dtSalutation != null)
                    {
                        ddlSalutaion.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSalutation, ("CON_NAME").ToString());
                        ddlSalutaion.DataTextField = "CON_NAME";
                        ddlSalutaion.DataValueField = "CON_PK";
                        ddlSalutaion.DataBind();
                    }
                    ddlSalutaion.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

                    break;

                case ControlsEnum.RELIGION:
                    if (dtReligion != null)
                    {
                        ddlReligion.DataSource = CommonFunctions.HtmlDecodeDataTable(dtReligion, ("CON_NAME").ToString());
                        ddlReligion.DataTextField = "CON_NAME";
                        ddlReligion.DataValueField = "CON_PK";
                        ddlReligion.DataBind();
                        ddlReligion.Items.Insert(0, new ListItem("Select", "-1"));
                    }

                    break;

                case ControlsEnum.SUBRELIGION:
                    if (dtSubReligion != null)
                    {
                        ddlSubReligion.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSubReligion, ("CON_NAME").ToString());
                        ddlSubReligion.DataTextField = "CON_NAME";
                        ddlSubReligion.DataValueField = "CON_PK";
                        ddlSubReligion.DataBind();
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdditionalInfo", "ShowHideAdditionalInfo('1');", true);

                    break;
                case ControlsEnum.MARITALSTATUS:
                    if (dtMaritalStatus != null)
                    {
                        ddlMaritalStatus.DataSource = CommonFunctions.HtmlDecodeDataTable(dtMaritalStatus, ("CON_NAME").ToString());
                        ddlMaritalStatus.DataTextField = "CON_NAME";
                        ddlMaritalStatus.DataValueField = "CON_PK";
                        ddlMaritalStatus.DataBind();
                        ddlMaritalStatus.Items.Insert(0, new ListItem("Select", "-1"));
                    }
                    break;
                case ControlsEnum.GENDER:
                    if (dtGender != null)
                    {
                        ddlGender.DataSource = CommonFunctions.HtmlDecodeDataTable(dtGender, ("CON_NAME").ToString());
                        ddlGender.DataTextField = "CON_NAME";
                        ddlGender.DataValueField = "CON_PK";
                        ddlGender.DataBind();
                    }
                    ddlGender.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #region Company
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecodeDataTable(dtCompany, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();

                        ddlCompany.Items.Insert(0, new ListItem("Select", "-1"));
                    }
                    //if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    //{
                    //    ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                    //    ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                    //    ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                    //    ddlCompany.DataBind();

                    //    ddlCompany.Items.Insert(0, new ListItem("Select", "-1"));
                    //}
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        // ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                        ddlCompany.SelectedValue = currentUser.SBUID.ToString();
                    }

                    break;
                #endregion

                case ControlsEnum.PAYMENTMODE:
                    ddlPaymentMode.DataSource = pageData;
                    ddlPaymentMode.DataTextField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_TEXT;
                    ddlPaymentMode.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CFG_VALUE;
                    ddlPaymentMode.DataBind();
                    ddlPaymentMode.Items.HtmlDecode();
                    ddlPaymentMode.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.BANK:
                    ddlBankName.DataSource = pageData;
                    ddlBankName.DataTextField = GTIService.Constants.HRMS.Employee.Fields.CBM_NAME;
                    ddlBankName.DataValueField = GTIService.Constants.HRMS.Employee.Fields.CBM_PK;
                    ddlBankName.DataBind();
                    ddlBankName.Items.HtmlDecode();
                    ddlBankName.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                //case ControlsEnum.SALARYTEMPLATE:
                //    ddlSalaryTemplate.DataSource = pageData;
                //    ddlSalaryTemplate.DataTextField = GTIService.Constants.HRMS.Employee.Fields.STE_NAME;
                //    ddlSalaryTemplate.DataValueField = GTIService.Constants.HRMS.Employee.Fields.STE_PK;
                //    ddlSalaryTemplate.DataBind();
                //    ddlSalaryTemplate.Items.HtmlDecode();
                //    ddlSalaryTemplate.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                //    break;
                case ControlsEnum.EMPLOYEEMENTTYPE:
                    ddlEmployementType.DataSource = pageData;
                    ddlEmployementType.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_NAME;
                    ddlEmployementType.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_PK;
                    ddlEmployementType.DataBind();
                    ddlEmployementType.Items.HtmlDecode();
                    ddlEmployementType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.LEAVETEMPLATE:
                    ddlLeaveTemplate.DataSource = pageData;
                    ddlLeaveTemplate.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.LTE_NAME;
                    ddlLeaveTemplate.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.LTE_PK;
                    ddlLeaveTemplate.DataBind();
                    ddlLeaveTemplate.Items.HtmlDecode();
                    ddlLeaveTemplate.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.OTTEMPLATE:
                    ddlOTTemplate.DataSource = pageData;
                    ddlOTTemplate.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.OTE_NAME;
                    ddlOTTemplate.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.OTE_PK;
                    ddlOTTemplate.DataBind();
                    ddlOTTemplate.Items.HtmlDecode();
                    ddlOTTemplate.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #region WORKING DAYS TYPE
                case ControlsEnum.WORKINGDAYSTYPE:
                    ddlWorkingDayType.Items.Clear();
                    ddlWorkingDayType.DataSource = pageData;
                    ddlWorkingDayType.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                    ddlWorkingDayType.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                    ddlWorkingDayType.DataBind();
                    ddlWorkingDayType.Items.HtmlDecode();
                    ddlWorkingDayType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                case ControlsEnum.PAYROLLTYPE:
                    ddlPayrollType.DataSource = dtPayrollType;
                    ddlPayrollType.DataTextField = GTIService.Constants.HRMS.Employee.Fields.PTM_NAME;
                    ddlPayrollType.DataValueField = GTIService.Constants.HRMS.Employee.Fields.PTM_PK;
                    ddlPayrollType.Items.HtmlDecode();
                    ddlPayrollType.DataBind();
                    ddlPayrollType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Method for Bind Grids
        /// </summary>
        /// <param name="controlType"></param>
        private void BindGrid(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.DETAILS:
                    grdStatusHistory.DataSource = dtStatusHistory;
                    grdStatusHistory.DataBind();
                    break;
                case ControlsEnum.DEPARTMENTDETAILS:
                    grdDepartmentHistory.DataSource = dtDepartmentHistory;
                    grdDepartmentHistory.DataBind();
                    break;
                case ControlsEnum.DESIGNATIONDETAILS:
                    grdDesignationHistory.DataSource = dtDesignationHistory;
                    grdDesignationHistory.DataBind();
                    break;
                case ControlsEnum.BRANCHDETAILS:
                    grdBranchHistory.DataSource = dtBranchHistory;
                    grdBranchHistory.DataBind();
                    break;
                #region Working Days
                case ControlsEnum.WORKINGDAYS:
                    grdWorkingDays.DataSource = _workingHoursList;
                    grdWorkingDays.DataBind();
                    break;
                #endregion
                case ControlsEnum.EMPYMNTTYPEUPDN:
                    grdEmpymntTypeHistory.DataSource = dtEmpymntTypeHistory;
                    grdEmpymntTypeHistory.DataBind();
                    break;
                default:
                    break;
            }
        }

        private void ResetForm()
        {
            CurrPK = 0;
            txtEmpCode.Text = string.Empty;
            ddlSalutaion.Items.Clear();
            ddlSalutaion.ClearSelection();
            GetFieldValues(ControlsEnum.SALUTATION);
            SetFieldValues(ControlsEnum.SALUTATION);
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtSurname.Text = string.Empty;
            txtEmpNameLL.Text = string.Empty;
            ddlMaritalStatus.Items.Clear();
            ddlMaritalStatus.ClearSelection();
            GetFieldValues(ControlsEnum.MARITALSTATUS);
            SetFieldValues(ControlsEnum.MARITALSTATUS);
            ddlMaritalStatus.SelectedIndex = 0;
            ddlStatus.Items.Clear();
            ddlStatus.ClearSelection();
            ddlStatusPopUp.Items.Clear();
            ddlStatusPopUp.ClearSelection();
            GetFieldValues(ControlsEnum.STATUSLIST);
            SetFieldValues(ControlsEnum.STATUSLIST);
            txtPersonalEmail.Text = "";
            txtNationality.Text = "";
            txtDOB.Text = "";
            ddlGender.Items.Clear();
            ddlGender.ClearSelection();
            GetFieldValues(ControlsEnum.GENDER);
            SetFieldValues(ControlsEnum.GENDER);
            txtMobile.Text = "";
            txtDepartment.Text = "";
            txtTeam.Text = "";
            txtCostCenter.Text = "";
            txtDesignation.Text = String.Empty;
            hdfDesignation.Value = "0";
            GetFieldValues(ControlsEnum.DESIGNATION);
            SetFieldValues(ControlsEnum.DESIGNATION);
            ddlEmploymentType.Items.Clear();
            ddlEmploymentType.ClearSelection();
            ddlEmpymntTypePopup.Items.Clear();
            ddlEmpymntTypePopup.ClearSelection();

            ddlReligion.Items.Clear();
            ddlReligion.ClearSelection();
            GetFieldValues(ControlsEnum.RELIGION);
            SetFieldValues(ControlsEnum.RELIGION);
            ddlReligion.SelectedIndex = 0;
            GetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
            SetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
            ddlEmploymentType.SelectedIndex = 0;
            ddlEmpymntTypePopup.SelectedIndex = 0;
            GetFieldValues(ControlsEnum.BLOODGROUP);
            SetFieldValues(ControlsEnum.BLOODGROUP);
            ddlBloodGroup.SelectedIndex = 0;
            txtBranchLocation.Text = "";
            txtDOJ.Text = "";
            ddlCompany.Items.Clear();
            ddlCompany.ClearSelection();
            GetFieldValues(ControlsEnum.COMPANY);
            SetFieldValues(ControlsEnum.COMPANY);
            ddlCompany.SelectedIndex = 0;
            txtConfirmedOn.Text = "";
            txtReportTo.Text = "";
            txtOfficialEmail.Text = "";
            txtOfficialPhone.Text = "";
            txtOffPhoExtension.Text = "";
            txtOfficialMobile.Text = "";
            ddljobCategory.Items.Clear();
            ddljobCategory.ClearSelection();
            GetFieldValues(ControlsEnum.JOBCATEGORY);
            SetFieldValues(ControlsEnum.JOBCATEGORY);
            ddljobCategory.SelectedIndex = 0;
            ddlJobLevel.Items.Clear();
            ddlJobLevel.ClearSelection();
            GetFieldValues(ControlsEnum.JOBLEVEL);
            SetFieldValues(ControlsEnum.JOBLEVEL);
            ddlJobLevel.SelectedIndex = 0;
            ddlJobStream.Items.Clear();
            ddlJobStream.ClearSelection();
            GetFieldValues(ControlsEnum.JOBSTREAM);
            SetFieldValues(ControlsEnum.JOBSTREAM);
            ddlJobStream.SelectedIndex = 0;
            ddlSkillLevel.Items.Clear();
            ddlSkillLevel.ClearSelection();
            GetFieldValues(ControlsEnum.SKILLLEVEL);
            SetFieldValues(ControlsEnum.SKILLLEVEL);
            ddlSkillLevel.SelectedIndex = 0;
            txtPermanentAddress.Text = "";
            txtCommunicationAddress.Text = "";
            txtCity1.Text = "";
            txtCity.Text = "";
            txtState.Text = Resources.ErpRes.AutoDefaultValue;
            txtState1.Text = Resources.ErpRes.AutoDefaultValue;
            hdfState.Value = CommonConstants.SELECTVAL;
            hdfState1.Value = CommonConstants.SELECTVAL; ;
            txtDistrict1.Text = "";
            txtDistrict2.Text = "";
            txtCountry.Text = Resources.ErpRes.AutoDefaultValue;
            txtCountry1.Text = Resources.ErpRes.AutoDefaultValue;
            txtZipCode.Text = "";
            txtZipCode1.Text = "";
            txtPhone.Text = "";
            txtPhone1.Text = "";
            txtreligion.Text = "";
            txtSubReligion.Text = "";
            txtCountryOfBirth.Text = "";
            TxtPlaceOfBirth.Text = "";
            txtProfession.Text = "";
            txtAltContactNO.Text = "";
            TxtFatherName.Text = "";
            txtMotherName.Text = "";
            TxtSpouseName.Text = "";
            txtSpouseSurname.Text = string.Empty;
            txtIncomeTaxNo.Text = string.Empty;
            chkIsSpouseWorking.Checked = false;
            txtreligion.Text = "";
            txtSubReligion.Text = "";
            txtBioMetricId.Text = txtOldCode.Text = string.Empty;
            ddlPayrollType.SelectedIndex = 0;
            txtBasicSalary.Text = string.Empty;

            txtFIdNumber.Text = string.Empty;
            txtMIdNumber.Text = string.Empty;
            txtSIdNumber.Text = string.Empty;
            txtFatherSpouse.Text = string.Empty;
            txtFatherSpouseIdNo.Text = string.Empty;
            txtMothrSpouse.Text = string.Empty;
            txtMothrSpouseIdNo.Text = string.Empty;
            txtNoOfChildrenId.Text = string.Empty;
            txtNoOfChildEduId.Text = string.Empty;
            chkTaxpayer.Checked = false;
            txtNoOfchildrenEdu.Text = string.Empty;
            txtBreakTime.Text = string.Empty;

            chkESIReqrd.Checked = false;
            chkPFRequrd.Checked = false;
            chkSSOReq.Checked = false;

            DesignPK = 0;
        }

        private void DisableControls()
        {
            anchorFile.Visible = false;
            txtEmpCode.Enabled = false;
            ddlSalutaion.Enabled = false;
            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtMiddleName.Enabled = false;
            txtSurname.Enabled = false;
            txtEmpNameLL.Enabled = false;
            ddlMaritalStatus.Enabled = false;
            ddlStatus.Enabled = false;
            txtPersonalEmail.Enabled = false;
            txtNationality.Enabled = false;
            txtDOB.Enabled = false;
            ddlGender.Enabled = false;
            txtMobile.Enabled = false;
            txtDepartment.Enabled = false;
            txtTeam.Enabled = false;
            txtCostCenter.Enabled = false;
            txtDesignation.Enabled = false;
            ddlEmploymentType.Enabled = false;
            ddlReligion.Enabled = false;
            ddlSubReligion.Enabled = false;
            ddlBloodGroup.Enabled = false;
            txtBranchLocation.Enabled = false;
            txtDOJ.Enabled = false;
            ddlCompany.Enabled = false;
            txtConfirmedOn.Enabled = false;
            txtReportTo.Enabled = false;
            txtOfficialEmail.Enabled = false;
            txtOfficialPhone.Enabled = false;
            txtOffPhoExtension.Enabled = false;
            txtOfficialMobile.Enabled = false;
            ddljobCategory.Enabled = false;
            ddlJobLevel.Enabled = false;
            ddlJobStream.Enabled = false;
            ddlSkillLevel.Enabled = false;
            txtPermanentAddress.Enabled = false;
            txtCommunicationAddress.Enabled = false;
            txtCity1.Enabled = false;
            txtCity.Enabled = false;
            txtState.Enabled = false;
            txtState1.Enabled = false;
            txtDistrict1.Enabled = false;
            txtDistrict2.Enabled = false;
            txtCountry.Enabled = false;
            txtCountry1.Enabled = false;
            txtZipCode.Enabled = false;
            txtZipCode1.Enabled = false;
            txtPhone.Enabled = false;
            txtPhone1.Enabled = false;
            txtCountryOfBirth.Enabled = false;
            TxtPlaceOfBirth.Enabled = false;
            txtProfession.Enabled = false;
            txtAltContactNO.Enabled = false;
            TxtFatherName.Enabled = false;
            txtMotherName.Enabled = false;
            TxtSpouseName.Enabled = false;
            txtIncomeTaxNo.Enabled = false;
            txtSpouseSurname.Enabled = false;
            chkIsSpouseWorking.Enabled = false;
            txtNoOfchildren.Enabled = false;
            chkSameAsAbove.Enabled = false;

            txtFIdNumber.Enabled = false;
            txtMIdNumber.Enabled = false;
            txtSIdNumber.Enabled = false;
            txtFatherSpouse.Enabled = false;
            txtFatherSpouseIdNo.Enabled = false;
            txtMothrSpouse.Enabled = false;
            txtMothrSpouseIdNo.Enabled = false;
            txtNoOfChildrenId.Enabled = false;
            txtNoOfChildEduId.Enabled = false;
            chkTaxpayer.Enabled = false;
            txtNoOfchildrenEdu.Enabled = false;
            ddlEmploymentType.Enabled = false;

        }

        private void GetImage()
        {
            try
            {

            }
            catch
            {

            }
        }

        private int? GetNullableInt(string str)
        {
            int result;
            if (int.TryParse(str, out result))
            {
                return (int?)result;
            }
            return null;
        }

        private void ClearPopupControls()
        {
            ddlStatusPopUp.SelectedIndex = 0;
            txtStatusDatePopup.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
            txtReasonPopup.Text = string.Empty;
        }

        private void ClearDepartmentPopupControls()
        {
            txtDepartmentPopup.Text = string.Empty;
            hdfDepartmentPopup.Value = "0";
            txtDatePopup.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
            txtRsnPopup.Text = string.Empty;
        }

        private void ClearDesignationPopupControls()
        {
            txtDesignationPopup.Text = string.Empty;
            hdfDesignationPopup.Value = "0";
            txtDDatePopup.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
            txtDRsnPopup.Text = string.Empty;
        }

        private void ClearBranchPopupControls()
        {
            txtBranchLocationPopup.Text = txtReasonPopupBranch.Text = hdfBranchLocationPopup.Value = "";
            txtDatePopupBranch.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
        }
        private void ClearEmpymntTyePopupControls()
        {
            ddlEmpymntTypePopup.SelectedIndex = 0;
            txtEmpymntTypeDatePopup.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
            txtReasonPopupEmpymntType.Text = string.Empty;
        }

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }

        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }

        private void EmpStatusCheck(int status)
        {
            // if ((status == (int)(EmpStatusEnum.RESIGNED)) || (status == (int)(EmpStatusEnum.TERMINATED)) || (status == (int)(EmpStatusEnum.EXPIRED)))
            if (dtEmpRelStatus.Select().ToList().Exists(row => row["CFG_VALUE"].ToString() == status.ToString()))
            {
                ddlStatus.Enabled = false;
                //btnPopUpAdd.Visible = false;
                ddlStatusPopUp.Visible = false;
                ddlStatusCancelPopUp.Visible = true;
            }
            else
            {
                ddlStatusPopUp.Visible = true;
                ddlStatusCancelPopUp.Visible = false;
            }
        }

        /// <summary>
        /// Configuration Settings
        /// </summary>
        private void SetConfigValue()
        {
            DataTable dtConfig = CommonBL.GetApplicaitonConfiguaration("USER MODULE SETTING", string.Empty, currentUser.CurrentSBUPK);
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                GTIService.Utilities.CryptoServices crypto = new GTIService.Utilities.CryptoServices();
                string retVal = crypto.DecryptString(dtConfig.Rows[0]["ACF_DATA"].ToString(),
                                        ConfigurationManager.AppSettings["SYSKEY"]);
                try
                {
                    if (retVal.Length > 0)
                        EmployeesLimit = Convert.ToInt32((retVal.Split('-')[(int)BusinessObject.Common.UserModuleLimitIndex.Employee]));
                }
                catch (Exception ex)
                {
                }
            }
        }
        #endregion

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
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
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
        }
        #region ControlEnum
        public enum ControlsEnum
        {
            COUNTRY,
            STATE,
            COUNTRY1,
            STATE1,
            COUNTRYBIRTH,
            SELECTSTATE1,
            COMPANY,
            DESIGNATION,
            MARITALSTATUS,
            GENDER,
            EMPLOYMENTTYPE,
            BLOODGROUP,
            SALUTATION,
            EMPLOYEEDETAILSBYID,
            JOBLEVEL,
            JOBCATEGORY,
            SKILLLEVEL,
            JOBSTREAM,
            EMPLOYEEDETAILSHEADER,
            STATUSLIST,
            RELIGION,
            SUBRELIGION,
            DETAILS,
            BRANCHDETAILS,
            PAYMENTMODE,
            BANK,
            //SALARYTEMPLATE,
            EMPLOYEEMENTTYPE,
            OTTEMPLATE,
            LEAVETEMPLATE,
            WORKINGDAYS,
            SELECTEDPEYDETAILS,
            MARITALSTATUSCONST,
            WORKINGDAYSTYPE,
            SELECTEDEMPTYPE,
            WEEKDAYS,
            BANKDETAILS,
            PAYROLLTYPE,
            CURRENCY,
            EMPYMNTTYPEUPDN,
            EMPTOTALCOUNT,
            EMPRELSTATUS,
            EMPRELCANCELSTATUS,
            DEPARTMENTDETAILS,
            DESIGNATIONDETAILS,
            GETJOBDESIGNATIONDETAILS
        }



        #endregion

        #region Employee Status
        public enum EmpStatusEnum
        {
            RESIGNED = 5,
            TERMINATED = 6,
            EXPIRED = 7
        }



        #endregion

    }
}