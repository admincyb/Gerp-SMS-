using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Administration.Masters;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class CurrencyMaster : ERP.Store.UI.MyBasePage
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
            BusinessObject.Administration.Masters.CurrencyMaster Currency = new BusinessObject.Administration.Masters.CurrencyMaster();
            //Currency. = new List<BusinessObject.DispersionManagement.Material>();
            Currency.ConversionList = new List<BusinessObject.Administration.Masters.Exchange>();
            CurrencyDetails.Value = Newtonsoft.Json.JsonConvert.SerializeObject(Currency);



            //ConversionList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(Dispersion);
        }
    }
}
