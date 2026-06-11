using System;
using System.Data;
using BusinessObject;

namespace DataAccess.CompoundManagement
{
    # region Methods
    public class CompoundMasterDL
    {
             
        /// <summary>
        /// Get Polymer Type to Fill DDL
        /// </summary>
        /// <param name="sBU"></param>
        /// <returns></returns>
        public static DataTable GetPolymerType(int sBU)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.BizUnit, sBU),
            };
            DataTable dtPolymerType = new DataTable();
            dtPolymerType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_GetPolymer, colParameters).Tables[0];
            return dtPolymerType;


        }

        /// <summary>
        /// Get Conversion UomList to Fill DDL
        /// </summary>
        /// <param name="uOM"></param>
        /// <returns></returns>
        public static DataTable GetConversionUOMList(int uOM)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.UOM, uOM),
            };
            DataTable dtUOM = new DataTable();
            dtUOM = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_GetUOMConversionUOM, colParameters).Tables[0];
            return dtUOM;


        }

        /// <summary>
        /// Get Converssion Factor By Material PK
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="conversionUOMPK"></param>
        /// <returns></returns>
        public static string GetConversionFactor(int uomFrm, int uomTo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.UOMFROM, uomFrm),
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.UOMTO, uomTo)
              
            };
           return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_GetConversionFactor, colParameters).ToString();
        }
      
        /// <summary>
        /// Get Material Name by Material Category To Fill Item To DDL
        /// </summary>
        /// <param name="sBU"></param>
        /// <returns></returns>
        public static DataTable GetMaterialName(int sBU, int catg,int cat_type = 0 )
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {    
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.BizUnit, sBU),
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.CATEGORYTYPE, catg),
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.P_CAT_TYPE, cat_type),
            };
            DataTable dtMaterialName = new DataTable();
            dtMaterialName = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_GetMaterialName, colParameters).Tables[0];
            return dtMaterialName;


        }

        /// <summary>
        /// Get Material type Name List Name
        /// </summary>
        /// <param name="sBU"></param>
        /// <returns></returns>
        public static DataTable GetMaterialTypeName(int sBU)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {    
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.BizUnit, sBU),
            };
            DataTable dtMaterialType = new DataTable();
            dtMaterialType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_GetMaterialCategoryName, colParameters).Tables[0];
            return dtMaterialType;
        }
              
        /// <summary>
        /// Save Compound Details As XML
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns>int</returns>
        public static int SaveCompoundDtls(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.COMPOUNDXML , strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_SaveCompound, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get Compound Details By Compound ID As XML Format
        /// </summary>
        /// <param name="compoundID"></param>
        /// <returns>string</returns>
        public static string GetCompoundDetails(int compoundID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.COMPOUNDPK  ,  compoundID),  
            };
            return Convert.ToString(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_GetCompoundDetails, colParameters));
        }

        /// <summary>
        /// Get Compound List - To List All items In Listing Page
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetCompoundList(GridPrams grid, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME  , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL, grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%",DataAccess.DBService.ParameterType.NVarChar),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ? "COM_PK" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection == null ? "DESC" : grid.SortDirection),
            };
            DataSet dsCompound = new DataSet();
            dsCompound = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_GetCompoundList, colParameters);
            return dsCompound;
        }

        /// <summary>
        /// Delete Compound Details by Compound ID 
        /// </summary>
        /// <param name="compoundID"></param>
        /// <returns>string</returns>
        public static string DeleteCompoundDtls(int compoundID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.COMPOUNDPK,  compoundID == 0 ? (object)DBNull.Value :  compoundID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_DeleteCompoundDtls, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.UOM.Parameters.UOMType_Parameters.RETVAL]).Value);

        }
        /// <summary>
        /// ActivateInactivate Compound Details by Compound ID 
        /// </summary>
        /// <param name="compoundID"></param>
        /// <returns>string</returns>
        public static string ActivateInactivateCompoundDtls(int compoundID,int status,int userPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.P_COMPOUNDPK,  compoundID == 0 ? (object)DBNull.Value :  compoundID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,userPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,status),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_ActivateInActivateCompoundDtls, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.UOM.Parameters.UOMType_Parameters.RETVAL]).Value);

        }
        /// <summary>
        /// For Search Details As AutoComplete In Compoound Master
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY  ,  searchBy),  
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE   ,  searchValue),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit), 
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_CompoundAutoSearch, colParameters).Tables[0];
            return dtSearchValue;

        }

        /// <summary>
        /// Get formulationType Name
        /// </summary>
        /// <param name="sBU"></param>
        /// <returns></returns>
        public static DataTable GetFormulationTypeName(int sBU)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {    
                new DBService.Parameters(GTIService.Constants.Compound.Parameters.BizUnit, sBU),
            };
            DataTable dtFormulationType = new DataTable();
            dtFormulationType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_FormulationTypeName, colParameters).Tables[0];
            return dtFormulationType;
        }
        
        /// <summary>
        /// Get UOM Details For an itm in compound
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="catg"></param>
        /// <param name="uOM"></param>
        /// <returns></returns>
        public static DataTable GetUOMListForItem(int materialPK, int catg)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.COMPITEMPK, materialPK),
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.ITEMCATG, catg)    
            };
            DataTable dtUOM = new DataTable();
            dtUOM = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_UOMDetailsofItem, colParameters).Tables[0];
            return dtUOM;
        }

        /// <summary>
        /// Get UOM List With have Convesion factor, for Item UOM and Compound UOM
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="catg"></param>
        /// <param name="uom"></param>
        /// <returns></returns>
        public static DataTable GetUOMListWithCF(int materialPK, int catg, int uom)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.COMPITEM, materialPK),
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.COMPITEMCATG, catg),
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.COMPUOM, uom)     
            };
            DataTable dtUOM = new DataTable();
            dtUOM = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_UOMListWithConversionFactor, colParameters).Tables[0];
            return dtUOM;


        }

        // 11 July 2011 2PM

        /// <summary>
        /// Get Material UOM By Categor and Material
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="catg"></param>
        /// <returns></returns>
        public static DataTable GetMaterialUOM(int materialPK, int catg)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.ITEMPK, materialPK),
                 new DBService.Parameters(GTIService.Constants.Compound.Parameters.COMP_TYPE, catg),  
            };
            DataTable dtUOM = new DataTable();
            dtUOM = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Compound.Procedures.SP_GetMaterialUOM, colParameters).Tables[0];
            return dtUOM;


        }


    }

    #endregion
}
