using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.DashboardManagement
{
    public class DashBoardDataService
    {
        public  DataTable ExecuteSP(string spname, string param)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("P_XML", param)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, spname, colParameters).Tables[0];
        }

        public DataTable ExecuteSPCustomer(string spname, int UserPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("P_USR_PK", UserPk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, spname, colParameters).Tables[0];
        }
    }
}