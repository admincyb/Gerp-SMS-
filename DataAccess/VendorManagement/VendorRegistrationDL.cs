using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using BusinessObject;
using GTIService.Constants.Vendor;

namespace DataAccess.VendorManagement
{
    public class VendorRegistrationDL
    {
        public static List<object> SaveVendorDetails(string xmlVendorDetails)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            List<object> retvals = new List<object>();
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_XML ,(object)xmlVendorDetails,DBService.ParameterType.XML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO ,  string.Empty, 50,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                 
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SAVEVENDOR, colParameters);
            int result = Convert.ToInt32((((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value).ToString());
            string RetNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);

            retvals.Add(result);
            retvals.Add(RetNo);
            return retvals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlTaxDiscount"></param>
        /// <returns></returns>
        public static string TaxDiscountApply(string xmlTaxDiscount)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_XML ,(object)xmlTaxDiscount,DBService.ParameterType.XML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                 
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SAVEVETAXDISCOUNT, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value).ToString();
        }

        public static string GetVendorDetails(int vendorID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PVENDORID , vendorID),                
                 
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORDETILS, colParameters).Tables[0];
            return dtxml.Rows[0][0].ToString();
            
        }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemPK"></param>
    /// <param name="category"></param>
    /// <returns></returns>
        public static string GetVendorItemTaxDiscount(int itemPK,int category)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.V_ITV_PK , itemPK),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.V_IVT_TAX_CATEGORY , category),
                 
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SPINV_ITEM_VENDOR_TAX_GET, colParameters).Tables[0];
            string xmlString = string.Empty;
            for(int i=0;i<dtxml.Rows.Count;i++)
                xmlString+=dtxml.Rows[i][0].ToString();
            return xmlString;
            
        }


        public static DataTable GetVendorRole(int venPK, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                  new DBService.Parameters("P_BIZUNIT", bizUnit),
                  //new DBService.Parameters(GTIService.Constants.Administration.Masters.StoreMaterialMapping.Parameters.MATERIALMAPID, mapPK == 0 ? (object)DBNull.Value : mapPK),
                  new DBService.Parameters("P_VEN_PK" , venPK),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "SPPUR_VENDOR_ROLE_MAP_GET_TREE", colParameters).Tables[0];
        }

        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="fields"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="status"></param>
        /// <param name="Searchtxt"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetVendorDetails(GridPrams grid, int sbuID, int procId, string PageUrl, string venCode, string venName, int venType, string venPhone, int venActive)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            if (grid.SearchBy == "VEN_TYPE")
                grid.SearchValue = grid.SearchValue + "%";
            else
                grid.SearchValue = "%"+ grid.SearchValue + "%";
            
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0"? "VEN_NAME" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL , grid.SearchValue == "" ? "%" : grid.SearchValue,DataAccess.DBService.ParameterType.NVarChar),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , procId),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , grid.UserPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DEPTPK , grid.DeptPK == 0 ? 1 : grid.DeptPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy == null ? (object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "Asc" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,sbuID),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_CODE ,  venCode==string.Empty ?(object)DBNull.Value:venCode.Trim()),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_NAME ,  venName==string.Empty ?(object)DBNull.Value:venName),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_PO_TYPE ,  venType==0 ?(object)DBNull.Value:venType),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_PHONE ,  venPhone==string.Empty ?(object)DBNull.Value:venPhone.Trim()),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VEN_ACTIVE ,  venActive)
            };

            DataSet dtProduct = new DataSet();
            dtProduct = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORDETILSLIST, colParameters);
            return dtProduct;


        }

     /// <summary>
     /// Get Vendor Material 
     /// </summary>
     /// <param name="grid"></param>
     /// <param name="vendorID"></param>
     /// <returns></returns>
        public static DataSet GetVndMaterials(GridPrams grid, int vendorID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_VEN_PK ,vendorID ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
            };

            DataSet dtProduct = new DataSet();
            dtProduct = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SPINV_ITM_VND_GET_LIST, colParameters);
            return dtProduct;


        }



        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue,User objUser, int procId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHBY ,  searchBy),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHVALUE ,  searchValue),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.USERPK , objUser.PKUser),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT , objUser.SBUID),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.PROCESSID , procId),

            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Delete Vendor Material
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns></returns>
        public static int DeleteVndMaterial(int materialID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_ITV_PK , materialID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_ITEM_VENDOR_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }
        /// <summary>
        /// Methord to get the Delete Corresponding Vendor with Provided Vendor ID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>DataTable</returns>
        public static int DeleteVendorDetails(int vendorID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.VENDRSPK , vendorID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.DELETEVENDORS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }

        /// <summary>
        /// Methord to get the vedor details corresponding to a vendor
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetVendorDtls(int vendorID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Vendor.Parameters.VENDRSPK ,  vendorID),
              
            };
            DataTable dtvendor = new DataTable();
            dtvendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORDTL, colParameters).Tables[0];
            return dtvendor;
        }

        /// <summary>
        /// Methord to get the vedor details corresponding to a vendor
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetVendorDtlsStatus(int vendorID, int status)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Vendor.Parameters.VENDRSPK ,  vendorID),
               new DBService.Parameters( GTIService.Constants.Vendor.Parameters.VEN_ACTIVE ,  status>0?status:(object)DBNull.Value),
              
            };
            DataTable dtvendor = new DataTable();
            dtvendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORDTL, colParameters).Tables[0];
            return dtvendor;
        }


        /// <summary>
        /// Get Vendor mapped Material list
        /// </summary>
        /// <Created By>Vineeth</Created>
        /// <For>PO Filling Material Corresponding to Vendor selected</For>
        /// <Used In>Pomaterial Filling </Used>
        /// <param name="vendorID"></param>
        /// <returns>Datatable</returns>
        public static DataTable GetVendorMaterials(int vendorID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDRMTRSPK, vendorID), 
            };

            DataTable dtSuppliedMaterial = new DataTable();
            dtSuppliedMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETESUPPLIEDMATERIAL, colParameters).Tables[0];
            return dtSuppliedMaterial;
        }
        /// <summary>
        ///  Get Vendor store mapped Material list
        /// </summary>
        /// <param name="vendorID"></param>
        /// <param name="storeID"></param>
        /// <returns></returns>
        public static DataTable GetVendorStoreMaterials(int vendorID, int storeID, int userPK, string searchValue = "",int itemPk=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDORPK, vendorID), 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_DEPT, storeID > 0 ? storeID : (object)DBNull.Value), 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_USER, userPK > 0 ? userPK :  (object)DBNull.Value), 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_ITEM_TEXT, searchValue!=string.Empty ? searchValue :  (object)DBNull.Value), 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_ITM_PK, itemPk),   
            };

            DataTable dtSuppliedMaterial = new DataTable();
            dtSuppliedMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SPINV_ITEM_VENDOR_DEPT_GET_ITM, colParameters).Tables[0];
            return dtSuppliedMaterial;


        }

        /// <summary>
        /// Get Material Details corresponding to a vendor return materil detial and UOM Details
        /// </summary>
        /// <Created By>Vineeth</Created>
        /// <For>PO Filling Material details Corresponding to Vendor selected</For>
        /// <Used In>Pomaterial Filling </Used>
        /// <param name="vendorID"></param>
        /// <returns>Datatable</returns>
        public static DataTable GetVendorMaterialDetails(int vendorID,int materialID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDRMTRSPK, vendorID), 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PITMPK, materialID), 
            };

            DataTable dtMaterial = new DataTable();
            dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETMATERIALDETAILS, colParameters).Tables[0];
            return dtMaterial;


        }


        /// <summary>
        /// Get Material Details corresponding to a vendor return materil detial and UOM Details
        /// </summary>
        /// <Created By>Vineeth</Created>
        /// <For>PO Filling Material details Corresponding to Vendor selected</For>
        /// <Used In>Pomaterial Filling </Used>
        /// <param name="vendorID"></param>
        /// <returns>Datatable</returns>
        public static DataTable GetMaterialUOM(int vendorID, int materialID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDRMTRSPK, vendorID), 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PITMPK, materialID), 
            };

            DataTable dtUOM = new DataTable();
            dtUOM = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETMATERIALDETAILS, colParameters).Tables[1];
            return dtUOM;


        }

        /// <summary>
        /// Function used to get the vendor details for report
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns></returns>
        public static string GetVendorDetailsReport(int vendorID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            string xml = string.Empty;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PVENDORID , vendorID),                
                 
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORDETILSREPORT, colParameters).Tables[0];
            foreach (DataRow drRows in dtxml.Rows)
            {
                xml +=  drRows[0].ToString();
            }
            return xml;
        }

        public static DataSet GetVendorPerformanceReport(int vendorPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            string xml = string.Empty;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_VEN_PK , vendorPK),                
                 
            };
            DataSet dsResult= dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SPPUR_VENDOR_REG_RPT, colParameters);
            //foreach (DataRow drRows in dtxml.Rows)
            //{
            //    xml += drRows[0].ToString();
            //}
            return dsResult;
        }
        /// <summary>
        /// Get Report SubType
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        public static DataSet GetAppSubType(string appCode)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            string xml = string.Empty;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_APT_CODE , appCode),                
                 
            };
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SPADM_APP_SUB_TYPE_DATA_GET, colParameters);
            //foreach (DataRow drRows in dtxml.Rows)
            //{
            //    xml += drRows[0].ToString();
            //}
            return dsResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetVendorPOType(BusinessObject.User objUser, string CfgType, int Active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CfgPK,0),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,  Active),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CFG_TYPE, CfgType),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORPOTYPE, colParameters).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlVendorBank"></param>
        /// <returns>string VBD_PK (Bank Id)</returns>
        public static string SaveVendorBank(string xmlVendorBank)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_XML ,(object)xmlVendorBank,DBService.ParameterType.XML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                 
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SAVEVENDORBANK, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value).ToString();

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlVendorBank"></param>
        /// <returns>string VBD_PK (Bank Id)</returns>
        public static string SaveVenLocalAddress(string xmlVendorLocalAddress)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_XML ,(object)xmlVendorLocalAddress,DBService.ParameterType.XML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SPPUR_VENDOR_CONTACT_LOC_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value).ToString();

        }



        /// <summary>
        /// 
        /// </summary>

        /// <returns>DataTable</returns>
        public static DataTable GetAccountTypesBank()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_PK,0),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_ACTIVE,  1),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CGT_VALUE, 9),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CNG_VALUE, 4),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CON_BIZUNIT, 1)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Common.Common.SP_CONST_MST_GET, colParameters).Tables[0];
        }

        /// <summary>
        /// Delete Vendor Bank
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>int</returns>
        public static int DeleteVndBank(int bankId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_VBD_PK , bankId),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.DELETEVENDORBANK, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }

        public static int DeleteVenLocalAddress(int vnclcId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_VNC_LC_PK , vnclcId),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPPUR_VENDOR_CONTACT_LOC_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }
    }

}
