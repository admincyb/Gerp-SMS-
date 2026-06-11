using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using System.Data;
using BusinessObject;

namespace DataAccess.UOMManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class UOMMaster
    {
        #region Methods

        /// <summary>
        /// Get UOMType 
        /// </summary>
        /// <returns>Datatable</returns>
        public static DataTable GetUOMType(int sbuPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMTYPEPK, (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMSBU, sbuPK),
            };

            DataTable dtUOMType = new DataTable();
            dtUOMType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.GETUOMTYPE, colParameters).Tables[0];
            return dtUOMType;


        }

        /// <summary>
        /// Get UOM Corresponding to UOM Type 
        /// </summary>
        /// <param name="UOMTypeID"></param>
        /// <returns>Datatable</returns>
        public static DataTable GetUOM(string UOMTypeID, int uomPK, int sbu, string uomType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMTYPE, UOMTypeID=="0" ? (object)DBNull.Value : UOMTypeID),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMPK, uomPK),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMSTATUS, 1),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMBIZUNIT, sbu==0 ? (object)DBNull.Value : sbu),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMTYPENAME , uomType==string.Empty ? (object)DBNull.Value : uomType),
            };

            DataTable dtUnit = new DataTable();
            dtUnit = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.GETUOM, colParameters).Tables[0];
            return dtUnit;


        }

        /// <summary>
        /// Get UOM using New Sp Created By Biju
        /// </summary>
        /// <param name="uomPK"></param>
        /// <param name="Type"></param>
        /// <param name="active"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetUOM(int uomPK, int? uomType, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.P_PK, uomPK),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.P_UOM_TYPE,uomType??(object)DBNull.Value),                
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.P_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.BIZUNIT, bizUnit)
            };
            DataTable dtUnit = new DataTable();
            dtUnit = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.GETUOM_NEW, colParameters).Tables[0];
            return dtUnit;
        }
        /// <summary>
        /// Used To Save UOM
        /// </summary>
        /// <param name="UOMDet"></param>
        /// <returns>string</returns>
        public static int SaveUOM(string strxml)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMXML , strxml),  

                new DBService.Parameters(GTIService.Constants.UOM.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.SAVEUOM, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.UOM.Parameters.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Used to Save UOM Type
        /// </summary>
        /// <param name="UOMDet"></param>
        /// <returns>string</returns>
        public static string SaveUOMType(BusinessObject.UOMManagement.UOM UOMDet)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMType_Parameters.UMTPK , UOMDet.UMT_PK == 0 ? (object)DBNull.Value : UOMDet.UMT_PK),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMType_Parameters.UMTNAME , UOMDet.UMT_NAME),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMType_Parameters.UMTCODE , UOMDet.UMT_CODE==null?string.Empty:UOMDet.UMT_CODE),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMType_Parameters.ACTIVSTATUS ,UOMDet.Status==0?GTIService.Constants.UOM.Parameters.ACTIVESTATUS:UOMDet.Status),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMType_Parameters.BIZUNIT, UOMDet.BizUnit),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMType_Parameters.CREATEDBY, UOMDet.UserPk),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMType_Parameters.MODBY, UOMDet.UserPk),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMType_Parameters.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.SAVEUOMTYPE, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.UOM.Parameters.UOMType_Parameters.RETVAL]).Value);

        }

        /// <summary>
        /// Used To GEt UOM
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static DataSet GetUOMList(GridPrams grid, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),   
              new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOM_Search.SEARCHNAME, grid.SearchBy == string.Empty || grid.SearchBy == "0" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOM_Search.SEARCHVAL, grid.SearchValue == string.Empty ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOM_Search.FIELDS,  grid.Fields== null ? (object)DBNull.Value : grid.Fields),
              new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOM_Search.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOM_Search.PAGESIZE,  grid.PageSize),
              ////new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOM_Search.SORTBY,  grid.SortBy == null ? GTIService.Constants.UOM.Fields.UOMPK: grid.SortBy),
              ////new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOM_Search.SORTDIRC, grid.SortDirection == null ?  GTIService.Constants.UOM.Fields.ORDERBYDESC : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOM_Search.SORTBY,  grid.SortBy == null ? "UMT_NAME" :grid.SortBy),
              new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOM_Search.SORTDIRC, grid.SortDirection==null ? "ASC" :  grid.SortDirection ),


            };

            DataSet dsUOM = new DataSet();
            dsUOM = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.GETUOMLIST, colParameters);
            return dsUOM;
        }

        /// <summary>
        /// Used to get UOM Types
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static DataSet GetUOMTypeList(GridPrams grid, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),   
                 new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMTYpe.   UOMTYPEPK , (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMTYpe.   FIELDS ,  grid.Fields== null ? (object)DBNull.Value : grid.Fields),
                 new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMTYpe.   PAGENO ,  grid.PageNumber),
                 new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMTYpe.   PAGESIZE ,  grid.PageSize),
                 new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMTYpe.  SORTBY ,  grid.SortBy),
                 new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMTYpe. SORTDIRC ,  grid.SortDirection)
            };

            DataSet dsUOM = new DataSet();
            dsUOM = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.GETUOMTYPELIST, colParameters);
            return dsUOM;
        }

        /// <summary>
        /// Used To delete UOM
        /// </summary>
        /// <param name="UOMId"></param>
        /// <returns>string</returns>
        public static string DeleteUOM(int UOMId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   

                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMPK ,  UOMId == 0 ? (object)DBNull.Value :  UOMId),
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.   UOMType_Parameters.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.UOMDELETE, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.UOM.Parameters.UOMType_Parameters.RETVAL]).Value);

        }

        /// <summary>
        /// Used to delete UOM Type
        /// </summary>
        /// <param name="UOMTypeId"></param>
        /// <returns>string</returns>
        public static string DeleteUOMType(int UOMTypeId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.UOM.Parameters. UOMTYpe.UOMTYPEPK,  UOMTypeId == 0 ? (object)DBNull.Value :  UOMTypeId),
                 new DBService.Parameters(GTIService.Constants.UOM.Parameters.   UOMType_Parameters.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.DELETEUOMTYPE, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.UOM.Parameters.UOMType_Parameters.RETVAL]).Value);
        }

        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Seatch Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>Datatable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.UOM.Parameters.FIELDNAME,  searchBy),
              new DBService.Parameters(GTIService.Constants.UOM.Parameters.FIELDVALUE,  searchValue),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.UOMAUTOSEARCH, colParameters).Tables[0];
            return dtSearchValue;

        }

        /// <summary>
        /// Get UOM Details By Material PK
        /// </summary>
        /// <param name="materialPK"></param>
        /// <returns></returns>
        public static DataTable GetUomDtlsByMaterialPk(int materialPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
                new DBService.Parameters(GTIService.Constants.UOM.Parameters. ITEMPK ,  materialPK==0 ?(object)DBNull.Value:materialPK),
             };
            DataTable dtUomDtls = new DataTable();
            dtUomDtls = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.GETITEMUOM, colParameters).Tables[0];
            return dtUomDtls;
        }

        /// <summary>
        /// Get UOMDetails By UOMPk
        /// </summary>
        /// <param name="uOMPK"></param>
        /// <returns>String XML Format</returns>
        public static string GetUOMDetails(int uOMPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.UOMPK   ,  uOMPK),  
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.   UOMType_Parameters.RETVAL , string.Empty,4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            return Convert.ToString(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.GETUOMDTLSXMLASPK, colParameters));

        }

        /// <summary>
        ///  Get given UOM conversion factered UOM's.
        /// </summary>
        /// <param name="uOMPK"></param>
        /// <returns>String XML Format</returns>
        public static DataTable GetUOMConversionsByUOMPK(int uOMPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.UOM.Parameters.P_UOM   ,  uOMPK) 
            };
            DataTable dtUomDtls = new DataTable();
            dtUomDtls = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.UOM.Procedures.SPINV_UOM_CONV_UOM_GET, colParameters).Tables[0];
            return dtUomDtls;
        }

        #endregion

    }
}
