using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using System.IO;
using Microsoft.SqlServer.Dts.Runtime;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class SSISPackageImport2 : System.Web.UI.Page
    {
        #region Variables and Properties
        private string dbName;
        string message = string.Empty;
        string Exmsg = string.Empty;
        // public static string message = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        #region PageActionHandler
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    GetFieldValues(ControlEnums.DBNAME);
                    SetFieldValues(ControlEnums.DBNAME);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls
        /// </summary>
        private void GetFieldValues(ControlEnums controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlEnums.DBNAME:
                    string conString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    System.Data.IDbConnection connection = new System.Data.SqlClient.SqlConnection(conString);
                    dbName = connection.Database;
                    break;
            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// </summary>
        private void SetFieldValues(ControlEnums controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlEnums.DBNAME:
                    txtDestinationDB.Text = dbName;
                    break;
            }
        }
        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            ActionsEnum commonActions = ActionsEnum.DEFAULT;
            string excelFilePath = string.Empty;
            try
            {
                Application app = new Application();
                Package package = null;
                string packageFileFullPath = string.Empty; // can be change according to Package Type
                string packageApplicationName = string.Empty; // can be change according to Package Type


                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region IMPORT
                    case ActionsEnum.IMPORT:
                        string destinationDB = txtDestinationDB.Text.Trim();
                        excelFilePath = UploadFile();
                        PackageType currentPacakge = (PackageType)Enum.Parse(typeof(PackageType), ddlPackage.SelectedItem.Text.Trim());
                        switch (currentPacakge)
                        {
                            case PackageType.Product:
                                Response.Write("<br/>ProductPackage");
                                packageFileFullPath = HttpContext.Current.Server.MapPath("~/Administration/Configurations/SSISPackage/Products.dtsx");
                                packageApplicationName = System.Configuration.ConfigurationManager.AppSettings["ProductPackageApplicationName"];
                                break;
                            case PackageType.Brand:
                                packageFileFullPath = HttpContext.Current.Server.MapPath("~/Administration/Configurations/SSISPackage/Brand.dtsx");
                                packageApplicationName = System.Configuration.ConfigurationManager.AppSettings["BrandPackageApplicationName"];
                                break;
                            default:
                                break;
                        }
                        Response.Write("<br/>excelFilePath: " + excelFilePath);
                        Response.Write("<br/>packageFileFullPath: " + packageFileFullPath);
                        DtsEventClass eventClass1 = new DtsEventClass(this, "<br/>FromLoadPackage:");
                       // package=app.LoadPackage(packageFileFullPath, eventClass1);
                        package = app.LoadPackage(packageFileFullPath, null);
                        Response.Write("<br/>After Load Package");
                        foreach (var item in package.Connections)
                        {
                            Response.Write("<br/>Foreach");
                            if (item.CreationName == "EXCEL")
                            {
                                Response.Write("<br/>Excel");
                                item.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFilePath + ";Extended Properties=\"EXCEL 12.0;HDR=YES\";";
                                Response.Write("Excel:" + item.ConnectionString + "</br>");
                            }
                            if (item.CreationName == "OLEDB")
                            {
                                Response.Write("<br/>Oledb");
                                string conStringWebConfig = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(conStringWebConfig);
                                string dataSource = builder.DataSource;
                                string userId = builder.UserID;
                                string password = builder.Password;
                                string dataBase = builder.InitialCatalog;
                                string conString = string.Format(@"Data Source={0};User ID={1};Password={2};Initial Catalog={3};Provider=SQLNCLI10.1;Auto Translate=False;Application Name={4}192.168.1.220\SQLSERVER2008.gERP_DEV_150713;"
                                   , dataSource, userId, password, dataBase, packageApplicationName);
                                item.ConnectionString = conString;
                                Response.Write("OLEDB:" + item.ConnectionString + "</br>");
                            }
                        }
                        Response.Write("<br/>package.Execute");
                        DtsEventClass eventClass = new DtsEventClass(this);
                        Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute(package.Connections, null, eventClass, null, null);
                        if (results == DTSExecResult.Success)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Save_msg").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, currentPacakge.ToString());
                            WriteMessage(litErrorMsg.Text);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_Save_Msg").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, currentPacakge.ToString());
                            WriteMessage(litErrorMsg.Text);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        divMessage.InnerHtml = message;

                        break; 
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ddlPackage.SelectedIndex = 0;
                        divMessage.InnerText = string.Empty;
                        break; 
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GetFullExMessage(ex);               
                Response.Write(Exmsg);
                divMessage.InnerHtml = message;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            //if (File.Exists(excelFilePath)) File.Delete(excelFilePath);
        }
        #endregion

        #region UploadFile
        private string UploadFile()
        {
            string savePath = string.Empty;
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
            {
                savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"Upload\Package\Excel";
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
                savePath = HttpContext.Current.Request.PhysicalApplicationPath + @"Upload\Package\Excel\";
            }
            else
            {
                savePath = string.Format(@"{0}Package\Excel\", System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower());
            }

            FileInfo postedFileInfo = new FileInfo(fupExcelFileUploder.PostedFile.FileName);
            string fileNameToUpload = string.Format(@"{0}{1}{2}", savePath, Guid.NewGuid().ToString(), postedFileInfo.Extension);
            fupExcelFileUploder.PostedFile.SaveAs(fileNameToUpload);            
            return fileNameToUpload;
        }
        #endregion

        #region WriteMessage
        public void WriteMessage(string msg)
        {
            message += (msg + "<br/>");
        }
        #endregion

        #region DtsEventClass
        class DtsEventClass : IDTSEvents
        {
            SSISPackageImport2 frm = new SSISPackageImport2();
            string strFrom = string.Empty;
            public DtsEventClass(SSISPackageImport2 frm1, string fr="")
            {
                frm = frm1;
                strFrom = fr;
            }

            public void OnBreakpointHit(IDTSBreakpointSite breakpointSite, BreakpointTarget breakpointTarget)
            {
                //throw new NotImplementedException();
            }

            public void OnCustomEvent(TaskHost taskHost, string eventName, string eventText, ref object[] arguments, string subComponent, ref bool fireAgain)
            {
                //throw new NotImplementedException();
            }

            public bool OnError(DtsObject source, int errorCode, string subComponent, string description, string helpFile, int helpContext, string idofInterfaceWithError)
            {
                frm.WriteMessage("<b>" + strFrom + "Error:</b> " + description);
                return true;
                // throw new NotImplementedException();
            }

            public void OnExecutionStatusChanged(Executable exec, DTSExecStatus newStatus, ref bool fireAgain)
            {
                // throw new NotImplementedException();
            }

            public void OnInformation(DtsObject source, int informationCode, string subComponent, string description, string helpFile, int helpContext, string idofInterfaceWithError, ref bool fireAgain)
            {
                frm.WriteMessage("<b>" + strFrom + "OnInformation:</b> " + description);
                // throw new NotImplementedException();
            }

            public void OnPostExecute(Executable exec, ref bool fireAgain)
            {
                //  frm.WriteMessage("OnPostExecute: ");
                // throw new NotImplementedException();
            }

            public void OnPostValidate(Executable exec, ref bool fireAgain)
            {
                // throw new NotImplementedException();
            }

            public void OnPreExecute(Executable exec, ref bool fireAgain)
            {
                // throw new NotImplementedException();
            }

            public void OnPreValidate(Executable exec, ref bool fireAgain)
            {
                // throw new NotImplementedException();
            }

            public void OnProgress(TaskHost taskHost, string progressDescription, int percentComplete, int progressCountLow, int progressCountHigh, string subComponent, ref bool fireAgain)
            {
                // throw new NotImplementedException();
            }

            public bool OnQueryCancel()
            {
                return false;
                // throw new NotImplementedException();
            }

            public void OnTaskFailed(TaskHost taskHost)
            {
                frm.WriteMessage("<b>" + strFrom + "OnTaskFailed:</b> " + taskHost.Description);
                // divMessage.InnerHtml = message;
              //  frm.WriteMessage("Failed: ");
                //throw new NotImplementedException();
            }

            public void OnVariableValueChanged(DtsContainer DtsContainer, Variable variable, ref bool fireAgain)
            {
                //throw new NotImplementedException();
            }

            public void OnWarning(DtsObject source, int warningCode, string subComponent, string description, string helpFile, int helpContext, string idofInterfaceWithError)
            {
                frm.WriteMessage("<b>" + strFrom + "Warning:</b> " + description);
                //throw new NotImplementedException();
            }
        }
        #endregion

        #region ActionsEnum
        enum ActionsEnum
        {
            IMPORT,
            CLEAR,
            DEFAULT
        }
        #endregion

        #region ControlEnums
        public enum ControlEnums
        {
            DBNAME
        }
        #endregion

        #region PackageType
        enum PackageType
        {
            Product = 0,
            Brand = 1
        }
        #endregion

        private void GetFullExMessage(Exception ex)
        {
            Exmsg += ("<br/>" + ex.Message);
            if (ex.InnerException!=null)
            {
                GetFullExMessage(ex.InnerException);
            }
        }

    }
}