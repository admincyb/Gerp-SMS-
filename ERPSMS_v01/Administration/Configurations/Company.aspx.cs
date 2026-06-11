using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Administration.Configurations;
using BusinessObject.AccountManagement;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using GTIService;
using GTIService.Constants.Common;
using System.IO;
using System.Data;
using DataAccess.CommonManagement;
using System.Text.RegularExpressions;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class Company : ERP.Store.UI.MyBasePage
    {
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
        /// Save Return Value
        /// </summary>
        private int RetVal
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.RetVal]);
            }
            set
            {
                this.ViewState[ViewstateStrings.RetVal] = value;
            }
        }

        /// <summary>
        /// Deleted File Path
        /// </summary>
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
        /// Output logo file name
        /// </summary>
        private string LogoFileName
        {
            get
            {
                return this.ViewState[ViewstateStrings.LogoFileName] == null ? string.Empty : this.ViewState[ViewstateStrings.LogoFileName].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.LogoFileName] = value;
            }
        }
        /// <summary>
        /// Output logo file path
        /// </summary>
        private string LogoFilePath
        {
            get
            {
                return this.ViewState[ViewstateStrings.LogoFilePath] == null ? string.Empty : this.ViewState[ViewstateStrings.LogoFilePath].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.LogoFilePath] = value;
            }
        }
        /// <summary>
        /// Keep Previous Logo File Path
        /// </summary>
        private string PrevLogoFileName
        {
            get
            {
                return this.ViewState["PrevLogoFileName"] == null ? string.Empty : this.ViewState["PrevLogoFileName"].ToString();
            }
            set
            {
                this.ViewState["PrevLogoFileName"] = value;
            }
        }
        /// <summary>
        /// Keep Previous Output Logo File Path
        /// </summary>
        private string PrevOptLogoFileName
        {
            get
            {
                return this.ViewState["PrevOptLogoFileName"] == null ? string.Empty : this.ViewState["PrevOptLogoFileName"].ToString();
            }
            set
            {
                this.ViewState["PrevOptLogoFileName"] = value;
            }
        }
        #endregion

        private DataTable dtPageData;
        private DataTable dtCompanyDetails;
        private DataTable dtVersionDetails;
        private DataTable dtSBU;
        //Global Private Variables used to maintaion data across methods in the same postback
        private int countryPK = 0;
        private int statePK = 0;
        private int currencyPK = 0;
        private int SBUPK = 0;

        BusinessObject.User currentUser;
        // List variables for binding details to controls
        private ActionsEnum commonActions;
        //Get Or Save Company detais
        private CompanyBO objCompany;

        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum controlType)
        {

            try
            {
                switch (controlType)
                {
                    //Gets the country list to dropdown
                    case ControlsEnum.COUNTRY:
                        dtPageData = CommonDL.GetCountry();
                        break;
                    //Gets the state list to the corresponding country
                    case ControlsEnum.STATE:
                        dtPageData = CommonDL.GetState(Convert.ToInt32(ddlCountry.SelectedValue));
                        break;
                    //Gets the currency list 
                    case ControlsEnum.CURRENCY:
                        dtPageData = CommonDL.GetCurrency(currentUser, currentUser.SBUID);
                        break;
                    //Gets the company details
                    case ControlsEnum.COMPANYDETAILS:
                        dtCompanyDetails = CompanyBL.GetCompanyDetails(CurrPK, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID);
                        break;

                    //Gets the version details 
                    case ControlsEnum.VERSIONDETAILS:
                        dtVersionDetails = CompanyBL.GetVersionDetails();
                        break;

                    //Gets the SBU details
                    case ControlsEnum.SBU:
                        dtSBU = BusinessLogic.Administration.Configurations.SBUConfiguartion.GetMasterSBU(0);
                        break;
                    default:
                        break;
                }
                //UserManagementService.Close();
            }
            catch (Exception ex)
            {
                // UserManagementService.Abort();
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
                    case ControlsEnum.COUNTRY:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.STATE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.CURRENCY:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.COMPANYDETAILS:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.VERSIONDETAILS:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.SBU:
                        BindDropDown(controlType);
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

        #region Helper Methods
        /// <summary>
        /// Bind DropDown  as per type 
        /// </summary>
        /// <param name="drpName"></param>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                // Fill Shift Details to DropDown
                case ControlsEnum.COUNTRY:
                    if (dtPageData != null)
                    {
                        ddlCountry.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, GetLocalResourceObject("CountryName").ToString());
                        ddlCountry.DataTextField = GetLocalResourceObject("CountryName").ToString();
                        ddlCountry.DataValueField = GetLocalResourceObject("CountryPK").ToString();
                        ddlCountry.DataBind();
                    }
                    ddlCountry.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (countryPK > 0 && ddlCountry.Items.FindByValue(countryPK.ToString()) != null)
                        ddlCountry.SelectedValue = countryPK.ToString();
                    break;
                case ControlsEnum.STATE:
                    if (dtPageData != null)
                    {
                        ddlState.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, GetLocalResourceObject("StateName").ToString());
                        ddlState.DataTextField = GetLocalResourceObject("StateName").ToString();
                        ddlState.DataValueField = GetLocalResourceObject("StatePK").ToString();
                        ddlState.DataBind();
                    }
                    //ddlState.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (statePK > 0 && ddlState.Items.FindByValue(statePK.ToString()) != null)
                        ddlState.SelectedValue = statePK.ToString();
                    break;
                case ControlsEnum.CURRENCY:
                    if (dtPageData != null)
                    {
                        ddlCurrency.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, GetLocalResourceObject("CurrencyName").ToString());
                        ddlCurrency.DataTextField = GetLocalResourceObject("CurrencyCode").ToString();
                        ddlCurrency.DataValueField = GetLocalResourceObject("CurrencyPK").ToString();
                        ddlCurrency.DataBind();
                    }
                    ddlCurrency.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (currencyPK > 0 && ddlState.Items.FindByValue(currencyPK.ToString()) != null)
                        ddlCurrency.SelectedValue = currencyPK.ToString();
                    break;
                case ControlsEnum.SBU:
                    if (dtSBU != null)
                    {
                        ddlSBU.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSBU, GetLocalResourceObject("SBUName").ToString());
                        ddlSBU.DataTextField = GetLocalResourceObject("SBUName").ToString();
                        ddlSBU.DataValueField = GetLocalResourceObject("SBUPK").ToString();
                        ddlSBU.DataBind();
                    }
                    ddlSBU.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (SBUPK > 0 && ddlSBU.Items.FindByValue(SBUPK.ToString()) != null)
                        ddlSBU.SelectedValue = SBUPK.ToString();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get Value from Control to object
        /// </summary>
        /// <returns></returns>
        private CompanyBO SetUIValuesToObject()
        {
            //string filePath = string.Empty;
            ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
            CompanyBO objCompany = new CompanyBO();
            objCompany.PK = CurrPK;
            objCompany.CompanyCode = HttpUtility.HtmlEncode(txtCompanyCode.Text);
            objCompany.CompanyName = HttpUtility.HtmlEncode(txtCompanyName.Text);
            objCompany.Description = string.Empty;
            objCompany.Address1 = HttpUtility.HtmlEncode(txtAddress1.Text);
            objCompany.Address2 = HttpUtility.HtmlEncode(txtAddress2.Text);
            objCompany.Address3 = HttpUtility.HtmlEncode(txtLocalLanguageAddress.Text);
            objCompany.City = HttpUtility.HtmlEncode(txtCity.Text);
            objCompany.Country = Convert.ToInt32(ddlCountry.SelectedValue);
            objCompany.State = Convert.ToInt32(ddlState.SelectedValue);
            objCompany.Fax = HttpUtility.HtmlEncode(txtFax.Text);
            objCompany.Phone = HttpUtility.HtmlEncode(txtPhone.Text);
            objCompany.Mobile = HttpUtility.HtmlEncode(txtMobile.Text);
            objCompany.Email = HttpUtility.HtmlEncode(txtEmail.Text);
            objCompany.Currency = Convert.ToInt32(ddlCurrency.SelectedValue);
            objCompany.TaxNo = HttpUtility.HtmlEncode(txtTaxNo.Text);
            objCompany.Website = HttpUtility.HtmlEncode(txtWebsite.Text);
            objCompany.CompanyNameLocal = HttpUtility.HtmlEncode(txtLocalLanguageName.Text);
            objCompany.ZipCode = HttpUtility.HtmlEncode(txtZipCode.Text);
            objCompany.BisRegNo = HttpUtility.HtmlEncode(txtBusinessRegNo.Text);
            objCompany.GSTNo = HttpUtility.HtmlEncode(txtGSTNo.Text);
            objCompany.BizUnit = Convert.ToInt32(ddlSBU.SelectedValue);
            if (txtFinStartDate.Text.Trim() != string.Empty)
            {
                objCompany.FinYearStartDate = Convert.ToDateTime(txtFinStartDate.Text.Trim());//.ToString(Resources.Constants.DateFormatShort);
            }
            if (txtFinEndDate.Text.Trim() != string.Empty)
            {
                objCompany.FinYearEndDate = Convert.ToDateTime(txtFinEndDate.Text.Trim());//.ToString(Resources.Constants.DateFormatShort);
            }

            if (fudLogo.HasFile)
            {
                FileInfo tempFileInfoObj;
                tempFileInfoObj = new FileInfo(fudLogo.PostedFile.FileName);
                string attachmentFileFormat = tempFileInfoObj.Extension;
                //string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                string Logoname = Regex.Replace(txtCompanyCode.Text, "[^a-zA-Z0-9_]+", "");
                FileName = Logoname + "_Logo_" + Guid.NewGuid().ToString() + attachmentFileFormat;
                objCompany.Logo = FileName;
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                {
                    //FilePath = Server.MapPath(GetLocalResourceObject("AttPath").ToString());
                    //if (!Directory.Exists(FilePath))
                    //    Directory.CreateDirectory(FilePath);
                    //FilePath = System.Configuration.ConfigurationManager.AppSettings["CompanyLogo"].ToLower() + FileName;
                    FilePath = "~/Upload/" + FileName;

                }
                else
                {
                    FilePath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + FileName;
                }

            }
            else
            {
                objCompany.Logo = FileName;
            }
            objCompany.LogoURL = FilePath;

            if (fudOutputLogo.HasFile)
            {
                FileInfo tempFileInfoObj;
                tempFileInfoObj = new FileInfo(fudOutputLogo.PostedFile.FileName);
                string attachmentFileFormat = tempFileInfoObj.Extension;
                //string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                string Logoname = Regex.Replace(txtCompanyCode.Text, "[^a-zA-Z0-9_]+", "");
                LogoFileName = Logoname + "_OutputLogo_" + Guid.NewGuid().ToString() + attachmentFileFormat;
                objCompany.OutputLogo = LogoFileName;
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                {
                    LogoFilePath = "~/Upload/" + LogoFileName;
                }
                else
                {
                    LogoFilePath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + LogoFileName;
                }
            }
            else
            {
                objCompany.OutputLogo = LogoFileName;
            }
            objCompany.OutputLogoURL = LogoFilePath;
            objCompany.DisplayCode = HttpUtility.HtmlEncode(txtDisplayCode.Text);
            objCompany.DisplayName = HttpUtility.HtmlEncode(txtDisplayName.Text);
            #region config settings
            objCompany.ProductionIn = Convert.ToInt32(ddlProductionIn.SelectedValue);//1 for Pcs 2 for Weight
            objCompany.NoOfPcsPerBskt = string.IsNullOrEmpty(txtNoOfPcs.Text) ? -1 : Convert.ToInt32(txtNoOfPcs.Text);
            objCompany.WeightPerBskt = string.IsNullOrEmpty(txtWeight.Text) ? -1 : Convert.ToInt32(txtWeight.Text);
            objCompany.NoOfBasket = string.IsNullOrEmpty(txtNoOfBasket.Text) ? -1 : Convert.ToInt32(txtNoOfBasket.Text);
            objCompany.TimeReqToPrd = string.IsNullOrEmpty(txtPrdReqTime.Text) ? -1 : Convert.ToInt32(txtPrdReqTime.Text);
            objCompany.PrintLabel = chbPrintLabel.Checked == true ? 1 : 0;

            #endregion
            #region QA Sample
            objCompany.Medical = Convert.ToInt32(txtMedical.Text);
            objCompany.NonMedical = Convert.ToInt32(txtNonMedical.Text);
            #endregion
            return objCompany;
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
                    case ControlsEnum.COMPANYDETAILS:
                        if (dtCompanyDetails != null && dtCompanyDetails.Rows.Count > 0)
                        {
                            ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
                            txtCompanyCode.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_CODE"].ToString());
                            txtCompanyName.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_NAME"].ToString());
                            txtAddress1.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_ADDR1"].ToString());
                            txtAddress2.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_ADDR2"].ToString());
                            txtLocalLanguageName.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_NAME2"].ToString());
                            txtAddress1.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_ADDR1"].ToString());
                            txtLocalLanguageAddress.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_ADDR3"].ToString());
                            txtCity.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_CITY"].ToString());
                            txtZipCode.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_ZIP"].ToString());
                            ddlCountry.SelectedIndex = Convert.ToInt32(ddlCountry.Items.IndexOf(ddlCountry.Items.FindByValue(dtCompanyDetails.Rows[0]["CMP_CNTRY"].ToString())));
                            GetFieldValues(ControlsEnum.STATE);
                            SetFieldValues(ControlsEnum.STATE);
                            ddlState.SelectedIndex = Convert.ToInt32(ddlState.Items.IndexOf(ddlState.Items.FindByValue(dtCompanyDetails.Rows[0]["CMP_STATE"].ToString())));
                            txtFax.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_FAX"].ToString());
                            txtPhone.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_PHONE"].ToString());
                            txtMobile.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_MOBIL"].ToString());
                            txtEmail.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_EMAIL"].ToString());
                            txtTaxNo.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_TAX_NO"].ToString());
                            txtWebsite.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_WEBSITE"].ToString());
                            txtBusinessRegNo.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_REG_NO"].ToString());
                            txtGSTNo.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_GST_NO"].ToString());
                            if (!string.IsNullOrEmpty(dtCompanyDetails.Rows[0]["CMP_FIN_START_DT"].ToString()))
                            {
                                txtFinStartDate.Text = Convert.ToDateTime(dtCompanyDetails.Rows[0]["CMP_FIN_START_DT"].ToString()).ToString(Resources.Constants.FinYearFormat);
                            }
                            if (!string.IsNullOrEmpty(dtCompanyDetails.Rows[0]["CMP_FIN_END_DT"].ToString()))
                            {
                                txtFinEndDate.Text = Convert.ToDateTime(dtCompanyDetails.Rows[0]["CMP_FIN_END_DT"].ToString()).ToString(Resources.Constants.FinYearFormat);
                            }

                            ddlCurrency.SelectedIndex = Convert.ToInt32(ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(dtCompanyDetails.Rows[0]["CMP_CURRENCY"].ToString())));
                            FileName = dtCompanyDetails.Rows[0]["CMP_LOGO"].ToString();
                            LogoFileName = dtCompanyDetails.Rows[0]["CMP_OP_LOGO"].ToString();
                            anchorFile.Visible = true;
                            anchorFile.InnerHtml = dtCompanyDetails.Rows[0]["CMP_LOGO"].ToString();
                            anchorFile.HRef = dtCompanyDetails.Rows[0]["CMP_LOGO_URL"].ToString();
                            FilePath = dtCompanyDetails.Rows[0]["CMP_LOGO_URL"].ToString();
                            PrevLogoFileName = dtCompanyDetails.Rows[0]["CMP_LOGO"].ToString();// For removing Previous Image
                            anchorLogo.Visible = true;
                            anchorLogo.InnerHtml = dtCompanyDetails.Rows[0]["CMP_OP_LOGO"].ToString();
                            anchorLogo.HRef = dtCompanyDetails.Rows[0]["CMP_OP_LOGO_URL"].ToString();
                            PrevOptLogoFileName = dtCompanyDetails.Rows[0]["CMP_OP_LOGO"].ToString();// For removing Previous Image
                           
                            LogoFilePath = dtCompanyDetails.Rows[0]["CMP_OP_LOGO_URL"].ToString();
                            ddlSBU.SelectedIndex = Convert.ToInt32(ddlSBU.Items.IndexOf(ddlSBU.Items.FindByValue(dtCompanyDetails.Rows[0]["CMP_BIZUNIT"].ToString())));
                            txtDisplayCode.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_DISPLAY_CODE"].ToString());
                            txtDisplayName.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_DISPLAY_NAME"].ToString());
                            #region Config Settings
                            ddlProductionIn.SelectedIndex = Convert.ToInt32(ddlProductionIn.Items.IndexOf(ddlProductionIn.Items.FindByValue(dtCompanyDetails.Rows[0]["CMP_PCS_OR_WGT"].ToString())));
                            txtNoOfPcs.Text=string.IsNullOrEmpty(dtCompanyDetails.Rows[0]["CMP_NO_OF_PCS_PER_BSKT"].ToString()) ? string.Empty: dtCompanyDetails.Rows[0]["CMP_NO_OF_PCS_PER_BSKT"].ToString();
                            txtWeight.Text=string.IsNullOrEmpty(dtCompanyDetails.Rows[0]["CMP_WGT_PER_BSKT"].ToString()) ? string.Empty: dtCompanyDetails.Rows[0]["CMP_WGT_PER_BSKT"].ToString();
                            txtNoOfBasket.Text=string.IsNullOrEmpty(dtCompanyDetails.Rows[0]["CMP_NO_OF_BSKT"].ToString()) ? string.Empty: dtCompanyDetails.Rows[0]["CMP_NO_OF_BSKT"].ToString();
                            txtPrdReqTime.Text=string.IsNullOrEmpty(dtCompanyDetails.Rows[0]["CMP_PRDCTION_REQD_TM"].ToString()) ? string.Empty: dtCompanyDetails.Rows[0]["CMP_PRDCTION_REQD_TM"].ToString();
                            if (!string.IsNullOrEmpty(dtCompanyDetails.Rows[0]["CMP_PRINT_LABEL"].ToString()))
                                chbPrintLabel.Checked = dtCompanyDetails.Rows[0]["CMP_PRINT_LABEL"].ToString() == "1" ? true : false;
                            EnableValidation(ddlProductionIn.SelectedValue);
                            #endregion
                            #region QA Sample
                            txtMedical.Text = dtCompanyDetails.Rows[0]["CMP_MEDICAL"].ToString();
                            txtNonMedical.Text = dtCompanyDetails.Rows[0]["CMP_NON_MEDICAL"].ToString();
                            #endregion
                        }
                        break;
                    case ControlsEnum.VERSIONDETAILS:
                        if (dtVersionDetails != null && dtVersionDetails.Rows.Count > 0)
                        {
                            lblProductName.Text = HttpUtility.HtmlDecode(dtVersionDetails.Rows[0]["SYS_NAME"].ToString());
                            lblVersion.Text = HttpUtility.HtmlDecode(dtVersionDetails.Rows[0]["SYS_VERSION"].ToString());
                            lblGAF.Text = HttpUtility.HtmlDecode(dtVersionDetails.Rows[0]["SYS_GAF_VERSION"].ToString());
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }


        /// <summary>
        /// <summary>
        /// Save signature
        /// </summary>
        private void SaveLogo()
        {
            string fileURL = string.Empty;
            try
            {
                string targetPath = string.Empty;
                if (fudLogo.HasFile)
                {
                    #region Delete Old Image
                    if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                    {
                        fileURL = (System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + PrevLogoFileName);
                        if (File.Exists(fileURL))
                        {
                            File.Delete(fileURL);
                        }
                    }
                    else
                    {
                        fileURL = Server.MapPath("~/Upload/" + PrevLogoFileName);
                        if (File.Exists(fileURL))
                        {
                            File.Delete(fileURL);
                        }
                    }

                    #endregion
                    targetPath = Server.MapPath(FilePath);
                    fudLogo.SaveAs(targetPath);
                }
                if (fudOutputLogo.HasFile)
                {
                    #region Delete Old Image
                    if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                    {
                        fileURL = (System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + PrevOptLogoFileName);
                        if (File.Exists(fileURL))
                        {
                            File.Delete(fileURL);
                        }
                    }
                    else
                    {
                        fileURL = Server.MapPath("~/Upload/" + PrevOptLogoFileName);
                        if (File.Exists(fileURL))
                        {
                            File.Delete(fileURL);
                        }
                    }
                    #endregion
                    targetPath = Server.MapPath(LogoFilePath);
                    fudOutputLogo.SaveAs(targetPath);
                }
            }
            catch
            {

            }
        }
        private void EnableValidation(string val)
        {
            switch (val)
            {
                case "1"://for Pcs 
                   // reqWeight.Enabled = false;
                    reqNoOfPcs.Enabled = true;
                    break;
                case "2":// for weight
                  //  reqWeight.Enabled = true;
                    reqNoOfPcs.Enabled = false;
                    break;
            }
        }
        #endregion

        #region PageActionHandler
        /// <summary>
        /// Method to handle Page Load Action
        /// </summary>
        public void PageActionHandler()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.COUNTRY);
                    SetFieldValues(ControlsEnum.COUNTRY);
                    GetFieldValues(ControlsEnum.CURRENCY);
                    SetFieldValues(ControlsEnum.CURRENCY);
                    GetFieldValues(ControlsEnum.VERSIONDETAILS);
                    SetFieldValues(ControlsEnum.VERSIONDETAILS);
                    GetFieldValues(ControlsEnum.SBU);
                    SetFieldValues(ControlsEnum.SBU);

                    if (Request.QueryString["cmpPK"] != null)
                    {
                        CurrPK = Convert.ToInt32(Request.QueryString["cmpPK"]);
                        btnDelete.Visible = true;
                        GetFieldValues(ControlsEnum.COMPANYDETAILS);
                        SetFieldValues(ControlsEnum.COMPANYDETAILS);
                    }

                    hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";

                    string QASampleConfig = GetGlobalResourceObject("ConfigurationsRes", "QASampleConfig").ToString();
                    divQASample.Visible = false;
                    if (QASampleConfig == "1")
                    {
                        divQASample.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
            finally
            {
            }
        }
        #endregion

        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        protected void ActionHandler(object sender, EventArgs e)
        {
            int result;
            result = 0;

            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlCountry")
                    {
                        commonActions = ActionsEnum.SELECTSTATE;
                    }
                    if (((DropDownList)sender).ID == "ddlProductionIn")
                    {
                        commonActions = ActionsEnum.SELECT;
                    }
                }
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }

                switch (commonActions)
                {
                    case ActionsEnum.SELECT:
                        EnableValidation(ddlProductionIn.SelectedValue);
                        break;
                    #region SELECTSTATE
                    case ActionsEnum.SELECTSTATE:
                        GetFieldValues(ControlsEnum.STATE);
                        SetFieldValues(ControlsEnum.STATE);
                        break;

                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (IsValid)
                        {
                            string routeURL = Resources.PageURL.CompanyList.ToString();
                            objCompany = SetUIValuesToObject();
                            result = CompanyBL.SaveCompany(objCompany, currentUser);
                            if (result > 0) // Success !  redirect to listing page
                            {
                                SaveLogo();
                                PrevOptLogoFileName = string.Empty;
                                PrevLogoFileName = string.Empty;
                                // Show Save Message and redired to listing page
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyDetails);

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CompanyList) + "');", true);
                            }
                            else
                            {
                                // if any error occur, show error details
                                DbSaveStatus saveStatus = (DbSaveStatus)result;
                                switch (saveStatus)
                                {
                                    case DbSaveStatus.SQLERROR://SQl Error
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                        break;
                                    case DbSaveStatus.CONCURRENCY://Cuncurrency Check
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Concurrent;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyDetails.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CompanyList) + "');", true);
                                        break;
                                    case DbSaveStatus.ALREADYDELETED://Cuncurrency Check
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyDetails.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                      + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CompanyList) + "');", true);
                                        break;
                                    case DbSaveStatus.OLDCODEEXIST://Entry already Exists
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Already_Exists;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyDetails.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                      + "','" + Resources.ErpRes.Information + "');", true);
                                        break;

                                    default:
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError; //Other Errors
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                        break;
                                }
                            }
                        }
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        CurrPK = Convert.ToInt32(Request.QueryString["cmpPK"]);
                        result = CompanyBL.DeleteCompany(CurrPK);

                        DbDeleteStatus deleteStatus = (DbDeleteStatus)result;
                        switch (deleteStatus)
                        {
                            case DbDeleteStatus.DELETED://If deletion is success
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyDetails);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CompanyList) + "');", true);
                                break;
                            case DbDeleteStatus.REFERRED://If referred to another page
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyDetails);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CompanyList) + "');", true);
                                break;
                            case DbDeleteStatus.SQLERROR://Sql error
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyDetails);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                 + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CompanyList) + "');", true);
                                break;
                        }
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        Response.Redirect(Resources.PageURL.CompanyList, true);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Captions.Information + "');", true);
            }
            finally
            {

            }
        }

     
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            COUNTRY,
            STATE,
            CURRENCY,
            SAVE,
            CANCEL,
            COMPANYDETAILS,
            DELETEITEM,
            VERSIONDETAILS,
            SBU
        }



        #endregion

        #region Page PreRender
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitDate1", "$(document).ready(function () {InitDate();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideDisplayCode", "$(document).ready(function () {ShowHideDisplayCode();});", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}