using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject;
using BusinessObject.AccountManagement;
using ERPData;
using ERPManager;
using gComs.WorkFlow;
using ERPService;
using ERPService.Administration;
using BusinessObject.CommonManagement;
using System.Web.UI.HtmlControls;
using System.Threading;
using ERP.Store.UI;
using System.Data;
using BusinessObject.Journalize;
using ERP.Utilities.Validations;
using System.Drawing;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using DocumentFormat.OpenXml.Packaging;
using XCell = DocumentFormat.OpenXml.Spreadsheet.Cell;
using XCellValues = DocumentFormat.OpenXml.Spreadsheet.CellValues;
using XRow = DocumentFormat.OpenXml.Spreadsheet.Row;
using XSharedStringTable = DocumentFormat.OpenXml.Spreadsheet.SharedStringTable;
using XSheet = DocumentFormat.OpenXml.Spreadsheet.Sheet;

namespace ERPSMS_v01.Journalize.UserControls
{
    public partial class TransactionImport : System.Web.UI.UserControl
    {
        #region EventHandlers
        public event EventHandler TransactionActionHandler;
        public event EventHandler AfterValidation;
        #endregion

        #region Variables and Properties
        private User currentUser;
        private ActionsEnum commonActions;
        private OleDbConnection connExcel;
        private OleDbCommand cmdExcel;
        private OleDbDataAdapter oleDbDataAdapter;
        private string uploadPath;
        private string applicationPath;
        private string uploadName;
        private DataTable dtExcelSchema;
        public DataSet dsTransactions;
        private DataTable dtBrands;
        private DataTable dtLandingHdr;
        private DataTable dtLandingDtl;
        private FileInfo uploadInfo;
        private string extns;
        private FileInfo attchInfo;
        private string landingSheet;
        private string XmlDoc;
        bool InvalidCCAmountFlag = false;
        int ResultAmount = 0;
        string AccName;
        string SlNo = "";

        public List<VoucherTransactionBO> VoucherDetails
        {
            get
            {
                return this.Session[ViewstateStrings.VoucherDetails] == null ? new List<VoucherTransactionBO>() : (List<VoucherTransactionBO>)this.Session[ViewstateStrings.VoucherDetails];
            }
            set
            {
                this.Session[ViewstateStrings.VoucherDetails] = value;
            }
        }


