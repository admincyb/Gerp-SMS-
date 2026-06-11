using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Finance;

namespace DataAccess.Finance
{
    public class CNDNDL
    {
        public static DataSet GetCNDNDetails(int RecPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(Parameters.CDH_PK, RecPK==0?(object) DBNull.Value:RecPK)
              };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.CNDNGetReport, colParameters);
            return dsList;
        }

        /// <summary>
        /// Delete Attachment Documents
        /// </summary>
        /// <param name="docPk"></param>
        /// <returns>int</returns>
        public static int DeleteAttachmentDocuments(int docPk, int? docTask, int? docTaskId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(Parameters.DOC_PK, docPk)
               , new DBService.Parameters(Parameters.P_DOC_TASK, docTask==null?(object) DBNull.Value:docTask)
               , new DBService.Parameters(Parameters.P_DOC_TASK_ID, docTaskId==null?(object) DBNull.Value:docTaskId)
               , new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
              };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_DOC_ATTACH_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
