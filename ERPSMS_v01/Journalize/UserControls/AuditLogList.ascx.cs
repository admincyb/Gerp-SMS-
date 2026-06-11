using BusinessObject.AccountManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Journalize.UserControls
{
    public partial class AuditLogList : System.Web.UI.UserControl
    {
        private ActionsEnum commonActions;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
//        protected void ActionHandler(object sender, EventArgs e)
//        {
//            if (sender.GetType().IsEquivalentTo(typeof(Button)))
//            {
//                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
//            }
//            switch (commonActions)
//            {
//                #region COMPARE
//                case ActionsEnum.ASSIGN:
//                    if (Session[ERP.Utilities.SessionStrings.TransactionType].ToString() != "DPVJ")
//                    {
//Session["SelRow"]=hdfsel                    }
//                    break;
//                    #endregion
//            }
//        }
            public void FillDetails(DataTable dtAuditDetails)
        {
            grdAuditLog.DataSource = dtAuditDetails;
            grdAuditLog.DataBind();
        }
        public void ResetGrivRowColor()
        {
            for (int i = 0; i < grdAuditLog.Rows.Count; i++)
            {
                grdAuditLog.Rows[i].BackColor = System.Drawing.Color.White;
            }
        }
        public void CurrentSelectedRowColor(int rowIndex)
        {
            for (int i = 0; i < grdAuditLog.Rows.Count; i++)
            {
                //int rowIndex = Convert.ToInt32(hdfSelRow.Value);
                if (i == rowIndex)
                {
                    grdAuditLog.Rows[i].BackColor = System.Drawing.Color.Yellow;
                }
                else
                {
                    grdAuditLog.Rows[i].BackColor = System.Drawing.Color.White;

                }
            }
        }
        protected void grdAuditLog_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAuditVersion = e.Row.FindControl("lblAuditVersion") as Label;
                HiddenField hdfFTH_PK = e.Row.FindControl("hdfFTH_PK") as HiddenField;
                HiddenField hdfRowIndex = e.Row.FindControl("hdfRowIndex") as HiddenField;
                HiddenField hdfRefType = e.Row.FindControl("hdfRefType") as HiddenField;
                e.Row.Attributes.Add("onclick", "ChangeRowColor(this,'" + lblAuditVersion.Text + "','" + hdfFTH_PK.Value + "','" + hdfRowIndex.Value + "','"+ hdfRefType.Value+"')");

            }
        }
        protected void ActionHandler(object sender, EventArgs e)
        {
            
        }
    }
}