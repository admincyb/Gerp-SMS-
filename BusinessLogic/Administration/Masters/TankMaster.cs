using System.Data;
using BusinessObject;

using DataAccess.MaterialManagement;

namespace BusinessLogic.Administration.Masters
{
   public class TankMaster
    {
        /// <summary>
        ///  Function Used To Get all Tank Type from sbuPk
        /// </summary>
        /// <param name="sbuPk"></param>
       
        /// <returns>string</returns>
        public static string GetTankType(int sbuPk)
        {
            DataTable dtTankType = DataAccess.Administration.Masters.TankMasterDL.GetTankType( sbuPk);
            string jString = string.Empty;
            if (dtTankType.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtTankType,GTIService.Constants.Tank.Fields.TANKNAME, GTIService.Constants.Tank.Fields.TANKPK );

            }
            return jString;
        }
        /// <returns>string</returns>
        public static string GetTankTypeList(GridPrams grid, int sbuPk)
        {

            DataSet dsTankTypeList = DataAccess.Administration.Masters.TankMasterDL.GetTankTypeList(grid,sbuPk);
            string jString = string.Empty;
            if (dsTankTypeList.Tables.Count > 1 && dsTankTypeList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsTankTypeList);
            }
            return jString;
        }
        /// <summary>
        /// Delete Tank Details
        /// </summary>
        /// <param name="tankTypeId"></param>
        /// <returns>String</returns>
        public static string DeleteTankType(int tankTypeId)
        {
            return DataAccess.Administration.Masters.TankMasterDL.DeleteTankType(tankTypeId).ToString();
        }
        /// <summary>
        /// Saving Tank details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>string</returns>
        public static string SaveTankType(string requestData)
        {
            BusinessObject.Administration.Masters.TankType tank = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Administration.Masters.TankType>(requestData);
            return DataAccess.Administration.Masters.TankMasterDL.SaveTankType(tank);
        }
        /// <summary>
        /// Returns Tankmaster details list in json string format
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetTankMasterList(GridPrams grid, int bizUnit, int statusPk)
        {
            DataSet dsTankMasterList = DataAccess.Administration.Masters.TankMasterDL.GetTankMasterList(grid, bizUnit, statusPk);
            string jString = string.Empty;
            if (dsTankMasterList.Tables.Count > 1 && dsTankMasterList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsTankMasterList);
            }
            return jString;
        }
        /// <summary>
        /// Saving Tank master
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>string</returns>
        public static string SaveTankMaster(string requestData)
        {
            BusinessObject.Administration.Masters.TankMaster tank = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Administration.Masters.TankMaster>(requestData);
            //var xxx= Newtonsoft.Json.JsonConvert.DeserializeXmlNode(tank.TankDetails.ToString(), "root");
            string xmlstr= ERP.Utilities.CommonFunctions.XmlSerialize(tank);
            //string xmlstr = GTIService.CommonFunctions.JsonToXml(tank.TankDetails.ToString());
            return DataAccess.Administration.Masters.TankMasterDL.SaveTankMaster(tank,xmlstr);
        }
        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
       
        /// <returns>string</returns>
        public static string GetTankMasterSearchValues(string searchValue, string searchBy,int sbuPK)
        {
            DataTable dtSearch = DataAccess.Administration.Masters.TankMasterDL.GetTankMasterSearchValues(searchValue, searchBy, sbuPK);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);

        }
        /// <summary>
        /// Delete Tank Master Details
        /// </summary>
        /// <param name="tankMasterId"></param>
        /// <returns>String</returns>
        public static string DeleteTankMaster(int tankMasterId,string lastModDate)
        {
            return DataAccess.Administration.Masters.TankMasterDL.DeleteTankMasterDtls(tankMasterId, lastModDate).ToString();
        }

       /// <summary>
        /// Get TankName By Tank Type
       /// </summary>
       /// <param name="sbuPk"></param>
       /// <param name="typePK"></param>
       /// <returns></returns>

        public static string GetTankName(int sbuPk, int typePK)
        {
            DataTable dtTankType = DataAccess.Administration.Masters.TankMasterDL.GetTankName(sbuPk, typePK);
            string jString = string.Empty;
            if (dtTankType.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtTankType, "TNK_NAME", "TNK_PK");

            }
            return jString;
        }

           /// <summary>
       /// To get Lines from LineMaster
       /// </summary>
       /// <param name="bizUnit"></param>
       /// <returns></returns>
        public static string GetAllLine(int bizunit, int LinePk = 0, int Status = 1, int virtualLine=-1)
        {
            DataTable dtLine = DataAccess.Administration.Masters.TankMasterDL.GetAllLine(bizunit, LinePk, Status, virtualLine);            
            string jString = string.Empty;
            if (dtLine.Rows.Count > 0)
            {
                jString = jString = GTIService.CommonFunctions.GetTextValueList(dtLine, GTIService.Constants.Tank.Fields.LINECODE, GTIService.Constants.Tank.Fields.LINEPK);

            }
            return jString;
        }
    }
}
