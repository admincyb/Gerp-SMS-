using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Utilities;
using System.Data;
using BusinessObject.HRMS.eDocs;

namespace DataAccess.HRMS.eDoc
{
    public class EdocManagementDL
    {
        public static int Save(EDocBO eDoc)
        {
            string xmlDoc = CommonFunctions.XmlSerialize(eDoc);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.eDocs.Procedures.SPDMS_DOC_SAVE
                , colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

            return result;
        }

        public static DataTable GetProjectsOrSite(int? pk, int active = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.PprjPK , pk??(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.Pactive , active)   
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.eDocs.Procedures.SpPrjProjectInfGetKV
                , colParameters);
            return dsSet.Tables[0];
        }

        /// <summary>
        /// Only Get Projects Or Sites Used in Any Transactions
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUsedProjectsOrSite()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.PprjPK , (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.Pactive , (object)DBNull.Value)   
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.eDocs.Procedures.SpPrjProjectInfGetKV
                , colParameters);
            return dsSet.Tables[0];
        }

        public static EDocBO GetEDocDetailsByID(long eDocPk, int userPk)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCH_PK,eDocPk),
                new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_USER_PK,userPk)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure,
                GTIService.Constants.HRMS.eDocs.Procedures.SPDMS_DOC_GET, colParameters).Tables[0];

            string objXml = string.Empty;

            foreach (DataRow row in dtList.Rows)
            {
                objXml += row[0].ToString();
            }

            EDocBO eDoc = null;
            if (!objXml.IsNullOrEmptyOrWhitespace())
                eDoc = CommonFunctions.XmlDeserialize<EDocBO>(objXml);

            return eDoc;
        }

        public static DataTable GetSendToUsers(int excludedUsrPk, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_USER_PK , excludedUsrPk),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active)   
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.eDocs.Procedures.SPDMS_DOC_USER_GET_KV
                , colParameters);
            return dsSet.Tables[0];
        }


        public static DataTable GetAutoEdocEmployee(string searchValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_USER_NAME , searchValue),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, 1)   
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.eDocs.Procedures.SPDMS_DOC_USER_GET_KV
                , colParameters);
            return dsSet.Tables[0];
        }

        public static DataTable GetEdocs(EDocSearchParameter parameter)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {       
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO  , parameter.GridParams.PageNumber), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE  , parameter.GridParams.PageSize),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , parameter.GridParams.SearchBy == "0" || parameter.GridParams.SearchBy == "Date" ? (object)DBNull.Value : parameter.GridParams.SearchBy ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , parameter.GridParams.SearchValue== string.Empty || parameter.GridParams.SearchValue=="0" ? "%" : parameter.GridParams.SearchValue+"%"), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , parameter.GridParams.SortBy== string.Empty ? (Object)DBNull.Value : parameter.GridParams.SortBy),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , parameter.GridParams.ThenBy== string.Empty ? (Object)DBNull.Value : parameter.GridParams.ThenBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , parameter.GridParams.SortDirection== string.Empty ? (Object)DBNull.Value : parameter.GridParams.SortDirection),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , parameter.GridParams.ThenDirection== string.Empty ? (Object)DBNull.Value : parameter.GridParams.ThenDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , parameter.GridParams.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(parameter.GridParams.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , parameter.GridParams.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(parameter.GridParams.ToDate)),                  
                
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_PROJECT, parameter.ProjectSite == -1 ? (object)DBNull.Value : parameter.ProjectSite),  
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_TRX_DEPT, parameter.Department == -1 ? (object)DBNull.Value : parameter.Department),  
             
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCH_FROM_USER, parameter.Send == -1 ? (object)DBNull.Value : parameter.Send),  
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCH_TO_USER, parameter.SendTo == -1 ? (object)DBNull.Value : parameter.SendTo),  

              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_LETTER_FROM, parameter.From == -1 ? (object)DBNull.Value : parameter.From),  
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_LETTER_TO, parameter.To == -1 ? (object)DBNull.Value : parameter.To),  

              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_DOC_NO, string.IsNullOrWhiteSpace( parameter.DocNo)?(object)DBNull.Value : string.Format("%{0}%", parameter.DocNo)),  
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_LETTER_NO, string.IsNullOrWhiteSpace( parameter.LetterNo)?(object)DBNull.Value : string.Format("%{0}%", parameter.LetterNo)),              
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_SUBJECT,string.IsNullOrWhiteSpace(  parameter.Subject)?(object)DBNull.Value : string.Format("%{0}%", parameter.Subject)),  
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_TAGS, string.IsNullOrWhiteSpace( parameter.Tags)?(object)DBNull.Value : string.Format("%{0}%", parameter.Tags)),  
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_COMMENT, string.IsNullOrWhiteSpace( parameter.Comments)?(object)DBNull.Value : string.Format("%{0}%", parameter.Comments)),  
             
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_SEARCH_TEXT, string.IsNullOrWhiteSpace( parameter.SearchText)?(object)DBNull.Value : parameter.SearchText),  
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_TRX_STATUS, parameter.Status == 4 | parameter.Status == 3 | parameter.Status == -1 ?(object)DBNull.Value : parameter.Status),  
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_DOC_STATUS, parameter.DocStatus == -1?(object)DBNull.Value : parameter.DocStatus),  
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_FILE_MODE, parameter.FileMode ? 1 : 0),  
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_FILE_TEXT, string.IsNullOrWhiteSpace( parameter.FileText)?(object)DBNull.Value : parameter.FileText),
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_DCT_FOLDER, parameter.Folder > 0 ?parameter.Folder : (object)DBNull.Value),

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, parameter.BizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, parameter.GridParams.UserPK)   
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.eDocs.Procedures.SPDMS_DOC_TRX_GET_LIST
                , colParameters);
            return dsSet.Tables[0];
        }

        public static DataTable GetEdocsSummary(int userPk, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.eDocs.Parameters.P_USER_PK , userPk),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)   
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.eDocs.Procedures.SPDMS_DOC_TRX_SUMMARY_LIST
                , colParameters);
            return dsSet.Tables[0];
        }

    }
}
