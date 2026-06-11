using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Administration.Masters
{
    public partial class MachineryMaster : ERP.Store.UI.MyBasePage //System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillInitialData();
                FillMachineryDetails();
                ConfigurationSettings();

                if (Convert.ToInt32(hdfIsMultiplePlant.Value) > 0)
                {
                    divPlant.Visible = true;
                }
                else
                {
                    divPlant.Visible = false;
                }
            }
        }

        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";

            string ShowMachineMasureType = GetGlobalResourceObject("ConfigurationsRes", "ShowMachineMasureType").ToString();
            liMeasuringType.Visible = false;
            if (ShowMachineMasureType == "1")
                liMeasuringType.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillInitialData()
        {

            BusinessObject.MachineryManagement.Machinery ObjMachine = new BusinessObject.MachineryManagement.Machinery();
            ObjMachine.MaintenanceList = new List<BusinessObject.MachineryManagement.Machinery.MaintenaceInfo> { };
            //Assigning initialized Orderobject to hidden field (OrderObject)
            MaintenanceList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(ObjMachine);

            BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
            file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
            FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);

            USER_PK.Value = ((BusinessObject.User)(HttpContext.Current.User.Identity)).PKUser.ToString();

        }
        /// <summary>
        /// 
        /// </summary>
        private void FillMachineryDetails()
        {
            if (Request.QueryString["PK"] != null)
            {
                MCH_PK.Value = Request.QueryString["PK"];
            }
        }

    }
}