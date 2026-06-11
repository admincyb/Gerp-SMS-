using System;
using System.Data;
using BusinessObject;

namespace DataAccess.MaterialManagement
{
    /// <summary>
    /// This class is used to communicate with data access layer.
    /// </summary>
   public  class MaterialMasterDL
   {
       #region methods

       /// <summary>
       /// SAVING Vendor DETAILS
       /// </summary>
       /// <param name="strxml"></param>
       /// <returns> INT</returns>
       public static int SaveMaterialVendorDetails(string strxml)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( GTIService.Constants.Material.Parameters.VENDORDETAILSXML  , strxml),
               
                new DBService.Parameters(GTIService.Constants.Material.Parameters.VENDORDETAILVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SAVEVENDORDETAILSXML, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters.VENDORDETAILVALUE]).Value);
           return result;

       }
        /// <summary>
        /// Save Order Item Details
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveOrderItemDetails(string strXml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_XML, strXml),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.VENDORDETAILVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
             };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_WO_ITEM_MATERIAL_MAP_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters.VENDORDETAILVALUE]).Value);
            return result;

        }

        /// <summary>
        /// Get Vendor Details By item Id as A Xml Format
        /// </summary>
        /// <param name="itemPk"></param>
        /// <returns>Xml Formatted Vendor Details </returns>
        public static string GetVendorMappingDetails(int itemPk)
       {

           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           DataSet dsMapping;
           string result;
           result = string.Empty;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALREQPK ,  itemPk),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALRETURNVALUE, 0,4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
           dsMapping = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETVENDORMAPPINGDETAILSBYITEMPKXML, colParameters);
           if (dsMapping != null && dsMapping.Tables.Count > 0)
           {
               foreach (DataRow dr in dsMapping.Tables[0].Rows)
               {
                   result += dr[0].ToString();
               }
           }
           return result;
       }
       /// <summary>
       /// SAVING MATERIAL DETAILS
       /// </summary>
       /// <param name="material"></param>
       /// <returns> INT</returns>
       public static string SaveMaterial(BusinessObject.MaterialManagement.Material material, int departementPK,string storeXml, string prdXml="")
       {

           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;

           colParameters = new DBService.Parameters[] 
            { 
              
                //Packing section Start
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_WEIGHT , material.ITM_WEIGHT==null ? 0 : material.ITM_WEIGHT),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_MOQ , material.ITM_MOQ==null ? 0 : material.ITM_MOQ),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_MAX_OQ , material.ITM_MAX_OQ==null ? 0 : material.ITM_MAX_OQ),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_PK , material.IPD_PK),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_ITEM , material.IPD_ITEM),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_TYPE , material.IPD_TYPE),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_CLASSIFICATION , material.IPD_CLASSIFICATION==0?(object)DBNull.Value: material.IPD_CLASSIFICATION),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_INNER_LENGTH , material.IPD_INNER_LENGTH),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_INNER_BREADTH , material.IPD_INNER_BREADTH),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_INNER_HEIGHT , material.IPD_INNER_HEIGHT),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_OUTER_LENGTH , material.IPD_OUTER_LENGTH),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_OUTER_BREADTH , material.IPD_OUTER_BREADTH),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_OUTER_HEIGHT , material.IPD_OUTER_HEIGHT),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_PLY , material.IPD_PLY),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_PAPER_COLOR , material.IPD_PAPER_COLOR),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_ART_WORK , material.IPD_ART_WORK),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_CUSTOMER , material.IPD_CUSTOMER == 0 ? (object)DBNull.Value: material.IPD_CUSTOMER) ,
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_ACTIVE , material.IPD_ACTIVE),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_DOC_PK , material.DOC_PK  == 0 ? (object)DBNull.Value: material.DOC_PK) ,
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_DOC_SEQ_NO , material.DOC_SEQ_NO??(object)DBNull.Value),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_DOC_TITLE , material.DOC_TITLE),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_DOC_NAME , material.DOC_NAME),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_DOC_TYPE , material.DOC_TYPE),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_DOC_PATH , material.DOC_PATH),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_SET , material.ITM_SET),
                //end
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALPK, material.MaterialDetailId == 0 ? (object)DBNull.Value: material.MaterialDetailId) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALNAME , material.ITM_NAME.Trim()) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALCATEGORY, material.ITC_PK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALDESC, material.ITM_DESC) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALMODIFIED, material.UserID == 0 ? material.UserPk: material.UserID) ,
                //new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALRETURNTYPE, material.ITM_TYPE_TEXT) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALSTATUS, material.STATUS) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALUOM, material.UOM_PK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_UOM_PURCHASE, material.ITM_UOM_PURCHASE) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_UOM_SALE, material.ITM_UOM_SALE) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALMINLEVEL, material.ITM_MIN_STK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALMAXLEVEL, material.ITM_MAX_STK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALREORDERLEVEL, material.ITM_ROL_STK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_PHR, material.ITM_PHR==null?(object)DBNull.Value:material.ITM_PHR) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_TSC, material.ITM_TSC==null?(object)DBNull.Value:material.ITM_TSC) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_BATCH_CODE, material.ITM_BATCH_CODE==null?(object)DBNull.Value:material.ITM_BATCH_CODE),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALCODE, material.ITM_CODE) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALCREATEDBY, material.UserID == 0 ? material.UserPk: material.UserID) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.BIZUNIT, material.SBU== 0 ? material.BizUnitPk: material.SBU) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.DPTPK, departementPK== 0 ? (object)DBNull.Value: departementPK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_XML, storeXml == string.Empty  ? (object)DBNull.Value: storeXml) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_THICKNESS , material.IPD_THICKNESS),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_IPD_PAPER_TYPE , material.IPD_PAPER_TYPE),   
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ISD_SIZE , material.P_ISD_SIZE) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_OST_QTY_OPENING, material.P_OST_QTY_OPENING) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_GROUP , material.ITM_GROUP) ,
                //new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_TYPE , material.ITM_TYPE) , //passed ITM_TYPE_TEXT instead of ITM_TYPE (Bug:36532)
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_TYPE , material.ITM_TYPE_TEXT==null?(object)DBNull.Value:material.ITM_TYPE_TEXT) , //Material master Eligibility not saved (Bug ID:41103)
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_XML_PRD, prdXml == string.Empty  ? (object)DBNull.Value: prdXml) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_LAST_MOD_DT, material.P_LAST_MOD_DT) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_NEED_QC_INSP, material.ITM_NEED_QC_INSP) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_IS_WORK_ORDER, material.ITM_IS_WORK_ORDER) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_NEED_BATCH_STK, material.ITM_NEED_BATCH_STK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_IS_LINKED_ITEM, material.ITM_IS_LINKED_ITEM) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_INACTIVE_PERIOD , material.InactivePeriod),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_GST_CLASS , material.ITM_GST_CLASS>0?material.ITM_GST_CLASS:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_IS_CONVERSION_REQD, material.ITM_IS_CONVERSION_REQD) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_IS_ASSET, material.ITM_IS_ASSET) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALRETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
           
           dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SAVEMATERAILDETAILS, colParameters);
           return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALRETURNVALUE]).Value).ToString();
       }
       /// <summary>
       /// Save Vendor Material 
       /// </summary>
       /// <param name="material"></param>
       /// <param name="venPK"></param>
       /// <returns></returns>
       public static string SaveVenMaterial(BusinessObject.MaterialManagement.VenMaterial material,User objUser)
       {

           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;

           colParameters = new DBService.Parameters[] 
            { 
              
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITV_PK , material.P_ITV_PK),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITV_ITEM , material.P_ITV_ITEM),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITV_VENDOR , material.P_ITV_VENDOR),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITV_NAME , material.P_ITV_NAME),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITV_PRICE , material.P_ITV_PRICE),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITV_CURRENCY , material.P_ITV_CURRENCY),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITV_MOQ , material.P_ITV_MOQ),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITV_MOQ_UOM , material.P_ITV_MOQ_UOM),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITV_LEAD_TIME , material.P_ITV_LEAD_TIME),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ACTIVE , material.P_ACTIVE),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_USER_PK ,objUser.PKUser),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_BIZUNIT , objUser.SBUID),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALRETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };

           dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_ITEM_VENDOR_SAVE, colParameters);
           return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALRETURNVALUE]).Value).ToString();
       }

       /// <summary>
       /// Methord to get the Search Vlaues Corresponding to Search Type
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
       /// <param name="sbuPk"></param>
       /// <returns>DataTable</returns>
       public static DataTable GetSearchValues(string searchBy, string searchValue, int sbuPk)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALSEARCHBY ,  searchBy),
              new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALSEARCHVALUE ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk) 
            };
           DataTable dtSearchValue = new DataTable();
           dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETSEARCHVALUE, colParameters).Tables[0];
           return dtSearchValue;

       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
       /// <param name="sbuPk"></param>
       /// <returns></returns>
       public static DataTable GetSearchTypeValues(string searchBy, string searchValue,int type, int sbuPk)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALSEARCHBY ,  searchBy),
               new DBService.Parameters( GTIService.Constants.Material.Parameters.P_ITM_SET ,  type),
              new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALSEARCHVALUE ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk) 
            };
           DataTable dtSearchValue = new DataTable();
           dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETSEARCHVALUE, colParameters).Tables[0];
           return dtSearchValue;

       }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
       /// <param name="sbuPk"></param>
       /// <returns></returns>
       public static DataTable GetMaterialCodeNameByCategoryAuto(string searchBy, string searchValue, int type, int sbuPk, int catId, int IsWorkOrder=0, int BrandPK = 0)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALSEARCHBY ,  searchBy),
              new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALSEARCHVALUE ,  searchValue),
              new DBService.Parameters( GTIService.Constants.Material.Parameters.P_ITM_SET ,  type > 0 ? type : (object)DBNull.Value),     
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk) ,
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_CAT,  catId > 0 ? catId : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_IS_WORK_ORDER,  IsWorkOrder > 0 ? IsWorkOrder : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_CIM_PK,  BrandPK > 0 ? BrandPK : (object)DBNull.Value)
            };
           DataTable dtSearchValue = new DataTable();
           dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_ITEM_MST_GET_AUTO, colParameters).Tables[0];
           return dtSearchValue;

       }
       /// <summary>
       /// Methord to get the Search Vlaues Corresponding to Search Type purchase request.
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
       /// <param name="categoryPk"></param>
       /// <returns>DataTable</returns>
       public static DataTable GetMaterialNameSearchValues(string searchValue, int categoryPk)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALSEARCHVALUE ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALCATEGORY,  categoryPk) 
            };
           DataTable dtSearchValue = new DataTable();
           dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALNAMESEARCHVALUE, colParameters).Tables[0];
           return dtSearchValue;

       }

       /// <summary>
       /// Methord to get the Search Vlaues Corresponding to Search Type purchase request.
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
       /// <param name="categoryPk"></param>
       /// <returns>DataTable</returns>
       public static DataTable GetMaterialSearchValueByCategoryAndStore(string searchValue, int categoryPk, int store, int active = 0, int Alternate = 0, int ItemPK = 0, int StockExist = 1)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALSEARCHVALUE ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALCATEGORY,  categoryPk > 0 ? categoryPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_ACTIVE,  active > 0 ? active : (object)DBNull.Value), //P_ITM_ACTIVE is not a parameter in SP
              new DBService.Parameters(GTIService.Constants.Material.Parameters.ITEMDEPARTMENT ,  store > 0 ? store : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_INCLUDE_ALT ,  Alternate > 0 ? Alternate : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ALT_ITM ,  ItemPK > 0 ? ItemPK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_STOCK_ITM ,  StockExist)
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETSTOREMATERIALNAMESEARCHVALUE, colParameters).Tables[0];
        }
        public static DataTable GetMaterialsPlantToPlant(string searchValue, int categoryPk, int store, int active = 0, int Alternate = 0, int ItemPK = 0, int StockExist = 1)
        {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALSEARCHVALUE ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALCATEGORY,  categoryPk > 0 ? categoryPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_ACTIVE,  active > 0 ? active : (object)DBNull.Value), //P_ITM_ACTIVE is not a parameter in SP
              new DBService.Parameters(GTIService.Constants.Material.Parameters.ITEMDEPARTMENT ,  store > 0 ? store : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_INCLUDE_ALT ,  Alternate > 0 ? Alternate : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ALT_ITM ,  ItemPK > 0 ? ItemPK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_STOCK_ITM ,  StockExist)
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_ITEM_DEPT_STOCK_WISE_GET, colParameters).Tables[0];
        }

        public static DataTable GetAccountTypeAuto(string SubType , int AccountPK, int Active, int IsGroup, int BizUnitPK, string srchValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_COA_SUB_TYPE, SubType),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_COA_PK,  AccountPK > 0 ? AccountPK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ACTIVE,  Active > 0 ? Active : (object)DBNull.Value), 
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_COA_IS_GROUP ,  IsGroup > 0 ? IsGroup : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.P_BIZUNIT_PK, BizUnitPK > 0 ? BizUnitPK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.SERACHVALUE, srchValue)
             };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETSPFINCOAMSTAUTOGET, colParameters).Tables[0];
        }
        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type purchase request.
        /// Show materials based on Qty available (not based on Active)
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetMaterialSearchValueByCategoryAndStoreStk(string searchValue, int categoryPk, int store)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALSEARCHVALUE ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALCATEGORY,  categoryPk > 0 ? categoryPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.ITEMDEPARTMENT ,  store > 0 ? store : (object)DBNull.Value) 
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETSTOREMATERIALNAMESEARCHVALUESTK, colParameters).Tables[0];
       }

       /// <summary>
       /// Function Used To Get all Material UMO
       /// </summary>
       /// <summary>
       /// This Function Used To Get all Material UOM For Dispersion Master
       /// </summary>
       /// <param name="materialPK"></param>
       /// <param name="status"></param>
       /// <returns>DataTable</returns>
       public static DataTable GetMaterialUMODtls(int materialPK, int status)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALPK  , materialPK)  
                
            };
           if (status == 0)
               return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALUMO, colParameters).Tables[0];
           else if (status == 1)
               return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALUMO, colParameters).Tables[1];
           else
               return null;
       }

       /// <summary>
       /// Function Used To Get all Material UMO
       /// </summary>
       /// <summary>
       /// This Function Used To Get all Material UOM For Dispersion Master
       /// </summary>
       /// <param name="materialPK"></param>
       /// <param name="status"></param>
       /// <returns>DataTable</returns>
       public static DataTable GetUOMConvExistsByMaterial(int materialPK)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALPK  , materialPK)  
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALUMOCONV, colParameters).Tables[0];
       }

       /// <summary>
       /// Function Used To Get all material Name and material pk
       /// </summary>
       /// <summary>
       /// This Function Used To Get material Name and material pk For Dispersion Master
       /// </summary>
       /// <returns>DataTable</returns>
       public static DataTable GetMaterialName()
       {
           DBService dbService = new DBService();
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALLIST).Tables[0];
       }
       /// <summary>
       /// method for search based on the Criteria
       /// </summary>
       /// <param name="GridPrams"></param>
       /// <param name="bizUnit"></param>
       /// <returns>DataSet</returns>
       public static DataSet GetMaterialList(GridPrams grid, int bizUnit)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALLSTSTATUS , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALLSTSRCH ,  grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection),
            };

           DataSet dtMaterial = new DataSet();
           dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALLISTBYSRH, colParameters);
           return dtMaterial;


       }
       /// <summary>
       /// Material Category, Level base
       /// </summary>
       /// <param name="sbuPk"></param>
       /// <returns></returns>
       public static DataTable GetMaterialCategoryByLevel(int sbuPk, int level)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters("P_BIZUNIT",sbuPk),
              new DBService.Parameters("P_ITC_LEVEL",level), 
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_ITEM_CATEGORY_TREE_LIST", colParameters).Tables[0];
       }

        public static DataSet GetWOMaterialList(GridPrams grid,int bizUnit, int? active = 1, int ITMCAT = 0, int isWorkOrder = 0, int WOType=1,int isMapped=0,int subType=0,int customer=0,int brand=0, int PackingCustomer = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
              new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALLSTSTATUS , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALLSTSRCH ,  grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%",DataAccess.DBService.ParameterType.NVarChar),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_WO_ITEM_TYPE, WOType),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_WO_ITEM_IS_MAPPED, isMapped),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PROD_SUB_TYPE, subType>0 ? subType : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CUS_PK,  customer>0 ? customer : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_CIM_PK,  brand>0 ? brand : (object)DBNull.Value),

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE ,active != null && active.ToString() != string.Empty && active >= 0 ? active:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_CAT , ITMCAT == 0 ? (object)DBNull.Value : ITMCAT),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_IS_WORK_ORDER , isWorkOrder == 0 ? (object)DBNull.Value : isWorkOrder),
            new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BOM_CUS_PK , PackingCustomer == 0 ? (object)DBNull.Value : PackingCustomer)
             };

            DataSet dtMaterial = new DataSet();
            dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETWOMATERIALLISTBYSRH, colParameters);
            return dtMaterial;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="type"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataSet GetMaterialTypeList(GridPrams grid,int type, int bizUnit, int? active = 1,int ITMCAT=0, int isWorkOrder=0)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALLSTSTATUS , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALLSTSRCH ,  grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%",DataAccess.DBService.ParameterType.NVarChar),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_SET,  type),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE ,active != null && active.ToString() != string.Empty && active >= 0 ? active:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_CAT , ITMCAT == 0 ? (object)DBNull.Value : ITMCAT),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_IS_WORK_ORDER , isWorkOrder == 0 ? (object)DBNull.Value : isWorkOrder),
               

            };

           DataSet dtMaterial = new DataSet();
           dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALLISTBYSRH, colParameters);
           return dtMaterial;


       }       
       /// <summary>
       /// 
       /// </summary>
       /// <param name="itemPK"></param>
       /// <param name="bizUnit"></param>
       /// <returns></returns>
       public static DataTable GetMaterialStores(int itemPK, int bizUnit)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {        
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ACTIVE , 1),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_PK , itemPK),

            };
           return dbService.DataAdapter(CommandType.StoredProcedure,  GTIService.Constants.Material.Procedures.SPINV_ITEM_DEPT_GET_TREE, colParameters).Tables[0];
       }
       /// <summary>
       /// Delete material Details By materialID
       /// </summary>
       /// <param name="materialID"></param>
       /// <returns>int- 1(Success)</returns>
       public static int DeleteMaterialDtls(int materialID)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;

           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALREQPK,  materialID == 0 ? (object)DBNull.Value :  materialID),
                new DBService.Parameters(GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALRETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

           dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.DELETEMATERIALDETAILS, colParameters);

           return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALRETURNVALUE]).Value);



       }
      
       //280311
       /// <summary>
       /// Get MaterialName By materialID
       /// </summary>
       /// <param name="materialID"></param>
       /// <returns>int- 1(Success)</returns>
       public static string GetItemName(int materialID)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;

           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALREQPK,  materialID == 0 ? (object)DBNull.Value :  materialID),
                new DBService.Parameters(GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALRETURNVALUE,string.Empty, 400,ParameterDirection.Output, DBService.ParameterType.VarChar)

            };
          
           dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETITEMNAME, colParameters);

           return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALRETURNVALUE]).Value);

       }

       /// <summary>
       /// Get MaterialName By material category
       /// </summary>
       /// <param name="categoryID"></param>
       /// <param name="itemID"></param>
       /// <param name="sbuPk"></param>
       /// <returns>int- 1(Success)</returns>
       public static DataTable GetMaterialByCategory(int categoryID, int itemID, int sbuPk,string searchValue="")
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMCATEGORYID , categoryID==0?(object)DBNull.Value:categoryID) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMPK ,itemID ==0?(object)DBNull.Value:itemID) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.ITM_NAME, searchValue=="" ?(object)DBNull.Value: searchValue)
                
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALBYCATEGORY, colParameters).Tables[0];

       }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="itemID"></param>
       /// <param name="sbuPk"></param>
       /// <returns></returns>
       public static DataTable GetRelatedMaterial(int itemID, int sbuPk)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMPK ,itemID ==0?(object)DBNull.Value:itemID) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETRELATEDMATERIAL, colParameters).Tables[0];

       }
        public static DataTable GetBOMaterial(int itemID, int sbuPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMPK ,itemID ==0?(object)DBNull.Value:itemID) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
             };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_BOM_ITEM_MST_GET_KV, colParameters).Tables[0];

        }
        

       /// <summary>
       /// Get MaterialName((Code)Name) By material category
       /// </summary>
       /// <param name="categoryID"></param>
       /// <param name="itemID"></param>
       /// <param name="sbuPk"></param>
       /// <returns>int- 1(Success)</returns>
       public static DataTable GetMaterialCodeNameByCategory(int categoryID, int itemID, int sbuPk, string searchValue = "")
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMCATEGORYID , categoryID==0?(object)DBNull.Value:categoryID) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMPK ,itemID ==0?(object)DBNull.Value:itemID) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.ITM_NAME, searchValue=="" ?(object)DBNull.Value: searchValue)
                
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALBYCATEGORY, colParameters).Tables[0];

       }
       /// <summary>
       /// Get MaterialName By material category
       /// </summary>
       /// <param name="categoryID"></param>
       /// <param name="itemID"></param>
       /// <param name="sbuPk"></param>
       /// <returns>int- 1(Success)</returns>
       public static DataTable GetMaterialByCategoryAndStore(int categoryID, int itemID, int sbuPk, int type, int userPK, int store,int stock,int active,string searchValue="", int IsStoreRequest = 0)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMCATEGORYID , categoryID==0?(object)DBNull.Value:categoryID) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMPK , itemID==0?(object)DBNull.Value:itemID) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.TYPE ,type ==0?(object)DBNull.Value:type) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.STORE ,store ==0?(object)DBNull.Value:store) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.IS_STOCK ,stock ==0?(object)DBNull.Value:stock) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMSTATUS ,active ==1?(object)DBNull.Value:1) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  userPK),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.NAMESEARCH ,searchValue ==""?(object)DBNull.Value:searchValue) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IS_SR,  IsStoreRequest == 0 ? (object)DBNull.Value : IsStoreRequest)

            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALBYCATEGORYANDSTORE, colParameters).Tables[0];

       }

       public static DataTable GetMaterialByCategoryAndStoreAuto(int categoryID, int itemID, int sbuPk, int type, int userPK, int store,string searchValue)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMCATEGORYID , categoryID==0?(object)DBNull.Value:categoryID) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMPK , itemID==0?(object)DBNull.Value:itemID) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.TYPE ,type ==0?(object)DBNull.Value:type) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.STORE ,store ==0?(object)DBNull.Value:store) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  userPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.ITM_NAME,  searchValue)
                
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALBYCATEGORYANDSTOREAUTO, colParameters).Tables[0];

       }

       /// <summary>
       /// Get UOM Details By Material PK
       /// </summary>
       /// <param name="materialPK"></param>
       /// <returns></returns>
       public static DataTable GetUomDtlsByMaterialPk(int materialPK)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
             {   
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITEMPK,  materialPK==0 ?(object)DBNull.Value:materialPK),
             };
           DataTable dtUomDtls = new DataTable();
           dtUomDtls = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETITEMUOM, colParameters).Tables[0];
           return dtUomDtls;
       }

       //02042011

       /// <summary>
       /// Get MaterialDetails by matrial id
       /// </summary>
       /// <param name="itemID"></param>
       /// <param name="sbuPk"></param>
       /// <returns>int- 1(Success)</returns>
       public static DataTable GetMaterialDetails(int itemID, int sbuPk, int dept = 0, int active = 0, int vendorPK = 0)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMPK ,itemID ==0?(object)DBNull.Value:itemID) , 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DEPT,dept != null && dept.ToString() != string.Empty && dept > 0 ? dept:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_ACTIVE,active != null && active.ToString() != string.Empty && active > 0 ? active:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_VEN_PK, vendorPK > 0 ? vendorPK:(object)DBNull.Value)
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALBYCATEGORY, colParameters).Tables[0];
       }

       /// <summary>
      //03OCT2019
       /// </summary>
       /// <param name="itemID"></param>
       /// <param name="sbuPk"></param>
       /// <param name="dept"></param>
       /// <param name="active"></param>
       /// <returns></returns>

       public static DataTable GetMaterialRateDetails(int itemID, DateTime Date, int CusPk, int Currencyid)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_PK ,itemID ==0?(object)DBNull.Value:itemID) , 
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_DATE ,Date) , 
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_CUS_PK ,CusPk ==0?(object)DBNull.Value:CusPk) , 
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_CURRENCY ,Currencyid ==0?(object)DBNull.Value:Currencyid)  
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPSAL_BRAND_PRODUCT_RATE_GET, colParameters).Tables[0];
       }


       /// <summary>
       /// 
       /// </summary>
       /// <param name="itemID"></param>
       /// <returns></returns>
       public static DataTable GetPakingMaterialDetails(int itemID)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_ITM_PK ,itemID ==0?(object)DBNull.Value:itemID)
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_ITEM_PACK_DOC_DTL_GET, colParameters).Tables[0];
       }

       /// <summary>
       /// Function Used To Get Material Description
       /// </summary>
       /// <param name="materialID"></param>
       /// <param name="sbuPk"></param>
       /// <returns></returns>
       public static DataTable GetMaterialDescription(int materialID, int departmentID, int toUOM = 0)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITEMPK ,materialID == 0 ? (object)DBNull.Value : materialID),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.DPT_PK ,departmentID == 0 ? (object)DBNull.Value : departmentID),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.TO_UOM ,toUOM == 0 ? (object)DBNull.Value : toUOM)
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALDESCRIPTION, colParameters).Tables[0];
       }

       /// <summary>
       /// Get MaterialDetails by matrial id
       /// </summary>
       /// <param name="itemID"></param>
       /// <param name="sbuPk"></param>
       /// <returns>int- 1(Success)</returns>
       public static DataTable GetMaterialDetailsForStore(int itemID, int sbuPk)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMPK ,itemID ==0?(object)DBNull.Value:itemID) , 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.ITM_ACTIVE,  2)
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALBYCODE, colParameters).Tables[0];

       }
       /// <summary>
       /// Get current stock for store by matrial id
       /// </summary>
       /// <param name="itemID"></param>
       /// <param name="store"></param>
       /// <returns></returns>
       public static DataTable GetCurrentStockForStore(int itemID, int store, DateTime? date = null, int toUOM = 0)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMPK ,itemID ==0?(object)DBNull.Value:itemID) , 
                new DBService.Parameters(GTIService.Constants.Material.Parameters.STORE,  store),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_STD_MOD_DT,  date),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.TO_UOM ,toUOM ==0?(object)DBNull.Value:toUOM),
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETCURRENTSTOCK, colParameters).Tables[0];

       }

       /// <summary>
       /// Get current stock for store by matrial id
       /// </summary>
       /// <param name="itemID"></param>
       /// <param name="store"></param>
       /// <returns></returns>
       public static DataTable GetBreakupForStoreCurrStock(int itemID, int deptType, int deptCat, DateTime? date,int toUOM=0)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITEMPK ,itemID ==0?(object)DBNull.Value:itemID),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_DPT_TYPE ,deptType ==0?(object)DBNull.Value:deptType),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.TO_UOM ,toUOM ==0?(object)DBNull.Value:toUOM),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_DPT_CATEGORY ,deptCat),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_STD_MOD_DT ,date.HasValue?date:(object)DBNull.Value)
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETCURRENTSTOCK, colParameters).Tables[0];

       }


       /// <summary>
       /// Get MaterialDetails by matrial id
       /// </summary>
       /// <param name="itemID"></param>
       /// <param name="sbuPk"></param>
       /// <returns>int- 1(Success)</returns>
       public static DataTable GetInActiveMaterialDetails(int itemID, int sbuPk, int status)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITEMPK ,itemID) , 
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITEMSTATUS , status) , 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, sbuPk)
                
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALBYCATEGORY, colParameters).Tables[0];

       }
       
       /// <summary>
       /// This Function Used To Get all Material UOM Conversion Factor by passing the material and New UOM
       /// </summary>
       /// <Createdby>Vineeth Babu</Createdby>
       /// <for>Po Creation</for>
       /// <Used In>Finding uom Conversion when adding material</Used>
       /// <param name="materialPK"></param>
       /// <param name="uom"></param>
       /// <returns>DataTable</returns>
       public static DataTable GetMaterialUOMConversion(int materialPK, int uom)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALPK  , materialPK),  
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALUOM  , uom) , 
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALUOMCONVERSION, colParameters).Tables[0];
           
       }


       /// <summary>
       /// Get Material Corresponding to a department(Inventory)
       /// </summary>
       /// <param name="bizUnit"></param>
       /// <returns></returns>
       public static DataTable GetDepartmentMaterials(int storeID, string searchVal)
       {
           DataTable dtShift = new DataTable();
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITEMDEPARTMENT, storeID == 0 ? (object)DBNull.Value : storeID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchVal == "" ? "%": searchVal)           
            };
           dtShift = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETSTOREMATERIALS, colParameters).Tables[0];
           return dtShift;
       }

       /// <summary>
       /// Get Material Corresponding to a department(Inventory)
       /// </summary>
       /// <param name="bizUnit"></param>
       /// <returns></returns>
       public static DataTable GetDepartmentCategoryMaterial(int storePK, int categoryPK, string searchVal, int bizUnit)
       {
           DataTable dtItems = new DataTable();
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITEMDEPARTMENT, storePK),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITEMCATEGORY, categoryPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchVal == "" ? "%": searchVal) ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
            };
           dtItems = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETSTORECATEGORYMATERIALS, colParameters).Tables[0];
           return dtItems;
       }


       //NewMaterial Start

       public static DataTable GetRateHistory(int itemPK, int vendorPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_PK, itemPK),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_VEN_PK, vendorPK)
                 
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETRATEHISTORY, colParameters).Tables[0];
            //string retStr = "";
            //for (int i = 0; i < dtxml.Rows.Count; i++)
            //    retStr += dtxml.Rows[i][0].ToString();
            return dtxml;
            
        }

        /// <summary>
        /// Get Order Item
        /// </summary>
        /// <param name="itemPK"></param>
        /// <returns></returns>
        public static string GetOrderItem(int itemPK, int itemType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_PK, itemPK),
                 new DBService.Parameters(GTIService.Constants.Material.Parameters.P_WIM_ITEM_TYPE, itemType),
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GET_ORDER_ITEM, colParameters).Tables[0];
            string retStr = "";
            for (int i = 0; i < dtxml.Rows.Count; i++)
                retStr += dtxml.Rows[i][0].ToString();
            return retStr;

        }
        //Ne End

        /// <summary>
        /// Get Order Item
        /// </summary>
        /// <param name="itemPK"></param>
        /// <returns></returns>
        public static string GetOrderItemByTypeItemQty(int itemTypePK, int itemPK, decimal qty, int operationPK, int customerPK = 0, int brandPK = 0, int subContractorPK = 0, int WOPK = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_PK, itemPK > 0 ? itemPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITEM_TYPE, itemTypePK > 0 ? itemTypePK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_QTY, qty > 0 ? qty : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_OPERATION, operationPK > 0 ? operationPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_WOM_CUSTOMER, customerPK > 0 ? customerPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_WOM_BRAND, brandPK > 0 ? brandPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_VENDOR, subContractorPK > 0 ? subContractorPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_WIH_PK, WOPK > 0 ? WOPK : (object)DBNull.Value)
            };
            DataTable dtXml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_WO_ITEM_MATERIAL_MAP_GET_LIST, colParameters).Tables[0];
            string retStr = "";
            for (int i = 0; i < dtXml.Rows.Count; i++)
                retStr += dtXml.Rows[i][0].ToString();
            return retStr;
        }

        /// <summary>
        /// Get Item Rates
        /// </summary>
        /// <param name="itemPK"></param>
        /// <param name="vendorPK"></param>
        /// <returns></returns>
        public static DataTable GetItemRates(int itemPK, int vendorPK, int toUOMPK=0)
       {
           DBService dbService = new DBService();

           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_ITM_PK, itemPK > 0 ? itemPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_VEN_PK,  vendorPK > 0 ? vendorPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_TO_UOM,  toUOMPK > 0 ? toUOMPK : (object)DBNull.Value)
            };
           DataTable dtItemRates = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GET_ITEM_RATES, colParameters).Tables[0];
           return dtItemRates;
       }
       #endregion

       /// <summary>
       /// Get Batch No
       /// </summary>
       /// <param name="itemPK"></param>
       /// <param name="vendorPK"></param>
       /// <returns></returns>
       public static DataTable GetBatchNo(int itemPK, int deptPK, int batchPK, DateTime? date = null, int? IsShowZeroQtyBatches = 0, DateTime? transDate = null, int TestResult=0)
       {
           DBService dbService = new DBService();

           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_ITM_PK, itemPK > 0 ? itemPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_DEPT,  deptPK > 0 ? deptPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_SBD_PK,  batchPK > 0 ? batchPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_TEST_RESULT,  TestResult > 0 ? TestResult : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_SBD_MOD_DT,  date),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_IS_FULL_QTY,  IsShowZeroQtyBatches),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_BATCH_DATE,  transDate),
            };
           DataTable dtItemRates = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GET_BATCH_NO, colParameters).Tables[0];
           return dtItemRates;
       }
       /// <summary>
       /// Get Batch No
       /// </summary>
       /// <param name="itemPK"></param>
       /// <param name="vendorPK"></param>
       /// <returns></returns>
       public static DataTable GetBatchNo_Consumption(int itemPK, int deptPK, int batchPK, DateTime? date = null, int? IsShowZeroQtyBatches = 0, DateTime? transDate = null, int? cdhPK = 0)
       {
           DBService dbService = new DBService();

           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_ITM_PK, itemPK > 0 ? itemPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_DEPT,  deptPK > 0 ? deptPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_SBD_PK,  batchPK > 0 ? batchPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_SBD_MOD_DT,  date),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_IS_FULL_QTY,  IsShowZeroQtyBatches),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_BATCH_DATE,  transDate),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_CDH_PK,  cdhPK > 0 ? cdhPK : (object)DBNull.Value),
            };
           DataTable dtBatches = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_STK_BATCH_CONS_GET_KV, colParameters).Tables[0];
           return dtBatches;
       }
       /// <summary>
       /// Get Batch Details
       /// </summary>
       /// <param name="itemPK"></param>
       /// <param name="vendorPK"></param>
       /// <returns></returns>
       public static DataTable GetBatchDetails(int batchPK, int GrnBatchConfg = 0)
       {
           DBService dbService = new DBService();

           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_SBD_PK, batchPK > 0 ? batchPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_HAS_GRN_IMPORT, GrnBatchConfg  > 0 ? GrnBatchConfg : (object)DBNull.Value),
               // new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

           DataTable dtBatchDetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GET_BATCHDETAILS, colParameters).Tables[0];
          // int Result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.P_RETR_VAL]).Value);
           return dtBatchDetails;
       }

       /// <summary>
       /// Get Batch Details For Dispersion
       /// </summary>
       /// <param name="itemPK"></param>
       /// <param name="vendorPK"></param>
       /// <returns></returns>
       public static DataTable GetBatchDetailsDispersion(int batchPK,int active, int bizunit)
       {
           DBService dbService = new DBService();

           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_DTH_PK, batchPK > 0 ? batchPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_BIZUNIT,bizunit)
            };
           DataTable dtBatchDetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETCURRENTSTOCKDISP, colParameters).Tables[0];
           return dtBatchDetails;
       }

       /// <summary>
       ///Check ItemCode is Exist or not
       /// </summary>
       /// <param name="itemPK"></param>
       /// <param name="vendorPK"></param>
       /// <returns></returns>
       public static DataTable CheckItemCodeExist(int ItemCode)
       {
           DBService dbService = new DBService();

           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_CODE, ItemCode > 0 ? ItemCode : (object)DBNull.Value) 
            };
           DataTable dtItemDetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.IS_ITEM_CODE_EXIST, colParameters).Tables[0];
           return dtItemDetails;
       }


       /// <summary>
       /// Get GST Classification for ddl
       /// </summary>
       /// <param name="Pk"></param>
       /// <param name="sbuPk"></param>
       /// <returns>int- 1(Success)</returns>
       public static DataTable GetGSTClassificationList(int Pk, int sbuPk, int status)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_GCM_PK ,Pk > 0 ? Pk : (object)DBNull.Value) , 
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ACTIVE , status) , 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, sbuPk)
                
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETGSTCLASSIFICATION, colParameters).Tables[0];

       }

       /// <summary>
       /// SAVING MATERIAL DETAILS From PR
       /// </summary>
       /// <param name="material"></param>
       /// <returns> INT</returns>
       public static string SaveMaterialFromPR(BusinessObject.MaterialManagement.MaterialBO material, int departementPK, string storeXml, string prdXml = "")
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_SET , material.ITM_SET),              
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALPK, material.ITM_PK == 0 ? (object)DBNull.Value: material.ITM_PK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALCODE, material.ItemCode) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALNAME , material.ItemName.Trim()) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALCATEGORY, material.ITC_PK) , 
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALUOM, material.UOM_PK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_UOM_PURCHASE, material.ITM_UOM_PURCHASE) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_UOM_SALE, material.ITM_UOM_SALE) ,  
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALMINLEVEL, material.ITM_MIN_STK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALMAXLEVEL, material.ITM_MAX_STK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALREORDERLEVEL, material.ITM_ROL_STK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_ITM_NEED_BATCH_STK, material.ITM_NEED_BATCH_STK) ,  
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_TYPE , material.ITM_TYPE) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.ITM_MOQ , material.ITM_MOQ==null ? 0 : material.ITM_MOQ),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALCREATEDBY, material.UserPk) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALMODIFIED, material.UserPk) ,              
                new DBService.Parameters(GTIService.Constants.Material.Parameters.MATERIALSTATUS, material.ITM_ACTIVE) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.BIZUNIT, material.SBU== 0 ? material.BizUnitPk: material.SBU) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.DPTPK, departementPK== 0 ? (object)DBNull.Value: departementPK) ,
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_XML, storeXml == string.Empty  ? (object)DBNull.Value: storeXml) ,                
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_LAST_MOD_DT, material.P_LAST_MOD_DT) ,             
              
                new DBService.Parameters(GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALRETURNVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };

           dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SAVEMATERAILDETAILS, colParameters);
           return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters_RequisitionSlip.MATERIALRETURNVALUE]).Value).ToString();
       }

       #region PR Trading
       /// <summary>
       /// Function Used To Get all Material UMO
       /// </summary>
       /// <summary>
       /// This Function Used To Get all Material UOM For Dispersion Master
       /// </summary>
       /// <param name="materialPK"></param>
       /// <param name="status"></param>
       /// <returns>DataTable</returns>
       public static DataTable GetItemUOMTrading(int materialPK)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_ITM_PK  , materialPK)  
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALUOMTRD, colParameters).Tables[0];
       }
       #endregion

       #region External Material Issue Multiple
       public static DataSet GetEMIMultipleList(GridPrams grid,string pageUrl, string fromDate, string toDate, int trnStatus, int issueStore, int itmCatPK, int itmPk, string issueNo, int issueType, int issueTo, int itemName, int bizUnit, int userPK, int pageIndex, int pageSize)
       //public static DataTable GetEMIMultipleList(string code, string name, int bizUnit, int userPK, int pageIndex, int pageSize)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_BIZUNIT ,bizUnit) , 
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_USER_PK ,userPK) , 
                //new DBService.Parameters(GTIService.Constants.Material.Parameters.P_PAGE_URL ,"/Inventory/MaterialIssue.aspx") ,
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  pageUrl==string.Empty ?(object)DBNull.Value:pageUrl),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.TRANSACTIONTYPE , 5),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
                // //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ||grid.SortBy == "PRH_DATE"|| grid.SortBy == "PRH_NO" ? GTIService.Constants.PurchaseRequest.Fields.PURCHASERQSTPK: grid.SortBy),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy==null? "PRH_DATE": grid.SortBy),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_FROM_DT, fromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(fromDate)),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.P_TO_DT, toDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(toDate)),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_NO , issueNo==string.Empty?(object)DBNull.Value: issueNo), //Issue No     
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_STATUS , trnStatus<0 ? (object)DBNull.Value:trnStatus ), //Status    
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ISS_RCV_TYPE , issueType>0? issueType :(object)DBNull.Value), //Issue Against     
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ISS_RCV_SUB_TYPE , issueTo>0?  issueTo:(object)DBNull.Value),   //Type
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_DEPT , issueStore>0?  issueStore:(object)DBNull.Value), //Issuing Store     
                //new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_LOT_NO ,lotNo== string.Empty ? (object)DBNull.Value : lotNo),  
                //new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_REF_NO ,refNo== string.Empty ? (object)DBNull.Value :  refNo),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ITEM_CATEGORY , itmCatPK>0?  itmCatPK:(object)DBNull.Value),  //Item Category   
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ITEM, itmPk>0?  itmPk:(object)DBNull.Value),  //Item
                new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_ISS_RCV_PK, itemName>0?  itemName:(object)DBNull.Value),  //Item Name
                // new DBService.Parameters(GTIService.Constants.Store.Parameters_ExternalMaterialIssue.P_ICH_COMPANY, cmpPk>0?  cmpPk:(object)DBNull.Value)
            };
           //return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_ITEM_EXT_ISS_GET_LIST, colParameters).Tables[1];

           DataSet dsPurchaseRqstList = new DataSet();
           dsPurchaseRqstList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_ITEM_EXT_ISS_GET_LIST, colParameters);
           return dsPurchaseRqstList;
       }
       #endregion
   }
}
