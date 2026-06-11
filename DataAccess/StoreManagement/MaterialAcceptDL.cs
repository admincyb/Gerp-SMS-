using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.StoreManagement
{
    public class MaterialAcceptDL
    {
        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetPendingSearchAuto(string searchBy, string searchValue, int deptPK, int mahPK, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.SEARCHBY , searchBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.SEARCHVALUE , searchValue),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.SEARCHCORR , deptPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.MATERIALACCEPTPK , mahPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.BIZUNIT,objUser.SBUID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.GETPENDINGSEARCH, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <param name="vendor"></param>
        /// <param name="mahID"></param>
        /// <returns></returns>
        public static DataSet GetStoreIssuePending(GridPrams grid, int sbuID, int mahPK, int store, int miPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.P_MAH_PK , mahPK),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.SEARCHCORR , store),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "MIH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.SEARCHVAL , grid.SearchValue.Trim() == "" ? "%" : "%"+grid.SearchValue.Trim()+"%"),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.BIZUNIT, sbuID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.MIPK , miPK == 0 ? (object)DBNull.Value : miPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.GETPENDINGSI, colParameters);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static List<object> SaveMaterialAccept(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialAccept.MATERIALACCEPTXML , xmlstr),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialAccept.ACCPTNO , string.Empty, 4000,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialAccept.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.SAVEMATERIALACCEPT, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialAccept.RETVAL]).Value.ToString();
            string accptNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialAccept.ACCPTNO]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(accptNo);
            return retvals;
        }


        public static List<object> SaveSTAConversion(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
                {
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.MATERIALACCEPTXML, xmlstr),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
                };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.SPINV_ITEM_ACCEPT_CONVERSION_SAVE, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialAccept.RETVAL]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            return retvals;
        }
public static DataTable GetAcceptStore(int appID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.MIPK , appID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.SPINV_ITEM_ISSUE_DTL_GET, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mahPK"></param>
        /// <returns></returns>
        public static string GetMaterialAccept(int mahPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialAccept.MAHPK , mahPK)
            };
            return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.GETMATERIALACCEPT, colParameters).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static DataSet GetMaterialAcceptList(GridPrams grid, User objUser, string pageURL)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "MAH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.SEARCHVAL , grid.SearchValue == "" ? "%" : "%" + grid.SearchValue + "%"),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.SORTBY ,  grid.SortBy == null ||grid.SortBy == "MAH_DATE"|| grid.SortBy == "MAH_NO" ? GTIService.Constants.Store.Parameters_StoreMaterialAccept.MAHPK : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.USERPK ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.PAGEURL,pageURL),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.MAH_STATUS,grid.FilterStatus=="-1"?(object)DBNull.Value:Convert.ToInt16(grid.FilterStatus)) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.GETMATERIALACCEPTLIST, colParameters);
        }

        public static DataSet GetSTAforConvert(int mahpk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.P_MAH_PK , mahpk ),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.SPINV_ITEM_ACCEPT_CONVERT_ITEM_GET, colParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetMANO()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.MAHPK , 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.GETMANO, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialAccept.MAHPK]).Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetMASearchValue(string searchBy, string searchValue, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY  ,  searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE   ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.GETMATAUTO, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mahPk"></param>
        /// <returns></returns>
        public static List<object> DeleteMaterialAccept(int mahPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialAccept.MAHPK ,  mahPk == 0 ? (object)DBNull.Value :  mahPk),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialAccept.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_StoreMaterialAccept.ACCPTNO , string.Empty, 4000,ParameterDirection.Output, DBService.ParameterType.NVarChar),

            };          
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.DELETEMATERIALACCEPT, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialAccept.RETVAL]).Value.ToString();
            string accptNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialAccept.ACCPTNO]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(accptNo);
            return retvals;

        }


        public static List<object> DeletSTAConversion(int mahPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
                {
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.P_MAH_PK, mahPk == 0 ? (object) DBNull.Value :  mahPk),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
              
                };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.SPINV_ITEM_ACCEPT_CONVERSION_DELETE, colParameters);
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_StoreMaterialAccept.RETVAL]).Value.ToString();

            List<object> retvals = new List<object>();
            retvals.Add(result);
            return retvals;

        }

/// <summary>
/// 
/// </summary>
/// <param name="mihID"></param>
/// <returns></returns>
public static DataSet GetPreviousMADetailsView(int mihID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreMaterialAccept.MIHPK, mihID),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures_StoreMaterialAccept.GETPREVIOUSMA, colParameters);
        }
    }
}
