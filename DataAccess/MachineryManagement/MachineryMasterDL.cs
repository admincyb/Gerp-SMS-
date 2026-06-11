using System;
using System.Data;
using BusinessObject;

namespace DataAccess.MachineryManagement
{

    /// <summary>
    /// Class Used for Access all Machine releted function
    /// </summary>
    public class MachineryMasterDL
    {
        #region Methods

        //================================================================================================================================================
        
        /// <summary>
        /// Save Machine type Details
        /// </summary>
        /// <param name="customerName"></param>
        /// <returns>int- MachineTypePk</returns>
        public static int SaveMachineType(BusinessObject.MachineryManagement.Machinery.MachineTypeMaster machineType)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MACHINETYPEPK ,   machineType.MachineTypePK == 0 ? (object)DBNull.Value :  machineType.MachineTypePK) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MACHINETYPECODE ,   machineType.MachineTypeCode) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MACHINETYPENAME ,   machineType.MachineTypeName) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MACHINETYPEACTIVE ,   machineType.Status==0?1:machineType.Status) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters. MACHINETYPESBU ,   machineType.SBU) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MACHINETYPEMODBY,   machineType.UserPK) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)            
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.SAVEMACHINETYPEDTLS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Machinery.Parameters.RETVAL]).Value);


        }
        /// <summary>
        /// Get machine Type Details
        /// </summary>
        /// <param name="machineTypePK"></param>
        /// <returns>Datatable - Machine Type Details</returns>
        public static DataTable GetMachineType( int sBU)
        {
            DBService dbService = new DBService();
            DataTable dtMachineType = new DataTable();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MACHINETYPEPK , (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters. MACHINETYPESBU,  sBU),
             
              
            };
            dtMachineType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETMACHINETYPEDTLSCOMBO, colParameters).Tables[0];
            return dtMachineType;
        }
        /// <summary>
        /// Get MachineType Details For Listing
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>Dataset - Table[0]- Count, table[1] - MachineTypeDetails</returns>
        public static DataSet GetMachineTypeDtls(GridPrams grid, int sBU)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sBU),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy == null ? GTIService.Constants.Machinery.Parameters.MACHINETYPEPK : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "DESC": grid.SortDirection),
            };
            DataSet dsMachineType = new DataSet();
            dsMachineType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETMACHINETYPEDTLS, colParameters);
            return dsMachineType;


        }
        /// <summary>
        /// Delete MachineTypeDetails By MachineID
        /// </summary>
        /// <param name="machineTypeID"></param>
        /// <returns>int 1= Success</returns>
        public static int DeleteMachineTypeDtls(int machineTypeID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MACHINETYPEPK , machineTypeID),
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.DELETEMACHINETYPEDTLS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Machinery.Parameters.RETVAL]).Value);
        }

        //=====================================================================  Fill DropDown ==============================================================
      
        /// <summary>
        /// Get RunByDetails
        /// </summary>
        /// <param name="runByDtlsID"></param>
        /// <returns>Datatabel - RunBy Details</returns>
        public static DataTable GetRunByDtls(int sBU)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
                
                 new DBService.Parameters(GTIService.Constants.Machinery.Parameters.RUNBYPK , (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT ,  sBU),
             };
            DataTable dtRunByType = new DataTable();
            dtRunByType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETRUNBYDTLS, colParameters).Tables[0];
            return dtRunByType;
        }

        /// <summary>
        /// Get Maintance Type Details
        /// </summary>
        /// <param name="maintenceType"></param>
        /// <returns>Datatable- MainatanceType Details</returns>
        public static DataTable GetMainteanceType(int sBU)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
               
                 new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MAINTENACEPK , (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MAINTENACEBIZUNIT ,  sBU),
             };
            DataTable dtMaintaenace = new DataTable();
            dtMaintaenace = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETMAINANCETYPE, colParameters).Tables[0];
            return dtMaintaenace;
        }
        /// <summary>
        /// Get FreequencyType Details
        /// </summary>
        /// <param name="freequncyType"></param>
        /// <returns>Datatable- Freequncy Type Details</returns>
        public static DataTable GetFrequencyType(int sBU)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
               
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.FREQUENCYPK , (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.FREQUENCYBIZUNIT,  sBU),
             };
            DataTable dtFreequencyType = new DataTable();
            dtFreequencyType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETFREEQUENCYTYPE, colParameters).Tables[0];
            return dtFreequencyType;
        }


        public static DataTable GetMeasuringType(int sBU, int Active, string CfgType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {

                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CfgPK , (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE , Active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT , sBU),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CFG_TYPE , CfgType)
             };
            DataTable dtMeasuringType = new DataTable();
            dtMeasuringType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPADM_CONFIG_MST_GET_KV, colParameters).Tables[0];
            return dtMeasuringType;
        }



        //================================================================================================================================================

        /// <summary>
        /// Save Location type Details
        /// </summary>
        /// <param name="customerName"></param>
        /// <returns>int - LocationPK</returns>
        public static int SaveLocation(BusinessObject.MachineryManagement.Machinery.LocationMaster locMaster)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                             
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.LOCATIONPK ,   locMaster.LocationPK == 0 ? (object)DBNull.Value :  locMaster.LocationPK) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.LOCNAME ,   locMaster.LocationName) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.LOCATIONMODBY ,   locMaster.UserPK) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.LOCATIONBIZUNIT ,   locMaster.SBU) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)            
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.SAVELOCATIONDTLS, colParameters);

            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Machinery.Parameters.RETVAL]).Value);

        }
       //==============================================================================   Location ======================================================
        /// <summary>
        /// Get Location Details
        /// </summary>
        /// <param name="sbuPK"></param>
        /// <returns>Datatable- Location Details</returns>
        public static DataTable GetLocation(int sbuPK)
        {
            DBService dbService = new DBService();
            DataTable dtLocation = new DataTable();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.Machinery.Parameters.LOCATIONBIZUNIT , sbuPK),
              

            };
            dtLocation = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETLOCATIONDTLS, colParameters).Tables[0];
            return dtLocation;
        }
        public static DataSet GetLocationList(GridPrams grid, int sbuPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.LOCATIONBIZUNIT , sbuPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection),
            };
            DataSet dtMaterial = new DataSet();
            dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETLOCATIONDTLSLIST, colParameters);
            return dtMaterial;

        }
        /// <summary>
        /// Delete Location Details By LocationID
        /// </summary>
        /// <param name="locationID"></param>
        /// <returns>int - 1= Success</returns>
        public static int DeleteLocationDtls(int locationID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.Machinery.Parameters.LOCATIONPK , locationID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.DELETELOCATIONDTLS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);
        }
        
        
        
       
        
        /// <summary>
        /// Get  Location Details For Listing
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>Dataset - Table[0]- Count, table[1] - Location Details</returns>
        public static DataSet GetLocationDtls(GridPrams grid)
        {
            
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.PK , grid.SearchBy == GTIService.Constants.Machinery.Fields.DEFAULTSEARCHBY? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.FIELDS ,  grid.Fields),
              new DBService.Parameters( GTIService.Constants.Machinery.Parameters.PAGENUMBER,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.SORTBY ,  grid.SortBy == null ? GTIService.Constants.Machinery.Fields.LOCATIONNAME : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.SORTDIRECTION , grid.SortDirection == null ? GTIService.Constants.Machinery.Fields.ORDERBYASC : grid.SortDirection),
            };
            DataSet dsLocation = new DataSet();
            dsLocation = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETLOCATIONDTLS, colParameters);
            return dsLocation;
        }
      //=================================================== Machine ==============================================
        /// <summary>
        /// Save Machine Details 
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns>int - Machine PK</returns>
        public static int SaveMachineDetails(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MACHINEXML  , strxml), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.SAVEMACHINEDTLS, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Get Machine Details List
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>Dataset - Table[0]- Count, table[1] - Machine Details</returns>
        public static DataSet GetMachineList(GridPrams grid, int sBU, int statusPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sBU),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME  , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL, grid.SearchValue == string.Empty ? (object)DBNull.Value : grid.SearchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ? "MCH_PK" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection == null ? "DESC" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.MCH_ACTIVE,statusPk == -1 ?(object)DBNull.Value:statusPk)
            };
            DataSet dsMachineList = new DataSet();
            dsMachineList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETMACHINELIST, colParameters);
            return dsMachineList;
        }
        /// <summary>
        ///   Methord to get the Search Vlaues Corresponding to Seatch Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>Datatable- Search Details </returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int sBU)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sBU),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY  ,  searchBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE ,  searchValue),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;
        }

        /// <summary>
        /// Delete machine Details By MachineID
        /// </summary>
        /// <param name="machineID"></param>
        /// <returns>int - 1- Success</returns>
        public static int DeleteMachineDtls(int machineID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MACHINEID , machineID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.DELETEMACHINEDTLS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);
        }

        /// <summary>
        /// Gat Machine Details By Machine ID 
        /// </summary>
        /// <param name="machineID"></param>
        /// <returns>String - XML Format- machine Details</returns>
        public static string GetMachineDetails(int machineID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MACHINEID  ,  machineID),  
                
            };
         
            return Convert.ToString(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETMACHINEDTLS, colParameters));
        }


        public static DataTable GetMachineNameByNamechineType(int sBU, int machType,int? processID=null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {    
                 // According to Dhanya this must need confirmation, so this is commented by Biju
                 //new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MCHTYPEPK ,  machType==0? (object)DBNull.Value :machType ),
                 new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MCHTYPEPK , (object)DBNull.Value ),
                 new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MACHINEBIZUNIT ,  sBU),
                 new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MPM_PROCESS ,  processID ?? (object)DBNull.Value),
             };
            DataTable dtRunByType = new DataTable();
            dtRunByType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETMACHINENAMEBYTYPE, colParameters).Tables[0];
            return dtRunByType;
        }
            
        #endregion

    }
}
