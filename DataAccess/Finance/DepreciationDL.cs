using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.Finance;
using ERP.Utilities;

namespace DataAccess.Finance
{
    public sealed class DepreciationDL
    {
        public static DataTable GetDepreciationTranList(GridPrams grid, int userPk, int bizUnit, string pageUrl,int PType,int AssetType,int? Plant = null, int? Status = null, int? Pending = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy =="FDH_PK"?grid.SearchValue:grid.SearchValue+"%")), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),                  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, userPk),             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  pageUrl),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_TYPE, PType),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PLANT,  Plant == -1 ? (object) DBNull.Value :  Plant),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.STATUSFILTER,  Status == null ? (object) DBNull.Value :  Status),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PendingAmt,  Pending == null ? (object) DBNull.Value :  Pending)              ,
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO  , grid.PageNumber), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE  , grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ASSET_TYPE  , AssetType)
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_DEPRECIATION_HDR_GET_LIST, colParameters);
            return dsSet.Tables[0];
        }

        public static DataTable GetAssetDisposalList(GridPrams grid, int userPk, int bizUnit, string pageUrl, int? Plant = null, int? Status = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy =="FDH_PK"?grid.SearchValue:grid.SearchValue+"%")),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? (Object)DBNull.Value : grid.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? (Object)DBNull.Value : grid.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, userPk),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  pageUrl),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_TYPE, PType),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PLANT,  Plant == -1 ? (object) DBNull.Value :  Plant),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.STATUSFILTER,  Status == null ? (object) DBNull.Value :  Status),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PendingAmt,  Pending == null ? (object) DBNull.Value :  Pending)              ,
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO  , grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE  , grid.PageSize)
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_ASSET_DISPOSAL_HDR_GET_LIST, colParameters);
            return dsSet.Tables[0];
        }

        public static DataTable GetLocations(int? locPK, short? active, int? bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {            
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_LOCPK,  locPK??(object)DBNull.Value),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active??(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit??(object)DBNull.Value), 
            };
            DataSet dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPADMLOCATIONMST_GET_KV, colParameters);
            return dsSet.Tables[0];
        }

        public static DataTable GetCategory(int? xcaPK, short? active, int? bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {            
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_XCAPK,  xcaPK??(object)DBNull.Value),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE , active??(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT  ,  bizUnit??(object)DBNull.Value), 
            };
            DataSet dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPXACCTGASSET_GET_KV, colParameters);
            return dsSet.Tables[0];
        }

        public static DataTable GetAssetType(int? atpPK, short? active, int? bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {            
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_atpPK,  atpPK??(object)DBNull.Value),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE , active??(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT  ,  bizUnit??(object)DBNull.Value), 
            };
            DataSet dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPASRASSETTYPEMST_GET_KV, colParameters);
            return dsSet.Tables[0];
        }

        public static DepreciationBO GetDepreciationDetails(int deprePK)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_PK,deprePK)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_DEPRECIATION_GET_XML, colParameters).Tables[0];

            string objXml = string.Empty;

            foreach (DataRow row in dtList.Rows)
            {
                objXml += row[0].ToString();
            }
            DepreciationBO depreciationmDetails=null;
            if(!objXml.IsNullOrEmptyOrWhitespace()) depreciationmDetails = CommonFunctions.XmlDeserialize<DepreciationBO>(objXml);
            return depreciationmDetails;
        }

        public static AssetDisposalBO GetDisposalDetails(int disposalPK)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_ADH_PK, disposalPK)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_ASSET_DISPOSAL_GET_XML, colParameters).Tables[0];

            string objXml = string.Empty;

            foreach (DataRow row in dtList.Rows)
            {
                objXml += row[0].ToString();
            }
            AssetDisposalBO disposalDetails = null;
            if (!objXml.IsNullOrEmptyOrWhitespace()) disposalDetails = CommonFunctions.XmlDeserialize<AssetDisposalBO>(objXml);
            return disposalDetails;
        }

        public static DataTable GetAssetList(AssetFilterForDepreParameterBinder parameter)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {            
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_FROM,  parameter.FromDate),  
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_TO, parameter.ToDate),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PUR_FDT, parameter.PurchaseDateFrom==DateTime.MinValue 
                                          ?(object)DBNull.Value
                                          :parameter.PurchaseDateFrom), 
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PUR_TDT,  parameter.PurchaseDateTo==DateTime.MinValue 
                                          ?(object)DBNull.Value
                                          :parameter.PurchaseDateTo),  
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_xcaPK, parameter.AssetCategory.ToString() == CommonConstants.SELECTVAL
                                          ?(object)DBNull.Value
                                          :parameter.AssetCategory),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_locPK, parameter.Location.ToString() == CommonConstants.SELECTVAL
                                          ? (object)DBNull.Value
                                          : parameter.Location), 
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_atpPK, parameter.AssetType.ToString() == CommonConstants.SELECTVAL
                                          ? (object)DBNull.Value
                                          : parameter.AssetType), 
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_asrCode, parameter.AssetCode),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_asrName, parameter.AssetName),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_TYPE, parameter.PType),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PLANT, parameter.Plant),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_DPRMNTH, parameter.DepreMonth),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_DPRMNTH_CONFG, parameter.CfgType),
            };
            DataSet dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_DEPRECIATION_ASSET_GET, colParameters);
            return dsSet.Tables[0];
        }

        public static DataTable GetPrevAssetList(AssetFilterForDepreParameterBinder parameter)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_FROM,  parameter.FromDate),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_TO, parameter.ToDate),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PUR_FDT, parameter.PurchaseDateFrom==DateTime.MinValue
                                          ?(object)DBNull.Value
                                          :parameter.PurchaseDateFrom),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PUR_TDT,  parameter.PurchaseDateTo==DateTime.MinValue
                                          ?(object)DBNull.Value
                                          :parameter.PurchaseDateTo),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_xcaPK, parameter.AssetCategory.ToString() == CommonConstants.SELECTVAL
                                          ?(object)DBNull.Value
                                          :parameter.AssetCategory),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_locPK, parameter.Location.ToString() == CommonConstants.SELECTVAL
                                          ? (object)DBNull.Value
                                          : parameter.Location),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_atpPK, parameter.AssetType.ToString() == CommonConstants.SELECTVAL
                                          ? (object)DBNull.Value
                                          : parameter.AssetType),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_asrCode, parameter.AssetCode),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_asrName, parameter.AssetName),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_TYPE, parameter.PType),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PLANT, parameter.Plant),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_DPRMNTH, parameter.DepreMonth),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_DPRMNTH_CONFG, parameter.CfgType),
            };
            DataSet dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_DEPRECIATION_ASSET_GET, colParameters);
            return dsSet.Tables[0];
        }

        public static int Save(DepreciationBO depreciation, out string depreNo)
        {
            depreciation.FDH_DESC = depreciation.FDH_DESC.HtmlDecode();

            string xmlDoc = CommonFunctions.XmlSerialize(depreciation);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            depreNo = string.Empty;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_NO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_DEPRECIATION_HDR_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            depreNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_NO]).Value);

            return result;
        }

        public static int? SaveDepreciationWkf(string xmlDoc, out int refID, out string transNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_XML, xmlDoc),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_NO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_DEPRECIATION_HDR_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_VAL]).Value);
            refID = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_REF_PK]).Value);
            transNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_NO]).Value);
            return result;
        }

        public static int? SaveAssetDisposalWkf(string xmlDoc, out int refID, out string transNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_XML, xmlDoc),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_RET_NO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_ASSET_DISPOSAL_HDR_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_VAL]).Value);
            refID = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_REF_PK]).Value);
            transNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.P_RET_NO]).Value);
            return result;
        }

        public static int Delete(int pk, DateTime lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_PK,pk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT,lastModifiedDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_DEPRECIATION_HDR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int DeleteDetails(int pk, DateTime lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDD_PK,pk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT,lastModifiedDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_DEPRECIATION_DTL_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataSet GetDepreciationRptDetails(int DeprPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                {            
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_PK,  DeprPK == 0 ? (object) DBNull.Value :  DeprPK),
                };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPASR_DEPRECIATION_RPT, colParameters);
            return dsList;
        }

        /// <summary>
        /// Returns the search result list for Autocomplete for Depreciation No
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="vendorPK"></param>
        /// <param name="excDate"></param>
        /// <returns></returns>
        public static DataTable GetDepreciationNoAuto(string searchValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FDH_NO,  searchValue == string.Empty ? (object) DBNull.Value :  searchValue)           
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_DEPRECIATION_HDR_GET, colParameters).Tables[0];
        }

        public static DataTable GetAssetDisposalNoAuto(string searchValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_ADH_NO,  searchValue == string.Empty ? (object) DBNull.Value :  searchValue)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_ASSET_DISPOSAL_HDR_GET, colParameters).Tables[0];
        }
        public static DataTable GetAssetStatus(string searchBy, string splCond)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL, searchBy??(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME, splCond??(object)DBNull.Value),
            };
            DataSet dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPASSET_XacConReuset_GET_KV, colParameters);
            return dsSet.Tables[1];
        }
        public static DataTable GetAssetListForDisposal(AssetFilterForDepreParameterBinder parameter)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_ADH_FROM,  parameter.FromDate),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_ADH_TO, parameter.ToDate),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PUR_FDT, parameter.PurchaseDateFrom==DateTime.MinValue
                                          ?(object)DBNull.Value
                                          :parameter.PurchaseDateFrom),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PUR_TDT,  parameter.PurchaseDateTo==DateTime.MinValue
                                          ?(object)DBNull.Value
                                          :parameter.PurchaseDateTo),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_xcaPK, parameter.AssetCategory.ToString() == CommonConstants.SELECTVAL
                                          ?(object)DBNull.Value
                                          :parameter.AssetCategory),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_locPK, parameter.Location.ToString() == CommonConstants.SELECTVAL
                                          ? (object)DBNull.Value
                                          : parameter.Location),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_atpPK, parameter.AssetType.ToString() == CommonConstants.SELECTVAL
                                          ? (object)DBNull.Value
                                          : parameter.AssetType),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_asrCode, parameter.AssetCode),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_asrName, parameter.AssetName),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_TYPE, parameter.PType),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_PLANT, parameter.Plant),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_ADH_DPRMNTH, parameter.DepreMonth),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_ADH_DPRMNTH_CONFG, parameter.CfgType),
            };
            DataSet dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_ASSET_DISPOSAL_GET, colParameters);
            return dsSet.Tables[0];
        }
    }
}
