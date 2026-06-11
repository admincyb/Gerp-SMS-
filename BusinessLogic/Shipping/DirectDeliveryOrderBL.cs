using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.CommonManagement;

namespace BusinessLogic.Shipping
{
   public class DirectDeliveryOrderBL
    {

       public static List<object> SaveDirectDeliveryOrder(string strXml, ref DataTable dtOut)
       {
           return DataAccess.Shipping.DirectDeliveryOrderDL.SaveDirectDeliveryOrder(strXml, ref dtOut);
       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="xmlstr"></param>
       /// <returns></returns>
       public static int SaveDOAllocation(string xmlstr)
       {
           return DataAccess.Shipping.DirectDeliveryOrderDL.SaveDOAllocation(xmlstr);
       }

       /// <summary>
       /// Get Stock Transfer Details Get
       /// </summary>
       /// <param name="grid"></param>
       /// <param name="objUser"></param>
       /// <returns></returns>
       public static DataSet GetDirectDOGetList(GridPrams grid, User objUser, string doNoSearch, int cusPk)
       {
           DataSet dsResult;
           dsResult = DataAccess.Shipping.DirectDeliveryOrderDL.GetDirectDOGetList(grid, objUser, doNoSearch, cusPk);
           return dsResult;
       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="dpdPK"></param>
       /// <returns></returns>
       public static DataTable GetAllocateCartonDODetails(int dpdPK)
       {
           DataTable dtResult = DataAccess.Shipping.DirectDeliveryOrderDL.GetAllocateCartonDODetails(dpdPK);
           return dtResult;
       }

       /// <summary>
       /// Get Direct DO Details By Pk
       /// </summary>
       /// <param name="grhPk"></param>
       /// <returns>DataTable</returns>
       public static string GetDirectDOByPk(int dphPk)
       {          
           return DataAccess.Shipping.DirectDeliveryOrderDL.GetDirectDOByPk(dphPk);
       }

       /// <summary>
       /// Method to Delete DirectDO Details
       /// </summary>
       /// <param name="pk"></param>
       /// /// <param name="lastModifiedDate"></param>
       /// <returns>int</returns>
       public static int DeleteDirectDO(int pk, string lastModifiedDate)
       {
           return DataAccess.Shipping.DirectDeliveryOrderDL.DeleteDirectDO(pk, lastModifiedDate);
       }

        /// <summary>
        /// Method to get All Store Name By Type
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <param name="deptPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDeptStores(User objUser, int sbuPk, int deptType, int deptPk)
        {
            DataTable dtStores = DataAccess.Shipping.DirectDeliveryOrderDL.GetDeptStores(objUser, sbuPk, deptType, deptPk);
            return dtStores;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataTable GetPendingSalesOrder(int sbuID, int storeID, int customer, GridPrams grid, int doPk, string ItemName, string sohNo, int sohPK = 0)
        {
            DataTable dsPOList = DataAccess.Shipping.DirectDeliveryOrderDL.GetPendingSalesOrder(sbuID, storeID, customer, grid, doPk, ItemName, sohNo, sohPK);
            return dsPOList;
        }
        /// <summary>
        ///Validation For Cancellation of Direct GRN cancel 
        /// </summary>
        /// <param name="CurrPK"></param>      
        /// <returns></returns>
        public static bool ValidationForCancellationDO(int CurrPK)
        {
            return DataAccess.Shipping.DirectDeliveryOrderDL.ValidationForCancellationDO(CurrPK);
        }
        
        public static DataTable GetBatchNo(int itemPK, int deptPK, int batchPK, DateTime? date = null, int BatchType = 0)
        {
            DataTable dtMaterialDtls = DataAccess.Shipping.DirectDeliveryOrderDL.GetBatchNo(itemPK, deptPK, batchPK, date, BatchType);
            return dtMaterialDtls;
        }
        public static DataTable GetBatchDetails(int batchPK,int ToUOM)
        {
            DataTable dtMaterialDtls = DataAccess.Shipping.DirectDeliveryOrderDL.GetBatchDetails(batchPK,ToUOM);
            return dtMaterialDtls;
        }

        /// <summary>
        /// Get Stock Transfer Details Get
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetDirectDOSaleOrderDetailList(int dphPk)
        {
            DataTable dtResult;
            dtResult = DataAccess.Shipping.DirectDeliveryOrderDL.GetDirectDOSaleOrderDetailList(dphPk);
            return dtResult;
        }
        /// <summary>
        ///  Function Used To Get all Material details corresponding to a material pk 
        /// </summary>
        /// <param name="materialPK"></param>
        /// <returns>string</returns>
        public static DataTable GetCurrentStoreStock(int itemID, int sbuPk, DateTime? date = null, int toUOM = 0)
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetCurrentStockForStore(itemID, sbuPk, date, toUOM);
            return dtMaterialDtls;
        }

        /// <summary>
        /// Get AuoComplete Service Order No
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetDirectDONoAutocomplete(string searchBy, string searchValue, User objUser, string pagURl)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = DataAccess.Shipping.DirectDeliveryOrderDL.GetDirectDONoAutocomplete(searchBy, searchValue, objUser,pagURl);
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

        public static int CheckforValidMultipleSO(string strxml)
        {

            return DataAccess.Shipping.DirectDeliveryOrderDL.CheckforValidMultipleSO(strxml);
        } 
      /// <summary>
        /// Get AuoComplete Service Order No
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetDirectSOPendingAutoComplete(string searchBy, string searchValue, User objUser, string cusPK, int dphPK)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = DataAccess.Shipping.DirectDeliveryOrderDL.GetDirectSOPendingAutoComplete(searchBy, searchValue, objUser, cusPK,dphPK);
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
       
    }
}
