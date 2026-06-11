using System;
using System.Data;
using BusinessObject;

namespace DataAccess.Administration.Masters
{
   public class TankMasterDL
    {
        /// <summary>
        /// Get TankType
        /// </summary>
        /// <param name="sbuPk"></param>
        /// <returns>int- 1(Success)</returns>
        public static DataTable GetTankType(int sbuPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKBIZUNIT,  sbuPk)
                
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Tank.Procedures.GETTANKTYPE, colParameters).Tables[0];

        }
        public static DataSet GetTankTypeList(GridPrams grid,int sbuPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                
              new DBService.Parameters(GTIService.Constants.Tank.Parameters.BIZUNIT,  sbuPk),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection),
            };
            DataSet dtMaterial = new DataSet();
            dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Tank.Procedures.GETTANKTYPELIST, colParameters);
            return dtMaterial;
            
        }
        /// <summary>
        /// Used to delete Tank Type
        /// </summary>
        /// <param name="TankTypeId"></param>
        /// <returns>string</returns>
        public static string DeleteTankType(int TankTypeId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKTYPEPK,  TankTypeId == 0 ? (object)DBNull.Value :  TankTypeId),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Tank.Procedures.DELETETANKTYPE, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);
        }
        /// <summary>
        /// Used to Save TankType
        /// </summary>
        /// <param name="tankDet"></param>
        /// <returns>string</returns>
        public static string SaveTankType(BusinessObject.Administration.Masters.TankType tankDet)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKTYPEPK , tankDet.TankTypePk== 0 ? (object)DBNull.Value : tankDet.TankTypePk),
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKTYPENAME , tankDet.TankTypeName),
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKBIZUNIT,  tankDet.SBU),
                
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKMODBY, tankDet.UserPk),
                
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Tank.Procedures.SAVETANKTYPE, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);

        }
        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetTankMasterList(GridPrams grid, int bizUnit, int statusPk)
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
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TNK_ACTIVE,statusPk == -1 ?(object)DBNull.Value:statusPk)
            };

            DataSet dtMaterial = new DataSet();
            dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Tank.Procedures.GETTANKMASTERLISTBYSRH, colParameters);
            return dtMaterial;


        }
        /// <summary>
        /// SAVING TankMaster DETAILS
        /// </summary>
        /// <param name="tankDet"></param>
        /// <returns> INT</returns>
        public static string SaveTankMaster(BusinessObject.Administration.Masters.TankMaster tankDet,string xmlStr="")
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {               
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKMASTERPK,   tankDet.TankMasterPk == 0 ? (object)DBNull.Value: tankDet.TankMasterPk) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKNAME ,   tankDet.TankName) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKLOCATION,   tankDet.Location) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKTYPE,   tankDet.TankType) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKCAPACITY,   tankDet.TankCapacity) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.UOMPK,   (tankDet.UOMPk==null || tankDet.UOMPk == 0) ?(object)DBNull.Value : tankDet.UOMPk) ,
               
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKMASTERMODBY,   tankDet.UserPk) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKMASTERBIZUNIT,   tankDet.SBU) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKCODE,   tankDet.TankCode) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKHEIGHT,   tankDet.TankHeight) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKCAPACITYPERCM,   tankDet.CapacityperCM) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TNKSEQ,   tankDet.Sequence) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKSLOPE,   tankDet.TankSlope) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKREMARKS,   tankDet.Remarks) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TNKGENCODE,   string.IsNullOrEmpty(tankDet.CompoundGenNo)?(object)DBNull.Value:tankDet.CompoundGenNo) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TNKLINE,  tankDet.Line > 0 ? tankDet.Line :(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TNK_PLANT,  tankDet.TNK_PLANT > 0 ? tankDet.TNK_PLANT :(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.P_XML, xmlStr) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TNK_HAS_STOCK, string.IsNullOrEmpty(tankDet.HasStock)==true?0:1) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.LASTMODDATE,   tankDet.LAST_MOD_DT == string.Empty ? (object)DBNull.Value: tankDet.LAST_MOD_DT) ,
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TNKACTIVE,string.IsNullOrEmpty(tankDet.TankActive)==true?0:1) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            //P_XML
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Tank.Procedures.SAVETANKDETAILS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value).ToString();
        }
        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
       
        /// <returns>DataTable</returns>
        public static DataTable GetTankMasterSearchValues(string searchValue, string searchBy,int sbuPK)
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
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Tank.Procedures.GETTANKMASTERSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Delete Tank Details By TANKMASTERPK
        /// </summary>
        /// <param name="tankMasterPk"></param>
        /// <returns>int- 1(Success)</returns>
        public static int DeleteTankMasterDtls(int tankMasterPk, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKMASTERPK,  tankMasterPk == 0 ? (object)DBNull.Value :  tankMasterPk),
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.LASTMODDATE,  lastModDate == string.Empty ? (object)DBNull.Value : lastModDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Tank.Procedures.DELETETANKMASTERDETAILS, colParameters);

            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);



        }
       /// <summary>
       /// Get TankName By Tank Type
       /// </summary>
       /// <param name="sbuPk"></param>
       /// <param name="typePk"></param>
       /// <returns></returns>

        public static DataTable GetTankName(int sbuPk, int typePk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.TANKTYPE,  typePk),
                 new DBService.Parameters(GTIService.Constants.Tank.Parameters.TNKBIZUNIT,  sbuPk)
                
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Tank.Procedures.GETTANKNAME, colParameters).Tables[0];

        }
       /// <summary>
       /// To get Lines from LineMaster
       /// </summary>
       /// <param name="bizUnit"></param>
       /// <returns></returns>
        public static DataTable GetAllLine(int bizUnit,int LinePk = 0,int Status = 1,int IsVirtual=-1)// to bind shift in Bin List page
        {
            //Data Table for holding shift reports
            DataTable dtLine;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.P_LNE_PK,LinePk),
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.P_LNE_ACTIVE,Status),
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.BIZUNIT,bizUnit),
                new DBService.Parameters(GTIService.Constants.Tank.Parameters.LNE_VIRTUAL,IsVirtual==-1 ?(object)DBNull.Value : IsVirtual ),
            };
            dtLine = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Tank.Procedures.SP_GETLINES, colParameters).Tables[0];

            return dtLine;
        }
    }
}
