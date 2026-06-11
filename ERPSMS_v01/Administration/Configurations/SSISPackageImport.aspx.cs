using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Dts.Runtime;
using ERP.Utilities;
using System.IO;
using System.Data.SqlClient;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class SSISPackageImport : System.Web.UI.Page
    {
        #region Variables and Properties
        private string dbName;
        public static string message = string.Empty;
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

        protected void ActionHandler(object sender, EventArgs e)
        {
            ActionsEnum commonActions = ActionsEnum.DEFAULT;
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                switch (commonActions)
                {
                    case ActionsEnum.IMPORT:
                        string destinationDB = txtDestinationDB.Text.Trim();

                        if (destinationDB.IsNullOrEmptyOrWhitespace())
                            throw new ApplicationException("Destination DB can't be null");
                        if (!Enum.IsDefined(typeof(PackageType), ddlPackage.SelectedItem.Text.Trim()))
                            throw new ApplicationException("Package Not Available");
                        if (!fupExcelFileUploder.HasFile)
                            throw new ApplicationException("Select an Excel File");

                        PackageType currentPacakge = (PackageType)Enum.Parse(typeof(PackageType), ddlPackage.SelectedItem.Text.Trim());
                        string excelFile = UploadFile();

                        PackageInfo packageInfo = new PackageInfo();
                        packageInfo.DestinationDB = destinationDB;
                        packageInfo.ExcelFile = excelFile;
                        packageInfo.PackageType = currentPacakge;

                        SSISPackageManager packageManager = new SSISPackageManager(divMessage);
                        if (packageManager.ExecutePackage(packageInfo))
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Save_msg").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, currentPacakge.ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_Save_Msg").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, currentPacakge.ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;

                    case ActionsEnum.CLEAR:
                        ddlPackage.SelectedIndex = 0;
                     
                        divMessage.InnerText = string.Empty;
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            Response.Write(message);
        }

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
        enum ActionsEnum
        {
            IMPORT,
            CLEAR,
            DEFAULT
        }

        enum PackageType
        {
            Product = 0,
            Brand = 1
        }

        public void WriteMessage(string strMessage)
        {
            message += strMessage;
        }

        #region SSISPackageManager
        private sealed class SSISPackageManager
        {
            #region Properties & Variables
            List<Package> packages;
            Application app;
            DTSEventListener dtsEventListener;
            #endregion
          
            #region Constructors
            public SSISPackageManager(System.Web.UI.HtmlControls.HtmlGenericControl div)
            {
                app = new Application();
                app.PackagePassword = System.Configuration.ConfigurationManager.AppSettings["ProductPackagePassword"]; //"1234abcA";
                //app.
                
                div.InnerHtml = string.Empty;
                dtsEventListener = new DTSEventListener(div);
                packages = new List<Package>();
            }
            #endregion

            #region Public Methods
            public bool ExecutePackage(PackageInfo packageInfo)
            {
                try
                {
                    if (packageInfo.PackageType == PackageType.Product)
                        AddProductPackage(packageInfo);
                    else if (packageInfo.PackageType == PackageType.Brand)
                        AddBrandPackage(packageInfo);

                    bool result = Execute();
                    RemovePackages();
                    return result;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error Occured while importing package");
                }
            }
            #endregion

            #region Private Methods
            private void AddProductPackage(PackageInfo packageInfo)
            {
                string pkgLocation;
                string sourceExcelConnectionString;
                string destinationDbConnectionString;
                string sourceExcelName;                

                pkgLocation = HttpContext.Current.Server.MapPath("~/Administration/Configurations/SSISPackage/Products.dtsx");

                sourceExcelName = packageInfo.ExcelFile;
                sourceExcelConnectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""EXCEL 12.0;HDR=YES"";", sourceExcelName);
              //  destinationDbConnectionString = string.Format(@"Data Source=192.168.1.220\SQLSERVER2008;User ID=dev;password=gtidev@2008;Initial Catalog={0};Provider=SQLNCLI10.1;Persist Security Info=True;Auto Translate=False;Application Name=SSIS-Package-{{BA64BE05-F610-430F-B906-483FA4481A69}}192.168.1.220\SQLSERVER2008.gERP_DEV_140721;", packageInfo.DestinationDB);

                string conString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conString);
                string dataSource = builder.DataSource;
                string userId = builder.UserID;
                string password = builder.Password;
                string productPackageCode = System.Configuration.ConfigurationManager.AppSettings["ProductPackageCode"]; // "4DDFBD61-6F2A-412F-89E4-12F344D6F437";
                destinationDbConnectionString = string.Format(@"Data Source={0};User ID={1};password={2};Initial Catalog={3};Provider=SQLNCLI10.1;Persist Security Info=True;Auto Translate=False;Application Name=SSIS-Products-{{{4}}}{0}.{3};"
                    , dataSource, userId, password, packageInfo.DestinationDB, productPackageCode);
                destinationDbConnectionString = @"Data Source=192.168.1.188\SQLSERVER2008;User ID=sa;Initial Catalog=gERP_BWH_Test_150731_SH_CPY;Provider=SQLNCLI10.1;Persist Security Info=True;Application Name=SSIS-Products-{4DDFBD61-6F2A-412F-89E4-12F344D6F437}Destination_DB;";
                //SSISPackageImport tmp = new SSISPackageImport();
                /*tmp.WriteMessage("====== In ProductPackage:<br/>");
                tmp.WriteMessage(" pkgLocation:" + pkgLocation + "<br/>");
                tmp.WriteMessage(" sourceExcelName:" + sourceExcelName + "<br/>");*/
                //tmp.WriteMessage(" destinationDbConnectionString:" + destinationDbConnectionString + "<br/>");

                Package newPackage = app.LoadPackage(pkgLocation, dtsEventListener);
                newPackage.DelayValidation = true;
                newPackage.PackagePassword = "1234abcA";
                newPackage.PackageType = DTSPackageType.DTSDesigner100;
                newPackage.CreatorComputerName = "GTID41";
                newPackage.CreatorName = "GRANDTRUST\rasheed";
                newPackage.Connections["Source_Excel"].ConnectionString = sourceExcelConnectionString;
                //newPackage.Connections["Destination_DB"].ConnectionString = destinationDbConnectionString;
               // newPackage.Connections["Destination_DB"].Properties["Password"].SetValue(newPackage,"sqlserver2008++");
                newPackage.ForceExecutionResult = DTSForcedExecResult.None;
                packages.Add(newPackage);
              
            }
            private void AddBrandPackage(PackageInfo packageInfo)
            {
                string pkgLocation;
                string sourceExcelConnectionString;
                string destinationDbConnectionString;
                string sourceExcelName;

                pkgLocation = HttpContext.Current.Server.MapPath("~/Administration/Configurations/SSISPackage/Brand.dtsx");
                sourceExcelName = packageInfo.ExcelFile;
                sourceExcelConnectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""EXCEL 12.0;HDR=YES"";", sourceExcelName);
//                destinationDbConnectionString = string.Format(@"Data Source=192.168.1.220\SQLSERVER2008;User ID=dev;password=gtidev@2008;Initial Catalog={0};
//                                        Provider=SQLNCLI10.1;Persist Security Info=True;Auto Translate=False;
//                                        Application Name=SSIS-Package-{{BA64BE05-F610-430F-B906-483FA4481A69}}192.168.1.220\SQLSERVER2008.gERP_DEV_140721;"
//                    , packageInfo.DestinationDB);

                string conString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conString);
                string dataSource = builder.DataSource;
                string userId = builder.UserID;
                string password = builder.Password;
                string brandPackageCode = System.Configuration.ConfigurationManager.AppSettings["BrandPackageCode"]; // "BA64BE05-F610-430F-B906-483FA4481A69";
                destinationDbConnectionString = string.Format(@"Data Source={0};User ID={1};password={2};Initial Catalog={3};Provider=SQLNCLI10.1;Persist Security Info=True;Auto Translate=False;Application Name=SSIS-Package-{{{4}}}{0}.{3};"
                    , dataSource, userId, password, packageInfo.DestinationDB, brandPackageCode);

                

                SSISPackageImport tmp = new SSISPackageImport();
                tmp.WriteMessage("====== In ProductPackage:<br/>");
                tmp.WriteMessage(" pkgLocation:" + pkgLocation + "<br/>");
                tmp.WriteMessage(" sourceExcelName:" + sourceExcelName + "<br/>");
                tmp.WriteMessage(" destinationDbConnectionString:" + destinationDbConnectionString + "<br/>");

                Package newPackage = app.LoadPackage(pkgLocation, dtsEventListener);
                newPackage.Connections["Source_Excel"].ConnectionString = sourceExcelConnectionString;
                newPackage.Connections["Destination_DB"].ConnectionString = destinationDbConnectionString;
                newPackage.Connections["SP_BRAND_IMPORT.sql"].ConnectionString = HttpContext.Current.Server.MapPath("~/Administration/Configurations/SSISPackage/Script/SP_BRAND_IMPORT.sql");
                newPackage.ForceExecutionResult = DTSForcedExecResult.None;

                packages.Add(newPackage);                
            }
            private bool Execute()
            {
                bool result = false;
                foreach (Package pkg in packages)
                {
                    DTSExecResult pkgResults = pkg.Execute(null, null, dtsEventListener, null, null);

                    if (pkgResults == DTSExecResult.Success)
                        result = true;
                    else
                    {
                        result = false;
                        break;
                    }
                }
                return result;
            }
            private void RemovePackages()
            {
                packages.ForEach(x =>
                {
                    packages.Remove(x);
                });
            }
            #endregion
        }
        #endregion

        class DTSEventListener : DefaultEvents
        {          
            System.Web.UI.HtmlControls.HtmlGenericControl divM;
           
            public DTSEventListener(System.Web.UI.HtmlControls.HtmlGenericControl div)
            {
                divM = div;
            }
            public override bool OnError(DtsObject source, int errorCode, string subComponent,
              string description, string helpFile, int helpContext, string idofInterfaceWithError)
            {                
                divM.InnerHtml += string.Format("<b>Error:</b>{0}<br/>", description);
                return false;
            }
            public override void OnWarning(DtsObject source, int warningCode, string subComponent, string description, string helpFile, int helpContext, string idofInterfaceWithError)
            {
                base.OnWarning(source, warningCode, subComponent, description, helpFile, helpContext, idofInterfaceWithError);
               
                divM.InnerHtml += string.Format("<b>Warning:</b>{0}<br/>", description); // <br/>
            }
        }

        #region PackageInfo
        private struct PackageInfo
        {
            public string ExcelFile { get; set; }
            public string DestinationDB { get; set; }
            //public string PackageFile { get; set; }
            public PackageType PackageType { get; set; }
        }
        #endregion

        public enum ControlEnums
        {
           DBNAME
        }
    }
}