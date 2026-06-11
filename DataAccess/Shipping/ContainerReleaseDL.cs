using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using ERP.Utilities.Constants.Shipping;
using BusinessObject.Shipping;
using ERP.Utilities;

namespace DataAccess.Shipping
{
    public class ContainerReleaseDL
    {
        /// <summary>
        /// Get Container Release Details
        /// </summary>
        /// <param name="pcrhPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetContainerReleaseHeader(int pcrhPK, int Active, int BizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ContainerRelease.PCRHPK,  pcrhPK == 0 ? (object) DBNull.Value :  0),
                new DBService.Parameters(ContainerRelease.PSNHPK,  pcrhPK == 0 ? (object) DBNull.Value :  pcrhPK),
                new DBService.Parameters(ContainerRelease.PACTIVE,  Active == 0 ? (object) DBNull.Value :  Active),
                new DBService.Parameters(ContainerRelease.PBIZUNIT,  BizUnit == 0 ? (object) DBNull.Value :  BizUnit)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, ContainerRelease.GETCONTAINERRELEASEHDR, colParameters).Tables[0];
            return dtxml;
        }
        /// <summary>
        /// Save PO Invoice
        /// </summary>
        /// <param name="ContainerReleaseHdrObj"></param>
        /// <returns></returns>
        public static int? SaveContainerReleaseHeader(ContainerReleaseHdr ContainerReleaseHdrObj)
        {
            //DBService dbService = new DBService();
            //DBService.Parameters[] colParameters = null;
            //colParameters = new DBService.Parameters[] 
            //{    
            //new DBService.Parameters(ContainerRelease.P_CRH_PK, ContainerReleaseHdrObj.P_CRH_PK),  
            //new DBService.Parameters(ContainerRelease.P_CRH_SHIPPING_PLAN, ContainerReleaseHdrObj.P_CRH_SHIPPING_PLAN),  
            //new DBService.Parameters(ContainerRelease.P_CRH_DATE, ContainerReleaseHdrObj.P_CRH_DATE),  
            //new DBService.Parameters(ContainerRelease.P_CRH_REMARKS, ContainerReleaseHdrObj.P_CRH_REMARKS),  
            //new DBService.Parameters(ContainerRelease.P_CRH_DEPT, ContainerReleaseHdrObj.P_CRH_DEPT),  
            //new DBService.Parameters(ContainerRelease.P_ACTIVE, ContainerReleaseHdrObj.P_ACTIVE),  
            //new DBService.Parameters(ContainerRelease.P_USER_PK, ContainerReleaseHdrObj.P_USER_PK),  
            //new DBService.Parameters(ContainerRelease.P_BIZUNIT, ContainerReleaseHdrObj.P_BIZUNIT),  
            //new DBService.Parameters(ContainerRelease.P_LAST_MOD_DT, ContainerReleaseHdrObj.P_LAST_MOD_DT),  
            //new DBService.Parameters(ContainerRelease.P_CRH_COMPANY, ContainerReleaseHdrObj.P_CRH_COMPANY), 
            //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            //};
            //dbService.ExecuteNonQuery(CommandType.StoredProcedure, ContainerRelease.SPSAL_CONTAINER_RELEASE_SAVE, colParameters);
            //int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            //return result;

            string xmlDoc = CommonFunctions.XmlSerialize(ContainerReleaseHdrObj);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ContainerRelease.SPSAL_CONTAINER_RELEASE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get DO Details Details
        /// </summary>
        /// <param name="shipPlanPk"></param>
        /// <returns>string</returns>
        public static string GetDODetails(int shipPlanPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ContainerRelease.P_DPH_SHIPPING_PLAN,  shipPlanPk == 0 ? (object) DBNull.Value :  shipPlanPk)
            };
            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, ContainerRelease.SPSAL_CONTAINER_RELEASE_GET, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }

        /// <summary>
        /// Get Pallete Details for Auto Complete
        /// </summary>
        /// <param name="fieldName">string</param>
        /// <param name="value">string</param>
        /// <param name="scID">int</param>
        /// <param name="BrandID">int</param>
        /// <param name="sbu">int</param>
        /// <returns>DataTable</returns>
        public static DataTable GetPalleteAutoComplete(string fieldName, string value, int scID, int BrandID, int sbu)
        {
            DataTable dtBincardDtls;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(ContainerRelease.P_FLD_NAME, fieldName),                
                new DBService.Parameters(ContainerRelease.P_VALUE, value),
                new DBService.Parameters(ContainerRelease.P_SOH_PK, scID== 0 ? (object) DBNull.Value :  scID),
                new DBService.Parameters(ContainerRelease.P_BRAND_PK, BrandID== 0 ? (object) DBNull.Value :  BrandID),
                new DBService.Parameters(ContainerRelease.P_BIZUNIT, sbu),

            };
            dtBincardDtls = dbService.DataAdapter(CommandType.StoredProcedure, ContainerRelease.SPSAL_CONTAINER_RELEASE_AUTO, colParameters).Tables[0];
            return dtBincardDtls;
        }

        /// <summary>
        /// AddCartonToList
        /// </summary>
        /// <param name="soDetailId">int</param>
        /// <param name="cartons">string</param>
        /// <param name="palleteId">int</param>
        /// <param name="locationId">int</param>        
        /// <returns>DataTable</returns>
        public static DataTable AddCartonToList(int soDetailId, string cartons, int palleteId, int locationId, string cartonPrefix, int brandPk, int currentCDRPk, string palletNo = "")
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(ContainerRelease.P_SOH_PK, soDetailId == 0 ? (object) DBNull.Value :  soDetailId),
                new DBService.Parameters(ContainerRelease.P_BCR_NO_LIST, cartons == string.Empty ? (object) DBNull.Value :  cartons),
                new DBService.Parameters(ContainerRelease.P_BCR_PALLET, palleteId == 0 ? (object) DBNull.Value :  palleteId),
                new DBService.Parameters(ContainerRelease.P_BCR_LOCATION, locationId == 0 ? (object) DBNull.Value :  locationId),
                new DBService.Parameters(ContainerRelease.P_BCR_NO_PFX, cartonPrefix == string.Empty ? (object) DBNull.Value :  cartonPrefix),
                new DBService.Parameters(ContainerRelease.P_BCR_BRAND, brandPk == 0 ? (object) DBNull.Value :  brandPk),
                new DBService.Parameters(ContainerRelease.P_CDR_PK, currentCDRPk == 0 ? (object) DBNull.Value :  currentCDRPk),
                new DBService.Parameters(ContainerRelease.P_BCR_PALLET_NO, palletNo == string.Empty ? (object) DBNull.Value :  palletNo),
                
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, ContainerRelease.SPSAL_CONTAINER_RELEASE_CARTON_GET, colParameters).Tables[0];
            return dtResult;
        }

        /// <summary>
        /// For get, Add Carton To DO List
        /// </summary>

        /// <param name="cartonsList"></param>
        /// <param name="cartonPrefix"></param>
        /// <param name="locationId"></param>
        /// <param name="brandPk"></param>
        /// <param name="currentDPDPk"></param>
        /// <returns></returns>
        public static DataTable AddCartonToDOList(string cartonsList, string cartonPrefix, int locationId, int brandPk, int currentDPDPk)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              //  new DBService.Parameters(ContainerRelease.P_SOH_PK, SohPK == 0 ? (object) DBNull.Value :  SohPK),
                new DBService.Parameters(ContainerRelease.P_BCR_NO_LIST, cartonsList == string.Empty ? (object) DBNull.Value :  cartonsList),
                new DBService.Parameters(ContainerRelease.P_BCR_NO_PFX, cartonPrefix == string.Empty ? (object) DBNull.Value :  cartonPrefix),
                new DBService.Parameters(ContainerRelease.P_BCR_LOCATION, locationId == 0 ? (object) DBNull.Value :  locationId),
                new DBService.Parameters(ContainerRelease.P_BCR_BRAND, brandPk == 0 ? (object) DBNull.Value :  brandPk),
                new DBService.Parameters(ContainerRelease.P_DPD_PK, currentDPDPk == 0 ? (object) DBNull.Value :  currentDPDPk),
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, ContainerRelease.SPSAL_DESPATCH_CARTON_GET, colParameters).Tables[0];
            return dtResult;
        }


        ///// <summary>
        ///// Get Carton Details Details
        ///// </summary>
        ///// <param name="shipPlanPk"></param>
        ///// <returns>DataTable</returns>
        //public static DataTable GetCartonDetails(int shipPlanPk)
        //{
        //    DBService dbService = new DBService();
        //    DBService.Parameters[] colParameters = null;
        //    colParameters = new DBService.Parameters[] 
        //    {
        //        new DBService.Parameters(ContainerRelease.P_DPH_SHIPPING_PLAN,  shipPlanPk == 0 ? (object) DBNull.Value :  shipPlanPk)
        //    };
        // //   DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, ContainerRelease.SPSAL_CONTAINER_RELEASE_GET, colParameters).Tables[0];
        //    return dtResult;
        //}

        //
        public static DataTable AutoAllocateCartonDetails(int brandPK, decimal Qty, int sohPk, int cdrPk, decimal DQQtyPcs = 0)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(ContainerRelease.P_BCR_BRAND, brandPK == 0 ? (object) DBNull.Value :  brandPK),
                new DBService.Parameters(ContainerRelease.P_BCR_QTY, Qty),
                new DBService.Parameters(ContainerRelease.P_SOH_PK, sohPk == 0 ? (object) DBNull.Value :  sohPk),
                new DBService.Parameters(ContainerRelease.P_CDR_PK, cdrPk == 0 ? (object) DBNull.Value :  cdrPk),
                new DBService.Parameters(ContainerRelease.P_DO_QTY, DQQtyPcs)
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, ContainerRelease.SPSAL_CONTAINER_RELEASE_CARTON_AUTO_GET, colParameters).Tables[0];
            return dtResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandPK"></param>
        /// <param name="Qty"></param>
        /// <param name="dpdPK"></param>
        /// <returns></returns>
        public static DataTable AutoAllocateCartonDODetails(int brandPK, decimal Qty, int dpdPK)
        {
            DataTable dtResult = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(ContainerRelease.P_BCR_BRAND, brandPK == 0 ? (object) DBNull.Value :  brandPK),
                new DBService.Parameters(ContainerRelease.P_BCR_QTY, Qty),
                new DBService.Parameters(ContainerRelease.P_DPD_PK, dpdPK == 0 ? (object) DBNull.Value :  dpdPK),
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, ContainerRelease.SPSAL_DESPATCH_CARTON_AUTO_GET, colParameters).Tables[0];
            return dtResult;
        }
       
    }
}
