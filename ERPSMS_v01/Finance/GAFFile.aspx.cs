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
using ERPManager;
using System.IO;
using DataAccess.Finance;
using BusinessLogic.Finance;
using BusinessObject.Finance;
using System.Data;
using BusinessObject;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.Finance
{
    public partial class GAFFile : System.Web.UI.Page
    {
        //private Fields
        #region Private Fields
        private ActionsEnum commonActions;
        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private ServiceUtility serviceUtilityObj;
        private GafFileViewModel gafViewModel = new GafFileViewModel();
        private GstFileTypes selectedGstFileType = GstFileTypes.NONE;
        DataTable dtCurrentCompany;
        User currentUser;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
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
                //Get Action from CommandName
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region Generate
                    case ActionsEnum.GENERATE:
                        string fileType = string.Empty;

                        if (((Button)sender).ID == "btnView")
                            fileType = ".xml";
                        else if (((Button)sender).ID == "btnViewText")
                            fileType = ".txt";
                        // call XML file Generation Modules From Here
                        SetUIValuesToObject(ActionsEnum.VIEW, gafViewModel);

                        #region Set fileName Based On Selected radioButton
                        string fileName = string.Empty;
                        switch (this.selectedGstFileType)
                        {
                            case GstFileTypes.NONE:
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + "Select a File Type" + "','" + Resources.Messages.Information + "');", true);
                                return;
                            case GstFileTypes.ALL:
                                fileName = string.Format("All_{0}" + fileType, DateTime.Now.Ticks);// Generate a Logic For FileName 
                                break;
                            case GstFileTypes.COMPANYINFO:
                                fileName = string.Format("CompanyInfo_{0}" + fileType, DateTime.Now.Ticks);
                                break;
                            case GstFileTypes.PURCHASE:
                                fileName = string.Format("Purchase_{0}" + fileType, DateTime.Now.Ticks);
                                break;
                            case GstFileTypes.SUPPLY:
                                fileName = string.Format("Supply_{0}" + fileType, DateTime.Now.Ticks);
                                break;
                            case GstFileTypes.GENERALLEDGER:
                                fileName = string.Format("GeneralLedger_{0}" + fileType, DateTime.Now.Ticks);
                                break;
                        }
                        #endregion

                        #region Get Upload Directory Path
                        string savePath = string.Empty;
                        if (CommonFunctions.IsNullOrEmptyOrWhitespace(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                        {
                            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                            if (!Directory.Exists(savePath))
                                Directory.CreateDirectory(savePath);
                            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                        }
                        else
                        {
                            savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                        }
                        #endregion
                        string _xmlString;

                        #region Create XML File To Upload Directory
                        string fileNameWithPath = string.Format(@"{0}{1}", savePath, fileName);
                        if (fileType == ".xml")
                        {
                            _xmlString = GstFileManagerBL.GetGSTFileAsXML(gafViewModel);
                        }
                        #endregion
                        else
                        {
                            _xmlString = GstFileManagerBL.GetGSTFileAsPipe(gafViewModel);

                        }

                        if (CommonFunctions.IsNullOrEmptyOrWhitespace(_xmlString))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + "No Data Returned" + "','" + Resources.Messages.Information + "');", true);
                            return;
                        }

                        File.WriteAllText(fileNameWithPath, _xmlString);
                        string xmlUrl = string.Empty;

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + fileNameWithPath +"');", true);
                        if (CommonFunctions.IsNullOrEmptyOrWhitespace(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                        {
                            xmlUrl = "~/Upload/" + fileName;
                        }
                        else
                        {
                            xmlUrl = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + fileName;
                        }
                        xmlUrl = Page.ResolveUrl(xmlUrl);
                        ClientScript.RegisterStartupScript(GetType(), "OpenXml", "window.open('" + xmlUrl + "','','width=600px, height=400px, toolbar=no, scrollbars=yes');", true);
                        break;
                    #endregion
                    case ActionsEnum.CANCEL:
                        Response.Redirect("~/AccountManagement/WorkflowInbox.aspx");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
        /// 
        private void GetFieldValues(ControlsEnum type)
        {
            //Services     
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            AdmCompanyMstService admCompanyMstServiceClient;
            try
            {
                switch (type)
                {
                    #region Default
                    case ControlsEnum.DEFAULT:
                        GetFieldValues(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region Default
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        dtCurrentCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion
                }
            }
            catch
            {
                throw;
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
                    case ControlsEnum.DEFAULT:
                        SetFieldValues(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
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
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode, Object srcObj)
        {
            gafViewModel = new GafFileViewModel();
            try
            {
                switch (mode)
                {
                    case ActionsEnum.VIEW:
                        gafViewModel.CompanyPK = Convert.ToInt32(ddlCompany.SelectedValue);
                        gafViewModel.SelectedGstFile = getGstFileTypes();
                        gafViewModel.FromDate = DateTime.Parse(txtStartDate.Text);
                        gafViewModel.ToDate = DateTime.Parse(txtToDate.Text);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return gafViewModel;
        }

        private GstFileTypes getGstFileTypes()
        {
            if (rdoAll.Checked)
            {
                this.selectedGstFileType = GstFileTypes.ALL;
                return GstFileTypes.ALL;
            }
            else if (rdoCompanyInfo.Checked)
            {
                this.selectedGstFileType = GstFileTypes.COMPANYINFO;
                return GstFileTypes.COMPANYINFO;
            }
            else if (rdoPurchase.Checked)
            {
                this.selectedGstFileType = GstFileTypes.PURCHASE;
                return GstFileTypes.PURCHASE;
            }
            else if (rdoSupply.Checked)
            {
                this.selectedGstFileType = GstFileTypes.SUPPLY;
                return GstFileTypes.SUPPLY;
            }
            else if (rdoGeneralLedger.Checked)
            {
                this.selectedGstFileType = GstFileTypes.GENERALLEDGER;
                return GstFileTypes.GENERALLEDGER;
            }
            else
            {
                this.selectedGstFileType = GstFileTypes.NONE;
                throw new Exception("Select a File Type");
            }
        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Company
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                    }
                    if (dtCurrentCompany != null && dtCurrentCompany.Rows.Count > 0)
                    {
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCurrentCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                        ddlCompany.Enabled = false;
                    }
                    //To set company related to current SBU 
                    //if (dtCmp != null && dtCmp.Rows.Count > 0)
                    //{
                    //    ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCmp.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    //}

                    break;
                #endregion
            }
        }
    }

    public enum ControlsEnum
    {
        DEFAULT,
        COMPANY
    }
}