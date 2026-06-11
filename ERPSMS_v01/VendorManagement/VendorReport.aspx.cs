using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Threading;
using BusinessObject.CommonManagement;
using System.IO;
using ERPData;
using ERPService;

namespace ERPSMS_v01.VendorManagement
{
    public partial class VendorReport : System.Web.UI.Page
    {

        BusinessObject.User currentUser;

        private string attachmentFilePath;
        private string attachmentFileFormat;
        private string attachmentFileContentType;
        private string attachmentFileName;
        private ERPEntities currentEntity;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
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
        /// fill Report Details
        /// </summary>
        /// <param name="vendorPK"></param>
        private void FillReport(int vendorPK)
        {
            RptViewerReport.Visible = true;
            LocalReport locRpt;
            ReportParameter parameters;
            string signaturePath = string.Empty;
            RptViewerReport.LocalReport.DataSources.Clear();
            try
            {
                #region commented
                //DataSet dsVendorDtls = BusinessLogic.VendorManagement.VendorRegistration.GetVendorDetailsReport(vendorPK);
                //if (dsVendorDtls != null)
                //{
                //    DataTable dtVendorDtls = dsVendorDtls.Tables[0];
                //    if (dtVendorDtls.Rows.Count > 0)
                //    {
                //        RptViewerReport.Visible = true;
                //        ReportDataSource rdsVendorHdr = new ReportDataSource("VendorHdrDtls", dtVendorDtls);
                //        ReportDataSource rdsVendorContactDtls = new ReportDataSource("VendorCondatctDtls", dsVendorDtls.Tables["AddressBookDetails"] == null ? new DataTable() : dsVendorDtls.Tables["AddressBookDetails"]);
                //        ReportDataSource rdsVendorTermsDtls = new ReportDataSource("VendorTermsDtls", dsVendorDtls.Tables["TermsDetails"] == null ? new DataTable() : dsVendorDtls.Tables["TermsDetails"]);
                //        locRpt = null;
                //        locRpt = RptViewerReport.LocalReport;
                //        locRpt.ReportPath = string.Empty;
                //        locRpt.ReportPath = Server.MapPath("../Reports/VendorReport.rdlc");
                //        RptViewerReport.Height = 500;
                //        RptViewerReport.LocalReport.DataSources.Clear();
                //        locRpt.EnableExternalImages = true;
                //        ReportParameter parameters;
                //        parameters = new ReportParameter("Title", currentUser.CurrentSBU);
                //        locRpt.SetParameters(parameters);
                //        object[] strArg = new object[2];
                //        strArg[0] = currentUser.UserName;
                //        strArg[1] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                //        parameters = new ReportParameter("FooterText", string.Format(this.GetGlobalResourceObject("Messages", "ReportFooterMessages").ToString(), strArg));
                //        locRpt.SetParameters(parameters);
                //        parameters = new ReportParameter("HeaderImage", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                //        locRpt.SetParameters(parameters);
                //        RptViewerReport.LocalReport.DataSources.Add(rdsVendorHdr);
                //        RptViewerReport.LocalReport.DataSources.Add(rdsVendorContactDtls);
                //        RptViewerReport.LocalReport.DataSources.Add(rdsVendorTermsDtls);
                //        RptViewerReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                //        RptViewerReport.LocalReport.Refresh();
                //    }
                //    else
                //    {
                //        RptViewerReport.Visible = false;
                //    }
                //}

                //GetFieldValues(ControlsEnum.REPORT);
                #endregion
                AppTypeDetailsList = new List<SPADM_APP_SUB_TYPE_DATA_GET_Result>();
                cm = new CommonService();
                string rptName = string.Empty;
                AppTypeDetailsList = cm.GetReportParameters("VND", 0, DateTime.Now.Date);
                DataSet dsVendorDtls = BusinessLogic.VendorManagement.VendorRegistration.GetVendorPerformanceReport(vendorPK);
                DataSet dsAppSubtype = BusinessLogic.VendorManagement.VendorRegistration.GetAppSubType("VND");
               
                if (dsVendorDtls != null)
                {
                    DataTable dtVendorDtls = dsVendorDtls.Tables[0];
                    if (dtVendorDtls.Rows.Count > 0)
                    {
                        RptViewerReport.Visible = true;
                        ReportDataSource ReportDtls = new ReportDataSource("ReportDtls", dtVendorDtls);
                        ReportDataSource ReportSubDtls = new ReportDataSource("ReportSubDtls", dsVendorDtls.Tables[1] == null ? null : dsVendorDtls.Tables[1]);
                        ReportDataSource ReportOtherDtls = new ReportDataSource("ReportOtherDtls", dsVendorDtls.Tables[2] == null ? null : dsVendorDtls.Tables[2]);
                        locRpt = null;
                        locRpt = RptViewerReport.LocalReport;
                        locRpt.ReportPath = string.Empty;
                        foreach (SPADM_APP_SUB_TYPE_DATA_GET_Result sa in AppTypeDetailsList)
                        {
                            rptName = sa.AST_OP_FILE1;
                            locRpt.ReportPath = Server.MapPath("../Reports/" + rptName);
                            RptViewerReport.Height = 500;
                            RptViewerReport.LocalReport.DataSources.Clear();
                            locRpt.EnableExternalImages = true;
                            parameters = new ReportParameter("QMSRef", sa.AST_QMS_REF);
                            locRpt.SetParameters(parameters);
                            parameters = new ReportParameter("HideQMSRef", sa.AST_QMS_VISIBILITY.ToString());
                            locRpt.SetParameters(parameters);
                        }
                        object[] strArg = new object[2];
                        strArg[0] = currentUser.EmpName;
                        strArg[1] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                        parameters = new ReportParameter("FooterText", string.Format(this.GetGlobalResourceObject("Messages", "ReportFooterMessages").ToString(), strArg));
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("Logo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                        locRpt.SetParameters(parameters);


                        string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                        //string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
                        string currencyformat = "#" + currencysep + "#0.";
                        string NoFormat = "#" + currencysep + "#0.";
                        string currencydecimals = "";
                        string Nodecimal = string.Empty;
                        DataTable dt = ConfigurationSettingsforReport();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            int curdigit = Convert.ToInt32(dt.Rows[0]["ACF_VALUE"].ToString());
                            for (int i = 0; i < curdigit; i++)
                            {
                                currencydecimals += "0";
                            }

                            int NoDigit = Convert.ToInt32(dt.Rows[3]["ACF_VALUE"].ToString());

                            for (int i = 0; i < NoDigit; i++)
                            {
                                Nodecimal += "0";
                            }
                        }
                        else
                        {
                            currencydecimals = "00";
                            Nodecimal = "00";
                        }
                        currencyformat = currencyformat + currencydecimals;
                        NoFormat = NoFormat + Nodecimal;
                        parameters = new ReportParameter("DateFormat", Resources.Constants.ReportDateFormat);
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("CurrencyFormat", currencyformat);
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("NumberFormat", NoFormat);
                        locRpt.SetParameters(parameters);

                        parameters = new ReportParameter("CompanyName", currentUser.CurrentSBU);
                        locRpt.SetParameters(parameters);
                        RptViewerReport.LocalReport.DataSources.Add(ReportDtls);
                        if(ReportSubDtls!=null)
                        RptViewerReport.LocalReport.DataSources.Add(ReportSubDtls);
                        if (ReportOtherDtls != null)
                        RptViewerReport.LocalReport.DataSources.Add(ReportOtherDtls);
                        RptViewerReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        RptViewerReport.LocalReport.Refresh();
                        if (dsAppSubtype != null && dsAppSubtype.Tables.Count > 0)
                        {

                            if (Convert.ToString(dsAppSubtype.Tables[0].Rows[0]["AST_APPROVED_SIGN"]) != string.Empty)
                            {
                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                {
                                    signaturePath = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + dsAppSubtype.Tables[0].Rows[0]["AST_APPROVED_SIGN"];
                                }
                                else
                                {
                                    signaturePath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + dsAppSubtype.Tables[0].Rows[0]["AST_APPROVED_SIGN"];
                                }

                                parameters = new ReportParameter("ApprovedBySign", signaturePath);
                                locRpt.SetParameters(parameters);
                            }

                            if (Convert.ToString(dsAppSubtype.Tables[0].Rows[0]["AST_APPROVED_SIGN1"]) != string.Empty)
                            {
                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                {
                                    signaturePath = "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + dsAppSubtype.Tables[0].Rows[0]["AST_APPROVED_SIGN1"];
                                }
                                else
                                {
                                    signaturePath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + dsAppSubtype.Tables[0].Rows[0]["AST_APPROVED_SIGN1"];
                                }
                                parameters = new ReportParameter("ApprovedBySign1", signaturePath);
                                locRpt.SetParameters(parameters);
                            }
                        }
                        SavePDF(locRpt);
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
                        RptViewerReport.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Convert to & save pdf
        /// </summary>
        private void SavePDF(LocalReport locRpt)
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
                attachmentFileName = "VendorsList.pdf";
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