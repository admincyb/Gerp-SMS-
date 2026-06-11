using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class DispersionMaster : ERP.Store.UI.MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hdfShowPrefix.Value = GetGlobalResourceObject("ConfigurationsRes", "IsDispersionPrefixVisible").ToString();
            FillInitialData();
        }

        private void FillInitialData()
        {
            BusinessObject.DispersionManagement.Dispersion Dispersion = new BusinessObject.DispersionManagement.Dispersion();
            Dispersion.Materials = new List<BusinessObject.DispersionManagement.Material>();
            Dispersion.ConversionList = new List<BusinessObject.DispersionManagement.ConversionInfo>();
            DispersionDetailsList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(Dispersion);



            //ConversionList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(Dispersion);
        }
    }


}