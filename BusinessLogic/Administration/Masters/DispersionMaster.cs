using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using BusinessObject;
using DataAccess.DispersionManagement;
using GTIService.Constants.Dispersion;
using System.Web;

namespace BusinessLogic.DispersionManagement
{
    public class DispersionMaster
    {
        /// <summary>
        /// Save Dispersion Details - Both Header And Dispersion Material Details
        /// </summary>
        /// <param name="DispersionMaster"></param>
        /// <returns>DispersionPk 0-if duplicate entry</returns>
        public static string SaveDispersionDetails(string DispersionDetails)
        {
            //convert json data to xml string
            string xmlstr = GTIService.CommonFunctions.JsonToXml(DispersionDetails);
            return DispersionMasterDL.SaveDispersionDetails(xmlstr).ToString();
        }

        /// <summary>
        /// Get list of dispersion to fill the grid
        /// </summary>
        /// <param name="grid">Grid parameters</param>
        /// <returns></returns>
        public static string GetDispersionList(GridPrams grid, int bizUnit)
        {
            //get dispersion list
            DataSet dsDispersionList = DataAccess.DispersionManagement.DispersionMasterDL.GetDispersionList(grid, bizUnit);
            string jString = string.Empty;
            if (dsDispersionList.Tables.Count > 1 && dsDispersionList.Tables[1].Rows.Count > 0)
            {
                //convert o tjson string
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsDispersionList);
            }
            return jString;
        }


        /// <summary>
        /// Get list of dispersion to fill the dropdown
        /// <param name="grid"></param>
        /// <returns></returns>
        public static string GetDispersionListCombo(int bizUnit)
        {
            //get dispersion list
            DataTable dsDispersionList = DataAccess.DispersionManagement.DispersionMasterDL.GetDispersionForDropdown(bizUnit);
            string jString = string.Empty;
            //convert to json string
            jString = GTIService.CommonFunctions.GetTextValueList(dsDispersionList, Fields.DISPERSIONNAME, Fields.DISPERSIONPK);

            return jString;
        }

        /// <summary>
        /// Get Dispersion Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns></returns>
        public static string GetDispersionDetails(int DispersionID, int dept)
        {
            try
            {

                BusinessObject.DispersionManagement.Dispersion obj = new BusinessObject.DispersionManagement.Dispersion();
                //get dispersion detail in xml format convert to json
                return GTIService.CommonFunctions.XmlToJson(DispersionMasterDL.GetDispersionDetails(DispersionID, dept));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Dispersion Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Delete Dispersion Details
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns>String -status</returns>
        public static string DeleteDispersion(int DispersionID)
        {
            return DispersionMasterDL.DeleteDispersionDtls(DispersionID).ToString();
        }

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static string GetSearchValues(string searchBy, string searchValue, int bizUnit)
        {
            DataTable dtSearch = DataAccess.DispersionManagement.DispersionMasterDL.GetSearchValues(searchBy, searchValue, bizUnit);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtSearch, Fields.SEARCHVALUE, Fields.SEARCHPK);
            return jString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetDispersionForAuto(string searchValue, User objUser, int dept = 0)
        {
            DataTable dtSearch = DataAccess.DispersionManagement.DispersionMasterDL.GetDispersionsForAuto(searchValue, objUser, dept);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);
        }

        /// <summary>
        /// Get list of dispersion types to fill the dropdown
        /// <param name="grid"></param>
        /// <returns></returns>
        public static string GetDispersionTypes(int cfgPK, string cfgType, int active, int bizUnit)
        {
            //get dispersion list
            DataTable dsDispersionTypes = DataAccess.DispersionManagement.DispersionMasterDL.GetDispersionTypes(cfgPK, cfgType, active, bizUnit);
            string jString = string.Empty;
            //convert to json string
            jString = GTIService.CommonFunctions.GetTextValueList(dsDispersionTypes, Fields.DISPTYPENAME, Fields.DISTPTYPEPK);

            return jString;
        }    
    }
}
