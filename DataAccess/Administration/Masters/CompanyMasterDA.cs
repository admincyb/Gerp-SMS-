using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.Administration.Masters;
using BusinessObject.CommonManagement;
using GTIService.Constants.Common;
using GTIService.Constants.Administration.Masters;

namespace DataAccess.Administration.Masters
{
    public class CompanyMasterDA
    {
        public static int? SaveCompanyMaster(CompanyMasterBO companyMasterBo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_PK, companyMasterBo.P_CMP_PK),  
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_CODE,companyMasterBo.P_CMP_CODE),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_NAME,companyMasterBo.P_CMP_NAME),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_DESC, companyMasterBo.P_CMP_DESC), 
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_ADDR1,companyMasterBo.P_CMP_ADDR1),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_ADDR2,companyMasterBo.P_CMP_ADDR2),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_CITY,companyMasterBo.P_CMP_CITY),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_PHONE,companyMasterBo.P_CMP_PHONE),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_MOBIL,companyMasterBo.P_CMP_MOBIL),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_FAX,companyMasterBo.P_CMP_FAX),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_EMAIL,companyMasterBo.P_CMP_EMAIL),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_STATE,companyMasterBo.P_CMP_STATE==0?(object)DBNull.Value:companyMasterBo.P_CMP_STATE),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_CNTRY,companyMasterBo.P_CMP_CNTRY==0?(object)DBNull.Value:companyMasterBo.P_CMP_CNTRY),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_CURRENCY,companyMasterBo.P_CMP_CURRENCY==0?(object)DBNull.Value:companyMasterBo.P_CMP_CURRENCY ),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_TAX_NO,companyMasterBo.P_CMP_TAX_NO),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_LOGO,companyMasterBo.P_CMP_LOGO),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_ACTIVE,companyMasterBo.P_ACTIVE),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_USER_PK,companyMasterBo.P_USER_PK),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_BIZUNIT,companyMasterBo.P_BIZUNIT),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_LAST_MOD_DT,companyMasterBo.P_LAST_MOD_DT),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SP_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Administration.Masters.Parameters.P_RET_VAL]).Value);
            return result;

        }

        public static int DeleteCompanyMaster(int apsPK, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_PK, apsPK),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_LAST_MOD_DT, (object)DBNull.Value), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SP_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get Company List as DataTable
        /// </summary>
        /// <param name="cmpPk"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataSet GetCompanyMaster(int cmpPk, short active, int bizUnit, string SplCond = null)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_PK, cmpPk>0?cmpPk:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_ACTIVE,  active),
                 new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_BIZUNIT,  bizUnit>0?bizUnit:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_SPL_COND,  !string.IsNullOrEmpty(SplCond)?SplCond:(object)DBNull.Value)
            };

            DataSet dsCompanyMaster = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SP_GET_KV, colParameters);
            return dsCompanyMaster;

        }
        /// <summary>
        /// Get Common DDL
        /// </summary>
        /// <param name="groupValue"></param>
        /// <returns></returns>
        public static DataSet GetCommonDDL(int groupValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_GROUP_TYPE_VALUE, 21),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_GROUP_VALUE,  groupValue)
            };

            DataSet dsCommonDDL = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
            return dsCommonDDL;
        }

        public static DataSet GetCountryList()
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            //colParameters = new DBService.Parameters[] 
            //{   
            //    new DBService.Parameters(GTIService.Constants.Common.Parameters.PCOUNTRY,  apsPK==0?(object)DBNull.Value:apsPK),
            //};

            DataSet CountryList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETCOUNTRY, colParameters);
            return CountryList;

        }

        public static DataSet GetCurrency(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_BIZUNIT,  bizUnit),
            };
            DataSet CurrencyList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SP_GETCURRENCY, colParameters);
            return CurrencyList;

        }
        public static DataTable POCategoryDDLGet(int Pk, int Active, int UserPk, int bizUnit, int ProcessID, int RefID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK, Pk),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_BIZUNIT, bizUnit > 0 ? bizUnit : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_USER_PK,  UserPk > 0 ? UserPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_Pprocess,  ProcessID > 0 ? ProcessID : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PReference,  RefID > 0 ? RefID : (object)DBNull.Value),
            };
            DataTable dsCommonDDL = dbService.DataAdapterTable(CommandType.StoredProcedure, "SPADM_CONST_PO_CAT_GET_KV", colParameters);
            return dsCommonDDL;
        }
        public static DataTable GetDepartments(int Pk, int Active, int UserPk, int bizUnit, int ProcessID, int RefID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK, Pk),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_BIZUNIT, bizUnit > 0 ? bizUnit : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_USER_PK,  UserPk > 0 ? UserPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_Pprocess,  ProcessID > 0 ? ProcessID : (object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_PReference,  RefID > 0 ? RefID : (object)DBNull.Value),
            };
            DataTable dsCommonDDL = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPADM_CONST_LOC_GET_KV, colParameters);
            return dsCommonDDL;
        }

        public static DataTable GetSubDepartments(int Pk, int Active, int ParentPk, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK, Pk),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CON_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PARENT,  ParentPk>0 ? ParentPk :(object)DBNull.Value)   ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_BIZUNIT, bizUnit > 0 ? bizUnit : (object)DBNull.Value)                
            };
            DataTable dsCommonDDL = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPADM_CONST_MST_GET_KV, colParameters);
            return dsCommonDDL;
        }
        public static DataTable GetSubDepartmentsMMT(int Pk, int Active, int ParentPk, int bizUnit,int cgt,int cfg)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK, Pk),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CON_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PARENT,  ParentPk>0 ? ParentPk :(object)DBNull.Value)   ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_BIZUNIT, bizUnit > 0 ? bizUnit : (object)DBNull.Value)  ,   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CGT_VALUE, cgt > 0 ? cgt : (object)DBNull.Value)  ,  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CNG_VALUE, cfg > 0 ? cfg : (object)DBNull.Value)  
            };
            DataTable dsCommonDDL = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPADM_CONST_MST_GET_KV, colParameters);
            return dsCommonDDL;
        }
        /// <summary>
        ///  SPADM_CONST_MST_GET_KV
        /// </summary>
        /// <param name="Pk"></param>
        /// <param name="Active"></param>
        /// <param name="Group"></param>
        /// <param name="GroupTypeConst"></param>
        /// <param name="GroupConstant"></param>
        /// <param name="BizUnit"></param>
        /// <returns></returns>
        public static DataTable GetConstantValue(int Pk, int Active, int Group, int GroupTypeConst,int GroupConstant,int BizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK, Pk),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CON_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_GROUP,  Group > 0 ? Group: (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CGT_VALUE,  GroupTypeConst > 0 ? GroupTypeConst: (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CNG_VALUE,  GroupConstant > 0 ? GroupConstant: (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_BIZUNIT, BizUnit > 0 ? BizUnit : (object)DBNull.Value)                
            };
            DataTable dsCommonDDL = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPADM_CONST_MST_GET_KV, colParameters);
            return dsCommonDDL;
        }
        public static DataTable GetInvestmentList(int PK,string SearchValue, int Active,int BizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_WOH_PK,  PK > 0 ? PK: (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, string.IsNullOrEmpty(SearchValue) ? "%": "%"+SearchValue+"%"),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_ACTIVE, Active>0 ? Active: (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, BizUnit > 0 ? BizUnit : (object)DBNull.Value)
            };
            DataTable dsCommonDDL = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPINV_WORK_ORDER_HDR_AUTO, colParameters);
            return dsCommonDDL;
        }
        
        /// <summary>
        ///  [SPADM_CONST_MST_GET_AUTO]
        /// </summary>
        /// <param name="Pk"></param>
        /// <param name="Active"></param>
        /// <param name="Group"></param>
        /// <param name="GroupTypeConst"></param>
        /// <param name="GroupConstant"></param>
        /// <param name="BizUnit"></param>
        /// <returns></returns>
        public static DataTable GetConstantValueAuto(int Pk, int Active, int Group, int GroupTypeConst, int GroupConstant, int BizUnit, string SearchValue="")
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK, Pk),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CON_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_GROUP,  Group > 0 ? Group: (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CGT_VALUE,  GroupTypeConst > 0 ? GroupTypeConst: (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CNG_VALUE,  GroupConstant > 0 ? GroupConstant: (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_BIZUNIT, BizUnit > 0 ? BizUnit : (object)DBNull.Value) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_NAME , SearchValue==string.Empty?(object)DBNull.Value:"%"+SearchValue+"%"),
            };
            DataTable dsCommonDDL = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPADM_CONST_MST_GET_AUTO, colParameters);
            return dsCommonDDL;
        }

        /// <summary>
        /// Get Company List as DataTable
        /// </summary>
        /// <param name="cmpPk"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataSet GetCompanyMappingDetails(int cmpPk, short active, int bizUnit,int deptPk)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_CMP_PK, cmpPk>0?cmpPk:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_ACTIVE,  active),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DEPT_PK, deptPk>0?deptPk:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_BIZUNIT, bizUnit > 0 ? bizUnit : (object)DBNull.Value)     
            };

            DataSet dsCompanyMaster = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPADM_COMPANY_MPG_GET_LIST, colParameters);
            return dsCompanyMaster;

        }
    }
}
