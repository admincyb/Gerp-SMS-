using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Admin.Masters;
using BusinessObject.HRMS.Admin.Masters;

namespace DataAccess.HRMS.Admin.Masters
{
    public class PayElementsMasterDL
    {
        /// <summary>
        /// for get parent elemetns
        /// </summary>
        /// <param name="CurrPK">Pk</param>
        /// <param name="status">status</param>
        /// <param name="bizUnit">biz unit</param>
        /// <returns></returns>
        public static DataTable GetParentElement(int CurrPK, int status, int bizUnit, int excludePK, int isLoanAdv = 0, int pelClass = -1, int? isDeduct = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_PEL_PK, CurrPK) ,
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) , 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_PARENT_PEL_PK, excludePK == 0 ? (object)DBNull.Value : excludePK) ,
                new DBService.Parameters(Parameters.P_IS_LOAN_ADV, isLoanAdv == 0 ? (object)DBNull.Value : isLoanAdv),
                new DBService.Parameters(Parameters.P_PEL_CLASS, pelClass == -1 ? (object)DBNull.Value : pelClass),
                new DBService.Parameters(Parameters.P_PEL_IS_DEDUCTION, isDeduct.HasValue? isDeduct : (object)DBNull.Value)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_GET_KV, colParameters);
        }

        public static DataTable GetEarnDeductPayElements(int payElementPk, int status, int bizUnit, int isDeduct)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                 new DBService.Parameters(Parameters.P_PEL_PK, payElementPk) , 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) , 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),   
                new DBService.Parameters(Parameters.P_PEL_IS_DEDUCTION, isDeduct)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_GET_KV, colParameters);
        }
        /// <summary>
        /// save Pay Elements
        /// </summary>
        /// <param name="objPayElements"></param>
        /// <returns></returns>
        public static int? SavePayElements(PayElementsMasterBO objPayElements)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                 
                new DBService.Parameters(Parameters.P_PEL_PK, objPayElements.PayElmntPk),
                new DBService.Parameters(Parameters.P_PEL_CODE, objPayElements.PayElmntCode),
                new DBService.Parameters(Parameters.P_PEL_NAME, objPayElements.PayElmntName),
                new DBService.Parameters(Parameters.P_PEL_CODE_LL, objPayElements.P_PEL_CODE_LL),
                new DBService.Parameters(Parameters.P_PEL_NAME_LL, objPayElements.P_PEL_NAME_LL),
                new DBService.Parameters(Parameters.P_PEL_CLASS, string.IsNullOrEmpty(objPayElements.PayElmntClass)? (object)DBNull.Value : objPayElements.PayElmntClass),
                new DBService.Parameters(Parameters.P_PEL_EFFECT_FROM, string.IsNullOrEmpty(objPayElements.EffectiveFrom) ?(object)DBNull.Value:objPayElements.EffectiveFrom),
                new DBService.Parameters(Parameters.P_PEL_EFFECT_TO, string.IsNullOrEmpty(objPayElements.EffectiveTo) ?(object)DBNull.Value:objPayElements.EffectiveTo),
                new DBService.Parameters(Parameters.P_PEL_PARENT, string.IsNullOrEmpty(objPayElements.Parent) ? (object)DBNull.Value : objPayElements.Parent),
                new DBService.Parameters(Parameters.P_PEL_IN_PAY_SLIP, objPayElements.PaySlip), 
                new DBService.Parameters(Parameters.P_PEL_RECURRING, objPayElements.Recurring),
                new DBService.Parameters(Parameters.P_PEL_IN_CTC, objPayElements.CTC),
                new DBService.Parameters(Parameters.P_PEL_IS_EDITABLE, objPayElements.IsEditable),
                new DBService.Parameters(Parameters.P_PEL_IN_GROSS, objPayElements.PartofGross),
                new DBService.Parameters(Parameters.P_PEL_TAXABLE, objPayElements.isTaxable), 
                new DBService.Parameters(Parameters.P_PEL_ACCOUNT, string.IsNullOrEmpty(objPayElements.AccountCode) ? (object)DBNull.Value : objPayElements.AccountCode),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, objPayElements.Active), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_USERPK, objPayElements.UserPk),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, objPayElements.BizUnit),
                new DBService.Parameters(Parameters.P_PEL_FORMULA_CODE, string.IsNullOrEmpty(objPayElements.FormulaCode) ? (object)DBNull.Value :objPayElements.FormulaCode),
                new DBService.Parameters(Parameters.P_PEL_CALC_MODE, string.IsNullOrEmpty(objPayElements.PayType) ? (object)DBNull.Value :objPayElements.PayType),
                new DBService.Parameters(Parameters.P_PEL_IN_SALARY, objPayElements.IncludeInSalary), 
                new DBService.Parameters(Parameters.P_PAY_EL_LASTMOD_DATE, objPayElements.LastModDate),
                new DBService.Parameters(Parameters.P_PEL_DESC,objPayElements.PayElmntDesc),                
                new DBService.Parameters(Parameters.PEL_IS_FORMULA_EDIT, objPayElements.FormulaEditable),
                new DBService.Parameters(Parameters.P_PEL_ROUND_OFF, objPayElements.RoundoffRequired),
                new DBService.Parameters(Parameters.P_PEL_SEQUENCE, objPayElements.Sequence),
                new DBService.Parameters(Parameters.P_PEL_SHOW_IN_REPORT, objPayElements.ShowReport),
                new DBService.Parameters(Parameters.P_PEL_SHOW_IN_REPORT1, objPayElements.ShowReport1),
                new DBService.Parameters(Parameters.P_PEL_SHOW_IN_EMPMAST, objPayElements.ShowInEmp),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_MST_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        
        /// <summary>
        /// For get  parent elemetns lsit
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bsu"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="classification"></param>
        /// <param name="status">1:Active records, 0:Inactive records, null:All records</param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public static DataTable GetParentElementList(BusinessObject.GridPrams gridParam, int bsu, string code, string name, string classification, int? status, string sortOrder = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, gridParam.PageNumber), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, gridParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bsu),
                new DBService.Parameters(Parameters.P_PEL_CODE, code ==  "Select/Type" ? "" : code),
                new DBService.Parameters(Parameters.P_PEL_NAME, name ==  "Select/Type" ? "" : name),
                new DBService.Parameters(Parameters.P_PEL_CLASS, classification ==  "Select/Type" ? "" : classification),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status.HasValue? status : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_SORT_BY, sortOrder!=null ? sortOrder!=string.Empty?sortOrder:(object)DBNull.Value:(object)DBNull.Value),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_GET_LIST, colParameters);
        }
        /// <summary>
        /// Delete Pay Elements
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int? DeletePayElement(int CurrPK, DateTime dateTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_PEL_PK, CurrPK == 0 ? (object)DBNull.Value : CurrPK) ,
                new DBService.Parameters(Parameters.P_LAST_MOD_DT, dateTime) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetPayElementSearchList(int bizUnit, string filterby, string searchValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {               
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, 1) , 
              new DBService.Parameters(Parameters.P_FLD_NAME, filterby == string.Empty || filterby == "" ? (object)DBNull.Value : filterby ),
              new DBService.Parameters(Parameters.P_VALUE, searchValue == string.Empty ? "%" : searchValue+"%")
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_ATUO, colParameters);
        }

        public static int? UpdatePayElementStatus(int PEL_PK, int Status, int UserPk, DateTime? LastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_PEL_PK, PEL_PK),
                new DBService.Parameters(Parameters.P_ACTIVE, Status),
                new DBService.Parameters(Parameters.P_USER_PK, UserPk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT, LastModDate.HasValue? LastModDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_MST_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetPayElementValueSingleList(int PayElementPk, int Status,int CurrPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {              
              new DBService.Parameters(Parameters.P_PEL_PK, PayElementPk ),
              new DBService.Parameters(Parameters.P_VAL_PK, CurrPK) ,
              new DBService.Parameters(Parameters.P_ACTIVE, Status)

            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_PAY_ELEMENT_VALUE_PK_GET, colParameters);
        }

        public static DataTable GetFormulaElement(int? CurrPK, int Status, int bizUnit, int isDeduction)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {              
              new DBService.Parameters(Parameters.P_PEL_PK, CurrPK.HasValue? CurrPK : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit) ,
              new DBService.Parameters(Parameters.P_ACTIVE, Status),
              new DBService.Parameters(Parameters.P_IS_DEDUCTION, isDeduction < 0? 0 : isDeduction)

            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_FORMULA_ELEMENT_GET_KV, colParameters);
        }

        
    }
}
