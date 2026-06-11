using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class DepartmentSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillInitialData();
            }
        }

        private void FillInitialData()
        {
            BusinessObject.Administration.Configurations.DepartmentSettings departmentSettings = new BusinessObject.Administration.Configurations.DepartmentSettings();
            departmentSettings.CFG_LIST = new List<BusinessObject.Administration.Configurations.ConfigList>();
            CFG_LIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(departmentSettings);
        }
    }
}