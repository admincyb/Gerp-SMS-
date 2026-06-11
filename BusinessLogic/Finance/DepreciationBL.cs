using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using DataAccess.Finance;
using BusinessObject.Finance;
using BusinessObject.CommonManagement;

namespace BusinessLogic.Finance
{
    public sealed class DepreciationBL
    {
        public static DataTable GetDepreciationTranList(GridPrams grid, int userPK, int bizUnit, string pageUrl,int PType,int AssetType,int? Plant = null, int? Status = null, int? Pending = null)
        {
            return DepreciationDL.GetDepreciationTranList(grid, userPK, bizUnit, pageUrl,PType, AssetType,Plant, Status, Pending);
        }

        public static DataTable GetAssetDisposalList(GridPrams grid, int userPk, int bizUnit, string pageUrl, int? Plant = null, int? Status = null)
        {
            return DepreciationDL.GetAssetDisposalList(grid, userPk, bizUnit, pageUrl, Plant, Status);
        }

        public static DataTable GetLocations(int? locPK, short? active, int? bizUnit)
        {
            return DepreciationDL.GetLocations(locPK, active, bizUnit);
        }
      
        public static DataTable GetCategory(int? xcaPK, short? active, int? bizUnit)
        {
            return DepreciationDL.GetCategory(xcaPK, active, bizUnit);
        }

        public static DataTable GetAssetType(int? atpPK, short? active, int? bizUnit)
        {
            return DepreciationDL.GetAssetType(atpPK, active, bizUnit);
        }

        public static DepreciationBO GetDepreciationDetails(int deprePK)
        {
            return DepreciationDL.GetDepreciationDetails(deprePK);
        }

        public static AssetDisposalBO GetDisposalDetails(int disposalPK)
        {
            return DepreciationDL.GetDisposalDetails(disposalPK);
        }

        public static int Save(DepreciationBO depreciation, out string depreNo)
        {
            return DepreciationDL.Save(depreciation, out depreNo);
        }

        public static int? SaveDepreciationWkf(string xmlDoc, out int refID, out string transNo)
        {
            return DepreciationDL.SaveDepreciationWkf(xmlDoc, out refID, out transNo);
        }

        public static int? SaveAssetDisposalWkf(string xmlDoc, out int refID, out string transNo)
        {
            return DepreciationDL.SaveAssetDisposalWkf(xmlDoc, out refID, out transNo);
        }

        public static DataTable GetAssetList(AssetFilterForDepreParameterBinder parameter)
        {
            return DepreciationDL.GetAssetList(parameter);
        }

        public static DataTable GetPrevAssetList(AssetFilterForDepreParameterBinder parameter)
        {
            return DepreciationDL.GetPrevAssetList(parameter);
        }

        public static int Delete(int pk, DateTime lastModifiedDate)
        {
            return DepreciationDL.Delete(pk, lastModifiedDate);
        }

        public static int DeleteDetails(int pk, DateTime lastModifiedDate)
        {
            return DepreciationDL.DeleteDetails(pk, lastModifiedDate);
        }
        /// <summary>
        /// Get Depreciation Print
        /// </summary>       
        /// <returns>DataSet</returns>
        public static DataSet GetDepreciationRptDetails(int DeprPK)
        {
            return DataAccess.Finance.DepreciationDL.GetDepreciationRptDetails(DeprPK);
        }

        /// <summary>
        /// Returns the search result list for Autocomplete for Depreciation No
        /// </summary>
        /// <param name="searchValue"></param>     
        /// <returns></returns>
        public static List<AutoCompleteBO> GetDepreciationNoAuto(string searchValue)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = DataAccess.Finance.DepreciationDL.GetDepreciationNoAuto(searchValue);
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    LongKey = row.Field<long>(Fields.SEARCHVALUEFIELD),
                    Name = row.Field<string>(Fields.SEARCHTEXTFIELD)
                }).ToList();
            }
            catch
            {
            }
            return result;
        }

        public static List<AutoCompleteBO> GetAssetDisposalNoAuto(string searchValue)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = DataAccess.Finance.DepreciationDL.GetAssetDisposalNoAuto(searchValue);
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    LongKey = row.Field<long>("ADH_PK"),
                    Name = row.Field<string>("ADH_NO")
                }).ToList();
            }
            catch
            {
            }
            return result;
        }
        public static DataTable GetAssetStatus(string searchBy, string splCond)
        {
            return DepreciationDL.GetAssetStatus(searchBy, splCond);
        }
        public static DataTable GetAssetListForDisposal(AssetFilterForDepreParameterBinder parameter)
        {
            return DepreciationDL.GetAssetListForDisposal(parameter);
        }
    }
}
