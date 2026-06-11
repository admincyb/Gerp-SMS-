using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.IO;
using BusinessObject.AccountManagement;
using BusinessObject;
using ERP.Utilities;
using System.Text;
using System.Text.RegularExpressions;
using BusinessObject.CommonManagement;
using System.Configuration;
using BusinessObject.ProductManagement;
using BusinessLogic.BrandRates;
using BusinessLogic.CommonManagement;
namespace ERPSMS_v01.OrderToCash
{
    public partial class BrandImport : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        private OleDbConnection connExcel;
        private OleDbCommand cmdExcel;
        private OleDbDataAdapter oleDbDataAdapter;
        private ActionsEnum commonActions;
        private string uploadPath;
        private string applicationPath;
        private string uploadName;
        private DataTable dtExcelSchema;
        private DataSet dsBrands;
        private DataTable dtBrands;
        private DataTable dtLandingHdr;
        private DataTable dtLandingDtl;
        private FileInfo uploadInfo;
        private string extns;
        private FileInfo attchInfo;
        User currentUser;
        private string landingSheet;
        private string xmlLanding;
        private string index;
        private BrandImportBO brandImport;
        //private LandingDtl landingDtl;
        private StringBuilder sb;
        private DataTable dtAircraft;
        private DataTable dtAircraftType;
        private int fromAirport;
        private int toAirport;
        private int aircraftType;
        private Regex isNumber;
        //private string[] airColums = { "RowNo", "CusCode", "Customer", "BrandName", "ProductCode", "ProductDesc", "BrandCode", "Rate", "Curr", "AwIB", "AwIBPrnCode", "AwIBSNo", "AwIBFile", "AwPC", "AwPCSNo", "AwPCFile", "AwIC", "AwICSNo", "AwICFile", "AwZB", "AwMC", "AwMCPrnCode", "AwMCSNo", "AwMCFile", "PackCode", "PackDesc", "Pcs1", "Pcs2", "Pcs3", "PcsTot", "IBL", "IBW", "IBH", "IBRate", "IBDim", "IBPrinter", "PCL", "PCW", "PCH", "PCDim", "PCRate", "PCPrinter", "ZBL", "ZBW", "ZBH", "ZBDim", "ZBPrinter", "ICL", "ICW", "ICH", "ICRate", "ICDim", "ICPrinter", "MCL1", "MCW1", "MCH1", "MCDim1", "MCL2", "MCW2", "MCH2", "MCDim2", "MCRate", "Ply", "PaperColor", "MCPrinter" };
        //private string[] airColums = { "RowNo", "CusCode", "Customer", "BrandName", "ProductCode", "BrandCode", "Rate", "AwIB", "AwIBPrnCode", "AwIBSNo", "AwIBFile", "AwPC", "AwPCSNo", "AwPCFile", "AwIC", "AwICSNo", "AwICFile", "AwZB", "AwMC", "AwMCPrnCode", "AwMCSNo", "AwMCFile", "PackCode", "PackDesc", "Pcs1", "Pcs2", "Pcs3", "PcsTot", "IBL", "IBW", "IBH", "IBRate", "IBPrinter", "PCL", "PCW", "PCH", "PCRate", "PCPrinter", "ZBL", "ZBW", "ZBH", "ZBPrinter", "ICL", "ICW", "ICH", "ICRate", "ICPrinter", "MCL1", "MCW1", "MCH1", "MCL2", "MCW2", "MCH2", "MCRate", "Ply", "PaperColor", "MCPrinter", "PackMtlGroup", "BrandGroup", "RateUnit" };
        private int LandingPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["Landing"]);
            }
            set
            {
                this.ViewState["Landing"] = value;
            }
        }

        private int LandingDtlPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["LandingDtl"]);
            }
            set
            {
                this.ViewState["LandingDtl"] = value;
            }
        }

        private string LastModDt
        {
            get
            {
                return Convert.ToString(this.ViewState["LastModDt"]);
            }
            set
            {
                this.ViewState["LastModDt"] = value;
            }
        }
        /// <summary>
        /// To keep Client Code
        /// </summary>
        private string ClientCode
        {
            get
            {
                return Convert.ToString(this.ViewState["ClientCode"]);
            }
            set
            {
                this.ViewState["ClientCode"] = value;
            }
        }
        /// <summary>
        /// To keep client specific SP
        /// </summary>
        private string SP_Name
        {
            get
            {
                return Convert.ToString(this.ViewState["SP_Name"]);
            }
            set
            {
                this.ViewState["SP_Name"] = value;
            }
        }

        #endregion

        #region Set Page Variables

        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
            //Initialze the current logged in user to the currentUser variable
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

        }
        /// <summary>
        /// 
        /// </summary>
        private void SetConfigurationValues()
        {
            SP_Name = GetLocalResourceObject("DefaultSP").ToString();
            DataTable dt = CommonBL.GetApplicaitonConfiguaration("CLIENT CODE", string.Empty);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["ACF_DATA"].ToString() == GetLocalResourceObject("HealthGlove").ToString())
                {
                    SP_Name = GetLocalResourceObject("HG_SP_Name").ToString();
                }
            }
        }
        #endregion

        #region Page Level Events

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageVariables();
            if (!IsPostBack)
            {
                SetConfigurationValues();
            }
        }

        #endregion

        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls
        /// </summary>
        private void GetFieldValues(PageControlEnums controlsEnum)
        {
            switch (controlsEnum)
            {
            }
        }

        #endregion

        #region Set Field Values

        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// </summary>
        private void SetFieldValues(PageControlEnums controlsEnum)
        {
            switch (controlsEnum)
            {
                case PageControlEnums.LANDINGEDIT:
                    SetLandingValuesFromObject();
                    break;
            }
        }

        #endregion

        #region -- For Buttons ---

        /// <summary>
        /// Methode Used to handle the EventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            int result = 0;
            if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
            {
                commonActions = ActionsEnum.CHANGE;
            }
            switch (commonActions)
            {

                case ActionsEnum.SAVE:

                    if (!IsValid)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else
                    {
                        if (fupImport.HasFile)
                        {
                            string conStr;
                            string filePath = SaveDetails(out conStr);
                            ImportToGrid(filePath, conStr);
                        }
                        else
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("AttachFile").ToString();
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                    }
                    break;


                case ActionsEnum.CANCEL:

                    ResetForms();
                    GetFieldValues(PageControlEnums.LANDING);
                    SetFieldValues(PageControlEnums.LANDING);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideDetails", "ShowHide('ADD_DETAILS');", true);
                    break;

            }
        }

        #endregion


        #region Helper Methods




        /// <summary>
        /// Methode used to read the excel data and save into grid
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="conStr"></param>
        private void ImportToGrid(string filePath, string conStr)
        {
            string[] airColums = (GetGlobalResourceObject("ConfigurationsRes", "BrandImportExcelColumn").ToString()).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                conStr = String.Format(conStr, filePath);
                connExcel = new OleDbConnection(conStr);
                cmdExcel = new OleDbCommand();
                oleDbDataAdapter = new OleDbDataAdapter();
                dtExcelSchema = new DataTable();
                dsBrands = new DataSet();

                cmdExcel.Connection = connExcel;
                connExcel.Open();
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dtExcelSchema != null && dtExcelSchema.Rows.Count > 0)
                {
                    if (dtExcelSchema != null)
                    {
                        landingSheet = string.Empty;
                        foreach (DataRow dr in dtExcelSchema.Rows)
                        {
                            if (dr["TABLE_NAME"].ToString().ToLower() == CommonConstants.EXELSHEETNAME.ToLower())
                            {
                                landingSheet = dr["TABLE_NAME"].ToString();
                            }
                        }
                        if (landingSheet == string.Empty)
                        {
                            litErrorMsg.Text = this.GetGlobalResourceObject("ErpRes", "Err_ExelsheetName").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                    }
                    cmdExcel.CommandText = "SELECT * From [" + landingSheet + "]";
                    oleDbDataAdapter.SelectCommand = cmdExcel;
                    oleDbDataAdapter.Fill(dsBrands, "Landing");
                    connExcel.Close();
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                else
                {
                    throw new Exception("Incorrect Format");
                }
                if (dsBrands != null && dsBrands.Tables.Count > 0)
                {
                    foreach (DataColumn item in dsBrands.Tables[0].Columns)
                    {
                        item.ColumnName = item.ColumnName.Replace(" ", "");
                    }
                    int cnt = (from p in airColums
                               where this.dsBrands.Tables[0].Columns.Contains(p)
                               select p).Count();

                    if (cnt != airColums.Length)
                    {
                        sb = new StringBuilder();
                        sb.Append(this.GetLocalResourceObject("Err_ExcelSheet").ToString());
                        foreach (string item in airColums)
                        {
                            if (!this.dsBrands.Tables[0].Columns.Contains(item))
                            {
                                sb.Append("<ul><li>" + item + "</li></ul>");
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString())
                                          + "','" + Resources.ErpRes.Information + "');", true);
                        return;
                    }
                    //if (cnt != airColums.Length)
                    //{
                    //    sb = new StringBuilder();
                    //    sb.Append(this.GetLocalResourceObject("Err_ExcelSheet").ToString());
                    //    foreach (string item in airColums)
                    //    {
                    //        sb.Append("<ul><li>" + item + "</li></ul>");
                    //    }
                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString())
                    //                       + "','" + Resources.ErpRes.Information + "');", true);
                    //    return;
                    //}
                    brandImport = new BrandImportBO();

                    brandImport.ImportData = dsBrands.Tables[0];
                    brandImport.ImportData = CommonFunctions.HtmlEncodeDataTable(dsBrands.Tables[0], "BrandName");
                    xmlLanding = CommonFunctions.ObjectTOXml(brandImport).InnerXml;
                    DataTable dtOut = null;
                    int result = BrandRatesBL.ImportCustomerBrands(xmlLanding, SP_Name,currentUser.SBUID ,ref dtOut);
                    // int  result = dsOut.Tables.Count > 0 ? Convert.ToInt32(dsOut.Tables[0].Rows[0][0]) : -1;
                    if (result > 0 && dsBrands.Tables.Count > 0)
                    {
                        ResetFields();
                        litErrorMsg.Text = this.GetLocalResourceObject("Save_Msg").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else
                        if (result == (int)DbSaveStatus.SQLERROR)
                        {
                            //litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_DataMissmatch").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                        {
                            litErrorMsg.Text = Resources.PageNameRes.CustomerBrand + " " + Resources.Messages.EditUsedByAnotherUser;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKPRINTER)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkPrinter").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKMCPRINTER)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkMcprinter").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);

                        }
                        else if (result == (int)DbSaveStatus.CHECKZBPRINTER)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkZbPrinter").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKPOUCHPRINTER)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkPouchPrinter").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKBOXPRINTER)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkBoxprinter").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKPACKINGTYPE)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkPakTypeName").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKQCANDTOTAL)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkQcTotal").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKPRODUCTEXIST)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkProductExist").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKCUSTOMEREXIST)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkCustomerExist").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKBRANDREPEAT)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkBrandRepeat").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKCURRENCYMASTER)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkCurrencyMaster").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKCURRENCYCODE)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkCurrencyCode").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CODEEXIST)
                        {

                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_BrandNotExists").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            litErrorMsg.Text = Resources.PageNameRes.CustomerBrand + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        }
                        else if (SaveDbEnum.NOEXELROWS == (SaveDbEnum)(result))
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_NoExelRows").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKBRANDNAMEEXIST)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_BrandExist").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            //litErrorMsg.Text = this.GetLocalResourceObject("Err_BrandExist").ToString();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.TOTALPCSZERO)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_TotalPiecesCountAreZero").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[1].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.PACKCODEZERO)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_PackCodeAreZero").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[1].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.PACKCODEVALIDATION)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_PackCodeValidationFailed").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[1].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.PACKCODEPIECESCOUNT)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_PackCodePiecesCountFailed").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[1].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.AQLNOTEXIST)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_AQLFailed").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKHCPRINTER)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkHcprinter").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKPACKINGSPEC)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkPakSpecName").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CHECKBRANDCODEEXIST)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkBrandCodeRepeat").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }

                        else if (result == (int)DbSaveStatus.CHECKBRANDCODEORBRANDNAMEEXIST)
                        {
                            sb = new StringBuilder();
                            sb.Append(this.GetLocalResourceObject("Err_ChkBrandCodeORBrandNameExist").ToString());
                            foreach (DataRow item in dtOut.Rows)
                            {
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            }
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }

                        else
                        {
                            //litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_DataMissmatch").ToString();
                            //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CustomerBrand);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                }
            }
            catch (OleDbException ex)
            {
                sb = new StringBuilder();
                sb.Append(this.GetLocalResourceObject("Err_ExcelSheet").ToString());
                foreach (string item in airColums)
                {
                    sb.Append("<ul><li>" + item + "</li></ul>");
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
            }
            catch (Exception ex)
            {
                //litErrorMsg.Text = ex.InnerException.ToString();
                litErrorMsg.Text = this.GetLocalResourceObject("Err_IncorrectExcel").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetFields()
        {
        }

        /// <summary>
        /// Methode used to save the excel file
        /// </summary>
        private string SaveDetails(out string conStr)
        {
            uploadInfo = new FileInfo(fupImport.PostedFile.FileName);
            extns = uploadInfo.Extension;
            uploadPath = CommonConstants.UPLOAD_FOLDER;
            applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;
            uploadName = GenerateFileName(uploadInfo.Name) + extns;
            attchInfo = new FileInfo(applicationPath + uploadPath + "\\" + uploadName);
            conStr = CheckValidFileType(extns);
            if (!string.IsNullOrEmpty(conStr))
            {
                if (CheckFolderExists(applicationPath + uploadPath))
                {
                    fupImport.SaveAs(attchInfo.FullName);
                }
            }
            return attchInfo.FullName;
        }

        /// <summary>
        /// Methode used to generate the new file name 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GenerateFileName(string fileName)
        {
            string uploadName;
            string[] fileDtls;
            fileDtls = new string[2];
            fileDtls = fileName.Split('.');
            uploadName = fileDtls[0] + Guid.NewGuid().ToString();
            return uploadName;
        }

        /// <summary>
        /// Check File is valid or not , using File Extension , if valid then get the connection string
        /// </summary>
        /// <param name="extn"></param>
        /// <returns>bool : True - valid File, false - Invalid File</returns>
        private string CheckValidFileType(string extn)
        {
            string conStr;
            switch (extn.ToLower())
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.AppSettings["Excel03ConString"];
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.AppSettings["Excel07ConString"];
                    break;
                default: conStr = string.Empty; break;
            }
            return conStr;
        }

        /// <summary>
        /// Method to Check Folder Exists or not
        /// </summary>
        /// <param name="uploadUrl"></param>
        /// <returns>Bool</returns>
        public bool CheckFolderExists(string uploadUrl)
        {
            if (System.IO.Directory.Exists(uploadUrl))
                return true;
            else
                return false;

        }

        /// <summary>
        /// Method used to get values of ui to the landing
        /// </summary>
        private void SetLandingValuesFromObject()
        {
            if (dtLandingDtl != null && dtLandingDtl.Rows.Count > 0)
            {
                fromAirport = Convert.ToInt32(dtLandingDtl.Rows[0]["ROH_Airport_From"]);
                toAirport = Convert.ToInt32(dtLandingDtl.Rows[0]["ROH_Airport_To"]);
                aircraftType = Convert.ToInt32(dtLandingDtl.Rows[0]["ROH_Aircraft_Type"]);
                GetFieldValues(PageControlEnums.FROMAIRPORT);
                SetFieldValues(PageControlEnums.FROMAIRPORT);
                GetFieldValues(PageControlEnums.TOAIRPORT);
                SetFieldValues(PageControlEnums.TOAIRPORT);
                GetFieldValues(PageControlEnums.AIRPORTCODE);
                SetFieldValues(PageControlEnums.AIRPORTCODE);
                index = string.Empty;
                for (int i = 1; i < 13; i++)
                {
                    index = string.Format("{0,2:00}", i);
                    ((this.Form.FindControl("MainContent").FindControl("txtLandMonth" + index)) as TextBox).Text = dtLandingDtl.Rows[0][this.GetLocalResourceObject("Month" + index).ToString()].ToString();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetForms()
        {
            LandingDtlPK = 0;
            fromAirport = 0;
            toAirport = 0;
            aircraftType = 0;
            LastModDt = string.Empty;
        }

        #endregion
    }
    public enum PageControlEnums
    {
        LANDINGLIST,
        LANDING,
        LANDINGEDIT,
        FROMAIRPORT,
        AIRPORTCODE,
        TOAIRPORT
    }

    public enum SaveDbEnum
    {
        SQLERROR = -1,
        PLANNAMEEXISTS = -2,
        FROMNOTEXISTS = -3,
        TONOTEXISTS = -4,
        CODENOTEXISTS = -5,
        NOEXELROWS = -6
    }
}