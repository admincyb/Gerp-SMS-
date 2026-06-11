using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.UserControls;
using ERP.Utilities;

namespace DataAccess.UserControl
{
    public class FolderExplorerDL
    {
        public static string GetFolderTreeXml(int folderPk, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_PK,folderPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,bizUnit)
            };

            string objXml = string.Empty;

            objXml = dbService.ExecuteScalar(CommandType.StoredProcedure,
                                GTIService.Constants.HRMS.eDocs.Procedures.SPADM_FOLDER_MST_GET_TREE,
                                colParameters)
                                .ToString();

            return objXml;
        }

        public static int Save(FolderBO folderBo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_PK,folderBo.FolderPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_NAME,folderBo.FolderName),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_PARENT,folderBo.ParentPk < 1? (object)DBNull.Value : folderBo.ParentPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_LEVEL,folderBo.Level == 0? (object)DBNull.Value : folderBo.Level),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_SEQUENCE,folderBo.Sequence == 0? (object)DBNull.Value : folderBo.Sequence),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_MODULE,folderBo.Module),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,folderBo.Active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,folderBo.UserPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,folderBo.BizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT,folderBo.LastModDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure
                , GTIService.Constants.Common.Procedures.SPADM_FOLDER_MST_SAVE
                , colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

            return result;
        }

        public static int Delete(long pk, DateTime lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FLD_PK, pk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT , lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPADM_FOLDER_MST_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.P_RET_VAL]).Value);
            return result;
        }

        public static DataTable GetMappingUsersList(long folderPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FUM_FOLDER , folderPk)
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(
                CommandType.StoredProcedure
                , GTIService.Constants.HRMS.eDocs.Procedures.SPADM_FOLDER_USER_MAP_GET
                , colParameters
                );
            return dsSet.Tables[0];
        }

        public static int SaveUserMapping(BusinessObject.HRMS.eDocs.FolderMappingBo folderMappingBo)
        {
            string xmlDoc = CommonFunctions.XmlSerialize(folderMappingBo);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure
                                        , GTIService.Constants.HRMS.eDocs.Procedures.SPADM_FOLDER_USER_MAP_SAVE
                                        , colParameters);

            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

            return result;
        }
    }
}
