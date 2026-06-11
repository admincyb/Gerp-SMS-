using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Finance
{
    public partial class IndustryCodes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            //Next
            Response.Redirect("TaxDeclaration.aspx");
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            //Previous
            Response.Redirect("AdditionalDetails.aspx");
        }
    }
}