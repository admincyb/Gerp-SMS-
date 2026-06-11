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

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class CompanyDemo : ERP.Store.UI.MyBasePage
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
        #endregion

        private DataTable dtPageData;
        private DataTable dtCompanyDetails;
        //Global Private Variables used to maintaion data across methods in the same postback
        private int countryPK;
        private int statePK;
        private int currencyPK;

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
                        GetUIValuesFromObject();
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
                            ddlCountry.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CNT_NAME");
                            ddlCountry.DataTextField = "CNT_NAME";
                            ddlCountry.DataValueField = "CNT_PK";
                            ddlCountry.DataBind();
                        }
                        ddlCountry.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (countryPK > 0 && ddlCountry.Items.FindByValue(countryPK.ToString()) != null)
                            ddlCountry.SelectedValue = countryPK.ToString();
                    break;
                case ControlsEnum.STATE:
                        if (dtPageData != null)
                        {
                            ddlState.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "STT_NAME");
                            ddlState.DataTextField = "STT_NAME";
                            ddlState.DataValueField = "STT_PK";
                            ddlState.DataBind();
                        }
                     //ddlState.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (statePK > 0 && ddlState.Items.FindByValue(statePK.ToString()) != null)
                            ddlState.SelectedValue = statePK.ToString();
                    break;
                case ControlsEnum.CURRENCY:
                    if (dtPageData != null)
                    {
                        ddlCurrency.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CUR_NAME");
                        ddlCurrency.DataTextField = "CUR_NAME";
                        ddlCurrency.DataValueField = "CUR_PK";
                        ddlCurrency.DataBind();
                    }
                    ddlCurrency.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (currencyPK > 0 && ddlState.Items.FindByValue(currencyPK.ToString()) != null)
                        ddlCurrency.SelectedValue = currencyPK.ToString();
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
            if (fudLogo.HasFile)
            {
                FileInfo tempFileInfoObj;
                tempFileInfoObj = new FileInfo(fudLogo.PostedFile.FileName);
                string attachmentFileFormat = tempFileInfoObj.Extension;
                //string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                FileName = HttpUtility.HtmlEncode(txtCompanyName.Text) + "_Logo" + attachmentFileFormat;
                objCompany.Logo = FileName;
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                {
                    FilePath = Server.MapPath(GetLocalResourceObject("AttachPath").ToString());
                }
                else
                {
                    //SavePath = Server.MapPath("../Upload");
                    FilePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                }

            }
            else
            {
                objCompany.Logo = FileName;
            }
            objCompany.LogoURL = FilePath;
            return objCompany;
        }
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            try
            {
                if (dtCompanyDetails != null && dtCompanyDetails.Rows.Count > 0)
                {
                    ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
                    txtCompanyCode.Text=HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_CODE"].ToString());
                    txtCompanyName.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_NAME"].ToString());
                    txtAddress1.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_ADDR1"].ToString());
                    txtAddress2.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_ADDR2"].ToString());
                    txtLocalLanguageName.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_NAME2"].ToString());
                    txtAddress1.Text = HttpUtility.HtmlDecode(dtCompanyDetails.Rows[0]["CMP_ADDR1"].ToString());
                    txtLocalLanguageAddress.Text = dtCompanyDetails.Rows[0]["CMP_ADDR3"].ToString();
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
                    ddlCurrency.SelectedIndex = Convert.ToInt32(ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(dtCompanyDetails.Rows[0]["CMP_CURRENCY"].ToString())));
                    lblLogoName.Text = dtCompanyDetails.Rows[0]["CMP_LOGO"].ToString();
                    FileName = dtCompanyDetails.Rows[0]["CMP_LOGO"].ToString();
                    FilePath = dtCompanyDetails.Rows[0]["CMP_LOGO_URL"].ToString();

                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        //private List<WkfUserGroupMpg> SetUIValuesToObject()
        //{
        //    try
        //    {
        //        wkfUserGroupMpgList = new List<WkfUserGroupMpg>();
        //        foreach (TreeNode root in trvGroup.Nodes)
        //        {
        //            foreach (TreeNode node in root.ChildNodes)
        //                foreach (TreeNode child in node.ChildNodes)
        //                {
        //                    if (child.Checked)
        //                    {
        //                        wkfUserGroupMpgObj = new WkfUserGroupMpg();
        //                        wkfUserGroupMpgObj.gumGroup = Convert.ToInt16(child.Value);
        //                        wkfUserGroupMpgObj.gumUser = CurrPK;
        //                        wkfUserGroupMpgList.Add(wkfUserGroupMpgObj);
        //                    }
        //                }
        //        }

        //        //foreach (TreeNode node in trvGroup.CheckedNodes)
        //        //{
        //        //    wkfUserGroupMpgObj = new WkfUserGroupMpg();
        //        //    wkfUserGroupMpgObj.gumGroup = Convert.ToInt16(node.Value);
        //        //    wkfUserGroupMpgObj.gumUser = CurrPK;
        //        //    wkfUserGroupMpgList.Add(wkfUserGroupMpgObj);
        //        //}
        //        if (wkfUserGroupMpgList.Count == 0)
        //        {
        //            // if to save null group
        //            //pdrgrsuObj = new WkfUserGroupMpg();
        //            //pdrgrsuObj.gumGroup = -1;
        //            //pdrgrsuObj.gumUser = CurrPK;
        //            //pdrgrsuList.Add(pdrgrsuObj);

        //            //don't allow null group
        //            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Group").ToString();
        //            throw new Exception(litErrorMsg.Text);
        //        }
        //        return wkfUserGroupMpgList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        wkfUserGroupMpgList = null;
        //        wkfUserGroupMpgObj = null;
        //    }
        //}

        /// <summary>
        /// <summary>
        /// Resets the form for a fresh entry
        /// </summary>
        //private void ResetForm()
        //{
        //    //Codes for Clearing the controls in the page
        //    CurrPK = 0;
        //    txtUserName.Text = string.Empty;
        //    txtPassword.Text = string.Empty;
        //    txtEmail.Text = string.Empty;
        //    ddlEmployee.SelectedIndex = -1;
        //    ddlStatus.SelectedIndex = -1;
        //}

        /// <summary>
        /// <summary>
        /// Save signature
        /// </summary>
        private bool SaveLogo()
        {
            try
            {
                //string fileName = string.Empty;
                string targetPath = string.Empty;
                //string targetPath = Server.MapPath(GetLocalResourceObject("AttachPath").ToString());
                if (fudLogo.HasFile)
                {
                    //fileName = txtCompanyName.Text + "_Logo" + Path.GetExtension(fudLogo.PostedFile.FileName);
                    targetPath = FilePath + FileName;
                    fudLogo.SaveAs(targetPath);
                    return true;
                }
            }
            catch
            { 
            
            }
            return false;
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

                    if (Request.QueryString["cmpPK"] != null)
                    {
                        CurrPK = Convert.ToInt32(Request.QueryString["cmpPK"]);
                        btnDelete.Visible = true;
                        GetFieldValues(ControlsEnum.COMPANYDETAILS);
                        SetFieldValues(ControlsEnum.COMPANYDETAILS);
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
                }
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }

                switch (commonActions)
                {
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
                        result=CompanyBL.DeleteCompany(CurrPK);

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
            DELETEITEM
        }
        #endregion
        
    }
}