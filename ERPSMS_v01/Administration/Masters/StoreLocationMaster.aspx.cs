using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.Administration.Masters
{
    public partial class StoreLocationMaster : ERP.Store.UI.MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdfAppType.Value = ApplicationType.STRLOMS;
                hdfAppSubType.Value = string.Empty;               
            }
        }
    }
}