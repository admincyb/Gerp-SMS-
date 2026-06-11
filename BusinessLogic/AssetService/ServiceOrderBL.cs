using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using DataAccess.AssetService;
using BusinessObject.CommonManagement;
using BusinessObject.AssetService;
using GTIService;

namespace BusinessLogic.AssetService
{
    public class ServiceOrderBL
    {
        public static DataTable GetServiceOrderList(GridPrams grid, User objUser, string trxNo, int vendorPk, int seriveTypePk)
        {
            return ServiceOrderDL.GetServiceOrderList(grid, objUser, trxNo, vendorPk, seriveTypePk);
        }

        public static int? SaveServiceOrderDetails(string strxml, out string TrxNo)
        {
            try
            {
                return ServiceOrderDL.SaveServiceOrderDetails(strxml, out TrxNo);
            }
            catch
            {
                throw;
            }
        }

        public static AssetServiceOrderHeader GetServiceOrderByPK(int itemPK)
        {
            try
            {
                AssetServiceOrderHeader addolidayMaster = new AssetServiceOrderHeader();
                string dtl = ServiceOrderDL.GetServiceOrderByPK(itemPK);
                if (dtl != string.Empty)
                {
                    addolidayMaster = (AssetServiceOrderHeader)CommonFunctions.DeserializeObject(dtl, addolidayMaster);
                    return addolidayMaster;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        public static int DeleteServiceOrder(int pk, string lastModifiedDate)
        {
            return ServiceOrderDL.DeleteServiceOrder(pk, lastModifiedDate);
        }       
        /// <summary>
        /// Get AuoComplete Service Order No
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetAssetServiceOrderNoAutocomplete(string searchBy, string searchValue, User objUser)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = ServiceOrderDL.GetAssetServiceOrderNoAutocomplete(searchBy, searchValue, objUser);
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
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataSet GetServiceRequestPending(int sbuID, int storeID, int vendor, int serviceType, int sohPk)
        {
            DataSet dsPOList = ServiceOrderDL.GetServiceRequestPending(sbuID, storeID, vendor, serviceType,sohPk);
            return dsPOList;
        }
        public static MultipleSRHeader GetSelectedServiceRequestDetail(string strxml)
        {
            try
            {
                MultipleSRHeader addolidayMaster = new MultipleSRHeader();
                string dtl = ServiceOrderDL.GetSelectedServiceRequestDetail(strxml);
                if (dtl != string.Empty)
                {
                    addolidayMaster = (MultipleSRHeader)CommonFunctions.DeserializeObject(dtl, addolidayMaster);
                    return addolidayMaster;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static DataTable GetSRVendorStoreDetails(int srPk)
        {
            return ServiceOrderDL.GetSRVendorStoreDetails(srPk);
        }
        /// <summary>
        ///Validation For Cancellation of Asset Service Order
        /// </summary>
        /// <param name="CurrPK"></param>      
        /// <returns></returns>
        public static bool ValidationForCancellationASO(int CurrPK)
        {
            return ServiceOrderDL.ValidationForCancellationASO(CurrPK);
        }
        //Asset Service Order Output Report
        public static DataSet GetAssetServiceOrderReport(int RecPK)
        {
            return ServiceOrderDL.GetAssetServiceOrderReport(RecPK);
        }
    }
}
