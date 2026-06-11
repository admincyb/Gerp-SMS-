using System;
using System.Data;
using BusinessObject;
using GTIService.Constants.Common;
namespace DataAccess.VendorManagement
{
    public class VendorMasterDL
    {
        #region Methods For Machinery Vendor Management

        /// <summary>
        /// Save Vendor Details
        /// </summary>
        /// <param name="customerName"></param>
        /// <returns>int VendorPK</returns>
        public static int SaveVendor(BusinessObject.MachineryManagement.Machinery.Vendor vendor)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.PK ,   vendor.vendorPK == 0 ? (object)DBNull.Value :  vendor.vendorPK) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.CONTACTNAME ,   vendor.ContactName) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.VENDORNAME ,   vendor.VendorName) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.VENDORPHONE,   vendor.VendorPhone) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.EMAIL ,   vendor.Email) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.ADDRESS ,   vendor.Address) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.MODBY,   vendor.UserPK) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.STATUS ,  GTIService.Constants.Machinery.Fields.ACTIVESTATUS) ,
                new DBService.Parameters(GTIService.Constants.Machinery.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)            
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.SAVEVENDORDTLS, colParameters);

            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Machinery.Parameters.RETVAL]).Value);


        }
        /// <summary>
        /// Get vendor Details
        /// </summary>
        /// <param name="vendorPK"></param>
        /// <returns>Datatable - vendor Details</returns>
        public static DataTable GetVendor()
        {
            DBService dbService = new DBService();
            DataTable dtVendor = new DataTable();
            dtVendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETVENDORDTLS).Tables[1];
            return dtVendor;
        }
        /// <summary>
        /// Delete Vendor Details By VendorID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>int - 1- Success</returns>
        public static int DeleteVendorDtls(int vendorID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.Machinery.Parameters.PK , vendorID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.DELETEVENDORDTLS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);
        }
        /// <summary>
        /// Get Vendor Details For Listing
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>Dataset - Table[0]- Count, table[1] - VendorDetails</returns>
        public static DataSet GetVendorDtls(GridPrams grid)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {           
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.PK, grid.SearchBy == GTIService.Constants.Machinery.Fields.DEFAULTSEARCHBY? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.FIELDS,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.PAGENUMBER,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.SORTBY,  grid.SortBy == null ? GTIService.Constants.Machinery.Fields.VENDORNAME : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Machinery.Parameters.SORTDIRECTION, grid.SortDirection == null ? GTIService.Constants.Machinery.Fields.ORDERBYASC : grid.SortDirection),
            };
            DataSet dsVendor = new DataSet();
            dsVendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Machinery.Procedures.GETVENDORDTLS, colParameters);
            return dsVendor;
        }

        #endregion

        #region Methods For Vendor Management

        /// <summary>
        /// Getting Country for Auto Complete
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static DataTable GetCountry(string searchValue)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   

              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PGSEARCHBY,  GTIService.Constants.Vendor.Fields.VNDCOUNTRY),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PGSEARCHVAL,  searchValue),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SEARCHAUTO, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Getting States for Auto Complete
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static DataTable GetStates(string searchValue)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   

              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PGSEARCHBY,  GTIService.Constants.Vendor.Fields.VNDSTATE),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PGSEARCHVAL,  searchValue),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SEARCHAUTO, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Get Vendors for listing
        /// </summary>
        /// <returns>Datatable</returns>
        public static DataTable GetVendors(int BizUnitPk, int? vendertype = null, int? active = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDRSPK, 0),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_PO_TYPE, vendertype.HasValue ?vendertype<0?(object)DBNull.Value:vendertype :(object)DBNull.Value ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, BizUnitPk),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VEN_ACTIVE, active.HasValue ? active :(object)DBNull.Value)
            };

            DataTable dtUOMType = new DataTable();
            dtUOMType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDOR , colParameters).Tables[0];
            return dtUOMType;


        }

        /// <summary>
        /// Get Active Vendors
        /// </summary>
        /// <returns>Datatable</returns>
        public static DataTable GetActiveVendors(int BizUnitPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDRSPK, 0),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, BizUnitPk),              
            };

            DataTable dtUOMType = new DataTable();
            dtUOMType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDOR, colParameters).Tables[0];
            return dtUOMType;


        }

        /// <summary>
        /// Get Active Vendors(Dealer & PM Manufacturer only)
        /// </summary>
        /// <returns>Datatable</returns>
        public static DataTable GetActiveVendorsOnly(int BizUnitPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDRSPK, 0),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, BizUnitPk),              
            };

            DataTable dtUOMType = new DataTable();
            dtUOMType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORONLY, colParameters).Tables[0];
            return dtUOMType;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BizUnitPk"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataTable GetTypeVendors(int BizUnitPk,int type)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_PK, (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, BizUnitPk),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITC_VALUE, type>0 ? type : 1),
              
            };

            DataTable dtUOMType = new DataTable();
            dtUOMType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORTYPE, colParameters).Tables[0];
            return dtUOMType;


        }

        /// <summary>
        /// All Active Vendors only 
        /// </summary>
        /// <param name="BizUnitPk"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataTable GetTypeVendorsActive(int BizUnitPk, int type,int VenPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_PK, VenPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, BizUnitPk),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITC_VALUE, type>0 ? type : 1),
              
            };

            DataTable dtUOMType = new DataTable();
            dtUOMType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORTYPE, colParameters).Tables[0];
            return dtUOMType;


        }

        /// <summary>
        /// Get vendor Address and Code
        /// </summary>
        /// <returns>Datatable</returns>
        public static DataTable GetVendorAddress(int vendorID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDRSPK, vendorID==0?(object)DBNull.Value:vendorID)
              
            };

            DataTable dtUOMType = new DataTable();
            dtUOMType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORDTL, colParameters).Tables[0];
            dtUOMType.Rows[0]["VEN_ADDR1"] = dtUOMType.Rows[0]["VEN_ADDR1"].ToString() + " " + dtUOMType.Rows[0]["VEN_ADDR2"].ToString() + " " + dtUOMType.Rows[0]["VEN_CITY"].ToString() + " " + dtUOMType.Rows[0]["VEN_PIN"].ToString();
            return dtUOMType;


        }
        /// <summary>
        /// Getting Values for Auto Complete
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static DataTable GetSearchVals(string searchBy, string searchValue)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   

              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PGSEARCHBY,  searchBy),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PGSEARCHVAL,  searchValue),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SEARCHAUTO, colParameters).Tables[0];//SEARCHVENDOR
            return dtSearchValue;

        }
        /// <summary>
        /// Get Currency
        /// </summary>
        /// <returns>Datatable</returns>
        public static DataTable GetCurrency()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDPK, (object)DBNull.Value)
              
            };

            DataTable dtUOMType = new DataTable();
            dtUOMType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETCURRENCY, colParameters).Tables[0];
            return dtUOMType;


        }
        /// <summary>
        /// Save Vendor Details
        /// </summary>
        /// <param name="customerName"></param>
        /// <returns>int VendorPK</returns>
        public static int SaveVendorDetails(BusinessObject.VendorManagement.Vendor vendor)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDSTATUS ,  GTIService.Constants.Vendor.Fields.ACTIVESTATUS) ,
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO ,  string.Empty, 50,ParameterDirection.Output, DBService.ParameterType.NVarChar),           
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SAVEVENDOR, colParameters);
            string RetNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Vendor.Parameters.RETVAL]).Value);

        }
        /// <summary>
        /// Get Vendor Details For Listing
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>Dataset - Table[0]- Count, table[1] - VendorDetails</returns>
        public static DataSet GetVendorsList(GridPrams grid)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {           
               new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PGSEARCHBY , grid.SearchBy == string.Empty || grid.SearchBy == "0" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PGSEARCHVAL , grid.SearchValue == string.Empty ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PGFLDS,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters.PGNUMR,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters.PGSIZ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters.SORT,  grid.SortBy == null ? GTIService.Constants.Vendor.Fields.VENDORNAME : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters.SORTDRTION, grid.SortDirection == null ? GTIService.Constants.Vendor.Fields.ORDERBYASC : grid.SortDirection),
            };
            DataSet dsVendor = new DataSet();
            dsVendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORLIST, colParameters);
            return dsVendor;
        }
        /// <summary>
        /// Delete Vendor Details By VendorID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>int - 1- Success</returns>
        public static int DeleteVendorDetails(int vendorID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDPK , vendorID),
            };
            return dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.DELVENDOR, colParameters);
             
        }

        /// <summary>
        /// Methode used to get the purhase order vendor 
        /// </summary>
        /// <param name="bizUnitPk"></param>
        /// <returns></returns>
        public static DataTable GetPurchaseOrderVendors(int bizUnitPk, int grhPK, int shpDeptPK = 0 )
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPk),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.GRNPK, grhPK == 0 ? (object)DBNull.Value : grhPK),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_GoodsReceiptNote.SHIP_DEPT, shpDeptPK == 0 ? (object)DBNull.Value : shpDeptPK)
            };
            DataTable dtVendor = new DataTable();
            dtVendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETPURCHASEORDERVENDOR, colParameters).Tables[0];
            return dtVendor;
        }

        //////////////////////////////////////////////////////////////////////////////New
        /// <summary>
        /// Get Address Type
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAddressTypeList(int bizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, 1),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.ADDRESSTYPE, "VENDOR CONTACT TYPE"),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPK)          
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.ADDRESSTYPEGET, colParameters).Tables[0];
            return dtParams;
        }
        #endregion

        public static DataTable GetVendor(User objUser, int VendorPk, short Active, string VendorName)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDRSPK,(VendorPk > 0)? VendorPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VEN_ACTIVE, Active ),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_NAME,(string.IsNullOrEmpty(VendorName))? (object)DBNull.Value : VendorName)
              
            };

            DataTable dtVendor = new DataTable();
            dtVendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDOR, colParameters).Tables[0];
            return dtVendor;
        }

        /// <summary>
        /// Get Vendor banks
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="VendorbankPk"></param>
        /// <param name="VendorPk"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetVendorBanks(User objUser, int VendorBankPk, int VendorPk, short Active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VBD_PK,VendorBankPk),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VENDOR,(VendorPk > 0)? VendorPk : (object)DBNull.Value),                
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_ACTIVE, Active )              
            };

            DataTable dtVendorBank = new DataTable();
            dtVendorBank = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORBANKDTL, colParameters).Tables[0];
            return dtVendorBank;
        }


        public static DataTable GetVendorLocalAddress(User objUser, int VendorLcAddrsPk, int VendorPk, short Active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                 new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VNC_LC_PK,VendorLcAddrsPk),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VENDOR,(VendorPk > 0)? VendorPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_ACTIVE, Active )
            };

            DataTable dtVendorLcAddress = new DataTable();
            dtVendorLcAddress = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SPPUR_VENDOR_CONTACT_LOC_GET_KV, colParameters).Tables[0];
            return dtVendorLcAddress;
        }

        /// <summary>
        ///  Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="pageURL"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetVendorAutoSearch(string searchBy, string searchValue, int bizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY, searchBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPK)         
             
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SPPUR_VENDOR_GET_AUTO, colParameters).Tables[0];
            return dtSearchValue;

        }
        public static DataTable GetPOVendorAutoSearch(string searchValue, int bizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_VEN_NAME, searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPK) ,
              new DBService.Parameters("VEN_PK",0),
              new DBService.Parameters("VEN_ACTIVE",1)
             
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDOR, colParameters).Tables[0];
            return dtSearchValue;

        }


        /// <summary>
        /// Getting Values for Auto Complete
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static DataTable GetVendorsByRoleAuto(int venPK, int vrmRole, string venName, int bizUnitPK, int active)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   

              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_PK,(venPK > 0)? venPK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_NAME,venName),   
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VRM_ROLE,vrmRole),       
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPK)  ,
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_ACTIVE, active )              
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SPASR_VENDOR_ROLE_GET_KV, colParameters).Tables[0];
            return dtSearchValue;

        }
    }
}
