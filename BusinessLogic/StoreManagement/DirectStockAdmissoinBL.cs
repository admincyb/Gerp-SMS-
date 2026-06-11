using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.CommonManagement;

namespace BusinessLogic.StoreManagement
{
    public class DirectStockAdmissoinBL
    {
        /// <summary>
        /// Method to Save Stock Transfer Details
        /// </summary>
        /// <param name="stockTransferDtls"></param>
        /// <returns></returns>
        public static List<object> SaveStockTransferDtls(string xmlstr)
        {
            List<object> result=DataAccess.StoreManagement.DirectStockTransferDA.SaveStockTransferDetails(xmlstr);
            return result;
        }

        /// <summary>
        /// Methode used to get the purhase order vendor 
        /// </summary>
        /// <param name="bizUnitPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetPurchaseOrderVendors(int bizUnitPk, int grhPK,int Role=0)
        {
            DataTable dtVendors = DataAccess.StoreManagement.DirectStockTransferDA.GetPurchaseOrderVendors(bizUnitPk, grhPK, Role);
            return dtVendors;
        }

        /// <summary>
        /// Method to get All Store Name By Type
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <param name="deptPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetStockAdmissionStores(User objUser, int sbuPk, int deptType, int deptPk)
        {
            DataTable dtStores = DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetStoresByType(objUser, sbuPk, deptType, deptPk);
            return dtStores;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataSet GetPurchaseOrderPending(int sbuID, int storeID, int vendor, GridPrams grid, int grnID, int pohPK = 0)
        {
            DataSet dsPOList = DataAccess.StoreManagement.DirectStockTransferDA.GetPurchaseOrderPending(sbuID, storeID, vendor, grid,   grnID,  pohPK);
            return dsPOList;
        }

        public static DataSet GetWOPending(int sbuID, int storeID, int vendor, GridPrams grid, int grnID, int wohPK = 0)
        {
            DataSet dsPOList = DataAccess.StoreManagement.DirectStockTransferDA.GetWOPending(sbuID, storeID, vendor, grid, grnID, wohPK);
            return dsPOList;
        }


        /// <summary>
        /// Get Stock Transfer Details Get
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataSet GetStockTransferList(GridPrams grid, User objUser, int procID, string PageUrl, int? deptSearch, string admissionNoSearch, string poNoSearch, int? vendorSearch,int cmpPk=0)
        {
            DataSet dsResult;
            dsResult=DataAccess.StoreManagement.DirectStockTransferDA.GetStockTransferList(grid, objUser, procID, PageUrl, deptSearch, admissionNoSearch, poNoSearch, vendorSearch,cmpPk);
            return dsResult;
        }
        public static DataSet GetStockTransferListByType(GridPrams grid, User objUser, int procID, string PageUrl, int? deptSearch, string admissionNoSearch, string poNoSearch, int? vendorSearch, int cmpPk = 0)
        {
            DataSet dsResult;
            dsResult = DataAccess.StoreManagement.DirectStockTransferDA.GetStockTransferListByType(grid, objUser, procID, PageUrl, deptSearch, admissionNoSearch, poNoSearch, vendorSearch, cmpPk);
            return dsResult;
        }

        /// <summary>
        /// Get Stock Admission Details By Pk
        /// </summary>
        /// <param name="grhPk"></param>
        /// <returns>DataTable</returns>
        public static string GetDirectStockAdmissionByPk(int grhPk)
        {
            //DataTable dtResult;
            //dtResult = DataAccess.StoreManagement.DirectStockTransferDA.GetDirectStockAdmissionByPk(grhPk);
            //return dtResult;
           
            return DataAccess.StoreManagement.DirectStockTransferDA.GetDirectStockAdmissionByPk(grhPk);           
        }

        /// <summary>
        /// Get Damage Types
        /// </summary>
        /// <param name="bizunit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDamageTypes(int bizUnit)
        {
            DataTable dtResult;
            dtResult = DataAccess.StoreManagement.StoreAuditDL.GetDamageTypes(bizUnit);
            return dtResult;
        }

        /// <summary>
        /// Get Damaged Items Store
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDamagedItemsStore(User objUser)
        {
            int deptType=1, deptPk=0;
            DataTable dtStores = DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetStoresByType(objUser, objUser.SBUID, deptType, deptPk);
            return dtStores;
        }

        /// <summary>
        /// Method to Delete StockTransfer Details
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteStockTransfer(int pk, string lastModifiedDate)
        {
            return  DataAccess.StoreManagement.DirectStockTransferDA.DeleteStockTransfer(pk, lastModifiedDate);
        }
        public static DataSet GetDirectStockAdmissionReport(int RccPK)
        {
            return DataAccess.StoreManagement.DirectStockTransferDA.GetDirectStockAdmissionReport(RccPK);
        }

        public static DataSet GetDirectStockAdmissionReportDOCNOREVISION(int RccPK,int reportpk)
        {
            return DataAccess.StoreManagement.DirectStockTransferDA.GetDirectStockAdmissionReportDOCNOREVISION(RccPK, reportpk);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="vendorPK"></param>
        /// <param name="grnPK"></param>
        /// <param name="objUser"></param>
        /// <param name="deptPk"></param>
        /// <returns></returns>
        public static string GetPendingDirectGRNSearchAuto(string searchBy, string searchValue, int vendorPK, int grnPK, User objUser, int deptPk)
        {
            DataTable dtSearch = DataAccess.StoreManagement.DirectStockTransferDA.GetPendingDirectGRNSearchAuto(searchBy, searchValue, vendorPK, grnPK, objUser, deptPk);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);
        }
        public static string GetPendingWOGRNSearchAuto(string searchBy, string searchValue, int vendorPK, int grnPK, User objUser, int deptPk,int PendingWo)
        {
            DataTable dtSearch = DataAccess.StoreManagement.DirectStockTransferDA.GetPendingWOGRNSearchAuto(searchBy, searchValue, vendorPK, grnPK, objUser, deptPk, PendingWo);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);
        }

