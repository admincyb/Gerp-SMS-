using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using DataAccess.AssetService;
using BusinessObject.AssetService;
using BusinessObject.CommonManagement;
using GTIService;

namespace BusinessLogic.AssetService
{
    public class ServiceRequestBL
    {
        public static DataTable GetServiceRequestList(GridPrams grid, User objUser, string trxNo, int vendorPk, int seriveTypePk)
        {
            return ServiceRequestDL.GetServiceRequestList(grid, objUser,trxNo,vendorPk,seriveTypePk);
        }

        public static int? SaveServiceRequestDetails(string strxml, out string TrxNo)
        {
            try
            {
                return ServiceRequestDL.SaveServiceRequestDetails(strxml, out TrxNo);
            }
            catch
            {
                throw;
            }
        }

        public static AssetServiceRequestHeader GetServiceRequestByPK(int itemPK)
        {
            try
            {
                AssetServiceRequestHeader addolidayMaster = new AssetServiceRequestHeader();
                string dtl = ServiceRequestDL.GetServiceRequestByPK(itemPK);
                if (dtl != string.Empty)
                {
                    addolidayMaster = (AssetServiceRequestHeader)CommonFunctions.DeserializeObject(dtl, addolidayMaster);
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
        public static int DeleteServiceRequest(int pk, string lastModifiedDate)
        {
            return ServiceRequestDL.DeleteServiceRequest(pk, lastModifiedDate);
        }

        /// <summary>
        /// Method to get All Store Name By Type
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <param name="deptPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetAssetServiceType(User objUser, int vtpPK, int active)
        {
            DataTable dtStores = ServiceRequestDL.GetAssetServiceType(objUser, vtpPK,active);
            return dtStores;
        }
        /// <summary>
        /// Get Asset Type
        /// </summary>
        /// <param name="sbuID"></param>
        /// <param name="assetPK"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetAssetType(int sbuID, string assetPK, int active)
        {
            DataTable dtStores = ServiceRequestDL.GetAssetType(sbuID, assetPK, active);
            return dtStores;
        }
         /// <summary>
        /// Get AuoComplete Search Details
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetAssetServiceRequestNoAutocomplete(string searchBy, string searchValue, User objUser)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = ServiceRequestDL.GetAssetServiceRequestNoAutocomplete(searchBy, searchValue, objUser);
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
        /// Method to get All Store Name 
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <param name="deptPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetRequestingStores(User objUser, int sbuPk, int deptType, int deptPk)
        {
            DataTable dtStores = ServiceRequestDL.GetRequestingStoresNew(objUser, sbuPk, deptType, deptPk);
            return dtStores;
        }

        /// <summary>
        ///Validation For Cancellation of Asset Service Request
        /// </summary>
        /// <param name="CurrPK"></param>      
        /// <returns></returns>
        public static bool ValidationForCancellationASR(int CurrPK)
        {
            return ServiceRequestDL.ValidationForCancellationASR(CurrPK);
        }

        //Asset Service Request Output Report
        public static DataSet GetAssetServiceRequestReport(int RecPK)
        {
            return ServiceRequestDL.GetAssetServiceRequestReport(RecPK);
        }
        //Asset Disposal Output Report
        public static DataSet GetAssetDisposalReport(int RecPK)
        {
            return ServiceRequestDL.GetAssetDisposalReport(RecPK);
        }
        public static DataSet GetCWIPReport(int RecPK)
        {
            return ServiceRequestDL.GetCWIPReport(RecPK);
        }
    }
}
