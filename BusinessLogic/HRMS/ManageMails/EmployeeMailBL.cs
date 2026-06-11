using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.HRMS.ManageMails;
using BusinessObject;

namespace BusinessLogic.HRMS.ManageMails
{
   public class EmployeeMailBL
    {

        /// <summary>
        /// method for Get Mail  Details
        /// </summary>
        /// <param name="mailPK"></param>
        /// <param name="process"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
       public static DataTable GetEmployeeMailDetails(int? Pk, int? Status)
        {
            DataSet dsMailQue;
            DataTable dtMailQue=new DataTable();
            dsMailQue = DataAccess.Administration.Configurations.ViewMailsDA.GetPaySlipMailList(Pk, Status);
            if (dsMailQue != null && dsMailQue.Tables.Count > 0)
                dtMailQue = dsMailQue.Tables[0];
            return dtMailQue;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="grid"></param>
       /// <param name="partyType"></param>
       /// <param name="partyPK"></param>
       /// <param name="applicationType"></param>
       /// <returns></returns>
       public static DataTable GetEmpMailQList(GridPrams grid, int employeePK, int status)
       {
           DataSet dsMailQue;
           DataTable dtMailQue = new DataTable();
           dsMailQue = EmployeeMailDL.GetEmpMailQList(grid, employeePK, status);
           if (dsMailQue != null && dsMailQue.Tables.Count > 0)
               dtMailQue = dsMailQue.Tables[0];
           return dtMailQue;         
       }
    }
}
