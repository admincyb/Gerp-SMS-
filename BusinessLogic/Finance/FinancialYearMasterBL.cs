using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using BusinessObject.POInvoicing;
using GTIService;
using DataAccess.Finance;
using BusinessObject;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.Finance;

namespace BusinessLogic.Finance
{
   public class FinancialYearMasterBL
    {
       public static int? SaveFinancialYear( FinancialYearMasterBO objFinYear,int deptPK, int userPK, int bizunit, string lastmodifieddate=null)
       {
           return DataAccess.Finance.FianancialYearMasterDL.SaveFinancialYear(objFinYear, deptPK, userPK, bizunit, lastmodifieddate);
       }
       public static DataTable GetFinYearDetails( int active, int bizunit)
       {
           return DataAccess.Finance.FianancialYearMasterDL.GetFinYearDetails(active, bizunit);
       }
       public static DataTable GetFinYearEditDetails(int CurrPK, int active, int bizunit)
       {
           return DataAccess.Finance.FianancialYearMasterDL.GetFinYearEditDetails(CurrPK, active, bizunit);
       }
       
       public static int DeleteFinancialYear(int ltmPK, string lastModDate)
       {
           return DataAccess.Finance.FianancialYearMasterDL.DeleteFinancialYear(ltmPK, lastModDate);
       }

    }
}
