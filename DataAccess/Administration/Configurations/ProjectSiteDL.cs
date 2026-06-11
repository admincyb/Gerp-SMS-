using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Configurations.ProjectSite;

namespace DataAccess.Administration.Configurations
{
    public class ProjectSiteDL
    {

        /// <summary>
        /// Function Used To save Project Site
        /// </summary>
        /// <param name="sbuConfiguartion"></param>
        /// <returns></returns>
        public static string SaveProjectSite(BusinessObject.Administration.Configurations.ProjectSiteBO projectSite)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.PLOCPK, projectSite.LOC_PK == 0 ? (object)DBNull.Value: projectSite.LOC_PK),  
                new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.PLOC_CODE, projectSite.LOC_CODE),  
                new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.PLOC_NAME, projectSite.LOC_NAME),
                new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.PLOC_DESC, projectSite.LOC_DESC),
                new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.ACTIVE, projectSite.LOC_ACTIVE),
                new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.USERPK, projectSite.UserPK),
                new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.BIZUNIT, projectSite.LOC_BIZUNIT),
                //new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.LASTMODDATE, projectSite.LOC_MOD_DT),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.ProjectSite.Procedure.SP_SAV_EPROJECTSITE, colParameters);
            return ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.SBUConfig.Parameters.RETVAL]).Value.ToString();
        }

        /// <summary>
        /// Function Used To get all active sbu details for fill combo based on the user 
        /// if userpk =0 then get all sbu other his sbu
        /// </summary>
        /// <returns></returns>
        public static DataTable GetProjectSiteList(int userPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Configurations.ProjectSite.Parameters.USERPK , userPK == 0 ? (object)DBNull.Value : userPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.ProjectSite.Procedure.SP_GET_PROJECTSITE_LIST, colParameters).Tables[0];
        }

        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <param name="objUser"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetProjectSiteList(GridPrams grid, int bizUnit, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.SEARCHNAME, grid.SearchBy == ((object)DBNull.Value).ToString()? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.SEARCHVAL , grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.SORTBY,   grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.SORTDIR,  grid.SortDirection)
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.ProjectSite.Procedure.SP_GET_PROJECTSITE_LIST, colParameters);
            return dtRequisition;

        }
        /// <summary>
        /// DeleteProject Site Details By MRHPK
        /// </summary>
        /// <param name="MRHPK"></param>
        /// <returns>int= 1(Success)</returns>
        public static string DeleteProjectSite(int ProjectPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Configurations.ProjectSite.Parameters.PLOCPK,  ProjectPK == 0 ? (object)DBNull.Value :  ProjectPK),
                new DBService.Parameters( GTIService.Constants.Configurations.SBUConfig.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.ProjectSite.Procedure.SP_DELETE_PROJECTSITE, colParameters);
            return ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Configurations.SBUConfig.Parameters.RETVAL]).Value.ToString();
        }

        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="sbuPk"></param>
        /// <param name="objUser"></param>
        /// <returns>DataTable</returns>
        public static DataTable ProjectSiteGetSearchValue(string searchBy, string searchValue, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Configurations.ProjectSite.Parameters.FIELDNAME ,  searchBy),
              new DBService.Parameters( GTIService.Constants.Configurations.ProjectSite.Parameters.VALUE ,  searchValue),
              new DBService.Parameters(  GTIService.Constants.Configurations.ProjectSite.Parameters.BIZUNIT , objUser.SBUID)
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.ProjectSite.Procedure.SP_GET_PROJECTSITE_AUTO, colParameters).Tables[0];
            return dtSearchValue;

        }
    }
}
