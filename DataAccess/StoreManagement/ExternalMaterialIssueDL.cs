using System;
using System.Data;
using BusinessObject;
using System.Collections.Generic;
using GTIService.Constants.Configurations.Config;

namespace DataAccess.StoreManagement
{
   public class ExternalMaterialIssueDL
    {
        /// <summary>
        /// Get Parent Issuing Type List .
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="parentDepartment"></param>
        /// <returns></returns>
       public static DataTable GetIssuingTypeList(int bizUnit, int issuingType, int despatched, string searchValue,int? DeptType)
        {
            //AssetDBService dbService = new AssetDBService();
            //AssetDBService.Parameters[] colParameters = null;
            //colParameters = new AssetDBService.Parameters[] 
            //{        
            //    new AssetDBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
            //    new AssetDBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, 1),
            //    new AssetDBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.ISSUINGTYPE, issuingType),
                
            //};
            //return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETIISUINGYOLIST, colParameters).Tables[0];
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, 1),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.DESPATCHED, despatched == 0 ? (object)DBNull.Value :  despatched),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.ISSUINGTYPE, issuingType),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_SEARCHVAL, searchValue),
                 new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_DPT_TYPE, DeptType==0?(Object)DBNull.Value:DeptType),
          
                
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETIISUINGYOLIST, colParameters).Tables[0];
        }

        public static DataTable GetDamageStore(int DeptPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters.DEPTPK, DeptPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPADM_DEPT_DMG_GET, colParameters).Tables[0];
        }

        public static DataTable GetAssetFormer(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters.BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.SPINV_ITEM_CATEGORY_ASSET_GET, colParameters).Tables[0];
        }

        //SaveConsumptionDetails
        /// <summary>
        /// SAVING REQUISITION DETAILS
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns> INT</returns>
        public static List<object> SaveExternalMaterialIssue(string strxml)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MATERIALISSUEXML, strxml),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MISNO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MATERIALISSUERETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.SAVEISSUEXML, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MATERIALISSUERETURNVALUE]).Value);
            string misNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MISNO]).Value.ToString();
            retvals.Add(result);
            retvals.Add(misNo);
            return retvals;

        }
       /// <summary>
        /// Save External Material Issue/Receipt with Wkf
       /// </summary>
       /// <param name="strxml"></param>
       /// <returns></returns>
        public static List<object> SaveExternalMaterialIssueWkf(string strxml)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MATERIALISSUEXML, strxml),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MISNO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MATERIALISSUERETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.SAVEISSUEWKFXML, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MATERIALISSUERETURNVALUE]).Value);
            int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_RET_REF_PK]).Value);
            string misNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MISNO]).Value.ToString();
            retvals.Add(result);
            retvals.Add(misNo);
            retvals.Add(refPK);
            return retvals;

        }

        public static List<object> SaveEMIDamageWkf(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MATERIALISSUEXML, strxml),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MISNO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_RET_VCH_NO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MATERIALISSUERETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            List<object> retvals = new List<object>();
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.SAVEEMIDAMAGE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MATERIALISSUERETURNVALUE]).Value);
            int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_RET_REF_PK]).Value);
            string misNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_ExternalMaterialIssue.MISNO]).Value.ToString();
            string voucherNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_RET_VCH_NO]).Value.ToString();
            retvals.Add(result);
            retvals.Add(misNo);
            retvals.Add(refPK);
            retvals.Add(voucherNo);
            return retvals;
        }

        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="sbuPk"></param>
        /// <param name="objUser"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int sbuPk, User objUser, int procID,int transactionType)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.REQUISITIONSRCHBY ,  searchBy),
               new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.USER_PK ,  objUser.PKUser),
              new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.REQUISITIONSRCHVALUE ,  searchValue),
               new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.TRANSACTIONTYPE ,  transactionType),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT ,  sbuPk)
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.GETMATERIALISSUEAUTO, colParameters).Tables[0];
            return dtSearchValue;

        }

        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <param name="objUser"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetIssuingList(GridPrams grid, int bizUnit, User objUser, int procID, int transactionType, string pageURL, DateTime? fromDate, DateTime? toDate, string issueNo, int trnStatus, int issueType, int issueTo, int issueStore, string lotNo, string refNo, int itmCatPK, int itmPk) //, int cmpPk
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.REQUISITIONLSTSTATUS , grid.SearchBy == ((object)DBNull.Value).ToString()|| grid.SearchBy == "Date"? (object)DBNull.Value : grid.SearchBy ),                                                                                                                         
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.REQUISITIONLSTSRCH , grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.TRANSACTIONTYPE , transactionType),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,   grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC,  grid.SortDirection),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY,   grid.SortBy1),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC,  grid.SortDirection1),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.USER_PK , objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.PAGE_URL , pageURL),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.FROMDATE, fromDate),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.TODATE,toDate)  ,
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_NO , issueNo==string.Empty?(object)DBNull.Value: "%"+issueNo+"%"),      
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_STATUS , trnStatus<0 ? (object)DBNull.Value:trnStatus ),     
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ISS_RCV_TYPE , issueType>0? issueType :(object)DBNull.Value),      
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ISS_RCV_PK , issueTo>0?  issueTo:(object)DBNull.Value),   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_DEPT , issueStore>0?  issueStore:(object)DBNull.Value),      
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_LOT_NO ,lotNo== string.Empty ? (object)DBNull.Value : lotNo),  
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_REF_NO ,refNo== string.Empty ? (object)DBNull.Value :  refNo),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ITEM_CATEGORY , itmCatPK>0?  itmCatPK:(object)DBNull.Value),     
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ITEM, itmPk>0?  itmPk:(object)DBNull.Value),     
              //new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_COMPANY, cmpPk>0?  cmpPk:(object)DBNull.Value)
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_MENU_TYPE, transactionType == 7 ?  transactionType : (object)DBNull.Value)
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.GETMATERIALISSUELIST, colParameters);
            return dtRequisition;


        }

       /// <summary>
        /// Get Issuing List with Workflow
       /// </summary>
       /// <param name="grid"></param>
       /// <param name="bizUnit"></param>
       /// <param name="objUser"></param>
       /// <param name="procID"></param>
       /// <param name="transactionType"></param>
       /// <param name="pageURL"></param>
       /// <param name="fromDate"></param>
       /// <param name="toDate"></param>
       /// <param name="issueNo"></param>
       /// <param name="trnStatus"></param>
       /// <param name="issueType"></param>
       /// <param name="issueTo"></param>
       /// <param name="issueStore"></param>
       /// <param name="lotNo"></param>
       /// <param name="refNo"></param>
       /// <param name="itmCatPK"></param>
       /// <param name="itmPk"></param>
       /// <returns></returns>
        public static DataSet GetIssuingListWkf(GridPrams grid, int bizUnit, User objUser, int procID, int transactionType, string pageURL, DateTime? fromDate, DateTime? toDate, string issueNo, int trnStatus, int issueType, int issueTo, int issueStore, string lotNo, string refNo, int itmCatPK, int itmPk) //, int cmpPk
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.REQUISITIONLSTSTATUS , grid.SearchBy == ((object)DBNull.Value).ToString()|| grid.SearchBy == "Date"? (object)DBNull.Value : grid.SearchBy ),                                                                                                                         
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.REQUISITIONLSTSRCH , grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.TRANSACTIONTYPE , transactionType),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,   grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC,  grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY,   grid.SortBy1),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC,  grid.SortDirection1),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.USER_PK , objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.PAGE_URL , pageURL),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.FROMDATE, fromDate),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.TODATE,toDate)  ,
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_NO , issueNo==string.Empty?(object)DBNull.Value: issueNo),      
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_STATUS , trnStatus<0 ? (object)DBNull.Value:trnStatus ),     
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ISS_RCV_TYPE , issueType>0? issueType :(object)DBNull.Value),      
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ISS_RCV_PK , issueTo>0?  issueTo:(object)DBNull.Value),   
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_DEPT , issueStore>0?  issueStore:(object)DBNull.Value),      
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_LOT_NO ,lotNo== string.Empty ? (object)DBNull.Value : lotNo),  
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_REF_NO ,refNo== string.Empty ? (object)DBNull.Value :  refNo),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ITEM_CATEGORY , itmCatPK>0?  itmCatPK:(object)DBNull.Value),     
              new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ITEM, itmPk>0?  itmPk:(object)DBNull.Value),     
             // new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_COMPANY, cmpPk>0?  cmpPk:(object)DBNull.Value)
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.GETMATERIALISSUELISTWKF, colParameters);
            return dtRequisition;


        }
        //GetICHNo
        /// <summary>
        /// Function Used To Get SRS No 
        /// transactionType 1 for Material Issue 2 for material return
        /// </summary>
        /// <returns>string</returns>
        public static string GetICHNo(int bizUnit,int transactionType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TRANSACTIONTYPE,  transactionType),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.ICHPK, 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.GETNEXTNO, colParameters);

            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_ExternalMaterialIssue.ICHPK]).Value);

        }

        /// <summary>
        /// Get Requisition Details By Requisition Id as A Xml Format
        /// </summary>
        /// <param name="requisitionID"></param>
        /// <returns>Xml Formatted Requisition Details</returns>
        public static string GetMaterialIssueDetails(int materialIssueID)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.ICHPK ,  materialIssueID)
            };
            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.GETMATERIALISSUEDTL, colParameters);
            string strRetVal = "";
            for (int i = 0; i < dtRequisition.Tables[0].Rows.Count; i++)
                strRetVal += Convert.ToString(dtRequisition.Tables[0].Rows[i][0]);//.ToString();
            strRetVal = strRetVal == "" ? "<root />" : strRetVal;
            return strRetVal;

            //return Convert.ToString(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_MaterialConsumption.GETITEMCONSUMPTION, colParameters));

        }
        /// <summary>
        /// Get Requisition Details By Requisition Id as A Xml Format
        /// </summary>
        /// <param name="requisitionID"></param>
        /// <returns>Xml Formatted Requisition Details</returns>
        public static string GetDetailsFromCRDR(int crdrPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.CDH_PK ,  crdrPK)
            };
            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.GETDTLFROMCRDR, colParameters);
            string strRetVal = "";
            for (int i = 0; i < dtRequisition.Tables[0].Rows.Count; i++)
                strRetVal += Convert.ToString(dtRequisition.Tables[0].Rows[i][0]);//.ToString();
            strRetVal = strRetVal == "" ? "<root />" : strRetVal;
            return strRetVal;

            //return Convert.ToString(dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_MaterialConsumption.GETITEMCONSUMPTION, colParameters));

        }
        /// <summary>
        /// Delete Requisition Details By MRHPK
        /// </summary>
        /// <param name="MRHPK"></param>
        /// <returns>int= 1(Success)</returns>
        public static int DeleteExternalMaterialIssue(int ICHPK,int? UserPk,int HasWorkflow=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.ICHPK,  ICHPK == 0 ? (object)DBNull.Value :  ICHPK),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.USER_PK,  UserPk == 0 ? (object)DBNull.Value :  UserPk),
                 new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_HAS_WKF,  HasWorkflow),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.REQUISITIONRETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.DELETEMATERIALISSUE, colParameters);

            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_ExternalMaterialIssue.REQUISITIONRETURNVALUE]).Value);



        }

        /// <summary>
        /// function used to store requisition report
        /// </summary>
        /// <param name="requisitionID"></param>
        /// <returns></returns>
        public static DataSet GetIssuingReportByReqId(int requistID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
             new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.ICHPK ,  requistID),  
             
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.MATERIALISSUEREPORT, colParameters);
            return dtRequisition;


        }


       /// <summary>
        /// function used to Store Requisition (EMI)report
       /// </summary>
       /// <param name="requistID"></param>
       /// <returns></returns>
        public static DataSet GetStoreRequisitionByReqId(int requistID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
             new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.STREQICHPK ,  requistID),  
             
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.STOREREQUISITIONREPORT, colParameters);
            return dtRequisition;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configText"></param>
        /// <param name="spclCond"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetWeightedAverage(int active, string acfSetting, string acfData, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ACTIVE, active),
               new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ACF_SETTING, acfSetting == string.Empty ? (object)DBNull.Value : acfSetting),
               new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ACF_DATA, acfData==string.Empty ? (object)DBNull.Value : acfData),
               new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_BIZUNIT, acfData==string.Empty ? (object)DBNull.Value : bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.SPADM_APP_CONFIG_MST_GET, colParameters).Tables[0];
        }


        public static DataTable GetGRNAutoComplteList(int bizUnit, string srchValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),               
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, srchValue)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedures.SPINV_EMI_GRN_AUTO, colParameters).Tables[0];

        }

        public static string GetGRNDetailsList(int GRNPK, int IssuingStore)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_GRH_PK ,  GRNPK),
                new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_DEPT ,  IssuingStore)
            };
            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.SPINV_EMI_GRN_DTL_GET, colParameters);
            string strRetVal = "";
            for (int i = 0; i < dtRequisition.Tables[0].Rows.Count; i++)
                strRetVal += Convert.ToString(dtRequisition.Tables[0].Rows[i][0]);//.ToString();
            strRetVal = strRetVal == "" ? "<root />" : strRetVal;
            return strRetVal;
        }

        #region Material Issue (External Material Issue Multiple)
        /// <summary>
        /// Get Parent Issuing Type List .
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="parentDepartment"></param>
        /// <returns></returns>
        public static DataTable GetItemNameDDL(string searchVal, int bizUnit, int issuingType, int issueAgainst)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, 1),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.ISSUINGTYPE, issueAgainst),
                 new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_SEARCHVAL, searchVal),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ISS_RCV_SUB_TYPE, issuingType==0 ? (object)DBNull.Value : issuingType)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Procedures.GETIISUINGYOLIST, colParameters).Tables[0];
        }

        public static string GetEMIDetails(int CurrPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(Parameters.P_ICH_PK ,  CurrPK>0 ? CurrPK : (object)DBNull.Value)          
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPINV_ITEM_EXT_ISS_RCV_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// function used to store requisition report
        /// </summary>
        /// <param name="requisitionID"></param>
        /// <returns></returns>
        public static DataSet GetEMIMultipleReport(int requistID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
             new DBService.Parameters( GTIService.Constants.Store.Parameters_ExternalMaterialIssue.ICHPK ,  requistID),  
             
            };

            DataSet dtRequisition = new DataSet();
            dtRequisition = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_ExternalMaterialIssue.EMIMULTIPLEREPORT, colParameters);
            return dtRequisition;


        }
        #endregion
    }
}
