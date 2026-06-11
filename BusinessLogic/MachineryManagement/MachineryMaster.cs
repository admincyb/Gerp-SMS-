
using System.Data;
using BusinessObject;

namespace BusinessLogic.MachineryManagement
{
    /// <summary>
    /// Class Used for Access all Machinery  releted function
    /// </summary>
    public class MachineryMaster
    {
        #region Methods

        //============================================= Machine Type       ==========================================================================
        /// <summary>
        /// Get Machine Type Details By MachineTypeID
        /// </summary>
        /// <param name="machineTypeID"></param>
        /// <returns>string</returns>
        public static string GetMachineType(int sBU)
        {
            DataTable dtSearch = DataAccess.MachineryManagement.MachineryMasterDL.GetMachineType(sBU);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Machinery.Fields.MACHINETYPENAME, GTIService.Constants.Machinery.Fields.MACHINETYPEPK).ToString();

        }
        /// <summary>
        /// Save machine Type Details
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="userPk"></param>
        /// <returns>string</returns>
        public static string SaveMachineType(string requestData, int userPk, int sBU)
        {
            BusinessObject.MachineryManagement.Machinery.MachineTypeMaster machineType = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.MachineryManagement.Machinery.MachineTypeMaster>(requestData);
            machineType.UserPK = userPk;
            machineType.SBU = sBU;
            return (DataAccess.MachineryManagement.MachineryMasterDL.SaveMachineType(machineType)).ToString();
        }
        /// <summary>
        /// Delete Machine Type Details By Machine Type ID
        /// </summary>
        /// <param name="machineTypeID"></param>
        /// <returns>string</returns>
        public static string DeleteMachineTypeDtls(int machineTypeID)
        {
            return DataAccess.MachineryManagement.MachineryMasterDL.DeleteMachineTypeDtls(machineTypeID).ToString();
        }

