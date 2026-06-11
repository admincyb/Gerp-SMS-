using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Administration.Masters.TaxSettings;
using BusinessObject;
using GTIService.Constants.Common;

namespace DataAccess.Administration.Masters
{
    public class TaxSettingsMasterDL
    {
        /// <summary>
        /// Get Tax parameters 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTaxParameters()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXPARAMPK, DBNull.Value),                    
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.TAXPARAMETERSGET, colParameters).Tables[0];
            return dtParams;
        }

        /// <summary>
        /// SAVING MATERIAL DETAILS
        /// </summary>
        /// <param name="material"></param>
        /// <returns> INT</returns>
        public static string SaveTaxSettings(BusinessObject.Administration.Masters.TaxSettings taxSettings)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXPK,taxSettings.TAX_PK == 0 ? (object)DBNull.Value: taxSettings.TAX_PK) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_SUB_CATEGORY,taxSettings.TAX_SUB_CATEGORY == null ? (object)DBNull.Value: taxSettings.TAX_SUB_CATEGORY) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXHEAD, taxSettings.TAX_HEAD) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXCATEGORY, taxSettings.TAX_CATEGORY) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXFORMULA, taxSettings.TAX_Formula) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXFROMDT, taxSettings.TAX_FROM_DT) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXTODT, taxSettings.TAX_TO_DT) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXTYPE, taxSettings.TAX_TYPE) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXDISCFROM_DT, taxSettings.DISC_FROM==""?(object)DBNull.Value: taxSettings.DISC_FROM) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXEXTFROM_DT, taxSettings.EXT_FROM==""?(object)DBNull.Value: taxSettings.EXT_FROM) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXEXTTODATE, taxSettings.EXT_TO==""?(object)DBNull.Value: taxSettings.EXT_TO) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXACCOUNT, taxSettings.TAX_ACCOUNT==0 ?(object)DBNull.Value: taxSettings.TAX_ACCOUNT) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_DESC, taxSettings.TAX_DESC=="" ?(object)DBNull.Value: taxSettings.TAX_DESC) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.P_TAX_GST_GROUP, taxSettings.TAX_GST_GROUP=="0" ?(object)DBNull.Value: taxSettings.TAX_GST_GROUP) , //For GST group
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXMODBY, taxSettings.UserID) ,     
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXBIZUNIT, taxSettings.BIZUNIT) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXNOTDUE, string.IsNullOrEmpty(taxSettings.TAX_NOT_DUE)==true?0:1),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_DISP_NAME, taxSettings.TAX_DISP_NAME) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_IS_PURCHASE, string.IsNullOrEmpty(taxSettings.TAX_IS_PURCHASE)==true?0:1),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_IS_RETURN, string.IsNullOrEmpty(taxSettings.TAX_IS_RETURN)==true?0:1),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_IS_SALE, string.IsNullOrEmpty(taxSettings.TAX_IS_SALE)==true?0:1),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_IS_FOB_CAL, string.IsNullOrEmpty(taxSettings.TAX_IS_FOB_CAL)==true?0:1),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_CODE, taxSettings.TAX_CODE) ,
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_RATE, taxSettings.TAX_RATE=="" ?(object)DBNull.Value:taxSettings.TAX_RATE) ,

                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_ACTIVE, taxSettings.TAX_ACTIVE==""?"0":taxSettings.TAX_ACTIVE),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_AUTO_OTHER_ENABLE, string.IsNullOrEmpty(taxSettings.TAX_AUTO_OTHER_ENABLE)==true?0:1),

                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.SAVETAXSETTINGS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value).ToString();
        }


        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetTaxDetails(GridPrams grid, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL ,  grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS, grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy == null ? "TAX_PK" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "desc" : grid.SortDirection),
            };

            DataSet dtMaterial = new DataSet();
            dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.GETTAXSETTINGS, colParameters);
            return dtMaterial;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="isTax"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataSet GetTaxTypeDetails(GridPrams grid, int isTax, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL ,  grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS, grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_TAX_FLAG, isTax),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy == null ? "TAX_HEAD" :grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "ASC" : grid.SortDirection),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy == null ? "TAX_PK" : grid.SortBy),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "desc" : grid.SortDirection),

            };

            DataSet dtMaterial = new DataSet();
            dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.GETTAXSETTINGS, colParameters);
            return dtMaterial;


        }

        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int BizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHBY ,  searchBy),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHVALUE ,  searchValue),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT ,  BizUnit),

            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.GETSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }

        /// <summary>
        /// Methord used to get Active tax for the current day
        /// </summary>
        /// <createdby>Vineeth Babu</createdby>
        /// <for>Po Creation</for>
        /// <used in>Po creation assigning tax settings to controls </used in>
        /// <param name="bizUnitpk"></param>
        /// <returns> DatatTable</returns>
        public static DataTable GetActiveTax(int bizUnitPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPK)                    
            };

            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.GETACTIVETAX, colParameters).Tables[0];

        }

        /// <summary>
        /// Get Tax Category 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTaxCategory(int bizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, 1),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.CATEGORYTYPE, "CHARGE TYPE"),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPK)          
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.TAXCATEGORYGET, colParameters).Tables[0];
            return dtParams;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cfgType"></param>
        /// <param name="bizUnitPK"></param>
        /// <returns></returns>
        public static DataTable GetCfgValue(string cfgType, int bizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, 1),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.CATEGORYTYPE, cfgType),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPK)          
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.TAXCATEGORYGET, colParameters).Tables[0];
            return dtParams;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsTax"></param>
        /// <param name="bizUnitPK"></param>
        /// <returns></returns>
        public static DataTable GetTaxTypeCategory(int IsTax, int bizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, 1),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.P_TAX_FLAG, IsTax),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnitPK)          
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.SPFIN_TAX_TYPE_GET_KV, colParameters).Tables[0];
            return dtParams;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetActiveCategoryValue(int taxPK, int categoryPK, int bizUnit, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, active),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXPK, taxPK == 0 ? (object) DBNull.Value : taxPK ),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXCATEGORY, categoryPK == 0 ? (object) DBNull.Value : categoryPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)          
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.CATEGORYVALUEGET, colParameters).Tables[0];
            return dtParams;
        }
        /// <summary>
        /// Get Tax Category 
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="subCategoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetTaxCategoryValue(int categoryPK, int subCategoryPK, int bizUnit, int active, int isTaxSale = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, active),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXCATEGORY, categoryPK == 0 ? (object) DBNull.Value : categoryPK),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAX_SUB_CATEGORY, subCategoryPK == 0 ? (object) DBNull.Value : subCategoryPK ),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.P_TAX_IS_SALE, isTaxSale == 0 ? (object) DBNull.Value : isTaxSale ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)          
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.CATEGORYVALUEGET, colParameters).Tables[0];
            return dtParams;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetActiveCategoryDateValue(int categoryPK, int bizUnit, int active, DateTime taxDate, int subCategory, string specialCond = null, int taxDue = 0, int? purchaseTax = 0, int? isSalestax = 0,string CatXML=null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, active),
                //new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXPK, taxPK == 0 ? (object) DBNull.Value : taxPK ),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.P_TAX_CATEGORY, categoryPK == 0 ? (object) DBNull.Value : categoryPK),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.P_TAX_SUB_CATEGORY, subCategory == 0 ? (object) DBNull.Value : subCategory),
                //new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.P_CFG_SPL_COND, specialCond == null ? (object) DBNull.Value : specialCond),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXDATE,taxDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit > 0 ? bizUnit : (object) DBNull.Value ),
                 new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.P_TAX_NOT_DUE,taxDue),
                 new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.P_TAX_IS_PURCHASE, purchaseTax == 0 ? (object) DBNull.Value : purchaseTax),
                 new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.P_TAX_IS_SALE , isSalestax == 0 ? (object) DBNull.Value : isSalestax),
                 new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.P_CATXML , CatXML == null||CatXML==string.Empty ? (object) DBNull.Value : CatXML)
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.CATEGORYVALUEWITHDATEGET, colParameters).Tables[0];
            return dtParams;
        }


        //Vendor tax

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxPK"></param>
        /// <param name="categoryPK"></param>
        /// <param name="vendorPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetVendorActiveCategoryDateValue(int itemPK, int vendorPK, int active, DateTime taxDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, active),               
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.ITM_PK, itemPK == 0 ? (object) DBNull.Value : itemPK),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.VENDOR_PK, vendorPK == 0 ? (object) DBNull.Value : vendorPK ),                              
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXDATE,taxDate),               
                 
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.GETVENDOR_TAX_DTL_GET_KV, colParameters).Tables[0];
            return dtParams;
        }



        //end

        public static int DeleteTaxDetails(int taxPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXPK,  taxPK == 0 ? (object)DBNull.Value :  taxPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.DELETETAXDETAILS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }
    }
}
