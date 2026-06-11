using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.HRMS.Admin.Masters;
using ERP.Utilities;

namespace DataAccess.HRMS.Admin.Masters
{
    public class EmployeeTypeDL
    {
        public static DataTable GetEmployeeTypeList(int bizUnit, int pageNo, int pageSize, string typeName = null, string typeCode = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM  , pageNo), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE  , pageSize),    
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_EMT_NAME, typeName!=null?typeName!=string.Empty?typeName:(object)DBNull.Value:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_EMT_CODE, typeCode!=null?typeCode!=string.Empty?typeCode:(object)DBNull.Value:(object)DBNull.Value)
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_EMP_TYPE_GET_LIST
                , colParameters);
            return dsSet.Tables[0];
        }

        public static DataTable GetEmployeeTypeGetKV(int? empTypePk, int bizUnit, int active = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_EMT_PK  , empTypePk?? (object)DBNull.Value), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE  , active),    
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_EMP_TYPE_GET_KV
                , colParameters);
            return dsSet.Tables[0];
        }

        public static EmployeeTypeBO GetEmployeeTypeByID(int pk, int active)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_EMT_PK,pk)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure
                                , GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_EMP_TYPE_GET_XML
                                , colParameters)
                                .Tables[0];

            string objXml = string.Empty;

            foreach (DataRow row in dtList.Rows)
            {
                objXml += row[0].ToString();
            }

            EmployeeTypeBO empType = CommonFunctions.XmlDeserialize<EmployeeTypeBO>(objXml);
            return empType;
        }

        public static int Save(EmployeeTypeBO empType)
        {
            string xmlDoc = CommonFunctions.XmlSerialize(empType);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_EMP_TYPE_SAVE
                , colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

            return result;
        }

        public static int DeleteEmployeeType(int pk, DateTime lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_EMT_PK , pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_LAST_MOD_DT , lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_EMP_TYPE_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static DataTable GetWorkingDays(int empTypePk, int active = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_EMT_PK  , empTypePk),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_ESH_ACTIVE  , active)   
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_EMP_TYPE_WORK_HRS_GET
                , colParameters);
            return dsSet.Tables[0];
        }

        public static int UpdateEmployeeTypeStatus(int currPK, int status, int userPK, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_EMT_PK  , currPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE  , status),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK  , userPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT  , lastModDate == string.Empty  ? (object)DBNull.Value : lastModDate),  
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_EMP_TYPE_MST_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL]).Value);
            return result;
        }
    }
}
