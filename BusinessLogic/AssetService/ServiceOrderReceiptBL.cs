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
   public class ServiceOrderReceiptBL
    {
        public static DataTable GetServiceOrderReceiptList(GridPrams grid, User objUser, string trxNo, int vendorPk, int seriveTypePk)
        {
            return ServiceOrderReceiptDL.GetServiceOrderReceiptList(grid, objUser, trxNo, vendorPk, seriveTypePk);
        }

        public static int? SaveServiceOrderReceiptDetails(string strxml, out string TrxNo)
        {
            try
            {
                return ServiceOrderReceiptDL.SaveServiceOrderReceiptDetails(strxml, out TrxNo);
            }
            catch
            {
                throw;
            }
        }

        public static AssetServiceOrderReceiptHeader GetServiceOrderReceiptByPK(int itemPK)
        {
            try
            {
                AssetServiceOrderReceiptHeader addolidayMaster = new AssetServiceOrderReceiptHeader();
                string dtl = ServiceOrderReceiptDL.GetServiceOrderReceiptByPK(itemPK);
                if (dtl != string.Empty)
                {
                    addolidayMaster = (AssetServiceOrderReceiptHeader)CommonFunctions.DeserializeObject(dtl, addolidayMaster);
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
        public static int DeleteServiceOrderReceipt(int pk, string lastModifiedDate)
        {
            return ServiceOrderReceiptDL.DeleteServiceOrderReceipt(pk, lastModifiedDate);
        }
        /// <summary>
        /// Get AuoComplete Service Order No
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetAssetServiceOrderReceiptNoAutocomplete(string searchBy, string searchValue, User objUser)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = ServiceOrderReceiptDL.GetAssetServiceOrderReceiptNoAutocomplete(searchBy, searchValue, objUser);
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
        public static DataSet GetServiceOrderPending(int sbuID, int storeID, int vendor, int serviceType, int sohPk)
        {
            DataSet dsPOList = ServiceOrderReceiptDL.GetServiceOrderPending(sbuID, storeID, vendor, serviceType, sohPk);
            return dsPOList;
        }
        public static MultipleSOHeader GetSelectedServiceOrderDetail(string strxml)
        {
            try
            {
                MultipleSOHeader addolidayMaster = new MultipleSOHeader();
                string dtl = ServiceOrderReceiptDL.GetSelectedServiceOrderDetail(strxml);
                if (dtl != string.Empty)
                {
                    addolidayMaster = (MultipleSOHeader)CommonFunctions.DeserializeObject(dtl, addolidayMaster);
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

        public static DataTable GetSOVendorStoreDetails(int srPk)
        {
            return ServiceOrderReceiptDL.GetSOVendorStoreDetails(srPk);
        }
        //Asset Service Order Receipt Output Report
        public static DataSet GetAssetServiceOrderReceiptReport(int RecPK)
        {
            return ServiceOrderReceiptDL.GetAssetServiceOrderReceiptReport(RecPK);
        }
    }
}
