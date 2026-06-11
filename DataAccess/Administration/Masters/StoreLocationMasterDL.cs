using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.Administration.Masters
{
   public class StoreLocationMasterDL
    {
        #region Methods
        /// <summary>
        /// Get All Department Details By UserID and sbupk
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetProductionDepartmentDtls(int BizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.P_BIZUNIT ,  BizUnit)
            };
            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPPRD_DEPT_LOC_AUTO, colParameters).Tables[0];
            return dtResult;
        }
       /// <summary>      
       /// Save Sub Departmetn Details
        /// </summary>       
       public static string SaveStoreLocationDtls(BusinessObject.Administration.Masters.StoreLocationMaster subDept)
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
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.STATE ,  subDept.DPT_STATE == 0?(object)DBNull.Value :subDept.DPT_STATE) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.COUNTRY ,subDept.DPT_CNTRY == 0?(object)DBNull.Value :subDept.DPT_CNTRY ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.CURRENCY ,subDept.DPT_CURR == 0?(object)DBNull.Value :subDept.DPT_CURR) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.BIZUNIT ,   subDept.SBU ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.ACTIVE , string.IsNullOrEmpty(subDept.DPT_ACTIVE)==true?0:1),
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.IS_STOCK , string.IsNullOrEmpty(subDept.DPT_IS_STOCK)==true?0:1),
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.CREATEDBY ,   subDept.UserPK ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.MODBY ,   subDept.UserPK ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.PROJECT ,   subDept.DPT_PROJECT > 0 ? subDept.DPT_PROJECT : (object)DBNull.Value ),

                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.RETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

                                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.ZIPCODE ,   subDept.DPT_ZIP==null?string.Empty:subDept.DPT_ZIP ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.MOBILE ,   subDept.DPT_MOBILE==null?string.Empty:subDept.DPT_MOBILE ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.GSTNO ,   subDept.DPT_GST_NO==null?string.Empty:subDept.DPT_GST_NO ) ,
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.PLANT ,   subDept.DPT_COMPANY==0?(object)DBNull.Value :subDept.DPT_COMPANY ) 
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SAVESTORELOC, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.SubDepartment.Parameters.RETURNVALUE]).Value).ToString();
        }

       /// <summary>
       /// Get Sub Department Details
       /// </summary>
       /// <param name="grid"></param>
       /// <returns>DataSet</returns>
       public static DataSet GetStoreLocationList(GridPrams grid, int bizUnit, int deptCategory)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),    
              new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_CATEGORY, deptCategory),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ? "DPT_PARENT_NAME": grid.SortBy),
            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection==null ? "ASC" :  grid.SortDirection),

            };

           DataSet dsDesig = new DataSet();
           dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.GETSTORELOCLIST, colParameters);
           return dsDesig;


       }

       /// <summary>
       /// Get StoreLocation Print Label List
       /// </summary>
       /// <param name="grid"></param>
       /// <returns>DataSet</returns>
       public static DataTable GetStoreLocationPrintLabelList(string searchName, string searchValue, int bizUnit, int deptCategory)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),    
              new DBService.Parameters(GTIService.Constants.Administration.Masters.Parameters.P_DPT_CATEGORY, deptCategory),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , searchName == string.Empty? (object)DBNull.Value : searchName ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , searchValue== string.Empty ? "%" :searchValue+"%"),             
            };

           DataTable dtStoreLocn = new DataTable();
           dtStoreLocn = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPADM_DEPT_MST_DTL_GET, colParameters).Tables[0];
           return dtStoreLocn;


       }

       /// <summary>
       /// Delete Store Location Details
       /// </summary>
       /// <param name="storeLocID"></param>
       /// <returns>int</returns>
       public static int DeleteStoreLocationDtls(int storeLocID)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;

           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.DEPTPK,  storeLocID),
                new DBService.Parameters(GTIService.Constants.SubDepartment.Parameters.RETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

           dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.SubDepartment.Procedures.DELETESUBDEPT, colParameters);

           return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.SubDepartment.Parameters.RETURNVALUE]).Value);



       }

       /// <summary>
       /// Get  store location details By StorelocID
       /// </summary>
       /// <param name="subDeptID"></param>
       /// <returns>DataSet</returns>
       public static DataSet GetStoreLocDtlsByID(int subDeptID)
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
        #endregion
    }
}
