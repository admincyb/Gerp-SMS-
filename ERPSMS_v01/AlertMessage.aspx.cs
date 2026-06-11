using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.CommonManagement;
using BusinessObject.PurchaseOrderManagement;
using System.Web.Security;

namespace ERPSMS_v01
{
    public partial class AlertMessage : System.Web.UI.Page
    {
        BusinessObject.User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Type"] != null)
                {
                    AlertSettingBO curAlert;
                    switch (Request.QueryString["Type"].ToString())
                    {
                        case ApplicationType.PO:
                            curAlert = (AlertSettingBO)Session[ERP.Utilities.SessionStrings.PoAlert];
                            string xmlstr = GTIService.CommonFunctions.JsonToXml(BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPODetails(curAlert.TypePK));
                            PODetails poObj = ERP.Utilities.CommonFunctions.XmlDeserialize<PODetails>(xmlstr);
                            ucrAlert.TypeCode = ApplicationType.PO;
                            ucrAlert.TypePK = poObj.POH_PK;
                            ucrAlert.TypeRef = poObj.POH_NO;
                            ucrAlert.TrxDate =Convert.ToDateTime(poObj.POH_DATE);
                            ucrAlert.TypeText = curAlert.TypeText;
                            ucrAlert.TypePartyName = poObj.VEN_NAME;
                            ucrAlert.ReturnURL = curAlert.PageURL;
                            hdfRedirectURl.Value = curAlert.PageURL;
                            ucrAlert.GetAlertList();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                            break;
                    }
                }
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            currentUser = GetUserIdentity();
            // Set the User Theme
            if (!string.IsNullOrEmpty(currentUser.Theme))
            {
                Page.Theme = currentUser.Theme;
            }
            else
            {
                Page.Theme = "ClassicExt";
            }
            //Page.Theme = "NewTheme";

        }
        /// <summary>
        /// Get User Identity
        /// </summary>
        /// <returns></returns>
        public BusinessObject.User GetUserIdentity()
        {
            try
            {
                return (((BusinessObject.User)(HttpContext.Current.User.Identity)));
            }
            catch (Exception ex)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx");
            }
            return null;
        }
    }
}