        /// <summary>
        /// Get AuoComplete Search Details
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetStockAdmissionAutocomplete(string searchBy, string searchValue, User objUser,int? MenuType=0)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = DataAccess.StoreManagement.DirectStockTransferDA.GetDetailsForAutoSearch(searchBy, searchValue, objUser,MenuType);
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
        ///Validation For Cancellation of Direct GRN cancel 
        /// </summary>
        /// <param name="CurrPK"></param>      
        /// <returns></returns>
        public static bool ValidationForCancellationDSA(int CurrPK)
        {         
            return DataAccess.StoreManagement.DirectStockTransferDA.ValidationForCancellationDSA(CurrPK);
        }

        public static List<object> SaveStockTransferWkf(string strXml, out string strTrxNumber)
        {
            return DataAccess.StoreManagement.DirectStockTransferDA.SaveStockTransferWkf(strXml, out strTrxNumber);
        }

        /// <summary>
        /// Get Plant Codes
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetDirectGRNAutocomplete(string searchBy, string searchValue, User objUser)
        {
            DataTable dt = DataAccess.StoreManagement.DirectStockTransferDA.GetDetailsForAutoSearch(searchBy, searchValue, objUser);           
            return dt;
        }

        public static DataSet GetGRNForConvert(int grhPk)
        {
            return DataAccess.StoreManagement.DirectStockTransferDA.GetGRNForConvert(grhPk);
        }

        public static int ConvertGRN(string strXml)
        {
            return DataAccess.StoreManagement.DirectStockTransferDA.ConvertGRN(strXml);
        }

        public static int DeleteGRNConversion(int pk)
        {
            return DataAccess.StoreManagement.DirectStockTransferDA.DeleteGRNConversion(pk);
        }

        public static string GetWOBOMForReturn(string strXml, int supplierPK,int MaterialRetturn=0)
        {
            return DataAccess.StoreManagement.DirectStockTransferDA.GetWOBOMForReturn(strXml, supplierPK, MaterialRetturn);
        }
    }
}
