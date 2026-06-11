using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Production
{
    public partial class TopUpRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillInitialData();
                FillMachineryDetails();
            }
        }

        private void FillInitialData()
        {


            BusinessObject.Production.TopUpRecord objTopUp = new BusinessObject.Production.TopUpRecord();
            objTopUp.ItemList = new List<BusinessObject.Production.TopUpRecord.ItemListDetails>();
            ItemList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(objTopUp);

           

        }
        /// <summary>
        /// 
        /// </summary>
        private void FillMachineryDetails()
        {
            if (Request.QueryString["PK"] != null)
            {
                TUH_PK.Value = Request.QueryString["PK"];
            }
        }
    }
}