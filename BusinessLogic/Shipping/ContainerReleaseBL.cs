using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.POInvoicing;
using GTIService;
using DataAccess.POInvoicing;
using BusinessObject;
using System.Data;
using BusinessObject.Shipping;
using DataAccess.Shipping;
using BusinessObject.CommonManagement;

namespace BusinessLogic.Shipping
{
    public class ContainerReleaseBL
    {
        /// <summary>
        /// Get PO Invoice Details
        /// </summary>
        /// <param name="poPK"></param>
        /// <param name="invPK"></param>
        /// <returns></returns>
        public static DataTable GetContainerReleaseHeader(int pcrhPK, int Active, int BizUnit)
        {
            try
            {
                ContainerReleaseHeader invoiceHeaderObj = new ContainerReleaseHeader();
                DataTable ContainerRelease = ContainerReleaseDL.GetContainerReleaseHeader(pcrhPK, Active, BizUnit);
                if (ContainerRelease != null)
                {
                    return ContainerRelease;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Save PO Invoice
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int? SaveContainerReleaseHeader(ContainerReleaseHdr ContainerReleaseHdrObj)
        {
            return ContainerReleaseDL.SaveContainerReleaseHeader(ContainerReleaseHdrObj);
        }

        /// <summary>
        /// Get DO Details Details
        /// </summary>
        /// <param name="shipPlanPk"></param>
        /// <returns>string</returns>
        public static string GetDODetails(int shipPlanPk)
        {
            return ContainerReleaseDL.GetDODetails(shipPlanPk);
        }

        /// <summary>
        /// Get Pallete Details for Auto Complete
        /// </summary>
        /// <param name="fieldName">string</param>
        /// <param name="value">string</param>
        /// <param name="scID">int</param>
        /// <param name="BrandID">int</param>
        /// <param name="sbu">int</param>
        /// <returns>List<AutoCompleteBO></returns>
        public static List<AutoCompleteBO> GetPalleteAutoComplete(string fieldName, string value, int scID, int BrandID, int sbu)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            DataTable dtSearch = ContainerReleaseDL.GetPalleteAutoComplete(fieldName, value, scID, BrandID, sbu);
            result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
            {
                Key = row.Field<int>(GTIService.Constants.Common.Fields.PK),
                Name = row.Field<string>(GTIService.Constants.Common.Fields.VALUE)
            }).ToList();

            return result;
        }

        /// <summary>
        /// AddCartonToList
        /// </summary>
        /// <param name="soDetailId">int</param>
        /// <param name="cartons">string</param>
        /// <param name="palleteId">int</param>
        /// <param name="locationId">int</param>        
        /// <returns>DataTable</returns>
        public static DataTable AddCartonToList(int soDetailId, string cartons, int palleteId, int locationId, string cartonPrefix, int brandPk, int currentCDRPk,string palletNo="")
        {
            DataTable dtResult = ContainerReleaseDL.AddCartonToList(soDetailId, cartons, palleteId, locationId, cartonPrefix, brandPk, currentCDRPk,palletNo);
            return dtResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SohPK"></param>
        /// <param name="cartonsList"></param>
        /// <param name="cartonPrefix"></param>
        /// <param name="locationId"></param>
        /// <param name="brandPk"></param>
        /// <param name="currentDPDPk"></param>
        /// <returns></returns>
        public static DataTable AddCartonToDOList(string cartonsList, string cartonPrefix, int locationId, int brandPk, int currentDPDPk)
        {
            DataTable dtResult = ContainerReleaseDL.AddCartonToDOList( cartonsList, cartonPrefix, locationId, brandPk, currentDPDPk);
            return dtResult;
        }

        ///// <summary>
        ///// Get Carton Details Details
        ///// </summary>
        ///// <param name="shipPlanPk"></param>
        ///// <returns>DataTable</returns>
        //public static DataTable GetCartonDetails(int shipPlanPk)
        //{
        //    return ContainerReleaseDL.GetCartonDetails(shipPlanPk);
        //}

        public static DataTable AutoAllocateCartonDetails(int brandPK, decimal Qty, int sohPk, int cdrPk, decimal DQQtyPcs = 0)
        {
            DataTable dtResult = ContainerReleaseDL.AutoAllocateCartonDetails(brandPK, Qty, sohPk, cdrPk, DQQtyPcs);
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
            DataTable dtResult = ContainerReleaseDL.AutoAllocateCartonDODetails(brandPK, Qty, dpdPK);
            return dtResult;
        }
       
    }
}
