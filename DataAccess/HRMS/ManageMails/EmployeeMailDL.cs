using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities.Constants.DA.Administration;
using BusinessObject;

namespace DataAccess.HRMS.ManageMails
{
  public class EmployeeMailDL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="partyType"></param>
        /// <param name="partyPK"></param>
        /// <param name="applicationType"></param>
        /// <returns></returns>

      public static DataSet GetEmpMailQList(GridPrams grid, int employeePk, int status)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                  new DBService.Parameters(GTIService.Constants.HRMS.ManageMails.Parameters.P_PAGE_NUM, grid.PageNumber == 0 ?(Object)DBNull.Value : grid.PageNumber),
                  new DBService.Parameters(GTIService.Constants.HRMS.ManageMails.Parameters.P_PAGE_SIZE, grid.PageSize  == 0 ?(Object)DBNull.Value : grid.PageSize),
                  new DBService.Parameters(GTIService.Constants.HRMS.ManageMails.Parameters.P_EPM_FROM_DATE,grid.FromDate== string.Empty ?(Object)DBNull.Value : grid.FromDate),                
                  new DBService.Parameters(GTIService.Constants.HRMS.ManageMails.Parameters.P_EPM_TO_DATE,grid.ToDate== string.Empty ?(Object)DBNull.Value : grid.ToDate),
                  new DBService.Parameters(GTIService.Constants.HRMS.ManageMails.Parameters.P_empPK, employeePk > 0 ? employeePk : (object)DBNull.Value),                
                  new DBService.Parameters(GTIService.Constants.HRMS.ManageMails.Parameters.P_EPM_IS_GENERATED, status> -1 ? status :(Object)DBNull.Value ),               

                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.ManageMails.Procedures.SPHRM_EMP_PAYROLL_MAIL_DTL_GET_LIST, colParameters);
            }
            return ds;

        }
    }
}
