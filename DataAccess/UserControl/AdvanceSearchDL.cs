using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using System.Data;

namespace DataAccess.UserControl
{
    public class AdvanceSearchDL
    {
        /// <summary>
        /// Function used to save advance search details
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static string SaveAdvSearchDetails(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.UserControl.AdvanceSearch.Parameters.ADVANCESEARCHXML , strxml),  
                new DBService.Parameters( GTIService.UserControl.AdvanceSearch.Parameters.ADVANCESEARCHRETVAL, 0, 20, ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.UserControl.AdvanceSearch.Procedures.SPSAVESEARCHADVANCE , colParameters);
            return (((IDataParameter)dbService.oCommand.Parameters[GTIService.UserControl.AdvanceSearch.Parameters.ADVANCESEARCHRETVAL]).Value).ToString();
        }

        /// <summary>
        /// Function Used to Get Advance Search List
        /// </summary>
        /// <param name="gridPrams"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static DataSet GetAdvanceSearchList(GridPrams gridPrams, int user,string pageTitle)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters.USERPK , user),
              new DBService.Parameters(GTIService.UserControl.AdvanceSearch.Parameters.PAGETITLE , pageTitle),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.UserControl.AdvanceSearch.Procedures.SPGETSEARCHADVANCE, colParameters);
        }

        /// <summary>
        /// Function Used to get advance search details by search id
        /// </summary>
        /// <param name="srchPK"></param>
        /// <returns></returns>
        public static string GetAdvanceSearchDetails(int srchPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.UserControl.AdvanceSearch.Parameters.SEARCHPK ,  srchPK),  
                new DBService.Parameters(GTIService.UserControl.AdvanceSearch.Parameters.ADVANCESEARCHRETVAL, string.Empty, 4000, ParameterDirection.Output,          DBService.ParameterType.NVarChar)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.UserControl.AdvanceSearch.Procedures.SPGETSEARCHADVANCEDETAILS , colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.UserControl.AdvanceSearch.Parameters.ADVANCESEARCHRETVAL]).Value);
        }
    }
}
