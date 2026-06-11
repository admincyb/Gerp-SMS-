using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERPSMS_v01.UserControls;
using System.Data;
using BusinessObject.Administration.Masters;
using BusinessLogic.Administration.Masters;
using System.IO;

namespace ERPSMS_v01.Administration.Masters
{
    public partial class CompanyMaster : ERP.Store.UI.MyBasePage
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
        /// Deleted File Path
        /// </summary>
        private string FilePath
        {
            get
            {
                return this.ViewState[GTIService.Constants.Common.ViewstateStrings.FilePath] == null ? string.Empty : this.ViewState[GTIService.Constants.Common.ViewstateStrings.FilePath].ToString();
            }
            set
            {
                this.ViewState[GTIService.Constants.Common.ViewstateStrings.FilePath] = value;
            }
        }
        #endregion

        private ActionsEnum commonActions;
        private List<ADM_CONST_MST> admConstMstList;
        private DataTable dtControlsData;
        private BusinessObject.User currentUser;
        private CommonService CommonServiceClient;
        private DataSet dsPageData;
        private CompanyMasterBO CompanyMasterBO;
        private DataSet dsCountryList;
        private DataSet dsCurrency;

        #endregion

        #region PageLevel Events
        /// <summary>
        /// page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
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
            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        SortBy = SortBy == null ? Resources.DataFieldRes.CompanyMstPK : SortBy;
                        SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        dsPageData = BusinessLogic.Administration.Masters.CompanyMasterBL.GetCompanyMaster(CurrPK, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID);
                            //ddlFilterBy.SelectedItem.Text == Resources.Controls.PackingSpecification ? txtSearchBy.Text : string.Empty,
                            //ddlFilterBy.SelectedItem.Text == Resources.Controls.PackingType ? txtSearchBy.Text : string.Empty);
                        //Session[ERP.Utilities.SessionStrings.PackingPk] = null;
                        //Session[ERP.Utilities.SessionStrings.PackingMode] = null;
                        break;
                    case ControlsEnum.COUNTRYLIST:
                        dsCountryList=BusinessLogic.Administration.Masters.CompanyMasterBL.GetCountryList();
                        break;

                    case ControlsEnum.CURRENCY:
                        dsCurrency = BusinessLogic.Administration.Masters.CompanyMasterBL.GetCurrency(Convert.ToInt32(DbActiveStatus.ACTIVE));
                        break;

                    case ControlsEnum.COMPANY:
                        dsPageData = BusinessLogic.Administration.Masters.CompanyMasterBL.GetCompanyMaster(CurrPK, Convert.ToInt16(DbActiveStatus.HASPK), currentUser.SBUID);
                        break;
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
                    case ControlsEnum.COMPANY:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.COUNTRYLIST:
                        BindCountryDropDown();
                        break;
                    case ControlsEnum.CURRENCY:
                        BindCurrencyDropDown();
                        break;
                    //case ControlsEnum.CONTROLS:
                    //    SetControls();
                    //    break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods

        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            try
            {
                switch (mode)
                {
                    #region Save
                    case ActionsEnum.SAVE:
                        // case ActionsEnum.WRKFSUBMIT:
                        CompanyMasterBO = new CompanyMasterBO();
                        CompanyMasterBO.P_CMP_PK = CurrPK;
                        CompanyMasterBO.P_CMP_CODE = txtCompanyCode.Text;
                        CompanyMasterBO.P_CMP_NAME = txtCompanyName.Text;
                        CompanyMasterBO.P_CMP_DESC = string.Empty;
                        CompanyMasterBO.P_CMP_ADDR1 = txtCompanyAddress1.Text;
                        CompanyMasterBO.P_CMP_ADDR2 = txtCompanyAddress2.Text;
                        CompanyMasterBO.P_CMP_CITY = txtCompanyCity.Text;
                        CompanyMasterBO.P_CMP_PHONE = txtCompanyPhone.Text;
                        CompanyMasterBO.P_CMP_MOBIL = txtCompanyMobile.Text;
                        CompanyMasterBO.P_CMP_FAX = txtCompanyFax.Text;
                        CompanyMasterBO.P_CMP_EMAIL = txtCompanyEmail.Text;
                        CompanyMasterBO.P_CMP_STATE = 0;
                        CompanyMasterBO.P_CMP_CNTRY = ddlCompanyCountry.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlCompanyCountry.SelectedValue) : 0;
                        CompanyMasterBO.P_CMP_CURRENCY =ddlCurrency.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlCurrency.SelectedValue) : 0;
                        CompanyMasterBO.P_CMP_TAX_NO = txtCompanyTaxNo.Text;
                        //CompanyMasterBO.P_CMP_LOGO = fudLogo.PostedFile.FileName;
                        //CompanyMasterBO.P_CMP_LOGO = string.Empty;



