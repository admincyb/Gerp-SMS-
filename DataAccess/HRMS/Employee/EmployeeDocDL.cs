using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.HRMS.Employee;
using System.Data.Common;
using ERP.Utilities;
using ERP.Utilities.HRMS;

namespace DataAccess.HRMS.Employee
{
    public sealed class EmployeeDocDL
    {
        public static DataTable GetEmployeeDocs(int employeePK, int bizUnit, string sortBy = "empPK", string sortDirection = "ASC")
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPPK,employeePK),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT ,bizUnit),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SORT_BY ,sortBy),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SORT_DIR ,sortDirection)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EMPDOCLIST, colParameters).Tables[0];
            return dtList;
        }

        public static EmployeeDoc GetEmployeeDocByDocID(int docID, int active)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMP_DOC_PK,docID)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EMPLOYEE_DOC_DETAILS, colParameters).Tables[0];

            string objXml = string.Empty;

            foreach (DataRow row in dtList.Rows)
            {
                objXml += row[0].ToString();
            }

            //string xmlDoc = CommonFunctions.XmlSerialize(dtList);
            EmployeeDoc empDoc = CommonFunctions.XmlDeserialize<EmployeeDoc>(objXml);
            return empDoc;
        }

        public static DataTable GetEmployeeDocTypeList(string configType, int configPK = 0, int active = 1, int bizUnit = 1)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CFG_PK,configPK),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CFG_TYPE,configType),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ACTIVE,active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT,bizUnit)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_CONFIG_MST_GET_KV, colParameters).Tables[0];

            return dtList;
        }

        public static DataTable GetAutoCompleteEmployeeList(int? empPK, string searchKey = "", int active = 1, int? empCategory = null, int? empBranch = null, int? empType = null, int? employmentType = null, int? empCompany = null, int? EmpDept = null, int? empDesignation = null, int? PaymentMode = null, int? EmpCurrency = null, string toDate = null)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK,(empPK.HasValue && empPK > 0) ? empPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPNAME,searchKey),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_Active,active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPCATEGORY,empCategory.HasValue? empCategory : (object)DBNull.Value),               
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EmpBranch,(empBranch.HasValue && empBranch > 0) ?  empBranch : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_EMP_TYPE,(empType.HasValue && empType > 0) ? empType : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMP_EMPLOYMENT_Type,(employmentType.HasValue && employmentType > 0) ? employmentType : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMP_COMPANY,(empCompany.HasValue && empCompany > 0) ? empCompany : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_empDepartment, (EmpDept.HasValue && EmpDept > 0) ? EmpDept : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_empDesignation, (empDesignation.HasValue && empDesignation > 0) ? empDesignation : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_PAY_MODE, (PaymentMode.HasValue && PaymentMode > 0) ? PaymentMode : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_CURRENCY, (EmpCurrency.HasValue && EmpCurrency > 0) ? EmpCurrency : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_TO_DATE, (toDate!=string.Empty) ? toDate : (object)DBNull.Value)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_AUTOCOMPLETEMPLOYEE, colParameters).Tables[0];
            return dtList;
        }

        public static DataTable GetAutoCompleteEmployeeListByFilter(int? empPK, string searchKey = "", int active = 1, int? empCategory = null, string filter = "", int? EmpDept = null, int? EmpType = null,int? ProcessMode = null, string toDate = null)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK,empPK),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPNAME,searchKey),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPCATEGORY,empCategory.HasValue? empCategory : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_Active,active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EmpBranch,(!string.IsNullOrEmpty(filter) && Convert.ToInt32(filter) > 0) ? filter : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_empDepartment, (EmpDept.HasValue && EmpDept > 0) ? EmpDept : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_EMP_TYPE,EmpType.HasValue? (EmpType >= 0 ? EmpType : (object)DBNull.Value) : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_PTM_PRC_MODE,ProcessMode.HasValue? (ProcessMode >= 0 ? ProcessMode : (object)DBNull.Value) : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_TO_DATE, (toDate!=string.Empty) ? toDate : (object)DBNull.Value)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_AUTOCOMPLETEMPLOYEE, colParameters).Tables[0];

            return dtList;
        }


        //public static DataTable GetAutoCompleteAgentList()
        //{
        //     DataTable dtList;
        //    DBService dbService = new DBService();
        //    DBService.Parameters[] colParameters = null;
        //    colParameters = new DBService.Parameters[] 
        //    {   
     
        //    };
        //}









        public static DataTable GetAutoCompleteEmployeePayrollList(int? empPK,string FromDate,string ToDate, string searchKey = "", int active = 1, int? empCategory = null, int? empBranch = null, int? empType = null, 
            int? employmentType = null, int? empCompany = null, int? EmpDept = null, int? empDesignation = null, int? PaymentMode = null, int? EmpCurrency = null,int? EmpPayrollType=null )
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK,(empPK.HasValue && empPK > 0) ? empPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPNAME,searchKey),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_Active,active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPCATEGORY,empCategory.HasValue? empCategory : (object)DBNull.Value),               
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EmpBranch,(empBranch.HasValue && empBranch > 0) ?  empBranch : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_EMP_TYPE,(empType.HasValue && empType > 0) ? empType : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMP_EMPLOYMENT_Type,(employmentType.HasValue && employmentType > 0) ? employmentType : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMP_COMPANY,(empCompany.HasValue && empCompany > 0) ? empCompany : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_empDepartment, (EmpDept.HasValue && EmpDept > 0) ? EmpDept : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_empDesignation, (empDesignation.HasValue && empDesignation > 0) ? empDesignation : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_PAY_MODE, (PaymentMode.HasValue && PaymentMode > 0) ? PaymentMode : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_CURRENCY, (EmpCurrency.HasValue && EmpCurrency > 0) ? EmpCurrency : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_FROM_DT, (FromDate !=string.Empty) ? FromDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_TO_DT, (ToDate !=string.Empty) ? ToDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_PAYROLL_TYPE, (EmpPayrollType.HasValue && EmpPayrollType > 0) ? EmpPayrollType : (object)DBNull.Value),
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_PAYROLL_EMP_GET, colParameters).Tables[0];
            return dtList;
        }

        public static int Save(EmployeeDoc doc, out string empName)
        {
            string xmlDoc = CommonFunctions.XmlSerialize(doc);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_XML,xmlDoc),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SAVE_EMPLOYEE_DOC, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            empName = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static int DeleteEmployeeDoc(int docID, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMP_DOC_PK,docID),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_LAST_MOD_DT, lastModDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_DOC_DTL_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetEmployeeDocsLog(int docID)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMP_DOC_PK,docID)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EMPDOCLOGLIST, colParameters).Tables[0];

            return dtList;
        }

        public static DataTable GetEmpDocByFilterOptions(EmpDocumentFilterParameterBinder filterOptions)
        {
            List<EmployeeDocumentDetails> docList = null;

            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            //Change ParamName based on SP
            colParameters = new DBService.Parameters[] 
            {       
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMP_DOC_PK,filterOptions.DocPk),

                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPCODE,
                                            filterOptions.EmployeeCode.IsNullOrEmptyOrWhitespace()?(object)DBNull.Value:filterOptions.EmployeeCode,
                                            DBService.ParameterType.NVarChar),

                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMP_NAME,
                                            filterOptions.EmployeeName.IsNullOrEmptyOrWhitespace()?(object)DBNull.Value:filterOptions.EmployeeName,
                                            DBService.ParameterType.NVarChar),

                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDD_DOC_NO,
                                            filterOptions.DocNo.IsNullOrEmptyOrWhitespace()?(object)DBNull.Value:filterOptions.DocNo,
                                            DBService.ParameterType.NVarChar),

                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDD_EXPIRES_ON,
                                            filterOptions.ExpiresBefore.IsNullOrEmptyOrWhitespace()?(object)DBNull.Value:filterOptions.ExpiresBefore,
                                            DBService.ParameterType.NVarChar),

                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDD_EXPIRES_IN,filterOptions.ExpiringIn,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPNATIONALITY,filterOptions.Nationality,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPCOMPANY,filterOptions.Company,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDD_IS_CHECKED_FOR,filterOptions.IsCheckedFor,DBService.ParameterType.SByte),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDD_DOC_TYPE,filterOptions.DocumentType,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SORT_BY,filterOptions.SortBy,DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SORT_DIR,filterOptions.SortDirection,DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE,filterOptions.Employee,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT,filterOptions.BizUnit,DBService.ParameterType.Int32)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SHHRM_EMP_DOC_DTL_GET_LIST, colParameters).Tables[0];

            //docList=dtList.ToList<EmployeeDocumentDetails>();
            return dtList;
        }

        public static int CheckInOrCheckOut(EmployeeDocumentCheckInCheckOutParameterBinder parameter)
        {
            string xmlDoc = CommonFunctions.XmlSerialize(parameter);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(
                                CommandType.StoredProcedure,
                                GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_DOC_TRX_DTL_SAVE,
                                colParameters
                                );
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

            return result;
        }

        public static DataTable GetInternalOrExtenalType(ConfigurationParameterBinder parameter)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CFG_PK,parameter.ConfigPk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ACTIVE,parameter.Active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CFG_TYPE,parameter.ConfigType)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_CONFIG_MST_GET_KV, colParameters).Tables[0];

            return dtList;
        }

        public static DataTable GetIssuedFor(ConfigurationParameterBinder parameter)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {    
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_GROUP_TYPE_VALUE,parameter.GroupTypeValue),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_GROUP_VALUE,parameter.GroupValue)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters).Tables[0];

            return dtList;
        }

        public static DataTable GetFilterForList(ConfigurationParameterBinder parameter)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CFG_PK,parameter.ConfigPk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ACTIVE,parameter.Active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CFG_TYPE,parameter.ConfigType)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_CONFIG_MST_GET_KV, colParameters).Tables[0];

            return dtList;
        }


    }
}
