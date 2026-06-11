using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities;

namespace DataAccess.HRMS.Admin.Masters
{
    public class LeaveTemplateDL
    {
        public static DataTable GetLeaveTemplateList(int bizUnit, int pk = 0, int active = 1)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LTE_PK ,pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ACTIVE ,active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT ,bizUnit)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure
                                , GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_LEAVE_TEMP_GET_KV
                                , colParameters)
                                .Tables[0];
            return dtList;
        }

        public static int UpdateLeaveTemplateStatus(int currPK, int status, int userPK, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LTE_PK  , currPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE  , status),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK  , userPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT  , lastModDate == string.Empty  ? (object)DBNull.Value : lastModDate),  
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_LEAVE_TEMP_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL]).Value);
            return result;
        }


        /// <summary>
        /// Method to Save Leave Template
        /// </summary>
        /// <param name="leaveTemplate">BusinessObject.HRMS.Admin.Masters.LeaveTemplateBO.LeaveTemplate</param>
        /// <returns>int</returns>
        public static int SaveLeaveTemplate(BusinessObject.HRMS.Admin.Masters.LeaveTemplateBO.LeaveTemplate leaveTemplate)
        {
            string xmlDoc = CommonFunctions.XmlSerialize(leaveTemplate);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_LEAVE_TEMP_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to Get Leave Templates for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetLeaveTemplateListPage(BusinessObject.GridPrams gridParam, int bizUnit, string tempalteCode = null,string sortOrder=null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              //new DBService.Parameters(Parameters.P_SER_NAME , grid.SearchBy ==Fields.STRINGEMPTY ? (object)DBNull.Value : grid.SearchBy ),
              //new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_STE_NAME , gridParam.SearchValue == Parameters.STRINGEMPTY ? (object)DBNull.Value : Fields.VALUE_PERC + gridParam.SearchValue + Fields.VALUE_PERC),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PAGE_NUM ,  gridParam.PageNumber),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PAGE_SIZE,  gridParam.PageSize),
               new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_BIZUNIT, bizUnit),
               new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LTE_NAME, gridParam.SearchValue == GTIService.Constants.HRMS.Admin.Masters.Parameters.STRINGEMPTY 
                    ? (object)DBNull.Value : GTIService.Constants.HRMS.Admin.Masters.Fields.VALUE_PERC + gridParam.SearchValue + GTIService.Constants.HRMS.Admin.Masters.Fields.VALUE_PERC),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LTE_CODE, tempalteCode!=null ? tempalteCode!=string.Empty?tempalteCode:(object)DBNull.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_SORT_BY, sortOrder!=null ? sortOrder!=string.Empty?sortOrder:(object)DBNull.Value:(object)DBNull.Value),
              //new DBService.Parameters(Parameters.P_FIELDS, grid.Fields == Fields.STRINGEMPTY ? Fields.VALUE_STAR : grid.Fields),              
              //new DBService.Parameters(Parameters.P_SORT_BY,  grid.SortBy == null ||grid.SortBy ==GTIService.Constants.DirectStockTransfer.Fields.GRH_DATE|| grid.SortBy == GTIService.Constants.DirectStockTransfer.Fields.GRH_NO ?GTIService.Constants.DirectStockTransfer.Fields.GRH_PK : grid.SortBy),
              //new DBService.Parameters(Parameters.P_SORT_DIR , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              //new DBService.Parameters(Parameters.P_FROM_DT, grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              //new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              //new DBService.Parameters(Parameters.P_USER_PK ,  objUser.PKUser),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS ,  string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),
             
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_LEAVE_TEMP_GET_LIST, colParameters).Tables[0];
        }


        /// <summary>
        /// Method to Delete Leave Template Details
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteLeaveTemplate(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LTE_PK , pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_LEAVE_TEMP_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// <summary>
        /// Get LeaveTemplate XML
        /// </summary>
        /// <param name="templatePk">int</param>
        /// <returns>string</returns>
        public static string LeaveTemplateGetXML(int templatePk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LTE_PK,  templatePk == 0 ? (object) DBNull.Value :  templatePk)
            };
            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure,
                GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_LEAVE_TEMP_GET_XML, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }

    }
}
