using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ERPManager;
using ERP.Utilities;

namespace ERPSMS_v01.UserControls
{
    public partial class CheckListSearchControl : System.Web.UI.UserControl
    {
        #region Properties
        public List<DDLMaster> ListData
        {
            get
            {
                return this.ViewState[ViewstateStrings.CheckListData] == null ? null : (List<DDLMaster>)(this.ViewState[ViewstateStrings.CheckListData]);

            }
            set
            {
                this.ViewState[ViewstateStrings.CheckListData] = value;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            //chkAll.Checked = false;

        }
        #region Public Methods
        /// <summary>
        /// Bind Check box List
        /// </summary>
        public void BindData()
        {
            cblList.DataTextField = "Value";
            cblList.DataValueField = "PK";
            cblList.DataSource = ListData;
            cblList.DataBind();
        }
        public void ClearData()
        {
            cblList.Items.Clear();
        }
        /// <summary>
        /// Get Selected Items
        /// </summary>
        /// <returns></returns>
        public List<ListItem> GetCheckedItems()
        {
            List<ListItem> selected = cblList.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
            return selected;
        }
        #endregion
        //public void BindControl()
        //{
        //    DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetDepartment(1, 1);
        //    cblList.DataTextField = "DPT_NAME";
        //    cblList.DataValueField = "DPT_PK";
        //    cblList.DataSource = dt;
        //    cblList.DataBind();
        //}
    }
}