using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using BusinessObject;
using System.Data;
using BusinessObject.CommonManagement;
using System.Xml;
using System.Text;
using BusinessObject.Finance;
using ERP.Store.UI;
using Microsoft.Office.Interop;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;

namespace ERPSMS_v01.Finance
{
    public partial class GSTRReports : ERP.Store.UI.MyBasePageUserRight
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Upload path
        /// </summary>
        private string UploadPath
        {
            get
            {
                return this.ViewState[ViewstateStrings.UploadPath] == null ? string.Empty : Convert.ToString(this.ViewState[ViewstateStrings.UploadPath]);
            }
            set
            {
                this.ViewState[ViewstateStrings.UploadPath] = value;
            }
        }

        /// <summary>
        /// Process ID of the Page
        /// </summary>
        private int PageProcessID
        {
            get
            {
                return this.ViewState["PageProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["PageProcessID"]);
            }
            set
            {
                this.ViewState["PageProcessID"] = value;
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
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.ENTRYMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }

        #endregion
        private ActionsEnum commonActions;
        private User currentUser;
        private DataTable dtReportTypes;
        private DataSet dsGstrData;
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
                    GetFieldValues(ControlsEnum.REPORTTYPE);
                    SetFieldValues(ControlsEnum.REPORTTYPE);
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        UploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                        if (!Directory.Exists(UploadPath))
                            Directory.CreateDirectory(UploadPath);
                        UploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                    }
                    else
                    {
                        UploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                    }

                    ddlReportType.Focus();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(System.Web.UI.Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            GSTRParams objParams;
            try
            {
                switch (type)
                {
                    #region REPORT TYPE
                    case ControlsEnum.REPORTTYPE:
                        dtReportTypes = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "GST REPORTS");
                        break;
                    #endregion
                    #region GSTR REPORT DATA
                    case ControlsEnum.GSTRREPORTDATA:
                        objParams = new GSTRParams();
                        DateTime? FromDate = null;
                        DateTime? ToDate = null;
                        if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
                            FromDate = DateTime.Parse(txtFromDate.Text.Trim());
                        if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
                            ToDate = DateTime.Parse(txtToDate.Text.Trim());
                        objParams.BizUnit = currentUser.SBUID;
                        objParams.FromDate = FromDate;
                        objParams.ToDate = ToDate;
                        objParams.ReportType = Convert.ToInt32(ddlReportType.SelectedValue);
                        dsGstrData = BusinessLogic.Finance.GSTReportBL.GetGSTRReports(objParams);
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

                    #region REPORT TYPE
                    case ControlsEnum.REPORTTYPE:
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
        #region Set UI Values To Object

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            try
            {
                switch (controlType)
                {
                    default: ;
                        break;
                }
                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
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
                    default: ;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region REPORT TYPE
                case ControlsEnum.REPORTTYPE:
                    ddlReportType.DataSource = dtReportTypes;
                    ddlReportType.DataTextField = Resources.DataFieldRes.cfgData;
                    ddlReportType.DataValueField = Resources.DataFieldRes.cfgValue;
                    ddlReportType.DataBind();
                    ddlReportType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                default: ;
                    break;
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
                    default: ;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                if (Mode == ActionsEnum.VIEW)
                {
                    EntryStatus = EntryStatus.VIEWMODE;
                }
                else
                {
                    EntryStatus = EntryStatus.ENTRYMODE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region DOWNLOAD
                    case ActionsEnum.DOWNLOAD:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(System.Web.UI.Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            GetFieldValues(ControlsEnum.GSTRREPORTDATA);
                            if (dsGstrData != null && dsGstrData.Tables.Count > 0 && dsGstrData.Tables[0].Rows.Count > 0)
                            {
                                //dsGstrData.DataSetName = "GSTR1";
                                int sheetCount = 1;
                                for (int colIndex = 0; colIndex < dsGstrData.Tables[0].Columns.Count; colIndex++)
                                {
                                    if (dsGstrData.Tables.Count > sheetCount)
                                    {
                                        dsGstrData.Tables[sheetCount].TableName = Convert.ToString(dsGstrData.Tables[0].Rows[0][colIndex]);
                                    }
                                    sheetCount++;
                                }
                                //Removing the first table that contains the excel tab names
                                dsGstrData.Tables.RemoveAt(0);
                            }
                            ExportDataSetToExcel(dsGstrData);
                            //ExportDataSetToExcel(dsGstrData, CreateExcel());
                        }
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ddlReportType.ClearSelection();
                        txtFromDate.Text = txtToDate.Text = string.Empty;
                        ddlReportType.Focus();
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(System.Web.UI.Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {

            }
        }

        #endregion
        #region Helper Methods
        private void ExportDataSetToExcel(DataSet ds)
        {
            try
            {
                string excelName = ddlReportType.SelectedItem.Text + "_" + Guid.NewGuid().ToString() + GetLocalResourceObject("ExcelFormat").ToString();
                string destination = UploadPath + excelName;
                using (var workbook = SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                    // Adding style
                    WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                    stylePart.Stylesheet = GenerateStylesheet();
                    stylePart.Stylesheet.Save();

                    foreach (System.Data.DataTable table in ds.Tables)
                    {
                        var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                        var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                        sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);
                        DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                        string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);
                        uint sheetId = 1;
                        if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                        {
                            sheetId =
                                sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }
                        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                        sheets.Append(sheet);
                        DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                        List<String> columns = new List<string>();
                        foreach (System.Data.DataColumn column in table.Columns)
                        {
                            columns.Add(column.ColumnName);
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                            cell.StyleIndex = 2;
                            headerRow.AppendChild(cell);
                        }
                        sheetData.AppendChild(headerRow);
                        foreach (System.Data.DataRow dsrow in table.Rows)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                            int colindex = 0;
                            foreach (String col in columns)
                            {
                                DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                                if (table.Columns[colindex].DataType == Type.GetType("System.Decimal") || table.Columns[colindex].DataType == Type.GetType("System.Int64"))
                                {                                    
                                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString());
                                    cell.StyleIndex = 1;
                                }
                                else if (table.Columns[colindex].DataType == Type.GetType("System.DateTime"))
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dsrow[col]).Trim()))
                                    {
                                        DateTime dateTime = DateTime.Parse(dsrow[col].ToString());
                                        cell.CellValue = new CellValue(dateTime.ToString(GetLocalResourceObject("GST_DateFormat").ToString()));
                                    }
                                    cell.DataType = new EnumValue<CellValues>(CellValues.String);
                                    cell.StyleIndex = 1;                                                        
                                }
                                else
                                {
                                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                                    if (dsrow[col].ToString().StartsWith("="))//For identifying formula 
                                        cell.CellFormula = new DocumentFormat.OpenXml.Spreadsheet.CellFormula(dsrow[col].ToString());
                                    else
                                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString());
                                    cell.StyleIndex = 1;
                                }


                                newRow.AppendChild(cell);
                                colindex++;
                            }
                            sheetData.AppendChild(newRow);
                        }
                    }
                }

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + excelName);
                Response.BinaryWrite(File.ReadAllBytes(destination));
                File.Delete(destination);
                Response.End();
            }
            catch (Exception ex) { }

        }

        private DocumentFormat.OpenXml.Spreadsheet.Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 11 }

                ),
                new Font( // Index 1 - header
                    new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 11 },                    
                    new Bold()//,
                   // new Color() { Rgb = "FFFFFF" }

                ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new DocumentFormat.OpenXml.HexBinaryValue() { Value = "FFC14E" } }) { PatternType = PatternValues.Solid }) // Index 2 - header
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );
            Alignment alignment = new Alignment();
            alignment.Horizontal = HorizontalAlignmentValues.Center;
            alignment.Vertical = VerticalAlignmentValues.Center;

            //NumberingFormats numberingFormats = new NumberingFormats(
            //     new NumberingFormat(),
            //     new NumberingFormat() { NumberFormatId = (UInt32Value)176U, FormatCode = "dd-MM-yyyy" }
            //    );

            //NumberingFormats numberingFormats1 = new NumberingFormats() { Count = (UInt32Value)1U };
            //NumberingFormat numberingFormat1 = new NumberingFormat() { NumberFormatId = (UInt32Value)176U, FormatCode = "dd\\.mm\\.yyyy;@" };
            //numberingFormats1.Append(numberingFormat1);

            uint iExcelIndex = 164;
            NumberingFormats nfs = new NumberingFormats();
            NumberingFormat nfDateTime = new NumberingFormat();
            nfDateTime.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nfDateTime.FormatCode = StringValue.FromString("dd-mm-yyyy");
            nfs.Append(nfDateTime);
          
            NumberingFormat nfDecimal = new NumberingFormat();
            nfDecimal.NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++);
            nfDecimal.FormatCode = StringValue.FromString("#0.00");
            nfs.Append(nfDecimal);

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), //style index =0 default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, //style index =1 body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyBorder = true, ApplyFill = true, ApplyAlignment = true, Alignment = alignment }, // style index =2 header
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, FormatId=1, ApplyNumberFormat = true, NumberFormatId = nfDecimal.NumberFormatId } //style index =3 body
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);           
            return styleSheet;
        }

        private string CreateExcel()
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                return string.Empty;
            }
            string excelName = Guid.NewGuid().ToString() + GetLocalResourceObject("ExcelFormat").ToString();
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            //Here saving the file
            xlWorkBook.SaveAs(UploadPath + excelName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, misValue,
            misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            return excelName;
        }

        private void ExportDataSetToExcel(DataSet ds, string fileName)
        {

            //Creae an Excel application instance
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            //Create an Excel workbook instance and open it from the predefined location
            Microsoft.Office.Interop.Excel.Workbook excelWorkBook = excelApp.Workbooks.Open(UploadPath + fileName);
            foreach (DataTable table in ds.Tables)
            {
                //Add a new worksheet to workbook with the Datatable name
                Microsoft.Office.Interop.Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add(Before: excelWorkBook.Sheets[excelWorkBook.Sheets.Count - 2]);
                excelWorkSheet.Name = table.TableName;
                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                }
                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int k = 0; k < table.Columns.Count; k++)
                    {
                        excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                        if (table.Columns[k].DataType == Type.GetType("System.DateTime"))
                        {
                            excelWorkSheet.Cells[j + 2, k + 1].NumberFormat = GetLocalResourceObject("GST_DateFormat").ToString();
                        }
                    }
                }

                #region Setting excel header row style
                Microsoft.Office.Interop.Excel.Range usedRange = excelWorkSheet.UsedRange;
                Microsoft.Office.Interop.Excel.Range rows = usedRange.Rows;
                rows.Cells.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                foreach (Microsoft.Office.Interop.Excel.Range row in rows)
                {
                    Microsoft.Office.Interop.Excel.Range firstCell = row.Cells[1];
                    string firstCellValue = firstCell.Value as String;
                    if (!string.IsNullOrEmpty(firstCellValue))
                    {
                        row.Interior.Color = System.Drawing.Color.Orange;
                        row.Font.Bold = true;
                        break;
                    }
                }
                #endregion
            }
            //Set first sheet as active
            ((Microsoft.Office.Interop.Excel.Worksheet)excelApp.ActiveWorkbook.Sheets[1]).Select();

            excelWorkBook.Save();
            excelWorkBook.Close();
            excelApp.Quit();
            //Response.ContentType = "application/vnd.ms-excel";
            Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(File.ReadAllBytes(UploadPath + fileName));
            File.Delete(UploadPath + fileName);
            Response.End();
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
            //uclPaging.CurrentPage = 1;          
        }

        /// <summary>
        /// Button Load event
        /// Resets visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_Load(object sender, EventArgs e)
        {
            //(sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            //base.CheckBtnVisibility(sender);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }
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
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(System.Web.UI.Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(System.Web.UI.Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            REPORTTYPE,
            GSTRREPORTDATA

        }
        #endregion
    }
}