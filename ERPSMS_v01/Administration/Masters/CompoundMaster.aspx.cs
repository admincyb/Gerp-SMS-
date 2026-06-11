using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Administration.Masters
{
    public partial class CompoundMaster : ERP.Store.UI.MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillInitialData();
            }
        }
        /// <summary>
        /// fill details
        /// </summary>
        private void FillInitialData()
        {
            if (Request.QueryString["PK"] != null)
            {
                COM_PK.Value = Request.QueryString["PK"].ToString();
                BusinessObject.CompoundManagement.Compound compObject = new BusinessObject.CompoundManagement.Compound();
                compObject.ConversionList = new List<BusinessObject.CompoundManagement.ConversionInfo>();
                ConversionList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(compObject);
                compObject.CompoundMaterialsList = new List<BusinessObject.CompoundManagement.CompoudMaterials>();
                CompoundMaterialsList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(compObject);

            }
            else
            {
                BusinessObject.CompoundManagement.Compound compObject = new BusinessObject.CompoundManagement.Compound();
                compObject.ConversionList = new List<BusinessObject.CompoundManagement.ConversionInfo>();
                ConversionList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(compObject);
                compObject.CompoundMaterialsList = new List<BusinessObject.CompoundManagement.CompoudMaterials>();
                CompoundMaterialsList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(compObject);


            }
        }
    }
}
