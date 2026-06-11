using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using GTIService.Constants.HRMS.Admin.Masters;
using ERP.Utilities.HRMS;

namespace DataAccess.HRMS.Admin.Masters
{
    public class SlabDefinitionMasterDL
    {
        //Get Employee Leave Master List
        public static string GetEmployeeLeaveList(int currPk, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_PK, currPk==0?(object)DBNull.Value:currPk) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_BIZUNIT, bizUnit),
            };

            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OB_LEAVE_GET_XML, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }

        

        /// <summary>
        /// Method to Delete EmployeeLeaveMaster
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteEmployeeLeaveMaster(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_PK , pk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT , lastModifiedDate),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OB_LEAVE_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to Get Current Leave Credit
        /// </summary>
        /// <param name="employeeID"></param>
        ///<param name="leaveTypeID"></param>
        ///<param name="date"></param>
        /// <returns>int</returns>
        public static int GetCurrentLeaveCredit(int employeeID, int leaveTypeID, string date)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            //colParameters = new DBService.Parameters[] 
            //{     
            //    new DBService.Parameters(Parameters.P_GRH_PK , pk),
            //    new DBService.Parameters(Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
            //    new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            //};
            //dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_DELETE, colParameters);
            //int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            int result = 0;
            return result;
        }

        /// <summary>
        /// Method to Get Leave Credit List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetEmployeeLeaveListListingPage(BusinessObject.GridPrams gridParam, int bizUnit, string FilterFromDate, string FilterToDate, string Employee)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              //new DBService.Parameters(Parameters.P_SER_NAME , grid.SearchBy ==Fields.STRINGEMPTY ? (object)DBNull.Value : grid.SearchBy ),
              //new DBService.Parameters(Parameters.P_SER_VAL , grid.SearchValue == Fields.STRINGEMPTY ? (object)DBNull.Value : Fields.VALUE_PERC + grid.SearchValue + Fields.VALUE_PERC),
              new DBService.Parameters(Parameters.P_PAGE_NUM ,  gridParam.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  gridParam.PageSize),
              new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_DATE_FROM, FilterFromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(FilterFromDate) ),
              new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_DATE_TO, FilterToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(FilterToDate) ),
              new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ELH_EMPLOYEE, Employee==string.Empty?(object)DBNull.Value:Employee) ,

             // new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_PK, gridParam.SearchValue==string.Empty?(object)DBNull.Value:gridParam.SearchValue) 
              //new DBService.Parameters(Parameters.P_FIELDS, grid.Fields == Fields.STRINGEMPTY ? Fields.VALUE_STAR : grid.Fields),              
              //new DBService.Parameters(Parameters.P_SORT_BY,  grid.SortBy == null ||grid.SortBy ==GTIService.Constants.DirectStockTransfer.Fields.GRH_DATE|| grid.SortBy == GTIService.Constants.DirectStockTransfer.Fields.GRH_NO ?GTIService.Constants.DirectStockTransfer.Fields.GRH_PK : grid.SortBy),
              //new DBService.Parameters(Parameters.P_SORT_DIR , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
             // new DBService.Parameters(Parameters.P_FROM_DT, grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              //new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              //new DBService.Parameters(Parameters.P_USER_PK ,  objUser.PKUser),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS ,  string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),
              new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
             // new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_PK, gridParam.SearchValue==string.Empty?(object)DBNull.Value:gridParam.SearchValue) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OB_LEAVE_GET_LIST, colParameters);
        }
        /// <summary>
        /// Method to Save Salary slab definitions
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static int SaveSlabDefinition(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_XML , xmlstr),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_SLAB_HDR_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to Get Salary Slab Definition List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataSet GetSlabDefinitionList(FilterParameters gridParam, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
                new DBService.Parameters(Parameters.P_CODE,  !string.IsNullOrEmpty(gridParam.Code)? gridParam.Code : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_NAME, !string.IsNullOrEmpty(gridParam.Name)? gridParam.Name : (object)DBNull.Value), 
                //new DBService.Parameters(Parameters.P_EFFECT_FROM,  gridParam.FromDate.HasValue? gridParam.FromDate : (object)DBNull.Value),
                //new DBService.Parameters(Parameters.P_EFFECT_TO,  gridParam.ToDate.HasValue? gridParam.ToDate : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_PAY_ELEMENT,  gridParam.PayElementPk.HasValue? gridParam.PayElementPk : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_ACTIVE,  gridParam.Status.HasValue? gridParam.Status : (object)DBNull.Value),             
                new DBService.Parameters(Parameters.P_PAGE_NUM ,  gridParam.PageNumber),
                new DBService.Parameters(Parameters.P_PAGE_SIZE,  gridParam.PageSize),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit)         
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_SLAB_GET_LIST, colParameters);
        }

        /// <summary>
        /// Method to get Slab definition details
        /// </summary>
        /// <param name="SlabPk"></param>
        /// <param name="bizUnit"></param>
        /// <param name="Staus"></param>
        /// <param name="PayElement"></param>
        /// <returns></returns>
        public static string GetSlabDefinitionDetails(int SlabPk, int bizUnit, int Staus, int? PayElement = null, int? VersionPk = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_PHS_PK, SlabPk == 0? (object)DBNull.Value : SlabPk) , 
                new DBService.Parameters(Parameters.P_PHS_PAY_ELEMENT, PayElement.HasValue? PayElement : (object)DBNull.Value), 
                 new DBService.Parameters(Parameters.P_PEV_PK, VersionPk.HasValue? VersionPk : (object)DBNull.Value), 
                new DBService.Parameters(Parameters.P_ACTIVE, Staus),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
            };

            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_SLAB_HDR_GET_KV, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }

        /// <summary>
        /// Method to delete Slab definition
        /// </summary>
        /// <param name="SlabPk">PK</param>
        /// <param name="LastModDate">Modified date</param>
        /// <returns></returns>
        public static int DeleteSlabDefinition(int SlabPk, int? VersionPk,  DateTime LastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_PHS_PK , SlabPk),
                new DBService.Parameters(Parameters.P_PEV_PK , VersionPk.HasValue? VersionPk : (object)DBNull.Value), 
                new DBService.Parameters(Parameters.P_LAST_MOD_DT , LastModDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_SLAB_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static int UpdateSlabDefinitionStatus(int SlabPk, int Status, int UserPk, DateTime? LastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_PHS_PK , SlabPk),
                new DBService.Parameters(Parameters.P_ACTIVE , Status),
                new DBService.Parameters(Parameters.P_USER_PK , UserPk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT , LastModDate.HasValue? LastModDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_SLAB_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }
    }
}
