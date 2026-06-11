using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Administration.Configurations
{
    public partial class DefaultValueConfig : ERP.Store.UI.MyBasePage
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
            BusinessObject.Administration.Configurations.DefaultValueConfig defaultValueConfig = new BusinessObject.Administration.Configurations.DefaultValueConfig();
            defaultValueConfig.DFT_LIST = new List<BusinessObject.Administration.Configurations.DefaultValueConfigList>();
            DFT_LIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(defaultValueConfig);
        }
    }
}