using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using DataAccess;
using BusinessObject.Inventory;
using GTIService;
using BusinessObject.CommonManagement;

namespace BusinessLogic.Inventory
{
    public class PackingMasterBL
    {
        public static DataSet GetPackingMaster(int apsPK, short active, int bizUnit, string name, string type, string code, string sortby="")
        {
            return DataAccess.Inventory.PackingMasterDA.GetPackingMaster(apsPK, active, bizUnit,name,type,code,sortby);
        }
        public static int? SavePackingMaster(PackingMasterBO packMasterBo)
        {
            return DataAccess.Inventory.PackingMasterDA.SavePackingMaster(packMasterBo);
        }
        public static int DeletePackingMaster(int apsPK, DateTime lastModDate)
        {
            return DataAccess.Inventory.PackingMasterDA.DeletePackingMaster(apsPK, lastModDate);
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
            return DataAccess.Inventory.PackingMasterDA.GetPackingMappingList(pimPk, active, bizunit, packingPk, CusPk, BrandPk, Artwork, isMapped, packSpecCode);
        }
        /// <summary>
        /// Save Packing Mapping Details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? SavePackingMapping(Packingmapping obj)
        {
            return DataAccess.Inventory.PackingMasterDA.SavePackingMapping(obj);
        }

        /// <summary>
        /// Delete Packing Mapping
        /// </summary>
        /// <param name="pPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeletePackingMapping(int pPK, DateTime lastModDate)
        {
            return DataAccess.Inventory.PackingMasterDA.DeletePackingMapping(pPK, lastModDate);
        }

        /// <summary>
        /// Methord to get the Packing Mapping List (Dynamic)
        /// </summary>
        /// <param name="bmdPk"></param>
        /// <param name="pimPk"></param>
        /// <param name="active"></param>
        /// <param name="bizunit"></param>        
        /// <returns>DataTable</returns>
        public static DataTable GetPackingMappingListDynamic(int bmdPk, int active, int pimPk, int bizunit)
        {
            return DataAccess.Inventory.PackingMasterDA.GetPackingMappingListDynamic(bmdPk, active,pimPk, bizunit);
        }
        public static long SavePackingSpec(int brandPk,int packingSpecPk)
        {
            return DataAccess.Inventory.PackingMasterDA.SavePackingSpec(brandPk, packingSpecPk);
        }

        public static int? ActivateArtwork(int packingSpecPk, int UserPk)
        {
            return DataAccess.Inventory.PackingMasterDA.ActivateArtwork(packingSpecPk, UserPk);
        }

        /// <summary>
        /// To Save Brand Product
        /// </summary>
        /// <param name="Product"></param>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static int BrandProductSpecSave(int ItemPK, int UserPk)
        {
            return DataAccess.Inventory.PackingMasterDA.BrandProductSpecSave(ItemPK, UserPk);
           // return 0;
        }
    }
}