                        CompanyMasterBO.P_ACTIVE = Convert.ToInt16(DbActiveStatus.ACTIVE);
                        CompanyMasterBO.P_USER_PK = currentUser.PKUser;
                        CompanyMasterBO.P_BIZUNIT = currentUser.SBUID;
                        CompanyMasterBO.P_LAST_MOD_DT = LastModifiedTime;

                        string fileName = string.Empty;
                        if (fudLogo.HasFile)
                            fileName = txtCompanyCode.Text + "_Logo" + Path.GetExtension(fudLogo.PostedFile.FileName);
                        else if (lblSignatureName.Text != string.Empty && (lblSignatureName.Text.Substring(0, lblSignatureName.Text.IndexOf('_')) != txtCompanyCode.Text))
                            fileName = lblSignatureName.Text.Replace(lblSignatureName.Text.Substring(0, lblSignatureName.Text.IndexOf('_')), txtCompanyCode.Text);
                        else
                            fileName = lblSignatureName.Text;
                        CompanyMasterBO.P_CMP_LOGO = fileName;


                        returnObj = CompanyMasterBO;
                        break;
                    #endregion
                }
                return returnObj;
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
                if (dsPageData != null && dsPageData.Tables[0].Rows.Count > 0)
                {
                    CurrPK = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["CMP_PK"].ToString());
                    txtCompanyCode.Text = dsPageData.Tables[0].Rows[0]["CMP_CODE"].ToString();
                    txtCompanyName.Text = dsPageData.Tables[0].Rows[0]["CMP_NAME"].ToString();

                    txtCompanyAddress1.Text = dsPageData.Tables[0].Rows[0]["CMP_ADDR1"].ToString();
                    txtCompanyAddress2.Text = dsPageData.Tables[0].Rows[0]["CMP_ADDR2"].ToString();
                    txtCompanyCity.Text = dsPageData.Tables[0].Rows[0]["CMP_CITY"].ToString();
                    txtCompanyPhone.Text = dsPageData.Tables[0].Rows[0]["CMP_PHONE"].ToString();
                    txtCompanyMobile.Text = dsPageData.Tables[0].Rows[0]["CMP_MOBIL"].ToString();
                    txtCompanyFax.Text = dsPageData.Tables[0].Rows[0]["CMP_FAX"].ToString();
                    txtCompanyEmail.Text = dsPageData.Tables[0].Rows[0]["CMP_EMAIL"].ToString();
                    txtCompanyTaxNo.Text = dsPageData.Tables[0].Rows[0]["CMP_TAX_NO"].ToString();
                    //ddlCompanyCountry.SelectedValue = dsPageData.Tables[0].Rows[0]["CMP_CNTRY"].ToString();
                    ddlCompanyCountry.SelectedIndex = Convert.ToInt32(ddlCompanyCountry.Items.IndexOf(ddlCompanyCountry.Items.FindByValue(dsPageData.Tables[0].Rows[0]["CMP_CNTRY"].ToString())));
                    //ddlCurrency.SelectedValue = dsPageData.Tables[0].Rows[0]["CMP_CURRENCY"].ToString();
                    ddlCurrency.SelectedIndex = Convert.ToInt32(ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(dsPageData.Tables[0].Rows[0]["CMP_CURRENCY"].ToString())));
                    LastModifiedTime = Convert.ToDateTime(dsPageData.Tables[0].Rows[0]["CMP_MOD_DT"].ToString());
                    lblSignatureName.Text = dsPageData.Tables[0].Rows[0]["CMP_LOGO"].ToString();
                    lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                }
                else
                {
                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Packing);
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
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.DEFAULT:
                        if (dsPageData != null)
                        {
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            grdCompanyMst.PageIndex = Convert.ToInt32(PageIndex);
                            dsPageData.Tables[0].DefaultView.Sort = SortBy + " " + SortDirection;
                            grdCompanyMst.DataSource = dsPageData.Tables[0].DefaultView;
                            grdCompanyMst.DataBind();
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
        /// Method for Country DropDown
        /// </summary>
        public void BindCountryDropDown()
        {
            ddlCompanyCountry.Items.Clear();
            if (dsCountryList != null && dsCountryList.Tables[0].Rows.Count > 0)
            {
                ddlCompanyCountry.DataSource = dsCountryList.Tables[0];
                ddlCompanyCountry.DataTextField = Resources.DataFieldRes.CountryName;
                ddlCompanyCountry.DataValueField = Resources.DataFieldRes.CountryPk;
                ddlCompanyCountry.DataBind();
            }
            ddlCompanyCountry.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));
        }
        /// <summary>
        /// Method for Currency DropDown
        /// </summary>
        public void BindCurrencyDropDown()
        {
            ddlCurrency.Items.Clear();
            if (dsCurrency != null && dsCurrency.Tables[0].Rows.Count > 0)
            {
                ddlCurrency.DataSource = dsCurrency.Tables[0];
                ddlCurrency.DataTextField = Resources.DataFieldRes.CurrencyName;
                ddlCurrency.DataValueField = Resources.DataFieldRes.CurrencyPK;
                ddlCurrency.DataBind();
            }
            ddlCurrency.Items.Insert(0, new ListItem(Resources.Captions.SelectText, ERP.Utilities.CommonConstants.SELECTVAL));
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdCompanyMst.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt16(grdCompanyMst.DataKeys[grdrow.RowIndex].Values[0]);
                        GetFieldValues(ControlsEnum.COUNTRYLIST);
                        SetFieldValues(ControlsEnum.COUNTRYLIST);
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);
                        //GetFieldValues(ControlsEnum.CONTROLS);
                        //SetFieldValues(ControlsEnum.CONTROLS);
                        txtCompanyCode.Focus();
                        ModifiedDatePnl.Visible = true;
                        if (Mode == ActionsEnum.VIEW)
                            EntryStatus = EntryStatus.VIEWMODE;
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;
                        return;
                    }
                }

                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Messages.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                EntryStatus = EntryStatus.LISTMODE;
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
            txtCompanyAddress1.Text = string.Empty;
            txtCompanyAddress2.Text = string.Empty;
            txtCompanyCity.Text = string.Empty;
            txtCompanyCode.Text = string.Empty;
            txtCompanyEmail.Text = string.Empty;
            txtCompanyFax.Text = string.Empty;
            txtCompanyMobile.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            txtCompanyPhone.Text = string.Empty;
            txtCompanyTaxNo.Text = string.Empty;
            ddlCompanyCountry.SelectedIndex = 0;
            ddlCurrency.SelectedIndex = 0;
            ddlFilterBy.SelectedIndex = 0;
            PageIndex = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
            lblLastModifiedHDR.Text = string.Empty;
            ModifiedDatePnl.Visible = false;
        }

        /// <summary>
        /// <summary>
        /// Save signature
        /// </summary>
        private bool SaveSignature()
        {
            try
            {
                string fileName = string.Empty;
                string targetPath = string.Empty;
                //string targetPath = Server.MapPath(GetLocalResourceObject("AttachPath").ToString());
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                {
                    targetPath = Server.MapPath(GetLocalResourceObject("AttachPath").ToString());
                    if (!Directory.Exists(targetPath))
                        Directory.CreateDirectory(targetPath);
                }
                else
                {
                    //SavePath = Server.MapPath("../Upload");
                    targetPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                }

                if (fudLogo.HasFile)
                {
                    fileName = txtCompanyCode.Text + "_Logo" + Path.GetExtension(fudLogo.PostedFile.FileName);
                    targetPath = targetPath + fileName;
                    fudLogo.SaveAs(targetPath);

                    if (lblSignatureName.Text != string.Empty && (lblSignatureName.Text.Substring(0, lblSignatureName.Text.IndexOf('_')) != txtCompanyCode.Text))
                    {
                        targetPath = targetPath.Replace(fileName, lblSignatureName.Text);
                        if (File.Exists(targetPath))
                            File.Delete(targetPath);
                    }
                }
                else
                {
                    if (lblSignatureName.Text != string.Empty && (lblSignatureName.Text.Substring(0, lblSignatureName.Text.IndexOf('_')) != txtCompanyCode.Text))
                    {
                        targetPath = targetPath + lblSignatureName.Text;
                        if (File.Exists(targetPath))
                            File.Move(targetPath, targetPath.Replace(lblSignatureName.Text.Substring(0, lblSignatureName.Text.IndexOf('_')), txtCompanyCode.Text));
                    }
                }
                return true;
            }
            catch
            {
                litErrorMsg.Text = this.GetLocalResourceObject("Err_SaveSignature").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                return false;
            }
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
                int? result;
                string file = "";
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                switch (commonActions)
                {
                    #region Save
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            CompanyMasterBO = (CompanyMasterBO)SetUIValuesToObject(commonActions);
                            if (CompanyMasterBO != null)
                            {
                                result = CompanyMasterBL.SaveCompanyMaster(CompanyMasterBO);
                                if (result >= 0) // Success ! re-initialize the page
                                {

                                    if (!string.IsNullOrEmpty(FilePath))
                                    {
                                        lblSignatureName.Text = FilePath;
                                        //string file = Server.MapPath(GetLocalResourceObject("AttachPath").ToString()) + FilePath;
                                        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                        {
                                            file = Server.MapPath(GetLocalResourceObject("AttachPath").ToString()) + FilePath;
                                        }
                                        else
                                        {
                                            //SavePath = Server.MapPath("../Upload");
                                            file = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + FilePath;
                                        }

                                        if (File.Exists(file))
                                            File.Delete(file);
                                    }
                                    // Save Signature to folder
                                    if (!SaveSignature())
                                        return;

                                    SortBy = Resources.DataFieldRes.CompanyMstPK;
                                    SortDirection = Resources.Report.SortDescending;
                                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CompanyMaster);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                    btnNew.Focus();
                                }
                                //Duplicate records
                                //Packing spec duplicates
                                else
                                {
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.CompanyMaster + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl("~/Administration/Masters/CompanyMaster.aspx") + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.CompanyMaster + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl("~/Administration/Masters/CompanyMaster.aspx") + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.DATEOVERLAP)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.CompanyMaster + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl("~/Administration/Masters/CompanyMaster.aspx") + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyMaster);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = BusinessLogic.Administration.Masters.CompanyMasterBL.DeleteCompanyMaster(CurrPK, LastModifiedTime);
                            if (result > 0)
                            {

                                FilePath = lblSignatureName.Text;
                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                {
                                    file = Server.MapPath(GetLocalResourceObject("AttachPath").ToString()) + FilePath;
                                }
                                else
                                {
                                    //SavePath = Server.MapPath("../Upload");
                                    file = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + FilePath;
                                }

                                if (File.Exists(file))
                                    File.Delete(file);
                                lblSignatureName.Text = string.Empty;

                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                ResetForm();
                                btnNew.Focus();
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.CompanyMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.CompanyMaster + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl("~/Administration/Masters/CompanyMaster.aspx") + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.CompanyMaster + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.CompanyMaster + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl("~/Administration/Masters/CompanyMaster.aspx") + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyMaster);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region New
                    case ActionsEnum.NEW:
                        ModifiedDatePnl.Visible = false;
                        //this.txtPackCode.Focus();
                        GetFieldValues(ControlsEnum.COUNTRYLIST);
                        SetFieldValues(ControlsEnum.COUNTRYLIST);
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        //GetFieldValues(ControlsEnum.CONTROLS);
                        //SetFieldValues(ControlsEnum.CONTROLS);
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion

                    //#region Search
                    //case ActionsEnum.SEARCH:
                    //    btnSearch.Focus();
                    //    PageIndex = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                    //    GetFieldValues(ControlsEnum.DEFAULT);
                    //    SetFieldValues(ControlsEnum.DEFAULT);
                    //    EntryStatus = EntryStatus.LISTMODE;
                    //    break;
                    //#endregion

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

                    #region View
                    case ActionsEnum.VIEW:
                        SetUIEditView(commonActions);
                        btnCancel.Focus();
                        break;
                    #endregion

                    #region View
                    case ActionsEnum.ACTIVATE:
                        SetUIEditView(commonActions);
                        break;
                    #endregion

                    //#region ShowDetails
                    //case ActionsEnum.SHOWDETAILS:
                    //    GridViewRow row = ((RadioButton)sender).Parent.Parent as GridViewRow;
                    //    int PackingPK = Convert.ToInt32(((HiddenField)row.FindControl("hfPackingPK")).Value);
                    //    if (PackingPK != null)
                    //    {
                    //        Session[ERP.Utilities.SessionStrings.PackingPk] = PackingPK;
                    //    }

                    //    break;

                    //#endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("PackingSpecDuplicate").ToString()))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.PackingSpecs)) + "','" + Resources.Messages.Information + "');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }

        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
            EntryStatus = EntryStatus.LISTMODE;

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
                this.PageIndex = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }


        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }


        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
        private void SetControls()
        {
            if (dtControlsData != null)
            {
                //if (dtControlsData.Rows.Count > 0)
                //{
                //    lblPouchpcs.Text = dtControlsData.Rows[0][Resources.DataFieldRes.ConstName].ToString();

                //    if (dtControlsData.Rows.Count > 1)
                //    {
                //        lblInnerBox.Text = dtControlsData.Rows[1][Resources.DataFieldRes.ConstName].ToString();
                //    }
                //    if (dtControlsData.Rows.Count > 2)
                //    {
                //        lblInnerCarton.Text = dtControlsData.Rows[2][Resources.DataFieldRes.ConstName].ToString();
                //    }
                //    if (dtControlsData.Rows.Count > 3)
                //    {
                //        lblZipperBag.Text = dtControlsData.Rows[3][Resources.DataFieldRes.ConstName].ToString();
                //    }
                //    if (dtControlsData.Rows.Count > 4)
                //    {
                //        lblMasterCarton.Text = dtControlsData.Rows[4][Resources.DataFieldRes.ConstName].ToString();
                //    }
                //    if (dtControlsData.Rows.Count > 5)
                //    {
                //        lblSack.Text = dtControlsData.Rows[5][Resources.DataFieldRes.ConstName].ToString();
                //    }
                //}
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

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            string breadCrumb;
            breadCrumb = this.GetLocalResourceObject("BreadcrumbCompanyCreation").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);


            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
                breadCrumb = this.GetLocalResourceObject("BreadcrumbCompanyList").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
            }
            lblBreadCrum.Text = breadCrumb;
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


                    //////this.btnNew.Focus();
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.CompanyMstPK;
                    grdCompanyMst.DataKeyNames = datakeyarray;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            COMPANY,
            CONTROLS,
            COUNTRYLIST,
            CURRENCY
        }
        #endregion


    }
}