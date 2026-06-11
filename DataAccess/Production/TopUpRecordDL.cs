using System;
using System.Data;
using BusinessObject;

namespace DataAccess.Production
{
    public class TopUpRecordDL
    {
        #region Methods

        /// <summary>
        /// Get TopUp Record list
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataSet GetTopUpList(GridPrams grid, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ||grid.SortBy == "TUH_DATE"? "TUH_PK": grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection == null ? "desc": grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) )
            };
            DataSet dsTopUpList = new DataSet();
            dsTopUpList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedure_TopUpRecord.GETTOPUPRECORDLIST, colParameters);
            return dsTopUpList;


        }

        /// <summary>
        /// Get AutoComplete Sarach For TopUp Record
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY  ,  searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE   ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID), 
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedure_TopUpRecord.TOPUPAUTO, colParameters).Tables[0];
            return dtSearchValue;
        }

        /// <summary>
        /// Delete TopUp Details 
        /// </summary>
        /// <param name="topUpPk"></param>
        /// <returns></returns>
        public static int DeleteTopUpDtls(int topUpPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Production.Parameters_TopUpRecord.TOPUPPK ,  topUpPk == 0 ? (object)DBNull.Value :  topUpPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Production.Procedure_TopUpRecord.DELETETOPUPRECORD, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

        }

        /// <summary>
        /// Get Uom List For TopUp Items
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="catg"></param>
        /// <returns></returns>
        public static DataTable GetUomList(int tankPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
                 new DBService.Parameters(GTIService.Constants.Production.Parameters_TopUpRecord.TANKPK,  tankPK),
             };
            DataTable dtUomDtls = new DataTable();
            dtUomDtls = dbService.DataAdapter(CommandType.StoredProcedure,GTIService.Constants.Production.Procedure_TopUpRecord. GETTOPUPUOMLIST, colParameters).Tables[0];
            return dtUomDtls;
        }

        /// <summary>
        /// Save TopUp Record Details
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveTopUpDetails(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Production.Parameters_TopUpRecord.TOPUPXML  , strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Production.Procedure_TopUpRecord.SAVETOPUPRECORD , colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get TopUp Details By Pk
        /// </summary>
        /// <param name="topUpPk"></param>
        /// <returns></returns>
        public static string GetTopUpDetails(int topUpPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Production.Parameters_TopUpRecord.TOPUPPK  ,  topUpPk),   
            };
            return Convert.ToString(dbService.ExecuteScalar(CommandType.StoredProcedure,GTIService.Constants.Production.Procedure_TopUpRecord.GETTOPUPRECORDDTLS, colParameters));
        }

        /// <summary>
        /// Chgeck Stock Available or not
        /// </summary>
        /// <param name="itemCatg"></param>
        /// <param name="item"></param>
        /// <param name="qty"></param>
        /// <param name="toUOM"></param>
        /// <param name="topUpItemPk"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static int CheckStockAvailable( int itemCatg, int item, double qty, int toUOM, int topUpItemPk, int sbu )
        {
           DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Production.Parameters_TopUpRecord.TOPUPITEMTYPE,  itemCatg == 0 ? (object)DBNull.Value :  itemCatg),
                new DBService.Parameters(GTIService.Constants.Production.Parameters_TopUpRecord.TOPUPITEM,  item == 0 ? (object)DBNull.Value :  item),
                new DBService.Parameters(GTIService.Constants.Production.Parameters_TopUpRecord.TOPUPQTY,  qty == 0 ? (object)DBNull.Value :  qty),
                new DBService.Parameters(GTIService.Constants.Production.Parameters_TopUpRecord.TOPUPQTYUOM,  toUOM == 0 ? (object)DBNull.Value :  toUOM),
                new DBService.Parameters(GTIService.Constants.Production.Parameters_TopUpRecord.TOPUPDTLSPK,  topUpItemPk == 0 ? (object)DBNull.Value :  topUpItemPk),
                new DBService.Parameters(GTIService.Constants.Production.Parameters_TopUpRecord.TOPUPSBU,  sbu == 0 ? (object)DBNull.Value :  sbu),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure,GTIService.Constants.Production.Procedure_TopUpRecord.CHECKSTOCKAVAILABLE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

        }  

        /// <summary>
        /// Get Item List For a Slected tankType UOM and Catg
        /// </summary>
        /// <param name="sbu"></param>
        /// <param name="catg"></param>
        /// <param name="tankPK"></param>
        /// <returns></returns>
        public static DataTable GetItemNameList(int sbu, int catg, int tankPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
                 new DBService.Parameters(GTIService.Constants.Production.Parameters_TopUpRecord .TOPUPTANKPK,  tankPK),
                 new DBService.Parameters(GTIService.Constants.Production.Parameters_TopUpRecord.TOPUPCATG,  catg),
             };
            DataTable dtItemList = new DataTable();
            dtItemList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedure_TopUpRecord.GETTOPUPITEMLIST, colParameters).Tables[0];
            return dtItemList;
        }

        # endregion
    }
}
