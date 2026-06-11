using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using BusinessObject.Administration.Masters;
using System.Xml;
using System.Xml.Serialization;
using GTIService.Constants.Administration.Masters.GeneralTemplate;
using System.Data;
using System.IO;
namespace DataAccess.Administration.Masters
{
  public  class GeneralTemplateMasterDL
    {

        /// <summary>
        /// Save  General Template - Save  General Template header and Terms Details - Pass as Xml Format
        /// </summary>
        /// <param name="DispersionMaster"></param>
        /// <returns>TemplatePk/ -1 Exception </returns>
        public static int SaveTemplateDetails(string strxml)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( Parameters.TEMPLATEDETAILXML , (object)strxml,DBService.ParameterType.XML),  
                 new DBService.Parameters(Parameters.RETVAL2, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };

            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.TEMPLATEMASTERSAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.RETVAL2]).Value);
            return result;
        }

        /// <summary>
        /// Save Template Group Details
        /// </summary>
        /// <param name="customerName"></param>
        /// <returns>int- TemplateGroupPK</returns>
        public static int SaveTemplateGroup(TemplateGroupMaster templateGroup)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(Parameters.TEMPLATEGROUPPK ,   templateGroup.TemplateGrpPK == 0 ? (object)DBNull.Value :  templateGroup.TemplateGrpPK) ,
                new DBService.Parameters(Parameters.TEMPLATEGROUPNAME ,   templateGroup.TemplateGrpName) ,
                new DBService.Parameters(Parameters.TEMPLATEGROUPACTIVE ,  1) ,
                new DBService.Parameters(Parameters.TEMPLATEGROUPBIZUNIT ,templateGroup.BizUnitPk) ,
                new DBService.Parameters(Parameters.TEMPLATEGROUPCODE,DBNull.Value),
                new DBService.Parameters(Parameters.TEMPLATEGROUPDESCR,DBNull.Value),
                new DBService.Parameters(Parameters.TEMPLATEGROUPMODBY,templateGroup.UserPK),
                new DBService.Parameters(Parameters.TEMPLATEGROUPMODDT,DBNull.Value),
                 new DBService.Parameters(Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure,Procedures.TEMPLATEGROUPSAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.RETVAL]).Value);


        }

        /// <summary>
        /// Get Template Group list for filling combo
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTemplateGroupCombo(int bizUnitPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
            
              new DBService.Parameters(Parameters.TERMSPK, DBNull.Value),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,bizUnitPk)
            };

            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.TEMPLATEGROUPGET, colParameters).Tables[1];
            return dtDispersions;
        }

        /// <summary>
        /// Get Template Group Details For Listing
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="BizUnitPk"></param>
        /// <returns>Dataset - Table[0]- Count, table[1] - Template Group</returns>
        public static DataSet GetTemplateGroupDtls(GridPrams grid, int bizUnitPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "asc" : grid.SortDirection),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS,  grid.Fields),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME ,  grid.SearchBy=="0"?(object)DBNull.Value:grid.SearchBy),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPk),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL,  grid.SearchValue==string.Empty?(object)DBNull.Value:"%"+grid.SearchValue+"%"),
            };
            DataSet dsMachineType = new DataSet();
            dsMachineType = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.TEMPLATEGROUPGETLIST, colParameters);
            return dsMachineType;

        }

        /// <summary>
        /// Delete Template Group Details  
        /// </summary>
        /// <param name="TemplateGrpID"></param>
        /// <returns>int - 1- Success 0-Reference exists -1 -Default Group</returns>
        public static int DeleteTemplateGroupDtls(int templateGrpID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(Parameters.TERMSPK , templateGrpID),
                 new DBService.Parameters(Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.TEMPLATEGROUPDELET, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.RETVAL]).Value);
        }
        /// <summary>
        /// Get general template list for filling
        /// <param name="Grid">Grid parameters</param>
        /// <returns>DataSet</returns>
        public static DataSet GetGeneralTemplateList(GridPrams grid, int bizUnitPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "asc" : grid.SortDirection),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS,  grid.Fields),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME ,  grid.SearchBy=="0"?(object)DBNull.Value:grid.SearchBy),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPk),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL,  grid.SearchValue==string.Empty?(object)DBNull.Value:"%"+grid.SearchValue+"%"),
            };

            DataSet dtDispersions = new DataSet();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.TEMPLATELISTGET, colParameters);
            return dtDispersions;

        }
        /// <summary>
        /// Delete Template  Details By TemplateID
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns>int- 1(Success) 0- Reference Exists -1 -Default Group</returns>
        public static int DeleteGeneralTemplateDtls(int templateID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters( Parameters.TMHPK,  templateID == 0 ? (object)DBNull.Value :  templateID),
                new DBService.Parameters(Parameters.RETVAL, string.Empty,4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.TEMPLATEDELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.RETVAL]).Value);
        }

        /// <summary>
        /// Get General Template Details By Template Id as A Xml Format
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns>Xml Formatted Template Details</returns>
        public static string GetGeneralTemplateDetails(int templateID)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( Parameters.TMHPK ,  templateID),  

                 new DBService.Parameters(Parameters.RETVAL, string.Empty,4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };

             return dbService.ExecuteScalar(CommandType.StoredProcedure, Procedures.TEMPLATEDETAILGET, colParameters).ToString();
           // return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters["P_RET_VAL"]).Value);
        }

        /// <summary>
        /// Method to get the Search Vlaues Corresponding to Seatch Type
        /// </summary>
        /// <returns>Datatable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int bizUnitPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
            
               new DBService.Parameters(Parameters.FLDNAME,searchBy),
               new DBService.Parameters(Parameters.PVALUE, searchValue),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPk),

            };

            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.TEMPLATEAUTO, colParameters).Tables[0];
            return dtDispersions;
        }

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="BizUnitPk"></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <For>Po Generation</For>
        /// <usedin>Po Creation Listing Vendor Terms</usedin>
        /// <returns></returns>
        public static DataTable GetGeneralTemplates(int bizUnitPk, int termspk, string term = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,bizUnitPk),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.GeneralTemplate.Parameters.TMDPK , termspk==0?(object)DBNull.Value:termspk),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.GeneralTemplate.Parameters.TMH_GROUP , term)
            };
            DataTable dtTerms = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.GeneralTemplate.Procedures.GETTEMPLATES, colParameters).Tables[0];
            return dtTerms;

        }
        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="BizUnitPk"></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <For>Po Generation</For>
        /// <usedin>Po Creation Listing Vendor Terms</usedin>
        /// <returns></returns>
        public static DataTable GetPOTemplate(int bizUnitPk, int termspk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,bizUnitPk),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.GeneralTemplate.Parameters.TMDPK , termspk==0?(object)DBNull.Value:termspk),
                 new DBService.Parameters(GTIService.Constants.Administration.Masters.GeneralTemplate.Parameters.TMH_GROUP , GTIService.Constants.Administration.Masters.GeneralTemplate.Parameters.POTERMS)
            };
            DataTable dtTerms = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.GeneralTemplate.Procedures.GETTEMPLATES, colParameters).Tables[0];
            return dtTerms;

        }
    }
}


