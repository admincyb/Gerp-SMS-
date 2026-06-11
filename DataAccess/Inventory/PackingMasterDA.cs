using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.Inventory;
using BusinessObject.CommonManagement;
using GTIService.Constants.Common;

namespace DataAccess.Inventory
{
    public class PackingMasterDA
    {
        public static DataSet GetPackingMaster(int apsPK, short active, int bizUnit, string name, string type, string code, string sortby = "")
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_PK,  apsPK==0?(object)DBNull.Value:apsPK),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ACTIVE,  active),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_BIZUNIT,  bizUnit),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_NAME,  name==string.Empty?(object)DBNull.Value:name),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_TYPE, (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_CON_NAME,  type==string.Empty?(object)DBNull.Value:type),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_CODE,  code==string.Empty?(object)DBNull.Value:code),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_SORT_BY,  sortby==string.Empty?(object)DBNull.Value:sortby)
                                          
            };

            DataSet dsPackingMaster = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPADM_PACK_SPEC_MST_GET_KV, colParameters);
            return dsPackingMaster;

        }
        public static int? SavePackingMaster(PackingMasterBO packMasterBo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_PK, packMasterBo.APS_PK),  
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_CODE,packMasterBo.APS_CODE), 
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_NAME, packMasterBo.APS_NAME), 
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_TYPE, packMasterBo.APS_TYPE),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_PC_PCS, packMasterBo.APS_PC_PCS),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_IB_PCS, packMasterBo.APS_IB_PCS),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_IC_PCS, packMasterBo.APS_IC_PCS),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_ZB_PCS, packMasterBo.APS_ZB_PCS),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_MC_PCS, packMasterBo.APS_MC_PCS),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_SC_PCS, packMasterBo.APS_SC_PCS),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_POB_PCS, packMasterBo.APS_POB_PCS),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_PRB_PCS, packMasterBo.APS_PRB_PCS),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_WLT_PCS, packMasterBo.APS_WLT_PCS),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_TOTAL_PCS, packMasterBo.APS_TOTAL_PCS),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_DESC, packMasterBo.APS_DESC), 
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ACTIVE, packMasterBo.ACTIVE), 
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_USER_PK, packMasterBo.USER_PK), 
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_BIZUNIT, packMasterBo.BIZUNIT), 
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_LAST_MOD_DT, packMasterBo.LAST_MOD_DT), 
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPADM_PACK_SPEC_MST_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Inventory.Parameters.P_RET_VAL]).Value);
            return result;

        }
        public static int DeletePackingMaster(int apsPK, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_PK, apsPK),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_LAST_MOD_DT, (object)DBNull.Value), 
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPADM_PACK_SPEC_MST_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }


        /// <summary>
        /// Methord to get the Packing Mapping List
        /// </summary>
        /// <param name="pimPk"></param>
        /// <param name="active"></param>
        /// <param name="bizunit"></param>
        /// <param name="packingPk"></param>
        /// <param name="CusPk"></param>
        /// <param name="BrandPk"></param>
        /// <param name="Artwork"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetPackingMappingList(int pimPk, int active, int bizunit, int packingPk, int CusPk, int BrandPk, string Artwork, bool? isMapped = null, string packSpecCode = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_PIM_PK , pimPk),
              new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_ACTIVE ,  active!=-1 ? active:(object)DBNull.Value ),
              new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_BIZUNIT ,  bizunit),
              new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_PACK_SPEC ,  packingPk>0 ?packingPk:(object)DBNull.Value),
              new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_CUSTOMER ,  CusPk>0 ?CusPk:(object)DBNull.Value),
              new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_CUST_ITEM ,   BrandPk>0 ?BrandPk:(object)DBNull.Value),
              new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_ART_WORK , Artwork),
              new DBService.Parameters(GTIService.Constants.Inventory.Parameters.IS_MAPPED , isMapped.HasValue ? isMapped.Value : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_APS_CODE , packSpecCode !=null?packSpecCode : (object)DBNull.Value)
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPADM_PACK_CUST_ITEM_MAP_GET_KV, colParameters).Tables[0];
            return dtSearchValue;

        }

        /// <summary>
        /// Methord to get the Packing Mapping List (Dynamic)
        /// </summary>
        /// <param name="bmdPk"></param>
        /// <param name="active"></param>
        /// <param name="pimPk"></param>
        /// <param name="bizunit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetPackingMappingListDynamic(int bmdPk, int active, int pimPk, int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_BMD_PK , bmdPk),
              new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_PIM_PK , pimPk),
              new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_ACTIVE ,  active!=-1 ? active:(object)DBNull.Value ),
              new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_BIZUNIT ,  bizunit)
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPADM_PACK_CUST_ITEM_MAP_DTL_GET_KV, colParameters).Tables[0];
            return dtSearchValue;
        }


        /// <summary>
        /// Save Packing Mapping Details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? SavePackingMapping(Packingmapping obj)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_PIM_PK , obj.P_PIM_PK ),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_PACK_SPEC , obj.PIM_PACK_SPEC ),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_CUSTOMER	 , obj.PIM_CUSTOMER	),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_CUST_ITEM , obj.PIM_CUST_ITEM	),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_ART_WORK , obj.PIM_ART_WORK),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_DESC	 , obj.PIM_DESC	),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_PC_ITEM , obj.PIM_PC_ITEM >0?obj.PIM_PC_ITEM:(object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_IB_ITEM , obj.PIM_IB_ITEM >0?obj.PIM_IB_ITEM:(object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_IC_ITEM , obj.PIM_IC_ITEM >0?obj.PIM_IC_ITEM:(object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_ZB_ITEM	 , obj.PIM_ZB_ITEM	 >0?obj.PIM_ZB_ITEM	:(object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_MC_ITEM	 , obj.PIM_MC_ITEM	 >0?obj.PIM_MC_ITEM	:(object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_SC_ITEM, obj.PIM_SC_ITEM >0?obj.PIM_SC_ITEM:(object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_ACTIVE , obj.P_ACTIVE),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_BIZUNIT , obj.P_BIZUNIT),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_LAST_MOD_DT , obj.P_LAST_MOD_DT),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_USER_PK	 , obj.P_USER_PK	),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_WLT_ITEM, obj.PIM_WLT_ITEM >0?obj.PIM_WLT_ITEM:(object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_PRB_ITEM, obj.PIM_PRB_ITEM >0?obj.PIM_PRB_ITEM:(object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.PIM_POB_ITEM, obj.PIM_POB_ITEM >0?obj.PIM_POB_ITEM:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_DETAILS, !string.IsNullOrEmpty(obj.P_DETAILS)?obj.P_DETAILS : (object)DBNull.Value ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPADM_PACK_CUST_ITEM_MAP_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;

        }


        /// <summary>
        /// Delete Packing Mapping
        /// </summary>
        /// <param name="pPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeletePackingMapping(int pPK, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PIM_PK, pPK),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_LAST_MOD_DT , (object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPADM_PACK_CUST_ITEM_MAP_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }


        public static long SavePackingSpec(int brandPk, int packingSpecPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_CIM_PK ,brandPk >0?brandPk:(object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_APS_PK ,packingSpecPk >0?packingSpecPk:(object)DBNull.Value ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPADM_PACK_SPEC_CHANGE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int? ActivateArtwork(int packingSpecPk, int UserPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_PIM_PK ,packingSpecPk),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_USER_PK ,UserPk ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPADM_PACK_CUST_ITEM_MAP_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int BrandProductSpecSave(int ItemPK, int UserPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_APS_PK ,ItemPK >0?ItemPK:(object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_USER_PK ,UserPk >0?UserPk:(object)DBNull.Value ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPPRD_BRAND_PRODUCT_SPEC_COPY_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
            //return 0;
        }
    }
}