        /// <summary>
        /// Type For Voucher Number Generation
        /// </summary>
        public string VoucherApplicationType
        {
            get
            {
                if (this.ViewState[ViewstateStrings.VoucherApplicationType] != null)
                    return this.ViewState[ViewstateStrings.VoucherApplicationType].ToString();
                else
                    return null;
            }
            set
            {
                this.ViewState[ViewstateStrings.VoucherApplicationType] = value;
            }
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
                // btnImport.Click += new EventHandler(ActionHandler);
                if (!IsPostBack)
                {

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

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

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }

                switch (commonActions)
                {
                    #region ImportExcel
                    case ActionsEnum.IMPORT:
                        if (fupImport.HasFile)
                        {
                            string conStr;
                            string filePath = SaveDetails(out conStr);
                            ImportToGrid(filePath, conStr, sender, e);
                        }

                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        /// <summary>
        /// Methode used to read the excel data and save into grid
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="conStr"></param>
        private void ImportToGrid(string filePath, string conStr, object sender, EventArgs e)
        {
            string[] TransactionColums = (GetGlobalResourceObject("ConfigurationsRes", "VoucherTransactionExcelImportColumn").ToString()).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                dsTransactions = new DataSet();
                int Result = 0;

                if (Path.GetExtension(filePath).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    dsTransactions.Tables.Add(ReadXlsxTransactionSheet(filePath));
                }
                else
                {
                    conStr = String.Format(conStr, filePath);
                    connExcel = new OleDbConnection(conStr);
                    cmdExcel = new OleDbCommand();
                    oleDbDataAdapter = new OleDbDataAdapter();
                    dtExcelSchema = new DataTable();
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
                                if (dr["TABLE_NAME"].ToString().ToLower() == CommonConstants.EXELSHEETNAME_TRANSACTION.ToLower())
                                {
                                    landingSheet = dr["TABLE_NAME"].ToString();
                                }
                            }
                            if (landingSheet == string.Empty)
                            {
                                litErrorMsg.Text = this.GetGlobalResourceObject("ErpRes", "Err_TransactionExelsheetName").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                return;
                            }
                        }


                        cmdExcel.CommandText = "SELECT * From [" + landingSheet + "]";
                        oleDbDataAdapter.SelectCommand = cmdExcel;
                        oleDbDataAdapter.Fill(dsTransactions, "Landing");
                        connExcel.Close();

                        //foreach (DataRow row in dsTransactions.Tables["Landing"].Rows)
                        //{
                        //    row["Dr Amount"] = Convert.ToDecimal(row["Dr Amount"]);
                        //    row["Cr Amount"] = Convert.ToDecimal(row["Cr Amount"]);

                        //}
                    }
                    else
                    {
                        throw new Exception("Incorrect Format");
                    }
                }

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                if (dsTransactions != null && dsTransactions.Tables.Count > 0)
                {
                    DataTable dtTransactions = new DataTable();
                    dtTransactions = CommonFunctions.HtmlEncodeDataTable(dsTransactions.Tables[0], "Account");
                    //string SlNo = "";



                    for (int i = 0; i < dtTransactions.Rows.Count; i++)
                    {
                        SlNo = dtTransactions.Rows[i]["Sl No"].ToString();

                        //Sl No Validation 
                        if (dtTransactions.Rows[i]["Sl No"].ToString() == "" && dtTransactions.Rows[i]["Account Code"].ToString() != ""
                            || dtTransactions.Rows[i]["Sl No"].ToString() != "")
                        {
                            int slno = 0;
                            bool res_slno = int.TryParse(dtTransactions.Rows[i]["Sl No"].ToString(), out slno);
                            if (!res_slno)
                            {
                                litErrorMsg.Text = this.GetLocalResourceObject("Err_SlNo").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                BindAfterValidation(sender, e);
                                return;
                            }
                        }

                        //mode validation
                        if (dtTransactions.Rows[i]["Mode"].ToString() != string.Empty && dtTransactions.Rows[i]["Mode"].ToString() != "")
                        {
                            double mode = 0;
                            bool res_mode = double.TryParse(dtTransactions.Rows[i]["Mode"].ToString(), out mode);
                            if (res_mode)
                            {
                                litErrorMsg.Text = "Sl No =" + SlNo + " , " + this.GetLocalResourceObject("Err_Mode").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                BindAfterValidation(sender, e);
                                return;
                            }
                        }

                        //mode validation based on application type only work in direct payment voucher import
                        if (VoucherApplicationType == "DPVJ")
                        {
                            if (dtTransactions.Rows[i]["Mode"].ToString() == "" && dtTransactions.Rows[i]["Account Code"].ToString() != "")
                            {
                                //double mode = 0;
                                //bool res_mode = double.TryParse(dtTransactions.Rows[i]["Mode"].ToString(), out mode);
                                //if (res_mode)
                                //{
                                litErrorMsg.Text = "Sl No =" + SlNo + " , " + this.GetLocalResourceObject("Err_Enter_Mode").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                BindAfterValidation(sender, e);
                                return;
                                //}
                            }
                        }


                        //Dr Amount Validation
                        if (dtTransactions.Rows[i]["Dr Amount"].ToString() != string.Empty && dtTransactions.Rows[i]["Dr Amount"].ToString() != null &&
                            dtTransactions.Rows[i]["Dr Amount"].ToString() != "")
                        {
                            decimal DrAmt = 0;
                            bool DrRes = decimal.TryParse(dtTransactions.Rows[i]["Dr Amount"].ToString(), out DrAmt);
                            if (DrRes == false)
                            {
                                litErrorMsg.Text = "Sl No =" + SlNo + " , " + this.GetLocalResourceObject("Err_DrAmount").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                BindAfterValidation(sender, e);
                                return;
                            }
                        }

                        //Cr Amount Validation
                        if (dtTransactions.Rows[i]["Cr Amount"].ToString() != string.Empty && dtTransactions.Rows[i]["Cr Amount"].ToString() != null &&
                            dtTransactions.Rows[i]["Cr Amount"].ToString() != "")
                        {
                            decimal CrAmt = 0;
                            bool CrRes = decimal.TryParse(dtTransactions.Rows[i]["Cr Amount"].ToString(), out CrAmt);
                            if (CrRes == false)
                            {
                                litErrorMsg.Text = "Sl No =" + SlNo + " , " + this.GetLocalResourceObject("Err_CrAmount").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                BindAfterValidation(sender, e);
                                return;
                            }
                        }

                        //CostCenter Amount Validation
                        if (dtTransactions.Rows[i]["CC Amount"].ToString() != string.Empty && dtTransactions.Rows[i]["CC Amount"].ToString() != null &&
                            dtTransactions.Rows[i]["CC Amount"].ToString() != "" || dtTransactions.Rows[i]["CC Amount"].ToString() == "" &&
                            dtTransactions.Rows[i]["Cost Center"].ToString() != "")
                        {
                            decimal CcAmt = 0;
                            bool CcRes = decimal.TryParse(dtTransactions.Rows[i]["CC Amount"].ToString(), out CcAmt);
                            if (CcRes == false)
                            {
                                litErrorMsg.Text = "Sl No =" + SlNo + " , " + this.GetLocalResourceObject("Err_CcAmount").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                BindAfterValidation(sender, e);
                                return;
                            }
                        }

                        //Pdc value validation
                        if (dtTransactions.Rows[i]["PDC"].ToString() != "0" && dtTransactions.Rows[i]["PDC"].ToString() != "1" && dtTransactions.Rows[i]["PDC"].ToString() != "")
                        {
                            litErrorMsg.Text = "Sl No =" + SlNo + " , " + "Invalid PDC";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }

                        if (dtTransactions.Columns.Contains("Amt before VAT"))
                        {
                            if (dtTransactions.Rows[i]["Amt before VAT"].ToString() != string.Empty &&
                                dtTransactions.Rows[i]["Amt before VAT"].ToString() != "" &&
                                dtTransactions.Rows[i]["Amt before VAT"].ToString() != null)
                            {
                                decimal AmtBeforeVat = 0;
                                bool AmtBef = decimal.TryParse(dtTransactions.Rows[i]["Amt before VAT"].ToString(), out AmtBeforeVat);
                                if (AmtBef == false)
                                {
                                    litErrorMsg.Text = "Sl No =" + SlNo + " , " + this.GetLocalResourceObject("Err_AmtBeforeVat").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                    BindAfterValidation(sender, e);
                                    return;
                                }
                            }
                        }
                    }

                    //data binding
                    decimal ccAmount;
                    List<VoucherTransactionBO> VoucherTrxList = new List<VoucherTransactionBO>();
                    VoucherTransactionBO VoucherTrxObj = new VoucherTransactionBO();
                    VoucherTrxObj.CostCenterMapList = new List<CostCenterMapBO>();
                    CostCenterMapBO CCObj = new CostCenterMapBO();
                    string CurrSLNo = dtTransactions.Rows[0]["Sl No"].ToString();
                    VoucherTrxObj.SLNO = dtTransactions.Rows[0]["Sl No"].ToString();
                    VoucherTrxObj.ACCOUNT_TEXT = dtTransactions.Rows[0]["Account"].ToString();
                    VoucherTrxObj.ACCOUNT_CODE = dtTransactions.Rows[0]["Account Code"].ToString();
                    VoucherTrxObj.ACC_SUB_TYPE = dtTransactions.Rows[0]["SubTypeAccount"].ToString();
                    VoucherTrxObj.PAYMENT_MODE_TEXT = dtTransactions.Rows[0]["Mode"].ToString();
                    VoucherTrxObj.NARRATION = dtTransactions.Rows[0]["Narration"].ToString();
                    VoucherTrxObj.DR_AMT_BC =CommonFunctions.DecimalFormat(Convert.ToDecimal(dtTransactions.Rows[0]["Dr Amount"].ToString() == string.Empty ? 0 : dtTransactions.Rows[0]["Dr Amount"]),2);
                    VoucherTrxObj.CR_AMT_BC =CommonFunctions.DecimalFormat(Convert.ToDecimal(dtTransactions.Rows[0]["Cr Amount"].ToString() == string.Empty ? 0 : dtTransactions.Rows[0]["Cr Amount"]),2);
                    VoucherTrxObj.INSTR_NO = dtTransactions.Rows[0]["Instr No"].ToString();
                    VoucherTrxObj.INSTR_DATE = dtTransactions.Rows[0]["Date"].ToString();
                    VoucherTrxObj.INSTR_FAVOUR = dtTransactions.Rows[0]["Favour of"].ToString();
                    VoucherTrxObj.PDC = dtTransactions.Rows[0]["PDC"].ToString();

                    //Add aditional 4 column for mmt
                    if(dtTransactions.Columns.Contains("Vendor Code"))
                    {
                        VoucherTrxObj.VENDOR_CODE= dtTransactions.Rows[0]["Vendor Code"].ToString();
                    }
                    if (dtTransactions.Columns.Contains("Date Invoice"))
                    {
                        VoucherTrxObj.DATE_INVOICE = dtTransactions.Rows[0]["Date Invoice"].ToString();
                    }
                    if (dtTransactions.Columns.Contains("Invoice no"))
                    {
                        VoucherTrxObj.INVOICE_NO = dtTransactions.Rows[0]["Invoice no"].ToString();
                    }
                    if (dtTransactions.Columns.Contains("Refer PUR"))
                    {
                        VoucherTrxObj.REFER_PUR = dtTransactions.Rows[0]["Refer PUR"].ToString();
                    }
                    if (dtTransactions.Columns.Contains("Amt before VAT"))
                    {
                        VoucherTrxObj.FTR_AMT_BFR_VAT= CommonFunctions.DecimalFormat(Convert.ToDecimal(dtTransactions.Rows[0]["Amt before VAT"].ToString() == string.Empty ? 0 : dtTransactions.Rows[0]["Amt before VAT"]), 2);
                    }



                    if (!string.IsNullOrEmpty(dtTransactions.Rows[0]["Cost Center"].ToString())
                        && !string.IsNullOrEmpty(dtTransactions.Rows[0]["CC Amount"].ToString())
                        && decimal.TryParse(dtTransactions.Rows[0]["CC Amount"].ToString(), out ccAmount) == true)
                        VoucherTrxObj.CostCenterMapList.Add(
                            new CostCenterMapBO()
                            {
                                COST_CENTER_TEXT = dtTransactions.Rows[0]["Cost Center"].ToString(),
                                AMT_BC = CommonFunctions.DecimalFormat(ccAmount, 4)
                                  

                            });
                    for (int i = 1; i < dtTransactions.Rows.Count; i++)
                    {
                        if (CurrSLNo == dtTransactions.Rows[i]["Sl No"].ToString() || dtTransactions.Rows[i]["Sl No"].ToString() == string.Empty)
                        {
                            if (!string.IsNullOrEmpty(dtTransactions.Rows[i]["Cost Center"].ToString())
                             && !string.IsNullOrEmpty(dtTransactions.Rows[i]["CC Amount"].ToString())
                             && decimal.TryParse(dtTransactions.Rows[i]["CC Amount"].ToString(), out ccAmount) == true)
                                VoucherTrxObj.CostCenterMapList.Add(
                                    new CostCenterMapBO()
                                    {
                                        COST_CENTER_TEXT = dtTransactions.Rows[i]["Cost Center"].ToString(),
                                        AMT_BC = CommonFunctions.DecimalFormat(ccAmount, 2)
                                    });
                        }
                        else
                        {
                            if (VoucherTrxObj != null)
                            {
                                VoucherTrxList.Add(VoucherTrxObj);
                                VoucherTrxObj = new VoucherTransactionBO();
                                VoucherTrxObj.CostCenterMapList = new List<CostCenterMapBO>();
                            }
                            CurrSLNo = dtTransactions.Rows[i]["Sl No"].ToString();
                            VoucherTrxObj.SLNO = dtTransactions.Rows[i]["Sl No"].ToString();
                            VoucherTrxObj.ACCOUNT_TEXT = dtTransactions.Rows[i]["Account"].ToString();
                            VoucherTrxObj.ACCOUNT_CODE = dtTransactions.Rows[i]["Account Code"].ToString();
                            VoucherTrxObj.ACC_SUB_TYPE = dtTransactions.Rows[i]["SubTypeAccount"].ToString();
                            VoucherTrxObj.PAYMENT_MODE_TEXT = dtTransactions.Rows[i]["Mode"].ToString();
                            VoucherTrxObj.NARRATION = dtTransactions.Rows[i]["Narration"].ToString();
                            VoucherTrxObj.DR_AMT_BC =CommonFunctions.DecimalFormat(Math.Round(Convert.ToDecimal(dtTransactions.Rows[i]["Dr Amount"].ToString() == string.Empty ? 0 : dtTransactions.Rows[i]["Dr Amount"]),2),2);
                            VoucherTrxObj.CR_AMT_BC =CommonFunctions.DecimalFormat(Math.Round(Convert.ToDecimal(dtTransactions.Rows[i]["Cr Amount"].ToString() == string.Empty ? 0 : dtTransactions.Rows[i]["Cr Amount"]),2),2);
                            VoucherTrxObj.INSTR_NO = dtTransactions.Rows[i]["Instr No"].ToString();
                            VoucherTrxObj.INSTR_DATE = dtTransactions.Rows[i]["Date"].ToString();
                            VoucherTrxObj.INSTR_FAVOUR = dtTransactions.Rows[i]["Favour of"].ToString();
                            VoucherTrxObj.PDC = dtTransactions.Rows[i]["PDC"].ToString();

                            //Add aditional 4 column for mmt
                            if (dtTransactions.Columns.Contains("Vendor Code"))
                            {
                                VoucherTrxObj.VENDOR_CODE = dtTransactions.Rows[i]["Vendor Code"].ToString();
                            }
                            if (dtTransactions.Columns.Contains("Date Invoice"))
                            {
                                VoucherTrxObj.DATE_INVOICE = dtTransactions.Rows[i]["Date Invoice"].ToString();
                            }
                            if (dtTransactions.Columns.Contains("Invoice no"))
                            {
                                VoucherTrxObj.INVOICE_NO = dtTransactions.Rows[i]["Invoice no"].ToString();
                            }
                            if (dtTransactions.Columns.Contains("Refer PUR"))
                            {
                                VoucherTrxObj.REFER_PUR = dtTransactions.Rows[i]["Refer PUR"].ToString();
                            }
                            if (dtTransactions.Columns.Contains("Amt before VAT"))
                            {
                                VoucherTrxObj.FTR_AMT_BFR_VAT = CommonFunctions.DecimalFormat(Convert.ToDecimal(dtTransactions.Rows[i]["Amt before VAT"].ToString() == string.Empty ? 0 : dtTransactions.Rows[i]["Amt before VAT"]), 2);
                            }


                            if (!string.IsNullOrEmpty(dtTransactions.Rows[i]["Cost Center"].ToString())
                                && !string.IsNullOrEmpty(dtTransactions.Rows[i]["CC Amount"].ToString())
                                && decimal.TryParse(dtTransactions.Rows[i]["CC Amount"].ToString(), out ccAmount) == true)
                                VoucherTrxObj.CostCenterMapList.Add(
                                    new CostCenterMapBO()
                                    {
                                        COST_CENTER_TEXT = dtTransactions.Rows[i]["Cost Center"].ToString(),
                                        AMT_BC =CommonFunctions.DecimalFormat(ccAmount,2)
                                    });

                        }
                    }//end of loop
                    if (VoucherTrxObj != null)
                    {
                        VoucherTrxList.Add(VoucherTrxObj);
                        VoucherTrxObj = new VoucherTransactionBO();
                        VoucherTrxObj.CostCenterMapList = new List<CostCenterMapBO>();
                    }

                    //Cost center sum and debit or credit amount equal check
                    int result = 0;
                    result = CCAmountValidityChecking(VoucherTrxList);

                    //if (InvalidCCAmountFlag == true)
                    //Cost center not equal
                    if (result == -2)
                    {
                        litErrorMsg.Text = "Sl No =" + SlNo + " , " + AccName + " Account has " + this.GetLocalResourceObject("Err_CC_SplitMissmatch").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        BindAfterValidation(sender, e);
                        return;
                    }
                    else if (result == -3)
                    {
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_SUM_CR_DR_Amount").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        BindAfterValidation(sender, e);
                        return;
                    }
                    else if (result == -4)
                    {
                        litErrorMsg.Text = "Sl No =" + SlNo + " , " + this.GetLocalResourceObject("Err_DebitOrCreditAmount").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        BindAfterValidation(sender, e);
                        return;
                    }

                    else
                    {
                        string Ret_Xml = "";
                        string SLNO = "Sl No = ";
                        DataTable dt_text = new DataTable();
                        VoucheData objData = new VoucheData();
                        objData.VoucherTransactionList = VoucherTrxList;
                        //XmlDoc = CommonFunctions.ObjectTOXml(objData).InnerXml; 
                        XmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objData).InnerXml;
                        Result = BusinessLogic.Jouralize.JournalizeBL.VoucherTransactionExcelImportValidation(XmlDoc, out Ret_Xml, out dt_text);
                        if (Result > 0)
                        {
                            VoucheData VoucherList = CommonFunctions.XmlDeserialize<VoucheData>(Ret_Xml);
                            VoucherDetails = VoucherList.VoucherTransactionList;
                            ((Button)sender).CommandName = ActionsEnum.IMPORT.ToString();
                            TransactionActionHandler(sender, e);
                        }
                        else if (Result == -11) //Account is not valid
                        {
                            litErrorMsg.Text = SLNO + dt_text.Rows[0][0].ToString() + ",  " + dt_text.Rows[0][1].ToString() + "  " + this.GetLocalResourceObject("Msg_AccountNotValid").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            BindAfterValidation(sender, e);
                            return;
                        }
                        else if (Result == -12)//Cost Center is not valid
                        {
                            litErrorMsg.Text = SLNO + dt_text.Rows[0][0].ToString() + ",  " + dt_text.Rows[0][1].ToString() + "  " + this.GetLocalResourceObject("Msg_CCNotValid").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            BindAfterValidation(sender, e);
                            return;
                        }
                        else if (Result == -13)//Cost Center is not maped with Account
                        {
                            litErrorMsg.Text = SLNO + dt_text.Rows[0][0].ToString() + ",  " + dt_text.Rows[0][1].ToString() + " , " + this.GetLocalResourceObject("Msg_CCNotMapedWithAccount").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            BindAfterValidation(sender, e);
                            return;
                        }

                        else if (Result == -14)//SubType not matching with Account
                        {
                            litErrorMsg.Text = SLNO + dt_text.Rows[0][0].ToString() + ",  " + dt_text.Rows[0][1].ToString() + " , " + this.GetLocalResourceObject("Msg_SubTypeError").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            BindAfterValidation(sender, e);
                            return;
                        }

                        else if (Result == -15)//Cost Center Required
                        {
                            litErrorMsg.Text = SLNO + dt_text.Rows[0][0].ToString() + ",  " + dt_text.Rows[0][1].ToString() + "   " + this.GetLocalResourceObject("Msg_CostCenterRequired").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            BindAfterValidation(sender, e);
                            return;
                        }

                        else if (Result == -16)//Vendor is not valid
                        {
                            litErrorMsg.Text = SLNO + dt_text.Rows[0][0].ToString() + ",  " + dt_text.Rows[0][1].ToString() + "   " + this.GetLocalResourceObject("Msg_VendorNotValid").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            BindAfterValidation(sender, e);
                            return;
                        }

                        else if (Result == -17)//SubType Account is missing
                        {
                            litErrorMsg.Text = SLNO + dt_text.Rows[0][0].ToString() + ",  " + dt_text.Rows[0][1].ToString() + "   " + this.GetLocalResourceObject("Msg_SubTypeAccountMissing").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            BindAfterValidation(sender, e);
                            return;
                        }
                    }
                }
                else
                {
                    litErrorMsg.Text = this.GetLocalResourceObject("Err_IncorrectExcel").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                }
            }
            catch (Exception ex)
            {
                litErrorMsg.Text = this.GetLocalResourceObject("Err_IncorrectExcel").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }

        private DataTable ReadXlsxTransactionSheet(string filePath)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                XSheet sheet = workbookPart.Workbook.Descendants<XSheet>()
                    .FirstOrDefault(s => (s.Name == null ? string.Empty : s.Name.Value).Equals(CommonConstants.EXELSHEETNAME_TRANSACTION.TrimEnd('$'), StringComparison.OrdinalIgnoreCase));

                if (sheet == null)
                {
                    litErrorMsg.Text = this.GetGlobalResourceObject("ErpRes", "Err_TransactionExelsheetName").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                    throw new Exception("Incorrect Format");
                }

                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                XSharedStringTable sharedStrings = workbookPart.SharedStringTablePart == null ? null : workbookPart.SharedStringTablePart.SharedStringTable;
                List<XRow> rows = worksheetPart.Worksheet.Descendants<XRow>().ToList();
                if (rows.Count == 0)
                {
                    throw new Exception("Incorrect Format");
                }

                DataTable table = new DataTable("Landing");
                Dictionary<int, string> headers = ReadRowValues(rows[0], sharedStrings);
                int maxColumnIndex = headers.Count == 0 ? 0 : headers.Keys.Max();

                for (int columnIndex = 1; columnIndex <= maxColumnIndex; columnIndex++)
                {
                    string columnName = headers.ContainsKey(columnIndex) ? headers[columnIndex].Trim() : string.Empty;
                    if (string.IsNullOrEmpty(columnName))
                    {
                        columnName = "Column" + columnIndex.ToString();
                    }
                    if (table.Columns.Contains(columnName))
                    {
                        columnName = columnName + "_" + columnIndex.ToString();
                    }
                    table.Columns.Add(columnName);
                }

                for (int rowIndex = 1; rowIndex < rows.Count; rowIndex++)
                {
                    Dictionary<int, string> values = ReadRowValues(rows[rowIndex], sharedStrings);
                    DataRow dataRow = table.NewRow();
                    bool hasValue = false;
                    for (int columnIndex = 1; columnIndex <= maxColumnIndex; columnIndex++)
                    {
                        string value = values.ContainsKey(columnIndex) ? values[columnIndex] : string.Empty;
                        dataRow[columnIndex - 1] = value;
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            hasValue = true;
                        }
                    }

                    if (hasValue)
                    {
                        table.Rows.Add(dataRow);
                    }
                }

                return table;
            }
        }

        private Dictionary<int, string> ReadRowValues(XRow row, XSharedStringTable sharedStrings)
        {
            Dictionary<int, string> values = new Dictionary<int, string>();
            foreach (XCell cell in row.Elements<XCell>())
            {
                values[GetColumnIndex(cell.CellReference.Value)] = GetCellValue(cell, sharedStrings);
            }
            return values;
        }

        private string GetCellValue(XCell cell, XSharedStringTable sharedStrings)
        {
            string value = cell.CellValue == null ? string.Empty : cell.CellValue.InnerText;
            if (cell.DataType == null)
            {
                return value;
            }

            switch (cell.DataType.Value)
            {
                case XCellValues.SharedString:
                    int sharedStringIndex;
                    if (int.TryParse(value, out sharedStringIndex) && sharedStrings != null)
                    {
                        return sharedStrings.ElementAt(sharedStringIndex).InnerText;
                    }
                    return string.Empty;
                case XCellValues.Boolean:
                    return value == "1" ? "TRUE" : "FALSE";
                case XCellValues.InlineString:
                    return cell.InnerText;
                default:
                    return value;
            }
        }

        private int GetColumnIndex(string cellReference)
        {
            int columnIndex = 0;
            foreach (char character in cellReference)
            {
                if (!char.IsLetter(character))
                {
                    break;
                }

                columnIndex *= 26;
                columnIndex += (char.ToUpperInvariant(character) - 'A' + 1);
            }
            return columnIndex;
        }

        public int CCAmountValidityChecking(List<VoucherTransactionBO> VoucherList)
        {
            decimal CCSum = 0;
            decimal DrCrAmount = 0;
            decimal DrSum = 0;
            decimal CrSum = 0;

            foreach (var item in VoucherList)
            {
                if (item.CostCenterMapList != null && item.CostCenterMapList.Count > 0)
                {
                    AccName = item.ACCOUNT_TEXT;
                    SlNo = item.SLNO;
                    if (item.CR_AMT_BC > 0 || item.DR_AMT_BC > 0)
                    {
                        DrCrAmount = (item.DR_AMT_BC > 0 ? item.DR_AMT_BC : item.CR_AMT_BC);
                        CCSum = (item.CostCenterMapList.Sum(s => s.AMT_BC > 0 ? s.AMT_BC : s.AMT_TC));
                        if (DrCrAmount != CCSum)
                        {
                            ResultAmount = -2;
                            break;
                        }
                    }
                    else
                    {
                        ResultAmount = -4;
                        break;
                    }
                }

                if (item.DR_AMT_BC > 0 || item.DR_AMT_TC > 0)
                {
                    DrSum = DrSum + item.DR_AMT_BC;
                }
                if (item.CR_AMT_BC > 0 || item.CR_AMT_TC > 0)
                {
                    CrSum = CrSum + item.CR_AMT_BC;
                }
            }
            if (ResultAmount != -2 && ResultAmount != -4)
            {
                if (DrSum != CrSum)
                {
                    ResultAmount = -3;
                }
            }
            return ResultAmount;
        }

        public void BindAfterValidation(object sender, EventArgs e)
        {
            ((Button)sender).CommandName = ActionsEnum.AFTERVALIDATION.ToString();
            AfterValidation(sender, e);
            ((Button)sender).CommandName = ActionsEnum.IMPORT.ToString();
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
    }
}