        /// <summary>
        /// Get Machine Details - For Listing
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetMachineTypeDtls(GridPrams grid, int sBU)
        {
            DataSet dsMachineTypeList = DataAccess.MachineryManagement.MachineryMasterDL.GetMachineTypeDtls(grid, sBU);
            string jString = string.Empty;
            if (dsMachineTypeList.Tables.Count > 1 && dsMachineTypeList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsMachineTypeList);
            }
            return jString;
        }

        //============================================ Fill Drip Down =================================================
        /// <summary>
        ///  Get RunByDtls
        /// </summary>
        /// <param name="runByDtlsID"></param>
        /// <returns>string</returns>
        public static string GetRunByDtls(int sBU)
        {
            DataTable dtSearch = DataAccess.MachineryManagement.MachineryMasterDL.GetRunByDtls(sBU);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Machinery.Fields.RUNBYNAME, GTIService.Constants.Machinery.Fields.RUNBYPK).ToString();

        }

        /// <summary>
        /// Get Mode of Purchase
        /// </summary>
        /// <param name="freequncyType"></param>
        /// <returns>String</returns>
        public static string GetFrequencyType(int sBU)
        {
            DataTable dtSearch = DataAccess.MachineryManagement.MachineryMasterDL.GetFrequencyType(sBU);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch,GTIService.Constants.Machinery.Fields.FREEQUENCYNAME, GTIService.Constants.Machinery.Fields.FREEQUENCYPK ).ToString();

        }

        public static string GetMeasuringType(int sBU, int Active, string CfgType)
        {
            DataTable dtMeasuringType = DataAccess.MachineryManagement.MachineryMasterDL.GetMeasuringType(sBU, Active, CfgType);
            return GTIService.CommonFunctions.GetTextValueList(dtMeasuringType, "CFG_DATA", "CFG_VALUE").ToString();
        }

        /// <summary>
        /// Get MaintenaceType Details
        /// </summary>
        /// <param name="maintenceType"></param>
        /// <returns>String</returns>
        public static string GetMainteanceType(int sBU)
        {
            DataTable dtSearch = DataAccess.MachineryManagement.MachineryMasterDL.GetMainteanceType( sBU);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch,GTIService.Constants.Machinery.Fields.MIANTENACETYPENAME, GTIService.Constants.Machinery.Fields.MIANTENACETYPEPK ).ToString();

        }
       

        //================================================ Location =========================================

        /// <summary>
        /// Get Location Details By locationID 
        /// </summary>
        /// <param name="locationID"></param>
        /// <returns>string</returns>
        public static string GetLocation(int sbuPK)
        {
            DataTable dtSearch = DataAccess.MachineryManagement.MachineryMasterDL.GetLocation(sbuPK);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Machinery.Fields.LOCNAME, GTIService.Constants.Machinery.Fields.LOCPK).ToString();

        }
        /// <returns>string</returns>
        public static string GetLocationList(GridPrams grid, int sbuPk)
        {

            DataSet dsLocationList = DataAccess.MachineryManagement.MachineryMasterDL.GetLocationList(grid, sbuPk);
            string jString = string.Empty;
            if (dsLocationList.Tables.Count > 1 && dsLocationList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsLocationList);
            }
            return jString;
        }
        /// <summary>
        /// Save Location Details
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="userPk"></param>
        /// <returns>string</returns>
        public static string SaveLocation(string requestData, int userPk)
        {
            BusinessObject.MachineryManagement.Machinery.LocationMaster locMaster = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.MachineryManagement.Machinery.LocationMaster>(requestData);
            locMaster.UserPK = userPk;
            return DataAccess.MachineryManagement.MachineryMasterDL.SaveLocation(locMaster).ToString();
        }

        /// <summary>
        /// Delete Location Details By locationID
        /// </summary>
        /// <param name="locationID"></param>
        /// <returns>string</returns>
        public static string DeleteLocationDtls(int locationID)
        {
            return DataAccess.MachineryManagement.MachineryMasterDL.DeleteLocationDtls(locationID).ToString();
        }
        /// <summary>
        /// Get Location Details - For Listing
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetLocationDtls(GridPrams grid)
        {
            DataSet dsLocList = DataAccess.MachineryManagement.MachineryMasterDL.GetLocationDtls(grid);
            string jString = string.Empty;
            if (dsLocList.Tables.Count > 1 && dsLocList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsLocList);
            }
            return jString;
        }
        
       //================================================= Machine ========================================================================================

        /// <summary>
        /// Save Machine Details 
        /// </summary>
        /// <param name="machineDetails"></param>
        /// <returns>string</returns>
        public static string SaveMachineDetails(string machineDetails)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(machineDetails);
            int machinePk = DataAccess.MachineryManagement.MachineryMasterDL.SaveMachineDetails(xmlstr);
            if (machinePk != 0)
            {
                //used to update the files to Permanenet location
                BusinessObject.CommonManagement.CommonObject.File fileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(machineDetails);
                for (int i = 0; i < fileObject.FILELIST.Count; i++)
                {
                    if (fileObject.FILELIST[i].DOC_PK == 0)
                    {
                        if (fileObject.FILELIST[i].DOC_TITLE != string.Empty)
                        {
                            if (CommonManagement.CommonManagement.UploadFromTempFolderForXml(fileObject.FILELIST[i].DOC_NAME, GTIService.Constants.Machinery.Fields.UPLOADFOLDER))
                            {

                            }
                        }
                    }
                }
            }

            return machinePk.ToString() ;

        }
       
        /// <summary>
        /// Get Machine List - For Paging
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetMachineList(GridPrams grid, int sBU, int StatusPK)
        {
            DataSet dsMachineList = DataAccess.MachineryManagement.MachineryMasterDL.GetMachineList(grid, sBU, StatusPK);
            string jString = string.Empty;
            if (dsMachineList.Tables.Count > 1 && dsMachineList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsMachineList);
            }
            return jString;
        }
        
        /// <summary>
        /// Get Machine Details By MachineID
        /// </summary>
        /// <param name="machineID"></param>
        /// <returns>string</returns>
        public static string GetMachineDetails(int machineID)
        {            
            return GTIService.CommonFunctions.XmlToJson(DataAccess.MachineryManagement.MachineryMasterDL.GetMachineDetails(machineID)).ToString();
        }
        /// <summary>
        /// Delete Machine Details By machineID
        /// </summary>
        /// <param name="machineID"></param>
        /// <returns>string</returns>
        public static string DeleteMachineDtls(int machineID)
        {
            return DataAccess.MachineryManagement.MachineryMasterDL.DeleteMachineDtls(machineID).ToString();
        }
        /// <summary>
        /// AutoComplete Search - For machine Details
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchMachineValues(string searchBy, string searchValue, int sBU)
        {
            DataTable dtSearch = DataAccess.MachineryManagement.MachineryMasterDL.GetSearchValues(searchBy, searchValue, sBU);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.VALUE, GTIService.Constants.Common.Fields.PK).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sBU"></param>
        /// <returns></returns>
        public static string GetMachineNameByMachineType(int sBU, int machineType,int? processID=null)
        {
            DataTable dtMachineName = DataAccess.MachineryManagement.MachineryMasterDL.GetMachineNameByNamechineType(sBU, machineType,processID);
            return GTIService.CommonFunctions.GetTextValueList(dtMachineName, GTIService.Constants.Machinery.Fields.MCHNAME, GTIService.Constants.Machinery.Fields.MCHPK).ToString();

        }
        #endregion
       
    }
}
