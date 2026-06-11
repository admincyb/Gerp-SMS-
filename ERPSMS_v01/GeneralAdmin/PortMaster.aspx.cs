using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic.Administration.Masters;
using BusinessObject.AccountManagement;
using BusinessObject.Administration.Masters;
using BusinessObject.CommonManagement;
using GTIService;
using GTIService.Constants.Common;
using System.IO;
using System.Data;
using DataAccess.CommonManagement;
using System.Text.RegularExpressions;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class PortMaster : ERP.Store.UI.MyBasePage
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
            if (this.GetLocalResourceObject("Breadcrumb") != null)
            {
                string breadCrumb;
                breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                lblBreadCrum.Text = breadCrumb;
                Page.Title = GetLocalResourceObject("Title_Port").ToString();
            }
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
        #endregion

        private DataTable dtPageData;
        private DataTable dtPortDetails;
 
        //Global Private Variables used to maintaion data across methods in the same postback
        private int countryPK = 0;
        private int statePK = 0;
       
        private int TypeValue = 0;
      

        BusinessObject.User currentUser;
        // List variables for binding details to controls
        private ActionsEnum commonActions;
        //Get Or Save Company detais
        private PortMasterBO objPort;

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
                     //Get Config Type
                    case ControlsEnum.TYPE:
                        dtPageData = CommonDA.GetAppConfig(currentUser.CurrentSBUPK, "PORT TYPE",null);
                        break;
                    case ControlsEnum.PORTEDIT:
                        dtPortDetails = BusinessLogic.Administration.Masters.PortMasterBL.GetPortEdit(this.CurrPK, Convert.ToInt32(DbActiveStatus.HASPK));
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
                    case ControlsEnum.COUNTRY:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.STATE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.TYPE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.PORTEDIT:
                        GetUIValuesFromObject(controlType);
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
                    ddlState.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                    ddlState.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (statePK > 0 && ddlState.Items.FindByValue(statePK.ToString()) != null)
                        ddlState.SelectedValue = statePK.ToString();
                    break;
                case ControlsEnum.TYPE:
                    if (dtPageData != null)
                    {
                        ddlType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, GetLocalResourceObject("TypeName").ToString());
                        ddlType.DataTextField = GetLocalResourceObject("TypeName").ToString();
                        ddlType.DataValueField = GetLocalResourceObject("TypeValue").ToString();
                        ddlType.DataBind();
                    }
                    ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (TypeValue > 0 && ddlType.Items.FindByValue(TypeValue.ToString()) != null)
                        ddlType.SelectedValue = TypeValue.ToString();
                    break;

              
                default:
                    break;
            }
        }

        /// <summary>
        /// Get Value from Control to object
        /// </summary>
        /// <returns></returns>
        private PortMasterBO SetUIValuesToObject()
        {
            //string filePath = string.Empty;
            ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
            PortMasterBO objPort = new PortMasterBO();
            objPort.PK = CurrPK;
            objPort.Name = HttpUtility.HtmlEncode(txtPortName.Text);
            objPort.DisplayCode = HttpUtility.HtmlEncode(txtPortCode.Text);
            objPort.Type = Convert.ToInt32(ddlType.SelectedValue);
            objPort.Address = HttpUtility.HtmlEncode(txtAddress.Text);
            objPort.City = HttpUtility.HtmlEncode(txtCity.Text);
            objPort.Country = Convert.ToInt32(ddlCountry.SelectedValue);
            objPort.State = Convert.ToInt32(ddlState.SelectedValue);
            objPort.Fax = HttpUtility.HtmlEncode(txtFax.Text);
            objPort.Phone = HttpUtility.HtmlEncode(txtPhone.Text);
            objPort.Mobile = HttpUtility.HtmlEncode(txtMobile.Text);
            objPort.Email = HttpUtility.HtmlEncode(txtEmail.Text);
            objPort.ZipCode = HttpUtility.HtmlEncode(txtZipCode.Text);
            objPort.IsPurFrom = chkPurFrom.Checked ? 1 : 0;
            objPort.IsPurTo = chkPurTo.Checked ? 1 : 0;
            objPort.IsSalesFrom = chkSalesFrom.Checked ? 1 : 0;
            objPort.IsSalesTo = chkSalesTo.Checked ? 1 : 0;
            objPort.Active = chkActive.Checked ? 1 : 0;
            objPort.PRM_MOD_DT = LastModifiedTime;

            return objPort;
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
                    case ControlsEnum.PORTEDIT:
                        if (dtPortDetails != null && dtPortDetails.Rows.Count > 0)
                        {
                            ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
                            txtPortName.Text = HttpUtility.HtmlDecode(dtPortDetails.Rows[0]["PRM_NAME"].ToString());
                            txtPortCode.Text = HttpUtility.HtmlDecode(dtPortDetails.Rows[0]["PRM_DISPLAY_CODE"].ToString());
                            ddlType.SelectedIndex = Convert.ToInt32(ddlType.Items.IndexOf(ddlType.Items.FindByValue(dtPortDetails.Rows[0]["PRM_TYPE"].ToString())));
                            txtAddress.Text = HttpUtility.HtmlDecode(dtPortDetails.Rows[0]["PRM_ADDRESS"].ToString());
                            txtCity.Text = HttpUtility.HtmlDecode(dtPortDetails.Rows[0]["PRM_CITY"].ToString());
                            ddlCountry.SelectedIndex = Convert.ToInt32(ddlCountry.Items.IndexOf(ddlCountry.Items.FindByValue(dtPortDetails.Rows[0]["PRM_COUNTRY"].ToString())));
                            GetFieldValues(ControlsEnum.STATE);
                            SetFieldValues(ControlsEnum.STATE);
                            ddlState.SelectedIndex = Convert.ToInt32(ddlState.Items.IndexOf(ddlState.Items.FindByValue(dtPortDetails.Rows[0]["PRM_STATE"].ToString())));
                            txtZipCode.Text = HttpUtility.HtmlDecode(dtPortDetails.Rows[0]["PRM_ZIP"].ToString());
                          
                            
                            txtPhone.Text = HttpUtility.HtmlDecode(dtPortDetails.Rows[0]["PRM_PHONE"].ToString());
                            txtMobile.Text = HttpUtility.HtmlDecode(dtPortDetails.Rows[0]["PRM_MOBILE"].ToString());
                            txtFax.Text = HttpUtility.HtmlDecode(dtPortDetails.Rows[0]["PRM_FAX"].ToString());
                            txtEmail.Text = HttpUtility.HtmlDecode(dtPortDetails.Rows[0]["PRM_EMAIL"].ToString());
                            chkSalesFrom.Checked = string.IsNullOrEmpty(dtPortDetails.Rows[0]["PRM_IS_SALES_FROM"].ToString()) ? false : Convert.ToInt32(dtPortDetails.Rows[0]["PRM_IS_SALES_FROM"].ToString()) == 1 ? true : false;
                            chkSalesTo.Checked = string.IsNullOrEmpty(dtPortDetails.Rows[0]["PRM_IS_SALES_TO"].ToString()) ? false : Convert.ToInt32(dtPortDetails.Rows[0]["PRM_IS_SALES_TO"].ToString()) == 1 ? true : false;
                            chkPurFrom.Checked = string.IsNullOrEmpty(dtPortDetails.Rows[0]["PRM_IS_PUR_FROM"].ToString()) ? false : Convert.ToInt32(dtPortDetails.Rows[0]["PRM_IS_PUR_FROM"].ToString()) == 1 ? true : false;
                            chkPurTo.Checked = string.IsNullOrEmpty(dtPortDetails.Rows[0]["PRM_IS_PUR_TO"].ToString()) ? false : Convert.ToInt32(dtPortDetails.Rows[0]["PRM_IS_PUR_TO"].ToString()) == 1 ? true : false;
                            chkActive.Checked = string.IsNullOrEmpty(dtPortDetails.Rows[0]["PRM_ACTIVE"].ToString()) ? false : Convert.ToInt32(dtPortDetails.Rows[0]["PRM_ACTIVE"].ToString()) == 1 ? true : false;

                            LastModifiedTime = Convert.ToDateTime(dtPortDetails.Rows[0]["PRM_MOD_DT"].ToString());
                           
                           
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

                    GetFieldValues(ControlsEnum.TYPE);
                    SetFieldValues(ControlsEnum.TYPE);
                    txtPortCode.Focus();

                    if (Request.QueryString["prmPK"] != null)
                    {
                        CurrPK = Convert.ToInt32(Request.QueryString["prmPK"]);
                        btnDelete.Visible = true;
                        GetFieldValues(ControlsEnum.PORTEDIT);
                        SetFieldValues(ControlsEnum.PORTEDIT);
                    }

                    hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
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
                            //Sate field mandatory for Demo-IN
                            if (ddlState.SelectedValue == CommonConstants.SELECTVAL && GetGlobalResourceObject("ConfigurationsRes", "IsEnableStateValidation").ToString() == CommonConstants.SELECT_VALUE_ONE)
                            {
                                vrfState.Enabled = true;
                                //vrfState.ErrorMessage = GetLocalResourceObject("Err_State").ToString();
                                vrfState.Validate();
                                break;
                            }
                            objPort = SetUIValuesToObject();
                            result = PortMasterBL.SavePort(objPort, currentUser,currentUser.SBUID);
                            if (result > 0) // Success !  redirect to listing page
                            {
                                
                                // Show Save Message and redired to listing page
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails);

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PortList) + "');", true);
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
                                    case DbSaveStatus.CODEEXIST: //Code Exists   
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Name_Already_Exists;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PortList) + "');", true);
                                        break;
                                    case DbSaveStatus.CONCURRENCY://Cuncurrency Check
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Error_Concurrent;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                   + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PortList) + "');", true);
                                        break;
                                    case DbSaveStatus.ALREADYDELETED://Cuncurrency Check
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                      + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PortList) + "');", true);
                                        break;
                                    case DbSaveStatus.OLDCODEEXIST://Entry already Exists
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Already_Exists;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails.ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                      + "','" + Resources.ErpRes.Information + "');", true);
                                        break;

                                    case DbSaveStatus.INCORRECT://Entry already Exists
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Code_Already_Exists;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails.ToString());
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
                        CurrPK = Convert.ToInt32(Request.QueryString["prmPK"]);
                        result = PortMasterBL.DeletePort(CurrPK);

                        DbDeleteStatus deleteStatus = (DbDeleteStatus)result;
                        switch (deleteStatus)
                        {
                            case DbDeleteStatus.DELETED://If deletion is success
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PortList) + "');", true);
                                break;
                            case DbDeleteStatus.REFERRED://If referred to another page
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PortList) + "');", true);
                                break;
                            case DbDeleteStatus.SQLERROR://Sql error
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                 + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PortList) + "');", true);
                                break;
                        }
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        
                        Response.Redirect(Resources.PageURL.PortList, true);
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
            SBU,
            TYPE,
            PORTEDIT
        }



        #endregion
    }
}