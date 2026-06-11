using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;


namespace DataAccess.Administration.Masters
{
    public class StoreMaterialMappingDL
    {

        /// <summary>
        /// Save MaterialMap Details 
        /// </summary>
        /// <param name="materialMapDetails"></param>
        /// <returns></returns>
        public static string SaveMaterialMappingDetails(string materialMapDetails)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Administration.Masters.StoreMaterialMapping.Parameters.MATERIALMAPXML ,(object)materialMapDetails ,DBService.ParameterType.XML),                 
                new DBService.Parameters( GTIService.Constants.Common.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Procedures.SAVEMATERIALMAPXML, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);
        }
        /// <summary>
        /// Get MaterialMap Details
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="deptParentPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>

        public static DataTable GetMaterialMappingDtls(int mapParentPK, int bizUnit, int store,string pVal,int type)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                  //new DBService.Parameters(GTIService.Constants.Administration.Masters.StoreMaterialMapping.Parameters.MATERIALMAPID, mapPK == 0 ? (object)DBNull.Value : mapPK),
                  new DBService.Parameters(GTIService.Constants.Administration.Masters.StoreMaterialMapping.Parameters.MATERIALMAPPARENT , mapParentPK),
                  //new DBService.Parameters(GTIService.Constants.Administration.Masters.StoreMaterialMapping.Parameters.SBU , bizUnit),
                  new DBService.Parameters(GTIService.Constants.Administration.Masters.StoreMaterialMapping.Parameters.STORE , store == 0 ? (object)DBNull.Value : store),
                   new DBService.Parameters(GTIService.Constants.Administration.Masters.StoreMaterialMapping.Parameters.TYPE , type == 0 ? (object)DBNull.Value : type),
                  new DBService.Parameters(GTIService.Constants.Administration.Masters.StoreMaterialMapping.Parameters.PVAL , pVal == "null" ? (object)DBNull.Value : pVal),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Procedures.GETMATERIALMAPPINGTREE, colParameters).Tables[0];
        }
      
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetMaterialMappingList(GridPrams grid, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL ,  grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection),
            };

            DataSet dtMaterial = new DataSet();
            dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Procedures.GETMATERIALMAPLISTBYSRH, colParameters);
            return dtMaterial;


        }
        /// Methord to get the Search Vlaues Corresponding to Search Type 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetMaterialMapSearchValues(string searchValue, string searchBy, int sbuPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHBY ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE,  searchBy) ,
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPK) 
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Procedures.GETMATERIALMAPSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }
    }
}
