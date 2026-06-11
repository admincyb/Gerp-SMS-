using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Xml;
using System.IO;
using BusinessObject.CommonManagement;
using System.Threading;
using ERPData;

namespace ERPSMS_v01.StoreManagement 
{

    public partial class StockTransferReport : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;

        private string attachmentFilePath;
        private string attachmentFileFormat;
        private string attachmentFileContentType;
        private string attachmentFileName;
        private ERPEntities currentEntity;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                if (Request.QueryString["PK"] != null)
                {
                    FillReport(Convert.ToInt32(Request.QueryString["PK"]));
                }
            }
        }
        private DataTable ConfigurationSettingsforReport()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }
        /// <summary>
        /// To Get Company Details
        /// </summary>
        private ReportDataSource GetCompanyDetails(int CmpnyPk)
        {
            ReportDataSource CompanyDtls = null;
            currentEntity = new ERPEntities();
            List<SPADM_COMPANY_MST_GET_KV_Result> CompanyList = currentEntity.SPADM_COMPANY_MST_GET_KV(CmpnyPk, Convert.ToByte(DbActiveStatus.HASPK),null,null,null).ToList();

            if (CompanyList != null && CompanyList.Count > 0)
            {
                if (Convert.ToString(CompanyList[0].CMP_LOGO) != string.Empty)
                {
                    bool fileExists = false;
                    string LogoPath = string.Empty;
                    // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN); 
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        if (File.Exists(Server.MapPath(Resources.Controls.LogoPath) + CompanyList[0].CMP_LOGO))
                        {
                            LogoPath = "file:///" + Server.MapPath(Resources.Controls.LogoPath) + CompanyList[0].CMP_LOGO;
                            fileExists = true;
                        }
                    }
                    else
                    {
                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_LOGO))
                        {
                            LogoPath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_LOGO;
                            fileExists = true;
                        }
                    }
                    if (!fileExists)
                    {
                        LogoPath = string.Empty;
                    }
                    CompanyList[0].CMP_LOGO = LogoPath;
                }



            }
            CompanyDtls = new ReportDataSource("CompanyDtls", CompanyList);
            return CompanyDtls;
        }

        /// <summary>
        /// To display report
        /// </summary>
        /// <param name="storeAdjID"></param>
        private void FillReport(int stockTrfID)
        {
            rvStockTransfer.Visible = true;
            LocalReport locRpt;
            string stockTrfDtls;
            DataSet dsStockTrfDetails = new DataSet();

            stockTrfDtls = BusinessLogic.StoreManagement.StockTransferBL.GetStockTransferRptDetails(stockTrfID);
            dsStockTrfDetails.ReadXml(new XmlTextReader(new StringReader(stockTrfDtls)));
            if (dsStockTrfDetails != null)
            {
                if (dsStockTrfDetails.Tables.Count > 0)
                {
                    int CompanyPK = 0;
                    rvStockTransfer.Visible = true;
                    ReportDataSource dsStockTrfHeader = new ReportDataSource("StockTrfHeader", dsStockTrfDetails.Tables["Root"] == null ? new DataTable() : dsStockTrfDetails.Tables["Root"]);
                    ReportDataSource dsPODetail = new ReportDataSource("PODetail", dsStockTrfDetails.Tables["PODetail"] == null ? new DataTable() : dsStockTrfDetails.Tables["PODetail"]);
                    ReportDataSource dsPRDetail = new ReportDataSource("PRDetail", dsStockTrfDetails.Tables["PRDetail"] == null ? new DataTable() : dsStockTrfDetails.Tables["PRDetail"]);
                    ReportDataSource dsAddnlDetail = new ReportDataSource("AddnlDetail", dsStockTrfDetails.Tables["AdditionalDetail"] == null ? new DataTable() : dsStockTrfDetails.Tables["AdditionalDetail"]);
                    if (dsStockTrfDetails.Tables["Root"] != null & dsStockTrfDetails.Tables["Root"].Rows.Count > 0)
                    {
                        CompanyPK = Convert.ToInt32(dsStockTrfDetails.Tables["Root"].Rows[0]["SFH_COMPANY"].ToString());
                    }
                    locRpt = null;
                    locRpt = rvStockTransfer.LocalReport;
                    locRpt.ReportPath = string.Empty;
                    locRpt.ReportPath = Server.MapPath("../Reports/StockTransferReport.rdlc");
                    rvStockTransfer.Height = 500;
                    rvStockTransfer.LocalReport.DataSources.Clear();
                    locRpt.EnableExternalImages = true;
                    ReportParameter parameters;
                    parameters = new ReportParameter("Title", currentUser.CurrentSBU);
                    locRpt.SetParameters(parameters);
                    int RowCount;
                    int PRRowCount;
                    if (dsStockTrfDetails.Tables["AdditionalDetail"] == null)
                    {
                        RowCount = 0;
                    }
                    else
                    {
                        RowCount = 1;
                    }
                    if (dsStockTrfDetails.Tables["PRDetail"] == null)
                    {
                        PRRowCount = 0;
                    }
                    else
                    {
                        PRRowCount = 1;
                    }
                    parameters = new ReportParameter("rowcount", RowCount.ToString());
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("PRRowCount", PRRowCount.ToString());
                    locRpt.SetParameters(parameters);
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    //string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
                    string currencyformat = "#" + currencysep + "#0.";
                    string NoFormat = "#" + currencysep + "#0.";
                    string QtyforPurchase = "#" + currencysep + "#0.";
                    string currencydecimals = "";
                    string Nodecimal = string.Empty;
                    string QtyDecforPuchase = string.Empty;
                    DataTable dt = ConfigurationSettingsforReport();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int curdigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < curdigit; i++)
                        {
                            currencydecimals += "0";
                        }

                        int NoDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());

                        for (int i = 0; i < NoDigit; i++)
                        {
                            Nodecimal += "0";
                        }
                        int QtyDecimalPurchase = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "NumberDecimalDigitP2P")["ACF_VALUE"].ToString());
                        for (int i = 0; i < QtyDecimalPurchase; i++)
                        {
                            QtyDecforPuchase += "0";
                        }
                    }
                    else
                    {
                        currencydecimals = "00";
                        Nodecimal = "00";
                    }
                    currencyformat = currencyformat + currencydecimals;
                    NoFormat = NoFormat + Nodecimal;
                    QtyforPurchase = QtyforPurchase + QtyDecforPuchase;
                    parameters = new ReportParameter("DateFormat", Resources.Constants.ReportDateFormat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("CurrencyFormat", currencyformat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("NumberFormat", NoFormat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("QtyFormatPurchase", QtyforPurchase);
                    locRpt.SetParameters(parameters);
                    object[] strArg = new object[2];
                    strArg[0] = currentUser.EmpName;
                    strArg[1] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                    parameters = new ReportParameter("FooterText", string.Format(this.GetGlobalResourceObject("Messages", "ReportFooterMessages").ToString(), strArg));
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("HeaderImage", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                    locRpt.SetParameters(parameters);
                    rvStockTransfer.LocalReport.DataSources.Add(dsStockTrfHeader);
                    rvStockTransfer.LocalReport.DataSources.Add(dsPODetail);
                    rvStockTransfer.LocalReport.DataSources.Add(dsPRDetail);
                    rvStockTransfer.LocalReport.DataSources.Add(dsAddnlDetail);
                    rvStockTransfer.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                    rvStockTransfer.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    rvStockTransfer.LocalReport.Refresh();

                    SavePDF(locRpt, "StockTransfer" + stockTrfID);
                    if (File.Exists(attachmentFilePath))
                    {

                        Response.ClearContent();
                        Response.ContentType = "application/pdf";
                        //Response.AddHeader("content-Disposition", "attachment;filename=" + attachmentFileName);
                        //Response.TransmitFile(attachmentFilePath);
                        Response.Redirect(Resources.PageURL.PDFUrl + attachmentFileName);
                        Response.Flush();

                    }
                }
                else
                {
                    rvStockTransfer.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "json", "$(document).ready(function() { showEmptyDataMsgBox();});", true);
                }
            }
        }


        /// <summary>
        /// Convert to & save pdf
        /// </summary>
        private void SavePDF(LocalReport locRpt, string Reportname)
        {
            try
            {
                string mimeType;
                string encoding;
                string extension;
                string[] streamids;
                Microsoft.Reporting.WebForms.Warning[] warnings;
                string format;
                string savePath;

                savePath = Server.MapPath("~/") + Resources.PageURL.OfflineTestDocs;
                attachmentFilePath = string.Empty;
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
                attachmentFileName = Reportname + ".pdf";
                attachmentFilePath = savePath + attachmentFileName;
                attachmentFileFormat = ".pdf";
                attachmentFileContentType = "application/pdf";

                //if file is exists delete file
                if (File.Exists(attachmentFilePath))
                    File.Delete(attachmentFilePath);

                format = "PDF";
                byte[] bytes = locRpt.Render(format, "", out mimeType, out encoding, out extension, out streamids, out warnings);
                /* stream to use for attachment - can implement later
                Stream stream = new MemoryStream();
                stream.Write(bytes, 0, bytes.Length);
                SendMail(stream);
                 */
                //save the pdf byte to the folder
                FileStream fs = new FileStream(attachmentFilePath, FileMode.OpenOrCreate);
                byte[] data = new byte[fs.Length];
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}