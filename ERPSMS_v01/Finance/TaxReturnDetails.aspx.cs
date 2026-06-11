using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPSMS_v01.Administration.Masters;

namespace ERPSMS_v01.Finance
{
    public partial class TaxReturnDetails : System.Web.UI.Page
    {
        private ActionsEnum commonActions;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region -- For Buttons ---
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {

            if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
            {
                commonActions = ActionsEnum.SHOWDETAILS;
            }
            switch (commonActions)
            {
              
                #region Show Popup
                case ActionsEnum.PRINT:
                   // poPK = ((LinkButton)sender).CommandArgument;
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + "&APPTYPE=" + "GST" + "&APPSUBTYPE=7") + "');", true);

                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=1" + "&APPTYPE=" + "GST" + "&APPSUBTYPE=7") + "');", true);
                    break;
                #endregion
            }
        }


        #endregion
        protected void btnNext_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdditionalDetails.aspx");
        }
    }
}