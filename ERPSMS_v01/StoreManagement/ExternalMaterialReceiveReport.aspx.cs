using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using BusinessLogic;
using System.Data;
using System.IO;
using System.Threading;
using BusinessObject.CommonManagement;
using ERPData;
namespace ERPSMS_v01.StoreManagement
{
    public partial class ExternalMaterialReceiveReport : ERP.Store.UI.MyBasePage
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
                if (Request.QueryString["IssueID"] != null)
                {
                    FillReport(Convert.ToInt32(Request.QueryString["IssueID"]));
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        private DataTable ConfigurationSettingsforReport()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }
        private void FillReport(int ReqID)
        {
            RptMaterialIssue.Visible = true;
            ReportDataSource dsStoreReqHeader;
            ReportDataSource dsStoreReqDetail;
            LocalReport locRpt;
            RptMaterialIssue.LocalReport.DataSources.Clear();
            DataSet dsetStoreReqDtls = null;
            dsetStoreReqDtls = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetIssuingReportByReqId(ReqID);
            if (dsetStoreReqDtls != null)
            {
                DataTable dtRequestHeader = dsetStoreReqDtls.Tables[0];
                DataTable dtRequestDetail = dsetStoreReqDtls.Tables[1];
                if (dtRequestHeader.Rows.Count > 0 || dtRequestDetail.Rows.Count > 0)
                {
                    RptMaterialIssue.Visible = true;
                    dsStoreReqHeader = new ReportDataSource("StoreRequestHeader", dtRequestHeader);
                    dsStoreReqDetail = new ReportDataSource("StoreRequestDetail", dtRequestDetail);

                    locRpt = null;
                    locRpt = RptMaterialIssue.LocalReport;
                    locRpt.ReportPath = string.Empty;
                    locRpt.ReportPath = Server.MapPath("../Reports/ExternalMaterialReceipt.rdlc");
                    locRpt.EnableHyperlinks = true;
                    RptMaterialIssue.LocalReport.DataSources.Clear();
                    locRpt.EnableExternalImages = true;
                    ReportParameter parameters;
                    parameters = new ReportParameter("Title", currentUser.CurrentSBU);
                    locRpt.SetParameters(parameters);
                    object[] strArg = new object[2];
                    strArg[0] = currentUser.EmpName;
                    strArg[1] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                    parameters = new ReportParameter("FooterText", string.Format(this.GetGlobalResourceObject("Messages", "ReportFooterMessages").ToString(), strArg));
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
                    // currencyformat = {0:n} + currencydecimals;
                    parameters = new ReportParameter("DateFormat", Resources.Constants.ReportDateFormat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("CurrencyFormat", currencyformat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("NumberFormat", NoFormat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("QtyFormatPurchase", QtyforPurchase);
                    locRpt.SetParameters(parameters);

                    parameters = new ReportParameter("HeaderImage", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                    locRpt.SetParameters(parameters);

                    if (dtRequestHeader != null & dtRequestHeader.Rows.Count > 0)
                    {
                        int CompanyPK = Convert.ToInt32(dtRequestHeader.Rows[0]["ICH_BIZUNIT"].ToString());
                        RptMaterialIssue.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
                    }

                    RptMaterialIssue.LocalReport.DataSources.Add(dsStoreReqHeader);
                    RptMaterialIssue.LocalReport.DataSources.Add(dsStoreReqDetail);
                    RptMaterialIssue.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    RptMaterialIssue.LocalReport.Refresh();

                    SavePDF(locRpt, "EMR" + ReqID);
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
                    RptMaterialIssue.Visible = false;
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
        private ReportDataSource GetCompanyDetails(int CmpnyPk)
        {
            ReportDataSource CompanyDtls = null;
            currentEntity = new ERPEntities();
            List<SPADM_COMPANY_MST_GET_KV_Result> CompanyList = currentEntity.SPADM_COMPANY_MST_GET_KV(CmpnyPk, Convert.ToByte(DbActiveStatus.HASPK), null, null,null).ToList();

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
    }
}