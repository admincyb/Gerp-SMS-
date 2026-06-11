using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace BusinessLogic.ReportsManagement
{
    public class CashFlowBL
    {
        public static DataSet GetCashFlowTables(string Date)
        {
            return DataAccess.ReportsManagement.CashFlowDL.GetCashFlowTables(Date);
        }

        public static DataSet GetReportDT(string pXML)
        {
            return DataAccess.ReportsManagement.CashFlowDL.GetReportDT(pXML);
        }
    }
}
