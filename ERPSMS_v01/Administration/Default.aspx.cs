using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Administration
{
    public partial class Default : System.Web.UI.Page
    {
        public enum DeptTypes
        {
            MaterialCategories = 1,
            UOMTypes = 2,
            MaterialTypes = 3,
            StatusValues = 4
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
    }
}