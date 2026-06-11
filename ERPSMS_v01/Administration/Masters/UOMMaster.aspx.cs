using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Administration.Masters
{
    public partial class UOMMaster : ERP.Store.UI.MyBasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
           // if (!IsPostBack)
           // {
                FillInitialData();
           // }
        }
        /// <summary>
        /// Fill UOM Obje Serialized Date To hidden Field
        /// </summary>
        private void FillInitialData()
        {
            if (Request.QueryString["UOMID"] != null)
            {
                // Check QueryString 

            }
            else
            {
                BusinessObject.UOMManagement.UOM uomObject = new BusinessObject.UOMManagement.UOM();

                uomObject.ConversionList = new List<BusinessObject.UOMManagement.UOM.ConversionInfo>();

                ConversionList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(uomObject);
            }
        }
    }
}