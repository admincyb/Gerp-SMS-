using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Administration.Masters;

namespace ERPSMS_01.Administration.Masters
{
    public partial class TermsTemplate : ERP.Store.UI.MyBasePage
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
            if (Request.QueryString["TemplateID"] != null)
            {

                //Assigning initialized Dispersion to hidden field (DispersionObject)
                TemplateDetails.Value = BusinessLogic.DispersionManagement.DispersionMaster.GetDispersionDetails(Convert.ToInt32(Request.QueryString["DispersionID"].ToString()), Convert.ToInt32(Request.QueryString["DepartmentID"].ToString()));
            }
            else
            {
                GeneralTemplateMaster dispObject = new GeneralTemplateMaster();
                dispObject.TemplateDetails = new List<TemplateDetail>();
                //Assigning initialized Dispersionobject to hidden field (DispersionObject)
                TemplateDetails.Value = Newtonsoft.Json.JsonConvert.SerializeObject(dispObject);
            }
        }
    }
}