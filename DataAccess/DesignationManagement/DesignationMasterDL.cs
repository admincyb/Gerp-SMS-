using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTIService;
using System.Data;
using System.IO;
using BusinessObject;

namespace DataAccess.DesignationManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class DesignationMasterDL
    {
        #region Methods

        /// <summary>
        /// Get Designation List
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetDesignationList(GridPrams grid, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Designation.Parameters.SEARCHNAME , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Designation.Parameters.SEARCHVALUE  , grid.SearchValue== string.Empty ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Designation.Parameters.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Designation.Parameters.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Designation.Parameters.FIELDS ,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Designation.Parameters.SORTBY ,  grid.SortBy == null ? GTIService.Constants.Designation.Fields.ORDERBYDSGPK : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Designation.Parameters.SORTDIRCTION , grid.SortDirection == null ? GTIService.Constants.Designation.Fields.DSCORDERBY : grid.SortDirection),
            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Designation.Procedures.GETDESIGNATION, colParameters);
            return dsDesig;


        }

        /// <summary>
       /// AutoComplete Search Details
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit), 
              new DBService.Parameters(GTIService.Constants.Designation.Parameters.SEARCHFILED  ,  searchBy),  
              new DBService.Parameters(GTIService.Constants.Designation.Parameters.SEARCHFIELDVALUE   ,  searchValue),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Designation.Procedures.DETAILSSEARCH, colParameters).Tables[0];
            return dtSearchValue;

        }

        /// <summary>
        /// Save Designation Details
        /// </summary>
        /// <param name="designation"></param>
        /// <returns>string</returns>
        public static string SaveDesignation(BusinessObject.DesignationManagement.Designation designation)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Designation.Parameters.DESIGPK,   designation.DSG_PK == 0 ? (object)DBNull.Value: designation.DSG_PK) ,
                new DBService.Parameters(GTIService.Constants.Designation.Parameters.DESIGCODE,   designation.DSG_CODE == null ? string.Empty: designation.DSG_CODE) ,
                new DBService.Parameters(GTIService.Constants.Designation.Parameters.DESIGNAME,   designation.DSG_NAME == null ? string.Empty: designation.DSG_NAME) ,
                new DBService.Parameters(GTIService.Constants.Designation.Parameters.DESIGDESC,   designation.DSG_DESC == null ? string.Empty: designation.DSG_DESC) ,
                new DBService.Parameters(GTIService.Constants.Designation.Parameters.DESIG_ACTIVE,   designation.DSG_ACTIVE==0?1:designation.DSG_ACTIVE) ,
                new DBService.Parameters(GTIService.Constants.Designation.Parameters.DESIGDEPT ,   designation.DSG_DEPT) ,
                new DBService.Parameters(GTIService.Constants.Designation.Parameters.BUSINESSUNIT,   designation.SBU ) ,
                new DBService.Parameters(GTIService.Constants.Designation.Parameters.MODBY,   designation.UserPk) ,
                new DBService.Parameters(GTIService.Constants.Designation.Parameters.RETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure,GTIService.Constants.Designation.Procedures.SAVEDESIGNATION, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Designation.Parameters.RETURNVALUE]).Value).ToString();
        }

        /// <summary>
        /// Delete Designation Details
        /// </summary>
        /// <param name="desigID"></param>
        /// <returns>int</returns>
        public static int DeleteDesignationDtls(int desigID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Designation.Parameters.DESIGPK,  desigID == 0 ? (object)DBNull.Value :  desigID),
                new DBService.Parameters(GTIService.Constants.Designation.Parameters.RETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

              dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Designation.Procedures.DELETEDESIGNATION, colParameters);

              return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Designation.Parameters.RETURNVALUE]).Value);



        }

        #endregion


    }
}
