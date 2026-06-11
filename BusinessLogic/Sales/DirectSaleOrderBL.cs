using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.CommonManagement;

namespace BusinessLogic.Sales
{
    public class DirectSaleOrderBL
    {
        /// <summary>
        /// Save Direct Sale Order
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static int? SaveDirectSaleOrderWkfDetails(string xmlDoc, out int refID)
        {
            return DataAccess.SaleOrder.DirectSaleOrderDL.SaveDirectSaleOrderWkfDetails(xmlDoc, out refID);
        }

        /// <summary>
        /// Get Direct Sale order
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataSet GetDirectSOGetList(GridPrams grid, User objUser, string doNoSearch, int cusPk, string sohType)
        {
            DataSet dsResult;
            dsResult = DataAccess.SaleOrder.DirectSaleOrderDL.GetDirectSOGetList(grid, objUser, doNoSearch, cusPk, sohType);
            return dsResult;
        }

        /// <summary>
        /// Get Direct Sale Order Details By Pk
        /// </summary>
        /// <param name="grhPk"></param>
        /// <returns>DataTable</returns>
        public static string GetDirectSaleOrderByPk(int sohPk)
        {
            return DataAccess.SaleOrder.DirectSaleOrderDL.GetDirectSaleOrderByPk(sohPk);
        }

        /// <summary>
        /// Delete Sale Order details by PK
        /// </summary>
        /// <param name="grhPk"></param>
        /// <returns>DataTable</returns>
        public static int DeleteSaleOrderDetails(int sohPK, DateTime lastModDate, string deleteReason = "")
        {
            return DataAccess.SaleOrder.DirectSaleOrderDL.DeleteSaleOrderDetails(sohPK, lastModDate, deleteReason);
        }

        /// <summary>
        /// Get Stock Transfer Details Get
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetExtDirectSaleOrderDetailList(int sohPk = 0, int sodPk = 0)
        {
            DataTable dtResult;
            dtResult = DataAccess.SaleOrder.DirectSaleOrderDL.GetDirectDOSaleOrderDetailList(sohPk,sodPk);
            return dtResult;
        }

        /// <summary>
        /// Get AuoComplete Service Order No
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetDirectSONoAutocomplete(string searchBy, string searchValue, User objUser, string pagURl)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = DataAccess.SaleOrder.DirectSaleOrderDL.GetDirectSONoAutocomplete(searchBy, searchValue, objUser, pagURl);
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>(GTIService.Constants.Designation.Fields.PK),
                    Name = row.Field<string>(GTIService.Constants.Designation.Fields.VALUE)
                }).ToList();
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        ///Validation For Cancellation of Direct Sale Order 
        /// </summary>
        /// <param name="CurrPK"></param>      
        /// <returns></returns>
        public static bool ValidationForCancellationSO(int sohPK)
        {
            return DataAccess.SaleOrder.DirectSaleOrderDL.ValidationForCancellationSO(sohPK);
        }
        /// <summary>
        /// Save Direct Sale Order Short Close
        /// </summary>
        /// <param name="sohPK"></param>
        /// <param name="reason"></param>
        /// <param name="refNo"></param>
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static int? SaveDirectSaleOrderShortClose(int sohPK, string reason, string refNo, int userPK)
        {
            return DataAccess.SaleOrder.DirectSaleOrderDL.SaveDirectSaleOrderShortClose(sohPK, reason, refNo, userPK);
        }
    }
}
