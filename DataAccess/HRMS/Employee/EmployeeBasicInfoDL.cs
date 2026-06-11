using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities.HRMS;
using BusinessObject.HRMS.Employee;
using ERP.Utilities;


namespace DataAccess.HRMS.Employee
{
    public class EmployeeBasicInfoDL
    {

        // Save Employee Basic information
        public static int SaveEmployeeBasicInformation(string strxml, out string EmpCode, out DataTable dtErrors)
        {
            //strxml = strxml.HtmlDecode();
            // EmployeeBasicInfomtn obj = CommonFunctions.XmlDeserialize<EmployeeBasicInfomtn>(strxml);
            dtErrors = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SAVE_EMPLOYEEBASICINFO, colParameters);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtErrors = dsResult.Tables[0];

            //int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SAVE_EMPLOYEEBASICINFO, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            EmpCode = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }


        //Auto Complete Nationality

        public static DataTable GetAutoNationality(string SearchKey,string OpParam)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters( ERP.Utilities.HRMS.Employee.CNT_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CNT_NATIONALITY , SearchKey +"%"),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_SORT_BY ,  String.IsNullOrEmpty(OpParam)?null:OpParam),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_AUTOCOMPLETENATINALITY, colParameters);
        }
        //Auto Complete Country
        public static DataTable GetAutoCountry(string SearchKey)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters( ERP.Utilities.HRMS.Employee.CNT_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.CNT_NAME , SearchKey +"%"),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_AUTOCOMPLETENATINALITY, colParameters);
        }

        //Auto Complete Country
        public static DataTable GetTransactionNo(string searchKey)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters( ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters( ERP.Utilities.HRMS.Employee.P_BIZUNIT, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_SEARCH_VAL , searchKey +"%")
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_ATTENDANCE_AUTO, colParameters);
        }

        //Auto Complete State
        public static DataTable GetAutoState(string SearchKey,int countryPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters( ERP.Utilities.HRMS.Employee.STT_COUNTRY, countryPk>0?countryPk:(object)DBNull.Value) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_VALUE , SearchKey +"%"),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPADM_STATE_MST_AUTO, colParameters);
        }


        //Auto Complete Profession
        public static DataTable GetAutoProfession(int conPK, string SearchKey)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 10) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_TEXT_VALUE , SearchKey +"%"),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        //Auto Complete Religion
        public static DataTable GetAutoReligion(int conPK, string SearchKey)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 17) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_TEXT_VALUE , SearchKey +"%"),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        //Auto Complete Team
        public static DataTable GetAutoTeam(string SearchKey)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, (object)DBNull.Value) ,
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 28) ,
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_TEXT_VALUE , SearchKey +"%"),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }


        public static DataTable GetDropDownReligion(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 17) ,  
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }
        //Auto Complete Sub Religion
        public static DataTable GetAutoSubReligion(int ConPK, string SearchKey)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, ConPK) ,  
                //new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PARENT, conParentPK) , 
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 18) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_TEXT_VALUE , SearchKey +"%"),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        public static DataTable GetDropDownSubReligion(int ConPK, int conParentPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, ConPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PARENT, conParentPK) , 
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 18) ,  
               
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        //Auto Complete Department
        public static DataTable GetAutoDepartment(int conPK, string SearchKey)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 9) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_TEXT_VALUE , SearchKey +"%"),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        //Auto Complete DepartmentAuto
        public static DataTable GetFillDepartmentAutocomplete(int conPK, string SearchKey)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 9) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_TEXT_VALUE , SearchKey +"%"),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_DEPT_GET_KV, colParameters);
        }

        //Auto Complete Department
        public static DataTable GetAutoFillBranchLocation(int conPK, string SearchKey, int? UserPk = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 7) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_TEXT_VALUE , SearchKey +"%"),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, UserPk.HasValue ? UserPk : (object)DBNull.Value) 
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }



        //Auto Complete EmployeeList

        public static DataTable GetAutoEmployeeList(string SearchKey)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_empName , SearchKey +"%"),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_AUTOCOMPLETEMPLOYEE, colParameters);
        }

        //Auto Complete EmployeeList

        public static DataTable GetEmployeeAuto(string SearchKey, string empBranchLoc, string empType)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_empPK , 0),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_empName , SearchKey +"%"),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EMP_BRANCH , empBranchLoc == CommonConstants.SELECTVAL ? (object)DBNull.Value : empBranchLoc),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EMP_TYPE ,  empType == CommonConstants.SELECTVAL ? (object)DBNull.Value : empType),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EmployeeHeaderList, colParameters);
        }

        //Fill Marital Staus
        public static DataTable GetMaritalStatus(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 3) , 
                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        //Fill Salutation
        public static DataTable GetSalutation(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 2) , 
                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        //Fill Job Level
        public static DataTable GetJobLevel(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 14) ,  

                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }
        //Fill Job Category
        public static DataTable GetJobCategory(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 15) ,  

                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        //Fill Skill Level
        public static DataTable GetSkillLevel(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 1) ,  

                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }


        //Fill jobStream
        public static DataTable GetjobStream(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 16) ,  

                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }


        //Fill Employee Gender
        public static DataTable GetEmployeeGender(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 6) ,  

                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        //Fill EmploymentType
        public static DataTable GetEmploymentType(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 8) ,  

                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        //Fill BranchLocationType
        public static DataTable GetBranchLocationType(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 7) ,  

                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        //Fill EmploymentType
        public static DataTable GetBloodGroup(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 4) ,  

                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        //Fill Employee Designation
        public static DataTable GetEmployeeDesignation(int desPK, string searchKey = "")
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_dsgPK, desPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_dsgName , String.IsNullOrEmpty(searchKey)?string.Empty:searchKey),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EMPLOYEE_DESIGNATION, colParameters);
        }

        //Fill Employee Designation By Job Category and Job level
        public static DataTable GetEmployeeDesignationByJob(string searchKey = "", int? JobCategory = null, int? JobLevel = null)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                //new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_dsgPK, desPK) ,  ,(empBranch.HasValue && empBranch > 0) ?  empBranch : (object)DBNull.Value)
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_dsgJobCategory ,(JobCategory.HasValue && JobCategory > 0) ?  JobCategory : (object)DBNull.Value),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_dsgJobLevel,(JobLevel.HasValue && JobLevel > 0) ?  JobLevel : (object)DBNull.Value),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_dsgName , String.IsNullOrEmpty(searchKey)?string.Empty:searchKey),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EMPLOYEE_DESIGNATIONBYJOB, colParameters);
        }

        //Fill Employee Detail List
        public static DataTable GetEmployeeDetailList()
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_empActive, 1) , 
     
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EmployeeList, colParameters);
        }

        //Fill Employee Detail Header
        public static DataTable GetEmployeeDetailListHeader(int empPk, string sortBy, int active = 1, int ispayroll = 0)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_empPK, empPk) ,
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE_HEADER, active) , 
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_SORT_BY, sortBy==string.Empty?(object)DBNull.Value:sortBy),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_Is_Payroll, ispayroll==0?(object)DBNull.Value:ispayroll)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EmployeeHeaderList, colParameters);
        }

        //Fill Employee Training Details by ID
        public static DataTable GetEmployeeTrainingByID(int empPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EMP_PK, empPk) ,
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_TRAINING_EMP_GET, colParameters);
        }

        public static string GetEmployeebyID(int empPk, int? UserPk = null)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_empPK, empPk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_USER_PK,UserPk.HasValue? UserPk : (object)DBNull.Value)             
            };

            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EmployeeBYID, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int DeleteEmployee(int employeePK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_empPK, employeePK),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.DELETE_EMPLOYEE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ERP.Utilities.HRMS.Employee.P_RET_VAL]).Value);
        }

        //Fill Employee Status CFG
        public static DataTable GetEmployeeStatus(string configType, int configPK = 0, int active = 1, int bizUnit = 1)
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


        //Filter options in Employee List

        public static DataTable GetEmpListByFilterOptions(EmpDocumentFilterParameterBinder filterOptions)
        {
            List<EmployeeBasicInfomtn> EmpList = null;

            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            //Change ParamName based on SP
            colParameters = new DBService.Parameters[] 
            {       


                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SearchempCode,
                                            filterOptions.EmployeeCode.IsNullOrEmptyOrWhitespace()?(object)DBNull.Value:filterOptions.EmployeeCode,
                                            DBService.ParameterType.NVarChar),

                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SearchempName,
                                            filterOptions.EmployeeName.IsNullOrEmptyOrWhitespace()?(object)DBNull.Value:filterOptions.EmployeeName,
                                            DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_empPassportNo,
                                            filterOptions.PassportNO.IsNullOrEmptyOrWhitespace()?(object)DBNull.Value:filterOptions.PassportNO,
                                            DBService.ParameterType.NVarChar),

                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_empPermitNo,
                                            filterOptions.PermitNO.IsNullOrEmptyOrWhitespace()?(object)DBNull.Value:filterOptions.PermitNO,
                                            DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SearchempNationality,filterOptions.Nationality,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SearchempCompany,filterOptions.Company,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SearchempDesignation,filterOptions.Designation,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SearchempDepartment,filterOptions.Department,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_EMP_TYPE,filterOptions.EmployeeType,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SearchempCurStatus,filterOptions.CurrentStatus < 0? (object)DBNull.Value : filterOptions.CurrentStatus,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPCATEGORY,filterOptions.EmpCategory.HasValue? filterOptions.EmpCategory : (object)DBNull.Value,DBService.ParameterType.Int32),  
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_SearchempActive,filterOptions.Active,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT,filterOptions.BizUnit,DBService.ParameterType.Int32),                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,filterOptions.PageIndex,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,filterOptions.PageSize,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_empBranch,filterOptions.BranchLoc,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPCOSTCENTERID,filterOptions.CostCenter.HasValue? filterOptions.CostCenter : (object)DBNull.Value,DBService.ParameterType.Int32),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPTEAMID,filterOptions.Team.HasValue? filterOptions.Team : (object)DBNull.Value,DBService.ParameterType.Int32),
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EmployeeList, colParameters).Tables[0];

            //docList=dtList.ToList<EmployeeDocumentDetails>();
            return dtList;
        }

        //Save JobDetailsStatus
        public static int SaveJobDetailStatus(int pk, int employeeId, DateTime date, Int16 status, Int16 fromStatus, string reason, int userPk, int bizUnit,int cancelStatus)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMS_PK,pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMS_EMPLOYEE,employeeId),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMS_DATE,date),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMS_STATUS,status),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMS_FROM_STATUS,fromStatus),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMS_REASON,reason),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_USER_PK,userPk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT,bizUnit),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMS_CANCEL_STATUS,cancelStatus>0?cancelStatus:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_STATUS_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
       
        //Save Job Details Department
        public static int SaveJobDetailDepartment(int pk, int employeeId, DateTime date, Int16 fromDept, Int16 toDept, string reason, int userPk, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDL_PK,pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDL_EMPLOYEE,employeeId),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDL_DATE,date),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDL_FROM_DEPT,fromDept),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDL_TO_DEPT,toDept),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDL_REASON,reason),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_USER_PK,userPk),
               // new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT,bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_DEPT_LOG_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        //Save Job Details Designation
        public static int SaveJobDetailDesignation(int pk, int employeeId, DateTime date, Int16 fromDesig, Int16 toDesig, string reason, int userPk, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_DSL_PK,pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_DSL_EMPLOYEE,employeeId),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_DSL_DATE,date),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_DSL_FROM_DESIG,fromDesig),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_DSL_TO_DESIG,toDesig),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_DSL_REASON,reason),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_USER_PK,userPk),
               // new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT,bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_DESIG_LOG_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        //Fill Employee Status Details
        public static DataTable GetEmployeeStatusDetails(int pk, int bizUnit, int active)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMS_PK,pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ACTIVE,active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT,bizUnit)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_STATUS_GET_KV, colParameters).Tables[0];

            return dtList;
        }

        //Get Employee Status History
        public static DataTable GetEmployeeStatusHistory(int pk)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMS_EMP_PK,pk)
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_STATUS_GET, colParameters).Tables[0];
            return dtList;
        }

        //Get Employee Department History
        public static DataTable GetEmployeeDepartmentHistory(int pk)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EDL_EMPLOYEE,pk)
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_DEPT_LOG_GET, colParameters).Tables[0];
            return dtList;
        }

        //Get Employee Designation History
        public static DataTable GetEmployeeDesignationHistory(int pk)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_DSL_EMPLOYEE,pk)
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_DESIG_LOG_GET, colParameters).Tables[0];
            return dtList;
        }

        //Save JobDetails Branch
        public static int SaveJobDetailBranch(int pk, int employeeId, DateTime date, int branch,int FromBranch, string reason, int userPk, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMB_PK,pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMB_EMPLOYEE,employeeId),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMB_DATE,date),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMB_BRANCH,branch),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMB_FROM_BRANCH,FromBranch),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMB_REASON,reason),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_USER_PK,userPk),
                //new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT,bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_BRANCH_LOG_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        //Get Employee Status History
        public static DataTable GetJobDetailsBranchHistory(int pk)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMB_EMP_PK,pk)
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_BRANCH_GET, colParameters).Tables[0];
            return dtList;
        }

        public static DataTable GetMaritalStatusConst(int conPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 2) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 3) , 
                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        public static DataTable GetEmploymentTypeHistory(int CurrPK)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ETY_EMP_PK, CurrPK)
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_EMPLOYEMENT_TYPE_LOG_GET, colParameters).Tables[0];
            return dtList;
        }

        public static int? SaveEmploymentTypeHistory(FilterParameters objParametrs, int FromEmploymentType, string Remarks,int IsContract, int MailBeforeDays)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ETY_PK, objParametrs.PK),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ETY_EMPLOYEE, objParametrs.Employee),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ETY_DATE, objParametrs.Date),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ETY_FROM_EMP_TYPE, FromEmploymentType),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ETY_EMP_TYPE, objParametrs.EmploymentType),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ETY_REASON, Remarks),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_USER_PK, objParametrs.UserPK),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EXPIRED_ON, objParametrs.ToDate.HasValue? objParametrs.ToDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_MAIL_BEFORE, MailBeforeDays),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_IS_CONTRACT, IsContract),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_EMPLOYEMENT_TYPE_LOG_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetEmpDesignationDtl(int DesigPk, int Status)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_dsgPK, DesigPk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_Active, Status)
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EMPLOYEE_DESIGNATIONBYJOB, colParameters).Tables[0];
            return dtList;
        }
    }
}
