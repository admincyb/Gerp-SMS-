using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace ERPSMS_v01.PBIData
{
    public partial class MonthlySalesReport : System.Web.UI.Page
    {
        #region Page Variables
        DataSet dsMonthlySales;
        #endregion 
        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            if (!IsPostBack)
            {
                GetFieldValues(ControlsEnum.MONTLYSALES);
                BindGrid(ControlsEnum.MONTLYSALES);
            }
        }
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    case ControlsEnum.MONTLYSALES:
                        dsMonthlySales = BusinessLogic.Sales.SaleOrderBL.MonthlySalesList();
                        break;
                }
            }
            catch (Exception ex)
            { }

        }
        #endregion
         /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.MONTLYSALES:
                        if (dsMonthlySales != null && dsMonthlySales.Tables.Count > 0)
                        {
                            grdMonthlySalesList.DataSource = dsMonthlySales.Tables[0];
                            grdMonthlySalesList.DataBind();
                        }
                        break;
                }
            }
            catch (Exception ex)
            { }
        }
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            MONTLYSALES
        }
        #endregion
    }
}