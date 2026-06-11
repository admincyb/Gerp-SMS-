using System;
using System.Data;
using BusinessObject;

namespace DataAccess.SubDepartmentManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class SubDepartmentMasterDL
    {
        #region Methods
        /// <summary>
        /// Get All Department Details By UserID and sbupk
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetDepartmentDtls(User objUser, int sbuPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
                  new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.USERID  ,objUser.PKUser == 0?(object)DBNull.Value :objUser.PKUser  ),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
             };
            DataTable dtRunByType = new DataTable();
            dtRunByType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.GETDEPTDTLS, colParameters).Tables[0];
            return dtRunByType;
        }
        public static DataTable GetDepartmentDtls(int sbuPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
                
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
             };
            DataTable dtRunByType = new DataTable();
            dtRunByType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.GETDEPTDTLS, colParameters).Tables[0];
            return dtRunByType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static DataTable GetNonStoreDeptDeparment(User objUser, int sbuPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.USERPK  ,  objUser.PKUser),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
             };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.GETNONSTOREDEPT, colParameters).Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>GetNonStoreDeptUserSwitch
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static DataTable GetNonStoreDeptUserSwitch(int sbuPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.USERPK  , (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
             };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.GETNONSTOREDEPT, colParameters).Tables[0];
        }
        /// <summary>
        /// Get All Department Details By UserID and base dpt and sbupk
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="baseDpt"></param>
        /// <returns></returns>
        public static DataTable GetDepartmentDtls(User objUser, int sbuPk, int baseDpt)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.USERID  ,  objUser.PKUser),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
                   new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DEPTPK,  baseDpt),
             };
            DataTable dtRunByType = new DataTable();
            dtRunByType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.GETDEPTDTLS, colParameters).Tables[0];
            return dtRunByType;
        }
        /// <summary>
        /// Save Sub Departmetn Details
        /// </summary>
        /// <param name="subDept"></param>
        /// <returns>string</returns>   //subDept.Status
        public static string SaveSubDepartment(BusinessObject.SubDepartmentManagement.SubDepartmentMaster subDept)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.DEPTPK ,   subDept.DPT_PK == 0?(object)DBNull.Value :subDept.DPT_PK ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.DEPTCODE ,   subDept.DPT_CODE ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.DEPTNAME ,   subDept.DPT_NAME ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.DEPTDESCRIPTION ,   subDept.DPT_DESC==null?string.Empty:subDept.DPT_DESC) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.DEPTPARENT ,   subDept.DPT_PARENT) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.DEPTCATEGORY ,   subDept.DPT_CATEGORY) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.ADDRESS1  ,   subDept.DPT_ADDR1==null?string.Empty:subDept.DPT_ADDR1 ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.ADDRESS2,   subDept.DPT_ADDR2==null?string.Empty:subDept.DPT_ADDR2 ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.EMAIL ,   subDept.DPT_EMAIL==null?string.Empty:subDept.DPT_EMAIL ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.PHONE,   subDept.DPT_PHONE==null?string.Empty:subDept.DPT_PHONE ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.CITY ,   subDept.DPT_CITY==null?string.Empty:subDept.DPT_CITY ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.ZIPCODE ,   subDept.DPT_ZIP==null?string.Empty:subDept.DPT_ZIP ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.MOBILE ,   subDept.DPT_MOBILE==null?string.Empty:subDept.DPT_MOBILE ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.GSTNO ,   subDept.DPT_GST_NO==null?string.Empty:subDept.DPT_GST_NO ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.STATE ,   subDept.DPT_STATE) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.COUNTRY ,   subDept.DPT_CNTRY ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.CURRENCY ,   subDept.DPT_CURR) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.BIZUNIT ,   subDept.SBU ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.ACTIVE , string.IsNullOrEmpty(subDept.DPT_ACTIVE)==true?0:1),
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.IS_STOCK , string.IsNullOrEmpty(subDept.DPT_IS_STOCK)==true?0:1),
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.CREATEDBY ,   subDept.UserPK ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.MODBY ,   subDept.UserPK ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.PROJECT ,  Convert.ToInt32(subDept.DPT_PROJECT) > 0 ? subDept.DPT_PROJECT : (object)DBNull.Value ),
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.PLANT ,   subDept.DPT_COMPANY ),

                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.RETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.SAVESUBDEPT, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.SubDepartment.Parameters.RETURNVALUE]).Value).ToString();
        }
        /// <summary>
        /// Get Sub Department Details
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSubDepartmentList(GridPrams grid, int bizUnit, int Status)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ? "DPT_PARENT_NAME": grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DPT_ACTIVE , Status),
              
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ? GTIService.Constants.SubDepartment.Fields.ORDERBYDEPTPK: grid.SortBy),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.SubDepartment.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection==null ? "ASC" :  grid.SortDirection),

            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.GETSUBDEPTLIST, colParameters);
            return dsDesig;


        }
        /// <summary>
        /// Get All Search Type Details For AutoComplete
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit), 
              new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.SEARCHFILED  ,  searchBy),  
              new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.SEARCHFIELDVALUE   ,  searchValue),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.SUBDEPTDTLSAUTOSEARCH, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Get Sub Department Deatils By Sub DeptID
        /// </summary>
        /// <param name="subDeptID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSubDepartmentDtlsBySubDeptID(int subDeptID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                            
               new DBService.Parameters( GTIService.Constants.SubDepartment.Parameters.DEPTPK ,  subDeptID),  
               new DBService.Parameters( GTIService.Constants.SubDepartment.Parameters.ACTIVE ,  DBNull.Value)
            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.SUBDEPTDTLSBYID, colParameters);
            return dsDesig;


        }
        /// <summary>
        /// Delete SubDepartment Details
        /// </summary>
        /// <param name="subDeptID"></param>
        /// <returns>int</returns>
        public static int DeleteSubDepartmentDtls(int subDeptID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.DEPTPK,  subDeptID),
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.RETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.DELETESUBDEPT, colParameters);

            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.SubDepartment.Parameters.RETURNVALUE]).Value);



        }

        /// <summary>
        /// Get All Inventory department details 
        /// </summary>
        /// <Created By>Vineeth</Created>
        /// <For>PO Dept Filling</For>
        /// <Used In>PoDepFilling,FillSelectedDepartDetails </Used>
        /// <param name="userID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetInvDepartment(int bIZUNIT, int deptID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
             {   
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT ,  bIZUNIT),
                 new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.DEPTPK  ,  deptID == 0 ?(object)DBNull.Value:deptID),
             };
            DataTable dtInvDep = new DataTable();
            dtInvDep = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.GETINVSUBDEPT, colParameters).Tables[0];
            return dtInvDep;
        }      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>
        public static DataTable GetInventoryStores(int sbuPk, int deptType, int userPK,int deptCompany=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("P_ACTIVE",1),
              new DBService.Parameters("DPT_CATEGORY", deptType),
              new DBService.Parameters("P_USER", userPK == 0 ? (object)DBNull.Value : userPK), 
              new DBService.Parameters("P_BIZUNIT", sbuPk),
              new DBService.Parameters("P_DPT_COMPANY" ,deptCompany==0?(object)DBNull.Value:deptCompany),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_DEPT_STORE_GET_KV", colParameters).Tables[0];
        }
        public static DataTable GetIssuingStores(int sbuPk, int deptType, int userPK,int deptCompany, int MaterialPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("P_ACTIVE",1),
              new DBService.Parameters("DPT_CATEGORY", deptType),
              new DBService.Parameters("P_USER", userPK == 0 ? (object)DBNull.Value : userPK), 
              new DBService.Parameters("P_BIZUNIT", sbuPk),
              new DBService.Parameters("P_DPT_COMPANY" ,deptCompany==0?(object)DBNull.Value:deptCompany),
              new DBService.Parameters("P_ITM_PK" ,MaterialPK==0?(object)DBNull.Value:MaterialPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPADM_DEPT_MI_STORE_GET_KV", colParameters).Tables[0];
        }


        /// Function Used To Get all store and store pk By Type
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetStoresByType(int deptPk, int deptType, User objUser, int sbuPk, int userFlag,int MenuType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("STR_PK" ,  deptPk==0?(object)DBNull.Value:deptPk),
              new DBService.Parameters("DPT_TYPE" ,deptType==0?(object)DBNull.Value:deptType),
              new DBService.Parameters("RMSP",  objUser.PKUser),
              new DBService.Parameters("P_BIZUNIT",   sbuPk),
              new DBService.Parameters("DPT_ACTIVE",   1),
              new DBService.Parameters("DPT_CATEGORY",MenuType==0? Convert.ToInt32( GTIService.Constants.Common.StoresGetFlag.GeneralAndCompoundStore) :  Convert.ToInt32( GTIService.Constants.Common.StoresGetFlag.WOStore)),
              new DBService.Parameters("FLAG", userFlag ),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_STORE_DEPT_GET_KV", colParameters).Tables[0];
        }

        public static DataTable GetStoresByTypeNew(int deptPk, int deptType, User objUser, int sbuPk, int userFlag, int MenuType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
              new DBService.Parameters("STR_PK" ,  deptPk==0?(object)DBNull.Value:deptPk),
              new DBService.Parameters("DPT_TYPE" ,deptType==0?(object)DBNull.Value:deptType),
              new DBService.Parameters("RMSP",  objUser.PKUser),
              new DBService.Parameters("P_BIZUNIT",   sbuPk),
              new DBService.Parameters("DPT_ACTIVE",   1),
              new DBService.Parameters("DPT_CATEGORY", MenuType),
              new DBService.Parameters("FLAG", userFlag ),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_STORE_DEPT_GET_KV", colParameters).Tables[0];
        }

        /// Function Used To Get all store and store pk By Type
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetGeneralStores(int deptPk, int deptType, User objUser, int sbuPk, int userFlag, int deptChild=0,int deptCompany=0,int DeptCategory=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("STR_PK" ,  deptPk==0?(object)DBNull.Value:deptPk),
              new DBService.Parameters("DPT_TYPE" ,deptType==0?(object)DBNull.Value:deptType),
              new DBService.Parameters("RMSP",  objUser.PKUser),
              new DBService.Parameters("P_BIZUNIT",   sbuPk),
              new DBService.Parameters("DPT_ACTIVE",   1),
              new DBService.Parameters("DPT_CATEGORY", DeptCategory==0 ? Convert.ToInt32( GTIService.Constants.Common.StoresGetFlag.GeneralStores) : DeptCategory ),
              new DBService.Parameters("FLAG", userFlag ),
              new DBService.Parameters("P_DPT_CHILD" ,deptChild==0?(object)DBNull.Value:deptChild),
              new DBService.Parameters("P_DPT_COMPANY" ,deptCompany==0?(object)DBNull.Value:deptCompany),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_STORE_DEPT_GET_KV", colParameters).Tables[0];
        }

        /// Function Used To Get all store and store pk By Type
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetAllStores(int deptPk, int deptType, User objUser, int sbuPk, int userFlag, int deptCatg)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("STR_PK" ,  deptPk==0?(object)DBNull.Value:deptPk),
              new DBService.Parameters("DPT_TYPE" ,deptType==0?(object)DBNull.Value:deptType),
              new DBService.Parameters("RMSP",  objUser.PKUser),
              new DBService.Parameters("P_BIZUNIT",   sbuPk),
              new DBService.Parameters("DPT_ACTIVE",   1),
              new DBService.Parameters("DPT_CATEGORY",  deptCatg==0?(object)DBNull.Value:deptCatg),
              new DBService.Parameters("FLAG", userFlag ),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_STORE_DEPT_GET_KV", colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>
        public static DataTable GetInventoryStoresBasedOnConfig(string fieldName, int sbuPk,int userPK, int processId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("P_FLD_NAME",fieldName==null?string.Empty:fieldName),
              new DBService.Parameters("P_BIZUNIT", sbuPk),
              new DBService.Parameters("P_PROCESS" ,processId==0?(object)DBNull.Value:processId),
              new DBService.Parameters("P_USER_PK", userPK == 0 ? (object)DBNull.Value : userPK)            
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.SPPUR_ORDER_REQ_MAP_AUTO, colParameters).Tables[0];
        }

        #endregion       

    }
}

